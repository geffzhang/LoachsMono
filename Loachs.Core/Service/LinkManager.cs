using System;
using System.Collections.Generic;
using System.Text;

using Loachs.Common;
using Loachs.Data;
using Loachs.Entity;

namespace Loachs.Business
{
    /// <summary>
    /// 连接管理
    /// </summary>
    public class LinkManager
    {
        static ILink dao = DataAccess.CreateLink();

        /// <summary>
        /// 列表
        /// </summary>
        private static List<LinkInfo> _links;

        /// <summary>
        /// lock
        /// </summary>
        private static object lockHelper = new object();

        static LinkManager()
        {
            LoadLink();
        }


        /// <summary>
        /// 初始化
        /// </summary>
        public static void LoadLink()
        {
            if (_links == null)
            {
                lock (lockHelper)
                {
                    if (_links == null)
                    {
                        _links = dao.GetLinkList();

                    }
                }
            }
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static int InsertLink(LinkInfo link)
        {
            link.LinkId = dao.InsertLink(link);
            _links.Add(link);
            _links.Sort();

            return link.LinkId;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="link"></param>
        /// <returns></returns>
        public static int UpdateLink(LinkInfo link)
        {
            _links.Sort();
            return dao.UpdateLink(link);
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="termid"></param>
        /// <returns></returns>
        public static int DeleteLink(int linkId)
        {

            LinkInfo link = GetLink(linkId);
            if (link != null)
            {
                _links.Remove(link);
            }
            return dao.DeleteLink(linkId);
        }

        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="linkId"></param>
        /// <returns></returns>
        public static LinkInfo GetLink(int linkId)
        {

            foreach (LinkInfo l in _links)
            {
                if (l.LinkId == linkId)
                {
                    return l;
                }
            }
            return null;
        }

        /// <summary>
        /// 获取连接列表
        /// </summary>
        /// <param name="position"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<LinkInfo> GetLinkList(int position, int status)
        {
            return GetLinkList(-1, position, status);
        }

        /// <summary>
        /// 获取连接列表
        /// </summary>
        /// <param name="type"></param>
        /// <param name="position"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static List<LinkInfo> GetLinkList(int type, int position, int status)
        {
            List<LinkInfo> list = _links;

            if (type != -1)
            {
                list = list.FindAll(delegate(LinkInfo link) { return link.Type == type; });
            }

            if (position != -1)
            {
                list = list.FindAll(delegate(LinkInfo link) { return link.Position == position; });
            }

            if (status != -1)
            {
                list = list.FindAll(delegate(LinkInfo link) { return link.Status == status; });
            }

            return list;
        }

        /// <summary>
        /// 获取连接列表
        /// </summary>
        /// <returns></returns>
        public static List<LinkInfo> GetLinkList()
        {
            return _links;
        }
    }
}
