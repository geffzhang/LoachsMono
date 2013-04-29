using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class admin_login : PageBase
    {

        protected void Page_Load(object sender, EventArgs e)
        {

            if (PageUtils.IsLogin)
            {
                Response.Redirect("default.aspx");
            }
            Title = "登录 - Powered by Loachs";


        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            string userName = StringHelper.HtmlEncode(txtUserName.Text.Trim());
            string password = StringHelper.GetMD5(txtPassword.Text.Trim());
            int expires = chkRemember.Checked ? 43200 : 0;
            string verifyCode = txtVerifyCode.Text;

            if (string.IsNullOrEmpty(verifyCode) || verifyCode != PageUtils.VerifyCode)
            {
                lblMessage.Text = "验证码错误!";
                return;
            }

            UserInfo user = UserManager.GetUser(userName, password);

            if (user != null)
            {
                if (user.Status == 0)
                {
                    lblMessage.Text = "此用户已停用!";
                    return;
                }
                FormsAuthentication.SetAuthCookie(user.UserId.ToString(), false);
                PageUtils.WriteUserCookie(user.UserId, user.UserName, user.Password, expires);
                Response.Redirect("default.aspx");
            }
            else
            {
                lblMessage.Text = "用户名或密码错误!";
            }

        }
    }
}