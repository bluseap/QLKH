<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="DSKHMoiPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo.DSKHMoiPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        
        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

        function CheckFormExcel() {
            //openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkEXCEL) %>', '');
        }

        function CheckFormWord() {
            //openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkWORD) %>', '');
        }

        function CheckFormMDK() {
            //openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkKHMMucDK) %>', '');
        }

        function CheckFormExcelTS() {
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkXuatExcelTS) %>', '');
        }
        
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">

    <asp:UpdatePanel ID="upnlBaoCao" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                             <td class="crmcell right">Tháng</td>
                             <td class="crmcell">
                                <div class="left width-150">
                                    <asp:DropDownList ID="cboTHANG" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                    <asp:Label ID="lbRELOAD" runat="server" Visible="false"></asp:Label>
                                </div>
                             </td>
                        </tr>
                        <tr>
                           
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="cboKhuVuc" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </div>
                                <div class="left width-100">
                                    <asp:TextBox ID="txtMaDp" runat="server" Width="25px" Visible="false"/>
                                    <asp:TextBox ID="txtDuongPhu" runat="server" Width="25px" Visible="false" />
                                    
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>                                
                            </td>                                  
                        </tr> 
                        <tr>
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="cboMucDichSuDung" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </div>                                
                                <div class="left">
                                    <div class="right">Người lập</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNguoiLap" runat="server"  Width="225px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" runat="server"  CssClass="report" 
                                        OnClick="btnBaoCao_Click" OnClientClick="return CheckFormReport();" />
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkEXCEL" runat="server" OnClick="lkEXCEL_Click" UseSubmitBehavior="false" CssClass="myButton"  
                                            OnClientClick="return CheckFormExcel();" Visible="true">Xuất Excel</asp:LinkButton>
                                    </div>                                    
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkXuatExcelTS" runat="server" UseSubmitBehavior="false" CssClass="myButton"  
                                            OnClientClick="return CheckFormExcelTS();" OnClick="lkXuatExcelTS_Click" >Xuất Excel(TS)</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkWORD" runat="server" OnClick="lkWORD_Click" UseSubmitBehavior="false" CssClass="myButton"  
                                            OnClientClick="return CheckFormWord();" Visible="false">Xuất Word</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="left">
                                    <div class="right">
                                            <asp:LinkButton ID="lkKHMMucDK" runat="server" OnClick="lkKHMMucDK_Click" UseSubmitBehavior="false" CssClass="myButton"  
                                                OnClientClick="return CheckFormMDK();" Visible="false">DS Mục đích khác</asp:LinkButton>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lkEXCEL" />
            <asp:PostBackTrigger ControlID="lkXuatExcelTS" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                    DisplayGroupTree="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
