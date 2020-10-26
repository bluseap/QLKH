using System;
using System.IO;
using System.Web;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Collections.Generic;
namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class NhapThietKeLX : Authentication
    {
        private readonly PhuongDao _pDao = new PhuongDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        private THIETKE ThietKe
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var thietke = new THIETKE
                {
                    MADDK = txtMADDK.Text.Trim(),
                    TENTK = txtTENTK.Text.Trim(),
                    CHUTHICH = txtCHUTHICH.Text.Trim(),
                    MANVLTK = LoginInfo.MANV,
                    THAMGIAONGCAI = cbTHAMGIAONGCAI.Checked,
                    MANVTK = lbNV1.Text.Trim(),
                    TENNVTK = txtNV1.Text.Trim(),
                    SODB = string.Empty.Equals(txtSODB.Text.Trim()) ? "" : txtSODB.Text.Trim().ToUpper(),

                    NGAYN = DateTime.Now
                };

                if (!txtTHECHAP.Text.Trim().Equals(String.Empty))
                    thietke.THECHAP = Int32.Parse(txtTHECHAP.Text.Trim());
                else
                    thietke.THECHAP = null;

                if (!txtNGAYTK.Text.Trim().Equals(String.Empty))
                    thietke.NGAYLTK = DateTimeUtil.GetVietNamDate(txtNGAYTK.Text.Trim());
                else
                    thietke.NGAYLTK = DateTime.Now;

                thietke.DIACHITK = txtDIACHITK.Text.Trim();
                thietke.DUONGHEMTK = txtDUONGHEMTK.Text.Trim();
                thietke.PHUONGTK = ddlPHUONGTK.SelectedValue;
                thietke.SDTTK = txtSDTTK.Text.Trim();
                thietke.VITRIDHTK = txtVITRITK.Text.Trim();
                thietke.DANHSOTK = txtDANHSOTK.Text.Trim();
                thietke.ISKHTT100 = ckKHTT100.Checked;

                thietke.MADPLX = txtMADP.Text.Trim();

                return thietke;
            }
        }

        #region Properties
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
            
        protected String Keyword
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_KEYWORD))
                {
                    return null;
                }
                return EncryptUtil.Decrypt(param[Constants.PARAM_KEYWORD].ToString());
            }
        }

        protected DateTime? FromDate
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_FROMDATE))
                {
                    return null;
                }
                try
                {
                    return DateTimeUtil.GetVietNamDate(EncryptUtil.Decrypt(param[Constants.PARAM_FROMDATE].ToString()));
                }
                catch
                {
                    return null;
                }
            }
        }

        protected DateTime? ToDate
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_TODATE))
                {
                    return null;
                }
                try
                {
                    return DateTimeUtil.GetVietNamDate(EncryptUtil.Decrypt(param[Constants.PARAM_TODATE].ToString()));
                }
                catch
                {
                    return null;
                }
            }
        }

        #endregion

        #region Startup script registeration
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

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
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
                Authenticate(Functions.TK_ThietKeVaVatTuLX, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TK_THIETKEBOCVATTU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_NHAPTHIETKE;
            }

            CommonFunc.SetPropertiesForGrid(gvThietKe);
            CommonFunc.SetPropertiesForGrid(gvDDK);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
            //txtNGAYTK.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void LoadStaticReferences()
        {
            //TODO: Load các đối tượng có liên quan lên UI
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                UpdateMode = Mode.Create;

                /*var list = kvDao.GetList();
                ddlMaKV.Items.Clear();
                ddlMaKV.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in list)
                {
                    ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }*/

                timkv();
                txtNGAYTK.Text = DateTime.Now.ToString("dd/MM/yyyy");

                var phuonglist = _pDao.GetListKV(_nvDao.Get(b).MAKV);
                ddlPHUONGTK.Items.Clear();
                ddlPHUONGTK.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in phuonglist)
                    ddlPHUONGTK.Items.Add(new ListItem(kv.TENPHUONG, kv.MAPHUONG));

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

                if (a.MAKV == "99" && b == "nguyen")
                {
                    var kvList = kvDao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else if (a.MAKV == "99")
                {
                    var kvList = kvDao.GetList();
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    ddlMaKV.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void BindDataForGrid()
        {
            try
            {
                //var objList = ddkDao.GetListForBocVatTu(Keyword, FromDate, ToDate, LoginInfo.MANV);
                //phong ban
                //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                //if (loginInfo == null) return;
                string b = LoginInfo.MANV;
                var pb = _nvDao.GetKV(b);

                if (pb.MAPB == "NB" || pb.MAPB == "TA" || pb.MAPB == "TD"
                    || pb.MAPB == "TS" || pb.MAPB == "TO" || pb.MAPB == "TK" || pb.MAPB == "NS" || pb.MAPB == "NH")
                {
                    var objListPB = ddkDao.GetListForBocVatTuPB(Keyword, FromDate, ToDate, LoginInfo.MANV, pb.MAPB);

                    gvThietKe.DataSource = objListPB;
                    gvThietKe.PagerInforText = objListPB.Count.ToString();
                    gvThietKe.DataBind();
                }
                else //if (pb.MAKV == "O")
                {
                    var objListKV = ddkDao.GetListForBocVatTuKV(Keyword, FromDate, ToDate, LoginInfo.MANV, pb.MAKV);

                    gvThietKe.DataSource = objListKV;
                    gvThietKe.PagerInforText = objListKV.Count.ToString();
                    gvThietKe.DataBind();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private bool IsDataValid()
        {
            if (string.Empty.Equals(txtMADDK.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã đơn"), txtMADDK.ClientID);
                return false;
            }

            var existed = tkDao.Get(txtMADDK.Text.Trim());
            if (existed != null && existed.DONDANGKY.TTTK.Equals(TTTK.TK_P))
            {
                ShowError("Mã đơn đã tồn tại", txtMADDK.ClientID);
                return false;
            }

            if (string.Empty.Equals(txtTENTK.Text.Trim()))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tên thiết kế"), txtTENTK.ClientID);
                return false;
            }

            // check datetime textboxes
            if (!string.Empty.Equals(txtNGAYTK.Text.Trim()))
            {
                try
                {
                    DateTimeUtil.GetVietNamDate(txtNGAYTK.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Ngày thiết kế"), txtNGAYTK.ClientID);
                    return false;
                }
            }

            // check datetime textboxes
            if (!string.Empty.Equals(txtTHECHAP.Text.Trim()))
            {
                try
                {
                    Int32.Parse(txtTHECHAP.Text.Trim());
                }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Tiền thế chấp"), txtTHECHAP.ClientID);
                    return false;
                }
            }

            return true;
        }

        private void ClearContent()
        {
            txtMADDK.Text = "";
            txtTENTK.Text = "";
            txtTENKH.Text = "";
            txtCHUTHICH.Text = "";
            txtNGAYKS.Text = "";
            txtNGAYTK.Text = "";
            txtTHECHAP.Text = "";
            txtSODB.Text = "";
            txtNV1.Text = "";
            cbTHAMGIAONGCAI.Checked = false;
            txtDANHSOTK.Text = "";
            txtVITRITK.Text = "";
            txtSDTTK.Text = "";
            ddlPHUONGTK.SelectedIndex = 0;
            txtDUONGHEMTK.Text = "";
            txtDIACHITK.Text = "";
            lbMADDKTRAHSTK.Text = "";

            ckKHTT100.Checked = false;
        }

        private void BindToInfor(THIETKE obj)
        {            
            txtMADDK.Text = obj.MADDK;
            lbMADDKTRAHSTK.Text = obj.MADDK;

            SetControlValue(txtTENKH.ClientID, ddkDao.Get(obj.MADDK).TENKH);           
            txtTENTK.Text = obj.TENTK;
            SetControlValue(txtNGAYKS.ClientID, obj.DONDANGKY.NGAYKS.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.DONDANGKY.NGAYKS.Value) : "");
            txtNGAYTK.Text = obj.NGAYLTK.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYLTK.Value) : "";
            SetControlValue(txtTHECHAP.ClientID, obj.THECHAP.HasValue ? obj.THECHAP.Value.ToString() : "");
            cbTHAMGIAONGCAI.Checked = obj.THAMGIAONGCAI.HasValue && obj.THAMGIAONGCAI.Value;
            lbNV1.Text = (obj.MANVTK != null) ? obj.MANVTK.ToString() : "";
            txtNV1.Text = (obj.TENNVTK != null) ? obj.TENNVTK.ToString() : "";
            txtSODB.Text = (obj.SODB != null) ? obj.SODB.ToString() : "";
            txtCHUTHICH.Text = obj.CHUTHICH != null ? obj.CHUTHICH : "";

            txtDIACHITK.Text = obj.DIACHITK != null ? obj.DIACHITK : "";
            txtDUONGHEMTK.Text = obj.DUONGHEMTK != null ? obj.DUONGHEMTK : "";

            if (obj.PHUONGTK != null)
            {
                var p = ddlPHUONGTK.Items.FindByValue(obj.PHUONGTK);
                if (p != null)
                    ddlPHUONGTK.SelectedIndex = ddlPHUONGTK.Items.IndexOf(p);
            }

            txtVITRITK.Text = obj.VITRIDHTK != null ? obj.VITRIDHTK : "";
            txtSDTTK.Text = obj.SDTTK != null ? obj.SDTTK : "";
            txtDANHSOTK.Text = obj.DANHSOTK != null ? obj.DANHSOTK : "";

            ckKHTT100.Checked = obj.ISKHTT100.HasValue && obj.ISKHTT100.Value  ;

            txtMADP.Text = obj.MADPLX != null ? obj.MADPLX : "";
            lbTenDuongPho.Text = _dpDao.GetDP(obj.MADPLX) != null ? _dpDao.GetDP(obj.MADPLX).TENDP : "";

            upnlInfor.Update();
        }

        protected void gvThietKe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                switch (e.CommandName)
                {
                    case "SelectMADDK":
                        var ddk = ddkDao.Get(id);
                        if (ddk == null) return;

                        if (ddk.TTTK == "TK_RA")
                        {
                            ShowError("Đã từ chối thiết kế. Không được bốc vật tư");
                            CloseWaitingDialog();
                            return;
                        }

                        if (_nvDao.Get(b).MAKV == "X")
                        {
                            Session["NHAPTHIETKE_MADDK"] = id;
                            var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTuLX.aspx";
                            Response.Redirect(url, false);
                        }
                        else
                        {
                            Session["NHAPTHIETKE_MADDK"] = id;
                            var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx";
                            Response.Redirect(url, false);
                        }

                        break;

                    case "editTK":
                        var obj = tkDao.Get(id);
                        if (obj == null) return;

                        var ddk2 = ddkDao.Get(id);
                        if (ddk2 == null) return;
                        if (ddk2.TTTK == "TK_RA")
                        {
                            ShowError("Đã từ chối thiết kế. Không được bốc vật tư.");
                            CloseWaitingDialog();
                            return;
                        }

                        BindToInfor(obj);
                        UpdateMode = Mode.Update;

                        break;

                    case "SuaViTriDH":
                        var objvt = tkDao.Get(id);
                        if (objvt == null) return;

                        var ddkvt = ddkDao.Get(id);
                        if (ddkvt == null) return;
                        if (ddkvt.TTTK == "TK_RA")
                        {
                            ShowError("Đã từ chối thiết kế. Không được bốc vật tư.");
                            CloseWaitingDialog();
                            return;
                        }

                        lbCOBIENSUAVTDH.Text = "SUAVITRIDH";

                        BindToInfor(objvt);
                        UpdateMode = Mode.Update;

                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvThietKe_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvThietKe.PageIndex = e.NewPageIndex;
                BindDataForGrid();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var don = ThietKe;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            if (query.MAKV != "O" && query.MAKV != "N" && query.MAKV != "X") //chi lay khu vuc thoai son
            {
                if (string.IsNullOrEmpty(txtSODB.Text.Trim()))
                {
                    ShowError("Đường phố chưa có. Nhập đường phố. VD: UA0A0002");
                    CloseWaitingDialog();
                    return;
                }
                else
                {
                    if (txtSODB.Text.Trim().ToUpper().Substring(0, 1) != "U")
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Nhập đường phố trước. VD: UA01"), txtSODB.ClientID);
                        CloseWaitingDialog();
                        return;
                    }

                    if (_dpDao.GetDP(txtSODB.Text.Trim().ToUpper().Substring(0, 4)) == null)
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Đường phố chưa có. Nhập đường phố trước. VD: UA01"), txtSODB.ClientID);
                        CloseWaitingDialog();
                        return;
                    }

                    if (string.Empty.Equals(txtSODB.Text.Trim()) || txtSODB.Text.Trim().Length != 8)
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Xin nhập danh số bằng 8 ký tự. Ví du: UA0A0002"), txtSODB.ClientID);
                        CloseWaitingDialog();
                        return;
                    }
                }
            }

            if (ddlPHUONGTK.SelectedValue == "%")
            {
                ShowError("Chọn phường, xã. Kiểm tra lại");
                CloseWaitingDialog();
                return;
            }

            Message msg;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = tkDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBien(don.MADDK, LoginInfo.MANV, query.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "INTHIETKE");
            }
            else
            {
                if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = tkDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBien(don.MADDK, LoginInfo.MANV, query.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "UPTHIETKE");

                if (!string.IsNullOrEmpty(lbCOBIENSUAVTDH.Text.Trim()) || lbCOBIENSUAVTDH.Text != "")
                {
                    var dontktc = ddkDao.Get(don.MADDK);

                    if (dontktc.TTTC != "TC_RA")
                    {
                        ShowError("Chưa nhập thi công hoặc trả hồ sơ từ thi công về thiết kế. Kiểm tra lại.");
                        CloseWaitingDialog();
                        return;
                    }

                    msg = ddkDao.DuyetTKCTTraHSThiCong(dontktc, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV); 
                }
            }

            CloseWaitingDialog();

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                //Trả lại màn hình trống ban đầu
                ClearContent();

                upnlInfor.Update();
                BindDataForGrid();
                upnlGrid.Update();

                ShowInfor(ResourceLabel.Get(msg));
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            UpdateMode = Mode.Create;

            CloseWaitingDialog();
        }

        private void BindToInfor(DONDANGKY obj)
        {            
            txtMADDK.Text = obj.MADDK;
            SetControlValue(txtTENKH.ClientID, ddkDao.Get(obj.MADDK).TENKH);            
            txtTENTK.Text = "Lắp mới";
            SetControlValue(txtNGAYKS.ClientID, obj.NGAYKS.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYKS.Value) : "");

            txtSODB.Text = obj.MADPKHGAN != null ? obj.MADPKHGAN + (obj.MADBKHGAN != null ? obj.MADBKHGAN : "") : "";

            upnlInfor.Update();
        }

        protected void btnBrowseDDK_Click(object sender, EventArgs e)
        {
            BindDDK();
            upnlDonDangKy.Update();
            UnblockDialog("divDonDangKy");
        }

        private void BindDDK()
        {
            DateTime? tungay = null;
            DateTime? denngay = null;
            try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); }
            catch { txtTuNgay.Text = ""; }
            try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); }
            catch { txtDenNgay.Text = ""; }

            //phong ban
            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            string b = LoginInfo.MANV;
            var pb = _nvDao.GetKV(b);

            //var list = ddkDao.GetList(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, LoginInfo.MANV);
            //var list = ddkDao.GetListPB(txtFilter.Text.Trim(), pb.MAPB.ToString());
            var list = ddkDao.GetListPBKV(txtFilter.Text.Trim(), pb.MAPB.ToString(), pb.MAKV);

            gvDDK.DataSource = list;
            gvDDK.PagerInforText = list.Count.ToString();
            gvDDK.DataBind();
        }

        protected void gvDDK_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDDK_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDDK.PageIndex = e.NewPageIndex;
                BindDDK();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDDK_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var obj = ddkDao.Get(id);
                        if (obj == null) return;

                        BindToInfor(obj);
                        CloseWaitingDialog();
                        HideDialog("divDonDangKy");

                        UpdateMode = Mode.Create;

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
            BindDDK();
            CloseWaitingDialog();
        }

        protected void btnFilterNV_Click(object sender, EventArgs e)
        {
            BindNhanVien();
            CloseWaitingDialog();
        }

        protected void btnBrowseNhanVien_Click(object sender, EventArgs e)
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
                        var nv = _nvDao.Get(id);
                        if (nv != null)
                        {
                            //txtMANV.Focus();
                            lbNV1.Text = id.ToString();
                            txtNV1.Text = nv.HOTEN.ToString();                           

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
                gvNhanVien.PageIndex = e.NewPageIndex;
                BindNhanVien();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindNhanVien()
        {
            //var list = _nvDao.SearchKV3(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
            var list = _nvDao.SearchKVPB(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
        }

        protected void btnTUCHOI_Click(object sender, EventArgs e)
        {
            try
            {
                var thietke = ThietKe;
                if (thietke == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;

                // Authenticate
                if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (!string.IsNullOrEmpty(txtMADDK.Text.Trim()) || txtMADDK.Text != "")
                {
                    var objs = ddkDao.Get(txtMADDK.Text.Trim());

                    try { objs.NGAYKS = Convert.ToDateTime(txtCHUTHICH.Text); }
                    catch { }

                    objs.TTTK = TTTK.TK_RA.ToString();
                    objs.NOIDUNG = txtCHUTHICH.Text.Trim();

                    if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }                    

                    var msg2 = tkDao.Insert(thietke, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.UPKHCOBIEN(txtMADDK.Text.Trim(), makv, 0, 0, "TK_RA", txtCHUTHICH.Text.Trim(), "UPTUCHOITK");

                    _rpClass.HisNgayDangKyBien(thietke.MADDK, LoginInfo.MANV, makv, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "INTHIETKE");

                    _rpClass.HisNgayDangKyBien(thietke.MADDK, LoginInfo.MANV, makv, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOITK");

                    //TODO: check relation before update list
                    //ddkDao.UpdateTuChoiTK(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    if (msg2 != null)
                    {
                        if (msg2.MsgType != MessageType.Error)
                        {
                            ClearContent();

                            CloseWaitingDialog();
                            ShowInfor(ResourceLabel.Get(msg2));
                            
                            BindDataForGrid();
                        }
                        else
                        {
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msg2));
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowError("Từ chối đơn khảo sát thiết kế không thành công.");
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Vui lòng chọn đơn cần từ chối.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btTRAHOSO_Click(object sender, EventArgs e)
        {
            try
            {
                var thietke = ThietKe;
                if (thietke == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;

                // Authenticate
                if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if ((string.IsNullOrEmpty(txtMADDK.Text.Trim()) || txtMADDK.Text == "")
                        && (string.IsNullOrEmpty(lbMADDKTRAHSTK.Text.Trim()) || lbMADDKTRAHSTK.Text == ""))
                {
                    CloseWaitingDialog();
                    ShowError("Chọn khách hàng cần trả hồ sơ. Kiểm tra lại.");
                    return;
                }

                if ((!string.IsNullOrEmpty(txtMADDK.Text.Trim()) || txtMADDK.Text != "")
                        && (string.IsNullOrEmpty(lbMADDKTRAHSTK.Text.Trim()) || lbMADDKTRAHSTK.Text == ""))
                {
                    var objs = ddkDao.Get(txtMADDK.Text.Trim());

                    try { objs.NGAYKS = Convert.ToDateTime(txtCHUTHICH.Text); }
                    catch { }

                    objs.TTTK = TTTK.TK_RA.ToString();
                    objs.NOIDUNG = txtCHUTHICH.Text.Trim();

                    if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    thietke.NGAYTRAHSKD = DateTime.Now;
                    thietke.LYDOTRAHSKD = txtCHUTHICH.Text.Trim();

                    var msg2 = tkDao.Insert(thietke, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.UPKHCOBIEN(txtMADDK.Text.Trim(), makv, 0, 0, "TK_RA", txtCHUTHICH.Text.Trim(), "TRAHSKHTK");

                    _rpClass.HisNgayDangKyBien(thietke.MADDK, LoginInfo.MANV, makv, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "INTHIETKE");

                    _rpClass.HisNgayDangKyBien(thietke.MADDK, LoginInfo.MANV, makv, DateTime.Now, DateTime.Now, DateTime.Now,
                       "", "", "", "", "TRAHSTKKD");

                    if (msg2 != null)
                    {
                        if (msg2.MsgType != MessageType.Error)
                        {
                            ClearContent();

                            CloseWaitingDialog();
                            ShowInfor(ResourceLabel.Get(msg2));

                            BindDataForGrid();
                        }
                        else
                        {
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msg2));
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowError("Từ chối đơn khảo sát thiết kế không thành công.");
                    }
                }                

                if ((!string.IsNullOrEmpty(lbMADDKTRAHSTK.Text.Trim()) || lbMADDKTRAHSTK.Text != "")
                        && (!string.IsNullOrEmpty(txtMADDK.Text.Trim()) || txtMADDK.Text != ""))
                {
                    var objs = ddkDao.Get(lbMADDKTRAHSTK.Text.Trim());

                    try { objs.NGAYKS = Convert.ToDateTime(txtCHUTHICH.Text); }
                    catch { }

                    objs.TTTK = TTTK.TK_RA.ToString();
                    objs.NOIDUNG = txtCHUTHICH.Text.Trim();

                    if (!HasPermission(Functions.TK_ThietKeVaVatTuLX, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }

                    thietke.NGAYTRAHSKD = DateTime.Now;
                    thietke.LYDOTRAHSKD = txtCHUTHICH.Text.Trim();

                    var msg2 = tkDao.Update(thietke, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    _rpClass.UPKHCOBIEN(txtMADDK.Text.Trim(), makv, 0, 0, "TK_RA", txtCHUTHICH.Text.Trim(), "TRAHSKHTK");

                    _rpClass.HisNgayDangKyBien(thietke.MADDK, LoginInfo.MANV, makv, DateTime.Now, DateTime.Now, DateTime.Now,
                       "", "", "", "", "TRAHSTKKD");

                    if (msg2 != null)
                    {
                        if (msg2.MsgType != MessageType.Error)
                        {
                            ClearContent();

                            CloseWaitingDialog();
                            ShowInfor(ResourceLabel.Get(msg2));

                            BindDataForGrid();
                        }
                        else
                        {
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msg2));
                        }
                    }
                    else
                    {
                        CloseWaitingDialog();
                        ShowError("Từ chối đơn khảo sát thiết kế không thành công.");
                    }
                }                             

                upnlInfor.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            CloseWaitingDialog();
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            if (txtMADP.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var dp = _dpDao.GetDP(txtMADP.Text.Trim());

            if (dp != null)
            {
                UpdateKhuVuc(dp);
                CloseWaitingDialog();
            }
            else
            {
                CloseWaitingDialog();
                ShowError("Mã đường phố không hợp lệ");
            }
        }

        private void BindDuongPho()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var list = _dpDao.GetList(_nvDao.Get(b).MAKV, txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            txtMADP.Text = dp.MADP;
            lbTenDuongPho.Text = dp.TENDP;

            //var kv = ddlKHUVUC.Items.FindByValue(dp.MAKV);
            //if (kv != null)
            //{
            //    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(kv);
            //    LoadDynamicReferences();
            //}
        }

        #region gvDuongPho
        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnIDDP") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":

                        var dp = _dpDao.GetDP(id);
                        if (dp != null)
                        {
                            txtMADP.Text = dp.MADP;
                            lbTenDuongPho.Text = dp.TENDP;

                            //UpdateKhuVuc(dp);
                            upnlInfor.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvDuongPho.PageIndex = e.NewPageIndex;
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        #endregion

    }
}