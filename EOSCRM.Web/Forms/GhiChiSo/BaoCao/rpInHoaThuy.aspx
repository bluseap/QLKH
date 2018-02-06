<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="rpInHoaThuy.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.rpInHoaThuy" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>            
                <tr>
                    <td class="crmcell" valign="top">
                        <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" 
                            HasToggleGroupTreeButton="False" PrintMode="ActiveX" 
                            DisplayGroupTree="False" />
                        <br />
                    </td>
                </tr>
            </tbody>
        </table>           
    </div>  
</asp:Content>
