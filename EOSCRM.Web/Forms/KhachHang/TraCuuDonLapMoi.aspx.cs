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

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class TraCuuDonLapMoi : Authentication
    {
        private readonly StoredProcedureDao _spDao = new StoredProcedureDao();
        private readonly HisNgayDangKyDao _hndkDao = new HisNgayDangKyDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly ChietTinhDao ctDao = new ChietTinhDao();
        private readonly HopDongDao hdDao = new HopDongDao();
        private readonly ThiCongDao tcDao = new ThiCongDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly MucDichSuDungDao mdsdDao = new MucDichSuDungDao();
        private readonly PhuongDao phuongDao = new PhuongDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KyDuyetDao _lvkdDao = new KyDuyetDao();
        private readonly BBNghiemThuDao _bbntDao = new BBNghiemThuDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly DongHoDao _dhDao = new DongHoDao();
        private readonly KyDuyetDao _kyduyetDao = new KyDuyetDao();
        private readonly DuyetQuyenDao _dqDao = new DuyetQuyenDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();

        #region Properties     
        protected DONDANGKY DonDangKy
        {
            get {
                try { return (DONDANGKY) Session["TCDLDM_DDK"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_DDK"] = value;  }
        }

        protected THIETKE ThietKe
        {
            get
            {
                try { return (THIETKE)Session["TCDLDM_TK"]; }
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

        protected HOPDONG HopDong
        {
            get
            {
                try { return (HOPDONG)Session["TCDLDM_HD"]; }
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

        protected BBNGHIEMTHU NghiemThu
        {
            get
            {
                try { return (BBNGHIEMTHU)Session["TCDLDM_NT"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_NT"] = value; }
        }

        private DONDANGKY DonDangKyObj
        {
            get
            {
                if (!IsDataValid())
                    return null;
                var obj = ddkDao.Get(txtMADDK.Text.Trim());


                obj.MADDK = txtMADDK.Text.Trim();
                obj.MADDKTONG = null;//ddlMADDKTONG.SelectedValue.Equals("") ? null : ddlMADDKTONG.SelectedValue,
                obj.TENKH = txtTENKH.Text.Trim().ToUpper();

                obj.TENDK = txtUYQUYEN.Text.Trim();
                obj.TENCHUCVU = txtTENCHUCVU.Text.Trim();

                obj.SONHA = txtSONHA.Text.Trim();
                obj.DIENTHOAI = txtDIENTHOAI.Text.Trim();
                obj.TEN_DC_KHAC = txtDIACHIKHAC.Text.Trim();//SOHK
                obj.DAIDIEN = false; //cbDAIDIEN.Checked,
                //obj.NOIDUNG = "";
                obj.CTCTMOI = false;
                obj.MANV = LoginInfo.MANV;

                obj.DIACHILD = txtDCLAPDAT.Text.Trim();
                // dai dien, ma duong
                //var phuong = phuongDao.Get(ddlPHUONG.SelectedValue);
                var khuvuc = kvDao.Get(ddlKHUVUC.SelectedValue);
                var phuong = phuongDao.GetMAKV(ddlPHUONG.SelectedValue,khuvuc.MAKV);

                obj.MAPHUONG = phuong != null ? phuong.MAPHUONG : null;                
                obj.MAKV = khuvuc != null ? khuvuc.MAKV : null;

                var mdsd = mdsdDao.Get(ddlMUCDICH.SelectedValue);
                obj.MAMDSD = mdsd != null ? mdsd.MAMDSD : null;

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
                    obj.MADP = txtMADP.Text.Trim();
                    obj.DUONGPHU = txtDUONGPHU.Text.Trim();

                    var duong = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                    if (duong != null)
                        tenduong = duong.TENDP + ", ";
                }
                else
                {
                    obj.MADP = null;
                    obj.DUONGPHU = null;
                    tenduong = txtDIACHIKHAC.Text.Trim().Equals(String.Empty) ? "" : txtDIACHIKHAC.Text.Trim() + ", ";
                }

                //obj.DIACHILD = string.Format("{0}{1}{2}{3}", sn, tenduong, tenphuong, tenkv);


                //so ho ngheo
                obj.DIACHINGHEO = txtDIACHIHN.Text.Trim();
                obj.ISHONGHEO = ckISHONGHEO.Checked;
                obj.DONVICAPHN = txtDONVICAP.Text.Trim();
                obj.MAHN = txtMASOHN.Text.Trim();
                if (!txtNGAPCAPHN.Text.Trim().Equals(String.Empty))
                    obj.NGAYCAPHN = DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim());
                else
                    obj.NGAYCAPHN = null;

                if (!txtNGAYKTHN.Text.Trim().Equals(String.Empty))
                    obj.NGAYKETTHUCHN = DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim());
                else
                    obj.NGAYKETTHUCHN = null;

                if (!txtNGAYKYSOHN.Text.Trim().Equals(String.Empty))
                    obj.NGAYKYHN = DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim());
                else
                    obj.NGAYKYHN = null;     


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

                if (!txtNGAYCAPCMND.Text.Trim().Equals(String.Empty))
                    obj.CAPNGAY = DateTimeUtil.GetVietNamDate(txtNGAYCAPCMND.Text.Trim());
                else
                    obj.CAPNGAY = null;

                obj.SONHA2 = txtSONHA2.Text.Trim();
                obj.MADPKHGAN = !string.IsNullOrEmpty(txtDPKHKEBEN.Text.Trim()) ? txtDPKHKEBEN.Text.Trim() : "";

                obj.NOIDUNG = txtNOIDUNGKEMTHEO.Text.Trim();

                string namsinh = "11/" + "11/" + txtNamSinhDLM.Text.Trim();
                obj.NGAYSINH = namsinh;

                return obj;
            }
        }       
        #endregion

        #region co loc, up
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

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((EOS)Page.Master).SetReadonly(id, isReadonly);
        }

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
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
            Page.Title = Resources.Message.TITLE_KH_TRACUUTRANGTHAI;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TRACUUTRANGTHAI;
            }
            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDPKHKEBEN);
        }       

        private void BindDataForGrid()
        {
            try
            {                
                int? sohodn = null;
                int? sonk = null;
                int? dmnk = null;
                    
                // ReSharper disable EmptyGeneralCatchClause
                try { sohodn = Convert.ToInt32(txtSOHODN.Text.Trim()); } catch { }
                try { sonk = Convert.ToInt32(txtSONK.Text.Trim()); } catch { }
                try { dmnk = Convert.ToInt32(txtDMNK.Text.Trim()); } catch { }
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
                    || query.MAPB == "LA" || query.MAPB == "NC" || query.MAPB == "VH") // TAN CHAU
                {
                    if (ckIsXoaDLM.Checked)
                    {
                        var objList = ddkDao.GetListByXoaDLM(ckIsXoaDLM.Checked);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();

                    }
                    else
                    {
                        //giao ho so roi
                        var objList = ddkDao.GetListForTcdldmPB(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                        txtDIACHIKHAC.Text.Trim(),
                                        sohodn, sonk, dmnk, ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue, query.MAPB.ToString(),
                                        txtCMND.Text.Trim());

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                }
                //else if (ddlPHONGBAN2.SelectedValue == "%") // tim tat ca nha may, to
                //{
                //    var objList = ddkDao.GetListForTcdldmCM(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                //                    txtDIACHIKHAC.Text.Trim(),
                //                    sohodn, sonk, dmnk,
                //                    ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlToNhaMay.SelectedValue,//ddlPHUONG.SelectedValue, 
                //                    txtCMND.Text.Trim());

                //    gvList.DataSource = objList;
                //    gvList.PagerInforText = objList.Count.ToString();
                //    gvList.DataBind();
                //}
                else
                {
                    if (ckIsXoaDLM.Checked)
                    {
                        var objList = ddkDao.GetListByXoaDLM(ckIsXoaDLM.Checked);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();

                    }
                    else
                    {
                        var objList = ddkDao.GetListForTcdldmCM(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                            txtDIACHIKHAC.Text.Trim(),
                                            sohodn, sonk, dmnk,
                                            ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlToNhaMay.SelectedValue,//ddlPHUONG.SelectedValue, 
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

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();                    
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }                   
                }
            }
            //xa phuong
            var listXAPHUONG = _xpDao.GetListKV(_nvDao.GetKV(b).MAKV);
            ddlTENXA.Items.Clear();
            ddlTENXA.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var xa in listXAPHUONG)
            {
                ddlTENXA.Items.Add(new ListItem(xa.TENXA, xa.MAXA));
            }
            //ddlTENXA.DataSource = listXAPHUONG;
            //ddlTENXA.DataTextField = "TENXA";
            //ddlTENXA.DataValueField = "MAXA";
            //ddlTENXA.DataBind();

            txtDONVICAP.Text = ddlTENXA.SelectedValue;
        }

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;

                var khuvuclist = kvDao.GetList();

                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in khuvuclist)
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));                
               
                var mdsdList = mdsdDao.GetList();
                ddlMUCDICH.Items.Clear();
                ddlMUCDICH.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var mdsd in mdsdList)
                    ddlMUCDICH.Items.Add(new ListItem(mdsd.TENMDSD, mdsd.MAMDSD));

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
            //var items = pDao.GetList(ddlKHUVUC.SelectedValue);
            var items = pDao.GetListKV(ddlKHUVUC.SelectedValue);

            ddlPHUONG.Items.Clear();
            ddlPHUONG.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var p in items)
                ddlPHUONG.Items.Add(new ListItem(p.TENPHUONG, p.MAPHUONG));
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
                var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

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
            txtTENCHUCVU.Text = "";

            txtNOILAPDHN.Text = "";
            cbISTUYENONGCHUNG.Checked = false;
            //lbNGAYNHAPDON.Text = "";
            //lbNGAYNHAPTK.Text = "";
            //lbNGAYCHIETTINH.Text = "";
            //lbNGAYHOPDONG.Text = "";
            //lbNGAYTHICONG.Text = "";
            //lbNGAYBBNT.Text = "";
            //lbNVKYNGHIEMTHU.Text = "";
            lbNGAYKHAITHAC.Text = "";
            txtSOHD1.Text = "";
            lbSONODH.Text = "";

            //so ho ngheo
            ddlTENXA.SelectedIndex = 0;
            txtDONVICAP.Text = ddlTENXA.SelectedValue;
            txtMASOHN.Text = "";
            txtNGAPCAPHN.Text = "";
            txtNGAYKTHN.Text = "";
            txtNGAYKYSOHN.Text = "";
            txtDIACHIHN.Text = "";

            txtDIACHIHN.Enabled = false;
            txtDONVICAP.Enabled = false;
            txtMASOHN.Enabled = false;
            txtNGAPCAPHN.Enabled = false;
            txtNGAYKTHN.Enabled = false;
            txtNGAYKYSOHN.Enabled = false;
            ImageButton1.Visible = false;
            ImageButton2.Visible = false;
            ImageButton3.Visible = false;

            ddlPHONGBAN2.SelectedIndex = 0;

            txtMADPDLM.Text = "";
            txtMADBDLM.Text = "";
            ddlToNhaMay.SelectedIndex = 0;

            txtSONODHN.Text = "";

            ckIsXoaDLM.Checked = false;
            txtGhiChuXoaDLM.Text = "";
            txtGhiChuXoaDLM.Enabled = false;
            btnDelete.Visible = false;

            txtNamSinhDLM.Text = "";            
        }

        private void SetDDKToForm(DONDANGKY ddk)
        {
            txtMADDK.Text = ddk.MADDK;

            SetReadonly(txtMADDK.ClientID, true);

            txtTENKH.Text = ddk.TENKH;

            txtSONHA.Text = ddk.SONHA;
            txtDIENTHOAI.Text = ddk.DIENTHOAI;
            txtDIACHIKHAC.Text = ddk.TEN_DC_KHAC;
            txtDCLAPDAT.Text = ddk.DIACHILD;

            if (ddk.DUONGPHO != null)
            {
                txtMADP.Text = ddk.MADP;
                txtDUONGPHU.Text = ddk.DUONGPHU;
            }
            
            txtSOHODN.Text = ddk.SOHODN.HasValue ? String.Format("{0:0,0}", ddk.SOHODN.Value) : "";
            txtSONK.Text = ddk.SONK.HasValue ? String.Format("{0:0,0}", ddk.SONK.Value) : "";
            txtDMNK.Text = ddk.DMNK.HasValue ? String.Format("{0:0,0}", ddk.DMNK.Value) : "";
            txtNGAYCD.Text = ddk.NGAYDK.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYDK.Value) : "";
            txtNGAYKS.Text = ddk.NGAYHKS.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYHKS.Value) : "";

            txtNGAYCAPCMND.Text = ddk.CAPNGAY.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.CAPNGAY.Value) : "";

            var kv = ddlKHUVUC.Items.FindByValue(ddk.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            LoadDynamicReferences();

            var p = ddlPHUONG.Items.FindByValue(ddk.MAPHUONG);
            if (p != null)
                ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(p);

            var mdsd = ddlMUCDICH.Items.FindByValue(ddk.MAMDSD);
            if (mdsd != null)
                ddlMUCDICH.SelectedIndex = ddlMUCDICH.Items.IndexOf(mdsd);

            txtCMND.Text = ddk.CMND;
            txtMST.Text = ddk.MST != null ? ddk.MST : "";

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

            //so ho ngheo
            var isCheckedHN = ddk.ISHONGHEO.HasValue && ddk.ISHONGHEO.Value;
            ckISHONGHEO.Checked = isCheckedHN;
            if (isCheckedHN)
            {
                if (ddk.DONVICAPHN != null)
                {
                    var pn = ddlTENXA.Items.FindByValue(ddk.DONVICAPHN);
                    if (pn != null)
                        ddlTENXA.SelectedIndex = ddlTENXA.Items.IndexOf(pn);
                }
                else 
                { 
                    ddlTENXA.SelectedIndex = 0; 
                }

                txtDIACHIHN.Text = ddk.DIACHINGHEO != null ? ddk.DIACHINGHEO : "";
                txtDONVICAP.Text = ddk.DONVICAPHN != null ? ddk.DONVICAPHN : "";
                txtMASOHN.Text = ddk.MAHN != null ? ddk.MAHN : "";
                txtNGAPCAPHN.Text = ddk.NGAYCAPHN != null ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYCAPHN.Value) : "";
                txtNGAYKTHN.Text = ddk.NGAYKETTHUCHN != null ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYKETTHUCHN.Value) : "";
                txtNGAYKYSOHN.Text = ddk.NGAYKYHN != null ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYKYHN.Value) : "";

                ddlTENXA.Enabled = true;
                txtDONVICAP.Enabled = true;
                txtMASOHN.Enabled = true;
                txtNGAPCAPHN.Enabled = true;
                txtNGAYKTHN.Enabled = true;
                txtNGAYKYSOHN.Enabled = true;
                txtDIACHIHN.Enabled = true;
                ImageButton1.Visible = true;
                ImageButton2.Visible = true;
                ImageButton3.Visible = true;      
            }
            else
            {
                ddlTENXA.SelectedIndex = 0;
                txtDONVICAP.Text = "";
                txtMASOHN.Text = "";
                txtNGAPCAPHN.Text = "";
                txtNGAYKTHN.Text = "";
                txtNGAYKYSOHN.Text = "";
                txtDIACHIHN.Text = "";

                ddlTENXA.Enabled = false;
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                txtDIACHIHN.Enabled = false;
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;
            }

            //Xoa don lap moi
            string isXoaDLM = _spDao.Get_DonDangKy_ByMaddk(ddk.MADDK).Tables[0].Rows.Count != 0 ?
                (_spDao.Get_DonDangKy_ByMaddk(ddk.MADDK).Tables[0].Rows[0]["IsXoaDLM"].ToString() == "" ||
                 _spDao.Get_DonDangKy_ByMaddk(ddk.MADDK).Tables[0].Rows[0]["IsXoaDLM"].ToString() == "False" ? "False" : "True") : "False";            

            var isCheckedXoaDLM = Convert.ToBoolean(isXoaDLM);
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

            var isCheckedONGCAI = ddk.ISTUYENONGCHUNG.HasValue && ddk.ISTUYENONGCHUNG.Value;
            cbISTUYENONGCHUNG.Checked = isCheckedONGCAI;

            if (ddk.TENDK != null)
            { txtUYQUYEN.Text = ddk.TENDK; }

            if (ddk.TENCHUCVU != null)
            { txtTENCHUCVU.Text = ddk.TENCHUCVU; }

            if (ddk.NOILAPDHHN != null)
            {
                txtNOILAPDHN.Text = ddk.NOILAPDHHN;
            }
            else
            {
                txtNOILAPDHN.Text = "";
            }

            var sohd = hdDao.Get(ddk.MADDK);
            if (sohd != null)
            {
                txtSOHD1.Text = sohd.SOHD.ToString();

                txtMADPDLM.Text = sohd.MADP != null ? sohd.MADP : "";
                txtMADBDLM.Text = sohd.MADB != null ? sohd.MADB : "";
            }
            else 
            {
                txtSOHD1.Text = "";

                txtMADPDLM.Text = "";
                txtMADBDLM.Text = "";
            }      

            var dq = _dqDao.Get(ddk.MADDK);
            if (dq != null)
            {
                var pb2 = ddlPHONGBAN2.Items.FindByValue(dq.MAPB);
                if (pb2 != null)
                    ddlPHONGBAN2.SelectedIndex = ddlPHONGBAN2.Items.IndexOf(pb2);
            }

            LoadDateDon(ddk.MADDK);

            txtSONHA2.Text = ddk.SONHA2 != null ? ddk.SONHA2 : "";
            txtDPKHKEBEN.Text = ddk.MADPKHGAN != null ? ddk.MADPKHGAN : "";

            txtNOIDUNGKEMTHEO.Text = ddk.NOIDUNG != null ? ddk.NOIDUNG : "";

            var chiettinh = ctDao.Get(ddk.MADDK);
            if (chiettinh != null)
            {
                if(chiettinh.TONGTIENCTPS != null)
                    lbTongTienTK.Text =  String.Format("{0:0,0}", Convert.ToInt64(chiettinh.TONGTIENCTPS));
                else
                    lbTongTienTK.Text = "0";
            }
            else
            {
                lbTongTienTK.Text = "0";
            }

            var tonhamay = ddlToNhaMay.Items.FindByValue(ddk.MADDK.Substring(1, 2));
            if (tonhamay != null)
                ddlToNhaMay.SelectedIndex = ddlToNhaMay.Items.IndexOf(tonhamay);

            txtNamSinhDLM.Text = ddk.NGAYSINH.Substring(ddk.NGAYSINH.Length - 4, 4);

            upnlInfor.Update();
        }
        
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();
                
                var dondangky = ddkDao.Get(madon);
                var thietke = tkDao.Get(madon);
                var chiettinh = ctDao.Get(madon);
                var hopdong = hdDao.Get(madon);
                var thicong = tcDao.Get(madon);
                var bbnghiemthu = _bbntDao.Get(madon);

                switch (e.CommandName)
                {
                    case "showDKStatus":
                        if (!string.Empty.Equals(madon))
                        {                            
                            //var ddk = ddkDao.Get(madon);
                            if (dondangky != null)
                            {                                
                                lbMADDK.Text = dondangky.MADDK;
                                lbDDKTENKH.Text = dondangky.TENKH;
                                lbDDKDCLAP.Text = dondangky.DIACHILD != null ? dondangky.DIACHILD : "";
                                lbDDKDIENTHOAI.Text = dondangky.DIENTHOAI != null ? dondangky.DIENTHOAI : "";
                                lbDDKMDSD.Text = dondangky.MAMDSD != null ? mdsdDao.Get(dondangky.MAMDSD).TENMDSD : "";
                                //lbDDKNGAYDK.Text = dondangky.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", dondangky.NGAYN) : "";
                                lbDDKNGAYDK.Text = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";

                                lbNgayChuyenHSToTK.Text = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";

                                //lbSoNgayLamHoSo.Text = dondangky.NGAYDK != null && dondangky.NGAYHKS != null ? 
                                //    Math.Ceiling(((DateTime)dondangky.NGAYHKS - (DateTime)dondangky.NGAYDK).TotalDays).ToString() : "0";                               

                                //lbSoNgayLamHoSo.Text = dondangky.NGAYDK != null && dondangky.NGAYHKS != null ?
                                //    Convert.ToString(DateTimeUtil.CountDayDiffT7CN(dondangky.NGAYDK ?? DateTime.Now,
                                //    dondangky.NGAYHKS ?? DateTime.Now) + 1) : "0";

                                //var data = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString();
                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayLamHoSo.Text = songaylamhosoDDK;

                                lbDDKNOIDUNG.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                                lbDDKNVNHAP.Text = _nvDao.Get(dondangky.MANV).HOTEN;

                                UpdateMode = Mode.Update;
                                upnlDangKy.Update();
                                UnblockDialog("divDangKy");
                                CloseWaitingDialog();
                            }                            
                        }
                        break;

                    case "showTKStatus":
                        if (!string.Empty.Equals(madon))
                        {                           
                            //var tk = tkDao.Get(madon);
                            //var tkdon =  ddkDao.Get(madon);
                            if (thietke != null)
                            {
                                lbTKMADDK.Text = thietke.MADDK;
                                lbTKTENKH.Text = dondangky.TENKH;
                                lbTKDCLAPD.Text = dondangky.DIACHILD != null ? dondangky.DIACHILD : "";
                                lbTKSODT.Text = dondangky.DIENTHOAI != null ? dondangky.DIENTHOAI : "";
                                lbTKTENTK.Text = thietke.TENTK != null ? thietke.TENTK : "";
                                //lbTKNGAYDK.Text = dondangky.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", dondangky.NGAYN) : "";
                                string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                                string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                                lbTKNGAYDK.Text = ngaydkTK + " - " + ngayduyetdonTK;

                                lbGhiChuDDKTK.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                                //lbTKNGAYTK.Text = String.Format("{0:dd/MM/yyyy}", tk.NGAYLTK);
                                //lbTKSoNgayLamHS.Text = dondangky.NGAYDK != null && dondangky.NGAYHKS != null ?
                                //  Math.Ceiling(((DateTime)dondangky.NGAYHKS - (DateTime)dondangky.NGAYDK).TotalDays).ToString() : "0";
                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbTKSoNgayLamHS.Text = songaylamhosoDDK;                                

                                lbNoiDungDonDangKyTK.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";

                                lbTKNGAYTK.Text = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";

                                lbTKNVPHUTRACH.Text = thietke.TENNVTK != null ? thietke.TENNVTK : "";
                                lbTKNVDUYET.Text = thietke.MANVDTK != null ? _nvDao.Get(thietke.MANVDTK).HOTEN : "";

                                lbTKNGAYDUYET.Text = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                //lbTKSoNgayThietKe.Text = thietke.NGAYDTK != null && thietke.NGAYLTK != null ?
                                //   Math.Ceiling(((DateTime)thietke.NGAYDTK - (DateTime)thietke.NGAYLTK).TotalDays).ToString() : "0";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbTKSoNgayThietKe.Text = songaythietkeTK;

                                lbTKNVNHAP.Text = thietke.MANVLTK != null ? _nvDao.Get(thietke.MANVLTK).HOTEN : "";
                                lbGhiChuTK.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";                                
                            }
                            else
                            {
                                lbTKMADDK.Text = dondangky.MADDK;
                                lbTKTENKH.Text = dondangky.TENKH;
                                lbTKDCLAPD.Text = dondangky.DIACHILD != null ? dondangky.DIACHILD : "";
                                lbTKSODT.Text = dondangky.DIENTHOAI != null ? dondangky.DIENTHOAI : "";
                                lbTKTENTK.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                                lbTKNGAYDK.Text = "";
                                lbGhiChuDDKTK.Text = "";
                                lbTKSoNgayLamHS.Text = "0";
                                lbNoiDungDonDangKyTK.Text = "";
                                lbTKNGAYTK.Text = "";
                                lbTKNVPHUTRACH.Text = "";
                                lbTKNVDUYET.Text = "";
                                lbTKNGAYDUYET.Text = "";
                                lbTKSoNgayThietKe.Text = "0";
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
                            if (chiettinh != null)
                            {
                                lbCTMADDK.Text = dondangky.MADDK;
                                lbCTTENKH.Text = dondangky.TENKH;

                                string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                                string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                                lbCTNGAYNHAPDON.Text = ngaydkTK + " - " + ngayduyetdonTK;
                                lbGhiChuDDKCT.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";

                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbCTSoNgayLamHS.Text = songaylamhosoDDK;

                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbCTNGAYNHAPTK.Text = ngayltkCT + " - " + ngaydtkCT;

                                lbGhiChuTKCT.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbCTSoNgayTK.Text = songaythietkeTK;

                                lbCTNGAYLCT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";

                                //string songaychiettinhCT2 = thietke.NGAYDTK != null && chiettinh.NGAYN != null ?
                                //    Convert.ToString(DateTimeUtil.CountDayDiffT7CN(thietke.NGAYDTK ?? DateTime.Now,
                                //    chiettinh.NGAYN ?? DateTime.Now)) : "0";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbCTSoNgayCT.Text = songaychiettinhCT;

                                //lbCTNGAYN.Text = chiettinh.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", chiettinh.NGAYN) : "";
                                //lbCTMANVLCT.Text = chiettinh.MANVLCT != null ? _nvDao.Get(chiettinh.MANVLCT).HOTEN : "";
                                lbCTGHICHU.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                            }
                            else
                            {
                                lbCTMADDK.Text = dondangky.MADDK;
                                lbCTTENKH.Text = dondangky.TENKH;
                                lbCTNGAYNHAPDON.Text = "";
                                lbGhiChuDDKCT.Text = "";
                                lbCTSoNgayLamHS.Text = "0";
                                lbCTNGAYNHAPTK.Text = "";
                                lbGhiChuTKCT.Text = "";
                                lbCTSoNgayTK.Text = "0";
                                lbCTNGAYLCT.Text = "";
                                //lbCTNGAYN.Text = "";
                                //lbCTMANVLCT.Text = "";
                                lbCTSoNgayCT.Text = "0";
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
                            if (hopdong != null)
                            {
                                lbHDMADDK.Text = dondangky.MADDK;   
                                lbHDTENKH.Text = dondangky.TENKH;
                                //lbHDNGAYNHAPDON.Text = dondangky.NGAYN != null ? String.Format("{0:dd/MM/yyyy}", dondangky.NGAYN) : ""; 
                                string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                                string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                                lbHDNGAYNHAPDON.Text = ngaydkTK + " - " + ngayduyetdonTK;
                                lbGhiChuDDKHD.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";

                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayDDKHD.Text = songaylamhosoDDK;

                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbHDNGAYTK.Text = ngayltkCT + " - " + ngaydtkCT;
                                lbGhiChuTKHD.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTKHD.Text = songaythietkeTK;
                                
                                lbNGAYCT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";
                                lbGhiChuCTHD.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayCTHD.Text = songaychiettinhCT;

                                lbHDNGAYNHAPHD.Text = hopdong.NGAYTAO != null ? hopdong.NGAYTAO.ToString() : "";
                                string songayhopdongHD = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INHOPDONG").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INHOPDONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayHopDongHD.Text = songayhopdongHD;
                                string ghichuhopdongHD = _spDao.Get_HopDong_ByMaddk(dondangky.MADDK).Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HopDong_ByMaddk(dondangky.MADDK).Tables[0].Rows[0]["GhiChu"].ToString() : "";
                                txtGhiChuHopDongHD.Text = ghichuhopdongHD;
                            }
                            else 
                            {
                                lbHDMADDK.Text = dondangky.MADDK;
                                lbHDTENKH.Text = dondangky.TENKH;
                                lbHDNGAYNHAPDON.Text = "";
                                lbGhiChuDDKHD.Text = "";
                                lbSoNgayDDKHD.Text = "0";
                                lbHDNGAYTK.Text = "";
                                lbGhiChuTKHD.Text = "";
                                lbSoNgayTKHD.Text = "0";
                                lbNGAYCT.Text = "";
                                lbGhiChuCTHD.Text = "";
                                lbSoNgayCTHD.Text = "0";
                                lbHDNGAYNHAPHD.Text = "";
                                lbSoNgayHopDongHD.Text = "0";
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
                            if (thicong != null)
                            {
                                lbTCMADDK.Text = dondangky.MADDK;   
                                lbTCTENKH.Text = dondangky.TENKH;                                   

                                string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                                string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                                lbTCNGAYNHAPDON.Text = ngaydkTK + " - " + ngayduyetdonTK;
                                lbGhiChuDKTC.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayDKTC.Text = songaylamhosoDDK;

                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbTCNGAYTK.Text = ngayltkCT + " - " + ngaydtkCT;
                                lbGhiChuTKTC.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTKTC.Text = songaythietkeTK;
                                
                                lbTCNGAYCT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";
                                lbGhiChuCTTC.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayCTTC.Text = songaychiettinhCT;
                               
                                lbTCNGAYHD.Text = hopdong.NGAYTAO != null ? hopdong.NGAYTAO.ToString() : "";
                                string songayhopdongHD = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INHOPDONG").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INHOPDONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                string ghichuhopdongHD = _spDao.Get_HopDong_ByMaddk(dondangky.MADDK).Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HopDong_ByMaddk(dondangky.MADDK).Tables[0].Rows[0]["GhiChu"].ToString() : "";
                                lbGhiChuHDTC.Text = ghichuhopdongHD;
                                lbSoNgayHDTC.Text = songayhopdongHD;

                                lbTCNGAYNHAP.Text = thicong.NGAYGTC != null ? thicong.NGAYGTC.ToString() : "";
                                lbTCGHICHU.Text = thicong.GHICHU != null ? thicong.GHICHU : "";
                                string songayTC = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHICONG").Tables[0].Rows.Count != 0 ?
                                   _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHICONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTC.Text = songayTC;
                            }
                            else 
                            {
                                lbTCMADDK.Text = dondangky.MADDK;
                                lbTCTENKH.Text = dondangky.TENKH;
                                lbTCNGAYNHAPDON.Text = "";
                                lbGhiChuDKTC.Text = "";
                                lbSoNgayDKTC.Text = "0";
                                lbTCNGAYTK.Text = "";
                                lbGhiChuTKTC.Text = "";
                                lbSoNgayTKTC.Text = "0";
                                lbTCNGAYCT.Text = "";
                                lbGhiChuCTTC.Text = "";
                                lbSoNgayCTTC.Text = "0";
                                lbTCNGAYHD.Text = "";
                                lbGhiChuHDTC.Text = "";
                                lbSoNgayHDTC.Text = "0";
                                lbTCNGAYNHAP.Text = "";
                                lbTCGHICHU.Text = "";
                                lbSoNgayTC.Text = "0";
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
                            var bbnt = _bbntDao.Get(madon);
                            var ntdon = ddkDao.Get(madon);
                            if (bbnt != null)
                            {
                                //BindHisNgayBBNT(bbnt.MADDK);
                                //upnlNghiemThu.Update();
                                //UnblockDialog("divNghiemThu");        
                                lbMADDKBBNT.Text = bbnt.MADDK;
                                lbTENKHBBNT.Text = dondangky.TENKH != null ? dondangky.TENKH : "";

                                string ngaydkTK = dondangky.NGAYDK != null ? dondangky.NGAYDK.ToString() : "";
                                string ngayduyetdonTK = dondangky.NGAYHKS != null ? dondangky.NGAYHKS.ToString() : "";
                                lbNgayDHNT.Text = ngaydkTK + " - " + ngayduyetdonTK;
                                lbGhiChuDKNT.Text = dondangky.NOIDUNG != null ? dondangky.NOIDUNG : "";
                                string songaylamhosoDDK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETDONDANGKY").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayDKNT.Text = songaylamhosoDDK;

                                string ngayltkCT = thietke.NGAYLTK != null ? thietke.NGAYLTK.ToString() : "";
                                string ngaydtkCT = thietke.NGAYDTK != null ? thietke.NGAYDTK.ToString() : "";
                                lbNgayTKNT.Text = ngayltkCT + " - " + ngaydtkCT;
                                lbGhiChuTKNT.Text = thietke.CHUTHICH != null ? thietke.CHUTHICH : "";
                                string songaythietkeTK = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHIETKE").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTKNT.Text = songaythietkeTK;

                                lbNgayNhapCTNT.Text = chiettinh.NGAYN != null ? chiettinh.NGAYN.ToString() : "";
                                lbGhiChuCTNT.Text = chiettinh.GHICHU != null ? chiettinh.GHICHU : "";
                                string songaychiettinhCT = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETCTKH").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayCTNT.Text = songaychiettinhCT;

                                lbNgayNhapHDNT.Text = hopdong.NGAYTAO != null ? hopdong.NGAYTAO.ToString() : "";
                                string songayhopdongHD = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INHOPDONG").Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INHOPDONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                string ghichuhopdongHD = _spDao.Get_HopDong_ByMaddk(dondangky.MADDK).Tables[0].Rows.Count != 0 ?
                                    _spDao.Get_HopDong_ByMaddk(dondangky.MADDK).Tables[0].Rows[0]["GhiChu"].ToString() : "";
                                lbGhiChuHDNT.Text = ghichuhopdongHD;
                                lbSoNgayHDNT.Text = songayhopdongHD;

                                lbNgayNhapTCNT.Text = thicong.NGAYGTC != null ? thicong.NGAYGTC.ToString() : "";
                                lbGhiChuTCNT.Text = thicong.GHICHU != null ? thicong.GHICHU : "";
                                string songayTC = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHICONG").Tables[0].Rows.Count != 0 ?
                                   _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "DUYETTHICONG").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayTCNT.Text = songayTC;

                                lbNgayNBBNT.Text = bbnt.NGAYLAPBB != null ? bbnt.NGAYLAPBB.ToString() : "";
                                lbNgayNhanHS.Text = bbnt.NGAYNHANHSTC != null ? bbnt.NGAYNHANHSTC.ToString() : "";
                                //lbNgayChuyenHS.Text = "";
                                string songayNT = _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INBBNT").Tables[0].Rows.Count != 0 ?
                                  _spDao.Get_HisNgayDangKy_ByMaDDKMoTaTTDON(dondangky.MADDK, "INBBNT").Tables[0].Rows[0]["SoNgay"].ToString() : "0";
                                lbSoNgayNT.Text = songayNT;
                                txtGhiChuNT.Text = bbnt.GHICHU != null ? bbnt.GHICHU.ToString() : "";
                            }
                            else
                            {
                                lbMADDKBBNT.Text = ntdon.MADDK;
                                lbTENKHBBNT.Text = ntdon.TENKH;
                                lbNgayDHNT.Text = ""; 
                                //lbNgayDuyetNT.Text = "";
                                lbNgayTKNT.Text = "";
                                //lbNgayDuyetTKNT.Text = "";
                                //lbNgayDuyetKHNT.Text = "";
                                lbNgayNhapHDNT.Text = "";
                                lbNgayNhapTCNT.Text = "";
                                //lbNgayDuyetTCNT.Text = "";
                                lbNgayNBBNT.Text = "";
                                lbNgayNhanHS.Text = "";
                                //lbNgayChuyenHS.Text = "";                                
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
                            var don = ddkDao.Get(madon);
                            if (don == null) return;
                            SetDDKToForm(don);
                        }

                        UpdateMode = Mode.Update;
                        CloseWaitingDialog();
                        break;
                        
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindHisNgayBBNT(string maddk)
        {
            try
            {
                var nt = _bbntDao.Get(maddk);
                var tc = tcDao.Get(maddk);
                var hd = hdDao.Get(maddk);
                var ct = ctDao.Get(maddk);
                var tk = tkDao.Get(maddk);
                var ntdon = ddkDao.Get(maddk);

                lbMADDKBBNT.Text = nt.MADDK;
                lbTENKHBBNT.Text = ntdon.TENKH != null ? ntdon.TENKH : "";

                var hisngayindk = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "INDONDANGKY");
                if (hisngayindk != null)
                {
                    lbNgayDHNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayindk.NGAYNHAP);
                }
                else
                {
                    lbNgayDHNT.Text = String.Format("{0:dd/MM/yyyy}", ntdon.NGAYN);
                }

                //var hisngayduyetdk = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "DUYETDONDANGKY");
                //if (hisngayduyetdk != null)
                //{
                //    lbNgayDuyetNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayduyetdk.NGAYDUYET);
                //}
                //else
                //{
                //    lbNgayDuyetNT.Text = String.Format("{0:dd/MM/yyyy}", ntdon.NGAYDUYETHS);
                //}

                var hisngayintk = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "INTHIETKE");
                if (hisngayintk != null)
                {
                    lbNgayTKNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayintk.NGAYNHAP);
                }
                else
                {
                    lbNgayTKNT.Text = String.Format("{0:dd/MM/yyyy}", ntdon.NGAYN);
                }

                //var hisngayduyettk = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "DUYETTHIETKE");
                //if (hisngayduyettk != null)
                //{
                //    lbNgayDuyetTKNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayduyettk.NGAYDUYET);
                //}
                //else
                //{
                //    lbNgayDuyetTKNT.Text = String.Format("{0:dd/MM/yyyy}", tk.NGAYDTK);
                //}

                //var hisngayduyetct = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "DUYETCTKH");
                //if (hisngayduyetct != null)
                //{
                //    lbNgayDuyetKHNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayduyetct.NGAYDUYET);
                //}
                //else
                //{
                //    lbNgayDuyetKHNT.Text = String.Format("{0:dd/MM/yyyy}", ct.NGAYN);
                //}

                var hisngayinhd = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "INHOPDONG");
                if (hisngayinhd != null)
                {
                    lbNgayNhapHDNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayinhd.NGAYNHAP);
                }
                else
                {
                    lbNgayNhapHDNT.Text = String.Format("{0:dd/MM/yyyy}", hd.NGAYTAO);
                }

                var hisngayintc = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "INTHICONG");
                if (hisngayintc != null)
                {
                    lbNgayNhapTCNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayintc.NGAYNHAP);
                }
                else
                {
                    lbNgayNhapTCNT.Text = String.Format("{0:dd/MM/yyyy}", tc.NGAYN);
                }

                //var hisngayduyettc = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "DUYETTHICONG");
                //if (hisngayduyettc != null)
                //{
                //    lbNgayDuyetTCNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayduyettc.NGAYDUYET);
                //}
                //else
                //{
                //    lbNgayDuyetTCNT.Text = String.Format("{0:dd/MM/yyyy}", tc.NGAYN);
                //}

                var hisngayinbbnt = _hndkDao.GetMaxMADDKMoTa(nt.MADDK, "INBBNT");
                if (hisngayinbbnt != null)
                {
                    lbNgayNBBNT.Text = String.Format("{0:dd/MM/yyyy}", hisngayinbbnt.NGAYNHAP);
                }
                else
                {
                    lbNgayNBBNT.Text = String.Format("{0:dd/MM/yyyy}", nt.NGAYNHAP);
                }

                if (nt.NGAYNHANHSTC != null)
                {
                    lbNgayNhanHS.Text = String.Format("{0:dd/MM/yyyy}", nt.NGAYNHANHSTC);
                }
                else
                {
                    lbNgayNhanHS.Text = "";
                }

                //if (nt.NGAYCHUYENHSKTOAN != null)
                //{
                //    lbNgayChuyenHS.Text = String.Format("{0:dd/MM/yyyy}", nt.NGAYCHUYENHSKTOAN);
                //}
                //else
                //{
                //    lbNgayChuyenHS.Text = "";
                //}

            }
            catch { }
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

            var source = gvList.DataSource as List<DONDANGKY>;
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

                if(ttdk != null)
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

        private void LoadToNhaMay(string makv)
        {
            try
            {
                var phongban = _pbDao.GetListKV(makv);
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
            try
            {
                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var don = DonDangKyObj;
                if (don == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                if (don.IsXoaDLM == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đơn lắp mới đã Xóa. Đề nghị nhập đơn mới!");
                    return;
                }

                var maddkkh = _khDao.GetMADDK(don.MADDK);
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                _rpClass.HisBienCo(don.MADDK, don.MAKV, LoginInfo.MANV, "DONHIS");

                Filtered = FilteredMode.None;
                var msg = ddkDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _spDao.Update_DonDangKyHopDongThietKe_NamSinhMadpMadb(don.MADDK, txtNamSinhDLM.Text.Trim(),
                    txtMADPDLM.Text.Trim(), txtMADBDLM.Text.Trim(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBien(don.MADDK, LoginInfo.MANV, don.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                    "", "", "", "", "UPDONDK");

                //update duyet_quyen
                if (don.TTTK != null)
                {
                    if ((_nvDao.Get(LoginInfo.MANV).MAPB.Equals("KD") || LoginInfo.MANV.Equals("nguyen")) && don.TTDK.Equals("DK_A")
                        && (don.TTTK.Equals("TK_N") || don.TTTK.Equals("TK_P") ))
                    {
                        var msqDQ = _dqDao.Update(don.MADDK, LoginInfo.MANV, ddlPHONGBAN2.SelectedValue, don.MAKV);
                    }
                }

                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    //ShowInfor(ResourceLabel.Get(msgDQ));
                    //Trả lại màn hình trống ban đầu
                    ClearContent();
                    CloseWaitingDialog();
                    // Refresh grid view
                    BindDataForGrid();
                    upnlGrid.Update();

                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                //_rpClass.HisBienCo(txtMADDK.Text.Trim(), "", LoginInfo.MANV, "DONHIS");

                var msg = ddkDao.DeleteDonLapMoi(txtMADDK.Text.Trim(), txtGhiChuXoaDLM.Text.Trim(), CommonFunc.GetComputerName(), 
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
                    var objs = new List<DONDANGKY>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (!ddkDao.IsInUse(ma)) continue;

                        var ddk = ddkDao.Get(ma);
                        var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đơn đăng ký", ddk.TENKH);

                        CloseWaitingDialog();
                        ShowError(ResourceLabel.Get(msgIsInUse));
                        return;
                    }

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    //TODO: check relation before update list
                    var msg = ddkDao.DoAction(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

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

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetControlValue(txtDIACHIKHAC.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
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

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

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
                        var dp = dpDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);
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
                String //_dontt = "DK_A", _tktt = "TK_P", _cttt = "CT_N", _hdtt = "HD_N", _tctt = "TC_N",
                    _nttt = "NT_A";//Thêm mới thi công.
                String //_donmota = "Nhập đơn lắp mới", _tkmota = "Nhập thiết kế.", _ctmota = "Chạy chiết tính", _hdmota = "Nhập hợp đồng"
                    //, _tcmota = "Thêm mới thi công.", 
                    _ntmota = "Nhập biên bản nghiệm thu.";
                  
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
                var dateBBNT = _bbntDao.Get(maddk);
                var maddkkh = _khDao.GetMADDK(maddk);
                
                //if (dateBBNT != null)
                //{ 
                //    lbNGAYBBNT.Text = dateBBNT.NGAYNHAP.Value.ToString("dd/MM/yyyy");
                //    lbNVKYNGHIEMTHU.Text = dateBBNT.NGAYLAPBB.Value.ToString("dd/MM/yyyy");
                //}
                //else 
                //{ 
                //    lbNGAYBBNT.Text = "";
                //    lbNVKYNGHIEMTHU.Text = "";
                //};

                if (maddkkh != null)
                {
                    lbNGAYKHAITHAC.Text = maddkkh.KYKHAITHAC.Value.ToString("MM/yyyy");
                }
                else { lbNGAYKHAITHAC.Text = ""; }

                var mathicong = tcDao.Get(maddk);
                
                if (mathicong != null)
                {
                    if (mathicong.MADH != null)
                    {
                        var dhno = _dhDao.Get(mathicong.MADH);

                        if (dhno != null)
                        {
                            lbSONODH.Text = dhno.SONO.ToString();
                        }
                        else
                        {
                            lbSONODH.Text = "";
                            ShowError("Số No đồng hồ sai. Kiểm tra lại. Vào phần sửa đồng hồ chưa khai thác nhập lại. ","");
                        }
                    }
                    else 
                    { 
                        lbSONODH.Text = ""; 
                    }
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

            //var phongban = _pbDao.GetListKV(ddlKHUVUC.SelectedValue);
            var phongban = _pbDao.GetList();            

            ddlToNhaMay.Items.Clear();
            ddlToNhaMay.Items.Add(new ListItem("Tất cả", "%"));
            //ddlToNhaMay.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
            //ddlToNhaMay.Items.Add(new ListItem("Phòng KT Điện Nước", "KTDN"));
            foreach (var pb in phongban)
            {
                ddlToNhaMay.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
            }
        }

        protected void ckISHONGHEO_CheckedChanged(object sender, EventArgs e)
        {
            if (ckISHONGHEO.Checked)
            {
                txtDIACHIHN.Enabled = true;
                ddlTENXA.Enabled = true;
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
                txtDIACHIHN.Enabled = false;
                ddlTENXA.Enabled = false;
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;
            }
        }

        protected void ddlTENXA_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDONVICAP.Text = ddlTENXA.SelectedValue;
            txtMASOHN.Focus();
        }

        private void BindDPKHKEBEN()
        {
            var list = dpDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
            gvDPKHKEBEN.DataSource = list;
            gvDPKHKEBEN.PagerInforText = list.Count.ToString();
            gvDPKHKEBEN.DataBind();
        }

        #region gvDPKHKEBEN
        protected void gvDPKHKEBEN_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDPKHKEBEN_RowCommand(object sender, GridViewCommandEventArgs e)
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
                            txtDPKHKEBEN.Text = dp.MADP;

                            upnlInfor.Update();

                            HideDialog("divDPKHKEBEN");
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

        protected void gvDPKHKEBEN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDPKHKEBEN.PageIndex = e.NewPageIndex;
                BindDPKHKEBEN();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        protected void btTIMSONODHN_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(txtSONODHN.Text.Trim()))
                {
                    Filtered = FilteredMode.Filtered;

                    var objList = ddkDao.GetListDonDongHo(ddlKHUVUC.SelectedValue, txtSONODHN.Text.Trim());

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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;                

                var maddkkh = _khDao.GetMADDK(lbMADDK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = ddkDao.Get(lbMADDK.Text.Trim());

                _rpClass.HisBienCo(dondk.MADDK, dondk.MAKV, LoginInfo.MANV, "DONHIS");

                Filtered = FilteredMode.None;

                var msg = ddkDao.UpdateNoiDungDDK(lbMADDK.Text.Trim(), lbDDKNOIDUNG.Text.Trim(), CommonFunc.GetComputerName(), 
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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khDao.GetMADDK(lbTKMADDK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = ddkDao.Get(lbTKMADDK.Text.Trim());

                _rpClass.HisBienCo(dondk.MADDK, dondk.MAKV, LoginInfo.MANV, "THIETKEHIS");

                Filtered = FilteredMode.None;

                var msg = tkDao.UpdateGhiChuTK(lbTKMADDK.Text.Trim(), lbGhiChuTK.Text.Trim(), CommonFunc.GetComputerName(),
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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khDao.GetMADDK(lbCTMADDK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = ddkDao.Get(lbCTMADDK.Text.Trim());

                _rpClass.HisBienCo(dondk.MADDK, dondk.MAKV, LoginInfo.MANV, "ChietTinhHis");

                Filtered = FilteredMode.None;

                var msg = ctDao.UpdateGhiChuCT(lbCTMADDK.Text.Trim(), lbCTGHICHU.Text.Trim(), CommonFunc.GetComputerName(),
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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khDao.GetMADDK(lbHDMADDK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = ddkDao.Get(lbHDMADDK.Text.Trim());

                //_rpClass.HisBienCo(dondk.MADDK, dondk.MAKV, LoginInfo.MANV, "ChietTinhHis");

                Filtered = FilteredMode.None;

                var msg = hdDao.UpdateGhiChu(lbHDMADDK.Text.Trim(), txtGhiChuHopDongHD.Text.Trim(), CommonFunc.GetComputerName(),
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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khDao.GetMADDK(lbTCMADDK.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = ddkDao.Get(lbTCMADDK.Text.Trim());

                _rpClass.HisBienCo(lbTCMADDK.Text.Trim(), dondk.MAKV, LoginInfo.MANV, "ThiCongHis");

                Filtered = FilteredMode.None;

                var msg = tcDao.UpdateGhiChuTC(lbTCMADDK.Text.Trim(), lbTCGHICHU.Text.Trim(), CommonFunc.GetComputerName(),
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
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (UpdateMode != Mode.Update)
                    return;

                var maddkkh = _khDao.GetMADDK(lbMADDKBBNT.Text.Trim());
                if (maddkkh != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                    return;
                }

                var dondk = ddkDao.Get(lbMADDKBBNT.Text.Trim());

                _rpClass.HisBienCo(lbMADDKBBNT.Text.Trim(), dondk.MAKV, LoginInfo.MANV, "NghiemThuHis");

                Filtered = FilteredMode.None;

                var msg = _bbntDao.UpdateGhiChuNT(lbMADDKBBNT.Text.Trim(), txtGhiChuNT.Text.Trim(), CommonFunc.GetComputerName(),
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