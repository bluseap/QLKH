<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="TraCuNhanVienVayTK.aspx.cs" Inherits="EOSCRM.Web.Forms.VayCongDoan.TraCuNhanVienVayTK" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#divNVVTK").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divNVVTKlgContainer");
                }
            });

            $("#divTTNVTK").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTTNVTKlgContainer");
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

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divTTNVTKlgContainer">
        <div id="divTTNVTK" style="display: none">
            <asp:UpdatePanel ID="upnlTTNVTK" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTTNVTK" runat="server" UseCustomPager="true" PageSize="15"							            
							            OnPageIndexChanging="gvTTNVTK_PageIndexChanging">
                                        <PagerSettings FirstPageText="thông tin nhân viên vay" PageButtonCount="2" />
                                        <Columns>                                            
                                            <asp:BoundField HeaderStyle-Width="10%" DataField="NAM" HeaderText="Năm" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="THANG" HeaderText="tháng" />                                             
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="TIENGOC" HeaderText="Tiền gốc" />  
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="TIENLAI" HeaderText="Tiền lãi" />   
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="TONGTIEN" HeaderText="Tổng tiền" />  
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="CONLAI" HeaderText="Còn lại" />                                         
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
    <div id="divNVVTKlgContainer">
        <div id="divNVVTK" style="display: none">
            <asp:UpdatePanel ID="upnlNVVTK" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvNVVTK" runat="server" UseCustomPager="true" 							            
							            OnPageIndexChanging="gvNVVTK_PageIndexChanging">
                                        <PagerSettings FirstPageText=" nhân viên vay" PageButtonCount="2" />
                                        <Columns>                                            
                                            <asp:BoundField HeaderStyle-Width="10%" DataField="ID" HeaderText="Số TT" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="KYBATDAU" HeaderText="KỲ bắt đầu" />                                             
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="KYKETTHUC" HeaderText="Kỳ kết thúc" />  
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="TIENVAY" HeaderText="Tiền vay" />   
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="THANHTOAN" HeaderText="T.Toán" />  
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="KYTATTOAN" HeaderText="Kỳ trả hết" />                                         
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
                            <td class="crmcell right">Mã nhân viên vay</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="100px" MaxLength="200" TabIndex="1" ReadOnly="True"
                                     />                                    
                                </div>
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên nhân viên</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:TextBox ID="txtHOTEN" runat="server" Width="250px" MaxLength="200" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" runat="server" Width="262px" TabIndex="3" />
                                </div>
                            </td>
                        </tr>                       
                        <tr>
                            <td class="crmcell right">Phòng ban</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHONGBAN" Width="262px" runat="server" TabIndex="4" />
                                </div>
                                <div class="left">
                                    <div class="right">Công việc</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlCONGVIEC" Width="262px" runat="server" TabIndex="5" />
                                </div>
                            </td>
                        </tr>                        
                         <tr>
                            <td class="crmcell right">Kỳ bắt đầu tham gia TK</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYBATDAU" runat="server" Width="80px" MaxLength="80" TabIndex="6" />
                                </div>                                
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYBATDAU"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKETTHUC" runat="server" Width="80px" MaxLength="80" TabIndex="6" />
                                </div>                               
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYKETTHUC"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                
                                <div class="left">
                                    <div class="right">Ngày sinh</div>
                                </div>
                                 <div class="left">
                                    <asp:TextBox ID="txtNGAYSINH" runat="server" Width="100px" MaxLength="200" TabIndex="6" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYSINH" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="7" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYSINH"
                                    PopupButtonID="imgNGAYSINH" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                
                            </td>
                        </tr>                        
                        <tr>
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI" runat="server" Width="250px" MaxLength="200" TabIndex="8" />
                                </div>
                                <div class="left">
                                    <div class="right">Điện thoại</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDIENTHOAI" runat="server" Width="250px" MaxLength="200" TabIndex="9" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tổng tiền đóng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbTONGTIENDONG" runat="server" Font-Size="Larger" ForeColor="#0066FF"></asp:Label>
                                </div>
                               <div class="left">
                                    <div class="right">Tổng lãi đóng</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGLAIDONG" runat="server" Font-Size="Larger" ForeColor="#0066FF" ></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGTHU" runat="server" Font-Size="Larger" ForeColor="#0066FF" Visible="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="header">TỔNG THU, CHI ĐẾN NAY</td> 
		                </tr>
                        <tr>    
                             <td class="crmcell right">Tổng thu nhân viên tham gia tiết kiệm</td>
                             <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:Label ID="lbTHUTIETKIEM" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>     
                                <div class="left">
                                    <div class="right">Tổng chi còn lại</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbCHICONLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>                              
                             </td>
                         </tr>
                        <tr>
                            <td class="crmcell right">Tổng thu lãi</td>
                            <td class="crmcell"> 
                                 <div class="left">
                                    <asp:Label ID="lbTHUTIENLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tổng tiền còn lại</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGTIENCONLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>  
                             </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lnkNVNTK" OnClick="lnkNVNTK_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử nhân viên vay tiết kiệm', 750, 'divNVVTK')" TabIndex="43">
                                        Lược sử nhân viên vay tiết kiệm
                                    </asp:LinkButton>
                                </div>
                                <div class="left">
                                     <div class="right">                  </div>                                  
                                </div>
                                <div class="left">
                                    <asp:LinkButton ID="lnkTTNVTK" OnClick="lnkTTNVTK_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Thông tin nhân viên vay tiết kiệm', 750, 'divTTNVTK')" TabIndex="43">
                                        Thông tin vay tiết kiệm
                                    </asp:LinkButton>
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
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false" Visible="false"
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
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MANVV") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MANVV") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã nhân viên" HeaderStyle-Width="8%">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MANVV") %>'
                                    CommandName="EditItem" CssClass="link" Text='<%# Eval("MANVV") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên NV" DataField="HOTEN" >                        
                            <HeaderStyle Width="25%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Phòng, ban" DataField="TENPB" >                        
                            <HeaderStyle Width="25%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="Số lần vay" DataField="SOLANVAY" >                        
                            <HeaderStyle Width="5%" Font-Bold="true" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Tổng tiền TK" HeaderStyle-Width="11%" >
                            <ItemTemplate>
                                <%# (Eval("TTTIETKIEM") != null) ?
                                        String.Format("{0:0,0}", Eval("TTTIETKIEM")) : "0" %>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" Font-Size="Large"/>                            
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Kỳ đóng TK" HeaderStyle-Width="8%" >
                            <ItemTemplate>
                                <%# (Eval("KYBATDAU") != null) ?
                                        String.Format("{0:0,0}", Eval("KYBATDAU")) : "0" %>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
