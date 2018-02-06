<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.CongNo.ThuTaiQuay" CodeBehind="ThuTaiQuay.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>



<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        var isValid = true; 
		$(document).ready(function() {
            $("#divDuongPho").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divDuongPhoDlgContainer");
                }
            });

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
        }); 
        
        function CheckFormFilterDP() {

            openWaitingDialog();
            unblockWaitingDialog();
            
           __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
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
                return CheckFormSearch();
            }
        }

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }
        
        function getPrefixId(controlId) {
            return controlId.substring(0, controlId.lastIndexOf('_'));
        }

        function getControlName(controlId) {
            return controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
        }


        function getNextId(controlId) {
            var ctrlName = controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
            var prefix = controlId.substring(0, controlId.lastIndexOf('_'));
            var index = prefix.substring(prefix.lastIndexOf('_') + 4, prefix.length);
            var shortPrefix = prefix.substring(0, prefix.lastIndexOf('_')) + '_ctl';

            var nextIndex = parseInt(index, 11) + 1;
            if (nextIndex < 11)
                nextIndex = '0' + nextIndex;

            return shortPrefix + nextIndex + '_' + ctrlName;
        }

        function getPrevId(controlId) {
            var ctrlName = controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
            var prefix = controlId.substring(0, controlId.lastIndexOf('_'));
            var index = prefix.substring(prefix.lastIndexOf('_') + 4, prefix.length);
            var shortPrefix = prefix.substring(0, prefix.lastIndexOf('_')) + '_ctl';

            var nextIndex = parseInt(index, 11) - 1;
            if (nextIndex < 11)
                nextIndex = '0' + nextIndex;

            return shortPrefix + nextIndex + '_' + ctrlName;
        }
    </script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDuongPhoDlgContainer">
        <div id="divDuongPho" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeyword" onchange="return CheckFormFilterDP();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDP" OnClick="btnFilterDP_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDP();" 
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
							        <eoscrm:Grid ID="gvDuongPho" runat="server" AllowPaging="true" AutoGenerateColumns="false"  
							            CssClass="crmgrid" UseCustomPager="true" 
							            OnPageIndexChanging="gvDuongPho_PageIndexChanging" OnRowCommand="gvDuongPho_RowCommand">
                                        <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="40%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' 
                                                        CommandName="SelectMADP" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHU" HeaderText="Đường phụ" />
                                            <asp:BoundField HeaderStyle-Width="50%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Khu vực">
                                                <ItemTemplate>
                                                    <%# Eval("KHUVUC.TENKV") %>
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
                                                            <asp:DropDownList ID="ddlKHUVUC" runat="server"></asp:DropDownList>
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
                                            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							                OnPageIndexChanging="gvDanhSach_PageIndexChanging" 
							                OnRowDataBound="gvDanhSach_RowDataBound" OnRowCommand="gvDanhSach_RowCommand">
							                <RowStyle CssClass="row" />
                                            <AlternatingRowStyle CssClass="altrow" />
                                            <HeaderStyle CssClass="header" />
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
    <asp:UpdatePanel ID="upnlGhiChiSo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">Mã đường</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:TextBox ID="txtMADP" runat="server" MaxLength="4" Width="40px" TabIndex="7" />
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="30px" TabIndex="8" />
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')"
                                        TabIndex="9" />
                                </div>
                                <div class="left">
                                    <div class="right width-150">Từ ngày</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTuNgay" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left"><strong>Đến ngày</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtDenNgay" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Kỳ</td>
                            <td class="crmcell">    
                                <div class="left width-200">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="70px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <div class="right width-150">Ngày thu</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYCN" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số DB</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:TextBox ID="txtSODB" runat="server" onkeydown="return CheckChangeKH(event);" MaxLength="8" Width="90px" TabIndex="7" />                           
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                    <asp:Label ID="lblSoDb" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right width-150">Tên khách hàng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTHONGTINKH" runat="server" Width="300px" MaxLength="300" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số tiền</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:TextBox ID="txtSoTien" runat="server" Width="100px" MaxLength="500" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right width-150">Ghi chú</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChuCn" runat="server" Width="300px" MaxLength="500" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">   
                                <div class="left">
                                    <asp:Button ID="btnTimKiem" runat="server" CssClass="filter" OnClick="btnTimKiem_Click" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btn_ThemMoi" runat="server" CssClass="addnew" OnClick="btn_ThemMoi_Click" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btn_Delete" runat="server" CssClass="delete" OnClick="btnDelete_Click"
                                        OnClientClick="return deleteRecord();" TabIndex="18" UseSubmitBehavior="false" />
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
            <div class="crmcontainer" id="divttqList" runat="server" >            
                    <eoscrm:Grid ID="gvList" runat="server" AutoGenerateColumns="false" UseCustomPager="true"
                        CssClass="crmgrid"                    
                        OnPageIndexChanging="gvList_PageIndexChanging">
                        <RowStyle CssClass="row" />
                        <AlternatingRowStyle CssClass="altrow" />
                        <HeaderStyle CssClass="header" />                 
                        <PagerSettings FirstPageText="Thu TQ" PageButtonCount="2" />
                        <HeaderStyle HorizontalAlign="Left" />
                        <Columns>
                            <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                <HeaderTemplate>
                                    <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                        onclick="CheckAllItems(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <input id="Id" runat="server" type="hidden" value='<%# Eval("ID") %>' />
                                    <input name="listIds" type="checkbox" value='<%# Eval("ID").ToString() %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Mã KH">
                                <ItemTemplate>
                                    <%# 
                                        (Eval("TIEUTHU.KHACHHANG") != null) ? Eval("TIEUTHU.KHACHHANG.MADP").ToString() + Eval("TIEUTHU.KHACHHANG.DUONGPHU").ToString() + Eval("TIEUTHU.KHACHHANG.MADB").ToString() 
                                            : ""
                                    %>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="10%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tên KH">
                                <ItemTemplate>
                                    <%# 
                                        (Eval("TIEUTHU.KHACHHANG") != null) ? Eval("TIEUTHU.KHACHHANG.TENKH").ToString() : ""
                                    %>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="15%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Địa chỉ">
                                <ItemTemplate>
                                    <%# 
                                        (Eval("TIEUTHU.KHACHHANG") != null) ? Eval("TIEUTHU.KHACHHANG.SONHA").ToString() != "" ?
                                                                                                                   Eval("TIEUTHU.KHACHHANG.SONHA").ToString() + " , " + Eval("TIEUTHU.DUONGPHO.TENDP").ToString() + " , " + Eval("TIEUTHU.KHACHHANG.KHUVUC.TENKV").ToString()
                                                                                           : Eval("TIEUTHU.DUONGPHO.TENDP").ToString() + " , " + Eval("TIEUTHU.KHACHHANG.KHUVUC.TENKV").ToString() : ""
                                    %>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="20%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Số tiền">
                                <ItemTemplate>
                                    <%# Eval("SOTIEN")%>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="7%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Tiền nước">
                                <ItemTemplate>
                                    <%# Eval("TIENNUOC")%>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="7%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phí">
                                <ItemTemplate>
                                    <%# Eval("PHI")%>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="7%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ngày nhập">
                                <ItemTemplate>
                                    <%# 
                                        (Eval("NGAYNHAP") != null) ?
                                                                    DateTimeUtil.GetDateStringToDisplay(Eval("NGAYNHAP").ToString())
                                            : ""
                                    %>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="7%" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Ghi chú">
                                <ItemTemplate>
                                    <%# Eval("GHICHU") %>
                                </ItemTemplate>
                                <ItemStyle Font-Bold="false" />
                                <HeaderStyle Width="20%" />
                            </asp:TemplateField>
                        </Columns>
                    </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>   
    <asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
        <ContentTemplate>
            <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
