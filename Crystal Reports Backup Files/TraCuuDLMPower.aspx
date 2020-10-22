<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="TraCuuDLMPower.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.TraCuuDLMPower" Culture="vi-VN" uiCulture="vi"%>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divDangKy").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDangKyDlgContainer");
                }
            });

            $("#divThietKe").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divThietKeDlgContainer");
                }
            });

            $("#divChietTinh").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divChietTinhDlgContainer");
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

            $("#divThiCong").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divThiCongDlgContainer");
                }
            });

            $("#divNghiemThu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divNghiemThuDlgContainer");
                }
            });

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
        });

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

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckFormFitler() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
            return false;
        }

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');
            return false;
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
            return false;
        }

        function clickButtonbtnFilter(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=btnFilter.ClientID %>').focus();
                return false;
            }
        }

        function CheckFormTIMSONODHN() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btTIMSONODHN) %>', '');
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
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADPPO") + "-" + Eval("DUONGPHUPO") %>' CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHUPO" HeaderText="Đường phụ" />
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
    <div id="divDangKyDlgContainer">
        <div id="divDangKy" style="display: none">
            <asp:UpdatePanel ID="upnlDangKy" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Mã đơn đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.MADDKPO : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.TENKH : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Điện thoại</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Mục đích sử dụng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.MDSDPO != null ?  DonDangKyPo.MDSDPO.TENMDSD : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.NGAYDK.HasValue ?  String.Format("{0:dd/MM/yyyy}", DonDangKyPo.NGAYDK.Value) : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Nhân viên nhập</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.NHANVIEN != null ?  DonDangKyPo.NHANVIEN.HOTEN : ""
                                                                      : "" %></div>
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
    <div id="divThietKeDlgContainer">
        <div id="divThietKe" style="display: none">
            <asp:UpdatePanel ID="upnlThietKe" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Mã đơn đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.MADDKPO : ""%></div>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.TENKH : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Tên công trình</td>
                                            <td class="crmcell">
                                                <div class="left"><%= ThietKePo != null ? ThietKePo.TENTK : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Điện thoại</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.NGAYDK.HasValue ?  String.Format("{0:dd/MM/yyyy}", DonDangKyPo.NGAYDK.Value) : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="crmcell right">Nhân viên phụ trách</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.NHANVIEN2 != null) ? ThietKePo.NHANVIEN2.HOTEN : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày thiết kế</td>
                                            <td class="crmcell">
                                                <div class="left"><%= ThietKePo != null ? ThietKePo.NGAYLTK.HasValue ? String.Format("{0:dd/MM/yyyy}", ThietKePo.NGAYLTK.Value) : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Trạng thái</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.TRANGTHAITHIETKE1 != null ?  DonDangKyPo.TRANGTHAITHIETKE1.TENTT : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Người duyệt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.NHANVIEN != null) ? ThietKePo.NHANVIEN.HOTEN : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày duyệt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= ThietKePo != null ? ThietKePo.NGAYDTK.HasValue ? String.Format("{0:dd/MM/yyyy}", ThietKePo.NGAYDTK.Value) : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Nhân viên nhập</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.NHANVIEN1 != null) ? ThietKePo.NHANVIEN1.HOTEN : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ghi chú</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.CHUTHICH != null) ? ThietKePo.CHUTHICH : "" %></div>
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
    <div id="divChietTinhDlgContainer">
        <div id="divChietTinh" style="display: none">
            <asp:UpdatePanel ID="upnlChietTinh" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Mã đơn đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.MADDKPO : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.TENKH : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Điện thoại</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (DonDangKyPo != null && DonDangKyPo.NGAYDK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", DonDangKyPo.NGAYDK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày thiết kế</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.NGAYLTK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ThietKePo.NGAYLTK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="crmcell right">Người lập chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.NHANVIEN1 != null) ? ChietTinh.NHANVIEN1.HOTEN : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày lập chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.NGAYLCT.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ChietTinh.NGAYLCT.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Trạng thái</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.TRANGTHAITHIETKE2 != null ?  DonDangKyPo.TRANGTHAITHIETKE2.TENTT : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Người duyệt chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.NHANVIEN != null) ? ChietTinh.NHANVIEN.HOTEN : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày duyệt chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.NGAYDCT.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ChietTinh.NGAYDCT.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <%--<tr>
                                            <td class="crmcell right">Tổng giá trị công trình</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.TONG_ST.HasValue) ?  
                                                                      String.Format("{0:0,0}", ChietTinh.TONG_ST.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>--%>
                                        <tr>
                                            <td class="crmcell right vtop">Ghi chú</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtGHICHUCT" ReadOnly="true" Width="425px" TextMode="MultiLine" runat="server" />
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
    <div id="divHopDongDlgContainer">
        <div id="divHopDong" style="display: none">
            <asp:UpdatePanel ID="upnlHopDong" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Mã đơn đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= HopDongPo != null ? HopDongPo.MADDKPO : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.TENKH : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Điện thoại</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : ""%></div>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (DonDangKyPo != null && DonDangKyPo.NGAYDK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", DonDangKyPo.NGAYDK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày thiết kế</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.NGAYLTK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ThietKePo.NGAYLTK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày lập chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.NGAYLCT.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ChietTinh.NGAYLCT.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày lập hợp đồng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (HopDongPo != null && HopDongPo.NGAYTAO.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", HopDongPo.NGAYTAO.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Nhân viên nhập</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Label ID="lvNVLAPHĐ" runat="server" Text="NV lập"></asp:Label>
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
    <div id="divThiCongDlgContainer">
        <div id="divThiCong" style="display: none">
            <asp:UpdatePanel ID="upnlThiCong" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Mã đơn đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= ThiCong != null ? ThiCong.MADDK : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.TENKH : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Điện thoại</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : ""%></div>
                                            </td>
                                        </tr>
                                         <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (DonDangKyPo != null && DonDangKyPo.NGAYDK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", DonDangKyPo.NGAYDK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày thiết kế</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKePo != null && ThietKePo.NGAYLTK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ThietKePo.NGAYLTK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày lập chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.NGAYLCT.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ChietTinh.NGAYLCT.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày lập hợp đồng</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (HopDongPo != null && HopDongPo.NGAYTAO.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", HopDongPo.NGAYTAO.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Nhân viên thi công</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThiCong != null && ThiCong.NHANVIEN != null) ?  ThiCong.NHANVIEN.HOTEN : "" %></div>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Trạng thái</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.TRANGTHAITHIETKE4 != null ?  DonDangKyPo.TRANGTHAITHIETKE4.TENTT : ""
                                                                      : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày gắn ĐH</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThiCong != null && ThiCong.NGAYHT.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ThiCong.NGAYHT.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Nhân viên nhập</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Label ID="lvNVLAPTHICONG" runat="server" Text="NV lập"></asp:Label>
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
    <div id="divNghiemThuDlgContainer">
        <div id="divNghiemThu" style="display: none">
            <asp:UpdatePanel ID="upnlNghiemThu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Mã đơn đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbMADDKBBNT" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr>                                        
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbTENKHBBNT" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbNTNGAYNHAPDON" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr> 
                                        <tr>
                                            <td class="crmcell right">Ngày thiết kế</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbNTNGAYTK" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày chiết tính</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbNTNGAYCT" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr> 
                                        <tr>
                                            <td class="crmcell right">Ngày hợp đồng</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbNTNGAYHD" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày thi công</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbNTNGAYTC" runat="server" ></asp:Label></div>
                                            </td>
                                        </tr> 
                                        <tr>
                                            <td class="crmcell right">Ngày nhập BB nghiệm thu</td>
                                            <td class="crmcell">
                                                <div class="left"><asp:Label ID="lbNTNGAYLAPBBNT" runat="server" ></asp:Label></div>
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
    <div id="divEditCustomerDlgContainer">
        <div id="divEditCustomer" style="display: none">
            <asp:UpdatePanel ID="upnlEditCustomer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td style="height: 300px">
                                
                                <div id="divCT" runat="server">
                                    Thông tin chiết tính<br />
                                    MADDK:
                                    <%= ChietTinh != null ? ChietTinh.MADDK : ""%>
                                    <br />
                                    Tên công trình :
                                    <%= ChietTinh != null ? ChietTinh.TENCT : ""%>
                                    <br />
                                    Địa chỉ lắp đặt :
                                    <%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %>
                                    <br />
                                    Điện thoại :
                                    <%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : "" %>
                                    <br />
                                    <%--Phường :
                                    <%= ( DonDangKyPo != null || DonDangKyPo.PHUONGPO != null || DonDangKyPo.PHUONGPO.TENPHUONG != null) ? DonDangKyPo.PHUONGPO.TENPHUONG  : "" %>
                                    <br />--%>
                                    Khu vực :
                                    <%= DonDangKyPo != null ? DonDangKyPo.KHUVUCPO.TENKV  : "" %>
                                    <br />
                                    Ngày đăng ký :
                                    <%= DonDangKyPo != null ? DateTimeUtil.GetDateStringToDisplay(DonDangKyPo.NGAYDK ) : "" %>
                                    <br />
                                    Trạng thái :
                                    <%= DonDangKyPo != null ? DonDangKyPo.TRANGTHAITHIETKE2 != null ? DonDangKyPo.TRANGTHAITHIETKE2.TENTT : "" : ""%>
                                    <br />
                                    Nhân viên lập :
                                    <%= ChietTinh != null ? ChietTinh.NHANVIEN != null? ChietTinh.NHANVIEN.HOTEN  : ""  : "" %>
                                    <br />
                                    Ngày lập :
                                    <%= ChietTinh != null ? ChietTinh.NGAYLCT != null? DateTimeUtil.GetDateStringToDisplay(ChietTinh.NGAYLCT)  : ""  : "" %>
                                    <br />
                                    Người duyệt :
                                    <asp:Label ID="lblNguoiDuyetCT" runat="server" Text=""></asp:Label>
                                    <br />
                                    Ngày duyệt :
                                    <asp:Label ID="lblNgayDuyetCT" runat="server" Text=""></asp:Label>
                                </div>
                                <div id="divHD" runat="server">
                                    Thông tin hợp đồng<br />
                                    MADDK:
                                    <%= HopDongPo != null ? HopDongPo.MADDKPO : ""%>
                                    <br />
                                    Tên khách hàng :
                                    <%= DonDangKyPo != null ? DonDangKyPo.TENKH : "" %>
                                    <br />
                                    Địa chỉ lắp đặt :
                                    <%= DonDangKyPo!= null ? DonDangKyPo.DIACHILD : "" %>
                                    <br />
                                    Trạng thái :
                                    <%= DonDangKyPo != null ? DonDangKyPo.TRANGTHAITHIETKE3 != null ? DonDangKyPo.TRANGTHAITHIETKE3.TENTT : "" : ""%>
                                    <br />
                                    Nhân viên lập :
                                    <asp:Label ID="lblNhanVienLapHD" runat="server" Text=""></asp:Label>
                                    <br />
                                    Ngày lập :
                                     <%= HopDongPo != null ? HopDongPo.NGAYTAO != null ? DateTimeUtil.GetDateStringToDisplay(HopDongPo.NGAYTAO) : "" : ""%>
                                   
                                </div>
                                <div id="divTC" runat="server">
                                    Thông tin thi công<br />
                                    MADDK:<%= ThiCong != null ? ThiCong.MADDK : ""%><br />Tên khách hàng :
                                    <%= DonDangKyPo != null ? DonDangKyPo.TENKH : "" %>
                                    <br />
                                    Địa chỉ lắp đặt :
                                    <%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %>
                                    <br />
                                    Điện thoại :
                                    <%= DonDangKyPo != null ? DonDangKyPo.DIENTHOAI : "" %>
                                    <br />
                                    Trạng thái :
                                    <%= DonDangKyPo != null ? DonDangKyPo.TRANGTHAITHIETKE4 != null ? DonDangKyPo.TRANGTHAITHIETKE4.TENTT : "" : ""%>
                                    <br />
                                    Đơn vị thi công :
                                    <%= ThiCong != null ? ThiCong.MAPB != null ? ThiCong.PHONGBAN.TENPB : "" : ""%>
                                    <br />
                                    Người thi công :
                                    <%= ThiCong != null ? ThiCong.MANV != null ? ThiCong.NHANVIEN.HOTEN : "" : ""%>
                                    <br />
                                    Ngày hoàn thành :
                                    <%= ThiCong != null ? ThiCong.NGAYHT != null ? DateTimeUtil.GetDateStringToDisplay(ThiCong.NGAYHT) : "" : ""%>
                                    <br />
                                    Ghi chú thi công :
                                    <%= ThiCong != null ? ThiCong.GHICHU : ""%>
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
                            <td class="crmcell right width-125">Mã đơn đăng ký</td>
                            <td class="crmcell width-250">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="95px" MaxLength="15" 
                                        TabIndex="1" />                            
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbDAIDIEN" Enabled="false" runat="server" />
                                </div>
                                <div class="left width-50">
                                    <strong>Là đại diện</strong>
                                </div>                                 
                            </td>
                            <td class="crmcell right width-125">Mục đích sử dụng</td>
                            <td class="crmcell width-175"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlMUCDICH" Width="142px" TabIndex="7" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">Số HĐ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHD1" runat="server" Width="120px" MaxLength="4" TabIndex="9" />
                                </div>                               
                            </td>
                            <td class="crmcell right width-100"></td>
                            <td class="crmcell"></td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã đơn đại diện</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlMADDKTONG" Enabled="false" Width="140px" TabIndex="2" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">Số hộ đấu nối</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHODN" runat="server" Width="130px" MaxLength="4" TabIndex="8" />
                                </div>
                            </td>
                            <td class="crmcell right">Số nhân khẩu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONK" runat="server" Width="120px" MaxLength="4" TabIndex="9" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Họ tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="200px" MaxLength="200" TabIndex="3" />
                                </div>
                            </td>
                            <td class="crmcell right">ĐM/NK</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDMNK" runat="server" Width="130px" MaxLength="4" TabIndex="10" />
                                </div>   
                            </td>
                            <td class="crmcell right">Định mức</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDINHMUC" runat="server" Width="120px" Text="0" Enabled="false"
                                        MaxLength="200" />
                                </div> 
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Điện thoại</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtDIENTHOAI" runat="server" Width="130px" MaxLength="200" TabIndex="4" />
                                </div>
                            </td>
                            <td class="crmcell right">CMND</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCMND" runat="server" Width="130px" MaxLength="20" TabIndex="9" />
                                </div>
                            </td>
                            <td class="crmcell right">Ngày cấp CMND</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYCAPCMND" runat="server" Width="80px" MaxLength="10" TabIndex="11" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgCAPCMND" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYCAPCMND"
                                        PopupButtonID="imgCAPCMND" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ thường trú</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA" runat="server" Width="200px" MaxLength="200" TabIndex="5" />
                                </div>             
                            </td>
                            <td class="crmcell right">Ngày nhận đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYCD" runat="server" Width="100px" MaxLength="10" TabIndex="11" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNHANDON" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYCD"
                                        PopupButtonID="imgNHANDON" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                            <td class="crmcell right">Số No</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbSONODH" runat="server" Text="Số No" Font-Bold="True" ForeColor="#0066FF"></asp:Label>
                                </div>                               
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtDCLAPDAT" runat="server" Width="200px" MaxLength="200" TabIndex="5" />
                                </div>
                            </td>
                            <td class="crmcell right">MST</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMST" runat="server" Width="130px" MaxLength="20" TabIndex="9" />
                                </div>
                            </td>
                            <td class="crmcell right">Hẹn khảo sát</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKS" runat="server" Width="80px" MaxLength="10" TabIndex="12" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgKHAOSAT" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYKS"
                                        PopupButtonID="imgKHAOSAT" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" onblur="checkDuongPhoForm();" runat="server" Width="40px" MaxLength="200" TabIndex="6" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MADP</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="24px" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" />
                                </div>
                            </td>
                            <td class="crmcell right">Số HK</td>
                            <td class="crmcell" colspan="3">    
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHIKHAC" runat="server" Width="400px" MaxLength="200" TabIndex="6" />
                                </div>                                                  
                            </td> 
                        </tr>
                        <tr>    
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" Width="142px" AutoPostBack="True"
                                        TabIndex="13" runat="server" onchange="openWaitingDialog(); unblockWaitingDialog();"  
                                        onselectedindexchanged="ddlKHUVUC_SelectedIndexChanged" />
                                </div>
                            </td>
                            <td class="crmcell right">Phòng, tổ, nhà máy</td>
                            <td class="crmcell" colspan="3">
                                <div class="left">
                                    <asp:DropDownList ID="ddlToNhaMay" TabIndex="14" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Phường, xã</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" TabIndex="14" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Người uỷ quyền</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtUYQUYEN" runat="server" MaxLength="100" Width="200px" />
                                </div>
                             </td> 
                            <td class="crmcell right">Nơi lắp đồng hồ điện</td>
                            <td class="crmcell" colspan="3">    
                                <div class="left">
                                    <asp:TextBox ID="txtNOILAPDHN" runat="server" Width="400px" MaxLength="200" TabIndex="6" />
                                </div>                                                               
                            </td>                           
                        </tr>
                        <tr>
                            <td class="crmcell right">Hồ sơ giao cho nhà máy, tổ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHONGBAN2" TabIndex="14" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <td class="crmcell right">Tổng tiền </td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Label ID="lbTongTienTK" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Blue" ></asp:Label>
                                    </div>
                                </td>
                                <td class="crmcell right">Danh số</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtMADPDLM" runat="server" Width="50px" MaxLength="10" TabIndex="11" ReadOnly="True" />
                                        <asp:TextBox ID="txtMADBDLM" runat="server" Width="50px" MaxLength="10" TabIndex="11" ReadOnly="True" />
                                    </div>
                                </td>
                             </td>                                                
                        </tr>
                        <tr>
                            <td class="crmcell right">Nội dung hồ sơ kèm theo</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNOIDUNGKEMTHEO" runat="server" Width="800px"></asp:TextBox>
                                </div>
                             </td>                                                
                        </tr>
                        <tr>
                            <td class="crmcell right">Số trụ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOTRU" runat="server" ></asp:TextBox>
                                </div>
                             </td>                                                
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ngày nhập đơn đăng ký</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbNGAYNHAPDON" runat="server" Text=""></asp:Label>
                                </div>
                                <td class="crmcell right">Ngày nhập thiết kế</td>
                                <td class="crmcell" colspan="3">
                                    <div class="left">
                                        <asp:Label ID="lbNGAYNHAPTK" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ngày nhập chiết tính</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbNGAYCHIETTINH" runat="server" Text=""></asp:Label>
                                </div>
                                <td class="crmcell right">Ngày nhập hợp đồng</td>
                                <td class="crmcell" colspan="3">
                                    <div class="left">
                                        <asp:Label ID="lbNGAYHOPDONG" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ngày nhập thi công</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbNGAYTHICONG" runat="server" Text=""></asp:Label>
                                </div>
                                <td class="crmcell right">Ngày nhập biên bản nghiệm thu</td>
                                <td class="crmcell" colspan="3">
                                    <div class="left">
                                        <asp:Label ID="lbNGAYBBNT" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>                                
                            </td>
                        </tr>   
                        <tr>    
                            <td class="crmcell right">Ngày khai thác</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbNGAYKHAITHAC" runat="server" Text=""></asp:Label>
                                </div>                                                        
                            </td>
                        </tr>                      
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell" colspan="5">
                                <div class="left">
                                    <asp:CheckBox ID="cbISTUYENONGCHUNG" runat="server" TabIndex="34" Visible="false"/>
                                </div>
                                <div class="left"><strong></strong></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Đổi thông tin xuất hóa đơn
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="cbSDInfo_INHOADON" runat="server" TabIndex="34" />                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH_INHOADON" MaxLength="200" runat="server" Width="300px" TabIndex="38" />
                                </div>
                            </td>
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell" colspan="3">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI_INHOADON" MaxLength="200" runat="server" Width="400px" TabIndex="39" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Số No ĐH</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONODHN"  runat="server"   />
                                    <asp:Button ID="btTIMSONODHN" runat="server" CssClass="myButton" Text="Tìm Số No"
                                         OnClientClick="return CheckFormTIMSONODHN();"  UseSubmitBehavior="false" OnClick="btTIMSONODHN_Click" />
                                </div>                               
                            </td>   
                        </tr>
                        <tr>    
                            <td class="crmcell right btop"></td>
                            <td class="crmcell btop" colspan="5">
                                <div class="left">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="filter"
                                        OnClick="btnFilter_Click" OnClientClick="return CheckFormFitler();" TabIndex="15" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" TabIndex="16" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                         OnClientClick="return CheckFormCancel();" TabIndex="17" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="18" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>                        
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">            
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" 
                    OnRowCommand="gvList_RowCommand" OnRowDataBound="gvList_RowDataBound" 
                    OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDKPO") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDKPO") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Mã đơn">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên khách hàng" HeaderStyle-Width="25%" DataField="TENKH" />
                        <asp:BoundField HeaderStyle-Width="40%" HeaderText="Địa chỉ thường trú" DataField="SONHA" />
                        <asp:BoundField HeaderStyle-Width="40%" HeaderText="Địa chỉ lắp" DataField="DIACHILD" />
                        <asp:TemplateField HeaderText="Kỳ khai thác" HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <%# new KhachHangPoDao().GetMADDK(Eval("MADDKPO").ToString()) != null ? 
                                        String.Format("{0:MM/yyyy}", new KhachHangPoDao().GetMADDK(Eval("MADDKPO").ToString()).KYKHAITHAC)  : ""  %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Đăng ký" HeaderStyle-Width="55px">
                            <ItemTemplate>
                                <asp:Button ID="imgDK" runat="server" Width="70px" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="showDKStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin đơn đăng ký', 500, 'divDangKy')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thiết kế" HeaderStyle-Width="55px">
                            <ItemTemplate>
                                <asp:Button ID="imgTK" runat="server" Width="70px" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="showTKStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin thiết kế', 500, 'divThietKe')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Chiết tính" HeaderStyle-Width="55px">
                            <ItemTemplate>
                                <asp:Button ID="imgCT" runat="server" Width="70px" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="showCTStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin chiết tính', 600, 'divChietTinh')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hợp đồng" HeaderStyle-Width="55px">
                            <ItemTemplate>
                                <asp:Button ID="imgHD" runat="server" Width="70px" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="showHDStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin hợp đồng', 500, 'divHopDong')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thi công" HeaderStyle-Width="55px">
                            <ItemTemplate>                                
                                <asp:Button ID="imgTC" runat="server" Width="70px" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="showTCStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin thi công', 500, 'divThiCong')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nghiệm thu" HeaderStyle-Width="55px">
                            <ItemTemplate>                                
                                <asp:Button ID="imgNT" runat="server" Width="70px" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="showNTStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin nghiệm thu', 500, 'divNghiemThu')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">
                        Đang xử lý
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <div class="newIndicator" style="cursor: default; width: 120px;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Chờ duyệt
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <div class="inprogressIndicator" style="cursor: default; width: 120px;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Đã duyệt
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <div class="approveIndicator" style="cursor: default; width: 120px;" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Bị từ chối
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <div class="rejectIndicator" style="cursor: default; width: 120px;" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
</asp:Content>
