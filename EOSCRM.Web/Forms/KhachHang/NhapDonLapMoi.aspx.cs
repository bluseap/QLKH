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
    public partial class NhapDonLapMoi : Authentication
    {
        private readonly StoredProcedureDao _spDao = new StoredProcedureDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly MucDichSuDungDao mdsdDao = new MucDichSuDungDao();
        private readonly PhuongDao phuongDao = new PhuongDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly HinhThucThanhToanDao htttDao = new HinhThucThanhToanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();

        string huyentinhct = "";

        #region Properties

        private DONDANGKY DonDangKy
        {
            get
            {
                if (!IsDataValid())
                    return null;

                //var obj = ddkDao.Get(txtMADDK.Text.Trim()) ?? new DONDANGKY { MADDK = txtMADDK.Text.Trim() };
                var obj = (ddkDao.Get(txtMADDK.Text.Trim()) != null) ? ddkDao.Get(txtMADDK.Text.Trim()) : new DONDANGKY { MADDK = ddlKHUVUC.SelectedValue + lbMAPHONG.Text.Trim() + ddkDao.NewId() };
                
                obj.MADDKTONG = null; //ddlMADDKTONG.SelectedValue.Equals("") ? null : ddlMADDKTONG.SelectedValue,
                obj.TENKH = txtTENKH.Text.Trim().ToUpper();
                obj.TENDK = txtUYQUYEN.Text.Trim();
                obj.TENCHUCVU = string.IsNullOrEmpty(txtTENCHUCVU.Text.Trim()) ? " " : "Chức vụ:" + txtTENCHUCVU.Text.Trim();
                //obj.SONHA = txtSONHA.Text.Trim() + huyentinhct;
                //txtHUYEN
                //obj.SONHA = txtSONHA.Text.Trim() + "," + txtHUYEN.Text.Trim();
                obj.SONHA = txtSoNhaTenDuongKH.Text.Trim() + "," + ddlPhuongXaKH.SelectedItem.Text.ToString() + "," +
                    ddlToApKH.SelectedValue == "0" ? "" : ddlToApKH.SelectedItem.Text.ToString();

                obj.DIENTHOAI = txtDIENTHOAI.Text.Trim();
                obj.CMND = txtCMND.Text.Trim();

                string namsinh = "11/" + "11/" + txtNGAYSINH.Text.Trim();
                obj.NGAYSINH = namsinh;

                if (!txtCAPNGAY.Text.Trim().Equals(String.Empty))
                    obj.CAPNGAY = DateTimeUtil.GetVietNamDate(txtCAPNGAY.Text.Trim());
                else
                    obj.CAPNGAY = null;
                obj.TAI = txtTAI.Text.Trim();
                var httt = htttDao.Get(ddlHTTT.SelectedValue);
                obj.MAHTTT = httt != null ? httt.MAHTTT : null; 
                
                obj.TEN_DC_KHAC = txtDIACHIKHAC.Text.Trim();//SOHK
                obj.DAIDIEN = false; //cbDAIDIEN.Checked,
                
                //ho so khach hang
                String ndhk="",ndcm="",ndxn="",ndkd="";
                if (ckHK.Checked == true)
                {
                    ndhk = "Hộ khẩu photo. ";
                }
                else { ndhk = ""; }
                if (ckCM.Checked == true)
                {
                    ndcm = "CMND photo. ";
                }
                else { ndcm = ""; }
                if (ckXN.Checked == true)
                {
                    ndxn = "Giấy xác nhận địa phương. ";
                }
                else { ndxn = ""; }
                if (ckKD.Checked == true)
                {
                    ndkd = "Giấy phép ĐKKD (đ/v SX-KD-DV). ";
                }
                else { ndkd = ""; }


                string ndtt = "", cnnha = "", hdtnha = "", cndat = "", chuyennhuong = "", giaytocanthiet = "";
                if (ckTAMTRU.Checked == true)
                {
                    ndtt = "Giấy chứng nhận tạm trú dài hạn. ";
                }
                else { ndtt = ""; }                
                if (ckCNNHA.Checked == true)
                {
                    cnnha = "Giấy chứng nhận sở hữu nhà. ";
                }
                else { cnnha = ""; }
                if (ckHDTHUE.Checked == true)
                {
                    hdtnha = "Hợp đồng thuê nhà. ";
                }
                else { hdtnha = ""; }

                if (ckCNDAT.Checked == true)
                {
                    cndat = "Giấy chứng nhận sở hữu đất.(Photo,công chứng) ";
                }
                else { cndat = ""; }

                if (ckCHUYENNHUONG.Checked == true)
                {
                    chuyennhuong = "Giấy chuyển nhượng (Photo,công chứng) ";
                }
                else { chuyennhuong = ""; }

                if (ckHSCANTHIET.Checked == true)
                {
                    giaytocanthiet = "Những giấy tờ cần thiết. (Photo,công chứng) ";
                }
                else { giaytocanthiet = ""; }

                //obj.NOIDUNG = txtNOIDUNG.Text.Trim();
                obj.NOIDUNG = ndhk + ndcm + ndxn + ndkd + ndtt + cnnha + hdtnha + cndat + chuyennhuong + giaytocanthiet;

                obj.CTCTMOI = false;
                obj.MANV = LoginInfo.MANV;

                var khuvuc = kvDao.Get(ddlKHUVUC.SelectedValue);
                obj.MAKV = khuvuc != null ? khuvuc.MAKV : null;

                // dai dien, ma duong
                var phuong = phuongDao.GetMAKV(ddlPHUONG.SelectedValue, ddlKHUVUC.SelectedValue);
                if (phuong != null)
                    obj.MAPHUONG = phuong != null ? (phuong.MAPHUONG != null ? phuong.MAPHUONG : null): null;               

                var mdsd = mdsdDao.Get(ddlMUCDICH.SelectedValue);
                obj.MAMDSD = mdsd != null ? mdsd.MAMDSD: null;                

                if (!txtSOHODN.Text.Trim().Equals(String.Empty))
                    obj.SOHODN = Convert.ToInt32(txtSOHODN.Text.Trim());
                else
                    obj.SOHODN = null;

                var sn = (txtSONHA.Text.Trim().Equals(String.Empty) ? "" : txtSONHA.Text.Trim() + ", ");
                var tenduong = "";
                
                //var tenphuong = phuong != null ? phuong.TENPHUONG + ", " : "";
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
                    tenduong = txtDIACHIKHAC.Text.Trim().Equals(String.Empty) ? "" : txtDIACHIKHAC.Text.Trim() + ", ";
                }

                //obj.DIACHILD = string.Format("{0}{1}{2}{3}", sn, tenduong, tenphuong, tenkv);
                //obj.DIACHILD = txtDIACHILAPDAT.Text.Trim() + huyentinhct; 
                //"," + txtHUYEN.Text.Trim();
                obj.DIACHILD = txtDIACHILAPDAT.Text.Trim() + "," + txtHUYENDLLAP.Text.Trim();

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
                    obj.NGAYDK = DateTime.Now;

                if (!txtNGAYKS.Text.Trim().Equals(String.Empty))
                    obj.NGAYHKS = DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());
                else
                    obj.NGAYHKS = DateTime.Now;


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


                obj.MST = txtMST.Text.Trim();

                obj.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if(cbSDInfo_INHOADON.Checked)
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

                obj.NGAYN = DateTime.Now;
                obj.SONHA2 = txtSONHA2.Text.Trim();
                obj.MADPKHGAN = !string.IsNullOrEmpty(txtDPKHKEBEN.Text.Trim()) ? txtDPKHKEBEN.Text.Trim() : "";
                obj.MADBKHGAN = !string.IsNullOrEmpty(txtMADBKHKEBEN.Text.Trim()) ? txtMADBKHKEBEN.Text.Trim() : "";

                obj.TIENCOCLX = !string.IsNullOrEmpty(txtTEINCOCLX.Text.Trim()) ? Convert.ToInt32(txtTEINCOCLX.Text.Trim()) : 
                        Convert.ToInt32("0");
                obj.TIENVATTULX = !string.IsNullOrEmpty(txtTIENVATTULX.Text.Trim()) ? Convert.ToInt32(txtTIENVATTULX.Text.Trim()) :
                        Convert.ToInt32("0");

                // TENDUONG = so nha + ten duong
                obj.TENDUONG = txtSoNhaTenDuongKH.Text.Trim();
                obj.ThanhPhoTinhIdKH = Convert.ToInt32(ddlTinhKH.SelectedValue.ToString());
                obj.QuanHuyenIdKH = Convert.ToInt32(ddlThanhPhoHuyenKH.SelectedValue.ToString());
                obj.MAXA = ddlPhuongXaKH.SelectedValue.ToString();
                obj.TENXA = ddlPhuongXaKH.SelectedItem.Text.ToString();
                obj.MAAPTO = ddlToApKH.SelectedValue.ToString();
                obj.TENAPTO = ddlToApKH.SelectedItem.Text.ToString();

                obj.SoNhaTenDuongLD = txtSoNhaTenDuongDCL.Text.Trim();
                obj.ThanhPhoTinhIdLD = Convert.ToInt32(ddlTinhDCL.SelectedValue.ToString());
                obj.QuanHuyenIdLD = Convert.ToInt32(ddlThanhPhoHuyenDCL.SelectedValue.ToString());
                obj.PhuongXaIdLD = Convert.ToInt32(ddlPhuongXaDCL.SelectedValue.ToString());
                obj.TenPhuongXaLD = ddlPhuongXaDCL.SelectedItem.Text.ToString();
                obj.MaApLD = ddlToApDCL.SelectedValue.ToString();
                obj.TenApLD = ddlToApDCL.SelectedItem.Text.ToString();

                return obj;
            }
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

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_DonLapDatMoi, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
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
            Page.Title = Resources.Message.TITLE_KH_DONLAPDATMOI;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_DONLAPDATMOI;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDPKHKEBEN);
        }        

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);

            lbMAPHONG.Text = _nvDao.Get(b).MAPB; 
            foreach (var a in query)
            {
                string d = a.MAKV;//makv

                if (a.MAKV == "99")
                {
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    var khuvuc = kvDao.Get(d);
                    var phuongList = phuongDao.GetList(d);
                    ddlKHUVUC.Items.Clear();
                    ddlPHUONG.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));                        
                    }
                    foreach(var ph in phuongList)
                    {
                        ddlPHUONG.Items.Add(new ListItem(ph.TENPHUONG,ph.MAPHUONG));
                    }
                    txtHUYEN.Text = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";
                    txtHUYENDLLAP.Text = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";
                    huyentinhct = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";

                    //xa phuong
                    var listXAPHUONG = _xpDao.GetListKV(d);
                    ddlTENXA.DataSource = listXAPHUONG;
                    ddlTENXA.DataTextField = "TENXA";
                    ddlTENXA.DataValueField = "MAXA";
                    ddlTENXA.DataBind();
                    txtDONVICAP.Text = ddlTENXA.SelectedValue;
                }
                //lbMAPHONG.Text = a.MAPB.ToString();
                //txtHUYEN.Text = huyentinhct;
            }

        }

        private void LoadStaticReferences()
        {
            try
            {
                UpdateMode = Mode.Create;
                Filtered = FilteredMode.None;

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                /*var khuvuclist = kvDao.GetList();
                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in khuvuclist)
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                ddlKHUVUC.SelectedIndex = 1;*/ 
                //LoadDynamicReferences();
                // bind dllMDSD
                timkv();

                var mdsdList = mdsdDao.GetList();
                ddlMUCDICH.Items.Clear();
                ddlMUCDICH.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var mdsd in mdsdList)
                    ddlMUCDICH.Items.Add(new ListItem(mdsd.TENMDSD, mdsd.MAMDSD));

                ddlMUCDICH.SelectedIndex = 1;

                //txtMADDK.Text = ddkDao.NewId();
                txtMADDK.Text = ddlKHUVUC.SelectedValue + lbMAPHONG.Text.Trim() + ddkDao.NewId();

                txtMADDK.Focus();

                txtCAPNGAY.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYCD.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYKS.Text = DateTime.Now.ToString("dd/MM/yyyy");

                var listHTTT = htttDao.GetList();
                ddlHTTT.DataSource = listHTTT;
                ddlHTTT.DataTextField = "MOTA";
                ddlHTTT.DataValueField = "MAHTTT";
                ddlHTTT.DataBind();                

                //ddlHOSO.Items.Add(new ListItem("Hộ khẩu thường trú", "HKTT"));
                //ddlHOSO.Items.Add(new ListItem("Xác nhận địa phương", "XNDP"));
                //ddlHOSO.Items.Add(new ListItem("Giấy CNQSD đất", "GCNQ"));
                //ddlHOSO.Items.Add(new ListItem("Kinh doanh", "DKKD"));

                if (_nvDao.Get(b).MAKV == "X")
                {
                    lbSONHAN2.Visible = true;
                    txtSONHA2.Visible = true;
                }

                txtTEINCOCLX.Text = "0";
                txtTIENVATTULX.Text = "0";

                loadTinhPhuongXa(_nvDao.Get(b).MAKV);

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadDynamicReferences()
        {
            /*
            // bind ddlDAIDIEN
            var dd = ddkDao.GetDaiDienList(ddlKHUVUC.SelectedValue);
            ddlMADDKTONG.Items.Clear();
            ddlMADDKTONG.Items.Add(new ListItem("", ""));

            foreach (var d in dd)
            {
                ddlMADDKTONG.Items.Add(new ListItem(d.MADDK, d.MADDK));
            }
            */

            // bind dllPHUONG
            var items = pDao.GetList(ddlKHUVUC.SelectedValue);

            ddlPHUONG.Items.Clear();
            ddlPHUONG.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var p in items)
                ddlPHUONG.Items.Add(new ListItem(p.TENPHUONG, p.MAPHUONG));
        }
        
        private void BindDataForGrid()
        {
            try
            {
                /*if (Filtered == FilteredMode.None)
                {
                    //hien theo phong ban, khu vuc
                    var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo == null) return;
                    string b = loginInfo.Username;
                    var query = _nvDao.Get(b);//nhan vien khu vuc ??

                    var objList = ddkDao.GetListKV(query.MAKV.ToString());
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else*/
                //{ 
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
                        || query.MAPB == "LA" || query.MAPB == "NC" || query.MAPB == "VH") // TAN CHAU)
                    {
                        var objList = ddkDao.GetListMAPB(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                        txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                        ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue, query.MAPB.ToString());

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else //if (query.MAPB == "KD")
                    {                                               

                        var objList = ddkDao.GetListKV(query.MAKV.ToString());
                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();

                        /*var objList = ddkDao.GetList(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                        txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                        ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind(); */
                    }
                    /*var objList = ddkDao.GetList(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                        txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                        ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();*/


                //}
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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

                if(ngayks < ngaycd)
                {
                    ShowError("Ngày hẹn khảo sát phải sau ngày nhận đơn", txtNGAYKS.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtCAPNGAY.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtCAPNGAY.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Kiểm tra lại ngày cấp CMND."), txtCAPNGAY.ClientID);
                    return false;
                }
            }

            //nam sinh
            /*if (!string.Empty.Equals(txtNGAYSINH.Text.Trim()) )
            {
                try
                {
                    String namsinh = "11/" + "11/" + txtNGAYSINH.Text.Trim();
                    DateTimeUtil.GetVietNamDate(namsinh);
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Năm sinh "), txtNGAYSINH.ClientID);
                    return false;
                }
            }*/

            #endregion

            return true;
        }

        private void ClearContent()
        {
            //TODO: xóa UI
            txtMADDK.Text = ddlKHUVUC.SelectedValue + lbMAPHONG.Text.Trim() + ddkDao.NewId();
            txtMADDK.ReadOnly = false;
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtCMND.Text = "";
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
            txtMST.Text = "";
            cbSDInfo_INHOADON.Checked = false;
            txtTENKH_INHOADON.Text = "";
            txtDIACHI_INHOADON.Text = "";            
            txtNGAYSINH.Text = "";
            txtCAPNGAY.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTAI.Text = "";
            ddlHTTT.SelectedIndex = 0;
            txtNOIDUNG.Text = "";
            txtNOILAPDHN.Text = "";
            txtDIACHILAPDAT.Text = "";

            ddlTENXA.SelectedIndex = 0;
            txtDONVICAP.Text = ddlTENXA.SelectedValue;
            txtMASOHN.Text = "";
            txtNGAPCAPHN.Text = "";
            txtNGAYKTHN.Text = "";
            txtNGAYKYSOHN.Text = "";
            txtDIACHIHN.Text = "";

            cbISTUYENONGCHUNG.Checked = false;

            txtTEINCOCLX.Text = "0";
            txtTIENVATTULX.Text = "0";
            ckKHMuaVatTu.Checked = false;

            txtSoDienThoai2.Text = "";

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
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var makvts = _nvDao.Get(b);

            var don = DonDangKy;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (!string.Empty.Equals(txtCAPNGAY.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtCAPNGAY.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Kiểm tra lại ngày cấp CMND."), txtCAPNGAY.ClientID);
                    return ;
                }
            }

            if (txtTENKH.Text.Trim() == "") { 
                CloseWaitingDialog(); 
                return; 
            }                   

            Message msg;
            Filtered = FilteredMode.None;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.KH_DonLapDatMoi, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // check exists
                var existed = ddkDao.Get(don.MADDK);

                // đảm bảo không bị tình trạng lặp vô tận, quá lắm 100 lần không thể sai được
                var count = 0;
                while (existed != null && count < 100)
                {
                    txtMADDK.Text = ddkDao.NewId();
                    don.MADDK = txtMADDK.Text.Trim();
                    existed = ddkDao.Get(don.MADDK);
                    count++;
                }

                // default value
                don.LOAIDK = LOAIDK.DK.ToString();
                don.TTDK = TTDK.DK_A.ToString();
                don.MAPB = makvts.MAPB != null ? makvts.MAPB : "KD";

                // insert
                msg = ddkDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);                

                _rpClass.HisNgayDangKyBien(don.MADDK, LoginInfo.MANV, don.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                    "","","","","INDONDK");

                _spDao.Update_DonDangKy_SoDienThoai2MuaVatTu(don.MADDK, txtSoDienThoai2.Text.Trim(), ckKHMuaVatTu.Checked, LoginInfo.MANV);
            }
            else
            {
                if (!HasPermission(Functions.KH_DonLapDatMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = ddkDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
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
                CloseWaitingDialog();
               
                UpdateMode = Mode.Create;
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }           
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            UpdateMode = Mode.Create;
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
                if (!HasPermission(Functions.KH_DonLapDatMoi, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
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

                        // bind pager
                        UpdateMode = Mode.Create;
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }
        
        private void SetDDKToForm(DONDANGKY ddk)
        {
            SetControlValue(txtMADDK.ClientID, ddk.MADDK);
            SetReadonly(txtMADDK.ClientID, true);

            SetControlValue(txtTENKH.ClientID, ddk.TENKH);
            SetControlValue(txtSONHA.ClientID, ddk.SONHA);
            SetControlValue(txtCMND.ClientID, ddk.CMND);
            SetControlValue(txtDIENTHOAI.ClientID, ddk.DIENTHOAI);
            SetControlValue(txtDIACHIKHAC.ClientID, ddk.TEN_DC_KHAC);

            SetControlValue(txtNGAYSINH.ClientID, ddk.NGAYSINH);
            SetControlValue(txtCAPNGAY.ClientID, ddk.CAPNGAY.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.CAPNGAY.Value) : "");
            SetControlValue(txtTAI.ClientID, ddk.TAI);
            var httt = ddlHTTT.Items.FindByValue(ddk.MAHTTT);
            if (httt != null)
                ddlHTTT.SelectedIndex = ddlHTTT.Items.IndexOf(httt);
            SetControlValue(txtNOIDUNG.ClientID, ddk.NOIDUNG);

            if(ddk.DUONGPHO != null)
            {
                SetControlValue(txtMADP.ClientID, ddk.MADP);
                SetControlValue(txtDUONGPHU.ClientID, ddk.DUONGPHU);
            }
            
            SetControlValue(txtSOHODN.ClientID, ddk.SOHODN.HasValue ? String.Format("{0:0,0}", ddk.SOHODN.Value) : "");
            SetControlValue(txtSONK.ClientID, ddk.SONK.HasValue ? String.Format("{0:0,0}", ddk.SONK.Value) : "");
            SetControlValue(txtDMNK.ClientID, ddk.DMNK.HasValue ? String.Format("{0:0,0}", ddk.DMNK.Value) : "");

            SetControlValue(txtNGAYCD.ClientID, ddk.NGAYDK.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYDK.Value) : "");
            SetControlValue(txtNGAYKS.ClientID, ddk.NGAYHKS.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYHKS.Value) : "");

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

            var isCheckedONGCAI = ddk.ISTUYENONGCHUNG.HasValue && ddk.ISTUYENONGCHUNG.Value;
            cbISTUYENONGCHUNG.Checked = isCheckedONGCAI;

            upnlInfor.Update();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditHoSo":
                        /*
                        if (!string.Empty.Equals(madon))
                        {
                            var don = ddkDao.Get(madon);
                            if (don == null) return;

                            UpdateMode = Mode.Update;
                            SetDDKToForm(don);
                        }*/

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
                // Update page index
                gvList.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

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

                CloseWaitingDialog();
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

                            //UpdateKhuVuc(dp);
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

        protected void ckISHONGHEO_CheckedChanged(object sender, EventArgs e)
        {
            if (ckISHONGHEO.Checked)
            {
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
        }

        protected void ddlTENXA_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtDONVICAP.Text = ddlTENXA.SelectedValue;
            txtMASOHN.Focus();
        }

        protected void lkKTCMND_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var makvts = _nvDao.Get(b);

                if (makvts.MAKV != "O" && makvts.MAKV != "X")
                {
                    if (ddkDao.DemCMND(txtCMND.Text.Trim(), makvts.MAKV) > 0)//kiem tra cmnd trùng.
                    {
                        CloseWaitingDialog();
                        ShowError("Chứng minh nhân dân bị trùng. Kiểm tra lại");
                        return;
                    }

                    if (txtCMND.Text.Trim().Length < 9) //kiem tra cmnd trùng.
                    {
                        CloseWaitingDialog();
                        ShowError("Chứng minh nhân dân >= 9 số. Kiểm tra lại");
                        return;
                    }
                }
                
                CloseWaitingDialog();
                txtCAPNGAY.Focus();
            }
            catch { }
        }

        protected void lkKTSDT_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var makvts = _nvDao.Get(b);

                if (makvts.MAKV == "X")
                {
                    if (txtDIENTHOAI.Text.Trim().Length < 10)
                    {
                        CloseWaitingDialog();
                        ShowError("Số điện thoại >= 10 số. Kiểm tra lại");
                        return;
                    }
                }

                CloseWaitingDialog();
                txtMST.Focus();
            }
            catch { }
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

        protected void btDPKHKEBEN_Click(object sender, EventArgs e)
        {
            BindDPKHKEBEN();
            upnlDPKHKEBEN.Update();
            UnblockDialog("divDPKHKEBEN");
        }

        protected void txtDPKHKEBEN_TextChanged(object sender, EventArgs e)
        {
            string madpkh = txtDPKHKEBEN.Text.Trim();

            var dp = dpDao.GetDP(madpkh.ToUpper());

            if (dp == null)
            {                
                ShowError("Chọn danh số khách hàng kế bên cho đúng.");

                txtDPKHKEBEN.Focus();
                upnlInfor.Update();

                CloseWaitingDialog();
                return;
            }
        }

        private void loadTinhPhuongXa(string makv)
        {
            try
            {
                int tinhAnGiang = 89;

                var tinhthanhpho = _spDao.Get_ThanhPhoTinh_ByAll();
                var quanhuyenByTinhAnGiang = _spDao.Get_QuanHuyen_ByThanhPhoTinhId(tinhAnGiang);
                //var phuongxa = _spDao.Get_PhuongXa_ByAll();
                
                var tinhthanhphoById = _spDao.Get_ThanhPhoTinh_ById(tinhAnGiang);

                ddlTinhKH.Items.Clear();                
                ddlTinhKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));      
                if (tinhthanhpho.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < tinhthanhpho.Tables[0].Rows.Count; i++)
                    {
                        ddlTinhKH.Items.Add(new System.Web.UI.WebControls.ListItem(tinhthanhpho.Tables[0].Rows[i]["TenTinh"].ToString(),
                            tinhthanhpho.Tables[0].Rows[i]["Id"].ToString()));                       
                    }
                }

                ddlThanhPhoHuyenKH.Items.Clear();
                ddlThanhPhoHuyenKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                
                ddlPhuongXaKH.Items.Clear();
                ddlPhuongXaKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));                

                ddlToApKH.Items.Clear();
                ddlToApKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));


                ddlTinhDCL.Items.Clear();                             
                if (tinhthanhphoById.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < tinhthanhphoById.Tables[0].Rows.Count; i++)
                    {
                        ddlTinhDCL.Items.Add(new System.Web.UI.WebControls.ListItem(tinhthanhphoById.Tables[0].Rows[i]["TenTinh"].ToString(),
                            tinhthanhphoById.Tables[0].Rows[i]["Id"].ToString()));                        
                    }
                }

                ddlThanhPhoHuyenDCL.Items.Clear();
                ddlThanhPhoHuyenDCL.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                if (quanhuyenByTinhAnGiang.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < quanhuyenByTinhAnGiang.Tables[0].Rows.Count; i++)
                    {
                        ddlThanhPhoHuyenDCL.Items.Add(new System.Web.UI.WebControls.ListItem(quanhuyenByTinhAnGiang.Tables[0].Rows[i]["TenQuan"].ToString(),
                            quanhuyenByTinhAnGiang.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }

                ddlPhuongXaDCL.Items.Clear();
                ddlPhuongXaDCL.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));

                ddlToApDCL.Items.Clear();
                ddlToApDCL.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));

            }
            catch { }
        }

        protected void ddlTinhKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int tinhId = Convert.ToInt32(ddlTinhKH.SelectedValue.ToString());
                var quanhuyenByTinh = _spDao.Get_QuanHuyen_ByThanhPhoTinhId(tinhId);

                ddlThanhPhoHuyenKH.Items.Clear();
                ddlThanhPhoHuyenKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                if (quanhuyenByTinh.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < quanhuyenByTinh.Tables[0].Rows.Count; i++)
                    {
                        ddlThanhPhoHuyenKH.Items.Add(new System.Web.UI.WebControls.ListItem(quanhuyenByTinh.Tables[0].Rows[i]["TenQuan"].ToString(),
                            quanhuyenByTinh.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }
            }
            catch { }
        }

        protected void ddlThanhPhoHuyenKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int quanhuyenId = Convert.ToInt32(ddlThanhPhoHuyenKH.SelectedValue.ToString());
                var phuongxaByQuanHuyen = _spDao.Get_PhuongXa_ByQuanHuyenId(quanhuyenId);

                ddlPhuongXaKH.Items.Clear();
                ddlPhuongXaKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                if (phuongxaByQuanHuyen.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < phuongxaByQuanHuyen.Tables[0].Rows.Count; i++)
                    {
                        ddlPhuongXaKH.Items.Add(new System.Web.UI.WebControls.ListItem(phuongxaByQuanHuyen.Tables[0].Rows[i]["TenPhuongXa"].ToString(),
                            phuongxaByQuanHuyen.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }
                loadTinhThanhPhoKH();
            }
            catch { }
        }

        protected void ddlPhuongXaKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string phuongxaId = ddlPhuongXaKH.SelectedValue.ToString();
                var aptoByPhuongXa = _spDao.Get_ApTo_ByPhuongXaId(phuongxaId);

                ddlToApKH.Items.Clear();
                ddlToApKH.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                if (aptoByPhuongXa.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < aptoByPhuongXa.Tables[0].Rows.Count; i++)
                    {
                        ddlToApKH.Items.Add(new System.Web.UI.WebControls.ListItem(aptoByPhuongXa.Tables[0].Rows[i]["TENAPTO"].ToString(),
                            aptoByPhuongXa.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }
                loadPhuongApKH();
            }
            catch { }
        }

        protected void ddlToApKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                loadPhuongApKH();
            }
            catch { }
        }

        protected void ddlThanhPhoHuyenDCL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int quanhuyenId = Convert.ToInt32(ddlThanhPhoHuyenDCL.SelectedValue.ToString());
                var phuongxaByQuanHuyen = _spDao.Get_PhuongXa_ByQuanHuyenId(quanhuyenId);

                ddlPhuongXaDCL.Items.Clear();
                ddlPhuongXaDCL.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                if (phuongxaByQuanHuyen.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < phuongxaByQuanHuyen.Tables[0].Rows.Count; i++)
                    {
                        ddlPhuongXaDCL.Items.Add(new System.Web.UI.WebControls.ListItem(phuongxaByQuanHuyen.Tables[0].Rows[i]["TenPhuongXa"].ToString(),
                            phuongxaByQuanHuyen.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }
                loadTinhThanhPhoDCL();
            }
            catch { }
        }

        protected void ddlPhuongXaDCL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string phuongxaId = ddlPhuongXaDCL.SelectedValue.ToString();
                var aptoByPhuongXa = _spDao.Get_ApTo_ByPhuongXaId(phuongxaId);

                ddlToApDCL.Items.Clear();
                ddlToApDCL.Items.Add(new System.Web.UI.WebControls.ListItem("-- Tất cả --", "0"));
                if (aptoByPhuongXa.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < aptoByPhuongXa.Tables[0].Rows.Count; i++)
                    {
                        ddlToApDCL.Items.Add(new System.Web.UI.WebControls.ListItem(aptoByPhuongXa.Tables[0].Rows[i]["TENAPTO"].ToString(),
                            aptoByPhuongXa.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }
                loadPhuongApDLC();
            }
            catch { }
        }

        protected void ddlToApDCL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                loadPhuongApDLC();
            }
            catch { }
        }

        private void loadTinhThanhPhoKH()
        {
            try
            {
                string tinh = ddlTinhKH.SelectedItem.Text.ToString();
                string huyen = ddlThanhPhoHuyenKH.SelectedItem.Text.ToString();
                txtHUYEN.Text = huyen + "," + tinh;
            }
            catch { }
        }

        private void loadPhuongApKH()
        {
            try
            {
                string phuong = ddlPhuongXaKH.SelectedItem.Text.ToString();
                string ap = ddlToApKH.SelectedValue == "0" ? "" : ddlToApKH.SelectedItem.Text.ToString();
                txtDIACHILAPDAT.Text = ap + "," + phuong;
            }
            catch { }
        }

        private void loadTinhThanhPhoDCL()
        {
            try
            {
                string tinh = ddlTinhDCL.SelectedItem.Text.ToString();
                string huyen = ddlThanhPhoHuyenDCL.SelectedItem.Text.ToString();
                txtHUYENDLLAP.Text = huyen + "," + tinh;
            }
            catch { }
        }

        private void loadPhuongApDLC()
        {
            try
            {
                string phuong = ddlPhuongXaDCL.SelectedItem.Text.ToString();
                string ap = ddlToApDCL.SelectedValue == "0" ? "" : ddlToApKH.SelectedItem.Text.ToString();
                txtDIACHILAPDAT.Text = ap + "," + phuong;
            }
            catch { }
        }
        
    }
}