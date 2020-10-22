<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="TraCuuChietTinh.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.TraCuuChietTinh" %>
    
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divThietKeVatTu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTKVTDlgContainer");
                }
            });

            $("#divChietTinh").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divChietTinhDlgContainer");
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
        });

        function CheckFormFilterDHSONO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDHSONO) %>', '');
            return false;
        }

        function CheckFormSaveDH() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btSaveDH) %>', '');
            return false;
        }
       

    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divTKVTDlgContainer">
        <div id="divThietKeVatTu" style="display: none">
            <asp:UpdatePanel ID="upnlThietKeVatTu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 700px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Tên khách hàng </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Label ID="lbTENKHVTTK" runat="server" Text="tenkh"></asp:Label>
                                                    <asp:Label ID="lbMADDKVTTK" runat="server" Visible="false"></asp:Label>
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
							        <eoscrm:Grid ID="gvTKVT" runat="server"  UseCustomPager="true"			            
							            OnPageIndexChanging="gvTKVT_PageIndexChanging">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ký hiệu" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <%# Eval("MAVT") != null ? Eval("MAVT").ToString() : ""%>
                                                </ItemTemplate>
                                            </asp:TemplateField>            
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="NOIDUNG" HeaderText="Tên vật tư" />
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="SOLUONG" HeaderText="Số lương" />
                                            <asp:TemplateField HeaderText="#" HeaderStyle-Width="25%">
                                                <ItemTemplate>
                                                    <%# Eval("ISCTYDTU") != null ? Eval("ISCTYDTU").ToString() == "True" ? "Cấp" : "Bán"
                                                        : ""%>
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
							        <eoscrm:Grid ID="gvDongHoSoNo" runat="server" UseCustomPager="true" AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnPageIndexChanging="gvDongHoSoNo_PageIndexChanging" OnRowCommand="gvDongHoSoNo_RowCommand">
							            <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <PagerSettings FirstPageText="Đồng hồ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MADH") %>' 
                                                        CommandName="SelectMADH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADH").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Loại ĐH" DataField="MALDH" />
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
                                                <div class="left"><%= DonDangKy != null ? DonDangKy.MADDK : ""%></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKy != null ? DonDangKy.DIACHILD : "" %></div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày đăng ký</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (DonDangKy != null && DonDangKy.NGAYDK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", DonDangKy.NGAYDK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Ngày thiết kế</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ThietKe != null && ThietKe.NGAYLTK.HasValue) ?  
                                                                      String.Format("{0:dd/MM/yyyy}", ThietKe.NGAYLTK.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Trạng thái</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKy != null ? 
                                                                        DonDangKy.TRANGTHAITHIETKE2 != null ?  DonDangKy.TRANGTHAITHIETKE2.TENTT : ""
                                                                      : "" %></div>
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
                                        <tr>
                                            <td class="crmcell right">Tổng giá trị công trình</td>
                                            <td class="crmcell">
                                                <div class="left"><%= (ChietTinh != null && ChietTinh.TONG_ST.HasValue) ?  
                                                                      String.Format("{0:0,0}", ChietTinh.TONG_ST.Value) 
                                                                      : "" %>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right vtop">Ghi chú</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtGHICHU" ReadOnly="true" Width="425px" TextMode="MultiLine" runat="server" />
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
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
    <br />
    <asp:UpdatePanel ID="upTTKH" UpdateMode="Conditional" runat="server" Visible="false">
        <ContentTemplate>
            <div class="crmcontainer">
                <table cellpadding="3" cellspacing="1" style="width: 500px;">
                    <tbody>
                        <tr>                    
                            <td class="crmcell right">Mã đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbMADDKKH" runat="server" ></asp:Label>
                                </div>
                                <td class="crmcell right">Tên KH</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Label ID="lbTenKH" runat="server" ></asp:Label>
                                    </div>                                                        
                                </td>                                                     
                            </td>                             
                        </tr>
                        <tr>                    
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbDiaChiKH" runat="server" ></asp:Label>
                                </div>                                                                                    
                            </td>                             
                        </tr>
                        <tr>                    
                            <td class="crmcell right">Số No</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbSoNo" runat="server" ></asp:Label>
                                    <asp:Label ID="lbMaDH" runat="server" Visible="false"></asp:Label>                                    
                                </div>                                                                                    
                            </td>                             
                        </tr>
                        <tr>
                            <td ></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btSaveDH" runat="server" CssClass="myButton" Text = "Lưu số No đồng hồ" OnClick="btSaveDH_Click"
                                        OnClientClick="return CheckFormSaveDH();" UseSubmitBehavior="false" Visible="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
     <asp:UpdatePanel ID="upTongTienCT" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table cellpadding="3" cellspacing="1" style="width: 500px;">
                    <tbody>
                        <tr>                    
                            <td class="crmcell right">Tổng tiền </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTongTienCT" runat="server"></asp:TextBox>
                                    <asp:Label ID="lbMADDK" runat="server" Visible="false"></asp:Label>
                                </div>                                                                                 
                            </td>                             
                        </tr>
                        <tr>
                            <td ></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btSaveTongTienCT" runat="server" CssClass="myButton" Text="Lưu" OnClick="btSaveTongTienCT_Click" />
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
            <div class="crmcontainer" id="divGridList" runat="server">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnRowDataBound="gvList_RowDataBound"  OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="chiết tính" PageButtonCount="2" />  
                    <Columns>  
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên KH" DataField="TENKH" HeaderStyle-Width="25%" />
                        <asp:BoundField HeaderText="Điện thoại" DataField="DIENTHOAI" HeaderStyle-Width="80px" />
                        <asp:BoundField HeaderText="Địa chỉ lắp đặt" DataField="DIACHIHM" HeaderStyle-Width="35%" />
                        <asp:TemplateField HeaderText="Ngày duyệt" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# (Eval("NGAYN") != null) ?
                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYN")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="280px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="ReportItem" Text='Báo cáo'></asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnIDEdit" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="EditItem" Text='Chỉnh sửa'
                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách Đồng hồ', 500, 'divDongHoSoNo')" >
                                </asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lkSuaTongTien" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="SuaTongTien" Text='Sửa tổng tiền'>
                                </asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lkVatTuTK" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    OnClientClick="openDialogAndBlock('Thiết kế vật tư', 700, 'divThietKeVatTu')"
                                    CommandName="VatTuThietKe" Text='Vật tư TK'>
                                </asp:LinkButton>                         
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái đơn"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgTT" runat="server" Width="90px" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="showCTStatus" CausesValidation="false" UseSubmitBehavior="false"
                                    OnClientClick="openDialogAndBlock('Thông tin chiết tính', 600, 'divChietTinh')" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</asp:Content>
