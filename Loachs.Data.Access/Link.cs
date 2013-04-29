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
    public class Link : ILink
    {
        public int InsertLink(LinkInfo link)
        {
            string cmdText = @"insert into [loachs_links]
                            (
                            [type],[name],[href],[position],[target],[description],[displayorder],[status],[createdate]
                            )
                            values
                            (
                            @type,@name,@href,@position,@target,@description,@displayorder,@status,@createdate
                            )";
            SqliteParameter[] prams = { 
                                SqliteDbHelper.MakeInParam("@type",DbType.Int32,4,link.Type),
								SqliteDbHelper.MakeInParam("@name",DbType.String,100,link.Name),
                                SqliteDbHelper.MakeInParam("@href",DbType.String,255,link.Href),
                                SqliteDbHelper.MakeInParam("@position",DbType.Int32,4,link.Position),
                                SqliteDbHelper.MakeInParam("@target",DbType.String,50,link.Target),
								SqliteDbHelper.MakeInParam("@description",DbType.String,255,link.Description),
                                SqliteDbHelper.MakeInParam("@displayorder",DbType.Int32,4,link.Displayorder),
								SqliteDbHelper.MakeInParam("@status",DbType.Int32,4,link.Status),
								SqliteDbHelper.MakeInParam("@createdate",DbType.Date,8,link.CreateDate),
							};

            int r = SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            if (r > 0)
            {
                return Convert.ToInt32(SqliteDbHelper.ExecuteScalar("select   [linkid] from [loachs_links]  order by [linkid] desc limit 1"));
            }
            return 0;
        }

        public int UpdateLink(LinkInfo link)
        {
            string cmdText = @"update [loachs_links] set
                                [type]=@type,
                                [name]=@name,
                                [href]=@href,
                                [position]=@position,
                                [target]=@target,
                                [description]=@description,
                                [displayorder]=@displayorder,
                                [status]=@status,
                                [createdate]=@createdate
                                where linkid=@linkid";
            SqliteParameter[] prams = { 
                                SqliteDbHelper.MakeInParam("@type",DbType.Int32,4,link.Type),
								SqliteDbHelper.MakeInParam("@name",DbType.String,100,link.Name),
                                SqliteDbHelper.MakeInParam("@href",DbType.String,255,link.Href),
                                SqliteDbHelper.MakeInParam("@position",DbType.Int32,4,link.Position),
                                SqliteDbHelper.MakeInParam("@target",DbType.String,50,link.Target),
								SqliteDbHelper.MakeInParam("@description",DbType.String,255,link.Description),
                                SqliteDbHelper.MakeInParam("@displayorder",DbType.Int32,4,link.Displayorder),
								SqliteDbHelper.MakeInParam("@status",DbType.Int32,4,link.Status),
								SqliteDbHelper.MakeInParam("@createdate",DbType.Date,8,link.CreateDate),
                                SqliteDbHelper.MakeInParam("@linkid",DbType.Int32,4,link.LinkId),
							};

            return Convert.ToInt32(SqliteDbHelper.ExecuteScalar(CommandType.Text, cmdText, prams));
        }

        public int DeleteLink(int linkId)
        {
            string cmdText = "delete from [loachs_links] where [linkid] = @linkid";
            SqliteParameter[] prams = { 
								SqliteDbHelper.MakeInParam("@linkid",DbType.Int32,4,linkId)
							};
            return SqliteDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);


        }

        //public LinkInfo GetLink(int linkid)
        //{
        //    string cmdText = "select * from [loachs_links] where [linkid] = @linkid";
        //    SqliteParameter[] prams = { 
        //                        SqliteDbHelper.MakeInParam("@linkid",DbType.Int32,4,linkid)
        //                    };

        //    List<LinkInfo> list = DataReaderToList(SqliteDbHelper.ExecuteReader(CommandType.Text, cmdText, prams));
        //    return list.Count > 0 ? list[0] : null;
        //}


        //public List<LinkInfo> GetLinkList(int type, int position, int status)
        //{
        //    string condition = " 1=1 ";
        //    if (type != -1)
        //    {
        //        condition += " and [type]=" + type;
        //    }
        //    if (position != -1)
        //    {
        //        condition += " and [position]=" + position;
        //    }
        //    if (status != -1)
        //    {
        //        condition += " and [status]=" + status;
        //    }
        //    string cmdText = "select * from [loachs_links] where " + condition + "  order by [displayorder] asc";

        //    return DataReaderToList(SqliteDbHelper.ExecuteReader(cmdText));

        //}

        public List<LinkInfo> GetLinkList()
        {

            string cmdText = "select * from [loachs_links]  order by [displayorder] asc,[linkid] asc";

            return DataReaderToList(SqliteDbHelper.ExecuteReader(cmdText));

        }


        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="read">OleDbDataReader</param>
        /// <returns>LinkInfo</returns>
        private static List<LinkInfo> DataReaderToList(SqliteDataReader read)
        {
            List<LinkInfo> list = new List<LinkInfo>();
            while (read.Read())
            {
                LinkInfo link = new LinkInfo();
                link.LinkId = Convert.ToInt32(read["Linkid"]);
                link.Type = Convert.ToInt32(read["Type"]);
                link.Name = Convert.ToString(read["Name"]);
                link.Href = Convert.ToString(read["Href"]);
                if (read["Position"] != DBNull.Value)
                {
                    link.Position = Convert.ToInt32(read["Position"]);
                }

                link.Target = Convert.ToString(read["Target"]);
                link.Description = Convert.ToString(read["Description"]);
                link.Displayorder = Convert.ToInt32(read["Displayorder"]);
                link.Status = Convert.ToInt32(read["Status"]);
                link.CreateDate = Convert.ToDateTime(read["CreateDate"]);

                list.Add(link);
            }
            read.Close();
            return list;
        }
    }
}
