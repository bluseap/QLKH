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


namespace EOSCRM.Web.Forms.ThiCongCongTrinh.BaoCao
{
    public partial class DSBBNT : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

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
                    //listPhongBan();                    
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.TC_BAOCAO_BBNTN];
                    Report(dt);

                    //var dtngayn = (DataTable)Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN];
                    //ReportNgayN(dtngayn);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_DSBBNTN;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_DSBBNTN;
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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.Get(b);

            
            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            var ds = new ReportClass().DSQuiTrinhNuocBien(TuNgay, DenNgay, cboKhuVuc.Text, ddlPHONGBAN.SelectedValue, "", "", "DSBBNTN");
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
                catch  {        }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../../Reports/DonLapDatMoi/DSCDDangKy.rpt");
            var path = Server.MapPath("~/Reports/ThiCong/DSBBNghiemThuN.rpt");
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
                txtTitle.Text = "DANH SÁCH BIÊN BẢN NGHIỆM THU NƯỚC";

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;
            //txtTENKHUVUC
            var txtTENKHUVUC = rp.ReportDefinition.ReportObjects["txtTENKHUVUC"] as TextObject;
            if (txtTENKHUVUC != null)
                txtTENKHUVUC.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();


            rp.SetDataSource(dt);
            //rpViewer.ReportSource = rp;
            //rpViewer.DataBind();

            divReport.Visible = true;

            Session[SessionKey.TC_BAOCAO_BBNTN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}