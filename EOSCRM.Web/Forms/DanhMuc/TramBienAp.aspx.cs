using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Web.Common;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Dao;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class TramBienAp : Authentication
    {
        private readonly QuanHuyenDao _qhDao = new QuanHuyenDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly TramBADao _tbaDao = new TramBADao();

        private TRAMBIENAP ObjTBA
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var tr = (string.IsNullOrEmpty(lbMATBA.Text.Trim()) || lbMATBA.Text == "") ? new TRAMBIENAP() : _tbaDao.Get(lbMATBA.Text.Trim());
                if (tr == null)
                    return null;

                tr.MAKVPO = ddlKHUVUC.SelectedValue;
                tr.MADPPO = ddlDUONGPHO.SelectedValue;
                tr.TENTBA = txtTENTBA.Text.Trim();
                tr.DSODL = txtDANHSODL.Text.Trim();
                tr.MAXA = ddlXAPHUONG.SelectedValue;
                tr.NGAYN = DateTime.Now;

                tr.TENTBA2 = txtTenTBADienLuc.Text.Trim();
                tr.QuanHuyenId = ddlQuanHuyen.SelectedValue;

                return tr;
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
                Authenticate(Functions.DM_TramBienAp, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                }

                //if (reloadm.Text == "1")
                //{
                //    BaoCaoHN();
                //    btnKHACHHANG.Visible = false;
                //    btnSave.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_DM_TRAMBIENAP;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_TRAMBIENAP;
            }

            CommonFunc.SetPropertiesForGrid(gvList);            
        }

        protected void LoadStaticReferences()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var nhanvien =  _nvDao.Get(b);
                var khuvucpo = _kvpoDao.GetPo(nhanvien.MAKV);

                //timkv();
                LoadListKVQHPX();

                if (nhanvien.MAKV != "99")
                {
                    LoadKhuVucXaPhuong(_nvDao.Get(b).MAKV);
                }

                var dp = _dppoDao.GetList(_kvpoDao.GetPo(nhanvien.MAKV).MAKVPO);
                ddlDUONGPHO.Items.Clear();
                ddlDUONGPHO.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var x in dp)
                {
                    ddlDUONGPHO.Items.Add(new ListItem(x.MADPPO + ": " + x.TENDP, x.MADPPO));
                }                
            }
            catch { }
        }

        private void LoadListKVQHPX()
        {
            var listKhuVuc = _kvpoDao.GetList();
            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("-- Tất cả --", "%"));
            foreach (var kv in listKhuVuc)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
            }

            var huyen = _qhDao.GetList();
            ddlQuanHuyen.Items.Clear();
            ddlQuanHuyen.Items.Add(new ListItem("-- Tất cả --", "%"));
            foreach (var h in huyen)
            {
                ddlQuanHuyen.Items.Add(new ListItem(h.TENQUAN, h.Id));
            }

            var xp = _xpDao.GetList();
            ddlXAPHUONG.Items.Clear();
            ddlXAPHUONG.Items.Add(new ListItem("-- Tất cả --", "%"));
            foreach (var x in xp)
            {
                ddlXAPHUONG.Items.Add(new ListItem(x.TENXA, x.MAXA));
            }
        }

        private void LoadKhuVucXaPhuong(string makv)
        {            
            var khuvucpo = _kvpoDao.GetPo(makv);
            var quanhuyen = _qhDao.GetMAKV(makv);

            var kvpo = ddlKHUVUC.Items.FindByValue(khuvucpo.MAKVPO);
            if (kvpo != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kvpo);

            var qh = ddlQuanHuyen.Items.FindByValue(quanhuyen.Id);
            if (qh != null)
                ddlQuanHuyen.SelectedIndex = ddlQuanHuyen.Items.IndexOf(qh);

            var xp = _xpDao.GetListKV(makv);
            ddlXAPHUONG.Items.Clear();
            ddlXAPHUONG.Items.Add(new ListItem("-- Tất cả --", "%"));
            foreach (var x in xp)
            {
                ddlXAPHUONG.Items.Add(new ListItem(x.TENXA, x.MAXA));
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
                    var listKhuVuc = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("-- Tất cả --", "%"));
                    foreach (var kv in listKhuVuc)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListPo(d);
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("-- Tất cả --","%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void BindDataForGrid()
        {
            if (ddlKHUVUC.SelectedValue != "99")
            {
                if (Filtered == FilteredMode.None)
                {
                    var objList = _tbaDao.GetListKV(ddlKHUVUC.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var objList = _tbaDao.GetListTim(ddlKHUVUC.SelectedValue, ddlXAPHUONG.SelectedValue, ddlDUONGPHO.SelectedValue,
                            txtTENTBA.Text.Trim(), txtDANHSODL.Text.Trim());

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
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
                var matram = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        if (!string.Empty.Equals(matram))
                        {
                            var objDb = _tbaDao.Get(matram);
                            if (objDb != null)
                            {
                                UpdateMode = Mode.Update;

                                BindInfo(objDb);                                                       
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

        protected void BindInfo(TRAMBIENAP obj)
        {
            try
            {
                lbMATBA.Text = obj.MATBA;

                var maxa = ddlXAPHUONG.Items.FindByValue(obj.MAXA);
                if (maxa != null)
                    ddlXAPHUONG.SelectedIndex = ddlXAPHUONG.Items.IndexOf(maxa);

                var dp = ddlDUONGPHO.Items.FindByValue(obj.MADPPO);
                if (dp != null)
                    ddlDUONGPHO.SelectedIndex = ddlDUONGPHO.Items.IndexOf(dp);

                var qh = ddlQuanHuyen.Items.FindByValue(obj.QuanHuyenId);
                if (qh != null)
                    ddlQuanHuyen.SelectedIndex = ddlQuanHuyen.Items.IndexOf(qh);

                txtTENTBA.Text = obj.TENTBA;
                txtDANHSODL.Text = obj.DSODL;

                txtTenTBADienLuc.Text = obj.TENTBA2 != null ? obj.TENTBA2 : "";

                upnlInfor.Update();
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var info = ObjTBA;
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

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.DM_TramBienAp, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }
                    
                    //var tontai = _xpDao.Get(txtMaXaPhuong.Text.Trim().ToUpper(), cboKhuVuc.SelectedValue);
                    //if (tontai != null)
                    //{
                    //    CloseWaitingDialog();
                    //    ShowError("Mã xã, phường đã tồn tại. Vd: Có thể nhập thêm 02,03...", txtMaXaPhuong.ClientID);
                    //    return;
                    //}

                    string tenmay = CommonFunc.GetComputerName();
                    string ipmay = CommonFunc.GetLanIPAddressM();

                    info.MATBA = _tbaDao.NewId();

                    msg = _tbaDao.Insert(info, tenmay, ipmay, b);
                }
                // update
                else
                {
                    if (!HasPermission(Functions.DM_TramBienAp, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    _rpClass.HisBienCo(info.MATBA, info.MAKVPO, b, "trambap");

                    msg = _tbaDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);
                }

                UpdateMode = Mode.Create;

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

        protected void Clear()
        {
            lbMATBA.Text = "";
            ddlXAPHUONG.SelectedIndex = 0;
            txtTENTBA.Text = "";
            txtDANHSODL.Text = "";
            ddlDUONGPHO.SelectedIndex = 0;
            txtTenTBADienLuc.Text = "";
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (ddlKHUVUC.SelectedValue != "99")
            {
                Filtered = FilteredMode.Filtered;
                BindDataForGrid();
            }

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            try
            {        
                Clear();

                Filtered = FilteredMode.None;
                UpdateMode = Mode.Create;

                LoadStaticReferences();
                BindDataForGrid();
              
                CloseWaitingDialog();
            }
            catch { }
        }
       
        protected void ddlQuanHuyen_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQuanHuyen.SelectedValue != "%")
            {
                var quanhuyen = _qhDao.Get(ddlQuanHuyen.SelectedValue);
                //var khuvucpo = _kvpoDao.Get(quanhuyen.MAKV);

                var xp = _xpDao.GetListKV(quanhuyen.MAKV);
                ddlXAPHUONG.Items.Clear();
                ddlXAPHUONG.Items.Add(new ListItem("-- Tất cả --", "%"));
                foreach (var x in xp)
                {
                    ddlXAPHUONG.Items.Add(new ListItem(x.TENXA, x.MAXA));
                }
            }

            upnlInfor.Update();
        }

    }
}