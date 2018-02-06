using System;
using System.Data;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using System.Globalization;
using System.IO;
using System.Web.UI;

using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;

namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class DSKDO : Authentication
    {        
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        #region Startup script registeration       
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.GCS_BAOCAO_KIEMDO3T];
                    ReportM(dt);
                }
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

        private void LoadStaticReferences()
        {          
            
            timkv();
            ClearForm();

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var dotin = _diDao.GetListKVNN(_nvDao.Get(b).MAKV);
            ddlDOTGCS.Items.Clear();
            ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var d in dotin)
            {
                ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
            }

            var duongpho = dpDao.GetListKV(_nvDao.Get(b).MAKV);
            ddlDuongPhoPo.Items.Clear();
            ddlDuongPhoPo.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var d in duongpho)
            {
                ddlDuongPhoPo.Items.Add(new ListItem(d.MADP + ": " + d.TENDP, d.MADP));
            }            
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
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void ClearForm()
        {            
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
        }        

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCaoM();
            CloseWaitingDialog();
        }

        private void LayBaoCaoM()
        {
            try
            {
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var dt = new ReportClass().KiemDo3T(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                            ddlKHUVUC.SelectedValue).Tables[0];
                    if (dt == null) { CloseWaitingDialog(); return; }                   
                    ReportM(dt);
                }
                else
                {
                    var dt = new ReportClass().KiemDo3TDotIn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,
                            ddlDOTGCS.SelectedValue, "", "KD3TDOTIN").Tables[0];
                    if (dt == null) { CloseWaitingDialog(); return; }                   
                    ReportM(dt);
                }                

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReportM(DataTable dt)
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
                catch { }
            }
            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("~/Reports/GhiChiSo/DSKiemTraDo.rpt");
            rp.Load(path);

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";            

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = "KỲ " + ddlTHANG.SelectedValue.ToString() + " NĂM " + txtNAM.Text.Trim() + tendot;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            upBAOCAO.Update(); 

            Session[SessionKey.GCS_BAOCAO_KIEMDO3T] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void btEXCEL_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dt;
                if (ddlDOTGCS.SelectedValue == "%" && ddlDuongPhoPo.SelectedValue == "%")
                {
                    dt = new ReportClass().KiemDo3T(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                            ddlKHUVUC.SelectedValue).Tables[0];
                }
                else if (ddlDOTGCS.SelectedValue == "%" && ddlDuongPhoPo.SelectedValue != "%")
                {
                    dt = new ReportClass().KiemDo3TDotIn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,
                            ddlDOTGCS.SelectedValue, ddlDuongPhoPo.SelectedValue, "KD3TDOTINMADP").Tables[0];
                }
                else if (ddlDOTGCS.SelectedValue != "%" && ddlDuongPhoPo.SelectedValue == "%")
                {
                    dt = new ReportClass().KiemDo3TDotIn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,
                            ddlDOTGCS.SelectedValue, "", "KD3TDOTIN").Tables[0];
                }
                else
                {
                    dt = new ReportClass().KiemDo3TDotIn(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC.SelectedValue,
                            ddlDOTGCS.SelectedValue, ddlDuongPhoPo.SelectedValue, "KD3TDOTINDP").Tables[0];
                }

                //dt = new ReportClass().KiemDo3T(int.Parse(ddlTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                //            ddlKHUVUC.SelectedValue).Tables[0];                  
                if (dt == null) { CloseWaitingDialog(); return; }                

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                CloseWaitingDialog();
                UpINFO.Update();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=KD3T" + ddlTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");               
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";

                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");                

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();               
            }
            catch { }
        }

    }
}