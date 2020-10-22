<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="SuaDongHoPo.aspx.cs" Inherits="EOSCRM.Web.Forms.ThiCongCongTrinh.Power.SuaDongHoPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
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

        function CheckFormFilterDDK() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDDK) %>', '');
            return false;
        }

        function CheckFormFilterDHSONO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDHSONO) %>', '');
            return false;
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
                                            <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Mã ĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MADHPO") %>' 
                                                        CommandName="SelectMADH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADHPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Loại ĐH" DataField="MALDHPO" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Năm SX" DataField="NAMSX" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số No" DataField="SONO" /> 
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số CN KĐ" DataField="SOKD" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số tem KĐ" DataField="TEMKD" />                                           
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
                                                    <asp:TextBox ID="txtFilter" onchange="return CheckFormFilterDDK();" runat="server" MaxLength="200" />
                                                </div>
                                                <td class="crmcell right">Khu vực</td>
                                                <td class="crmcell">                                                
                                                    <div class="right">
                                                        <asp:DropDownList ID="ddlMaKV" runat="server" />
                                                    </div>  
                                                </td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Từ ngày</td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtTuNgay" runat="server" />
                                                </div>
                                                <td class="crmcell right">Đến ngày</td>
                                                <td class="crmcell">
                                                    <div class="left">
                                                        <asp:TextBox ID="txtDenNgay" runat="server" />
                                                    </div>
                                                </td>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"></td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDDK" OnClick="btnFilterDDK_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDDK();" 
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
							            OnPageIndexChanging="gvDDK_PageIndexChanging" >
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDKPO") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <HeaderStyle Width="10%" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" 
                                                HeaderText="Tên khách hàng" >
                                                <HeaderStyle Width="25%" />
                                            </asp:BoundField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="DIACHILD" 
                                                HeaderText="Địa chỉ lắp đặt" >
                                                <HeaderStyle Width="35%" />
                                            </asp:BoundField>
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
                                    <asp:Label ID="lbMADDK1" runat="server" ></asp:Label>
                                </div>
                                <div class="left">  
                                    <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" 
		                                OnClick="btnBrowseDDK_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" 
                                        TabIndex="2"  />
                                </div>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbTENKH1" runat="server" ></asp:Label>
                                </div>    
                                <div class="left">
                                    <asp:Label ID="lbMADHCU" runat="server" Visible="False" ></asp:Label>
                            </div>
                            <div class="left">
                                    <asp:Label ID="lbMADHMOI" runat="server" Visible="False" ></asp:Label>
                            </div>                                                
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại ĐH cũ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbLOAIDHCU" runat="server" ></asp:Label>
                                </div>
                                <div class="left"><div class="right">Số No cũ</div></div>
                                <div class="left">
                                    <asp:Label ID="lbSONOCU" runat="server" ></asp:Label>
                                </div>                            
                            </td>
                        </tr>
                        <tr>                            
                            <td class="crmcell right btop">Tìm số No cần đổi</td>
                            <td class="crmcell btop">
                                <div class="left">
                                    <asp:Button ID="btnBrowseDHSONO" runat="server" CssClass="pickup" OnClick="btnBrowseDHSONO_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách Đồng hồ', 500, 'divDongHoSoNo')" 
                                        TabIndex="21" />
                                </div>                                                    
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại ĐH mới</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbLOAIDHMOI" runat="server" ForeColor="#0066FF" ></asp:Label>
                                </div>
                                <div class="left"><div class="right">Số No mới</div></div>
                                <div class="left">
                                    <asp:Label ID="lbSONOMOI" runat="server" ForeColor="#3366FF" ></asp:Label>
                                </div>                            
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" Rows="3" Width="525" TabIndex="9" 
                                        MaxLength="500" runat="server" Font-Names="Times New Roman" />
                                </div>
                            </td>
                        </tr> 
                        <tr>                            
                            <td class="crmcell right btop"></td>
                            <td class="crmcell btop">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CommandArgument="Insert" CssClass="save"
                                        OnClick="btnSave_Click" TabIndex="16" OnClientClick="return CheckFormSave();" UseSubmitBehavior="false" />
                                </div>                                                    
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
                     UseCustomPager="true" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altrow" />
                    <HeaderStyle CssClass="header" />
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="10%" DataField="MADDKPO" HeaderText="Mã đơn" />                        
                        <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-Width="40%">
                            <ItemTemplate>
                                <%# (Eval("DONDANGKYPO") != null) ? Eval("DONDANGKYPO.TENKH") : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="10%" DataField="SONOCU" HeaderText="Số No cũ" />
                        <asp:BoundField HeaderStyle-Width="10%" DataField="SONOMOI" HeaderText="Số No mới" />
                        <asp:TemplateField HeaderText="Ngày nhập" HeaderStyle-Width="10%">
                            <ItemTemplate>
                                <%# (Eval("NGAYNHAP") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYNHAP")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="20%" DataField="GHICHU" HeaderText="Ghi chú" />                     
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
