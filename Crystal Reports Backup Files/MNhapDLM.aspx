<%@ Page Language="C#" AutoEventWireup="true" 
    CodeBehind="MNhapDLM.aspx.cs" Inherits="EOSCRM.Web.WebMobi.KhachHang.MNhapDLM" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">    
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1"/>
    <link rel="stylesheet" href="/content/jquery/jquery.mobile-1.4.5.min.css"/>
    <link rel="stylesheet" href="/content/jquery/jquery-ui.css" />
    <link rel="shortcut icon" href="/content/images/powaco.ico" />
    <script src="/content/jquery/jquery-1.11.3.min.js"></script>
    <script src="/content/jquery/jquery-ui.js"></script>
    <script src="/content/jquery/jquery.mobile-1.4.5.min.js"></script>
    
    <script type="text/javascript">

        $(function () {
            $("#txtCAPNGAY").datepicker({ dateFormat: "dd/mm/yy" })
        });
        $(function () {
            $("#txtNGAYCD").datepicker({ dateFormat: "dd/mm/yy" })
        });

        $(function () {
            $("#popup-outside-page").enhanceWithin().popup();
        });
    </script>
    

</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" >
        </asp:ScriptManager> 
        <div id="popup-outside-page" data-theme="a">    
            <ul data-role="listview">
                <li>Menu</li>
                <li><a href="MNhapDLM.aspx">Nhập đơn lắp mới</a></li>
                <li><a href="MNhapDonLM.aspx">Nhập thiết kế</a></li>
                <li><a href="MNhapDonLM.aspx">Bốc vật tư</a></li>
            </ul>
        </div>     
        
        <div data-role="page">
            <div data-role="header">
                <div style="text-align:center">
                    <img src="/content/images/powacmo.png" />
                </div>
                    <a href="#popup-outside-page" data-rel="popup">Home</a>
                    
            </div>
            
          <div data-role="main" class="ui-content">
            <form method="post" action="demoform.asp">
                

              <div class="ui-field-contain">
                  <label for="fname">Mã dơn:</label>
                  <asp:Label ID="lbMAPHONG" runat="server" Text="MAPHONG" Visible="false"></asp:Label>
                  <asp:TextBox ID="txtMADDK" runat="server"></asp:TextBox>
                  <label for="lname">Tên trạm:</label>
                  <asp:DropDownList ID="ddlPHUONG" TabIndex="3" runat="server"></asp:DropDownList>
                  <label for="fname">Họ tên:</label>
                  <asp:TextBox ID="txtTENKH" runat="server" Width="180px" MaxLength="200" TabIndex="4" />
                  <label for="fname">Năm sinh:</label>
                  <asp:TextBox ID="txtNGAYSINH" runat="server" Width="100px" MaxLength="4" TabIndex="5" />
                  <label for="fname">Địa chỉ thường trú:</label>
                  <asp:TextBox ID="txtSONHA" runat="server" Width="180px" MaxLength="150" TabIndex="6" />
                  <label for="lname">Huyện, tỉnh:</label>
                  <asp:TextBox ID="txtHUYEN" runat="server" Width="200px" MaxLength="150" TabIndex="6" />
                  <label for="lname">Số CMND (số KD):</label>
                  <asp:TextBox ID="txtCMND" runat="server" Width="130px" MaxLength="9" TabIndex="7" />      
                  <label for="lname">Cấp ngày:</label>
                  <asp:TextBox ID="txtCAPNGAY" runat="server" Width="100px" MaxLength="15" TabIndex="8" />
                  <label for="lname">Tại:</label>
                  <asp:TextBox ID="txtTAI" runat="server" Width="200px" MaxLength="30" TabIndex="9" />
                  <label for="lname">Khu vực:</label>
                  <asp:DropDownList ID="ddlKHUVUC" Width="142px" AutoPostBack="True" TabIndex="33" runat="server"     />                  
                  <label for="lname">Số HK thường trú:</label>
                  <asp:TextBox ID="txtDIACHIKHAC" runat="server" Width="360px" MaxLength="200" TabIndex="10" />
                  <label for="lname">Số hộ dùng chung:</label>
                  <asp:TextBox ID="txtSOHODN" runat="server" Width="90px" MaxLength="4" TabIndex="11" />
                  <label for="lname">Số nhân khẩu:</label>
                  <asp:TextBox ID="txtSONK" runat="server" Width="90px" MaxLength="4" TabIndex="12" />
                  <label for="lname">Mục đích sử dụng:</label>
                  <asp:DropDownList ID="ddlMUCDICH" Width="130px" TabIndex="13" runat="server"></asp:DropDownList>
                  <label for="lname">HT thanh toán:</label>
                  <asp:DropDownList ID="ddlHTTT" Width="100px" runat="server" TabIndex="14"></asp:DropDownList>
                  <label for="lname">Địa chỉ lắp đặt:</label>
                  <asp:TextBox ID="txtDIACHILAPDAT" runat="server" Width="180px" MaxLength="200" TabIndex="15" />                  
                  <label for="lname">Huyện, tỉnh ĐC lắp:</label>
                  <asp:TextBox ID="txtHUYENDLLAP" runat="server" Width="200px" MaxLength="200" TabIndex="15" />
                  <label for="lname">Điện thoại:</label>
                  <asp:TextBox ID="txtDIENTHOAI" runat="server" Width="90px" MaxLength="20" TabIndex="16" />
                  <label for="lname">MST:</label>
                  <asp:TextBox ID="txtMST" runat="server" Width="90px" MaxLength="30" TabIndex="98" />
                  <label for="lname">Ngày nhận đơn:</label>
                  <asp:TextBox ID="txtNGAYCD" runat="server" MaxLength="200" TabIndex="17"   Width="100px" />
                  <label for="lname">Nơi lắp đồng hồ nước:</label>
                  <asp:TextBox ID="txtNOILAPDHN" runat="server" Width="410px" MaxLength="200" TabIndex="15" />

              </div>
              <asp:Button ID="btSAVE" runat="server" Text="Lưu"  OnClick="btSAVE_Click" />
            </form>
            </div> 

        <br />
    
    <asp:UpdatePanel ID="upnlGrid" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">                
                <eoscrm:Grid ID="gvList" runat="server" UseCustomPager="true" OnRowDataBound="gvList_OnRowDataBound"  CssClass="crmgrid" AutoGenerateColumns="false"
                   OnRowCommand="gvList_RowCommand" OnPageIndexChanging="gvList_PageIndexChanging" PageSize="20">
                    <RowStyle CssClass="row" />
                    <AlternatingRowStyle CssClass="altrow" />
                    <PagerSettings FirstPageText="đơn" PageButtonCount="2" />
                    <Columns>                                   
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã đơn&nbsp;" >
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkBtnID" runat="server" CommandArgument='<%# Eval("MADDK") %>'
                                    CommandName="EditHoSo" Text='<%# Eval("MADDK") %>'></asp:LinkButton>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                            <ItemStyle Font-Bold="true" />
                        </asp:TemplateField>
                        <asp:BoundField HeaderStyle-Width="34%" HeaderText="Tên khách hàng" DataField="TENKH" >                        
                        <HeaderStyle Width="34%" />
                        </asp:BoundField>
                        <asp:BoundField HeaderStyle-Width="34%" HeaderText="Địa chỉ lắp đặt" DataField="DIACHILD" >
                        <HeaderStyle Width="34%" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Ngày đăng ký&nbsp;">
                            <ItemTemplate>
                                <%# (Eval("NGAYDK") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("NGAYDK"))
                                        : "" %>
                            </ItemTemplate>
                            <HeaderStyle Width="10%" />
                        </asp:TemplateField>
                    </Columns>
               </eoscrm:Grid>
           </div>
           <br />
           <div class="crmcontainer p-5">
                <a href="../ThietKe/KhaoSatThietKe.aspx">Chuyển sang bước kế tiếp</a>
           </div>
        </ContentTemplate>
    </asp:UpdatePanel>

    </div>
    </form>
    
</body>
</html>
