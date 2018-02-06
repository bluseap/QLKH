<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="BienBanNghiemThu.aspx.cs" 
    Inherits="EOSCRM.Web.Forms.ThiCongCongTrinh.BienBanNghiemThu" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
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
                open: function(event, ui) {
                    $(this).parent().appendTo("#divTKVTDlgContainer");
                }
            });
        
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

            $("#divNhanVien2").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer2");
                }
            });

            $("#divNhanVien3").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer3");
                }
            });
            
            $("#divDonDangKy").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divDonDangKyDlgContainer");
                }
            });            
        });


        function CheckFormFilterDDK() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDDK) %>', '');

            return false;
        }

        function CheckFormSave() {

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

            return false;
        }

        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');

            return false;
        }

        function CheckFormSearchBB() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnLocBB) %>', '');

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

        function CheckFormFilterNV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');

            return false;
        }

        function CheckFormFilterNV2() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV2) %>', '');

            return false;
        }

        function CheckFormFilterNV3() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV3) %>', '');

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
                                                    <asp:Label ID="lbTENKH2" runat="server" Text="tenkh"></asp:Label>
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
							        <eoscrm:Grid ID="gvTKVT" runat="server" AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"							            
							            OnPageIndexChanging="gvTKVT_PageIndexChanging">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>                                            
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="MAVT" HeaderText="Mã vật tư" />
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
    <div id="divNhanVienDlgContainer2">
        <div id="divNhanVien2" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien2" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordNV2" onchange="return CheckFormFilterNV2();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterNV2" OnClick="btnFilterNV2_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterNV2();" 
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
							        <eoscrm:Grid ID="gvNhanVien2" runat="server" UseCustomPager="true" 
							            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvNhanVien2_RowDataBound" OnRowCommand="gvNhanVien2_RowCommand" 
							            OnPageIndexChanging="gvNhanVien2_PageIndexChanging">
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
    <div id="divNhanVienDlgContainer3">
        <div id="divNhanVien3" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien3" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordNV3" onchange="return CheckFormFilterNV3();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterNV3" OnClick="btnFilterNV3_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterNV3();" 
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
							        <eoscrm:Grid ID="gvNhanVien3" runat="server" UseCustomPager="true" 
							            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvNhanVien3_RowDataBound" OnRowCommand="gvNhanVien3_RowCommand" 
							            OnPageIndexChanging="gvNhanVien3_PageIndexChanging">
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
							            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvDDK_RowDataBound" OnRowCommand="gvDDK_RowCommand" 
							            OnPageIndexChanging="gvDDK_PageIndexChanging">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDK") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" HeaderText="Tên khách hàng" />
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
    <asp:UpdatePanel ID="upnlThongTin" UpdateMode="Conditional" runat="server">       
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Kỳ khai thác</td>
                            <td class="crmcell">
                                <div class="left">
                                   <asp:DropDownList ID="ddlTHANG1" runat="server" >
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
                                    <asp:TextBox ID="txtNAM1" runat="server" Width="30px" MaxLength="4" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbNV1" runat="server" Text="nv1" Visible="false"></asp:Label>
                                    <asp:Label ID="lbNV2" runat="server" Text="nv2" Visible="false"></asp:Label>
                                    <asp:Label ID="lbNV3" runat="server" Text="nv3" Visible="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày lập biên bản</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLAMBB" runat="server" Width="90px" MaxLength="200"
                                        TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNHANDON" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtLAMBB"
                                        PopupButtonID="imgNHANDON" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left">
                                    <div class="right">Hệ thống cấp nước</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtHETHONGCN" runat="server" Width="200px" MaxLength="200"
                                        TabIndex="4"  />  
                                    <ajaxToolkit:AutoCompleteExtender ID="txtHETHONGCN_AutoCompleteExtender1" runat="server" DelimiterCharacters="" 
                                        Enabled="True" ServiceMethod="GetHeThongCN" MinimumPrefixLength="1" TargetControlID="txtHETHONGCN" UseContextKey="True">
                                    </ajaxToolkit:AutoCompleteExtender>
                                </div>                                                      
                            </td>
                        </tr>                        <tr>    
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="120px" MaxLength="15" 
                                        TabIndex="1" ReadOnly="True" />
                                </div>
                                <div class="left">  
                                    <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" 
		                                OnClick="btnBrowseDDK_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" 
                                        TabIndex="2"  />
                                </div>                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lbTENKH" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Địa chỉ</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDP1" runat="server" Text=""></asp:Label>
                                </div>                                                      
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right">Danh số</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:Label ID="lbDANHSO" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Địa chỉ</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDP2" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại đồng hồ</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:Label ID="lbKICHCO" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Hiệu</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbMALDH" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Nước SX</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbNSX" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Số No</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:Label ID="lbSONO" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Chỉ số đồng hồ</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbCSDAU" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right">Mã chì mặt 1</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:Label ID="lbMACHIM1" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Mã chì mặt 2</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbMACHIM2" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtNV1" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <div class="right">Chức vụ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCV1" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtNV2" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien2" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien2_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien2')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <div class="right">Chức vụ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCV2" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtNV3" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien3" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien3_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien3')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <div class="right">Chức vụ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCV3" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chỉ số ĐH</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtCSDONGHO" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <div class="right">Chiều cao lắp</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCHIEUCAO" runat="server"></asp:TextBox>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Khoảng cách từ ống đến đồng hồ nước</td>
                            <td class="crmcell">                                
                                <div class="left" >
                                    <asp:TextBox ID="txtKHOANCACH" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Vị trí lắp</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtVITRILAP" Width="600px" runat="server"></asp:TextBox>
                                    <ajaxToolkit:AutoCompleteExtender ID="txtVITRILAP_AutoCompleteExtender1" runat="server" DelimiterCharacters="" 
                                        Enabled="True" ServiceMethod="GetViTriLap" MinimumPrefixLength="1" TargetControlID="txtVITRILAP" UseContextKey="True">
                                    </ajaxToolkit:AutoCompleteExtender>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã chì KĐ M1</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtCHIKDM1" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <div class="right">Mã chì KĐ M2</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCHIKDM2" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã số kìm mặt 1</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtCHIM1" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <div class="right">Mã số kìm mặt 2</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCHIM2" runat="server"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:LinkButton ID="lkCTVT" runat="server" onclick="lkCTVT_Click"
                                        OnClientClick="openDialogAndBlock('Thiết kế vật tư', 700, 'divThietKeVatTu')"
                                    >
                                    Phụ kiện lắp thực tế</asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Kết luận và kiến nghị sau khi chạy thử </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtKETLUAN" runat="server" Width="600px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Từ ngày </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtFromDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <strong>Đến</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtToDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Width="20px" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <asp:Button ID="btnLocBB" runat="server" CssClass="filter" 
                                        TabIndex="20" UseSubmitBehavior="false" OnClientClick="return CheckFormSearchBB();" OnClick="btnLocBB_Click" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbnt" runat="server" Text="Label" Visible="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tổng khối lượng: </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbKHOILUONG" runat="server" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tổng khách hàng:</div>
                                </div>
                                <div class="left">
                                        <asp:Label ID="lbKHACHHANG" runat="server" Text="" Font-Bold="true"></asp:Label>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click" 
                                        TabIndex="13" UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="filter" OnClick="btnSearch_Click"
                                        TabIndex="20" UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="19" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvBienBan" runat="server" UseCustomPager="true" OnRowDataBound="gvBienBan_RowDataBound" 
                    AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
                    OnRowCommand="gvBienBan_RowCommand" OnPageIndexChanging="gvBienBan_PageIndexChanging">
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altrow" />
                    <HeaderStyle CssClass="header" />
                    <PagerSettings FirstPageText="biên bản" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MABBNT") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MABBNT") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã BB" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MABBNT") %>'
                                    CommandName="EditItem" Text='<%# Eval("MABBNT") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên đơn ĐK" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# Eval("DONDANGKY.TENKH")  %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày lập" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# (Eval("NGAYLAPBB") != null) ? String.Format("{0:dd/MM/yyyy}", Eval("NGAYLAPBB")) : "" %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
           

</asp:Content>
