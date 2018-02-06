using System;
using System.Data;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class TinhHinhTieuThu : Authentication
        // public partial class DSCDDangKy
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
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_QLKH_TINHHINHTIEUTHU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_QLKH_TINHHINHTIEUTHU;
            }
        }

        private void LoadReferences()
        {
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


            DataTable dt = (DataTable)Session["DSINHOADON"];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/TTTTKH.rpt");
            rp.Load(path);

          
            TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}