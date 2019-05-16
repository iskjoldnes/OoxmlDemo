using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.ExternalServices.MockDb.Dto
{
    public class Person
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Address PostalAddress { get; set; }
    }
}