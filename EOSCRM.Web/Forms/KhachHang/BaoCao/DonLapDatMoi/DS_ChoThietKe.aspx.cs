using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Domain;


namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class DS_ChoThietKe : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        string tieudetk = "";

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
                }
                else
                {
                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "CHOTHIETKE")
                    {
                        var dt = (DataTable)Session[SessionKey.TK_BAOCAO_CHOTHIETKE];
                        Report(dt);
                    }

                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "TUCHOITK")
                    {
                        var dttc = (DataTable)Session[SessionKey.TK_BAOCAO_TUCHOITK];
                        ReportTCTK(dttc);
                    }

                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "CHOTK")
                    {
                        var dttc = (DataTable)Session[SessionKey.TK_BAOCAO_CHOTK];
                        ReportChoTK(dttc);
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DLM_CHOTHIETKE;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DLM_CHOTHIETKE;
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
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
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
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            if (_nvDao.Get(b).MAKV == "X")
            {
                //var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_P", ddlNHAMAYTO.SelectedValue, "", "DSNOBVTTKN");
                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_P", ddlNHAMAYTO.SelectedValue, "", "DSNOBVTTKNLX");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                Report(ds.Tables[0]);
            }
            else
            {
                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_P", ddlNHAMAYTO.SelectedValue, "", "DSNOBVTTKN");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                Report(ds.Tables[0]);
            }

            Session[SessionKey.TK_BAOCAO_COBIEN] = "CHOTHIETKE";

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

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
                catch  {   }
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
                txtTitle.Text = "DANH SÁCH CHƯA BỐC VẬT TƯ THIẾT KẾ";

            if (_nvDao.Get(b).MAKV == "X")
            {
                var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
                if (txtTENKHUVUC != null)
                    txtTENKHUVUC.Text = "XÍ NGHIỆP CẤP NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            }
            else
            {
                var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
                if (txtTENKHUVUC != null)
                    txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            }

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_CHOTHIETKE] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btBCTUCHOITK_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            if (_nvDao.Get(b).MAKV == "X")
            {
                //var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKN");
                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKNLX");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportTCTK(ds.Tables[0]);
            }
            else
            {
                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKN");
                //var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKNLX");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportTCTK(ds.Tables[0]);
            }

            Session[SessionKey.TK_BAOCAO_COBIEN] = "TUCHOITK";

            tieudetk = "TUCHOITK";

            CloseWaitingDialog();
        }

        private void ReportTCTK(DataTable dt)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

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
                catch   {  }
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

            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (tieudetk == "TUCHOITK")
            {                
                if (txtTitle != null)
                    txtTitle.Text = "DANH SÁCH TỪ CHỐI THIẾT KẾ";
            }
            else
            {
                if (tieudetk == "CHOTK")
                {                   
                    if (txtTitle != null)
                        txtTitle.Text = "DANH SÁCH CHỜ THIẾT KẾ";
                }
                else
                {                   
                    if (txtTitle != null)
                        txtTitle.Text = "DANH SÁCH TỪ CHỐI THIẾT KẾ.";
                }
            }

            if (_nvDao.Get(b).MAKV == "X")
            {
                var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
                if (txtTENKHUVUC != null)
                    txtTENKHUVUC.Text = "XÍ NGHIỆP CẤP NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            }
            else
            {
                var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
                if (txtTENKHUVUC != null)
                    txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            }

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_TUCHOITK] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btDSChoTK_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            if (_nvDao.Get(b).MAKV == "X")
            {
                //var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKN");
                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "DK_A", ddlNHAMAYTO.SelectedValue, "", "DSNHAPDONLX");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportChoTK(ds.Tables[0]);
            }
            else
            {
                var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "DK_A", ddlNHAMAYTO.SelectedValue, "", "DSNHAPDONLX");
                //var ds = new ReportClass().dskoBVTThietKe(TuNgay, DenNgay, cboKhuVuc.Text, "TK_RA", ddlNHAMAYTO.SelectedValue, "", "DSTCBVTTKNLX");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportChoTK(ds.Tables[0]);
            }

            Session[SessionKey.TK_BAOCAO_COBIEN] = "CHOTK";

            tieudetk = "CHOTK";

            CloseWaitingDialog();
        }

        private void ReportChoTK(DataTable dt)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

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

            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH CHỜ THIẾT KẾ";
            //if (tieudetk == "TUCHOITK")
            //{
            //    if (txtTitle != null)
            //        txtTitle.Text = "DANH SÁCH TỪ CHỐI THIẾT KẾ";
            //}
            //else
            //{
            //    if (tieudetk == "CHOTK")
            //    {
            //        if (txtTitle != null)
            //            txtTitle.Text = "DANH SÁCH CHỜ THIẾT KẾ";
            //    }
            //    else
            //    {
            //        if (txtTitle != null)
            //            txtTitle.Text = "DANH SÁCH TỪ CHỐI THIẾT KẾ.";
            //    }
            //}

            if (_nvDao.Get(b).MAKV == "X")
            {
                var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
                if (txtTENKHUVUC != null)
                    txtTENKHUVUC.Text = "XÍ NGHIỆP CẤP NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            }
            else
            {
                var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
                if (txtTENKHUVUC != null)
                    txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            }

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_CHOTK] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}