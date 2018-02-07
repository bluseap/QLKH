<%@ Page Language="C#" AutoEventWireup="true" Inherits="EOSCRM.Web.Login" CodeBehind="Login.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>

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

    <link href="content/css/bootstrap.css" rel="stylesheet" />
    <link href="content/css/ie10-viewport-bug-workaround.css" rel="stylesheet" />
    <link href="content/css/signin.css" rel="stylesheet" />
    <script src="content/js/ie-emulation-modes-warning.js"></script>

    <script type="text/javascript" src="content/scripts/jquery-1.4.2.min.js"></script>
    <!-- Addon for background tiling support -->
    <script type="text/javascript" src="content/scripts/iepngfix_tilebg.js"></script>
    <style type="text/css">
        img, div, input { behavior: url("content/scripts/iepngfix.htc") }
        .auto-style2 {
            width: 251px;
        }
    </style>

    <script type="text/javascript">
        
       
        //var x = document.getElementById("demo");
        function getLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(showPosition, showError);

            }
            else { alert( "Geolocation is not supported by this browser."); }

            //alert("dasda asd");
            return;
        }

        function showPosition(position) {

            var latlondata = position.coords.latitude + "," + position.coords.longitude;
            var latlon = "Your Latitude Position is:=" + position.coords.latitude + "," + "Your Longitude Position is:=" + position.coords.longitude;
            alert(latlon);
           
            document.getElementById("<%=hfLATVT.ClientID %>").value = position.coords.latitude;
            document.getElementById("<%=hfLONGVT.ClientID %>").value = position.coords.longitude;                

            //__doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnReset) %>', '');
        }

        function showError(error) {
            if (error.code == 1) {
                alert( "User denied the request for Geolocation.");
            }
            else if (err.code == 2) {
                alert( "Location information is unavailable.");
            }
            else if (err.code == 3) {
                alert( "The request to get user location timed out.");
            }
            else {
                alert( "An unknown error occurred.");
            }
        }

        //window.onload = LoadgetLocation();
        function LoadgetLocation() {
            if (navigator.geolocation) {
                navigator.geolocation.getCurrentPosition(LoadshowPosition, showError);
            }
            else { alert("Geolocation is not supported by this browser."); }            
        }

        function LoadshowPosition(position) {

            var latlondata = position.coords.latitude + "," + position.coords.longitude;
            var latlon = "Lat is:=" + position.coords.latitude + "," + "Long is:=" + position.coords.longitude;
            //alert(latlon);

            document.getElementById("<%=hfLATVT.ClientID %>").value = position.coords.latitude;
            document.getElementById("<%=hfLONGVT.ClientID %>").value = position.coords.longitude;

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnReset) %>', '');
        }            
      

     </script>
        

</head>
<body >    

    <%--<div class="container">
      <form class="form-signin">
        <h2 class="form-signin-heading">Please sign in</h2>
        <label for="inputEmail" class="sr-only">Email address</label>

       

        <input type="email" id="inputEmail" class="form-control" placeholder="Email address" required autofocus>
        <label for="inputPassword" class="sr-only">Password</label>
        <input type="password" id="inputPassword" class="form-control" placeholder="Password" required>
        <div class="checkbox">
          <label>
            <input type="checkbox" value="remember-me"> Remember me
          </label>
        </div>
        <button class="btn btn-lg btn-primary btn-block" type="submit">Sign in</button>
      </form>
    </div> <!-- /container -->
    <script src="/content/js/ie10-viewport-bug-workaround.js"></script>--%>
    
    <form id="form1" runat="server">     
             
        <asp:HiddenField ID="hfLATVT" runat="server" />
        <asp:HiddenField ID="hfLONGVT" runat="server" />

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

                <%--<td width="80%">
                    <iframe src="http://docs.google.com/gview?url=http://powaco.com.vn/UpLoadFile/powaco/060217Mau De Nghi Cong Tac.An Phu (hoan pha)5.xls&embedded=true" style="width:600px; height:500px;" frameborder="0"></iframe>
                </td>--%>



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
                                        <asp:Button ID="btnOK" runat="server" OnClick="btnOK_Click" CssClass="btn_lg" TabIndex="3"  />
                                        <asp:Button ID="btnReset" TabIndex="4" runat="server" CssClass="btn_cc" OnClick="btnReset_Click"/>
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
