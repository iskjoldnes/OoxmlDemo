using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.DocumentProcessing.Dto
{
    public class PackagePartImage : PackagePartBase
    {
        public byte[] Contents { get; set; }
        protected override Task ReadContentAsync(Stream stream)
        {
            Contents = new byte[stream.Length];
            return stream.ReadAsync(Contents, 0, Contents.Length);
        }

        public override Task WriteContentAsync(Stream stream)
        {
            return stream.WriteAsync(Contents, 0, Contents.Length);
        }

    }
}