using System;
using System.Web.UI.WebControls;
using System.Globalization;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class NhapKHPower : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly ApToDao _atDao = new ApToDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly LoaiDongHoPoDao _ldhpoDao = new LoaiDongHoPoDao();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly DanhSachCoQuanThanhToanDao cqDao = new DanhSachCoQuanThanhToanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly DMDotInHDDao _dmdihdDao = new DMDotInHDDao();
        private readonly DotInHDDao _dihdDao = new DotInHDDao();

        private KHACHHANGPO KhachHangPo
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var kh = string.IsNullOrEmpty(hdfIDKH.Value) ? new KHACHHANGPO() : _khpoDao.Get(hdfIDKH.Value);
                if (kh == null)
                    return null;

                kh.MADBPO = txtMADB.Text.Trim();
                kh.MADPPO = txtMADP.Text.ToUpper().Trim();
                kh.DUONGPHUPO = txtDUONGPHU.Text.Trim();
                //kh.MALKHDB = ddlLKHDB.SelectedValue;
                kh.MALKHDB = "D";
                kh.MAMDSDPO = ddlMDSD.SelectedValue;
                kh.MADDKPO = lbMADDK.Text.Trim();
                kh.SOHD = txtSOHD.Text.Trim();
                kh.MABG = ddlMAGIA.SelectedValue.Equals("NULL") ? null : ddlMAGIA.SelectedValue;
                kh.MAHOTRO = ddlHOTRO.SelectedValue;
                //kh.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : ddlPHUONG.SelectedValue;
                //kh.MAPHUONGPO = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : txtMADP.Text.ToUpper().Trim().Substring(1, 1);
                kh.MAPHUONGPO = String.IsNullOrEmpty(txtMADP.Text.ToUpper().Trim()) ? null : txtMADP.Text.ToUpper().Trim().Substring(1, 1);

                kh.TENKH = txtTENKH.Text.Trim();

                kh.SONHA2 = txtSONHA2.Text.Trim();
                kh.SONHA = txtDIACHILD.Text.Trim();

                kh.SOTRU = lbSoTruThietKe.Text.Trim();
                kh.SOTRUKD = txtSOTRUKD.Text.Trim();                

                kh.MST = txtMSTHUE.Text.Trim();
                kh.MAHTTT = ddlHTTT.SelectedValue;
                kh.STK = txtSOTK.Text.Trim();
                kh.SDT = txtSDT.Text.Trim();
                kh.TTSD = "";
                kh.KOPHINT = false;
                kh.THUYLK = ddlKICHCODH.SelectedValue;
                kh.NGAYGNHAP = DateTime.Now;
                //kh.ISDINHMUC = cbISDINHMUC.Checked;
                kh.ISDINHMUC = false;
                kh.VAT = true;
                //kh.KHONGTINH117 = cbKHONGTINH117.Checked;
                kh.KHONGTINH117 = false;
                //kh.MATT = ddlTT.SelectedValue;
                kh.MATT = "GDH_BT";

                string kykt = "01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
                //string kykt = "11/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim();
                kh.KYKHAITHAC = DateTimeUtil.GetVietNamDate(kykt);

                //kh.GHI2THANG1LAN = ddlGHI2THANG1LAN.SelectedValue;
                kh.THUHO = ddlTHUHO.SelectedValue;

                var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (dp == null)
                    return null;

                kh.MAKVPO = dp.MAKVPO;
                //kh.MAKVDN = ddlKHUVUCDN.SelectedValue;
                kh.MAKVDN = "O";

                if (!string.IsNullOrEmpty(txtCQ.Text.Trim()))
                    kh.MACQ = txtCQ.Text.Trim();

                //if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
                //    kh.SOHO = decimal.Parse(txtSOHO.Text.Trim());
                kh.SOHO = 1;

                //if (!string.IsNullOrEmpty(txtSONK.Text.Trim()))
                //    kh.SONK = int.Parse(txtSONK.Text.Trim());
                kh.SONK = 1;

                /*string dateht = txtNGAYHT.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);
                string dateht1 = ngayht + "/" + thanght + "/20" + namht;*/

                if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim()))
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());

                if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
                    kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());

                if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
                    kh.MALDHPO = txtMALDH.Text.Trim();

                if (!string.IsNullOrEmpty(txtMADH.Text.Trim()))
                    kh.MADHPO = txtMADH.Text.Trim();

                //if (!string.IsNullOrEmpty(txtM3.Text.Trim()))
                //    kh.KLKHOAN = int.Parse(txtM3.Text.Trim());
                kh.KLKHOAN = 4;

                if (!string.IsNullOrEmpty(txtSTT.Text.Trim()))
                    kh.STT = int.Parse(txtSTT.Text.Trim());

                kh.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked)
                {
                    kh.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    kh.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                }

                //if (!string.Empty.Equals(txtNAM.Text.Trim()))
                //    kh.NAMBDKT = int.Parse(txtNAM.Text.Trim());
                kh.NAMBDKT = DateTime.Now.Year;
                //kh.THANGBDKT = int.Parse(ddlTHANG.SelectedValue);
                kh.THANGBDKT = DateTime.Now.Month;
                //if (!string.Empty.Equals(txtCHISODAU.Text.Trim()))
                //    kh.CHISODAU = decimal.Parse(txtCHISODAU.Text.Trim());

                //kh.CHISODAU = txtCHISODAU;
                if (!string.Empty.Equals(txtCHISODAU.Text.Trim()))
                    kh.CHISODAU = decimal.Parse(txtCHISODAU.Text.Trim());

                if (!string.Empty.Equals(txtCHISOCUOI.Text.Trim()))
                    kh.CHISOCUOI = decimal.Parse(txtCHISOCUOI.Text.Trim());

                if (!string.Empty.Equals(txtChiSoTruyThu.Text.Trim()))
                    kh.MTRUYTHU = decimal.Parse(txtChiSoTruyThu.Text.Trim());

                kh.DOTINHD = ddlDOTINHD.SelectedValue;

                

                kh.XOABOKHPO = false;

                kh.SKU = txtSKU.Text.Trim();
                kh.DBO = txtDBO.Text.Trim();

                kh.SODINHMUC = 1;
                return kh;
            }

            set
            {
                if (value == null)
                    return;

                ClearForm();

                hdfIDKH.Value = value.IDKHPO;

                txtMADB.Text = value.MADBPO;
                txtMADP.Text = value.MADPPO;
                txtDUONGPHU.Text = value.DUONGPHUPO;
                lblTENDUONG.Text = value.DUONGPHOPO.TENDP;

                var item = ddlLKHDB.Items.FindByValue(value.MALKHDB);
                if (item != null)
                    ddlLKHDB.SelectedIndex = ddlLKHDB.Items.IndexOf(item);

                var cq = cqDao.Get(value.MACQ);
                if (cq != null)
                {
                    txtCQ.Text = cq.MACQ;
                    lblTENCQ.Text = cq.TENCQ;
                }

                var item1 = ddlMDSD.Items.FindByValue(value.MAMDSDPO);
                if (item1 != null)
                    ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(item1);

                txtSOHD.Text = value.SOHD;

                var kv = _kvpoDao.Get(value.MAKVPO);

                var item5 = ddlKHUVUC.Items.FindByValue(value.MAKVPO);
                if (item5 != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item5);

                var item9 = ddlKHUVUCDN.Items.FindByValue(value.MAKVDN);
                if (item9 != null)
                    ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);

                /**
                 * re-bind phuong
                 * 
                 */
                LoadDynamicReferences(kv);

                /*var item2 = ddlPHUONG.Items.FindByValue(value.MAPHUONGPO);
                if (item2 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item2);
                */
                txtTENKH.Text = value.TENKH;
                txtDIACHILD.Text = value.SONHA;

                txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;

                txtMSTHUE.Text = value.MST;

                var item3 = ddlPHUONG.Items.FindByValue(value.MAPHUONGPO);
                if (item3 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item3);

                var item4 = ddlHTTT.Items.FindByValue(value.MAHTTT);
                if (item4 != null)
                    ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(item4);

                //var g2t1l = ddlGHI2THANG1LAN.Items.FindByValue(value.GHI2THANG1LAN);
                //if (g2t1l != null)
                //{
               //     ddlGHI2THANG1LAN.SelectedIndex = ddlGHI2THANG1LAN.Items.IndexOf(g2t1l);
                //}

                var thuho = ddlTHUHO.Items.FindByValue(value.THUHO);
                if (thuho != null)
                {
                    ddlTHUHO.SelectedIndex = ddlTHUHO.Items.IndexOf(thuho);
                }

                txtSOTK.Text = value.STK;
                txtSDT.Text = value.SDT;

                txtMALDH.Text = value.MALDHPO;
                //var dh = dhDao.Get(value.MADH);
                //lblMALDH.Text = dh != null ? dh.MALDH : "";
                txtMADH.Text = value.MADHPO;
                //cbLADHTONG.Checked = value.ISDHT.HasValue ? value.ISDHT.Value : false;
                txtNGAYHT.Text = value.NGAYHT.HasValue ? value.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
                txtKYHOTRO.Text = value.KYHOTRO.HasValue ?
                    string.Format("{0}/{1}", value.KYHOTRO.Value.ToString("MM"), value.KYHOTRO.Value.Year) : "";

                var item6 = ddlKICHCODH.Items.FindByValue(value.THUYLK);
                if (item6 != null)
                    ddlKICHCODH.SelectedIndex = ddlKICHCODH.Items.IndexOf(item6);

                cbKHONGTINH117.Checked = value.KHONGTINH117.HasValue && value.KHONGTINH117.Value;

                txtM3.Text = value.KLKHOAN.HasValue ? value.KLKHOAN.Value.ToString() : "4";

                cbSDInfo_INHOADON.Checked = value.SDInfo_INHOADON.HasValue && value.SDInfo_INHOADON.Value;
                if (cbSDInfo_INHOADON.Checked)
                {
                    txtTENKH_INHOADON.Text = value.TENKH_INHOADON;
                    txtDIACHI_INHOADON.Text = value.DIACHI_INHOADON;
                }

                var item7 = ddlTT.Items.FindByValue(value.MATT);
                if (item7 != null)
                    ddlTT.SelectedIndex = ddlTT.Items.IndexOf(item7);

                txtCHISODAU.Text = value.CHISODAU.HasValue ? value.CHISODAU.Value.ToString() : "0";
                txtCHISOCUOI.Text = value.CHISOCUOI.HasValue ? value.CHISOCUOI.Value.ToString() : "0";

                //txtLOTRINH.Text = value.LOTRINH.HasValue ? value.LOTRINH.Value.ToString() : "";
                txtSTT.Text = value.STT.HasValue ? value.STT.Value.ToString() : "";

                var item8 = ddlMAGIA.Items.FindByValue(value.MABG);
                ddlMAGIA.SelectedIndex = (item8 != null) ? ddlHTTT.Items.IndexOf(item8) : 0;
                var item10 = ddlHOTRO.Items.FindByValue(value.MAHOTRO);
                if (item10 != null)
                    ddlHOTRO.SelectedIndex = ddlHOTRO.Items.IndexOf(item10);

                txtSKU.Text = value.SKU != null ? value.SKU : "";
                txtDBO.Text = value.DBO != null ? value.DBO : "";

            }
        }

        private HOPDONGPO HopDongPo
        {
            get { return (HOPDONGPO)Session["KHACHHANG_HOPDONG"]; }
            set { Session["KHACHHANG_HOPDONG"] = value; }
        }

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
        #endregion

        private bool IsDataValid()
        {
            #region Thông tin khách hàng
            //if (!string.Empty.Equals(txtNAM.Text.Trim())) {
            //    try {
            //        DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
            //    }
            //    catch {
            //        ShowError("Năm bắt đầu kỳ khai thác không hợp lệ.", txtNAM.ClientID);
            //        return false;
            //    }
            //}

            // check độ dài mã đường phố = 3
            if (txtMADP.Text.Trim().Length != 4)
            {
                ShowError("Độ dài mã đường phố phải là 4.", txtMADP.ClientID);
                return false;
            }

            // check mã đường phố có tồn tại không
            var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            if (dp == null)
            {
                ShowError("Mã đường phố không tồn tại.", txtMADP.ClientID);
                return false;
            }

            // check độ dài mã danh bộ = 4
            if (txtMADB.Text.Trim().Length != 0 && txtMADB.Text.Trim().Length != 4)
            {
                ShowError("Độ dài mã danh bộ phải là 4.", txtMADB.ClientID);
                return false;
            }

            // check độ dài mã danh bộ = 4
            /*if (!string.Empty.Equals(txtLOTRINH.Text.Trim()))
            {
                try {
                    int.Parse(txtLOTRINH.Text.Trim());
                }
                catch {
                    ShowError("Lộ trình phải là số.", txtLOTRINH.ClientID);
                    return false;
                }
            }*/

            // check độ dài mã danh bộ = 4
            if (!string.Empty.Equals(txtSTT.Text.Trim()))
            {
                try
                {
                    int.Parse(txtSTT.Text.Trim());
                }
                catch
                {
                    ShowError("Số thứ tự phải là số.", txtSTT.ClientID);
                    return false;
                }
            }

            // check tên khách hàng
            if (string.Empty.Equals(txtTENKH.Text))
            {
                ShowError("Tên khách hàng không được rỗng.", txtTENKH.ClientID);
                return false;
            }

            // check tên khách hàng
            if (!string.Empty.Equals(txtCQ.Text))
            {
                var nh = cqDao.Get(txtCQ.Text.Trim());
                if (nh == null)
                {
                    ShowError("Cơ quan thanh toán không hợp lệ.", txtCQ.ClientID);
                    return false;
                }
            }

            // check mã đồng hồ và mã đồng hồ tổng
            if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
            {
                var ldh = _ldhpoDao.Get(txtMALDH.Text.Trim());
                if (ldh == null)
                {
                    ShowError("Loại đồng hồ không tồn tại.", txtMALDH.ClientID);
                    return false;
                }
            }

            /*
            // check mã đồng hồ và mã đồng hồ tổng
            if (!string.IsNullOrEmpty(txtMADH.Text.Trim())) {
                var dh = dhDao.Get(txtMADH.Text.Trim());
                if (dh == null) {
                    ShowError("Mã đồng hồ không tồn tại.", txtMADH.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtMADHTONG.Text.Trim())) {
                var dh = dhDao.Get(txtMADHTONG.Text.Trim());
                if (dh == null) {
                    ShowError("Mã đồng hồ tổng không tồn tại.", txtMADHTONG.ClientID);
                    return false;
                }
            }
            */

            // check số hộ là số và phải >= 1
            if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
            {
                try
                {
                    var sh = decimal.Parse(txtSOHO.Text.Trim());
                    if (sh < 1)
                    {
                        ShowError("Số hộ sử dụng tối thiểu là 1.", txtSOHO.ClientID);
                        return false;
                    }
                }
                catch
                {
                    ShowError("Số hộ sử dụng không hợp lệ.", txtSOHO.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtSONK.Text.Trim()))
            {
                try
                {
                    var sh = int.Parse(txtSONK.Text.Trim());
                    if (sh < 1)
                    {
                        ShowError("Số nhân khẩu tối thiểu là 1.", txtSONK.ClientID);
                        return false;
                    }
                }
                catch
                {
                    ShowError("Số nhân khẩu không hợp lệ.", txtSONK.ClientID);
                    return false;
                }
            }

            // check khối lượng khoán tối thiểu là số
            if (!string.IsNullOrEmpty(txtM3.Text.Trim()))
            {
                try
                {
                    decimal.Parse(txtM3.Text.Trim());
                }
                catch
                {
                    ShowError("Khối lượng khoán tối thiểu không hợp lệ.", txtM3.ClientID);
                    return false;
                }
            }
            // check ngày hoàn thành có định dạng dd/MM/yyyy
            if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                }
                catch
                {
                    //ShowError("Ngày hoàn thành không hợp lệ.", txtNGAYHT.ClientID);
                    return false;
                }
            }

            // check kỳ hỗ trợ có định dạng MM/yyyy
            if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                }
                catch
                {
                    ShowError("Kỳ hỗ trợ không hợp lệ.", txtKYHOTRO.ClientID);
                    return false;
                }
            }
            #endregion

            #region Xuất hóa đơn, thông số tiêu thụ, giá hợp đồng
            // check tên và địa chỉ xuất hóa đơn
            if (cbSDInfo_INHOADON.Checked)
            {
                if (string.IsNullOrEmpty(txtTENKH_INHOADON.Text.Trim()))
                {
                    ShowError("Tên xuất hóa đơn không được rỗng.", txtTENKH_INHOADON.ClientID);
                    return false;
                }
                if (string.IsNullOrEmpty(txtDIACHI_INHOADON.Text.Trim()))
                {
                    ShowError("Địa chỉ xuất hóa đơn không được rỗng.", txtDIACHI_INHOADON.ClientID);
                    return false;
                }
            }

            int namm = int.Parse(txtNGAYHT.Text.Substring(6, 4));
            if (namm <= 2010 || namm >= 2020)
            {
                ShowError("Ngày hoàn thành không dúng.", txtNGAYHT.ClientID);
                return false;
            }

            // check chỉ số đầu là số
            if (!string.IsNullOrEmpty(txtCHISODAU.Text.Trim()) ||
                !string.IsNullOrEmpty(txtCHISOCUOI.Text.Trim()))
            {
                decimal csd;
                decimal csc;
                try
                {
                    csd = decimal.Parse(txtCHISODAU.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số đầu không hợp lệ.", txtCHISODAU.ClientID);
                    return false;
                }

                try
                {
                    csc = decimal.Parse(txtCHISOCUOI.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số cuối không hợp lệ.", txtCHISOCUOI.ClientID);
                    return false;
                }

                if (csd > csc)
                {
                    ShowError("Chỉ số đầu không được lớn hơn chỉ số cuối.", txtCHISODAU.ClientID);
                    return false;
                }
            }
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_NhapMoiDien, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindKhachHangGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_NHAPMOIKHPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_NHAPMOIKHPO;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDongHo);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
            CommonFunc.SetPropertiesForGrid(gvHopDongDien);
            CommonFunc.SetPropertiesForGrid(gvCQTT);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
        }        

        private void ClearForm()
        {
            /*
             * clear phần thông tin hồ sơ
             */
            hdfIDKH.Value = "";

            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();

            txtSOHD.Text = "";
            txtSDT.Text = "";
            txtTENKH.Text = "";
            txtDIACHILD.Text = "";

            ddlKHUVUC.SelectedIndex = 0;
            var kv = _kvpoDao.Get(ddlKHUVUC.SelectedValue);
            //lblKHUVUC.Text = "";
            LoadDynamicReferences(kv);

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            txtMADB.Text = "";
            lblTENDUONG.Text = "";

            ddlHTTT.SelectedIndex = 0;
            txtSOTK.Text = "";
            txtMSTHUE.Text = "";
            txtCQ.Text = "";
            lblTENCQ.Text = "";

            txtMALDH.Text = "";
            //lblMALDH.Text = "";
            ddlKICHCODH.SelectedIndex = 0;
            txtMADH.Text = "";
            //cbLADHTONG.Checked = false;
            ddlLOAIKH.SelectedIndex = 0;
            ddlLKHDB.SelectedIndex = 0;
            ddlMDSD.SelectedIndex = 0;

            txtSOHO.Text = "1";
            txtSONK.Text = "1";
            cbISDINHMUC.Checked = false;
            txtM3.Text = "4";

            cbKHONGTINH117.Checked = false;
            txtNGAYHT.Text = DateTime.Now.ToString("dd/MM/yyyy");

            /*
             * clear phần đổi thông tin hóa đơn
             */
            cbSDInfo_INHOADON.Checked = false;
            txtTENKH_INHOADON.Text = "";
            txtDIACHI_INHOADON.Text = "";

            ddlMAGIA.SelectedIndex = 0;
            //txtLOTRINH.Text = "0";
            txtSTT.Text = "0";
            /*
             * clear thông số tiêu thụ áp dụng cho kỳ đầu tiên 
             */
            ddlTT.SelectedIndex = 0;

            txtCHISODAU.Text = "0";
            txtCHISOCUOI.Text = "0";
            txtChiSoTruyThu.Text = "0";

            ddlDOTINHD.SelectedIndex = 0;
            lblKICHCO.Text = "";

            lbSoTruThietKe.Text = "";
            txtSOTRUKD.Text = "";
            txtSONHA2.Text = "";

            ckDotInHD.Checked = false;
            ddlDOTINHD.Enabled = false;
        }

        private void LoadStaticReferences()
        {
            UpdateMode = Mode.Create;

            // load khu vuc
            var listKhuVuc = new KhuVucPoDao().GetList();
            ddlKHUVUC.Items.Clear();
            ddlKHUVUCDN.Items.Clear();

            foreach (var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                ddlKHUVUCDN.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
            }

            var listLoaiKhachHang = new LoaiKhDacBietDao().GetList();
            ddlLKHDB.DataSource = listLoaiKhachHang;
            ddlLKHDB.DataTextField = "TENLKHDB";
            ddlLKHDB.DataValueField = "MALKHDB";
            ddlLKHDB.DataBind();

            var listMucDichSuDung = new MucDichSuDungPoDao().GetList();
            ddlMDSD.DataSource = listMucDichSuDung;
            ddlMDSD.DataTextField = "TENMDSD";
            ddlMDSD.DataValueField = "MAMDSDPO";
            ddlMDSD.DataBind();

            var listHttt = new HinhThucThanhToanDao().GetList();
            ddlHTTT.DataSource = listHttt;
            ddlHTTT.DataTextField = "MOTA";
            ddlHTTT.DataValueField = "MAHTTT";
            ddlHTTT.DataBind();
                       
            timkv();
            ClearForm();
        }       

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = _kvpoDao.GetPo(a.MAKV).MAKVPO;

                if (d == "99")
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
                    var kvList = _kvpoDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }

                    var kvIn = _dihdDao.GetListKVPO(d);
                    ddlDOTINHD.Items.Clear();
                    foreach (var dotin in kvIn)
                    {
                        ddlDOTINHD.Items.Add(new ListItem(_dmdihdDao.Get(dotin.MADOTIN).TENDOTIN, dotin.IDMADOTIN));
                    }                    
                }
            }
           
        }

        private void LoadDynamicReferences(KHUVUCPO kv)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var nhanvien = _nvDao.Get(b);

            if (nhanvien.MAKV == "T" || nhanvien.MAKV == "P" || nhanvien.MAKV == "N" || nhanvien.MAKV == "O" // tan chau, phu tan,CHAU PHU, CHAU THANH
                || nhanvien.MAKV == "S" || nhanvien.MAKV == "K" || nhanvien.MAKV == "L" || nhanvien.MAKV == "M" || nhanvien.MAKV == "Q"
                 || nhanvien.MAKV == "U")
            {
                if (kv == null) return;

                // load phuong
                var listPhuong = _xpDao.GetListKV(kv.MAKV);
                //ddlPHUONG.Items.Clear();
                //ddlPHUONG.Items.Add(new ListItem("--Tất cả--", "%"));          
                ddlPHUONG.DataSource = listPhuong;                     
                ddlPHUONG.DataTextField = "TENXA";
                ddlPHUONG.DataValueField = "MAXA";
                ddlPHUONG.DataBind();                

                var item9 = ddlKHUVUCDN.Items.FindByValue(kv.MAKVPO);
                if (item9 != null)
                    ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);

                LoadApTo(kv.MAKV, ddlPHUONG.SelectedValue);
            }
            else
            {
                if (kv == null) return;

                // load phuong
                var listPhuong = _ppoDao.GetList(kv.MAKVPO);
                ddlPHUONG.DataSource = listPhuong;
                ddlPHUONG.DataTextField = "TENPHUONG";
                ddlPHUONG.DataValueField = "MAPHUONGPO";
                ddlPHUONG.DataBind();

                var item9 = ddlKHUVUCDN.Items.FindByValue(kv.MAKVPO);
                if (item9 != null)
                    ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);
            }
        }

        private void LoadApTo(string makv, string maxa)
        {
            try
            {
                var listapto = _atDao.GetList(makv, maxa);

                ddlAPTO.DataSource = listapto;
                ddlAPTO.Items.Add(new ListItem("Tất cả", "%"));
                ddlAPTO.DataTextField = "TENAPTO";
                ddlAPTO.DataValueField = "MAAPTO";
                ddlAPTO.DataBind();

                upnlCustomers.Update();
            }
            catch { }
        }

        protected void ddlPHUONG_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadApTo(_kvpoDao.Get(ddlKHUVUC.SelectedValue).MAKV, ddlPHUONG.SelectedValue);
            }
            catch { }
        }
       
        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            var kv = _kvpoDao.Get(ddlKHUVUC.SelectedValue);
            LoadDynamicReferences(kv);            

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";

            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var phien7dot1 = _dihdDao.Get(ddlDOTINHD.SelectedValue);
                
                if ((ddlMDSD.SelectedValue == "A" || ddlMDSD.SelectedValue == "B" || ddlMDSD.SelectedValue == "G" || ddlMDSD.SelectedValue == "Z")
                        && phien7dot1.MADOTIN == "DDP7D1" )// kiem tra khong phai muc dich khac
                {
                    CloseWaitingDialog();
                    ShowInfor("Kiểm tra lại Mục đích sử dung hoặc phiên cho đúng.");
                    return;
                }

                //lock cap nhap chi so
                //int thang1 = DateTime.Now.Month;
                //string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                string nam = txtNAM.Text.Trim();

                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1); txtMADP.Text.Trim()                

                bool ksdot = _gcspoDao.IsLockDotIn(ddlDOTINHD.SelectedValue, kynay1, ddlKHUVUC.SelectedValue);
                if (phien7dot1.MADOTIN == "DDP7D1")
                {
                    if (ksdot == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số mục đích khác. Kiểm tra lại MĐSD khách hàng cho đúng.");
                        return;
                    }
                }

                bool dung = _gcspoDao.IsLockTinhCuocKy1(kynay1, ddlKHUVUC.SelectedValue, txtMADP.Text.Trim()); 
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }                

                Message msg;
                Message msgkt;

                var kh = KhachHangPo;

                if (kh == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.KH_NhapMoiDien, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    //kh.IDKHPO = lbMADDK.Text.Trim();
                    //hdfIDKH.Value = kh.IDKHPO;
                    kh.IDKHPO = _khpoDao.NewId();
                    hdfIDKH.Value = kh.IDKHPO;

                    kh.STTTS = _khpoDao.NewSTTTS(ddlKHUVUC.SelectedValue);

                    if (phien7dot1.MADOTIN == "DDP7D1")
                    {
                        kh.MUCDICHKHAC = true;
                        msg = _khpoDao.Insert(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    }
                    else
                    {
                        kh.MUCDICHKHAC = false;
                        msg = _khpoDao.Insert(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    }

                    var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
                    //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
                    msgkt = _gcspoDao.KhoiTaoGhiChiSo(kynay, kh);                    

                    var hd = HopDongPo;
                    if (hd != null && hd.SOHD == kh.SOHD)
                    {
                        bool dacap = true;
                        _hdpoDao.UpdateM(hd, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, dacap);
                    }

                    //update dong ho su dung
                    var dasd = _dhpoDao.Get(txtMADH.Text);
                    _dhpoDao.UpdateDASD(dasd);       
                }
                else // update khach hang
                {
                    if (!HasPermission(Functions.KH_NhapMoiDien, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }                  

                    msg = _khpoDao.Update(kh, DateTime.Now.Month, DateTime.Now.Year, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                
                }

                //update ma so thue
                if (!string.IsNullOrEmpty(txtMSTHUE.Text.Trim()) || txtMSTHUE.Text != "")
                {
                    _rpClass.UPKHMSTPO(kh.IDKHPO, txtMSTHUE.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                        "KHÁCH HÀNG MỚI GẮN MST");
                }

                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    //ShowInfor(ResourceLabel.Get(msg));

                    ClearForm();

                    // bind grid
                    BindKhachHangGrid();

                    upnlCustomers.Update();
                    UpdateMode = Mode.Create;

                    txtSOHD.Focus();
                }
                else
                {
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtMADP.ClientID);
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            UpdateMode = Mode.Create;
        }

        protected void ddlMDSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            MDSDToDotInHD(ddlMDSD.SelectedValue);
            
            CloseWaitingDialog();
            upnlCustomers.Update();
        }

        private void MDSDToDotInHD(string mamdsd)
        {
            try
            {
                if (mamdsd == "A" || mamdsd == "B" || mamdsd == "G" || mamdsd == "Z") // khach hang binh thuong
                {
                    ddlDOTINHD.SelectedIndex = 0;
                }
                else // muc dich khac
                {
                    string phien7dot1 = "DDP7D1";
                    var dotin = _dihdDao.GetKVDot(phien7dot1, ddlKHUVUC.SelectedValue);

                    var madotin = ddlDOTINHD.Items.FindByValue(dotin.IDMADOTIN);
                    if (madotin != null)
                    {
                        ddlDOTINHD.SelectedIndex = ddlDOTINHD.Items.IndexOf(madotin);
                    }
                }
                
            }
            catch { }
        }

        private void SetControlEnable()
        {
            var val = ddlMDSD.SelectedValue;

            if (val == MAMDSD.HD.GetHashCode().ToString())
            {
                ddlLKHDB.Enabled = false;
                ddlLOAIKH.Enabled = false;
                txtSOHO.Enabled = false;
                txtSONK.Enabled = false;
                cbISDINHMUC.Enabled = false;
                txtKYHOTRO.Enabled = false;
            }

            if (val == MAMDSD.KD.GetHashCode().ToString() || val == MAMDSD.CQ.GetHashCode().ToString())
            {
                ddlLKHDB.Enabled = true;
                ddlLOAIKH.Enabled = true;
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                cbISDINHMUC.Enabled = true;
                txtKYHOTRO.Enabled = false;
            }

            if (val == MAMDSD.HN.GetHashCode().ToString())
            {
                ddlLKHDB.Enabled = false;
                ddlLOAIKH.Enabled = false;
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                cbISDINHMUC.Enabled = true;
                txtKYHOTRO.Enabled = true;
            }
        }
     
        #region Khách hàng
        private void BindKhachHangGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var nhanvien = _nvDao.Get(b);
            var khuvucpo = _kvpoDao.GetPo(nhanvien.MAKV);

            //var list = khDao.GetListInKKT(DateTime.Now.Month, DateTime.Now.Year);
            //var list = _khpoDao.GetListKH(DateTime.Now.Month, DateTime.Now.Year);
            var list = _khpoDao.GetListKhuVuc(DateTime.Now.Month, DateTime.Now.Year, khuvucpo.MAKVPO);
            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            upnlCustomers.Update();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                /* var id = e.CommandArgument.ToString();

                 switch (e.CommandName)
                 {
                     case "SelectHD":
                         UnblockWaitingDialog();
                         var kh = khDao.Get(id);
                         KhachHang = kh;
                         SetControlEnable();

                         UpdateMode = Mode.Update;
                         CloseWaitingDialog();
                         break;
                 } */
                CloseWaitingDialog();
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
                BindKhachHangGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

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
                var list = _dppoDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
                gvDuongPho.DataSource = list;
                gvDuongPho.PagerInforText = list.Count.ToString();
                gvDuongPho.DataBind();
            }
            catch { }
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
                        //var res = id.Split('-');
                        //var dp = _dppoDao.Get(res[0], res[1]);

                        var dp = _dppoDao.GetDP(id);
                        if (dp != null)
                        {
                            txtMADP.Text = dp.MADPPO;
                            txtDUONGPHU.Text = "";

                            UpdateKhuVuc(dp);
                            upnlCustomers.Update();

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

        private void UpdateKhuVuc(DUONGPHOPO dp)
        {
            // update khu vuc, generate new madb, update label
            //SetControlValue(txtMADB.ClientID, khDao.NewMADB(dp.MADP));
            SetControlValue(txtSTT.ClientID, _khpoDao.NewSTT(dp.MADPPO).ToString());

            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKVPO);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            //LoadDynamicReferences(dp.KHUVUC);
            txtMADB.Focus();
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMADP.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

            if (dp != null)
            {
                UpdateKhuVuc(dp);
                CloseWaitingDialog();
            }
            else
            {
                CloseWaitingDialog();
                ShowWarning("Mã đường phố không hợp lệ");
            }
            txtMADB.Focus();
        }
        #endregion

        #region Đồng hồ
        protected void btnFilterDH_Click(object sender, EventArgs e)
        {
            BindDongHo();
            CloseWaitingDialog();
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            CloseWaitingDialog();
        }

        private void BindDongHo()
        {
            var list = _ldhpoDao.GetList(txtKeywordDH.Text.Trim());
            gvDongHo.DataSource = list;
            gvDongHo.PagerInforText = list.Count.ToString();
            gvDongHo.DataBind();
        }

        private void BindDongHoSoNo()
        {
            var list = _dhpoDao.GetListDASD(txtKeywordDHSONO.Text.Trim(), ddlKHUVUC.SelectedValue);
            gvDongHoSoNo.DataSource = list;
            gvDongHoSoNo.PagerInforText = list.Count.ToString();
            gvDongHoSoNo.DataBind();
        }

        protected void btnBrowseLOAIDH_Click(object sender, EventArgs e)
        {
            BindDongHo();
            upnlDongHo.Update();
            UnblockDialog("divDongHo");
        }

        protected void btnBrowseDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
        }

        protected void gvDongHo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMALDH":
                        var ldh = _ldhpoDao.Get(id);
                        if (ldh != null)
                        {
                            SetControlValue(txtMALDH.ClientID, ldh.MALDHPO);

                            //SetLabel(lblMALDH.ClientID, dp.MALDH);

                            HideDialog("divDongHo");
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDongHo.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDongHo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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
                            //SetControlValue(txtMADH.ClientID, dh.MADH);
                            txtMADH.Text = dh.MADHPO;
                            SetControlValue(txtMALDH.ClientID, dh.MALDHPO);
                            SetLabel(lblSONO.ClientID, dh.SONO);

                            lblKICHCO.Text = dh.CONGSUAT;
                            //SetLabel(lblKICHCO.ClientID, ldhDao.Get(dh.MALDH).ToString());
                            //var ldhkc = _ldhpoDao.GetListldh(dh.MALDHPO);
                            //foreach (var kc in ldhkc)
                            //{
                            //    //string a = kc.KICHCO; lblKICHCO
                            //    lblKICHCO.Text = kc.KICHCO);
                            //}

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
        #endregion

        #region Cơ quan thanh toán
        protected void btnBrowseCQ_Click(object sender, EventArgs e)
        {
            BindCQTT();
            upnlCQTT.Update();
            UnblockDialog("divCQTT");
        }

        protected void btnFilterCQTT_Click(object sender, EventArgs e)
        {
            BindCQTT();
            CloseWaitingDialog();
        }

        private void BindCQTT()
        {
            var list = cqDao.GetList(txtKeywordCQTT.Text.Trim());
            gvCQTT.DataSource = list;
            gvCQTT.PagerInforText = list.Count.ToString();
            gvCQTT.DataBind();
        }

        protected void gvCQTT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvCQTT.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindCQTT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvCQTT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMACQ":
                        var dp = cqDao.Get(id);
                        if (dp != null)
                        {
                            SetControlValue(txtCQ.ClientID, dp.MACQ);
                            SetLabel(lblTENCQ.ClientID, dp.TENCQ);

                            HideDialog("divCQTT");
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        #region Hợp đồng
        private void BindHD()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var nhanvien = _nvDao.Get(b);
                var makvpo = _kvpoDao.GetPo(nhanvien.MAKV).MAKVPO;

                //var list = _hdpoDao.GetListNN(txtKeywordHD.Text.Trim(), false);
                var list = _hdpoDao.GetListKhuVuc(txtKeywordHD.Text.Trim(), false, makvpo);

                gvHopDongDien.DataSource = list;
                gvHopDongDien.PagerInforText = list.Count.ToString();
                gvHopDongDien.DataBind();
            }
            catch { }
        }

        protected void btnFilterHD_Click(object sender, EventArgs e)
        {
            BindHD();
            CloseWaitingDialog();
        }

        protected void btnBrowseSOHD_Click(object sender, EventArgs e)
        {
            BindHD();
            upnlHopDong.Update();
            UnblockDialog("divHopDong");
        }

        protected void gvHopDongDien_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvHopDongDien.PageIndex = e.NewPageIndex;                
                BindHD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvHopDongDien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADDK":
                        var hd = _hdpoDao.Get(id);
                        if (hd != null)
                        {
                            //UnblockWaitingDialog();
                            BindHopDongToForm(hd);
                            CloseWaitingDialog();
                            HopDongPo = hd;
                            HideDialog("divHopDong");

                            txtCHISOCUOI.Text = "0";                            
                            upnlCustomers.Update();
                            upnlHopDong.Update();
                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvHopDongDien_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        private void BindHopDongToForm(HOPDONGPO hd)
        {
            //SetControlValue(txtSOHD.ClientID, hd.SOHD);
            txtSOHD.Text = hd.SOHD.ToString();
            lbMADDK.Text = hd.MADDKPO.ToString();

            SetControlValue(txtSDT.ClientID, hd.DONDANGKYPO != null ? hd.DONDANGKYPO.DIENTHOAI : "");
            SetControlValue(txtTENKH.ClientID, hd.DONDANGKYPO != null ? hd.DONDANGKYPO.TENKH : "");
            
            //txtDIACHILD.Text = hd.SONHA != null ? hd.SONHA : "";
            var dondangky = _ddkpoDao.Get(hd.MADDKPO);
            txtSONHA2.Text = dondangky.SONHA2 != null ? dondangky.SONHA2  : "";
            txtDIACHILD.Text = dondangky.TENDUONG != null ? dondangky.TENDUONG : "";

            var phuongxa = ddlPHUONG.Items.FindByValue(dondangky.MAXA != null ? dondangky.MAXA : "");
            if (phuongxa != null)
                ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(phuongxa);

            SetControlValue(txtMSTHUE.ClientID, hd.MST);

            var thicong = _tcDao.Get(hd.MADDKPO);
            txtNGAYHT.Text = thicong.NGAYHT.Value.ToString("dd/MM/yyyy");
            //DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
            //SetControlValue(txtMADH.ClientID, thicong.SOSERIAL);
            txtMADH.Text = thicong.MADH;
            var dh = _dhpoDao.Get(thicong.MADH);
            //SetLabel(lblSONO.ClientID, dh.SONO);
            lblSONO.Text = dh.SONO;
            txtMALDH.Text = dh.MALDHPO != null ? dh.MALDHPO : "";
            //SetControlValue(txtCHISOCUOI.ClientID, Convert.ToString(String.Format("{0:#.##}", thicong.CSDAU)));
            lblKICHCO.Text = dh.CONGSUAT;

            //get danh bo tu hop dong
            txtMADP.Text = hd.MADPPO.ToString();
            txtDUONGPHU.Text = "";
            txtMADB.Text = hd.MADB.ToString();
            txtSTT.Text = _khpoDao.NewSTT(hd.MADPPO).ToString();

            if (hd.KHUVUCPO != null)
            {

                var item5 = ddlKHUVUC.Items.FindByValue(hd.KHUVUCPO.MAKVPO);
                if (item5 != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item5);

                //var listPhuong = _ppoDao.GetList(hd.MAKVPO);
                //ddlPHUONG.DataSource = listPhuong;
                //ddlPHUONG.DataTextField = "TENPHUONG";
                //ddlPHUONG.DataValueField = "MAPHUONGPO";
                //ddlPHUONG.DataBind();

                upnlCustomers.Update();

                //if (hd.MAPHUONGPO != null)
                //    SetControlValue(ddlPHUONG.ClientID, hd.MAPHUONGPO);
            }

            /*
            if (hd.DUONGPHO != null)
            {
                SetControlValue(txtMADP.ClientID, hd.MADP);
                SetControlValue(txtDUONGPHU.ClientID, hd.DUONGPHU);
                SetControlValue(txtMADB.ClientID, khDao.NewMADB(hd.MADP));
                SetControlValue(txtSTT.ClientID, khDao.NewSTT(hd.MADP).ToString());
            }
            else
            {
                SetControlValue(txtMADP.ClientID, "");
                SetControlValue(txtDUONGPHU.ClientID, "");
                SetControlValue(txtMADB.ClientID, "");
                SetControlValue(txtSTT.ClientID, "");
            }*/

            if (hd.HTTHANHTOAN != null)
                SetControlValue(ddlHTTT.ClientID, hd.MAHTTT);

            if (hd.CODH != null)
                SetControlValue(ddlKICHCODH.ClientID, hd.CODH);

            if (hd.LOAIHD != null)
                SetControlValue(ddlLOAIKH.ClientID, hd.LOAIHD);

            if (hd.MDSDPO != null)
            {
                SetControlValue(ddlMDSD.ClientID, hd.MAMDSDPO);
                MDSDToDotInHD(hd.MAMDSDPO);
            }

            SetControlValue(txtSOHO.ClientID, hd.SOHO.HasValue ? hd.SOHO.Value.ToString() : "1");
            SetControlValue(txtSONK.ClientID, hd.SONHANKHAU.HasValue ? hd.SONHANKHAU.Value.ToString() : "1");

            //SetControlValue(txtNGAYHT.ClientID, hd.NGAYTAO.HasValue ? hd.NGAYTAO.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy"));
            if (hd.MADPPO != null)
            {
                txtMADP.Text = hd.MADPPO;
            }
            if (hd.MADB != null)
            {
                txtMADB.Text = hd.MADB;
            }

            var tkpo = _tkpoDao.Get(hd.MADDKPO);

            if (tkpo != null)
            {
                lbSoTruThietKe.Text = tkpo.SOTRUKH != null ? tkpo.SOTRUKH : "";
            }
            else
            {
                lbSoTruThietKe.Text = "";
            }
            

        }
        #endregion

        protected void txtNGAYHT_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string dateht = txtNGAYHT.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);

                if (int.Parse(namht) <= 10 && int.Parse(namht) >= 20)
                {
                    ShowWarning("Ngày lắp đặt không hợp lệ");
                    return;
                }
                else
                {
                    txtNGAYHT.Text = ngayht + "/" + thanght + "/20" + namht;
                }
            }
            catch 
            { 
                ShowWarning("Ngày lắp đặt không hợp lệ"); 
            }

        }

        protected void ckDotInHD_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDotInHD.Checked)
            {
                ddlDOTINHD.Enabled = true;
            }
            else
            {
                ddlDOTINHD.Enabled = false;
            }

        }

        protected void gvKhachHang_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

       
        

    }
}