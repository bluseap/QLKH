<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="LichSuSuDungNuoc.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.LichSuSuDungNuoc" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        function CheckFormReport() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;

            }
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
                    <td class="crmcell right">Năm</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="1" />
                        </div>
                        <div class="left"><strong>Mã đường</strong>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtMaDp" runat="server" Width="30px" />
                        </div>
                        <div class="left"><strong>Khu vực</strong></div>
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" Height="24px" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                    </td>
                    <td class="crmcell">
                        <div class="left ">
                            <asp:Button ID="btnBaoCao" runat="server" CssClass="report" OnClick="btnBaoCao_Click" ToolTip="Sổ bộ 6 tháng đầu năm"
                                OnClientClick="return CheckFormReport();" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <br />
    <div class="crmcontainer" id="divReport" runat="server" visible="false">
        <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX"
            DisplayGroupTree="False" />
    </div>
</asp:Content>
