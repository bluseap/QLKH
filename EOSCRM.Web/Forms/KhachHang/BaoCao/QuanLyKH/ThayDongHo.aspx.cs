using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Domain;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;
using System.IO;
using System.Web.UI;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class ThayDongHo : Authentication        
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private ReportDocument rp = new ReportDocument();
        private NhanVienDao _nvDao = new NhanVienDao();
        private KhuVucDao _kvDao = new KhuVucDao();

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

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
                    //TODO: Load references
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
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_THAYDONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_THAYDONGHO;
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

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            timkv();
            
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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
        }

        private void LayBaoCao()
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

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            //var dotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, _nvDao.Get(b).MAKV);

            //if (ddlDOTGCS.SelectedValue == "%")
            //{
            //    dt = new ReportClass().ThayDongHo(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), 
            //        cboKhuVuc.SelectedValue).Tables[0];
            //}
            //else
            //{
            //    dt = new ReportClass().ThayDongHodOotIn(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
            //        cboKhuVuc.SelectedValue, "", ddlDOTGCS.SelectedValue, "DSTHAYDOTIN").Tables[0];
            //}
            if (_nvDao.Get(b).MAKV == "X")
            {
                //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                    int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSTHAYDHNLX");
                dt = ds.Tables[0];
            }
            else
            {
                var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                   int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSTHAYDHNLX");
                dt = ds.Tables[0];
            }

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHo.rpt");
            rp.Load(path);

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " (" 
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;            
            txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot ;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + _kvDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = _kvDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper() + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();            

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void ReLoadBaoCao()
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

            DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());

            DataTable dt = (DataTable)Session["DS_DonDangKy"];//new ReportClass().RPDSCDDangKy(DateTime.Parse(txtTuNgay.Text), DateTime.Parse(txtDenNgay.Text), cboKhuVuc.Text).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/ThayDongHo.rpt");
            rp.Load(path);

            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";

            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;

            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + _kvDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper();
            //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            //txtNguoiLap1.Text = txtNguoiLap.Text;
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = _kvDao.Get(cboKhuVuc.SelectedValue.ToString()).TENKV.ToUpper() + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();           
                        
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

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
              
                DataTable dt;

                //if (ddlDOTGCS.SelectedValue == "%")
                //{
                //    dt = new ReportClass().ThayDongHo(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), 
                //        cboKhuVuc.SelectedValue).Tables[0];
                //}
                //else
                //{
                //    dt = new ReportClass().ThayDongHodOotIn(Convert.ToInt32(ddlTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                //            cboKhuVuc.SelectedValue, "", ddlDOTGCS.SelectedValue, "DSTHAYDOTIN").Tables[0];
                //}   
                if (_nvDao.Get(b).MAKV == "X")
                {
                    //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                    var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                        int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSTHAYDHNLX");
                    dt = ds.Tables[0];
                }
                else
                {
                    var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                       int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSTHAYDHNLX");
                    dt = ds.Tables[0];
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDHN" + ddlTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
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
                //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //Response.Write(style);
                string style = @"<style> TD { mso-number-format:\@; } </style>";
                Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                //upnlBaoCao.Update();
            }
            catch { }            
        }

    }
}