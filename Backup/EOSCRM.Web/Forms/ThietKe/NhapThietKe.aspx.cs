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

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class NhapThietKe : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly KhuVucDao kvDao = new KhuVucDao();
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
                    TENNVTK = txtNV1.Text.Trim()
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



        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_ThietKeVaVatTu, Permission.Read);

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
            //txtNGAYTK.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

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

        private void LoadStaticReferences()
        {
            //TODO: Load các đối tượng có liên quan lên UI
            try
            {
                UpdateMode = Mode.Create;
                var list = kvDao.GetList();

                ddlMaKV.Items.Clear();
                ddlMaKV.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kv in list)
                {
                    ddlMaKV.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                }

                txtNGAYTK.Text = DateTime.Now.ToString("dd/MM/yyyy");
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
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

                if (pb.MAPB == "NB" || pb.MAPB == "TA" || pb.MAPB == "TD")
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
            cbTHAMGIAONGCAI.Checked = false;
        }

        

        private void BindToInfor(THIETKE obj)
        {
            //SetControlValue(txtMADDK.ClientID, obj.MADDK);
            txtMADDK.Text = obj.MADDK;
            SetControlValue(txtTENKH.ClientID, ddkDao.Get(obj.MADDK).TENKH);
            //SetControlValue(txtTENTK.ClientID, obj.TENTK);
            txtTENTK.Text = obj.TENTK;
            SetControlValue(txtNGAYKS.ClientID, obj.DONDANGKY.NGAYKS.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.DONDANGKY.NGAYKS.Value) : "");
            txtNGAYTK.Text = obj.NGAYLTK.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYLTK.Value) : "";
            SetControlValue(txtTHECHAP.ClientID, obj.THECHAP.HasValue ? obj.THECHAP.Value.ToString() : "");

            cbTHAMGIAONGCAI.Checked = obj.THAMGIAONGCAI.HasValue && obj.THAMGIAONGCAI.Value;

            lbNV1.Text = obj.MANVTK.ToString();
            txtNV1.Text = obj.TENNVTK.ToString();

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
                        Session["NHAPTHIETKE_MADDK"] = id;
                        var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx";
                        Response.Redirect(url, false);
                        break;

                    case "editTK":
                        var obj = tkDao.Get(id);
                        if (obj == null) return;
                            
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
                // Update page index
                gvThietKe.PageIndex = e.NewPageIndex;

                // Bind data for grid
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

            Message msg;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!HasPermission(Functions.TK_ThietKeVaVatTu, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = tkDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
            }
            else
            {
                if (!HasPermission(Functions.TK_ThietKeVaVatTu, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = tkDao.Update(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
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




        private void BindToInfor(DONDANGKY obj)
        {
            //SetControlValue(txtMADDK.ClientID, obj.MADDK);
            txtMADDK.Text = obj.MADDK;
            SetControlValue(txtTENKH.ClientID, ddkDao.Get(obj.MADDK).TENKH);
            //SetControlValue(txtTENTK.ClientID, obj.TENKH);
            txtTENTK.Text = "Lắp mới";
            SetControlValue(txtNGAYKS.ClientID, obj.NGAYKS.HasValue ? String.Format("{0:dd/MM/yyyy}", obj.NGAYKS.Value) : "");

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
            try { tungay = DateTimeUtil.GetVietNamDate(txtTuNgay.Text.Trim()); } catch { txtTuNgay.Text = ""; }
            try { denngay = DateTimeUtil.GetVietNamDate(txtDenNgay.Text.Trim()); } catch { txtDenNgay.Text = ""; }

            //phong ban
            //var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            //if (loginInfo == null) return;
            string b = LoginInfo.MANV;
            var pb = _nvDao.GetKV(b);

            //var list = ddkDao.GetList(txtFilter.Text.Trim(), tungay, denngay, ddlMaKV.SelectedValue, LoginInfo.MANV);
            var list = ddkDao.GetListPB(txtFilter.Text.Trim(), pb.MAPB.ToString());
            
            
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
            var list = _nvDao.SearchKV3(txtKeywordNV.Text.Trim(), _nvDao.Get(LoginInfo.MANV).MAKV, _nvDao.Get(LoginInfo.MANV).MAPB);
            gvNhanVien.DataSource = list;
            gvNhanVien.PagerInforText = list.Count.ToString();
            gvNhanVien.DataBind();
        }


    }  
}
