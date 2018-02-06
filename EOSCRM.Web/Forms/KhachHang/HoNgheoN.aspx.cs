using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Globalization;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Data;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class HoNgheoN : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly MucDichSuDungDao _mdsdDao = new MucDichSuDungDao();        
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly HoNgheoNDao _hnnDao = new HoNgheoNDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly XaPhuongDao _xpDao = new XaPhuongDao();
        private readonly ReportClass _rpClass = new ReportClass();

        #region loc, up
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

        private HONGHEON ObjHN
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var hn = (string.IsNullOrEmpty(lbMANGHEO.Text.Trim()) || lbMANGHEO.Text == "") ? new HONGHEON() : _hnnDao.Get(lbMANGHEO.Text.Trim());
                if (hn == null)
                    return null;               

                hn.IDKH = txtMADDK.Text.Trim();  

                //so ho ngheo
                hn.MAXA = ddlTENXA.SelectedValue;      
         
                if (!txtKYHOTROHN.Text.Trim().Equals(String.Empty))
                {
                    hn.KYHOTROHN = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTROHN.Text.Trim() + "/" + txtNAMHOTRO.Text.Trim());
                }
                else { hn.KYHOTROHN = null; }

                hn.DIACHINGHEO = txtDIACHIHN.Text.Trim();
                hn.ISHONGHEO = ckISHONGHEO.Checked;                
                hn.DONVICAPHN = txtDONVICAP.Text.Trim();
                hn.MAHN = txtMASOHN.Text.Trim();
                if (!txtNGAPCAPHN.Text.Trim().Equals(String.Empty))
                    hn.NGAYCAPHN = DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim());
                else
                    hn.NGAYCAPHN = null;

                if (!txtNGAYKTHN.Text.Trim().Equals(String.Empty))
                    hn.NGAYKETTHUCHN = DateTimeUtil.GetVietNamDate(txtNGAYKTHN.Text.Trim());
                else
                    hn.NGAYKETTHUCHN = null;

                if (!txtNGAYKYSOHN.Text.Trim().Equals(String.Empty))
                    hn.NGAYKYHN = DateTimeUtil.GetVietNamDate(txtNGAYKYSOHN.Text.Trim());
                else
                    hn.NGAYKYHN = null;
                hn.NGAYNHAP = DateTime.Now;

                return hn;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_HoNgheoN, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                }

                if (reloadm.Text == "1")
                {
                    BaoCaoHN();
                    btnKHACHHANG.Visible = false;
                    btnSave.Visible = false;
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
                Filtered = FilteredMode.None;
                
                //ho ngheo
                ddlTENXA.Enabled = false;              
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                txtDIACHIHN.Enabled = false;
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;
                ckISHONGHEO.Visible = false;

                txtKYHOTROHN.Text = DateTime.Now.Month.ToString();
                txtNAMHOTRO.Text = DateTime.Now.Year.ToString();
                txtTUTHANG.Text = DateTime.Now.Month.ToString();
                txtTUNAM.Text = DateTime.Now.Year.ToString();
                txtDENTHANG.Text = DateTime.Now.Month.ToString();
                txtDENNAM.Text = DateTime.Now.Year.ToString();

                timkv();
                
                reloadm.Text = "0";

                divCR.Visible = false;
                upnlCrystalReport.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void Clear()
        {
            UpdateMode = Mode.Create;

            //so ho ngheo
            ckISHONGHEO.Checked = false;
            ddlTENXA.SelectedIndex = 0;
            txtDONVICAP.Text = ddlTENXA.SelectedValue;
            txtMASOHN.Text = "";
            txtNGAPCAPHN.Text = "";
            txtNGAYKTHN.Text = "";
            txtNGAYKYSOHN.Text = "";
            txtDIACHIHN.Text = "";
            ddlTENXA.Enabled = false;
            txtDONVICAP.Enabled = false;
            txtMASOHN.Enabled = false;
            txtNGAPCAPHN.Enabled = false;
            txtNGAYKTHN.Enabled = false;
            txtNGAYKYSOHN.Enabled = false;
            txtDIACHIHN.Enabled = false;
            ImageButton1.Visible = false;
            ImageButton2.Visible = false;
            ImageButton3.Visible = false;
            lbTENKHCU.Text = "";
            lbTENMDSD.Text = "";
            lbDANHSO.Text = "";

            lbGIAHANHN.Text = "";
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_HONGHEONUOC;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_HONGHEONUOC;
            }
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvLSGIAHANHNN);            
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
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUCMOI.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUCMOI.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }

                    //xa phuong
                    var listXAPHUONG = _xpDao.GetListKV(d);                    
                    ddlTENXA.DataSource = listXAPHUONG;
                    ddlTENXA.DataTextField = "TENXA";
                    ddlTENXA.DataValueField = "MAXA";
                    ddlTENXA.DataBind();

                    var tenxa = _xpDao.Get(ddlTENXA.SelectedValue, d);
                    txtDONVICAP.Text = tenxa.TENXA.ToString();
                }
            }
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

                        if (khachhang.ISHONGHEO == true)
                        {
                            ShowInfor("Khách hàng: " + khachhang.TENKH.ToString() + " đang nghèo. Xin chọn khách hàng khác.");
                            CloseWaitingDialog();
                            return;
                        }

                        if (khachhang != null)
                        {
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            BindStatus(khachhang);

                            ckISHONGHEO.Visible = true;
                            upnlInfor.Update();
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

        private void BindStatus(KHACHHANG kh)
        {
            var mdsd = _mdsdDao.Get(kh.MAMDSD);

            txtMADDK.Text = kh.IDKH.ToString();//idkh
            lbTENKHCU.Text = kh.TENKH.ToString();            

            lbTENMDSD.Text = mdsd.TENMDSD.ToString();
            lbDANHSO.Text = (kh.MADP + kh.MADB).ToString();
            lbMAMDSD.Text = kh.MAMDSD.ToString();//muc dich
            lbMANV.Text = LoginInfo.MANV.ToString();
            if (kh.MADDK != null)
            {
                lbMADDK.Text = kh.MADDK.ToString();
            }
            else { lbMADDK.Text = ""; }

            upnlInfor.Update();
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }       

        protected void btnKHACHHANG_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
            upnlKhachHang.Update();
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

            var lnkBtnID = e.Row.FindControl("lnkBtnMANGHEO") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "EditItem":                       
                        UpdateMode = Mode.Update;

                        BindNgheo(id);

                        CloseWaitingDialog();
                        upnlInfor.Update();
                        break;

                    case "S_GiaHan":
                        UpdateMode = Mode.Update;

                        BindNgheoGiaHan(id);

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

        private void BindNgheo(string mangheo)
        {
            try 
            {
                var hn = _hnnDao.Get(mangheo);
                lbMANGHEO.Text = mangheo;//mangheo
                var kh = _khDao.Get(hn.IDKH);
                txtMADDK.Text = kh.IDKH.ToString();//idkh
                lbTENKHCU.Text = kh.TENKH.ToString();
                var mdsd = _mdsdDao.Get(kh.MAMDSD);
                lbTENMDSD.Text = mdsd.TENMDSD.ToString();
                lbDANHSO.Text = (kh.MADP + kh.MADB).ToString();
                lbMAMDSD.Text = kh.MAMDSD.ToString();//muc dich
                lbMANV.Text = LoginInfo.MANV.ToString();
                if (kh.MADDK != null)
                {
                    lbMADDK.Text = kh.MADDK.ToString();
                }
                else { lbMADDK.Text = ""; }

                //so ho ngheo
                var isCheckedHN = hn.ISHONGHEO.HasValue && hn.ISHONGHEO.Value;
                ckISHONGHEO.Checked = isCheckedHN;
                if (isCheckedHN)
                {                    
                    txtKYHOTROHN.Text = hn.KYHOTROHN != null ? String.Format("{0:MM}", hn.KYHOTROHN.Value) : "";
                    txtNAMHOTRO.Text = hn.KYHOTROHN != null ? String.Format("{0:yyyy}", hn.KYHOTROHN.Value) : "";
                    //txtKYHOTROHN.Enabled = true;
                    //txtNAMHOTRO.Enabled = true;

                    if (hn.DONVICAPHN != null)
                    {
                        var pn = ddlTENXA.Items.FindByValue(hn.MAXA);
                        if (pn != null)
                            ddlTENXA.SelectedIndex = ddlTENXA.Items.IndexOf(pn);
                    }
                    else { ddlTENXA.SelectedIndex = 0; }

                    txtDONVICAP.Text = hn.DONVICAPHN != null ? hn.DONVICAPHN : "";
                    txtMASOHN.Text = hn.MAHN != null ? hn.MAHN : "";
                    txtNGAPCAPHN.Text = hn.NGAYCAPHN != null ? String.Format("{0:dd/MM/yyyy}", hn.NGAYCAPHN.Value) : "";
                    txtNGAYKTHN.Text = hn.NGAYKETTHUCHN != null ? String.Format("{0:dd/MM/yyyy}", hn.NGAYKETTHUCHN.Value) : "";
                    txtNGAYKYSOHN.Text = hn.NGAYKYHN != null ? String.Format("{0:dd/MM/yyyy}", hn.NGAYKYHN.Value) : "";
                    txtDIACHIHN.Text = hn.DIACHINGHEO != null ? hn.DIACHINGHEO : "";
                    //txtKYHOTROHN.Enabled = true;
                    txtDIACHIHN.Enabled = true;
                    ddlTENXA.Enabled = true;
                    txtDONVICAP.Enabled = true;
                    txtMASOHN.Enabled = true;
                    txtNGAPCAPHN.Enabled = true;
                    txtNGAYKTHN.Enabled = true;
                    txtNGAYKYSOHN.Enabled = true;
                    ImageButton1.Visible = true;
                    ImageButton2.Visible = true;
                    ImageButton3.Visible = true;
                }
                else
                {
                    txtKYHOTROHN.Text = "";
                    txtNAMHOTRO.Text = "";
                    ddlTENXA.SelectedIndex = 0;
                    txtDONVICAP.Text = "";
                    txtMASOHN.Text = "";
                    txtNGAPCAPHN.Text = "";
                    txtNGAYKTHN.Text = "";
                    txtNGAYKYSOHN.Text = "";
                    txtDIACHIHN.Text = "";

                    //txtKYHOTROHN.Enabled = false;
                    txtDIACHIHN.Enabled = false;
                    ddlTENXA.Enabled = false;
                    txtDONVICAP.Enabled = false;
                    txtMASOHN.Enabled = false;
                    txtNGAPCAPHN.Enabled = false;
                    txtNGAYKTHN.Enabled = false;
                    txtNGAYKYSOHN.Enabled = false;
                    ImageButton1.Visible = false;
                    ImageButton2.Visible = false;
                    ImageButton3.Visible = false;
                    //txtKYHOTROHN.Enabled = false;
                    //txtNAMHOTRO.Enabled = false;
                }

                upnlInfor.Update();
            }
            catch { }
        }

        private void BindNgheoGiaHan(string mangheo)
        {
            try
            {
                var hn = _hnnDao.Get(mangheo);
                lbMANGHEO.Text = mangheo;//mangheo
                var kh = _khDao.Get(hn.IDKH);
                txtMADDK.Text = kh.IDKH.ToString();//idkh
                lbTENKHCU.Text = kh.TENKH.ToString();
                var mdsd = _mdsdDao.Get(kh.MAMDSD);
                lbTENMDSD.Text = mdsd.TENMDSD.ToString();
                lbDANHSO.Text = (kh.MADP + kh.MADB).ToString();
                lbMAMDSD.Text = kh.MAMDSD.ToString();//muc dich
                lbMANV.Text = LoginInfo.MANV.ToString();
                if (kh.MADDK != null)
                {
                    lbMADDK.Text = kh.MADDK.ToString();
                }
                else { lbMADDK.Text = ""; }

                //so ho ngheo
                var isCheckedHN = hn.ISHONGHEO.HasValue && hn.ISHONGHEO.Value;
                ckISHONGHEO.Checked = isCheckedHN;
                if (isCheckedHN)
                {
                    lbGIAHANHN.Visible = true;
                    lbGIAHANHN.Text = "Gia hạn";

                    txtKYHOTROHN.Text = DateTime.Now.Month.ToString();
                    txtNAMHOTRO.Text = DateTime.Now.Year.ToString();                    

                    if (hn.DONVICAPHN != null)
                    {
                        var pn = ddlTENXA.Items.FindByValue(hn.MAXA);
                        if (pn != null)
                            ddlTENXA.SelectedIndex = ddlTENXA.Items.IndexOf(pn);
                    }
                    else { ddlTENXA.SelectedIndex = 0; }

                    txtDONVICAP.Text = hn.DONVICAPHN != null ? hn.DONVICAPHN : "";
                    txtMASOHN.Text = "";
                    txtNGAPCAPHN.Text = "";
                    txtNGAYKTHN.Text = "";
                    txtNGAYKYSOHN.Text = "";
                    txtDIACHIHN.Text = hn.DIACHINGHEO != null ? hn.DIACHINGHEO : "";
                    //txtKYHOTROHN.Enabled = true;
                    txtDIACHIHN.Enabled = true;
                    ddlTENXA.Enabled = true;
                    txtDONVICAP.Enabled = true;
                    txtMASOHN.Enabled = true;
                    txtNGAPCAPHN.Enabled = true;
                    txtNGAYKTHN.Enabled = true;
                    txtNGAYKYSOHN.Enabled = true;
                    ImageButton1.Visible = true;
                    ImageButton2.Visible = true;
                    ImageButton3.Visible = true;
                }
                else
                {
                    txtKYHOTROHN.Text = "";
                    txtNAMHOTRO.Text = "";
                    ddlTENXA.SelectedIndex = 0;
                    txtDONVICAP.Text = "";
                    txtMASOHN.Text = "";
                    txtNGAPCAPHN.Text = "";
                    txtNGAYKTHN.Text = "";
                    txtNGAYKYSOHN.Text = "";
                    txtDIACHIHN.Text = "";

                    //txtKYHOTROHN.Enabled = false;
                    txtDIACHIHN.Enabled = false;
                    ddlTENXA.Enabled = false;
                    txtDONVICAP.Enabled = false;
                    txtMASOHN.Enabled = false;
                    txtNGAPCAPHN.Enabled = false;
                    txtNGAYKTHN.Enabled = false;
                    txtNGAYKYSOHN.Enabled = false;
                    ImageButton1.Visible = false;
                    ImageButton2.Visible = false;
                    ImageButton3.Visible = false;
                    //txtKYHOTROHN.Enabled = false;
                    //txtNAMHOTRO.Enabled = false;
                }

                upnlInfor.Update();
            }
            catch { }
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = _nvDao.Get(loginInfo.Username).MAKV;

                if (Filtered == FilteredMode.None)
                {
                    var objList = _hnnDao.GetListKV(ddlKHUVUCMOI.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    /*var objList = _hnnDao.GetList();
                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();*/
                }
            }
            catch { }
        }        

        protected void ckISHONGHEO_CheckedChanged(object sender, EventArgs e)
        {
            if (ckISHONGHEO.Checked)
            {
                ddlTENXA.Enabled = true;                
                txtDONVICAP.Enabled = true;
                txtMASOHN.Enabled = true;
                txtNGAPCAPHN.Enabled = true;
                txtNGAYKTHN.Enabled = true;
                txtNGAYKYSOHN.Enabled = true;
                txtDIACHIHN.Enabled = true;
                ImageButton1.Visible = true;
                ImageButton2.Visible = true;
                ImageButton3.Visible = true;

                //txtKYHOTROHN.Enabled = true;
                //txtNAMHOTRO.Enabled = true;
                //txtKYHOTROHN.Text = DateTime.Now.Month.ToString();
                //txtNAMHOTRO.Text = DateTime.Now.Year.ToString();
            }
            else
            {
                ddlTENXA.Enabled = false;                
                txtDONVICAP.Enabled = false;
                txtMASOHN.Enabled = false;
                txtNGAPCAPHN.Enabled = false;
                txtNGAYKTHN.Enabled = false;
                txtNGAYKYSOHN.Enabled = false;
                txtDIACHIHN.Enabled = false;
                ImageButton1.Visible = false;
                ImageButton2.Visible = false;
                ImageButton3.Visible = false;

                //txtKYHOTROHN.Enabled = false;
                //txtNAMHOTRO.Enabled = false;
                //txtKYHOTROHN.Text = "";
                //txtNAMHOTRO.Text = "";
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

                //bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, query.MAKV.ToString());
                var info = ObjHN;
                var khn = _khDao.Get(info.IDKH);
                if (info == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                bool dung = _gcsDao.IsLockTinhCuocKy1(kynay1, query.MAKV.ToString(), _khDao.Get(info.IDKH).MADP);

                if (dung == true)
                {
                    CloseWaitingDialog();
                    ShowInfor("Đã khoá sổ ghi chỉ số.");
                    return;
                }

                //var info = ObjHN;
                //var khn = _khDao.Get(info.IDKH);
                //if (info == null)
                //{
                //    CloseWaitingDialog();
                //    return;
                //}

                int mnam = DateTime.Now.Year;
                int mthang = DateTime.Now.Month;

                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.KH_HoNgheoN, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    if (_hnnDao.Get(txtMADDK.Text.Trim()) != null)
                    {
                        ShowError("Khách hàng đã nghèo. Xin chọn khách hàng khác.");
                        CloseWaitingDialog();
                        return;
                    }

                    string mangheom = _hnnDao.NewId(); 
                    info.MANGHEO = mangheom;
                    info.MANVN = b;
                    info.MAKV = khn.MAKV.ToString();
                    var tenxa = _xpDao.Get(ddlTENXA.SelectedValue, query.MAKV);
                    txtDONVICAP.Text = tenxa.TENXA.ToString();

                    _hnnDao.Insert(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.UpKhachHangHoNgheo(txtMADDK.Text.Trim(), mangheom, 1); // up ho ngheoo kh

                    msg = null;
                }
                else // update
                {
                    /*int thang2 = Int16.Parse(txtKYHOTROHN.Text.Trim());
                    int nam2 = Int16.Parse(txtNAMHOTRO.Text.Trim());
                    var kynay2 = new DateTime(nam2, thang2, 1);
                    bool khoaso = _gcsDao.IsLockTinhCuocKy(kynay2, query.MAKV.ToString());
                    if (khoaso == true)
                    {
                        CloseWaitingDialog();
                        ShowInfor("Đã khoá sổ ghi chỉ số.");
                        return;
                    }*/

                    if (!HasPermission(Functions.KH_HoNgheoN, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    if (!string.IsNullOrEmpty(lbGIAHANHN.Text.Trim()) || lbGIAHANHN.Text != "")
                    {
                        _rpClass.UpHoNgheoHis(info.MANGHEO, "", "UPHNGIAHHIS");

                        msg = _hnnDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, lbMANGHEO.Text.Trim(), txtMADDK.Text.Trim());
                        
                    }
                    else
                    {
                        //lbIDTDH; txtMADDK;
                        _rpClass.UpHoNgheoHis(info.MANGHEO, "", "UPHONGHEOHIS");

                        msg = _hnnDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, lbMANGHEO.Text.Trim(), txtMADDK.Text.Trim());
                        //msg = _hnnDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddress(), LoginInfo.MANV, lbMANGHEO.Text.Trim(), txtMADDK.Text.Trim());                   
                    }
                }

                CloseWaitingDialog();

                Clear();
                BindDataForGrid();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void ddlTENXA_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.GetKV(b);

                var tenxa = _xpDao.Get(ddlTENXA.SelectedValue, query.MAKV);
                txtDONVICAP.Text = tenxa.TENXA.ToString(); ;
            }
            catch { }
        }        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                int nam = int.Parse(txtNAMHOTRO.Text.Trim());
                int thang = int.Parse(txtKYHOTROHN.Text.Trim());                

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
               
                //var list = _hnnDao.GetListKy(nam, thang);
                var list = _hnnDao.GetListKyKV(nam, thang, _nvDao.Get(b).MAKV);
                gvList.DataSource = list;
                gvList.PagerInforText = list.Count.ToString();
                gvList.DataBind();

                CloseWaitingDialog();
                upnlGrid.Update();

                divCR.Visible = false;
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindDataForGrid();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            try
            {
                BaoCaoHN();
                Clear();
                CloseWaitingDialog();  
            }
            catch { }
        }

        private void BaoCaoHN()
        {
            try
            {
                ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
                if (rp != null)
                {
                    try
                    {
                        rp.Close();
                        rp.Dispose();
                        GC.Collect();
                    }
                    catch  {  }
                }

                /*var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;*/

                DataTable dt = new ReportClass().DSHONGHEON(int.Parse(txtTUTHANG.Text.Trim()), int.Parse(txtTUNAM.Text.Trim()),
                    int.Parse(txtDENTHANG.Text.Trim()), int.Parse(txtDENNAM.Text.Trim()),"O","O","O").Tables[0];               

                rp = new ReportDocument();
                //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
                var path = Server.MapPath("../../Reports/QuanLyKhachHang/DSHoNgheo.rpt");

                rp.Load(path);

                rp.SetDataSource(dt);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();

                
                TextObject txtXN = rp.ReportDefinition.ReportObjects["txtXN"] as TextObject;
                txtXN.Text = "XN ĐIỆN NƯỚC " + ddlKHUVUCMOI.SelectedItem.ToString().ToUpper(); 
                //txtTuNgay
                TextObject txtTuNgay = rp.ReportDefinition.ReportObjects["txtTuNgay"] as TextObject;
                txtTuNgay.Text = "Từ kỳ " + txtTUTHANG.Text.Trim() + "/" + txtTUNAM.Text.Trim() + " đến " + txtDENTHANG.Text.Trim() + "/" + txtDENNAM.Text.Trim();
                var d = DateTime.Now.Day;
                var m = DateTime.Now.Month;
                var y = DateTime.Now.Year;
                TextObject ngay = rp.ReportDefinition.ReportObjects["txtNGAY"] as TextObject;
                ngay.Text = ddlKHUVUCMOI.SelectedItem + ", ngày " + d + " tháng " + m + " năm " + y;
                

                divCR.Visible = true;
                upnlCrystalReport.Update();

                reloadm.Text = "1";

                Session["DS_DonDangKy"] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                CloseWaitingDialog();  
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void ddlKHUVUCMOI_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btTIMHONGHEON_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtTIMHONGHEON.Text.Trim()) || txtTIMHONGHEON.Text.Trim() == "")
                {
                    divCR.Visible = false;
                    upnlCrystalReport.Update();

                    CloseWaitingDialog();
                    return;
                }

                var objList = _hnnDao.TimHNN(txtTIMHONGHEON.Text.Trim(), ddlKHUVUCMOI.SelectedValue);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

                CloseWaitingDialog();
                upnlGrid.Update();

                divCR.Visible = false;
                upnlCrystalReport.Update();
            }
            catch { }
        }

        protected void BindGiaHanHNN(string mangheo)
        {
            var objList = _rpClass.UpHoNgheoHis(mangheo, "", "DSGIAHANHNN");

            gvLSGIAHANHNN.DataSource = objList;
            //gvList.PagerInforText = objList.Count.ToString();
            upnLSGIAHANHNN.DataBind();

            CloseWaitingDialog();
            upnLSGIAHANHNN.Update();    
        }

        protected void gvLSGIAHANHNN_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvLSGIAHANHNN.PageIndex = e.NewPageIndex;
                // Bind data for grid
                //BindGiaHanHNN();
                upnLSGIAHANHNN.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void txtMASOHN_TextChanged(object sender, EventArgs e)
        {

        }

        protected void lkLSGIAHANN_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(lbMANGHEO.Text.Trim()) || lbMANGHEO.Text.Trim() == "")
                {
                    ShowInfor("Chọn hộ nghèo.");
                    CloseWaitingDialog();
                    HideDialog("divGIAHANHNN");
                    return;
                }
                else
                {
                    BindGiaHanHNN(lbMANGHEO.Text.Trim());
                    upnLSGIAHANHNN.Update();
                    UnblockDialog("divGIAHANHNN");
                }
            }
            catch { }
        }
        

    }
}