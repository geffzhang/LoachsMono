using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
using System.Runtime.InteropServices;
using System.Diagnostics;


namespace Loachs.Web
{

    public partial class admin_default : AdminPage
    {

        #region 变量
        /// <summary>
        /// 数据库大小
        /// </summary>
        protected string DbSize;

        /// <summary>
        /// 数据库路径
        /// </summary>
        protected string DbPath;

        /// <summary>
        /// 附件大小
        /// </summary>
        protected string UpfileSize;

        /// <summary>
        /// 附件路径
        /// </summary>
        protected string UpfilePath;

        /// <summary>
        /// 附件个数
        /// </summary>
        protected int UpfileCount = 0;

        /// <summary>
        /// 操作系统版本
        /// </summary>
        protected string OSVersion;

        /// <summary>
        /// IIS 版本
        /// </summary>
        protected string IISVersion;

        /// <summary>
        /// .net 版本
        /// </summary>
        protected string NETVersion;

        /// <summary>
        /// CPU信息
        /// </summary>
        protected string CPUInfo;

        /// <summary>
        /// 使用内存大小
        /// </summary>
        protected string MemoryInfo;

        //  protected MemoryInfo memoryInfo;  

        //   private string CommentMessage = "";

        protected List<CommentInfo> commentlist;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("首页");

            if (!IsPostBack)
            {
                CheckStatistics();

                commentlist = CommentManager.GetCommentList(15, 1, -1, -1, -1, (int)ApprovedStatus.Wait, -1, string.Empty);
                //rptComment.DataSource = list;
                //rptComment.DataBind();
                //if (list.Count == 0)
                //{
                //    CommentMessage = "暂时无待审核评论";
                //}


                DbPath = ConfigHelper.SitePath + ConfigHelper.DbConnection;
                System.IO.FileInfo file = new System.IO.FileInfo(Server.MapPath(ConfigHelper.SitePath + ConfigHelper.DbConnection));
                DbSize = GetFileSize(file.Length);

                UpfilePath = ConfigHelper.SitePath + "upfiles";

                GetDirectorySize(Server.MapPath(UpfilePath));

                UpfileSize = GetFileSize(dirSize);

                GetDirectoryCount(Server.MapPath(UpfilePath));

                GetSystemInfo();
            }

            ShowResult();


            if (string.IsNullOrEmpty(ResponseMessage))
            {
                if (PageUtils.IsIE6 || PageUtils.IsIE7)
                {
                    ShowError("小提示: 系统发现您正在使用老旧的浏览器(IE内核版本过低或启用了兼容模式) , 建议您升级或更换更标准更好体验的 <a href=\"http://www.microsoft.com/windows/internet-explorer/\" title=\"Microsoft Internet Explorer\" target=\"_blank\">IE8 +</a> , <a href=\"http://www.google.com/chrome?hl=zh-CN\" title=\"Google Chrome\" target=\"_blank\">Chrome</a> , <a href=\"http://www.mozillaonline.com/\" title=\"Mozilla Firefox\" target=\"_blank\">Firefox</a> , <a href=\"http://www.apple.com.cn/safari/\" title=\"Apple Safari\" target=\"_blank\">Safari</a>");
                }
            }
        }

        /// <summary>
        /// 显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = RequestHelper.QueryInt("result");
            switch (result)
            {
                case 11:
                    ShowMessage("统计分类文章数完成!");
                    break;
                case 12:
                    ShowMessage("统计标签文章数完成!");
                    break;
                case 13:
                    ShowMessage("统计作者文章和评论数完成!");
                    break;
                default:
                    break;
            }
        }

        protected void CheckStatistics()
        {
            StatisticsInfo s = StatisticsManager.GetStatistics();
            bool update = false;

            int totalPosts = PostManager.GetPostCount(-1, -1, -1, (int)PostStatus.Published, 0);
            if (totalPosts != s.PostCount)
            {
                s.PostCount = totalPosts;
                update = true;
            }

            int totalComments = CommentManager.GetCommentCount(true);
            if (totalComments != s.CommentCount)
            {
                s.CommentCount = totalComments;
                update = true;
            }
            int totalTags = TagManager.GetTagList().Count;
            if (totalTags != s.TagCount)
            {
                s.TagCount = totalTags;
                update = true;
            }
            if (update == true)
            {
                StatisticsManager.UpdateStatistics();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="size">byte</param>
        /// <returns></returns>
        protected string GetFileSize(long size)
        {
            string FileSize = string.Empty;
            if (size > (1024 * 1024 * 1024))
                FileSize = ((double)size / (1024 * 1024 * 1024)).ToString(".##") + " GB";
            else if (size > (1024 * 1024))
                FileSize = ((double)size / (1024 * 1024)).ToString(".##") + " MB";
            else if (size > 1024)
                FileSize = ((double)size / 1024).ToString(".##") + " KB";
            else if (size == 0)
                FileSize = "0 Byte";
            else
                FileSize = ((double)size / 1).ToString(".##") + " Byte";

            return FileSize;
        }

        /// <summary>
        /// 文件夹大小
        /// </summary>
        public long dirSize = 0;

        /// <summary>
        /// 递归文件夹大小
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        private long GetDirectorySize(string dirp)
        {
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    FileInfo fi = (FileInfo)fsi;
                    dirSize += fi.Length;
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo)fsi;
                    string new_dir = di.FullName;
                    GetDirectorySize(new_dir);
                }
            }
            return dirSize;
        }



        /// <summary>
        /// 递归文件数量
        /// </summary>
        /// <param name="dirp"></param>
        /// <returns></returns>
        private int GetDirectoryCount(string dirp)
        {
            DirectoryInfo mydir = new DirectoryInfo(dirp);
            foreach (FileSystemInfo fsi in mydir.GetFileSystemInfos())
            {
                if (fsi is FileInfo)
                {
                    //   FileInfo fi = (FileInfo)fsi;
                    UpfileCount += 1;
                }
                else
                {
                    DirectoryInfo di = (DirectoryInfo)fsi;
                    string new_dir = di.FullName;
                    GetDirectoryCount(new_dir);
                }
            }
            return UpfileCount;
        }

        protected void btnCategory_Click(object sender, EventArgs e)
        {
            List<CategoryInfo> list = CategoryManager.GetCategoryList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Count = PostManager.GetPostCount(list[i].CategoryId, -1, -1, (int)PostStatus.Published, 0);
                CategoryManager.UpdateCategory(list[i]);
            }
            Response.Redirect("default.aspx?result=11");
        }

        protected void btnTag_Click(object sender, EventArgs e)
        {
            List<TagInfo> list = TagManager.GetTagList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].Count = PostManager.GetPostCount(-1, list[i].TagId, -1, (int)PostStatus.Published, 0);
                TagManager.UpdateTag(list[i]);
            }
            Response.Redirect("default.aspx?result=12");
        }

        protected void btnUser_Click(object sender, EventArgs e)
        {

            List<UserInfo> list = UserManager.GetUserList();
            for (int i = 0; i < list.Count; i++)
            {
                list[i].PostCount = PostManager.GetPostCount(-1, -1, list[i].UserId, (int)PostStatus.Published, 0);
                list[i].CommentCount = CommentManager.GetCommentCount(list[i].UserId, -1, false);
                UserManager.UpdateUser(list[i]);
            }
            Response.Redirect("default.aspx?result=13");
        }

        /// <summary>
        /// 获取系统内存大小
        /// </summary>
        /// <returns>内存大小（单位GB）</returns>
        private static int GetPhisicalMemory()
        {
            //try
            //{
            //    ManagementObjectSearcher searcher = new ManagementObjectSearcher();   //用于查询一些如系统信息的管理对象 
            //    searcher.Query = new SelectQuery("Win32_PhysicalMemory ", "", new string[] { "Capacity" });//设置查询条件 
            //    ManagementObjectCollection collection = searcher.Get();   //获取内存容量 
            //    ManagementObjectCollection.ManagementObjectEnumerator em = collection.GetEnumerator();

            //    long capacity = 0;
            //    while (em.MoveNext())
            //    {
            //        ManagementBaseObject baseObj = em.Current;
            //        if (baseObj.Properties["Capacity"].Value != null)
            //        {
            //            try
            //            {
            //                capacity += long.Parse(baseObj.Properties["Capacity"].Value.ToString());
            //            }
            //            catch
            //            {
            //                return 0;
            //            }
            //        }
            //    }
            //    return (int)(capacity / 1024 / 1024 / 1024);
            //}
            //catch
            //{
            //    return 0;
            //}
            return 0;
        }



        ////定义内存的信息结构   
        //[StructLayout(LayoutKind.Sequential)]
        //public struct MEMORY_INFO
        //{
        //    public uint dwLength;
        //    public uint dwMemoryLoad;
        //    public uint dwTotalPhys;
        //    public uint dwAvailPhys;
        //    public uint dwTotalPageFile;
        //    public uint dwAvailPageFile;
        //    public uint dwTotalVirtual;
        //    public uint dwAvailVirtual;
        //}

        /// <summary>
        /// 获取系统信息
        /// </summary>
        protected void GetSystemInfo()
        {
            try
            {

                OSVersion = Environment.OSVersion.ToString();

                IISVersion = Request.ServerVariables["SERVER_SOFTWARE"];

                if (OSVersion.IndexOf("Microsoft Windows NT 5.0") > -1)
                {
                    OSVersion = string.Concat("Microsoft Windows 2000 (", OSVersion, ")");
                    IISVersion = "IIS 5";
                }
                else if (OSVersion.IndexOf("Microsoft Windows NT 5.1") > -1)
                {
                    OSVersion = string.Concat("Microsoft Windows XP (", OSVersion, ")");
                    IISVersion = "IIS 5.1";
                }
                else if (OSVersion.IndexOf("Microsoft Windows NT 5.2") > -1)
                {
                    OSVersion = string.Concat("Microsoft Windows 2003 (", OSVersion, ")");
                    IISVersion = "IIS 6";
                }
                else if (OSVersion.IndexOf("Microsoft Windows NT 6.0") > -1)
                {
                    OSVersion = string.Concat("Microsoft Windows Vista or Server 2008 (", OSVersion, ")");
                    IISVersion = "IIS 7";
                }
                else if (OSVersion.IndexOf("Microsoft Windows NT 6.1") > -1)
                {
                    OSVersion = string.Concat("Microsoft Windows 7 or Server 2008 R2 (", OSVersion, ")");
                    IISVersion = "IIS 7.5";
                }

                NETVersion = Environment.Version.ToString();

                CPUInfo = Environment.GetEnvironmentVariable("PROCESSOR_IDENTIFIER") + " (" + Environment.ProcessorCount + " 核)";


                //MEMORY_INFO MemInfo;
                //MemInfo = new MEMORY_INFO();
                //GlobalMemoryStatus(ref   MemInfo);
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    PerformanceCounter p = new PerformanceCounter("Mono Memory", "Total Physical Memory");
                    MemoryInfo = "物理内存:" + (p.RawValue / 1024 / 1024) + " MB / 当前程序已占用物理内存:" + (Environment.WorkingSet / 1024 / 1024).ToString() + " MB";

                }
            }
            catch(Exception  ex)
            {

            }


        }
    }
}