<%@ Page Language="C#" AutoEventWireup="true" Inherits="Loachs.Web.admin_upfilebyeditor"
    CodeBehind="upfilebyeditor.aspx.cs" %>

<%@ Register Src="UserControls/upfilemanager.ascx" TagName="upfilemanager" TagPrefix="uc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <link type="text/css" rel="stylesheet" href="../common/css/admin.css" />
    <style type="text/css">
        #content
        {
            margin: 0px;
            border: 0px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="content">
        <uc1:upfilemanager ID="upfilemanager1" runat="server" />
        <div style="clear: both;">
        </div>
    </div>
    </form>
</body>
</html>
