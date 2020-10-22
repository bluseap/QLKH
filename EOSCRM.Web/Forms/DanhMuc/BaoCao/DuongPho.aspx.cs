using System;
using System.Data;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Controllers.DanhMuc.BaoCao;
using FastMember;
using POWACO.Dapper.DanhMuc.BaoCao.DuongPho;

namespace EOSCRM.Web.Forms.DanhMuc.BaoCao
{
    public partial class DuongPho : Authentication
    {
        private readonly KhuVucDao kvDao = new KhuVucDao();

        private readonly DuongPhoService duongphoService = new DuongPhoService();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_BaoCao_DuongPho, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.DM_BAOCAO_DUONGPHO];
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
            Page.Title = Resources.Message.TITLE_DM_BAOCAO_DUONGPHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_BAOCAO_DUONGPHO;
            }
        }
               
        private void LoadReferences()
        {
            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.DataSource = listkhuvuc;
            cboKhuVuc.DataTextField = "TENKV";
            cboKhuVuc.DataValueField = "MAKV";
            cboKhuVuc.DataBind();

            divReport.Visible = false;
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {

            var ds = duongphoService.GetAll();
            if (ds == null || ds.Count == 0) { CloseWaitingDialog(); return; }

            DataTable table = new DataTable();
            using (var reader = ObjectReader.Create(ds))
            {
                table.Load(reader);
            }

            Report(table);

            //var ds = new ReportClass().DuongPho(cboKhuVuc.SelectedValue);
            //if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            //var danhsach = ds.Tables[0];
            //Report(ds.Tables[0]);

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
            rpViewer.Enabled = true;

            var path = Server.MapPath("../../../Reports/DanhMucHeThong/DUONGPHO.rpt");
            rp.Load(path);

            var kv = kvDao.Get(cboKhuVuc.SelectedValue);
            if (kv != null)
            {
                var txtTieuDe = rp.ReportDefinition.ReportObjects["txtKhuVuc"] as TextObject;
                if (txtTieuDe != null)
                    txtTieuDe.Text = "Khu vực: " + kv.TENKV;
            }

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;
            Session[SessionKey.DM_BAOCAO_DUONGPHO] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}
