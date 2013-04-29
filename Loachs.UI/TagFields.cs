using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
namespace Loachs.Web
{
    /// <summary>
    /// NVelocity 标签集合
    /// </summary>
    public class TagFields
    {
        #region 全局

        /// <summary>
        /// 网站名称
        /// </summary>
        public const string SITE_NAME = "sitename";

        /// <summary>
        /// 网站描述
        /// </summary>
        public const string SITE_DESCRIPTION = "sitedescription";

        /// <summary>
        /// 页面关键字
        /// </summary>
        public const string META_KEYWORDS = "metakeywords";

        /// <summary>
        /// 页面描述
        /// </summary>
        public const string META_DESCRIPTION = "metadescription";

        /// <summary>
        /// 页底HTML
        /// </summary>
        public const string FOOTER_HTML = "footerhtml";

        /// <summary>
        /// 版本
        /// </summary>
        public const string VERSION = "version";

        /// <summary>
        /// 页面标题
        /// </summary>
        public const string PAGE_TITLE = "pagetitle";

        /// <summary>
        /// 网站URL
        /// </summary>
        public const string SITE_URL = "siteurl";

        /// <summary>
        /// 网站路径
        /// </summary>
        public const string SITE_PATH = "sitepath";

        [Obsolete("共存")]
        /// <summary>
        /// 模板路径
        /// </summary>
        public const string THEME_PATH = "themepath";

        /// <summary>
        /// 模板地址
        /// 1.2 添加
        /// </summary>
        public const string THEME_URL = "themeurl";



        /// <summary>
        /// 是否为首页
        /// </summary>
        public const string IS_DEFAULT = "isdefault";

        /// <summary>
        /// 是否为文章页
        /// </summary>
        public const string IS_POST = "ispost";
        //是否其它页

        /// <summary>
        /// 订阅文章URL
        /// </summary>
        public const string FEED_URL = "feedurl";

        /// <summary>
        /// 订阅评论URL
        /// </summary>
        public const string FEED_COMMENT_URL = "feedcommenturl";

        /// <summary>
        /// 分页
        /// </summary>
        public const string PAGER = "pager";

        /// <summary>
        /// 分页(第几页)
        /// </summary>
        public const string PAGER_INDEX = "pagerindex";

        /// <summary>
        /// 当前URL(重写前)
        /// </summary>
        public const string URL = "url";

        /// <summary>
        /// 当前日期,时间
        /// </summary>
        public const string DATE = "date";

        /// <summary>
        /// 归档
        /// </summary>
        public const string ARCHIVES = "archives";

        /// <summary>
        /// 搜索关键字
        /// </summary>
        public const string SEARCH_KEYWORD = "searchkeyword";


        /// <summary>
        /// 当前页面查询数次
        /// </summary>
        public const string QUERY_COUNT = "querycount";

        /// <summary>
        /// 当前页面执行时间
        /// </summary>
        public const string PROCESS_TIME = "processtime";

        /// <summary>
        /// 是否启用验证码
        /// </summary>
        public const string ENABLE_VERIFYCODE = "enableverifycode";

        /// <summary>
        /// 头部动态输出
        /// </summary>
        /// <remarks>
        /// 1.2 新增
        /// </remarks>
        public const string HEAD = "head";

        /// <summary>
        /// 数据调用
        /// </summary>
        /// <remarks>
        /// 1.3 新增
        /// </remarks>
        public const string LOACHS = "loachs";

        ///// <summary>
        ///// 搜索地址
        ///// </summary>
        ///// <remarks>
        ///// 1.3新增
        ///// </remarks>
        //public const string SEARCH_URL = "searchurl";

        #endregion

        #region 文章

        /// <summary>
        /// 文章(实体)
        /// </summary>
        public const string POST = "post";

        /// <summary>
        /// 文章列表信息(作者,分类等)
        /// </summary>
        public const string POST_MESSAGE = "postmessage";

        /// <summary>
        /// 文章列表(分页)
        /// </summary>
        public const string POSTS = "posts";

        /// <summary>
        /// 最近文章列表
        /// </summary>
        public const string RECENT_POSTS = "recentposts";

        /// <summary>
        /// 推荐文章列表
        /// </summary>
        public const string RECOMMEND_POSTS = "recommendposts";

        /// <summary>
        /// 置顶文章列表
        /// </summary>
        public const string TOP_POSTS = "topposts";

        ///// <summary>
        ///// 点击文章列表
        ///// </summary>
        //public const string HOTVIEW_POSTS = "hotviewposts";

        ///// <summary>
        ///// 随机文章列表
        ///// </summary>
        //public const string RANDOM_POSTS = "randomposts";

        /// <summary>
        /// 订阅文章列表
        /// </summary>
        public const string FEED_POSTS = "feedposts";

        #endregion

        #region 评论

        /// <summary>
        /// 评论列表(分页)
        /// </summary>
        public const string COMMENTS = "comments";

        /// <summary>
        /// 最近评论列表
        /// </summary>
        public const string RECENT_COMMENTS = "recentcomments";

        /// <summary>
        /// 作者
        /// </summary>
        public const string COMMENT_AUTHOR = "commentauthor";
        /// <summary>
        /// 邮箱
        /// </summary>
        public const string COMMENT_EMAIL = "commentemail";
        /// <summary>
        /// 网址
        /// </summary>
        public const string COMMENT_SITEURL = "commentsiteurl";
        /// <summary>
        /// 内容
        /// </summary>
        public const string COMMENT_CONTENT = "commentcontent";
        /// <summary>
        /// 提示
        /// </summary>
        public const string COMMENT_MESSAGE = "commentmessage";

        #endregion

        #region 文章标签
        /// <summary>
        /// 标签列表(分页),未付值
        /// </summary>
        public const string TAGS = "tags";

        /// <summary>
        /// 最近标签列表
        /// </summary>
        public const string RECENT_TAGS = "recenttags";

        ///// <summary>
        ///// 热门标签列表(使用数)
        ///// </summary>
        //public const string HOT_TAGS = "hottags";

        ///// <summary>
        ///// 随机标签列表
        ///// </summary>
        //public const string RANDOM_TAGS = "randomtags";

        #endregion

        #region 文章分类
        /// <summary>
        /// 分类列表
        /// </summary>
        public const string CATEGORIES = "categories";

        #endregion

        #region 作者
        /// <summary>
        /// 作者列表
        /// </summary>
        public const string AUTHORS = "authors";

        #endregion

        #region 连接
        /// <summary>
        /// 连接列表(全部)
        /// </summary>
        public const string LINKS = "links";

        /// <summary>
        /// 导航连接列表
        /// </summary>
        public const string NAV_LINKS = "navlinks";

        /// <summary>
        /// 常规连接列表
        /// </summary>
        public const string GENERAL_LINKS = "generallinks";

        #endregion

        #region 统计

        /// <summary>
        /// 文章数
        /// </summary>
        public const string POST_COUNT = "postcount";

        /// <summary>
        /// 评论数
        /// </summary>
        public const string COMMENT_COUNT = "commentcount";

        /// <summary>
        /// 作者数
        /// </summary>
        public const string AUTHOR_COUNT = "authorcount";

        ///// <summary>
        ///// 标签数
        ///// </summary>
        //public const string TAG_COUNT = "tagcount";

        /// <summary>
        /// 访问数
        /// </summary>
        public const string VIEW_COUNT = "viewcount";

        #endregion
    }
}