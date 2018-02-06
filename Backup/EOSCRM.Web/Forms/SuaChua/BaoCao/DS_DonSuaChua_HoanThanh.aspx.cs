using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.SuaChua.BaoCao
{
    public partial class DS_DonSuaChua_HoanThanh : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    Session["DS_DonSuaChua"] = null;
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
            Page.Title = Resources.Message.TITLE_SC_BAOCAO_DANHSACHDON_HOANTHANH;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_BAOCAO_DANHSACHDON_HOANTHANH;
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

            var nvListGhiThu = new NhanVienDao () .GetListByCV(MACV.GT.ToString());
            cboThuNgan.Items.Clear();
            cboThuNgan.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var tn in nvListGhiThu)
                cboThuNgan.Items.Add(new ListItem(tn.HOTEN, tn.MANV));

            var nvListNvXuLy = new NhanVienDao().GetList();
            cboNhanVienXuLy.Items.Clear();
            cboNhanVienXuLy.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var tn in nvListNvXuLy)
                cboNhanVienXuLy.Items.Add(new ListItem(tn.HOTEN, tn.MANV));

            var nvListXuLy = new ThongTinXuLyDao().GetList();
            cboLoaiXuLy.Items.Clear();
            cboLoaiXuLy.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var xl in nvListXuLy)
                cboLoaiXuLy.Items.Add(new ListItem(xl.TENXL, xl.MAXL));

            divReport.Visible = false;
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
                catch
                {

                }
            }

            #endregion FreeMemory

            var TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            var DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            var dt = new ReportClass().RpdsDonSuaChua(TuNgay, DenNgay, cboNhanVienXuLy.Text.Trim(),
                cboThuNgan.SelectedValue, cboLoaiXuLy.SelectedValue, cboKhuVuc.SelectedValue).Tables[0];
            
            rp = new ReportDocument();
            var path = "";
            if(cboNhomTheo.Text .Trim() == "1")
                path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChuaNVXL.rpt");
            else if (cboNhomTheo.Text.Trim() == "2")
                path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChuaTHUNGAN.rpt");
            else if (cboNhomTheo.Text.Trim() == "3")
                path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChuaLOAIXL.rpt");
            rp.Load(path);

            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) 
                txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DS_DonSuaChua"] = dt;
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
                catch
                {

                }
            }

            #endregion FreeMemory

            var dt = (DataTable)Session["DS_DonSuaChua"];
            rp = new ReportDocument();

            var path = "";
            if (cboNhomTheo.Text.Trim() == "1")
                path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChuaNVXL.rpt");
            else if (cboNhomTheo.Text.Trim() == "2")
                path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChuaTHUNGAN.rpt");
            else if (cboNhomTheo.Text.Trim() == "3")
                path = Server.MapPath("../../../Reports/SuaChua/DS_DonSuaChuaLOAIXL.rpt");
            rp.Load(path);
             
            var txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            if (txtTuNgay1 != null)
                txtTuNgay1.Text = "Từ ngày " + txtTuNgay.Text.Trim() + " đến ngày " + txtDenNgay.Text.Trim();
     
            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null) txtNguoiLap1.Text = txtNguoiLap.Text;


            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divReport.Visible = true;

            Session["DS_DonSuaChua"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }
    }
}