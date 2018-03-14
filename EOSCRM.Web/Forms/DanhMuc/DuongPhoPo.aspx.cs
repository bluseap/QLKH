using System;
using System.Collections.Generic;
using System.Globalization;
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
    public partial class DuongPhoPo : Authentication
    {
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();        
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly EOSCRMDataContext _db = new EOSCRMDataContext();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();

        int thangF = DateTime.Now.Month;
        int namF = DateTime.Now.Year;

        private DUONGPHOPO ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var dp = new DUONGPHOPO
                {
                    MADPPO = txtMADP.Text.Trim(),
                    DUONGPHUPO = txtDUONGPHU.Text.Trim(),
                    TENDP = txtTENDP.Text.Trim(),                    
                    MAKVPO = cboMAKV.SelectedValue,
                    //KOPHIMT = chkKOPHIMT.Checked,
                    IDMADOTIN = ddlDOTGCS.SelectedValue
                };

                //if (!string.Empty.Equals(txtNGAYGHI.Text.Trim()))
                //    dp.NGAYGHI = DateTimeUtil.GetVietNamDate(txtNGAYGHI.Text.Trim());
                //else
                //    dp.NGAYGHI = null;                

                return dp;
            }
        }

        #region loc,up
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
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
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

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_DuongPhoPo, Permission.Read);
                PrepareUI();
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
            Page.Title = Resources.Message.TITLE_DM_DUONGPHOPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_DUONGPHOPO;
            }
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private bool ValidateData()
        {
            if (string.Empty.Equals(txtMADP.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đường phố"), txtMADP.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtTENDP.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên đường phố"), txtTENDP.ClientID);
                return false;
            }

            return true;
        }

        private void LoadReferences()
        {
            ddlTHANG1.SelectedIndex = thangF - 1;
            txtNAM1.Text = namF.ToString();

            timkv();

            UpdateMode = Mode.Create;
            txtMADP.Text = _dppoDao.NewId();
            txtMADP.ReadOnly = false;
            txtMADP.Focus();

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var dotin = _diDao.GetListKVDDNotP7(_kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO);
            ddlDOTGCS.Items.Clear();
            ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var d in dotin)
            {
                ddlDOTGCS.Items.Add(new ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
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
                    var kvList = _kvpoDao.GetList();
                    cboMAKV.Items.Clear();
                    cboMAKV.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboMAKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }                   
                }
                else
                {
                    var kvList = _kvpoDao.GetListKV(_kvpoDao.GetPo(d).MAKVPO);
                    cboMAKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboMAKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
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
                    //var objList = _objDao.GetList();
                    var objList = _dppoDao.GetListKV(cboMAKV.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var madp = txtMADP.Text.Trim();
                    var duongphu = txtDUONGPHU.Text.Trim();
                    var tendp = txtTENDP.Text.Trim();
                    var makv = cboMAKV.SelectedValue;               

                    if (ddlDOTGCS.SelectedValue == "%")
                    {
                        var objList = _dppoDao.GetListKV(madp, duongphu, tendp, makv);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
                        var objList = _dppoDao.GetListKVDotIn(cboMAKV.SelectedValue, ddlDOTGCS.SelectedValue);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                }

                upnlGrid.Update();
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
                            var res = id.Split('-');
                            var dp = _dppoDao.Get(res[0], res[1]);
                            if (dp != null)
                            {
                                BindItem(dp);
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

        private void BindItem(DUONGPHOPO obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMADP.ClientID, obj.MADPPO);
            SetReadonly(txtMADP.ClientID, true);

            SetControlValue(txtTENDP.ClientID, obj.TENDP);
            SetControlValue(txtTENTAT.ClientID, obj.TENTAT);

            var tt = cboMAKV.Items.FindByValue(obj.MAKVPO);
            if (tt != null)
                cboMAKV.SelectedIndex = cboMAKV.Items.IndexOf(tt);

            //SetControlValue(txtNGAYGHI.ClientID, obj.NGAYGHI.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYGHI.Value) : "");
           
            //var idmadt = _dpDao.GetDP(obj.MADP);
            if (obj.IDMADOTIN != null)
            {
                var dotin = ddlDOTGCS.Items.FindByValue(obj.IDMADOTIN);
                if (dotin != null)
                    ddlDOTGCS.SelectedIndex = ddlDOTGCS.Items.IndexOf(dotin);
            }
            else
            {
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new ListItem("Tất cả", "%"));
            }

            upnlInfor.Update();
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

                    var objs = new List<DUONGPHOPO>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        var res = ma.Split('-');
                        var dp = _dppoDao.Get(res[0], res[1]);

                        if (_dppoDao.IsInUse(res[0], res[1]))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đường phố với mã", ma);
                            ShowError(ResourceLabel.Get(msgIsInUse));

                            return;
                        }

                        if (dp != null)
                            objs.Add(dp);
                    }


                    if (objs.Count > 0)
                    {
                        var msg = _dppoDao.DeleteList(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
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
            txtMADP.Text = _dppoDao.NewId();
            txtMADP.ReadOnly = false;
            txtMADP.Focus();
            txtDUONGPHU.Text = "";
            txtTENDP.Text = "";
            txtTENTAT.Text = "";
            cboMAKV.SelectedIndex = 0;
            ckDotGhiCS.Checked = false;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;

            BindDataForGrid();

            upnlGrid.Update();
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
            Filtered = FilteredMode.None;
            Message msg;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            var kynayF = new DateTime(int.Parse(txtNAM1.Text.Trim()), int.Parse(ddlTHANG1.SelectedValue), 1);
            var query = _nvDao.GetKV(b);

            if (ddlDOTGCS.SelectedValue == "%")
            {
                ShowError("Chọn đợt ghi chỉ số. Kiểm tra lại.");
                CloseWaitingDialog();
                return;
            }

            // insert new
            if (UpdateMode == Mode.Create)
            {
                bool dung = _gcspoDao.IsLockTinhCuocKy(kynayF, query.MAKV.ToString());
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số. Không được thêm dường phố.");
                    return;
                }

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    CloseWaitingDialog();
                    ShowError("Chưa chọn đợt ghi chỉ số. Kiểm tra lại!");
                    return;
                }

                if (!HasPermission(Functions.DM_DuongPhoPo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _dppoDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã đường phố đã tồn tại", txtMADP.ClientID);
                    return;
                }

                msg = _dppoDao.Insert(info);

                _rpClass.KHDuongPhoPo(txtMADP.Text.Trim(), b, kynayF, cboMAKV.SelectedValue);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_DuongPhoPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (ddlDOTGCS.SelectedValue == "%")
                {
                    CloseWaitingDialog();
                    ShowError("Chưa chọn đợt ghi chỉ số. Kiểm tra lại!");
                    return;
                }

                bool dung = _gcspoDao.IsLockTinhCuocKy(kynayF, query.MAKV.ToString());
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số. Không được cập nhật dường phố.");
                    return;
                }

                if (ckDotGhiCS.Checked == true)
                {
                    msg = _dppoDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.UPKHDUONGPHODOTINPO(info.MADPPO, b, kynayF, cboMAKV.SelectedValue, info.IDMADOTIN, ddlDOTGCS.SelectedValue);
                }
                else
                {
                    msg = _dppoDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.KHDuongPhoTen(info.MADPPO, b, kynayF, cboMAKV.SelectedValue, ddlDOTGCS.SelectedValue);
                }
            }

            CloseWaitingDialog();

            if (msg == null) return;

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));

                // Refresh grid view
                //BindDataForGrid();

                ClearForm();

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
                if (!HasPermission(Functions.DM_DuongPhoPo, Permission.Delete))
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
            Filtered = FilteredMode.None;
            ClearForm();
            CloseWaitingDialog();
            upnlGrid.Update();            
        }

        protected void ckDotGhiCS_CheckedChanged(object sender, EventArgs e)
        {
            if (ckDotGhiCS.Checked)
            {
                ddlDOTGCS.Enabled = true;
            }
            else
            {
                ddlDOTGCS.Enabled = false;
            }
        }


    }
}