using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.GhiChiSo
{
    public partial class DieuChinhHDTangG : Authentication
    {
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly TieuThuDCTGDao _ttdctgDao = new TieuThuDCTGDao();
        private readonly ReportClass _rp = new ReportClass();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.GCS_DieuChinhHDTG, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_GCS_DIEUCHINHHDTG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_GHICHISO;
                header.TitlePage = Resources.Message.PAGE_GCS_DIEUCHINHHDTG;
            }

            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvDieuChinhHD);            
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
                ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM.Text = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

                var listMucDichSuDung = new MucDichSuDungDao().GetList();
                ddlMDSD.DataSource = listMucDichSuDung;
                ddlMDSD.DataTextField = "TENMDSD";
                ddlMDSD.DataValueField = "MAMDSD";
                ddlMDSD.DataBind();

                txtMASOHD.Text = "00";

                timkv();

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
        private void UnblockWaitingDialog()
        {
            ((EOS)Page.Master).UnblockWaitingDialog();
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
            int thangIndex = 0;
            if (DateTime.Now.Month == 1)
            {
                thangIndex = 11;
            }
            else
            {
                thangIndex = DateTime.Now.Month - 2;
            }

            int namIndex = DateTime.Now.Year - 1;
            //lock cap nhap chi so
            int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            bool dung = gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);
            //bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), );

            if (txtNAM.Text == Convert.ToString(nam) || txtNAM.Text == Convert.ToString(namIndex))
            {
                if (ddlTHANG.SelectedIndex == thangIndex + 1) //lay thang, nam hien tai
                {
                    UnblockDialog("divKhachHang");
                    BindKhachHang();
                    upnlKhachHang.Update();

                    //if (dung == false)
                    //{
                    //    UnblockDialog("divKhachHang");
                    //    BindKhachHang();
                    //    upnlKhachHang.Update();
                    //}
                    //else
                    //{
                    //    CloseWaitingDialog();
                    //    HideDialog("divKhachHang");
                    //    ShowInfor("Đã khoá sổ. Không được điều chỉnh.");
                    //}
                }
                else
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowInfor("Chọn kỳ điều chỉnh cho đúng. Chọn kỳ hiện tại.");
                }
            }
            else
            {
                CloseWaitingDialog();
                HideDialog("divKhachHang");
                ShowInfor("Chọn năm điều chỉnh cho đúng.");
            }

        }

        private void BindStatus(KHACHHANG kh)
        {
            SetControlValue(txtSODB.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            SetLabel(lblIDKH.ClientID, kh.IDKH);
            lblIDKH.Text = kh.IDKH;
            SetLabel(lblTENDP.ClientID, kh.DUONGPHO != null ? kh.DUONGPHO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUC != null ? kh.KHUVUC.TENKV : "");
            SetLabel(lblMAMDSD.ClientID, kh.MAMDSD);
            var tieuthu = _ttDao.GetTN(kh.IDKH, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
            if (tieuthu != null)
            {
                SetLabel(lblCSMOI.ClientID, Convert.ToString(tieuthu.CHISOCUOI));
                SetLabel(lblCSCU.ClientID, Convert.ToString(tieuthu.CHISODAU));
                SetLabel(lblTIEUTHU.ClientID, Convert.ToString(tieuthu.KLTIEUTHU));
                SetLabel(lblTHANHTIEN.ClientID, Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENNUOC)));
                SetLabel(lblTHUEGTGT.ClientID, Convert.ToString(String.Format("{0:#.##}", tieuthu.TIENTHUE)));
                SetLabel(lblTONGTIEN.ClientID, Convert.ToString(String.Format("{0:#.####}", tieuthu.TONGTIEN)));
                
                txtCSCU.Text = Convert.ToString(tieuthu.CHISODAU).ToString();

                upnlThongTin.Update();
                CloseWaitingDialog();
                txtMASOHD.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không có tiêu thụ trong tháng này. Xin chọn khách hàng lại", txtSODB.ClientID);
            }
            upnlThongTin.Update();
        }

        private void BindStatusKHDC(KHACHHANG kh)
        {

            SetControlValue(txtSODB.ClientID, kh.MADP + kh.DUONGPHU + kh.MADB);
            SetLabel(lblTENKH.ClientID, kh.TENKH);
            SetLabel(lblIDKH.ClientID, kh.IDKH);
            lblIDKH.Text = kh.IDKH;
            SetLabel(lblTENDP.ClientID, kh.DUONGPHO != null ? kh.DUONGPHO.TENDP : "");
            SetLabel(lblTENKV.ClientID, kh.KHUVUC != null ? kh.KHUVUC.TENKV : "");
            SetLabel(lblMAMDSD.ClientID, kh.MAMDSD);
            
            upnlThongTin.Update();
        }

        private void BindDC(TIEUTHUDCTG dc)
        {

            SetLabel(lblCSMOI.ClientID, Convert.ToString(dc.CHISOCUOI));
            SetLabel(lblCSCU.ClientID, Convert.ToString(dc.CHISODAU));
            SetLabel(lblTIEUTHU.ClientID, Convert.ToString(dc.KLTIEUTHU));
            SetLabel(lblTHANHTIEN.ClientID, Convert.ToString(String.Format("{0:#.##}", dc.TIENNUOC)));
            SetLabel(lblTHUEGTGT.ClientID, Convert.ToString(String.Format("{0:#.##}", dc.TIENTHUE)));
            SetLabel(lblTONGTIEN.ClientID, Convert.ToString(String.Format("{0:#.####}", dc.TONGTIEN)));

            ddlTHANG.SelectedValue = dc.THANG.ToString();
            txtNAM.Text = dc.NAM.ToString();
            ddlKHUVUC1.SelectedValue = dc.MAKV;
            txtMASOHD.Text = dc.MASOHDDC;
            txtCSMOI.Text = dc.CHISOCUOIDC.ToString();
            txtCSCU.Text = dc.CHISODAUDC.ToString();
            txtGhiChu.Text = dc.GHICHUDC;
            ddlMDSD.SelectedValue = dc.MAMDSD;

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
                            txtMASOHD.Focus();
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
                gvDanhSach.PageIndex = e.NewPageIndex;               
                BindKhachHang();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        protected void txtSODB_TextChanged(object sender, EventArgs e)
        {
            
            var khachhang = _khDao.GetKhachHangFromMadb(lblIDKH.Text.Trim());
            var tieuthu = _ttDao.GetTN(khachhang.IDKH, ddlTHANG.SelectedIndex + 1, int.Parse(txtNAM.Text.Trim()));

            if (khachhang != null)
            {
                lblTENKH.Text = khachhang.TENKH;
                lblIDKH.Text = khachhang.IDKH;
                lblTENDP.Text = khachhang.DUONGPHO != null ? khachhang.DUONGPHO.TENDP : "";
                lblTENKV.Text = khachhang.KHUVUC != null ? khachhang.KHUVUC.TENKV : "";
                lblCSMOI.Text = Convert.ToString(tieuthu.CHISOCUOI);
                lblCSCU.Text = Convert.ToString(tieuthu.CHISODAU);
                lblTIEUTHU.Text = Convert.ToString(tieuthu.KLTIEUTHU);
                lblTHANHTIEN.Text = Convert.ToString(tieuthu.TIENNUOC);
                lblTHUEGTGT.Text = Convert.ToString(tieuthu.TIENTHUE);
                lblTONGTIEN.Text = Convert.ToString(tieuthu.TONGTIEN);

                CloseWaitingDialog();
                txtSODB.Focus();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Khách hàng không tồn tại", txtSODB.ClientID);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).MADP);
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ClearForm();
                    BindGrid();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                if (!HasPermission(Functions.GCS_DieuChinhHoaDon, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                else
                {
                    if (!IsDataValid())
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    else
                    {
                        try
                        {

                            _rp.ThemTieuThuDCTG(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim(), ddlMDSD.SelectedValue.ToString());

                            var msg1 = gcsDao.TinhTienTTDCTG(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                            ShowInfor(ResourceLabel.Get(msg1));

                            CloseWaitingDialog();
                            ClearForm();
                            BindGrid();
                        }
                        catch
                        {
                            CloseWaitingDialog();
                            ShowError("Hoá đơn này đã điều chỉnh rồi.");
                        }
                    }
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
            try
            {
                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay1 = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                //bool dung = gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                bool dung = gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), _khDao.Get(lblIDKH.Text.Trim()).MADP);
                if (dung == true)
                {
                    CloseWaitingDialog();
                    ClearForm();
                    BindGrid();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                if (!HasPermission(Functions.GCS_DieuChinhHoaDon, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                else
                {
                    if (!IsDataValid())
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    else
                    {
                        try
                        {
                            _rp.UpTieuThuDCTG(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), int.Parse(txtCSCU.Text.Trim()),
                                int.Parse(txtCSMOI.Text.Trim()), txtGhiChu.Text.Trim(), txtMASOHD.Text.Trim());

                            var msg1 = gcsDao.TinhTienTTDCTG(lblIDKH.Text.Trim(), int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                            ShowInfor(ResourceLabel.Get(msg1));

                            CloseWaitingDialog();
                            ClearForm();
                            BindGrid();
                        }
                        catch
                        {
                            CloseWaitingDialog();
                            ShowError("Hoá đơn này đã điều chỉnh rồi.");
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                CloseWaitingDialog();
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void ClearForm()
        {
            txtMASOHD.Text = "00";
            txtGhiChu.Text = "";
            txtCSCU.Text = "";
            txtCSMOI.Text = "";
            
            ddlMDSD.SelectedIndex = 0;

            btnSaveUp.Visible = false;
            btnSave.Visible = true;
        }

        private bool IsDataValid()
        {
            if (string.Empty.Equals(txtMASOHD.Text.Trim()))
            {
                ShowError("Mã số hoá đơn không được rỗng.", txtMASOHD.ClientID);
                return false;
            }
            if (!string.Empty.Equals(txtCSMOI.Text.Trim()))
            {
                try
                {
                    int.Parse(txtCSMOI.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số mới phải là số.", txtCSMOI.ClientID);
                    return false;
                }
            }
            if (!string.Empty.Equals(txtCSCU.Text.Trim()))
            {
                try
                {
                    int.Parse(txtCSCU.Text.Trim());
                }
                catch
                {
                    ShowError("Chỉ số cũ phải là số.", txtCSCU.ClientID);
                    return false;
                }
            }
            return true;
        }

        protected void gvDieuChinhHD_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvDieuChinhHD.PageIndex = e.NewPageIndex;                
                BindGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDieuChinhHD_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectDC":
                        //var dc = _ttdcDao.Get(id);
                        var dc = _ttdctgDao.GetTNTG(id, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                        SetLabel(lblIDKH.ClientID, dc.IDKH);
                        lblIDKH.Text = dc.IDKH;

                        var khachhang = _khDao.Get(id);
                        if (khachhang != null)
                        {
                            BindStatusKHDC(khachhang);
                            BindDC(dc);
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

        protected void gvDieuChinhHD_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        private void BindGrid()
        {
            if (ddlKHUVUC1.SelectedValue == "99")
            {
                var list = _ttdctgDao.GetDCTG1(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                gvDieuChinhHD.DataSource = list;
                gvDieuChinhHD.PagerInforText = list.Count.ToString();
                gvDieuChinhHD.DataBind();

                upnlTTDC.Update();
                CloseWaitingDialog();
            }
            else
            {
                var list = _ttdctgDao.GetDCTG(ddlKHUVUC1.SelectedValue, int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()));
                gvDieuChinhHD.DataSource = list;
                gvDieuChinhHD.PagerInforText = list.Count.ToString();
                gvDieuChinhHD.DataBind();

                upnlTTDC.Update();
                CloseWaitingDialog();
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
                    ddlKHUVUC1.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                    btnNGUYENDC.Visible = true;
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGrid();
            ClearForm();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            LayBaoCao();
        }

        private void LayBaoCao()
        {
            var dtDSKTKY =
                new ReportClass().BKDieuChinh(int.Parse(ddlTHANG.SelectedValue), int.Parse(txtNAM.Text.Trim()), ddlKHUVUC1.SelectedValue.ToString()).Tables[0];
            if (dtDSKTKY.Rows.Count > 0)
            {               
                CloseWaitingDialog();
                Page.Response.Redirect(ResolveUrl("~") + "Forms/GhiChiSo/BaoCao/rpDieuChinhHoaDon.aspx");
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Không tìm thấy dữ liệu để làm báo cáo", "");
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnNGUYENDC_Click(object sender, EventArgs e)
        {
            int thangIndex = 0;
            if (DateTime.Now.Month == 1)
            {
                thangIndex = 11;
            }
            else
            {
                thangIndex = DateTime.Now.Month - 2;
            }

            int namIndex = DateTime.Now.Year - 1;
            //lock cap nhap chi so
            int thang1 = DateTime.Now.Month;
            string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
            var kynay = new DateTime(int.Parse(nam), thang1, 1);
            //var kynay = new DateTime(2013, 6, 1);
            bool dung = gcsDao.IsLockTinhCuocKy(kynay, ddlKHUVUC1.SelectedValue);

            UnblockDialog("divKhachHang");
            BindKhachHang();
            upnlKhachHang.Update();
        }

        protected void ddlMDSD_SelectedIndexChanged(object sender, EventArgs e)
        {
            UnblockWaitingDialog();            
            CloseWaitingDialog();
        }


    }
}