using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class DSDongHoCSLon : Authentication
   {
        private ReportDocument rp = new ReportDocument();
        private NhanVienDao _nvDao = new NhanVienDao();
        private KhuVucDao _kvDao = new KhuVucDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Authenticate(Functions..DanhSachVatTu, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    LoadReferences();
                }
                else 
                {
                    ReLoadBaoCao();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DSDONGHOCSLON;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DSDONGHOCSLON;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.Day.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + DateTime.Now.Year.ToString();
            txtDenNgay.Text = DateTime.Now.Day.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" + DateTime.Now.Year.ToString();

            timkv();
            /*var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.DataSource = listkhuvuc;
            cboKhuVuc.DataTextField = "TENKV";
            cboKhuVuc.DataValueField = "MAKV";
            cboKhuVuc.DataBind();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            cboKhuVuc.Text = "%";*/

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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
        }
        private void LayBaoCao()
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
                catch
                {

                }
            }

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt = new ReportClass().DSDongHoCSLon(TuNgay, DenNgay, cboKhuVuc.Text, 25).Tables[0];             

            rp = new ReportDocument();
            //var path = Server.MapPath("../../Reports/DanhMucHeThong/DSDongHoCSLON.rpt");
            var path = Server.MapPath("~/Reports/DanhMucHeThong/DSDongHoCSLON.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            
            /*
            TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            txtXN.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();            
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " + m + " năm " + y;
            */

            TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            txtXN.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " + m + " năm " + y;

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void ReLoadBaoCao()
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
                catch
                {

                }
            }

            #endregion FreeMemory

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt = new ReportClass().DSDongHoCSLon(TuNgay, DenNgay, cboKhuVuc.Text, 25).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/DanhMucHeThong/DSDongHoCSLON.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            txtXN.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " + m + " năm " + y;

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}