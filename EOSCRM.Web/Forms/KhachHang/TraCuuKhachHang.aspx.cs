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
using System.Data;


namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class TraCuuKhachHang : Authentication
    {
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly DMThuHoDao _dmthDao = new DMThuHoDao();
        private readonly KhachHangDao khDao = new KhachHangDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly LoaiDongHoDao ldhDao = new LoaiDongHoDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly DongHoDao dhDao = new DongHoDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();
        private readonly TieuThuDao ttDao = new TieuThuDao();
        private readonly NhanVienDao nvDao = new NhanVienDao();
        private readonly DonDangKyDao dkDao = new DonDangKyDao();
        private readonly ReportClass report = new ReportClass();       
        private readonly DanhSachCoQuanThanhToanDao cqDao = new DanhSachCoQuanThanhToanDao();
        private readonly ChiTietThietKeDao _cttkdao = new ChiTietThietKeDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly HoNgheoNDao _hnnDao = new HoNgheoNDao();

        int thangF, namF;
        
        private KHACHHANG KhachHang
        {
            get
            {
                if(string.IsNullOrEmpty(hdfIDKH.Value))
                    return null;

                if (!IsDataValid())
                    return null;

                var kh = khDao.Get(hdfIDKH.Value);

                kh.CMND = txtSOCMND.Text.Trim();

                if (ckCSCUOIKHAITHAC.Checked)
                {
                    kh.MADP = txtMADPKHM.Text.Trim().ToUpper();
                    kh.MADB = txtDANHBOKHM.Text.Trim().ToUpper();
                    kh.SONHA2 = txtSONHA2KHTAM.Text.Trim();
                    kh.MAMDSD = ddlMDSDKHMTAM.SelectedValue;

                    kh.SODINHMUC = Convert.ToInt32(txtSODINHMUCTAM.Text.Trim());
                }
                else
                {
                    kh.MADB = txtMADB.Text.Trim().ToUpper();
                    kh.MADP = txtMADP.Text.Trim().ToUpper();
                    kh.SONHA2 = txtSONHA2.Text.Trim();
                    kh.MAMDSD = ddlMDSD.SelectedValue;

                    if (!string.IsNullOrEmpty(txtSODINHMUC.Text.Trim()))
                        kh.SODINHMUC = int.Parse(txtSODINHMUC.Text.Trim());
                    else
                        kh.SODINHMUC = int.Parse("1");
                }

                kh.DUONGPHU = txtDUONGPHU.Text.Trim();
                kh.MALKHDB = ddlLKHDB.SelectedValue;
                
                kh.MADDK = lbMADDK.Text.Trim();
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
                //kh.ISDINHMUC = cbISDINHMUC.Checked;
                kh.GHI2THANG1LAN = ddlGHI2THANG1LAN.SelectedValue;
                kh.THUHO = ddlTHUHO.SelectedValue;
                kh.VAT = true;
                kh.KHONGTINH117 = cbKHONGTINH117.Checked;

                var dp = dpDao.Get(txtMADP.Text.Trim().ToUpper(), txtDUONGPHU.Text.Trim());
                if (dp == null)
                    return null;

                kh.MAKV = dp.MAKV;
                kh.MAKVDN = ddlKHUVUCDN.SelectedValue;

                kh.MACQ = !string.IsNullOrEmpty(txtCQ.Text.Trim()) ? txtCQ.Text.Trim() : null;

                // dinh muc
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

                kh.CHISODAU = !string.IsNullOrEmpty(txtCSDAUKHAITHAC.Text.Trim()) ? Convert.ToDecimal(txtCSDAUKHAITHAC.Text.Trim()) : Convert.ToDecimal("0");
                kh.CHISOCUOI = !string.IsNullOrEmpty(txtCSCUOIKHAITHAC.Text.Trim()) ? Convert.ToDecimal(txtCSCUOIKHAITHAC.Text.Trim()) : Convert.ToDecimal("0");

                kh.IDKHLX = txtIDKHLX.Text.Trim();
                kh.TIENCOCLX = !String.IsNullOrEmpty(txtTIENCOCLX.Text.Trim()) ? Convert.ToDecimal(txtTIENCOCLX.Text.Trim()) : Convert.ToDecimal("0");
                kh.VITRI = txtVITRI.Text.Trim();

                kh.STTTS = Convert.ToInt32(!string.IsNullOrEmpty(txtSTTTS.Text.Trim()) ? txtSTTTS.Text.Trim() : "1");

                //so ho ngheo
               /* if (!txtKYHOTROHN.Text.Trim().Equals(String.Empty))
                    kh.KYHOTROHN = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTROHN.Text.Trim());
                else
                    kh.KYHOTROHN = null;                
                kh.ISHONGHEO = ckISHONGHEO.Checked;
                kh.DONVICAPHN = txtDONVICAP.Text.Trim();
                kh.MAHN = txtMASOHN.Text.Trim();
                if (!txtNGAPCAPHN.Text.Trim().Equals(String.Empty))
                    kh.NGAYCAPHN = DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim());
                else
                    kh.NGAYCAPHN = null;

                if (!txtNGAYKTHN.Text.Trim().Equals(String.Empty))
                    kh.NGAYKETTHUCHN = DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim());
                else
                    kh.NGAYKETTHUCHN = null;

                if (!txtNGAYKYSOHN.Text.Trim().Equals(String.Empty))
                    kh.NGAYKYHN = DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim());
                else
                    kh.NGAYKYHN = null;    */ 

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
                txtDIACHILD.Text = value.SONHA != null ? value.SONHA : "";

                txtSOHO.Text = value.SOHO.HasValue ? value.SOHO.Value.ToString() : "1";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "1";
                txtSODINHMUC.Text = value.SODINHMUC.HasValue ? value.SODINHMUC.Value.ToString() : "1";//SODINHMUC

                //cbISDINHMUC.Checked = value.ISDINHMUC.HasValue && value.ISDINHMUC.Value;

                txtMSTHUE.Text = value.MST;

                var item3 = ddlPHUONG.Items.FindByValue(value.MAPHUONG);
                if (item3 != null)
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(item3);

                var item4 = ddlHTTT.Items.FindByValue(value.MAHTTT);
                if (item4 != null)
                    ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(item4);

                txtSOTK.Text = value.STK;
                txtSDT.Text = value.SDT;
                txtSOCMND.Text = value.CMND;

                txtMALDH.Text = value.MALDH;
                //var dh = dhDao.Get(value.MADH);
                //lblMALDH.Text = dh != null ? dh.MALDH : "";

                txtMADH.Text = value.MADH;
                //LOAD DONG HO

                var donghonuoc = dhDao.Get(value.MADH);
                if (donghonuoc != null)
                {
                    lbSONO.Text = donghonuoc.SONO;
                    lblKICHCO.Text = donghonuoc.CONGSUAT;
                }
                else
                {
                    lbSONO.Text = "";
                    lblKICHCO.Text = "";
                }
                
                //var listDongHo = dhDao.GetList(value.MADH);
                //foreach (var dh1 in listDongHo)
                //{
                //    lbSONO.Text = dh1.SONO;
                //    lblKICHCO.Text = dh1.CONGSUAT;
                //}

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

                txtCSDAUKHAITHAC.Text = value.CHISODAU.ToString();
                txtCSCUOIKHAITHAC.Text = value.CHISOCUOI.ToString();

                txtIDKHLX.Text = value.IDKHLX != null ? value.IDKHLX.ToString() : "";
                txtTIENCOCLX.Text = value.TIENCOCLX != null ? value.TIENCOCLX.ToString() : "0";

                txtMADPKHM.Text = value.MADP;
                txtDANHBOKHM.Text = value.MADB;

                txtSONHA2.Text = value.SONHA2 != null ? value.SONHA2 : "";
                txtSONHA2KHTAM.Text = value.SONHA2 != null ? value.SONHA2 : "";

                var item11 = ddlMDSDKHMTAM.Items.FindByValue(value.MAMDSD);
                if (item11 != null)
                    ddlMDSDKHMTAM.SelectedIndex = ddlMDSDKHMTAM.Items.IndexOf(item11);

                txtVITRI.Text = value.VITRI != null ? value.VITRI : "";

                txtSODINHMUCTAM.Text = value.SODINHMUC != null ? value.SODINHMUC.ToString() : "1";

                txtSTTTS.Text = value.STTTS != null ? value.STTTS.ToString() : "1";
            }
        }

        #region Filter, Update
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

        protected String SODIENTHOAI
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                return !param.ContainsKey(Constants.PARAM_SODIENTHOAI)
                           ? null
                           : EncryptUtil.Decrypt(param[Constants.PARAM_SODIENTHOAI].ToString());
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

        private void OpenWaitingDialog()
        {
            ((EOS)Page.Master).OpenWaitingDialog();
        }
        #endregion        

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
            CommonFunc.SetPropertiesForGrid(gvTDCT);
            CommonFunc.SetPropertiesForGrid(gvTDH);
            CommonFunc.SetPropertiesForGrid(gvDSVATTU);
            CommonFunc.SetPropertiesForGrid(gvDuongPhoKHM);
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
            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            if (dp == null)
            {
                ShowError("Mã đường phố không tồn tại.", txtMADP.ClientID);
                return false;
            }

            // check độ dài mã danh bộ = 4
            //if (txtMADB.Text.Trim().Length != 0 && txtMADB.Text.Trim().Length != 4)
            //{
            //    ShowError("Độ dài mã danh bộ phải là 4.", txtMADB.ClientID);
            //    return false;
            //}

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
        }
      
        private void ClearForm()
        {
            try
            {
                hdfIDKH.Value = "";

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

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
                txtSOCMND.Text = "";
                lbDCTHUONGTRU.Text = "";
                lbDCLAP.Text = "";
                lbNOILAP.Text = "";
                lbMADDK.Text = "";
                txtLDDIACHI.Text = "";
                txtLDDANHSO.Text = "";
                txtLDMST.Text = "";
                txtLDMDSD.Text = "";
                txtCSCUOIKHAITHAC.Text = "";

                ClearFormCheck();

                ddlTHANGGC.SelectedIndex = DateTime.Now.Month - 1;
                txtNAMGC.Text = DateTime.Now.Year.ToString();
                txtGHICHUKDLX.Text = "";
                txtGHICHUCSLX.Text = "";
                lbIDKHGC.Text = "";

                btDOISONOLX.Visible = true;
                //if (nvDao.Get(b).MAKV == "X" || nvDao.Get(b).MAKV == "S" || nvDao.Get(b).MAKV == "")
                //{
                //    btDOISONOLX.Visible = true;
                //}
                //else
                //{
                //    btDOISONOLX.Visible = false;
                //}

                //thu ho
                ckThuHo.Checked = false;
                ddlTHUHO.Enabled = false;
                lbLyDoThuHo.Visible = false;
                txtLyDoThuHo.Visible = false;
            }
            catch { }
        }

        private void ClearFormCheck()
        {
            ckTENKH.Checked = false;
            ckDIACHILD.Checked = false;
            ckDANHBO.Checked = false;
            ckMSTHUE.Checked = false;
            ckMDSD.Checked = false;
            ckCSCUOIKHAITHAC.Checked = false;

            txtTENKH.Enabled = false;
            txtDIACHILD.Enabled = false;
            ddlPHUONG.Enabled = false;
            txtMADP.Enabled = false;
            txtDUONGPHU.Enabled = false;
            btnBrowseDP.Visible = false;
            txtMADB.Enabled = false;
            txtMSTHUE.Enabled = false;
            ddlMDSD.Enabled = false;
            txtCSCUOIKHAITHAC.Enabled = false;

            //dinh muc
            txtSOHO.Enabled = false;
            txtSONK.Enabled = false;
            txtSODINHMUC.Enabled = false;
            cbISDINHMUC.Checked = false;
            lbLyDoDMNK.Visible = false;
            txtLyDoDMNK.Visible = false;

            //check chi so cuoi, cho khm
            txtCSDAUKHAITHAC.Enabled = false;
            txtCSCUOIKHAITHAC.Enabled = false;
            txtIDKHLX.Enabled = false;
            txtTIENCOCLX.Enabled = false;
            btTIMDPKHM.Visible = false;
            txtDANHBOKHM.Enabled = false;
            txtSONHA2KHTAM.Enabled = false;
            ddlMDSDKHMTAM.Enabled = false;
            txtSODINHMUCTAM.Enabled = false;

            txtLDDIACHI.Visible = false;
            txtLDMDSD.Visible = false;
            txtLyDoDMNK.Visible = false;
            txtLDDANHSO.Visible = false;
            txtLDMST.Visible = false;

            lbLDDIACHI.Visible = false;
            lbLDMDSD.Visible = false;
            lbLyDoDMNK.Visible = false;
            lbLDDANHSO.Visible = false;
            lbLDMST.Visible = false;           
        }

        private void LoadStaticReferences()
        {
            ddlTHANGTDCT.SelectedIndex = DateTime.Now.Month - 1;
            txtNAMTDCT.Text = DateTime.Now.Year.ToString();

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

            var listMucDichSuDungKHMLX = new MucDichSuDungDao().GetList();
            ddlMDSDKHMTAM.DataSource = listMucDichSuDungKHMLX;
            ddlMDSDKHMTAM.DataTextField = "TENMDSD";
            ddlMDSDKHMTAM.DataValueField = "MAMDSD";
            ddlMDSDKHMTAM.DataBind();

            var listHttt = new HinhThucThanhToanDao().GetList();
            ddlHTTT.DataSource = listHttt;
            ddlHTTT.DataTextField = "MOTA";
            ddlHTTT.DataValueField = "MAHTTT";
            ddlHTTT.DataBind();

            var dmth = _dmthDao.GetList();
            ddlTHUHO.DataSource = dmth;
            ddlTHUHO.DataTextField = "THUHO";
            ddlTHUHO.DataValueField = "ID";
            ddlTHUHO.DataBind();

            //xa phuong
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var querykv = nvDao.Get(b);
            var listXAPHUONG = _xpDao.GetListKV(querykv.MAKV);
            ddlTENXA.DataSource = listXAPHUONG;
            ddlTENXA.DataTextField = "TENXA";
            ddlTENXA.DataValueField = "MAXA";
            ddlTENXA.DataBind(); 
            var tenxa = _xpDao.Get(ddlTENXA.SelectedValue, querykv.MAKV);
            txtDONVICAP.Text = tenxa.TENXA.ToString() ;

            var listLOAIDH = ldhDao.GetList();
            ddlLOAIDHDOILX.DataSource = listLOAIDH;
            ddlLOAIDHDOILX.DataTextField = "MALDH";
            ddlLOAIDHDOILX.DataValueField = "MALDH";
            ddlLOAIDHDOILX.DataBind();            

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
                #region Doi chi so cuoi kh moi
                if (ckCSCUOIKHAITHAC.Checked == true && HasPermission(Functions.KH_NhapMoi, Permission.Update))
                {
                    var loginInfo11 = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo11 == null) return;
                    string b11 = loginInfo11.Username;
                    var query11 = nvDao.GetListKV(b11);

                    int thang111 = int.Parse(ddlTHANGTDCT.SelectedValue);
                    string nam11 = txtNAM.Text.Trim();
                    var kynay111 = new DateTime(int.Parse(nam11), thang111, 1);

                    //var kynay = new DateTime(2013, 6, 1);
                    //bool dung11 = gcsDao.IsLockTinhCuocKy(kynay111, ddlKHUVUC.SelectedValue);
                    var kht = KhachHang;
                    var demkh = khDao.IsKyKhaiThacCSCuoi(kynay111, kht.IDKH);

                    bool dung11 = gcsDao.IsLockTinhCuocKy1(kynay111, ddlKHUVUC.SelectedValue, kht.MADP);

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

                    report.KhachHangHis(kht.IDKH);

                    //var kht = KhachHang;
                    //var demkh = khDao.IsKyKhaiThacCSCuoi(kynay111, kht.IDKH);
                    //var khtt = khDao.Get(hdfIDKH.Value);
                    if (demkh == 1)
                    {
                        //report.UPKHCOBIEN(kht.IDKH, kht.MAKV, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                        //    txtCSCUOIKHAITHAC.Text.Trim(), txtCSDAUKHAITHAC.Text.Trim(), "UPCSCUOIKT");
                        report.UPKHTTCOBIEN(kht.IDKH, "", kht.MAKV, int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()),
                            txtSONHA2KHTAM.Text.Trim(), ddlMDSDKHMTAM.SelectedValue, txtMADPKHM.Text.Trim(), txtDANHBOKHM.Text.Trim(),
                            txtIDKHLX.Text.Trim(), Convert.ToDecimal(txtTIENCOCLX.Text.Trim()),
                            Convert.ToDecimal(txtCSDAUKHAITHAC.Text.Trim()),Convert.ToDecimal(txtCSCUOIKHAITHAC.Text.Trim()), 
                                "UPCSCUOIKTTT");

                        report.InDinhMucTamKHTAMLX(kht.IDKH.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                            cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()), b11.ToString(), ddlMDSD.SelectedValue,
                            txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01", "UPDMTAMKHLX");

                        //var msg9 = khDao.UpdateKHMCSC(khtt, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                        //report.KhachHangHis(kh.IDKH);
                        //UpdateTieuThu();

                        ClearForm();
                        divCustomersContainer.Visible = false;
                        // bind grid
                        BindKhachHangGrid();
                        CloseWaitingDialog();

                        ShowInfor("Cập nhật Chỉ số cuối khách hàng mới thành công.");
                        return;
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowInfor("Khách hàng đã khai thác rồi. Không được cập nhật CS cuối.");
                        return;
                    }
                }
                #endregion

                #region Up ten khach hang khong dua vao thay doi chi tiet. cap nhat ten co dau khach hang
                if (ckTENKH.Checked == true && HasPermission(Functions.KH_NhapMoi, Permission.Update))
                {
                    var kht = KhachHang;

                    report.KhachHangHis(kht.IDKH);

                    report.UPKHTENUP(kht.IDKH, txtTENKH.Text.Trim(),
                            int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()));

                    ClearForm();
                    divCustomersContainer.Visible = false;
                    // bind grid
                    BindKhachHangGrid();
                    CloseWaitingDialog();
                   
                    ShowInfor("Cập nhật tên khách hàng thành công.");
                    return;
                }
                #endregion

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = nvDao.GetListKV(b);                

                int thanght = DateTime.Now.Month;
                int namht = DateTime.Now.Year;
                var kyht = new DateTime(namht, thanght, 1);

                int thang1 = int.Parse(ddlTHANGTDCT.SelectedValue);
                string nam = txtNAMTDCT.Text.Trim();
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);
                var kh = KhachHang;
                if (kh == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                report.KhachHangHis(kh.IDKH);

                int thangForm = int.Parse(ddlTHANGTDCT.SelectedValue);
                int namForm = int.Parse(txtNAMTDCT.Text.Trim());
                var kyForm = new DateTime(namForm, thangForm, 1);

                bool dungForm = gcsDao.IsLockTinhCuocKy1(kyForm, ddlKHUVUC.SelectedValue, kh.MADP);

                //bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, ddlKHUVUC.SelectedValue, kh.MADP);

                if (kh.IDMADOTIN != null)
                {
                    bool khoasodotin = _gcspoDao.IsLockDotIn(kh.IDMADOTIN, kyForm, ddlKHUVUC.SelectedValue);

                    if (khoasodotin == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ nhờ thu ghi chỉ số.");
                        return;
                    }
                }

                foreach (var a in query)
                {
                    string d = a.MAKV;
                    if (a.MAKV != "99")
                    {
                        //if (dung == true)
                        if (dungForm == true)
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

                //var kh = KhachHang;               
                //if (kh == null)
                //{
                //    CloseWaitingDialog();
                //    return;
                //}

                kh.IDKH = hdfIDKH.Value;

                if (kh.XOABOKH.Equals(true))
                {
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Lỗi: Khách hàng đã xóa. Không được thay đổi.<br />", txtTENKH.ClientID);
                    CloseWaitingDialog();
                    return;
                }
                /*if(ckTENKH.Checked==true)
                {
                    report.UPKHTEN(kh.IDKH,txtTENKH.Text.Trim(),
                            int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                }*/

                //if (ckCSCUOIKHAITHAC.Checked == true )
                //{
                //    report.UPKHCOBIEN(kh.IDKH, kh.MAKV, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()),
                //            txtCSCUOIKHAITHAC.Text.Trim(), txtCSDAUKHAITHAC.Text.Trim(), "UPCSCUOIKT");    
                //}

                if (ckTENKH.Checked == true)
                {
                    report.UPKHTENUP(kh.IDKH, txtTENKH.Text.Trim(),
                            int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()));
                }

                if (ckDIACHILD.Checked == true)
                {
                    report.UPKHSONHA(kh.IDKH, txtSONHA2.Text.Trim(), txtDIACHILD.Text.Trim(),
                        int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), txtLDDIACHI.Text.Trim());
                }

                if (ckDANHBO.Checked == true)
                {
                    bool khoasoDANHBO = gcsDao.IsLockTinhCuocKy1(kyForm, ddlKHUVUC.SelectedValue, txtMADP.Text.Trim());

                    if (khoasoDANHBO == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số đường " + txtMADP.Text.Trim() + ". Kiểm tra lại đợt in.");
                        return;
                    }                        

                    report.UPKHDANHBO(kh.IDKH, txtMADP.Text.Trim().ToUpper(), txtMADB.Text.Trim(), txtDUONGPHU.Text.Trim(),
                        int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), txtLDDANHSO.Text.Trim());
                }

                if (ckMSTHUE.Checked == true)
                {
                    report.UPKHMST(kh.IDKH, txtMSTHUE.Text.Trim(),
                        int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), txtLDMST.Text.Trim());
                }

                if (ckMDSD.Checked == true)
                {
                    report.UPKHMAMDSD(kh.IDKH, ddlMDSD.SelectedValue,
                        int.Parse(ddlTHANGTDCT.SelectedValue), int.Parse(txtNAMTDCT.Text.Trim()), txtLDMDSD.Text.Trim());
                }

                // dinh muC
                //if (cbISDINHMUC.Checked == true)
                if (cbISDINHMUC.Checked == true )//&& ckDMUCTAM.Checked == false)
                {
                    //report.InDinhMuc(kh.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                    //    cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                    //    txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01");

                    report.InDinhMucLyDo(kh.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                        cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                        txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01", txtLyDoDMNK.Text.Trim(), "INDMNKLYDO");                    
                }

                int thangtdct = Convert.ToInt32(ddlTHANGTDCT.SelectedValue);
                int namtdct = Convert.ToInt32(txtNAMTDCT.Text.Trim());
                
                #region ckThuHo
                if (ckThuHo.Checked)
                {
                    bool khoasodotin = _gcspoDao.IsLockDotIn(kh.IDMADOTIN, kyForm, ddlKHUVUC.SelectedValue);
                    if (khoasodotin == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ nhờ thu ghi chỉ số.");
                        return;
                    }

                    if (ddlTHUHO.SelectedValue != "KO")
                    {
                        bool khoasonhothu = _gcspoDao.IsLockDotInHD(kyForm, ddlKHUVUC.SelectedValue, "NNNTD1");
                        if (khoasonhothu == true)
                        {
                            CloseWaitingDialog();
                            ShowInfor("Đã khoá sổ nhờ thu ghi chỉ số.");
                            return;
                        }
                    }  

                    if (ddlTHUHO.SelectedValue != "KO")
                    {
                        var ketquakhthuho = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                            ddlTHUHO.SelectedValue, b, "", "", "", 0, 0, 0, "UPKHTHUHO");                       
                        
                        DataTable dtth = ketquakhthuho.Tables[0];
                    
                        if (dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
                        {
                            CloseWaitingDialog();
                            ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                        }
                    }
                    else
                    {                        
                        var ketqua = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                            "", "", "", "", "", 0, 0, 0, "UPTHUHODOTINKO");                      
                      
                        DataTable dtth = ketqua.Tables[0];
                       
                        if (dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
                        {
                            CloseWaitingDialog();
                            ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                        }
                    }

                    /*if (thangtdct == thanght && namtdct == namht)
                    {
                        if (ddlTHUHO.SelectedValue != "KO")
                        {
                            var ketquakhthuho = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                                ddlTHUHO.SelectedValue, b, "", "", "", 0, 0, 0, "UPKHTHUHO");

                            //var ketqua = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                            //   "", "", "", "", "", 0, 0, 0, "UPTHUHODOTIN");

                            //DataTable dt = ketqua.Tables[0];
                            DataTable dtth = ketquakhthuho.Tables[0];

                            //if (dt.Rows[0]["KETQUA"].ToString() != "DUNG" && dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
                            if (dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
                            {
                                CloseWaitingDialog();
                                ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                            }
                        }
                        else
                        {
                           // var ketquakhthuho = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                             //   ddlTHUHO.SelectedValue, b, "", "", "", 0, 0, 0, "UPKHTHUHO");
                            var ketqua = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                                "", "", "", "", "", 0, 0, 0, "UPTHUHODOTINKO");

                            //var ketqua = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                            //   "", "", "", "", "", 0, 0, 0, "UPTHUHODOTINKO");

                            // DataTable dt = ketqua.Tables[0];
                            DataTable dtth = ketqua.Tables[0];

                            //if (dt.Rows[0]["KETQUA"].ToString() != "DUNG" && dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
                            if (dtth.Rows[0]["KETQUA"].ToString() != "DUNG")
                            {
                                CloseWaitingDialog();
                                ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                            }
                        }
                    }
                    else
                    {
                        if (ddlTHUHO.SelectedValue != "KO")
                        {
                            var ketqua = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                                ddlTHUHO.SelectedValue, b, "", "", "", 0, 0, 0, "UPTHUHODOTIN");                            

                            DataTable dt = ketqua.Tables[0];

                            if (dt.Rows[0]["KETQUA"].ToString() != "DUNG")
                            {
                                CloseWaitingDialog();
                                ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                            }
                        }
                        else
                        {
                            var ketqua = report.UPKHTTCOBIEN(kh.IDKH, txtLyDoThuHo.Text.Trim(), kh.MAKV, thangtdct, namtdct,
                                "", "", "", "", "", 0, 0, 0, "UPTHUHODOTINKO");

                            DataTable dt = ketqua.Tables[0];

                            if (dt.Rows[0]["KETQUA"].ToString() != "DUNG")
                            {
                                CloseWaitingDialog();
                                ShowError("Lỗi Save thu hộ. Kiểm tra lại.", "");
                            }
                        }
                        
                    }*/
                }
                #endregion ckThuHo

                // dinh muC tam
                //if (ckDMUCTAM.Checked == true)
                //{
                //    report.InDinhMucTam(kh.IDKH.ToString(), Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                //        cbISDINHMUC.Checked, int.Parse(txtSODINHMUC.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                //        txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01");                    
                //}

                //report.InDinhMucTamKHTAMLX(kh.IDKH.ToString(), "", Convert.ToDecimal(txtSOHO.Text.Trim()), int.Parse(txtSONK.Text.Trim()),
                //            cbISDINHMUC.Checked, int.Parse(txtSODINHMUCTAM.Text.Trim()), b.ToString(), ddlMDSD.SelectedValue,
                //            txtNAMTDCT.Text.Trim() + "/" + ddlTHANGTDCT.SelectedValue + "/01", "UPDMTAMKHLX");

                var msg = khDao.Update(kh, namForm, thangForm, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                //report.KhachHangHis(kh.IDKH);

                if (namForm == DateTime.Now.Year && thangForm == DateTime.Now.Month)
                {
                    UpdateTieuThu();
                }

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
                    NAM = int.Parse(txtNAMTDCT.Text),
                    THANG = int.Parse(ddlTHANGTDCT.SelectedValue),
                    //NAM = int.Parse(txtNAM.Text),
                    //THANG = int.Parse(ddlTHANG.SelectedValue),

                    DUONGPHU = kh.DUONGPHU,
                    MADB = kh.MADB,
                    SODB = kh.MADP + kh.MADB,
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
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (nvDao.Get(b).MAKV == "X")
                {
                    var list = khDao.GetListLX(IDKH, SOHD, MADH, TENKH, SONHA, TENDP, MAKV, XOABONUOC, SODIENTHOAI);

                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }
                else
                {
                    var list = khDao.GetList(IDKH, SOHD, MADH, TENKH, SONHA, TENDP, MAKV, XOABONUOC);

                    gvKhachHang.DataSource = list;
                    gvKhachHang.PagerInforText = list.Count.ToString();
                    gvKhachHang.DataBind();
                }

                upnlCustomers.Update();
            }
            catch { }
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
                        var ddk = dkDao.Get(kh.MADDK);
                        var ttkhm = ttDao.GetTN(id, kh.KYKHAITHAC.Value.Month, kh.KYKHAITHAC.Value.Year);

                        KhachHang = kh;

                        txtPHIENLX.Text = dpDao.GetDP(kh.MADP).DOT != null ? dpDao.GetDP(kh.MADP).DOT : "";

                        txtCSDAUKHAITHAC.Text = ttkhm != null && ttkhm.CHISODAU != null ? Convert.ToInt32(ttkhm.CHISODAU).ToString() : "0";
                        txtCSCUOIKHAITHAC.Text = ttkhm != null && ttkhm.CHISOCUOI != null ? Convert.ToInt32(ttkhm.CHISOCUOI).ToString() : "0";

                        //SetControlEnable();
                        var ldh = dhDao.Get(kh.MADH);                        
                        lbMALOAIDHM.Text = ldh.MALDH.ToString();

                        //xoa bo
                        if (kh.XOABOKH.Equals(true))
                        {
                            lbKYXOABO.Visible = true;
                            if (kh.NGAYXOABO != null)
                            {
                                lbKYXOABO.Text = "Kỳ xóa bộ KH: " + String.Format("{0:MM/yyyy}", kh.NGAYXOABO.Value);
                            }                            
                        }
                        else lbKYXOABO.Visible = false;


                        if (kh.KYKHAITHAC != null)
                        {
                            lbKYKT.Text = "Kỳ " + kh.KYKHAITHAC.Value.Month + " năm " + kh.KYKHAITHAC.Value.Year;
                        }
                        else { lbKYKT.Text ="";}

                        txtSOCMND.Text = kh.CMND;
                        //chon dondangky
                        if (ddk != null)
                        {
                            //txtSOCMND.Text = ddk.CMND.ToString();

                            if (ddk.SONHA != null)
                            { lbDCTHUONGTRU.Text = ddk.SONHA; }
                            else { lbDCTHUONGTRU.Text = ""; }
                            if (ddk.DIACHILD != null)
                            { lbDCLAP.Text = ddk.DIACHILD; }
                            else { lbDCLAP.Text = ""; }
                            if (ddk.NOILAPDHHN != null)
                            { lbNOILAP.Text = ddk.NOILAPDHHN; }
                            else { lbNOILAP.Text = ""; }
                            if (ddk.MADDK != null)
                            {
                                lbMADDK.Text = ddk.MADDK;
                            }
                            else { lbMADDK.Text = ""; }

                            LoadHoNgheo(kh.IDKH);
                        }
                        else 
                        { 
                            //txtSOCMND.Text = "";
                            lbDCTHUONGTRU.Text = "";
                            lbDCLAP.Text = "";
                            lbNOILAP.Text = "";
                            lbMADDK.Text = "";

                            LoadHoNgheo(kh.IDKH);
                        }                        
                        
                        ClearFormCheck();
                        divCustomersContainer.Visible = true;

                        var tieuthu = ttDao.GetTN(kh.IDKH, Convert.ToInt16(ddlTHANGGC.SelectedValue), Convert.ToInt16(txtNAMGC.Text.Trim()));
                        if (tieuthu != null)
                        {
                            if (tieuthu.GHICHUKDLX != null)
                            { txtGHICHUKDLX.Text = tieuthu.GHICHUKDLX; }
                            else
                            { txtGHICHUKDLX.Text = ""; }

                            if (tieuthu.GHICHUCSLX != null)
                            { txtGHICHUCSLX.Text = tieuthu.GHICHUCSLX; }
                            else
                            { txtGHICHUCSLX.Text = ""; }
                        }
                        else
                        {
                            txtGHICHUKDLX.Text = "";
                            txtGHICHUCSLX.Text = "";
                        }

                        CloseWaitingDialog();
                        break;

                    case "SelectTT":
                        var kh3 = khDao.Get(id);
                        var ddk3 = dkDao.Get(kh3.MADDK);                        
                        KhachHang = kh3;
                        //SetControlEnable();
                        var ldh3 = dhDao.Get(kh3.MADH);                        
                        lbMALOAIDHM.Text = ldh3.MALDH.ToString();

                        //xoa bo
                        if (kh3.XOABOKH.Equals(true))
                        {
                            lbKYXOABO.Visible = true;
                            if (kh3.NGAYXOABO != null)
                            {                                
                                lbKYXOABO.Text = "Kỳ xóa bộ KH: " + String.Format("{0:MM/yyyy}", kh3.NGAYXOABO.Value);
                            }                            
                        }
                        else lbKYXOABO.Visible = false;

                        //chon dondangky
                        if (ddk3 != null)
                        {
                            //txtSOCMND.Text = ddk.CMND.ToString();

                            if (ddk3.SONHA != null)
                            { lbDCTHUONGTRU.Text = ddk3.SONHA; }
                            else { lbDCTHUONGTRU.Text = ""; }
                            if (ddk3.DIACHILD != null)
                            { lbDCLAP.Text = ddk3.DIACHILD; }
                            else { lbDCLAP.Text = ""; }
                            if (ddk3.NOILAPDHHN != null)
                            { lbNOILAP.Text = ddk3.NOILAPDHHN; }
                            else { lbNOILAP.Text = ""; }
                            if (ddk3.MADDK != null)
                            {
                                lbMADDK.Text = ddk3.MADDK;
                            }
                            else { lbMADDK.Text = ""; }

                            LoadHoNgheo(kh3.IDKH);
                        }
                        else
                        {
                            //txtSOCMND.Text = "";
                            lbDCTHUONGTRU.Text = "";
                            lbDCLAP.Text = "";
                            lbNOILAP.Text = "";
                            lbMADDK.Text = "";

                            LoadHoNgheo(kh3.IDKH);
                        }
                        

                        if (kh3.KYKHAITHAC != null)
                        {
                            lbKYKT.Text = "Kỳ " + kh3.KYKHAITHAC.Value.Month + " năm " + kh3.KYKHAITHAC.Value.Year;
                        }
                        else { lbKYKT.Text ="";}

                        txtSOCMND.Text = kh3.CMND;
                        /*if (ddk3 != null)
                        {
                            txtSOCMND.Text = ddk3.CMND.ToString();
                        }
                        else { txtSOCMND.Text = ""; }*/
                        
                        ClearFormCheck();
                        divCustomersContainer.Visible = true;                        
                        CloseWaitingDialog();

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
            try
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
            catch { }
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
                gvDCHD.PageIndex = e.NewPageIndex;                

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
                gvTDCT.PageIndex = e.NewPageIndex;             

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
            var kh = khDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                var list = report.LSDIEUCHINHCS(hdfIDKH.Value);
                gvDCHD.DataSource = list;
                gvDCHD.DataBind();
            }
        }

        protected void BindTDH()
        {
            var kh = khDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                var list = report.LSTHAYDONGHO(hdfIDKH.Value);
                gvTDH.DataSource = list;
                gvTDH.DataBind();
            }
        }

        protected void BindTDCT()
        {
            var kh = khDao.Get(hdfIDKH.Value);
            if (kh != null)
            {
                var list = report.LSTHAYDOICHITIET(hdfIDKH.Value, kh.MAKV);
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

        protected void gvTTTT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                string[] listIDKH = id.Split(new char[] { '-' });
                string idkh = listIDKH[0].ToString();
                int nam = Convert.ToInt16(listIDKH[1].ToString());
                int thang = Convert.ToInt16(listIDKH[2].ToString());

                switch (e.CommandName)
                {
                    case "SelectTieuThu":
                        //ShowInfor(idkh + "-" + Convert.ToString(nam) + "-" + Convert.ToString(thang));
                        var tieuthu = ttDao.GetTN(idkh, thang, nam);
                        if (tieuthu != null)
                        {
                            lbIDKHGC.Text = idkh;
                            ddlTHANGGC.SelectedIndex = tieuthu.THANG - 1;
                            txtNAMGC.Text = tieuthu.NAM.ToString();
                            txtGHICHUKDLX.Text = tieuthu.GHICHUKDLX != null ? tieuthu.GHICHUKDLX : "";
                            txtGHICHUCSLX.Text = tieuthu.GHICHUCSLX != null ? tieuthu.GHICHUCSLX : "";
                        }                       

                        HideDialog("divTTTT");

                        upnlCustomers.Update();

                        CloseWaitingDialog();
                        break;
                }
            }
            catch { }
        }
        #endregion

        #region Đường phố
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            try
            {
                BindDuongPho();
                CloseWaitingDialog();
            }
            catch { }
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
                gvDuongPho.PageIndex = e.NewPageIndex;              
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
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (nvDao.Get(b).MAKV == "X")
                {
                    txtMADB.Text = "000000";
                }
                else
                {                    
                    txtMADB.Text = khDao.NewMADB(dp.MADP);
                }

                SetControlValue(txtSTT.ClientID, khDao.NewSTT(dp.MADP).ToString());
                SetLabel(lblTENDUONG.ClientID, dp.TENDP);

                var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
                if (kv != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

                LoadDynamicReferences(dp.KHUVUC);
            }
            catch { }
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMADP.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var dp = dpDao.Get(txtMADP.Text.Trim().ToUpper(), txtDUONGPHU.Text.Trim());

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

        #region Duong pho KHM
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
            var list = dpDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDPKHM.Text.Trim());
            gvDuongPhoKHM.DataSource = list;
            gvDuongPhoKHM.PagerInforText = list.Count.ToString();
            gvDuongPhoKHM.DataBind();
        }

        protected void gvDuongPhoKHM_RowCommand(object sender, GridViewCommandEventArgs e)
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
                            txtMADPKHM.Text = dp.MADP;
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
                            
                            /*var ldhkc = ldhDao.GetListldh(ldh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetControlValue(lblKICHCO.ClientID, kc.KICHCO);
                            }*/

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
                gvDongHo.PageIndex = e.NewPageIndex;             
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
                txtLDDIACHI.Visible = true;
                lbLDDIACHI.Visible = true;

                txtSONHA2.Enabled = true;
            }
            else
            {
                txtDIACHILD.Enabled = false;
                txtLDDIACHI.Visible = false;
                lbLDDIACHI.Visible = false;

                txtSONHA2.Enabled = false;
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

        protected void txtMADB_TextChanged(object sender, EventArgs e)
        {
            var kh = KhachHang;
            if (kh == null)
            {
                CloseWaitingDialog();
                return;
            }

            var tontai = khDao.ExistsMaDanhBoKH(kh.IDKH, txtMADP.Text.Trim().ToUpper(), txtMADB.Text.Trim());
            if (tontai.Equals(1))
            {
                CloseWaitingDialog();
                ShowError("Mã danh bộ đã tồn tại.", txtMADP.ClientID);
                return;
            }

        }

        protected void lbtMADB_Click(object sender, EventArgs e)
        {
            if (txtMADB.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var kh = KhachHang;
            if (kh == null)
            {
                CloseWaitingDialog();
                return;
            }
            var tontai = khDao.ExistsMaDanhBoKH(kh.IDKH, txtMADP.Text.Trim().ToUpper(), txtMADB.Text.Trim());
            if (tontai)
            {
                CloseWaitingDialog();
                ShowError("Mã danh bộ đã tồn tại.", txtMADP.ClientID);
               
                txtMADB.Focus();
                return;
            }
            else 
            { 
                CloseWaitingDialog(); 
                return; 
            }
        }

        protected void lnkDSVATTU_Click(object sender, EventArgs e)
        {
            BindDSVATTU();
            upnlDSVATTU.Update();
            UnblockDialog("divDSVATTU");
        }

        protected void gvDSVATTU_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                gvDSVATTU.PageIndex = e.NewPageIndex;            
                BindDSVATTU();
                upnlTDH.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDSVATTU()
        {
            try
            {
                var kh = khDao.Get(hdfIDKH.Value);
                if (kh != null)
                {
                    var list = _cttkdao.GetList(kh.MADDK);

                    gvDSVATTU.DataSource = list;
                    gvDSVATTU.PagerInforText = list.Count.ToString();
                    gvDSVATTU.DataBind();
                }
            }
            catch { }
        }

        private void LoadHoNgheo(string idkh)
        {
            var khachhang = khDao.Get(idkh);

            //so ho ngheo 
            if (khachhang.MANGHEO != null)
            {
                var hnn = _hnnDao.Get(khachhang.MANGHEO);

                //var isCheckedHN = khachhang.ISHONGHEO.HasValue && khachhang.ISHONGHEO.Value;
                if (hnn.DIACHINGHEO != null)
                {
                    txtDIACHIHN.Text = hnn.DIACHINGHEO;
                }
                else
                {
                    txtDIACHIHN.Text = "";
                }
                ckISHONGHEO.Checked = khachhang.ISHONGHEO.Value;
                txtKYHOTROHN.Text = hnn.KYHOTROHN.Value.Month.ToString();
                txtNAMHOTRO.Text = hnn.KYHOTROHN.Value.Year.ToString();
                txtMASOHN.Text = hnn.MAHN.ToString();
                txtDONVICAP.Text = hnn.DONVICAPHN;

                var item5 = ddlTENXA.Items.FindByValue(hnn.MAXA);
                if (item5 != null)
                    ddlTENXA.SelectedIndex = ddlTENXA.Items.IndexOf(item5);

                txtNGAYKYSOHN.Text = hnn.NGAYKYHN.Value.ToString("dd/MM/yyyy");
                txtNGAPCAPHN.Text = hnn.NGAYCAPHN.Value.ToString("dd/MM/yyyy");
                txtNGAYKTHN.Text = hnn.NGAYKETTHUCHN.Value.ToString("dd/MM/yyyy");
            }

            
            /*if (isCheckedHN)
            {
                txtKYHOTROHN.Text = khachhang.KYHOTROHN != null ? String.Format("{0:MM/yyyy}", khachhang.KYHOTROHN.Value) : "";
                txtDONVICAP.Text = khachhang.DONVICAPHN != null ? khachhang.DONVICAPHN : "";
                txtMASOHN.Text = khachhang.MAHN != null ? khachhang.MAHN : "";
                txtNGAPCAPHN.Text = khachhang.NGAYCAPHN != null ? String.Format("{0:dd/MM/yyyy}", khachhang.NGAYCAPHN.Value) : "";
                txtNGAYKTHN.Text = khachhang.NGAYKETTHUCHN != null ? String.Format("{0:dd/MM/yyyy}", khachhang.NGAYKETTHUCHN.Value) : "";
                txtNGAYKYSOHN.Text = khachhang.NGAYKYHN != null ? String.Format("{0:dd/MM/yyyy}", khachhang.NGAYKYHN.Value) : "";
                
                txtKYHOTROHN.Enabled = true;
                txtDONVICAP.Enabled = true;
                txtMASOHN.Enabled = true;
                txtNGAPCAPHN.Enabled = true;
                txtNGAYKTHN.Enabled = true;
                txtNGAYKYSOHN.Enabled = true;
                ImageButton1.Visible = true;
                ImageButton2.Visible = true;
                ImageButton3.Visible = true;
            }
            else
            {
                txtKYHOTRO.Text = "";
                txtDONVICAP.Text = "";
                txtMASOHN.Text = "";
                txtNGAPCAPHN.Text = "";
                txtNGAYKTHN.Text = "";
                txtNGAYKYSOHN.Text = "";

                txtKYHOTROHN.Enabled = false;
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;
            }*/
        }

        protected void ckISHONGHEO_CheckedChanged(object sender, EventArgs e)
        {
            if (ckISHONGHEO.Checked)
            {
                txtKYHOTROHN.Enabled = true;
                txtNAMHOTRO.Enabled = true;
                txtDONVICAP.Enabled = true;
                txtMASOHN.Enabled = true;
                txtNGAPCAPHN.Enabled = true;
                txtNGAYKTHN.Enabled = true;
                txtNGAYKYSOHN.Enabled = true;
                ddlTENXA.Enabled = true;

                ImageButton1.Visible = true;
                ImageButton2.Visible = true;
                ImageButton3.Visible = true;                
            }
            else
            {
                txtKYHOTROHN.Enabled = false;
                txtNAMHOTRO.Enabled = false;
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                ddlTENXA.Enabled = false;

                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;
            }
        }

        protected void ddlTENXA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = nvDao.GetKV(b);

                var tenxa = _xpDao.Get(ddlTENXA.SelectedValue, query.MAKV);
                txtDONVICAP.Text = tenxa.TENXA.ToString(); ;
            }
            catch { }
        }

        protected void cbISDINHMUC_CheckedChanged(object sender, EventArgs e)
        {
            if (cbISDINHMUC.Checked)
            {
                txtSOHO.Enabled = true;
                txtSONK.Enabled = true;
                txtSODINHMUC.Enabled = true;

                lbLyDoDMNK.Visible = true;
                txtLyDoDMNK.Visible = true;
            }
            else
            {
                txtSOHO.Enabled = false;
                txtSONK.Enabled = false;
                txtSODINHMUC.Enabled = false;

                lbLyDoDMNK.Visible = false;
                txtLyDoDMNK.Visible = false;
            }
        }

        protected void ckISVIPHAM_CheckedChanged(object sender, EventArgs e)
        {
            if (ckISVIPHAM.Checked)
            {
                txtNGAYTRATIEN.Enabled = true;
                txtTONGTIENVIPH.Enabled = true;
                txtSOTIENTRA.Enabled = true;
                txtSOTIENCL.Enabled = true;
            }
            else
            {
                txtNGAYTRATIEN.Enabled = false;
                txtTONGTIENVIPH.Enabled = false;
                txtSOTIENTRA.Enabled = false;
                txtSOTIENCL.Enabled = false;
            }
        }        

        protected void ckCSCUOIKHAITHAC_CheckedChanged(object sender, EventArgs e)
        {
            var kh = khDao.Get(KhachHang.IDKH);

            int thangkt = kh.KYKHAITHAC.Value.Month;
            int namkt = kh.KYKHAITHAC.Value.Year;

            var kyht = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var khkt = new DateTime(namkt, thangkt, 1);

            if (DateTime.Now.Month == thangkt && DateTime.Now.Year == namkt || kyht == khkt.AddMonths(-1))
            {
                if (ckCSCUOIKHAITHAC.Checked)
                {
                    txtCSDAUKHAITHAC.Enabled = true;
                    txtCSCUOIKHAITHAC.Enabled = true;
                    txtIDKHLX.Enabled = true;
                    txtTIENCOCLX.Enabled = true;
                    btTIMDPKHM.Visible = true;
                    txtDANHBOKHM.Enabled = true;
                    txtSONHA2KHTAM.Enabled = true;
                    ddlMDSDKHMTAM.Enabled = true;

                    txtSODINHMUCTAM.Enabled = true;
                }
                else
                {
                    txtCSDAUKHAITHAC.Enabled = false;
                    txtCSCUOIKHAITHAC.Enabled = false;
                    txtIDKHLX.Enabled = false;
                    txtTIENCOCLX.Enabled = false;
                    btTIMDPKHM.Visible = false;
                    txtDANHBOKHM.Enabled = false;
                    txtSONHA2KHTAM.Enabled = false;
                    ddlMDSDKHMTAM.Enabled = false;

                    txtSODINHMUCTAM.Enabled = false;
                }
            }
            else
            {
                ckCSCUOIKHAITHAC.Checked = false;
                ShowInfor("Khách hàng đã khai thác. Xin chọn khách hàng mới khai thác.");

                //upnlCustomers.Update();
            }        
        }

        protected void txtSOHO_TextChanged(object sender, EventArgs e)
        {

        }

        protected void ckDMUCTAM_CheckedChanged(object sender, EventArgs e)
        {                
            var kh = khDao.Get(KhachHang.IDKH);

            int thangkt = kh.KYKHAITHAC.Value.Month;
            int namkt = kh.KYKHAITHAC.Value.Year;

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
                ShowError("Khách hàng đã khai thác. Xin chọn khách hàng mới khai thác trong kỳ.", kh.TENKH);
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
                var kh = khDao.Get(hdfIDKH.Value);
                if (kh != null)
                {
                    //var list = _cttkdao.GetList(kh.MADDK);
                    var list = report.ThemTieuThuTTVP(kh.IDKH, 0, 0, 0, 0, "", "", "", 0, 0, "DSLSVPTTVPN");
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

        protected void btLuuGhiChuTT_Click(object sender, EventArgs e)
        {
            try
            {
                var idkh = lbIDKHGC.Text.Trim();
                var thang = Convert.ToUInt16(ddlTHANGGC.SelectedValue);
                var nam = Convert.ToUInt16(txtNAMGC.Text.Trim());

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                Message msg;

                var ghichukd = !string.IsNullOrEmpty(txtGHICHUKDLX.Text.Trim()) ? txtGHICHUKDLX.Text.Trim() : "";
                var ghichucs = !string.IsNullOrEmpty(txtGHICHUCSLX.Text.Trim()) ? txtGHICHUCSLX.Text.Trim() : "";

                report.HisTieuThuBien(idkh, "", thang, nam, "", DateTime.Now, DateTime.Now, DateTime.Now, "", "",
                    "INTIEUTHUGC");

                msg = ttDao.UpdateGhiChuLX(idkh, thang, nam, b, ghichukd, ghichucs, "", "", "");                

                if (!msg.MsgType.Equals(MessageType.Error))
                {                    
                    ShowInfor(ResourceLabel.Get(msg));
                }
                else
                {                    
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), idkh);
                }
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btDOISONOLX_Click(object sender, EventArgs e)
        {
            try
            {                
                UnblockDialog("divUpSoNoLX");
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btSaveUpSoNo_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var dongho = dhDao.Get(txtMADH.Text.Trim());
                var idkh = KhachHang.IDKH;
                var idkhm = khDao.Get(idkh);

                int thang111 = DateTime.Now.Month;
                int nam11 = DateTime.Now.Year;
                var kynay111 = new DateTime(nam11, thang111, 1);                
                var kht = KhachHang;
                var demkh = khDao.IsKyKhaiThacCSCuoi(kynay111, kht.IDKH);

                bool dung11 = gcsDao.IsLockTinhCuocKy1(kynay111, ddlKHUVUC.SelectedValue, kht.MADP);               
                if (dung11 == true)
                {
                    CloseWaitingDialog();
                    ShowError("Đã khoá sổ ghi chỉ số. Kiểm tra lại.", kht.TENKH);
                    return;
                }                   

                if (!string.IsNullOrEmpty(idkhm.MADDK) || dkDao.Get(idkhm.MADDK) != null )
                {                        
                    ShowError("Xin chọn khách hàng cũ, không có mã đơn đăng ký! Kiểm tra lại", KhachHang.TENKH.ToString());

                    HideDialog("divUpSoNoLX");
                    CloseWaitingDialog();
                    upnlCustomers.Update();

                    return;                       
                }               

                Message msg;

                if (dongho != null)
                {
                    report.DongHo_His(dongho.MADH, b, DateTime.Now);

                    report.KHDongHoNuocBien(dongho.MADH, "", dongho.SONO != null ? dongho.SONO : "", txtSONODOIMOILX.Text.Trim(), dongho.MALDH, 
                        ddlLOAIDHDOILX.SelectedValue, dongho.CONGSUAT, txtCONGSUATDOILX.Text.Trim(),
                        idkhm.IDKH, idkhm.MADP, idkhm.MADB, idkhm.MAKV, idkhm.TENKH,
                        b, "Đổi No ĐH cũ khi nhập sai.", DateTime.Now, DateTime.Now, "DOISONOKHCU");

                    dongho.SONO = txtSONODOIMOILX.Text.Trim();
                    dongho.MALDH = ddlLOAIDHDOILX.SelectedValue;
                    dongho.CONGSUAT = txtCONGSUATDOILX.Text.Trim();                    

                    msg = dhDao.Update(dongho, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }
                
                HideDialog("divUpSoNoLX");
                divCustomersContainer.Visible = false;
                BindKhachHangGrid();
                CloseWaitingDialog();
                upnlCustomers.Update();
            }
            catch { }
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
        
    }
}