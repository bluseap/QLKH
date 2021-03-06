<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="rpDSKTKY.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.rpDSKTKY" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

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
                    <td class="crmcell right">
                        Người lập
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="200px" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnBaoCao" runat="server" CssClass="report" 
                                onclick="btnBaoCao_Click" OnClientClick="return CheckFormReport();" />
                        </div>                        
                    </td>
                </tr>
            </tbody>
        </table>           
    </div>  
    <br />
    <div class="crmcontainer">
        <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" 
            HasToggleGroupTreeButton="False" PrintMode="ActiveX" DisplayGroupTree="False" />           
    </div>
</asp:Content>
