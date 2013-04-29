using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

using Loachs.Entity;
using Loachs.Data;

namespace Loachs.Core.Access
{
    public class User : IUser
    {
        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public int InsertUser(UserInfo _userinfo)
        {
            string cmdText = @" insert into [loachs_users](
                                [Type],[UserName],[Name],[Password],[Email],[SiteUrl],[AvatarUrl],[Description],[displayorder],[Status],[PostCount],[CommentCount],[CreateDate])
                                values (
                                @Type,@UserName,@Name,@Password,@Email,@SiteUrl,@AvatarUrl,@Description,@Displayorder,@Status, @PostCount,@CommentCount,@CreateDate )";
            OleDbParameter[] prams = { 
                                        OleDbHelper.MakeInParam("@Type", OleDbType.Integer,4, _userinfo.Type),
                                        OleDbHelper.MakeInParam("@UserName", OleDbType.VarWChar,50, _userinfo.UserName),
                                        OleDbHelper.MakeInParam("@Name", OleDbType.VarWChar,50, _userinfo.Name),
                                        OleDbHelper.MakeInParam("@Password", OleDbType.VarWChar,50, _userinfo.Password),
                                        OleDbHelper.MakeInParam("@Email", OleDbType.VarWChar,50, _userinfo.Email),
                                        OleDbHelper.MakeInParam("@SiteUrl", OleDbType.VarWChar,255, _userinfo.SiteUrl),
                                        OleDbHelper.MakeInParam("@AvatarUrl", OleDbType.VarWChar,255, _userinfo.AvatarUrl),
                                        OleDbHelper.MakeInParam("@Displayorder", OleDbType.VarWChar,255, _userinfo.Description),
                                        OleDbHelper.MakeInParam("@Status", OleDbType.Integer,4, _userinfo.Displayorder),
                                        OleDbHelper.MakeInParam("@Status", OleDbType.Integer,4, _userinfo.Status),                           
                                        OleDbHelper.MakeInParam("@PostCount", OleDbType.Integer,4, _userinfo.PostCount),
                                        OleDbHelper.MakeInParam("@CommentCount", OleDbType.Integer,4, _userinfo.CommentCount),
                                        OleDbHelper.MakeInParam("@CreateDate", OleDbType.Date,8, _userinfo.CreateDate),
                                        
                                    };
            int r = OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            if (r > 0)
            {
                return Convert.ToInt32(OleDbHelper.ExecuteScalar("select top 1 [UserId] from [loachs_users]  order by [UserId] desc"));
            }
            return 0;
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public int UpdateUser(UserInfo _userinfo)
        {
            string cmdText = @"update [loachs_users] set
                                [Type]=@Type,
                                [UserName]=@UserName,
                                [Name]=@Name,
                                [Password]=@Password,
                                [Email]=@Email,
                                [SiteUrl]=@SiteUrl,
                                [AvatarUrl]=@AvatarUrl,
                                [Description]=@Description,
                                [Displayorder]=@Displayorder,
                                [Status]=@Status,
                                [PostCount]=@PostCount,
                                [CommentCount]=@CommentCount,
                                [CreateDate]=@CreateDate
                                where UserId=@UserId";
            OleDbParameter[] prams = { 
                                        OleDbHelper.MakeInParam("@Type", OleDbType.Integer,4, _userinfo.Type),
                                        OleDbHelper.MakeInParam("@UserName", OleDbType.VarWChar,50, _userinfo.UserName),
                                        OleDbHelper.MakeInParam("@Name", OleDbType.VarWChar,50, _userinfo.Name),
                                        OleDbHelper.MakeInParam("@Password", OleDbType.VarWChar,50, _userinfo.Password),
                                        OleDbHelper.MakeInParam("@Email", OleDbType.VarWChar,50, _userinfo.Email),
                                        OleDbHelper.MakeInParam("@SiteUrl", OleDbType.VarWChar,255, _userinfo.SiteUrl),
                                        OleDbHelper.MakeInParam("@AvatarUrl", OleDbType.VarWChar,255, _userinfo.AvatarUrl),
                                        OleDbHelper.MakeInParam("@Description", OleDbType.VarWChar,255, _userinfo.Description),
                                        OleDbHelper.MakeInParam("@Displayorder", OleDbType.VarWChar,255, _userinfo.Displayorder),
                                        OleDbHelper.MakeInParam("@Status", OleDbType.Integer,4, _userinfo.Status),                           
                                        OleDbHelper.MakeInParam("@PostCount", OleDbType.Integer,4, _userinfo.PostCount),
                                        OleDbHelper.MakeInParam("@CommentCount", OleDbType.Integer,4, _userinfo.CommentCount),
                                        OleDbHelper.MakeInParam("@CreateDate", OleDbType.Date,8, _userinfo.CreateDate),
                                        OleDbHelper.MakeInParam("@UserId", OleDbType.Integer,4, _userinfo.UserId),
                                    };
            return OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        public int DeleteUser(int userid)
        {
            string cmdText = "delete from [loachs_users] where [userid] = @userid";
            OleDbParameter[] prams = { 
								        OleDbHelper.MakeInParam("@userid",OleDbType.Integer,4,userid)
							        };
            return OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
        }

       
        ///// <summary>
        ///// 获取实体
        ///// </summary>
        ///// <param name="userName"></param>
        ///// <param name="password"></param>
        ///// <returns></returns>
        //public UserInfo GetUser(string userName, string password)
        //{
        //    string cmdText = "select * from [loachs_users] where [userName] = @userName and [Password]=@password";
        //    OleDbParameter[] prams = { 
        //                        OleDbHelper.MakeInParam("@userName",OleDbType.VarWChar,50,userName),
        //                        OleDbHelper.MakeInParam("@password",OleDbType.VarWChar,50,password),
        //                    };
        //    List<UserInfo> list = DataReaderToUserList(OleDbHelper.ExecuteReader(CommandType.Text, cmdText, prams));
        //    if (list.Count > 0)
        //    {
        //        return list[0];
        //    }
        //    return null;
         
        //}

      

        /// <summary>
        /// 获取全部
        /// </summary>
        /// <returns></returns>
        public List<UserInfo> GetUserList()
        {
            string cmdText = "select * from [loachs_users]  order by [displayorder] asc,[userid] asc";
            return DataReaderToUserList(OleDbHelper.ExecuteReader(cmdText));
 
        }

        /// <summary>
        /// 数据转换
        /// </summary>
        /// <param name="read"></param>
        /// <returns></returns>
        private List<UserInfo> DataReaderToUserList(OleDbDataReader read)
        {
            List<UserInfo> list = new List<UserInfo>();
            while (read.Read())
            {
                UserInfo _userinfo = new UserInfo();
                _userinfo.UserId = Convert.ToInt32(read["UserId"]);
                _userinfo.Type = Convert.ToInt32(read["Type"]);
                _userinfo.UserName = Convert.ToString(read["UserName"]);
                _userinfo.Name = Convert.ToString(read["Name"]);
                _userinfo.Password = Convert.ToString(read["Password"]);
                _userinfo.Email = Convert.ToString(read["Email"]);
                _userinfo.SiteUrl = Convert.ToString(read["SiteUrl"]);
                _userinfo.AvatarUrl = Convert.ToString(read["AvatarUrl"]);
                _userinfo.Description = Convert.ToString(read["Description"]);
                _userinfo.Displayorder = Convert.ToInt32(read["Displayorder"]);
                _userinfo.Status = Convert.ToInt32(read["Status"]);
                _userinfo.PostCount = Convert.ToInt32(read["PostCount"]);
                _userinfo.CommentCount = Convert.ToInt32(read["CommentCount"]);
                _userinfo.CreateDate = Convert.ToDateTime(read["CreateDate"]);
             

                list.Add(_userinfo);
            }
            read.Close();
            return list;
        }

        
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public bool ExistsUserName(string userName)
        {
            string cmdText = "select count(1) from [loachs_users] where [userName] = @userName ";
            OleDbParameter[] prams = { 
                                        OleDbHelper.MakeInParam("@userName",OleDbType.VarWChar,50,userName),
							        };
            return Convert.ToInt32(OleDbHelper.ExecuteScalar(CommandType.Text, cmdText, prams)) > 0;
        }
    }
}
