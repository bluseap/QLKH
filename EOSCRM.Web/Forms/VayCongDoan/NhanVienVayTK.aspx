<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="NhanVienVayTK.aspx.cs" Inherits="EOSCRM.Web.Forms.VayCongDoan.NhanVienVayTK" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">

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

        function onFocusEventHandler(controlId) {
            FocusAndSelect(controlId);
        }

        function onSelectedIndexChangedEventHandler(txtTIENGOCId, txtTIENLAIId, ddlHTTTId, ddlTHANHTOANId, hfGCSId, e) {            

            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");

            var oldTiengoc = idArr[3];
            var oldTienlai = idArr[4];

            var httt = $("#" + ddlHTTTId + " option:selected").val();
            var thanhtoan = $("#" + ddlTHANHTOANId + " option:selected").val();

            var tiengoc = $("#" + txtTIENGOCId).val();
            var tienlai = $("#" + txtTIENLAIId).val();
            
            var msg = 'Vui 022 lòng nhập chỉ số hợp lệ.';
            var msgTT = 'Vui lòng nhập chỉ số hợp lệ trước khi chuyển về trạng thái ghi bình thường.';

            var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateNhanVienVayKy(idArr[0], idArr[1], idArr[2], tiengoc.toString(), tienlai.toString(), httt.toString(), thanhtoan.toString());

            $.ajax({
                type: "POST",
                url: "EOSCRM.Web.Common.AjaxCRM.cs/UpdateNhanVienVayKy",
                data: '{idArr[0], idArr[1], idArr[2],tiengoc.toString(), tienlai.toString(), httt.toString(), thanhtoan.toString()}',
                contentType: "json"
            });

            if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                setControlValue(txtTIENGOCId, oldTiengoc);
                setControlValue(txtTIENLAIId, oldTienlai);

                alertWithFocusSelect(msg, oldTiengoc);
                return;
            }

            // update hidden field
            var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                idArr[1] + "<%= DELIMITER.Delimiter %>" +
                        idArr[2] + "<%= DELIMITER.Delimiter %>" +
                        csd + "<%= DELIMITER.Delimiter %>" +
                        csc + "<%= DELIMITER.Delimiter %>" +                        

            alertWithFocusSelect("Cập nhật 01 chỉ số thành công.", ddlTHANHTOANId);

            // save to hidden field
            $("#" + hfGCSId).val(val);

        }

        function onKeyDownEventHandler(txtTIENGOCId, txtTIENLAIId, ddlHTTTId, ddlTHANHTOANId, hfGCSId, e) {
            // get key code
            if (!isValid)
                return;

            var code = (e.keyCode ? e.keyCode : e.which);
            jQuery.fn.exists = function () { return jQuery(this).length > 0; }

            // key codes: 
            // tab = 9, right arrow = 39
            // enter = 13, page down = 34, down arrow = 40
            // page up = 33, up arrow = 38 
            // left arrow = 37

            var hfvalue = $("#" + hfGCSId).val();
            var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
            if (idArr.length != 4) return;

            var oldTiengoc = idArr[3];
            var oldTienlai = idArr[4];        
            
            var httt = $("#" + ddlHTTTId + " option:selected").val();
            var thanhtoan = $("#" + ddlTHANHTOANId + " option:selected").val();           

            var tiengoc = $("#" + txtTIENGOCId).val();
            var tienlai = $("#" + txtTIENLAIId).val();

            var msg = 'Vui lòng nhập chỉ số hợp lệ.';
            var msg2 = 'Khối lượng tiêu thụ không khớp với chỉ số. \r\nCó tiếp tục nhập theo dạng khoán không?\r\n\r\nNếu tiếp tục thì bấm /';
            var msg3 = 'Cập nhật 03 chỉ số không thành công';

            // enter: validation, save record, move to next row
            if (code == 13) {
                if (order == 2)
                    FocusAndSelect(txtTIENLAIId);

                else if (order == 3)
                    FocusAndSelect(ddlHTTTId);

                else if (order == 4) {
                    // validation
                    /*if (isDataValid(txtCHISODAUId, txtCHISOCUOIId, txtMTRUYTHUId, txtKLTIEUTHUId) == false) {
                        alertWithFocusSelect(msg, txtCHISOCUOIId);
                        return;
                    }
                    */                   

                    /*if (kltt != csc + mtt - csd) 
                    {
                        //tthaighi = 'Q';
                        var ret = prompt(msg2, "");
                        if (ret != '/' && ret != '/') 
                        {
                            FocusAndSelect(txtCHISOCUOIId);
                            return;
                        } 
                    }*/

                    // save record
                    // UpdateGCS(idkh, namStr, thangStr, csdStr, cscStr, klttStr, ghichu, tthaighi, manv)
                    
                    //var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateGCS2(idArr[0], idArr[1], idArr[2], csd, csc, mtt, kltt, idArr[8], tthaighi, idArr[7]);
                    
                    var savingMsg = EOSCRM.Web.Common.AjaxCRM.UpdateNhanVienVayKy(idArr[0], idArr[1], idArr[2], tiengoc.toString(), tienlai.toString(), httt.toString(), thanhtoan.toString());

                    if (savingMsg.value != "<%= DELIMITER.Passed %>") {
                        setControlValue(txtTIENGOCId, oldTiengoc);
                        setControlValue(txtTIENLAIId, oldTienlai);                       

                        alertWithFocusSelect(msg3, oldTiengoc);
                        return;
                    }

                    // update hidden field
                    var val = idArr[0] + "<%= DELIMITER.Delimiter %>" +
                        idArr[1] + "<%= DELIMITER.Delimiter %>" +
                        idArr[2] + "<%= DELIMITER.Delimiter %>" +
                        tiengoc + "<%= DELIMITER.Delimiter %>" +
                        tienlai + "<%= DELIMITER.Delimiter %>"

                    // save to hidden field
                    $("#" + hfGCSId).val(val);
                    

                    alertWithFocusSelect("Cập nhật kỳ vay thành công.", txtTIENGOCId);
                }
            }

        }

        function getPrefixId(controlId) {
            return controlId.substring(0, controlId.lastIndexOf('_'));
        }

        function getControlName(controlId) {
            return controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
        }


        function getNextId(controlId) {
            var ctrlName = controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
            var prefix = controlId.substring(0, controlId.lastIndexOf('_'));
            var index = prefix.substring(prefix.lastIndexOf('_') + 4, prefix.length);
            var shortPrefix = prefix.substring(0, prefix.lastIndexOf('_')) + '_ctl';

            var nextIndex = parseInt(index, 10) + 1;
            if (nextIndex < 10)
                nextIndex = '0' + nextIndex;

            return shortPrefix + nextIndex + '_' + ctrlName;
        }

        function getPrevId(controlId) {
            var ctrlName = controlId.substring(controlId.lastIndexOf('_') + 1, controlId.length);
            var prefix = controlId.substring(0, controlId.lastIndexOf('_'));
            var index = prefix.substring(prefix.lastIndexOf('_') + 4, prefix.length);
            var shortPrefix = prefix.substring(0, prefix.lastIndexOf('_')) + '_ctl';

            var nextIndex = parseInt(index, 10) - 1;
            if (nextIndex < 10)
                nextIndex = '0' + nextIndex;

            return shortPrefix + nextIndex + '_' + ctrlName;
        }


    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" OnTextChanged="txtNAM_TextChanged" />
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlTHANG2" runat="server" TabIndex="1" Visible="false">
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
                            </td>
                       </tr>
                        <tr>    
                            <td class="crmcell right">Tổng tiền gốc</td>
                            <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:Label ID="lblTONGTIENGOC" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tổng tiền lãi</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblTONGTIENLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div> 
                                <div class="left">
                                    <div class="right">Tổng thu</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGTHU" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>                                
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tổng tiền chưa thu</td>
                            <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:Label ID="lbTONGTIENCHUATHU" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div> 
                                <div class="left">
                                    <div class="right">Tổng chi còn lại</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTIENCHICONLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
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
                                
                            </td>
                        </tr>
                        <tr>
                            <td class="header">TỔNG THU, CHI ĐẾN NAY</td> 
		                </tr>
                        <tr>    
                             <td class="crmcell right">Tổng thu nhân viên tham gia tiết kiệm</td>
                             <td class="crmcell"> 
                                <div class="left">                                    
                                    <asp:Label ID="lbTHUTIETKIEM" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>     
                                <div class="left">
                                    <div class="right">Tổng chi còn lại</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbCHICONLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>                              
                             </td>
                         </tr>
                        <tr>
                            <td class="crmcell right">Tổng thu lãi</td>
                            <td class="crmcell"> 
                                 <div class="left">
                                    <asp:Label ID="lbTHUTIENLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tổng tiền còn lại</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lbTONGTIENCONLAI" runat="server" ForeColor="Blue" Font-Size="Large"></asp:Label>
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
            <div class="crmcontainer" id="divList" runat="server">
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" PageSize="700"  
                    OnRowDataBound="gvList_RowDataBound" OnPageIndexChanging="gvList_PageIndexChanging">
                    <PagerSettings FirstPageText="nhân viên vay" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                            <HeaderStyle CssClass="checkbox" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="10px" HeaderText="Mã NV">
                            <ItemTemplate>
                                <%# Eval("MANVV")%>
                                <%--
                                
                                cau truc du lieu trong hidden field:
                                
                                <IDKH>__crmdelimiter__
                                <NAM>__crmdelimiter__
                                <THANG>__crmdelimiter__
                                <CHISODAU>__crmdelimiter__
                                <CHISOCUOI>__crmdelimiter__
                                <KLTIEUTHU>__crmdelimiter__
                                <MANV_CS>__crmdelimiter__
                                <GHICHU_CS>
                                
                                --%>
                                
                                <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("MANVV") + DELIMITER.Delimiter + 
                                        Eval("NAM") + DELIMITER.Delimiter + 
                                        Eval("THANG") + DELIMITER.Delimiter + 
                                        Eval("TIENGOC") + DELIMITER.Delimiter + 
                                        Eval("TIENLAI") + DELIMITER.Delimiter
                                 %>' />                                 
                            </ItemTemplate>
                            <HeaderStyle Width="65px" />
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="25%" HeaderText="Tên KH" DataField="HOTEN" >                        
                            <HeaderStyle Width="25%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Tiền gốc">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTIENGOC" Text='<%# Bind("TIENGOC") %>'                                     
                                    CssClass='<%# Eval("THANHTOAN").ToString() != "CO" ? "readonly" : "editable" %>'
                                    ReadOnly='<%# Eval("THANHTOAN").ToString().Equals("CO") ? false : true %>' 
                                    runat="server" />                                    
                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Tiền lãi">
                            <ItemTemplate>
                                <asp:TextBox ID="txtTIENLAI" Text='<%# Bind("TIENLAI") %>' 
                                    CssClass='<%# Eval("THANHTOAN").ToString() != "CO" ? "readonly" : "editable" %>'
                                    ReadOnly='<%# Eval("THANHTOAN").ToString().Equals("CO") ? false : true %>' 
                                    runat="server" />
                            </ItemTemplate>
                            <HeaderStyle Width="60px" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Tổng tiền" HeaderStyle-Width="11%" >
                            <ItemTemplate>
                                <%# (Eval("TONGTIEN") != null) ?
                                        String.Format("{0:0,0}", Eval("TONGTIEN")) : "0" %>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" Font-Size="Large"/>                            
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Hình thức trả">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlHTTT" Width="110px" SelectedValue='<%# Bind("MAHTTTT") %>' 
                                    CssClass='<%# Eval("THANHTOAN").ToString() != "CO" ? "Enabled" : "editable" %>'
                                    Enabled='<%# Eval("THANHTOAN").ToString().Equals("CO") ? true : false %>' 
                                    runat="server">
                                    <asp:ListItem Value="TT" Text="Trừ lương" />                                    
                                    <asp:ListItem Value="TM" Text="Tiền mặt" />                                    
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="115px" />
                        </asp:TemplateField>  
                        <asp:TemplateField HeaderText="Tổng tiền CL" HeaderStyle-Width="7%" >
                            <ItemTemplate>
                                <%# (Eval("CONLAI") != null) ?
                                        String.Format("{0:0,0}", Eval("CONLAI")) : "0" %>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>                       
                        <asp:TemplateField HeaderStyle-Width="100px" HeaderText="Thanh toán">
                            <ItemTemplate>
                                <asp:DropDownList ID="ddlTHANHTOAN" Width="100px" SelectedValue='<%# Bind("THANHTOAN") %>' 
                                    CssClass='<%# (Int32.Parse(Eval("NAM").ToString()) != DateTime.Now.AddMonths(-1).Year && Int32.Parse(Eval("THANG").ToString()) != DateTime.Now.AddMonths(-1).Month) ? "Enabled" : "editable" %>'
                                    Enabled='<%# (Int32.Parse(Eval("NAM").ToString()) == DateTime.Now.AddMonths(-1).Year && Int32.Parse(Eval("THANG").ToString()) == DateTime.Now.AddMonths(-1).Month) ? true : false %>' 
                                    runat="server">
                                    <asp:ListItem Value="CO" Text="Chưa đóng" />                                    
                                    <asp:ListItem Value="NO" Text="Đã đóng" />    
                                    <asp:ListItem Value="TA" Text="Tất toán" />
                                    <asp:ListItem Value="KO" Text="Trở lại" />                                
                                </asp:DropDownList>
                            </ItemTemplate>
                            <HeaderStyle Width="115px" />
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderText="Kỳ trả" HeaderStyle-Width="3%" >
                            <ItemTemplate>
                                <%# (Eval("TRAKYLAN") != null) ?
                                        String.Format("{0:0,0}", Eval("TRAKYLAN")) : "0" %>
                            </ItemTemplate>
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Trạng thái"  HeaderStyle-Width="70px">
                            <ItemTemplate>
                                <asp:Button ID="imgTT" runat="server" Width="90px" OnClientClick="return false;"
                                     CausesValidation="false" UseSubmitBehavior="false" />
                            </ItemTemplate>
                        </asp:TemplateField>   
                    </Columns>    
                </eoscrm:Grid>
            </div>      
            <br />
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right"><a href="#top">Về đầu trang</a></td>
                            <td class="crmcell">   
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
