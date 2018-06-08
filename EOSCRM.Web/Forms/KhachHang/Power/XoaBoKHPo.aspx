<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="XoaBoKHPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.XoaBoKHPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register src="/UserControls/KhachHangFilterPanel.ascx" tagname="KhachHangFilterPanel" tagprefix="powaco" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=10.5.3700.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divKhachHang").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divKhachHangDlgContainer");
                }
            });
        });

        function CheckFormSave() {
            if (CheckRecordSelected('delete')) {
                openWaitingDialog();
                unblockWaitingDialog();
                __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');
            }
            return false;
        }

        function CheckFormDelete() {
            //if (CheckRecordSelected('delete')) {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnDelete) %>', '');
            //}
                    return false;
                }

                function CheckFormReport() {
                    openWaitingDialog();
                    unblockWaitingDialog();
                    __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnBaoCao) %>', '');
        }

        function CheckFormSearch() {
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');
        }

        function CheckFormFilterKH() {  
            openWaitingDialog();
            unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterKH) %>', '');
            return false;
        }

        function CheckFormbtExcelXoa() {
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btExcelXoa) %>', '');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divKhachHangDlgContainer">
        <div id="divKhachHang" style="display: none">
            <asp:UpdatePanel ID="upnlKhachHang" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <table cellpadding="3" cellspacing="1" style="width: 610px;">                        
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>
                                        <tr>
                                            <td class="crmcell right">
                                                Mã khách hàng
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtIDKH" runat="server" OnChange="return CheckFormFilterKH();" MaxLength="200" Width="80px"/>
                                                </div>
                                                <div class="left ">
                                                    <div class="right">Tên KH</div>
                                                </div>
                                                <div class="left">
                                                    <asp:TextBox ID="txtTENKH" runat="server" MaxLength="200" Width="100px"/>
                                                </div>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Mã đồng hồ
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtMADH" runat="server" MaxLength="200" Width="80px"/>
                                                </div>
                                                <div class="left ">
                                                    <div class="right">Số hợp đồng</div>
                                                </div>
                                                <div class="left">
                                                    <asp:TextBox ID="txtSOHD" runat="server" MaxLength="200" Width="100px"/>
                                                </div>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Số nhà
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtSONHA" runat="server" MaxLength="200" Width="80px"/>
                                                </div>
                                                <div class="left ">
                                                    <div class="right">Tên đường phố</div>
                                                </div>
                                                <div class="left">
                                                    <asp:TextBox ID="txtTENDP" runat="server" MaxLength="200" Width="100px"/>
                                                </div>  
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right">
                                                Khu vực
                                            </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:DropDownList ID="ddlKHUVUC" runat="server"></asp:DropDownList>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"></td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="btnFilterKH" OnClick="btnFilterKH_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterKH();" 
                                                        runat="server" CssClass="filter" Text="" />
                                                </div>
                                            </td>
                                        </tr>
                                    </tbody>
                                </table>                                                                  
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcontainer">
                                <div class="crmcontainer">                                   
                                    <eoscrm:Grid ID="gvDanhSach" runat="server" UseCustomPager="true" 
							            OnPageIndexChanging="gvDanhSach_PageIndexChanging" 
							            OnRowDataBound="gvDanhSach_RowDataBound" OnRowCommand="gvDanhSach_RowCommand">
                                        <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã KH">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnIDDS" runat="server" CommandName="SelectSODB" 
                                                        CommandArgument='<%# Eval("IDKHPO") %>'
                                                        Text='<%# Eval("MADPPO").ToString() + Eval("MADBPO").ToString() %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="TENKH" HeaderText="Tên khách hàng" />
                                            <asp:TemplateField HeaderStyle-Width="55%" HeaderText="Địa chỉ">
                                                <ItemTemplate>
                                                    <%# Eval("SONHA") != null ? Eval("SONHA") + ", " : "" %>                                                     
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
    <asp:UpdatePanel ID="upnlThongTin" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">Kỳ áp dụng</td>
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
                                </div>
                                <div class="left width-150 pleft-50">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC1" AutoPostBack="true" Width="150px" runat="server" TabIndex="3">
                                    </asp:DropDownList>
                                </div>                              
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Danh số</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:TextBox ID="txtSODB" runat="server" 
                                        MaxLength="8" Width="90px" TabIndex="2" ReadOnly="True" 
                                        />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseKH" runat="server" CssClass="pickup" OnClick="btnBrowseKH_Click"
                                        CausesValidation="false" UseSubmitBehavior="false" 
                                        OnClientClick="openDialogAndBlock('Chọn từ danh sách khách hàng', 610, 'divKhachHang')" 
                                        TabIndex="6" />
                                </div>                                 
                                <div class="left">
                                    <asp:Label ID="lblIDKH" runat="server" Visible="False"></asp:Label>
                                    <asp:Label ID="reloadm" runat="server" Visible="False"></asp:Label>
                                </div>                               
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Tên khách hàng</td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:Label ID="lblTENKH" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">    
                                <div class="left width-100">
                                    <asp:Label ID="lblTENDP" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Khu vực</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTENKV" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Mục đích sử dụng</div>
                                </div>
                                <div class="left">
                                    <asp:Label ID="lblMAMDSD" runat="server" Text=""></asp:Label>
                                </div>
                            </td>                            
                        </tr>
                        <tr>
                            <td class="crmcell right">CS Mới</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:Label ID="lblCSMOI" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">CS Cũ</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblCSCU" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tiêu thụ</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTIEUTHU" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Thành tiền</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:Label ID="lblTHANHTIEN" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Thuế GTGT</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTHUEGTGT" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="left">
                                    <div class="right">Tổng tiền</div>
                                </div>
                                <div class="left width-100">
                                    <asp:Label ID="lblTONGTIEN" runat="server" Text=""></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Xã, phường</td>
                            <td class="crmcell">
                                <div class="left width-100">
                                    <asp:DropDownList ID="ddlXAPHUONG" runat="server" OnSelectedIndexChanged="ddlXAPHUONG_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                                </div>
                                <div class="left">
                                    <div class="right">Ấp, khóm</div>
                                </div>
                                <div class="left">
                                    <asp:DropDownList ID="ddlAPKHOM" runat="server"></asp:DropDownList>
                                </div>
                            </td>                                  
                        </tr>               
                        <tr>    
                            <td class="crmcell right">Lý do xóa bộ</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtGhiChu" TextMode="MultiLine" Rows="3" Width="525" TabIndex="9" 
                                        MaxLength="300" runat="server" Font-Names="Times New Roman" />
                                </div>
                            </td>
                        </tr>   
                        <tr>    
                            <td class="crmcell right">  </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click"
                                        TabIndex="19" UseSubmitBehavior="false" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnSearch" OnClick="btnSearch_Click" 
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSearch();" 
                                        runat="server" CssClass="filter" Text="" TabIndex="12" />
                                </div>  
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right">  </td>
                            <td class="crmcell">                                
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CssClass="myButton" Text ="Phục hồi KH "  OnClick="btnSave_Click" 
                                        TabIndex="13" UseSubmitBehavior="false" OnClientClick="return CheckFormSave();" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnBaoCao" OnClientClick="return CheckFormReport();" runat="server" onclick="btnBaoCao_Click" CssClass="report" />
                                </div>   
                                <td class="crmcell right">
                                     <div class="left ">
                                        <asp:Button ID="btExcelXoa" runat="server" CssClass="myButton" OnClientClick="return CheckFormbtExcelXoa();"
                                                 OnClick="btExcel_Click" TabIndex="19" UseSubmitBehavior="false" Text="Xuất Excel"/>
                                    </div>                          
                                </td>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btExcelXoa" />
        </Triggers>
    </asp:UpdatePanel>
    <br />
    <asp:UpdatePanel ID="upnlCustomers" UpdateMode="Conditional" runat="server">
        <ContentTemplate> 
            <div class="crmcontainer">
                <eoscrm:Grid 
                    ID="gvKhachHang" runat="server" UseCustomPager="true" PageSize="50" OnRowDataBound="gvDanhSach_RowDataBound"
                        OnRowCommand="gvKhachHang_RowCommand" OnPageIndexChanging="gvKhachHang_PageIndexChanging">
                    <PagerSettings FirstPageText="khách hàng" PageButtonCount="2" />
                    <Columns>                        
                        <asp:TemplateField HeaderStyle-Width="10px" HeaderText="Danh số">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>  
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("IDKHPO") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("IDKHPO").ToString() %>' />
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                
                        <asp:TemplateField HeaderStyle-Width="70px" HeaderText="Danh số">
                            <ItemTemplate>
                            <asp:LinkButton ID="linkMa" runat="server" 
                                CommandArgument='<%# Eval("IDKHPO") %>'
                                CommandName="SelectHD" 
                                Text='<%# HttpUtility.HtmlEncode(Eval("MADPPO").ToString()+ Eval("MADBPO")) %>'>
                            </asp:LinkButton>                                
                            </ItemTemplate>
                            <ItemStyle Font-Bold="True" />
                        </asp:TemplateField>                             
                        <asp:BoundField HeaderStyle-Width="100px" HeaderText="Tên khách hàng" DataField="TENKH" />                       
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Khu vực">
                            <ItemTemplate>
                                <%# Eval("KHUVUCPO.TENKV") %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="Số No">
                            <ItemTemplate>
                                <%# Eval("DONGHOPO.SONO") %>
                            </ItemTemplate>
                        </asp:TemplateField> 
                        <asp:TemplateField HeaderStyle-Width="15px" HeaderText="MĐSD">
                            <ItemTemplate>
                                <%# Eval("MAMDSDPO") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="140px" HeaderText="Lý do" DataField="LYDOXOA" />    
                        <asp:TemplateField HeaderStyle-Width="40px" HeaderText="Kỳ xóa">
                            <ItemTemplate>
                               <%#  (Eval("NGAYXOA") != null) ?  String.Format("{0:MM/yyyy}", Eval("NGAYXOA"))                                                            : "" %>                                
                            </ItemTemplate>
                        </asp:TemplateField>                                         
                    </Columns>
                </eoscrm:Grid>   
            </div>         
        </ContentTemplate>
    </asp:UpdatePanel>
    <br />  
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" visible="false">
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                    DisplayGroupTree="False" />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>
</asp:Content>
