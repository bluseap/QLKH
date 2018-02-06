<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.CongNo.KiemTraHoaDonTon" CodeBehind="KiemTraHoaDonTon.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnReport) %>', '');
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
        }



        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; };
            if (code == 13) {
                return CheckFormSearch();
            }
        }
    </script>

</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">
                        Kỳ thu
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:DropDownList ID="ddlTHANGFILTER" runat="server"  TabIndex="1">
                                <asp:ListItem Text="01" Value="01" />
                                <asp:ListItem Text="02" Value="02" />
                                <asp:ListItem Text="03" Value="03" />
                                <asp:ListItem Text="04" Value="04" />
                                <asp:ListItem Text="05" Value="05" />
                                <asp:ListItem Text="06" Value="06" />
                                <asp:ListItem Text="07" Value="07" />
                                <asp:ListItem Text="08" Value="08" />
                                <asp:ListItem Text="09" Value="09" />
                                <asp:ListItem Text="10" Value="10" />
                                <asp:ListItem Text="11" Value="11" />
                                <asp:ListItem Text="12" Value="12" />
                            </asp:DropDownList>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtNAMFILTER" runat="server" Width="40px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left width-150 pleft-50" style="width: 150px;">
                            <div class="right">Mã đường</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtMADP" runat="server" onkeydown="return CheckChangeDP(event);" Width="40px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnFilter" OnClick="btnFilter_Click" OnClientClick="return CheckFormSearch();"
                                UseSubmitBehavior="false" runat="server" CssClass="filter" Text="" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Mã vạch
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtMAVACH" runat="server" CssClass="width-100" MaxLength="15" TabIndex="1"
                                OnTextChanged="txtMAVACH_TextChanged" AutoPostBack="true"></asp:TextBox>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClick="btnDelete_Click"
                                OnClientClick="return CheckFormDelete();" TabIndex="18" UseSubmitBehavior="false" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <br />
    <div class="crmcontainer" id="divThongKe" Visible="False" runat="server">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Tiền đã thu</td>
                    <td class="crmcell">
                        <div class="left width-150">
                            <div class="right"><asp:Label ID="lblDATHU" runat="server" Text=""></asp:Label></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Tiền quét tồn</td>
                    <td class="crmcell">
                        <div class="left width-150">
                            <div class="right"><asp:Label ID="lblTON" runat="server" Text=""></asp:Label></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Tổng tiền</td>
                    <td class="crmcell">
                        <div class="left width-150">
                            <div class="right"><asp:Label ID="lblTONG" runat="server" Text=""></asp:Label></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Tồn bị thiếu</td>
                    <td class="crmcell">
                        <div class="left width-150">
                            <div class="right"><asp:Label ID="lblTONTHIEU" runat="server" Text=""></asp:Label></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Chuẩn thu</td>
                    <td class="crmcell">
                        <div class="left width-150">
                            <div class="right"><asp:Label ID="lblCHUANTHU" runat="server" Text=""></asp:Label></div>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnReport" runat="server" CssClass="report" OnClick="btnReport_Click"
                                OnClientClick="return CheckFormReport();" TabIndex="18" UseSubmitBehavior="false" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:UpdatePanel ID="upnlMissingGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div class="crmcontainer" id="divMissingList" runat="server" visible="false">
                <strong>DANH SÁCH HÓA ĐƠN TỒN BỊ THIẾU</strong>
                <br/>
                <eoscrm:Grid ID="gvMissingList" runat="server" UseCustomPager="true" PageSize="5" OnPageIndexChanging="gvMissingList_PageIndexChanging"
                    AutoGenerateColumns="False">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="1%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="3%" DataField="THANG" HeaderText="Tháng" />
                        <asp:BoundField HeaderStyle-Width="3%" DataField="NAM" HeaderText="Năm" />
                        <asp:TemplateField HeaderStyle-Width="7%" HeaderText="Danh bộ">
                            <ItemTemplate>
                                <%# (Eval("KHACHHANG") != null) ?  
                                        Eval("KHACHHANG.MADP") + Eval("KHACHHANG.DUONGPHU").ToString() + Eval("KHACHHANG.MADB") 
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%#  (Eval("KHACHHANG") != null) ?  Eval("KHACHHANG.TENKH") : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="17%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.SONHA") != null && Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : ""%>
                                <%# Eval("DUONGPHO.TENDP")%>, 
                                <%# Eval("KHACHHANG.PHUONG") != null ? Eval("KHACHHANG.PHUONG.TENPHUONG") : ""%>
                            </ItemTemplate>
                            <HeaderStyle Width="17%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Số tiền" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TONGTIEN")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div class="crmcontainer" id="divCongNoList" runat="server" visible="false">
                <strong>DANH SÁCH HÓA ĐƠN TỒN QUÉT KIỂM TRA</strong>
                <br/>
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="300" OnPageIndexChanging="gvList_PageIndexChanging"
                    AutoGenerateColumns="False">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="1%">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("IDKH") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("IDKH") + "|" + Eval("THANG") + "|" +  Eval("NAM") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="1%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="3%" DataField="THANG" HeaderText="Tháng" />
                        <asp:BoundField HeaderStyle-Width="3%" DataField="NAM" HeaderText="Năm" />
                        <asp:TemplateField HeaderStyle-Width="7%" HeaderText="Danh bộ">
                            <ItemTemplate>
                                <%# (Eval("KHACHHANG") != null) ?  
                                        Eval("KHACHHANG.MADP") + Eval("KHACHHANG.DUONGPHU").ToString() + Eval("KHACHHANG.MADB") 
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%#  (Eval("KHACHHANG") != null) ?  Eval("KHACHHANG.TENKH") : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="17%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.SONHA") != null && Eval("KHACHHANG.SONHA").ToString() != "" ? Eval("KHACHHANG.SONHA") + ", " : ""%>
                                <%# Eval("DUONGPHO.TENDP")%>, 
                                <%# Eval("KHACHHANG.PHUONG") != null ? Eval("KHACHHANG.PHUONG.TENPHUONG") : ""%>
                            </ItemTemplate>
                            <HeaderStyle Width="17%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Số tiền" ItemStyle-HorizontalAlign="Right"
                            HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TONGTIEN")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
