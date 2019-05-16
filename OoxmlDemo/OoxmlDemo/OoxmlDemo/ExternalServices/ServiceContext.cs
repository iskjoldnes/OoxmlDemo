using OoxmlDemo.ExternalServices.MetNo;
using OoxmlDemo.ExternalServices.MockDb;
using OoxmlDemo.ExternalServices.Nrk;
using OoxmlDemo.ExternalServices.OpenStreetMap;
using OoxmlDemo.ExternalServices.Statics;
using OoxmlDemo.ExternalServices.Test;
using OoxmlDemo.ExternalServices.TheCatApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices
{
    public static class ServiceContext
    {
        public static async Task<object> GetInfo( string[] serviceName, Dictionary<string,string> parameters)
        {
            var externalServiceName = serviceName.FirstOrDefault();

            ExternalServiceBase service;
            return _services.TryGetValue(externalServiceName, out service)
                ? await service.GetInfoAsync(serviceName.Skip(1).ToArray(), parameters)
                : $"[Ukjent service {externalServiceName ?? "null"}]";
        }

        public static ServiceT GetService<ServiceT>() where ServiceT : ExternalServiceBase
        {
            return _services.Values.OfType<ServiceT>().FirstOrDefault();
        }

        private static Dictionary<string, ExternalServiceBase> _services = new Dictionary<string, ExternalServiceBase>
        {
            { "nrk", new NrkNewsService() },
            { "yr", new MetNoService() },
            { "map", new OpenStreetMapService() },
            { "test", new TestService() },
            { "cat", new TheCatApiService() },
            { "mockdb", new MockDbService() },
            { "datetime", new DateTimeService() }
        };

    }
}