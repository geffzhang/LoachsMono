<%@ Page Language="C#" AutoEventWireup="true" validateRequest="false" Inherits="Loachs.Web.admin_login" Codebehind="login.aspx.cs" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>登录</title>
    <link rel="stylesheet" href="../common/css/admin.css?v=1.2" type="text/css" media="screen" />
    <script type="text/javascript">
        if (top.location != self.location) {top.location=self.location;}
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="login"  >
        <p>
            <label for="txtUserName" style="display:block; margin:3px 0;" class="small">用户名:</label>
            <asp:TextBox ID="txtUserName"  runat="server" Width="200" CssClass="text"></asp:TextBox>
        </p>
        <p>
            <label for="txtPassword" style="display:block;margin:3px 0;" class="small ">密码:</label>
            <asp:TextBox ID="txtPassword" TextMode="password" runat="server" Width="200"  CssClass="text"></asp:TextBox>
        </p>
        <p>
            <label for="txtVerifyCode" style="display:block;margin:2px 0;" class="small">验证码:</label>
            <asp:TextBox ID="txtVerifyCode"   runat="server" Width="50"  CssClass="text"></asp:TextBox>      
            <img align="absMiddle"  onclick="this.src='../common/tools/verifyimage.aspx?time=' + Math.random()" src="../common/tools/verifyimage.aspx" style="cursor: pointer;" alt="点击刷新"/>
        </p>
        <p>
            <asp:CheckBox ID="chkRemember" runat="server" Text="保持登录(一个月)"  CssClass="small"/>
        </p>
        <p>
            <asp:Button ID="btnLogin" runat="server"   Text="登录" OnClick="btnLogin_Click" CssClass="button" />
        </p>
        <p ><asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label></p>
        <p><a href="../" class="small">&laquo;返回首页</a></p>
    </div>
    <div  style="  text-align:center; padding:5px 0;" class="small">Powered by <a href="http://www.loachs.com" target="_blank" >Loachs</a></div>
    </form>
    <script type="text/javascript"> 
        document.getElementById('<%=txtUserName.ClientID %>').focus();
    </script> 
</body>
</html>
