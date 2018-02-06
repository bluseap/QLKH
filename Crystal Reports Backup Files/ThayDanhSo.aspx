<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="ThayDanhSo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.ThayDanhSo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
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
        
        function CheckFormSave() {

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

            return false;
        }

        function CheckChangeDP(e) {
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
        
        function onKeyDownEventHandler(txtMADPGId, txtMADBGId, hfGCSId, order, e) 
        {
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
            
            if (idArr.length != 6) return;
            var oldMadp = idArr[3];
            var oldMadb = idArr[4];
           
            var msg = 'Vui lòng nhập danh bộ hợp lý.';      

            // enter: validation, save record, move to next row
            if (code == 13) 
            {
                if (order == 1)
                    FocusAndSelect(txtMADBGId);
                else if (order == 2) 
                {
                    // validation
                    if (isDataValid(txtMADPGId,txtMADBGId) == false) 
                    {
                        alertWithFocusSelect(msg, txtMADBGId);
                        return;
                    }
                    
                    var mdp = $("#" + txtMADPGId).val();
                    var mdb = $("#" + txtMADBGId).val();
                    
                    var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateDB(idArr[0], idArr[1], idArr[2], mdp.toString(), mdb.toString(), idArr[5]);

                    if (savingMsg.value != "<%= DELIMITER.Passed %>") 
                    {
                        setControlValue(txtMADPGId, oldMadp);      
                        setControlValue(txtMADBGId, oldMadb);                   
                        alertWithFocusSelect(msg, txtMADBGId);
                        return;
                    }

                    // update hidden field
                    var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                              idArr[1] + "<%= DELIMITER.Delimiter %>" +
                              idArr[2] + "<%= DELIMITER.Delimiter %>" +
                              mdp      + "<%= DELIMITER.Delimiter %>" +
                              sdb      + "<%= DELIMITER.Delimiter %>" +
                              idArr[5]; ;

                    // save to hidden field
                    $("#" + hfGCSId).val(val);

                    // move to next row
                    var mdbId = getNextId(txtMADBGId) + "";
                    if ($("#" + mdbId).exists()) 
                    {
                        FocusAndSelect(mdbId);
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
        function isDataValid(txtMADPGId, txtMADBGId) {
            var mdp = $("#" + txtMADPGId).val();
            var mdb = $("#" + txtMADBGId).val();
            if (!isUnsignedInteger(mdp) || !isUnsignedInteger(mdb))
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
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' 
                                                        CommandName="SelectMADP" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DUONGPHU" HeaderStyle-Width="15%" 
                                                HeaderText="Đường phụ" />
                                            <asp:BoundField DataField="TENDP" HeaderStyle-Width="50%" 
                                                HeaderText="Tên đường phố" />
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
    
    <asp:UpdatePanel ID="upnlThayDanhSo" UpdateMode="Conditional" runat="server">
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
                                <div class="left width-150 pleft-50">
                                    <div class="right">Khu vực</div>
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
                <eoscrm:Grid ID="gvList" runat="server" PageSize="300" UseCustomPager="true"  CssClass="crmgrid"
                    AutoGenerateColumns="false"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altrow" />
                    <HeaderStyle CssClass="header" />
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />                    
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên KH" DataField="TENKH" />
                        <asp:TemplateField HeaderStyle-Width="55px" HeaderText="Danh số cũ">
                            <ItemTemplate>
                                <%# Eval("SODB")%>
                                <%--
                                
                                cau truc du lieu trong hidden field:
                                
                                <IDKH>__crmdelimiter__
                                <NAM>__crmdelimiter__
                                <THANG>__crmdelimiter__
                                <MADP>__crmdelimiter__
                                <MADB>__crmdelimiter__
                                <GHICHU_CS>
                                --%>
                                
                                <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("IDKH")  + DELIMITER.Delimiter + 
                                        Eval("NAM")   + DELIMITER.Delimiter + 
                                        Eval("THANG") + DELIMITER.Delimiter + 
                                        Eval("MADP")  + DELIMITER.Delimiter + 
                                        Eval("MADB") + DELIMITER.Delimiter + 
                                        LoginInfo.NHANVIEN.HOTEN
                                 %>' />                                 
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="25px" HeaderText="Mã ĐP">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMADPG" CssClass="editable" Text='<%# Bind("MADP") %>' runat="server" Width="50" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="250px" HeaderText="Số DB">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMADBG" CssClass="editable" Text='<%# Bind("MADB") %>' runat="server" Width="50" />
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
