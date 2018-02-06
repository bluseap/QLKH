<%@ Control Language="C#" AutoEventWireup="True" Inherits="EOSCRM.Web.Forms.Group.GroupUI" CodeBehind="GroupUI.ascx.cs" %>
<%@ Register Src="PermissionUI.ascx" TagName="PermissionUI" TagPrefix="uc2" %>

<div class="crmcontainer">
    <table class="crmtable">
        <tbody>
            <tr>
                <td class="crmcell right vtop">Tên nhóm</td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtGroupName" runat="server" Width="250px" MaxLength="50" 
                            TabIndex="1" Font-Names="Times New Roman" />&nbsp;
                        <asp:RequiredFieldValidator ID="reqGroupName" runat="server" ControlToValidate="txtGroupName"
                            SetFocusOnError="True" ValidationGroup="SaveGroup" TabIndex="-1"></asp:RequiredFieldValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right vtop">Mô tả</td>
                <td class="crmcell">
                    <div class="left">
                        <asp:TextBox ID="txtDesc" runat="server" Width="250px" MaxLength="500" 
                            TabIndex="2" Rows="4" TextMode="MultiLine" Font-Names="Times New Roman" />&nbsp;
                        <asp:RegularExpressionValidator ID="regDesc" ControlToValidate="txtDesc" runat="server"
                            ValidationGroup="SaveGroup"></asp:RegularExpressionValidator>
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right vtop">Kích hoạt</td>
                <td class="crmcell">
                    <div class="left">
                        <asp:CheckBox ID="CheckBox_IsActive" runat="server" Checked="true" TabIndex="3" />
                    </div>
                </td>
            </tr>
            <tr>
                <td class="crmcell right vtop">Phân quyền</td>
                <td class="crmcell">
                    <div class="left">
                        <uc2:PermissionUI ID="UcPermissionUI" runat="server" />
                    </div>
                </td>
            </tr>
        </tbody>
    </table>
</div>
<asp:Label ID="lblGroupID" runat="server" Text="-1" Visible="False"></asp:Label>
<asp:Label ID="lblUpdateDate" runat="server" Visible="False"></asp:Label>
<asp:Label ID="lblIsActive" runat="server" Visible="False"></asp:Label>
