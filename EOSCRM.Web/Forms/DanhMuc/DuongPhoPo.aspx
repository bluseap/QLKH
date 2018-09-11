<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="DuongPhoPo.aspx.cs" Inherits="EOSCRM.Web.Forms.DanhMuc.DuongPhoPo" %>

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

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
            return false;
        }

        function CheckFormXuatExcelDotGhi() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btXuatExcelDotGhi) %>', '');
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
                            <td class="crmcell right">Kỳ khai thác</td>
                            <td class="crmcell">
                                <div class="left">
                                   <asp:DropDownList ID="ddlTHANG1" runat="server" >
                                        <asp:ListItem Text="01" Value="01" />
                                        <asp:ListItem Text="02" Value="02" />
                                        <asp:ListItem Text="03" Value="03" />
                                        <asp:ListItem Text="04" Value="04" />
                                        <asp:ListItem Text="05" Value="05" />
                                        <asp:ListItem Text="06" Value="06" />
                                        <asp:ListItem Text="07" Value="07" />
                                        <asp:ListItem Text="08" Value="08" />
                                        <asp:ListItem Text="09" Value="09" />
                                        <asp:ListItem Text="10" Value="10" />
                                        <asp:ListItem Text="11" Value="11" />
                                        <asp:ListItem Text="12" Value="12" />
                                   </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNAM1" runat="server" Width="30px" MaxLength="4" />
                                </div>
                            </td>
                        </tr>
                         <tr>    
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="cboMAKV" runat="server" TabIndex="5">
                                    </asp:DropDownList>
                                </div>                               
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã đường phố</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" runat="server" Width="50px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" Width="50px" MaxLength="1" TabIndex="2" Visible="false" />
                                </div>                                
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên đường phố</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENDP" runat="server" Width="250px" MaxLength="100" TabIndex="3" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên tắt</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENTAT" runat="server" Width="250px" MaxLength="100" TabIndex="4" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server" Enabled="False"></asp:DropDownList>
                                    <asp:CheckBox ID="ckDotGhiCS" runat="server" TabIndex="28"  oncheckedchanged="ckDotGhiCS_CheckedChanged" 
                                    Text="Chuyển đợt ghi chỉ số" AutoPostBack="True" /> 
                                </div>                                
                            </td>                                  
                        </tr>                    
                        <tr>
                            <td class="crmcell right"></td>
                            <td>
                               <div class="left">
                                    <asp:Label ID="lbGHICHU" runat="server" Text="Thêm đường phố, cập nhật đưa vào danh sách thay đổi chi tiết" Font-Bold="True" Font-Size="Large" ForeColor="#FF3300"></asp:Label>
                                </div>
                             </td>
                        </tr>                       
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" TabIndex="19" />
                                </div> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" TabIndex="20" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" TabIndex="21" OnClientClick="return CheckFormDelete();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="22" OnClientClick="return CheckFormCancel();" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btXuatExcelDotGhi" runat="server" Text="Xuất Excel DS đợt GCS" CssClass="myButton" UseSubmitBehavior="false" 
                                        OnClick="btXuatExcelDotGhi_Click" OnClientClick="return CheckFormXuatExcelDotGhi();" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
         <Triggers>
            <asp:PostBackTrigger ControlID="btXuatExcelDotGhi" />
        </Triggers>
    </asp:UpdatePanel>  
    <br />    
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="500" OnRowDataBound="gvList_RowDataBound"
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADPPO") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADPPO") + "-" + Eval("DUONGPHUPO") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã đường" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" OnClientClick="openWaitingDialog();unblockWaitingDialog();" 
                                    runat="server" CommandArgument='<%# Eval("MADPPO") + "-" + Eval("DUONGPHUPO") %>'
                                    CommandName="EditItem" Text='<%# Eval("MADPPO") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                        
                        <asp:BoundField HeaderText="Tên đường" DataField="TENDP" HeaderStyle-Width="42%" />
                        <asp:TemplateField HeaderText="Khu vực&nbsp;" HeaderStyle-Width="40%">
                            <ItemTemplate>
                                <%# Eval("KHUVUCPO.TENKV") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Đợt in" HeaderStyle-Width="17%">
                            <ItemTemplate>
                                <%# new EOSCRM.Dao.DotInHDDao().GetDMDotIn(Eval("IDMADOTIN").ToString()).TENDOTIN.ToString() %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>        
            </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
