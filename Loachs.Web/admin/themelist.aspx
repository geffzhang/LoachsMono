<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_themelist" Title="无标题页" Codebehind="themelist.aspx.cs" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Loachs.Business" %>
<%@ Import Namespace="Loachs.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>主题管理</h2>
<%=ResponseMessage%>
<h4>电脑,手机主题<span class="small gray normal">(default 为系统默认主题,禁止删除,不建议修改)</span></h4>
<ul class="theme">
<%
    DirectoryInfo dir = new DirectoryInfo(Server.MapPath("../themes/"));
 foreach (DirectoryInfo d in dir.GetDirectories())
        {
            if (d.Name == ".svn") continue;
%>
    <li <%=(SettingManager.GetSetting().Theme == d.Name || SettingManager.GetSetting().MobileTheme == d.Name)?" class=\"current\"":"" %>  >
        <p class="gray"><%=ConfigHelper.SitePath %>themes/<%=d.Name %>/</p>
        <p><img src="../themes/<%=d.Name %>/theme.jpg" width="200" height="150" alt=""  style="border:1px solid #ccc;" /></p>
        <h5 style="margin:5px 0 3px 0 ;"><%=ThemeManager.GetTemplate(d.FullName).Name %></h5>
        <p  class="gray">作者: <a href="mailto:<%=ThemeManager.GetTemplate(d.FullName).Email %>"><%=ThemeManager.GetTemplate(d.FullName).Author %> </a></p>
        <p class="gray">主页: <a href="<%=ThemeManager.GetTemplate(d.FullName).SiteUrl %>" target="_blank"><%=ThemeManager.GetTemplate(d.FullName).SiteUrl %></a></p>
        <p class="gray" style="border-bottom:1px solid #ccc; padding-bottom:5px;">发布: <%=ThemeManager.GetTemplate(d.FullName).PubDate %></p>
       
        <p style="border-bottom:1px solid #ccc; padding:5px 0;">
        <%if (SettingManager.GetSetting().Theme == d.Name)
          {%>     电脑版使用
        <%}
          else
          { %>
         <a href="themelist.aspx?operate=update&type=pc&themename=<%=d.Name %>">电脑版使用</a>
           
        <%} %>
       
        <%if (SettingManager.GetSetting().MobileTheme == d.Name)
          {%>     手机版使用
        <%}
          else
          { %>
            <a href="themelist.aspx?operate=update&type=mobile&themename=<%=d.Name %>">手机版使用</a> 
        <%} %>
        </p>
        <p style=" padding-top:5px ;">
            <a href="../theme/<%=d.Name %>.aspx">预览</a>
            <a href="themeedit.aspx?themename=<%=d.Name%>" title="编辑模板,样式,脚本,其它文本文件">编辑</a> 
            <a href="themelist.aspx?operate=insert&themename=<%=d.Name %>"  title="复制该主题">复制</a>
            <%if (d.Name != "default")
              { %>
            <a href="themelist.aspx?operate=delete&themename=<%=d.Name %>" onclick=" return confirm('确定要删除吗?');" title="删除该主题">删除</a>
            <%} %>
       </p>
    </li>
    
<%} %>
</ul>
</asp:Content>

