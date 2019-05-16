using Microsoft.VisualStudio.TestTools.UnitTesting;
using OoxmlDemo.ExternalServices.Nrk;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoxmlDemoTests.ExternalServices
{
    [TestClass]
    public class NrkNewsServiceTests
    {

        [TestMethod]
        public void GetNewsTest()
        {
            var newsChannel = new NrkNewsService().GetNewsAsync("").Result;
        }
    }

}
