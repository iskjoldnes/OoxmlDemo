using Microsoft.VisualStudio.TestTools.UnitTesting;
using OoxmlDemo.ExternalServices.OpenStreetMap;
using OoxmlDemo.ExternalServices.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoxmlDemoTests.ExternalServices
{
    [TestClass]
    public class OpenStreetMapServiceTests
    {
        [TestMethod]
        public void GetLocationTest()
        {
            var location = new OpenStreetMapService().GetLocation(new { country = "norway", city ="oslo"}).Result;
        }
    }
}
