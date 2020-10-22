<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="DS_ChoThietKe.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi.DS_ChoThietKe" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        function CheckFormReport() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }
        
        function CheckFormBCTUCHOITK() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btBCTUCHOITK) %>', '');
        }

        function CheckFormbtDSChoTK() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btDSChoTK) %>', '');
        }
        
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Từ ngày</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:TextBox ID="txtTuNgay" runat="server" Width="75px" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtTuNgay"
                            PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        <div class="left">
                            <div class="right">Đến ngày</div>
                        </div> 
                        <div class="left">
                            <asp:TextBox ID="txtDenNgay" runat="server" Width="75px" Height="16px" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDenNgay"
                            PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />   
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">    
                        <asp:Label ID="Label1" runat="server" Text="Lấy ngày nhập trên máy" Font-Bold="True" ForeColor="Red"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Khu vực</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server" />
                        </div>
                        <div class="left">
                            <div class="right">Người lập</div>
                        </div> 
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" />
                        </div> 
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Nhà máy, tổ
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="ddlNHAMAYTO" runat="server" />                                    
                        </div>                        
                    </td>                            
                </tr>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:Button ID="btnBaoCao" OnClientClick="return CheckFormReport();" runat="server" onclick="btnBaoCao_Click" CssClass="myButton" Text="DS chưa duyệt TK" />
                        </div>
                        <div class="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btBCTUCHOITK" OnClientClick="return CheckFormBCTUCHOITK();" runat="server" CssClass="myButton" Text="BC Từ chối TK" OnClick="btBCTUCHOITK_Click" />
                        </div>
                        <div class="left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Button ID="btDSChoTK" OnClientClick="return CheckFormbtDSChoTK();" runat="server" CssClass="myButton" Text="DS Chờ TK" OnClick="btDSChoTK_Click"  />
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
