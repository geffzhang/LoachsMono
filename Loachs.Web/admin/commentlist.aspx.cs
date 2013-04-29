
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

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class admin_commentlist : AdminPage
    {


        /// <summary>
        /// 审核
        /// </summary>
        int approved = RequestHelper.QueryInt("approved", -1);

        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("评论管理");

            OperateComment();

            ShowResult();

            if (!IsPostBack)
            {
                BindCommentList();
            }

        }

        /// <summary>
        /// 审核,删除单条记录
        /// </summary>
        private void OperateComment()
        {
            int commentId = RequestHelper.QueryInt("commentid", 0);
            if (Operate == OperateType.Delete)
            {
                CommentManager.DeleteComment(commentId);

                Response.Redirect("commentlist.aspx?result=3&page=" + Pager1.PageIndex);
            }
            else if (Operate == OperateType.Update)
            {
                CommentInfo comment = CommentManager.GetComment(commentId);
                if (comment != null)
                {
                    comment.Approved = (int)ApprovedStatus.Success;
                    CommentManager.UpdateComment(comment);

                    Response.Redirect("commentlist.aspx?result=4&page=" + Pager1.PageIndex);
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

                case 3:
                    ShowMessage("删除成功!");
                    break;
                case 4:
                    ShowMessage("审核成功!");
                    break;
                default:
                    break;
            }
        }


        /// <summary>
        /// 绑定
        /// </summary>
        protected void BindCommentList()
        {
            int totalRecord = 0;

            List<CommentInfo> list = CommentManager.GetCommentList(Pager1.PageSize, Pager1.PageIndex, out totalRecord, 1, -1, -1, -1, approved, -1, string.Empty);
            rptComment.DataSource = list;
            rptComment.DataBind();
            Pager1.RecordCount = totalRecord;
        }

        /// <summary>
        /// 文章连接
        /// </summary>
        /// <param name="postId"></param>
        /// <returns></returns>
        protected string GetPostLink(int postId)
        {

            PostInfo post = PostManager.GetPost(postId);
            if (post != null)
            {
                return string.Format(" 评: {0}", StringHelper.CutString(post.Title, 20, "..."));
            }
            return string.Empty;
        }



        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnDelete_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (RepeaterItem item in rptComment.Items)
            {
                HtmlInputCheckBox box = ((HtmlInputCheckBox)item.FindControl("chkRow"));
                if (box.Checked)
                {
                    int commentId = Convert.ToInt32(box.Value);
                    CommentManager.DeleteComment(commentId);
                    i++;

                }
            }


            Response.Redirect("commentlist.aspx?result=3&page=" + Pager1.PageIndex + "&approved=" + approved);

        }

        /// <summary>
        /// 审核
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnApproved_Click(object sender, EventArgs e)
        {
            int i = 0;
            foreach (RepeaterItem item in rptComment.Items)
            {
                HtmlInputCheckBox box = ((HtmlInputCheckBox)item.FindControl("chkRow"));
                if (box.Checked)
                {
                    int commentID = Convert.ToInt32(box.Value);
                    CommentInfo c = CommentManager.GetComment(commentID);
                    if (c != null)
                    {
                        c.Approved = (int)ApprovedStatus.Success;
                        if (CommentManager.UpdateComment(c) > 0)
                        {
                            i++;
                        }
                    }
                }
            }
            Response.Redirect("commentlist.aspx?result=4&page=" + Pager1.PageIndex + "&approved=" + approved);
        }
    }
}