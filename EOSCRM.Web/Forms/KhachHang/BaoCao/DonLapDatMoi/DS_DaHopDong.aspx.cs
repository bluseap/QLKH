using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;


namespace EOSCRM.Web.Forms.KhachHang.BaoCao.DonLapDatMoi
{
    public partial class DS_DaHopDong : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

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
                    var dt = (DataTable)Session[SessionKey.TK_BAOCAO_DAHOPDONG];
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DLM_DAHOPDONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DLM_DAHOPDONG;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in listkhuvuc)
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            divReport.Visible = false;

            timkv();
            listPhongBan();

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
        }

        protected void listPhongBan()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "O")
                {
                    if (a.MAPB == "NB")
                    {
                        ddlTONHAMAY.Items.Clear();
                        ddlTONHAMAY.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                    }
                    if (a.MAPB == "TA")
                    {
                        ddlTONHAMAY.Items.Clear();
                        ddlTONHAMAY.Items.Add(new ListItem("Tổ An Châu", "TA"));
                    }
                    if (a.MAPB == "TD")
                    {
                        ddlTONHAMAY.Items.Clear();
                        ddlTONHAMAY.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                    }
                    if (a.MAPB == "KD")
                    {
                        ddlTONHAMAY.Items.Clear();
                        ddlTONHAMAY.Items.Add(new ListItem("Tất cả", "%"));
                        ddlTONHAMAY.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                        ddlTONHAMAY.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                        ddlTONHAMAY.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                        ddlTONHAMAY.Items.Add(new ListItem("Tổ An Châu", "TA"));
                        ddlTONHAMAY.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                    }
                }

                if (a.MAKV == "U")
                {
                    ddlTONHAMAY.Items.Clear();
                    if (a.MAPB == "KD")
                    {
                        var kvList = _pbDao.GetListKV(a.MAKV);
                        ddlTONHAMAY.Items.Add(new ListItem("Tất cả", "%"));
                        ddlTONHAMAY.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                        foreach (var pb in kvList)
                        {
                            ddlTONHAMAY.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }
                    }
                    else
                    {
                        var pbList = _pbDao.GetListPB(a.MAPB);
                        foreach (var pb in pbList)
                        {
                            ddlTONHAMAY.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }
                    }
                }
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
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
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

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            //var ds = new ReportClass().RpdsDaHopDong(TuNgay, DenNgay, cboKhuVuc.Text, "HD_A");
            var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text, ddlTONHAMAY.SelectedValue, "dsHDCHUAKT", TuNgay, DenNgay);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSHopDong.rpt");
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
                txtTitle.Text = "DANH SÁCH ĐÃ HỢP ĐỒNG";

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_DAHOPDONG] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}