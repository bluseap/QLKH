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
    public partial class ApKhom : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly ApToDao _atDao = new ApToDao();

        private APTO ItemObj
        {
            get
            {
                //if (!ValidateData())
                //   return null;
                var xph = new APTO
                {
                    MAKV = cboKhuVuc.SelectedValue,
                    MAXA = ddlXAPHUONG.SelectedValue,
                    MAAPTO = txtMaApKhom.Text.Trim().ToUpper(),                    
                    TENAPTO = txtTenXaPhuong.Text.Trim(),
                    STT = Convert.ToInt16(txtSTT.Text.Trim())
                };
                return xph;
            }
        }

        #region Up, Loc
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
                Authenticate(Functions.DM_ApKhom, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_DM_APKHOM;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_APKHOM;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }        

        private void Clear()
        {
            try
            {
                //timkv();

                txtMaApKhom.Text = "";
                txtMaApKhom.Enabled = true;                
                txtTenXaPhuong.Text = "";
                txtSTT.Text = "1";
            }
            catch { }
        }

        private void LoadStaticReferences()
        {
            try
            {
                timkv();                

                txtSTT.Text = "1";
            }
            catch { }
        }

        private void BindApKhom(APTO obj)
        {
            try
            {
                if (obj == null)
                    return;

                txtMaApKhom.Text = obj.MAAPTO;

                var kv = cboKhuVuc.Items.FindByValue(obj.MAKV);
                if (kv != null)
                    cboKhuVuc.SelectedIndex = cboKhuVuc.Items.IndexOf(kv);

                var xp = ddlXAPHUONG.Items.FindByValue(obj.MAXA);
                if (xp != null)
                    ddlXAPHUONG.SelectedIndex = ddlXAPHUONG.Items.IndexOf(xp);

                txtTenXaPhuong.Text = obj.TENAPTO;
                txtSTT.Text = obj.STT.ToString();

                upnlInfor.Update();
            }
            catch { }
        }

        private void BindDataForGrid()
        {
            if (Filtered == FilteredMode.None)
            {
                var objList = _atDao.GetList(cboKhuVuc.SelectedValue, ddlXAPHUONG.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            else
            {
                var objList = _atDao.GetList(cboKhuVuc.SelectedValue, ddlXAPHUONG.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
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
                var maapkhom = e.CommandArgument.ToString();                

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(maapkhom))
                        {
                            var objDb = _atDao.GetAp(maapkhom, cboKhuVuc.SelectedValue);
                            if (objDb != null)
                            {
                                BindApKhom(objDb);
                                txtMaApKhom.Enabled = false;

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
                    var listKhuVuc = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    cboKhuVuc.Items.Add(new ListItem("--Tất cả--", "%"));
                    foreach (var kv in listKhuVuc)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    ddlXAPHUONG.Items.Clear();
                    ddlXAPHUONG.Items.Add(new ListItem("--Tất cả--", "%"));                    
                }
                else if (a.MAKV == "O")
                {
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    var xaphuong = _xpDao.GetListKV(cboKhuVuc.SelectedValue);
                    ddlXAPHUONG.Items.Clear();
                    ddlXAPHUONG.Items.Add(new ListItem("--Tất cả--", "%"));        
                    foreach (var xp in xaphuong)
                    {
                        ddlXAPHUONG.Items.Add(new ListItem(xp.TENXA + ": " + xp.MAXA, xp.MAXA));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    var xaphuong = _xpDao.GetListKV(d);
                    ddlXAPHUONG.Items.Clear();
                    ddlXAPHUONG.Items.Add(new ListItem("--Tất cả--", "%"));        
                    foreach (var xp in xaphuong)
                    {
                        ddlXAPHUONG.Items.Add(new ListItem(xp.TENXA + ": " + xp.MAXA, xp.MAXA));
                    }
                }
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var info = ItemObj;
                if (info == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                Message msg;
                Filtered = FilteredMode.None;

                if (txtMaApKhom.Text.ToUpper().Length > 5)
                {
                    CloseWaitingDialog();
                    ShowError("Mã ấp, khóm chỉ nhập 5 ký tự thôi.");
                    return;
                }

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.DM_ApKhom, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    //var tontai = _objDao.Get(txtMaPhuong.Text.Trim());
                    var tontai = _atDao.Get(txtMaApKhom.Text.Trim().ToUpper(), ddlXAPHUONG.SelectedValue, cboKhuVuc.SelectedValue);
                    if (tontai != null)
                    {
                        CloseWaitingDialog();
                        ShowError("Mã ấp, khóm đã tồn tại. Vd: nhập 02,03..", txtMaApKhom.ClientID);
                        return;
                    }

                    msg = _atDao.Insert(info);
                }
                // update
                else
                {
                    if (!HasPermission(Functions.DM_ApKhom, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    _rpClass.HisApKhomBien(info.MAAPTO, info.MAXA, info.MAKV, b, "apkhom");

                    msg = _atDao.Update(info);
                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    Clear();
                    BindDataForGrid();

                    upnlGrid.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;

            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        

    }
}