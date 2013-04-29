using System;
using System.Collections.Generic;
using System.Text;

namespace Loachs.Entity
{
    /// <summary>
    /// 主题实体
    /// </summary>
    public class ThemeInfo
    {
        private string _name;
        private string _version;
        private string _author;
        private string _email;
        private string _siteurl;
        private string _pubdate;

        /// <summary>
        /// 主题名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 对应程序版本
        /// </summary>
        public string Version
        {
            set { _version = value; }
            get { return _version; }
        }

        /// <summary> 
        /// 作者
        /// </summary>
        public string Author
        {
            set { _author = value; }
            get { return _author; }
        }

        /// <summary>
        /// 作者Email
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }

        /// <summary>
        /// 作者网站
        /// </summary>
        public string SiteUrl
        {
            set { _siteurl = value; }
            get { return _siteurl; }
        }

        /// <summary>
        /// 发布日期
        /// </summary>
        public string PubDate
        {
            set { _pubdate = value; }
            get { return _pubdate; }
        }
    }
}
