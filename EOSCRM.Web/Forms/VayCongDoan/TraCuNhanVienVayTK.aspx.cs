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

namespace EOSCRM.Web.Forms.VayCongDoan
{
    public partial class TraCuNhanVienVayTK : Authentication
    {
        private readonly NhanVienVayDao _nvvDao = new NhanVienVayDao();
        private readonly KyTietKiemDao _ktkDao = new KyTietKiemDao();

        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly CongViecDao _cvDao = new CongViecDao();
        private readonly ReportClass _rpClass = new ReportClass();

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

        private NVVAYCD ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var nv = new NVVAYCD
                {
                    MANVV = _nvvDao.NewId(),
                    HOTEN = txtHOTEN.Text.Trim(),
                    MAPB = ddlPHONGBAN.SelectedValue,
                    MACV = ddlCONGVIEC.SelectedValue,
                    MAKV = ddlKHUVUC.SelectedValue,
                    DIACHI = txtDIACHI.Text.Trim(),
                    SODT = txtDIENTHOAI.Text.Trim(),
                    KYBATDAU = DateTimeUtil.GetVietNamDate("01/" + txtNGAYBATDAU.Text.Trim()),
                    KYKETTHUC = DateTimeUtil.GetVietNamDate("01/" + txtNGAYKETTHUC.Text.Trim())
                };

                if (!string.IsNullOrEmpty(txtNGAYSINH.Text.Trim()))
                    nv.NGAYSINH = DateTimeUtil.GetVietNamDate(txtNGAYSINH.Text);

                return nv;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.VAY_NhanVienVay, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    // Bind data for grid view
                    LoadStaticReferences();
                    BindTongTien();
                    //BindDataForGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_VAY_NVVAYCD;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_VAYCONGDOAN;
                header.TitlePage = Resources.Message.PAGE_VAY_NHANVIENVAY;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvNVVTK);
            CommonFunc.SetPropertiesForGrid(gvTTNVTK);
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

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void UnblockWaitingDialog()
        {
            ((EOS)Page.Master).UnblockWaitingDialog();
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion


        private void BindDataForGrid()
        {
            try
            {
                if (Filtered == FilteredMode.None)
                {
                    var objList = _nvvDao.GetListTraCuuNV();

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var objList = _nvvDao.GetListTraCuuNVM(txtMANV.Text.Trim(), txtHOTEN.Text.Trim(), ddlPHONGBAN.SelectedValue);
                    //var objList = _nvvDao.GetListTraCuuNV();
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
            }
            catch { }
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
                if (b == "chau")
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {

                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                }
            }
            /*var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = _kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {                   
                        var kvList = _kvDao.GetListKV(d);
                        ddlKHUVUC.Items.Clear();
                        foreach (var kv in kvList)
                        {
                            ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        }  
                }
            }*/
        }

        private void LoadStaticReferences()
        {
            // bind khu vuc
            /*var kvList = _kvDao.GetList();
            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
            foreach(var kv in kvList)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }*/
            timkv();

            // bind phong ban
            var pbList = _pbDao.GetList();
            ddlPHONGBAN.Items.Clear();
            ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var pb in pbList)
            {
                ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
            }

            // bind cong viec
            var cvList = _cvDao.GetList();
            ddlCONGVIEC.Items.Clear();
            ddlCONGVIEC.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var cv in cvList)
            {
                ddlCONGVIEC.Items.Add(new ListItem(cv.TENCV, cv.MACV));
            }

            txtNGAYBATDAU.Text = DateTime.Now.ToString("MM/yyyy");
            txtNGAYKETTHUC.Text = DateTime.Now.ToString("MM/yyyy");
            lbTONGTIENDONG.Text = "";
            lbTONGTHU.Text = "";
            lbTONGLAIDONG.Text = "";

        }

        private bool ValidateData()
        {

            if (string.Empty.Equals(txtHOTEN.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên nhân viên"), txtMANV.ClientID);
                return false;
            }

            var kv = _kvDao.Get(ddlKHUVUC.SelectedValue);
            if (kv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khu vực"));
                return false;
            }

            var pb = _pbDao.Get(ddlPHONGBAN.SelectedValue);
            if (pb == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Phòng ban"));
                return false;
            }

            var cv = _cvDao.Get(ddlCONGVIEC.SelectedValue);
            if (cv == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Công việc"));
                return false;
            }

            if (!string.IsNullOrEmpty(txtNGAYSINH.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYSINH.Text);
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày sinh"), txtNGAYSINH.ClientID);
                    return false;
                }
            }

            return true;
        }

        private void ClearForm()
        {
            UpdateMode = Mode.Create;

            txtMANV.Text = "";
            txtMANV.Focus();
            txtMANV.ReadOnly = false;

            txtHOTEN.Text = "";
            ddlCONGVIEC.SelectedIndex = 0;
            //ddlKHUVUC.SelectedIndex = 0;
            ddlPHONGBAN.SelectedIndex = 0;

            txtNGAYSINH.Text = "";
            txtDIENTHOAI.Text = "";
            txtDIACHI.Text = "";

            txtNGAYBATDAU.Text = DateTime.Now.ToString("MM/yyyy");
            txtNGAYKETTHUC.Text = DateTime.Now.ToString("MM/yyyy");
            lbTONGTIENDONG.Text = "";
            lbTONGTHU.Text = "";
            lbTONGLAIDONG.Text = "";

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

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.None;
            ClearForm();
            //BindDataForGrid();

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var nv = ItemObj;
            if (nv == null)
            {
                CloseWaitingDialog();
                return;
            }

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            Message msg;
            Filtered = FilteredMode.None;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.VAY_NhanVienVay, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _nvvDao.Get(txtMANV.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã nhân viên đã tồn tại", txtMANV.ClientID);
                    return;
                }

                msg = _nvvDao.Insert(nv, DateTimeUtil.GetVietNamDate("01/" + txtNGAYBATDAU.Text.Trim()), DateTimeUtil.GetVietNamDate("01/" + txtNGAYKETTHUC.Text.Trim()), b);
            }
            else
            {
                if (!HasPermission(Functions.VAY_NhanVienVay, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = _nvvDao.Update(nv, txtMANV.Text.ToString());
            }

            BindDataForGrid();
            upnlGrid.Update();

            CloseWaitingDialog();

            if (msg == null) return;

            if (msg.MsgType != MessageType.Error)
            {
                ShowInfor(ResourceLabel.Get(msg));
                ClearForm();

                // Refresh grid view
                //BindDataForGrid();

                upnlGrid.Update();
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            try
            {
                Filtered = FilteredMode.Filtered;
                BindDataForGrid();

                upnlGrid.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {

        }

        private void BindItem(NVVAYCD obj)
        {
            if (obj == null)
                return;

            txtMANV.Text = obj.MANVV;

            SetControlValue(txtHOTEN.ClientID, obj.HOTEN);

            var kv = ddlKHUVUC.Items.FindByValue(obj.MAKV);
            if (kv != null)
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);

            var pb = ddlPHONGBAN.Items.FindByValue(obj.MAPB);
            if (pb != null)
                ddlPHONGBAN.SelectedIndex = ddlPHONGBAN.Items.IndexOf(pb);

            var cv = ddlCONGVIEC.Items.FindByValue(obj.MACV);
            if (cv != null)
                ddlCONGVIEC.SelectedIndex = ddlCONGVIEC.Items.IndexOf(cv);

            SetControlValue(txtNGAYSINH.ClientID, obj.NGAYSINH.HasValue ? obj.NGAYSINH.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtDIACHI.ClientID, obj.DIACHI);
            SetControlValue(txtDIENTHOAI.ClientID, obj.SODT);

            SetControlValue(txtNGAYBATDAU.ClientID, obj.KYBATDAU.Value.ToString("MM/yyyy"));
            SetControlValue(txtNGAYKETTHUC.ClientID, obj.KYKETTHUC.Value.ToString("MM/yyyy"));

            if (_ktkDao.GetSumTien(obj.MANVV) != null)
            {
                int sumtien = Int32.Parse(_ktkDao.GetSumTien(obj.MANVV));
                lbTONGTIENDONG.Text = sumtien.ToString("#,###");
            }
            else
            { lbTONGTIENDONG.Text = "0"; }

            if (_ktkDao.GetSumTienThu(obj.MANVV) != null)
            {
                int sumtt = Int32.Parse(_ktkDao.GetSumTienThu(obj.MANVV));
                lbTONGTHU.Text = sumtt.ToString("#,###");
            }
            else { lbTONGTHU.Text = "0"; }


            if (_nvvDao.SumLaiNV(obj.MANVV) != null)
            {
                int sumtt2 = Int32.Parse(_nvvDao.SumLaiNV(obj.MANVV).ToString() == "" ? "0" : _nvvDao.SumLaiNV(obj.MANVV).ToString());
                lbTONGLAIDONG.Text = sumtt2.ToString("#,###");
            }
            else { lbTONGLAIDONG.Text = "0"; }

            upnlInfor.Update();
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
                        var objDb = _nvvDao.Get(id);

                        if (objDb != null)
                        {
                            BindItem(objDb);
                            UpdateMode = Mode.Update;
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

        protected void gvNVVTK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvNVVTK.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindNVVTK();
                upnlNVVTK.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void BindNVVTK()
        {
            try
            {
                var list = _rpClass.LSNHANVIENVAYTK(txtMANV.Text.Trim());
                gvNVVTK.DataSource = list;
                gvNVVTK.DataBind();
            }
            catch { }
        }

        protected void lnkNVNTK_Click(object sender, EventArgs e)
        {
            BindNVVTK();
            upnlNVVTK.Update();
            UnblockDialog("divNVVTK");
        }

        protected void lnkTTNVTK_Click(object sender, EventArgs e)
        {
            BindTTNVTK();
            upnlTTNVTK.Update();
            UnblockDialog("divTTNVTK");
        }

        protected void gvTTNVTK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvTTNVTK.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindTTNVTK();
                upnlTTNVTK.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void BindTTNVTK()
        {
            try
            {
                var objList = _nvvDao.GetListKyVayMN(txtMANV.Text.Trim());

                gvTTNVTK.DataSource = objList;
                gvTTNVTK.PagerInforText = objList.Count.ToString();
                gvTTNVTK.DataBind();
            }
            catch { }
        }

        protected void BindTongTien()
        {  

            if (_ktkDao.GetSumTienThu2() != null)
            {
                int sumtt2 = Int32.Parse(_ktkDao.GetSumTienThu2().ToString() == "" ? "0" : _ktkDao.GetSumTienThu2().ToString());
                lbTHUTIETKIEM.Text = sumtt2.ToString("#,###");
            }
            else { lbTHUTIETKIEM.Text = "0"; }

            if (_nvvDao.SumTongTienLai() != null)
            {
                int sumttl2 = (int)_nvvDao.SumTongTienLaiInt();
                lbTHUTIENLAI.Text = sumttl2.ToString("#,###");
            }
            else { lbTHUTIENLAI.Text = "0"; }

            //var kynaym = new DateTime(int.Parse(DateTime.Now.Year.ToString()), int.Parse((DateTime.Now.Month - 1).ToString()), 1);
            if (_nvvDao.SumConLaiKyMax() != null && _ktkDao.GetSumTienThu2() != null)
            {
                int tienthu = Int32.Parse(_ktkDao.GetSumTienThu2().ToString() == "" ? "0" : _ktkDao.GetSumTienThu2().ToString());
                int conlaiky = Int32.Parse(_nvvDao.SumConLaiKyMax().ToString() == "" ? "0" : _nvvDao.SumConLaiKyMax().ToString());

                lbTONGTIENCONLAI.Text = (tienthu - conlaiky).ToString("#,###");
                lbCHICONLAI.Text = conlaiky.ToString("#,###");
            }
            else
            {
                lbTONGTIENCONLAI.Text = "0";
                lbCHICONLAI.Text = "0";
            }
        }


    }
}