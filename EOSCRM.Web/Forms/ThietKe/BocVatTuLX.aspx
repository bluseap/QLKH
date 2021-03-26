<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="BocVatTuLX.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.BocVatTuLX" %>

<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
     namespace="CrystalDecisions.Web" tagprefix="CR" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util"%>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Import Namespace="EOSCRM.Web.Common"%>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divVatTu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divVatTuDlgContainer");
                }
            });

            $("#divVatTuKHTT").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divVatTuKHTTDlgContainer");
                }
            });

            $("#divVatTuKHTTSAUDH").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divVatTuKHTTSAUDHDlgContainer");
                }
            });            
            
        });

        function file_change2(f) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = document.getElementById("imgTK2");
                img.src = e.target.result;
                img.style.display = "inline";
            };
            reader.readAsDataURL(f.files[0]);
        }

        function file_change(f) {
            var reader = new FileReader();
            reader.onload = function (e) {
                var img = document.getElementById("imgTK1");
                img.src = e.target.result;
                img.style.display = "inline";
            };
            reader.readAsDataURL(f.files[0]);
        }

        function CheckFormMAVTKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeMAVT) %>', '');
            }
        }
        
        function CheckFormMAVTKHTTKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkChangeMAVTKHTT) %>', '');
            }
        }

        function CheckFormMAVTKHTTSAUDHKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkChangeMAVTKHTTSAUDH) %>', '');
            }
        }       

        function CheckFormKhoiLuongKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeKhoiLuong) %>', '');
            }
        }

        function CheckFormKhoiLuongKHTTKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkChangeKhoiLuongKHTT) %>', '');
            }
        }
        
        function CheckFormKhoiLuongKHTTSAUDHKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkChangeKhoiLuongKHTTSAUDH) %>', '');
            }
        }

        function CheckFormFilterVatTu() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTu) %>', '');
            return;
        }
        
        function CheckFormFilterVatTuKHTT() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTuKHTT) %>', '');
            return;
        }
        
        function CheckFormFilterVatTuKHTTSAUDH() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTuKHTTSAUDH) %>', '');
            return;
        }

        function checkChange() {
            if (confirm('Tất cả dữ liệu có sẵn sẽ bị thay thế bởi mẫu bốc vật tư. Đổi?')) {
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnChange) %>', '');
		    }
        }

        function CheckFormBaoCao() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

        function updateCTTK(maMBVT, maVT, txtSLId, txtGIAVTId, lblTIENVT, txtGIANCId, lblTIENNC, cbISCTYDTU) {
            openWaitingDialog();
            unblockWaitingDialog();

            var sl = document.getElementById(txtSLId).value;
            var gvt = document.getElementById(txtGIAVTId).value;
            var gnc = document.getElementById(txtGIANCId).value;
            var cb = document.getElementById(cbISCTYDTU).checked ? "true" : "false";

            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCTTK(maMBVT, maVT, sl, gvt, gnc, cb);

            if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {

		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");

		        if (idArr.length == 4) {
		            document.getElementById(txtSLId).value = idArr[0];
		            document.getElementById(txtGIAVTId).value = idArr[1];
		            document.getElementById(txtGIANCId).value = idArr[2];
		            if (idArr[3] == "true")
		                document.getElementById(cbISCTYDTU).checked = true;
		            else {
		                document.getElementById(cbISCTYDTU).checked = false;
		            }
		        }
		    }

            if (msg.value == "<%= DELIMITER.Passed %>") {
		        var sl1 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gvt);
		        var sl2 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gnc);
		        document.getElementById(lblTIENVT).innerHTML = sl1;
		        document.getElementById(lblTIENNC).innerHTML = sl2;
            }

            CheckFormTINHTIENTK;
		    closeWaitingDialog();
		}

        function updateCTTKKHTT(maMBVT, maVT, txtSLId, txtGIAVTId, lblTIENVT, txtGIANCId, lblTIENNC, cbISCTYDTUKHTT) {
            openWaitingDialog();
            unblockWaitingDialog();

            var sl = document.getElementById(txtSLId).value;
            var gvt = document.getElementById(txtGIAVTId).value;
            var gnc = document.getElementById(txtGIANCId).value;
            var cb = document.getElementById(cbISCTYDTUKHTT).checked ? "true" : "false";

            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCTTK(maMBVT, maVT, sl, gvt, gnc, cb);

            if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {

                var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");

                if (idArr.length == 4) {
                    document.getElementById(txtSLId).value = idArr[0];
                    document.getElementById(txtGIAVTId).value = idArr[1];
                    document.getElementById(txtGIANCId).value = idArr[2];
                    if (idArr[3] == "true")
                        document.getElementById(cbISCTYDTUKHTT).checked = true;
                    else {
                        document.getElementById(cbISCTYDTUKHTT).checked = false;
                    }
                }
            }

            if (msg.value == "<%= DELIMITER.Passed %>") {
                var sl1 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gvt);
                var sl2 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gnc);
                document.getElementById(lblTIENVT).innerHTML = sl1;
                document.getElementById(lblTIENNC).innerHTML = sl2;
            }

            CheckFormTINHTIENTK;
            closeWaitingDialog();
        }

        function updateCTTKKHTTSAUDH(maMBVT, maVT, txtSLId, txtGIAVTId, lblTIENVT, txtGIANCId, lblTIENNC, cbISCTYDTUKHTTSAUDH) {
            openWaitingDialog();
            unblockWaitingDialog();

            var sl = document.getElementById(txtSLId).value;
            var gvt = document.getElementById(txtGIAVTId).value;
            var gnc = document.getElementById(txtGIANCId).value;
            var cb = document.getElementById(cbISCTYDTUKHTTSAUDH).checked ? "true" : "false";


            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCTTK(maMBVT, maVT, sl, gvt, gnc, cb);

            if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {

                var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");

                if (idArr.length == 4) {
                    document.getElementById(txtSLId).value = idArr[0];
                    document.getElementById(txtGIAVTId).value = idArr[1];
                    document.getElementById(txtGIANCId).value = idArr[2];
                    if (idArr[3] == "true")
                        document.getElementById(cbISCTYDTUKHTTSAUDH).checked = true;
                    else {
                        document.getElementById(cbISCTYDTUKHTTSAUDH).checked = false;
                    }
                }
            }

            if (msg.value == "<%= DELIMITER.Passed %>") {
                var sl1 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gvt);
                var sl2 = parseFloat(sl.replace(/,/g, '.')) * parseFloat(gnc);
                document.getElementById(lblTIENVT).innerHTML = sl1;
                document.getElementById(lblTIENNC).innerHTML = sl2;
            }

            CheckFormTINHTIENTK;
            closeWaitingDialog();
        }

		function updateGCTK(maGC, clientId) {
		    openWaitingDialog();
		    unblockWaitingDialog();

		    var noidung = document.getElementById(clientId).value;
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateGCTK(maGC, noidung);

		    if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		        document.getElementById(clientId).value = msg.value;
		    }

		    closeWaitingDialog();
        }

        function updateCPTK(maCP, txtNDclientId, txtDGclientId, ddlDVTclientId, txtSLclientId, //txtHSclientId
                txtCHIEURONGclientId, txtCHIEUCAOclientId, lblTTclientId, ddlLCPclientId) {
            openWaitingDialog();
            unblockWaitingDialog();

            var nd = document.getElementById(txtNDclientId).value;
            var dg = document.getElementById(txtDGclientId).value;

            var e = document.getElementById(ddlDVTclientId);
            var dvt = e.options[e.selectedIndex].value;

            var sl = document.getElementById(txtSLclientId).value;
            //var hs = document.getElementById(txtHSclientId).value;
            var chieurong = document.getElementById(txtCHIEURONGclientId).value;
            var chieucao = document.getElementById(txtCHIEUCAOclientId).value;

            var f = document.getElementById(ddlLCPclientId);
            var loai = f.options[f.selectedIndex].value;

            //var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPTK(maCP, nd, dg, dvt, sl, hs, loai);
            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPTKChieuCaoRong(maCP, nd, dg, dvt, sl, chieurong, chieucao, loai);

            if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {

		        var idArr = msg.value.split("<%= DELIMITER.Delimiter %>");

		        if (idArr.length == 7) {
		            document.getElementById(txtNDclientId).value = idArr[0];
		            document.getElementById(txtDGclientId).value = idArr[1];
		            document.getElementById(ddlDVTclientId).value = idArr[2];
		            document.getElementById(txtSLclientId).value = idArr[3];
		            //document.getElementById(txtHSclientId).value = idArr[4];
		            document.getElementById(txtCHIEURONGclientId).value = idArr[4];
		            document.getElementById(txtCHIEUCAOclientId).value = idArr[5];
		            document.getElementById(ddlLCPclientId).value = idArr[6];
		        }
		    }
            
            if (msg.value == "<%= DELIMITER.Passed %>") {
                var tongtien = parseFloat(sl.replace(/,/g, '.')) * parseFloat(chieurong.replace(/,/g, '.')) * parseFloat(chieucao.replace(/,/g, '.')) * dg;

                document.getElementById(lblTTclientId).innerHTML = parseInt(tongtien);
		            //parseFloat(sl.replace(/,/g, '.')) * parseFloat(chieurong.replace(/,/g, '.')) * parseFloat(chieucao.replace(/,/g, '.')) * dg;
		    }

            CheckFormTINHTIENTK;
		    closeWaitingDialog();
        }

        function CheckFormAddChiPhiDLVC() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btAddChiPhiDLVC) %>', '');
        }
        
        function CheckFormAddChiPhiLap() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btAddChiPhiLap) %>', '');
        }

        function CheckFormAddChiPhiVanChuyen() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btAddChiPhiVanChuyen) %>', '');
        }        
        
        function CheckFormTINHTIENTK() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btTINHTIENTK) %>', '');
        }       

    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divVatTuDlgContainer">	
		<div id="divVatTu" style="display:none">		    
	        <asp:UpdatePanel ID="upnlVatTu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table cellpadding="3" cellspacing="1" style="width: 600px;">
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
						                OnRowDataBound="gvVatTu_RowDataBound" OnRowCommand="gvVatTu_RowCommand" 
						                OnPageIndexChanging="gvVatTu_PageIndexChanging">
							            <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MAVT") %>'
                                                        CommandName="EditItem" Text='<%# Eval("KYHIEUVT") != null ? Eval("KYHIEUVT").ToString() : "" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Mã hiệu" DataField="MAHIEU" HeaderStyle-Width="15%" />
                                            <asp:BoundField HeaderText="Tên vật tư" DataField="TENVT" HeaderStyle-Width="35%" />
                                            <asp:BoundField HeaderText="Giá VT" DataField="GIAVT" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Giá NC" DataField="GIANC" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Đơn vị tính" DataField="DVT" HeaderStyle-Width="15%" />
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
    <div id="divVatTuKHTTDlgContainer">	
		<div id="divVatTuKHTT" style="display:none">		    
	        <asp:UpdatePanel ID="upVatTuKHTT" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table cellpadding="3" cellspacing="1" style="width: 600px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">                                                
                                                    <asp:TextBox ID="txtFilterVatTuKHTT" onchange="return CheckFormFilterVatTuKHTT();" runat="server" />
                                                </div>
                                                <div class="left">  
                                                    <asp:Button ID="btnFilterVatTuKHTT" OnClientClick="return CheckFormFilterVatTuKHTT();" 
                                                        runat="server" CssClass="filter" UseSubmitBehavior="false" OnClick="btnFilterVatTuKHTT_Click" />
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
                                    <eoscrm:Grid ID="gvVatTuKHTT" runat="server" UseCustomPager="true" 
						                OnRowDataBound="gvVatTuKHTT_RowDataBound" OnRowCommand="gvVatTuKHTT_RowCommand" 
						                OnPageIndexChanging="gvVatTuKHTT_PageIndexChanging">
							            <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnIDKHTT" runat="server" CommandArgument='<%# Eval("MAVT") %>'
                                                        CommandName="EditItem" Text='<%# Eval("KYHIEUVT") != null ? Eval("KYHIEUVT").ToString() : "" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Mã hiệu" DataField="MAHIEU" HeaderStyle-Width="15%" />
                                            <asp:BoundField HeaderText="Tên vật tư" DataField="TENVT" HeaderStyle-Width="35%" />
                                            <asp:BoundField HeaderText="Giá VT" DataField="GIAVT" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Giá NC" DataField="GIANC" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Đơn vị tính" DataField="DVT" HeaderStyle-Width="15%" />
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
    <div id="divVatTuKHTTSAUDHDlgContainer">	
		<div id="divVatTuKHTTSAUDH" style="display:none">		    
	        <asp:UpdatePanel ID="upVatTuKHTTSAUDH" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table cellpadding="3" cellspacing="1" style="width: 600px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">                                                
                                                    <asp:TextBox ID="txtFilterVatTuKHTTSAUDH" onchange="return CheckFormFilterVatTuKHTTSAUDH();" runat="server" />
                                                </div>
                                                <div class="left">  
                                                    <asp:Button ID="btnFilterVatTuKHTTSAUDH" OnClientClick="return CheckFormFilterVatTuKHTTSAUDH();" 
                                                        runat="server" CssClass="filter" UseSubmitBehavior="false" OnClick="btnFilterVatTuKHTTSAUDH_Click" />
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
                                    <eoscrm:Grid ID="gvVatTuKHTTSAUDH" runat="server" UseCustomPager="true" 
						                OnRowDataBound="gvVatTuKHTTSAUDH_RowDataBound" OnRowCommand="gvVatTuKHTTSAUDH_RowCommand" 
						                OnPageIndexChanging="gvVatTuKHTTSAUDH_PageIndexChanging">
							            <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Mã VT" HeaderStyle-Width="15%">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnIDKHTTSAUDH" runat="server" CommandArgument='<%# Eval("MAVT") %>'
                                                        CommandName="EditItem" Text='<%# Eval("KYHIEUVT") != null ? Eval("KYHIEUVT").ToString() : "" %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderText="Mã hiệu" DataField="MAHIEU" HeaderStyle-Width="15%" />
                                            <asp:BoundField HeaderText="Tên vật tư" DataField="TENVT" HeaderStyle-Width="35%" />
                                            <asp:BoundField HeaderText="Giá VT" DataField="GIAVT" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Giá NC" DataField="GIANC" HeaderStyle-Width="10%" />
                                            <asp:BoundField HeaderText="Đơn vị tính" DataField="DVT" HeaderStyle-Width="15%" />
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

    <asp:UpdatePanel ID="upTHONGTINTK" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
        <table class="crmtable">
            <tbody>
                <tr>
                    <td class="crmcell right">Mã đơn thiết kế</td>
                    <td class="crmcell">
                        <div class="left">
                            <%= ThietKe != null ? ThietKe.MADDK : "" %>
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Tên khách hàng</td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Label ID="lbTENKH" runat="server" Text="Label"></asp:Label>
                        </div>
                        
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Tên thiết kế
                    </td>
                    <td class="crmcell">
                        <div class="left">
                            <%= ThietKe != null ? ThietKe.TENTK : "" %>
                        </div>
                    </td>
                </tr>                
                <tr>
                    <td class="crmcell right">Chú thích</td>
                    <td class="crmcell">
                        <div class="left">
                            <%= ThietKe != null ? ThietKe.CHUTHICH: "" %>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td class="crmcell right">Hình thiết kế 1</td>
                    <td class="crmcell">                                 
                            <asp:FileUpload ID="UpHINH" runat="server" type="file" onchange="file_change(this)" Width="70px" />
                            <input id="f" type="file" onchange="file_change(this)" style="display: none" />
                            <input type="button" onclick="document.getElementById('f').click()" />
                        <div class="left">                                   
                            <img id="imgTK1" style="display: none" Height="150px" Width="120px"/>
                            <asp:Image ID="imgHINHTK1" runat="server" Height="150px" Width="120px"/>
                        </div> 
                        <td class="crmcell right">Hình thiết kế 2</td>
                        <td class="crmcell">                                 
                                <asp:FileUpload ID="UpHINH2" runat="server" type="file" onchange="file_change2(this)" Width="70px" />
                                <input id="f2" type="file" onchange="file_change2(this)" style="display: none" />
                                <input type="button" onclick="document.getElementById('f2').click()" />
                            <div class="left">                                   
                                <img id="imgTK2" style="display: none" Height="150px" Width="120px"/>
                                <asp:Image ID="imgHINHTK2" runat="server" Height="150px" Width="120px"/>                         
                            </div>                                    
                        </td>                                
                    </td>
                </tr>               
                <tr>
                    <td class="crmcell right"></td>
                    <td class="crmcell">
                        <div class="left">
                            <asp:Button ID="btnSave" runat="server" CommandArgument="Change" CssClass="save"
                                OnClick="btnSave_Click" TabIndex="16" UseSubmitBehavior="false" />
                        </div>
                    </td>
                </tr>
            </tbody>
        </table>
    </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnSave" />            
        </Triggers> 
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upMAUBVTLX" UpdateMode="Conditional" runat="server">
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
                                    <span style="color: Red; font-weight: bold">Chú ý: Thay đổi mẫu bốc vật tư sẽ xóa hết danh sách vật tư, ghi chú, và chi phí đào lấp hiện tại.</span>
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
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlMBVT" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <table class="crmcontainer">
                <tr>
                    <td class="header">Xí nghiệp chịu chi phí - 5m trước đồng hồ</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right">Mã vật tư</td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtMAVT" onkeypress="return CheckFormMAVTKeyPress(event);"   
		                                    Width="60px" runat="server" ReadOnly="True" />
		                                <asp:LinkButton ID="linkBtnChangeMAVT" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeMAVT_Click" runat="server">Change MAVT</asp:LinkButton>
		                                    
		                            </div>
		                            <div class="left">
		                                <asp:Button ID="btnBrowseVatTu" runat="server" CssClass="pickup" 
		                                    OnClick="btnBrowseVatTu_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 600, 'divVatTu')"  />
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lbSOLUONG" runat="server" Text="Số lượng" Font-Bold="True"></asp:Label>
		                                <asp:TextBox ID="txtKHOILUONG" Width="60px" runat="server"
		                                    onkeypress="return CheckFormKhoiLuongKeyPress(event);"  />
		                                <asp:LinkButton ID="linkBtnChangeKhoiLuong" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeKhoiLuong_Click" runat="server">Change KL</asp:LinkButton>
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lblTENVT" runat="server" Text=""></asp:Label>
		                            </div>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:CheckBox ID="chkIsCtyDauTu" runat="server" />
		                            </div>
		                            <div class="left"><strong>Công ty đầu tư</strong></div>
		                        </td>
		                    </tr>                            
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
                            <eoscrm:Grid ID="gvSelectedVatTu" runat="server" UseCustomPager="true" PageSize="200" 
                                FooterStyle-HorizontalAlign="Center" 
                                FooterStyle-Font-Bold="true" 
                                FooterStyle-ForeColor="#555555"
                                ShowFooter="true" 
                                OnRowCommand="gvSelectedVatTu_RowCommand" 
                                OnRowDataBound="gvSelectedVatTu_RowDataBound">
                                <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                <footerstyle backcolor="LightCyan"  forecolor="MediumBlue"/>
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.STT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ký hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.KYHIEUVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>                                    
                                    <asp:TemplateField HeaderText="Nội dung công việc">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.TENVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Công ty đầu tư" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbISCTYDTU" runat="server" />                                           
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
                                        <ItemStyle HorizontalAlign="Right" Width="12%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENVT" Text='<%# Bind("TIENVT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>                                    
                                        <FooterTemplate>                                            
                                            <asp:Label ID="lblPageTotal" runat="server"/>                                            
                                            <asp:Label ID="lblGrandTotal" runat="server"/>
                                        </FooterTemplate>                                       
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
            </table>            
		</ContentTemplate>
	</asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upBVTKHTHANHTOAN" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <table class="crmcontainer">
                <tr>
                    <td class="header">Khách hàng thanh toán - Phần vượt 5m trước đồng hồ</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right">Mã vật tư</td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtMAVTKHTT" onkeypress="return CheckFormMAVTKHTTKeyPress(event);" ReadOnly="True" 
		                                    Width="60px" runat="server" />
		                                <asp:LinkButton ID="lkChangeMAVTKHTT" CausesValidation="false" style="display:none"  
                                            OnClick="lkChangeMAVTKHTT_Click" runat="server">Change MAVT</asp:LinkButton>		                                    
		                            </div>
		                            <div class="left">
		                                <asp:Button ID="btVATTUKHTT" runat="server" CssClass="pickup" 
		                                    OnClick="btVATTUKHTT_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 600, 'divVatTuKHTT')"  />
		                            </div>
		                            <div class="left"><asp:Label ID="lbSOLUONGKHTT" runat="server" Text="Số lượng" Font-Bold="True"></asp:Label>
		                                <asp:TextBox ID="txtKHOILUONGKHTT" Width="60px" runat="server"
		                                    onkeypress="return CheckFormKhoiLuongKHTTKeyPress(event);" />
		                                <asp:LinkButton ID="lkChangeKhoiLuongKHTT" CausesValidation="false" style="display:none"  
                                            OnClick="lkChangeKhoiLuongKHTT_Click" runat="server">Change KL</asp:LinkButton>
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lblTENVTKHTT" runat="server" Text=""></asp:Label>
		                            </div>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:CheckBox ID="ckCONGTYDTKHTT" runat="server" />
		                            </div>
		                            <div class="left"><strong>Công ty đầu tư</strong></div>
		                        </td>
		                    </tr>                            
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
                            <eoscrm:Grid ID="gvSelectedVatTuKHTT" runat="server" UseCustomPager="true" PageSize="200" 
                                FooterStyle-HorizontalAlign="Center" 
                                FooterStyle-Font-Bold="true" 
                                FooterStyle-ForeColor="#555555"
                                ShowFooter="true" 
                                OnRowCommand="gvSelectedVatTuKHTT_RowCommand" 
                                OnRowDataBound="gvSelectedVatTuKHTT_RowDataBound">
                                <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                <footerstyle backcolor="LightCyan"  forecolor="MediumBlue"/>
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.STT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ký hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.KYHIEUVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Nội dung công việc">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.TENVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Công ty đầu tư" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="gvcbISCTYDTUKHTT" runat="server" />                                           
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
                                        <ItemStyle HorizontalAlign="Right" Width="12%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENVT" Text='<%# Bind("TIENVT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>                                    
                                        <FooterTemplate>                                            
                                            <asp:Label ID="lblPageTotal" runat="server"/>                                            
                                            <asp:Label ID="lblGrandTotal" runat="server"/>
                                        </FooterTemplate>                                       
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
            </table>            
		</ContentTemplate>
	</asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upCPDAOLAPVC" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <table class="crmcontainer">
                <tr>
                    <td class="header">Khách hàng thanh toán - Chi phí đào, lắp, vận chuyển</td> 
		        </tr>
                <tr>
		            <td>
		               <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:Button ID="btAddChiPhiDLVC" runat="server" CssClass="myButton" OnClientClick="return CheckFormAddChiPhiDLVC();" 
		                                    OnClick="btAddChiPhiDLVC_Click" CausesValidation="false" UseSubmitBehavior="false" Text ="CP Đào"/>
		                            </div>
                                    <div class="left">
		                                <asp:Button ID="btAddChiPhiLap" runat="server" CssClass="myButton" OnClientClick="return CheckFormAddChiPhiLap();" 
		                                    OnClick="btAddChiPhiLap_Click" CausesValidation="false" UseSubmitBehavior="false" Text ="CP Lắp"/>
		                            </div>
                                    <div class="left">
		                                <asp:Button ID="btAddChiPhiVanChuyen" runat="server" CssClass="myButton" OnClientClick="return CheckFormAddChiPhiVanChuyen();" 
		                                    OnClick="btAddChiPhiVanChuyen_Click" CausesValidation="false" UseSubmitBehavior="false" Text ="CP Vận chuyển"/>
		                            </div>
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important; border-bottom: none !important;">
		                    <eoscrm:Grid ID="gvChiPhiDLVC" runat="server" UseCustomPager="true" PageSize="2000"
			                    OnRowCommand="gvChiPhiDLVC_RowCommand" OnRowDataBound="gvChiPhiDLVC_RowDataBound" >
			                    <PagerSettings FirstPageText="chi phí" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Nội dung">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNOIDUNG" Text='<%# Bind("NOIDUNG") %>' Width="50%" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn giá" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                           <asp:TextBox ID="txtDONGIA" Text='<%# Bind("DONGIACP") %>' Width="60px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Đơn vị" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlDVT" runat="server" DataSource="<%# new DvtDao().GetList() %>" 
                                                DataTextField="TENDVT" DataValueField="DVT1" Width="60px" SelectedValue='<%# Bind("DVT") %>'>
                                            </asp:DropDownList>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Khối lượng" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtSOLUONG" Text='<%# Bind("SOLUONG") %>' Width="50px" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chiều rộng" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCHIEURONG" Text='<%# Bind("CHIEURONG") %>' Width="50px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Chiều cao" HeaderStyle-Width="40px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtCHIEUCAO" Text='<%# Bind("CHIEUCAO") %>' Width="50px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Hệ số" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtHESOCP" Text='<%# Bind("HESOCP") %>' Width="50px" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
                                    <asp:TemplateField HeaderText="Thành tiền" HeaderStyle-Width="90px">
                                        <ItemTemplate>
                                            <asp:Label ID="lblTHANHTIENCP" Text='<%# Bind("THANHTIENCP") %>' runat="server"></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Loại chi phí" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlLOAICP" runat="server" SelectedValue='<%# Bind("LOAICP") %>'
                                                ToolTip='<%# Bind("LOAICP") %>' Width="98%">                                                
                                                <asp:ListItem Value="DAO">Đào</asp:ListItem>
                                                <asp:ListItem Value="LAP">Lắp</asp:ListItem>
                                                <asp:ListItem Value="VC">Vận chuyển</asp:ListItem>
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
    <br />
    <asp:UpdatePanel ID="upBVTKHTHANHTOANSAUDH" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <table class="crmcontainer">
                <tr>
                    <td class="header">Khách hàng thanh toán - Sau đồng hồ</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right">Mã vật tư</td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtMAVTKHTTSAUDH" onkeypress="return CheckFormMAVTKHTTSAUDHKeyPress(event);" ReadOnly="True"
		                                    Width="60px" runat="server" />
		                                <asp:LinkButton ID="lkChangeMAVTKHTTSAUDH" CausesValidation="false" style="display:none"  
                                            OnClick="lkChangeMAVTKHTTSAUDH_Click" runat="server">Change MAVT</asp:LinkButton>		                                    
		                            </div>
		                            <div class="left">
		                                <asp:Button ID="btVATTUKHTTSAUDH" runat="server" CssClass="pickup" 
		                                    OnClick="btVATTUKHTTSAUDH_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 600, 'divVatTuKHTTSAUDH')"  />
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lbSOLUONGKHTTSAUDH" runat="server" Text="Số lượng" Font-Bold="True"></asp:Label>
		                                <asp:TextBox ID="txtKHOILUONGKHTTSAUDH" Width="60px" runat="server"
		                                    onkeypress="return CheckFormKhoiLuongKHTTSAUDHKeyPress(event);" />
		                                <asp:LinkButton ID="lkChangeKhoiLuongKHTTSAUDH" CausesValidation="false" style="display:none"  
                                            OnClick="lkChangeKhoiLuongKHTTSAUDH_Click" runat="server">Change KL</asp:LinkButton>
		                            </div>
		                            <div class="left">
                                        <asp:Label ID="lblTENVTKHTTSAUDH" runat="server" Text=""></asp:Label>
		                            </div>
		                        </td>
		                    </tr>
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:CheckBox ID="ckCONGTYDTKHTTSAUDH" runat="server" />
		                            </div>
		                            <div class="left"><strong>Công ty đầu tư</strong></div>
		                        </td>
		                    </tr>                            
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
                            <eoscrm:Grid ID="gvSelectedVatTuKHTTSAUDH" runat="server" UseCustomPager="true" PageSize="200" 
                                FooterStyle-HorizontalAlign="Center" 
                                FooterStyle-Font-Bold="true" 
                                FooterStyle-ForeColor="#555555"
                                ShowFooter="true" 
                                OnRowCommand="gvSelectedVatTuKHTTSAUDH_RowCommand" 
                                OnRowDataBound="gvSelectedVatTuKHTTSAUDH_RowDataBound">
                                <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
                                <footerstyle backcolor="LightCyan"  forecolor="MediumBlue"/>
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.STT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ký hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.KYHIEUVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField> 
                                    <asp:TemplateField HeaderText="Nội dung công việc">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.TENVT") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Công ty đầu tư" HeaderStyle-Width="100px">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbISCTYDTUKHTTSAUDH" runat="server" />                                           
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
                                        <ItemStyle HorizontalAlign="Right" Width="12%"></ItemStyle>
                                        <ItemTemplate>
                                            <asp:Label ID="lblTIENVT" Text='<%# Bind("TIENVT") %>' runat="server"></asp:Label>
                                        </ItemTemplate>                                    
                                        <FooterTemplate>                                            
                                            <asp:Label ID="lblPageTotal" runat="server"/>                                            
                                            <asp:Label ID="lblGrandTotal" runat="server"/>
                                        </FooterTemplate>                                       
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
            </table>            
		</ContentTemplate>
	</asp:UpdatePanel>
    <br />
     <asp:UpdatePanel ID="upTONGTIENTK" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <table class="crmcontainer">
                <tr>
		            <td class="crmcell right"></td>
		            <td class="crmcell">
		                <div class="left">
		                    <asp:Button ID="btTINHTIENTK" runat="server" CssClass="myButton" OnClientClick="return CheckFormTINHTIENTK();" 
		                        OnClick="btTINHTIENTK_Click" CausesValidation="false" UseSubmitBehavior="false" Text ="Tính tiền"/>	   
		            </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTHCPNCT1" runat="server" Text="Chi phí nhân công"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTHCPNCT2" runat="server" Text ="T" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTHCPNCT3" runat="server" Text="CPNC = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTHCPNCT4" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTHCPCHUNGC1" runat="server" Text="Chi phí chung"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTHCPCHUNGC2" runat="server" Text ="C" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <div class="right">
                                <asp:Label ID="lbTHCPCHUNGC3" runat="server" Text="T x "></asp:Label>                            
                                <asp:TextBox ID="txtChiPhiChungGCTKLX" Width="30px" runat="server"  />
                                <asp:Label ID="lbTHCPCHUNGC3Bang" runat="server" Text=" = "></asp:Label>
                             </div>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTHCPCHUNGC4" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTHTNCTTTTL1" runat="server" Text="Thu nhập chịu thuế tính trước"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTHTNCTTTTL2" runat="server" Text ="TL" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTHTNCTTTTL3" runat="server" Text="(T + C) x "></asp:Label>
                            <asp:TextBox ID="txtThuNhapChiuThueTinhTruoc55" Width="30px" runat="server" />
                            <asp:Label ID="lbTHTNCTTTTL3Bang" runat="server" Text=" = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTHTNCTTTTL4" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbGTXLTTG1" runat="server" Text="Giá trị xây lắp trước thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbGTXLTTG2" runat="server" Text ="G" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbGTXLTTG3" runat="server" Text="T + C + TL = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbGTXLTTG4" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTGTGTXLVAT11" runat="server" Text="Thuế GTGT xây lắp"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTGTGTXLVAT12" runat="server" Text ="VAT1" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTGTGTXLVAT13" runat="server" Text="G x "></asp:Label>
                            <asp:TextBox ID="txtTGTGTXLVAT1310" Width="30px" runat="server" />
                            <asp:Label ID="lbTGTGTXLVAT1310Bang" runat="server" Text=" = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTGTGTXLVAT14" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPXLSTG11" runat="server" Text="Chi phí xây lắp sau thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPXLSTG12" runat="server" Text ="G1" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPXLSTG13" runat="server" Text="G x VAT1 = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPXLSTG14" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="#FF3300" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPTKTTTK1" runat="server" Text="Chi phí thiết kế trước thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPTKTTTK2" runat="server" Text ="TK" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPTKTTTK3" runat="server" Text="G x "></asp:Label>
                            <asp:TextBox ID="txtCPTKTTTK3207" Width="30px" runat="server" />
                            <asp:Label ID="lbtxtCPTKTTTK3207x" runat="server" Text=" x "></asp:Label>
                            <asp:TextBox ID="txtCPTKTTTK313" Width="30px" runat="server" />
                            <asp:Label ID="txtCPTKTTTK3207Bang" runat="server" Text=" = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPTKTTTK4" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTGTGTTTVAT21" runat="server" Text="Thuế GTGT thiết kế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTGTGTTTVAT22" runat="server" Text ="VAT2" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTGTGTTTVAT23" runat="server" Text="TK x "></asp:Label>
                            <asp:TextBox ID="txtTGTGTTTVAT2310" Width="30px" runat="server" />
                            <asp:Label ID="lbtxtTGTGTTTVAT2310" runat="server" Text=" = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTGTGTTTVAT24" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPTKSTG21" runat="server" Text="Chi phí thiết kế sau thế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPTKSTG22" runat="server" Text ="G2" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPTKSTG23" runat="server" Text="TK + VAT2 = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPTKSTG24" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Red" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPVTTTVT1" runat="server" Text="Chi phí vật tư trước thế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPVTTTVT2" runat="server" Text ="VT" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPVTTTVT13" runat="server" Text="CPVT = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPVTTTVT14" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTGTGTVT1" runat="server" Text="Thuế GTGT vật tư"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTGTGTVT12" runat="server" Text ="VAT3" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTGTGTVT13" runat="server" Text="VT x "></asp:Label>
                            <asp:TextBox ID="txtTGTGTVT1310" Width="30px" runat="server" />
                            <asp:Label ID="lbTGTGTVT1310" runat="server" Text=" = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTGTGTVT14" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPVTSTG31" runat="server" Text="Chi phí vật tư sau thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPVTSTG312" runat="server" Text ="G3" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPVTSTG313" runat="server" Text="VT + VAT3 = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPVTSTG314" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="#FF3300" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPVCTTVAT11" runat="server" Text="Chi phí vận chuyển trước thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPVCTTVAT12" runat="server" Text ="VC" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPVCTTVAT13" runat="server" Text="CPVC = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPVCTTVAT14" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTGTGTVC1" runat="server" Text="Thuế GTGT vận chuyển"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTGTGTVC2" runat="server" Text ="VAT4" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTGTGTVC3" runat="server" Text="VC x 10% = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTGTGTVC4" runat="server" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbCPVCSTG41" runat="server" Text="Chi phí vận chuyển sau thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbCPVCSTG42" runat="server" Text ="G4" Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbCPVCSTG43" runat="server" Text="VC + VAT4 = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbCPVCSTG44" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Red" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <tr>
                    <td class="crmcell right">
                        <asp:Label ID="lbTCPTTSTTC1" runat="server" Text="Tổng chi phí thanh toan sau thuế"></asp:Label>
                    </td>
                    <td class="crmcell">
                        <div class="right">
                            <asp:Label ID="lbTCPTTSTTC2" runat="server" Text =" " Font-Bold="True" Font-Size="Larger" ></asp:Label>
                        </div> 
                        <td class="crmcell right">
                            <asp:Label ID="lbTCPTTSTTC3" runat="server" Text="G1 + G2 + G3 + G4 = "></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTCPTTSTTC4" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="#FF3300" ></asp:Label>
                            </div>                        
                        </td>                       
                    </td>
		        </tr>
                <caption>
                    <br />
                    <br />
                    <tr>
                        <td class="crmcell right">
                            <asp:Label ID="lbTGTXLTT" runat="server" Text="Tổng giá trị xây lắp trước thuế"></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTGTXLTT2" runat="server" Font-Bold="True" Font-Size="Larger"></asp:Label>
                            </div>
                            <td class="crmcell right">
                                <asp:Label ID="lbTCTVCTT" runat="server" Text="Tổng CP vận chuyển trước thuế"></asp:Label>
                            </td>
                            <td class="crmcell">
                                <div class="right">
                                    <asp:Label ID="lbTCTVCTT2" runat="server" Font-Bold="True" Font-Size="Larger"></asp:Label>
                                </div>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">
                            <asp:Label ID="lbTTGTGT" runat="server" Text="Tổng thuế GTGT 10%"></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTTGTGT2" runat="server" Font-Bold="True" Font-Size="Larger"></asp:Label>
                            </div>
                            <td class="crmcell right">
                                <asp:Label ID="lbTTGTGTVC" runat="server" Text="Tổng thuế GTGT 10%"></asp:Label>
                            </td>
                            <td class="crmcell">
                                <div class="right">
                                    <asp:Label ID="lbTTGTGTVC2" runat="server" Font-Bold="True" Font-Size="Larger"></asp:Label>
                                </div>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">
                            <asp:Label ID="lbTGTXLST" runat="server" Text="Tổng giá trị xây lắp sau thuế"></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="right">
                                <asp:Label ID="lbTGTXLST2" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Blue"></asp:Label>
                            </div>
                            <td class="crmcell right">
                                <asp:Label ID="lbTCPVCST" runat="server" Text="Tổng CP vận chuyển sau thuế"></asp:Label>
                            </td>
                            <td class="crmcell">
                                <div class="right">
                                    <asp:Label ID="lbTCPVCST2" runat="server" Font-Bold="True" Font-Size="Larger" ForeColor="Blue"></asp:Label>
                                </div>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <td class="crmcell right">
                            <asp:Label ID="lbTONCONGSAUTHUE" runat="server" Text="Tổng cộng sau thuế"></asp:Label>
                        </td>
                        <td class="crmcell">
                            <div class="left">
                                <asp:Label ID="lbTONCONGSAUTHUE2" runat="server" Font-Bold="True" Font-Size="XX-Large" ForeColor="Red"></asp:Label>
                            </div>
                        </td>
                    </tr>
                </caption>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upButton" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tr>
                        <td class="crmcell right">
                            <asp:Button ID="btnBaoCao" CssClass="report" runat="server" UseSubmitBehavior="false"
                                     OnClientClick="return CheckFormBaoCao();" OnClick="btnBaoCao_Click" />
                        </td>
                        <td class="crmcell">
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upBAOCAOBVT" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <div id="divBVTLX" class="crmcontainer">
                <table class="crmtable">
                    <tr>
                        <td class="crmcell" colspan="6" style="height: 502px" valign="top">
                            <CR:CrystalReportViewer ID="rpViewer" runat="server" PrintMode="ActiveX" AutoDataBind="true" 
                               Visible="false"/>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>
