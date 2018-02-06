using System;
using System.Data;
using EOSCRM.Web.Common;
using System.Drawing;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class rpInHoaThuy : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();
                LayBaoCao();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void Page_Init(object sender, EventArgs e)
        {
       
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_INHOADON;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_GHICHISO;
            header.TitlePage = Resources.Message.PAGE_GCS_INHOADON;
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
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
                    
            var dt = (DataTable)Session["DSINHOADON"];

            //Update Customers DataTable with barcode image
            
         
            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/Inhoadon_phoiTNTHUY.rpt");
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/InHoaDon.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX;
            
            rpViewer.DataBind();

            Session["DSINHOADON"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            
        }
    }
}