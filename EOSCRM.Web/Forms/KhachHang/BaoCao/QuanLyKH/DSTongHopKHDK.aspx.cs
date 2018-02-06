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

using System.IO;
using System.Web.UI;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class DSTongHopKHDK : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        string ngayF = DateTime.Now.Day.ToString();
        string thangF = DateTime.Now.Month.ToString();
        string namF = DateTime.Now.Year.ToString();

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
                    var dt = (DataTable)Session[SessionKey.KH_BAOCAO_TONGHOPDK];                    
                    Report(dt);                   
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        // KH_BC_DSTONGHOPKHDK                
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_DSTONGHOPKHDK;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_DSTONGHOPKHDK;
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

                ddlDieuKien.Items.Clear();
                ddlDieuKien.Items.Add(new ListItem("Tất cả", "%"));
                ddlDieuKien.Items.Add(new ListItem("DS 3 tháng chưa nhập thi công", "DS3TKOTC"));
                ddlDieuKien.Items.Add(new ListItem("DS 1 tháng nhập đơn chưa thiết kế", "DS1TKOTK"));
                //ddlDieuKien.Items.Add(new ListItem("DS 6 tháng chưa nhập thi công", "DS6TKOTC"));
                
            }
            catch { }
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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
            CloseWaitingDialog(); 
        }

        private void LayBaoCao()
        {
            //var dt = new ReportClass().DSQuiTrinhNuocBien(DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()),
            //            DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()), cboKhuVuc.SelectedValue, ddlNHAMAYTO.SelectedValue,
            //            "", "", "DSTONGHOPDK").Tables[0];

            DateTime tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            var dt = new ReportClass().DSQuiTrinhNuocBien(tungay, denngay, cboKhuVuc.SelectedValue, ddlNHAMAYTO.SelectedValue,
                        "", "", "DSTONGHOPDK").Tables[0];

            Report(dt);

            CloseWaitingDialog();   
        }

        private void Report(DataTable dt)
        {
            if (dt == null)
                return;

            var rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }              
                catch       {    }
            }

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSTongHopDK.rpt");
            rp.Load(path);

            var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            if (txtXN != null)
                txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();

            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH TỔNG HỢP ĐƠN ĐĂNG KÝ KHÁCH HÀNG";            

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày: " + DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()).ToString("dd/MM/yyyy") +
                        " đến ngày " + DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()).ToString("dd/MM/yyyy");

            var txtNGAY = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            if (txtNGAY != null)
                txtNGAY.Text = "An Giang, ngày " + ngayF + " tháng " + thangF + " năm " + namF;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.KH_BAOCAO_TONGHOPDK] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            
        }

        protected void btExcelDSKH_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                DateTime tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                DateTime denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    var ds = new ReportClass().DSTongHopDDK(tungay, denngay, cboKhuVuc.SelectedValue, ddlNHAMAYTO.SelectedValue,
                        "", "", "DSTHDDKN");
                    dt = ds.Tables[0];

                }
                else
                {
                    var ds = new ReportClass().DSTongHopDDK(tungay, denngay, cboKhuVuc.SelectedValue, ddlNHAMAYTO.SelectedValue,
                        "", "", "DSTHDDKN"); //LAY NGAY NHAP TREN MAY
                    dt = ds.Tables[0];
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=THDDK" + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //Response.Write(style);
                //string style = @"<style> TD { mso-number-format:\@; } </style>";
                Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                
            }
            catch { }
        }

        protected void btDSDieuKien_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                DateTime tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                DateTime denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                DataTable dt;

                if (ddlDieuKien.SelectedValue != "%")
                {
                    if (ddlDieuKien.SelectedValue == "DS3TKOTC")
                    {
                        var ds = new ReportClass().DSTongHopDDK(tungay, denngay, cboKhuVuc.SelectedValue, ddlNHAMAYTO.SelectedValue,
                            "", ddlDieuKien.SelectedValue, "DS3TKOTC"); //LAY NGAY NHAP TREN MAY
                        dt = ds.Tables[0];
                    }
                    else if (ddlDieuKien.SelectedValue == "DS1TKOTK")
                    {
                        var ds = new ReportClass().DSTongHopDDK(tungay, denngay, cboKhuVuc.SelectedValue, ddlNHAMAYTO.SelectedValue,
                            "", ddlDieuKien.SelectedValue, "DS1TKOTK"); //LAY NGAY NHAP TREN MAY
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        CloseWaitingDialog();
                        return;
                    }
                }
                else
                {
                    CloseWaitingDialog();                    
                    return;
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=THDK" + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //Response.Write(style);
                //string style = @"<style> TD { mso-number-format:\@; } </style>";
                Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
            }
            catch { }
        }


    }
}