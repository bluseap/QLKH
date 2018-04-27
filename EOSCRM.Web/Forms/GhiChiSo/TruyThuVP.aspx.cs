using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.IO;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class TruyThuVP : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly TieuThuTTVPDao _ttttvpDao = new TieuThuTTVPDao();
        private readonly ReportClass _rp = new ReportClass();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }
        private void UnblockWaitingDialog()
        {
            ((EOS)Page.Master).UnblockWaitingDialog();
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
                Authenticate(Functions.GCS_TruyThuVP, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    //BindGrid();
                }

                if (reloadm.Text == "1")
                {
                    BaoCaoTTVP(); 
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_TRUYTHUVP;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_TRUYTHUVP;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvDieuChinhHD);
        }

        private void LoadStaticReferences()
        {
            try
            {
                var kvList = _kvDao.GetList();

                timkv();               

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                var listMucDichSuDung = new MucDichSuDungDao().GetList();
                ddlMDSD.DataSource = listMucDichSuDung;
                ddlMDSD.DataTextField = "TENMDSD";
                ddlMDSD.DataValueField = "MAMDSD";
                ddlMDSD.DataBind();

                txtCSMOI.Text = "0";
                txtCSCU.Text = "0";
                txtMTRUYTHU.Text = "0";
                txtSODINHMUC.Text = "1";

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
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
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
                    ddlKHUVUC1.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                    btnNGUYENDC.Visible = true;
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }        

        protected void btnBrowseKH_Click(object sender, EventArgs e)
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
            int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            bool dung = gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);

            if (txtNAM.Text == Convert.ToString(nam) && Int16.Parse(ddlTHANG.SelectedValue) == thang1)
            {
                UnblockDialog("divKhachHang");
                BindKhachHang();
                upnlKhachHang.Update();

                //if (dung == false)
                //{
                //    UnblockDialog("divKhachHang");
                //    BindKhachHang();
                //    upnlKhachHang.Update();
                //}
                //else
                //{
                //    CloseWaitingDialog();
                //    HideDialog("divKhachHang");
                //    ShowInfor("Đã khoá sổ. Không được truy thu vi phạm.");
                //}

                /*UnblockDialog("divKhachHang");
                BindKhachHang();
                upnlKhachHang.Update();*/
            }
            else
            {
                CloseWaitingDialog();
                HideDialog("divKhachHang");
                ShowInfor("Chọn kỳ truy thu vi phạm cho đúng.");
            }
        }

        private void BindKhachHang()
        {
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
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

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        //var khachhang = _khDao.GetKhachHangFromMadb(id);
                        var khachhang = _khDao.Get(id);
                        if (khachhang != null)
                        {
                            BindStatus(khachhang);                            
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            txtMASOHD.Focus();
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

        private void BindStatus(KHACHHANG kh)
        {
            txtSODB.Text = (kh.MADP + kh.DUONGPHU + kh.MADB).ToString();
            lblTENKH.Text = kh.TENKH.ToString();
            lblIDKH.Text = kh.IDKH.ToString();
            lblIDKH.Text = kh.IDKH.ToString();
            lblTENDP.Text = kh.DUONGPHO != null ? kh.DUONGPHO.TENDP.ToString() : "";
            lblTENKV.Text = kh.KHUVUC != null ? kh.KHUVUC.TENKV.ToString() : "";
            lblMAMDSD.Text = kh.MAMDSD.ToString();
            var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
            if (tieuthu != null)
            {
                lblCSMOI.Text = Convert.ToString(tieuthu.CHISOCUOI);
                lblCSCU.Text = Convert.ToString(tieuthu.CHISODAU);
                lblTIEUTHU.Text = Convert.ToString(tieuthu.KLTIEUTHU);
                lblTHANHTIEN.Text = Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENNUOC));
                lblTHUEGTGT.Text = Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENTHUE));
                lblTONGTIEN.Text = Convert.ToString(String.Format("{0:#.####}", tieuthu.TONGTIEN));

                upnlThongTin.Update();
                CloseWaitingDialog();
                txtMASOHD.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không có tiêu thụ trong tháng này. Xin chọn khách hàng lại", txtSODB.ClientID);
            }

            ddlMDSD.SelectedValue = kh.MAMDSD.ToString();

            upnlThongTin.Update();
        }

        private void ClearForm()
        {
            txtMASOHD.Text = "";
            txtGhiChu.Text = "";
            txtCSCU.Text = "0";
            txtCSMOI.Text = "0";
            txtMTRUYTHU.Text = "0";
            txtSODINHMUC.Text = "1";

            btnSaveUp.Visible = false;
            btnSave.Visible = true;

            ckTINH1GIA.Checked = false;
            //ddlMDSD.Visible = false;
        }

        private bool IsDataValid()
        {
            /*if (string.Empty.Equals(txtMASOHD.Text.Trim()))
            {
                ShowError("Mã số hoá đơn không được rỗng.", txtMASOHD.ClientID);
                return false;
            }
            if (!string.Empty.Equals(txtCSMOI.Text.Trim()))
            {
                try
                {
                    int.Parse(txtCSMOI.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số mới phải là số.", txtCSMOI.ClientID);
                    return false;
                }
            }
            if (!string.Empty.Equals(txtCSCU.Text.Trim()))
            {
                try
                {
                    int.Parse(txtCSCU.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số cũ phải là số.", txtCSCU.ClientID);
                    return false;
                }
            }*/
            if (!string.Empty.Equals(txtSODINHMUC.Text.Trim()))
            {
                try
                {
                    int.Parse(txtSODINHMUC.Text.Trim());
                }
                catch
                {
                    ShowError("Định mức phải là số.", txtSODINHMUC.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtMTRUYTHU.Text.Trim()))
            {
                try
                {
                    int.Parse(txtMTRUYTHU.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số truy thu phải là số.", txtMTRUYTHU.ClientID);
                    return false;
                }
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                //bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).MADP);
                bool dung = false;

                if (dung == true)
                {
                    CloseWaitingDialog();
                    ClearForm();
                    BindGrid();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                if (!HasPermission(Functions.GCS_TruyThuVP, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                else
                {
                    if (!IsDataValid())
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (ckTINH1GIA.Checked == true)
                            {
                                string inhddc;
                                if (ckINHDDC.Checked == true)
                                { inhddc = "1"; }
                                else { inhddc = "0"; }

                                _rp.ThemTieuThuTTVP(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá. " + txtGhiChu.Text.Trim(),
                                    ddlTRUYTHUVP.SelectedValue, inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()), "INTTTTVPN");

                                var msg = gcsDao.TinhTienDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                //var msg1 = gcsDao.TinhTienTTDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                ShowInfor(ResourceLabel.Get(msg));
                                //ShowInfor(ResourceLabel.Get(msg1));

                                CloseWaitingDialog();
                                ClearForm();
                                BindGrid();
                            }
                            else
                            {
                                string inhddc;
                                if (ckINHDDC.Checked == true)
                                { inhddc = "1"; }
                                else { inhddc = "0"; }

                                _rp.ThemTieuThuTTVP(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), ddlTRUYTHUVP.SelectedValue,
                                    inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()), "INTTTTVPN");

                                if (ddlTRUYTHUVP.SelectedValue == "CHETN")
                                {
                                    var msg = gcsDao.TinhTienTTVP(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    ShowInfor(ResourceLabel.Get(msg));
                                    //ShowInfor(ResourceLabel.Get(msg1));
                                }

                                if (ddlTRUYTHUVP.SelectedValue == "VIPHAMN") // tinh tien bac thang cao nhat
                                {
                                    var msg = gcsDao.TinhTienTTVPSD(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));                                    
                                    ShowInfor(ResourceLabel.Get(msg));                                    
                                }

                                CloseWaitingDialog();
                                ClearForm();
                                BindGrid();
                            }
                        }
                        catch
                        {
                            CloseWaitingDialog();
                            ShowError("Khách hàng đã truy thu vi phạm rồi.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnSaveUp_Click(object sender, EventArgs e)
        {
            try
            {
                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                bool dung = false;

                if (dung == true)
                {
                    CloseWaitingDialog();
                    ClearForm();
                    BindGrid();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                if (!HasPermission(Functions.GCS_TruyThuVP, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                else
                {
                    if (!IsDataValid())
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    else
                    {
                        try
                        {
                            if (ckTINH1GIA.Checked == true)
                            {
                                string inhddc;
                                if (ckINHDDC.Checked == true)
                                { inhddc = "1"; }
                                else { inhddc = "0"; }

                                _rp.ThemTieuThuTTVP(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                   int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá. " + txtGhiChu.Text.Trim(),
                                   ddlTRUYTHUVP.SelectedValue, inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()), "UPTTTTVPN");

                                var msg = gcsDao.TinhTienTTVP1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                //var msg1 = gcsDao.TinhTienTTDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                ShowInfor(ResourceLabel.Get(msg));
                                //ShowInfor(ResourceLabel.Get(msg1));

                                CloseWaitingDialog();
                                ClearForm();
                                BindGrid();
                            }
                            else
                            {
                                string inhddc;
                                if (ckINHDDC.Checked == true)
                                { inhddc = "1"; }
                                else { inhddc = "0"; }

                                _rp.ThemTieuThuTTVP(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), ddlTRUYTHUVP.SelectedValue,
                                    inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()), "UPTTTTVPN");

                                if (ddlTRUYTHUVP.SelectedValue == "CHETN")
                                {
                                    var msg = gcsDao.TinhTienTTVP(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    ShowInfor(ResourceLabel.Get(msg));
                                    //ShowInfor(ResourceLabel.Get(msg1));
                                }

                                if (ddlTRUYTHUVP.SelectedValue == "VIPHAMN") // tinh tien bac thang cao nhat
                                {
                                    var msg = gcsDao.TinhTienTTVPSD(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    ShowInfor(ResourceLabel.Get(msg));
                                }

                                CloseWaitingDialog();
                                ClearForm();
                                BindGrid();
                            }
                        }
                        catch
                        {
                            CloseWaitingDialog();
                            ShowError("Hoá đơn này đã điều chỉnh rồi.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            ClearForm();
        }        

        protected void gvDieuChinhHD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDieuChinhHD.PageIndex = e.NewPageIndex;               
                BindGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDieuChinhHD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectDC":

                        var dc = _ttttvpDao.GetTN(id, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                        SetLabel(lblIDKH.ClientID, dc.IDKH);
                        lblIDKH.Text = dc.IDKH;

                        var khachhang = _khDao.Get(id);

                        if (khachhang != null)
                        {
                            BindStatusKHDC(khachhang);
                            BindDC(dc);
                        }

                        btnSaveUp.Visible = true;
                        btnSave.Visible = false;

                        CloseWaitingDialog();
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDieuChinhHD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        private void BindGrid()
        {
            if (ddlKHUVUC1.SelectedValue == "99")
            {
                var list = _ttttvpDao.GetDC1(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                gvDieuChinhHD.DataSource = list;
                gvDieuChinhHD.PagerInforText = list.Count.ToString();
                gvDieuChinhHD.DataBind();
            }
            else
            {
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var list = _ttttvpDao.GetDC(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                    gvDieuChinhHD.DataSource = list;
                    gvDieuChinhHD.PagerInforText = list.Count.ToString();
                    gvDieuChinhHD.DataBind();                    
                }
                else
                {
                    //var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, ddlKHUVUC1.SelectedValue);
                    var list = _ttttvpDao.GetDCDotIn(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        ddlDOTGCS.SelectedValue);
                    gvDieuChinhHD.DataSource = list;
                    gvDieuChinhHD.PagerInforText = list.Count.ToString();
                    gvDieuChinhHD.DataBind();
                }
            }

            upnlTTDC.Update();
            CloseWaitingDialog();
        }

        private void BindStatusKHDC(KHACHHANG kh)
        {
            txtSODB.Text = (kh.MADP + kh.DUONGPHU + kh.MADB).ToString();
            lblTENKH.Text = kh.TENKH.ToString();
            lblIDKH.Text = kh.IDKH.ToString();
            lblIDKH.Text = kh.IDKH;
            lblTENDP.Text = kh.DUONGPHO != null ? kh.DUONGPHO.TENDP.ToString() : "";
            lblTENKV.Text = kh.KHUVUC != null ? kh.KHUVUC.TENKV.ToString() : "";
            lblMAMDSD.Text = kh.MAMDSD.ToString();

            var idmadt = _dpDao.GetDP(kh.MADP);
            var dotin = ddlDOTGCS.Items.FindByValue(idmadt.IDMADOTIN);
            if (dotin != null)
                ddlDOTGCS.SelectedIndex = ddlDOTGCS.Items.IndexOf(dotin);

            upnlThongTin.Update();
        }

        private void BindDC(TIEUTHUTTVP dc)
        {
            if (dc.MAMDSDVP1GIA != null)
            {
                lblCSMOI.Text = Convert.ToString(dc.CHISOCUOI);
                lblCSCU.Text = Convert.ToString(dc.CHISODAU);
                lblTIEUTHU.Text = Convert.ToString(dc.KLTIEUTHU);
                lblTHANHTIEN.Text = Convert.ToString(String.Format("{0:#.##}", dc.TIENNUOC));
                lblTHUEGTGT.Text = Convert.ToString(String.Format("{0:#.##}", dc.TIENTHUE));
                lblTONGTIEN.Text = Convert.ToString(String.Format("{0:#.####}", dc.TONGTIEN));
                ddlTHANG.SelectedValue = dc.THANG.ToString();
                txtNAM.Text = dc.NAM.ToString();
                ddlKHUVUC1.SelectedValue = dc.MAKV;
                txtMASOHD.Text = dc.MASOHDVP;
                txtCSMOI.Text = dc.CHISOCUOIVP.ToString();
                txtCSCU.Text = dc.CHISODAUVP.ToString();
                txtGhiChu.Text = dc.GHICHUVP;
                txtMTRUYTHU.Text = dc.MTRUYTHUVP.ToString();
                if (dc.INHDVP == "0")
                { ckINHDDC.Checked = false; }
                else { ckINHDDC.Checked = true; }

                ckTINH1GIA.Checked = true;
                ddlMDSD.Visible = true;
                ddlMDSD.SelectedValue = dc.MAMDSDVP1GIA.ToString();

                ddlTRUYTHUVP.SelectedValue = dc.MATTVP.ToString();
                txtSODINHMUC.Text = Convert.ToInt32(dc.DMNK).ToString();

                upnlThongTin.Update();
            }
            else
            {
                lblCSMOI.Text = Convert.ToString(dc.CHISOCUOI);
                lblCSCU.Text = Convert.ToString(dc.CHISODAU);
                lblTIEUTHU.Text = Convert.ToString(dc.KLTIEUTHU);
                lblTHANHTIEN.Text = Convert.ToString(String.Format("{0:#.##}", dc.TIENNUOC));
                lblTHUEGTGT.Text = Convert.ToString(String.Format("{0:#.##}", dc.TIENTHUE));
                lblTONGTIEN.Text = Convert.ToString(String.Format("{0:#.####}", dc.TONGTIEN));
                ddlTHANG.SelectedValue = dc.THANG.ToString();
                txtNAM.Text = dc.NAM.ToString();
                ddlKHUVUC1.SelectedValue = dc.MAKV;
                txtMASOHD.Text = dc.MASOHDVP;
                txtCSMOI.Text = dc.CHISOCUOIVP.ToString();
                txtCSCU.Text = dc.CHISODAUVP.ToString();
                txtGhiChu.Text = dc.GHICHUVP;
                txtMTRUYTHU.Text = dc.MTRUYTHUVP.ToString();
                if (dc.INHDVP == "0")
                { ckINHDDC.Checked = false; }
                else { ckINHDDC.Checked = true; }

                ddlTRUYTHUVP.SelectedValue = dc.MATTVP.ToString();
                txtSODINHMUC.Text = Convert.ToInt32(dc.DMNK).ToString();

                upnlThongTin.Update();
            }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCaoTTVP();               
                CloseWaitingDialog();
            }
            catch { }
        }

        private void BaoCaoTTVP()
        {
            try
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

                /*var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;*/

                //var dt = _rp.BKDieuChinh(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), cboKhuVuc.SelectedValue.ToString()).Tables[0];
                //DataTable dt = _rp.ThemTieuThuTTVP(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                //                    int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), 
                //                    ddlTRUYTHUVP.SelectedValue, "", 
                //                    int.Parse(txtMTRUYTHU.Text.Trim()),
                //                    int.Parse(txtSODINHMUC.Text.Trim()), "DSVPTTVPN").Tables[0];

                DataTable dt;

                if(ddlDOTGCS.SelectedValue == "%")
                {
                    dt = _rp.ThemTieuThuTTVP(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), ddlTRUYTHUVP.SelectedValue, "", 
                                    int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()), "DSVPTTVPN").Tables[0];
                }
                else
                {
                    //var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, ddlKHUVUC1.SelectedValue);
                    dt = _rp.ThemTieuThuTTVP(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), ddlDOTGCS.SelectedValue,
                                    ddlTRUYTHUVP.SelectedValue, "", int.Parse(txtMTRUYTHU.Text.Trim()),
                                    int.Parse(txtSODINHMUC.Text.Trim()), "DSVPTTVPNDOTIN").Tables[0];
                }
               
                
                rp = new ReportDocument();
                var path = Server.MapPath("../../Reports/QuanLyGhiDHTinhCuocInHD/VIPHAMTTN.rpt");                

                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                string tendot = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                    + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")";

                TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendot;
                TextObject xn1 = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                xn1.Text = "XN ĐIỆN NƯỚC " + ddlKHUVUC1.SelectedItem.ToString().ToUpper();
                //TextObject txtNguoiLap1 = rp.ReportDefinition.ReportObjects["txtNguoiLap"] as TextObject;
                //txtNguoiLap1.Text = txtNguoiLap.Text;
                var d = DateTime.Now.Day;
                var m = DateTime.Now.Month;
                var y = DateTime.Now.Year;

                TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                ngay.Text = ddlKHUVUC1.SelectedItem + ", ngày " + d + " tháng " +
                        m + " năm " + y;


                divCR.Visible = true;                

                reloadm.Text = "1";

                Session["DS_DonDangKy"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                CloseWaitingDialog();
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void btXuatExcel_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    dt = _rp.ThemTieuThuTTVP(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), ddlTRUYTHUVP.SelectedValue, "",
                                    int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()), "DSVPTTVPN").Tables[0];
                }
                else
                {
                    //var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, ddlKHUVUC1.SelectedValue);
                    dt = _rp.ThemTieuThuTTVP(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                    int.Parse(txtCSMOI.Text.Trim()), ddlDOTGCS.SelectedValue,
                                    ddlTRUYTHUVP.SelectedValue, "", int.Parse(txtMTRUYTHU.Text.Trim()),
                                    int.Parse(txtSODINHMUC.Text.Trim()), "DSVPTTVPNDOTIN").Tables[0];
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DC" + ddlTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".xls");
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
                //Response.Write(style);
                //string style = @"<style> TD { mso-number-format:\@; } </style>";
                Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                upnlThongTin.Update();
            }
            catch { }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnNGUYENDC_Click(object sender, EventArgs e)
        {

        }

        protected void gvDieuChinhHD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void txtCSMOI_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtGhiChu_TextChanged(object sender, EventArgs e)
        {

        }

        

    }
}