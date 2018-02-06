<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.SuaChua.PhanCongSuaChua" CodeBehind="PhanCongSuaChua.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    
    <script type="text/javascript">
        $(document).ready(function() {
            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
                }
            });
        });

        function CheckFormFilterNV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');

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
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
							            OnRowDataBound="gvNhanVien_RowDataBound" OnRowCommand="gvNhanVien_RowCommand" 
							            OnPageIndexChanging="gvNhanVien_PageIndexChanging">
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
                            <td class="crmcell right">Mã nhân viên</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="90px" MaxLength="200" TabIndex="4" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <div class="right">Nhân viên phụ trách</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENNV" runat="server" Width="250px" MaxLength="200" TabIndex="4" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Thông tin sữa chữa</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="cboMAXL" runat="server" Width="300px"> </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Ngày báo</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYBAO" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left"><strong>Giờ</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtGio" runat="server" Width="30px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left"><strong>Phút</strong></div>
                                <div class="left">
                                    <asp:TextBox ID="txtPhut" runat="server" Width="30px" MaxLength="15" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right btop"></td>
                            <td class="crmcell btop">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" TabIndex="17" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                         OnClientClick="return CheckFormCancel();" TabIndex="18" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />
    <bwaco:FilterPanel ID="filterPanel" runat="server" ShowAreaCode="True" />
    <br />
    
    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" 
                    OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADON") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADON") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderStyle-Width="60px" HeaderText="Mã đơn">
                            <ItemTemplate>
                                <strong><%# Eval("MADON") %></strong>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã KH" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <%# 
                                    (Eval("KHACHHANG") != null) ? string.Format("{0}{1}{2}", 
                                        Eval("KHACHHANG.MADP"), Eval("KHACHHANG.DUONGPHU"), Eval("KHACHHANG.MADB")) 
                                    : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên KH" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# 
                                    (Eval("KHACHHANG") != null) ? string.Format("{0}", Eval("KHACHHANG.TENKH")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# (Eval("KHACHHANG") != null) ? 
                                        (Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : "") +
                                        Eval("KHACHHANG.DUONGPHO.TENDP") + ", " +
                                        (Eval("KHACHHANG.PHUONG") != null ? Eval("KHACHHANG.PHUONG.TENPHUONG") + ", " : "") +
                                        Eval("KHACHHANG.KHUVUC.TENKV")                                                        
                                    : Eval("THONGTINKH")
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Thông tin XL" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# (Eval("THONGTINXULY") != null) ? Eval("THONGTINXULY.TENXL") : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Ngày báo">
                            <ItemTemplate>
                                <%# (Eval("NGAYBAO") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYBAO"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
               </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br /><br />
    <asp:UpdatePanel ID="upnlGridAssigned" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="p-5"><b>DANH SÁCH ĐƠN SỬA CHỮA ĐÃ PHÂN CÔNG</b></div>
            <div class="crmcontainer">                
                <eoscrm:Grid ID="gvListAssigned" runat="server" UseCustomPager="true" OnRowDataBound="gvListAssigned_RowDataBound" 
                    OnPageIndexChanging="gvListAssigned_PageIndexChanging" OnRowCommand="gvListAssigned_RowCommand" PageSize="20">
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="60px" HeaderText="Mã đơn">
                            <ItemTemplate>
                                <strong><%# Eval("MADON") %></strong>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã KH" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <%# 
                                    (Eval("KHACHHANG") != null) ? string.Format("{0}{1}{2}", 
                                        Eval("KHACHHANG.MADP"), Eval("KHACHHANG.DUONGPHU"), Eval("KHACHHANG.MADB")) 
                                    : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên KH" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# 
                                    (Eval("KHACHHANG") != null) ? string.Format("{0}", Eval("KHACHHANG.TENKH")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# (Eval("KHACHHANG") != null) ? 
                                        (Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : "") +
                                        Eval("KHACHHANG.DUONGPHO.TENDP") + ", " +
                                        (Eval("KHACHHANG.PHUONG") != null ? Eval("KHACHHANG.PHUONG.TENPHUONG") + ", " : "") +
                                        Eval("KHACHHANG.KHUVUC.TENKV")                                                        
                                    : Eval("THONGTINKH")
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Thông tin XL" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# (Eval("THONGTINXULY") != null) ? Eval("THONGTINXULY.TENXL") : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhân viên xử lý" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <%# 
                                    (Eval("NHANVIEN1") != null) ? string.Format("{0}", Eval("NHANVIEN1.HOTEN")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Ngày báo">
                            <ItemTemplate>
                                <%# (Eval("NGAYBAO") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYBAO"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADON") %>'
                                    CommandName="ReportItem" Text='Báo cáo'></asp:LinkButton>                                                    
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
               </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
