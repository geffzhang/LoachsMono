using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

using Loachs.Entity;
using Loachs.Data;

namespace Loachs.Core.Access
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
            OleDbParameter[] prams = {
					                        OleDbHelper.MakeInParam("@PostCount", OleDbType.Integer,4,statistics.PostCount),
					                        OleDbHelper.MakeInParam("@CommentCount", OleDbType.Integer,4,statistics.CommentCount),
					                        OleDbHelper.MakeInParam("@VisitCount", OleDbType.Integer,4,statistics.VisitCount),
					                        OleDbHelper.MakeInParam("@TagCount", OleDbType.Integer,4,statistics.TagCount),
                                        };

            return OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams) == 1;
        }

        public StatisticsInfo GetStatistics()
        {
            string cmdText = "select top 1 * from [loachs_sites]";

            string insertText = "insert into [loachs_sites] ([PostCount],[CommentCount],[VisitCount],[TagCount],[setting]) values ( '0','0','0','0','<?xml version=\"1.0\" encoding=\"utf-8\"?><SettingInfo xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"></SettingInfo>')";

            List<StatisticsInfo> list = DataReaderToList(OleDbHelper.ExecuteReader(cmdText));

            if (list.Count == 0)
            {
                OleDbHelper.ExecuteNonQuery(insertText);
            }
            list = DataReaderToList(OleDbHelper.ExecuteReader(cmdText));

            return list.Count > 0 ? list[0] : null;
        }

        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="read">OleDbDataReader</param>
        /// <returns>TermInfo</returns>
        private static List<StatisticsInfo> DataReaderToList(OleDbDataReader read)
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
