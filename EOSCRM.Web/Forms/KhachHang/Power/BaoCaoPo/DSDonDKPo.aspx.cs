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

using System.IO;
using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo
{
    public partial class DSDonDKPo : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
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
                    //LoadPrint();
                }
                else
                {       
                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "DONDANGKYNGAYN")
                    {
                        var dtngayn = (DataTable)Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN];
                        ReportNgayN(dtngayn);
                    }
                    else
                    {
                        var dt = (DataTable)Session[SessionKey.TK_BAOCAO_DONDANGKY];
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAOPO_DSDLMPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAOPO_DSDLMPO;
            }
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

                if (a.MAKV == "U" || a.MAKV == "S" || a.MAKV == "P" || a.MAKV == "T")
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

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            /*var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach(var kv in listkhuvuc)
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));*/
            timkv();

            divReport.Visible = false;

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
        }              

        private void ReportNgayN(DataTable dt)
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
            //var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DSCDDangKyNgayN.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKyNgayNPo.rpt");
            //DD
            rp.Load(path);

            string nhamay = "";
            if (ddlPHONGBAN.SelectedValue.Equals("%"))
            {
                nhamay = "";
            }
            else
            {
                var tonm = ddlPHONGBAN.Items.FindByValue(ddlPHONGBAN.SelectedValue);
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
                txtTitle.Text = "DANH SÁCH ĐƠN ĐĂNG KÝ ĐIỆN";

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;
            //txtTENKHUVUC
            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
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
            var path = Server.MapPath("~/Reports/DonLapDatMoi/DSCDDangKy.rpt");
            rp.Load(path);

            string nhamay = "";
            if (ddlPHONGBAN.SelectedValue.Equals("%"))
            {
                nhamay = "";
            }
            else
            {
                var tonm = ddlPHONGBAN.Items.FindByValue(ddlPHONGBAN.SelectedValue);
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
                txtTitle.Text = "DANH SÁCH ĐƠN ĐĂNG KÝ";

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;
            //txtTENKHUVUC
            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TK_BAOCAO_DONDANGKY] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.Get(b);

            //if (query.MAKV != "O")
            //{
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                var ds = new ReportClass().RpdscdDangKyPBNgayNPo(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue);
                
                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayN(ds.Tables[0]);

                Session[SessionKey.TK_BAOCAO_COBIEN] = "DONDANGKYNGAYN";

                CloseWaitingDialog();
            //}
            /*else// chau thanh lay ngay dang ky tren don
            {
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                //var ds = new ReportClass().RpdscdDangKy(TuNgay, DenNgay, cboKhuVuc.Text);
                var ds = new ReportClass().RpdscdDangKyPB(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue);//RpdscdDangKyPB
                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                Report(ds.Tables[0]);

                CloseWaitingDialog();
            }*/
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);
               
                var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
                var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

                var ds = new ReportClass().DSQuiTrinhPoBien(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue,"","",
                        "DSDLMPOCDNGAYN");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                //ReportNgayN(ds.Tables[0]);

                DataTable dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DLMD" + TuNgay.Month.ToString() + TuNgay.Year.ToString().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
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