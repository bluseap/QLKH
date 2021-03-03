<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" EnableEventValidation="true" CodeBehind="NhapDonLapMoiPower.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.NhapDonLapMoiPower" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divDuongPho").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDuongPhoDlgContainer");
                }
            });
        });

        function checkDuongPhoForm() {
            // check text boxes have at least one value
            var dp = jQuery.trim(document.getElementById('<%= txtMADP.ClientID %>').value);
            if (dp != '') {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnHidden) %>', '');
            }
            return false;
        }

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
            return false;
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckFormFitler() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');
            return false;
        }

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');
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

        function checkCMNDForm() {
            // check text boxes have at least one value
            var dp = jQuery.trim(document.getElementById('<%= txtCMND.ClientID %>').value);
            if (dp != '') {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkKTCMND) %>', '');
            }
            return false;
        }
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDuongPhoDlgContainer">
        <div id="divDuongPho" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
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
                                                    <asp:TextBox ID="txtKeywordDP" onchange="return CheckFormFilterDP();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDP" OnClick="btnFilterDP_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDP();" 
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
							        <eoscrm:Grid ID="gvDuongPho" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDuongPho_RowDataBound" OnRowCommand="gvDuongPho_RowCommand" 
							            OnPageIndexChanging="gvDuongPho_PageIndexChanging">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADPPO").ToString() %>' CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <%--<asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHU" HeaderText="Đường phụ" />--%>
                                            <asp:BoundField HeaderStyle-Width="60%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Khu vực">
                                                <ItemTemplate>
                                                    <%# Eval("KHUVUCPO.TENKV") %>
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
                            <td class="crmcell right width-125"> </td>
                            <td class="crmcell width-250">    
                                <div class="left">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="95px" MaxLength="10" 
                                        TabIndex="1" ReadOnly="True" Visible="false"/>                            
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbDAIDIEN" Enabled="false" runat="server" Visible="false" />
                                </div>
                                <div class="left width-50">
                                    <strong> </strong>
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlMADDKTONG" Enabled="false" Width="140px" TabIndex="2" 
                                        runat="server" Visible="False" >
                                    </asp:DropDownList>
                                    <asp:Label ID="lbMAPHONG" runat="server" Text="MAPHONG" Visible="false"></asp:Label>
                                    <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" TabIndex="3" runat="server" Visible=" false">
                                    </asp:DropDownList>
                                </div>
                                </div>
                            </td>                             
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên trạm</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" onblur="checkDuongPhoForm();" runat="server" Width="40px" MaxLength="200" TabIndex="99" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MADP</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="10px" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTENTRAMPO" runat="server"></asp:Label>
                                </div>                              
                            </td>
                            <td class="crmcell right">Số HK thường trú</td>
                            <td class="crmcell" colspan="3">    
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHIKHAC" runat="server" Width="410px" MaxLength="200" TabIndex="10" />
                                </div>                          
                            </td> 
                        </tr>                        
                        <tr>    
                            <td class="crmcell right">Họ tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" Width="180px" MaxLength="200" TabIndex="4" />
                                </div>                                
                            </td>
                            <td class="crmcell right">Số hộ dùng chung</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHODN" runat="server" Width="90px" MaxLength="4" TabIndex="11" />
                                </div>                                
                            </td>
                            <td class="crmcell right">Số nhân khẩu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONK" runat="server" Width="90px" MaxLength="4" TabIndex="12" />
                                </div>
                            </td>                                                                         
                        </tr>
                        <tr>
                            <td class="crmcell right">Năm sinh</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYSINH" runat="server" Width="100px" MaxLength="4" TabIndex="5" />
                                </div>
                                <%--<div class="left">
                                    <asp:ImageButton runat="Server" ID="imgNGAYSINH" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="False" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtNGAYSINH"
                                        PopupButtonID="imgNGAYSINH" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>--%>
                                <td class="crmcell right">Mục đích sử dụng</td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:DropDownList ID="ddlMUCDICH" Width="130px" TabIndex="13" runat="server">
                                        </asp:DropDownList>
                                    </div>                                
                                <td class="crmcell right">HT thanh toán</td>
                                <td class="crmcell"> 
                                    <div class="left">
                                         <asp:DropDownList ID="ddlHTTT" Width="100px" runat="server" TabIndex="14">
                                         </asp:DropDownList>
                                    </div>
                                </td>    
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số nhà, tên đường KH</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSoNhaTenDuongKH" runat="server" Width="180px" MaxLength="150" 
                                        TabIndex="6" />
                                </div>                                
                                <td class="crmcell right">
                                    <div class="right">
                                        <asp:DropDownList ID="ddlTinhKH" TabIndex="3" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlTinhKH_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </div>   
                                </td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:DropDownList ID="ddlThanhPhoHuyenKH" TabIndex="3" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlThanhPhoHuyenKH_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlPhuongXaKH" TabIndex="3" runat="server" AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlPhuongXaKH_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlToApKH" TabIndex="3" runat="server"  AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlToApKH_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                    </div>                
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số nhà, tên đường địa chỉ lắp</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSoNhaTenDuongDCL" runat="server" Width="180px" MaxLength="150" 
                                        TabIndex="6" />
                                </div>                                
                                <td class="crmcell right">
                                    <div class="right">
                                        <asp:DropDownList ID="ddlTinhDCL" TabIndex="3" runat="server" >
                                        </asp:DropDownList>
                                    </div>   
                                </td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:DropDownList ID="ddlThanhPhoHuyenDCL" TabIndex="3" runat="server"  AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlThanhPhoHuyenDCL_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlPhuongXaDCL" TabIndex="3" runat="server"  AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlPhuongXaDCL_SelectedIndexChanged" >
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlToApDCL" TabIndex="3" runat="server"   AutoPostBack="True" 
                                            OnSelectedIndexChanged="ddlToApDCL_SelectedIndexChanged" >
                                        </asp:DropDownList>                                           
                                    </div>                
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ thường trú</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA" runat="server" Width="180px" MaxLength="150" 
                                        TabIndex="6" />
                                </div>
                                <td class="crmcell right">Số nhà (Đ/c lắp)</td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:TextBox ID="txtSoNhaNhapDon" runat="server" Width="50px" MaxLength="200" TabIndex="15" />
                                    </div>
                                    <div class="left">
                                        <asp:Label ID="lbTenDuongNhapDon" runat="server" Text=",tên đường (ấp tổ)" Font-Bold="True" ></asp:Label>
                                    </div>
                                    <div class="left">
                                        <asp:TextBox ID="txtTenDuongNhapDon" runat="server" Width="180px" MaxLength="200" TabIndex="15" />
                                    </div>
                                    <div class="left">
                                        <asp:TextBox ID="txtDIACHILAPDAT" runat="server" Width="50px" MaxLength="200" TabIndex="15" Visible="false" />
                                    </div>
                                    <asp:Label ID="lbTextApXa" runat="server" Text="Chỉ nhập ấp, xã." Font-Bold="True" Visible="false"></asp:Label>                                                              
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell">    
                                <div class="left">                                    
                                </div>
                                <td class="crmcell right">Phường, xã (Đ/c lắp)</td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:DropDownList ID="ddlPhuongXa" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlPhuongXa_SelectedIndexChanged"></asp:DropDownList>
                                    </div> 
                                    <div class="left">
                                        <asp:CheckBox ID="ckDoiPhuongXa" runat="server" TabIndex="28" AutoPostBack="True" Text=" Phường, xã khác huyện" 
                                            OnCheckedChanged="ckDoiPhuongXa_CheckedChanged" />
                                    </div>                                                                                          
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Huyện, tỉnh</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtHUYEN" runat="server" Width="180px" MaxLength="150" 
                                        TabIndex="6" />
                                </div>
                                <td class="crmcell right">Huyện, tỉnh (Đ/C lắp)</td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:TextBox ID="txtHUYENDCLAP" runat="server" Width="180px" MaxLength="200" TabIndex="15" />
                                    </div>                                                                
                                </td>                            
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Số CMND (số KD)</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCMND" runat="server" Width="130px" MaxLength="20" onblur="checkCMNDForm();"
                                        TabIndex="7" />                                    
                                    <asp:LinkButton ID="lkKTCMND" CausesValidation="false" style="display:none"  
                                        OnClick="lkKTCMND_Click" runat="server"></asp:LinkButton>    
                                </div>
                                <td class="crmcell right">Điện thoại</td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:TextBox ID="txtDIENTHOAI" runat="server" Width="90px" MaxLength="20" TabIndex="16"  />
                                    </div>   
                                </td>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Cấp ngày</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCAPNGAY" runat="server" Width="100px" MaxLength="200" TabIndex="8" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgCAPNGAY" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender8" runat="server" TargetControlID="txtCAPNGAY"
                                        PopupButtonID="imgCAPNGAY" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <td class="crmcell right">MST</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtMST" runat="server" Width="90px" MaxLength="30" TabIndex="16" />
                                    </div>
                                </td>
                            </td>
                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tại</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTAI" runat="server" Width="200px" MaxLength="30" 
                                        TabIndex="9" />
                                </div>
                                <td class="crmcell right">Ngày nhận đơn</td>
                                <td class="crmcell">
                                        <div class="left">
                                            <asp:TextBox ID="txtNGAYCD" runat="server" MaxLength="200" TabIndex="17" 
                                                Width="90px" />
                                        </div>
                                        <div class="left">
                                            <asp:ImageButton ID="imgNHANDON" runat="Server" 
                                                AlternateText="Click to show calendar" 
                                                ImageUrl="~/content/images/icons/calendar.png" />
                                            <ajaxToolkit:CalendarExtender ID="calendarButtonExtender1" runat="server" 
                                                Format="dd/MM/yyyy" PopupButtonID="imgNHANDON" TargetControlID="txtNGAYCD" 
                                                TodaysDateFormat="dd/MM/yyyy" />
                                        </div>
                                </td>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" Width="142px" AutoPostBack="True"
                                        TabIndex="33" runat="server" onchange="openWaitingDialog(); unblockWaitingDialog();"  
                                        onselectedindexchanged="ddlKHUVUC_SelectedIndexChanged" />
                                </div>
                                 <div class="left">
                                    <asp:TextBox ID="txtDMNK" runat="server" Width="130px" MaxLength="20" TabIndex="7" Visible="false"/>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDINHMUC" runat="server" Width="130px" MaxLength="20" TabIndex="7" Visible="false"/>
                                </div>
                                <td class="crmcell right">Hẹn khảo sát</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtNGAYKS" runat="server" Width="90px" MaxLength="200" TabIndex="17" />
                                    </div>
                                    <div class="left">
                                        <asp:ImageButton runat="Server" ID="imgKHAOSAT" ImageUrl="~/content/images/icons/calendar.png"
                                            AlternateText="Click to show calendar" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtNGAYKS"
                                            PopupButtonID="imgKHAOSAT" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                    </div>
                                </td>                                
                            </td>                            
                        </tr>                        
                        </tr>
                        <tr>
                            <td class="crmcell right">Đ/c lắp </td>
                            <td class="crmcell">                                  
                                <div class="left">
                                    <asp:TextBox ID="txtNOILAPDHN" runat="server" MaxLength="200" TabIndex="18" />
                                </div>
                                <td class="crmcell right">Số trụ</td>
                                <td class="crmcell" colspan="3">    
                                    <div class="left">
                                        <asp:TextBox ID="txtSOTRUPO" runat="server" width="70px" MaxLength="50" TabIndex="18" />
                                    </div>                             
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">HỒ SƠ KÈM THEO</td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:CheckBox ID="ckHK" runat="server" Text="  Hộ khẩu thường trú photo" TabIndex="18"/>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckCM" runat="server" Text="  CMND photo" TabIndex="19"/>
                                </div>
                             
                                <div class="left">
                                    <asp:CheckBox ID="ckXN" runat="server" Text="  Xác nhận địa phương" TabIndex="20"/>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckTAMTRU" runat="server" Text="  Giấy chứng nhận tạm trú dài hạn" TabIndex="21"/>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckHONGHEOKK" runat="server" Text="  Hộ nghèo / Cận nghèo / HC Khó khăn" TabIndex="21"/>
                                </div>                                                                
                            </td> 
                            <td class="crmcell">
                                <div class="left">
                                    <asp:CheckBox ID="ckCNNHA" runat="server" Text="  Giấy chứng nhận sở hữu nhà" TabIndex="18"/>
                                </div>
                                 <div class="left">
                                    <asp:CheckBox ID="ckHDTHUE" runat="server" Text="  Hợp đồng thuê nhà" TabIndex="18"/>
                                </div>
                                 <div class="left">
                                    <asp:CheckBox ID="ckDKKD" runat="server" Text="  Giấy chứng nhận ĐKKD (đ/v SX-KD-DV)" TabIndex="18"/>
                                </div>
                                 <div class="left">
                                    <asp:CheckBox ID="ckDKTHUE" runat="server" Text="  Giấy chứng nhận đăng ký thuế" TabIndex="18"/>
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckSoHuuDat" runat="server" Text="  Giấy chứng nhận sở hữu đất (Photo,công chứng)" TabIndex="18"/>
                                </div>
                                 <div class="left">
                                    <asp:CheckBox ID="ckThoaThuanKhachHang" runat="server" Text="  Giấy thỏa thuận với khách hàng khác" TabIndex="18"/>
                                </div>
                            </td>                       
                        </tr>
                        <tr>
                            <td class="crmcell right">Người uỷ quyền</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtUYQUYEN" runat="server" Width="200px" />
                                </div>
                             </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNOIDUNG"  Width="300" TabIndex="118"
                                        MaxLength="1000" runat="server" Font-Names="Times New Roman" 
                                        Visible="False" />
                                </div>
                            </td>
                         </tr>                                                                   
                         <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell" colspan="5">
                                <div class="left">
                                    <asp:CheckBox ID="cbISTUYENONGCHUNG" runat="server" TabIndex="34" 
                                        Visible="False" />
                                </div>
                                <div class="left"><strong></strong></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="header" colspan="6">
                                <div class="left">
                                    Đổi thông tin xuất hóa đơn
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="cbSDInfo_INHOADON" runat="server" TabIndex="34" />                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH_INHOADON" MaxLength="200" runat="server" Width="300px" TabIndex="38" />
                                </div>
                            </td>
                            <td class="crmcell right">Địa chỉ</td>
                            <td class="crmcell" colspan="3">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI_INHOADON" MaxLength="200" runat="server" Width="400px" TabIndex="39" />
                                </div>
                            </td>
                        </tr>                        
                        <tr>    
                            <td class="crmcell right btop"></td>
                            <td class="crmcell btop" colspan="5">
                                <div class="left">
                                    <asp:Button ID="btnFilter" runat="server" CssClass="filter"
                                        OnClick="btnFilter_Click" OnClientClick="return CheckFormFitler();" TabIndex="23" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" OnClientClick="return CheckFormSave();"
                                        OnClick="btnSave_Click" TabIndex="22" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                         OnClientClick="return CheckFormCancel();" TabIndex="24" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="25" UseSubmitBehavior="false" Visible="false"/>
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_OnRowDataBound" 
                    OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDKPO") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDKPO") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>            
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã đơn&nbsp;">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDKPO") %>'
                                    CommandName="EditHoSo" Text='<%# Eval("MADDKPO") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="34%" HeaderText="Tên khách hàng" DataField="TENKH" />
                        <asp:BoundField HeaderStyle-Width="10%" HeaderText="Điện thoại" DataField="DIENTHOAI" />
                        <asp:BoundField HeaderStyle-Width="34%" HeaderText="Địa chỉ lắp đặt" DataField="DIACHILD" />
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Ngày đăng ký&nbsp;">
                            <ItemTemplate>
                                <%# (Eval("NGAYDK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYDK"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
               </eoscrm:Grid>
           </div>
           <br />
           <div class="crmcontainer p-5">
                <a href="/Forms/ThietKe/Power/KhaoSatThietKePo.aspx">Chuyển sang bước kế tiếp</a>
           </div>
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
