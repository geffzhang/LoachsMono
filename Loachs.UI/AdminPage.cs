using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;

using Loachs.Business;
using Loachs.Entity;
using Loachs.Common;

/// <summary>
/// 操作类型
/// </summary>
public enum OperateType
{
    /// <summary>
    /// 添加
    /// </summary>
    Insert = 0,
    /// <summary>
    /// 更新
    /// </summary>
    Update = 1,
    /// <summary>
    /// 删除
    /// </summary>
    Delete = 2,
}

namespace Loachs.Web
{
    /// <summary>
    /// 后台基类
    /// </summary>
    public class AdminPage : PageBase
    {
        protected SettingInfo setting;

        public AdminPage()
        {
            CheckLoginAndPermission();
            setting = SettingManager.GetSetting();
        }

        /// <summary>
        /// 检查登录和权限
        /// </summary>
        protected void CheckLoginAndPermission()
        {
            if (!PageUtils.IsLogin)
            {
                HttpContext.Current.Response.Redirect("login.aspx?returnurl=" + HttpContext.Current.Server.UrlEncode(RequestHelper.CurrentUrl));
            }
            UserInfo user = UserManager.GetUser(PageUtils.CurrentUserId);

            if (user == null)       //删除已登陆用户时有效
            {
                PageUtils.RemoveUserCookie();
                FormsAuthentication.SignOut();
                HttpContext.Current.Response.Redirect("login.aspx?returnurl=" + HttpContext.Current.Server.UrlEncode(RequestHelper.CurrentUrl));
                return;
            }

            //if (StringHelper.GetMD5(user.UserId + HttpContext.Current.Server.UrlEncode(user.UserName) + user.Password) != PageUtils.CurrentKey)
            //{
            //    PageUtils.RemoveUserCookie();
            //    HttpContext.Current.Response.Redirect("login.aspx?returnurl=" + HttpContext.Current.Server.UrlEncode(RequestHelper.CurrentUrl));
            //}

            if (PageUtils.CurrentUser.Status == 0)
            {
                ResponseError("您的用户名已停用", "您的用户名已停用,请与管理员联系!");
            }


            string[] plist = new string[] { "themelist.aspx", "themeedit.aspx", "linklist.aspx", "userlist.aspx", "setting.aspx", "categorylist.aspx", "taglist.aspx", "commentlist.aspx" };
            if (PageUtils.CurrentUser.Type == (int)UserType.Author)
            {
                string pageName = System.IO.Path.GetFileName(HttpContext.Current.Request.Url.ToString()).ToLower();

                foreach (string p in plist)
                {
                    if (pageName == p)
                    {
                        ResponseError("没有权限", "您没有权限使用此功能,请与管理员联系!");
                    }
                }
            }
        }



        protected void SetPageTitle(string title)
        {
            Page.Title = title + " - 管理中心 - Powered by Loachs";
        }


        /// <summary>
        /// 操作
        /// </summary>
        private string _operate = RequestHelper.QueryString("Operate");

        /// <summary>
        /// 操作类型
        /// </summary>
        public OperateType Operate
        {
            get
            {
                switch (_operate)
                {
                    case "update":
                        return OperateType.Update;
                    case "delete":
                        return OperateType.Delete;
                    default:
                        return OperateType.Insert;
                }
            }
        }

        /// <summary>
        /// 操作字符串
        /// </summary>
        public string OperateString
        {
            get
            {
                return _operate;
            }
        }

        /// <summary>
        /// 输出信息
        /// </summary>
        protected string ResponseMessage = string.Empty;

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public string ShowError(string error)
        {
            ResponseMessage = "<div class=\"p_error\">";
            ResponseMessage += error;
            ResponseMessage += "</div>";
            return ResponseMessage;
        }

        /// <summary>
        /// 正确提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ShowMessage(string message)
        {

            ResponseMessage = "<div class=\"p_message\">";
            ResponseMessage += message;
            ResponseMessage += "</div>";
            return ResponseMessage;
        }
    }

}