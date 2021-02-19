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

using OfficeOpenXml;
using OfficeOpenXml.Style;

using System.Collections.Generic;
using System.Linq;

namespace EOSCRM.Web.Forms.KhachHang.BaoCao.QuanLyKH
{
    public partial class BCDSKHMoi : Authentication
    {
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

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
                    if (lbRELOAD.Text.Trim() == "1")
                    {
                        ReportNgayTSon();
                    }
                    if (lbRELOAD.Text.Trim() == "2")
                    {
                        ReportCThanh();
                    }
                    //ReLoadBaoCao();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_BAOCAO_QLKH_DANHSACHKHMOI;

            var header = (UserControls.Header) Master.FindControl("header");
            if (header == null) return;
            header.ModuleName = Resources.Message.MODULE_KHACHHANG;
            header.TitlePage = Resources.Message.PAGE_KH_BAOCAO_QLKH_DANHSACHKHMOI;

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
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
                        cboKhuVuc.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void LoadReferences()
        {
            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.DataSource = listkhuvuc;
            cboKhuVuc.DataTextField = "TENKV";
            cboKhuVuc.DataValueField = "MAKV";
            cboKhuVuc.DataBind();
            cboKhuVuc.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
            cboKhuVuc.Text = "%";

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

            var dotin = _diDao.GetListKVNN(_nvDao.Get(b).MAKV);
            ddlDOTGCS.Items.Clear();
            ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
            foreach (var d in dotin)
            {
                ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
            }

            ddlDenThang.SelectedIndex = DateTime.Now.Month - 1;
            txtDenNam.Text = DateTime.Now.Year.ToString();

            if (_nvDao.Get(b).MAKV == "X")
            {
                lbDenThang.Visible = false;
                ddlDenThang.Visible = false;
                txtDenNam.Visible = false;
                lbGhiChuDenThang.Visible = false;
            }
            else
            {
                //if (_nvDao.Get(b).MAKV == "P")
                //{
                //    lkXuatExcelTS.Visible = false;
                //}

                lbDenThang.Visible = false;
                ddlDenThang.Visible = false;
                txtDenNam.Visible = false;
                lbGhiChuDenThang.Visible = false;
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
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            var dt = new ReportClass().DskhMoiDotIn(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboMucDichSuDung.Text.Trim(),
                                          cboTrangThai.Text.Trim(), cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue,
                                          "", "DSKHMDOTIN").Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSKhachHang.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH KHÁCH HÀNG MỚI";

            string dotin = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + cboTHANG.Text.Trim() + " NĂM " + txtNAM.Text.Trim() + dotin;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbRELOAD.Text = "2";
            upnlBaoCao.Update();

            CloseWaitingDialog();
            //rp.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, "PersonDetails");

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;

            //DateTime TuNgay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim());
            //DateTime DenNgay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim());            

            //if (kv.MAKV != "O")
            //{
            //    DataTable ds;
            //    //dt = new ReportClass().DskhMoi(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboMucDichSuDung.Text.Trim(),
            //     //                          cboTrangThai.Text.Trim(), cboKhuVuc.Text.Trim()).Tables[0];
            //    var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
            //    var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

            //    //var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, cboKhuVuc.Text.Trim());
            //    ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMOIDOTIN", TuNgay, DenNgay).Tables[0];                 

            //    if (ds == null) { CloseWaitingDialog(); return; }
            //    ReportNgayN(ds);                

            //    divCR.Visible = true;
            //    upnlCrystalReport.Update();

            //    lbRELOAD.Text = "1";
            //    upnlBaoCao.Update();

            //    CloseWaitingDialog();
            //}
            //else // dung cho Chau Thanh
            //{
            //    //DataTable dt;

            //    //var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, cboKhuVuc.Text.Trim());
            //    var dt = new ReportClass().DskhMoiDotIn(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboMucDichSuDung.Text.Trim(),
            //                              cboTrangThai.Text.Trim(), cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue , 
            //                              "", "DSKHMDOTIN").Tables[0];

            //    rp = new ReportDocument();
            //    var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSKhachHang.rpt");
            //    rp.Load(path);

            //    rp.SetDataSource(dt);
            //    rpViewer.ReportSource = rp;
            //    rpViewer.DataBind();

            //    TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            //    txtTIEUDE1.Text = "DANH SÁCH KHÁCH HÀNG MỚI";

            //    string dotin = ddlDOTGCS.SelectedValue == "%" ? "" : " (" 
            //        + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            //    TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            //    txtTuNgay1.Text = "KỲ " + cboTHANG.Text.Trim() + " NĂM " + txtNAM.Text.Trim() + dotin;
            //    TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            //    xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            //    var d = DateTime.Now.Day;
            //    var m = DateTime.Now.Month;
            //    var y = DateTime.Now.Year;
            //    TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            //    ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
            //            m + " năm " + y;             

            //    divCR.Visible = true;
            //    upnlCrystalReport.Update();

            //    lbRELOAD.Text = "2";
            //    upnlBaoCao.Update();

            //    CloseWaitingDialog();
            //    //rp.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, "PersonDetails");

            //    Session["DSBAOCAO"] = dt;
            //    Session[Constants.REPORT_FREE_MEM] = rp;
            //}

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
// ReSharper disable EmptyGeneralCatchClause
                catch
// ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory            

            string path;
            if (kv.MAKV != "O")
            {
                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

                //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI", TuNgay, DenNgay);
                var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMOIDOTIN", TuNgay, DenNgay);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReportNgayN(ds.Tables[0]);
                
                divCR.Visible = true;
                upnlCrystalReport.Update();

                lbRELOAD.Text = "1";
                upnlBaoCao.Update();

                CloseWaitingDialog();
            }
            else
            {
                var dt = (DataTable)Session["DSBAOCAO"];
                rp = new ReportDocument();

                path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSKhachHang.rpt");
                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
                txtTIEUDE1.Text = "DANH SÁCH KHÁCH HÀNG MỚI";
                TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay1.Text = "KỲ " + cboTHANG.Text.Trim() + " NĂM " + txtNAM.Text.Trim();
                TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
                var d = DateTime.Now.Day;
                var m = DateTime.Now.Month;
                var y = DateTime.Now.Year;
                TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                        m + " năm " + y;

                divCR.Visible = true;
                upnlCrystalReport.Update();

                lbRELOAD.Text = "2";
                upnlBaoCao.Update();


                Session["DSBAOCAO"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;
            }            
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
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSCBIKT.rpt");
            //var path = Server.MapPath("~/Reports/QuanLyKhachHang/DSCBIKT.rpt");
            rp.Load(path);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string tenkv = _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;

            //txtXN
            var txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            if (txtXN != null)
                txtXN.Text = "XÍ NGHIỆP ĐIỆN NƯỚC " + tenkv.ToUpper();
            //txtTIEUDE
            var txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            if (txtTIEUDE != null)
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG MỚI";
            //txtTENPHUONG
            string dotin = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            var txtTENPHUONG = rp.ReportDefinition.ReportObjects["txtTENPHUONG"] as TextObject;
            if (txtTENPHUONG != null)
                txtTENPHUONG.Text = "KỲ: " + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()) + dotin;


            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;

            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = "An Giang, ngày " + d + " tháng " + m + " năm " + y;

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();
           
            //divReport.Visible = true;
            upnlCrystalReport.Update();

            Session[SessionKey.TK_BAOCAO_DONDANGKYNGAYN] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        #region Đường phố
        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }
        
        private void BindDuongPho()
        {
            var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            // update khu vuc, generate new madb, update label
            var kv = cboKhuVuc.Items.FindByValue(dp.MAKV);
            if (kv != null)
                cboKhuVuc.SelectedIndex = cboKhuVuc.Items.IndexOf(kv);
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDuongPho.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var res = id.Split('-');
                        var dp = dpDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMaDp.ClientID, dp.MADP);
                            SetControlValue(txtDuongPhu.ClientID, dp.DUONGPHU);

                            UpdateKhuVuc(dp);
                            upnlBaoCao.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion

        protected void lkEXCEL_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var nhanvien = _nvDao.Get(b);

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                int thanght = DateTime.Now.Month;
                int namht = DateTime.Now.Year;
                int thangF = Convert.ToInt32(cboTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());               

                if (thangF == thanght && namF == namht)
                {
                    _rpClass.UPKHTTCOBIEN("", "", "", thanght, namht, "", "", "", "", "", 0, 0, 0, "UPCSTTVAOKHMOI");
                }                            

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                    var ds = new ReportClass().BienKHNuoc("", cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, ddlPHIENLX.SelectedValue,
                        int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), "DSKHMLXPHIEN");
                    dt = ds.Tables[0];
                }
                else
                {
                    if (_nvDao.Get(b).MAKV == "N") // chau phu
                    {
                        //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOIExCP", TuNgay, DenNgay);
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "S") // chau doc
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDotCD", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "L" || _nvDao.Get(b).MAKV == "M" || _nvDao.Get(b).MAKV == "Q") // tri ton, tinh bien, an phu
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "U") // thoai son
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDotTS", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];                        
                    }
                }                

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
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
                upnlBaoCao.Update();
            }
            catch { }
        }       

        protected void lkWORD_Click(object sender, EventArgs e)
        {
            ExportExcelEPPLUS();
        }

        private void ReportNgayTSon()
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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
            var DenNgay = DateTimeUtil.GetVietNamDate("01/01/2011");

            //var dt = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI", TuNgay, DenNgay);
            //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMOIDOTIN", TuNgay, DenNgay);
            var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, cboKhuVuc.Text.Trim());
            var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, 
                                    "dsKHMOIDOTIN", TuNgay, DenNgay);

            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            ReportNgayN(ds.Tables[0]);

            divCR.Visible = true;
            upnlCrystalReport.Update();

            //lbRELOAD.Text = "1";
            upnlBaoCao.Update();

            CloseWaitingDialog();
        }

        private void ReportCThanh()
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
                // ReSharper disable EmptyGeneralCatchClause
                catch
                // ReSharper restore EmptyGeneralCatchClause
                {

                }
            }

            #endregion FreeMemory

            var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, cboKhuVuc.Text.Trim());
            var dt = new ReportClass().DskhMoiDotIn(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboMucDichSuDung.Text.Trim(),
                                           cboTrangThai.Text.Trim(), cboKhuVuc.Text.Trim(),
                                           ddlDOTGCS.SelectedValue, "", "DSKHMDOTIN").Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../../Reports/QuanLyKhachHang/DSKhachHang.rpt");
            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH KHÁCH HÀNG MỚI";

            string dotin = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";
            TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
            txtTuNgay1.Text = "KỲ " + cboTHANG.Text.Trim() + " NĂM " + txtNAM.Text.Trim() + dotin;
            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + cboKhuVuc.SelectedItem.ToString().ToUpper();
            var d = DateTime.Now.Day;
            var m = DateTime.Now.Month;
            var y = DateTime.Now.Year;
            TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
            ngay.Text = cboKhuVuc.SelectedItem + ", ngày " + d + " tháng " +
                    m + " năm " + y;

            divCR.Visible = true;
            upnlCrystalReport.Update();

            //lbRELOAD.Text = "2";
            upnlBaoCao.Update();

            CloseWaitingDialog();
            //rp.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.ExcelRecord, Response, true, "PersonDetails");

            Session["DSBAOCAO"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }        

        private void ExportExcelEPPLUS()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                    var ds = new ReportClass().BienKHNuoc("", cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, ddlPHIENLX.SelectedValue,
                        int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), "DSKHMLXPHIEN");
                    dt = ds.Tables[0];
                }
                else
                {
                    if (_nvDao.Get(b).MAKV == "N")
                    {
                        var ds2 = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOIExCP", TuNgay, DenNgay);
                        dt = ds2.Tables[0];
                    }
                    else
                    {
                        var ds3 = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds3.Tables[0];
                    }
                }

                var dataset = new ReportClass().BienKHNuoc("", cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, ddlPHIENLX.SelectedValue,
                        int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), "DSKHMLXPHIEN");

                using (DataSet ds2 = dataset)
                {
                    if (ds2 != null && ds2.Tables.Count > 0)
                    {
                        using (ExcelPackage xp = new ExcelPackage())
                        {
                            foreach (DataTable dt2 in ds2.Tables)
                            {
                                ExcelWorksheet ws = xp.Workbook.Worksheets.Add(dt2.TableName);

                                int rowstart = 2;
                                int colstart = 2;
                                int rowend = rowstart;
                                int colend = colstart + dt2.Columns.Count;

                                ws.Cells[rowstart, colstart, rowend, colend].Merge = true;
                                ws.Cells[rowstart, colstart, rowend, colend].Value = dt2.TableName;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Font.Bold = true;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                                ws.Cells[rowstart, colstart, rowend, colend].Style.Fill.BackgroundColor.SetColor(System.Drawing.Color.LightGray);

                                rowstart += 2;
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

            }
            catch { }
        }

        protected void lkExcelLX_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                    var ds = new ReportClass().BienKHNuoc("", cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, ddlPHIENLX.SelectedValue,
                        int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), "DSKHMLXPHIEN");
                    dt = ds.Tables[0];
                }
                else
                {
                    if (_nvDao.Get(b).MAKV == "N")
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOIExCP", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];

                        //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_Ex", TuNgay, DenNgay);
                        //dt = ds.Tables[0];
                    }
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
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
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);

                //string style = @"<style> TD { mso-number-format:\@; } </style>";
                //Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                upnlBaoCao.Update();
            }
            catch { }
        }

        protected void lkXuatExcelTS_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var nhanvien = _nvDao.Get(b);

                var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                int thanght = DateTime.Now.Month;
                int namht = DateTime.Now.Year;
                int thangF = Convert.ToInt32(cboTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());

                if (thangF == thanght && namF == namht)
                {
                    _rpClass.UPKHTTCOBIEN("", "", "", thanght, namht, "", "", "", "", "", 0, 0, 0, "UPCSTTVAOKHMOI");
                }

                //var kynay = new DateTime(namht, thanght, 1);
                //bool dung11 = _gcsDao.IsLockTinhCuocKy(kynay, nhanvien.MAKV);
                //if (dung11 == false)
                //{
                //    if (thangF == thanght && namF == namht)
                //    {
                //        _rpClass.UPKHTTCOBIEN("", "", "", thanght, namht, "", "", "", "", "", 0, 0, 0, "UPCSTTVAOKHMOI");
                //    }
                //}                

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {                    
                    var ds = new ReportClass().BienKHNuoc("", cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, ddlPHIENLX.SelectedValue,
                        int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()), "DSKHMLXPHIEN");
                    dt = ds.Tables[0];
                }
                else
                {
                    if (_nvDao.Get(b).MAKV == "N") // chau phu
                    {
                        //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOIExCP", TuNgay, DenNgay);
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "S") // chau doc
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDotCD", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "L" || _nvDao.Get(b).MAKV == "M" || _nvDao.Get(b).MAKV == "Q") // tri ton, tinh bien, an phu
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "U") // thoai son
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDotTS", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else if (_nvDao.Get(b).MAKV == "P") // phu tan
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDotPT", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                    else 
                    {
                        var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), ddlDOTGCS.SelectedValue, "dsKHMExDot", TuNgay, DenNgay);
                        dt = ds.Tables[0];
                    }
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
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
                upnlBaoCao.Update();
            }
            catch { }
        }


    }
}