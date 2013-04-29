using System;
using Loachs.Common;
namespace Loachs.Web
{
    public partial class admin_admin : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected string GetCssName(string fileType)
        {
            string filename = StringHelper.GetFileName(Request.Url.ToString());

            if (filename.IndexOf(fileType) != -1)
            {
                return "current";
            }
            return string.Empty;

        }
    }
}