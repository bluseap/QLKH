<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="MHome.aspx.cs" Inherits="EOSCRM.Web.WebMobi.MHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.css"/>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css" />
    <link rel="shortcut icon" href="content/images/powaco.ico" />
    <script src="http://code.jquery.com/jquery-1.11.3.min.js"></script>
    <script src="http://code.jquery.com/ui/1.11.4/jquery-ui.js"></script>
    <script src="http://code.jquery.com/mobile/1.4.5/jquery.mobile-1.4.5.min.js"></script>

    
</head>
<body>
    <form id="form1" runat="server">
    <div data-role="page" id="pageone">
        <div data-role="header">
            <div style="text-align:center">
                <img src="/content/images/powacmo.png" />
            </div>
        </div>
        <div data-role="main" class="ui-content">
                <div data-role="navbar">
                  <ul>
                    <li><a href="/WebMobi/KhachHang/MNhapDLM.aspx">Nhập đơn lắp mới</a></li>
                    <li><a href="/WebMobi/KhachHang/MNhapDonLM.aspx">Duyệt đơn lắp mới</a></li>
                    <li><a href="/WebMobi/ThietKe/MNhapDLM.aspx">Khảo sát - Thiết kế</a></li>
                    <li><a href="/WebMobi/ThietKe/MNhapDLM.aspx">Duyệt thiết kế</a></li>
                  </ul>
                </div>
        </div>
        <div>
                  <asp:Label ID="lbTENNV" runat="server" Text="tennv" Visible="false"></asp:Label>
        </div>
        <div data-role="footer">
            <h1>POWACO</h1>
        </div>
    </div> 
    </form>
</body>
</html>
