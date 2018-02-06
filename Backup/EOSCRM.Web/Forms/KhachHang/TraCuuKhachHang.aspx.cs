using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class TraCuuKhachHang : Authentication
    {
        private readonly KhachHangDao khDao = new KhachHangDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly LoaiDongHoDao ldhDao = new LoaiDongHoDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly DongHoDao dhDao = new DongHoDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();
        private readonly TieuThuDao ttDao = new TieuThuDao();
        private readonly NhanVienDao nvDao = new NhanVienDao();

        private readonly ReportClass report = new ReportClass();        

        private readonly DanhSachCoQuanThanhToanDao cqDao = new DanhSachCoQuanThanhToanDao();

        #region Properties
        private KHACHHANG KhachHang
        {
            get
            {
                if(string.IsNullOrEmpty(hdfIDKH.Value))
                    return null;

                if (!IsDataValid())
                    return null;

                var kh = khDao.Get(hdfIDKH.Value);

                kh.MADB = txtMADB.Text.Trim();
                kh.MADP = txtMADP.Text.Trim();
                kh.DUONGPHU = txtDUONGPHU.Text.Trim();
                kh.MALKHDB = ddlLKHDB.SelectedValue;
                kh.MAMDSD = ddlMDSD.SelectedValue;
                kh.MADDK = null;
                kh.SOHD = txtSOHD.Text.Trim();
                kh.MABG = ddlMAGIA.SelectedValue.Equals("NULL") ? null : ddlMAGIA.SelectedValue;
                kh.MAHOTRO = ddlHOTRO.SelectedValue;
                kh.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : ddlPHUONG.SelectedValue;
                kh.TENKH = txtTENKH.Text.Trim();
                kh.SONHA = txtDIACHILD.Text.Trim();
                kh.MST = txtMSTHUE.Text.Trim();
                kh.MAHTTT = ddlHTTT.SelectedValue;
                kh.STK = txtSOTK.Text.Trim();
                kh.SDT = txtSDT.Text.Trim();
                kh.KOPHINT = false;
                kh.THUYLK = ddlKICHCODH.SelectedValue;
                kh.NGAYGNHAP = DateTime.Now;
                kh.ISDINHMUC = cbISDINHMUC.Checked;
                kh.GHI2THANG1LAN = ddlGHI2THANG1LAN.SelectedValue;
                kh.THUHO = ddlTHUHO.SelectedValue;
                kh.VAT = true;
                kh.KHONGTINH117 = cbKHONGTINH117.Checked;

                var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (dp == null)
                    return null;

                kh.MAKV = dp.MAKV;
                kh.MAKVDN = ddlKHUVUCDN.SelectedValue;

                kh.MACQ = !string.IsNullOrEmpty(txtCQ.Text.Trim()) ? txtCQ.Text.Trim() : null;

                if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
                    kh.SOHO = decimal.Parse(txtSOHO.Text.Trim());
                else
                    kh.SOHO = null;

                if (!string.IsNullOrEmpty(txtSONK.Text.Trim()))
                    kh.SONK = int.Parse(txtSONK.Text.Trim());
                else
                    kh.SONK = null;

                if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim()))
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                else
                    kh.NGAYHT = null;

                if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
                    kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                else
                    kh.KYHOTRO = null;

                kh.MALDH = !string.IsNullOrEmpty(txtMALDH.Text.Trim()) ? txtMALDH.Text.Trim() : null;

                kh.MADH = !string.IsNullOrEmpty(txtMADH.Text.Trim()) ? txtMADH.Text.Trim() : null;

                if (!string.IsNullOrEmpty(txtM3.Text.Trim()))
                    kh.KLKHOAN = int.Parse(txtM3.Text.Trim());
                else
                    kh.KLKHOAN = null;

                if (!string.IsNullOrEmpty(txtSTT.Text.Trim()))
                    kh.STT = int.Parse(txtSTT.Text.Trim());
                else
                    kh.STT = null;

                kh.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked)
                {
                    kh.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    kh.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                }

                if (!string.Empty.Equals(txtNAM.Text.Trim()))
                    kh.NAMBDKT = int.Parse(txtNAM.Text.Trim());
                else
                    kh.NAMBDKT = null;

                kh.THANGBDKT = int.Parse(ddlTHANG.SelectedValue);

                return kh;
            }

            set
            {
                if (value == null)
                    return;

                ClearForm();

                hdfIDKH.Value = value.IDKH;

                txtMADB.Text = value.MADB;
                txtMADP.Text = value.MADP;
                txtDUONGPHU.Text = value.DUONGPHU;
                lblTENDUONG.Text = value.DUONGPHO.TENDP; 

                var item = ddlLKHDB.Items.FindByValue(value.MALKHDB);
                if (item != null)
                    ddlLKHDB.SelectedIndex = ddlLKHDB.Items.IndexOf(item);

                var cq = cqDao.Get(value.MACQ);
                if (cq != null)
                {
                    txtCQ.Text = cq.MACQ;
                    lblTENCQ.Text = cq.TENCQ;
                }

                var item1 = ddlMDSD.Items.FindByValue(value.MAMDSD);
                if (item1 != null)
                    ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(item1);

                txtSOHD.Text = value.SOHD;

                var kv = kvDao.Get(value.MAKV);

                var item5 = ddlKHUVUC.Items.FindByValue(value.MAKV);
                if (item5 != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item5);

                var item9 = ddlKHUVUCDN.Items.FindByValue(value.MAKVDN);
                if (item9 != null)
                    ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);

                var g2t1l = ddlGHI2THANG1LAN.Items.FindByValue(value.GHI2THANG1LAN);
                if (g2t1l != null)
                {
                    ddlGHI2THANG1LAN.SelectedIndex=ddlGHI2THANG1LAN.Items.IndexOf(g2t1l);
                }
                var thuho = ddlTHUHO.Items.FindByValue(value.THUHO);                
                if (thuho != null)
                {
                    ddlTHUHO.SelectedIndex = ddlTHUHO.Items.IndexOf(thuho);
                }
                /**
                 * re-bind phuong
                 * 
                 */
                LoadDynamicReferences(kv);

                var item2 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if (item2 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item2);

                txtTENKH.Text = value.TENKH;
                txtDIACHILD.Text = value.SONHA;

                txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;

                txtMSTHUE.Text = value.MST;

                var item3 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if (item3 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item3);

                var item4 = ddlHTTT.Items.FindByValue(value.MAHTTT);
                if (item4 != null)
                    ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(item4);

                txtSOTK.Text = value.STK;
                txtSDT.Text = value.SDT;

                txtMALDH.Text = value.MALDH;
                //var dh = dhDao.Get(value.MADH);
                //lblMALDH.Text = dh != null ? dh.MALDH : "";

                txtMADH.Text = value.MADH;
                //LOAD DONG HO

                var listDongHo = dhDao.GetList(value.MADH);
                foreach (var dh1 in listDongHo)
                {
                    lbSONO.Text = dh1.SONO;

                }

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
                
                txtSTT.Text = value.STT.HasValue ? value.STT.Value.ToString() : "";

                var item8 = ddlMAGIA.Items.FindByValue(value.MABG);
                ddlMAGIA.SelectedIndex = (item8 != null) ? ddlHTTT.Items.IndexOf(item8) : 0;

                txtTTSD.Text = value.TTSD;
                txtNGAYTHAYDH.Text = value.NGAYTHAYDH.HasValue ? value.NGAYTHAYDH.Value.ToString("dd/MM/yyy") : "";
                var item10 = ddlHOTRO.Items.FindByValue(value.MAHOTRO);
                if (item10 != null)
                    ddlHOTRO.SelectedIndex = ddlHOTRO.Items.IndexOf(item10);
               
            }
        }

        

       

        protected String IDKH
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_IDKH) ?
                    null :
                    EncryptUtil.Decrypt(param[Constants.PARAM_IDKH].ToString());
            }
        }

        protected String SOHD
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_SOHD) ?
                    null :
                    EncryptUtil.Decrypt(param[Constants.PARAM_SOHD].ToString());
            }
        }

        protected String MADH
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_MADH)
                           ? null
                           : EncryptUtil.Decrypt(param[Constants.PARAM_MADH].ToString());
            }
        }

        protected String TENKH
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_TENKH)
                           ? null
                           : EncryptUtil.Decrypt(param[Constants.PARAM_TENKH].ToString());
            }
        }

        protected String SONHA
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_SONHA)
                           ? null
                           : EncryptUtil.Decrypt(param[Constants.PARAM_SONHA].ToString());
            }
        }

        protected String TENDP
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_TENDP)
                           ? null
                           : EncryptUtil.Decrypt(param[Constants.PARAM_TENDP].ToString());
            }
        }

        protected String MAKV
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_AREACODE))
                    return null;                
                var mkv = EncryptUtil.Decrypt(param[Constants.PARAM_AREACODE].ToString());
                var kv = kvDao.Get(mkv);
                return kv == null ? null : mkv;
            }
        }

        protected String GHI2THANG1LAN
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_GHI2THANG1LAN))
                    return null;

                var g2t1l = EncryptUtil.Decrypt(param[Constants.PARAM_GHI2THANG1LAN].ToString());                
                return g2t1l == "NULL" ?
                    null : 
                    g2t1l;
            }
        }

        protected bool HasFiltered
        {
            get
            {
                return ParameterWrapper.GetParams().Keys.Count > 0;
            }
        }
        #endregion

        private bool IsDataValid()
        {
            #region Thông tin khách hàng
            
            // check độ dài mã đường phố = 3
            if (txtMADP.Text.Trim().Length != 4)
            {
                ShowError("Độ dài mã đường phố phải là 4.", txtMADP.ClientID);
                return false;
            }

            // check mã đường phố có tồn tại không
            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
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

            // check mã loại đồng hồ
            if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
            {
                var ldh = ldhDao.Get(txtMALDH.Text.Trim());
                if (ldh == null)
                {
                    ShowError("Loại đồng hồ không tồn tại.", txtMALDH.ClientID);
                    return false;
                }
            }

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
                    ShowError("Ngày hoàn thành không hợp lệ.", txtNGAYHT.ClientID);
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
            /*
            // check chỉ số đầu là số
            if (!string.IsNullOrEmpty(txtCHISODAU.Text.Trim())) {
                try {
                    decimal.Parse(txtCHISODAU.Text.Trim());
                }
                catch {
                    ShowError("Chỉ số đầu không hợp lệ.", txtCHISODAU.ClientID);
                    return false;
                }
            }

            // check chỉ số cuối là số
            if (!string.IsNullOrEmpty(txtCHISOCUOI.Text.Trim())) {
                try {
                    decimal.Parse(txtCHISOCUOI.Text.Trim());
                }
                catch {
                    ShowError("Chỉ số cuối không hợp lệ.", txtCHISOCUOI.ClientID);
                    return false;
                }
            }

            // check khối lượng sinh hoạt là số
            if (!string.IsNullOrEmpty(txtM3SH.Text.Trim())) {
                try {
                    decimal.Parse(txtM3SH.Text.Trim());
                }
                catch {
                    ShowError("Khối lượng sinh hoạt không hợp lệ.", txtM3SH.ClientID);
                    return false;
                }
            }

            // check khối lượng sản xuất là số
            if (!string.IsNullOrEmpty(txtM3SX.Text.Trim())) {
                try {
                    decimal.Parse(txtM3SX.Text.Trim());
                }
                catch {
                    ShowError("Khối lượng sản xuất không hợp lệ.", txtM3SX.ClientID);
                    return false;
                }
            }

            // check khối lượng kinh doanh là số
            if (!string.IsNullOrEmpty(txtM3KD.Text.Trim())) {
                try {
                    decimal.Parse(txtM3KD.Text.Trim());
                }
                catch {
                    ShowError("Khối lượng kinh doanh không hợp lệ.", txtM3KD.ClientID);
                    return false;
                }
            }

            // check khối lượng hợp đồng là số
            if (!string.IsNullOrEmpty(txtM3CQ.Text.Trim())) {
                try {
                    decimal.Parse(txtM3CQ.Text.Trim());
                }
                catch {
                    ShowError("Khối lượng cơ quan không hợp lệ.", txtM3CQ.ClientID);
                    return false;
                }
            }
             * */
            #endregion

            return true;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_TraCuuKhachHang, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();                    
                    divCustomersContainer.Visible = false;
                    
                    if (HasFiltered)
                    {
                        BindKhachHangGrid();
                        BindFilterPanel();
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
            Page.Title = Resources.Message.TITLE_KH_TRACUUKHACHHANG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TRACUUKHACHHANG;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDongHo);
            CommonFunc.SetPropertiesForGrid(gvTTTT);
            CommonFunc.SetPropertiesForGrid(gvCQTT);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
        }

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

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
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

        private void BindFilterPanel()
        {
            filterPanel.AreaCode = MAKV;
            filterPanel.IDKH = IDKH;
            filterPanel.SOHD = SOHD;
            filterPanel.MADH = MADH;
            filterPanel.TENKH = TENKH;
            filterPanel.SONHA = SONHA;
            filterPanel.TENDP = TENDP;
            filterPanel.GHI2THANG1LAN = GHI2THANG1LAN;
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
            var kv = kvDao.Get(ddlKHUVUC.SelectedValue);
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

            ClearFormCheck();



        }

        private void ClearFormCheck()
        {
            ckTENKH.Checked = false;
            ckDIACHILD.Checked = false;
            ckDANHBO.Checked = false;
            ckMSTHUE.Checked = false;
            ckMDSD.Checked = false;

            txtTENKH.Enabled = false;
            txtDIACHILD.Enabled = false;
            ddlPHUONG.Enabled = false;
            txtMADP.Enabled = false;
            txtDUONGPHU.Enabled = false;
            btnBrowseDP.Visible = false;
            txtMADB.Enabled = false;
            txtMSTHUE.Enabled = false;
            ddlMDSD.Enabled = false;
        }

        private void LoadStaticReferences()
        {
            // load khu vuc
            var listKhuVuc = new KhuVucDao().GetList();
            ddlKHUVUC.Items.Clear();
            ddlKHUVUCDN.Items.Clear();

            foreach (var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                ddlKHUVUCDN.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

            var listLoaiKhachHang = new LoaiKhDacBietDao().GetList();
            ddlLKHDB.DataSource = listLoaiKhachHang;
            ddlLKHDB.DataTextField = "TENLKHDB";
            ddlLKHDB.DataValueField = "MALKHDB";
            ddlLKHDB.DataBind();

            var listMucDichSuDung = new MucDichSuDungDao().GetList();
            ddlMDSD.DataSource = listMucDichSuDung;
            ddlMDSD.DataTextField = "TENMDSD";
            ddlMDSD.DataValueField = "MAMDSD";
            ddlMDSD.DataBind();

            var listHttt = new HinhThucThanhToanDao().GetList();
            ddlHTTT.DataSource = listHttt;
            ddlHTTT.DataTextField = "MOTA";
            ddlHTTT.DataValueField = "MAHTTT";
            ddlHTTT.DataBind();

            ClearForm();
        }

        private void LoadDynamicReferences(KHUVUC kv)
        {
            // load phuong
            var listPhuong = pDao.GetList(kv.MAKV);
            ddlPHUONG.DataSource = listPhuong;
            ddlPHUONG.DataTextField = "TENPHUONG";
            ddlPHUONG.DataValueField = "MAPHUONG";
            ddlPHUONG.DataBind();

            var item9 = ddlKHUVUCDN.Items.FindByValue(kv.MAKV);
            if (item9 != null)
                ddlKHUVUCDN.SelectedIndex = ddlKHUVUCDN.Items.IndexOf(item9);
        }

        

        #region Control event handlers
        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            var kv = kvDao.Get(ddlKHUVUC.SelectedValue);
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
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = nvDao.GetListKV(b);

                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                string nam = txtNAM.Text.Trim();
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                bool dung = gcsDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);

                foreach (var a in query)
                {
                    string d = a.MAKV;
                    if (a.MAKV != "99" )
                    {
                        if (dung == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ ghi chỉ số.");
                            return; 
                        }
                        
                    }
                }

                if (!HasPermission(Functions.KH_NhapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var kh = KhachHang;               

                if (kh == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                kh.IDKH = hdfIDKH.Value;
                
                if(ckTENKH.Checked==true)
                {
                    report.UPKHTEN(kh.IDKH,txtTENKH.Text.Trim(),
                            int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                }

                if (ckDIACHILD.Checked == true)
                {
                    report.UPKHSONHA(kh.IDKH, txtDIACHILD.Text.Trim(),
                        int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                }

                if (ckDANHBO.Checked == true)
                {
                    report.UPKHDANHBO(kh.IDKH, txtMADP.Text.Trim(), txtMADB.Text.Trim(), txtDUONGPHU.Text.Trim(),
                        int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                }

                if (ckMSTHUE.Checked == true)
                {
                    report.UPKHMST(kh.IDKH, txtMSTHUE.Text.Trim(),
                        int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                }

                if (ckMDSD.Checked == true)
                {
                    report.UPKHMAMDSD(kh.IDKH, ddlMDSD.SelectedValue,
                        int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                }

                var msg = khDao.Update(kh, DateTime.Now.Month, DateTime.Now.Year, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                //report.KhachHangHis(kh.IDKH);
                UpdateTieuThu();                
                
                
                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ClearForm();
                    divCustomersContainer.Visible = false;

                    // bind grid
                    BindKhachHangGrid();
                    CloseWaitingDialog();

                    ShowInfor(ResourceLabel.Get(msg));
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtMADP.ClientID);
                }
                report.KhachHangHis(kh.IDKH);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }

           

        }

        public void UpdateTieuThu()
        {
            try
            {
                var kh = KhachHang;

                if (kh == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                kh.IDKH = hdfIDKH.Value;

                var tieuthu = new TIEUTHU
                {
                    IDKH = kh.IDKH,
                    MADP = kh.MADP,
                    NAM = int.Parse(txtNAM.Text),
                    THANG = int.Parse(ddlTHANG.SelectedValue),

                    DUONGPHU = kh.DUONGPHU,
                    MADB = kh.MADB,
                    SODB = kh.MADP + kh.DUONGPHU + kh.MADB,
                    MAMDSD = kh.MAMDSD,
                    SOHO = kh.SOHO


                };
                ttDao.Update(tieuthu);
                

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
                    
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
        }

        protected void ddlMDSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnblockWaitingDialog();

            //SetControlEnable();

            CloseWaitingDialog();
        }
       
        #endregion

        #region Khách hàng
        private void BindKhachHangGrid()
        {
            var list = khDao.GetList(IDKH, SOHD, MADH, TENKH, SONHA, TENDP, MAKV, GHI2THANG1LAN);

            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();            

            upnlCustomers.Update();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectHD":
                        var kh = khDao.Get(id);
                        KhachHang = kh;
                        //SetControlEnable();
                        ClearFormCheck();
                        divCustomersContainer.Visible = true;
                        
                        CloseWaitingDialog();
                        break;

                    case "SelectTT":                        
                        var kh1 = khDao.Get(id);
                        if (kh1 != null)
                        {
                            var list = khDao.GetThongTinTieuThu(kh1.IDKH);
                            gvTTTT.DataSource = list;
                            gvTTTT.PagerInforText = list.Count.ToString();
                            gvTTTT.DataBind();
                        }

                        lblTenKH.Text = kh1.TENKH.Trim();

                        upnlTTTT.Update();
                        UnblockDialog("divTTTT");
                        
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
                // Update page index
                gvKhachHang.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindKhachHangGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        } 
        #endregion

        #region Thông tin tiêu thụ
        protected void linkBtnTieuThu_Click(object sender, EventArgs e)
        {
            /*KHACHHANG khachhang =
                khDao.GetKhachHangFromMadb(txtMADP.Text.Trim() + txtDUONGPHU.Text.Trim() + txtMADB.Text.Trim());
            if (khachhang != null)
            {
                var dtDsinhoadon = new ReportClass().TinhHinhTieuThu(khachhang.IDKH).Tables[0];

                if (dtDsinhoadon.Rows.Count > 0)
                {
                    Session["DSINHOADON"] = dtDsinhoadon;
                    CloseWaitingDialog();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/QuanLyKH/TinhHinhTieuThu.aspx");
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Không tìm thấy dữ liệu để làm báo cáo", "");
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo", "");
            }*/

            XemTieuThu();
            
        }

        protected void XemTieuThu()
        {
            var kh = khDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                var list = khDao.GetThongTinTieuThu(kh.IDKH);
                gvTTTT.DataSource = list;
                gvTTTT.PagerInforText = list.Count.ToString();
                gvTTTT.DataBind();
            }

            lblTenKH.Text = kh.TENKH.Trim();

            upnlTTTT.Update();
            UnblockDialog("divTTTT");
        }

        protected void linkTieuThu_Click(object sender, EventArgs e)
        { 
            
        }

        protected void gvTTTT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvTTTT.PageIndex = e.NewPageIndex;

                var kh = khDao.Get(hdfIDKH.Value);
                if (kh != null)
                {
                    var list = khDao.GetThongTinTieuThu(kh.IDKH);
                    gvTTTT.DataSource = list;
                    gvTTTT.DataBind();
                }

                upnlTTTT.Update();
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
        }

        private void BindDuongPho()
        {
            var list = dpDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
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
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);

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

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            // update khu vuc, generate new madb, update label
            SetControlValue(txtMADB.ClientID, khDao.NewMADB(dp.MADP));
            SetControlValue(txtSTT.ClientID, khDao.NewSTT(dp.MADP).ToString());

            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            LoadDynamicReferences(dp.KHUVUC);
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMADP.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

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
        }
        #endregion

        #region Đồng hồ
        protected void btnFilterDH_Click(object sender, EventArgs e)
        {
            BindDongHo();
            CloseWaitingDialog();
        }

        private void BindDongHo()
        {
            var list = ldhDao.GetList(txtKeywordDH.Text.Trim());
            gvDongHo.DataSource = list;
            gvDongHo.PagerInforText = list.Count.ToString();
            gvDongHo.DataBind();
        }

        protected void btnBrowseLOAIDH_Click(object sender, EventArgs e)
        {
            BindDongHo();
            upnlDongHo.Update();
            UnblockDialog("divDongHo");
        }

        protected void gvDongHo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMALDH":
                        var ldh = ldhDao.Get(id);
                        if (ldh != null)
                        {
                            SetControlValue(txtMALDH.ClientID, ldh.MALDH);
                            
                            var ldhkc = ldhDao.GetListldh(ldh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetControlValue(lblKICHCO.ClientID, kc.KICHCO);
                            }

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

        protected void btnInBaoCao_Click(object sender, EventArgs e)
        {
            KHACHHANG khachhang =
                khDao.GetKhachHangFromMadb(txtMADP.Text.Trim() + txtDUONGPHU.Text.Trim() + txtMADB.Text.Trim());
            if (khachhang != null)
            {
                var dtDsinhoadon = new ReportClass().TinhHinhTieuThu(khachhang.IDKH).Tables[0];

                if (dtDsinhoadon.Rows.Count > 0)
                {
                    Session["DSINHOADON"] = dtDsinhoadon;
                    CloseWaitingDialog();
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/QuanLyKH/TinhHinhTieuThu.aspx");
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Không tìm thấy dữ liệu để làm báo cáo","");
                }
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo", "");
            }
        }

        protected void ckTENKH_CheckedChanged(object sender, EventArgs e)
        {
            if(ckTENKH.Checked)
            {
                txtTENKH.Enabled = true;
            }
            else
            {
                txtTENKH.Enabled = false;
            }
        }

        protected void ckDIACHILD_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDIACHILD.Checked)
            {
                txtDIACHILD.Enabled = true;
            }
            else
            {
                txtDIACHILD.Enabled = false;
            }
        }

        protected void ckDANHBO_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDANHBO.Checked)
            {
                ddlPHUONG.Enabled = true;
                txtMADP.Enabled = true;
                txtDUONGPHU.Enabled = true;
                btnBrowseDP.Visible = true;
                txtMADB.Enabled = true;
            }
            else
            {
                ddlPHUONG.Enabled = false;
                txtMADP.Enabled = false;
                txtDUONGPHU.Enabled = false;
                btnBrowseDP.Visible = false;
                txtMADB.Enabled = false;
            }
        }

        protected void ckMSTHUE_CheckedChanged(object sender, EventArgs e)
        {
            if (ckMSTHUE.Checked)
            {
                txtMSTHUE.Enabled = true;
            }
            else
            {
                txtMSTHUE.Enabled = false;
            }
        }

        protected void ckMDSD_CheckedChanged(object sender, EventArgs e)
        {
            if (ckMDSD.Checked)
            {
                ddlMDSD.Enabled = true;
            }
            else
            {
                ddlMDSD.Enabled = false;
            }
        }

        
    }
}