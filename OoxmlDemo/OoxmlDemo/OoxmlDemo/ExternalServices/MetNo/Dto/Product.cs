using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Product
    {
        [XmlAttribute("class")]
        public string productClass { get; set; }

        [XmlElement("time")]
        public List<Time> time { get; set; }
    }
}