using System;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class TraCuuHopDong : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly HopDongDao hdDao = new HopDongDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly HinhThucThanhToanDao htttDao = new HinhThucThanhToanDao();
        private readonly MucDichSuDungDao mdsdDao = new MucDichSuDungDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        #region Properties
        private HOPDONG HopDong
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var don = ddkDao.Get(txtMADDK.Text.Trim());
                if (don == null)
                    return null;

                var hopdong = hdDao.Get(txtMADDK.Text.Trim());
                if (hopdong == null)
                    return null;

                hopdong.SOHD = txtSOHD.Text.Trim();                
                hopdong.LOTRINH = 0;
                hopdong.SONHA = txtSONHA.Text.Trim();
                hopdong.MAPHUONG = String.IsNullOrEmpty(ddlPHUONG.SelectedValue) ? null : ddlPHUONG.SelectedValue;
                hopdong.MAKV = ddlKHUVUC.SelectedValue;
                hopdong.CODH = ddlKICHCODH.SelectedValue;
                hopdong.LOAIONG = txtLOAIONG.Text.Trim();
                hopdong.MAHTTT = ddlHTTT.SelectedValue;
                hopdong.MAMDSD = ddlMDSD.SelectedValue;
                hopdong.DINHMUCSD = Int32.Parse(txtDMSD.Text.Trim());
                hopdong.SOHO = Int32.Parse(txtSOHO.Text.Trim());
                hopdong.SONHANKHAU = Int32.Parse(txtSONK.Text.Trim());
                hopdong.LOAIHD = ddlLOAIHD.SelectedValue;
                hopdong.TRANGTHAI = 0;
                hopdong.CMND = txtCMND.Text.Trim();
                hopdong.MST = txtMST.Text.Trim();

                hopdong.MADP = ddlKHUVUC.SelectedValue == "X" ? "XA01" : txtDANHSOHD.Text.Trim().Substring(0, 4);
                hopdong.DUONGPHU = "";
                hopdong.MADB = ddlKHUVUC.SelectedValue == "X" ? "000000" : txtDANHSOHD.Text.Trim().Substring(4, 4);
                /*if (!string.IsNullOrEmpty(txtMADP.Text.Trim()))
                {
                    hopdong.MADP = txtMADP.Text.Trim();
                    hopdong.DUONGPHU = txtDUONGPHU.Text.Trim();
                }
                else
                {
                    hopdong.MADP = null;
                    hopdong.DUONGPHU = null;
                }*/

                if (!txtNGAYTAO.Text.Trim().Equals(String.Empty))
                    hopdong.NGAYTAO = DateTimeUtil.GetVietNamDate(txtNGAYTAO.Text.Trim());

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

                return hopdong;
            }
        }

        protected String MaDDK
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_MADDK))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_MADDK].ToString());
            }
        }

        protected String TenKH
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_TENKH))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_TENKH].ToString());
            }
        }

        protected String SoNha
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_SONHA))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_SONHA].ToString());
            }
        }

        protected String SoHD
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_SOHD))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_SOHD].ToString());
            }
        }

        protected String KhuVuc
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_AREACODE))
                {
                    return null;
                }
                var mkv = EncryptUtil.Decrypt(param[Constants.PARAM_AREACODE].ToString());
                var kv = kvDao.Get(mkv);
                if (kv == null)
                    return null;

                return kv.MAKV;
            }
        }

        protected DateTime? FromDate
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_FROMDATE))
                {
                    return null;
                }
                try
                {
                    return DateTimeUtil.GetVietNamDate(EncryptUtil.Decrypt(param[Constants.PARAM_FROMDATE].ToString()));
                    //return Convert.ToDateTime(EncryptUtil.Decrypt(param[Constants.PARAM_FROMDATE].ToString()));
                }
                catch
                {
                    return null;
                }
            }
        }

        protected DateTime? ToDate
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_TODATE))
                {
                    return null;
                }
                try
                {
                    return DateTimeUtil.GetVietNamDate(EncryptUtil.Decrypt(param[Constants.PARAM_TODATE].ToString()));
                }
                catch
                {
                    return null;
                }
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

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_TraCuuHopDong, Permission.Read);
                PrepareUI();

                if(!Page.IsPostBack)
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
            Page.Title = Resources.Message.TITLE_TK_TRACUUHOPDONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_TRACUUHOPDONG;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                //var list = hdDao.GetList(MaDDK, SoHD, TenKH, SoNha, KhuVuc, FromDate, ToDate);
                var list = hdDao.GetList(MaDDK, SoHD, TenKH, SoNha, _nvDao.Get(b).MAKV, FromDate, ToDate);
                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString();
                gvList.DataBind();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadStaticReferences()
        {
            //TODO: Load các đối tượng có liên quan lên UI
            try
            {
                var list = kvDao.GetList();
                timkv();
                /*
                ddlKHUVUC.DataSource = list;
                ddlKHUVUC.DataTextField = "TENKV";
                ddlKHUVUC.DataValueField = "MAKV";
                ddlKHUVUC.DataBind();
                */
                var listHTTT = htttDao.GetList();
                ddlHTTT.DataSource = listHTTT;
                ddlHTTT.DataTextField = "MOTA";
                ddlHTTT.DataValueField = "MAHTTT";
                ddlHTTT.DataBind();

                var listMDSD = mdsdDao.GetList();
                ddlMDSD.DataSource = listMDSD;
                ddlMDSD.DataTextField = "TENMDSD";
                ddlMDSD.DataValueField = "MAMDSD";
                ddlMDSD.DataBind();

                txtSOHD.Text = hdDao.NewId();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadDynamicReferences(string makv)
        {
            // load phuong
            var listPhuong = new PhuongDao().GetList(makv);
            ddlPHUONG.DataSource = listPhuong;
            ddlPHUONG.DataTextField = "TENPHUONG";
            ddlPHUONG.DataValueField = "MAPHUONG";
            ddlPHUONG.DataBind();
        }

        private bool IsDataValid()
        {
            #region Mã đơn, số hợp đồng
            if (string.Empty.Equals(txtMADDK.Text.Trim())) {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADDK.ClientID);
                return false;
            }

            var existed = hdDao.Get(txtMADDK.Text.Trim());
            if (existed != null && existed.DONDANGKY.TTHD.Equals(TTHD.HD_P)) {
                ShowError("Mã đơn đã tồn tại", txtMADDK.ClientID);
                return false;
            }

            //if (string.Empty.Equals(txtSOHD.Text.Trim())) {
            //    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Số hợp đồng"), txtSOHD.ClientID);
            //    return false;
            //}           

            #endregion

            #region Định mức, số hộ, nhân khẩu, tỷ lệ
            try {
                Int32.Parse(txtDMSD.Text.Trim());
            }
            catch {
                ShowError(String.Format(Resources.Message.E0023, "Định mức sử dụng"), txtDMSD.ClientID);
                return false;
            }

            try {
                Int32.Parse(txtSOHO.Text.Trim());
            }
            catch {
                ShowError(String.Format(Resources.Message.E0023, "Số hộ"), txtSOHO.ClientID);
                return false;
            }

            try {
                Int32.Parse(txtSONK.Text.Trim());
            }
            catch {
                ShowError(String.Format(Resources.Message.E0023, "Số nhân khẩu"), txtSONK.ClientID);
                return false;
            }
            #endregion

            #region Ngày tháng
            try {
                if (txtNGAYTAO.Text.Trim() != "")
                    DateTimeUtil.GetVietNamDate(txtNGAYTAO.Text.Trim());
            }
            catch {
                ShowError(String.Format(Resources.Message.E0023, "Ngày làm hợp đồng"), txtNGAYTAO.ClientID);
                return false;
            }

            try {
                if (txtNGAYHL.Text.Trim() != "")
                    DateTimeUtil.GetVietNamDate(txtNGAYHL.Text.Trim());
            }
            catch {
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
            txtSOHD.Text = hdDao.NewId();
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtDUONGPHU.Text = "";
            txtTENDUONG.Text = "";
            ddlLOAIHD.SelectedIndex = 0;
            ddlHTTT.SelectedIndex = 0;
            ddlKICHCODH.SelectedIndex = 0;
            txtLOAIONG.Text = "HDPE 25";
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
            txtDANHSOHD.Text = "";
        }        
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasPermission(Functions.TK_NhapHopDong, Permission.Update))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            var hopdong = HopDong;
            if (hopdong == null)
            {
                CloseWaitingDialog();
                return;
            }

            var maddkkh = _khDao.GetMADDK(hopdong.MADDK);
            if (maddkkh != null)
            {
                CloseWaitingDialog();
                ShowInfor("Khách hàng đã khai thác. Không được sửa.");
                return;
            }

            //hopdong.SOHD = txtSOHD.Text.Trim();
            var msg = hdDao.Update(hopdong, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
            
            CloseWaitingDialog();

            if (msg == null) return;

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));

                // Refresh grid view
                BindDataForGrid();
                ClearContent();
                upnlGrid.Update();
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void ddlKHUVUC_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadDynamicReferences(ddlKHUVUC.SelectedValue);

            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            txtTENDUONG.Text = "";

            CloseWaitingDialog();
        }

        private void SetUpdateForm(HOPDONG obj)
        {
            // set control's value
            txtMADDK.Text = obj.MADDK.ToString();
            txtDANHSOHD.Text = obj.MADP.ToString() + obj.MADB.ToString();

            SetReadonly(txtMADDK.ClientID, true);
            SetControlValue(txtTENKH.ClientID, obj.DONDANGKY != null ? obj.DONDANGKY.TENKH : "");

            txtSOHD.Text = obj.SOHD;


            var kv = ddlKHUVUC.Items.FindByValue(obj.MAKV);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

                var listPhuong = pDao.GetList(obj.MAKV);
                ddlPHUONG.DataSource = listPhuong;
                ddlPHUONG.DataTextField = "TENPHUONG";
                ddlPHUONG.DataValueField = "MAPHUONG";
                ddlPHUONG.DataBind();

                var ph = ddlPHUONG.Items.FindByValue(obj.MAPHUONG);
                if (ph != null)
                {
                    ddlPHUONG.SelectedIndex = ddlPHUONG.Items.IndexOf(ph);
                }
            }

            /*if (obj.DUONGPHO != null)
            {
                SetControlValue(txtMADP.ClientID, obj.MADP);
                SetControlValue(txtDUONGPHU.ClientID, "");
                SetLabel(lblTENDUONG.ClientID, obj.DUONGPHO.TENDP);
            }*/

            SetControlValue(txtSONHA.ClientID, obj.SONHA);
            SetControlValue(txtTENDUONG.ClientID, obj.DONDANGKY != null ? obj.DONDANGKY.TEN_DC_KHAC : "");

            SetControlValue(ddlLOAIHD.ClientID, obj.LOAIHD);
            SetControlValue(ddlHTTT.ClientID, obj.MAHTTT);
            SetControlValue(ddlKICHCODH.ClientID, obj.CODH);
            SetControlValue(ddlMDSD.ClientID, obj.MAMDSD);

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
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = hdDao.Get(id);
                        if (obj == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }

                        divInfor.Visible = true;
                        SetUpdateForm(obj);
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
                gvDuongPho.PageIndex = e.NewPageIndex;
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

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetLabel(lblTENDUONG.ClientID, dp.TENDP);

            var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
                LoadDynamicReferences(dp.MAKV);
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
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlMaKV.Items.Clear();                    
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        //ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    //ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        //ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }


    }
}
