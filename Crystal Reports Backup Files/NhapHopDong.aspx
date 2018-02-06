<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="NhapHopDong.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.NhapHopDong" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
		$(document).ready(function() {
		    $("#divDonDangKy").dialog({
		        autoOpen: false,
		        modal: true,
		        minHeight: 20,
		        height: 'auto',
		        width: 'auto',
		        resizable: false,
		        open: function(event, ui) {
		            $(this).parent().appendTo("#divDonDangKyDlgContainer");
		        }
		    });

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

		function CheckFormFilterDP() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');

		    return false;
		}

		function CheckFormFilterDDK() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDDK) %>', '');

		    return false;
		}

		function CheckChangeKhuVuc() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		}

		function CheckFormCancel() {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');

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
                                                    <asp:TextBox ID="txtFilter" onchange="return CheckFormFilterDDK();" runat="server" Width="150px" MaxLength="200" />
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
							            OnPageIndexChanging="gvDDK_PageIndexChanging">
                                        <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã đơn">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADDK") %>' CommandName="EditItem"                                                         
                                                        Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" DataField="TENKH" HeaderText="Tên khách hàng" />
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
	<asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Mã đơn</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="90px" MaxLength="11" 
                                        TabIndex="1" ReadOnly="True" />
                                    <asp:Button ID="btnBrowseDDK" runat="server" CssClass="addnew" 
		                                OnClick="btnBrowseDDK_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                OnClientClick="openDialogAndBlock('Chọn đơn đăng ký', 700, 'divDonDangKy')" 
                                        TabIndex="2"  />
                                </div>
                                <div class="left">
                                    <div class="right">Số hợp đồng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSOHD" runat="server" Width="90px" MaxLength="10" 
                                        TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" ReadOnly="true" runat="server" Width="387px" MaxLength="200" 
                                        TabIndex="4" />
                                </div>
                            </td>            
                        </tr>  
                        <tr>
                            <td class="crmcell right">Địa chỉ lắp đặt</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA" runat="server" Width="50px" MaxLength="150" 
                                        TabIndex="5" />                                    
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENDUONG" runat="server" Width="315px" ReadOnly="true" 
                                        MaxLength="200" TabIndex="6" /> 
                                </div>
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                
                                <div class="left"></div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="40px" Visible="false"
                                        TabIndex="8" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="9" Visible="False" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" Width="164px" 
                                        runat="server" TabIndex="10" 
                                        onselectedindexchanged="ddlKHUVUC_SelectedIndexChanged" 
                                        onchange="CheckChangeKhuVuc();">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Phường</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" Width="164px" runat="server" TabIndex="11">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            
                        <tr>
                            <td class="crmcell right">Loại hợp đồng</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlLOAIHD" runat="server" Width="94px" TabIndex="12">
                                        <asp:ListItem Text="Tư nhân" Value="TN" />
                                        <asp:ListItem Text="Nhà nước" Value="NN" />
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Hình thức thanh toán</div>
                                </div>
                                <div class="left">
                                     <asp:DropDownList ID="ddlHTTT" Width="153px" runat="server" TabIndex="13">
                                     </asp:DropDownList>
                                </div>
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">Cỡ đồng hồ nước</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlKICHCODH" Width="94px" runat="server" TabIndex="14">
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
                                    <div class="right">Loại ống nhánh</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtLOAIONG" runat="server" Width="174px" MaxLength="200" 
                                        TabIndex="15" />                                    
                                </div>
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlMDSD" Width="164px" runat="server" TabIndex="16" />
                                </div>
                                <div class="left">
                                    <div class="right">Định mức sử dụng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDMSD" runat="server" Width="50px" MaxLength="200" 
                                        TabIndex="17" />                                     
                                </div>
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">Số hộ</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtSOHO" runat="server" Width="50px" MaxLength="200" 
                                        TabIndex="18" />                                    
                                </div>
                                <div class="left">
                                    <div class="right">Số nhân khẩu</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSONK" runat="server" Width="50px" MaxLength="200" 
                                        TabIndex="19" />                                    
                                </div>
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">CMND</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtCMND" runat="server" Width="150px" MaxLength="20" 
                                        TabIndex="20" />                                    
                                </div>
                                <div class="left">
                                    <div class="right">Ngày làm hợp đồng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYTAO" onkeypress="return CheckKeywordFilter(event);" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="24" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgCreateDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="25" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYTAO"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right">MST</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtMST" runat="server" Width="150px" MaxLength="20" 
                                        TabIndex="21" />         
                                </div>
                                <div class="left">
                                    <div class="right">Ngày hiệu lực</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYHL" onkeypress="return CheckKeywordFilter(event);" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="26" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgHLDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="27" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayHL" runat="server" TargetControlID="txtNGAYHL"
                                    PopupButtonID="imgHLDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Đường phố
                            </td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHO" runat="server" MaxLength="4" Width="50px" TabIndex="91" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btDUONGPHO" runat="server" CssClass="pickup" OnClick="btDUONGPHO_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 600, 'divDuongPho')" 
                                        TabIndex="52" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbDUONGPHO" runat="server" />
                                </div>
                                <div class="left">
                                    <div class="right">Danh bộ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" runat="server" MaxLength="4" Width="40px" Visible="false"
                                        TabIndex="7" />
                                </div>                               
                                <div class="left">                                    
                                    <asp:TextBox ID="txtMADB" runat="server" MaxLength="8" Width="80px" 
                                        TabIndex="7" />
                                </div>                            
                            </td> 
                        </tr>
                        <%--
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left width-250">
                                </div>
                                <div class="left">
                                    <div class="right">Ngày kết thúc</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKT" onkeypress="return CheckKeywordFilter(event);" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="28" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgKTDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="29" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayKT" runat="server" TargetControlID="txtNGAYKT"
                                    PopupButtonID="imgKTDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>          
                        </tr>--%>
                        <tr>
                            <td class="header btop" colspan="2">
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
                                <div class="left width-250">
                                    <asp:TextBox ID="txtTENKH_INHOADON" MaxLength="200" runat="server" Width="300px" TabIndex="38" />
                                </div>
                                <div class="left">
                                    <div class="right">Địa chỉ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI_INHOADON" MaxLength="200" runat="server" Width="350px" TabIndex="39" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="30" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClientClick="return CheckFormCancel();" 
                                        OnClick="btnCancel_Click" TabIndex="31" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
		</ContentTemplate>
	</asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGrid" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid 
                    ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand" 
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound" PageSize="20">
                    <PagerSettings FirstPageText="hợp đồng" PageButtonCount="2" />
                    <Columns>                    
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="75px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                CommandName="EditItem" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MADDK").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>    
                        <asp:BoundField HeaderText="Số hợp đồng" DataField="SOHD" HeaderStyle-Width="80px" />
                        <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <%# Eval("DONDANGKY.TENKH")%>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# Eval("SONHA").ToString().Trim() != "" ?  Eval("SONHA").ToString().Trim() + ", " : "" %>
                                <%# Eval("DUONGPHO") != null ? Eval("DUONGPHO.TENDP") + ", " : Eval("DONDANGKY.TEN_DC_KHAC") + ", " %>
                               
                                <%# Eval("KHUVUC.TENKV")%>
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderText="Ngày tạo" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYTAO") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYTAO")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày hiệu lực" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYHL") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYHL")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày kết thúc" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYKT") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYKT")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel> 
	<br />    
	<div class="crmcontainer p-5">
        <a href="../ThiCongCongTrinh/NhapThiCong.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
