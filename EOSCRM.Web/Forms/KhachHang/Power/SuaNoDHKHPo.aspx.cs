using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Globalization;

namespace EOSCRM.Web.Forms.KhachHang.Power
{
    public partial class SuaNoDHKHPo : Authentication
    {
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly KHSoNoDHPoDao _khsndhpoDao = new KHSoNoDHPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly MucDichSuDungPoDao _mdsdpoDao = new MucDichSuDungPoDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();

        int thanght = DateTime.Now.Month;
        string namht = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

        #region KH KHSONODHPO
        private KHSONODHPO ObjKHSONO
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var sono = (string.IsNullOrEmpty(lbIDSONO.Text.Trim()) || lbIDSONO.Text == "") ? new KHSONODHPO() : _khsndhpoDao.Get(Convert.ToInt16(lbIDSONO.Text.Trim()));
                if (sono == null)
                    return null;

                sono.MADHPOCU = lbMADHCU.Text.Trim();
                sono.SONOCU = lbSONOCU.Text.Trim();
                sono.MADHPOMOI = lbMADHMOI.Text.Trim();
                sono.SONOMOI = lbSONOMOI.Text.Trim();
                sono.GHICHU = txtGHICHU.Text.Trim();
                sono.NGAYN = DateTime.Now;

                //nam thang sono.IDKH = txtMADDK.Text.Trim();  sono.MADP = ""  sono.MADB = ""  sono.MAKV = ""   sono.TENKH = ""  sono.NGAY = "";      sono.MANVN = ;
                //if (!txtNGAPCAPHN.Text.Trim().Equals(String.Empty))
                //     hn.NGAYCAPHN = DateTimeUtil.GetVietNamDate(txtNGAPCAPHN.Text.Trim()); else   hn.NGAYCAPHN = null;                

                return sono;
            }
        }
        #endregion

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
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_SuaNoDHKHPo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                }

                //if (reloadm.Text == "1")
                //{
                //    BaoCaoHN();
                //    btnKHACHHANG.Visible = false;
                //    btnSave.Visible = false;
                //}
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_SUANODHKHPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_SUANODHKHPO;
            }
            CommonFunc.SetPropertiesForGrid(gvDanhSach);
            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDongHoSoNo);
        }

        private void LoadStaticReferences()
        {
            try
            {
                //Filtered = FilteredMode.None;   

                ddlTHANG1.SelectedIndex = DateTime.Now.Month - 1;
                txtNAM1.Text = DateTime.Now.Year.ToString();

                timkv();

                reloadm.Text = "0";

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

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = _kvpoDao.GetPo(a.MAKV).MAKVPO;

                if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUCMOI.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUCMOI.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                        ddlKHUVUCMOI.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
            }
        }

        protected void btnKHACHHANG_Click(object sender, EventArgs e)
        {
            UnblockDialog("divKhachHang");
            upnlKhachHang.Update();
        }

        protected void btnFilterKH_Click(object sender, EventArgs e)
        {
            BindKhachHang();
            upnlKhachHang.Update();
            CloseWaitingDialog();
        }

        private void BindKhachHang()
        {
            var danhsach = _khpoDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(), txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
                                                 txtSONHA.Text.Trim(), txtTENDP.Text.Trim(), ddlKHUVUC.SelectedValue.Trim());
            gvDanhSach.DataSource = danhsach;
            gvDanhSach.PagerInforText = danhsach.Count.ToString();
            cpeFilter.Collapsed = true;
            gvDanhSach.DataBind();
            tdDanhSach.Visible = true;
        }

        #region gv DS khach hang
        protected void gvDanhSach_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();
                switch (e.CommandName)
                {
                    case "SelectSODB":
                        var khachhang = _khpoDao.Get(id);

                        if (khachhang != null)
                        {
                            HideDialog("divKhachHang");
                            CloseWaitingDialog();
                            BindStatus(khachhang);

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
        #endregion

        private void BindStatus(KHACHHANGPO kh)
        {
            var mdsd = _mdsdpoDao.Get(kh.MAMDSDPO);

            txtMADDK.Text = kh.IDKHPO.ToString();//idkh
            lbTENKHCU.Text = kh.TENKH.ToString();

            lbTENMDSD.Text = mdsd.TENMDSD.ToString();
            lbDANHSO.Text = (kh.MADPPO + kh.MADBPO).ToString();
            lbMAMDSD.Text = kh.MAMDSDPO.ToString();//muc dich
            lbMANV.Text = LoginInfo.MANV.ToString();
            if (kh.MADDKPO != null)
            {
                lbMADDK.Text = kh.MADDKPO.ToString();
            }
            else { lbMADDK.Text = ""; }

            var dh = _dhpoDao.Get(kh.MADHPO);
            lbMADHCU.Text = dh.MADHPO;
            lbSONOCU.Text = dh.SONO;

            upnlInfor.Update();
        }

        private void BindDataForGrid()
        {
            try
            {
                if (Filtered == FilteredMode.None)
                {
                    var objList = _khsndhpoDao.GetList();

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var objList = _khsndhpoDao.GetListKyKV(Convert.ToInt16(txtNAM1.Text.Trim()), Convert.ToInt16(ddlTHANG1.SelectedValue), ddlKHUVUCMOI.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
            }
            catch { }
        }

        #region gv list so no kh
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

            var lkIDSONO = e.Row.FindControl("lkIDSONO") as LinkButton;
            if (lkIDSONO == null) return;
            lkIDSONO.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lkIDSONO) + "')");
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

                        var suano = _khsndhpoDao.Get(Convert.ToInt32(id));

                        BingKHSuaDH(suano);

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
        #endregion

        private void BingKHSuaDH(KHSONODHPO khsn)
        {
            var mdsd = _mdsdpoDao.Get(_khpoDao.Get(khsn.IDKHPO).MAMDSDPO);

            lbIDSONO.Text = khsn.IDSONO.ToString();

            txtMADDK.Text = khsn.IDKHPO.ToString();//idkh
            lbTENKHCU.Text = khsn.TENKH.ToString();
            lbTENMDSD.Text = mdsd.TENMDSD.ToString();
            lbDANHSO.Text = (khsn.MADPPO + khsn.MADBPO).ToString();
            lbMAMDSD.Text = mdsd.MAMDSDPO;//muc dich

            lbMADHCU.Text = khsn.MADHPOCU;
            lbMADHMOI.Text = khsn.MADHPOMOI;
            lbSONOCU.Text = khsn.SONOCU;
            lbSONOMOI.Text = khsn.SONOMOI;

            upnlInfor.Update();
        }

        private void BindDongHoSoNo()
        {
            var list = _dhpoDao.GetListDASD(txtKeywordDHSONO.Text.Trim());
            gvDongHoSoNo.DataSource = list;
            gvDongHoSoNo.PagerInforText = list.Count.ToString();
            gvDongHoSoNo.DataBind();
        }

        #region gv dong ho
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
                            lbMADHMOI.Text = dh.MADHPO.ToString();
                            lbSONOMOI.Text = dh.SONO.ToString();

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
                gvDongHoSoNo.PageIndex = e.NewPageIndex;
                BindDongHoSoNo();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

        protected void btnFilterDHSONO_Click(object sender, EventArgs e)
        {
            BindDongHoSoNo();
            CloseWaitingDialog();
        }

        protected void btSONOMOI_Click(object sender, EventArgs e)
        {
            //BindDongHoSoNo();
            upnlDongHoSoNo.Update();
            UnblockDialog("divDongHoSoNo");
        }

        private void Clear()
        {
            try
            {
                UpdateMode = Mode.Create;

                lbIDSONO.Text = ""; //id khach hang so no
                txtMADDK.Text = "";//idkh
                lbTENKHCU.Text = "";
                lbTENMDSD.Text = "";
                lbDANHSO.Text = "";
                lbMAMDSD.Text = "";//muc dich su dung

                lbMADHCU.Text = "";
                lbMADHMOI.Text = "";
                lbSONOCU.Text = "";
                lbSONOMOI.Text = "";
            }
            catch { }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var kynay1 = new DateTime(int.Parse(namht), thanght, 1);
                bool dung = _gcspoDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                Message msg;

                int namF = Convert.ToInt16(txtNAM1.Text.Trim());
                int thangF = Convert.ToInt16(ddlTHANG1.SelectedValue);                

                if (string.IsNullOrEmpty(lbMADHMOI.Text.Trim()) && lbMADHMOI.Text.Trim() == "")
                {
                    ShowError("Chọn số No mới cho đồng đồ. Kiểm tra lại");
                    CloseWaitingDialog();
                    return;
                }

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.KH_SuaNoDHKHPo, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    //test trung kh trong ky
                    var khsn3 = _khsndhpoDao.GetKyIDKV(namF, thangF, txtMADDK.Text.Trim(), ddlKHUVUCMOI.SelectedValue);
                    if (khsn3 != null)
                    {
                        ShowError("Khách hàng này đã thay đổi số No trong kỳ rồi. Kiểm tra lại");
                        CloseWaitingDialog();
                        return;
                    }

                    if (dung == true)
                    {
                        var kysau = kynay1.AddMonths(1);

                        var sonokysau = ObjKHSONO;
                        var kh = _khpoDao.Get(txtMADDK.Text.Trim());

                        sonokysau.NAM = kysau.Year;
                        sonokysau.THANG = kysau.Month;
                        sonokysau.IDKHPO = txtMADDK.Text.Trim();
                        sonokysau.MADPPO = kh.MADPPO;
                        sonokysau.MADBPO = kh.MADBPO;
                        sonokysau.MAKVPO = kh.MAKVPO;
                        sonokysau.TENKH = kh.TENKH;
                        sonokysau.NGAY = DateTime.Now;
                        sonokysau.MANVN = b;

                        msg = _khsndhpoDao.Insert(sonokysau, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                        Message msdhcu, msddhmoi;
                        var dhcu = _dhpoDao.Get(lbMADHCU.Text.Trim());
                        var dhmoi = _dhpoDao.Get(lbMADHMOI.Text.Trim());
                        msdhcu = _dhpoDao.UpdateKoSD(dhcu); //update dh cu ko su dung
                        msddhmoi = _dhpoDao.UpdateDASD(dhmoi); //update dh moi da su dung

                        ShowInfor("Kỳ này đã khóa sổ, tự động chuyển qua kỳ sau.");
                        CloseWaitingDialog();
                    }
                    else
                    {
                        var sono = ObjKHSONO;
                        var kh = _khpoDao.Get(txtMADDK.Text.Trim());

                        sono.NAM = namF;
                        sono.THANG = thangF;
                        sono.IDKHPO = txtMADDK.Text.Trim();
                        sono.MADPPO = kh.MADPPO;
                        sono.MADBPO = kh.MADBPO;
                        sono.MAKVPO = kh.MAKVPO;
                        sono.TENKH = kh.TENKH;
                        sono.NGAY = DateTime.Now;
                        sono.MANVN = b;

                        msg = _khsndhpoDao.Insert(sono, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                        Message msdhcu, msddhmoi;
                        var dhcu = _dhpoDao.Get(lbMADHCU.Text.Trim());
                        var dhmoi = _dhpoDao.Get(lbMADHMOI.Text.Trim());
                        msdhcu = _dhpoDao.UpdateKoSD(dhcu); //update dh cu ko su dung
                        msddhmoi = _dhpoDao.UpdateDASD(dhmoi); //update dh moi da su dung
                    }
                }
                else
                {
                    if (!HasPermission(Functions.KH_SuaNoDHKHPo, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    var sono = ObjKHSONO;

                    Message msddhmoi;
                    var dhmoi = _dhpoDao.Get(_khsndhpoDao.Get(sono.IDSONO).MADHPOMOI); //lbIDSONO.Text.Trim()
                    msddhmoi = _dhpoDao.UpdateKoSD(dhmoi); //update dh cu ko su dung

                    sono.MANVN = b;

                    msg = _khsndhpoDao.Update(sono, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                    Message msddhmoi2;
                    var dhmoi2 = _dhpoDao.Get(lbMADHMOI.Text.Trim());
                    msddhmoi2 = _dhpoDao.UpdateDASD(dhmoi2); //update dh da su dung
                }

                Clear();
                upnlInfor.Update();

                BindDataForGrid();
                upnlGrid.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Clear();
            CloseWaitingDialog();
            upnlInfor.Update();
        }

        protected void btLOC_Click(object sender, EventArgs e)
        {
            try
            {
                BindDataForGrid();
                CloseWaitingDialog();
                upnlGrid.Update();
            }
            catch { }
        }

    }
}