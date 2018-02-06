<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.CongNo.NhapCongNoNguoc" CodeBehind="NhapCongNoNguoc.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
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
                    <td class="crmcell right">
                        Kỳ thu (dùng để lọc)
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
                            <asp:TextBox ID="txtMADP" runat="server" Width="40px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnFilter" OnClick="btnFilter_Click" OnClientClick="return CheckFormSearch();"
                                UseSubmitBehavior="false" runat="server" CssClass="filter" Text="" />
                        </div>
                        <div class="left">
                            Chọn kỳ công nợ, mã đường phố, lọc ra danh sách còn tồn. Kiểm tra xong, thêm thông tin ngày thu, số phiếu rồi bấm lưu.    
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
                        <div class="left width-150 pleft-50">
                            <div class="right">Hình thức thanh toán</div>
                        </div>
                        <div class="left">
                            <asp:DropDownList ID="ddlHTTT" Width="112px" runat="server" ></asp:DropDownList>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">
                        Ngày thu công nợ
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:TextBox ID="txtNGAYCN" runat="server" Width="70px" MaxLength="15" TabIndex="1"></asp:TextBox>
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYCN"
                                PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        </div>
                        <div class="left width-150 pleft-50" style="width: 150px;">
                            <div class="right">Số phiếu</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtSOPHIEU" runat="server" Width="100px" MaxLength="15" TabIndex="1"></asp:TextBox>
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
                        <div class="left">
                            <asp:Button ID="btnSave" runat="server" CssClass="save" OnClick="btnSave_Click"
                                OnClientClick="return CheckFormSave();" TabIndex="18" UseSubmitBehavior="false" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div class="crmcontainer" id="divCongNoList" runat="server" visible="false">
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
