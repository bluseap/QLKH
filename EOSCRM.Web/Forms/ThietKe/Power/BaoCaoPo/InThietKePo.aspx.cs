using System;
using System.Data;
using EOSCRM.Web.Common;
using System.Globalization;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;
using Message = EOSCRM.Util.Message;
using System.IO;

namespace EOSCRM.Web.Forms.ThietKe.Power.BaoCaoPo
{
    public partial class InThietKePo : Authentication
    {
        private KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private ThietKePoDao _tkpoDao = new ThietKePoDao();

        private ReportDocument rp = new ReportDocument();
        private NhanVienDao _nvDao = new NhanVienDao();        
        private ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    LayBaoCao();
                }
                else
                {
                    if (reloadm.Text == "1")
                    {
                        LayBaoCao();
                    }
                    else if (reloadm.Text == "2")
                    {
                        LayBaoCaoMau();
                    }
                    else if (reloadm.Text == "3")
                    {
                        LayBaoCaoMauLoi();
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
            Page.Title = Resources.Message.TITLE_TK_BAOCAOPO_THIETKEPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_BAOCAOPO_THIETKEPO;
            }
        }

        private void LoadReferences()
        {
            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy");

            if (LoginInfo.NHANVIEN.MANV == "loi")
            {
                linkTKMAU_LOI.Visible = true;
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
        }

        private void LayBaoCao()
        {
            string dbp = "NDBP", dbt = "NDBT", tbp = "NTBP", tbt = "NTBT", dlbp = "NDQLBP", dlbt = "NDQLBT", tlbp = "NTQLBP", tlbt = "NTQLBT";

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];
            var thietke = _tkpoDao.Get(MADDK.ToString());

            BaoCaoGiayTrang();

            /*if (thietke.MAMAUTK == null || thietke.MAMAUTK == "ALL")
           {
               BaoCaoGiayTrang();
           }
           else
           {
               
               if (thietke.MAMAUTK == dbp)
               {
                   DaiBenPhai();
               }
               if (thietke.MAMAUTK == dbt)
               {
                   DaiBenTrai();
               }
               if (thietke.MAMAUTK == tbp)
               {
                   TeBenPhai();
               }
               if (thietke.MAMAUTK == tbt)
               {
                   TeBenTrai();
               }
               if (thietke.MAMAUTK == dlbp)
               {
                   LoDaiBenPhai();
               }
               if (thietke.MAMAUTK == dlbt)
               {
                   LoDaiBenTrai();
               }
               if (thietke.MAMAUTK == tlbp)
               {
                   LoTeBenPhai();
               }
               if (thietke.MAMAUTK == tlbt)
               {
                   LoTeBenTrai();
               }
                
           }*/

        }

        private void LayBaoCaoMau()
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
                catch  {   }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKeMau.rpt");
            rp.Load(path);

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            /*var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
            
            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            */
            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

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

        protected void linkTKMAU_Click(object sender, EventArgs e)
        {
            reloadm.Text = "2";
            LayBaoCaoMau();
        }

        private void LayBaoCaoMauLoi()
        {
            string dhn = "DHN", van = "VAN", pvc = "PVC", kr = "KR", co = "CO", t = "TE", dai = "DAI", keo = "KEO";
            string krt = "KRT21001", krn = "KRN21001", krt27 = "KRT2721001", krn27 = "KRN2721001";
            string keodan = "KEODA001", keonon = "KEONA001";

            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch    {   }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKeMauLoi.rpt");
            rp.Load(path);


            var s_dhn = _cttkDao.GetNhomVT(MADDK, dhn);
            var txtDHN = rp.ReportDefinition.ReportObjects["txtDHN"] as TextObject;
            if (txtDHN != null)
            {
                if (s_dhn != null)
                {
                    txtDHN.Text = Convert.ToInt32(s_dhn.SOLUONG).ToString();
                }
                else { txtDHN.Text = "0"; }
            }
            else { txtDHN.Text = "0"; }

            var s_van2c = _cttkDao.GetNhomVT(MADDK, van);
            var txtVAN2CHIEU = rp.ReportDefinition.ReportObjects["txtVAN2CHIEU"] as TextObject;
            if (txtVAN2CHIEU != null)
            {
                if (s_van2c != null)
                {
                    txtVAN2CHIEU.Text = Convert.ToInt32(s_van2c.SOLUONG).ToString();
                }
                else { txtVAN2CHIEU.Text = "0"; }
            }
            else { txtVAN2CHIEU.Text = "0"; }

            var s_pvc = _cttkDao.GetNhomVT(MADDK, pvc);
            var txtPVC = rp.ReportDefinition.ReportObjects["txtPVC"] as TextObject;
            if (txtPVC != null)
            {
                if (s_pvc != null)
                {
                    txtPVC.Text = Convert.ToInt32(s_pvc.SOLUONG).ToString();
                }
                else { txtPVC.Text = "0"; }
            }
            else { txtPVC.Text = "0"; }

            var s_krt = _cttkDao.Get(MADDK, krt);
            var txtKRTRONG = rp.ReportDefinition.ReportObjects["txtKRTRONG"] as TextObject;
            if (txtKRTRONG != null)
            {
                if (s_krt != null)
                {
                    txtKRTRONG.Text = Convert.ToInt32(s_krt.SOLUONG).ToString();
                }
                else { txtKRTRONG.Text = "0"; }
            }
            else { txtKRTRONG.Text = "0"; }

            var s_krn = _cttkDao.Get(MADDK, krn);
            var txtKRNGOAI = rp.ReportDefinition.ReportObjects["txtKRNGOAI"] as TextObject;
            if (txtKRNGOAI != null)
            {
                if (s_krn != null)
                {
                    txtKRNGOAI.Text = Convert.ToInt32(s_krn.SOLUONG).ToString();
                }
                else { txtKRNGOAI.Text = "0"; }
            }
            else { txtKRNGOAI.Text = "0"; }

            var s_co = _cttkDao.GetNhomVT(MADDK, co);
            var txtCO = rp.ReportDefinition.ReportObjects["txtCO"] as TextObject;
            if (txtCO != null)
            {
                if (s_co != null)
                {
                    txtCO.Text = Convert.ToInt32(s_co.SOLUONG).ToString();
                }
                else { txtCO.Text = "0"; }
            }
            else { txtCO.Text = "0"; }

            var s_t = _cttkDao.GetNhomVT(MADDK, t);
            var txtTE = rp.ReportDefinition.ReportObjects["txtTE"] as TextObject;
            if (txtTE != null)
            {
                if (s_t != null)
                {
                    txtTE.Text = Convert.ToInt32(s_t.SOLUONG).ToString();
                }
                else { txtTE.Text = "0"; }
            }
            else { txtTE.Text = "0"; }

            var s_dai = _cttkDao.GetNhomVT(MADDK, dai);
            var txtDAI = rp.ReportDefinition.ReportObjects["txtDAI"] as TextObject;
            if (txtDAI != null)
            {
                if (s_dai != null)
                {
                    txtDAI.Text = Convert.ToInt32(s_dai.SOLUONG).ToString();
                }
                else { txtDAI.Text = "0"; }
            }
            else { txtDAI.Text = "0"; }

            var s_keodan = _cttkDao.Get(MADDK, keodan);
            var txtKEODAN = rp.ReportDefinition.ReportObjects["txtKEODAN"] as TextObject;
            if (txtKEODAN != null)
            {
                if (s_keodan != null)
                {
                    txtKEODAN.Text = Convert.ToInt32(s_keodan.SOLUONG).ToString();
                }
                else { txtKEODAN.Text = "0"; }
            }
            else { txtKEODAN.Text = "0"; }

            var s_krt27 = _cttkDao.Get(MADDK, krt27);
            var txtKRT27 = rp.ReportDefinition.ReportObjects["txtKRT27"] as TextObject;
            if (txtKRT27 != null)
            {
                if (s_krt27 != null)
                {
                    txtKRT27.Text = Convert.ToInt32(s_krt27.SOLUONG).ToString();
                }
                else { txtKRT27.Text = "0"; }
            }
            else { txtKRT27.Text = "0"; }

            var s_keonon = _cttkDao.Get(MADDK, keonon);
            var txtKEONON = rp.ReportDefinition.ReportObjects["txtKEONON"] as TextObject;
            if (txtKEONON != null)
            {
                if (s_keonon != null)
                {
                    txtKEONON.Text = Convert.ToInt32(s_keonon.SOLUONG).ToString();
                }
                else { txtKEONON.Text = "0"; }
            }
            else { txtKEONON.Text = "0"; }


            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            /*var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
            
            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            */
            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }
            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void linkTKMAU_LOI_Click(object sender, EventArgs e)
        {
            reloadm.Text = "3";
            LayBaoCaoMauLoi();
        }

        private void DaiBenPhai()
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
                catch {  }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_DBP.rpt");
            rp.Load(path);

            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void DaiBenTrai()
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
                catch {  }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_DBT.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void TeBenPhai()
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
                catch   {  }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_TEBP.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void TeBenTrai()
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

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_TEBT.rpt");
            //var path = Server.MapPath("../../../Reports/DonLapDatMoi/VatTuThietKe_TEBT.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void LoDaiBenPhai()
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

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_LDBP.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void LoDaiBenTrai()
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

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_LDBT.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void LoTeBenPhai()
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

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_LTEBP.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void LoTeBenTrai()
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

            var MADDK = (string)Session["NHAPTHIETKE_MADDKPO"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe_LTEBT.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtTENKHP = rp.ReportDefinition.ReportObjects["txtTENKHP"] as TextObject;
            if (txtTENKHP != null)
            {
                if (tk.TENKHPHAI != null)
                    txtTENKHP.Text = tk.TENKHPHAI.ToString();
                else txtTENKHP.Text = "";
            }
            var TXTTENKHT = rp.ReportDefinition.ReportObjects["TXTTENKHT"] as TextObject;
            if (TXTTENKHT != null)
            {
                if (tk.TENKHTRAI != null)
                    TXTTENKHT.Text = tk.TENKHTRAI.ToString();
                else TXTTENKHT.Text = "";
            }
            var txtDSP = rp.ReportDefinition.ReportObjects["txtDSP"] as TextObject;
            if (txtDSP != null)
            {
                if (tk.DANHSOPHAI != null)
                    txtDSP.Text = tk.DANHSOPHAI.ToString();
                else txtDSP.Text = "";
            }
            var txtDST = rp.ReportDefinition.ReportObjects["txtDST"] as TextObject;
            if (txtDST != null)
            {
                if (tk.DANHSOTRAI != null)
                    txtDST.Text = tk.DANHSOTRAI.ToString();
                else txtDST.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var tenkv = _kvpoDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            if (dt.Tables.Count > 0)
            {
                rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();
            }

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void BaoCaoGiayTrang()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var nhanvien = _nvDao.Get(b);

            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch { }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyPoDao().Get(MADDK);
            var tk = new ThietKePoDao().Get(MADDK);

            var dt = new ReportClass().BaoCaoVatTuThietKePo(MADDK);

            rp = new ReportDocument();

            if (nhanvien.MAKV == "O" || nhanvien.MAKV == "M" || nhanvien.MAKV == "Q") // chau thanh-tinh bien-an phu
            {
                var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKeCTPo.rpt");
                rp.Load(path);
            }
            else
            {
                var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKePo.rpt");
                rp.Load(path);
            }

            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                if (ddk.NOILAPDHHN != null)
                    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                else txtDIACHILAPDHN.Text = "";
            }

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH;

            var txtDANHSO = rp.ReportDefinition.ReportObjects["txtDANHSO"] as TextObject;
            if (txtDANHSO != null)
                txtDANHSO.Text = tk.SODB;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = ddk.DIACHILD;

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                //txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                //                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();
                txtNgayThangNam.Text = "An Giang, ngày .... tháng .... năm .....";

            var tenkv = _kvpoDao.GetPo(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                //txtCHUTHICH.Text = tk.CHUTHICH.ToString();
                txtCHUTHICH.Text = tk.KETLUANTK != null ? tk.KETLUANTK.ToString() : "";

            //txtNGAYIN;txtNHANVIENIN
            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();

            var rtxtSOTRUKH = rp.ReportDefinition.ReportObjects["rtxtSOTRUKH"] as TextObject;
            if (rtxtSOTRUKH != null)
            {
                if (tk.SOTRUKH != null)
                    rtxtSOTRUKH.Text = tk.SOTRUKH.ToString();
                else rtxtSOTRUKH.Text = "";
            }
            var rtxtTENTRAMKH = rp.ReportDefinition.ReportObjects["rtxtTENTRAMKH"] as TextObject;
            if (rtxtTENTRAMKH != null)
            {
                if (tk.TENTRAMKH != null)
                    rtxtTENTRAMKH.Text = tk.TENTRAMKH.ToString();
                else rtxtTENTRAMKH.Text = "";
            }
            var rtxtTUYENDAYHATHE = rp.ReportDefinition.ReportObjects["rtxtTUYENDAYHATHE"] as TextObject;
            if (rtxtTUYENDAYHATHE != null)
            {
                if (tk.TUYENDAYHATHE != null)
                    rtxtTUYENDAYHATHE.Text = tk.TUYENDAYHATHE.ToString();
                else rtxtTUYENDAYHATHE.Text = "";
            }

            var rtxtTENKHPHAI = rp.ReportDefinition.ReportObjects["rtxtTENKHPHAI"] as TextObject;
            if (rtxtTENKHPHAI != null)
            {
                if (tk.TENKHPHAI != null)
                    rtxtTENKHPHAI.Text = tk.TENKHPHAI.ToString();
                else rtxtTENKHPHAI.Text = "";
            }
            var rtxtTENKHTRAI = rp.ReportDefinition.ReportObjects["rtxtTENKHTRAI"] as TextObject;
            if (rtxtTENKHTRAI != null)
            {
                if (tk.TENKHTRAI != null)
                    rtxtTENKHTRAI.Text = tk.TENKHTRAI.ToString();
                else rtxtTENKHTRAI.Text = "";
            }
            var rtxtDSKHPHAI = rp.ReportDefinition.ReportObjects["rtxtDSKHPHAI"] as TextObject;
            if (rtxtDSKHPHAI != null)
            {
                if (tk.DANHSOPHAI!= null)
                    rtxtDSKHPHAI.Text = tk.DANHSOPHAI.ToString();
                else rtxtDSKHPHAI.Text = "";
            }
            var rtxtDSKHTRAI = rp.ReportDefinition.ReportObjects["rtxtDSKHTRAI"] as TextObject;
            if (rtxtDSKHTRAI != null)
            {
                if (tk.DANHSOTRAI != null)
                    rtxtDSKHTRAI.Text = tk.DANHSOTRAI.ToString();
                else rtxtDSKHTRAI.Text = "";
            }
            var rtxtTRUTRUOCTEN = rp.ReportDefinition.ReportObjects["rtxtTRUTRUOCTEN"] as TextObject;
            if (rtxtTRUTRUOCTEN != null)
            {
                if (tk.TENTRUPHAI != null)
                    rtxtTRUTRUOCTEN.Text = tk.TENTRUPHAI.ToString();
                else rtxtTRUTRUOCTEN.Text = "";
            }
            var rtxtTRUSAUTEN = rp.ReportDefinition.ReportObjects["rtxtTRUSAUTEN"] as TextObject;
            if (rtxtTRUSAUTEN != null)
            {
                if (tk.TENTRUTRAI != null)
                    rtxtTRUSAUTEN.Text = tk.TENTRUTRAI.ToString();
                else rtxtTRUSAUTEN.Text = "";
            }
            var rtxtTRUTRUOCDSO = rp.ReportDefinition.ReportObjects["rtxtTRUTRUOCDSO"] as TextObject;
            if (rtxtTRUTRUOCDSO != null)
            {
                if (tk.DANHSOTRUPHAI != null)
                    rtxtTRUTRUOCDSO.Text = tk.DANHSOTRUPHAI.ToString();
                else rtxtTRUTRUOCDSO.Text = "";
            }
            var rtxtTRUSAUDSO = rp.ReportDefinition.ReportObjects["rtxtTRUSAUDSO"] as TextObject;
            if (rtxtTRUSAUDSO != null)
            {
                if (tk.DANHSOTRUTRAI != null)
                    rtxtTRUSAUDSO.Text = tk.DANHSOTRUTRAI.ToString();
                else rtxtTRUSAUDSO.Text = "";
            }

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();


            DataTable dtt = dt.Tables[0];
            dtt.Columns.Add("HINH1", typeof(byte[]));

            var hinhtk1 = tk.HINHTK1 != null ? tk.HINHTK1 : "~/UpLoadFile/longxuyen/hinhthietke/tranglx.jpg";
            //                                              ~/UpLoadFile/chauthanh/hinhthietke/1712239858tranglx.jpg

            if (dtt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dtt.Rows)
                {
                    System.Drawing.Image bc = System.Drawing.Image.FromFile(Server.MapPath(hinhtk1), true);
                    byte[] ar = imageToByte(bc);
                    row["HINH1"] = ar;
                }                
            }

            rp.SetDataSource(dtt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();
            
            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private byte[] imageToByte(System.Drawing.Image img)
        {
            MemoryStream objMS = new MemoryStream();
            img.Save(objMS, System.Drawing.Imaging.ImageFormat.Png);
            return objMS.ToArray();
        }

        protected void txtNguoiLap_TextChanged(object sender, EventArgs e)
        {

        }

    }
}