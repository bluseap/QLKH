<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="DSDongHoCSLon.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.DSDongHoCSLon" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div class="crmcontainer">
        <table cellspacing="0" class="rgMasterTable rgClipCells form" style="width: 100%;
            table-layout: fixed; empty-cells: show;">
            <tbody>
                <tr>    
                    <td class="crmcell right">Khu vực</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:DropDownList ID="cboKhuVuc" runat="server">
                            </asp:DropDownList>
                        </div>
                        <td class="crmcell right"></td>
                        <td class="crmcell" >    
                            <div class="left">                               
                            </div>                                                
                        </td>                            
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Người lập
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNguoiLap" runat="server" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnBaoCao" runat="server" OnClick="btnBaoCao_Click" CssClass="report" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">                        
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtTuNgay" runat="server" Width="15px" Visible="False" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" Visible="False" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtTuNgay"
                            PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        <div class="left">
                            <strong> </strong>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtDenNgay" runat="server" Width="15px" Height="16px" Visible="False" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" Visible="False" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDenNgay"
                            PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        <div>
                            <strong> </strong>
                        </div>
                        <div>
                            <asp:TextBox ID="txtMADP" runat="server" Visible="False" width="15"/>
                        </div>                        
                    </td>
                </tr>
                
                <tr>
                    <td class="crmcell" colspan="2">
                        <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>  
</asp:Content>
