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


namespace EOSCRM.Web.Forms.SuaChua
{
    public partial class NhapThongTinSuaChua : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly SuaChuaDao _scDao = new SuaChuaDao();
        private readonly DongHoDao _dhDao = new DongHoDao();
        private readonly MucDichSuDungDao _mdsdDao = new MucDichSuDungDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly ThongTinXuLyDao _ttxlDao = new ThongTinXuLyDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();        

        private SUACHUA SuaChuaObj
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var sc = new SUACHUA
                {
                    TTXULYID = ddlThongTinXuLy.SelectedValue,
                    NOIDUNGSC = txtNoiDungSC.Text.Trim(),
                    SDTSUA = txtSDTSuaKH.Text.Trim(),
                    NGAYSC = DateTimeUtil.GetVietNamDate(txtNgayBaoSC.Text.Trim())                    
                };               

                var nhanvien = _nvDao.Get(lbNhanVienSuaID.Text.Trim());
                sc.NHANVIENSUAID = lbNhanVienSuaID.Text.Trim();
                sc.TENNVSC = nhanvien != null ? nhanvien.HOTEN : "";

                return sc;
            }
        }

        #region Startup script registeration
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

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((EOS)Page.Master).SetReadonly(id, isReadonly);
        }

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void ShowInFor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        #endregion

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
                    return Mode.Create;                }
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_NhapThongTinSuaChua, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_SC_NHAPTHONGTINSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_NHAPTHONGTINSUACHUA;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
        }        

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;
                UpdateMode = Mode.Create;

                timkv();                              

                var listTTXuLy= _ttxlDao.GetList();
                ddlThongTinXuLy.Items.Clear();
                //ddlThongTinXuLy.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var xl in listTTXuLy)
                    ddlThongTinXuLy.Items.Add(new ListItem(xl.TENXL, xl.MAXL));

                txtNgayBaoSC.Text = DateTime.Now.ToString("dd/MM/yyyy");
               
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

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    ddlKHUVUCKH.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUCKH.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    ddlKHUVUC.Items.Clear();                   
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUCKH.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUCKH.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
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
                    var objList = _scDao.GetListKV(ddlKHUVUC.SelectedValue);
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var objList = _scDao.GetListKV(ddlKHUVUC.SelectedValue);

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

        public bool IsDataValid()
        {           

            return true;
        }
        
        private void ClearContent()
        {
            Filtered = FilteredMode.None;
            UpdateMode = Mode.Create;

            txtSUACHUAID.Text = "";
            lbIDKH.Text = "";

            ddlThongTinXuLy.SelectedIndex = 0;
            txtNoiDungSC.Text = "";
            txtSDTSuaKH.Text = "";
            txtNgayBaoSC.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtTenNVSua.Text = ""; 

            lbNhanVienSuaID.Text = "";             
        }    

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var suachua = SuaChuaObj;
            if (suachua == null)
            {
                CloseWaitingDialog();
                return;
            }
            
            Message msg;
            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.SC_NhapThongTinSuaChua, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                suachua.ID = _scDao.SuaChuaNewID(ddlKHUVUC.SelectedValue.Substring(0,1));
                suachua.KHACHHANGID = lbIDKH.Text.Trim();

                var khachhang = _khDao.Get(lbIDKH.Text.Trim());
                if (khachhang != null)
                {                    
                    suachua.MADP = khachhang.MADP;
                    suachua.MADB = khachhang.MADB;
                    suachua.TENKH = khachhang.TENKH;
                    suachua.KHUVUCID = khachhang.MAKV;
                    suachua.SDT = khachhang.SDT;                    
                }

                suachua.TTSUA = "SNS_A";

                suachua.NGAYN = DateTime.Now;
                suachua.MANVN = LoginInfo.MANV;

                // insert
                msg = _scDao.Insert(suachua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
            }
            else
            {
                if (!HasPermission(Functions.SC_NhapThongTinSuaChua, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                //his table SUACHUAHIS
                _rpClass.HisTableCoBien(txtSUACHUAID.Text.Trim(), "", "", "", "", DateTime.Now, DateTime.Now, 1, 1, "", "", "HISSUACHUA");

                suachua.NGAYUP = DateTime.Now;
                suachua.MANVUP = LoginInfo.MANV;

                msg = _scDao.Update(suachua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
            }

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                CloseWaitingDialog();

                ShowInfor(ResourceLabel.Get(msg));
               
                ClearContent();

                BindDataForGrid();

                upnlGrid.Update();              
            }
            else
            {
                CloseWaitingDialog();
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            UpdateMode = Mode.Create;
            Filtered = FilteredMode.None;
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }       

        #region gvList
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "EditItem":
                        var sua = _scDao.Get(id);
                        if (sua != null)
                        {
                            UpdateMode = Mode.Update;

                            BindSuaChuaToForm(sua);
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

        protected void gvList_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion

        private void BindSuaChuaToForm(SUACHUA sua)
        {
            try
            {
                var khachhang = _khDao.Get(sua.KHACHHANGID);
                BindKHInfor(khachhang);

                txtSUACHUAID.Text = sua.ID;

                var ttxl = ddlThongTinXuLy.Items.FindByValue(sua.TTXULYID);
                if (ttxl != null)
                    ddlThongTinXuLy.SelectedIndex = ddlThongTinXuLy.Items.IndexOf(ttxl);

                txtNoiDungSC.Text = sua.NOIDUNGSC != null ? sua.NOIDUNGSC : "";
                txtSDTSuaKH.Text = sua.SDTSUA != null ? sua.SDTSUA : "";
                txtNgayBaoSC.Text = sua.NGAYSC.Value.ToString("dd/MM/yyyy");
                
                lbNhanVienSuaID.Text = sua.NHANVIENSUAID != null ? sua.NHANVIENSUAID : "";             

            }
            catch{}
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
        }

        private void BindKhachHang()
        {
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKHFILTER.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUCKH.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        private void BindKHInfor(KHACHHANG kh)
        {
            lbIDKH.Text = kh.IDKH;

            SetControlValue(txtDANHSO.ClientID, kh.MADP + kh.MADB);             
            SetControlValue(txtTENKH.ClientID, kh.TENKH);

            var mdsd = _mdsdDao.Get(kh.MAMDSD);
            SetControlValue(lbTenMDSD.ClientID, mdsd.TENMDSD);

            SetControlValue(txtSDTKH.ClientID, kh.SDT != null ? kh.SDT : "");

            var dh = _dhDao.Get(kh.MADH);
            SetControlValue(lbCongSuatDH.ClientID, dh.CONGSUAT != null ?  dh.CONGSUAT : "");
            SetControlValue(lbSoNoDH.ClientID, dh.SONO != null ? dh.SONO : "");
            SetControlValue(lbLoaiDH.ClientID, dh.MALDH != null ? dh.MALDH : ""); 
                           
            
        }

        #region gvDanhSach
        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khDao.GetKhachHangFromMadb(id);
                        if (khachhang != null)
                        {
                            BindKHInfor(khachhang);
                            HideDialog("divKhachHang");
                            upnlInfor.Update();
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

        protected void gvDanhSach_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDanhSach_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDanhSach.PageIndex = e.NewPageIndex;            
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion       

        protected void btnBrowseNhanVien_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            upnlNhanVien.Update();
            UnblockDialog("divNhanVien");
        }

        private void BindNhanVien()
        {
            var list = _nvDao.SearchKV3(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
        }

        #region gvNhanVien
        protected void gvNhanVien_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;
            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvNhanVien_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectMANV":
                        var nv = _nvDao.Get(id);
                        if (nv != null)
                        {
                            //txtMANV.Focus();
                            //lbNV1.Text = id.ToString();
                            //txtNV1.Text = nv.HOTEN.ToString();

                            upnlInfor.Update();
                        }
                        HideDialog("divNhanVien");
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvNhanVien_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvNhanVien.PageIndex = e.NewPageIndex;           
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }



    }
}