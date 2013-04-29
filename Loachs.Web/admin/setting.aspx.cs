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
using System.Drawing;
using System.Drawing.Text;

using Loachs.Common;
using Loachs.Entity;
using Loachs.Business;
namespace Loachs.Web
{
    public partial class admin_setting : AdminPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            SetPageTitle("网站设置");

            if (!IsPostBack)
            {
                BindSetting();
            }
            Page.MaintainScrollPositionOnPostBack = true;
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
                case 2:
                    ShowMessage("保存成功!");
                    break;

                default:
                    break;
            }
        }

        /// <summary>
        /// 绑定
        /// </summary>
        protected void BindSetting()
        {
            LoadDefaultData();

            SettingInfo s = SettingManager.GetSetting();
            if (s != null)
            {
                txtSiteName.Text = StringHelper.HtmlDecode(s.SiteName);
                txtSiteDescription.Text = StringHelper.HtmlDecode(s.SiteDescription);
                txtMetaKeywords.Text = StringHelper.HtmlDecode(s.MetaKeywords);
                txtMetaDescription.Text = StringHelper.HtmlDecode(s.MetaDescription);

                chkSiteStatus.Checked = s.SiteStatus == 1 ? true : false;


                chkEnableVerifyCode.Checked = s.EnableVerifyCode == 1 ? true : false;


                txtSidebarPostCount.Text = s.SidebarPostCount.ToString();
                txtSidebarCommentCount.Text = s.SidebarCommentCount.ToString();
                txtSidebarTagCount.Text = s.SidebarTagCount.ToString();

                txtPageSizeCommentCount.Text = s.PageSizeCommentCount.ToString();
                txtPageSizePostCount.Text = s.PageSizePostCount.ToString();
                //    txtPageSizeTagCount.Text = s.PageSizeTagCount.ToString();

                txtPostRelatedCount.Text = s.PostRelatedCount.ToString();

                txtFooterHtml.Text = s.FooterHtml;

                // ddlRewriteExtension.SelectedValue = s.RewriteExtension;

                //chkCommentApproved.Checked = s.CommentApproved == 1 ? true : false;

                //水印
                ddlWatermarkType.SelectedValue = s.WatermarkType.ToString();
                txtWatermarkText.Text = s.WatermarkText;
                ddlWatermarkFontName.SelectedValue = s.WatermarkFontName;
                ddlWatermarkFontSize.SelectedValue = s.WatermarkFontSize.ToString();
                txtWatermarkImage.Text = s.WatermarkImage;
                ddlWatermarkTransparency.SelectedValue = s.WatermarkTransparency.ToString();
                ddlWatermarkPosition.SelectedValue = s.WatermarkPosition.ToString();
                ddlWatermarkQuality.SelectedValue = s.WatermarkQuality.ToString();

                //评论
                chkCommentStatus.Checked = s.CommentStatus == 1 ? true : false;
                ddlCommentOrder.SelectedValue = s.CommentOrder.ToString();
                ddlCommentApproved.SelectedValue = s.CommentApproved.ToString();
                txtCommentSpamwords.Text = s.CommentSpamwords;

                //rss
                chkRssStatus.Checked = s.RssStatus == 1 ? true : false;
                txtRssRowCount.Text = s.RssRowCount.ToString();
                ddlRssShowType.SelectedValue = s.RssShowType.ToString();

                //rewrite
                //   chkRewriteStatus.Checked = s.RewriteStatus == 1 ? true : false;
                ddlRewriteExtension.SelectedValue = s.RewriteExtension;

                //total
                ddlTotalType.SelectedValue = s.SiteTotalType.ToString();

                ddlPostShowType.SelectedValue = s.PostShowType.ToString();

                //邮件
                txtSmtpEmail.Text = s.SmtpEmail;
                txtSmtpServer.Text = s.SmtpServer;
                txtSmtpServerPort.Text = s.SmtpServerPost.ToString();
                txtSmtpUserName.Text = s.SmtpUserName;
                txtSmtpPassword.Text = s.SmtpPassword;
                chkSmtpEnableSsl.Checked = s.SmtpEnableSsl == 1 ? true : false;

                //发送邮件设置
                chkSendMailAuthorByPost.Checked = s.SendMailAuthorByPost == 1 ? true : false;
                chkSendMailAuthorByComment.Checked = s.SendMailAuthorByComment == 1 ? true : false;
                chkSendMailNotifyByComment.Checked = s.SendMailNotifyByComment == 1 ? true : false;


            }

        }

        /// <summary>
        /// 加载默认数据
        /// </summary>
        private void LoadDefaultData()
        {

            ddlWatermarkFontName.Items.Clear();
            InstalledFontCollection fonts = new InstalledFontCollection();
            foreach (FontFamily family in fonts.Families)
            {
                ddlWatermarkFontName.Items.Add(new ListItem(family.Name, family.Name));
            }

            ddlWatermarkQuality.Items.Clear();
            for (int i = 100; i >= 0; i--)
            {
                string text = i.ToString();
                if (i == 100)
                {
                    text += "(最高)";
                }
                if (i == 80)
                {
                    text += "(推荐)";
                }
                ddlWatermarkQuality.Items.Add(new ListItem(text, i.ToString()));

            }

            ddlWatermarkTransparency.Items.Clear();
            for (int i = 10; i > 0; i--)
            {
                string text = i.ToString();
                if (i == 10)
                {
                    text += "(不透明)";
                }

                ddlWatermarkTransparency.Items.Add(new ListItem(text, i.ToString()));

            }
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnEdit_Click(object sender, EventArgs e)
        {
            SettingInfo s = SettingManager.GetSetting();
            if (s != null)
            {
                s.SiteName = StringHelper.HtmlEncode(txtSiteName.Text);
                s.SiteDescription = StringHelper.HtmlEncode(txtSiteDescription.Text);
                s.MetaKeywords = StringHelper.HtmlEncode(txtMetaKeywords.Text);
                s.MetaDescription = StringHelper.HtmlEncode(txtMetaDescription.Text);

                //   s.RewriteExtension = ddlRewriteExtension.SelectedValue;
                s.SiteStatus = chkSiteStatus.Checked ? 1 : 0;

                //  s.CommentApproved = chkCommentApproved.Checked ? 1 : 0;
                //    //c.GuestBookVerifyStatus = chkGuestBookVerifyStatus.Checked ? 1 : 0;
                s.EnableVerifyCode = chkEnableVerifyCode.Checked ? 1 : 0;



                //  s.PageSizeTagCount = StringHelper.StrToInt(txtPageSizeTagCount.Text, 10);
                s.PageSizePostCount = StringHelper.StrToInt(txtPageSizePostCount.Text, 10);
                s.PageSizeCommentCount = StringHelper.StrToInt(txtPageSizeCommentCount.Text, 50);

                s.SidebarPostCount = StringHelper.StrToInt(txtSidebarPostCount.Text, 10);
                s.SidebarTagCount = StringHelper.StrToInt(txtSidebarTagCount.Text, 10);
                s.SidebarCommentCount = StringHelper.StrToInt(txtSidebarCommentCount.Text, 10);

                //    //c.TagArticleNum = StringHelper.StrToInt(txtTagArticleNum.Text, 10);

                s.FooterHtml = txtFooterHtml.Text;

                //    //c.ArticleShowType = StringHelper.StrToInt(ddlArticleShowType.SelectedValue, 0);


                //水印

                s.WatermarkType = StringHelper.StrToInt(ddlWatermarkType.SelectedValue, 1);
                s.WatermarkText = txtWatermarkText.Text;
                s.WatermarkFontName = ddlWatermarkFontName.SelectedValue;
                s.WatermarkFontSize = StringHelper.StrToInt(ddlWatermarkFontSize.SelectedValue, 14);
                s.WatermarkImage = txtWatermarkImage.Text;
                s.WatermarkTransparency = StringHelper.StrToInt(ddlWatermarkTransparency.SelectedValue, 10);
                s.WatermarkPosition = StringHelper.StrToInt(ddlWatermarkPosition.SelectedValue, 1);
                s.WatermarkQuality = StringHelper.StrToInt(ddlWatermarkQuality.SelectedValue, 100);


                //评论
                s.CommentStatus = chkCommentStatus.Checked ? 1 : 0;
                s.CommentOrder = StringHelper.StrToInt(ddlCommentOrder.SelectedValue, 1);
                s.CommentApproved = StringHelper.StrToInt(ddlCommentApproved.SelectedValue, 1);
                s.CommentSpamwords = txtCommentSpamwords.Text;

                //rss
                s.RssStatus = chkRssStatus.Checked ? 1 : 0;
                s.RssRowCount = StringHelper.StrToInt(txtRssRowCount.Text, 20);
                s.RssShowType = StringHelper.StrToInt(ddlRssShowType.SelectedValue, 2);

                //rewrite
                //   s.RewriteStatus = chkRewriteStatus.Checked ? 1 : 0;
                s.RewriteExtension = ddlRewriteExtension.SelectedValue;

                //total
                s.SiteTotalType = StringHelper.StrToInt(ddlTotalType.SelectedValue, 1);

                s.PostShowType = StringHelper.StrToInt(ddlPostShowType.SelectedValue, 2);


                //邮件
                s.SmtpEmail = txtSmtpEmail.Text.Trim();
                s.SmtpServer = txtSmtpServer.Text.Trim();
                s.SmtpServerPost = StringHelper.StrToInt(txtSmtpServerPort.Text, 25);
                s.SmtpUserName = txtSmtpUserName.Text.Trim();
                s.SmtpPassword = txtSmtpPassword.Text.Trim();
                s.SmtpEnableSsl = chkSmtpEnableSsl.Checked == true ? 1 : 0;

                //发送邮件设置
                s.SendMailAuthorByPost = chkSendMailAuthorByPost.Checked == true ? 1 : 0;
                s.SendMailAuthorByComment = chkSendMailAuthorByComment.Checked == true ? 1 : 0;
                s.SendMailNotifyByComment = chkSendMailNotifyByComment.Checked == true ? 1 : 0;

                if (SettingManager.UpdateSetting())
                {


                    Response.Redirect("setting.aspx?result=2");
                }
            }
        }

        /// <summary>
        /// 测试邮箱设置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void btnTestSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (!StringHelper.IsEmail(txtTestEmail.Text))
                {
                    ltTestSendMessage.Text = "<span class=\"m_error\" >请输入正确的测试邮箱!</span>";
                    ShowError("请输入正确的测试邮箱!");
                    return;
                }
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress(txtSmtpEmail.Text, txtSiteName.Text);
                mail.To.Add(txtTestEmail.Text);
                mail.Subject = "这封邮件来自" + txtSiteName.Text;
                mail.Body = "测试邮件发送成功!";
                SmtpClient smtp = new SmtpClient(txtSmtpServer.Text);
                smtp.Credentials = new System.Net.NetworkCredential(txtSmtpUserName.Text, txtSmtpPassword.Text);
                smtp.EnableSsl = chkSmtpEnableSsl.Checked;
                smtp.Port = int.Parse(txtSmtpServerPort.Text, CultureInfo.InvariantCulture);
                smtp.Send(mail);
                ltTestSendMessage.Text = "<span class=\"m_pass\" >发送成功!</span>";
                ShowMessage("发送成功!");
            }
            catch (Exception ex)
            {
                ltTestSendMessage.Text = string.Format("<span class=\"m_error\" >发送失败!{0}</span>", ex.Message);
                ShowError("发送失败!");
            }
        }
    }
}