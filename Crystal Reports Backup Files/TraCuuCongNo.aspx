<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.CongNo.TraCuuCongNo" CodeBehind="TraCuuCongNo.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">    
    <script type="text/javascript">
        function CheckFormSearch() {
            var nam = jQuery.trim($("#<%= txtNAMFILTER.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAMFILTER.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:Panel ID="HeaderPanel" runat="server" CssClass="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr class="crmfilter">
                    <td class="crmcell" style="border-bottom: solid 1px #F1F5FB">
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
    <asp:Panel ID="ContentPanel" runat="server" CssClass="crmcontainer cleantop">
       <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Kỳ thu</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="ddlTHANGFILTER" runat="server"  TabIndex="1">
                                <asp:ListItem Text="Tất cả" Value="%" />
                                <asp:ListItem Text="01" Value="01" />
                                <asp:ListItem Text="02" Value="02" />
                                <asp:ListItem Text="03" Value="03" />
                                <asp:ListItem Text="04" Value="04" />
                                <asp:ListItem Text="05" Value="05" />
                                <asp:ListItem Text="06" Value="06" />
                                <asp:ListItem Text="07" Value="07" />
                                <asp:ListItem Text="08" Value="08" />
                                <asp:ListItem Text="09" Value="09" />
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="11" Value="11" />
                                <asp:ListItem Text="12" Value="12" />
                            </asp:DropDownList>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtNAMFILTER" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left width-150 pleft-50">
                            <div class="right">Ngày thu công nợ</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtNGAYCN" runat="server" Width="70px" MaxLength="200" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYCN"
                                PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Mã khách hàng
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtMAKH" runat="server" Width="115px" MaxLength="200" />
                        </div>                    
                        <div class="left width-150 pleft-50">
                            <div class="right">Tên khách hàng</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtTENKH" runat="server" Width="180px" MaxLength="200" />
                        </div>
                        <div class="left width-150">
                            <div class="right">Số phiếu</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtSOPHIEUCN" runat="server" Width="100px" MaxLength="50" />
                        </div>                        
                    </td>                
                </tr>
                <tr>
                    <td class="crmcell right">
                        Số nhà
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtSONHA" runat="server" Width="115px" MaxLength="200" />
                        </div>
                        <div class="left width-150 pleft-50">
                            <div class="right">Tên đường phố</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtTENDP" runat="server" Width="180px" MaxLength="200" />
                        </div>
                        <div class="left width-150">
                            <div class="right">Khu vực</div>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="ddlKHUVUC" Width="113px" AutoPostBack="false" runat="server">
                            </asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnFilter" OnClick="btnFilter_Click" OnClientClick="return CheckFormSearch();"
                                UseSubmitBehavior="false" runat="server" CssClass="filter" Text="" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClick="btnDelete_Click"
                                OnClientClick="return CheckFormDelete();" TabIndex="18" UseSubmitBehavior="false" />
                        </div>
                    </td>
                </tr>      
            </tbody>
        </table>                                    
    </asp:Panel>
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="Server" 
        TargetControlID="ContentPanel"
        ExpandControlID="HeaderPanel" 
        CollapseControlID="HeaderPanel" 
        Collapsed="False"
        TextLabelID="Text"
        ImageControlID="Image" 
        ExpandedText="Click vào để ẩn bộ lọc" 
        CollapsedText="Click vào để hiển thị bộ lọc"
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div class="crmcontainer" id="divList" runat="server" visible="false">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="20" 
                    OnPageIndexChanging="gvList_PageIndexChanging" AutoGenerateColumns="False">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="1%">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("IDKH") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("IDKH") + "|" + Eval("THANG") + "|" +  Eval("NAM") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="1%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="3%" DataField="THANG" HeaderText="Tháng" >
                            <HeaderStyle Width="3%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="3%" DataField="NAM" HeaderText="Năm" >                        
                            <HeaderStyle Width="3%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="6%" HeaderText="Danh bộ">
                            <ItemTemplate>
                                <%# (Eval("KHACHHANG") != null) ?  
                                        Eval("KHACHHANG.MADP") + Eval("KHACHHANG.DUONGPHU").ToString() + Eval("KHACHHANG.MADB") 
                                        : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="6%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%#  (Eval("KHACHHANG") != null) ?  Eval("KHACHHANG.TENKH") : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.SONHA") != null && Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : ""%>
                                <%# Eval("DUONGPHO.TENDP")%>, 
                                <%# Eval("KHACHHANG.PHUONG") != null ? Eval("KHACHHANG.PHUONG.TENPHUONG") : ""%>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField> 
                        <asp:BoundField DataField="MAHTTT" HeaderText="HTTT" HeaderStyle-Width="4%" /> 
                        <asp:BoundField DataField="SOPHIEUCN" HeaderText="Số phiếu" HeaderStyle-Width="7%" />
                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Số tiền" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TONGTIEN")) %>
                            </ItemTemplate>
                            <HeaderStyle HorizontalAlign="Right" Width="5%" />
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="6%" HeaderText="Ngày nhập">
                            <ItemTemplate>
                                <%# (Eval("NGAYNHAPCN") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYNHAPCN"))
                                        : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="6%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="6%" HeaderText="Ngày thu">
                            <ItemTemplate>
                                <%# (Eval("NGAYCN") != null) ? 
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYCN")) : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="6%" />
                            <ItemStyle Font-Bold="false" />
                        </asp:TemplateField>                       
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
