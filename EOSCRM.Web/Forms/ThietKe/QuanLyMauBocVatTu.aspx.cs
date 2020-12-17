using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Controls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class QuanLyMauBocVatTu : Authentication
    {
        private readonly KhoDanhMucDao _kdmDao = new KhoDanhMucDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly MauBocVatTuDao ddkDao = new MauBocVatTuDao();
        private readonly ChiTietMauBocVatTuDao ctmbvtDao = new ChiTietMauBocVatTuDao();
        private readonly DaoLapMauBocVatTuDao dlmbvtDao = new DaoLapMauBocVatTuDao();
        private readonly GhiChuMauBocVatTuDao gcmbvtDao = new GhiChuMauBocVatTuDao();
        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly DvtDao dvtDao = new DvtDao();

        #region Properties
        /*private MAUBOCVATTU MauBocVatTu
        {
            get
            {
                if (!IsDataValid())
                    return null;

                var mbvt = new MAUBOCVATTU
                {
                    MADDK = txtMADDK.Text.Trim(),
                    TENTK = txtTENTK.Text.Trim(),
                    CHUTHICH = "",
                    FILETK = "",
                    FILETK_HC = "",
                    NGAYGUI_CT = null,
                    NGAYNHAN_CT = null,
                    NGAYTK = DateTime.Now,
                    SOBCT = null
                };

                return mbvt;
            }
        }
         */

        protected MAUBOCVATTU SelectedMBVT
        {
            get
            {
                try
                {
                    return (MAUBOCVATTU)Session["QLMBVT_SELECTED"];
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                Session["QLMBVT_SELECTED"] = value;
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

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
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

        private void FocusAndSelect(string controlId)
        {
            ((EOS)Page.Master).FocusAndSelect(controlId);
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_QuanLyMauBocVatTu, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadDataToForm();
                    BindDataForGrid();
                    tblMBVT.Visible = false;
                    UpdateMode = Mode.Create;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_QUANLYMAUBOCVATTU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_QUANLYMAUBOCVATTU;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvVatTu);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu);
            CommonFunc.SetPropertiesForGrid(gvGhiChu);
            CommonFunc.SetPropertiesForGrid(gvChiPhi);
        }

        private void LoadDataToForm()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            timkv();

            var nhanvien = _nvDao.Get(b);
            if (nhanvien.MAKV == "X")
            {
                var khoxn = _kdmDao.GetListXiNghiepLoaiVatTu("X", "NN");
                ddlKhoXiNghiep.Items.Clear();
                ddlKhoXiNghiep.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kho in khoxn)
                {
                    ddlKhoXiNghiep.Items.Add(new ListItem(kho.TenKho, kho.Id));
                }
            }
            else
            {
                var khoxn = _kdmDao.GetListXiNghiepLoaiVatTu("XN", "NN");
                ddlKhoXiNghiep.Items.Clear();
                ddlKhoXiNghiep.Items.Add(new ListItem("Tất cả", "%"));
                foreach (var kho in khoxn)
                {
                    ddlKhoXiNghiep.Items.Add(new ListItem(kho.TenKho, kho.Id));
                }
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
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    //ddlKHUVUC1.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }                   
                }
            }
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (_nvDao.Get(b).MAKV == "X" )
                {
                    var objList = ddkDao.GetListMAKV(ddlKHUVUC.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else if (_nvDao.Get(b).MAKV == "S") // Châu Đốc
                {
                    var objList = ddkDao.GetListNNBravo();

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    //var objList = ddkDao.GetListNNBravo();
                    var objList = ddkDao.GetListMAKV(ddlKHUVUC.SelectedValue);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
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
                MsgBox.Show(String.Format(Resources.Message.E_INVALID_DATA, "Mã mẫu bốc vật tư"));
                txtMADDK.Focus();
                return false;
            }

            var existed = ddkDao.Get(txtMADDK.Text.Trim());
            if (existed != null)
            {
                MsgBox.Show("Mã mẫu bốc vật tư đã tồn tại");
                txtMADDK.Focus();
                return false;
            }

            if (string.Empty.Equals(txtTENTK.Text.Trim()))
            {
                MsgBox.Show(String.Format(Resources.Message.E_INVALID_DATA, "Tên mẫu bốc vật tư"));
                txtTENTK.Focus();
                return false;
            }

            return true;
        }

        private void ClearContent()
        {
            txtMADDK.Text = "";
            txtTENTK.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {           

            var MauBocVatTu = new MAUBOCVATTU
            {
                MADDK = txtMADDK.Text.Trim(),
                TENTK = txtTENTK.Text.Trim(),
                CHUTHICH = "",
                FILETK = "",
                FILETK_HC = "",
                NGAYGUI_CT = null,
                NGAYNHAN_CT = null,
                NGAYTK = DateTime.Now,
                SOBCT = null,
                LOAIMBVT="NN",
                MAKV = ddlKHUVUC.SelectedValue,
                MauCuaAi = "KT"
            };

            var don = MauBocVatTu;
            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }
            Message msg;

            if (UpdateMode.Equals(Mode.Create))
            {
                if (!IsDataValid())
                    return;

                if (!HasPermission(Functions.TK_QuanLyMauBocVatTu, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // insert
                msg = ddkDao.Insert(don);
            }
            else
            {
                if (!HasPermission(Functions.TK_QuanLyMauBocVatTu, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                msg = ddkDao.Update(don);
            }

            CloseWaitingDialog();

            if (!msg.MsgType.Equals(MessageType.Error))
            {
                ShowInfor(ResourceLabel.Get(msg));

                //Trả lại màn hình trống ban đầu
                ClearContent();

                // Refresh grid view
                BindDataForGrid();

                upnlGrid.Update();
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_QuanLyMauBocVatTu, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<MAUBOCVATTU>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    var msg = ddkDao.DeleteList(objs);

                    CloseWaitingDialog();

                    if ((msg != null) && (msg.MsgType != MessageType.Error))
                    {
                        ShowInfor(ResourceLabel.Get(msg));

                        // Refresh grid view
                        BindDataForGrid();

                        upnlGrid.Update();
                    }
                    else
                    {
                        // Show message
                        ShowError(ResourceLabel.Get(msg));
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindMBVTToForm(MAUBOCVATTU obj)
        {
            txtMADDK.Text = obj.MADDK;
            txtTENTK.Text = obj.TENTK;

            upnlInfor.Update();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        // Authenticate
                        if (!HasPermission(Functions.TK_QuanLyMauBocVatTu, Permission.Update))
                        {
                            CloseWaitingDialog();
                            ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                            return;
                        }

                        if (!string.Empty.Equals(id))
                        {
                            var mbvt = ddkDao.Get(id);
                            if (mbvt == null) return;

                            UpdateMode = Mode.Update;
                            BindMBVTToForm(mbvt);
                        }

                        CloseWaitingDialog();

                        break;

                    case "BocVatTu":
                        // Authenticate
                        if (!HasPermission(Functions.TK_QuanLyMauBocVatTu, Permission.Update))
                        {
                            CloseWaitingDialog();
                            ShowInfor(Resources.Message.WARN_PERMISSION_DENIED);
                            return;
                        }

                        if (!string.Empty.Equals(id))
                        {
                            var mbvt = ddkDao.Get(id);
                            if (mbvt == null)
                            {
                                CloseWaitingDialog();
                                return;
                            }

                            SelectedMBVT = mbvt;
                            CloseWaitingDialog();

                            BindSelectedVatTuGrid();
                            BindChiPhi();
                            BindGhiChu();
                            tblMBVT.Visible = true;
                            upnlMBVT.Update();
                        }
                        else
                        {
                            CloseWaitingDialog();
                        }

                        break;

                    case "DeleteItem":
                        // Authenticate
                        Authenticate(Functions.TK_QuanLyMauBocVatTu, Permission.Delete);

                        if (!string.Empty.Equals(id))
                        {
                            var mbvt = ddkDao.Get(id);
                            if (mbvt == null) return;

                            var msg = ddkDao.DeleteList(new List<MAUBOCVATTU> { mbvt });

                            if ((msg != null) && (msg.MsgType != MessageType.Error))
                            {
                                CloseWaitingDialog();

                                ShowInfor(ResourceLabel.Get(msg));

                                // Refresh grid view
                                BindDataForGrid();

                                upnlGrid.Update();
                            }
                            else
                            {
                                CloseWaitingDialog();

                                ShowError(ResourceLabel.Get(msg));
                            }
                        }
                        else
                        {
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

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");

            var btnBocVatTu = e.Row.FindControl("btnBocVatTu") as LinkButton;
            if (btnBocVatTu == null) return;
            btnBocVatTu.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(btnBocVatTu) + "')");

            var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            if (btnDeleteItem == null) return;
            btnDeleteItem.Attributes.Add("onclick", "onClientClickGridDelete('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }

        private void BindVatTu()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var khuvuc = _nvDao.Get(b).MAKV;

            //if (khuvuc == "X" || khuvuc == "S")
            if (khuvuc == "X")
            {
                var list = vtDao.SearchMAKVAll(txtFilterVatTu.Text.Trim(), khuvuc);

                gvVatTu.DataSource = list;
                gvVatTu.PagerInforText = list.Count.ToString();
                gvVatTu.DataBind();
            }
            else
            {
                //var list = vtDao.Search(txtFilterVatTu.Text.Trim());
                var list = vtDao.SearchMaSoKeToan(ddlKhoXiNghiep.SelectedValue, txtFilterVatTu.Text.Trim());
                
                gvVatTu.DataSource = list;
                gvVatTu.PagerInforText = list.Count.ToString();
                gvVatTu.DataBind();
            }
        }

        protected void btnBrowseVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            upnlVatTu.Update();
            UnblockDialog("divVatTu");
        }

        protected void gvVatTu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }

        protected void gvVatTu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvVatTu.PageIndex = e.NewPageIndex;             
                BindVatTu();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvVatTu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        CloseWaitingDialog();

                        var vt = vtDao.Get(id);

                        if (vt != null)
                        {
                            SetControlValue(txtMAVT.ClientID, id);
                            SetLabel(lblTENVT.ClientID, vt.TENVT);
                            FocusAndSelect(txtKHOILUONG.ClientID);
                        }

                        HideDialog("divVatTu");

                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnFilterVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            CloseWaitingDialog();
        }

        private void BindSelectedVatTuGrid()
        {
            var mbvt = SelectedMBVT;

            if (mbvt == null) return;
            var list = ctmbvtDao.GetListBravo(mbvt.MADDK);

            gvSelectedVatTu.DataSource = list;
            gvSelectedVatTu.PagerInforText = list.Count.ToString();
            gvSelectedVatTu.DataBind();
        }

        protected void gvSelectedVatTu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtSOLUONG") as TextBox;
            var txtGIAVT = e.Row.FindControl("txtGIAVT") as TextBox;
            var lblTIENVT = e.Row.FindControl("lblTIENVT") as Label;
            var txtGIANC = e.Row.FindControl("txtGIANC") as TextBox;
            var lblTIENNC = e.Row.FindControl("lblTIENNC") as Label;
            var cbISCTYDTU = e.Row.FindControl("cbISCTYDTU") as CheckBox;

            if (txtSL == null || txtGIAVT == null || txtGIANC == null ||
                lblTIENNC == null || lblTIENVT == null || cbISCTYDTU == null) return;

            var source = gvSelectedVatTu.DataSource as List<CTMAUBOCVATTU>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MAVT;
            var mambvt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MADDK;

            cbISCTYDTU.Checked = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].ISCTYDTU.HasValue && source[e.Row.RowIndex].ISCTYDTU.Value;

            var script = "javascript:updateCTMBVT(\"" + mambvt + "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\", \"" + cbISCTYDTU.ClientID +
                                                        "\")";
            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);
            cbISCTYDTU.Attributes.Add("onchange", script);
        }

        protected void gvSelectedVatTu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
                case "DeleteVatTu":
                    var mbvt = SelectedMBVT;
                    if (mbvt == null) return;

                    var deletingCTMBVT = ctmbvtDao.Get(mbvt.MADDK, mavt);
                    if (deletingCTMBVT == null) return;

                    ctmbvtDao.Delete(deletingCTMBVT);

                    BindSelectedVatTuGrid();

                    //CloseWaitingDialog();

                    break;
            }
        }

        /*
        protected void btnAddVatTu_Click(object sender, EventArgs e)
        {
            if (txtMAVT.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập mã vật tư", txtMAVT.ClientID);
                return;
            }

            var vt = vtDao.Get(txtMAVT.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.");
                return;
            }

            var mbvt = SelectedMBVT;
            if (mbvt == null)
            {
                CloseWaitingDialog();
                return;
            }

            // add to grid
            var ctmbvt = new CTMAUBOCVATTU
            {
                MADDK = mbvt.MADDK,
                MAVT = vt.MAVT,
                NOIDUNG = vt.TENVT,
                SOLUONG = 1,
                ISCTYDTU = chkIsCtyDauTu.Checked
            };

            txtMAVT.Text = "";
            ctmbvtDao.Insert(ctmbvt);
            BindSelectedVatTuGrid();

            txtMAVT.Focus();

            CloseWaitingDialog();
        }
        */



        private void BindGhiChu()
        {
            var mbvt = SelectedMBVT;

            if (mbvt == null) return;
            var list = gcmbvtDao.GetList(mbvt.MADDK);

            gvGhiChu.DataSource = list;
            gvGhiChu.PagerInforText = list.Count.ToString();
            gvGhiChu.DataBind();
        }

        protected void gvGhiChu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtNOIDUNG") as TextBox;
            if (txtSL == null) return;

            var source = gvGhiChu.DataSource as List<GCMAUBOCVATTU>;
            if (source == null) return;

            var magc = source[e.Row.RowIndex + gvGhiChu.PageSize * gvGhiChu.PageIndex].MAGHICHU;

            var script = "javascript:updateGCMBVT(\"" + magc + "\", \"" + txtSL.ClientID + "\")";
            txtSL.Attributes.Add("onblur", script);

            var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            if (btnDeleteItem == null) return;
            btnDeleteItem.Attributes.Add("onclick", "onClientClickGridDelete('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }

        protected void gvGhiChu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var magc = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteGhiChu":
                    var deletingGCMBVT = gcmbvtDao.Get(Int32.Parse(magc));
                    if (deletingGCMBVT == null) return;

                    gcmbvtDao.Delete(deletingGCMBVT);

                    BindGhiChu();

                    CloseWaitingDialog();

                    break;
            }
        }

        protected void btnAddGhiChu_Click(object sender, EventArgs e)
        {
            try
            {
                var mbvt = SelectedMBVT;

                if (mbvt == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var gcmbvt = new GCMAUBOCVATTU
                {
                    MAMBVT = mbvt.MADDK,
                    NOIDUNG = ""
                };

                gcmbvtDao.Insert(gcmbvt);
                BindGhiChu();

                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindChiPhi()
        {
            var mbvt = SelectedMBVT;

            if (mbvt == null) return;
            var list = dlmbvtDao.GetList(mbvt.MADDK);

            gvChiPhi.DataSource = list;
            gvChiPhi.PagerInforText = list.Count.ToString();
            gvChiPhi.DataBind();
        }

        protected void gvChiPhi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var macp = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteChiPhi":
                    var deletingCPMBVT = dlmbvtDao.Get(Int32.Parse(macp));
                    if (deletingCPMBVT == null) return;

                    dlmbvtDao.Delete(deletingCPMBVT);

                    BindChiPhi();

                    //CloseWaitingDialog();

                    break;
            }
        }

        protected void gvChiPhi_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtND = e.Row.FindControl("txtNOIDUNG") as TextBox;
            var txtDG = e.Row.FindControl("txtDONGIA") as TextBox;
            var ddlDVT = e.Row.FindControl("ddlDVT") as DropDownList;
            var txtSL = e.Row.FindControl("txtSOLUONG") as TextBox;
            
            var txtCHIEURONG = e.Row.FindControl("txtCHIEURONG") as TextBox;
            var txtCHIEUCAO = e.Row.FindControl("txtCHIEUCAO") as TextBox;

            var txtHS = e.Row.FindControl("txtHESOCP") as TextBox;
            var lblTHANHTIENCP = e.Row.FindControl("lblTHANHTIENCP") as Label;
            var ddlLCP = e.Row.FindControl("ddlLOAICP") as DropDownList;

            if (txtND == null || txtDG == null || ddlDVT == null || txtSL == null ||
                txtCHIEURONG == null || txtCHIEUCAO == null ||
                txtHS == null || lblTHANHTIENCP == null || ddlLCP == null) return;

            var source = gvChiPhi.DataSource as List<DAOLAPMAUBOCVATTU>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi.PageSize * gvChiPhi.PageIndex].MADON;


            var script = "javascript:updateCPMBVT(\"" + madon + "\", \"" + txtND.ClientID +
                                                                "\", \"" + txtDG.ClientID +
                                                                "\", \"" + ddlDVT.ClientID +
                                                                "\", \"" + txtSL.ClientID +
                                                                "\", \"" + txtCHIEURONG.ClientID +
                                                                "\", \"" + txtCHIEUCAO.ClientID +
                                                                "\", \"" + txtHS.ClientID +
                                                                "\", \"" + lblTHANHTIENCP.ClientID +
                                                                "\", \"" + ddlLCP.ClientID +
                                                                "\")";
            txtND.Attributes.Add("onblur", script);
            txtDG.Attributes.Add("onblur", script);
            txtSL.Attributes.Add("onblur", script);
            txtCHIEURONG.Attributes.Add("onblur", script);
            txtCHIEUCAO.Attributes.Add("onblur", script);
            txtHS.Attributes.Add("onblur", script);

            ddlDVT.Attributes.Add("onchange", script);
            ddlLCP.Attributes.Add("onchange", script);

            lblTHANHTIENCP.Text = String.Format(CultureInfo.CreateSpecificCulture("vi-VN"), "{0:#,##}", source[e.Row.RowIndex + gvChiPhi.PageSize * gvChiPhi.PageIndex].THANHTIENCP);

            //var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            //if (btnDeleteItem == null) return;
            //btnDeleteItem.Attributes.Add("onclick", "onClientClickGridDelete('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }

        protected void btnAddChiPhi_Click(object sender, EventArgs e)
        {
            try
            {
                var mbvt = SelectedMBVT;

                if (mbvt == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var dvtList = dvtDao.GetList();
                if (dvtList.Count == 0)
                {
                    CloseWaitingDialog();
                    ShowError("Không lấy được danh sách đơn vị tính từ cơ sở dữ liệu.");
                    return;
                }

                var dlmbvt = new DAOLAPMAUBOCVATTU
                {
                    MAMAUBOCVATTU = mbvt.MADDK,
                    NOIDUNG = "",
                    DONGIACP = 0,
                    SOLUONG = 1,
                    DVT = dvtList[0].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 0,
                    LOAICP = "DAO",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP",
                    CHIEUCAO = Convert.ToDecimal("0.2"),
                    CHIEURONG = Convert.ToDecimal("0.2")
                };

                dlmbvtDao.Insert(dlmbvt);
                BindChiPhi();

                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkBtnChangeKhoiLuong_Click(object sender, EventArgs e)
        {
            if (txtMAVT.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập mã vật tư", txtMAVT.ClientID);
                return;
            }

            var vt = vtDao.Get(txtMAVT.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.", txtMAVT.ClientID);
                return;
            }

            try
            {
                decimal.Parse(txtKHOILUONG.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khối lượng"), txtKHOILUONG.ClientID);
                return;
            }

            var mbvt = SelectedMBVT;
            if (mbvt == null)
            {
                CloseWaitingDialog();
                return;
            }

            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            if (_nvDao.Get(b).MAKV == "X")
            {
                // add to grid
                var ctmbvt = new CTMAUBOCVATTU
                {
                    MADDK = mbvt.MADDK,
                    MAVT = vt.MAVT,
                    NOIDUNG = vt.TENVT,
                    SOLUONG = decimal.Parse(txtKHOILUONG.Text.Trim()),
                    GIAVT = vt.GIAVT.HasValue ? vt.GIAVT.Value : 0,
                    GIANC = vt.GIANC.HasValue ? vt.GIANC.Value : 0,
                    TIENVT = vt.GIAVT.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIAVT.Value : 0,
                    TIENNC = vt.GIANC.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIANC.Value : 0,
                    ISCTYDTU = true
                };
                ctmbvtDao.Insert(ctmbvt);
            }
            else
            {
                // add to grid
                var ctmbvt = new CTMAUBOCVATTU
                {
                    MADDK = mbvt.MADDK,
                    MAVT = vt.MAVT,
                    NOIDUNG = vt.TENVT,
                    SOLUONG = decimal.Parse(txtKHOILUONG.Text.Trim()),
                    GIAVT = vt.GIAVT.HasValue ? vt.GIAVT.Value : 0,
                    GIANC = vt.GIANC.HasValue ? vt.GIANC.Value : 0,
                    TIENVT = vt.GIAVT.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIAVT.Value : 0,
                    TIENNC = vt.GIANC.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIANC.Value : 0,
                    ISCTYDTU = chkIsCtyDauTu.Checked
                };
                ctmbvtDao.Insert(ctmbvt);
            }

            BindSelectedVatTuGrid();

            txtMAVT.Text = "";
            txtKHOILUONG.Text = "";
            lblTENVT.Text = "";
            FocusAndSelect(txtMAVT.ClientID);

            CloseWaitingDialog();
            upnlMBVT.Update();
        }

        protected void linkBtnChangeMAVT_Click(object sender, EventArgs e)
        {
            if (txtMAVT.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var vt = vtDao.Get(txtMAVT.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.", txtMAVT.ClientID);
                return;
            }

            lblTENVT.Text = vt.TENVT;
            txtKHOILUONG.Text = "1";
            FocusAndSelect(txtKHOILUONG.ClientID);

            CloseWaitingDialog();
        }

        protected void btCapNhatGiaMoi_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var tungay = DateTime.Now;
                var denngay = DateTime.Now;

                _rpClass.DSQuiTrinhNuocBien(tungay, denngay, _nvDao.Get(b).MAKV, "", "", "", "UPGIATIENNCVT");

                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                var objList = ddkDao.GetListMAKV(ddlKHUVUC.SelectedValue);
                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();

                ClearContent();
                CloseWaitingDialog();
                upnlGrid.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }            
        }

    }
}
