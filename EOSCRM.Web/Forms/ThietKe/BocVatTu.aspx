<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true"
    EnableEventValidation="true" CodeBehind="BocVatTu.aspx.cs" Inherits="EOSCRM.Web.Forms.ThietKe.BocVatTu" %>

<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Import Namespace="EOSCRM.Util"%>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function() {
            $("#divVatTu").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 20,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divVatTuDlgContainer");
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

        function CheckFormAddVatTu() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(linkBtnChangeKhoiLuong) %>', '');
        }

        function CheckFormAddGhiChu() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddGhiChu) %>', '');
        }

        function CheckFormAddChiPhi() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnAddChiPhi) %>', '');
        }
        
        function CheckFormFilterVatTu() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterVatTu) %>', '');
            return;
        }
		
		function checkChange()
		{
		    if(confirm('Tất cả dữ liệu có sẵn sẽ bị thay thế bởi mẫu bốc vật tư. Đổi?'))
		    {
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
		    closeWaitingDialog();
		}
		
		function updateGCTK(maGC, clientId) {
		    openWaitingDialog();
		    unblockWaitingDialog();
		    
		    var noidung = document.getElementById(clientId).value;
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateGCTK(maGC, noidung);		    
		    		   
		    if(msg.value != "<%= DELIMITER.Passed %>" && msg.value != "<%= DELIMITER.Failed %>") {
		        document.getElementById(clientId).value = msg.value;
		    }

		    closeWaitingDialog();
		}
		
		function updateCPTK(maCP, txtNDclientId, txtDGclientId, ddlDVTclientId, txtSLclientId, 
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
		    
		    var msg = EOSCRM.Web.Common.AjaxCRM.UpdateCPTK(maCP, nd, dg, dvt, sl, hs, loai);		    
		    		   
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
		        
		                                                   /* Math.formatNumber(sl * hs * dg + '', {
		                                                                                    decimals: 0,
		                                                                                    currency: false,
		                                                                                    currencySymbol: '',
		                                                                                    formatWhole: true,
		                                                                                    wholeDelimiter: '.',
		                                                                                    decimalDelimiter: ','
		                                                                                });*/
		    }
		    
		    closeWaitingDialog();
		}
		
	</script>
	
	
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
                            <asp:Button ID="btnSave" runat="server" CommandArgument="Change" CssClass="save" Visible="false"
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
    <br />
    	
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
                                                        CommandName="EditItem" Text='<%# Eval("MAVT") %>'></asp:LinkButton>
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
	
    <asp:UpdatePanel ID="upnlMBVT" UpdateMode="Conditional" runat="server">
		<ContentTemplate>
            <table class="crmcontainer">
                <tr>
                    <td class="header">Bốc vật tư</td> 
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
		                                    OnClientClick="openDialogAndBlock('Chọn từ danh sách vật tư', 600, 'divVatTu')"  />
		                            </div>
		                            <div class="left">
		                                <asp:TextBox ID="txtKHOILUONG" Width="60px" runat="server"
		                                    onkeypress="return CheckFormKhoiLuongKeyPress(event);" />
		                                <asp:LinkButton ID="linkBtnChangeKhoiLuong" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnChangeKhoiLuong_Click" runat="server">Change KL</asp:LinkButton>
		                            </div>
                                    <div class="left">
                                        <asp:Button ID="btAddVatTu" runat="server" CssClass="myButton" Text="Thêm " UseSubmitBehavior="false"
                                            OnClientClick="return CheckFormAddVatTu();" OnClick="btAddVatTu_Click" />
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
		        <tr style="display:none">
                    <td class="header">Ghi chú</td> 
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
                <tr>
                    <td class="header">Sơ đồ thiết kế</td> 
		        </tr>
                <tr>
                    <td>
		               <table class="crmtable">
		                    <tr>
                                <td class="crmcell right">Tên khách hàng bên phải</td>
                                <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtTENKHBP"  
		                                    Width="150px" runat="server" />
		                            </div>
                                    <div class="left" >
                                        <strong>Danh số KH bên phải</strong>
                                    </div>
                                    <div class="left">
                                        <asp:TextBox ID="txtDANHSOBP" runat="server" Width="80px" Text = "" />
                                    </div>                       
                                </td>                                
                            </tr>
                        </table>
                     </td>
                </tr>
                <tr>
                    <td>
		               <table class="crmtable">
		                    <tr>
                                <td class="crmcell right">Tên khách hàng bên trái</td>
                                <td class="crmcell">
		                            <div class="left">
		                                <asp:TextBox ID="txtTENKHBT"  
		                                    Width="150px" runat="server" />
		                            </div>
                                    <div class="left" >
                                        <strong>Danh số KH bên trái</strong>
                                    </div>
                                    <div class="left">
                                        <asp:TextBox ID="txtDANHSOBT" runat="server" Width="80px" Text = "" />
                                    </div>                       
                                </td>                                
                            </tr>
                        </table>
                     </td>
                </tr>
                <tr>
                    <td>
		               <table class="crmtable">
		                    <tr>
                                <td class="crmcell right">Chọn mẫu thiết kế</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:DropDownList ID="ddlMBTHIETKE" Width="250px" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddlMBTHIETKE_SelectedIndexChanged1"       >
                                        </asp:DropDownList>
                                    </div> 
                                    <div class="left">
                                        <span style="color: Red; font-weight: bold">Chú ý: Phải nhập tên khách hàng và danh số trước mới chọn mẫu thiết kế.</span>
                                     </div>                                   
                                </td>
                                </tr>
                        </table>
                     </td>
                </tr>
                <tr>
                    <td>
		               <table class="crmtable">
		                    <tr>
                                <td class="crmcell right"> </td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Button ID="btSaveMauThietKe" runat="server" CssClass="myButton" Text="Lưu mẫu thiết kế" OnClick="btSaveMauThietKe_Click" />
                                    </div>                                                                      
                                </td>
                                </tr>
                        </table>
                     </td>
                </tr>
		        <tr>
                    <td class="header">Chi phí đào lắp</td> 
		        </tr>
		        <tr>
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
                                            <asp:Label ID="lblTHANHTIENCP" Text='<%# Bind("THANHTIENCP") %>' runat="server"></asp:Label>
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
            <br />
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
	
	<br />
    <div class="crmcontainer p-5">
        <a href="../ThietKe/DuyetThietKe.aspx">Chuyển sang bước kế tiếp</a>
    </div>
    <div class="crmcontainer p-5">
        <a href="../ThietKe/NhapThietKe.aspx">Quay lại nhập thiết kế</a>
    </div>
</asp:Content>
