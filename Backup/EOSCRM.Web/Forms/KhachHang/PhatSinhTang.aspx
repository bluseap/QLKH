<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.KhachHang.PhatSinhTang" CodeBehind="PhatSinhTang.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        

        function onChangeKhuVuc() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(ddlKHUVUC) %>', '');

            return false;
        }

        function CheckFormFilter() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

            return false;
        }

        function CheckFormSave() {
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

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }

            return false;
        }

        function onKhoiLuongBlur() {
            var kl = jQuery.trim($("#<%= txtKLTieuThu.ClientID %>").val());

            if (IsNumeric(kl) || parseInt(kl) > 0) {
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHiddenKhoiLuong) %>', '');
            }

            return false;
        }

        function onDonGiaBlur() {
            var kl = jQuery.trim($("#<%= txtDonGia.ClientID %>").val());

            if (IsNumeric(kl) || parseInt(kl) > 0) {
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHiddenDonGia) %>', '');
            }

            return false;
        }

        function onVATBlur() {
            var kl = jQuery.trim($("#<%= txtVAT.ClientID %>").val());

            if (IsNumeric(kl) || parseInt(kl) > 0) {
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHiddenVAT) %>', '');
            }

            return false;
        }

        function onPhiBlur() {
            var kl = jQuery.trim($("#<%= txtPhi.ClientID %>").val());

            if (IsNumeric(kl) || parseInt(kl) > 0) {
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHiddenPhi) %>', '');
            }
        }

        function onChiSoDauBlur() {
            var csd = jQuery.trim($("#<%= txtCSD.ClientID %>").val());
            var csc = jQuery.trim($("#<%= txtCSC.ClientID %>").val());

            if (IsNumeric(csd) && parseInt(csd) >= 0 && IsNumeric(csc) && parseInt(csc) >= 0) {
                var kl = 0;
                if(csc > csd)
                    kl = csc - csd;

                $("#<%= txtKLTieuThu.ClientID %>").val(kl);

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHiddenChiSoDau) %>', '');
            }

            return false;
        }

        function onChiSoCuoiBlur() {
            var csd = jQuery.trim($("#<%= txtCSD.ClientID %>").val());
            var csc = jQuery.trim($("#<%= txtCSC.ClientID %>").val());

            if (IsNumeric(csd) && parseInt(csd) >= 0 && IsNumeric(csc) && parseInt(csc) >= 0) {
                var kl = 0;
                if (csc > csd)
                    kl = csc - csd;

                $("#<%= txtKLTieuThu.ClientID %>").val(kl);

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHiddenChiSoCuoi) %>', '');
            }

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
                            <td class="crmcell right">Tháng</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="cboTHANG" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" OnSelectedIndexChanged="ddlKHUVUC_SelectedIndexChanged" 
                                        onchange="onChangeKhuVuc();" runat="server" Width="147px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:TextBox ID="txtMAPS" runat="server" Width="85px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <div class="right">Đường phố</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlDuongPho" runat="server" Width = "147px">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                       
                        <tr>
                            <td class="crmcell right">Họ tên khách hàng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtHOTEN" runat="server" Width="405px" MaxLength="200" />
                                </div>
                            </td>
                        </tr>
                        
                         <tr>
                            <td class="crmcell right">Số nhà</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI" runat="server" Width="85px" MaxLength="200" />
                                </div>
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">
                                <div class="left width-150">
                                     <asp:DropDownList ID="ddlMDSD" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Mã số thuế</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMST" runat="server" Width="85px" MaxLength="200" />
                                </div> 
                            </td>
                        </tr>
                       
                        <tr>
                            <td class="crmcell right">Chỉ số đầu</td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:TextBox ID="txtCSD" onblur="onChiSoDauBlur()" runat="server" Width="85px" MaxLength="200" Text = "0" />
                                    <asp:LinkButton ID="linkBtnHiddenChiSoDau" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHiddenChiSoDau_Click" runat="server">Update khoi luong</asp:LinkButton>
                                </div>                               
                                <div class="left">
                                    <div class="right">Chỉ số cuối</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtCSC" onblur="onChiSoCuoiBlur()" runat="server" Width="85px" MaxLength="200" Text = "0" />
                                    <asp:LinkButton ID="linkBtnHiddenChiSoCuoi" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHiddenChiSoCuoi_Click" runat="server">Update khoi luong</asp:LinkButton>
                                </div>    
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="crmcell right">Khối lượng</td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:TextBox ID="txtKLTieuThu" onblur="onKhoiLuongBlur()" runat="server" Width="85px" MaxLength="200" Text = "0" />
                                    <asp:LinkButton ID="linkBtnHiddenKhoiLuong" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHiddenKhoiLuong_Click" runat="server">Update khoi luong</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <div class="right">Đơn giá</div>
                                </div>   
                                <div class="left">                             
                                    <asp:TextBox ID="txtDonGia" onblur="onDonGiaBlur()" runat="server" Width="85px" MaxLength="200" Text = "0" />                                     
                                    <asp:LinkButton ID="linkBtnHiddenDonGia" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHiddenDonGia_Click" runat="server">Update khoi luong</asp:LinkButton>
                                </div>    
                            </td>
                        </tr>
                        
                        <tr>
                            <td class="crmcell right">
                                Thành tiền
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtThanhTien" runat="server" Width="85px" MaxLength="200" Text = "0" />
                                </div>                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Thuế VAT</td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:TextBox ID="txtVAT" onblur="onVATBlur()" runat="server" Width="85px" MaxLength="200" Text = "0" />
                                    <strong >(%)</strong>
                                    <asp:LinkButton ID="linkBtnHiddenVAT" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHiddenVAT_Click" runat="server">Update khoi luong</asp:LinkButton>    
                                </div>                               
                                <div class="left">
                                    <div class="right">Tiền thuế</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTienThue" runat="server" Width="85px" MaxLength="200" Text = "0" />                                     
                                </div>    
                            </td>
                        </tr>
                    
                        <tr>
                            <td class="crmcell right">Phí môi trường</td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:TextBox ID="txtPhi" onblur="onPhiBlur()" runat="server" Width="85px" MaxLength="200">0</asp:TextBox>
                                    <asp:LinkButton ID="linkBtnHiddenPhi" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHiddenPhi_Click" runat="server">Update khoi luong</asp:LinkButton>
                                </div>                               
                                <div class="left">
                                    <div class="right">Tiền phí</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTienPhi" runat="server" Width="85px" MaxLength="200">0</asp:TextBox>                                     
                                </div>    
                            </td>
                        </tr>
                    
                        <tr>
                            <td class="crmcell right">Tổng tiền</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTongTien" ReadOnly="true" runat="server" Width="85px" MaxLength="200">0</asp:TextBox>
                                </div>
                            </td>
                        </tr>                        
                         <tr>
                            <td class="crmcell right">Lý do</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLyDo" TextMode="MultiLine" Rows="2" runat="server" Width="405px" MaxLength="300" Text = "Phát sinh tăng" />
                                </div>
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="filter"
                                        OnClick="btnFilter_Click" OnClientClick="CheckFormFilter();" TabIndex="36" UseSubmitBehavior="false" />
                                </div>                                
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="37" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="38" UseSubmitBehavior="false" OnClientClick="CheckFormCancel();"  />
                                </div>                                
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClick="btnDelete_Click"
                                        TabIndex="39" UseSubmitBehavior="false" OnClientClick="CheckFormDelete();" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MAPS") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MAPS") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kỳ ghi" HeaderStyle-Width="5%">
                            <ItemTemplate>
                                <%# Eval("THANG") + "/" + Eval("NAM") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        <asp:TemplateField HeaderText="Mã KH" HeaderStyle-Width="6%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MAPS") %>'
                                    CommandName="EditItem" Text='<%# Eval("MAPS") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                            <FooterTemplate>
                                <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                    <strong>Bỏ chọn hết</strong></a>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên khách hàng" DataField="TENKH" HeaderStyle-Width="25%" />
                        <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="32%">
                            <ItemTemplate>
                                <%# (Eval("DIACHI").ToString() != "" ?  Eval("DIACHI") + ", " : "") + 
                                        ((Eval("DUONGPHO") != null) ? Eval("DUONGPHO.TENDP") + ", " : "") +
                                        ((Eval("KHUVUC") != null) ? Eval("KHUVUC.TENKV") : "")
                                    %> 
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Đơn giá" HeaderStyle-Width="6%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("DONGIA")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tiền nước" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TIENNUOC")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Thuế" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TIENTHUE")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Phí" HeaderStyle-Width="5%" ItemStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TIENPHI")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tổng tiền" HeaderStyle-Width="7%" ItemStyle-HorizontalAlign="Right">
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
