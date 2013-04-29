using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Business;
using Loachs.Common;

namespace Loachs.Entity
{
    /// <summary>
    /// 文章实体
    /// 是否要加个排序字段
    /// 是否要加个图片URL字段
    /// </summary>
    public class PostInfo : IComparable<PostInfo>
    {
        public int CompareTo(PostInfo other)
        {

            return other.PostId.CompareTo(this.PostId);
        }
        private int _postid;
        private int _type;
        private int _categoryid;
        private string _name;
        private string _summary;
        private string _content;
        private string _customurl;
        private int _userid;
        private int _commentstatus;
        private int _totalcomment;
        private int _hits;
        private string _tag;
        private int _urltype;
        private string _template = "post.html";
        private int _recommend;
        private int _status;
        private int _topstatus;
        private int _hidestatus;
        private DateTime _createdate;
        private DateTime _updatedate;

        #region 非字段

        //private string _url;
        //private string _link;
        //private string _detail;
        //private UserInfo _user;
        //private CategoryInfo _category;
        //private List<TagInfo> _tags;
        //private List<PostInfo> _relatedposts;
      
        /// <summary>
        /// 内容地址
        /// </summary>
        /// <remarks>
        /// desc:自动判断是否支持URL重写
        /// date:2012.7.5
        /// </remarks>
        public string Url
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=post&name={1}", ConfigHelper.SiteUrl, !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString());
                }
                else
                {
                    switch ((PostUrlFormat)this.UrlFormat)
                    {
                        //case PostUrlFormat.Category:
                        //    url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.AppUrl, StringHelper.UrlEncode(this.Category.Slug), !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), SettingManager.GetSetting().RewriteExtension);
                        //    break;

                        case PostUrlFormat.Slug:
                            url = string.Format("{0}post/{1}{2}", ConfigHelper.SiteUrl, !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), SettingManager.GetSetting().RewriteExtension);
                            break;
                        case PostUrlFormat.Date:
                        default:
                            url = string.Format("{0}post/{1}/{2}{3}", ConfigHelper.SiteUrl, this.CreateDate.ToString(@"yyyy\/MM\/dd"), !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), SettingManager.GetSetting().RewriteExtension);
                            break;
                    }
                }
                return Utils.CheckPreviewThemeUrl(url);

            }
        }

        /// <summary>
        /// 分页URL
        /// </summary>
        public string PageUrl
        {
            get
            {
                string url = string.Empty;

                if (Utils.IsSupportUrlRewriter == false)
                {
                    url = string.Format("{0}default.aspx?type=post&name={1}&page={2}", ConfigHelper.SiteUrl, !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), "{0}");
                }
                else
                {
                    switch ((PostUrlFormat)this.UrlFormat)
                    {

                        case PostUrlFormat.Slug:

                            url = string.Format("{0}post/{1}/page/{2}{3}", ConfigHelper.SiteUrl, !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), "{0}", SettingManager.GetSetting().RewriteExtension);
                            break;

                        case PostUrlFormat.Date:
                        default:
                            url = string.Format("{0}post/{1}/{2}/page/{3}{4}", ConfigHelper.SiteUrl, this.CreateDate.ToString(@"yyyy\/MM\/dd"), !string.IsNullOrEmpty(this.Slug) ? StringHelper.UrlEncode(this.Slug) : this.PostId.ToString(), "{0}", SettingManager.GetSetting().RewriteExtension);
                            break;
                    }
                }
                return Utils.CheckPreviewThemeUrl(url);
            }
        }
        
        /// <summary>
        /// 连接
        /// </summary>
        public string Link
        {
            get { return string.Format("<a href=\"{0}\">{1}</a>", this.Url, this.Title); }
        }

        /// <summary>
        /// 订阅URL
        /// </summary>
        public string FeedUrl
        {
            get
            {
                //return string.Format("{0}feed/comment/postid/{1}{2}", ConfigHelper.SiteUrl, this.PostId, SettingManager.GetSetting().RewriteExtension);
                return string.Format("{0}feed/comment/postid/{1}.aspx", ConfigHelper.SiteUrl, this.PostId);
            }
        }

        /// <summary>
        /// 订阅连接
        /// </summary>
        public string FeedLink
        {
            get
            {
                return string.Format("<a href=\"{0}\" title=\"订阅:{1} 的评论\">订阅</a>", this.FeedUrl, this.Title);
            }
        }

        /// <summary>
        /// 文章 详情
        /// 根据设置而定,可为摘要,正文
        /// </summary>
        public string Detail
        {
            get
            {
                switch (SettingManager.GetSetting().PostShowType)
                {
                    case 1:
                        return string.Empty;
                    case 2:
                    default:
                        return this.Summary;

                    case 3:
                        return StringHelper.CutString(StringHelper.RemoveHtml(this.Content), 200, "...");
                    case 4:
                        return this.Content;
                }
            }
        }

        /// <summary>
        /// Rss 详情
        /// 根据设置而定,可为摘要,正文,前200字,空等
        /// </summary>
        public string FeedDetail
        {
            get
            {
                switch (SettingManager.GetSetting().RssShowType)
                {
                    case 1:
                        return string.Empty;
                    case 2:
                    default:
                        return this.Summary;

                    case 3:
                        return StringHelper.CutString(StringHelper.RemoveHtml(this.Content), 200, "...");
                    case 4:
                        return this.Content;
                }
            }
        }

        /// <summary>
        /// 作者
        /// </summary>
        public UserInfo Author
        {
            get
            {
                UserInfo user = UserManager.GetUser(this.UserId);
                if (user != null)
                {
                    return user;
                }
                return new UserInfo();

            }
        }

        /// <summary>
        /// 所属分类
        /// </summary>
        public CategoryInfo Category
        {
            get
            {
                CategoryInfo category = CategoryManager.GetCategory(this.CategoryId);
                if (category != null)
                {
                    return category;
                }
                return new CategoryInfo();
            }
        }

        /// <summary>
        /// 对应标签
        /// </summary>
        public List<TagInfo> Tags
        {
            get
            {
                string temptags = this.Tag.Replace("{", "").Replace("}", ",");

                if (temptags.Length > 0)
                {
                    temptags = temptags.TrimEnd(',');
                }

                return TagManager.GetTagList(temptags);
            }
        }

        /// <summary>
        /// 下一篇文章(新)
        /// </summary>
        public PostInfo Next
        {
            get
            {
                List<PostInfo> list = PostManager.GetPostList();
                PostInfo post = list.Find(delegate(PostInfo p) { return p.HideStatus == 0 && p.Status == 1 && p.PostId > this.PostId; });
                return post != null ? post : new PostInfo();
            }
        }

        /// <summary>
        /// 上一篇文章(旧)
        /// </summary>
        public PostInfo Previous
        {
            get
            {
                List<PostInfo> list = PostManager.GetPostList();
                PostInfo post = list.Find(delegate(PostInfo p) { return p.HideStatus == 0 && p.Status == 1 && p.PostId < this.PostId; });

                return post != null ? post : new PostInfo();
            }
        }

        /// <summary>
        /// 相关文章
        /// </summary>
        public List<PostInfo> RelatedPosts
        {
            get
            {
                if (string.IsNullOrEmpty(this.Tag))
                {
                    return new List<PostInfo>();
                }

                List<PostInfo> list = PostManager.GetPostList().FindAll(delegate(PostInfo p) { return p.HideStatus == 0 && p.Status == 1; });

                string tags = this.Tag.Replace("}", "},");
                tags = tags.TrimEnd(',');

                string[] temparray = tags.Split(',');

                int num = 0;
                List<PostInfo> list2 = list.FindAll(delegate(PostInfo p)
                {
                    if (num >= SettingManager.GetSetting().PostRelatedCount)
                    {
                        return false;
                    }

                    foreach (string tag in temparray)
                    {
                        if (p.Tag.IndexOf(tag) != -1 && p.PostId != this.PostId)
                        {
                            num++;
                            return true;
                        }
                    }
                    return false;

                });


                return list2;
            }
        }


        #endregion

        /// <summary>
        /// ID
        /// </summary>
        public int PostId
        {
            set { _postid = value; }
            get { return _postid; }
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
        /// 分类ID
        /// </summary>
        public int CategoryId
        {
            set { _categoryid = value; }
            get { return _categoryid; }
        }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title
        {
            set { _name = value; }
            get { return _name; }
        }
        /// <summary>
        /// 别名
        /// </summary>
        public string Slug
        {
            set { _customurl = value; }
            get { return _customurl; }
        }

        /// <summary>
        /// 摘要
        /// </summary>
        public string Summary
        {
            set { _summary = value; }
            get { return _summary; }
        }
        /// <summary>
        /// 正文
        /// </summary>
        public string Content
        {
            set { _content = value; }
            get { return _content; }
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
        /// 是否允许评论
        /// </summary>
        public int CommentStatus
        {
            set { _commentstatus = value; }
            get
            {
                if (_commentstatus == 1 && SettingManager.GetSetting().CommentStatus == 1)
                {
                    return 1;
                }
                return 0;

            }
        }

        /// <summary>
        /// 评论数
        /// </summary>
        public int CommentCount
        {
            set { _totalcomment = value; }
            get { return _totalcomment; }
        }
        /// <summary>
        /// 点击数
        /// </summary>
        public int ViewCount
        {
            set { _hits = value; }
            get { return _hits; }
        }
        /// <summary>
        /// 标签
        /// </summary>
        public string Tag
        {
            set { _tag = value; }
            get { return _tag; }
        }

        /// <summary>
        /// URL类型,见枚举PostUrlFormat
        /// </summary>
        public int UrlFormat
        {
            set { _urltype = value; }
            get { return _urltype; }
        }

        /// <summary>
        /// 模板
        /// </summary>
        public string Template
        {
            set { _template = value; }
            get { return _template; }
        }

        /// <summary>
        /// 推荐
        /// </summary>
        public int Recommend
        {
            set { _recommend = value; }
            get { return _recommend; }
        }


        /// <summary>
        /// 状态
        /// </summary>
        public int Status
        {
            set { _status = value; }
            get { return _status; }
        }

  
        /// <summary>
        /// 置顶
        /// </summary>
        public int TopStatus
        {
            set { _topstatus = value; }
            get { return _topstatus; }
        }

        /// <summary>
        /// 隐藏于列表
        /// </summary>
        public int HideStatus
        {
            set { _hidestatus = value; }
            get { return _hidestatus; }
        }
        /// <summary>
        /// 添加时间
        /// </summary>
        public DateTime CreateDate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }
        /// <summary>
        /// 最后编辑时间
        /// </summary>
        public DateTime UpdateDate
        {
            set { _updatedate = value; }
            get { return _updatedate; }
        }
    }
}
