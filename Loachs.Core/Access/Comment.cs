using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

using Loachs.Entity;
using Loachs.Data;

namespace Loachs.Core.Access
{
    public class Comment : IComment
    {

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int InsertComment(CommentInfo comment)
        {
            string cmdText = @"insert into [loachs_comments](
                            PostId, ParentId,UserId,Name,Email,SiteUrl,Content,EmailNotify,IpAddress,CreateDate,Approved)
                             values (
                            @PostId, @ParentId,@UserId,@Name,@Email,@SiteUrl,@Content,@EmailNotify,@IpAddress,@CreateDate,@Approved)";

            OleDbParameter[] prams = { 
                                        OleDbHelper.MakeInParam("@PostId", OleDbType.Integer,4, comment.PostId),
                                        OleDbHelper.MakeInParam("@ParentId", OleDbType.Integer,4, comment.ParentId),
                                        OleDbHelper.MakeInParam("@UserId", OleDbType.Integer,4, comment.UserId),
                                        OleDbHelper.MakeInParam("@Name", OleDbType.VarWChar,255, comment.Name),
                                        OleDbHelper.MakeInParam("@Email", OleDbType.VarWChar,255, comment.Email),
                                        OleDbHelper.MakeInParam("@SiteUrl", OleDbType.VarWChar,255, comment.SiteUrl),
                                        OleDbHelper.MakeInParam("@Content", OleDbType.VarWChar,255, comment.Content),
                                        OleDbHelper.MakeInParam("@EmailNotify", OleDbType.Integer,4 ,    comment.EmailNotify),
                                        OleDbHelper.MakeInParam("@IpAddress", OleDbType.VarWChar,255, comment.IpAddress),
                                        OleDbHelper.MakeInParam("@CreateDate", OleDbType.Date,8, comment.CreateDate),
                                        OleDbHelper.MakeInParam("@Approved", OleDbType.Integer,4 ,   comment.Approved),
            };
            OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);

            int newId = Convert.ToInt32(OleDbHelper.ExecuteScalar("select top 1 [CommentId] from [loachs_comments]  order by [CommentId] desc"));
            return newId;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="comment"></param>
        /// <returns></returns>
        public int UpdateComment(CommentInfo comment)
        {
            string cmdText = @"update [loachs_comments] set 
                            PostId=@PostId,                            
                            ParentId=@ParentId,
                            UserId=@UserId,
                            Name=@Name,
                            Email=@Email,
                            SiteUrl=@SiteUrl,
                            Content=@Content,
                            EmailNotify=@EmailNotify,
                            IpAddress=@IpAddress,
                            CreateDate=@CreateDate,
                            Approved=@Approved
                            where CommentId=@CommentId ";

            OleDbParameter[] prams = { 
                                        OleDbHelper.MakeInParam("@PostId", OleDbType.Integer,4, comment.PostId),
                                        OleDbHelper.MakeInParam("@ParentId", OleDbType.Integer,4, comment.ParentId),
                                        OleDbHelper.MakeInParam("@UserId", OleDbType.Integer,4, comment.UserId),
                                        OleDbHelper.MakeInParam("@Name", OleDbType.VarWChar,255, comment.Name),
                                        OleDbHelper.MakeInParam("@Email", OleDbType.VarWChar,255, comment.Email),
                                        OleDbHelper.MakeInParam("@SiteUrl", OleDbType.VarWChar,255, comment.SiteUrl),
                                        OleDbHelper.MakeInParam("@Content", OleDbType.VarWChar,255, comment.Content),
                                        OleDbHelper.MakeInParam("@EmailNotify", OleDbType.Integer,4 ,    comment.EmailNotify),
                                        OleDbHelper.MakeInParam("@IpAddress", OleDbType.VarWChar,255, comment.IpAddress),
                                        OleDbHelper.MakeInParam("@CreateDate", OleDbType.Date,8, comment.CreateDate),
                                        OleDbHelper.MakeInParam("@Approved", OleDbType.Integer,4 ,   comment.Approved),
                                        OleDbHelper.MakeInParam("@CommentId", OleDbType.Integer,4, comment.CommentId),

                                    };
            return OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);

        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public int DeleteComment(int commentId)
        {
            CommentInfo comment = GetComment(commentId);        //删除前

            string cmdText = "delete from [loachs_comments] where [commentId] = @commentId";
            OleDbParameter[] prams = { 
								OleDbHelper.MakeInParam("@commentId",OleDbType.Integer,4,commentId)
							};

            int result = OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            return result;
        }

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <param name="commentId"></param>
        /// <returns></returns>
        public CommentInfo GetComment(int commentId)
        {
            string cmdText = "select * from [loachs_comments] where [commentId] = @commentId";
            OleDbParameter[] prams = { 
								        OleDbHelper.MakeInParam("@commentId",OleDbType.Integer,4,commentId)
							          };
            List<CommentInfo> list = DataReaderToCommentList(OleDbHelper.ExecuteReader(cmdText, prams));

            return list.Count > 0 ? list[0] : null;
        }


        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="totalRecord"></param>
        /// <returns></returns>
        public List<CommentInfo> GetCommentList(int pageSize, int pageIndex, out int totalRecord, int order, int userId, int postId, int parentId, int approved, int emailNotify, string keyword)
        {
            string condition = " 1=1 ";// "[ParentId]=0 and [PostId]=" + postId;

            if (userId != -1)
            {
                condition += " and userid=" + userId;
            }
            if (postId != -1)
            {
                condition += " and postId=" + postId;
            }
            if (parentId != -1)
            {
                condition += " and parentId=" + parentId;
            }

            if (approved != -1)
            {
                condition += " and approved=" + approved;
            }

            if (emailNotify != -1)
            {
                condition += " and emailNotify=" + emailNotify;
            }

            if (!string.IsNullOrEmpty(keyword))
            {
                condition += string.Format(" and (content like '%{0}%' or author like '%{0}%' or ipaddress like '%{0}%' or email like '%{0}%'  or siteurl like '%{0}%' )", keyword);
            }

            string cmdTotalRecord = "select count(1) from [loachs_comments] where " + condition;
            totalRecord = Convert.ToInt32(OleDbHelper.ExecuteScalar(CommandType.Text, cmdTotalRecord));

            //   throw new Exception(cmdTotalRecord);

            string cmdText = OleDbHelper.GetPageSql("[loachs_comments]", "[CommentId]", "*", pageSize, pageIndex, order, condition);
            return DataReaderToCommentList(OleDbHelper.ExecuteReader(cmdText));
        }

        /// <summary>
        /// 根据日志ID删除评论
        /// </summary>
        /// <param name="postId">日志ID</param>
        /// <returns></returns>
        public int DeleteCommentByPost(int postId)
        {
            string cmdText = "delete from [loachs_comments] where [postId] = @postId";
            OleDbParameter[] prams = { 
								OleDbHelper.MakeInParam("@postId",OleDbType.Integer,4,postId)
							};
            int result = OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);

            return result;
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private List<CommentInfo> DataReaderToCommentList(OleDbDataReader read)
        {
            List<CommentInfo> list = new List<CommentInfo>();
            while (read.Read())
            {
                CommentInfo comment = new CommentInfo();
                comment.CommentId = Convert.ToInt32(read["CommentId"]);
                comment.ParentId = Convert.ToInt32(read["ParentId"]);
                comment.PostId = Convert.ToInt32(read["PostId"]);
                comment.UserId = Convert.ToInt32(read["UserId"]);
                comment.Name = Convert.ToString(read["Name"]);
                comment.Email = Convert.ToString(read["Email"]);
                comment.SiteUrl = Convert.ToString(read["SiteUrl"]);
                comment.Content = Convert.ToString(read["Content"]);
                comment.EmailNotify = Convert.ToInt32(read["EmailNotify"]);
                comment.IpAddress = Convert.ToString(read["IpAddress"]);
                comment.CreateDate = Convert.ToDateTime(read["CreateDate"]);
                comment.Approved = Convert.ToInt32(read["Approved"]);
                list.Add(comment);
            }
            read.Close();
            return list;
        }


        /// <summary>
        /// 统计评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="postId"></param>
        /// <param name="incChild"></param>
        /// <returns></returns>
        public int GetCommentCount(int userId, int postId, bool incChild)
        {
            string condition = " 1=1 ";
            if (userId != -1)
            {
                condition += " and [userId] = " + userId;
            }
            if (postId != -1)
            {
                condition += " and [postId] = " + postId;
            }
            if (incChild == false)
            {
                condition += " and [parentid]=0";
            }
            string cmdText = "select count(1) from [loachs_comments] where " + condition;
            return Convert.ToInt32(OleDbHelper.ExecuteScalar(CommandType.Text, cmdText));
        }
    }
}
