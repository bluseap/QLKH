<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="rpDSKDO.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.rpDSKDO" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web,  Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>


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