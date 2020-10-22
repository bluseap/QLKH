<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="ThongTinKH.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.ThongTinKH" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">        
        
        function CheckFormBAOCAO() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btBAOCAO) %>', '');
            return false;
        }

        function CheckFormLoc() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btLOC) %>', '');
            return false;
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
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Danh bộ</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="100px" MaxLength="20" 
                                        TabIndex="4" />
                                    <asp:Label ID="lbIDTTKH" runat="server" Visible="False" ></asp:Label>
                                    <asp:Label ID="reloadm" runat="server" Visible="false"></asp:Label>
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
                            <td class="crmcell right">Tên khách hàng </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKHMOI" runat="server" Width="250px" MaxLength="200" 
                                        TabIndex="5" Font-Names="Times New Roman" />
                                </div>                               
                            </td>            
                        </tr>  
                        <tr>
                            <td class="crmcell right">Địa chỉ </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHILD" runat="server" Width="250px" MaxLength="200" 
                                        TabIndex="6" />                                    
                                </div>                                            
                            </td>          
                        </tr> 
                        <tr>
                            <td class="crmcell right">Lý do </td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:DropDownList ID="ddlLOAIPHANHOI" runat="server"></asp:DropDownList>        
                                </div> 
                                 <div class="left">
                                    <div class="right">Ngày nhận thông tin</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYNHAN" 
                                        runat="server" Width="90px" MaxLength="10" TabIndex="6" />
                                </div>
                               
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtNGAYNHAN"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />                                                     
                            </td>          
                        </tr>
                        <tr>
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell">
                                <div class="left width-250">
                                    <asp:TextBox ID="txtGHICHU" runat="server" Width="300px" 
                                        TabIndex="7" Font-Names="Time" />     
                                </div>                                                      
                            </td>          
                        </tr>
                        <tr>
                    <td class="crmcell right">Từ ngày</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:TextBox ID="txtTuNgay" runat="server" Width="75px" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtTuNgay"
                            PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        <div class="left">
                            <div class="right">Đến ngày</div>
                        </div> 
                        <div class="left">
                            <asp:TextBox ID="txtDenNgay" runat="server" Width="75px" Height="16px" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                        </div>
                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDenNgay"
                            PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />   
                    </td>
                </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="8" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClientClick="return CheckFormCancel();" 
                                        OnClick="btnCancel_Click" TabIndex="9" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btLOC" runat="server" CssClass="filter" OnClientClick="return CheckFormLoc();" 
                                        TabIndex="9" UseSubmitBehavior="false" OnClick="btLOC_Click" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btBAOCAO" runat="server" CssClass="report" OnClientClick="return CheckFormBAOCAO();" 
                                        TabIndex="9" UseSubmitBehavior="false" OnClick="btBAOCAO_Click"  />
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
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound" PageSize="50">
                    <PagerSettings FirstPageText="thông tin" PageButtonCount="2" />
                    <Columns>                    
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="75px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lnkBtnIDTTKH" runat="server" CommandArgument='<%# Eval("IDTTKH") %>'
                                CommandName="EditItem" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("SODB").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>    
                        <asp:BoundField HeaderText="Tên KH" DataField="TENKH" HeaderStyle-Width="80px" />
                        <asp:BoundField HeaderText="Địa chỉ" DataField="DIACHI" HeaderStyle-Width="80px" />
                        <asp:TemplateField HeaderText="Ngày nhận" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("NGAYNHAN") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYNHAN")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Ghi chú" DataField="GHICHU" HeaderStyle-Width="200px" />                                        
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
