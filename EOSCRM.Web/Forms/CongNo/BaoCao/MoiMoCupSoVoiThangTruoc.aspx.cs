using System;
using System.Data;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.CongNo.BaoCao
{
    public partial class MoiMoCupSoVoiThangTruoc : Authentication
    {
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
            Page.Title = Resources.Message.TITLE_CN_BAOCAO_KHACHHANGMOIMOCUP;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_CONGNO;
            header.TitlePage = Resources.Message.PAGE_CN_BAOCAO_KHACHHANGMOIMOCUP;
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString();
            txtNguoiLap.Text = LoginInfo.NHANVIEN.HOTEN;
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

      
        private void LayBaoCaoDSKHMoi()
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
                new ReportClass().DanhSachKhachHangMoiSoVoiThangTruoc(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim())).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/DSKhachHangSoKyTruoc.rpt");
            rp.Load(path);

            //var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            //if (txtKy != null) 
            //    txtKy.Text = cboTHANG.Text + "/" + txtNAM.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) 
                txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
        private void LayBaoCaoDSKHMo()
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
                new ReportClass().DanhSachKhachHangMoSoVoiThangTruoc(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim())).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/DSKhachHangSoKyTruoc.rpt");
            rp.Load(path);

            //var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            //if (txtKy != null)
            //    txtKy.Text = cboTHANG.Text + "/" + txtNAM.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
        private void LayBaoCaoDSKHCup()
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
                new ReportClass().DanhSachKhachHangCupSoVoiThangTruoc(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim())).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/DSKhachHangSoKyTruoc.rpt");
            rp.Load(path);

            //var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            //if (txtKy != null)
            //    txtKy.Text = cboTHANG.Text + "/" + txtNAM.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
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
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory


            var dt = (DataTable)Session["DSBAOCAO"];
            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/CongNo/DSKhachHangSoKyTruoc.rpt");
            rp.Load(path);

            //var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            //if (txtKy != null) txtKy.Text = cboTHANG.Text + "/" + txtNAM.Text .Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btnDSKHMoi_Click(object sender, EventArgs e)
        {
            LayBaoCaoDSKHMoi();
            CloseWaitingDialog();
        }

        protected void btnDSKHMo_Click(object sender, EventArgs e)
        {
            LayBaoCaoDSKHMo();
            CloseWaitingDialog();
        }

        protected void btnDSKHCup_Click(object sender, EventArgs e)
        {
            LayBaoCaoDSKHCup();
            CloseWaitingDialog();
        }
    }
}