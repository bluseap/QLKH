<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="true" 
    Inherits="EOSCRM.Web.Forms.Common.Error" Codebehind="Error.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolderMain" runat="Server">
    <table class="form" border="0" cellspacing="0" cellpadding="0" width="100%">
        <tr>
            <td class="right">
                <input type="button" class="ButtonBack" onclick="javascript:history.back(-1);" />
            </td>
        </tr>
    </table>
    <br />
    <div class="Component">
        <center>
            <table cellpadding="2" cellspacing="0" border="0" width="60%">
                <tr height="100px">
                    <td>
                        &nbsp;
                    </td>
                </tr>
                <tr>
                    <td align="center" valign="middle">
                        <fieldset>
                            <legend>Thông báo lỗi:</legend>
                            <table cellpadding="5" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td align="center" width="130px">
                                        <img src="../../content/images/common/IconCritical.gif" alt="" />
                                    </td>
                                    <td align="left" valign="middle">
                                        <asp:Label ID="lblmessage" runat="server" Font-Bold="true" ForeColor="Red"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </td>
                </tr>                
            </table>
        </center>
    </div>
</asp:Content>

