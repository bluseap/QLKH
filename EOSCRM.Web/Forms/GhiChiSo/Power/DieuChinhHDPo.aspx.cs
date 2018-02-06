using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.IO;
using System.Web.UI;
using System.Data;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo.Power
{
    public partial class DieuChinhHDPo : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();        
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly TieuThuPoDao _ttpoDao = new TieuThuPoDao();
        private readonly TieuThuDCPoDao _ttdcpoDao = new TieuThuDCPoDao();        
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass _rp = new ReportClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_DieuChinhHoaDonPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    //BindGrid();
                }
                else
                {
                    if (reloadm.Text == "1")
                    {
                        ReLoadBCDCKHSH();
                    }
                    if (reloadm.Text == "2")
                    {
                        ReLoadBCMUCDK();
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
            Page.Title = Resources.Message.TITLE_GCS_DIEUCHINHHOADONPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_DIEUCHINHHOADONPO;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvDieuChinhHD);
            //CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
        }

        private void LoadStaticReferences()
        {
            try
            {
                var kvList = _kvpoDao.GetList();

                timkv();                

                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);                

                txtCSMOI.Text = "0";
                txtCSCU.Text = "0";
                txtMTRUYTHU.Text = "0";
                txtSODINHMUC.Text = "1";

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

                /*var dotin = _diDao.GetListKVDDNotP7(_kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }*/

                if (_nvDao.Get(b).MAKV == "T" || _nvDao.Get(b).MAKV == "P" || _nvDao.Get(b).MAKV == "N" || _nvDao.Get(b).MAKV == "S" //tan chau,phutan,chau phu,chaudoc
                   || _nvDao.Get(b).MAKV == "K" || _nvDao.Get(b).MAKV == "L" || _nvDao.Get(b).MAKV == "M" //cho moi,tri ton,tinh bien,
                   || _nvDao.Get(b).MAKV == "Q" // an phu
                   )
                {
                    txtMASOHD.Text = "00";
                    //txtMASOHD.Enabled = false;
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

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
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
            bool dung = _gcspoDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);

            if (txtNAM.Text == Convert.ToString(nam) || txtNAM.Text == Convert.ToString(namIndex))
            {
                if (ddlTHANG.SelectedIndex == thangIndex)
                {
                    UnblockDialog("divKhachHang");                    
                    upnlKhachHang.Update();

                    //    BindKhachHang();
                    //    upnlKhachHang.Update();

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
                    //    ShowInfor("Đã khoá sổ. Không được điều chỉnh.");
                    //}
                }
                else
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowInfor("Chọn kỳ điều chỉnh cho đúng.");
                }
            }
            else
            {
                CloseWaitingDialog();
                HideDialog("divKhachHang");
                ShowInfor("Chọn năm điều chỉnh cho đúng.");
            }

        }

        private void BindStatus(KHACHHANGPO kh)
        {
            SetControlValue(txtSODB.ClientID, kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            SetLabel(lblIDKH.ClientID, kh.IDKHPO);
            lblIDKH.Text = kh.IDKHPO;
            SetLabel(lblTENDP.ClientID, kh.DUONGPHOPO != null ? kh.DUONGPHOPO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUCPO != null ? kh.KHUVUCPO.TENKV : "");
            SetLabel(lblMAMDSD.ClientID, kh.MAMDSDPO);
            var tieuthu = _ttpoDao.GetTN(kh.IDKHPO, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
            if (tieuthu != null)
            {
                SetLabel(lblCSMOI.ClientID, Convert.ToString(tieuthu.CHISOCUOI));
                SetLabel(lblCSCU.ClientID, Convert.ToString(tieuthu.CHISODAU));
                SetLabel(lblTIEUTHU.ClientID, Convert.ToString(tieuthu.KLTIEUTHU));
                SetLabel(lblTHANHTIEN.ClientID, Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENDIEN)));
                SetLabel(lblTHUEGTGT.ClientID, Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENTHUE)));
                SetLabel(lblTONGTIEN.ClientID, Convert.ToString(String.Format("{0:#.####}", tieuthu.TONGTIEN)));

                upnlThongTin.Update();
                CloseWaitingDialog();
                txtMASOHD.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không có tiêu thụ trong tháng này. Xin chọn khách hàng lại", txtSODB.ClientID);
            }
            upnlThongTin.Update();
        }

        private void BindStatusKHDC(KHACHHANGPO kh)
        {

            SetControlValue(txtSODB.ClientID, kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            SetLabel(lblIDKH.ClientID, kh.IDKHPO);
            lblIDKH.Text = kh.IDKHPO;
            SetLabel(lblTENDP.ClientID, kh.DUONGPHOPO != null ? kh.DUONGPHOPO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUCPO != null ? kh.KHUVUCPO.TENKV : "");
            SetLabel(lblMAMDSD.ClientID, kh.MAMDSDPO);

            var idmadt = _dppoDao.GetDP(kh.MADPPO);
            var dotin = ddlDOTGCS.Items.FindByValue(idmadt.IDMADOTIN);
            if (dotin != null)
                ddlDOTGCS.SelectedIndex = ddlDOTGCS.Items.IndexOf(dotin);

            upnlThongTin.Update();
        }

        private void BindDC(TIEUTHUDCPO dc)
        {
            SetLabel(lblCSMOI.ClientID, Convert.ToString(dc.CHISOCUOI));
            SetLabel(lblCSCU.ClientID, Convert.ToString(dc.CHISODAU));
            SetLabel(lblTIEUTHU.ClientID, Convert.ToString(dc.KLTIEUTHU));
            SetLabel(lblTHANHTIEN.ClientID, Convert.ToString(String.Format("{0:#.##}", dc.TIENDIEN)));
            SetLabel(lblTHUEGTGT.ClientID, Convert.ToString(String.Format("{0:#.##}", dc.TIENTHUE)));
            SetLabel(lblTONGTIEN.ClientID, Convert.ToString(String.Format("{0:#.####}", dc.TONGTIEN)));
            ddlTHANG.SelectedValue = dc.THANG.ToString();
            txtNAM.Text = dc.NAM.ToString();
            ddlKHUVUC1.SelectedValue = dc.MAKVPO;
            txtMASOHD.Text = dc.MASOHDDC != null ? dc.MASOHDDC : "";
            txtCSMOI.Text = dc.CHISOCUOIDC.ToString();
            txtCSCU.Text = dc.CHISODAUDC.ToString();
            txtGhiChu.Text = dc.GHICHUDC;

            if (dc.INHDDC == "0")
            { ckINHDDC.Checked = false; }
            else { ckINHDDC.Checked = true; }

            upnlThongTin.Update();
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
        #endregion

        protected void txtSODB_TextChanged(object sender, EventArgs e)
        {
            //var khachhang = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            var khachhang = _khpoDao.GetKhachHangFromMadb(lblIDKH.Text.Trim());
            var tieuthu = _ttpoDao.GetTN(khachhang.IDKHPO, ddlTHANG.SelectedIndex + 1, int.Parse(txtNAM.Text.Trim()));

            if (khachhang != null)
            {
                lblTENKH.Text = khachhang.TENKH;
                lblIDKH.Text = khachhang.IDKHPO;
                lblTENDP.Text = khachhang.DUONGPHOPO != null ? khachhang.DUONGPHOPO.TENDP : "";
                lblTENKV.Text = khachhang.KHUVUCPO != null ? khachhang.KHUVUCPO.TENKV : "";
                lblCSMOI.Text = Convert.ToString(tieuthu.CHISOCUOI);
                lblCSCU.Text = Convert.ToString(tieuthu.CHISODAU);
                lblTIEUTHU.Text = Convert.ToString(tieuthu.KLTIEUTHU);
                lblTHANHTIEN.Text = Convert.ToString(tieuthu.TIENDIEN);
                lblTHUEGTGT.Text = Convert.ToString(tieuthu.TIENTHUE);
                lblTONGTIEN.Text = Convert.ToString(tieuthu.TONGTIEN);

                CloseWaitingDialog();
                txtSODB.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
            }
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
                var kvpo = _kvpoDao.GetPo(query.MAKV);

                var kkh = _khpoDao.Get(lblIDKH.Text.Trim());

                //bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, kvpo.MAKVPO.ToString());
                bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay1, kvpo.MAKVPO.ToString(), kkh.MADPPO);
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ClearForm();
                    BindGrid();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                //khoa so theo dot in hoa don
                //var kkh = _khpoDao.Get(lblIDKH.Text.Trim());
                bool p7d1 = _gcspoDao.IsLockDotInHD(kynay1, kvpo.MAKVPO.ToString(), "DDP7D1");//phien 7 , kh muc dich khac, ngoai sinh hoat
                if (kkh.MAMDSDPO != "A" && kkh.MAMDSDPO != "B" && kkh.MAMDSDPO != "G" && kkh.MAMDSDPO != "Z")
                {
                    if (p7d1 == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1. Không được điều chỉnh");
                        return;
                    }
                }

                if (!HasPermission(Functions.GCS_DieuChinhHoaDonPo, Permission.Insert))
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
                            string inhddc;
                            if (ckINHDDC.Checked == true)
                            { inhddc = "1"; }
                            else { inhddc = "0"; }

                            _rp.ThemTieuThuDCPo(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                            var msg = _gcspoDao.TinhTienDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                            var msg1 = _gcspoDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                            ShowInfor(ResourceLabel.Get(msg));
                            ShowInfor(ResourceLabel.Get(msg1));

                            CloseWaitingDialog();
                            ClearForm();
                            BindGrid();
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
                var kvpo = _kvpoDao.GetPo(query.MAKV);

                var kkh = _khpoDao.Get(lblIDKH.Text.Trim());

                //bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, kvpo.MAKVPO.ToString());
                //bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay1, kvpo.MAKVPO.ToString(), kkh.MADPPO);
                //if (dung == true)
                //{
                //    CloseWaitingDialog();
                //    ClearForm();
                //    BindGrid();
                //    ShowInfor("Đã khoá sổ ghi chỉ số.");
                //    return;
                //}

                //khoa so theo dot in hoa don
                //var kkh = _khpoDao.Get(lblIDKH.Text.Trim());
                bool p7d1 = _gcspoDao.IsLockDotInHD(kynay1, kvpo.MAKVPO.ToString(), "DDP7D1");//phien 7 , kh muc dich khac, ngoai sinh hoat
                if (kkh.MAMDSDPO != "A" && kkh.MAMDSDPO != "B" && kkh.MAMDSDPO != "G" && kkh.MAMDSDPO != "Z")
                {
                    if (p7d1 == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1. Không được điều chỉnh");
                        return;
                    }
                }

                if (!HasPermission(Functions.GCS_DieuChinhHoaDonPo, Permission.Update))
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
                            string inhddc;
                            if (ckINHDDC.Checked == true)
                            { inhddc = "1"; }
                            else { inhddc = "0"; }

                            _rp.UpTieuThuDCPo(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                            var msg = _gcspoDao.TinhTienDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                            var msg1 = _gcspoDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                            ShowInfor(ResourceLabel.Get(msg));
                            ShowInfor(ResourceLabel.Get(msg1));

                            CloseWaitingDialog();
                            ClearForm();
                            BindGrid();
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

        private void ClearForm()
        {
            txtMASOHD.Text = "00";
            txtGhiChu.Text = "";
            txtCSCU.Text = "0";
            txtCSMOI.Text = "0";
            txtMTRUYTHU.Text = "0";
            txtSODINHMUC.Text = "1";

            btnSaveUp.Visible = false;
            btnSave.Visible = true;
        }

        private bool IsDataValid()
        {
            if (string.Empty.Equals(txtMASOHD.Text.Trim()))
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
            }
            return true;
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
                        //var dc = _ttdcDao.Get(id);
                        var dc = _ttdcpoDao.GetTN(id, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                        SetLabel(lblIDKH.ClientID, dc.IDKHPO);
                        lblIDKH.Text = dc.IDKHPO;

                        var khachhang = _khpoDao.Get(id);

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
                var list = _ttdcpoDao.GetDC1(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                gvDieuChinhHD.DataSource = list;
                gvDieuChinhHD.PagerInforText = list.Count.ToString();
                gvDieuChinhHD.DataBind();
            }
            else
            {
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var list = _ttdcpoDao.GetDC(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                    gvDieuChinhHD.DataSource = list;
                    gvDieuChinhHD.PagerInforText = list.Count.ToString();
                    gvDieuChinhHD.DataBind();
                }
                else
                {
                    var list = _ttdcpoDao.GetDCDotIn(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                    ddlDOTGCS.SelectedValue);
                    gvDieuChinhHD.DataSource = list;
                    gvDieuChinhHD.PagerInforText = list.Count.ToString();
                    gvDieuChinhHD.DataBind();
                }
            }

            upnlTTDC.Update();
            CloseWaitingDialog();
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
                    ddlKHUVUC1.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                    btnNGUYENDC.Visible = true;
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            ClearForm();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            /*var str_madp = "";

            for (var i = 0; i < dpDataList.Items.Count; i++)
            {
                var cb = dpDataList.Items[i].FindControl("chkDuongPho") as HtmlInputCheckBox;
                if (cb == null || !cb.Checked) continue;

                var madp = cb.Attributes["title"].Trim();
                var duongphu = cb.Attributes["lang"].Trim();

                if (duongphu.Length > 0)
                {
                    str_madp += " (DP.MADP= '" + madp + "' and DP.DUONGPHU = '" + duongphu + "') OR";
                }
                else
                {
                    str_madp += " (DP.MADP= '" + madp + "') OR";
                }
            }

            str_madp = "(" + str_madp + ") and";
            str_madp = (str_madp == "() and") ?
                "" :
                str_madp.Replace("OR) and", ")  ");*/

            LayBaoCao();

        }

        private void LayBaoCao()
        {
            try
            {
                BCDCKHSH();
                CloseWaitingDialog();
            }
            catch { }
        }

        private void BCDCKHSH()
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
                    catch { }
                }

                DataTable dt;

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    dt = _rp.BienKHPo("", ddlKHUVUC1.SelectedValue, "", "", int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSDCHDSHPO").Tables[0];
                }
                else
                {
                    dt = _rp.BienKHPo(ddlDOTGCS.SelectedValue, ddlKHUVUC1.SelectedValue, "", "", int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                        "DSDCHDSHPODOTIN").Tables[0];
                }

                rp = new ReportDocument();
                var path = Server.MapPath("~/Reports/QuanLyGhiDHTinhCuocInHD/BKDCSHPO.rpt");

                rp.Load(path);
                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                var tendotin = ddlDOTGCS.SelectedValue == "%" ? "" : " (" 
                        + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")"; 

                TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendotin;
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

        private void ReLoadBCDCKHSH()
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
                    catch { }
                }

                DataTable dt;

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    dt = _rp.BienKHPo("", ddlKHUVUC1.SelectedValue, "", "", int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSDCHDSHPO").Tables[0];
                }
                else
                {
                    dt = _rp.BienKHPo(ddlDOTGCS.SelectedValue, ddlKHUVUC1.SelectedValue, "", "", int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                        "DSDCHDSHPODOTIN").Tables[0];
                }

                rp = new ReportDocument();
                var path = Server.MapPath("~/Reports/QuanLyGhiDHTinhCuocInHD/BKDCSHPO.rpt");

                rp.Load(path);
                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                var tendotin = ddlDOTGCS.SelectedValue == "%" ? "" : " ("
                    + _dmdiDao.Get(_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN).TENDOTIN + ")"; 

                TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim() + tendotin;
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

                //reloadm.Text = "1";

                Session["DS_DonDangKy"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                CloseWaitingDialog();
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnNGUYENDC_Click(object sender, EventArgs e)
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
            bool dung = _gcspoDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);

            UnblockDialog("divKhachHang");
            BindKhachHang();
            upnlKhachHang.Update();
        }

        protected void btBCDCMUCDK_Click(object sender, EventArgs e)
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
                    catch { }
                }

                DataTable dt = _rp.BienKHPo("", ddlKHUVUC1.SelectedValue, "", "",
                                    int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSDCHDMUCDKPO").Tables[0];

                rp = new ReportDocument();
                var path = Server.MapPath("~/Reports/QuanLyGhiDHTinhCuocInHD/BKDCSHPO.rpt");

                rp.Load(path);
                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();


                TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim();
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

                reloadm.Text = "2";

                Session["DS_DonDangKy"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                CloseWaitingDialog();
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void ReLoadBCMUCDK()
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
                    catch { }
                }

                DataTable dt = _rp.BienKHPo("", ddlKHUVUC1.SelectedValue, "", "",
                                    int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), "DSDCHDMUCDKPO").Tables[0];

                rp = new ReportDocument();
                var path = Server.MapPath("~/Reports/QuanLyGhiDHTinhCuocInHD/BKDCSHPO.rpt");

                rp.Load(path);
                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();


                TextObject txtTuNgay1 = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay1.Text = "KỲ " + ddlTHANG.SelectedValue + " NĂM " + txtNAM.Text.Trim();
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

                //reloadm.Text = "2";

                Session["DS_DonDangKy"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                CloseWaitingDialog();
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void btExcelDieuChinhPo_Click(object sender, EventArgs e)
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
                    //dt = new ReportClass().BKDieuChinh(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString()).Tables[0];
                    dt = new ReportClass().BKDieuChinhDotIn(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString(),
                                ddlDOTGCS.SelectedValue, "", "DSDCDOTINALLPO").Tables[0];
                    CloseWaitingDialog();
                }
                else
                {
                    //var idotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, cboKhuVuc.SelectedValue);
                    dt = new ReportClass().BKDieuChinhDotIn(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString(),
                                ddlDOTGCS.SelectedValue, "", "DSDCDOTINPO").Tables[0];
                    CloseWaitingDialog();
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

    }
}