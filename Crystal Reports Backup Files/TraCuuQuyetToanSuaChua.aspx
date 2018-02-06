<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="TraCuuQuyetToanSuaChua.aspx.cs" Inherits="EOSCRM.Web.Forms.SuaChua.TraCuuQuyetToanSuaChua" %>
    
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>

<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
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
        });
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divGridList" runat="server">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnRowDataBound="gvList_RowDataBound"  OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="chiết tính" PageButtonCount="2" />  
                    <Columns>  
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADON") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên thiết kế" DataField="TENCT" HeaderStyle-Width="25%" />
                        <%--<asp:BoundField HeaderText="Điện thoại" DataField="DIENTHOAI" HeaderStyle-Width="80px" />--%>
                        <asp:BoundField HeaderText="Địa chỉ hạng mục" DataField="DIACHIHM" HeaderStyle-Width="35%" />
                        <asp:TemplateField HeaderText="Ngày lập chiết tính" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# (Eval("NGAYLCT") != null) ?
                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYLCT")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="ReportItem" Text='Báo cáo'></asp:LinkButton>
                                &nbsp;&nbsp;<asp:LinkButton ID="lnkBtnIDEdit" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="EditItem" Text='Chỉnh sửa'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái đơn"  HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# (Eval("NGAYDQT")!=null) ?"Đã duyệt quyết toán": "Chưa duyệt quyết toán" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    
</asp:Content>
