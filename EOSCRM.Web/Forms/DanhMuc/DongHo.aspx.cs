using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html.simpleparser;
using System.IO;
using System.Web.UI;
using System.Web;
using System.Data;
using System.Data.SqlClient;

using System.Configuration;
using System.Collections;
using System.Web.Security;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class DongHo : Authentication
    {
        private readonly DonDangKyDao _ddkDao = new DonDangKyDao();
        private KhachHangDao _khDao = new KhachHangDao();
        private ThiCongDao _tcDao = new ThiCongDao();
        private readonly DongHoDao _objDao = new DongHoDao();
        private readonly LoaiDongHoDao _ldhDao = new LoaiDongHoDao();
        private readonly KyDuyetDao _lvkdDao = new KyDuyetDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        private DONGHO ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;
                var nv = new DONGHO
                {
                    MADH = txtMADH.Text.Trim(),
                    MALDH = ddlMALDH.SelectedValue,
                    TRANGTHAI = txtTRANGTHAI.Text.Trim(),
                    SONO = txtSONO.Text.Trim(),
                    DASD = false,//mac dinh la chua su dung
                    SOKD = txtSOKD.Text.Trim(),
                    TEMKD = txtTEMKD.Text.Trim(),
                    TENCTKD = txtTENCTKD.Text.Trim(),
                    SXTAI = txtSXTAI.Text.Trim(),
                    MAKV = ddlKHUVUC.SelectedValue,
                    CONGSUAT = txtCONGSUAT.Text.Trim()
                };
                if (!string.IsNullOrEmpty("11/" + txtHANKD.Text.Trim()))
                    nv.HANKD = DateTimeUtil.GetVietNamDate("11/" + txtHANKD.Text);
                if (!string.IsNullOrEmpty(txtNGAYKD.Text.Trim()))
                    nv.NGAYKD = DateTimeUtil.GetVietNamDate(txtNGAYKD.Text);
                if (!string.IsNullOrEmpty(txtNAMSX.Text.Trim()))
                    nv.NAMSX = txtNAMSX.Text.Trim();
                if (!string.IsNullOrEmpty(txtNAMTT.Text.Trim()))
                    nv.NAMTT = txtNAMTT.Text.Trim();
                if (!string.IsNullOrEmpty(txtNGAYNK.Text.Trim()))
                    nv.NGAYNK = DateTimeUtil.GetVietNamDate(txtNGAYNK.Text);
                if (!string.IsNullOrEmpty(txtNGAYXK.Text.Trim()))
                    nv.NGAYXK = DateTimeUtil.GetVietNamDate(txtNGAYXK.Text);
                return nv;
            }
        }

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

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_DongHo, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                    //BindGridXuat();
                    EventEnter();                    
                }
                else if (lbReExcel.Text == "1")
                {
                    if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()) && !string.IsNullOrEmpty(txtToDate.Text.Trim()))
                        InDSDongHo(DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text));
                }
                else if (lbReExcel.Text == "2")
                {                    
                        InDSDongHoDaSD();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void EventEnter()
        {
            txtSOKD.Attributes.Add("onkeypress", "return clickButton(event)");
            ddlMALDH.Attributes.Add("onkeypress", "return clickButtonddlMALDH(event)");
            txtSONO.Attributes.Add("onkeypress", "return clickButtonddltxtSONO(event)");
            txtSXTAI.Attributes.Add("onkeypress", "return clickButtontxtSXTAI(event)");
            txtNAMSX.Attributes.Add("onkeypress", "return clickButtontxtNAMSX(event)");
            txtTEMKD.Attributes.Add("onkeypress", "return clickButtontxtTEMKD(event)");
            txtHANKD.Attributes.Add("onkeypress", "return clickButtontxtHANKD(event)");
            txtNGAYKD.Attributes.Add("onkeypress", "return clickButtontxtNGAYKD(event)");
            txtTENCTKD.Attributes.Add("onkeypress", "return clickButtontxtTENCTKD(event)");           
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_DM_DONGHO;
            var header = (EOSCRM.Web.UserControls.Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_DONGHO;
            }
            CommonFunc.SetPropertiesForGrid(gvList);
            //CommonFunc.SetPropertiesForGrid(gvXuat);
        }        
            
        private void BindDataForGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (Filtered == FilteredMode.None)
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;
                // ReSharper disable EmptyGeneralCatchClause
                try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
                catch { }
                try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
                catch { }

                if (toDate != null)
                {
                    //var objList = _objDao.GetListKV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                    //            txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                    //            fromDate, toDate, txtTRANGTHAI.Text.Trim(), _nvDao.Get(b).MAKV);

                    var objList = _objDao.GetListKV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                                    txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                                    fromDate, toDate.Value.AddDays(1), txtTRANGTHAI.Text.Trim(), _nvDao.Get(b).MAKV);  

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    //var objList = _objDao.GetList2(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, txtSONO.Text.Trim());
                    var objList = _objDao.GetList2KV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, txtSONO.Text.Trim(), _nvDao.Get(b).MAKV);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
            }
            else
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;
               
                try
                {
                    if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                        fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text);
                    if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
                        toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text);

                    if (toDate != null)
                    {
                        var objList = _objDao.GetListKV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                                    txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                                    fromDate, toDate.Value.AddDays(1), txtTRANGTHAI.Text.Trim(), _nvDao.Get(b).MAKV);                       

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();

                    }
                    else
                    {
                        //var objList = _objDao.GetList2(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, txtSONO.Text.Trim());
                        var objList = _objDao.GetList2KV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, txtSONO.Text.Trim(), _nvDao.Get(b).MAKV);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }  
                }
                catch { }
            }
        }

        private void BindDataDongHo()
        {
            try
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;
                if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()))
                    fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text);
                if (!string.IsNullOrEmpty(txtToDate.Text.Trim()))
                    toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text);

                var objList = _objDao.GetList(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                                    txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                                    fromDate, toDate.Value.AddDays(1), txtTRANGTHAI.Text.Trim());

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

            }
            catch { }
        }
        
        private void LoadStaticReferences()
        {
            var ldhList = _ldhDao.GetList();
            ddlMALDH.Items.Clear();
            ddlMALDH.Items.Add(new System.Web.UI.WebControls.ListItem("Tất cả", "%"));
            foreach(var ldh in ldhList)
                ddlMALDH.Items.Add(new System.Web.UI.WebControls.ListItem(ldh.MALDH, ldh.MALDH));
            lbReExcel.Text = "0";
            ClearForm();
            timkv();
        }
        
        public bool ValidateData()
        {
            /*if (string.Empty.Equals(txtMADH.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đồng hồ"), txtMADH.ClientID);
                return false;
            }*/
            var ldh = _ldhDao.Get(ddlMALDH.SelectedValue);
            if(ldh == null)
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Loại đồng hồ"));
                return false;
            }
            if (!string.IsNullOrEmpty(txtNAMSX.Text.Trim()))
            {
                try
                {
                    DateTime.ParseExact(txtNAMSX.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Năm sản xuất"), txtNAMSX.ClientID);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtNAMTT.Text.Trim()))
            {
                try
                {
                    DateTime.ParseExact(txtNAMTT.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Năm TT"), txtNAMTT.ClientID);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtNGAYNK.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYNK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày nhập kho"), txtNGAYNK.ClientID);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtHANKD.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate("11/"+txtHANKD.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Hạn kiểm định"), txtHANKD.ClientID);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtNGAYKD.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYKD.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày kiểm định"), txtNGAYKD.ClientID);
                    return false;
                }
            }
            if (!string.IsNullOrEmpty(txtNGAYXK.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYXK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày xuất kho"), txtNGAYXK.ClientID);
                    return false;
                }
            }
            return true;
        }

        private void ClearForm()
        {
            UpdateMode = Mode.Create;

            txtMADH.Text = "";
            txtMADH.Focus();
            txtMADH.ReadOnly = false;            
            ddlMALDH.SelectedIndex = 0;
            txtNAMSX.Text = "";
            txtNAMTT.Text = "";
            txtNGAYXK.Text = "";
            txtNGAYNK.Text = "";
            chkDASD.Checked = false;
            txtTRANGTHAI.Text = "";
            txtSONO.Text = "";
            txtSOKD.Text = "";
            txtTEMKD.Text = "";
            txtHANKD.Text = "";
            txtNGAYKD.Text = "";
            txtTENCTKD.Text = "";
            lbNGAYNHAPDH.Text = "";
            lbReExcel.Text = "0";
            txtFromDate.Text = "";
            txtToDate.Text = "";
            txtCONGSUAT.Text = "15";

            divCR.Visible = false;
            upnlCrystalReport.Update();

            upnlInfor.Update();
        }

        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    //TODO: check relation before update list
                    var objs = new List<DONGHO>();
                    var listIds = strIds.Split(',');
                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đồng hồ với mã", ma);
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msgIsInUse));
                            return;
                        }
                    }
                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _objDao.Get(ma)));
                    if (objs.Count > 0)
                    {
                        var msg = _objDao.DeleteList(objs);
                        if (msg != null)
                        {
                            CloseWaitingDialog();
                            switch (msg.MsgType)
                            {
                                case MessageType.Error:
                                    ShowError(ResourceLabel.Get(msg));
                                    break;
                                case MessageType.Info:
                                    ShowInfor(ResourceLabel.Get(msg));
                                    break;
                                case MessageType.Warning:
                                    ShowWarning(ResourceLabel.Get(msg));
                                    break;
                            }
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindItem(DONGHO obj)
        {
            if (obj == null)
                return;
            //SetControlValue(txtMADH.ClientID, obj.MADH);
            //SetReadonly(txtMADH.ClientID, true);
            txtMADH.Text = obj.MADH;
            var ldh = ddlMALDH.Items.FindByValue(obj.MALDH);
            if (ldh != null)
                ddlMALDH.SelectedIndex = ddlMALDH.Items.IndexOf(ldh);
            SetControlValue(txtNAMSX.ClientID, obj.NAMSX);
            SetControlValue(txtNAMTT.ClientID, obj.NAMTT);
            SetControlValue(txtNGAYNK.ClientID, obj.NGAYNK.HasValue ? obj.NGAYNK.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtNGAYXK.ClientID, obj.NGAYXK.HasValue ? obj.NGAYXK.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtTRANGTHAI.ClientID, obj.TRANGTHAI);
            SetControlValue(txtSONO.ClientID, obj.SONO);
            chkDASD.Checked = obj.DASD;
            SetControlValue(txtSOKD.ClientID, obj.SOKD);
            SetControlValue(txtTEMKD.ClientID, obj.TEMKD);
            SetControlValue(txtHANKD.ClientID, obj.HANKD.HasValue ? obj.HANKD.Value.ToString("MM/yyyy") : "");
            SetControlValue(txtNGAYKD.ClientID, obj.NGAYKD.HasValue ? obj.NGAYKD.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtTENCTKD.ClientID, obj.TENCTKD);
            SetControlValue(txtSXTAI.ClientID, obj.SXTAI);
            //xem ngay nhap dong ho nuoc
            /*String _dhtt = "DH_A";
            String _dhmota = "Nhập đồng hồ nước.";
            var dhn = _lvkdDao.GetMaDon(obj.MADH, _dhtt, _dhmota);
            if (dhn != null)
            { lbNGAYNHAPDH.Text = dhn.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
            else { lbNGAYNHAPDH.Text = ""; }
            */
            if (obj.NGAYNHAP != null)
            {
                lbNGAYNHAPDH.Text = obj.NGAYNHAP.Value.ToString("dd/MM/yyyy");
            }

            var kv = ddlKHUVUC.Items.FindByValue(obj.MAKV);
            if (kv != null)
            {
                ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
            }

            txtCONGSUAT.Text = obj.CONGSUAT.ToString();

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
                var objDb = _objDao.Get(id);

                switch (e.CommandName)
                {
                    case "EditItem":                        
                        if (objDb != null)
                        {
                            BindItem(objDb);
                            UpdateMode = Mode.Update;
                        }
                        CloseWaitingDialog();
                        break;

                    case "DonKhachHang":
                        if (objDb != null)
                        {
                            if (objDb.DASD == true)
                            {
                                txtMADDKTT.Text = "";
                                txtTENKHTT.Text = "";
                                txtDANHSOTT.Text = "";
                                txtTENKHKTTT.Text = "";

                                var thicong = _tcDao.GetMADH(objDb.MADH);
                                if (thicong != null)
                                {                                    
                                    var dondk = _ddkDao.Get(thicong.MADDK);

                                    txtMADDKTT.Text = dondk.MADDK;
                                    txtTENKHTT.Text = dondk.TENKH;                                                                     
                                }

                                var khachhang = _khDao.GetMADH(objDb.MADH);
                                if (khachhang != null)
                                {
                                    txtDANHSOTT.Text = khachhang.MADP + khachhang.MADB;
                                    txtTENKHKTTT.Text = khachhang.TENKH;
                                }  

                                UnblockDialog("divDonKhachHang");
                                upDonKhachHang.Update();                                
                            }
                            else
                            {
                                HideDialog("divDonKhachHang");
                                ShowInfor("Chọn đồng hồ đã sử dụng.");                                
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            //Filtered = FilteredMode.Filtered;

            BindDataForGrid();
            
            if ( !string.IsNullOrEmpty(txtFromDate.Text.Trim()) && !string.IsNullOrEmpty(txtToDate.Text.Trim()))
                InDSDongHo(DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text));

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        public bool TestData()
        {
            if (!string.IsNullOrEmpty(txtCONGSUAT.Text.Trim()))
            {
                try
                {
                    //DateTime.ParseExact(txtNAMSX.Text.Trim(), "yyyy", CultureInfo.CurrentCulture);
                    Int32.Parse(txtCONGSUAT.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Sai công suất, phải nhập số. Kiểm tra lại !"), txtCONGSUAT.ClientID);
                    return false;
                }
            }
            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var info = ItemObj;
                if (info == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                if (!TestData())
                {                   
                    CloseWaitingDialog();
                    return;
                }               

                Message msg;
                //Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.DM_DongHo, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    if (txtHANKD.Text.Trim().Count() > 7)
                    {
                        CloseWaitingDialog();
                        ShowError("Chỉ nhập tháng, năm kiểm định. VD: 01/2016");
                        return;
                    }

                    var tontai = _objDao.Get(txtMADH.Text.Trim());
                    if (tontai != null)
                    {
                        CloseWaitingDialog();
                        ShowError("Mã đồng hồ đã tồn tại", txtMADH.ClientID);
                        return;
                    }

                    if (_nvDao.Get(b).MAKV == "X")
                    {
                        //txtMADH.Text= _objDao.NewId();
                        info.MADH = "LX" + _objDao.NewIdLX(_nvDao.Get(b).MAKV);
                        //info.MADH = _objDao.NewIdLX();
                        info.MANVNHAP = LoginInfo.MANV.ToString();
                        info.NGAYNHAP = DateTime.Now;
                        //msg = _objDao.Insert(info);
                        _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    }
                    else
                    {
                        if (_nvDao.Get(b).MAKV == "S")
                        {
                            //txtMADH.Text= _objDao.NewId();NewIdCD
                            //info.MADH = "CD" + _objDao.NewIdLX(_nvDao.Get(b).MAKV);
                            info.MADH = "CD" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "CD");
                            //info.MADH = "CD" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "S");
                            //info.MADH = _objDao.NewIdLX();
                            info.MANVNHAP = LoginInfo.MANV.ToString();
                            info.NGAYNHAP = DateTime.Now;
                            //msg = _objDao.Insert(info);
                            _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                        }
                        else
                        {
                            if (_nvDao.Get(b).MAKV == "P")
                            {
                                info.MADH = "PT" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "PT");
                                info.MANVNHAP = LoginInfo.MANV.ToString();
                                info.NGAYNHAP = DateTime.Now;

                                _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                            }
                            else
                            {
                                if (_nvDao.Get(b).MAKV == "T")
                                {
                                    info.MADH = "TC" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "TC");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;

                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                                else if (_nvDao.Get(b).MAKV == "N")
                                {
                                    info.MADH = "CP" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "CP");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;

                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                                else if (_nvDao.Get(b).MAKV == "K")
                                {
                                    info.MADH = "CM" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "CM");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;

                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                                else if (_nvDao.Get(b).MAKV == "L")
                                {
                                    info.MADH = "TT" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "TT");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;

                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                                else if (_nvDao.Get(b).MAKV == "M")
                                {
                                    info.MADH = "TB" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "TB");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;

                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                                else if (_nvDao.Get(b).MAKV == "Q")
                                {
                                    info.MADH = "AP" + _objDao.NewIdCD(_nvDao.Get(b).MAKV, "AP");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;

                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                                else  // khu vuc khac
                                {
                                    var sono = _objDao.GetSoNoKVLoai(txtSONO.Text.Trim(), _nvDao.Get(b).MAKV, ddlMALDH.SelectedValue);
                                    if (sono != null)
                                    {
                                        CloseWaitingDialog();
                                        ShowError("Số No đồng hồ đã tồn tại. Kiểm tra lại.", txtSONO.ClientID);
                                        return;
                                    }

                                    //txtMADH.Text= _objDao.NewId();
                                    //info.MADH = Convert.ToInt64(_objDao.NewIdKVCT(_nvDao.Get(b).MAKV)) <= Convert.ToInt64(_objDao.NewIdKVCT("O")) ?
                                    //    _objDao.NewIdKVCT("O") : _objDao.NewIdKVCT(_nvDao.Get(b).MAKV);
                                    info.MADH = _objDao.NewIdKVCTTS("");
                                    info.MANVNHAP = LoginInfo.MANV.ToString();
                                    info.NGAYNHAP = DateTime.Now;
                                    //msg = _objDao.Insert(info);
                                    _objDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                                }
                            }
                        }
                    }

                    msg = null;

                    //BindDataForGrid();
                    BindDataDongHo();

                    upnlGrid.Update();
                    txtSOKD.Text = "";
                    txtSONO.Text = "";
                    txtTEMKD.Text = "";
                    txtCONGSUAT.Text = "15";
                    //txtSONO.Focus();
                    txtSOKD.Focus();                    
                }
                else // update
                {
                    if (!HasPermission(Functions.DM_DongHo, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    if (txtHANKD.Text.Trim().Count() > 7)
                    {
                        CloseWaitingDialog();
                        ShowError("Chỉ nhập tháng, năm kiểm định. VD: 01/2016");
                        return;
                    }

                    _rpClass.DongHo_His(info.MADH, b, DateTime.Now);

                    //msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV); 
                    
                    var dhsua = _objDao.Get(info.MADH);

                    if (dhsua.DASD != true)
                    {
                        msg = _objDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);                        
                    }
                    else 
                    {
                        if (b == "nguyen")
                        {
                            dhsua.MADH = txtMADH.Text.Trim();
                            dhsua.MALDH = ddlMALDH.SelectedValue;
                            dhsua.TRANGTHAI = txtTRANGTHAI.Text.Trim();
                            dhsua.SONO = txtSONO.Text.Trim();
                            dhsua.DASD = chkDASD.Checked;
                            dhsua.SOKD = txtSOKD.Text.Trim();
                            dhsua.TEMKD = txtTEMKD.Text.Trim();
                            dhsua.TENCTKD = txtTENCTKD.Text.Trim();
                            dhsua.SXTAI = txtSXTAI.Text.Trim();
                            dhsua.MAKV = ddlKHUVUC.SelectedValue;
                            dhsua.CONGSUAT = txtCONGSUAT.Text.Trim();

                            msg = _objDao.Update(dhsua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                        }
                        else
                        {
                            dhsua.MADH = txtMADH.Text.Trim();
                            dhsua.MALDH = ddlMALDH.SelectedValue;
                            dhsua.TRANGTHAI = txtTRANGTHAI.Text.Trim();
                            //dhsua.SONO = txtSONO.Text.Trim();
                            //dhsua.DASD = chkDASD.Checked;
                            dhsua.SOKD = txtSOKD.Text.Trim();
                            dhsua.TEMKD = txtTEMKD.Text.Trim();
                            dhsua.TENCTKD = txtTENCTKD.Text.Trim();
                            dhsua.SXTAI = txtSXTAI.Text.Trim();
                            dhsua.MAKV = ddlKHUVUC.SelectedValue;
                            dhsua.CONGSUAT = txtCONGSUAT.Text.Trim();

                            msg = _objDao.Update(dhsua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                            ShowError("Đồng hồ đã sử dụng không được sửa số No.");
                        }
                    }                     

                    ClearForm();
                }
                CloseWaitingDialog();
                //ClearForm();                
                BindDataForGrid();
                upnlGrid.Update();                
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.DM_DongHo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                DeleteList();
                BindDataForGrid();
                ClearForm();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CloseWaitingDialog();
            //Filtered = FilteredMode.None;
            ClearForm();
        }

        //private void XuatDuLieuRaExcel(GridView MyGridview)        
        private void XuatDuLieuRaWord(GridView MyGridview)
        {
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.doc");
            Response.Charset = "";
            Response.ContentType = "application/vnd.ms-word ";
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            // Bỏ phân trang - Nếu chỉ muỗn Export Trang hiện hành thì chọn =true
            MyGridview.AllowPaging = false;
            MyGridview.DataBind();
            MyGridview.RenderControl(hw);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }

        private void XuatDuLieuGridRaPDF(GridView MyGridview)
        {
            Response.ContentType = "application/pdf";
            Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.pdf");
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            StringWriter sw = new StringWriter();
            HtmlTextWriter hw = new HtmlTextWriter(sw);
            MyGridview.AllowPaging = false;
            MyGridview.DataBind();
            MyGridview.RenderControl(hw);
            StringReader sr = new StringReader(sw.ToString());
            Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
            HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
            PdfWriter.GetInstance(pdfDoc, Response.OutputStream);
            pdfDoc.Open();
            htmlparser.Parse(sr);
            pdfDoc.Close();
            Response.Write(pdfDoc);
            Response.End();
        }

        private void XuatDuLieuRaExcel(GridView MyGridview)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                //var TuNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(cboTHANG.Text.Trim()) + "/" + int.Parse(txtNAM.Text.Trim()));
                //var DenNgay = DateTimeUtil.GetVietNamDate("01/" + int.Parse(ddlDenThang.Text.Trim()) + "/" + int.Parse(txtDenNam.Text.Trim()));


                DateTime fromDate = DateTime.Now;
                DateTime toDate = DateTime.Now;

                try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
                catch { }
                try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
                catch { }
                
                var objList = _rpClass.BienKHPoTuDenNgay(fromDate, toDate, "", "", _nvDao.Get(b).MAKV, "", "", "", "DSDHTHEONGAY");

                DataTable dt;
                dt = objList.Tables[0];
                   

                //Create a dummy GridView
                GridView GridView1 = new GridView();
                GridView1.AllowPaging = false;
                GridView1.DataSource = dt;
                GridView1.DataBind();

                Response.Clear();
                Response.Buffer = true;
                Response.AddHeader("content-disposition", "attachment;filename=DSDH.xls");
                //Response.AddHeader("content-disposition", "attachment;filename=KHM" + cboTHANG.Text.Trim() + txtNAM.Text.Trim().Substring(2, 2) + ".doc");
                Response.Charset = "";
                Response.ContentType = "application/vnd.ms-excel";
                //Response.ContentType = "application/vnd.ms-word ";
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                StringWriter sw = new StringWriter();
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                hw.WriteLine("<meta http-equiv=\"Content-Type\" content=\"text/html; charset=utf-8\">");
                for (int i = 0; i < GridView1.Rows.Count; i++)
                {
                    //Apply text style to each Row
                    GridView1.Rows[i].Attributes.Add("class", "textmode");
                }
                GridView1.RenderControl(hw);

                //style to format numbers to string
                //string style = @"<style> .textmode { mso-number-format:\@; } </style>";
                //Response.Write(style);
                string style = @"<style> TD { mso-number-format:\@; } </style>";
                Response.Write(style);

                Response.Output.Write(sw.ToString());
                Response.Flush();
                Response.End();

                CloseWaitingDialog();
                upnlInfor.Update();
                
            }
            catch  {  } 
        }

        protected void btnXuatExcel_Click(object sender, EventArgs e)
        {
            XuatDuLieuRaExcel(gvXuat);

            CloseWaitingDialog();
            upnlInfor.Update();              
        }

        public override void VerifyRenderingInServerForm(Control control)
	    {
        }

        protected void gvXuat_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvXuat.PageIndex = e.NewPageIndex;
                gvXuat.DataBind();     
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindGridXuat()
        {
            try
            {                
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var tungay =  DateTimeUtil.GetVietNamDate(txtFromDate.Text);
                var denngay = DateTimeUtil.GetVietNamDate(txtToDate.Text);
           
                string TruyVan = @"SELECT MADH,SONO FROM DONGHO WHERE MAKV='" + _nvDao.Get(b).MAKV + "' AND NGAYNHAP >= '" 
                    + tungay + "' AND NGAYNHAP <= DATEADD(DAY,1,'" + denngay + "')";
                SqlCommand cmd = new SqlCommand(TruyVan);
                DataTable dt = _objDao.GetDataToTable(cmd);

                gvXuat.DataSource = dt;
                gvXuat.DataBind();

                //XuatDuLieuRaExcel(gvXuat);
            }
            catch { }
        }

        private void InDSDongHo(DateTime tungay, DateTime denngay)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

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

            DataTable dt = new ReportClass().ListDongHo("N0", tungay, denngay, _nvDao.Get(b).MAKV).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DanhMucHeThong/DSDONGHODATE.rpt");
            rp.Load(path);
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            //string b = loginInfo.Username;
            string tenkv =  _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;

            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            //txtNGAYDENNGAY DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text)
            TextObject txtNGAYDENNGAY = rp.ReportDefinition.ReportObjects["txtNGAYDENNGAY"] as TextObject;
            txtNGAYDENNGAY.Text = "Từ ngày " + txtFromDate.Text.ToString() + " đến ngày " + txtToDate.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbReExcel.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void lkINDSDHKOSD_Click(object sender, EventArgs e)
        {            
                InDSDongHoDaSD();

                upnlGrid.Update();
                CloseWaitingDialog();
        }

        private void InDSDongHoDaSD()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch    {  }
            }

            DataTable dt = new ReportClass().ListDongHo("N1", DateTimeUtil.GetVietNamDate("11/11/2111"), DateTimeUtil.GetVietNamDate("11/11/2111"), _nvDao.Get(b).MAKV).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DanhMucHeThong/DSDONGHODATE.rpt");
            rp.Load(path);
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            //string b = loginInfo.Username;
            string tenkv = _kvDao.Get(_nvDao.Get(b).MAKV).TENKV;

            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            //txtNGAYDENNGAY DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text)
            TextObject txtNGAYDENNGAY = rp.ReportDefinition.ReportObjects["txtNGAYDENNGAY"] as TextObject;
            txtNGAYDENNGAY.Text = "(Đồng hồ nước chưa được sử dụng)";

            divCR.Visible = true;
            upnlCrystalReport.Update();

            lbReExcel.Text = "2";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
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
                    //ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                        //ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = _kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    //ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new System.Web.UI.WebControls.ListItem(kv.TENKV, kv.MAKV));
                        //ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }



    }
}