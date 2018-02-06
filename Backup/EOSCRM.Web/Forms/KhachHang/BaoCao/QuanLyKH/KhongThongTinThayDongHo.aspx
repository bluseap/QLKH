<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="KhongThongTinThayDongHo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.KhongThongTinThayDongHo" %>

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
                    <td class="crmcell right">
                        Đường
                    </td>
                    <td class="crmcell">
                        
                        <div>
                            <asp:TextBox ID="txtMADP" runat="server" />
                        </div>
                        <div>
                            <strong>Khu vực </strong>
                        </div>
                        <div>
                            <asp:DropDownList ID="cboKhuVuc" runat="server">
                            </asp:DropDownList>
                        </div>
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
                    <td class="crmcell" colspan="2">
                        <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" />
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
