<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="True" EnableEventValidation="true"
    CodeBehind="CapNhatSoBo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.CapNhatSoBo" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
        });

        function CheckFormFilterDP() {

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');

            return false;
        }

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
            }
        }

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

            return false;
        }

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onKeyDownEventHandler(txtSTTId, txtTENKHId, txtSONHAId, txtMADPGId, txtMADBGId, hfCNSBId, order, e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }

            // key codes: 
            // tab = 9, right arrow = 39
            // enter = 13, page down = 34, down arrow = 40
            // page up = 33, up arrow = 38 
            // left arrow = 37

            var hfvalue = $("#" + hfCNSBId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length != 6) return;
            var oldSTT = idArr[1];
            var oldTENKH = idArr[2];
            var oldSONHA = idArr[3];
            var oldMADPG = idArr[4];
            var oldMADBG = idArr[5];
            var msg = 'Vui lòng nhập số thứ tự hợp lệ.';

            // enter, down arrow keycodes
            if (code == 13 || code == 34 || code == 40) { 
                if (order == 1) {
                    if (isDataValid(txtSTTId) == false) {
                        setControlValue(txtSTTId, oldSTT);
                        showErrorWithFocusSelect(msg, txtSTTId);
                        return;
                    }
                    var sttId = getNextId(txtSTTId) + "";
                    if ($("#" + sttId).exists()) {
                        FocusAndSelect(sttId);
                    }
                }
                else if (order == 2) {
                    var tenkhId = getNextId(txtTENKHId) + "";
                    if ($("#" + tenkhId).exists()) {
                        FocusAndSelect(tenkhId);
                    }
                }
                else if (order == 3) {
                    var sonhaId = getNextId(txtSONHAId) + "";
                    if ($("#" + sonhaId).exists()) {
                        FocusAndSelect(sonhaId);
                    }
                }
                else if (order == 4) {
                    var madpId = getNextId(txtMADPGId) + "";
                    if ($("#" + madpId).exists()) {
                        FocusAndSelect(madpId);
                    }
                }
                else if (order == 5) {
                    var madbId = getNextId(txtMADBGId) + "";
                    if ($("#" + madbId).exists()) {
                        FocusAndSelect(madbId);
                    }
                } 
            }
            // up arrow key code
            else if (code == 33 || code == 38) {    
                if (order == 1) {
                    if (isDataValid(txtSTTId) == false) {
                        setControlValue(txtSTTId, oldSTT);
                        showErrorWithFocusSelect(msg, txtSTTId);
                        return;
                    }
                    var sttId = getPrevId(txtSTTId) + "";
                    if ($("#" + sttId).exists()) {
                        FocusAndSelect(sttId);
                    }
                }
                else if (order == 2) {
                    var tenkhId = getPrevId(txtTENKHId) + "";
                    if ($("#" + tenkhId).exists()) {
                        FocusAndSelect(tenkhId);
                    }
                }
                else if (order == 3) {
                    var sonhaId = getPrevId(txtSONHAId) + "";
                    if ($("#" + sonhaId).exists()) {
                        FocusAndSelect(sonhaId);
                    }
                }
                else if (order == 4) {
                    var madpId = getPrevId(txtMADPGId) + "";
                    if ($("#" + madpId).exists()) {
                        FocusAndSelect(madpId);
                    }
                }
                else if (order == 5) {
                    var madbId = getPrevId(txtMADBGId) + "";
                    if ($("#" + madbId).exists()) {
                        FocusAndSelect(madbId);
                    }
                }
            }
        }


        /**
         * check if data in textboxes is valid or not
         */
        function isDataValid(txtSTTId) {
            var stt = $("#" + txtSTTId).val();

            if (!isUnsignedInteger(stt))
                return false;
                
            return true;
        }

        function onBlurEventHandler(txtSTTId, txtTENKHId, txtSONHAId, txtMADPGId, txtMADBGId, hfCNSBId, manv, order) {

            //TODO: split hidden field to get csc
            var idkh = '';
            var oldSTT = 0;
            var oldTENKH = '';
            var oldSONHA = '';
            var oldMADPG = '';
            var oldMADBG = '';

            var hfvalue = $("#" + hfCNSBId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length == 6) {
                idkh = idArr[0];
                oldSTT = idArr[1];
                oldTENKH = idArr[2];
                oldSONHA = idArr[3];
                oldMADPG = idArr[4];
                oldMADBG = idArr[5];
            }
            else {
                return;
            }
            

            var stt = $("#" + txtSTTId).val();
            var tenkh = $("#" + txtTENKHId).val();
            var sonha = $("#" + txtSONHAId).val();
            var madp = $("#" + txtMADPGId).val();
            var madb = $("#" + txtMADBGId).val();
            var failedMsg = 'Cập nhật khách hàng không thành công.';

            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateSoBo(idkh, oldSTT, stt, tenkh, sonha, madp, madb, manv);

            if (msg.value != "<%= DELIMITER.Passed %>") {
                setControlValue(txtSTTId, oldSTT);
                setControlValue(txtTENKHId, oldTENKH);
                setControlValue(txtSONHAId, oldSONHA);
                setControlValue(txtMADPGId, oldMADPG);
                setControlValue(txtMADBGId, oldMADBG);

                if (order == 1) {
                    showErrorWithFocusSelect(failedMsg, txtSTTId);
                }
                if (order == 2) {
                    showErrorWithFocusSelect(failedMsg, txtTENKHId);
                }
                if (order == 3) {
                    showErrorWithFocusSelect(failedMsg, txtSONHAId);
                }
                if (order == 4) {
                    showErrorWithFocusSelect(failedMsg, txtMADPGId);
                }
                if (order == 5) {
                    showErrorWithFocusSelect(failedMsg, txtMADBGId);
                }

                return;
            }
            
            var val =   idkh + "<%= DELIMITER.Delimiter %>" +
                        stt + "<%= DELIMITER.Delimiter %>" +
                        tenkh + "<%= DELIMITER.Delimiter %>" +
                        sonha + "<%= DELIMITER.Delimiter %>" +
                        madp + "<%= DELIMITER.Delimiter %>" +
                        madb;
                        
            // save to hidden field
            $("#" + hfCNSBId).val(val);

            // if update stt, refresh grid
            if (order == 1 && stt != oldSTT ) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHidden) %>', '');
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
                                                    <asp:TextBox ID="txtKeywordDP" onchange="return CheckFormFilterDP();" runat="server" Width="250px" MaxLength="200" />
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
							        <eoscrm:Grid ID="gvDuongPho" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDuongPho_RowDataBound" OnRowCommand="gvDuongPho_RowCommand" 
							            OnPageIndexChanging="gvDuongPho_PageIndexChanging">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' CommandName="SelectMADP"                                                         
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
    <asp:UpdatePanel ID="upnlGhiChiSo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <a name="top"></a>
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" runat="server" onkeydown="return CheckChangeDP(event);" MaxLength="4" Width="40px" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="30px" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="3" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="4" />
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
            <asp:HiddenField id="hfMADP" runat="server" />
            <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                OnClick="linkBtnHidden_Click" runat="server">Refresh GridView</asp:LinkButton>
            <div class="crmcontainer" id="divList" runat="server" visible="false">
            </div>    
            <eoscrm:Grid ID="gvList" runat="server" 
                OnPageIndexChanging="gvList_PageIndexChanging" 
                OnRowDataBound="gvList_RowDataBound" PageSize="500" UseCustomPager="true">
                <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="60px" HeaderText="Danh bộ">
                        <ItemTemplate>
                            <%# Eval("MADP") + Eval("DUONGPHU").ToString()+ Eval("MADB") %>
                            <asp:HiddenField ID="hfCNSB" runat="server" value='<%# 
                                        Eval("IDKH") + DELIMITER.Delimiter + 
                                        Eval("STT") + DELIMITER.Delimiter + 
                                        Eval("TENKH") + DELIMITER.Delimiter + 
                                        Eval("SONHA") + DELIMITER.Delimiter + 
                                        Eval("MADP") + DELIMITER.Delimiter + 
                                        Eval("MADB") 
                                 %>' />
                        </ItemTemplate>
                        <ItemStyle Font-Bold="true" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="Mã đường">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMADPG" runat="server" Text='<%# Bind("MADP") %>' 
                                Width="70px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="40px" HeaderText="Danh số">
                        <ItemTemplate>
                            <asp:TextBox ID="txtMADBG" runat="server" Text='<%# Bind("MADB") %>' 
                                Width="90px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="60px" HeaderText="Số thứ tự">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSTT" runat="server" Text='<%# Bind("STT") %>' 
                                Width="55px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="555px" HeaderText="Tên khách hàng">
                        <ItemTemplate>
                            <asp:TextBox ID="txtTENKH" runat="server" Text='<%# Bind("TENKH") %>' 
                                Width="550px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Số nhà">
                        <ItemTemplate>
                            <asp:TextBox ID="txtSONHA" runat="server" Text='<%# Bind("SONHA") %>' 
                                Width="80px" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </eoscrm:Grid>
            <br />
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"><a href="#top">Về đầu trang</a></td>
                            <td class="crmcell">    
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
