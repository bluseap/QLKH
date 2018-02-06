<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.TrinhDo" CodeBehind="TrinhDo.aspx.cs" %>

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
                            <td class="crmcell right">Mã trình độ</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMATD" runat="server" Width="50px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left width-175">
                                    <div class="right">Tên trình độ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENTD" runat="server" Width="250px" MaxLength="50" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
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
                    <PagerSettings FirstPageText="trình độ" PageButtonCount="2" />
                    <Columns>
                       <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MATD") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MATD") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã trình độ" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MATD") %>'
                                    CommandName="EditItem" Text='<%# Eval("MATD") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="78%" DataField="TENTD" HeaderText="Tên trình độ" />
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>


<%--<%@ Import Namespace="EOSCRM.Util" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <table class="form" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td class="right">
                <asp:Button ID="btnAddNew" runat="server" CssClass="addnew" OnClick="btnAddNew_Click"
                    UseSubmitBehavior="false" />
                <asp:Button ID="btnUpdate" runat="server" CommandArgument="Insert" CssClass="save"
                    OnClick="btnUpdate_Click" ValidationGroup="SaveGroup" TabIndex="9" UseSubmitBehavior="false" />
                <asp:Button ID="btn_Delete" runat="server" CssClass="delete" OnClick="btn_Delete_Click"
                    OnClientClick="return deleteRecord();" UseSubmitBehavior="false" />
                <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                    TabIndex="10" UseSubmitBehavior="false" />
            </td>
        </tr>
    </table>
    <table class="form" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td class="label">
                Mã trình độ
            </td>
            <td align="left">
                <asp:TextBox ID="txtMATD" runat="server" Width="250px" MaxLength="10" TabIndex="1" />
                <div>
                    <asp:Label ID="lblMATD" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                </div>
            </td>
        </tr>
        <tr>
            <td class="label">
                Tên trình độ
            </td>
            <td align="left">
                <asp:TextBox ID="txtTENTD" runat="server" Width="250px" MaxLength="100" TabIndex="1" />
                <div>
                    <asp:Label ID="lblTENTD" runat="server" ForeColor="Red" Visible="False"></asp:Label>
                </div>
            </td>
        </tr>
    </table>
    <br />
    <asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
        CssClass="grid" OnRowCommand="gvList_RowCommand" PagerStyle-HorizontalAlign="Right"
        OnPageIndexChanging="gvList_PageIndexChanging">
        <PagerStyle CssClass="Pager" HorizontalAlign="Right" />
        <Columns>
            <asp:TemplateField HeaderStyle-CssClass="checkbox">
                <HeaderTemplate>
                    <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                        onclick="CheckAllItems(this);" />
                </HeaderTemplate>
                <ItemTemplate>
                    <input id="Id" runat="server" type="hidden" value='<%# Eval("MATD") %>' />
                    <input name="listIds" type="checkbox" value='<%# Eval("MATD") %>' />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Mã trình độ&nbsp;" SortExpression="MALDH">
                <ItemTemplate>
                    <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("MATD") %>'
                        CommandName="EditHoSo" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MATD").ToString()) %>'></asp:LinkButton>
                </ItemTemplate>
                <ItemStyle Font-Bold="True" />
                <HeaderStyle Width="10%" />
                <FooterTemplate>
                    <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                        <strong>Bỏ chọn hết</strong></a>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tên trình độ&nbsp;">
                <ItemTemplate>
                    <asp:Label ID="lblTenLoai" runat="server" Text='<%# Eval("TENTD") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Font-Bold="false" />
                <HeaderStyle Width="35%" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
<asp:UpdatePanel ID="upnlJsRunner" UpdateMode="Always" runat="server">
	    <ContentTemplate>
		    <asp:PlaceHolder ID="phrJsRunner" runat="server"></asp:PlaceHolder>	        
	    </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
--%>