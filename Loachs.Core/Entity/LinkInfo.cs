using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    /// 连接实体(包括导航和友情连接)
    /// </summary>
    public class LinkInfo : IComparable<LinkInfo>
    {
        public int CompareTo(LinkInfo other)
        {
            if (this.Displayorder != other.Displayorder)
            {
                return this.Displayorder.CompareTo(other.Displayorder);
            }
            return this.LinkId.CompareTo(other.LinkId);
        }

        private int _linkid;
        private int _type;
        private string _name;
        private string _href;
        private int _position;
        private string _target;
        private string _description;
        private int _displayorder;
        private int _status;
        private DateTime _createdate;

        #region 非字段
        
        /// <summary>
        /// 连接Url
        /// </summary>
        public string Url
        {
            get
            {
                return Href.Replace("${siteurl}", ConfigHelper.SiteUrl);
            }
        }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Link
        {
            get
            {
                return string.Format("<a href=\"{0}\" title=\"{1}\" target=\"{2}\">{3}</a>", this.Url, this.Description, this.Target, this.Name);
            }
        }
        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public int LinkId
        {
            set { _linkid = value; }
            get { return _linkid; }
        }

        /// <summary>
        /// 类型(待用,现默认为0)
        /// </summary>
        public int Type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }

        /// <summary>
        /// 连接地址
        /// </summary>
        public string Href
        {
            set { _href = value; }
            get { return _href; }
        }

        /// <summary>
        /// 位置
        /// </summary>
        public int Position
        {
            set { _position = value; }
            get { return _position; }
        }

        /// <summary>
        /// 打开方式
        /// </summary>
        public string Target
        {
            set { _target = value; }
            get { return _target; }
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
        /// 状态(1:显示,0:隐藏)
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
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
