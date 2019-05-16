using OoxmlDemo.ExternalServices.Dto;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace OoxmlDemo.ExternalServices.Test
{
    public class TestService : ExternalServiceBase
    {
        public override async Task<object> GetInfoAsync(string[] serviceName, Dictionary<string, string> parameters)
        {
            switch( (serviceName?.FirstOrDefault() ?? string.Empty).ToLower())
            {
                case "text":
                    return "Dette er en test.";
                case "longtext":
                    return "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.";
                case "list":
                    return new List<string> { "test1", "test2", "test3" };
                case "dict":
                    return new Dictionary<string, string> {
                        { "key1", "value1" },
                        { "key2", "value2" },
                        { "key3", "value3" },
                    };
                case "null":
                    return null;
                case "img":
                    return new ImageFile {
                        Content = TestImg,
                        ContentType = "image/gif"
                    };
            }
            return "test";
        }

        private static byte[] _testImg = null;
        private byte[] TestImg => _testImg ?? (_testImg = File.ReadAllBytes( "ExternalServices/Test/farkle.gif"));

    }
}