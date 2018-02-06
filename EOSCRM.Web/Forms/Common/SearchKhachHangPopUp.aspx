<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SearchKhachHangPopUp.aspx.cs"
    Inherits="EOSCRM.Web.Forms.Common.SearchKhachHangPopUp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tìm kiếm khách hang</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table id="ThongTin" runat="server">
            <tr>
                <td style="padding-right: 10px">
                    Số danh bộ:
                </td>
                <td>
                    <asp:TextBox ID="txtMAKH" runat="server" Width="60px"></asp:TextBox>
                </td>
                <td style="padding-right: 10px; padding-left: 20px">
                    Tên khách hàng:
                </td>
                <td>
                    <asp:TextBox ID="txtTENKH" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td style="padding-right: 10px; padding-left: 20px">
                    Mã đồng hồ:
                </td>
                <td>
                    <asp:TextBox ID="txtMADH" runat="server" Width="60px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="padding-right: 10px; ">
                    Địa chỉ:
                </td>
                <td colspan = "2">
                    <asp:TextBox ID="txtDiaChi" runat="server" Width="200px"></asp:TextBox>
                </td>
                <td colspan = "3">
                    <asp:DropDownList ID = "cboMADP" runat ="server"  Width = "250px" >
                    </asp:DropDownList>
                    <asp:Button ID = "btnSeach" runat ="server" Text = "Tìm kiếm"  />
                </td>
            </tr>
        </table>
    </div>
    
    <asp:GridView ID="gvList" runat="server" AllowPaging="True" AllowSorting="True"
                AutoGenerateColumns="False" CssClass="GridView" OnRowCommand="gvList_RowCommand"
                PagerStyle-HorizontalAlign="Right" 
                OnPageIndexChanging="gvList_PageIndexChanging" Width="670px">
                <PagerStyle CssClass="Pager" HorizontalAlign="Right" />
                <HeaderStyle CssClass="linkHeader" />
                <AlternatingRowStyle CssClass="ev" />
                <Columns>
                    
                    <asp:TemplateField HeaderText="IDKH&nbsp;" SortExpression="MADP" Visible ="false" >
                        <ItemTemplate>
                            <asp:LinkButton ID="linkMa" runat="server" CommandArgument='<%# Eval("IDKH") %>' CommandName="EditHoSo"
                                CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("IDKH").ToString() ) %>'></asp:LinkButton>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="True" />
                        <HeaderStyle Width="10%" />
                        <FooterTemplate>
                            <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                <strong>Bỏ chọn hết</strong></a>
                        </FooterTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MADP">
                        <ItemTemplate>
                            <asp:Label ID="lblMADP" runat="server" Text='<%# Eval("MADP") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="false" />
                        <HeaderStyle Width="3%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="DP">
                        <ItemTemplate>
                            <asp:Label ID="lblDP" runat="server" Text='<%# Eval("DUONGPHU") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="false" />
                        <HeaderStyle Width="2%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="MADB">
                        <ItemTemplate>
                            <asp:Label ID="lblMADB" runat="server" Text='<%# Eval("MADB") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="false" />
                        <HeaderStyle Width="3%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Tên khách hàng&nbsp;">
                        <ItemTemplate>
                            <asp:Label ID="lblTen" runat="server" Text='<%# Eval("TENKH") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="false" />
                        <HeaderStyle Width="30%" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Địa  chỉ&nbsp;">
                        <ItemTemplate>
                            <asp:Label ID="lblKhuVuc" runat="server" Text='<%# Eval("SONHA").ToString() + " " + Eval("HEM").ToString() + " - " +  Eval("DUONGPHO.TENDP").ToString() %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="false" />
                        <HeaderStyle Width="30%" />
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Lộ trình&nbsp;">
                        <ItemTemplate>
                            <asp:Label ID="lblLoTrinh" runat="server" Text='<%# Eval("LOTRINH") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="false" />
                        <HeaderStyle Width="10%" />
                    </asp:TemplateField>
                    
                    
                </Columns>
            </asp:GridView>         
    </form>
</body>
</html>
