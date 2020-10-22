<%@ Control Language="C#" AutoEventWireup="true" 
    Inherits="EOSCRM.Web.UserControls.Header" CodeBehind="Header.ascx.cs" %>
<%--<asp:Button ID="btnHome" runat="server" CssClass="ButtonHome" OnClick="btnHome_Click"  <div id="cmenu" class="ddlevelsmenu">
    UseSubmitBehavior="false" />
<asp:Button ID="btnLogout" runat="server" CssClass="ButtonLogout" OnClick="btnLogout_Click"
    UseSubmitBehavior="false" />--%>

<div id="cheader">
    <div id="header">
        
        <div id="hlft" class="fl">
            <div class="appname">
                <%= modulename %></div>            
        </div>                     
       
        
        <div id="hlft2" class="fl">
            <marquee direction=FEFT scrollamount="2" Width="60%">
                <div class="appname2"><%= tencongty %></div>
             </marquee>
        </div>         
                  
        <div id="hrgh" class="fr">
            <asp:LinkButton ID="lnkbtnLogOut" CssClass="logout" runat="server" OnClick="lnkbtnLogOut_Click">Thoát</asp:LinkButton>
            [<%= hoten %>]
        </div>
        <div id="hrgh2" class="fr">
           <%-- <a href="http://powaco.com.vn/index.html" class="changepwd">Chat</a>--%>
            <a href="<%= ResolveUrl("~")%>Forms/HeThong/ChangePassword.aspx" class="changepwd">Pass</a>          
        </div>

            
    </div>  
    
</div>
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
                        <li <%# Container.ItemIndex == 0 ? " class=\"top\"" : "" %>>
                            <a href="<%= ResolveUrl("~") %><%# Eval("Url") %>"><%# Eval("Name") %></a>
                            <asp:Repeater ID="childRepeater2" runat="server"  OnItemDataBound="childRepeater2_ItemDataBound">
                                <HeaderTemplate>
                                    <ul>
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <li<%# Container.ItemIndex == 0 ? " class=\"top\"" : "" %>>
                                        <a href="<%= ResolveUrl("~")%><%# Eval("Url") %>"> <%# Eval("Name") %> </a>
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
    <%--<ul>
        <li><a href="#">Homepage</a></li>
        <li><a href="#">Resource</a>
            <ul>
                <li><a href="#">Reource Master List</a></li>
                <li><a href="#">Skill Set Management</a></li>
                <li><a href="#">Reource Master List</a></li>
                <li><a href="#">Skill Set Management</a></li>
                <li><a href="#">Reource Master List</a></li>
            </ul>
        </li>
        <li><a href="#">Project</a>
            <ul>
                <li><a href="#">Reource Master List</a></li>
                <li><a href="#">Skill Set Management</a></li>
            </ul>
        </li>
    </ul>--%>
</div>

<div id="ctitle">
    <div id="tpage">
        <div id="ctlft" class="fl">
            <%= titlepage %></div>        
        <%--<div id="ctrgh" class="fr">
            <div id="csearch">
                <table width="100%" border="0" cellspacing="0" cellpadding="0" height="30">
                    <tr>
                        <td class="ctsear">
                            <input type="text" value="Enter Keyword..." class="txtsear" />
                        </td>
                        <td>
                            <input type="button" value="" class="btnsear" 
                                onclick="alert('search function')" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>--%>
    </div>
</div>
