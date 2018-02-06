using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class LichSuSuDungNuoc : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_BaoCao_QLKH_LichSuSuDungNuoc, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                }
                else 
                {
                    var dt = (DataTable)Session["DSBAOCAO_KH_LICHSUSUDUNGNUOC"];
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_QLKH_LICHSUSUDUNGNUOC;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_QLKH_LICHSUSUDUNGNUOC;
            }
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString();

            var kvList = _kvDao.GetList();

            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }
        #endregion

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var dt =
                new ReportClass().LichSuSuDungNuoc(int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), cboKhuVuc.SelectedValue).Tables[0];

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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/LichSuSuDungNuoc.rpt");
            rp.Load(path);

            /*
            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                txtNgay.Text = string.Format("Đồng Tháp, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));
            */

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DSBAOCAO_KH_LICHSUSUDUNGNUOC"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

    }
}