using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class TraCuuDonLapMoi : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly ChietTinhDao ctDao = new ChietTinhDao();
        private readonly HopDongDao hdDao = new HopDongDao();
        private readonly ThiCongDao tcDao = new ThiCongDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly MucDichSuDungDao mdsdDao = new MucDichSuDungDao();
        private readonly PhuongDao phuongDao = new PhuongDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();


        #region Properties
     
        protected DONDANGKY DonDangKy
        {
            get {
                try { return (DONDANGKY) Session["TCDLDM_DDK"]; }
                catch { return null; }
            }

            set { Session["TCDLDM_DDK"] = value;  }
        }

        protected THIETKE ThietKe
        {
            get
            {
                try { return (THIETKE)Session["TCDLDM_TK"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_TK"] = value; }
        }

        protected CHIETTINH ChietTinh
        {
            get
            {
                try { return (CHIETTINH)Session["TCDLDM_CT"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_CT"] = value; }
        }

        protected HOPDONG HopDong
        {
            get
            {
                try { return (HOPDONG)Session["TCDLDM_HD"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_HD"] = value; }
        }

        protected THICONG ThiCong
        {
            get
            {
                try { return (THICONG)Session["TCDLDM_TC"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_TC"] = value; }
        }

        private DONDANGKY DonDangKyObj
        {
            get
            {
                if (!IsDataValid())
                    return null;
                var obj = ddkDao.Get(txtMADDK.Text.Trim());


                obj.MADDK = txtMADDK.Text.Trim();
                obj.MADDKTONG = null;//ddlMADDKTONG.SelectedValue.Equals("") ? null : ddlMADDKTONG.SelectedValue,
                obj.TENKH = txtTENKH.Text.Trim();

                obj.TENDK = txtUYQUYEN.Text.Trim();

                obj.SONHA = txtSONHA.Text.Trim();
                obj.DIENTHOAI = txtDIENTHOAI.Text.Trim();
                obj.TEN_DC_KHAC = txtDIACHIKHAC.Text.Trim();
                obj.DAIDIEN = false; //cbDAIDIEN.Checked,
                //obj.NOIDUNG = "";
                obj.CTCTMOI = false;
                obj.MANV = LoginInfo.MANV;
               
                // dai dien, ma duong
                var phuong = phuongDao.Get(ddlPHUONG.SelectedValue);
                obj.MAPHUONG = phuong != null ? phuong.MAPHUONG : null;

                var khuvuc = kvDao.Get(ddlKHUVUC.SelectedValue);
                obj.MAKV = khuvuc != null ? khuvuc.MAKV : null;

                var mdsd = mdsdDao.Get(ddlMUCDICH.SelectedValue);
                obj.MAMDSD = mdsd != null ? mdsd.MAMDSD : null;

                if (!txtSOHODN.Text.Trim().Equals(String.Empty))
                    obj.SOHODN = Convert.ToInt32(txtSOHODN.Text.Trim());
                else
                    obj.SOHODN = null;

                var sn = (txtSONHA.Text.Trim().Equals(String.Empty) ? "" : txtSONHA.Text.Trim() + ", ");
                var tenduong = "";

                var tenphuong = phuong != null ? phuong.TENPHUONG + ", " : "";
                var tenkv = khuvuc != null ? khuvuc.TENKV : "";

                if (!txtMADP.Text.Trim().Equals(String.Empty))
                {
                    obj.MADP = txtMADP.Text.Trim();
                    obj.DUONGPHU = txtDUONGPHU.Text.Trim();

                    var duong = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
                    if (duong != null)
                        tenduong = duong.TENDP + ", ";
                }
                else
                {
                    obj.MADP = null;
                    obj.DUONGPHU = null;
                    tenduong = txtDIACHIKHAC.Text.Trim().Equals(String.Empty) ? "" : txtDIACHIKHAC.Text.Trim() + ", ";
                }

                obj.DIACHILD = string.Format("{0}{1}{2}{3}", sn, tenduong, tenphuong, tenkv);


                if (!txtSONK.Text.Trim().Equals(String.Empty))
                    obj.SONK = Convert.ToInt32(txtSONK.Text.Trim());
                else
                    obj.SONK = null;

                if (!txtDMNK.Text.Trim().Equals(String.Empty))
                    obj.DMNK = Convert.ToInt32(txtDMNK.Text.Trim());
                else
                    obj.DMNK = null;

                if (!txtNGAYCD.Text.Trim().Equals(String.Empty))
                    obj.NGAYDK = DateTimeUtil.GetVietNamDate(txtNGAYCD.Text.Trim());
                else
                    obj.NGAYDK = null;

                if (!txtNGAYKS.Text.Trim().Equals(String.Empty))
                    obj.NGAYHKS = DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());
                else
                    obj.NGAYHKS = null;

                obj.CMND = txtCMND.Text.Trim();
                obj.MST = txtMST.Text.Trim();

                obj.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked)
                {
                    obj.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    obj.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                }
                else
                {
                    obj.TENKH_INHOADON = "";
                    obj.DIACHI_INHOADON = "";
                }

                obj.ISTUYENONGCHUNG = cbISTUYENONGCHUNG.Checked;

                return obj;
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
                Authenticate(Functions.KH_TraCuuDonLapMoi, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_KH_TRACUUTRANGTHAI;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_TRACUUTRANGTHAI;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
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

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        #endregion



        private void BindDataForGrid()
        {
            try
            {
                /*if (Filtered == FilteredMode.None)
                {
                    var objList = ddkDao.GetListForTcdldm();
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {*/
                    int? sohodn = null;
                    int? sonk = null;
                    int? dmnk = null;
                    
                    // ReSharper disable EmptyGeneralCatchClause
                    try { sohodn = Convert.ToInt32(txtSOHODN.Text.Trim()); } catch { }
                    try { sonk = Convert.ToInt32(txtSONK.Text.Trim()); } catch { }
                    try { dmnk = Convert.ToInt32(txtDMNK.Text.Trim()); } catch { }
                    // ReSharper restore EmptyGeneralCatchClause

                    //hien theo phong ban, khu vuc
                    var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                    if (loginInfo == null) return;
                    string b = loginInfo.Username;
                    var query = _nvDao.Get(b);//nhan vien khu vuc ??

                    if (query.MAPB == "NB" || query.MAPB == "TA" || query.MAPB == "TD")
                    {

                        var objList = ddkDao.GetListForTcdldmPB(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                        txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                        ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue,query.MAPB.ToString());

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                    else
                    {
                        var objList = ddkDao.GetListForTcdldm(txtMADDK.Text.Trim(), txtTENKH.Text.Trim(), txtSONHA.Text.Trim(), txtDIENTHOAI.Text.Trim(), txtMADP.Text.Trim(),
                                        txtDIACHIKHAC.Text.Trim(), sohodn, sonk, dmnk,
                                        ddlMUCDICH.SelectedValue, ddlKHUVUC.SelectedValue, ddlPHUONG.SelectedValue);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }
                //}
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;

                var khuvuclist = kvDao.GetList();

                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in khuvuclist)
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));

                LoadDynamicReferences();

                // bind dllMDSD
                var mdsdList = mdsdDao.GetList();
                ddlMUCDICH.Items.Clear();
                ddlMUCDICH.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var mdsd in mdsdList)
                    ddlMUCDICH.Items.Add(new ListItem(mdsd.TENMDSD, mdsd.MAMDSD));

                ddlMUCDICH.SelectedIndex = 1;

                txtMADDK.Text = "";
                txtMADDK.Focus();

                txtNGAYCD.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYKS.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadDynamicReferences()
        {
            // bind dllPHUONG
            var items = pDao.GetList(ddlKHUVUC.SelectedValue);

            ddlPHUONG.Items.Clear();
            ddlPHUONG.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var p in items)
                ddlPHUONG.Items.Add(new ListItem(p.TENPHUONG, p.MAPHUONG));
        }

        private bool IsDataValid()
        {
            #region check id
            // check MADDK
            if (string.Empty.Equals(txtMADDK.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn đăng ký"), txtMADDK.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtMADP.Text.Trim()))
            {
                var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

                if (dp == null)
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đường phố"), txtMADP.ClientID);
                    return false;
                }
            }
            #endregion

            #region check integer
            if (!string.Empty.Equals(txtSOHODN.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtSOHODN.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Số hộ đấu nối"), txtSOHODN.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtSONK.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtSONK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Số nhân khẩu"), txtSONK.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtDMNK.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtDMNK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Định mức / NK"), txtDMNK.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtDINHMUC.Text.Trim()))
            {
                try
                {
                    Convert.ToInt32(txtDINHMUC.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_NUMBER, "Định mức"), txtDINHMUC.ClientID);
                    return false;
                }
            }

            #endregion

            #region check datetime
            // check datetime textboxes
            if (!string.Empty.Equals(txtNGAYCD.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYCD.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày nhận đơn"), txtNGAYCD.ClientID);
                    return false;
                }
            }

            // check datetime textboxes
            if (!string.Empty.Equals(txtNGAYKS.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày hẹn khảo sát"), txtNGAYKS.ClientID);
                    return false;
                }
            }

            if (!string.Empty.Equals(txtNGAYCD.Text.Trim()) && !string.Empty.Equals(txtNGAYKS.Text.Trim()))
            {
                var ngaycd = DateTimeUtil.GetVietNamDate(txtNGAYCD.Text.Trim());
                var ngayks = DateTimeUtil.GetVietNamDate(txtNGAYKS.Text.Trim());

                if (ngayks < ngaycd)
                {
                    ShowError("Ngày hẹn khảo sát phải sau ngày nhận đơn", txtNGAYKS.ClientID);
                    return false;
                }
            }
            #endregion

            return true;
        }

        private void ClearContent()
        {
            //TODO: xóa UI
            txtMADDK.Text = "";
            txtMADDK.ReadOnly = false;
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtDIENTHOAI.Text = "";
            txtMADP.Text = "";
            txtSOHODN.Text = "";
            txtSONK.Text = "";
            txtDMNK.Text = "";
            txtNGAYCD.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNGAYKS.Text = DateTime.Now.ToString("dd/MM/yyyy");
            cbDAIDIEN.Checked = false;
            ddlKHUVUC.SelectedIndex = 0;
            LoadDynamicReferences();
            ddlPHUONG.SelectedIndex = 0;
            ddlMUCDICH.SelectedIndex = 0;
            txtDIACHIKHAC.Text = "";

            txtCMND.Text = "";
            txtMST.Text = "";
            cbSDInfo_INHOADON.Checked = false;
            txtDIACHI_INHOADON.Text = "";
            txtTENKH_INHOADON.Text = "";

            cbISTUYENONGCHUNG.Checked = false;
        }



        private void SetDDKToForm(DONDANGKY ddk)
        {
            SetControlValue(txtMADDK.ClientID, ddk.MADDK);
            SetReadonly(txtMADDK.ClientID, true);

            SetControlValue(txtTENKH.ClientID, ddk.TENKH);
            SetControlValue(txtSONHA.ClientID, ddk.SONHA);
            SetControlValue(txtDIENTHOAI.ClientID, ddk.DIENTHOAI);
            SetControlValue(txtDIACHIKHAC.ClientID, ddk.TEN_DC_KHAC);

            if (ddk.DUONGPHO != null)
            {
                SetControlValue(txtMADP.ClientID, ddk.MADP);
                SetControlValue(txtDUONGPHU.ClientID, ddk.DUONGPHU);
            }

            SetControlValue(txtSOHODN.ClientID, ddk.SOHODN.HasValue ? String.Format("{0:0,0}", ddk.SOHODN.Value) : "");
            SetControlValue(txtSONK.ClientID, ddk.SONK.HasValue ? String.Format("{0:0,0}", ddk.SONK.Value) : "");
            SetControlValue(txtDMNK.ClientID, ddk.DMNK.HasValue ? String.Format("{0:0,0}", ddk.DMNK.Value) : "");

            SetControlValue(txtNGAYCD.ClientID, ddk.NGAYDK.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYDK.Value) : "");
            SetControlValue(txtNGAYKS.ClientID, ddk.NGAYHKS.HasValue ? String.Format("{0:dd/MM/yyyy}", ddk.NGAYHKS.Value) : "");

            var kv = ddlKHUVUC.Items.FindByValue(ddk.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            LoadDynamicReferences();

            var p = ddlPHUONG.Items.FindByValue(ddk.MAPHUONG);
            if (p != null)
                ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(p);

            var mdsd = ddlMUCDICH.Items.FindByValue(ddk.MAMDSD);
            if (mdsd != null)
                ddlMUCDICH.SelectedIndex = ddlMUCDICH.Items.IndexOf(mdsd);

            txtCMND.Text = ddk.CMND;
            txtMST.Text = ddk.MST;

            var isChecked = ddk.SDInfo_INHOADON.HasValue && ddk.SDInfo_INHOADON.Value;
            cbSDInfo_INHOADON.Checked = isChecked;
            if (isChecked)
            {
                txtTENKH_INHOADON.Text = ddk.TENKH_INHOADON;
                txtDIACHI_INHOADON.Text = ddk.DIACHI_INHOADON;
            }
            else
            {
                txtTENKH_INHOADON.Text = "";
                txtDIACHI_INHOADON.Text = "";
            }

            var isCheckedONGCAI = ddk.ISTUYENONGCHUNG.HasValue && ddk.ISTUYENONGCHUNG.Value;
            cbISTUYENONGCHUNG.Checked = isCheckedONGCAI;

            if (ddk.TENDK != null)
            { txtUYQUYEN.Text = ddk.TENDK; }

            upnlInfor.Update();
        }
        
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "showDKStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKy = ddkDao.Get(madon);
                            
                            upnlDangKy.Update();
                            UnblockDialog("divDangKy");

                            CloseWaitingDialog();
                        }

                        break;

                    case "showTKStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKy = ddkDao.Get(madon);
                            ThietKe = tkDao.Get(madon);

                            upnlThietKe.Update();
                            UnblockDialog("divThietKe");

                            CloseWaitingDialog();
                        }

                        break;

                    case "showCTStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKy = ddkDao.Get(madon);
                            ThietKe = tkDao.Get(madon);
                            ChietTinh = ctDao.Get(madon);

                            if(ChietTinh != null)
                                txtGHICHUCT.Text = ChietTinh.GHICHU;

                            upnlChietTinh.Update();
                            UnblockDialog("divChietTinh");

                            CloseWaitingDialog();
                        }

                        break;

                    case "showHDStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKy = ddkDao.Get(madon);
                            ThietKe = tkDao.Get(madon);
                            ChietTinh = ctDao.Get(madon);
                            HopDong = hdDao.Get(madon);
                            
                            upnlHopDong.Update();
                            UnblockDialog("divHopDong");

                            CloseWaitingDialog();
                        }

                        break;

                    case "showTCStatus":
                        if (!string.Empty.Equals(madon))
                        {
                            DonDangKy = ddkDao.Get(madon);
                            ThietKe = tkDao.Get(madon);
                            ChietTinh = ctDao.Get(madon);
                            HopDong = hdDao.Get(madon);
                            ThiCong = tcDao.Get(madon);
                            
                            upnlThiCong.Update();
                            UnblockDialog("divThiCong");

                            CloseWaitingDialog();
                        }

                        break;

                    case "EditItem":
                        if (!string.Empty.Equals(madon))
                        {
                            var don = ddkDao.Get(madon);
                            if (don == null) return;

                            SetDDKToForm(don);
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

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var source = gvList.DataSource as List<DONDANGKY>;
            if (source == null) return;

            var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;

            var imgDK = e.Row.FindControl("imgDK") as Button;
            var imgTK = e.Row.FindControl("imgTK") as Button;
            var imgCT = e.Row.FindControl("imgCT") as Button;
            var imgHD = e.Row.FindControl("imgHD") as Button;
            var imgTC = e.Row.FindControl("imgTC") as Button;

            if (imgDK != null)
            {
                imgDK.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgDK) + "')");
                var maTTDK = source[index].TTDK;
                var ttdk = ttDao.Get(maTTDK);

                if(ttdk != null)
                {
                    imgDK.Attributes.Add("class", ttdk.COLOR);
                    imgDK.ToolTip = ttdk.TENTT;
                } 
                else
                {
                    imgDK.ToolTip = "Chưa duyệt khảo sát";
                    imgDK.Attributes.Add("class", "noneIndicator");
                }
            }
            
            if (imgTK != null)
            {
                imgTK.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTK) + "')");
                var maTTTK = source[index].TTTK;
                var tttk = ttDao.Get(maTTTK);

                if (tttk != null)
                {
                    imgTK.Attributes.Add("class", tttk.COLOR);
                    imgTK.ToolTip = tttk.TENTT;
                }
                else
                {
                    imgTK.ToolTip = "Chưa nhập thiết kế";
                    imgTK.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgCT != null)
            {
                imgCT.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgCT) + "')");
                var maTTCT = source[index].TTCT;
                var ttct = ttDao.Get(maTTCT);

                if (ttct != null)
                {
                    imgCT.Attributes.Add("class", ttct.COLOR);
                    imgCT.ToolTip = ttct.TENTT;
                }
                else
                {
                    imgCT.ToolTip = "Chưa lập chiết tính";
                    imgCT.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgHD != null)
            {
                imgHD.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgHD) + "')");
                var maTTHD = source[index].TTHD;
                var tthd = ttDao.Get(maTTHD);

                if (tthd != null)
                {
                    imgHD.Attributes.Add("class", tthd.COLOR);
                    imgHD.ToolTip = tthd.TENTT;
                }
                else
                {
                    imgHD.ToolTip = "Chưa nhập hợp đồng";
                    imgHD.Attributes.Add("class", "noneIndicator");
                }
            }

            if (imgTC != null)
            {
                imgTC.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTC) + "')");
                var maTTTC = source[index].TTTC;
                var tttc = ttDao.Get(maTTTC);

                if (tttc != null)
                {
                    imgTC.Attributes.Add("class", tttc.COLOR);
                    imgTC.ToolTip = tttc.TENTT;
                }
                else
                {
                    imgTC.ToolTip = "Chưa nhập thi công";
                    imgTC.Attributes.Add("class", "noneIndicator");
                }
            }

            if (e.Row.Cells.Count < 5)
                return;
            var fifth = e.Row.Cells[e.Row.Cells.Count - 1];
            var fourth = e.Row.Cells[e.Row.Cells.Count - 2];
            var third = e.Row.Cells[e.Row.Cells.Count - 3];
            var second = e.Row.Cells[e.Row.Cells.Count - 4];
            var first = e.Row.Cells[e.Row.Cells.Count - 5];

            if (fifth == null || fourth == null || second == null ||
                third == null || first == null)
                return;

            fifth.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");
            fourth.Attributes.Add("style", "border-left: none 0px; border-right: none 0px; padding: 6px 0 4px !important;");
            third.Attributes.Add("style", "border-left: none 0px; border-right: none 0px; padding: 6px 0 4px !important;");
            second.Attributes.Add("style", "border-left: none 0px; border-right: none 0px; padding: 6px 0 4px !important;");
            first.Attributes.Add("style", "border-right: none 0px; padding: 6px 0 4px !important;");

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }



        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                LoadDynamicReferences();

                txtMADP.Text = "";
                txtDUONGPHU.Text = "";
                txtDIACHIKHAC.Text = "";

                CloseWaitingDialog();
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            // Authenticate
            if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Update))
            {
                CloseWaitingDialog();
                ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            var don = DonDangKyObj;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }

            Filtered = FilteredMode.None;

            var msg = ddkDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

            CloseWaitingDialog();

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                ShowInfor(ResourceLabel.Get(msg));

                //Trả lại màn hình trống ban đầu
                ClearContent();

                // Refresh grid view
                BindDataForGrid();

                upnlGrid.Update();

            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            Filtered = FilteredMode.None;

            BindDataForGrid();
            upnlGrid.Update();

            CloseWaitingDialog();
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                Filtered = FilteredMode.None;

                // Authenticate
                if (!HasPermission(Functions.KH_TraCuuDonLapMoi, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKY>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (!ddkDao.IsInUse(ma)) continue;

                        var ddk = ddkDao.Get(ma);
                        var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đơn đăng ký", ddk.TENKH);

                        CloseWaitingDialog();
                        ShowError(ResourceLabel.Get(msgIsInUse));
                        return;
                    }

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    //TODO: check relation before update list
                    var msg = ddkDao.DoAction(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

                    if (msg.MsgType != MessageType.Error)
                    {
                        CloseWaitingDialog();

                        ShowInfor(ResourceLabel.Get(msg));

                        ClearContent();

                        // Refresh grid view
                        BindDataForGrid();

                        upnlGrid.Update();
                    }
                    else
                    {
                        CloseWaitingDialog();

                        ShowError(ResourceLabel.Get(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }



        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetControlValue(txtDIACHIKHAC.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
                LoadDynamicReferences();
            }
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMADP.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());

            if (dp != null)
            {
                UpdateKhuVuc(dp);
                CloseWaitingDialog();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Mã đường phố không hợp lệ");
            }
        }

        private void BindDuongPho()
        {
            var list = dpDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var res = id.Split('-');
                        var dp = dpDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);

                            UpdateKhuVuc(dp);
                            upnlInfor.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();

                        }

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDuongPho.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        
    }
}