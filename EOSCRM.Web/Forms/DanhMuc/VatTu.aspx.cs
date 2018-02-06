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
    public partial class VatTu : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly VatTuDao _objDao = new VatTuDao();
        private readonly DvtDao _dvtDao = new DvtDao();
        private readonly NhomVatTuDao _nvtDao = new NhomVatTuDao();

        private VATTU VatTuObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var vt = new VATTU
                {
                    //MAVT = ddlLOAIVATTU.SelectedValue + txtMAVT.Text.Trim(),
                    MAHIEU = txtMAHIEU.Text.Trim(),
                    MANHOM = ddlNHOM.Text.Trim(),
                    DVT = ddlDVT.Text.Trim(),
                    TENVT = txtTENVT.Text.Trim(),
                    MANVN = LoginInfo.Username,

                    KYHIEUVT = txtMAVT.Text.Trim(),
                    MAKV = ddlKHUVUC.SelectedValue,
                    SOLUONG = 1,
                    STT = Int16.Parse(txtMAHIEU.Text.Trim())
                };

                if (!string.IsNullOrEmpty(txtGIAVT.Text.Trim()))
                    vt.GIAVT = decimal.Parse(txtGIAVT.Text.Trim());

                if (!string.IsNullOrEmpty(txtGIANC.Text.Trim()))
                    vt.GIANC = decimal.Parse(txtGIANC.Text.Trim());

                return vt;
            }
        }

        #region loc, up
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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
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
                Authenticate(Functions.DM_VatTu, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_DM_VATTU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_VATTU;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }       

        private void BindDataForGrid()
        {
            if (Filtered == FilteredMode.None)
            {
                if (ddlKHUVUC.SelectedValue == "X" || ddlKHUVUC.SelectedValue == "S")
                {
                    if (ddlLOAIVATTU.SelectedValue == "%")
                    {
                        var objList = _objDao.GetListKhuVuc(ddlKHUVUC.SelectedValue);
                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
                        var objList = _objDao.GetListLoaiVTKhuVucKHTT(ddlLOAIVATTU.SelectedValue, ddlKHUVUC.SelectedValue);
                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                }
                else
                {
                    if (ddlLOAIVATTU.SelectedValue == "%")
                    {
                        var objList = _objDao.GetList();
                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
                        var objList = _objDao.GetListLoaiVT(ddlLOAIVATTU.SelectedValue);
                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                }
            }
            else
            {
                if (ddlKHUVUC.SelectedValue == "X" || ddlKHUVUC.SelectedValue == "S")
                {
                    if (ddlLOAIVATTU.SelectedValue == "%")
                    {
                        decimal? giavt = null;
                        decimal? gianc = null;

                        // ReSharper disable EmptyGeneralCatchClause
                        try { giavt = decimal.Parse(txtGIAVT.Text.Trim()); }
                        catch { }
                        try { gianc = decimal.Parse(txtGIANC.Text.Trim()); }
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause
                        var objList = _objDao.GetListKhuVuc(txtMAVT.Text.Trim(), txtMAHIEU.Text.Trim(), txtTENVT.Text.Trim(),
                                    ddlDVT.SelectedValue, ddlNHOM.SelectedValue, gianc, giavt, ddlKHUVUC.SelectedValue);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
                        decimal? giavt = null;
                        decimal? gianc = null;

                        // ReSharper disable EmptyGeneralCatchClause
                        try { giavt = decimal.Parse(txtGIAVT.Text.Trim()); }
                        catch { }
                        try { gianc = decimal.Parse(txtGIANC.Text.Trim()); }
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause
                        if (ddlKHUVUC.SelectedValue == "S")
                        {
                            var objList = _objDao.GetListLoaiVTKhuVuc(txtMAVT.Text.Trim(), txtMAHIEU.Text.Trim(), txtTENVT.Text.Trim(),
                                        ddlDVT.SelectedValue, ddlNHOM.SelectedValue,
                                        gianc, giavt, ddlLOAIVATTU.SelectedValue, ddlKHUVUC.SelectedValue);

                            gvList.DataSource = objList;
                            gvList.PagerInforText = objList.Count.ToString();
                            gvList.DataBind();
                        }
                        else
                        {
                            var objList = _objDao.GetListLoaiVTKhuVucKHTT(txtMAVT.Text.Trim(), txtMAHIEU.Text.Trim(), txtTENVT.Text.Trim(),
                                        ddlDVT.SelectedValue, ddlNHOM.SelectedValue,
                                        gianc, giavt, ddlLOAIVATTU.SelectedValue, ddlKHUVUC.SelectedValue);

                            gvList.DataSource = objList;
                            gvList.PagerInforText = objList.Count.ToString();
                            gvList.DataBind();
                        }
                    }
                }
                else
                {
                    if (ddlLOAIVATTU.SelectedValue == "%")
                    {
                        decimal? giavt = null;
                        decimal? gianc = null;

                        // ReSharper disable EmptyGeneralCatchClause
                        try { giavt = decimal.Parse(txtGIAVT.Text.Trim()); }
                        catch { }
                        try { gianc = decimal.Parse(txtGIANC.Text.Trim()); }
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause
                        var objList = _objDao.GetList(txtMAVT.Text.Trim(), txtMAHIEU.Text.Trim(), txtTENVT.Text.Trim(),
                                    ddlDVT.SelectedValue, ddlNHOM.SelectedValue,
                                    gianc, giavt);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
                        decimal? giavt = null;
                        decimal? gianc = null;

                        // ReSharper disable EmptyGeneralCatchClause
                        try { giavt = decimal.Parse(txtGIAVT.Text.Trim()); }
                        catch { }
                        try { gianc = decimal.Parse(txtGIANC.Text.Trim()); }
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause
                        var objList = _objDao.GetListLoaiVTKHTT(txtMAVT.Text.Trim(), txtMAHIEU.Text.Trim(), txtTENVT.Text.Trim(),
                                    ddlDVT.SelectedValue, ddlNHOM.SelectedValue,
                                    gianc, giavt, ddlLOAIVATTU.SelectedValue);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                }
            }
        }

        private void LoadStaticReferences()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            timkv();

            var nvtList = _nvtDao.GetList();
            ddlNHOM.Items.Clear();
            ddlNHOM.Items.Add(new ListItem("Tất cả", "%"));
            foreach(var nvt in nvtList)
                ddlNHOM.Items.Add(new ListItem(nvt.TENNHOM, nvt.MANHOM));

            var dvtList = _dvtDao.GetList();
            ddlDVT.Items.Clear();
            ddlDVT.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var dvt in dvtList)
                ddlDVT.Items.Add(new ListItem(dvt.TENDVT, dvt.DVT1));

            if (_nvDao.Get(b).MAKV == "X")
            {
                ddlLOAIVATTU.Items.Clear();                
                ddlLOAIVATTU.Items.Add(new ListItem("Vật tư NƯỚC", "NN"));                
            }
            else
            {
                ddlLOAIVATTU.Items.Clear();
                ddlLOAIVATTU.Items.Add(new ListItem("Tất cả", "%"));
                ddlLOAIVATTU.Items.Add(new ListItem("Vật tư NƯỚC", "NN"));
                ddlLOAIVATTU.Items.Add(new ListItem("Vật tư ĐIỆN", "DD"));
            }

            ClearForm();
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
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
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

        public bool ValidateData()
        {
            if (string.Empty.Equals(txtMAVT.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã vật tư"), txtMAVT.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtMAHIEU.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã hiệu"), txtMAHIEU.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtTENVT.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên vật tư"), txtTENVT.ClientID);
                return false;
            }

            var dvt = _dvtDao.Get(ddlDVT.SelectedValue);
            if (dvt == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Đơn vị tính"));
                return false;
            }

            var nvt = _nvtDao.Get(ddlNHOM.SelectedValue);
            if (nvt == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Nhóm vật tư"));
                return false;
            }

            if (!string.IsNullOrEmpty(txtGIAVT.Text.Trim()))
            {
                try
                {
                    decimal.Parse(txtGIAVT.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá vật tư"), txtGIAVT.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtGIANC.Text.Trim()))
            {
                try
                {
                    decimal.Parse(txtGIANC.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá nhân công"), txtGIANC.ClientID);
                    return false;
                }
            }

            return true;
        }

        private void ClearForm()
        {
            txtMAVT.Text = "";
            txtMAVT.ReadOnly = false;
            txtMAHIEU.Text = "";
            ddlNHOM.SelectedIndex = 0;
            ddlDVT.SelectedIndex = 0;
            txtTENVT.Text = "";
            txtGIANC.Text = "0";
            txtGIAVT.Text = "0";
            ddlLOAIVATTU.SelectedIndex = 0;
            ddlLOAIVATTU.Enabled = true;
            txtMAVT.Focus();

            lbMAVT.Text = "";

            UpdateMode = Mode.Create;

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
                    var objs = new List<VATTU>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "vật tư với mã", ma);

                            CloseWaitingDialog();

                            ShowError(ResourceLabel.Get(msgIsInUse));
                            return;
                        }
                    }

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _objDao.Get(ma)));

                    if (objs.Count > 0)
                    {
                        var msg = _objDao.DeleteList(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
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

        private void BindItem(VATTU obj)
        {
            if (obj == null)
                return;

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (_nvDao.Get(b).MAKV == "X")
            {
                lbMAVT.Text = obj.MAVT;
                txtMAVT.Text = obj.KYHIEUVT;
                lbMAVTCTY.Text = obj.MAVTCTY;

                SetControlValue(txtMAHIEU.ClientID, obj.MAHIEU);
                SetControlValue(txtTENVT.ClientID, obj.TENVT);
                SetControlValue(txtGIAVT.ClientID, obj.GIAVT.HasValue ? obj.GIAVT.Value.ToString() : "");
                SetControlValue(txtGIANC.ClientID, obj.GIANC.HasValue ? obj.GIANC.Value.ToString() : "");

                var dvt = ddlDVT.Items.FindByValue(obj.DVT);
                if (dvt != null)
                    ddlDVT.SelectedIndex = ddlDVT.Items.IndexOf(dvt);

                var nvt = ddlNHOM.Items.FindByValue(obj.MANHOM);
                ddlNHOM.SelectedIndex = nvt != null ? ddlNHOM.Items.IndexOf(nvt) : 0;

                ddlLOAIVATTU.Enabled = false;
            }
            else
            {
                txtMAVT.Text = obj.MAVT;
                SetReadonly(txtMAVT.ClientID, true);

                //lbMAVT.Text = obj.MAVT;
                //txtMAVT.Text = obj.KYHIEUVT;
                //lbMAVTCTY.Text = obj.MAVTCTY;                

                SetControlValue(txtMAHIEU.ClientID, obj.MAHIEU);
                SetControlValue(txtTENVT.ClientID, obj.TENVT);
                SetControlValue(txtGIAVT.ClientID, obj.GIAVT.HasValue ? obj.GIAVT.Value.ToString() : "");
                SetControlValue(txtGIANC.ClientID, obj.GIANC.HasValue ? obj.GIANC.Value.ToString() : "");

                var dvt = ddlDVT.Items.FindByValue(obj.DVT);
                if (dvt != null)
                    ddlDVT.SelectedIndex = ddlDVT.Items.IndexOf(dvt);

                var nvt = ddlNHOM.Items.FindByValue(obj.MANHOM);
                ddlNHOM.SelectedIndex = nvt != null ? ddlNHOM.Items.IndexOf(nvt) : 0;

                ddlLOAIVATTU.Enabled = false;
            }

            upnlInfor.Update();
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
                        var objDb = _objDao.Get(id);

                        if (objDb != null)
                        {
                            BindItem(objDb);
                            UpdateMode = Mode.Update;
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var info = VatTuObj;
            if (info == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;
            Filtered = FilteredMode.None;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.DM_VatTu, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMAVT.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã vật tư đã tồn tại", txtMAVT.ClientID);
                    return;
                }

                if (ddlLOAIVATTU.SelectedValue == "%")
                {
                    CloseWaitingDialog();
                    ShowError("Chọn loại vật tư là Điện hay Nước", ddlLOAIVATTU.ClientID);
                    return;
                }

                string mavt1, mavt2, mavtcty ;
                if (_nvDao.Get(b).MAKV == "X")
                {
                    info.MAVT = ddlLOAIVATTU.SelectedValue + _objDao.NewId(ddlKHUVUC.SelectedValue);
                    mavtcty = info.MAVT;
                    mavt1 = info.MAVT;
                    msg = _objDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }
                else
                {
                    info.MAVT = ddlLOAIVATTU.SelectedValue + txtMAVT.Text.Trim();
                    mavt1 = info.MAVT;
                    mavtcty = info.MAVT;
                    msg = _objDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }

                if (_nvDao.Get(b).MAKV == "X")
                {
                    info.MAVT = ddlLOAIVATTU.SelectedValue + _objDao.NewId(ddlKHUVUC.SelectedValue);
                    info.MAVTCTY = mavtcty;
                    mavt2 = info.MAVT;

                    info.MAHIEU = mavt1;
                    info.LOAIVT = "KHTT";
                    _objDao.InsertKHTT(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }
                else
                {
                    info.MAVT = ddlLOAIVATTU.SelectedValue + txtMAVT.Text.Trim();
                    info.MAVTCTY = mavtcty;
                    mavt2 = info.MAVT;                    
                    info.MAHIEU = mavt1;
                    info.LOAIVT = "KHTT";
                    _objDao.InsertKHTT(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }
                
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_VatTu, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (_nvDao.Get(b).MAKV == "X")
                {
                    info.MAVT = lbMAVT.Text.Trim();
                    msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    
                }
                else
                {
                    info.MAVT = txtMAVT.Text.Trim();
                    msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }

                if (_nvDao.Get(b).MAKV == "X")
                {
                    info.MAVT = lbMAVTCTY.Text.Trim();
                    msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                }
                //else
                //{
                //    info.MAVT = lbMAVTCTY.Text.Trim();
                //    msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                //}
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
                if (!HasPermission(Functions.DM_VatTu, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DeleteList();

                ClearForm();
                BindDataForGrid();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
            Filtered = FilteredMode.None;
            ClearForm();
        }
    }
}