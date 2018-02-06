using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Domain;

namespace EOSCRM.Web.Forms.ThietKe.Power.BaoCaoPo
{
    public partial class DSNoBVTTKPo : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();

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
                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "BVTCHUADUYETTKPO")
                    {
                        var dtbvt = (DataTable)Session[SessionKey.TK_BAOCAO_BVTCHUADUYETTKPO];
                        ReportBVTChuaDuyet(dtbvt);
                    }

                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "TUCHOITKPO")
                    {
                        var dttc = (DataTable)Session[SessionKey.TK_BAOCAO_TUCHOITKPO];
                        ReportTCTK(dttc);
                    }

                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "CHOTHICONG")
                    {
                        var dt = (DataTable)Session[SessionKey.TK_BAOCAO_CHOTHIETKE];
                        Report(dt);
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
            Page.Title = Resources.Message.TITLE_TK_BAOCAOPO_DSNOBVTTK;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_BAOCAOPO_DSNOBVTTKPO;
            }
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

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvpoDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    cboKhuVuc.Items.Clear();
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

        private void LoadReferences()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kvnv = _nvDao.Get(b);

                txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

                timkv();

                divReport.Visible = false;
                txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";

                var NhaMayList = _pbDao.GetListKV(kvnv.MAKV);
                ddlNHAMAYTO.Items.Clear();
                ddlNHAMAYTO.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in NhaMayList)
                {
                    ddlNHAMAYTO.Items.Add(new ListItem(kv.MAPB + " " + kv.TENPB, kv.MAPB));

                }
            }
            catch { }
        }       

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            //var ds = new ReportClass().RpdsChoThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_P");
            var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_P", ddlNHAMAYTO.SelectedValue, "", "DSNOBVTTKPO");

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

            Session[SessionKey.TK_BAOCAO_COBIEN] = "CHOTHICONG";

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
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
                catch    {     }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DS_ChoTKN.rpt");
            rp.Load(path);

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH CHƯA BỐC VẬT TƯ THIẾT KẾ ĐIỆN";

            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();     

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_CHOTHIETKE] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;          
        }

        protected void btBCTUCHOITK_Click(object sender, EventArgs e)
        {
            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());
                                  
            var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKPO");

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            ReportTCTK(ds.Tables[0]);

            Session[SessionKey.TK_BAOCAO_COBIEN] = "TUCHOITKPO";

            CloseWaitingDialog();
        }

        private void ReportTCTK(DataTable dt)
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
                catch { }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DS_ChoTKN.rpt");
            rp.Load(path);

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH TỪ CHỐI THIẾT KẾ ĐIỆN";

            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;
           
            Session[SessionKey.TK_BAOCAO_TUCHOITKPO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;           
        }

        protected void btBVTKoDuyet_Click(object sender, EventArgs e)
        {
            try
            {
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_P", ddlNHAMAYTO.SelectedValue, "", "DSBVTKODUYETTKPO");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportBVTChuaDuyet(ds.Tables[0]);

                Session[SessionKey.TK_BAOCAO_COBIEN] = "BVTCHUADUYETTKPO";

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportBVTChuaDuyet(DataTable dt)
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
                catch { }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DS_ChoTKN.rpt");
            rp.Load(path);

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH BỐC VẬT TƯ CHƯA DUYỆT THIẾT KẾ ĐIỆN";

            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_BVTCHUADUYETTKPO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;      
        }

    }
}