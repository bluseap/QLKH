using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class InSoGhiNuoc : Authentication
    {
        private ReportDocument rp = new ReportDocument();
        private NhanVienDao _nvDao = new NhanVienDao();
        private KhuVucDao _kvDao = new KhuVucDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    ClearForm();
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
            Page.Title = Resources.Message.TITLE_GCS_BAOCAO_INSOGHINUOC;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;
            header.ModuleName = Resources.Message.MODULE_GHICHISO;
            header.TitlePage = Resources.Message.PAGE_GCS_BAOCAO_INSOGHINUOC;
        }

        private void LoadReferences()
        {                  
            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.Items.Clear();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in listkhuvuc)
                cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            timkv();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
            //CloseWaitingDialog();
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

            var dt = new ReportClass().INSOGHINUOC(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue.ToString()).Tables[0];
            

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/INSOGHINUOC.rpt");
            rp.Load(path);

            /*var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
            {
                if (Session["DSKTKY_THANGNAM"] != null)
                    txtKy.Text = Session["DSKTKY_THANGNAM"].ToString();
            }*/

            
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            TextObject ky = rp.ReportDefinition.ReportObjects["txtKY"] as TextObject;
            ky.Text = "KỲ " + int.Parse(ddlTHANG.SelectedValue);
            TextObject ky1 = rp.ReportDefinition.ReportObjects["txtKY1"] as TextObject;
            ky1.Text = "KỲ " + (int.Parse(ddlTHANG.SelectedValue)+1);
            TextObject ky2 = rp.ReportDefinition.ReportObjects["txtKY2"] as TextObject;
            ky2.Text = "KỲ " + (int.Parse(ddlTHANG.SelectedValue) + 2);
            TextObject ky3 = rp.ReportDefinition.ReportObjects["txtKY3"] as TextObject;
            ky3.Text = "KỲ " + (int.Parse(ddlTHANG.SelectedValue) + 3);

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session["DSKTKY"] = dt;
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

            var dt = new ReportClass().INSOGHINUOC(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue.ToString()).Tables[0];


            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/INSOGHINUOC.rpt");
            rp.Load(path);

            /*var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
            {
                if (Session["DSKTKY_THANGNAM"] != null)
                    txtKy.Text = Session["DSKTKY_THANGNAM"].ToString();
            }*/


            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            TextObject ky = rp.ReportDefinition.ReportObjects["txtKY"] as TextObject;
            ky.Text = "KỲ " + int.Parse(ddlTHANG.SelectedValue);
            TextObject ky1 = rp.ReportDefinition.ReportObjects["txtKY1"] as TextObject;
            ky1.Text = "KỲ " + (int.Parse(ddlTHANG.SelectedValue) + 1);
            TextObject ky2 = rp.ReportDefinition.ReportObjects["txtKY2"] as TextObject;
            ky2.Text = "KỲ " + (int.Parse(ddlTHANG.SelectedValue) + 2);
            TextObject ky3 = rp.ReportDefinition.ReportObjects["txtKY3"] as TextObject;
            ky3.Text = "KỲ " + (int.Parse(ddlTHANG.SelectedValue) + 3);

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            Session["DSKTKY"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void ClearForm()
        {
            /*
             * clear phần thông tin hồ sơ
             */
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();

        }

    }
}
