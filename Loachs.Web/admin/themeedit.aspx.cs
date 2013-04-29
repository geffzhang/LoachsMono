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
    public partial class admin_themeedit : AdminPage
    {
        /// <summary>
        /// 主题名
        /// </summary>
        protected string themeName = RequestHelper.QueryString("themename");

        /// <summary>
        /// 文件路径
        /// </summary>
        protected string filePath = RequestHelper.QueryString("filepath");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("主题编辑");

            if (!IsPostBack)
            {
                BindFile();
            }
        }

        /// <summary>
        /// 绑定文件
        /// </summary>
        protected void BindFile()
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                using (StreamReader objReader = new StreamReader(Server.MapPath(ConfigHelper.SitePath + "themes/" + themeName + "/" + filePath), Encoding.UTF8))
                {
                    txtContent.Text = objReader.ReadToEnd();
                    objReader.Close();
                }
            }
            else
            {
                Response.Redirect("themeedit.aspx?themename=" + themeName + "&filepath=template/default.html");
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string filepath = Server.MapPath(ConfigHelper.SitePath + "themes/" + themeName + "/" + filePath);
            using (FileStream fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite, FileShare.ReadWrite))
            {
                Byte[] info = Encoding.UTF8.GetBytes(txtContent.Text);
                fs.Write(info, 0, info.Length);
                fs.Close();
            }
            ShowMessage("保存成功!");
        }
    }
}