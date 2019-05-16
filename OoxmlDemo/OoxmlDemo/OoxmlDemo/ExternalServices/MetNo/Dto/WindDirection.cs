using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class WindDirection
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("name")]
        public string name { get; set; }

        [XmlAttribute("deg")]
        public decimal deg { get; set; }

    }
}