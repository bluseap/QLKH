using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class ThongTinThayDongHo : Authentication
   {
        private ReportDocument rp = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Authenticate(Functions..DanhSachVatTu, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    LoadReferences();
                }
                else 
                {
                    ReLoadBaoCao();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_THONGTINTHAYDONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_THONGTINTHAYDONGHO;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.Day.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + DateTime.Now.Year.ToString();
            txtDenNgay.Text = DateTime.Now.Day.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + DateTime.Now.Year.ToString();

            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.DataSource = listkhuvuc;
            cboKhuVuc.DataTextField = "TENKV";
            cboKhuVuc.DataValueField = "MAKV";
            cboKhuVuc.DataBind();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            cboKhuVuc.Text = "%";

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
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
                catch
                {

                }
            }

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt = new ReportClass().ThongTinNgayThayDongHo(TuNgay, DenNgay, txtMADP.Text.Trim(), "", cboKhuVuc.Text).Tables[0]; 
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThongTinNgayThayDongHo.rpt");
            rp.Load(path);

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim(); ;

            TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

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
                catch
                {

                }
            }

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt = (DataTable)Session["DS_DonDangKy"];//new ReportClass().RPDSCDDangKy(DateTime.Parse(txtTuNgay.Text), DateTime.Parse(txtDenNgay.Text), cboKhuVuc.Text).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThongTinNgayThayDongHo.rpt");
            rp.Load(path);

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim(); ;

            TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}