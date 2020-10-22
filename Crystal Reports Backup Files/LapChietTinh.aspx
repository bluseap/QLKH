<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" EnableEventValidation="true"
    CodeBehind="LapChietTinh.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.LapChietTinh" %>

<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Web.Common" %>

<%@ Register Src="../../UserControls/FilterPanel.ascx" TagName="FilterPanel" TagPrefix="bwaco" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
		$(document).ready(function() {
			$("#divVatTu").dialog({
				autoOpen: false,
				modal: true,
				minHeight: 30,
				height: 'auto',
				width: 'auto',
				resizable: false,
				open: function(event, ui) {
					$(this).parent().appendTo("#divVatTuDlgContainer");
				}
			});;
		});

        //nhay qua save
		function clickButtonSAVE(e) {
		    var evt = e ? e : window.event;
		    if (evt.keyCode == 13) {
		        $('#<%=btnSave.ClientID %>').focus();
                return false;
            }
        }

        /* phần vật tư miễn phí */
        
        function CheckFormMAVTKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeMAVT) %>', '');
            }
        }

        function CheckFormKhoiLuongKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeKhoiLuong) %>', '');
            }
        }

        /* phần vật tư khách hàng thanh toán */
        
        function CheckFormMAVT117KeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeMAVT117) %>', '');
            }
        }

        function CheckFormKhoiLuong117KeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function() { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeKhoiLuong117) %>', '');
            }
        }

        function CheckFormAddChiPhi() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddChiPhi) %>', '');

            return false;
        }

        function CheckFormAddChiPhi117() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddChiPhi117) %>', '');
        }

        function CheckFormNCVUOT() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnNCVUOT) %>', '');
            return false;
        }
        
        function CheckFormFilterVatTu() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTu) %>', '');

            return false;
        }

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

            return false;
        }

        function CheckFormCapNhatGiaTri() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCapNhatGiaTri) %>', '');

            return false;
        }

        function CheckFormBaoCao() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');

            return false;
        }

        function checkChange() {
            
            if (confirm('Tất cả vật tư có sẵn sẽ bị thay thế bởi mẫu bốc vật tư. Đổi?')) {
                openWaitingDialog();
                unblockWaitingDialog();
                
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnChange) %>', '');
            }

            return false;
        }

        function updateCTCT(maMBVT, maVT, txtSLId, txtGIAVTId, lblTIENVT, txtGIANCId, lblTIENNC) {

		    openWaitingDialog();
		    unblockWaitingDialog();
		    
		    var sl = document.getElementById(txtSLId).value;
		    var gvt = document.getElementById(txtGIAVTId).value;
		    var gnc = document.getElementById(txtGIANCId).value;

		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCTCT(maMBVT, maVT, sl, gvt, gnc);		    
		    		   
		    if(msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		    
		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");
    		    
		        if(idArr.length == 3) {
                    document.getElementById(txtSLId).value = idArr[0];
                    document.getElementById(txtGIAVTId).value = idArr[1];
                    //document.getElementById(txtTIENVTId).value = idArr[2];
                    document.getElementById(txtGIANCId).value = idArr[2];
                    //document.getElementById(txtTIENNCId).value = idArr[4];
		        }
		    }

		    if (msg.value == "<%= DELIMITER.Passed %>") 
		    {
		        var sl1 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gvt);
		        var sl2 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gnc);
		        document.getElementById(lblTIENVT).innerHTML = sl1;
		        document.getElementById(lblTIENNC).innerHTML = sl2;
		    }
		    closeWaitingDialog();
		}
		
		function updateCTCT117(maDDK, maVT, txtSLId, txtGIAVTId, lblTIENVT, txtGIANCId, lblTIENNC) {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    
		    var sl = document.getElementById(txtSLId).value;
		    var gvt = document.getElementById(txtGIAVTId).value;
		    var gnc = document.getElementById(txtGIANCId).value;
		    
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCTCT117(maDDK, maVT, sl, gvt, gnc);		    
		    		   
		    if(msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		    
		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");
    		    
		        if(idArr.length == 3) {
                    document.getElementById(txtSLId).value = idArr[0];
                    document.getElementById(txtGIAVTId).value = idArr[1];
                    //document.getElementById(txtTIENVTId).value = idArr[2];
                    document.getElementById(txtGIANCId).value = idArr[2];
                    //document.getElementById(txtTIENNCId).value = idArr[4];
		        }
		    }
		    
		    if(msg.value == "<%= DELIMITER.Passed %>") {
		        var sl1 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gvt);
		        var sl2 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gnc);
		        document.getElementById(lblTIENVT).innerHTML = sl1;
		        document.getElementById(lblTIENNC).innerHTML = sl2;	
		        __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnRefreshTongHopChiPhi) %>', '');
		    }

		    closeWaitingDialog();
		}
		
		function updateGCCT(maGC, clientId)
		{
		    var noidung = document.getElementById(clientId).value;
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateGCCT(maGC, noidung);		    
		    		   
		    if(msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		        document.getElementById(clientId).value = msg.value;
		    }
		}
		
		function updateCPCT(maCP, txtNDclientId, txtDGclientId, ddlDVTclientId, txtSLclientId, 
		    txtHSclientId, lblTTclientId, ddlLCPclientId) {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    var nd = document.getElementById(txtNDclientId).value;
		    var dg = document.getElementById(txtDGclientId).value;
		    
		    var e = document.getElementById(ddlDVTclientId);
		    var dvt = e.options[e.selectedIndex].value;
		    
		    var sl = document.getElementById(txtSLclientId).value;
		    var hs = document.getElementById(txtHSclientId).value;
		    
		    var f = document.getElementById(ddlLCPclientId);
		    var loai = f.options[f.selectedIndex].value;
		    
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPCT(maCP, nd, dg, dvt, sl, hs, loai);		    
		    		   
		    if(msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		    
		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");
    		    
		        if(idArr.length == 6) {
                    document.getElementById(txtNDclientId).value = idArr[0];
                    document.getElementById(txtDGclientId).value = idArr[1];
                    document.getElementById(ddlDVTclientId).value = idArr[2];
                    document.getElementById(txtSLclientId).value = idArr[3];
                    document.getElementById(txtHSclientId).value = idArr[4];
                    //document.getElementById(txtTTclientId).value = idArr[5];
                    document.getElementById(ddlLCPclientId).value = idArr[5];
		        }
		    }
		    
		    if(msg.value == "<%= DELIMITER.Passed %>")
		    {
		        document.getElementById(lblTTclientId).innerHTML = 
		            parseFloat(sl.replace(/,/g, '.')) * parseFloat(hs.replace(/,/g, '.')) * dg;
		    }
		    closeWaitingDialog();
		}
		
		function updateCPCT117(maCP, txtNDclientId, txtDGclientId, ddlDVTclientId, txtSLclientId, 
		    txtHSclientId, lblTTclientId, ddlLCPclientId) {
		    openWaitingDialog();
		    unblockWaitingDialog();
		
		    var nd = document.getElementById(txtNDclientId).value;
		    var dg = document.getElementById(txtDGclientId).value;		    
		    var e = document.getElementById(ddlDVTclientId);
		    var dvt = e.options[e.selectedIndex].value;		    
		    var sl = document.getElementById(txtSLclientId).value;
		    var hs = document.getElementById(txtHSclientId).value;		    
		    var f = document.getElementById(ddlLCPclientId);
		    var loai = f.options[f.selectedIndex].value;
		    
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPCT117(maCP, nd, dg, dvt, sl, hs, loai);		    
		    		   
		    if(msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		    
		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");
    		    
		        if(idArr.length == 6) {
                    document.getElementById(txtNDclientId).value = idArr[0];
                    document.getElementById(txtDGclientId).value = idArr[1];
                    document.getElementById(ddlDVTclientId).value = idArr[2];
                    document.getElementById(txtSLclientId).value = idArr[3];
                    document.getElementById(txtHSclientId).value = idArr[4];
                    document.getElementById(ddlLCPclientId).value = idArr[5];
		        }
		    }
		    
		    if(msg.value == "<%= DELIMITER.Passed %>")
		    {
		        document.getElementById(lblTTclientId).innerHTML =
		            parseFloat(sl.replace(/,/g, '.')) * parseFloat(hs.replace(/,/g, '.')) * dg;
		        //__doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnRefreshTongHopChiPhi) %>', '');		        
		    }
		    closeWaitingDialog();
		}
		
        
		function updateCPNCVUOT(maCP, txtNDclientId, txtDGclientId, ddlDVTclientId, txtSLclientId,
		    txtHSclientId, lblTTclientId, ddlLCPclientId) {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    var nd = document.getElementById(txtNDclientId).value;
		    var dg = document.getElementById(txtDGclientId).value;
		    var e = document.getElementById(ddlDVTclientId);
		    var dvt = e.options[e.selectedIndex].value;
		    var sl = document.getElementById(txtSLclientId).value;
		    var hs = document.getElementById(txtHSclientId).value;
		    var f = document.getElementById(ddlLCPclientId);
		    var loai = f.options[f.selectedIndex].value;

		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPCT117(maCP, nd, dg, dvt, sl, hs, loai);

		    if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {

		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");

		        if (idArr.length == 6) {
		            document.getElementById(txtNDclientId).value = idArr[0];
		            document.getElementById(txtDGclientId).value = idArr[1];
		            document.getElementById(ddlDVTclientId).value = idArr[2];
		            document.getElementById(txtSLclientId).value = idArr[3];
		            document.getElementById(txtHSclientId).value = idArr[4];
		            document.getElementById(ddlLCPclientId).value = idArr[5];
		        }
		    }

		    if (msg.value == "<%= DELIMITER.Passed %>") {
		        document.getElementById(lblTTclientId).innerHTML =
		            parseFloat(sl.replace(/,/g, '.')) * parseFloat(hs.replace(/,/g, '.')) * dg;
		        //__doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnRefreshTongHopChiPhi) %>', '');		        
		    }
		    closeWaitingDialog();
		}
		
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <bwaco:FilterPanel ID="filterPanel" runat="server" />
    <br />    
    <div class="crmcontainer" id="divGridList" runat="server">
        <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_RowDataBound"
            OnRowCommand="gvList_RowCommand"  OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
            <PagerSettings FirstPageText="thiết kế" PageButtonCount="2" />
            <Columns>
                <asp:TemplateField HeaderText="Mã đơn" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                            CommandName="EditItem" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                    </ItemTemplate>
                    <ItemStyle Font-Bold="True" />
                </asp:TemplateField>
                <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên khách hàng" DataField="TENKH" />
                <asp:BoundField HeaderStyle-Width="75px" HeaderText="Điện thoại" DataField="DIENTHOAI" />
                <asp:BoundField HeaderStyle-Width="35%" HeaderText="Địa chỉ lắp đặt" DataField="DIACHILD" />
                <asp:TemplateField HeaderStyle-Width="75px" HeaderText="Ngày đăng ký">
                    <ItemTemplate>
                        <%# (Eval("NGAYDK") != null) ?
                                String.Format("{0:dd/MM/yyyy}", Eval("NGAYDK"))
                                : "" %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="80px">
                    <ItemTemplate>
                        <asp:LinkButton ID="lnkBtnID2" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                            CommandName="EditItem" Text='Chạy chiết tính'></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </eoscrm:Grid>
    </div>
    
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div id="divChietTinhInfo" class="crmcontainer" runat="server" visible="false">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Công trình</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENCT" runat="server" MaxLength="200" Width="300px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHUONG" TabIndex="3" runat="server" Visible="false"
                                        > 
                                    </asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tổng tiền</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTONGTIENCT" runat="server" MaxLength="200" Width="100px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Công tác</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTenHM" runat="server" MaxLength="200" Width="300px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Địa chỉ lắp</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Label ID="lblDIACHI" runat="server" Text="[Địa chỉ lắp đặt]"></asp:Label>                                    
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Ghi chú</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" runat="server" MaxLength="200" Width="300px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" OnClientClick="return CheckFormSave();"
                                        CssClass="save" OnClick="btnSave_Click" TabIndex="16" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div id="divVatTuDlgContainer">	
		<div id="divVatTu" style="display:none">		    
	        <asp:UpdatePanel ID="upnlVatTu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table cellpadding="3" cellspacing="1" style="width: 700px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">                                                
                                                    <asp:TextBox ID="txtFilterVatTu" onchange="return CheckFormFilterVatTu();" runat="server" />
                                                </div>
                                                <div class="left">  
                                                    <asp:Button ID="btnFilterVatTu" OnClientClick="return CheckFormFilterVatTu();" 
                                                        runat="server" CssClass="filter" UseSubmitBehavior="false" OnClick="btnFilterVatTu_Click" />
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
                                    <eoscrm:Grid ID="gvVatTu" runat="server" UseCustomPager="true" 
						                OnRowDataBound="gvVatTu_RowDataBound" OnRowCommand="gvVatTu_RowCommand" PageSize="16"
						                OnPageIndexChanging="gvVatTu_PageIndexChanging">
							            <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MAVT") %>'
                                                        CommandName="EditItem" Text='<%# Eval("MAVT") %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Mã hiệu" DataField="MAHIEU" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Tên vật tư" DataField="TENVT" HeaderStyle-Width="45%" />
                                            <asp:BoundField HeaderText="Đơn vị tính" DataField="DVT" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Giá VT" DataField="GIAVT" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Giá NC" DataField="GIANC" HeaderStyle-Width="10%" />
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
	    
    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Mẫu bốc vật tư:</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMBVT" Width="200px" runat="server">
                                    </asp:DropDownList>
                                </div>
                                <div class="left">
                                    <span style="color: Red; font-weight: bold">Chú ý: Thay đổi mẫu bốc vật tư sẽ xóa hết danh sách vật tư hiện tại.</span>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnChange" runat="server" CommandArgument="Change" CssClass="change"
                                        OnClientClick="return checkChange();" OnClick="btnChange_Click" TabIndex="16" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
            <br />
            <table class="crmcontainer">
                <tr>
                    <td class="header">PHẦN CHI PHÍ VẬT TƯ MIỄN PHÍ</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right">Mã vật tư</td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtMAVT" Width="60px" runat="server" onkeypress="return CheckFormMAVTKeyPress(event);" />
		                                <asp:LinkButton ID="linkBtnChangeMAVT" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeMAVT_Click" runat="server">Change MAVT</asp:LinkButton>
		                            </div>
		                            <div class="left">
		                                <asp:Button ID="btnBrowseVatTu" runat="server" CssClass="pickup" 
		                                    OnClick="btnBrowseVatTu_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 700, 'divVatTu')"  />
		                            </div>
		                            <div class="left">
		                                <asp:TextBox ID="txtKHOILUONG" Width="60px" runat="server"
		                                    onkeypress="return CheckFormKhoiLuongKeyPress(event);" />
		                                <asp:LinkButton ID="linkBtnChangeKhoiLuong" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeKhoiLuong_Click" runat="server">Change KL</asp:LinkButton>
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lblTENVT" runat="server" Text=""></asp:Label>
		                            </div>
		                            
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
                            <eoscrm:Grid ID="gvSelectedVatTu" runat="server" UseCustomPager="true" PageSize="2000" 
                                OnRowCommand="gvSelectedVatTu_RowCommand" OnRowDataBound="gvSelectedVatTu_RowDataBound">
                                <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("MAVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.MAHIEU") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nội dung công việc">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.TENVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ĐVT" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.DVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOLUONG" Text='<%# Bind("SOLUONG") %>' Width="60px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Giá vật tư" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGIAVT" Text='<%# Bind("GIAVT") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tiền vật tư" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENVT" Text='<%# Bind("TIENVT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Giá NC" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGIANC" Text='<%# Bind("GIANC") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tiền NC" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENNC" Text='<%# Bind("TIENNC") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
					                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
						                <ItemTemplate>
							                <asp:LinkButton ID="btnDelete" Text="Xóa" CommandName="DeleteVatTu" 
							                    CausesValidation="false" CommandArgument='<%# Eval("MAVT")%>' runat="server"></asp:LinkButton>	
						                </ItemTemplate>
					                </asp:TemplateField>
				                </Columns>
			                </eoscrm:Grid>
			            </div>
		            </td>
		        </tr>		        
		        <tr style="display:none">
                    <td class="header">Chi phí đào lắp</td> 
		        </tr>
		        <tr style="display:none">
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:Button ID="btnAddChiPhi" runat="server" CssClass="addnew" OnClientClick="return CheckFormAddChiPhi();" 
		                                    OnClick="btnAddChiPhi_Click" CausesValidation="false" UseSubmitBehavior="false" />
		                            </div>
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
		                    <eoscrm:Grid ID="gvChiPhi" runat="server" UseCustomPager="true" PageSize="2000"
			                    OnRowCommand="gvChiPhi_RowCommand" OnRowDataBound="gvChiPhi_RowDataBound" >
			                    <PagerSettings FirstPageText="chi phí" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nội dung">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNOIDUNG" Text='<%# Bind("NOIDUNG") %>' Width="98%" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn giá" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                           <asp:TextBox ID="txtDONGIA" Text='<%# Bind("DONGIACP") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn vị" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDVT" runat="server" DataSource="<%# new DvtDao().GetList() %>" 
                                                DataTextField="TENDVT" DataValueField="DVT1" Width="100px" SelectedValue='<%# Bind("DVT") %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOLUONG" Text='<%# Bind("SOLUONG") %>' Width="50px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hệ số" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHESOCP" Text='<%# Bind("HESOCP") %>' Width="50px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Thành tiền" HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTHANHTIENCP" runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loại chi phí" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlLOAICP" runat="server" SelectedValue='<%# Bind("LOAICP") %>'
                                                ToolTip='<%# Bind("LOAICP") %>' Width="98%">
                                                <asp:ListItem Value="LAP">Lắp</asp:ListItem>
                                                <asp:ListItem Value="DAO">Đào</asp:ListItem>
                                            </asp:DropDownList>                         
                                        </ItemTemplate>
                                    </asp:TemplateField>
					                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
						                <ItemTemplate>
							                <asp:LinkButton ID="btnDelete" Text="Xóa" CommandName="DeleteChiPhi" 
							                    CausesValidation="false" CommandArgument='<%# Eval("MADON") %>' runat="server"></asp:LinkButton>	
						                </ItemTemplate>
					                </asp:TemplateField>
				                </Columns>
                            </eoscrm:Grid>	
                        </div>		            
		            </td>		        
		        </tr>
		        <tr>
                    <td class="header">PHẦN CHI PHÍ VẬT TƯ KHÁCH HÀNG THANH TOÁN</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right">Mã vật tư</td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtMAVT117" Width="60px" runat="server" onkeypress="return CheckFormMAVT117KeyPress(event);" />
		                                <asp:LinkButton ID="linkBtnChangeMAVT117" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeMAVT117_Click" runat="server">Change MAVT 117</asp:LinkButton>
		                            </div>
		                            <div class="left">
		                                <asp:Button ID="btnBrowseVatTu117" runat="server" CssClass="pickup" 
		                                    OnClick="btnBrowseVatTu117_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 700, 'divVatTu')"  />
		                            </div>
		                            <div class="left">
		                                <asp:TextBox ID="txtKHOILUONG117" Width="60px" runat="server"
		                                    onkeypress="return CheckFormKhoiLuong117KeyPress(event);" />
		                                <asp:LinkButton ID="linkBtnChangeKhoiLuong117" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeKhoiLuong117_Click" runat="server">Change KL 117</asp:LinkButton>
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lblTENVT117" runat="server" Text=""></asp:Label>
		                            </div>
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
                            <eoscrm:Grid ID="gvSelectedVatTu117" runat="server" UseCustomPager="true" PageSize="2000" 
                                OnRowCommand="gvSelectedVatTu117_RowCommand" OnRowDataBound="gvSelectedVatTu117_RowDataBound">
                                <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("MAVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.MAHIEU") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nội dung công việc">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.TENVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="ĐVT" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.DVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOLUONG" Text='<%# Bind("SOLUONG") %>' Width="60px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Giá vật tư" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGIAVT" Text='<%# Bind("GIAVT") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tiền vật tư" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENVT" Text='<%# Bind("TIENVT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Giá NC" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtGIANC" Text='<%# Bind("GIANC") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Tiền NC" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENNC" Text='<%# Bind("TIENNC") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
					                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
						                <ItemTemplate>
							                <asp:LinkButton ID="btnDelete" Text="Xóa" CommandName="DeleteVatTu" 
							                    CausesValidation="false" CommandArgument='<%# Eval("MAVT")%>' runat="server"></asp:LinkButton>	
						                </ItemTemplate>
					                </asp:TemplateField>
				                </Columns>
			                </eoscrm:Grid>
			            </div>
		            </td>
		        </tr>
		         <tr>		            
                    <td class="crmcell center">                        
                        <asp:LinkButton ID="lkTIENVTCTCAP" CausesValidation="false" UseSubmitBehavior="false" 
                           runat="server">Tiền vật tư Công ty cấp:        </asp:LinkButton>
                        <asp:Label ID="lbTIENVTCTCAP" runat="server" Font-Size="Large" ></asp:Label>
                    </td>
                </tr>
		        <tr>		            
                    <td class="crmcell center">                        
                        <asp:LinkButton ID="lkTIENVT" CausesValidation="false" UseSubmitBehavior="false" 
                           runat="server">Tiền vật tư:        </asp:LinkButton>
                        <asp:Label ID="lbTIENVT" runat="server" Font-Size="Large" ></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell center">
                        <asp:LinkButton ID="lkTIENNC" CausesValidation="false" UseSubmitBehavior="false" 
                           runat="server">Tiền nhân công:        </asp:LinkButton>
                        <asp:Label ID="lbTIENNC" runat="server" Font-Size="Large" ></asp:Label>
                    </td>
                </tr>	        
                <tr>
                    <td class="header">PHẦN CHI PHÍ NHÂN CÔNG VƯỢT, LẮP - KHÁCH HÀNG THANH TOÁN</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>		                        
                                <td class="crmcell center">                                    
                                     <div class="center">
		                                <asp:Button ID="btnNCVUOT" runat="server" CssClass="addnew" OnClientClick="return CheckFormNCVUOT();"
		                                     CausesValidation="false" UseSubmitBehavior="false" onclick="btnNCVUOT_Click"
		                                      />
		                            </div>
                                </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important; border-bottom: none !important;">
		                    <eoscrm:Grid ID="gvNCVUOT" runat="server" UseCustomPager="true" PageSize="2000" AutoGenerateColumns="false" CssClass="crmgrid"
			                    OnRowCommand="gvNCVUOT_RowCommand" OnRowDataBound="gvNCVUOT_RowDataBound" >
			                    <RowStyle CssClass="row" />
                                <AlternatingRowStyle CssClass="altrow" />
                                <HeaderStyle CssClass="header" />
			                    <PagerSettings FirstPageText="chi phí" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nội dung">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNOIDUNGNCVUOT" Text='<%# Bind("NOIDUNG") %>' Width="98%" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn giá" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                           <asp:TextBox ID="txtDONGIANCVUOT" Text='<%# Bind("DONGIACP") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn vị" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDVTNCVUOT" runat="server" DataSource="<%# new DvtDao().GetList() %>" 
                                                DataTextField="TENDVT" DataValueField="DVT1" Width="100px" SelectedValue='<%# Bind("DVT") %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOLUONGNCVUOT" Text='<%# Bind("SOLUONG") %>' Width="50px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hệ số" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHESOCPNCVUOT" Text='<%# Bind("HESOCP") %>' Width="50px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Thành tiền" HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTHANHTIENCPNCVUOT" Text='<%# Bind("THANHTIENCP")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loại chi phí" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlLOAICPNCVUOT" runat="server" SelectedValue='<%# Bind("LOAICP") %>'
                                                ToolTip='<%# Bind("LOAICP") %>' Width="98%">
                                                <asp:ListItem Value="NC">Nhân công</asp:ListItem>
                                                <asp:ListItem Value="TT">Chi phí trực tiếp</asp:ListItem>
                                                <asp:ListItem Value="C">Cộng</asp:ListItem> 
                                                <asp:ListItem Value="CPC">Chi phí chung</asp:ListItem>                                               
                                                <asp:ListItem Value="TL">Thu nhập chịu thuế</asp:ListItem>
                                                <asp:ListItem Value="Z">Công xây lắp trước thuế</asp:ListItem>
                                                <asp:ListItem Value="XL">Thuế VAT</asp:ListItem>
                                                <asp:ListItem Value="VXT">Xây lắp sau thuế</asp:ListItem>
                                            </asp:DropDownList>                         
                                        </ItemTemplate>
                                    </asp:TemplateField>
					                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
						                <ItemTemplate>
							                <asp:LinkButton ID="btnDeleteNCVUOT" Text="Xóa" CommandName="DeleteChiPhi" 
							                    CausesValidation="false" CommandArgument='<%# Eval("MADON") %>' runat="server"></asp:LinkButton>	
						                </ItemTemplate>
					                </asp:TemplateField>
				                </Columns>
                            </eoscrm:Grid>	
                        </div>
		            </td>
		        </tr>
		        <tr>		            
                    <td class="crmcell center">
                        <asp:LinkButton ID="lkTHONGKE" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnRefresh_Click"
                           runat="server">Chi phí khách hàng chịu:        </asp:LinkButton>
                        <asp:Label ID="lblTHONGKE" runat="server" Font-Size="Large" ></asp:Label>
                    </td>
                </tr>
		        <tr>
                    <td class="header">PHẦN CHI PHÍ VẬN CHUYỂN KHÁCH HÀNG THANH TOÁN</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:Button ID="btnAddChiPhi117" runat="server" CssClass="addnew" OnClientClick="return CheckFormAddChiPhi117();" 
		                                    OnClick="btnAddChiPhi117_Click" CausesValidation="false" UseSubmitBehavior="false" />
		                            </div>
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important; border-bottom: none !important;">
		                    <eoscrm:Grid ID="gvChiPhi117" runat="server" UseCustomPager="true" PageSize="2000"
			                    OnRowCommand="gvChiPhi117_RowCommand" OnRowDataBound="gvChiPhi117_RowDataBound" >
			                    <PagerSettings FirstPageText="chi phí" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nội dung">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNOIDUNG" Text='<%# Bind("NOIDUNG") %>' Width="98%" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn giá" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                           <asp:TextBox ID="txtDONGIA" Text='<%# Bind("DONGIACP") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn vị" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDVT" runat="server" DataSource="<%# new DvtDao().GetList() %>" 
                                                DataTextField="TENDVT" DataValueField="DVT1" Width="100px" SelectedValue='<%# Bind("DVT") %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOLUONG" Text='<%# Bind("SOLUONG") %>' Width="50px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Hệ số" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHESOCP" Text='<%# Bind("HESOCP") %>' Width="50px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Thành tiền" HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTHANHTIENCP" Text='<%# Bind("THANHTIENCP")%>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loại chi phí" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlLOAICP" runat="server" SelectedValue='<%# Bind("LOAICP") %>'
                                                ToolTip='<%# Bind("LOAICP") %>' Width="98%">
                                                <asp:ListItem Value="VC">Vận chuyển</asp:ListItem>
                                                <asp:ListItem Value="LAP">Lắp</asp:ListItem>
                                                <asp:ListItem Value="DAO">Đào</asp:ListItem>                                                
                                            </asp:DropDownList>                         
                                        </ItemTemplate>
                                    </asp:TemplateField>
					                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
						                <ItemTemplate>
							                <asp:LinkButton ID="btnDelete" Text="Xóa" CommandName="DeleteChiPhi" 
							                    CausesValidation="false" CommandArgument='<%# Eval("MADON") %>' runat="server"></asp:LinkButton>	
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
                        
    <asp:UpdatePanel ID="upnlTongHopChiPhi" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <asp:LinkButton ID="btnRefreshTongHopChiPhi" CausesValidation="false" UseSubmitBehavior="false" OnClick="btnRefreshGrid_Click"
                Style="display: none" runat="server"></asp:LinkButton>
            <div class="crmcontainer">
                <table class="crmtable" style="display: none">
                    <tr>
                        <td class="crmcell right width-200">
                            <asp:Button ID="btnCapNhatGiaTri" CssClass="save" OnClientClick="return CheckFormCapNhatGiaTri();" 
                                    runat="server" OnClick="btnCapNhatGiaTri_Click" UseSubmitBehavior="false" />
                        </td>
                        <td class="crmcell width-250">
                            <div class="left">
                                
                            </div>
                        </td>
                        <td class="crmcell width-100"></td>
                        <td class="crmcell"></td>
                    </tr>
                    <tr>
                        <td class="crmcell right width-200">Giảm giá nhân công</td>
                        <td class="crmcell width-250">
                            <div class="center">
                                <asp:TextBox ID="txtGIAMGIACPNC" runat="server" Text="0" MaxLength="6" Width="40px" /> (%)
                            </div>
                        </td>
                        <td class="crmcell width-100"></td>
                        <td class="crmcell"></td>
                    </tr>
                    <tr>
                        <td class="crmcell right width-200">Giảm giá vật liệu</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtGIAMGIACPVL" runat="server" Text="0" MaxLength="6" Width="40px" /> (%)
                            </div>
                        </td>
                        <td class="crmcell" colspan="2"></td>
                    </tr>
                    <tr>
                        <td class="crmcell right">Chi phí vật liệu</td>
                        <td class="crmcell">
                           
                        </td>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:Label ID="lblChiPhiVatLieu" runat="server" Text=""></asp:Label>
                            </div>                            
                        </td>
                        <td class="crmcell">
                            <div class="left"><strong>(VL)</strong></div>
                        </td>
                    </tr>
                    
                    <tr>
                        <td class="crmcell right"></td>
                        <td class="crmcell">
                           
                        </td>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:Label ID="lblChiPhiKhachHang" runat="server" Text=""></asp:Label>
                            </div>                            
                        </td>
                        <td class="crmcell">
                            <div class="left"><strong>(A)</strong></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">Chi phí nhân công</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtHSNHANCONG" runat="server" Text="2.0860" MaxLength="6" Width="60px" />
                                *
                                <asp:TextBox ID="txtHSTHIETKE3" runat="server" Text="1.00" MaxLength="6" Width="60px" />
                                * (B)
                            </div>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:Label ID="lblChiPhiNhanCong" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell">
                            <div class="left"><strong>(NC)</strong></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">Trực tiếp phí khác</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtHSCPC" runat="server" Text="0.02" MaxLength="6" Width="60px" />
                                * (VL + NC)
                            </div>
                        </td>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:Label ID="lblCPC" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell">
                            <div class="left"><strong>(TT)</strong></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">Cộng chi phí trực tiếp</td>
                        <td class="crmcell">
                            <div class="center">(VL + NC + TT)</div>
                        </td>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:Label ID="lblCongChiPhiTrucTiep" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell">    
                            <div class="left"><strong>(T)</strong></div>
                        </td>
                    </tr>  
                    <tr>
                        <td class="crmcell right">Chi phí chung</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtHSCHUNG" runat="server" Text="0.05" MaxLength="6" Width="60px" />
                                * (T)
                            </div>
                        </td>
                        <td class="crmcell">  
                            <div class="tright">
                                <asp:Label ID="lblCPCHUNG" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell"> 
                            <div class="left"><strong>(C)</strong></div>
                        </td>
                    </tr>  
                    <tr>
                        <td class="crmcell right">Thu nhập chịu thuế tính trước</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtHSTHUNHAP" runat="server" Text="0.055" MaxLength="6" Width="60px" />
                                * (T + C)
                            </div>
                        </td>
                        <td class="crmcell">  
                            <div class="tright">
                                <asp:Label ID="lblCPTHUNHAP" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell"> 
                            <div class="left"><strong>(TL)</strong></div>
                        </td>
                    </tr> 
                    <tr>
                        <td class="crmcell right">Thuế giá trị gia tăng đầu ra</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtThue" runat="server" Text="10" MaxLength="6" Width="60px" />
                                * (T + C + TL)
                            </div>
                        </td>
                        <td class="crmcell">  
                            <div class="tright">                                
                                <asp:Label ID="lblThue" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell"> 
                            <div class="left"><strong>(VAT)</strong></div>
                        </td>
                    </tr>  
                    <tr>
                        <td class="crmcell right">Khảo sát phí</td>
                        <td class="crmcell">
                            <div class="center">
                                <asp:TextBox ID="txtHSTHIETKE2" runat="server" Text="1.1" MaxLength="6" Width="60px" />
                                * 
                                <asp:TextBox ID="txtHSTHIETKE1" runat="server" Text="0.032" MaxLength="6" Width="60px" />
                                * (T + C + TL)
                            </div>
                        </td>
                        <td class="crmcell">                             
                            <div class="tright">
                                <asp:Label ID="lblCPTHIETKE" runat="server" Text=""></asp:Label>
                            </div>
                        </td>
                        <td class="crmcell">    
                            <div class="left"><strong>(KS)</strong></div>                            
                        </td>
                    </tr>  
                    <tr>
                        <td class="crmcell right">Chi phí khác</td>
                        <td class="crmcell"></td>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:TextBox ID="txtChiPhiKhac" runat="server" Text="0" MaxLength="6" Width="50px" />
                            </div>
                        </td>
                        <td class="crmcell"> 
                            <div class="left"><strong>(F)</strong></div>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">Giá trị chiết tính làm tròn</td>
                        <td class="crmcell">
                            <div class="center">(T + C + TL + VAT + KS + F)
                            </div>
                        </td>
                        <td class="crmcell">
                            <div class="tright">
                                <asp:Label ID="lblTONGST" runat="server" Text="" />
                            </div>
                        </td>
                        <td class="crmcell"></td>
                    </tr>
                </table>
                <table class="crmtable">
                    <tr>
                        <td class="crmcell right">
                            <asp:Button ID="btnBaoCao" CssClass="report" runat="server" UseSubmitBehavior="false"
                                     OnClientClick="return CheckFormBaoCao();" OnClick="btnBaoCao_Click" />
                        </td>
                        <td class="crmcell">
                            <div class="left">
                                
                            </div>
                        </td>
                    </tr>
                </table>
            </div>            
        </ContentTemplate>
    </asp:UpdatePanel>
    
    <br />    
	<div class="crmcontainer p-5">
        <a href="DuyetChietTinh.aspx">Chuyển sang bước kế tiếp</a>
    </div>
</asp:Content>
