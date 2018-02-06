using System;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.GhiChiSo.Power
{
    public partial class SuaChiSoPo : Authentication
    {
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();

        private readonly NhanVienDao _nvDao = new NhanVienDao();

        int thang = DateTime.Now.Month;
        string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_SuaChiSoPo, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo == null) return;
                    string b = loginInfo.Username;

                    if (b == "nguyen")
                    {
                        btnNGUYEN.Visible = true;
                        btnSNGUYEN.Visible = true;
                        btnChuyenNguyen.Visible = true;
                    }
                    LoadStaticReferences();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_GCS_SUACHISOPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_SUACHISOPO;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
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

        private void LoadStaticReferences()
        {
            var kvList = _kvpoDao.GetList();

            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in kvList)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
            }

            ddlKHUVUC1.DataSource = kvList;
            ddlKHUVUC1.DataTextField = "TENKV";
            ddlKHUVUC1.DataValueField = "MAKVPO";
            ddlKHUVUC1.DataBind();

            timkv();
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

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
                    ddlKHUVUC1.Items.Clear();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                    txtNAM.Enabled = true;
                    ddlTHANG.Enabled = true;
                }
                else
                {
                    var kvList = _kvpoDao.GetListKVPO(d);
                    ddlKHUVUC1.Items.Clear();
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                    //lock cap nhap chi so

                    var kynay = new DateTime(int.Parse(nam), thang, 1);
                    //var kynay = new DateTime(2013, 5, 1);
                    //btnSearch.Visible = !_gcspoDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
                    btnSearch.Visible = true;

                }
            }
        }

        private void BindData()
        {
            if (txtSODB.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Chọn khách hàng để cập nhật chỉ số.", txtSODB.ClientID);
                return;
            }

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            var kh = _khpoDao.GetKHDBKV(txtSODB.Text.Trim(), ddlKHUVUC1.SelectedValue);


            //khoa so theo dot in hoa don
            /*bool p7d1 = _gcspoDao.IsLockDotInHD(kynay, ddlKHUVUC.SelectedValue, "DDP7D1");//phien 7 , kh muc dich khac, ngoai sinh hoat
            if (kh.MAMDSDPO != "A" && kh.MAMDSDPO != "B" && kh.MAMDSDPO != "G" && kh.MAMDSDPO != "Z")
            {
                if (p7d1 == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số. Đợt 1.");
                    return;
                }
            */


            if (kh == null)
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại. Vui lòng chọn lại danh bộ khách hàng.", txtSODB.ClientID);
                return;
            }

            if (!_gcspoDao.IsDaKhoiTao(kynay, kh.IDKHPO))
            {
                var msg = _gcspoDao.KhoiTaoGhiChiSo(kynay, kh);

                if (msg != null && msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowError("Khởi tạo chỉ số cho khách hàng mới không thành công.", txtSODB.ClientID);
                    return;
                }
            }

            var list = _gcspoDao.GetListForUpdate(kynay, txtSODB.Text.Trim(), kh.MADPPO);

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();
            divList.Visible = list.Count > 0;

            upnlGrid.Update();

            CloseWaitingDialog();
        }

        private void BindDataNguyen()
        {
            if (txtSODB.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Chọn khách hàng để cập nhật chỉ số.", txtSODB.ClientID);
                return;
            }

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            var kh = _khpoDao.GetKHDBKV(txtSODB.Text.Trim(), ddlKHUVUC1.SelectedValue);

            if (kh == null)
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại. Vui lòng chọn lại danh bộ khách hàng.", txtSODB.ClientID);
                return;
            }

            if (!_gcspoDao.IsDaKhoiTao(kynay, kh.IDKHPO))
            {
                var msg = _gcspoDao.KhoiTaoGhiChiSo(kynay, kh);

                if (msg != null && msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowError("Khởi tạo chỉ số cho khách hàng mới không thành công.", txtSODB.ClientID);
                    return;
                }
            }

            var list = _gcspoDao.GetListForUpdate(kynay, txtSODB.Text.Trim(), kh.MADPPO);

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();
            divList.Visible = list.Count > 0;

            upnlGrid.Update();

            CloseWaitingDialog();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetListKV(b);

            int thang1 = int.Parse(ddlTHANG.SelectedValue);
            string nam = txtNAM.Text.Trim();
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);            

            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV != "99")
                {
                    if (dung == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }

                }
            }

            //int thang = DateTime.Now.Month - 1;
            if (int.Parse(txtNAM.Text.Trim()) > int.Parse(nam))
            {
                CloseWaitingDialog();
                HideDialog("divKhachHang");
                ShowError("Chọn năm cho đúng.");
            }
            else
            {
                if (ddlTHANG.SelectedIndex > thang - 1)
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowError("Chọn kỳ cho đúng.");
                }
                else
                {
                    BindData();
                }
            }

        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;
            var ddlTTHAIGHI = e.Row.FindControl("ddlTTHAIGHI") as DropDownList;
            var txtCHISODAU = e.Row.FindControl("txtCHISODAU") as TextBox;
            var txtCHISOCUOI = e.Row.FindControl("txtCHISOCUOI") as TextBox;
            var txtMTRUYTHU = e.Row.FindControl("txtMTRUYTHU") as TextBox;
            var txtKLTIEUTHU = e.Row.FindControl("txtKLTIEUTHU") as TextBox;

            if (hfGCS == null || txtCHISODAU == null || txtCHISOCUOI == null || txtMTRUYTHU == null ||
                txtKLTIEUTHU == null || ddlTTHAIGHI == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtCHISODAU.ClientID +
                                                                "\", \"" + txtCHISOCUOI.ClientID +
                                                                "\", \"" + txtMTRUYTHU.ClientID +
                                                                "\", \"" + txtKLTIEUTHU.ClientID +
                                                                "\", \"" + ddlTTHAIGHI.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            txtCHISODAU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtCHISOCUOI.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtMTRUYTHU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            txtKLTIEUTHU.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");

            txtCHISODAU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISODAU.ClientID + "\");");
            txtCHISOCUOI.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISOCUOI.ClientID + "\");");
            txtMTRUYTHU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtMTRUYTHU.ClientID + "\");");
            txtKLTIEUTHU.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtKLTIEUTHU.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + txtCHISODAU.ClientID +
                                                                "\", \"" + txtCHISOCUOI.ClientID +
                                                                "\", \"" + txtMTRUYTHU.ClientID +
                                                                "\", \"" + txtKLTIEUTHU.ClientID +
                                                                "\", \"" + ddlTTHAIGHI.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";

            ddlTTHAIGHI.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
        }

        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            //TODO: do not bind khach hang first, wait for filter
            //BindKhachHang();
            //upnlKhachHang.Update();
            //int thang = DateTime.Now.Month - 1;
            if (int.Parse(txtNAM.Text.Trim()) > int.Parse(nam))
            {
                CloseWaitingDialog();
                HideDialog("divKhachHang");
                ShowError("Chọn năm cho đúng.");
            }
            else
            {
                if (ddlTHANG.SelectedIndex > thang - 1)
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowError("Chọn kỳ cho đúng.");
                }
                else
                {
                    UnblockDialog("divKhachHang");
                }
            }

        }

        private void BindKhachHang()
        {
            var danhsach = _khpoDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUC.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khpoDao.GetKhachHangFromMadb(id);
                        if (khachhang != null)
                        {
                            SetControlValue(txtSODB.ClientID, id);
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            txtSODB.Focus();
                        }
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
        #endregion

        protected void ddlTHANG_SelectedIndexChanged(object sender, EventArgs e)
        {
            //int thang = DateTime.Now.Month-1;
            if (int.Parse(txtNAM.Text.Trim()) > int.Parse(nam))
            {
                CloseWaitingDialog();
                //HideDialog();
                ShowError("Chọn năm cho đúng.");
            }
            else
            {
                if (ddlTHANG.SelectedIndex > thang - 1)
                {
                    CloseWaitingDialog();
                    //HideDialog();
                    ShowError("Chọn kỳ cho đúng.");
                }
            }
        }

        protected void btnNGUYEN_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
        }

        protected void btnSNGUYEN_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetListKV(b);

            int thang1 = int.Parse(ddlTHANG.SelectedValue);
            string nam = txtNAM.Text.Trim();
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);

            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV != "99")
                {
                    if (dung == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }
                }
            }
            BindDataNguyen();
        }

        protected void btnChuyenNguyen_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetListKV(b);

            int thang1 = int.Parse(ddlTHANG.SelectedValue);
            string nam = txtNAM.Text.Trim();
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);

            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV != "99")
                {
                    if (dung == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }
                }
            }
            BindDataN();
        }

        private void BindDataN()
        {
            if (txtSODB.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Chọn khách hàng để cập nhật chỉ số.", txtSODB.ClientID);
                return;
            }

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
            var kh = _khpoDao.GetKHDBKV(txtSODB.Text.Trim(), ddlKHUVUC1.SelectedValue);

            if (kh == null)
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại. Vui lòng chọn lại danh bộ khách hàng.", txtSODB.ClientID);
                return;
            }

            if (!_gcspoDao.IsDaKhoiTao(kynay, kh.IDKHPO))
            {
                var msg = _gcspoDao.KhoiTaoGhiChiSo(kynay, kh);

                if (msg != null && msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowError("Khởi tạo chỉ số cho khách hàng mới không thành công.", txtSODB.ClientID);
                    return;
                }
            }

        }
    }
}