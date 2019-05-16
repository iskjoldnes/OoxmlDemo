using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing
{
    public class SimpleUpdater : PackageContainer
    {
        public SimpleUpdater(string path) : base(path) { }

        protected override async Task UpdateDocumentAsync( )
        {
            ContentPart.Content = ContentPart.Content.Replace( "Test", "Eset");
        }
    }
}