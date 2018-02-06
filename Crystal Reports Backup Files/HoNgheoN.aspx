<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="HoNgheoN.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.HoNgheoN" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divKhachHang").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divKhachHangDlgContainer");
                }
            });

            $("#divGIAHANHNN").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divGIAHANHNNlgContainer");
                }
            });

        });

        function CheckFormFilterKH() {
            var idkh = jQuery.trim($("#<%= txtIDKH.ClientID %>").val());
            var tenkh = jQuery.trim($("#<%= txtTENKH.ClientID %>").val());
            var madh = jQuery.trim($("#<%= txtMADH.ClientID %>").val());
            var sohd = jQuery.trim($("#<%= txtSOHD.ClientID %>").val());
            var sonha = jQuery.trim($("#<%= txtSONHA.ClientID %>").val());
            var tendp = jQuery.trim($("#<%= txtTENDP.ClientID %>").val());
            var makv = jQuery.trim($("#<%= ddlKHUVUC.ClientID %>").val());

            if (idkh == '' && tenkh == '' && madh == '' &&
                    sohd == '' && sonha == '' && tendp == '' && (makv == '' || makv == 'NULL')) {
                showError('Chọn tối thiểu một thông tin để lọc khách hàng.', '<%= txtIDKH.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterKH) %>', '');
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

        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
            return false;
        }
        
        function CheckFormTIMHONGHEON() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btTIMHONGHEON) %>', '');
            return false;
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

	</script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divKhachHangDlgContainer">
        <div id="divKhachHang" style="display: none">
            <asp:UpdatePanel ID="upnlKhachHang" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="3" cellspacing="1" style="width: 610px;">
                        <tbody>
                            <tr>
                                <td>
                                    <asp:Panel ID="headerPanel" runat="server" CssClass="crmcontainer">
                                        <table class="crmtable">
                                            <tbody>
                                                <tr class="crmfilter">
                                                    <td class="crmcell">
                                                        <div class="wrap">
                                                            <asp:ImageButton ID="imgCollapse" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                                                AlternateText="Hiện bộ lọc" />
                                                        </div>
                                                        <div class="wrap">
                                                            <asp:Label ID="lblCollapse" runat="server">Click vào để hiển thị bộ lọc</asp:Label>
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>    
                                    </asp:Panel>
                                    <asp:Panel ID="contentPanel" runat="server" CssClass="crmcontainer cleantop">
                                        <table class="crmtable">
                                            <tbody>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Mã khách hàng
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtIDKH" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                            <div class="rightsmall">Tên khách hàng</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:TextBox ID="txtTENKH" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Mã đồng hồ
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtMADH" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                            <div class="rightsmall">Số hợp đồng</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:TextBox ID="txtSOHD" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Số nhà
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:TextBox ID="txtSONHA" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>
                                                        <div class="left width-100 pleft-50">
                                                           <div class="rightsmall">Tên đường phố</div>
                                                        </div>
                                                        <div class="left">
                                                            <asp:TextBox ID="txtTENDP" runat="server" onchange="return CheckFormFilterKH();" CssClass="width-150" MaxLength="200" />
                                                        </div>  
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall">
                                                        Khu vực
                                                    </td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:DropDownList ID="ddlKHUVUC" runat="server"></asp:DropDownList>
                                                        </div>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td class="crmcell rightsmall"></td>
                                                    <td class="crmcell">
                                                        <div class="left">
                                                            <asp:Button ID="btnFilterKH" OnClick="btnFilterKH_Click"
                                                                UseSubmitBehavior="false" OnClientClick="return CheckFormFilterKH();" 
                                                                runat="server" CssClass="filter" Text="" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </tbody>
                                        </table>
                                    </asp:Panel>
                                    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="Server" 
                                        Collapsed="false"
                                        TargetControlID="contentPanel"
                                        ExpandControlID="headerPanel" 
                                        CollapseControlID="headerPanel" 
                                        TextLabelID="lblCollapse"
                                        ImageControlID="imgCollapse" 
                                        ExpandedText="Click vào để ẩn bộ lọc" 
                                        CollapsedText="Click vào để hiển thị bộ lọc"
                                        ExpandedImage="~/content/images/icons/collapsed.png" 
                                        CollapsedImage="~/content/images/icons/expanded.png" />
                                </td>
                            </tr>
                            <tr>
                                <td class="ptop-10" id="tdDanhSach" runat="server" visible="false">
                                    <div class="crmcontainer">                                   
                                        <eoscrm:Grid ID="gvDanhSach" runat="server" UseCustomPager="true" 
							                OnPageIndexChanging="gvDanhSach_PageIndexChanging" 
							                OnRowDataBound="gvDanhSach_RowDataBound" OnRowCommand="gvDanhSach_RowCommand">
                                            <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                                            <Columns>
                                                <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã KH">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lnkBtnID" runat="server" CommandName="SelectSODB" 
                                                            CommandArgument='<%# Eval("IDKH") %>'
                                                            Text='<%# Eval("MADP") + Eval("DUONGPHU").ToString() + Eval("MADB") %>'></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField HeaderStyle-Width="35%" DataField="TENKH" HeaderText="Tên khách hàng" />
                                                <asp:TemplateField HeaderStyle-Width="55%" HeaderText="Địa chỉ">
                                                    <ItemTemplate>
                                                        <%# Eval("SONHA") != null ? Eval("SONHA") + ", " : "" %>
                                                        <%# Eval("DUONGPHO") != null ? Eval("DUONGPHO.TENDP") + ", " : "" %>
                                                        <%# Eval("KHUVUC") != null ? Eval("KHUVUC.TENKV") : "" %>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </eoscrm:Grid>
                                    </div>
                                </td>
                            </tr>
                        </tbody>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    
    <div id="divGIAHANHNNlgContainer">
        <div id="divGIAHANHNN" style="display: none">
            <asp:UpdatePanel ID="upnLSGIAHANHNN" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvLSGIAHANHNN" runat="server" UseCustomPager="true" 							            
							            OnPageIndexChanging="gvLSGIAHANHNN_PageIndexChanging">
                                        <PagerSettings FirstPageText="hộ nghèo" PageButtonCount="2" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="SODB" HeaderText="Danh số" />
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="TENKH" HeaderText="Lý do đổi" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="NGAYCAPHN" HeaderText="Ngày" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="NGAYKETTHUCHN" HeaderText="Ngày" />                                            
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
                            <td class="crmcell right">Danh bộ</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="60px" MaxLength="11" 
                                        TabIndex="1" ReadOnly="True" Visible="False" />          
                                    <asp:Label ID="lbMADDK" runat="server" Visible="False" ></asp:Label>                          
                                    <asp:Label ID="lbDANHSO" runat="server" ForeColor="#3333CC" ></asp:Label>   
                                    <asp:Label ID="lbMANGHEO" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="reloadm" runat="server" Visible="False"></asp:Label>                             
                                    <asp:Button ID="btnKHACHHANG" runat="server" CssClass="addnew" 
		                                OnClick="btnKHACHHANG_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                OnClientClick="openDialogAndBlock('Chọn khách hàng', 700, 'divKhachHang')" 
                                        TabIndex="2"  />
                                </div>
                                                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbTENKHCU" runat="server" Text="Tên kh" ForeColor="#3333CC"></asp:Label>
                                </div>                                 
                            </td>            
                        </tr>
                        <tr>
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbTENMDSD" runat="server" Text="Mục đích sử dụng" ForeColor="#3333CC"></asp:Label>    
                                </div>     
                                <div class="left">
                                    <asp:Label ID="lbMAMDSD" runat="server" Visible="False" ></asp:Label>
                                    <asp:Label ID="lbMANV" runat="server" Visible="False" ></asp:Label>
                                </div>         
                                <td class="crmcell right">Khu vực</td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:DropDownList ID="ddlKHUVUCMOI" runat="server" OnSelectedIndexChanged="ddlKHUVUCMOI_SelectedIndexChanged"></asp:DropDownList>
                                    </div> 
                                </td>                     
                            </td>          
                        </tr> 
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Sổ hộ nghèo
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="ckISHONGHEO" runat="server" TabIndex="34" OnCheckedChanged="ckISHONGHEO_CheckedChanged" AutoPostBack="True" />                                
                                </div>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Kỳ hổ trợ</td>
                            <td class="crmcell">                                    
                                <div class="left">
                                    <asp:TextBox ID="txtKYHOTROHN" runat="server" Width="20px" MaxLength="2"    TabIndex="41" />
                                    <asp:TextBox ID="txtNAMHOTRO" runat="server" Width="60px" MaxLength="4"    TabIndex="41" />                                
                                    <asp:Button ID="btnSearch" runat="server" CssClass="filter" OnClick="btnSearch_Click"
                                        TabIndex="20" UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" />
                                    <asp:Label ID="lbGIAHANHN" runat="server" Visible="False" Font-Bold="True" ForeColor="#3366FF"></asp:Label>
                                </div>                           
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ khách hàng</td>
                            <td class="crmcell">
                                <div class="left">                                    
                                        <asp:TextBox ID="txtDIACHIHN" runat="server" Width="200px" MaxLength="300" />
                                 </div>                                      
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã sổ HN</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMASOHN" runat="server" Width="80px" MaxLength="100" TabIndex="42" OnTextChanged="txtMASOHN_TextChanged"/>
                                </div>
                                <td class="crmcell right">Đơn vị cấp sổ HN</td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:TextBox ID="txtDONVICAP" runat="server" Width="10px" MaxLength="100" TabIndex="43" Visible="false" />                                        
                                    </div>   
                                    <div class="left">
                                         <asp:DropDownList ID="ddlTENXA" Width="180px" runat="server" TabIndex="14" Enabled="False" AutoPostBack="True" OnSelectedIndexChanged="ddlTENXA_SelectedIndexChanged">
                                         </asp:DropDownList>
                                    </div>                                                        
                                </td>                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày ký sổ HN</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKYSOHN" runat="server" Width="100px" MaxLength="20" TabIndex="44" Enable="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="ImageButton3" ImageUrl="~/content/images/icons/calendar.png" Visible="false"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtNGAYKYSOHN"
                                        PopupButtonID="imgNGAYKYSOHN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>                                
                            </td>                            
                        </tr> 
                        <tr>
                            <td class="crmcell right">Ngày cấp sổ HN</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAPCAPHN" runat="server" Width="100px" MaxLength="20" TabIndex="45" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="ImageButton1" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="false" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtNGAPCAPHN"
                                        PopupButtonID="imgNGAPCAPHN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <td class="crmcell right">Ngày kết thúc HN</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtNGAYKTHN" runat="server" Width="90px" MaxLength="20" TabIndex="46" />
                                    </div>  
                                    <div class="left">
                                        <asp:ImageButton runat="Server" ID="ImageButton2" ImageUrl="~/content/images/icons/calendar.png" Visible="false"
                                            AlternateText="Click to show calendar" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtNGAYKTHN"
                                            PopupButtonID="imgNGAYKTHN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                    </div>                                  
                                </td>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:LinkButton ID="lkLSGIAHANN" runat="server" OnClick="lkLSGIAHANN_Click"
                                        OnClientClick="openDialogAndBlock('Lược sử gia hạn hộ nghèo', 700, 'divGIAHANHNN')">
                                        Lược sử gia hạn hộ nghèo</asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="13" UseSubmitBehavior="false" />
                                </div>                                
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClientClick="return CheckFormCancel();" 
                                        OnClick="btnCancel_Click" TabIndex="14" UseSubmitBehavior="false" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Báo cáo danh sách hộ nghèo.
                                </div>                                                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Kỳ hổ trợ</td>
                            <td class="crmcell">                                    
                                <div class="left">
                                    <asp:TextBox ID="txtTUTHANG" runat="server" Width="20px" MaxLength="2"    TabIndex="41" />
                                    <asp:TextBox ID="txtTUNAM" runat="server" Width="40px" MaxLength="4"    TabIndex="41" />  
                                    <asp:Label ID="Label1" runat="server" Text=" đến "></asp:Label>
                                    <asp:TextBox ID="txtDENTHANG" runat="server" Width="20px" MaxLength="2"    TabIndex="41" />
                                    <asp:TextBox ID="txtDENNAM" runat="server" Width="40px" MaxLength="4"    TabIndex="41" />                                
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" OnClientClick="return CheckFormReport();" runat="server" onclick="btnBaoCao_Click" CssClass="report" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
		</ContentTemplate>
	</asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upTIMHNN" runat="server" UpdateMode="Conditional">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Tìm khách hàng nghèo
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTIMHONGHEON" runat="server"  MaxLength="50"    TabIndex="41" />      
                                </div>
                                <div class="left">
                                    <asp:Button ID="btTIMHONGHEON" runat="server" CssClass="filter" OnClick="btTIMHONGHEON_Click"
                                        TabIndex="20" UseSubmitBehavior="false" OnClientClick="return CheckFormTIMHONGHEON();" />
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
                <eoscrm:Grid 
                    ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand" 
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound" PageSize="100">
                    <PagerSettings FirstPageText="hộ nghèo" PageButtonCount="2" />
                    <Columns>                    
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Danh số " HeaderStyle-Width="50px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnMANGHEO" runat="server" CommandArgument='<%# Eval("MANGHEO") %>'
                                CommandName="EditItem" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("KHACHHANG.MADP").ToString()+Eval("KHACHHANG.MADB")) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.TENKH")%>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="18%" HeaderText="Tên xã">
                            <ItemTemplate>
                                <%# Eval("XAPHUONG.TENXA")%>
                            </ItemTemplate>
                            <HeaderStyle Width="25%" />
                        </asp:TemplateField>                                                         
                        <asp:TemplateField HeaderText="Ngày cấp" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYCAPHN") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYCAPHN")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày hết hạn" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYKETTHUCHN") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYKETTHUCHN")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kỳ hổ trợ" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("KYHOTROHN") != null) ?
                                    String.Format("{0:MM/yyyy}", Eval("KYHOTROHN")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Gia hạn " HeaderStyle-Width="50px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtGIAHAN" runat="server" CommandArgument='<%# Eval("MANGHEO") %>'
                                CommandName="S_GiaHan" CssClass="link" Text ="Gia hạn"></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>       
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel> 
	<br />  
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                    DisplayGroupTree="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
