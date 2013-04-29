using System;
using System.Collections.Generic;
using System.Text;
using System.Data.OleDb;
using System.Data;

using Loachs.Entity;
using Loachs.Data;

namespace Loachs.Core.Access
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
            OleDbParameter[] prams = { 
                                OleDbHelper.MakeInParam("@type",OleDbType.Integer,4,link.Type),
								OleDbHelper.MakeInParam("@name",OleDbType.VarWChar,100,link.Name),
                                OleDbHelper.MakeInParam("@href",OleDbType.VarWChar,255,link.Href),
                                OleDbHelper.MakeInParam("@position",OleDbType.Integer,4,link.Position),
                                OleDbHelper.MakeInParam("@target",OleDbType.VarWChar,50,link.Target),
								OleDbHelper.MakeInParam("@description",OleDbType.VarWChar,255,link.Description),
                                OleDbHelper.MakeInParam("@displayorder",OleDbType.Integer,4,link.Displayorder),
								OleDbHelper.MakeInParam("@status",OleDbType.Integer,4,link.Status),
								OleDbHelper.MakeInParam("@createdate",OleDbType.Date,8,link.CreateDate),
							};

            int r = OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);
            if (r > 0)
            {
                return Convert.ToInt32(OleDbHelper.ExecuteScalar("select top 1 [linkid] from [loachs_links]  order by [linkid] desc"));
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
            OleDbParameter[] prams = { 
                                OleDbHelper.MakeInParam("@type",OleDbType.Integer,4,link.Type),
								OleDbHelper.MakeInParam("@name",OleDbType.VarWChar,100,link.Name),
                                OleDbHelper.MakeInParam("@href",OleDbType.VarWChar,255,link.Href),
                                OleDbHelper.MakeInParam("@position",OleDbType.Integer,4,link.Position),
                                OleDbHelper.MakeInParam("@target",OleDbType.VarWChar,50,link.Target),
								OleDbHelper.MakeInParam("@description",OleDbType.VarWChar,255,link.Description),
                                OleDbHelper.MakeInParam("@displayorder",OleDbType.Integer,4,link.Displayorder),
								OleDbHelper.MakeInParam("@status",OleDbType.Integer,4,link.Status),
								OleDbHelper.MakeInParam("@createdate",OleDbType.Date,8,link.CreateDate),
                                OleDbHelper.MakeInParam("@linkid",OleDbType.Integer,4,link.LinkId),
							};

            return Convert.ToInt32(OleDbHelper.ExecuteScalar(CommandType.Text, cmdText, prams));
        }

        public int DeleteLink(int linkId)
        {
            string cmdText = "delete from [loachs_links] where [linkid] = @linkid";
            OleDbParameter[] prams = { 
								OleDbHelper.MakeInParam("@linkid",OleDbType.Integer,4,linkId)
							};
            return OleDbHelper.ExecuteNonQuery(CommandType.Text, cmdText, prams);


        }

        //public LinkInfo GetLink(int linkid)
        //{
        //    string cmdText = "select * from [loachs_links] where [linkid] = @linkid";
        //    OleDbParameter[] prams = { 
        //                        OleDbHelper.MakeInParam("@linkid",OleDbType.Integer,4,linkid)
        //                    };

        //    List<LinkInfo> list = DataReaderToList(OleDbHelper.ExecuteReader(CommandType.Text, cmdText, prams));
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

        //    return DataReaderToList(OleDbHelper.ExecuteReader(cmdText));

        //}

        public List<LinkInfo> GetLinkList()
        {

            string cmdText = "select * from [loachs_links]  order by [displayorder] asc,[linkid] asc";

            return DataReaderToList(OleDbHelper.ExecuteReader(cmdText));

        }


        /// <summary>
        /// 转换实体
        /// </summary>
        /// <param name="read">OleDbDataReader</param>
        /// <returns>LinkInfo</returns>
        private static List<LinkInfo> DataReaderToList(OleDbDataReader read)
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
