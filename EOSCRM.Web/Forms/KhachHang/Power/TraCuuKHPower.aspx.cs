using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class TraCuuKHPower : Authentication
    {
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly LoaiDongHoPoDao _ldhpoDao = new LoaiDongHoPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly TieuThuPoDao _ttpoDao = new TieuThuPoDao();        
        private readonly DonDangKyPoDao _dkpoDao = new DonDangKyPoDao();
        private readonly NhanVienDao nvDao = new NhanVienDao();
        private readonly ReportClass report = new ReportClass();
        private readonly DanhSachCoQuanThanhToanDao cqDao = new DanhSachCoQuanThanhToanDao();
        private readonly DMDotInHDDao _dmdihdDao = new DMDotInHDDao();
        private readonly DotInHDDao _dihdDao = new DotInHDDao();
        private readonly DMThuHoDao _dmthDao = new DMThuHoDao();

        int thangF, namF;

        #region Properties
        private KHACHHANGPO KhachHang
        {
            get
            {
                if (string.IsNullOrEmpty(hdfIDKH.Value))
                    return null;

                if (!IsDataValid())
                    return null;

                var kh = _khpoDao.Get(hdfIDKH.Value);

                kh.CMND = txtSOCMND.Text.Trim();

                if (ckUpdateKHM.Checked)
                {
                    kh.MADPPO = txtMADPKHM.Text.Trim().ToUpper();
                    kh.MADBPO = txtDANHBOKHM.Text.Trim().ToUpper();                    
                    kh.MAMDSDPO = ddlMDSDKHMTAM.SelectedValue;

                    kh.SODINHMUC = Convert.ToInt32(txtSODINHMUCTAM.Text.Trim());
                }
                else
                {
                    kh.MADBPO = txtMADB.Text.Trim().ToUpper();
                    kh.MADPPO = txtMADP.Text.Trim().ToUpper();                    
                    kh.MAMDSDPO = ddlMDSD.SelectedValue;

                    if (!string.IsNullOrEmpty(txtSODINHMUC.Text.Trim()))
                        kh.SODINHMUC = int.Parse(txtSODINHMUC.Text.Trim());
                    else
                        kh.SODINHMUC = int.Parse("1");
                }

                //kh.MADBPO = txtMADB.Text.Trim();
                //kh.MADPPO = txtMADP.Text.Trim();
                kh.DUONGPHUPO = "";
                kh.MALKHDB = ddlLKHDB.SelectedValue;
                //kh.MAMDSDPO = ddlMDSD.SelectedValue;
                kh.MADDKPO = lbMADDK.Text.Trim();
                kh.SOHD = txtSOHD.Text.Trim();
                kh.MABG = ddlMAGIA.SelectedValue.Equals("NULL") ? null : ddlMAGIA.SelectedValue;
                kh.MAHOTRO = ddlHOTRO.SelectedValue;

                kh.MAPHUONGPO = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : ddlPHUONG.SelectedValue;
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
                kh.ISDINHMUCTAM = ckDMUCTAM.Checked;
                //kh.GHI2THANG1LAN = ddlGHI2THANG1LAN.SelectedValue;
                kh.THUHO = ddlTHUHO.SelectedValue;
                kh.VAT = true;
                kh.KHONGTINH117 = cbKHONGTINH117.Checked;

                var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (dp == null)
                    return null;

                kh.MAKVPO = dp.MAKVPO;
                kh.MAKVDN = ddlKHUVUCDN.SelectedValue;

                kh.MACQ = !string.IsNullOrEmpty(txtCQ.Text.Trim()) ? txtCQ.Text.Trim() : null;

                //if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
                //    kh.SOHO = decimal.Parse(txtSOHO.Text.Trim());
                //else
                //    kh.SOHO = null;

                //if (!string.IsNullOrEmpty(txtSONK.Text.Trim()))
                //    kh.SONK = int.Parse(txtSONK.Text.Trim());
                //else
                //    kh.SONK = null;

                if (!string.IsNullOrEmpty(txtNGAYHT.Text.Trim()))
                    kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNGAYHT.Text.Trim());
                else
                    kh.NGAYHT = null;

                if (!string.IsNullOrEmpty(txtKYHOTRO.Text.Trim()))
                    kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                else
                    kh.KYHOTRO = null;

                kh.MALDHPO = !string.IsNullOrEmpty(txtMALDH.Text.Trim()) ? txtMALDH.Text.Trim() : null;

                kh.MADHPO = !string.IsNullOrEmpty(txtMADH.Text.Trim()) ? txtMADH.Text.Trim() : null;

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

                // dinh muc
                if (!string.IsNullOrEmpty(txtSOHO.Text.Trim()))
                    kh.SOHO = decimal.Parse(txtSOHO.Text.Trim());
                else
                    kh.SOHO = null;
                if (!string.IsNullOrEmpty(txtSONK.Text.Trim()))
                    kh.SONK = int.Parse(txtSONK.Text.Trim());
                else
                    kh.SONK = null;

                kh.SONHA2 = txtSONHA2.Text.Trim();
                kh.SOTRUKD = txtSOTRUKD.Text.Trim();

                kh.STTTS = Convert.ToInt32(!string.IsNullOrEmpty(txtSTTTS.Text.Trim()) ? txtSTTTS.Text.Trim() : "1");

                //if (!string.IsNullOrEmpty(txtSODINHMUC.Text.Trim()))
                //    kh.SODINHMUC = int.Parse(txtSODINHMUC.Text.Trim());
                //else
                //    kh.SODINHMUC = null;                

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

                /*var g2t1l = ddlGHI2THANG1LAN.Items.FindByValue(value.GHI2THANG1LAN);
                if (g2t1l != null)
                {
                    ddlGHI2THANG1LAN.SelectedIndex = ddlGHI2THANG1LAN.Items.IndexOf(g2t1l);
                }*/
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

                var item2 = ddlPHUONG.Items.FindByValue(value.MAPHUONGPO);
                if (item2 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item2);

                txtTENKH.Text = value.TENKH;
                txtDIACHILD.Text = value.SONHA;

                //txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                //txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                //cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;

                txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                txtSODINHMUC.Text = value.SODINHMUC.HasValue ? value.SODINHMUC.Value.ToString() : "1";//SODINHMUC

                cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;
                ckDMUCTAM.Checked = value.ISDINHMUCTAM.HasValue && value.ISDINHMUCTAM.Value;

                txtMSTHUE.Text = value.MST;

                var item3 = ddlPHUONG.Items.FindByValue(value.MAPHUONGPO);
                if (item3 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item3);

                var item4 = ddlHTTT.Items.FindByValue(value.MAHTTT);
                if (item4 != null)
                    ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(item4);

                txtSOTK.Text = value.STK;
                txtSDT.Text = value.SDT;
                txtSOCMND.Text = value.CMND;

                txtMALDH.Text = value.MALDHPO;
                //var dh = dhDao.Get(value.MADH);
                //lblMALDH.Text = dh != null ? dh.MALDH : "";

                txtMADH.Text = value.MADHPO;
                //LOAD DONG HO

                var listDongHo = _dhpoDao.GetList(value.MADHPO);
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

                var item11 = ddlMDSDKHMTAM.Items.FindByValue(value.MAMDSDPO);
                if (item11 != null)
                    ddlMDSDKHMTAM.SelectedIndex = ddlMDSDKHMTAM.Items.IndexOf(item11);
                txtMADPKHM.Text = value.MADPPO;
                txtDANHBOKHM.Text = value.MADBPO;
                txtCSDAUKHAITHAC.Text = value.CHISODAU.ToString();
                txtCSCUOIKHAITHAC.Text = value.CHISOCUOI.ToString();

                txtSONHA2.Text = value.SONHA2 != null ? value.SONHA2.ToString() : "";
                txtSOTRUKD.Text = value.SOTRUKD != null ? value.SOTRUKD.ToString() : "";

                txtSODINHMUCTAM.Text = value.SODINHMUC != null ? value.SODINHMUC.ToString() : "1";

                txtSTTTS.Text = value.STTTS != null ? value.STTTS.ToString() : "1";

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
                var kv = _kvpoDao.GetPo(mkv);
                //return kv == null ? null : mkv;
                return kv == null ? null : kv.MAKVPO;
            }
        }

        protected String MaKhachHang
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_MAKHACHHANG) ?
                    null :
                    EncryptUtil.Decrypt(param[Constants.PARAM_MAKHACHHANG].ToString());
            }
        }

        protected String Cmnd
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_CMND) ?
                    null :
                    EncryptUtil.Decrypt(param[Constants.PARAM_CMND].ToString());
            }
        }

        protected String XOABONUOC
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_XOABONUOC))
                    return null;

                var g2t1l = EncryptUtil.Decrypt(param[Constants.PARAM_XOABONUOC].ToString());
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_TraCuuKhachHangPo, Permission.Read);
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

                thangF = Convert.ToInt16(ddlTHANGTDCT.SelectedValue);
                namF = Convert.ToInt16(txtNAMTDCT.Text.Trim());
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_TRACUUKHACHHANGPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TRACUUKHACHHANGPO;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDongHo);
            CommonFunc.SetPropertiesForGrid(gvTTTT);
            CommonFunc.SetPropertiesForGrid(gvCQTT);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
            CommonFunc.SetPropertiesForGrid(gvTDCT);
            CommonFunc.SetPropertiesForGrid(gvTDH);
            CommonFunc.SetPropertiesForGrid(gvDuongPhoKHM);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNoKHM);
            
        }

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
            /*if (!string.IsNullOrEmpty(txtMALDH.Text.Trim()))
            {
                var ldh = ldhDao.Get(txtMALDH.Text.Trim());
                if (ldh == null)
                {
                    ShowError("Loại đồng hồ không tồn tại.", txtMALDH.ClientID);
                    return false;
                }
            }*/

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

        private void BindFilterPanel()
        {
            filterPanel.AreaCode = MAKV;
            filterPanel.IDKH = IDKH;
            filterPanel.SOHD = SOHD;
            filterPanel.MADH = MADH;
            filterPanel.TENKH = TENKH;
            filterPanel.SONHA = SONHA;
            filterPanel.TENDP = TENDP;
            filterPanel.XOABONUOC = XOABONUOC;

            filterPanel.MaKhachHang = MaKhachHang;
            filterPanel.Cmnd = Cmnd;
        }

        private void ClearForm()
        {            
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
            txtSOCMND.Text = "";
            lbDCTHUONGTRU.Text = "";
            lbDCLAP.Text = "";
            lbNOILAP.Text = "";
            lbMADDK.Text = "";
            txtLDDIACHI.Text = "";
            txtLDDANHSO.Text = "";
            txtLDMST.Text = "";
            txtLDMDSD.Text = "";            

            ClearFormCheck();

            txtSOHO.Enabled = false;
            txtSONK.Enabled = false;
            txtSODINHMUC.Enabled = false;

            txtSONHA2.Text = "";
            txtSOTRUKD.Text = "";
            lbSOTRUTK.Text = "";

            ckThuHo.Checked = false;
            ddlTHUHO.Enabled = false;
            lbLyDoThuHo.Visible = false;
            txtLyDoThuHo.Visible = false;
            txtLyDoThuHo.Text = "";
            ddlTHUHO.SelectedIndex = 0;

            ckDotInHD.Checked = false;            
            ddlDOTINHD.Enabled = false;
            lbLyDoDotInHD.Visible = false;
            txtLyDoDotInHD.Visible = false;
            txtLyDoDotInHD.Text = "";
            ddlDOTINHD.SelectedIndex = 0;

            ckHeSoNhan.Checked = false;
            txtHeSoNhan.Enabled = false;
            lbLyDoHeSoNhan.Visible = false;
            txtLyDoHeSoNhan.Visible = false;
            txtLyDoHeSoNhan.Text = "";

            lbMaDongHoKHM.Text = "";
            lbLoaiDongHoKHM.Text = "";
            lbCongSuatDongHoKHM.Text = "";
            lbSoNoKHM.Text = "";

            ckDMUCTAM.Checked = false;
            cbISDINHMUC.Checked = false;
        }

        private void ClearFormCheck()
        {
            ckTENKH.Checked = false;
            ckDIACHILD.Checked = false;
            ckDANHBO.Checked = false;
            ckMSTHUE.Checked = false;
            ckMDSD.Checked = false;

            txtTENKH.Enabled = false;

            txtSONHA2.Enabled = false;
            txtDIACHILD.Enabled = false;

            ddlPHUONG.Enabled = false;
            txtMADP.Enabled = false;
            txtDUONGPHU.Enabled = false;
            btnBrowseDP.Visible = false;
            txtMADB.Enabled = false;
            txtMSTHUE.Enabled = false;
            ddlMDSD.Enabled = false;

            //check chi so cuoi, cho khm
            txtCSDAUKHAITHAC.Enabled = false;
            txtCSCUOIKHAITHAC.Enabled = false;            
            btTIMDPKHM.Visible = false;
            txtDANHBOKHM.Enabled = false;            
            ddlMDSDKHMTAM.Enabled = false;
            txtSODINHMUCTAM.Enabled = false;

            ckUpdateKHM.Checked = false;

            
        }

        private void LoadStaticReferences()
        {
            ddlTHANGTDCT.SelectedIndex = DateTime.Now.Month - 1;
            txtNAMTDCT.Text = DateTime.Now.Year.ToString();

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

            var listMucDichSuDungKHMLX = new MucDichSuDungPoDao().GetList();
            ddlMDSDKHMTAM.DataSource = listMucDichSuDungKHMLX;
            ddlMDSDKHMTAM.DataTextField = "TENMDSD";
            ddlMDSDKHMTAM.DataValueField = "MAMDSDPO";
            ddlMDSDKHMTAM.DataBind();

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = nvDao.Get(b);
            var kvpo = _kvpoDao.GetPo(query.MAKV);
            var kvIn = _dihdDao.GetListKVPO(kvpo.MAKVPO);

            ddlDOTINHD.Items.Clear();
            ddlDOTINHD.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var dotin in kvIn)
            {
                ddlDOTINHD.Items.Add(new ListItem(_dmdihdDao.Get(dotin.MADOTIN).TENDOTIN, dotin.IDMADOTIN));
            }

            var listLOAIDH = _ldhpoDao.GetList();
            ddlLOAIDHDOIPO.DataSource = listLOAIDH;
            ddlLOAIDHDOIPO.DataTextField = "MALDHPO";
            ddlLOAIDHDOIPO.DataValueField = "MALDHPO";
            ddlLOAIDHDOIPO.DataBind();

            var dmth = _dmthDao.GetList();
            ddlTHUHO.DataSource = dmth;
            ddlTHUHO.DataTextField = "THUHO";
            ddlTHUHO.DataValueField = "ID";
            ddlTHUHO.DataBind();

            ClearForm();

            // load phuong, xa
            var khuvucpo = _kvpoDao.Get(ddlKHUVUC.SelectedValue);
            var listPhuongXa = _xpDao.GetListKV(khuvucpo.MAKV);

            ddlPhuongXa.Items.Clear();
            ddlPhuongXa.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var phuongxa in listPhuongXa)
            {
                ddlPhuongXa.Items.Add(new ListItem(phuongxa.TENXA, phuongxa.MAXA));
            }
        }

        private void LoadDynamicReferences(KHUVUCPO kv)
        {
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

        #region Control event handlers
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
                var kh = KhachHang;
                if (kh == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                kh.IDKHPO = hdfIDKH.Value;

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = nvDao.GetListKV(b);

                int thangF = int.Parse(ddlTHANGTDCT.SelectedValue);
                int namF = Convert.ToInt32(txtNAMTDCT.Text.Trim());
                var kynayF = new DateTime(namF, thangF, 1);

                int thang1 = int.Parse(ddlTHANG.SelectedValue);
                string nam = txtNAM.Text.Trim();
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);

                //khoa so theo dot in hoa don
                var dotin = _dihdDao.Get(kh.DOTINHD != null ? kh.DOTINHD : "");
                bool p7d1 = _gcspoDao.IsLockDotInHD(kynayF, ddlKHUVUC.SelectedValue, dotin != null && dotin.MADOTIN != null ? dotin.MADOTIN : "");//phien 7 , kh muc dich khac, ngoai sinh hoat

                if (p7d1 == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1 P7.");
                    return;
                }                             

                //bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);//khoa so theo duong pho, chuyen ky
                bool dung = _gcspoDao.IsLockTinhCuocKy1(kynayF, ddlKHUVUC.SelectedValue, kh.MADPPO);
                
                foreach (var a in query)
                {
                    string d = a.MAKV;
                    if (a.MAKV != "99")
                    {
                        if (dung == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ ghi chỉ số.");
                            return;
                        }
                    }
                }

                if (!IsMucDichKhac(ddlMDSD.SelectedValue, kynayF, ddlKHUVUC.SelectedValue, "DDP7D1"))
                    return;                

                if (!HasPermission(Functions.KH_TraCuuKhachHangPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }              

                // doi chi so cuoi kh moi
                if (ckUpdateKHM.Checked == true && HasPermission(Functions.KH_TraCuuKhachHangPo, Permission.Update))
                {
                    var loginInfo11 = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo11 == null) return;
                    string b11 = loginInfo11.Username;
                    var query11 = nvDao.GetListKV(b11);

                    int thang111 = int.Parse(ddlTHANGTDCT.SelectedValue);
                    string nam11 = txtNAM.Text.Trim();
                    var kynay111 = new DateTime(int.Parse(nam11), thang111, 1);

                    var kht = KhachHang;
                    var demkh = _khpoDao.IsKyKhaiThacCSCuoi(kynay111, kht.IDKHPO);                    

                    bool dung11 = _gcspoDao.IsLockTinhCuocKy1(kynay111, ddlKHUVUC.SelectedValue, kht.MADPPO);
                    foreach (var a in query11)
                    {
                        string d = a.MAKV;
                        if (a.MAKV != "99")
                        {
                            if (dung11 == true)
                            {
                                CloseWaitingDialog();
                                ShowInfor("Đã khoá sổ ghi chỉ số. Không cập nhật chỉ số cuối.");
                                return;
                            }
                        }
                    }

                    //khoa so theo dot in hoa don
                    var dotinp7 = _dihdDao.Get(kht.DOTINHD != null ? kht.DOTINHD : "");
                    bool p7d1khm = _gcspoDao.IsLockDotInHD(kynayF, ddlKHUVUC.SelectedValue, dotinp7 != null && dotinp7.MADOTIN != null ? dotinp7.MADOTIN : "");//phien 7 , kh muc dich khac, ngoai sinh hoat
                   
                    if (p7d1khm == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1 P7.");
                        return;
                    }    

                    report.KhachHangHis(kht.IDKHPO);

                    if (demkh == 1)
                    {
                        var ketqua = report.UPKHTTCOBIEN(kht.IDKHPO, "", kht.MAKVPO, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()),
                            "", ddlMDSDKHMTAM.SelectedValue, txtMADPKHM.Text.Trim(), txtDANHBOKHM.Text.Trim(), "", 0,
                            Convert.ToDecimal(txtCSDAUKHAITHAC.Text.Trim()), Convert.ToDecimal(txtCSCUOIKHAITHAC.Text.Trim()), "UPCSCUOIKTTTPO");

                        var ketqua2 = report.InDinhMucTamKHTAMLX(kht.IDKHPO.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                            cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()), b11.ToString(), ddlMDSD.SelectedValue,
                            txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01", "UPDMTAMKHPO");

                        // doi so No dong ho khai thac mới bị nham
                        if (!string.IsNullOrEmpty(lbMaDongHoKHM.Text.Trim()))
                        {
                            var ketquadonghosai = report.BienKHNuoc(kht.IDKHPO, kht.MAKVPO, lbMaDongHoKHM.Text.Trim(), "", 0, 0, "UPSONODHPOKHM").Tables[0];
                            if (ketquadonghosai.Rows[0]["KETQUA"].ToString() != "DUNG" )
                            {
                                ShowError("Lỗi đổi số No nhầm của khách hàng mới.", "");
                                CloseWaitingDialog();
                                return;
                            }
                        }

                        DataTable dt = ketqua.Tables[0];
                        DataTable dt2 = ketqua2.Tables[0];

                        if (dt.Rows[0]["KETQUA"].ToString() == "DUNG" && dt2.Rows[0]["KETQUA"].ToString() == "DUNG")
                        {
                            ClearForm();
                            divCustomersContainer.Visible = false;

                            BindKhachHangGrid();

                            CloseWaitingDialog();
                            ShowInfor("Cập nhật khách hàng mới thành công.");
                            return;
                        }
                        else
                        {
                            ShowError("Lỗi...", "");
                            CloseWaitingDialog();
                            return;
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowInfor("Khách hàng đã khai thác rồi. Không được cập nhật CS cuối.");
                        return;
                    }
                }

                if (ckDIACHILD.Checked == true)
                {
                    report.UPTHayDoiCTPO(kh.IDKHPO, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), "CTSONHAPO",
                        txtDIACHILD.Text.Trim(), txtSONHA2.Text.Trim(), "",  txtLDDIACHI.Text.Trim());
                }

                if (ckDANHBO.Checked == true)
                {
                    bool khoasoDANHBO = _gcspoDao.IsLockTinhCuocKy1(kynayF, ddlKHUVUC.SelectedValue, txtMADP.Text.Trim());

                    if (khoasoDANHBO == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số đường " + txtMADP.Text.Trim() + ". Kiểm tra lại đợt in.");
                        return;
                    }                        

                    report.UPTHayDoiCTPO(kh.IDKHPO, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), "CTDANHBOPO",
                            txtMADP.Text.Trim(),
                            txtMADB.Text.Trim(), ddlMDSD.SelectedValue, txtLDDANHSO.Text.Trim());
                }

                if (ckMSTHUE.Checked == true)
                {
                    report.UPTHayDoiCTPO(kh.IDKHPO, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), "CTSOTHUEPO",
                        txtMSTHUE.Text.Trim(),  
                        "", "", txtLDMST.Text.Trim());
                }

                if (ckMDSD.Checked == true)
                {       
                    report.UPTHayDoiCTPO(kh.IDKHPO, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), "CTMDSDPO",
                         ddlMDSD.SelectedValue, "", "", txtLDMDSD.Text.Trim());
                }

                // dinh muC
                //if (cbISDINHMUC.Checked == true)
                if (cbISDINHMUC.Checked == true && ckDMUCTAM.Checked == false)
                {
                    report.InDinhMucPo(kh.IDKHPO.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                        cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                        txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01");
                }

                // dinh muC tam
                if (ckDMUCTAM.Checked == true)
                {
                    //report.InDinhMucTamPo(kh.IDKHPO.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                    //    cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                    //    txtNAM.Text.Trim() + "/" + ddlTHANG.SelectedValue + "/01/", "UPDMTAMPO");

                    report.InDinhMucTamPo(kh.IDKHPO.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                        cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                        txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01", "INDMTAMPO");
                }

                if (ckDotInHD.Checked == true)
                {
                    report.UPTHayDoiCTPO(kh.IDKHPO, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), "CTDOTINHDPO",
                         ddlDOTINHD.SelectedValue, LoginInfo.MANV, "", txtLyDoDotInHD.Text.Trim());
                }

                if (ckHeSoNhan.Checked == true)
                {
                    report.UPTHayDoiCTPO(kh.IDKHPO, Convert.ToInt16(ddlTHANGTDCT.SelectedValue),  Convert.ToInt16(txtNAMTDCT.Text.Trim()), "InHeSoNhan",
                        ddlDOTINHD.SelectedValue, LoginInfo.MANV, txtHeSoNhan.Text.Trim(),  txtLyDoHeSoNhan.Text.Trim()  );
                }

                kh.MAXA = ddlPhuongXa.SelectedValue;

                var msg = _khpoDao.Update(kh, DateTime.Now.Month, DateTime.Now.Year, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                //report.KhachHangHis(kh.IDKH);
                UpdateTieuThu();

                ClearForm();
                upnlCustomers.Update();

                divCustomersContainer.Visible = false;

                // bind grid
                BindKhachHangGrid();
                CloseWaitingDialog();                

                report.KhachHangHis(kh.IDKHPO);
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
                kh.IDKHPO = hdfIDKH.Value;

                var tieuthu = new TIEUTHUPO
                {
                    IDKHPO = kh.IDKHPO,
                    MADPPO = kh.MADPPO,
                    NAM = int.Parse(txtNAM.Text),
                    THANG = int.Parse(ddlTHANG.SelectedValue),

                    DUONGPHUPO = kh.DUONGPHUPO,
                    MADBPO = kh.MADBPO,
                    SODBPO = kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO,
                    MAMDSDPO = kh.MAMDSDPO,
                    SOHO = kh.SOHO


                };
                _ttpoDao.Update(tieuthu);
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
            //UnblockWaitingDialog();
            //CloseWaitingDialog();
            //if (!IsMucDichKhac(ddlMDSD.SelectedValue))
            //    return;  

            MDSDToDotInHD(ddlMDSD.SelectedValue);

            CloseWaitingDialog();
            upnlCustomers.Update();
        }

        private void MDSDToDotInHD(string mamdsd)
        {
            try
            {
                if (mamdsd == "A" || mamdsd == "B" || mamdsd == "G" || mamdsd == "Z" ) // khach hang binh thuong
                {
                    var dp = _dppoDao.GetDP(txtMADP.Text.Trim());

                    if (dp.IDMADOTIN != null)
                    {
                        var madotin = ddlDOTINHD.Items.FindByValue(dp.IDMADOTIN);
                        if (madotin != null)
                        {
                            ddlDOTINHD.SelectedIndex = ddlDOTINHD.Items.IndexOf(madotin);

                            ckDotInHD.Checked = false;
                        }                        
                    }    
                }
                else // muc dich khac
                {
                    string phien7dot1 = "DDP7D1";
                    var dotin = _dihdDao.GetKVDot(phien7dot1, ddlKHUVUC.SelectedValue);

                    var madotin = ddlDOTINHD.Items.FindByValue(dotin.IDMADOTIN);
                    if (madotin != null)
                    {
                        ddlDOTINHD.SelectedIndex = ddlDOTINHD.Items.IndexOf(madotin);

                        ckDotInHD.Checked = true;
                    }
                }

            }
            catch { }
        }

        #endregion

        #region Khách hàng
        private void BindKhachHangGrid()
        {
            //var list = _khpoDao.GetList(IDKH, SOHD, MADH, TENKH, SONHA, TENDP, MAKV, XOABONUOC);
            var list = _khpoDao.GetListMaKhachHang(IDKH, SOHD, MADH, TENKH, SONHA, TENDP, MAKV, XOABONUOC, MaKhachHang, Cmnd);
            
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
                        var kh = _khpoDao.Get(id);
                        var ddk = _dkpoDao.Get(kh.MADDKPO);
                        KhachHang = kh;

                        if (kh.ISDINHMUC == true || kh.ISDINHMUCTAM == true)
                            txtSODINHMUC.Enabled = true;

                        var ldh = _dhpoDao.Get(kh.MADHPO);
                        lbMALOAIDHM.Text = ldh.MALDHPO != null ? ldh.MALDHPO.ToString() : "";

                        if (kh.KYKHAITHAC != null)
                        {
                            lbKYKT.Text = "Kỳ " + kh.KYKHAITHAC.Month + " năm " + kh.KYKHAITHAC.Year;
                        }
                        else { lbKYKT.Text = ""; }
                        //chon dondangky
                        if (ddk != null)
                        {
                            txtSOCMND.Text = ddk.CMND.ToString();

                            if (ddk.SONHA != null)
                            { lbDCTHUONGTRU.Text = ddk.SONHA; }
                            else { lbDCTHUONGTRU.Text = ""; }
                            if (ddk.DIACHILD != null)
                            { lbDCLAP.Text = ddk.DIACHILD; }
                            else { lbDCLAP.Text = ""; }
                            if (ddk.NOILAPDHHN != null)
                            { lbNOILAP.Text = ddk.NOILAPDHHN; }
                            else { lbNOILAP.Text = ""; }
                            if (ddk.MADDKPO != null)
                            {
                                lbMADDK.Text = ddk.MADDKPO;
                            }
                            else { lbMADDK.Text = ""; }
                        }
                        else
                        {
                            //txtSOCMND.Text = "";
                            lbDCTHUONGTRU.Text = "";
                            lbDCLAP.Text = "";
                            lbNOILAP.Text = "";
                            lbMADDK.Text = "";
                        }

                        lbSOTRUTK.Text = kh.SOTRU != null ? kh.SOTRU : "";

                        ClearFormCheck();
                        divCustomersContainer.Visible = true;

                        MDSDToDotInHD(ddlMDSD.SelectedValue);

                        var phuongxa = ddlPhuongXa.Items.FindByValue(kh.MAXA != null ? kh.MAXA : "");
                        if (phuongxa != null)
                        {
                            ddlPhuongXa.SelectedIndex = ddlPhuongXa.Items.IndexOf(phuongxa);
                        }
                        else
                        {
                            ddlPhuongXa.SelectedIndex = 0;
                        }

                        CloseWaitingDialog();
                        break;

                    case "SelectTT":
                        var kh3 = _khpoDao.Get(id);
                        var ddk3 = _dkpoDao.Get(kh3.MADDKPO);
                        KhachHang = kh3;

                        if (kh3.ISDINHMUC == true || kh3.ISDINHMUCTAM == true)
                            txtSODINHMUC.Enabled = true;

                        var ldh3 = _dhpoDao.Get(kh3.MADHPO);
                        lbMALOAIDHM.Text = ldh3 != null || ldh3.MALDHPO != null ? ldh3.MALDHPO.ToString() : "TQ";

                        if (kh3.KYKHAITHAC != null)
                        {
                            lbKYKT.Text = "Kỳ " + kh3.KYKHAITHAC.Month + " năm " + kh3.KYKHAITHAC.Year;
                        }
                        else { lbKYKT.Text = ""; }

                        if (ddk3 != null)
                        {
                            txtSOCMND.Text = ddk3.CMND.ToString();
                        }
                        else { txtSOCMND.Text = ""; }

                        ClearFormCheck();
                        divCustomersContainer.Visible = true;
                        CloseWaitingDialog();

                        var kh1 = _khpoDao.Get(id);
                        if (kh1 != null)
                        {
                            var list = _khpoDao.GetThongTinTieuThu(kh1.IDKHPO);
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
                gvKhachHang.PageIndex = e.NewPageIndex;              
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
            var kh = _khpoDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                var list = _khpoDao.GetThongTinTieuThu(kh.IDKHPO);
                gvTTTT.DataSource = list;
                gvTTTT.PagerInforText = list.Count.ToString();
                gvTTTT.DataBind();
            }

            lblTenKH.Text = kh.TENKH.Trim();

            upnlTTTT.Update();
            UnblockDialog("divTTTT");
        }

        //lnkTDCT_Click
        protected void lnkTDCT_Click(object sender, EventArgs e)
        {
            BindTDCT();
            upnlTDCT.Update();
            UnblockDialog("divTDCT");
        }

        protected void gvDCHD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDCHD.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDCHD();
                upnlDCHD.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvTDH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvTDH.PageIndex = e.NewPageIndex;               
                BindTDH();
                upnlTDH.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvTDCT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvTDCT.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindTDCT();
                upnlTDCT.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void BindDCHD()
        {
            var kh = _khpoDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                //var list = report.LSDIEUCHINHCS(hdfIDKH.Value);
                var list = report.LSDIEUCHINHCS(hdfIDKH.Value);
                gvDCHD.DataSource = list;
                gvDCHD.DataBind();
            }
        }

        protected void BindTDH()
        {
            var kh = _khpoDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                //var list = report.LSTHAYDONGHO(hdfIDKH.Value);
                var list = report.BienKHPo(hdfIDKH.Value,"","","",0,0,"LSTHAYDHPO");
                gvTDH.DataSource = list;
                gvTDH.DataBind();
            }
        }

        protected void BindTDCT()
        {
            var kh = _khpoDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                var list = report.LSTHAYDOICHITIET(hdfIDKH.Value, kh.MAKVPO);
                gvTDCT.DataSource = list;
                gvTDCT.DataBind();
            }
        }

        protected void lnkTHAYDH_Click(object sender, EventArgs e)
        {
            BindTDH();
            upnlTDH.Update();
            UnblockDialog("divTDH");
        }

        protected void lnkDCHD_Click(object sender, EventArgs e)
        {
            BindDCHD();
            upnlDCHD.Update();
            UnblockDialog("divDCHD");
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

                var kh = _khpoDao.Get(hdfIDKH.Value);
                if (kh != null)
                {
                    var list = _khpoDao.GetThongTinTieuThu(kh.IDKHPO);
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
            CloseWaitingDialog();
        }

        private void BindDuongPho()
        {
            var list = _dppoDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
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
                        var dp = _dppoDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADPPO);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHUPO);

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
            //SetControlValue(txtMADB.ClientID, _khpoDao.NewMADB(dp.MADPPO));
            //SetControlValue(txtSTT.ClientID, _khpoDao.NewSTT(dp.MADPPO).ToString());

            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKVPO);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            LoadDynamicReferences(dp.KHUVUCPO);

            CloseWaitingDialog();
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
            var list = _ldhpoDao.GetList(txtKeywordDH.Text.Trim());
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
                        var ldh = _ldhpoDao.Get(id);
                        if (ldh != null)
                        {
                            SetControlValue(txtMALDH.ClientID, ldh.MALDHPO);

                            var ldhkc = _ldhpoDao.GetListldh(ldh.MALDHPO);
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
            KHACHHANGPO khachhang =
                _khpoDao.GetKhachHangFromMadb(txtMADP.Text.Trim() + txtDUONGPHU.Text.Trim() + txtMADB.Text.Trim());
            if (khachhang != null)
            {
                var dtDsinhoadon = new ReportClass().TinhHinhTieuThu(khachhang.IDKHPO).Tables[0];

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
            }
        }

        protected void ckTENKH_CheckedChanged(object sender, EventArgs e)
        {
            if (ckTENKH.Checked)
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
                txtSONHA2.Enabled = true;
                txtDIACHILD.Enabled = true;
                txtLDDIACHI.Visible = true;
                lbLDDIACHI.Visible = true;
            }
            else
            {
                txtSONHA2.Enabled = false;
                txtDIACHILD.Enabled = false;
                txtLDDIACHI.Visible = false;
                lbLDDIACHI.Visible = false;
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
                txtLDDANHSO.Visible = true;
                lbLDDANHSO.Visible = true;
            }
            else
            {
                ddlPHUONG.Enabled = false;
                txtMADP.Enabled = false;
                txtDUONGPHU.Enabled = false;
                btnBrowseDP.Visible = false;
                txtMADB.Enabled = false;
                txtLDDANHSO.Visible = false;
                lbLDDANHSO.Visible = false;
            }
        }

        protected void ckMSTHUE_CheckedChanged(object sender, EventArgs e)
        {
            if (ckMSTHUE.Checked)
            {
                txtMSTHUE.Enabled = true;
                txtLDMST.Visible = true;
                lbLDMST.Visible = true;
            }
            else
            {
                txtMSTHUE.Enabled = false;
                txtLDMST.Visible = false;
                lbLDMST.Visible = false;
            }
        }

        protected void ckMDSD_CheckedChanged(object sender, EventArgs e)
        {
            if (ckMDSD.Checked)
            {
                ddlMDSD.Enabled = true;
                txtLDMDSD.Visible = true;
                lbLDMDSD.Visible = true;
            }
            else
            {
                ddlMDSD.Enabled = false;
                txtLDMDSD.Visible = false;
                lbLDMDSD.Visible = false;
            }
        }

        protected void lkTRUYTHUVP_Click(object sender, EventArgs e)
        {
            BindTTVPKH();
            upTTVPKH.Update();
            UnblockDialog("divTTVPKH");
        }

        private void BindTTVPKH()
        {
            try
            {
                var kh = _khpoDao.Get(hdfIDKH.Value);
                if (kh != null)
                {
                    //var list = _cttkdao.GetList(kh.MADDK);
                    var list = report.ThemTieuThuTTVP(kh.IDKHPO, 0, 0, 0, 0, "", "", "", 0, 0, "DSLSVPTTVPPO");
                    gvTTVPKH.DataSource = list;
                    //gvTTVPKH.PagerInforText = list.Count.ToString();
                    gvTTVPKH.DataBind();
                }
            }
            catch { }
        }

        protected void gvTTVPKH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvTTVPKH.PageIndex = e.NewPageIndex;
                BindTTVPKH();
                upTTVPKH.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void ckDMUCTAM_CheckedChanged(object sender, EventArgs e)
        {
            var kh = _khpoDao.Get(KhachHang.IDKHPO);

            int thangkt = kh.KYKHAITHAC.Month;
            int namkt = kh.KYKHAITHAC.Year;

            if (DateTime.Now.Month == thangkt && DateTime.Now.Year == namkt)
            {

                if (ckDMUCTAM.Checked)
                {
                    txtSOHO.Enabled = true;
                    txtSONK.Enabled = true;
                    txtSODINHMUC.Enabled = true;

                    cbISDINHMUC.Checked = true;
                }
                else
                {
                    txtSOHO.Enabled = false;
                    txtSONK.Enabled = false;
                    txtSODINHMUC.Enabled = false;

                    cbISDINHMUC.Checked = false;
                }
            }
            else
            {
                ckDMUCTAM.Checked = false;
                ShowInfor("Khách hàng đã khai thác. Xin chọn khách hàng mới khai thác trong kỳ.");
            }
        }

        protected void cbISDINHMUC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbISDINHMUC.Checked)
            {
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                txtSODINHMUC.Enabled = true;
            }
            else
            {
                txtSOHO.Enabled = false;
                txtSONK.Enabled = false;
                txtSODINHMUC.Enabled = false;
            }
        }

        protected void btTIMDPKHM_Click(object sender, EventArgs e)
        {
            try
            {
                BindDuongPhoKHM();
                upnlDuongPhoKHM.Update();
                UnblockDialog("divDuongPhoKHM");
            }
            catch { }
        }

        protected void btnFilterDPKHM_Click(object sender, EventArgs e)
        {
            try
            {
                BindDuongPhoKHM();
                CloseWaitingDialog();
            }
            catch { }
        }  

        private void BindDuongPhoKHM()
        {
            var list = _dppoDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDPKHM.Text.Trim());
            gvDuongPhoKHM.DataSource = list;
            gvDuongPhoKHM.PagerInforText = list.Count.ToString();
            gvDuongPhoKHM.DataBind();
        }

        #region gvDuongPhoKHM
        protected void gvDuongPhoKHM_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var dp = _dppoDao.GetDP(id);
                        if (dp != null)
                        {
                            txtMADPKHM.Text = dp.MADPPO;
                            lbMADPKHM.Text = dp.TENDP;

                            upnlCustomers.Update();

                            HideDialog("divDuongPhoKHM");
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

        protected void gvDuongPhoKHM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDuongPhoKHM.PageIndex = e.NewPageIndex;
                BindDuongPhoKHM();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPhoKHM_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnIDKHM") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion        

        protected void ckUpdateKHM_CheckedChanged(object sender, EventArgs e)
        {            
            var khpo = _khpoDao.Get(KhachHang.IDKHPO);

            int thangkt = Convert.ToInt32(khpo.KYKHAITHAC.Month.ToString());
            int namkt = Convert.ToInt32(khpo.KYKHAITHAC.Year.ToString());

            var kyht = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var khkt = new DateTime(namkt, thangkt, 1);

            if (DateTime.Now.Month == thangkt && DateTime.Now.Year == namkt || kyht == khkt.AddMonths(-1))
            {
                if (ckUpdateKHM.Checked)
                {
                    txtCSDAUKHAITHAC.Enabled = true;
                    txtCSCUOIKHAITHAC.Enabled = true;                       
                    btTIMDPKHM.Visible = true;
                    txtDANHBOKHM.Enabled = true;                       
                    ddlMDSDKHMTAM.Enabled = true;
                    txtSODINHMUCTAM.Enabled = true;
                }
                else
                {
                    txtCSDAUKHAITHAC.Enabled = false;
                    txtCSCUOIKHAITHAC.Enabled = false;                        
                    btTIMDPKHM.Visible = false;
                    txtDANHBOKHM.Enabled = false;                        
                    ddlMDSDKHMTAM.Enabled = false;
                    txtSODINHMUCTAM.Enabled = false;
                }
            }
            else
            {
                ckUpdateKHM.Checked = false;
                ShowInfor("Khách hàng đã khai thác. Xin chọn khách hàng mới khai thác.");
            }

            upnlCustomers.Update();
        }

        protected void btDoiSoNoPo_Click(object sender, EventArgs e)
        {
            try
            {
                UnblockDialog("divUpSoNoPo");
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btSaveUpSoNoPo_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var dongho = _dhpoDao.Get(txtMADH.Text.Trim());
                var idkh = KhachHang.IDKHPO;
                var idkhm = _khpoDao.Get(idkh);

                int thang111 = DateTime.Now.Month;
                int nam11 = DateTime.Now.Year;
                var kynay111 = new DateTime(nam11, thang111, 1);
                var kht = KhachHang;
                var demkh = _khpoDao.IsKyKhaiThacCSCuoi(kynay111, kht.IDKHPO);

                bool dung11 = _gcspoDao.IsLockTinhCuocKy1(kynay111, ddlKHUVUC.SelectedValue, kht.MADPPO);
                if (dung11 == true)
                {
                    CloseWaitingDialog();
                    ShowError("Đã khoá sổ ghi chỉ số. Kiểm tra lại.", kht.TENKH);
                    return;
                }

                if (!string.IsNullOrEmpty(idkhm.MADDKPO) || _dkpoDao.Get(idkhm.MADDKPO) != null)
                {
                    ShowError("Xin chọn khách hàng cũ, chọn khách hàng chưa có mã đơn đăng ký! Kiểm tra tra lại", KhachHang.TENKH.ToString());

                    HideDialog("divUpSoNoPo");
                    CloseWaitingDialog();
                    upnlCustomers.Update();

                    return;
                }

                Message msg;

                if (dongho != null)
                {
                    report.DongHoPo_His(dongho.MADHPO, b, DateTime.Now);

                    var ketqua = report.KHDongHoNuocBien(dongho.MADHPO, "", dongho.SONO != null ? dongho.SONO : "", txtSONODOIMOIPO.Text.Trim(), dongho.MALDHPO,
                        ddlLOAIDHDOIPO.SelectedValue, dongho.CONGSUAT, txtCONGSUATDOIPO.Text.Trim(),
                        idkhm.IDKHPO, idkhm.MADPPO, idkhm.MADBPO, idkhm.MAKVPO, idkhm.TENKH,
                        b, "Đổi No ĐH cũ khi nhập sai.", DateTime.Now, DateTime.Now, "DOISONOKHCUPO");

                    DataTable dt = ketqua.Tables[0];

                    if (dt.Rows[0]["KETQUA"].ToString() == "DUNG")
                    {

                        dongho.SONO = txtSONODOIMOIPO.Text.Trim();
                        dongho.MALDHPO = ddlLOAIDHDOIPO.SelectedValue;
                        dongho.CONGSUAT = txtCONGSUATDOIPO.Text.Trim();

                        msg = _dhpoDao.Update(dongho, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    }
                    else
                    {
                        ShowError("Lỗi...", "");
                        CloseWaitingDialog();
                        return;
                    }

                }

                HideDialog("divUpSoNoPo");
                divCustomersContainer.Visible = false;
                BindKhachHangGrid();
                CloseWaitingDialog();
                upnlCustomers.Update();
            }
            catch { }
        }

        private bool IsMucDichKhac(string mamdsd, DateTime kynayF, string makv, string madotin)
        {
            bool p7d1 = _gcspoDao.IsLockDotInHD(kynayF, makv, madotin);//phien 7 , kh muc dich khac, ngoai sinh hoat

            if (mamdsd != "A" && mamdsd != "B" && mamdsd != "G" && mamdsd != "Z")
            {
                if (p7d1 == true)
                {
                    ShowError("Mục đích khác đã khóa sổ, chọn khách hàng sinh hoạt. Kiểm tra lại.", mamdsd);
                    return false;
                }
                else
                {
                    return true;
                }
            }           

            return true;
        }

        protected void ckThuHo_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckThuHo.Checked)
                {
                    ddlTHUHO.Enabled = true;

                    lbLyDoThuHo.Visible = true;
                    txtLyDoThuHo.Visible = true;
                }
                else
                {
                    ddlTHUHO.Enabled = false;

                    lbLyDoThuHo.Visible = false;
                    txtLyDoThuHo.Visible = false;
                }
            }
            catch { }
        }

        protected void ckDotInHD_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if (ckDotInHD.Checked)
                {
                    ddlDOTINHD.Enabled = true;

                    lbLyDoDotInHD.Visible = true; 
                    txtLyDoDotInHD.Visible = true;
                }
                else
                {
                    ddlDOTINHD.Enabled = false;

                    lbLyDoDotInHD.Visible = false;
                    txtLyDoDotInHD.Visible = false;
                }
            }
            catch { }
        }

        protected void btDoiSoNoKHM_Click(object sender, EventArgs e)
        {
            try
            {
                UnblockDialog("divUpSoNoKHM");

                BindDongHoSoNoKHM();

                UpdivUpSoNoKHM.Update();
            }
            catch { }
        }

        protected void btnFilterDHSONOKHM_Click(object sender, EventArgs e)
        {
            BindDongHoSoNoKHM();

            UnblockDialog("divUpSoNoKHM");
            CloseWaitingDialog();
        }

        private void BindDongHoSoNoKHM()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var khuvucpo = _khpoDao.Get(nvDao.Get(b).MAKV);
            var list = _dhpoDao.GetListDASDKV(txtKeywordDHSONOKHM.Text.Trim(), khuvucpo.MAKVPO);

            gvDongHoSoNoKHM.DataSource = list;
            gvDongHoSoNoKHM.PagerInforText = list.Count.ToString();
            gvDongHoSoNoKHM.DataBind();
        }

        #region gvDongHoSoNoKHM
        protected void gvDongHoSoNoKHM_RowCommand(object sender, GridViewCommandEventArgs e)
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
                            lbMaDongHoKHM.Text = id;

                            lbLoaiDongHoKHM.Text = dh.MALDHPO != null ? dh.MALDHPO : "";
                            lbCongSuatDongHoKHM.Text = dh.CONGSUAT != null ? dh.CONGSUAT : "";
                            lbSoNoKHM.Text = dh.SONO != null ? dh.SONO : "";

                            upnlCustomers.Update();
                            HideDialog("divUpSoNoKHM");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHoSoNoKHM_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDongHoSoNoKHM.PageIndex = e.NewPageIndex;
                BindDongHoSoNoKHM();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        protected void ckHeSoNhan_CheckedChanged(object sender, EventArgs e)
        {
            if (ckHeSoNhan.Checked)
            {
                txtHeSoNhan.Enabled = true;

                lbLyDoHeSoNhan.Visible = true;
                txtLyDoHeSoNhan.Visible = true;
            }
            else
            {
                txtHeSoNhan.Enabled = false;

                lbLyDoHeSoNhan.Visible = false;
                txtLyDoHeSoNhan.Visible = false;
            }
        }
        

    }
}