using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.UI;

using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Common;

namespace EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo
{
    public partial class DSThayDoiCTPo : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();

        int day = DateTime.Now.Day;
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

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
                    if (lbRELOAD.Text == "1")
                    { ReLoadTDCTN(); }
                    if (lbRELOAD.Text == "2")
                    { ReLoadTDCTMDKPO(); }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_THAYDOICHITIETNUOC;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_KHACHHANG;
            header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_THAYDOICHITIETNUOC;

            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;           

            txtNAM.Text = DateTime.Now.Year.ToString();

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            timkv();

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var kvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

            if (kvpo == "J")
            {
                var dotin = _diDao.GetListKVDDP7(kvpo);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }
            }
            else
            {
                //var dotin = _diDao.GetListKVDDNotP7(kvpo);
                var dotin = _diDao.GetListKVDDP7(kvpo);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }
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
                    var kvList = _kvpoDao.GetList();
                    cboKhuVuc.Items.Clear();
                    cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            #region FreeMemory
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch  {    }
            }

            #endregion FreeMemory

            DataTable dt;

            if (ddlDOTGCS.SelectedValue == "%")
            {
                dt = new ReportClass().DSTHAYDOICHITIETDIEN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                            cboKhuVuc.SelectedValue, "DSTDCTPO").Tables[0];
            }
            else
            {
                dt = new ReportClass().DSTHAYDOICTPODOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                            cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTPODOTIN").Tables[0];
            }

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSTHAYDOICTNUOC.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH THAY ĐỔI CHI TIẾT ĐIỆN";

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + cboTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            lbRELOAD.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void ReLoadTDCTN()
        {
            #region FreeMemory
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
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

            //DataTable dt = new ReportClass().DSTHAYDOICHITIETDIEN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
            //    cboKhuVuc.SelectedValue, "DSTDCTPO").Tables[0];

            DataTable dt;
            if (ddlDOTGCS.SelectedValue == "%")
            {
                dt = new ReportClass().DSTHAYDOICHITIETDIEN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                            cboKhuVuc.SelectedValue, "DSTDCTPO").Tables[0];
            }
            else
            {
                dt = new ReportClass().DSTHAYDOICTPODOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                            cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTPODOTIN").Tables[0];
            }

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSTHAYDOICTNUOC.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH THAY ĐỔI CHI TIẾT ĐIỆN";

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + cboTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            //lbRELOAD.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();

        }

        protected void btBCKHMUCDK_Click(object sender, EventArgs e)
        {
            #region FreeMemory
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
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

            DataTable dt = new ReportClass().DSTHAYDOICTMUCDKPO(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                cboKhuVuc.SelectedValue, "DSTDCTMDKPO").Tables[0];

            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSTHAYDOICTNUOC.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH THAY ĐỔI CHI TIẾT ĐIỆN";

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + cboTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim();
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            lbRELOAD.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReLoadTDCTMDKPO()
        {
            #region FreeMemory
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
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


            DataTable dt = new ReportClass().DSTHAYDOICTMUCDKPO(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                cboKhuVuc.SelectedValue, "DSTDCTMDKPO").Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSTHAYDOICTNUOC.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH THAY ĐỔI CHI TIẾT ĐIỆN";

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + cboTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim();
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;

        }

        protected void btXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kv = _nvDao.Get(b);
               
                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

                DataTable dt;
                //if (_diDao.Get(ddlDOTGCS.SelectedValue) != null || _diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN != "DDP7D1")
                //{
                    if (ddlDOTGCS.SelectedValue == "%")
                    {
                        var ds = new ReportClass().DSTHAYDOICHITIETDIEN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                    cboKhuVuc.SelectedValue, "DSTDCTPO");
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        if (ddlDOTGCS.SelectedValue == "DDP7D1")
                        {
                            var ds = new ReportClass().DSTHAYDOICTPOP7D1(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTPOP7D1");
                            dt = ds.Tables[0];   
                        }
                        else
                        {
                            var ds = new ReportClass().DSTHAYDOICTPODOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                        cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTPODOTIN");
                            dt = ds.Tables[0];
                        }
                    }                                 
                //}
                //else
                //{
                    
                //}

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDCT" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);

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

                CloseWaitingDialog();           
                
            }
            catch { }
        }

    }
}