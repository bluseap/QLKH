<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="SuaChiSo.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.SuaChiSo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
    
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    
    <script type="text/javascript">
        var isValid = true;        
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
                return CheckFormSearch();
            }
        }

        function CheckFormSearch() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }
            
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');

            return false;
        }

        function CheckFormSearchNguyen() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSNGUYEN) %>', '');

            return false;
        } 

        function CheckFormSearchbtnChuyenNguyen() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnChuyenNguyen) %>', '');

            return false;
        }

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onKeyDownEventHandler(txtCHISODAUId, txtCHISOCUOIId, txtMTRUYTHUId, txtKLTIEUTHUId, ddlTTHAIGHIId, hfGCSId, order, e) {
            // get key code
            //if (!isValid)
            //    return;
            
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }

            // key codes: 
            // tab = 9, right arrow = 39
            // enter = 13, page down = 34, down arrow = 40
            // page up = 33, up arrow = 38 
            // left arrow = 37

            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length != 9) return;
            var oldCsd = idArr[3];
            var oldCsc = idArr[4];
            var oldMtt = idArr[5];
            var oldKltt = idArr[6];
            //var tthaighi = 'GDH_BT';
            var tthaighi = $("#" + ddlTTHAIGHIId + " option:selected").val();
            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msg2 = 'Khối lượng tiêu thụ không khớp với chỉ số. \r\nCó tiếp tục nhập theo dạng khoán không?\r\n\r\nNếu tiếp tục thì bấm /';
            var msg3 = 'Cập nhật chỉ số không thành công';
            
            // enter: validation, save record, move to next row
            if (code == 13) {
                if (order == 1)
                    FocusAndSelect(txtCHISOCUOIId);

                else if (order == 2)
                    FocusAndSelect(txtMTRUYTHUId);
                
                else if (order == 3) {
                    

                    


                    var csd = $("#" + txtCHISODAUId).val();
                    var csc = $("#" + txtCHISOCUOIId).val();
                    var mtt = $("#" + txtMTRUYTHUId).val();
                    var kltt = $("#" + txtKLTIEUTHUId).val();

                    kltt = (Number(csc) + Number(mtt)) - Number(csd);
                    setControlValue(txtKLTIEUTHUId, kltt);
                    
                    FocusAndSelect(txtKLTIEUTHUId);
                }

                else if (order == 4) {
                    // validation
                    if (isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtMTRUYTHUId, txtKLTIEUTHUId) == false) {
                        alertWithFocusSelect(msg, txtCHISOCUOIId);
                        return;
                    }

                    var csd = $("#" + txtCHISODAUId).val();
                    var csc = $("#" + txtCHISOCUOIId).val();
                    var mtt = $("#" + txtMTRUYTHUId).val();
                    var kltt = $("#" + txtKLTIEUTHUId).val();

                    /*if (kltt != csc + mtt - csd) {
                        //tthaighi = 'Q';
                        var ret = prompt(msg2, "");
                        if (ret != '/' && ret != '/') {
                            FocusAndSelect(txtCHISOCUOIId);
                            return;
                        }
                    }
                    */
                    
                    // save record
                    // UpdateGCS(idkh, namStr, thangStr, csdStr, cscStr, klttStr, ghichu, tthaighi, manv)
                    //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCS(idArr[0], idArr[1], idArr[2], csd, csc, mtt, kltt, idArr[8], tthaighi, idArr[7]);
                    var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCS2(idArr[0], idArr[1], idArr[2], csd, csc, mtt, kltt, idArr[8], tthaighi, idArr[7]);
                    //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCSTT(idArr[0], idArr[1], idArr[2], csd, csc, mtt, kltt, idArr[8], tthaighi, idArr[7]);

                    if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                        setControlValue(txtCHISOCUOIId, oldCsc);
                        setControlValue(txtMTRUYTHUId, oldMtt);
                        setControlValue(txtKLTIEUTHUId, oldKltt);

                        alertWithFocusSelect(msg3, txtCHISOCUOIId);
                        return;
                    }

                    // update hidden field
                    var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                        idArr[1] + "<%= DELIMITER.Delimiter %>" +
                        idArr[2] + "<%= DELIMITER.Delimiter %>" +
                        csd + "<%= DELIMITER.Delimiter %>" +
                        csc + "<%= DELIMITER.Delimiter %>" +
                        mtt + "<%= DELIMITER.Delimiter %>" +
                        kltt + "<%= DELIMITER.Delimiter %>" +
                        idArr[7] + "<%= DELIMITER.Delimiter %>" +
                        idArr[8];

                    // save to hidden field
                    $("#" + hfGCSId).val(val);

                    // move to next row
                    var cscId = getNextId(txtCHISOCUOIId) + "";
                    if ($("#" + cscId).exists()) {
                        FocusAndSelect(cscId);
                    }

                    alertWithFocusSelect("Cập nhật chỉ số thành công.", txtCHISODAUId);
                }
            }
            
            // move up, page up
            else if (code == 33 || code == 38) {
                if (order == 1) {
                    var csdId = getPrevId(txtCHISODAUId) + "";
                    if ($("#" + csdId).exists()) {
                        FocusAndSelect(csdId);
                    }
                }
                if (order == 2) {
                    var cscId = getPrevId(txtCHISOCUOIId) + "";
                    if ($("#" + cscId).exists()) {
                        FocusAndSelect(cscId);
                    }
                }
                if (order == 3) {
                    var mttId = getPrevId(txtMTRUYTHUId) + "";
                    if ($("#" + mttId).exists()) {
                        FocusAndSelect(mttId);
                    }
                }
                if (order == 4) {
                    var klttId = getPrevId(txtKLTIEUTHUId) + "";
                    if ($("#" + klttId).exists()) {
                        FocusAndSelect(klttId);
                    }
                }
            }

            // move down, page down
            else if (code == 34 || code == 40) {
                if (order == 1) {
                    var csdId = getNextId(txtCHISODAUId) + "";
                    if ($("#" + csdId).exists()) {
                        FocusAndSelect(csdId);
                    }
                }
                if (order == 2) {
                    var cscId = getNextId(txtCHISOCUOIId) + "";
                    if ($("#" + cscId).exists()) {
                        FocusAndSelect(cscId);
                    }
                }
                if (order == 3) {
                    var mttId = getNextId(txtMTRUYTHUId) + "";
                    if ($("#" + mttId).exists()) {
                        FocusAndSelect(mttId);
                    }
                }
                if (order == 4) {
                    var klttId = getNextId(txtKLTIEUTHUId) + "";
                    if ($("#" + klttId).exists()) {
                        FocusAndSelect(klttId);
                    }
                }
            }
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

        /**
         * check if data in textboxes is valid or not
         */
        function isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtMTRUYTHUId, txtKLTIEUTHUId) {
            var csd = $("#" + txtCHISODAUId).val();
            var csc = $("#" + txtCHISOCUOIId).val();
            var mtt = $("#" + txtMTRUYTHUId).val();
            var kltt = $("#" + txtKLTIEUTHUId).val();

            if (!isUnsignedInteger(csd) || !isUnsignedInteger(csc) || !isUnsignedInteger(mtt) || !isUnsignedInteger(kltt))
                return false;
                
            return true;
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
    <asp:UpdatePanel ID="upnlGhiChiSo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">
                                Kỳ khai thác
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANG" runat="server" TabIndex="1" Enabled="False" 
                                        onselectedindexchanged="ddlTHANG_SelectedIndexChanged">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" Enabled="False" />
                                </div>
                                <div class="left">
                                    <strong>Khu vực</strong>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC1" AutoPostBack="true" Width="150px" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>  
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">
                                Số danh bộ
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" onkeydown="return CheckChangeKH(event);" MaxLength="8" Width="90px" TabIndex="7" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnNGUYEN" runat="server" CssClass="pickup" 
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" OnClick="btnNGUYEN_Click" Visible="False" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnSNGUYEN" 
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSearchNguyen();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" OnClick="btnSNGUYEN_Click" Visible="False" />
                                </div> 
                                <div class="left">
                                    <asp:Button ID="btnChuyenNguyen" 
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSearchbtnChuyenNguyen();" 
                                        runat="server" Text="chuyen KH moi" TabIndex="12" Visible="False" OnClick="btnChuyenNguyen_Click" />
                                </div> 
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div class="crmcontainer" id="divList" runat="server" visible="false">
                <eoscrm:Grid ID="gvList" runat="server" PageSize="100" UseCustomPager="true" 
                    OnRowDataBound="gvList_RowDataBound">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Mã KH">
                            <ItemTemplate>
                                <%# Eval("SODB")%>
                                <%--
                                
                                cau truc du lieu trong hidden field:
                                
                                <IDKH>__crmdelimiter__
                                <NAM>__crmdelimiter__
                                <THANG>__crmdelimiter__
                                <CHISODAU>__crmdelimiter__
                                <CHISOCUOI>__crmdelimiter__
                                <MTRUYTHU>__crmdelimiter__
                                <KLTIEUTHU>__crmdelimiter__
                                <MANV_CS>__crmdelimiter__
                                <GHICHU_CS>
                                
                                --%>
                                
                                <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("IDKH") + DELIMITER.Delimiter + 
                                        Eval("NAM") + DELIMITER.Delimiter + 
                                        Eval("THANG") + DELIMITER.Delimiter + 
                                        Eval("CHISODAU") + DELIMITER.Delimiter + 
                                        Eval("CHISOCUOI") + DELIMITER.Delimiter + 
                                        Eval("MTRUYTHU") + DELIMITER.Delimiter + 
                                        Eval("KLTIEUTHU") + DELIMITER.Delimiter + 
                                        LoginInfo.MANV + DELIMITER.Delimiter + 
                                        LoginInfo.NHANVIEN.HOTEN 
                                 %>' />
                                 
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="Số nhà" DataField="SONHA" />
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên KH" DataField="TENKH" />
                        <asp:TemplateField HeaderStyle-Width="115px" HeaderText="Trạng thái ghi">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlTTHAIGHI" Width="110px" SelectedValue='<%# Bind("TTHAIGHI") %>' runat="server">
                                    <asp:ListItem Value="GDH_BT" Text="Bình thường" />                                    
                                    <asp:ListItem Value="I" Text="I_Ít sử dụng" />
                                    <asp:ListItem Value="THAY" Text="Thay ĐH" />
                                    <asp:ListItem Value="CUP" Text="N_Cúp" />
                                    <asp:ListItem Value="D" Text="D_Đóng cửa" />                                    
                                    <asp:ListItem Value="L" Text="L_Lố số" />
                                    <asp:ListItem Value="M" Text="M_Bỏ địa phương" />
                                    <asp:ListItem Value="C" Text="C_Cháy - Chết" />
                                    <asp:ListItem Value="H" Text="H_Hư hỏng" />
                                    <asp:ListItem Value="B" Text="Bất thường" />
                                    <asp:ListItem Value="Q" Text="Q_Qua số" />                                    
                                    <asp:ListItem Value="GDH_KH" Text="Khoán" />
                                    <asp:ListItem Value="K" Text="K_Khác" />
                                    <asp:ListItem Value="NV" Text="V_Ngưng do vi phạm" />
                                    
                                    <asp:ListItem Value="A" Text="A_KH cung cấp" />
                                    <asp:ListItem Value="X" Text="X_Bể ống sau ĐH" />
                                    <asp:ListItem Value="R" Text="R_Đồng hồ mờ" />
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>                          
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="CS cũ">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHISODAU" Text='<%# Bind("CHISODAU") %>' CssClass='editable' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="CS mới">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHISOCUOI" CssClass="editable" Text='<%# Bind("CHISOCUOI") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="M3 truy thu">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMTRUYTHU" CssClass="editable" Text='<%# Bind("MTRUYTHU") %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Khối lượng">
                            <ItemTemplate>
                                <asp:TextBox ID="txtKLTIEUTHU" Text='<%# Bind("KLTIEUTHU") %>' CssClass='editable' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="100px" HeaderText="Nhân viên" DataField="MANV_CS" />
                    </Columns>    
                </eoscrm:Grid>
            </div> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
