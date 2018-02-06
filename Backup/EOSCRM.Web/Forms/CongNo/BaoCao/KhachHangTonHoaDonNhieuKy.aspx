<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="KhachHangTonHoaDonNhieuKy.aspx.cs" Inherits="EOSCRM.Web.Forms.CongNo.BaoCao.KhachHangTonHoaDonNhieuKy" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register TagPrefix="ajaxToolkit" Namespace="AjaxControlToolkit" Assembly="AjaxControlToolkit, Version=3.5.40412.0, Culture=neutral, PublicKeyToken=28f01b0e84b6d53e" %>
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
                    <td class="crmcell right">Nhân viên thu</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="cboNhanVienThu" runat="server" Height="24px" Width="180px" TabIndex="1" />
                            
                        </div>
                        <div class="left"><div class="right">Mã đường</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtMaDp" runat="server" Width="40px" TabIndex="2" />
                        </div>
                        <div class="left"><div class="right">Khu vực</div></div>
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" Height="24px" Width="200px" TabIndex="3" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Từ kỳ</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtTUKY" runat="server" Width="50px" TabIndex="4" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgTUKY" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" TabIndex="5" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTUKY"
                            PopupButtonID="imgTUKY" TodaysDateFormat="dd/MM/yyyy" Format="MM/yyyy" />
                        <div class="left">
                            <strong>Đến kỳ</strong>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtDENKY" runat="server" Width="50px" TabIndex="6" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgDENKY" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" TabIndex="7" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDENKY"
                            PopupButtonID="imgDENKY" TodaysDateFormat="dd/MM/yyyy" Format="MM/yyyy" />
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="180px" TabIndex="8" />
                        </div>
                        <div class="left ">
                            <asp:Button ID="btnBaoCao" runat="server" CssClass="report" OnClick="btnBaoCao_Click"
                                OnClientClick="return CheckFormReport();" TabIndex="9" />
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
