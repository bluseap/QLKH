<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.SuaChua.DuyetQuyetToanSuaChua" CodeBehind="DuyetQuyetToanSuaChua.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server"> 
    <script type="text/javascript">
        function CheckFormApprove() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnApprove) %>', '');
        }

        function CheckFormReject() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnReject) %>', '');
        }
    </script>
</asp:Content>


<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <bwaco:FilterPanel ID="filterPanel" runat="server" ShowAreaCode="True" />
    <br />
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>  
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Ngày duyệt đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtApproveDate" runat="server" Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgApproveDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtApproveDate"
                                    PopupButtonID="imgApproveDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <asp:Button ID="btnApprove" CssClass="approve" runat="server" UseSubmitBehavior="false" 
                                        OnClientClick="return CheckFormApprove();" onclick="btnApprove_Click" />    
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnReject" CssClass="reject" runat="server" UseSubmitBehavior="false" 
                                        OnClientClick="return CheckFormReject();" onclick="btnReject_Click" />    
                                </div>
                            </td>      
                        </tr>
                    </tbody>
                </table>
            </div>    
            <br />    
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true"
                    OnPageIndexChanging="gvList_PageIndexChanging" OnRowDataBound="gvList_RowDataBound">
                    <PagerSettings FirstPageText="chiết tính" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="10px">
                            <HeaderTemplate>
                                <span class="checkbox">
                                    <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                        onclick="CheckAllItems(this);" />
                                </span>
                            </HeaderTemplate>
                            <ItemTemplate>
                                <span class="checkbox">
                                    <input id="Id" runat="server" type="hidden" value='<%# Eval("MADON") %>' />
                                    <input name="listIds" type="checkbox" value='<%# Eval("MADON") %>' />
                                </span>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# 
                                    (Eval("MADON") != null) ? string.Format("{0}", Eval("MADON")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên HM" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# 
                                    (Eval("TENHM") != null) ? string.Format("{0}", Eval("TENHM")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="35%">
                            <ItemTemplate>
                                <%# (Eval("DIACHIHM") != null) ?Eval("DIACHIHM"):"" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Thông tin XL" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# (Eval("TENHM") != null) ? Eval("TENHM") : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhân viên xử lý" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <%# 
                                    (Eval("NHANVIEN1") != null) ? string.Format("{0}", Eval("NHANVIEN1.HOTEN")) : ""
                                %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Ngày lập chiết tính">
                            <ItemTemplate>
                                <%# (Eval("NGAYLCT") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYLCT"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    
    <%--<eoscrm:Grid ID="gvList" runat="server" AutoGenerateColumns="False" UseCustomPager="true"
        CssClass="grid" AllowPaging="true" AllowSorting="false" OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
        <PagerSettings PreviousPageText="&laquo; previous" NextPageText="next &raquo;" PageButtonCount="3" />
        <HeaderStyle HorizontalAlign="Left" />
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
            <asp:TemplateField HeaderText="Mã đơn" SortExpression="MACB">
                <ItemTemplate>
                    <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("MADON") %>'
                        CommandName="EditHoSo" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MADON").ToString()) %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Font-Bold="True" />
                <HeaderStyle Width="5%" />
                <FooterTemplate>
                    <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                        <strong>Bỏ chọn hết</strong></a>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mã KH">
                <ItemTemplate>
                    <%# 
                        (Eval("KHACHHANG") != null) ?
                         Eval("KHACHHANG.MADP").ToString()   + Eval("KHACHHANG.DUONGPHU").ToString()   + Eval("KHACHHANG.MADB").ToString() 
                            : ""
                    %>
                </ItemTemplate>
                <ItemStyle Font-Bold="false" />
                <HeaderStyle Width="10%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tên KH">
                <ItemTemplate>
                    <%# 
                        (Eval("KHACHHANG") != null) ? Eval("KHACHHANG.TENKH").ToString()   : ""
                    %>
                </ItemTemplate>
                <ItemStyle Font-Bold="false" />
                <HeaderStyle Width="15%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Địa chỉ">
                <ItemTemplate>
                    <%# 
                        (Eval("KHACHHANG") != null) ? Eval("KHACHHANG.SONHA").ToString() != ""?
                         Eval("KHACHHANG.SONHA").ToString() + " , " + Eval("KHACHHANG.DUONGPHO.TENDP").ToString()
                                                 : Eval("KHACHHANG.DUONGPHO.TENDP").ToString() : ""
                    %>
                </ItemTemplate>
                <ItemStyle Font-Bold="false" />
                <HeaderStyle Width="20%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Thông tin khách hàng">
                <ItemTemplate>
                    <%# Eval("THONGTINKH") %>
                </ItemTemplate>
                <ItemStyle Font-Bold="false" />
                <HeaderStyle Width="20%" />
            </asp:TemplateField>
        </Columns>
    </eoscrm:Grid>--%>
    
</asp:Content>
