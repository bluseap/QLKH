<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="DuyetDonTKTC.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.DuyetDonTKTC" %>
    
<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register src="../../UserControls/FilterPanel.ascx" tagname="FilterPanel" tagprefix="bwaco" %>
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
        
        function CheckFormChapNhan() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btCHAPNHAN) %>', '');
        }

        function CheckFormKhongDuDieuKien() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btKODUDIEUKIEN) %>', '');
        }        

    </script>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
							        <eoscrm:Grid ID="gvTKVT" runat="server" AutoGenerateColumns="false" CssClass="crmgrid"							            
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
                            <td class="crmcell right">Ngày duyệt </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtApproveDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgApproveDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtApproveDate"
                                    PopupButtonID="imgApproveDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <%--<div class="left">
                                    <asp:Button ID="btnApprove" CssClass="approve" runat="server" OnClientClick="return CheckFormApprove();"
                                        UseSubmitBehavior="false" onclick="btnApprove_Click" /> 
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnReject" CssClass="reject" runat="server" OnClientClick="return CheckFormReject();" 
                                        UseSubmitBehavior="false" onclick="btnReject_Click" />  
                                </div>--%>
                                <div class="left">
                                    <asp:Button ID="btCHAPNHAN" CssClass="myButton" runat="server" OnClientClick="return CheckFormChapNhan();"
                                        UseSubmitBehavior="false" onclick="btCHAPNHAN_Click" Text ="Chấp nhận"/> 
                                </div>
                                <div class="left">
                                    <asp:Button ID="btKODUDIEUKIEN" CssClass="myButton" runat="server" OnClientClick="return CheckFormKhongDuDieuKien();" 
                                        UseSubmitBehavior="false" onclick="btKODUDIEUKIEN_Click" Text ="Không đủ điều kiện"/>  
                                </div>
                            </td>                        
                        </tr>                        
                        <tr>
                            <td class="crmcell right vtop">Nội dung</td>
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
               <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_RowDataBound"
                    OnRowCommand="gvList_RowCommand"  OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20" >
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
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
                       
                        <asp:TemplateField HeaderText="Mã đơn&nbsp;">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("MADDK") %>'                                    
                                    CommandName="EditHoSo" CssClass="link" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                            <HeaderStyle Width="80px" />
                            <FooterTemplate>
                                <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                    <strong>Bỏ chọn hết</strong></a>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên khách hàng" DataField="TENKH" />
                        <asp:BoundField HeaderStyle-Width="75px" HeaderText="Điện thoại" DataField="DIENTHOAI" />
                        <asp:BoundField HeaderStyle-Width="35%" HeaderText="Địa chỉ lắp đặt" DataField="DIACHILD" />
                        <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Ngày đăng ký">
                            <ItemTemplate>
                                <%# (Eval("NGAYDK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYDK"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Ngày hẹn khảo sát">
                            <ItemTemplate>
                                <%# (Eval("NGAYHKS") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYHKS"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>                                                
                        <asp:TemplateField HeaderText="TT T.Kế"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgTT" runat="server" Width="90px" CausesValidation="false" UseSubmitBehavior="false" />
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
            <br />
            <div class="crmcontainer p-5">
                <a href="../ThietKe/NhapThietKe.aspx">Chuyển sang bước kế tiếp</a>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
