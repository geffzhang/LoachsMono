<%@ Page Language="C#" MasterPageFile="admin.master" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_userlist" Title="无标题页" Codebehind="userlist.aspx.cs" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server"></asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<h2>作者管理</h2>
 <%=ResponseMessage %>
<div class="right">
    <h4>添加作者</h4>
    <p>
        <label class="label" for="<%=ddlUserType.ClientID %>">角色:</label>
        <asp:DropDownList ID="ddlUserType" runat="server" Width="99%">
            <asp:ListItem Value="1" Text="管理员"></asp:ListItem>
            <asp:ListItem Value="5" Text="写作者"></asp:ListItem>
        </asp:DropDownList>
    </p>
    <p>
        <label class="label" for="<%=txtUserName.ClientID %>">用户名:<span class="small gray">(字母,数字,中文,连字符)</span></label>
        <asp:TextBox ID="txtUserName"    MaxLength="50"  runat="server" Width="96%"  CssClass="text" ></asp:TextBox>
    </p>
    <p>
        <label class="label" for="<%=txtNickName.ClientID %>">昵称:</label>
        <asp:TextBox ID="txtNickName"   MaxLength="50"  runat="server" Width="96%" CssClass="text" ></asp:TextBox>
    </p>
    <p>
        <label class="label" for="<%=txtPassword.ClientID %>">密码:<span class="small gray"><%=PasswordMessage %></span></label>
        <asp:TextBox ID="txtPassword" TextMode="password"    runat="server" Width="96%" CssClass="text" ></asp:TextBox>
    </p>
    <p>
        <label class="label" for="<%=txtPassword2.ClientID %>">确认密码:<span class="small gray"><%=PasswordMessage %></span></label>
        <asp:TextBox ID="txtPassword2" TextMode="password" runat="server" Width="96%" CssClass="text" ></asp:TextBox>
    </p>
    <p><label class="label" for="<%=txtEmail.ClientID %>">邮箱:</label>
        <asp:TextBox ID="txtEmail"   runat="server" Width="96%" CssClass="text" ></asp:TextBox>
    </p>
    <p><label class="label" for="<%=txtDisplayOrder.ClientID %>">排序:</label><asp:TextBox ID="txtDisplayOrder" runat="server" Width="50" CssClass="text"></asp:TextBox>
        <span class="m_desc">越小越靠前</span>
    </p>
    <p>
        <asp:CheckBox ID="chkStatus" runat="server" Text="使用" Checked="true" />
     </p>
     <p>
        <asp:Button ID="btnEdit"  runat="server" OnClick="btnEdit_Click" Text="添加"  CssClass="button"/>
     </p>
</div>
<div class="left" >
    <table width="100%">
        <tr class="category">
            <td>排序</td>
            <td>作者(用户名)</td>
            <td>文章/评论</td>
            <td>状态</td>
            <td>创建日期</td>
            <td>操作</td>
        </tr>
        <asp:Repeater ID="rptUser" runat="server">
            <ItemTemplate>
                <tr class="row">                 
                    <td>
                    
                    <%# DataBinder.Eval(Container.DataItem, "Displayorder")%></td>
                    <td>
                        [<%#GetUserType(DataBinder.Eval(Container.DataItem, "Type"))%>]
                        <%# DataBinder.Eval(Container.DataItem, "Link")%>(<%# DataBinder.Eval(Container.DataItem, "UserName")%>)
                    </td>
                    
                    <td><%#  DataBinder.Eval(Container.DataItem,"PostCount")%> / <%# DataBinder.Eval(Container.DataItem, "CommentCount")%></td>
                    
                    <td><%# DataBinder.Eval(Container.DataItem, "Status").ToString() == "1" ? "<img src=\"../common/images/admin/yes.gif\" title=\"使用\"/>" : "<img src=\"../common/images/admin/no.gif\" title=\"停用\"/>"%></td>
                    <td><%# Convert.ToDateTime( DataBinder.Eval(Container.DataItem, "CreateDate")).ToString("yyyy-MM-dd")%></td>
                    <td><a href="userlist.aspx?operate=update&userId=<%# DataBinder.Eval(Container.DataItem, "userId")%>">编辑</a> <a href="userlist.aspx?operate=delete&userId=<%# DataBinder.Eval(Container.DataItem, "userId")%>" onclick="return confirm('删除作者不会删除作者发表的文章和评论,确定要删除吗?');">删除</a></td>
                </tr>
            </ItemTemplate>
        </asp:Repeater>
    </table>
</div>
</asp:Content>