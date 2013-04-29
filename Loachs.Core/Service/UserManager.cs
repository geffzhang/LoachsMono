using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    /// 用户管理
    /// </summary>
    public class UserManager
    {

        private static IUser dao = DataAccess.CreateUser();

        //  private static readonly string CacheKey = "users";

        /// <summary>
        /// 列表
        /// </summary>
        private static List<UserInfo> _users;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static UserManager()
        {
            LoadUser();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void LoadUser()
        {
            if (_users == null)
            {
                lock (lockHelper)
                {
                    if (_users == null)
                    {
                        _users = dao.GetUserList();

                        //   BuildUser();
                    }
                }
            }
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public static int InsertUser(UserInfo _userinfo)
        {
            _userinfo.UserId = dao.InsertUser(_userinfo);
            _users.Add(_userinfo);
            _users.Sort();

            return _userinfo.UserId;
        }

        /// <summary>
        /// 修改用户
        /// </summary>
        /// <param name="_userinfo"></param>
        /// <returns></returns>
        public static int UpdateUser(UserInfo _userinfo)
        {
            _users.Sort();
            return dao.UpdateUser(_userinfo);
        }

        /// <summary>
        /// 更新用户文章数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdateUserPostCount(int userId, int addCount)
        {
            UserInfo user = GetUser(userId);
            if (user != null)
            {
                user.PostCount += addCount;
                return UpdateUser(user);
            }
            return 0;
        }

        /// <summary>
        /// 更新用户评论数
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="addCount"></param>
        /// <returns></returns>
        public static int UpdateUserCommentCount(int userId, int addCount)
        {
            UserInfo user = GetUser(userId);
            if (user != null)
            {
                user.CommentCount += addCount;

                return UpdateUser(user);
            }
            return 0;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static int DeleteUser(int userId)
        {
            UserInfo user = GetUser(userId);
            if (user != null)
            {
                _users.Remove(user);
            }

            return dao.DeleteUser(userId);
        }


        /// <summary>
        /// 获取全部用户
        /// </summary>
        /// <returns></returns>
        public static List<UserInfo> GetUserList()
        {
            return _users;
        }

        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static bool ExistsUserName(string userName)
        {
            return dao.ExistsUserName(userName);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public static UserInfo GetUser(int userId)
        {
            foreach (UserInfo user in _users)
            {
                if (user.UserId == userId)
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据用户名获取用户 
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static UserInfo GetUser(string userName)
        {

            foreach (UserInfo user in _users)
            {
                if (user.UserName.ToLower() == userName.ToLower())
                {
                    return user;
                }
            }
            return null;
        }

        /// <summary>
        /// 根据用户名和密码获取用户
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static UserInfo GetUser(string userName, string password)
        {
            foreach (UserInfo user in _users)
            {
                if (  user.UserName.ToLower() == userName.ToLower() && user.Password.ToLower() == password.ToLower())
                {
                    return user;
                }
            }
            return null;
        }
    }
}
