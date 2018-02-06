<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="TachDuongPhoPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.TachDuongPhoPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">      
        $(document).ready(function() {
            $("#divTachDuong").dialog({
                autoOpen: false,
                modal: true,
                minHeight: 100,
                height: 'auto',
                width: 'auto',
                resizable: false,
                open: function(event, ui) {
                    $(this).parent().appendTo("#divTachDuongNDlgContainer");
                }
            });
            
        });

        function CheckFormTaiDuongPho() {
            //openWaitingDialog();
            //unblockWaitingDialog();
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btTAIDUONGPHO) %>', '');
            return false;
        }

        function CheckFormUpDuongPho() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btUpDuongPho) %>', '');
            return false;
        }       

        function CheckFormlkXemFileUp() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(lkXemFileUp) %>', '');
            return false;
        }

        function CheckFormLoc() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btLOC) %>', '');
            return false;
        }

        function CheckFormbtLuuSuaTDN() {            
            __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btLuuSuaTDN) %>', '');
            return false;
        }
    </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <div id="divTachDuongNDlgContainer">
        <div id="divTachDuong" style="display: none">
            <asp:UpdatePanel ID="updivTachDuong" runat="server" UpdateMode="Conditional">
				<ContentTemplate>
                    <table cellpadding="3" cellspacing="1" style="width: 600px;">          
                        <tr>
                            <td class="crmcontainer">
                                <table class="crmtable">
                                    <tbody>              
                                        <tr>
                                            <td class="crmcell right"> Tên </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Label ID="lbTENKH" runat="server" Font-Bold="True" ></asp:Label>
                                                    <asp:Label ID="lbdivIDMATACH" runat="server" Visible="false"></asp:Label>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"> Mã DP Cũ </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtMADPCUTD" runat="server" Width="70px" Enabled="false"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td class="crmcell right"> Mã DB Cũ </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtMADBCUTD" runat="server" Width="70px" Enabled="false"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="crmcell right"> Mã DP Mới </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtMADPMOITD" runat="server" Width="70px"></asp:TextBox>
                                                </div>
                                            </td>
                                            <td class="crmcell right"> Mã DB Mới </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:TextBox ID="txtMADBMOITD" runat="server" Width="70px"></asp:TextBox>
                                                </div>
                                            </td>
                                        </tr>                                       
                                        <tr>
                                            <td class="crmcell right">  </td>
                                            <td class="crmcell">
                                                <div class="left">
                                                    <asp:Button ID="btLuuSuaTDN" runat="server" CssClass="myButton" Text="Lưu"
                                                        OnClick="btLuuSuaTDN_Click" UseSubmitBehavior="false" 
                                                        OnClientClick="return CheckFormbtLuuSuaTDN();"  />
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

    <asp:UpdatePanel ID="upnlThongTin" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right">Kỳ </td>
                            <td class="crmcell">
                                <div class="left">
                                   <asp:DropDownList ID="ddlTHANG" runat="server" >
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="40px" MaxLength="4" />
                                </div>
                                <div class="left">
                                    <asp:Button ID="btLOC" runat="server" CssClass="filter" OnClientClick="return CheckFormLoc();" 
                                        TabIndex="9" UseSubmitBehavior="false" OnClick="btLOC_Click" />
                                </div>
                                <td class="crmcell right">Khu vực</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:DropDownList ID="ddlKHUVUC" runat="server"></asp:DropDownList>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đường phố</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlDUONGPHO" runat="server"></asp:DropDownList>
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Button ID="btTAIDUONGPHO" runat="server" CssClass="myButton" Text="Tải về đường phố" OnClick="btTAIDUONGPHO_Click"
                                            UseSubmitBehavior="false" OnClientClick="return CheckFormTaiDuongPho();" />
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">Đợt GCS</td>
                            <td class="crmcell">
                                <div class="left" >
                                    <asp:DropDownList ID="ddlDOTGCS" runat="server"></asp:DropDownList>
                                </div>
                                <td class="crmcell right">Phiên (LX)</td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:DropDownList ID="ddlPHIENLX" runat="server">
                                            <asp:ListItem Value ="%">Tất cả</asp:ListItem>
                                            <asp:ListItem Value="1">1</asp:ListItem>
                                            <asp:ListItem Value="2">2</asp:ListItem>
                                            <asp:ListItem Value="3">3</asp:ListItem>
                                            <asp:ListItem Value="4">4</asp:ListItem>
                                            <asp:ListItem Value="5">5</asp:ListItem>
                                            <asp:ListItem Value="6">6</asp:ListItem>
                                            <asp:ListItem Value="A">A</asp:ListItem>
                                        </asp:DropDownList>
                                    </div>
                                </td>                           
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Chọn file đường phố cần up</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:FileUpload ID="fileUploadDuongPho" type="file" runat="server" />
                                    <asp:Label ID="lbMAUPDP" runat="server" Visible="False" ></asp:Label>
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell">
                                    <div class="left">
                                         <asp:Button ID="btUpDuongPho" runat="server" CssClass="myButton" Text="Upload đường phố" 
                                           OnClick="btUpDuongPho_Click" UseSubmitBehavior="false"  />
                                       <%-- <asp:Button ID="btUpDuongPho" runat="server" CssClass="myButton" Text="Upload đường phố" 
                                           OnClick="btUpDuongPho_Click" UseSubmitBehavior="false" OnClientClick="return CheckFormUpDuongPho();"  />--%>
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right"> </td>
                            <td class="crmcell">
                                <div class="left" >
                                    <asp:LinkButton ID="lkXemFileUp" runat="server" OnClick="lkXemFileUp_Click"
                                        UseSubmitBehavior="false"  OnClientClick="return CheckFormlkXemFileUp();"
                                        >Xem file Upload</asp:LinkButton>
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btTAIDUONGPHO" /> 
            <asp:PostBackTrigger ControlID="btUpDuongPho" /> 
        </Triggers> 
    </asp:UpdatePanel>
    <br />       
    <eoscrm:Grid ID="gvUploadFle" runat="server" UseCustomPager="true" PageSize="2" 
        AutoGenerateColumns="False" DataKeyNames="TENPATH" EnableModelValidation="True"   
        OnRowDataBound="gvUploadFle_RowDataBound" OnRowCommand="gvUploadFle_RowCommand" 
		    OnPageIndexChanging="gvUploadFle_PageIndexChanging"     > 
    <PagerSettings FirstPageText="kỳ" PageButtonCount="2" />
    <Columns>
        <asp:TemplateField HeaderStyle-Width="5%" HeaderText="#">
            <ItemTemplate>
                <%# Container.DataItemIndex + 1%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã KH">
                <ItemTemplate>
                    <asp:LinkButton ID="lkMAUPDP" runat="server" CommandName="SMaupDP" 
                        CommandArgument='<%# Eval("MAUPDP") %>'                          
                        Text='<%# Eval("MAUPDP").ToString()  %>'></asp:LinkButton>
                </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-Width="55%" HeaderText="Tên file">
            <ItemTemplate>
                <%# Eval("TENFILE").ToString() %>                                                
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton ID="lkDownloadDP" runat="server" Text="Download" OnClick="lkDownloadDP_Click"    ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>
    </Columns>
    </eoscrm:Grid>
    <br />
    <asp:UpdatePanel ID="upTachDuong" runat="server" UpdateMode="Conditional">
		<ContentTemplate>               
			<div class="crmcontainer" id="divlistUpDPN" runat="server" >
                <table class="crmtable">
                    <tbody>
                        <tr>
                            <td class="crmcell right"></td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btDSTachDuongKH" runat="server" CssClass="myButton" Text="Bắt đầu tách đường" OnClick="btDSTachDuongKH_Click"/>                                    
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Button ID="btDSTachDuong" runat="server" CssClass="myButton" Text="DS tách đường TĐCT" OnClick="btDSTachDuong_Click" />
                                    </div>
                                </td>
                            </td>
                        </tr>
                    </tbody>
                </table>
                <eoscrm:Grid ID="gvUpDPN" runat="server" UseCustomPager="true" PageSize="500" AutoGenerateColumns="False" 
                    EnableModelValidation="True"  OnRowDataBound="gvUpDPN_RowDataBound" OnRowCommand="gvUpDPN_RowCommand" 
                    OnPageIndexChanging="gvUpDPN_PageIndexChanging"     > 
                    <PagerSettings FirstPageText="kỳ" PageButtonCount="2" />
                    <Columns>
                        <asp:TemplateField HeaderStyle-Width="10px" HeaderText="#">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="40px" HeaderText="Mã ">
                            <ItemTemplate>
                                <asp:LinkButton ID="lkIDTACH" runat="server"
                                    CommandName="SIDTACH" CommandArgument='<%# Eval("IDTACH") %>'
                                    OnClientClick="openDialogAndBlock('Sửa tách đường', 400, 'divTachDuong')" 
                                    Text='<%# Eval("IDTACH").ToString()  %>'></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="150px" HeaderText="Tên KH">
                            <ItemTemplate>
                                <%# Eval("TENKH").ToString() %>                                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" HeaderText="MADP Cũ">
                            <ItemTemplate>
                                <%# Eval("MADPPOCU").ToString() %>                                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="MADB Cũ">
                            <ItemTemplate>
                                <%# Eval("MADBPOCU").ToString() %>                                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="30px" HeaderText="MADP Moi">
                            <ItemTemplate>
                                <%# Eval("MADPPOMOI").ToString() %>                                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderStyle-Width="50px" HeaderText="MADB Moi">
                            <ItemTemplate>
                                <%# Eval("MADBPOMOI").ToString() %>                                                
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </eoscrm:Grid>
            </div>
		</ContentTemplate> 
        <Triggers>
            <asp:PostBackTrigger ControlID="btDSTachDuong" />             
        </Triggers> 
                       
	</asp:UpdatePanel>
</asp:Content>
