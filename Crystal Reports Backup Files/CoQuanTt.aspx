<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.CoQuanTt" CodeBehind="CoQuanTt.aspx.cs" %>

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
                            <td class="crmcell right">Mã cơ quan</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMACQ" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Tên cơ quan</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENCQ" runat="server" Width="250px" MaxLength="500" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ngân hàng</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlNGANHANG" TabIndex="3" runat="server" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Mã số thuế</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMS" runat="server" Width="100px" MaxLength="50" TabIndex="4" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">DVQHNS</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtDVQHNS" runat="server" Width="100px" MaxLength="50" TabIndex="5" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">CHUONG</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCHUONG" runat="server" Width="100px" MaxLength="50" TabIndex="6" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">NKT</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtNKT" runat="server" Width="100px" MaxLength="50" TabIndex="7" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">NDKT</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNDKT" runat="server" Width="100px" MaxLength="50" TabIndex="8" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">NUONNS</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtNUONNS" runat="server" Width="100px" MaxLength="50" TabIndex="9" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">SOTK</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSOTK" runat="server" Width="100px" MaxLength="50" TabIndex="10" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">SODT</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSODT" runat="server" Width="100px" MaxLength="50" TabIndex="11" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI" MaxLength="500" Width="500px" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" OnClientClick="return CheckFormSave();"
                                        UseSubmitBehavior="false" runat="server" CssClass="save" Text="" TabIndex="7" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="8" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="9" OnClientClick="return CheckFormCancel();" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_RowDataBound" 
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="cơ quan" PageButtonCount="2" />
                    <Columns>
                       <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MACQ") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MACQ") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã CQ" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MACQ") %>'
                                    CommandName="EditItem" Text='<%# Eval("MACQ") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="38%" DataField="TENCQ" HeaderText="Tên CQ" />
                        <asp:TemplateField HeaderText="Ngân hàng" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <%# Eval("NGANHANG") != null ? Eval("NGANHANG.TENNH") : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="38%" DataField="DIACHI" HeaderText="Địa chỉ" />
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>