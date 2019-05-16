using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.Nrk.Dto
{
    public class NewsChannel
    {
        [XmlElement("pubDate")]
        public string PublicationDate { get; set; }

        [XmlElement("item")]
        public List<NewsItem>   NewsItems { get; set; }
    }
}