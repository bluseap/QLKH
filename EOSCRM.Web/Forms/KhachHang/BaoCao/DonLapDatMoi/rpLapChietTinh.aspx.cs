using System;
using System.Data;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using System.Globalization;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using Message=EOSCRM.Util.Message;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class rpLapChietTinh : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly DonDangKyDao _ddkDao = new DonDangKyDao();
        private readonly PhuongDao _pDao = new PhuongDao();
        
        string tp;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_LapChietTinh, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    Report();
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.TK_BAOCAO_LAPCHIETTINH];
                    Report();
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

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            Report();
            CloseWaitingDialog();
        }

        
        private void Report()
        {
            var MADDK = (string)Session["N_LAPCHIETTINH_MADDK"];
            var chiettinh = new ChietTinhDao().Get(MADDK);

            var mp = _ddkDao.Get(MADDK);

            string makv = _nvDao.Get(LoginInfo.MANV).MAKV;
            if (mp.MAPHUONG != null)
            {
                var mm = _pDao.GetMAKV(mp.MAPHUONG, makv);
                tp = mm.TENPHUONG.ToString();
            }
            else { tp = ""; }
            

            if (chiettinh == null)
            {
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx", false);
                return;
            }

            var ds = new ReportClass().BaoCaoLapChietTinh(MADDK);

            if (ds == null || ds.Tables.Count == 0) { return; }
            
            var dt = ds.Tables[0];

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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/LapChietTinh.rpt");
            rp.Load(path);

            #region Text box    txtVATTUPVC, txtVATTUPVCVAT, txtVATTUSTK, txtVATTUSTKVAT, txtNHANCONG, txtNHANCONGVAT, txtTongCong, txtBangChu

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = chiettinh.TENCT;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = chiettinh.DONDANGKY.DIACHILD;

            decimal pvc = 0;
            decimal thuepvc = 0, tthuepvc = 0;
            decimal stk = 0;
            decimal thuestk = 0, tthuestk=0;
            decimal nc = 0;
            decimal thuenc = 0, tthuenc=0;
            decimal tongvattu = 0;

            decimal pvc_117 = 0;
            decimal stk_117 = 0;
            decimal nc_117 = 0;
            decimal tongvattu_117 = 0;

            foreach (DataRow row in dt.Rows)
            {
                //if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() == "PVC")
                if (row["THUTU"].ToString() == "2")
                    pvc = pvc + (decimal) row["TIENVT"];

                if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() == "DAOLAP")
                    //stk = stk + (decimal) row["TIENVT"];
                    stk = stk + (decimal)row["TIENNC"];

                if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() != "DAOLAP")
                    nc = nc + (decimal) row["TIENNC"];

                if (row["THUTU"].ToString() == "1")
                    pvc_117 = pvc_117 + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "1" && row["MANHOM"].ToString() == "STK")
                    stk_117 = stk_117 + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "1")
                    nc_117 = nc_117 + (decimal)row["TIENNC"];
            }

            //thuepvc = Math.Round(pvc * 10 / 100, 0);
            //thuestk = Math.Round(stk * 10 / 100, 0);
            //thuenc = Math.Round(nc * 10 / 100, 0);
            thuepvc = Math.Round(pvc / 11, 0); tthuepvc = Math.Round(pvc - thuepvc, 0);
            thuestk = Math.Round(stk / 11, 0); tthuestk = Math.Round(stk - thuestk,0);
            thuenc = Math.Round(nc / 11, 0); tthuenc = Math.Round(nc - thuenc,0);
            //tongvattu = pvc + thuepvc + stk + thuestk + nc + thuenc;
            tongvattu = tthuepvc + thuepvc + tthuestk + thuestk + tthuenc + thuenc;

            tongvattu_117 = pvc_117 + stk_117 + nc_117;

            //VTU KH
            var txtVATTUPVC = rp.ReportDefinition.ReportObjects["txtVATTUPVC"] as TextObject;
            if (txtVATTUPVC != null)
                txtVATTUPVC.Text = string.Format("{0:0,0}", pvc).Replace(",", ".");

            //nc kh
            var daolap117 = new DaoLapChietTinhNd117Dao().GetVXT(MADDK);   
              
            var txtNHANCONG = rp.ReportDefinition.ReportObjects["txtNHANCONG"] as TextObject;
            if (daolap117 != null)
            {
                if (txtNHANCONG != null)
                    txtNHANCONG.Text = string.Format("{0:0,0}", daolap117.THANHTIENCP).Replace(",", ".");
            }
            else {txtNHANCONG.Text = "0";}
            //vc kh
            var daolap117vc = new DaoLapChietTinhNd117Dao().GetVC(MADDK);
            var txtCPVC = rp.ReportDefinition.ReportObjects["txtCPVC"] as TextObject;

            if (daolap117vc != null)
            {
                if (txtCPVC != null)
                    txtCPVC.Text = string.Format("{0:0,0}", daolap117vc.THANHTIENCP).Replace(",", ".");
            }
            else { txtCPVC.Text = "0"; }
            //VTu CTY
            var txtVATTUPVC_117 = rp.ReportDefinition.ReportObjects["txtVATTUPVC_117"] as TextObject;
            if (txtVATTUPVC_117 != null)
                txtVATTUPVC_117.Text = string.Format("{0:0,0}", pvc_117).Replace(",", ".");

            // tong tien kh tra
          
            var d1 = daolap117 == null ? 0 : daolap117.THANHTIENCP;            
            var d2 = daolap117vc == null ? 0 : daolap117vc.THANHTIENCP;

            var  tongtienkhtra = pvc 
                    + d1
                    + d2;
                var txtTongCong = rp.ReportDefinition.ReportObjects["txtTongCong"] as TextObject;

                if (tongtienkhtra != null)
                {
                    if (txtTongCong != null)
                        txtTongCong.Text = string.Format("{0:0,0}", tongtienkhtra).Replace(",", ".");
                }
            


            // tong tien cong ty dau tu
            var txtTongCong_117 = rp.ReportDefinition.ReportObjects["txtTongCong_117"] as TextObject;
            if (txtTongCong_117 != null)
                txtTongCong_117.Text = string.Format("{0:0,0}", pvc_117).Replace(",", ".");
            
            var txtBangChu = rp.ReportDefinition.ReportObjects["txtBangChu"] as TextObject;
            if (txtBangChu != null)
                txtBangChu.Text = CommonUtil.DocSoTien(Convert.ToDecimal(tongtienkhtra));

            var kvnv = new NhanVienDao().Get(LoginInfo.MANV);
            var kv = new KhuVucDao().Get(kvnv.MAKV);
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + kv.TENKV.ToUpper();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            TextObject lm = rp.ReportDefinition.ReportObjects["txtLAPMOI"] as TextObject;
            lm.Text = "LM";

            TextObject phuong1 = rp.ReportDefinition.ReportObjects["txtPHUONG"] as TextObject;
            phuong1.Text = tp;

            #endregion

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session[SessionKey.TK_BAOCAO_LAPCHIETTINH] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/LapChietTinh.aspx?" + Constants.PARAM_REPORTED + "=true", false);
        }

        private void ReLoadBaoCao()
        {
            var MADDK = (string)Session["N_LAPCHIETTINH_MADDK"];
            var chiettinh = new ChietTinhDao().Get(MADDK);

            var mp = _ddkDao.Get(MADDK);

            if (mp.MAPHUONG != null)
            {
                var mm = _pDao.Get(mp.MAPHUONG);
                tp = mm.TENPHUONG.ToString();
            }
            else { tp = ""; }


            if (chiettinh == null)
            {
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx", false);
                return;
            }

            var ds = new ReportClass().BaoCaoLapChietTinh(MADDK);

            if (ds == null || ds.Tables.Count == 0) { return; }

            var dt = ds.Tables[0];

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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/LapChietTinh.rpt");
            rp.Load(path);

            #region Text box    txtVATTUPVC, txtVATTUPVCVAT, txtVATTUSTK, txtVATTUSTKVAT, txtNHANCONG, txtNHANCONGVAT, txtTongCong, txtBangChu

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = chiettinh.TENCT;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = chiettinh.DONDANGKY.DIACHILD;

            decimal pvc = 0;
            decimal thuepvc = 0, tthuepvc = 0;
            decimal stk = 0;
            decimal thuestk = 0, tthuestk = 0;
            decimal nc = 0;
            decimal thuenc = 0, tthuenc = 0;
            decimal tongvattu = 0;

            decimal pvc_117 = 0;
            decimal stk_117 = 0;
            decimal nc_117 = 0;
            decimal tongvattu_117 = 0;

            foreach (DataRow row in dt.Rows)
            {
                //if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() == "PVC")
                if (row["THUTU"].ToString() == "2")
                    pvc = pvc + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() == "DAOLAP")
                    //stk = stk + (decimal) row["TIENVT"];
                    stk = stk + (decimal)row["TIENNC"];

                if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() != "DAOLAP")
                    nc = nc + (decimal)row["TIENNC"];

                if (row["THUTU"].ToString() == "1")
                    pvc_117 = pvc_117 + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "1" && row["MANHOM"].ToString() == "STK")
                    stk_117 = stk_117 + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "1")
                    nc_117 = nc_117 + (decimal)row["TIENNC"];
            }

            //thuepvc = Math.Round(pvc * 10 / 100, 0);
            //thuestk = Math.Round(stk * 10 / 100, 0);
            //thuenc = Math.Round(nc * 10 / 100, 0);
            thuepvc = Math.Round(pvc / 11, 0); tthuepvc = Math.Round(pvc - thuepvc, 0);
            thuestk = Math.Round(stk / 11, 0); tthuestk = Math.Round(stk - thuestk, 0);
            thuenc = Math.Round(nc / 11, 0); tthuenc = Math.Round(nc - thuenc, 0);
            //tongvattu = pvc + thuepvc + stk + thuestk + nc + thuenc;
            tongvattu = tthuepvc + thuepvc + tthuestk + thuestk + tthuenc + thuenc;

            tongvattu_117 = pvc_117 + stk_117 + nc_117;

            //VTU KH
            var txtVATTUPVC = rp.ReportDefinition.ReportObjects["txtVATTUPVC"] as TextObject;
            if (txtVATTUPVC != null)
                txtVATTUPVC.Text = string.Format("{0:0,0}", pvc).Replace(",", ".");

            //nc kh
            var daolap117 = new DaoLapChietTinhNd117Dao().GetVXT(MADDK);

            var txtNHANCONG = rp.ReportDefinition.ReportObjects["txtNHANCONG"] as TextObject;
            if (daolap117 != null)
            {
                if (txtNHANCONG != null)
                    txtNHANCONG.Text = string.Format("{0:0,0}", daolap117.THANHTIENCP).Replace(",", ".");
            }
            else { txtNHANCONG.Text = "0"; }
            //vc kh
            var daolap117vc = new DaoLapChietTinhNd117Dao().GetVC(MADDK);
            var txtCPVC = rp.ReportDefinition.ReportObjects["txtCPVC"] as TextObject;

            if (daolap117vc != null)
            {
                if (txtCPVC != null)
                    txtCPVC.Text = string.Format("{0:0,0}", daolap117vc.THANHTIENCP).Replace(",", ".");
            }
            else { txtCPVC.Text = "0"; }
            //VTu CTY
            var txtVATTUPVC_117 = rp.ReportDefinition.ReportObjects["txtVATTUPVC_117"] as TextObject;
            if (txtVATTUPVC_117 != null)
                txtVATTUPVC_117.Text = string.Format("{0:0,0}", pvc_117).Replace(",", ".");

            // tong tien kh tra

            var d1 = daolap117 == null ? 0 : daolap117.THANHTIENCP;
            var d2 = daolap117vc == null ? 0 : daolap117vc.THANHTIENCP;

            var tongtienkhtra = pvc
                    + d1
                    + d2;
            var txtTongCong = rp.ReportDefinition.ReportObjects["txtTongCong"] as TextObject;

            if (tongtienkhtra != null)
            {
                if (txtTongCong != null)
                    txtTongCong.Text = string.Format("{0:0,0}", tongtienkhtra).Replace(",", ".");
            }



            // tong tien cong ty dau tu
            var txtTongCong_117 = rp.ReportDefinition.ReportObjects["txtTongCong_117"] as TextObject;
            if (txtTongCong_117 != null)
                txtTongCong_117.Text = string.Format("{0:0,0}", pvc_117).Replace(",", ".");

            var txtBangChu = rp.ReportDefinition.ReportObjects["txtBangChu"] as TextObject;
            if (txtBangChu != null)
                txtBangChu.Text = CommonUtil.DocSoTien(Convert.ToDecimal(tongtienkhtra));

            var kvnv = new NhanVienDao().Get(LoginInfo.MANV);
            var kv = new KhuVucDao().Get(kvnv.MAKV);
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + kv.TENKV.ToUpper();

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            TextObject lm = rp.ReportDefinition.ReportObjects["txtLAPMOI"] as TextObject;
            lm.Text = "LM";

            TextObject phuong1 = rp.ReportDefinition.ReportObjects["txtPHUONG"] as TextObject;
            phuong1.Text = tp;

            #endregion

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session[SessionKey.TK_BAOCAO_LAPCHIETTINH] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}