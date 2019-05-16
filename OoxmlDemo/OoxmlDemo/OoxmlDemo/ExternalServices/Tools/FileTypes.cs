using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.ExternalServices.Tools
{
    public static class FileTypes
    {
        private const int AnyByte = 0x100;
        private struct FileType
        {
            public int[] Signature;
            public string ContentType;
        }
        private static FileType[] _filetypes = new FileType[] {
            new FileType { ContentType = "image/bmp", Signature = new int[] { 0x42, 0x4D } } ,
            new FileType { ContentType = "image/gif", Signature =  new int[]{ 0x47, 0x49, 0x46, 0x38, 0x37, 0x61} } ,
            new FileType { ContentType = "image/gif", Signature =  new int[]{ 0x47, 0x49, 0x46, 0x38, 0x39, 0x61} } ,
            new FileType { ContentType = "image/jpeg", Signature = new int[]{ 0xFF, 0xD8, 0xFF, 0xDB} } ,
            new FileType { ContentType = "image/jpeg", Signature = new int[]{ 0xFF, 0xD8, 0xFF, 0xE0, 0x00, 0x10, 0x4A, 0x46, 0x49, 0x46, 0x00, 0x01} } ,
            new FileType { ContentType = "image/jpeg", Signature = new int[]{ 0xFF, 0xD8, 0xFF, 0xEE } } ,
            new FileType { ContentType = "image/jpeg", Signature = new int[]{ 0xFF, 0xD8, 0xFF, 0xE1, AnyByte, AnyByte, 0x45, 0x78, 0x69, 0x66, 0x00, 0x00} } ,
            new FileType { ContentType = "image/png", Signature = new int[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A} } ,
        };

        public static string GetContentType( byte[] signature)
        {
            if (signature.Length > 16)
            {
                foreach (var filetype in _filetypes)
                {
                    for (int i = filetype.Signature.Length; i-- > 0 && (filetype.Signature[i] == AnyByte || filetype.Signature[i] == signature[i]) ;)
                    {
                        if ( i == 0)
                        {
                            return filetype.ContentType;
                        }
                    }
                }
            }
            return null;
        }
    }
}