using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;


namespace EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo
{
    public partial class DSThayDHPo : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private ReportDocument rp = new ReportDocument();
        private NhanVienDao _nvDao = new NhanVienDao();
        private KhuVucDao _kvDao = new KhuVucDao();
        private KhuVucPoDao _kvpoDao = new KhuVucPoDao();

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

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
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
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    LoadReferences();
                    ClearForm();
                }
                else
                {
                    if (lbRELOAD.Text == "1")
                    { ReLoadBaoCao(); }
                    if (lbRELOAD.Text == "2")
                    { ReLoadBaoCaoMucDK(); }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAOPO_DSTDHPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAOPO_DSTDHPO;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
           
            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            timkv();

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var dotin = _diDao.GetListKVDDNotP7(_kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
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

                if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        private void ClearForm()
        {
            /*
             * clear phần thông tin hồ sơ
             */
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();

        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
        }

        private void LayBaoCao()
        {
            #region FreeMemory
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch  {  }
            }

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            //var dotin = _diDao.GetKVPoIdDot(ddlDOTGCS.SelectedValue, _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
            //var dotin = _diDao.Get(ddlDOTGCS.SelectedValue);
            //if (dotin == null)
            //{
            //    ShowError("Chưa có đợt in này! Xin kiểm tra lại.");
            //    CloseWaitingDialog();
            //    return;
            //}

            if (ddlDOTGCS.SelectedValue == "%")
            {
                dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, "", "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        "DSTHAYDHSINHH").Tables[0];
            }
            else
            {
                dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        "DSTHAYDHSINHHPODOTIN").Tables[0];
            }

            //dt = new ReportClass().ThayDongHoPo(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            //dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, "", "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
            //                            "DSTHAYDHSINHH").Tables[0];
            
            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHoPo.rpt");
            rp.Load(path);
            //}
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " (" 
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN  + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            lbRELOAD.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void ReLoadBaoCao()
        {
            #region FreeMemory
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

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            //var dotin = _diDao.GetKVPoDot(ddlDOTGCS.SelectedValue, _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
            //if (dotin == null)
            //{
            //    ShowError("Chưa có đợt in này! Xin kiểm tra lại.");
            //    CloseWaitingDialog();
            //    return;
            //}

            if (ddlDOTGCS.SelectedValue == "%")
            {
                dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, "", "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        "DSTHAYDHSINHH").Tables[0];
            }
            else
            {
                dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        "DSTHAYDHSINHHPODOTIN").Tables[0];
            }

            //dt = new ReportClass().ThayDongHoPo(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            //dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, "", "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
            //                            "DSTHAYDHSINHH").Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHoPo.rpt");
            rp.Load(path);
            //}
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                    + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //lbRELOAD.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btBCMucDK_Click(object sender, EventArgs e)
        {
            LayBaoCaoMucDK();
        }

        private void LayBaoCaoMucDK()
        {
            #region FreeMemory
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

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt;
            /* (cboKhuVuc.SelectedValue != "O")
            {
                dt = new ReportClass().ThayDongHoDC(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                rp = new ReportDocument();
                var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHoDC.rpt");
                rp.Load(path);
            }
            else
            {*/
            //dt = new ReportClass().ThayDongHoPo(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, "", "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        "DSTHAYDHMDK").Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHoPo.rpt");
            rp.Load(path);
            //}
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim();
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper() + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            lbRELOAD.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void ReLoadBaoCaoMucDK()
        {
            #region FreeMemory
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

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt;
            
            //dt = new ReportClass().ThayDongHoPo(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            dt = new ReportClass().BienKHPo("", cboKhuVuc.SelectedValue, "", "", Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        "DSTHAYDHSINHH").Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHoPo.rpt");
            rp.Load(path);
            //}
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim();
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = _kvpoDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper() + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();


            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}