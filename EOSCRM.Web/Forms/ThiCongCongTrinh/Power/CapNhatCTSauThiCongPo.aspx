<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="CapNhatCTSauThiCongPo.aspx.cs" Inherits="EOSCRM.Web.Forms.ThiCongCongTrinh.Power.CapNhatCTSauThiCongPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {

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

            $("#divDongHoSoNo").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDongHoSoNoDlgContainer");
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

        function CheckFormFilterNV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');

            return false;
        }

        function CheckFormFilterDDK() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDDK) %>', '');

            return false;
        }

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');

            return false;
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

            return false;
        }

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

            return false;
        }

        function CheckFormMANV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnMANV) %>', '');

            return false;
        }

        function CheckFormFilterDHSONO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDHSONO) %>', '');
            return false;
        }

        function CheckFormMANVKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormMANV();
            }
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
                                                    <asp:TextBox ID="txtFilter" onchange="return CheckFormFilterDDK();" runat="server" Width="150px" MaxLength="200" />
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
                                                    <asp:TextBox ID="txtTuNgay" runat="server" width="90px"/>
                                                </div>
                                                <div class="left">
                                                    <div class="right">Đến ngày</div>                                                
                                                </div>
                                                <div class="left">
                                                    <div class="right">
                                                        <asp:TextBox ID="txtDenNgay" runat="server" width="90px"/>
                                                    </div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"></td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDDK" OnClick="btnFilterDDK_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDDK();" 
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
							            OnPageIndexChanging="gvDDK_PageIndexChanging" >
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDKPO") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" 
                                                HeaderText="Tên khách hàng" >
                                                <HeaderStyle Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="DIACHILD" 
                                                HeaderText="Địa chỉ lắp đặt" >
                                                <HeaderStyle Width="35%" />
                                            </asp:BoundField>
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
    <div id="divDongHoSoNoDlgContainer">
        <div id="divDongHoSoNo" style="display: none">
            <asp:UpdatePanel ID="upnlDongHoSoNo" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
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
                                                    <asp:TextBox ID="txtKeywordDHSONO" onchange="return CheckFormFilterDHSONO();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDHSONO" OnClick="btnFilterDHSONO_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDHSONO();" 
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
							        <eoscrm:Grid ID="gvDongHoSoNo" runat="server" UseCustomPager="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnPageIndexChanging="gvDongHoSoNo_PageIndexChanging" OnRowCommand="gvDongHoSoNo_RowCommand">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <PagerSettings FirstPageText="Đồng hồ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MADHPO") %>' 
                                                        CommandName="SelectMADH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADHPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Loại ĐH" DataField="MALDHPO" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Năm SX" DataField="NAMSX" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số No" DataField="SONO" />                                            
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
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="90px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left">  
                                    <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" 
		                                OnClick="btnBrowseDDK_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" 
                                        TabIndex="2"  />
                                    <asp:Label ID="lbMADDTRAHSTK" runat="server" Visible="false"></asp:Label>
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="400px" MaxLength="200" TabIndex="3" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Địa chỉ lắp đặt
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA" runat="server" Width="68px" MaxLength="200" TabIndex="4" />
                                    <asp:TextBox ID="txtDUONG" runat="server" Width="200px" MaxLength="200" TabIndex="5" />
                                    <asp:TextBox ID="txtPHUONG" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                    <asp:TextBox ID="txtKHUVUC" runat="server" Width="150px" MaxLength="200" TabIndex="7" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên phụ trách</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="90px" 
                                        onkeypress="return CheckFormMANVKeyPress(event);"
                                        MaxLength="200" TabIndex="4" />
                                    <asp:LinkButton ID="linkBtnMANV" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnMANV_Click" runat="server">Change MANV</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <strong>Tên nhân viên</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENNV" runat="server" Width="250px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày giao</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNgayGiaoThiCong" runat="server" Width="90px" MaxLength="200"
                                        TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYGIAO" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNgayGiaoThiCong"
                                        PopupButtonID="imgNGAYGIAO" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right btop">Ngày gắn</td>
                            <td class="crmcell btop">
                                <div class="left">
                                    <asp:TextBox ID="txtNgayHoanThanh" runat="server" Width="90px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYHOANTHANH" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNgayHoanThanh"
                                        PopupButtonID="imgNGAYHOANTHANH" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left">
                                    <div class="right">Ngày cập nhật</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNgayCapNhat" runat="server" Width="90px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYCAPNHAT" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNgayCapNhat"
                                        PopupButtonID="imgNGAYCAPNHAT" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Số serial</td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:TextBox ID="txtSOSERIAL" runat="server" Width="120px" MaxLength="50" 
                                        TabIndex="4" ReadOnly="True" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDHSONO" runat="server" CssClass="pickup" OnClick="btnBrowseDHSONO_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách Đồng hồ', 500, 'divDongHoSoNo')" 
                                        TabIndex="21" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMaDH" runat="server" Width="1px" MaxLength="20" 
                                        TabIndex="4" ReadOnly="True" Visible="False" />
                                </div>
                                <div class="left filtered"></div>
                                <div class="left">
                                    <div class="right">Chỉ số khi lắp</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtChiSo" runat="server" Width="120px" MaxLength="200" 
                                        TabIndex="4" ReadOnly="false" />
                                </div>
                                <div class="left filtered"></div>
                                <div class="left">
                                    <asp:CheckBox ID="chkLapQuyetToan" Checked="false" runat="server" Text="" />
                                </div>
                                <div class="left">
                                    <strong>Có lập quyết toán</strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã số chì kiểm định mặt 1</td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:TextBox ID="txtCKDM1" runat="server" Width="120px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left filtered"></div>
                                <div class="left"><div class="right">Mặt 2</div></div>
                                <div class="left">
                                    <asp:TextBox ID="txtCKDM2" runat="server" Width="120px" MaxLength="200" 
                                        TabIndex="4"  />
                                </div>
                            </td>
                        </tr>     
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtOngChinh" runat="server" Width="1px" MaxLength="200" 
                                        TabIndex="4" Visible="False" />
                                </div>                                
                                <div class="left"><div class="right"></div></div>
                                <div class="left">
                                    <asp:TextBox ID="txtDuongKinh" runat="server" Width="1px" MaxLength="200" 
                                        TabIndex="4" Visible="False" />
                                </div>                                
                                <div class="left"><div class="right"></div></div>
                                <div class="left">
                                    <asp:TextBox ID="txtMetThucTe" runat="server" Width="1px" MaxLength="200" 
                                        TabIndex="4" Visible="False" />
                                </div> 
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" runat="server" Width="1px" MaxLength="200" TabIndex="4"
                                            TextMode="MultiLine" Visible="False" />
                                </div> 
                            </td>
                        </tr>                                                      
                        <tr>
                            <td class="crmcell right btop"></td>
                            <td class="crmcell btop">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CommandArgument="Insert" CssClass="save"
                                        OnClick="btnSave_Click" TabIndex="16" OnClientClick="return CheckFormSave();" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="17" UseSubmitBehavior="false" OnClientClick="return CheckFormCancel();" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtFromDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <strong>Đến</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtToDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                 <div class="left">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="filter"
                                        OnClick="btnFilter_Click" OnClientClick="return CheckFormFilter();" TabIndex="15" UseSubmitBehavior="false" />
                                </div>  
                            </td>
                        </tr>               
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <%# (Eval("DONDANGKYPO") != null) ? Eval("DONDANGKYPO.TENKH") : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="30%">
                            <ItemTemplate>
                                <%# (Eval("DONDANGKYPO") != null) ? Eval("DONDANGKYPO.DIACHILD") : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhân viên">
                            <ItemTemplate>
                                <%# (Eval("NHANVIEN") != null) ? Eval("NHANVIEN.HOTEN") : ""%>
                            </ItemTemplate>
                            <HeaderStyle Width="15%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày giao" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYGTC") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYGTC")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Trạng thái"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgTT" runat="server" Width="90px" OnClientClick="return false;"
                                     CausesValidation="false" UseSubmitBehavior="false" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkTRAHSTK" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="TraHSVeTK" Text='Trả HS về TK'></asp:LinkButton>                                                    
                            </ItemTemplate>
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="ReportItem" Text='Báo cáo'></asp:LinkButton>                                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="" HeaderStyle-Width="40px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkSUAMACHIKD" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="SuaMaChiKD" Text='Sửa Mã chì KĐ'></asp:LinkButton>                                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />    
	<div class="crmcontainer p-5">
        <a href="/Forms/ThiCongCongTrinh/Power/BBNghiemThuPo.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
