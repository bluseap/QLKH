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
    public partial class XaPhuong : Authentication
    {
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ReportClass _rpClass = new ReportClass();

        private XAPHUONG ItemObj
        {
            get
            {
                //if (!ValidateData())
                 //   return null;
                var xph = new XAPHUONG
                {
                    MAXA = txtMaXaPhuong.Text.Trim().ToUpper(),
                    MAKV = cboKhuVuc.SelectedValue,
                    TENXA = txtTenXaPhuong.Text.Trim(),
                    STT = Convert.ToInt16(txtSTT.Text.Trim())
                };
                return xph;
            }
        }

        #region Update,Filter
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
                Authenticate(Functions.DM_XaPhuong, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_DM_XAPHUONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_XAPHUONG;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }       

        private void Clear()
        {
            try
            {
                UpdateMode = Mode.Create;

                txtMaXaPhuong.Text = "";
                txtMaXaPhuong.Enabled = true;
                cboKhuVuc.SelectedIndex = 0;
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

        private void BindXaPhuong(XAPHUONG obj)
        {
            try
            {
                if (obj == null)
                    return;

                txtMaXaPhuong.Text = obj.MAXA;                

                var kv = cboKhuVuc.Items.FindByValue(obj.MAKV);
                if (kv != null)
                    cboKhuVuc.SelectedIndex = cboKhuVuc.Items.IndexOf(kv);

                txtTenXaPhuong.Text = obj.TENXA;
                txtSTT.Text = obj.STT.ToString();

                upnlInfor.Update();
            }
            catch { }
        }

        private void BindDataForGrid()
        {
            if (Filtered == FilteredMode.None)
            {                
                var objList = _xpDao.GetListKV(cboKhuVuc.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            else
            {                
                var objList = _xpDao.GetListKV(cboKhuVuc.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
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
                var maxaphuong = e.CommandArgument.ToString();              

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(maxaphuong))
                        {
                            var objDb = _xpDao.Get(maxaphuong, cboKhuVuc.SelectedValue);
                            if (objDb != null)
                            {
                                BindXaPhuong(objDb);
                                txtMaXaPhuong.Enabled = false;

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
                    cboKhuVuc.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in listKhuVuc)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "O")
                {
                    var kvList = _kvDao.GetList();
                    cboKhuVuc.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        cboKhuVuc.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
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

                //Filtered = FilteredMode.None;

                if (txtMaXaPhuong.Text.ToUpper().Length > 4)
                {
                    CloseWaitingDialog();
                    ShowError("Mã xã, phường chỉ nhập 4 ký tự thôi.");
                    return;
                }

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.DM_XaPhuong, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    //var tontai = _objDao.Get(txtMaPhuong.Text.Trim());
                    var tontai = _xpDao.Get(txtMaXaPhuong.Text.Trim().ToUpper(), cboKhuVuc.SelectedValue);
                    if (tontai != null)
                    {
                        CloseWaitingDialog();
                        ShowError("Mã xã, phường đã tồn tại. Vd: Có thể nhập thêm 02,03...", txtMaXaPhuong.ClientID);
                        return;
                    }

                    msg = _xpDao.Insert(info);
                }
                // update
                else
                {
                    if (!HasPermission(Functions.DM_XaPhuong, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    _rpClass.HisBienCo(info.MAXA, info.MAKV, b, "xaphuong");

                    msg = _xpDao.Update(info);
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

    }
}