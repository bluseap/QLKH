<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="TongHopChuanThu.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.TongHopChuanThu" %>

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
        
        function CheckFormbtnKyChart() {     
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnKyChart) %>', '');
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Tháng</td>
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
                            <asp:TextBox ID="txtNAM" runat="server" Width="40px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left width-150">
                            <div class="right">Khu vực</div>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" Height="24px" Width="200px">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>  
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTuKy" runat="server" Text="Từ kỳ"></asp:Label></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="ddlTuKy" runat="server" TabIndex="1">
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
                            <asp:TextBox ID="txtTuNam" runat="server" Width="40px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left width-150">
                            <div class="right"> <asp:Label ID="lbDenKy" runat="server" Text="đến kỳ"></asp:Label></div>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="ddlDenKy" runat="server" TabIndex="1">
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
                            <asp:TextBox ID="txtDenNam" runat="server" Width="40px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnKyChart" runat="server" CssClass="myButton" Text="Biểu đồ cột" OnClick="btnKyChart_Click" 
                                OnClientClick="return CheckFormbtnKyChart();" />
                        </div>
                    </td>
                </tr>                
                <tr>  
                    <td class="crmcell right">Người lập</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="150px" />
                        </div>
                    </td>
                </tr>               
                <tr>                    
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnBaoCao" runat="server" OnClick="btnBaoCao_Click" CssClass="report" OnClientClick="return CheckFormReport();" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <br />
    <div class="crmcontainer" id="divCR" runat="server" visible="false">
        <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" DisplayGroupTree="False" />
    </div>
</asp:Content>
