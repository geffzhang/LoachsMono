 
using System;

using Loachs.Common;
using Loachs.Business;

namespace Loachs.Entity
{
    /// <summary>
    /// 用户实体
    /// </summary>
    public class UserInfo : IComparable<UserInfo>
    {

        public int CompareTo(UserInfo other)
        {
            if (this.Displayorder != other.Displayorder)
            {
                return this.Displayorder.CompareTo(other.Displayorder);
            }
            return this.UserId.CompareTo(other.UserId);
        }

        private int _userid;
        private int _type;
        private int _status;
        private string _username;
        private string _name;
        private string _password;
        private string _email;
        private string _siteurl;
        private string _description;
        private string _avatarurl;
        private int _displayorder;

        private int _postcount;
        private int _commentcount;
      
        private DateTime _createdate;

        #region 非字段
   
        private string _url;
        private string _link;

        /// <summary>
        /// 地址
        /// </summary>
        public string Url
        {
          
            get {
                  string url = string.Empty;

                  if (Utils.IsSupportUrlRewriter == false)
                  {
                      url = string.Format("{0}default.aspx?type=author&username={1}", ConfigHelper.SiteUrl, StringHelper.UrlEncode(this.UserName));
                  }
                  else
                  {
                      return ConfigHelper.SiteUrl + "author/" + StringHelper.UrlEncode(this.UserName) + SettingManager.GetSetting().RewriteExtension;

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

            get { return string.Format("<a href=\"{0}\" title=\"作者:{1}\">{1}</a>", this.Url, this.Name); }
        }

        #endregion


        /// <summary>
        ///  用户ID
        /// </summary>
        public int UserId
        {
            set { _userid = value; } 
            get { return _userid; }
        }
        /// <summary>
        /// 用户类型 
        /// </summary>
        public int  Type
        {
            set { _type = value; }
            get { return _type; }
        }

        /// <summary>
        /// 用户账号
        /// </summary>
        public string UserName
        {
            set { _username = value; }
            get { return _username; }
        }
        /// <summary>
        /// 显示名称
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password
        {
            set { _password = value; }
            get { return _password; }
        }
        /// <summary>
        /// 邮箱
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 个人网站
        /// </summary>
        public string SiteUrl
        {
            set { _siteurl = value; }
            get { return _siteurl; }
        }


        /// <summary>
        /// 头像地址
        /// </summary>
        public string AvatarUrl
        {
            set { _avatarurl = value; }
            get { return _avatarurl; }
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
        /// 用户状态
        /// 1:使用,0:停用
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }
        
        /// <summary>
        /// 统计日志数
        /// </summary>
        public int PostCount
        {
            set { _postcount = value; }
            get { return _postcount; }
        }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount
        {
            set { _commentcount = value; }
            get { return _commentcount; }
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
        /// 创建日期
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
  
    }
}
