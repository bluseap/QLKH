<%@ Page Language="C#" AutoEventWireup="true" Inherits="EOSCRM.Web.Login" CodeBehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>EOS-CRM > Đăng nhập hệ thống</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="content/images/eosicon.ico" />
    <link type="text/css" href="content/css/core.css" rel="stylesheet" />
    <link type="text/css" href="content/css/login.css" rel="stylesheet" />
    <!--[if IE]>
		<link type="text/css" href="content/css/fixie.css" rel="stylesheet" />
	<![endif]-->
    <!--[if IE 6]>
		<link type="text/css" href="content/css/fixieIE6.css" rel="stylesheet" />
	<![endif]-->

    <script type="text/javascript" src="content/scripts/jquery-1.4.2.min.js"></script>
    <!-- Addon for background tiling support -->
    <script type="text/javascript" src="content/scripts/iepngfix_tilebg.js"></script>
    <style type="text/css">
        img, div, input { behavior: url("content/scripts/iepngfix.htc") }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div id="wrapper">
        <div id="container">
            <div id="login">
                <div id="lform">
                    <div>
                        <label for="txtUsername">
                            Username</label></div>
                    <div>
                        <asp:TextBox ID="txtUserName" runat="server" TabIndex="1" />
                        <asp:RequiredFieldValidator ID="reqUsername" runat="server" ErrorMessage="*" ControlToValidate="txtUserName"
                            SetFocusOnError="True"></asp:RequiredFieldValidator></div>
                    <div class="mr10">
                        <label for="txtPassword">
                            Password</label></div>
                    <div>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" TabIndex="2" /></div>
                    <div class="mr10">
                        <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="btn_lg" TabIndex="3" />
                        <asp:Button ID="btnReset" TabIndex="4" runat="server" CssClass="btn_cc" OnClick="btnReset_Click" />
                    </div>
                    <div class="mr10 err">
                        <asp:Label ID="lblErrorMsg" runat="server" Font-Bold="True"></asp:Label>
                    </div>
                </div>
            </div>
        </div>        
        <div id="TracNghiem" align="center" style="font-size: x-large" >
            <asp:HyperLink ID="TSLamBai" runat="server" NavigateUrl="~/TracNghiem/TSLamBai.aspx" Visible="false">
                Hướng dẫn làm bài thi Trắc nghiệm
            </asp:HyperLink>
        
        </div>
    </div>
    </form>
</body>
</html>
