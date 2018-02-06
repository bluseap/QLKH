<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="TruyThuVP.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.TruyThuVP" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divKhachHang").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
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
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
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
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showErrorWithFocus('Phải chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
            return false;
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
            return false;
        }

        function CheckFormbtXuatExcel() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btXuatExcel) %>', '');
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
                                                        Danh số
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
                            <td class="crmcell right">Kỳ áp dụng</td>
                            <td class="crmcell"> 
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
                                <div class="left width-150 pleft-50">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC1" AutoPostBack="true" Width="150px" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>                              
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Danh số</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" 
                                        MaxLength="8" Width="90px" TabIndex="2" ReadOnly="True" onchange="CheckSearchKH();"
                                        />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                </div>
                                 <div class="left">
                                    <asp:Button ID="btnNGUYENDC" runat="server" CssClass="pickup"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" OnClick="btnNGUYENDC_Click" Visible="False" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblIDKH" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="reloadm" runat="server" Visible="False"></asp:Label>
                                </div>                               
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lblTENKH" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">    
                                <div class="left width-100">
                                    <asp:Label ID="lblTENDP" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTENKV" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Mục đích sử dụng</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblMAMDSD" runat="server" Text=""></asp:Label>
                                </div>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">CS Mới</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:Label ID="lblCSMOI" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">CS Cũ</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblCSCU" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tiêu thụ</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTIEUTHU" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Thành tiền</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:Label ID="lblTHANHTIEN" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Thuế GTGT</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTHUEGTGT" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tổng tiền</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTONGTIEN" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã số HĐ sai (Ký hiệu)</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:TextBox ID="txtMASOHD" runat="server" Width="120px" MaxLength="20" TabIndex="6" Enabled="False"/>
                                </div>
                                <div class="left">
                                    <div class="right">Không in bảng kê</div>
                                </div>
                                <div class="left">                                    
                                    <asp:CheckBox ID="ckINHDDC" runat="server" TabIndex="28"  
                                         />
                                </div>
                                </div>                             
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">MĐSD</td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:DropDownList ID="ddlMDSD" runat="server" TabIndex="12" Enabled="False" />
                                </div>
                                <div class="left">                                    
                                    <asp:CheckBox ID="ckTINH1GIA" runat="server" TabIndex="28"  Visible="false" 
                                        AutoPostBack="True" />
                                </div>                                  
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Truy thu vi phạm</td>
                            <td class="crmcell">                                
                                <div class="left width-100">
                                    <asp:DropDownList ID="ddlTRUYTHUVP" runat="server" TabIndex="12" >
                                        <asp:ListItem Value="CHETN" Text="Đồng hồ chết" />
                                        <asp:ListItem Value="VIPHAMN" Text="Vi phạm sử dụng nước" />                                   
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Định mức</div>
                                </div>
                                <div class="left width-100">
                                    <asp:TextBox ID="txtSODINHMUC" runat="server" Width="50px" TabIndex="8" />
                                </div>                                                             
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">CS Mới</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:TextBox ID="txtCSMOI" runat="server" Width="50px" MaxLength="10" TabIndex="7" Enabled="False" OnTextChanged="txtCSMOI_TextChanged" />
                                </div>
                                <div class="left">
                                    <div class="right">CS Cũ</div>
                                </div>
                                <div class="left width-100">
                                    <asp:TextBox ID="txtCSCU" runat="server" Width="50px" MaxLength="10" TabIndex="8" Enabled="false"/>
                                </div>       
                                <div class="left">
                                    <div class="right">Truy thu</div>
                                </div>
                                <div class="left width-100">
                                    <asp:TextBox ID="txtMTRUYTHU" runat="server" Width="50px" MaxLength="10" TabIndex="8" />
                                </div>                         
                            </td>
                        </tr>                        
                        <tr>    
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" Rows="3" Width="525" TabIndex="9" 
                                        MaxLength="500" runat="server" Font-Names="Times New Roman" OnTextChanged="txtGhiChu_TextChanged" />
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
                            <td class="crmcell right">  </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSaveUp" runat="server" CssClass="save" OnClick="btnSaveUp_Click" Visible="false"
                                        TabIndex="12" UseSubmitBehavior="false" OnClientClick="return CheckFormSaveUp();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click" 
                                        TabIndex="13" UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="19" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div>                                
                                <div class="left">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btnBaoCao" OnClick="btnBaoCao_Click"  OnClientClick="return CheckFormReport();" 
                                        runat="server" CssClass="report" TabIndex="13" />
                                </div>
                                <div class="left">
                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                    <asp:Button ID="btXuatExcel" OnClientClick="return CheckFormbtXuatExcel();" 
                                        runat="server" CssClass="myButton" Text="Xuất Excel" TabIndex="13" OnClick="btXuatExcel_Click" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btXuatExcel" />           
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlTTDC" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvDieuChinhHD" runat="server" UseCustomPager="True" PageSize="20"
                    OnRowCommand="gvDieuChinhHD_RowCommand" OnPageIndexChanging="gvDieuChinhHD_PageIndexChanging" PagerInforText="" OnSelectedIndexChanged="gvDieuChinhHD_SelectedIndexChanged">
                    <PagerSettings FirstPageText="điều chỉnh hoá đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="12%" HeaderText="Danh số">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("IDKH") %>' 
                                CommandName="SelectDC" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("SODB").ToString()) %>'>
                            </asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên KH" DataField="TENKH" >                        
                            <HeaderStyle Width="40%" />
                        </asp:BoundField>                        
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tr.Thu" DataField="MTRUYTHUVP" >                        
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tiêu thụ" DataField="KLTIEUTHUVP" >                        
                            <HeaderStyle Width="10%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tổng tiền" 
                            DataField="TONGTIENVP" DataFormatString="{0:#.####}" >                        
                            <HeaderStyle Width="30%" />
                        </asp:BoundField>                                                     
                    </Columns>                    
                </eoscrm:Grid> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />  
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                    DisplayGroupTree="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel> 
</asp:Content>
