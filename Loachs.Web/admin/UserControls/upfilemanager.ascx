<%@ Control Language="C#" AutoEventWireup="true" Inherits="Loachs.Web.admin_usercontrols_upfilemanager" Codebehind="upfilemanager.ascx.cs" %>
<%@ Import Namespace="System.IO" %>
<%@ Import Namespace="Loachs.Business" %>
<%@ Import Namespace="Loachs.Common" %>
<%@ Import Namespace="Loachs.Web" %>
<h2>附件管理</h2>
 <%=ResponseMessage%>
<div  class="right">           
    <h4>上传附件</h4>
    <p><%=ConfigHelper.SitePath  %>upfiles/<%=DateTime.Now.ToString("yyyyMM") %></p>
    <p><asp:FileUpload ID="FileUpload1" size="15" runat="server" Width="180px"  /></p>
    <p><asp:CheckBox ID="chkWatermark" runat="server" Text="加水印(图片)" Checked="true" /> <a href="setting.aspx#watermarks" class="small">水印详细设置</a> </p>
    <p>存在同名文件时:</p>
    <p> 
        <asp:RadioButtonList ID="rblistType" runat="server" RepeatLayout="Flow" RepeatDirection="Horizontal">
            <asp:ListItem Value="1" Text="跳过" Selected="True"></asp:ListItem>
            <asp:ListItem Value="2" Text="重命名"></asp:ListItem>
            <asp:ListItem Value="3" Text="覆盖"></asp:ListItem>
        </asp:RadioButtonList>
    </p>
    <p><asp:Button ID="btnUpload" runat="server"  Text="上传" onclick="btnUpload_Click"  CssClass="button"/></p>
    <p class="notice" style=" word-wrap : break-word ;word-break : break-all ;overflow:auto;" title="<%=AllowFileExtension%>">允许格式:<br /><%=AllowFileExtension%>.</p>
</div>
<div class="left" >
    <h4>当前位置: <%=GetPathUrl()%></h4>
    <ul class="upfile">
    <%
        foreach (FileSystemInfo d in currentDirectory.GetFileSystemInfos())
            {
    %>
        <li>
        <%if (d is DirectoryInfo)
          {%>
            <a href="<%=FileName %>?path=<%=path %><%=d.Name %>/" title="点击打开此文件夹"><img src="../common/images/file/folder.png"  alt="点击打开此文件夹" width="48" height="48"/></a>
             <p class="small">
                <span title="文件夹:<%=d.Name %>" ><%=d.Name %> </span>
                <br />
                <span class="gray"><%=((DirectoryInfo)d).GetFileSystemInfos().Length %> 个对象</span>
             </p>
        <%}
          else 
          { %>
                <a href="<%=path+d.Name %>" target="_blank" onclick=" return returnValue('<%=path+d.Name %>','<%=d.Extension %>');">
             <%if (IsImage(d.Extension))
               { %>
                <img src="<%=path+d.Name %>" width="48" height="48"/>
             <%}
               else
               { %>
                <img src="../common/images/file/<%=GetFileImage(d.Extension) %>"  width="48" height="48"/>
             <%} %>
                </a>
                <p class="small">
                <span title="<%=d.Name %>" ><%=d.Name %> </span>
                <br />
                <span class="gray"><%=PageUtils. ConvertUnit( ((FileInfo)d).Length )%></span>
                </p>
        <%} %>
        <a class="delete" href="<%=FileName %>?operate=delete&category=<%= d.Attributes%>&deletepath=<%=path %><%=d.Name %>"  title="删除" onclick=" return confirm('确定要删除 <%=d.Name %> 吗?');">X</a>
        </li>
        
    <%} if (currentDirectory.GetFileSystemInfos().Length == 0) { Response.Write("<p>还没有上传任何附件!</p>"); } %>
    </ul>
</div>

<script type="text/javascript">
function returnValue(fileUrl,fileExtension){
<%if(FileName=="upfilebyeditor.aspx"){ %>
    parent.addFileToEditor(fileUrl,fileExtension) ;
    parent.tb_remove();
    return false;
<%} %>
}
</script>