using System;
using System.Web.UI.WebControls;
using System.Globalization;
using System.Data;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Linq;


using System.Web.UI;
using System.IO;
using System.Web;


namespace EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo
{
    public partial class DSKHCBiKTPo : Authentication
    {
        private readonly QuanHuyenDao _qhDao = new QuanHuyenDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();

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

        private void UnblockWaitingDialog()
        {
            ((PO)Page.Master).UnblockWaitingDialog();
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
                //AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindPhuong();
                }
                else
                {
                    if (lbReLoad.Text == "1")
                    {
                        BaoCao();
                    }
                    if (lbReLoad.Text == "2")
                    {
                        BaoCaoTangGiam();
                    }
                    if (lbReLoad.Text == "3")
                    {
                        BaoCaoTangGiamTo();
                    }
                    if (lbReLoad.Text == "4")
                    {
                        BCTGMDKhac();
                    }
                    if (lbReLoad.Text == "5")
                    {
                        BaoCaoMDKhac();
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadStaticReferences()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kvnv = _nvDao.Get(b);

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();

                timkv();              

                var NhaMayList = _pbDao.GetListKV(kvnv.MAKV);
                ddlNHAMAYTO.Items.Clear();
                ddlNHAMAYTO.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var kv in NhaMayList)
                {
                    ddlNHAMAYTO.Items.Add(new ListItem(kv.MAPB + " " + kv.TENPB, kv.MAPB));
                }                

                var kvList = _ppoDao.GetListKV(_kvpoDao.GetPo(kvnv.MAKV).MAKVPO);
                ddlMAPHUONG.Items.Clear();
                ddlMAPHUONG.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var kv in kvList)
                {
                    ddlMAPHUONG.Items.Add(new ListItem(kv.MAPHUONGPO + " " + kv.TENPHUONG, kv.MAPHUONGPO));
                }

                var huyen = _qhDao.GetList();
                ddlQuanHuyen.Items.Clear();
                ddlQuanHuyen.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var h in huyen)
                {
                    ddlQuanHuyen.Items.Add(new ListItem(h.TENQUAN, h.Id));
                }

                var xpList = _xpDao.GetListKV(kvnv.MAKV);
                ddlPHUONGXA.Items.Clear();
                ddlPHUONGXA.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var xa in xpList)
                {
                    ddlPHUONGXA.Items.Add(new ListItem(xa.MAXA + " " + xa.TENXA, xa.MAXA));
                }                  

                var dotin = _diDao.GetListKVDDNotP7(_kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }

                if (kvnv.MAKV != "99")
                {
                    LoadKhuVucXaPhuong(kvnv.MAKV);
                }
            }
            catch { }
        }

        private void LoadKhuVucXaPhuong(string makv)
        {
            var khuvucpo = _kvpoDao.GetPo(makv);
            var quanhuyen = _qhDao.GetMAKV(makv);

            //var kvpo = ddlKHUVUC.Items.FindByValue(khuvucpo.MAKVPO);
            //if (kvpo != null)
            //    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kvpo);

            var qh = ddlQuanHuyen.Items.FindByValue(quanhuyen.Id);
            if (qh != null)
                ddlQuanHuyen.SelectedIndex = ddlQuanHuyen.Items.IndexOf(qh);

            var xp = _xpDao.GetListKV(makv);
            ddlPHUONGXA.Items.Clear();
            ddlPHUONGXA.Items.Add(new ListItem("-- Tất cả --", "%"));
            foreach (var x in xp)
            {
                ddlPHUONGXA.Items.Add(new ListItem(x.TENXA, x.MAXA));
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

                if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC1.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(a.MAKV);
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAOPO_DSKHCBIKT;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAOPO_DSKHCBIKT;
            }

            CommonFunc.SetPropertiesForGrid(gvPhuong);
            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }       

        protected void gvPhuong_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                switch (e.CommandName)
                {
                    case "SelectMAPHUONG":
                        var maphuong = _ppoDao.GetMAKV(id.ToString(), _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO).MAPHUONGPO;

                        txtMAPHUONG.Text = maphuong.ToString();

                        upKHCHUANBIKT.Update();
                        HideDialog("divPhuong");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvPhuong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvPhuong.PageIndex = e.NewPageIndex;                
                BindPhuong();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindPhuong()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var list = _ppoDao.GetListKVTS(_kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO, txtKeywordDP.Text.Trim());

                gvPhuong.DataSource = list;
                gvPhuong.PagerInforText = list.Count.ToString();
                gvPhuong.DataBind();

                upnlPhuong.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnBrowseSOHD_Click(object sender, EventArgs e)
        {
            BindPhuong();
            upnlPhuong.Update();
            UnblockDialog("divPhuong");
        }

        protected void btnFilterPhuong_Click(object sender, EventArgs e)
        {
            BindPhuong();
        }

        protected void btnDSKHCBiKT_Click(object sender, EventArgs e)
        {
            BaoCao();
        }

        private void BaoCao()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);               

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam
                                
                //var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlNHAMAYTO.SelectedValue.ToString(), "dsCBIKTPO", TuNgay, DenNgay);
                var ds = new ReportClass().DSQuiTrinhPoBien(TuNgay, DenNgay, _kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlNHAMAYTO.SelectedValue.ToString(),
                                     ddlDOTGCS.SelectedValue, "", "dsCBIKTDOTINPO" );

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayN(ds.Tables[0]);

                lbReLoad.Text = "1";

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportNgayN(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKTPO.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).TENKV;

            //txtXN
            var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            if (txtXN != null)
                txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG ĐIỆN CHUẨN BỊ KHAI THÁC";
            //txtTENPHUONG
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            string kyF = ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
            if (txtTENPHUONG != null)
                //txtTENPHUONG.Text = "Danh bộ: " + _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO + (ddlMAPHUONG.SelectedValue != "%" ? ddlMAPHUONG.SelectedValue : "");
                txtTENPHUONG.Text = "Kỳ: " + kyF + ". " + ddlDOTGCS.SelectedItem.ToString();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divReport.Visible = true;
            upKHCHUANBIKT.Update();
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btDSTANGGIAM_Click(object sender, EventArgs e)
        {
            BaoCaoTangGiam();
        }

        private void BaoCaoTangGiam()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);

                //string ngay = "";
                //if (query.MAKV == "U")
                //{
                //    ngay = "7";
                //}
                //else
                //{
                //    ngay = "16";
                //}

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam
                                
                var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlPHUONGXA.SelectedValue.ToString(), "dsCBIKTPOPX", 
                    TuNgay, DenNgay);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayNTangGiam(ds.Tables[0]);

                lbReLoad.Text = "2";

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportNgayNTangGiam(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKTPO_TG.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).TENKV;

            //txtXN
            //var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            //if (txtXN != null)
            //    txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH SỐ HỘ TĂNG HOẶC GIẢM ĐI";
            //txtTENPHUONG
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            if (txtTENPHUONG != null)
                txtTENPHUONG.Text = "Tháng " + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            //TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            //ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divReport.Visible = true;
            upKHCHUANBIKT.Update();
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }       

        protected void btDSTANGGIAMTO_Click1(object sender, EventArgs e)
        {
            BaoCaoTangGiamTo();
        }

        protected void BaoCaoTangGiamTo()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);

                //string ngay = "";
                //if (query.MAKV == "U")
                //{
                //    ngay = "7";
                //}
                //else
                //{
                //    ngay = "16";
                //}

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam
                                
                var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlNHAMAYTO.SelectedValue.ToString(), "dsCBIKTPOPTO", TuNgay, DenNgay);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayNTangGiamTo(ds.Tables[0]);

                lbReLoad.Text = "3";

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportNgayNTangGiamTo(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKTPO_TG.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).TENKV;

            //txtXN
            //var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            //if (txtXN != null)
            //    txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH SỐ HỘ TĂNG HOẶC GIẢM ĐI";
            //txtTENPHUONG
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            if (txtTENPHUONG != null)
                txtTENPHUONG.Text = "Tháng " + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            //TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            //ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divReport.Visible = true;
            upKHCHUANBIKT.Update();
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btDSTGMDK_Click(object sender, EventArgs e)
        {
            BCTGMDKhac();
        }

        protected void BCTGMDKhac()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);              

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam

                var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlNHAMAYTO.SelectedValue.ToString(), "dsCBIKTPOMDK", TuNgay, DenNgay);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayNTangGiamMDKhac(ds.Tables[0]);

                lbReLoad.Text = "4";

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportNgayNTangGiamMDKhac(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKTPO_TG.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).TENKV;

            //txtXN
            //var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            //if (txtXN != null)
            //    txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH SỐ HỘ TĂNG HOẶC GIẢM ĐI";
            //txtTENPHUONG
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            if (txtTENPHUONG != null)
                txtTENPHUONG.Text = "Tháng " + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            //TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            //ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //divReport.Visible = true;
            upKHCHUANBIKT.Update();
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnDSKHCBiKTMDK_Click(object sender, EventArgs e)
        {
            BaoCaoMDKhac();
        }

        private void BaoCaoMDKhac()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);               

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam

                //var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlMAPHUONG.SelectedValue.ToString(), "dsCBIKTPO", TuNgay, DenNgay);
                var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), ddlNHAMAYTO.SelectedValue.ToString(), "dsCBIKTMDKPO", TuNgay, DenNgay);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayN(ds.Tables[0]);
                
                lbReLoad.Text = "5";

                CloseWaitingDialog();
            }
            catch { }
        }

        protected void ddlQuanHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQuanHuyen.SelectedValue != "%")
            {
                var quanhuyen = _qhDao.Get(ddlQuanHuyen.SelectedValue);
                //var khuvucpo = _kvpoDao.Get(quanhuyen.MAKV);

                var xp = _xpDao.GetListKV(quanhuyen.MAKV);
                ddlPHUONGXA.Items.Clear();
                ddlPHUONGXA.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var x in xp)
                {
                    ddlPHUONGXA.Items.Add(new ListItem(x.TENXA, x.MAXA));
                }
            }

            upKHCHUANBIKT.Update();

        }

        protected void btXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var nhanvien = _nvDao.Get(b);

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     //thang
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    //nam

                DataTable dt;

                var ds = new ReportClass().dsKHCBiKT(_kvpoDao.GetPo(nhanvien.MAKV).MAKVPO.ToString(), ddlPHUONGXA.SelectedValue.ToString(), "dsCBIKTPOPXEX",
                    TuNgay, DenNgay);
                dt = ds.Tables[0];     
               

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=KHTG" + ddlTHANG.SelectedValue.ToString().Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";
                Response.ContentEncoding = System.Text.Encoding.UTF8;

                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //Response.Write(style);
                string style = @"<style> TD { mso-number-format:\@; } </style>";
                Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                upKHCHUANBIKT.Update();
            }
            catch ( Exception ex )
            {
                ShowError(ex.ToString(), "");
            }
        }

    }
}