using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Cloudiness
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("percent")]
        public decimal percent { get; set; }

    }
}