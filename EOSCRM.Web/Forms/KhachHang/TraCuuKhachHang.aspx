﻿<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True" EnableEventValidation="true"
    Inherits="EOSCRM.Web.Forms.KhachHang.TraCuuKhachHang" CodeBehind="TraCuuKhachHang.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register src="../../UserControls/KhachHangFilterPanel.ascx" tagname="KhachHangFilterPanel" tagprefix="bwaco" %>

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

            $("#divDuongPhoKHM").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDuongPhoKHMDlgContainer");
                }
            });

            $("#divDongHo").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divDongHoDlgContainer");
                }
            });

            $("#divUpSoNoLX").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divUpSoNoLXDlgContainer");
                }
            });
            
            $("#divUpSoNoKHM").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divUpSoNoKHMDlgContainer");
                }
            });

            $("#divCQTT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divCQTTDlgContainer");
                }
            });
            
            $("#divTDCT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTDCTlgContainer");
                }
            });
            
            $("#divTDH").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTDHlgContainer");
                }
            });
            
            $("#divDCHD").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDCHDlgContainer");
                }
            });

            $("#divDSVATTU").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divDSVATTUlgContainer");
                }
            });

            $("#divHopDong").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divHopDongDlgContainer");
                }
            });

            $("#divTTTT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divTTTTDlgContainer");
                }
            });
            
            $("#divTTVPKH").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divTTVPKHlgContainer");
                }
            });
            /*
            * check exactly length of madp = 3 or 0
            */
            $(function() {
                $("#<%= txtMADP.ClientID %>").focusout(function() {
                    var value = $("#<%= txtMADP.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 4) {
                        jAlert('Độ dài mã đường phố phải là 4', 'Cảnh báo');
                        $("#<%= txtMADP.ClientID %>").focus();
                    }
                }).blur(function() {
                    var value = $("#<%= txtMADP.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();
                    
                    if (len != 0 && len != 4) {
                        jAlert('Độ dài mã đường phố phải là 4', 'Cảnh báo');
                        $("#<%= txtMADP.ClientID %>").focus();
                    }
                });
            });

            /*
            * check exactly length of madb = 4 or 0
            */
            $(function() {
                $("#<%= txtMADB.ClientID %>").focusout(function() {
                    var value = $("#<%= txtMADB.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 8) {
                        jAlert('Độ dài mã danh bộ phải là 8', 'Cảnh báo');
                        $("#<%= txtMADB.ClientID %>").focus();
                    }
                }).blur(function() {
                    var value = $("#<%= txtMADB.ClientID %>").val();
                    value = $.trim(value);
                    var len = value.length();

                    if (len != 0 && len != 8) {
                        jAlert('Độ dài mã danh bộ phải là 8', 'Cảnh báo');
                        $("#<%= txtMADB.ClientID %>").focus();
                    }
                });
            });

            $(function() {
                $('#<%= cbSDInfo_INHOADON.ClientID %>').click(function() {
                    var other = $('#<%= cbSDInfo_INHOADON.ClientID %>:checked').val();
                    if (other != undefined) {
                        $('#<%=txtTENKH_INHOADON.ClientID %>').removeAttr('readonly');
                        $('#<%=txtDIACHI_INHOADON.ClientID %>').removeAttr('readonly');
                    }
                    else {
                        $('#<%=txtTENKH_INHOADON.ClientID %>').attr('readonly', 'readonly');
                        $('#<%=txtDIACHI_INHOADON.ClientID %>').attr('readonly', 'readonly');
                    }
                });
            });
        });

        function CheckFormbtLuuGhiChuTT() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btLuuGhiChuTT) %>', '');
            return false;
        }
       
        function CheckFormSave() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    //__doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
		    return false;
		}

		function OpenTTTT() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnTieuThu) %>', '');
		    return false;
		}
		
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

        function checkDanhBoPhoForm() {
            // check text boxes have at least one value
            var dp = jQuery.trim(document.getElementById('<%= txtMADB.ClientID %>').value);

            if (dp != '') {
                //openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lbtMADB) %>', '');
            }
            return false;
        }

		function CheckFormFilterDP() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDP) %>', '');
		    return false;
		}

        function CheckFormFilterDPKHM() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDPKHM) %>', '');
		    return false;
        }
        
		function CheckFormReport() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnInBaoCao) %>', '');
		    return false;
		}
		
		function CheckFormFilterDH() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDH) %>', '');
		    return false;
		}

		function CheckFormFilterCQTT() {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterCQTT) %>', '');
		    return false;
		}		
   
        function CheckFormbtSaveUpSoNo() {           
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btSaveUpSoNo) %>', '');
		    return false;
        }
        
        function CheckFormFilterDHSONOKHM() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterDHSONOKHM) %>', '');
            return false;
        }

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    
    <div id="divTTVPKHlgContainer">
        <div id="divTTVPKH" style="display: none">
            <asp:UpdatePanel ID="upTTVPKH" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTTVPKH" runat="server" UseCustomPager="true" CssClass="crmgrid"  AutoGenerateColumns="false"	 							            
							            OnPageIndexChanging="gvTTVPKH_PageIndexChanging" >
                                        <RowStyle CssClass="row" />
                                        <PagerSettings FirstPageText="danh sách truy thu vi phạm" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Kỳ T.Thu" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("THANG").ToString() + "/" + Eval("NAM").ToString() %>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:TemplateField HeaderText="D.Số" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("DANHSO") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Tên KH" HeaderStyle-Width="150px">
                                                <ItemTemplate>
                                                    <%# Eval("TENKH") %>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Tr.Thu" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <%# Eval("MTRUYTHUVP") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Đ.mức" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <%# Eval("DMNK") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="T.Tiền" HeaderStyle-Width="100px">
                                                <ItemTemplate>
                                                    <%# Eval("TONGTIENVP") %>
                                                </ItemTemplate>
                                            </asp:TemplateField> 
                                            <asp:TemplateField HeaderText="Ghi chú" HeaderStyle-Width="200px">
                                                <ItemTemplate>
                                                    <%# Eval("GHICHUVP") %>
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
    <div id="divDSVATTUlgContainer">
        <div id="divDSVATTU" style="display: none">
            <asp:UpdatePanel ID="upnlDSVATTU" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvDSVATTU" runat="server" UseCustomPager="true" CssClass="crmgrid"  AutoGenerateColumns="false"	 							            
							            OnPageIndexChanging="gvDSVATTU_PageIndexChanging" >
                                        <RowStyle CssClass="row" />
                                        <PagerSettings FirstPageText="danh sách v.tư" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                                <ItemTemplate>
                                                    <%# Container.DataItemIndex + 1%>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("MAVT") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="60px">
                                                <ItemTemplate>
                                                    <%# Eval("VATTU.MAHIEU") %> 
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Nội dung công việc">
                                                <ItemTemplate>
                                                    <%# Eval("VATTU.TENVT") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Công ty đầu tư" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <%# (Eval("ISCTYDTU").Equals(true) ? "Có" : "Không") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ĐVT" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <%# Eval("VATTU.DVT") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="30px">
                                                <ItemTemplate>
                                                    <%# Eval("SOLUONG") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>     
                                            <asp:TemplateField HeaderText="Tiền" HeaderStyle-Width="50px">
                                                <ItemTemplate>
                                                    <%# Eval("TIENVT") %>
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
    <div id="divDCHDlgContainer">
        <div id="divDCHD" style="display: none">
            <asp:UpdatePanel ID="upnlDCHD" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvDCHD" runat="server" UseCustomPager="true" CssClass="crmgrid"  AutoGenerateColumns="false"	 						            
							            OnPageIndexChanging="gvDCHD_PageIndexChanging">
                                        <RowStyle CssClass="row" />
                                        <PagerSettings FirstPageText="điều chỉnh HĐ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-Width="13%" DataField="SODB" HeaderText="Danh số" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="NAM" HeaderText="Năm" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="THANG" HeaderText="Tháng" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="CHISOCUOI" HeaderText="CS Cuối" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="CHISOCUOIDC" HeaderText="CS cuối ĐC" />
                                            <asp:BoundField HeaderStyle-Width="7%" DataField="KLTIEUTHUDC" HeaderText="KL" />
                                            <asp:BoundField HeaderStyle-Width="11%" DataField="TONGTIENDC" HeaderText="T.Tiền" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="MASOHDDC" HeaderText="Mã HĐ ĐC" />
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Ngày ĐC">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYDCCS") != null) ?
                                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYDCCS"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" DataField="GHICHUDC" HeaderText="Ghi chú" />                         
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
    <div id="divTDHlgContainer">
        <div id="divTDH" style="display: none">
            <asp:UpdatePanel ID="upnlTDH" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTDH" runat="server" UseCustomPager="true" 							            
							            OnPageIndexChanging="gvTDH_PageIndexChanging">
                                        <PagerSettings FirstPageText="thay ĐH" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Ngày thay cũ">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYTHAYCU") != null) ?
                                                            String.Format("{0:dd/MM/yyyy}", Eval("NGAYTHAYCU"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="LOAIDHCU" HeaderText="Loại ĐH cũ" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="SONOCU" HeaderText="Số No cũ" />  
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Ngày thay mới">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYHT") != null) ?
                                                            String.Format("{0:MM/yyyy}", Eval("NGAYHT"))
                                                            : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>  
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="SONO" HeaderText="Số No mới" />  
                                            <asp:BoundField HeaderStyle-Width="50%" DataField="GHICHU" HeaderText="Ghi chú" />
                                            <asp:BoundField HeaderStyle-Width="20%" DataField="KYTHAYDH" HeaderText="Kỳ thay" />                                           
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
    <div id="divTDCTlgContainer">
        <div id="divTDCT" style="display: none">
            <asp:UpdatePanel ID="upnlTDCT" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 600px;">                        
						<tr>
							<td class="ptop-10">
							    <div class="crmcontainer">
							        <eoscrm:Grid ID="gvTDCT" runat="server" UseCustomPager="true" 							            
							            OnPageIndexChanging="gvTDCT_PageIndexChanging">
                                        <PagerSettings FirstPageText="thay đổi" PageButtonCount="2" />
                                        <Columns>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="SODB" HeaderText="Danh số" />
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="THAYDOI" HeaderText="Lý do đổi" />                                             
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Kỳ ">
                                                <ItemTemplate>
                                                    <%# (Eval("NGAYDOI") != null) ?
                                                            String.Format("{0:MM/yyyy}", Eval("NGAYDOI"))
                                                            : "" %>
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
    <div id="divDuongPhoDlgContainer">
        <div id="divDuongPho" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPho" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDP" runat="server" Width="250px" MaxLength="200" />
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
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' 
                                                        CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="15%" DataField="DUONGPHU" HeaderText="Đường phụ" />
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Khu vực">
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
    <div id="divDuongPhoKHMDlgContainer">
        <div id="divDuongPhoKHM" style="display: none">
            <asp:UpdatePanel ID="upnlDuongPhoKHM" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDPKHM" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDPKHM" OnClick="btnFilterDPKHM_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDPKHM();" 
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
							        <eoscrm:Grid ID="gvDuongPhoKHM" runat="server" UseCustomPager="true" 
							            OnRowDataBound="gvDuongPhoKHM_RowDataBound" OnRowCommand="gvDuongPhoKHM_RowCommand" 
							            OnPageIndexChanging="gvDuongPhoKHM_PageIndexChanging">
                                        <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐP">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnIDKHM" runat="server" 
                                                        CommandArgument='<%# Eval("MADP") + "-" + Eval("DUONGPHU") %>' 
                                                        CommandName="SelectMADP"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADP").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:BoundField HeaderStyle-Width="45%" DataField="TENDP" HeaderText="Tên đường phố" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Khu vực">
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
    <div id="divDongHoDlgContainer">
        <div id="divDongHo" style="display: none">
            <asp:UpdatePanel ID="upnlDongHo" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDH" onchange="return CheckFormFilterDH();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDH" OnClick="btnFilterDH_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDH();" 
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
							        <eoscrm:Grid ID="gvDongHo" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvDongHo_PageIndexChanging" OnRowCommand="gvDongHo_RowCommand">
                                        <PagerSettings FirstPageText="loại" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã LĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MALDH") %>' 
                                                        CommandName="SelectMALDH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MALDH").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" HeaderText="Nơi sản xuất" DataField="NSX" />
                                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Kích cỡ" DataField="KICHCO" />
                                            <asp:BoundField HeaderStyle-Width="20%" HeaderText="Kiểu" DataField="KDH" />
                                            <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Giá" 
                                                ItemStyle-HorizontalAlign="Right" HeaderStyle-HorizontalAlign="Right">
                                                <ItemTemplate>
                                                    <%# String.Format("{0:0,0}", Eval("GIA"))%>
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
    <div id="divUpSoNoLXDlgContainer">
        <div id="divUpSoNoLX" style="display: none">
            <asp:UpdatePanel ID="UpdivUpSoNoLX" runat="server" UpdateMode="Conditional">
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
                                        <tr>
                                            <td class="crmcell right">                                                
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                        <asp:Button ID="btSaveUpSoNo" OnClick="btSaveUpSoNo_Click"
                                                            UseSubmitBehavior="false" OnClientClick="return CheckFormbtSaveUpSoNo();" 
                                                            runat="server" CssClass="myButton" Text="Lưu Số No" />
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
    <div id="divUpSoNoKHMDlgContainer">
        <div id="divUpSoNoKHM" style="display: none">
            <asp:UpdatePanel ID="UpdivUpSoNoKHM" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordDHSONOKHM" onchange="return CheckFormFilterDHSONOKHM();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterDHSONOKHM" OnClick="btnFilterDHSONOKHM_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterDHSONOKHM();" 
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
							        <eoscrm:Grid ID="gvDongHoSoNoKHM" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvDongHoSoNoKHM_PageIndexChanging" OnRowCommand="gvDongHoSoNoKHM_RowCommand">
                                        <PagerSettings FirstPageText="Đồng hồ" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã ĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MADH") %>' 
                                                        CommandName="SelectMADH" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MADH").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Loại ĐH" DataField="MALDH" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Năm SX" DataField="NAMSX" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số No" DataField="SONO" />
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số ĐK" DataField="SOKD" /> 
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Số tem" DataField="TEMKD" />                                            
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
    <div id="divTTTTDlgContainer">
        <div id="divTTTT" style="display: none">
            <asp:UpdatePanel ID="upnlTTTT" runat="server" UpdateMode="Conditional">
				<ContentTemplate>				    
				    <table cellpadding="3" cellspacing="1" style="width: 800px;">
                        <tr>
							<td class="crmcontainer">
							    <asp:Button ID = "btnInBaoCao" runat ="server" CssClass="report" OnClientClick="checkFormReport();" onclick="btnInBaoCao_Click" />
							    <br />
							    <asp:Label ID="Label1" runat="server" Text="Tên khách hàng: "></asp:Label>
                                <asp:Label ID="lblTenKH" runat="server" Text="" Font-Bold=true></asp:Label>
							    <br /> 
							    <eoscrm:Grid ID="gvTTTT" runat="server" UseCustomPager="true" PageSize="15"
                                   OnRowCommand="gvTTTT_RowCommand" OnPageIndexChanging="gvTTTT_PageIndexChanging" > 
                                    <PagerSettings FirstPageText="kỳ" PageButtonCount="2" />
                                    <Columns>
                                        <asp:TemplateField HeaderStyle-Width="1%" HeaderText="#">
                                            <ItemTemplate>
                                                <%# Container.DataItemIndex + 1%>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="Năm" DataField="NAM" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="Tháng" DataField="THANG" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="CSD" DataField="CHISODAU" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="CSC" DataField="CHISOCUOI" />
                                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="Truy T" DataField="MTRUYTHU" />
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="KLTT" DataField="KLTIEUTHU" />
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Tiền nước">
                                            <ItemTemplate>
                                                <%# String.Format("{0:0,0}", Eval("TONGTIEN")) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                                                                
                                        <asp:TemplateField HeaderStyle-Width="19%" HeaderText="MDSD">
                                            <ItemTemplate>
                                                <%# Eval("MDSD.TENMDSD") %>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField HeaderStyle-Width="8%" HeaderText="T.Thái" DataField="TTHAIGHI" />
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Ngày ghi">
                                            <ItemTemplate>
                                                <%#  string.Format("{0:dd/MM/yyyy}", Eval("NGAYGHICS")) %>
                                            </ItemTemplate>
                                        </asp:TemplateField>                                        
                                        <asp:BoundField HeaderStyle-Width="6%" HeaderText="Mã NV" DataField="MANVN_CS" />

                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="GC KD">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lkGHICHUKDLX" runat="server" 
                                                    CommandArgument='<%# Eval("IDKH").ToString()+"-"+Eval("NAM").ToString()+"-"+Eval("THANG").ToString() %>'
                                                    CommandName="SelectTieuThu" 
                                                    Text='<%# HttpUtility.HtmlEncode(Eval("GHICHUKDLX") != null ? Eval("GHICHUKDLX").ToString() : "Ko") %>'>
                                                </asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="GC CS">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lkGHICHUCSLX" runat="server" 
                                                    CommandArgument='<%# Eval("IDKH").ToString()+"-"+Eval("NAM").ToString()+"-"+Eval("THANG").ToString() %>'
                                                    CommandName="SelectTieuThu" 
                                                    Text='<%# HttpUtility.HtmlEncode(Eval("GHICHUCSLX") != null ? Eval("GHICHUCSLX").ToString() : "Ko") %>'>
                                                </asp:LinkButton>                                                
                                            </ItemTemplate>
                                        </asp:TemplateField> 
                                    </Columns>
                                </eoscrm:Grid>                                 
							</td>
						</tr>
					</table>
				</ContentTemplate>
	        </asp:UpdatePanel>
        </div>
    </div>
    <div id="divCQTTDlgContainer">
        <div id="divCQTT" style="display: none">
            <asp:UpdatePanel ID="upnlCQTT" runat="server" UpdateMode="Conditional">
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
                                                    <asp:TextBox ID="txtKeywordCQTT" onchange="return CheckFormFilterCQTT();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterCQTT" OnClick="btnFilterCQTT_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterCQTT();" 
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
							        <eoscrm:Grid ID="gvCQTT" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvCQTT_PageIndexChanging" OnRowCommand="gvCQTT_RowCommand">
                                        <PagerSettings FirstPageText="cơ quan" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="15%" HeaderText="Mã LĐH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="linkMa" runat="server" 
                                                        CommandArgument='<%# Eval("MACQ") %>' 
                                                        CommandName="SelectMACQ" CssClass="link" 
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MACQ").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="30%" HeaderText="Tên CQ" DataField="TENCQ" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Ngân hàng">
                                                <ItemTemplate>
                                                    <%# Eval("NGANHANG.TENNH") %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tài khoản" DataField="SOTK" />
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
	
    <bwaco:KhachHangFilterPanel ID="filterPanel" runat="server" />    
    
    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div id="divCustomersContainer" runat="server" class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">                                
                            </td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:Label ID="lbKYXOABO" runat="server" Font-Bold="True" ForeColor="Red"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Kỳ thay đổi chi tiết
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANGTDCT" runat="server" TabIndex="1">
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
                                    <asp:TextBox ID="txtNAMTDCT" runat="server" Width="50px" MaxLength="4" TabIndex="2" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khai thác
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANG" runat="server" TabIndex="1" Visible="false">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" Visible="false"/>
                                    <asp:Label ID="lbKYKT" runat="server" Font-Bold="True" ></asp:Label>
                                </div>
                            </td>
                            <td class="crmcell right">
                                Loại đồng hồ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbMALOAIDHM" runat="server" Font-Bold="True" Font-Size="Large" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtMALDH" runat="server" Width="100px" MaxLength="15" 
                                        TabIndex="20" Enabled="False" Visible="False" />                                    
                                </div>
                                <%--<div class="left">/</div>
                                <div class="left">
                                    <asp:Label ID="lblMALDH" runat="server" />
                                </div>--%>
                                <div class="left">
                                    <asp:Button ID="btnBrowseLOAIDH" runat="server" CssClass="pickup" OnClick="btnBrowseLOAIDH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách loại đồng hồ', 500, 'divDongHo')" 
                                        TabIndex="20" Visible="False" />
                                </div>                                                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số hợp đồng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHD" runat="server" Width="100px" MaxLength="20" TabIndex="3" ReadOnly="True" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Kích cỡ đồng hồ
                            </td>
                            <td class="crmcell">   
                                <div class="left">
                                    <asp:Label ID="lblKICHCO" runat="server" Font-Bold="True" Font-Size="Large" />
                                </div>                              
                                <div class="left">
                                    <asp:DropDownList ID="ddlKICHCODH" Width="80px" runat="server" TabIndex="21" Visible="false"
                                        Enabled="False" >
                                        <asp:ListItem Text="15" Value="15" />
                                        <asp:ListItem Text="20" Value="20" />
                                        <asp:ListItem Text="34" Value="34" />
                                        <asp:ListItem Text="42" Value="42" />
                                        <asp:ListItem Text="49" Value="49" />
                                        <asp:ListItem Text="60" Value="60" />
                                        <asp:ListItem Text="100" Value="100" />
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số điện thoại
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSDT" runat="server" MaxLength="15" Width="100px" TabIndex="4" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Serial đồng hồ
                            </td>
                            <td class="crmcell">  
                                <div class="left">
                                    <asp:Label ID="lbSONO" runat="server" Font-Bold="True" Font-Size="Large" />
                                </div>
                                <div class="left" >
                                    <asp:Button ID="btDOISONOLX" runat="server" CssClass="myButton" Text="Đổi No" OnClick="btDOISONOLX_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" Visible="true"
                                        OnClientClick="openDialogAndBlock('Đổi số No đồng hồ', 500, 'divUpSoNoLX')"  />                                    
                                </div>                   
                                <div class="left" >
                                    <asp:TextBox ID="txtMADH" runat="server" MaxLength="20" Width="100px" 
                                        TabIndex="22" Enabled="False" Visible="False"  />
                                </div>
                                
                                
                                <%--
                                <div class="left">
                                    <asp:CheckBox ID="cbLADHTONG" Enabled="false" runat="server" TabIndex="23" />
                                </div>
                                <div class="left">
                                    <strong>Là đồng hồ tổng</strong>
                                </div>--%>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số CMND
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:TextBox ID="txtSOCMND" runat="server" MaxLength="20" Width="100px" ReadOnly="false"/>
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Mã đơn:
                            </td>
                            <td class="crmcell">
                             <div class="left">
                                    <asp:Label ID="lbMADDK" runat="server" Font-Bold="True" Font-Size="Small" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Tên khách hàng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH" runat="server" MaxLength="50" Width="180px" 
                                        TabIndex="5" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckTENKH" runat="server" TabIndex="28" oncheckedchanged="ckTENKH_CheckedChanged" 
                                         AutoPostBack="True" Visible="true" />
                                </div>
                            </td>
                            <td class="crmcell right">
                               
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlLOAIKH" runat="server" TabIndex="23" Visible="false">
                                        <asp:ListItem Text="Tư nhân" Value="TN" />
                                        <asp:ListItem Text="Nhà nước" Value="NN" />
                                    </asp:DropDownList>                                
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlGHI2THANG1LAN" Width="70px" runat="server" TabIndex="23" Visible="False">
                                        <asp:ListItem Text="Không" Value="0" />
                                        <asp:ListItem Text="Có" Value="1" />
                                    </asp:DropDownList>                                
                                </div>
                                <div class="left">
                                    <strong></strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số nhà, địa chỉ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA2" runat="server" Width="60px" 
                                        TabIndex="6" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHILD" runat="server" MaxLength="150" Width="120px" 
                                        TabIndex="6" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckDIACHILD" runat="server" TabIndex="28"  oncheckedchanged="ckDIACHILD_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlLKHDB" runat="server" TabIndex="24" Visible="false">
                                    </asp:DropDownList>  
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"> 
                                <asp:Label ID="lbLDDIACHI" runat="server" Text="Lý do" Visible="False" ForeColor="Red"></asp:Label>                              
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:TextBox ID="txtLDDIACHI" Width="150px" runat="server" Visible="False" ></asp:TextBox>                                    
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" AutoPostBack="true" runat="server" Width="160px" TabIndex="7" 
                                        onselectedindexchanged="ddlKHUVUC_SelectedIndexChanged" onchange="openWaitingDialog()">
                                    </asp:DropDownList>                                    
                                </div>
                            </td>
                            <td class="crmcell right">
                                Mục đích sử dụng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMDSD" AutoPostBack="true" onchange="openWaitingDialog()" 
                                    OnSelectedIndexChanged="ddlMDSD_SelectedIndexChanged" runat="server" 
                                        Width="150px" TabIndex="25" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckMDSD" runat="server" TabIndex="28"  oncheckedchanged="ckMDSD_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">                               
                            </td>
                            <td class="crmcell">
                            </td>
                            <td class="crmcell right">       
                                <asp:Label ID="lbLDMDSD" runat="server" Text="Lý do" Visible="False" ForeColor="#FF3300"></asp:Label>                           
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLDMDSD" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Phường
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" runat="server" Width="150px" TabIndex="8" 
                                        Enabled="False">
                                    </asp:DropDownList>
                                </div>
                            </td>
                            <td class="crmcell right">
                                Số hộ sử dụng
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOHO" runat="server" Width="20px" TabIndex="26" OnTextChanged="txtSOHO_TextChanged" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckDMUCTAM" runat="server" TabIndex="28" AutoPostBack="true" OnCheckedChanged="ckDMUCTAM_CheckedChanged" Visible="False" />
                                </div>
                                <div class="left">
                                    <strong></strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Đường phố
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADP" onchange="checkDuongPhoForm();" runat="server" 
                                        MaxLength="4" Width="40px" TabIndex="9" Enabled="False" />
                                    <asp:LinkButton ID="linkBtnHidden" CausesValidation="false" style="display:none"  
                                        OnClick="linkBtnHidden_Click" runat="server">Update MADP</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtDUONGPHU" runat="server" MaxLength="1" Width="25px" 
                                        TabIndex="10" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseDP" runat="server" CssClass="pickup" OnClick="btnBrowseDP_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 600, 'divDuongPho')" 
                                        TabIndex="11" Visible="False" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENDUONG" runat="server" />
                                </div>
                            </td> 
                            <td class="crmcell right">Số nhân khẩu</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONK" runat="server" Width="20px" TabIndex="27" />
                                </div>
                                <div class ="left">
                                    <asp:Label ID="lbSODINHMUC" runat="server" Text="ĐM" Font-Bold="True"></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtSODINHMUC" runat="server" Width="20px" TabIndex="27" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="cbISDINHMUC" runat="server" TabIndex="28" OnCheckedChanged="cbISDINHMUC_CheckedChanged" AutoPostBack="true" />
                                </div>
                                <div class="left">
                                    <strong>ĐMNK</strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">                               
                            </td>
                            <td class="crmcell">
                            </td>
                            <td class="crmcell right">       
                                <asp:Label ID="lbLyDoDMNK" runat="server" Text="Lý do" Visible="False" ForeColor="#FF3300"></asp:Label>                           
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLyDoDMNK" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Danh bộ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:HiddenField ID="hdfIDKH" runat="server" />
                                    <asp:TextBox ID="txtMADB" runat="server" MaxLength="6" Width="80px" onchange="checkDanhBoPhoForm();"
                                        TabIndex="12" Enabled="False" OnTextChanged="txtMADB_TextChanged" />       
                                    <asp:LinkButton ID="lbtMADB" CausesValidation="false" style="display:none"  
                                        OnClick="lbtMADB_Click" runat="server">Update DB</asp:LinkButton>                             
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckDANHBO" runat="server" TabIndex="28"  oncheckedchanged="ckDANHBO_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                Nhờ thu
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHUHO"  runat="server" AutoPostBack="true" TabIndex="23" Enabled="false" OnSelectedIndexChanged="ddlTHUHO_SelectedIndexChanged">                                       
                                    </asp:DropDownList>   
                                    <asp:CheckBox ID="ckThuHo" runat="server" TabIndex="24" Visible="true" AutoPostBack="true" OnCheckedChanged="ckThuHo_CheckedChanged" />                             
                                </div>  
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">   
                                <asp:Label ID="lbLDDANHSO" runat="server" Text="Lý do" Visible="False" ForeColor="Red"></asp:Label>                            
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLDDANHSO" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                    
                            </td>
                            <td class="crmcell right">                                    
                                <asp:Label ID="lbLyDoThuHo" runat="server" Text="Lý do" Visible="False" ForeColor="#FF3300"></asp:Label>                   
                            </td>
                            <td class="crmcell right"> 
                                <div class="left">
                                    <asp:TextBox ID="txtLyDoThuHo" runat="server" Visible="False"></asp:TextBox>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtM3" runat="server" Width="30px" TabIndex="29" Visible="False" />
                                </div>    
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">   
                                Phiên (LX)
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtPHIENLX" Width="30px" runat="server" ></asp:TextBox>
                                </div>
                                <td class="crmcell right">
                                    Đợt in HĐ 
                                </td>                                  
                                <td class="crmcell">
                                    <div class="left">                                   
                                        <asp:DropDownList ID="ddlDOTINHD" runat="server" OnSelectedIndexChanged="ddlDOTINHD_SelectedIndexChanged" >                                        
                                        </asp:DropDownList>
                                        <asp:CheckBox ID="ckDotInHD" runat="server" AutoPostBack="true" OnCheckedChanged="ckDotInHD_CheckedChanged"  />
                                    </div>                                
                                </td>                             
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">                                                            
                            </td>
                            <td class="crmcell">                                                   
                            </td>
                            <td class="crmcell right">                                    
                                <asp:Label ID="lbLyDoDotInHD" runat="server" Text="Lý do" Visible="False" ForeColor="#FF3300"></asp:Label>                   
                            </td>
                            <td class="crmcell right"> 
                                <div class="left">
                                    <asp:TextBox ID="txtLyDoDotInHD" runat="server" Visible="False"></asp:TextBox>
                                </div>                                  
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                               Số thứ tự</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSTT" runat="server" MaxLength="20" Width="40px" TabIndex="13" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Không tính
                            </td>
                            <td class="crmcell">
                                
                                <div class="left">
                                    <asp:CheckBox ID="cbKHONGTINH117" runat="server" TabIndex="30" />
                                </div>
                                <div class="left">
                                    <strong>Theo nghị định 117</strong>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Khu vực cấp nước
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUCDN" runat="server" Width="160px" TabIndex="14" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Cơ quan thanh toán
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtCQ" Enabled="false" MaxLength="20" runat="server" Width="100px" TabIndex="31" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseCQ" Enabled="false" runat="server" CssClass="pickup" OnClick="btnBrowseCQ_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách cơ quan thanh toán', 500, 'divCQTT')" 
                                        TabIndex="32" />
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTENCQ" runat="server" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Hình thức thanh toán
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlHTTT" Width="150px" runat="server" TabIndex="15">
                                    </asp:DropDownList> 
                                </div>
                            </td>
                            <td class="crmcell right">
                                Mã giá áp dụng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMAGIA" Enabled="false" Width="112px" runat="server" TabIndex="33">
                                        <asp:ListItem Text="" Value="NULL" />
                                        <asp:ListItem Text="A" Value="A" />
                                        <asp:ListItem Text="B" Value="B" />
                                        <asp:ListItem Text="D" Value="D" />
                                        <asp:ListItem Text="E" Value="E" />
                                        <asp:ListItem Text="T" Value="T" />
                                        <asp:ListItem Text="M" Value="M" />
                                        <asp:ListItem Text="N" Value="N" />
                                        <asp:ListItem Text="I" Value="I" />
                                    </asp:DropDownList> 
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlHOTRO" Width="112px" runat="server" TabIndex="34">
                                        <asp:ListItem Text="Cho không" Value="C" />
                                        <asp:ListItem Text="Tiền mặt" Value="M" />
                                        <asp:ListItem Text="Không" Value="KK" />                                        
                                    </asp:DropDownList> 
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Số tài khoản
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSOTK" runat="server" MaxLength="20" Width="100px" TabIndex="16" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                Ngày lắp đặt</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYHT" runat="server" Width="100px" 
                                        onkeypress="return CheckKeywordFilter(event);" TabIndex="34" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgCreateDate" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" TabIndex="35" />
                                </div>
                                <ajaxToolkit:CalendarExtender ID="ceNgayTao" runat="server" TargetControlID="txtNGAYHT"
                                    PopupButtonID="imgCreateDate" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Mã số thuế
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMSTHUE" runat="server" MaxLength="25" Width="100px" 
                                        TabIndex="17" Enabled="False" />
                                </div>
                                <div class="left">
                                    <asp:CheckBox ID="ckMSTHUE" runat="server" TabIndex="28"  oncheckedchanged="ckMSTHUE_CheckedChanged" 
                                         AutoPostBack="True" />
                                </div>                                
                            </td>
                            <td class="crmcell right">
                                 </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtKYHOTRO" Visible="False" runat="server" Width="100px"  TabIndex="36" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="imgKYHOTRO" ImageUrl="~/content/images/icons/calendar.png"
                                        Visible="false" AlternateText="Click to show calendar" TabIndex="37" />
                                </div>                                
                                <ajaxToolkit:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="txtKYHOTRO"
                                     PopupButtonID="imgKYHOTRO" DefaultView="Months" TodaysDateFormat="dd/MM/yyyy" Format="MM/yyyy" />
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                 <asp:Label ID="lbLDMST" runat="server" Text="Lý do" Visible="False" ForeColor="Red"></asp:Label>                                 
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtLDMST" Width="150px" runat="server" Visible="False"></asp:TextBox>
                                </div>                               
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Trạng thái sử dụng</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTTSD" ReadOnly="true" runat="server" Width="100px" TabIndex="19" />
                                </div>
                            </td>
                            <td class="crmcell right">Ngày thay đồng hồ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYTHAYDH" ReadOnly="true" runat="server" Width="100px" TabIndex="36" />
                                </div>
                            </td>  
                        </tr> 
                         <tr>
                            <td class="crmcell right">Vị trí</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtVITRI" runat="server" Width="200px" MaxLength="500" />
                                </div>
                            </td>
                            <td class="crmcell right">Số thứ tự ID</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSTTTS" ReadOnly="true" runat="server" Width="100px" TabIndex="36" />
                                </div>
                            </td>
                        </tr>                      
                        <tr>
                            <td class="crmcell right">Địa chỉ thường trú</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbDCTHUONGTRU" runat="server" Text=""></asp:Label>
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nơi lắp</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lbNOILAP" runat="server" Text=""></asp:Label>
                                </div>
                                <td class="crmcell right">Địa chỉ lắp</td>
                                <td class="crmcell" colspan="3">
                                    <div class="left">
                                        <asp:Label ID="lbDCLAP" runat="server" Text=""></asp:Label>
                                    </div>
                                </td>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lnkTDCT" OnClick="lnkTDCT_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử thay đổi chi tiết khách hàng', 750, 'divTDCT')" TabIndex="93">
                                        Lược sử thay đổi chi tiết
                                    </asp:LinkButton>
                                </div>                              
                            </td>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:LinkButton ID="lnkTHAYDH" OnClick="lnkTHAYDH_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử thay đồng hồ khách hàng', 750, 'divTDH')" TabIndex="94">
                                        Lược sử thay đồng hồ
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lnkDCHD" OnClick="lnkDCHD_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử điều chỉnh hoá đơn khách hàng', 750, 'divDCHD')" TabIndex="95">
                                        Lược sử điều chỉnh hoá đơn
                                    </asp:LinkButton>
                                </div>                              
                            </td>   
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:LinkButton ID="lnkDSVATTU" runat="server" 
                                        OnClientClick="openDialogAndBlock('Danh sách vật tư của khách hàng', 750, 'divDSVATTU')" TabIndex="96" OnClick="lnkDSVATTU_Click">
                                        Danh sách vật tư của khách hàng
                                    </asp:LinkButton>
                                </div>
                            </td>                         
                        </tr>
                        <tr>
                            <td class="crmcell right"</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:LinkButton ID="lkTRUYTHUVP" OnClick="lkTRUYTHUVP_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Lược sử truy thu vi phạm khách hàng', 750, 'divTTVPKH')" TabIndex="95">
                                        Lược sử truy thu vi phạm
                                    </asp:LinkButton>
                                </div>                              
                            </td>              
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Cập nhật Khách hàng mới khai thác: số nhà, MĐSD, danh bộ, chỉ số, IDKHLX, tiền cọc, số No
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="ckCSCUOIKHAITHAC" runat="server" TabIndex="28"  
                                             AutoPostBack="True" OnCheckedChanged="ckCSCUOIKHAITHAC_CheckedChanged" />                          
                                </div>                                
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    Số nhà
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSONHA2KHTAM" runat="server" TabIndex="17"
                                        Enabled="False" />
                                </div>                                                                   
                            </td>
                            <td class="crmcell right">
                                    Mục đích SD
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMDSDKHMTAM" runat="server" enabled="False"></asp:DropDownList>                                                                    
                                </div>                                                                   
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    Đường phố
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADPKHM" runat="server" MaxLength="4" Width="40px" TabIndex="9" ReadOnly="True"  />
                                    <asp:Label ID="lbMADPKHM" runat="server" Font-Bold="True" ></asp:Label>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btTIMDPKHM" runat="server" CssClass="pickup" OnClick="btTIMDPKHM_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách đường phố', 600, 'divDuongPhoKHM')" 
                                        TabIndex="11" Visible="False" />
                                </div>
                            </td>
                            <td class="crmcell right">
                                    Danh bộ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDANHBOKHM" runat="server" TabIndex="17" MaxLength="6" Enabled="False"/>                                    
                                </div>                                                                   
                            </td>  
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    C.số đầu khi khai thác
                            </td>
                            <td class="crmcell">
                                <div class="left">                                    
                                    <asp:TextBox ID="txtCSDAUKHAITHAC" runat="server" Enabled="False"></asp:TextBox>
                                </div>                                                                   
                            </td>
                            <td class="crmcell right">
                                    C.số cuối khi khai thác
                            </td>
                            <td class="crmcell">
                                <div class="left">                                       
                                    <asp:TextBox ID="txtCSCUOIKHAITHAC" runat="server" Enabled="False"></asp:TextBox>                             
                                </div>                                                                   
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    IDKHLX
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtIDKHLX" runat="server" TabIndex="17" Enabled="False"/>
                                </div>                                                                   
                            </td>
                            <td class="crmcell right">
                                    Tiền cọc
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTIENCOCLX" runat="server" TabIndex="17" Enabled="False"  />                                    
                                </div>                                                                   
                            </td>                            
                        </tr>
                         <tr>
                            <td class="crmcell right">
                                    Số định mức tạm
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtSODINHMUCTAM" runat="server" Enabled="False"/>
                                </div>                                                                                                   
                            </td>
                            <td class="crmcell right">
                                    Loại đồng hồ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbLoaiDongHoKHM" runat="server" ></asp:Label>     
                                    <asp:Label ID="lbMaDongHoKHM" runat="server" Visible="false"></asp:Label>                       
                                </div>                                                                   
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                    Công suất đồng hồ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbCongSuatDongHoKHM" runat="server" ></asp:Label>   
                                </div>                                                                                                   
                            </td>
                             <td class="crmcell right">
                                    Số No đồng hồ
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lbSoNoKHM" runat="server" Font-Bold="True" ></asp:Label>                         
                                </div>
                                <div class="left" >
                                    <asp:Button ID="btDoiSoNoKHM" runat="server" CssClass="myButton" Text="Đổi No KHM" 
                                        CausesValidation="false" UseSubmitBehavior="false" Visible="False"
                                        OnClientClick="openDialogAndBlock('Đổi số No đồng hồ KHM', 500, 'divUpSoNoKHM')" OnClick="btDoiSoNoKHM_Click"  />                                    
                                </div>                                                                
                            </td>
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Đổi thông tin xuất hóa đơn
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="cbSDInfo_INHOADON" runat="server" TabIndex="38" />                                
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENKH_INHOADON" MaxLength="200" runat="server" TabIndex="39" />
                                </div>
                            </td>
                            <td class="crmcell right">Đ.chỉ xuất hóa đơn</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtDIACHI_INHOADON" MaxLength="200" runat="server" TabIndex="40" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Sổ hộ nghèo
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="ckISHONGHEO" runat="server" TabIndex="34" OnCheckedChanged="ckISHONGHEO_CheckedChanged" AutoPostBack="True" Visible="false" />                                
                                </div>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Kỳ hổ trợ</td>
                            <td class="crmcell">                                    
                                <div class="left">
                                    <asp:TextBox ID="txtKYHOTROHN" runat="server" Width="20px" MaxLength="7"    TabIndex="41" Enabled="false" />
                                </div> 
                                <div class="left">
                                    <asp:TextBox ID="txtNAMHOTRO" runat="server" Width="40px" MaxLength="7"    TabIndex="41" Enabled="false" />
                                </div>                                                           
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Địa chỉ khách hàng</td>
                            <td class="crmcell">
                                <div class="left">                                    
                                        <asp:TextBox ID="txtDIACHIHN" runat="server" Width="200px" MaxLength="300" Enabled ="false" />
                                 </div>                                      
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Mã sổ HN</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtMASOHN" runat="server" Width="80px" MaxLength="100" TabIndex="42" Enabled="false" />
                                </div>
                                <td class="crmcell right">Đơn vị cấp sổ HN</td>
                                <td class="crmcell" colspan="3">                                          
                                    <div class="left">
                                         <asp:DropDownList ID="ddlTENXA" Width="180px" runat="server" TabIndex="14" AutoPostBack="True" OnSelectedIndexChanged="ddlTENXA_SelectedIndexChanged" Enabled="false" >
                                         </asp:DropDownList>
                                    </div> 
                                    <div class="left">
                                        <asp:TextBox ID="txtDONVICAP" runat="server" Width="20px" MaxLength="100"   TabIndex="43" Visible="false"/>                                        
                                    </div>                                                        
                                </td>                            
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày ký sổ HN</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYKYSOHN" runat="server" Width="100px" MaxLength="20" TabIndex="44" Enabled="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="ImageButton3" ImageUrl="~/content/images/icons/calendar.png" Visible="false" 
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender5" runat="server" TargetControlID="txtNGAYKYSOHN"
                                        PopupButtonID="imgNGAYKYSOHN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>                                
                            </td>                            
                        </tr> 
                        <tr>
                            <td class="crmcell right">Ngày cấp sổ HN</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAPCAPHN" runat="server" Width="100px" MaxLength="20" TabIndex="45"  Enabled="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="ImageButton1" ImageUrl="~/content/images/icons/calendar.png"
                                        AlternateText="Click to show calendar" Visible="false" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender3" runat="server" TargetControlID="txtNGAPCAPHN"
                                        PopupButtonID="imgNGAPCAPHN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <td class="crmcell right">Ngày kết thúc HN</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:TextBox ID="txtNGAYKTHN" runat="server" Width="90px" MaxLength="20" TabIndex="46" Enabled="false"  />
                                    </div>  
                                    <div class="left">
                                        <asp:ImageButton runat="Server" ID="ImageButton2" ImageUrl="~/content/images/icons/calendar.png" Visible="false"
                                            AlternateText="Click to show calendar" />
                                        <ajaxToolkit:CalendarExtender ID="CalendarExtender4" runat="server" TargetControlID="txtNGAYKTHN"
                                            PopupButtonID="imgNGAYKTHN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                    </div>                                  
                                </td>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Khách hàng vi phạm
                                </div> 
                                <div class="left" style="padding-left: 10px">
                                    <asp:CheckBox ID="ckISVIPHAM" runat="server" TabIndex="34" OnCheckedChanged="ckISVIPHAM_CheckedChanged" AutoPostBack="True" Visible="true" />                                
                                </div>                              
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ngày trả tiền</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtNGAYTRATIEN" runat="server" Width="100px" MaxLength="20" TabIndex="44" Enabled="false" />
                                </div>
                                <div class="left">
                                    <asp:ImageButton runat="Server" ID="ImageButton4" ImageUrl="~/content/images/icons/calendar.png" Visible="false" 
                                        AlternateText="Click to show calendar" />
                                    <ajaxToolkit:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="txtNGAYTRATIEN"
                                        PopupButtonID="imgNGAYTRATIEN" TodaysDateFormat="dd/MM/yyyy" Format="dd/MM/yyyy" />
                                </div>
                                <td class="crmcell right">Tiền KH vi phạm</td>
                                <td class="crmcell" colspan="3"> 
                                    <div class="left">
                                        <asp:TextBox ID="txtTONGTIENVIPH" runat="server" Width="80px" MaxLength="100" TabIndex="43" Visible="true" Enabled="false"/>                                        
                                    </div>                                                        
                                </td>                                                            
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">Tiền KH trả</td>
                            <td class="crmcell">
                                <div class="left">
                                        <asp:TextBox ID="txtSOTIENTRA" runat="server" Width="80px" MaxLength="100" TabIndex="43" Visible="true" Enabled="false"/>                                        
                                 </div>                                
                                <td class="crmcell right">Tiền còn lại</td>
                                <td class="crmcell" colspan="3"> 
                                    <div class="left">
                                        <asp:TextBox ID="txtSOTIENCL" runat="server" Width="80px" MaxLength="100" TabIndex="43" Visible="true" Enabled="false"/>                                        
                                    </div>                                                        
                                </td>                               
                            </td>                            
                        </tr>
                        <tr>
                            <td class="header btop" colspan="6">
                                <div class="left">
                                    Ghi chú kinh doanh - Ghi chú kỳ ghi chỉ số
                                </div>                                                              
                            </td>
                        </tr>       
                        <tr>
                            <td class="crmcell right">Kỳ ghi</td>
                            <td class="crmcell">
                                <div class="left">
                                   <asp:DropDownList ID="ddlTHANGGC" runat="server" >
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
                                    <asp:TextBox ID="txtNAMGC" runat="server" Width="50px" MaxLength="4" />
                                    <asp:Label ID="lbIDKHGC" runat="server" Visible="False"></asp:Label>
                                </div>
                                <td>
                                    <div class="left">
                                        <asp:Button ID="btLuuGhiChuTT" runat="server" CssCLass="myButton" Text="Lưu ghi chú" 
                                            OnClick="btLuuGhiChuTT_Click" OnClientClick="CheckFormbtLuuGhiChuTT();"/>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ghi chú kinh doanh</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtGHICHUKDLX" runat="server" Width="200px"  />           
                                </div>
                                <td class="crmcell right">Ghi chú chỉ số</td>
                                <td class="crmcell" colspan="3"> 
                                    <div class="left">
                                        <asp:TextBox ID="txtGHICHUCSLX" runat="server" Width="200px"  />                                        
                                    </div>                                                        
                                </td> 
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:LinkButton ID="linkBtnTieuThu" OnClick="linkBtnTieuThu_Click" runat="server" 
                                        OnClientClick="openDialogAndBlock('Thông tin tiêu thụ khách hàng', 750, 'divTTTT')" TabIndex="97">
                                        Xem thông tin ghi chú - tiêu thụ
                                    </asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="4" class="bbottom pbottom-10">
                            </td>
                        </tr>   
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CommandArgument="Insert" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="CheckFormSave();"
                                        TabIndex="47" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" OnClick="btnCancel_Click"
                                        TabIndex="48" UseSubmitBehavior="false" />                                    
                                </div>                                
                            </td>
                        </tr>             
                    </tbody>
                </table>
            </div>                     
            <br />
            <div class="crmcontainer">
                <eoscrm:Grid 
                    ID="gvKhachHang" runat="server" UseCustomPager="true" PageSize=20
                        OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#" HeaderStyle-Width="15%">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="12%" HeaderText="Danh số">
                            <ItemTemplate>
                            <asp:LinkButton ID="linkMa" runat="server" 
                                CommandArgument='<%# Eval("IDKH") %>'
                                CommandName="SelectHD" OnClientClick="openWaitingDialog();unblockWaitingDialog();" 
                                Text='<%# HttpUtility.HtmlEncode(Eval("MADP") + Eval("DUONGPHU").ToString()+ Eval("MADB")) %>'>
                            </asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField> 
                        <asp:BoundField HeaderStyle-Width="5%" HeaderText="Số nhà" DataField="SONHA" />
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên khách hàng" DataField="TENKH" />
                        <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Địa chỉ">
                            <ItemTemplate>
                                <%# Eval("SONHA") != null && Eval("SONHA").ToString() != "" ? Eval("SONHA") + ", " : ""%>
                                <%# Eval("DUONGPHO.TENDP")%>
                            </ItemTemplate>
                        </asp:TemplateField>    
                        <asp:TemplateField HeaderStyle-Width="8%" HeaderText="Khu vực">
                            <ItemTemplate>
                                <%# Eval("KHUVUC.TENKV") %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="7%" HeaderText="Kỳ KT">
                            <ItemTemplate>
                                <%# Eval("KYKHAITHAC") != null ? String.Format("{0:MM/yyyy}", Eval("KYKHAITHAC")) : ""  %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="80px" HeaderText="Số No">
                            <ItemTemplate>
                                <%# Eval("DONGHO.SONO") %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="15px" HeaderText="MĐSD">
                            <ItemTemplate>
                                <%# Eval("MAMDSD") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Ngày bấm chì">
                            <ItemTemplate>
                                <%# (Eval("NGAYHT") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYHT"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Ngày thay">
                            <ItemTemplate>
                                <%# (Eval("NGAYTHAYDH") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYTHAYDH"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Ngày nhập">
                            <ItemTemplate>
                                <%# (Eval("NGAYGNHAP") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYGNHAP"))
                                        : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Xem tiêu thụ">
                            <ItemTemplate>
                                <asp:LinkButton ID="linkTieuThu" runat="server"
                                    CommandArgument='<%# Eval("IDKH") %>'
                                    CommandName="SelectTT" 
                                    OnClientClick="openDialogAndBlock('Thông tin tiêu thụ khách hàng', 750, 'divTTTT')"                                     
                                    Text='Xem t.thụ'>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>   
            </div>         
        </ContentTemplate>
    </asp:UpdatePanel> 
</asp:Content>
