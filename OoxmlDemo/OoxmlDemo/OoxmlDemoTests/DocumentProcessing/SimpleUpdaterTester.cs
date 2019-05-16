using Microsoft.VisualStudio.TestTools.UnitTesting;
using OoxmlDemo.DocumentProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OoxmlDemoTests.DocumentProcessing
{
    [TestClass]
    public class SimpleUpdaterTester
    {
        [TestMethod]
        public void ExpandDocumentTest()
        {
            var path = @"\\VBOXSVR\ooxml\aa.docx";
            var result = new SimpleUpdater(path).ExpandDocumentAsync().Result;
            File.WriteAllBytes(@"\\VBOXSVR\ooxml\zz.docx", result.Content);
        }
    }
}
