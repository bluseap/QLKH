<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.LoaiDongHo" CodeBehind="LoaiDongHo.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">    
    <script type="text/javascript">
        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

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

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');

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
                            <td class="crmcell right">Mã loại</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMaLoai" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Nơi sản xuất</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtNSX" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right">Mô tả nơi sản xuất</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMOTANSX" runat="server" Width="250px" MaxLength="50" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Kích cỡ</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtKICHCO" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right">Mô tả kích cỡ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMOTAKC" runat="server" Width="250px" MaxLength="50" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Kiểu đồng hồ</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtKDH" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right">Mô tả kiểu đồng hồ</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMoTaKDH" runat="server" Width="250px" MaxLength="50" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Giá</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtGIA" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right">Giá đã tính thuế</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtGIAVAT" runat="server" Width="100px" MaxLength="15" TabIndex="1" />
                                </div>
                            </td>
                        </tr>                        
                        <tr>    
                            <td class="crmcell right">Chỉ số tối đa</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtCHISOMAX" runat="server" Width="100px" MaxLength="50" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right">Lưu lượng DN</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtLUULUONGDN" runat="server" Width="100px" MaxLength="50" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Lưu lượng CT</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtLUULUONGCT" runat="server" Width="100px" MaxLength="50" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <div class="right">Lưu lượng NN</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtLUULUONGNN" runat="server" Width="100px" MaxLength="50" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" Text="" TabIndex="12" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="13" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="13" OnClientClick="return CheckFormCancel();" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand" PagerStyle-HorizontalAlign="Right"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="loại đồng hồ" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MALDH") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MALDH") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã loại" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MALDH") %>'
                                    CommandName="EditItem" CssClass="link" Text='<%# Eval("MALDH") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                            <HeaderStyle Width="10%" />
                            <FooterTemplate>
                                <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                    <strong>Bỏ chọn hết</strong></a>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Kiểu đồng hồ" DataField="KDH" HeaderStyle-Width="50px" />
                        <asp:BoundField HeaderText="Kích cỡ" DataField="KICHCO" HeaderStyle-Width="50px" />
                        <asp:BoundField HeaderText="Mô tả" DataField="MOTAKC" HeaderStyle-Width="30%" />
                        <asp:BoundField HeaderText="Nơi sản xuất" DataField="MOTANSX" HeaderStyle-Width="30%" />
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
