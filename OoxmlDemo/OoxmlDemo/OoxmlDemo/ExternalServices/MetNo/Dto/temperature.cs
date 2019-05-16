﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class Temperature
    {
        [XmlAttribute("id")]
        public string id { get; set; }

        [XmlAttribute("value")]
        public decimal value { get; set; }

        [XmlAttribute("unit")]
        public string unit { get; set; }

    }
}