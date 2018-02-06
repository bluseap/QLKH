using System;
using System.Data;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.UserControls;
using Message=EOSCRM.Util.Message;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class rpPhieuCongTac : Authentication
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
            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            var MADDK = (string)Session["NHAPTHICONG_MADDK"];
            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var thicong = new ThiCongDao().Get(MADDK);
         //   txtNguoiNhan.Text = thicong.NHANVIEN != null ? thicong.NHANVIEN.HOTEN : "";
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

            var MADDK = (string)Session["NHAPTHICONG_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var thicong = new ThiCongDao().Get(MADDK);
             
            
            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/GiayBaoCongTac.rpt");
            rp.Load(path);

            #region Text box
            var txtCSD = rp.ReportDefinition.ReportObjects["txtCSD"] as TextObject;
            if (txtCSD != null)
            {
                txtCSD.Text = Convert.ToString(thicong.CSDAU);

            }

            var txtTenKH = rp.ReportDefinition.ReportObjects["txtTenKH"] as TextObject;
            if (txtTenKH != null)
            {
                txtTenKH.Text = thicong.DONDANGKY.TENKH;

            }

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
            {
                txtDiaChi.Text = thicong.DONDANGKY.DUONGPHO != null ? thicong.DONDANGKY.SONHA + " , " + thicong.DONDANGKY.DUONGPHO.TENDP : thicong.DONDANGKY.TEN_DC_KHAC;
            }

            var txtPhuong = rp.ReportDefinition.ReportObjects["txtPhuong"] as TextObject;
            if (txtPhuong != null)
            {
                txtPhuong.Text = thicong.DONDANGKY.PHUONG != null ? thicong.DONDANGKY.PHUONG.TENPHUONG : "";
            }

            var txtThiXa = rp.ReportDefinition.ReportObjects["txtThiXa"] as TextObject;
            if (txtThiXa != null)
            {
                txtThiXa.Text = thicong.DONDANGKY.KHUVUC != null ? thicong.DONDANGKY.KHUVUC.TENKV : "";
            }

            var txtPhong = rp.ReportDefinition.ReportObjects["txtPhong"] as TextObject;
            if (txtPhong != null)
            {
                txtPhong.Text = txtPKTKT.Text;
            }

            var txtHoTenNvgs = rp.ReportDefinition.ReportObjects["txtHoTenNVGS"] as TextObject;
            if (txtHoTenNvgs != null)
            {
                txtHoTenNvgs.Text = txtNguoiLap.Text;
            }

            var txtKyThuat = rp.ReportDefinition.ReportObjects["txtKyThuat"] as TextObject;
            if (txtKyThuat != null)
            {
                txtKyThuat.Text = txtNguoiLap.Text;
            }

            var txtHoTenNvtc = rp.ReportDefinition.ReportObjects["txtHoTenNVTC"] as TextObject;
            if (txtHoTenNvtc != null)
            {
                txtHoTenNvtc.Text = thicong.NHANVIEN != null ? thicong.NHANVIEN.HOTEN : "";
            }


            var txtChucVu = rp.ReportDefinition.ReportObjects["txtChucVu"] as TextObject;
            if (txtChucVu != null)
            {
                if (thicong.NHANVIEN != null)
                    txtChucVu.Text = thicong.NHANVIEN.CONGVIEC != null ? thicong.NHANVIEN.CONGVIEC.TENCV : "";
            }




            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
            {
                txtNgay.Text = "Đồng Tháp, ngày " + DateTime.Now.Day.ToString() + " tháng " + DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
            }


            #endregion

           
             rpViewer.ReportSource = rp;
             rpViewer.DataBind();
            
            
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
               
      
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThiCongCongTrinh/NhapThiCong.aspx", false);
        }
    }
}