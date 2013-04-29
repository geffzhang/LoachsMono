<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_setting" Title="无标题页" Codebehind="setting.aspx.cs" %>
<%@ Import Namespace="Loachs.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<style type="text/css">
.navs{line-height:22px; border:0px solid #ccc;color:#666; padding:3px  ;}
.navs a { padding-right:8px; font-size:14px;}
</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>网站设置</h2>
<%=ResponseMessage %>
<div class="navs" ><a href="#bases">基本设置</a> <a href="#rsss">Rss设置</a><a href="#comments">评论设置</a><a href="#displays">显示设置</a><a href="#emails">邮箱设置</a>
<a href="#emailsends">邮件发送设置</a><a href="#watermarks">水印设置</a><a href="#footers">页脚设置</a>
</div>
<h4 id="bases">基本设置</h4>
<table width="100%">
    <tr class="row">
        <td  style="width:20%;"><label  for="<%=txtSiteName.ClientID %>">网站标题:</label></td>
        <td><asp:TextBox ID="txtSiteName"   Width="70%"  runat="server" CssClass="text"></asp:TextBox></td>
    </tr>
    <tr class="row">
        <td><label  for="<%=txtSiteDescription.ClientID %>">网站描述:</label></td>
        <td><asp:TextBox ID="txtSiteDescription" Width="70%"   runat="server" CssClass="text"></asp:TextBox></td>
    </tr>
    <tr class="row">
        <td ><label  for="<%=txtMetaKeywords.ClientID %>">Meta Keywords:</label></td>
        <td><asp:TextBox ID="txtMetaKeywords"   Width="70%"  runat="server" CssClass="text"></asp:TextBox> <span class="m_desc" >首页关键字,用逗号","隔开</span></td>
    </tr>
    <tr class="row">
        <td><label  for="<%=txtMetaDescription.ClientID %>">Meta Description:</label></td>
        <td><asp:TextBox ID="txtMetaDescription" Width="70%"   runat="server" CssClass="text"></asp:TextBox> <span class="m_desc" >首页描述</span></td>
    </tr>
    <tr class="row">
        <td><label  for="<%=ddlTotalType.ClientID %>">统计类型:</label></td>
        <td>
            <asp:DropDownList ID="ddlTotalType" runat="server" Width="160"  >
                <asp:ListItem Value="1" Text="刷新一次增加一次"></asp:ListItem>
                <asp:ListItem Value="2" Text="一天只增加一次"></asp:ListItem>
            </asp:DropDownList>
            <span class="m_desc" >统计包括文章浏览数,网站浏览数</span>
        </td>
    </tr>
    <tr class="row">
        <td ><label  for="<%=ddlRewriteExtension.ClientID %>">页面扩展名:</label></td>
        <td>
             <asp:DropDownList ID="ddlRewriteExtension" runat="server" Width="160">
                <asp:ListItem Value=".aspx" Text=".aspx"></asp:ListItem>
                <asp:ListItem Value=".html" Text=".html"></asp:ListItem>
                <asp:ListItem Value="" Text="无扩展名(文件夹形式)"></asp:ListItem>
            </asp:DropDownList>
            <span class="m_desc" >非.aspx后缀需要修改IIS站点映射设置</span>
        </td>
    </tr>
    <tr class="rowend">
        <td></td>
        <td>
            <asp:CheckBox ID="chkSiteStatus" runat="server" Text="启用网站" Checked="true" />          
            <asp:CheckBox ID="chkEnableVerifyCode" runat="server" Checked="true" Text="启用验证码(评论)" />
        </td>
    </tr>
    
</table>
<h4 id="rsss">Rss设置</h4>
<table width="100%">
    <tr class="row">
        <td  style="width:20%;"><label  for="<%=chkRssStatus.ClientID %>">状态:</label></td>
        <td><asp:CheckBox ID="chkRssStatus" runat="server" Text="启用" Checked="true" /></td>
    </tr>
    <tr class="row">
        <td ><label  for="<%=txtRssRowCount.ClientID %>">输出条数:</label></td>
        <td>
                <asp:TextBox ID="txtRssRowCount"    runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >指输出最新文章的数量,默认为20</span>
        </td>
    </tr>
    <tr class="rowend">
        <td><label  for="<%=ddlRssShowType.ClientID %>">输出方式:</label></td>
        <td>
             <asp:DropDownList ID="ddlRssShowType" runat="server" Width="160"  >
                <asp:ListItem Value="1" Text="仅标题"></asp:ListItem>
                <asp:ListItem Value="2" Text="摘要"></asp:ListItem>
                <asp:ListItem Value="3" Text="正文前200字"></asp:ListItem>
                <asp:ListItem Value="4" Text="正文"></asp:ListItem>
            </asp:DropDownList>
            <span class="m_desc" >指正文显示的内容,默认为摘要</span>
        </td>
    </tr>
     
</table>
<h4 id="comments">评论设置</h4>
<table width="100%">
    <tr class="row">
        <td  style="width:20%;"><label  for="<%=chkCommentStatus.ClientID %>">状态:</label></td>
        <td><asp:CheckBox ID="chkCommentStatus" runat="server" Text="启用" Checked="true" />  <span class="m_desc" >这是总开头,如果不启用,所以文章均不能评论</span></td>
    </tr>
    <tr class="row">
        <td ><label  for="<%=ddlCommentOrder.ClientID %>">排序模式:</label></td>
        <td>
                <asp:DropDownList ID="ddlCommentOrder" runat="server" Width="160"  >
                <asp:ListItem Value="0" Text="顺序(默认)"></asp:ListItem>
                <asp:ListItem Value="1" Text="倒序(后发表的在前面)"></asp:ListItem>
                
            </asp:DropDownList>
             
        </td>
    </tr>
    <tr class="row">
        <td ><label  for="<%=ddlCommentApproved.ClientID %>">审核类型:</label></td>
        <td>
                <asp:DropDownList ID="ddlCommentApproved" runat="server" Width="160"  >
                <asp:ListItem Value="1" Text="无需审核"></asp:ListItem>
                <asp:ListItem Value="2" Text="自动审核(配合垃圾词语)"></asp:ListItem>
                <asp:ListItem Value="3" Text="人工审核"></asp:ListItem>
            </asp:DropDownList>
           
        </td>
    </tr>
    <tr class="rowend">
        <td><label  for="<%=txtCommentSpamwords.ClientID %>">垃圾词语:</label></td>
        <td>
             <asp:TextBox ID="txtCommentSpamwords" runat="server" TextMode="MultiLine" Width="80%" Height="100"></asp:TextBox>
             <div class="m_desc" >多个词语以","隔开, 当审核类型为"自动审核"时,评论包括这些关键字,则该评论转为待审核,否则审核通过</div>
        </td>
    </tr>
     
</table>
<h4 id="displays">显示设置</h4>
<table width="100%">
    <tr class="row">
        <td  style="width:20%;"><label  for="<%=ddlPostShowType.ClientID %>">文章列表显示:</label></td>
        <td  >
            <asp:DropDownList ID="ddlPostShowType" runat="server" Width="160"  >
                <asp:ListItem Value="1" Text="仅标题"></asp:ListItem>
                <asp:ListItem Value="2" Text="摘要"></asp:ListItem>
                <asp:ListItem Value="3" Text="正文前200字"></asp:ListItem>
                <asp:ListItem Value="4" Text="正文"></asp:ListItem>
            </asp:DropDownList>
        </td>
    </tr>
    <tr class="row">
        <td  ><label  for="<%=txtPageSizePostCount.ClientID %>">每页文章数:</label></td>
        <td>
            
                <asp:TextBox ID="txtPageSizePostCount"    runat="server" CssClass="text"></asp:TextBox>
           
            <span class="m_desc" >指文章列表每页条数</span>
        </td>
    </tr>
    <tr class="row">
        <td><label  for="<%=txtPageSizeCommentCount.ClientID %>">每页评论数:</label></td>
        <td>
           
                <asp:TextBox ID="txtPageSizeCommentCount" runat="server"  CssClass="text"></asp:TextBox>
           
            <span class="m_desc" >指正文下方评论每页条数</span>
        </td>
    </tr>

    <%--<tr class="row">
        <td><label  for="<%=txtPageSizeTagCount.ClientID %>">每页标签数:</label></td>
        <td>
            <asp:TextBox ID="txtPageSizeTagCount" runat="server" ></asp:TextBox>
            <span class="m_desc" >指标签页每页条数</span>
        </td>
    </tr>--%>
 
    <tr class="row">
        <td><label  for="<%=txtPostRelatedCount.ClientID %>">相关文章数:</label></td>
        <td>
            <asp:TextBox ID="txtPostRelatedCount"    runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >指正文下方相关文章条数</span>
        </td>
    </tr>
    <tr class="row">
        <td><label  for="<%=txtSidebarPostCount.ClientID %>">侧栏文章数:</label></td>
        <td>
            
                <asp:TextBox ID="txtSidebarPostCount"    runat="server" CssClass="text"></asp:TextBox>
          
            <span class="m_desc" >指侧栏显示的文章条数</span>
        </td>
    </tr>
    
    <tr class="row">
        <td><label  for="<%=txtSidebarCommentCount.ClientID %>">侧栏评论数:</label></td>
        <td>
          
                <asp:TextBox ID="txtSidebarCommentCount"    runat="server" CssClass="text"></asp:TextBox>
             
            <span class="m_desc" >指侧栏显示的评论条数</span>
        </td>
    </tr>
    <tr class="rowend">
        <td><label  for="<%=txtSidebarTagCount.ClientID %>">侧栏标签数:</label></td>
        <td>
            
                <asp:TextBox ID="txtSidebarTagCount"   runat="server" CssClass="text"></asp:TextBox>
           
            <span class="m_desc" >指侧栏显示的标签个数</span>
        </td>
    </tr>
    
</table>
<h4 id="emails">邮箱设置<span class="small gray normal">(推荐使用Gmail)</span></h4>
<table width="100%">
    
    <tr class="row">
        <td style="width:20%;"><label  for="<%=txtSmtpServer.ClientID %>">Smtp服务器:</label></td>
        <td>
            <asp:TextBox ID="txtSmtpServer"  runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >如:smtp.gmail.com</span>
        </td>
    </tr>
    <tr class="row">
        <td><label  for="<%=txtSmtpServerPort.ClientID %>">Smtp服务器端口:</label></td>
        <td>
            <asp:TextBox ID="txtSmtpServerPort"   runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >默认为25, Gmail可尝试使用587</span>
        </td>
    </tr>
    <tr class="row">
        <td ><label  for="<%=txtSmtpEmail.ClientID %>">邮箱:</label></td>
        <td>
            <asp:TextBox ID="txtSmtpEmail"   runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >如:yourname@gmail.com</span>
        </td>
    </tr>

    <tr class="row">
        <td><label  for="<%=txtSmtpUserName.ClientID %>">用户名:</label></td>
        <td>
            <asp:TextBox ID="txtSmtpUserName"  runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >如:yourname</span>
        </td>
    </tr>
    <tr class="row">
        <td><label  for="<%=txtSmtpPassword.ClientID %>">密码:</label></td>
        <td>
            <asp:TextBox ID="txtSmtpPassword" runat="server" CssClass="text"></asp:TextBox>
            <span class="m_desc" >您的邮箱密码</span>
        </td>
    </tr>
    
    <tr class="row">
        <td><label  for="<%=chkSmtpEnableSsl.ClientID %>">安全连接Ssl:</label></td>
        <td><asp:CheckBox ID="chkSmtpEnableSsl" runat="server" Text="启用"   /> <span class="m_desc" >默认不启用, Gmail需要启用</span></td>
    </tr>
    
    <tr class="rowend">
        <td>&nbsp;</td>
        <td>
           <label class="" for="<%=txtTestEmail.ClientID %>">测试邮箱:</label> <asp:TextBox ID="txtTestEmail" runat="server" CssClass="text"></asp:TextBox>
           <asp:Button ID="btnTestSend" CssClass=" " runat="server"   Text="发送测试邮件"  OnClick="btnTestSend_Click" TabIndex="999" UseSubmitBehavior="false"  />
           <asp:Literal ID="ltTestSendMessage" runat="server" ></asp:Literal>
        </td>
    </tr>
</table>
<h4 id="emailsends">邮件发送设置<span class="small gray normal">(完成邮箱设置后方生效)</span></h4>
<table width="100%">
    <tr class="row">
        <td style="width:20%;"><label class="" for="<%=chkSendMailAuthorByPost.ClientID %>">发表新文章时:</label></td>
        <td><asp:CheckBox ID="chkSendMailAuthorByPost" runat="server" Text="给所有作者发一封邮件" Checked="false" /></td>
    </tr>
    <tr class="rowend">
        <td><label class="" for="<%=chkSendMailAuthorByComment.ClientID %>">收到新评论时:</label></td>
        <td>
            <asp:CheckBox ID="chkSendMailAuthorByComment" runat="server" Text="给文章作者发一封邮件"  Checked="false"/>
            <asp:CheckBox ID="chkSendMailNotifyByComment" runat="server" Text="给该文章评论订阅者发一封邮件"  Checked="true" />
        </td>
    </tr>
   
</table>
<h4 id="watermarks">水印设置<span class="small gray normal">(上传附件为图片时使用)</span></h4>
<table width="100%">
        <tr class="row">
            <td style="width:20%;"><label class="" for="<%=ddlWatermarkType.ClientID %>">水印类型:</label></td>
            <td>
                 <asp:DropDownList ID="ddlWatermarkType" runat="server" Width="160"  >
                    <asp:ListItem Value="1" Text="文字"></asp:ListItem>
                    <asp:ListItem Value="2" Text="图片"></asp:ListItem>
                </asp:DropDownList>
            </td>
        </tr>
        <tr class="row">
            <td><label class="" for="<%=txtWatermarkText.ClientID %>">文字型水印内容:</label></td>
            <td>
                <asp:TextBox ID="txtWatermarkText"    runat="server" CssClass="text"></asp:TextBox>
                <span class="m_desc" >水印文字</span>
            </td>
        </tr>
        <tr class="row">
            <td><label class="" for="<%=ddlWatermarkFontName.ClientID %>">文字型水印字体:</label></td>
            <td>
                  <asp:DropDownList ID="ddlWatermarkFontName" runat="server" Width="160"  CssClass="text"></asp:DropDownList>
            </td>
        </tr>
        <tr class="row">
            <td><label class="" for="<%=ddlWatermarkFontSize.ClientID %>">文字型水印大小:</label></td>
            <td>
                    <asp:DropDownList ID="ddlWatermarkFontSize" runat="server" Width="160" >
                        <asp:ListItem Value="60" Text="60px"></asp:ListItem>
                        <asp:ListItem Value="48" Text="48px"></asp:ListItem>
                        <asp:ListItem Value="36" Text="36px"></asp:ListItem>
                        <asp:ListItem Value="24" Text="24px"></asp:ListItem>
                        <asp:ListItem Value="20" Text="20px"></asp:ListItem>
                        <asp:ListItem Value="18" Text="18px"></asp:ListItem>
                        <asp:ListItem Value="16" Text="16px"></asp:ListItem>
                        <asp:ListItem Value="14" Text="14px"></asp:ListItem>
                        <asp:ListItem Value="12" Text="12px"></asp:ListItem>
                        <asp:ListItem Value="10" Text="10px"></asp:ListItem>
                    </asp:DropDownList>
            </td>
        </tr>
        <tr class="row">
            <td><label class="" for="<%=txtWatermarkImage.ClientID %>">图片型水印文件名:</label></td>
            <td>
                <asp:TextBox ID="txtWatermarkImage"    runat="server" CssClass="text"></asp:TextBox>
                <span class="m_desc" >请将图片放于<%=ConfigHelper.SitePath  %>common/images/watermark/下,如不存在将自动启用文字型水印</span>
            </td>
        </tr>
        <tr class="row">
            <td><label class="" for="<%=ddlWatermarkTransparency.ClientID %>">图片型水印透明度:</label></td>
            <td>
             <asp:DropDownList ID="ddlWatermarkTransparency" runat="server" Width="160" ></asp:DropDownList>
            </td>
        </tr>
        <tr class="row">
            <td><label class="" for="<%=ddlWatermarkPosition.ClientID %>">水印定位:</label></td>
            <td>
                  <asp:DropDownList ID="ddlWatermarkPosition" runat="server" Width="160" >
                        <asp:ListItem Value="1" Text="左上"></asp:ListItem>
                        <asp:ListItem Value="2" Text="左下"></asp:ListItem>
                        <asp:ListItem Value="3" Text="右上"></asp:ListItem>
                        <asp:ListItem Value="4" Text="右下"></asp:ListItem>
                        <asp:ListItem Value="5" Text="中心"></asp:ListItem>
                    </asp:DropDownList>
            </td>
        </tr>
        <tr class="rowend">
            <td><label class="" for="<%=ddlWatermarkQuality.ClientID %>">上传图片质量:</label></td>
            <td>               
                  <asp:DropDownList ID="ddlWatermarkQuality" runat="server" Width="160" ></asp:DropDownList>
                  <span class="m_desc">选择适合的质量,可大大减少图片体积</span>
            </td>
        </tr>
    </table>
<h4 id="footers">页脚设置<span class="small gray normal">(支持Html,网站统计,备案号等可放于此)</span></h4>
<table width="100%">
        <tr class="row">
            <td  style="width:20%;"><label class="" for="<%=txtFooterHtml.ClientID %>">页脚内容:</label></td>
            <td><asp:TextBox ID="txtFooterHtml" Width="80%" Height="100px" TextMode="multiLine"    runat="server"></asp:TextBox></td>
        </tr>
        <tr class="rowend">
            <td>&nbsp;</td>
            <td><asp:Button ID="btnEdit" CssClass="button" runat="server" OnClick="btnEdit_Click" Text="保存"  /></td>
        </tr>
    </table>
</asp:Content>

