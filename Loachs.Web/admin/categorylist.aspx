<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_categorylist" Title="无标题页" Codebehind="categorylist.aspx.cs" %>
<%@ Register Assembly="Loachs.Core" Namespace="Loachs.Controls" TagPrefix="loachs" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>分类管理</h2>
<%=ResponseMessage%>
<div  class="right">
    <h4>添加分类</h4>
    <p><label class="label" for="<%=txtName.ClientID %>">名称:</label><asp:TextBox ID="txtName" runat="server"  Width="96%"  CssClass="text"></asp:TextBox></p>
    <p><label class="label"  for="<%=txtSlug.ClientID %>">别名:<span  class="gray small"  > (字母,数字,中文,连字符)</span></label><asp:TextBox ID="txtSlug" runat="server"  Width="96%"  CssClass="text"></asp:TextBox></p>
    <p><label class="label"  for="<%=txtDescription.ClientID %>">描述:</label><asp:TextBox ID="txtDescription" runat="server"  Width="96%"  CssClass="text"></asp:TextBox></p>
    <p><label class="label" for="<%=txtDisplayOrder.ClientID %>">排序:</label><asp:TextBox ID="txtDisplayOrder" runat="server" Width="30" CssClass="text"></asp:TextBox>
    <span class="m_desc">越小越靠前</span></p>
    <p><asp:Button ID="btnEdit" runat="server"  Text="添加" onclick="btnEdit_Click"  CssClass="button"/></p>
</div>
<div class="left" >
    <table width="100%">
        <tr class="category">
            <td>排序</td>
            <td>名称</td>
            <td style="width:40%;">描述</td>
            <td>文章</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptCategory" runat="server">
            <ItemTemplate>
                <tr class="row">
                    <td><%# DataBinder.Eval(Container.DataItem, "Displayorder")%></td>
                    <td><a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" ><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Description")%></td>
                    <td><%#  DataBinder.Eval(Container.DataItem,"Count")%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateDate")).ToString("yyyy-MM-dd")%></td>            
                    <td><a href="categorylist.aspx?operate=update&categoryid=<%# DataBinder.Eval(Container.DataItem, "categoryid")%>">编辑</a> <a href="categorylist.aspx?operate=delete&categoryid=<%# DataBinder.Eval(Container.DataItem, "categoryid")%>" onclick="return confirm('删除分类不会删除所属的文章,确定要删除吗?');">删除</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
     
</div>
 
</asp:Content>

