using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;
using EOSCRM.Domain;
using EOSCRM.Web.Common;

namespace EOSCRM.Web.Forms.ThietKe.Power.BaoCaoPo
{
    public partial class DSChuaHopDongPo : Authentication
    {
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
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
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.TK_BAOCAO_CHUAHOPDONGPO];
                    if(dt != null)
                        Report(dt);
                                        
                    var dt2 = (DataTable)Session[SessionKey.TK_BAOCAO_DAHOPDONGPO];
                    if(dt2 != null)
                        ReportDaLapHopDong(dt2);                    
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        //TK_BaoCaoDien_DSCHUAHDPo           
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_BAOCAOPO_DSCHUAHDPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_BAOCAOPO_DSCHUAHDPO;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            timkv();

            divReport.Visible = false;

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKVPO(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            var ds = new ReportClass().BienKHPoTuDenNgay(TuNgay, DenNgay, "","", cboKhuVuc.SelectedValue, "", "HD_N", "", "DSHDNODCTPO");

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
        {
            if (dt == null)
                return;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var nhanvien = _nvDao.Get(b);

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
                catch {  }
            }
            #endregion FreeMemory            

            rp = new ReportDocument();

            if (nhanvien.MAKV == "O")
            {
                var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKyPoCT.rpt");
                rp.Load(path);
            }
            else
            {
                var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKyPo.rpt");
                rp.Load(path);
            }

            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + _kvpoDao.Get(cboKhuVuc.SelectedValue).TENKV.ToUpper();

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH CHƯA LẬP HỢP ĐỒNG ĐIỆN";

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_CHUAHOPDONGPO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btDaLapHD_Click(object sender, EventArgs e)
        {
            try
            {
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                var ds = new ReportClass().BienKHPoTuDenNgay(TuNgay, DenNgay, "", "", cboKhuVuc.SelectedValue, "", "HD_A", "", "DSHDDALAPPO");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportDaLapHopDong(ds.Tables[0]);

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportDaLapHopDong(DataTable dt)
        {
            if (dt == null)
                return;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var nhanvien = _nvDao.Get(b);

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
                catch { }
            }
            #endregion FreeMemory
                        
            rp = new ReportDocument();

            if (nhanvien.MAKV == "O") // chau thanh
            {
                var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKyPoCT.rpt");
                rp.Load(path);
            }
            else
            {
                var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKyPo.rpt");
                rp.Load(path);
            }

            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + _kvpoDao.Get(cboKhuVuc.SelectedValue).TENKV.ToUpper();

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH ĐÃ LẬP HỢP ĐỒNG ĐIỆN";

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_DAHOPDONGPO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }


    }
}