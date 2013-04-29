using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;
using Loachs.Business;

namespace Loachs.Entity
{
    /// <summary>
    /// 归档实体
    /// </summary>
    public class ArchiveInfo
    {
      //  private string _name;
        private DateTime _date;
      //  private string _url;
      //  private string _link;
        private int _count;

        //[Obsolete]
        ///// <summary>
        ///// 名称
        ///// </summary>
        //public string Name
        //{
        //    set { _name = value; }
        //    get { return _name; }
        //}
        /// <summary>
        /// 日期,用于拼Url
        /// </summary>
        public DateTime Date
        {
            set { _date = value; }
            get { return _date; }
        }
        /// <summary>
        /// url地址
        /// </summary>
        public string Url
        {

            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=archive&date={1}", ConfigHelper.SiteUrl, this.Date.ToString("yyyyMM"));
                }
                else
                {
                    return ConfigHelper.SiteUrl + "archive/" + this.Date.ToString("yyyyMM") + SettingManager.GetSetting().RewriteExtension;
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public string Link
        {

            get
            {
                return string.Format("<a href=\"{0}\" >{1}</a>", this.Url, this.Date.ToString("yyyyMM"));
            }
        }

        /// <summary>
        /// 数量
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }
    }
}
