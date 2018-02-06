<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MHeader.ascx.cs" Inherits="EOSCRM.Web.UserControls.MHeader" %>


  <div id="cmenu" class="ddsmoothmenu">
    <asp:Repeater ID="repeater" runat="server" OnItemDataBound="repeater_ItemDataBound">
        <HeaderTemplate>
            <ul>
                <li><a href="<%= ResolveUrl("~")%>">Trang chủ</a></li>
        </HeaderTemplate>
        <ItemTemplate>
            <li><a href="<%# Eval("Url") %>"><%# Eval("Name") %></a>
                <asp:Repeater ID="childRepeater" runat="server" OnItemDataBound="childRepeater_ItemDataBound">
                    <HeaderTemplate>
                        <ul>
                    </HeaderTemplate>
                    <ItemTemplate>
                        <li<%# Container.ItemIndex == 0 ? " class=\"top\"" : "" %>><a href="<%= ResolveUrl("~")%><%# Eval("Url") %>">
                            <%# Eval("Name") %></a>
                            <asp:Repeater ID="childRepeater2" runat="server"  OnItemDataBound="childRepeater2_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li<%# Container.ItemIndex == 0 ? " class=\"top\"" : "" %>><a href="<%= ResolveUrl("~")%><%# Eval("Url") %>">
                                        <%# Eval("Name") %></a>
                                        <asp:Repeater ID="childRepeater3" runat="server">
                                            <HeaderTemplate>
                                                <ul>
                                            </HeaderTemplate>
                                            <ItemTemplate>
                                                <li<%# Container.ItemIndex == 0 ? " class=\"top\"" : "" %>><a href="<%= ResolveUrl("~")%><%# Eval("Url") %>">
                                                    <%# Eval("Name") %></a>
                                            </ItemTemplate>
                                            <FooterTemplate>
                                                </ul>
                                            </FooterTemplate>
                                        </asp:Repeater>
                                </ItemTemplate>
                                <FooterTemplate>
                                    </ul>
                                </FooterTemplate>
                            </asp:Repeater>
                        </li>
                    </ItemTemplate>
                    <FooterTemplate>
                        </ul>
                    </FooterTemplate>
                </asp:Repeater>
            </li>
        </ItemTemplate>
        <FooterTemplate>
            </ul>
        </FooterTemplate>
    </asp:Repeater>
   
</div>
