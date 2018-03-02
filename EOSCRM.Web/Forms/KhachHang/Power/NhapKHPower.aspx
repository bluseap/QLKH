<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="NhapKHPower.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.NhapKHPower" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
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

            $("#divDongHo").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDongHoDlgContainer");
                }
            });

            $("#divDongHoSoNo").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDongHoSoNoDlgContainer");
                }
            });

            $("#divCQTT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divCQTTDlgContainer");
                }
            });

            $("#divHopDong").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divHopDongDlgContainer");
                }
            });      
        });

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
		    return false;
        }

        function checkDuongPhoForm() {
            // check text boxes have at least one value
            var dp = jQuery.trim(document.getElementById('<%= txtMADP.ClientID %>').value);
		    if (dp != '') {
		        openWaitingDialog();
		        unblockWaitingDialog();
		        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHidden) %>', '');
            }
            return false;
        }

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
		    return false;
        }

        function CheckFormFilterDH() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDH) %>', '');
		    return false;
        }

        function CheckFormFilterDHSONO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDHSONO) %>', '');
		    return false;
        }

        function CheckFormFilterCQTT() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterCQTT) %>', '');
		    return false;
        }

        function CheckFormFilterHD() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterHD) %>', '');
		    return false;
        }
    </script>
    <style type="text/css">
        .auto-style1 {
            height: 29px;
        }
    </style>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDuongPhoDlgContainer">
        <div id="divDuongPho" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">
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
                                                        CommandArgument='<%# Eval("MADPPO")%>' 
                                                        CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="TENDP" HeaderText="Tên đường phố" />                                           
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
    <div id="divDongHoDlgContainer">
        <div id="divDongHo" style="display: none">
            <asp:UpdatePanel ID="upnlDongHo" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDH" onchange="return CheckFormFilterDH();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDH" OnClick="btnFilterDH_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDH();" 
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
							        <eoscrm:Grid ID="gvDongHo" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvDongHo_PageIndexChanging" OnRowCommand="gvDongHo_RowCommand">
                                        <PagerSettings FirstPageText="loại" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã LĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MALDHPO") %>' 
                                                        CommandName="SelectMALDH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MALDHPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" HeaderText="Nơi sản xuất" DataField="NSX" />
                                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Kích cỡ" DataField="KICHCO" />
                                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Kiểu" DataField="KDH" />
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Giá" 
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# String.Format("{0:0,0}", Eval("GIA"))%>
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
    <div id="divDongHoSoNoDlgContainer">
        <div id="divDongHoSoNo" style="display: none">
            <asp:UpdatePanel ID="upnlDongHoSoNo" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDHSONO" onchange="return CheckFormFilterDHSONO();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDHSONO" OnClick="btnFilterDHSONO_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDHSONO();" 
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
							        <eoscrm:Grid ID="gvDongHoSoNo" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvDongHoSoNo_PageIndexChanging" OnRowCommand="gvDongHoSoNo_RowCommand">
                                        <PagerSettings FirstPageText="Đồng hồ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADHPO") %>' 
                                                        CommandName="SelectMADH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADHPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Năm SX" DataField="NAMSX" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số No" DataField="SONO" />                                            
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
    <div id="divCQTTDlgContainer">
        <div id="divCQTT" style="display: none">
            <asp:UpdatePanel ID="upnlCQTT" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordCQTT" onchange="return CheckFormFilterCQTT();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterCQTT" OnClick="btnFilterCQTT_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterCQTT();" 
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
							        <eoscrm:Grid ID="gvCQTT" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvCQTT_PageIndexChanging" OnRowCommand="gvCQTT_RowCommand">
                                        <PagerSettings FirstPageText="cơ quan" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã LĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MACQ") %>' 
                                                        CommandName="SelectMACQ" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MACQ").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên CQ" DataField="TENCQ" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Ngân hàng">
                                                <ItemTemplate>
                                                    <%# Eval("NGANHANG.TENNH") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tài khoản" DataField="SOTK" />
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
	<div id="divHopDongDlgContainer">
	    <div id="divHopDong" style="display: none">
            <asp:UpdatePanel ID="upnlHopDong" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 900px;">
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
                                                    <asp:TextBox ID="txtKeywordHD" onchange="return CheckFormFilterHD();" runat="server" Width="250px" MaxLength="200" Font-Names="Times New Roman" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterHD" OnClick="btnFilterHD_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterHD();" 
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
							        <eoscrm:Grid ID="gvHopDongDien" runat="server" UseCustomPager="true" 
							            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvHopDongDien_RowDataBound" OnRowCommand="gvHopDongDien_RowCommand" 
							            OnPageIndexChanging="gvHopDongDien_PageIndexChanging">
                                        <PagerSettings FirstPageText="hợp đồng điện" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="80px" ItemStyle-Font-Bold="true" HeaderText="Mã ĐĐK">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDKPO") %>' 
                                                        CommandName="SelectMADDK" Text='<%# Eval("MADDKPO") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Số hợp đồng" HeaderStyle-Width="80px" DataField="SOHD" />
                                            <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-Width="25%" >
                                                <ItemTemplate>
                                                    <%# Eval("DONDANGKYPO.TENKH")%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:TemplateField HeaderText="Địa chỉ">
                                                <ItemTemplate>
                                                    <%# Eval("SONHA").ToString().Trim() != "" ?  Eval("SONHA").ToString().Trim() + ", " : "" %>
                                                    <%# Eval("DUONGPHOPO") != null ? Eval("DUONGPHOPO.TENDP") + ", " : Eval("DONDANGKYPO.TEN_DC_KHAC") + ", " %>
                                                    <%# Eval("PHUONGPO") != null ? Eval("PHUONGPO.TENPHUONG") + ", " : "" %>
                                                    <%# Eval("KHUVUCPO.TENKV") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ngày tạo" HeaderStyle-Width="80px">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYTAO") != null) ?
                                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYTAO"))
                                                        : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>--%>
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
                                <asp:Label ID="lbMADDK" runat="server" Visible="False"></asp:Label>
                            </td>
                            <td class="crmcell right">
                                Serial đồng hồ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADH" runat="server" MaxLength="20" Width="100px" 
                                        TabIndex="9" Visible="False" />
                                </div>   
                                <div class="left">
                                    <asp:Label ID="lblSONO" runat="server" />
                                </div>                                                               
                                <div class="left">
                                    <asp:Button ID="btnBrowseDHSONO" runat="server" CssClass="pickup" OnClick="btnBrowseDHSONO_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách Đồng hồ', 500, 'divDongHoSoNo')" 
                                        TabIndex="21" Visible="true" />
                                </div>                        
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số hợp đồng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHD" runat="server" Width="100px" MaxLength="20" TabIndex="3" Font-Names="Times New Roman" ReadOnly="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseSOHD" runat="server" CssClass="pickup" OnClick="btnBrowseSOHD_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách hợp đồng', 900, 'divHopDong')" 
                                        TabIndex="44" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Kích cỡ đồng hồ
                            </td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:DropDownList ID="ddlKICHCODH" runat="server" TabIndex="10" Visible="false"
                                        >
                                        <asp:ListItem Text="15" Value="15" />
                                        <asp:ListItem Text="20" Value="20" />
                                        <asp:ListItem Text="34" Value="34" />
                                        <asp:ListItem Text="42" Value="42" />
                                        <asp:ListItem Text="49" Value="49" />
                                        <asp:ListItem Text="60" Value="60" />
                                        <asp:ListItem Text="100" Value="100" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblKICHCO" runat="server" Visible="true"/>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số điện thoại
                            </td>
                            <td class="crmcell" style="height: 29px">
                                <div class="left">
                                    <asp:TextBox ID="txtSDT" runat="server" MaxLength="15" Width="100px" TabIndex="55" />
                                </div>
                            </td>
                            <td class="crmcell right" style="height: 29px">
                                Loại đồng hồ
                            </td>
                            <td class="crmcell" style="height: 29px">
                                <div class="left">
                                    <asp:TextBox ID="txtMALDH" runat="server" Width="45px" MaxLength="20" 
                                        TabIndex="11" ReadOnly="True"  />                                    
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseLOAIDH" runat="server" CssClass="pickup" OnClick="btnBrowseLOAIDH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách loại đồng hồ', 500, 'divDongHo')" 
                                        TabIndex="21" Visible="False" />
                                </div>                                
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Tên khách hàng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" MaxLength="200" Width="300px" TabIndex="4" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Mục đích sử dụng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMDSD" AutoPostBack="true" onchange="openWaitingDialog()" 
                                    OnSelectedIndexChanged="ddlMDSD_SelectedIndexChanged" runat="server" Width="200px" TabIndex="12" />
                                </div>                        
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số nhà
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA2" runat="server" MaxLength="100" Width="50px" TabIndex="77" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHILD" runat="server" MaxLength="150" Width="250px" TabIndex="77" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Ngày lặp đặt
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYHT" runat="server" Width="100px"
                                        TabIndex="13" 
                                        ontextchanged="txtNGAYHT_TextChanged" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgCreateDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="36" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYHT"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số trụ 
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOTRUKD" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:Label ID="Label1" runat="server" Text="Số trụ TK: "></asp:Label>
                                    <asp:Label ID="lbSoTruThietKe" runat="server" ></asp:Label>
                                </div>
                                <td class="crmcell right">
                                    Đợt in HĐ  <!--M<sup>3</sup> khoán tối thiểu!-->
                                </td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:DropDownList ID="ddlDOTINHD" runat="server" >                                        
                                        </asp:DropDownList>
                                    </div>
                                    <div class="left">
                                        <asp:TextBox ID="txtM3" runat="server" Width="100px" TabIndex="30" Visible="false" />
                                    </div>
                                    <div class="left">
                                        <!--<strong>m<sup>3</sup></strong>!-->
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" runat="server" Width="160px" TabIndex="77" 
                                        onselectedindexchanged="ddlKHUVUC_SelectedIndexChanged" onchange="openWaitingDialog()">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">
                                 
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlGHI2THANG1LAN" Width="70px" runat="server" TabIndex="23" Visible="False">
                                        <asp:ListItem Text="Không" Value="0" />
                                        <asp:ListItem Text="Có" Value="1" />
                                    </asp:DropDownList>                                
                                </div> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAIKH" Width="90px" Enabled="False" runat="server" TabIndex="24" Visible="false">
                                        <asp:ListItem Text="Tư nhân" Value="TN" />
                                        <asp:ListItem Text="Nhà nước" Value="NN" />
                                    </asp:DropDownList>                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Phường, Xã
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" runat="server" TabIndex="5" AutoPostBack="true" OnSelectedIndexChanged="ddlPHUONG_SelectedIndexChanged" >
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">
                               
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlHOTRO" Width="112px" runat="server" TabIndex="34" Visible="False">
                                        <asp:ListItem Text="Cho không" Value="C" />
                                        <asp:ListItem Text="Tiền mặt" Value="M" />
                                        <asp:ListItem Text="Không" Value="KK" />                                        
                                    </asp:DropDownList> 
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSOHO" runat="server" Width="100px" Enabled="False" TabIndex="27" Visible="false"/>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Ấp, Tổ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlAPTO" runat="server" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Đường phố
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" onblur="checkDuongPhoForm();" runat="server" MaxLength="4" Width="40px" TabIndex="6" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MADP</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="25px" TabIndex="91" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 600, 'divDuongPho')" 
                                        TabIndex="52" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td> 
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHUHO" Width="70px" runat="server" TabIndex="23" Visible="False">
                                        <asp:ListItem Text="Không" Value="0" />
                                        <asp:ListItem Text="Tại quầy" Value="T" />
                                        <asp:ListItem Text="Kho bạc" Value="K" />
                                    </asp:DropDownList>                                
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSONK" runat="server" Width="100px" Enabled="False" TabIndex="28" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbISDINHMUC" runat="server" Enabled="False" TabIndex="29" Visible="false"/>
                                </div>
                                <div class="left">
                                    <strong><!--Tính theo ĐMNK!--></strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Danh bộ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:HiddenField ID="hdfIDKH" runat="server" />
                                    <asp:TextBox ID="txtMADB" runat="server" MaxLength="4" Width="100px" 
                                        TabIndex="7" />
                                </div>
                            </td>                            
                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                               Số thứ tự</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSTT" runat="server" MaxLength="20" Width="40px" TabIndex="64" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                SKU </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSKU" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDBO" runat="server" Width="100px"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbKHONGTINH117" runat="server" TabIndex="31" Visible="false" />
                                </div>
                                <div class="left">
                                    <strong><!--Theo nghị định 117!--></strong>
                                </div>
                            </td> 
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực cấp
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUCDN" runat="server" Width="160px" TabIndex="55" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                <!--Cơ quan thanh toán!-->
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCQ" MaxLength="20" Enabled="false" runat="server" Width="100px" TabIndex="32" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseCQ" runat="server" Enabled="false" CssClass="pickup" OnClick="btnBrowseCQ_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách cơ quan thanh toán', 500, 'divCQTT')" 
                                        TabIndex="33" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENCQ" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Hình thức thanh toán
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlHTTT" Width="160px" runat="server" TabIndex="16">
                                    </asp:DropDownList> 
                                </div>
                            </td>
                            <td class="crmcell right">
                                <!--Mã giá áp dụng!--></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMAGIA" Enabled="false" Width="112px" runat="server" TabIndex="34" Visible="false">
                                        <asp:ListItem Text="" Value="NULL" />
                                        <asp:ListItem Text="A" Value="A" />
                                        <asp:ListItem Text="B" Value="B" />
                                        <asp:ListItem Text="D" Value="D" />
                                        <asp:ListItem Text="E" Value="E" />
                                        <asp:ListItem Text="T" Value="T" />
                                        <asp:ListItem Text="M" Value="M" />
                                        <asp:ListItem Text="N" Value="N" />
                                        <asp:ListItem Text="I" Value="I" />
                                    </asp:DropDownList> 
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số tài khoản
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOTK" runat="server" MaxLength="20" Width="100px" TabIndex="17" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                <!--Khách hàng đặc biệt!--></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlLKHDB" runat="server" Enabled="False" Width="200px" TabIndex="25" Visible="false">
                                    </asp:DropDownList>  
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Mã số thuế
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMSTHUE" runat="server" MaxLength="25" Width="100px" TabIndex="8" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                <!--Kỳ hỗ trợ!--></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtKYHOTRO" Enabled="False" runat="server" Width="100px"  TabIndex="37" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgKYHOTRO" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="38" Visible="false"/>
                                </div>
                                <div class="left">
                                    <strong><!--(Dành cho hộ nghèo)!--></strong>
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtKYHOTRO"
                                    PopupButtonID="imgKYHOTRO" DefaultView="Months" TodaysDateFormat="dd/MM/yyyy" Format="MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td style="padding-bottom: 10px;">
                            </td>
                        </tr>
                            <tr>
                            <td class="header btop" colspan="4">
                                Các thông số tiêu thụ áp dụng cho kỳ đầu tiên
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Trạng thái</td>
                            <td class="crmcell" colspan=3>
                                <div class="left">
                                    <asp:DropDownList ID="ddlTT" Width="112px" runat="server" TabIndex="42">
                                        <asp:ListItem Text="Bình thường" Value="GDH_BT" />
                                        <asp:ListItem Text="Khoán" Value="GDH_KH" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <strong></strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCHISODAU" runat="server" Width="100px" TabIndex="43" Visible="false" />
                                </div>
                                <div class="left">
                                    <strong>Chỉ số cuối</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCHISOCUOI" runat="server" Width="100px" TabIndex="14" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="pbottom-10">
                            </td>
                        </tr>
                        <tr>
                            <td class="header" colspan="4">
                                <div class="left">
                                    Đổi thông tin xuất hóa đơn
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="cbSDInfo_INHOADON" runat="server" TabIndex="39" />                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH_INHOADON" MaxLength="200" runat="server" Width="250px" TabIndex="40" />
                                </div>
                            </td>
                            <td class="crmcell right">Địa chỉ xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI_INHOADON" MaxLength="200" runat="server" Width="250px" TabIndex="41" />
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td colspan="4" class="bbottom pbottom-10">
                            </td>
                        </tr>   
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" 
                                        TabIndex="15" UseSubmitBehavior="false"/>
                                    
                                </div>  
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="46" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>             
                    </tbody>
                </table>
            </div> 
            <br />
            <div class="crmcontainer">
                <eoscrm:Grid 
                    ID="gvKhachHang" runat="server" UseCustomPager="true" PageSize=20
                        OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging" OnSelectedIndexChanged="gvKhachHang_SelectedIndexChanged">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox"  HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã khách hàng">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnID" runat="server" 
                                CommandArgument='<%# Eval("IDKHPO") %>'
                                CommandName="SelectHD"  OnClientClick="openWaitingDialog();unblockWaitingDialog();" 
                                Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO") + Eval("DUONGPHUPO").ToString()+ Eval("MADBPO") )%>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                
                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="Số TT" DataField="STT" />
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên khách hàng" DataField="TENKH" />
                        <%--<asp:TemplateField HeaderStyle-Width="25%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%# String.IsNullOrEmpty(Eval("SONHA").ToString()) ? "" : Eval("SONHA") + ", " %>
                                <%# Eval("DUONGPHOPO.TENDP")%>, <%# Eval("PHUONGPO.TENPHUONG") %>
                            </ItemTemplate>
                        </asp:TemplateField> --%>   
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Khu vực">
                            <ItemTemplate>
                                <%# Eval("KHUVUCPO.TENKV") %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Ngày nhập">
                            <ItemTemplate>
                                <%# (Eval("NGAYGNHAP") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYGNHAP"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid> 
            </div>           
        </ContentTemplate>
    </asp:UpdatePanel>  
</asp:Content>
