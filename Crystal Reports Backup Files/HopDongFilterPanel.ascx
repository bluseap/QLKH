<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="HopDongFilterPanel.ascx.cs"
    Inherits="EOSCRM.Web.UserControls.HopDongFilterPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script type="text/javascript">
    function CheckKeywordFilter(e) {
        if (window.event) {
            if (window.event.keyCode == 13) {
                window.event.keyCode = 0;
                return CheckFormFilter();
            }
        }
        else if (e.which) {
            if (window.event.keyCode == 13) {
                return CheckFormFilter();
            }
        }
    }
    
    function CheckFormFilter() {
        // check text boxes have at least one value
        var fd = jQuery.trim(document.getElementById('<%= txtFromDate.ClientID %>').value);
        var td = jQuery.trim(document.getElementById('<%= txtToDate.ClientID %>').value);

        // check date time
        if (fd != '') {
            if (isDate(fd) == false) {
                document.getElementById('<%= txtFromDate.ClientID %>').focus();
                return false;
            }
        }

        if (td != '') {
            if (isDate(td) == false) {
                document.getElementById('<%= txtToDate.ClientID %>').focus();
                return false;
            }
        }        
        
        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
    }
</script>


<asp:Panel ID="Header" runat="server" CssClass="crmcontainer">
    <table class="crmtable">
        <tbody>
            <tr class="crmfilter">
                <td class="crmcell">
                    <div class="wrap">
                        <asp:ImageButton ID="Image" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                            AlternateText="Hiện bộ lọc" />
                    </div>
                    <div class="wrap">
                        <asp:Label ID="Text" runat="server">Click vào để hiển thị bộ lọc</asp:Label>
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
    
</asp:Panel>
<asp:Panel ID="Content" runat="server" CssClass="crmcontainer">
   <table class="crmtable">
        <tbody>
            <tr>
                <td class="crmcell right">
                    Số hợp đồng
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtSOHD" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="300px" MaxLength="200" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Mã đơn
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtMADDK" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="300px" MaxLength="200" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Tên khách hàng
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtTENKH" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="300px" MaxLength="200" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Số nhà
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtSONHA" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="300px" MaxLength="200" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                   
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:DropDownList ID="ddlKHUVUC" Width="120px" AutoPostBack="false" runat="server" Visible="false">
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Ngày tạo
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtFromDate" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="90px" MaxLength="10" />
                    </div>
                    <div class="left">
                        <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                            AlternateText="Click to show calendar" />
                    </div>
                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtFromDate"
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
                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtToDate"
                        PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                <td class="crmcell">
                    <div class="left">
                        <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                            UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                            runat="server" CssClass="filter" Text="" />
                    </div>
                </td>
            </tr>      
        </tbody>
    </table>                                    
</asp:Panel>
<ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="Server" 
    TargetControlID="Content"
    ExpandControlID="Header" 
    CollapseControlID="Header" 
    Collapsed="False"
    TextLabelID="Text"
    ImageControlID="Image" 
    ExpandedText="Click vào để ẩn bộ lọc" 
    CollapsedText="Click vào để hiển thị bộ lọc"
    ExpandedImage="~/content/images/icons/collapsed.png" 
    CollapsedImage="~/content/images/icons/expanded.png"
    SuppressPostBack="true" />
