﻿using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe.Power
{
    public partial class TraCuuCTPower : Authentication
    {
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao(); 
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly ChietTinhDao ctDao = new ChietTinhDao();
        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();

        #region Properties

        protected String Keyword
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_KEYWORD))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_KEYWORD].ToString());
            }
        }

        protected String StateCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_STATECODE))
                {
                    return null;
                }
                var res = EncryptUtil.Decrypt(param[Constants.PARAM_STATECODE].ToString());
                return res == "NULL" ? null : res;
            }
        }

        protected String AreaCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_AREACODE))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_AREACODE].ToString());
            }
        }

        protected DateTime? FromDate
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_FROMDATE))
                {
                    return null;
                }
                try
                {
                    return DateTimeUtil.GetVietNamDate(EncryptUtil.Decrypt(param[Constants.PARAM_FROMDATE].ToString()));
                }
                catch
                {
                    return null;
                }
            }
        }

        protected DateTime? ToDate
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_TODATE))
                {
                    return null;
                }
                try
                {
                    return DateTimeUtil.GetVietNamDate(EncryptUtil.Decrypt(param[Constants.PARAM_TODATE].ToString()));
                }
                catch
                {
                    return null;
                }
            }
        }

        protected DONDANGKYPO DonDangKyPo
        {
            get
            {
                try { return (DONDANGKYPO)Session["TCDLDM_DDK"]; }
                catch { return null; }
            }

            set { Session["TCDLDM_DDK"] = value; }
        }

        protected THIETKEPO ThietKePo
        {
            get
            {
                try { return (THIETKEPO)Session["TCDLDM_TK"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_TK"] = value; }
        }

        protected CHIETTINH ChietTinh
        {
            get
            {
                try { return (CHIETTINH)Session["TCDLDM_CT"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_CT"] = value; }
        }

        #endregion

        #region Startup script registeration
        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowInFor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void FocusAndSelect(string controlId)
        {
            ((PO)Page.Master).FocusAndSelect(controlId);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_TraCuuChietTinhPo, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    BindDataForGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_TRACUUCHIETTINHPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_TRACUUCHIETTINHPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvTKVT);
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var nhanvien = _nvDao.Get(b);

                //var objList = _ddkpoDao.GetListForTraCuuChietTinh(Keyword, FromDate, ToDate, StateCode, AreaCode);
                var objList = _ddkpoDao.GetListForTraCuuChietTinh(Keyword, FromDate, ToDate, StateCode, _kvpoDao.GetPo(nhanvien.MAKV).MAKVPO);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];

            var source = gvList.DataSource as List<DUYETCHIETTINHPO>;
            if (source == null) return;

            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;

            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = _ddkpoDao.Get(source[index].MADDKPO);
                var dct = source[index];


                if (imgTT != null && ddk != null)
                {
                    imgTT.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTT) + "')");

                    var maTTCT = dct.TTCT;
                    var ttct = ttDao.Get(maTTCT);

                    if (ttct != null)
                    {
                        imgTT.Attributes.Add("class", ttct.COLOR);
                        imgTT.ToolTip = ttct.TENTT;
                    }
                    else
                    {
                        imgTT.ToolTip = "Chưa duyệt chiết tính";
                        imgTT.Attributes.Add("class", "noneIndicator");
                    }
                }
            }
            catch { }

            var lnkBtnIDReport = e.Row.FindControl("lnkBtnIDReport") as LinkButton;
            if (lnkBtnIDReport == null) return;
            lnkBtnIDReport.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnIDReport) + "')");

            var lnkBtnIDEdit = e.Row.FindControl("lnkBtnIDEdit") as LinkButton;
            if (lnkBtnIDEdit == null) return;
            lnkBtnIDEdit.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnIDEdit) + "')");

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvList.PageIndex = e.NewPageIndex;                
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                var list = _ddkpoDao.Get(id);
                switch (e.CommandName)
                {
                    case "EditItem":
                        if (LoginInfo.MANV == "nguyen")
                        {
                            Session["LAPCHIETTINH_MADDK"] = id;
                            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/Power/LapCTPower.aspx?" + Constants.PARAM_REPORTED + "=true", false);
                            CloseWaitingDialog();
                            break;
                        }
                        else
                        {
                            if (list.TTCT.Equals("CT_A"))
                            {
                                ShowInFor("Chiết tính đã duyệt. Xin chọn Chiết tính chưa duyệt.");
                                CloseWaitingDialog();
                                break;
                            }
                            else
                            {
                                Session["LAPCHIETTINH_MADDK"] = id;
                                Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/Power/LapCTPower.aspx?" + Constants.PARAM_REPORTED + "=true", false);

                                CloseWaitingDialog();

                                break;
                            }
                        }
                    case "ReportItem":
                        Session["LAPCHIETTINH_MADDK"] = id;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpLapChietTinh.aspx", false);

                        CloseWaitingDialog();

                        break;

                    case "showCTStatus":
                        if (!string.Empty.Equals(id))
                        {
                            DonDangKyPo = _ddkpoDao.Get(id);
                            ThietKePo = _tkpoDao.Get(id);
                            ChietTinh = ctDao.Get(id);

                            txtGHICHU.Text = ChietTinh.GHICHU;

                            upnlChietTinh.Update();
                            UnblockDialog("divChietTinh");
                        }

                        CloseWaitingDialog();

                        break;

                    case "SuaTongTien":
                        if (!string.Empty.Equals(id))
                        {
                            lbMADDK.Text = id;
                            var chiettinh = ctDao.Get(lbMADDK.Text.Trim());

                            txtTongTienCT.Text = Convert.ToInt64(chiettinh.TONGTIENCTPS != null ? chiettinh.TONGTIENCTPS : 0).ToString();

                            upTongTienCT.Update();
                        }

                        CloseWaitingDialog();

                        break;

                    case "VatTuThietKe":
                        if (!string.Empty.Equals(id))
                        {
                            var ddk = _ddkpoDao.Get(id);

                            lbTENKHVTTK.Text = ddk.TENKH;
                            lbMADDKVTTK.Text = id;

                            BindTKVT(id);

                            upnlThietKeVatTu.Update();
                            UnblockDialog("divThietKeVatTu");
                        }

                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void Clear()
        {
            try
            {
                txtTongTienCT.Text = "";

                upTongTienCT.Update();
            }
            catch { }
        }

        protected void btSaveTongTienCT_Click(object sender, EventArgs e)
        {
            try
            {
                var chiettinh = ctDao.Get(lbMADDK.Text.Trim());

                chiettinh.TONGTIENCTPS = Convert.ToDecimal(txtTongTienCT.Text.Trim());

                var msg = ctDao.Update(chiettinh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBienPo(lbMADDK.Text.Trim(), LoginInfo.MANV, _nvDao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", txtTongTienCT.Text.Trim(), "", "UPTONGTIENCT");

                Clear();
                CloseWaitingDialog();
            }
            catch { }
        }

        private void BindTKVT(string madon)
        {
            var list = _cttkDao.GetList(madon);

            gvTKVT.DataSource = list;
            gvTKVT.PagerInforText = list.Count.ToString();
            gvTKVT.DataBind();
        }

        protected void gvTKVT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTKVT.PageIndex = e.NewPageIndex;
                BindTKVT(lbMADDKVTTK.Text.Trim());
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

    }
}