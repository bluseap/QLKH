<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="InGiayDeNghiPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo.InGiayDeNghiPo" %>

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

        $("#divDonDangKy").dialog({
            autoOpen: false,
            modal: true,
            minHeight: 20,
            height: 'auto',
            width: 'auto',
            resizable: false,
            open: function (event, ui) {
                $(this).parent().appendTo("#divDonDangKyDlgContainer");
            }
        });
    });

    function CheckFormFilterDP() {
        openWaitingDialog();
        unblockWaitingDialog();

        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDDK) %>', '');

            return false;
    }

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

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDonDangKyDlgContainer">
        <div id="divDonDangKy" style="display: none">
            <asp:UpdatePanel ID="upnlDonDangKy" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 700px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtFilter" onchange="return CheckFormFilterDP();" runat="server" Width="150px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <div class="right">Khu vực</div>                                                
                                                </div>
                                                <div class="left">
                                                    <div class="right">
                                                        <asp:DropDownList ID="ddlMaKV" runat="server" Width="160px" />
                                                    </div>                                                
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Từ ngày</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtTuNgay" runat="server" />
                                                </div>
                                                <div class="left">
                                                    <div class="right">Đến ngày</div>                                                
                                                </div>
                                                <div class="left">
                                                    <asp:TextBox ID="txtDenNgay" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"></td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDDK" OnClick="btnFilterDDK_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDP();" 
                                                        runat="server" CssClass="filter" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvDDK" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDDK_RowDataBound" OnRowCommand="gvDDK_RowCommand" 
							            OnPageIndexChanging="gvDDK_PageIndexChanging">
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="13%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDKPO") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" HeaderText="Tên khách hàng" />
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="NGAYSINH" HeaderText="Năm sinh" />
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="DIACHILD" HeaderText="Địa chỉ lắp đặt" />
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
                                        <asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                            CommandName="EditItem" Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
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
                    <div class="crmcontainer">
                    <table class="crmtable">
                        <tbody>
                            <tr>    
                                <td class="crmcell right width-125">Mã đơn đăng ký</td>
                                <td class="crmcell width-250">    
                                    <div class="left">
                                        <asp:TextBox ID="txtMADDK" runat="server" Width="120px" MaxLength="15" 
                                            TabIndex="1" />                            
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" OnClick="btnBrowseDDK_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" />
                                    </div>                                    
                                    <td class="crmcell">    
                                        <div class="left">
                                            <asp:TextBox ID="txtTENKH" runat="server" Width="300px" MaxLength="200" TabIndex="3" OnTextChanged="txtTENKH_TextChanged" Visible="false"/>
                                        </div>                                    
                                    </td>
                                </td>
                            </tr>
                            <tr>
                                <td class="crmcell right">Họ tên nhân viên</td>
                                <td class="crmcell width-200">                                    
                                    <div class="left">
                                            <asp:TextBox ID="txtTENNV2" runat="server" Width="190px" MaxLength="15" 
                                                TabIndex="1" Readonly="True"/>                            
                                    </div
                                </td>
                                <td class="crmcell right">Ngày in</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtNGAYIN" runat="server" Width="200px" Text = "" ReadOnly="True"/>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td class="crmcell right"> </td>
                                <td class="crmcell left">
                                    <div class="right">
                                        <asp:LinkButton ID="linkGDN"  runat="server" Visible="false"
                                            OnClientClick="return CheckFormReport();" OnClick="linkGDN_Click" >
                                            In giấy đề nghị theo mẫu.
                                        </asp:LinkButton>
                                    </div>
                                    <div class="left">
                                        <asp:Label ID="reloadm" runat="server" Visible="False" />
                                    </div>
                                </td>
                            </tr>
                            <tr>    
                                <td class="crmcell right btop"></td>
                                <td class="crmcell btop" colspan="3">
                                    <div class="left">
                                        <asp:Button ID="btnFilter" runat="server" CssClass="filter" Visible="false"
                                            OnClick="btnFilter_Click" 
                                            ausesValidation="false" UseSubmitBehavior="false" 
                                            OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divHopDong')"  />
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btnBaoCao" runat="server" CssClass="report" OnClick="btnBaoCao_Click"                                                                             
                                           OnClientClick="return CheckFormReport();" UseSubmitBehavior="false" TabIndex="1" />
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                  </div>
        </ContentTemplate>
    </asp:UpdatePanel>     
      <br />
    
     <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" >
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                     />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel> 
            
      
        
  
</asp:Content>

