<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="InSoGhiNuoc.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.InSoGhiNuoc" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    
    <script type="text/javascript">
        
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">    
    <div class="crmcontainer">
        <table cellspacing="0" class="rgMasterTable rgClipCells form" style="width: 100%;
            table-layout: fixed; empty-cells: show;">
            <tbody>
                <tr>
                    <td class="crmcell right">
                                Kỳ khai thác
                    </td>
                    <td>
                    <div class="left">
                                    <asp:DropDownList ID="ddlTHANG" runat="server" TabIndex="1">
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
                    <div class="left width-150 pleft-50">
                          <div class="right">Khu vực</div>
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
                            <asp:TextBox ID="txtNguoiLap" runat="server" Width="200px" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnBaoCao" runat="server" CssClass="report" 
                                onclick="btnBaoCao_Click" OnClientClick="return CheckFormReport();" />
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
    
</asp:Content>
