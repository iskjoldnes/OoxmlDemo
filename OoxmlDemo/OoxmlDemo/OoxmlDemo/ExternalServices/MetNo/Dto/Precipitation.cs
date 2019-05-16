using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Precipitation
    {
        [XmlAttribute("value")]
        public decimal value { get; set; }

        [XmlAttribute("unit")]
        public string unit { get; set; }

        [XmlAttribute("maxvalue")]
        public decimal maxvalue { get; set; }

        [XmlAttribute("minvalue")]
        public decimal minvalue { get; set; }
    }
}