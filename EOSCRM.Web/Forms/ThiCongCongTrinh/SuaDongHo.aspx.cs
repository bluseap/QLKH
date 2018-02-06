using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh
{
    public partial class SuaDongHo : Authentication
    {
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly DongHoDao dhDao = new DongHoDao();
        private readonly NhanVienDao nvdao = new NhanVienDao();
        private readonly KhuVucDao kvdao = new KhuVucDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly SuaDongHoDao _sdhDao = new SuaDongHoDao();
        private readonly ReportClass _rpDao = new ReportClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_SuaDongHo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TC_SUADONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_SUADONGHO;
            }

            //CommonFunc.SetPropertiesForGrid(gvList);            
            CommonFunc.SetPropertiesForGrid(gvDDK);            
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
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

        #region Startup script registeration

        private void SetReadonly(string id, bool isReadonly)
        {
            ((EOS)Page.Master).SetReadonly(id, isReadonly);
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void ShowInFor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        #endregion

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

            var query = nvdao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = kvdao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = kvdao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvdao.GetListKV(d);
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
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

            var list = ddkDao.GetListDonDaThiCong(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue);
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
                        var obj = ddkDao.Get(id);
                        if (obj == null) return;

                        var khachhang = _khDao.GetMADDK(id);
                        if (khachhang != null)
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

        private void BindDDKM(DONDANGKY obj)
        {
            try
            {
                var thicong = _tcDao.Get(obj.MADDK);
                var dhm = dhDao.Get(thicong.MADH);
                lbMADDK1.Text = obj.MADDK;
                lbTENKH1.Text = obj.TENKH;
                lbMADHCU.Text = dhm.MADH.ToString();
                lbLOAIDHCU.Text = dhm.MALDH.ToString();
                lbSONOCU.Text = dhm.SONO.ToString();
            }
            catch { }
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            CloseWaitingDialog();
        }

        private void BindDongHoSoNo()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var list = dhDao.GetListDASDKV(txtKeywordDHSONO.Text.Trim(), nvdao.Get(b).MAKV);
                gvDongHoSoNo.DataSource = list;
                gvDongHoSoNo.PagerInforText = list.Count.ToString();
                gvDongHoSoNo.DataBind();
            }
            catch{}
        }

        protected void gvDongHoSoNo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectMADH":
                        var dh = dhDao.Get(id);
                        if (dh != null)
                        {
                            lbMADHMOI.Text = dh.MADH.ToString();
                            lbLOAIDHMOI.Text = dh.MALDH.ToString();
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
                    ShowError("Kiểm tra lại số No đồng hồ!");
                    return;
                }

                //msg = _objDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                var bien = _rpDao.INSSUADONGHO(lbMADDK1.Text.Trim(), lbMADHCU.Text.Trim(), lbMADHMOI.Text.Trim(),lbSONOCU.Text.Trim(),
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
                    var objList = _sdhDao.GetListKV(nvdao.Get(b).MAKV);

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