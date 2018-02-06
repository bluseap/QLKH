<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="True" EnableEventValidation="true"
    CodeBehind="CapNhatDoan.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.CapNhatDoan" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>

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

        function onKeyDownEventHandler(txtMADOANId, hfCNSBId, e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; };

            // key codes: 
            // tab = 9, right arrow = 39
            // enter = 13, page down = 34, down arrow = 40
            // page up = 33, up arrow = 38 
            // left arrow = 37

            var hfvalue = $("#" + hfCNSBId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length != 2) return;

            // enter, down arrow keycodes
            if (code == 13 || code == 34 || code == 40) {
                var mdId = getNextId(txtMADOANId) + "";
                if ($("#" + mdId).exists()) {
                    FocusAndSelect(mdId);
                }
            }
            // up arrow key code
            else if (code == 33 || code == 38) {
                var sttId = getPrevId(txtMADOANId) + "";
                if ($("#" + sttId).exists()) {
                    FocusAndSelect(sttId);
                }
            }
        }

        function onBlurEventHandler(txtMADOANId, hfCNSBId, manv) {

            //TODO: split hidden field to get csc
            var idkh = '';
            var oldMADOAN = '';

            var hfvalue = $("#" + hfCNSBId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length == 2) {
                idkh = idArr[0];
                oldMADOAN = idArr[1];
            }
            else {
                return;
            }


            var md = $("#" + txtMADOANId).val();

            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateMaDoan(idkh, md, manv);

            if (msg.value != "<%= DELIMITER.Passed %>") {
                setControlValue(txtMADOANId, oldMADOAN);
                showErrorWithFocusSelect(msg.value, txtMADOANId);

                return;
            }
            
            var val =   idkh + "<%= DELIMITER.Delimiter %>" +md;
                        
            // save to hidden field
            $("#" + hfCNSBId).val(val);
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
                <eoscrm:Grid ID="gvList" runat="server" PageSize="500" UseCustomPager="true" 
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Danh bộ">
                            <ItemTemplate>
                                <%# Eval("MADP") + Eval("DUONGPHU").ToString()+ Eval("MADB") %>
                                <asp:HiddenField id="hfCNSB" runat="server" value='<%# 
                                        Eval("IDKH") + DELIMITER.Delimiter + 
                                        Eval("MADOAN")
                                 %>' />
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="60px" DataField="STT" HeaderText="Số thứ tự" />
                        <asp:BoundField HeaderStyle-Width="500px" DataField="TENKH" HeaderText="Tên khách hàng" />
                        <asp:BoundField HeaderStyle-Width="200px" DataField="SONHA" HeaderText="Số nhà" />
                        <asp:TemplateField HeaderStyle-Width="210px" HeaderText="Mã đoạn">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMADOAN" Text='<%# Bind("MADOAN") %>' Width="205px" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>  
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
