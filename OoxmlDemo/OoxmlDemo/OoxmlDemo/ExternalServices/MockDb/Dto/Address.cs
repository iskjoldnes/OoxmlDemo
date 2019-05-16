using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.ExternalServices.MockDb.Dto
{
    public class Address
    {
        public int Id { get; set; }
        public string Street { get; set; }
        public string HouseNumber { get; set; }
        public string ZipCode { get; set; }
        public string City { get; set; }
    }
}