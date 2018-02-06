using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class SoBoCongTy : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();

        private SessionKey.PrintMode PrintMode
        {
            get
            {
                try
                {
                    if (Session[SessionKey.PRINTMODE_SOBOCONGTY] != null)
                    {
                        var mode = Convert.ToInt32(Session[SessionKey.PRINTMODE_SOBOCONGTY]);
                        if (mode == SessionKey.PrintMode.SoBoSauThangCuoiNam.GetHashCode())
                            return SessionKey.PrintMode.SoBoSauThangCuoiNam;

                        return SessionKey.PrintMode.SoBoSauThangDauNam;
                    }

                    return SessionKey.PrintMode.SoBoSauThangDauNam;
                }
                catch (Exception)
                {
                    return SessionKey.PrintMode.SoBoSauThangDauNam;
                }
            }

            set
            {
                Session[SessionKey.PRINTMODE_SOBOCONGTY] = value.GetHashCode();
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_BaoCao_QLKH_DS_SoBoCongTy, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session["DSBAOCAO_KH_SOBOCONGTY"];
                    Report(dt, PrintMode);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_QLKH_SOBO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.TITLE_KH_BAOCAO_QLKH_SOBO;
            }
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString();
            
            //TODO: load loai cong trinh

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
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            PrintMode = SessionKey.PrintMode.SoBoSauThangDauNam;

            var dt =
                new ReportClass().SoBoCongTy(int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), cboKhuVuc.Text.Trim()).Tables[0];

            Report(dt, PrintMode);

            CloseWaitingDialog();
        }

        protected void btnBaoCao2_Click(object sender, EventArgs e)
        {
            PrintMode = SessionKey.PrintMode.SoBoSauThangCuoiNam;

            var dt =
                new ReportClass().SoBoCongTy(int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), cboKhuVuc.Text.Trim()).Tables[0];

            Report(dt, PrintMode);

            CloseWaitingDialog();
        }

        private void Report(DataTable dt, SessionKey.PrintMode printMode)
        {
            if (dt == null)
                return;

            var dp = _dpDao.Get(txtMaDp.Text.Trim(), "");
            if (dp == null)
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
            var path = (printMode == SessionKey.PrintMode.SoBoSauThangDauNam) ?
                            Server.MapPath("../../../../Reports/QuanLyKhachHang/SoBoCongTy.rpt")
                            : Server.MapPath("../../../../Reports/QuanLyKhachHang/SoBoCongTy2.rpt");
            rp.Load(path);

            var txtDuongPho =  rp.ReportDefinition.ReportObjects["txtDuongPho"] as TextObject;
            if (txtDuongPho != null)
                txtDuongPho.Text = string.Format("Đường phố: {0} - {1}", dp.MADP, dp.TENDP);

            /*
            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));
            */

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DSBAOCAO_KH_SOBOCONGTY"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}