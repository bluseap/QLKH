<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/WPO.Master" AutoEventWireup="true" CodeBehind="AboutPo.aspx.cs" Inherits="EOSCRM.Web.NhanSu.AboutPo" %>


<asp:Content ID="HeaderContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="https://maxcdn.bootstrapcdn.com/bootstrap/3.3.4/css/bootstrap.min.css" />
    <style type="text/css">
        .container {
            width: 100%;
        }
        .container {
            padding-right: 2px;
            padding-left: 2px;
            margin-right: auto;
            margin-left: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>
        Responsive Navigation Menu using Twitter Bootstrap in Asp.net
    </h3>
    <asp:Label ID="lblMenu" runat="server"></asp:Label>
    <script type="text/javascript" src="js/jquery.min.js"></script>
    <script type="text/javascript" src="js/bootstrap.min.js"></script>
</asp:Content>



