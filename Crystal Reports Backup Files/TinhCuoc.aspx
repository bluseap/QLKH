<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="TinhCuoc.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.TinhCuoc" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
    
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
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

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormSearch();
            }
        }

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
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
        }

        function CheckFormTinhCuoc() {
            openWaitingDialog();
            unblockWaitingDialog();            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnTinhCuoc) %>', '');
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
    <asp:UpdatePanel ID="upnlTinhCuoc" UpdateMode="Conditional" runat="server">
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
                                <div class="left">
                                    <strong>Khu vực</strong>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" runat="server" TabIndex="3" OnSelectedIndexChanged="ddlKHUVUC_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <strong></strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" runat="server" onkeydown="return CheckChangeDP(event);" MaxLength="4" Width="30px" TabIndex="4" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="30px" TabIndex="5" Visible="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click" Visible="false"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">
                                Đợt ghi
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlDOTIN" runat="server"></asp:DropDownList>
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
                                <div class="right">
                                    <asp:Button ID="btnTinhCuoc" runat="server" CommandArgument="Insert" CssClass="save"
                                        OnClick="btnTinhCuoc_Click" TabIndex="13" OnClientClick="return CheckFormTinhCuoc();" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upgvDotInThuHo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <asp:DataList ID="dlDotInHD" Width="100%" CssClass="crmcontainer" runat="server" 
                OnItemDataBound="dlDotInHD_ItemDataBound" OnItemCommand="dlDotInHD_ItemCommand"
                RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table">
                <HeaderTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow-down.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkEnabledDOTIN', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkEnabledDOTIN', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </HeaderTemplate>
                <FooterTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkEnabledDOTIN', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkEnabledDOTIN', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </FooterTemplate>
                <ItemTemplate>
                    <table class="crmdcrmtable p-5">
                        <tbody>
                            <tr>
                                <td style="width: 4%; border: 0px;">
                                    <input id="chkEnabledDOTIN" runat="server" type="checkbox"  />
                                </td>
                                <td style="width: 76%; border: 0px;">
                                    <%# Eval("TENKV") %>
                                </td>                                   
                                <td style="width: 4%; border: 0px;">
                                    <asp:ImageButton ID="imgUnlock" runat="server" CausesValidation="False" 
                                        CommandArgument='<%# Eval("MADP").ToString() + "-" + Eval("MAKV").ToString() + "-" + Eval("THANG").ToString() + "-" + Eval("NAM").ToString()  %>' 
                                        CommandName="Unlock" OnClientClick="openWaitingDialog();unblockWaitingDialog();" ImageAlign="Baseline" ImageUrl='~/content/images/common/btnUnlock2.png' ToolTip="Mở khóa để tính cước" />
                                </td>  
                            </tr>
                        </tbody>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upgvDOTIN" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <asp:DataList ID="dlTinhCuocDotIn" Width="100%" CssClass="crmcontainer" runat="server" 
                OnItemDataBound="dlTinhCuocDotIn_ItemDataBound" OnItemCommand="dlTinhCuocDotIn_ItemCommand"
                RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table">
                <HeaderTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow-down.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkEnabledDI', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkEnabledDI', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </HeaderTemplate>
                <FooterTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkEnabledDI', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkEnabledDI', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </FooterTemplate>
                <ItemTemplate>
                    <table class="crmdcrmtable p-5">
                        <tbody>
                            <tr>
                                <td style="width: 4%; border: 0px;">
                                    <input id="chkEnabledDI" runat="server" type="checkbox"  />
                                </td>
                                <td style="width: 76%; border: 0px;">
                                    <%# Eval("TENDP")%>
                                </td>
                                <td style="width: 8%; border: 0px;">
                                    <%# Eval("MADP") %>
                                </td>
                                <td style="width: 4%; border: 0px;">
                                    <%# Eval("DUONGPHU")%>
                                </td>      
                                <td style="width: 4%; border: 0px;">
                                    <asp:ImageButton ID="imgUnlock" runat="server" CausesValidation="False" 
                                        CommandArgument='<%# Eval("MADP") + "-" + 
                                                (Eval("DUONGPHU").ToString() == "" ? "NULL" : Eval("DUONGPHU")) + "-" +
                                                Eval("MAKV") + "-" + 
                                                Eval("KYGHI.Month") + "-" + 
                                                Eval("KYGHI.Year") %>' 
                                        CommandName="UnlockDI" OnClientClick="openWaitingDialog();unblockWaitingDialog();" ImageAlign="Baseline" ImageUrl='~/content/images/common/btnUnlock2.png' ToolTip="Mở khóa để tính cước" />
                                </td>      
                            </tr>
                        </tbody>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--<asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <asp:DataList ID="dlTinhCuoc" Width="100%" CssClass="crmcontainer" runat="server" 
                OnItemDataBound="dlTinhCuoc_ItemDataBound" OnItemCommand="dlTinhCuoc_ItemCommand"
                RepeatColumns="3" RepeatDirection="Vertical" RepeatLayout="Table" Visible="false">
                <HeaderTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow-down.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkEnabled', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkEnabled', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </HeaderTemplate>
                <FooterTemplate>
                    <table class="crmtable p-5">
                        <tbody>
                            <tr class="listheader">
                                <td style="width: 4%; border: 0px;">
                                    <img src="<%= ResolveUrl("~")%>content/images/common/arrow.png" />
                                </td>
                                <td style="border: 0px">
                                    <a href="#" onclick="SelectAll('chkEnabled', true); return false;">
                                        <strong>Chọn hết</strong></a> 
                                    / 
                                    <a href="#" onclick="SelectAll('chkEnabled', false); return false;">
                                        <strong>Bỏ chọn hết</strong></a>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </FooterTemplate>
                <ItemTemplate>
                    <table class="crmdcrmtable p-5">
                        <tbody>
                            <tr>
                                <td style="width: 4%; border: 0px;">
                                    <input id="chkEnabled" runat="server" type="checkbox"  />
                                </td>
                                <td style="width: 76%; border: 0px;">
                                    <%# Eval("TENDP")%>
                                </td>
                                <td style="width: 8%; border: 0px;">
                                    <%# Eval("MADP") %>
                                </td>
                                <td style="width: 4%; border: 0px;">
                                    <%# Eval("DUONGPHU")%>
                                </td>      
                                <td style="width: 4%; border: 0px;">
                                    <asp:ImageButton ID="imgUnlock" runat="server" CausesValidation="False" 
                                        CommandArgument='<%# Eval("MADP") + "-" + 
                                                (Eval("DUONGPHU").ToString() == "" ? "NULL" : Eval("DUONGPHU")) + "-" +
                                                Eval("MAKV") + "-" + 
                                                Eval("KYGHI.Month") + "-" + 
                                                Eval("KYGHI.Year") %>' 
                                        CommandName="Unlock" OnClientClick="openWaitingDialog();unblockWaitingDialog();" ImageAlign="Baseline" ImageUrl='~/content/images/common/btnUnlock2.png' ToolTip="Mở khóa để tính cước" />
                                </td>      
                            </tr>
                        </tbody>
                    </table>
                </ItemTemplate>
            </asp:DataList>
        </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>
