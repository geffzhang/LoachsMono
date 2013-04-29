using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;
using Loachs.Business;
using Loachs.Entity;

namespace Loachs.Entity
{
    /// <summary>
    /// 标签实体
    /// </summary>
    public class TagInfo : IComparable<TagInfo>
    {
 
        public int CompareTo(TagInfo other)
        {
            if (this.Displayorder != other.Displayorder)
            {
                return this.Displayorder.CompareTo(other.Displayorder);
            }
            return this.TagId.CompareTo(other.TagId);
        }

        public TagInfo()
        {
            _name = string.Empty;
            _slug = string.Empty;
            _description = string.Empty;
            
        }
        private int _tagid;
     //   private int _type;
        private string _name;
        private string _slug;
        private string _description;
        private int _displayorder;
        private int _count;
        private DateTime _createdate;

        #region 非字段

        private string _url;
        private string _link;

        /// <summary>
        /// 地址
        /// </summary>
        public string Url
        {
            //set { _url = value; }
            //get { return _url; }
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=tag&slug={1}", ConfigHelper.SiteUrl, StringHelper.UrlEncode(this.Slug));
                }
                else
                {
                    url = string.Format("{0}tag/{1}{2}", ConfigHelper.SiteUrl, StringHelper.UrlEncode(this.Slug), SettingManager.GetSetting().RewriteExtension);
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }

        /// <summary>
        /// 连接
        /// </summary>
        public string Link
        {
            //set { _link = value; }
            //get { return _link; }
            get
            {
                return string.Format("<a href=\"{0}\" title=\"标签:{1}\">{2}</a>", this.Url, this.Name, this.Name);
            }
        }

        #endregion


        /// <summary>
        /// ID
        /// </summary>
        public int TagId
        {
            set
            {
                _tagid = value;

            }
            get { return _tagid; }
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
