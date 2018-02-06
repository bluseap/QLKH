<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="TSLamBai.aspx.cs" Inherits="EOSCRM.Web.TracNghiem.TSLamBai" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title></title>
    <style type="text/css">
        .style1
        {
            font-size: xx-large;
            font-weight: bold;
        }
        .style2
        {
            color: #0066FF;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center; height: 129px">
        <asp:Image ID="logoct" runat="server" align="left" ImageUrl="~/Logo/logoct.png"/>
        <span class="style1">
        <br />
        <span class="style2">CÔNG TY CỔ PHẦN ĐIỆN NƯỚC AN GIANG</span><br />
        </span>
        <br />    <br />      <br />      <br />     <br />       <br /> <br />
    </div>
    <div id="tieude" align="center" font="Times New Roman" style="font-size: x-large" >
    </div> <br />
    <div id="Div1" align="left" font="Times New Roman" style="font-size: large" >
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        <asp:Label ID="Label2" runat="server" 
            
            Text="-	Phần mềm tra cứu thông tin khách hàng nước trên di động, dùng cho hệ điều hành Android." 
            style="text-align: center">
        </asp:Label> <br /> <br />
        
    </div> <br />    
    <div id="Div2" align="left" font="Times New Roman" style="font-size: large" >
        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;        
        <asp:LinkButton ID="lkANDROID" runat="server" style="text-align: center">http://www.mediafire.com/download/nnanwu144ygok8j/01POWACO_ANDROID.apk</asp:LinkButton>
    </div> <br />
    </form>
</body>
</html>
