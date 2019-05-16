using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Symbol
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("number")]
        public int number { get; set; }

    }
}