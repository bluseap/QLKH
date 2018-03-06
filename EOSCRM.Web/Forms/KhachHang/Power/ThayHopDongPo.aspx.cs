using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Globalization;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class ThayHopDongPo : Authentication
    {
        private readonly DotInHDDao _dihdDao = new DotInHDDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly MucDichSuDungPoDao _mdsdpoDao = new MucDichSuDungPoDao();        
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly ThayHopDongPoDao _thdpoDao = new ThayHopDongPoDao();
        
        private THAYHOPDONGPO ObjTHD
        {
            get
            {
                if (!ValidateData())
                    return null;

                var thd = (string.IsNullOrEmpty(lbIDTDH.Text.Trim()) || lbIDTDH.Text == "") ? new THAYHOPDONGPO() : _thdpoDao.Get(lbIDTDH.Text.Trim());
                if (thd == null)
                    return null;

                //IDTHD = _thdDao.NewId(),

                thd.IDKHPO = txtMADDK.Text.Trim();
                thd.MADDKPO = lbMADDK.Text.Trim();
                //SOHDMOI = _thdDao.NewId(),
                thd.SOHDCU = lbSOHDCU.Text.Trim();
                thd.TENKHMOI = txtTENKHMOI.Text.Trim();
                thd.TENKHCU = lbTENKHCU.Text.Trim();
                thd.MADPPO = lbDANHSO.Text.Trim().Substring(0, 4);
                thd.MADBPO = lbDANHSO.Text.Trim().Substring(4, 4);
                //thd.DIACHILD = txtDIACHILD.Text.Trim() + ", CHÂU THÀNH, AN GIANG";
                thd.UYQUYEN = txtUYQUYEN.Text.Trim();
                String namsinh = "11/" + "11/" + (!string.IsNullOrEmpty(txtNGAYSINH.Text.Trim()) ? txtNGAYSINH.Text.Trim() : "2099");
                thd.NGAYSINH = DateTimeUtil.GetVietNamDate(namsinh);
                thd.CAPNGAY = DateTimeUtil.GetVietNamDate(!string.IsNullOrEmpty(txtCAPNGAY.Text.Trim()) ? txtCAPNGAY.Text.Trim() : "11/11/2099");
                thd.TAI = txtTAIDAU.Text.Trim();
                thd.SONHA = txtSONHAMOI.Text.Trim();
                thd.CMND = txtCMND.Text.Trim();
                thd.MST = txtMST.Text.Trim();
                thd.DIENTHOAI = txtDIENTHOAI.Text.Trim();
                thd.NGAYKT = DateTimeUtil.GetVietNamDate(txtNGAYLAPHD.Text.Trim());
                thd.NGAYHL = DateTimeUtil.GetVietNamDate(txtNGAYHIEULUC.Text.Trim());
                thd.NGAYNHAP = DateTime.Now;
                thd.MAMDSD = lbMAMDSD.Text.Trim();
                thd.MANV = lbMANV.Text.Trim();
                thd.LYDO = txtLYDOTHAYHD.Text.Trim();
                thd.SOHOKHAU = txtSOHOKHAU.Text.Trim();

                return thd;
            }
        }

        #region loc, up
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

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_ThayHopDongPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                }

                if (reloadm.Text == "1")
                { BaoCaoInTHD(lbIDTDH.Text.Trim()); }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        public bool ValidateData()
        {
            if (!string.IsNullOrEmpty(txtCAPNGAY.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtCAPNGAY.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày cấp CMND "), txtCAPNGAY.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtNGAYLAPHD.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYLAPHD.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày lập HĐ "), txtNGAYLAPHD.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtNGAYHIEULUC.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYHIEULUC.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày hiệu lực HĐ "), txtNGAYHIEULUC.ClientID);
                    return false;
                }
            }

            return true;
        }        

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_THAYHOPDONGPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_THAYHOPDONGPO;
            }
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void LoadStaticReferences()
        {
            try
            {
                timkv();

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString();       

                txtNGAYLAPHD.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYHIEULUC.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYSINH.Text = "";
                txtCAPNGAY.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtLYDOTHAYHD.Text = "";
                reloadm.Text = "0";
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void Clear()
        {
            UpdateMode = Mode.Create;

            txtMADDK.Text = "";
            lbMADDK.Text = "";
            lbDANHSO.Text = "";
            lbIDTDH.Text = "";
            lbTENKHCU.Text = "";
            lbSOHDCU.Text = "";
            lbSOHDMOI.Text = "";
            lbTENMDSD.Text = "";
            lbMAMDSD.Text = "";
            lbMANV.Text = "";

            txtTENKHMOI.Text = "";
            txtUYQUYEN.Text = "";
            txtDIACHILD.Text = "";
            txtCMND.Text = "";
            txtTAIDAU.Text = "";
            txtSONHAMOI.Text = "";
            txtMST.Text = "";
            txtDIENTHOAI.Text = "";
            txtLYDOTHAYHD.Text = "";
            txtSOHOKHAU.Text = "";

            lbIDTDH.Text = "";
            
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
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUCMOI.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKV(_kvpoDao.GetPo(d).MAKVPO);
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUCMOI.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }        

        protected void btnKHACHHANG_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
            upnlKhachHang.Update();
        }

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
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            BindStatus(khachhang);
                            upnlInfor.Update();
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

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
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

        private void BindKhachHang()
        {
            var danhsach = _khpoDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUC.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        private void BindStatus(KHACHHANGPO kh)
        {
            var mdsd = _mdsdpoDao.Get(kh.MAMDSDPO);

            txtMADDK.Text = kh.IDKHPO.ToString();
            lbTENKHCU.Text = kh.TENKH.ToString();

            if (kh.SOHD != null)
            {
                lbSOHDCU.Text = kh.SOHD.ToString();
            }
            else { lbSOHDCU.Text = ""; }

            lbTENMDSD.Text = mdsd.TENMDSD.ToString();
            lbDANHSO.Text = (kh.MADPPO + kh.MADBPO).ToString();
            lbMAMDSD.Text = kh.MAMDSDPO.ToString();
            lbMANV.Text = LoginInfo.MANV.ToString();
            if (kh.MADDKPO != null)
            {
                lbMADDK.Text = kh.MADDKPO.ToString();
            }
            else { lbMADDK.Text = ""; }

            upnlInfor.Update();
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                string nam = txtNAM.Text.Trim();

                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var hientai1 = new DateTime(int.Parse(DateTime.Now.Year.ToString(CultureInfo.InvariantCulture)), DateTime.Now.Month, 1);

                if (hientai1.AddMonths(1) == kynay1 || hientai1 == kynay1)
                {
                    var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo == null) return;
                    string b = loginInfo.Username;
                    var query = _nvDao.GetKV(b);

                    var info = ObjTHD;
                    if (info == null)
                    {
                        CloseWaitingDialog();
                        return;
                    }                    

                    int mnam = DateTime.Now.Year;
                    int mthang = DateTime.Now.Month;

                    Message msg;
                    Filtered = FilteredMode.None;

                    var khachhangpo = _khpoDao.Get(info.IDKHPO);

                    //khoa so theo dot in hoa don
                    var dotin = _dihdDao.Get(khachhangpo.DOTINHD != null ? khachhangpo.DOTINHD : "");
                    bool p7d1 = _gcspoDao.IsLockDotInHD(kynay1, ddlKHUVUC.SelectedValue, dotin != null && dotin.MADOTIN != null ? dotin.MADOTIN : "");//phien 7 , kh muc dich khac, ngoai sinh hoat

                    if (p7d1 == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1 P7.");
                        return;
                    }  

                    var madpkh = _khpoDao.Get(txtMADDK.Text.Trim()).MADPPO;
                    // insert new
                    if (UpdateMode == Mode.Create)
                    {                        
                        //bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, _kvpoDao.GetPo(query.MAKV).MAKVPO.ToString());
                        bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay1, _kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), madpkh);
                        
                        if (dung == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ ghi chỉ số đường phố.");
                            return;
                        }

                        if (!HasPermission(Functions.KH_ThayHopDongPo, Permission.Insert))
                        {
                            CloseWaitingDialog();
                            ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                            return;
                        }

                        _rpDao.KhachHangHis(info.IDKHPO); // his khach hang 

                        info.IDTHDPO = _thdpoDao.NewId();
                        info.SOHDMOI = info.IDTHDPO;
                        info.DIACHILD = txtDIACHILD.Text.Trim() + ", " + ddlKHUVUCMOI.SelectedItem.ToString().ToUpper() + ", AN GIANG";
                        info.THANG = thang1;
                        info.NAM = Convert.ToInt32(nam);

                        _thdpoDao.InsertToKHTen(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV,
                            txtMADDK.Text.Trim(), txtTENKHMOI.Text.Trim(), thang1, Convert.ToInt32(nam), txtLYDOTHAYHD.Text.Trim());
                       
                        //_rpDao.UPKHTENTHDPO(txtMADDK.Text.Trim(), txtTENKHMOI.Text.Trim(), thang1, Convert.ToInt32(nam), 1, txtLYDOTHAYHD.Text.Trim());

                        msg = null;

                    }
                    // update
                    else
                    {
                        if (!HasPermission(Functions.KH_ThayHopDongPo, Permission.Update))
                        {
                            CloseWaitingDialog();
                            ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                            return;
                        }

                        bool dung2 = _gcspoDao.IsLockTinhCuocKy1(kynay1, _kvpoDao.GetPo(query.MAKV).MAKVPO.ToString(), madpkh);
                        if (dung2 == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ ghi chỉ số đường phố..");
                            return;
                        }

                        _rpDao.KhachHangHis(info.IDKHPO); // his khach hang 

                        info.DIACHILD = txtDIACHILD.Text.Trim() + ", " + ddlKHUVUCMOI.SelectedItem.ToString().ToUpper() + ", AN GIANG";
                        info.THANG = thang1;
                        info.NAM = Convert.ToInt32(nam);

                        //lbIDTDH; txtMADDK;
                        msg = _thdpoDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV,
                            lbIDTDH.Text.Trim(), txtMADDK.Text.Trim());

                        _rpDao.UPKHTENTHDPO(txtMADDK.Text.Trim(), txtTENKHMOI.Text.Trim(), thang1, Convert.ToInt32(nam), 0, txtLYDOTHAYHD.Text.Trim());

                    }
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Chọn kỳ cho đúng.");
                    return;
                }

                CloseWaitingDialog();

                Clear();
                BindDataForGrid();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            LoadStaticReferences();
            Clear();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = _thdpoDao.Get(id);
                        if (obj == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }
                        BindItem(obj);
                        UpdateMode = Mode.Update;
                        CloseWaitingDialog();
                        upnlInfor.Update();
                        break;

                    case "INLAIHD":
                        var objj = _thdpoDao.Get(id);
                        if (objj == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }
                        BaoCaoInTHD(id);
                        BindItem(objj);
                        gvList.Visible = true;
                        upnlGrid.Update();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BaoCaoInTHD(string idthd)
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

            DataTable dt = new ReportClass().InThayHopDongPo(idthd).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DonLapDatMoi/INHOPDONG.rpt");

            rp.Load(path);

            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            reloadm.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvList.PageIndex = e.NewPageIndex;                
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnIDTHD") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");

        }

        private void BindDataForGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = _nvDao.Get(loginInfo.Username).MAKV;

            if (Filtered == FilteredMode.None)
            {
                //var objList = _thdDao.GetList();
                var objList = _thdpoDao.GetListKV(_kvpoDao.GetPo(b).MAKVPO);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            else
            {
                //var objList = _thdDao.GetList();
                var objList = _thdpoDao.GetListKV(_kvpoDao.GetPo(b).MAKVPO);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
                /*
                DateTime? fromDate = null;
                DateTime? toDate = null;

                // ReSharper disable EmptyGeneralCatchClause
                try { fromDate = DateTime.Parse(txtFromDate.Text.Trim()); }
                catch { }
                try { toDate = DateTime.Parse(txtToDate.Text.Trim()); }
                catch { }
                // ReSharper restore EmptyGeneralCatchClause

                //var objList = _objDao.GetList(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, 
                //            txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                //            fromDate, toDate, txtTRANGTHAI.Text.Trim());

                var objList = _objDao.GetList2(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                           txtSONO.Text.Trim());
                
                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();*/
            }
        }

        private void BindItem(THAYHOPDONGPO obj)
        {
            if (obj == null)
                return;
            var mdsd = _mdsdpoDao.Get(obj.MAMDSD);

            lbIDTDH.Text = obj.IDTHDPO.ToString();

            txtMADDK.Text = obj.IDKHPO;
            lbMADDK.Text = obj.MADDKPO;
            lbDANHSO.Text = (obj.MADPPO + obj.MADBPO).ToString();
            lbMAMDSD.Text = obj.MAMDSD.ToString();
            lbTENKHCU.Text = obj.TENKHCU;
            lbSOHDCU.Text = obj.SOHDCU;
            if (obj.SOHDMOI != null)
            {
                lbSOHDMOI.Text = obj.SOHDMOI;
            }
            else { lbSOHDMOI.Text = ""; }
            if (mdsd.TENMDSD != null)
            { lbTENMDSD.Text = mdsd.TENMDSD; }
            else { lbTENMDSD.Text = ""; }
            txtTENKHMOI.Text = obj.TENKHMOI;
            if (obj.UYQUYEN != null)
            {
                txtUYQUYEN.Text = obj.UYQUYEN.ToString();
            }
            else { txtUYQUYEN.Text = ""; }
            if (obj.DIACHILD != null)
            { txtDIACHILD.Text = obj.DIACHILD.ToString(); }
            else { txtDIACHILD.Text = ""; }
            if (obj.NGAYSINH != null)
            { txtNGAYSINH.Text = obj.NGAYSINH.Value.ToString("yyyy"); }
            else { txtNGAYSINH.Text = ""; }
            if (obj.CMND != null)
            { txtCMND.Text = obj.CMND.ToString(); }
            else { txtCMND.Text = ""; }
            if (obj.CAPNGAY != null)
            { txtCAPNGAY.Text = obj.CAPNGAY.Value.ToString("dd/MM/yyyy"); }
            else { txtCAPNGAY.Text = ""; }
            if (obj.TAI != null)
            { txtTAIDAU.Text = obj.TAI.ToString(); }
            else { txtTAIDAU.Text = ""; }
            if (obj.SONHA != null)
            { txtSONHAMOI.Text = obj.SONHA.ToString(); }
            else { txtSONHAMOI.Text = ""; }
            if (obj.MST != null)
            { txtMST.Text = obj.MST.ToString(); }
            else { txtMST.Text = ""; }
            if (obj.DIENTHOAI != null)
            { txtDIENTHOAI.Text = obj.DIENTHOAI.ToString(); }
            else { txtDIENTHOAI.Text = ""; }
            if (obj.NGAYKT != null)
            { txtNGAYLAPHD.Text = obj.NGAYKT.Value.ToString("dd/MM/yyyy"); }
            else { txtNGAYLAPHD.Text = ""; }
            if (obj.NGAYHL != null)
            { txtNGAYHIEULUC.Text = obj.NGAYHL.Value.ToString("dd/MM/yyyy"); }
            else { txtNGAYHIEULUC.Text = ""; }
            if (obj.LYDO != null)
            { txtLYDOTHAYHD.Text = obj.LYDO.ToString(); }
            else { txtLYDOTHAYHD.Text = ""; }
            if (obj.SOHOKHAU != null)
            { txtSOHOKHAU.Text = obj.SOHOKHAU.ToString(); }
            else { txtSOHOKHAU.Text = ""; }

            ddlTHANG.SelectedIndex = Convert.ToInt16(obj.THANG) - 1;
            txtNAM.Text = obj.NAM.ToString();          

            upnlInfor.Update();
        }

    }
}