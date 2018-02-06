<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="TraCuuTKPower.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.Power.TraCuuTKPower" %>

<%@ Import Namespace="EOSCRM.Util"%>
<%@ Register src="/UserControls/FilterPanel.ascx" tagname="FilterPanel" tagprefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
    <br />
    <div class="crmcontainer">    
        <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"  
            OnRowDataBound = "gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
            <PagerSettings FirstPageText="thiết kế" PageButtonCount="2" />
            <Columns>
                <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                            CommandName="EditItem" Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Font-Bold="True" />
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên khách hàng" HeaderStyle-Width="37%">
                    <ItemTemplate>
                        <%# Eval("DONDANGKYPO.TENKH")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="Lý do thiết kế" HeaderStyle-Width="25%" DataField="TENTK" />
                <asp:TemplateField HeaderText="Điện thoại" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <%# Eval("DONDANGKYPO.DIENTHOAI")%>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Địa chỉ lắp đặt" HeaderStyle-Width="35%">
                    <ItemTemplate>
                        <%# Eval("DONDANGKYPO.DIACHILD")  %> 
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày thiết kế" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <%# (Eval("NGAYLTK") != null) ?
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYLTK")) : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnIDReport" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                            CommandName="ReportItem" Text='Báo cáo'></asp:LinkButton>
                        &nbsp;&nbsp;<asp:LinkButton ID="lnkBtnIDEdit" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                            CommandName="EditItem" Text='Chỉnh sửa'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>                        
                <asp:TemplateField HeaderText="Trạng thái đơn"  HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:Button ID="imgTT" runat="server" Width="90px" OnClientClick="return false;"
                             CausesValidation="false" UseSubmitBehavior="false" />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
       </eoscrm:Grid>   
  </div>
</asp:Content>

