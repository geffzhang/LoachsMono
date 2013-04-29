using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class admin_userlist : AdminPage
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        protected int UserId = RequestHelper.QueryInt("UserId");

        /// <summary>
        /// 修改时提示
        /// </summary>
        protected string PasswordMessage = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("作者管理");


            if (!IsPostBack)
            {
                BindUserList();

                if (Operate == OperateType.Update)
                {
                    BindUser();
                    btnEdit.Text = "修改";
                    txtUserName.Enabled = false;

                    PasswordMessage = "(不修改请留空)";
                }
                else if (Operate == OperateType.Delete)
                {
                    DeleteUser();
                }
            }

            ShowResult();
        }

        /// <summary>
        /// 显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = RequestHelper.QueryInt("result");
            switch (result)
            {
                case 1:
                    ShowMessage("添加成功!");
                    break;
                case 2:
                    ShowMessage("修改成功!");
                    break;
                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 4:
                    ShowError("不能删除自己!");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 绑定实体
        /// </summary>
        protected void BindUser()
        {
            UserInfo u = UserManager.GetUser(UserId);
            if (u != null)
            {
                txtUserName.Text = StringHelper.HtmlDecode(u.UserName);
                txtNickName.Text = StringHelper.HtmlDecode(u.Name);

                txtEmail.Text = StringHelper.HtmlDecode(u.Email);

                chkStatus.Checked = u.Status == 1 ? true : false;

                ddlUserType.SelectedValue = u.Type.ToString();

                txtDisplayOrder.Text = u.Displayorder.ToString();
            }
            if (u.UserId == PageUtils.CurrentUserId)
            {
                ddlUserType.Enabled = false;
                chkStatus.Enabled = false;
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindUserList()
        {
            List<UserInfo> list = UserManager.GetUserList();
            rptUser.DataSource = list;
            rptUser.DataBind();
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        protected void DeleteUser()
        {
            if (UserId == PageUtils.CurrentUserId)
            {
                Response.Redirect("userlist.aspx?result=4");
                return;
            }
            else
            {
                UserManager.DeleteUser(UserId);
                Response.Redirect("userlist.aspx?result=3");
            }

        }


        protected string GetUserType(object userType)
        {
            int type = Convert.ToInt32(userType);
            switch (type)
            {
                case (int)UserType.Administrator:
                    return "管理员";
                case (int)UserType.Author:
                    return "写作者";
                default:
                    return "未知身份";
            }
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            UserInfo u = new UserInfo();
            if (Operate == OperateType.Update)
            {
                u = UserManager.GetUser(UserId);
            }
            else
            {
                u.CommentCount = 0;
                u.CreateDate = DateTime.Now;
                u.PostCount = 0;
                u.UserName = StringHelper.HtmlEncode(txtUserName.Text.Trim());
            }

            u.Email = StringHelper.HtmlEncode(txtEmail.Text.Trim());
            u.SiteUrl = string.Empty;// StringHelper.HtmlEncode(txtSiteUrl.Text.Trim());
            u.Status = chkStatus.Checked ? 1 : 0;
            u.Description = string.Empty;// StringHelper.TextToHtml(txtDescription.Text);
            u.Type = StringHelper.StrToInt(ddlUserType.SelectedValue, 0);
            u.Name = StringHelper.HtmlEncode(txtNickName.Text.Trim());
            u.AvatarUrl = string.Empty;
            u.Displayorder = StringHelper.StrToInt(txtDisplayOrder.Text, 1000);

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()))
            {
                u.Password = StringHelper.GetMD5(txtPassword.Text.Trim());
            }

            if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && txtPassword.Text != txtPassword2.Text)
            {
                ShowError("两次密码输入不相同!");
                return;
            }


            if (Operate == OperateType.Update)
            {
                UserManager.UpdateUser(u);

                //  如果修改自己,更新COOKIE
                if (!string.IsNullOrEmpty(txtPassword.Text.Trim()) && u.UserId == PageUtils.CurrentUserId)
                {
                    PageUtils.WriteUserCookie(u.UserId, u.UserName, u.Password, 0);
                }
                Response.Redirect("userlist.aspx?result=2");
            }
            else
            {
                //System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex(@"[A-Za-z0-9\u4e00-\u9fa5-]");
                //if (!reg.IsMatch(u.UserName))
                //{
                //    ShowError("用户名不合法！");
                //    return;
                //}

                if (string.IsNullOrEmpty(u.UserName))
                {
                    ShowError("请输入登陆用户名!");
                    return;
                }

                System.Text.RegularExpressions.Regex reg = new System.Text.RegularExpressions.Regex("[A-Za-z0-9\u4e00-\u9fa5-]");
                if (!reg.IsMatch(u.UserName))
                {
                    ShowError("用户名限字母,数字,中文,连字符!");
                    return;
                }
                if (StringHelper.IsInt(u.UserName))
                {
                    ShowError("用户名不能为全数字!");
                    return;
                }

                if (string.IsNullOrEmpty(u.Password))
                {
                    ShowError("请输入密码!");
                    return;
                }
                if (UserManager.ExistsUserName(u.UserName))
                {
                    ShowError("该登陆用户名已存在,请换之");
                    return;
                }

                u.UserId = UserManager.InsertUser(u);
                Response.Redirect("userlist.aspx?result=1");
            }
        }
    }
}