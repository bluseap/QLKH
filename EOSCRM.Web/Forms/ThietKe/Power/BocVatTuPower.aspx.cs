using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.IO;

namespace EOSCRM.Web.Forms.ThietKe.Power
{
    public partial class BocVatTuPower : Authentication
    {
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly MauBocVatTuDao mbvtDao = new MauBocVatTuDao();
        private readonly DaoLapThietKeDao dltkDao = new DaoLapThietKeDao();
        private readonly ChiTietThietKeDao cttkdao = new ChiTietThietKeDao();
        private readonly GhiChuThietKeDao gctkdao = new GhiChuThietKeDao();
        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly DvtDao dvtDao = new DvtDao();        
        private readonly MauThietKeDao _mtkDao = new MauThietKeDao();
        private readonly ReportClass _rpC = new ReportClass();

        string khuvuc, filenametk1, duongdantk1;

        #region Startup script registeration
        private void ShowError(string message, string controlId)
        {
            ((PO)Page.Master).ShowError(message, controlId);
        }

        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInFor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void SetControlValue(string id, string value)
        {
            ((PO)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((PO)Page.Master).SetLabel(id, value);
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

        private void FocusAndSelect(string controlId)
        {
            ((PO)Page.Master).FocusAndSelect(controlId);
        }
        #endregion

        protected THIETKEPO ThietKePo
        {
            get
            {
                if (Session["NHAPTHIETKE_MADDK"] == null)
                    return null;

                return _tkpoDao.Get(Session["NHAPTHIETKE_MADDK"].ToString());
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_ThietKeVaVatTuPo, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadReferences();

                    BindSelectedVatTuGrid();
                    BindChiPhi();
                    BindGhiChu();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_THIETKEBOCVATTUDIEN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_BOCVATTUDIEN;
            }

            CommonFunc.SetPropertiesForGrid(gvVatTu);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu);
            CommonFunc.SetPropertiesForGrid(gvGhiChu);
            CommonFunc.SetPropertiesForGrid(gvChiPhi);
        }        

        private void LoadReferences()
        {
            if (ThietKePo == null)
                return;            

            var list = mbvtDao.GetListDien();
            ddlMBVT.Items.Clear();
            ddlMBVT.Items.Add(new ListItem("", ""));

            lbTENKH.Text = _ddkpoDao.Get(ThietKePo.MADDKPO.ToString()).TENKH;

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

            var listMTK = _mtkDao.GetListDien();
            ddlMBTHIETKE.Items.Clear();
            ddlMBTHIETKE.Items.Add(new ListItem("Chọn mẫu TK", "ALL"));
            foreach (var mtk in listMTK)
                ddlMBTHIETKE.Items.Add(new ListItem(mtk.TENMAUTK, mtk.MAMAUTK));

            LoadMauTK();

            var thieke = _tkpoDao.Get(ThietKePo.MADDKPO);
            if (thieke.TENKHPHAI != null)
            { txtTENKHBP.Text = thieke.TENKHPHAI.ToString(); }
            else { txtTENKHBP.Text = ""; }
            if (thieke.TENKHTRAI != null)
            {
                txtTENKHBT.Text = thieke.TENKHTRAI.ToString();
            }
            else { txtTENKHBT.Text = ""; }
            if (thieke.DANHSOPHAI != null)
            {
                txtDANHSOBP.Text = thieke.DANHSOPHAI.ToString();
            }
            else { txtDANHSOBP.Text = ""; }
            if (ThietKePo.DANHSOTRAI != null)
            {
                txtDANHSOBT.Text = ThietKePo.DANHSOTRAI.ToString();
            }
            else { txtDANHSOBT.Text = ""; }

            if (ThietKePo.TENTRUPHAI != null)
            {
                txtTRUTRUOC.Text = ThietKePo.TENTRUPHAI.ToString();
            }
            else { txtTRUTRUOC.Text = ""; }
            if (ThietKePo.DANHSOTRUPHAI != null)
            {
                txtDSTRUTRUOC.Text = ThietKePo.DANHSOTRUPHAI.ToString();
            }
            else { txtDSTRUTRUOC.Text = ""; }

            if (ThietKePo.TENTRUTRAI != null)
            {
                txtTRUSAU.Text = ThietKePo.TENTRUTRAI.ToString();
            }
            else { txtTRUSAU.Text = ""; }

            if (ThietKePo.DANHSOTRUTRAI != null)
            {
                txtDSTRUSAU.Text = ThietKePo.DANHSOTRUTRAI.ToString();
            }
            else { txtDSTRUSAU.Text = ""; }

            if (ThietKePo.HINHTK1 != null)
            {
                imgHINHTK1.ImageUrl = ThietKePo.HINHTK1;
            }
            else
            {
                imgHINHTK1.Visible = false;
            }

            if (ThietKePo.KETLUANTK != null)
            {
                txtKetLuanSauTK.Text = ThietKePo.KETLUANTK.ToString();
            }
            else { txtKetLuanSauTK.Text = ""; }
        }

        private void LoadMauTK()
        {
            var mautk = _tkpoDao.Get(ThietKePo.MADDKPO).MAMAUTK;
            if (mautk != null)
            {
                var item10 = ddlMBTHIETKE.Items.FindByValue(mautk);
                if (item10 != null)
                    ddlMBTHIETKE.SelectedIndex = ddlMBTHIETKE.Items.IndexOf(item10);
            }
            else
            {
                var listMTK = _mtkDao.GetListDien();
                ddlMBTHIETKE.Items.Clear();
                ddlMBTHIETKE.Items.Add(new ListItem("Chọn mẫu TK", "ALL"));
                foreach (var mtk in listMTK)
                    ddlMBTHIETKE.Items.Add(new ListItem(mtk.TENMAUTK, mtk.MAMAUTK));
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            var mbvt = mbvtDao.GetD(ddlMBVT.SelectedValue);
            _tkpoDao.ChangeFromMBVT(ThietKePo, mbvt);

            BindSelectedVatTuGrid();
            BindChiPhi();
            BindGhiChu();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var url = ResolveUrl("~") + "Forms/ThietKe/Power/NhapTKPower.aspx";
            Response.Redirect(url, false);
        }

        private void BindVatTu()
        {
            var list = vtDao.SearchDien(txtFilterVatTu.Text.Trim());
            gvVatTu.DataSource = list;
            gvVatTu.PagerInforText = list.Count.ToString();
            gvVatTu.DataBind();
        }

        protected void btnBrowseVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            upnlVatTu.Update();
            UnblockDialog("divVatTu");
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

        protected void gvVatTu_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvVatTu.PageIndex = e.NewPageIndex;
                // Bind data for grid
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

        protected void btnFilterVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            CloseWaitingDialog();
        }

        private void BindSelectedVatTuGrid()
        {
            if (ThietKePo == null)
                return;

            var list = cttkdao.GetList(ThietKePo.MADDKPO);

            gvSelectedVatTu.DataSource = list;
            gvSelectedVatTu.PagerInforText = list.Count.ToString();
            gvSelectedVatTu.DataBind();
        }

        protected void gvSelectedVatTu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteVatTu":
                    if (ThietKePo == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }

                    var deletingCTTK = cttkdao.Get(ThietKePo.MADDKPO, mavt);
                    if (deletingCTTK == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    cttkdao.Delete(deletingCTTK);

                    BindSelectedVatTuGrid();

                    //CloseWaitingDialog();

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
                lblTIENNC == null || lblTIENVT == null || cbISCTYDTU == null) return;

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

        private void BindGhiChu()
        {
            if (ThietKePo == null)
                return;

            var list = gctkdao.GetList(ThietKePo.MADDKPO);

            gvGhiChu.DataSource = list;
            gvGhiChu.PagerInforText = list.Count.ToString();
            gvGhiChu.DataBind();
        }

        protected void gvGhiChu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var magc = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteGhiChu":
                    var deletingGCTK = gctkdao.Get(Int32.Parse(magc));
                    if (deletingGCTK == null)
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    gctkdao.Delete(deletingGCTK);

                    BindGhiChu();

                    CloseWaitingDialog();
                    break;
            }
        }

        protected void gvGhiChu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtNOIDUNG") as TextBox;
            if (txtSL == null) return;

            var source = gvGhiChu.DataSource as List<GCTHIETKE>;
            if (source == null) return;

            var magc = source[e.Row.RowIndex + gvGhiChu.PageSize * gvGhiChu.PageIndex].MAGHICHU;

            var script = "javascript:updateGCTK(\"" + magc + "\", \"" + txtSL.ClientID + "\")";
            txtSL.Attributes.Add("onblur", script);

            var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            if (btnDeleteItem == null) return;
            btnDeleteItem.Attributes.Add("onclick", "onClientClickGridDelete('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }

        protected void btnAddGhiChu_Click(object sender, EventArgs e)
        {
            try
            {
                if (ThietKePo == null)
                {
                    CloseWaitingDialog();
                    return;
                }

                var gcmbvt = new GCTHIETKE
                {
                    MAMBVT = ThietKePo.MADDKPO,
                    NOIDUNG = ""
                };
                gctkdao.Insert(gcmbvt);
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
            if (ThietKePo == null)
                return;

            var list = dltkDao.GetList(ThietKePo.MADDKPO);

            gvChiPhi.DataSource = list;
            //gvChiPhi.PagerInforText = list.Count.ToString();
            gvChiPhi.DataBind();
        }

        protected void gvChiPhi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var macp = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteChiPhi":
                    var deletingCPTK = dltkDao.Get(Int32.Parse(macp));
                    if (deletingCPTK == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    dltkDao.Delete(deletingCPTK);

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
            var txtHS = e.Row.FindControl("txtHESOCP") as TextBox;
            var lblTHANHTIENCP = e.Row.FindControl("lblTHANHTIENCP") as Label;
            var ddlLCP = e.Row.FindControl("ddlLOAICP") as DropDownList;

            if (txtND == null || txtDG == null || ddlDVT == null || txtSL == null ||
                txtHS == null || lblTHANHTIENCP == null || ddlLCP == null) return;

            var source = gvChiPhi.DataSource as List<DAOLAPTK>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi.PageSize * gvChiPhi.PageIndex].MADON;


            var script = "javascript:updateCPTK(\"" + madon + "\", \"" + txtND.ClientID +
                                                                "\", \"" + txtDG.ClientID +
                                                                "\", \"" + ddlDVT.ClientID +
                                                                "\", \"" + txtSL.ClientID +
                                                                "\", \"" + txtHS.ClientID +
                                                                "\", \"" + lblTHANHTIENCP.ClientID +
                                                                "\", \"" + ddlLCP.ClientID +
                                                                "\")";
            txtND.Attributes.Add("onblur", script);
            txtDG.Attributes.Add("onblur", script);
            txtSL.Attributes.Add("onblur", script);
            txtHS.Attributes.Add("onblur", script);
            ddlDVT.Attributes.Add("onchange", script);
            ddlLCP.Attributes.Add("onchange", script);

            //var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            //if (btnDeleteItem == null) return;
            //btnDeleteItem.Attributes.Add("onclick", "onClientClickGridDelete('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");

        }

        protected void btnAddChiPhi_Click(object sender, EventArgs e)
        {
            try
            {
                if (ThietKePo == null)
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

                var cptk = new DAOLAPTK
                {
                    MADDK = ThietKePo.MADDKPO,
                    NOIDUNG = "Lắp điện",
                    DONGIACP = 5500,
                    SOLUONG = 1,
                    DVT = dvtList[8].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 5500,
                    LOAICP = "LAPD",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP"
                };
                dltkDao.Insert(cptk);
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
            if (ThietKePo == null)
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

            var ctvt = cttkdao.Get(ThietKePo.MADDKPO, txtMAVT.Text.Trim());
            if (ctvt != null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư đã có. Chọn vật tư khác.");
                return;
            }

            // add to grid
            var cttk = new CTTHIETKE
            {
                MADDK = ThietKePo.MADDKPO,
                MAVT = vt.MAVT,
                NOIDUNG = vt.TENVT,
                SOLUONG = decimal.Parse(txtKHOILUONG.Text.Trim()),
                GIAVT = vt.GIAVT.HasValue ? vt.GIAVT.Value : 0,
                GIANC = vt.GIANC.HasValue ? vt.GIANC.Value : 0,
                TIENVT = vt.GIAVT.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIAVT.Value : 0,
                TIENNC = vt.GIANC.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIANC.Value : 0,
                ISCTYDTU = chkIsCtyDauTu.Checked
            };

            cttkdao.Insert(cttk);
            BindSelectedVatTuGrid();

            txtMAVT.Text = "";
            txtKHOILUONG.Text = "";
            lblTENVT.Text = "";
            FocusAndSelect(txtMAVT.ClientID);

            upnlMBVT.Update();
            CloseWaitingDialog();
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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            Session["NHAPTHIETKE_MADDK"] = ThietKePo.MADDKPO;
            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/Power/BaoCaoPo/InThietKePo.aspx",
                                   false);
            CloseWaitingDialog();

        }

        protected void ddlMBTHIETKE_SelectedIndexChanged1(object sender, EventArgs e)
        {
            //var mamtk = ddlMBTHIETKE.SelectedValue;
            //var maddk = ThietKePo.MADDKPO;
            //if (mamtk.ToString() == "ALL")
            //{
            //    ShowError("Chọn mẫu thiết kế...");
            //    return;
            //}
            //else
            //{
            //    try
            //    {
            //        _rpC.UPMAUTKPO(maddk.ToString(), mamtk.ToString(),
            //            string.Empty.Equals(txtTENKHBP.Text) ? " " : txtTENKHBP.Text.ToString(),
            //            string.Empty.Equals(txtTENKHBT.Text) ? " " : txtTENKHBT.Text.ToString(),
            //            string.Empty.Equals(txtDANHSOBP.Text) ? " " : txtDANHSOBP.Text.ToString(),
            //            string.Empty.Equals(txtDANHSOBT.Text) ? " " : txtDANHSOBT.Text.ToString(),
                        
            //            string.Empty.Equals(txtTRUTRUOC.Text) ? " " : txtTRUTRUOC.Text.ToString(),
            //            string.Empty.Equals(txtTRUSAU.Text) ? " " : txtTRUSAU.Text.ToString(),
            //            string.Empty.Equals(txtDSTRUTRUOC.Text) ? " " : txtDSTRUTRUOC.Text.ToString(),
            //            string.Empty.Equals(txtDSTRUSAU.Text) ? " " : txtDSTRUSAU.Text.ToString()
            //            );
            //        ShowInFor("Cập nhật sơ đồ thiết kế thành công..");
            //    }
            //    catch 
            //    { 
            //        ShowError("Lỗi cập nhật mẫu sơ đồ thiết kế."); 
            //    }
            //}

        }        

        protected void btSaveMauTK_Click(object sender, EventArgs e)
        {
            try
            {
                if (ThietKePo == null)
                    return;

                LoadKhuVucUpdate();

                string ngaygio = DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString() +
                        DateTime.Now.Day.ToString() + DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString() +
                        DateTime.Now.Second.ToString();

                if (UpHinhTK.PostedFile.ContentLength > 0)
                {
                    filenametk1 = Path.GetFileName(UpHinhTK.PostedFile.FileName);
                    duongdantk1 = "UpLoadFile/" + khuvuc + "/hinhthietke/" + ngaygio + filenametk1;
                    UpHinhTK.SaveAs(Server.MapPath("~/" + duongdantk1));

                    var tk1 = new THIETKEPO
                    {
                        MADDKPO = ThietKePo.MADDKPO,
                        HINHTK1 = "~/" + duongdantk1
                    };
                    _tkpoDao.UpdateHinh1(tk1, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }
                else
                {
                    var tk2 = new THIETKEPO
                    {
                        MADDKPO = ThietKePo.MADDKPO,
                        HINHTK1 = "~/UpLoadFile/longxuyen/hinhthietke/tranglx.jpg"
                    };
                    _tkpoDao.UpdateHinh1(tk2, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
                }
                
                if (ThietKePo.HINHTK1 != null)
                {
                    imgHINHTK1.ImageUrl = ThietKePo.HINHTK1;
                    imgHINHTK1.Visible = true;
                }
                else
                {
                    imgHINHTK1.Visible = false;
                }


                var mamtk = ddlMBTHIETKE.SelectedValue;
                var maddk = ThietKePo.MADDKPO;

                //if (mamtk.ToString() == "ALL")
                //{
                //    ShowError("Chọn mẫu thiết kế...");
                //    return;
                //}
                //else
                //{
                    try
                    {
                        _rpC.UPMAUTKPO(maddk.ToString(), mamtk.ToString(),
                            string.Empty.Equals(txtTENKHBP.Text) ? " " : txtTENKHBP.Text.ToString(),
                            string.Empty.Equals(txtTENKHBT.Text) ? " " : txtTENKHBT.Text.ToString(),
                            string.Empty.Equals(txtDANHSOBP.Text) ? " " : txtDANHSOBP.Text.ToString(),
                            string.Empty.Equals(txtDANHSOBT.Text) ? " " : txtDANHSOBT.Text.ToString(),

                            string.Empty.Equals(txtTRUTRUOC.Text) ? " " : txtTRUTRUOC.Text.ToString(),
                            string.Empty.Equals(txtTRUSAU.Text) ? " " : txtTRUSAU.Text.ToString(),
                            string.Empty.Equals(txtDSTRUTRUOC.Text) ? " " : txtDSTRUTRUOC.Text.ToString(),
                            string.Empty.Equals(txtDSTRUSAU.Text) ? " " : txtDSTRUSAU.Text.ToString()
                            );

                        _rpC.BienKHPo(maddk.ToString(), khuvuc, string.Empty.Equals(txtKetLuanSauTK.Text.Trim()) ? " " : txtKetLuanSauTK.Text.Trim(), 
                            "", 1, 1, "UPKETLUANSAUTK");

                        ShowInFor("Cập nhật sơ đồ thiết kế thành công..");
                    }
                    catch
                    {
                        ShowError("Lỗi cập nhật mẫu sơ đồ thiết kế.");
                    }
                //}

                CloseWaitingDialog();
                upnlMBVT.Update();
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
                if (a.MAKV == "N")
                {
                    khuvuc = "chauphu";
                }
                if (a.MAKV == "K")
                {
                    khuvuc = "chomoi";
                }
                if (a.MAKV == "L")
                {
                    khuvuc = "triton";
                }
                if (a.MAKV == "P")
                {
                    khuvuc = "phutan";
                }
                if (a.MAKV == "Q")
                {
                    khuvuc = "anphu";
                }
                if (a.MAKV == "T")
                {
                    khuvuc = "tanchau";
                }
                if (a.MAKV == "U")
                {
                    khuvuc = "thoaison";
                }
                if (a.MAKV == "S")
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

        protected void btAddVatTu_Click(object sender, EventArgs e)
        {
            try
            {
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
        }

        protected void txtTRUTRUOC_TextChanged(object sender, EventArgs e)
        {

        }

        protected void txtDANHSOBT_TextChanged(object sender, EventArgs e)
        {

        }

        
    }
}