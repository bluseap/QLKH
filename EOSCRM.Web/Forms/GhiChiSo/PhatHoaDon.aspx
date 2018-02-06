<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="PhatHoaDon.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.PhatHoaDon" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
    
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnReport) %>', '');
        }

        function CheckChangeKV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(ddlKHUVUC) %>', '');
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlTinhCuoc" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">
                                Kỳ khai thác
                            </td>
                            <td class="crmcell">    
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
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" OnSelectedIndexChanged="ddlKHUVUC_SelectedIndexChanged" 
                                        AutoPostBack="true" Width="150px" onchange="return CheckChangeKV();" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:DropDownList ID="ddlNHANVIEN" AutoPostBack="true" Width="150px" runat="server" TabIndex="4">
                                    </asp:DropDownList>
                                    </div>
                                </div>
                                 <div class="left">
                                    <div class="right">Người lập</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNguoiLap" runat="server" Width="150px" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnReport" OnClick="btnReport_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormReport();" 
                                        runat="server" CssClass="report" Text="" TabIndex="12" />
                                </div> 
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                 <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" DisplayGroupTree="False" />       
            </div>    
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>
