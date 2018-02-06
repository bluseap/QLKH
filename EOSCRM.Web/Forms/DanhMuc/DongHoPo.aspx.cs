using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using CrystalDecisions.CrystalReports.Engine;
using System.Data;

using System.IO;
using System.Web.UI;

namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class DongHoPo : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly LoaiDongHoPoDao _ldhpoDao = new LoaiDongHoPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly KyDuyetDao _lvkdDao = new KyDuyetDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        private void SetReadonly(string id, bool isReadonly)
        {
            ((PO)Page.Master).SetReadonly(id, isReadonly);
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

        private void ShowWarning(string message)
        {
            ((PO)Page.Master).ShowWarning(message);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }
        #endregion

        #region loc,up
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

        private DONGHOPO ItemObj
        {
            get
            {
                if (!ValidateData())
                    return null;
                var nv = new DONGHOPO
                {
                    MADHPO = txtMADH.Text.Trim(),
                    MALDHPO = ddlMALDH.SelectedValue,
                    TRANGTHAI = txtTRANGTHAI.Text.Trim(),
                    SONO = txtSONO.Text.Trim(),
                    DASD = chkDASD.Checked,
                    SOKD = txtSOKD.Text.Trim(),
                    TEMKD = txtTEMKD.Text.Trim(),
                    TENCTKD = txtTENCTKD.Text.Trim(),
                    SXTAI = txtSXTAI.Text.Trim(),
                    MAKVPO = ddlKHUVUC.SelectedValue,
                    CONGSUAT = ddlCONGSUATD.SelectedValue,
                    NGAYNHAP = DateTime.Now                    
                };
                //if (!string.IsNullOrEmpty("11/" + txtHANKD.Text.Trim()))
                //    nv.HANKD = DateTimeUtil.GetVietNamDate("11/" + txtHANKD.Text);
                if (!string.IsNullOrEmpty(txtHANKD.Text.Trim()))
                    nv.HANKD = DateTimeUtil.GetVietNamDate(txtHANKD.Text);
                //else nv.HANKD = null;

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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_DongHoPo, Permission.Read);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                    EventEnter();
                }
                else
                {
                    if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()) && !string.IsNullOrEmpty(txtToDate.Text.Trim()))
                        InDSDongHoPo(DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text));
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
            Page.Title = Resources.Message.TITLE_DM_DONGHOPO;
            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_DONGHOPO;
            }
            CommonFunc.SetPropertiesForGrid(gvList);
        }        

        private void BindDataForGrid()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

            if (Filtered == FilteredMode.None)
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
                        var objList = _dhpoDao.GetListKV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                                    txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                                    fromDate, toDate.Value.AddDays(1), txtTRANGTHAI.Text.Trim(), makvpo);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();

                    }
                    else
                    {
                        var objList = _dhpoDao.GetList2KV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, txtSONO.Text.Trim(), makvpo);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }                  
                }
                catch { }

                //DateTime? fromDate = null;
                //DateTime? toDate = null;
                // ReSharper disable EmptyGeneralCatchClause
                //try { fromDate = DateTimeUtil.GetVietNamDate(txtFromDate.Text.Trim()); }
                //catch { }
                //try { toDate = DateTimeUtil.GetVietNamDate(txtToDate.Text.Trim()); }
                //catch { }

                //var objList = _dhpoDao.GetListKV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                //            txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                //            fromDate, toDate, txtTRANGTHAI.Text.Trim(), makvpo);

                //gvList.DataSource = objList;
                //gvList.PagerInforText = objList.Count.ToString();
                //gvList.DataBind();

                //var objList = _dhpoDao.GetListKV(ddlKHUVUC.SelectedValue);
                //gvList.DataSource = objList;
                //gvList.PagerInforText = objList.Count.ToString();
                //gvList.DataBind();
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
                        var objList = _dhpoDao.GetListKV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                                    txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                                    fromDate, toDate.Value.AddDays(1), txtTRANGTHAI.Text.Trim(), makvpo);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();

                    }
                    else
                    {
                        var objList = _dhpoDao.GetList2KV(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, txtSONO.Text.Trim(), makvpo);

                        gvList.DataSource = objList;
                        gvList.PagerInforText = objList.Count.ToString();
                        gvList.DataBind();
                    }

                    //DateTime? fromDate = null;
                    //DateTime? toDate = null;
                    //// ReSharper disable EmptyGeneralCatchClause
                    //try { fromDate = DateTime.Parse(txtFromDate.Text.Trim()); }
                    //catch { }
                    //try { toDate = DateTime.Parse(txtToDate.Text.Trim()); }
                    //catch { }              

                    //var objList = _dhpoDao.GetList2(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                    //           txtSONO.Text.Trim(), ddlKHUVUC.SelectedValue);

                    //gvList.DataSource = objList;
                    //gvList.PagerInforText = objList.Count.ToString();
                    //gvList.DataBind();
                }
                catch { }
            }
        }

        private void LoadStaticReferences()
        {
            var ldhList = _ldhpoDao.GetList();
            ddlMALDH.Items.Clear();
            ddlMALDH.Items.Add(new ListItem("Tất cả", "%"));
            foreach (var ldh in ldhList)
                ddlMALDH.Items.Add(new ListItem(ldh.MALDHPO, ldh.MALDHPO));

            timkv();

            ddlCONGSUATD.SelectedIndex = 0;
        }

        public bool ValidateData()
        {
            /*if (string.Empty.Equals(txtMADH.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đồng hồ"), txtMADH.ClientID);
                return false;
            }*/
            var ldh = _ldhpoDao.Get(ddlMALDH.SelectedValue);
            if (ldh == null)
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
            /*if (!string.IsNullOrEmpty(txtHANKD.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate("11/" + txtHANKD.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Hạn kiểm định"), txtHANKD.ClientID);
                    return false;
                }
            }*/
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

            //if (string.IsNullOrEmpty(txtCONGSUATPO.Text.Trim()) || txtCONGSUATPO.Text == "")
            //{
            //    try
            //    {
            //        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Công suất đồng hồ điện "), txtCONGSUATPO.ClientID);
            //        return false;
            //    }
            //    catch
            //    {
            //        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Công suất đồng hồ điện "), txtCONGSUATPO.ClientID);
            //        return false;
            //    }
            //}
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

            ddlCONGSUATD.SelectedIndex = 0;
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
                    var objs = new List<DONGHOPO>();
                    var listIds = strIds.Split(',');
                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_dhpoDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "đồng hồ với mã", ma);
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msgIsInUse));
                            return;
                        }
                    }
                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _dhpoDao.Get(ma)));
                    if (objs.Count > 0)
                    {
                        var msg = _dhpoDao.DeleteList(objs);
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

        private void BindItem(DONGHOPO obj)
        {
            if (obj == null)
                return;
            //SetControlValue(txtMADH.ClientID, obj.MADH);
            //SetReadonly(txtMADH.ClientID, true);
            txtMADH.Text = obj.MADHPO;
            var ldh = ddlMALDH.Items.FindByValue(obj.MALDHPO);
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
            SetControlValue(txtHANKD.ClientID, obj.HANKD.HasValue ? obj.HANKD.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtNGAYKD.ClientID, obj.NGAYKD.HasValue ? obj.NGAYKD.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtTENCTKD.ClientID, obj.TENCTKD);
            SetControlValue(txtSXTAI.ClientID, obj.SXTAI);
            //xem ngay nhap dong ho nuoc
            String _dhtt = "DH_A";
            String _dhmota = "Nhập đồng hồ điện.";
            var dhn = _lvkdDao.GetMaDon(obj.MADHPO, _dhtt, _dhmota);
            if (dhn != null)
            { lbNGAYNHAPDH.Text = dhn.NGAYTHUCHIEN.Value.ToString("dd/MM/yyyy"); }
            else { lbNGAYNHAPDH.Text = ""; }

            var cs = ddlCONGSUATD.Items.FindByValue(obj.CONGSUAT);
            if (cs != null)
                ddlCONGSUATD.SelectedIndex = ddlCONGSUATD.Items.IndexOf(cs);            

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
                var objDb = _dhpoDao.Get(id);

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

                                var thicong = _tcDao.GetMADH(objDb.MADHPO);
                                if (thicong != null)
                                {
                                    var dondk = _ddkpoDao.Get(thicong.MADDK);

                                    txtMADDKTT.Text = dondk.MADDKPO;
                                    txtTENKHTT.Text = dondk.TENKH;
                                }

                                var khachhang = _khpoDao.GetMADH(objDb.MADHPO);
                                if (khachhang != null)
                                {
                                    txtDANHSOTT.Text = khachhang.MADPPO + khachhang.MADBPO;
                                    txtTENKHKTTT.Text = khachhang.TENKH;
                                }

                                UnblockDialog("divDonKhachHang");
                                upDonKhachHang.Update();
                            }
                            else
                            {
                                ShowInfor("Chọn đồng hồ đã sử dụng.");
                                HideDialog("divDonKhachHang");
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

            if (!string.IsNullOrEmpty(txtFromDate.Text.Trim()) && !string.IsNullOrEmpty(txtToDate.Text.Trim()))
                InDSDongHoPo(DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text));

            upnlGrid.Update();
            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var info = ItemObj;
                if (info == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                
                Message msg;
                //Filtered = FilteredMode.None;
                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.DM_DongHoPo, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    var tontai = _dhpoDao.Get(txtMADH.Text.Trim());
                    if (tontai != null)
                    {
                        CloseWaitingDialog();
                        ShowError("Mã đồng hồ đã tồn tại", txtMADH.ClientID);
                        return;
                    }
                    //txtMADH.Text= _objDao.NewId();
                    info.MADHPO = _dhpoDao.NewIdMAKV(_kvpoDao.GetPo(_nvDao.Get(LoginInfo.MANV).MAKV).MAKVPO);
                    info.MANVNHAP = LoginInfo.MANV;
                    //msg = _objDao.Insert(info);
                    _dhpoDao.Insert2(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    msg = null;
                    BindDataForGrid();
                    upnlGrid.Update();
                    txtSOKD.Text = "";
                    txtSONO.Text = "";
                    txtTEMKD.Text = "";
                    //txtSONO.Focus();
                    txtSOKD.Focus();

                    ddlCONGSUATD.SelectedIndex = 0;
                }
                // update
                else
                {
                    if (!HasPermission(Functions.DM_DongHoPo, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    _rpClass.DongHoPo_His(info.MADHPO, b, DateTime.Now);

                    var dhsua = _dhpoDao.Get(txtMADH.Text);

                    if (_dhpoDao.Get(txtMADH.Text).DASD != true)
                    {
                        msg = _dhpoDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);                    
                        
                    }
                    else
                    {
                        dhsua.MADHPO = txtMADH.Text.Trim();
                        dhsua.MALDHPO = ddlMALDH.SelectedValue;
                        dhsua.TRANGTHAI = txtTRANGTHAI.Text.Trim();
                        dhsua.SONO = txtSONO.Text.Trim();
                        dhsua.DASD = chkDASD.Checked;
                        dhsua.SOKD = txtSOKD.Text.Trim();
                        dhsua.TEMKD = txtTEMKD.Text.Trim();
                        dhsua.TENCTKD = txtTENCTKD.Text.Trim();
                        dhsua.SXTAI = txtSXTAI.Text.Trim();
                        dhsua.MAKVPO = ddlKHUVUC.SelectedValue;
                        dhsua.CONGSUAT = ddlCONGSUATD.SelectedValue;

                        msg = _dhpoDao.Update(dhsua, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);        

                        //if (LoginInfo.MANV == "nguyen")
                        //{
                        //    msg = _dhpoDao.Update(info, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                        //}
                        //else
                        //{
                        //    ShowInfor("Đồng hồ đang sử dụng. Bạn không đủ quyền sửa!");
                        //}
                    }
                    ClearForm();  
                }

                CloseWaitingDialog();
                //ClearForm();  
                ddlCONGSUATD.SelectedIndex = 0;

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
                if (!HasPermission(Functions.DM_DongHoPo, Permission.Delete))
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

        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = _kvpoDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKVPO(d);
                    var khuvuc = _kvpoDao.GetPo(d);

                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }

                }
            }
        }

        private void InDSDongHoPo(DateTime tungay, DateTime denngay)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var makvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO;

            ReportDocument rp = (ReportDocument)Session[Constants.REPORT_FREE_MEM];
            if (rp != null)
            {
                try
                {
                    rp.Close();
                    rp.Dispose();
                    GC.Collect();
                }
                catch { }
            }
           
            DataTable dt = new ReportClass().ListDongHo("P0", tungay, denngay, makvpo).Tables[0];
            // DataTable dt = new ReportClass().RPDSCDDangKy(TuNgay, DenNgay.AddDays(1), cboKhuVuc.Text).Tables[0];

            rp = new ReportDocument();
            //var path = Server.MapPath("../../../Reports/QuanLyGhiDHTinhCuocInHD/DSTTBANG.rpt");
            var path = Server.MapPath("~/Reports/DanhMucHeThong/DSDONGHOPODATE.rpt");
            rp.Load(path);
            rp.SetDataSource(dt);
            rpViewer.ReportSource = rp;
            rpViewer.DataBind();

            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            //string b = loginInfo.Username;
            string tenkv = _kvpoDao.GetPo(_nvDao.Get(b).MAKV).TENKV;

            TextObject xn1 = rp.ReportDefinition.ReportObjects["txtTENKV"] as TextObject;
            xn1.Text = "XN ĐIỆN NƯỚC " + tenkv.ToString().ToUpper();
            //txtNGAYDENNGAY DateTimeUtil.GetVietNamDate(txtFromDate.Text), DateTimeUtil.GetVietNamDate(txtToDate.Text)
            TextObject txtNGAYDENNGAY = rp.ReportDefinition.ReportObjects["txtNGAYDENNGAY"] as TextObject;
            txtNGAYDENNGAY.Text = "Từ ngày " + txtFromDate.Text.ToString() + " đến ngày " + txtToDate.Text.ToString();

            divCR.Visible = true;
            upnlCrystalReport.Update();

            //lbReExcel.Text = "1";

            Session["DS_DonDangKy"] = dt;
            Session[Constants.REPORT_FREE_MEM] = rp;
            CloseWaitingDialog();
        }

        protected void btnXuatExcel_Click(object sender, EventArgs e)
        {
            XuatDuLieuRaExcel();

            CloseWaitingDialog();
            upnlInfor.Update();  
        }

        private void XuatDuLieuRaExcel()
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

                var objList = _rpClass.BienKHPoTuDenNgay(fromDate, toDate, "", "", _kvpoDao.GetPo(_nvDao.Get(b).MAKV).MAKVPO, "", "", "", "DSDHPOTHEONGAY");

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
            catch { }
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

    }
}