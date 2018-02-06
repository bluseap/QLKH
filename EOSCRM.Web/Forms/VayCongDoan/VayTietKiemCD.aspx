<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="VayTietKiemCD.aspx.cs" Inherits="EOSCRM.Web.Forms.VayCongDoan.VayTietKiemCD" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
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

        function CheckFormFilterNV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');

            return false;
        }

        function CheckFormKhoiTao() {
            var nam = jQuery.trim($("#<%= txtNAM1.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM1.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnKhoiTaoKyVay) %>', '');
        }

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divNhanVienDlgContainer">
        <div id="divNhanVien" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 800px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">
                                                Từ khóa
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtKeywordNV" onchange="return CheckFormFilterNV();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterNV" OnClick="btnFilterNV_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterNV();" 
                                                        runat="server" CssClass="filter" Text="" />
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
							        <eoscrm:Grid ID="gvNhanVien" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvNhanVien_RowDataBound" OnRowCommand="gvNhanVien_RowCommand" 
							            OnPageIndexChanging="gvNhanVien_PageIndexChanging">
                                        <PagerSettings FirstPageText="nhân viên" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã NV">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MANVV") %>' CommandName="SelectMANV"                                                         
                                                        Text='<%# Eval("MANVV")%>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="HOTEN" HeaderText="Họ tên" />                                                                             
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
                            <td class="crmcell right">Khởi tạo kỳ</td>
                            <td class="crmcell">
                                <div class="left">
                                   <asp:DropDownList ID="ddlTHANG1" runat="server" Enabled="False" >
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
                                    <asp:TextBox ID="txtNAM1" runat="server" Width="30px" MaxLength="4" ReadOnly="True" />
                                </div>
                                <div class="left">
                                    <div class="right">sang kỳ</div>
                                </div>
                                <div class="left">
                                   <asp:DropDownList ID="ddlTHANG2" runat="server" Enabled="False" >
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
                                    <asp:TextBox ID="txtNAM2" runat="server" Width="30px" MaxLength="4" ReadOnly="True" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnKhoiTaoKyVay" runat="server" CommandArgument="Insert" CssClass="init"
                                        CausesValidation="false" UseSubmitBehavior="false" TabIndex="4" 
                                        OnClick="btnKhoiTaoKyVay_Click"  />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã nhân viên vay</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="100px" MaxLength="200" TabIndex="1" ReadOnly="true"
                                     />                                    
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNVV" runat="server" CssClass="pickup" OnClick="btnBrowseNVV_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên đóng quỹ tiết kiệm', 800, 'divNhanVien')" 
                                        TabIndex="1" />
                                </div>
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên nhân viên</td>
                            <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:Label ID="lblTENNVVAY" runat="server" ForeColor="Blue"></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Phòng ban</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENPB" runat="server" ForeColor="Blue"></asp:Label>
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số tiền cần vay</td>
                            <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:TextBox ID="txtTIENCANVAY" runat="server" Width="100px" OnTextChanged="txtTIENCANVAY_TextChanged1" AutoPostBack="true"  />
                                </div>   
                                <div class="left">
                                    <div class="right">Lãi suất 0.5% tháng</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbLAISUAT" runat="server" Visible="false"></asp:Label>
                                </div>                                                         
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Kỳ bắt đầu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYBATDAU" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                </div>                                
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYBATDAU"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                <div class="left">
                                    <div class="right">Kỳ kết thúc</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKETTHUC" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                </div>                               
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYKETTHUC"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tiền gốc phải trả ban đầu</td>
                            <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:TextBox ID="txtTIENGOC" runat="server" Width="100px"></asp:TextBox>
                                </div>   
                                <div class="left">
                                    <div class="right">Tiền lãi phải trả ban đầu</div>
                                </div>   
                                <div class="left">                                    
                                    <asp:TextBox ID="txtTIENLAI" runat="server" Width="100px" ReadOnly="true"></asp:TextBox>
                                </div>                                              
                            </td>
                        </tr>
                        
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilter();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="10" />
                                </div> 
                                <div class="left">
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" 
                                        runat="server" CssClass="save" Text="" TabIndex="11" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false" Visible="False"
                                        OnClick="btnDelete_Click" TabIndex="12" OnClientClick="return CheckFormDelete();" />
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="200"
                    OnRowCommand="gvList_RowCommand" OnRowDataBound="gvList_RowDataBound"
                    OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="nhân viên" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MALV") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MALV") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã nhân viên" HeaderStyle-Width="18%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MALV") %>'
                                    CommandName="EditItem" CssClass="link" Text='<%# Eval("MALV") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="HOTEN" HeaderText="Tên nhân viên" HeaderStyle-Width="40%" />                        
                        <asp:TemplateField HeaderText="Kỳ bắt đầu" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("KYBATDAU") != null) ?
                                        String.Format("{0:MM/yyyy}", Eval("KYBATDAU")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Kỳ kết thúc" HeaderStyle-Width="75px">
                            <ItemTemplate>
                                <%# (Eval("KYKETTHUC") != null) ?
                                        String.Format("{0:MM/yyyy}", Eval("KYKETTHUC")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                        
                        
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
