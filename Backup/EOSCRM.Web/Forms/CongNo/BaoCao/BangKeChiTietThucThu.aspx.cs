using System;
using System.Data;
using System.Globalization;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.CongNo.BaoCao
{
    public partial class BangKeChiTietThucThu : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_BaoCao_BangKeChiTietThucThu, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session[SessionKey.CN_BAOCAO_BANGKECHITIETTHUCTHU];
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
            Page.Title = Resources.Message.TITLE_CN_BAOCAO_BANGKECHITIETTHUCTHU;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_CONGNO;
            header.TitlePage = Resources.Message.PAGE_CN_BAOCAO_BANGKECHITIETTHUCTHU;
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString();
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;
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
            var ds =
                new ReportClass().BangKeChiTietThucThu(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()));

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
            var path = Server.MapPath("../../../Reports/CongNo/BangKeChiTietThucThu.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = cboTHANG.Text + "/" + txtNAM.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));

            #region Tien mat, chuyen khoan

            int HDCK = 0;
            decimal TienNuocCK = 0;
            decimal PhiCK = 0;
            decimal ThueCK = 0;
            decimal TongCongCK = 0;

            int HDTM = 0;
            decimal TienNuocTM = 0;
            decimal PhiTM = 0;
            decimal ThueTM = 0;
            decimal TongCongTM = 0;



            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow["MAHTTT"].ToString().ToUpper() == "CK")
                {
                    HDCK = HDCK + (int)dataRow["SLHD"];
                    TienNuocCK = TienNuocCK + (decimal)dataRow["TIENNUOC"];
                    PhiCK = PhiCK + (decimal)dataRow["PHI"];
                    ThueCK = ThueCK + (decimal)dataRow["THUE"];
                    TongCongCK = TongCongCK + (decimal)dataRow["TONGCONG"];
                }
                else
                {
                    HDTM = HDTM + (int)dataRow["SLHD"];
                    TienNuocTM = TienNuocTM + (decimal)dataRow["TIENNUOC"];
                    PhiTM = PhiTM + (decimal)dataRow["PHI"];
                    ThueTM = ThueTM + (decimal)dataRow["THUE"];
                    TongCongTM = TongCongTM + (decimal)dataRow["TONGCONG"];
                }
            }

            var txtHDCK = rp.ReportDefinition.ReportObjects["txtHDCK"] as TextObject;
            if (txtHDCK != null)
                txtHDCK.Text = HDCK.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtTienNuocCK = rp.ReportDefinition.ReportObjects["txtTienNuocCK"] as TextObject;
            if (txtTienNuocCK != null)
                txtTienNuocCK.Text = TienNuocCK.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtPhiCK = rp.ReportDefinition.ReportObjects["txtPhiCK"] as TextObject;
            if (txtPhiCK != null)
                txtPhiCK.Text = PhiCK.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtThueCK = rp.ReportDefinition.ReportObjects["txtThueCK"] as TextObject;
            if (txtThueCK != null)
                txtThueCK.Text = ThueCK.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtTongCongCK = rp.ReportDefinition.ReportObjects["txtTongCongCK"] as TextObject;
            if (txtTongCongCK != null)
                txtTongCongCK.Text = TongCongCK.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtHDTM = rp.ReportDefinition.ReportObjects["txtHDTM"] as TextObject;
            if (txtHDTM != null)
                txtHDTM.Text = HDTM.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtTienNuocTM = rp.ReportDefinition.ReportObjects["txtTienNuocTM"] as TextObject;
            if (txtTienNuocTM != null)
                txtTienNuocTM.Text = TienNuocTM.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));


            var txtPhiTM = rp.ReportDefinition.ReportObjects["txtPhiTM"] as TextObject;
            if (txtPhiTM != null)
                txtPhiTM.Text = PhiTM.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtThueTM = rp.ReportDefinition.ReportObjects["txtThueTM"] as TextObject;
            if (txtThueTM != null)
                txtThueTM.Text = ThueTM.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            var txtTongCongTM = rp.ReportDefinition.ReportObjects["txtTongCongTM"] as TextObject;
            if (txtTongCongTM != null)
                txtTongCongTM.Text = TongCongTM.ToString("0,0", CultureInfo.CreateSpecificCulture("vi-VN"));

            #endregion

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.CN_BAOCAO_BANGKECHITIETTHUCTHU] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}