<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.HeThong.ChangePassword" CodeBehind="ChangePassword.aspx.cs" %>
    
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

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
    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Tên đăng nhập</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbHOTEN" runat="server" Text="Label"></asp:Label>
                                    <asp:Label ID="lblUSERNAME" runat="server" Text="Label" Visible="false"></asp:Label>
                                </div>
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Mật khẩu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtPASSWORD" runat="server" Width="100px" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <%--<tr>
                            <td class="crmcell right">Nhóm người dùng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lblNHOM" runat="server" Text="Label"></asp:Label>
                                </div>
                            </td>
                        </tr>--%>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save"
                                        OnClick="btnSave_Click"  OnClientClick="return CheckFormSave();"
                                        TabIndex="5" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="6" UseSubmitBehavior="false" OnClientClick="CheckFormCancel();" />                                    
                                </div>
                            </td>
                        </tr> 
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
