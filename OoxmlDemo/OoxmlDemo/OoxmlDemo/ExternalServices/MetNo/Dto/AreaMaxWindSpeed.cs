using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class AreaMaxWindSpeed
    {
        [XmlAttribute("mps")]
        public decimal mps { get; set; }
    }
}