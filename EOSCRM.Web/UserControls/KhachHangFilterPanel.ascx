<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="KhachHangFilterPanel.ascx.cs"
    Inherits="EOSCRM.Web.UserControls.KhachHangFilterPanel" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<script type="text/javascript">
    function CheckKeywordFilter(e) {
        var code = (e.keyCode ? e.keyCode : e.which);
        jQuery.fn.exists = function() { return jQuery(this).length > 0; }
        if (code == 13) {
            return CheckFormFilter();
        }
    }
    
    function CheckFormFilter() {
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
<asp:Panel ID="Content" runat="server" CssClass="crmcontainer cleantop">
   <table class="crmtable">
        <tbody>
            <tr>
                <td class="crmcell right">
                    Danh số
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtIDKH" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="200px" MaxLength="200" />
                    </div>
                    <div class="left" style="width: 150px;">
                        <strong>Số hợp đồng</strong>
                    </div>
                    <div class="left">
                        <asp:TextBox ID="txtSOHD" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="200px" MaxLength="200" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Số No đồng hồ
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtMADH" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="200px" MaxLength="200" />
                    </div>                    
                    <div class="left" style="width: 150px;">
                        <strong>Tên khách hàng</strong>
                    </div>
                    <div class="left">
                        <asp:TextBox ID="txtTENKH" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="400px" MaxLength="200" />
                    </div>                    
                </td>                              
            </tr>
            <tr>
                <td class="crmcell right">
                    Số nhà
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtSONHA" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="200px" MaxLength="200" />
                    </div>
                    <div class="left" style="width: 150px;">
                        <strong>Đường phố</strong>
                    </div>
                    <div class="left">
                        <asp:TextBox ID="txtTENDP" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="200px" MaxLength="200" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Số điện thoại
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtSODIENTHOAI" onkeypress="return CheckKeywordFilter(event);" runat="server" Width="200px" MaxLength="200" />
                    </div>                    
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                    Khu vực
                </td>
                <td class="crmcell">
                    <div class="left">
                        <asp:DropDownList ID="ddlKHUVUC" Width="210px" AutoPostBack="false" 
                            runat="server" >
                        </asp:DropDownList>
                    </div>
                    <div class="left" style="width: 150px;">
                        <strong>Xóa bộ</strong>
                    </div>                
                    <div class="left">
                        <asp:DropDownList ID="ddlXOABO" Width="210px" AutoPostBack="false" 
                            runat="server" Visible="true" >
                        </asp:DropDownList>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right">
                <td class="crmcell">
                    <div class="left">
                        <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                            UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                            runat="server" CssClass="filter" />
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
