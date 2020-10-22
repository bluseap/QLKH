﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="NhapThietKeLX.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.NhapThietKeLX" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
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

            $("#divDonDangKy").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDonDangKyDlgContainer");
                }
            }); 

            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
                }
            });
        });

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
            return false;
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');
            return false;
        }

        function CheckFormTUCHOI() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnTUCHOI) %>', '');
            return false;
        }

        function CheckFormTRAHOSO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btTRAHOSO) %>', '');
            return false;
        }        

        function checkDuongPhoForm() {            
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
                                                    <asp:LinkButton ID="lnkBtnIDDP" runat="server" 
                                                        CommandArgument='<%# Eval("MADP") %>' CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHU" HeaderText="Đường phụ" />--%>
                                            <asp:BoundField HeaderStyle-Width="60%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Khu vực">
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

    <div id="divDonDangKySuaVTDlgContainer">
        <div id="divDonDangKySuaVT" style="display: none">
            <asp:UpdatePanel ID="upnlDonDangKySuaVT" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 700px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="TextBox1" onchange="return CheckFormFilterDP();" runat="server" Width="150px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <div class="right">Khu vực</div>                                                
                                                </div>
                                                <div class="left">
                                                    <div class="right">
                                                        <asp:DropDownList ID="DropDownList1" runat="server" Width="160px" />
                                                    </div>                                                
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Từ ngày</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="TextBox2" runat="server" />
                                                </div>
                                                <div class="left">
                                                    <div class="right">Đến ngày</div>                                                
                                                </div>
                                                <div class="left">
                                                    <asp:TextBox ID="TextBox3" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"></td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="Button1" OnClick="btnFilter_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDP();" 
                                                        runat="server" CssClass="filter" />
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
							        <eoscrm:Grid ID="Grid1" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDDK_RowDataBound" OnRowCommand="gvDDK_RowCommand" 
							            OnPageIndexChanging="gvDDK_PageIndexChanging">
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="13%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDK") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" HeaderText="Tên khách hàng" />
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="NGAYSINH" HeaderText="Năm sinh" />
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="DIACHILD" HeaderText="Địa chỉ lắp đặt" />
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
    <div id="divDonDangKyDlgContainer">
        <div id="divDonDangKy" style="display: none">
            <asp:UpdatePanel ID="upnlDonDangKy" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 700px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtFilter" onchange="return CheckFormFilterDP();" runat="server" Width="150px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <div class="right">Khu vực</div>                                                
                                                </div>
                                                <div class="left">
                                                    <div class="right">
                                                        <asp:DropDownList ID="ddlMaKV" runat="server" Width="160px" />
                                                    </div>                                                
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Từ ngày</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtTuNgay" runat="server" />
                                                </div>
                                                <div class="left">
                                                    <div class="right">Đến ngày</div>                                                
                                                </div>
                                                <div class="left">
                                                    <asp:TextBox ID="txtDenNgay" runat="server" />
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"></td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDP();" 
                                                        runat="server" CssClass="filter" />
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
							        <eoscrm:Grid ID="gvDDK" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDDK_RowDataBound" OnRowCommand="gvDDK_RowCommand" 
							            OnPageIndexChanging="gvDDK_PageIndexChanging">
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="13%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDK") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" HeaderText="Tên khách hàng" />
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="NGAYSINH" HeaderText="Năm sinh" />
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="DIACHILD" HeaderText="Địa chỉ lắp đặt" />
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
    <div id="divNhanVienDlgContainer">
        <div id="divNhanVien" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 800px;">
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
                                                    <asp:TextBox ID="txtKeywordNV" onchange="return CheckFormFilterNV();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterNV" OnClick="btnFilterNV_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterNV();" 
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
							        <eoscrm:Grid ID="gvNhanVien" runat="server" UseCustomPager="true" 
							            AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvNhanVien_RowDataBound" OnRowCommand="gvNhanVien_RowCommand" 
							            OnPageIndexChanging="gvNhanVien_PageIndexChanging">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings FirstPageText="nhân viên" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã NV">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MANV") %>' CommandName="SelectMANV"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MANV").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="HOTEN" HeaderText="Họ tên" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Phòng ban">
                                                <ItemTemplate>
                                                    <%# Eval("PHONGBAN") != null ? Eval("PHONGBAN.TENPB") : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Công việc">
                                                <ItemTemplate>
                                                    <%# Eval("CONGVIEC") != null ? Eval("CONGVIEC.TENCV") : ""%>
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
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Mã đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="90px" MaxLength="10" 
                                        TabIndex="1" ReadOnly="True" />
                                    <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" OnClick="btnBrowseDDK_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" />                                   
                                    <asp:Label ID="lbMADDKTRAHSTK" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="lbCOBIENSUAVTDH" runat="server" Visible="False"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="400px" MaxLength="200" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên thiết kế</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtTENTK" runat="server" MaxLength="200" TabIndex="3" />
                                </div>
                                <div class="left width-150">
                                    <strong>Danh số KH kế bên</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" Width="100px" MaxLength="8" TabIndex="3" ReadOnly="True" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" onblur="checkDuongPhoForm();" runat="server" Width="60px" MaxLength="200" TabIndex="99" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MADP</asp:LinkButton>
                                </div>                               
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" />
                                </div>  
                                <div class="left">
                                    <asp:Label ID="lbTenDuongPho" runat="server" Font-Bold="True" ></asp:Label>                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtDIACHITK" runat="server" TabIndex="4" />
                                </div>
                                <div class="left width-150">
                                    <strong>Mô tả vị trí, đường, hẽm</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGHEMTK" runat="server" TabIndex="5" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Phường (Xã)</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlPHUONGTK" runat="server" TabIndex="6"></asp:DropDownList>
                                </div>
                                <div class="left width-150">
                                    <strong>Số điện thoại</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSDTTK" runat="server" TabIndex="7" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Vị trí đồng hồ</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtVITRITK" runat="server" TabIndex="7" />
                                </div>
                                <div class="left width-150">
                                    <strong>Danh số tk</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDANHSOTK" runat="server" TabIndex="7" />
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right">Nhân viên</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID="txtNV1" runat="server"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbNV1" runat="server" Text="Label" Visible="False"></asp:Label>
                                </div>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày thiết kế</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYTK" onkeypress="return CheckKeywordFilter(event);" runat="server"
                                        Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYTK"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <asp:CheckBox ID="ckKHTT100" runat="server" Text="  Khách hàng thanh toán 100%" />
                                </div>
                                <div class="left">
                                    <strong></strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKS" onkeypress="return CheckKeywordFilter(event);" runat="server"
                                        Width="90px" MaxLength="10" Visible=false/>
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible=false/>
                                </div>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYKS"
                                    PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />                               
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Chú thích (Lý do trả hồ sơ)</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCHUTHICH" Width="400px" TextMode="MultiLine" runat="server" Height="61px">Khách hàng tự trám lại sân, nền, tường, vỉa hè sau khi thi công xong công trình</asp:TextBox>
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTHECHAP" runat="server" Width="130px" MaxLength="20" TabIndex="9" Visible=false/>
                                </div>
                                <div class="left width-150">
                                    <div class="right">
                                        <asp:CheckBox ID="cbTHAMGIAONGCAI" runat="server" TabIndex="34" Visible=false/>
                                    </div>
                                </div>
                                <div class="left"><strong></strong></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="16" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClientClick="return CheckFormCancel();" 
                                        OnClick="btnCancel_Click" TabIndex="17" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnTUCHOI" CssClass="reject" runat="server" OnClientClick="return CheckFormTUCHOI();" 
                                        UseSubmitBehavior="false" OnClick="btnTUCHOI_Click" />  
                                </div>
                                <div class="left">
                                    <asp:Button ID="btTRAHOSO" CssClass="myButton" runat="server" OnClientClick="return CheckFormTRAHOSO();" 
                                        UseSubmitBehavior="false" OnClick="btTRAHOSO_Click" Text="Trả hồ sơ KD" />  
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <%--<Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />               
        </Triggers>--%>
    </asp:UpdatePanel>
    <br />
    
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
    <br />
    
    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvThietKe" runat="server" UseCustomPager="true" PageSize="20"
                    OnRowCommand="gvThietKe_RowCommand" OnPageIndexChanging="gvThietKe_PageIndexChanging">
                    <PagerSettings FirstPageText="thiết kế" PageButtonCount="2" />
                    <Columns>
                        <%--<asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDK") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDK") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="SelectMADDK" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MADDK").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên đơn ĐK" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# Eval("DONDANGKY.TENKH")  %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên thiết kế" DataField="TENTK" HeaderStyle-Width="25%" />
                        <asp:TemplateField HeaderText="Địa chỉ lắp đặt" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# Eval("DONDANGKY.DIACHILD")  %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày KS" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <%# (Eval("DONDANGKY.NGAYKS") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("DONDANGKY.NGAYKS")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày TK" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <%# (Eval("NGAYLTK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYLTK")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText=" " HeaderStyle-Width="200px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkEdit" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="editTK" CssClass="link" Text='Sửa'></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBocVatTu" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="SelectMADDK" CssClass="link" Text='Bốc vật tư'></asp:LinkButton>
                                &nbsp;&nbsp;&nbsp;
                                <asp:LinkButton ID="lnkSUAVITRIDH" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="SuaViTriDH" CssClass="link" Text='Sửa vị trí ĐH-TC'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />
    <div class="crmcontainer p-5">
        <a href="../ThietKe/DuyetThietKe.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>