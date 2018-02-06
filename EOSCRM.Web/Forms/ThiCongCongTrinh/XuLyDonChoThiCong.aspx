<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="XuLyDonChoThiCong.aspx.cs" Inherits="EOSCRM.Web.Forms.ThiCongCongTrinh.XuLyDonChoThiCong" %>
    
<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util"%>

<%@ Register src="../../UserControls/FilterPanel.ascx" tagname="FilterPanel" tagprefix="bwaco" %>
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

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
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
                            </td> 
                        </tr> 
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
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
                    OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="đơn đăng ký" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <asp:HiddenField ID="hdfId" Value='<%# Eval("MADDK") %>' runat="server" />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDK") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Mã đơn" HeaderStyle-Width="60px" DataField="MADDK" />
                        <asp:TemplateField HeaderText="Đơn vị thi công&nbsp;">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlPhongBan" runat="server" DataSource ="<%# BindPhongBan() %>"
                                    DataTextField="TENPB" DataValueField="MAPB" Width="98%" />
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên công trình" HeaderStyle-Width="25%" DataField="TENKH" />
                        <asp:BoundField HeaderText="Điện thoại" HeaderStyle-Width="60px" DataField="DIENTHOAI" />
                        <asp:BoundField HeaderText="Địa chỉ lắp đặt" HeaderStyle-Width="35%" DataField="DIACHILD" />
                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Ngày đăng ký">
                            <ItemTemplate>
                                <%# (Eval("NGAYDK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYDK"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
    <br />    
	<div class="crmcontainer p-5">
        <a href="../ThiCongCongTrinh/NhapThiCong.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
