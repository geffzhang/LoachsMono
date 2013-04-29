using System;
using System.Collections.Generic;
using System.Text;

namespace Loachs.Common
{
    /// <summary>
    /// web.config 配置类
    /// </summary>
    public class ConfigHelper
    {

        private static string _siteprefix;

        /// <summary>
        /// Cache、Session、Cookies 等应用程序变量名前缀
        /// </summary>
        public static string SitePrefix
        {
            get
            {
                if (string.IsNullOrEmpty(_siteprefix))
                {
                    _siteprefix = GetValue("loachs_siteprefix");
                }
                return _siteprefix;
            }
        }

        private static string _sitepath;

        /// <summary>
        /// 程序相对根路径
        /// </summary>
        public static string SitePath
        {
            get
            {
                if (string.IsNullOrEmpty(_sitepath))
                {
                    _sitepath = GetValue("loachs_sitepath");

                    if (!_sitepath.EndsWith("/"))
                    {
                        _sitepath += "/";
                    }
                }

                return _sitepath;
            }
        }
 

        /// <summary>
        /// 程序Url
        /// 未考虑https://;存在多个域名指向时，有BUG，因为静态变量已存在，如www.loachs.com,loachs.com
        /// 已解决
        /// </summary>
        public static string SiteUrl
        {
            get
            {

                string _siteurl = "http://" + System.Web.HttpContext.Current.Request.Url.Host + SitePath;
                if (System.Web.HttpContext.Current.Request.Url.Port != 80)
                {
                    _siteurl = "http://" + System.Web.HttpContext.Current.Request.Url.Host + ":" + System.Web.HttpContext.Current.Request.Url.Port + SitePath;
                }

                return _siteurl;
            }
        }

        private static  string _dbconnection;

        /// <summary>
        /// 数据库路径
        /// </summary>
        public static string DbConnection
        {
            get
            {
                if (string.IsNullOrEmpty(_dbconnection))
                {
                    _dbconnection = GetValue("loachs_dbconnection");
                }
                return _dbconnection;
            }
        }

        ///// <summary>
        ///// 数据库类型
        ///// </summary>
        //public static string DbType
        //{
        //    get { return GetValue("loachs_dbtype"); }
        //}

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        private static string GetValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];

        }
    }
}