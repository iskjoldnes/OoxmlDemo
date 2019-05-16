using AutoMapper;
using OoxmlDemo.DocumentProcessing.Dto;
using OoxmlDemo.ExternalServices.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing
{
    public abstract class PackageContainer
    {
        public PackageContainer(string path) {
            Path = path;
        }

        public string Path { get; protected set; }

        protected PackagePartBase[] pkgParts { get; set; }
        protected PkgProperties PackageProperties { get; set; }
        protected PkgRelationship[] PackageRelations { get; set; }
        protected PackagePartDocument ContentPart { get; set; }
        protected PackagePartDocument StylesPart { get; set; }
        protected PackagePartDocument NumberingPart { get; set; }

        protected abstract Task UpdateDocumentAsync();

        /// <summary>
        /// Expanding the template document.
        /// </summary>
        /// <returns>New document</returns>
        public virtual async Task<ImageFile> ExpandDocumentAsync()
        {

            await Initialize();

            await UpdateDocumentAsync();

            return await GenerateDocument();
        }

        private async Task Initialize()
        {
            using (var storage = new MemoryStream())
            {
                using (var fileStream = File.OpenRead(Path))
                {
                    await fileStream.CopyToAsync(storage).ConfigureAwait(true);
                    fileStream.Close();
                }
                var templateZipPackage = Package.Open(storage, FileMode.Open, FileAccess.Read);

                PackageProperties = Mapper.Map<PkgProperties>(templateZipPackage.PackageProperties);
                PackageRelations = templateZipPackage.GetRelationships().Select( rel => MapRelationship(rel)).ToArray();

                pkgParts = await Task.WhenAll(templateZipPackage.GetParts()
                    .Where(p => !p.Uri.ToString().Contains("/_rels/"))
                    .Cast<ZipPackagePart>()
                    .Select(p => PackagePartBase.CreateAsync(p))
                    .ToArray());

                ContentPart = pkgParts.OfType<PackagePartDocument>().Single( p => p.PartUri.ToString() == "/word/document.xml");
                StylesPart = pkgParts.OfType<PackagePartDocument>().Single(p => p.PartUri.ToString() == "/word/styles.xml");
                NumberingPart = pkgParts.OfType<PackagePartDocument>().SingleOrDefault(p => p.PartUri.ToString() == "/word/numbering.xml");
            }

        }

        private async Task<ImageFile> GenerateDocument()
        {
            List<PackagePartBase> partsWithRelationships = new List<PackagePartBase>();
            var destinationStorage = new MemoryStream();
            var destinationZipPackage = System.IO.Packaging.Package.Open(destinationStorage, FileMode.Create);
            foreach (var part in pkgParts)
            {
                if (part.HasRelationships)
                {
                    partsWithRelationships.Add(part);
                }
                else
                {
                    await CopyPartAsync(part, destinationZipPackage);
                }
            }
            foreach (var partsWithRelationship in partsWithRelationships)
            {
                await CopyPartAsync(partsWithRelationship, destinationZipPackage);
            }

            // Copy packet root relationships
            CopyRelationships(PackageRelations, destinationZipPackage);

            // Copy packet properties
            Mapper.Map<PkgProperties, PackageProperties>(PackageProperties, destinationZipPackage.PackageProperties);


            destinationZipPackage.Flush();
            destinationZipPackage.Close();

            return new ImageFile
            {
                Content = destinationStorage.ToArray(),
                ContentType = "application/vnd.openxmlformats-officedocument.wordprocessingml.document"
            };
        }

        private ZipPackagePart CopyPart(ZipPackagePart part, Package destinationPackage, string newContents = null)
        {
            var newPart = (ZipPackagePart)destinationPackage.CreatePart(part.Uri, part.ContentType, part.CompressionOption);
            using (var filestream = newPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                if( newContents != null)
                {
                    using (var streamWriter = new StreamWriter(filestream))
                    {
                        streamWriter.Write(newContents);
                        streamWriter.Flush();
                    }
                }
                else
                {
                    using (var sourceFilestream = part.GetStream(FileMode.Open, FileAccess.Read))
                    {
                        sourceFilestream.CopyTo(filestream);
                    }
                }
            }
            CopyProperties( part, newPart);

            return newPart;
        }

        private async Task<ZipPackagePart> CopyPartAsync(PackagePartBase part, Package destinationPackage)
        {
            var newPart = (ZipPackagePart)destinationPackage.CreatePart(part.PartUri, part.ContentType, part.CompressionOption);
            using (var filestream = newPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                await part.WriteContentAsync(filestream);
            }
            part.WriteRelationships(newPart);

            return newPart;
        }

        
        private void CopyProperties(ZipPackagePart sourcePart, ZipPackagePart destinationPart)
        {
            foreach (var rel in sourcePart.GetRelationships())
            {
                destinationPart.CreateRelationship(rel.TargetUri, rel.TargetMode, rel.RelationshipType, rel.Id);
            }
        }

        public virtual PkgRelationship MapRelationship(PackageRelationship relationship)
        {
            return new PkgRelationship()
            {
                Id = relationship.Id,
                RelationshipType = relationship.RelationshipType,
                SourceUri = relationship.SourceUri,
                TargetMode = relationship.TargetMode,
                TargetUri = relationship.TargetUri
            };
        }
        public virtual void CopyRelationships(IEnumerable<PkgRelationship> relationships, ZipPackagePart part)
        {
            foreach (var rel in relationships)
            {
                part.CreateRelationship(rel.TargetUri, rel.TargetMode, rel.RelationshipType, rel.Id);
            }
        }
        public virtual void CopyRelationships(IEnumerable<PkgRelationship> relationships, Package package)
        {
            foreach (var rel in relationships)
            {
                package.CreateRelationship(rel.TargetUri, rel.TargetMode, rel.RelationshipType, rel.Id);
            }
        }

        static PackageContainer()
        {
            Mapper.Initialize(cfg => { });
        }
    }
}