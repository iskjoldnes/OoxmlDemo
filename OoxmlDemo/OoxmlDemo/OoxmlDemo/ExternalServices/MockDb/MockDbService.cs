using OoxmlDemo.ExternalServices.MockDb.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.MockDb
{
    public class MockDbService : ExternalServiceBase
    {
        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            switch ((serviceName.FirstOrDefault() ?? string.Empty).ToLower())
            {
                case "persons":
                    int id;
                    if (int.TryParse(parameters
                        .Where(p => "id".Equals(p.Key, StringComparison.OrdinalIgnoreCase))
                        .Select(p => p.Value)
                        .FirstOrDefault() ?? string.Empty, out id))
                    {

                        Person person;
                        return _persons.TryGetValue(id, out person) ? person : null;
                    }
                    break;
            }
            return null;
        }

        private static Dictionary<int, Person> _persons;
        static MockDbService()
        {
            string[] firstNames = { "Ole", "Gro", "Bjarne", "Ingrid", "Hans", "Marte", "Per", "Siv", "Magnus", "Line", "Henrik", "Linea", "Marta" };
            string[] lastNames = { "Olsen", "Hansen", "Johnsen", "Nilsen", "Bruntland", "Solberg", "Kristoffersen" };
            string[] streets = { "Stortorget", "Jallegata", "Storgata", "Svingen", "Tiriltoppen", "Karl Johan" };
            string[] cities = { "Oslo", "Bergen", "Stavanger", "Skien", "Drammen", "Hamar" };

            Random postalcodes = new Random();
            int streetConter = 0;
            int citiesConter = 0;
            int id = 100;
            _persons = firstNames.SelectMany(f => lastNames.Select(l =>
              new Person
              {
                  Id = id++,
                  FirstName = f,
                  LastName = l,
                  PostalAddress = new Address {
                      Id = id+100,
                      Street = streets[streetConter++ % streets.Length],
                      HouseNumber = postalcodes.Next( 1, 134).ToString(),
                      City = cities[citiesConter++ % cities.Length],
                      ZipCode = postalcodes.Next(20, 9100).ToString("D04"),
                  }
              })).ToDictionary(p => p.Id);
        }
    }
}