using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Meta
    {
        [XmlElement("model")]
        public List<Model> model {get;set;}
    }
}