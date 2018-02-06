<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.Group.Edition" CodeBehind="Edition.aspx.cs" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Src="GroupUI.ascx" TagName="GroupUI" TagPrefix="bwaco" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">    
    <script type="text/javascript">
        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnSave" OnClientClick="return CheckFormSave();"  runat="server" 
                                CssClass="save" UseSubmitBehavior="false" OnClick="btnSave_Click" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnCancel" UseSubmitBehavior="false" OnClick="btnCancel_Click"
                                runat="server" CssClass="cancel" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>   
    <br />
    <bwaco:GroupUI ID="UcGroupUI" runat="server" />
    <br />
    <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnSaveBottom" OnClientClick="return CheckFormSave();"  runat="server" 
                                CssClass="save" UseSubmitBehavior="false" OnClick="btnSave_Click" />
                        </div>
                        <div class="left">
                            <asp:Button ID="btnCancelBottom" UseSubmitBehavior="false" OnClick="btnCancel_Click"
                                runat="server" CssClass="cancel" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div> 
</asp:Content>
