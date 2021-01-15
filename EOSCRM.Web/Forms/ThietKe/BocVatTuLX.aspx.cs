using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;

using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class BocVatTuLX : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly MauBocVatTuDao mbvtDao = new MauBocVatTuDao();
        private readonly DaoLapThietKeDao dltkDao = new DaoLapThietKeDao();
        private readonly ChiTietThietKeDao cttkdao = new ChiTietThietKeDao();
        private readonly GhiChuThietKeDao gctkdao = new GhiChuThietKeDao();
        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly DvtDao dvtDao = new DvtDao();
        private readonly DonDangKyDao _ddk = new DonDangKyDao();
        private readonly MauThietKeDao _mtkDao = new MauThietKeDao();
        private readonly ReportClass _rpC = new ReportClass();
        private readonly GiaiQuyetThongTinSuaChuaDao _scDao = new GiaiQuyetThongTinSuaChuaDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();

        string makhuvuc, khuvuc, filenametk1, filenametk2, duongdantk1, duongdantk2;      

        protected THIETKE ThietKe
        {
            get
            {
                if (Session["NHAPTHIETKE_MADDK"] == null)
                    return null;
                return tkDao.Get(Session["NHAPTHIETKE_MADDK"].ToString());
            }
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

        private void ShowInFor(string message)
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
                Authenticate(Functions.TK_ThietKeVaVatTuLX, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();
                if (!Page.IsPostBack)
                {
                    LoadReferences();
                    BindSelectedVatTuGrid();
                    BindSelectedVatTuKHTT();
                    BindSelectedVatTuKHTTSAUDH();
                    BindChiPhiDLVC();

                    TongTienTK();
                }
                else
                {
                    //var dt = (DataTable)Session[SessionKey.TK_BAOCAO_BOCVATTULX];
                    //Report(dt);
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
                header.TitlePage = Resources.Message.PAGE_TK_BOCVATTU;
            }
            CommonFunc.SetPropertiesForGrid(gvVatTu);
            CommonFunc.SetPropertiesForGrid(gvVatTuKHTT);
            CommonFunc.SetPropertiesForGrid(gvVatTuKHTTSAUDH);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTuKHTT);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTuKHTTSAUDH);
            CommonFunc.SetPropertiesForGrid(gvChiPhiDLVC);
        }

        private void LoadReferences()
        {   
            if (ThietKe == null)
                return;

            LoadMaKhuVuc();

            var madon2 = _ddk.Get(ThietKe.MADDK.ToString());
            if (madon2 != null)
            {
                var list = mbvtDao.GetListMAKV(makhuvuc);
                ddlMBVT.Items.Clear();
                ddlMBVT.Items.Add(new ListItem("", ""));

                lbTENKH.Text = madon2.TENKH.ToString();

                var index = -1;
                var i = 0;

                foreach (var item in list)
                {
                    if (item.DUOCCHON.HasValue && item.DUOCCHON.Value)
                        index = i;
                    i++;
                    ddlMBVT.Items.Add(new ListItem(item.TENTK, item.MADDK));
                }

                if (index > -1)
                {
                    ddlMBVT.Items.RemoveAt(0);
                    ddlMBVT.SelectedIndex = index;
                }                

                if (ThietKe.HINHTK1 != null)
                {
                    imgHINHTK1.ImageUrl = ThietKe.HINHTK1;
                }
                else
                {
                    imgHINHTK1.Visible = false;
                }

                if (ThietKe.HINHTK2 != null)
                {
                    imgHINHTK2.ImageUrl = ThietKe.HINHTK2;
                }
                else
                {
                    imgHINHTK2.Visible = false;
                }

                chkIsCtyDauTu.Checked = true;

                LoadLabelTongTien();
            } 
        }

        private void LoadMaKhuVuc()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            makhuvuc = _nvDao.Get(b).MAKV;
        }

        private void LoadLabelTongTien()
        {
            try
            {
                lbTGTXLTT2.Text = "0";
                lbTTGTGT2.Text = "0";
                lbTGTXLST2.Text = "0";

                lbTCTVCTT2.Text = "0";
                lbTTGTGTVC2.Text = "0";
                lbTCPVCST2.Text = "0";
                lbTONCONGSAUTHUE2.Text = "0";
            }
            catch { }
        }

        protected void btnFilterVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            CloseWaitingDialog();
        }

        protected void btnFilterVatTuKHTT_Click(object sender, EventArgs e)
        {
            BindVatTuKHTT();
            CloseWaitingDialog();
        }
        
        protected void btnFilterVatTuKHTTSAUDH_Click(object sender, EventArgs e)
        {
            BindVatTuKHTTSAUDH();
            CloseWaitingDialog();
        }

        private void BindVatTu()
        {
            LoadMaKhuVuc();

            var list = vtDao.SearchMAKVKoKHTT(txtFilterVatTu.Text.Trim(), makhuvuc);
            gvVatTu.DataSource = list;
            gvVatTu.PagerInforText = list.Count.ToString();
            gvVatTu.DataBind();
        }

        #region gvVatTu
        protected void gvVatTu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":                        
                        var vt = vtDao.Get(id);
                        if (vt != null)
                        {
                            txtMAVT.Text = vt.MAVT;
                            SetLabel(lblTENVT.ClientID, vt.TENVT);
                            FocusAndSelect(txtKHOILUONG.ClientID);
                        }
                        HideDialog("divVatTu");
                        CloseWaitingDialog();

                        upnlMBVT.Update();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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

        protected void gvVatTu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;
            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion

        private void BindVatTuKHTT()
        {
            LoadMaKhuVuc();

            var list = vtDao.SearchMAKVOngVuot(txtFilterVatTuKHTT.Text.Trim(), makhuvuc);
            gvVatTuKHTT.DataSource = list;
            gvVatTuKHTT.PagerInforText = list.Count.ToString();
            gvVatTuKHTT.DataBind();
        }

        #region gvVatTuKHTT
        protected void gvVatTuKHTT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var vt = vtDao.Get(id);
                        if (vt != null)
                        {
                            txtMAVTKHTT.Text = vt.MAVT;
                            SetLabel(lblTENVTKHTT.ClientID, vt.TENVT);
                            FocusAndSelect(txtKHOILUONGKHTT.ClientID);
                        }
                        HideDialog("divVatTuKHTT");
                        CloseWaitingDialog();

                        upBVTKHTHANHTOAN.Update();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvVatTuKHTT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVatTuKHTT.PageIndex = e.NewPageIndex;
                BindVatTuKHTT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvVatTuKHTT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;
            var lnkBtnID = e.Row.FindControl("lnkBtnIDKHTT") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion

        private void BindVatTuKHTTSAUDH()
        {
            LoadMaKhuVuc();

            var list = vtDao.SearchMAKVKHTT(txtFilterVatTuKHTTSAUDH.Text.Trim(), makhuvuc);
            gvVatTuKHTTSAUDH.DataSource = list;
            gvVatTuKHTTSAUDH.PagerInforText = list.Count.ToString();
            gvVatTuKHTTSAUDH.DataBind();
        }

        #region gvVatTuKHTTSAUDH
        protected void gvVatTuKHTTSAUDH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        var vt = vtDao.Get(id);
                        if (vt != null)
                        {
                            txtMAVTKHTTSAUDH.Text = vt.MAVT;
                            SetLabel(lblTENVTKHTTSAUDH.ClientID, vt.TENVT);
                            FocusAndSelect(txtKHOILUONGKHTTSAUDH.ClientID);
                        }
                        HideDialog("divVatTuKHTTSAUDH");
                        CloseWaitingDialog();

                        upBVTKHTHANHTOANSAUDH.Update();
                        break;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvVatTuKHTTSAUDH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gvVatTuKHTTSAUDH.PageIndex = e.NewPageIndex;
                BindVatTuKHTTSAUDH();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvVatTuKHTTSAUDH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;
            var lnkBtnID = e.Row.FindControl("lnkBtnIDKHTTSAUDH") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        }
        #endregion

        private void BindSelectedVatTuGrid()
        {
            if (ThietKe == null)
                return;

            var list = cttkdao.GetListCTyDauTu(ThietKe.MADDK);
            gvSelectedVatTu.DataSource = list;
            gvSelectedVatTu.PagerInforText = list.Count.ToString();
            gvSelectedVatTu.DataBind();
        }

        #region gvSelectedVatTu
        protected void gvSelectedVatTu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteVatTu":
                    if (ThietKe == null)
                    {                       
                        return;
                    }

                    var deletingCTTK = cttkdao.Get(ThietKe.MADDK, mavt);
                    if (deletingCTTK == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    cttkdao.Delete(deletingCTTK);

                    BindSelectedVatTuGrid();                  

                    break;
            }
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
                lblTIENNC == null || lblTIENVT == null || cbISCTYDTU == null)
                //|| lblPageTotal == null || lblGrandTotal == null) 
                return;


            var source = gvSelectedVatTu.DataSource as List<CTTHIETKE>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MAVT;
            var mambvt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MADDK;

            cbISCTYDTU.Checked = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].ISCTYDTU.HasValue && source[e.Row.RowIndex].ISCTYDTU.Value;
            var script = "javascript:updateCTTK(\"" + mambvt + "\", \"" + mavt +
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
        #endregion

        private void BindSelectedVatTuKHTT()
        {
            if (ThietKe == null)
                return;

            var list = cttkdao.GetListKHTTVuotOng(ThietKe.MADDK);
            gvSelectedVatTuKHTT.DataSource = list;
            gvSelectedVatTuKHTT.PagerInforText = list.Count.ToString();
            gvSelectedVatTuKHTT.DataBind();
        }

        #region gvSelectedVatTuKHTT
        protected void gvSelectedVatTuKHTT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteVatTu":
                    if (ThietKe == null)
                    {
                        return;
                    }

                    var deletingCTTK = cttkdao.Get(ThietKe.MADDK, mavt);
                    if (deletingCTTK == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    cttkdao.Delete(deletingCTTK);

                    BindSelectedVatTuKHTT();

                    break;
            }
        }

        protected void gvSelectedVatTuKHTT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtSOLUONG") as TextBox;
            var txtGIAVT = e.Row.FindControl("txtGIAVT") as TextBox;
            var lblTIENVT = e.Row.FindControl("lblTIENVT") as Label;
            var txtGIANC = e.Row.FindControl("txtGIANC") as TextBox;
            var lblTIENNC = e.Row.FindControl("lblTIENNC") as Label;
            var gvcbISCTYDTUKHTT = e.Row.FindControl("gvcbISCTYDTUKHTT") as CheckBox;

            if (txtSL == null || txtGIAVT == null || txtGIANC == null ||
                lblTIENNC == null || lblTIENVT == null || gvcbISCTYDTUKHTT == null)
                //|| lblPageTotal == null || lblGrandTotal == null) 
                return;


            var source = gvSelectedVatTuKHTT.DataSource as List<CTTHIETKE>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTuKHTT.PageSize * gvSelectedVatTuKHTT.PageIndex].MAVT;
            var mambvt = source[e.Row.RowIndex + gvSelectedVatTuKHTT.PageSize * gvSelectedVatTuKHTT.PageIndex].MADDK;

            gvcbISCTYDTUKHTT.Checked = source[e.Row.RowIndex + gvSelectedVatTuKHTT.PageSize * gvSelectedVatTuKHTT.PageIndex].ISCTYDTU.HasValue && source[e.Row.RowIndex].ISCTYDTU.Value;
            var script = "javascript:updateCTTKKHTT(\"" + mambvt + "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\", \"" + gvcbISCTYDTUKHTT.ClientID +
                                                        "\")";
            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);
            gvcbISCTYDTUKHTT.Attributes.Add("onchange", script);

        }
        #endregion

        private void BindSelectedVatTuKHTTSAUDH()
        {
            if (ThietKe == null)
                return;

            var list = cttkdao.GetListLoaiVTKHTT(ThietKe.MADDK);
            gvSelectedVatTuKHTTSAUDH.DataSource = list;
            gvSelectedVatTuKHTTSAUDH.PagerInforText = list.Count.ToString();
            gvSelectedVatTuKHTTSAUDH.DataBind();
        }

        #region gvSelectedVatTuKHTTSAUDH
        protected void gvSelectedVatTuKHTTSAUDH_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteVatTu":
                    if (ThietKe == null)
                    {
                        return;
                    }

                    var deletingCTTK = cttkdao.Get(ThietKe.MADDK, mavt);
                    if (deletingCTTK == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    cttkdao.Delete(deletingCTTK);

                    BindSelectedVatTuKHTTSAUDH();

                    break;
            }
        }

        protected void gvSelectedVatTuKHTTSAUDH_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtSOLUONG") as TextBox;
            var txtGIAVT = e.Row.FindControl("txtGIAVT") as TextBox;
            var lblTIENVT = e.Row.FindControl("lblTIENVT") as Label;
            var txtGIANC = e.Row.FindControl("txtGIANC") as TextBox;
            var lblTIENNC = e.Row.FindControl("lblTIENNC") as Label;
            var cbISCTYDTUKHTTSAUDH = e.Row.FindControl("cbISCTYDTUKHTTSAUDH") as CheckBox;

            if (txtSL == null || txtGIAVT == null || txtGIANC == null ||
                lblTIENNC == null || lblTIENVT == null || cbISCTYDTUKHTTSAUDH == null)
                //|| lblPageTotal == null || lblGrandTotal == null) 
                return;


            var source = gvSelectedVatTuKHTTSAUDH.DataSource as List<CTTHIETKE>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTuKHTTSAUDH.PageSize * gvSelectedVatTuKHTTSAUDH.PageIndex].MAVT;
            var mambvt = source[e.Row.RowIndex + gvSelectedVatTuKHTTSAUDH.PageSize * gvSelectedVatTuKHTTSAUDH.PageIndex].MADDK;

            cbISCTYDTUKHTTSAUDH.Checked = source[e.Row.RowIndex + gvSelectedVatTuKHTTSAUDH.PageSize * gvSelectedVatTuKHTTSAUDH.PageIndex].ISCTYDTU.HasValue && source[e.Row.RowIndex].ISCTYDTU.Value;
            var script = "javascript:updateCTTKKHTTSAUDH(\"" + mambvt + "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\", \"" + cbISCTYDTUKHTTSAUDH.ClientID +
                                                        "\")";
            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);
            cbISCTYDTUKHTTSAUDH.Attributes.Add("onchange", script);

        }
        #endregion

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

        protected void lkChangeMAVTKHTT_Click(object sender, EventArgs e)
        {
            if (txtMAVTKHTT.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var vt = vtDao.Get(txtMAVTKHTT.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.", txtMAVTKHTT.ClientID);
                return;
            }

            lblTENVTKHTT.Text = vt.TENVT;
            txtKHOILUONGKHTT.Text = "1";
            FocusAndSelect(txtKHOILUONGKHTT.ClientID);
           
            CloseWaitingDialog();
        }

        protected void lkChangeMAVTKHTTSAUDH_Click(object sender, EventArgs e)
        {
            if (txtMAVTKHTTSAUDH.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var vt = vtDao.Get(txtMAVTKHTTSAUDH.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.", txtMAVTKHTTSAUDH.ClientID);
                return;
            }

            lblTENVTKHTTSAUDH.Text = vt.TENVT;
            txtKHOILUONGKHTTSAUDH.Text = "1";
            FocusAndSelect(txtKHOILUONGKHTTSAUDH.ClientID);
           
            CloseWaitingDialog();
        }

        protected void linkBtnChangeKhoiLuong_Click(object sender, EventArgs e)
        {
            if (ThietKe == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (txtMAVT.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập mã vật tư", txtMAVT.ClientID);
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

            var vt = vtDao.Get(txtMAVT.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.");
                return;
            }

            // add to grid
            var cttk = new CTTHIETKE
            {
                MADDK = ThietKe.MADDK,
                MAVT = vt.MAVT,
                NOIDUNG = vt.TENVT,
                SOLUONG = decimal.Parse(txtKHOILUONG.Text.Trim()),
                GIAVT =  0,
                GIANC =  0,
                TIENVT = 0,
                TIENNC = 0,
                ISCTYDTU = true
            };

            cttkdao.Insert(cttk);
            BindSelectedVatTuGrid();

            txtMAVT.Text = "";
            txtKHOILUONG.Text = "";
            lblTENVT.Text = "";
            FocusAndSelect(txtMAVT.ClientID);

            TongTienTK();
            upnlMBVT.Update();
            CloseWaitingDialog();
        }

        protected void lkChangeKhoiLuongKHTT_Click(object sender, EventArgs e)
        {
            if (ThietKe == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (txtMAVTKHTT.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập mã vật tư", txtMAVTKHTT.ClientID);
                return;
            }

            try
            {
                decimal.Parse(txtKHOILUONGKHTT.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khối lượng"), txtKHOILUONGKHTT.ClientID);
                return;
            }

            var vt = vtDao.Get(txtMAVTKHTT.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.");
                return;
            }

            // add to grid
            var cttk = new CTTHIETKE
            {
                MADDK = ThietKe.MADDK,
                MAVT = vt.MAVT,
                NOIDUNG = vt.TENVT,
                SOLUONG = decimal.Parse(txtKHOILUONGKHTT.Text.Trim()),
                GIAVT = vt.GIAVT.HasValue ? vt.GIAVT.Value : 0,
                GIANC = vt.GIANC.HasValue ? vt.GIANC.Value : 0,
                TIENVT = vt.GIAVT.HasValue ? decimal.Parse(txtKHOILUONGKHTT.Text.Trim()) * vt.GIAVT.Value : 0,
                TIENNC = vt.GIANC.HasValue ? decimal.Parse(txtKHOILUONGKHTT.Text.Trim()) * vt.GIANC.Value : 0,
                ISCTYDTU = false
            };

            cttkdao.Insert(cttk);
            BindSelectedVatTuKHTT();

            txtMAVTKHTT.Text = "";
            txtKHOILUONGKHTT.Text = "";
            lblTENVTKHTT.Text = "";
            FocusAndSelect(txtMAVTKHTT.ClientID);

            TongTienTK();
            upBVTKHTHANHTOAN.Update();
            CloseWaitingDialog();
        }

        protected void lkChangeKhoiLuongKHTTSAUDH_Click(object sender, EventArgs e)
        {
            if (ThietKe == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (txtMAVTKHTTSAUDH.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập mã vật tư", txtMAVTKHTTSAUDH.ClientID);
                return;
            }

            try
            {
                decimal.Parse(txtKHOILUONGKHTTSAUDH.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khối lượng"), txtKHOILUONGKHTTSAUDH.ClientID);
                return;
            }

            var vt = vtDao.Get(txtMAVTKHTTSAUDH.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.");
                return;
            }

            // add to grid
            var cttk = new CTTHIETKE
            {
                MADDK = ThietKe.MADDK,
                MAVT = vt.MAVT,
                NOIDUNG = vt.TENVT,
                SOLUONG = decimal.Parse(txtKHOILUONGKHTTSAUDH.Text.Trim()),
                GIAVT = vt.GIAVT.HasValue ? vt.GIAVT.Value : 0,
                GIANC = vt.GIANC.HasValue ? vt.GIANC.Value : 0,
                TIENVT = vt.GIAVT.HasValue ? decimal.Parse(txtKHOILUONGKHTTSAUDH.Text.Trim()) * vt.GIAVT.Value : 0,
                TIENNC = vt.GIANC.HasValue ? decimal.Parse(txtKHOILUONGKHTTSAUDH.Text.Trim()) * vt.GIANC.Value : 0,
                ISCTYDTU = false
            };

            cttkdao.Insert(cttk);
            BindSelectedVatTuKHTTSAUDH();

            txtMAVTKHTTSAUDH.Text = "";
            txtKHOILUONGKHTTSAUDH.Text = "";
            lblTENVTKHTTSAUDH.Text = "";
            FocusAndSelect(txtMAVTKHTTSAUDH.ClientID);

            TongTienTK();
            upBVTKHTHANHTOANSAUDH.Update();
            CloseWaitingDialog();
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            try
            {
                var mbvt = mbvtDao.Get(ddlMBVT.SelectedValue);
                tkDao.ChangeFromMBVT(ThietKe, mbvt);

                BindSelectedVatTuGrid();
                BindSelectedVatTuKHTT();
                BindChiPhiDLVC();
                BindSelectedVatTuKHTTSAUDH();
                TongTienTK();

                upnlMBVT.Update();
                upBVTKHTHANHTOAN.Update();
                upCPDAOLAPVC.Update();
                upBVTKHTHANHTOANSAUDH.Update();
                upTONGTIENTK.Update();
            }
            catch { }
        }        

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {               
                if (ThietKe == null)
                    return;

                LoadKhuVucUpdate();

                string ngaygio = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() +
                        DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                        DateTime.Now.Second.ToString();

                if (UpHINH.PostedFile.ContentLength > 0)
                {
                    filenametk1 = Path.GetFileName(UpHINH.PostedFile.FileName);
                    duongdantk1 = "UpLoadFile/" + khuvuc + "/hinhthietke/" + ngaygio + filenametk1;
                    UpHINH.SaveAs(Server.MapPath("~/" + duongdantk1));

                    var tk1 = new THIETKE
                    {
                        MADDK = ThietKe.MADDK,
                        HINHTK1 = "~/" + duongdantk1
                        //HINHTK2 = "~/" + duongdantk2
                    };
                    tkDao.UpdateHinh1(tk1, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }

                if (UpHINH2.PostedFile.ContentLength > 0)
                {
                    filenametk2 = Path.GetFileName(UpHINH2.PostedFile.FileName);
                    duongdantk2 = "UpLoadFile/" + khuvuc + "/hinhthietke/" + ngaygio + filenametk2;
                    UpHINH2.SaveAs(Server.MapPath("~/" + duongdantk2));

                    var tk2 = new THIETKE
                    {
                        MADDK = ThietKe.MADDK,
                        //HINHTK1 = "~/" + duongdantk1
                        HINHTK2 = "~/" + duongdantk2
                    };
                    tkDao.UpdateHinh2(tk2, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }

                //var tk = new THIETKE
                //{
                //    MADDK = ThietKe.MADDK,
                //    HINHTK1 = "~/" + duongdantk1,
                //    HINHTK2 = "~/" + duongdantk2
                //};
                //tkDao.Update(tk, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                if (ThietKe.HINHTK1 != null)
                {
                    imgHINHTK1.ImageUrl = ThietKe.HINHTK1;
                    imgHINHTK1.Visible = true;
                }
                else
                {
                    imgHINHTK1.Visible = false;
                }

                if (ThietKe.HINHTK2 != null)
                {
                    imgHINHTK2.ImageUrl = ThietKe.HINHTK2;
                    imgHINHTK2.Visible = true;
                }
                else
                {
                    imgHINHTK2.Visible = false;
                }

                CloseWaitingDialog();
                upTHONGTINTK.Update();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void LoadKhuVucUpdate()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            //string b = loginInfo.Username;
            string manv = loginInfo.Username;

            var query = _nvDao.GetListKV(manv);
            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV == "99")
                {
                    khuvuc = "powaco";
                }
                if (a.MAKV == "O")
                {
                    khuvuc = "chauthanh";
                }
                if (a.MAKV == "D")
                {
                    khuvuc = "chauphu";
                }
                if (a.MAKV == "A")
                {
                    khuvuc = "chomoi";
                }
                if (a.MAKV == "B")
                {
                    khuvuc = "triton";
                }
                if (a.MAKV == "F")
                {
                    khuvuc = "phutan";
                }
                if (a.MAKV == "G")
                {
                    khuvuc = "anphu";
                }
                if (a.MAKV == "H")
                {
                    khuvuc = "tanchau";
                }
                if (a.MAKV == "U")
                {
                    khuvuc = "thoaison";
                }
                if (a.MAKV == "J")
                {
                    khuvuc = "chaudoc";
                }
                if (a.MAKV == "M")
                {
                    khuvuc = "tinhbien";
                }
                if (a.MAKV == "X")
                {
                    khuvuc = "longxuyen";
                }
            }
        }

        protected void btnBrowseVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            upnlVatTu.Update();
            UnblockDialog("divVatTu");
        }

        protected void btVATTUKHTT_Click(object sender, EventArgs e)
        {
            BindVatTuKHTT();
            upBVTKHTHANHTOAN.Update();
            UnblockDialog("divVatTuKHTT");
        }

        protected void btVATTUKHTTSAUDH_Click(object sender, EventArgs e)
        {
            BindVatTuKHTTSAUDH();
            upBVTKHTHANHTOANSAUDH.Update();
            UnblockDialog("divVatTuKHTTSAUDH");
        }

        private void BindChiPhiDLVC()
        {
            if (ThietKe == null)
                return;

            var list = dltkDao.GetList(ThietKe.MADDK);
            gvChiPhiDLVC.DataSource = list;
            gvChiPhiDLVC.PagerInforText = list.Count.ToString();
            gvChiPhiDLVC.DataBind();
        }

        #region gvChiPhiDLVC
        protected void gvChiPhiDLVC_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var macp = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteChiPhi":
                    var deletingCPTK = dltkDao.Get(Int32.Parse(macp));
                    if (deletingCPTK == null)
                    {                       
                        return;
                    }
                    dltkDao.Delete(deletingCPTK);

                    BindChiPhiDLVC();
               
                    break;
            }
        }

        protected void gvChiPhiDLVC_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtND = e.Row.FindControl("txtNOIDUNG") as TextBox;
            var txtDG = e.Row.FindControl("txtDONGIA") as TextBox;
            var ddlDVT = e.Row.FindControl("ddlDVT") as DropDownList;
            var txtSL = e.Row.FindControl("txtSOLUONG") as TextBox;
            //var txtHS = e.Row.FindControl("txtHESOCP") as TextBox;
            var txtCHIEURONG = e.Row.FindControl("txtCHIEURONG") as TextBox;
            var txtCHIEUCAO = e.Row.FindControl("txtCHIEUCAO") as TextBox;

            var lblTHANHTIENCP = e.Row.FindControl("lblTHANHTIENCP") as Label;
            var ddlLCP = e.Row.FindControl("ddlLOAICP") as DropDownList;

            if (txtND == null || txtDG == null || ddlDVT == null || txtSL == null ||//  txtHS == null || 
                txtCHIEURONG == null || txtCHIEUCAO == null ||
                lblTHANHTIENCP == null || ddlLCP == null) return;

            var source = gvChiPhiDLVC.DataSource as List<DAOLAPTK>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhiDLVC.PageSize * gvChiPhiDLVC.PageIndex].MADON;

            var script = "javascript:updateCPTK(\"" + madon + "\", \"" + txtND.ClientID +
                                                                "\", \"" + txtDG.ClientID +
                                                                "\", \"" + ddlDVT.ClientID +
                                                                "\", \"" + txtSL.ClientID +
                                                                //"\", \"" + txtHS.ClientID +
                                                                "\", \"" + txtCHIEURONG.ClientID +
                                                                "\", \"" + txtCHIEUCAO.ClientID +
                                                                "\", \"" + lblTHANHTIENCP.ClientID +
                                                                "\", \"" + ddlLCP.ClientID +
                                                                "\")";
            txtND.Attributes.Add("onblur", script);
            txtDG.Attributes.Add("onblur", script);
            txtSL.Attributes.Add("onblur", script);
            //txtHS.Attributes.Add("onblur", script);
            txtCHIEURONG.Attributes.Add("onblur", script);
            txtCHIEUCAO.Attributes.Add("onblur", script);

            ddlDVT.Attributes.Add("onchange", script);
            ddlLCP.Attributes.Add("onchange", script);           
        }
        #endregion       

        protected void btAddChiPhiDLVC_Click(object sender, EventArgs e)
        {
            try
            {
                if (ThietKe == null)
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

                //var vtvuot = cttkdao.GetVatTuVuot(ThietKe.MADDK);
                //if (vtvuot == null)
                //{
                //    CloseWaitingDialog();
                //    return;
                //}

                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;

                var cptkdao = new DAOLAPTK
                {
                    MADDK = ThietKe.MADDK,
                    NOIDUNG = "Nhân công đào đất đặt ống (0.2 x 0.2)",
                    DONGIACP = 175392,
                    //SOLUONG = vtvuot.SOLUONG != null ? vtvuot.SOLUONG : 1,
                    SOLUONG = 1,
                    DVT = dvtList[8].DVT1,
                    HESOCP = 1,
                    //THANHTIENCP = vtvuot.SOLUONG != null ? vtvuot.SOLUONG * 175392 * Convert.ToDecimal("0.2") * Convert.ToDecimal("0.2") : 0,
                    THANHTIENCP = 1 * 175392 * Convert.ToDecimal("0.2") * Convert.ToDecimal("0.2") ,
                    LOAICP = "DAO",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP",
                    CHIEUCAO = Convert.ToDecimal("0.2"),
                    CHIEURONG = Convert.ToDecimal("0.2"),
                    MAKV = makv,
                    KYHIEUVT = "AB.11512"
                };
                dltkDao.Insert(cptkdao);

                TongTienTK();
                upTONGTIENTK.Update();

                BindChiPhiDLVC();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }        

        protected void btAddChiPhiLap_Click(object sender, EventArgs e)
        {
            try
            {
                if (ThietKe == null)
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

                //var vtvuot = cttkdao.GetVatTuVuot(ThietKe.MADDK);
                //if (vtvuot == null)
                //{
                //    CloseWaitingDialog();
                //    return;
                //}

                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;

                var cptkdao = new DAOLAPTK
                {
                    MADDK = ThietKe.MADDK,
                    NOIDUNG = "Nhân công lắp đất đặt ống (0.2 x 0.2)",
                    DONGIACP = 115643,
                    //SOLUONG = vtvuot.SOLUONG != null ? vtvuot.SOLUONG : 1,
                    SOLUONG = 1,
                    DVT = dvtList[8].DVT1,
                    HESOCP = 1,
                    //THANHTIENCP = vtvuot.SOLUONG != null ? vtvuot.SOLUONG * 115643 * Convert.ToDecimal("0.2") * Convert.ToDecimal("0.2") : 0,
                    THANHTIENCP = 1 * 115643 * Convert.ToDecimal("0.2") * Convert.ToDecimal("0.2") ,
                    LOAICP = "LAP",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP",
                    CHIEUCAO = Convert.ToDecimal("0.2"),
                    CHIEURONG = Convert.ToDecimal("0.2"),
                    MAKV = makv,
                    KYHIEUVT = "AB.13121"
                };
                dltkDao.Insert(cptkdao);

                TongTienTK();
                upTONGTIENTK.Update();
             
                BindChiPhiDLVC();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btAddChiPhiVanChuyen_Click(object sender, EventArgs e)
        {
            try
            {
                if (ThietKe == null)
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

                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;

                var cptkdao = new DAOLAPTK
                {
                    MADDK = ThietKe.MADDK,
                    NOIDUNG = "Vận chuyển",
                    DONGIACP = 5000,
                    SOLUONG = 1,
                    DVT = dvtList[8].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 5000,
                    LOAICP = "VC",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP",
                    CHIEUCAO = 1,
                    CHIEURONG = 1,
                    MAKV = makv
                };
                dltkDao.Insert(cptkdao);

                TongTienTK();
                upTONGTIENTK.Update();

                BindChiPhiDLVC();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btTINHTIENTK_Click(object sender, EventArgs e)
        {
            TongTienTK();
            upTONGTIENTK.Update();
        }

        private void TongTienTK()
        {
            try
            {
                if (ThietKe == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                decimal? ttnhancong = cttkdao.TTNhanCong(ThietKe.MADDK) != null ? cttkdao.TTNhanCong(ThietKe.MADDK) : 0;
                decimal? ttvattu = cttkdao.TTVatTu(ThietKe.MADDK) != null ? cttkdao.TTVatTu(ThietKe.MADDK) : 0;

                decimal? ttdaolap = dltkDao.TTDaoLap(ThietKe.MADDK) != null ? dltkDao.TTDaoLap(ThietKe.MADDK) : 0;
                decimal? ttvanchuyen = dltkDao.TTVanChuyen(ThietKe.MADDK) != null ? dltkDao.TTVanChuyen(ThietKe.MADDK) : 0;

                decimal? ttG = ttdaolap + ttnhancong + (ttnhancong * (decimal)0.05) + ((ttnhancong + (ttnhancong * (decimal)0.05)) * (decimal)0.055);
                decimal? ttcptk = ttG * (decimal)0.0207 * (decimal)1.3;

                decimal? ttxltruocthue = Convert.ToInt64(ttG) + Convert.ToInt64(ttcptk) + Convert.ToInt64(ttvattu);
                decimal? ttxlthueGTGT = ttxltruocthue * (decimal)0.1;
              
                decimal? ttvctruocthue = ttvanchuyen / (decimal)1.1;
                decimal? ttvcthueGTGT = ttvctruocthue * (decimal)0.1;

                lbTCTVCTT2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(ttvctruocthue));
                lbTTGTGTVC2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(ttvcthueGTGT));
                lbTCPVCST2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(ttvctruocthue + ttvcthueGTGT));

                lbTHCPNCT4.Text = string.Format("{0 : 0,0}", Convert.ToInt64(ttnhancong + ttdaolap));          // T
                decimal? chiphichungC = Convert.ToInt64((ttnhancong + ttdaolap) * (decimal)0.05);               // C = T * 5%
                lbTHCPCHUNGC4.Text = string.Format("{0 : 0,0}", chiphichungC);
                decimal? thunhapchiuthuetinhtruocTL = (ttnhancong + ttdaolap + chiphichungC) * (decimal)0.055;  // TL = (T + C) * 5.5%
                lbTHTNCTTTTL4.Text = string.Format("{0 : 0,0}", thunhapchiuthuetinhtruocTL);
                decimal? gtxaylaptthueG = ttnhancong + ttdaolap + chiphichungC + thunhapchiuthuetinhtruocTL;   // G
                lbGTXLTTG4.Text = string.Format("{0 : 0,0}", gtxaylaptthueG);
                decimal? tgtgtxaylapVAT1 = gtxaylaptthueG * (decimal)0.1;                           // VAT1 = G * 10%
                lbTGTGTXLVAT14.Text = string.Format("{0 : 0,0}", tgtgtxaylapVAT1);
                decimal? cpxaylapsauthueG1 = gtxaylaptthueG + tgtgtxaylapVAT1;                      // G1
                lbCPXLSTG14.Text = string.Format("{0 : 0,0}", cpxaylapsauthueG1);
                decimal? cpthietkettTK = gtxaylaptthueG * (decimal)0.0207 * (decimal)1.3;           // TK = G * 2.07% * 1.3
                lbCPTKTTTK4.Text = string.Format("{0 : 0,0}", cpthietkettTK);
                decimal? tgtgttkVAT2 = cpthietkettTK * (decimal)0.1;                                // VAT2 = TK * 10%
                lbTGTGTTTVAT24.Text = string.Format("{0 : 0,0}", tgtgttkVAT2);
                decimal? cptkstG2 = cpthietkettTK + tgtgttkVAT2;                                    // G2
                lbCPTKSTG24.Text = string.Format("{0 : 0,0}", cptkstG2);
                lbCPVTTTVT14.Text = string.Format("{0 : 0,0}", ttvattu);                            // VT
                decimal? tgtgtvtVAT3 = ttvattu * (decimal)0.1;                                      // VAT3 = VT * 10%
                lbTGTGTVT14.Text = string.Format("{0 : 0,0}", tgtgtvtVAT3);
                decimal? cpvtstG3 = ttvattu + tgtgtvtVAT3;                                          // G3
                lbCPVTSTG314.Text = string.Format("{0 : 0,0}", cpvtstG3);

                lbCPVCTTVAT14.Text = string.Format("{0 : 0,0}", ttvctruocthue);                     // VC
                lbTGTGTVC4.Text = string.Format("{0 : 0,0}", ttvcthueGTGT);                         // VAT4 = VC * 10%
                decimal? cpvcstG4 = ttvctruocthue + ttvcthueGTGT;                                   // G4
                lbCPVCSTG44.Text = string.Format("{0 : 0,0}", cpvcstG4);  
                decimal? tongcongcpsauthuekh = cpxaylapsauthueG1 + cptkstG2 + cpvtstG3 + cpvcstG4;
                lbTCPTTSTTC4.Text = string.Format("{0 : 0,0}", tongcongcpsauthuekh);

                decimal? tonggiatrixaylaptt = gtxaylaptthueG + cpthietkettTK + ttvattu;
                lbTGTXLTT2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(tonggiatrixaylaptt));
                decimal? thuetonggiatrixaylaptt = tonggiatrixaylaptt * (decimal)0.1;
                lbTTGTGT2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(thuetonggiatrixaylaptt));
                lbTGTXLST2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(tonggiatrixaylaptt + thuetonggiatrixaylaptt));

                decimal? tongtiensauthue = tonggiatrixaylaptt + thuetonggiatrixaylaptt + ttvctruocthue + ttvcthueGTGT;
                lbTONCONGSAUTHUE2.Text = string.Format("{0 : 0,0}", Convert.ToInt64(tongtiensauthue));

                var tktt = new THIETKE
                {
                    MADDK = ThietKe.MADDK,
                    TONGTIENTK = tongtiensauthue
                };
                tkDao.UpdateTongTien(tktt, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                _rpClass.TongHopCPTK(ThietKe.MADDK, "", "", ttnhancong + ttdaolap, chiphichungC, thunhapchiuthuetinhtruocTL,
                            gtxaylaptthueG, tgtgtxaylapVAT1, cpxaylapsauthueG1, cpthietkettTK, tgtgttkVAT2, cptkstG2,
                            ttvattu, tgtgtvtVAT3, cpvtstG3, ttvctruocthue, ttvcthueGTGT, cpvcstG4, "INTHCPTKN");

                //if (gctkdao.GetMaDonKy(ThietKe.MADDK) != null)
                //{
                //    //update
                //    _rpClass.TongHopCPTK(ThietKe.MADDK, "", "", ttnhancong, chiphichungC, thunhapchiuthuetinhtruocTL,
                //            gtxaylaptthueG, tgtgtxaylapVAT1, cpxaylapsauthueG1, cpthietkettTK, tgtgttkVAT2, cptkstG2,
                //            ttvattu, tgtgtvtVAT3, cpvtstG3, ttvctruocthue, ttvcthueGTGT, cpvcstG4, "UPTHCPTKN");                                                            
                //}
                //else 
                //{
                //    //insert
                //    _rpClass.TongHopCPTK(ThietKe.MADDK, "", "", ttnhancong, chiphichungC, thunhapchiuthuetinhtruocTL,
                //            gtxaylaptthueG, tgtgtxaylapVAT1, cpxaylapsauthueG1, cpthietkettTK, tgtgttkVAT2, cptkstG2,
                //            ttvattu, tgtgtvtVAT3, cpvtstG3, ttvctruocthue, ttvcthueGTGT, cpvcstG4, "INTHCPTKN");
                //}

                upTONGTIENTK.Update();
                CloseWaitingDialog();
            }
            catch { }
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            Session["NHAPTHIETKE_MADDK"] = ThietKe.MADDK;
            Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpVTTKBVT.aspx", false);
            CloseWaitingDialog();  

            //BaoCaoBVT();
            //CloseWaitingDialog();
        }

        private void BaoCaoBVT()
        {
            try
            {
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

                //var dt = new ReportClass().BaoCaoVTTK(MADDK);

                rp = new ReportDocument();
                var path = Server.MapPath("~/Reports/DonLapDatMoi/VatTuThietKe_LX.rpt");
                rp.Load(path);


                //rp.SetDataSource(dt.Tables[0]);
                rpViewer.ReportSource = rp;
                rpViewer.DataBind();


                //Session[SessionKey.TK_BAOCAO_BOCVATTULX] = dt;
                Session[Constants.REPORT_FREE_MEM] = rp;

                rpViewer.Visible = true;
                upBAOCAOBVT.Update();
            }
            catch { }
        }

    }
}