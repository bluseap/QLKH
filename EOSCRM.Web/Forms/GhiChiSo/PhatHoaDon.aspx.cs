using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class PhatHoaDon : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();

        protected List<TINHCUOC> DataSource
        {
            get { return Session["TINHCUOC_DATASOURCE"] as List<TINHCUOC>; }
            set { Session["TINHCUOC_DATASOURCE"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_PhatHoaDon, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
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
            Page.Title = Resources.Message.TITLE_GCS_PHATHOADON;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_PHATHOADON;
            }
        }

        private void LoadStaticReferences()
        {
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;

            // load khu vuc
            var listKhuVuc = kvDao.GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));

            foreach(var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            LoadDynamicReferences();
        }

        private void LoadDynamicReferences()
        {
            var nvList = dpDao.GetListNhanVienGT(ddlKHUVUC.SelectedValue, DateTime.Now.Year, ddlTHANG.SelectedIndex + 1);

            ddlNHANVIEN.Items.Clear();
            ddlNHANVIEN.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var nv in nvList)
            {
                ddlNHANVIEN.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
            }
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnReport_Click(object sender, EventArgs e)
        {
            LayBaoCao();
            CloseWaitingDialog();
        }

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDynamicReferences();
            CloseWaitingDialog();
        }

        private void LayBaoCao()
        {
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

            var dt =
                new ReportClass().BangKeGiaoNhanHoaDon(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                                              ddlNHANVIEN.Text.Trim(), ddlKHUVUC.Text.Trim()).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../Reports/QuanLyGhiDHTinhCuocInHD/BangKeGNHoaDon.rpt");
            rp.Load(path);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) 
                txtNguoiLap1.Text = txtNguoiLap.Text;
            /*
             * TODO: check lai BangKeGNHoaDon.rpt
             * 
            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = "Thành phố Cao Lãnh, ngày " + DateTime.Now.ToString("dd/MM/yyyy");
            */

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

// ReSharper disable UnusedPrivateMember
        private void ReLoadBaoCao()
// ReSharper restore UnusedPrivateMember
        {
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

            var dt = (DataTable)Session["DSBAOCAO"];
            rp = new ReportDocument();
            var path = Server.MapPath("../../Reports/QuanLyGhiDHTinhCuocInHD/BangKeGNHoaDon.rpt");
            rp.Load(path);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) txtNguoiLap1.Text = txtNguoiLap.Text;

            /*
             * TODO: check lai BangKeGNHoaDon.rpt
             * 
            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = "Thành phố Cao Lãnh, ngày " + DateTime.Now.ToString("dd/MM/yyyy");
            */

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}
