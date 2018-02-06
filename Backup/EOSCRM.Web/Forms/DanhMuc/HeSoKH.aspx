<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.HeSoKH" CodeBehind="HeSoKH.aspx.cs" %>

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
        
        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');

            return false;
        }
        
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="hệ số" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderText="Mã hệ số" HeaderStyle-Width="80px">
                            <ItemTemplate>
                                <%# Eval("MAHS") %>                                    
                                <asp:HiddenField runat="server" ID="hdfMAHS" Value='<%#Eval("MAHS")%>'></asp:HiddenField>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />                       
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên hệ số">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTENHS" Width="98%" runat="server" />                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá trị" HeaderStyle-Width="90px">
                            <ItemTemplate>
                                <asp:TextBox ID="txtGIATRI" Width="80px" runat="server" />                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày áp dụng" HeaderStyle-Width="140px">
                            <ItemTemplate>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYAD" Width="80px" runat="server" />  
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYAD" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNGAYAD" runat="server" TargetControlID="txtNGAYAD"
                                    PopupButtonID="imgNGAYAD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />                              
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
            <br />
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" Text="" TabIndex="12" />
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
</asp:Content>
