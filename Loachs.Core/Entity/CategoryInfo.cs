using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;
using Loachs.Business;
using Loachs.Entity;

namespace Loachs.Entity
{
    /// <summary>
    /// 分类实体
    /// RSSURL
    /// </summary>
    public class CategoryInfo : IComparable<CategoryInfo>
    {
        public int CompareTo(CategoryInfo other)
        {
            if (this.Displayorder != other.Displayorder)
            {
                return this.Displayorder.CompareTo(other.Displayorder);
            }
            return this.CategoryId.CompareTo (other.CategoryId);
        }

        public CategoryInfo()
        {
            _name = string.Empty;
            _slug = string.Empty;
            _description = string.Empty;
            
        }
        private int _categoryid;
     //   private int _type;
        private string _name;
        private string _slug;
        private string _description;
        private int _displayorder;
        private int _count;
        private DateTime _createdate;

        #region 非字段

      

        /// <summary>
        /// 地址
        /// </summary>
        public string Url
        {

            get
            {
                string url = string.Empty;

                string slug = this.CategoryId.ToString();
                if (!string.IsNullOrEmpty(this.Slug))
                {
                    slug = StringHelper.UrlEncode(this.Slug);
                }


                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=category&slug={1}", ConfigHelper.SiteUrl, slug);
                }
                else
                {

                    url = string.Format("{0}category/{1}{2}", ConfigHelper.SiteUrl, slug, SettingManager.GetSetting().RewriteExtension);

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
                return string.Format("<a href=\"{0}\" title=\"分类:{1}\">{2}</a>", this.Url, this.Name, this.Name);
            }
        }


        /// <summary>
        /// 订阅URL
        /// </summary>
        public string FeedUrl
        {
            get
            {
                return string.Format("{0}feed/post/categoryid/{1}{2}", ConfigHelper.SiteUrl, this.CategoryId, SettingManager.GetSetting().RewriteExtension);
            }
        }
        /// <summary>
        /// 订阅连接
        /// </summary>
        public string FeedLink
        {
            get
            {
                return string.Format("<a href=\"{0}\" title=\"订阅:{1}\">订阅</a>", this.FeedUrl, this.Name);
            }
        }

        #endregion


        /// <summary>
        /// ID
        /// </summary>
        public int CategoryId
        {
            set
            {
                _categoryid = value;

            }
            get { return _categoryid; }
        }

        ///// <summary>
        ///// 类型
        ///// </summary>
        //public int Type
        //{
        //    set { _type = value; }
        //    get { return _type; }
        //}

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 别名
        /// </summary>
        public string Slug
        {
            set { _slug = value; }
            get { return _slug; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            set { _description = value; }
            get { return _description; }
        }

        /// <summary>
        /// 排序
        /// </summary>
        public int Displayorder
        {
            set { _displayorder = value; }
            get { return _displayorder; }
        }

        /// <summary>
        /// 次数
        /// </summary>
        public int Count
        {
            set { _count = value; }
            get { return _count; }
        }

        /// <summary>
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
    }
}
