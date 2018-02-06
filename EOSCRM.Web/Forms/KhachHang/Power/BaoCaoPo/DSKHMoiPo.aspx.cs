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

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

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
using CrystalDecisions.Shared;

using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Net;
using OfficeOpenXml;
using OfficeOpenXml.Style;

namespace EOSCRM.Web.Forms.KhachHang.Power.BaoCaoPo
{
    public partial class DSKHMoiPo : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: Load references
                    LoadReferences();
                }
                else
                {
                    if (lbRELOAD.Text == "1")
                    { ReLoadBaoCao(); }
                    if (lbRELOAD.Text == "2")
                    { ReLoadBaoCaoMucDK(); }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void UnblockWaitingDialog()
        {
            ((PO)Page.Master).UnblockWaitingDialog();
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAOPO_DSKHMPO;

            var header = (UserControls.Header)Master.FindControl("header");
            if (header == null) return;
            header.ModuleName = Resources.Message.MODULE_KHACHHANG;
            header.TitlePage = Resources.Message.PAGE_KH_BAOCAOPO_DSKHMPO;

            //CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        private void LoadReferences()
        {           

            // bind dllMDSD
            var mdsd = new MucDichSuDungDao().GetList();
            cboMucDichSuDung.DataSource = mdsd;
            cboMucDichSuDung.DataValueField = "MAMDSD";
            cboMucDichSuDung.DataTextField = "TENMDSD";
            cboMucDichSuDung.DataBind();
            cboMucDichSuDung.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
            cboMucDichSuDung.Text = "%";

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();

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
                var dotin = _diDao.GetListKVDDNotP7(kvpo);
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
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(a.MAKV);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
            CloseWaitingDialog();
        }

        private void LayBaoCao()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var kv = _nvDao.Get(b);

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
                catch {   }
            }

            #endregion FreeMemory 
            
            var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
            var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

            DataTable ds;
            if (ddlDOTGCS.SelectedValue == "%")
            {
                ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO", TuNgay, DenNgay).Tables[0];
            }
            else
            {
                ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPODOTIN", TuNgay, DenNgay).Tables[0];
            }

            //var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO", TuNgay, DenNgay);            
            ReportNgayN(ds);

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbRELOAD.Text = "1";
            upnlBaoCao.Update();

            CloseWaitingDialog();            
            
        }

        private void ReportNgayN(DataTable dt)
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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKTPO.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            
            string bpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;
            string tenkvpo = _kvpoDao.Get(bpo).TENKV;

            //txtXN
            var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            if (txtXN != null)
                txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkvpo.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG MỚI ĐIỆN";
            //txtTENPHUONG
            string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " (" 
                    + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            if (txtTENPHUONG != null)
                txtTENPHUONG.Text = "KỲ: " + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()) + tendot;

            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        private void ReLoadBaoCao()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var kv = _nvDao.Get(b);

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

            var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
            var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

            //var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO", TuNgay, DenNgay);
            DataTable ds;
            if (ddlDOTGCS.SelectedValue == "%")
            {
                ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO", TuNgay, DenNgay).Tables[0];
            }
            else
            {
                ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPODOTIN", TuNgay, DenNgay).Tables[0];
            }

            //if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            ReportNgayN(ds);

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbRELOAD.Text = "1";
            upnlBaoCao.Update();

            CloseWaitingDialog();  
        }

        protected void lkEXCEL_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kv = _nvDao.Get(b);                

                int thanght = DateTime.Now.Month;
                int namht = DateTime.Now.Year;
                int thangF1 = Convert.ToInt32(cboTHANG.SelectedValue);
                int namF1 = Convert.ToInt32(txtNAM.Text.Trim());

                if (thangF1 == thanght && namF1 == namht)
                {
                    _rpClass.UPKHTTCOBIEN("", "", "", thanght, namht, "", "", "", "", "", 0, 0, 0, "UPCSTTVAOKHMOIPO");
                }

                if (kv.MAKV == "S") // CHAU DOC
                {
                    //ExportExcelKHMChauDocEPP();

                    int thangF = Convert.ToInt16(cboTHANG.SelectedValue);
                    int namF = Convert.ToInt32(txtNAM.Text.Trim());

                    var TuNgay = new DateTime(namF, thangF, 1);
                    var DenNgay = DateTime.Now;                    

                    var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCD", TuNgay, DenNgay);

                    //listkhm = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCD",  TuNgay, DenNgay);

                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=KHMD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
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


                    CloseWaitingDialog();
                    upnlBaoCao.Update();
                }
                else
                {
                    var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                    var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");
                    var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPODot", 
                        TuNgay, DenNgay);
                    //var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO_Ex", TuNgay, DenNgay);
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=KHMD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
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
                    string style = @"<style> TD { mso-number-format:\@; } </style>";
                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();


                    CloseWaitingDialog();
                    upnlBaoCao.Update();
                }
            }
            catch { }
        }

        private void ExportExcelKHMChauDocEPP()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kv = _nvDao.Get(b);

                int thangF = Convert.ToInt16(cboTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());

                var TuNgay = new DateTime(namF, thangF, 1);                
                var DenNgay = DateTime.Now;

                DataSet listkhm;
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    listkhm = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCD",
                        TuNgay, DenNgay);
                }
                else if (_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN == "DDP7D1")
                {
                    listkhm = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCDP7",
                        TuNgay, DenNgay);
                }
                else
                {
                    listkhm = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCD",
                        TuNgay, DenNgay);
                }

                using (DataSet ds2 = listkhm)
                {
                    if (ds2 != null && ds2.Tables.Count > 0)
                    {
                        using (ExcelPackage xp = new ExcelPackage())
                        {
                            foreach (DataTable dt2 in ds2.Tables)
                            {
                                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt2.TableName);

                               /* int rowstart = 2;
                                int colstart = 2;
                                int rowend = rowstart;
                                int colend = colstart + dt2.Columns.Count;

                                ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                               ws.Cells[rowstart, colstart, rowend, colend].Value = "DANH SÁCH KHÁCH HÀNG MỚI ĐIỆN (KỲ " + cboTHANG.SelectedValue +
                                        "/" + txtNAM.Text.Trim() + ")";
                               ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                                rowstart += 2;*/


                                int rowstart = 0;
                                int colstart = 1;
                                int rowend = rowstart;
                                int colend = colstart + dt2.Columns.Count;                                

                                rowstart += 1;

                                rowend = rowstart + dt2.Rows.Count;
                                ws.Cells[rowstart, colstart].LoadFromDataTable(dt2, true);
                                int i = 1;
                                foreach (DataColumn dc in dt2.Columns)
                                {
                                    i++;
                                    if (dc.DataType == typeof(decimal))
                                        //ws.Column(i).Style.Numberformat.Format = "#0.00";
                                        ws.Column(i).Style.Numberformat.Format = "#0";
                                }
                                ws.Cells[ws.Dimension.Address].AutoFitColumns();

                                ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Top.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Bottom.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Left.Style =
                                   ws.Cells[rowstart, colstart, rowend, colend].Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;

                            }
                            Response.AddHeader("content-disposition", "attachment;filename=" + ds2.DataSetName + ".xlsx");
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                            Response.BinaryWrite(xp.GetAsByteArray());
                            Response.End();
                        }
                    }
                }

                CloseWaitingDialog();
                upnlBaoCao.Update();
            }
            catch { }
        }

        protected void lkWORD_Click(object sender, EventArgs e)
        {
            try
            {
                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");
                var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO_Ex", TuNgay, DenNgay);
                DataTable dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                //Response.AddHeader("content-disposition", "attachment;filename=KHMD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.AddHeader("content-disposition", "attachment;filename=KHMD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                //Response.ContentType = "application/vnd.ms-excel";
                Response.ContentType = "application/vnd.ms-word ";
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


                CloseWaitingDialog();
                upnlBaoCao.Update();
            }
            catch { }
        }

        protected void lkKHMMucDK_Click(object sender, EventArgs e)
        {
            LayBaoCaoMucDK();
            CloseWaitingDialog();
        }

        private void LayBaoCaoMucDK()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var kv = _nvDao.Get(b);

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

            var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
            var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

            var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIMDKPO", TuNgay, DenNgay);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            ReportNgayN(ds.Tables[0]);

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbRELOAD.Text = "2";
            upnlBaoCao.Update();

            CloseWaitingDialog();

        }

        private void ReLoadBaoCaoMucDK()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var kv = _nvDao.Get(b);

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

            var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
            var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

            var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIMDKPO", TuNgay, DenNgay);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            ReportNgayN(ds.Tables[0]);

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbRELOAD.Text = "2";
            upnlBaoCao.Update();

            CloseWaitingDialog();
        }

        protected void lkXuatExcelTS_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kv = _nvDao.Get(b);                

                int thanght = DateTime.Now.Month;
                int namht = DateTime.Now.Year;
                int thangF1 = Convert.ToInt32(cboTHANG.SelectedValue);
                int namF1 = Convert.ToInt32(txtNAM.Text.Trim());

                if (thangF1 == thanght && namF1 == namht)
                {
                    _rpClass.UPKHTTCOBIEN("", "", "", thanght, namht, "", "", "", "", "", 0, 0, 0, "UPCSTTVAOKHMOIPO");
                }

                if (kv.MAKV == "S") // CHAU DOC
                {
                    //ExportExcelKHMChauDocEPP();

                    int thangF = Convert.ToInt16(cboTHANG.SelectedValue);
                    int namF = Convert.ToInt32(txtNAM.Text.Trim());

                    var TuNgay = new DateTime(namF, thangF, 1);
                    var DenNgay = DateTime.Now;

                    var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCD", TuNgay, DenNgay);

                    //listkhm = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPOCD",  TuNgay, DenNgay);

                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=KHMD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
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


                    CloseWaitingDialog();
                    upnlBaoCao.Update();
                }
                else
                {
                    var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                    var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");
                    var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMPODotTS",
                        TuNgay, DenNgay);
                    //var ds = new ReportClass().dsKHCBiKTPO(cboKhuVuc.Text.Trim(), "", "dsKHMOIPO_Ex", TuNgay, DenNgay);
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;
                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=KHMD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
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
                    string style = @"<style> TD { mso-number-format:\@; } </style>";

                    Response.Write(style);
                    Response.Output.Write(sw.ToString());
                    Response.Flush();
                    Response.End();


                    CloseWaitingDialog();
                    upnlBaoCao.Update();
                }
            }
            catch { }
        }

    }
}