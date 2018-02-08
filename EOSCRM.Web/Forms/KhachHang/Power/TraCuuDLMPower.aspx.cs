using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class TraCuuDLMPower : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly MucDichSuDungPoDao _mdsdpoDao = new MucDichSuDungPoDao();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly BBNghiemThuPoDao _bbntpoDao = new BBNghiemThuPoDao();

        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();       
        private readonly ChietTinhDao ctDao = new ChietTinhDao();        
        private readonly ThiCongDao tcDao = new ThiCongDao();                 
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KyDuyetDao _lvkdDao = new KyDuyetDao();          
        private readonly KyDuyetDao _kyduyetDao = new KyDuyetDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly DuyetQuyenDao _dqDao = new DuyetQuyenDao();
        private readonly ReportClass _rpClass = new ReportClass();

        #region Properties

        protected DONDANGKYPO DonDangKyPo
        {
            get
            {
                try { return (DONDANGKYPO)Session["TCDLDM_DDK"]; }
                catch { return null; }
            }

            set { Session["TCDLDM_DDK"] = value; }
        }

        protected THIETKEPO ThietKePo
        {
            get
            {
                try { return (THIETKEPO)Session["TCDLDM_TK"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_TK"] = value; }
        }

        protected CHIETTINH ChietTinh
        {
            get
            {
                try { return (CHIETTINH)Session["TCDLDM_CT"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_CT"] = value; }
        }

        protected HOPDONGPO HopDongPo
        {
            get
            {
                try { return (HOPDONGPO)Session["TCDLDM_HD"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_HD"] = value; }
        }

        protected THICONG ThiCong
        {
            get
            {
                try { return (THICONG)Session["TCDLDM_TC"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_TC"] = value; }
        }

        protected BBNGHIEMTHUPO NghiemThuPo
        {
            get
            {
                try { return (BBNGHIEMTHUPO)Session["TCDLDM_NT"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_NT"] = value; }
        }

        private DONDANGKYPO DonDangKyObjPo
        {
            get
            {
                if (!IsDataValid())
                    return null;
                var obj = _ddkpoDao.Get(txtMADDK.Text.Trim());

                obj.MADDKPO = txtMADDK.Text.Trim();
                obj.MADDKTONG = null;//ddlMADDKTONG.SelectedValue.Equals("") ? null : ddlMADDKTONG.SelectedValue,
                obj.TENKH = txtTENKH.Text.Trim().ToUpper();

                obj.TENDK = txtUYQUYEN.Text.Trim();

                obj.SONHA = txtSONHA.Text.Trim();
                obj.DIENTHOAI = txtDIENTHOAI.Text.Trim();
                obj.TEN_DC_KHAC = txtDIACHIKHAC.Text.Trim();//SOHK
                obj.DAIDIEN = false; //cbDAIDIEN.Checked,
                //obj.NOIDUNG = "";
                obj.CTCTMOI = false;
                obj.MANV = LoginInfo.MANV;

                obj.DIACHILD = txtDCLAPDAT.Text.Trim();                

                var khuvuc = _kvpoDao.Get(ddlKHUVUC.SelectedValue);
                obj.MAKVPO = khuvuc != null ? khuvuc.MAKVPO : null;

                // dai dien, ma duong
                var phuong = _ppoDao.GetMAKV(ddlPHUONG.SelectedValue, khuvuc.MAKVPO);
                obj.MAPHUONG = phuong != null ? phuong.MAPHUONGPO : null;

                var mdsd = _mdsdpoDao.Get(ddlMUCDICH.SelectedValue);
                obj.MAMDSDPO = mdsd != null ? mdsd.MAMDSDPO : null;

                if (!txtSOHODN.Text.Trim().Equals(String.Empty))
                    obj.SOHODN = Convert.ToInt32(txtSOHODN.Text.Trim());
                else
                    obj.SOHODN = null;

                var sn = (txtSONHA.Text.Trim().Equals(String.Empty) ? "" : txtSONHA.Text.Trim() + ", ");
                var tenduong = "";

                var tenphuong = phuong != null ? phuong.TENPHUONG + ", " : "";
                var tenkv = khuvuc != null ? khuvuc.TENKV : "";

                if (!txtMADP.Text.Trim().Equals(String.Empty))
                {
                    obj.MADPPO = txtMADP.Text.Trim();
                    obj.DUONGPHUPO = txtDUONGPHU.Text.Trim();

                    var duong = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                    if (duong != null)
                        tenduong = duong.TENDP + ", ";
                }
                else
                {
                    obj.MADPPO = null;
                    obj.DUONGPHUPO = null;
                    tenduong = txtDIACHIKHAC.Text.Trim().Equals(String.Empty) ? "" : txtDIACHIKHAC.Text.Trim() + ", ";
                }

                //obj.DIACHILD = string.Format("{0}{1}{2}{3}", sn, tenduong, tenphuong, tenkv);


                if (!txtSONK.Text.Trim().Equals(String.Empty))
                    obj.SONK = Convert.ToInt32(txtSONK.Text.Trim());
                else
                    obj.SONK = null;

                if (!txtDMNK.Text.Trim().Equals(String.Empty))
                    obj.DMNK = Convert.ToInt32(txtDMNK.Text.Trim());
                else
                    obj.DMNK = null;

                if (!txtNGAYCD.Text.Trim().Equals(String.Empty))
                    obj.NGAYDK = DateTimeUtil.GetVietNamDate(txtNGAYCD.Text.Trim());
                else
                    obj.NGAYDK = null;

                if (!txtNGAYKS.Text.Trim().Equals(String.Empty))
                    obj.NGAYHKS = DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());
                else
                    obj.NGAYHKS = null;

                if (!txtNGAYCAPCMND.Text.Trim().Equals(String.Empty))
                    obj.CAPNGAY = DateTimeUtil.GetVietNamDate(txtNGAYCAPCMND.Text.Trim());
                else
                    obj.CAPNGAY = null;
                

                obj.CMND = txtCMND.Text.Trim();
                obj.MST = txtMST.Text.Trim();

                obj.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked)
                {
                    obj.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    obj.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                }
                else
                {
                    obj.TENKH_INHOADON = "";
                    obj.DIACHI_INHOADON = "";
                }

                obj.ISTUYENONGCHUNG = cbISTUYENONGCHUNG.Checked;
                obj.NOILAPDHHN = txtNOILAPDHN.Text.Trim();

                obj.NOIDUNG = txtNOIDUNGKEMTHEO.Text.Trim();

                return obj;
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

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_TraCuuDonLapMoi, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    EventEnter();
                    LoadStaticReferences();                    
                    BindDataForGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_TRACUUDLMD;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TRACUUDLMD;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }        

        private void BindDataForGrid()
        {
            try
            {
                /*if (Filtered == FilteredMode.None)
                {
                    var objList = ddkDao.GetListForTcdldm();
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {*/
                int? sohodn = null;
                int? sonk = null;
                int? dmnk = null;

                // ReSharper disable EmptyGeneralCatchClause
                try { sohodn = Convert.ToInt32(txtSOHODN.Text.Trim()); }
                catch { }
                try { sonk = Convert.ToInt32(txtSONK.Text.Trim()); }
                catch { }
                try { dmnk = Convert.ToInt32(txtDMNK.Text.Trim()); }
                catch { }
                // ReSharper restore EmptyGeneralCatchClause

                //hien theo phong ban, khu vuc
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);//nhan vien khu vuc ??

                if (query.MAPB == "NB" || query.MAPB == "TA" || query.MAPB == "TD"
                        || query.MAPB == "TS" || query.MAPB == "TO" || query.MAPB == "TK" || query.MAPB == "NS" || query.MAPB == "NH"                       
                        || query.MAPB == "CV" || query.MAPB == "HL" || query.MAPB == "MM" || query.MAPB == "PM" // PHU TAN
                        || query.MAPB == "BC" || query.MAPB == "CT" || query.MAPB == "NT" || query.MAPB == "TT" // tri ton
                        || query.MAPB == "CL" || query.MAPB == "MB" || query.MAPB == "NC" || query.MAPB == "TB" // TINH BIEN
                        || query.MAPB == "AT" || query.MAPB == "CM" || query.MAPB == "HB" || query.MAPB == "KT" // CHO MOI
                        || query.MAPB == "LG" || query.MAPB == "ML" || query.MAPB == "NL" || query.MAPB == "TM" // CHO MOI
                        || query.MAPB == "LA" || query.MAPB == "NC" || query.MAPB == "VH") // TAN CHAU)
                {//giao ho so roi

                    ////var objList = _ddkpoDao.GetListForTcdldmPB(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                    ////                txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                    ////                ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue, query.MAPB.ToString());
                    var objList = _ddkpoDao.GetListForTcdldmPBcm(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                    txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                    ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue, query.MAPB.ToString(),
                                    txtCMND.Text.Trim());

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    //var objList = _ddkpoDao.GetListForTcdldm(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                    //                txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                    //                ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue);
                    var objList = _ddkpoDao.GetListForTcdldmcm(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                    txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                    ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, 
                                    ddlToNhaMay.SelectedValue,
                                    //ddlPHUONG.SelectedValue,
                                    txtCMND.Text.Trim());

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                //}
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
                    var kvList = _kvpoDao.GetListKVPO(d);
                    var khuvuc = _kvpoDao.GetPo(d);
                    var phuongList = _ppoDao.GetList(d);

                    ddlKHUVUC.Items.Clear();
                    ddlPHUONG.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                    foreach (var ph in phuongList)
                    {
                        ddlPHUONG.Items.Add(new ListItem(ph.TENPHUONG, ph.MAPHUONGPO));
                    }
                    
                }
                
            }
        }

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;

                var khuvuclist = _kvpoDao.GetList();

                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in khuvuclist)
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));                

                var mdsdList = _mdsdpoDao.GetList();
                ddlMUCDICH.Items.Clear();
                ddlMUCDICH.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var mdsd in mdsdList)
                    ddlMUCDICH.Items.Add(new ListItem(mdsd.TENMDSD, mdsd.MAMDSDPO));

                //ddlMUCDICH.SelectedIndex = 1;
                ddlMUCDICH.SelectedIndex = 0;

                txtMADDK.Text = "";
                txtMADDK.Focus();
                txtNGAYCD.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYKS.Text = DateTime.Now.ToString("dd/MM/yyyy");

                timkv();

                LoadDynamicReferences();

                listPhongBan();

                LoadToNhaMay(ddlKHUVUC.SelectedValue);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadDynamicReferences()
        {
            // bind dllPHUONG
            var items = _ppoDao.GetListKV(ddlKHUVUC.SelectedValue);

            ddlPHUONG.Items.Clear();
            ddlPHUONG.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var p in items)
                ddlPHUONG.Items.Add(new ListItem(p.TENPHUONG, p.MAPHUONGPO));
        }

        private bool IsDataValid()
        {
            #region check id
            // check MADDK
            if (string.Empty.Equals(txtMADDK.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn đăng ký"), txtMADDK.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtMADP.Text.Trim()))
            {
                var dp = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

                if (dp == null)
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đường phố"), txtMADP.ClientID);
                    return false;
                }
            }
            #endregion

            #region check integer
            if (!string.Empty.Equals(txtSOHODN.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtSOHODN.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Số hộ đấu nối"), txtSOHODN.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtSONK.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtSONK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Số nhân khẩu"), txtSONK.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtDMNK.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtDMNK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Định mức / NK"), txtDMNK.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtDINHMUC.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtDINHMUC.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Định mức"), txtDINHMUC.ClientID);
                    return false;
                }
            }

            #endregion

            #region check datetime
            // check datetime textboxes
            if (!string.Empty.Equals(txtNGAYCD.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYCD.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày nhận đơn"), txtNGAYCD.ClientID);
                    return false;
                }
            }

            // check datetime textboxes
            if (!string.Empty.Equals(txtNGAYKS.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày hẹn khảo sát"), txtNGAYKS.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtNGAYCD.Text.Trim()) && !string.Empty.Equals(txtNGAYKS.Text.Trim()))
            {
                var ngaycd = DateTimeUtil.GetVietNamDate(txtNGAYCD.Text.Trim());
                var ngayks = DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());

                if (ngayks < ngaycd)
                {
                    ShowError("Ngày hẹn khảo sát phải sau ngày nhận đơn", txtNGAYKS.ClientID);
                    return false;
                }
            }
            #endregion

            return true;
        }

        private void ClearContent()
        {
            //TODO: xóa UI
            txtMADDK.Text = "";
            txtMADDK.ReadOnly = false;
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtDIENTHOAI.Text = "";
            txtMADP.Text = "";
            txtSOHODN.Text = "";
            txtSONK.Text = "";
            txtDMNK.Text = "";
            txtNGAYCD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNGAYKS.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cbDAIDIEN.Checked = false;
            ddlKHUVUC.SelectedIndex = 0;
            LoadDynamicReferences();
            ddlPHUONG.SelectedIndex = 0;
            ddlMUCDICH.SelectedIndex = 0;
            txtDIACHIKHAC.Text = "";
            txtCMND.Text = "";
            txtMST.Text = "";
            cbSDInfo_INHOADON.Checked = false;
            txtDIACHI_INHOADON.Text = "";
            txtTENKH_INHOADON.Text = "";
            txtDCLAPDAT.Text = "";
            txtUYQUYEN.Text = "";
            cbISTUYENONGCHUNG.Checked = false;

            lbNGAYNHAPDON.Text = "";
            lbNGAYNHAPTK.Text = "";
            lbNGAYCHIETTINH.Text = "";
            lbNGAYHOPDONG.Text = "";
            lbNGAYTHICONG.Text = "";
            lbNGAYBBNT.Text = "";
            lbNGAYKHAITHAC.Text = "";
            txtSOHD1.Text = "";
            lbSONODH.Text = "";

            txtNOIDUNGKEMTHEO.Text = "";
            txtSOTRU.Text = "";

            txtMADPDLM.Text = "";
            txtMADBDLM.Text = "";
            ddlToNhaMay.SelectedIndex = 0;

        }

        private void SetDDKToForm(DONDANGKYPO ddk)
        {
            SetControlValue(txtMADDK.ClientID, ddk.MADDKPO);
            SetReadonly(txtMADDK.ClientID, true);
            SetControlValue(txtTENKH.ClientID, ddk.TENKH);
            SetControlValue(txtSONHA.ClientID, ddk.SONHA);
            SetControlValue(txtDIENTHOAI.ClientID, ddk.DIENTHOAI);
            SetControlValue(txtDIACHIKHAC.ClientID, ddk.TEN_DC_KHAC);
            SetControlValue(txtDCLAPDAT.ClientID, ddk.DIACHILD);

            if (ddk.DUONGPHOPO != null)
            {
                SetControlValue(txtMADP.ClientID, ddk.MADPPO);
                SetControlValue(txtDUONGPHU.ClientID, ddk.DUONGPHUPO);
            }

            SetControlValue(txtSOHODN.ClientID, ddk.SOHODN.HasValue ? String.Format("{0:0,0}", ddk.SOHODN.Value) : "");
            SetControlValue(txtSONK.ClientID, ddk.SONK.HasValue ? String.Format("{0:0,0}", ddk.SONK.Value) : "");
            SetControlValue(txtDMNK.ClientID, ddk.DMNK.HasValue ? String.Format("{0:0,0}", ddk.DMNK.Value) : "");
            SetControlValue(txtNGAYCD.ClientID, ddk.NGAYDK.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYDK.Value) : "");
            SetControlValue(txtNGAYKS.ClientID, ddk.NGAYHKS.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYHKS.Value) : "");
            txtNGAYCAPCMND.Text = ddk.CAPNGAY.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.CAPNGAY.Value) : "";

            var kv = ddlKHUVUC.Items.FindByValue(ddk.MAKVPO);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            LoadDynamicReferences();

            var p = ddlPHUONG.Items.FindByValue(ddk.MAPHUONG);
            if (p != null)
                ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(p);

            var mdsd = ddlMUCDICH.Items.FindByValue(ddk.MAMDSDPO);
            if (mdsd != null)
                ddlMUCDICH.SelectedIndex = ddlMUCDICH.Items.IndexOf(mdsd);

            txtCMND.Text = ddk.CMND;
            txtMST.Text = ddk.MST;

            var isChecked = ddk.SDInfo_INHOADON.HasValue && ddk.SDInfo_INHOADON.Value;
            cbSDInfo_INHOADON.Checked = isChecked;
            if (isChecked)
            {
                txtTENKH_INHOADON.Text = ddk.TENKH_INHOADON;
                txtDIACHI_INHOADON.Text = ddk.DIACHI_INHOADON;
            }
            else
            {
                txtTENKH_INHOADON.Text = "";
                txtDIACHI_INHOADON.Text = "";
            }

            var isCheckedONGCAI = ddk.ISTUYENONGCHUNG.HasValue && ddk.ISTUYENONGCHUNG.Value;
            cbISTUYENONGCHUNG.Checked = isCheckedONGCAI;

            if (ddk.TENDK != null)
            { txtUYQUYEN.Text = ddk.TENDK; }

            if (ddk.NOILAPDHHN != null)
            {
                txtNOILAPDHN.Text = ddk.NOILAPDHHN;
            }
            else
            {
                txtNOILAPDHN.Text = "";
            }

            var sohd = _hdpoDao.Get(ddk.MADDKPO);
            if (sohd != null)
            {
                txtSOHD1.Text = sohd.SOHD.ToString();

                txtMADPDLM.Text = sohd.MADPPO != null ? sohd.MADPPO : "";
                txtMADBDLM.Text = sohd.MADB != null ? sohd.MADB : "";
            }
            else
            {
                txtSOHD1.Text = "";

                txtMADPDLM.Text = "";
                txtMADBDLM.Text = "";
            }

            LoadDateDon(ddk.MADDKPO);

            var dq = _dqDao.Get(ddk.MADDKPO);
            if (dq != null)
            {
                var pb2 = ddlPHONGBAN2.Items.FindByValue(dq.MAPB);
                if (pb2 != null)
                    ddlPHONGBAN2.SelectedIndex = ddlPHONGBAN2.Items.IndexOf(pb2);
            }

            txtNOIDUNGKEMTHEO.Text = ddk.NOIDUNG != null ? ddk.NOIDUNG : "";

            if (_tkpoDao.Get(ddk.MADDKPO) != null)
            {
                txtSOTRU.Text = _tkpoDao.Get(ddk.MADDKPO).SOTRUKH != null ? _tkpoDao.Get(ddk.MADDKPO).SOTRUKH : "";
            }

            var chiettinh = ctDao.Get(ddk.MADDKPO);
            if (chiettinh != null)
            {
                if (chiettinh.TONGTIENCTPS != null)
                    lbTongTienTK.Text = String.Format("{0:0,0}", chiettinh.TONGTIENCTPS);
                else
                    lbTongTienTK.Text = "0";
            }
            else
            {
                lbTongTienTK.Text = "0";
            }

            var tonhamay = ddlToNhaMay.Items.FindByValue(ddk.MADDKPO.Substring(1, 2));
            if (tonhamay != null)
                ddlToNhaMay.SelectedIndex = ddlToNhaMay.Items.IndexOf(tonhamay);           

            upnlInfor.Update();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "showDKStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKyPo = _ddkpoDao.Get(madon);
                            upnlDangKy.Update();
                            UnblockDialog("divDangKy");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showTKStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKyPo = _ddkpoDao.Get(madon);
                            ThietKePo = _tkpoDao.Get(madon);
                            upnlThietKe.Update();
                            UnblockDialog("divThietKe");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showCTStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKyPo = _ddkpoDao.Get(madon);
                            ThietKePo = _tkpoDao.Get(madon);
                            ChietTinh = ctDao.Get(madon);
                            if (ChietTinh != null)
                                txtGHICHUCT.Text = ChietTinh.GHICHU;
                            upnlChietTinh.Update();
                            UnblockDialog("divChietTinh");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showHDStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKyPo = _ddkpoDao.Get(madon);
                            ThietKePo = _tkpoDao.Get(madon);
                            ChietTinh = ctDao.Get(madon);
                            HopDongPo = _hdpoDao.Get(madon);

                            if (HopDongPo != null)
                            {
                                var kyduyet = _kyduyetDao.GetMaTT(madon, "HD_N");
                                lvNVLAPHĐ.Text = _nvDao.Get(kyduyet.MANV).HOTEN.ToString();
                            }
                            else { lvNVLAPHĐ.Text = ""; }

                            upnlHopDong.Update();
                            UnblockDialog("divHopDong");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showTCStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKyPo = _ddkpoDao.Get(madon);
                            ThietKePo = _tkpoDao.Get(madon);
                            ChietTinh = ctDao.Get(madon);
                            HopDongPo = _hdpoDao.Get(madon);
                            ThiCong = tcDao.Get(madon);     //TC_P

                            if (ThiCong != null)
                            {
                                var kyduyet = _kyduyetDao.GetMaTT(madon, "TC_P");
                                if (kyduyet != null)
                                { 
                                    lvNVLAPTHICONG.Text = _nvDao.Get(kyduyet.MANV).HOTEN.ToString(); 
                                }
                                else 
                                { 
                                    lvNVLAPTHICONG.Text = ""; 
                                }
                            }
                            else 
                            { 
                                lvNVLAPTHICONG.Text = ""; 
                            }

                            upnlThiCong.Update();
                            UnblockDialog("divThiCong");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showNTStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            var dondangkypo = _ddkpoDao.Get(madon);
                            var thietkepo = _tkpoDao.Get(madon);
                            var chiettinh = ctDao.Get(madon);
                            var hopdongpo = _hdpoDao.Get(madon);
                            var thicong = tcDao.Get(madon);
                            var nghiemthupo = _bbntpoDao.Get(madon);

                            if (nghiemthupo != null)
                            {
                                lbMADDKBBNT.Text = dondangkypo.MADDKPO;
                                lbTENKHBBNT.Text = dondangkypo.TENKH;
                                lbNTNGAYNHAPDON.Text = dondangkypo.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", dondangkypo.NGAYN) : "";
                                lbNTNGAYTK.Text = thietkepo.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", thietkepo.NGAYN) : "";
                                lbNTNGAYCT.Text = chiettinh.NGAYLCT != null ? String.Format("{0:dd/MM/yyyy}", chiettinh.NGAYLCT) : "";
                                lbNTNGAYHD.Text = hopdongpo.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", hopdongpo.NGAYN) : "";
                                lbNTNGAYTC.Text = thicong.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", thicong.NGAYN) : "";
                                lbNTNGAYLAPBBNT.Text = nghiemthupo.NGAYNHAP != null ? String.Format("{0:dd/MM/yyyy}", nghiemthupo.NGAYNHAP) : "";
                            }
                            else
                            {
                                lbMADDKBBNT.Text = dondangkypo.MADDKPO;
                                lbTENKHBBNT.Text = dondangkypo.TENKH;
                                lbNTNGAYNHAPDON.Text = dondangkypo.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", dondangkypo.NGAYN) : "";
                                lbNTNGAYTK.Text = "";
                                lbNTNGAYCT.Text = "";
                                lbNTNGAYHD.Text = "";
                                lbNTNGAYTC.Text = "";
                                lbNTNGAYLAPBBNT.Text = "";
                            }

                            upnlNghiemThu.Update();
                            UnblockDialog("divNghiemThu");

                            CloseWaitingDialog();
                        }
                        break;

                    case "EditItem":
                        if (!string.Empty.Equals(madon))
                        {
                            var don = _ddkpoDao.Get(madon);
                            if (don == null) return;
                            SetDDKToForm(don);
                        }
                        CloseWaitingDialog();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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

            var source = gvList.DataSource as List<DONDANGKYPO>;
            if (source == null) return;

            var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;

            var imgDK = e.Row.FindControl("imgDK") as Button;
            var imgTK = e.Row.FindControl("imgTK") as Button;
            var imgCT = e.Row.FindControl("imgCT") as Button;
            var imgHD = e.Row.FindControl("imgHD") as Button;
            var imgTC = e.Row.FindControl("imgTC") as Button;
            var imgNT = e.Row.FindControl("imgNT") as Button;

            if (imgDK != null)
            {
                imgDK.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgDK) + "')");
                var maTTDK = source[index].TTDK;
                var ttdk = ttDao.Get(maTTDK);

                if (ttdk != null)
                {
                    imgDK.Attributes.Add("class", ttdk.COLOR);
                    imgDK.ToolTip = ttdk.TENTT;
                }
                else
                {
                    imgDK.ToolTip = "Chưa duyệt khảo sát";
                    imgDK.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgTK != null)
            {
                imgTK.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTK) + "')");
                var maTTTK = source[index].TTTK;
                var tttk = ttDao.Get(maTTTK);

                if (tttk != null)
                {
                    imgTK.Attributes.Add("class", tttk.COLOR);
                    imgTK.ToolTip = tttk.TENTT;
                }
                else
                {
                    imgTK.ToolTip = "Chưa nhập thiết kế";
                    imgTK.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgCT != null)
            {
                imgCT.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgCT) + "')");
                var maTTCT = source[index].TTCT;
                var ttct = ttDao.Get(maTTCT);

                if (ttct != null)
                {
                    imgCT.Attributes.Add("class", ttct.COLOR);
                    imgCT.ToolTip = ttct.TENTT;
                }
                else
                {
                    imgCT.ToolTip = "Chưa lập chiết tính";
                    imgCT.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgHD != null)
            {
                imgHD.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgHD) + "')");
                var maTTHD = source[index].TTHD;
                var tthd = ttDao.Get(maTTHD);

                if (tthd != null)
                {
                    imgHD.Attributes.Add("class", tthd.COLOR);
                    imgHD.ToolTip = tthd.TENTT;
                }
                else
                {
                    imgHD.ToolTip = "Chưa nhập hợp đồng";
                    imgHD.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgTC != null)
            {
                imgTC.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTC) + "')");
                var maTTTC = source[index].TTTC;
                var tttc = ttDao.Get(maTTTC);

                if (tttc != null)
                {
                    imgTC.Attributes.Add("class", tttc.COLOR);
                    imgTC.ToolTip = tttc.TENTT;
                }
                else
                {
                    imgTC.ToolTip = "Chưa nhập thi công";
                    imgTC.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgNT != null)
            {
                imgNT.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgNT) + "')");
                var maTTNT = source[index].TTNT;
                var ttnt = ttDao.Get(maTTNT);

                if (ttnt != null)
                {
                    imgNT.Attributes.Add("class", ttnt.COLOR);
                    imgNT.ToolTip = ttnt.TENTT;
                }
                else
                {
                    imgNT.ToolTip = "Chưa nhập nghiệm thu";
                    imgNT.Attributes.Add("class", "noneIndicator");
                }
            }

            if (e.Row.Cells.Count < 5)
                return;
            var fifth = e.Row.Cells[e.Row.Cells.Count - 1];
            var fourth = e.Row.Cells[e.Row.Cells.Count - 2];
            var third = e.Row.Cells[e.Row.Cells.Count - 3];
            var second = e.Row.Cells[e.Row.Cells.Count - 4];
            var first = e.Row.Cells[e.Row.Cells.Count - 5];

            if (fifth == null || fourth == null || second == null ||
                third == null || first == null)
                return;

            fifth.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");
            fourth.Attributes.Add("style", "border-left: none 0px; border-right: none 0px; padding: 6px 0 4px !important;");
            third.Attributes.Add("style", "border-left: none 0px; border-right: none 0px; padding: 6px 0 4px !important;");
            second.Attributes.Add("style", "border-left: none 0px; border-right: none 0px; padding: 6px 0 4px !important;");
            first.Attributes.Add("style", "border-right: none 0px; padding: 6px 0 4px !important;");

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDynamicReferences();

                txtMADP.Text = "";
                txtDUONGPHU.Text = "";
                txtDIACHIKHAC.Text = "";

                LoadToNhaMay(ddlKHUVUC.SelectedValue);

                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadToNhaMay(string makvpo)
        {
            try
            {
                var khuvucpo = _kvpoDao.Get(makvpo);
                var phongban = _pbDao.GetListKV(khuvucpo.MAKV);

                ddlToNhaMay.Items.Clear();
                ddlToNhaMay.Items.Add(new ListItem("Tất cả", "%"));
                ddlToNhaMay.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                ddlToNhaMay.Items.Add(new ListItem("Phòng KT Điện Nước", "KTDN"));
                foreach (var pb in phongban)
                {
                    ddlToNhaMay.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                }
            }
            catch { }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Authenticate
            if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
            {
                CloseWaitingDialog();
                ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            var don = DonDangKyObjPo;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }

            var maddkkh = _khpoDao.GetMADDK(don.MADDKPO);
            if (maddkkh != null)
            {
                CloseWaitingDialog();
                ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                return;
            }

            _rpClass.HisBienCo(don.MADDKPO, don.MAKVPO, LoginInfo.MANV, "DONPOHIS");

            Filtered = FilteredMode.None;

            var msg = _ddkpoDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

            _rpClass.HisNgayDangKyBienPo(don.MADDKPO, LoginInfo.MANV, don.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                   "", "", "", "", "UPDONDK");

            //update duyet_quyen
            if (don.TTTK != null)
            {        
                if ((_nvDao.Get(LoginInfo.MANV).MAPB.Equals("KD") || LoginInfo.MANV.Equals("nguyen")) && don.TTDK.Equals("DK_A")
                    && don.TTTK.Equals("TK_N"))
                {
                    if (_dqDao.Get(don.MADDKPO) != null)
                    {
                        var msqDQ = _dqDao.Update(don.MADDKPO, LoginInfo.MANV, ddlPHONGBAN2.SelectedValue, don.MAKVPO);                        
                    }
                    else
                    {
                        if (ddlPHONGBAN2.SelectedValue == "%")
                        {
                            CloseWaitingDialog();
                            ShowError("Chọn nơi giao hồ sơ cho đúng.");
                            return;
                        }
                        else
                        {
                            _rpClass.BienKHNuoc(don.MADDKPO, don.MAKVPO, ddlPHONGBAN2.SelectedValue, LoginInfo.MANV, 1, 1, "INDUYETQTS");
                        }
                    }
                }
            }

            CloseWaitingDialog();

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                ShowInfor(ResourceLabel.Get(msg));
                //Trả lại màn hình trống ban đầu
                ClearContent();

                // Refresh grid view
                BindDataForGrid();
                upnlGrid.Update();

            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            Filtered = FilteredMode.None;
            BindDataForGrid();
            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Filtered = FilteredMode.None;

                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKYPO>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (!_ddkpoDao.IsInUse(ma)) continue;

                        var ddk = _ddkpoDao.Get(ma);
                        var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đơn đăng ký", ddk.TENKH);

                        CloseWaitingDialog();
                        ShowError(ResourceLabel.Get(msgIsInUse));
                        return;
                    }

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                    //TODO: check relation before update list
                    var msg = _ddkpoDao.DoAction(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    if (msg.MsgType != MessageType.Error)
                    {
                        CloseWaitingDialog();

                        ShowInfor(ResourceLabel.Get(msg));

                        ClearContent();

                        // Refresh grid view
                        BindDataForGrid();

                        upnlGrid.Update();
                    }
                    else
                    {
                        CloseWaitingDialog();

                        ShowError(ResourceLabel.Get(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void UpdateKhuVuc(DUONGPHOPO dp)
        {
            SetControlValue(txtDIACHIKHAC.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKVPO);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
                LoadDynamicReferences();
            }
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
                ShowError("Mã đường phố không hợp lệ");
            }
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

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
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
                            upnlInfor.Update();
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

        private void EventEnter()
        {
            //txtSOKD.Attributes.Add("onkeypress", "return clickButton(event)");
            //ddlMALDH.Attributes.Add("onkeypress", "return clickButtonddlMALDH(event)");
            txtMADDK.Attributes.Add("onkeypress", "return clickButtonbtnFilter(event)");
            txtTENKH.Attributes.Add("onkeypress", "return clickButtonbtnFilter(event)");
            txtDIENTHOAI.Attributes.Add("onkeypress", "return clickButtonbtnFilter(event)");
            txtCMND.Attributes.Add("onkeypress", "return clickButtonbtnFilter(event)");
        }

        private void LoadDateDon(String maddk)
        {
            try
            {
                String _dontt = "DK_A", _tktt = "TK_P", _cttt = "CT_N", _hdtt = "HD_N", _tctt = "TC_N", _nttt = "NT_A";//Thêm mới thi công.
                String _donmota = "Nhập đơn lắp mới", _tkmota = "Nhập thiết kế.", _ctmota = "Chạy chiết tính", _hdmota = "Nhập hợp đồng", _tcmota = "Thêm mới thi công.", _ntmota = "Nhập biên bản nghiệm thu.";

                var dondk = _lvkdDao.GetMaDon(maddk, _dontt, _donmota);
                if (dondk != null)
                { lbNGAYNHAPDON.Text = dondk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                else { lbNGAYNHAPDON.Text = ""; }

                var tkdk = _lvkdDao.GetMaDon(maddk, _tktt, _tkmota);
                if (tkdk != null)
                { lbNGAYNHAPTK.Text = tkdk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                else { lbNGAYNHAPTK.Text = ""; }

                var ctdk = _lvkdDao.GetMaDon(maddk, _cttt, _ctmota);
                if (ctdk != null)
                { lbNGAYCHIETTINH.Text = ctdk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                else { lbNGAYCHIETTINH.Text = ""; }

                var hddk = _lvkdDao.GetMaDon(maddk, _hdtt, _hdmota);
                if (hddk != null)
                { lbNGAYHOPDONG.Text = hddk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                else { lbNGAYHOPDONG.Text = ""; }

                var tcdk = _lvkdDao.GetMaDon(maddk, _tctt, _tcmota);
                if (tcdk != null)
                { lbNGAYTHICONG.Text = tcdk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                else { lbNGAYTHICONG.Text = ""; }

                var ntdk = _lvkdDao.GetMaDon(maddk, _nttt, _ntmota);
                var dateBBNT = _bbntpoDao.Get(maddk);
                var maddkkh = _khpoDao.GetMADDK(maddk);

                if (dateBBNT != null)
                {
                    lbNGAYBBNT.Text = dateBBNT.NGAYNHAP.Value.ToString("dd/MM/yyyy");
                }
                else { lbNGAYBBNT.Text = ""; };

                if (maddkkh != null)
                {
                    lbNGAYKHAITHAC.Text = maddkkh.KYKHAITHAC.ToString("MM/yyyy");
                }
                else { lbNGAYKHAITHAC.Text = ""; }

                var mathicong = tcDao.Get(maddk);

                if (mathicong != null)
                {
                    if (mathicong.MADH != null)
                    {
                        var dhno = _dhpoDao.Get(mathicong.MADH);
                        lbSONODH.Text = dhno.SONO.ToString();
                    }
                    else { lbSONODH.Text = ""; }
                }
                else { lbSONODH.Text = ""; }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void listPhongBan()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.Get(b);
            //foreach (var a in query)
            //{
            //string d = a.MAKV;

            if (query.MAKV == "O")
            {
                if (query.MAPB == "NB")
                {
                    ddlPHONGBAN2.Items.Clear();
                    ddlPHONGBAN2.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                }
                if (query.MAPB == "TA")
                {
                    ddlPHONGBAN2.Items.Clear();
                    ddlPHONGBAN2.Items.Add(new ListItem("Tổ An Châu", "TA"));
                }
                if (query.MAPB == "TD")
                {
                    ddlPHONGBAN2.Items.Clear();
                    ddlPHONGBAN2.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                }
                if (query.MAPB == "KD")
                {
                    ddlPHONGBAN2.Items.Clear();
                    ddlPHONGBAN2.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                    ddlPHONGBAN2.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                    ddlPHONGBAN2.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                    ddlPHONGBAN2.Items.Add(new ListItem("Tổ An Châu", "TA"));
                    ddlPHONGBAN2.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                }
            }
            else
            {
                ddlPHONGBAN2.Items.Clear();
                if (query.MAPB == "KD")
                {
                    var kvList = _pbDao.GetListKV(query.MAKV);
                    ddlPHONGBAN2.Items.Add(new ListItem("Tất cả", "%"));
                    ddlPHONGBAN2.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                    ddlPHONGBAN2.Items.Add(new ListItem("Phòng KT Điện Nước", "KTDN"));
                    foreach (var pb in kvList)
                    {
                        ddlPHONGBAN2.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                    }
                }
                else
                {
                    var pbList = _pbDao.GetListPB(query.MAPB);
                    foreach (var pb in pbList)
                    {
                        ddlPHONGBAN2.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                    }
                }
            }
            
            var phongban = _pbDao.GetList();
            ddlToNhaMay.Items.Clear();
            ddlToNhaMay.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var pb in phongban)
            {
                ddlToNhaMay.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
            }
        }

        protected void btTIMSONODHN_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSONODHN.Text.Trim()))
                {
                    Filtered = FilteredMode.Filtered;

                    var objList = _ddkpoDao.GetListDonDongHo(ddlKHUVUC.SelectedValue, txtSONODHN.Text.Trim());

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }

                upnlGrid.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

    }
}