using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;

using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class PhongBan : Authentication
    {
        private readonly PhongBanDao _objDao = new PhongBanDao();


        private PHONGBAN ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var pb = _objDao.Get(txtMAPB.Text.Trim()) ?? new PHONGBAN();

                pb.MAPB = txtMAPB.Text.Trim();
                pb.TENPB = txtTENPB.Text.Trim();

                var tt = _objDao.Get(cboTRUCTHUOC.SelectedValue);
                if (tt != null)
                    pb.TRUCTHUOC = tt.MAPB;

                pb.DIACHI = txtDIACHI.Text.Trim();
                pb.MOTA = txtMOTA.Text.Trim();
                pb.SDT = txtSDT.Text.Trim();

                if (!string.Empty.Equals(txtTENPB.Text))
                    pb.ORDERS = int.Parse(txtORDERS.Text.Trim());

                return pb;
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
                PrepareUI();
                Authenticate(Functions.DM_PhongBan, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_DM_PHONGBAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_PHONGBAN;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion
        
        public bool ValidateData()
        {
            if (string.Empty.Equals(txtMAPB.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã phòng ban"), txtMAPB.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtTENPB.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên phòng ban"), txtTENPB.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtTENPB.Text))
            {
                try { int.Parse(txtORDERS.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Số thứ tự"), txtORDERS.ClientID);
                    return false;
                }
            }

            return true;
        }

        private void BindDataForGrid()
        {
            var objList = _objDao.GetList();

            gvList.DataSource = objList;
            gvList.PagerInforText = objList.Count.ToString();
            gvList.DataBind();

            upnlGrid.Update();
        }

        private void LoadStaticReferences()
        {
            //TODO: load loai cong trinh
            cboTRUCTHUOC.Items.Clear();
            var listKhuVuc = _objDao.GetList();
            cboTRUCTHUOC.Items.Add(new ListItem("", ""));
            foreach(var tt in listKhuVuc)
                cboTRUCTHUOC.Items.Add(new ListItem(tt.TENPB, tt.MAPB));
        }

        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    //TODO: check relation before update list

                    var objs = new List<PHONGBAN>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "phòng ban với mã", ma);
                            ShowError(ResourceLabel.Get(msgIsInUse));

                            return;
                        }
                    }

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _objDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        var msg = _objDao.DeleteList(objs);
                        if (msg != null)
                        {
                            switch (msg.MsgType)
                            {
                                case MessageType.Error:
                                    ShowError(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Info:
                                    ShowInfor(ResourceLabel.Get(msg));
                                    break;

                                case MessageType.Warning:
                                    ShowWarning(ResourceLabel.Get(msg));
                                    break;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void ClearForm()
        {
            UpdateMode = Mode.Create;

            txtMAPB.Text = "";
            txtMAPB.ReadOnly = false;
            txtMAPB.Focus();
            txtTENPB.Text = "";
            txtDIACHI.Text = "";
            txtMOTA.Text = "";
            txtORDERS.Text = "";
            txtSDT.Text = "";
        }
        
        private void BindItem(PHONGBAN obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMAPB.ClientID, obj.MAPB);
            SetReadonly(txtMAPB.ClientID, true);

            SetControlValue(txtTENPB.ClientID, obj.TENPB);

            var tt = cboTRUCTHUOC.Items.FindByValue(obj.TRUCTHUOC);
            if (tt != null)
                cboTRUCTHUOC.SelectedIndex = cboTRUCTHUOC.Items.IndexOf(tt);

            SetControlValue(txtDIACHI.ClientID, obj.DIACHI);
            SetControlValue(txtMOTA.ClientID, obj.MOTA);
            SetControlValue(txtORDERS.ClientID, obj.ORDERS.HasValue ? obj.ORDERS.Value.ToString() : "");

            txtMAPB.Focus();

            upnlInfor.Update();
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

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(id))
                        {
                            var objDb = _objDao.Get(id);
                            if (objDb != null)
                            {
                                BindItem(objDb);
                                UpdateMode = Mode.Update;
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

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
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
                if (!HasPermission(Functions.DM_PhongBan, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMAPB.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã phòng ban đã tồn tại", txtMAPB.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_PhongBan, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = _objDao.Update(info);
            }

            CloseWaitingDialog();

            if (msg == null) return;

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));

                ClearForm();

                // Refresh grid view
                BindDataForGrid();
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
                if (!HasPermission(Functions.DM_PhongBan, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DeleteList();
                BindDataForGrid();

                CloseWaitingDialog();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
            ClearForm();
        }
    }
}