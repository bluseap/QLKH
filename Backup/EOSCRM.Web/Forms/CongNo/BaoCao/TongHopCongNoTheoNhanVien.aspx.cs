using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.CongNo.BaoCao
{
    public partial class TongHopCongNoTheoNhanVien : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_BaoCao_TongHopCongNoTheoNhanVien, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session[SessionKey.CN_BAOCAO_TONGHOPCONGNOTHEONHANVIEN];
                    Report(dt);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_CN_BAOCAO_TONGHOPCONGNOTHEONHANVIEN;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_CONGNO;
            header.TitlePage = Resources.Message.PAGE_CN_BAOCAO_TONGHOPCONGNOTHEONHANVIEN;
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString();
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;

            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;

            var nvList = _nvDao.GetListByCV(MACV.GT.ToString());

            ddlNHANVIEN.Items.Clear();
            ddlNHANVIEN.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var nv in nvList)
            {
                ddlNHANVIEN.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
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
            var ds = new ReportClass().TongHopCongNoTheoNhanVien(int.Parse(cboTHANG.Text.Trim()), 
                                                                    int.Parse(txtNAM.Text.Trim()));

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
        {
            if (dt == null)
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
            var path = Server.MapPath("../../../Reports/CongNo/TongHopCongNoTheoNhanVien.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = string.Format("Kỳ ghi: {0}/{1}", cboTHANG.Text, txtNAM.Text.Trim());

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0} tháng {1} năm {2}", 
                    DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.CN_BAOCAO_TONGHOPCONGNOTHEONHANVIEN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}