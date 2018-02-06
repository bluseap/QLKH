<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="True" 
    CodeBehind="DSKHTieuThu.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.BaoCao.DSKHTieuThu" %>
    
<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="System.Data" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register Assembly="CrystalDecisions.Web,  Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
    
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

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormReport();
            }
        }

       function CheckFormReport() {
           var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

           if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
               showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
               return false;
           }

           var klt = jQuery.trim($("#<%= txtKLTu.ClientID %>").val());
           var kld = jQuery.trim($("#<%= txtKLDen.ClientID %>").val());
           var ttt = jQuery.trim($("#<%= txtTongTienTu.ClientID %>").val());
           var ttd = jQuery.trim($("#<%= txtTongTienDen.ClientID %>").val());

           if (!IsNumeric(klt) || !IsNumeric(kld) || !IsNumeric(ttt) || !IsNumeric(ttd) ||
                parseInt(klt) < 0 || parseInt(kld) < 0 || parseFloat(ttt) < 0 || parseFloat(ttd) < 0) {
               showError('Nhập chỉ số lọc hợp lệ.');
               return false;
           }
       
           openWaitingDialog();
           unblockWaitingDialog();

           __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
       }
        
        function CheckFormEXCELTT6TG() {
            //openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkEXCELTT6TG) %>', '');
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
    <asp:UpdatePanel ID="upnlGhiChiSo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <asp:Panel ID="headerPanel" runat="server" CssClass="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr class="crmfilter">
                            <td class="crmcell">
                                <div class="wrap">
                                    <asp:ImageButton ID="imgCollapse" runat="server" ImageUrl="~/content/images/icons/expanded.png"
                                        AlternateText="Hiện bộ lọc" />
                                </div>
                                <div class="wrap">
                                    <asp:Label ID="lblCollapse" runat="server">Click vào để hiển thị bộ lọc</asp:Label>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>    
            </asp:Panel>
            <asp:Panel ID="contentPanel" runat="server" CssClass="crmcontainer cleantop">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">
                                Tháng
                            </td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:DropDownList ID="cboTHANG" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <div class="right">Mã đường</div>
                                </div>
                                <div class="left width-150">
                                    <asp:TextBox ID="txtMaDp" onkeydown="return CheckChangeDP(event);" runat="server" Width="40px" />&nbsp;
                                    <asp:TextBox ID="txtDuongPhu" runat="server" Width="25px" />&nbsp;                            
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="6" />
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="cboKhuVuc" runat="server" Width="165px">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Mục đích sử dụng
                            </td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:DropDownList ID="cboMucDichSuDung" runat="server" Height="24px" Width="100px">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Trạng thái</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="cboTrangThai" runat="server" Width="100px">
                                        <asp:ListItem Value="%">Tất cả</asp:ListItem>
                                        <asp:ListItem Value="CUP">Cúp</asp:ListItem>
                                        <asp:ListItem Value="MO">Mở</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khối lượng từ:
                            </td>
                            <td class="crmcell">
                                <div class="left width-150">
                                    <asp:TextBox ID="txtKLTu" runat="server" Width="50px" MaxLength="5" TabIndex="2"  Text ="0" />
                                    <strong> Đến </strong>
                                    <asp:TextBox ID="txtKLDen" runat="server" Width="50px" MaxLength="5" TabIndex="2" Text = "9999" />
                                </div>
                                <div class="left">
                                    <div class="right">Tiền nước từ</div>
                                </div>
                                <div class="left width-150">
                                    <asp:TextBox ID="txtTongTienTu" runat="server" Width="50px" MaxLength="16" TabIndex="2" Text = "0" />
                                    <strong> Đến </strong>
                                    <asp:TextBox ID="txtTongTienDen" runat="server" Width="50px" MaxLength="16" TabIndex="2" Text = "9999999999" />
                                </div>
                                <div class="left">
                                    <div class="right">Người lập</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNguoiLap" runat="server" Width="150px" />
                                </div>
                               <div class="left">
                                    <asp:Label ID="reloadm" runat="server" Visible="False" />
                                </div>
                            </td>
                        </tr>   
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" runat="server" CssClass="report"  
                                        OnClientClick="return CheckFormReport();" OnClick="btnBaoCao_Click" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"> </td>
                            <td class="crmcell left">
                                <div class="right">
                                    <asp:LinkButton ID="linkBang"  runat="server" onclick="linkBang_Click" 
                                        OnClientClick="return CheckFormReport();" >
                                        Tiêu thụ 3 tháng bằng nhau.
                                    </asp:LinkButton>
                                </div>
                                <div class="right">
                                    <asp:LinkButton ID="linkGiam"  runat="server" onclick="linkGiam_Click" 
                                        OnClientClick="return CheckFormReport();" >
                                        Tiêu thụ liên tục giảm trong 3 tháng.
                                    </asp:LinkButton>
                                </div>
                                <div class="right">
                                    <asp:LinkButton ID="linkkhd"  runat="server" onclick="linkkhd_Click" 
                                        OnClientClick="return CheckFormReport();" >
                                        Danh sách không ra hoá đơn có lý do.
                                    </asp:LinkButton>
                                </div>
                                <div class="right">
                                    <asp:LinkButton ID="linkchd"  runat="server" onclick="linkchd_Click" 
                                        OnClientClick="return CheckFormReport();" >
                                        Danh sách không ra hoá đơn không có lý do.
                                    </asp:LinkButton>
                                </div>
                                <div class="right">
                                    <asp:LinkButton ID="linkinhopdong"  runat="server" onclick="linkinhopdong_Click" 
                                        OnClientClick="return CheckFormReport();" >
                                        In hợp đồng
                                    </asp:LinkButton>
                                </div>   
                                <div class="left">
                                    <div class="left">
                                        <asp:DropDownList ID="ddlTTG6THANG" runat="server" >
                                            <asp:ListItem Value="TT6GBLT">TT 6 tháng giảm hoặc bằng </asp:ListItem>
                                            <asp:ListItem Value="TT6GLT">TT 6 tháng giảm liên tục</asp:ListItem>                                            
                                        </asp:DropDownList>
                                    </div>
                                    <div class="right">
                                        <asp:LinkButton ID="lkEXCELTT6TG" runat="server"  UseSubmitBehavior="false" CssClass="myButton"  
                                            OnClientClick="return CheckFormEXCELTT6TG();" OnClick="lkEXCELTT6TG_Click">Xuất Excel</asp:LinkButton>
                                    </div>
                                </div>                             
                            </td>
                        </tr>
                    </tbody>
                </table>
            </asp:Panel>
            <ajaxToolkit:CollapsiblePanelExtender ID="cpeFilter" runat="Server" 
                Collapsed="false"
                TargetControlID="contentPanel"
                ExpandControlID="headerPanel" 
                CollapseControlID="headerPanel" 
                TextLabelID="lblCollapse"
                ImageControlID="imgCollapse" 
                ExpandedText="Click vào để ẩn bộ lọc" 
                CollapsedText="Click vào để hiển thị bộ lọc"
                ExpandedImage="~/content/images/icons/collapsed.png" 
                CollapsedImage="~/content/images/icons/expanded.png" />
                
            </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lkEXCELTT6TG" />
        </Triggers>
    </asp:UpdatePanel>
    <br />            
    <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                 <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" DisplayGroupTree="False" />       
            </div>            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>        
    </asp:UpdatePanel>    
</asp:Content>
