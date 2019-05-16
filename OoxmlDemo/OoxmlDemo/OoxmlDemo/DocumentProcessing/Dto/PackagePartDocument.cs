using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing.Dto
{
    public class PackagePartDocument : PackagePartBase
    {
        public string Content { get; set; }
        protected override async Task ReadContentAsync(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                Content = await streamReader.ReadToEndAsync();
            }
        }

        public override async Task WriteContentAsync(Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream))
            {
                await streamWriter.WriteAsync(Content);
                await streamWriter.FlushAsync();
            }
        }
    }
}