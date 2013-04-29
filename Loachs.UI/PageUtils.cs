using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Loachs.Common;
using Loachs.Business;
using Loachs.Entity;
using System.Net;
namespace Loachs.Web
{
    /// <summary>
    /// 页面工具类
    /// </summary>
    public class PageUtils
    {
        private PageUtils()
        {
        }



        /// <summary>
        /// 是否登陆
        /// </summary>
        public static bool IsLogin
        {
            get
            {
                if (HttpContext.Current.User == null) return false;
                return HttpContext.Current.User.Identity.IsAuthenticated;


            }
        }

        #region 用户COOKIE操作
        /// <summary>
        /// 用户COOKIE名
        /// </summary>
        private static readonly string CookieUser = ConfigHelper.SitePrefix + "amdin";
        /// <summary>
        /// 读当前用户COOKIE
        /// </summary>
        /// <returns></returns>
        public static HttpCookie ReadUserCookie()
        {
            return HttpContext.Current.Request.Cookies[CookieUser];
        }

        /// <summary>
        /// 移除当前用户COOKIE
        /// </summary>
        /// <returns></returns>
        public static bool RemoveUserCookie()
        {
            HttpCookie cookie = new HttpCookie(CookieUser);

            //cookie.Values.Clear();
            //cookie.Expires = DateTime.Now.AddYears(-1);

            //HttpContext.Current.Response.AppendCookie(cookie);
            return true;
        }

        /// <summary>
        /// 写/改当前用户COOKIE
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="expires"></param>
        /// <returns></returns>
        public static bool WriteUserCookie(int userID, string userName, string password, int expires)
        {
            HttpCookie cookie = HttpContext.Current.Request.Cookies[CookieUser];
            if (cookie == null)
            {
                cookie = new HttpCookie(CookieUser);
            }
            if (expires > 0)
            {
                cookie.Values["expires"] = expires.ToString();
                cookie.Expires = DateTime.Now.AddMinutes(expires);
            }
            else
            {
                int temp_expires = Convert.ToInt32(cookie.Values["expires"]);
                if (temp_expires > 0)
                {
                    cookie.Expires = DateTime.Now.AddMinutes(temp_expires);
                }
            }
            //    cookie.Values["userid"] = userID.ToString();
            cookie.Values["username"] = HttpContext.Current.Server.UrlEncode(userName);
            //cookie.Values["key"] = StringHelper.GetMD5(userID + HttpContext.Current.Server.UrlEncode(userName) + password);

            HttpContext.Current.Response.AppendCookie(cookie);
            return true;
        }
        #endregion

        /// <summary>
        /// 登陆用户ID
        /// </summary>
        public static int CurrentUserId
        {
            get
            {

                if (IsLogin)
                {
                    return StringHelper.StrToInt(HttpContext.Current.User.Identity.Name, 0);
                }
                return 0;
            }
        }

        /// <summary>
        /// 登陆名
        /// </summary>
        public static string CurrentUserName
        {
            get
            {
                if (IsLogin)
                {
                    return PageUtils.CurrentUser.UserName;
                    //return HttpContext.Current.Server.UrlDecode(PageUtils.ReadUserCookie()["username"]);
                }
                return string.Empty;
            }
        }



        ///// <summary>
        ///// 当前Key
        ///// </summary>
        //public static string CurrentKey
        //{
        //    get
        //    {
        //        if (IsLogin)
        //        {
        //            return PageUtils.ReadUserCookie()["key"];
        //        }
        //        return string.Empty;
        //    }
        //}

        /// <summary>
        /// 当前用户
        /// </summary>
        public static UserInfo CurrentUser
        {
            get
            {
                if (IsLogin)
                {
                    return UserManager.GetUser(CurrentUserId);
                }
                return null;
            }
        }

        /// <summary>
        /// 获取Gravatar Url
        /// </summary>
        /// <param name="email"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public static string GetGravatarUrl(string email, int size)
        {
            string hash = FormsAuthentication.HashPasswordForStoringInConfigFile(email, "MD5").ToLowerInvariant();
            string gravatar = "http://www.gravatar.com/avatar/" + hash + ".jpg?s=" + size + "&amp;d=";

            return gravatar;
        }


        /// <summary>
        /// 别名处理
        /// 仅留下字母,中文,数字,连字符
        /// 纯数字或NULL为自动处理,
        /// </summary>
        /// <param name="slug">别名</param>
        /// <param name="prefix">纯数字时的前缀</param>
        /// <returns></returns>
        public static string FilterSlug(string slug, string prefix)
        {
            return FilterSlug(slug, prefix, false);
        }

        /// <summary>
        /// 别名处理
        /// 仅留下字母,中文,数字,连字符
        /// 纯数字或NULL为自动处理,
        /// </summary>
        /// <param name="slug">别名</param>
        /// <param name="prefix">纯数字时的前缀</param>
        /// <param name="allowEmpty">允许为空</param>
        /// <returns></returns>
        public static string FilterSlug(string slug, string prefix, bool allowEmpty)
        {
            slug = System.Text.RegularExpressions.Regex.Replace(slug, "[^A-Za-z0-9\u4e00-\u9fa5-]", "");

            if (string.IsNullOrEmpty(slug) && allowEmpty == true)
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(slug))
            {
                slug = prefix + new Random().Next(10000, 99999);
            }

            if (StringHelper.IsInt(slug))
            {
                slug = prefix + slug;
            }

            return slug;
        }


        /// <summary>
        /// 写cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <param name="strValue">值</param>
        /// <param name="strValue">过期时间(分钟),0为永久</param>
        public static void SetCookie(string strName, string strValue, int expires)
        {
            strName = ConfigHelper.SitePrefix + strName;
            HttpCookie cookie = HttpContext.Current.Request.Cookies[strName];
            if (cookie == null)
            {
                cookie = new HttpCookie(strName);
            }

            cookie.Value = HttpContext.Current.Server.UrlEncode(strValue);
            //if (expires > 0)
            //{
            cookie.Expires = DateTime.Now.AddMinutes(expires);
            //}
            //else
            //{
            //    cookie.Expires = DateTime.Now.AddYears(10);
            //}
            HttpContext.Current.Response.AppendCookie(cookie);

        }

        /// <summary>
        /// 读cookie值
        /// </summary>
        /// <param name="strName">名称</param>
        /// <returns>cookie值</returns>
        public static string GetCookie(string strName)
        {
            strName = ConfigHelper.SitePrefix + strName;
            if (HttpContext.Current.Request.Cookies != null && HttpContext.Current.Request.Cookies[strName] != null)
            {
                return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.Cookies[strName].Value);
            }
            return string.Empty;
        }

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        public static string VerifyCode
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Session[ConfigHelper.SitePrefix + "VerifyCode"]);
            }
            set
            {
                HttpContext.Current.Session[ConfigHelper.SitePrefix + "VerifyCode"] = value;
            }

        }

        /// <summary>
        /// 单位转换
        /// </summary>
        /// <param name="size">byte</param>
        /// <returns></returns>
        public static string ConvertUnit(long size)
        {
            string FileSize = string.Empty;
            if (size > (1024 * 1024 * 1024))
                FileSize = ((double)size / (1024 * 1024 * 1024)).ToString(".##") + " GB";
            else if (size > (1024 * 1024))
                FileSize = ((double)size / (1024 * 1024)).ToString(".##") + " MB";
            else if (size > 1024)
                FileSize = ((double)size / 1024).ToString(".##") + " KB";
            else if (size == 0)
                FileSize = "0 Byte";
            else
                FileSize = ((double)size / 1).ToString(".##") + " Byte";

            return FileSize;
        }

        /// <summary>
        /// 是否为IE6
        /// </summary>
        public static bool IsIE6
        {
            get
            {
                if (HttpContext.Current.Request.Browser.Browser == "IE" && HttpContext.Current.Request.Browser.MajorVersion == 6)
                {
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 是否为IE7
        /// </summary>
        public static bool IsIE7
        {
            get
            {
                if (HttpContext.Current.Request.Browser.Browser == "IE" && HttpContext.Current.Request.Browser.MajorVersion == 7)
                {
                    return true;
                }

                return false;
            }
        }


    }
}