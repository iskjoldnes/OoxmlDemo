using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.Nrk.Dto
{
    public class NewsItem
    {
        [XmlElement("title")]
        public string Title { get; set; }

        [XmlElement("link")]
        public string Link { get; set; }
        
        [XmlElement("description")]
        public string Description { get; set; }

        [XmlElement("pubDate")]
        public string PublicationDate { get; set; }
    }
}