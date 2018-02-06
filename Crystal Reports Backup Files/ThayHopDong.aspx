<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="ThayHopDong.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.ThayHopDong" %>

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
                    sohd == '' && sonha == '' && tendp == '' && (makv == '' || makv == 'NULL'))
            {
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
    
	<asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Danh bộ</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="60px" MaxLength="11" 
                                        TabIndex="1" ReadOnly="True" Visible="False" />          
                                    <asp:Label ID="lbMADDK" runat="server" Visible="False" ></asp:Label>                          
                                    <asp:Label ID="lbDANHSO" runat="server" ForeColor="#3333CC" ></asp:Label>   
                                    <asp:Label ID="lbIDTDH" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="reloadm" runat="server" Visible="False"></asp:Label>                             
                                    <asp:Button ID="btnKHACHHANG" runat="server" CssClass="addnew" 
		                                OnClick="btnKHACHHANG_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                OnClientClick="openDialogAndBlock('Chọn khách hàng', 700, 'divKhachHang')" 
                                        TabIndex="2"  />
                                </div>
                                                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên khách hàng cũ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbTENKHCU" runat="server" Text="Tên kh cũ" ForeColor="#3333CC"></asp:Label>
                                </div>
                                 <div class="left">
                                    <div class="right">Số hợp đồng cũ</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbSOHDCU" runat="server" Text="Số HD cũ" ForeColor="#3333CC"></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Số hợp đồng mới</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbSOHDMOI" runat="server" Text="Số HD mới" ForeColor="#3333CC"></asp:Label>
                                </div>
                            </td>            
                        </tr>
                        <tr>
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:Label ID="lbTENMDSD" runat="server" Text="Mục đích sử dụng" ForeColor="#3333CC"></asp:Label>
                                    <asp:Label ID="lbMAMDSD" runat="server" Visible="False" ></asp:Label>
                                    <asp:Label ID="lbMANV" runat="server" Visible="False" ></asp:Label>
                                </div>                                
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlKHUVUCMOI" runat="server"></asp:DropDownList>
                                </div>                      
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">Tên khách hàng mới</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKHMOI" runat="server" Width="250px" MaxLength="200" 
                                        TabIndex="2" Font-Names="Times New Roman" />
                                </div>
                                <div class="left">
                                    <div class="right">Tên uỷ quyền</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtUYQUYEN" runat="server" Width="250px" MaxLength="200" 
                                        TabIndex="3" Font-Names="Times New Roman" />
                                </div>
                            </td>            
                        </tr>  
                        <tr>
                            <td class="crmcell right">Địa chỉ lắp đặt(nhập tổ,xã)</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHILD" runat="server" Width="250px" MaxLength="200" 
                                        TabIndex="4" />                                    
                                </div> 
                                <div class="left">
                                    <div class="right">Năm sinh</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYSINH" 
                                        runat="server" Width="90px" MaxLength="4" TabIndex="6" />
                                </div>                             
                                        
                            </td>          
                        </tr>                                               
                        <tr>
                            <td class="crmcell right">CMND</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtCMND" runat="server" Width="150px" MaxLength="20" 
                                        TabIndex="5" />                                    
                                </div>
                                <div class="left">
                                    <div class="right">Cấp ngày</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCAPNGAY" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="6" />
                                </div>
                               
                                <ajaxToolkit:CalendarExtender ID="ceCAPNGAY" runat="server" TargetControlID="txtCAPNGAY"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right">Tại đâu</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtTAIDAU" runat="server" Width="150px" MaxLength="50" 
                                        TabIndex="7" />         
                                </div>
                                <div class="left">
                                    <div class="right">Nơi thường trú</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSONHAMOI" runat="server" Width="250px" MaxLength="200" 
                                        TabIndex="8" />
                                </div>                                
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right">MST</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtMST" runat="server" Width="150px" MaxLength="20" 
                                        TabIndex="9" />         
                                </div>
                                <div class="left">
                                    <div class="right">Điện thoại</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDIENTHOAI" runat="server" Width="150px" MaxLength="50" 
                                        TabIndex="10" />
                                </div>
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày lập HĐ</td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYLAPHD" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="11" />
                                </div>                                
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYLAPHD"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <div class="right">Ngày hiệu lực</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYHIEULUC" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="12" />
                                </div>                                
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYHIEULUC"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
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
                            <td class="crmcell right">Lý do thay HĐ</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtLYDOTHAYHD" runat="server" Width="300px" MaxLength="100" 
                                        TabIndex="9" Font-Names="Time" />         
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSOHOKHAU" runat="server" Width="150px" MaxLength="50" 
                                        TabIndex="10" Visible="False" />
                                </div>
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td>
                               <div class="left">
                                    <asp:Label ID="lbGHICHU" runat="server" Text="Tên khách hàng được cập nhật vào danh sách thay đổi chi tiết" Font-Bold="True" Font-Size="Large" ForeColor="#FF3300"></asp:Label>
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
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound" PageSize="70">
                    <PagerSettings FirstPageText="hợp đồng" PageButtonCount="2" />
                    <Columns>                    
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="75px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnIDTHD" runat="server" CommandArgument='<%# Eval("IDTHD") %>'
                                CommandName="EditItem" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()+Eval("MADB").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>    
                        <asp:BoundField HeaderText="Số HĐ mới" DataField="SOHDMOI" HeaderStyle-Width="80px" />
                        <asp:BoundField HeaderText="Số HĐ cũ" DataField="SOHDCU" HeaderStyle-Width="80px" />
                        <asp:TemplateField HeaderText="Tên KH mới" HeaderStyle-Width="25%">
                            <ItemTemplate>
                                <%# Eval("TENKHMOI")%>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:BoundField HeaderText="SCMND" DataField="CMND" HeaderStyle-Width="80px" />                              
                        <asp:TemplateField HeaderText="Ngày nhập" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYNHAP") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYNHAP")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="In HĐ">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkInHD" runat="server"
                                    CommandArgument='<%# Eval("IDTHD") %>'
                                    CommandName="INLAIHD"                                                                        
                                    Text='In HĐ'>
                                </asp:LinkButton>
                            </ItemTemplate>
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
