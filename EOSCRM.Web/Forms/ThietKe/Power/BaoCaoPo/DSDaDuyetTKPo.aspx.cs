using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe.Power.BaoCaoPo
{
    public partial class DSDaDuyetTKPo : Authentication
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
                    var dt = (DataTable)Session[SessionKey.TK_BAOCAO_DATHIETKE];
                    Report(dt);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_DSDADUYETTKPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_TK_DSDADUYETTKPO;
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
                    var kvList = _kvpoDao.GetListPo(d);
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
                    ddlNHAMAYTO.Items.Add(new ListItem(kv.TENPB, kv.MAPB));
                }
            }
            catch { }
        }        

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.Get(b);

            if (query.MAKV != "O")
            {
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());
                               
                //var ds = new ReportClass().DSDaThietKePo(TuNgay, DenNgay, cboKhuVuc.Text, "%");//LAY NGAY NHAP VAO MAY
                var ds = new ReportClass().DSQuiTrinhPoBien(TuNgay, DenNgay, cboKhuVuc.Text, ddlNHAMAYTO.SelectedValue,
                            "", "", "DSDUYETTKPBPO");//LAY NGAY NHAP VAO MAY

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                Report(ds.Tables[0]);

                CloseWaitingDialog();
            }
            else
            {
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                //var ds = new ReportClass().RpdsDaLapCt(TuNgay, DenNgay, cboKhuVuc.Text, "CT_A");
                var ds = new ReportClass().DSDaThietKePo(TuNgay, DenNgay, cboKhuVuc.Text, "%");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                Report(ds.Tables[0]);

                CloseWaitingDialog();
            }
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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DSCDDangKy.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKyNgayN.rpt");

            rp.Load(path);


            string nhamay = "";
            if (ddlNHAMAYTO.SelectedValue.Equals("%"))
            {
                nhamay = "";
            }
            else
            {
                var tonm = ddlNHAMAYTO.SelectedItem.ToString();
                if (tonm != null)
                    nhamay = ". Tại: " + tonm.ToString();
            }

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim() + nhamay;

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH ĐƠN ĐĂNG KÝ ĐÃ DUYỆT THIẾT KẾ ĐIỆN";

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).TENKV;
            //txtTENKHUVUC
            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_DATHIETKE] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}