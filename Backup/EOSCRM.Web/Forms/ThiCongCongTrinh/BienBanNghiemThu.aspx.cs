using System;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh
{
    public partial class BienBanNghiemThu : Authentication
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
                Authenticate(Functions.TC_BienBanNghiemThu, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataBB();
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

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_BIENBANNGHIEMTHU;
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
                
                txtNV1.Text ="";                
                txtNV2.Text ="";                
                txtNV3.Text ="";
                txtCHIEUCAO.Text = "";
                txtKHOANCACH.Text = "";
                txtVITRILAP.Text = "";
                txtCHIM1.Text = "";
                txtCHIM2.Text ="";

                txtCSDONGHO.Text = "";

                txtKETLUAN.Text = "Thuỷ lượng kế hoạt động bình thường. Lắp đặt đúng qui định.";
                //txtNgayGiaoThiCong.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
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

            //var list = ddkDao.GetListDonChoThiCong(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue);            
            var list = ddkDao.GetListBienBanPB(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
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
                        if (id == null || id=="")
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
                var hd = _hdDao.Get(madon.ToString());
                var dp = _dpDao.GetDP(hd.MADP.ToString());
                var tc = _tcDao.Get(madon.ToString());
                var dh = _dhDao.Get(tc.MADH.ToString());
                var ldh = _ldhDao.Get(dh.MALDH.ToString());

                lbTENKH.Text = dondk.TENKH;
                lblTENDP1.Text = dp.TENDP;
                lbDANHSO.Text = (hd.MADP + hd.MADP).ToString();
                lblTENDP2.Text = dp.TENDP;
                lbKICHCO.Text = ldh.KICHCO.ToString();
                lbMALDH.Text = ldh.MALDH.ToString();
                lbNSX.Text = ldh.NSX.ToString();
                lbSONO.Text = dh.SONO.ToString();
                lbCSDAU.Text = tc.CSDAU.ToString();
                lbMACHIM1.Text = tc.CHIKDM1.ToString();
                lbMACHIM2.Text = tc.CHIKDM2.ToString();

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
                var hd = _hdDao.Get(madon.ToString());
                var dp = _dpDao.GetDP(hd.MADP.ToString());
                var tc = _tcDao.Get(madon.ToString());
                var dh = _dhDao.Get(tc.MADH.ToString());
                var ldh = _ldhDao.Get(dh.MALDH.ToString());
                var bb = _bbntDao.Get(madon);

                var nv1 = nvdao.Get(bb.MANV1);
                var cv1 = _cvDao.Get(nv1.MACV);
                var nv2 = nvdao.Get(bb.MANV2);
                var cv2 = _cvDao.Get(nv2.MACV);
                var nv3 = nvdao.Get(bb.MANV3);
                var cv3 = _cvDao.Get(nv3.MACV);

                lbTENKH.Text = dondk.TENKH;
                lblTENDP1.Text = dp.TENDP;
                lbDANHSO.Text = (hd.MADP + hd.MADP).ToString();
                lblTENDP2.Text = dp.TENDP;
                lbKICHCO.Text = ldh.KICHCO.ToString();
                lbMALDH.Text = ldh.MALDH.ToString();
                lbNSX.Text = ldh.NSX.ToString();
                lbSONO.Text = dh.SONO.ToString();
                lbCSDAU.Text = tc.CSDAU.ToString();
                lbMACHIM1.Text = tc.CHIKDM1.ToString();
                lbMACHIM2.Text = tc.CHIKDM2.ToString();

                lbNV1.Text = bb.MANV1.ToString();
                lbNV2.Text = bb.MANV2.ToString();
                lbNV3.Text = bb.MANV3.ToString();
                txtNV1.Text = bb.HOTEN1.ToString();
                txtNV2.Text = bb.HOTEN2.ToString();
                txtNV3.Text = bb.HOTEN3.ToString();
                txtCV1.Text = cv1.TENCV.ToString();
                txtCV2.Text = cv2.TENCV.ToString();
                txtCV3.Text = cv3.TENCV.ToString();
                txtCHIEUCAO.Text = bb.CHIEUCAO.ToString();
                txtKHOANCACH.Text = bb.KHOANGCACH.ToString();
                txtVITRILAP.Text = bb.VITRI.ToString();
                txtCHIM1.Text = bb.CHINIEMM1.ToString();
                txtCHIM2.Text = bb.CHINIEMM2.ToString();
                txtKETLUAN.Text = bb.KETLUAN.ToString();

                txtCSDONGHO.Text = bb.MADH.ToString();

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

                msg = _bbntDao.Insert(info);
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

                msg = _bbntDao.Update(info);
            }

            upnlGrid.Update();
            CloseWaitingDialog();

            
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
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
            var objList = _bbntDao.GetList();

            gvBienBan.DataSource = objList;
            gvBienBan.PagerInforText = objList.Count.ToString();
            gvBienBan.DataBind();
        }

        

        protected void gvBienBan_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvBienBan.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDataBB();
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

            lbTENKH.Text = ddkDao.Get(madon1).TENKH;

            gvTKVT.DataSource = list;
            gvTKVT.PagerInforText = list.Count.ToString();
            gvTKVT.DataBind();
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




    }
}
