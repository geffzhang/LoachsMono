<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" Inherits="Loachs.Web.admin_commentlist" Title="无标题页" Codebehind="commentlist.aspx.cs" %>
<%@ Import Namespace="Loachs.Common" %>
<%@ Import Namespace="Loachs.Entity" %>
<%@ Register Assembly="Loachs.Core" Namespace="Loachs.Controls" TagPrefix="loachs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<script type="text/javascript">
var checkFlag = true;
function chooseAll()
{
　　if( checkFlag ) // 全选　
　　{
　　　　　var inputs =  document.getElementsByTagName("input");
　　　　　for (var i=0; i < inputs.length; i++)  
　　　　　{
　　　　　　　if (inputs[i].type == "checkbox" && inputs[i].id != "checkAll" )
　　　　　　　{
　　　　　　　　　　inputs[i].checked = true;
　　　　　　　}     
　　　　　}
　　　　　checkFlag = false;
　　}
　　else  // 取消 
　　{
　　　　　var inputs =  document.getElementsByTagName("input");
　　　　　for (var i=0; i < inputs.length; i++)  
　　　　　{
　　　　　　　if (inputs[i].type == "checkbox" && inputs[i].id != "checkAll" )
　　　　　　　{
　　　　　　　　　　inputs[i].checked = false;
　　　　　　　}
　　　　　}
　　　　　checkFlag = true;
　　}
}

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>评论管理</h2>
    <%=ResponseMessage%>
    <table width="100%" >
        <tr class="category">
            <td style="width:7%;">选择</td>
            <td style="width:50%;">评论</td>
            
            <td style="width:25%;">作者</td>
            <td style="width:8%;"><a href="commentlist.aspx?approved=<%=(int)ApprovedStatus.Wait %>">审核</a></td>
            <td style="width:10%;">操作</td>
        </tr>
        <asp:Repeater ID="rptComment" runat="server">
            <ItemTemplate>
                <tr class="row">
                    <td><input type="checkbox" id="chkRow" runat="server" value='<%# DataBinder.Eval(Container.DataItem, "CommentId")%>' /></td>
                    <td style="line-height:150%;">
                      <%# GetPostLink(Convert.ToInt32(DataBinder.Eval(Container.DataItem, "PostId")))%>
                    <br />
                    <a href="<%#DataBinder.Eval(Container.DataItem, "Url") %>">
                        <span title="<%#DataBinder.Eval(Container.DataItem, "Content") %>">
                        <%# StringHelper.CutString( StringHelper.RemoveHtml( DataBinder.Eval(Container.DataItem, "Content").ToString()),60,"...")%>
                        </span>
                    </a>
                        <br />
                        <span class="gray small" ><%#DataBinder.Eval(Container.DataItem, "CreateDate")%></span>
                    </td>
                   
                    <td>      
                       <div  style="float:left; padding:0 8px 0 0;">
                            <img src="http://www.gravatar.com/avatar/<%#DataBinder.Eval(Container.DataItem, "GravatarCode")%>?size=32" style="width:32px ;height :32px; border:1px solid #999;" alt="gravatar 图像" title="gravatar 图像"/>
                                  
                        </div>
                        <div style="float:left;">
                                <%#  DataBinder.Eval(Container.DataItem, "AuthorLink")%>
                                <span class="gray small">(<%# DataBinder.Eval(Container.DataItem, "IPAddress")%>)</span>
                        <br />
                        <%#  DataBinder.Eval(Container.DataItem, "Email")  %>

                        </div>
                                </td>
                     
                     
                    <td><%# DataBinder.Eval(Container.DataItem, "Approved").ToString()=="1" ?"<img src=\"../common/images/admin/yes.gif\" title=\"已审核\"/>" : "<img src=\"../common/images/admin/no.gif\" title=\"未审核\"/>"%></td>
                    <td>
                            <%#  DataBinder.Eval(Container.DataItem, "Approved").ToString()=="0"?"<a href=\"commentlist.aspx?operate=update&commentid="+DataBinder.Eval(Container.DataItem, "commentId")+"&page="+Pager1.PageIndex+"\">审核</a>":""%>
                         
                         <a href="commentlist.aspx?operate=delete&commentid=<%#DataBinder.Eval(Container.DataItem, "commentId")%>&page=<%=Pager1.PageIndex %>" onclick="return confirm('确定要删除吗?')">删除</a>   
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
        <tr class="rowend">
            <td colspan="8">
                <input id="checkAll" onclick="chooseAll()" type="checkbox" name="checkAll" /><label for="checkAll">全选</label>
                <asp:Button ID="btnApproved" runat="server"   Text="审 核" OnClick="btnApproved_Click" CssClass="button" />
                <asp:Button ID="btnDelete" runat="server"   Text="删 除" OnClientClick="return confirm('确定要删除吗?')" OnClick="btnDelete_Click"  CssClass="button" />
            </td>
        </tr>
    </table>
    <loachs:Pager id="Pager1" runat="server" PageSize="20"  CssClass="pager"></loachs:Pager>
 

</asp:Content>
