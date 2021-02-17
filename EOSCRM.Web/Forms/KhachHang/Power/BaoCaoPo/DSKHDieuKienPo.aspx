<%@ Page Title="" Language="C#" MasterPageFile="~/Shared/PO.Master" AutoEventWireup="true" CodeBehind="DSKHDieuKienPo.aspx.cs" Inherits="EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo.DSKHDieuKienPo" %>

<%@ Import Namespace="EOSCRM.Web.Common"%>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>
<%@ Register assembly="CrystalDecisions.Web, Version=13.0.4000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" namespace="CrystalDecisions.Web" tagprefix="CR" %>

<%@ Import Namespace="EOSCRM.Util" %>
<%@ Import Namespace="EOSCRM.Dao" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
    <script type="text/javascript">        
       
    </script>
</asp:Content>

<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server"> 
    <asp:UpdatePanel ID="upDSKHDieuKienPo" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="35px" MaxLength="4" TabIndex="2" />
                                    <asp:Label ID="lbReLoad" runat="server" Visible="false"></asp:Label>
                                </div>
                            </td>
                        </tr>
                        <tr>                           
                            <td class="crmcell right">Khu vực</td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlKHUVUC" runat="server" >
                                    </asp:DropDownList>
                                </div>                                                             
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                                Đường phố
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:DropDownList ID="ddlMaDuongPho" runat="server" />                                    
                                </div> 
                                <td class="crmcell right"></td>
                                <td class="crmcell">
                                    <div class="left">
                                        <asp:Button ID="btnDsDuongPhoSoTru" runat="server" CssClass="myButton" OnClick="btnDsDuongPhoSoTru_Click"
                                            Text="Xuất Excel DS Đường phố theo trụ" CausesValidation="false" UseSubmitBehavior="false" />
                                    </div>
                                </td>
                            </td>                            
                        </tr>
                        <tr>    
                            <td class="crmcell right">Chọn file đường phố cần up</td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <asp:FileUpload ID="fileUploadDuongPhoTheoTru" type="file" runat="server" />
                                    <asp:Label ID="lbMAUPDP" runat="server" Visible="False" ></asp:Label>
                                </div>
                                <td class="crmcell right"></td>
                                <td class="crmcell">
                                    <div class="left">
                                         <asp:Button ID="btUpDuongPhoTheoTru" runat="server" CssClass="myButton" Text="Upload đường phố theo trụ" 
                                           OnClick="btUpDuongPhoTheoTru_Click" UseSubmitBehavior="false"  />
                                    </div>
                                </td>
                            </td>
                        </tr>
                        <tr>
                            <td class="crmcell right">
                            </td>
                            <td class="crmcell">
                                <div class="left">
                                    <asp:Button ID="btnLuuDsDuongPhoTheoTru" runat="server" CssClass="myButton" OnClick="btnLuuDsDuongPhoTheoTru_Click"
                                        TabIndex="15" UseSubmitBehavior="false" Text="Lưu Ds Đường phố theo trụ"/>                                    
                                </div>                                
                                <%--<td class="crmcell right">
                                    <div class="left">
                                        <asp:Button ID="btnDSKHCBiKTMDK" runat="server" CssClass="myButton" OnClick="btnDSKHCBiKTMDK_Click"
                                            TabIndex="15" UseSubmitBehavior="false" Text="DS MĐ.Khác"/>                                    
                                    </div>
                                </td>
                                <td class="crmcell"> 
                                    <div class="left">
                                        <asp:Button ID="btDSTGMDK" runat="server" CssClass="myButton" Text="DS TG MĐ.Khác"
                                            TabIndex="15" UseSubmitBehavior="false" OnClick="btDSTGMDK_Click"/>   
                                    </div> 
                                </td>               --%>                                               
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDsDuongPhoSoTru" />
            <asp:PostBackTrigger ControlID="btUpDuongPhoTheoTru" />
            <asp:PostBackTrigger ControlID="btnLuuDsDuongPhoTheoTru" />
        </Triggers>
    </asp:UpdatePanel>
    <br />       
    <eoscrm:Grid ID="gvUploadFle" runat="server" UseCustomPager="true" PageSize="4" 
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
        <asp:TemplateField HeaderStyle-Width="10%" HeaderText="Mã ">
                <ItemTemplate>
                    <asp:LinkButton ID="lkMAUPDP" runat="server" CommandName="SMaupDP" 
                        CommandArgument='<%# Eval("MAUPLOAD") %>'                          
                        Text='<%# Eval("ID").ToString()  %>'></asp:LinkButton>
                </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderStyle-Width="55%" HeaderText="Tên file">
            <ItemTemplate>
                <%# Eval("TENFILE").ToString() %>                                                
            </ItemTemplate>
        </asp:TemplateField>
        <%--<asp:TemplateField HeaderText="">
            <ItemTemplate>
                <asp:LinkButton ID="lkDownloadDP" runat="server" Text="Download" OnClick="lkDownloadDP_Click"    ></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>--%>
    </Columns>
    </eoscrm:Grid>
    <br />
    <asp:UpdatePanel ID="upnlCrystalReport" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <div class="crmcontainer" id="divCR" runat="server" >
                <CR:CrystalReportViewer ID="rpViewer" runat="server" AutoDataBind="true" PrintMode="ActiveX" 
                     />
            </div>
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="rpViewer" />
        </Triggers>
    </asp:UpdatePanel>      
</asp:Content>
