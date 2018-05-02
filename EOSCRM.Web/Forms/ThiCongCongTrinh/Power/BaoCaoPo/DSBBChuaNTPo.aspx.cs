using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Domain;
using System.Web.UI;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh.Power.BaoCaoPo
{
    public partial class DSBBChuaNTPo : Authentication
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
                    listPhongBan();
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.TC_BAOCAO_BBCHUANTPO];
                    Report(dt);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        //TC_DSBBChuaNTPo            
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_BAOCAO_BBCHUANTNPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_BAOCAO_BBCHUANTNPO;
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
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                    }
                    if (a.MAPB == "TA")
                    {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ An Châu", "TA"));
                    }
                    if (a.MAPB == "TD")
                    {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                    }
                    if (a.MAPB == "KD")
                    {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
                        ddlPHONGBAN.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                        ddlPHONGBAN.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                        ddlPHONGBAN.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ An Châu", "TA"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                    }
                }
                else if (a.MAKV == "U" || a.MAKV == "S" || a.MAKV == "P" || a.MAKV == "T")
                {
                    ddlPHONGBAN.Items.Clear();
                    if (a.MAPB == "KD")
                    {
                        var kvList = _pbDao.GetListKV(a.MAKV);
                        ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
                        ddlPHONGBAN.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                        foreach (var pb in kvList)
                        {
                            ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }
                    }
                    else
                    {
                        var pbList = _pbDao.GetListPB(a.MAPB);
                        foreach (var pb in pbList)
                        {
                            ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }
                    }
                }
                else
                {
                    ddlPHONGBAN.Items.Clear();
                    if (a.MAPB == "KD")
                    {
                        var kvList = _pbDao.GetListKV(a.MAKV);
                        ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
                        ddlPHONGBAN.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                        foreach (var pb in kvList)
                        {
                            ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }
                    }
                    else
                    {
                        var pbList = _pbDao.GetListPB(a.MAPB);
                        foreach (var pb in pbList)
                        {
                            ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.Get(b);

            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            //var ds = new ReportClass().DSQuiTrinhPoBien(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue, "", "", "DSBBCHUANTPO");
            var ds = new ReportClass().DSQuiTrinhPoBien(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue, "", "", "DSBBCHUANTPOTO");

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
                catch  {   }
            }

            #endregion FreeMemory

            rp = new ReportDocument();

            var path = Server.MapPath("~/Reports/ThiCong/DSBBChuaNTPo.rpt");
            rp.Load(path);            

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();// +nhamay;

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH CHƯA NGHIỆM THU ĐIỆN";

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

            Session[SessionKey.TC_BAOCAO_BBCHUANTPO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}