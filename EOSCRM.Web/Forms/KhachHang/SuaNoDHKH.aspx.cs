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

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class SuaNoDHKH : Authentication
    {
        private readonly DongHoDao _dhDao = new DongHoDao();
        private readonly KHSoNoDHDao _khsndhDao = new KHSoNoDHDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly MucDichSuDungDao _mdsdDao = new MucDichSuDungDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();

        int thanght = DateTime.Now.Month;
        string namht = DateTime.Now.Year.ToString(CultureInfo.InvariantCulture);

        #region KH KHSONODH
        private KHSONODH ObjKHSONO
        {
            get
            {
                //if (!ValidateData())
                //    return null;

                var sono = (string.IsNullOrEmpty(lbIDSONO.Text.Trim()) || lbIDSONO.Text == "") ? new KHSONODH() : _khsndhDao.Get(Convert.ToInt16(lbIDSONO.Text.Trim()));                                              
                if (sono == null)
                    return null;
               
                sono.MADHCU = lbMADHCU.Text.Trim();
                sono.SONOCU = lbSONOCU.Text.Trim();
                sono.MADHMOI = lbMADHMOI.Text.Trim();
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

        private void OpenWaitingDialog()
        {
            ((EOS)Page.Master).OpenWaitingDialog();
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_SuaNoDHKH, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_KH_SUANODHKH;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_SUANODHKH;
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
            var danhsach = _khDao.SearchKhachHang(txtIDKH.Text.Trim(), txtTENKH.Text.Trim(), txtMADH.Text.Trim(), txtSOHD.Text.Trim(),
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
                        var khachhang = _khDao.Get(id);
                       
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

            var dh = _dhDao.Get(kh.MADH);
            lbMADHCU.Text = dh.MADH;
            lbSONOCU.Text = dh.SONO;

            upnlInfor.Update();
        }

        private void BindDataForGrid()
        {
            try
            {
                if (Filtered == FilteredMode.None)
                {
                    var objList = _khsndhDao.GetList();

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var objList = _khsndhDao.GetListKyKV(Convert.ToInt16(txtNAM1.Text.Trim()), Convert.ToInt16(ddlTHANG1.SelectedValue), ddlKHUVUCMOI.SelectedValue);

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

                        var suano = _khsndhDao.Get(Convert.ToInt32(id));

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

        private void BingKHSuaDH(KHSONODH khsn)
        {
            var mdsd = _mdsdDao.Get(_khDao.Get(khsn.IDKH).MAMDSD);

            lbIDSONO.Text = khsn.IDSONO.ToString();

            txtMADDK.Text = khsn.IDKH.ToString();//idkh
            lbTENKHCU.Text = khsn.TENKH.ToString();
            lbTENMDSD.Text = mdsd.TENMDSD.ToString();
            lbDANHSO.Text = (khsn.MADP + khsn.MADB).ToString();
            lbMAMDSD.Text = mdsd.MAMDSD;//muc dich

            lbMADHCU.Text = khsn.MADHCU;
            lbMADHMOI.Text = khsn.MADHMOI;
            lbSONOCU.Text = khsn.SONOCU;
            lbSONOMOI.Text = khsn.SONOMOI;

            upnlInfor.Update();
        }

        private void BindDongHoSoNo()
        {
            var list = _dhDao.GetListDASD(txtKeywordDHSONO.Text.Trim());
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
                        var dh = _dhDao.Get(id);
                        if (dh != null)
                        {
                            lbMADHMOI.Text = dh.MADH.ToString();                            
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
                bool dung = _gcsDao.IsLockTinhCuocKy(kynay1, ddlKHUVUC.SelectedValue);

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
                    if (!HasPermission(Functions.KH_SuaNoDHKH, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    //test trung kh trong ky
                    var khsn3 = _khsndhDao.GetKyIDKV(namF, thangF, txtMADDK.Text.Trim(), ddlKHUVUCMOI.SelectedValue);
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
                        var kh = _khDao.Get(txtMADDK.Text.Trim());

                        sonokysau.NAM = kysau.Year;
                        sonokysau.THANG = kysau.Month;
                        sonokysau.IDKH = txtMADDK.Text.Trim();
                        sonokysau.MADP = kh.MADP;
                        sonokysau.MADB = kh.MADB;
                        sonokysau.MAKV = kh.MAKV;
                        sonokysau.TENKH = kh.TENKH;
                        sonokysau.NGAY = DateTime.Now;
                        sonokysau.MANVN = b;

                        msg = _khsndhDao.Insert(sonokysau, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                        Message msdhcu, msddhmoi;
                        var dhcu = _dhDao.Get(lbMADHCU.Text.Trim());
                        var dhmoi = _dhDao.Get(lbMADHMOI.Text.Trim());
                        msdhcu = _dhDao.UpdateKoSD(dhcu); //update dh cu ko su dung
                        msddhmoi = _dhDao.UpdateDASD(dhmoi); //update dh moi da su dung

                        ShowInfor("Kỳ này đã khóa sổ, tự động chuyển qua kỳ sau.");
                        CloseWaitingDialog();
                    }
                    else
                    {
                        var sono = ObjKHSONO;
                        var kh = _khDao.Get(txtMADDK.Text.Trim());

                        sono.NAM = namF;
                        sono.THANG = thangF;
                        sono.IDKH = txtMADDK.Text.Trim();
                        sono.MADP = kh.MADP;
                        sono.MADB = kh.MADB;
                        sono.MAKV = kh.MAKV;
                        sono.TENKH = kh.TENKH;
                        sono.NGAY = DateTime.Now;
                        sono.MANVN = b;

                        msg = _khsndhDao.Insert(sono, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                        Message msdhcu, msddhmoi;
                        var dhcu = _dhDao.Get(lbMADHCU.Text.Trim());
                        var dhmoi = _dhDao.Get(lbMADHMOI.Text.Trim());
                        msdhcu = _dhDao.UpdateKoSD(dhcu); //update dh cu ko su dung
                        msddhmoi = _dhDao.UpdateDASD(dhmoi); //update dh moi da su dung
                    }
                }
                else
                {
                    if (!HasPermission(Functions.KH_SuaNoDHKH, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }                    
                    
                    var sono = ObjKHSONO;

                    Message msddhmoi;
                    var dhmoi = _dhDao.Get(_khsndhDao.Get(sono.IDSONO).MADHMOI); //lbIDSONO.Text.Trim()
                    msddhmoi = _dhDao.UpdateKoSD(dhmoi); //update dh cu ko su dung

                    sono.MANVN = b;

                    msg = _khsndhDao.Update(sono, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), b);

                    Message msddhmoi2;
                    var dhmoi2 = _dhDao.Get(lbMADHMOI.Text.Trim());
                    msddhmoi2 = _dhDao.UpdateDASD(dhmoi2); //update dh da su dung
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