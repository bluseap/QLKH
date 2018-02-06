﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.NganHang" CodeBehind="NganHang.aspx.cs" %>

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
                            <td class="crmcell right">Mã ngân hàng</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMANH" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Tên ngân hàng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENNH" runat="server" Width="250px" MaxLength="200" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số tài khoản</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtSOTKCTY" runat="server" Width="100px" MaxLength="20" TabIndex="1" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Tỉnh / Thành phố</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTINHTP" runat="server" Width="250px" MaxLength="100" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI" runat="server" Width="530px" MaxLength="200" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtGHICHU" runat="server" Width="530px" MaxLength="200" TabIndex="1" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_RowDataBound" 
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="ngân hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MANH") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MANH") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã ngân hàng" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MANH") %>'
                                    CommandName="EditItem" Text='<%# Eval("MANH") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="50%" DataField="TENNH" HeaderText="Tên ngân hàng" />
                        <asp:BoundField HeaderStyle-Width="30%" DataField="TINHTP" HeaderText="Tỉnh / Thành phố" />
                        <asp:BoundField HeaderStyle-Width="150px" DataField="STKCTY" HeaderText="Số tài khoản" />
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
