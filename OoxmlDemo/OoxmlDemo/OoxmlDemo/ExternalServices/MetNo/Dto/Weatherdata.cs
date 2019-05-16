using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    [XmlRoot("weatherdata")]
    public class Weatherdata
    {
        [XmlAttribute("created")]
        public DateTime created { get; set; }

        [XmlElement("meta")]
        public Meta meta { get; set; }

        [XmlElement("product")]
        public Product product { get; set; }
    }
}