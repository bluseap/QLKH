<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="NhapTKPower.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.Power.NhapTKPower" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Src="/UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
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

            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
                }
            });
        });

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
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

        function CheckFormTUCHOI() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnTUCHOI) %>', '');
            return false;
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
                                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
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
							            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvNhanVien_RowDataBound" OnRowCommand="gvNhanVien_RowCommand" 
							            OnPageIndexChanging="gvNhanVien_PageIndexChanging">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle CssClass="header" />
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
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Mã đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="90px" MaxLength="10" 
                                        TabIndex="1" ReadOnly="True" />
                                    <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" OnClick="btnBrowseDDK_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="400px" MaxLength="200" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên thiết kế</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENTK" runat="server" Width="400px" MaxLength="200" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên trạm</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlTENTRAMDS" runat="server"></asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Danh số</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" Width="100px" MaxLength="8" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Số trụ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOTRUKH" runat="server" Width="50px" MaxLength="10" TabIndex="3" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>                                
                                <div class="left">
                                    <div class="right">Tuyến đường dây hạ thế</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTUYENDAYHATHEKH" runat="server" Width="150px" MaxLength="50" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtNV1" runat="server" ReadOnly="True"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbNV1" runat="server" Text="Label" Visible="False"></asp:Label>
                                </div>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKS" onkeypress="return CheckKeywordFilter(event);" runat="server"
                                        Width="90px" MaxLength="10" Visible=false/>
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible=false/>
                                </div>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYKS"
                                    PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <strong>Ngày thiết kế</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYTK" onkeypress="return CheckKeywordFilter(event);" runat="server"
                                        Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYTK"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chú thích</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCHUTHICH" Width="400px" TextMode="MultiLine" runat="server" Height="61px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTHECHAP" runat="server" Width="130px" MaxLength="20" TabIndex="9" Visible=false OnTextChanged="txtTHECHAP_TextChanged"/>
                                </div>
                                <div class="left width-150">
                                    <div class="right">
                                        <asp:CheckBox ID="cbTHAMGIAONGCAI" runat="server" TabIndex="34" Visible=false/>
                                    </div>
                                </div>
                                <div class="left"><strong></strong></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="16" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClientClick="return CheckFormCancel();" 
                                        OnClick="btnCancel_Click" TabIndex="17" UseSubmitBehavior="false" />
                                </div>                                
                                <div class="left">
                                    <asp:Button ID="btnTUCHOI" CssClass="reject" runat="server" OnClientClick="return CheckFormTUCHOI();" 
                                        UseSubmitBehavior="false" OnClick="btnTUCHOI_Click" />  
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />               
        </Triggers>--%>
    </asp:UpdatePanel>
    <br />
    
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
    <br />
    
    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvThietKe" runat="server" UseCustomPager="true" PageSize="20"
                    OnRowCommand="gvThietKe_RowCommand" OnPageIndexChanging="gvThietKe_PageIndexChanging">
                    <PagerSettings FirstPageText="thiết kế" PageButtonCount="2" />
                    <Columns>
                        <%--<asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDK") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDK") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="SelectMADDK" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MADDKPO").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên đơn ĐK" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# Eval("DONDANGKYPO.TENKH")  %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên thiết kế" DataField="TENTK" HeaderStyle-Width="25%" />
                        <asp:TemplateField HeaderText="Địa chỉ lắp đặt" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# Eval("DONDANGKYPO.DIACHILD")  %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày KS" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <%# (Eval("DONDANGKYPO.NGAYKS") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("DONDANGKYPO.NGAYKS")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày TK" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <%# (Eval("NGAYLTK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYLTK")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="File TK" HeaderStyle-Width="160px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="editTK" CssClass="link" Text='Sửa'></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBocVatTu" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="SelectMADDK" CssClass="link" Text='Bốc vật tư'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />
    <div class="crmcontainer p-5">
        <a href="/Forms/ThietKe/Power/DuyetTKPower.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
