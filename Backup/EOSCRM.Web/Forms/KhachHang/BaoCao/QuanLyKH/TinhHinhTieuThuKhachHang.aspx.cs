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
    public partial class TinhHinhTieuThuKhachHang : Authentication
    {
        private readonly KhachHangDao khDao = new KhachHangDao();
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_BaoCao_QLKH_TinhHinhTieuThuKhachHang, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_QLKH_TINHHINHTIEUTHU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_QLKH_TINHHINHTIEUTHU;
            }
        }

        private void LoadReferences()
        {
            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
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

            var khachhang = khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());

            if (khachhang == null)
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
                return;
            }

            var dtDsinhoadon = new ReportClass().TinhHinhTieuThu(khachhang.IDKH).Tables[0];

            if (dtDsinhoadon == null || dtDsinhoadon.Rows.Count == 0)
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo");
                return;
            }

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/TTTTKH.rpt");
            rp.Load(path);


            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) 
                txtNguoiLap1.Text = txtNguoiLap.Text;

            rp.SetDataSource(dtDsinhoadon);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();
            Session["DSBAOCAO"] = dtDsinhoadon;
            divReport.Visible = true;

            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
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

            if (dt == null || dt.Rows.Count == 0)
                return;

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/TTTTKH.rpt");
            rp.Load(path);


            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();
            Session["DSBAOCAO"] = dt;
            divReport.Visible = true;

            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}