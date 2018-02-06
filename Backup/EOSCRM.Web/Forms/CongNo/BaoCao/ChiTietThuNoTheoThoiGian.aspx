<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="ChiTietThuNoTheoThoiGian.aspx.cs" Inherits="EOSCRM.Web.Forms.CongNo.BaoCao.ChiTietThuNoTheoThoiGian" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

        function CheckFormReport2() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao2) %>', '');
        }



        function CheckFormReport3() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao3) %>', '');
        }         
    </script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Nhân viên thu</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="cboNhanVienThu" runat="server" Height="24px" Width="200px">
                            </asp:DropDownList>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" Height="24px" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Từ ngày</td>
                    <td class="crmcell">
                        <div class="left">
                             <asp:TextBox ID="txtTuNgay" runat="server" Width="100px" MaxLength="20" TabIndex="2" />
                        </div>
                        <div class="left">
                            <strong>Đến ngày</strong>
                        </div>
                        <div class ="left">
                             <asp:TextBox ID="txtDenNgay" runat="server" Width="100px" MaxLength="20" TabIndex="2" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="180px" />
                        </div>
                        <div class="left ">
                            <asp:Button ID="btnBaoCao" runat="server" CssClass="report" OnClick="btnBaoCao_Click"
                                OnClientClick="return CheckFormReport();" ToolTip="Nhóm theo khách hàng" />
                        </div>
                        <div class="left ">
                            <asp:Button ID="btnBaoCao2" runat="server" CssClass="report" OnClick="btnBaoCao2_Click"
                                OnClientClick="return CheckFormReport2();" ToolTip="Nhóm theo ngày thu" />
                        </div>                        
                        <div class="left ">
                            <asp:Button ID="btnBaoCao3" runat="server" CssClass="report" OnClick="btnBaoCao3_Click"
                                OnClientClick="return CheckFormReport3();" ToolTip="Nhóm theo tháng thu" />
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
