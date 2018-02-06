<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.DuongPho" CodeBehind="DuongPho.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>

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

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

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
                            <td class="crmcell right">Mã đường phố</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" runat="server" Width="50px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" Width="50px" MaxLength="1" TabIndex="2" />
                                </div>
                                <div class="left filtered">
                                </div>
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên đường phố</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENDP" runat="server" Width="250px" MaxLength="100" TabIndex="3" />
                                </div>
                                <div class="left filtered">
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên tắt</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENTAT" runat="server" Width="250px" MaxLength="100" TabIndex="4" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="cboMAKV" runat="server" Width="262px" TabIndex="5">
                                    </asp:DropDownList>
                                </div>
                                <div class="left filtered">
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ngày ghi</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYGHI" runat="server" Width="70px" MaxLength="50" TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYGHI" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="7" />
                                </div>
                                <div class="left filtered">
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNGAYGHI" runat="server" TargetControlID="txtNGAYGHI"
                                    PopupButtonID="imgNGAYGHI" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tỷ lệ phụ thu</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTLPHUTHU" runat="server" Width="70px" MaxLength="50" TabIndex="8" />
                                </div>
                                <div class="left filtered">
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Nông thôn</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:CheckBox ID="chkNONGTHON" runat="server" TabIndex="9" />
                                </div>
                                <div class="left">
                                    <div class="right width-200">Không phí môi trường</div>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="chkKOPHIMT" runat="server" TabIndex="10" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Nhân viên ghi</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlMANVG" runat="server" TabIndex="11" />
                                </div>
                                <div class="left">
                                    <div class="right width-200">Nhân viên thu</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlMANVT" runat="server" TabIndex="12" />
                                </div>
                            </td>
                        </tr>
                        
                        <tr>    
                            <td class="crmcell right">Bước nhảy</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtBUOCNHAY" runat="server" Width="70px" Text="20" MaxLength="50" TabIndex="13" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tính giá khác</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:CheckBox ID="cbGIAKHAC" runat="server" TabIndex="14" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Giá sinh hoạt</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtGIASH" runat="server" Width="70px" MaxLength="50" TabIndex="15" />
                                </div>
                                <div class="left">
                                    <div class="right width-150">Giá cơ quan</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtGIACQ" runat="server" Width="70px" MaxLength="50" TabIndex="16" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Giá kinh doanh</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtGIAKD" runat="server" Width="70px" MaxLength="50" TabIndex="17" />
                                </div>
                                <div class="left">
                                    <div class="right width-150">Giá hộ nghèo</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtGIAHN" runat="server" Width="70px" MaxLength="50" TabIndex="18" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" TabIndex="19" />
                                </div> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" TabIndex="20" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="21" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="22" OnClientClick="return CheckFormCancel();" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="50" OnRowDataBound="gvList_RowDataBound"
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADP") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đường" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" OnClientClick="openWaitingDialog();unblockWaitingDialog();" 
                                    runat="server" CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADP") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Đường phụ" DataField="DUONGPHU" HeaderStyle-Width="8%" />
                        <asp:BoundField HeaderText="Tên đường" DataField="TENDP" HeaderStyle-Width="42%" />
                        <asp:TemplateField HeaderText="Khu vực&nbsp;" HeaderStyle-Width="40%">
                            <ItemTemplate>
                                <%# Eval("KHUVUC.TENKV") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
