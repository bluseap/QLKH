<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="VatTu.aspx.cs" Inherits="EOSCRM.Web.Forms.DanhMuc.BaoCao.VatTu" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
<script type="text/javascript">
        function CheckFormReport() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
		}
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:Button ID="btnBaoCao" OnClientClick="return CheckFormReport();" runat="server" onclick="btnBaoCao_Click" CssClass="report" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <br />
    <div class="crmcontainer" id="divReport" runat="server">
        <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" />
    </div>  
</asp:Content>
