using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices.MetNo.Dto
{
    public class MetLocation
    {
        [XmlAttribute("longitude")]
        public decimal longitude { get; set; }

        [XmlAttribute("latitude")]
        public decimal latitude { get; set; }

        [XmlAttribute("altitude")]
        public int altitude { get; set; }

        [XmlElement("precipitation")]
        public Precipitation precipitation { get; set; }

        [XmlElement("symbol")]
        public Symbol symbol { get; set; }

        [XmlElement("temperature")]
        public Temperature temperature { get; set; }

        [XmlElement("location")]
        public MetLocation location { get; set; }

        [XmlElement("windDirection")]
        public WindDirection windDirection { get; set; }

        [XmlElement("windSpeed")]
        public WindSpeed windSpeed { get; set; }

        [XmlElement("windGust")]
        public WindGust windGust { get; set; }

        [XmlElement("areaMaxWindSpeed")]
        public AreaMaxWindSpeed areaMaxWindSpeed { get; set; }

        [XmlElement("humidity")]
        public Humidity humidity { get; set; }

        [XmlElement("pressure")]
        public Pressure pressure { get; set; }

        [XmlElement("cloudiness")]
        public Cloudiness cloudiness { get; set; }

        [XmlElement("fog")]
        public Cloudiness fog { get; set; }

        [XmlElement("lowClouds")]
        public Cloudiness lowClouds { get; set; }

        [XmlElement("mediumClouds")]
        public Cloudiness mediumClouds { get; set; }

        [XmlElement("highClouds")]
        public Cloudiness highClouds { get; set; }

        [XmlElement("dewpointTemperature")]
        public Humidity dewpointTemperature { get; set; }
    }
}