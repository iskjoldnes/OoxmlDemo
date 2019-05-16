using OoxmlDemo.ExternalServices.Dto;
using OoxmlDemo.ExternalServices.OpenStreetMap.Dto;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.OpenStreetMap
{
    public class OpenStreetMapService : ExternalServiceBase
    {
        private static readonly string _baseUrl = "https://nominatim.openstreetmap.org/search";
        private static string[] _searchParameters = new[] { "city", "county", "country", "postalcode", "state", "street" };
        private static string MakeUrl(dynamic parameters)
        {
            var getters = GetPropertyGetters((Type)parameters.GetType());

            return string.Concat(
                _baseUrl,
                "?",
                string.Join("&",
                getters.Where(p => _searchParameters.Any(n => n.Equals(p.Key, StringComparison.OrdinalIgnoreCase))).Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value(parameters)?.ToString() ?? string.Empty)}")),
                "&format=json");
        }

        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            return await GetLocation( new { city = serviceName.First() ?? "oslo"});
        }

        public async Task<Location> GetLocation( dynamic searchCriteria)
        {
            //https://nominatim.openstreetmap.org/search?country=norway&city=oslo&format=json
            var json = await HttpGetJson<SearchResponse[]>(MakeUrl(searchCriteria));
            return new Location { };
        }

        public async Task<SearchResponse[]> GetLocation(Dictionary<string, string> parameters)
        {
            //https://nominatim.openstreetmap.org/search?country=norway&city=oslo&format=json
            var url = string.Concat(
                _baseUrl,
                "?",
                string.Join("&",
                parameters.Where(p => _searchParameters.Any(n => n.Equals(p.Key, StringComparison.OrdinalIgnoreCase))).Select(p => $"{p.Key}={HttpUtility.UrlEncode(p.Value ?? string.Empty)}")),
                "&format=json");
            return await HttpGetJson<SearchResponse[]>(url);
        }
    }
}