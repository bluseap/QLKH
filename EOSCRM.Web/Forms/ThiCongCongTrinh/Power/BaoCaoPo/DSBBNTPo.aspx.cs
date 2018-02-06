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


namespace EOSCRM.Web.Forms.ThiCongCongTrinh.Power.BaoCaoPo
{
    public partial class DSBBNTPo : Authentication
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
                    var dt = (DataTable)Session[SessionKey.TC_BAOCAO_BBNTPO];
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
            Page.Title = Resources.Message.TITLE_TC_DSBBNTPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_DSBBNTPO;
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

            var ds = new ReportClass().DSQuiTrinhPoBien(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue, "", "", "DSBBNTPO");
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
                catch  { }
            }
            #endregion FreeMemory

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DSCDDangKy.rpt");
            var path = Server.MapPath("~/Reports/ThiCong/DSBBNghiemThuPo.rpt");
            rp.Load(path);

            /*string nhamay = "";
            if (ddlPHONGBAN.SelectedValue.Equals("%"))
            {
                nhamay = "";
            }
            else
            {
                var tonm = ddlPHONGBAN.Items.FindByValue(ddlPHONGBAN.SelectedValue);
                if (tonm != null)
                    nhamay = ". Tại: " + tonm.ToString();
            }*/

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();// +nhamay;

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH BIÊN BẢN NGHIỆM THU ĐIỆN";

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

            Session[SessionKey.TC_BAOCAO_BBNTPO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}