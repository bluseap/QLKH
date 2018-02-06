<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="TraCuuKHPower.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.TraCuuKHPower" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register src="/UserControls/KhachHangFilterPanel.ascx" tagname="KhachHangFilterPanel" tagprefix="bwaco" %>

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

            $("#divUpSoNoPo").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divUpSoNoPoDlgContainer");
                }
            });

            $("#divDuongPhoKHM").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDuongPhoKHMDlgContainer");
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

            $("#divTDCT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTDCTlgContainer");
                }
            });

            $("#divTDH").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTDHlgContainer");
                }
            });

            $("#divDCHD").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDCHDlgContainer");
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

            $("#divTTTT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTTTTDlgContainer");
                }
            });

            $("#divTTVPKH").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTTVPKHlgContainer");
                }
            });

            /*
            * check exactly length of madp = 3 or 0
            */
            $(function () {
                $("#<%= txtMADP.ClientID %>").focusout(function () {
                    var value = $("#<%= txtMADP.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 4) {
                        jAlert('Độ dài mã đường phố phải là 4', 'Cảnh báo');
                        $("#<%= txtMADP.ClientID %>").focus();
                    }
                }).blur(function () {
                    var value = $("#<%= txtMADP.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 4) {
                        jAlert('Độ dài mã đường phố phải là 4', 'Cảnh báo');
                        $("#<%= txtMADP.ClientID %>").focus();
                    }
                });
            });

            /*
            * check exactly length of madb = 4 or 0
            */
            $(function () {
                $("#<%= txtMADB.ClientID %>").focusout(function () {
                    var value = $("#<%= txtMADB.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 8) {
                        jAlert('Độ dài mã danh bộ phải là 8', 'Cảnh báo');
                        $("#<%= txtMADB.ClientID %>").focus();
                    }
                }).blur(function () {
                    var value = $("#<%= txtMADB.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 8) {
                        jAlert('Độ dài mã danh bộ phải là 8', 'Cảnh báo');
                        $("#<%= txtMADB.ClientID %>").focus();
                    }
                });
            });

            $(function () {
                $('#<%= cbSDInfo_INHOADON.ClientID %>').click(function () {
                    var other = $('#<%= cbSDInfo_INHOADON.ClientID %>:checked').val();
                    if (other != undefined) {
                        $('#<%=txtTENKH_INHOADON.ClientID %>').removeAttr('readonly');
                        $('#<%=txtDIACHI_INHOADON.ClientID %>').removeAttr('readonly');
                    }
                    else {
                        $('#<%=txtTENKH_INHOADON.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtDIACHI_INHOADON.ClientID %>').attr('readonly', 'readonly');
                    }
                });
            });
        });

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            //__doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
		    return false;
        }

        function OpenTTTT() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnTieuThu) %>', '');
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

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnInBaoCao) %>', '');
		    return false;
        }

        function CheckFormFilterDH() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDH) %>', '');
		    return false;
        }

        function CheckFormFilterCQTT() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterCQTT) %>', '');
		    return false;
        }

        function CheckFormbtSaveUpSoNoPo() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btSaveUpSoNoPo) %>', '');
            return false;
        }

        function CheckFormFilterDPKHM() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDPKHM) %>', '');
            return false;
        }
       

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div id="divUpSoNoPoDlgContainer">
        <div id="divUpSoNoPo" style="display: none">
            <asp:UpdatePanel ID="UpdivUpSoNoPo" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">
                                                Số No đổi
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtSONODOIMOIPO" runat="server" />
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Loại đồng hồ đổi
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:DropDownList ID="ddlLOAIDHDOIPO" runat="server"></asp:DropDownList>
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Công suất đổi
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtCONGSUATDOIPO" runat="server" />
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">                                                
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                        <asp:Button ID="btSaveUpSoNoPo" OnClick="btSaveUpSoNoPo_Click"
                                                            UseSubmitBehavior="false" OnClientClick="return CheckFormbtSaveUpSoNoPo();" 
                                                            runat="server" CssClass="myButton" Text="Lưu Số No" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>					
					</table>
				</ContentTemplate>
	        </asp:UpdatePanel>
        </div>        
    </div>
    <div id="divDuongPhoKHMDlgContainer">
        <div id="divDuongPhoKHM" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPhoKHM" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDPKHM" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDPKHM" OnClick="btnFilterDPKHM_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDPKHM();" 
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
							        <eoscrm:Grid ID="gvDuongPhoKHM" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDuongPhoKHM_RowDataBound" OnRowCommand="gvDuongPhoKHM_RowCommand" 
							            OnPageIndexChanging="gvDuongPhoKHM_PageIndexChanging">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnIDKHM" runat="server" 
                                                        CommandArgument='<%# Eval("MADPPO")  %>' 
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
    <div id="divTTVPKHlgContainer">
        <div id="divTTVPKH" style="display: none">
            <asp:UpdatePanel ID="upTTVPKH" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTTVPKH" runat="server" UseCustomPager="true" CssClass="crmgrid"  AutoGenerateColumns="false"	 							            
							            OnPageIndexChanging="gvTTVPKH_PageIndexChanging" >
                                        <RowStyle CssClass="row" />
                                        <PagerSettings FirstPageText="danh sách truy thu vi phạm" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Kỳ T.Thu" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("THANG").ToString() + "/" + Eval("NAM").ToString() %>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="D.Số" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("DANHSO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tên KH" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <%# Eval("TENKH") %>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Tr.Thu" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <%# Eval("MTRUYTHUVP") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Đ.mức" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <%# Eval("DMNK") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="T.Tiền" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("TONGTIENVP") %>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Ghi chú" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <%# Eval("GHICHUVP") %>
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
    <div id="divDCHDlgContainer">
        <div id="divDCHD" style="display: none">
            <asp:UpdatePanel ID="upnlDCHD" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvDCHD" runat="server" UseCustomPager="true" CssClass="crmgrid"  AutoGenerateColumns="false"	 						            
							            OnPageIndexChanging="gvDCHD_PageIndexChanging">
                                        <RowStyle CssClass="row" />
                                        <PagerSettings FirstPageText="điều chỉnh HĐ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-Width="13%" DataField="SODBPO" HeaderText="Danh số" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="NAM" HeaderText="Năm" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="THANG" HeaderText="Tháng" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="CHISOCUOI" HeaderText="CS Cuối" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="CHISOCUOIDC" HeaderText="CS cuối ĐC" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="KLTIEUTHUDC" HeaderText="KL" />
                                            <asp:BoundField HeaderStyle-Width="11%" DataField="TONGTIENDC" HeaderText="T.Tiền" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="MASOHDDC" HeaderText="Mã HĐ ĐC" />
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Ngày ĐC">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYDCCS") != null) ?
                                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYDCCS"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" DataField="GHICHUDC" HeaderText="Ghi chú" />                         
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
    <div id="divTDHlgContainer">
        <div id="divTDH" style="display: none">
            <asp:UpdatePanel ID="upnlTDH" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTDH" runat="server" UseCustomPager="true" 							            
							            OnPageIndexChanging="gvTDH_PageIndexChanging">
                                        <PagerSettings FirstPageText="thay ĐH" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Ngày thay cũ">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYTHAYCU") != null) ?
                                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYTHAYCU"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="LOAIDHCU" HeaderText="Loại ĐH cũ" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="SONOCU" HeaderText="Số No cũ" />  
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Ngày thay mới">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYHT") != null) ?
                                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYHT"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>  
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="SONO" HeaderText="Số No mới" />  
                                            <asp:BoundField HeaderStyle-Width="50%" DataField="GHICHU" HeaderText="Ghi chú" />            
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Kỳ thay">
                                                <ItemTemplate>
                                                    <%# (Eval("KYTHAYDH") != null) ?
                                                            String.Format("{0:MM/yyyy}", Eval("KYTHAYDH"))
                                                            : "" %>
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
    <div id="divTDCTlgContainer">
        <div id="divTDCT" style="display: none">
            <asp:UpdatePanel ID="upnlTDCT" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTDCT" runat="server" UseCustomPager="true" 							            
							            OnPageIndexChanging="gvTDCT_PageIndexChanging">
                                        <PagerSettings FirstPageText="thay đổi" PageButtonCount="2" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="SODB" HeaderText="Danh số" />
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="THAYDOI" HeaderText="Lý do đổi" />
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Kỳ đổi">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYDOI") != null) ?
                                                            String.Format("{0:MM/yyyy}", Eval("NGAYDOI"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderStyle-Width="20%" DataField="NGAYDOI" HeaderText="Ngày" />     --%>                                       
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
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADPPO") + "-" + Eval("DUONGPHUPO") %>' 
                                                        CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHUPO" HeaderText="Đường phụ" />
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Khu vực">
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
                                                    <asp:LinkButton ID="linkMa" runat="server" 
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
    <div id="divTTTTDlgContainer">
        <div id="divTTTT" style="display: none">
            <asp:UpdatePanel ID="upnlTTTT" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    
				    <table cellpadding="3" cellspacing="1" style="width: 800px;">
                        <tr>
							<td class="crmcontainer">
							    <asp:Button ID = "btnInBaoCao" runat ="server" CssClass="report" OnClientClick="checkFormReport();" onclick="btnInBaoCao_Click" />
							    <br />
							    <asp:Label ID="Label1" runat="server" Text="Tên khách hàng: "></asp:Label>
                                <asp:Label ID="lblTenKH" runat="server" Text="" Font-Bold=true></asp:Label>
							    <br /> 
							    <eoscrm:Grid ID="gvTTTT" runat="server" UseCustomPager="true" PageSize="15"
                                    OnPageIndexChanging="gvTTTT_PageIndexChanging" >
                                    <PagerSettings FirstPageText="kỳ" PageButtonCount="2" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="1%" HeaderText="#">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="Năm" DataField="NAM" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="Tháng" DataField="THANG" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="CSD" DataField="CHISODAU" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="CSC" DataField="CHISOCUOI" />
                                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="Truy T" DataField="MTRUYTHU" />
                                        <asp:BoundField HeaderStyle-Width="7%" HeaderText="KLTT" DataField="KLTIEUTHU" />
                                        <asp:TemplateField HeaderStyle-Width="11%" HeaderText="Tiền điện">
                                            <ItemTemplate>
                                                <%# String.Format("{0:0,0}", Eval("TONGTIEN")) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                                                                
                                        <asp:TemplateField HeaderStyle-Width="14%" HeaderText="MDSD">
                                            <ItemTemplate>
                                                <%# Eval("MDSDPO.TENMDSD") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="8%" HeaderText="T.Thái" DataField="TTHAIGHI" />
                                        <asp:BoundField HeaderStyle-Width="17%" HeaderText="Ngày ghi" DataField="NGAYGHICS" />
                                        
                                    </Columns>
                                </eoscrm:Grid>                                 
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
                                                    <asp:LinkButton ID="linkMa" runat="server" 
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
	
    <bwaco:KhachHangFilterPanel ID="filterPanel" runat="server" />    
    
    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div id="divCustomersContainer" runat="server" class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">
                                Kỳ thay đổi chi tiết
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANGTDCT" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAMTDCT" runat="server" Width="50px" MaxLength="4" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khai thác
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANG" runat="server" TabIndex="1" Visible="false">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" Visible="false"/>
                                    <asp:Label ID="lbKYKT" runat="server" Font-Bold="True" ></asp:Label>
                                </div>
                            </td>
                            <td class="crmcell right">
                                Loại đồng hồ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMALDH" runat="server" Width="100px" MaxLength="15" 
                                        TabIndex="20" Enabled="False" Visible="False" />                                    
                                </div>
                                <%--<div class="left">/</div>
                                <div class="left">
                                    <asp:Label ID="lblMALDH" runat="server" />
                                </div>--%>
                                <div class="left">
                                    <asp:Button ID="btnBrowseLOAIDH" runat="server" CssClass="pickup" OnClick="btnBrowseLOAIDH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách loại đồng hồ', 500, 'divDongHo')" 
                                        TabIndex="20" Visible="False" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbMALOAIDHM" runat="server" Font-Bold="True" Font-Size="Large" />
                                </div>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số hợp đồng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHD" runat="server" Width="100px" MaxLength="20" TabIndex="3" ReadOnly="True" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Kích cỡ đồng hồ
                            </td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:DropDownList ID="ddlKICHCODH" Width="80px" runat="server" TabIndex="21" 
                                        Enabled="False" >
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
                                    <asp:Label ID="lblKICHCO" runat="server" Visible="TRUE" />
                                </div>                              
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số điện thoại
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSDT" runat="server" MaxLength="15" Width="100px" TabIndex="4" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Serial đồng hồ
                            </td>
                            <td class="crmcell">                                
                                <div class="left" >
                                    <asp:TextBox ID="txtMADH" runat="server" MaxLength="20" Width="100px" 
                                        TabIndex="22" Enabled="False" Visible="False"  />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbSONO" runat="server" Font-Bold="True" Font-Size="Large" />
                                </div>
                                <div class="left" >
                                    <asp:Button ID="btDoiSoNoPo" runat="server" CssClass="myButton" Text="Đổi No" CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Đổi số No đồng hồ', 500, 'divUpSoNoPo')" OnClick="btDoiSoNoPo_Click"  />                                    
                                </div>    
                                <%--
                                <div class="left">
                                    <asp:CheckBox ID="cbLADHTONG" Enabled="false" runat="server" TabIndex="23" />
                                </div>
                                <div class="left">
                                    <strong>Là đồng hồ tổng</strong>
                                </div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số CMND
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:TextBox ID="txtSOCMND" runat="server" MaxLength="20" Width="100px" ReadOnly="True"/>
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Mã đơn:
                            </td>
                            <td class="crmcell">
                             <div class="left">
                                    <asp:Label ID="lbMADDK" runat="server" Font-Bold="True" Font-Size="Small" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Tên khách hàng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" MaxLength="50" Width="180px" 
                                        TabIndex="5" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckTENKH" runat="server" TabIndex="28" oncheckedchanged="ckTENKH_CheckedChanged" Visible="false"
                                         AutoPostBack="True" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Loại khách hàng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAIKH" Width="90px" runat="server" TabIndex="23">
                                        <asp:ListItem Text="Tư nhân" Value="TN" />
                                        <asp:ListItem Text="Nhà nước" Value="NN" />
                                    </asp:DropDownList>                                
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlGHI2THANG1LAN" Width="70px" runat="server" TabIndex="23" Visible="False">
                                        <asp:ListItem Text="Không" Value="0" />
                                        <asp:ListItem Text="Có" Value="1" />
                                    </asp:DropDownList>                                
                                </div>
                                <div class="left">
                                    <strong></strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số nhà
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA2" runat="server" Width="60px" 
                                        TabIndex="6" Enabled="False" />
                                    <asp:TextBox ID="txtDIACHILD" runat="server" MaxLength="150" Width="150px" 
                                        TabIndex="6" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckDIACHILD" runat="server" TabIndex="28"  oncheckedchanged="ckDIACHILD_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Khách hàng đặc biệt
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlLKHDB" runat="server" Width="200px" TabIndex="24">
                                    </asp:DropDownList>  
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"> 
                                <asp:Label ID="lbLDDIACHI" runat="server" Text="Lý do" Visible="False" ForeColor="Red"></asp:Label>                              
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:TextBox ID="txtLDDIACHI" Width="150px" runat="server" Visible="False" ></asp:TextBox>                                    
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" runat="server" Width="160px" TabIndex="7" 
                                        onselectedindexchanged="ddlKHUVUC_SelectedIndexChanged" onchange="openWaitingDialog()">
                                    </asp:DropDownList>                                    
                                </div>
                            </td>
                            <td class="crmcell right">
                                Mục đích sử dụng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMDSD" AutoPostBack="true" onchange="openWaitingDialog()" 
                                    OnSelectedIndexChanged="ddlMDSD_SelectedIndexChanged" runat="server" 
                                        Width="150px" TabIndex="25" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckMDSD" runat="server" TabIndex="28"  oncheckedchanged="ckMDSD_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">                               
                            </td>
                            <td class="crmcell">
                            </td>
                            <td class="crmcell right">       
                                <asp:Label ID="lbLDMDSD" runat="server" Text="Lý do" Visible="False" ForeColor="#FF3300"></asp:Label>                           
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLDMDSD" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">                               
                            </td>
                            <td class="crmcell">
                            </td>
                            <td class="crmcell right">
                                Phiên                                   
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:DropDownList ID="ddlDOTINHD" runat="server" Enabled="False">                                        
                                    </asp:DropDownList>
                                </div>                                                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Phường
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" runat="server" Width="150px" TabIndex="8" 
                                        Enabled="False">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">
                                Số hộ sử dụng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHO" runat="server" Width="20px" TabIndex="26" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckDMUCTAM" runat="server" TabIndex="28" AutoPostBack="true" OnCheckedChanged="ckDMUCTAM_CheckedChanged" />
                                </div>
                                <div class="left">
                                    <strong>Đ.Mức tạm</strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Đường phố
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" onchange="checkDuongPhoForm();" runat="server" 
                                        MaxLength="4" Width="40px" TabIndex="9" Enabled="False" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MADP</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="25px" 
                                        TabIndex="10" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 600, 'divDuongPho')" 
                                        TabIndex="11" Visible="False" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td> 
                            <td class="crmcell right">Số nhân khẩu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONK" runat="server" Width="20px" TabIndex="27" />
                                </div>
                                <div class ="left">
                                    <asp:Label ID="lbSODINHMUC" runat="server" Text="ĐM" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSODINHMUC" runat="server" Width="20px" TabIndex="27" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbISDINHMUC" runat="server" TabIndex="28" OnCheckedChanged="cbISDINHMUC_CheckedChanged" AutoPostBack="true" />
                                </div>
                                <div class="left">
                                    <strong>Tính theo ĐMNK</strong>
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
                                    <asp:TextBox ID="txtMADB" runat="server" MaxLength="4" Width="80px" 
                                        TabIndex="12" Enabled="False" />                                    
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckDANHBO" runat="server" TabIndex="28"  oncheckedchanged="ckDANHBO_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                M<sup>3</sup> khoán tối thiểu
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtM3" runat="server" Width="30px" TabIndex="29" />
                                </div>
                                <div class="left">
                                    <strong>m<sup>3</sup></strong>
                                </div>
                                <div class="left">
                                    <strong>Thu hộ</strong>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHUHO" Width="70px" runat="server" TabIndex="23">
                                        <asp:ListItem Text="Không" Value="0" />
                                        <asp:ListItem Text="Tại quầy" Value="T" />
                                        <asp:ListItem Text="Kho bạc" Value="K" />
                                    </asp:DropDownList>                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">   
                                <asp:Label ID="lbLDDANHSO" runat="server" Text="Lý do" Visible="False" ForeColor="Red"></asp:Label>                            
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLDDANHSO" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Số trụ KD</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOTRUKD" runat="server" TabIndex="13" />
                                </div>
                            </td>
                            <td class="crmcell right">Số trụ TK</td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:Label ID="lbSOTRUTK" runat="server" ></asp:Label>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                               Số thứ tự</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSTT" runat="server" MaxLength="20" Width="40px" TabIndex="13" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Không tính
                            </td>
                            <td class="crmcell">
                                
                                <div class="left">
                                    <asp:CheckBox ID="cbKHONGTINH117" runat="server" TabIndex="30" />
                                </div>
                                <div class="left">
                                    <strong>Theo nghị định 117</strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực cấp nước
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUCDN" runat="server" Width="160px" TabIndex="14" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Cơ quan thanh toán
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCQ" Enabled="false" MaxLength="20" runat="server" Width="100px" TabIndex="31" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseCQ" Enabled="false" runat="server" CssClass="pickup" OnClick="btnBrowseCQ_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách cơ quan thanh toán', 500, 'divCQTT')" 
                                        TabIndex="32" />
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
                                    <asp:DropDownList ID="ddlHTTT" Width="150px" runat="server" TabIndex="15">
                                    </asp:DropDownList> 
                                </div>
                            </td>
                            <td class="crmcell right">
                                Mã giá áp dụng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMAGIA" Enabled="false" Width="112px" runat="server" TabIndex="33">
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
                                <div class="left">
                                    <asp:DropDownList ID="ddlHOTRO" Width="112px" runat="server" TabIndex="34">
                                        <asp:ListItem Text="Cho không" Value="C" />
                                        <asp:ListItem Text="Tiền mặt" Value="M" />
                                        <asp:ListItem Text="Không" Value="KK" />                                        
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
                                    <asp:TextBox ID="txtSOTK" runat="server" MaxLength="20" Width="100px" TabIndex="16" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Ngày hoàn thành</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYHT" runat="server" Width="100px" 
                                        onkeypress="return CheckKeywordFilter(event);" TabIndex="34" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgCreateDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="35" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYHT"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Mã số thuế
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMSTHUE" runat="server" MaxLength="25" Width="100px" 
                                        TabIndex="17" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckMSTHUE" runat="server" TabIndex="28"  oncheckedchanged="ckMSTHUE_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Kỳ hỗ trợ </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtKYHOTRO" Enabled="False" runat="server" Width="100px"  TabIndex="36" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgKYHOTRO" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="37" />
                                </div>
                                <div class="left">
                                    <strong>(Dành cho hộ nghèo)</strong>
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtKYHOTRO"
                                    PopupButtonID="imgKYHOTRO" DefaultView="Months" TodaysDateFormat="dd/MM/yyyy" Format="MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                 <asp:Label ID="lbLDMST" runat="server" Text="Lý do" Visible="False" ForeColor="Red"></asp:Label>                                 
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLDMST" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Trạng thái sử dụng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTTSD" ReadOnly="true" runat="server" Width="100px" TabIndex="19" />
                                </div>
                            </td>
                            <td class="crmcell right">Ngày thay đồng hồ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYTHAYDH" ReadOnly="true" runat="server" Width="100px" TabIndex="36" />
                                </div>
                            </td>  
                        </tr>
                        <tr>
                            <td class="crmcell right">Địa chỉ thường trú</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbDCTHUONGTRU" runat="server" Text=""></asp:Label>
                                </div> 
                                <td class="crmcell right">Số thứ tự ID</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtSTTTS" ReadOnly="true" runat="server" Width="100px" TabIndex="36" />
                                    </div>
                                </td>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nơi lắp</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbNOILAP" runat="server" Text=""></asp:Label>
                                </div>
                                <td class="crmcell right">Địa chỉ lắp</td>
                                <td class="crmcell" colspan="3">
                                    <div class="left">
                                        <asp:Label ID="lbDCLAP" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lnkTDCT" OnClick="lnkTDCT_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử thay đổi chi tiết khách hàng', 750, 'divTDCT')" TabIndex="43">
                                        Lược sử thay đổi chi tiết
                                    </asp:LinkButton>
                                </div>                              
                            </td>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:LinkButton ID="lnkTHAYDH" OnClick="lnkTHAYDH_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử thay đồng hồ khách hàng', 750, 'divTDH')" TabIndex="43">
                                        Lược sử thay đồng hồ
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lnkDCHD" OnClick="lnkDCHD_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử điều chỉnh hoá đơn khách hàng', 750, 'divDCHD')" TabIndex="43">
                                        Lược sử điều chỉnh hoá đơn
                                    </asp:LinkButton>
                                </div>                              
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lkTRUYTHUVP" OnClick="lkTRUYTHUVP_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử truy thu vi phạm khách hàng', 750, 'divTTVPKH')" TabIndex="95">
                                        Lược sử truy thu vi phạm
                                    </asp:LinkButton>
                                </div>                              
                            </td>              
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Cập nhật Khách hàng mới khai thác: số nhà, MĐSD, danh bộ
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="ckUpdateKHM" runat="server" TabIndex="28"  
                                             AutoPostBack="True" OnCheckedChanged="ckUpdateKHM_CheckedChanged"/>                          
                                </div>                                
                            </td>
                        </tr>
                        <tr>                            
                            <td class="crmcell right">
                                    Mục đích SD
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMDSDKHMTAM" runat="server" enabled="False"></asp:DropDownList>                                                                    
                                </div>                                                                   
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    Đường phố
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADPKHM" runat="server" MaxLength="4" Width="40px" TabIndex="9" ReadOnly="True"  />
                                    <asp:Label ID="lbMADPKHM" runat="server" Font-Bold="True" ></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btTIMDPKHM" runat="server" CssClass="pickup" OnClick="btTIMDPKHM_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 600, 'divDuongPhoKHM')" 
                                        TabIndex="11" Visible="False" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                    Danh bộ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDANHBOKHM" runat="server" TabIndex="17" MaxLength="6" Enabled="False"/>                                    
                                </div>                                                                   
                            </td>  
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    C.số đầu khi khai thác
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:TextBox ID="txtCSDAUKHAITHAC" runat="server" Enabled="False"></asp:TextBox>
                                </div>                                                                   
                            </td>
                            <td class="crmcell right">
                                    C.số cuối khi khai thác
                            </td>
                            <td class="crmcell">
                                <div class="left">                                       
                                    <asp:TextBox ID="txtCSCUOIKHAITHAC" runat="server" Enabled="False"></asp:TextBox>                             
                                </div>                                                                   
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    Số định mức tạm
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSODINHMUCTAM" runat="server" Enabled="False"/>
                                </div>                                                                   
                            </td>
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Đổi thông tin xuất hóa đơn
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="cbSDInfo_INHOADON" runat="server" TabIndex="38" />                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH_INHOADON" MaxLength="200" runat="server" Width="250px" TabIndex="39" />
                                </div>
                            </td>
                            <td class="crmcell right">Địa chỉ xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI_INHOADON" MaxLength="200" runat="server" Width="250px" TabIndex="40" />
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
                                    <asp:Button ID="btnSave" runat="server" CommandArgument="Insert" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="CheckFormSave();"
                                        TabIndex="41" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="42" UseSubmitBehavior="false" />                                    
                                </div>
                                <div class="left">
                                    <asp:LinkButton ID="linkBtnTieuThu" OnClick="linkBtnTieuThu_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Thông tin tiêu thụ khách hàng', 750, 'divTTTT')" TabIndex="43">
                                        Xem thông tin tiêu thụ
                                    </asp:LinkButton>
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
                        OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="12%" HeaderText="Danh số">
                            <ItemTemplate>
                            <asp:LinkButton ID="linkMa" runat="server" 
                                CommandArgument='<%# Eval("IDKHPO") %>'
                                CommandName="SelectHD" OnClientClick="openWaitingDialog();unblockWaitingDialog();" 
                                Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO") + Eval("DUONGPHUPO").ToString()+ Eval("MADBPO")) %>'>
                            </asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                        
                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="Số nhà" DataField="SONHA" />
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên khách hàng" DataField="TENKH" />
                        <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Địa chỉ">
                            <ItemTemplate>                                
                                <%# Eval("DUONGPHOPO.TENDP")%>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Khu vực">
                            <ItemTemplate>
                                <%# Eval("KHUVUCPO.TENKV") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="7%" HeaderText="Kỳ KT">
                            <ItemTemplate>
                                <%# Eval("KYKHAITHAC") != null ? String.Format("{0:MM/yyyy}", Eval("KYKHAITHAC")) : ""  %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Số No">
                            <ItemTemplate>
                                <%# Eval("DONGHOPO.SONO") %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="15px" HeaderText="MĐSD">
                            <ItemTemplate>
                                <%# Eval("MAMDSDPO") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Ngày bấm chì">
                            <ItemTemplate>
                                <%# (Eval("NGAYHT") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYHT"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Ngày thay">
                            <ItemTemplate>
                                <%# (Eval("NGAYTHAYDH") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYTHAYDH"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Ngày nhập">
                            <ItemTemplate>
                                <%# (Eval("NGAYGNHAP") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYGNHAP"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Xem tiêu thụ">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkTieuThu" runat="server"
                                    CommandArgument='<%# Eval("IDKHPO") %>'
                                    CommandName="SelectTT" 
                                    OnClientClick="openDialogAndBlock('Thông tin tiêu thụ khách hàng', 750, 'divTTTT')"                                     
                                    Text='Xem t.thụ'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>   
            </div>         
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>

