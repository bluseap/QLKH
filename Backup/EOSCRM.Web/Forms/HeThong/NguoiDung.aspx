<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.HeThong.NguoiDung" CodeBehind="NguoiDung.aspx.cs" %>
    
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
                }
            });
        });

        function CheckFormFilterNV() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');

		    return false;
		}

		function CheckFormSave() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

		    return false;
		}

		function CheckFormCancel() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');

		    return false;
		}
		
		function CheckFormDelete() {
		    if (CheckRecordSelected('delete')) {
		        openWaitingDialog();
		        unblockWaitingDialog();

		        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
		    }

		    return false;
		}
		
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divNhanVienDlgContainer">
        <div id="divNhanVien" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 800px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">
                                                Từ khóa
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtKeywordNV" onchange="return CheckFormFilterNV();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterNV" OnClick="btnFilterNV_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterNV();" 
                                                        runat="server" CssClass="filter" Text="" />
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
							        <eoscrm:Grid ID="gvNhanVien" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvNhanVien_RowDataBound" OnRowCommand="gvNhanVien_RowCommand" 
							            OnPageIndexChanging="gvNhanVien_PageIndexChanging">
                                        <PagerSettings FirstPageText="nhân viên" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã NV">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MANV") %>' CommandName="SelectMANV"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MANV").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="HOTEN" HeaderText="Họ tên" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Phòng ban">
                                                <ItemTemplate>
                                                    <%# Eval("PHONGBAN") != null ? Eval("PHONGBAN.TENPB") : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Công việc">
                                                <ItemTemplate>
                                                    <%# Eval("CONGVIEC") != null ? Eval("CONGVIEC.TENCV") : ""%>
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
    <asp:HiddenField ID="hdId" runat="server" />
    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Tên đăng nhập</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtUSERNAME" runat="server" Width="100px" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mật khẩu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtPASSWORD" runat="server" Width="100px" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Kích hoạt</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:CheckBox ID="cbActive" Checked="true" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click"  OnClientClick="return CheckFormSave();"
                                        TabIndex="5" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" OnClientClick="return deleteRecord();" UseSubmitBehavior="false"
                                        runat="server" CssClass="delete" OnClick="btnDelete_Click" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="6" UseSubmitBehavior="false" OnClientClick="CheckFormCancel();" />                                    
                                </div>
                            </td>
                        </tr> 
                    </tbody>
                </table>
            </div>
            <br />            
            <asp:DataList ID="dpDataList" RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table" 
                runat="server" CssClass="crmcontainer" Width="100%" OnItemDataBound="dpDataList_ItemDataBound">
                <HeaderTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow-down.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkGroupUser', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkGroupUser', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>                                        
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </HeaderTemplate>
                <FooterTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkGroupUser', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkGroupUser', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </FooterTemplate>
                <ItemTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr>
                                <td style="width: 5%; border: 0px;">
                                    <input id="chkGroupUser" title='<%# Eval("Id") %>' runat="server" type="checkbox" />
                                </td>                                    
                                <td style="width: 95%; border: 0px;">
                                    <%# Eval("Name") %>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </ContentTemplate>
	</asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid 
                    ID="gvNguoiDung" runat="server" UseCustomPager="true" PageSize="100"
                    OnRowCommand="gvNguoiDung_RowCommand" OnRowDataBound="gvNguoiDung_RowDataBound" 
                    OnPageIndexChanging="gvNguoiDung_PageIndexChanging">
                    <PagerSettings FirstPageText="người dùng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <input type="hidden" id="UserAdminId" value='<%# Eval("Id") %>' runat="server" />
                                <input type="hidden" id="UpdateDate" value='<%# Eval("UpdateDate") %>' runat="server" />
                                <input name="listIds" type="checkbox" value='<%# Eval("Id") %>' onclick="DoCheckItem();" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <FooterTemplate>
                                <input id="chkAllBottom" title="Chọn hết / Bỏ chọn hết" name="chkAllBottom" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên người dùng">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" 
                                    CommandArgument='<%# Eval("Id") %>' CommandName="EditUser"
                                    Text='<%# HttpUtility.HtmlEncode(Eval("NHANVIEN.HOTEN").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                
                        <asp:BoundField HeaderText="Tạo bởi" DataField="CreateBy" />
                        <asp:BoundField HeaderText="Cập nhật" DataField="UpdateBy" />
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
