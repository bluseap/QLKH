<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="True"
    CodeBehind="BCDSKHMoi.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH.BCDSKHMoi" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="System.Data" %>
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

        function CheckFormExcel() { 
            //openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkEXCEL) %>', '');
        }

        function CheckFormlkExcelLX() {            
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkExcelLX) %>', '');
        }
        
        function CheckFormWord() {            
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkWORD) %>', '');
        }
        
        function CheckFormExcelTS() {            
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkXuatExcelTS) %>', '');
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
                             <td class="crmcell right">Tháng</td>
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
                                    <asp:Label ID="lbRELOAD" runat="server" Visible="false"></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbDenThang" runat="server" Text="đến tháng" Font-Bold="True"></asp:Label>
                                </div>
                                 <div class="left width-150">
                                    <asp:DropDownList ID="ddlDenThang" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtDenNam" runat="server" Width="30px" MaxLength="4" TabIndex="2" />  
                                    <asp:Label ID="lbGhiChuDenThang" runat="server" Text="(Lấy tháng, năm nhập thi công đồng hồ)" Font-Bold="True"></asp:Label>                                  
                                </div>
                             </td>
                        </tr>
                        <tr>                           
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="cboKhuVuc" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </div>
                                <div class="left width-100">
                                    <asp:TextBox ID="txtMaDp" runat="server" Width="25px" Visible="false"/>
                                    <asp:TextBox ID="txtDuongPhu" runat="server" Width="25px" Visible="false" />
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="6" Visible="false"/>
                                </div>
                                <div class="left">
                                    <div class="right"></div>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Phiên (Long Xuyên)</div>
                                </div>
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlPHIENLX" runat="server">
                                        <asp:ListItem Value ="%">Tất cả</asp:ListItem>
                                        <asp:ListItem Value="1">1</asp:ListItem>
                                        <asp:ListItem Value="2">2</asp:ListItem>
                                        <asp:ListItem Value="3">3</asp:ListItem>
                                        <asp:ListItem Value="4">4</asp:ListItem>
                                        <asp:ListItem Value="5">5</asp:ListItem>
                                        <asp:ListItem Value="6">6</asp:ListItem>
                                        <asp:ListItem Value="A">A</asp:ListItem>
                                    </asp:DropDownList>
                                </div>                                
                            </td>                            
                        </tr> 
                        <tr>
                            <td class="crmcell right">Mục đích sử dụng</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="cboMucDichSuDung" runat="server" Width="150px">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Trạng thái</div>
                                </div>
                                <div class="left width-100">
                                    <asp:DropDownList ID="cboTrangThai" runat="server" Width="150px">
                                        <asp:ListItem Value ="%">Tất cả</asp:ListItem>
                                        <asp:ListItem Value="CUP">Cúp</asp:ListItem>
                                        <asp:ListItem Value="MO">Mở</asp:ListItem>
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Người lập</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtNguoiLap" runat="server"  Width="225px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" runat="server"  CssClass="report" 
                                        OnClick="btnBaoCao_Click" OnClientClick="return CheckFormReport();" />
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkEXCEL" runat="server" OnClick="lkEXCEL_Click" UseSubmitBehavior="false" CssClass="myButton"  
                                            OnClientClick="return CheckFormExcel();">Xuất Excel</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkXuatExcelTS" runat="server" UseSubmitBehavior="false" CssClass="myButton"  Visible="false"
                                            OnClientClick="return CheckFormExcelTS();" OnClick="lkXuatExcelTS_Click" >Xuất Excel(TS)</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkWORD" runat="server" OnClick="lkWORD_Click" UseSubmitBehavior="false" CssClass="myButton"  
                                            OnClientClick="return CheckFormWord();" Visible="False">Xuất Word</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="left">
                                    <div class="right">
                                        <asp:LinkButton ID="lkExcelLX" runat="server" OnClick="lkExcelLX_Click" UseSubmitBehavior="false" CssClass="myButton"  
                                            Visible="false" OnClientClick="return CheckFormlkExcelLX();">Xuất Excel (LX)</asp:LinkButton>
                                    </div>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="lkEXCEL" />
            <asp:PostBackTrigger ControlID="lkWORD"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="lkExcelLX"></asp:PostBackTrigger>
            <asp:PostBackTrigger ControlID="lkXuatExcelTS"></asp:PostBackTrigger>
        </Triggers>
        <Triggers>
            <asp:PostBackTrigger ControlID="lkWORD" />
        </Triggers>
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
    <br/>
</asp:Content>
