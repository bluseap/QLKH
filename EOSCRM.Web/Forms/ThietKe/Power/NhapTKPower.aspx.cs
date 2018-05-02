using System;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Linq;
using System.Collections.Generic;


namespace EOSCRM.Web.Forms.ThietKe.Power
{
    public partial class NhapTKPower : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

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

        private THIETKEPO ThietKePo
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var thietke = new THIETKEPO
                {
                    MADDKPO = txtMADDK.Text.Trim(),
                    TENTK = txtTENTK.Text.Trim(),
                    CHUTHICH = txtCHUTHICH.Text.Trim(),
                    MANVLTK = LoginInfo.MANV,
                    THAMGIAONGCAI = cbTHAMGIAONGCAI.Checked,
                    MANVTK = lbNV1.Text.Trim(),
                    TENNVTK = txtNV1.Text.Trim(),
                    SODB = string.Empty.Equals(txtSODB.Text.Trim()) ? "" : txtSODB.Text.Trim().ToUpper(),
                    SOTRUKH = txtSOTRUKH.Text.Trim(),
                    TENTRAMKH = ddlTENTRAMDS.SelectedValue != "%" ?
                        ddlTENTRAMDS.SelectedValue + ". " + _dppoDao.GetDP(ddlTENTRAMDS.SelectedValue).TENDP : "",
                    TUYENDAYHATHE = txtTUYENDAYHATHEKH.Text.Trim(),

                    NGAY = DateTime.Now
                };

                if (!txtTHECHAP.Text.Trim().Equals(String.Empty))
                    thietke.THECHAP = Int32.Parse(txtTHECHAP.Text.Trim());
                else
                    thietke.THECHAP = null;

                if (!txtNGAYTK.Text.Trim().Equals(String.Empty))
                    thietke.NGAYLTK = DateTimeUtil.GetVietNamDate(txtNGAYTK.Text.Trim());
                else
                    thietke.NGAYLTK = null;

                return thietke;
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

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }

        private void HideDialog(string divId)
        {
            ((PO)Page.Master).HideDialog(divId);
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_ThietKeVaVatTuPo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TK_THIETKEBOCVATTUPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_NHAPTHIETKEPO;
            }

            CommonFunc.SetPropertiesForGrid(gvThietKe);
            CommonFunc.SetPropertiesForGrid(gvDDK);
            //txtNGAYTK.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }        

        private void LoadStaticReferences()
        {
            //TODO: Load các đối tượng có liên quan lên UI
            try
            {
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

                //ten tram la danh so duong pho
                var lTRAMDP = _dppoDao.GetList(ddlMaKV.SelectedValue);
                ddlTENTRAMDS.Items.Clear();
                ddlTENTRAMDS.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var tram in lTRAMDP)
                {
                    ddlTENTRAMDS.Items.Add(new ListItem(tram.MADPPO + ": " + tram.TENDP , tram.MADPPO));
                }
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
                    var kvList = _kvpoDao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else if (a.MAKV == "99")
                {   
                    var kvList = _kvpoDao.GetList();
                    ddlMaKV.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
                    }
                }
                else
                {
                    var kvList = _kvpoDao.GetListKVPO(d);
                    var khuvuc = _kvpoDao.GetPo(d);                    

                    ddlMaKV.Items.Clear();                   
                    foreach (var kv in kvList)
                    {
                        ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKVPO));
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
                var kvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV);

                if (pb.MAPB == "NB" || pb.MAPB == "TA" || pb.MAPB == "TD"
                        || pb.MAPB == "TO" || pb.MAPB == "TK" || pb.MAPB == "TS"
                        || pb.MAPB == "NS" || pb.MAPB == "NH"
                        || pb.MAPB == "CV" || pb.MAPB == "HL" || pb.MAPB == "MM" || pb.MAPB == "PM" // PHU TAN
                        || pb.MAPB == "BC" || pb.MAPB == "CT" || pb.MAPB == "NT" || pb.MAPB == "TT" // tri ton
                        || pb.MAPB == "CL" || pb.MAPB == "MB" || pb.MAPB == "NC" || pb.MAPB == "TB" // TINH BIEN
                        || pb.MAPB == "AT" || pb.MAPB == "CM" || pb.MAPB == "HB" || pb.MAPB == "KT" // CHO MOI
                        || pb.MAPB == "LG" || pb.MAPB == "ML" || pb.MAPB == "NL" || pb.MAPB == "TM" // CHO MOI
                        || pb.MAPB == "LA" || pb.MAPB == "NC" || pb.MAPB == "VH") // TAN CHAU)
                {
                    var objListPB = _ddkpoDao.GetListForBocVatTuPB(Keyword, FromDate, ToDate, LoginInfo.MANV, pb.MAPB, kvpo.MAKVPO);

                    gvThietKe.DataSource = objListPB;
                    gvThietKe.PagerInforText = objListPB.Count.ToString();
                    gvThietKe.DataBind();
                }
                else //if (pb.MAKV == "O")
                {
                    var objListKV = _ddkpoDao.GetListForBocVatTuKV(Keyword, FromDate, ToDate, LoginInfo.MANV, kvpo.MAKVPO);

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

            var existed = _tkpoDao.Get(txtMADDK.Text.Trim());
            if (existed != null && existed.DONDANGKYPO.TTTK.Equals(TTTK.TK_P))
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
            txtSOTRUKH.Text = "";            
            txtTUYENDAYHATHEKH.Text = "";

            ddlTENTRAMDS.SelectedIndex = 0;
        }

        private void BindToInfor(THIETKEPO obj)
        {
            //SetControlValue(txtMADDK.ClientID, obj.MADDK);
            txtMADDK.Text = obj.MADDKPO;
            SetControlValue(txtTENKH.ClientID, _ddkpoDao.Get(obj.MADDKPO).TENKH);
            //SetControlValue(txtTENTK.ClientID, obj.TENTK);
            txtTENTK.Text = obj.TENTK;
            SetControlValue(txtNGAYKS.ClientID, obj.DONDANGKYPO.NGAYKS.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.DONDANGKYPO.NGAYKS.Value) : "");
            txtNGAYTK.Text = obj.NGAYLTK.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYLTK.Value) : "";
            SetControlValue(txtTHECHAP.ClientID, obj.THECHAP.HasValue ? obj.THECHAP.Value.ToString() : "");
            cbTHAMGIAONGCAI.Checked = obj.THAMGIAONGCAI.HasValue && obj.THAMGIAONGCAI.Value;
            lbNV1.Text = (obj.MANVTK != null) ? obj.MANVTK.ToString() : "";
            txtNV1.Text = (obj.TENNVTK != null) ? obj.TENNVTK.ToString() : "";
            txtSODB.Text = (obj.SODB != null) ? obj.SODB.ToString() : "";
            txtCHUTHICH.Text = (obj.CHUTHICH != null) ? obj.CHUTHICH : "";
            txtSOTRUKH.Text = obj.SOTRUKH != null ? obj.SOTRUKH.ToString() : "";            
            txtTUYENDAYHATHEKH.Text = obj.TUYENDAYHATHE != null ? obj.TUYENDAYHATHE.ToString() : "";

            var item = ddlTENTRAMDS.Items.FindByValue(obj.TENTRAMKH.Substring(0,4));
            if (item != null)
                ddlTENTRAMDS.SelectedIndex = ddlTENTRAMDS.Items.IndexOf(item);
            //ddlTENTRAMDS


            upnlInfor.Update();
        }

        protected void gvThietKe_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADDK":
                        var ddk = _ddkpoDao.Get(id);
                        if (ddk == null) return;

                        if (ddk.TTTK == "TK_A")
                        {
                            ShowError("Đã duyệt thiết kế điện. Không được bốc vật tư");
                            CloseWaitingDialog();
                            return;
                        }

                        if (ddk.TTTK == "TK_RA")
                        {
                            ShowError("Đã từ chối thiết kế điện. Không được bốc vật tư");
                            CloseWaitingDialog();
                            return;
                        }

                        Session["NHAPTHIETKE_MADDK"] = id;
                        var url = ResolveUrl("~") + "Forms/ThietKe/Power/BocVatTuPower.aspx";
                        Response.Redirect(url, false);
                        break;

                    case "editTK":
                        var ddk2 = _ddkpoDao.Get(id);
                        if (ddk2 == null) return;

                        var obj = _tkpoDao.Get(id);
                        if (obj == null) return;

                        //if (ddk2.TTTK == "TK_A" && ddk2.TTCT == "CT_N" //chi tri ton,an phu,tinh bien,thoai son,chau phu,chau doc
                        //    || (ddk2.TTTK == "TK_A" && (ddk2.TTCT == null || ddk2.TTHD == null)))
                        //{
                        //    BindToInfor(obj);
                        //}    

                        BindToInfor(obj);
                   
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
            var don = ThietKePo;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            if (query.MAKV != "O" && query.MAKV != "N" && query.MAKV != "S" && query.MAKV != "P"
                && query.MAKV != "K" && query.MAKV != "L" && query.MAKV != "M" && query.MAKV != "Q"
                )   //chi lay khu vuc thoai son, tan chau
            {
                if (string.IsNullOrEmpty(txtSODB.Text.Trim()))
                {
                    ShowError("Đường phố chưa có. Nhập đường phố. VD: IA0A0002");
                    CloseWaitingDialog();
                    return;
                }
                else
                {
                    if (txtSODB.Text.Trim().ToUpper().Substring(0, 1) != "I" || txtSODB.Text.Trim().ToUpper().Substring(0, 1) != "H")
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Nhập đường phố trước. VD: IA01"), txtSODB.ClientID);
                        CloseWaitingDialog();
                        return;
                    }

                    if (_dppoDao.GetDP(txtSODB.Text.Trim().ToUpper().Substring(0, 4)) == null)
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Đường phố chưa có. Nhập đường phố trước. VD: IA01"), txtSODB.ClientID);
                        CloseWaitingDialog();
                        return;
                    }

                    if (string.Empty.Equals(txtSODB.Text.Trim()) || txtSODB.Text.Trim().Length != 8)
                    {
                        ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Xin nhập danh số bằng 8 ký tự. Ví du: IA0A0002"), txtSODB.ClientID);
                        CloseWaitingDialog();
                        return;
                    }
                }
            }

            Message msg;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.TK_ThietKeVaVatTuPo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (ddlTENTRAMDS.SelectedValue == "%")
                {
                    CloseWaitingDialog();
                    ShowInfor("Chọn tên trạm.");
                    return;
                }

                msg = _tkpoDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBienPo(don.MADDKPO, LoginInfo.MANV, _kvpoDao.GetPo(query.MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "INTHIETKE");
            }
            else
            {
                if (!HasPermission(Functions.TK_ThietKeVaVatTuPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = _tkpoDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.HisNgayDangKyBienPo(don.MADDKPO, LoginInfo.MANV, _kvpoDao.GetPo(query.MAKV).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "UPTHIETKE");
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
                // Show message
                ShowError(ResourceLabel.Get(msg));
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearContent();
            UpdateMode = Mode.Create;

            CloseWaitingDialog();
        }

        private void BindToInfor(DONDANGKYPO obj)
        {            
            txtMADDK.Text = obj.MADDKPO;
            SetControlValue(txtTENKH.ClientID, _ddkpoDao.Get(obj.MADDKPO).TENKH);            
            txtTENTK.Text = "Lắp mới";
            SetControlValue(txtNGAYKS.ClientID, obj.NGAYKS.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYKS.Value) : "");

            txtSOTRUKH.Text = obj.SOTRUPO != null ? obj.SOTRUPO.ToString() : "";

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
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string bb = loginInfo.Username;
            var kvpo = _kvpoDao.GetPo(_nvDao.Get(bb).MAKV);

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
            var list = _ddkpoDao.GetListPB(txtFilter.Text.Trim(), pb.MAPB.ToString(), kvpo.MAKVPO);

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
                // Update page index
                gvDDK.PageIndex = e.NewPageIndex;
                // Bind data for grid
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
                        var obj = _ddkpoDao.Get(id);
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
            if (_nvDao.Get(LoginInfo.MANV).MAKV == "S" || _nvDao.Get(LoginInfo.MANV).MAKV == "P")
            {
                var list = _nvDao.SearchKVPB(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
            else
            {
                var list = _nvDao.SearchKV3(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
                gvNhanVien.DataSource = list;
                gvNhanVien.PagerInforText = list.Count.ToString();
                gvNhanVien.DataBind();
            }
        }

        protected void txtTHECHAP_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnTUCHOI_Click(object sender, EventArgs e)
        {
            try
            {
                var thietke = ThietKePo;
                if (thietke == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;

                // Authenticate
                if (!HasPermission(Functions.TK_ThietKeVaVatTuPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (!string.IsNullOrEmpty(txtMADDK.Text.Trim()) || txtMADDK.Text != "")
                {
                    var objs = _ddkpoDao.Get(txtMADDK.Text.Trim());

                    try { objs.NGAYKS = Convert.ToDateTime(txtCHUTHICH.Text); }
                    catch { }

                    objs.TTTK = TTTK.TK_RA.ToString();
                    objs.NOIDUNG = txtCHUTHICH.Text.Trim();

                    if (!HasPermission(Functions.TK_ThietKeVaVatTuPo, Permission.Insert))
                    {
                        CloseWaitingDialog();
                        ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }
                    var msg2 = _tkpoDao.Insert(thietke, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                    
                    _rpClass.UPKHCOBIEN(txtMADDK.Text.Trim(), _kvpoDao.GetPo(makv).MAKVPO, 0, 0, "TK_RA", txtCHUTHICH.Text.Trim(), "UPTUCHOITKPO");

                    _rpClass.HisNgayDangKyBienPo(thietke.MADDKPO, LoginInfo.MANV, _kvpoDao.GetPo(makv).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                       "", "", "", "", "INTHIETKE");

                    _rpClass.HisNgayDangKyBienPo(thietke.MADDKPO, LoginInfo.MANV, _kvpoDao.GetPo(makv).MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOITK");

                    //TODO: check relation before update list
                    //var msg = _ddkpoDao.Update(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    if (msg2 != null)
                    {
                        if (msg2.MsgType != MessageType.Error)
                        {
                            ClearContent();

                            CloseWaitingDialog();
                            ShowInfor(ResourceLabel.Get(msg2));
                            // Refresh grid view
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

            //try
            //{
            //    // Authenticate
            //    if (!HasPermission(Functions.TK_ThietKeVaVatTuPo, Permission.Update))
            //    {
            //        CloseWaitingDialog();
            //        ShowError(Resources.Message.WARN_PERMISSION_DENIED);
            //        return;
            //    }

            //    // Get list of ids that to be update
            //    var strIds = Request["listIds"];
            //    //if ((strIds != null) && (!string.Empty.Equals(strIds)))
            //    if (!string.IsNullOrEmpty(txtMADDK.Text.Trim()) || txtMADDK.Text != "")
            //    {
            //        //var objs = new List<DONDANGKY>();
            //        var objs = _ddkpoDao.GetListMa(txtMADDK.Text.Trim());
            //        var listIds = strIds.Split(',');

            //        //Add ma vao danh sách cần delete
            //        objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

            //        foreach (var obj in objs)
            //        {
            //            // ReSharper disable EmptyGeneralCatchClause
            //            try { obj.NGAYKS = Convert.ToDateTime(txtCHUTHICH.Text); }
            //            catch { }
            //            // ReSharper restore EmptyGeneralCatchClause

            //            obj.TTTK = TTTK.TK_RA.ToString();

            //            obj.NOIDUNG = txtCHUTHICH.Text.Trim();
            //        }

            //        //TODO: check relation before update list
            //        var msg = _ddkpoDao.UpdateListTuChoi(objs, CommonFunc.GetComputerName(),
            //                                  CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

            //        if (msg != null)
            //        {
            //            if (msg.MsgType != MessageType.Error)
            //            {
            //                CloseWaitingDialog();

            //                ShowInfor(ResourceLabel.Get(msg));

            //                // Refresh grid view
            //                BindDataForGrid();
            //            }
            //            else
            //            {
            //                CloseWaitingDialog();
            //                ShowError(ResourceLabel.Get(msg));
            //            }
            //        }
            //        else
            //        {
            //            CloseWaitingDialog();
            //            ShowError("Từ chối đơn khảo sát thiết kế không thành công.");
            //        }
            //    }
            //    else
            //    {
            //        CloseWaitingDialog();
            //        ShowError("Vui lòng chọn đơn cần từ chối.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            //}
        }

    }
}