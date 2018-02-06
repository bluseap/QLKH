using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util ;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Web.UI;
using System.Data;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class ThayDongHo : Authentication
    {
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly LoaiDongHoDao _loaiDhDao = new LoaiDongHoDao();
        private readonly ThayDongHoDao _thaydonghoDao = new ThayDongHoDao();
        private readonly DongHoDao dhDao = new DongHoDao();
        private readonly ReportClass rp = new ReportClass();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.CN_NhapCongNo, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_THAYDONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_THAYDONGHO;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
        }

        private void LoadStaticReferences()
        {
            try
            {
                var kvList = _kvDao.GetList();

                ddlKHUVUC.Items.Clear();
                ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in kvList)
                {
                    ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }


                var loaiDhList = _loaiDhDao.GetList();
                cboLoaiDh.Items.Clear();
                cboLoaiDh.DataTextField = "MALDH";
                cboLoaiDh.DataValueField = "MALDH";
                cboLoaiDh.DataSource = loaiDhList;
                cboLoaiDh.DataBind();
                ClearForm();
                //txtNAM.Text = DateTime.Now.Year.ToString();
                //ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                //txtNgayThay.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
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
       
        #region Khách hàng
        protected void btnFilterKH_Click(object sender, EventArgs e)
        {            
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        private void BindKhachHang()
        {
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(),
                                                           txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                           txtSONHA.Text.Trim(), txtTENDP.Text.Trim(),
                                                           ddlKHUVUC.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;

            upnlKhachHang.Update();
        }

        protected void btnBrowseKH_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");           
        }

        private void BindStatus(KHACHHANG kh)
        {
            var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));
            SetControlValue(txtSODB.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            //SetLabel(lblIDKH.ClientID, kh.IDKH);
            //SetControlValue(lblIDKH.ClientID, kh.IDKH);
            lblIDKH.Text = kh.IDKH;
            SetLabel(lblTENDP.ClientID, kh.DUONGPHO != null ? kh.DUONGPHO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUC != null ? kh.KHUVUC.TENKV : "");
            //SetLabel(lblLOAITK.ClientID, kh.MALDH );
            //SetLabel(lblSONO.ClientID, kh.MADH );
            lblLOAITK.Text = kh.MALDH;
            lblSONO.Text = kh.MADH;
            SetControlValue(ddlKICHCODH.ClientID , kh.THUYLK);
            //SetLabel(lblLDHTHAY.ClientID, kh.DONGHO != null ? kh.DONGHO.MALDH : "");
            SetLabel(lblCSDAU.ClientID, tieuthu.CHISODAU.ToString());
            SetLabel(lblCSCUOI.ClientID, tieuthu.CHISOCUOI.ToString());
            lblNGAYTHAY.Text = kh.NGAYTHAYDH.HasValue ? kh.NGAYTHAYDH.Value.ToString("dd/MM/yyyy") : "";
            lblNGAYHOANTHANH.Text = kh.NGAYHT.HasValue ? kh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";

            upnlThongTin.Update();
        }

        private void BindStatusTDH(KHACHHANG kh)
        {
            var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG1.SelectedValue), int.Parse(txtNAM1.Text.Trim()));
            SetControlValue(txtSODB.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            //SetLabel(lblIDKH.ClientID, kh.IDKH);
            //SetControlValue(lblIDKH.ClientID, kh.IDKH);
            lblIDKH.Text = kh.IDKH;
            SetLabel(lblTENDP.ClientID, kh.DUONGPHO != null ? kh.DUONGPHO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUC != null ? kh.KHUVUC.TENKV : "");
            lblNGAYTHAY.Text = kh.NGAYTHAYDH.HasValue ? kh.NGAYTHAYDH.Value.ToString("dd/MM/yyyy") : "";
            lblNGAYHOANTHANH.Text = kh.NGAYHT.HasValue ? kh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";

            upnlThongTin.Update();
        }

        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        //var khachhang = _khDao.GetKhachHangFromMadb(id);
                        var khachhang = _khDao.Get(id);
                        if (khachhang != null)
                        {
                            BindStatus(khachhang);
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            txtSODB.Focus();
                        }
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
        #endregion

        private bool IsValidate()
        {
            // check ngày hoàn thành có định dạng dd/MM/yyyy
            if (!string.IsNullOrEmpty(txtNgayThay.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                }
                catch
                {
                    ShowError("Ngày thay không hợp lệ.", txtNgayThay.ClientID);
                    return false;
                }
            }

            // check ngày hoàn thành có định dạng dd/MM/yyyy
            if (!string.IsNullOrEmpty(txtNgayHoanThanh.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                }
                catch
                {
                    ShowError("Ngày hoàn thành không hợp lệ.", txtNgayHoanThanh.ClientID);
                    return false;
                }
            }
                        
            return true;
        }

        private void ClearForm()
        {
            
            ddlTHANG1.SelectedIndex = DateTime.Now.Month - 1;     
            txtNAM1.Text = DateTime.Now.Year.ToString();
            txtIDKH.Text = "";
            lblIDKH.Text = "";
            lblTENKH.Text = "";
            lblTENDP.Text = "";
            lblTENKV.Text = "";
            lblLOAITK.Text = "";
            lblSONO.Text = "";
            txtCSNGUNG.Text = "0";
            txtTRUYTHU.Text = "0";
            txtMaDongho.Text = "";
            txtCSBATDAU.Text = "0";
            txtCSMOI.Text = "0";
            txtGhiChu.Text = "";            
            txtNgayThay.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtNgayHoanThanh.Text = DateTime.Now.ToString("dd/MM/yyyy");

            btnSaveUp.Visible = false;
            btnSave.Visible = true;
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            //lock cap nhap chi so
            /*int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);            

            bool dung = _gcsDao.IsLockTinhCuocKy(kynay1,query.MAKV.ToString());
            if (dung == true)
            {
                CloseWaitingDialog();
                ShowInfor("Đã khoá sổ ghi chỉ số.");
                return;
            }*/
            
            if (!HasPermission(Functions.KH_ThayDongHo , Permission.Insert))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            /*if (!IsValidate())
            {
                CloseWaitingDialog();
                return;
            }*/

            try
            {
                //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
                var kh = _khDao.Get(lblIDKH.Text.Trim());
                string h = lblIDKH.ClientID;
                if(kh == null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                    return;

                }

                kh.THUYLK = ddlKICHCODH.SelectedValue;
                kh.MALDH = cboLoaiDh.Items.Count > 0 ?  cboLoaiDh.SelectedValue : null;
                kh.MADH = txtMaDongho.Text.Trim();
                kh.NGAYTHAYDH = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                kh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                kh.CHISODAU = Convert.ToInt32(txtCSNGUNG.Text.Trim());
                kh.CHISOCUOI = Convert.ToInt32(txtCSBATDAU.Text.Trim());
                kh.m4Poor = Convert.ToInt32(txtTRUYTHU.Text.Trim());
                kh.KLKHOAN = Convert.ToInt32(txtCSMOI.Text.Trim());

                var msg = _khDao.UpdateThayDongHo(kh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()) , txtSoTem .Text .Trim() , txtGhiChu.Text.Trim(), 
                                                CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV,
                                                lblSONO.Text.Trim(), lblLOAITK.Text.Trim(),DateTimeUtil.GetVietNamDate(lblNGAYTHAY.Text.Trim()),DateTimeUtil.GetVietNamDate(lblNGAYHOANTHANH.Text.Trim()));

                rp.UpdateTieuThuDH(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), Convert.ToInt32(txtTRUYTHU.Text.Trim()));
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();

                    upnlGrid.Update();
                }
                else
                {
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtSODB.ClientID);
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnSaveUp_Click(object sender, EventArgs e)
        {
            //lock cap nhap chi so
            /*int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
            if (dung == true)
            {
                CloseWaitingDialog();
                ClearForm();
                BindGrid();
                ShowInfor("Đã khoá sổ ghi chỉ số.");
                return;
            }
            */


            if (!HasPermission(Functions.KH_ThayDongHo, Permission.Update))
            {
                CloseWaitingDialog();
                ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                return;
            }

            /*if (!IsValidate())
            {
                CloseWaitingDialog();
                return;
            }*/

            try
            {
                
                var dh = _thaydonghoDao.Get(int.Parse(lblID.Text.Trim()));
                string h = lblIDKH.ClientID;
                if (dh == null)
                {
                    CloseWaitingDialog();
                    ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
                    return;

                }

                dh.CHISONGUNG = Convert.ToInt32(txtCSNGUNG.Text.Trim());
                dh.MTRUYTHU = Convert.ToInt32(txtTRUYTHU.Text.Trim());
                dh.MALDH = cboLoaiDh.Items.Count > 0 ? cboLoaiDh.SelectedValue : null;
                dh.MADH = txtMaDongho.Text.Trim();
                dh.CHISOBATDAU = Convert.ToInt32(txtCSBATDAU.Text.Trim());
                dh.CHISOMOI = Convert.ToInt32(txtCSMOI.Text.Trim());
                dh.NGAYTD = DateTimeUtil.GetVietNamDate(txtNgayThay.Text.Trim());
                dh.NGAYHT = DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim());
                dh.KICHCO = ddlKICHCODH.SelectedValue;

                var msg = _thaydonghoDao.UpThayDongHo(dh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
                var msg1 = _khDao.UpThayDongHo(dh, DateTimeUtil.GetVietNamDate(txtNgayHoanThanh.Text.Trim()), txtSoTem.Text.Trim(), txtGhiChu.Text.Trim(),
                                                CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);


                rp.UpdateTieuThuDH(lblIDKH.Text.Trim(), Convert.ToInt32(txtCSBATDAU.Text.Trim()), Convert.ToInt32(txtCSMOI.Text.Trim()), Convert.ToInt32(txtTRUYTHU.Text.Trim()));
                CloseWaitingDialog();

                if (!msg.MsgType.Equals(MessageType.Error))
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                    BindGrid();

                    upnlGrid.Update();
                }
                else
                {
                    ShowError("<strong>Lỗi xảy ra</strong>. <br/><br/>Dò lỗi: <br />" +
                        ResourceLabel.Get(msg), txtSODB.ClientID);
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtSODB_TextChanged(object sender, EventArgs e)
        {
            
            txtIDKH.Text = txtSODB.Text.Trim();
            BindKhachHang();
            //upnlKhachHang.Update();
            CloseWaitingDialog();
            

            /*
            var khachhang = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());            
            
            if (khachhang != null)
            {
                lblTENKH.Text = khachhang.TENKH;
                lblIDKH.Text = khachhang.IDKH;
                lblTENDP.Text = khachhang.DUONGPHO != null ? khachhang.DUONGPHO.TENDP : "";
                lblTENKV.Text = khachhang.KHUVUC != null ? khachhang.KHUVUC.TENKV : "";
                lblLOAITK.Text = khachhang.MALDH ;
                lblSONO.Text = khachhang.MADH ;
                //lblCSDAU.Text = tieuthu.CHISODAU.ToString();
                //lblCSCUOI.Text = tieuthu.CHISOCUOI.ToString();
                

                CloseWaitingDialog();
                txtSODB.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
            }*/
        }
        
        private void BindGrid()
        {
            /*
            int thang, nam;
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                thang = int.Parse(ddlTHANG.SelectedValue);
                nam = int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                return;
            }
            */
            int thang = int.Parse(ddlTHANG1.SelectedValue);
            var list = _khDao.GetThayDongHoListThang(thang);
            //var list = _khDao.GetThayDongHoList();
            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            upnlGrid.Update();
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvKhachHang.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindTDH(THAYDONGHO thaydh)
        {
            lblLOAITK.Text = thaydh.MALDHCU;
            lblSONO.Text = thaydh.MADHCU;
            txtCSNGUNG.Text = thaydh.CHISONGUNG.ToString();
            txtTRUYTHU.Text = thaydh.MTRUYTHU.ToString();
            cboLoaiDh.SelectedValue = thaydh.MALDH;
            txtMaDongho.Text = thaydh.MADH;
            txtCSBATDAU.Text = thaydh.CHISOBATDAU.ToString();
            txtCSMOI.Text = thaydh.CHISOMOI.ToString();
            txtNgayThay.Text = thaydh.NGAYTD.HasValue ? thaydh.NGAYTD.Value.ToString("dd/MM/yyyy") : "";
            txtNgayHoanThanh.Text = thaydh.NGAYHT.HasValue ? thaydh.NGAYHT.Value.ToString("dd/MM/yyyy") : "";
            ddlKICHCODH.SelectedValue = thaydh.KICHCO;
            txtGhiChu.Text = thaydh.GHICHU;

            upnlThongTin.Update();
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                var tdh = _thaydonghoDao.Get(int.Parse(id));

                switch (e.CommandName)
                {
                    case "SelectTDH":
                        var khachhang = _khDao.Get(tdh.IDKH);
                        if (khachhang != null)
                        {
                            BindStatusTDH(khachhang);
                            BindTDH(tdh);
                            HideDialog("divKhachHang");
                            lblID.Text = id;
                            CloseWaitingDialog();
                            txtSODB.Focus();
                        }
                        btnSaveUp.Visible = true;
                        btnSave.Visible = false;

                        CloseWaitingDialog();
                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        /*
        protected void btnSearch_Click(object sender, EventArgs e)
        {
            int thang, nam;
            try
            {
                DateTime.ParseExact(txtNAM.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                thang = int.Parse(ddlTHANG.SelectedValue);
                nam = int.Parse(txtNAM.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập năm hợp lệ.", txtNAM.ClientID);
                return;
            }

            var list = _khDao.GetThayDongHoList(thang, nam);
            gvKhachHang.DataSource = list;
            gvKhachHang.PagerInforText = list.Count.ToString();
            gvKhachHang.DataBind();

            ClearForm();

            upnlGrid.Update();
            CloseWaitingDialog();
        }
        */


        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
               
                // Authenticate
                if (!HasPermission(Functions.KH_ThayDongHo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<THAYDONGHO>();
                    var listIds = strIds.Split(',');

                   
                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _thaydonghoDao.Get(int.Parse(ma))));

                    //TODO: check relation before update list
                    var msg = _thaydonghoDao.DoAction(objs, PageAction.Delete, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

                    if (msg.MsgType != MessageType.Error)
                    {
                        CloseWaitingDialog();

                        ShowInfor(ResourceLabel.Get(msg));

                       
                        // Refresh grid view
                        BindGrid();

                        upnlGrid.Update();
                    }
                    else
                    {
                        CloseWaitingDialog();

                        ShowError(ResourceLabel.Get(msg));
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Chọn record để xóa.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBrowseDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
        }

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            //BindDongHoSoNo();
            //CloseWaitingDialog();
            BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
        }

        private void BindDongHoSoNo()
        {
            var list = dhDao.GetListDASD(txtKeywordDHSONO.Text.Trim());
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
                        var dh = dhDao.Get(id);
                        if (dh != null)
                        {
                            SetControlValue(txtMaDongho.ClientID, dh.MADH);
                            //SetControlValue(txtMALDH.ClientID, dh.MALDH);
                            SetLabel(lbSONODH.ClientID, dh.SONO);
                            //SetLabel(lblKICHCO.ClientID, ldhDao.Get(dh.MALDH).ToString());
                            /*var ldhkc = _loaiDhDao.GetListldh(dh.MALDH);
                            foreach (var kc in ldhkc)
                            {
                                //string a = kc.KICHCO;
                                SetLabel(lblKICHCO.ClientID, kc.KICHCO);
                            }*/

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

        protected void txtNgayThay_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string dateht = txtNgayThay.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);

                if (dateht.Length != 10 && dateht.Length == 6)
                {
                    txtNgayThay.Text = ngayht + "/" + thanght + "/20" + namht;
                }
                
                //upnlThongTin.Update();
                //txtNgayHoanThanh.Focus();
            }
            catch { ShowWarning("Ngày lắp đặt không hợp lệ"); }        
        }

        protected void txtNgayHoanThanh_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string dateht = txtNgayHoanThanh.Text.Trim();
                string ngayht = dateht.Substring(0, 2);
                string thanght = dateht.Substring(2, 2);
                string namht = dateht.Substring(4, 2);

                if (dateht.Length != 10 && dateht.Length==6)
                {
                    txtNgayHoanThanh.Text = ngayht + "/" + thanght + "/20" + namht;
                }

                
                //upnlThongTin.Update();
                //txtGhiChu.Focus();
            }
            catch { ShowWarning("Ngày lắp đặt không hợp lệ"); }  
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();

            ClearForm();
            CloseWaitingDialog();
            upnlGrid.Update();
        }

    }
}