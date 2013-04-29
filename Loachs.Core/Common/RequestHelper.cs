using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

namespace Loachs.Common
{

    public class RequestHelper
    {

        /// <summary>
        /// 判断当前页面是否接收到了Post请求
        /// </summary>
        /// <returns></returns>
        public static bool IsPost
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("POST");
            }
        }

        /// <summary>
        /// 判断当前页面是否接收到了Get请求
        /// </summary>
        /// <returns></returns>
        public static bool IsGet
        {
            get
            {
                return HttpContext.Current.Request.HttpMethod.Equals("GET");
            }
        }

        /// <summary>
        /// 返回当前页面是否是跨站提交
        /// </summary>
        /// <returns></returns>
        public static bool IsCrossSitePost
        {
            get
            {
                if (IsPost)
                {
                    if (UrlReferrer.Length < 7)
                    {
                        return true;
                    }
                    Uri u = new Uri(UrlReferrer);
                    return u.Host != Host;
                }
                return false;
            }
        }

        /// <summary>
        /// 得到主机头
        /// </summary>
        /// <returns></returns>
        public static string Host
        {
            get
            {
                return HttpContext.Current.Request.Url.Host;
            }
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的值</returns>
        public static string QueryString(string strName)
        {
            return QueryString(strName, false);
        }

        /// <summary>
        /// 获得指定Url参数的值
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="isLower">是否转化成小写</param>
        /// <returns></returns>
        public static string QueryString(string strName, bool isLower)
        {
            string temp = HttpContext.Current.Request.QueryString[strName];

            if (temp == null)
            {
                return string.Empty;
            }
            if (isLower)
            {
                return StringHelper.UrlDecode(temp.ToLower());
            }
            return StringHelper.UrlDecode(temp);
        }

        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <returns>Url参数的int类型值</returns>
        public static int QueryInt(string strName)
        {
            return StringHelper.StrToInt(HttpContext.Current.Request.QueryString[strName], 0);
        }
        /// <summary>
        /// 获得指定Url参数的int类型值
        /// </summary>
        /// <param name="strName">Url参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>Url参数的int类型值</returns>
        public static int QueryInt(string strName, int defValue)
        {
            return StringHelper.StrToInt(HttpContext.Current.Request.QueryString[strName], defValue);
        }



        /// <summary>
        /// 获得指定表单参数的值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <returns>表单参数的值</returns>
        public static string FormString(string strName)
        {
            if (HttpContext.Current.Request.Form[strName] == null)
            {
                return string.Empty;
            }
            return HttpContext.Current.Request.Form[strName];
        }

        /// <summary>
        /// 获得指定表单参数的int类型值
        /// </summary>
        /// <param name="strName">表单参数</param>
        /// <param name="defValue">缺省值</param>
        /// <returns>表单参数的int类型值</returns>
        public static int FormInt(string strName, int defValue)
        {
            return StringHelper.StrToInt(HttpContext.Current.Request.Form[strName], defValue);
        }


        /// <summary>
        /// 客户端IP地址
        /// </summary>
        public static string IPAddress
        {
            get
            {
                string result = String.Empty;
                result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (null == result || result == String.Empty)
                {
                    result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
                }
                if (null == result || result == String.Empty)
                {
                    result = HttpContext.Current.Request.UserHostAddress;
                }
                if (null == result || result == String.Empty || !StringHelper.IsIP(result))
                {
                    return "0.0.0.0";
                }
                return result;
            }
        }

        /// <summary> 
        /// 获得当前URL(重写前)
        /// 中文编码问题，默认页问题
        /// </summary>
        public static string CurrentUrl
        {
            get
            {
                return "http://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl;
            }
        }

        /// <summary>
        /// 获取有关客户端上次请求的 URL 的信息，该请求链接到当前的 URL。
        /// </summary>
        public static string UrlReferrer
        {
            get
            {
                return Convert.ToString(HttpContext.Current.Request.UrlReferrer);
            }
        }
        

        private static readonly Regex MOBILE_REGEX = new Regex(@"(nokia|sonyericsson|blackberry|iphone|samsung|sec\-|windows ce|motorola|mot\-|up.b|midp\-)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        /// <summary>
        /// 是否是手机浏览
        /// opera没搞定
        /// </summary>
        public static bool IsMobile
        {
            get
            {
                //if (IsSearchEngine)
                //{
                //    return false;
                //}

                HttpContext context = HttpContext.Current;
                if (context != null)
                {
                    HttpRequest request = context.Request;
                    //if (request.Browser.IsMobileDevice)
                    //    return true;
                    //!string.IsNullOrEmpty(request.UserAgent) &&

                    string useragent = request.UserAgent;

                    //if (!string.IsNullOrEmpty(useragent) && MOBILE_REGEX.IsMatch(useragent))
                    //{
                    //    return true;
                    //}

                    string accept = request.ServerVariables["HTTP_ACCEPT"];

                    if (!string.IsNullOrEmpty(accept) && accept.IndexOf("wap") > 0)
                    {
                        return true;
                    }
                    //if (!string.IsNullOrEmpty(accept) && MOBILE_REGEX.IsMatch(accept))
                    //{
                    //    return true;
                    //}
                    //if (string.IsNullOrEmpty(useragent))
                    //{
                    //    return true;
                    //}
                }

                return false;
            }
        }

        ///// <summary>
        ///// 判断是否来自搜索引擎链接
        ///// </summary>
        ///// <returns>是否来自搜索引擎链接</returns>
        //public static bool IsSearchEngine
        //{
        //    get
        //    {
        //        if (HttpContext.Current.Request.UrlReferrer == null)
        //            return false;

        //        string[] SearchEngine = { "google", "yahoo", "msn", "baidu", "sogou", "sohu", "sina", "163", "lycos", "tom", "yisou", "iask", "soso", "gougou", "zhongsou" };
        //        string tmpReferrer = HttpContext.Current.Request.UrlReferrer.ToString().ToLower();
        //        for (int i = 0; i < SearchEngine.Length; i++)
        //        {
        //            if (tmpReferrer.IndexOf(SearchEngine[i]) >= 0)
        //                return true;
        //        }
        //        return false;
        //    }
        //}
    }
}