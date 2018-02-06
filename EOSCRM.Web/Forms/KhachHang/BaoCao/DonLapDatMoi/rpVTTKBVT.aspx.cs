using System;
using System.Data;
using EOSCRM.Web.Common;
using System.Globalization;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;
using Message = EOSCRM.Util.Message;
using System.IO;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class rpVTTKBVT : Authentication
    {
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly MucDichSuDungDao _mdsdDao = new MucDichSuDungDao();
        private readonly PhuongDao _pDao = new PhuongDao();
        private readonly GhiChuThietKeDao _gctkDao = new GhiChuThietKeDao();
        private ReportDocument rp = new ReportDocument();
        private NhanVienDao _nvDao = new NhanVienDao();
        private KhuVucDao _kvDao = new KhuVucDao();
        private ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();
        private ThietKeDao _tkDao = new ThietKeDao();

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

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
                    else if (Session[SessionKey.TK_BAOCAO_TED27BENPHAI] == "TK_BAOCAO_TED27BENPHAI")
                    {
                        //var dtte27bp = (DataTable)Session[SessionKey.TK_BAOCAO_TED27BENPHAI];
                        //ReportTeD27BenPhai(dtte27bp);
                        BaoCaoMauThietKe2();
                    }
                    else
                    {
                        var dt = (DataTable)Session[SessionKey.TK_BAOCAO_BOCVATTULX];
                        ReportBVTLX(dt);
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
            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN.ToUpper() : "";
            txtNGAYIN.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtPHONGKYTHUAT.Text = "LƯU THANH VIỆT";

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
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            string dbp = "NDBP", dbt = "NDBT", tbp = "NTBP", tbt= "NTBT", dlbp = "NDQLBP", dlbt = "NDQLBT", tlbp = "NTQLBP", tlbt = "NTQLBT";

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];
            var thietke = _tkDao.Get(MADDK.ToString());

            if (_nvDao.Get(b).MAKV == "X")
            {
                BaoCcaoBVTLX();
            }
            else
            {
                if (thietke.MAMAUTK == null || thietke.MAMAUTK == "ALL")
                {
                    BaoCaoGiayTrang();
                }
                else
                {
                    if (thietke.MAMAUTK == dbp)
                    {
                        DaiBenPhai();
                    }
                    else if (thietke.MAMAUTK == dbt)
                    {
                        DaiBenTrai();
                    }
                    else if (thietke.MAMAUTK == tbp)
                    {
                        TeBenPhai();
                    }
                    else if (thietke.MAMAUTK == tbt)
                    {
                        TeBenTrai();
                    }
                    else if (thietke.MAMAUTK == dlbp)
                    {
                        LoDaiBenPhai();
                    }
                    else if (thietke.MAMAUTK == dlbt)
                    {
                        LoDaiBenTrai();
                    }
                    else if (thietke.MAMAUTK == tlbp)
                    {
                        LoTeBenPhai();
                    }
                    else if (thietke.MAMAUTK == tlbt)
                    {
                        LoTeBenTrai();
                    }
                    else
                    {
                        //BaoCaoMauThietKe();
                        BaoCaoMauThietKe2();
                    }                   
                }
            }            
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
                catch   {     }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

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
                catch
                {

                }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);            
            
            var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKeMauLoi.rpt");
            rp.Load(path);


            var s_dhn = _cttkDao.GetNhomVT(MADDK,dhn);
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
                catch    {       }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_DBP.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch       {        }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_DBT.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch       {       }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_TEBP.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch   {     }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_TEBT.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch         {          }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_LDBP.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch         {       }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_LDBT.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ?  tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch       {        }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_LTEBP.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
                catch      {     }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_LTEBT.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

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
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch {   }
            }

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            //var dt = new ReportClass().BaoCaoVatTuThietKe(MADDK);
            var dt = new ReportClass().BaoCaoVTTK(MADDK); 

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/VatTuThietKe.rpt");
            rp.Load(path);


            //txtDIACHILAPDHN
            var txtDIACHILAPDHN = rp.ReportDefinition.ReportObjects["txtDIACHILAPDHN"] as TextObject;
            if (txtDIACHILAPDHN != null)
            {
                txtDIACHILAPDHN.Text = ddk.DIACHILD != null ? ddk.DIACHILD+" "+ (ddk.NOILAPDHHN != null ? ddk.NOILAPDHHN : "") : "";
                //if (ddk.NOILAPDHHN != null)
                //    txtDIACHILAPDHN.Text = ddk.NOILAPDHHN.ToString();
                //else txtDIACHILAPDHN.Text = "";
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

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK.ToString();

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH.ToString();

            //txtNGAYIN;txtNHANVIENIN
            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

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

        private void BaoCcaoBVTLX()
        {
            try
            {                
                var MADDK = (string)Session["NHAPTHIETKE_MADDK"];
                
                var tk = _tkDao.Get(MADDK);

                if (tk.ISKHTT100 == true)
                {
                    var ds = new ReportClass().BaoCaoBVTCTLXKHTT100(MADDK, "", "");

                    if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }

                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("HINH1", typeof(byte[]));
                    dt.Columns.Add("HINH2", typeof(byte[]));

                    ReportBVTLX(dt);     
                }
                else
                {
                    var ds = new ReportClass().BaoCaoBVTCTLX(MADDK, "", "");

                    if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }

                    DataTable dt = ds.Tables[0];
                    dt.Columns.Add("HINH1", typeof(byte[]));
                    dt.Columns.Add("HINH2", typeof(byte[]));

                    ReportBVTLX(dt);     
                }                           

                CloseWaitingDialog();  
            }
            catch { }
        }

        private void ReportBVTLX(DataTable dt)
        {
            if (dt == null)
                return;

            #region FreeMemory
            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }             
                catch    {  }
            }
            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_LX.rpt");
            rp.Load(path);

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];
            var hinhtk1 = _tkDao.Get(MADDK).HINHTK1 != null ? _tkDao.Get(MADDK).HINHTK1 : "~/UpLoadFile/longxuyen/hinhthietke/tranglx.jpg";
            var hinhtk2 = _tkDao.Get(MADDK).HINHTK2 != null ? _tkDao.Get(MADDK).HINHTK2 : "~/UpLoadFile/longxuyen/hinhthietke/tranglx.jpg"; 
            
            if (dt.Rows.Count > 0)
            {
                foreach (System.Data.DataRow row in dt.Rows)
                {
                    System.Drawing.Image bc = System.Drawing.Image.FromFile(Server.MapPath(hinhtk1), true);
                    byte[] ar = imageToByte(bc);
                    row["HINH1"] = ar;

                    System.Drawing.Image bc2 = System.Drawing.Image.FromFile(Server.MapPath(hinhtk2), true);
                    byte[] ar2 = imageToByte(bc2);
                    row["HINH2"] = ar2;
                }
            }

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN CẤP NƯỚC " + tenkv.ToString().ToUpper();

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = ddk.TENKH != null ? ddk.TENKH.ToString() : "";            

            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();                  

            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgayThangNam != null)
                txtNgayThangNam.Text = "An Giang, ngày " + DateTime.Now.Day.ToString() + " tháng " +
                                   DateTime.Now.Month.ToString() + " năm " + DateTime.Now.Year.ToString();

            var gcthietkeT = _gctkDao.GetMaDonKyHieu(MADDK, "T");
            var rpT = rp.ReportDefinition.ReportObjects["rpT"] as TextObject;
            if (rpT != null)
                rpT.Text = gcthietkeT.THANHTIEN != null ? Convert.ToInt64(gcthietkeT.THANHTIEN).ToString() : "0"; 
            
            var gcthietkeC = _gctkDao.GetMaDonKyHieu(MADDK, "C");
            var rpC = rp.ReportDefinition.ReportObjects["rpC"] as TextObject;
            if (rpC != null)
                rpC.Text = gcthietkeC.THANHTIEN != null ? Convert.ToInt64(gcthietkeC.THANHTIEN).ToString() : "0";

            var gcthietkeTL = _gctkDao.GetMaDonKyHieu(MADDK, "TL");
            var rpTL = rp.ReportDefinition.ReportObjects["rpTL"] as TextObject;
            if (rpTL != null)
                rpTL.Text = gcthietkeTL.THANHTIEN != null ? Convert.ToInt64(gcthietkeTL.THANHTIEN).ToString() : "0";

            var gcthietkeG = _gctkDao.GetMaDonKyHieu(MADDK, "G");
            var rpG = rp.ReportDefinition.ReportObjects["rpG"] as TextObject;
            if (rpG != null)
                rpG.Text = gcthietkeG.THANHTIEN != null ? Convert.ToInt64(gcthietkeG.THANHTIEN).ToString() : "0";

            var gcthietkeVAT1 = _gctkDao.GetMaDonKyHieu(MADDK, "VAT1");
            var rpVAT1 = rp.ReportDefinition.ReportObjects["rpVAT1"] as TextObject;
            if (rpVAT1 != null)
                rpVAT1.Text = gcthietkeVAT1.THANHTIEN != null ? Convert.ToInt64(gcthietkeVAT1.THANHTIEN).ToString() : "0";

            var gcthietkeG1 = _gctkDao.GetMaDonKyHieu(MADDK, "G1");
            var rpG1 = rp.ReportDefinition.ReportObjects["rpG1"] as TextObject;
            if (rpG1 != null)
                rpG1.Text = gcthietkeG1.THANHTIEN != null ? Convert.ToInt64(gcthietkeG1.THANHTIEN).ToString() : "0";

            var gcthietkeTK = _gctkDao.GetMaDonKyHieu(MADDK, "TK");
            var rpTK = rp.ReportDefinition.ReportObjects["rpTK"] as TextObject;
            if (rpTK != null)
                rpTK.Text = gcthietkeTK.THANHTIEN != null ? Convert.ToInt64(gcthietkeTK.THANHTIEN).ToString() : "0";

            var gcthietkeVAT2 = _gctkDao.GetMaDonKyHieu(MADDK, "VAT2");
            var rpVAT2 = rp.ReportDefinition.ReportObjects["rpVAT2"] as TextObject;
            if (rpVAT2 != null)
                rpVAT2.Text = gcthietkeVAT2.THANHTIEN != null ? Convert.ToInt64(gcthietkeVAT2.THANHTIEN).ToString() : "0";

            var gcthietkeG2 = _gctkDao.GetMaDonKyHieu(MADDK, "G2");
            var rpG2 = rp.ReportDefinition.ReportObjects["rpG2"] as TextObject;
            if (rpG2 != null)
                rpG2.Text = gcthietkeG2.THANHTIEN != null ? Convert.ToInt64(gcthietkeG2.THANHTIEN).ToString() : "0";

            var gcthietkeVT = _gctkDao.GetMaDonKyHieu(MADDK, "VT");
            var rpVT = rp.ReportDefinition.ReportObjects["rpVT"] as TextObject;
            if (rpVT != null)
                rpVT.Text = gcthietkeVT.THANHTIEN != null ? Convert.ToInt64(gcthietkeVT.THANHTIEN).ToString() : "0";

            var gcthietkeVAT3 = _gctkDao.GetMaDonKyHieu(MADDK, "VAT3");
            var rpVAT3 = rp.ReportDefinition.ReportObjects["rpVAT3"] as TextObject;
            if (rpVAT3 != null)
                rpVAT3.Text = gcthietkeVAT3.THANHTIEN != null ? Convert.ToInt64(gcthietkeVAT3.THANHTIEN).ToString() : "0";

            var gcthietkeG3 = _gctkDao.GetMaDonKyHieu(MADDK, "G3");
            var rpG3 = rp.ReportDefinition.ReportObjects["rpG3"] as TextObject;
            if (rpG3 != null)
                rpG3.Text = gcthietkeG3.THANHTIEN != null ? Convert.ToInt64(gcthietkeG3.THANHTIEN).ToString() : "0";

            var gcthietkeVC = _gctkDao.GetMaDonKyHieu(MADDK, "VC");
            var rpVC = rp.ReportDefinition.ReportObjects["rpVC"] as TextObject;
            if (rpVC != null)
                rpVC.Text = gcthietkeVC.THANHTIEN != null ? Convert.ToInt64(gcthietkeVC.THANHTIEN).ToString() : "0";

            var gcthietkeVAT4 = _gctkDao.GetMaDonKyHieu(MADDK, "VAT4");
            var rpVAT4 = rp.ReportDefinition.ReportObjects["rpVAT4"] as TextObject;
            if (rpVAT4 != null)
                rpVAT4.Text = gcthietkeVAT4.THANHTIEN != null ? Convert.ToInt64(gcthietkeVAT4.THANHTIEN).ToString() : "0";

            var gcthietkeG4 = _gctkDao.GetMaDonKyHieu(MADDK, "G4");
            var rpG4 = rp.ReportDefinition.ReportObjects["rpG4"] as TextObject;
            if (rpG4 != null)
                rpG4.Text = gcthietkeG4.THANHTIEN != null ? Convert.ToInt64(gcthietkeG4.THANHTIEN).ToString() : "0";

            decimal? gcthietkeG1234 = gcthietkeG1.THANHTIEN + gcthietkeG2.THANHTIEN + gcthietkeG3.THANHTIEN + gcthietkeG4.THANHTIEN;
            var rpG1234 = rp.ReportDefinition.ReportObjects["rpG1234"] as TextObject;
            if (rpG1234 != null)
                rpG1234.Text = gcthietkeG1234 != null ? Convert.ToInt64(gcthietkeG1234).ToString() : "0";

            decimal? tgtxlttrp = gcthietkeG.THANHTIEN + gcthietkeTK.THANHTIEN + gcthietkeVT.THANHTIEN;
            var rpGTXLTT = rp.ReportDefinition.ReportObjects["rpGTXLTT"] as TextObject;
            if (rpGTXLTT != null)
                rpGTXLTT.Text = tgtxlttrp != null ? Convert.ToInt64(tgtxlttrp).ToString() : "0";
            var rpGTGTXL = rp.ReportDefinition.ReportObjects["rpGTGTXL"] as TextObject;
            if (rpGTGTXL != null)
                rpGTGTXL.Text = tgtxlttrp != null ? Convert.ToInt64(tgtxlttrp * (decimal)0.1).ToString() : "0";
            var rpGTXLST = rp.ReportDefinition.ReportObjects["rpGTXLST"] as TextObject;
            if (rpGTXLST != null)
                rpGTXLST.Text = tgtxlttrp != null ? Convert.ToInt64(tgtxlttrp + (tgtxlttrp * (decimal)0.1)).ToString() : "0";

            var rpCPVCTT = rp.ReportDefinition.ReportObjects["rpCPVCTT"] as TextObject;
            if (rpCPVCTT != null)
                rpCPVCTT.Text = tgtxlttrp != null ? Convert.ToInt64(gcthietkeVC.THANHTIEN).ToString() : "0";
            var rpGTGTVC = rp.ReportDefinition.ReportObjects["rpGTGTVC"] as TextObject;
            if (rpGTGTVC != null)
                rpGTGTVC.Text = tgtxlttrp != null ? Convert.ToInt64(gcthietkeVAT4.THANHTIEN).ToString() : "0";
            var rpTCPVCST = rp.ReportDefinition.ReportObjects["rpTCPVCST"] as TextObject;
            if (rpTCPVCST != null)
                rpTCPVCST.Text = tgtxlttrp != null ? Convert.ToInt64(gcthietkeG4.THANHTIEN).ToString() : "0";

            var rpTONGCONGST = rp.ReportDefinition.ReportObjects["rpTONGCONGST"] as TextObject;
            if (rpTONGCONGST != null)
                rpTONGCONGST.Text = tgtxlttrp != null ? string.Format("{0 : 0,0}", Convert.ToInt64(gcthietkeG1234)) : "0";

            var rpTONGCONGCHU = rp.ReportDefinition.ReportObjects["rpTONGCONGCHU"] as TextObject;
            if (rpTONGCONGCHU != null)
                rpTONGCONGCHU.Text = NumberToTextVN((decimal) gcthietkeG1234);
            
            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = tk.DIACHITK != null ? tk.DIACHITK : "";

            string duonghem = tk.DUONGHEMTK != null ? tk.DUONGHEMTK : "";
            string tendp = !string.IsNullOrEmpty(tk.MADPLX) ? (_dpDao.GetDP(tk.MADPLX) != null ? _dpDao.GetDP(tk.MADPLX).TENDP : "" ): "";
            var rptxtDUONGHEM = rp.ReportDefinition.ReportObjects["rptxtDUONGHEM"] as TextObject;
            if (rptxtDUONGHEM != null)
                rptxtDUONGHEM.Text = duonghem + " " + tendp;

            var rptxtPHUONGXA = rp.ReportDefinition.ReportObjects["rptxtPHUONGXA"] as TextObject;
            if (rptxtPHUONGXA != null)
                rptxtPHUONGXA.Text = tk.PHUONGTK != null ? (_pDao.GetMAKV(tk.PHUONGTK, LoginInfo.NHANVIEN.MAKV) != null ? _pDao.GetMAKV(tk.PHUONGTK, LoginInfo.NHANVIEN.MAKV).TENPHUONG : "") : "";

            var rptxtMDSD = rp.ReportDefinition.ReportObjects["rptxtMDSD"] as TextObject;
            if (rptxtMDSD != null)
                rptxtMDSD.Text = ddk.MAMDSD != null ? (_mdsdDao.Get(ddk.MAMDSD) != null ? _mdsdDao.Get(ddk.MAMDSD).TENMDSD : "") : "";

            var txtDIENTHOAI = rp.ReportDefinition.ReportObjects["txtDIENTHOAI"] as TextObject;
            if (txtDIENTHOAI != null)
                txtDIENTHOAI.Text = tk.SDTTK != null ? tk.SDTTK : "";

            var rptxtVITRITK = rp.ReportDefinition.ReportObjects["rptxtVITRITK"] as TextObject;
            if (rptxtVITRITK != null)
                rptxtVITRITK.Text = tk.VITRIDHTK != null ? tk.VITRIDHTK : "";

            var rptxtDANHSOTK = rp.ReportDefinition.ReportObjects["rptxtDANHSOTK"] as TextObject;
            if (rptxtDANHSOTK != null)
                rptxtDANHSOTK.Text = tk.DANHSOTK != null ? tk.DANHSOTK : "";

            var rptxtTENTK = rp.ReportDefinition.ReportObjects["rptxtTENTK"] as TextObject;
            if (rptxtTENTK != null)
                rptxtTENTK.Text = txtNguoiLap.Text.Trim();

            var rptxtPHONGKT = rp.ReportDefinition.ReportObjects["rptxtPHONGKT"] as TextObject;
            if (rptxtPHONGKT != null)
                rptxtPHONGKT.Text = txtPHONGKYTHUAT.Text.Trim();

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session[SessionKey.TK_BAOCAO_BOCVATTULX] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private byte[] imageToByte(System.Drawing.Image img)
        {
            MemoryStream objMS = new MemoryStream();
            img.Save(objMS, System.Drawing.Imaging.ImageFormat.Png);
            return objMS.ToArray();
        }

        public string NumberToTextVN(decimal total)
        {
            try
            {
                string rs = "";
                total = Math.Round(total, 0);
                string[] ch = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
                string[] rch = { "lẻ", "mốt", "", "", "", "lăm" };
                string[] u = { "", "mươi", "trăm", "ngàn", "", "", "triệu", "", "", "tỷ", "", "", "ngàn", "", "", "triệu" };
                string nstr = total.ToString();

                int[] n = new int[nstr.Length];
                int len = n.Length;
                for (int i = 0; i < len; i++)
                {
                    n[len - 1 - i] = Convert.ToInt32(nstr.Substring(i, 1));
                }

                for (int i = len - 1; i >= 0; i--)
                {
                    if (i % 3 == 2)// số 0 ở hàng trăm
                    {
                        if (n[i] == 0 && n[i - 1] == 0 && n[i - 2] == 0) continue;//nếu cả 3 số là 0 thì bỏ qua không đọc
                    }
                    else if (i % 3 == 1) // số ở hàng chục
                    {
                        if (n[i] == 0)
                        {
                            if (n[i - 1] == 0) { continue; }// nếu hàng chục và hàng đơn vị đều là 0 thì bỏ qua.
                            else
                            {
                                rs += " " + rch[n[i]]; continue;// hàng chục là 0 thì bỏ qua, đọc số hàng đơn vị
                            }
                        }
                        if (n[i] == 1)//nếu số hàng chục là 1 thì đọc là mười
                        {
                            rs += " mười"; continue;
                        }
                    }
                    else if (i != len - 1)// số ở hàng đơn vị (không phải là số đầu tiên)
                    {
                        if (n[i] == 0)// số hàng đơn vị là 0 thì chỉ đọc đơn vị
                        {
                            if (i + 2 <= len - 1 && n[i + 2] == 0 && n[i + 1] == 0) continue;
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 1)// nếu là 1 thì tùy vào số hàng chục mà đọc: 0,1: một / còn lại: mốt
                        {
                            rs += " " + ((n[i + 1] == 1 || n[i + 1] == 0) ? ch[n[i]] : rch[n[i]]);
                            rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);
                            continue;
                        }
                        if (n[i] == 5) // cách đọc số 5
                        {
                            if (n[i + 1] != 0) //nếu số hàng chục khác 0 thì đọc số 5 là lăm
                            {
                                rs += " " + rch[n[i]];// đọc số 
                                rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                                continue;
                            }
                        }
                    }

                    rs += (rs == "" ? " " : ", ") + ch[n[i]];// đọc số
                    rs += " " + (i % 3 == 0 ? u[i] : u[i % 3]);// đọc đơn vị
                }
                if (rs[rs.Length - 1] != ' ')
                    rs += " đồng";
                else
                    rs += "đồng";

                if (rs.Length > 2)
                {
                    string rs1 = rs.Substring(0, 2);
                    rs1 = rs1.ToUpper();
                    rs = rs.Substring(2);
                    rs = rs1 + rs;
                }
                return rs.Trim().Replace("lẻ,", "lẻ").Replace("mươi,", "mươi").Replace("trăm,", "trăm").Replace("mười,", "mười");
            }
            catch
            {
                return "";
            }

        }

        private void BaoCaoMauThietKe()
        {
            try
            {     
                var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

                if (string.IsNullOrEmpty(MADDK))
                    Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");
                
                var dt = new ReportClass().BaoCaoVTTK(MADDK);

                if (dt == null || dt.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportTeD27BenPhai(dt.Tables[0]);
                
                CloseWaitingDialog();
            }
            catch (Exception ex) {  }
        }

        private void ReportTeD27BenPhai(DataTable dt)
        {
            if (dt == null)
                return;

            var maddk = (string)Session["NHAPTHIETKE_MADDK"];
            if (string.IsNullOrEmpty(maddk))
                    Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var ddk = new DonDangKyDao().Get(maddk);
            var tk = new ThietKeDao().Get(maddk);

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
                catch { }
            }
            #endregion          

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKeUpHinh.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = maddk.ToString();

            // hinh tk theo mau           
            DataTable dtt = dt;
            dtt.Columns.Add("HINH1", typeof(byte[]));

            var hinhtk1 = tk.HINHTK1 != null && tk.HINHTK1.Length > 15 ? tk.HINHTK1 : "~/UpLoadFile/longxuyen/hinhthietke/tranglx.jpg";
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

            Session[SessionKey.TK_BAOCAO_TED27BENPHAI] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void BaoCaoMauThietKe2()
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
                catch { }
            }
            #endregion

            var MADDK = (string)Session["NHAPTHIETKE_MADDK"];

            if (string.IsNullOrEmpty(MADDK))
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx");

            var dt = new ReportClass().BaoCaoVTTK(MADDK);

            if (dt == null || dt.Tables.Count == 0) { CloseWaitingDialog(); return; }

            var ddk = new DonDangKyDao().Get(MADDK);
            var tk = new ThietKeDao().Get(MADDK);            

            rp = new ReportDocument();

            var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKeUpHinh.rpt");
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
                txtNgayThangNam.Text = "An Giang, ngày......tháng.....năm....";

            var tenkv = _kvDao.Get(LoginInfo.NHANVIEN.MAKV).TENKV;
            TextObject txtTENKV = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            if (txtTENKV != null)
                txtTENKV.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();

            var txtLYDOTK = rp.ReportDefinition.ReportObjects["txtLYDOTK"] as TextObject;
            if (txtLYDOTK != null)
                txtLYDOTK.Text = tk.TENTK != null ? tk.TENTK.ToString() : "";

            var txtCHUTHICH = rp.ReportDefinition.ReportObjects["txtGHICHU"] as TextObject;
            if (txtCHUTHICH != null)
                txtCHUTHICH.Text = tk.CHUTHICH != null ? tk.CHUTHICH.ToString() : "";

            var txtNGAYIN = rp.ReportDefinition.ReportObjects["txtNGAYIN"] as TextObject;
            if (txtNGAYIN != null)
                txtNGAYIN.Text = txtNGAYIN.Text.ToString();

            var txtNHANVIENIN = rp.ReportDefinition.ReportObjects["txtNHANVIENIN"] as TextObject;
            if (txtNHANVIENIN != null)
                txtNHANVIENIN.Text = txtNguoiLap.Text.ToString();
            var txtNHANVIENIN2 = rp.ReportDefinition.ReportObjects["txtNHANVIENIN2"] as TextObject;
            if (txtNHANVIENIN2 != null)
                txtNHANVIENIN2.Text = txtNguoiLap.Text.ToString();

            //txtMADDKRP
            var txtMADDKRP = rp.ReportDefinition.ReportObjects["txtMADDKRP"] as TextObject;
            if (txtMADDKRP != null)
                txtMADDKRP.Text = MADDK.ToString();

            // hinh tk theo mau           
            DataTable dtt = dt.Tables[0];
            dtt.Columns.Add("HINH1", typeof(byte[]));

            var hinhtk1 = tk.HINHTK1 != null && tk.HINHTK1.Length > 15 ? tk.HINHTK1 : "~/UpLoadFile/longxuyen/hinhthietke/tranglx.jpg";
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

            Session[SessionKey.TK_BAOCAO_TED27BENPHAI] = "TK_BAOCAO_TED27BENPHAI";
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void txtNguoiLap_TextChanged(object sender, EventArgs e)
        {

        }

    }
}