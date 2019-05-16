using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.ExternalServices.TheCatApi.Dto
{
    public class CatItem
    {
        public string id { get; set; }
        public string url { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }
}