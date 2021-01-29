using System;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Web.Common;

using CrystalDecisions.CrystalReports.Engine;
using EOSCRM.Util;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;

using System.IO;
using System.Web.UI;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh
{
    public partial class BienBanNghiemThu : Authentication
    {
        private readonly StoredProcedureDao _spDao = new StoredProcedureDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly NhanVienDao nvdao = new NhanVienDao();
        private readonly KhuVucDao kvdao = new KhuVucDao();
        private readonly HopDongDao _hdDao = new HopDongDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly DongHoDao _dhDao = new DongHoDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly LoaiDongHoDao _ldhDao = new LoaiDongHoDao();
        private readonly CongViecDao _cvDao = new CongViecDao();
        private readonly BBNghiemThuDao _bbntDao = new BBNghiemThuDao();
        private readonly ThietKeDao _tkDao = new ThietKeDao();
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();
        private readonly ReportClass _rpDao = new ReportClass();

        string madon1;

        private BBNGHIEMTHU ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var bb = _bbntDao.Get(txtMADDK.Text.Trim()) ?? new BBNGHIEMTHU();

                bb.MABBNT = txtMADDK.Text.Trim();
                bb.MADDK = txtMADDK.Text.Trim();
                bb.MANV1 = lbNV1.Text.Trim();
                bb.HOTEN1 = txtNV1.Text.Trim();
                bb.MANV2 = lbNV2.Text.Trim();
                bb.HOTEN2 = txtNV2.Text.Trim();
                bb.MANV3 = lbNV3.Text.Trim();
                bb.HOTEN3 = txtNV3.Text.Trim();
                bb.CHIEUCAO = (txtCHIEUCAO.Text.Trim() != "") ? Convert.ToDecimal(txtCHIEUCAO.Text.Trim()) : 0;
                bb.KHOANGCACH = (txtKHOANCACH.Text.Trim() != "") ? Convert.ToDecimal(txtKHOANCACH.Text.Trim()) : 0;
                bb.VITRI = txtVITRILAP.Text.Trim();
                bb.CHINIEMM1 = txtCHIM1.Text.Trim();
                bb.CHINIEMM2 = txtCHIM2.Text.Trim();
                bb.KETLUAN = txtKETLUAN.Text.Trim();
                bb.MADH = txtCSDONGHO.Text.Trim();
                bb.NGAYLAPBB = (txtLAMBB.Text.Trim() != "") ? DateTimeUtil.GetVietNamDate(txtLAMBB.Text.Trim()) : DateTime.Now;
                bb.NGAYNHAP = DateTime.Now;

                bb.HETHONGCN = txtHETHONGCN.Text.Trim();
                bb.MANV = LoginInfo.MANV.ToString();
                bb.NGAYNHANHSTC = (txtNGAYNHANHS.Text.Trim() != "") ? DateTimeUtil.GetVietNamDate(txtNGAYNHANHS.Text.Trim())
                        : (DateTime?)null;

                bb.NGAYCHUYENHSKTOAN = (txtNGAYCHUYENHS.Text.Trim() != "") ? DateTimeUtil.GetVietNamDate(txtNGAYCHUYENHS.Text.Trim())
                        : (DateTime?)null; //: DateTimeUtil.GetVietNamDate(DateTime.Now.ToString("dd/MM/yyyy"));
                return bb;
            }
        }

        #region loc, up
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

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((EOS)Page.Master).SetReadonly(id, isReadonly);
        }

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

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_BienBanNghiemThu, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();

                    if (lbnt.Text == "1")
                    { BinBienBan(); }
                    else { BindDataBB(); }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_BIENBANNGHIEMTHU;

            var header = (UserControls.Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_BIENBANNGHIEMTHU;
            }

            CommonFunc.SetPropertiesForGrid(gvTKVT);
            CommonFunc.SetPropertiesForGrid(gvBienBan);
            CommonFunc.SetPropertiesForGrid(gvMauNhanVienChiTiet);
        }

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
                    var kvList = kvdao.GetListKV(d);

                    ddlMaKV.Items.Clear();
                    //ddlMaKV.Items.Add(new ListItem("--Tất cả--", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }

                txtNV1.Text = "";
                txtNV2.Text = "";
                txtNV3.Text = "";
                txtCHIEUCAO.Text = "";
                txtKHOANCACH.Text = "";
                txtVITRILAP.Text = "";
                //txtCHIKDM1.Text = "";
                //txtCHIKDM2.Text = "";
                txtCSDONGHO.Text = "";
                //txtHETHONGCN.Text = "";

                var nv = nvdao.Get(b);
                if (nv.MAKV == "O")
                {
                    if (nv.MANV == "ctpth")
                    {
                        txtCHIM1.Text = "N2/2018"; //"CT/ĐNAG";
                        txtCHIM2.Text = "CT/ĐNAG"; //"N2/2007";
                    }
                    else if (nv.MANV == "cthtq")
                    {
                        txtCHIM1.Text = "N1/2018"; //"E2N/ĐNAG"; //"CT/ĐNAG";
                        txtCHIM2.Text = "CT/ĐNAG"; //"CT/2001"; //"N1/2007";
                    }
                    else if (nv.MANV == "dnl")
                    {
                        txtCHIM1.Text = "N3/2018"; //"E2N/ĐNAG"; //"CT/ĐNAG";
                        txtCHIM2.Text = "CT/ĐNAG"; //"CT/2001"; //"N1/2007";
                    }
                    else
                    {
                        txtCHIM1.Text = "";
                        txtCHIM2.Text = "";
                    }

                    //nguyễn thị thanh thuy
                    lbNV1.Text = "nttt";
                    txtNV1.Text = nvdao.Get("nttt").HOTEN.ToString();
                    var cv = _cvDao.Get(nvdao.Get("nttt").MACV.ToString());
                    txtCV1.Text = cv.TENCV.ToString();

                }
                else
                {
                    if (nv.MAKV == "X")
                    {
                        txtLAMBB.Text = DateTime.Now.ToString("dd/MM/yyyy");
                        txtCHIM1.Text = "LX-ĐNAG";
                    }
                    else
                    {
                        txtCHIM1.Text = "";
                        txtCHIM2.Text = "";
                    }
                }

                //txtMADDK.Text = "";

                txtKETLUAN.Text = "Đồng hồ nước hoạt động bình thường. Lắp đặt đúng qui định.";
                //txtNgayGiaoThiCong.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbKHOILUONG.Text = "";
                lbKHACHHANG.Text = "";

                //txtNGAYCHUYENHS.Text = DateTime.Now.ToString("dd/MM/yyyy");

                var khuvuc = nvdao.GetKV(b);
                // 1 : Nuoc ; 2 : Dien
                loadMauNhanVien(khuvuc.MAKV, 1);

                txtSortOrderMauNhanVien.Text = "1";
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
                var list = ddkDao.GetListBienBanPBCD(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                gvDDK.DataSource = list;
                gvDDK.PagerInforText = list.Count.ToString();
                gvDDK.DataBind();
            }
            else
            {
                var list = ddkDao.GetListBienBanPB(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                gvDDK.DataSource = list;
                gvDDK.PagerInforText = list.Count.ToString();
                gvDDK.DataBind();
            }
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
                        var obj = ddkDao.Get(id);
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
                        LoadStaticReferences();
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

                var dondk = ddkDao.Get(madon.ToString());
                if (dondk != null)
                {
                    lbTENKH.Text = dondk.TENKH;
                }

                var hd = _hdDao.Get(madon.ToString());
                if (hd != null)
                {
                    var dp = _dpDao.GetDP(hd.MADP.ToString());
                    if (hd.MADP != null || hd.MADB != null)
                    {
                        lbDANHSO.Text = (hd.MADP + hd.MADB).ToString();
                    }
                    else {
                        lbDANHSO.Text = "";
                    }

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
                }

                var tc = _tcDao.Get(madon.ToString());
                if (tc != null)
                {
                    var dh = _dhDao.Get(tc.MADH.ToString());
                    var ldh = _ldhDao.Get(dh.MALDH.ToString());
                    lbCSDAU.Text = tc.CSDAU != null ? tc.CSDAU.ToString() : "";
                    lbMACHIM1.Text = tc.CHIKDM1 != null ? tc.CHIKDM1.ToString() : "";
                    lbMACHIM2.Text = tc.CHIKDM2 != null ? tc.CHIKDM2.ToString() : "";
                    lbKICHCO.Text = ldh.KICHCO != null ? ldh.KICHCO.ToString() : "";
                    lbMALDH.Text = ldh.MALDH != null ? ldh.MALDH.ToString() : "";
                    lbNSX.Text = ldh.NSX != null ? ldh.NSX.ToString() : "";
                    lbSONO.Text = dh.SONO != null ? dh.SONO.ToString() : "";

                    txtCHIKDM1.Text = tc.CHIKDM1 != null ? tc.CHIKDM1.ToString() : "";
                    txtCHIKDM2.Text = tc.CHIKDM2 != null ? tc.CHIKDM2.ToString() : "";
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

                var dondk = ddkDao.Get(madon.ToString());
                if (dondk != null)
                {
                    if (dondk.TENKH != null)
                        lbTENKH.Text = dondk.TENKH;
                }

                var hd = _hdDao.Get(madon.ToString());
                if (hd != null)
                {
                    if (hd.MADP != null || hd.MADB != null)
                        lbDANHSO.Text = (hd.MADP + hd.MADB).ToString();
                }

                var dp = _dpDao.GetDP(hd.MADP.ToString());
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

                var dh = _dhDao.Get(tc.MADH.ToString());
                if (dh != null)
                {
                    if (dh.SONO != null)
                        lbSONO.Text = dh.SONO.ToString();
                }

                var ldh = _ldhDao.Get(dh.MALDH.ToString());
                if (ldh != null)
                {
                    if (ldh.KICHCO != null)
                        lbKICHCO.Text = ldh.KICHCO.ToString();
                    if (ldh.MALDH != null)
                        lbMALDH.Text = ldh.MALDH.ToString();
                    if (ldh.NSX != null)
                        lbNSX.Text = ldh.NSX.ToString();
                }

                var bb = _bbntDao.Get(madon);
                if (bb != null)
                {
                    if (bb.CHIEUCAO != null)
                        txtCHIEUCAO.Text = bb.CHIEUCAO.ToString();
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
                    if (bb.MADH != null)
                        txtCSDONGHO.Text = bb.MADH.ToString();
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
                    else {
                        txtLAMBB.Text = "";
                    }
                    if (bb.HETHONGCN != null)
                    { txtHETHONGCN.Text = bb.HETHONGCN.ToString(); }
                    else
                    {
                        txtHETHONGCN.Text = "";
                    }
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

                if (bb.NGAYNHANHSTC != null)
                {
                    txtNGAYNHANHS.Text = bb.NGAYNHANHSTC.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtNGAYNHANHS.Text = "";
                }

                if (bb.NGAYCHUYENHSKTOAN != null)
                {
                    txtNGAYCHUYENHS.Text = bb.NGAYCHUYENHSKTOAN.Value.ToString("dd/MM/yyyy");
                }
                else
                {
                    txtNGAYCHUYENHS.Text = "";
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
                if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _bbntDao.Get(txtMADDK.Text.Trim());
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

                if (nvdao.Get(LoginInfo.MANV).MAKV == "X")
                {
                    if (txtNGAYNHANHS.Text.Trim() == "")
                    {
                        CloseWaitingDialog();
                        ShowError("Nhập ngày nhận hồ sơ. Kiểm tra lại", txtNGAYNHANHS.ClientID);
                        return;
                    }

                    info.NGAYNHANHSTC = txtNGAYNHANHS.Text.Trim() != "" ? DateTimeUtil.GetVietNamDate(txtNGAYNHANHS.Text.Trim())
                        : DateTimeUtil.GetVietNamDate(DateTime.Now.ToString("dd/MM/yyyy"));
                }

                msg = _bbntDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

                _rpDao.HisNgayDangKyBien(info.MADDK, LoginInfo.MANV, nvdao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                           "", "", "", "", "INBBNT");
            }
            // update
            else
            {
                if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Update))
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

                if (_khDao.GetMADDK(info.MADDK) != null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng đã khai thác. Kiểm tra lại.");
                    return;
                }

                if (string.IsNullOrEmpty(lbMADDTRAHSTK.Text.Trim()) || lbMADDTRAHSTK.Text == "")
                {
                    info.MANV = LoginInfo.MANV.ToString();
                    info.GHICHU = txtGHICHUBBNT.Text.Trim();

                    msg = _bbntDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

                    _rpDao.HisNgayDangKyBien(info.MADDK, LoginInfo.MANV, nvdao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                               "", "", "", "", "UPBBNT");
                }
                else
                {
                    info.MANV = LoginInfo.MANV.ToString();
                    info.GHICHU = txtGHICHUBBNT.Text.Trim();

                    msg = _bbntDao.UpdateNTToTK(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    //_rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

                    _rpDao.HisNgayDangKyBien(info.MADDK, LoginInfo.MANV, nvdao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                               "", "", "", "", "UPBBNT");
                }
            }

            upnlGrid.Update();
            CloseWaitingDialog();

            txtCHIKDM1.Text = "";
            txtCHIKDM2.Text = "";

            lbMADDTRAHSTK.Text = "";

            BindDataBB();

            upnlThongTin.Update();
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
                // Update page index
                gvNhanVien.PageIndex = e.NewPageIndex;
                // Bind data for grid
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

            if (makv == "S" || makv == "P" || makv == "T")
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
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                BindNhanVien();
                upnlNhanVien.Update();
                UnblockDialog("divNhanVien");
            }
            catch { }
        }

        protected void btnBrowseNhanVien2_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

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
                // Update page index
                gvNhanVien2.PageIndex = e.NewPageIndex;

                // Bind data for grid
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

            if (makv == "S")
            {
                var list = nvdao.SearchTP_PT(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                gvNhanVien2.DataSource = list;
                gvNhanVien2.PagerInforText = list.Count.ToString();
                gvNhanVien2.DataBind();
            }
            else
            {
                if (nvdao.Get(LoginInfo.MANV).MAKV.Equals("U") || nvdao.Get(LoginInfo.MANV).MAKV.Equals("N")
                        || nvdao.Get(LoginInfo.MANV).MAKV.Equals("X"))
                {
                    var list = nvdao.SearchKV3(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                    gvNhanVien2.DataSource = list;
                    gvNhanVien2.PagerInforText = list.Count.ToString();
                    gvNhanVien2.DataBind();
                }
                else if (nvdao.Get(LoginInfo.MANV).MAKV.Equals("P"))
                {
                    var list = nvdao.SearchKV3(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                    gvNhanVien2.DataSource = list;
                    gvNhanVien2.PagerInforText = list.Count.ToString();
                    gvNhanVien2.DataBind();
                }
                else
                {
                    //var list = nvdao.SearchKV3(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);                
                    var list = nvdao.TimNVKyThuat(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                    //var list = nvdao.TimNV_NM_To(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                    gvNhanVien2.DataSource = list;
                    gvNhanVien2.PagerInforText = list.Count.ToString();
                    gvNhanVien2.DataBind();
                }
            }

            upnlNhanVien2.Update();
        }

        private void BindNhanVien3()
        {
            string makv = nvdao.Get(LoginInfo.MANV).MAKV;

            if (makv == "S")//|| makv == "P" || makv == "T")
            {
                var list = nvdao.SearchKV3(txtKeywordNV3.Text.Trim(), makv, nvdao.Get(LoginInfo.MANV).MAPB);

                gvNhanVien3.DataSource = list;
                gvNhanVien3.PagerInforText = list.Count.ToString();
                gvNhanVien3.DataBind();
            }
            else
            {
                if (nvdao.Get(LoginInfo.MANV).MAKV.Equals("U") || nvdao.Get(LoginInfo.MANV).MAKV.Equals("N")
                        || nvdao.Get(LoginInfo.MANV).MAKV.Equals("X") || makv == "P" || makv == "T"
                    || makv == "K" || makv == "L" || makv == "M" || makv == "Q")
                {
                    var list = nvdao.SearchKV3(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                    gvNhanVien3.DataSource = list;
                    gvNhanVien3.PagerInforText = list.Count.ToString();
                    gvNhanVien3.DataBind();
                }
                else
                {
                    var list = nvdao.TimNV_NM_To(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);

                    gvNhanVien3.DataSource = list;
                    gvNhanVien3.PagerInforText = list.Count.ToString();
                    gvNhanVien3.DataBind();
                }
            }

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
                // Update page index
                gvNhanVien3.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindNhanVien3();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDataBB()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (string.IsNullOrEmpty(txtTIMKHBB.Text.Trim()) || txtTIMKHBB.Text == "")
                {
                    var objList = _bbntDao.GetListKV(nvdao.Get(b).MAKV);

                    gvBienBan.DataSource = objList;
                    gvBienBan.PagerInforText = objList.Count.ToString();
                    gvBienBan.DataBind();
                }
                else
                {
                    var objList = _bbntDao.GetListKVTenMaDon(nvdao.Get(b).MAKV, txtTIMKHBB.Text.Trim());

                    gvBienBan.DataSource = objList;
                    gvBienBan.PagerInforText = objList.Count.ToString();
                    gvBienBan.DataBind();
                }

                upnlGrid.Update();
            }
            catch { }
        }

        private void BinBienBan()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                DateTime? fromDate = null;
                DateTime? toDate = null;

                // ReSharper disable EmptyGeneralCatchClause
                try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
                catch { }
                try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
                catch { }

                if (_nvDao.Get(b).MAKV == "X")
                {
                    var objList = _bbntDao.GetListKVLX(fromDate, toDate, nvdao.Get(b).MAKV);
                    gvBienBan.DataSource = objList;
                    gvBienBan.PagerInforText = objList.Count.ToString();
                    gvBienBan.DataBind();

                    lbKHACHHANG.Text = objList.Count.ToString();
                }
                else
                {
                    var objList = _bbntDao.GetListKV(fromDate, toDate, nvdao.Get(b).MAKV);
                    gvBienBan.DataSource = objList;
                    gvBienBan.PagerInforText = objList.Count.ToString();
                    gvBienBan.DataBind();

                    lbKHACHHANG.Text = objList.Count.ToString();
                }

                //var tungay = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim());
                //var denngay = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); 
                //var sumkl = _rpDao.SumKhoiLuongBB(tungay, denngay);

                //var ds = sumkl.Tables[0];
                //foreach (DataRow row in ds.Rows)
                //{
                //    lbKHOILUONG.Text = row["SUMKL"].ToString();
                //}  

                CloseWaitingDialog();
            }
            catch { }
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
                            var objDb = _bbntDao.Get(id);
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
                            var objDb = _bbntDao.Get(id);
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

            //lbTENKH.Text = ddkDao.Get(madon1).TENKH;
            //lbTENKH2.Text = ddkDao.Get(madon1).TENKH;

            gvTKVT.DataSource = list;
            gvTKVT.PagerInforText = list.Count.ToString();
            gvTKVT.DataBind();

            upnlThietKeVatTu.Update();
        }

        protected void lkCTVT_Click(object sender, EventArgs e)
        {
            try
            {
                var madon = txtMADDK.Text.ToString();

                if (!string.Empty.Equals(madon))
                {
                    madon1 = madon;
                    BindTKVT();

                    lbTENKH.Text = ddkDao.Get(madon1).TENKH;
                    lbTENKH2.Text = ddkDao.Get(madon1).TENKH;

                    upnlThietKeVatTu.Update();
                    UnblockDialog("divThietKeVatTu");
                }
                else
                {
                    ShowError("Chọn khách hàng cần xem vật tư.");
                    CloseWaitingDialog();
                    return;
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

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]

        public static string[] GetHeThongCN(string prefixText)
        {
            string sql = "select HETHONGCN from BBNGHIEMTHU Where HETHONGCN like @prefixText group by HETHONGCN ";
            SqlDataAdapter da = new SqlDataAdapter(sql, Connectionstring);
            da.SelectCommand.Parameters.Add("@prefixText", SqlDbType.NVarChar, 50).Value = prefixText + "%";
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["HETHONGCN"].ToString(), i);
                i++;
            }
            return items;
        }

        [System.Web.Services.WebMethodAttribute(), System.Web.Script.Services.ScriptMethodAttribute()]
        public static string[] GetViTriLap(string prefixText)
        {
            string sql = "select VITRI from BBNGHIEMTHU Where VITRI like @prefixText group by VITRI ";
            SqlDataAdapter da = new SqlDataAdapter(sql, Connectionstring);
            da.SelectCommand.Parameters.Add("@prefixText", SqlDbType.NVarChar, 50).Value = prefixText + "%";
            DataTable dt = new DataTable();
            da.Fill(dt);
            string[] items = new string[dt.Rows.Count];
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                items.SetValue(dr["VITRI"].ToString(), i);
                i++;
            }
            return items;
        }

        protected void btSAVNGAYCHUYEN_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                DateTime? fromDate = null;
                DateTime? toDate = null;

                // ReSharper disable EmptyGeneralCatchClause
                try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
                catch { }
                try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
                catch { }

                if (string.IsNullOrEmpty(txtNGAYCHUYENHS.Text.Trim()) || txtNGAYCHUYENHS.Text == "")
                {
                    ShowError("Nhập ngày chuyển hồ sơ. Kiểm tra lại.");
                    CloseWaitingDialog();
                    return;
                }

                //var objList = _bbntDao.GetList(fromDate, toDate);            
                var objList = _bbntDao.GetListKVLX(fromDate, toDate, nvdao.Get(b).MAKV);

                var msg = _bbntDao.UpdateChuyenHS_KT(objList, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV,
                        DateTimeUtil.GetVietNamDate(txtNGAYCHUYENHS.Text.Trim()));

                if (msg == null) return;
                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    BinBienBan();

                    txtNGAYCHUYENHS.Text = "";

                    upnlThongTin.Update();
                    upnlGrid.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btXuatExcel_Click(object sender, EventArgs e)
        {
            XuatExcelDSNgayChuyen();

            BinBienBan();

            CloseWaitingDialog();
            upnlThongTin.Update();
            upnlGrid.Update();
        }

        private void XuatExcelDSNgayChuyen()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                string tungayNgayThang = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()).Day.ToString() + DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()).Month.ToString();
                string denngayNgayThangNam = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()).Day.ToString() + DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()).Month.ToString() + DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()).Year.ToString();

                var ds = _rpDao.DSQuiTrinhNuocBien(DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()), DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()),
                            nvdao.Get(b).MAKV, "", "", "", "DSBBNTCHUYENKT");

                DataTable dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=NTCHUYEN" + tungayNgayThang + "_" + denngayNgayThangNam + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";

                Response.ContentEncoding = System.Text.Encoding.UTF8;

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                BinBienBan();
                upnlThongTin.Update();
                upnlGrid.Update();
            }
            catch { }
        }

        protected void btExcelNhanHS_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                string tungayNgayThang = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()).Day.ToString() + DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()).Month.ToString();
                string denngayNgayThangNam = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()).Day.ToString() + DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()).Month.ToString() + DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()).Year.ToString();

                var ds = _rpDao.DSQuiTrinhNuocBien(DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()), DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()),
                            nvdao.Get(b).MAKV, "", "", "", "DSBBNTNHANHSTC");

                DataTable dt = ds.Tables[0];

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=NTNHAN" + tungayNgayThang + "_" + denngayNgayThangNam + ".xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";

                Response.ContentEncoding = System.Text.Encoding.UTF8;

                System.IO.StringWriter sw = new System.IO.StringWriter();
                System.Web.UI.HtmlTextWriter hw = new System.Web.UI.HtmlTextWriter(sw);

                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");

                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                Response.Write(style);
                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                BinBienBan();
                upnlThongTin.Update();
                upnlGrid.Update();
            }
            catch { }
        }

        private void loadMauNhanVien(string makv, int serviceid)
        {
            try
            {
                var query = _spDao.Get_MauNhanVien_ByMakvService(makv, serviceid);

                ddlMauNhanVienTao.Items.Clear();
                ddlChonMauNhanVien.Items.Clear();
                ddlMauNhanVienTao.Items.Add(new System.Web.UI.WebControls.ListItem("-- Chọn mẫu --", "0"));
                ddlChonMauNhanVien.Items.Add(new System.Web.UI.WebControls.ListItem("-- Chọn mẫu --", "0"));

                if (query.Tables[0].Rows.Count != 0)
                {
                    for (int i = 0; i < query.Tables[0].Rows.Count; i++)
                    {
                        ddlMauNhanVienTao.Items.Add(new System.Web.UI.WebControls.ListItem(query.Tables[0].Rows[i]["TenMauNhanVien"].ToString(),
                            query.Tables[0].Rows[i]["Id"].ToString()));

                        ddlChonMauNhanVien.Items.Add(new System.Web.UI.WebControls.ListItem(query.Tables[0].Rows[i]["TenMauNhanVien"].ToString(),
                            query.Tables[0].Rows[i]["Id"].ToString()));
                    }
                }
            }
            catch { }
        }

        protected void btnTaoMauNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                lbParaTaoMauNhanVien.Text = "1";

                upnlMauNhanVien.Update();
                UnblockDialog("divMauNhanVien");
            }
            catch { }
        }

        protected void btnLuuMauNhanVien_Click(object sender, EventArgs e)
        {
            try
            {
                string paraTaoMau = lbParaTaoMauNhanVien.Text.Trim();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var khuvuc = nvdao.GetKV(b);

                if (paraTaoMau == "1")
                {
                    if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    var result = _spDao.Insert_MauNhanVien(txtTenMauNhanVien.Text.Trim(), txtKimM1MauNhanVien.Text.Trim(), 
                        txtKimM2MauNhanVien.Text.Trim(), khuvuc.MAKV, 1, 1, b); // services 1: nuoc; 2: dien

                    if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                    {
                        ShowInfor("Lưu mẫu nhân viên thành công.");
                        loadMauNhanVien(khuvuc.MAKV, 1); // services 1: nuoc; 2: dien
                    }
                    else
                    {
                        ShowError("Lưu không thành công. Kiểm tra lại.");
                    }

                    lbParaTaoMauNhanVien.Text = "1";
                }

                if (paraTaoMau == "2")
                {
                    if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    var result = _spDao.Update_MauNhanVien(Convert.ToInt32(ddlMauNhanVienTao.SelectedValue.ToString()),
                        txtTenMauNhanVien.Text.Trim(), txtKimM1MauNhanVien.Text.Trim(), txtKimM2MauNhanVien.Text.Trim(), b);

                    if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                    {
                        ShowInfor("Sửa mẫu nhân viên thành công.");
                        loadMauNhanVien(khuvuc.MAKV, 1); // services 1: nuoc; 2: dien
                    }
                    else
                    {
                        ShowError("Sửa không thành công. Kiểm tra lại.");
                    }

                    lbParaTaoMauNhanVien.Text = "1";
                }

                txtTenMauNhanVien.Text = "";
                //txtChiKDM1MauNhanVien.Text = "";
                //txtChiKDM2MauNhanVienn.Text = "";
                txtKimM1MauNhanVien.Text = "";
                txtKimM2MauNhanVien.Text = "";

                txtTenMauNhanVien.Enabled = false;
                //txtChiKDM1MauNhanVien.Enabled = false;
                //txtChiKDM2MauNhanVienn.Enabled = false;
                txtKimM1MauNhanVien.Enabled = false;
                txtKimM2MauNhanVien.Enabled = false;

                btnLuuMauNhanVien.Visible = false;

                upnlMauNhanVien.Update();
                upnlThongTin.Update();
            }
            catch { }
        }

        protected void btnSuaTenMau_Click(object sender, EventArgs e)
        {
            try
            {
                lbParaTaoMauNhanVien.Text = "2";

                txtTenMauNhanVien.Enabled = true;
                //txtChiKDM1MauNhanVien.Enabled = true;
                //txtChiKDM2MauNhanVienn.Enabled = true;
                txtKimM1MauNhanVien.Enabled = true;
                txtKimM2MauNhanVien.Enabled = true;

                btnLuuMauNhanVien.Visible = true;

                int maunhanvienid = Convert.ToInt32(ddlMauNhanVienTao.SelectedValue.ToString());

                var result = _spDao.Get_MauNhanVien_ById(maunhanvienid);

                txtTenMauNhanVien.Text = result.Tables[0].Rows[0]["TenMauNhanVien"].ToString();
                txtKimM1MauNhanVien.Text = result.Tables[0].Rows[0]["MaSoKimM1"].ToString();
                txtKimM2MauNhanVien.Text = result.Tables[0].Rows[0]["MaSoKimM2"].ToString();
            }
            catch { }
        }

        protected void btnThemMoiMau_Click(object sender, EventArgs e)
        {
            try
            {
                lbParaTaoMauNhanVien.Text = "1";

                txtTenMauNhanVien.Enabled = true;
                //txtChiKDM1MauNhanVien.Enabled = true;
                //txtChiKDM2MauNhanVienn.Enabled = true;
                txtKimM1MauNhanVien.Enabled = true;
                txtKimM2MauNhanVien.Enabled = true;

                btnLuuMauNhanVien.Visible = true;

                upnlMauNhanVien.Update();
            }
            catch { }
        }

        protected void btnXoaTenMau_Click(object sender, EventArgs e)
        {
            try
            {
                //Filtered = FilteredMode.None;
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var khuvuc = nvdao.GetKV(b);

                // Authenticate
                if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                int maunhanvienid = Convert.ToInt32(ddlMauNhanVienTao.SelectedValue.ToString());

                var result = _spDao.Delete_MauNhanVien(maunhanvienid, b);

                if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                {
                    ShowInfor("Xóa mẫu nhân viên thành công.");
                    loadMauNhanVien(khuvuc.MAKV, 1); // services 1: nuoc; 2: dien
                }
                else
                {
                    ShowError("Xóa không thành công. Kiểm tra lại.");
                }

                CloseWaitingDialog();
                upnlMauNhanVien.Update();
                upnlThongTin.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindMauNhanVienChiTiet()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var khuvuc = nvdao.GetKV(b);

                int maunhanvienid = Convert.ToInt32(ddlMauNhanVienTao.SelectedValue.ToString());

                if (maunhanvienid != 0)
                {
                    var result = _spDao.Get_MauNhanVienChiTiet_ByMauNhanVienId(maunhanvienid);

                    gvMauNhanVienChiTiet.DataSource = result.Tables[0];
                    gvMauNhanVienChiTiet.PagerInforText = result.Tables[0].Rows.Count.ToString();
                    gvMauNhanVienChiTiet.DataBind();
                }
                else
                {
                    gvMauNhanVienChiTiet.DataSource = null;
                    gvMauNhanVienChiTiet.PagerInforText = "0";
                    gvMauNhanVienChiTiet.DataBind();
                }

                CloseWaitingDialog();
            }
            catch { }
        }

        protected void ddlMauNhanVienTao_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                BindMauNhanVienChiTiet();
            }
            catch { }
        }

        protected void gvMauNhanVienChiTiet_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvMauNhanVienChiTiet_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                int maunhanvienchitietId = Convert.ToInt16(id);

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                switch (e.CommandName)
                {
                    case "EditSortOrderMauNhanVienChiTiet":                        

                        var result = _spDao.Get_MauNhanVienChiTiet_ById(maunhanvienchitietId);

                        if (result.Tables[0].Rows.Count != 0)
                        {
                            lbMauNhanVienChiTietId.Text = result.Tables[0].Rows[0]["Id"].ToString();
                            lbTenNhanVienMauNhanVien.Text = result.Tables[0].Rows[0]["NhanVienHoTen"].ToString();
                            txtSortOrderMauNhanVien.Text = result.Tables[0].Rows[0]["SortOrder"].ToString();
                        }

                        upnlMauNhanVien.Update();
                        CloseWaitingDialog();
                        break;

                    case "DeleteMauNhanVienChiTiet":

                        // Authenticate
                        if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Delete))
                        {
                            CloseWaitingDialog();
                            ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                            return;
                        }

                        var resultDelete = _spDao.Delete_MauNhanVienChiTiet(maunhanvienchitietId, b);

                        if (resultDelete.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                        {
                            ShowInfor("Xóa mẫu nhân viên thành công.");
                            BindMauNhanVienChiTiet();
                        }
                        else
                        {
                            ShowError("Xóa không thành công. Kiểm tra lại.");
                        }

                        upnlMauNhanVien.Update();
                        CloseWaitingDialog();
                        break;                        
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvMauNhanVienChiTiet_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvMauNhanVienChiTiet.PageIndex = e.NewPageIndex;              
                BindMauNhanVienChiTiet();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        protected void btnTimNhanVienMau_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                int maunhanvienid = Convert.ToInt32(ddlMauNhanVienTao.SelectedValue.ToString());
                if (maunhanvienid == 0)
                {
                    ShowError("Chọn mẫu nhân viên.");
                    return;
                }

                BindNhanVienMau();
                //upnlNhanVienMau.Update();
                UnblockDialog("divNhanVienMau");

                CloseWaitingDialog();
                upnlMauNhanVien.Update();
            }
            catch { }
        }

        private void BindNhanVienMau()
        {
            string makv = nvdao.Get(LoginInfo.MANV).MAKV;

            if (makv == "S" || makv == "P" || makv == "T")
            {
                var list = nvdao.SearchKV_GD(txtNhanVienMau.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVienMau.DataSource = list;
                gvNhanVienMau.PagerInforText = list.Count.ToString();
                gvNhanVienMau.DataBind();
            }
            else
            {
                var list = nvdao.SearchKV3(txtNhanVienMau.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVienMau.DataSource = list;
                gvNhanVienMau.PagerInforText = list.Count.ToString();
                gvNhanVienMau.DataBind();
            }

            upnlNhanVienMau.Update();
        }

        protected void gvNhanVienMau_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNhanVienMau_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;                

                switch (e.CommandName)
                {
                    case "SelectMANVMau":
                        var nv = nvdao.Get(id);
                        if (nv != null)
                        {              
                            // Authenticate
                            if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Insert))
                            {
                                CloseWaitingDialog();
                                ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                                return;
                            }

                            int maunhanvienid = Convert.ToInt32(ddlMauNhanVienTao.SelectedValue.ToString());
                            int sortOrder = Convert.ToInt32(txtSortOrderMauNhanVien.Text.Trim());

                            var result = _spDao.Insert_MauNhanVienChiTiet(id, maunhanvienid, sortOrder, b);

                            if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                            {
                                ShowInfor("Thêm nhân viên vào mẫu thành công.");
                                txtSortOrderMauNhanVien.Text = "1";
                                //loadMauNhanVien(khuvuc.MAKV, 1); // services 1: nuoc; 2: dien
                            }                            

                            CloseWaitingDialog();
                            BindMauNhanVienChiTiet();
                            upnlMauNhanVien.Update();
                        }

                        HideDialog("divNhanVienMau");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNhanVienMau_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvNhanVienMau.PageIndex = e.NewPageIndex;                
                BindNhanVienMau();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterNVMau_Click(object sender, EventArgs e)
        {
            BindNhanVienMau();
            CloseWaitingDialog();
        }

        protected void btnLuuSortOrder_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TC_BienBanNghiemThu, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                int maunhanvienchitietid = Convert.ToInt32(lbMauNhanVienChiTietId.Text.Trim());
                int sortOrder = Convert.ToInt32(txtSortOrderMauNhanVien.Text.Trim());

                var result = _spDao.Update_MauNhanVienChiTiet_BySortOrder(maunhanvienchitietid, sortOrder, b);

                if (result.Tables[0].Rows[0]["KetQua"].ToString() == "Ok")
                {
                    ShowInfor("Sắp xếp nhân viên thành công.");
                    lbTenNhanVienMauNhanVien.Text = "";
                    txtSortOrderMauNhanVien.Text = "1";                    
                }
                else
                {
                    ShowError("Lỗi sắp xếp nhân viên. Kiểm tra lại!");
                }

                CloseWaitingDialog();
                BindMauNhanVienChiTiet();
                upnlMauNhanVien.Update();
            }
            catch { }
        }

        protected void ddlChonMauNhanVien_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int maunhanvienid = Convert.ToInt32(ddlChonMauNhanVien.SelectedValue.ToString());

                if (maunhanvienid != 0)
                {
                    var result = _spDao.Get_MauNhanVienChiTiet_ByMauNhanVienId(maunhanvienid);

                    if (result.Tables[0].Rows.Count != 0)
                    {
                        for (int i = 0; i < result.Tables[0].Rows.Count; i++)
                        {
                           int sortOrder = Convert.ToInt16(result.Tables[0].Rows[i]["SortOrder"].ToString());

                           if (sortOrder == 1)
                            {
                                lbNV1.Text = result.Tables[0].Rows[i]["NhanVienId"].ToString();
                                txtNV1.Text = result.Tables[0].Rows[i]["NhanVienHoTen"].ToString();                                
                                txtCV1.Text = result.Tables[0].Rows[i]["TenPhong"].ToString();
                            }

                            if (sortOrder == 2)
                            {
                                lbNV2.Text = result.Tables[0].Rows[i]["NhanVienId"].ToString();
                                txtNV2.Text = result.Tables[0].Rows[i]["NhanVienHoTen"].ToString();
                                txtCV2.Text = result.Tables[0].Rows[i]["TenPhong"].ToString();
                            }

                            if (sortOrder == 3)
                            {
                                lbNV3.Text = result.Tables[0].Rows[i]["NhanVienId"].ToString();
                                txtNV3.Text = result.Tables[0].Rows[i]["NhanVienHoTen"].ToString();
                                txtCV3.Text = result.Tables[0].Rows[i]["TenPhong"].ToString();
                            }
                        }
                    }

                    var maunhanvien = _spDao.Get_MauNhanVien_ById(maunhanvienid);
                    if (maunhanvien.Tables[0].Rows.Count != 0)
                    {
                        txtCHIM1.Text = maunhanvien.Tables[0].Rows[0]["MaSoKimM1"] != null ? 
                            maunhanvien.Tables[0].Rows[0]["MaSoKimM1"].ToString() : "";
                        txtCHIM2.Text = maunhanvien.Tables[0].Rows[0]["MaSoKimM2"] != null ? 
                            maunhanvien.Tables[0].Rows[0]["MaSoKimM2"].ToString() : "";
                    }
                }

                CloseWaitingDialog();
                upnlThongTin.Update();
            }
            catch { }
        }              

    }
}
