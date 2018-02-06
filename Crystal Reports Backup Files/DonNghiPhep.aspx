<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
 CodeBehind="DonNghiPhep.aspx.cs" Inherits="EOSCRM.Web.Forms.Phep_CongTac.DonNghiPhep" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlNghiPhep" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Tên nhân viên </td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lblTENNV" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Phòng, ban </div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENPHONG" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Công việc </td>
                            <td class="crmcell">    
                                <div class="left width-200">
                                    <asp:Label ID="lblCONGVIEC" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Số điện thoại </div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblSODT" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Từ ngày </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID = "txtTuNgay" runat ="server" Width="78px" TabIndex="10" />
                                </div>
                                <div class="left width-100">
                                    <asp:ImageButton runat="Server" ID="imgTUNGAY" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtTuNgay"
                                        PopupButtonID="imgTUNGAY" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left">
                                    <div class="right">Đến ngày </div>
                                </div>
                                <div class="left">
                                     <asp:TextBox ID = "txtDenNgay" runat ="server" Width="78px" 
                                         TabIndex="11" ontextchanged="txtDenNgay_TextChanged" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgDENNGAY" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtDenNgay"
                                        PopupButtonID="imgDENNGAY" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>                                                               
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa điểm </td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID = "txtNOIDUNG" runat ="server" Width="180px" TabIndex="10" />
                                </div>
                                <div class="left">
                                    <div class="right">Số ngày nghĩ </div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblNGAYNGHI" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Lý do </td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:TextBox ID = "txtLYDO" runat ="server" Width="500px" TabIndex="10" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click" 
                                        TabIndex="13" UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="19" UseSubmitBehavior="false" />
                                </div>                               
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
   </asp:UpdatePanel>
   <br />
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvNghiPhep" runat="server" UseCustomPager="true" PageSize="20"
                    OnRowCommand="gvNghiPhep_RowCommand" OnPageIndexChanging="gvNghiPhep_PageIndexChanging"
                    OnRowDataBound="gvNghiPhep_RowDataBound" 
                    onselectedindexchanged="gvNghiPhep_SelectedIndexChanged" >
                    <PagerSettings FirstPageText="nghĩ phép" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MAPHEP") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MAPHEP") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
                     
</asp:Content>
