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
    public partial class rpLapChietTinhSuaChua : Authentication
    {
        private ReportDocument rp = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_LapChietTinhSuaChua, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_SC_BAOCAO_BANGDUTOAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_BAOCAO_BANGDUTOAN;
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

            var MADDK = (string)Session["LAPCHIETTINHSUACHUA_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var chiettinh = new ChietTinhSuaChuaDao().Get(MADDK);
             
            var dt = new ReportClass().BaoCaoLapChietTinhSuaChua(MADDK);
            
            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/LapChietTinhSuaChua.rpt");
            rp.Load(path);


            #region Text box

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if(txtKhachHang != null)
                txtKhachHang.Text = chiettinh .TENCT ;
            
            var txtCongTac = rp.ReportDefinition.ReportObjects["txtCongTac"] as TextObject;
            if(txtCongTac!= null)
                txtCongTac.Text = chiettinh .TENHM ;
            
            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if(txtDiaChi != null)
                txtDiaChi.Text = chiettinh.DIACHIHM ;
            
            var txtGhiChu = rp.ReportDefinition.ReportObjects["txtGhiChu"] as TextObject;
            if(txtGhiChu != null)
                txtGhiChu.Text = chiettinh .GHICHU ;
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            txtTitle.Text = "BẢNG DỰ TOÁN CÔNG TÁC SỬA CHỮA";
            #endregion

            #region bo
            #endregion

            #region Set các giá trị
            //Chi phí vật liệu công ty đầu tư
            decimal VL = 0;
            //Chi phí vật liệu khách hàng
            decimal A = 0;
            //Chi phí nhân công cho hai trường hợp trên
            decimal B = 0;
            //Lấy giá trị tiền vật tư công ty, khách hàng, tiền nhân công
            foreach (DataRow dataRow in dt.Tables [0].Rows)
            {
                if(dataRow ["THUTU"].ToString() == "1")
                {
                    VL = VL + (decimal) dataRow["TIENVT"];
                }

                if (dataRow["THUTU"].ToString() == "2")
                {
                    A = A + (decimal)dataRow["TIENVT"];
                }

                B = B + (decimal)dataRow["TIENNC"];
            }
            //Trường hợp có giảm giá chi phí vật liệu 
            if(chiettinh.GIAMGIACPVL != null && chiettinh.GIAMGIACPVL > 0)
            {
                VL = VL - VL*(decimal) chiettinh.GIAMGIACPVL/100;
                A = A - A*(decimal) chiettinh.GIAMGIACPVL/100;
            }

            VL = Math.Round(VL, 0,MidpointRounding.ToEven);
            A = Math.Round(A, 0, MidpointRounding.ToEven);
            //Trường hợp có giảm giá chi phí nhân công)
            if (chiettinh.GIAMGIACPNC != null && chiettinh.GIAMGIACPNC > 0)
            {
                B = B - B*(decimal) chiettinh.GIAMGIACPNC/100;
            }
            B = Math.Round(B, 0, MidpointRounding.ToEven);

            var txtChiPhiVatLieu = rp.ReportDefinition.ReportObjects["txtChiPhiVatLieu"] as TextObject;
            if (txtChiPhiVatLieu != null)
                txtChiPhiVatLieu.Text = string.Format("{0:0,0}", VL).Replace(",", ".");

            var txtChiPhiKhachHang = rp.ReportDefinition.ReportObjects["txtChiPhiKhachHang"] as TextObject;
            if (txtChiPhiKhachHang != null)
                txtChiPhiKhachHang.Text = string.Format("{0:0,0}", A).Replace(",", ".");

            //Chi phí nhân công
            decimal NC = 0;
            NC = B * (decimal)chiettinh.HSNHANCONG;
            NC = Math.Round(NC, 0, MidpointRounding.ToEven);


            var txtHeSoChiPhiNhanCong = rp.ReportDefinition.ReportObjects["txtHeSoChiPhiNhanCong"] as TextObject;
            if (txtHeSoChiPhiNhanCong != null)
                txtHeSoChiPhiNhanCong.Text = string.Format("{0:0.00}", chiettinh.HSNHANCONG).Replace(".", ",") + "  x  " + string.Format("{0:0.00}", chiettinh.HSTHIETKE3).Replace(".", ",") + "  x  (b)";
            var txtChiPhiNhanCong = rp.ReportDefinition.ReportObjects["txtChiPhiNhanCong"] as TextObject;
            if (txtChiPhiNhanCong != null)
                txtChiPhiNhanCong.Text = string.Format("{0:0,0}", NC).Replace(",", ".");
            
            //Chi phí trực tiếp khác 
            decimal TT = 0;
            TT = (VL + A + NC)*(decimal) chiettinh.HSCPC;
            TT = Math.Round(TT, 0, MidpointRounding.ToEven);

            var txtHeSoTrucTiepPhiKhac = rp.ReportDefinition.ReportObjects["txtHeSoTrucTiepPhiKhac"] as TextObject;
            if (txtHeSoTrucTiepPhiKhac != null)
                txtHeSoTrucTiepPhiKhac.Text = string.Format("{0:0.00}", chiettinh.HSCPC*100).Replace(".", ",") + "(%) x (VL + NC)";
            var txtTrucTiepPhiKhac = rp.ReportDefinition.ReportObjects["txtTrucTiepPhiKhac"] as TextObject;
            if (txtTrucTiepPhiKhac != null)
                txtTrucTiepPhiKhac.Text = string.Format("{0:0,0}", TT).Replace(",", ".");
            
            //Cộng chi phí trực tiếp
            decimal T = 0;
            T = VL + A + NC + TT;
            T = Math.Round(T, 0, MidpointRounding.ToEven);
            var txtHeSoCongChiPhiTrucTiep = rp.ReportDefinition.ReportObjects["txtHeSoCongChiPhiTrucTiep"] as TextObject;
            if (txtHeSoCongChiPhiTrucTiep != null)
                txtHeSoCongChiPhiTrucTiep.Text = "(A + VL + NC + TT)";
            var txtCongChiPhiTrucTiep = rp.ReportDefinition.ReportObjects["txtCongChiPhiTrucTiep"] as TextObject;
            if (txtGhiChu != null)
                txtCongChiPhiTrucTiep.Text = string.Format("{0:0,0}", T).Replace(",", ".");

            //Chi phí chung
            decimal C = 0;
            C = T*(decimal) chiettinh.HSCHUNG;
            C = Math.Round(C, 0, MidpointRounding.ToEven);

            var txtHeSoChiPhiChung = rp.ReportDefinition.ReportObjects["txtHeSoChiPhiChung"] as TextObject;
            if (txtHeSoChiPhiChung != null)
                txtHeSoChiPhiChung.Text = string.Format("{0:0.00}", chiettinh.HSCHUNG*100).Replace(".", ",") + "(%) x (T)";
            var txtChiPhiChung = rp.ReportDefinition.ReportObjects["txtChiPhiChung"] as TextObject;
            if (txtChiPhiChung != null)
                txtChiPhiChung.Text = string.Format("{0:0,0}", C).Replace(",", ".");
                     
            //Thu nhập chịu thuế tính trước
            decimal TL = 0;
            TL = (A + NC + TT + C)*(decimal)chiettinh.HSTHUNHAP;
            TL = Math.Round(TL, 0, MidpointRounding.ToEven);

            var txtHeSoThuNhapChiuThueTinhTruoc = rp.ReportDefinition.ReportObjects["txtHeSoThuNhapChiuThueTinhTruoc"] as TextObject;
            if (txtHeSoThuNhapChiuThueTinhTruoc != null)
                txtHeSoThuNhapChiuThueTinhTruoc.Text = "(A + NC + TT + C) x  " + string.Format("{0:0.00}", chiettinh.HSTHUNHAP*100).Replace(".", ",") + ("(%)");
            var txtThuNhapChiuThueTinhTruoc = rp.ReportDefinition.ReportObjects["txtThuNhapChiuThueTinhTruoc"] as TextObject;
            if (txtGhiChu != null)
                txtThuNhapChiuThueTinhTruoc.Text = string.Format("{0:0,0}", TL).Replace(",", "."); ;

            //Thuế giá trị gia tăng đầu ra
            decimal VAT = 0;
            VAT = (A + NC + TT + C + TL)*(decimal) chiettinh.HSTHUE/100;
            VAT = Math.Round(VAT, 0, MidpointRounding.ToEven);

            var txtHeSoThueGiaTriGiaTăngDauRa = rp.ReportDefinition.ReportObjects["txtHeSoThueGiaTriGiaTăngDauRa"] as TextObject;
            if (txtHeSoThueGiaTriGiaTăngDauRa != null)
                txtHeSoThueGiaTriGiaTăngDauRa.Text = "(A + NC + TT + C + TL) x " + string.Format("{0:0.000}", chiettinh.HSTHUE).Replace(".", ",").Substring(0, 4) + " (%) ";
            var txtThueGiaTriGiaTăngDauRa = rp.ReportDefinition.ReportObjects["txtThueGiaTriGiaTăngDauRa"] as TextObject;
            if (txtThueGiaTriGiaTăngDauRa != null)
                txtThueGiaTriGiaTăngDauRa.Text = string.Format("{0:0,0}", VAT).Replace(",", ".");

            //Khảo sát phí
            decimal KS = 0;
            KS = (T + C + TL)*(decimal) chiettinh.HSTHIETKE1*(decimal) chiettinh.HSTHIETKE2;
            KS = Math.Round(KS, 0, MidpointRounding.ToEven);

            var txtHeSoKhaoSatPhi = rp.ReportDefinition.ReportObjects["txtHeSoKhaoSatPhi"] as TextObject;
            if (txtHeSoKhaoSatPhi != null)
                txtHeSoKhaoSatPhi.Text = "(T + C + TL) x " +
                                     string.Format("{0:0.00}", chiettinh.HSTHIETKE1*100).Replace(".", ",") + "(%)  x  " +
                                     string.Format("{0:0.00}", chiettinh.HSTHIETKE2).Replace(".", ",");
            var txtKhaoSatPhi = rp.ReportDefinition.ReportObjects["txtKhaoSatPhi"] as TextObject;
            if (txtKhaoSatPhi != null)
                txtKhaoSatPhi.Text = string.Format("{0:0,0}", KS).Replace(",", ".");

            //Giá trị dự tóan 
            decimal GTDT = A + NC + TT + C + TL + VAT + KS;
            GTDT = Math.Round(GTDT, 0, MidpointRounding.ToEven);

            var txtGTDT = rp.ReportDefinition.ReportObjects["txtGTDT"] as TextObject;
            if (txtGTDT != null)
                txtGTDT.Text = "(A + NC + TT + C + TL + VAT + KS)";
            
            var txtTongDuToan = rp.ReportDefinition.ReportObjects["txtTongDuToan"] as TextObject;
            if (txtTongDuToan != null)
                txtTongDuToan.Text = string.Format("{0:0,0}", GTDT).Replace(",", ".");

            // Làm tròn
            decimal Lamtron = Math.Round(GTDT/1000, 0);
            Lamtron = Lamtron*1000;
            var txtLamTron = rp.ReportDefinition.ReportObjects["txtLamTron"] as TextObject;
            if (txtLamTron != null)
                txtLamTron.Text = string.Format("{0:0,0}", Lamtron).Replace(",", ".");

            var txtBangChu = rp.ReportDefinition.ReportObjects["txtBangChu"] as TextObject;
            if (txtBangChu != null)
                txtBangChu.Text = @"Bằng chữ : " + CommonUtil.DocSoTien(Lamtron);

            
            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgayThangNam"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "Đồng Tháp, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var lapDuToan = rp.ReportDefinition.ReportObjects["txtLapDuToan"] as TextObject;
            if (lapDuToan != null)
                lapDuToan.Text = txtNguoiLap.Text;
            var phongKhkt = rp.ReportDefinition.ReportObjects["txtPhongKHKT"] as TextObject;
            if (phongKhkt != null)
                phongKhkt.Text = txtKHKT.Text.Trim();
            var txtGiamDoc1 = rp.ReportDefinition.ReportObjects["txtGiamDoc"] as TextObject;
            if (txtGiamDoc1 != null)
                txtGiamDoc1.Text = txtGiamDoc.Text.Trim();

            #endregion

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }
            Session["ds_Dondangki"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(ResolveUrl("~") + "Forms/SuaChua/LapChietTinhSuaChua.aspx?" + Constants.PARAM_REPORTED + "=true", false);
        }
    }
}