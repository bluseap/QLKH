using System;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh
{
    public partial class BBNTSuaChua : Authentication
    {
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

        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly GiaiQuyetThongTinSuaChuaDao _gqscDao = new GiaiQuyetThongTinSuaChuaDao();

        string madon1;

        private BBNGHIEMTHU ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var bb = _bbntDao.Get(txtMADDK.Text.Trim()) ?? new BBNGHIEMTHU();

                bb.MABBNT = txtMADDK.Text.Trim();//madon sua chua
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
                bb.NGAYLAPBB = (txtLAMBB.Text.Trim() != "") ? DateTimeUtil.GetVietNamDate(txtLAMBB.Text.Trim())
                        : DateTimeUtil.GetVietNamDate(DateTime.Now.ToString("dd/MM/yyyy"));
                bb.NGAYNHAP = DateTime.Now;
                bb.HETHONGCN = txtHETHONGCN.Text.Trim();
                bb.MANV = LoginInfo.MANV.ToString();

                return bb;
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_BBNTSuaChua, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TC_BBNTSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_BBNTSUACHUA;
            }

            //CommonFunc.SetPropertiesForGrid(gvNhanVien);
            //CommonFunc.SetPropertiesForGrid(gvDDK);
            //CommonFunc.SetPropertiesForGrid(gvList);
        }

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
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
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

                var nv = nvdao.Get(b);
                if (nv.MAKV == "O")
                {
                    if (nv.MANV == "ctpth")
                    {
                        txtCHIM1.Text = "CT/ĐNAG";
                        txtCHIM2.Text = "N2/2007";
                    }
                    else
                    {
                        if (nv.MANV == "cthtq" || nv.MANV == "dnl")
                        {
                            txtCHIM1.Text = "CT/ĐNAG";
                            txtCHIM2.Text = "N1/2007";
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

                txtKETLUAN.Text = "Thuỷ lượng kế hoạt động bình thường. Lắp đặt đúng qui định.";
                //txtNgayGiaoThiCong.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                lbKHOILUONG.Text = "";
                lbKHACHHANG.Text = "";
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

            var list = _gqscDao.GetListBienBanPB(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
            gvDDK.DataSource = list;
            gvDDK.PagerInforText = list.Count.ToString();
            gvDDK.DataBind();
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
                // Update page index
                gvDDK.PageIndex = e.NewPageIndex;
                // Bind data for grid
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
                var id = e.CommandArgument.ToString();//MADON
                switch (e.CommandName)
                {
                    case "EditItem":
                        var idkh = _gqscDao.Get(id);
                        var obj = _khDao.Get(idkh.IDKH);
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

                        BindToInfor(obj.IDKH, id);
                        LoadStaticReferences();
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");                       

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindToInfor(String madon, string dongq)
        {
            try
            {
                txtMADDK.Text = dongq.ToString();//madon giai quyet

                var dondk = _khDao.Get(madon.ToString());// madon la idkh
                if (dondk != null)
                {
                    lbTENKH.Text = dondk.TENKH;
                }

                lbDANHSO.Text = (dondk.MADP + dondk.MADB).ToString();

                var dp = _dpDao.GetDP(dondk.MADP.ToString());
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

                var dh = _dhDao.Get(dondk.MADH.ToString());
                var ldh = _ldhDao.Get(dh.MALDH.ToString());
                //lbCSDAU.Text = tc.CSDAU.ToString();
                //lbMACHIM1.Text = tc.CHIKDM1.ToString();
                 //lbMACHIM2.Text = tc.CHIKDM2.ToString();
                lbCSDAU.Text = "";
                lbMACHIM1.Text = "";
                lbMACHIM2.Text = "";

                if (ldh.KICHCO != null)
                    lbKICHCO.Text = ldh.KICHCO.ToString();
                else lbKICHCO.Text = "";
                if (ldh.NSX != null)
                    lbNSX.Text = ldh.NSX.ToString();
                else lbNSX.Text = "";

                lbMALDH.Text = ldh.MALDH.ToString();
                lbSONO.Text = dh.SONO.ToString();               

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

                var sc = _gqscDao.Get(madon.ToString());
                var dondk = _khDao.Get(sc.IDKH.ToString());
                if (dondk != null)
                {
                    if (dondk.TENKH != null)
                        lbTENKH.Text = dondk.TENKH;
                }

                lbDANHSO.Text = (dondk.MADP + dondk.MADB).ToString();

                var dp = _dpDao.GetDP(dondk.MADP.ToString());
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

                /*var tc = _tcDao.Get(madon.ToString());
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
                }*/

                var dh = _dhDao.Get(dondk.MADH.ToString());
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
                if (!HasPermission(Functions.TC_BBNTSuaChua, Permission.Insert))
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

                msg = _bbntDao.InsertSC(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                //_rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());

            }
            // update
            else
            {
                if (!HasPermission(Functions.TC_BBNTSuaChua, Permission.Update))
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

                info.MANV = LoginInfo.MANV.ToString();

                msg = _bbntDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                //_rpDao.UPCHIKDNUOC(txtMADDK.Text.Trim(), txtCHIKDM1.Text.Trim(), txtCHIKDM2.Text.Trim());
            }

            upnlGrid.Update();
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
            var list = nvdao.SearchKV3(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
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
            var list = nvdao.SearchKV3(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien2.DataSource = list;
            gvNhanVien2.PagerInforText = list.Count.ToString();
            gvNhanVien2.DataBind();
        }

        private void BindNhanVien3()
        {
            var list = nvdao.SearchKV3(txtKeywordNV3.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien3.DataSource = list;
            gvNhanVien3.PagerInforText = list.Count.ToString();
            gvNhanVien3.DataBind();
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
            var objList = _bbntDao.GetListSC();

            gvBienBan.DataSource = objList;
            gvBienBan.PagerInforText = objList.Count.ToString();
            gvBienBan.DataBind();
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

            var objList = _bbntDao.GetListSC(fromDate, toDate);

            gvBienBan.DataSource = objList;
            gvBienBan.PagerInforText = objList.Count.ToString();
            gvBienBan.DataBind();

            var tungay = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim());
            var denngay = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim());
            var sumkl = _rpDao.SumKhoiLuongBB(tungay, denngay);

            var ds = sumkl.Tables[0];
            foreach (DataRow row in ds.Rows)
            {
                lbKHOILUONG.Text = row["SUMKL"].ToString();
            }

            lbKHACHHANG.Text = objList.Count.ToString();
        }

        protected void gvBienBan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvBienBan.PageIndex = e.NewPageIndex;
                // Bind data for grid
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
                // Update page index
                gvTKVT.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindTKVT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindTKVT()
        {
            var list = _cttkDao.GetList(madon1);

            lbTENKH.Text = _khDao.Get(_gqscDao.Get(madon1).IDKH).TENKH;

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
                    upnlThietKeVatTu.Update();
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



    }
}