<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="MoiMoCupSoVoiThangTruoc.aspx.cs" Inherits="EOSCRM.Web.Forms.CongNo.BaoCao.MoiMoCupSoVoiThangTruoc" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        function CheckFormCup() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            
            }
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDSKHCup) %>', '');
        }

        function CheckFormMo() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;

            }
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDSKHMo) %>', '');
        }

        function CheckFormMoi() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;

            }
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDSKHMoi) %>', '');
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
                        <div class="left">
                            <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="150px" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnDSKHMoi" runat="server"  
                                OnClientClick="return CheckFormMoi();"  Text = "Danh sách khách hàng mới" 
                                onclick="btnDSKHMoi_Click" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnDSKHMo" runat="server"  
                                OnClientClick="return CheckFormMo();" Text = "Danh sách khách hàng mở nước" 
                                onclick="btnDSKHMo_Click"/>
                        </div>
                        
                        <div class="left">
                            <asp:Button ID="btnDSKHCup" runat="server"  
                                OnClientClick="return CheckFormCup();"  
                                Text = "Danh sách khách hàng cúp nước" onclick="btnDSKHCup_Click" />
                        </div>
                    </td>                   
                </tr>
            </tbody>
        </table>
    </div>
    <br />
    <div class="crmcontainer" id="divReport" runat="server" visible="false">
        <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" DisplayGroupTree="False" />
    </div>
</asp:Content>
