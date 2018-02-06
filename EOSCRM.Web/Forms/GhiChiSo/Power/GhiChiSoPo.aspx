<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="GhiChiSoPo.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.Power.GhiChiSoPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">   

    <script type="text/javascript">
        var isValid = true;
        $(document).ready(function () {
            $("#divDuongPho").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
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

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormSearch();
            }
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

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onSelectedIndexChangedEventHandler(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId, ddlTTHAIGHIId, hfGCSId, e) {
            var oldCsd = 0;
            var oldCsc = 0;
            var oldTT = 'GDH_BT';
            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length == 11) {
                oldCsc = idArr[4];
                oldTT = idArr[6];
            }

            var tt = $("#" + ddlTTHAIGHIId + " option:selected").val();
            var csd = $("#" + txtCHISODAUId).val();
            var csc = $("#" + txtCHISOCUOIId).val();
            var kltt = $("#" + txtKLTIEUTHUId).val();
            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msgTT = 'Vui lòng nhập chỉ số hợp lệ trước khi chuyển về trạng thái ghi bình thường.';

        }

        function onKeyDownEventHandler(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId, ddlTTHAIGHIId, hfGCSId, order, e) {
            // get key code
            //if (!isValid)
            //    return;

            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }

            // key codes: 
            // tab = 9, right arrow = 39
            // enter = 13, page down = 34, down arrow = 40
            // page up = 33, up arrow = 38 
            // left arrow = 37

            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length != 11) return;
            var oldCsd = idArr[3];
            var oldCsc = idArr[4];
            var oldKltt = idArr[5];
            var tthaighi = $("#" + ddlTTHAIGHIId + " option:selected").val();
            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msg2 = 'Khối lượng tiêu thụ không khớp với chỉ số. \r\nCó tiếp tục nhập theo dạng khoán không?\r\n\r\nNếu tiếp tục thì bấm /';
            var msg3 = 'Chỉ số bất thường. Không thành công';
            var mtt = 0;

            // enter: validation, save record, move to next row
            if (code == 13) {
                if (order == 1) {
                    FocusAndSelect(txtCHISOCUOIId);
                }
                else if (order == 2) {
                    var csd = $("#" + txtCHISODAUId).val();
                    var csc = $("#" + txtCHISOCUOIId).val();
                    if (csc != "g") {
                        if (csc != "i") {
                            if (csc != "d") {
                                if (csc != "n") {
                                    if (csc != "l") {
                                        if (csc != "c") {
                                            if (csc != "h") {
                                                if (csc != "m") {
                                                    if (csc != "t") {

                                                        if (isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId) == false) {
                                                            alertWithFocusSelect(msg, txtCHISOCUOIId);
                                                            return;
                                                        }
                                                        FocusAndSelect(ddlTTHAIGHIId);
                                                    }
                                                    else {
                                                        setControlValue(txtCHISOCUOIId, csd);
                                                        setControlValue(ddlTTHAIGHIId, "THAY");
                                                        FocusAndSelect(ddlTTHAIGHIId);
                                                    }
                                                }
                                                else {
                                                    setControlValue(txtCHISOCUOIId, csd);
                                                    setControlValue(ddlTTHAIGHIId, "M");
                                                    FocusAndSelect(ddlTTHAIGHIId);
                                                }
                                            }
                                            else {
                                                setControlValue(txtCHISOCUOIId, csd);
                                                setControlValue(ddlTTHAIGHIId, "H");
                                                FocusAndSelect(ddlTTHAIGHIId);
                                            }
                                        }
                                        else {
                                            setControlValue(txtCHISOCUOIId, csd);
                                            setControlValue(ddlTTHAIGHIId, "C");
                                            FocusAndSelect(ddlTTHAIGHIId);
                                        }
                                    }
                                    else {
                                        setControlValue(txtCHISOCUOIId, csd);
                                        setControlValue(ddlTTHAIGHIId, "L");
                                        FocusAndSelect(ddlTTHAIGHIId);
                                    }
                                }
                                else {
                                    setControlValue(txtCHISOCUOIId, csd);
                                    setControlValue(ddlTTHAIGHIId, "CUP");
                                    FocusAndSelect(ddlTTHAIGHIId);
                                }
                            }
                            else {
                                setControlValue(txtCHISOCUOIId, csd);
                                setControlValue(ddlTTHAIGHIId, "D");
                                FocusAndSelect(ddlTTHAIGHIId);
                            }
                        }
                        else {
                            setControlValue(txtCHISOCUOIId, csd);
                            setControlValue(ddlTTHAIGHIId, "I");
                            FocusAndSelect(ddlTTHAIGHIId);
                        }
                    }
                    else {
                        setControlValue(txtCHISOCUOIId, csd);
                        setControlValue(ddlTTHAIGHIId, "1");
                        FocusAndSelect(ddlTTHAIGHIId);
                    }
                }
                else if (order == 3) {
                    //if (tthaighi == 'GDH_BT') {
                    if (tthaighi != "Q") {
                        // move to next row
                        var cscId = getNextId(txtCHISOCUOIId) + "";
                        if ($("#" + cscId).exists()) {
                            FocusAndSelect(cscId);
                        }
                    }
                    else {
                        // validation
                        if (isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId) == false) {
                            alertWithFocusSelect(msg, txtCHISOCUOIId);
                            return;
                        }

                        var csd = $("#" + txtCHISODAUId).val();
                        var csc = $("#" + txtCHISOCUOIId).val();
                        var kltt = $("#" + txtKLTIEUTHUId).val();

                        // save record
                        // UpdateGCS(idkh, namStr, thangStr, csdStr, cscStr, klttStr, ghichu, tthaighi, manv)
                        //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCS(idArr[0], idArr[1], idArr[2], csd.toString(), csc.toString(), kltt.toString(), idArr[7], tthaighi, idArr[6]);


                        //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCS(idArr[0], idArr[1], idArr[2], csd.toString(), csc.toString(), mtt.toString(), kltt.toString(), idArr[7], tthaighi, idArr[6]);

                        $.ajax({
                            type: "POST",
                            url: "EOSCRM.Web.Common.AjaxCRM.cs/UpdateGCSPo",
                            data: '{idArr[0], idArr[1], idArr[2], csd.toString(), csc.toString(), mtt.toString(), kltt.toString(), idArr[7], tthaighi, idArr[6]}',
                            contentType: "json"
                        });

                        if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                            setControlValue(txtCHISOCUOIId, oldCsc);
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
                                                    kltt + "<%= DELIMITER.Delimiter %>" +
                                                        idArr[6] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[7] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[8] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[9] + "<%= DELIMITER.Delimiter %>" +
                                                                idArr[10];

                        // save to hidden field
                        $("#" + hfGCSId).val(val);

                        // move to next row
                        var cscId = getNextId(txtCHISOCUOIId) + "";
                        if ($("#" + cscId).exists()) {
                            FocusAndSelect(cscId);
                        }

                    }
                }
                else if (order == 4) {
                    //if (tthaighi == 'GDH_BT') {
                    if (tthaighi != "Q") {
                        
                        var oldCSD_1 = idArr[8];
                        var oldCSC_1 = idArr[9];
                        var oldKltt_1 = idArr[10];

                        //thay nham dh                        
                        if (tthaighi == "TNHAM") {
                            setControlValue(txtCHISODAUId, oldCSC_1);
                            setControlValue(txtCHISOCUOIId, oldCSC_1);
                            tthaighi = "I";
                        }

                        //trang thai thay
                        if (tthaighi == "THAY") {
                            setControlValue(txtCHISODAUId, 0);                            
                        }

                        var csd = $("#" + txtCHISODAUId).val();
                        var csc = $("#" + txtCHISOCUOIId).val();
                        var kltt = csc - csd;

                        // validation
                        if (isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId) == false) {
                            alertWithFocusSelect(msg, txtCHISOCUOIId);
                            return;
                        } 

                        if (kltt < 0 ) {
                            setControlValue(txtCHISOCUOIId, oldCsc);
                            setControlValue(txtKLTIEUTHUId, oldKltt);
                            alertWithFocusSelect(msg3, txtCHISOCUOIId);
                            return;
                        }

                        if (parseInt(oldKltt_1) + 30 <= kltt || parseInt(oldKltt_1) + 100 <= kltt || (parseInt(oldKltt_1) + (parseInt(oldKltt_1) / 8)) <= kltt) {
                            //setControlValue(txtCHISOCUOIId, oldCsc);
                            //setControlValue(txtKLTIEUTHUId, oldKltt);
                            alertWithFocusSelect(msg3, kltt);
                            //return;
                        }

                        
                        //alertWithFocusSelect(msg3 + oldKltt_1, csc);

                        setControlValue(txtKLTIEUTHUId, kltt);

                        // save record
                        // UpdateGCS(idkh, namStr, thangStr, csdStr, cscStr, klttStr, ghichu, tthaighi, manv)


                        var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCSPo(idArr[0], idArr[1], idArr[2], csd.toString(), csc.toString(), mtt.toString(), kltt.toString(), idArr[7], tthaighi, idArr[6]);


                        if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                            setControlValue(txtCHISOCUOIId, oldCsc);
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
                                                    kltt + "<%= DELIMITER.Delimiter %>" +
                                                        idArr[6] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[7] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[8] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[9] + "<%= DELIMITER.Delimiter %>" +
                                                                idArr[10];

                        // save to hidden field
                        $("#" + hfGCSId).val(val);

                        // move to next row
                        var cscId = getNextId(txtCHISOCUOIId) + "";
                        if ($("#" + cscId).exists()) {
                            FocusAndSelect(cscId);
                        }

                    }
                    else {
                        //TODO: khoán, chuyển sang khối lượng tiêu thụ
                        FocusAndSelect(txtKLTIEUTHUId);
                    }
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
}
    // left
else if (code == 39) {
    if (order == 1) {
        var csdId = getNextId(txtCHISODAUId) + "";
        if ($("#" + csdId).exists()) {
            FocusAndSelect(txtCHISOCUOIId);
        }
    }
    if (order == 2) {
        var cscId = getNextId(txtCHISOCUOIId) + "";
        if ($("#" + cscId).exists()) {
            FocusAndSelect(ddlTTHAIGHIId);
        }
    }
}
    // right
else if (code == 37) {
    if (order == 2) {
        var cscId = getNextId(txtCHISOCUOIId) + "";
        if ($("#" + cscId).exists()) {
            FocusAndSelect(txtCHISODAUId);
        }
    }
    if (order == 4) {
        var cscId = getNextId(ddlTTHAIGHIId) + "";
        if ($("#" + cscId).exists()) {
            FocusAndSelect(txtCHISOCUOIId);
        }
    }
}

            // ky tu N
            /*else if (code == 73 || code == 78 || code == 68 || code == 76 || code == 77
             || code == 67 || code == 72 || code == 81 || code == 75) {

                if (order == 4) {
                    // move to next row
                    var csdId = getNextId(txtCHISODAUId) + "";
                    
                    if ($("#" + csdId).exists()) {
                        FocusAndSelect(csdId);
                        var cscId = getNextId(txtCHISOCUOIId) + "";    
                        FocusAndSelect(txtCHISOCUOIId);
                    }
                }
            }*/


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
function isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtKLTIEUTHUId) {
    var csd = $("#" + txtCHISODAUId).val();
    var csc = $("#" + txtCHISOCUOIId).val();
    var kltt = $("#" + txtKLTIEUTHUId).val();

    if (!isUnsignedInteger(csd) || !isUnsignedInteger(csc) || !isUnsignedInteger(kltt))
        return false;

    return true;
}
    </script>
    <style type="text/css">
        .style1
        {
            color: #FF0000;
        }
    </style>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDuongPhoDlgContainer">
        <div id="divDuongPho" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table style="width: 500px;">
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
							        <eoscrm:Grid ID="gvDuongPho" runat="server" OnRowCommand="gvDuongPho_RowCommand" UseCustomPager="true">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADPPO") + "-" + Eval("DUONGPHUPO") %>' 
                                                        CommandName="SelectMADP" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DUONGPHUPO" HeaderStyle-Width="15%" 
                                                HeaderText="Đường phụ" />
                                            <asp:BoundField DataField="TENDP" HeaderStyle-Width="50%" 
                                                HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Khu vực">
                                                <ItemTemplate>
                                                    <%# Eval("KHUVUCPO.TENKV") %>
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
                                <div class="left">
                                    <strong>Khu vực</strong>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" Width="150px" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>                                
                                <div class="left">
                                    <strong>Đường phố</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" runat="server" onkeydown="return CheckChangeDP(event);" MaxLength="4" Width="50px" TabIndex="4" />
                                    <%--onblur="CheckFormSearch();" --%>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="30px" TabIndex="5" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td>
                        </tr>                        
                        <tr style="display: none">    
                            <td class="crmcell right">
                                Mã danh bộ
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADB" runat="server" MaxLength="4" Width="90px" TabIndex="7" />
                                </div>
                                <div class="left width-150 pleft-50">
                                    <div class="right">Tên khách hàng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" MaxLength="50" Width="333px" TabIndex="8" />
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none"> 
                            <td class="crmcell right">
                                Trạng thái
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlTT" Width="102px" runat="server" TabIndex="9">
                                        <asp:ListItem Value="NULL" Text="Tất cả" />
                                        <asp:ListItem Value="GDH_BT" Text="Bình thường" />
                                        <asp:ListItem Value="GDH_KH" Text="Khoán" />
                                        <asp:ListItem Value="CUP" Text="Cúp" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left width-150 pleft-50">
                                    <div class="right">THBT</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHBT" Width="120px" runat="server" TabIndex="10">
                                        <asp:ListItem Value="NULL" Text="Tất cả" />
                                        <asp:ListItem Value="BT" Text="Bình thường" />
                                        <asp:ListItem Value="CG" Text="Chưa ghi" />
                                        <asp:ListItem Value="BATT" Text="Bất thường" />
                                        <asp:ListItem Value="BATT-CG" Text="Bất thường + Chưa ghi" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <strong>Loại khách hàng</strong>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAIKH" Width="100px" runat="server" TabIndex="11">
                                        <asp:ListItem Value="NULL" Text="Tất cả" />
                                        <asp:ListItem Value="TN" Text="Tư nhân" />
                                        <asp:ListItem Value="CQ" Text="Khoán" />
                                    </asp:DropDownList>
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
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" UseSubmitBehavior="false"
                                        OnClick="btnSave_Click" TabIndex="13" OnClientClick="return CheckFormSave();" Visible="false"/>
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="350"  
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="65px" HeaderText="Mã KH">
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
                                        LoginInfo.MANV + DELIMITER.Delimiter + 
                                        LoginInfo.NHANVIEN.HOTEN + DELIMITER.Delimiter + 
                                        Eval("CHISODAU_1") + DELIMITER.Delimiter +
                                        Eval("CHISOCUOI_1") + DELIMITER.Delimiter + 
                                        Eval("KLTIEUTHU_1")
                                 %>' />                                 
                            </ItemTemplate>
                            <HeaderStyle Width="65px" />
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="Số nhà" DataField="SONHA" >
                            <HeaderStyle Width="7%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên KH" DataField="TENKH" >                        
                            <HeaderStyle Width="30%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="CS cũ">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHISODAU" Text='<%# Bind("CHISODAU") %>' 
                                    CssClass='<%# Eval("TTHAIGHI").ToString() != "Q" ? "readonly" : "editable" %>'
                                    ReadOnly='<%# Eval("TTHAIGHI").ToString() == "Q" ? false : true %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="CS mới">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHISOCUOI" CssClass="editable" Text='<%# Bind("CHISOCUOI") %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="70px" />
                        </asp:TemplateField>
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
                                    <asp:ListItem Value="Q" Text="Q_Điều chỉnh - Qua số" />
                                    <asp:ListItem Value="1" Text="Ghi 2 tháng" />
                                    <asp:ListItem Value="GDH_KH" Text="Khoán" />
                                    <asp:ListItem Value="K" Text="K_Khác" />
                                    <asp:ListItem Value="NV" Text="V_Ngưng do vi phạm" />
                                    <asp:ListItem Value="A" Text="A_KH cung cấp" /> 
                                    <asp:ListItem Value="M" Text="M_Bỏ địa phương" />
                                    <asp:ListItem Value="U" Text="U_Tạm tính" />
                                    <asp:ListItem Value="R" Text="R_Đồng hồ mờ" />
                                    <asp:ListItem Value="X" Text="X_Bể ống sau ĐH" />        
                                    <asp:ListItem Value="TNHAM" Text="Thay nhầm" />
                                    <asp:ListItem Value="S" Text="S_Điều chỉnh chỉ số cũ" />
                                    <asp:ListItem Value="V" Text="V_Nghi ngờ" />                                
                                    <asp:ListItem Value="Z" Text="Z_Đồng hồ nghiêng" />
                                    <asp:ListItem Value="O" Text="O_Không chì" />
                                    <asp:ListItem Value="P" Text="P_Đứt chì" />

                                    <asp:ListItem Value="E" Text="E_Đề nghị xóa bộ" />
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="115px" />
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Khối lượng">
                            <ItemTemplate>
                                <asp:TextBox ID="txtKLTIEUTHU" Text='<%# Bind("KLTIEUTHU") %>' 
                                    CssClass='<%# Eval("TTHAIGHI").ToString() != "Q" ? "readonly" : "editable" %>'
                                    ReadOnly='<%# Eval("TTHAIGHI").ToString().Equals("Q") ? false : true %>' runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="70px" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="100px" HeaderText="Nhân viên" 
                            DataField="MANV_CS" >
                            <HeaderStyle Width="100px" />
                        </asp:BoundField>
                    </Columns>    
                </eoscrm:Grid>
            </div>      
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