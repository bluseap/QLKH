<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.DanhMuc.VatTu" CodeBehind="VatTu.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util"%>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">    
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divVatTu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divVatTuDlgContainer");
                }
            });
        });

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

        function CheckFormFilterVatTuKeToan() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTuKeToan) %>', '');
            return;
        }
        
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divVatTuDlgContainer">	
		<div id="divVatTu" style="display:none">		    
	        <asp:UpdatePanel ID="upnlVatTu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table cellpadding="3" cellspacing="1" style="width: 600px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">                                                
                                                    <asp:TextBox ID="txtFilterVatTuKeToan" onchange="return CheckFormFilterVatTuKeToan();" runat="server" />
                                                </div>
                                                <div class="left">  
                                                    <asp:Button ID="btnFilterVatTuKeToan" OnClientClick="return CheckFormFilterVatTuKeToan();" 
                                                        runat="server" CssClass="filter" UseSubmitBehavior="false" OnClick="btnFilterVatTuKeToan_Click" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="ptop-10">
                                <div class="crmcontainer">
                                    <eoscrm:Grid ID="gvVatTuKeToan" runat="server" UseCustomPager="true" 
						                OnRowDataBound="gvVatTuKeToan_RowDataBound" OnRowCommand="gvVatTuKeToan_RowCommand" 
						                OnPageIndexChanging="gvVatTuKeToan_PageIndexChanging">
							            <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Mã vật tư" >
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnVatTuKeToanID" runat="server" CommandArgument='<%# Eval("Id") %>'
                                                        CommandName="EditItem" Text='<%# Eval("VatTuId") %>'></asp:LinkButton>
                                                </ItemTemplate>                                                
                                            </asp:TemplateField>  
                                            <asp:TemplateField HeaderText="Tên vật tư">
                                                <ItemTemplate>
                                                     <%# Eval("TenVatTu") !=null ?  Eval("TenVatTu").ToString() : "" %>    
                                                </ItemTemplate>                                                
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Ngày tạo Kế toán">
                                                <ItemTemplate>
                                                     <%# Eval("NgayTaoBravo") !=null ? String.Format("{0:dd/MM/yyyy}", Eval("NgayTaoBravo")) : "" %>    
                                                </ItemTemplate>                                                
                                            </asp:TemplateField>
                                        </Columns>
                                    </eoscrm:Grid>
                                </div>
                            </td>
                        </tr>
                    </table>
				</ContentTemplate>
			</asp:UpdatePanel>						
		</div>
	</div>
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div >
                                    <asp:DropDownList ID="ddlKHUVUC" runat="server"></asp:DropDownList>
                                    <asp:Label ID="lbMAVT" runat="server" Visible="false"></asp:Label>
                                    <asp:Label ID="lbMAVTCTY" runat="server" Visible="false"></asp:Label>
                                </div>                                              
                            </td>
                        </tr> 
                        <tr>    
                            <td class="crmcell right">Loại vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAIVATTU" runat="server" TabIndex="4" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Ký hiệu vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMAVT" runat="server" Width="100px" MaxLength="10" TabIndex="1" />
                                </div>                                
                                <div class="left width-150">
                                    <div class="right">Số TT</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMAHIEU" runat="server" Width="30px" MaxLength="30" TabIndex="2" />
                                </div                                                              
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtTENVT" runat="server" Width="250px" MaxLength="500" TabIndex="3" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đơn vị tính</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlDVT" runat="server" Width="262px" TabIndex="4" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Nhóm vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlNHOM" runat="server" Width="262px" TabIndex="5" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Giá vật tư</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtGIAVT" runat="server" Width="70px" MaxLength="15" TabIndex="6" />
                                </div>                                
                                <div class="left width-100">
                                    <div class="right">Giá nhân công</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtGIANC" runat="server" Width="70px" MaxLength="15" TabIndex="7" />
                                </div>                                
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã vật tư kế toán</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMaVatTuKeToan" runat="server" Width="100px" MaxLength="15" TabIndex="6" />
                                </div>                     
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Kho vật tư kế toán</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlKhoVatTuKeToan" runat="server"></asp:DropDownList>
                                </div>                       
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
                                <td class="crmcell right"></td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:Button ID="btnVatTuKeToan" runat="server" CssClass="myButton" UseSubmitBehavior="false" Text="Vật tư kế toán"
                                            TabIndex="13" OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 600, 'divVatTu')"  
                                            OnClick="btnVatTuKeToan_Click" />
                                    </div>
                                </td>
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
                        <asp:TemplateField HeaderText="Mã kế toán">
                            <ItemTemplate>
                                 <%# Eval("KeToanMaSoVatTu") !=null ?  Eval("KeToanMaSoVatTu").ToString() : "" %>    
                            </ItemTemplate>
                            <ItemStyle Font-Bold="false" />
                            <HeaderStyle Width="12%" />
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Kho XN" HeaderStyle-Width="160px">
                            <ItemTemplate>
                                <%# new KhoDanhMucDao().Get(Eval("KhoDanhMucId") != null ? Eval("KhoDanhMucId").ToString() : "" ) != null ? 
                                new KhoDanhMucDao().Get(Eval("KhoDanhMucId") != null ? Eval("KhoDanhMucId").ToString() : "" ).TenKho.ToString() : ""   %>
                            </ItemTemplate>
                        </asp:TemplateField>                      
                        <asp:TemplateField HeaderText="Ký hiệu&nbsp;">
                            <ItemTemplate>
                                 <%# Eval("KYHIEUVT") !=null ?  Eval("KYHIEUVT").ToString() : "" %>    
                            </ItemTemplate>
                            <ItemStyle Font-Bold="false" />
                            <HeaderStyle Width="8%" />
                        </asp:TemplateField>
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
