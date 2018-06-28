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
using System.Web.UI;
using System.Data;
using CrystalDecisions.Shared;

using System.IO;


namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class ThayDHPower : Authentication
    {
        private readonly TrangThaiGhiDao _ttghiDao = new TrangThaiGhiDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly LoaiDongHoPoDao _ldhpoDao = new LoaiDongHoPoDao();
        private readonly ThayDongHoPoDao _tdhpoDao = new ThayDongHoPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();        
        private readonly TieuThuPoDao _ttpoDao = new TieuThuPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass rp = new ReportClass();

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
                Authenticate(Functions.KH_ThayDongHoPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_THAYDONGHOPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_THAYDONGHOPO;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
        }

        private void LoadStaticReferences()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                timkv();

                var loaiDhList = _ldhpoDao.GetList();
                cboLoaiDh.Items.Clear();
                cboLoaiDh.DataTextField = "MALDHPO";
                cboLoaiDh.DataValueField = "MALDHPO";
                cboLoaiDh.DataSource = loaiDhList;
                cboLoaiDh.DataBind();

                ClearForm();

                //txtNAM.Text = DateTime.Now.Year.ToString();
                //ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                //txtNgayThay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");

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
                    //var dotin = _diDao.GetListKVDDNotP7(kvpo);
                    var dotin = _diDao.GetListKVDDP7(kvpo);
                    
                    ddlDOTGCS.Items.Clear();
                    ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
                    foreach (var d in dotin)
                    {
                        ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                    }
                }               

                //trang thai ghi
                var trangthai = _ttghiDao.GetList();
                ddlTrangThaiGhi.Items.Clear();
                ddlTrangThaiGhi.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var tt in trangthai)
                {
                    ddlTrangThaiGhi.Items.Add(new ListItem(tt.TENTTHAIGHI, tt.TTHAIGHI));
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }       

        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        private void BindKhachHang()
        {
            if (ddlTrangThaiGhi.SelectedValue == "%")
            {
                var danhsach = _khpoDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(), txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                    txtSONHA.Text.Trim(), txtTENDP.Text.Trim(), ddlKHUVUC.SelectedValue.Trim());

                gvDanhSach.DataSource = danhsach;
                gvDanhSach.PagerInforText = danhsach.Count.ToString();
                cpeFilter.Collapsed = true;
                gvDanhSach.DataBind();
                tdDanhSach.Visible = true;
            }
            else
            {
                var danhsach = _khpoDao.SearchKhachHangThayDH(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(), txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                    txtSONHA.Text.Trim(), txtTENDP.Text.Trim(), ddlKHUVUC.SelectedValue.Trim(), int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()),
                    ddlTrangThaiGhi.SelectedValue);

                gvDanhSach.DataSource = danhsach;
                gvDanhSach.PagerInforText = danhsach.Count.ToString();
                cpeFilter.Collapsed = true;
                gvDanhSach.DataBind();
                tdDanhSach.Visible = true;
            }

            upnlKhachHang.Update();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");               
        }

        private void BindStatus(KHACHHANGPO kh)
        {
            var tieuthu = _ttpoDao.GetTN(kh.IDKHPO, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));

            txtSODB.Text = (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).ToString();
            lblTENKH.Text = kh.TENKH.ToString();
            //SetLabel(lblIDKH.ClientID, kh.IDKH);
            //SetControlValue(lblIDKH.ClientID, kh.IDKH);
            lblIDKH.Text = kh.IDKHPO;
            lblTENDP.Text = kh.DUONGPHOPO != null ? kh.DUONGPHOPO.TENDP.ToString() : "";
            lblTENKV.Text = kh.KHUVUCPO != null ? kh.KHUVUCPO.TENKV.ToString() : "";    

            txtCSMOI.Text = tieuthu.CHISOCUOI.ToString();

            if (_dhpoDao.Get(kh.MADHPO) != null)
            {
                if (_dhpoDao.Get(kh.MADHPO).SONO != null)
                {
                    lbMADHPOCU.Text = kh.MADHPO;
                    lblSONO.Text = _dhpoDao.Get(kh.MADHPO).SONO.ToString();
                    lblLOAITK.Text = _dhpoDao.Get(kh.MADHPO.ToString()).CONGSUAT;
                }
                else 
                { 
                    lblSONO.Text = "";
                    lblLOAITK.Text = "";
                }
            }
            else { lblSONO.Text = ""; }

            if (kh.THUYLK != null)
            {
                //ddlKICHCODH.Text = kh.THUYLK.ToString();
                ddlKICHCODH.SelectedIndex = 0;
            }
            else
            {
                ddlKICHCODH.SelectedIndex = 0;
            }

            if (int.Parse(ddlTHANG1.SelectedValue) != 1)
            {
                var tieuthukytruoc = _ttpoDao.GetTNKyTruoc(kh.IDKHPO, int.Parse(ddlTHANG1.SelectedValue) - 1, int.Parse(txtNAM1.Text.Trim()));

                lblCSDAU.Text = tieuthukytruoc.CHISOCUOI != null ? tieuthukytruoc.CHISOCUOI.ToString() : "0";
                lblCSCUOI.Text = "0";
            }
            else
            {
                var tieuthukytruoc = _ttpoDao.GetTNKyTruoc(kh.IDKHPO, 12, int.Parse(txtNAM1.Text.Trim()) - 1);

                lblCSDAU.Text = tieuthukytruoc.CHISOCUOI != null ? tieuthukytruoc.CHISOCUOI.ToString() : "0";
                lblCSCUOI.Text = "0";
            }
            
            lblNGAYTHAY.Text = kh.NGAYTHAYDH.HasValue ? kh.NGAYTHAYDH.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
            lblNGAYHOANTHANH.Text = kh.NGAYHT.HasValue ? kh.NGAYHT.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");

            var madotin = _dppoDao.GetDP(kh.MADPPO).IDMADOTIN;
            var dotin = ddlDOTGCS.Items.FindByValue(madotin);
            if (dotin != null)
                ddlDOTGCS.SelectedIndex = ddlDOTGCS.Items.IndexOf(dotin);

            upnlThongTin.Update();
        }

        private void BindStatusTDH(KHACHHANGPO kh)
        {
            try
            {
                var tieuthu = _ttpoDao.GetTN(kh.IDKHPO, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));

                SetControlValue(txtSODB.ClientID, kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO);
                SetLabel(lblTENKH.ClientID, kh.TENKH);
                lblIDKH.Text = kh.IDKHPO;
                SetLabel(lblTENDP.ClientID, kh.DUONGPHOPO != null ? kh.DUONGPHOPO.TENDP : "");
                SetLabel(lblTENKV.ClientID, kh.KHUVUCPO != null ? kh.KHUVUCPO.TENKV : "");
                lblNGAYTHAY.Text = kh.NGAYTHAYDH.HasValue ? kh.NGAYTHAYDH.Value.ToString("dd/MM/yyyy") : "";
                lblNGAYHOANTHANH.Text = kh.NGAYHT.HasValue ? kh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";

                var idmadt = _dppoDao.GetDP(kh.MADPPO);
                var dotin = ddlDOTGCS.Items.FindByValue(idmadt.IDMADOTIN);
                if (dotin != null)
                    ddlDOTGCS.SelectedIndex = ddlDOTGCS.Items.IndexOf(dotin);

                upnlThongTin.Update();
            }
            catch { }
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
                            txtSODB.Focus();
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

        private bool IsValidate()
        {
            // check ngày hoàn thành có định dạng dd/MM/yyyy
            if (!string.IsNullOrEmpty(txtNgayThay.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                }
                catch
                {
                    ShowError("Ngày thay không hợp lệ. Kiểm tra lại.", txtNgayThay.ClientID);
                    return false;
                }
            }

            // check ngày hoàn thành có định dạng dd/MM/yyyy
            if (!string.IsNullOrEmpty(txtNgayHoanThanh.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                }
                catch
                {
                    ShowError("Ngày bấm chì không hợp lệ. Kiểm tra lại.", txtNgayHoanThanh.ClientID);
                    return false;
                }
            }
            return true;
        }

        private void ClearForm()
        {
            ddlTHANG1.SelectedIndex = DateTime.Now.Month - 1;

            txtNAM1.Text = DateTime.Now.Year.ToString();
            txtIDKH.Text = "";
            lblIDKH.Text = "";
            lblTENKH.Text = "";
            lblTENDP.Text = "";
            lblTENKV.Text = "";
            lblLOAITK.Text = "";
            lblSONO.Text = "";
            txtCSNGUNG.Text = "0";
            txtTRUYTHU.Text = "0";
            txtMaDongho.Text = "";
            lbSONODH.Text = "";
            txtCSBATDAU.Text = "0";
            txtCSMOI.Text = "0";
            txtGhiChu.Text = "";
            txtNgayThay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");
            lbMADHPOCU.Text = "";
            ddlDONGHOCAPBAN.SelectedIndex = 0;
            btnSaveUp.Visible = false;
            btnSave.Visible = true;
            ddlLYDOTHAYDH.SelectedIndex = 0;

            lbMaLoaiDH.Text = "";
            lbCongSuat.Text = "";

            txtHeSoNhan.Text = "1";
        }

        protected void btnSave_Click(object sender, EventArgs e)
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

            int thangForm = Convert.ToUInt16(ddlTHANG1.SelectedValue);
            int namForm = Convert.ToInt32(txtNAM1.Text.Trim());
            var kyForm = new DateTime(namForm, thangForm, 1);

            var kh = _khpoDao.Get(lblIDKH.Text.Trim());
            string h = lblIDKH.ClientID;
            string madhkh = kh.MADHPO;

            //var phien7dot1 = _diDao.Get(kh.MAMDSDPO);

            //khoa so theo dot in hoa don
            var dotin = _diDao.Get(kh.DOTINHD != null ? kh.DOTINHD : "");
            bool p7d1m = _gcspoDao.IsLockDotInHD(kyForm, ddlKHUVUC.SelectedValue, dotin != null && dotin.MADOTIN != null ? dotin.MADOTIN : "");//phien 7 , kh muc dich khac, ngoai sinh hoat

            if (p7d1m == true)
            {
                CloseWaitingDialog();
                ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1 P7.");
                return;
            }     

            //bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, kvpo.MAKVPO.ToString());
            //bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay1, kvpo.MAKVPO.ToString(), _khpoDao.Get(lblIDKH.Text.Trim()).MADPPO); 
            bool dung = _gcspoDao.IsLockTinhCuocKy1(kyForm, kvpo.MAKVPO.ToString(), _khpoDao.Get(lblIDKH.Text.Trim()).MADPPO); 
           
            if (dung == true)
            {
                CloseWaitingDialog();
                ShowInfor("Đã khoá sổ ghi chỉ số.");
                return;
            }

            if (lbSONODH.Text.Trim() == null || string.Empty.Equals(lbSONODH.Text.Trim())
                    || txtMaDongho.Text.Trim() == null || string.Empty.Equals(txtMaDongho.Text.Trim()))
            {
                CloseWaitingDialog();
                ShowInfor("Chưa có số No đồng hồ cần thay! Kiểm tra lại");
                return;
            }            

            if (!HasPermission(Functions.KH_ThayDongHoPo, Permission.Insert))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            if (!IsValidate())
            {
                CloseWaitingDialog();
                return;
            }

            try
            {   

                if (kh == null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                    return;
                }

                //var isTHD = _tdhpoDao.GetKy(kh.IDKHPO, DateTime.Now.Month, DateTime.Now.Year);namForm
                var isTHD = _tdhpoDao.GetKy(kh.IDKHPO, thangForm, namForm);
                if (isTHD != null)
                {
                    ShowError("Khách hàng thay đồng hồ rồi. Kiểm tra lại.");
                    CloseWaitingDialog();
                    return;
                }

                //khoa so theo dot in hoa don
                bool p7d1 = _gcspoDao.IsLockDotInHD(kyForm, kvpo.MAKVPO.ToString(), "DDP7D1");//phien 7 , kh muc dich khac, ngoai sinh hoat
                if (kh.MAMDSDPO != "A" && kh.MAMDSDPO != "B" && kh.MAMDSDPO != "G" && kh.MAMDSDPO != "Z")
                {
                    if (p7d1 == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1. Không được thay ĐH");
                        return;
                    }
                }

                kh.THUYLK = lbCongSuat.Text.Trim();// ddlKICHCODH.SelectedValue;

                kh.MALDHPO = cboLoaiDh.Items.Count > 0 ? cboLoaiDh.SelectedValue : null;
                kh.MADHPO = txtMaDongho.Text.Trim();

                var madhpo = _dhpoDao.GetDASD(kh.MADHPO);
                if (madhpo != null)
                {
                    ShowError("Đồng hồ điện đã sử dụng. Kiểm tra lại.");
                    CloseWaitingDialog();
                    return;
                }
                
                //var msgdh = _dhpoDao.UpdateDASDDH(txtMaDongho.Text.Trim());

                kh.NGAYTHAYDH = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                kh.CHISODAU = Convert.ToInt32(txtCSNGUNG.Text.Trim());
                kh.CHISOCUOI = Convert.ToInt32(txtCSBATDAU.Text.Trim());
                kh.m4Poor = Convert.ToInt32(txtTRUYTHU.Text.Trim());
                kh.KLKHOAN = Convert.ToInt32(txtCSMOI.Text.Trim());

                kh.DIACHI_INHOADON = ddlLYDOTHAYDH.SelectedItem + ": " + ddlLYDOTHAYDH.SelectedValue;

                var msg = _khpoDao.UpdateThayDongHoKyThay(kh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV,
                                                lblSONO.Text.Trim(), lblLOAITK.Text.Trim(), DateTimeUtil.GetVietNamDate(lblNGAYTHAY.Text.Trim()),
                                                DateTimeUtil.GetVietNamDate(lblNGAYHOANTHANH.Text.Trim()), madhkh, ddlDONGHOCAPBAN.SelectedValue, kyForm);

                rp.UpdateTieuThuDHPo(kh.IDKHPO, Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), Convert.ToInt32(txtTRUYTHU.Text.Trim()));

                rp.UPDATETTDHPOMOI(kh.IDKHPO, kvpo.MAKVPO, "", namForm, thangForm, 0, 0, "", "", 0, 0, 0, Convert.ToInt16(txtHeSoNhan.Text.Trim())
                    , DateTime.Now, DateTime.Now, "", "", "", "", "", LoginInfo.MANV, "UPHESONHANPO");

                //rp.UPTHayDoiCTPO(kh.IDKHPO, Convert.ToInt16(ddlTHANGTDCT.SelectedValue), Convert.ToInt16(txtNAMTDCT.Text.Trim()), "UpHeSoNhanTDH",
                 //       ddlDOTINHD.SelectedValue, LoginInfo.MANV, txtHeSoNhan.Text.Trim(), txtLyDoHeSoNhan.Text.Trim());

                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    //rp.UpdateTieuThuDHPo(kh.IDKHPO, Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), 
                    //    Convert.ToInt32(txtTRUYTHU.Text.Trim()));

                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();

                    upnlThongTin.Update();
                    upnlGrid.Update();
                }
                else
                {
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtSODB.ClientID);
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

            var dh = _tdhpoDao.Get(int.Parse(lblID.Text.Trim()));
            string h = lblIDKH.ClientID;

            var kh = _khpoDao.Get(dh.IDKHPO);

            int thangForm = Convert.ToUInt16(ddlTHANG1.SelectedValue);
            int namForm = Convert.ToInt32(txtNAM1.Text.Trim());
            var kyForm = new DateTime(namForm, thangForm, 1);

            //khoa so theo dot in hoa don
            var dotin = _diDao.Get(kh.DOTINHD != null ? kh.DOTINHD : "");
            bool p7d1m = _gcspoDao.IsLockDotInHD(kyForm, ddlKHUVUC.SelectedValue, dotin != null && dotin.MADOTIN != null ? dotin.MADOTIN : "");//phien 7 , kh muc dich khac, ngoai sinh hoat

            if (p7d1m == true)
            {
                CloseWaitingDialog();
                ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1 P7.");
                return;
            }   

            //bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, kvpo.MAKVPO.ToString());
            bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay1, kvpo.MAKVPO.ToString(), _khpoDao.Get(_tdhpoDao.Get(Convert.ToInt32(lblID.Text.Trim())).IDKHPO).MADPPO); 

            if (dung == true)
            {
                CloseWaitingDialog();
                ClearForm();
                BindGrid();
                ShowInfor("Đã khoá sổ ghi chỉ số.");
                return;
            }

            if (!HasPermission(Functions.KH_ThayDongHoPo, Permission.Update))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            if (!IsValidate())
            {
                CloseWaitingDialog();
                return;
            }

            try
            {
                
                if (dh == null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                    return;
                }

                if (lbSONODH.Text.Trim() == null || string.Empty.Equals(lbSONODH.Text.Trim())
                    || txtMaDongho.Text.Trim() == null || string.Empty.Equals(txtMaDongho.Text.Trim()))
                {
                    CloseWaitingDialog();
                    ShowInfor("Chưa có số No đồng hồ cần thay! Kiểm tra lại...");
                    return;
                } 

                //khoa so theo dot in hoa don
                //var kh = _khpoDao.Get(dh.IDKHPO);
                //bool p7d1 = _gcspoDao.IsLockDotInHD(kynay1, kvpo.MAKVPO.ToString(), "DDP7D1");//phien 7 , kh muc dich khac, ngoai sinh hoat
                //if (kh.MAMDSDPO != "A" && kh.MAMDSDPO != "B" && kh.MAMDSDPO != "G" && kh.MAMDSDPO != "Z")
                //{
                //    if (p7d1 == true)
                //    {
                //        CloseWaitingDialog();
                //        ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1. Không được Up thay ĐH");
                //        return;
                //    }
                //}

                if (dh.MADHPO != txtMaDongho.Text.Trim())
                {
                    var madhsua = _dhpoDao.Get(dh.MADHPO);
                    var msgdhsua = _dhpoDao.UpdateKoSD(madhsua);
                }

                dh.CHISONGUNG = Convert.ToInt32(txtCSNGUNG.Text.Trim());
                dh.MTRUYTHU = Convert.ToInt32(txtTRUYTHU.Text.Trim());
                dh.MALDHPO = cboLoaiDh.Items.Count > 0 ? cboLoaiDh.SelectedValue : null;
                dh.MADHPO = txtMaDongho.Text.Trim();
                dh.CHISOBATDAU = Convert.ToInt32(txtCSBATDAU.Text.Trim());
                dh.CHISOMOI = Convert.ToInt32(txtCSMOI.Text.Trim());
                dh.NGAYTD = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                dh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                dh.KICHCO = lbCongSuat.Text.Trim();//ddlKICHCODH.SelectedValue;
                dh.DHCAPBAN = ddlDONGHOCAPBAN.SelectedValue;

                dh.LYDOTHAY = ddlLYDOTHAYDH.SelectedItem + ": " + ddlLYDOTHAYDH.SelectedValue;

                var msg = _tdhpoDao.UpThayDongHo(dh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                var msg1 = _khpoDao.UpThayDongHo(dh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                rp.UpdateTieuThuDHPo(kh.IDKHPO, Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), Convert.ToInt32(txtTRUYTHU.Text.Trim()));

                rp.UPDATETTDHPOMOI(kh.IDKHPO, kvpo.MAKVPO, "", namForm, thangForm, 0, 0, "", "", 0, 0, 0, Convert.ToInt16(txtHeSoNhan.Text.Trim())
                    , DateTime.Now, DateTime.Now, "", "", "", "", "", LoginInfo.MANV, "UPHESONHANPO");

                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();

                    upnlThongTin.Update();
                    upnlGrid.Update();
                }
                else
                {
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtSODB.ClientID);
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtSODB_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(txtSODB.Text.Trim()))
            {              
                txtIDKH.Text = txtSODB.Text.Trim();
                upnlKhachHang.Update();

                BindKhachHang();                
                CloseWaitingDialog();
            }
            else
            {
                txtIDKH.Text = txtSODB.Text.Trim();
                //BindKhachHang();               
                CloseWaitingDialog();
            }
            
        }

        private void BindGrid()
        {            
            int thang = int.Parse(ddlTHANG1.SelectedValue);
            int nam = int.Parse(txtNAM1.Text.Trim());
            //var list = _khpoDao.GetThayDongHoListThangNam(thang, nam);

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (ddlDOTGCS.SelectedValue == "%")
            {
                var list = _khpoDao.GetTHDKV(thang, nam, _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);

                gvKhachHang.DataSource = list;
                gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();
            }
            else
            {
                var list = _khpoDao.GetListTDHDotIn2(thang, nam, _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO, ddlDOTGCS.SelectedValue);
                if (list != null)
                {
                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }                
            }

            CloseWaitingDialog();
            upnlGrid.Update();
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvKhachHang.PageIndex = e.NewPageIndex;                
                BindGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindTDH(THAYDONGHOPO thaydh)
        {
            lblLOAITK.Text = thaydh.MALDHCU;
            lblSONO.Text = thaydh.MADHCU;
            txtCSNGUNG.Text = thaydh.CHISONGUNG.ToString();
            txtTRUYTHU.Text = thaydh.MTRUYTHU.ToString();
            cboLoaiDh.SelectedValue = thaydh.MALDHPO;
            txtMaDongho.Text = thaydh.MADHPO;
            lbSONODH.Text = thaydh.MADHPO != null ? _dhpoDao.Get(thaydh.MADHPO).SONO : "";
            txtCSBATDAU.Text = thaydh.CHISOBATDAU.ToString();
            txtCSMOI.Text = thaydh.CHISOMOI.ToString();
            txtNgayThay.Text = thaydh.NGAYTD.HasValue ? thaydh.NGAYTD.Value.ToString("dd/MM/yyyy") : "";
            txtNgayHoanThanh.Text = thaydh.NGAYHT.HasValue ? thaydh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
            //ddlKICHCODH.SelectedValue = thaydh.KICHCO;
            lbCongSuat.Text = thaydh.KICHCO != null ? thaydh.KICHCO  : "" ;
            txtGhiChu.Text = thaydh.GHICHU;

            var capban = ddlDONGHOCAPBAN.Items.FindByValue(thaydh.DHCAPBAN != null ? thaydh.DHCAPBAN : null);
            if (capban != null)
                ddlDONGHOCAPBAN.SelectedIndex = ddlDONGHOCAPBAN.Items.IndexOf(capban);

            if (thaydh.LYDOTHAY != null)
            {
                var lydothaydh = ddlLYDOTHAYDH.Items.FindByValue(thaydh.LYDOTHAY.Substring(thaydh.LYDOTHAY.Length - 1, 1));
                if (lydothaydh != null)
                {
                    ddlLYDOTHAYDH.SelectedIndex = ddlLYDOTHAYDH.Items.IndexOf(lydothaydh);
                }
                else
                {
                    ddlLYDOTHAYDH.Items.Clear();
                    ddlLYDOTHAYDH.Items.Add(new ListItem("Không biết", "%"));
                }
            }
            else
            {
                ddlLYDOTHAYDH.Items.Clear();
                ddlLYDOTHAYDH.Items.Add(new ListItem("Không biết", "%"));
            }

            upnlThongTin.Update();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                var tdh = _tdhpoDao.Get(int.Parse(id));

                switch (e.CommandName)
                {
                    case "SelectTDH":
                        var khachhang = _khpoDao.Get(tdh.IDKHPO);
                        if (khachhang != null)
                        {
                            BindStatusTDH(khachhang);
                            BindTDH(tdh);
                            HideDialog("divKhachHang");
                            lblID.Text = id;
                            CloseWaitingDialog();
                            txtSODB.Focus();
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

        protected void gvKhachHang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        /*
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int thang, nam;
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                thang = int.Parse(ddlTHANG.SelectedValue);
                nam = int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập năm hợp lệ.", txtNAM.ClientID);
                return;
            }

            var list = _khDao.GetThayDongHoList(thang, nam);
            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            ClearForm();

            upnlGrid.Update();
            CloseWaitingDialog();
        }
        */


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_ThayDongHoPo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<THAYDONGHOPO>();
                    var listIds = strIds.Split(',');


                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _tdhpoDao.Get(int.Parse(ma))));

                    //TODO: check relation before update list
                    var msg = _tdhpoDao.DoAction(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    if (msg.MsgType != MessageType.Error)
                    {
                        CloseWaitingDialog();

                        ShowInfor(ResourceLabel.Get(msg));

                        // Refresh grid view
                        BindGrid();

                        upnlGrid.Update();
                    }
                    else
                    {
                        CloseWaitingDialog();

                        ShowError(ResourceLabel.Get(msg));
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Chọn record để xóa.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBrowseDHSONO_Click(object sender, EventArgs e)
        {
            UnblockDialog("divDongHoSoNo");
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            //upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
            CloseWaitingDialog();
        }

        private void BindDongHoSoNo()
        {            
            var list = _dhpoDao.GetListDASD(txtKeywordDHSONO.Text.Trim(), ddlKHUVUC.SelectedValue);
            gvDongHoSoNo.DataSource = list;
            gvDongHoSoNo.PagerInforText = list.Count.ToString();
            gvDongHoSoNo.DataBind();            
        }

        protected void gvDongHoSoNo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADH":
                        var dh = _dhpoDao.Get(id);
                        if (dh != null)
                        {
                            //SetControlValue(txtMaDongho.ClientID, dh.MADH);
                            txtMaDongho.Text = dh.MADHPO.ToString();                           
                            lbSONODH.Text = dh.SONO.ToString();
                            //SetLabel(lblKICHCO.ClientID, ldhDao.Get(dh.MALDH).ToString());
                            /*var ldhkc = _loaiDhDao.GetListldh(dh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetLabel(lblKICHCO.ClientID, kc.KICHCO);
                            }*/

                            lbMaLoaiDH.Text = dh.MALDHPO != null ? dh.MALDHPO : "";
                            lbCongSuat.Text = dh.CONGSUAT != null ? dh.CONGSUAT : "";

                            upnlThongTin.Update();
                            HideDialog("divDongHoSoNo");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHoSoNo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDongHoSoNo.PageIndex = e.NewPageIndex;                
                BindDongHoSoNo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtNgayThay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string dateht = txtNgayThay.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);

                if (dateht.Length != 10 && dateht.Length == 6)
                {
                    txtNgayThay.Text = ngayht + "/" + thanght + "/20" + namht;
                }

                //upnlThongTin.Update();
                //txtNgayHoanThanh.Focus();
            }
            catch { ShowWarning("Ngày lắp đặt không hợp lệ"); }
        }

        protected void txtNgayHoanThanh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string dateht = txtNgayHoanThanh.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);

                if (dateht.Length != 10 && dateht.Length == 6)
                {
                    txtNgayHoanThanh.Text = ngayht + "/" + thanght + "/20" + namht;
                }


                //upnlThongTin.Update();
                //txtGhiChu.Focus();
            }
            catch { ShowWarning("Ngày lắp đặt không hợp lệ"); }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            ClearForm();

            CloseWaitingDialog();
            upnlGrid.Update();
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
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKV(_kvpoDao.GetPo(d).MAKVPO);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void txtCSNGUNG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                int csn = txtCSNGUNG.Text.Trim() != "" ? Int32.Parse(txtCSNGUNG.Text.Trim())  : 0 ;
                int csd = lblCSDAU.Text.Trim() != "" ? Int32.Parse(lblCSDAU.Text.Trim())  : 0 ;

                txtTRUYTHU.Text = (csn - csd).ToString();
                txtTRUYTHU.Focus();

                upnlThongTin.Update();
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

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    //dt = new ReportClass().BienKHPo("", makvpo, "", "",
                    //    Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                    //                        "DSTHAYDHSINHH").Tables[0];

                    dt = new ReportClass().BienKHPo("", makvpo, "", "",
                        Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                                            "DSTHAYDHSINHHCD").Tables[0];                    
                }
                else if (_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN == "DDP7D1")
                {                        
                    dt = new ReportClass().BienKHPo("", makvpo, ddlDOTGCS.SelectedValue, "",
                        Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                                            "DSTDHSINHHPOP7D1").Tables[0];   
                }
                else
                {
                    dt = new ReportClass().BienKHPo("", makvpo, ddlDOTGCS.SelectedValue, "",
                        Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                                            "DSTDHSINHHPODOTINCD").Tables[0];
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDH" + ddlTHANG1.Text.Trim() + txtNAM1.Text.Trim().Substring(2, 2) + ".xls");
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
                upnlThongTin.Update();    
            }
            catch { }
        }
        
        protected void btDSChuaThay_Click(object sender, EventArgs e)
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

                if (ddlDOTGCS.SelectedValue == "%")
                {      
                    dt = new ReportClass().BienKHPo("", makvpo, "", "",
                        Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                                            "DSNOTHAYDHSINHHCD").Tables[0];
                }
                else if (_diDao.Get(ddlDOTGCS.SelectedValue).MADOTIN == "DDP7D1")
                {
                    dt = new ReportClass().BienKHPo("", makvpo, ddlDOTGCS.SelectedValue, "",
                        Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                                            "DSNOTDHPOP7D1").Tables[0];
                }
                else
                {
                    dt = new ReportClass().BienKHPo("", makvpo, ddlDOTGCS.SelectedValue, "",
                        Convert.ToInt32(ddlTHANG1.SelectedValue), Convert.ToInt32(txtNAM1.Text.Trim()),
                                            "DSNOTDHPODOTINCD").Tables[0];
                }

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=TDH" + ddlTHANG1.Text.Trim() + txtNAM1.Text.Trim().Substring(2, 2) + ".xls");
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
                upnlThongTin.Update();    
            }
            catch { }
        }

        protected void ddlLYDOTHAYDH_SelectedIndexChanged(object sender, EventArgs e)
        {

        }


    }
}