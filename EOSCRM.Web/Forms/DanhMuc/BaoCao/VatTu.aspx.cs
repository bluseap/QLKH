using System;
using System.Data;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.DanhMuc.BaoCao
{
    public partial class VatTu : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_BaoCao_VatTu, Permission.Read);
                PrepareUI();

                if (Page.IsPostBack)
                {
                    var dt = (DataTable)Session[SessionKey.DM_BAOCAO_VATTU];
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
            Page.Title = Resources.Message.TITLE_DM_BAOCAO_VATTU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_BAOCAO_VATTU;
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
            var ds = new ReportClass().Rpvattu();

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
            
            var path = Server.MapPath("../../../Reports/DanhMucHeThong/DSVatTu.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;
            Session[SessionKey.DM_BAOCAO_VATTU] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}
