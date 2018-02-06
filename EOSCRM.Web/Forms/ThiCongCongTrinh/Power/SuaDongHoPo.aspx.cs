using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh.Power
{
    public partial class SuaDongHoPo : Authentication
    {
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly NhanVienDao _nvdao = new NhanVienDao();
        private readonly KhuVucPoDao _kvpodao = new KhuVucPoDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly SuaDongHoPoDao _sdhpoDao = new SuaDongHoPoDao();
        private readonly ReportClass _rpDao = new ReportClass();

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
        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
        }

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowInFor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
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
                Authenticate(Functions.TC_SuaDongHoPo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TC_SUADONGHOPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_SUADONGHOPO;
            }

            //CommonFunc.SetPropertiesForGrid(gvList);            
            CommonFunc.SetPropertiesForGrid(gvDDK);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
        }        

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;

                //txtNgayCapNhat.Text = DateTime.Now.ToString("dd/MM/yyyy");                
                lbMADDK1.Text = "";
                lbTENKH1.Text = "";
                lbMADHCU.Text = "";
                lbMADHMOI.Text = "";
                lbLOAIDHCU.Text = "";
                lbSONOCU.Text = "";
                lbLOAIDHMOI.Text = "";
                lbSONOMOI.Text = "";

                txtGhiChu.Text = "";

                timkv();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvdao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvpodao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvpodao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpodao.GetListKVPO(d);
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void btnFilterDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            CloseWaitingDialog();
        }

        private void BindDDK()
        {
            DateTime? tungay = null;
            DateTime? denngay = null;
            try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
            catch { txtTuNgay.Text = ""; }
            try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
            catch { txtDenNgay.Text = ""; }

            var list = _ddkpoDao.GetListDonDaThiCong(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue);
            gvDDK.DataSource = list;
            gvDDK.PagerInforText = list.Count.ToString();
            gvDDK.DataBind();
        }

        protected void gvDDK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;
            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDDK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {               
                gvDDK.PageIndex = e.NewPageIndex;               
                BindDDK();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDDK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = _ddkpoDao.Get(id);
                        if (obj == null) return;

                        var khachhangpo = _khpoDao.GetMADDK(id);
                        if (khachhangpo != null)
                        {
                            HideDialog("divDonDangKy");
                            ShowError("Khách hàng đã khai thác. Không được sửa số No.");

                            CloseWaitingDialog();
                            upnlInfor.Update();
                            return;                                               
                        }
                        else
                        {
                            BindDDKM(obj);
                            CloseWaitingDialog();
                            HideDialog("divDonDangKy");      
                        }

                        upnlInfor.Update();    
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDDKM(DONDANGKYPO obj)
        {
            var thicong = _tcDao.Get(obj.MADDKPO);
            var dhm = _dhpoDao.Get(thicong.MADH);
            lbMADDK1.Text = obj.MADDKPO;
            lbTENKH1.Text = obj.TENKH;
            lbMADHCU.Text = dhm.MADHPO.ToString();
            lbLOAIDHCU.Text = dhm.MALDHPO.ToString();
            lbSONOCU.Text = dhm.SONO.ToString();
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            CloseWaitingDialog();
        }

        private void BindDongHoSoNo()
        {
            var list = _dhpoDao.GetListDASD(txtKeywordDHSONO.Text.Trim(), ddlMaKV.SelectedValue);
            gvDongHoSoNo.DataSource = list;
            gvDongHoSoNo.PagerInforText = list.Count.ToString();
            gvDongHoSoNo.DataBind();
        }

        protected void gvDongHoSoNo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectMADH":
                        var dh = _dhpoDao.Get(id);
                        if (dh != null)
                        {
                            lbMADHMOI.Text = dh.MADHPO.ToString();
                            lbLOAIDHMOI.Text = dh.MALDHPO.ToString();
                            lbSONOMOI.Text = dh.SONO.ToString();
                            upnlInfor.Update();
                            HideDialog("divDongHoSoNo");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDongHoSoNo_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDongHoSoNo.PageIndex = e.NewPageIndex;
                BindDongHoSoNo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBrowseDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            upnlDonDangKy.Update();
            UnblockDialog("divDonDangKy");
        }

        protected void btnBrowseDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            Message msg;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.TC_SuaDongHo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (string.IsNullOrEmpty(lbMADHMOI.Text.Trim()))
                {
                    CloseWaitingDialog();
                    ShowError("Kiểm tra số No đồng hồ cho đúng!");
                    return;
                }

                //_rpDao.INSSUADONGHOPO(lbMADDK1.Text.Trim(), lbMADHCU.Text.Trim(), lbMADHMOI.Text.Trim(), lbSONOCU.Text.Trim(),
                                //lbSONOMOI.Text.Trim(), lbLOAIDHCU.Text.Trim(), lbLOAIDHMOI.Text.Trim(), LoginInfo.MANV.ToString(), txtGhiChu.Text.Trim());
                var bien = _rpDao.INSSUADONGHOPO(lbMADDK1.Text.Trim(), lbMADHCU.Text.Trim(), lbMADHMOI.Text.Trim(), lbSONOCU.Text.Trim(),
                                lbSONOMOI.Text.Trim(), lbLOAIDHCU.Text.Trim(), lbLOAIDHMOI.Text.Trim(), LoginInfo.MANV.ToString(), txtGhiChu.Text.Trim());
                if (bien == null || bien.Tables.Count == 0) 
                {
                    ShowError("Sửa số No đồng hồ không thành công...");
                    CloseWaitingDialog(); 
                    return; 
                }
            }

            CloseWaitingDialog();

            //Trả lại màn hình trống ban đầu
            LoadStaticReferences();
            // Refresh grid view
            BindDataForGrid();

            upnlGrid.Update();
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

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (Filtered == FilteredMode.None)
                {
                    //ddlMaKV.SelectedValue
                    //var objList = _sdhDao.GetList();
                    var objList = _sdhpoDao.GetListKV(_kvpodao.GetPo(_nvdao.Get(b).MAKV).MAKVPO);

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

    }
}