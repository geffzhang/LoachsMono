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
    public partial class admin_taglist : AdminPage
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        protected int tagId = RequestHelper.QueryInt("tagid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("标签管理");
            if (!IsPostBack)
            {
                BindCategoryList();


                if (Operate == OperateType.Update)
                {
                    BindCategory();
                    btnEdit.Text = "编辑";
                }
                if (Operate == OperateType.Delete)
                {
                    Delete();
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
                case 1:
                    ShowMessage("添加成功!");
                    break;
                case 2:
                    ShowMessage("修改成功!");
                    break;

                case 3:
                    ShowMessage("删除成功!");
                    break;

                default:
                    break;
            }
        }

        protected void Delete()
        {
            TagManager.DeleteTag(tagId);
            Response.Redirect("taglist.aspx?result=3&page=" + Pager1.PageIndex);
        }
        /// <summary>
        /// 绑定分类
        /// </summary>
        protected void BindCategory()
        {
            TagInfo tag = TagManager.GetTag(tagId);
            if (tag != null)
            {
                txtName.Text = StringHelper.HtmlDecode(tag.Name);
                //    txtSlug.Text = StringHelper.HtmlDecode(tag.Slug);
                txtDescription.Text = StringHelper.HtmlDecode(tag.Description);
                txtDisplayOrder.Text = tag.Displayorder.ToString();
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindCategoryList()
        {
            int recordCount = 0;

            List<TagInfo> list = TagManager.GetTagList(Pager1.PageSize, Pager1.PageIndex, out recordCount);
            rptCategory.DataSource = list;
            rptCategory.DataBind();

            Pager1.RecordCount = recordCount;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            TagInfo tag = new TagInfo();
            if (Operate == OperateType.Update)
            {
                tag = TagManager.GetTag(tagId);
            }
            else
            {
                tag.CreateDate = DateTime.Now;
                tag.Count = 0;
            }
            tag.Name = StringHelper.HtmlEncode(txtName.Text.Trim());

            //tag.Slug = txtSlug.Text.Trim();
            //if (string.IsNullOrEmpty(tag.Slug))
            //{
            //    tag.Slug = tag.Name;
            //}

            //tag.Slug = StringHelper.HtmlEncode(PageUtils.FilterSlug(tag.Slug, "tag"));


            //tag.Slug = StringHelper.HtmlEncode(PageUtils.FilterSlug(txtSlug.Text,"tag"));

            tag.Slug = PageUtils.FilterSlug(tag.Name, "tag");
            tag.Description = StringHelper.HtmlEncode(txtDescription.Text);
            tag.Displayorder = StringHelper.StrToInt(txtDisplayOrder.Text, 1000);

            if (tag.Name == "")
            {
                ShowError("请输入名称!");
                return;
            }

            if (Operate == OperateType.Update)
            {
                TagManager.UpdateTag(tag);
                Response.Redirect("taglist.aspx?result=2&page=" + Pager1.PageIndex);
            }
            else
            {
                TagManager.InsertTag(tag);
                Response.Redirect("taglist.aspx?result=1&page=" + Pager1.PageIndex);
            }
        }
    }
}