using System;
using System.Collections.Generic;
using System.Text;
using Loachs.Common;
using Mono.Data.Sqlite;
using System.Data;
using System.Threading;

namespace Loachs.Data.Access
{
    public class SqliteDbHelper
    {
        public static string ConnectionString = string.Format("Data Source={0};Version=3", System.Web.HttpContext.Current.Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection));

        /// <summary>
        /// 查询次数统计
        /// </summary>
        private static int _querycount = 0;
        private static object lockobject = new object();

        /// <summary>
        /// 查询次数统计
        /// </summary>
        public static int QueryCount
        {
            get { return _querycount; }
            set { _querycount = value; }
        }



        #region MakeCommand
        ///// <summary>
        ///// 创建Command命令
        ///// </summary>
        ///// <param name="conn">数据连接</param>
        ///// <param name="cmdType">命令类型</param>
        ///// <param name="cmdText">SQL语句</param>
        ///// <returns>SqliteCommand</returns>
        //private static SqliteCommand MakeCommand(SqliteConnection conn, CommandType cmdType, string cmdText)
        //{
        //    if (conn.State != ConnectionState.Open)
        //    {
        //        conn.Open();
        //    }
        //    SqliteCommand cmd = new SqliteCommand();
        //    cmd.Connection = conn;
        //    cmd.CommandText = cmdText;
        //    cmd.CommandType = cmdType;
        //    return cmd;
        //}

        /// <summary>
        /// 创建Command命令
        /// </summary>
        /// <param name="conn">数据连接</param>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="prams">参数级</param>
        /// <returns></returns>
        private static SqliteCommand MakeCommand(SqliteConnection conn, CommandType cmdType, string cmdText, SqliteParameter[] prams)
        {
            if (conn.State != ConnectionState.Open)
            {
                conn.Open();
            }
            SqliteCommand cmd = new SqliteCommand();
            cmd.Connection = conn;
            cmd.CommandText = cmdText;
            cmd.CommandType = cmdType;
            if (prams != null)
            {
                foreach (SqliteParameter p in prams)
                {
                    cmd.Parameters.Add(p);
                }
            }
            return cmd;
        }
        #endregion

        #region MakeParam
        /// <summary>
        /// 生成输入参数
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        public static SqliteParameter MakeInParam(string ParamName, DbType dbType, int Size, object Value)
        {
            return MakeParam(ParamName, dbType, Size, ParameterDirection.Input, Value);
        }

        /// <summary>
        /// 生成输出参数
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <returns>New parameter.</returns>
        public static SqliteParameter MakeOutParam(string ParamName, DbType dbType, int Size)
        {
            return MakeParam(ParamName, dbType, Size, ParameterDirection.Output, null);
        }

        /// <summary>
        /// 生成返回参数,我添加
        /// </summary>
        /// <param name="ParamName"></param>
        /// <param name="DbType"></param>
        /// <param name="Size"></param>
        /// <returns></returns>
        public static SqliteParameter MakeReturnParam(string ParamName, DbType dbType, int Size)
        {
            return MakeParam(ParamName, dbType, Size, ParameterDirection.ReturnValue, null);
        }

        /// <summary>
        /// 生成各种参数
        /// </summary>
        /// <param name="ParamName">Name of param.</param>
        /// <param name="DbType">Param type.</param>
        /// <param name="Size">Param size.</param>
        /// <param name="Direction">Parm direction.</param>
        /// <param name="Value">Param value.</param>
        /// <returns>New parameter.</returns>
        private static SqliteParameter MakeParam(string ParamName, DbType dbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqliteParameter param;

            if (Size > 0)
                param = new SqliteParameter(ParamName, dbType, Size);
            else
                param = new SqliteParameter(ParamName, dbType);

            param.Direction = Direction;
            if (Direction == ParameterDirection.Input && Value != null)
            {
                param.Value = Value;
            }
            return param;
        }
        #endregion

        #region ExecuteScalar
        /// <summary>
        /// 返回查询结果的第一行的第一列,忽略其他列或行
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(string cmdText)
        {
            return ExecuteScalar(CommandType.Text, cmdText);
        }

        /// <summary>
        /// 返回查询结果的第一行的第一列,忽略其他列或行
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText)
        {

            return ExecuteScalar(cmdType, cmdText, null);
        }

        /// <summary>
        /// 返回查询结果的第一行的第一列,忽略其他列或行
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="prams">参数组</param>
        /// <returns>object</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, SqliteParameter[] prams)
        {
            _querycount++;
            System.Web.HttpContext.Current.Application["total"] = Convert.ToInt32(System.Web.HttpContext.Current.Application["total"]) + 1;
            using (SqliteConnection conn = new SqliteConnection(ConnectionString))
            {
                SqliteCommand cmd = MakeCommand(conn, cmdType, cmdText, prams);
                object obj = cmd.ExecuteScalar();
                cmd.Parameters.Clear();
                return obj;
            }
        }

        #endregion

        #region ExecuteNonQuery
        /// <summary>
        /// 返回受影响的行数,常用于Update和Delete 语句
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>int</returns>
        public static int ExecuteNonQuery(string cmdText)
        {
            return ExecuteNonQuery(CommandType.Text, cmdText);
        }

        /// <summary>
        /// 返回受影响的行数,常用于Update和Delete 语句
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>int</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText)
        {
            return ExecuteNonQuery(cmdType, cmdText, null);
        }

        /// <summary>
        /// 返回受影响的行数,常用于Insert,Update和Delete 语句
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="prams">参数组</param>
        /// <returns>int</returns>
        public static int ExecuteNonQuery(CommandType cmdType, string cmdText, SqliteParameter[] prams)
        {
            _querycount++;
            System.Web.HttpContext.Current.Application["total"] = Convert.ToInt32(System.Web.HttpContext.Current.Application["total"]) + 1;
            Monitor.Enter(lockobject);
            using (SqliteConnection conn = new SqliteConnection(ConnectionString))
            {
                SqliteCommand cmd = MakeCommand(conn, cmdType, cmdText, prams);
                int i = cmd.ExecuteNonQuery();
                Monitor.Exit(lockobject);
                cmd.Parameters.Clear();
                return i;
            }
        }

        #endregion

        #region ExecuteReader
        /// <summary>
        /// 返回 SqliteDataReader
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>SqliteDataReader</returns>
        public static SqliteDataReader ExecuteReader(string cmdText)
        {
            return ExecuteReader(CommandType.Text, cmdText);
        }

        [Obsolete]
        /// <summary>
        /// 返回 SqliteDataReader
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>SqliteDataReader</returns>
        public static SqliteDataReader ExecuteReader(CommandType cmdType, string cmdText)
        {
            return ExecuteReader(cmdType, cmdText, null);
        }

        /// <summary>
        /// 返回 SqliteDataReader
        /// </summary>
        /// <param name="cmdText"></param>
        /// <param name="prams"></param>
        /// <returns></returns>
        public static SqliteDataReader ExecuteReader(string cmdText, SqliteParameter[] prams)
        {
            return ExecuteReader(CommandType.Text, cmdText, prams);
        }

        /// <summary>
        /// 返回 SqliteDataReader
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="prams">参数组</param>
        /// <returns></returns>
        public static SqliteDataReader ExecuteReader(CommandType cmdType, string cmdText, SqliteParameter[] prams)
        {

            _querycount++;

            System.Web.HttpContext.Current.Application["total"] = Convert.ToInt32(System.Web.HttpContext.Current.Application["total"]) + 1;

            SqliteConnection conn = new SqliteConnection(ConnectionString);
            SqliteCommand cmd = MakeCommand(conn, cmdType, cmdText, prams);
            SqliteDataReader read = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            cmd.Parameters.Clear();
            return read;
        }

        #endregion

        #region ExecuteDataSet
        /// <summary>
        /// 返回 DataSet
        /// </summary>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(string cmdText)
        {
            return ExecuteDataSet(CommandType.Text, cmdText, null);
        }
        /// <summary>
        /// 返回 DataSet
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText)
        {
            //using (SqliteConnection conn = new SqliteConnection(ConnectionString))
            //{
            //    SqliteCommand cmd = MakeCommand(conn, cmdType, cmdText);
            //    SqliteDataAdapter apt = new SqliteDataAdapter(cmd);
            //    DataSet ds = new DataSet();
            //    apt.Fill(ds);
            //    cmd.Parameters.Clear();
            //    return ds;
            //}
            return ExecuteDataSet(cmdType, cmdText, null);
        }

        /// <summary>
        /// 返回 DataSet
        /// </summary>
        /// <param name="cmdType">命令类型</param>
        /// <param name="cmdText">SQL语句或存储过程名</param>
        /// <param name="prams">参数组</param>
        /// <returns>DataSet</returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, SqliteParameter[] prams)
        {
            _querycount++;
            System.Web.HttpContext.Current.Application["total"] = Convert.ToInt32(System.Web.HttpContext.Current.Application["total"]) + 1;

            using (SqliteConnection conn = new SqliteConnection(ConnectionString))
            {
                SqliteCommand cmd = MakeCommand(conn, cmdType, cmdText, prams);
                SqliteDataAdapter apt = new SqliteDataAdapter(cmd);
                DataSet ds = new DataSet();
                apt.Fill(ds);
                cmd.Parameters.Clear();
                return ds;
            }
        }

        #endregion

        /// <summary>
        /// 获取分页Sql
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="colName"></param>
        /// <param name="colList"></param>
        /// <param name="pageSize"></param>
        /// <param name="pageIndex"></param>
        /// <param name="orderBy"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static string GetPageSql(string tableName, string colName, string colList, int pageSize, int pageIndex, int orderBy, string condition)
        {
            string temp = string.Empty;
            string sql = string.Empty;
            if (string.IsNullOrEmpty(condition))
            {
                condition = " 1=1 ";
            }

            //降序
            if (orderBy == 1)
            {
                temp = "select  {1} from {2} where {5} and {3}   order by {3} desc limit {4} offset  {0} ";
                sql = string.Format(temp, pageSize, colList, tableName, colName, pageSize * (pageIndex - 1), condition);
            }
            //降序
            if (orderBy == 0)
            {
                temp = "select  {1} from {2} where {5} and {3}   order by {3} asc limit {4} offset  {0} ";
                sql = string.Format(temp, pageSize, colList, tableName, colName, pageSize * (pageIndex - 1), condition);
            }
            //第一页
            if (pageIndex == 1)
            {
                temp = "select  {1} from {2} where {3} order by {4} {5} limit {0}";
                sql = string.Format(temp, pageSize, colList, tableName, condition, colName, orderBy == 1 ? "desc" : "asc");
            }

            return sql;

        }
    }
}