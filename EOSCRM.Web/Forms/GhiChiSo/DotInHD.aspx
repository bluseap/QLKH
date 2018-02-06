<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="DotInHD.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.DotInHD" %>

<%--<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>--%>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
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

        function CheckFormFilterDP() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
            return false;
        }

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormSearch();
            }
        }

        function CheckFormSearch() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());
            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showErrorWithFocus('Phải chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
            return false;
        }
        
        function CheckFormSAVELIST() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btSAVELIST) %>', '');
            return false;
        }
        
        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onSelectedIndexChangedEventHandler(ddlDOTINId, hfGCSId, e) {
            var oldCsd = 0;
            var oldCsc = 0;
            var oldTT = 'GDH_BT';
            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length == 4) {
                oldCsc = idArr[4];
                oldTT = idArr[6];
            }

            var di = $("#" + ddlDOTINId + " option:selected").val();
            
            //var csd = $("#" + txtCHISODAUId).val();
            //var csc = $("#" + txtCHISOCUOIId).val();
            //var kltt = $("#" + txtKLTIEUTHUId).val();
            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msgTT = 'Vui lòng nhập chỉ số hợp lệ trước khi chuyển về trạng thái ghi bình thường.';

            //alertWithFocusSelect(idArr[0] + "," + di + "," + idArr[2] + "," + idArr[3]);

            // MADP     IDMADOTIN       MANV        TENNV
            var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateDotIn(idArr[0], di, idArr[2], idArr[3]);

            if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                //setControlValue(txtCHISOCUOIId, oldCsc);
                //setControlValue(txtKLTIEUTHUId, oldKltt);
                alertWithFocusSelect(idArr[0] + "," + idArr[1] + "," + idArr[2] + "," + idArr[3]);
                return;
            }

            // update hidden field
            var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                      idArr[1] + "<%= DELIMITER.Delimiter %>" +
                      idArr[2] + "<%= DELIMITER.Delimiter %>" +
                      idArr[3];

            // save to hidden field
            $("#" + hfGCSId).val(val);

        }

        function CheckFormReport() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btBAOCAOLG) %>', '');
        }
        //rvLICHGHICS
        //function CheckFormbtINBAOCAOLGCS() {           

        //    var viewerReference = $find("rvLICHGHICS");

        //    var stillonLoadState = clientViewer.get_isLoading();

        //    if (!stillonLoadState) {
        //        var reportArea = viewerReference.get_reportAreaContentType();
        //        if (reportArea == Microsoft.Reporting.WebFormsClient.ReportAreaContent.ReportPage) {
        //            $find("rvLICHGHICS").invokePrintDialog();
        //        }
        //    }            
        //}

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divDuongPhoDlgContainer">
        <div id="divDuongPho" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
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
							        <eoscrm:Grid ID="gvDuongPho" runat="server" OnRowCommand="gvDuongPho_RowCommand" UseCustomPager="true"
                                        OnPageIndexChanging="gvDuongPho_PageIndexChanging" OnRowDataBound="gvDuongPho_RowDataBound">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' 
                                                        CommandName="SelectMADP" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DUONGPHU" HeaderStyle-Width="15%" 
                                                HeaderText="Đường phụ" />
                                            <asp:BoundField DataField="TENDP" HeaderStyle-Width="50%" 
                                                HeaderText="Tên đường phố" />
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
            <div class="crmcontainer">
                <a name="top"></a>
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
                                    <asp:Label ID="reloadm" runat="server" Visible="false"></asp:Label>
                                </div>  
                            </td>
                        </tr> 
                        <tr>    
                            <td class="crmcell right">
                                Khu vực
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>
                                <td class="crmcell right">Đường phố</td>
                                <td class="crmcell" colspan="3"> 
                                    <div class="left">
                                        <asp:TextBox ID="txtMADP" runat="server" onkeydown="return CheckChangeDP(event);" MaxLength="4" Width="50px" TabIndex="4" />
                                        <%--onblur="CheckFormSearch();" --%>
                                    </div>
                                    <div class="left">
                                        <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="30px" TabIndex="5" Visible="false" />
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                            CausesValidation="false" UseSubmitBehavior="false" 
                                            OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 500, 'divDuongPho')" 
                                            TabIndex="6" />
                                    </div>
                                    <div class="left">
                                        <asp:Label ID="lblTENDUONG" runat="server" />
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">
                                Đợt ghi
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td>
                               <div class="left">
                                    <asp:Label ID="lbGHICHU" runat="server" Text="Lưu ý: Chuyển đợt ghi chỉ số cuối kỳ." Font-Bold="True" Font-Size="Large" ForeColor="#FF3300"></asp:Label>
                                </div>
                             </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div>
                                <td class="crmcell right">
                                    <div class="right">
                                        <asp:Button ID="btXEMBAOBAO" runat="server" Text="Xem báo cáo" Class="myButton" OnClick="btXEMBAOBAO_Click"/>
                                    </div>
                                </td>
                                <td class="crmcell" colspan="3"> 
                                    <div class="right">
                                        <asp:Button ID="btBAOCAOLG" runat="server" Text="Báo cáo" Class="myButton" OnClick="btBAOCAOLG_Click"/>
                                    </div>                                                   
                                </td>                                    
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
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="350"  
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="65px" HeaderText="Mã đường phố">
                            <ItemTemplate>
                                <%# Eval("MADP")%>                               
                                
                                <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("MADP") + DELIMITER.Delimiter + 
                                        Eval("IDMADOTIN") + DELIMITER.Delimiter +                                         
                                        LoginInfo.MANV + DELIMITER.Delimiter + 
                                        LoginInfo.NHANVIEN.HOTEN 
                                 %>' />                                 
                            </ItemTemplate>
                            <HeaderStyle Width="65px" />
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="200px" HeaderText="Tên đường phố" DataField="TENDP" >
                            <HeaderStyle Width="200px" />
                        </asp:BoundField>                        
                        <asp:TemplateField HeaderText="Đợt GCS" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDOTIN" runat="server"
                                    DataSource='<%# new ReportClass().BienKHNuoc("", new DotInHDDao().Get(Eval("IDMADOTIN").ToString()).MAKV, "", "", 1, 1, "DSDOTINKVNN")  %>' 
                                    DataTextField="TENDOTIN" DataValueField="IDMADOTIN" Width="100px" SelectedValue='<%# Bind("IDMADOTIN") %>' >
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        
                    </Columns>    
                </eoscrm:Grid>
            </div>
            <br />
            <div class="crmcontainer" id="divGVXEMBAOCAO" runat="server" visible="false">
                <eoscrm:Grid ID="gvXEMBAOCAO" runat="server" UseCustomPager="true" PageSize="350" OnRowCommand="gvXEMBAOCAO_RowCommand" 
                    OnRowDataBound="gvXEMBAOCAO_RowDataBound" OnPageIndexChanging="gvXEMBAOCAO_PageIndexChanging">
                    <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã Đ.Phố " HeaderStyle-Width="50px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lkbtMADPXBC" runat="server" CommandArgument='<%# Eval("IDMADP") %>'
                                CommandName="EditItem" CssClass="link" Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>  
                        <asp:BoundField HeaderStyle-Width="150px" HeaderText="Tên đường phố" DataField="TENDP" >
                            <HeaderStyle Width="150px" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Đợt GCS cũ" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDOTINCUXBC" runat="server" CssClass="readonly" ReadOnly="true"
                                    DataSource='<%# new ReportClass().BienKHNuoc("", new DotInHDDao().Get(Eval("IDMADOTINCU").ToString()).MAKV, "", "", 1, 1, "DSDOTINKVNN")  %>' 
                                    DataTextField="TENDOTIN" DataValueField="IDMADOTIN" Width="100px" SelectedValue='<%# Bind("IDMADOTINCU") %>' >
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>                    
                        <asp:TemplateField HeaderText="Đợt GCS mới" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlDOTINXBC" runat="server" CssClass="readonly" ReadOnly="true"
                                    DataSource='<%# new ReportClass().BienKHNuoc("", new DotInHDDao().Get(Eval("IDMADOTIN").ToString()).MAKV, "", "", 1, 1, "DSDOTINKVNN")  %>' 
                                    DataTextField="TENDOTIN" DataValueField="IDMADOTIN" Width="100px" SelectedValue='<%# Bind("IDMADOTIN") %>' >
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Xóa " HeaderStyle-Width="30px">
                            <ItemTemplate>
                            <asp:LinkButton ID="lkbtXOAXBC" runat="server" CommandArgument='<%# Eval("IDMADP") %>'
                                CommandName="XoaEditItem" CssClass="link" Text='Xóa LGCS'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        
                    </Columns>    
                </eoscrm:Grid>
            </div>     
            <br />
            <div class="crmcontainer" id="divupGV" runat="server" >
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"></a></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btSAVELIST" OnClick="btSAVELIST_Click" Visible="false"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSAVELIST();" 
                                        runat="server" CssClass="save" Text="" TabIndex="12" />
                                </div>
                            </td>
                           
                            <td class="crmcell right"><a href="#top">Về đầu trang</a></td>
                            <td class="crmcell">   
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />  
    <asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>           
            <div class="crmcontainer" id="divrvLICHGHICS" runat="server" >
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"></a></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btINBAOCAOLGCS" Visible="false"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormbtINBAOCAOLGCS();" 
                                        runat="server" CssClass="myButton" Text="In báo cáo" TabIndex="12" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" DisplayGroupTree="False" />                
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>
    <%--<asp:UpdatePanel ID="upnlReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>           
            <div class="crmcontainer" id="divrvLICHGHICS" runat="server" >
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"></a></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btINBAOCAOLGCS" Visible="false"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormbtINBAOCAOLGCS();" 
                                        runat="server" CssClass="myButton" Text="In báo cáo" TabIndex="12" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <rsweb:ReportViewer ID="rvLICHGHICS" runat="server" Width="950px"></rsweb:ReportViewer>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rvLICHGHICS" />
        </Triggers>
    </asp:UpdatePanel>--%>
    
</asp:Content>
