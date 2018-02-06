<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.VatTu" CodeBehind="VatTu.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
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
                            <td class="crmcell right">Mã vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMAVT" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>
                                <div class="left filtered"></div>
                                <div class="left width-150">
                                    <div class="right">Mã hiệu</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMAHIEU" runat="server" Width="100px" MaxLength="30" TabIndex="2" />
                                </div>
                                <div class="left filtered"></div>                                
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENVT" runat="server" Width="250px" MaxLength="100" TabIndex="3" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đơn vị tính</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlDVT" runat="server" Width="262px" TabIndex="4" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Nhóm vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlNHOM" runat="server" Width="262px" TabIndex="5" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Giá vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtGIAVT" runat="server" Width="100px" MaxLength="15" TabIndex="6" />
                                </div>
                                <div class="left filtered"></div>
                                <div class="left width-150">
                                    <div class="right">Giá nhân công</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtGIANC" runat="server" Width="100px" MaxLength="15" TabIndex="7" />
                                </div>
                                <div class="left filtered"></div>
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowCommand="gvList_RowCommand" PageSize="50" PagerStyle-HorizontalAlign="Right"
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="phường" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MAVT") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MAVT") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã vật tư&nbsp;" SortExpression="MAVT">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MAVT") %>'
                                    CommandName="EditItem" Text='<%# Eval("MAVT") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                            <HeaderStyle Width="10%" />
                            <FooterTemplate>
                                <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                    <strong>Bỏ chọn hết</strong></a>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Mã hiệu" DataField="MAHIEU" HeaderStyle-Width="12%" />
                        <asp:BoundField HeaderText="Tên vật tư" DataField="TENVT" HeaderStyle-Width="25%" />
                        
                        <asp:TemplateField HeaderText="Giá vật tư&nbsp;">
                            <ItemTemplate>
                                <%# String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", Eval("GIAVT"))%>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="false" />
                            <HeaderStyle Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Giá nhân công&nbsp;">
                            <ItemTemplate>
                                <%# String.Format(System.Globalization.CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", Eval("GIANC"))%>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="false" />
                            <HeaderStyle Width="12%" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Nhân viên nhập&nbsp;">
                            <ItemTemplate>
                                <%# Eval("NHANVIEN.HOTEN") %>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="false" />
                            <HeaderStyle Width="12%" />
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
