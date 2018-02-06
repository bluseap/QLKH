<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="True"
    CodeBehind="InHoaDonLe.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.InHoaDonLe" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormSearch();
            }
        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

        function CheckFormSearch() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
        }

        function CheckFormInLe() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }

            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnInLeTheoDanhSach) %>', '');
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
    <asp:UpdatePanel ID="upnlTinhCuoc" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">
                                Kỳ khai thác
                            </td>
                            <td class="crmcell">
                                <div class="left width-100">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                </div>
                                <div class="left" style="display: none">
                                    <div class="right">Mã đường</div>
                                </div>
                                <div class="left width-150" style="display: none">
                                    <asp:TextBox ID="txtMADP" onkeydown="return CheckChangeDP(event);" runat="server" Width="25px" />&nbsp;
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" Width="25px" />&nbsp;                            
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                        TabIndex="6" />
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                                <div class="left" style="display: none">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left" style="display: none">
                                    <asp:DropDownList ID="ddlKHUVUC" Width="150px" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none">    
                            <td class="crmcell right">Mã khách hàng</td>
                            <td class="crmcell"> 
                                <div class="left width-100">
                                    <asp:TextBox ID="txtMAKH" runat="server" Width="84px" MaxLength="8" TabIndex="2" />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Tên khách hàng</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" CssClass="width-200"  TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none">    
                            <td class="crmcell right">Lộ trình đầu</td>
                            <td class="crmcell"> 
                                <div class="left width-100">
                                    <asp:TextBox ID="txtLoTrinhDau" runat="server" Width="84px" TabIndex="2" Text = "0"  />
                                </div>
                                <div class="left width-150">
                                    <div class="right">Lộ trình cuối</div>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtLoTrinhCuoi" runat="server" Width="84px"  TabIndex="2" Text = "999999" />
                                </div>
                            </td>
                        </tr>
                        <tr style="display: none">    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" OnClientClick="return CheckFormSearch();" 
                                        runat="server" CssClass="filter" TabIndex="12" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" OnClick="btnInLoTrinh_Click" ToolTip="Chọn đường, lộ trình đầu cuối và in" OnClientClick="return CheckFormReport();"
                                        runat="server" CssClass="intheolotrinh" TabIndex="13" />
                                </div>
                                
                                <div class="left">
                                    <asp:Button ID="btnBaoCaoThuy" OnClick="btnBaoCao_Click" ToolTip="Chọn đường, lọc khách hàng, check chọn khách hàng và in" CssClass="inle"  runat="server" 
                                        TabIndex="13" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right btop vtop">In theo danh sách</td>
                            <td class="crmcell btop"> 
                                <div class="left">
                                    <asp:TextBox ID="txtDanhSach" runat="server" TextMode="MultiLine" Width="650px" />
                                </div>
                                <div class="left">
                                    <font color="red"><strong>Danh bộ cách nhau <br />bởi dấu phẩy (,)</strong></font>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnInLeTheoDanhSach" OnClick="btnInLeTheoDanhSach_Click" CssClass="inle"  runat="server" 
                                        TabIndex="13" OnClientClick="return CheckFormInLe();" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />  
            <div class="crmcontainer" id="divList" runat="server" visible="false">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="100" 
                    OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="3%">
                            <ItemTemplate>
                                <input type="hidden" id="SODB" value='<%# Eval("MADP") + Eval("DUONGPHU").ToString() + Eval("MADB") %>' runat="server" />
                                <input name="listIds" type="checkbox" value='<%# Eval("IDKH") %>' onclick="DoCheckItem();" />
                            </ItemTemplate>
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Check All / UnCheck All" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <FooterTemplate>
                                <input id="chkAllBottom" title="Check All / UnCheck All" name="chkAllBottom" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="3%" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="7%" HeaderText="Danh bộ">
                            <ItemTemplate>
                                <%# Eval("MADP") + Eval("DUONGPHU").ToString() + Eval("MADB") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                         <asp:TemplateField HeaderStyle-Width="35%" HeaderText="Tên khách hàng">
                            <ItemTemplate>
                                <%# Eval("KHACHHANG.TENKH")%>
                            </ItemTemplate>
                        </asp:TemplateField>     
                        <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%--<%# 
                                    (Eval("KHACHHANG") != null) ? 
                                        Eval("KHACHHANG.SONHA").ToString() != "" ?
                                            Eval("KHACHHANG.SONHA") + ", " + Eval("KHACHHANG.DUONGPHO.TENDP")
                                            : Eval("KHACHHANG.DUONGPHO.TENDP") 
                                        : ""
                                %>--%>
                            </ItemTemplate>
                            <HeaderStyle Width="20%" />
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Chỉ số đầu" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("CHISODAU")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Chỉ số cuối" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("CHISOCUOI")) %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Số tiền" 
                            ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                            <ItemTemplate>
                                <%# String.Format("{0:0,0}", Eval("TONGTIEN_PS")) %>
                            </ItemTemplate>
                        </asp:TemplateField>                  
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>    
</asp:Content>
