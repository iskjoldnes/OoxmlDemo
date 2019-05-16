using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.Statics
{
    public class DateTimeService : ExternalServiceBase
    {
        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            return (object)System.DateTime.Now;
        }

    }
}