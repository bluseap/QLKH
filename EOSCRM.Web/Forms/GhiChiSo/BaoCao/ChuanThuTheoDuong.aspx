<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="ChuanThuTheoDuong.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.ChuanThuTheoDuong" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
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

        function CheckFormReportTTHAI() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());
            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lnTTHAIGHITHEODUONG) %>', '');
        }

        function CheckFormbtChuanThuTheoKV() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());
            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btChuanThuTheoKV) %>', '');
        }
        
    </script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">
                        Tháng
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="cboTHANG" runat="server" TabIndex="1">
                                <asp:ListItem Text="01" Value="01" />
                                <asp:ListItem Text="02" Value="02" />
                                <asp:ListItem Text="03" Value="03" />
                                <asp:ListItem Text="04" Value="04" />
                                <asp:ListItem Text="05" Value="05" />
                                <asp:ListItem Text="06" Value="06" />
                                <asp:ListItem Text="07" Value="07" />
                                <asp:ListItem Text="08" Value="08" />
                                <asp:ListItem Text="09" Value="09" />
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="11" Value="11" />
                                <asp:ListItem Text="12" Value="12" />
                            </asp:DropDownList>
                        </div>
                        <div class="left width-200">
                            <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left">
                            <strong>Khu vực</strong>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" Height="24px" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Loại khách hàng
                    </td>
                    <td class="crmcell">
                        <div class="left width-175">
                            <asp:DropDownList ID="ddlMDSD" runat="server" Height="24px" Width="150px" AutoPostBack="true" onchange="openWaitingDialog()" 
                                    OnSelectedIndexChanged="ddlMDSD_SelectedIndexChanged">
                            </asp:DropDownList>
                        </div>
                        <div class="left">
                            <div class="right">Loại cơ quan</div>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="ddlLKHDB" runat="server" Width="200px" TabIndex="25">
                            </asp:DropDownList>  
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Người lập</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="180px" />
                        </div>                        
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnBaoCao" runat="server" CssClass="myButton" OnClick="btnBaoCao_Click" Text="Chuẩn thu theo đường"
                                OnClientClick="return CheckFormReport();" />
                        </div>
                        <div class="left ">
                            <asp:Button ID="btChuanThuTheoKV" runat="server" CssClass="myButton" Text="Chuẩn thu theo khu vực"
                                OnClientClick="return CheckFormbtChuanThuTheoKV();" OnClick="btChuanThuTheoKV_Click" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right"> </td>
                    <td class="crmcell left">
                        <div class="right">
                            <asp:LinkButton ID="lnTTHAIGHITHEODUONG"  runat="server" onclick="lnTTHAIGHITHEODUONG_Click" 
                                OnClientClick="return CheckFormReportTTHAI();" >
                                Thống kê trạng thái ghi chỉ số theo đường.
                            </asp:LinkButton>
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
