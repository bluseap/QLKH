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
    public partial class CapNhatCTSauThiCongPo : Authentication
    {
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KhuVucPoDao _kvpodao = new KhuVucPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly PhuongPoDao _ppoDao = new PhuongPoDao();
        private readonly ThiCongDao tcdao = new ThiCongDao();
        private readonly NhanVienDao nvdao = new NhanVienDao();    

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
                objUi.GHICHU = txtGhiChu.Text.Trim();

                //objUi.CSDAU = null;
                objUi.CSDAU = 0;
                objUi.DUONGKINH = null;
                objUi.MTHUCTE = null;

                if (txtChiSo.Text.Trim() != "")
                    objUi.CSDAU = int.Parse(txtChiSo.Text.Trim());

                if (txtDuongKinh.Text.Trim() != "")
                    objUi.DUONGKINH = int.Parse(txtDuongKinh.Text.Trim());

                if (txtMetThucTe.Text.Trim() != "")
                    objUi.MTHUCTE = decimal.Parse(txtMetThucTe.Text.Trim());

                objUi.SOSERIAL = txtSOSERIAL.Text.Trim();
                objUi.CHIKDM1 = txtCKDM1.Text.Trim();
                objUi.CHIKDM2 = txtCKDM2.Text.Trim();

                return objUi;
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



        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_CapNhatCTSauThiCongPo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TC_CAPNHATCTTHICONGPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_CAPNHATCTTHICONGPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvNhanVien);
            CommonFunc.SetPropertiesForGrid(gvDDK);
        }

        #region Startup script registeration
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

        private void LoadStaticReferences()
        {
            try
            {
                Filtered = FilteredMode.None;

                txtNgayCapNhat.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtFromDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                txtToDate.Text = DateTime.Now.ToString("dd/MM/yyyy");              
               
                timkv();

                txtChiSo.Text = "0";
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

        private void BindDataForGrid()
        {
            try
            {
                if (Filtered == FilteredMode.None)
                {
                    //var objList = tcdao.GetListDaThiCong();
                    //ddlMaKV.SelectedValue
                    var objList = tcdao.GetListDangThiCong2Po(ddlMaKV.SelectedValue);
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

                    var objList = tcdao.GetListDaThiCongPo(maddk, tenkh, sonha, tenduong, tenphuong, tenkv,
                        manv, tennv, madh, chiso, ongnhanh, duongkinh, somet, soserial, ghichu, fromDate, toDate, ddlMaKV.SelectedValue);

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


            if (string.Empty.Equals(txtMaDH.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMaDH.ClientID);
                return false;
            }

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

            txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNgayCapNhat.Text = DateTime.Now.ToString("dd/MM/yyyy");

            chkLapQuyetToan.Checked = false;

            txtMaDH.Text = "";
            txtChiSo.Text = "";
            txtOngChinh.Text = "";
            txtSOSERIAL.Text = "";
            txtDuongKinh.Text = "";
            txtMetThucTe.Text = "";
            txtGhiChu.Text = "";

            lbMADDTRAHSTK.Text = "";
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
            if (!HasPermission(Functions.TC_CapNhatCTSauThiCongPo, Permission.Insert))
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

            Message msg;

            var khpom = _khpoDao.GetMADDK(thicong.MADDK);

            if (string.IsNullOrEmpty(lbMADDTRAHSTK.Text.Trim()) || lbMADDTRAHSTK.Text == "")
            {
                if (khpom != null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng đã khai thác. Kiểm tra lại!");
                    return;
                }
                else
                {
                    msg = tcdao.UpdatePower(thicong, TTTC.TC_A, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.HisNgayDangKyBienPo(thicong.MADDK, LoginInfo.MANV, _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                               "", "", "", "", "DUYETTHICONG");
                }
            }
            else
            {
                var don = _ddkpoDao.Get(lbMADDTRAHSTK.Text.Trim());

                //if (don.MAKVPO == "P" || don.MAKVPO == "T") // phu tan, tan chau
                //{
                //    msg = tcdao.UpdateTraTCToTK(thicong, TTTC.TC_RA, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                //}
                //else
                //{
                //    msg = tcdao.UpdateTraTCToTK(thicong, TTTC.TC_RA, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                //}

                msg = tcdao.UpdateTraTCToTK(thicong, TTTC.TC_RA, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBien(thicong.MADDK, LoginInfo.MANV, nvdao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                           "", "", "", "", "TRAHSTCKT");
            }

            //var msg = tcdao.UpdatePower(thicong, TTTC.TC_A, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
            //_rpClass.HisNgayDangKyBienPo(thicong.MADDK, LoginInfo.MANV, _kvpodao.GetPo(nvdao.Get(LoginInfo.MANV).MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
            //               "", "", "", "", "DUYETTHICONG");

            //update dong ho su dung
            var dasd = _dhpoDao.Get(txtMaDH.Text.Trim());
            _dhpoDao.UpdateDASD(dasd);

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                var msg2 = tcdao.UpdateQuyetToan(thicong, LoginInfo.MANV);

                if (!msg2.MsgType.Equals(MessageType.Error))
                {
                    CloseWaitingDialog();
                    ShowInfor(ResourceLabel.Get(msg2));

                    return;
                }

                CloseWaitingDialog();

                //update dong ho su dung
                //var dasd = dhDao.Get(txtMaDH.Text.Trim());
                //dhDao.UpdateDASD(dasd);

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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        private void BindTCToForm(THICONG obj)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var kvpo =  _kvpodao.GetPo(nvdao.Get(b).MAKV);

            SetControlValue(txtMADDK.ClientID, obj.MADDK);
            SetReadonly(txtMADDK.ClientID, true);

            txtTENKH.Text = obj.DONDANGKYPO.TENKH;
            txtSONHA.Text = obj.DONDANGKYPO.SONHA;
            txtDUONG.Text = obj.DONDANGKYPO.DUONGPHOPO != null ? obj.DONDANGKYPO.DUONGPHOPO.TENDP : "";
            //txtPHUONG.Text = obj.DONDANGKYPO.PHUONGPO != null ? obj.DONDANGKYPO.PHUONGPO.TENPHUONG : "";
            txtPHUONG.Text = obj.DONDANGKYPO.MAPHUONG != null ? _ppoDao.GetMAKV(obj.DONDANGKYPO.MAPHUONG, kvpo.MAKVPO).TENPHUONG : "";
            txtKHUVUC.Text = obj.DONDANGKYPO.KHUVUCPO != null ? obj.DONDANGKYPO.KHUVUCPO.TENKV : "";

            /*
            var pb = ddlPhongBanThiCong.Items.FindByValue(obj.MAPB);
            if (pb != null)
                ddlPhongBanThiCong.SelectedIndex = ddlPhongBanThiCong.Items.IndexOf(pb);
            */

            txtMANV.Text = obj.NHANVIEN != null ? obj.NHANVIEN.MANV : "";
            txtTENNV.Text = obj.NHANVIEN != null ? obj.NHANVIEN.HOTEN : "";
            txtNgayGiaoThiCong.Text = obj.NGAYGTC.HasValue ? obj.NGAYGTC.Value.ToString("dd/MM/yyyy") : "";

            txtNgayHoanThanh.Text = obj.NGAYHT.HasValue ? obj.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
            txtNgayCapNhat.Text = obj.NGAYBD.HasValue ? obj.NGAYBD.Value.ToString("dd/MM/yyyy") : "";

            txtMaDH.Text = obj.MADH;
            txtChiSo.Text = obj.CSDAU.HasValue ? obj.CSDAU.Value.ToString() : "";
            chkLapQuyetToan.Checked = obj.TTQT.HasValue && obj.TTQT.Value;
            txtOngChinh.Text = obj.ONGNHANH;

            var dh = _dhpoDao.Get(obj.MADH);
            if (dh != null)
            { txtSOSERIAL.Text = dh.SONO.ToString(); }
            else { txtSOSERIAL.Text = ""; }

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
                            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                            if (loginInfo == null) return;
                            string b = loginInfo.Username;

                            ShowInFor("Thi công đã duyệt. Xin chọn thi công chưa duyệt.");
                            CloseWaitingDialog();
                            break;
                            /*if (b == "tam")
                            {
                                var obj = tcdao.Get(id);
                                if (obj != null)
                                {
                                    ClearContent();
                                    BindTCToForm(obj);
                                }
                                CloseWaitingDialog();
                                break;
                            }
                            else
                            {
                                ShowInFor("Thi công đã duyệt. Xin chọn thi công chưa duyệt.");
                                CloseWaitingDialog();
                                break;
                            }*/
                        }
                        else
                        {
                            var obj = tcdao.Get(id);
                            if (obj != null)
                            {
                                ClearContent();
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

                    case "TraHSVeTK":
                        if (list.TTTC.Equals("TC_A"))
                        {
                            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                            if (loginInfo == null) return;
                            string b = loginInfo.Username;

                            ShowInFor("Thi công đã duyệt. Xin chọn thi công chưa duyệt.");
                            CloseWaitingDialog();

                            break;
                        }
                        else
                        {
                            var obj = tcdao.Get(id);
                            if (obj != null)
                            {
                                lbMADDTRAHSTK.Text = id;

                                ClearContent();

                                BindTCToForm(obj);
                            }
                            CloseWaitingDialog();

                            break;
                        }

                    case "SuaMaChiKD":
                        var ttnt = list.TTNT != null ? list.TTNT : "";
                        if (ttnt.Equals("NT_A"))
                        {
                            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                            if (loginInfo == null) return;
                            string b = loginInfo.Username;

                            ShowInFor("Khách hàng đã nghiệm thu. Không sửa được.");                                             
                        }
                        else
                        {
                            var tttc = list.TTTC != null ? list.TTTC : "";
                            if (tttc.Equals("TC_A"))
                            {
                                var obj = tcdao.Get(id);
                                if (obj != null)
                                {
                                    ClearContent();
                                    BindTCToForm(obj);
                                }                                
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
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;            

            var list = nvdao.SearchKV1(txtKeywordNV.Text.Trim(), nvdao.Get(b).MAKV);
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

        private void BindToInfor(DONDANGKYPO obj)
        {
            SetControlValue(txtMADDK.ClientID, obj.MADDKPO);
            SetReadonly(txtMADDK.ClientID, true);

            txtTENKH.Text = obj.TENKH;
            txtSONHA.Text = obj.SONHA;
            txtDUONG.Text = obj.MADPPO != null ? _dppoDao.GetDP(obj.MADPPO).TENDP : "";
            txtPHUONG.Text = obj.MAPHUONG != null ? _ppoDao.GetMAKV(obj.MAPHUONG, obj.MAKVPO).TENPHUONG : "";
            txtKHUVUC.Text = obj.MAKVPO != null ? _kvpodao.Get(obj.MAKVPO).TENKV : "";

            var tc = tcdao.Get(obj.MADDKPO);
            if (tc != null)
            {
                /*
                var pb = ddlPhongBanThiCong.Items.FindByValue(tc.MAPB);
                if (pb != null)
                    ddlPhongBanThiCong.SelectedIndex = ddlPhongBanThiCong.Items.IndexOf(pb);
                */

                txtMANV.Text = tc.NHANVIEN != null ? tc.NHANVIEN.MANV : "";
                txtTENNV.Text = tc.NHANVIEN != null ? tc.NHANVIEN.HOTEN : "";
                txtNgayGiaoThiCong.Text = tc.NGAYGTC.HasValue ? tc.NGAYGTC.Value.ToString("dd/MM/yyyy") : "";
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
            try
            {
                DateTime? tungay = null;
                DateTime? denngay = null;
                try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
                catch { txtTuNgay.Text = ""; }
                try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
                catch { txtDenNgay.Text = ""; }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (nvdao.Get(b).MAKV == "S")
                {
                    var list = _ddkpoDao.GetListDonThiCongCD(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue);
                    gvDDK.DataSource = list;
                    gvDDK.PagerInforText = list.Count.ToString();
                    gvDDK.DataBind();
                }
                else
                {
                    var list = _ddkpoDao.GetListDonThiCong(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue);
                    gvDDK.DataSource = list;
                    gvDDK.PagerInforText = list.Count.ToString();
                    gvDDK.DataBind();
                }
            }
            catch { }
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

                        BindToInfor(obj);
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");

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

        protected void btnBrowseDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
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
                            //SetControlValue(txtMADH.ClientID, dh.MADH);
                            txtMaDH.Text = dh.MADHPO;
                            //SetControlValue(txtMALDH.ClientID, dh.MALDH);
                            //SetLabel(lblSONO.ClientID, dh.SONO);
                            txtSOSERIAL.Text = dh.SONO;
                            //SetLabel(lblKICHCO.ClientID, ldhDao.Get(dh.MALDH).ToString());
                            /*var ldhkc = ldhDao.GetListldh(dh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetLabel(lblKICHCO.ClientID, kc.KICHCO);
                            }*/
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
                // Update page index
                gvDongHoSoNo.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDongHoSoNo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            CloseWaitingDialog();
        }
    }
}