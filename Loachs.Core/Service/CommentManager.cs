using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Security;

using Loachs.Common;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    /// 评论管理
    /// </summary>
    public class CommentManager
    {
        private static IComment dao = DataAccess.CreateComment();

        /// <summary>
        /// 最近评论列表
        /// </summary>
        private static List<CommentInfo> _recentcomments;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static CommentManager()
        {
            //   LoadRecentComment(); 
        }


        /// <summary>
        /// 加载最近评论
        /// </summary>
        private static void LoadRecentComment(int rowCount)
        {
            lock (lockHelper)
            {
                _recentcomments = GetCommentList(rowCount, 1, -1, -1, 0, (int)ApprovedStatus.Success, -1, string.Empty);
            }
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static int InsertComment(CommentInfo comment)
        {
            int result = dao.InsertComment(comment);

            //统计
            StatisticsManager.UpdateStatisticsCommentCount(1);

            //用户
            UserManager.UpdateUserCommentCount(comment.UserId, 1);

            //文章
            PostManager.UpdatePostCommentCount(comment.PostId, 1);


            _recentcomments = null;

            return result;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public static int UpdateComment(CommentInfo comment)
        {
            _recentcomments = null;

            return dao.UpdateComment(comment);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public static int DeleteComment(int commentId)
        {
            CommentInfo comment = GetComment(commentId);

            int result = dao.DeleteComment(commentId);

            //统计
            StatisticsManager.UpdateStatisticsCommentCount(-1);

            if (comment != null)
            {
                //用户
                UserManager.UpdateUserCommentCount(comment.UserId, -1);
                //文章
                PostManager.UpdatePostCommentCount(comment.PostId, -1);
            }

            _recentcomments = null;

            return result;
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public static CommentInfo GetComment(int commentId)
        {
            CommentInfo comment = dao.GetComment(commentId);
            return comment;
        }


        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="rowCount"></param>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="parentId"></param>
        /// <param name="approved"></param>
        /// <param name="emailNotify"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<CommentInfo> GetCommentList(int rowCount, int order, int userId, int postId, int parentId, int approved, int emailNotify, string keyword)
        {
            int totalRecord = 0;
            return GetCommentList(rowCount, 1, out totalRecord, order, userId, postId, parentId, approved, emailNotify, keyword);
        }

        /// <summary>
        /// 最近评论
        /// </summary>
        /// <param name="rowCount"></param>
        /// <returns></returns>
        public static List<CommentInfo> GetCommentListByRecent(int rowCount)
        {
            if (_recentcomments == null)
            {
                LoadRecentComment(rowCount);
            }
            return _recentcomments;

            //string cacheKey = "/comment/recent";// +rowCount;

            //List<CommentInfo> list = (List<CommentInfo>)Caches.Get(cacheKey);
            //if (list == null)
            //{
            //    int totalRecord = 0;
            //    list = GetCommentList(rowCount, 1, out totalRecord, 1, -1, -1, 0, (int)ApprovedStatus.Success, -1, string.Empty);

            //    Caches.Add(cacheKey, list);
            //}
            //return list;
        }

        /// <summary>
        /// 获取评论
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="parentId"></param>
        /// <param name="approved"></param>
        /// <param name="emailStatus"></param>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public static List<CommentInfo> GetCommentList(int pageSize, int pageIndex, out int totalRecord, int order, int userId, int postId, int parentId, int approved, int emailNotify, string keyword)
        {
            List<CommentInfo> list = dao.GetCommentList(pageSize, pageIndex, out totalRecord, order, userId, postId, parentId, approved, emailNotify, keyword);

            int floor = 1;
            foreach (CommentInfo comment in list)
            {

                comment.Floor = pageSize * (pageIndex - 1) + floor;
                floor++;
            }
            return list;
        }

        /// <summary>
        /// 根据日志ID删除评论
        /// </summary>
        /// <param name="postId">日志ID</param>
        /// <returns></returns>
        public static int DeleteCommentByPost(int postId)
        {
            int result = dao.DeleteCommentByPost(postId);

            StatisticsManager.UpdateStatisticsCommentCount(-result);

            _recentcomments = null;

            return result;
        }

        /// <summary>
        /// 统计评论数
        /// </summary>
        /// <param name="incChild"></param>
        /// <returns></returns>
        public static int GetCommentCount(bool incChild)
        {
            return GetCommentCount(-1, incChild);
        }

        /// <summary>
        /// 统计评论数
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        public static int GetCommentCount(int postId, bool incChild)
        {
            return GetCommentCount(-1, postId, incChild);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="incChild"></param>
        /// <returns></returns>
        public static int GetCommentCount(int userId, int postId, bool incChild)
        {

            return dao.GetCommentCount(userId, postId, incChild);
        }

    }
}
