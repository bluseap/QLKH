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
    public partial class rpLapQuyetToanSuaChua : Authentication
    {
        private ReportDocument rp = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_LapChietTinh, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DLM_BANGDUTOAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DLM_BANGDUTOAN;
            }
        }

        private void LoadReferences()
        {
            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;
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

            var MADDK = (string)Session["LAPCHIETTINHSUACHUA_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var chiettinh = new QuyetToanSuaChuaDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoLapQuyetToanSuaChua(MADDK);
            
            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/LapQuyetToanSuaChua.rpt");
            rp.Load(path);


            #region Text box
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            txtTitle.Text = "BẢNG QUYẾT TOÁN CÔNG TÁC SỬA CHỮA";
            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if(txtKhachHang != null)
                txtKhachHang.Text = chiettinh .TENCT ;
            
            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if(txtDiaChi != null)
                txtDiaChi.Text = chiettinh.DIACHIHM;

            decimal tienvattu = 0;
            decimal thuevattu = 0;
            decimal tongvattu = 0;
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                if(row["THUTU"].ToString() =="1")
                {
                    tienvattu = tienvattu + (decimal) row["TIENVT"];
                }
            }
            thuevattu = tienvattu*10/100;
            thuevattu = Math.Round(thuevattu, 0);
            tongvattu = tienvattu + thuevattu;

            var txtTienVatTu = rp.ReportDefinition.ReportObjects["txtTienVatTu"] as TextObject;
            if (txtTienVatTu != null)
                txtTienVatTu.Text = string.Format("{0:0,0}", tienvattu).Replace(",", "."); 

            var txtThueTienVatTu = rp.ReportDefinition.ReportObjects["txtThueTienVatTu"] as TextObject;
            if (txtThueTienVatTu != null)
                txtThueTienVatTu.Text = string.Format("{0:0,0}", thuevattu).Replace(",", ".");
            
            var txtTongTienVatTu = rp.ReportDefinition.ReportObjects["txtTongTienVatTu"] as TextObject;
            if (txtTongTienVatTu != null)
                txtTongTienVatTu.Text = string.Format("{0:0,0}", tongvattu).Replace(",", ".");

            decimal tienSTK = 0;
            decimal thueSTK = 0;
            decimal tongSTK = 0;
            foreach (DataRow row in dt.Tables[0].Rows)
            {
                if (row["THUTU"].ToString() == "2")
                {
                    tienSTK = tienSTK + (decimal)row["TIENVT"];
                }
            }
            thueSTK = thueSTK * 10 / 100;
            thueSTK = Math.Round(thueSTK, 0);
            tongSTK = tienSTK + thueSTK;

            var txtTienStk = rp.ReportDefinition.ReportObjects["txtTienSTK"] as TextObject;
            if (txtTienStk != null)
                txtTienStk.Text = string.Format("{0:0,0}", tienSTK).Replace(",", ".");

            var txtThueStk = rp.ReportDefinition.ReportObjects["txtThueSTK"] as TextObject;
            if (txtThueStk != null)
                txtThueStk.Text = string.Format("{0:0,0}", thueSTK).Replace(",", ".");

            var txtTongStk = rp.ReportDefinition.ReportObjects["txtTongSTK"] as TextObject;
            if (txtTongStk != null)
                txtTongStk.Text = string.Format("{0:0,0}", tongSTK).Replace(",", ".");


            decimal tienNC = 0;
            decimal thueNC = 0;
            decimal tongNC = 0;
            foreach (DataRow row in dt.Tables[0].Rows)
            {

                tienNC = tienNC + (decimal)row["TIENVT"];

            }
            thueNC = tienNC * 10 / 100;
            thueNC = Math.Round(thueNC, 0);
            tongNC = tienNC + thueNC;

            var txtTienNhanCong = rp.ReportDefinition.ReportObjects["txtTienNhanCong"] as TextObject;
            if (txtTienNhanCong != null)
                txtTienNhanCong.Text = string.Format("{0:0,0}", tienNC).Replace(",", ".");

            var txtThueNhanCong = rp.ReportDefinition.ReportObjects["txtThueNhanCong"] as TextObject;
            if (txtThueNhanCong != null)
                txtThueNhanCong.Text = string.Format("{0:0,0}", thueNC).Replace(",", ".");

            var txtTongNhanCong = rp.ReportDefinition.ReportObjects["txtTongNhanCong"] as TextObject;
            if (txtTongNhanCong != null)
                txtTongNhanCong.Text = string.Format("{0:0,0}", tongNC).Replace(",", ".");

            decimal tongcong = tongvattu + tongSTK + tongNC;

            var txtTongCong = rp.ReportDefinition.ReportObjects["txtTongCong"] as TextObject;
            if (txtTongCong != null)
                txtTongCong.Text = string.Format("{0:0,0}", tongcong).Replace(",", ".");

            var txtBangChu = rp.ReportDefinition.ReportObjects["txtBangChu"] as TextObject;
            if (txtBangChu != null)
                txtBangChu.Text = CommonUtil.DocSoTien(tongcong);

            /*
            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgayThangNam"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "Ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
            */

            #endregion

           
          
            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }
            Session["LAPCHIETTINHSUACHUA_MADDK"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/LapChietTinh.aspx?" + Constants.PARAM_REPORTED + "=true", false);
        }
    }
}