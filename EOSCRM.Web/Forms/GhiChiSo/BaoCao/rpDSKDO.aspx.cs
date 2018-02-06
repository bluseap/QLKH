using System;
using System.Data;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class rpDSKDO : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                    LoadReferences();

                LayBaoCao();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_BAOCAO_DANHSACHKIEMTRA;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;
            header.ModuleName = Resources.Message.MODULE_GHICHISO;
            header.TitlePage = Resources.Message.PAGE_GCS_BAOCAO_DANHSACHKIEMTRA;
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadReferences()
        {
            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;            
        }      

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
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

            var dt = (DataTable)Session["DSKTKY"];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSKiemTraDo.rpt");
            rp.Load(path);

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
            {
                if (Session["DSKTKY_THANGNAM"] != null)
                    txtKy.Text = Session["DSKTKY_THANGNAM"].ToString();
            }


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session["DSKTKY"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}
