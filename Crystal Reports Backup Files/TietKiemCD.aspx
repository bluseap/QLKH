<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="TietKiemCD.aspx.cs" Inherits="EOSCRM.Web.Forms.VayCongDoan.TietKiemCD" %>

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
                            <td class="crmcell right">Mã nhân viên vay</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="100px" MaxLength="200" TabIndex="1" ReadOnly="True"
                                     />                                    
                                </div>
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên nhân viên</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtHOTEN" runat="server" Width="250px" MaxLength="200" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" runat="server" Width="262px" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Phòng ban</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHONGBAN" Width="262px" runat="server" TabIndex="4" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Công việc</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlCONGVIEC" Width="262px" runat="server" TabIndex="5" />
                                </div>
                            </td>
                        </tr>
                         <tr>
                            <td class="crmcell right">Kỳ bắt đầu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYBATDAU" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                </div>                                
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYBATDAU"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKETTHUC" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                </div>                               
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYKETTHUC"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                <div class="left">
                                    <div class="right">Tổng số tiền đóng</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGTIENDONG" runat="server" Font-Size="Larger" ForeColor="#0066FF"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày sinh</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYSINH" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYSINH" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="7" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYSINH"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                <div class="left">
                                    <div class="right">Tổng số thu</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGTHU" runat="server" Font-Size="Larger" ForeColor="#0066FF"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI" runat="server" Width="250px" MaxLength="200" TabIndex="8" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Điện thoại</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIENTHOAI" runat="server" Width="250px" MaxLength="200" TabIndex="9" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="10" />
                                </div> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" Text="" TabIndex="11" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false" Visible="false"
                                        OnClick="btnDelete_Click" TabIndex="12" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="13" OnClientClick="return CheckFormCancel();" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="200"
                    OnRowCommand="gvList_RowCommand" OnRowDataBound="gvList_RowDataBound"
                    OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="nhân viên" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MANVV") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MANVV") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã nhân viên" HeaderStyle-Width="18%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MANVV") %>'
                                    CommandName="EditItem" CssClass="link" Text='<%# Eval("MANVV") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="HOTEN" HeaderText="Tên nhân viên" HeaderStyle-Width="40%" />
                        <asp:TemplateField HeaderText="Phòng ban" HeaderStyle-Width="40%">
                            <ItemTemplate>
                                <%# Eval("PHONGBAN.TENPB") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
