<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.CongNo.PhatSinhTang" CodeBehind="PhatSinhTang.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        function CheckFormSearch() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnInPhieu) %>', '');
        }

        OnClientClick = "return CheckFormDelete();" 
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Kỳ hóa đơn</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:DropDownList ID="ddlTHANG" Width="60px" runat="server" TabIndex="1">
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
                            <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                        </div>
                        <div class="left">
                            <div class="right">Ngày thu công nợ</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtNGAYCN" runat="server" Width="70px" MaxLength="15" TabIndex="1" />
                        </div>
                        <div class="left">
                            <asp:ImageButton runat="Server" ID="imgFromDate" ImageUrl="~/content/images/icons/calendar.png"
                                AlternateText="Click to show calendar" />
                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" TargetControlID="txtNGAYCN"
                                PopupButtonID="imgFromDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                        </div>
                    </td>
                </tr>
                <tr>    
                    <td class="crmcell right">Mã khách hàng</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:TextBox ID="txtSODB" runat="server" CssClass="width-100" MaxLength="15" TabIndex="1"
                                OnTextChanged="txtSODB_TextChanged" AutoPostBack="true" />
                        </div>
                        <div class="left">
                            <div class="right">Số phiếu</div>
                        </div>
                        <div class="left">
                            <asp:TextBox ID="txtSOPHIEU" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                        </div>
                    </td>
                </tr>
                <tr>    
                    <td class="crmcell right">Ghi chú</td>
                    <td class="crmcell">    
                        <div class="left">
                            <asp:TextBox ID="txtGhiChuCn" TextMode="MultiLine" Rows="2" runat="server" Width="382px" MaxLength="100" TabIndex="1" />
                        </div>
                    </td>
                </tr>                        
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnFilter" OnClick="btnFilter_Click" OnClientClick="return CheckFormSearch();"
                                UseSubmitBehavior="false" runat="server" CssClass="filter" Text="" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClick="btnDelete_Click"
                                OnClientClick="return CheckFormDelete();" TabIndex="18" UseSubmitBehavior="false" />
                        </div>
                        
                         <div class="left">
                            <asp:Button ID="btnInPhieu" runat="server" CssClass="report" OnClientClick="return CheckFormReport();"
                                 OnClick="btnInPhieu_Click" TabIndex="18" UseSubmitBehavior="false" />
                        </div>
                    </td>
                </tr>  
            </tbody>
        </table>
    </div>
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />            
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="20" 
                    OnPageIndexChanging="gvList_PageIndexChanging" AutoGenerateColumns="False">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="1%">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MAPS") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MAPS") %>' />
                            </ItemTemplate>
                            <HeaderStyle Width="1%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="3%" DataField="THANG" HeaderText="Tháng" />
                        <asp:BoundField HeaderStyle-Width="3%" DataField="NAM" HeaderText="Năm" />
                        <asp:BoundField HeaderText="Tên khách hàng" DataField="TENKH" HeaderStyle-Width="25%" />
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="32%">
                            <ItemTemplate>
                                <%# (Eval("DIACHI").ToString() != "" ?  Eval("DIACHI") + ", " : "") + 
                                        ((Eval("DUONGPHO") != null) ? Eval("DUONGPHO.TENDP") + ", " : "") +
                                        ((Eval("KHUVUC") != null) ? Eval("KHUVUC.TENKV") : "")
                                    %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tổng tiền" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TONGTIEN")) %>
                            </ItemTemplate>
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderStyle-Width="6%" HeaderText="NV thu">
                            <ItemTemplate>
                                <%# (Eval("NHANVIEN1") != null) ?
                                        Eval("NHANVIEN1.MANV").ToString()
                                        : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="6%" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="75px" DataField="SOPHIEUCN" HeaderText="Số phiếu" />
                        <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Ngày thu">
                            <ItemTemplate>
                                <%# (Eval("NGAYNHAPCN") != null) ? 
                                    String.Format("{0:dd/MM/yyyy}", Eval("NGAYNHAPCN")) : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="6%" />
                            <ItemStyle Font-Bold="false" />
                        </asp:TemplateField>                                
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
