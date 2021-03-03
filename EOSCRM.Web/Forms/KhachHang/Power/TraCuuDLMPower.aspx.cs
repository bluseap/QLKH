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
        private readonly StoredProcedureDao _spDao = new StoredProcedureDao();
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

                string sonhatenduongKH = txtSoNhaTenDuongKH.Text.Trim();
                string[] sonhatenduongKH1 = sonhatenduongKH.Split(' ');

                string sonhatenduongDCL = txtSoNhaTenDuongDCL.Text.Trim();
                string[] sonhatenduongDCL1 = sonhatenduongDCL.Split(' ');

                obj.SoNhaKH = sonhatenduongKH1[0].ToString();
                obj.TenDuongKH = sonhatenduongKH.Substring(sonhatenduongKH1.Length, sonhatenduongKH.Length - sonhatenduongKH1.Length);
                obj.ThanhPhoTinhIdKH = Convert.ToInt32(ddlTinhKH.SelectedValue.ToString());
                obj.QuanHuyenIdKH = Convert.ToInt32(ddlThanhPhoHuyenKH.SelectedValue.ToString());
                obj.PhuongXaIdKH = Convert.ToInt32(ddlPhuongXaKH.SelectedValue.ToString());
                obj.ApToIdKH = ddlToApKH.SelectedValue.ToString();

                obj.SoNhaLD = sonhatenduongDCL1[0].ToString();
                obj.TenDuongLD = sonhatenduongDCL.Substring(sonhatenduongDCL1.Length, sonhatenduongDCL.Length - sonhatenduongDCL1.Length);
                obj.ThanhPhoTinhIdLD = Convert.ToInt32(ddlTinhDCL.SelectedValue.ToString());
                obj.QuanHuyenIdLD = Convert.ToInt32(ddlThanhPhoHuyenDCL.SelectedValue.ToString());
                obj.PhuongXaIdLD = Convert.ToInt32(ddlPhuongXaDCL.SelectedValue.ToString());
                obj.ApToIdLD = ddlToApDCL.SelectedValue.ToString();

                return obj;
            }
        }
                
        #endregion

        #region loc, up
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
                    if (ckIsXoaDLM.Checked)
                    {
                        var objList = _ddkpoDao.GetListByXoaDLM(ckIsXoaDLM.Checked);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {                        
                        var objList = _ddkpoDao.GetListForTcdldmPBcm(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                    txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                    ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue, query.MAPB.ToString(),
                                    txtCMND.Text.Trim());

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }                    
                }
                else
                {
                    if (ckIsXoaDLM.Checked)
                    {
                        var objList = _ddkpoDao.GetListByXoaDLM(ckIsXoaDLM.Checked);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
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
                }
                ClearContent();
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
            UpdateMode = Mode.Create;

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

            //lbNGAYNHAPDON.Text = "";
            //lbNGAYNHAPTK.Text = "";
            //lbNGAYCHIETTINH.Text = "";
            //lbNGAYHOPDONG.Text = "";
            //lbNGAYTHICONG.Text = "";
            //lbNGAYBBNT.Text = "";
            lbNGAYKHAITHAC.Text = "";
            txtSOHD1.Text = "";
            lbSONODH.Text = "";

            txtNOIDUNGKEMTHEO.Text = "";
            txtSOTRU.Text = "";

            txtMADPDLM.Text = "";
            txtMADBDLM.Text = "";
            ddlToNhaMay.SelectedIndex = 0;

            ckIsXoaDLM.Checked = false;
            txtGhiChuXoaDLM.Text = "";
            txtGhiChuXoaDLM.Enabled = false;
            btnDelete.Visible = false;

            txtNamSinhDLM.Text = "";

            txtSoNhaTenDuongKH.Text = "";
            ddlTinhKH.SelectedIndex = 1;
            ddlThanhPhoHuyenKH.SelectedIndex = 0;
            ddlPhuongXaKH.SelectedIndex = 0;
            ddlToApKH.SelectedIndex = 0;

            txtSoNhaTenDuongDCL.Text = "";
            ddlTinhDCL.SelectedIndex = 0;
            ddlThanhPhoHuyenDCL.SelectedIndex = 0;
            ddlPhuongXaDCL.SelectedIndex = 0;
            ddlToApDCL.SelectedIndex = 0;
        }

        private void SetDDKToForm(DONDANGKYPO ddk)
        {
            txtMADDK.Text = ddk.MADDKPO;
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

            //Xoa don lap moi
            var isCheckedXoaDLM = ddk.IsXoaDLM is null || ddk.IsXoaDLM == false ? false : ddk.IsXoaDLM.Value;
            ckIsXoaDLM.Checked = isCheckedXoaDLM;            
            if (isCheckedXoaDLM)
            {
                txtGhiChuXoaDLM.Enabled = true;
                btnDelete.Visible = true;
            }
            else
            {
                txtGhiChuXoaDLM.Enabled = false;
                btnDelete.Visible = false;
            }
            txtGhiChuXoaDLM.Text = ddk.GhiChuXoaDLM != null ? ddk.GhiChuXoaDLM.ToString() : "";

            txtNamSinhDLM.Text = ddk.NGAYSINH.Substring(ddk.NGAYSINH.Length - 4, 4);


            txtSoNhaTenDuongKH.Text = ddk.SoNhaKH + " " + ddk.TenDuongKH;

            var thanhphotinhidkh = ddlTinhKH.Items.FindByValue(ddk.ThanhPhoTinhIdKH.ToString());
            ddlTinhKH.SelectedIndex = ddlTinhKH.Items.IndexOf(thanhphotinhidkh);

            var quanhuyenidkh = ddlThanhPhoHuyenKH.Items.FindByValue(ddk.QuanHuyenIdKH.ToString());
            ddlThanhPhoHuyenKH.SelectedIndex = ddlThanhPhoHuyenKH.Items.IndexOf(quanhuyenidkh);

            var phuongxaidkh = ddlPhuongXaKH.Items.FindByValue(ddk.PhuongXaIdKH.ToString());
            ddlPhuongXaKH.SelectedIndex = ddlPhuongXaKH.Items.IndexOf(phuongxaidkh);

            var aptoidkh = ddlToApKH.Items.FindByValue(ddk.ApToIdKH.ToString());
            ddlToApKH.SelectedIndex = ddlToApKH.Items.IndexOf(aptoidkh);

            txtSoNhaTenDuongDCL.Text = ddk.SoNhaLD + " " + ddk.TenDuongLD;

            var thanhphotinhidld = ddlTinhDCL.Items.FindByValue(ddk.ThanhPhoTinhIdLD.ToString());
            ddlTinhDCL.SelectedIndex = ddlTinhDCL.Items.IndexOf(thanhphotinhidld);

            var quanhuyenidld = ddlThanhPhoHuyenDCL.Items.FindByValue(ddk.QuanHuyenIdLD.ToString());
            ddlThanhPhoHuyenDCL.SelectedIndex = ddlThanhPhoHuyenDCL.Items.IndexOf(quanhuyenidld);

            var phuongxaidld = ddlPhuongXaDCL.Items.FindByValue(ddk.PhuongXaIdLD.ToString());
            ddlPhuongXaDCL.SelectedIndex = ddlPhuongXaDCL.Items.IndexOf(phuongxaidld);

            var aptoidld = ddlToApDCL.Items.FindByValue(ddk.ApToIdLD.ToString());
            ddlToApDCL.SelectedIndex = ddlToApDCL.Items.IndexOf(aptoidld);

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
                            var dondangky = _ddkpoDao.Get(madon);

                            lbMaddkDK.Text = dondangky.MADDKPO;
                            lbTenKHDK.Text = dondangky.TENKH;

                            lbNgayDkDK.Text = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";

                            lbNgayChuyenHSToTK.Text = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";

                            string songaylamhosoDDK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                            lbSoNgayLamHoSo.Text = songaylamhosoDDK;

                            lbNoiDungDK.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                            lbNhanVienDK.Text = _nvDao.Get(dondangky.MANV).HOTEN;

                            UpdateMode = Mode.Update;                            

                            upnlDangKy.Update();
                            UnblockDialog("divDangKy");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showTKStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            var dondangky = _ddkpoDao.Get(madon);
                            var thietke = _tkpoDao.Get(madon);

                            lbMadkTK.Text = dondangky.MADDKPO;
                            lbTenKHTK.Text = dondangky.TENKH;

                            string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                            string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                            lbTKNGAYDK.Text = ngaydkTK + " - " + ngayduyetdonTK;

                            lbGhiChuDDKTK.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                          
                            string songaylamhosoDDK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                              _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                            lbTKSoNgayLamHS.Text = songaylamhosoDDK;

                            lbNoiDungDonDangKyTK.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";

                            if (thietke != null)
                            {
                                lbTKNGAYTK.Text = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";

                                lbTKNVPHUTRACH.Text = thietke.TENNVTK != null ? thietke.TENNVTK : "";
                                lbTKNVDUYET.Text = thietke.MANVDTK != null ? _nvDao.Get(thietke.MANVDTK).HOTEN : "";

                                lbTKNGAYDUYET.Text = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";

                                string songaythietkeTK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbTKSoNgayThietKe.Text = songaythietkeTK;

                                lbTKNVNHAP.Text = thietke.MANVLTK != null ? _nvDao.Get(thietke.MANVLTK).HOTEN : "";
                                lbGhiChuTK.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                            }
                            else
                            {
                                lbTKNGAYTK.Text = "";
                                lbTKNVPHUTRACH.Text = "";
                                lbTKNVDUYET.Text = "";
                                lbTKSoNgayThietKe.Text = "";
                                lbTKNVNHAP.Text = "";
                                lbGhiChuTK.Text = "";
                            }                            

                            UpdateMode = Mode.Update;
                            upnlThietKe.Update();
                            UnblockDialog("divThietKe");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showCTStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            var dondangky = _ddkpoDao.Get(madon);
                            var thietke = _tkpoDao.Get(madon);
                            var chiettinh = ctDao.Get(madon);

                            lbMadkCT.Text = dondangky.MADDKPO;
                            lbTenKHCT.Text = dondangky.TENKH;

                            string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                            string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                            lbCTNGAYNHAPDON.Text = ngaydkTK + " - " + ngayduyetdonTK;
                            lbGhiChuDDKCT.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";

                            string songaylamhosoDDK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                              _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                            lbCTSoNgayLamHS.Text = songaylamhosoDDK;

                            if (chiettinh != null)
                            {
                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbCTNGAYNHAPTK.Text = ngayltkCT + " - " + ngaydtkCT;

                                lbGhiChuTKCT.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbCTSoNgayTK.Text = songaythietkeTK;

                                lbCTNGAYLCT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";

                                string songaychiettinhCT = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbCTSoNgayCT.Text = songaychiettinhCT;

                                lbCTGHICHU.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                            }
                            else
                            {
                                lbCTNGAYNHAPTK.Text = "";
                                lbGhiChuTKCT.Text = "";
                                lbCTSoNgayTK.Text = "";
                                lbCTNGAYLCT.Text = "";
                                lbCTSoNgayCT.Text = "";
                                lbCTGHICHU.Text = "";
                            }                            

                            UpdateMode = Mode.Update;
                            upnlChietTinh.Update();
                            UnblockDialog("divChietTinh");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showHDStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            var dondangky = _ddkpoDao.Get(madon);
                            var thietke = _tkpoDao.Get(madon);
                            var chiettinh = ctDao.Get(madon);
                            var hopdong = _hdpoDao.Get(madon);

                            lbMaddkHD.Text = dondangky.MADDKPO;
                            lbTenKHHD.Text = dondangky.TENKH;
                           
                            string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                            string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                            lbHDNGAYNHAPDON.Text = ngaydkTK + " - " + ngayduyetdonTK;
                            lbGhiChuDDKHD.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";

                            string songaylamhosoDDK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                              _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                            lbSoNgayDDKHD.Text = songaylamhosoDDK;

                            if (hopdong != null)
                            {
                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbHDNGAYTK.Text = ngayltkCT + " - " + ngaydtkCT;
                                lbGhiChuTKHD.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTKHD.Text = songaythietkeTK;

                                lbNGAYCT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";
                                lbGhiChuCTHD.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayCTHD.Text = songaychiettinhCT;

                                lbHDNGAYNHAPHD.Text = hopdong.NGAYTAO != null ? hopdong.NGAYTAO.ToString() : "";
                                string songayhopdongHD = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INHOPDONG").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INHOPDONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayHopDongHD.Text = songayhopdongHD;
                                string ghichuhopdongHD = _spDao.Get_HopDongPo_ByMaddk(dondangky.MADDKPO).Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HopDongPo_ByMaddk(dondangky.MADDKPO).Tables[0].Rows[0]["GhiChu"].ToString() : "";
                                txtGhiChuHopDongHD.Text = ghichuhopdongHD;
                            }
                            else
                            {
                                lbHDNGAYTK.Text = "";
                                lbGhiChuTKHD.Text = "";
                                lbSoNgayTKHD.Text = "";
                                lbNGAYCT.Text = "";
                                lbGhiChuCTHD.Text = "";
                                lbSoNgayCTHD.Text = "";
                                lbHDNGAYNHAPHD.Text = "";
                                lbSoNgayHopDongHD.Text = "";
                                txtGhiChuHopDongHD.Text = "";
                            }

                            UpdateMode = Mode.Update;
                            upnlHopDong.Update();
                            UnblockDialog("divHopDong");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showTCStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            var dondangky = _ddkpoDao.Get(madon);
                            var thietke = _tkpoDao.Get(madon);
                            var chiettinh = ctDao.Get(madon);
                            var hopdong = _hdpoDao.Get(madon);
                            var thicong = tcDao.Get(madon);

                            lbMaddkTC.Text = dondangky.MADDKPO;
                            lbTenKHTC.Text = dondangky.TENKH;

                            string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                            string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                            lbTCNGAYNHAPDON.Text = ngaydkTK + " - " + ngayduyetdonTK;
                            lbGhiChuDKTC.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                            string songaylamhosoDDK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                              _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                            lbSoNgayDKTC.Text = songaylamhosoDDK;

                            if (thicong !=null)
                            {
                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbTCNGAYTK.Text = ngayltkCT + " - " + ngaydtkCT;
                                lbGhiChuTKTC.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTKTC.Text = songaythietkeTK;

                                lbTCNGAYCT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";
                                lbGhiChuCTTC.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayCTTC.Text = songaychiettinhCT;

                                lbTCNGAYHD.Text = hopdong.NGAYTAO != null ? hopdong.NGAYTAO.ToString() : "";
                                string songayhopdongHD = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INHOPDONG").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INHOPDONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                string ghichuhopdongHD = _spDao.Get_HopDongPo_ByMaddk(dondangky.MADDKPO).Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HopDongPo_ByMaddk(dondangky.MADDKPO).Tables[0].Rows[0]["GhiChu"].ToString() : "";
                                lbGhiChuHDTC.Text = ghichuhopdongHD;
                                lbSoNgayHDTC.Text = songayhopdongHD;

                                lbTCNGAYNHAP.Text = thicong.NGAYGTC != null ? thicong.NGAYGTC.ToString() : "";
                                lbTCGHICHU.Text = thicong.GHICHU != null ? thicong.GHICHU : "";
                                string songayTC = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHICONG").Tables[0].Rows.Count != 0 ?
                                   _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHICONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTC.Text = songayTC;
                            }
                            else
                            {
                                lbTCNGAYTK.Text = "";
                                lbGhiChuTKTC.Text = "";
                                lbSoNgayTKTC.Text = "";
                                lbTCNGAYCT.Text = "";
                                lbGhiChuCTTC.Text = "";
                                lbSoNgayCTTC.Text = "";
                                lbTCNGAYHD.Text = "";
                                lbGhiChuHDTC.Text = "";
                                lbSoNgayHDTC.Text = "";
                                lbTCNGAYNHAP.Text = "";
                                lbTCGHICHU.Text = "";
                                lbSoNgayTC.Text = "";
                            }

                            UpdateMode = Mode.Update;
                            upnlThiCong.Update();
                            UnblockDialog("divThiCong");
                            CloseWaitingDialog();
                        }
                        break;

                    case "showNTStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            var dondangky = _ddkpoDao.Get(madon);
                            var thietke = _tkpoDao.Get(madon);
                            var chiettinh = ctDao.Get(madon);
                            var hopdong = _hdpoDao.Get(madon);
                            var thicong = tcDao.Get(madon);
                            var bbnt = _bbntpoDao.Get(madon);

                            lbMADDKBBNT.Text = dondangky.MADDKPO;
                            lbTENKHBBNT.Text = dondangky.TENKH != null ? dondangky.TENKH : "";

                            if (bbnt != null)
                            {                           
                                string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                                string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                                lbNgayDHNT.Text = ngaydkTK + " - " + ngayduyetdonTK;
                                lbGhiChuDKNT.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayDKNT.Text = songaylamhosoDDK;

                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbNgayTKNT.Text = ngayltkCT + " - " + ngaydtkCT;
                                lbGhiChuTKNT.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTKNT.Text = songaythietkeTK;

                                lbNgayNhapCTNT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";
                                lbGhiChuCTNT.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayCTNT.Text = songaychiettinhCT;

                                lbNgayNhapHDNT.Text = hopdong.NGAYTAO != null ? hopdong.NGAYTAO.ToString() : "";
                                string songayhopdongHD = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INHOPDONG").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INHOPDONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                string ghichuhopdongHD = _spDao.Get_HopDongPo_ByMaddk(dondangky.MADDKPO).Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HopDongPo_ByMaddk(dondangky.MADDKPO).Tables[0].Rows[0]["GhiChu"].ToString() : "";
                                lbGhiChuHDNT.Text = ghichuhopdongHD;
                                lbSoNgayHDNT.Text = songayhopdongHD;

                                lbNgayNhapTCNT.Text = thicong.NGAYGTC != null ? thicong.NGAYGTC.ToString() : "";
                                lbGhiChuTCNT.Text = thicong.GHICHU != null ? thicong.GHICHU : "";
                                string songayTC = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHICONG").Tables[0].Rows.Count != 0 ?
                                   _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "DUYETTHICONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTCNT.Text = songayTC;

                                lbNgayNBBNT.Text = bbnt.NGAYLAPBB != null ? bbnt.NGAYLAPBB.ToString() : "";
                                //lbNgayNhanHS.Text = bbnt.NGAYNHANHSTC != null ? bbnt.NGAYNHANHSTC.ToString() : "";
                                //lbNgayChuyenHS.Text = "";
                                string songayNT = _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INBBNT").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(dondangky.MADDKPO, "INBBNT").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayNT.Text = songayNT;
                                txtGhiChuNT.Text = bbnt.GHICHU != null ? bbnt.GHICHU.ToString() : "";
                            }
                            else
                            {
                                lbMADDKBBNT.Text = "";
                                lbTENKHBBNT.Text = "";
                                lbNgayDHNT.Text = "";
                                lbGhiChuDKNT.Text = "";
                                lbSoNgayDKNT.Text = "";
                                lbNgayTKNT.Text = "";
                                lbGhiChuTKNT.Text = "";
                                lbSoNgayTKNT.Text = "";
                                lbNgayNhapCTNT.Text = "";
                                lbGhiChuCTNT.Text = "";
                                lbSoNgayCTNT.Text = "";
                                lbNgayNhapHDNT.Text = "";
                                lbGhiChuHDNT.Text = "";
                                lbSoNgayHDNT.Text = "";
                                lbNgayNhapTCNT.Text = "";
                                lbGhiChuTCNT.Text = "";
                                lbSoNgayTCNT.Text = "";
                                lbNgayNBBNT.Text = "";
                                lbSoNgayNT.Text = "";
                                txtGhiChuNT.Text = "";
                            }

                            UpdateMode = Mode.Update;
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

                            UpdateMode = Mode.Update;
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

            if (UpdateMode != Mode.Update)
                return;

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

            _spDao.Update_DonDangKyHopDongThietKePo_NamSinhMadpMadb(don.MADDKPO, txtNamSinhDLM.Text.Trim(),
                   txtMADPDLM.Text.Trim(), txtMADBDLM.Text.Trim(), LoginInfo.MANV);

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

                var msg = _ddkpoDao.DeleteDonLapMoiPo(txtMADDK.Text.Trim(), txtGhiChuXoaDLM.Text.Trim(), CommonFunc.GetComputerName(), 
                    CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

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

                // Get list of ids that to be update
                /*var strIds = Request["listIds"];
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
                }*/
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

                //var dondk = _lvkdDao.GetMaDon(maddk, _dontt, _donmota);
                //if (dondk != null)
                //{ lbNGAYNHAPDON.Text = dondk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                //else { lbNGAYNHAPDON.Text = ""; }

                //var tkdk = _lvkdDao.GetMaDon(maddk, _tktt, _tkmota);
                //if (tkdk != null)
                //{ lbNGAYNHAPTK.Text = tkdk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                //else { lbNGAYNHAPTK.Text = ""; }

                //var ctdk = _lvkdDao.GetMaDon(maddk, _cttt, _ctmota);
                //if (ctdk != null)
                //{ lbNGAYCHIETTINH.Text = ctdk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                //else { lbNGAYCHIETTINH.Text = ""; }

                //var hddk = _lvkdDao.GetMaDon(maddk, _hdtt, _hdmota);
                //if (hddk != null)
                //{ lbNGAYHOPDONG.Text = hddk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                //else { lbNGAYHOPDONG.Text = ""; }

                //var tcdk = _lvkdDao.GetMaDon(maddk, _tctt, _tcmota);
                //if (tcdk != null)
                //{ lbNGAYTHICONG.Text = tcdk.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
                //else { lbNGAYTHICONG.Text = ""; }

                var ntdk = _lvkdDao.GetMaDon(maddk, _nttt, _ntmota);
                var dateBBNT = _bbntpoDao.Get(maddk);
                var maddkkh = _khpoDao.GetMADDK(maddk);

                //if (dateBBNT != null)
                //{
                //    lbNGAYBBNT.Text = dateBBNT.NGAYNHAP.Value.ToString("dd/MM/yyyy");
                //}
                //else { lbNGAYBBNT.Text = ""; };

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

                        if (dhno != null)
                        {
                            lbSONODH.Text = dhno.SONO.ToString();
                        }
                        else
                        {
                            lbSONODH.Text = "";
                            ShowError("Số No đồng hồ điện sai. Kiểm tra lại. Vào phần sửa đồng hồ chưa khai thác nhập lại. ", "");
                        }                        
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

        protected void btnSaveNoiDungDDK_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khpoDao.GetMADDK(lbMaddkDK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = _ddkpoDao.Get(lbMaddkDK.Text.Trim());

                _rpClass.HisBienCo(dondk.MADDKPO, dondk.MAKVPO, LoginInfo.MANV, "DONPOHIS");

                Filtered = FilteredMode.None;

                var msg = _ddkpoDao.UpdateNoiDungDDK(lbMaddkDK.Text.Trim(), lbNoiDungDK.Text.Trim(), CommonFunc.GetComputerName(),
                    CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                upnlDangKy.Update();

                HideDialog("divDangKy");
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearContent();
                    CloseWaitingDialog();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btnSaveChuThichTK_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khpoDao.GetMADDK(lbMadkTK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = _ddkpoDao.Get(lbMadkTK.Text.Trim());

                _rpClass.HisBienCo(dondk.MADDKPO, dondk.MAKVPO, LoginInfo.MANV, "THIETKEPOHIS");

                Filtered = FilteredMode.None;

                var msg = _tkpoDao.UpdateGhiChuTK(lbMadkTK.Text.Trim(), lbNoiDungDonDangKyTK.Text.Trim(), CommonFunc.GetComputerName(),
                    CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                upnlThietKe.Update();

                HideDialog("divThietKe");
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearContent();
                    CloseWaitingDialog();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btnSaveGhiChuCT_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khpoDao.GetMADDK(lbMadkCT.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = _ddkpoDao.Get(lbMadkCT.Text.Trim());

                _rpClass.HisBienCo(dondk.MADDKPO, dondk.MAKVPO, LoginInfo.MANV, "ChietTinhHis");

                Filtered = FilteredMode.None;

                var msg = ctDao.UpdateGhiChuCT(lbMadkCT.Text.Trim(), lbCTGHICHU.Text.Trim(), CommonFunc.GetComputerName(),
                    CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                upnlChietTinh.Update();

                HideDialog("divChietTinh");
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearContent();
                    CloseWaitingDialog();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btnSaveGhiChuHD_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khpoDao.GetMADDK(lbMaddkHD.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = _ddkpoDao.Get(lbMaddkHD.Text.Trim());

                //_rpClass.HisBienCo(dondk.MADDK, dondk.MAKV, LoginInfo.MANV, "ChietTinhHis");

                Filtered = FilteredMode.None;

                var msg = _hdpoDao.UpdateGhiChu(lbMaddkHD.Text.Trim(), txtGhiChuHopDongHD.Text.Trim(), CommonFunc.GetComputerName(),
                   CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                upnlHopDong.Update();

                HideDialog("divHopDong");
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearContent();
                    CloseWaitingDialog();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btnSaveGhiChuTC_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khpoDao.GetMADDK(lbMaddkTC.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = _ddkpoDao.Get(lbMaddkTC.Text.Trim());

                _rpClass.HisBienCo(lbMaddkTC.Text.Trim(), dondk.MAKVPO, LoginInfo.MANV, "ThiCongHis");

                Filtered = FilteredMode.None;

                var msg = tcDao.UpdateGhiChuTC(lbMaddkTC.Text.Trim(), lbTCGHICHU.Text.Trim(), CommonFunc.GetComputerName(),
                   CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                upnlThiCong.Update();

                HideDialog("divThiCong");
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearContent();
                    CloseWaitingDialog();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btnSaveGhiChuNT_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoiPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khpoDao.GetMADDK(lbMADDKBBNT.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = _ddkpoDao.Get(lbMADDKBBNT.Text.Trim());

                _rpClass.HisBienCo(lbMADDKBBNT.Text.Trim(), dondk.MAKVPO, LoginInfo.MANV, "NghiemThuPoHis");

                Filtered = FilteredMode.None;

                var msg = _bbntpoDao.UpdateGhiChuNT(lbMADDKBBNT.Text.Trim(), txtGhiChuNT.Text.Trim(), CommonFunc.GetComputerName(),
                   CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                upnlNghiemThu.Update();

                HideDialog("divNghiemThu");
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearContent();
                    CloseWaitingDialog();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void ckIsXoaDLM_CheckedChanged(object sender, EventArgs e)
        {
            if (ckIsXoaDLM.Checked)
            {
                txtGhiChuXoaDLM.Enabled = true;
                btnDelete.Visible = true;
            }
            else
            {
                txtGhiChuXoaDLM.Enabled = false;
                btnDelete.Visible = false;
            }
        }

    }
}