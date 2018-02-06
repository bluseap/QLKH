using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Web.UI;
using System.Data;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.VayCongDoan
{
    public partial class NhanVienVayTK : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        
        private readonly NhanVienVayDao _nvvDao = new NhanVienVayDao();
        private readonly KyTietKiemDao _ktkDao = new KyTietKiemDao();

        private readonly KhuVucDao _kvDao = new KhuVucDao();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_VAY_DSNHANVIENVAY;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_VAYCONGDOAN;
                header.TitlePage = Resources.Message.PAGE_VAY_DSNHANVIENVAY;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadStaticReferences()
        {
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            ddlTHANG2.SelectedIndex = DateTime.Now.Month - 2;
            txtNAM.Text = DateTime.Now.Year.ToString();
            
        }


        private bool BindData()
        {            

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            var list = _nvvDao.GetListKyVay(kynay);

            if (list == null || list.Count.Equals(0) )
            {

                CloseWaitingDialog();
                ShowInfor("Xin chọn kỳ cho đúng.");
                return false;
            }
            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();

            //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
            //gvList.Enabled = !gcsDao.IsLockTinhCuoc(kynay, dp);
            divList.Visible = true;
            //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

            BindTongTien(kynay);

            upnlGrid.Update();

            return true;
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindData();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*
            <asp:HiddenField id="hfGCS" runat="server" value='<%# 
                                        Eval("MANVV") + DELIMITER.Delimiter + 
                                        Eval("NAM") + DELIMITER.Delimiter + 
                                        Eval("TIENGOC") + DELIMITER.Delimiter + 
                                        Eval("TIENLAI") + DELIMITER.Delimiter 
                                 %>' /> 
                                     */
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;

            var ddlHTTT = e.Row.FindControl("ddlHTTT") as DropDownList;
            var ddlTHANHTOAN = e.Row.FindControl("ddlTHANHTOAN") as DropDownList;
            var txtTIENGOC = e.Row.FindControl("txtTIENGOC") as TextBox;
            var txtTIENLAI = e.Row.FindControl("txtTIENLAI") as TextBox;

            if (hfGCS == null || ddlHTTT == null || ddlTHANHTOAN == null || txtTIENGOC == null || txtTIENLAI == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtTIENGOC.ClientID +
                                                                "\", \"" + txtTIENLAI.ClientID +
                                                                "\", \"" + ddlHTTT.ClientID +
                                                                "\", \"" + ddlTHANHTOAN.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            txtTIENGOC.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtTIENLAI.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            ddlHTTT.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            ddlTHANHTOAN.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");


            txtTIENGOC.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtTIENGOC.ClientID + "\");");
            txtTIENLAI.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtTIENLAI.ClientID + "\");");
            ddlHTTT.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + ddlHTTT.ClientID + "\");");
            ddlTHANHTOAN.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + ddlTHANHTOAN.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + txtTIENGOC.ClientID +
                                                                "\", \"" + txtTIENLAI.ClientID +
                                                                "\", \"" + ddlHTTT.ClientID +
                                                                "\", \"" + ddlTHANHTOAN.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            //txtCHISODAU.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
            ddlHTTT.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
            ddlTHANHTOAN.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");



            var source = gvList.DataSource as List<VAYKYNHANVIEN>;
            if (source == null) return;
            var imgTT = e.Row.FindControl("imgTT") as Button;
            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var kyvay = _nvvDao.GetNVVayKy(source[index].MANVV, source[index].NAM, source[index].THANG);
                
                string maudo = "rejectIndicator";
                string mauxanh = "approveIndicator";
                string maucam = "inprogressIndicator";

                if (kyvay != null)
                {
                    if (kyvay.THANHTOAN == "CO")
                    {
                        if (imgTT != null)
                        {
                            imgTT.Attributes.Add("class", maudo);
                            imgTT.ToolTip = "Chưa đóng";
                        }
                    }
                    else if (kyvay.THANHTOAN == "NO" && kyvay.TATTOAN == "TA")
                    {

                        if (imgTT != null)
                        {
                            imgTT.Attributes.Add("class", maucam);
                            imgTT.ToolTip = "Tất toán";
                        }
                    }
                    else
                    {
                        if (imgTT != null)
                        {
                            imgTT.Attributes.Add("class", mauxanh);
                            imgTT.ToolTip = "Đóng rồi";
                        }
                    }
                }               
                
            }
            catch { }

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var nam = int.Parse(txtNAM.Text.Trim());
            var thang = int.Parse(ddlTHANG.SelectedValue);
            //var kv = ddlKHUVUC.SelectedValue;

            //var msg = gcsDao.TinhTien(thang, nam, kv);

            CloseWaitingDialog();
            //ShowInfor(ResourceLabel.Get(msg));
        }

        protected void txtNAM_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            gvList.PageIndex = 0;

            if (!BindData())
            {
                CloseWaitingDialog();
                ShowError("Chọn kỳ vay cho đúng.");
                return;
            }           

            CloseWaitingDialog();
        }

        protected void BindTongTien(DateTime kynay)
        {
            if (_nvvDao.SumTongTienGocKy(kynay) != null)
            {
                int sumgoc = Int32.Parse(_nvvDao.SumTongTienGocKy(kynay).ToString() == "" ? "0" : _nvvDao.SumTongTienGocKy(kynay).ToString());
                lblTONGTIENGOC.Text = sumgoc.ToString("#,###");
            }
            else {lblTONGTIENGOC.Text = "0";}

            if (_nvvDao.SumTongTienLaiKy(kynay) != null)
            {
                int sumlai = (int) _nvvDao.SumTongTienLaiKyInt(kynay);
                lblTONGTIENLAI.Text = sumlai.ToString("#,###");
            }
            else {lblTONGTIENLAI.Text = "0";}

            if (_nvvDao.SumTongTienKy(kynay) != null)
            {
                int sumttky = Int32.Parse(_nvvDao.SumTongTienKy(kynay).ToString() == "" ? "0" : _nvvDao.SumTongTienKy(kynay).ToString());
                lbTONGTHU.Text = sumttky.ToString("#,###");
            }
            else {lbTONGTHU.Text = "0";}

            if (_nvvDao.SumTongTienChuaThuKy(kynay) != null)
            {
                try
                {
                    int sumchuaky = Int32.Parse((_nvvDao.SumTongTienChuaThuKy(kynay).ToString() == "") ? "0" : _nvvDao.SumTongTienChuaThuKy(kynay).ToString());
                    lbTONGTIENCHUATHU.Text = sumchuaky.ToString("#,###");
                }
                catch { }
            }
            else { lbTONGTIENCHUATHU.Text = "0";}

            if (_nvvDao.SumTongChiConLaiKy(kynay) != null)
            {
                int sumchiconlai = Int32.Parse(_nvvDao.SumTongChiConLaiKy(kynay).ToString() == "" ? "0" : _nvvDao.SumTongChiConLaiKy(kynay).ToString());
                lbTIENCHICONLAI.Text = sumchiconlai.ToString("#,###");
            }
            else { lbTIENCHICONLAI.Text = "0";}



            if ( _ktkDao.GetSumTienThu2() != null)
            {
                int sumtt2 = Int32.Parse(_ktkDao.GetSumTienThu2().ToString() == "" ? "0" : _ktkDao.GetSumTienThu2().ToString());
                lbTHUTIETKIEM.Text = sumtt2.ToString("#,###");
            }
            else { lbTHUTIETKIEM.Text = "0";}

            if ( _nvvDao.SumTongTienLai() != null)
            {
                int sumttl2 = (int) _nvvDao.SumTongTienLaiInt();
                lbTHUTIENLAI.Text = sumttl2.ToString("#,###");
            }
            else {lbTHUTIENLAI.Text = "0";}

            //var kynaym = new DateTime(int.Parse(DateTime.Now.Year.ToString()), int.Parse((DateTime.Now.Month - 1).ToString()), 1);
            if (_nvvDao.SumConLaiKyMax() != null && _ktkDao.GetSumTienThu2() != null)
            {
                int tienthu = Int32.Parse(_ktkDao.GetSumTienThu2().ToString() == "" ? "0" : _ktkDao.GetSumTienThu2().ToString());
                int conlaiky = Int32.Parse(_nvvDao.SumConLaiKyMax().ToString() == "" ? "0" : _nvvDao.SumConLaiKyMax().ToString());

                lbTONGTIENCONLAI.Text = (tienthu - conlaiky).ToString("#,###");
                lbCHICONLAI.Text = conlaiky.ToString("#,###");
            }
            else { 
                lbTONGTIENCONLAI.Text = "0";
                lbCHICONLAI.Text = "0";
            }


            

        }



    }
}