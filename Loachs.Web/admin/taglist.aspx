<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true"  validateRequest="false" Inherits="Loachs.Web.admin_taglist" Title="无标题页" Codebehind="taglist.aspx.cs" %>
<%@ Register Assembly="Loachs.Core" Namespace="Loachs.Controls" TagPrefix="loachs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>标签管理</h2>
<%=ResponseMessage%>
<div  class="right">
    <h4>添加标签</h4>
    <p><label class="label" for="<%=txtName.ClientID %>">名称:</label><asp:TextBox ID="txtName" runat="server"  Width="96%"  CssClass="text"></asp:TextBox></p>
    <%--<p><label class="label"  for="<%=txtSlug.ClientID %>">别名:<span  class="gray small"  > (字母,数字,中文,连字符)</span></label><asp:TextBox ID="txtSlug" runat="server"  Width="96%"  CssClass="text"></asp:TextBox></p>--%>
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
                    <td><a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>"  ><%# DataBinder.Eval(Container.DataItem, "Name")%></a></td>
                    <td><%# DataBinder.Eval(Container.DataItem, "Description")%></td>
                    <td><%#  DataBinder.Eval(Container.DataItem,"Count")%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateDate")).ToString("yyyy-MM-dd")%></td>            
                    <td><a href="taglist.aspx?operate=update&tagid=<%# DataBinder.Eval(Container.DataItem, "TagId")%>&page=<%=Pager1.PageIndex %>">编辑</a> <a href="taglist.aspx?operate=delete&tagid=<%# DataBinder.Eval(Container.DataItem, "TagId")%>&page=<%=Pager1.PageIndex %>" onclick="return confirm('确定要删除吗?');">删除</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
    <loachs:Pager id="Pager1" runat="server" PageSize="20"  CssClass="pager"></loachs:Pager>
</div>
</asp:Content>

