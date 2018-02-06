<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="true"
    Inherits="EOSCRM.Web.Forms.Group.List" CodeBehind="List.aspx.cs" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>
        
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">    
    <script type="text/javascript">
        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddnew) %>', '');
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
        }
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnAddnew" OnClientClick="return CheckFormSave();"  runat="server" 
                                CssClass="addnew" UseSubmitBehavior="false" OnClick="btnAddnew_Click" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnDelete" UseSubmitBehavior="false" OnClick="btnDelete_Click"
                                runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();"  />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>        
    <br />
    <div class="crmcontainer">
        <eoscrm:Grid ID="grdView" runat="server" UseCustomPager="true" PageSize="50"
            OnPageIndexChanging="grdData_PageIndexChanging" OnRowCommand="grdData_RowCommand">
            <PagerSettings FirstPageText="nhóm" PageButtonCount="2" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                    <ItemTemplate>
                        <input type="hidden" id="GroupId" value='<%# Eval("Id") %>' runat="server" />
                        <input type="hidden" id="UpdateDate" value='<%# Eval("UpdateDate") %>' runat="server" />
                        <input name="listIds" type="checkbox" value='<%# Eval("Id") %>' onclick="DoCheckItem();" />
                    </ItemTemplate>
                    <HeaderTemplate>
                        <input id="chkAllTop" title="Check All / UnCheck All" name="chkAllTop" type="checkbox"
                            onclick="CheckAllItems(this);" />
                    </HeaderTemplate>
                    <FooterTemplate>
                        <input id="chkAllBottom" title="Check All / UnCheck All" name="chkAllBottom" type="checkbox"
                            onclick="CheckAllItems(this);" />
                    </FooterTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Tên nhóm">
                    <ItemTemplate>
                        <asp:LinkButton ID="linkGroupName" runat="server" CommandArgument='<%# Eval("Id") %>'
                            CommandName="EditGroup" Text='<%# HttpUtility.HtmlEncode(Eval("Name").ToString()) %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Font-Bold="True" />
                    <HeaderStyle Width="60%" HorizontalAlign="Left" />
                </asp:TemplateField>
                <asp:BoundField DataField="Active" HeaderText="Kích hoạt">
                    <HeaderStyle Width="10%" HorizontalAlign="Center" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="CreateBy" HeaderText="Người tạo">
                    <HeaderStyle Width="10%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
                <asp:BoundField DataField="UpdateBy" HeaderText="Người cập nhật">
                    <HeaderStyle Width="15%" />
                    <ItemStyle HorizontalAlign="Center" />
                </asp:BoundField>
            </Columns>
        </eoscrm:Grid>
    </div>
</asp:Content>
