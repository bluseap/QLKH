<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" 
    CodeBehind="PhanCongKhaoSatThietKe.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.PhanCongKhaoSatThietKe" %>
<%@ Import Namespace="EOSCRM.Domain"%>
<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util"%>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        function checkFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

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
    <table border="0" cellpadding="5" cellspacing="0" width="100%">
        <tr>
            <td>
                <asp:GridView ID="gvPhanCong" runat="server" AllowPaging="false" AllowSorting="false"
                    AutoGenerateColumns="False" CssClass="grid" OnRowDataBound="gvPhanCong_RowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTopGrid" title="Chọn hết / Bỏ chọn hết" name="chkAllTopGrid" type="checkbox"
                                    onclick="CheckTop(this,'<%= gvPhanCong.ClientID %>', '<%= hfPhanCong.ClientID %>');" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="MaNV" runat="server" type="hidden" value='<%# Eval("MaNV") %>' />
                                <input id="nvcheck-<%# Eval("MaNV") %>" name="nvcheck-<%# Eval("MaNV") %>" 
                                    <%# IsParentCheck(Eval("MaNV").ToString()) %> 
                                    type="checkbox" value='<%# Eval("MaNV") %>' 
                                    onclick="CheckParentItem(this, 'chkAllTopGrid', 'nvcheck-', 'div-<%# Eval("MaNV") %>', '<%= hfPhanCong.ClientID %>');" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#" HeaderStyle-Width="40px" ItemStyle-HorizontalAlign="Center">
                            <HeaderStyle Width="4%" HorizontalAlign="Center" />
                            <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" />
                            <ItemTemplate>
                                <%# (gvPhanCong.PageIndex) * gvPhanCong.PageSize + Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên nhân viên">
                            <HeaderStyle HorizontalAlign="Left" Width="18%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <a href="javascript:ToggleGroup('div-<%# Eval("MaNV") %>')"><strong>
                                    <%# HttpUtility.HtmlEncode(Eval("HoTen").ToString()) %></strong></a>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phòng ban">
                            <HeaderStyle HorizontalAlign="Left" Width="12%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <%# Eval("PHONGBAN") != null ? ((PHONGBAN)Eval("PHONGBAN")).TENPB : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Cấp bậc">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <%# Eval("CAPBAC") != null ? ((CAPBAC)Eval("CAPBAC")).TENCB : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Công việc">
                            <HeaderStyle HorizontalAlign="Left" Width="10%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <%# Eval("CONGVIEC") != null ? ((CONGVIEC)Eval("CONGVIEC")).TENCV : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phường được phân công">
                            <HeaderStyle HorizontalAlign="Left" Width="40%" />
                            <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                            <ItemTemplate>
                                <!-- Grid Multi Work Load -->
                                <div id="div-<%# Eval("MaNV") %>" <%# HasChecked(Eval("manv").ToString()) %>>
                                    <asp:GridView ID="grdMultiLoad" runat="server" CssClass="grid" AutoGenerateColumns="False"
                                        ShowHeader="false">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemStyle HorizontalAlign="Center" VerticalAlign="Top" Width="30px" />
                                                <ItemTemplate>
                                                    <input value='<%# Eval("manv") + DELIMITER.Delimiter + Eval("maphuong") %>' 
                                                        class='checkedPhuong' 
                                                        <%# IsChecked(Eval("manv").ToString(), Eval("maphuong").ToString()) %>
                                                        onclick="CheckChildItem(this, 'chkAllTopGrid', 'nvcheck-<%# Eval("MaNV") %>', 'nvcheck', 'pcheck-<%# Eval("manv") %>', 'div-<%# Eval("MaNV") %>', '<%= hfPhanCong.ClientID %>');"
                                                        name='pcheck-<%# Eval("manv")  + DELIMITER.Delimiter + Eval("maphuong") %>' type="checkbox" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Phường" HeaderStyle-Width="120px" ItemStyle-HorizontalAlign="Center">
                                                <ItemStyle HorizontalAlign="Left" VerticalAlign="Top" />
                                                <ItemTemplate>
                                                    <%# (Eval("tenkv").ToString() != "" ? Eval("tenkv") + " / " : "") 
                                                        + Eval("tenphuong") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                    </asp:GridView>
                                </div>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:HiddenField ID="hfPhanCong" runat="server" />
            </td>
        </tr>        
    </table>
    <br />
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:Button ID="btnSave" runat="server" CommandArgument="Insert" CssClass="save"
                                OnClick="btnSave_Click" UseSubmitBehavior="false" OnClientClick="return checkFormSave();"
                                TabIndex="9" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                TabIndex="10" OnClientClick="return checkFormCancel();" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    
</asp:Content>
