<%@ Page Language="C#" MasterPageFile="~/Shared/EOS.master" AutoEventWireup="True"
    Inherits="EOSCRM.Web.Forms.CongNo.PhanCongGhiThu" CodeBehind="PhanCongGhiThu.aspx.cs" %>

<%@ Import Namespace="EOSCRM.Web.Common" %>
<%@ Import Namespace="EOSCRM.Util" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="EOSCRM.Controls" Namespace="EOSCRM.Controls" TagPrefix="eoscrm" %>

<asp:Content ID="head" ContentPlaceHolderID="headCPH" runat="server">
     <script type="text/javascript">
         function onChangeEventHandler(ddlMANVGId, ddlMANVTId, hfGTId) {
             var hfvalue = $("#" + hfGTId).val();
             var idArr = hfvalue.split("<%= DELIMITER.Delimiter %>");
             var nam = idArr[0];
             var thang = idArr[1];
             var madp = idArr[2];
             var duongphu = idArr[3];
             var makv = idArr[4];
             var manvg = $("#" + ddlMANVGId + " option:selected").val();
             var manvt = $("#" + ddlMANVTId + " option:selected").val();
             
             /*
                                
                cau truc du lieu trong hidden field:
                
                <NAM>__crmdelimiter__
                <THANG>__crmdelimiter__
                <MADP>__crmdelimiter__
                <DUONGPHU>__crmdelimiter__
                <MAKV>__crmdelimiter__
                <MANVG>__crmdelimiter__
                <MANVT>
                
             */

             var val = nam + "<%= DELIMITER.Delimiter %>" +
                        thang + "<%= DELIMITER.Delimiter %>" +
                        madp + "<%= DELIMITER.Delimiter %>" +
                        duongphu + "<%= DELIMITER.Delimiter %>" +
                        makv + "<%= DELIMITER.Delimiter %>" +
                        manvg + "<%= DELIMITER.Delimiter %>" + manvt;

             // save to hidden field
             $("#" + hfGTId).val(val);
         }

         function CheckFormSave() {
             openWaitingDialog();
             unblockWaitingDialog();

             __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSave) %>', '');

             return false;
         }

         function CheckFormSearch() {
             var nam = jQuery.trim($("#<%= txtNAM.ClientID %>").val());

             if (!IsNumeric(nam) ||
                    parseInt(nam) < 1990 || parseInt(nam) > 2999) {
                 showError('Chọn năm hợp lệ.', '<%= txtNAM.ClientID %>');
                 return false;
             }

             openWaitingDialog();
             unblockWaitingDialog();

             __doPostBack('<%= CommonFunc.UniqueIDWithDollars(btnSearch) %>', '');

             return false;
         }
     </script>
</asp:Content>
<asp:Content ID="content" ContentPlaceHolderID="ContentPlaceHolderMain" runat="server">
    <asp:UpdatePanel ID="upnlGhiChiSo" UpdateMode="Conditional" runat="server">
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
                                    <asp:TextBox ID="txtNAM" runat="server" Width="30px" MaxLength="4" TabIndex="2" />
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
                            <td class="crmcell right">
                                Nhân viên
                            </td>
                            <td class="crmcell">    
                                <div class="left">
                                    <asp:DropDownList ID="ddlNHANVIEN" runat="server"></asp:DropDownList>
                                </div>
                            </td>
                        </tr>
                        <tr>    
                            <td class="crmcell right"></td>
                            <td class="crmcell"> 
                                <div class="left">
                                    <%--<asp:Button ID="btnSearch" OnClick="btnSearch_Click" runat="server" OnClientClick="retun CheckFormSearch();"  
                                        UseSubmitBehavior="false" CssClass="filter" TabIndex="12" />--%>
                                    <asp:Button ID="btnSearch" runat="server" CommandArgument="Search" CssClass="filter"
                                        OnClick="btnSearch_Click" OnClientClick="return CheckFormSearch();" TabIndex="13" UseSubmitBehavior="false" />
                                </div>   
                                <div class="left">
                                    <asp:Button ID="btnSave" runat="server" CommandArgument="Insert" CssClass="save"
                                        OnClick="btnSave_Click" OnClientClick="return CheckFormSave();" TabIndex="13" UseSubmitBehavior="false" />
                                </div>
                            </td>
                        </tr>
                    </tbody>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:UpdatePanel ID="upnlGhiThu" UpdateMode="Conditional" runat="server">
        <ContentTemplate>
            <br />
            <div class="crmcontainer" id="divGhiThu" runat="server" visible="false"><eoscrm:Grid 
                ID="gvDuongPho" runat="server" UseCustomPager="true"
                OnRowDataBound="gvDuongPho_RowDataBound" PageSize="2000">
                <PagerSettings FirstPageText="đường phố" PageButtonCount="2" />
                <Columns>
                    <asp:TemplateField HeaderStyle-CssClass="checkbox" HeaderText="#">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1%>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="12%" HeaderText="Mã đường phố">
                        <ItemTemplate>  
                            <%# Eval("MADP") %>
                            
                            <%--
                                
                            cau truc du lieu trong hidden field:
                            
                            <NAM>__crmdelimiter__
                            <THANG>__crmdelimiter__
                            <MADP>__crmdelimiter__
                            <DUONGPHU>__crmdelimiter__
                            <MAKV>__crmdelimiter__
                            <MANVG>__crmdelimiter__
                            <MANVT>
                            
                            --%>
                            
                            <asp:HiddenField id="hfGT" runat="server" value='<%# 
                                    Eval("NAM") + DELIMITER.Delimiter + 
                                    Eval("THANG") + DELIMITER.Delimiter + 
                                    Eval("MADP") + DELIMITER.Delimiter + 
                                    (Eval("DUONGPHU").ToString() == "" ? DELIMITER.NullString + DELIMITER.Delimiter : Eval("DUONGPHU") + DELIMITER.Delimiter) + 
                                    Eval("MAKV") + DELIMITER.Delimiter + 
                                    Eval("MANVG") + DELIMITER.Delimiter + 
                                    Eval("MANVT")
                             %>' />
                        </ItemTemplate>
                        <ItemStyle Font-Bold="True" />
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderStyle-Width="22%" HeaderText="Tên đường phố">
                        <ItemTemplate>  
                            <%# Eval("DUONGPHO.TENDP") %>
                        </ItemTemplate>
                        <ItemStyle Font-Bold="True" />
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderStyle-Width="20%" HeaderText="Khu vực">
                        <ItemTemplate>  
                            <%# Eval("KHUVUC.TENKV") %>
                        </ItemTemplate>
                    </asp:TemplateField> 
                    <asp:TemplateField HeaderStyle-Width="22%" HeaderText="Nhân viên ghi">
                        <ItemTemplate>  
                            <asp:DropDownList ID="ddlMANVG" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="22%" HeaderText="Nhân viên thu">
                        <ItemTemplate>  
                            <asp:DropDownList ID="ddlMANVT" runat="server">
                            </asp:DropDownList>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </eoscrm:Grid></div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
