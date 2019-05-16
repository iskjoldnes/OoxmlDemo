using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class WindGust
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("mps")]
        public decimal mps { get; set; }

    }
}