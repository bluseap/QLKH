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

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class ThongTinPhanHoi : Authentication
    {
        private readonly ThongTinPhanHoiDao _objDao = new ThongTinPhanHoiDao();
        private readonly LoaiPhanHoiDao _lphDao = new LoaiPhanHoiDao();
        private readonly NhomPhanHoiDao _nphDao = new NhomPhanHoiDao();

        private THONGTINPHANHOI ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var obj = _objDao.Get(txtMAPH.Text.Trim()) ?? new THONGTINPHANHOI();

                obj.MAPH = txtMAPH.Text.Trim();
                obj.TENPH = txtTENPH.Text.Trim();
                obj.MANHOM = ddlNHOM.SelectedValue;
                obj.MALOAI = ddlLOAI.SelectedValue;
                if(txtSTT.Text.Trim() != "")
                    obj.STT = int.Parse(txtSTT.Text.Trim());

                obj.HIENTHI = cbActive.Checked;

                return obj;
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
                Authenticate(Functions.DM_ThongTinPhanHoi, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_DM_THONGTINPHANHOI;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_THONGTINPHANHOI;
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
            if (string.Empty.Equals(txtMAPH.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã phản hồi"), txtMAPH.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtTENPH.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên phản hồi"), txtTENPH.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtSTT.Text.Trim()))
            {
                try { int.Parse(txtSTT.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Số thứ tự"), txtSTT.ClientID);
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
            var nphList = _nphDao.GetList();
            ddlNHOM.Items.Clear();
            foreach (var nph in nphList)
                ddlNHOM.Items.Add(new ListItem(nph.TENNHOM, nph.MANHOM));

            var lphList = _lphDao.GetList();
            ddlLOAI.Items.Clear();
            foreach (var lph in lphList)
                ddlLOAI.Items.Add(new ListItem(lph.TENLOAI, lph.MALOAI));
        }

        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<THONGTINPHANHOI>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "phản hồi với mã", ma);
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

            txtMAPH.Text = "";
            txtMAPH.ReadOnly = false;
            txtMAPH.Focus();
            txtTENPH.Text = "";

            ddlLOAI.SelectedIndex = 0;
            ddlNHOM.SelectedIndex = 0;

            txtSTT.Text = "";
            cbActive.Checked = false;
        }





        private void BindItem(THONGTINPHANHOI obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMAPH.ClientID, obj.MAPH);
            SetReadonly(txtMAPH.ClientID, true);

            SetControlValue(txtTENPH.ClientID, obj.TENPH);
            SetControlValue(txtSTT.ClientID, obj.STT.HasValue ? obj.STT.Value.ToString() : "");

            var nhom = ddlNHOM.Items.FindByValue(obj.MANHOM);
            if (nhom != null)
                ddlNHOM.SelectedIndex = ddlNHOM.Items.IndexOf(nhom);

            var loai = ddlLOAI.Items.FindByValue(obj.MALOAI);
            if (loai != null)
                ddlLOAI.SelectedIndex = ddlLOAI.Items.IndexOf(loai);

            cbActive.Checked = obj.HIENTHI.HasValue && obj.HIENTHI.Value ? obj.HIENTHI.Value : false;

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
                if (!HasPermission(Functions.DM_ThongTinPhanHoi, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMAPH.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã phản hồi đã tồn tại", txtMAPH.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_ThongTinPhanHoi, Permission.Update))
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
                if (!HasPermission(Functions.DM_ThongTinPhanHoi, Permission.Delete))
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
    }
}