<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="QuanLyMauBVTPower.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.Power.QuanLyMauBVTPower" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util"%>
<%@ Import Namespace="EOSCRM.Dao"%>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

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
        });

        function CheckFormMAVTKeyPress(e) {
            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }
            if (code == 13 || code == 9) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeMAVT) %>', '');
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

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            return false;
        }

        function CheckFormCancel() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnCancel) %>', '');
            return false;
        }

        function CheckFormAddGhiChu() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddGhiChu) %>', '');
            return false;
        }

        function CheckFormAddChiPhi() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddChiPhi) %>', '');
            return false;
        }

        function CheckFormFilterVatTu() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTu) %>', '');
            return false;
        }

        function CheckFormDelete() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            }
            return false;
        }

        function updateCTMBVT(maMBVT, maVT, txtSLId, txtGIAVTId, lblTIENVT, txtGIANCId, lblTIENNC, cbISCTYDTU) {
            openWaitingDialog();
            unblockWaitingDialog();

            var sl = document.getElementById(txtSLId).value;
            var gvt = document.getElementById(txtGIAVTId).value;
            var gnc = document.getElementById(txtGIANCId).value;
            var cb = document.getElementById(cbISCTYDTU).checked ? "true" : "false";

            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCTMBVT(maMBVT, maVT, sl, gvt, gnc, cb);

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

            closeWaitingDialog();
        }

        function updateGCMBVT(maGC, clientId) {
            openWaitingDialog();
            unblockWaitingDialog();

            var noidung = document.getElementById(clientId).value;
            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateGCMBVT(maGC, noidung);

            if (msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
                document.getElementById(clientId).value = msg.value;
            }

            closeWaitingDialog();
        }

        function updateCPMBVT(maCP, txtNDclientId, txtDGclientId, ddlDVTclientId, txtSLclientId,
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

            var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPMBVT(maCP, nd, dg, dvt, sl, hs, loai);

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
                /*Math.formatNumber(sl * hs * dg + '', {
                decimals:0,
                currency:false,
                currencySymbol:'',
                formatWhole:true,
                wholeDelimiter:'.',
                decimalDelimiter:','
                });*/

                /*var number = sl * hs * dg;
                number = $("#" + lblTTclientId).format(number, { format: "#,###.00", locale: "vn" });
                $("#" + lblTTclientId).val(number);*/

            }

            closeWaitingDialog();
        }

	</script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Mã mẫu bốc vật tư</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMADDK" runat="server" Width="200px" MaxLength="10" TabIndex="1" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Tên mẫu bốc vật tư</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtTENTK" runat="server" Width="400px" MaxLength="200" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="save" UseSubmitBehavior="false"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" TabIndex="16" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnCancel" runat="server" CssClass="cancel" UseSubmitBehavior="false"
                                        OnClick="btnCancel_Click" OnClientClick="return CheckFormCancel();" TabIndex="17" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false"
                                        OnClick="btnDelete_Click" OnClientClick="return CheckFormDelete();" TabIndex="18" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />    
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_RowDataBound" 
                    OnRowCommand="gvList_RowCommand"  OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <PagerSettings FirstPageText="mẫu bốc" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MADDK") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MADDK") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Mã mẫu bốc vật tư" HeaderStyle-Width="120px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="EditItem" CssClass="link" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                            <FooterTemplate>
                                <a href="javascript:ToggleAll(true)"><strong>Chọn hết</strong></a> / <a href="javascript:ToggleAll(false)">
                                    <strong>Bỏ chọn hết</strong></a>
                            </FooterTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tên tên mẫu bốc vật tư">
                            <ItemTemplate>
                                <%# Eval("TENTK") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ngày tạo" HeaderStyle-Width="60px">
                            <ItemTemplate>
                                <%# (Eval("NGAYTK") != null) ? 
                                                String.Format("{0:dd/MM/yyyy}", Eval("NGAYTK")) : "" %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="100px">
                            <ItemTemplate>                    
                                <asp:LinkButton ID="btnBocVatTu" Text="Bốc vật tư" CommandName="BocVatTu"
						            CausesValidation="false" CommandArgument='<%#Eval("MADDK")%>' runat="server"></asp:LinkButton>&nbsp;&nbsp;	                                
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />
    <div id="divVatTuDlgContainer">	
		<div id="divVatTu" style="display:none">		    
	        <asp:UpdatePanel ID="upnlVatTu" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
					<table cellpadding="3" cellspacing="1" style="width: 500px;">
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">Kho Xí nghiệp</td>
                                            <td class="crmcell">
                                                <div class="left">                                                
                                                    <asp:DropDownList ID="ddlKhoXiNghiep" runat="server" ></asp:DropDownList>
                                                </div>                                                
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">Từ khóa</td>
                                            <td class="crmcell">
                                                <div class="left">                                                
                                                    <asp:TextBox ID="txtFilterVatTu" onchange="return CheckFormFilterVatTu();" runat="server" />
                                                </div>
                                                <div class="left">  
                                                    <asp:Button ID="btnFilterVatTu" OnClientClick="return CheckFormFilterVatTu();" runat="server" 
                                                        CssClass="filter" OnClick="btnFilterVatTu_Click" UseSubmitBehavior="false" />
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
                                            <asp:TemplateField HeaderText="Mã VT K.Toán" HeaderStyle-Width="90px">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MAVT") %>'  CommandName="EditItem" 
                                                        Text='<%# Eval("KeToanMaSoVatTu") != null ?  Eval("KeToanMaSoVatTu").ToString() : "" %>' >
                                                    </asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                           <%-- <asp:BoundField HeaderText="Mã hiệu" DataField="MAHIEU" HeaderStyle-Width="15%" />--%>
                                            <asp:BoundField HeaderText="Tên vật tư" DataField="TENVT" HeaderStyle-Width="240px" />
                                            <asp:BoundField HeaderText="Đơn vị tính" DataField="DVT" HeaderStyle-Width="40px" />
                                            <asp:TemplateField HeaderText="Kho XN" HeaderStyle-Width="160px">
                                                <ItemTemplate>
                                                    <%# new KhoDanhMucDao().Get(Eval("KhoDanhMucId") != null ? Eval("KhoDanhMucId").ToString() : "" ).TenKho.ToString()    %>
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
	
	                    
			            
	<asp:UpdatePanel ID="upnlMBVT" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
		    
		    <table class="crmcontainer" id="tblMBVT" runat="server">
		        <tr>
                    <td class="header">Bốc vật tư (<%= SelectedMBVT != null ? SelectedMBVT.TENTK : "" %>)</td> 
		        </tr>
		        <tr>
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right">Mã vật tư</td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtMAVT" onkeypress="return CheckFormMAVTKeyPress(event);"     
		                                    Width="60px" runat="server" />
		                                <asp:LinkButton ID="linkBtnChangeMAVT" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeMAVT_Click" runat="server">Change MAVT</asp:LinkButton>
		                            </div>
		                            <div class="left">
		                                <asp:Button ID="btnBrowseVatTu" runat="server" CssClass="pickup" 
		                                    OnClick="btnBrowseVatTu_Click" CausesValidation="false" UseSubmitBehavior="false"
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 500, 'divVatTu')"  />
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
                            <eoscrm:Grid ID="gvSelectedVatTu" runat="server" UseCustomPager="true" PageSize="2000" 
                                OnRowCommand="gvSelectedVatTu_RowCommand" OnRowDataBound="gvSelectedVatTu_RowDataBound">
                                <PagerSettings FirstPageText="vật tư" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Mã VT K.Toán" HeaderStyle-Width="80px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.KeToanMaSoVatTu") != null ?  Eval("VATTU.KeToanMaSoVatTu").ToString() : "" %>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <%--<asp:TemplateField HeaderText="Mã hiệu" HeaderStyle-Width="60px">
                                        <ItemTemplate>
                                            <%# Eval("VATTU.MAHIEU") %>
                                        </ItemTemplate>
                                    </asp:TemplateField>--%>
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
		            <td class="header">Ghi chú (<%= SelectedMBVT != null ? SelectedMBVT.TENTK : "" %>)</td>		        
		        </tr>
		        <tr style="display:none">
		            <td>
		                <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:Button ID="btnAddGhiChu" runat="server" CssClass="addnew" OnClientClick="return CheckFormAddGhiChu();" 
		                                    OnClick="btnAddGhiChu_Click" CausesValidation="false" UseSubmitBehavior="false" />
		                            </div>
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important;">
		                    <eoscrm:Grid ID="gvGhiChu" runat="server" UseCustomPager="true" PageSize="2000"
		                        OnRowCommand="gvGhiChu_RowCommand" OnRowDataBound="gvGhiChu_RowDataBound">
		                        <PagerSettings FirstPageText="ghi chú" PageButtonCount="2" />
				                <Columns>
					                <asp:TemplateField HeaderText="#" HeaderStyle-CssClass="checkbox">
                                        <ItemTemplate>
                                            <%# Container.DataItemIndex + 1%>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Ghi chú">
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtNOIDUNG" Text='<%# Bind("NOIDUNG") %>' Width="98%" TextMode="MultiLine" runat="server" />                            
                                        </ItemTemplate>
                                    </asp:TemplateField>
					                <asp:TemplateField HeaderText="Hoạt động" HeaderStyle-Width="60px">
						                <ItemTemplate>
							                <asp:LinkButton ID="btnDelete" Text="Xóa" CommandName="DeleteGhiChu" 
							                    CausesValidation="false" CommandArgument='<%# Eval("MAGHICHU")%>' runat="server"></asp:LinkButton>	
						                </ItemTemplate>
					                </asp:TemplateField>
				                </Columns>
			                </eoscrm:Grid>
			            </div>
		            </td>
		        </tr>
		        <%--<tr>
		            <td class="header">Chi phí đào lắp (<%= SelectedMBVT != null ? SelectedMBVT.TENTK : "" %>)</td>		        
		        </tr>--%>
		        <tr>
		            <td>
		               <table class="crmtable">
		                    <tr>
		                        <td class="crmcell right"></td>
		                        <td class="crmcell">
		                            <div class="left">
		                                <asp:Button ID="btnAddChiPhi" runat="server" CssClass="addnew" OnClientClick="return CheckFormAddChiPhi();" 
		                                    OnClick="btnAddChiPhi_Click" CausesValidation="false" UseSubmitBehavior="false" Visible="False" />
		                            </div>
		                        </td>
		                    </tr>
		                </table>
		                <div class="crmcontainer" style="border-left: none !important; border-right: none !important; border-bottom: none !important;">
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
		    </table>			
		</ContentTemplate>
	</asp:UpdatePanel>
</asp:Content>
