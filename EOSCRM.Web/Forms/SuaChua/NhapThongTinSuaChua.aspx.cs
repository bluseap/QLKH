using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.SuaChua
{
    public partial class NhapThongTinSuaChua : Authentication
    {
        private readonly GiaiQuyetThongTinSuaChuaDao _objDao = new GiaiQuyetThongTinSuaChuaDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly ThongTinPhanHoiDao _phDao = new ThongTinPhanHoiDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ThietKeDao _tkDao = new ThietKeDao();

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

        private void ShowInFor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        #endregion

        private GIAIQUYETTHONGTINSUACHUA GQTTSCObj
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var gcttsc = new GIAIQUYETTHONGTINSUACHUA
                {
                    NGAYBAO = DateTimeUtil.GetVietNamDate(txtNGAYBAO.Text.Trim(), txtGio.Text.Trim(), txtPhut.Text.Trim()),
                    MADON = txtMADON.Text.Trim(),
                    SDT = txtSDT.Text.Trim(),
                    THONGTINKH = txtTHONGTINKH.Text.Trim(),
                    MAPH = cboMAPH.SelectedValue,
                    MANVN = LoginInfo.MANV,
                    MAKV = ddlKHUVUC.SelectedValue,
                    NGAYNHAP = DateTime.Now
                };

                if (!string.IsNullOrEmpty(txtMAKH.Text.Trim()))
                    gcttsc.IDKH = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim()).IDKH;

                if (gcttsc.IDKH != null)
                    gcttsc.MAKV = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim()).MAKV;

                return gcttsc;
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
                    return Mode.Create;                }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_NhapThongTinSuaChua, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_SC_NHAPTHONGTINSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_NHAPTHONGTINSUACHUA;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
        }        

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;
                UpdateMode = Mode.Create;

                var listThongTinPhanHoi = _phDao.GetList();
                cboMAPH.Items.Clear();
                //cboMAPH.Items.Add(new ListItem("Tất cả", "%"));
                foreach(var maph in listThongTinPhanHoi)
                    cboMAPH.Items.Add(new ListItem(maph.TENPH, maph.MAPH));

                var khuvuclist = _kvDao.GetList();
                ddlKHUVUC.Items.Clear();
                //ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                //ddlKHUVUCKH.Items.Clear();
                //ddlKHUVUCKH.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in khuvuclist)
                {
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    //ddlKHUVUCKH.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }

                timkv();
                
                txtMADON.Text = _objDao.MakeIdkhToInsertNew();
                txtMADON.Focus();

                txtNGAYBAO.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtGio.Text = DateTime.Now.Hour.ToString();
                txtPhut.Text = DateTime.Now.Minute.ToString();
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
                    var kvList = _kvDao.GetList();
                    ddlKHUVUCKH.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUCKH.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUCKH.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUCKH.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void BindDataForGrid()
        {
            try
            {
                if (Filtered == FilteredMode.None)
                {
                    var objList = _objDao.GetListChuaPhanCong();
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    DateTime? ngaybd = null;
                    DateTime? ngaykt = null;

                    // ReSharper disable EmptyGeneralCatchClause
                    try { ngaybd = DateTimeUtil.GetVietNamDate(txtNGAYBD.Text.Trim()); } catch { }
                    try { ngaykt = DateTimeUtil.GetVietNamDate(txtNGAYKT.Text.Trim()); } catch { }
                    // ReSharper restore EmptyGeneralCatchClause

                    var objList = _objDao.GetListChuaPhanCong(txtMADON.Text.Trim(), ddlKHUVUC.SelectedValue,
                                                              txtMAKH.Text.Trim(),
                                                              txtTENKH.Text.Trim(), txtSDT.Text.Trim(),
                                                              txtTHONGTINKH.Text.Trim(),
                                                              cboMAPH.SelectedValue, ngaybd, ngaykt);

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

        public bool IsDataValid()
        {
            if (string.Empty.Equals(txtMADON.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADON.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtMAKH.Text))
            {
                var khachhang = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim());
                if (khachhang == null)
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã khách hàng"), txtMAKH.ClientID);
                    return false;
                }
            }

            var kv = _kvDao.Get(ddlKHUVUC.SelectedValue);
            if(kv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khu vực"));
                return false;
            }

            var ph = _phDao.Get(cboMAPH.SelectedValue);
            if (ph == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Nội dung báo"));
                return false;
            }

            try
            {
                DateTimeUtil.GetVietNamDate(txtNGAYBAO.Text.Trim(), txtGio.Text.Trim(), txtPhut.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày báo"), txtNGAYBAO.ClientID); 
                return false;
            }

            return true;
        }
        
        private void ClearContent()
        {
            txtMADON.Text = _objDao.MakeIdkhToInsertNew();
            txtMADON.ReadOnly = false;

            txtGio.Text = DateTime.Now.Hour.ToString();
            txtPhut.Text = DateTime.Now.Minute.ToString();
            txtNGAYBAO.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtMAKH.Text = "";
            txtSDT.Text = "";

            ddlKHUVUC.SelectedIndex = 0;
            cboMAPH.SelectedIndex = 0;

            txtTENKH.Text = "";
            txtTHONGTINKH.Text = "";
        }    

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var don = GQTTSCObj;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }
            
            Message msg;
            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.SC_NhapThongTinSuaChua, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                // check exists
                var existed = _objDao.Get(don.MADON);
                // đảm bảo không bị tình trạng lặp vô tận, quá lắm 100 lần không thể sai được
                var count = 0;
                while (existed != null && count < 100)
                {
                    txtMADON.Text = _objDao.MakeIdkhToInsertNew();
                    don.MADON = txtMADON.Text.Trim();
                    existed = _objDao.Get(don.MADON);
                    count++;
                }
                // insert
                //msg = _objDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                msg = _objDao.InsertTKSC(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV, txtNV1.Text.Trim(), lbNV1.Text.Trim() );
            }
            else
            {
                if (!HasPermission(Functions.SC_NhapThongTinSuaChua, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                //msg = _objDao.UpdateNhanTongTin(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                msg = _objDao.UpdateNhanTongTinTKSC(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV, txtNV1.Text.Trim(), lbNV1.Text.Trim());
            }

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                CloseWaitingDialog();

                ShowInfor(ResourceLabel.Get(msg));

                //Trả lại màn hình trống ban đầu
                ClearContent();

                // Refresh grid view
                BindDataForGrid();

                upnlGrid.Update();

                // bind pager
                UpdateMode = Mode.Create;

                Filtered = FilteredMode.None;
            }
            else
            {
                CloseWaitingDialog();

                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            UpdateMode = Mode.Create;
            Filtered = FilteredMode.None;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
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
                    var objs = new List<GIAIQUYETTHONGTINSUACHUA>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đơn", ma);
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msgIsInUse));
                            return;
                        }
                    }

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _objDao.Get(ma)));

                    //TODO: check relation before update list
                    var msg = _objDao.DeleteList(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

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
                        Filtered = FilteredMode.None;
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
                    ShowError("Chọn đơn để xóa.");
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

        private void SetGQTTSCToForm(GIAIQUYETTHONGTINSUACHUA gqttsc)
        {
            SetControlValue(txtMADON.ClientID, gqttsc.MADON);
            SetReadonly(txtMADON.ClientID, true);
            var kv = ddlKHUVUC.Items.FindByValue(gqttsc.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
            SetControlValue(txtMAKH.ClientID, gqttsc.KHACHHANG != null ? gqttsc.KHACHHANG.MADP + gqttsc.KHACHHANG.DUONGPHU + gqttsc.KHACHHANG.MADB : "");
            var kh = _khDao.GetKhachHangFromMadb(gqttsc.IDKH);
            if(kh != null)
                SetControlValue(txtTENKH.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetControlValue(txtSDT.ClientID, gqttsc.SDT);
            SetControlValue(txtTHONGTINKH.ClientID, gqttsc.THONGTINKH);
            var ph = cboMAPH.Items.FindByValue(gqttsc.MAPH);
            if (ph != null)
                cboMAPH.SelectedIndex = cboMAPH.Items.IndexOf(ph);
            SetControlValue(txtNGAYBAO.ClientID, gqttsc.NGAYBAO.HasValue ? String.Format("{0:dd/MM/yyyy}", gqttsc.NGAYBAO.Value) : "");
            SetControlValue(txtGio.ClientID, gqttsc.NGAYBAO.HasValue ? gqttsc.NGAYBAO.Value.Hour.ToString() : "");
            SetControlValue(txtPhut.ClientID, gqttsc.NGAYBAO.HasValue ? gqttsc.NGAYBAO.Value.Minute.ToString() : "");

            var tk = _tkDao.Get(gqttsc.MADON);
            lbNV1.Text = (tk.MANVTK != null) ? tk.MANVTK.ToString() : "";
            txtNV1.Text = (tk.TENNVTK != null) ? tk.TENNVTK.ToString() : "";

            upnlInfor.Update();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "EditItem":
                        var don = _objDao.Get(madon);
                        if (don != null)
                        {
                            UpdateMode = Mode.Update;
                            SetGQTTSCToForm(don);
                        }
                        CloseWaitingDialog();
                        break;

                    case "BOCVATTU":                       
                        //if (LoginInfo.MANV == "tam")
                        //{
                                Session["NHAPTHIETKE_MADDK"] = madon;
                                var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx";
                                Response.Redirect(url, false);
                                break;
                        //}
                        //ShowInFor("Thiết kế đã duyệt. Xin chọn thiết kế chưa duyệt.");
                        //break;                      
                    
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

        protected void gvList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
        }

        private void BindKhachHang()
        {
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKHFILTER.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUCKH.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        private void BindKHInfor(KHACHHANG kh)
        {
            SetControlValue(txtMAKH.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetControlValue(txtTENKH.ClientID, kh.TENKH);
            SetControlValue(txtSDT.ClientID, kh.SDT);

            var kv = ddlKHUVUC.Items.FindByValue(kh.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            txtMAKH.Focus();
        }

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khDao.GetKhachHangFromMadb(id);
                        if (khachhang != null)
                        {
                            BindKHInfor(khachhang);
                            HideDialog("divKhachHang");
                            upnlInfor.Update();
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

        protected void gvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDanhSach.PageIndex = e.NewPageIndex;            
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMAKH.Text.Trim() == "")
                CloseWaitingDialog();

            var khachhang = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim());
            if (khachhang != null)
            {
                var kv = ddlKHUVUC.Items.FindByValue(khachhang.MAKV);
                if (kv != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

                txtTENKH.Text = khachhang.TENKH;
                txtSDT.Text = khachhang.SDT;

                CloseWaitingDialog();
            }
            else
            {
                CloseWaitingDialog();
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã khách hàng"), txtMAKH.ClientID);
            }
        }

        protected void btnBrowseNhanVien_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            upnlNhanVien.Update();
            UnblockDialog("divNhanVien");
        }

        private void BindNhanVien()
        {
            var list = _nvDao.SearchKV3(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
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
                        var nv = _nvDao.Get(id);
                        if (nv != null)
                        {
                            //txtMANV.Focus();
                            lbNV1.Text = id.ToString();
                            txtNV1.Text = nv.HOTEN.ToString();

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

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }



    }
}