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
    public partial class admin_categorylist : Loachs.Web.AdminPage
    {
        /// <summary>
        /// 分类ID
        /// </summary>
        protected int categoryId = RequestHelper.QueryInt("categoryid");

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("分类管理");

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
            CategoryManager.DeleteCategory(categoryId);
            Response.Redirect("categorylist.aspx?result=3");
        }
        /// <summary>
        /// 绑定分类
        /// </summary>
        protected void BindCategory()
        {
            CategoryInfo term = CategoryManager.GetCategory(categoryId);
            if (term != null)
            {
                txtName.Text = StringHelper.HtmlDecode(term.Name);
                txtSlug.Text = StringHelper.HtmlDecode(term.Slug);
                txtDescription.Text = StringHelper.HtmlDecode(term.Description);
                txtDisplayOrder.Text = term.Displayorder.ToString();
            }
        }

        /// <summary>
        /// 绑定列表
        /// </summary>
        protected void BindCategoryList()
        {
            List<CategoryInfo> list = CategoryManager.GetCategoryList();
            rptCategory.DataSource = list;
            rptCategory.DataBind();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            CategoryInfo term = new CategoryInfo();
            if (Operate == OperateType.Update)
            {
                term = CategoryManager.GetCategory(categoryId);
            }
            else
            {
                term.CreateDate = DateTime.Now;
                term.Count = 0;
            }
            term.Name = StringHelper.HtmlEncode(txtName.Text);
            term.Slug = txtSlug.Text.Trim();
            if (string.IsNullOrEmpty(term.Slug))
            {
                term.Slug = term.Name;
            }

            term.Slug = StringHelper.HtmlEncode(PageUtils.FilterSlug(term.Slug, "cate"));

            term.Description = StringHelper.HtmlEncode(txtDescription.Text);
            term.Displayorder = StringHelper.StrToInt(txtDisplayOrder.Text, 1000);

            if (term.Name == "")
            {
                ShowError("请输入名称!");
                return;
            }

            if (Operate == OperateType.Update)
            {
                CategoryManager.UpdateCategory(term);
                Response.Redirect("categorylist.aspx?result=2");
            }
            else
            {
                CategoryManager.InsertCategory(term);
                Response.Redirect("categorylist.aspx?result=1");
            }
        }
    }
}