<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_themeedit" Title="无标题页" Codebehind="themeedit.aspx.cs" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Loachs.Business" %>
<%@ Import Namespace="Loachs.Common" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>主题编辑(<%=themeName %>)</h2>
<%=ResponseMessage %>
<div  class="right">
    <h4>模版文件</h4>
    <ul>
    <% DirectoryInfo dir = new DirectoryInfo(Server.MapPath(ConfigHelper.SitePath + "themes/" + themeName +"/template"));
       foreach (FileSystemInfo f in dir.GetFiles())
       {
    %>
        <li style="line-height:150%;"><a href="themeedit.aspx?themename=<%=themeName %>&filepath=template/<%=f.Name %>"><%=f.Name %></a></li>
    <%} %>
    </ul>
    
    <% 
        string cssPath = Server.MapPath(ConfigHelper.SitePath + "themes/" + themeName + "/css");
        if (System.IO.Directory.Exists(cssPath))
        {
            %>
            <h4>样式文件</h4>
    <ul>
            <%
            DirectoryInfo dircss = new DirectoryInfo(cssPath);
            foreach (FileSystemInfo f in dircss.GetFiles())
            {
    %>
        <li style="line-height:150%;"><a href="themeedit.aspx?themename=<%=themeName %>&filepath=css/<%=f.Name %>"><%=f.Name%></a></li>
    <%}
            %>
            </ul>
            <%
        } %>
    
    <h4>其它文本文件</h4>
    <ul>
    <% DirectoryInfo dirother = new DirectoryInfo(Server.MapPath(ConfigHelper.SitePath + "themes/" + themeName ));
       foreach (FileInfo f in dirother.GetFiles())
       {

           if (!(f.Extension.ToLower() == ".xml" || f.Extension.ToLower() == ".aspx" || f.Extension.ToLower() == ".html" || f.Extension.ToLower() == ".htm" || f.Extension.ToLower() == ".css" || f.Extension.ToLower() == ".txt" || f.Extension.ToLower() == ".js" || f.Extension.ToLower() == ".config"))
           {
               continue;
           }
    %>
        <li style="line-height:150%;"><a href="themeedit.aspx?themename=<%=themeName %>&filepath=<%=f.Name %>"><%=f.Name %></a></li>
    <%} %>
    </ul>
</div>
<div class="left" >
    <h4>文件: <%=ConfigHelper.SitePath  %>themes/<%=themeName %>/<%=filePath %> </h4>
    <p style="margin-top:0;">
        <asp:TextBox ID="txtContent" runat="server" TextMode="MultiLine" Height="450" Width="98%"></asp:TextBox>
    </p>
    <p>
        <asp:Button ID="btnSave" runat="server"     Text="保存" onclick="btnSave_Click"  CssClass="button"/>
        <input type="button"  onclick="location.href='themelist.aspx'"  value="返回"/>
    
    </p>
</div>
 
 
</asp:Content>

