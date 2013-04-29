
using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mail;
using System.Globalization;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class admin_linklist : AdminPage
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        protected int linkId = RequestHelper.QueryInt("linkid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("链接管理");

            if (!IsPostBack)
            {
                BindLinkList();

                if (Operate == OperateType.Update)
                {
                    BindLink();
                    btnEdit.Text = "编辑";
                }
            }


            if (Operate == OperateType.Delete)
            {
                DeleteLink();
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
                case 1:
                    ShowMessage("添加成功!");
                    break;
                case 2:
                    ShowMessage("修改成功!");
                    break;
                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 4:
                    ShowError("系统链接不能删除!");
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// 删除
        /// </summary>
        protected void DeleteLink()
        {
            //  LinkInfo link = LinkManager.GetLink(linkId);
            //if (link != null && link.Type == (int)LinkType.System)
            //{
            //    Response.Redirect("linklist.aspx?result=4");
            //}
            LinkManager.DeleteLink(linkId);
            Response.Redirect("linklist.aspx?result=3");
        }

        /// <summary>
        /// 绑定链接
        /// </summary>
        protected void BindLink()
        {
            LinkInfo link = LinkManager.GetLink(linkId);
            if (link != null)
            {
                txtName.Text = StringHelper.HtmlDecode(link.Name);
                txtHref.Text = StringHelper.HtmlDecode(link.Href);
                txtDescription.Text = StringHelper.HtmlDecode(link.Description);
                txtDisplayOrder.Text = link.Displayorder.ToString();
                chkStatus.Checked = link.Status == 1 ? true : false;
                chkPosition.Checked = link.Position == (int)LinkPosition.Navigation ? true : false;
                chkTarget.Checked = link.Target == "_blank" ? true : false;

            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindLinkList()
        {
            List<LinkInfo> list = LinkManager.GetLinkList();
            rptLink.DataSource = list;
            rptLink.DataBind();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            LinkInfo link = new LinkInfo();
            if (Operate == OperateType.Update)
            {
                link = LinkManager.GetLink(linkId);
            }
            else
            {
                link.CreateDate = DateTime.Now;
                link.Type = 0;// (int)LinkType.Custom;
            }
            link.Name = StringHelper.HtmlEncode(txtName.Text.Trim());
            link.Href = StringHelper.HtmlEncode(txtHref.Text.Trim());
            link.Description = StringHelper.HtmlEncode(txtDescription.Text);
            link.Displayorder = StringHelper.StrToInt(txtDisplayOrder.Text, 1000);
            link.Status = chkStatus.Checked ? 1 : 0;
            link.Position = chkPosition.Checked ? (int)LinkPosition.Navigation : (int)LinkPosition.General;
            link.Target = chkTarget.Checked ? "_blank" : "_self";

            if (link.Name == "")
            {
                ShowError("请输入名称!");
                return;
            }

            if (Operate == OperateType.Update)
            {
                LinkManager.UpdateLink(link);

                Response.Redirect("linklist.aspx?result=2");
            }
            else
            {
                LinkManager.InsertLink(link);
                Response.Redirect("linklist.aspx?result=1");
            }
        }
    }
}