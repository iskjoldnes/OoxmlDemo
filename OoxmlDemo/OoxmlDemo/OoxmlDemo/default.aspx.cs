using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace OoxmlDemo
{
    public partial class _default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            documentList.DataSource = Directory.EnumerateFiles(@"\\VBOXSVR\ooxml", "*.docx");
            documentList.DataBind();
        }
    }
}