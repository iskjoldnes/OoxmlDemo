using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Model
    {
        [XmlAttribute("to")]
        public DateTime to { get; set; }
        [XmlAttribute("from")]
        public DateTime from { get; set; }
        [XmlAttribute("nextrun")]
        public DateTime nextrun { get; set; }
        [XmlAttribute("runended")]
        public DateTime runended { get; set; }
        [XmlAttribute("termin")]
        public DateTime termin { get; set; }
        [XmlAttribute("name")]
        public string name { get; set; }
    }
}