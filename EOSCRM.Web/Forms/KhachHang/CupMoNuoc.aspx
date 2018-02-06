<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.KhachHang.CupMoNuoc" CodeBehind="CupMoNuoc.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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

        function CheckSearch() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');

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
    <asp:UpdatePanel ID="upnlThongTin" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" OnTextChanged="txtSODB_TextChanged" onchange="CheckSearchKH();" MaxLength="8" Width="90px" TabIndex="7" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblIDKH" runat="server" Text="" Visible="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lblTENKH" runat="server" Text="" ></asp:Label>
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
                            <td class="crmcell right">Trạng thái</td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlTTSD" runat="server">
                                        <asp:ListItem Text="Cúp" Value="CUP" />
                                        <asp:ListItem Text="Mở" Value="" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
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
                                </div>
                            </td>
                        </tr>  
                        <tr>    
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell">
                                <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" Rows="3" Width="525" MaxLength="500" runat="server" />
                            </td>
                        </tr>  
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click" Visible="false"
                                        TabIndex="13" OnClientClick="return CheckFormSave();" UseSubmitBehavior="false" />
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
                <eoscrm:Grid ID="gvKhachHang" runat="server" UseCustomPager="true" PageSize=20
                    OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging"
                    OnRowDataBound="gvKhachHang_RowDataBound">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Mã KH">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnID" runat="server" 
                                CommandArgument='<%# Eval("IDKH") + "-" + Eval("ID") %>' CommandName="SelectKH" 
                                Text='<%# Eval("KHACHHANG.MADP") + Eval("KHACHHANG.DUONGPHU").ToString() + Eval("KHACHHANG.MADB") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.TENKH")%>
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderStyle-Width="40%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.SONHA") != null && Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : "" %>
                                <%# Eval("KHACHHANG.DUONGPHO.TENDP")%>, <%# Eval("KHACHHANG.PHUONG.TENPHUONG")%>, <%# Eval("KHACHHANG.KHUVUC.TENKV")%>
                            </ItemTemplate>
                        </asp:TemplateField>   
                        <asp:TemplateField HeaderStyle-Width="7%" HeaderText="Trạng thái">
                            <ItemTemplate>
                                <%# (Eval("TTSD").ToString() == "CUP") ? "Cúp nước" : "Mở nước" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="18%" HeaderText="Ngày nhập">
                            <ItemTemplate>
                                <%# (Eval("NGAYTHAYDOI") != null) ?
                                        String.Format("{0:dd/MM/yy H:mm:ss}", Eval("NGAYTHAYDOI"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
