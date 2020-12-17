<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.KhachHang.ThayDongHo" CodeBehind="ThayDongHo.aspx.cs" %>
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

            $("#divDongHoSoNo").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divDongHoSoNoDlgContainer");
                }
            });
        });

        function CheckFormFilterDHSONO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDHSONO) %>', '');
            return false;
        }

        function CheckFormFilterKH() {
            var idkh = jQuery.trim($("#<%= txtIDKH.ClientID %>").val());
            var tenkh = jQuery.trim($("#<%= txtTENKH.ClientID %>").val());
            var madh = jQuery.trim($("#<%= txtMADH.ClientID %>").val());
            var sohd = jQuery.trim($("#<%= txtSOHD.ClientID %>").val());
            var sonha = jQuery.trim($("#<%= txtSONHA.ClientID %>").val());
            var tendp = jQuery.trim($("#<%= txtTENDP.ClientID %>").val());
            var makv = jQuery.trim($("#<%= ddlKHUVUC.ClientID %>").val());

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
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(txtSODB) %>', '');
            }
        }

        function CheckSearchKH() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(txtSODB) %>', '');
            return false;
        }
        
        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckFormSaveUp() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveUp) %>', '');
            return false;
        }

        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
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

        function CheckFormXuatExcel() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btXuatExcel) %>', '');
            return false;
        }

        function CheckFormbtDSChuaThay() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btDSChuaThay) %>', '');
            return false;
        }
        
    </script>

</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
							        <eoscrm:Grid ID="gvDongHoSoNo" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvDongHoSoNo_PageIndexChanging" OnRowCommand="gvDongHoSoNo_RowCommand">
                                        <PagerSettings FirstPageText="Đồng hồ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MADH") %>' 
                                                        CommandName="SelectMADH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADH").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Loại ĐH" DataField="MALDH" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Năm SX" DataField="NAMSX" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số No" DataField="SONO" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số ĐK" DataField="SOKD" /> 
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số tem" DataField="TEMKD" />                                            
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
                                                            <asp:TextBox ID="txtTENKH" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Mã đồng hồ
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtMADH" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
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
                                                            <asp:DropDownList ID="ddlKHUVUC" runat="server" CssClass="width-150"></asp:DropDownList>
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                           <div class="rightsmall">Trạng thái ghi</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:DropDownList ID="ddlTrangThaiGhi" runat="server"  CssClass="width-150"></asp:DropDownList>
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
                                                            CommandArgument='<%# Eval("IDKH") %>'
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
    <asp:UpdatePanel ID="upnlThongTin" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Kỳ </td>
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
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" OnTextChanged="txtSODB_TextChanged" 
                                        onchange="CheckSearchKH();" MaxLength="8" Width="90px" TabIndex="7" 
                                        ReadOnly="True" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblIDKH" runat="server" Visible="False"></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblID" runat="server" Visible="False"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lblTENKH" runat="server" Text=""></asp:Label>
                                </div>  
                                <div class="left">
                                    <asp:Label ID="lblNGAYTHAY" runat="server" Visible="False"></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblNGAYHOANTHANH" runat="server" Visible="False"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lbMucDichSuDung" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lblTENDP" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENKV" runat="server" Text=""></asp:Label>
                                </div>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại TK</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lblLOAITK" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Số No</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblSONO" runat="server" Text=""></asp:Label>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chỉ số đầu</td>
                            <td class="crmcell">   
                                <div class="left width-200">
                                    <asp:Label ID="lblCSDAU" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Chỉ số cuối</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblCSCUOI" runat="server" Text=""></asp:Label>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chỉ số ngưng</td>
                            <td class="crmcell">                                
                                <div class="left width-200">
                                        <asp:TextBox ID = "txtCSNGUNG" runat ="server" Width="78px" TabIndex="4" OnTextChanged="txtCSNGUNG_TextChanged" AutoPostBack="true"/>
                                </div>
                                <div class="left">
                                    <div class="right">Truy thu</div>
                                </div>
                                <div class="left">
                                        <asp:TextBox ID = "txtTRUYTHU" runat ="server" Width="78px" TabIndex="5" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại TK thay</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID= "cboLoaiDh" runat ="server" Width="120px" TabIndex="6" Enabled="false"></asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Số No thay</div>
                                </div>                                
                                <div class="left">
                                    <asp:TextBox ID = "txtMaDongho" runat ="server" Width="108px" TabIndex="7" 
                                        Visible="False" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbSONODH" runat="server" Font-Bold="True" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDHSONO" runat="server" CssClass="pickup" OnClick="btnBrowseDHSONO_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách Đồng hồ', 500, 'divDongHoSoNo')" 
                                        TabIndex="21" />
                                </div>                                                         
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Công suất</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lbCONGSUATLX" runat="server" ></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chỉ số bắt đầu</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID = "txtCSBATDAU" runat ="server" Width="78px" TabIndex="8" />
                                </div>
                                <div class="left">
                                    <div class="right">Chỉ số mới</div>
                                </div>
                                <div class="left width-200">
                                    <asp:TextBox ID = "txtCSMOI" runat ="server" Width="108px" TabIndex="9" />
                                </div> 
                            </td>
                        </tr>                     
                        <tr>    
                            <td class="crmcell right">Ngày thay</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID = "txtNgayThay" runat ="server" Width="78px" TabIndex="10" 
                                         ontextchanged="txtNgayThay_TextChanged"/>
                                </div>
                                <div class="left width-100">
                                    <asp:ImageButton runat="Server" ID="imgNGAYTHAY" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNgayThay"
                                        PopupButtonID="imgNGAYTHAY" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left">
                                    <div class="right">Ngày bấm chì</div>
                                </div>
                                <div class="left">
                                     <asp:TextBox ID = "txtNgayHoanThanh" runat ="server" Width="78px" 
                                         TabIndex="11" ontextchanged="txtNgayHoanThanh_TextChanged" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYHOANTHANH" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNgayHoanThanh"
                                        PopupButtonID="imgNGAYHOANTHANH" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"> </td>
                            <td class="crmcell">                                
                                <div class="left width-200">
                                     <asp:DropDownList ID="ddlKICHCODH" Width="120px" runat="server" TabIndex="21" Visible="False">
                                        <asp:ListItem Text="15" Value="15" />
                                        <asp:ListItem Text="20" Value="20" />
                                         <asp:ListItem Text="25" Value="25" />
                                        <asp:ListItem Text="34" Value="34" />
                                         <asp:ListItem Text="40" Value="40" />
                                        <asp:ListItem Text="42" Value="42" />
                                        <asp:ListItem Text="49" Value="49" />
                                         <asp:ListItem Text="50" Value="50" />
                                        <asp:ListItem Text="60" Value="60" />
                                        <asp:ListItem Text="100" Value="100" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Đồng hồ</div>
                                </div> 
                                <div class="left">
                                     <asp:DropDownList ID="ddlDONGHOCAPBAN" runat="server" TabIndex="21">
                                        <asp:ListItem Text="Chưa biết" Value="KO" />
                                        <asp:ListItem Text="Cấp" Value="CAP" />
                                        <asp:ListItem Text="Bán" Value="BAN" />                                        
                                    </asp:DropDownList>
                                </div>                            
                            </td>
                        </tr> 
                        <tr>
                            <td class="crmcell right">Mã trạng thái</td>
                            <td class="crmcell">                                
                                <div class="left width-200">
                                     <asp:TextBox ID = "txtMATRANGTHAI" runat ="server" Width="78px" TabIndex="8" />
                                </div>                                                       
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlXAPHUONG" runat="server" OnSelectedIndexChanged="ddlXAPHUONG_SelectedIndexChanged" AutoPostBack="true"
                                        Visible="false"></asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlAPKHOM" runat="server" Visible="false"></asp:DropDownList>
                                </div>
                            </td>                                  
                        </tr>
                        <tr>
                            <td class="crmcell right">Lý do thay</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlLYDOTHAYDH" runat="server" >
                                        <asp:ListItem Text="Không biết" Value="%" />
                                        <asp:ListItem Text="Định kỳ 5 năm" Value="5" />
                                        <asp:ListItem Text="Chết" Value="C" />
                                        <asp:ListItem Text="Đứt chì" Value="D" />                                             
                                        <asp:ListItem Text="Hư hỏng" Value="H" />
                                        <asp:ListItem Text="Đồng hồ gắn tạm" Value="K" />
                                        <asp:ListItem Text="Mua" Value="M" />
                                        <asp:ListItem Text="Chống thất thoát" Value="T" />

                                        <asp:ListItem Text="Luật đo lường" Value="V" />   
                                        <asp:ListItem Text="Nâng công suất" Value="N" />                                  
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>                      
                        <tr>                                
                            <td class="crmcell">
                                <div class="left">
                                     <asp:TextBox ID = "txtSoTem" runat ="server" Width="108px" visible="false"/>
                                </div>
                                
                            </td>
                        </tr>                          
                        <tr>    
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" Rows="3" Width="525" 
                                        MaxLength="500" runat="server" Font-Names="Times New Roman" 
                                        TabIndex="12" />
                                </div>
                            </td>
                        </tr>
                         <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>                                
                            </td>                                  
                        </tr> 
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSaveUp" runat="server" CssClass="save" OnClick="btnSaveUp_Click" 
                                        TabIndex="13" UseSubmitBehavior="false" 
                                        OnClientClick="return CheckFormSaveUp();" Visible="False" />
                                </div>    
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click" 
                                        TabIndex="13" UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSearch" runat="server" CssClass="filter" OnClick="btnSearch_Click"
                                        TabIndex="20" UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" />
                                </div>
                                <div class="left width-200">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="19" UseSubmitBehavior="false" />
                                </div>
                                
                                <div class="left">
                                    <asp:Button ID="btXuatExcel" runat="server" CssClass="myButton" OnClientClick="return CheckFormXuatExcel();" 
                                        Text="Xuất Excel" UseSubmitBehavior="false" OnClick="btXuatExcel_Click" />
                                </div>
                               <div class="left">
                                    <asp:Button ID="btDSChuaThay" runat="server" CssClass="myButton" OnClientClick="return CheckFormbtDSChuaThay();"
                                             TabIndex="19" UseSubmitBehavior="false" Text="DS chưa thay ĐH" OnClick="btDSChuaThay_Click"/>
                                </div>
                                <%--<div class="left">
                                    <div class="right">Kỳ áp dụng</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANG" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckSearch();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="3" />
                                </div>--%>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btXuatExcel" />     
            <asp:PostBackTrigger ControlID="btDSChuaThay" />         
        </Triggers>
    </asp:UpdatePanel>  
    <br />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvKhachHang" runat="server" UseCustomPager="true" PageSize="500"
                    OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging"
                    OnRowDataBound="gvKhachHang_RowDataBound" >
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("ID") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("ID") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>                        
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Danh số">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("ID") %>' 
                                    CommandName="SelectTDH" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("KHACHHANG.MADP").ToString()+Eval("KHACHHANG.DUONGPHU").ToString() + Eval("KHACHHANG.MADB").ToString()) %>'>
                                </asp:LinkButton>                               
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.TENKH")%>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateField>                             
                        <asp:TemplateField HeaderStyle-Width="18%" HeaderText="Ngày thay">
                            <ItemTemplate>
                                <%# (Eval("NGAYTD") != null) ?
                                        String.Format("{0:dd/MM/yy}", Eval("NGAYTD"))
                                        : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="18%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="18%" HeaderText="Ngày hoàn thành">
                            <ItemTemplate>
                                <%# (Eval("NGAYHT") != null) ?
                                     String.Format("{0:dd/MM/yy}", Eval("NGAYHT"))
                                        : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="18%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="Chỉ số BĐ" DataField="CHISOBATDAU" >
                            <HeaderStyle Width="7%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="CS mới" DataField="CHISOMOI" >
                            <HeaderStyle Width="7%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="Truy thu" DataField="MTRUYTHU" >
                            <HeaderStyle Width="7%" />
                        </asp:BoundField>
                    </Columns>
                </eoscrm:Grid> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
