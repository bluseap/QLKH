using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util ;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Web.UI;
using System.Data;
using CrystalDecisions.Shared;

using System.IO;
using System.Web;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class ThayDongHo : Authentication
    {
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly TrangThaiGhiDao _ttghiDao = new TrangThaiGhiDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly LoaiDongHoDao _loaiDhDao = new LoaiDongHoDao();
        private readonly ThayDongHoDao _thaydonghoDao = new ThayDongHoDao();
        private readonly DongHoDao dhDao = new DongHoDao();
        private readonly ReportClass rp = new ReportClass();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ApToDao _atDao = new ApToDao();

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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
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
                Authenticate(Functions.KH_ThayDongHo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_KH_THAYDONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_THAYDONGHO;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
        }

        private void LoadStaticReferences()
        {
            try
            {               
                timkv();

                //xa phuong
                var xaphuong = _xpDao.GetListKV(ddlKHUVUC.SelectedValue);
                ddlXAPHUONG.Items.Clear();
                ddlXAPHUONG.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var xp in xaphuong)
                {
                    ddlXAPHUONG.Items.Add(new ListItem(xp.TENXA, xp.MAXA));
                }
                //Ap khóm
                var apkhom = _atDao.GetList(ddlKHUVUC.SelectedValue, ddlXAPHUONG.SelectedValue);
                ddlAPKHOM.Items.Clear();
                ddlAPKHOM.Items.Add(new ListItem("Tất cả", "%"));

                var loaiDhList = _loaiDhDao.GetList();
                cboLoaiDh.Items.Clear();
                cboLoaiDh.DataTextField = "MALDH";
                cboLoaiDh.DataValueField = "MALDH";
                cboLoaiDh.DataSource = loaiDhList;
                cboLoaiDh.DataBind();

                ClearForm();
                //txtNAM.Text = DateTime.Now.Year.ToString();
                //ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                //txtNgayThay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");

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

                if (_nvDao.Get(b).MAKV == "X")
                {
                    txtNgayHoanThanh.Enabled = false;
                }
                else
                {
                    txtNgayHoanThanh.Enabled = true;
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

        //private void BindKhachHangThayDH()
        //{
        //    var danhsach = _khDao.SearchKhachHangThayDH(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(), txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
        //                                                   txtSONHA.Text.Trim(), txtTENDP.Text.Trim(), ddlKHUVUC.SelectedValue.Trim(),
        //                                                   int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()),ddlTrangThaiGhi.SelectedValue);
        //    gvDanhSach.DataSource = danhsach;
        //    gvDanhSach.PagerInforText = danhsach.Count.ToString();
        //    cpeFilter.Collapsed = true;
        //    gvDanhSach.DataBind();
        //    tdDanhSach.Visible = true;

        //    upnlKhachHang.Update();
        //}

        private void BindKhachHang()
        {
            if (ddlTrangThaiGhi.SelectedValue == "%")
            {
                var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),  txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                        txtSONHA.Text.Trim(), txtTENDP.Text.Trim(), ddlKHUVUC.SelectedValue.Trim());

                gvDanhSach.DataSource = danhsach;
                gvDanhSach.PagerInforText = danhsach.Count.ToString();
                cpeFilter.Collapsed = true;
                gvDanhSach.DataBind();
                tdDanhSach.Visible = true;
            }
            else
            {
                //var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),  txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                //txtSONHA.Text.Trim(), txtTENDP.Text.Trim(), ddlKHUVUC.SelectedValue.Trim());

                var danhsach = _khDao.SearchKhachHangThayDH(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(), txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
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
            //BindKhachHangThayDH();
        }

        private void BindStatus(KHACHHANG kh)
        {
            var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));

            txtSODB.Text = (kh.MADP + kh.DUONGPHU + kh.MADB).ToString();
            lblTENKH.Text = kh.TENKH.ToString();           
            lblIDKH.Text = kh.IDKH;
            lblTENDP.Text = kh.DUONGPHO != null ? kh.DUONGPHO.TENDP.ToString() : "";
            lblTENKV.Text = kh.KHUVUC != null ? kh.KHUVUC.TENKV.ToString() : "";            
            lblLOAITK.Text = kh.MALDH != null ? kh.MALDH : "";

            txtCSMOI.Text = tieuthu.CHISOCUOI.ToString();

            if (dhDao.Get(kh.MADH) != null)
            {
                if (dhDao.Get(kh.MADH).SONO != null)
                { 
                    lblSONO.Text = dhDao.Get(kh.MADH).SONO.ToString(); 
                }
                else { lblSONO.Text = ""; }
            }
            else {lblSONO.Text ="";}

            if (kh.THUYLK != null)
            {
                ddlKICHCODH.Text = kh.THUYLK.ToString();
            }
            else
            {
                ddlKICHCODH.Text = "15";
            }

            var tieuthukyhientai = _ttDao.GetTNKyTruoc(kh.IDKH, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));
            if (int.Parse(ddlTHANG1.SelectedValue) != 1)
            {
                var tieuthukytruoc = _ttDao.GetTNKyTruoc(kh.IDKH, int.Parse(ddlTHANG1.SelectedValue) - 1, int.Parse(txtNAM1.Text.Trim()));                

                lblCSDAU.Text = tieuthukytruoc.CHISOCUOI != null ? tieuthukytruoc.CHISOCUOI.ToString() : "0";
                lblCSCUOI.Text = tieuthukyhientai.CHISOCUOI != null ? tieuthukyhientai.CHISOCUOI.ToString() : "0";
            }
            else
            {
                var tieuthukytruoc = _ttDao.GetTNKyTruoc(kh.IDKH, 12, int.Parse(txtNAM1.Text.Trim()) - 1);

                lblCSDAU.Text = tieuthukytruoc.CHISOCUOI != null ? tieuthukytruoc.CHISOCUOI.ToString() : "0";
                lblCSCUOI.Text = tieuthukyhientai.CHISOCUOI != null ? tieuthukyhientai.CHISOCUOI.ToString() : "0";
            }

            lblNGAYTHAY.Text = kh.NGAYTHAYDH != null ? kh.NGAYTHAYDH.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("11/11/2011");
            lblNGAYHOANTHANH.Text = kh.NGAYHT != null ? kh.NGAYHT.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("11/11/2011");

            upnlThongTin.Update();
        }

        private void BindStatusTDH(KHACHHANG kh)
        {
            var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));

            txtSODB.Text = kh.MADP + kh.MADB;
            lblTENKH.Text = kh.TENKH;           
            lblIDKH.Text = kh.IDKH;
            lblTENDP.Text = kh.DUONGPHO != null ? kh.DUONGPHO.TENDP : "";
            lblTENKV.Text = kh.KHUVUC != null ? kh.KHUVUC.TENKV : "";
            lblNGAYTHAY.Text = kh.NGAYTHAYDH.HasValue ? kh.NGAYTHAYDH.Value.ToString("dd/MM/yyyy") : "";
            lblNGAYHOANTHANH.Text = kh.NGAYHT.HasValue ? kh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";

            var idmadt = _dpDao.GetDP(kh.MADP);
            var dotin = ddlDOTGCS.Items.FindByValue(idmadt.IDMADOTIN);
            if (dotin != null)
                ddlDOTGCS.SelectedIndex = ddlDOTGCS.Items.IndexOf(dotin);

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
                        var khachhang = _khDao.Get(id);

                        if (khachhang != null)
                        {
                            var thaydongho = _thaydonghoDao.GetIDKH(int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()), id);

                            if (thaydongho != null)
                            {
                                ShowError("Khách hàng đã nhập thay đồng trong kỳ rồi. Kiểm tra lại.");
                                HideDialog("divKhachHang");
                                CloseWaitingDialog();
                            }
                            else
                            {

                                BindStatus(khachhang);
                                HideDialog("divKhachHang");
                                CloseWaitingDialog();
                                txtSODB.Focus();
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
            lblCSDAU.Text = "";
            lblCSCUOI.Text = ""; 

            txtCSNGUNG.Text = "0";
            txtTRUYTHU.Text = "0";
            txtMaDongho.Text = "";            
            txtCSBATDAU.Text = "0";
            txtCSMOI.Text = "0";
            txtGhiChu.Text = "";            
            txtNgayThay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");
            btnSaveUp.Visible = false;
            btnSave.Visible = true;
            ddlDONGHOCAPBAN.SelectedIndex = 0;

            ddlLYDOTHAYDH.SelectedIndex = 0;
            lbSONODH.Text = "";
            lbCONGSUATLX.Text = "";            
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
            
            //bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
            var kh = _khDao.Get(lblIDKH.Text.Trim());
            string h = lblIDKH.ClientID;
            string madhkh = kh.MADH;
            if (kh == null)
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                return;
            } 

            int thangF = Convert.ToInt16(ddlTHANG1.SelectedValue);
            int namF = Convert.ToInt32(txtNAM1.Text.Trim());
            var kynayF = new DateTime(namF, thangF, 1);

            //bool dungdotin = _gcspoDao.IsLockDotInHD(kynayF, query.MAKV.ToString(), kh.IDMADOTIN != null ? kh.IDMADOTIN : "");
            bool dungdotin = _gcspoDao.IsLockDotIn(kh.IDMADOTIN != null ? kh.IDMADOTIN : "", kynayF, query.MAKV.ToString());
            
            if (dungdotin == true)
            {
                CloseWaitingDialog();
                ShowInfor("Đã khoá sổ kỳ nhờ thu.");
                return;
            }

            //if (query.MAKV == "L" || query.MAKV == "M" || query.MAKV == "Q")//tri ton - tinh bien - an phu
            //{
            //    bool dungdotin = _gcspoDao.IsLockDotInHD(kynayF, query.MAKV.ToString(), kh.IDMADOTIN);
            //    if (dungdotin == true)
            //    {
            //        CloseWaitingDialog();
            //        ShowInfor("Đã khoá sổ kỳ thu hộ, ghi chỉ số.");
            //        return;
            //    }
            //}

            bool dung = _gcsDao.IsLockTinhCuocKy1(kynayF, query.MAKV.ToString(), kh.MADP);
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

            if (_nvDao.Get(b).MAKV != "X")
            {
                if (ddlDONGHOCAPBAN.SelectedValue.Equals("KO"))
                {
                    CloseWaitingDialog();
                    ShowInfor("Kiểm tra lại đồng hồ cấp hay bán.");
                    return;
                }
            }

            if (!HasPermission(Functions.KH_ThayDongHo , Permission.Insert))
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
                //var kh = _khDao.Get(lblIDKH.Text.Trim());
                //string h = lblIDKH.ClientID;
                //string madhkh = kh.MADH;
                //if(kh == null)
                //{
                //    CloseWaitingDialog();
                //    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                //    return;
                //}                

                kh.THUYLK = ddlKICHCODH.SelectedValue;
                kh.MALDH = cboLoaiDh.Items.Count > 0 ?  cboLoaiDh.SelectedValue : null;                
                kh.MADH = txtMaDongho.Text.Trim();

                var madh = dhDao.GetDASD(kh.MADH);
                if (madh != null)
                {
                    ShowError("Đồng hồ đã sử dụng. Kiểm tra lại.");
                    CloseWaitingDialog();
                    return;
                }

               // var msgdh = dhDao.UpdateDASDDH(txtMaDongho.Text.Trim());

                kh.NGAYTHAYDH = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());//ngay thay moi

                if (query.MAKV == "X")
                {
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());//ngay thay moi
                }
                else
                {
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());//ngay hoan thanh moi
                }

                kh.CHISODAU = Convert.ToInt32(txtCSNGUNG.Text.Trim());
                kh.CHISOCUOI = Convert.ToInt32(txtCSBATDAU.Text.Trim());
                kh.m4Poor = Convert.ToInt32(txtTRUYTHU.Text.Trim());
                kh.KLKHOAN = Convert.ToInt32(txtCSMOI.Text.Trim());
                
                string lydothay = ddlLYDOTHAYDH.SelectedItem.ToString() + ": ";
                string malydothay = ddlLYDOTHAYDH.SelectedValue == "V" ? "5" : ddlLYDOTHAYDH.SelectedValue.ToString();

                kh.DIACHI_INHOADON = lydothay + malydothay;
                //kh.DIACHI_INHOADON = ddlLYDOTHAYDH.SelectedItem + ": " + (ddlLYDOTHAYDH.SelectedValue == "V" ? "5" : ddlLYDOTHAYDH.SelectedValue.ToString());

                var msg = _khDao.UpdateThayDongHoKyForm(kh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV,
                                                lblSONO.Text.Trim(), lblLOAITK.Text.Trim(),DateTimeUtil.GetVietNamDate(lblNGAYTHAY.Text.Trim()),
                                                DateTimeUtil.GetVietNamDate(lblNGAYHOANTHANH.Text.Trim()), madhkh, ddlDONGHOCAPBAN.SelectedValue, txtMATRANGTHAI.Text.Trim(),
                                                ddlXAPHUONG.SelectedValue, ddlAPKHOM.SelectedValue,kynayF);

                //rp.UpdateTieuThuDH(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), Convert.ToInt32(txtTRUYTHU.Text.Trim()));

                rp.UpdateTieuThuDHMoi(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()),
                    Convert.ToInt32(txtTRUYTHU.Text.Trim()), thangF, namF, "INTDHTOTT");

                //rp.UpdateTieuThuDHMoi2(lblIDKH.Text.Trim(), txtMaDongho.Text.Trim(), "", Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()),
                //    Convert.ToInt32(txtTRUYTHU.Text.Trim()), thangF, namF, "INTDHTOTT2");

                CloseWaitingDialog();
                ClearForm();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    //rp.UpdateTieuThuDHMoi(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()),
                    //    Convert.ToInt32(txtTRUYTHU.Text.Trim()), thangF, namF, "INTDHTOTT");

                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();

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
            //int thang1 = DateTime.Now.Month;
            //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            //var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            int thang1 = Convert.ToInt32(ddlTHANG1.SelectedValue);
            string nam = txtNAM1.Text.Trim().ToString(CultureInfo.InvariantCulture);
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);

            //var kynay = new DateTime(2013, 6, 1);
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            //bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
            var dh = _thaydonghoDao.Get(int.Parse(lblID.Text.Trim()));
            string h = lblIDKH.ClientID;
            if (dh == null)
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                return;

            }

            if (query.MAKV == "L" || query.MAKV == "M" || query.MAKV == "Q")//tri ton - tinh bien - an phu
            {
                bool dungdotin = _gcspoDao.IsLockDotInHD(kynay1, query.MAKV.ToString(), _khDao.Get(dh.IDKH).IDMADOTIN);
                if (dungdotin == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ kỳ thu hộ, ghi chỉ số.");
                    return;
                }
            }

            bool dung = _gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(),  _khDao.Get(dh.IDKH).MADP);
            if (dung == true || string.IsNullOrEmpty(txtMaDongho.Text.Trim()))
            {
                CloseWaitingDialog();
                ClearForm();
                BindGrid();
                ShowError("Đã khoá sổ ghi chỉ số.");
                return;
            }

            if (_nvDao.Get(b).MAKV != "X")
            {
                if (ddlDONGHOCAPBAN.SelectedValue.Equals("KO"))
                {
                    CloseWaitingDialog();
                    ShowInfor("Kiểm tra lại đồng hồ cấp hay bán.");
                    return;
                }
            }

            if (!HasPermission(Functions.KH_ThayDongHo, Permission.Update))
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
                //var dh = _thaydonghoDao.Get(int.Parse(lblID.Text.Trim()));
                //string h = lblIDKH.ClientID;
                //if (dh == null)
                //{
                //    CloseWaitingDialog();
                //    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                //    return;
                //}

                if (dh.MADH != txtMaDongho.Text.Trim())
                {
                    var madhsua = dhDao.Get(dh.MADH);
                    var msgdhsua = dhDao.UpdateKoSD(madhsua);
                }

                dh.CHISONGUNG = Convert.ToInt32(txtCSNGUNG.Text.Trim());
                dh.MTRUYTHU = Convert.ToInt32(txtTRUYTHU.Text.Trim());
                dh.MALDH = cboLoaiDh.Items.Count > 0 ? cboLoaiDh.SelectedValue : null;
                dh.MADH = txtMaDongho.Text.Trim();
                dh.CHISOBATDAU = Convert.ToInt32(txtCSBATDAU.Text.Trim());
                dh.CHISOMOI = Convert.ToInt32(txtCSMOI.Text.Trim());
                dh.NGAYTD = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());

                if (_nvDao.Get(b).MAKV == "X")
                {
                    dh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                }
                else
                {
                    dh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                }

                dh.KICHCO = ddlKICHCODH.SelectedValue;
                dh.DHCAPBAN = ddlDONGHOCAPBAN.SelectedValue;
                dh.MATRANGTHAI = txtMATRANGTHAI.Text.Trim();
                dh.MAXA = ddlXAPHUONG.SelectedValue;
                dh.MAAPTO = ddlAPKHOM.SelectedValue;

                string lydothay = ddlLYDOTHAYDH.SelectedItem.ToString() + ": ";
                string malydothay = ddlLYDOTHAYDH.SelectedValue == "V" ? "5" : ddlLYDOTHAYDH.SelectedValue.ToString();

                dh.LYDOTHAY = lydothay + malydothay;
                //dh.LYDOTHAY = ddlLYDOTHAYDH.SelectedItem + ": " + ddlLYDOTHAYDH.SelectedValue == "V" ? "5" : ddlLYDOTHAYDH.SelectedValue;

                var msg = _thaydonghoDao.UpThayDongHo(dh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                var msg1 = _khDao.UpThayDongHo(dh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);


                rp.UpdateTieuThuDHMoi(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), 
                    Convert.ToInt32(txtTRUYTHU.Text.Trim()), dh.KYTHAYDH.Value.Month,dh.KYTHAYDH.Value.Year,"UPTDHTOTT");

                //rp.UpdateTieuThuDHMoi(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()),
                //        Convert.ToInt32(txtTRUYTHU.Text.Trim()), thangF, namF, "INTDHTOTT");

                CloseWaitingDialog();
                ClearForm();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();

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
            txtIDKH.Text = txtSODB.Text.Trim();
            BindKhachHang();
            //upnlKhachHang.Update();
            CloseWaitingDialog();
        }
        
        private void BindGrid()
        {           
            int thang = int.Parse(ddlTHANG1.SelectedValue);
            int nam = int.Parse(txtNAM1.Text.Trim());

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var nhanvien = _nvDao.Get(b);

            if (nhanvien.MAKV == "X")
            {
                //var list = _khDao.GetThayDongHoListThangNamKV(thang, nam, _nvDao.Get(b).MAKV);
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var list = _khDao.GetThayDongHoListThangNamKVSortMADPDB(thang, nam, _nvDao.Get(b).MAKV);

                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }
                else
                {
                    var list = _khDao.GetThayDongHoListDotInMSortMADPDB(thang, nam, _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue);
                    if (list != null)
                    {
                        gvKhachHang.DataSource = list;
                        gvKhachHang.PagerInforText = list.Count.ToString();
                        gvKhachHang.DataBind();
                    }                    
                }
            }
            else
            {
                //var list = _khDao.GetThayDongHoListThangNamKV(thang, nam, _nvDao.Get(b).MAKV);
                if (ddlDOTGCS.SelectedValue == "%")
                {
                    var list = _khDao.GetThayDongHoListThangNamKV(thang, nam, _nvDao.Get(b).MAKV);

                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }                
                else
                {
                    var dotinhd = _diDao.Get(ddlDOTGCS.SelectedValue);
                    if (dotinhd.MADOTIN == "NNNTD1")
                    {
                        var list = _khDao.GetThayDongHoListDotInThuHo(thang, nam, _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue);
                        if (list != null)
                        {
                            gvKhachHang.DataSource = list;
                            gvKhachHang.PagerInforText = list.Count.ToString();
                            gvKhachHang.DataBind();
                        }
                    }
                    else
                    {
                        var list = _khDao.GetThayDongHoListDotInM(thang, nam, _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue);
                        if (list != null)
                        {
                            gvKhachHang.DataSource = list;
                            gvKhachHang.PagerInforText = list.Count.ToString();
                            gvKhachHang.DataBind();
                        }
                    }
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

        private void BindTDH(THAYDONGHO thaydh)
        {
            try
            {
                lblLOAITK.Text = thaydh.MALDHCU;
                lblSONO.Text = thaydh.MADHCU;
                txtCSNGUNG.Text = thaydh.CHISONGUNG.ToString();
                txtTRUYTHU.Text = thaydh.MTRUYTHU.ToString();
                cboLoaiDh.SelectedValue = thaydh.MALDH;
                txtMaDongho.Text = thaydh.MADH;
                lbSONODH.Text = thaydh.MADH != null ? dhDao.Get(thaydh.MADH).SONO : "";
                txtCSBATDAU.Text = thaydh.CHISOBATDAU.ToString();
                txtCSMOI.Text = thaydh.CHISOMOI.ToString();
                txtNgayThay.Text = thaydh.NGAYTD.HasValue ? thaydh.NGAYTD.Value.ToString("dd/MM/yyyy") : "";
                txtNgayHoanThanh.Text = thaydh.NGAYHT.HasValue ? thaydh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
                ddlKICHCODH.SelectedValue = thaydh.KICHCO;
                txtGhiChu.Text = thaydh.GHICHU;
                var capban = ddlDONGHOCAPBAN.Items.FindByValue(thaydh.DHCAPBAN);
                if (capban != null)
                    ddlDONGHOCAPBAN.SelectedIndex = ddlDONGHOCAPBAN.Items.IndexOf(capban);

                txtMATRANGTHAI.Text = thaydh.MATRANGTHAI != null ? thaydh.MATRANGTHAI : "";

                var maxa = ddlXAPHUONG.Items.FindByValue(thaydh.MAXA);
                if (maxa != null)
                    ddlXAPHUONG.SelectedIndex = ddlXAPHUONG.Items.IndexOf(maxa);
                var maap = ddlAPKHOM.Items.FindByValue(thaydh.MAAPTO);
                if (maap != null)
                    ddlAPKHOM.SelectedIndex = ddlAPKHOM.Items.IndexOf(maap);

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
            catch { }
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                var tdh = _thaydonghoDao.Get(int.Parse(id));

                switch (e.CommandName)
                {
                    case "SelectTDH":
                        var khachhang = _khDao.Get(tdh.IDKH);
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
                if (!HasPermission(Functions.KH_ThayDongHo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<THAYDONGHO>();
                    var listIds = strIds.Split(',');

                   
                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _thaydonghoDao.Get(int.Parse(ma))));

                    //TODO: check relation before update list
                    var msg = _thaydonghoDao.DoAction(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

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
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            //var list = dhDao.GetListDASD(txtKeywordDHSONO.Text.Trim());
            var list = dhDao.GetListDASDKV(txtKeywordDHSONO.Text.Trim(), _nvDao.Get(b).MAKV);

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
                        var dh = dhDao.Get(id);
                        if (dh != null)
                        {
                            //SetControlValue(txtMaDongho.ClientID, dh.MADH);
                            txtMaDongho.Text = dh.MADH.ToString();
                            //SetControlValue(txtMALDH.ClientID, dh.MALDH);
                            lbSONODH.Text = dh.SONO.ToString();
                            lbCONGSUATLX.Text = dh.CONGSUAT != null ? dh.CONGSUAT.ToString() : "";

                            var maldh = cboLoaiDh.Items.FindByValue(dh.MALDH);
                            if (maldh != null)
                                cboLoaiDh.SelectedIndex = cboLoaiDh.Items.IndexOf(maldh);
                            //SetLabel(lblKICHCO.ClientID, ldhDao.Get(dh.MALDH).ToString());
                            /*var ldhkc = _loaiDhDao.GetListldh(dh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetLabel(lblKICHCO.ClientID, kc.KICHCO);
                            }*/
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

                if (dateht.Length != 10 && dateht.Length==6)
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
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        protected void ddlXAPHUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //Ap khóm
                var apkhom = _atDao.GetList(ddlKHUVUC.SelectedValue, ddlXAPHUONG.SelectedValue);
                ddlAPKHOM.Items.Clear();
                //ddlAPKHOM.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var ak in apkhom)
                {
                    ddlAPKHOM.Items.Add(new ListItem(ak.TENAPTO, ak.MAAPTO));
                }
            }
            catch { }
        }

        protected void txtCSNGUNG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (_nvDao.Get(b).MAKV != "O")
                {
                    int csn = txtCSNGUNG.Text.Trim() != "" ? Int32.Parse(txtCSNGUNG.Text.Trim()) : 0;
                    int csd = lblCSDAU.Text.Trim() != "" ? Int32.Parse(lblCSDAU.Text.Trim()) : 0;

                    txtTRUYTHU.Text = (csn - csd).ToString();
                    txtTRUYTHU.Focus();
                    upnlThongTin.Update();
                }
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

                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlTHANG1.Text.Trim()) + "/" + int.Parse(txtNAM1.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                    var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                        int.Parse(ddlTHANG1.Text.Trim()), int.Parse(txtNAM1.Text.Trim()), "DSTHAYDHNLX");
                    dt = ds.Tables[0];
                }
                else
                {
                    var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                       int.Parse(ddlTHANG1.Text.Trim()), int.Parse(txtNAM1.Text.Trim()), "DSTHAYDHNLX");
                    dt = ds.Tables[0];
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
                string b = loginInfo.Username;

                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlTHANG1.Text.Trim()) + "/" + int.Parse(txtNAM1.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));

                DataTable dt;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    //var ds = new ReportClass().dsKHCBiKT(cboKhuVuc.Text.Trim(), "", "dsKHMOI_ExLX2", TuNgay, DenNgay);
                    var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                        int.Parse(ddlTHANG1.Text.Trim()), int.Parse(txtNAM1.Text.Trim()), "DSNOTHAYDHNLX");
                    dt = ds.Tables[0];
                }
                else
                {
                    var ds = new ReportClass().BienKHNuoc("", _nvDao.Get(b).MAKV, ddlDOTGCS.SelectedValue, "",
                       int.Parse(ddlTHANG1.Text.Trim()), int.Parse(txtNAM1.Text.Trim()), "DSNOTHAYDHNLX");
                    dt = ds.Tables[0];
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

    }
}