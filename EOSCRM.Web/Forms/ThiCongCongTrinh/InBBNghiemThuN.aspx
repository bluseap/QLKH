<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="InBBNghiemThuN.aspx.cs" Inherits="EOSCRM.Web.Forms.ThiCongCongTrinh.InBBNghiemThuN" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
    $(document).ready(function () {
        $("#divHopDong").dialog({
            autoOpen: false,
            modal: true,
            minHeight: 100,
            height: 'auto',
            width: 'auto',
            resizable: false,
            open: function (event, ui) {
                $(this).parent().appendTo("#divHopDongDlgContainer");
            }
        });
    });

    function CheckFormFitler() {
        openWaitingDialog();
        unblockWaitingDialog();

        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

        return false;
    }

    function CheckFormReport() {
        openWaitingDialog();
        unblockWaitingDialog();

        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
    }

    function CheckFormReportTK() {
        openWaitingDialog();
        unblockWaitingDialog();

        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCaoTKN) %>', '');
    }

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divHopDongDlgContainer">
        <div id="divHopDong" style="display: none">
            <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                        <td class="ptop-20">
                        <div class="crmcontainer">
                        <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" 
                            OnRowCommand="gvList_RowCommand" AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
                            OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                            <RowStyle CssClass="row" />
                            <AlternatingRowStyle CssClass="altrow" />
                            <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                            <Columns>                                     
                                <asp:TemplateField HeaderStyle-Width="120px" HeaderText="Mã đơn">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                            CommandName="EditItem" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <ItemStyle Font-Bold="true" />
                                </asp:TemplateField>
                                <asp:BoundField HeaderText="Tên khách hàng" HeaderStyle-Width="25%" DataField="TENKH" />
                                <asp:BoundField HeaderStyle-Width="40%" HeaderText="Địa chỉ lắp đặt" DataField="DIACHILD" />                                            
                            </Columns>
                        </eoscrm:Grid>
                        </div>                        
                        </td>
                        </tr>           
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
         </div>
      </div>
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>                       
                <asp:Panel ID="headerPanel" runat="server" CssClass="crmcontainer cleantop">
                    <table class="crmtable">
                        <tbody>
                            <tr>    
                                <td class="crmcell right width-125">Mã đơn đăng ký</td>
                                <td class="crmcell width-250">    
                                    <div class="left">
                                        <asp:TextBox ID="txtMADDK" runat="server" Width="120px" MaxLength="15" 
                                            TabIndex="1" />                            
                                    </div>
                                    <td class="crmcell right"></td>
                                    <td class="crmcell">    
                                        <div class="left">
                                            <asp:TextBox ID="txtTENKH" runat="server" Width="300px" MaxLength="200" TabIndex="3" Visible="false" />
                                        </div>                                    
                                    </td>
                                </td>
                            </tr>
                            <tr>
                                <td class="crmcell right width-125">Họ tên nhân viên</td>
                                <td class="crmcell width-250">    
                                    <div class="left">
                                        <asp:TextBox ID="txtHOTENNV1" runat="server" Width="300px" MaxLength="100"  ReadOnly="true"
                                            TabIndex="1" />                            
                                    </div>
                                    <td class="crmcell right">Ngày in</td>
                                    <td class="crmcell">    
                                        <div class="left">
                                            <asp:TextBox ID="txtNGAYIN" runat="server" Width="80px" MaxLength="20" TabIndex="3" Readonly="true"/>
                                        </div>                                    
                                    </td>
                                </td>                               
                            </tr>
                            <tr>    
                                <td class="crmcell right btop"></td>
                                <td class="crmcell btop" colspan="3">
                                    <div class="right">
                                        <asp:LinkButton ID="linkBBNTHUMAU"  runat="server" Visible="false"
                                            OnClientClick="return CheckFormReport();" OnClick="linkBBNTHUMAU_Click"  >
                                            In biên bản nghiệm thu theo mẫu.
                                        </asp:LinkButton>
                                    </div>
                                    <div class="left">
                                        <asp:Label ID="reloadm" runat="server" Visible="False" />
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="filter"
                                            OnClick="btnFilter_Click" Visible="false"
                                            ausesValidation="false" UseSubmitBehavior="false" 
                                            OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divHopDong')"  />
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btnBaoCao" runat="server" CssClass="report" OnClick="btnBaoCao_Click"                                                                             
                                           OnClientClick="return CheckFormReport();" UseSubmitBehavior="false" TabIndex="1" />
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btnBaoCaoTKN" runat="server" CssClass="myButton"                                                                     
                                           OnClientClick="return CheckFormReportTK();" UseSubmitBehavior="false" TabIndex="1" Text="Báo Cáo" OnClick="btnBaoCaoTKN_Click" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </asp:Panel>            
        </ContentTemplate>
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
