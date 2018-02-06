<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/EOS.Master" AutoEventWireup="true" CodeBehind="UpLoadFile.aspx.cs" Inherits="EOSCRM.Web.Forms.DanhMuc.UpLoadFile" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>


<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            $("#divNhanVien").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function (event, ui) {
                    $(this).parent().appendTo("#divNhanVienDlgContainer");
                }
            });
        });

        function CheckFormSave() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

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

        function CheckFormFilter() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilter) %>', '');

            return false;
        }

        function CheckFormFilterNV() {
            openWaitingDialog();
            unblockWaitingDialog();

            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnFilterNV) %>', '');

            return false;
        }

       
        CheckFormFilterNV
    </script>
   <style type="text/css">
        .modalBackground
        {
        background-color: Gray;
        filter: alpha(opacity=80);
        opacity: 0.8;
        z-index: 10000;
        }

        .GridviewDiv {font-size: 100%; font-family: 'Lucida Grande', 'Lucida Sans Unicode', Verdana, Arial, Helevetica, sans-serif; color: #303933;}
        Table.Gridview{border:solid 1px #df5015;}
        .Gridview th{color:#FFFFFF;border-right-color:#abb079;border-bottom-color:#abb079;padding:0.5em 0.5em 0.5em 0.5em;text-align:center}
        .Gridview td{border-bottom-color:#f0f2da;border-right-color:#f0f2da;padding:0.5em 0.5em 0.5em 0.5em;}
        .Gridview tr{color: Black; background-color: White; text-align:left}
        :link,:visited { color: #DF4F13; text-decoration:none }

    </style>
   

</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divNhanVienDlgContainer">
        <div id="divNhanVien" style="display: none">
            <asp:UpdatePanel ID="upnlNhanVien" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
				    <table cellpadding="3" cellspacing="1" style="width: 800px;">
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
                                                    <asp:TextBox ID="txtKeywordNV" onchange="return CheckFormFilterNV();" runat="server" Width="250px" MaxLength="200" />
                                                </div>
                                                <div class="left">
                                                    <asp:Button ID="btnFilterNV" OnClick="btnFilterNV_Click"
                                                        UseSubmitBehavior="false" OnClientClick="return CheckFormFilterNV();" 
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
							        <eoscrm:Grid ID="gvNhanVien" runat="server" UseCustomPager="true"  AllowPaging="true" AutoGenerateColumns="false" CssClass="crmgrid"
							            OnRowDataBound="gvNhanVien_RowDataBound" OnRowCommand="gvNhanVien_RowCommand" 
							            OnPageIndexChanging="gvNhanVien_PageIndexChanging">
                                        <RowStyle CssClass="row" />
                                        <AlternatingRowStyle CssClass="altrow" />
                                        <HeaderStyle CssClass="header" />
                                        <PagerSettings FirstPageText="nhân viên" PageButtonCount="2" />
                                        <Columns>
                                            <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã NV">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lnkBtnID" runat="server" 
                                                        CommandArgument='<%# Eval("MANV") %>' CommandName="SelectMANV"                                                         
                                                        Text='<%# HttpUtility.HtmlEncode(Eval("MANV").ToString()) %>'></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField HeaderStyle-Width="35%" DataField="HOTEN" HeaderText="Họ tên" />
                                            <asp:TemplateField HeaderStyle-Width="30%" HeaderText="Phòng ban">
                                                <ItemTemplate>
                                                    <%# Eval("PHONGBAN") != null ? Eval("PHONGBAN.TENPB") : "" %>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderStyle-Width="25%" HeaderText="Công việc">
                                                <ItemTemplate>
                                                    <%# Eval("CONGVIEC") != null ? Eval("CONGVIEC.TENCV") : ""%>
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
    
    <asp:UpdatePanel ID="upnlInfor" UpdateMode="Conditional" runat="server">
        <ContentTemplate> 
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>    
                            <td class="crmcell right">Chọn file cần up</td>
                            <td class="crmcell"> 

                                <div class="left">
                                    <asp:FileUpload ID="fileUpload1" type="file" runat="server" /> <br />
                                </div>
                                <div class="left">                                    
                                    <asp:Button ID="btnSave" OnClick="btnSave_Click"
                                        UseSubmitBehavior="false" OnClientClick="return CheckFormSave();"
                                        runat="server" CssClass="save" Text="" TabIndex="12" />                                   
                                </div> 
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell">                                
                                
                                <div class="left">
                                    <asp:Button ID="btnFilter" OnClick="btnFilter_Click"
                                        UseSubmitBehavior="false"
                                        runat="server" CssClass="filter" Text="" TabIndex="12" Visible="False" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnDelete" runat="server" CssClass="delete" UseSubmitBehavior="false" 
                                        TabIndex="13" OnClientClick="return CheckFormDelete();" OnClick="btnDelete_Click" Visible="False" />
                                </div>   
                                                           
                            </td>
                        </tr>   
                        
                        <tr>
                            <td class="header">Chọn nơi cần gửi file</td> 
		                </tr>
                                             
                        <tr>    
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" runat="server" Width="262px" TabIndex="3" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Phòng ban</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlPHONGBAN" Width="262px" runat="server" TabIndex="4" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Nhân viên cần gửi file</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:TextBox ID="txtMANV" runat="server" Width="90px" Readonly="true"
                                        onkeypress="return CheckFormMANVKeyPress(event);"
                                        MaxLength="200" TabIndex="4" />
                                    <asp:LinkButton ID="linkBtnMANV" CausesValidation="false" style="display:none"  
                                            OnClick="linkBtnMANV_Click" runat="server">Change MANV</asp:LinkButton>
                                </div>
                                <div class="left">
                                    <asp:Button ID="btnBrowseNhanVien" runat="server" CssClass="pickup" 
                                        OnClick="btnBrowseNhanVien_Click" OnClientClick="openDialogAndBlock('Chọn từ danh sách nhân viên', 800, 'divNhanVien')"
                                        UseSubmitBehavior="false" CausesValidation="false" />
                                </div>
                                <div class="left">
                                    <strong>Tên nhân viên</strong>
                                </div>
                                <div class="left">
                                    <asp:TextBox ID="txtTENNV" runat="server" Width="250px" MaxLength="200" TabIndex="4" ReadOnly="true" />
                                </div>
                                <div class="left filtered"></div>
                            </td>
                        </tr>
                        <tr>
                <td class="crmcell right"></td>   
                <td>
                    <div ><i class="fa fa-spinner fa-2x fa-pulse"></i>

                        
                                        <asp:LinkButton ID="linkTKMAU"  runat="server" class="myButton"
                                             OnClick="linkSETNFILE_Click"  >
                                            Báo cáo
                                        </asp:LinkButton>
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
    <eoscrm:Grid ID="gvDetails" runat="server" UseCustomPager="true"  AutoGenerateColumns="False" DataKeyNames="TENPATH" EnableModelValidation="True"    
        AllowPaging="false"  CssClass="crmgrid" PageSize=100>
            <RowStyle CssClass="row" />
            <AlternatingRowStyle CssClass="altrow" />
            <HeaderStyle CssClass="header" />
            <Columns>
                <asp:TemplateField HeaderStyle-CssClass="checkbox">
                            <HeaderTemplate>
                                <input id="chkAllTop" title="Chọn hết / Bỏ chọn hết" name="chkAllTop" type="checkbox"
                                    onclick="CheckAllItems(this);" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                <input id="Id" runat="server" type="hidden" value='<%# Eval("MAUPLOAD") %>' />
                                <input name="listIds" type="checkbox" value='<%# Eval("MAUPLOAD") %>' />
                            </ItemTemplate>
                    <HeaderStyle CssClass="checkbox"></HeaderStyle>
                 </asp:TemplateField>
                <asp:BoundField DataField="TENFILE" HeaderText="Tên file" >
                <ItemStyle Font-Bold="True" />
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                    <ItemTemplate>
                    <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" OnClick="lnkDownload_Click"></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Ngày up file" HeaderStyle-Width="100px">
                            <ItemTemplate>
                                <%# (Eval("DATE") != null) ?
                                        String.Format("{0:dd/MM/yyyy}", Eval("DATE"))
                                        : "" %>
                            </ItemTemplate>
                    <HeaderStyle Width="100px"></HeaderStyle>
                </asp:TemplateField>               
            </Columns>
    </eoscrm:Grid>  
</asp:Content>
