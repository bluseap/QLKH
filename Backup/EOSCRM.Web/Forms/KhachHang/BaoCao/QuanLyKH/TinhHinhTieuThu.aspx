﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="TinhHinhTieuThu.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.TinhHinhTieuThu" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    </asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div class="crmcontainer">
    <table cellspacing="0" class="rgMasterTable rgClipCells form" style="width: 100%; table-layout: fixed;
        empty-cells: show;">
        <tbody>
            <tr>
                
                <td class="crmcell right">
                    Người lập
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtNguoiLap" runat="server" Height="16px" />
                    </div>
                    <div class="left">
                        <asp:Button ID="btnBaoCao" runat="server" onclick="btnBaoCao_Click" 
                    Text="Báo Cáo" Width="60px" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell" colspan="6">
                    <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" />
                </td>
            </tr>
        </tbody>
    </table>           
    </div>  
</asp:Content>
