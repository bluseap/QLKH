using System;
using System.Data;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class ChuanThuTheoDuong : Authentication
    {
        private readonly MucDichSuDungDao _mdsdDao = new MucDichSuDungDao();
        private readonly  LoaiKhDacBietDao _lkhdbDao = new LoaiKhDacBietDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_BaoCao_DSChuanThuTheoDuong, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    if (Session[SessionKey.GCS_BAOCAO_BANGKECHITIETCHUANTHU] == "GCS_BAOCAO_BANGKECHITIETCHUANTHU")
                    {
                        var dt = (DataTable)Session[SessionKey.GCS_BAOCAO_BANGKECHITIETCHUANTHU];
                        Report(dt);
                    }

                    if (Session[SessionKey.GCS_BAOCAO_THTRANGTHAITHEODUONG] == "GCS_BAOCAO_THTRANGTHAITHEODUONG")
                    {
                        var trangthai = (DataTable)Session[SessionKey.GCS_BAOCAO_THTRANGTHAITHEODUONG];
                        ReportTHTRANGTHAI(trangthai);
                    }

                    if (Session[SessionKey.GCS_BAOCAO_BANGKECHITIETCHUANTHUKV] == "GCS_BAOCAO_BANGKECHITIETCHUANTHUKV")
                    {
                        var dtkv = (DataTable)Session[SessionKey.GCS_BAOCAO_BANGKECHITIETCHUANTHUKV];
                        ReportTheoKV(dtkv);
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
            Page.Title = Resources.Message.TITLE_GCS_BAOCAO_CHUANTHUTHEODUONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_GCS_BAOCAO_CHUANTHUTHEODUONG;
            }
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;
            
            var kvList = _kvDao.GetList();

            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            var lkhdbList = _lkhdbDao.GetList();
            ddlLKHDB.Items.Clear();
            ddlLKHDB.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var lkhdb in lkhdbList)
            {
                ddlLKHDB.Items.Add(new ListItem(lkhdb.TENLKHDB, lkhdb.MALKHDB));
            }

            // bind dllMDSD
            var mdsdList = _mdsdDao.GetList();
            ddlMDSD.Items.Clear();
            ddlMDSD.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var mdsd in mdsdList)
            {
                ddlMDSD.Items.Add(new ListItem(mdsd.TENMDSD, mdsd.MAMDSD));
            }
            timkv();

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
        }

        #region Startup script registeration
        private void UnblockWaitingDialog()
        {
            ((EOS)Page.Master).UnblockWaitingDialog();
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var lkhdb = ddlLKHDB.Enabled ? ddlLKHDB.SelectedValue : "%";

            var ds = new ReportClass().B_CHUANTHUTHEOMDSD(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

            CloseWaitingDialog();
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
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void Report(DataTable dt)
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
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/BangChuanThuTheoDuong.rpt");
            rp.Load(path);

            

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
            {
                var kv = _kvDao.Get(cboKhuVuc.SelectedValue);
                var mdsd = _mdsdDao.Get(ddlMDSD.SelectedValue);
                txtKy.Text = string.Format("Kỳ hóa đơn: {0}/{1}", cboTHANG.Text, txtNAM.Text.Trim());

                if (kv != null)
                    txtKy.Text += string.Format("                    " + "Khu vực: {0}", kv.TENKV);
                if(mdsd != null)
                    txtKy.Text += string.Format("                    " + "Loại KH: {0}", mdsd.TENMDSD);
            }

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("TP.Long Xuyên, ngày {0} tháng {1} năm {2}",
                    DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            /*
            decimal kettoan = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["TTSOTIEN"] != null)
                    kettoan += (decimal)row["TTSOTIEN"];
            }

            var txtKetToan = rp.ReportDefinition.ReportObjects["txtKetToan"] as TextObject;
            if (txtKetToan != null)
                txtKetToan.Text = CommonUtil.DocSoTien(Math.Round(kettoan, 0, MidpointRounding.AwayFromZero));

            */
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.GCS_BAOCAO_BANGKECHITIETCHUANTHU] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void ddlMDSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnblockWaitingDialog();

            var val = ddlMDSD.SelectedValue;
            ddlLKHDB.Enabled = (val == MAMDSD.CQ.GetHashCode().ToString() || val == MAMDSD.KD.GetHashCode().ToString() || val == "%");

            CloseWaitingDialog();
        }

        protected void lnTTHAIGHITHEODUONG_Click(object sender, EventArgs e)
        {
            var lkhdb = ddlLKHDB.Enabled ? ddlLKHDB.SelectedValue : "%";

            var ds =
                new ReportClass().B_TRANGTHAITHEODUONG(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                        cboKhuVuc.SelectedValue);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            ReportTHTRANGTHAI(ds.Tables[0]);

            CloseWaitingDialog();
        }

        private void ReportTHTRANGTHAI(DataTable dt)
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
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/TONGHOPTRANGTHAI.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
            {
                var kv = _kvDao.Get(cboKhuVuc.SelectedValue);
                var mdsd = _mdsdDao.Get(ddlMDSD.SelectedValue);
                txtKy.Text = string.Format("Kỳ hóa đơn: {0}/{1}", cboTHANG.Text, txtNAM.Text.Trim());

                if (kv != null)
                    txtKy.Text += string.Format("                    " + "Khu vực: {0}", kv.TENKV);
                if (mdsd != null)
                    txtKy.Text += string.Format("                    " + "Loại KH: {0}", mdsd.TENMDSD);
            }

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("TP.Long Xuyên, ngày {0} tháng {1} năm {2}",
                    DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;
            
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.GCS_BAOCAO_THTRANGTHAITHEODUONG] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btChuanThuTheoKV_Click(object sender, EventArgs e)
        {
            try
            {
                var lkhdb = ddlLKHDB.Enabled ? ddlLKHDB.SelectedValue : "%";

                var ds = new ReportClass().B_CHUANTHUTHEOMDSDKV(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportTheoKV(ds.Tables[0]);

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportTheoKV(DataTable dt)
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
                }    catch              { }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/BangChuanThuTheoDuong.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
            {
                var kv = _kvDao.Get(cboKhuVuc.SelectedValue);
                var mdsd = _mdsdDao.Get(ddlMDSD.SelectedValue);
                txtKy.Text = string.Format("Kỳ hóa đơn: {0}/{1}", cboTHANG.Text, txtNAM.Text.Trim());

                if (kv != null)
                    txtKy.Text += string.Format("                    " + "Khu vực: {0}", kv.TENKV);
                if (mdsd != null)
                    txtKy.Text += string.Format("                    " + "Loại KH: {0}", mdsd.TENMDSD);
            }

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("TP.Long Xuyên, ngày {0} tháng {1} năm {2}",
                    DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.GCS_BAOCAO_BANGKECHITIETCHUANTHUKV] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}