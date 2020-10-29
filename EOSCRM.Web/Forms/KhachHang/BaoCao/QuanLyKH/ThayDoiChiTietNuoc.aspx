<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="ThayDoiChiTietNuoc.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.ThayDoiChiTietNuoc" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
    
        function CheckFormReport() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());
            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }            
            openWaitingDialog();
            unblockWaitingDialog();            
        }

        function CheckFormbtXuatExcel() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btXuatExcel) %>', '');   
        }        
        
        function CheckFormbtXuatExcelDP() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btXuatExcelDP) %>', '');   
        }

    </script>    
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
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
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>                                
                            </td>                                  
                        </tr>             
                        <tr>
                            <td class="crmcell right">Người lập</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNguoiLap" runat="server" Width="180px" />
                                </div>
                                <div class="left width-200">
                                    <asp:Button ID="btnBaoCao" runat="server" CssClass="report" OnClick="btnBaoCao_Click"
                                        OnClientClick="return CheckFormReport();" Height="22px" />
                                </div>                        
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Phiên (Long Xuyên)</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlPHIENLX" runat="server">
                                        <asp:ListItem Value ="%">Tất cả</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="A">A</asp:ListItem>
                                    </asp:DropDownList>
                                </div>                                
                            </td>                                  
                        </tr> 
                        <tr>
                            <td class="crmcell right">Danh sách TĐCT</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDSTDCT" runat="server">
                                        <asp:ListItem Text="Tất cả" Value="%" />
                                        <asp:ListItem Text="Số nhà, địa chỉ" Value="CTSONHADIACHI" />
                                        <asp:ListItem Text="Danh bộ" Value="CTDANHBO" />
                                        <asp:ListItem Text="Mã số thuế" Value="CTMASOTHUE" />
                                        <asp:ListItem Text="Số hộ" Value="CTSOHO" />
                                        <asp:ListItem Text="Mã giá" Value="CTMAGIA" />
                                        <asp:ListItem Text="Tên khách hàng" Value="CTTENKH" />
                                        <asp:ListItem Text="Đổi số No KH cũ" Value="CTDOISONO" />
                                    </asp:DropDownList>
                                </div>                                
                            </td>                                  
                        </tr>  
                        <tr>
                            <td class="crmcell right"> </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btXuatExcel" runat="server" CssClass="myButton" OnClick="btXuatExcel_Click" UseSubmitBehavior="false"
                                        OnClientClick="return CheckFormbtXuatExcel();" Text ="Xuất Excel" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btXuatExcelDP" runat="server" CssClass="myButton" OnClick="btXuatExcelDP_Click" UseSubmitBehavior="false"
                                        OnClientClick="return CheckFormbtXuatExcelDP();" Text ="Xuất Excel đường phố (Long Xuyên)"/>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btXuatExcel" />
            <asp:PostBackTrigger ControlID="btXuatExcelDP" />
        </Triggers>     
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                 <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" />       
            </div>            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>            
    </asp:UpdatePanel> 
</asp:Content>
