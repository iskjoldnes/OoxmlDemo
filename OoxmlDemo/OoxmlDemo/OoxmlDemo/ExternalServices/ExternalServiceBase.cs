using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;
using System.Reflection;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Net;
using System.Xml.Serialization;

namespace OoxmlDemo.ExternalServices
{
    public abstract class ExternalServiceBase
    {

        public async Task<ResT> HttpGetJson<ResT>( string url)
        {
            using (var client = NewHttpClient())
            {
                SetHttpHeaders(client);

                var content = await client.GetStringAsync(new Uri(url)).ConfigureAwait(false);
                return JsonConvert.DeserializeObject<ResT>(content);
            }
        }

        public async Task<ResT> HttpGetXml<ResT>(string url)
        {
            using (var client = NewHttpClient())
            {
                SetHttpHeaders(client);

                using (var response = await client.GetAsync(url).ConfigureAwait(false))
                using (var contentStrm = await response.Content.ReadAsStreamAsync())
                {
                    var serializer = new XmlSerializer(typeof(ResT));
                    var res = (ResT)serializer.Deserialize(contentStrm);
                    return res;
                }
            }
        }

        public async Task<byte[]> HttpGetImage(string url)
        {
            using (var client = NewHttpClient())
            {
                SetHttpHeaders(client);

                return await client.GetByteArrayAsync(new Uri(url)).ConfigureAwait(false);
            }
        }

        private static Dictionary<Type, Dictionary<string, Func<object, object>>> _typeProperties = new Dictionary<Type, Dictionary<string, Func<object, object>>>();
        public static Dictionary<string,Func<object,object>> GetPropertyGetters( Type type)
        {
            Dictionary<string, Func<object, object>> propertyList;
            if( ! _typeProperties.TryGetValue( type, out propertyList))
            {
                propertyList = type.GetProperties(BindingFlags.FlattenHierarchy | BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public).Select(p => new KeyValuePair<string, Func<object, object>>(p.Name, o => p.GetValue(o)))
                    .Concat(type.GetFields(BindingFlags.FlattenHierarchy | BindingFlags.GetField | BindingFlags.Instance | BindingFlags.Public).Select(p => new KeyValuePair<string, Func<object, object>>(p.Name, o => p.GetValue(o))))
                    .ToDictionary( e => e.Key, e => e.Value);
                _typeProperties[type] = propertyList;
            }
            return propertyList;
        }

        protected virtual HttpClient NewHttpClient()
        {
            HttpClientHandler hch = new HttpClientHandler();
            hch.Proxy = null;
            hch.UseProxy = false;

            return new HttpClient(hch);
        }
        protected virtual void SetHttpHeaders( HttpClient client)
        {
            client.DefaultRequestHeaders.TryAddWithoutValidation("Referer", "http://localhost");
            client.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/74.0.3729.131 Safari/537.36");
        }

        public abstract Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters);

    }
}