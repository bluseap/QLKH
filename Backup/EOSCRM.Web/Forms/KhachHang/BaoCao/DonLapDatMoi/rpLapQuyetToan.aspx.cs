using System;
using System.Data;
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
    public partial class rpLapQuyetToan : Authentication
    {
        private ReportDocument rp = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_LapQuyetToan, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session[SessionKey.TK_BAOCAO_LAPQUYETTOAN];
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DLM_BANGQUYETTOAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DLM_BANGQUYETTOAN;
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
            var MADDK = (string)Session["LAPQUYETTOAN_MADDK"];
            var quyettoan = new QuyetToanDao().Get(MADDK);

            if (quyettoan == null)
            {
                Page.Response.Redirect(ResolveUrl("~") + "Login.aspx", false);
                return;
            }

            var ds = new ReportClass().BaoCaoLapQuyetToan(MADDK);

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
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/LapQuyetToan.rpt");
            rp.Load(path);

            #region Text box    txtVATTUPVC, txtVATTUPVCVAT, txtVATTUSTK, txtVATTUSTKVAT, txtNHANCONG, txtNHANCONGVAT, txtTongCong, txtBangChu

            var txtKhachHang = rp.ReportDefinition.ReportObjects["txtKhachHang"] as TextObject;
            if (txtKhachHang != null)
                txtKhachHang.Text = quyettoan.TENCT;

            var txtDiaChi = rp.ReportDefinition.ReportObjects["txtDiaChi"] as TextObject;
            if (txtDiaChi != null)
                txtDiaChi.Text = quyettoan.DONDANGKY.DIACHILD;

            decimal pvc = 0;
            decimal thuepvc = 0;
            decimal stk = 0;
            decimal thuestk = 0;
            decimal nc = 0;
            decimal thuenc = 0;
            decimal tongvattu = 0;

            decimal pvc_117 = 0;
            decimal stk_117 = 0;
            decimal nc_117 = 0;
            decimal tongvattu_117 = 0;

            foreach (DataRow row in dt.Rows)
            {
                if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() == "PVC")
                    pvc = pvc + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "2" && row["MANHOM"].ToString() == "STK")
                    stk = stk + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "2")
                    nc = nc + (decimal)row["TIENNC"];

                if (row["THUTU"].ToString() == "1" && row["MANHOM"].ToString() == "PVC")
                    pvc_117 = pvc_117 + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "1" && row["MANHOM"].ToString() == "STK")
                    stk_117 = stk_117 + (decimal)row["TIENVT"];

                if (row["THUTU"].ToString() == "1")
                    nc_117 = nc_117 + (decimal)row["TIENNC"];
            }

            thuepvc = Math.Round(pvc * 10 / 100, 0);
            thuestk = Math.Round(stk * 10 / 100, 0);
            thuenc = Math.Round(nc * 10 / 100, 0);
            tongvattu = pvc + thuepvc + stk + thuestk + nc + thuenc;

            tongvattu_117 = pvc_117 + stk_117 + nc_117;

            var txtVATTUPVC = rp.ReportDefinition.ReportObjects["txtVATTUPVC"] as TextObject;
            if (txtVATTUPVC != null)
                txtVATTUPVC.Text = string.Format("{0:0,0}", pvc).Replace(",", ".");

            var txtVATTUPVCVAT = rp.ReportDefinition.ReportObjects["txtVATTUPVCVAT"] as TextObject;
            if (txtVATTUPVCVAT != null)
                txtVATTUPVCVAT.Text = string.Format("{0:0,0}", thuepvc).Replace(",", ".");

            var txtVATTUSTK = rp.ReportDefinition.ReportObjects["txtVATTUSTK"] as TextObject;
            if (txtVATTUSTK != null)
                txtVATTUSTK.Text = string.Format("{0:0,0}", stk).Replace(",", ".");

            var txtVATTUSTKVAT = rp.ReportDefinition.ReportObjects["txtVATTUSTKVAT"] as TextObject;
            if (txtVATTUSTKVAT != null)
                txtVATTUSTKVAT.Text = string.Format("{0:0,0}", thuestk).Replace(",", ".");

            var txtNHANCONG = rp.ReportDefinition.ReportObjects["txtNHANCONG"] as TextObject;
            if (txtNHANCONG != null)
                txtNHANCONG.Text = string.Format("{0:0,0}", nc).Replace(",", ".");

            var txtNHANCONGVAT = rp.ReportDefinition.ReportObjects["txtNHANCONGVAT"] as TextObject;
            if (txtNHANCONGVAT != null)
                txtNHANCONGVAT.Text = string.Format("{0:0,0}", thuenc).Replace(",", ".");

            var txtTongCong = rp.ReportDefinition.ReportObjects["txtTongCong"] as TextObject;
            if (txtTongCong != null)
                txtTongCong.Text = string.Format("{0:0,0}", tongvattu).Replace(",", ".");


            var txtVATTUPVC_117 = rp.ReportDefinition.ReportObjects["txtVATTUPVC_117"] as TextObject;
            if (txtVATTUPVC_117 != null)
                txtVATTUPVC_117.Text = string.Format("{0:0,0}", pvc_117).Replace(",", ".");

            var txtVATTUSTK_117 = rp.ReportDefinition.ReportObjects["txtVATTUSTK_117"] as TextObject;
            if (txtVATTUSTK_117 != null)
                txtVATTUSTK_117.Text = string.Format("{0:0,0}", stk_117).Replace(",", ".");

            var txtNHANCONG_117 = rp.ReportDefinition.ReportObjects["txtNHANCONG_117"] as TextObject;
            if (txtNHANCONG_117 != null)
                txtNHANCONG_117.Text = string.Format("{0:0,0}", nc_117).Replace(",", ".");

            var txtTongCong_117 = rp.ReportDefinition.ReportObjects["txtTongCong_117"] as TextObject;
            if (txtTongCong_117 != null)
                txtTongCong_117.Text = string.Format("{0:0,0}", tongvattu_117).Replace(",", ".");


            var txtBangChu = rp.ReportDefinition.ReportObjects["txtBangChu"] as TextObject;
            if (txtBangChu != null)
                txtBangChu.Text = CommonUtil.DocSoTien(tongvattu);

            var txtLapBieu = rp.ReportDefinition.ReportObjects["txtLapBieu"] as TextObject;
            if (txtLapBieu != null)
                txtLapBieu.Text = txtNguoiLap.Text.Trim();

            var txtGiamDoc1 = rp.ReportDefinition.ReportObjects["txtGiamDoc"] as TextObject;
            if (txtGiamDoc1 != null)
                txtGiamDoc1.Text = txtGiamDoc.Text.Trim();

            #endregion

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session[SessionKey.TK_BAOCAO_LAPQUYETTOAN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThiCongCongTrinh/LapQuyetToan.aspx?" + Constants.PARAM_REPORTED + "=true", false);
        }
    }
}