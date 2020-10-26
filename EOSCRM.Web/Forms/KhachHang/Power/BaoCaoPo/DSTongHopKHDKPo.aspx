<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="DSTongHopKHDKPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo.DSTongHopKHDKPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlTongHopDDK" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="cboKhuVuc" runat="server" />
                                </div>
                                <td class="crmcell">    
                                    <div class="left">
                                        <div class="right">Người lập</div>
                                    </div> 
                                    <div class="left">
                                        <asp:TextBox ID="txtNguoiLap" runat="server" />
                                    </div> 
                                 </td>
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
                            <td class="crmcell right">Từ ngày</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTuNgay" runat="server"  />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtTuNgay"
                                    PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <td class="crmcell">    
                                    <div class="left">
                                        <div class="right">Đến ngày</div>
                                    </div> 
                                    <div class="left">
                                        <asp:TextBox ID="txtDenNgay" runat="server"  Height="16px" />
                                    </div>
                                    <div class="left">
                                        <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                            AlternateText="Click to show calendar" />
                                    </div>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDenNgay"
                                        PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />   
                                 </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" OnClientClick="return CheckFormReport();" runat="server" onclick="btnBaoCao_Click" CssClass="report" />
                                </div>                                                                                                                     
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">                            
                                 <div class="left">
                                    <asp:Button ID="btnExcelDSNgayNhap" OnClientClick="return CheckFormbtnExcelDSNgayNhap;" runat="server" CssClass="myButton" 
                                            OnClick="btnExcelDSNgayNhap_Click" Text="DS TH Khách hàng (theo ngày nhập)"/>
                                 </div>
                                <td class="crmcell">
                                     <div class="left">
                                        <asp:Button ID="btDSTongHopKH" OnClientClick="return CheckFormbtDSTongHopKH();" runat="server" CssClass="myButton" 
                                                Text="DS TH Khách hàng (theo máy)" OnClick="btDSTongHopKH_Click1"/>
                                     </div>  
                                 </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Điểu kiện</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlDieuKien" runat="server"></asp:DropDownList>
                                </div>
                                <td class="crmcell">    
                                    <div class="left">
                                        <asp:Button ID="btDSDieuKien" OnClientClick="return CheckFormbtDSDieuKien();" runat="server" CssClass="myButton" 
                                            OnClick="btDSDieuKien_Click" Text="DS KH theo điều kiện"/>
                                    </div>                                                                                                           
                                </td>                                                                                   
                            </td>
                        </tr>
                    </tbody>
                </table>        
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnBaoCao" />
            <asp:PostBackTrigger ControlID="btDSTongHopKH" />
            <asp:PostBackTrigger ControlID="btDSDieuKien" />
            <asp:PostBackTrigger ControlID="btnExcelDSNgayNhap" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <div class="crmcontainer" id="divReport" runat="server">
        <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" />
    </div>
    
</asp:Content>
