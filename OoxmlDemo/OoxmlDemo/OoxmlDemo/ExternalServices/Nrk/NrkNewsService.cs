using OoxmlDemo.ExternalServices.Nrk.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.Nrk
{
    public class NrkNewsService : ExternalServiceBase
    {

        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            return (await GetNewsAsync(serviceName.FirstOrDefault())).NewsItems;
        }

        public async Task<NewsChannel> GetNewsAsync(string channelName)
        {
            string path;
            if( ! _channels.TryGetValue((channelName ?? string.Empty).ToLower(), out path))
            {
                path = "https://www.nrk.no/toppsaker.rss";
            }
            return (await HttpGetXml<NewsRss>(path))?.Channel;
        }

        private static Dictionary<string, string> _channels = new Dictionary<string, string>
        {
            { String.Empty, "https://www.nrk.no/toppsaker.rss" },
            { "nyheter", "https://www.nrk.no/nyheter/siste.rss" },
            { "innenriks-nyheter", "https://www.nrk.no/norge/toppsaker.rss" },
            { "urix", "https://www.nrk.no/urix/toppsaker.rss" },
            { "sápmi", "https://www.nrk.no/sapmi/oddasat.rss" },
            { "sport", "https://www.nrk.no/sport/toppsaker.rss" },
            { "kultur", "https://www.nrk.no/kultur/toppsaker.rss" },
            { "livsstil", "https://www.nrk.no/livsstil/toppsaker.rss" },
            { "viten", "https://www.nrk.no/viten/toppsaker.rss" },
            { "ytring", "https://www.nrk.no/ytring/toppsaker.rss" },
            { "p3", "https://p3.no/musikk/feed/" },
            { "17mai", "https://www.nrk.no/17mai/toppsaker.rss" },
            { "mgp", "https://www.nrk.no/ytring/toppsaker.rss" },
        };
    }
}