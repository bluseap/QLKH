using System;
using System.Web.UI.WebControls;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;

using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using System.Globalization;
using System.Web.UI.HtmlControls;
using EOSCRM.Controls;
using System.IO;


namespace EOSCRM.Web.Forms.VayCongDoan
{
    public partial class VayTietKiemCD : Authentication
    {
        private readonly NhanVienVayDao _nvvDao = new NhanVienVayDao();        
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KyTietKiemDao _ktkDao = new KyTietKiemDao();

        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.VAY_NhanVienVayCD, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    // Bind data for grid view
                    LoadStaticReferences();
                    BindDataForGrid();
                }
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
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (b == "nguyen")
                {
                    ddlTHANG1.Enabled = true;
                    txtNAM1.ReadOnly = false;
                    ddlTHANG2.Enabled = true;
                    txtNAM2.ReadOnly = false;
                }

                ddlTHANG1.SelectedIndex = DateTime.Now.Month - 3;
                txtNAM1.Text = DateTime.Now.Year.ToString();
                ddlTHANG2.SelectedIndex = DateTime.Now.Month - 2;
                txtNAM2.Text = DateTime.Now.Year.ToString();

                txtNGAYBATDAU.Text = DateTime.Now.ToString("MM/yyyy");
                txtNGAYKETTHUC.Text = DateTime.Now.ToString("MM/yyyy");
                lbLAISUAT.Text = "0.005";
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void ClearForm()
        {
            UpdateMode = Mode.Create;
            
            ddlTHANG1.SelectedIndex = DateTime.Now.Month - 3;
            txtNAM1.Text = DateTime.Now.Year.ToString();
            ddlTHANG2.SelectedIndex = DateTime.Now.Month - 2;
            txtNAM2.Text = DateTime.Now.Year.ToString();
            txtMANV.Text = "";
            lblTENNVVAY.Text = "";
            lblTENPB.Text = "";
            txtTIENCANVAY.Text = "";
            lbLAISUAT.Text = "0.005";
            txtNGAYBATDAU.Text = DateTime.Now.ToString("MM/yyyy");
            txtNGAYKETTHUC.Text = DateTime.Now.ToString("MM/yyyy");
            txtTIENGOC.Text = "";
            txtTIENLAI.Text = "";            

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

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_VAY_NVVAYCDVAY;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_VAYCONGDOAN;
                header.TitlePage = Resources.Message.PAGE_VAY_NHANVIENVAYCD;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvNhanVien);
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {

        }

        protected void btnSave_Click(object sender, EventArgs e)
        {         

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username; 

            Message msg;
            Filtered = FilteredMode.None;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                //kiem tra da vay hay chua
                var lvnv = _nvvDao.GetLanVayMaNVV(txtMANV.Text.ToString());
                if (lvnv != null)
                {
                    CloseWaitingDialog();
                    ShowInfor("Nhân viên nay đang vay. Xin chọn nhân viên chưa vay.");
                    ClearForm();
                    return;
                }

                if (!HasPermission(Functions.VAY_NhanVienVayCD, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    ClearForm();
                    return;
                }               

                //msg = _nvvDao.Insert(nv, DateTimeUtil.GetVietNamDate("01/" + txtNGAYBATDAU.Text.Trim()), DateTimeUtil.GetVietNamDate("01/" + txtNGAYKETTHUC.Text.Trim()), b);
                
                try
                {
                    string malvm = _nvvDao.NewIdLanVay().ToString();
                    decimal tienvaym = Decimal.Parse(txtTIENCANVAY.Text.Trim());
                    decimal laisuatm = Decimal.Parse(lbLAISUAT.Text.Trim());
                    decimal tiengocm = Decimal.Parse(txtTIENGOC.Text.Trim());
                    decimal tienlaim = Decimal.Parse(txtTIENLAI.Text.Trim());
                    _rpClass.VayLanTKCD(malvm, txtMANV.Text.ToString(), tienvaym, laisuatm,
                        DateTimeUtil.GetVietNamDate("01/" + txtNGAYBATDAU.Text.Trim()),
                        DateTimeUtil.GetVietNamDate("01/" + txtNGAYKETTHUC.Text.Trim()), b, tiengocm, tienlaim, "TT", "CO");

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = txtMANV.Text.ToString(),
                        IPAddress = "192.168.1.19",
                        MANV = b,
                        UserAgent = b,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.I.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "VV_A",
                        MOTA = "Nhập nhân viên vay quỹ tiết kiệm."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    ShowInfor("Nhập nhân viên vay quỹ tiết kiệm thành công.");
                }
                catch { }                
            }
            else
            {
                if (!HasPermission(Functions.VAY_NhanVienVayCD, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                //msg = _nvvDao.Update(nv, txtMANV.Text.ToString());
            }

            ClearForm();
            CloseWaitingDialog();           

            BindDataForGrid();
            upnlGrid.Update();

        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            CloseWaitingDialog();
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        protected void btnBrowseNVV_Click(object sender, EventArgs e)
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
                        var nv = _nvvDao.Get(id);
                        var pb = _pbDao.Get(nv.MAPB);
                        if (nv != null)
                        {
                            //SetControlValue(txtMANV.ClientID, nv.MANV);                            
                            txtMANV.Text = nv.MANVV;
                            lblTENNVVAY.Text = nv.HOTEN;
                            if (pb != null)
                            {
                                lblTENPB.Text = pb.TENPB.ToString();
                            }
                            else { lblTENPB.Text = ""; }

                            txtTIENCANVAY.Focus();

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

        private void BindNhanVien()
        {
            //var list = nvdao.Search(txtKeywordNV.Text.Trim());
            var list = _nvvDao.SearchKV(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
        }        

        protected void txtTIENCANVAY_TextChanged1(object sender, EventArgs e)
        {
            GetGocLai(txtTIENCANVAY.Text.Trim());
        }

        protected void GetGocLai(string tongtienvay)
        {
            try
            {
                if (!IsNumber(tongtienvay))
                {
                    ShowInfor("Bạn phải nhập số tiền chính xác. Ví dụ là 10000000 hoặc 5000000.");
                }

                var sotien = int.Parse(tongtienvay);
                int tienvay10 = 10000000;
                int tienvay5 = 5000000;

                if (sotien.Equals(tienvay10))
                {
                    txtTIENGOC.Text = "850000";
                    txtTIENLAI.Text = ((tienvay10 * 0.5) / 100).ToString();
                }
                if (sotien.Equals(tienvay5))
                {
                    txtTIENGOC.Text = "425000";
                    txtTIENLAI.Text = ((tienvay5 * 0.5) / 100).ToString();
                }
            }
            catch (System.Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        public bool IsNumber(string pValue)
        {
            foreach (Char c in pValue)
            {
                if (!Char.IsDigit(c))
                    return false;
            }
            return true;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
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

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        BindItem(id);
                        UpdateMode = Mode.Update; 
                        CloseWaitingDialog();

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindDataForGrid()
        {
            if (Filtered == FilteredMode.None)
            {
                var objList = _nvvDao.GetListLanVay();

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            else
            {
                var objList = _nvvDao.GetListLanVay();

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
        }

        protected void BindItem(string malv)
        {
            var lv = _nvvDao.GetLanVay(malv);
            var nvv = _nvvDao.Get(lv.MANVV);
            var pb = _pbDao.Get(nvv.MAPB);

            txtMANV.Text = nvv.MANVV;
            lblTENNVVAY.Text = nvv.HOTEN.ToString();
            lblTENPB.Text = pb.TENPB.ToString();

            txtTIENCANVAY.Text = lv.TIENVAY.ToString();
            txtNGAYBATDAU.Text = lv.KYBATDAU.Value.ToString("MM/yyyy");
            txtNGAYKETTHUC.Text = lv.KYKETTHUC.Value.ToString("MM/yyyy");

            GetGocLai(lv.TIENVAY.ToString());

            upnlInfor.Update();
        }        

        protected void btnKhoiTaoKyVay_Click(object sender, EventArgs e)
        {
            try
            {
                Message msg = null;
                var kydau = new DateTime(int.Parse(txtNAM1.Text.Trim()), int.Parse(ddlTHANG1.SelectedValue), 1);
                var kycuoi = new DateTime(int.Parse(txtNAM2.Text.Trim()), int.Parse(ddlTHANG2.SelectedValue), 1);
                
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var khoitaokyvay = _nvvDao.GetListKyVay(kycuoi.Year, kycuoi.Month);
                var khoitaokytk = _ktkDao.GetListKyTietKiem(kycuoi.Year, kycuoi.Month);

                if (khoitaokyvay != null && khoitaokytk != null)
                {
                    _rpClass.KhoiTaoKyVay(kydau, kycuoi, b);
                    //msg = _nvvDao.KhoiTaoKyVay(kydau, kycuoi);
                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = b,
                        IPAddress = "192.168.1.19",
                        MANV = b,
                        UserAgent = b,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.I.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "VK_A",
                        MOTA = "Khởi tạo nhân viên vay quỹ tiết kiệm."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    ShowInfor("Đã khởi tạo thành công.");
                }
                else
                {
                    ShowInfor("Kỳ này đã khởi tạo. Xin chọn kỳ khác.");
                    return;                    
                }
                CloseWaitingDialog();

                if (msg.MsgType.Equals(MessageType.Error))
                {
                    ShowError(ResourceLabel.Get(msg));
                }
                else
                {
                    ShowInfor(ResourceLabel.Get(msg));
                }  
            }
            catch { }
        }        



    }
}