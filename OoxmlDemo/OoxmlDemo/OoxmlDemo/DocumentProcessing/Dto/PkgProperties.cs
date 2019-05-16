using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.DocumentProcessing.Dto
{
    public class PkgProperties
    {
        public string Category { get; set; }
        public string ContentStatus { get; set; }
        public string ContentType { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? LastPrinted { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime? Modified { get; set; }
        public string Creator { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
        public string Keywords { get; set; }
        public string Language { get; set; }
        public string Revision { get; set; }
        public string Subject { get; set; }
        public string Title { get; set; }
        public string Version { get; set; }

    }
}