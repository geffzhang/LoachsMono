using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

using Loachs.Entity;
using Loachs.Data;
using Mono.Data.Sqlite;

namespace Loachs.Data.Access
{
    public class Statistics : IStatistics
    {
        public bool UpdateStatistics(StatisticsInfo statistics)
        {
            string cmdText = @"update [loachs_sites] set 
                                PostCount=@PostCount,
                                CommentCount=@CommentCount,
                                VisitCount=@VisitCount,
                                TagCount=@TagCount";
            SqliteParameter[] prams = {
					                        SqliteDbHelper.MakeInParam("@PostCount", DbType.Int32,4,statistics.PostCount),
					                        SqliteDbHelper.MakeInParam("@CommentCount", DbType.Int32,4,statistics.CommentCount),
					                        SqliteDbHelper.MakeInParam("@VisitCount", DbType.Int32,4,statistics.VisitCount),
					                        SqliteDbHelper.MakeInParam("@TagCount", DbType.Int32,4,statistics.TagCount),
                                        };

            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams) == 1;
        }

        public StatisticsInfo GetStatistics()
        {
            string cmdText = "select  * from [loachs_sites] limit 1";

            string insertText = "insert into [loachs_sites] ([PostCount],[CommentCount],[VisitCount],[TagCount],[setting]) values ( '0','0','0','0','<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></SettingInfo>')";

            List<StatisticsInfo> list = DataReaderToList(SqliteDbHelper.ExecuteReader(cmdText));

            if (list.Count == 0)
            {
                SqliteDbHelper.ExecuteNonQuery(insertText);
            }
            list = DataReaderToList(SqliteDbHelper.ExecuteReader(cmdText));

            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="read">OleDbDataReader</param>
        /// <returns>TermInfo</returns>
        private static List<StatisticsInfo> DataReaderToList(SqliteDataReader read)
        {
            List<StatisticsInfo> list = new List<StatisticsInfo>();
            while (read.Read())
            {
                StatisticsInfo _site = new StatisticsInfo();

                _site.PostCount = Convert.ToInt32(read["PostCount"]);
                _site.CommentCount = Convert.ToInt32(read["CommentCount"]);
                _site.VisitCount = Convert.ToInt32(read["VisitCount"]);
                _site.TagCount = Convert.ToInt32(read["TagCount"]);

                list.Add(_site);
            }
            read.Close();
            return list;
        }
    }
}
