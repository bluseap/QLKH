﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.CapBac" CodeBehind="CapBac.aspx.cs" %>

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
                            <td class="crmcell right">Mã cấp bậc</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMACB" runat="server" Width="50px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left width-175">
                                    <div class="right">Tên cấp bậc</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENCB" runat="server" Width="250px" MaxLength="50" TabIndex="1" />
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
                    <PagerSettings FirstPageText="cấp bậc" PageButtonCount="2" />
                    <Columns>
                       <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MACB") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MACB") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã cấp bậc" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MACB") %>'
                                    CommandName="EditItem" Text='<%# Eval("MACB") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="78%" DataField="TENCB" HeaderText="Tên cấp bậc" />
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
