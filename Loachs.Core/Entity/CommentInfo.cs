using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;


using Loachs.Common;
using Loachs.Business;

namespace Loachs.Entity
{
    /// <summary>
    /// 评论实体
    /// </summary>
    public class CommentInfo
    {
        private int _commentid;
        private DateTime _createdate;
        private int _parentid;
        private int _articleid;
        private int _userid;
        private string _name;
        private string _email;
        private string _siteurl;
        private string _contents;
        private int _emailnotify;
        private string _ipaddress;
        private int _approved;

        #region 非字段
        private string _title;
        private string _url;
        private string _link;
        private string _authorlink;

        private int _floor;

        private string _gravatarcode;



        /// <summary>
        /// 标题(实为过滤html的正文某长度文字)
        /// </summary>
        public string Title
        {
            //set { _title = value; }
            get { return StringHelper.CutString(StringHelper.RemoveHtml(this.Content), 20, ""); }
        }


        /// <summary>
        /// URl地址
        /// </summary>
        /// <remarks>
        /// desc:考虑分页，总是跳到最后一页，需改进
        /// date:2012.7
        /// </remarks>
        public string Url
        {
            // set { _url = value; }
            get
            {
                //int commentCount = CommentManager.GetCommentCount(this.Post.PostId, false);

                //int pageCount = commentCount / SettingManager.GetSetting().PageSizeCommentCount;

                //if (commentCount % SettingManager.GetSetting().PageSizeCommentCount > 0)
                //{
                //    pageCount += 1;
                //}
                //string url = string.Format(this.Post.PageUrl + "#comment-" + CommentId, pageCount);

                //return url;

                PostInfo post = PostManager.GetPost(this.PostId);
                if (post != null)
                {
                    return string.Format("{0}#comment-{1}", post.Url, this.CommentId);
                }
                return "###";
            }
        }

        /// <summary>
        /// 评论连接
        /// </summary>
        public string Link
        {
            //  set { _link = value; }
            get
            {
                return string.Format("<a href=\"{0}\" title=\"{1} 发表于 {2}\">{3}</a>", this.Url, this.Name, this.CreateDate, this.Title);
            }
        }



        /// <summary>
        /// 作者连接
        /// </summary>
        public string AuthorLink
        {
            //   set { _authorlink = value; }
            get
            {
                if (this.UserId > 0)
                {
                    UserInfo user = UserManager.GetUser(this.UserId);
                    if (user != null)
                    {
                        return user.Link;
                    }

                }
                else if (StringHelper.IsHttpUrl(this.SiteUrl))
                {
                    return string.Format("<a href=\"{0}\" target=\"_blank\" title=\"{1}\">{1}</a>", this.SiteUrl, this.Name);
                }
                return this.Name;
            }
        }

        /// <summary>
        /// 层次
        /// </summary>
        public int Floor
        {
            set { _floor = value; }
            get { return _floor; }
        }

        /// <summary>
        /// Gravatar 加密后的字符串
        /// </summary>
        public string GravatarCode
        {
            //  set { _gravatarcode = value; }
            get
            {
                return FormsAuthentication.HashPasswordForStoringInConfigFile(this.Email, "MD5").ToLower();
            }
        }

        /// <summary>
        /// 评论对应文章
        /// </summary>
        public PostInfo Post
        {
            get
            {
                PostInfo post = PostManager.GetPost(this.PostId);
                if (post != null)
                {
                    return post;
                }
                return new PostInfo();
            }
        }

        #endregion


        /// <summary>
        /// 评论ID
        /// </summary>
        public int CommentId
        {
            set { _commentid = value; }
            get { return _commentid; }
        }

        /// <summary>
        /// 父ID
        /// </summary>
        public int ParentId
        {
            set { _parentid = value; }
            get { return _parentid; }
        }
        /// <summary>
        /// 文章ID
        /// </summary>
        public int PostId
        {
            set { _articleid = value; }
            get { return _articleid; }
        }
        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId
        {
            set { _userid = value; }
            get { return _userid; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// Email
        /// </summary>
        public string Email
        {
            set { _email = value; }
            get { return _email; }
        }
        /// <summary>
        /// 网址
        /// </summary>
        public string SiteUrl
        {
            set { _siteurl = value; }
            get { return _siteurl; }
        }
        /// <summary>
        /// 正文
        /// </summary>
        public string Content
        {
            set { _contents = value; }
            get { return _contents; }
        }

        /// <summary>
        /// 邮件通知
        /// 1:通知,0:不通知
        /// </summary>
        public int EmailNotify
        {
            set { _emailnotify = value; }
            get { return _emailnotify; }
        }

        /// <summary>
        /// IP地址
        /// </summary>
        public string IpAddress
        {
            set { _ipaddress = value; }
            get { return _ipaddress; }
        }
        /// <summary>
        /// 审核
        /// </summary>
        public int Approved
        {
            set { _approved = value; }
            get { return _approved; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
    }
}
