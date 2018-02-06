<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.SuaChua.NhapThongTinSuaChua" CodeBehind="NhapThongTinSuaChua.aspx.cs" %>

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
        });

        function CheckFormFilterKH() {
            var idkh = jQuery.trim($("#<%= txtIDKH.ClientID %>").val());
            var tenkh = jQuery.trim($("#<%= txtTENKH.ClientID %>").val());
            var madh = jQuery.trim($("#<%= txtMADH.ClientID %>").val());
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

        function CheckChangeMAKH(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHidden) %>', '');
            }
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

            return false;
        }

        function CheckFormFitler() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

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
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" TabIndex="8" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMAKH" onkeypress="return CheckChangeMAKH(event);" runat="server" Width="100px" MaxLength="300" TabIndex="1" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MAKH</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                </div>
                            </td>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="300px" MaxLength="300" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Số điện thoại</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSDT" runat="server" Width="100px" MaxLength="15" />
                                </div>
                            </td>
                            <td class="crmcell right">Thông tin sơ bộ</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTHONGTINKH" runat="server" Width="300px" MaxLength="300" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nội dung báo</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="cboMAPH" runat="server" />
                                </div>
                            </td>
                            <td class="crmcell right">Ngày báo</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYBAO" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
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
                            <td class="crmcell right btop"></td>
                            <td class="crmcell btop" colspan="3">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" TabIndex="17" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                         OnClientClick="return CheckFormCancel();" TabIndex="18" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="19" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell" colspan="3">
                                <div class="left"><strong>Ngày bắt đầu</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYBD" runat="server" Width="90px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYBD" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtNGAYBD"
                                        PopupButtonID="imgNGAYBD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left"><strong>Ngày kết thúc</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKT" runat="server" Width="90px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYKT" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtNGAYKT"
                                        PopupButtonID="imgNGAYKT" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                
                                <div class="left">
                                    <asp:Button ID="btnFilter" runat="server" CommandArgument="Insert" CssClass="filter"
                                        OnClick="btnFilter_Click" TabIndex="16" OnClientClick="return CheckFormFilter();" UseSubmitBehavior="false" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_OnRowDataBound" 
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADON") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADON") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Mã đơn">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADON") %>'></asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                            <FooterTemplate>
                                <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                    <strong>Bỏ chọn hết</strong></a>
                            </FooterTemplate>
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
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Ngày báo">
                            <ItemTemplate>
                                <%# (Eval("NGAYBAO") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYBAO"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
               </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
