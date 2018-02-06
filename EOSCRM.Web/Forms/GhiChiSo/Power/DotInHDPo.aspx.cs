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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.GhiChiSo.Power
{
    public partial class DotInHDPo : Authentication
    {
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly LichGCSPoDao _lgcspoDao = new LichGCSPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();

        int thang1 = DateTime.Now.Month;
        string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_DotInHDPo, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                }
                else
                {
                    if (reloadm.Text == "1")
                    {
                        BaoCaoLGCS();
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }

        }
              
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_DOTINHDPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_DOTINHDPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvXEMBAOCAO);
        }

        private void LoadStaticReferences()
        {
            divupGV.Visible = false;

            timkv();
            ClearForm();

            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            //string b = loginInfo.Username;

            var dotin = _diDao.GetListKVDDNotP7(ddlKHUVUC.SelectedValue);
            ddlDOTGCS.Items.Clear();
            ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var d in dotin)
            {
                ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
            }
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKVPO(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
            upnlGhiChiSo.Update();
        }

        private void ClearForm()
        {
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
        }

        private void BindDuongPho()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            //var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            var list = _dppoDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());

            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();

            upnlDuongPho.Update();
            CloseWaitingDialog();
        }

        #region gvDuongPho
        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var res = id.Split('-');
                        var dp = _dppoDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADPPO);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHUPO);

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();
                            txtMADP.Focus();

                            upnlGhiChiSo.Update();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDuongPho.PageIndex = e.NewPageIndex;
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        private void BindData()
        {
            if (string.IsNullOrEmpty(txtMADP.Text.Trim()) || txtMADP.Text.Trim() == "")
            {
                var list = _dppoDao.GetListKVDotIn(ddlKHUVUC.SelectedValue, ddlDOTGCS.SelectedValue);
                //var list = _rpClass.BienKHNuoc("", ddlKHUVUC.SelectedValue, ddlDOTGCS.SelectedValue, "", 1, 1, "DSDOTINNN");

                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
                gvList.DataBind();
            }
            else
            {
                var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (dp == null) return;

                var list = _dppoDao.GetListMADP(txtMADP.Text.Trim());
                //var list = _rpClass.BienKHNuoc("", "", txtMADP.Text.Trim(), "", 1, 1, "DSDOTINDPNN");

                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
                gvList.DataBind();
            }

            divList.Visible = true;

            upnlGrid.Update();
        }

        #region gvList Duong Pho
        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;

            var ddlDOTIN = e.Row.FindControl("ddlDOTIN") as DropDownList;
            //var txtCHISODAU = e.Row.FindControl("txtCHISODAU") as TextBox;


            if (hfGCS == null || ddlDOTIN == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + ddlDOTIN.ClientID +
                //                                                    "\", \"" + txtCHISOCUOI.ClientID +
                //                                                    "\", \"" + txtKLTIEUTHU.ClientID +
                //                                                    "\", \"" + ddlTTHAIGHI.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            //txtCHISODAU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            //txtCHISOCUOI.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            //txtKLTIEUTHU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");

            ddlDOTIN.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");



            //txtCHISODAU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISODAU.ClientID + "\");");
            //txtCHISOCUOI.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISOCUOI.ClientID + "\");");
            //txtKLTIEUTHU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtKLTIEUTHU.ClientID + "\");");
            ddlDOTIN.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + ddlDOTIN.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + ddlDOTIN.ClientID +
                //                                                    "\", \"" + txtCHISOCUOI.ClientID +
                //                                                    "\", \"" + txtKLTIEUTHU.ClientID +
                //                                                    "\", \"" + ddlTTHAIGHI.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";

            ddlDOTIN.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvList.PageIndex = e.NewPageIndex;
                BindData();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #endregion

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                if (!HasPermission(Functions.GCS_DotInHDPo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (!HasPermission(Functions.GCS_DotInHDPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                divGVXEMBAOCAO.Visible = false;
                divrvLICHGHICS.Visible = false;

                BindData();

                CloseWaitingDialog();
                upnlGrid.Update();
                upnlReport.Update();
            }
            catch { }
        }

        protected void btSAVELIST_Click(object sender, EventArgs e)
        {
            try
            {
                ShowInfor("Lưu thành công.");
                CloseWaitingDialog();
            }
            catch { }
        }

        private void BindXemBaoCao()
        {
            try
            {
                int namF = Convert.ToInt16(txtNAM.Text.Trim());
                int thangF = Convert.ToInt16(ddlTHANG.SelectedValue);
                var list = _lgcspoDao.GetListKyKV(namF, thangF, ddlKHUVUC.SelectedValue);

                gvXEMBAOCAO.DataSource = list;
                gvXEMBAOCAO.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
                gvXEMBAOCAO.DataBind();

                divGVXEMBAOCAO.Visible = true;
            }
            catch { }
        }

        #region gv Xem bao cao
        protected void gvXEMBAOCAO_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvXEMBAOCAO.PageIndex = e.NewPageIndex;
                BindXemBaoCao();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvXEMBAOCAO_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lkbtMADPXBC = e.Row.FindControl("lkbtMADPXBC") as LinkButton;
            if (lkbtMADPXBC == null) return;
            lkbtMADPXBC.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lkbtMADPXBC) + "')");
        }

        protected void gvXEMBAOCAO_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "EditItem":

                        CloseWaitingDialog();
                        upnlGrid.Update();
                        break;

                    case "XoaEditItem":

                        XoaLichGCS(Convert.ToInt32(id));
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        private void XoaLichGCS(int idmadp)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var lgcs = _lgcspoDao.Get(idmadp);

                Message msg2;
                msg2 = _dppoDao.UpDotInInLichGCS2(lgcs.MADPPO, lgcs.IDMADOTINCU, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                Message msg;
                msg = _lgcspoDao.DeleteLichGCS(idmadp, lgcs.MADPPO, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), "");

                BindXemBaoCao();

                CloseWaitingDialog();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btXEMBAOBAO_Click(object sender, EventArgs e)
        {
            try
            {
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);

                bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                if (!HasPermission(Functions.GCS_DotInHDPo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (!HasPermission(Functions.GCS_DotInHDPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                divList.Visible = false;
                divrvLICHGHICS.Visible = false;

                BindXemBaoCao();

                CloseWaitingDialog();
                upnlGrid.Update();
                upnlReport.Update();
            }
            catch { }
        }

        protected void btBAOCAOLG_Click(object sender, EventArgs e)
        {
            divList.Visible = false;
            divGVXEMBAOCAO.Visible = false;
            divupGV.Visible = false;

            divrvLICHGHICS.Visible = true;

            BaoCaoLGCS();

            CloseWaitingDialog();
            upnlGrid.Update();
            upnlReport.Update();
        }

        private void BaoCaoLGCS()
        {
            try
            {
                ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
                if (rp != null)
                {
                    try
                    {
                        rp.Close();
                        rp.Dispose();
                        GC.Collect();
                    }
                    catch { }
                }

                int namF = Convert.ToInt16(txtNAM.Text.Trim());
                int thangF = Convert.ToInt16(ddlTHANG.SelectedValue);
                DataTable dt = _rpClass.BienKHNuoc("", ddlKHUVUC.SelectedValue, ddlDOTGCS.SelectedValue, "", thangF, namF, "DSLICHGCSD").Tables[0];

                rp = new ReportDocument();
                var path = Server.MapPath("/Reports/GhiChiSo/DSDOTINHD.rpt");
                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                string khuvuc = "XÍ NGHIỆP ĐIỆN NƯỚC " + _kvpoDao.Get(ddlKHUVUC.SelectedValue).TENKV.ToUpper();
                string tieude = "DANH SÁCH CHUYỂN LỊCH GHI CHỈ SỐ ĐIỆN";
                string kyghi = ddlDOTGCS.SelectedValue == "%" ? "Kỳ: " + thangF.ToString() + "/" + namF.ToString() :
                                    "Kỳ: " + thangF.ToString() + "/" + namF.ToString() + " (" + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
                string ngaythang = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

                TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                txtXN.Text = khuvuc;
                TextObject txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
                txtTIEUDE.Text = tieude;
                TextObject txtKYGHI = rp.ReportDefinition.ReportObjects["txtKYGHI"] as TextObject;
                txtKYGHI.Text = kyghi;
                TextObject txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                txtNGAY.Text = ngaythang;

                reloadm.Text = "1";

                divrvLICHGHICS.Visible = true;
                CloseWaitingDialog();
                upnlReport.Update();
            }
            catch { }
        }

    }
}