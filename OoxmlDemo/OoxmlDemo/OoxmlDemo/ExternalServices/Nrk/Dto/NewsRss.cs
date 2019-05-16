using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.Nrk.Dto
{
    [XmlRoot("rss")]
    public class NewsRss
    {
        [XmlElement("channel")]
        public NewsChannel Channel { get; set; }
        
    }
}