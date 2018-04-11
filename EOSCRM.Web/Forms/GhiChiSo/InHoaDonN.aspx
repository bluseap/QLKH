﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="InHoaDonN.aspx.cs" Inherits="EOSCRM.Web.Forms.GhiChiSo.InHoaDonN" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>


<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divSoThuTuHoaDonChuaIn").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divSoThuTuHoaDonChuaInDlg");
                }
            });

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
       
        function CheckFormReport() {
            var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

            if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                showError('Vui lòng chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                return false;
            }
            
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCaoThuy) %>', '');
        }

        function CheckChangeDP(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13) {
                return CheckFormSearch();
            }
        }

        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
        }
        
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divSoThuTuHoaDonChuaInDlg">
        <div id="divSoThuTuHoaDonChuaIn" style="display: none">
            <asp:UpdatePanel ID="UpdivSoThuTuHoaDonChuaIn" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">
                                                Số No đổi
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtSONODOIMOILX" runat="server" />
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Loại đồng hồ đổi
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:DropDownList ID="ddlLOAIDHDOILX" runat="server"></asp:DropDownList>
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Công suất đổi
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtCONGSUATDOILX" runat="server" />
                                                </div>
                                                
                                            </td>
                                        </tr>
                                       
                                    </tbody>
                                </table>
                            </td>
                        </tr>					
					</table>
				</ContentTemplate>
	        </asp:UpdatePanel>
        </div>        
    </div>
     <asp:UpdatePanel ID="upnlTinhCuoc" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="divupnlTinhCuoc" runat="server" class="crmcontainer">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="40px" MaxLength="4" TabIndex="2" />
                                </div>                                                       
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" runat="server"
                                            TabIndex="3" OnSelectedIndexChanged="ddlKHUVUC_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </div>
                                <td class="crmcell right">
                                    Đợt in hóa đơn
                                </td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:DropDownList ID="ddlDOTGCS" AutoPostBack="true"
                                             runat="server" OnSelectedIndexChanged="ddlDOTGCS_SelectedIndexChanged"></asp:DropDownList>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                <asp:Label ID="lbSoHoaDon" runat="server" Text="Số hóa đơn" Visible="false"></asp:Label>
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbSoHoaDonSum" runat="server" Font-Bold="True" Font-Size="Larger" Visible="false"></asp:Label>
                                </div>
                                <td class="crmcell right">
                                    <asp:Label ID="lbSoKhachHang" runat="server" Text="Số khách hàng" Visible="false"></asp:Label>
                                </td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Label ID="lbSoKhachHangSum" runat="server" Font-Bold="True" Font-Size="Larger" Visible="false"></asp:Label>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                <asp:Label ID="lbSoHoaDonDaIn" runat="server" Text="Số hóa đơn đã in" Visible="false"></asp:Label>
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbSoHoaDonDaInSum" runat="server" Font-Bold="True" Font-Size="Larger" Visible="false"></asp:Label>
                                </div>
                                <td class="crmcell right">
                                    <asp:Label ID="lbSoHoaDonChuaIn" runat="server" Text="Số hóa đơn chưa in" Visible="false"></asp:Label>
                                </td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Label ID="lbSoHoaDonChuaInSum" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Red" Visible="false"></asp:Label>
                                    </div>
                                    <div class="left">
                                        <asp:Button ID="btXemSoHoaDonChuaIn" runat="server" Text="Số thứ tự HĐ chưa in" CssClass="myButton" OnClick="btXemSoHoaDonChuaIn_Click"
                                            CausesValidation="false" UseSubmitBehavior="false" Visible="false"
                                            OnClientClick="openDialogAndBlock('Xem số thứ tự hóa đơn chưa in', 500, 'divSoThuTuHoaDonChuaIn')"
                                            />
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số HĐ đầu
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                     <asp:TextBox ID="txtHDDAU" runat="server" Width="60px" MaxLength="7" TabIndex="2" />
                                </div>
                                <div class="left">
                                     <asp:Label ID="Label1" runat="server" Text="+"></asp:Label>
                                </div>                                
                                <div class="left">
                                     <asp:TextBox ID="txtHDCONG" OnTextChanged="txtHDCONG_TextChanged"
                                        runat="server" Width="60px" MaxLength="7" TabIndex="2" />
                                </div>
                                <div class="left">
                                     <asp:Label ID="Label2" runat="server" Text="="></asp:Label>
                                </div>
                                <div class="left">
                                     <asp:TextBox ID="txtHDCUOI" runat="server" Width="60px" MaxLength="7" TabIndex="2" />
                                </div>
                                <div class="left">
                                    <strong>Số HĐ chưa in</strong>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblSOHDCL" runat="server" />
                                </div> 
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Ghi chú
                            </td>
                            <td>
                                <div class="left">
                                         <asp:TextBox ID="txtGCSHD" 
                                            runat="server" Width="300px" MaxLength="100" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnSearch"  
                                        runat="server" CssClass="filter" TabIndex="12" onclick="btnSearch_Click" />
                                </div>  
                                <div class="left">
                                    <asp:Button ID="btnBaoCaoThuy" 
                                        runat="server" TabIndex="13" CssClass="intheolo" 
                                        onclick="btnBaoCaoThuy_Click" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
     <asp:UpdatePanel ID="UpdateHDIL" UpdateMode="Conditional" runat="server"  >
        <ContentTemplate>
            <div id="divUpdateHDIL" runat="server" class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">
                                Số HĐ đầu
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lblSOHDDAUIL" runat="server" />
                                </div>
                                <div class="left">
                                    <strong>Số HĐ cuối</strong>
                                </div>
                                <div class="left">
                                     <asp:Label ID="lblSOHDCUOIIL" runat="server" />
                                </div> 
                                <div class="left">
                                     <asp:Label ID="lblMASOHD" runat="server" Visible="false"/>
                                </div>                                                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số HĐ bắt đầu
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                     <div class="left">
                                     <asp:TextBox ID="txtSOHDDAUIL" runat="server" Width="60px" MaxLength="7" TabIndex="2" />
                                    </div>
                                </div>
                                <div class="left">
                                     <strong>Số HĐ cuối</strong>
                                </div>                                
                                <div class="left">
                                     <asp:TextBox ID="txtSOHDCUOIIL" 
                                        runat="server" Width="60px" MaxLength="7" TabIndex="2" />
                                </div>                                
                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Button ID="btnLocIL"  
                                        runat="server" CssClass="filter" TabIndex="12" onclick="btnLocIL_Click"  />
                                </div>  
                                <div class="left">
                                    <asp:Button ID="btnINLAI" 
                                        runat="server" TabIndex="13" CssClass="intheolo" onclick="btnINLAI_Click" 
                                         />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" TabIndex="13"  />
                                </div>
                            </td>
                        </tr>
                        
                    </tbody>
                 </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlGridSHD" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="divupnlGridSHD" runat="server" class="crmcontainer">
                <eoscrm:Grid ID="gvSoHoaDon" runat="server" UseCustomPager="true" PageSize=20
                    OnPageIndexChanging="gvSoHoaDon_PageIndexChanging"
                    OnRowCommand="gvSoHoaDon_RowCommand"
                    AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
                   >
                   <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altrow" />
                    <HeaderStyle CssClass="header" />
                    <PagerSettings FirstPageText="số hoá đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="12%" HeaderText="Mã HĐ">
                            <ItemTemplate>
                            <asp:LinkButton ID="linkMa" runat="server" 
                                CommandArgument='<%# Eval("MASOHD") %>'
                                CommandName="SelectHD" 
                                Text='<%#  Eval("SOHDDAU")+"-"+Eval("SOHDCUOI") %>'>
                            </asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>   
                        <asp:BoundField HeaderText="Số HĐ đầu" DataField="SOHDDAU" HeaderStyle-Width="78%" />
                        <asp:BoundField HeaderText="Số HĐ cuối" DataField="SOHDCUOI" HeaderStyle-Width="78%" />
                        <asp:BoundField HeaderText="Ngày in" DataField="NGAY" HeaderStyle-Width="78%" />
                        <asp:BoundField HeaderText="Ghi chú" DataField="GHICHU" HeaderStyle-Width="78%" />
                    </Columns>
                </eoscrm:Grid> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGridSHDIL" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="divupnlGridSHDIL" runat="server" class="crmcontainer">
                <eoscrm:Grid ID="gvSoHoaDonIL" runat="server" UseCustomPager="true" PageSize=20
                    OnPageIndexChanging="gvSoHoaDonIL_PageIndexChanging"                    
                    AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
                   >
                   <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altrow" />
                    <HeaderStyle CssClass="header" />
                    <PagerSettings FirstPageText="số hoá đơn" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="12%" HeaderText="Mã HĐ">
                            <ItemTemplate>
                            <asp:LinkButton ID="linkMa" runat="server" 
                                CommandArgument='<%# Eval("MASOHDIL") %>'
                                CommandName="SelectHD" 
                                Text='<%#  Eval("SOHDDAUIL")+"-"+Eval("SOHDCUOIIL") %>'>
                            </asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>   
                        <asp:BoundField HeaderText="Số HĐ đầu" DataField="SOHDDAUIL" HeaderStyle-Width="78%" />
                        <asp:BoundField HeaderText="Số HĐ cuối" DataField="SOHDCUOIIL" HeaderStyle-Width="78%" />
                        <asp:BoundField HeaderText="Ngày in" DataField="NGAY" HeaderStyle-Width="78%" />
                        <asp:BoundField HeaderText="Ghi chú" DataField="GHICHU" HeaderStyle-Width="78%" />
                    </Columns>
                </eoscrm:Grid> 
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    
</asp:Content>
