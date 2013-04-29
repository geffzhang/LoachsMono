using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
namespace Loachs.Web
{
    public partial class admin_logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PageUtils.RemoveUserCookie();
            FormsAuthentication.SignOut();
            Response.Redirect("../");
        }
    }
}