<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.ThongTinPhanHoi" CodeBehind="ThongTinPhanHoi.aspx.cs" %>

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
                            <td class="crmcell right">Mã phản hồi</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMAPH" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Tên phản hồi</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENPH" runat="server" Width="250px" MaxLength="50" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Loại</td>
                            <td class="crmcell">  
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAI" TabIndex="4" runat="server" Width="250px" />
                                </div> 
                                <div class="left">
                                    <b>Nhóm phản hồi :</b><asp:DropDownList ID="ddlNHOM" TabIndex="3" 
                                        runat="server"/>
                                </div>
                              
                              
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Thứ tự</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtSTT" runat="server" Width="100px" MaxLength="10" TabIndex="5" />
                                </div>
                                <div class="left width-150">
                                    <div class="right"></div>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbActive" runat="server" TabIndex="6" />
                                </div>
                                <div class="left">
                                    <strong>Có hiển thị</strong>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click" OnClientClick="return CheckFormSave();"
                                        UseSubmitBehavior="false" runat="server" CssClass="save" Text="" TabIndex="7" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="8" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="9" OnClientClick="return CheckFormCancel();" />
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
                    <PagerSettings FirstPageText="phản hồi" PageButtonCount="2" />
                    <Columns>
                       <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MAPH") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MAPH") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã PH" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MAPH") %>'
                                    CommandName="EditItem" Text='<%# Eval("MAPH") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="38%" DataField="TENPH" HeaderText="Tên PH" />
                        <%--<asp:TemplateField HeaderText="Nhóm" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <%# Eval("NHOMPHANHOI") != null ? Eval("NHOMPHANHOI.TENNHOM") : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="Loại" HeaderStyle-Width="20%">
                            <ItemTemplate>
                                <%# Eval("LOAIPHANHOI") != null ? Eval("LOAIPHANHOI.TENLOAI") : ""%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>