using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    /// 文章管理
    /// </summary>
    public class PostManager
    {
        static IPost dao = DataAccess.CreatePost();

        //private static readonly string CacheKey = "posts";

        //// 实体缓存键,如:"/post/detail25"



        ///// <summary>
        ///// 最近文章
        ///// </summary>
        //private static readonly string CACHEKEY_RECENTPOSTS = "/posts/recent";
        ///// <summary>
        ///// 推荐文章
        ///// </summary>
        //private static readonly string CACHEKEY_RECOMMENDPOSTS = "/posts/recommend";
        ///// <summary>
        ///// 置顶文章
        ///// </summary>
        //private static readonly string CACHEKEY_TOPPOSTS = "/posts/top";


        /// <summary>
        /// 列表
        /// </summary>
        private static List<PostInfo> _posts;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static PostManager()
        {
            LoadPost();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void LoadPost()
        {
            if (_posts == null)
            {
                lock (lockHelper)
                {
                    if (_posts == null)
                    {
                        _posts = dao.GetPostList();
                    }
                }
            }
        }



        ///// <summary>
        ///// 移动所有文章缓存
        ///// </summary>
        ///// <returns></returns>
        //private static bool RemovePostsCache()
        //{
        ////    Caches.Remove("/posts");

        //    return true;
        //}

        /// <summary>
        /// 添加文章
        /// </summary>
        /// <param name="post"></param>
        /// <returns></returns>
        public static int InsertPost(PostInfo post)
        {
            post.PostId = dao.InsertPost(post);

            _posts.Add(post);
            _posts.Sort();

            //统计
            StatisticsManager.UpdateStatisticsPostCount(1);
            //用户
            UserManager.UpdateUserPostCount(post.UserId, 1);
            //分类
            CategoryManager.UpdateCategoryCount(post.CategoryId, 1);
            //标签
            TagManager.UpdateTagUseCount(post.Tag, 1);

            //   RemovePostsCache();

            return post.PostId;
        }

        /// <summary>
        /// 修改文章
        /// </summary>
        /// <param name="_postinfo"></param>
        /// <returns></returns>
        public static int UpdatePost(PostInfo _postinfo)
        {
            //   PostInfo oldPost = GetPost(_postinfo.PostId);   //好像有问题,不能缓存

            PostInfo oldPost = GetPostByDatabase(_postinfo.PostId);

            int result = dao.UpdatePost(_postinfo);

            if (oldPost != null && oldPost.CategoryId != _postinfo.CategoryId)
            {
                //分类
                CategoryManager.UpdateCategoryCount(oldPost.CategoryId, -1);
                CategoryManager.UpdateCategoryCount(_postinfo.CategoryId, 1);
            }

            //     CacheHelper.Remove(CacheKey);

            //标签
            TagManager.UpdateTagUseCount(oldPost.Tag, -1);
            TagManager.UpdateTagUseCount(_postinfo.Tag, 1);

            //   RemovePostsCache();

            return result;
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static int DeletePost(int postid)
        {
            PostInfo oldPost = GetPost(postid);

            _posts.Remove(oldPost);

            int result = dao.DeletePost(postid);

            //统计
            StatisticsManager.UpdateStatisticsPostCount(-1);
            //用户
            UserManager.UpdateUserPostCount(oldPost.UserId, -1);
            //分类
            CategoryManager.UpdateCategoryCount(oldPost.CategoryId, -1);
            //标签
            TagManager.UpdateTagUseCount(oldPost.Tag, -1);

            //删除所有评论
            CommentManager.DeleteCommentByPost(postid);

            //     RemovePostsCache();

            return result;
        }

        /// <summary>
        /// 根据Id获取文章
        /// </summary>
        /// <param name="postid"></param>
        /// <returns></returns>
        public static PostInfo GetPost(int postid)
        {
            //PostInfo p = dao.GetPost(postid);
            ////  BuildPost(p);
            //return p;


            foreach (PostInfo post in _posts)
            {
                if (post.PostId == postid)
                {
                    return post;
                }
            }
            return null;
        }

        /// <summary>
        /// 从数据库获取文章
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static PostInfo GetPostByDatabase(int postId)
        {
            return dao.GetPost(postId);
        }

        /// <summary>
        /// 根据别名获取文章
        /// </summary>
        /// <param name="slug"></param>
        /// <returns></returns>
        public static PostInfo GetPost(string slug)
        {
            foreach (PostInfo post in _posts)
            {
                if (!string.IsNullOrEmpty(slug) && post.Slug.ToLower() == slug.ToLower())
                {
                    return post;
                }
            }
            return null;
        }

        ///// <summary>
        ///// 获取文章列表
        ///// </summary>
        ///// <param name="pageSize"></param>
        ///// <param name="pageIndex"></param>
        ///// <param name="recordCount"></param>
        ///// <returns></returns>
        //public static List<PostInfo> GetPostList(int pageSize, int pageIndex, out int recordCount)
        //{
        //    return GetPostList(pageSize, pageIndex, out recordCount,-1, -1, -1,-1, -1, -1, -1, null, null, null);
        //}


        /// <summary>
        /// 获取全部文章,是缓存的
        /// </summary>
        /// <returns></returns>
        public static List<PostInfo> GetPostList()
        {
            return _posts;
        }

        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int GetPostCount(int categoryId, int tagId, int userId)
        {
            int recordCount = 0;
            GetPostList(1, 1, out recordCount, categoryId, tagId, userId, -1, -1, -1, -1, string.Empty, string.Empty, string.Empty);

            return recordCount;
        }

        /// <summary>
        /// 获取文章数
        /// </summary>
        /// <param name="categoryId"></param>
        /// <param name="tagId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="hidestatus"></param>
        /// <returns></returns>
        public static int GetPostCount(int categoryId, int tagId, int userId,int status,int hidestatus)
        {
            int recordCount = 0;
            GetPostList(1, 1, out recordCount, categoryId, tagId, userId, -1, status, -1, hidestatus,  string.Empty, string.Empty, string.Empty);

            return recordCount;
        }

        /// <summary>
        /// 获取文章列表
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="recordCount"></param>
        /// <param name="type"></param>
        /// <param name="categoryId"></param>
        /// <param name="userId"></param>
        /// <param name="status"></param>
        /// <param name="topstatus"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<PostInfo> GetPostList(int pageSize, int pageIndex, out int recordCount, int categoryId, int tagId, int userId, int recommend, int status, int topstatus, int hidestatus, string begindate, string enddate, string keyword)
        {
            List<PostInfo> list = dao.GetPostList(pageSize, pageIndex, out recordCount, categoryId, tagId, userId, recommend, status, topstatus, hidestatus, begindate, enddate, keyword);
            return list;
        }

        public static List<PostInfo> GetPostList(int rowCount, int categoryId, int userId, int recommend, int status, int topstatus, int hidestatus)
        {
            List<PostInfo> list = _posts;
            if (categoryId != -1)
            {
                list = list.FindAll(delegate(PostInfo post) { return post.CategoryId == categoryId; });
            }

            if (userId != -1)
            {
                list = list.FindAll(delegate(PostInfo post) { return post.UserId == userId; });
            }
            if (recommend != -1)
            {
                list = list.FindAll(delegate(PostInfo post) { return post.Recommend == recommend; });
            }
            if (status != -1)
            {
                list = list.FindAll(delegate(PostInfo post) { return post.Status == status; });
            }
            if (topstatus != -1)
            {
                list = list.FindAll(delegate(PostInfo post) { return post.TopStatus == topstatus; });
            }
            if (hidestatus != -1)
            {
                list = list.FindAll(delegate(PostInfo post) { return post.HideStatus == hidestatus; });
            }

            if (rowCount > list.Count)
            {
                return list;
            }
            List<PostInfo> list2 = new List<PostInfo>();
            for (int i = 0; i < rowCount; i++)
            {
                list2.Add(list[i]);
            }
            return list2;
        }


        /// <summary>
        /// 更新点击数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdatePostViewCount(int postId, int addCount)
        {
            //   CacheHelper.Remove(CacheKey);

            PostInfo post = GetPost(postId);

            if (post != null)
            {
                post.ViewCount += addCount;
            }
            return dao.UpdatePostViewCount(postId, addCount);
        }

        /// <summary>
        /// 更新评论数
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdatePostCommentCount(int postId, int addCount)
        {
            PostInfo post = GetPost(postId);

            if (post != null)
            {
                post.CommentCount += addCount;

                return dao.UpdatePost(post);
            }
            return 0;

        }
    }
}
