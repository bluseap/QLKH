﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.BaoCao.DuongPho" CodeBehind="DuongPho.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
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
                    <td class="crmcell right">Khu vực</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" />
                        </div>
                    </td>
                </tr>
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
        <CR:CrystalReportViewer ID="rpViewer" runat="server" DisplayGroupTree="False" PrintMode="ActiveX" AutoDataBind="true" />
    </div>  
</asp:Content>