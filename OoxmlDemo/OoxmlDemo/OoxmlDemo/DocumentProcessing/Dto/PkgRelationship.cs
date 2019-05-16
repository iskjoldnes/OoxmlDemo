using System;
using System.Collections.Generic;
using System.IO.Packaging;
using System.Linq;
using System.Web;

namespace OoxmlDemo.DocumentProcessing.Dto
{
    public class PkgRelationship
    {
        public Uri SourceUri { get; set; }
        public Uri TargetUri { get; set; }
        public string RelationshipType { get; set; }
        public TargetMode TargetMode { get; set; }
        public string Id { get; set; }
    }
}