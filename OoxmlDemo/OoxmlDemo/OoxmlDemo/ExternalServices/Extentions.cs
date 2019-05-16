using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OoxmlDemo.ExternalServices
{
    public static class Extentions
    {
        public static string GetValue( this Dictionary<string,string> parameters, string parameterName)
        {
            return parameters.Where(param => string.Equals(param.Key, parameterName, StringComparison.OrdinalIgnoreCase)).Select(param => param.Value).FirstOrDefault();
        }
        public static T GetValue<T>(this Dictionary<string, string> parameters, string parameterName)
        {
            object returnValue = default(T);
            string textValue = parameters.GetValue(parameterName);
            if(textValue != null)
            {
                if( typeof(T) == typeof(int))
                {
                    int value;
                    returnValue = int.TryParse( textValue, out value) ? value : 0;
                }
            }
            return (T)returnValue;
        }
    }
}