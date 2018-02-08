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
    public partial class TraCuuThiCongPo : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly ThiCongDao tcdao = new ThiCongDao();
        private readonly NhanVienDao nvdao = new NhanVienDao();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();

        private THICONG ThiCong
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var objUi = tcdao.Get(txtMADDK.Text.Trim());

                var nv = nvdao.Get(txtMANV.Text.Trim());
                if (nv != null)
                {
                    objUi.MAPB = nv.MAPB;
                    objUi.MANV = nv.MANV;
                }

                objUi.NGAYGTC = DateTimeUtil.GetVietNamDate(txtNgayGiaoThiCong.Text.Trim());
                objUi.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                objUi.NGAYBD = DateTimeUtil.GetVietNamDate(txtNgayCapNhat.Text.Trim());
                objUi.MADH = txtMaDH.Text.Trim();
                objUi.TTQT = chkLapQuyetToan.Checked;

                objUi.ONGNHANH = txtOngChinh.Text.Trim();
                objUi.SOSERIAL = txtSOSERIAL.Text.Trim();
                objUi.GHICHU = txtGhiChu.Text.Trim();

                objUi.CSDAU = null;
                objUi.DUONGKINH = null;
                objUi.MTHUCTE = null;

                if (txtChiSo.Text.Trim() != "")
                    objUi.CSDAU = int.Parse(txtChiSo.Text.Trim());

                if (txtDuongKinh.Text.Trim() != "")
                    objUi.DUONGKINH = int.Parse(txtDuongKinh.Text.Trim());

                if (txtMetThucTe.Text.Trim() != "")
                    objUi.MTHUCTE = decimal.Parse(txtMetThucTe.Text.Trim());

                return objUi;
            }
        }

        #region filter,update
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

        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowInFor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void FocusAndSelect(string controlId)
        {
            ((PO)Page.Master).FocusAndSelect(controlId);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_TraCuuThiCongPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    Filtered = FilteredMode.None;
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
            Page.Title = Resources.Message.TITLE_TC_TRACUUTHICONGPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_TRACUUTHICONGPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvNhanVien);
        }       

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kvpo = _kvpoDao.GetPo(nvdao.Get(b).MAKV);

                if (Filtered == FilteredMode.None)
                {
                    //var objList = tcdao.GetListTraCuuThiCongPo();
                    var objList = tcdao.GetListTraCuuThiCongPoKV(kvpo.MAKVPO);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    DateTime? fromDate = null;
                    DateTime? toDate = null;
                    var maddk = txtMADDK.Text.Trim();
                    var tenkh = txtTENKH.Text.Trim();
                    var sonha = txtSONHA.Text.Trim();
                    var tenduong = txtDUONG.Text.Trim();
                    var tenphuong = txtPHUONG.Text.Trim();
                    var tenkv = txtKHUVUC.Text.Trim();
                    var manv = txtMANV.Text.Trim();
                    var tennv = txtTENNV.Text.Trim();
                    var madh = txtMaDH.Text.Trim();
                    int? chiso = null;
                    var ongnhanh = txtOngChinh.Text.Trim();
                    var soserial = txtSOSERIAL.Text.Trim();
                    int? duongkinh = null;
                    decimal? somet = null;
                    var ghichu = txtGhiChu.Text.Trim();

                    // ReSharper disable EmptyGeneralCatchClause
                    try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
                    catch { }
                    try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
                    catch { }
                    try { chiso = int.Parse(txtChiSo.Text.Trim()); }
                    catch { }
                    try { duongkinh = int.Parse(txtDuongKinh.Text.Trim()); }
                    catch { }
                    try { somet = decimal.Parse(txtMetThucTe.Text.Trim()); }
                    catch { }
                    // ReSharper restore EmptyGeneralCatchClause

                    var objList = tcdao.GetListTraCuuThiCongPo(maddk, tenkh, sonha, tenduong, tenphuong, tenkv,
                        manv, tennv, madh, chiso, ongnhanh, duongkinh, somet, soserial, ghichu, fromDate, toDate, kvpo.MAKVPO);

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

        private bool IsDataValid()
        {

            if (string.Empty.Equals(txtMADDK.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADDK.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtMANV.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã nhân viên"), txtMANV.ClientID);
                return false;
            }

            /*
            var pb = pbdao.Get(ddlPhongBanThiCong.SelectedValue);
            if (pb == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Phòng ban"));
                return false;
            }
            */

            #region Ngày tháng
            try
            {
                DateTimeUtil.GetVietNamDate(txtNgayGiaoThiCong.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày giao"), txtNgayGiaoThiCong.ClientID);
                return false;
            }

            try
            {
                DateTimeUtil.GetVietNamDate(txtNgayCapNhat.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày cập nhật"), txtNgayCapNhat.ClientID);
                return false;
            }

            try
            {
                DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
            }
            catch
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày hoàn thành"), txtNgayHoanThanh.ClientID);
                return false;
            }
            #endregion

            /*
            var vt = vtdao.Get(txtMAVT.Text.Trim());
            if (vt == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã vật tư"), txtMAVT.ClientID);
                return false;
            }
            */


            #region Số
            if (txtChiSo.Text.Trim() != "")
            {
                try
                {
                    int.Parse(txtChiSo.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Chỉ số mới"), txtChiSo.ClientID);
                    return false;
                }
            }

            if (txtDuongKinh.Text.Trim() != "")
            {
                try
                {
                    int.Parse(txtDuongKinh.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Đường kính"), txtDuongKinh.ClientID);
                    return false;
                }
            }

            if (txtDuongKinh.Text.Trim() != "")
            {
                try
                {
                    decimal.Parse(txtMetThucTe.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mét thực tế"), txtMetThucTe.ClientID);
                    return false;
                }
            }

            #endregion

            return true;
        }

        private void ClearContent()
        {
            txtMADDK.Text = "";
            txtMADDK.ReadOnly = false;
            txtTENKH.Text = "";
            txtSONHA.Text = "";
            txtDUONG.Text = "";
            txtPHUONG.Text = "";
            txtKHUVUC.Text = "";
            txtNgayGiaoThiCong.Text = "";
            txtMANV.Text = "";
            txtTENNV.Text = "";

            //ddlPhongBanThiCong.SelectedIndex = 0;
            chkLapQuyetToan.Checked = false;

            txtNgayHoanThanh.Text = "";
            txtNgayCapNhat.Text = "";

            txtMaDH.Text = "";
            txtChiSo.Text = "";
            txtOngChinh.Text = "";
            txtSOSERIAL.Text = "";
            txtDuongKinh.Text = "";
            txtMetThucTe.Text = "";
            txtGhiChu.Text = "";
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            Filtered = FilteredMode.None;

            BindDataForGrid();
            upnlGrid.Update();

            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (!HasPermission(Functions.TC_TraCuuThiCongPo, Permission.Update))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            var thicong = ThiCong;
            if (thicong == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg, msgdon; 

            if (Session[SessionKey.KH_COBIENNUOC] == "TUCHOITHICONG")
            {
                var khachhang = _khpoDao.GetMADDK(thicong.MADDK);
                if (khachhang != null)
                {
                    ShowError("Khách hàng đã khai thác. Kkông được từ chối thi công.");

                    CloseWaitingDialog();
                    ClearContent();
                    BindDataForGrid();

                    upnlGrid.Update();
                    return;
                }
                else
                {
                    var dondk = _ddkpoDao.Get(thicong.MADDK);
                    var tcsua = tcdao.Get(thicong.MADDK);

                    if (ckPhucHoiTuChoi.Checked)
                    {
                        _rpClass.HisNgayDangKyBien(thicong.MADDK, "", "", DateTime.Now, DateTime.Now, DateTime.Now, "", "", "", "",
                            "TUCHOITHICONG");

                        tcsua.GHICHU = txtGhiChu.Text.Trim();
                        msg = tcdao.Update(tcsua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                        dondk.TTTC = "TC_P";
                        msgdon = _ddkpoDao.Update(dondk, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    }
                    else
                    {
                        _rpClass.HisNgayDangKyBien(thicong.MADDK, "", "", DateTime.Now, DateTime.Now, DateTime.Now, "", "", "", "",
                            "TUCHOITHICONG");

                        tcsua.GHICHU = txtGhiChu.Text.Trim();
                        msg = tcdao.Update(tcsua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                        dondk.TTTC = "TC_RA";
                        msgdon = _ddkpoDao.Update(dondk, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    }
                }
                CloseWaitingDialog();
                ClearContent();
            }
            else
            {
                msg = tcdao.UpdatePo(thicong, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
            }

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                CloseWaitingDialog();

                // ShowInfor(ResourceLabel.Get(msg));

                ShowInfor("Cập nhật lại thông tin thi công thành công");

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        private void BindTCToForm(THICONG obj)
        {
            SetControlValue(txtMADDK.ClientID, obj.MADDK);
            SetReadonly(txtMADDK.ClientID, true);

            txtTENKH.Text = obj.DONDANGKYPO.TENKH;
            txtSONHA.Text = obj.DONDANGKYPO.SONHA;
            txtDUONG.Text = obj.DONDANGKYPO.DUONGPHOPO != null ? obj.DONDANGKYPO.DUONGPHOPO.TENDP : "";
            txtPHUONG.Text = obj.DONDANGKYPO.PHUONGPO != null ? obj.DONDANGKYPO.PHUONGPO.TENPHUONG : "";
            txtKHUVUC.Text = obj.DONDANGKYPO.KHUVUCPO != null ? obj.DONDANGKYPO.KHUVUCPO.TENKV : "";

            txtMANV.Text = obj.NHANVIEN != null ? obj.NHANVIEN.MANV : "";
            txtTENNV.Text = obj.NHANVIEN != null ? obj.NHANVIEN.HOTEN : "";
            txtNgayGiaoThiCong.Text = obj.NGAYGTC.HasValue ? obj.NGAYGTC.Value.ToString("dd/MM/yyyy") : "";

            txtNgayHoanThanh.Text = obj.NGAYHT.HasValue ? obj.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
            txtNgayCapNhat.Text = obj.NGAYBD.HasValue ? obj.NGAYBD.Value.ToString("dd/MM/yyyy") : "";

            txtMaDH.Text = obj.MADH;
            txtChiSo.Text = obj.CSDAU.HasValue ? obj.CSDAU.Value.ToString() : "";
            chkLapQuyetToan.Checked = obj.TTQT.HasValue && obj.TTQT.Value;


            txtOngChinh.Text = obj.ONGNHANH;
            txtSOSERIAL.Text = obj.SOSERIAL;
            txtDuongKinh.Text = obj.DUONGKINH.HasValue ? obj.DUONGKINH.Value.ToString() : "";
            txtMetThucTe.Text = obj.MTHUCTE.HasValue ? obj.MTHUCTE.Value.ToString() : "";
            txtGhiChu.Text = obj.GHICHU;


            upnlInfor.Update();
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
                var id = e.CommandArgument.ToString();
                var list = _ddkpoDao.Get(id);
                switch (e.CommandName)
                {
                    case "EditItem":
                        if (list.TTTC.Equals("TC_A"))
                        {
                            ShowInFor("Thi công đã duyệt. Xin chọn thi công chưa duyệt.");
                            CloseWaitingDialog();
                            break;
                        }
                        else
                        {
                            var obj = tcdao.Get(id);
                            if (obj != null)
                            {
                                BindTCToForm(obj);
                            }

                            CloseWaitingDialog();

                            break;
                        }

                    case "ReportItem":
                        Session["NHAPTHICONG_MADDK"] = id;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpPhieuCongTac.aspx", false);

                        CloseWaitingDialog();

                        break;

                    case "TuChoiTC":
                        if (list.TTTC.Equals("TC_A"))
                        {
                            ShowInFor("Thi công đã duyệt. Xin chọn thi công chưa duyệt.");
                            CloseWaitingDialog();
                            break;
                        }
                        else
                        {
                            var thicongtc = tcdao.Get(id);
                            if (thicongtc != null)
                            {
                                BindTCToForm(thicongtc);
                            }

                            Session[SessionKey.KH_COBIENNUOC] = "TUCHOITHICONG";
                            CloseWaitingDialog();
                            break;
                        }
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

            var source = gvList.DataSource as List<THICONG>;
            if (source == null) return;

            var imgTT = e.Row.FindControl("imgTT") as Button;
            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = _ddkpoDao.Get(source[index].MADDK);


                if (imgTT != null && ddk != null)
                {
                    imgTT.Attributes.Add("class", ddk.TRANGTHAITHIETKE4.COLOR);
                    imgTT.ToolTip = ddk.TRANGTHAITHIETKE4.TENTT;
                }
            }
            catch { }
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        private void BindNhanVien()
        {
            var list = nvdao.Search(txtKeywordNV.Text.Trim());
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
                        var nv = nvdao.Get(id);
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
                gvNhanVien.PageIndex = e.NewPageIndex;            
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkBtnMANV_Click(object sender, EventArgs e)
        {
            var nv = nvdao.Get(txtMANV.Text.Trim());
            if (nv != null)
            {
                txtTENNV.Text = nv.HOTEN;
                txtTENNV.Focus();
            }

            CloseWaitingDialog();
        }
    }
}