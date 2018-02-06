<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="TongHopVatTuNuoc.aspx.cs" Inherits="EOSCRM.Web.Forms.VatTu.TongHopVatTuNuoc" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>


<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Từ ngày</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTUNGAY" onkeypress="return CheckKeywordFilter(event);" runat="server"
                                        Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgToDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtTUNGAY"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <strong>đến ngày </strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDENNGAY" onkeypress="return CheckKeywordFilter(event);" runat="server"
                                        Width="90px" MaxLength="10" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="ImageButton1" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtDENNGAY"
                                    PopupButtonID="imgToDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />    
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnTim" runat="server"  CssClass="filter"
                                         TabIndex="16" UseSubmitBehavior="false" onclick="btnTim_Click" />
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
            <table class="crmcontainer">
                <tr>
                    <td class="header">PHẦN VẬT TƯ CÔNG TY</td> 
		        </tr>
		        <tr>
		            <td>
                            <div class="crmcontainer">
                                <eoscrm:Grid ID="gvListCT" runat="server" AutoGenerateColumns="false" PageSize="50" 
                                    CssClass="crmgrid" OnPageIndexChanging="gvListCT_PageIndexChanging" >
                                    <RowStyle CssClass="row" />
                                    <AlternatingRowStyle CssClass="altrow" />
                                    <HeaderStyle CssClass="header" />
                                     
                                    <PagerSettings FirstPageText="tổng hợp vật tư" PageButtonCount="2" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        <HeaderStyle CssClass="checkbox" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Mã vật tư" DataField="MAVT" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên vật tư" DataField="TENVT" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Giá vật tư" DataField="GIAVT" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Số lượng" DataField="SOLUONG" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tổng tiền" DataField="TONGTIENVT" />
                                    </Columns>
                               </eoscrm:Grid>
                            </div>
                    </td>
                 </tr>
             </table>
             <table class="crmcontainer">
                <tr>
                    <td class="header">PHẦN VẬT TƯ KHÁCH HÀNG</td> 
		        </tr>
		        <tr>
		            <td>
                            <div class="crmcontainer">
                                <eoscrm:Grid ID="gvListKH" runat="server" AutoGenerateColumns="false" PageSize="50" 
                                    CssClass="crmgrid" OnPageIndexChanging="gvListKH_PageIndexChanging" >
                                    <RowStyle CssClass="row" />
                                    <AlternatingRowStyle CssClass="altrow" />
                                    <HeaderStyle CssClass="header" />
                                    <PagerSettings FirstPageText="tổng hợp vật tư" PageButtonCount="2" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1 %>
                                            </ItemTemplate>
                                        <HeaderStyle CssClass="checkbox" />
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Mã vật tư" DataField="MAVT" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên vật tư" DataField="TENVT" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Giá vật tư" DataField="GIAVT" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Số lượng" DataField="SOLUONG" />
                                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tổng tiền" DataField="TONGTIENVT" />
                                    </Columns>
                               </eoscrm:Grid>
                            </div>
                    </td>
                 </tr>
             </table>        
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
