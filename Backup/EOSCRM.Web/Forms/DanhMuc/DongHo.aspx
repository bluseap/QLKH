<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.DongHo" CodeBehind="DongHo.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
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
		
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">Số chứng nhận kiểm định</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtSOKD" runat="server" Width="100px" MaxLength="20" 
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
                                    <asp:DropDownList ID="ddlMALDH" runat="server" Width="112px" TabIndex="10" />                
                                </div>
                                <div class="left filtered"></div> 
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số No</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtSONO" runat="server" Width="100px" MaxLength="20" 
                                        TabIndex="1" />                                
                                </div>
                                <div class="left">
                                    <div class="right">Sản xuất tại</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSXTAI" runat="server" Width="100px" MaxLength="100" TabIndex="1" />
                                </div>                                
                                <div class="left">
                                    <div class="right">Năm sản xuất</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNAMSX" runat="server" Width="80px" MaxLength="4" TabIndex="1" />
                                </div>
                            </td>                                
                        </tr>
                        <tr>
                            <td class="crmcell right">Số tem kiểm định</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTEMKD" runat="server" Width="100px" MaxLength="20" 
                                        TabIndex="1" Enabled="true" />
                                </div>
                                <div class="left">
                                    <div class="right">Hết hạn kiểm định</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtHANKD" runat="server" Width="70px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgHANKD" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtHANKD"
                                        PopupButtonID="imgHANKD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left">
                                    <div class="right">Ngày KĐ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKD" runat="server" Width="70px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYKD" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtNGAYKD"
                                        PopupButtonID="imgNGAYKD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên công ty kiểm định</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENCTKD" runat="server" Width="700px" MaxLength="200" TabIndex="1" />
                                </div>
                                <div class="left filtered"></div> 
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Năm TT</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNAMTT" runat="server" Width="100px" MaxLength="4" TabIndex="1" />
                                </div>
                                <div class="left filtered"></div> 
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ngày nhập kho</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYNK" runat="server" Width="70px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYNK" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYNK"
                                        PopupButtonID="imgNGAYNK" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <div class="left filtered"></div> 
                                <div class="left">
                                    <div class="right">Ngày xuất kho</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYXK" runat="server" Width="70px" MaxLength="4" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYXK" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYXK"
                                        PopupButtonID="imgNGAYXK" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Trạng thái</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTRANGTHAI" runat="server" Width="100px" MaxLength="2" TabIndex="1" />
                                </div>                                
                                <div class="left filtered"></div>
                                <div class="left">
                                   <div class="right"> </div>
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
                                <%--<div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div> 
                                --%><div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" Text="" TabIndex="12" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="13" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="13" OnClientClick="return CheckFormCancel();" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
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
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADH") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADH") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đồng hồ" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADH") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADH") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Loại đồng hồ" DataField="MALDH" HeaderStyle-Width="150px" />
                        <asp:BoundField HeaderText="Số No" DataField="SONO" HeaderStyle-Width="150px" />
                        <asp:BoundField HeaderText="Năm sản xuất" DataField="NAMSX" HeaderStyle-Width="100px" />
                        <asp:TemplateField HeaderText="Ngày nhập kho" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# (Eval("NGAYNK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYNK"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
