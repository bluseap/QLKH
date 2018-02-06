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
    public partial class KhuVuc : Authentication
    {
        private readonly KhuVucDao _objDao = new KhuVucDao();



        private KHUVUC ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var kv = new KHUVUC
                {
                    MAKV = txtMaKV.Text.Trim(),
                    TENKV = txtTenKV.Text.Trim(),
                    ORDERS = int.Parse(txtOrders.Text.Trim()),
                    STARTCODE = txtMaKV.Text.Trim(),
                    DACBIET = false
                };

                return kv;
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
                Authenticate(Functions.DM_KhuVuc, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
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
            Page.Title = Resources.Message.TITLE_DM_KHUVUC;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_KHUVUC;
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

      

        
        private void BindDataForGrid()
        {
            var objList = _objDao.GetList();
            gvList.DataSource = objList;
            gvList.PagerInforText = objList.Count.ToString();
            gvList.DataBind();
        }
        
        public bool ValidateData()
        {
            if (string.Empty.Equals(txtMaKV.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã khu vực"), txtMaKV.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtTenKV.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên khu vực"), txtTenKV.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtOrders.Text))
            {
                try { int.Parse(txtOrders.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Thứ tự"), txtOrders.ClientID);
                    return false;
                }
            }

            return true;
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

                    var objs = new List<KHUVUC>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "khu vực với mã", ma);

                            CloseWaitingDialog();

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
                            CloseWaitingDialog();

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
                    else
                    {
                        CloseWaitingDialog();
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
            txtMaKV.Text = "";
            txtMaKV.ReadOnly = false;
            txtTenKV.Text = "";
            txtOrders.Text = "";
            UpdateMode = Mode.Create;

            txtMaKV.Focus();
        }




        private void BindKhuVuc(KHUVUC obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMaKV.ClientID, obj.MAKV);
            SetReadonly(txtMaKV.ClientID, true);

            SetControlValue(txtTenKV.ClientID, obj.TENKV);
            SetControlValue(txtOrders.ClientID, obj.ORDERS.ToString());

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
                var makv = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(makv))
                        {
                            var objDb = _objDao.Get(makv);
                            if (objDb != null)
                            {
                                BindKhuVuc(objDb);
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
                if (!HasPermission(Functions.DM_KhuVuc, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMaKV.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã khu vực đã tồn tại", txtMaKV.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_KhuVuc, Permission.Update))
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
                if (!HasPermission(Functions.DM_DuongPho, Permission.Delete))
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