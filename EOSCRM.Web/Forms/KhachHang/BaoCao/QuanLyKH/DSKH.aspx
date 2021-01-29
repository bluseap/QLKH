<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    CodeBehind="DSKH.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.Dskh" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">

    <script type="text/javascript">
        $(document).ready(function() {
            $("#divDuongPho").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divDuongPhoDlgContainer");
                }
            });
        });

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
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
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHU" HeaderText="Đường phụ" />
                                            <asp:BoundField HeaderStyle-Width="50%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Khu vực">
                                                <ItemTemplate>
                                                    <%# Eval("KHUVUC.TENKV") %>
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
    <asp:UpdatePanel ID="upnlBaoCao" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right"> Kỳ khai thác                               
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANG" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                </div>
                                <td class="crmcell right">Khu vực</td>
                                <td class="crmcell">  
                                    <div class="left">
                                        <asp:DropDownList ID="cboKhuVuc" runat="server">
                                        </asp:DropDownList>
                                    </div>
                                </td>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"> Trạng thái                              
                            </td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="cboTrangThai" runat="server" Width="150px" OnSelectedIndexChanged="cboTrangThai_SelectedIndexChanged">
                                        <asp:ListItem Value ="%">Tất cả</asp:ListItem>
                                        <asp:ListItem Value="GDH_BT" Text="Bình thường" />                                    
                                        <asp:ListItem Value="I" Text="I_Ít sử dụng" />
                                        <asp:ListItem Value="THAY" Text="Thay ĐH" />
                                        <asp:ListItem Value="CUP" Text="N_Cúp" />
                                        <asp:ListItem Value="D" Text="D_Đóng cửa" />                                    
                                        <asp:ListItem Value="L" Text="L_Lố số" />
                                        <asp:ListItem Value="M" Text="M_Bỏ địa phương" />
                                        <asp:ListItem Value="C" Text="C_Cháy - Chết" />
                                        <asp:ListItem Value="H" Text="H_Hư hỏng" />
                                        <asp:ListItem Value="B" Text="Bất thường" />
                                        <asp:ListItem Value="Q" Text="Q_Điều chỉnh - Qua số" />                                    
                                        <asp:ListItem Value="GDH_KH" Text="Khoán" />
                                        <asp:ListItem Value="K" Text="K_Khác" />
                                    </asp:DropDownList>
                                </div>
                                <td class="crmcell right">Người lập</td>
                                <td class="crmcell" >  
                                    <div class="left">
                                        <asp:TextBox ID="txtNguoiLap" runat="server"  />
                                    </div>
                                </td>                        
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" runat="server"  CssClass="report" 
                                        OnClick="btnBaoCao_Click" OnClientClick="return CheckFormReport();" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">                                
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMaDp" runat="server" Width="40px" Visible="False" OnTextChanged="txtMaDp_TextChanged" />
                                    <asp:TextBox ID="txtDuongPhu" runat="server" Width="25px" Visible="False" OnTextChanged="txtDuongPhu_TextChanged" />
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="6" Visible="False" />
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>
                                <div class="left width-100">
                                     <asp:CheckBox ID = "chkLocHaiGia" runat ="server" Visible="False"  />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="cboMucDichSuDung" runat="server" Width="150px" Visible="False" OnSelectedIndexChanged="cboMucDichSuDung_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                     />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
