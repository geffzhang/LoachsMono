using System;
using System.Collections.Generic;

using Loachs.Data;
using Loachs.Entity;
using Loachs.Common;

namespace Loachs.Business
{
    /// <summary>
    /// 统计管理
    /// </summary>
    public static class StatisticsManager
    {
        private static IStatistics dao = DataAccess.CreateStatistics();

        /// <summary>
        /// 缓存统计
        /// </summary>
        private static StatisticsInfo _statistics;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static StatisticsManager()
        {
            LoadStatistics();
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private static void LoadStatistics()
        {
            if (_statistics == null)
            {
                lock (lockHelper)
                {
                    if (_statistics == null)
                    {
                        _statistics = dao.GetStatistics();
                    }
                }
            }
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <returns></returns>
        public static StatisticsInfo GetStatistics()
        {
            return _statistics;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <returns></returns>
        public static bool UpdateStatistics()
        {
            return dao.UpdateStatistics(_statistics);
        }

        /// <summary>
        /// 更新文章数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static bool UpdateStatisticsPostCount(int addCount)
        {
            _statistics.PostCount += addCount;
            return UpdateStatistics();
        }

        /// <summary>
        /// 更新评论数
        /// </summary>
        /// <param name="addCount">增加数，可为负数</param>
        /// <returns></returns>
        public static bool UpdateStatisticsCommentCount(int addCount)
        {
            _statistics.CommentCount += addCount;
            return UpdateStatistics();
        }
    }
}
