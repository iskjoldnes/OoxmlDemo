using OoxmlDemo.DocumentProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OoxmlDemo.docx
{
    public partial class Expand : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(ExecuterAsync));
        }
        protected async Task ExecuterAsync()
        {
            var filepath = Server.UrlDecode(Request["file"]);
            //var outputFile = await new SimpleUpdater(Server.UrlDecode(Request["file"])).ExpandDocumentAsync();
            var outputFile = await new MyUpdater(filepath).ExpandDocumentAsync();
            Response.ClearHeaders();
            Response.Headers.Add("Content-Disposition", $"inline; filename = \"{Path.GetFileName( filepath)}\"");
            Response.ContentType = outputFile.ContentType;

            Response.ClearContent();
            await Response.OutputStream.WriteAsync(outputFile.Content, 0, outputFile.Content.Length);
            await Response.OutputStream.FlushAsync();
            Response.End();
        }
    }
}