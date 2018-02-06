<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="InThietKePo.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.Power.BaoCaoPo.InThietKePo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web,  Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
     namespace="CrystalDecisions.Web" tagprefix="CR" %>


<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div class="crmcontainer">
    <table class="crmtable">
        <tbody>
            <tr>
                <td class="crmcell right">Tên Nhân viên in</td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtNguoiLap" runat="server" Width="200px" OnTextChanged="txtNguoiLap_TextChanged" ReadOnly="true" />
                    </div>
                </td>
                <td class="crmcell right">Ngày in</td>
                <td class="crmcell" style="width: 204px">
                    <div class="left">
                        <asp:TextBox ID="txtNGAYIN" runat="server" Width="200px" Text = "" ReadOnly="true"/>
                    </div>
                </td>
                <td class="crmcell" style="width: 204px">
                    <div class="left">
                        <asp:TextBox ID="txtKHKT" runat="server" Width="200px" Text = "" Visible="false"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right"></td>                
                <td class="crmcell" colspan="3">
                    <div class="left">
                        <asp:TextBox ID="txtGiamDoc" runat="server" Width="200px" Text = "" Visible="false"/>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right"></td>   
                <td class="crmcell">
                    <div class="right">
                                        <asp:LinkButton ID="linkTKMAU"  runat="server" Visible="false"
                                            OnClientClick="return CheckFormReport();" OnClick="linkTKMAU_Click"  >
                                            In thiết kế theo mẫu.
                                        </asp:LinkButton>
                    </div>
                    <div class="right">
                                        <asp:LinkButton ID="linkTKMAU_LOI"  runat="server" Visible="false"
                                            OnClientClick="return CheckFormReport();" OnClick="linkTKMAU_LOI_Click"  >
                                            In thiết kế theo mẫu của a.LOI..
                                        </asp:LinkButton>
                    </div>
                    <div class="left">
                                        <asp:Label ID="reloadm" runat="server" Visible="False" />
                    </div>
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
