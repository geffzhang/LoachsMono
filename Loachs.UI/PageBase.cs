using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using Loachs.Business;
using Loachs.Common;

namespace Loachs.Web
{
    /// <summary>
    /// 测试模版引擎
    /// </summary>
    public class PageBase : System.Web.UI.Page
    {
        /// <summary>
        /// 查询次数统计
        /// </summary>
        public int querycount = 0;

        /// <summary>
        /// 当前页面开始载入时间(毫秒)
        /// </summary>
        public DateTime starttick;

        public PageBase()
        {
            starttick = DateTime.Now;

            querycount = Loachs.Data.Access.SqliteDbHelper.QueryCount;
            Loachs.Data.Access.SqliteDbHelper.QueryCount = 0;

        }

        /// <summary>
        /// 输出提示,并终止页面
        /// </summary>
        /// <param name="title"></param>
        /// <param name="msg"></param>
        public void ResponseError(string title, string msg)
        {
            //string str = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><header><title>{0} - {1}</title><style>a {{ color:gray;}}</style></header><body><div  style=\"border:1px solid #94a2a3; background-color:#F1F8FE; width:500px;   padding:8px 25px; margin:100px auto 0 auto;\" ><h5>提示:</h5><h2>{2}</h2><h4><a href=\"{3}\">返回首页</a></h4></div><div  style=\"  text-align:center; padding:5px 0;color:gray;font-size:12px;\" >Powered by <a href=\"http://www.loachs.com\" target=\"_blank\">Loachs</a> </div></body></html>";

            //str = string.Format(str, title, SettingManager.GetSetting().SiteName, msg, ConfigHelper.SiteUrl);

            //HttpContext.Current.Response.Clear();
            //HttpContext.Current.Response.Write(str);
            //HttpContext.Current.Response.End();

            ResponseError(title, msg, 500);

        }

        public void ResponseError(string title, string msg, int statusCode)
        {
            string str = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\"><html xmlns=\"http://www.w3.org/1999/xhtml\"><header><title>{0} - {1}</title><style>a {{ color:gray;}}</style></header><body><div  style=\"border:1px solid #94a2a3; background-color:#F1F8FE; width:500px;   padding:8px 25px; margin:100px auto 0 auto;\" ><h5>提示:</h5><h2>{2}</h2><h4><a href=\"{3}\">返回首页</a></h4></div><div  style=\"  text-align:center; padding:5px 0;color:gray;font-size:12px;\" >Powered by <a href=\"http://www.loachs.com\" target=\"_blank\">Loachs</a> </div></body></html>";

            str = string.Format(str, title, SettingManager.GetSetting().SiteName, msg, ConfigHelper.SiteUrl);

            HttpContext.Current.Response.Clear();
            //  HttpContext.Current.Response.Status = "200 Internal Server Error";
            HttpContext.Current.Response.StatusCode = statusCode;
            HttpContext.Current.Response.Write(str);
            HttpContext.Current.Response.End();
        }


        /// <summary>
        /// 错误处理
        /// </summary>
        /// <param name="e"></param>
        protected override void OnError(EventArgs e)
        {


            base.OnError(e);

        }

    }
}
