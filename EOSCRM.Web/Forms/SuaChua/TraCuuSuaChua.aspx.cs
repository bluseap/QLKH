using System;
using System.Web.UI.WebControls;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.SuaChua
{
    public partial class TraCuuSuaChua : Authentication
    {
        private readonly GiaiQuyetThongTinSuaChuaDao _objDao = new GiaiQuyetThongTinSuaChuaDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly ThongTinXuLyDao _xlDao = new ThongTinXuLyDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        #region Properties

        protected String Keyword
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_KEYWORD))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_KEYWORD].ToString());
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

        protected String AreaCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_AREACODE))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_AREACODE].ToString());
            }
        }

        private GIAIQUYETTHONGTINSUACHUA GQTTSCObj
        {
            get
            {
                if (!IsDataValid())
                    return null;
               
                var obj = _objDao.Get(txtMADON.Text.Trim());
                if (obj == null)
                    return null;

                obj.MADON = txtMADON.Text.Trim();
                obj.XACNHAN = int.Parse(cboXacNhanThongTin.Text.Trim());
                obj.BIENBAN = chkBIENBAN.Checked;
                obj.LANXL = int.Parse(cboLANXL.Text.Trim());
                obj.LYDO = txtLyDo.Text.Trim();
                //obj.MAXL = cboMAXL.Text.Trim();
                obj.CHINIEM = txtCHINIEM.Text.Trim();
                obj.MADH = txtMaDH.Text.Trim();
                obj.CSTRUOC = txtChiSoTruoc.Text.Trim();
                obj.CSSAU = txtChiSoSau.Text.Trim();
               // obj.TTDON = cboTrangThaiDon.Text.Trim();
                //obj.ISLAPCHIETTINH = chkIsLapChietTinh.Checked;
                obj.THONGTINKH = txtTHONGTINKH.Text.Trim();
                obj.SDT = txtSDT.Text.Trim();
                obj.MANVXL = txtMANV.Text.Trim();
                obj.NGAYHT = DateTimeUtil.GetVietNamDate(txtNGAYNQ.Text.Trim(), txtGio.Text.Trim(), txtPhut.Text.Trim());
                obj.GHICHU = txtGhiChu.Text.Trim();
               
                if (!string.IsNullOrEmpty(txtMAKH.Text.Trim()))
                    obj.IDKH = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim()).IDKH;

                if (!string.IsNullOrEmpty(txtCODH.Text.Trim()))
                    obj.CODH = int.Parse(txtCODH.Text.Trim());

                return obj;
            }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_QuanLyThongTinDonSuaChua, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_SC_TRACUUSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_TRACUUSUACHUA;
            }

            CommonFunc.SetPropertiesForGrid(gvNhanVien);
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvList);
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

        #endregion

        private void LoadStaticReferences()
        {
            //txtNGAYNQ.Text = DateTime.Now.ToString("dd/MM/yyyy");
            //txtGio.Text = DateTime.Now.Hour.ToString();
            //txtPhut.Text = DateTime.Now.Minute.ToString();

            txtLapChietTinh.Text = "";
            //chkIsLapChietTinh.Checked = false;
            //chkIsLapChietTinh.Enabled = false;

            //var listLoaiXL = _xlDao.GetList();
            //cboMAXL.DataSource = listLoaiXL;
            //cboMAXL.DataTextField = "TENXL";
            //cboMAXL.DataValueField = "MAXL";
            //cboMAXL.DataBind();

            var khuvuclist = _kvDao.GetList();
            ddlKHUVUCKH.Items.Clear();
            ddlKHUVUCKH.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var kv in khuvuclist)
            {
                ddlKHUVUCKH.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }

        }

        private void BindDataForGrid()
        {
            try
            {
                var objList = _objDao.GetListForTraCuu(Keyword, FromDate, ToDate, AreaCode);
                //var objList =_objDao.GetListDonChoCapNhatSauSuaChua(Keyword, FromDate, ToDate, AreaCode );

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        public bool IsDataValid()
        {
            if (string.Empty.Equals(txtMADON.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADON.ClientID);
                return false;
            }

            if (!string.Empty.Equals(txtMAKH.Text))
            {
                var khachhang = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim());
                if (khachhang == null)
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã khách hàng"), txtMAKH.ClientID);
                    return false;
                }
            }

            try
            {
                DateTimeUtil.GetVietNamDate(txtNGAYNQ.Text.Trim(), txtGio.Text.Trim(), txtPhut.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày giải quyết"), txtNGAYNQ.ClientID);
                return false;
            }

            var nv = _nvDao.Get(txtMANV.Text.Trim());
            if (nv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã nhân viên"), txtMANV.ClientID);
                return false;
            }

            if (txtCODH.Text.Trim() != "")
            {
                try
                {
                    int.Parse(txtCODH.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Cỡ đồng hồ "), txtCODH.ClientID);
                    return false;
                }
            }

            return true;
        }

        private void ClearContent()
        {
            txtChiSoSau.Text = "";
            txtChiSoTruoc.Text = "";
            txtGhiChu.Text = "";
            txtGio.Text = DateTime.Now.Hour.ToString();
            txtLyDo.Text = "";
            txtNGAYNQ.Text = DateTime.Now.Day.ToString() + @"/" + DateTime.Now.Month.ToString() + @"/" +
                             DateTime.Now.Year.ToString();
            txtMaDH.Text = "";
            txtMADON.Text = "";
            txtMADON.ReadOnly = false;
            txtMAKH.Text = "";
            txtMANV.Text = "";
            txtPhut.Text = DateTime.Now.Minute.ToString();
            txtSDT.Text = "";
            txtTENKH.Text = "";
            txtTENNV.Text = "";
            txtMAPH.Text = "";
            txtTHONGTINKH.Text = "";

            chkBIENBAN.Checked = false;
            txtLapChietTinh.Text = "";
           // chkIsLapChietTinh.Checked = false;

            cboLANXL.SelectedIndex = 0;
            cboXacNhanThongTin.SelectedIndex = 0;
            txtTrangThai.Text = "";
            //cboTrangThaiDon.SelectedIndex = 0;
            //cboMAXL.SelectedIndex = 0;
            txtLoaiXL.Text = "";
        }





        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var don = GQTTSCObj;
                if (don == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var msg = _objDao.GiaiQuyetDonSuaChua(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);


                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();

                    ShowInfor(ResourceLabel.Get(msg));

                    //Trả lại màn hình trống ban đầu
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
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            CloseWaitingDialog();
        }

        private void BindDSCToForm(GIAIQUYETTHONGTINSUACHUA obj)
        {
            txtMADON.Text = obj.MADON;
            txtMADON.ReadOnly = true;
            txtMAPH.Text = obj.THONGTINPHANHOI != null ? obj.THONGTINPHANHOI.TENPH : "";
            cboXacNhanThongTin.SelectedIndex = (obj.XACNHAN.HasValue && obj.XACNHAN.Value == 0) ? 0 : 1;
            chkBIENBAN.Checked = obj.BIENBAN.HasValue && obj.BIENBAN.Value;
            cboLANXL.SelectedIndex = obj.LANXL.HasValue ? obj.LANXL.Value - 1 : 0;
            txtLyDo.Text = obj.LYDO;

            txtLoaiXL.Text = obj.THONGTINXULY != null ? obj.THONGTINXULY.TENXL : "";
        
            txtCODH.Text = obj.CODH.HasValue ? obj.CODH.Value.ToString() : "";

            txtCHINIEM.Text = obj.CHINIEM;
            txtMaDH.Text = obj.MADH;
            txtChiSoTruoc.Text = obj.CSTRUOC;
            txtChiSoSau.Text = obj.CSSAU;
            if ( obj.ISLAPCHIETTINH != null && obj.ISLAPCHIETTINH == true)
                txtLapChietTinh.Text = @"Có lập chiết tính";
            else
            {
                txtLapChietTinh.Text = @"";
            }

            if (obj.TTDON != null && obj.TTDON.Equals("SC_F"))
                txtTrangThai.Text = @"Đã hoàn thành";
            else
            {
                txtTrangThai.Text = @"Đang sửa chữa";
            }

            //var tt = cboTrangThaiDon.Items.FindByValue(obj.TTDON);
            //if (tt != null)
            //    cboTrangThaiDon.SelectedIndex = cboTrangThaiDon.Items.IndexOf(tt);

            //chkIsLapChietTinh.Checked = obj.ISLAPCHIETTINH.HasValue && obj.ISLAPCHIETTINH.Value ? true : false;
            //if (cboTrangThaiDon.SelectedValue == "SC_F")
            //{
            //    chkIsLapChietTinh.Enabled = false;
            //    chkIsLapChietTinh.Checked = false;
            //}
            //else
            //{
            //    chkIsLapChietTinh.Enabled = true;
            //}

            txtMAKH.Text = obj.KHACHHANG != null ?
                obj.KHACHHANG.MADP + obj.KHACHHANG.DUONGPHU + obj.KHACHHANG.MADB :
                "";
            txtTENKH.Text = obj.KHACHHANG != null ? obj.KHACHHANG.TENKH : "";
            txtTHONGTINKH.Text = obj.THONGTINKH;
            txtSDT.Text = obj.SDT;

            txtMANV.Text = obj.NHANVIEN1 != null ? obj.NHANVIEN1.MANV : "";
            txtTENNV.Text = obj.NHANVIEN1 != null ? obj.NHANVIEN1.HOTEN : "";

            txtNGAYNQ.Text = obj.NGAYHT.HasValue ? obj.NGAYHT.Value.ToString("dd/MM/yyyy") : DateTime.Now.ToString("dd/MM/yyyy");
            txtGio.Text = obj.NGAYHT.HasValue ? obj.NGAYHT.Value.Hour.ToString() : DateTime.Now.Hour.ToString();
            txtPhut.Text = obj.NGAYHT.HasValue ? obj.NGAYHT.Value.Minute.ToString() : DateTime.Now.Minute.ToString();
            txtGhiChu.Text = obj.GHICHU;
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var madon = e.CommandArgument.ToString();
                ClearContent();
                switch (e.CommandName)
                {
                    case "EditItem":
                        var don = _objDao.Get(madon);
                        if (don == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }

                        BindDSCToForm(don);
                        upnlInfor.Update();
                        CloseWaitingDialog();

                        break;

                    case "ReportItem":
                        Session["DONSUACHUA_MADON"] = madon;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpPhieuCongTacSuaChua.aspx", false);

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

            var lnkBtnIDReport = e.Row.FindControl("lnkBtnIDReport") as LinkButton;
            if (lnkBtnIDReport == null) return;
            lnkBtnIDReport.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnIDReport) + "')");
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

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        private void BindNhanVien()
        {
            var list = _nvDao.Search(txtKeywordNV.Text.Trim());
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
        }

        protected void btnBrowseNhanVien_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            upnlNhanVien.Update();
            UnblockDialog("divNhanVien");
        }

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
                            SetControlValue(txtMANV.ClientID, nv.MANV);
                            SetControlValue(txtTENNV.ClientID, nv.HOTEN);
                            txtMANV.Focus();

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
                // Update page index
                gvNhanVien.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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
                                                           txtMADHFilter.Text.Trim(), txtSOHD.Text.Trim(),
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
            SetControlValue(txtMAKH.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetControlValue(txtTENKH.ClientID, kh.TENKH);
            SetControlValue(txtSDT.ClientID, kh.SDT);

            txtMAKH.Focus();
        }

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
                // Update page index
                gvDanhSach.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtMAKH_TextChanged(object sender, EventArgs e)
        {
            if (txtMAKH.Text.Trim() == "")
                CloseWaitingDialog();

            var khachhang = _khDao.GetKhachHangFromMadb(txtMAKH.Text.Trim());
            if (khachhang != null)
            {
                BindKHInfor(khachhang);
                CloseWaitingDialog();
            }
            else
            {
                CloseWaitingDialog();
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã khách hàng"), txtMAKH.ClientID);
            }
        }
    }
}