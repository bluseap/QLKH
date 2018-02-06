using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class Doan : Authentication
    {
        private readonly DoanDao _objDao = new DoanDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();




        private DOAN ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var doan = _objDao.Get(txtMADOAN.Text.Trim()) ?? new DOAN();

                doan.MATHEHIEN = txtMATHEHIEN.Text.Trim();
                doan.MAKV = cboMAKV.SelectedValue;

                return doan;
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




        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                PrepareUI();
                Authenticate(Functions.DM_Doan, Permission.Read);

                if (!Page.IsPostBack)
                {
                    LoadReferences();
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
            Page.Title = Resources.Message.TITLE_DM_DOAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_DOAN;
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


        private void LoadReferences()
        {
            var listkhuvuc = _kvDao.GetList();

            cboMAKV.Items.Clear();
            cboMAKV.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in listkhuvuc)
                cboMAKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

            UpdateMode = Mode.Create;
        }

        public bool ValidateData()
        {
            if (string.Empty.Equals(txtMADOAN.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đoạn"), txtMADOAN.ClientID);
                return false;
            }

            var kv = _kvDao.Get(cboMAKV.SelectedValue);
            if (kv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khu vực"));
                return false;
            }

            return true;
        }

        private void BindDataForGrid()
        {
            try
            {
                if (Filtered == FilteredMode.None)
                {
                    var objList = _objDao.GetList();

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var madoan = txtMADOAN.Text.Trim();
                    var mathehien = txtMATHEHIEN.Text.Trim();
                    var makv = cboMAKV.SelectedValue;

                    var objList = _objDao.GetList(madoan, mathehien, makv);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }

                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }


        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DOAN>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đoạn với mã", ma);
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

            txtMADOAN.Text = "";
            txtMADOAN.ReadOnly = false;
            txtMADOAN.Focus();
            txtMATHEHIEN.Text = "";
        }





        private void BindItem(DOAN obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMADOAN.ClientID, obj.MADOAN);
            SetReadonly(txtMADOAN.ClientID, true);

            SetControlValue(txtMATHEHIEN.ClientID, obj.MATHEHIEN);

            var kv = cboMAKV.Items.FindByValue(obj.MAKV);
            if (kv != null)
                cboMAKV.SelectedIndex = cboMAKV.Items.IndexOf(kv);

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

            var source = gvList.DataSource as List<DOAN>;
            if (source == null) return;

            var lblKHUVUC = e.Row.FindControl("lblKHUVUC") as Label;
            if(lblKHUVUC == null) return;

            var kv = _kvDao.Get(source[e.Row.RowIndex + gvList.PageSize * gvList.PageIndex].MAKV);
            if(kv == null) return;

            lblKHUVUC.Text = kv.TENKV;
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
                if (!HasPermission(Functions.DM_Doan, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMADOAN.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã đoạn đã tồn tại", txtMADOAN.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_Doan, Permission.Update))
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
                if (!HasPermission(Functions.DM_CapBac, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DeleteList();
                BindDataForGrid();
                upnlGrid.Update();

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }
    }
}