using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.CongNo.BaoCao
{
    public partial class ChiTietThuNoTheoThoiGian : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        private ReportMode UpdateMode
        {
            get
            {
                try
                {
                    if (Session[SessionKey.MODE] != null)
                    {
                        var mode = Convert.ToInt32(Session[SessionKey.MODE]);
                        if (mode == ReportMode.Day.GetHashCode())
                            return ReportMode.Day;

                        if (mode == ReportMode.Month.GetHashCode())
                            return ReportMode.Month;

                        return ReportMode.Normal;
                    }

                    return ReportMode.Normal;
                }
                catch (Exception)
                {
                    return ReportMode.Normal;
                }
            }

            set
            {
                Session[SessionKey.MODE] = value.GetHashCode();
            }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_BaoCao_ChiTietThuNoTheoThoiGian, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    if (UpdateMode == ReportMode.Normal)
                    {
                        var dt = (DataTable)Session[SessionKey.CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN];
                        Report(dt);
                    }

                    else if (UpdateMode == ReportMode.Day)
                    {
                        var dt = (DataTable)Session[SessionKey.CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN];
                        Report2(dt);
                    }
                    else if (UpdateMode == ReportMode.Month)
                    {
                        var dt = (DataTable)Session[SessionKey.CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN];
                        Report3(dt);
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
            Page.Title = Resources.Message.TITLE_CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_CONGNO;
                header.TitlePage = Resources.Message.PAGE_CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            var nvList = _nvDao.GetListByCV(MACV.GT.ToString());

            cboNhanVienThu.Items.Clear();
            cboNhanVienThu.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var nv in nvList)
            {
                cboNhanVienThu.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
            }

            var kvList = _kvDao.GetList();

            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var tungay = DateTime.Now;
            var denngay = DateTime.Now;

            try
            {
                tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                return;
            }

            var ds =
                new ReportClass().ChiTietThuNoTheoThoiGian(tungay, denngay, cboNhanVienThu.Text.Trim(), cboKhuVuc.Text.Trim());
            if (ds == null || ds.Tables.Count == 0)
            { CloseWaitingDialog(); return; }

            Report(ds.Tables[0]);

            CloseWaitingDialog();
        }

        protected void btnBaoCao2_Click(object sender, EventArgs e)
        {
            UpdateMode = ReportMode.Day;
            var tungay = DateTime.Now;
            var denngay = DateTime.Now;

            try
            {
                tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                return;
            }

            var ds =
                new ReportClass().ChiTietThuNoTheoThoiGian(tungay, denngay, cboNhanVienThu.Text.Trim(), cboKhuVuc.Text.Trim());
            if (ds == null || ds.Tables.Count == 0)
            { CloseWaitingDialog(); return; }

            Report2(ds.Tables[0]);

            CloseWaitingDialog();
        }

        protected void btnBaoCao3_Click(object sender, EventArgs e)
        {
            UpdateMode = ReportMode.Month;

            var tungay = DateTime.Now;
            var denngay = DateTime.Now;

            try
            {
                tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                return;
            }

            var ds =
                new ReportClass().ChiTietThuNoTheoThoiGian(tungay, denngay, cboNhanVienThu.Text.Trim(), cboKhuVuc.Text.Trim());
            if (ds == null || ds.Tables.Count == 0)
            { CloseWaitingDialog(); return; }

            Report3(ds.Tables[0]);

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
        {
            if(dt == null)
                return;

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

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/ChiTietThuNoTheoThoiGian.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;
            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void Report2(DataTable dt)
        {
            if(dt == null)
                return;

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

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/ChiTietThuNoTheoThoiGianNgay.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void Report3(DataTable dt)
        {
            if(dt==null)
                return;
            
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

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/ChiTietThuNoTheoThoiGianThang.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.CN_BAOCAO_CHITIETTHUNOTHEOTHOIGIAN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}