using System;
using System.Collections.Generic;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.HeThong
{
    public partial class NguoiDung : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly UserAdminDao _objDao = new UserAdminDao();
        private readonly GroupDao _groupdao = new GroupDao();
        private readonly NhanVienDao _nvdao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
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

        #region Update,filter
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

        public Domain.UserAdmin UserAdmin
        {
            get
            {
                if (IsDataValid() == false)
                    return null;

                var objInfo = new Domain.UserAdmin
                {
                    //GroupId = int.Parse(ddlNHOM.SelectedValue),
                    CreateBy = LoginInfo.Username,
                    UpdateBy = LoginInfo.Username,
                    //Username = txtUSERNAME.Text.Trim(),
                    Username = lbMANV.Text.Trim(),
                    Password = txtPASSWORD.Text.Trim(),
                    Active = cbActive.Checked,
                    HoTen = txtUSERNAME.Text.Trim(),
                    //MANV = txtUSERNAME.Text.Trim()
                    MANV = lbMANV.Text.Trim()
                };

                if (!string.IsNullOrEmpty(hdId.Value))
                {
                    try {objInfo.Id = int.Parse(hdId.Value);}
                    catch { return null; }
                }

                return objInfo;
            }
        }

        protected int? UserId
        {
            get { return int.Parse(Session["NGUOIDUNG_USERID"].ToString()); }
            set { Session["NGUOIDUNG_USERID"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SYS_UserAdmin, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {                    
                    LoadStaticReferences();
                    BindDataForGrid();
                    ClearForm();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_SYS_NGUOIDUNG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_HETHONG;
                header.TitlePage = Resources.Message.PAGE_SYS_NGUOIDUNG;
            }

            CommonFunc.SetPropertiesForGrid(gvNguoiDung);
            CommonFunc.SetPropertiesForGrid(gvNhanVien);
        }       

        private bool IsDataValid()
        {            

            /*var nv = _nvdao.Get(txtUSERNAME.Text.Trim());
            if (nv == null)
            {
                ShowError("Tên đăng nhập không hợp lệ.", txtUSERNAME.ClientID);
                return false;
            }*/

            if (string.Empty.Equals(txtPASSWORD.Text.Trim()))
            {
                ShowError("Mật khẩu không hợp lệ.", txtPASSWORD.ClientID);
                return false;
            }

            /*
            var nhom = _groupdao.Get(int.Parse(ddlNHOM.SelectedValue));
            if(nhom == null)
            {
                ShowError("Vui lòng chọn nhóm.", ddlNHOM.ClientID);
                return false;
            }*/

            return true;
        }
        
        private void BindDataForGrid()
        {
            try
            {
                var objList = _objDao.GetList();
                gvNguoiDung.DataSource = objList;
                gvNguoiDung.PagerInforText = objList.Count.ToString();
                gvNguoiDung.DataBind();

                upnlGrid.Update();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadGroupDataList()
        {
            var list = _groupdao.GetList(null);
            dpDataList.DataSource = list;
            dpDataList.DataBind();
        }

        private void LoadStaticReferences()
        {
            UserId = null;

            timkv();

            var pbList = _pbDao.GetList();
            ddlPhongBan.Items.Clear();
            ddlPhongBan.Items.Add(new ListItem("--Tất cả--", "%"));
            foreach (var pb in pbList)
            {
                ddlPhongBan.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
            }

            LoadKhuVucPhongBan(ddlKHUVUC.SelectedValue);
            
            /*
            var grouplist = _groupdao.GetList(null);
            ddlNHOM.Items.Clear();
            ddlNHOM.Items.Add(new ListItem("Tất cả", "0"));
            foreach (var group in grouplist)
            {
                ddlNHOM.Items.Add(new ListItem(group.Name, group.Id.ToString()));
            }*/
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvdao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void ClearForm()
        {
            UpdateMode = Mode.Create;

            txtUSERNAME.Text = "";
            txtPASSWORD.Text = "";
            lbMANV.Text = "";
            //ddlNHOM.SelectedIndex = 0;
            cbActive.Checked = true;
            txtUSERNAME.ReadOnly = false;
            lbTENNGUOIDUNG.Text = "";
        }

        private void UpdateUserGroup()
        {
            var usrgrplist = new List<GroupUser>();
            for (var i = 0; i < dpDataList.Items.Count; i++)
            {
                var cb = dpDataList.Items[i].FindControl("chkGroupUser") as HtmlInputCheckBox;
                if (cb == null || !cb.Checked) continue;

                var groupId = cb.Attributes["title"].Trim();

                usrgrplist.Add(new GroupUser { GroupId = int.Parse(groupId), UserId = UserId.Value});
            }

            _objDao.UpdateUserGroupList(usrgrplist, UserId.Value);
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var userObj = UserAdmin;
            if (userObj == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;
            Filtered = FilteredMode.None;
            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.SYS_UserAdmin, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (_objDao.Count(txtUSERNAME.Text.Trim()) == 1)
                {
                    CloseWaitingDialog();
                    ShowError("Tên đăng nhập trùng. Hãy đổi tên khác.", txtUSERNAME.ClientID);
                    return ;
                }

                msg = _objDao.Insert(userObj);
            }
            else
            {
                if (!HasPermission(Functions.SYS_UserAdmin, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = _objDao.Update(userObj);
                UpdateUserGroup();
            }            

            CloseWaitingDialog();

            if (msg == null) return;

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));
                ClearForm();

                // Refresh grid view
                BindDataForGrid();
                upnlGrid.Update();
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.SYS_UserAdmin, Permission.Delete))
                {
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Do delete action
                DoAction(PageAction.Delete);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Do Action
        /// </summary>
        /// <param name="action"></param>
        private void DoAction(PageAction action)
        {
            // Get list of groupId that to be update
            var strIds = Request["listIds"];

            if ((strIds == null) || (string.Empty.Equals(strIds)))
            {
                CloseWaitingDialog();
                return;
            }

            var objList = new List<Domain.UserAdmin>();
            var listIds = strIds.Split(',');

            foreach (var userAdminId in listIds)
            {
                var updateDate = CommonFunc.GetValueFromGrid(gvNguoiDung, "UserAdminId", userAdminId, "UpdateDate");
                var objInfo = new Domain.UserAdmin
                {
                    Id = int.Parse(userAdminId),
                    UpdateDate = DateTime.Parse(updateDate),
                    UpdateBy = LoginInfo.Username
                };

                // Add group to list
                objList.Add(objInfo);
            }

            // Delete group list
            var msg = _objDao.UpdateList(objList, action);

            // Show message
            if (msg.MsgType == MessageType.Error)
                ShowInfor("Xóa không thành công.");
            else
            {
                BindDataForGrid();
                ClearForm();
                ShowInfor("Xóa thành công.");
            }

            // Reload login info
            //AppCtrl.ReloadLoginInfo();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            CloseWaitingDialog();
        }

        protected void gvNguoiDung_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNguoiDung_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditUser":
                        var user = _objDao.Get(null, Int32.Parse(id));

                        if (user != null)
                        {
                            UpdateMode = Mode.Update;
                            SetControlValue(hdId.ClientID, user.Id.ToString());
                            //SetControlValue(txtUSERNAME.ClientID, user.Username);
                            txtUSERNAME.Text = user.HoTen != null ? user.HoTen : "";
                            SetControlValue(txtPASSWORD.ClientID, user.Password);
                            lbMANV.Text = user.MANV;
                            txtUSERNAME.ReadOnly = true;
                            UserId = user.Id;

                            LoadGroupDataList();
                            cbActive.Checked = user.Active;
                            
                            upnlCustomers.Update();
                            CloseWaitingDialog();
                        }
                        else
                        {
                            CloseWaitingDialog();
                            ShowInfor("Người dùng không tồn tại.");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNguoiDung_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvNguoiDung.PageIndex = e.NewPageIndex;
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region Nhân viên
        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        private void BindNhanVien()
        {
            var list = _nvdao.Search(txtKeywordNV.Text.Trim());
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
                        var nv = _nvdao.Get(id);
                        if (nv != null)
                        {
                            //SetControlValue(txtUSERNAME.ClientID, nv.MANV);
                            lbMANV.Text = nv.MANV;
                            lbTENNGUOIDUNG.Text = nv.HOTEN;

                            upnlCustomers.Update();

                            var nguoidung = _objDao.GetMANV(nv.MANV);
                            if (nguoidung != null)
                            {
                                UpdateMode = Mode.Update;
                                SetControlValue(hdId.ClientID, nguoidung.Id.ToString());
                                //SetControlValue(txtUSERNAME.ClientID, user.Username);
                                txtUSERNAME.Text = nguoidung.HoTen != null ? nguoidung.HoTen : "";
                                SetControlValue(txtPASSWORD.ClientID, nguoidung.Password);
                                lbMANV.Text = nguoidung.MANV;
                                txtUSERNAME.ReadOnly = true;
                                UserId = nguoidung.Id;

                                LoadGroupDataList();
                                cbActive.Checked = nguoidung.Active;

                                upnlCustomers.Update();
                                CloseWaitingDialog();
                            }


                            HideDialog("divNhanVien");
                            CloseWaitingDialog();

                            txtUSERNAME.Focus();
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
        #endregion

        protected void dpDataList_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType != ListItemType.Item &&
             e.Item.ItemType != ListItemType.AlternatingItem) return;

            var checkbox = e.Item.FindControl("chkGroupUser") as HtmlInputCheckBox;
            if (checkbox == null) return;

            var source = dpDataList.DataSource as List<Domain.Group>;
            if (source == null) return;

            var group = source[e.Item.ItemIndex];

            var grpusr = _objDao.GetGroupUser(group.Id, UserId.Value);

            checkbox.Checked = grpusr != null;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvdao.GetKV(b);

                if (ddlKHUVUC.SelectedValue == "%" && ddlPhongBan.SelectedValue == "%")
                {
                    ShowInfor("Chọn Khu vực.");
                    CloseWaitingDialog();
                    return;
                }
                else if (ddlKHUVUC.SelectedValue != "%" && ddlPhongBan.SelectedValue == "%")
                {
                    var objList = _objDao.GetListKV(ddlKHUVUC.SelectedValue);
                    gvNguoiDung.DataSource = objList;
                    gvNguoiDung.PagerInforText = objList.Count.ToString();
                    gvNguoiDung.DataBind();
                }
                else if (ddlKHUVUC.SelectedValue != "%" && ddlPhongBan.SelectedValue != "%")
                {
                    var objList = _objDao.GetListKVPB(ddlKHUVUC.SelectedValue, ddlPhongBan.SelectedValue);
                    gvNguoiDung.DataSource = objList;
                    gvNguoiDung.PagerInforText = objList.Count.ToString();
                    gvNguoiDung.DataBind();
                }
                else
                {
                    var objList = _objDao.GetListKV(ddlKHUVUC.SelectedValue);
                    gvNguoiDung.DataSource = objList;
                    gvNguoiDung.PagerInforText = objList.Count.ToString();
                    gvNguoiDung.DataBind();
                }

                ClearForm();
                upnlGrid.Update();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadKhuVucPhongBan(ddlKHUVUC.SelectedValue);
            }
            catch  (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadKhuVucPhongBan(string makv)
        {
            try
            {
                if (makv == "99" || makv == "%")
                {
                    var pbList = _pbDao.GetList();

                    ddlPhongBan.Items.Clear();
                    ddlPhongBan.Items.Add(new ListItem("--Tất cả--", "%"));
                    foreach (var pb in pbList)
                    {
                        ddlPhongBan.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                    }
                }
                else
                {
                    var pbList = _pbDao.GetListKV(makv);

                    ddlPhongBan.Items.Clear();
                    ddlPhongBan.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                    ddlPhongBan.Items.Add(new ListItem("Phòng KT Điện Nước", "KTDN"));
                    ddlPhongBan.Items.Add(new ListItem("Phòng Kế Toán", "PKT"));
                    foreach (var pb in pbList)
                    {
                        ddlPhongBan.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

    }
}