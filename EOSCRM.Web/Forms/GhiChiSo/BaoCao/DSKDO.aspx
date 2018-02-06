<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="DSKDO.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.DSKDO" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="powacocrm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">        

        
        function CheckFormExcel() {
            //openWaitingDialog();
            //unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btEXCEL) %>', '');
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
   <asp:UpdatePanel ID="UpINFO" UpdateMode="Conditional" runat="server">
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
                                <td class="crmcell right">Khu vực</td>
                                <td class="crmcell">                                
                                    <div class="left">
                                        <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" Width="150px" runat="server"
                                            TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </td>                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>
                                <td class="crmcell right">Đường phố</td>
                                <td class="crmcell">                                
                                    <div class="left">
                                        <asp:DropDownList ID="ddlDuongPhoPo" runat="server" TabIndex="3">
                                        </asp:DropDownList>
                                    </div>
                                </td>                                    
                            </td>                                  
                        </tr>                 
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" OnClientClick="return CheckFormReport();" runat="server" onclick="btnBaoCao_Click" CssClass="report" />
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Button ID="btEXCEL" Text="Xuất Excel" OnClientClick="return CheckFormExcel();" runat="server" CssClass="myButton" OnClick="btEXCEL_Click" />
                                    </div>
                                </td>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
       <Triggers>
            <asp:PostBackTrigger ControlID="btEXCEL" />
        </Triggers>
       <Triggers>
            <asp:PostBackTrigger ControlID="btnBaoCao" />
        </Triggers>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upBAOCAO" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" >
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                    DisplayGroupTree="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>
    
</asp:Content>
