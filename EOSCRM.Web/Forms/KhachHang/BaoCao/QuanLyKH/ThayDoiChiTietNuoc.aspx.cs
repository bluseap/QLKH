using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Common;
using System.IO;
using System.Web.UI;

using System.Text;
using System.Drawing.Imaging;
using System.Drawing.Printing;
using System.Collections.Generic;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class ThayDoiChiTietNuoc : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();        
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        int day = DateTime.Now.Day;
        int month = DateTime.Now.Month;
        int year = DateTime.Now.Year;

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
                   LoadReferences();
                }
                else
                {
                    if (Session[SessionKey.TK_BAOCAO_COBIEN] == "KH_BAOCAO_DSTDCTN")
                    {
                        var dt = (DataTable)Session[SessionKey.KH_BAOCAO_DSTDCTN];
                        ReLoadTDCTN(dt);
                    }                    
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

            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.DataSource = listkhuvuc;
            cboKhuVuc.DataTextField = "TENKV";
            cboKhuVuc.DataValueField = "MAKV";
            cboKhuVuc.DataBind();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            cboKhuVuc.Text = "%";            

            txtNAM.Text = DateTime.Now.Year.ToString();

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
                catch
                {

                }
            }

            #endregion FreeMemory

            //DataTable dt;
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];
            if (ddlDOTGCS.SelectedValue == "%")
            {
                var ds = new ReportClass().DSTHAYDOICHITIETNUOC(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue);
                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReLoadTDCTN(ds.Tables[0]);

                //divCR.Visible = true;
                //upnlReport.Update();
                //Session[SessionKey.KH_BAOCAO_DSTDCTN] = "KH_BAOCAO_DSTDCTN";                
                //CloseWaitingDialog();
            }
            else
            {
                var ds = new ReportClass().DSTHAYDOICHITIETNUOCDOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), 
                        cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTNDOTIN");

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReLoadTDCTN(ds.Tables[0]);

                //divCR.Visible = true;
                //upnlReport.Update();
                //Session[SessionKey.KH_BAOCAO_DSTDCTN] = "KH_BAOCAO_DSTDCTN";  
                //CloseWaitingDialog();
            }

            divCR.Visible = true;
            upnlReport.Update();
            Session[SessionKey.TK_BAOCAO_COBIEN] = "KH_BAOCAO_DSTDCTN";
            CloseWaitingDialog();

            //rp = new ReportDocument();
            //var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSTHAYDOICTNUOC.rpt");

            //rp.Load(path);

            //TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            //txtTIEUDE1.Text = "DANH SÁCH THAY ĐỔI CHI TIẾT NƯỚC";

            //string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
            //    + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            //TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            //txtTuNgay1.Text = "KỲ " + cboTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;
            //TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            //xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            ////TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            ////txtNguoiLap1.Text = txtNguoiLap.Text;
            //var d = DateTime.Now.Day;
            //var m = DateTime.Now.Month;
            //var y = DateTime.Now.Year;

            //TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            //ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
            //        m + " năm " + y;

            //rp.SetDataSource(dt);
            //rpViewer.ReportSource = rp;
            //rpViewer.DataBind();

            ////divCR.Visible = true;
            ////upnlReport.Update();

            ////Session[SessionKey.KH_BAOCAO_DSTDCTN] = "KH_BAOCAO_DSTDCTN";
            ////Session["DS_DonDangKy"] = dt;
            ////Session[Constants.REPORT_FREE_MEM] = rp;
            ////CloseWaitingDialog();
        }

        private void ReLoadTDCTN(DataTable dt)
        {
            if (dt == null)
                return;

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

            

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSTHAYDOICTNUOC.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH THAY ĐỔI CHI TIẾT NƯỚC";

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

            Session[SessionKey.KH_BAOCAO_DSTDCTN] = dt;
            //Session["DS_DonDangKy"] = dt;            
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();         
        }

        protected void btXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;

                if (ddlDSTDCT.SelectedValue == "%")
                {
                    if (_nvDao.Get(b).MAKV == "X")
                    {
                        if (ddlDOTGCS.SelectedValue == "%")
                        {
                            dt = new ReportClass().DSTHAYDOICHITIETNUOCLX(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                        }
                        else
                        {
                            dt = new ReportClass().DSTHAYDOICHITIETNUOCDOTINLX(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                    cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTNDOTIN").Tables[0];
                        }
                    }
                    else
                    {
                        var dotinhd = _diDao.Get(ddlDOTGCS.SelectedValue);

                        if (ddlDOTGCS.SelectedValue == "%")
                        {
                            dt = new ReportClass().DSTHAYDOICHITIETNUOC(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                        }
                        else if (dotinhd.MADOTIN == "NNTHD1")
                        {
                            dt = new ReportClass().DSTHAYDOICHITIETNUOCDOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                    cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTNDOTINTH").Tables[0];
                        }
                        else
                        {
                            dt = new ReportClass().DSTHAYDOICHITIETNUOCDOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                    cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTNDOTIN").Tables[0];
                        }
                    }
                }
                else // long xuyen theo phien
                {
                    if (ddlDOTGCS.SelectedValue == "%")
                    {
                        dt = new ReportClass().DSTDCTNLX(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", ddlPHIENLX.SelectedValue, ddlDSTDCT.SelectedValue).Tables[0];
                    }
                    else
                    {
                        dt = new ReportClass().DSTDCTNLX(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", ddlPHIENLX.SelectedValue, ddlDSTDCT.SelectedValue).Tables[0];
                    }
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDCT" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
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

        protected void btXuatExcelDP_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;

                if (_nvDao.Get(b).MAKV == "X")
                {
                    if (ddlDOTGCS.SelectedValue == "%")
                    {
                        dt = new ReportClass().DSTHAYDOICHITIETNUOCDPLX(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    }
                    else
                    {
                        dt = new ReportClass().DSTHAYDOICHITIETNUOCDPLX(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    }
                }
                else
                {
                    if (ddlDOTGCS.SelectedValue == "%")
                    {
                        dt = new ReportClass().DSTHAYDOICHITIETNUOC(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    }
                    else
                    {
                        dt = new ReportClass().DSTHAYDOICHITIETNUOCDOTIN(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, ddlDOTGCS.SelectedValue, "", "DSTDCTNDOTIN").Tables[0];
                    }
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDCT" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
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
