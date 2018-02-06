<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="NhapKHMoiPoTS.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.NhapKHMoiPoTS" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Import Namespace="EOSCRM.Util" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onSelectedIndexChangedEventHandler(txtCHISODAUId, txtCHISOCUOIId, hfGCSId, e) {

            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");

            var oldCHISODAU = idArr[4];
            var oldCHISOCUOI = idArr[5];

            var csdau = $("#" + txtCHISODAUId).val();
            var cscuoi = $("#" + txtCHISOCUOIId).val();

            var msg = 'Vui 022 lòng nhập chỉ số hợp lệ.';
            var msgTT = 'Vui lòng nhập chỉ số hợp lệ trước khi chuyển về trạng thái ghi bình thường.';

            //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateNhanVienVayKy(idArr[0], idArr[1], idArr[2], tiengoc.toString(), tienlai.toString(), httt.toString(), thanhtoan.toString());

            //$.ajax({
            //    type: "POST",
            //    url: "EOSCRM.Web.Common.AjaxCRM.cs/UpdateNhanVienVayKy",
            //    data: '{idArr[0], idArr[1], idArr[2],tiengoc.toString(), tienlai.toString(), httt.toString(), thanhtoan.toString()}',
            //    contentType: "json"
            //});

            if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                setControlValue(txtCHISODAUId, oldCHISODAU);
                setControlValue(txtCHISOCUOIId, oldCHISOCUOI);

                alertWithFocusSelect(msg, oldCHISOCUOI);
                return;
            }

            // update hidden field
            var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                      idArr[1] + "<%= DELIMITER.Delimiter %>" +
                      idArr[2] + "<%= DELIMITER.Delimiter %>" +
                      idArr[3] + "<%= DELIMITER.Delimiter %>" +
                      csdau + "<%= DELIMITER.Delimiter %>" +
                      cscuoi + "<%= DELIMITER.Delimiter %>" +
                      idArr[6] + "<%= DELIMITER.Delimiter %>" +
                      idArr[7] + "<%= DELIMITER.Delimiter %>" +
                      idArr[8];

            alertWithFocusSelect("Cập nhật 01 chỉ số thành công.", txtCHISOCUOIId);

            // save to hidden field
            $("#" + hfGCSId).val(val);

        }

        function onKeyDownEventHandler(txtCHISODAUId, txtCHISOCUOIId, hfGCSId, order, e) {
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
            if (idArr.length != 9) return;

            var oldCHISODAU = idArr[4];
            var oldCHISOCUOI = idArr[5];

            var csdau = $("#" + txtCHISODAUId).val();
            var cscuoi = $("#" + txtCHISOCUOIId).val();

            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msg2 = 'Khối lượng tiêu thụ không khớp với chỉ số. \r\nCó tiếp tục nhập theo dạng khoán không?\r\n\r\nNếu tiếp tục thì bấm /';
            var msg3 = 'Cập nhật 03 chỉ số không thành công';

            // enter: validation, save record, move to next row
            if (code == 13) {
                if (order == 1) {
                    FocusAndSelect(txtCHISOCUOIId);
                    //alertWithFocusSelect(msg3, oldCHISOCUOI);
                }

                else if (order == 2) {

                    //alertWithFocusSelect(msg3, oldCHISOCUOI);
                    //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCS2(idArr[0], idArr[1], idArr[2], csd, csc, mtt, kltt, idArr[8], tthaighi, idArr[7]);
                    var savingMsg = EOSCRM.Web.Common.AjaxCRM.InKhachHangMoi(idArr[0], idArr[3], csdau, cscuoi, idArr[8], idArr[6], idArr[7]);

                    if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                        setControlValue(txtCHISODAUId, oldCHISODAU);
                        setControlValue(txtCHISOCUOIId, oldCHISOCUOI);

                        alertWithFocusSelect(idArr[0] + "," + idArr[1] + "," + idArr[2] + "," + idArr[3] + "," + idArr[4]
                                            + "," + idArr[5], + "," + idArr[6] + "," + idArr[7] + "," + idArr[8] + ","
                                            + csdau + "," + cscuoi//msg3
                            , oldCHISOCUOI);

                        alertWithFocusSelect(idArr[6] + "," + idArr[7] + "," + idArr[8] + ","
                                            + csdau + "," + cscuoi//msg3
                            , oldCHISOCUOI);
                        return;
                    }


                    // update hidden field
                    var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                                idArr[1] + "<%= DELIMITER.Delimiter %>" +
                                    idArr[2] + "<%= DELIMITER.Delimiter %>" +
                                        idArr[3] + "<%= DELIMITER.Delimiter %>" +
                                            csdau + "<%= DELIMITER.Delimiter %>" +
                                                cscuoi + "<%= DELIMITER.Delimiter %>" +
                                                    idArr[6] + "<%= DELIMITER.Delimiter %>" +
                                                        idArr[7] + "<%= DELIMITER.Delimiter %>" +
                                                            idArr[8];

                    // save to hidden field
                    $("#" + hfGCSId).val(val);

                    // move to next row
                    var cscId = getNextId(txtCHISOCUOIId) + "";
                    if ($("#" + cscId).exists()) {
                        FocusAndSelect(cscId);
                    }
                    //alertWithFocusSelect("Cập nhật kỳ vay thành công.", txtCHISOCUOIId);
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
                    FocusAndSelect(txtCHISODAUId);
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

    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btLOCDSKHCBKT" runat="server" CssClass="filter" 
                                        CausesValidation="false" UseSubmitBehavior="false" TabIndex="44" OnClick="btLOCDSKHCBKT_Click" />
                                </div>   
                            </td>                                                   
                        </tr>       
                    </tbody>
                </table>
            </div> 
            <br />
            <div class="crmcontainer">
                <eoscrm:Grid 
                    ID="gvKhachHang" runat="server" UseCustomPager="true" PageSize="300"
                        OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging"
                        OnRowDataBound="gvKhachHang_RowDataBound">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox"  HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Danh số">
                            <ItemTemplate>
                            <asp:LinkButton ID="linkMaKHM" runat="server" 
                                CommandArgument='<%# Eval("IDKH") %>'
                                CommandName="SelectHD"  OnClientClick="openWaitingDialog();unblockWaitingDialog();" 
                                Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString() + Eval("MADB")) %>'></asp:LinkButton>
                            
                            
                                <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("IDKH") + DELIMITER.Delimiter + 
                                        Eval("MADP") + DELIMITER.Delimiter + 
                                        Eval("MADB") + DELIMITER.Delimiter + 
                                        Eval("MAKV") + DELIMITER.Delimiter + 
                                        Eval("CHISODAU") + DELIMITER.Delimiter +
                                        Eval("CHISOCUOI") + DELIMITER.Delimiter +
                                        Eval("THANG") + DELIMITER.Delimiter +
                                        Eval("NAM") + DELIMITER.Delimiter +
                                        Eval("MAKV")
                                 %>' />     
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                                        
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên khách hàng" DataField="TENKH" />
                        <asp:TemplateField HeaderStyle-Width="30px" HeaderText="CS Bắt đầu">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHISODAUKHM" Text='<%# Bind("CHISODAU") %>' 
                                    CssClass='<%# "readonly"  %>'
                                    ReadOnly='<%# true %>' 
                                    runat="server" />                                    
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" HeaderText="C.Số cuối">
                            <ItemTemplate>
                                <asp:TextBox ID="txtCHISOCUOIKHM" Text='<%# Bind("CHISOCUOI") %>' 
                                    runat="server" />                                    
                            </ItemTemplate>
                            <HeaderStyle Width="30px" />
                        </asp:TemplateField>

                    </Columns>
                </eoscrm:Grid> 
            </div>                     
        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>
