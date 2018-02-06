<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="DongHoPo.aspx.cs" Inherits="EOSCRM.Web.Forms.DanhMuc.DongHoPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divDonKhachHang").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDonKhachHangDlgContainer");
                }
            });
        });

        function CheckFormXuatExcel() {
            //unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnXuatExcel) %>', '');
            return false;
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
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

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');
            return false;
        }

        function clickButton(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=ddlMALDH.ClientID %>').focus();
                return false;
            }
        }

        function clickButtonddlMALDH(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtSONO.ClientID %>').focus();
                return false;
            }
        }

        function clickButtonddltxtSONO(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtSXTAI.ClientID %>').focus();
                return false;
            }
        }

        function clickButtontxtSXTAI(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtNAMSX.ClientID %>').focus();
                return false;
            }
        }

        function clickButtontxtNAMSX(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtTEMKD.ClientID %>').focus();
                return false;
            }
        }

        function clickButtontxtTEMKD(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtHANKD.ClientID %>').focus();
                return false;
            }
        }

        function clickButtontxtHANKD(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtNGAYKD.ClientID %>').focus();
                return false;
            }
        }

        function clickButtontxtNGAYKD(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=txtTENCTKD.ClientID %>').focus();
                return false;
            }
        }

        function clickButtontxtTENCTKD(e) {
            var evt = e ? e : window.event;
            if (evt.keyCode == 13) {
                $('#<%=btnSave.ClientID %>').focus();
                return false;
            }
        }

        
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDonKhachHangDlgContainer">
        <div id="divDonKhachHang" style="display: none">
            <asp:UpdatePanel ID="upDonKhachHang" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">
                                                Mã đơn
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtMADDKTT" runat="server" />
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Tên khách hàng
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtTENKHTT" runat="server" />
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Danh số
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtDANHSOTT" runat="server" />
                                                </div>
                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Tên KH khai thác
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtTENKHKTTT" runat="server" />
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
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtSOKD" runat="server" Width="100px" MaxLength="20" Visible="false"
                                        TabIndex="1" Enabled="true" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMADH" runat="server" Width="100px" MaxLength="10" 
                                        TabIndex="99" Enabled="False" Visible="False" />
                                </div>                                                                
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại đồng hồ</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlMALDH" runat="server" Width="112px" TabIndex="2" />                
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" Width="164px" 
                                        runat="server" TabIndex="10"                                         
                                      >
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số No</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtSONO" runat="server" Width="100px" MaxLength="20" 
                                        TabIndex="3" />                                
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSXTAI" runat="server" Width="100px" MaxLength="100" TabIndex="4" Visible="false"/>
                                </div>                                
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNAMSX" runat="server" Width="80px" MaxLength="4" TabIndex="5" Visible="false"/>
                                </div>
                            </td>                                
                        </tr>
                        <tr>
                            <td class="crmcell right">Công suất ĐH điện</td>
                            <td class="crmcell">
                                <div class="left">
                                     <asp:DropDownList ID="ddlCONGSUATD" Width="120px" runat="server" TabIndex="21">                                        
                                        <asp:ListItem Text="5(20)" Value="5(20)" />
                                        <asp:ListItem Text="5(100)" Value="5(100)" />
                                        <asp:ListItem Text="10(30)" Value="10(30)" />
                                        <asp:ListItem Text="10(40)" Value="10(40)" />
                                        <asp:ListItem Text="10(80)" Value="10(80)" />
                                        <asp:ListItem Text="10x3(100)" Value="10x3(100)" />
                                        <asp:ListItem Text="20(80)" Value="20(80)" />
                                        <asp:ListItem Text="40(120)" Value="40(120)" />
                                         <asp:ListItem Text="30(60)" Value="30(60)" />
                                         <asp:ListItem Text="5(15)" Value="5(15)" />
                                    </asp:DropDownList>
                                </div>                              
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đã sử dụng</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:CheckBox ID="chkDASD" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTEMKD" runat="server" Width="100px" MaxLength="20" Visible="false"
                                        TabIndex="6" Enabled="true" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtHANKD" runat="server" Width="70px" MaxLength="7" TabIndex="7" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgHANKD" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="False" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtHANKD"
                                        PopupButtonID="imgHANKD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKD" runat="server" Width="70px" MaxLength="10" TabIndex="8" Visible="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYKD" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="false"/>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtNGAYKD"
                                        PopupButtonID="imgNGAYKD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENCTKD" runat="server" Width="700px" MaxLength="200" TabIndex="9" Visible="false"/>
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbNGAYNHAPDH" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNAMTT" runat="server" Width="100px" MaxLength="4" Visible="false" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYNK" runat="server" Width="70px" MaxLength="10" Visible="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYNK" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="false" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYNK"
                                        PopupButtonID="imgNGAYNK" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>                                 
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYXK" runat="server" Width="70px" MaxLength="4" Visible="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYXK" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="false"/>
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYXK"
                                        PopupButtonID="imgNGAYXK" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTRANGTHAI" runat="server" Width="100px" MaxLength="2"  Visible="false"/>
                                </div> 
                                <div class="left">
                                   <div class="right"> </div>
                                </div>
                                
                            </td>
                        </tr>
                         <tr>    
                            <td class="crmcell right">Từ ngày</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtFromDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtFromDate"
                                    PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <strong>Đến</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtToDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtToDate"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                 <div class="left">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="filter"
                                        OnClick="btnFilter_Click" OnClientClick="return CheckFormFilter();" TabIndex="15" UseSubmitBehavior="false" />
                                </div>  
                                <div class="left">
                                        <asp:Button ID="btnXuatExcel" runat="server" CssClass="myButton"  OnClientClick="return CheckFormXuatExcel();"                                                                    
                                         Visible="true"  UseSubmitBehavior="false" TabIndex="1" Text="Xuất Excel" OnClick="btnXuatExcel_Click"  />
                                </div>
                            </td>
                         </tr>            
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <%--<div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div> 
                                --%><div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" Text="" TabIndex="10" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="11" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="12" OnClientClick="return CheckFormCancel();" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div> 
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnXuatExcel" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" 
                    OnRowCommand="gvList_RowCommand" OnRowDataBound="gvList_RowDataBound"
                    OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="đồng hồ" PageButtonCount="2" />
                    <Columns>
                       <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADHPO") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADHPO") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đồng hồ" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADHPO") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADHPO") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Loại đồng hồ" DataField="MALDHPO" HeaderStyle-Width="150px" />
                        <asp:BoundField HeaderText="Số No" DataField="SONO" HeaderStyle-Width="150px" />
                        <asp:TemplateField HeaderText="Sử dụng" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <%# Eval("DASD") != null ? (Convert.ToString(Eval("DASD")) == "True" ? "Đã dùng" : "Chưa" ) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Năm sản xuất" DataField="NAMSX" HeaderStyle-Width="100px" />
                        <asp:TemplateField HeaderText="Ngày nhập" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# (Eval("NGAYNHAP") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYNHAP"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkbDonKhachHang" runat="server" CommandArgument='<%# Eval("MADHPO") %>'
                                    CommandName="DonKhachHang" CssClass="link" Text='Thông tin'
                                     OnClientClick="openDialogAndBlock('Thông khách hàng', 750, 'divDonKhachHang')"               
                                    >
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

    <div >                
                        <asp:GridView ID="gvXuat" runat="server" AllowPaging="True" Visible="false"
                            BackColor="White" BorderColor="#3366CC" BorderStyle="None" BorderWidth="1px" 
                            CellPadding="4" onpageindexchanging="gvXuat_PageIndexChanging">
                            <RowStyle BackColor="White" ForeColor="#003399" />
                            <FooterStyle BackColor="#99CCCC" ForeColor="#003399" />
                            <PagerStyle BackColor="#99CCCC" ForeColor="#003399" HorizontalAlign="Left" />
                            <SelectedRowStyle BackColor="#009999" Font-Bold="True" ForeColor="#CCFF99" />
                            <HeaderStyle BackColor="#003399" Font-Bold="True" ForeColor="#CCCCFF" />
                        </asp:GridView>
             </div>
</asp:Content>
