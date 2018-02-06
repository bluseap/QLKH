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


namespace EOSCRM.Web.Forms.GhiChiSo.BaoCao
{
    public partial class DSKHTieuThu : Authentication
    {
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
            var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
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
                catch
                {

                }
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
                catch
                {

                }
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

            //reloadm.Text = "6";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }



    }
}