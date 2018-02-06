using System;
using System.Data;
using EOSCRM.Web.Common;
using System.Globalization;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.UserControls;
using Message=EOSCRM.Util.Message;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class rpVatTuThietKe : Authentication
    {
        private ReportDocument rp = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
               // Authenticate(Functions.TK_LapChietTinh, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    LayBaoCao();
                }
                else 
                {
                    LayBaoCao();

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DLM_BANGKHOILUONGVATTU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DLM_BANGKHOILUONGVATTU;
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
           
            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var chiettinh = new ChietTinhDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            
            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe.rpt");
            rp.Load(path);

            #region Text box

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = chiettinh.TENCT;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = chiettinh.DONDANGKY.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "Ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
            #endregion

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }
            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx", false);
        }
    }
}