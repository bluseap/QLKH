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
    public partial class NhapDonLapMoiPower : Authentication
    {
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();
        private readonly MucDichSuDungPoDao _mdsdpoDao = new MucDichSuDungPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();

        //private readonly PhuongDao phuongDao = new PhuongDao();        
        private readonly HinhThucThanhToanDao htttDao = new HinhThucThanhToanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        string huyentinhct = "";

        #region Properties

        private DONDANGKYPO DonDangKyPo
        {
            get
            {
                if (!IsDataValid())
                    return null;

                //var obj = ddkDao.Get(txtMADDK.Text.Trim()) ?? new DONDANGKY { MADDK = txtMADDK.Text.Trim() };
                var obj = (_ddkpoDao.Get(txtMADDK.Text.Trim()) != null) ? _ddkpoDao.Get(txtMADDK.Text.Trim()) : new DONDANGKYPO { MADDKPO = ddlKHUVUC.SelectedValue + lbMAPHONG.Text.Trim() + _ddkpoDao.NewId() };

                obj.MADDKTONG = null; //ddlMADDKTONG.SelectedValue.Equals("") ? null : ddlMADDKTONG.SelectedValue,
                obj.TENKH = txtTENKH.Text.Trim().ToUpper();
                obj.TENDK = txtUYQUYEN.Text.Trim();

                //obj.SONHA = txtSONHA.Text.Trim() + huyentinhct;
                //txtHUYEN
                obj.SONHA = txtSONHA.Text.Trim() + "," + txtHUYEN.Text.Trim();

                obj.DIENTHOAI = txtDIENTHOAI.Text.Trim();
                obj.CMND = txtCMND.Text.Trim();

                String namsinh = "11/" + "11/" + txtNGAYSINH.Text.Trim();
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
                String ndhk = "", ndcm = "", ndxn = "", ndkd = "";                

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
                if (ckTAMTRU.Checked == true)
                {
                    ndkd = "Giấy chứng nhận tạm trú dài hạn. ";
                }
                else { ndkd = ""; }

                String cnnha = "", hdtnha = "", cndkkd = "", cnddthue = "";
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
                if (ckDKKD.Checked == true)
                {
                    cndkkd = "Giấy chứng nhận ĐKKD (đ/v SX-KD-DV). ";
                }
                else { cndkkd = ""; }
                if (ckDKTHUE.Checked == true)
                {
                    cnddthue = "Giấy chứng nhận đăng ký thuế. ";
                }
                else { cnddthue = ""; }

                string cnhongheo = "";
                if (ckHONGHEOKK.Checked == true)
                {
                    cnhongheo = "HN:Hộ nghèo/Cận nghèo/HC Khó khăn. ";
                }
                else { cnhongheo = ""; }//


                //obj.NOIDUNG = txtNOIDUNG.Text.Trim();
                obj.NOIDUNG = cnhongheo + ndhk + ndcm + ndxn + ndkd + cnnha + hdtnha + cndkkd + cnddthue ;

                obj.CTCTMOI = false;
                obj.MANV = LoginInfo.MANV;

                var khuvuc = _kvpoDao.Get(ddlKHUVUC.SelectedValue);
                obj.MAKVPO = khuvuc != null ? khuvuc.MAKVPO : null;

                // dai dien, ma duong
                var phuong = _ppoDao.GetMAKV(ddlPHUONG.SelectedValue, ddlKHUVUC.SelectedValue);
                obj.MAPHUONG = phuong != null ? phuong.MAPHUONGPO : null;                

                var mdsd = _mdsdpoDao.Get(ddlMUCDICH.SelectedValue);
                obj.MAMDSDPO = mdsd != null ? mdsd.MAMDSDPO : null;

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
                    obj.MADPPO = txtMADP.Text.Trim();
                    obj.DUONGPHUPO = txtDUONGPHU.Text.Trim();

                    var duong = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
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

                var sonhanhapdon = string.IsNullOrEmpty(txtSoNhaNhapDon.Text.Trim()) ? "" : txtSoNhaNhapDon.Text.Trim();
                var tenduongnhapdon = string.IsNullOrEmpty(txtTenDuongNhapDon.Text.Trim()) ? "" : txtTenDuongNhapDon.Text.Trim();
                var maxaphuong = ddlPhuongXa.SelectedValue;
                var tenxaphuong = ddlPhuongXa.SelectedItem;

                obj.SONHA2 = sonhanhapdon;
                obj.TENDUONG = tenduongnhapdon;
                obj.MAXA = maxaphuong;
                obj.TENXA = tenxaphuong.ToString();
                obj.DIACHILD = sonhanhapdon + " " + tenduongnhapdon + "," + tenxaphuong + "," + txtHUYENDCLAP.Text.Trim();
                //obj.DIACHILD = txtDIACHILAPDAT.Text.Trim() + "," + txtHUYENDCLAP.Text.Trim();

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
                obj.SOTRUPO = txtSOTRUPO.Text.Trim();

                obj.NGAYN = DateTime.Now;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_DonLapDatMoiPo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_KH_DONLAPDATMOIPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_DONLAPDATMOIPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

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

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        #endregion

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
                    var kvnv = _nvDao.Get(b).MAKV;
                    var phuongList = _ppoDao.GetList(_kvpoDao.GetPo(kvnv).MAKVPO);

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
                    txtHUYEN.Text = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";
                    txtHUYENDCLAP.Text = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";
                    huyentinhct = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";
                }
                lbMAPHONG.Text = a.MAPB.ToString();
            }
        }

        private void LoadStaticReferences()
        {
            try
            {
                UpdateMode = Mode.Create;
                Filtered = FilteredMode.None;

                /*var khuvuclist = kvDao.GetList();
                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in khuvuclist)
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                ddlKHUVUC.SelectedIndex = 1;*/
                timkv();

                //LoadDynamicReferences();

                // bind dllMDSD
                var mdsdList = _mdsdpoDao.GetList();
                ddlMUCDICH.Items.Clear();
                ddlMUCDICH.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var mdsd in mdsdList)
                    ddlMUCDICH.Items.Add(new ListItem(mdsd.TENMDSD, mdsd.MAMDSDPO));
                ddlMUCDICH.SelectedIndex = 1;

                //txtMADDK.Text = ddkDao.NewId();
                txtMADDK.Text = ddlKHUVUC.SelectedValue + lbMAPHONG.Text.Trim() + _ddkpoDao.NewId();

                txtMADDK.Focus();

                txtCAPNGAY.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYCD.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYKS.Text = DateTime.Now.ToString("dd/MM/yyyy");

                var listHTTT = htttDao.GetList();
                ddlHTTT.DataSource = listHTTT;
                ddlHTTT.DataTextField = "MOTA";
                ddlHTTT.DataValueField = "MAHTTT";
                ddlHTTT.DataBind();

                PhuongXa(ddlKHUVUC.SelectedValue);

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PhuongXa(string makv)
        {
            try
            {
                var khuvuc = _kvpoDao.Get(makv);
                var xaphuong = _xpDao.GetListKV(khuvuc.MAKV);

                ddlPhuongXa.Items.Clear();
                ddlPhuongXa.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var xp in xaphuong)
                    ddlPhuongXa.Items.Add(new ListItem(xp.TENXA, xp.MAXA));
            }
            catch { }
        }

        private void LoadDynamicReferences()
        {

            // bind dllPHUONG   GetListKV
            //var items = _ppoDao.GetList(ddlKHUVUC.SelectedValue);
            var items = _ppoDao.GetListKV(ddlKHUVUC.SelectedValue);

            ddlPHUONG.Items.Clear();
            ddlPHUONG.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var p in items)
                ddlPHUONG.Items.Add(new ListItem(p.TENPHUONG, p.MAPHUONGPO));
        }

        private void BindDataForGrid()
        {
            try
            {
               
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

                var kvpo = _kvpoDao.GetPo(query.MAKV.ToString());

                if (query.MAPB == "NB" || query.MAPB == "TA" || query.MAPB == "TD"
                     || query.MAPB == "TS" || query.MAPB == "TO" || query.MAPB == "TK" || query.MAPB == "NS" || query.MAPB == "NH"
                        || query.MAPB == "CV" || query.MAPB == "HL" || query.MAPB == "MM" || query.MAPB == "PM" // PHU TAN
                        || query.MAPB == "BC" || query.MAPB == "CT" || query.MAPB == "NT" || query.MAPB == "TT" // tri ton
                        || query.MAPB == "CL" || query.MAPB == "MB" || query.MAPB == "NC" || query.MAPB == "TB" // TINH BIEN
                        || query.MAPB == "AT" || query.MAPB == "CM" || query.MAPB == "HB" || query.MAPB == "KT" // CHO MOI
                        || query.MAPB == "LG" || query.MAPB == "ML" || query.MAPB == "NL" || query.MAPB == "TM" // CHO MOI
                        || query.MAPB == "LA" || query.MAPB == "NC" || query.MAPB == "VH") // TAN CHAU)
                {
                    var objList = _ddkpoDao.GetListMAPB(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                    txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                    ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue, query.MAPB.ToString());

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else //if (query.MAPB == "KD")
                {
                    
                    var objList = _ddkpoDao.GetListKV(kvpo.MAKVPO.ToString());
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();

                    
                }
                
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
            if (!string.Empty.Equals(txtCAPNGAY.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtCAPNGAY.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày cấp CMND"), txtCAPNGAY.ClientID);
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
            txtMADDK.Text = ddlKHUVUC.SelectedValue + lbMAPHONG.Text.Trim() + _ddkpoDao.NewId();
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
            cbISTUYENONGCHUNG.Checked = false;
            txtSOTRUPO.Text = "";
            lbTENTRAMPO.Text = "";

            txtSoNhaNhapDon.Text = "";
            txtTenDuongNhapDon.Text = "";
            ddlPhuongXa.SelectedIndex = 0;
            ckDoiPhuongXa.Checked = false;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var makvpots = _kvpoDao.GetPo(_nvDao.Get(b).MAKV);

            var don = DonDangKyPo;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (txtTENKH.Text.Trim() == "") { CloseWaitingDialog(); return; }            

            if (!string.Empty.Equals(txtCMND.Text.Trim()))
            {               
                if (_ddkpoDao.GetCMND(txtCMND.Text.Trim(), makvpots.MAKVPO) != null)//kiem tra cmnd trùng.
                {
                    ShowError("Trùng số Chứng minh nhân dân.");
                    CloseWaitingDialog();
                    return;
                }               
            }

            Message msg;
            Filtered = FilteredMode.None;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.KH_DonLapDatMoiPo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // check exists
                var existed = _ddkpoDao.Get(don.MADDKPO);

                // đảm bảo không bị tình trạng lặp vô tận, quá lắm 100 lần không thể sai được
                var count = 0;
                while (existed != null && count < 100)
                {
                    txtMADDK.Text = _ddkpoDao.NewId();
                    don.MADDKPO = txtMADDK.Text.Trim();
                    existed = _ddkpoDao.Get(don.MADDKPO);
                    count++;
                }

                if (ddlPhuongXa.SelectedValue == "%")
                {
                    ShowError("Nhập phường, xã. Kiểm tra lại.");
                    CloseWaitingDialog();                    
                    return;
                }

                // default value
                don.LOAIDK = LOAIDK.DK.ToString();
                don.TTDK = TTDK.DK_A.ToString();

                // insert
                msg = _ddkpoDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBienPo(don.MADDKPO, LoginInfo.MANV, don.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                    "", "", "", "", "INDONDK");
            }
            else
            {
                if (!HasPermission(Functions.KH_DonLapDatMoi, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = _ddkpoDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
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
                upnlInfor.Update();

                // bind pager
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
                if (!HasPermission(Functions.KH_DonLapDatMoiPo, Permission.Delete))
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

        private void SetDDKToForm(DONDANGKYPO ddk)
        {
            SetControlValue(txtMADDK.ClientID, ddk.MADDKPO);
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
                        var dp = _dppoDao.GetDP(id);
                        if (dp != null)
                        {
                            txtMADP.Text = dp.MADPPO;
                            txtDUONGPHU.Text = dp.DUONGPHUPO;
                            lbTENTRAMPO.Text = dp.TENDP;
                            
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

        protected void lkKTCMND_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var makvts = _nvDao.Get(b);

                if (makvts.MAKV != "O")
                {
                    if (_ddkpoDao.DemCMND(txtCMND.Text.Trim(), _kvpoDao.GetPo(makvts.MAKV).MAKVPO) > 0)//kiem tra cmnd trùng.
                    {
                        CloseWaitingDialog();
                        ShowError("Chứng minh nhân dân bị trùng. Kiểm tra lại");
                        return;
                    }
                }
                CloseWaitingDialog();
                txtCAPNGAY.Focus();
            }
            catch { }

        }

        protected void ckDoiPhuongXa_CheckedChanged(object sender, EventArgs e)
        {
            var khuvuc = _kvpoDao.Get(ddlKHUVUC.SelectedValue);

            if (ckDoiPhuongXa.Checked)
            {  
                if (ddlKHUVUC.SelectedValue == "J")
                {
                    var xaphuong = _xpDao.GetListActive(true);

                    ddlPhuongXa.Items.Clear();
                    foreach (var xp in xaphuong)
                        ddlPhuongXa.Items.Add(new ListItem(xp.TENXA, xp.MAXA));

                    txtHUYENDCLAP.Text = " CHÂU PHÚ,AN GIANG";
                }
                else if (ddlKHUVUC.SelectedValue == "E")
                {
                    var xaphuong = _xpDao.GetListActiveKV2("N" , "U" , true); // chau phu, thoai son

                    ddlPhuongXa.Items.Clear();
                    ddlPhuongXa.Items.Add(new ListItem("-- Lựa chọn --", "%"));
                    foreach (var xp in xaphuong)
                        ddlPhuongXa.Items.Add(new ListItem(xp.TENXA, xp.MAXA + xp.MAKV));

                    txtHUYENDCLAP.Text = "";
                }
                else
                {
                    txtHUYENDCLAP.Text = "";
                }
            }
            else
            {
                PhuongXa(ddlKHUVUC.SelectedValue);

                txtHUYENDCLAP.Text = " " + khuvuc.TENKV.ToUpper() + ",AN GIANG" ;
            }

            upnlInfor.Update();
        }

        protected void ddlPhuongXa_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ddlKHUVUC.SelectedValue == "E")
                {
                    var maxaphuong = ddlPhuongXa.SelectedValue.ToString().Substring(0,4);
                    var maxaphuongkv = ddlPhuongXa.SelectedValue.ToString().Substring(4, 1);

                    var xaphuong = _xpDao.Get(maxaphuong, maxaphuongkv);
                    var khuvuc = _kvDao.Get(maxaphuongkv);


                    txtHUYENDCLAP.Text =  " " + khuvuc.TENKV.ToUpper() + ",AN GIANG";

                    upnlInfor.Update();
                }                
            }
            catch { }
        }


    }
}