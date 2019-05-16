using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.ExternalServices.OpenStreetMap.Dto
{
    public class SearchResponse
    {
        public long place_id { get; set; }
        public string licence { get; set; }
        public string osm_type { get; set; }
        public long osm_id { get; set; }
        public decimal[] boundingbox { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
        public string display_name { get; set; }
        //public string class { get;set;}
        public string type { get; set; }
        public decimal importance { get; set; }
        public string icon { get; set; }
    }
}