<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.SuaChua.TraCuuSuaChua" CodeBehind="TraCuuSuaChua.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#divKhachHang").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divKhachHangDlgContainer");
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
        });

        function CheckFormFilterKH() {
            var idkh = jQuery.trim($("#<%= txtIDKH.ClientID %>").val());
            var tenkh = jQuery.trim($("#<%= txtTENKH.ClientID %>").val());
            var madh = jQuery.trim($("#<%= txtMADHFilter.ClientID %>").val());
            var sohd = jQuery.trim($("#<%= txtSOHD.ClientID %>").val());
            var sonha = jQuery.trim($("#<%= txtSONHA.ClientID %>").val());
            var tendp = jQuery.trim($("#<%= txtTENDP.ClientID %>").val());
            var makv = jQuery.trim($("#<%= ddlKHUVUCKH.ClientID %>").val());

            if (idkh == '' && tenkh == '' && madh == '' &&
                    sohd == '' && sonha == '' && tendp == '' && (makv == '' || makv == 'NULL')) {
                showError('Chọn tối thiểu một thông tin để lọc khách hàng.', '<%= txtIDKH.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterKH) %>', '');

            return false;
        }

        function CheckChangeKH(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormSearch();
            }
        }

        function CheckChangeMAKH() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(txtMAKH) %>', '');

            return false;
        }

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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divKhachHangDlgContainer">
        <div id="divKhachHang" style="display: none">
            <asp:UpdatePanel ID="upnlKhachHang" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="3" cellspacing="1" style="width: 610px;">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Panel ID="headerPanel" runat="server" CssClass="crmcontainer">
                                        <table class="crmtable">
                                            <tbody>
                                                <tr class="crmfilter">
                                                    <td class="crmcell">
                                                        <div class="wrap">
                                                            <asp:ImageButton ID="imgCollapse" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                                                AlternateText="Hiện bộ lọc" />
                                                        </div>
                                                        <div class="wrap">
                                                            <asp:Label ID="lblCollapse" runat="server">Click vào để hiển thị bộ lọc</asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>    
                                    </asp:Panel>
                                    <asp:Panel ID="contentPanel" runat="server" CssClass="crmcontainer cleantop">
                                        <table class="crmtable">
                                            <tbody>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Mã khách hàng
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtIDKH" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                            <div class="rightsmall">Tên khách hàng</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:TextBox ID="txtTENKHFILTER" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Mã đồng hồ
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtMADHFilter" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                            <div class="rightsmall">Số hợp đồng</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:TextBox ID="txtSOHD" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Số nhà
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtSONHA" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                           <div class="rightsmall">Tên đường phố</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:TextBox ID="txtTENDP" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Khu vực
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:DropDownList ID="ddlKHUVUCKH" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall"></td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:Button ID="btnFilterKH" OnClick="btnFilterKH_Click"
                                                                UseSubmitBehavior="false" OnClientClick="return CheckFormFilterKH();" 
                                                                runat="server" CssClass="filter" Text="" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="Server" 
                                        Collapsed="false"
                                        TargetControlID="contentPanel"
                                        ExpandControlID="headerPanel" 
                                        CollapseControlID="headerPanel" 
                                        TextLabelID="lblCollapse"
                                        ImageControlID="imgCollapse" 
                                        ExpandedText="Click vào để ẩn bộ lọc" 
                                        CollapsedText="Click vào để hiển thị bộ lọc"
                                        ExpandedImage="~/content/images/icons/collapsed.png" 
                                        CollapsedImage="~/content/images/icons/expanded.png" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ptop-10" id="tdDanhSach" runat="server" visible="false">
                                    <div class="crmcontainer">                                   
                                        <eoscrm:Grid ID="gvDanhSach" runat="server" UseCustomPager="true" 
							                OnPageIndexChanging="gvDanhSach_PageIndexChanging" 
							                OnRowDataBound="gvDanhSach_RowDataBound" OnRowCommand="gvDanhSach_RowCommand">
                                            <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã KH">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnID" runat="server" CommandName="SelectSODB" 
                                                            CommandArgument='<%# Eval("MADP") + Eval("DUONGPHU").ToString() + Eval("MADB") %>'
                                                            Text='<%# Eval("MADP") + Eval("DUONGPHU").ToString() + Eval("MADB") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderStyle-Width="35%" DataField="TENKH" HeaderText="Tên khách hàng" />
                                                <asp:TemplateField HeaderStyle-Width="55%" HeaderText="Địa chỉ">
                                                    <ItemTemplate>
                                                        <%# Eval("SONHA") != null ? Eval("SONHA") + ", " : "" %>
                                                        <%# Eval("DUONGPHO") != null ? Eval("DUONGPHO.TENDP") + ", " : "" %>
                                                        <%# Eval("KHUVUC") != null ? Eval("KHUVUC.TENKV") : "" %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </eoscrm:Grid>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
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
    
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Mã đơn</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADON" runat="server" Width="100px" MaxLength="20" TabIndex="1" />
                                </div>
                            </td>
                            <td class="crmcell right">Nội dung báo</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMAPH" runat="server" Width="300px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Xác nhận thông tin</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="cboXacNhanThongTin" runat="server">
                                        <asp:ListItem Selected="True" Value="0">Đúng</asp:ListItem>
                                        <asp:ListItem Value="1">Sai</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">Xác nhận biên bản</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:CheckBox ID="chkBIENBAN" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Lần xử lý</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="cboLANXL" runat="server">
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="7">7</asp:ListItem>
                                        <asp:ListItem Value="8">8</asp:ListItem>
                                        <asp:ListItem Value="9">9</asp:ListItem>
                                        <asp:ListItem Value="10">10</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">Lý do xử lý</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtLyDo" runat="server" MaxLength="100" Width="100px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại xử lý</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtLoaiXL" runat="server"  Width = "300px" Enabled ="false" />                                    
                                </div>
                            </td>
                            <td class="crmcell right">Cỡ đồng hồ</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtCODH" runat="server" MaxLength="100" Width="100px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Trình trạng niêm chì</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtCHINIEM" runat="server" MaxLength="50" Width="100px" Text="" />
                                </div>
                            </td>
                            <td class="crmcell right">Mã đồng hồ</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMaDH" runat="server" MaxLength="50" Width="100px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chỉ số trước</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtChiSoTruoc" runat="server" MaxLength="10" Width="100px" />
                                </div>
                            </td>
                            <td class="crmcell right">Chỉ số sau</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtChiSoSau" runat="server" MaxLength="10" Width="100px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Trạng thái</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTrangThai" runat="server" Width = "150px" Enabled = "false" />
                                   <%-- <asp:DropDownList ID="cboTrangThaiDon" runat="server" AutoPostBack="True" 
                                        OnSelectedIndexChanged="cboTrangThaiDon_SelectedIndexChanged" 
                                        Enabled="False">
                                        <asp:ListItem Value="SC_F">Đã sửa chữa</asp:ListItem>
                                        <asp:ListItem Value="SC_I">Đang sửa chữa</asp:ListItem>
                                    </asp:DropDownList>--%>
                                </div>
                            </td>
                            <td class="crmcell right">Có lập chiết tính?</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtLapChietTinh" runat="server"  Width= "100px" Enabled ="false" />
                                    <%--<asp:CheckBox ID="chkIsLapChietTinh" runat="server" Enabled="False" />--%>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMAKH" runat="server" Width="100px" MaxLength="300" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                </div>
                            </td>
                            <td class="crmcell right">Họ tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="300px" MaxLength="300" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Thông tin khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTHONGTINKH" runat="server" Width="300px" MaxLength="300" TabIndex="1" />
                                </div>
                            </td>
                            <td class="crmcell right">Số điện thoại</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSDT" runat="server" Width="300px" MaxLength="15" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên giải quyết</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="100px" MaxLength="300" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                            </td>
                            <td class="crmcell right">Họ tên nhân viên</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTENNV" runat="server" Width="300px" MaxLength="300" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày giải quyết</td>
                            <td class="crmcell" colspan="3">    
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYNQ" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left"><strong>Giờ</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtGio" runat="server" Width="30px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left"><strong>Phút</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtPhut" runat="server" Width="30px" MaxLength="15" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right vtop">Ghi chú</td>
                            <td class="crmcell" colspan="3">    
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" runat="server" Width="795px" MaxLength="1000" TabIndex="1"
                                        TextMode="MultiLine" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell" colspan="3">    
                                <div class="left">
                                   <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" TabIndex="17" UseSubmitBehavior="false" /> 
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                         OnClientClick="return CheckFormCancel();" TabIndex="18" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
        
    <br />
    <bwaco:FilterPanel ID="filterPanel" runat="server" ShowAreaCode="false" />
    <br />
        
    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_RowDataBound" 
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADON") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã KH" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <%# 
                                    (Eval("KHACHHANG") != null) ? string.Format("{0}{1}{2}", 
                                        Eval("KHACHHANG.MADP"), Eval("KHACHHANG.DUONGPHU"), Eval("KHACHHANG.MADB")) 
                                    : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên KH" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# 
                                    (Eval("KHACHHANG") != null) ? string.Format("{0}", Eval("KHACHHANG.TENKH")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# (Eval("KHACHHANG") != null) ? 
                                        (Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : "") +
                                        Eval("KHACHHANG.DUONGPHO.TENDP") + ", " +
                                        (Eval("KHACHHANG.PHUONG") != null ? Eval("KHACHHANG.PHUONG.TENPHUONG") + ", " : "") +
                                        Eval("KHACHHANG.KHUVUC.TENKV")                                                        
                                    : Eval("THONGTINKH")
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Thông tin XL" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# (Eval("THONGTINXULY") != null) ? Eval("THONGTINXULY.TENXL") : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhân viên xử lý" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <%# 
                                    (Eval("NHANVIEN1") != null) ? string.Format("{0}", Eval("NHANVIEN1.HOTEN")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Ngày báo">
                            <ItemTemplate>
                                <%# (Eval("NGAYBAO") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYBAO"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="ReportItem" Text='Báo cáo'></asp:LinkButton>                                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>            
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
