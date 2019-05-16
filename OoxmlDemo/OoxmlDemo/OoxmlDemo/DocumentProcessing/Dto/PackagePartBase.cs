using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing.Dto
{
    public abstract class PackagePartBase
    {
        public Uri PartUri { get; set; }
        public string ContentType { get; set; }
        public CompressionOption CompressionOption { get; set; }
        protected PkgRelationship[] _relationships { get; set; }

        private static string[] _documentPaths = { "/word/document.xml", "/word/styles.xml", "/word/numbering.xml" };
        public static async Task<PackagePartBase> CreateAsync(ZipPackagePart part)
        {
            PackagePartBase newPart;
            if(_documentPaths.Contains( part.Uri.ToString()))
            {
                newPart = new PackagePartDocument();
            } else
            {
                newPart = new PackagePartImage();
            }
            await newPart.LoadAsync(part);
            return newPart;
        }
        public virtual async Task LoadAsync(ZipPackagePart part)
        {
            using (var stream = part.GetStream())
            {
                await ReadContentAsync(stream);
            }
            PartUri = part.Uri;
            ContentType = part.ContentType;
            CompressionOption = part.CompressionOption;
            _relationships = part.GetRelationships().Select( rel => new PkgRelationship {
                Id = rel.Id,
                RelationshipType = rel.RelationshipType,
                SourceUri = rel.SourceUri,
                TargetMode = rel.TargetMode,
                TargetUri = rel.TargetUri
            }).ToArray();
        }

        protected abstract Task ReadContentAsync(Stream stream);

        protected virtual async Task WriteAsync( Package destinationPackage)
        {
            var newPart = (ZipPackagePart)destinationPackage.CreatePart(PartUri, ContentType, CompressionOption);
            using (var filestream = newPart.GetStream(FileMode.Create, FileAccess.Write))
            {
                await WriteContentAsync(filestream);
            }
            WriteRelationships( newPart);
        }
        public abstract Task WriteContentAsync(Stream stream);

        public bool HasRelationships => _relationships?.Any() ?? false;
        public PkgRelationship AddRelationships( string relationshipType, Uri targetUri, Uri sourceUri = null, TargetMode targetMode = TargetMode.Internal)
        {
            PkgRelationship[] newList = new PkgRelationship[(_relationships?.Length ?? 0) + 1];
            Array.Copy(_relationships, newList, _relationships.Length);
            var newRelationship = new PkgRelationship() {
                Id = "xx",
                RelationshipType = relationshipType,
                SourceUri = sourceUri,
                TargetMode = targetMode,
                TargetUri = targetUri
            };
            newList[_relationships.Length] = newRelationship;
            _relationships = newList;

            return newRelationship;
        }
        public virtual void WriteRelationships(ZipPackagePart part)
        {
            if (_relationships != null)
            {
                foreach (var rel in _relationships)
                {
                    part.CreateRelationship(rel.TargetUri, rel.TargetMode, rel.RelationshipType, rel.Id);
                }
            }
        }
    }
}