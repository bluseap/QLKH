using System;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh.Power
{
    public partial class BBNghiemThuPo : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly LoaiDongHoPoDao _ldhpoDao = new LoaiDongHoPoDao();
        private readonly KhuVucPoDao _kvpodao = new KhuVucPoDao();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly BBNghiemThuPoDao _bbntpoDao = new BBNghiemThuPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly MucDichSuDungPoDao _mdsdpoDao = new MucDichSuDungPoDao();

        private readonly ThiCongDao _tcDao = new ThiCongDao();        
        private readonly CongViecDao _cvDao = new CongViecDao();
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly NhanVienDao nvdao = new NhanVienDao();

        string madon1;

        private BBNGHIEMTHUPO ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var bb = _bbntpoDao.Get(txtMADDK.Text.Trim()) ?? new BBNGHIEMTHUPO();

                bb.MABBNTPO = txtMADDK.Text.Trim();
                bb.MADDKPO = txtMADDK.Text.Trim();
                bb.MANV1 = lbNV1.Text.Trim();
                bb.HOTEN1 = txtNV1.Text.Trim();
                bb.MANV2 = lbNV2.Text.Trim();
                bb.HOTEN2 = txtNV2.Text.Trim();
                bb.MANV3 = lbNV3.Text.Trim();
                bb.HOTEN3 = txtNV3.Text.Trim();                
                bb.KHOANGCACH = !string.Empty.Equals(txtKHOANCACH.Text.Trim()) ? Convert.ToDecimal(txtKHOANCACH.Text.Trim()) : 0;                
                bb.CHINIEMM1 = txtCHIM1.Text.Trim();
                bb.CHINIEMM2 = txtCHIM2.Text.Trim();
                bb.KETLUAN = txtKETLUAN.Text.Trim();
                bb.MADHPO = txtCSDONGHO.Text.Trim();
                bb.NGAYLAPBB = !string.Empty.Equals(txtLAMBB.Text.Trim()) ? DateTimeUtil.GetVietNamDate(txtLAMBB.Text.Trim())
                        : DateTimeUtil.GetVietNamDate(DateTime.Now.ToString("dd/MM/yyyy"));
                bb.NGAYNHAP = DateTime.Now;
                bb.HETHONGCN = txtHETHONGCN.Text.Trim();
                bb.MANV = LoginInfo.MANV.ToString();

                bb.MAMDSDPO = ddlMDSD.SelectedValue;
                bb.HOTENDAIDIEN = txtHOTENDAIDIEN.Text.Trim();
                bb.CNKIEMDINH = ddlCNKIEMDINH.SelectedValue;
                bb.CNHOPDAUDAY = ddlCNHOPDAUDAY.SelectedValue;
                bb.CNHOPNHUA = ddlCNHOPNHUA.SelectedValue;
                bb.NAPDAYHOPDAUDAY = ddlNAPDAYHOPDAUDAY.SelectedValue;
                bb.TTDONGHODIEN = ddlDONGHODIEN.SelectedValue;
                bb.TTBANGGONHUA = ddlBANGGO.SelectedValue;
                bb.CHIEUCAO = !string.Empty.Equals(txtCHIEUCAO.Text.Trim()) ? Convert.ToDecimal(txtCHIEUCAO.Text.Trim()) : 0;
                bb.VITRI = txtVITRILAP.Text.Trim();
                bb.KCDENPOTEKH = !string.Empty.Equals(txtKCDENPOTEKH.Text.Trim()) ? Convert.ToDecimal(txtKCDENPOTEKH.Text.Trim()) : 0;
                bb.DOVONGONGSU = !string.Empty.Equals(txtDOVONGONGSU.Text.Trim()) ? Convert.ToDecimal(txtDOVONGONGSU.Text.Trim()) : 0;
                bb.TCCHIEUDAIDAY = !string.Empty.Equals(txtTCCHIEUDAIDAY.Text.Trim()) ? Convert.ToDecimal(txtTCCHIEUDAIDAY.Text.Trim()) : 0;
                bb.LOAICHICHAY = ddlLOAICHICHAY.SelectedValue;
                bb.LOAITRUDO = ddlLOAITRUDO.SelectedValue;
                bb.SLTRUDO = !string.Empty.Equals(txtSLTRUDO.Text.Trim()) ? Convert.ToDecimal(txtSLTRUDO.Text.Trim()) : 0;
                bb.CHIEUDAIVUOTS = !string.Empty.Equals(txtCHIEUDAIVUOTS.Text.Trim()) ? Convert.ToDecimal(txtCHIEUDAIVUOTS.Text.Trim()) : 0;
                bb.CHIEUCAOVUOTS = !string.Empty.Equals(txtCHIEUCAOVUOTS.Text.Trim()) ? Convert.ToDecimal(txtCHIEUCAOVUOTS.Text.Trim()) : 0;
                bb.CHIEUDAIVUOTD = !string.Empty.Equals(txtCHIEUDAIVUOTD.Text.Trim()) ? Convert.ToDecimal(txtCHIEUDAIVUOTD.Text.Trim()) : 0;
                bb.CHIEUCAOVUOTD = !string.Empty.Equals(txtCHIEUCAOVUOTD.Text.Trim()) ? Convert.ToDecimal(txtCHIEUCAOVUOTD.Text.Trim()) : 0;
                bb.DAYNHANHSUDUNG = txtDAYNHANHSUDUNG.Text.Trim();
                bb.TTDAYNHANH = ddlTTDAYNHANH.SelectedValue;
                bb.SLTTDAYNHANH = !string.Empty.Equals(txtSLTTDAYNHANH.Text.Trim()) ? Convert.ToDecimal(txtSLTTDAYNHANH.Text.Trim()) : 0;
                bb.DAUNOIDAYNHANHDZ = txtDAUNOIDAYNHANHDZ.Text.Trim();
                bb.SLDAUNOIDAYNHANHDZ = !string.Empty.Equals(txtSLDAUNOIDAYNHANHDZ.Text.Trim()) ? Convert.ToDecimal(txtSLDAUNOIDAYNHANHDZ.Text.Trim()) : 0;
                bb.DAUNOIDNDZBKEO = ddlDAUNOIDNDZBKEO.SelectedValue;
                bb.DAYNHANHVUOTMAI = ddlDAYNHANHVUOTMAI.SelectedValue;
                bb.KCVUOTMAINHA = !string.Empty.Equals(txtKCVUOTMAINHA.Text.Trim()) ? Convert.ToDecimal(txtKCVUOTMAINHA.Text.Trim()) : 0;
                bb.LOAIDAYHOTRUOC = txtLOAIDAYHOTRUOC.Text.Trim();
                bb.CHIEUDAIDAYTRUOC = !string.Empty.Equals(txtCHIEUDAIDAYTRUOC.Text.Trim()) ? Convert.ToDecimal(txtCHIEUDAIDAYTRUOC.Text.Trim()) : 0;
                bb.DSHOTRUOC = txtDSHOTRUOC.Text.Trim();
                bb.LOAIDAYHOSAU = txtLOAIDAYHOSAU.Text.Trim();
                bb.CHIEUDAIDAYSAU = !string.Empty.Equals(txtCHIEUDAIDAYSAU.Text.Trim()) ? Convert.ToDecimal(txtCHIEUDAIDAYSAU.Text.Trim()) : 0;              
                bb.DSHOSAU = txtDSHOSAU.Text.Trim();           

                return bb;
            }
        }

        #region Co loc, up
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
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_BBNghiemThuPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    if (lbnt.Text == "1")
                    { 
                        BinBienBan(); 
                    }
                    else 
                    { 
                        BindDataBB(); 
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
            Page.Title = Resources.Message.TITLE_TC_BBNGHIEMTHUPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_BBNGHIEMTHUPO;
            }

            CommonFunc.SetPropertiesForGrid(gvTKVT);
            //CommonFunc.SetPropertiesForGrid(gvDDK);
            //CommonFunc.SetPropertiesForGrid(gvList);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
        }

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

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        public bool ValidateData()
        {
            if (string.Empty.Equals(txtMADDK.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã cấp bậc"), txtMADDK.ClientID);
                return false;
            }
            return true;
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

                var query = nvdao.GetListKV(b);
                foreach (var a in query)
                {
                    string d = a.MAKV;
                    var kvList = _kvpodao.GetListKVPO(d);

                    ddlMaKV.Items.Clear();
                    //ddlMaKV.Items.Add(new ListItem("--Tất cả--", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }

                txtNV1.Text = "";
                txtNV2.Text = "";
                txtNV3.Text = "";
                txtCHIEUCAO.Text = "";
                txtKHOANCACH.Text = "";
                txtVITRILAP.Text = "";
                txtCHIKDM1.Text = "";
                txtCHIKDM2.Text = "";
                txtCSDONGHO.Text = "";
                txtHETHONGCN.Text = "";

                ddlMDSD.SelectedIndex = 0;
                ddlCNKIEMDINH.SelectedIndex = 0;
                ddlCNHOPDAUDAY.SelectedIndex = 0;
                ddlCNHOPNHUA.SelectedIndex = 0;
                ddlNAPDAYHOPDAUDAY.SelectedIndex = 0;
                ddlDONGHODIEN.SelectedIndex = 0;
                ddlBANGGO.SelectedIndex = 0;
                txtCHIEUCAO.Text = "0";
                txtVITRILAP.Text = "";
                txtKCDENPOTEKH.Text = "0";
                txtDOVONGONGSU.Text = "0";
                txtTCCHIEUDAIDAY.Text = "0";
                ddlLOAICHICHAY.SelectedIndex = 0;
                ddlLOAITRUDO.SelectedIndex = 0;
                txtSLTRUDO.Text = "0";
                txtCHIEUDAIVUOTS.Text = "0";
                txtCHIEUDAIVUOTD.Text = "0";
                txtCHIEUCAOVUOTS.Text = "0";
                txtCHIEUCAOVUOTD.Text = "0";
                txtDAYNHANHSUDUNG.Text = "";
                ddlTTDAYNHANH.SelectedIndex = 0;
                txtSLTTDAYNHANH.Text = "0";
                txtDAUNOIDAYNHANHDZ.Text = "";
                txtSLDAUNOIDAYNHANHDZ.Text = "0";
                ddlDAUNOIDNDZBKEO.SelectedIndex = 0;
                ddlDAYNHANHVUOTMAI.SelectedIndex = 0;
                txtKCVUOTMAINHA.Text = "0";
                txtLOAIDAYHOTRUOC.Text = "";
                txtCHIEUDAIDAYTRUOC.Text = "0";
                txtDSHOTRUOC.Text = "";
                txtLOAIDAYHOSAU.Text = "";
                txtCHIEUDAIDAYSAU.Text = "0";
                txtDSHOSAU.Text = "";


                var nv = nvdao.Get(b);
                if (nv.MAKV == "O")
                {
                    if (nv.MANV == "ctpth")
                    {
                        txtCHIM1.Text = "CT/ĐNAG";
                        txtCHIM2.Text = "Đ1/2007";
                    }
                    else
                    {
                        if (nv.MANV == "cthtq" || nv.MANV == "dnl")
                        {
                            txtCHIM1.Text = "CT/ĐNAG";
                            txtCHIM2.Text = "Đ2/2007";
                        }
                        else
                        {
                            txtCHIM1.Text = "";
                            txtCHIM2.Text = "";
                        }
                    }
                }
                else
                {
                    txtCHIM1.Text = "";
                    txtCHIM2.Text = "";
                }               

                txtKETLUAN.Text = "Điện kế hoạt động bình thường. Lắp đặt đúng qui định.";
                //txtNgayGiaoThiCong.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbKHOILUONG.Text = "";
                lbKHACHHANG.Text = "";

                var listMDSD = _mdsdpoDao.GetList();
                ddlMDSD.DataSource = listMDSD;
                ddlMDSD.DataTextField = "TENMDSD";
                ddlMDSD.DataValueField = "MAMDSDPO";
                ddlMDSD.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBrowseDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            upnlDonDangKy.Update();
            UnblockDialog("divDonDangKy");
        }

        private void BindDDK()
        {
            try
            {
                DateTime? tungay = null;
                DateTime? denngay = null;
                try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
                catch { txtTuNgay.Text = ""; }
                try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
                catch { txtDenNgay.Text = ""; }

                string b = LoginInfo.MANV;
                var pb = nvdao.GetKV(b);

                if (nvdao.Get(b).MAKV == "S")
                {
                    //var list = ddkDao.GetListDonChoThiCong(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue);            
                    var list = _ddkpoDao.GetListBienBanPBCD(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                    gvDDK.DataSource = list;
                    gvDDK.PagerInforText = list.Count.ToString();
                    gvDDK.DataBind();
                }
                else
                {
                    var list = _ddkpoDao.GetListBienBanPB(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                    gvDDK.DataSource = list;
                    gvDDK.PagerInforText = list.Count.ToString();
                    gvDDK.DataBind();
                }

            }
            catch { }
        }

        protected void gvDDK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDDK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDDK.PageIndex = e.NewPageIndex;                
                BindDDK();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDDK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = _ddkpoDao.Get(id);
                        if (obj == null)
                        {
                            ShowInfor("Không có hồ sơ thiết kế..");
                            return;
                        }
                        if (id == null || id == "")
                        {
                            ShowInfor("Không có hồ sơ thiết kế....");
                            return;
                        }

                        //txtMADDK.Text = id.ToString();

                        BindToInfor(id);
                        //LoadStaticReferences();
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");

                        //UpdateMode = Mode.Create;

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindToInfor(String madon)
        {
            try
            {
                txtMADDK.Text = madon.ToString();

                var dondk = _ddkpoDao.Get(madon.ToString());
                if (dondk != null)
                {
                    lbTENKH.Text = dondk.TENKH;
                    lblTENDP1.Text = dondk.DIACHILD;
                }

                var hd = _hdpoDao.Get(madon.ToString());
                if (hd != null)
                {
                    var dp = _dppoDao.GetDP(hd.MADPPO.ToString());
                    if (hd.MADPPO != null || hd.MADB != null)
                    {
                        lbDANHSO.Text = (hd.MADPPO + hd.MADB).ToString();
                    }
                    else
                    {
                        lbDANHSO.Text = "";
                    }

                    if (dp != null)
                    {                        
                        lblTENDP2.Text = dp.TENDP;
                    }
                    else
                    {                        
                        lblTENDP2.Text = "";
                    }
                }

                var tc = _tcDao.Get(madon.ToString());
                if (tc != null)
                {
                    var dh = _dhpoDao.Get(tc.MADH.ToString());
                    var ldh = _ldhpoDao.Get(dh.MALDHPO.ToString());
                    lbCSDAU.Text = tc.CSDAU.ToString();
                    lbMACHIM1.Text = tc.CHIKDM1.ToString();
                    lbMACHIM2.Text = tc.CHIKDM2.ToString();
                    lbKICHCO.Text = dh.CONGSUAT != null ? dh.CONGSUAT : "";
                    lbMALDH.Text = dh.MALDHPO.ToString();
                    lbNSX.Text = "";// ldh.NSX.ToString();
                    lbSONO.Text = dh.SONO.ToString();

                    txtCHIKDM1.Text = tc.CHIKDM1.ToString();
                    txtCHIKDM2.Text = tc.CHIKDM2.ToString();
                    
                }

                upnlThongTin.Update();
            }
            catch (Exception ex)
            {
                ShowInfor("Không có hồ sơ thiết kế!");
            }
        }

        private void BindBienBan(String madon)
        {
            try
            {
                txtMADDK.Text = madon.ToString();

                var dondk = _ddkpoDao.Get(madon.ToString());
                if (dondk != null)
                {
                    if (dondk.TENKH != null)
                        lbTENKH.Text = dondk.TENKH;
                }

                var hd = _hdpoDao.Get(madon.ToString());
                if (hd != null)
                {
                    if (hd.MADPPO != null || hd.MADB != null)
                        lbDANHSO.Text = (hd.MADPPO + hd.MADB).ToString();
                }

                var dp = _dppoDao.GetDP(hd.MADPPO.ToString());
                if (dp != null)
                {
                    lblTENDP1.Text = dp.TENDP;
                    lblTENDP2.Text = dp.TENDP;
                }
                else
                {
                    lblTENDP1.Text = "";
                    lblTENDP2.Text = "";
                }

                var tc = _tcDao.Get(madon.ToString());
                if (tc != null)
                {
                    if (tc.CSDAU != null)
                        lbCSDAU.Text = tc.CSDAU.ToString();
                    if (tc.CHIKDM1 != null)
                        lbMACHIM1.Text = tc.CHIKDM1.ToString();
                    if (tc.CHIKDM2 != null)
                        lbMACHIM2.Text = tc.CHIKDM2.ToString();

                    if (tc.CHIKDM1 != null)
                        txtCHIKDM1.Text = tc.CHIKDM1.ToString();
                    if (tc.CHIKDM2 != null)
                        txtCHIKDM2.Text = tc.CHIKDM2.ToString();
                }

                var dh = _dhpoDao.Get(tc.MADH.ToString());
                if (dh != null)
                {
                    if (dh.SONO != null)
                        lbSONO.Text = dh.SONO.ToString();
                }

                var ldh = _ldhpoDao.Get(dh.MALDHPO.ToString());
                if (ldh != null)
                {
                    if (ldh.KICHCO != null)
                        lbKICHCO.Text = ldh.KICHCO.ToString();
                    if (ldh.MALDHPO != null)
                        lbMALDH.Text = ldh.MALDHPO.ToString();
                    if (ldh.NSX != null)
                        lbNSX.Text = ldh.NSX.ToString();
                }

                var bb = _bbntpoDao.Get(madon);
                if (bb != null)
                {
                    //if (bb.CHIEUCAO != null)
                        //txtCHIEUCAO.Text = bb.CHIEUCAO.ToString();
                    if (bb.KHOANGCACH != null)
                        txtKHOANCACH.Text = bb.KHOANGCACH.ToString();
                    if (bb.VITRI != null)
                        txtVITRILAP.Text = bb.VITRI.ToString();
                    if (bb.CHINIEMM1 != null)
                        txtCHIM1.Text = bb.CHINIEMM1.ToString();
                    if (bb.CHINIEMM2 != null)
                        txtCHIM2.Text = bb.CHINIEMM2.ToString();
                    if (bb.KETLUAN != null)
                        txtKETLUAN.Text = bb.KETLUAN.ToString();
                    if (bb.MADHPO != null)
                        txtCSDONGHO.Text = bb.MADHPO.ToString();
                    if (bb.MANV1 != null)
                        lbNV1.Text = bb.MANV1.ToString();
                    if (bb.MANV2 != null)
                        lbNV2.Text = bb.MANV2.ToString();
                    if (bb.MANV3 != null)
                        lbNV3.Text = bb.MANV3.ToString();
                    if (bb.HOTEN1 != null)
                        txtNV1.Text = bb.HOTEN1.ToString();
                    if (bb.HOTEN2 != null)
                        txtNV2.Text = bb.HOTEN2.ToString();
                    if (bb.HOTEN3 != null)
                        txtNV3.Text = bb.HOTEN3.ToString();
                    if (bb.NGAYLAPBB != null)
                    {
                        //txtLAMBB.Text =  DateTimeUtil.GetVietNamDate(bb.NGAYLAPBB.ToString()).ToString("dd/MM/yyyy");
                        //txtLAMBB.Text = bb.NGAYLAPBB.ToString().Substring(0, 10); .ToString("dd/MM/yyyy") 
                        txtLAMBB.Text = bb.NGAYLAPBB.Value.ToString("dd/MM/yyyy");
                    }
                    else
                    {
                        txtLAMBB.Text = "";
                    }
                    if (bb.HETHONGCN != null)
                    { txtHETHONGCN.Text = bb.HETHONGCN.ToString(); }
                    else
                    {
                        txtHETHONGCN.Text = "";
                    }


                    var md = ddlMDSD.Items.FindByValue(bb.MAMDSDPO);
                    if (md != null)
                        ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(md);
                    else ddlMDSD.SelectedIndex = 0;

                    if (bb.HOTENDAIDIEN != null)
                        txtHOTENDAIDIEN.Text = bb.HOTENDAIDIEN;
                    else txtHOTENDAIDIEN.Text = "";

                    var dcm = ddlCNKIEMDINH.Items.FindByValue(bb.CNKIEMDINH);
                    if (dcm != null)
                        ddlCNKIEMDINH.SelectedIndex = ddlCNKIEMDINH.Items.IndexOf(dcm);
                    else ddlCNKIEMDINH.SelectedIndex = 0;

                    var dhd = ddlCNHOPDAUDAY.Items.FindByValue(bb.CNHOPDAUDAY);
                    if (dhd != null)
                        ddlCNHOPDAUDAY.SelectedIndex = ddlCNHOPDAUDAY.Items.IndexOf(dhd);
                    else ddlCNHOPDAUDAY.SelectedIndex = 0;

                    var dhn = ddlCNHOPNHUA.Items.FindByValue(bb.CNHOPNHUA);
                    if (dhn != null)
                        ddlCNHOPNHUA.SelectedIndex = ddlCNHOPNHUA.Items.IndexOf(dhn);
                    else ddlCNHOPNHUA.SelectedIndex = 0;
                    
                    var dhdhd = ddlNAPDAYHOPDAUDAY.Items.FindByValue(bb.NAPDAYHOPDAUDAY);
                    if (dhdhd != null)
                        ddlNAPDAYHOPDAUDAY.SelectedIndex = ddlNAPDAYHOPDAUDAY.Items.IndexOf(dhdhd);
                    else ddlNAPDAYHOPDAUDAY.SelectedIndex = 0;
                    
                    var ddhd = ddlDONGHODIEN.Items.FindByValue(bb.TTDONGHODIEN);
                    if (ddhd != null)
                        ddlDONGHODIEN.SelectedIndex = ddlDONGHODIEN.Items.IndexOf(ddhd);
                    else ddlDONGHODIEN.SelectedIndex = 0;

                    var dbg = ddlBANGGO.Items.FindByValue(bb.TTBANGGONHUA);
                    if (dbg != null)
                        ddlBANGGO.SelectedIndex = ddlBANGGO.Items.IndexOf(dbg);
                    else ddlBANGGO.SelectedIndex = 0;
                    
                    if (bb.CHIEUCAO != null)
                        txtCHIEUCAO.Text = bb.CHIEUCAO.ToString();
                    else txtCHIEUCAO.Text = "";
                    
                    if (bb.KCDENPOTEKH != null)
                        txtKCDENPOTEKH.Text = bb.KCDENPOTEKH.ToString();
                    else txtKCDENPOTEKH.Text = ""; 
                  
                    if (bb.DOVONGONGSU != null)
                        txtDOVONGONGSU.Text = bb.DOVONGONGSU.ToString();
                    else txtDOVONGONGSU.Text = "";
                    
                    if (bb.TCCHIEUDAIDAY != null)
                        txtTCCHIEUDAIDAY.Text = bb.TCCHIEUDAIDAY.ToString();
                    else txtTCCHIEUDAIDAY.Text = "";
                   
                    var dlcc = ddlLOAICHICHAY.Items.FindByValue(bb.LOAICHICHAY);
                    if (dlcc != null)
                        ddlLOAICHICHAY.SelectedIndex = ddlLOAICHICHAY.Items.IndexOf(dlcc);
                    else ddlLOAICHICHAY.SelectedIndex = 0;
                   
                    var dltd = ddlLOAITRUDO.Items.FindByValue(bb.LOAITRUDO);
                    if (dltd != null)
                        ddlLOAITRUDO.SelectedIndex = ddlLOAITRUDO.Items.IndexOf(dltd);
                    else ddlLOAITRUDO.SelectedIndex = 0;
                    
                    if (bb.SLTRUDO != null)
                        txtSLTRUDO.Text = bb.SLTRUDO.ToString();
                    else txtSLTRUDO.Text = "";
                    
                    if (bb.CHIEUDAIVUOTS != null)
                        txtCHIEUDAIVUOTS.Text = bb.CHIEUDAIVUOTS.ToString();
                    else txtCHIEUDAIVUOTS.Text = "";
                    
                    if (bb.CHIEUCAOVUOTS != null)
                        txtCHIEUCAOVUOTS.Text = bb.CHIEUCAOVUOTS.ToString();
                    else txtCHIEUCAOVUOTS.Text = "";
                    
                    if (bb.CHIEUDAIVUOTD != null)
                        txtCHIEUDAIVUOTD.Text = bb.CHIEUDAIVUOTD.ToString();
                    else txtCHIEUDAIVUOTD.Text = "";

                    if (bb.CHIEUCAOVUOTD != null)
                        txtCHIEUCAOVUOTD.Text = bb.CHIEUCAOVUOTD.ToString();
                    else txtCHIEUCAOVUOTD.Text = "";
                    
                    if (bb.DAYNHANHSUDUNG != null)
                        txtDAYNHANHSUDUNG.Text = bb.DAYNHANHSUDUNG.ToString();
                    else txtDAYNHANHSUDUNG.Text = "";
                    
                    var dttdn = ddlTTDAYNHANH.Items.FindByValue(bb.TTDAYNHANH);
                    if (dttdn != null)
                        ddlTTDAYNHANH.SelectedIndex = ddlTTDAYNHANH.Items.IndexOf(dttdn);
                    else ddlTTDAYNHANH.SelectedIndex = 0;
                    
                    if (bb.SLTTDAYNHANH != null)
                        txtSLTTDAYNHANH.Text = bb.SLTTDAYNHANH.ToString();
                    else txtSLTTDAYNHANH.Text = "";
                    
                    if (bb.DAUNOIDAYNHANHDZ != null)
                        txtDAUNOIDAYNHANHDZ.Text = bb.DAUNOIDAYNHANHDZ.ToString();
                    else txtDAUNOIDAYNHANHDZ.Text = "";
                    
                    if (bb.SLDAUNOIDAYNHANHDZ != null)
                        txtSLDAUNOIDAYNHANHDZ.Text = bb.SLDAUNOIDAYNHANHDZ.ToString();
                    else txtSLDAUNOIDAYNHANHDZ.Text = "";
                    
                    var ddnk = ddlDAUNOIDNDZBKEO.Items.FindByValue(bb.DAUNOIDNDZBKEO);
                    if (ddnk != null)
                        ddlDAUNOIDNDZBKEO.SelectedIndex = ddlDAUNOIDNDZBKEO.Items.IndexOf(ddnk);
                    else ddlDAUNOIDNDZBKEO.SelectedIndex = 0;
                    
                    var ddnvm = ddlDAYNHANHVUOTMAI.Items.FindByValue(bb.DAYNHANHVUOTMAI);
                    if (ddnvm != null)
                        ddlDAYNHANHVUOTMAI.SelectedIndex = ddlDAYNHANHVUOTMAI.Items.IndexOf(ddnvm);
                    else ddlDAYNHANHVUOTMAI.SelectedIndex = 0;
                    
                    if (bb.KCVUOTMAINHA != null)
                        txtKCVUOTMAINHA.Text = bb.KCVUOTMAINHA.ToString();
                    else txtKCVUOTMAINHA.Text = "";
                    
                    if (bb.LOAIDAYHOTRUOC != null)
                        txtLOAIDAYHOTRUOC.Text = bb.LOAIDAYHOTRUOC.ToString();
                    else txtLOAIDAYHOTRUOC.Text = "";
                    
                    if (bb.CHIEUDAIDAYTRUOC != null)
                        txtCHIEUDAIDAYTRUOC.Text = bb.CHIEUDAIDAYTRUOC.ToString();
                    else txtCHIEUDAIDAYTRUOC.Text = "";
                    
                    if (bb.DSHOTRUOC != null)
                        txtDSHOTRUOC.Text = bb.DSHOTRUOC.ToString();
                    else txtDSHOTRUOC.Text = "";
                    
                    if (bb.LOAIDAYHOSAU != null)
                        txtLOAIDAYHOSAU.Text = bb.LOAIDAYHOSAU.ToString();
                    else txtLOAIDAYHOSAU.Text = "";
                   
                    if (bb.CHIEUDAIDAYSAU != null)
                        txtCHIEUDAIDAYSAU.Text = bb.CHIEUDAIDAYSAU.ToString();
                    else txtCHIEUDAIDAYSAU.Text = "";
                    
                    if (bb.DSHOSAU != null)
                        txtDSHOSAU.Text = bb.DSHOSAU.ToString();
                    else txtDSHOSAU.Text = "";
                    

                }

                var nv1 = nvdao.Get(bb.MANV1);
                if (nv1 != null)
                {
                    var cv1 = _cvDao.Get(nv1.MACV);
                    if (cv1.TENCV != null)
                        txtCV1.Text = cv1.TENCV.ToString();
                }

                var nv2 = nvdao.Get(bb.MANV2);
                if (nv2 != null)
                {
                    var cv2 = _cvDao.Get(nv2.MACV);
                    if (cv2.TENCV != null)
                        txtCV2.Text = cv2.TENCV.ToString();
                }

                var nv3 = nvdao.Get(bb.MANV3);
                if (nv3 != null)
                {
                    var cv3 = _cvDao.Get(nv3.MACV);
                    if (cv3.TENCV != null)
                        txtCV3.Text = cv3.TENCV.ToString();
                }

                txtGHICHUBBNT.Text = bb.GHICHU != null ? bb.GHICHU : "";

                upnlThongTin.Update();
            }
            catch (Exception ex)
            {
                ShowInfor("Không có biên bản nghiệm thu!");
            }
        }

        protected void btnFilterDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var info = ItemObj;
            if (info == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.TC_BBNghiemThuPo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _bbntpoDao.Get(txtMADDK.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Biên bản đã tồn tại", txtMADDK.ClientID);
                    return;
                }

                if (txtLAMBB.Text.Trim() == "")
                {
                    CloseWaitingDialog();
                    ShowError("Nhập ngày lập biên bản.", txtLAMBB.ClientID);
                    return;
                }

                info.MANV = LoginInfo.MANV.ToString();
                info.GHICHU = txtGHICHUBBNT.Text.Trim();

                msg = _bbntpoDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

                _rpDao.HisNgayDangKyBienPo(info.MADDKPO, LoginInfo.MANV, _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                           "", "", "", "", "INBBNT");
            }
            // update
            else
            {
                if (!HasPermission(Functions.TC_BBNghiemThuPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (txtLAMBB.Text.Trim() == "")
                {
                    CloseWaitingDialog();
                    ShowError("Nhập ngày lập biên bản.");
                    return;
                }

                if (string.IsNullOrEmpty(lbMADDTRAHSTK.Text.Trim()) || lbMADDTRAHSTK.Text == "")
                {
                    info.MANV = LoginInfo.MANV.ToString();
                    info.GHICHU = txtGHICHUBBNT.Text.Trim();

                    msg = _bbntpoDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

                    _rpDao.HisNgayDangKyBienPo(info.MADDKPO, LoginInfo.MANV, _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                               "", "", "", "", "UPBBNT");
                }
                else
                {
                    info.MANV = LoginInfo.MANV.ToString();
                    info.GHICHU = txtGHICHUBBNT.Text.Trim();

                    msg = _bbntpoDao.UpdateNTToTK(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    //_rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

                    _rpDao.HisNgayDangKyBienPo(info.MADDKPO, LoginInfo.MANV, _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                               "", "", "", "", "UPBBNT");
                }
            }

            lbMADDTRAHSTK.Text = "";

            BindDataBB();
            LoadStaticReferences();

            upnlGrid.Update();
            upnlThongTin.Update();
            CloseWaitingDialog();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            lbnt.Text = "0";
            BindDataBB();
            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void gvNhanVien_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNhanVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMANV":
                        var nv = nvdao.Get(id);
                        if (nv != null)
                        {
                            //SetControlValue(txtMANV.ClientID, nv.MANV);
                            //SetControlValue(txtTENNV.ClientID, nv.HOTEN);

                            //txtMANV.Text = nv.MANV;
                            //txtTENNV.Text = nv.HOTEN;
                            //txtMANV.Focus();
                            lbNV1.Text = id.ToString();
                            txtNV1.Text = nv.HOTEN.ToString();

                            var cv = _cvDao.Get(nv.MACV.ToString());
                            txtCV1.Text = cv.TENCV.ToString();

                            upnlThongTin.Update();
                        }
                        HideDialog("divNhanVien");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNhanVien_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                gvNhanVien.PageIndex = e.NewPageIndex;                
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindNhanVien()
        {
            string makv = nvdao.Get(LoginInfo.MANV).MAKV;

            if (makv == "P" || makv == "T")
            {
                var list = nvdao.SearchKV_GD(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
            else
            {
                var list = nvdao.SearchKV3(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }

            upnlNhanVien.Update();
        }

        protected void btnBrowseNhanVien_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            upnlNhanVien.Update();
            UnblockDialog("divNhanVien");
        }

        protected void btnBrowseNhanVien2_Click(object sender, EventArgs e)
        {
            BindNhanVien2();
            upnlNhanVien2.Update();
            UnblockDialog("divNhanVien2");
        }

        protected void btnBrowseNhanVien3_Click(object sender, EventArgs e)
        {
            BindNhanVien3();
            upnlNhanVien3.Update();
            UnblockDialog("divNhanVien3");
        }

        protected void btnFilterNV3_Click(object sender, EventArgs e)
        {
            BindNhanVien3();
            CloseWaitingDialog();
        }

        protected void btnFilterNV2_Click(object sender, EventArgs e)
        {
            BindNhanVien2();
            CloseWaitingDialog();
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        protected void gvNhanVien2_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNhanVien2_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMANV":
                        var nv = nvdao.Get(id);
                        if (nv != null)
                        {
                            //SetControlValue(txtMANV.ClientID, nv.MANV);
                            //SetControlValue(txtTENNV.ClientID, nv.HOTEN);

                            //txtMANV.Text = nv.MANV;
                            //txtTENNV.Text = nv.HOTEN;
                            //txtMANV.Focus();
                            lbNV2.Text = id.ToString();
                            txtNV2.Text = nv.HOTEN.ToString();

                            var cv = _cvDao.Get(nv.MACV.ToString());
                            txtCV2.Text = cv.TENCV.ToString();

                            upnlThongTin.Update();
                        }
                        HideDialog("divNhanVien2");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNhanVien2_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvNhanVien2.PageIndex = e.NewPageIndex;              
                BindNhanVien2();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindNhanVien2()
        {
            string makv = nvdao.Get(LoginInfo.MANV).MAKV;

            if (makv == "P" || makv == "T")            
            {
                var list = nvdao.TimNVKyThuat(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                gvNhanVien2.DataSource = list;
                gvNhanVien2.PagerInforText = list.Count.ToString();
                gvNhanVien2.DataBind();      
            }
            else
            {
                var list = nvdao.SearchKV3(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien2.DataSource = list;
                gvNhanVien2.PagerInforText = list.Count.ToString();
                gvNhanVien2.DataBind();
            }

            upnlNhanVien2.Update();
        }

        private void BindNhanVien3()
        {
            string makv = nvdao.Get(LoginInfo.MANV).MAKV;

            var list = nvdao.SearchKV3(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien3.DataSource = list;
            gvNhanVien3.PagerInforText = list.Count.ToString();
            gvNhanVien3.DataBind();

            //if (makv == "P" || makv == "T")
            //{
            //    var list = nvdao.TimNV_NM_To(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

            //    gvNhanVien3.DataSource = list;
            //    gvNhanVien3.PagerInforText = list.Count.ToString();
            //    gvNhanVien3.DataBind();
            //}
            //else
            //{
            //    var list = nvdao.SearchKV3(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
            //    gvNhanVien3.DataSource = list;
            //    gvNhanVien3.PagerInforText = list.Count.ToString();
            //    gvNhanVien3.DataBind();
            //}

            upnlNhanVien3.Update();
        }

        protected void gvNhanVien3_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNhanVien3_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMANV":
                        var nv = nvdao.Get(id);
                        if (nv != null)
                        {
                            //SetControlValue(txtMANV.ClientID, nv.MANV);
                            //SetControlValue(txtTENNV.ClientID, nv.HOTEN);

                            //txtMANV.Text = nv.MANV;
                            //txtTENNV.Text = nv.HOTEN;
                            //txtMANV.Focus();
                            lbNV3.Text = id.ToString();
                            txtNV3.Text = nv.HOTEN.ToString();

                            var cv = _cvDao.Get(nv.MACV.ToString());
                            txtCV3.Text = cv.TENCV.ToString();

                            upnlThongTin.Update();
                        }
                        HideDialog("divNhanVien3");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNhanVien3_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvNhanVien3.PageIndex = e.NewPageIndex;                
                BindNhanVien3();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDataBB()
        {
            var makvpo = _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO;            

            if (string.IsNullOrEmpty(txtTIMKHBB.Text.Trim()) || txtTIMKHBB.Text == "")
            {
                var objList = _bbntpoDao.GetListKV(makvpo.ToString());

                gvBienBan.DataSource = objList;
                gvBienBan.PagerInforText = objList.Count.ToString();
                gvBienBan.DataBind();
            }
            else
            {
                var objList = _bbntpoDao.GetListKVTenMaDon(makvpo, txtTIMKHBB.Text.Trim());

                gvBienBan.DataSource = objList;
                gvBienBan.PagerInforText = objList.Count.ToString();
                gvBienBan.DataBind();
            }

            upnlGrid.Update();
        }

        private void BinBienBan()
        {
            DateTime? fromDate = null;
            DateTime? toDate = null;

            // ReSharper disable EmptyGeneralCatchClause
            try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
            catch { }
            try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
            catch { }


            //var objList = _bbntpoDao.GetList(fromDate, toDate);
            var makvpo = _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO;
            var objList = _bbntpoDao.GetListKV(fromDate, toDate, makvpo);

            gvBienBan.DataSource = objList;
            gvBienBan.PagerInforText = objList.Count.ToString();
            gvBienBan.DataBind();

            //var tungay = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim());
            //var denngay = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim());
            //var sumkl = _rpDao.SumKhoiLuongBBPo(tungay, denngay);

            //var ds = sumkl.Tables[0];
            //foreach (DataRow row in ds.Rows)
            //{
            //    lbKHOILUONG.Text = row["SUMKL"].ToString();
            //}

            lbKHACHHANG.Text = objList.Count.ToString();
        }


        protected void gvBienBan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {              
                gvBienBan.PageIndex = e.NewPageIndex;
               
                if (lbnt.Text == "1")
                { BinBienBan(); }
                else { BindDataBB(); }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvBienBan_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(id))
                        {
                            var objDb = _bbntpoDao.Get(id);
                            if (objDb != null)
                            {
                                BindBienBan(id);
                                UpdateMode = Mode.Update;
                            }
                            else
                            {
                                ShowInfor("Không có biên bản..");
                            }
                        }
                        CloseWaitingDialog();

                        break;

                    case "TraHSVeTK":
                        if (!string.Empty.Equals(id))
                        {
                            var objDb = _bbntpoDao.Get(id);
                            if (objDb != null)
                            {
                                lbMADDTRAHSTK.Text = id;

                                BindBienBan(id);
                                UpdateMode = Mode.Update;

                                upnlThongTin.Update();
                            }
                            else
                            {
                                ShowInfor("Không có biên bản..");
                            }
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

        protected void gvBienBan_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvTKVT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                gvTKVT.PageIndex = e.NewPageIndex;
                BindTKVT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindTKVT()
        {
            var list = _cttkDao.GetList(txtMADDK.Text.ToString());

            if (list != null)
            {          
                gvTKVT.DataSource = list;
                gvTKVT.PagerInforText = list.Count.ToString();
                gvTKVT.DataBind();
            }            
        }

        protected void lkCTVT_Click(object sender, EventArgs e)
        {
            try
            {
                var madon = txtMADDK.Text.ToString();

                if (!string.Empty.Equals(madon))
                {
                    madon1 = madon;
                    lbTENKH.Text = _ddkpoDao.Get(madon1).TENKH;

                    BindTKVT();

                    upnlThietKeVatTu.Update();
                    upnlThongTin.Update();

                    UnblockDialog("divThietKeVatTu");
                }
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnLocBB_Click(object sender, EventArgs e)
        {
            lbnt.Text = "1";
            BinBienBan();
            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void gvBienBan_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        

       
    }
}