<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="DoViPham.aspx.cs" Inherits="EOSCRM.Web.Forms.TruyThu.DoViPham" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        var isValid = true;

        function FormatNumber(obj) {
            var strvalue;
            if (eval(obj))
                strvalue = eval(obj).value;
            else
                strvalue = obj;
            var num;
            num = strvalue.toString().replace(/\$|\,/g, '');

            if (isNaN(num))
                num = "";
            sign = (num == (num = Math.abs(num)));
            num = Math.floor(num * 100 + 0.50000000001);
            num = Math.floor(num / 100).toString();
            for (var i = 0; i < Math.floor((num.length - (1 + i)) / 3); i++)
                num = num.substring(0, num.length - (4 * i + 3)) + ',' +
            num.substring(num.length - (4 * i + 3));
            //return (((sign)?'':'-') + num);
            eval(obj).value = (((sign) ? '' : '-') + num);
        }
        
         
         
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

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onKeyDownEventHandler(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId, txtMTRUYTHUId, txtTIENTRUYTHUId, hfGCSId, order, e) {
            // get key code
            if (!isValid)
                return;
            
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }

            // key codes: 
            // tab = 9, right arrow = 39
            // enter = 13, page down = 34, down arrow = 40
            // page up = 33, up arrow = 38 
            // left arrow = 37

            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length != 10) return;
            var oldCsd = idArr[3];
            var oldCsc = idArr[4];
            var oldKltt = idArr[5];
            var oldMtt = idArr[6];
            var oldTtt = idArr[7];
            var tthaighi = 'GDH_BT';
            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msg2 = 'Khối lượng tiêu thụ không khớp với chỉ số. \r\nCó tiếp tục nhập theo dạng khoán không?\r\n\r\nNếu tiếp tục thì bấm /';
            var msg3 = 'Cập nhật chỉ số không thành công';
            
            // enter: validation, save record, move to next row
            if (code == 13) {
                if (order == 1) 
                    FocusAndSelect(txtCHISOCUOIId);                
                else if (order == 2)
                    FocusAndSelect(txtKLTIEUTHUId);
                else if (order == 3)
                    FocusAndSelect(txtMTRUYTHUId);
                else if (order == 4)
                    FocusAndSelect(txtTIENTRUYTHUId);
                else if (order == 5)                    
                {
                    //validation
                    if (isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId, txtMTRUYTHUId, txtTIENTRUYTHUId) == false) {
                        alertWithFocusSelect(msg, txtCHISOCUOIId);
                        return;
                    }

                    var csd = $("#" + txtCHISODAUId).val();
                    var csc = $("#" + txtCHISOCUOIId).val();
                    var kltt = $("#" + txtKLTIEUTHUId).val();
                    var mtt = $("#" + txtMTRUYTHUId).val();
                    var ttt = $("#" + txtTIENTRUYTHUId).val();
                                                                        

                    // save record
                    // UpdateGCS(idkh, namStr, thangStr, csdStr, cscStr, klttStr, ghichu, tthaighi, manv)
                    var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCSTT(idArr[0], idArr[1], idArr[2], csd, csc, kltt, mtt, ttt, idArr[9], tthaighi, idArr[8]);

                    if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                        setControlValue(txtCHISOCUOIId, oldCsc);
                        setControlValue(txtKLTIEUTHUId, oldKltt);
                        setControlValue(txtMTRUYTHUId, oldMtt);
                        setControlValue(txtTIENTRUYTHUId, oldTtt);

                        alertWithFocusSelect(msg3, txtCHISOCUOIId);
                        return;
                    }
                    
                    
                    // update hidden field
                    var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                        idArr[1] + "<%= DELIMITER.Delimiter %>" +
                        idArr[2] + "<%= DELIMITER.Delimiter %>" +
                        csd + "<%= DELIMITER.Delimiter %>" +
                        csc + "<%= DELIMITER.Delimiter %>" +
                        kltt + "<%= DELIMITER.Delimiter %>" +
                        mtt + "<%= DELIMITER.Delimiter %>" +
                        ttt + "<%= DELIMITER.Delimiter %>" +
                        idArr[8] + "<%= DELIMITER.Delimiter %>" +
                        idArr[9];

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
                    var klttId = getPrevId(txtKLTIEUTHUId) + "";
                    if ($("#" + klttId).exists()) {
                        FocusAndSelect(klttId);
                    }
                }
                if (order == 4) {
                    var mttId = getPrevId(txtMTRUYTHUId) + "";
                    if ($("#" + mttId).exists()) {
                        FocusAndSelect(mttId);
                    }
                }
                if (order == 5) {
                    var tttId = getPrevId(txtTIENTRUYTHUId) + "";
                    if ($("#" + tttId).exists()) {
                        FocusAndSelect(tttId);
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
                    var klttId = getNextId(txtKLTIEUTHUId) + "";
                    if ($("#" + klttId).exists()) {
                        FocusAndSelect(klttId);
                    }
                }
                if (order == 4) {
                    var mttId = getNextId(txtMTRUYTHUId) + "";
                    if ($("#" + mttId).exists()) {
                        FocusAndSelect(mttId);
                    }
                }
                if (order == 5) {
                    var tttId = getNextId(txtTIENTRUYTHUId) + "";
                    if ($("#" + tttId).exists()) {
                        FocusAndSelect(tttId);
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

            var nextIndex = parseInt(index, 10) + 1;
            if (nextIndex < 10)
                nextIndex = '0' + nextIndex;

            return shortPrefix + nextIndex + '_' + ctrlName;
        }

        function getPrevId(controlId) {
            var ctrlName = controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
            var prefix = controlId.substring(0, controlId.lastIndexOf('_'));
            var index = prefix.substring(prefix.lastIndexOf('_') + 4, prefix.length);
            var shortPrefix = prefix.substring(0, prefix.lastIndexOf('_')) + '_ctl';

            var nextIndex = parseInt(index, 10) - 1;
            if (nextIndex < 10)
                nextIndex = '0' + nextIndex;

            return shortPrefix + nextIndex + '_' + ctrlName;
        }

        /**
         * check if data in textboxes is valid or not
         */
        function isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId, txtMTRUYTHUId, txtTIENTRUYTHUId) {
            var csd = $("#" + txtCHISODAUId).val();
            var csc = $("#" + txtCHISOCUOIId).val();
            var kltt = $("#" + txtKLTIEUTHUId).val();
            var mtt = $("#" + txtMTRUYTHUId).val();
            var ttt = $("#" + txtTIENTRUYTHUId).val();

            if (!isUnsignedInteger(csd) || !isUnsignedInteger(csc) || !isUnsignedInteger(kltt) || 
                !isUnsignedInteger(mtt) || !isUnsignedInteger(ttt))
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
                                <KLTIEUTHU>__crmdelimiter__
                                <MTRUYTHU>__crmdelimiter__
                                <TIENTRUYTHU>__crmdelimiter__
                                <MANV_CS>__crmdelimiter__
                                <GHICHU_CS>
                                
                                --%>
                                
                                <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("IDKH") + DELIMITER.Delimiter + 
                                        Eval("NAM") + DELIMITER.Delimiter + 
                                        Eval("THANG") + DELIMITER.Delimiter + 
                                        Eval("CHISODAU") + DELIMITER.Delimiter + 
                                        Eval("CHISOCUOI") + DELIMITER.Delimiter + 
                                        Eval("KLTIEUTHU") + DELIMITER.Delimiter + 
                                        Eval("MTRUYTHU") + DELIMITER.Delimiter + 
                                        Eval("TIENTRUYTHU") + DELIMITER.Delimiter + 
                                        LoginInfo.MANV + DELIMITER.Delimiter + 
                                        LoginInfo.NHANVIEN.HOTEN
                                 %>' />
                                 
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="Số nhà" DataField="SONHA" />
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên KH" DataField="TENKH" />                        
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
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Khối lượng">
                            <ItemTemplate>
                                <asp:TextBox ID="txtKLTIEUTHU" Text='<%# Bind("KLTIEUTHU") %>' CssClass='editable' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="M3 Truy thu">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMTRUYTHU" Text='<%# Bind("MTRUYTHU") %>' CssClass='editable' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="90px" HeaderText="Tiền truy thu">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTIENTRUYTHU" Text='<%# Bind("TIENTRUYTHU") %>' CssClass='editable' runat="server"
                                    Width="90" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="90px" HeaderText="Nhân viên" DataField="MANV_CS" />
                    </Columns>    
                </eoscrm:Grid>
            </div> 
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>