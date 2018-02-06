<%@ Page Language="C#" AutoEventWireup="true" Inherits="EOSCRM.Web.Login" CodeBehind="Login.aspx.cs" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <title>EOS-CRM > Đăng nhập hệ thống</title>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link rel="shortcut icon" href="content/images/powaco.ico" />
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
        .auto-style2 {
            width: 251px;
        }
    </style>
</head>
<body>
    
    <form id="form1" runat="server">
        <table cellpadding="2" cellspacing="10" style="width: auto;" width="100%" >
            <tr>
                <td width="80%">                                       
                     <table cellpadding="3" cellspacing="10" style="width: auto;" width="80%" >
                        <tr>
                            <td width="10%">
                                <div  >
                                    <img src="/content/images/powacmo.png"/>
                                </div>
                            </td>                                                
                            <td width="70%" valign="top">
                                <div align="left" style="width: 250px" >
                                    <asp:LinkButton ID="LinkButton1" runat="server" Font-Bold="False" Font-Size="X-Large">Giới thiệu</asp:LinkButton>
                                </div>
                                <div align="left">
                                    <asp:Label ID="Label2" runat="server" Text="Công ty cổ phần"></asp:Label>
                                </div>
                                <div align="right" >
                                    <asp:LinkButton ID="LinkButton2" runat="server" >Xem chi tiết...</asp:LinkButton>
                                </div>
                            </td>
                        </tr>  
                         <tr>
                             <td height="20px"  >
                             </td>
                         </tr> 
                         <tr>
                            <td width="10%">
                                <div align="left" style="width: 250px" >
                                    <img src="/content/images/tintuc3.jpeg"/>
                                </div>
                            </td>                             
                            <td width="70%" valign="top">
                                <div align="left">
                                    <asp:LinkButton ID="LinkButton3" runat="server" Font-Bold="False" Font-Size="X-Large">Tin tức</asp:LinkButton>
                                </div>
                                <div >
                                    <asp:Label ID="Label1" runat="server" Text="Công ty cổ phần"></asp:Label>
                                </div>
                                <div align="right" >
                                    <asp:LinkButton ID="LinkButton4" runat="server" >Xem chi tiết...</asp:LinkButton>
                                </div>
                            </td>
                        </tr>  
                         <tr>
                             <td height="20px" >
                             </td>
                         </tr>   
                         <tr>
                            <td width="10%">                                
                                <div align="left" style="width: 250px" >
                                    <img src="/content/images/kh2.png"/>
                                </div>
                            </td>                                                          
                            <td width="70%" valign="top" >
                                <div align="left" >
                                    <asp:LinkButton ID="LinkButton5" runat="server" Font-Bold="False" Font-Size="X-Large">Khách hàng</asp:LinkButton>
                                </div>
                                <div >
                                    <asp:Label ID="Label3" runat="server" Text="Công ty cổ phần"></asp:Label>
                                </div>
                                <div align="right" >
                                    <asp:LinkButton ID="LinkButton6" runat="server" >Xem chi tiết...</asp:LinkButton>
                                </div>
                            </td>
                        </tr>    
                     </table>              
                </td>
                <td width="20%">
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
                        <div id="longxuyen" align="center" style="font-size: large" >
              <asp:HyperLink ID="hpLONGXUYEN" runat="server" NavigateUrl="~/GisWeb/glongxuyen.aspx" Visible="true">
                                Long Xuyên
              </asp:HyperLink>        
         </div>
        <div id="phutan" align="center" style="font-size: large" >
              <asp:HyperLink ID="hpPHUTAN" runat="server" NavigateUrl="~/GisWeb/gphutan.aspx" Visible="true">
                                Phú Tân
              </asp:HyperLink>        
         </div>
        <div id="thoaison" align="center" style="font-size: large" >
              <asp:HyperLink ID="hpTHOAISON" runat="server" NavigateUrl="~/GisWeb/gthoaison.aspx" Visible="true">
                                Thoại Sơn
              </asp:HyperLink>        
         </div>     
                        <div id="TracNghiem" align="center" style="font-size: x-large" >
                            <asp:HyperLink ID="TSLamBai" runat="server" NavigateUrl="~/TracNghiem/TSLamBai.aspx" Visible="false">
                                Hướng dẫn làm bài thi Trắc nghiệm
                            </asp:HyperLink>
        
                        </div>
                    </div>
                    </td>
                </tr>
       </table>
    </form>
</body>
<script type='text/javascript'>window._sbzq || function (e) { e._sbzq = []; var t = e._sbzq; t.push(["_setAccount", 12492]); var n = e.location.protocol == "https:" ? "https:" : "http:"; var r = document.createElement("script"); r.type = "text/javascript"; r.async = true; r.src = n + "//static.subiz.com/public/js/loader.js"; var i = document.getElementsByTagName("script")[0]; i.parentNode.insertBefore(r, i) }(window);</script> 
</html>
