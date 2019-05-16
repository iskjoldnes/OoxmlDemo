using OoxmlDemo.ExternalServices.Dto;
using OoxmlDemo.ExternalServices.TheCatApi.Dto;
using OoxmlDemo.ExternalServices.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.TheCatApi
{
    public class TheCatApiService : ExternalServiceBase
    {
        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            var catImageData = (await HttpGetJson<CatItem[]>("https://api.thecatapi.com/v1/images/search"))?.FirstOrDefault();

            var url = catImageData?.url;
            var content = await HttpGetImage(url);
            return content != null 
                ? new ImageFile {
                    Content = content,
                    ContentType = FileTypes.GetContentType( content)
                } 
                : null;
            
        }
    }
}