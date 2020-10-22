<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="TramBienAp.aspx.cs" Inherits="EOSCRM.Web.Forms.DanhMuc.TramBienAp" %>

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

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
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
                            <td class="crmcell right">Xí nghiệp</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" runat="server" ></asp:DropDownList>
                                    <asp:Label ID="lbMATBA" runat="server" Visible="false"></asp:Label>
                                </div>                                                            
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Th.Phố, Huyện</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlQuanHuyen" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQuanHuyen_SelectedIndexChanged" ></asp:DropDownList>                                 
                                </div>
                                <td class="crmcell right">Xã, phường</td>                                
                                <td class="crmcell" >
                                    <div class="left">
                                        <asp:DropDownList ID="ddlXAPHUONG" runat="server"></asp:DropDownList>
                                    </div>
                                </td>                              
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên trạm Công ty</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENTBA" runat="server" width="200px" TabIndex="3" />
                                </div>  
                                <td class="crmcell right">Tên trạm Điện lực</td>                                
                                <td class="crmcell" >
                                    <div class="left">
                                        <asp:TextBox ID="txtTenTBADienLuc" runat="server" width="200px" TabIndex="3" />
                                    </div>
                                </td>                                   
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">D.số công ty</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlDUONGPHO" runat="server"></asp:DropDownList>
                                </div>
                                <td class="crmcell right">D.số điện lực</td>                                
                                <td class="crmcell" >
                                    <div class="left">
                                        <asp:TextBox ID="txtDANHSODL" runat="server"  TabIndex="3" />                                        
                                    </div>
                                </td>                              
                            </td>                            
                        </tr> 
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div> 
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="30">
                    <PagerSettings FirstPageText="trạm " PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MATBA").ToString()%>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MATBA").ToString() %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã" HeaderStyle-Width="18%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MATBA").ToString()  %>'
                                    CommandName="EditItem" Text='<%# Eval("MATBA").ToString() %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Tên trạm Công ty" DataField="TENTBA" HeaderStyle-Width="25%" />
                        <asp:BoundField HeaderText="Tên trạm Điện lực" DataField="TENTBA2" HeaderStyle-Width="25%" />                        
                        <asp:BoundField HeaderText="D.Số Công ty" DataField="MADPPO" HeaderStyle-Width="5%" /> 
                        <asp:BoundField HeaderText="D.Số Điện Lực" DataField="DSODL" HeaderStyle-Width="5%" /> 
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
