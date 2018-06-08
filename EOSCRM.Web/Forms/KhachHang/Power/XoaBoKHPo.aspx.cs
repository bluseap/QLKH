using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Collections.Generic;
using System.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.IO;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class XoaBoKHPo : Authentication
    {
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly TieuThuPoDao _ttpoDao = new TieuThuPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly KhachHangXoaPoDao _khxpoDao = new KhachHangXoaPoDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ApToDao _atDao = new ApToDao();

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

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }
        #endregion

        #region loc,up
        private Mode UpdateMode
        {
            get
            {
                try
                {
                    if (Session[SessionKey.MODE] != null)
                    {
                        var mode = Convert.ToInt32(Session[SessionKey.MODE]);
                        return (mode == Mode.Update.GetHashCode()) ? Mode.Update : Mode.Create;
                    }
                    return Mode.Create;
                }
                catch (Exception)
                {
                    return Mode.Create;
                }
            }
            set
            {
                Session[SessionKey.MODE] = value.GetHashCode();
            }
        }

        private FilteredMode Filtered
        {
            get
            {
                try
                {
                    if (Session[SessionKey.FILTEREDMODE] != null)
                    {
                        var mode = Convert.ToInt32(Session[SessionKey.FILTEREDMODE]);
                        return (mode == FilteredMode.Filtered.GetHashCode()) ? FilteredMode.Filtered : FilteredMode.None;
                    }
                    return FilteredMode.None;
                }
                catch (Exception)
                {
                    return FilteredMode.None;
                }
            }
            set
            {
                Session[SessionKey.FILTEREDMODE] = value.GetHashCode();
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_XoaBoKHPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindKhachHangXoa();
                }
                else
                {
                    var dt = (DataTable)Session[SessionKey.KH_BAOCAOPO_XOABOKHPO];
                    ReLoadBaoCao(dt);

                    gvKhachHang.Visible = false;
                    upnlCustomers.Update();                                
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
         
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_XOABOKHPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_XOABOKHPO;
            }

            CommonFunc.SetPropertiesForGrid(gvKhachHang);
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
        }

        private void LoadStaticReferences()
        {
            try
            {

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                timkv();

                //xa phuong
                var makv = _kvpoDao.Get(ddlKHUVUC1.SelectedValue);
                var xaphuong = _xpDao.GetListKV(makv.MAKV);
                ddlXAPHUONG.Items.Clear();
                ddlXAPHUONG.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var xp in xaphuong)
                {
                    ddlXAPHUONG.Items.Add(new ListItem(xp.TENXA, xp.MAXA));
                }
                //Ap khóm
                var apkhom = _atDao.GetList(makv.MAKV, ddlXAPHUONG.SelectedValue);
                ddlAPKHOM.Items.Clear();
                ddlAPKHOM.Items.Add(new ListItem("Tất cả", "%"));
                /*foreach (var ak in apkhom)
                {
                    ddlAPKHOM.Items.Add(new ListItem(ak.TENAPTO, ak.MAAPTO));
                }*/

                divCR.Visible = false;
                //upnlCrystalReport.Update();
            }
            catch { }
        }

        private void timkv()
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
                    ddlKHUVUC1.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKV(_kvpoDao.GetPo(d).MAKVPO);
                    ddlKHUVUC1.Items.Clear();
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        private void BindKhachHangXoa()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                string makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

                var list = _khxpoDao.GetListKV(makvpo);

                gvKhachHang.DataSource = list;
                gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();

                upnlCustomers.Update();
            }
            catch { }
        }

        #region gvKhachHang
        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectHD":
                        var kh = _khxpoDao.Get(id);

                        lblIDKH.Text = kh.IDKHPO;
                        txtGhiChu.Text = kh.LYDOXOA;

                        CloseWaitingDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvKhachHang.PageIndex = e.NewPageIndex;
                BindKhachHangXoa();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("linkMa") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion
                
        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            try
            {
                int thangIndex = 0;
                if (DateTime.Now.Month == 1)
                {
                    thangIndex = 11;
                }
                else
                {
                    thangIndex = DateTime.Now.Month - 2;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);

                //var kynay = new DateTime(2013, 6, 1);
                bool dung = _gcspoDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);

                if (txtNAM.Text == Convert.ToString(nam) || txtNAM.Text == Convert.ToString(namIndex))
                {
                    if (dung == true) //kiem tra khoa so ky ghi
                    {
                        CloseWaitingDialog();
                        HideDialog("divKhachHang");
                        ShowInfor("Đã khoá sổ. Không được xóa bộ.");
                    }
                    else
                    {
                        UnblockDialog("divKhachHang");
                        //BindKhachHang();
                        CloseWaitingDialog();
                        upnlKhachHang.Update();
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowInfor("Chọn năm xóa bộ cho đúng.");
                }

                divCR.Visible = false;
                upnlCrystalReport.Update();
            }
            catch { }
        }

        private void BindKhachHang()
        {
            try
            {
                var danhsach = _khpoDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                               txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                               txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                               ddlKHUVUC.SelectedValue.Trim());
                gvDanhSach.DataSource = danhsach;
                gvDanhSach.PagerInforText = danhsach.Count.ToString();                
                gvDanhSach.DataBind();
               
                CloseWaitingDialog();
                upnlKhachHang.Update();
            }
            catch { }
        }

        #region gvDanhSach
        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        //var khachhang = _khDao.GetKhachHangFromMadb(id);
                        var khachhang = _khpoDao.Get(id);
                        if (khachhang != null)
                        {
                            BindStatus(khachhang);
                            HideDialog("divKhachHang");
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

        protected void gvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnIDDS") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDanhSach.PageIndex = e.NewPageIndex;
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        private void BindStatus(KHACHHANGPO kh)
        {
            try
            {
                txtSODB.Text = (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).ToString();
                lblTENKH.Text = kh.TENKH.ToString();
                lblIDKH.Text = kh.IDKHPO.ToString();
                lblIDKH.Text = kh.IDKHPO.ToString();
                lblTENDP.Text = kh.DUONGPHOPO != null ? kh.DUONGPHOPO.TENDP.ToString() : "";
                lblTENKV.Text = kh.KHUVUCPO != null ? kh.KHUVUCPO.TENKV.ToString() : "";
                lblMAMDSD.Text = kh.MAMDSDPO.ToString();
                var tieuthu = _ttpoDao.GetTN(kh.IDKHPO, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                if (tieuthu != null)
                {
                    lblCSMOI.Text = Convert.ToString(tieuthu.CHISOCUOI);
                    lblCSCU.Text = Convert.ToString(tieuthu.CHISODAU);
                    lblTIEUTHU.Text = Convert.ToString(tieuthu.KLTIEUTHU);
                    lblTHANHTIEN.Text = Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENDIEN));
                    lblTHUEGTGT.Text = Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENTHUE));
                    lblTONGTIEN.Text = Convert.ToString(String.Format("{0:#.####}", tieuthu.TONGTIEN));

                    upnlThongTin.Update();
                    CloseWaitingDialog();
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Không có tiêu thụ trong tháng này. Xin chọn khách hàng lại", txtSODB.ClientID);
                }
                upnlThongTin.Update();
            }
            catch { }
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            try
            {
                BindKhachHang();
                CloseWaitingDialog();
                upnlKhachHang.Update();
            }
            catch { }
        }        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                int thangIndex = 0;
                if (DateTime.Now.Month == 1)
                {
                    thangIndex = 11;
                }
                else
                {
                    thangIndex = DateTime.Now.Month - 2;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);

                // Authenticate
                if (!HasPermission(Functions.KH_XoaBoKHPo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                bool dung = _gcspoDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
                //bool dung = _gcsDao.IsLockTinhCuocKy1(kynay, ddlKHUVUC1.SelectedValue,);

                //kiem tra khoa so
                //if (dung == true)
                //{
                //    CloseWaitingDialog();
                //    ShowInfor("Đã khoá sổ. Không được phục hồi xóa bộ khách hàng.");
                //    return;
                //}               
                               
                RestoreKH();
                BindKhachHangXoa(); 
                upnlCustomers.Update();

                Clear();
                upnlThongTin.Update();

                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int thangIndex = 0;
                if (DateTime.Now.Month == 1)
                {
                    thangIndex = 11;
                }
                else
                {
                    thangIndex = DateTime.Now.Month - 2;
                }

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kh = _khpoDao.Get(lblIDKH.Text.Trim());

                //bool dung = _gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
                bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay, ddlKHUVUC1.SelectedValue, kh.MADPPO);

                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ. Không được xóa bộ khách hàng.");
                    return;
                }

                //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                //if (loginInfo == null) return;
                //string b = loginInfo.Username;
                //var kh = _khDao.Get(lblIDKH.Text.Trim());

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    _rpClass.InsKhachHangXoaPo(kh.IDKHPO, kh.MAKVPO, txtGhiChu.Text.Trim(), b, ddlXAPHUONG.SelectedValue,
                            ddlAPKHOM.SelectedValue, kynay);

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = kh.IDKHPO,
                        IPAddress = "192.168.1.11",
                        MANV = b,
                        UserAgent = "192.168.1.11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "XBPO",
                        MOTA = "Xóa bộ khách hàng điện."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    BindKhachHangXoa();
                }

                upnlCustomers.Update();
                CloseWaitingDialog();

                Clear();
                BindKhachHangXoa();
                upnlThongTin.Update();

                ShowInfor("Xóa bộ thành công. Xin kiểm tra lại.");               
            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                int nam = int.Parse(txtNAM.Text.Trim());
                var kynay = new DateTime(nam, thang1, 1);

                //var list = _khxDao.GetListKV(ddlKHUVUC.SelectedValue);
                var list = _khxpoDao.GetListKVKy(ddlKHUVUC1.SelectedValue, kynay);

                gvKhachHang.DataSource = list;
                gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();
                gvKhachHang.Visible = true;

                CloseWaitingDialog();
                upnlCustomers.Update();

                divCR.Visible = false;
                upnlCrystalReport.Update();
            }
            catch { }
        }

        private void RestoreKH()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<KHACHHANGXOAPO>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _khxpoDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        var msg = _khxpoDao.DeleteList(objs);
                        if (msg != null)
                        {
                            switch (msg.MsgType)
                            {
                                case MessageType.Error:
                                    ShowError(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Info:
                                    ShowInfor(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Warning:
                                    ShowWarning(ResourceLabel.Get(msg));
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCaoXoaBo();
                //Clear();
                CloseWaitingDialog();
                gvKhachHang.Visible = false;
                upnlCustomers.Update();
            }
            catch { }
        }

        private void BaoCaoXoaBo()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV); 

                var ds = new ReportClass().DSKHXOABOPO(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue, 1);

                if (ds == null || ds.Tables.Count == 0) { CloseWaitingDialog(); return; }
                ReLoadBaoCao(ds.Tables[0]);

                CloseWaitingDialog();
            }
            catch { }
        }

        private void ReLoadBaoCao(DataTable dt)
        {
            try
            {
                if (dt == null)
                    return;

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

                rp = new ReportDocument();

                var path = Server.MapPath("/Reports/QuanLyKhachHang/DSKhachHangXoa.rpt");                

                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                txtXN.Text = "XN ĐIỆN NƯỚC " + ddlKHUVUC1.SelectedItem.ToString().ToUpper();
                TextObject txtTIEUDE = rp.ReportDefinition.ReportObjects["txtTIEUDE"] as TextObject;
                txtTIEUDE.Text = "DANH SÁCH KHÁCH HÀNG ĐIỆN XÓA BỘ";
                //txtTuNgay
                TextObject txtTuNgay = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay.Text = "Kỳ: " + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
                var d = DateTime.Now.Day;
                var m = DateTime.Now.Month;
                var y = DateTime.Now.Year;
                TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                ngay.Text = ddlKHUVUC1.SelectedItem + ", ngày " + d + " tháng " + m + " năm " + y;


                divCR.Visible = true;
                upnlCrystalReport.Update();

                reloadm.Text = "1";

                Session[SessionKey.KH_BAOCAOPO_XOABOKHPO] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;                

                CloseWaitingDialog();
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void ddlXAPHUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Ap khóm
                var makv = _kvpoDao.Get(ddlKHUVUC1.SelectedValue);
                var apkhom = _atDao.GetList(makv.MAKV, ddlXAPHUONG.SelectedValue);
                ddlAPKHOM.Items.Clear();
                //ddlAPKHOM.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var ak in apkhom)
                {
                    ddlAPKHOM.Items.Add(new ListItem(ak.TENAPTO, ak.MAAPTO));
                }
            }
            catch { }
        }

        private void Clear()
        {
            try
            {
                UpdateMode = Mode.Create;

                txtSODB.Text = "";
                lblIDKH.Text = "";
                reloadm.Text = "";
                lblTENKH.Text = "";
                lblTENDP.Text = "";
                lblTENKV.Text = "";
                lblMAMDSD.Text = "";
                lblCSMOI.Text = "";
                lblCSCU.Text = "";
                lblTIEUTHU.Text = "";
                lblTHANHTIEN.Text = "";
                lblTHUEGTGT.Text = "";
                lblTONGTIEN.Text = "";
                ddlXAPHUONG.SelectedIndex = 0;
                ddlAPKHOM.SelectedIndex = 0;

            }
            catch { }
        }

        protected void btExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string username = loginInfo.Username;
                string makvpo = _kvpoDao.GetPo(_nvDao.Get(username).MAKV).MAKVPO;

                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlTHANG1.Text.Trim()) + "/" + int.Parse(txtNAM1.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;

                dt = new ReportClass().DSKHXOABOPO(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue, 1).Tables[0];                               

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDH" + ddlTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);
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
                upnlThongTin.Update();    
            }
            catch { }
        }


    }
}