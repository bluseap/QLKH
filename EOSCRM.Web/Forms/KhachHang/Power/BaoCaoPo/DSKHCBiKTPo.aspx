<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="DSKHCBiKTPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo.DSKHCBiKTPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divPhuong").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divPhuongDlgContainer");
                }
            });
        });

        function CheckFormFilterPhuong() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterPhuong) %>', '');
            return false;
        }

    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divPhuongDlgContainer">
        <div id="divPhuong" style="display: none">
            <asp:UpdatePanel ID="upnlPhuong" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table style="width: 500px;">
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
                                                    <asp:TextBox ID="txtKeywordDP" onchange="return CheckFormFilterPhuong();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterPhuong" OnClick="btnFilterPhuong_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterPhuong();" 
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
							        <eoscrm:Grid ID="gvPhuong" runat="server" OnRowCommand="gvPhuong_RowCommand" UseCustomPager="true">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MAPHUONGPO") %>' 
                                                        CommandName="SelectMAPHUONG" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MAPHUONGPO").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                           
                                            <asp:BoundField DataField="TENPHUONG" HeaderStyle-Width="50%" 
                                                HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Khu vực">
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

    <asp:UpdatePanel ID="upKHCHUANBIKT" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">
                                Kỳ khai thác
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="35px" MaxLength="4" TabIndex="2" />
                                    <asp:Label ID="lbReLoad" runat="server" Visible="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>                           
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC1" runat="server" >
                                    </asp:DropDownList>
                                </div>                                                             
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Nhà máy, tổ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlNHAMAYTO" runat="server" />                                    
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlMAPHUONG" runat="server" Visible="False"  />
                                    <asp:TextBox ID="txtMAPHUONG" runat="server" Width="50px" MaxLength="20" TabIndex="3" Font-Names="Times New Roman" Visible="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseSOHD" runat="server" CssClass="pickup" OnClick="btnBrowseSOHD_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách phường', 500, 'divPhuong')" 
                                        TabIndex="44" Visible="false"/>
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:Button ID="btDSTANGGIAMTO" runat="server" CssClass="myButton" Text="DS Tăng giảm (TỔ)"
                                            TabIndex="15" UseSubmitBehavior="false" OnClick="btDSTANGGIAMTO_Click1"/>   
                                    </div> 
                                </td>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Th.Phố, Huyện
                            </td>
                            <td class="crmcell">
                                 <div class="left">
                                    <asp:DropDownList ID="ddlQuanHuyen" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlQuanHuyen_SelectedIndexChanged" ></asp:DropDownList>                                 
                                </div>
                                <td class="crmcell right">
                                    <div class="left">
                                        <asp:Label ID="lbPhuongXa" runat="server" Text="Phường, Xã"></asp:Label>
                                        <asp:DropDownList ID="ddlPHUONGXA" runat="server" />                 
                                    </div>
                                </td>                                
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:Button ID="btDSTANGGIAM" runat="server" CssClass="myButton" OnClick="btDSTANGGIAM_Click" Text="DS Tăng giảm SH, MĐK (XÃ)"
                                            TabIndex="15" UseSubmitBehavior="false"/>   
                                    </div> 
                                </td>                                 
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left width-200">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>                                
                            </td>                                  
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnDSKHCBiKT" runat="server" CssClass="myButton" OnClick="btnDSKHCBiKT_Click"
                                        TabIndex="15" UseSubmitBehavior="false" Text="DS Sinh hoạt"/>                                    
                                </div>                                
                                <td class="crmcell right">
                                    <div class="left">
                                        <asp:Button ID="btnDSKHCBiKTMDK" runat="server" CssClass="myButton" OnClick="btnDSKHCBiKTMDK_Click"
                                            TabIndex="15" UseSubmitBehavior="false" Text="DS MĐ.Khác"/>                                    
                                    </div>
                                </td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:Button ID="btDSTGMDK" runat="server" CssClass="myButton" Text="DS TG MĐ.Khác"
                                            TabIndex="15" UseSubmitBehavior="false" OnClick="btDSTGMDK_Click"/>   
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
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" >
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                    DisplayGroupTree="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>  

    
</asp:Content>
