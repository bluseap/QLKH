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


namespace EOSCRM.Web.Forms.DanhMuc
{
    public partial class DongHo : Authentication
    {
        private readonly DongHoDao _objDao = new DongHoDao();
        private readonly LoaiDongHoDao _ldhDao = new LoaiDongHoDao();




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
                    DASD = chkDASD.Checked,

                    SOKD = txtSOKD.Text.Trim(),
                    TEMKD = txtTEMKD.Text.Trim(),
                    TENCTKD = txtTENCTKD.Text.Trim(),
                    SXTAI = txtSXTAI.Text.Trim()

                };

                if (!string.IsNullOrEmpty(txtHANKD.Text.Trim()))
                    nv.HANKD = DateTimeUtil.GetVietNamDate(txtHANKD.Text);

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
                Authenticate(Functions.DM_DongHo, Permission.Read);
                PrepareUI();               

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                    BindDataForGrid();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_DM_DONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_DONGHO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
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

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        

        
        private void BindDataForGrid()
        {
            if (Filtered == FilteredMode.None)
            {
                var objList = _objDao.GetList();

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            else
            {
                DateTime? fromDate = null;
                DateTime? toDate = null;

                // ReSharper disable EmptyGeneralCatchClause
                try { fromDate = DateTime.Parse(txtFromDate.Text.Trim()); } catch { }
                try { toDate = DateTime.Parse(txtToDate.Text.Trim()); } catch { }
                // ReSharper restore EmptyGeneralCatchClause

                //var objList = _objDao.GetList(txtMADH.Text.Trim(), ddlMALDH.SelectedValue, 
                //            txtNAMSX.Text.Trim(), txtNAMTT.Text.Trim(),
                //            fromDate, toDate, txtTRANGTHAI.Text.Trim());

                var objList = _objDao.GetList2(txtMADH.Text.Trim(), ddlMALDH.SelectedValue,
                           txtSONO.Text.Trim());

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
        }
        
        private void LoadStaticReferences()
        {
            var ldhList = _ldhDao.GetList();
            ddlMALDH.Items.Clear();
            ddlMALDH.Items.Add(new ListItem("Tất cả", "%"));

            foreach(var ldh in ldhList)
                ddlMALDH.Items.Add(new ListItem(ldh.MALDH, ldh.MALDH));
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
                    DateTimeUtil.GetVietNamDate(txtHANKD.Text.Trim());
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
            SetControlValue(txtHANKD.ClientID, obj.HANKD.HasValue ? obj.HANKD.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtNGAYKD.ClientID, obj.NGAYKD.HasValue ? obj.NGAYKD.Value.ToString("dd/MM/yyyy") : "");
            SetControlValue(txtTENCTKD.ClientID, obj.TENCTKD);
            SetControlValue(txtSXTAI.ClientID, obj.SXTAI);

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
                        var objDb = _objDao.Get(id);
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

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            Filtered = FilteredMode.Filtered;
            BindDataForGrid();

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

                Message msg;
                Filtered = FilteredMode.None;

                // insert new
                if (UpdateMode == Mode.Create)
                {
                    if (!HasPermission(Functions.DM_DongHo, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    var tontai = _objDao.Get(txtMADH.Text.Trim());
                    if (tontai != null)
                    {
                        CloseWaitingDialog();
                        ShowError("Mã đồng hồ đã tồn tại", txtMADH.ClientID);
                        return;
                    }

                    //txtMADH.Text= _objDao.NewId();
                    info.MADH = _objDao.NewId();

                    //msg = _objDao.Insert(info);
                   
                    _objDao.Insert2(info);
                    msg = null;
                    BindDataForGrid();
                    upnlGrid.Update();
                    txtSONO.Text = "";
                    txtSONO.Focus();
                    
                }
                // update
                else
                {
                    if (!HasPermission(Functions.DM_DongHo, Permission.Update))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    msg = _objDao.Update(info);
                    
                }                

                CloseWaitingDialog();

                //if (msg == null) return;

                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor(ResourceLabel.Get(msg));

                    ClearForm();

                    // Refresh grid view
                    BindDataForGrid();

                    upnlGrid.Update();
                }
                else
                {
                    ShowError(ResourceLabel.Get(msg));
                }
            }
            catch //(Exception ex)
            {
                //DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
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
            Filtered = FilteredMode.None;
            ClearForm();
        }
    }
}