using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Time
    {
        [XmlAttribute("to")]
        public DateTime to { get; set; }
        [XmlAttribute("from")]
        public DateTime from { get; set; }
        [XmlAttribute("datatype")]
        public string datatype { get; set; }

        [XmlElement("location")]
        public MetLocation location { get; set; }
    }
}