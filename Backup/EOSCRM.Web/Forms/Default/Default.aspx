<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="true"
    Inherits="EOSCRM.Web.Forms.Default.Default" CodeBehind="Default.aspx.cs" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">  
    <link type="text/css" href="<%= ResolveUrl("~")%>content/css/grid.css" rel="stylesheet" />
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="Server">
    
    <asp:Image ID="Image1" runat="server" 
        ImageUrl="~/content/images/login/tgnuoc.jpg" Height=480 Width=1008 />
    
</asp:Content>
