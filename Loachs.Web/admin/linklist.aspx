<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_linklist" Title="无标题页" Codebehind="linklist.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>链接管理</h2>
<%=ResponseMessage%>
<div  class="right">
    <h4>添加链接</h4>
    <p><label class="label"  for="<%=txtName.ClientID %>">名称:</label><asp:TextBox ID="txtName" runat="server" Width="96%"  CssClass="text" ></asp:TextBox></p>
    <p><label class="label"  for="<%=txtHref.ClientID %>">链接地址:</label><asp:TextBox ID="txtHref" runat="server" Width="96%"   CssClass="text"></asp:TextBox></p>
    <p><label class="label"  for="<%=txtDescription.ClientID %>">描述:</label><asp:TextBox ID="txtDescription" runat="server" Width="96%"  CssClass="text" ></asp:TextBox></p>
    <p><label class="label" for="<%=txtDisplayOrder.ClientID %>">排序:</label><asp:TextBox ID="txtDisplayOrder" runat="server" Width="50" CssClass="text"></asp:TextBox>
        <span class="m_desc">越小越靠前</span>
    </p>
    <p>
        <asp:CheckBox ID="chkStatus" runat="server" Text="显示" Checked="true" />
        <asp:CheckBox ID="chkPosition" runat="server" Text="导航"   />
        <asp:CheckBox ID="chkTarget" runat="server" Text="新窗口"  Checked="true" />
    </p>
    <p><asp:Button ID="btnEdit" runat="server"  Text="添加"  onclick="btnEdit_Click"  CssClass="button"/></p>
    <p class="notice">小提示:${siteurl} 表示根目录.</p>
</div>
<div class="left" >
    <table width="100%">
        <tr class="category">
            <td>排序</td>
            <td>名称</td>
            <td style="width:40%;">描述</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptLink" runat="server">
            <ItemTemplate>
                <tr class="row">
                    <td><%# DataBinder.Eval(Container.DataItem, "Displayorder")%></td>
                    <td>
                        <a href="<%# DataBinder.Eval(Container.DataItem, "Url")%>" ><%# DataBinder.Eval(Container.DataItem, "Name")%></a>
                        <%# Convert.ToInt32(DataBinder.Eval(Container.DataItem, "Position")) == (int)Loachs.Entity.LinkPosition.Navigation ? "[导航]" : ""%>
                        <%# DataBinder.Eval(Container.DataItem, "Status").ToString()=="0"?"[隐藏]":""%>
                    </td>
                  
                    <td><%# DataBinder.Eval(Container.DataItem, "Description")%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateDate")).ToString("yyyy-MM-dd")%></td>
                           
                    <td>
                        <a href="linklist.aspx?operate=update&LinkId=<%# DataBinder.Eval(Container.DataItem, "LinkId")%>">编辑</a> 
                        <a href="linklist.aspx?operate=delete&LinkId=<%# DataBinder.Eval(Container.DataItem, "LinkId") %> " onclick="return confirm('确定要删除吗?');">删除</a></td>
                    
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>

</asp:Content>