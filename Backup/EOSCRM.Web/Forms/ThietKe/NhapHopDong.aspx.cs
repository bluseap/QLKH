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
    public partial class NhapHopDong : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly HopDongDao hdDao = new HopDongDao();
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly PhuongDao pDao = new PhuongDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly HinhThucThanhToanDao htttDao = new HinhThucThanhToanDao();
        private readonly MucDichSuDungDao mdsdDao = new MucDichSuDungDao();

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

        private HOPDONG HopDong
        {
            get {
                if (!IsDataValid())
                    return null;

                var don = ddkDao.Get(txtMADDK.Text.Trim());
                if(don == null)
                    return null;

                var hopdong = new HOPDONG
                                  {
                                      MADDK = txtMADDK.Text.Trim(),
                                      SOHD = txtSOHD.Text.Trim(),
                                      //MADB = "",
                                      MADP = txtDUONGPHO.Text.Trim(),
                                      MADB = txtMADB.Text.Trim(),  
                                        
                                      LOTRINH = 0,
                                      SONHA = txtSONHA.Text.Trim(),
                                      MAPHUONG = ddlPHUONG.SelectedValue,
                                      MAKV = ddlKHUVUC.SelectedValue,
                                      CODH = ddlKICHCODH.SelectedValue,
                                      LOAIONG = txtLOAIONG.Text.Trim(),
                                      MAHTTT = ddlHTTT.SelectedValue,
                                      MAMDSD = ddlMDSD.SelectedValue,
                                      DINHMUCSD = Int32.Parse(txtDMSD.Text.Trim()),
                                      SOHO = Int32.Parse(txtSOHO.Text.Trim()),
                                      SONHANKHAU = Int32.Parse(txtSONK.Text.Trim()),
                                      LOAIHD = ddlLOAIHD.SelectedValue,
                                      TRANGTHAI = 0,
                                      CMND = txtCMND.Text.Trim(),
                                      MST = txtMST.Text.Trim(),
                                      DACAPDB = false
                                  };

                if(!string.IsNullOrEmpty(txtMADP.Text.Trim()))
                {
                    hopdong.MADP = txtMADP.Text.Trim();
                    hopdong.DUONGPHU = txtDUONGPHU.Text.Trim();
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

                return hopdong;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_NhapHopDong, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TK_NHAPHOPDONG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_NHAPHOPDONG;
            }

            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            CommonFunc.SetPropertiesForGrid(gvDDK);
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



        private void LoadStaticReferences()
        {
            try
            {
                UpdateMode = Mode.Create;

                var list = kvDao.GetList();

                ddlMaKV.Items.Clear();
                ddlMaKV.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in list)
                    ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                
                ddlKHUVUC.DataSource = list;
                ddlKHUVUC.DataTextField = "TENKV";
                ddlKHUVUC.DataValueField = "MAKV";
                ddlKHUVUC.DataBind();

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

                txtNGAYTAO.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNGAYHL.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtNGAYKT.Text = DateTime.Now.ToString("dd/MM/yyyy");

                txtLOAIONG.Text = "HDPE 25";
                txtDMSD.Text = "1";
                txtSOHO.Text = "1";
                txtSONK.Text = "1";
                
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

        private void BindDataForGrid()
        {
            try
            {
                var list = hdDao.GetList("", false);
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
            if (string.Empty.Equals(txtMADDK.Text.Trim())) {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADDK.ClientID);
                return false;
            }

            var existed = hdDao.Get(txtMADDK.Text.Trim());
            if (existed != null && existed.DONDANGKY.TTHD.Equals(TTHD.HD_P)) {
                ShowError("Mã đơn đã tồn tại", txtMADDK.ClientID);
                return false;
            }

            /*if (string.Empty.Equals(txtSOHD.Text.Trim())) {
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
            if (txtMADB.Text.Trim().Length != 0 && txtMADB.Text.Trim().Length != 4)
            {
                ShowError("Độ dài mã danh bộ phải là 4.", txtMADB.ClientID);
                return false;
            }

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
            ddlKICHCODH.SelectedIndex  = 0;
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

            txtDUONGPHO.Text = "";
            lbDUONGPHO.Text = "";
            txtMADB.Text = "";

        }

        
        
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var hopdong = HopDong;
            if (hopdong == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.TK_NhapHopDong, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = hdDao.Get(hopdong.MADDK);
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã đơn đăng ký đã tồn tại", txtMADDK.ClientID);
                    return;
                }
                
                msg = hdDao.Insert(hopdong, CommonFunc .GetComputerName(), CommonFunc.GetIpAdddressComputerName(),LoginInfo.MANV);
            }
            else
            {
                if (!HasPermission(Functions.TK_NhapHopDong, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                
                msg = hdDao.Update(hopdong, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
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
            UpdateMode = Mode.Update;

            // set control's value
            SetControlValue(txtMADDK.ClientID, obj.MADDK);
            SetControlValue(txtTENKH.ClientID, obj.DONDANGKY != null ? obj.DONDANGKY.TENKH : "");
            SetControlValue(txtSOHD.ClientID, obj.SOHD);

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

            if (obj.DUONGPHO != null)
            {
                SetControlValue(txtMADP.ClientID, obj.MADP);
                SetControlValue(txtDUONGPHU.ClientID, "");
                SetLabel(lblTENDUONG.ClientID, obj.DUONGPHO.TENDP);
            }

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

            if (obj.MADP != null)
            {
                txtMADP.Text = obj.MADP;
            }
            if (obj.MADB != null)
            {
                txtMADB.Text = obj.MADB;
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
                            //SetControlValue(txtMADP.ClientID, dp.MADP);
                            //SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);
                            //UpdateKhuVuc(dp);


                            txtDUONGPHO.Text = dp.MADP;
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
        
        

        
        private void BindToInfor(DONDANGKY obj)
        {
            //SetControlValue(txtMADDK.ClientID, obj.MADDK);
            txtMADDK.Text = obj.MADDK;
            SetReadonly(txtMADP.ClientID, true);
            SetControlValue(txtSOHD.ClientID, hdDao.NewId());
            txtTENKH.Text= obj.TENKH.ToString();
            SetControlValue(txtSONHA.ClientID, obj.SONHA);

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

            SetControlValue(txtTENDUONG.ClientID, obj.TEN_DC_KHAC);
            

            if(obj.DUONGPHO != null)
            {
                SetControlValue(txtMADP.ClientID, obj.MADP);
                SetControlValue(txtDUONGPHU.ClientID, "");
                SetLabel(lblTENDUONG.ClientID, obj.DUONGPHO.TENDP);
            }

            ddlLOAIHD.SelectedIndex = 0;
            ddlHTTT.SelectedIndex = 0;

            var mdsd = ddlMDSD.Items.FindByValue(obj.MAMDSD);
            if (mdsd != null)
            {
                ddlMDSD.SelectedIndex = ddlMDSD.Items.IndexOf(mdsd);
            }

            SetControlValue(ddlKICHCODH.ClientID, "15");
            SetControlValue(txtLOAIONG.ClientID, "HDPE 25");
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
            DateTime? tungay = null;
            DateTime? denngay = null;
            try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
            catch { txtTuNgay.Text = ""; }
            try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
            catch { txtDenNgay.Text = ""; }

            var list = ddkDao.GetListForDonChoHopdong(txtFilter.Text.Trim(), tungay, denngay, null, ddlMaKV.SelectedValue);
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
                        var obj = ddkDao.Get(id);
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
    }
}
