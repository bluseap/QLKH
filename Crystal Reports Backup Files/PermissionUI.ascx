<%@ Control Language="C#" AutoEventWireup="true" Inherits="EOSCRM.Web.Forms.Group.PermissionUI" Codebehind="PermissionUI.ascx.cs" %>

<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<div class="crmcontainer">
    <eoscrm:Grid ID="grdPermission" runat="server" PageSize="1000"
        OnPageIndexChanging="grdPermission_PageIndexChanging"  ShowFooter="True">
        <PagerSettings FirstPageText="chức năng" PageButtonCount="2" />
        <Columns>
            <asp:TemplateField HeaderStyle-CssClass="checkbox">
                <ItemTemplate>
                    <input id="chkFunctionId" onclick="CheckAllInRow(this);" runat="server" type="checkbox"
                        value='<%# Eval("Value") %>' />
                </ItemTemplate>
                <FooterTemplate>
                    <img src="../../content/images/common/arrow.png" />
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Tên chức năng">
                <ItemTemplate>
                    <%# Eval("Text") %>
                </ItemTemplate>
                <ItemStyle Font-Bold="True" />
                <HeaderStyle Width="78%" HorizontalAlign="Left" />
                <FooterTemplate>
                    <a href="#" onclick="SelectAll('chkFunctionId,chkRead,chkInsert,chkUpdate,chkDelete', true); return false;">
                        <strong>Chọn hết</strong></a> / <a href="#" onclick="SelectAll('chkFunctionId,chkRead,chkInsert,chkUpdate,chkDelete', false); return false;">
                            <strong>Bỏ chọn hết</strong></a>
                </FooterTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Xem">
                <ItemTemplate>
                    <input id="chkRead" runat="server" onclick="UpdateRowState(this,'chkFunctionId',1);"
                        type="checkbox" value="1" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle Width="5%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Thêm">
                <ItemTemplate>
                    <input id="chkInsert" runat="server" onclick="UpdateRowState(this,'chkFunctionId',1);"
                        type="checkbox" value="2" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle Width="5%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Sửa">
                <ItemTemplate>
                    <input id="chkUpdate" runat="server" onclick="UpdateRowState(this,'chkFunctionId',1);"
                        type="checkbox" value="3" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle Width="5%" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Xóa">
                <ItemTemplate>
                    <input id="chkDelete" runat="server" onclick="UpdateRowState(this,'chkFunctionId',1);"
                        type="checkbox" value="4" />
                </ItemTemplate>
                <ItemStyle HorizontalAlign="Center" />
                <HeaderStyle Width="5%" />
            </asp:TemplateField>
        </Columns>
    </eoscrm:Grid>
</div>