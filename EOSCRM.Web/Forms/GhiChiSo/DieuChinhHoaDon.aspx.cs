using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using System.IO;
using System.Web.UI;
using System.Data;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class DieuChinhHoaDon : Authentication
    {
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly TieuThuDCDao _ttdcDao = new TieuThuDCDao();
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
                Authenticate(Functions.GCS_DieuChinhHoaDon, Permission.Read);
                PrepareUI();
                
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    //BindGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_DIEUCHINHHOADON;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_DIEUCHINHHOADON;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvDieuChinhHD);
            //CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
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

                SetChiSoTien();

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

                if (_nvDao.Get(b).MAKV == "T" || _nvDao.Get(b).MAKV == "P" || _nvDao.Get(b).MAKV == "N" || _nvDao.Get(b).MAKV == "S" //tan chau,phutan,chau phu,chaudoc
                    || _nvDao.Get(b).MAKV == "K" ||  _nvDao.Get(b).MAKV == "L" || _nvDao.Get(b).MAKV == "M" //cho moi,tri ton,tinh bien,
                    ||  _nvDao.Get(b).MAKV == "Q" // an phu
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

        private void SetChiSoTien()
        {
            try
            {
                txtCSMOI.Text = "0";
                txtCSCU.Text = "0";
                txtMTRUYTHU.Text = "0";
                txtSODINHMUC.Text = "1";

                txtKhoiLuongNuoc.Text = "0";
                txtTienNuoc.Text = "0"; 
                txtTienThueGTGT.Text = "0"; 
                txtTienThueMoiTruong.Text = "0"; 
                txtTongTienNuoc.Text = "0";

                upnlThongTin.Update();
            }
            catch { }
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

        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
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
            bool dung = gcsDao.IsLockTinhCuocKy(kynay,ddlKHUVUC1.SelectedValue);

            if (txtNAM.Text == Convert.ToString(nam) || txtNAM.Text == Convert.ToString(namIndex))
            {
                if (ddlTHANG.SelectedIndex == thangIndex)
                {
                    UnblockDialog("divKhachHang");                   
                    upnlKhachHang.Update();

                    //if (dung == false)
                    //{
                    //    UnblockDialog("divKhachHang");
                    //    //BindKhachHang();
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
                lblTHANHTIEN.Text = Convert.ToString(String.Format("{0:#.##}",tieuthu.TIENNUOC));
                lblTHUEGTGT.Text = Convert.ToString(String.Format("{0:#.##}",tieuthu.TIENTHUE));
                lblTONGTIEN.Text = Convert.ToString(String.Format("{0:#.####}",tieuthu.TONGTIEN));

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

        private void BindDC(TIEUTHUDC dc)
        {
            if (dc.MAMDSD1GIA != null)
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
                txtMASOHD.Text = dc.MASOHDDC;

                txtCSMOI.Text = dc.CHISOCUOIDC.ToString();
                txtCSCU.Text = dc.CHISODAUDC.ToString();
                txtGhiChu.Text = dc.GHICHUDC;
                txtMTRUYTHU.Text = dc.MTRUYTHUDC.ToString();
                txtSODINHMUC.Text = dc.SODINHMUC.ToString();

                txtKhoiLuongNuoc.Text = dc.KLTIEUTHU != null ?  dc.KLTIEUTHU.ToString() : "0";
                txtTienNuoc.Text = dc.TIENNUOC != null ? dc.TIENNUOC.ToString() : "0";
                txtTienThueGTGT.Text = dc.TIENTHUE != null ? dc.TIENTHUE.ToString() : "0";
                txtTienThueMoiTruong.Text = dc.TIENPHI != null ? dc.TIENPHI.ToString() : "0";
                txtTongTienNuoc.Text = dc.TONGTIEN != null ? dc.TONGTIEN.ToString() : "0";

                if (dc.INHDDC == "0")
                { ckINHDDC.Checked = false; }
                else { ckINHDDC.Checked = true; }

                ckTINH1GIA.Checked = true;
                ddlMDSD.Visible = true;
                ddlMDSD.SelectedValue = dc.MAMDSD1GIA.ToString();
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
                txtMASOHD.Text = dc.MASOHDDC;
                txtCSMOI.Text = dc.CHISOCUOIDC.ToString();
                txtCSCU.Text = dc.CHISODAUDC.ToString();
                txtGhiChu.Text = dc.GHICHUDC;
                txtMTRUYTHU.Text = dc.MTRUYTHUDC.ToString();
                if (dc.INHDDC == "0")
                { ckINHDDC.Checked = false; }
                else { ckINHDDC.Checked = true; }

                txtKhoiLuongNuoc.Text = dc.KLTIEUTHU != null ? dc.KLTIEUTHU.ToString() : "0";
                txtTienNuoc.Text = dc.TIENNUOC != null ? dc.TIENNUOC.ToString() : "0";
                txtTienThueGTGT.Text = dc.TIENTHUE != null ? dc.TIENTHUE.ToString() : "0";
                txtTienThueMoiTruong.Text = dc.TIENPHI != null ? dc.TIENPHI.ToString() : "0";
                txtTongTienNuoc.Text = dc.TONGTIEN != null ? dc.TONGTIEN.ToString() : "0";

                upnlThongTin.Update();
            }
        }

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int namht = DateTime.Now.Year;
                int thanght = DateTime.Now.Month;

                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectSODB":
                        //var khachhang = _khDao.GetKhachHangFromMadb(id);
                        var khachhang = _khDao.Get(id);
                        if (khachhang != null)
                        {                            
                            var kyhientai = new DateTime(namht, thanght, 1);
                            bool dung = gcsDao.IsLockTinhCuocKy1(kyhientai, ddlKHUVUC1.SelectedValue, khachhang.MADP);    

                            if (dung == false)
                            {
                                BindStatus(khachhang);
                                HideDialog("divKhachHang");
                                CloseWaitingDialog();
                                txtMASOHD.Focus();
                            }
                            else
                            {
                                CloseWaitingDialog();
                                HideDialog("divKhachHang");
                                ShowInfor("Đã khoá sổ. Không được điều chỉnh.");
                            }                            
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
            var khachhang = _khDao.GetKhachHangFromMadb(lblIDKH.Text.Trim());
            var tieuthu = _ttDao.GetTN(khachhang.IDKH, ddlTHANG.SelectedIndex + 1, int.Parse(txtNAM.Text.Trim()));
            
            if (khachhang != null)
            {
                lblTENKH.Text = khachhang.TENKH;
                lblIDKH.Text = khachhang.IDKH;
                lblTENDP.Text = khachhang.DUONGPHO != null ? khachhang.DUONGPHO.TENDP : "";
                lblTENKV.Text = khachhang.KHUVUC != null ? khachhang.KHUVUC.TENKV : "";
                lblCSMOI.Text = Convert.ToString(tieuthu.CHISOCUOI);
                lblCSCU.Text = Convert.ToString(tieuthu.CHISODAU);
                lblTIEUTHU.Text = Convert.ToString(tieuthu.KLTIEUTHU);
                lblTHANHTIEN.Text = Convert.ToString(tieuthu.TIENNUOC);
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

                int thangF = Convert.ToInt32(ddlTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());
                int namTruoc = DateTime.Now.Year-1;

                if (_nvDao.Get(b).MAKV == "X")
                {
                    if(namF == Convert.ToInt32(nam) || namF == namTruoc)
                    {                        
                        if ((thangF == 12 ? 1 : thangF + 1) != thang1)
                        {
                            CloseWaitingDialog();
                            ShowError("Chọn kỳ, tháng, năm cho đúng.");
                            return;
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowError("Chọn năm cho đúng.");
                        return;
                    }
                }
                else
                {
                    if (query.MAKV == "L" || query.MAKV == "M" || query.MAKV == "Q")//tri ton - tinh bien - an phu
                    {
                        var kynayF = new DateTime(namF, thangF, 1);

                        bool dungdotin = _gcspoDao.IsLockDotInHD(kynayF, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).IDMADOTIN);

                        if (dungdotin == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ kỳ thu hộ, ghi chỉ số.");
                            return;
                        }
                    }

                    //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                    bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).MADP);
                    if (dung == true)
                    {
                        CloseWaitingDialog();
                        ClearForm();
                        BindGrid();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }
                }

                if (!HasPermission(Functions.GCS_DieuChinhHoaDon, Permission.Insert))
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
                            int chisodau = Convert.ToInt32(txtCSCU.Text.Trim());
                            int chisocuoi = Convert.ToInt32(txtCSMOI.Text.Trim());
                            int mtruythu = Convert.ToInt32(txtMTRUYTHU.Text.Trim());
                            int kltt = Convert.ToInt32(txtKhoiLuongNuoc.Text.Trim());

                            decimal tiennuoc = Convert.ToDecimal(txtTienNuoc.Text.Trim());
                            decimal tienthue = Convert.ToDecimal(txtTienThueGTGT.Text.Trim());
                            decimal tienthuemoitruong = Convert.ToDecimal(txtTienThueMoiTruong.Text.Trim());
                            decimal tongtien = Convert.ToDecimal(txtTongTienNuoc.Text.Trim());

                            if (query.MAKV == "X")
                            {
                                if (ckTINH1GIA.Checked == true)
                                {
                                    string inhddc;
                                    if (ckINHDDC.Checked == true)
                                    { inhddc = "1"; }
                                    else { inhddc = "0"; }

                                    _rp.ThemTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá. " + txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, 
                                        int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDC1GIALX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC1GIALX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

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

                                    _rp.ThemTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, 
                                        int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDCLX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));//co dinh muc
                                    //var msg1 = gcsDao.TinhTienTTDCLX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU
                                    
                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

                                    CloseWaitingDialog();
                                    ClearForm();
                                    BindGrid();
                                }
                            }   // long xuyen
                            else if (query.MAKV == "T")
                            {
                                if (ckTINH1GIA.Checked == true)
                                {
                                    string inhddc;
                                    if (ckINHDDC.Checked == true)
                                    { inhddc = "1"; }
                                    else { inhddc = "0"; }

                                    if (txtGhiChu.Text.Trim().Length > 300)
                                    {
                                        ShowError("Chiều dài ghi chú nhỏ hơn < 300 ký tự. Kiểm tra lại");
                                        CloseWaitingDialog();
                                        return;
                                    }

                                    _rp.ThemTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá. " + txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, 
                                        int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    var msg = gcsDao.TinhTienDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    var msg1 = gcsDao.TinhTienTTDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

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

                                    _rp.ThemTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, 
                                        int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));//co dinh muc
                                    //var msg1 = gcsDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

                                    CloseWaitingDialog();
                                    ClearForm();
                                    BindGrid();
                                }
                            } // tan chau
                            else
                            {
                                if (ckTINH1GIA.Checked == true)
                                {
                                    string inhddc;
                                    if (ckINHDDC.Checked == true)
                                    { inhddc = "1"; }
                                    else { inhddc = "0"; }

                                    if (txtGhiChu.Text.Trim().Length > 300)
                                    {
                                        ShowError("Chiều dài ghi chú nhỏ hơn < 300 ký tự. Kiểm tra lại");
                                        CloseWaitingDialog();
                                        return;
                                    }

                                    _rp.ThemTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá. " + txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), 
                                        inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

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

                                    _rp.ThemTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, 
                                        int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    
                                    //var msg = gcsDao.TinhTienDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim())); // Bang TIEUTHUDC
                                    //var msg1 = gcsDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim())); // Bang TIEUTHU

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU
                                   

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

                                    CloseWaitingDialog();
                                    ClearForm();
                                    BindGrid();
                                }
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

                int thangF = Convert.ToInt32(ddlTHANG.SelectedValue);
                int namF = Convert.ToInt32(txtNAM.Text.Trim());
                int namTruoc = DateTime.Now.Year - 1;

                if (_nvDao.Get(b).MAKV == "X")
                {
                    if (namF == Convert.ToInt32(nam) || namF == namTruoc)
                    {
                        if (thangF + 1 != thang1)
                        {
                            CloseWaitingDialog();
                            ShowError("Chọn kỳ, tháng, năm cho đúng.");
                            return;
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowError("Chọn năm cho đúng.");
                        return;
                    }
                }
                else
                {
                    if (query.MAKV == "L" || query.MAKV == "M" || query.MAKV == "Q")//tri ton - tinh bien - an phu
                    {
                        var kynayF = new DateTime(namF, thangF, 1);

                        bool dungdotin = _gcspoDao.IsLockDotInHD(kynayF, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).IDMADOTIN);

                        if (dungdotin == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ kỳ thu hộ, ghi chỉ số.");
                            return;
                        }
                    }

                    //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                    bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).MADP);
                    if (dung == true)
                    {
                        CloseWaitingDialog();
                        ClearForm();
                        BindGrid();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }
                }                

                if (!HasPermission(Functions.GCS_DieuChinhHoaDon, Permission.Update))
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
                            int chisodau = Convert.ToInt32(txtCSCU.Text.Trim());
                            int chisocuoi = Convert.ToInt32(txtCSMOI.Text.Trim());
                            int mtruythu = Convert.ToInt32(txtMTRUYTHU.Text.Trim());
                            int kltt = Convert.ToInt32(txtKhoiLuongNuoc.Text.Trim());

                            decimal tiennuoc = Convert.ToDecimal(txtTienNuoc.Text.Trim());
                            decimal tienthue = Convert.ToDecimal(txtTienThueGTGT.Text.Trim());
                            decimal tienthuemoitruong = Convert.ToDecimal(txtTienThueMoiTruong.Text.Trim());
                            decimal tongtien = Convert.ToDecimal(txtTongTienNuoc.Text.Trim());

                            if (query.MAKV == "X")
                            {
                                if (ckTINH1GIA.Checked == true)
                                {
                                    string inhddc;
                                    if (ckINHDDC.Checked == true)
                                    { inhddc = "1"; }
                                    else { inhddc = "0"; }

                                    _rp.UpTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá." + txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDC1GIALX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC1GIALX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

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

                                    _rp.UpTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDCLX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDCLX(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

                                    CloseWaitingDialog();
                                    ClearForm();
                                    BindGrid();
                                }
                            }
                            else
                            {
                                if (ckTINH1GIA.Checked == true)
                                {
                                    string inhddc;
                                    if (ckINHDDC.Checked == true)
                                    { inhddc = "1"; }
                                    else { inhddc = "0"; }

                                    _rp.UpTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), "Điều chỉnh 1 giá." + txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC1GIA(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

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

                                    _rp.UpTieuThuDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                        int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), inhddc, int.Parse(txtMTRUYTHU.Text.Trim()), int.Parse(txtSODINHMUC.Text.Trim()));

                                    //var msg = gcsDao.TinhTienDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                                    //var msg1 = gcsDao.TinhTienTTDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));

                                    var msg = gcsDao.TinhTongTienNuocDC(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHUDC   

                                    var msg1 = gcsDao.TinhTongTienNuoc(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                                        "A", tiennuoc, tienthue, tienthuemoitruong, tongtien, chisodau, chisocuoi, mtruythu, kltt); // Bang TIEUTHU

                                    ShowInfor(ResourceLabel.Get(msg));
                                    ShowInfor(ResourceLabel.Get(msg1));

                                    CloseWaitingDialog();
                                    ClearForm();
                                    BindGrid();
                                }
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

            ckTINH1GIA.Checked = false;
            ddlMDSD.Visible = false;

            SetChiSoTien();
        }

        private bool IsDataValid()
        {            
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (_nvDao.Get(b).MAKV != "X")
            {
                if (string.Empty.Equals(txtMASOHD.Text.Trim()))
                {
                    ShowError("Mã số hoá đơn không được rỗng.", txtMASOHD.ClientID);
                    return false;
                }
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
            if (!string.Empty.Equals(txtSODINHMUC.Text.Trim()))
            {
                try
                {
                    int.Parse(txtSODINHMUC.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số mới phải là số.", txtSODINHMUC.ClientID);
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
                        var dc = _ttdcDao.GetTN(id,int.Parse(ddlTHANG.SelectedValue),int.Parse(txtNAM.Text.Trim()));
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
                var list = _ttdcDao.GetDC1(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                gvDieuChinhHD.DataSource = list;
                gvDieuChinhHD.PagerInforText = list.Count.ToString();
                gvDieuChinhHD.DataBind();
            }
            else
            {
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var list = _ttdcDao.GetDC(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                    gvDieuChinhHD.DataSource = list;
                    gvDieuChinhHD.PagerInforText = list.Count.ToString();
                    gvDieuChinhHD.DataBind();
                }
                else
                {
                    //var iddotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, ddlKHUVUC1.SelectedValue);
                    var list = _ttdcDao.GetDCDotIn(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), 
                            int.Parse(txtNAM.Text.Trim()), ddlDOTGCS.SelectedValue);
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
            var dtDSKTKY =
                new ReportClass().BKDieuChinh(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString()).Tables[0];
            if (dtDSKTKY.Rows.Count>0)
            {
                //Session["DSKTKY"] = dtDSKTKY;
                //Session["DSKTKY_THANGNAM"] = string.Format("{0}/{1}",
                //                                        ddlTHANG.SelectedValue, txtNAM.Text.Trim());
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpDieuChinhHoaDon.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo", "");
            }            
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
            bool dung = gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);

            UnblockDialog("divKhachHang");
            //BindKhachHang();
            upnlKhachHang.Update();
        }

        protected void ddlMDSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnblockWaitingDialog();
            CloseWaitingDialog();
        }

        protected void ckTINH1GIA_CheckedChanged(object sender, EventArgs e)
        {
            if (ckTINH1GIA.Checked)
            {
                ddlMDSD.Visible = true;                
            }
            else
            {
                ddlMDSD.Visible = false;                
            }
        }

        protected void txtMASOHD_TextChanged(object sender, EventArgs e)
        {

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
                    //dt = new ReportClass().BKDieuChinh(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString()).Tables[0];
                    dt = new ReportClass().BKDieuChinhDotIn(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString(),
                                ddlDOTGCS.SelectedValue, "", "DSDCDOTINALL").Tables[0];
                }
                else
                {
                    //var idotin = _diDao.GetKVDot(ddlDOTGCS.SelectedValue, cboKhuVuc.SelectedValue);
                    dt = new ReportClass().BKDieuChinhDotIn(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString(),
                                ddlDOTGCS.SelectedValue, "", "DSDCDOTIN").Tables[0];
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

        private bool IsChiSoNuoc()
        {
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

            if (!string.Empty.Equals(txtSODINHMUC.Text.Trim()))
            {
                try
                {
                    int.Parse(txtSODINHMUC.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số mới phải là số.", txtSODINHMUC.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtKhoiLuongNuoc.Text.Trim()))
            {
                try
                {
                    int.Parse(txtKhoiLuongNuoc.Text.Trim());
                }
                catch
                {
                    ShowError("Khối lượng nước phải là số.", txtKhoiLuongNuoc.ClientID);
                    return false;
                }
            }

            //if (!string.Empty.Equals(txtTongTienNuoc.Text.Trim()))
            //{
            //    try
            //    {
            //        Convert.ToInt64(txtTongTienNuoc.Text.Trim());
            //    }
            //    catch
            //    {
            //        ShowError("Tổng tiền nước phải là số.", txtTongTienNuoc.ClientID);
            //        return false;
            //    }
            //}

            return true;
        }

        protected void btTinhTien_Click(object sender, EventArgs e)
        {
            try
            {
                TinhTienNuoc();                
            }
            catch { }
        }        

        private void TinhTienNuoc()
        {
            try
            {
                var kh = _khDao.Get(lblIDKH.Text.Trim());

                if (kh == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                if (!IsChiSoNuoc())
                {
                    CloseWaitingDialog();
                    return;
                }

                int chisomoi = Convert.ToInt32(txtCSMOI.Text.Trim());
                int chisocu = Convert.ToInt32(txtCSCU.Text.Trim());
                int truythu = Convert.ToInt32(txtMTRUYTHU.Text.Trim());
                int sodinhmuc = Convert.ToInt32(txtSODINHMUC.Text.Trim());

                Int64 kltt = chisomoi - chisocu + truythu;
                
                // thue GTGT = 5% ; phi moi truong = 10%
                string mdsd = kh.MAMDSD;  // kh.MAMDSD;
                decimal thuegtgt = (decimal)1.05;   // 1/(1+thue 5%)
                //decimal thuemt = (decimal)1.1;  // 1/(1+thue 10%)
                decimal thuemt = (decimal)0.1;  // thue 10%

                var tinhtiennuoc = ckTINH1GIA.Checked == true ? gcsDao.TongTienNuocGiaMuc01(kltt, mdsd, thuegtgt, thuemt, 1, sodinhmuc, 10)
                    : gcsDao.TongTienNuoc(kltt, mdsd, thuegtgt, thuemt, 1, sodinhmuc, 10);                

                foreach(var tiennuoc in tinhtiennuoc)
                {
                    txtTienNuoc.Text = tiennuoc.TienNuoc.ToString();
                    txtTienThueGTGT.Text = tiennuoc.TienThue.ToString();
                    txtTienThueMoiTruong.Text = tiennuoc.TienThueMoiTruong.ToString();

                    txtTongTienNuoc.Text = tiennuoc.TongTienNuoc.ToString();
                }               

                txtKhoiLuongNuoc.Text = kltt.ToString();    

                CloseWaitingDialog();
                upnlThongTin.Update();
            }
            catch { }
        }

    }
}
