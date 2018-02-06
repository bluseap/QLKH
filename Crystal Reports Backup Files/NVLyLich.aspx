<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/ERPPO.Master" AutoEventWireup="true" CodeBehind="NVLyLich.aspx.cs" Inherits="EOSCRM.Web.NhanSu.NVLyLich" Culture="vi-VN" uiCulture="vi" %>

<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
                }
            });
            
        });

        function CheckFormFilterNV() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');
		    return false;
        }

        function file_changeM1(f) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = document.getElementById("imgM1");
                img.src = e.target.result;
                img.style.display = "inline";
                img.style.height = "80px";
                img.style.width = "60px"
            };
            reader.readAsDataURL(f.files[0]);

        }

        function file_changeM2(f) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = document.getElementById("imgM2");
                img.src = e.target.result;
                img.style.display = "inline";
                img.style.height = "80px";
                img.style.width = "60px"
            };
            reader.readAsDataURL(f.files[0]);

        }

        function CheckFormSaveLyLich() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveLyLich) %>', '');
            return false;
        }
        
        function CheckFormSaveTrinhDo() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveTrinhDo) %>', '');
            return false;
        }
       
        function CheckFormSaveQTCT() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveQTCT) %>', '');
            return false;
        }
       
        function CheckFormSaveQuanDoi() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveQuanDoi) %>', '');
            return false;
        }
        
        function CheckFormSaveSoTruong() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveSoTruong) %>', '');
            return false;
        }
        //CheckFormDeleteTrinhDo
        function CheckFormDeleteTrinhDo() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();

                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDeleteTrinhDo) %>', '');
            }
            return false;
        }
        //SaveDTBD
        function CheckFormSaveDTBD() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveDTBD) %>', '');
            return false;
        }
        //btnSaveKTKL
        function CheckFormSaveKTKL() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveKTKL) %>', '');
            return false;
        }
        //CheckFormSaveQUANHEGD
        function CheckFormSaveQUANHEGD() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSaveQUANHEGD) %>', '');
            return false;
        }

    </script>
   
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">   
    <div id="divNhanVienDlgContainer">
        <div id="divNhanVien" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">
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
                                                    <asp:LinkButton ID="lnkBtnID" runat="server"  CssClass="link"
                                                        CommandArgument='<%# Eval("MANV") %>' CommandName="SelectMANV"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MANV").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle Font-Bold="True" />
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" DataField="HOTEN" HeaderText="Họ tên" />
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Khu vực">
                                                <ItemTemplate>
                                                    <%# Eval("KHUVUC") != null ? Eval("KHUVUC.TENKV") : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Phòng ban">
                                                <ItemTemplate>
                                                    <%# Eval("PHONGBAN") != null ? Eval("PHONGBAN.TENPB") : "" %>
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
    <asp:UpdatePanel ID="upinfoNhanVien" UpdateMode="Conditional" runat="server">
        <ContentTemplate>   
            <div class="crmcontainer">
                <table class="crmtable" >
                    <tr>    
                          <td class="crmcell right">Tìm nhân viên</td>
                            <td class="crmcell"> 
                                <div class="left">
                                        <asp:Button ID="btnBrowseNV" runat="server" CssClass="addnew" 
                                        OnClick="btnBrowseNV_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 400, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbMANV" runat="server" Visible="False"></asp:Label>
                                </div>                                
                            </td>
                       </tr>
                      <tr>
                            <td class="crmcell right"> Đơn vị</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:Label ID="lbTENKV" runat="server" ForeColor="#0000CC"></asp:Label>
                                </div>
                            </td>
                            <td class="crmcell right">Phòng, ban</td>                              
                            <td class="crmcell" >
                                    <div class="left">
                                        <asp:Label ID="lbTENPHONGBAN" runat="server" ForeColor="#0000CC"></asp:Label>
                                    </div>
                            </td>                            
                      </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div class="crmcontainer" id="divLyLich" runat="server">
        <asp:Panel ID="HeaderLyLich" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageLyLich" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label4" runat="server" Font-Bold="True">SƠ YẾU LÝ LỊCH</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>    
        </asp:Panel>
        <asp:Panel ID="ContentLyLich" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoLyLich" UpdateMode="Conditional" runat="server">
                <ContentTemplate>   
                    <div class="crmcontainer">
                        <table class="crmtable" >
                            <tbody>                  
                      
                      <tr>
                            <td class="crmcell right">Họ và tên (In hoa)</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:TextBox ID="txtHOTENKS" runat="server" MaxLength="100"  TabIndex="4"  /> 
                                </div>
                            </td>
                            <td class="crmcell right">Họ và tên thường gọi</td>                                
                            <td class="crmcell" >
                                    <div class="left">
                                        <asp:TextBox ID="txtTENTHUONG" runat="server" MaxLength="100"  TabIndex="5"  /> 
                                    </div>
                            </td>                            
                      </tr>
                      <tr>
                          <td class="crmcell right">Các tên gọi khác</td>
                          <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKHAC" runat="server" MaxLength="100" TabIndex="6"  />
                                </div>
                          </td>                                
                          <td class="crmcell right">Ngày sinh</td>
                          <td class="crmcell" >
                                    <div class="left">
                                        <asp:TextBox ID="txtNGAYSINH" runat="server" width="80px" MaxLength="10"  TabIndex="7" />   
                                                        
                                        <asp:ImageButton runat="Server" ID="imgNGAYSINH" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                            AlternateText="Click to show calendar"  Visible="false"/> 
                                        <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYSINH"
                                            PopupButtonID="imgNGAYSINH" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                    </div>
                          </td>
                          <td class="crmcell" >
                                    <div class="left">
                                        <asp:CheckBox ID="ckNAMSINH" runat="server" TabIndex="28" oncheckedchanged="ckNAMSINH_CheckedChanged" 
                                            AutoPostBack="True" />
                                    </div>
                                    <div class="left">
                                         <strong>Năm sinh</strong>
                                         <asp:TextBox ID="txtNAMSINH" runat="server" width="50px" MaxLength="4"  TabIndex="7" Visible="false" />  
                                    </div>
                           </td>
                          
                      </tr>    
                      <tr>
                            <td class="crmcell right">Giới tính</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:DropDownList ID="ddlGIOITINH" runat="server"  TabIndex="9">
                                        <asp:ListItem Text="Nam" Value="NAM" />
                                        <asp:ListItem Text="Nữ" Value="NU" />
                                    </asp:DropDownList>
                                    <asp:Label ID="lvMANVLL" runat="server" Visible="False"></asp:Label>
                                </div>
                            </td>
                            <td class="crmcell right">Nơi sinh</td>
                            <td class="crmcell" >                                
                                <div class="left">
                                    <asp:TextBox ID="txtNOISINH" runat="server" MaxLength="300" TabIndex="10"  />
                                </div>
                            </td>
                      </tr>
                      <tr>
                            <td class="crmcell right">Quê quán</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtQUEQUAN" runat="server" MaxLength="300" TabIndex="11"  />
                                </div>
                            </td>
                            <td class="crmcell right">Nơi ở hiện nay</td>
                            <td class="crmcell" >                                
                                <div class="left">
                                    <asp:TextBox ID="txtNOIO" runat="server" MaxLength="300" TabIndex="12"  />
                                </div>
                            </td>
                      </tr>
                      <tr>
                            <td class="crmcell right">Dân tộc</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlDANTOC" runat="server"  TabIndex="12"/>
                                </div>
                            </td>
                            <td class="crmcell right">Tôn giáo</td>
                            <td class="crmcell" >                               
                                <div class="left">
                                    <asp:DropDownList ID="ddlTONGIAO" runat="server" TabIndex="13"/>
                                </div>
                            </td>
                      </tr>
                      <tr>
                            <td class="crmcell right">Thành phần xuất thân</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:DropDownList ID="ddlTPXT" runat="server" TabIndex="14"/>
                                </div>
                            </td>
                            <td class="crmcell right">Nghề khi tuyển dụng</td>
                            <td class="crmcell" >                                
                                <div class="left">
                                    <asp:TextBox ID="txtNGHETRUOCTD" runat="server" MaxLength="100" TabIndex="12"  />
                                </div>
                            </td>
                      </tr>
                      <tr>
                            <td class="crmcell right">Sở trường CT</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:TextBox ID="txtSOTRUONGCT" runat="server" MaxLength="200" TabIndex="12"  />
                                </div>
                            </td>                           
                      </tr>
                      <tr>    
                            <td ></td>
                            <td class="crmcell">                                 
                                <div class="left">
                                    <asp:Button ID="btnSaveLyLich" OnClick="btnSaveLyLich_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSaveLyLich();" 
                                        runat="server" CssClass="save" Text="" TabIndex="11" />
                                </div>       
                            </td>
                     </tr>
                    </tbody>
                        </table>      
                    </div>                
                </ContentTemplate>          
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upLyLich" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvLyLich" runat="server" UseCustomPager="True" PageSize="50"
                            OnRowCommand="gvLyLich_RowCommand" OnRowDataBound="gvLyLich_RowDataBound"
                            OnPageIndexChanging="gvLyLich_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="Lý lịch nhân viên" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMMANVLL" runat="server" type="hidden" value='<%# Eval("MANVLL") %>' />
                                        <input name="listIdsMMANVLL" type="checkbox" value='<%# Eval("MANVLL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMMANVLL" runat="server" CommandArgument='<%# Eval("MANVLL") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MANVLL") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tên" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnHOTENKS" runat="server"
                                            Text='<%# Eval("HOTENKS") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Giới tính" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnGIOITINH" runat="server"
                                            Text='<%# Eval("GIOITINH").Equals(false) ? "Nam" : "Nữ" %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <br/>    
    <div class="crmcontainer" id="divTrinhDo" runat="server">
        <asp:Panel ID="HeaderTrinhDo" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageTrinhDo" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Text" runat="server" Font-Bold="True">TRÌNH ĐỘ HỌC VẤN</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>    
        </asp:Panel>
        <asp:Panel ID="ContentTrinhDo" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoTrinhDo" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <table class="crmtable" >
                    <tbody>
                        <tr>
                            <td class="crmcell right">Bằng cấp</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAIBANG" runat="server" TabIndex="13"/>
                                </div>
                            </td>
                            <td class="crmcell right">Chuyên ngành</td>
                            <td class="crmcell" >                                
                                <div class="left">
                                    <asp:TextBox ID="txtCHUYENNGANH" runat="server" MaxLength="200" TabIndex="12"  />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày cấp bằng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYCAPBANG" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                    <asp:ImageButton runat="Server" ID="imgNGAYCAPBANG" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                        AlternateText="Click to show calendar"  Visible="false"/> 
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYCAPBANG"
                                        PopupButtonID="imgNGAYCAPBANG" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                            </td>
                            <td class="crmcell right">Tên trường</td>
                            <td class="crmcell" >
                                <div class="left">
                                    <asp:TextBox ID="txtTENTRUONG" runat="server" MaxLength="200" TabIndex="12"  />
                                </div>                        
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Loại bằng</td>
                            <td class="crmcell">
                                <div class="left ">
                                    <asp:DropDownList ID="ddlCHEDOHOC" runat="server" TabIndex="13"/>
                                    <asp:Label ID="lbMATD" runat="server" Visible="false"></asp:Label>
                                </div>                       
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Hình bằng mặt 1</td>
                            <td class="crmcell">
                                <div class="left">                              
                                    <asp:FileUpload ID="UpBANGM1" runat="server" type="file" onchange="file_changeM1(this)" Width="85px" />
                                    <input id="bangm1" type="file" onchange="file_changeM1(this)" style="display: none" />
                                    <input type="button" onclick="document.getElementById('bangm1').click()" />
                                </div>
                            </td>                          
                            <td class="crmcell" >
                                <div class="left">                                   
                                    <img id="imgM1" style="display: none" />
                                    <asp:Image ID="imgBANGM1" runat="server" Height="80px" Visible="False" Width="60px" />
                                </div>                                    
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Hình bằng mặt 2</td>
                            <td class="crmcell">
                                <div class="left ">                              
                                    <asp:FileUpload ID="UpBANGM2" runat="server" type="file" onchange="file_changeM2(this)" Width="85px" />
                                    <input id="bangm2" type="file" onchange="file_changeM2(this)" style="display: none" />
                                    <input type="button" onclick="document.getElementById('bangm2').click()" />
                                </div>
                            </td>                          
                            <td class="crmcell" >
                                <div class="left">                                   
                                    <img id="imgM2" style="display: none" />
                                    <asp:Image ID="imgBANGM2" runat="server" Height="80px" Visible="False" Width="60px" />
                                </div>                                    
                            </td>
                        </tr>
                        <tr>    
                            <td ></td>
                            <td class="crmcell">                                 
                                <div class="left">
                                    <asp:Button ID="btnSaveTrinhDo" OnClick="btnSaveTrinhDo_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSaveTrinhDo();" 
                                        runat="server" CssClass="save" Text="" TabIndex="11" />
                                </div>       
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                        <asp:Button ID="btnDeleteTrinhDo" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                            OnClick="btnDeleteTrinhDo_Click" TabIndex="13" OnClientClick="return CheckFormDeleteTrinhDo();" />
                                 </div>
                            </td>
                        </tr>
                        </tbody>
                </table> 
                </ContentTemplate>
                <Triggers>
                    <asp:PostBackTrigger ControlID="btnSaveTrinhDo" />            
                </Triggers>  
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upTrinhDo" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvTrinhDo" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvTrinhDo_RowCommand" OnPageIndexChanging="gvTrinhDo_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="Trình độ nhân viên" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMATD" runat="server" type="hidden" value='<%# Eval("MATD") %>' />
                                        <input name="listIdsMATD" type="checkbox" value='<%# Eval("MATD") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMATD" runat="server" CommandArgument='<%# Eval("MATD") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MATD") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Chuyên ngành" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDCHUYENNGANH" runat="server"
                                            Text='<%# Eval("LLOAIBC.TENLOAIBC") + " " + Eval("CHUYENNGANH") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:ImageField DataImageUrlField="HINHBC2" HeaderText="Bằng M2">
                                    <ControlStyle Height="80px" Width="60px" />
                                </asp:ImageField> 
                                <asp:ImageField DataImageUrlField="HINHBC1" HeaderText="Bằng M1">
                                    <ControlStyle Height="80px" Width="60px" />
                                </asp:ImageField>
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>     
        </asp:Panel>
    </div>
    <br />
    <div class="crmcontainer" id="divQuaTrinhCT" runat="server">
        <asp:Panel ID="HeaderQuaTrinhCT" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageQuaTrinhCT" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label1" runat="server" Font-Bold="True">LỊCH SỬ BẢN THÂN, QT CÔNG TÁC, TG CÁCH MẠNG, TG ĐOÀN, TG ĐỘI</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>    
        </asp:Panel>
        <asp:Panel ID="ContentQuaTrinhCT" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoQuaTrinhCT" UpdateMode="Conditional" runat="server">
                <ContentTemplate>   
                   <div class="crmcontainer">
                        <table class="crmtable" >
                          <tbody> 
                              <tr>
                                    <td class="crmcell right">Từ tháng, năm</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtNGAYBDCT" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="imgNGAYBDCT" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYBDCT"
                                                PopupButtonID="imgNGAYBDCT" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                        </div>
                                    </td>
                                    <td class="crmcell right">Đến tháng, năm</td>
                                    <td class="crmcell" >
                                        <div class="left">
                                            <asp:TextBox ID="txtNGAYKTCT" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="imgNGAYKTCT" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtNGAYKTCT"
                                                PopupButtonID="imgNGAYKTCT" TodaysDateFormat="MM/yyyy" Format="MM/yyyy" />
                                        </div>                        
                                    </td>
                              </tr>
                              <tr>
                                    <td class="crmcell right vtop">Nội dung</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtNOIDUNGCT" runat="server" Width="600px" MaxLength="1000" TabIndex="4" TextMode="MultiLine" />
                                            <asp:Label ID="lbMAQTCT" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                              </tr>
                              <tr>    
                                    <td ></td>
                                    <td class="crmcell">                                 
                                        <div class="left">
                                            <asp:Button ID="btnSaveQTCT" OnClick="btnSaveQTCT_Click"
                                                UseSubmitBehavior="false" OnClientClick="return CheckFormSaveQTCT();" 
                                                runat="server" CssClass="save" Text="" TabIndex="11" />
                                        </div>       
                                    </td>
                             </tr>
                              </tbody>
                        </table>      
                   </div>                
                </ContentTemplate> 
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upQuaTrinhCT" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvQuaTrinhCT" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvQuaTrinhCT_RowCommand"  OnPageIndexChanging="gvQuaTrinhCT_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="Quá trình công tác" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMAQTCT" runat="server" type="hidden" value='<%# Eval("MAQTCT") %>' />
                                        <input name="listIdsMAQTCT" type="checkbox" value='<%# Eval("MAQTCT") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMAQTCT" runat="server" CommandArgument='<%# Eval("MAQTCT") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MAQTCT") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày BĐ" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDNGAYBD" runat="server"
                                            Text='<%# String.Format("{0:MM/yyyy}", Eval("NGAYBDCT")) %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Ngày KT" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDNGAYKT" runat="server"
                                            Text='<%# String.Format("{0:MM/yyyy}", Eval("NGAYKTCT"))%>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>    
                                <asp:TemplateField HeaderText="Nội dung" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDNOIDUNG" runat="server"
                                            Text='<%# Eval("NOIDUNG") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>                      
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel> 
        </asp:Panel>
    </div>
    <br />
    <div class="crmcontainer" id="divQuanDoi" runat="server">
        <asp:Panel ID="HeaderQuanDoi" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageQuanDoi" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label2" runat="server" Font-Bold="True">QUÁ TRÌNH THAM GIA NHẬP NGŨ</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>
        </asp:Panel>
        <asp:Panel ID="ContentQuanDoi" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoQuanDoi" UpdateMode="Conditional" runat="server">
               <ContentTemplate>   
                   <div class="crmcontainer">
                        <table class="crmtable" >
                          <tbody>
                              <tr>
                                    <td class="crmcell right">Ngày nhập ngũ</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtNGAYBDNN" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="imgtxtNGAYBDNN" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtNGAYBDNN"
                                                PopupButtonID="imgtxtNGAYBDNN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                        </div>
                                    </td>
                                    <td class="crmcell right">Ngày xuất ngũ</td>
                                    <td class="crmcell" >
                                        <div class="left">
                                            <asp:TextBox ID="txtNGAYKTNN" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="imgtxtNGAYKTNN" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtNGAYKTNN"
                                                PopupButtonID="imgtxtNGAYKTNN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                        </div>                        
                                    </td>
                              </tr>
                              <tr>
                                    <td class="crmcell right vtop">Quân hàm</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtQUANHAM" runat="server" MaxLength="100" TabIndex="4"  />
                                        </div>
                                    </td>
                                    <td class="crmcell right">Nội dung</td>
                                    <td class="crmcell" >
                                        <div class="left">
                                            <asp:TextBox ID="txtNOIDUNGNHAPNGU" runat="server" MaxLength="1000" TabIndex="4"  />
                                            <asp:Label ID="lbMANNGU" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                              </tr>
                              <tr>    
                                    <td ></td>
                                    <td class="crmcell">                                 
                                        <div class="left">
                                            <asp:Button ID="btnSaveQuanDoi" OnClick="btnSaveQuanDoi_Click"
                                                UseSubmitBehavior="false" OnClientClick="return CheckFormSaveQuanDoi();" 
                                                runat="server" CssClass="save" Text="" TabIndex="11" />
                                        </div>       
                                    </td>
                             </tr>
                              </tbody>
                        </table>      
                   </div>                
               </ContentTemplate>    
           </asp:UpdatePanel>
            <asp:UpdatePanel ID="upQuanDoi" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvQuanDoi" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvQuanDoi_RowCommand"  OnPageIndexChanging="gvQuanDoi_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="Nhập ngũ" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMANNGU" runat="server" type="hidden" value='<%# Eval("MANNGU") %>' />
                                        <input name="listIdsMANNGU" type="checkbox" value='<%# Eval("MANNGU") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMANNGU" runat="server" CommandArgument='<%# Eval("MANNGU") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MANNGU") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ngày BĐ" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDNGAYBDNN" runat="server"
                                            Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("NGAYBDNN")) %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Ngày KT" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDNGAYKTNN" runat="server"
                                            Text='<%# String.Format("{0:dd/MM/yyyy}", Eval("NGAYKTNN"))%>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>    
                                <asp:TemplateField HeaderText="Quân hàm" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDQUANHAM" runat="server"
                                            Text='<%# Eval("QUANHAM") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Nội dung" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnIDNOIDUNGNN" runat="server"
                                            Text='<%# Eval("NOIDUNG") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>                                 
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel> 
        </asp:Panel>
    </div>
    <br />
    <div class="crmcontainer" id="divSucKhoeST" runat="server">
        <asp:Panel ID="HeaderSucKhoeST" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageSucKhoeST" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label3" runat="server" Font-Bold="True">TÌNH HÌNH SỨC KHỎE</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>            
        </asp:Panel>
        <asp:Panel ID="ContentSucKhoeST" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoSucKhoe" UpdateMode="Conditional" runat="server">
               <ContentTemplate>   
                   <div class="crmcontainer">
                        <table class="crmtable" >
                          <tbody>                       
                              <tr>
                                    <td class="crmcell right vtop">Tình trạng sức khỏe</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtTINHTRANGSK" runat="server" MaxLength="50" TabIndex="4"  />
                                        </div>
                                    </td>
                                    <td class="crmcell right">Bệnh mãn tính</td>
                                    <td class="crmcell" >
                                        <div class="left">
                                            <asp:TextBox ID="txtBENHMANTINH" runat="server" MaxLength="100" TabIndex="4"  />
                                        </div>
                                    </td>
                              </tr>
                              <tr>
                                    <td class="crmcell right vtop">Chiều cao</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtCHIEUCAO" runat="server" TabIndex="4"  />
                                        </div>
                                    </td>
                                    <td class="crmcell right">Cân nặng</td>
                                    <td class="crmcell" >
                                        <div class="left">
                                            <asp:TextBox ID="txtCANNANG" runat="server" TabIndex="4"  />
                                            <asp:Label ID="lbMASUCKHOE" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>
                              </tr>
                              <tr>    
                                    <td ></td>
                                    <td class="crmcell">                                 
                                        <div class="left">
                                            <asp:Button ID="btnSaveSoTruong" OnClick="btnSaveSoTruong_Click"
                                                UseSubmitBehavior="false" OnClientClick="return CheckFormSaveSoTruong();" 
                                                runat="server" CssClass="save" Text="" TabIndex="11" />
                                        </div>       
                                    </td>
                             </tr>             
                
                          </tbody>
                        </table>      
                   </div>                
               </ContentTemplate>    
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upSucKhoe" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvSucKhoe" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvSucKhoe_RowCommand"  OnPageIndexChanging="gvSucKhoe_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="Nhập ngũ" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMASUCKHOE" runat="server" type="hidden" value='<%# Eval("MASUCKHOE") %>' />
                                        <input name="listIdsMASUCKHOE" type="checkbox" value='<%# Eval("MASUCKHOE") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMASUCKHOE" runat="server" CommandArgument='<%# Eval("MASUCKHOE") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MASUCKHOE") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>   
                                <asp:TemplateField HeaderText="Tình trạng SK" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnTINHTRANGSK" runat="server"
                                            Text='<%# Eval("TINHTRANG") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Chiều cao" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnCHIEUCAO" runat="server"
                                            Text='<%# Eval("CHIEUCAO") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Cân nặng" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkBtnCANNANG" runat="server"
                                            Text='<%# Eval("CANNANG") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>                            
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel> 
        </asp:Panel>
    </div>    
    <br />
    <div class="crmcontainer" id="divDaoTaoBD" runat="server">
        <asp:Panel ID="HeaderDaoTaoBD" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageDaoTaoBD" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label5" runat="server" Font-Bold="True">ĐÀO TẠO - BỒI ĐƯỠNG</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>            
        </asp:Panel>
        <asp:Panel ID="ContentDaoTaoBD" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoDaoTaoBD" UpdateMode="Conditional" runat="server">
               <ContentTemplate>   
                   <div class="crmcontainer">
                        <table class="crmtable" >
                            <tbody>
                                <tr>
                                    <td class="crmcell right">Bồi dưỡng, đào tạo</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:DropDownList ID="ddlLOAIDTBD" runat="server" TabIndex="13">                                                
                                            </asp:DropDownList>
                                            <asp:Label ID="lbMADTBD" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td class="crmcell right">Bằng cấp</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:DropDownList ID="ddlBCDTBD" runat="server" TabIndex="13"/>
                                        </div>
                                    </td>
                                    <td class="crmcell right">Chuyên ngành</td>
                                    <td class="crmcell" >                                
                                        <div class="left">
                                            <asp:TextBox ID="txtCNDTBD" runat="server" MaxLength="50" TabIndex="12"  />
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="crmcell right">Thời gian BĐ học</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtTGHOC" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="ImageTGHOC" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender6" runat="server" TargetControlID="txtTGHOC"
                                                PopupButtonID="imgTGHOC" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                        </div>
                                    </td>
                                    <td class="crmcell right">Thời gian KT học</td>
                                    <td class="crmcell" >
                                        <div class="left">
                                            <asp:TextBox ID="txtTGKTHOC" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="ImagetxtTGKTHOC" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtTGKTHOC"
                                                PopupButtonID="imgtxtTGKTHOC" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                        </div>                        
                                    </td>

                                </tr>
                                <tr>
                                    <td class="crmcell right">Loại bằng</td>
                                    <td class="crmcell">
                                        <div class="left ">
                                            <asp:DropDownList ID="ddlLOAIBANGDTBD" runat="server" TabIndex="13"/>                                            
                                        </div>                                        
                                    </td>
                                </tr>
                                <tr>
                                    <td class="crmcell right">Tên trường</td>
                                    <td class="crmcell">
                                        <div class="left">
                                                <asp:TextBox ID="txtTENTRUONGDTBD" runat="server" MaxLength="100" TabIndex="12"  />
                                        </div>
                                        <td class="crmcell right">Địa chỉ</td>
                                        <td class="crmcell" >
                                            <div class="left">
                                                <asp:TextBox ID="txtDIACHIDTBD" runat="server" MaxLength="100" TabIndex="12"  />
                                            </div>                        
                                        </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="crmcell right vtop">Nội dung</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtNOIDUNGDTBD" runat="server" Width="600px" MaxLength="1000" TabIndex="4" TextMode="MultiLine" />                                           
                                        </div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td ></td>
                                    <td class="crmcell">                                 
                                        <div class="left">
                                            <asp:Button ID="btnSaveDTBD" OnClick="btnSaveDTBD_Click"
                                                UseSubmitBehavior="false" OnClientClick="return CheckFormSaveDTBD();" 
                                                runat="server" CssClass="save" Text="" TabIndex="11" />
                                        </div>       
                                    </td>
                                </tr>
                            </tbody>
                        </table>
                   </div>                
               </ContentTemplate>    
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upDaoTaoBD" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvDaoTaoBD" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvDaoTaoBD_RowCommand"  OnPageIndexChanging="gvDaoTaoBD_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="đào tạo - bồi dưỡng" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMADTBD" runat="server" type="hidden" value='<%# Eval("MADTBD") %>' />
                                        <input name="listIdsMADTBD" type="checkbox" value='<%# Eval("MADTBD") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMADTBD" runat="server" CommandArgument='<%# Eval("MADTBD") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MADTBD") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loại ĐT BD" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkCHUYENNGANHDTBD" runat="server"
                                            Text='<%# Eval("LLOAIDTBD.TENLOAI") + " - " + Eval("CHUYENNGANH") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Tên trường" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTENTRUDTBD" runat="server"
                                            Text='<%# Eval("TENTRUONG") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Địa chỉ" HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkdcDTBD" runat="server"
                                            Text='<%# Eval("DCDAOTAOBD") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>                               
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>    
    <br />
    <div class="crmcontainer" id="divKhenThuongKL" runat="server">
        <asp:Panel ID="HeaderKhenThuongKL" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageKhenThuongKL" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label6" runat="server" Font-Bold="True">KHEN THƯỞNG - KỶ LUẬT</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>            
        </asp:Panel>
        <asp:Panel ID="ContentKhenThuongKL" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoKhenThuongKL" UpdateMode="Conditional" runat="server">
               <ContentTemplate>   
                   <div class="crmcontainer">
                        <table class="crmtable" >
                            <tbody>
                                <tr>
                                    <td class="crmcell right">Khen thưởng, kỷ luật</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:DropDownList ID="ddlKHENTHUONGKL" runat="server" TabIndex="13">                                             
                                            </asp:DropDownList>
                                            <asp:Label ID="lbMAKTKL" runat="server" Visible="false"></asp:Label>
                                        </div>
                                    </td>                                   
                                </tr>
                                <tr>
                                    <td class="crmcell right">Từ ngày</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtTUNGAYKTKL" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="ImagetxtTUNGAYKTKL" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender7" runat="server" TargetControlID="txtTUNGAYKTKL"
                                                PopupButtonID="imgtxtTUNGAYKTKL" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                        </div>
                                        <td class="crmcell right">Đến ngày</td>
                                        <td class="crmcell" >
                                            <div class="left">
                                            <asp:TextBox ID="txtDENGAYKTKL" runat="server" width="80px" MaxLength="10"  TabIndex="7" />
                                            <asp:ImageButton runat="Server" ID="ImagetxtDENGAYKTKL" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                AlternateText="Click to show calendar"  Visible="false"/> 
                                            <ajaxToolkit:CalendarExtender ID="CalendarExtender9" runat="server" TargetControlID="txtDENGAYKTKL"
                                                PopupButtonID="imgtxtDENGAYKTKL" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                        </div>                        
                                        </td>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="crmcell right">Số quyết định</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtSOQUYETDINH" runat="server" MaxLength="50" TabIndex="12"  />
                                        </div>
                                    </td>
                                </tr>

                                <tr>
                                    <td class="crmcell right vtop">Nội dung</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtNDKTKL" runat="server" Width="600px" MaxLength="1000" TabIndex="4" TextMode="MultiLine" />                                           
                                        </div>
                                    </td>
                              </tr>
                                <tr>    
                                    <td ></td>
                                    <td class="crmcell">                                 
                                        <div class="left">
                                            <asp:Button ID="btnSaveKTKL" OnClick="btnSaveKTKL_Click"
                                                UseSubmitBehavior="false" OnClientClick="return CheckFormSaveKTKL();" 
                                                runat="server" CssClass="save" Text="" TabIndex="11" />
                                        </div>       
                                    </td>
                             </tr>
                            </tbody>
                        </table>
                   </div>                
               </ContentTemplate>    
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upKhenThuongKL" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvKhenThuongKL" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvKhenThuongKL_RowCommand"  OnPageIndexChanging="gvKhenThuongKL_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="đào tạo - bồi dưỡng" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMAKTKL" runat="server" type="hidden" value='<%# Eval("MAKTKL") %>' />
                                        <input name="listIdsMAKTKL" type="checkbox" value='<%# Eval("MAKTKL") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMAKTKL" runat="server" CommandArgument='<%# Eval("MAKTKL") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MAKTKL") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>     
                                <asp:TemplateField HeaderText="Loại " HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkKHENTKL" runat="server"
                                            Text='<%# Eval("LLOAIKYLUATKT.TENLOAI") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField> 
                                <asp:TemplateField HeaderText="Nội dung " HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkNOIDUNGKLKTL" runat="server"
                                            Text='<%# Eval("NOIDUNG") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>                         
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <br />
    <div class="crmcontainer" id="divGiaDinh" runat="server">
        <asp:Panel ID="HeaderGiaDinh" runat="server" CssClass="crmcontainer">
            <table class="crmtable">
                <tbody>
                    <tr class="crmfilter">
                        <td class="crmcell">
                            <div class="wrap">
                                <asp:ImageButton ID="ImageGiaDinh" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                    AlternateText="Hiện bộ lọc" />
                            </div>
                            <div class="wrap">
                                <asp:Label ID="Label7" runat="server" Font-Bold="True">HOÀN CẢNH KINH TẾ - QUAN HỆ GIA ĐÌNH</asp:Label>
                            </div>
                        </td>
                    </tr>
                </tbody>
            </table>            
        </asp:Panel>
        <asp:Panel ID="ContentGiaDinh" runat="server" CssClass="crmcontainer cleantop">
            <asp:UpdatePanel ID="upinfoGiaDinh" UpdateMode="Conditional" runat="server">
               <ContentTemplate>   
                   <div class="crmcontainer">
                        <table class="crmtable" >
                            <tbody>
                                <tr>
                                    <td class="crmcell right">Mối quan hệ</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:DropDownList ID="ddlQUANHEGD" runat="server" TabIndex="13"/>
                                            <asp:Label ID="lbMAQHGD" runat="server" Visible="false"></asp:Label>
                                        </div>                                        
                                    </td>                                   
                                </tr>
                                <tr>
                                  <td class="crmcell right">Họ tên</td>
                                  <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtTENQHGD" runat="server" MaxLength="100" TabIndex="12"  />
                                        </div>
                                  </td>                                
                                  <td class="crmcell right">Ngày sinh</td>
                                  <td class="crmcell" >
                                            <div class="left">
                                                <asp:TextBox ID="txtNGAYSINHQHGD" runat="server" width="80px" MaxLength="10"  TabIndex="7" />   
                                                        
                                                <asp:ImageButton runat="Server" ID="ImagetxtNGAYSINHQHGD" ImageUrl="~/content/images/icons/calendar.png" TabIndex="8"
                                                    AlternateText="Click to show calendar"  Visible="false"/> 
                                                <ajaxToolkit:CalendarExtender ID="CalendarExtender10" runat="server" TargetControlID="txtNGAYSINHQHGD"
                                                    PopupButtonID="imgtxtNGAYSINHQHGD" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                            </div>
                                  </td>
                                  <td class="crmcell" >
                                            <div class="left">
                                                <asp:CheckBox ID="ckNAMSINHQHGD" runat="server" TabIndex="28" oncheckedchanged="ckNAMSINHQHGD_CheckedChanged" 
                                                    AutoPostBack="True" />
                                            </div>
                                            <div class="left">
                                                 <strong>Năm sinh</strong>
                                                 <asp:TextBox ID="txtNAMSINHQHGD" runat="server" width="50px" MaxLength="4"  TabIndex="7" Visible="false" />  
                                            </div>
                                   </td>
                          
                                </tr>
                                <tr>
                                    <td class="crmcell right">Dân tộc</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:DropDownList ID="ddlDTQHGD" runat="server"  TabIndex="12"/>
                                        </div>
                                    </td>
                                    <td class="crmcell right">Tôn giáo</td>
                                    <td class="crmcell" >                               
                                        <div class="left">
                                            <asp:DropDownList ID="ddlTGQHGD" runat="server" TabIndex="13"/>
                                        </div>
                                    </td>
                               </tr>
                                <tr>
                                    <td class="crmcell right">Quên quán</td>
                                    <td class="crmcell">
                                        <div class="left">
                                                <asp:TextBox ID="txtQUEQUANQHGD" runat="server" MaxLength="100" TabIndex="12"  />
                                        </div>
                                        <td class="crmcell right">Nghề nghiệp</td>
                                        <td class="crmcell" >
                                            <div class="left">
                                                <asp:TextBox ID="txtNGHENGHIEQHGD" runat="server" MaxLength="100" TabIndex="12"  />
                                            </div>                        
                                        </td>
                                    </td>
                                </tr> 
                                <tr>
                                    <td class="crmcell right">Đ.vị công tác</td>
                                    <td class="crmcell">
                                        <div class="left">
                                                <asp:TextBox ID="txtDVCONGTACQHGD" runat="server" MaxLength="100" TabIndex="12"  />
                                        </div>
                                    </td>
                                </tr>                           
                                <tr>
                                    <td class="crmcell right vtop">Ghi chú</td>
                                    <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtGHICHUQHGD" runat="server" Width="600px" MaxLength="1000" TabIndex="4" TextMode="MultiLine" />                                           
                                        </div>
                                    </td>
                                </tr>
                                <tr>    
                                    <td ></td>
                                    <td class="crmcell">                                 
                                        <div class="left">
                                            <asp:Button ID="btnSaveQUANHEGD" OnClick="btnSaveQUANHEGD_Click"
                                                UseSubmitBehavior="false" OnClientClick="return CheckFormSaveQUANHEGD();" 
                                                runat="server" CssClass="save" Text="" TabIndex="11" />
                                        </div>       
                                    </td>
                             </tr>
                            </tbody>
                        </table>
                   </div>                
               </ContentTemplate>    
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upGiaDinh" UpdateMode="Conditional" runat="server">
                <ContentTemplate>
                    <div class="crmcontainer">
                        <eoscrm:Grid ID="gvGiaDinh" runat="server" UseCustomPager="True" PageSize="5"
                            OnRowCommand="gvGiaDinh_RowCommand"  OnPageIndexChanging="gvGiaDinh_PageIndexChanging" EnableModelValidation="True" PagerInforText="">
                            <PagerSettings FirstPageText="đào tạo - bồi dưỡng" PageButtonCount="2" />
                            <Columns>
                                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                                    <HeaderTemplate>
                                        <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                            onclick="CheckAllItems(this);" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="IdMAQHGDL" runat="server" type="hidden" value='<%# Eval("MAQHGD") %>' />
                                        <input name="listIdsMAQHGD" type="checkbox" value='<%# Eval("MAQHGD") %>' />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="checkbox" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mã " HeaderStyle-Width="18%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lkMAQHGD" runat="server" CommandArgument='<%# Eval("MAQHGD") %>'
                                            CommandName="EditItem" CssClass="link" Text='<%# Eval("MAQHGD") %>'></asp:LinkButton>
                                    </ItemTemplate>
                                    <HeaderStyle Width="100px" />
                                    <ItemStyle Font-Bold="True" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Mối " HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkLOAIQHGD" runat="server"
                                            Text='<%# Eval("LLOAIQHGD.TENQHGD") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Loại " HeaderStyle-Width="30%">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lnkTENQHGD" runat="server"
                                            Text='<%# Eval("TEN") %>'></asp:LinkButton>
                                    </ItemTemplate>                                    
                                </asp:TemplateField>                            
                            </Columns>
                        </eoscrm:Grid>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    
    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterLyLich" runat="Server" 
        TargetControlID="ContentLyLich"
        ExpandControlID="HeaderLyLich" 
        CollapseControlID="HeaderLyLich" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageLyLich"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterTrinhDo" runat="Server" 
        TargetControlID="ContentTrinhDo"
        ExpandControlID="HeaderTrinhDo" 
        CollapseControlID="HeaderTrinhDo" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageTrinhDo"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterQuaTrinhCT" runat="Server" 
        TargetControlID="ContentQuaTrinhCT"
        ExpandControlID="HeaderQuaTrinhCT" 
        CollapseControlID="HeaderQuaTrinhCT" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageQuaTrinhCT"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterQuanDoi" runat="Server" 
        TargetControlID="ContentQuanDoi"
        ExpandControlID="HeaderQuanDoi" 
        CollapseControlID="HeaderQuanDoi" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageQuanDoi"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterSucKhoeST" runat="Server" 
        TargetControlID="ContentSucKhoeST"
        ExpandControlID="HeaderSucKhoeST" 
        CollapseControlID="HeaderSucKhoeST" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageSucKhoeST"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterDaoTaoBD" runat="Server" 
        TargetControlID="ContentDaoTaoBD"
        ExpandControlID="HeaderDaoTaoBD" 
        CollapseControlID="HeaderDaoTaoBD" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageDaoTaoBD"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterKhenThuongKL" runat="Server" 
        TargetControlID="ContentKhenThuongKL"
        ExpandControlID="HeaderKhenThuongKL" 
        CollapseControlID="HeaderKhenThuongKL" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageKhenThuongKL"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />

    <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilterGiaDinh" runat="Server" 
        TargetControlID="ContentGiaDinh"
        ExpandControlID="HeaderGiaDinh" 
        CollapseControlID="HeaderGiaDinh" 
        Collapsed="true"
        TextLabelID="Text"
        ImageControlID="ImageGiaDinh"         
        ExpandedImage="~/content/images/icons/collapsed.png" 
        CollapsedImage="~/content/images/icons/expanded.png"
        SuppressPostBack="true" />
</asp:Content>
