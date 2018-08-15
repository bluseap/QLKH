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
    public partial class NhapThiCong : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThiCongDao tcdao = new ThiCongDao();
        private readonly NhanVienDao nvdao = new NhanVienDao();
        private readonly KhuVucDao kvdao = new KhuVucDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly PhuongDao _pDao = new PhuongDao();               

        private THICONG ThiCong
        {
            get
            {
                if(!IsDataValid() )
                    return null;

                string matc = "";
                if (txtMANV2.Text == "")
                { matc = ""; }
                else { matc = txtMANV2.Text.Trim(); }

                return new THICONG
                           {
                               MADDK = txtMADDK.Text.Trim(),
                               MANV = txtMANV.Text.Trim(),
                               //MANV2 = txtMANV2.Text.Trim(),
                               MANV2 = matc,
                               NGAYGTC = DateTimeUtil.GetVietNamDate(txtNgayGiaoThiCong.Text.Trim())
                           };
            }
        }

        #region up, loc
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
                Authenticate(Functions.TC_NhapDonThiCong, Permission.Read);
                PrepareUI();

                if(!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();

                    //ChayChietTinh();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_NHAPTHICONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_NHAPTHICONG;
            }

            CommonFunc.SetPropertiesForGrid(gvNhanVien);
            CommonFunc.SetPropertiesForGrid(gvDDK);
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void ChayChietTinh()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var makv = nvdao.Get(b).MAKV;

                _rpClass.DonToKeToan("", makv, "", "", "", "", "UPCTKTTOCTKH");
            }
            catch { }
        }

        private void LoadStaticReferences()
        {
            try
            {
                UpdateMode = Mode.Create;
                Filtered = FilteredMode.None;

                var list = kvdao.GetList();
                timkv();
                /*ddlMaKV .Items.Clear();
                ddlMaKV.Items.Add(new ListItem("--Tất cả--", "%"));
                foreach (var kv in list)
                {
                    ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }*/

                txtNgayGiaoThiCong.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (Filtered == FilteredMode.None)
                {
                    //var objList = tcdao.GetListDangThiCong();
                    var objList = tcdao.GetListDangThiCongKV(nvdao.Get(b).MAKV);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    DateTime? fromDate = null;
                    DateTime? toDate = null;
                    var maddk = txtMADDK.Text.Trim();
                    var tenkh = txtTENKH.Text.Trim();
                    var sonha = txtSONHA.Text.Trim();
                    var tenduong = txtDUONG.Text.Trim();
                    var tenphuong = txtPHUONG.Text.Trim();
                    var tenkv = txtKHUVUC.Text.Trim();
                    var manv = txtMANV.Text.Trim();
                    var tennv = txtTENNV.Text.Trim();
    
                    // ReSharper disable EmptyGeneralCatchClause
                    try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); } catch { }
                    try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); } catch { }
                    // ReSharper restore EmptyGeneralCatchClause
                    

                    //var objList = tcdao.GetListDangThiCong(maddk, tenkh, sonha, tenduong, tenphuong, tenkv, manv, tennv, fromDate, toDate);
                    var objList = tcdao.GetListDangThiCongKV(maddk, tenkh, sonha, tenduong, tenphuong, tenkv, manv, tennv, fromDate, toDate, nvdao.Get(b).MAKV);

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
            
            if (string.Empty.Equals(txtMADDK.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADDK.ClientID);
                return false;
            }

            var nv = nvdao.Get(txtMANV.Text.Trim());
            if (nv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã nhân viên"), txtMANV.ClientID);
                return false;
            }

            /*
            var nv2 = nvdao.Get(txtMANV2.Text.Trim());
            if (nv2 == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã nhân viên"), txtMANV2.ClientID);
                return false;
            }*/

            /*
            var pb = pbdao.Get(ddlPhongBanThiCong.SelectedValue);
            if(pb == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Phòng ban"));
                return false;
            }
            */

            try
            {
                DateTimeUtil.GetVietNamDate(txtNgayGiaoThiCong.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày giao"), txtNgayGiaoThiCong.ClientID);
                return false;
            }
            return true;
        }

        private void ClearContent()
        {
            txtMADDK.Text = "";
            txtMADDK.ReadOnly = false;
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtDUONG.Text = "";
            txtPHUONG.Text = "";
            txtKHUVUC.Text = "";
            txtNgayGiaoThiCong.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtMANV.Text = "";
            txtTENNV.Text = "";
            txtMANV2.Text = "";
            txtTENNV2.Text = "";
        }        
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            Filtered = FilteredMode.None;
            UpdateMode = Mode.Create;

            BindDataForGrid();
            upnlGrid.Update();

            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var thicong = ThiCong;
            if (thicong == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;

            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.TC_NhapDonThiCong, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                thicong.NGAYN = DateTime.Now;
                msg = tcdao.Insert(thicong, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBien(thicong.MADDK, LoginInfo.MANV, nvdao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "INTHICONG");
            }
            else
            {
                if (!HasPermission(Functions.TC_NhapDonThiCong, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = tcdao.Update(thicong, TTTC.TC_P, CommonFunc.GetComputerName(),  CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBien(thicong.MADDK, LoginInfo.MANV, nvdao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                           "", "", "", "", "UPTHICONG");
            }

            if (msg != null)
            {
                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();

                    ShowInfor(ResourceLabel.Get(msg));

                    //Trả lại màn hình trống ban đầu
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
            else
            {
                CloseWaitingDialog();
                ShowError("Nhập thi công không thành công");
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }       

        private void BindTCToForm(THICONG obj)
        {
            var maphuong = obj.DONDANGKY.MAPHUONG;
            var makv = obj.DONDANGKY.MAKV;
            string phuong = "";
            if (maphuong != null && makv != null)
            {
                phuong = _pDao.GetMAKV(obj.DONDANGKY.MAPHUONG, obj.DONDANGKY.MAKV).TENPHUONG;
            }
            else { phuong = ""; }
            SetControlValue(txtMADDK.ClientID, obj.MADDK);
            SetReadonly(txtMADDK.ClientID, true);

            txtTENKH.Text = obj.DONDANGKY.TENKH;
            txtSONHA.Text = obj.DONDANGKY.SONHA;
            txtDUONG.Text = obj.DONDANGKY.DUONGPHO != null ? obj.DONDANGKY.DUONGPHO.TENDP : "";
            txtPHUONG.Text = phuong;
            txtKHUVUC.Text = obj.DONDANGKY.KHUVUC != null ? obj.DONDANGKY.KHUVUC.TENKV : "";

            /*
            var pb = ddlPhongBanThiCong.Items.FindByValue(obj.MAPB);
            if (pb != null)
                ddlPhongBanThiCong.SelectedIndex = ddlPhongBanThiCong.Items.IndexOf(pb);
            */

            txtMANV.Text = obj.NHANVIEN != null ? obj.NHANVIEN.MANV : "";
            txtTENNV.Text = obj.NHANVIEN != null ? obj.NHANVIEN.HOTEN : "";

            String m2 = obj.MANV2;
            
            if (obj.MANV2 != "")
            {
                txtMANV2.Text = obj.MANV2 != null ? obj.MANV2 : "";
                txtTENNV2.Text = obj.MANV2 != null ? nvdao.Get(m2).HOTEN.ToString() : "";
            }
            else { 
                txtMANV2.Text = "";
                txtTENNV2.Text = "";
            }

            txtNgayGiaoThiCong.Text = obj.NGAYGTC.HasValue ? obj.NGAYGTC.Value.ToString("dd/MM/yyyy") : "";

            upnlInfor.Update();
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");

            var lnkBtnIDReport = e.Row.FindControl("lnkBtnIDReport") as LinkButton;
            if (lnkBtnIDReport == null) return;
            lnkBtnIDReport.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnIDReport) + "')");
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

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = tcdao.Get(id);
                        if (obj != null)
                        {
                            BindTCToForm(obj);
                            UpdateMode = Mode.Update;
                        }

                        CloseWaitingDialog();

                        break;

                    case "ReportItem":
                        Session["NHAPTHICONG_MADDK"] = id;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpPhieuCongTac.aspx", false);

                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        protected void btnFilterNV2_Click(object sender, EventArgs e)
        {
            BindNhanVien2();
            CloseWaitingDialog();
        }

        private void BindNhanVien()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (nvdao.Get(b).MAKV == "X")
            {
                var list = nvdao.SearchKVLX(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
            else
            {
                var list = nvdao.SearchKV(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
        }

        private void BindNhanVien2()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (nvdao.Get(b).MAKV == "X")
            {
                var list = nvdao.SearchKVLX(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien2.DataSource = list;
                gvNhanVien2.PagerInforText = list.Count.ToString();
                gvNhanVien2.DataBind();
            }
            else
            {
                var list = nvdao.SearchKV(txtKeywordNV.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien2.DataSource = list;
                gvNhanVien2.PagerInforText = list.Count.ToString();
                gvNhanVien2.DataBind();
            }

            //var list = nvdao.SearchKV(txtKeywordNV2.Text.Trim(), nvdao.Get(LoginInfo.MANV).MAKV, nvdao.Get(LoginInfo.MANV).MAPB);
            //gvNhanVien2.DataSource = list;
            //gvNhanVien2.PagerInforText = list.Count.ToString();
            //gvNhanVien2.DataBind();
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
                            txtMANV.Text = nv.MANV;
                            txtTENNV.Text = nv.HOTEN;
                            txtMANV.Focus();

                            upnlInfor.Update();
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
                            //SetControlValue(txtMANV2.ClientID, nv.MANV);
                            //SetControlValue(txtTENNV2.ClientID, nv.HOTEN);
                            txtMANV2.Text = nv.MANV;
                            txtTENNV2.Text = nv.HOTEN;

                            upnlInfor.Update();
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

        //private void BindToInfor(DONDANGKY obj)
        private void BindToInfor(string maddk)
        {
            try
            {
                var obj = ddkDao.Get(maddk);

                //var kvdon = obj.MAKV;
                //var dpdon = obj.MADP;
                //var phuongdon = obj.MAPHUONG;                

                SetControlValue(txtMADDK.ClientID, obj.MADDK);
                SetReadonly(txtMADDK.ClientID, true);

                txtTENKH.Text = obj.TENKH;
                txtSONHA.Text = obj.SONHA;
                //txtDUONG.Text = obj.DUONGPHO != null ? obj.DUONGPHO.TENDP : "";
                txtPHUONG.Text = (obj.MAKV != null && obj.MAPHUONG != null) ? _pDao.GetMAKV(obj.MAPHUONG,obj.MAKV).TENPHUONG.ToString() : "";
                txtKHUVUC.Text = obj.KHUVUC != null ? obj.KHUVUC.TENKV : "";

                /*
                var tc = tcdao.Get(obj.MADDK);
                if (tc != null)
                {
                    var pb = ddlPhongBanThiCong.Items.FindByValue(tc.MAPB);
                    if (pb != null)
                        ddlPhongBanThiCong.SelectedIndex = ddlPhongBanThiCong.Items.IndexOf(pb);
                }
                */

                upnlInfor.Update();
            }
            catch { }
        }

        protected void btnBrowseDDK_Click(object sender, EventArgs e)
        {
            //ChayChietTinh();

            //BindDDK();
            upnlDonDangKy.Update();
            UnblockDialog("divDonDangKy");

            ChayChietTinh1Lan();
        }

        private void ChayChietTinh1Lan()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var makv = nvdao.Get(b).MAKV;

            //_rpClass.DonToKeToan("", makv, "", "", "", "", "UPCTKTTOCTKH");
            _rpClass.DonToBravo("", makv, "", "", "", "", "UPCTKTTOCTKH1LAN");
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

            if (nvdao.Get(b).MAKV == "X" )
            {
                var list = ddkDao.GetListDonChoThiCongKhuVuc(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                gvDDK.DataSource = list;
                gvDDK.PagerInforText = list.Count.ToString();
                gvDDK.DataBind();
            }
            else
            {
                //if (nvdao.Get(b).MAKV == "S" || nvdao.Get(b).MAKV == "P" || nvdao.Get(b).MAKV == "T")
                if (nvdao.Get(b).MAKV == "S")// || nvdao.Get(b).MAKV == "P" || nvdao.Get(b).MAKV == "T")
                {
                    var list = ddkDao.GetListDonChoThiCongKhuVucCD(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                    gvDDK.DataSource = list;
                    gvDDK.PagerInforText = list.Count.ToString();
                    gvDDK.DataBind();
                }
                else
                {
                    var list = ddkDao.GetListDonChoThiCongPB(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, pb.MAPB.ToString());
                    gvDDK.DataSource = list;
                    gvDDK.PagerInforText = list.Count.ToString();
                    gvDDK.DataBind();
                }
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
                        if (obj == null) return;

                        //BindToInfor(obj);
                        BindToInfor(id.ToString());
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");
                        upnlInfor.Update();
                        UpdateMode = Mode.Create;

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            CloseWaitingDialog();
        }

        protected void linkBtnMANV_Click(object sender, EventArgs e)
        {
            var nv = nvdao.Get(txtMANV.Text.Trim());
            if (nv != null)
            {
                txtTENNV.Text = nv.HOTEN;
                txtTENNV.Focus();
            }

            CloseWaitingDialog();
        }

        protected void linkBtnMANV2_Click(object sender, EventArgs e)
        {
            var nv = nvdao.Get(txtMANV.Text.Trim());
            if (nv != null)
            {
                txtTENNV.Text = nv.HOTEN;
                txtTENNV.Focus();
            }

            CloseWaitingDialog();
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = nvdao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = kvdao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = kvdao.GetList();
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvdao.GetListKV(d);
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        protected void btnChayChietTinhBravo_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
            ChayChietTinh();
        }


    }
}
