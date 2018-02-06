using System;
using System.Collections.Generic;
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
    public partial class LoaiDongHo : Authentication
    {
        private readonly LoaiDongHoDao _objDao = new LoaiDongHoDao();


        private LOAIDH LoaiDHObj
        {
            get
            {
                if (!ValidateData())
                    return null;

                var ldh = new LOAIDH
                {
                    MALDH = txtMaLoai.Text.Trim(),
                    NSX = txtNSX.Text.Trim(),
                    MOTANSX = txtMOTANSX.Text.Trim(),
                    KICHCO = txtKICHCO.Text.Trim(),
                    MOTAKC = txtMOTAKC.Text.Trim(),
                    KDH = txtKDH.Text.Trim(),
                    MOTAKDH = txtMoTaKDH.Text.Trim()
                };

                if (!string.IsNullOrEmpty(txtGIA.Text.Trim()))
                    ldh.GIA = decimal.Parse(txtGIA.Text.Trim());

                if (!string.IsNullOrEmpty(txtGIAVAT.Text.Trim()))
                    ldh.GIAVAT = decimal.Parse(txtGIAVAT.Text.Trim());

                if (!string.IsNullOrEmpty(txtCHISOMAX.Text.Trim()))
                    ldh.CHISOMAX = int.Parse(txtCHISOMAX.Text.Trim());

                if (!string.IsNullOrEmpty(txtLUULUONGCT.Text.Trim()))
                    ldh.LUULUONG_CT = int.Parse(txtLUULUONGCT.Text.Trim());

                if (!string.IsNullOrEmpty(txtLUULUONGDN.Text.Trim()))
                    ldh.LUULUONG_DN = int.Parse(txtLUULUONGDN.Text.Trim());

                if (!string.IsNullOrEmpty(txtLUULUONGNN.Text.Trim()))
                    ldh.LUULUONG_NN = int.Parse(txtLUULUONGNN.Text.Trim());

                return ldh;
            }
        }

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



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.DM_LoaiDongHo, Permission.Read);
                PrepareUI();

                if (!Page.IsPostBack)
                {
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
            Page.Title = Resources.Message.TITLE_DM_LOAIDONGHO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_DANHMUC;
                header.TitlePage = Resources.Message.PAGE_DM_LOAIDONGHO;
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
            var objList = _objDao.GetList();

            gvList.DataSource = objList;
            gvList.PagerInforText = objList.Count.ToString();
            gvList.DataBind();
        }
        
        public bool ValidateData()
        {
            if (string.Empty.Equals(txtMaLoai.Text))
            {
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Mã loại"), txtMaLoai.ClientID);
                return false;
            }

            if (!string.IsNullOrEmpty(txtGIA.Text.Trim()))
            {
                try { decimal.Parse(txtGIA.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá"), txtGIA.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtGIAVAT.Text.Trim()))
            {
                try { decimal.Parse(txtGIAVAT.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Giá đã tính thuế"), txtGIAVAT.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtCHISOMAX.Text.Trim()))
            {
                try { int.Parse(txtCHISOMAX.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Chỉ số tối đa"), txtCHISOMAX.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtLUULUONGCT.Text.Trim()))
            {
                try { int.Parse(txtLUULUONGCT.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Lưu lượng CT"), txtLUULUONGCT.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtLUULUONGDN.Text.Trim()))
            {
                try { int.Parse(txtLUULUONGDN.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Lưu lượng DN"), txtLUULUONGDN.ClientID);
                    return false;
                }
            }

            if (!string.IsNullOrEmpty(txtLUULUONGNN.Text.Trim()))
            {
                try { int.Parse(txtLUULUONGNN.Text.Trim()); }
                catch
                {
                    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Lưu lượng NN"), txtLUULUONGNN.ClientID);
                    return false;
                }
            }

            return true;
        }

        private void ClearForm()
        {
            txtMaLoai.Text = "";
            txtMaLoai.ReadOnly = false;
            txtNSX.Text = "";
            txtMOTANSX.Text = "";
            txtKICHCO.Text = "";
            txtMOTAKC.Text = "";
            txtKDH.Text = "";
            txtMoTaKDH.Text = "";
            txtGIA.Text = "";
            txtGIAVAT.Text = "";
            txtCHISOMAX.Text = "";
            txtLUULUONGDN.Text = "";
            txtLUULUONGCT.Text = "";
            txtLUULUONGNN.Text = "";

            txtMaLoai.Focus();

            UpdateMode = Mode.Create;
        }

        private void DeleteList()
        {
            try
            {
                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<LOAIDH>();
                    var listIds = strIds.Split(',');

                    // Kiem tra relation ship truoc khi delete
                    foreach (var ma in listIds)
                    {
                        if (_objDao.IsInUse(ma))
                        {
                            var msgIsInUse = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Info, "loại đồng hồ với mã", ma);

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




        private void BindItem(LOAIDH obj)
        {
            if (obj == null)
                return;

            SetControlValue(txtMaLoai.ClientID, obj.MALDH);
            SetReadonly(txtMaLoai.ClientID, true);

            SetControlValue(txtNSX.ClientID, obj.NSX);
            SetControlValue(txtMOTANSX.ClientID, obj.MOTANSX);
            SetControlValue(txtKICHCO.ClientID, obj.KICHCO);
            SetControlValue(txtMOTAKC.ClientID, obj.MOTAKC);
            SetControlValue(txtKDH.ClientID, obj.KDH);
            SetControlValue(txtMoTaKDH.ClientID, obj.MOTAKDH);
            SetControlValue(txtGIA.ClientID, obj.GIA.HasValue ? obj.GIA.Value.ToString() : "");
            SetControlValue(txtGIAVAT.ClientID, obj.GIAVAT.HasValue ? obj.GIAVAT.Value.ToString() : "");
            SetControlValue(txtCHISOMAX.ClientID, obj.CHISOMAX.HasValue ? obj.CHISOMAX.Value.ToString() : "");
            SetControlValue(txtLUULUONGDN.ClientID, obj.LUULUONG_DN.HasValue ? obj.LUULUONG_DN.Value.ToString() : "");
            SetControlValue(txtLUULUONGCT.ClientID, obj.LUULUONG_CT.HasValue ? obj.LUULUONG_CT.Value.ToString() : "");
            SetControlValue(txtLUULUONGNN.ClientID, obj.LUULUONG_NN.HasValue ? obj.LUULUONG_NN.Value.ToString() : "");

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
                var ma = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var objDb = _objDao.Get(ma);
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

        
        
        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            var info = LoaiDHObj;
            if (info == null)
            {
                CloseWaitingDialog();
                return;
            }

            Message msg;

            // insert new
            if (UpdateMode == Mode.Create)
            {
                if (!HasPermission(Functions.DM_LoaiDongHo, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var tontai = _objDao.Get(txtMaLoai.Text.Trim());
                if (tontai != null)
                {
                    CloseWaitingDialog();
                    ShowError("Mã loại đồng hồ đã tồn tại", txtMaLoai.ClientID);
                    return;
                }

                msg = _objDao.Insert(info);
            }
            // update
            else
            {
                if (!HasPermission(Functions.DM_LoaiDongHo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }
                
                msg = _objDao.Update(info);
            }

            CloseWaitingDialog();

            if (msg == null) return;

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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.DM_LoaiDongHo, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DeleteList();

                ClearForm();
                BindDataForGrid();
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
            ClearForm();
        }
    }
}