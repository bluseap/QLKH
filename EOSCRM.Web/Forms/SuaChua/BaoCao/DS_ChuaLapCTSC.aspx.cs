using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.SuaChua.BaoCao
{
    public partial class DS_ChuaLapCTSC : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    Session["DS_ChuaLapCTSC"] = null;
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
            Page.Title = Resources.Message.TITLE_SC_BAOCAO_DANHSACHDON;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_BAOCAO_DANHSACHDON;
            }
        }

        private void LoadReferences()
        {
            txtTuNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDenNgay.Text = DateTime.Now.ToString("dd/MM/yyyy");

            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in listkhuvuc)
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            divReport.Visible = false;
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
            ((EOS)Page.Master).CloseWaitingDialog();
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
                catch
                {

                }
            }

            #endregion FreeMemory

            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());
            string ttct = "CT_N";// =
            var dt = new ReportClass().RpDONSUACHUALAPCHIETTINH(TuNgay, DenNgay, cboKhuVuc.SelectedValue,ttct).Tables[0];
           
            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChua.rpt");
            rp.Load(path);

            SetTitle(rp);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DS_DonSuaChua"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
        void SetTitle(ReportDocument rp)
        {
            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            //TODO: sửa title cho báo cáo ở đây
            var txtTitle = rp.ReportDefinition.ReportObjects["txtTitle"] as TextObject;
            if (txtTitle != null)
                txtTitle.Text = "DANH SÁCH ĐƠN SỬA CHỮA CHƯA LẬP CHIẾT TÍNH";
            var txtNgayThangNam = rp.ReportDefinition.ReportObjects["txtNgayThangNam"] as TextObject;
            txtNgayThangNam.Text = string.Format("Đồng Tháp,Ngày {0}, tháng{1}, năm {2} ", DateTime.Now.Day.ToString(), DateTime.Now.Month.ToString(), DateTime.Now.Year.ToString());

        }
        private void ReLoadBaoCao()
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
                catch
                {

                }
            }

            #endregion FreeMemory

            var dt = Session["DS_ChuaLapCTSCa"] as DataTable;
            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChua.rpt");
            rp.Load(path);

            SetTitle(rp);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DS_ChuaLapCTSC"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}