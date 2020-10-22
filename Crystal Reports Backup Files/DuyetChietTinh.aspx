﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="DuyetChietTinh.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.DuyetChietTinh" %>

<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>

<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server"> 
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divThietKeVatTu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
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

        function CheckFormTRAHOSO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btTRAHOSO) %>', '');
            return false;
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
                                                    <%# Eval("MAVT") != null ? Eval("MAVT").ToString() : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>            
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="NOIDUNG" HeaderText="Tên vật tư" />
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="SOLUONG" HeaderText="Số lương" />
                                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <%# Eval("ISCTYDTU") != null ? Eval("ISCTYDTU").ToString() == "True" ? "Cấp" : "Bán"
                                                        : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>   
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
                        <%--<tr>
                            <td class="crmcell right">Lý do từ</td>
                            <td class="crmcell">
                                <div class="left">
                                
                                </div>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="crmcell right">Ngày duyệt đơn</td>
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
                                    <asp:Button ID="btTRAHOSO" CssClass="myButton" runat="server" OnClientClick="return CheckFormTRAHOSO();" 
                                        UseSubmitBehavior="false" OnClick="btTRAHOSO_Click" Text="Trả hồ sơ Kỹ thuật, Tổ, Nhà máy" />  
                                </div>
                            </td>      
                        </tr>
                        <tr>
                            <td class="crmcell right vtop">Lý do từ chối</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNoiDung" runat="server" Width="600px" MaxLength="1000" TabIndex="4" TextMode="MultiLine" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>    
            <br />    
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="20" OnRowCommand="gvList_RowCommand"   
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound">
                    <PagerSettings FirstPageText="chiết tính" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="10px">
                            <HeaderTemplate>
                                <span class="checkbox">
                                    <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                        onclick="CheckAllItems(this);" />
                                </span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="checkbox">
                                    <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDK") %>' />
                                    <input name="listIds" type="checkbox" value='<%# Eval("MADDK") %>' />
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkMaDDK" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    OnClientClick="openDialogAndBlock('Thiết kế vật tư', 700, 'divThietKeVatTu')"
                                    CommandName="EditHoSo" CssClass="link" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <%--<asp:BoundField HeaderText="Mã đơn" ItemStyle-Font-Bold="true" HeaderStyle-Width="60px" DataField="MADDK" />--%>
                        <asp:TemplateField HeaderText="Tên KH" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <%# new DonDangKyDao().Get(Eval("MADDK").ToString()) != null ? new DonDangKyDao().Get(Eval("MADDK").ToString()).TENKH : Eval("TENCT")
                                           %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Điện thoại" HeaderStyle-Width="60px" DataField="DIENTHOAI" />
                        <asp:BoundField HeaderText="Địa chỉ lắp đặt" HeaderStyle-Width="30%" DataField="DIACHIHM" />
                        <asp:TemplateField HeaderText="Trạng thái đơn"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgTT" runat="server" Width="90px" OnClientClick="return false;"
                                     CausesValidation="false" UseSubmitBehavior="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày lập chiết tính" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# (Eval("NGAYLCT") != null) ?
                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYLCT")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <br />
    <div class="crmcontainer p-5">
        <a href="../ThietKe/NhapHopDong.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
