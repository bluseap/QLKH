using System;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe.Power
{
    public partial class NhapHopDongPo : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();                
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly MucDichSuDungPoDao _mdsdpoDao = new MucDichSuDungPoDao();
        private readonly HinhThucThanhToanDao htttDao = new HinhThucThanhToanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();


        #region co up, loc
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

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
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

        private HOPDONGPO HopDongPo
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var don = _ddkpoDao.Get(txtMADDK.Text.Trim());
                if (don == null)
                    return null;

                //var MADP2 = txtMADB.Text.Trim().Substring(0,4);
                //var MADB2 = txtMADB.Text.Trim().Substring(4,4);

                var hopdong = new HOPDONGPO
                {
                    MADDKPO = txtMADDK.Text.Trim(),

                    //SOHD = txtSOHD.Text.Trim(),hdDao.NewId();
                    //SOHD = _hdpoDao.NewId().ToString(),
                    //MADB = "",
                    //MADP = txtDUONGPHO.Text.Trim(),
                    //MADB = txtMADB.Text.Trim(),  
                    MADPPO = ddlKHUVUC.SelectedValue == "J" ? "JA01" : txtMADB.Text.Trim().Substring(0, 4),
                    MADB = ddlKHUVUC.SelectedValue == "J" ? "0000" : txtMADB.Text.Trim().Substring(4, 4),

                    LOTRINH = 0,

                    SONHA = txtSONHA.Text.Trim(),
                    //SONHA = txtSONHA.Text.Trim().Substring(0,29),

                    MAPHUONGPO = ddlPHUONG.SelectedValue,
                    MAKVPO = ddlKHUVUC.SelectedValue,
                    CODH = ddlKICHCODH.SelectedValue,
                    LOAIONG = txtLOAIONG.Text.Trim(),
                    MAHTTT = ddlHTTT.SelectedValue,
                    MAMDSDPO = ddlMDSD.SelectedValue,
                    DINHMUCSD = Int32.Parse(txtDMSD.Text.Trim()),
                    SOHO = Int32.Parse(txtSOHO.Text.Trim()),
                    SONHANKHAU = Int32.Parse(txtSONK.Text.Trim()),
                    LOAIHD = ddlLOAIHD.SelectedValue,
                    TRANGTHAI = 0,
                    CMND = txtCMND.Text.Trim(),
                    MST = txtMST.Text.Trim(),
                    DACAPDB = false
                };

                if (!string.IsNullOrEmpty(txtMADP.Text.Trim()))
                {
                    hopdong.MADPPO = txtMADP.Text.Trim();
                    hopdong.DUONGPHUPO = txtDUONGPHU.Text.Trim();
                }

                if (!txtNGAYTAO.Text.Trim().Equals(String.Empty))
                    hopdong.NGAYTAO = DateTimeUtil.GetVietNamDate(txtNGAYTAO.Text.Trim());

                //if (!txtNGAYKT.Text.Trim().Equals(String.Empty))
                //    hopdong.NGAYKT = DateTimeUtil.GetVietNamDate(txtNGAYKT.Text.Trim());

                if (!txtNGAYHL.Text.Trim().Equals(String.Empty))
                    hopdong.NGAYHL = DateTimeUtil.GetVietNamDate(txtNGAYHL.Text.Trim());

                hopdong.SDInfo_INHOADON = cbSDInfo_INHOADON.Checked;
                if (cbSDInfo_INHOADON.Checked)
                {
                    hopdong.TENKH_INHOADON = txtTENKH_INHOADON.Text.Trim();
                    hopdong.DIACHI_INHOADON = txtDIACHI_INHOADON.Text.Trim();
                }
                else
                {
                    hopdong.TENKH_INHOADON = "";
                    hopdong.DIACHI_INHOADON = "";
                }

                hopdong.NGAYN = DateTime.Now;

                return hopdong;
            }
        }        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_NhapHopDongPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();

                    ChayChietTinhPo();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.PAGE_TK_NHAPHOPDONGPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.TITLE_TK_NHAPHOPDONGPO;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDDK);
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void ChayChietTinhPo()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

                _rpClass.DonToKeToan("", makvpo, "", "", "", "", "UPCTKTTOCTKHPO");
            }
            catch { }
        }

        private void LoadStaticReferences()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                UpdateMode = Mode.Create;

                var list = _kvpoDao.GetListKVPO(_nvDao.Get(b).MAKV);
                ddlMaKV.Items.Clear();
                //ddlMaKV.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in list)
                    ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));

                ddlKHUVUC.DataSource = list;
                ddlKHUVUC.DataTextField = "TENKV";
                ddlKHUVUC.DataValueField = "MAKVPO";
                ddlKHUVUC.DataBind();

                var listHTTT = htttDao.GetList();
                ddlHTTT.DataSource = listHTTT;
                ddlHTTT.DataTextField = "MOTA";
                ddlHTTT.DataValueField = "MAHTTT";
                ddlHTTT.DataBind();

                var listMDSD = _mdsdpoDao.GetList();
                ddlMDSD.DataSource = listMDSD;
                ddlMDSD.DataTextField = "TENMDSD";
                ddlMDSD.DataValueField = "MAMDSDPO";
                ddlMDSD.DataBind();

                txtSOHD.Text = "";// _hdpoDao.NewId();

                txtNGAYTAO.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYHL.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtNGAYKT.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtLOAIONG.Text = "2x7 mm2";
                txtDMSD.Text = "1";
                txtSOHO.Text = "1";
                txtSONK.Text = "1";

                LoadDynamicReferences(ddlKHUVUC.SelectedValue);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadDynamicReferences(string makv)
        {
            // load phuong
            var listPhuong = new PhuongPoDao().GetList(makv);
            ddlPHUONG.DataSource = listPhuong;
            ddlPHUONG.DataTextField = "TENPHUONG";
            ddlPHUONG.DataValueField = "MAPHUONGPO";
            ddlPHUONG.DataBind();
        }

        private void BindDataForGrid()
        {
            try
            {
                var list = _hdpoDao.GetList("", false, ddlKHUVUC.SelectedValue);
                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString();
                gvList.DataBind();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private bool IsDataValid()
        {
            #region Mã đơn, số hợp đồng
            if (string.Empty.Equals(txtMADDK.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADDK.ClientID);
                return false;
            }

            /*
            var existed = _hdpoDao.Get(txtMADDK.Text.Trim());
            if (existed != null && existed.DONDANGKYPO.TTHD.Equals(TTHD.HD_P))
            {
                ShowError("Mã đơn đã tồn tại", txtMADDK.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtSOHD.Text.Trim())) {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Số hợp đồng"), txtSOHD.ClientID);
                return false;
            }

            
            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim(), ddlKHUVUC.SelectedValue);
            if (dp == null)
            {
                ShowError(String.Format(Resources.Message.E0023, "Mã đường phố và khu vực"), txtMADP.ClientID);
                return false;
            }
            */

            // check độ dài mã danh bộ = 4
            /*if (txtMADB.Text.Trim().Length != 0 && txtMADB.Text.Trim().Length != 4)
            {
                ShowError("Độ dài mã danh bộ phải là 4.", txtMADB.ClientID);
                return false;
            }
            */
            #endregion

            #region Định mức, số hộ, nhân khẩu, tỷ lệ
            try
            {
                Int32.Parse(txtDMSD.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E0023, "Định mức sử dụng"), txtDMSD.ClientID);
                return false;
            }

            try
            {
                Int32.Parse(txtSOHO.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E0023, "Số hộ"), txtSOHO.ClientID);
                return false;
            }

            try
            {
                Int32.Parse(txtSONK.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E0023, "Số nhân khẩu"), txtSONK.ClientID);
                return false;
            }
            #endregion

            #region Ngày tháng
            try
            {
                if (txtNGAYTAO.Text.Trim() != "")
                    DateTimeUtil.GetVietNamDate(txtNGAYTAO.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E0023, "Ngày làm hợp đồng"), txtNGAYTAO.ClientID);
                return false;
            }

            try
            {
                if (txtNGAYHL.Text.Trim() != "")
                    DateTimeUtil.GetVietNamDate(txtNGAYHL.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E0023, "Ngày hiệu lực"), txtNGAYHL.ClientID);
                return false;
            }
            /*
                        try {
                            if (txtNGAYKT.Text.Trim() != "")
                                DateTimeUtil.GetVietNamDate(txtNGAYKT.Text.Trim());
                        }
                        catch {
                            ShowError(String.Format(Resources.Message.E0023, "Ngày kết thúc"), txtNGAYKT.ClientID);
                            return false;
                        }
            */
            #endregion

            return true;
        }

        private void ClearContent()
        {
            txtMADDK.Text = "";
            txtMADDK.ReadOnly = false;
            txtSOHD.Text = "";
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtDUONGPHU.Text = "";
            txtTENDUONG.Text = "";
            ddlLOAIHD.SelectedIndex = 0;
            ddlHTTT.SelectedIndex = 0;
            ddlKICHCODH.SelectedIndex = 0;
            txtLOAIONG.Text = "";
            ddlMDSD.SelectedIndex = 0;
            txtDMSD.Text = "1";
            txtSOHO.Text = "1";
            txtSONK.Text = "1";
            txtNGAYTAO.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNGAYHL.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtNGAYKT.Text = DateTime.Now.ToString("dd/MM/yyyy");

            txtCMND.Text = "";
            txtMST.Text = "";
            cbSDInfo_INHOADON.Checked = false;
            txtTENKH_INHOADON.Text = "";
            txtDIACHI_INHOADON.Text = "";

            txtDUONGPHO.Text = "";
            lbDUONGPHO.Text = "";
            txtMADP.Text = "";
            txtMADB.Text = "";

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlKHUVUC.SelectedValue != "J")
                {
                    if (string.Empty.Equals(txtMADB.Text.Trim()) || txtMADB.Text.Trim().Length != 8)
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Nhập danh bộ bằng 8 ký tự. Ví du: IA0A0002"), txtMADB.ClientID);

                        CloseWaitingDialog();
                        return;
                    }
                }

                var hopdongpo = HopDongPo;
                if (hopdongpo == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                
                Message msg, msghdtcpo;                

                if (UpdateMode.Equals(Mode.Create))
                {
                    if (!HasPermission(Functions.TK_NhapHopDongPo, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }
                    try
                    {
                        var tontai = _hdpoDao.Get(hopdongpo.MADDKPO);
                        if (tontai != null)
                        {
                            CloseWaitingDialog();
                            ShowError("Mã đơn đăng ký đã tồn tại", txtMADDK.ClientID);
                            return;
                        }
                    }
                    catch  { }

                    string maddkpo = hopdongpo.MADDKPO;

                    hopdongpo.SOHD = _hdpoDao.NewId();

                    msg = _hdpoDao.Insert(hopdongpo, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    //var dondangky = _ddkpoDao.Get(hopdongpo.MADDKPO);
                    //if (dondangky != null)
                    //{
                    //    dondangky.TTHD = "HD_A";// TTHD.HD_A.ToString();
                    //    dondangky.TTTC = "TC_N";// TTTC.TC_N.ToString();

                    //    msghdtcpo = _ddkpoDao.UpdateHDTC(dondangky);
                    //}

                    msghdtcpo = _ddkpoDao.UpdateHDTCMoi(txtMADDK.Text.Trim(), "");

                    _rpClass.HisNgayDangKyBienPo(maddkpo, LoginInfo.MANV, hopdongpo.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "INHOPDONG");
                }
                else
                {
                    if (!HasPermission(Functions.TK_NhapHopDongPo, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    hopdongpo.SOHD = txtSOHD.Text.Trim();

                    msg = _hdpoDao.Update(hopdongpo, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    //msghdtcpo = _ddkpoDao.UpdateHDTCMoi(txtMADDK.Text.Trim(), "");

                    _rpClass.HisNgayDangKyBienPo(hopdongpo.MADDKPO, LoginInfo.MANV, hopdongpo.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "UPHOPDONG");
                }

                CloseWaitingDialog();

                if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    // Refresh grid view
                    BindDataForGrid();
                    ClearContent();
                    UpdateMode = Mode.Create;
                    upnlGrid.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch { }
        }

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDynamicReferences(ddlKHUVUC.SelectedValue);

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            txtTENDUONG.Text = "";

            CloseWaitingDialog();
        }

        private void SetUpdateForm(HOPDONGPO obj)
        {
            UpdateMode = Mode.Update;

            // set control's value
            txtMADDK.Text = obj.MADDKPO.ToString();
            SetControlValue(txtTENKH.ClientID, obj.DONDANGKYPO != null ? obj.DONDANGKYPO.TENKH : "");
            txtSOHD.Text = obj.SOHD != null ? obj.SOHD : "";

            var kv = ddlKHUVUC.Items.FindByValue(obj.MAKVPO);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

                var listPhuong = _ppoDao.GetList(obj.MAKVPO);
                ddlPHUONG.DataSource = listPhuong;
                ddlPHUONG.DataTextField = "TENPHUONG";
                ddlPHUONG.DataValueField = "MAPHUONGPO";
                ddlPHUONG.DataBind();

                var ph = ddlPHUONG.Items.FindByValue(obj.MAPHUONGPO);
                if (ph != null)
                {
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(ph);
                }
            }

            if (obj.DUONGPHOPO != null)
            {
                SetControlValue(txtMADP.ClientID, obj.MADPPO);
                SetControlValue(txtDUONGPHU.ClientID, "");
                SetLabel(lblTENDUONG.ClientID, obj.DUONGPHOPO.TENDP);
            }

            SetControlValue(txtSONHA.ClientID, obj.SONHA);
            SetControlValue(txtTENDUONG.ClientID, obj.DONDANGKYPO != null ? obj.DONDANGKYPO.TEN_DC_KHAC : "");

            SetControlValue(ddlLOAIHD.ClientID, obj.LOAIHD);
            SetControlValue(ddlHTTT.ClientID, obj.MAHTTT);
            SetControlValue(ddlKICHCODH.ClientID, obj.CODH);
            SetControlValue(ddlMDSD.ClientID, obj.MAMDSDPO);

            SetControlValue(txtLOAIONG.ClientID, obj.LOAIONG);
            SetControlValue(txtDMSD.ClientID, obj.DINHMUCSD.HasValue ? obj.DINHMUCSD.Value.ToString() : "");
            SetControlValue(txtSOHO.ClientID, obj.SOHO.HasValue ? obj.SOHO.Value.ToString() : "");
            SetControlValue(txtSONK.ClientID, obj.SONHANKHAU.HasValue ? obj.SONHANKHAU.Value.ToString() : "");

            SetControlValue(txtNGAYTAO.ClientID, obj.NGAYTAO.HasValue ? obj.NGAYTAO.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtNGAYHL.ClientID, obj.NGAYHL.HasValue ? obj.NGAYHL.Value.ToString("dd/MM/yyyy") : "");
            //SetControlValue(txtNGAYKT.ClientID, obj.NGAYKT.HasValue ? obj.NGAYKT.Value.ToString("dd/MM/yyyy") : "");

            SetControlValue(txtCMND.ClientID, obj.CMND);
            SetControlValue(txtMST.ClientID, obj.MST);

            var isChecked = obj.SDInfo_INHOADON.HasValue && obj.SDInfo_INHOADON.Value;

            cbSDInfo_INHOADON.Checked = isChecked;

            if (isChecked)
            {
                SetControlValue(txtTENKH_INHOADON.ClientID, obj.TENKH_INHOADON);
                SetControlValue(txtDIACHI_INHOADON.ClientID, obj.DIACHI_INHOADON);
            }
            else
            {
                SetControlValue(txtTENKH_INHOADON.ClientID, "");
                SetControlValue(txtDIACHI_INHOADON.ClientID, "");
            }

            if (obj.MADPPO != null)
            {
                txtMADB.Text = obj.MADPPO + obj.MADB;
            }

            /*if (obj.MADB != null)
            {
                txtMADB.Text = obj.MADB;
            }*/

        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = _hdpoDao.Get(id);
                        if (obj == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }

                        SetUpdateForm(obj);
                        upnlInfor.Visible = true;
                        CloseWaitingDialog();
                        upnlInfor.Update();

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

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        private void BindDuongPho()
        {
            var list = _dppoDao.GetList(ddlKHUVUC.SelectedValue, txtKeywordDP.Text.Trim());
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

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var res = id.Split('-');
                        var dp = _dppoDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            //SetControlValue(txtMADP.ClientID, dp.MADP);
                            //SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);
                            //UpdateKhuVuc(dp);
                            txtDUONGPHO.Text = dp.MADPPO;
                            lbDUONGPHO.Text = dp.TENDP;
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

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        private void UpdateKhuVuc(DUONGPHOPO dp)
        {
            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKVPO);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
                LoadDynamicReferences(dp.MAKVPO);
            }
        }

        private void BindToInfor(DONDANGKYPO obj)
        {
            //SetControlValue(txtMADDK.ClientID, obj.MADDK);
            txtMADDK.Text = obj.MADDKPO;
            //SetReadonly(txtMADP.ClientID, true);
            txtSOHD.Text = _hdpoDao.NewId();
            txtTENKH.Text = obj.TENKH.ToString();

            String sn2 = "";
            String sn = "";
            sn2 = obj.SONHA.ToString();
            if (sn2.Length < 30)
            {
                SetControlValue(txtSONHA.ClientID, sn2);
            }
            else
            {
                sn = sn2.Substring(0, 29);
                SetControlValue(txtSONHA.ClientID, sn.Trim());
            }
            //SetControlValue(txtSONHA.ClientID, sn.Trim());
            //SetControlValue(txtSONHA.ClientID, obj.SONHA.ToString());

            var kv = ddlKHUVUC.Items.FindByValue(obj.MAKVPO);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

                var listPhuong = _ppoDao.GetList(obj.MAKVPO);
                ddlPHUONG.DataSource = listPhuong;
                ddlPHUONG.DataTextField = "TENPHUONG";
                ddlPHUONG.DataValueField = "MAPHUONGPO";
                ddlPHUONG.DataBind();

                var ph = ddlPHUONG.Items.FindByValue(obj.MAPHUONG);
                if (ph != null)
                {
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(ph);
                }
            }

            SetControlValue(txtTENDUONG.ClientID, obj.TEN_DC_KHAC);


            if (obj.DUONGPHOPO != null)
            {
                SetControlValue(txtMADP.ClientID, obj.MADPPO);
                SetControlValue(txtDUONGPHU.ClientID, "");
                SetLabel(lblTENDUONG.ClientID, obj.DUONGPHOPO.TENDP);
            }

            ddlLOAIHD.SelectedIndex = 0;
            ddlHTTT.SelectedIndex = 0;

            var mdsd = ddlMDSD.Items.FindByValue(obj.MAMDSDPO);
            if (mdsd != null)
            {
                ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(mdsd);
            }

            SetControlValue(ddlKICHCODH.ClientID, "15");
            SetControlValue(txtLOAIONG.ClientID, "2x7 mm2");
            SetControlValue(txtDMSD.ClientID, obj.DMNK.HasValue ? obj.DMNK.Value.ToString() : "0");
            SetControlValue(txtSOHO.ClientID, obj.SOHODN.HasValue ? obj.SOHODN.Value.ToString() : "1");
            SetControlValue(txtSONK.ClientID, obj.SONK.HasValue ? obj.SONK.Value.ToString() : "0");
            SetControlValue(txtCMND.ClientID, obj.CMND);
            SetControlValue(txtMST.ClientID, obj.MST);

            var isChecked = obj.SDInfo_INHOADON.HasValue && obj.SDInfo_INHOADON.Value;

            cbSDInfo_INHOADON.Checked = isChecked;

            if (isChecked)
            {
                SetControlValue(txtTENKH_INHOADON.ClientID, obj.TENKH_INHOADON);
                SetControlValue(txtDIACHI_INHOADON.ClientID, obj.DIACHI_INHOADON);
            }
            else
            {
                SetControlValue(txtTENKH_INHOADON.ClientID, "");
                SetControlValue(txtDIACHI_INHOADON.ClientID, "");
            }

            var sdb = _tkpoDao.Get(obj.MADDKPO);
            if (sdb != null)
            {
                if (sdb.SODB != null)
                {
                    txtMADB.Text = sdb.SODB.ToString();
                }
                else { txtMADB.Text = ""; }
            }
            else { txtMADB.Text = ""; }

            upnlInfor.Update();
        }

        protected void btnBrowseDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            upnlDonDangKy.Update();
            UnblockDialog("divDonDangKy");
        }

        private void BindDDK()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            string makv = _nvDao.Get(b).MAKV;

            DateTime? tungay = null;
            DateTime? denngay = null;
            try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
            catch { txtTuNgay.Text = ""; }
            try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
            catch { txtDenNgay.Text = ""; }

            if (makv == "S" || makv == "P" || makv == "T" || makv == "K" || makv == "L"//chau doc, phu tan, tan chau,CHOMOI, tri ton
                || makv == "M" || makv == "Q" // tinh bien,an phu
                )
            {
                var list = _ddkpoDao.GetListDaDuyetTK_CD(txtFilter.Text.Trim(), tungay, denngay, null, ddlMaKV.SelectedValue);

                gvDDK.DataSource = list;
                gvDDK.PagerInforText = list.Count.ToString();
                gvDDK.DataBind();
            }
            else
            {
                //var list = _ddkpoDao.GetListForDonChoHopdong(txtFilter.Text.Trim(), tungay, denngay, null, ddlMaKV.SelectedValue);
                var list = _ddkpoDao.GetListDaDuyetTK_CD(txtFilter.Text.Trim(), tungay, denngay, null, ddlMaKV.SelectedValue);

                gvDDK.DataSource = list;
                gvDDK.PagerInforText = list.Count.ToString();
                gvDDK.DataBind();
            }
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
                // Update page index
                gvDDK.PageIndex = e.NewPageIndex;
                // Bind data for grid
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

                        BindToInfor(obj);
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");

                        UpdateMode = Mode.Create;

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            CloseWaitingDialog();
        }

        protected void btDUONGPHO_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void btSearch_Click(object sender, EventArgs e)
        {
            try
            {
                BindHopDongTim(txtMADDKTENKH.Text.Trim());

                upnlGrid.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        private void BindHopDongTim(string keyword)
        {
            try
            {
                var list = _hdpoDao.GetListKeyword(keyword, ddlKHUVUC.SelectedValue);
                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString();
                gvList.DataBind();

                //if (string.IsNullOrEmpty(keyword) || keyword == "")
                //{
                //    var list = _hdpoDao.GetList("", false, ddlKHUVUC.SelectedValue);
                //    gvList.DataSource = list;
                //    gvList.PagerInforText = list.Count.ToString();
                //    gvList.DataBind();
                //}
                //else
                //{
                //    var list = _hdpoDao.GetList("", false, ddlKHUVUC.SelectedValue);
                //    gvList.DataSource = list;
                //    gvList.PagerInforText = list.Count.ToString();
                //    gvList.DataBind();
                //}              

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }


    }
}