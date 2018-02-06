<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="rpLapChietTinhSuaChua.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi.rpLapChietTinhSuaChua" %>

<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div class="crmcontainer">
    <table class="crmtable">
        <tbody>
            <tr>
                <td class="crmcell right">Người lập</td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtNguoiLap" runat="server" Width="200px" />
                    </div>
                </td>
                <td class="crmcell right">Phòng KHKT</td>
                <td class="crmcell" style="width: 204px">
                    <div class="left">
                        <asp:TextBox ID="txtKHKT" runat="server" Width="200px" Text = "" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">Giám đốc</td>                
                <td class="crmcell" colspan="3">
                    <div class="left">
                        <asp:TextBox ID="txtGiamDoc" runat="server" Width="200px" Text = "" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right"></td>   
                <td class="crmcell">
                    <div class="left">
                        <asp:Button ID="btnBaoCao" runat="server" onclick="btnBaoCao_Click" CssClass="report" />
                    </div>
                    <div class="left">
                        <asp:Button ID="btnBack" runat="server" onclick="btnBack_Click" CssClass="back" Text="Trở về" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell" colspan="6" style="height: 502px" valign="top">
                    <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" DisplayGroupTree="False" />
                    <br />
                </td>
            </tr>
        </tbody>
    </table>           
    </div>  
</asp:Content>
