using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Text;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class _Default : Loachs.Web.PageBase
    {
        /// <summary>
        /// 模板封装类
        /// </summary>
        private TemplateHelper th = null;// new TemplateHelper();

        /// <summary>
        /// 模板路径
        /// </summary>
        private string templatePath = null;

        /// <summary>
        /// 页面类型
        /// </summary>
        public string pageType = RequestHelper.QueryString("type", true);

        /// <summary>
        /// 主题名称(文件夹)
        /// </summary>
        public string themeName;

        /// <summary>
        /// 预览主题名称(文件夹)
        /// </summary>
        public string previewThemeName = RequestHelper.QueryString("theme", true);

        protected void Page_Load(object sender, EventArgs e)
        {
            //   Response.Write(Server.UrlDecode(Request.Url.ToString()));

            if (pageType == "checkurlrewriter")
            {
                Response.Clear();
                Response.Write("支持当前设置的URL重写规则");
                Response.End();
            }

            UpdateViewCount();

            if (SettingManager.GetSetting().SiteStatus == 0)
            {
                ResponseError("网站已关闭", "网站已关闭,请与站长联系!");
            }

            themeName = SettingManager.GetSetting().Theme;
            if (RequestHelper.IsMobile)
            {
                themeName = SettingManager.GetSetting().MobileTheme;
            }
            if (!string.IsNullOrEmpty(previewThemeName))
            {
                themeName = previewThemeName;
            }
            // Response.Write("ismobile:" + RequestHelper.IsMobile + ",IsMobileDevice:" + Request.Browser.IsMobileDevice + ",BrowserType:" + Request.Browser.Type + ",UserAgent:" + Request.UserAgent + ",HTTP_ACCEPT:" + Request.ServerVariables["HTTP_ACCEPT"]);


            templatePath = Server.MapPath(string.Format("{0}/themes/{1}/template/", ConfigHelper.SitePath, themeName));

            //if (!System.IO.Directory.Exists(templatePath) && !string.IsNullOrEmpty(previewThemeName))
            //{
            //    ResponseError("预览主题不存在", "预览的主题不存在,是否修改了URL地址?");
            //}

            //非预览时
            if (!System.IO.Directory.Exists(templatePath) && string.IsNullOrEmpty(previewThemeName))
            {
                SettingInfo s = SettingManager.GetSetting();
                if (RequestHelper.IsMobile)
                {
                    s.MobileTheme = "default";
                }
                else
                {
                    s.Theme = "default";
                }
                themeName = "default";

                SettingManager.UpdateSetting();

                templatePath = Server.MapPath(string.Format("{0}/themes/default/template/", ConfigHelper.SitePath));
            }

            th = new TemplateHelper(templatePath);



            LoadDefault();

            LoachsDataManager loachs = new LoachsDataManager();

            th.Put("loachs", loachs);

            switch (pageType)
            {
                case "feed":
                    LoadFeed();
                    break;
                case "post":
                    LoadPost();
                    break;
                case "rsd":
                    LoadRsd();
                    break;
                case "wlwmanifest":
                    LoadWlwmanifest();
                    break;
                case "metaweblog":
                    LoadMetaweblog();
                    break;
                default:
                    //if (IsXmlrpcPost() == true)
                    //{
                    //    LoadMetaweblog();
                    //}
                    //else
                    //{
                    LoadPostList();
                    //    }
                    break;
            }
        }

        /// <summary>
        /// Rsd
        /// from BlogEngine Source
        /// </summary>
        protected void LoadRsd()
        {
            Response.ContentType = "text/xml";
            using (XmlTextWriter rsd = new XmlTextWriter(Response.OutputStream, Encoding.UTF8))
            {
                rsd.Formatting = Formatting.Indented;
                rsd.WriteStartDocument();

                // Rsd tag
                rsd.WriteStartElement("rsd");
                rsd.WriteAttributeString("version", "1.0");

                // Service 
                rsd.WriteStartElement("service");
                rsd.WriteElementString("engineName", "Loachs" + SettingManager.GetSetting().Version);
                rsd.WriteElementString("engineLink", "http://www.loachs.com");
                rsd.WriteElementString("homePageLink", ConfigHelper.SiteUrl);

                // APIs
                rsd.WriteStartElement("apis");

                // MetaWeblog
                rsd.WriteStartElement("api");
                rsd.WriteAttributeString("name", "MetaWeblog");
                rsd.WriteAttributeString("preferred", "true");
                rsd.WriteAttributeString("apiLink", ConfigHelper.SiteUrl + "xmlrpc/metaweblog.aspx");
                rsd.WriteAttributeString("blogID", "1");
                rsd.WriteEndElement();

                // WordPress
                rsd.WriteStartElement("api");
                rsd.WriteAttributeString("name", "WordPress");
                rsd.WriteAttributeString("preferred", "false");
                rsd.WriteAttributeString("apiLink", ConfigHelper.SiteUrl + "xmlrpc/metaweblog.aspx");
                rsd.WriteAttributeString("blogID", "1");
                rsd.WriteEndElement();

                // BlogML
                //rsd.WriteStartElement("api");
                //rsd.WriteAttributeString("name", "BlogML");
                //rsd.WriteAttributeString("preferred", "false");
                //rsd.WriteAttributeString("apiLink", Utils.AbsoluteWebRoot + "api/BlogImporter.asmx");
                //rsd.WriteAttributeString("blogID", Utils.AbsoluteWebRoot.ToString());
                //rsd.WriteEndElement();

                // End APIs
                rsd.WriteEndElement();

                // End Service
                rsd.WriteEndElement();

                // End Rsd
                rsd.WriteEndElement();

                rsd.WriteEndDocument();

            }
        }

        /// <summary>
        /// 加载WLW配置
        /// </summary>
        protected void LoadWlwmanifest()
        {
            string xml = "<?xml version=\"1.0\" encoding=\"utf-8\" ?>"
        + "<manifest xmlns=\"http://schemas.microsoft.com/wlw/manifest/weblog\">"
         + " <options>"
          + "  <clientType>Metaweblog</clientType>"
          + "  <supportsKeywords>Yes</supportsKeywords>"
          + "  <supportsNewCategories>Yes</supportsNewCategories>"
           + " <supportsMultipleCategories>No</supportsMultipleCategories>"
            + "<supportsNewCategoriesInline>Yes</supportsNewCategoriesInline>"
           + " <supportsCommentPolicy>Yes</supportsCommentPolicy>"
            + "<supportsSlug>Yes</supportsSlug>"
            + "<supportsExcerpt>Yes</supportsExcerpt>"
            + "<supportsGetTags>Yes</supportsGetTags>"
            + "<supportsPages>No</supportsPages>"
            + "<supportsAuthor>No</supportsAuthor>"
            + "<supportsCustomDate>No</supportsCustomDate>"
            + "<requiresHtmlTitles>No</requiresHtmlTitles>"
          + "</options>"
          + "<weblog>"
        + "    <ServiceName>Loachs</ServiceName>"


            //+ "<imageUrl>../common/images/logo.gif</imageUrl>"  //ICO?
                //+ "<watermarkImageUrl>../common/images/watermark/watermark.gif</watermarkImageUrl>" //LOGO


            + "<imageUrl>../common/images/wlw/icon.png</imageUrl>"  //ICO?
            + "<watermarkImageUrl>../common/images/wlw/watermark.png</watermarkImageUrl>" //LOGO  Live Writer version:14.0 , 83*83


            + "<homepageLinkText>查看网站</homepageLinkText>"
            + "<adminLinkText>管理网站</adminLinkText>"
            + "<adminUrl><![CDATA[{blog-homepage-url}admin]]></adminUrl>"
          + "</weblog>"
          + "<buttons>"
        + "    <button>"
              + "<id>1</id>"
              + "<text>网站预览</text>"
              + "<imageUrl>../common/images/wlw/sitepreview.png</imageUrl>"
              + "<contentUrl><![CDATA[{blog-homepage-url}]]></contentUrl>"
              + "<contentDisplaySize>980,550</contentDisplaySize>"
            + "</button>"
          + "</buttons>"
        + "</manifest>";

            Response.ContentType = "text/xml";
            Response.Write(xml);
        }

        /// <summary>
        /// Metaweblog 接口操作
        /// </summary>
        public void LoadMetaweblog()
        {

            Loachs.MetaWeblog.MetaWeblogHelper mw = new Loachs.MetaWeblog.MetaWeblogHelper();
            mw.ProcessRequest(HttpContext.Current);


        }

        /// <summary>
        /// 更新访问次数
        /// </summary>
        protected void UpdateViewCount()
        {
            string cookie = "isview";
            int isview = StringHelper.StrToInt(PageUtils.GetCookie(cookie), 0);
            //未访问或按刷新统计
            if (isview == 0 || SettingManager.GetSetting().SiteTotalType == 1)
            {
                StatisticsInfo stat = StatisticsManager.GetStatistics();
                stat.VisitCount += 1;
                StatisticsManager.UpdateStatistics();
            }
            //未访问
            if (isview == 0 && SettingManager.GetSetting().SiteTotalType == 2)
            {
                PageUtils.SetCookie(cookie, "1", 1440);
            }
        }

        /// <summary>
        /// 生成分页
        /// </summary>
        /// <param name="type"></param>
        /// <param name="slug"></param>
        /// <returns></returns>
        protected string MakeUrl(string type, string key, string value)
        {

            string url = string.Empty;

            if (Utils.IsSupportUrlRewriter == false)
            {
                url = ConfigHelper.SiteUrl + "default.aspx?type=" + type + "&" + key + "=" + value + "&page={0}";
            }
            else
            {
                if (string.IsNullOrEmpty(type) == true && string.IsNullOrEmpty(key) == true)
                {
                    url = (ConfigHelper.SiteUrl.EndsWith("/") ? ConfigHelper.SiteUrl + "page/{0}" : ConfigHelper.SiteUrl + "/page/{0}") + SettingManager.GetSetting().RewriteExtension;
                }
                else
                {
                    url = ConfigHelper.SiteUrl + type + "/" + value + "/page/{0}" + SettingManager.GetSetting().RewriteExtension;
                }
            }
            return Utils.CheckPreviewThemeUrl(url);
        }

        /// <summary>
        /// 加载文章列表
        /// </summary>
        protected void LoadPostList()
        {
            int categoryId = -1;
            int tagId = -1;
            int userId = -1;
            string keyword = string.Empty;
            string data = string.Empty;
            string begindate = string.Empty;
            string enddate = string.Empty;

            int pageindex = RequestHelper.QueryInt("page", 1);

            string messageinfo = string.Empty;

            string url = MakeUrl(string.Empty, string.Empty, string.Empty);

            if (pageType == "category")
            {
                string slug = RequestHelper.QueryString("slug");
                CategoryInfo cate = CategoryManager.GetCategory(slug);
                if (cate != null)
                {
                    categoryId = cate.CategoryId;
                    th.Put(TagFields.META_KEYWORDS, cate.Name);
                    th.Put(TagFields.META_DESCRIPTION, cate.Description);
                    th.Put(TagFields.PAGE_TITLE, cate.Name);
                    messageinfo = string.Format("<h2 class=\"post-message\">分类:{0}</h2>", cate.Name);

                    url = MakeUrl("category", "slug", Server.UrlEncode(slug));
                }
            }
            else if (pageType == "tag")
            {
                string slug = RequestHelper.QueryString("slug");
                TagInfo tag = TagManager.GetTagBySlug(slug);
                if (tag != null)
                {
                    tagId = tag.TagId;
                    th.Put(TagFields.META_KEYWORDS, tag.Name);
                    th.Put(TagFields.META_DESCRIPTION, tag.Description);
                    th.Put(TagFields.PAGE_TITLE, tag.Name);
                    messageinfo = string.Format("<h2 class=\"post-message\">标签:{0}</h2>", tag.Name);

                    url = MakeUrl("tag", "slug", Server.UrlEncode(slug));


                }

            }
            else if (pageType == "author")
            {
                string userName = RequestHelper.QueryString("username");
                UserInfo user = UserManager.GetUser(userName);
                if (user != null)
                {
                    userId = user.UserId;
                    th.Put(TagFields.META_KEYWORDS, user.Name);
                    th.Put(TagFields.META_DESCRIPTION, user.Description);
                    th.Put(TagFields.PAGE_TITLE, user.Name);
                    messageinfo = string.Format("<h2 class=\"post-message\">作者:{0}</h2>", user.Name);

                    url = MakeUrl("author", "username", Server.UrlEncode(userName));
                }
            }
            else if (pageType == "search")
            {
                keyword = StringHelper.CutString(StringHelper.SqlEncode(RequestHelper.QueryString("keyword")), 15);
                th.Put(TagFields.META_KEYWORDS, keyword);
                th.Put(TagFields.META_DESCRIPTION, keyword);
                th.Put(TagFields.PAGE_TITLE, keyword);
                th.Put(TagFields.SEARCH_KEYWORD, keyword);
                messageinfo = string.Format("<h2 class=\"post-message\">搜索:{0}</h2>", keyword);

                url = MakeUrl("search", "keyword", Server.UrlEncode(keyword));

            }
            else if (pageType == "archive")     //先按月归档
            {
                string datestr = RequestHelper.QueryString("date");

                string year = datestr.Substring(0, 4);
                string month = datestr.Substring(4, 2);
                DateTime date = Convert.ToDateTime(year + "-" + month);
                begindate = date.ToString("yyy-MM-dd");
                enddate = date.AddMonths(1).ToString("yyy-MM-dd");
                th.Put(TagFields.META_KEYWORDS, "归档");
                th.Put(TagFields.META_DESCRIPTION, SettingManager.GetSetting().SiteName + date.ToString("yyyy-MM") + "的归档");
                th.Put(TagFields.PAGE_TITLE, "归档:" + date.ToString("yyyy-MM"));
                messageinfo = string.Format("<h2 class=\"post-message\">归档:{0}</h2>", date.ToString("yyyy-MM"));

                url = MakeUrl("archive", "date", datestr);
            }

            else    //首页
            {
                if (pageindex == 1)
                {
                    th.Put(TagFields.IS_DEFAULT, "1");
                }
            }

            th.Put(TagFields.POST_MESSAGE, messageinfo);
            //     th.Put(TagFields.PAGER_INDEX, pageindex);

            int recordCount = 0;
            th.Put(TagFields.POSTS, PostManager.GetPostList(SettingManager.GetSetting().PageSizePostCount, pageindex, out recordCount, categoryId, tagId, userId, -1, 1, -1, 0, begindate, enddate, keyword));
            th.Put(TagFields.PAGER, Pager.CreateHtml(SettingManager.GetSetting().PageSizePostCount, recordCount, url));

            Display("default.html");
        }

        /// <summary>
        /// 添加评论
        /// </summary>
        protected void AddComment()
        {

            int contentMaxLength = 1000;    //内容最长长度

            int postid = RequestHelper.FormInt("postid", 0);
            string author = StringHelper.CutString(RequestHelper.FormString("commentauthor"), 20);
            string email = StringHelper.CutString(RequestHelper.FormString("commentemail"), 50);
            string siteurl = StringHelper.CutString(RequestHelper.FormString("commentsiteurl"), 100);

            string content = RequestHelper.FormString("commentcontent");
            int remeber = RequestHelper.FormInt("commentremember", 0);
            int emailnotify = RequestHelper.FormInt("commentemailnotify", 0);
            string verifycode = RequestHelper.FormString("commentverifycode");

            int expires = 525600; //一年
            if (remeber == 0)
            {
                expires = -1;
            }
            if (!PageUtils.IsLogin)
            {
                PageUtils.SetCookie(TagFields.COMMENT_AUTHOR, Server.UrlEncode(author), expires);
                PageUtils.SetCookie(TagFields.COMMENT_EMAIL, Server.UrlEncode(email), expires);
                PageUtils.SetCookie(TagFields.COMMENT_SITEURL, Server.UrlEncode(siteurl), expires);
            }

            //  PageUtils.SetCookie("commentcontent", Server.UrlEncode(StringHelper.CutString(content, contentMaxLength, "...")), expires);     //保留

            //第一次发表评论,并失败时用到
            th.Put(TagFields.COMMENT_AUTHOR, author);
            th.Put(TagFields.COMMENT_EMAIL, email);
            th.Put(TagFields.COMMENT_SITEURL, siteurl);
            th.Put(TagFields.COMMENT_CONTENT, content);


            if (SettingManager.GetSetting().EnableVerifyCode == 1 && (verifycode != PageUtils.VerifyCode || string.IsNullOrEmpty(verifycode)))
            {
                th.Put(TagFields.COMMENT_MESSAGE, "<div>验证码输入错误!</div>");
                return;
            }
            if (!StringHelper.IsEmail(email))
            {
                th.Put(TagFields.COMMENT_MESSAGE, "<div>邮箱格式错误!</div>");
                return;
            }

            if (content.Length > contentMaxLength || content.Length == 0)
            {
                th.Put(TagFields.COMMENT_MESSAGE, "<div>评论不能为空且限制在" + contentMaxLength + "字以内!</div>");
                return;
            }

            //   PageUtils.SetCookie("commentcontent", string.Empty, -1);    //清空
            th.Put(TagFields.COMMENT_CONTENT, string.Empty); //清空

            PostInfo post = PostManager.GetPost(postid);

            if (post == null)
            {
                Response.Redirect(ConfigHelper.SitePath);
            }

            CommentInfo c = new CommentInfo();

            c.Content = StringHelper.TextToHtml(StringHelper.CutString(content, contentMaxLength, "..."));
            c.CreateDate = DateTime.Now;
            c.Email = StringHelper.HtmlEncode(email);
            c.EmailNotify = emailnotify;
            c.IpAddress = RequestHelper.IPAddress;
            c.ParentId = 0;
            c.PostId = postid;
            c.UserId = PageUtils.CurrentUserId;
            c.Name = author;
            if (!string.IsNullOrEmpty(siteurl) && siteurl.IndexOf("http://") == -1)
            {
                siteurl = "http://" + siteurl;
            }
            c.SiteUrl = StringHelper.HtmlEncode(siteurl);

            switch (SettingManager.GetSetting().CommentApproved)
            {
                case 1:
                    c.Approved = (int)ApprovedStatus.Success;
                    break;
                case 2:
                    string[] blackwords = SettingManager.GetSetting().CommentSpamwords.Split(',');
                    bool hasBlackword = false;
                    foreach (string word in blackwords)
                    {
                        if (c.Content.IndexOf(word) != -1)
                        {
                            hasBlackword = true;
                            break;
                        }
                    }
                    c.Approved = hasBlackword ? (int)ApprovedStatus.Wait : (int)ApprovedStatus.Success;
                    break;
                case 3:
                default:
                    c.Approved = (int)ApprovedStatus.Wait;
                    break;
            }

            int newID = CommentManager.InsertComment(c);

            #region 发邮件
            if (SettingManager.GetSetting().SendMailNotifyByComment == 1) //给订阅者发邮件
            {
                //先不考虑审核的问题
                List<CommentInfo> list = CommentManager.GetCommentList(int.MaxValue, 1, -1, postid, 0, -1, 1, string.Empty);

                List<string> emailList = new List<string>();

                foreach (CommentInfo cmt in list)
                {
                    if (!StringHelper.IsEmail(cmt.Email))
                    {
                        continue;
                    }
                    //自己不用发
                    if (email == cmt.Email)
                    {
                        continue;
                    }
                    //不重复发送
                    if (emailList.Contains(cmt.Email))
                    {
                        continue;
                    }
                    emailList.Add(cmt.Email);

                    string subject = string.Empty;
                    string body = string.Empty;

                    subject = string.Format("[评论订阅通知]{0}", post.Title);
                    body += string.Format("您订阅的{0}有新评论了:<br/>", post.Title);
                    body += "<hr/>";
                    body += content;
                    body += "<hr/>";
                    body += "<br />评论作者: " + author;

                    if (!string.IsNullOrEmpty(siteurl))
                    {
                        body += string.Format(" (<a href=\"{0}\">{0}</a>)", siteurl);
                    }

                    body += "<br />评论时间: " + DateTime.Now;
                    body += string.Format("<br />原文连接: <a href=\"{0}\" title=\"{1}\" >{1}</a>", post.Url, post.Title);

                    body += "<br />注:系统自动通知邮件,不要回复。";

                    EmailHelper.SendAsync(cmt.Email, subject, body);
                }
            }

            if (SettingManager.GetSetting().SendMailAuthorByComment == 1)       //给文章作者发邮件
            {
                string subject = string.Empty;
                string body = string.Empty;

                subject = string.Format("[新评论通知]{0}", post.Title);
                body += string.Format("您发表的{0}有新评论了:<br/>", post.Title);
                body += "<hr/>";
                body += content;
                body += "<hr/>";
                body += "<br />评论作者: " + author;

                if (!string.IsNullOrEmpty(siteurl))
                {
                    body += string.Format(" (<a href=\"{0}\">{0}</a>)", siteurl);
                }

                body += "<br />评论时间: " + DateTime.Now;
                body += string.Format("<br />原文连接: <a href=\"{0}\" title=\"{1}\" >{1}</a>", post.Url, post.Title);

                body += "<br />注:系统自动通知邮件,不要回复。";

                UserInfo user = UserManager.GetUser(post.UserId);
                if (user != null && StringHelper.IsEmail(user.Email))
                {
                    EmailHelper.SendAsync(user.Email, subject, body);
                }
            }

            #endregion

            if (newID > 0)
            {
                if (post != null)
                {
                    if (SettingManager.GetSetting().CommentOrder == 1)
                    {
                        Response.Redirect(post.Url + "#comment-" + newID);
                    }
                    else
                    {
                        int commentCount = CommentManager.GetCommentCount(postid, false);

                        int pageCount = commentCount / SettingManager.GetSetting().PageSizeCommentCount;


                        if (commentCount % SettingManager.GetSetting().PageSizeCommentCount > 0)
                        {
                            pageCount += 1;
                        }
                        string url = string.Format(post.PageUrl + "#comment-" + newID, pageCount);

                        Response.Redirect(url);

                    }
                }
            }
        }

        /// <summary>
        /// 加载通用标签
        /// </summary>
        protected void LoadDefault()
        {
            #region 全局

            th.Put(TagFields.SITE_NAME, SettingManager.GetSetting().SiteName);
            th.Put(TagFields.SITE_DESCRIPTION, SettingManager.GetSetting().SiteDescription);
            th.Put(TagFields.META_KEYWORDS, SettingManager.GetSetting().MetaKeywords);
            th.Put(TagFields.META_DESCRIPTION, SettingManager.GetSetting().MetaDescription);

            th.Put(TagFields.FOOTER_HTML, SettingManager.GetSetting().FooterHtml);

            th.Put(TagFields.VERSION, SettingManager.GetSetting().Version);

            th.Put(TagFields.PAGE_TITLE, "首页");

            th.Put(TagFields.SITE_PATH, ConfigHelper.SitePath);
            th.Put(TagFields.SITE_URL, ConfigHelper.SiteUrl);

            th.Put(TagFields.THEME_PATH, ConfigHelper.SitePath + "themes/" + themeName + "/");
            th.Put(TagFields.THEME_URL, ConfigHelper.SiteUrl + "themes/" + themeName + "/");

            th.Put(TagFields.IS_DEFAULT, "0");
            th.Put(TagFields.IS_POST, "0");

            //th.Put(TagFields.FEED_URL, ConfigHelper.SiteUrl + "feed/post" + SettingManager.GetSetting().RewriteExtension);
            //th.Put(TagFields.FEED_COMMENT_URL, ConfigHelper.SiteUrl + "feed/comment" + SettingManager.GetSetting().RewriteExtension);

            th.Put(TagFields.FEED_URL, ConfigHelper.SiteUrl + "feed/post.aspx");
            th.Put(TagFields.FEED_COMMENT_URL, ConfigHelper.SiteUrl + "feed/comment.aspx");

            th.Put(TagFields.PAGER, string.Empty);
            th.Put(TagFields.PAGER_INDEX, RequestHelper.QueryInt("page", 1));

            th.Put(TagFields.URL, RequestHelper.CurrentUrl);
            th.Put(TagFields.DATE, DateTime.Now);

            th.Put(TagFields.ARCHIVES, ArchiveManager.GetArchive());

            th.Put(TagFields.SEARCH_KEYWORD, string.Empty);

            th.Put(TagFields.QUERY_COUNT, 0);
            th.Put(TagFields.PROCESS_TIME, 0);

            th.Put(TagFields.ENABLE_VERIFYCODE, SettingManager.GetSetting().EnableVerifyCode);

            string headhtml = string.Empty;

            headhtml += string.Format("<meta name=\"generator\" content=\"Loachs {0}\" />\n", SettingManager.GetSetting().Version);
            headhtml += "<meta name=\"author\" content=\"Loachs Team\" />\n";
            headhtml += string.Format("<meta name=\"copyright\" content=\"2008-{0} Loachs Team.\" />\n", DateTime.Now.Year);
            headhtml += string.Format("<link rel=\"alternate\" type=\"application/rss+xml\" title=\"{0}\"  href=\"{1}\"  />\n", SettingManager.GetSetting().SiteName, ConfigHelper.SiteUrl + "feed/post" + SettingManager.GetSetting().RewriteExtension);
            headhtml += string.Format("<link rel=\"EditURI\" type=\"application/rsd+xml\" title=\"RSD\" href=\"{0}xmlrpc/rsd.aspx\" />\n", ConfigHelper.SiteUrl);
            headhtml += string.Format("<link rel=\"wlwmanifest\" type=\"application/wlwmanifest+xml\" href=\"{0}xmlrpc/wlwmanifest.aspx\" />", ConfigHelper.SiteUrl);

            th.Put(TagFields.HEAD, headhtml);

            //if (Utils.IsSupportUrlRewriter == false)
            //{
            //    th.Put(TagFields.SEARCH_URL, ConfigHelper.SiteUrl + "default.aspx?type=search&keyword=");
            //}
            //else
            //{
            //      th.Put(TagFields.SEARCH_URL, ConfigHelper.SiteUrl + "search");
            //   }

            #endregion

            #region 文章

            //th.Put(TagFields.POST, null);
            //th.Put(TagFields.POST_MESSAGE, null);
            //th.Put(TagFields.POSTS, null);

            th.Put(TagFields.RECENT_POSTS, PostManager.GetPostList(SettingManager.GetSetting().SidebarPostCount, -1, -1, -1, 1, -1, 0));
            th.Put(TagFields.RECOMMEND_POSTS, PostManager.GetPostList(SettingManager.GetSetting().SidebarPostCount, -1, -1, 1, 1, -1, 0));
            th.Put(TagFields.TOP_POSTS, PostManager.GetPostList(Int32.MaxValue, -1, -1, -1, 1, 1, 0));

            //th.Put(TagFields.FEED_POSTS, null);

            #endregion

            #region 评论

            //th.Put(TagFields.COMMENTS, null);

            th.Put(TagFields.RECENT_COMMENTS, CommentManager.GetCommentListByRecent(SettingManager.GetSetting().SidebarCommentCount));

            if (PageUtils.IsLogin)
            {
                UserInfo user = UserManager.GetUser(PageUtils.CurrentUserId);
                if (user != null)
                {
                    th.Put(TagFields.COMMENT_AUTHOR, user.Name);
                    th.Put(TagFields.COMMENT_EMAIL, user.Email);
                    th.Put(TagFields.COMMENT_SITEURL, user.SiteUrl);
                }
            }
            else
            {
                th.Put(TagFields.COMMENT_AUTHOR, Server.UrlDecode(PageUtils.GetCookie(TagFields.COMMENT_AUTHOR)));
                th.Put(TagFields.COMMENT_EMAIL, Server.UrlDecode(PageUtils.GetCookie(TagFields.COMMENT_EMAIL)));
                th.Put(TagFields.COMMENT_SITEURL, Server.UrlDecode(PageUtils.GetCookie(TagFields.COMMENT_SITEURL)));
            }
            th.Put(TagFields.COMMENT_CONTENT, string.Empty);
            th.Put(TagFields.COMMENT_MESSAGE, string.Empty);

            #endregion

            #region 作者,分类,标签

            th.Put(TagFields.AUTHORS, UserManager.GetUserList().FindAll(delegate(UserInfo user) { return user.Status == 1; }));
            th.Put(TagFields.CATEGORIES, CategoryManager.GetCategoryList());
            th.Put(TagFields.RECENT_TAGS, TagManager.GetTagList(SettingManager.GetSetting().SidebarTagCount));

            #endregion

            #region 连接

            th.Put(TagFields.LINKS, LinkManager.GetLinkList(-1, 1));
            th.Put(TagFields.NAV_LINKS, LinkManager.GetLinkList((int)LinkPosition.Navigation, 1));
            th.Put(TagFields.GENERAL_LINKS, LinkManager.GetLinkList((int)LinkPosition.General, 1));

            #endregion

            #region 统计

            th.Put(TagFields.POST_COUNT, StatisticsManager.GetStatistics().PostCount);
            th.Put(TagFields.COMMENT_COUNT, StatisticsManager.GetStatistics().CommentCount);
            th.Put(TagFields.VIEW_COUNT, StatisticsManager.GetStatistics().VisitCount);
            th.Put(TagFields.AUTHOR_COUNT, UserManager.GetUserList().FindAll(delegate(UserInfo user) { return user.Status == 1; }).Count);

            #endregion
        }

        /// <summary>
        /// 加载文章
        /// </summary>
        protected void LoadPost()
        {
            th.Put(TagFields.IS_POST, "1");

            if (RequestHelper.IsPost && !RequestHelper.IsCrossSitePost)
            {
                AddComment();
            }

            PostInfo post = null;

            int postId = -1;
            string name = RequestHelper.QueryString("name");

            if (StringHelper.IsInt(name))
            {
                post = PostManager.GetPost(StringHelper.StrToInt(name, 0));
            }
            else
            {
                post = PostManager.GetPost(StringHelper.SqlEncode(name));
            }

            if (post == null)
            {
                ResponseError("文章未找到", "囧！没有找到此文章！", 404);
            }

            if (post.Status == (int)PostStatus.Draft)
            {
                ResponseError("文章未发布", "囧！此文章未发布！");
            }

            string cookie = "isviewpost" + post.PostId;
            int isview = StringHelper.StrToInt(PageUtils.GetCookie(cookie), 0);
            //未访问或按刷新统计
            if (isview == 0 || SettingManager.GetSetting().SiteTotalType == 1)
            {
                PostManager.UpdatePostViewCount(post.PostId, 1);
            }
            //未访问
            if (isview == 0 && SettingManager.GetSetting().SiteTotalType == 2)
            {
                PageUtils.SetCookie(cookie, "1", 1440);
            }

            th.Put(TagFields.POST, post);
            th.Put(TagFields.PAGE_TITLE, post.Title);

            string metaKeywords = string.Empty;
            foreach (TagInfo tag in post.Tags)
            {
                metaKeywords += tag.Name + ",";
            }
            if (metaKeywords.Length > 0)
            {
                metaKeywords = metaKeywords.TrimEnd(',');
            }
            th.Put(TagFields.META_KEYWORDS, metaKeywords);

            string metaDescription = post.Summary;
            if (string.IsNullOrEmpty(post.Summary))
            {
                metaDescription = post.Content;
            }
            th.Put(TagFields.META_DESCRIPTION, StringHelper.CutString(StringHelper.RemoveHtml(metaDescription), 50).Replace("\n", ""));

            int recordCount = 0;
            List<CommentInfo> commentList = CommentManager.GetCommentList(SettingManager.GetSetting().PageSizeCommentCount, Pager.PageIndex, out recordCount, SettingManager.GetSetting().CommentOrder, -1, post.PostId, 0, -1, -1, null);
            th.Put(TagFields.COMMENTS, commentList);
            th.Put(TagFields.PAGER, Pager.CreateHtml(SettingManager.GetSetting().PageSizeCommentCount, recordCount, post.PageUrl + "#comments"));

            //同时判断评论数是否一致
            if (recordCount != post.CommentCount)
            {
                post.CommentCount = recordCount;
                PostManager.UpdatePost(post);

            }

            th.Put(TagFields.LOACHS, new LoachsDataManager());

            if (System.IO.File.Exists(Server.MapPath(string.Format("{0}/themes/{1}/template/{2}", ConfigHelper.SitePath, themeName, post.Template))))
            {
                Display(post.Template);
            }
            else
            {
                Display("post.html");
            }
        }

        /// <summary>
        /// 加载feed
        /// </summary>
        protected void LoadFeed()
        {
            int categoryId = RequestHelper.QueryInt("categoryid", -1);
            int postId = RequestHelper.QueryInt("postid", -1);
            string action = RequestHelper.QueryString("action", true);

            //   Response.Clear();
            Response.ContentType = "text/xml";
            if (SettingManager.GetSetting().RssStatus == 1)
            {
                switch (action)
                {
                    case "comment":
                        List<CommentInfo> commentList = CommentManager.GetCommentList(SettingManager.GetSetting().RssRowCount, 1, -1, postId, 0, 1, -1, null);
                        PostInfo commentPost = PostManager.GetPost(postId);
                        Response.Write("<rss version=\"2.0\" xmlns:dc=\"http://purl.org/dc/elements/1.1/\" xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\" xmlns:wfw=\"http://wellformedweb.org/CommentAPI/\" xmlns:slash=\"http://purl.org/rss/1.0/modules/slash/\">\r\n");
                        Response.Write("    <channel>\r\n");
                        Response.Write("        <title><![CDATA[" + (commentPost == null ? SettingManager.GetSetting().SiteName : commentPost.Title) + "的评论]]></title>\r\n");
                        Response.Write("        <link><![CDATA[" + (commentPost == null ? ConfigHelper.SiteUrl : commentPost.Url) + "]]></link>\r\n");
                        Response.Write("        <description><![CDATA[" + SettingManager.GetSetting().SiteDescription + "]]></description>\r\n");
                        Response.Write("        <pubDate>" + DateTime.Now.ToString("r") + "</pubDate>\r\n");
                        Response.Write("        <generator>Loachs</generator>\r\n");
                        Response.Write("        <language>zh-cn</language>\r\n");
                        foreach (CommentInfo comment in commentList)
                        {
                            Response.Write("        <item>\r\n");
                            Response.Write("            <title><![CDATA[" + comment.Name + "对" + comment.Post.Title + "的评论]]></title>\r\n");
                            Response.Write("            <link><![CDATA[" + comment.Url + "]]></link>\r\n");
                            Response.Write("            <guid><![CDATA[" + comment.Url + "]]></guid>\r\n");
                            Response.Write("            <author><![CDATA[" + comment.Name + "]]></author>\r\n");

                            Response.Write(string.Format("          <description><![CDATA[{0}]]></description>\r\n", comment.Content));
                            Response.Write("            <pubDate>" + comment.CreateDate.ToString("r") + "</pubDate>\r\n");
                            Response.Write("        </item>\r\n");
                        }
                        Response.Write("    </channel>\r\n");
                        Response.Write("</rss>\r\n");
                        break;
                    default:
                        List<PostInfo> list = PostManager.GetPostList(SettingManager.GetSetting().RssRowCount, categoryId, -1, -1, 1, -1, 0);
                        templatePath = Server.MapPath(ConfigHelper.SitePath + "common/config/");
                        th = new TemplateHelper(templatePath);

                        LoadDefault();
                        th.Put(TagFields.FEED_POSTS, list);
                        Display("feed.config");
                        break;
                }
            }
            else
            {
                Response.Write("<rss>error</rss>\r\n");
            }
            //  Response.End();
        }

        /// <summary>
        /// 显示模板
        /// </summary>
        /// <param name="templatFileName">模板文件名</param>
        protected void Display(string templateFile)
        {

            //全局
            th.Put(TagFields.QUERY_COUNT, querycount);
            th.Put(TagFields.PROCESS_TIME, DateTime.Now.Subtract(starttick).TotalMilliseconds / 1000);

            // HttpContext.Current.Response.Clear();
            //   HttpContext.Current.Response.Write(writer.ToString());
            HttpContext.Current.Response.Write(th.BuildString(templateFile));

            //  HttpContext.Current.Response.End();
        }

        ///// <summary>
        ///// 是否为离线写作工具提交
        ///// </summary>
        ///// <returns></returns>
        //protected bool IsXmlrpcPost()
        //{
        //    string s = Request.HttpMethod;

        //    string ss = Request.UserAgent;

        //    byte[] buffer = new byte[HttpContext.Current.Request.InputStream.Length];

        //    HttpContext.Current.Request.InputStream.Read(buffer, 0, buffer.Length);

        //    string str = System.Text.Encoding.UTF8.GetString(buffer);
        //    if (str.Length > 0)
        //    {
        //        if (str.IndexOf("metaWeblog.") > 0 || str.IndexOf("blogger.") > 0 || str.IndexOf("wp.") > 0)
        //        {
        //            return true;
        //        }
        //    }

        //    return false;
        //}

    }


}