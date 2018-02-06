using System;
using System.Web.UI.WebControls;
using System.Globalization;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class NhapKHCDoc : Authentication
    {
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly DotInHDDao _diDao = new DotInHDDao();
        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly HopDongDao _hdDao = new HopDongDao();

        #region co loc, up
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

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    //BindKhachHangGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        //KH_NhapKHMoiCDoc = 452,     
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_NHAPKHMCHAUDOC;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_NHAPKHMCHAUDOC;
            }

            CommonFunc.SetPropertiesForGrid(gvHopDong);
            CommonFunc.SetPropertiesForGrid(gvKhachHang);
        }

        private void LoadStaticReferences()
        {
            try
            {
                ClearForm();
                timkv();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var dotin = _diDao.GetListKVNN(_nvDao.Get(b).MAKV);
                ddlDOTGCS.Items.Clear();
                ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
                foreach (var d in dotin)
                {
                    ddlDOTGCS.Items.Add(new System.Web.UI.WebControls.ListItem(_dmdiDao.Get(d.MADOTIN).TENDOTIN, d.IDMADOTIN));
                }
            }
            catch { }
        }

        private void ClearForm()
        {
            //hdfIDKH.Value = "";
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
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
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC1.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC1.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        protected void gvHopDong_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvHopDong.PageIndex = e.NewPageIndex;
                BindHD();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvHopDong_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            /*try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADDK":
                        var hd = hdDao.Get(id);
                        if (hd != null)
                        {
                            UnblockWaitingDialog();
                            BindHopDongToForm(hd);
                            CloseWaitingDialog();

                            HopDong = hd;

                            HideDialog("divHopDong");
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }*/
        }

        protected void gvHopDong_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("linkMa") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        private void BindHD()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var list = _hdDao.GetListNNKVTSON("", false, _nvDao.Get(b).MAKV);

                gvHopDong.DataSource = list;
                gvHopDong.PagerInforText = list.Count.ToString();
                gvHopDong.DataBind();
            }
            catch { }
        }

        protected void btnBrowseSOHD_Click(object sender, EventArgs e)
        {
            try
            {
                BindHD();
                upnlHopDong.Update();
                UnblockDialog("divHopDong");
            }
            catch { }
        }

        private void BindKhachHangGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());     
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + ddlTHANG.SelectedValue + "/" + txtNAM.Text.Trim());    

                //var list = _rpClass.BienKHNuoc("", _nvDao.Get(b).MAKV, ddlTHANG.SelectedValue + "/01/" + txtNAM.Text.Trim(), "", 
                //Int16.Parse(ddlTHANG.SelectedValue.ToString()), Int32.Parse(txtNAM.Text.Trim()), "KHCBIKTTS");
                var list = _rpClass.BienKHNuoc(ddlDOTGCS.SelectedValue, _nvDao.Get(b).MAKV, ddlTHANG.SelectedValue + "/01/" + txtNAM.Text.Trim(), "",
                    Int16.Parse(ddlTHANG.SelectedValue.ToString()), Int32.Parse(txtNAM.Text.Trim()), "KHCBIKTCD");

                gvKhachHang.DataSource = list;
                //gvKhachHang.PagerInforText = list.Count.ToString();
                gvKhachHang.DataBind();

                upnlCustomers.Update();
            }
            catch { }
        }

        protected void gvKhachHang_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                var kh = _khDao.Get(id);

                switch (e.CommandName)
                {
                    case "DeleteKhachHang":
                        UnblockWaitingDialog();

                        if (kh.MADDK != null)
                        {
                            var deletingTieuThu = _ttDao.GetTNKV(kh.IDKH, Convert.ToInt16(ddlTHANG.SelectedValue), 
                                    Convert.ToInt32(txtNAM.Text.Trim()), kh.MAKV);
                            if (deletingTieuThu == null)
                            {                                
                                return;
                            }
                            _ttDao.Delete(deletingTieuThu);

                            var delteKhacHang = _khDao.Get(kh.IDKH);
                            if (delteKhacHang == null)
                                return;
                            _khDao.Delete(delteKhacHang, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                            BindKhachHangGrid();

                            //ShowInfor("Xóa khách hàng khai thác nhầm thành công.");
                        }
                       
                        CloseWaitingDialog();
                        break;
                } 
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvKhachHang.PageIndex = e.NewPageIndex;
                BindKhachHangGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvKhachHang_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;


            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;

            var txtMADPKHM = e.Row.FindControl("txtMADPKHM") as TextBox;
            var txtMADBKHM = e.Row.FindControl("txtMADBKHM") as TextBox;
              
            var txtCHISODAUKHM = e.Row.FindControl("txtCHISODAUKHM") as TextBox;
            var txtCHISOCUOIKHM = e.Row.FindControl("txtCHISOCUOIKHM") as TextBox;

            if (hfGCS == null || txtMADPKHM == null || txtMADBKHM == null || 
                    txtCHISODAUKHM == null || txtCHISOCUOIKHM == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtMADPKHM.ClientID +
                                                                "\", \"" + txtMADBKHM.ClientID +
                                                                "\", \"" + txtCHISODAUKHM.ClientID +
                                                                "\", \"" + txtCHISOCUOIKHM.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";

            txtMADPKHM.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtMADBKHM.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtCHISODAUKHM.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            txtCHISOCUOIKHM.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");

            txtMADPKHM.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtMADPKHM.ClientID + "\");");
            txtMADBKHM.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtMADBKHM.ClientID + "\");");
            txtCHISODAUKHM.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISODAUKHM.ClientID + "\");");
            txtCHISOCUOIKHM.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtCHISOCUOIKHM.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + txtMADPKHM.ClientID +
                                                                "\", \"" + txtMADBKHM.ClientID +
                                                                "\", \"" + txtCHISODAUKHM.ClientID +
                                                                "\", \"" + txtCHISOCUOIKHM.ClientID +
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            //txtCHISODAU.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
            //ddlHTTT.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");
            //ddlTHANHTOAN.Attributes.Add("onchange", onSelectedIndexChangedEventHandler + ");");



            //var lnkBtnID = e.Row.FindControl("linkMaKHM") as LinkButton;
            //if (lnkBtnID == null) return;
            //lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void btnKHOITAOKH_Click(object sender, EventArgs e)
        {
            try
            {
                _rpClass.BienKHNuoc("", "", "", "", 0, 0, "");

            }
            catch { }
        }

        protected void txtNAM_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btLOCDSKHCBKT_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var querykv = _nvDao.GetKV(b);

                //int thangIndex = 0;
                //if (DateTime.Now.Year == int.Parse(txtNAM.Text.Trim()))
                //{
                //    thangIndex = 1;
                //}
                //else
                //{
                //    thangIndex = DateTime.Now.Month ;
                //}

                int namIndex = DateTime.Now.Year - 1;
                //lock cap nhap chi so
                int thang1 = DateTime.Now.Month;
                string nam = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);
                var kynay = new DateTime(int.Parse(nam), thang1, 1);
                //var kynay = new DateTime(2013, 6, 1);

                //bool dung = _gcsDao.IsLockTinhCuocKy(kynay, querykv.MAKV);//khoa so trong ky
                bool dung = _gcsDao.IsLockTinhCuocKy(kynay, querykv.MAKV);//khoa so trong ky

                /*BindKhachHangGrid();
                CloseWaitingDialog();
                upnlCustomers.Update();*/

                if (txtNAM.Text == Convert.ToString(nam))// || txtNAM.Text == Convert.ToString(namIndex))
                {
                    if (int.Parse(ddlTHANG.SelectedValue) == thang1)
                    {
                        //BindKhachHangGrid();
                        //CloseWaitingDialog();
                        //upnlCustomers.Update();

                        if (dung == false)
                        {
                            BindKhachHangGrid();
                            CloseWaitingDialog();
                            upnlCustomers.Update();
                        }
                        else
                        {
                            CloseWaitingDialog();
                            HideDialog("divKhachHang");
                            ShowInfor("Đã khoá sổ. Không được Nhập khách hàng mới.");
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        HideDialog("divKhachHang");
                        ShowInfor("Chọn kỳ nhập khách hàng mới cho đúng.");
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    HideDialog("divKhachHang");
                    ShowInfor("Chọn năm nhập khách hàng mới cho đúng.");
                }
            }
            catch { }
        }
         

    }
}