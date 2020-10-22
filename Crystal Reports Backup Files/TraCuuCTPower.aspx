<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="TraCuuCTPower.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.Power.TraCuuCTPower" %>

<%@ Register Src="/UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

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
                open: function (event, ui) {
                    $(this).parent().appendTo("#divChietTinhDlgContainer");
                }
            });
        });
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
                                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? DonDangKyPo.DIACHILD : "" %></div>
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
                                            <td class="crmcell right">Trạng thái</td>
                                            <td class="crmcell">
                                                <div class="left"><%= DonDangKyPo != null ? 
                                                                        DonDangKyPo.TRANGTHAITHIETKE2 != null ?  DonDangKyPo.TRANGTHAITHIETKE2.TENTT : ""
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
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên thiết kế" DataField="TENKH" HeaderStyle-Width="25%" />
                        <asp:BoundField HeaderText="Điện thoại" DataField="DIENTHOAI" HeaderStyle-Width="80px" />
                        <asp:BoundField HeaderText="Địa chỉ lắp đặt" DataField="DIACHIHM" HeaderStyle-Width="35%" />
                        <asp:TemplateField HeaderText="Ngày đăng ký" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# (Eval("NGAYDK") != null) ?
                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYDK")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="280px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="ReportItem" Text='Báo cáo'>
                                </asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lnkBtnIDEdit" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="EditItem" Text='Chỉnh sửa'>
                                </asp:LinkButton>
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lkSuaTongTien" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="SuaTongTien" Text='Sửa tổng tiền'>
                                </asp:LinkButton>    
                                &nbsp;&nbsp;
                                <asp:LinkButton ID="lkVatTuTK" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    OnClientClick="openDialogAndBlock('Thiết kế vật tư', 700, 'divThietKeVatTu')"
                                    CommandName="VatTuThietKe" Text='Vật tư TK'>
                                </asp:LinkButton> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái đơn"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:Button ID="imgTT" runat="server" Width="90px" CommandArgument='<%# Eval("MADDKPO") %>'
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
