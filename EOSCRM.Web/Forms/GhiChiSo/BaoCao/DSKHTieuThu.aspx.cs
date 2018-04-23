using System;
using System.Data;
using System.Web.UI.WebControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using EOSCRM.Util;
using EOSCRM.Dao ;
using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Common;

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
    public partial class DSKHTieuThu : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly LoaiDongHoDao _ldhDao = new LoaiDongHoDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

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
                    //var dt = (DataTable)Session[SessionKey.GCS_BAOCAO_DSKHTIEUTHU];
                    //Report(dt);
                    if(reloadm.Text == "1")
                    {
                        ReLoadBang();
                    }
                    else if(reloadm.Text == "2")
                    {
                        ReLoadGiam();
                    }
                    else if(reloadm.Text == "3")
                    {
                        ReaLoadKhdcld();
                    }
                    else if(reloadm.Text == "4")
                    {
                        ReaLodKhdkld();
                    }
                    else if(reloadm.Text == "5")
                    {
                        var dt = (DataTable)Session[SessionKey.GCS_BAOCAO_DSKHTIEUTHU];
                        Report(dt);
                    }
                    else if (reloadm.Text == "6")
                    {
                        ReaLodHopDong();
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
            Page.Title = Resources.Message.TITLE_GCS_BAOCAO_DANHSACHTIEUTHUCODIEUKIEN;

            var header = (Header)Master.FindControl("header");
            if (header == null) return;

            header.ModuleName = Resources.Message.MODULE_GHICHISO;
            header.TitlePage = Resources.Message.PAGE_GCS_BAOCAO_DANHSACHTIEUTHUCODIEUKIEN;

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

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

        private void LoadReferences()
        {
            txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            cboTHANG.SelectedIndex = DateTime.Now.Month - 1;

            txtDenNam.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            ddlDenThang.SelectedIndex = DateTime.Now.Month - 1;

            var listkhuvuc = new KhuVucDao().GetList();
            cboKhuVuc.DataSource = listkhuvuc;
            cboKhuVuc.DataTextField = "TENKV";
            cboKhuVuc.DataValueField = "MAKV";
            cboKhuVuc.DataBind();
            cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
            cboKhuVuc.Text = "%";

            // bind dllMDSD
            var mdsd = new MucDichSuDungDao().GetList();
            cboMucDichSuDung.DataSource = mdsd;
            cboMucDichSuDung.DataValueField = "MAMDSD";
            cboMucDichSuDung.DataTextField = "TENMDSD";
            cboMucDichSuDung.DataBind();
            cboMucDichSuDung.Items.Add(new ListItem("Tất cả", "%"));
            cboMucDichSuDung.Text = "%";

            txtNAM.Text = DateTime.Now.Year.ToString();

            txtNguoiLap.Text = LoginInfo.NHANVIEN != null ? LoginInfo.NHANVIEN.HOTEN : "";
            timkv();

            var loaidongho = _ldhDao.GetList();
            ddlLOAIDHLX.DataSource = loaidongho;
            ddlLOAIDHLX.DataValueField = "MALDH";
            ddlLOAIDHLX.DataTextField = "MALDH";
            ddlLOAIDHLX.DataBind();
            ddlLOAIDHLX.Items.Add(new ListItem("Tất cả", "%"));
            ddlLOAIDHLX.Text = "%";

            txtKLTieutThu.Text = "0";
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            var ds =
                new ReportClass().DsTieuThuDk(int.Parse(cboTHANG.Text.Trim()), int.Parse(txtNAM.Text.Trim()),
                                              txtMaDp.Text.Trim(), txtDuongPhu.Text.Trim(), cboMucDichSuDung.Text.Trim(),
                                              cboTrangThai.Text.Trim(), cboKhuVuc.Text.Trim(),
                                              int.Parse(txtKLTu.Text.Trim()), int.Parse(txtKLDen.Text.Trim()),
                                              decimal.Parse(txtTongTienTu.Text.Trim()),
                                              decimal.Parse(txtTongTienDen.Text.Trim()));
            if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
            Report(ds.Tables[0]);

            CloseWaitingDialog();
        }

        private void Report(DataTable dt)
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
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSKHTieuThu.rpt");
            rp.Load(path);

            var txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
            if (txtNguoiLap1 != null)
                txtNguoiLap1.Text = txtNguoiLap.Text.Trim();

            var txtKy = rp.ReportDefinition.ReportObjects["txtKy"] as TextObject;
            if (txtKy != null)
                txtKy.Text = string.Format("{0}/{1}", cboTHANG.SelectedValue, txtNAM.Text.Trim());

            var txtTu = rp.ReportDefinition.ReportObjects["txtTu"] as TextObject;
            if (txtTu != null)
                txtTu.Text = string.Format("Từ {0} đến {1} m3", txtKLTu.Text.Trim(), txtKLDen.Text.Trim());

            var txtNgay = rp.ReportDefinition.ReportObjects["txtNgay"] as TextObject;
            if (txtNgay != null)
                //txtNgay.Text = string.Format("Long Xuyên, ngày {0}", DateTime.Now.ToString("dd/MM/yyyy"));
                txtNgay.Text = string.Format("Long Xuyên, ngày {0}", day) +
                                string.Format(" tháng {0}", month) + string.Format(" năm {0}", year);
            
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            reloadm.Text="5";

            Session[SessionKey.GCS_BAOCAO_DSKHTIEUTHU] = dt;
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

        #region Đường phố
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        private void BindDuongPho()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (_nvDao.Get(b).MAKV == "X")
                {
                    var list = dpDao.GetList(_nvDao.Get(b).MAKV, txtKeywordDP.Text.Trim());
                    gvDuongPho.DataSource = list;
                    gvDuongPho.PagerInforText = list.Count.ToString();
                    gvDuongPho.DataBind();
                }
                else
                {
                    var list = dpDao.GetList(_nvDao.Get(b).MAKV, txtKeywordDP.Text.Trim());
                    gvDuongPho.DataSource = list;
                    gvDuongPho.PagerInforText = list.Count.ToString();
                    gvDuongPho.DataBind();
                }
            }
            catch { }
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            var kv = cboKhuVuc.Items.FindByValue(dp.MAKV);
            if (kv != null)
                cboKhuVuc.SelectedIndex = cboKhuVuc.Items.IndexOf(kv);
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
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
                            upnlGhiChiSo.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();

                            txtMaDp.Focus();
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDuongPho.PageIndex = e.NewPageIndex;                
                BindDuongPho();
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

        protected void linkBang_Click(object sender, EventArgs e)
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

            DataTable dt = new ReportClass().DSTTBANG1(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            
            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ 3 THÁNG BẰNG NHAU";

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

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReLoadBang()
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
                DataTable dt = new ReportClass().DSTTBANG1(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

                rp = new ReportDocument();
                var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
                rp.Load(path);

                TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
                txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ 3 THÁNG BẰNG NHAU";
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

        private void ReLoadGiam()
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
            DataTable dt = new ReportClass().DSTTGIAM(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ GIẢM LIÊN TỤC 3 THÁNG";
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

        protected void linkGiam_Click(object sender, EventArgs e)
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

            DataTable dt = new ReportClass().DSTTGIAM(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ GIẢM LIÊN TỤC 3 THÁNG";
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

            reloadm.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void linkkhd_Click(object sender, EventArgs e)
        {
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

            DataTable dt = new ReportClass().DSTTBTCLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");

            rp.Load(path);

            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ KHÔNG CÓ HOÁ ĐƠN CÓ LÝ DO";
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

            reloadm.Text = "3";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReaLoadKhdcld()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch  {  }
            }

            DataTable dt = new ReportClass().DSTTBTCLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");

            rp.Load(path);
            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ KHÔNG CÓ HOÁ ĐƠN CÓ LÝ DO";
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

            //reloadm.Text = "3";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            
        }

        protected void linkchd_Click(object sender, EventArgs e)
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch {  }
            }

            DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");

            rp.Load(path);
            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ KHÔNG CÓ HOÁ ĐƠN KHÔNG CÓ LÝ DO";
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

            reloadm.Text = "4";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReaLodKhdkld()
        {
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

            DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");

            rp.Load(path);
            TextObject txtTIEUDE1 = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
            txtTIEUDE1.Text = "DANH SÁCH TIÊU THỤ KHÔNG CÓ HOÁ ĐƠN KHÔNG CÓ LÝ DO";
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

            //reloadm.Text = "4";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
        }

        protected void linkinhopdong_Click(object sender, EventArgs e)
        {
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

            DataTable dt = new ReportClass().INHOPDONG(txtNguoiLap.Text.Trim()).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("../../../Reports/DonLapDatMoi/INHOPDONG.rpt");

            rp.Load(path);
            
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            reloadm.Text = "6";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        private void ReaLodHopDong()
        {
            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch  {  }
            }

            DataTable dt = new ReportClass().INHOPDONG(txtNguoiLap.Text.Trim()).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("../../../Reports/DonLapDatMoi/INHOPDONG.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlReport.Update();

            //reloadm.Text = "6";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void lkEXCELTT6TG_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlTTG6THANG.Text == "TTCHETTT")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, "TTCHETTT");
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=TTCHET" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";

                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");

                   // System.IO.StringWriter sw = new System.IO.StringWriter();
                   // System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

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

                if (ddlTTG6THANG.Text == "TTCUPTT")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, "TTCUPTT");
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=TTCUP" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";                   

                    Response.ContentEncoding = System.Text.Encoding.UTF8;
                    StringWriter sw = new StringWriter();
                    HtmlTextWriter hw = new HtmlTextWriter(sw);
                    hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                    //System.IO.StringWriter sw = new System.IO.StringWriter();
                    //System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

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

                if (ddlTTG6THANG.Text == "TT6KOHD")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, "TT6KOHD");
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=TT6KOHD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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

                if (ddlTTG6THANG.Text == "TT6GBLT")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, "TT6GBLT");
                    DataTable dt = ds.Tables[0];


                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=TT6GBLT" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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

                if (ddlTTG6THANG.Text == "TT6GLT")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, "TT6GLT");
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();                

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=TT6GLT" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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

                if (ddlTTG6THANG.Text == "TTG40STT")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                cboKhuVuc.SelectedValue, "TTG40STT");
                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=TTG40SOKYTRUOC" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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

                if (ddlTTG6THANG.Text == "DSKHMDK")
                {
                    //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                    //var ds = new ReportClass().DSTTGIAM6THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                     //           cboKhuVuc.SelectedValue, "DSKHMDK");

                    var ds = new ReportClass().DSTTMUCDICHKHAC(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                                Convert.ToInt32(ddlDenThang.SelectedValue), Convert.ToInt32(txtDenNam.Text.Trim()), cboKhuVuc.SelectedValue, "", "", "DSTTMDKPO");

                    DataTable dt = ds.Tables[0];

                    //Create a dummy GridView
                    GridView GridView1 = new GridView();
                    GridView1.AllowPaging = false;

                    GridView1.Font.Name = "Times New Roman";

                    GridView1.DataSource = dt;
                    GridView1.DataBind();

                    Response.Clear();
                    Response.Buffer = true;
                    Response.AddHeader("content-disposition", "attachment;filename=KHMDK" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.ms-excel";
                    System.IO.StringWriter sw = new System.IO.StringWriter();
                    System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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

                CloseWaitingDialog();
                upnlGhiChiSo.Update();                  
            }
            catch { }
        }       

        protected void ckDSDPLX_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckDSDPLX.Checked)
                {
                    txtMaDp.Enabled = true;

                    btnBrowseDP.Visible = true;

                    ddlLOAIDHLX.Enabled = false;
                    txtCONGSUATDHLX.Enabled = false;
                    cboMucDichSuDung.Enabled = false;

                    ckLOAIDHLX.Checked = false;
                    ckCONGSUATLX.Checked = false;
                    ckMDSDLX.Checked = false;
                    ckKLTieuThu.Checked = false;

                    txtKLTieutThu.Enabled = false;
                }
                else
                {
                    txtMaDp.Enabled = false;

                    btnBrowseDP.Visible = false;
                }
            }
            catch { }
        }

        protected void ckLOAIDHLX_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckLOAIDHLX.Checked)
                {
                    ddlLOAIDHLX.Enabled = true;

                    txtMaDp.Enabled = false;
                    btnBrowseDP.Visible = false;
                    txtCONGSUATDHLX.Enabled = false;
                    cboMucDichSuDung.Enabled = false;

                    ckDSDPLX.Checked = false;
                    ckCONGSUATLX.Checked = false;
                    ckMDSDLX.Checked = false;
                    ckKLTieuThu.Checked = false;

                    txtKLTieutThu.Enabled = false;
                }
                else
                {
                    ddlLOAIDHLX.Enabled = false;
                }
            }
            catch { }
        }

        protected void ckCONGSUATLX_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckCONGSUATLX.Checked)
                {
                    txtCONGSUATDHLX.Enabled = true;

                    txtMaDp.Enabled = false;
                    btnBrowseDP.Visible = false;
                    ddlLOAIDHLX.Enabled = false;
                    cboMucDichSuDung.Enabled = false;

                    ckDSDPLX.Checked = false;
                    ckLOAIDHLX.Checked = false;
                    ckMDSDLX.Checked = false;
                    ckKLTieuThu.Checked = false;

                    txtKLTieutThu.Enabled = false;
                }
                else
                {
                    txtCONGSUATDHLX.Enabled = false;
                }
            }
            catch { }
        }

        protected void ckMDSDLX_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckMDSDLX.Checked)
                {
                    cboMucDichSuDung.Enabled = true;

                    txtMaDp.Enabled = false;
                    btnBrowseDP.Visible = false;
                    ddlLOAIDHLX.Enabled = false;
                    txtCONGSUATDHLX.Enabled = false;

                    ckDSDPLX.Checked = false;
                    ckLOAIDHLX.Checked = false;
                    ckCONGSUATLX.Checked = false;
                    ckKLTieuThu.Checked = false;

                    txtKLTieutThu.Enabled = false;
                }
                else
                {
                    cboMucDichSuDung.Enabled = false;
                }
            }
            catch { }
        }

        private void DSDuongPho()
        {
            try
            {
                DataTable dt;

                var ds = _rpClass.BienKHNuocLX(txtMaDp.Text.Trim(), "", cboKhuVuc.SelectedValue, "", "", Convert.ToInt16(cboTHANG.Text.Trim()), 
                    Convert.ToInt16(txtNAM.Text.Trim()), DateTime.Now, DateTime.Now, "DSKHMADPLX");
                dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.Font.Name = "Times New Roman";
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DSKHDP" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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
                upnlGhiChiSo.Update();  
            }
            catch { }
        }

        private void DSMAMDSD()
        {
            try
            {
                DataTable dt;

                var ds = _rpClass.BienKHNuocLX(cboMucDichSuDung.SelectedValue, "", cboKhuVuc.SelectedValue, "", "", Convert.ToInt16(cboTHANG.Text.Trim()),
                    Convert.ToInt16(txtNAM.Text.Trim()), DateTime.Now, DateTime.Now, "DSKHMAMDSDLX");
                dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.Font.Name = "Times New Roman";
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DSKHMDSD" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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
                upnlGhiChiSo.Update();
            }
            catch { }
        }

        private void DSLoaiDH()
        {
            try
            {
                DataTable dt;

                var ds = _rpClass.BienKHNuocLX(ddlLOAIDHLX.SelectedValue, "", cboKhuVuc.SelectedValue, "", "", Convert.ToInt16(cboTHANG.Text.Trim()),
                    Convert.ToInt16(txtNAM.Text.Trim()), DateTime.Now, DateTime.Now, "DSKHLOAIDHLX");
                dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.Font.Name = "Times New Roman";
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DSKHLDH" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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
                upnlGhiChiSo.Update();
            }
            catch { }
        }

        private void DSCongSuatDH()
        {
            try
            {
                DataTable dt;

                var ds = _rpClass.BienKHNuocLX(txtCONGSUATDHLX.Text.Trim(), "", cboKhuVuc.SelectedValue, "", "", Convert.ToInt16(cboTHANG.Text.Trim()),
                    Convert.ToInt16(txtNAM.Text.Trim()), DateTime.Now, DateTime.Now, "DSKHCSDHLX");
                dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.Font.Name = "Times New Roman";
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DSKHCSDH" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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
                upnlGhiChiSo.Update();
            }
            catch { }
        }

        protected void btExcelDSDPLX_Click(object sender, EventArgs e)
        {
            try
            {
                if (ckKLTieuThu.Checked)
                {
                    DSKLTieuThuLon();
                }

                if (ckDSDPLX.Checked)
                {
                    DSDuongPho();
                }
                
                if (ckMDSDLX.Checked)
                {
                    DSMAMDSD();
                }                  

                if (ckLOAIDHLX.Checked)
                {
                    DSLoaiDH();
                }
                       
                if (ckCONGSUATLX.Checked)
                {
                    DSCongSuatDH();
                }

                CloseWaitingDialog();
            }
            catch { }
        }

        protected void ckKLTieuThu_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckKLTieuThu.Checked)
                {
                    txtKLTieutThu.Enabled = true;

                    cboMucDichSuDung.Enabled = false;
                    txtMaDp.Enabled = false;
                    btnBrowseDP.Visible = false;
                    ddlLOAIDHLX.Enabled = false;
                    txtCONGSUATDHLX.Enabled = false;

                    ckDSDPLX.Checked = false;
                    ckLOAIDHLX.Checked = false;
                    ckCONGSUATLX.Checked = false;
                    ckMDSDLX.Checked = false;
                }
                else
                {
                    txtKLTieutThu.Enabled = false;
                }
            }
            catch { }
        }

        private void DSKLTieuThuLon()
        {
            try
            {
                //DataTable dt = new ReportClass().DSTTBTKLD(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue).Tables[0];
                var ds = new ReportClass().DSTTDK3THANG(Convert.ToInt32(cboTHANG.SelectedValue), Convert.ToInt32(txtNAM.Text.Trim()),
                            cboKhuVuc.SelectedValue, Convert.ToInt32(txtKLTieutThu.Text.Trim()), "", "TT3TDIEUKIENCT");
                DataTable dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.Font.Name = "Times New Roman";

                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TT3T" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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


               /* DataTable dt;

                var ds = _rpClass.BienKHNuocLX(txtCONGSUATDHLX.Text.Trim(), "", cboKhuVuc.SelectedValue, "", "", Convert.ToInt16(cboTHANG.Text.Trim()),
                    Convert.ToInt16(txtNAM.Text.Trim()), DateTime.Now, DateTime.Now, "DSKHTTDKCT");
                dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;

                GridView1.Font.Name = "Times New Roman";
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DSKHCSDH" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

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
                upnlGhiChiSo.Update();*/

                
            }
            catch { }
        }
        

    }
}