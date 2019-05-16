using Microsoft.VisualStudio.TestTools.UnitTesting;
using OoxmlDemo.ExternalServices.Dto;
using OoxmlDemo.ExternalServices.MetNo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoxmlDemoTests.ExternalServices
{
    [TestClass]
    public class MetNoServiceTests
    {
        [TestMethod]
        public void GetForecastTest()
        {
            var location = new MetNoService().GetForecast(new Location { Altitude=70, Latitude= 60.10M, Longtitude=9.58M }).Result;
        }

        [TestMethod]
        public void GetSymbolTest()
        {
            var imageFile = new MetNoService().GetSymbol( 3, "image/png").Result;
        }
    }
}
