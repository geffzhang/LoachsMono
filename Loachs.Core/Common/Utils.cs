using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using Loachs.Business;

namespace Loachs.Common
{
    public class Utils
    {

        private static string rewriteExtension = "unknow";

        private static bool isSupportUrlRewriter = false;

        /// <summary>
        /// 当前环境是否支持当前配置的URl重写模式
        /// </summary>
        /// <remarks>
        /// date:2012.7.5
        /// </remarks>
        public static bool IsSupportUrlRewriter
        {
            get
            {

                if (rewriteExtension == "unknow" || rewriteExtension != SettingManager.GetSetting().RewriteExtension)
                {
                    rewriteExtension = SettingManager.GetSetting().RewriteExtension;

                    string url = ConfigHelper.SiteUrl + "checkurlrewriter" + SettingManager.GetSetting().RewriteExtension;

                    HttpStatusCode code = NetHelper.GetHttpStatusCode(url);
                    if (code == HttpStatusCode.OK)
                    {
                        isSupportUrlRewriter = true;
                    }
                    else
                    {
                        isSupportUrlRewriter = false;
                    }
                }
                return isSupportUrlRewriter;
            }
        }

        /// <summary>
        /// 预览主题URL
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string CheckPreviewThemeUrl(string url)
        {
            string theme = RequestHelper.QueryString("theme");
            if (!string.IsNullOrEmpty(theme))
            {
                if (url.IndexOf('?') > 0)
                {
                    url += "&theme=" + theme;
                }
                else
                {
                    url += "?theme=" + theme;
                }
            }

            return url;
        }
    }
}
