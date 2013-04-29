using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.IO;
using System.Text;
using System.Xml;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class admin_themelist : AdminPage
    {
        /// <summary>
        /// 主题名(文件夹名)
        /// </summary>
        protected string themename = RequestHelper.QueryString("themename", true);

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("主题管理");

            string[] filelist = Directory.GetFileSystemEntries(Server.MapPath("../themes/default"));
            foreach (string str in filelist)
            {
                //   Response.Write(str+"<br>");
            }
            //   System.IO.Directory.Delete(Server.MapPath("../themes/test"),true);

            string type = RequestHelper.QueryString("type", true);

            if (Operate == OperateType.Update)
            {
                SettingInfo s = SettingManager.GetSetting();

                switch (type)
                {
                    case "mobile":
                        s.MobileTheme = themename;
                        break;
                    case "pc":
                    default:
                        s.Theme = themename;
                        break;
                }
                SettingManager.UpdateSetting();

                Response.Redirect("themelist.aspx?result=2");
            }
            else if (Operate == OperateType.Insert)
            {
                string srcPath = Server.MapPath("../themes/" + themename);

                if (!string.IsNullOrEmpty(themename) && System.IO.Directory.Exists(srcPath))
                {
                    string aimPath = string.Empty;
                    int count = 1;
                    while (true)
                    {
                        count++;
                        aimPath = Server.MapPath("../themes/" + themename + "-" + count);
                        if (!System.IO.Directory.Exists(aimPath))
                        {
                            break;
                        }
                    }
                    CopyDir(srcPath, aimPath);
                    Response.Redirect("themelist.aspx?result=1");
                }
            }
            else if (Operate == OperateType.Delete)
            {
                if (themename == "default")
                {
                    Response.Redirect("themelist.aspx?result=5");
                }
                else
                {
                    string path = Server.MapPath("../themes/" + themename);
                    if (System.IO.Directory.Exists(path))
                    {
                        System.IO.Directory.Delete(path, true);
                        Response.Redirect("themelist.aspx?result=3");
                    }
                }
            }

            ShowResult();
        }

        /// <summary>
        /// 显示结果
        /// </summary>
        protected void ShowResult()
        {
            int result = RequestHelper.QueryInt("result");
            switch (result)
            {
                case 5:
                    ShowMessage("未允许的操作!");
                    break;
                case 1:
                    ShowMessage("主题复制成功!");
                    break;
                case 2:
                    ShowMessage("主题修改成功!");
                    break;
                case 3:
                    ShowMessage("主题删除成功!");
                    break;
                case 4:
                    ShowMessage("无法复制!");
                    break;
                default:
                    break;
            }
        }



        /// <summary>
        /// 将整个文件夹复制到目标文件夹中。
        /// </summary>
        /// <param name="srcPath">源文件夹</param>
        /// <param name="aimPath">目标文件夹</param>
        public void CopyDir(string srcPath, string aimPath)
        {
            try
            {
                // 检查目标目录是否以目录分割字符结束如果不是则添加之
                if (aimPath[aimPath.Length - 1] != Path.DirectorySeparatorChar)
                    aimPath += Path.DirectorySeparatorChar;
                // 判断目标目录是否存在如果不存在则新建之
                if (!Directory.Exists(aimPath))
                    Directory.CreateDirectory(aimPath);
                // 得到源目录的文件列表，该里面是包含文件以及目录路径的一个数组
                // 如果你指向copy目标文件下面的文件而不包含目录请使用下面的方法
                // string[] fileList = Directory.GetFiles(srcPath);
                string[] fileList = Directory.GetFileSystemEntries(srcPath);
                // 遍历所有的文件和目录
                foreach (string file in fileList)
                {
                    // 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                    if (Directory.Exists(file))
                        CopyDir(file, aimPath + Path.GetFileName(file));
                    // 否则直接Copy文件
                    else
                        File.Copy(file, aimPath + Path.GetFileName(file), true);
                }
            }
            catch
            {
                Response.Redirect("themelist.aspx?result=4");
            }
        }
    }
}