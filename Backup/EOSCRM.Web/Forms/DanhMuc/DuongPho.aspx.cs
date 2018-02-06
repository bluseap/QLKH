using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class DuongPho : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly DuongPhoDao _objDao = new DuongPhoDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        private readonly EOSCRMDataContext _db = new EOSCRMDataContext();
        
        private DUONGPHO ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var dp = new DUONGPHO
                             {
                                 MADP = txtMADP.Text.Trim(),
                                 DUONGPHU = txtDUONGPHU.Text.Trim(),
                                 TENDP = txtTENDP.Text.Trim(),
                                 TENTAT = txtTENTAT.Text.Trim(),
                                 MAKV = cboMAKV.SelectedValue,
                                 KOPHIMT = chkKOPHIMT.Checked,
                                 NONGTHON = chkNONGTHON.Checked,
                                 MANVG = ddlMANVG.SelectedIndex > 0 ? ddlMANVG.SelectedValue : null,
                                 MANVT = ddlMANVT.SelectedIndex > 0 ? ddlMANVT.SelectedValue : null
                             };


                if (!string.Empty.Equals(txtNGAYGHI.Text.Trim()))
                    dp.NGAYGHI = DateTimeUtil.GetVietNamDate(txtNGAYGHI.Text.Trim());
                else
                    dp.NGAYGHI = null;


                if (!string.Empty.Equals(txtTLPHUTHU.Text.Trim()))
                    dp.TLPHUTHU = int.Parse(txtTLPHUTHU.Text.Trim());
                else
                    dp.TLPHUTHU = null;

                if (!string.Empty.Equals(txtBUOCNHAY.Text.Trim()))
                    dp.BUOCNHAY = int.Parse(txtBUOCNHAY.Text.Trim());
                else
                    dp.BUOCNHAY = null;

                if (cbGIAKHAC.Checked)
                {
                    dp.GIAKHAC = true;
                    dp.TIENNUOCCQ = decimal.Parse(txtGIACQ.Text.Trim());
                    dp.THUECQK = Math.Round(Math.Round((decimal) (dp.TIENNUOCCQ - dp.TIENNUOCCQ/(decimal) 1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven) ;
                    dp.PHICQK = Math.Round(Math.Round((decimal)(dp.TIENNUOCCQ - dp.TIENNUOCCQ / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.GIACQK = dp.TIENNUOCCQ - dp.PHICQK - dp.THUECQK;

                    dp.TIENNUOCKD = decimal.Parse(txtGIAKD.Text.Trim());
                    dp.THUEKDK = Math.Round(Math.Round((decimal)(dp.TIENNUOCKD - dp.TIENNUOCKD / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.PHIKDK = Math.Round(Math.Round((decimal)(dp.TIENNUOCKD - dp.TIENNUOCKD / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.GIAKDK = dp.TIENNUOCKD - dp.PHIKDK - dp.THUEKDK;

                    dp.TIENNUOCHN = decimal.Parse(txtGIAHN.Text.Trim());
                    dp.THUEHNK = Math.Round(Math.Round((decimal)(dp.TIENNUOCHN - dp.TIENNUOCHN / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.PHIHNK = Math.Round(Math.Round((decimal)(dp.TIENNUOCHN - dp.TIENNUOCHN / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.GIAHNK = dp.TIENNUOCHN - dp.PHIHNK - dp.THUEHNK;

                    dp.TIENNUOCSH = decimal.Parse(txtGIASH.Text.Trim());
                    dp.THUESHK = Math.Round(Math.Round((decimal)(dp.TIENNUOCSH - dp.TIENNUOCSH / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.PHISHK = Math.Round(Math.Round((decimal)(dp.TIENNUOCSH - dp.TIENNUOCSH / (decimal)1.1), 2, MidpointRounding.ToEven) / 2,
                                    2, MidpointRounding.ToEven);
                    dp.GIASHK = dp.TIENNUOCSH - dp.PHISHK - dp.THUESHK;
                }
                else
                {
                    dp.GIAKHAC = false;
                    dp.GIACQK = null; dp.THUECQK = null; dp.PHICQK = null;
                    dp.GIAKDK = null; dp.THUEKDK = null; dp.PHIKDK = null;
                    dp.GIAHNK = null; dp.THUEHNK = null; dp.PHIHNK = null;
                    dp.GIASHK = null; dp.THUESHK = null; dp.PHISHK = null;
                }

                return dp;
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
                Authenticate(Functions.DM_DuongPho, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_DM_DUONGPHO;

            var header = (Header)Master.FindControl("header");

            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_DUONGPHO;
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
                    cboMAKV.Items.Clear();
                    cboMAKV.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        cboMAKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                    
                    var nvList = _nvDao.GetListByCV(MACV.GT.ToString());
                    ddlMANVG.Items.Clear();
                    ddlMANVG.Items.Add(new ListItem("Chưa xác định", ""));
                    ddlMANVT.Items.Clear();
                    ddlMANVT.Items.Add(new ListItem("Chưa xác định", ""));
                    foreach (var nv in nvList)
                    {                        
                        ddlMANVG.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
                        ddlMANVT.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
                    }  
                }
                else
                {                    
                    var kvList = _kvDao.GetListKV(d);
                    cboMAKV.Items.Clear();                    
                    foreach (var kv in kvList)
                    {
                        cboMAKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    var nvList = _nvDao.GetListByCVNV(MACV.GT.ToString(),d);                    
                    ddlMANVG.Items.Clear();                    
                    ddlMANVG.Items.Add(new ListItem("Chưa xác định", ""));
                    ddlMANVT.Items.Clear();
                    ddlMANVT.Items.Add(new ListItem("Chưa xác định", ""));                    
                    foreach (var nv in nvList)
                    {                        
                        ddlMANVG.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
                        ddlMANVT.Items.Add(new ListItem(nv.HOTEN, nv.MANV));
                    }
                }
            }
        }

        private void LoadReferences()
        {            
            timkv();    

            UpdateMode = Mode.Create;
            txtMADP.Text = _objDao.NewId();
            txtMADP.ReadOnly = false;
            txtMADP.Focus();
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

            var kv = _kvDao.Get(cboMAKV.SelectedValue);
            if (kv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khu vực"));
                return false;
            }

            if (!string.Empty.Equals(txtNGAYGHI.Text))
            {
                try { DateTimeUtil.GetVietNamDate(txtNGAYGHI.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày ghi"), txtNGAYGHI.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtTLPHUTHU.Text))
            {
                try { int.Parse(txtTLPHUTHU.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tỉ lệ phụ thu"), txtTLPHUTHU.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtBUOCNHAY.Text))
            {
                try { int.Parse(txtBUOCNHAY.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Bước nhảy"), txtBUOCNHAY.ClientID);
                    return false;
                }
            }

            if (cbGIAKHAC.Checked)
            {
                try { decimal.Parse(txtGIACQ.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá cơ quan"), txtGIACQ.ClientID);
                    return false;
                }

                try { decimal.Parse(txtGIAKD.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá kinh doanh"), txtGIAKD.ClientID);
                    return false;
                }


                try { decimal.Parse(txtGIAHN.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá hộ nghèo"), txtGIAHN.ClientID);
                    return false;
                }
                try { decimal.Parse(txtGIASH.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá sinh hoạt"), txtGIASH.ClientID);
                    return false;
                }
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
                    var madp = txtMADP.Text.Trim();
                    var duongphu = txtDUONGPHU.Text.Trim();
                    var tendp = txtTENDP.Text.Trim();
                    var makv = cboMAKV.SelectedValue;
                    DateTime? ngayghi = null;
                    int? tlpt = null;

                    // ReSharper disable EmptyGeneralCatchClause
                    try { ngayghi = DateTimeUtil.GetVietNamDate(txtNGAYGHI.Text.Trim()); }
                    catch { }
                    try { tlpt = int.Parse(txtTLPHUTHU.Text.Trim()); }
                    catch { }
                    // ReSharper restore EmptyGeneralCatchClause

                    var objList = _objDao.GetList(madp, duongphu, tendp, makv, ngayghi, tlpt);

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

        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    //TODO: check relation before update list

                    var objs = new List<DUONGPHO>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        var res = ma.Split('-');
                        var dp = _objDao.Get(res[0], res[1]);

                        if (_objDao.IsInUse(res[0], res[1]))
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
                        var msg = _objDao.DeleteList(objs, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
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
            txtMADP.Text = _objDao.NewId();
            txtMADP.ReadOnly = false;
            txtMADP.Focus();
            txtDUONGPHU.Text = "";
            txtTENDP.Text = "";
            txtTENTAT.Text = "";
            cboMAKV.SelectedIndex = 0;
            txtNGAYGHI.Text = "";
            txtTLPHUTHU.Text = "";
            chkKOPHIMT.Checked = false;
            chkNONGTHON.Checked = false;
            txtBUOCNHAY.Text = "";
            cbGIAKHAC.Checked = false;
            txtGIACQ.Text = ""; 
            txtGIAKD.Text = ""; 
            txtGIAHN.Text = ""; 
            txtGIASH.Text = ""; 

            ddlMANVG.SelectedIndex = 0;
            ddlMANVT.SelectedIndex = 0;
        }




        private void BindItem(DUONGPHO obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMADP.ClientID, obj.MADP);
            SetReadonly(txtMADP.ClientID, true);

            SetControlValue(txtTENDP.ClientID, obj.TENDP);
            SetControlValue(txtTENTAT.ClientID, obj.TENTAT);

            var tt = cboMAKV.Items.FindByValue(obj.MAKV);
            if (tt != null)
                cboMAKV.SelectedIndex = cboMAKV.Items.IndexOf(tt);

            SetControlValue(txtNGAYGHI.ClientID, obj.NGAYGHI.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYGHI.Value) : "");
            SetControlValue(txtTLPHUTHU.ClientID, obj.TLPHUTHU.HasValue ? String.Format("{0:0,0}", obj.TLPHUTHU.Value) : "");
            SetControlValue(txtBUOCNHAY.ClientID, obj.BUOCNHAY.HasValue ? String.Format("{0:0,0}", obj.BUOCNHAY.Value) : "");

            //TODO: checkboxes
            chkKOPHIMT.Checked = obj.KOPHIMT.HasValue && obj.KOPHIMT.Value;
            chkNONGTHON.Checked = obj.NONGTHON.HasValue && obj.NONGTHON.Value;

            var manvg = ddlMANVG.Items.FindByValue(obj.MANVG);
            if (manvg != null)
                ddlMANVG.SelectedIndex = ddlMANVG.Items.IndexOf(manvg);

            var manvt = ddlMANVT.Items.FindByValue(obj.MANVT);
            if (manvt != null)
                ddlMANVT.SelectedIndex = ddlMANVT.Items.IndexOf(manvt);

            cbGIAKHAC.Checked = obj.GIAKHAC.HasValue && obj.GIAKHAC.Value;

            if (cbGIAKHAC.Checked)
            {
                SetControlValue(txtGIACQ.ClientID, obj.TIENNUOCCQ.HasValue ? String.Format("{0:0,0}", obj.TIENNUOCCQ.Value) : "");
                SetControlValue(txtGIAKD.ClientID, obj.TIENNUOCKD.HasValue ? String.Format("{0:0,0}", obj.TIENNUOCKD.Value) : "");
                SetControlValue(txtGIAHN.ClientID, obj.TIENNUOCHN.HasValue ? String.Format("{0:0,0}", obj.TIENNUOCHN.Value) : "");
                SetControlValue(txtGIASH.ClientID, obj.TIENNUOCSH.HasValue ? String.Format("{0:0,0}", obj.TIENNUOCSH.Value) : "");
            }
            else
            {
                SetControlValue(txtGIACQ.ClientID, "");
                SetControlValue(txtGIAKD.ClientID, "");
                SetControlValue(txtGIAHN.ClientID, "");
                SetControlValue(txtGIASH.ClientID, "");
            }

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
                            var res = id.Split('-');
                            var dp = _objDao.Get(res[0], res[1]);
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

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.DM_DuongPho, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã đường phố đã tồn tại", txtMADP.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_DuongPho, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
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
            Filtered = FilteredMode.None;
            ClearForm();
            //BindDataForGrid();
            upnlGrid.Update();

            CloseWaitingDialog();
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