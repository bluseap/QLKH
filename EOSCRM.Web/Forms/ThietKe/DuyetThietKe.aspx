﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="DuyetThietKe.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.DuyetThietKe" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>
    
<%@ Import Namespace="EOSCRM.Util"%>

<%@ Register src="../../UserControls/FilterPanel.ascx" tagname="FilterPanel" tagprefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server"> 
    <script type="text/javascript">        
       $(document).ready(function() {
            $("#divThietKeVatTu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divTKVTDlgContainer");
                }
            });
        });    
    
        function CheckFormApprove() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnApprove) %>', '');
        }

        function CheckFormReject() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnReject) %>', '');
        }

        function CheckFormbtChoTK() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btChoTK) %>', '');
        }
        
        function CheckFormbtThietKeLaiChuaChietTinh() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btThietKeLaiChuaChietTinh) %>', '');
        }
        
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divTKVTDlgContainer">
        <div id="divThietKeVatTu" style="display: none">
            <asp:UpdatePanel ID="upnlThietKeVatTu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 700px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Label ID="lbTENKH" runat="server" Text="tenkh"></asp:Label>
                                                    <asp:Label ID="lbMADDK" runat="server" Visible="false"></asp:Label>
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
							        <eoscrm:Grid ID="gvTKVT" runat="server"  UseCustomPager="true"			            
							            OnPageIndexChanging="gvTKVT_PageIndexChanging">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ký hiệu" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <%# Eval("VATTU.KYHIEUVT") != null ? Eval("VATTU.KYHIEUVT").ToString() : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>            
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="NOIDUNG" HeaderText="Tên vật tư" />
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="SOLUONG" HeaderText="Số lương" />
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
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
    <br />    
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>  
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Ngày duyệt đơn thiết kế</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtApproveDate" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgApproveDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtApproveDate"
                                    PopupButtonID="imgApproveDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <asp:Button ID="btnApprove" CssClass="approve" runat="server" UseSubmitBehavior="false" 
                                        OnClientClick="return CheckFormApprove();" onclick="btnApprove_Click" />    
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnReject" CssClass="reject" runat="server" UseSubmitBehavior="false" 
                                        OnClientClick="return CheckFormReject();" onclick="btnReject_Click" />    
                                </div>
                                <div class="left">                                   
                                    <asp:Button ID="btThietKeLaiChuaChietTinh" CssClass="myButton" runat="server" UseSubmitBehavior="false" 
                                        Text="Thiết kế lại chưa Chiết tính"
                                        OnClientClick="return CheckFormbtThietKeLaiChuaChietTinh();" OnClick="btThietKeLaiChuaChietTinh_Click" />                                       
                                </div> 
                            </td>                                                    
                        </tr>
                        <tr>
                            <td class="crmcell right vtop">Lý do từ chối thiết kế</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNoiDung" runat="server" Width="600px" MaxLength="1000" TabIndex="4" TextMode="MultiLine" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right vtop"></td>
                            <td class="crmcell">
                                <div class="left">                                   
                                    <asp:Button ID="btChoTK" CssClass="myButton" runat="server" UseSubmitBehavior="false" Text="Chờ TK"
                                        OnClientClick="return CheckFormbtChoTK();" OnClick="btChoTK_Click" />                                       
                                </div>                               
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>    
            <br />    
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand" OnRowDataBound="gvList_RowDataBound"                   
                    OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">                    
                    <PagerSettings FirstPageText="thiết kế" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDK") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDK") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    OnClientClick="openDialogAndBlock('Thiết kế vật tư', 700, 'divThietKeVatTu')"
                                    CommandName="EditHoSo" CssClass="link" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>                        
                        <asp:BoundField HeaderText="Tên khách hàng" HeaderStyle-Width="200px" DataField="TENKH" />
                        <asp:BoundField HeaderText="Tên công trình" HeaderStyle-Width="20%" DataField="TENTK" />
                        <asp:BoundField HeaderText="Điện thoại" HeaderStyle-Width="60px" DataField="DIENTHOAI" />
                        <asp:BoundField HeaderText="Địa chỉ lắp đặt" HeaderStyle-Width="35%" DataField="DIACHILD" />
                        <asp:TemplateField HeaderText="Ngày thiết kế" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# (Eval("NGAYLTK") != null) ?
                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYLTK")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TT T.Kế"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgTK" runat="server" Width="90px" OnClientClick="return false;"
                                     CausesValidation="false" UseSubmitBehavior="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="TT Ch.Tính"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgCT" runat="server" Width="90px" OnClientClick="return false;"
                                     CausesValidation="false" UseSubmitBehavior="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <br />
    <div class="crmcontainer p-5">
        <a href="../ThietKe/LapChietTinh.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
