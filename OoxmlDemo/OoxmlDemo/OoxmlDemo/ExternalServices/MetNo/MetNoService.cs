using OoxmlDemo.ExternalServices.Dto;
using OoxmlDemo.ExternalServices.MetNo.Dto;
using OoxmlDemo.ExternalServices.OpenStreetMap;
using OoxmlDemo.ExternalServices.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.MetNo
{
    public class MetNoService : ExternalServiceBase
    {
        private static readonly string _baseUrl = "https://api.met.no/weatherapi";

        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            switch( (serviceName.FirstOrDefault() ?? string.Empty).ToLower())
            {
                case "forecast":
                    return await GetForecast(serviceName.Skip(1).ToArray(), parameters);
                default:
                    return await GetForecast(serviceName, parameters);
                    //return $"[{serviceName.FirstOrDefault() ?? "null"} do not exist]";
            }

        }

        public async Task<object> GetForecast(string[] serviceName, Dictionary<string, string> parameters)
        {
            var searchResponse = (await ServiceContext.GetService<OpenStreetMapService>().GetLocation( parameters)).FirstOrDefault();
            if(searchResponse != null)
            {
                 var forecast = (await GetForecast(
                    new Location {
                        Altitude =70,
                        Latitude =searchResponse.lat,
                        Longtitude =searchResponse.lon
                    }))?.product;
                forecast.time = forecast.time.Where(t => t.location?.temperature != null).ToList();
                return PropertyNavigator.GetPropertyValue(forecast, serviceName);
            }
            return null;
        }

        public Task<Weatherdata> GetForecast(Location location)
        {
            var url = $"{_baseUrl}/locationforecast/1.9/?lat={location.Latitude.ToString()}&lon={location.Longtitude.ToString()}&msl={location.Altitude.ToString()}";
            return HttpGetXml<Weatherdata>(url);
        }

        private Dictionary<Tuple<int, string,bool>, byte[]> _symbolCache = new Dictionary<Tuple<int, string,bool>, byte[]>();
        public async Task<ImageFile> GetSymbol(int symbolId, string contentType, bool isNight = false)
        {
            byte[] image;
            if( ! _symbolCache.TryGetValue( new Tuple<int, string,bool>(symbolId,contentType,isNight), out image))
            {
                var url = $"{_baseUrl}/weathericon/1.1/?symbol={symbolId}&content_type={contentType}";
                if(isNight)
                {
                    url += "&is_night=1";
                }
                image = await HttpGetImage(url);

                _symbolCache[new Tuple<int, string, bool>(symbolId, contentType, isNight)] = image;
            }

            return new ImageFile { Content = image, ContentType = contentType };
        }

    }
}