<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_postlist" Title="无标题页" Codebehind="postlist.aspx.cs" %>
<%@ Register Assembly="Loachs.Core" Namespace="Loachs.Controls" TagPrefix="loachs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>文章管理</h2>
<%=ResponseMessage%>
<div  class="right">
    <h4>搜索</h4>
    <p>
        <label class="label" for="<%=txtKeyword.ClientID %>">关键字:</label>
        <asp:TextBox ID="txtKeyword" runat="server" CssClass="text"   ></asp:TextBox>
    </p>
    <p>
        <label class="label"  for="<%=ddlCategory.ClientID %>">分类:</label>
        <asp:DropDownList ID="ddlCategory" runat="server" Width="160"  ></asp:DropDownList>
    </p>
    <p>
        <label class="label"  for="<%=ddlAuthor.ClientID %>">作者:</label>
        <asp:DropDownList ID="ddlAuthor" runat="server" Width="160" ></asp:DropDownList>
    </p>
    <p>
        <asp:CheckBox ID="chkRecommend" runat="server" Text="推荐文章"  />
        <asp:CheckBox ID="chkHideStatus" runat="server" Text="列表隐藏" ToolTip="不会出现在文章列表,常用于留言本,关于我的页面!"  />
    </p>
    <p><asp:Button ID="btnSearch" runat="server"  Text="搜索"  OnClick="btnSearch_Click"  CssClass="button" /></p>
    
</div>
<div class="left" >
    <table width="100%">
        <tr class="category">
            <td style="width:60%;">标题</td>
            <td>评论/浏览</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptPost" runat="server">
            <ItemTemplate>
                <tr class="row">
                    <td>
                        [<%# DataBinder.Eval(Container.DataItem, "Author.Link")%>]
                        <%# DataBinder.Eval(Container.DataItem, "Link")%>
                        <%# DataBinder.Eval(Container.DataItem, "TopStatus").ToString()=="1" ?"[置顶]" : ""%>
                        <%# DataBinder.Eval(Container.DataItem, "Recommend").ToString() == "1" ? "[推荐]" : ""%>
                        <%# DataBinder.Eval(Container.DataItem, "Status").ToString()=="1" ?"" : "[草稿]"%>
                        <%# DataBinder.Eval(Container.DataItem, "HideStatus").ToString() == "1" ? "[隐藏]" : ""%>
                    </td>
                    <td><%# DataBinder.Eval(Container.DataItem, "CommentCount")%>/<%# DataBinder.Eval(Container.DataItem, "ViewCount")%></td>
                   
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateDate")).ToString("yyyy-MM-dd")%></td>
                    <td>
                       <%#GetEditLink(DataBinder.Eval(Container.DataItem, "postid"), DataBinder.Eval(Container.DataItem, "userid"))%>
                       <%#GetDeleteLink(DataBinder.Eval(Container.DataItem, "postid"), DataBinder.Eval(Container.DataItem, "userid"))%>
                       
                    </td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <loachs:Pager id="Pager1" runat="server" PageSize="15"  CssClass="pager"></loachs:Pager>
</div>
</asp:Content>

