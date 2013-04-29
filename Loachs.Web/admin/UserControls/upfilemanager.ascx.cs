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
    public partial class admin_usercontrols_upfilemanager : System.Web.UI.UserControl
    {

        /// <summary>
        /// 当前路径
        /// </summary>
        protected string path = RequestHelper.QueryString("path");

        /// <summary>
        /// 上传结果
        /// </summary>
        protected int result = RequestHelper.QueryInt("result", 0);

        /// <summary>
        /// 当前文件夹
        /// </summary>
        protected DirectoryInfo currentDirectory;

        /// <summary>
        /// 允许上传的文件格式
        /// </summary>
        public string AllowFileExtension = "jpg,jpeg,gif,png,bmp,ico,rar,zip,7z,txt,html,js,css,chm,doc,docx,xls,xlsx,csv,ppt,pptx,psd,pdf,swf,mp3,wma";


        protected string OperateString = RequestHelper.QueryString("operate", true);

        /// <summary>
        /// 输出信息
        /// </summary>
        public string ResponseMessage = string.Empty;

        /// <summary>
        /// 错误提示
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public string ShowError(string error)
        {
            ResponseMessage = "<div class=\"p_error\">";
            ResponseMessage += error;
            ResponseMessage += "</div>";
            return ResponseMessage;
        }

        /// <summary>
        /// 正确提示
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public string ShowMessage(string message)
        {

            ResponseMessage = "<div class=\"p_message\">";
            ResponseMessage += message;
            ResponseMessage += "</div>";
            return ResponseMessage;
        }


        /// <summary>
        /// 页面文件名称
        /// </summary>
        protected string FileName;

        ///// <summary>
        ///// 目的是获取附件
        ///// </summary>
        //protected  int GetFile = RequestHelper.QueryInt("getfile",0);


        protected void Page_Load(object sender, EventArgs e)
        {

            FileName = System.IO.Path.GetFileName(Request.Url.AbsolutePath);
            //  Response.Write(FileName);




            CheckPath();

            // if (Operate == OperateType.Delete)
            if (OperateString == "delete")
            {
                Delete();
            }

            if (result == 1)
            {
                ShowError("同名文件已存在,上传取消!");
            }
            else if (result == 2)
            {
                ShowMessage("上传成功!");
            }
            else if (result == 3)
            {
                ShowMessage("删除成功!");
            }
            else if (result == 444)
            {
                ShowMessage("权限不够!");
            }


        }

        /// <summary>
        /// 检查路径
        /// </summary>
        protected void CheckPath()
        {
            if (string.IsNullOrEmpty(path) || path.IndexOf("upfiles") == -1)
            {
                path = ConfigHelper.SitePath + "upfiles/";
            }

            if (!Directory.Exists(Server.MapPath(path)))
            {
                Response.Redirect(FileName);

            }

            currentDirectory = new DirectoryInfo(Server.MapPath(path));
        }

        /// <summary>
        /// 删除文件夹或文件
        /// </summary>
        protected void Delete()
        {
            string res = "";
            string deletepath = RequestHelper.QueryString("deletepath");
            string category = RequestHelper.QueryString("category", true);

            string return_deletepath = deletepath.Substring(0, deletepath.LastIndexOf('/') + 1);

            if (PageUtils.CurrentUser.Type != (int)UserType.Administrator)
            {
                Response.Redirect(FileName + "?path=" + return_deletepath + "&result=444");
            }

            if (deletepath.IndexOf("upfiles") != -1)
            {
                if (category.IndexOf("directory") != -1)
                {
                    Directory.Delete(Server.MapPath(deletepath), true);
                }
                else if (category.IndexOf("archive") != -1)
                {
                    File.Delete(Server.MapPath(deletepath));
                }
                else
                {
                    if (File.Exists(Server.MapPath(deletepath)))
                    {
                        File.Delete(Server.MapPath(deletepath));
                    }
                }
            }

            Response.Redirect(FileName + "?path=" + return_deletepath + "&result=3");
        }

        /// <summary>
        /// 获取当前路径所有连接
        /// </summary>
        protected string GetPathUrl()
        {
            string pathLink = string.Empty;

            string path2 = path.Substring(1, path.Length - 2);

            string[] tempPath = path2.Split('/');

            string temp = "/";

            pathLink = ConfigHelper.SitePath.TrimEnd('/');

            for (int i = 0; i < tempPath.Length; i++)
            {

                temp += tempPath[i] + "/";
                if (i == 0 && ConfigHelper.SitePath.Length > 1) //有虚拟目录
                {
                    continue;
                }
                pathLink += string.Format("/<a href='{2}?path={0}'>{1}</a>", temp, tempPath[i], FileName);
            }
            return pathLink;
        }

        /// <summary>
        /// 获取文件对应的图标
        /// </summary>
        /// <param name="fileExtension"></param>
        /// <returns></returns>
        protected string GetFileImage(string fileExtension)
        {
            switch (fileExtension.ToLower())
            {
                case ".gif":
                case ".jpg":
                case ".png":
                case ".bmp":
                case ".tif":
                    return "jpg.gif";

                case ".doc":
                case ".docx":
                case ".rtf":
                    return "doc.gif";
                case ".ppt":
                case ".pptx":
                    return "ppt.gif";
                case ".xls":
                case ".xlsx":
                case ".csv":
                    return "xls.gif";
                case ".pdf":
                    return "pdf.gif";

                case ".rar":
                case ".zip":
                case ".cab":
                case ".7z":
                    return "rar.gif";

                case ".wav":
                case ".wmv":
                case ".wma":
                case ".mpeg":
                case ".avi":
                case ".mp3":
                    return "wma.gif";

                case ".ini":
                case ".txt":
                case ".css":
                case ".js":
                case ".htm":
                case ".html":
                case ".xml":
                case ".h":
                case ".c":
                case ".php":
                case ".vb":
                case ".cpp":
                case ".cs":
                case ".aspx":
                case ".asm":
                case ".sln":
                case ".vs":
                    return "txt.gif";

                case ".fla":
                case ".flv":
                case ".swf":
                    return "swf.gif";

                case ".psd":
                    return "psd.gif";

                case ".chm":
                    return "chm.gif";

                case ".dll":
                case ".exe":
                case ".msi":
                case ".db":
                    return "exe.gif";

                default: return "default.gif";
            }
        }

        /// <summary>
        /// 是否为图片
        /// </summary>
        /// <param name="ext"></param>
        /// <returns></returns>
        protected bool IsImage(string ext)
        {
            if (!string.IsNullOrEmpty(ext))
            {
                ext = ext.ToLower();
            }
            if (ext == ".jpg" || ext == ".jpeg" || ext == ".gif" || ext == ".bmp" || ext == ".png")
            {
                return true;
            }
            return false;
        }



        /// <summary>
        /// 上传附件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnUpload_Click(object sender, EventArgs e)
        {
            HttpPostedFile postedFile = FileUpload1.PostedFile;

            string uploadPath = ConfigHelper.SitePath + "upfiles/" + DateTime.Now.ToString("yyyyMM") + "/";      //文件保存相对路径
            string saveDirectory = Server.MapPath(uploadPath);                                                  //文件保存绝对文件夹
            //  string waterPath = Server.MapPath("../common/images/watermark.gif");//待改

            string fileName = Path.GetFileName(postedFile.FileName);                                            //文件名
            fileName = fileName.Replace(" ", "");
            fileName = fileName.Replace("%", "");
            fileName = fileName.Replace("&", "");
            fileName = fileName.Replace("#", "");
            fileName = fileName.Replace("'", "");
            fileName = fileName.Replace("+", "");


            string fileExtension = Path.GetExtension(postedFile.FileName);                                      //文件后缀

            string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(fileName);                       //没有后缀的文件名



            int type = StringHelper.StrToInt(rblistType.SelectedValue, 1);

            if (postedFile.ContentLength == 0)
            {
                ShowError("请选择要上传的文件!");

                return;
            }

            string[] fileExts = AllowFileExtension.Split(',');
            bool allow = false;
            foreach (string str in fileExts)
            {
                if (("." + str) == fileExtension)
                {
                    allow = true;
                    break;
                }
            }
            if (allow == false)
            {
                ShowError("您上传的文件格式不被允许!");
                return;
            }

            if (!Directory.Exists(saveDirectory))
            {
                Directory.CreateDirectory(saveDirectory);
            }

            int iCounter = 0;

            int result = 1;
            while (true)
            {
                string fileSavePath = saveDirectory + fileName;
                string fileSavePath2 = saveDirectory + "da563457-1c3c-4b28-bf73-92f87f930896" + fileName;
                if (File.Exists(fileSavePath))
                {
                    if (type == 1)//跳过
                    {
                        result = 1;
                        break;
                    }
                    else if (type == 2)//重命名
                    {
                        iCounter++;
                        fileName = fileNameWithoutExtension + "(" + iCounter + ")" + fileExtension;

                    }
                    else if (type == 3)//覆盖 
                    {
                        File.Delete(fileSavePath);
                    }
                }
                else
                {
                    if ((fileExtension == ".gif" || fileExtension == ".jpg" || fileExtension == ".jpeg" || fileExtension == ".bmp" || fileExtension == ".png") && chkWatermark.Checked)
                    {
                        postedFile.SaveAs(fileSavePath2);

                        string newFileName = Path.GetFileNameWithoutExtension(postedFile.FileName) + "w(" + iCounter + ")" + Path.GetExtension(postedFile.FileName);
                        string newImagePath = Server.MapPath(uploadPath + newFileName);
                        string waterImagePath = Server.MapPath(ConfigHelper.SitePath + "common/images/watermark/" + SettingManager.GetSetting().WatermarkImage);

                        if (SettingManager.GetSetting().WatermarkType == 2 && File.Exists(waterImagePath))
                        {
                            Watermark.CreateWaterImage(fileSavePath2, fileSavePath, SettingManager.GetSetting().WatermarkPosition, waterImagePath, SettingManager.GetSetting().WatermarkTransparency, SettingManager.GetSetting().WatermarkQuality);
                        }
                        else
                        {
                            Watermark.CreateWaterText(fileSavePath2, fileSavePath, SettingManager.GetSetting().WatermarkPosition, SettingManager.GetSetting().WatermarkText, SettingManager.GetSetting().WatermarkQuality, SettingManager.GetSetting().WatermarkFontName, SettingManager.GetSetting().WatermarkFontSize);

                        }
                        File.Delete(fileSavePath2);
                    }
                    else
                    {
                        postedFile.SaveAs(fileSavePath);
                    }

                    result = 2;
                    break;
                }
            }

            Response.Redirect(FileName + "?path=" + uploadPath + "&result=" + result);
        }
    }
}
