using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class BocVatTu : Authentication
    {
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly MauBocVatTuDao mbvtDao = new MauBocVatTuDao();
        private readonly DaoLapThietKeDao dltkDao = new DaoLapThietKeDao();
        private readonly ChiTietThietKeDao cttkdao = new ChiTietThietKeDao();
        private readonly GhiChuThietKeDao gctkdao = new GhiChuThietKeDao();
        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly DvtDao dvtDao = new DvtDao();
        private readonly DonDangKyDao _ddk = new DonDangKyDao();

        
        
        
        protected THIETKE ThietKe
        {
            get
            {
                if (Session["NHAPTHIETKE_MADDK"] == null)
                    return null;

                return tkDao.Get(Session["NHAPTHIETKE_MADDK"].ToString());
            }    
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_ThietKeVaVatTu, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TK_THIETKEBOCVATTU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_BOCVATTU;
            }

            CommonFunc.SetPropertiesForGrid(gvVatTu);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu);
            CommonFunc.SetPropertiesForGrid(gvGhiChu);
            CommonFunc.SetPropertiesForGrid(gvChiPhi);
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



        private void LoadReferences()
        {
            if (ThietKe == null)
                return;

            var list = mbvtDao.GetList();
            ddlMBVT.Items.Clear();

            ddlMBVT.Items.Add(new ListItem("", ""));

            lbTENKH.Text = _ddk.Get(ThietKe.MADDK.ToString()).TENKH;

            var index = -1;
            var i = 0;

            foreach(var item in list)
            {
                if (item.DUOCCHON.HasValue && item.DUOCCHON.Value)
                    index = i;
                
                i++;
    
                ddlMBVT.Items.Add(new ListItem(item.TENTK, item.MADDK));
            }

            if(index > -1)
            {
                ddlMBVT.Items.RemoveAt(0);
                ddlMBVT.SelectedIndex = index;
            }
        }

        protected void btnChange_Click(object sender, EventArgs e)
        {
            var mbvt = mbvtDao.Get(ddlMBVT.SelectedValue);
            tkDao.ChangeFromMBVT(ThietKe, mbvt);

            BindSelectedVatTuGrid();
            BindChiPhi();
            BindGhiChu();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var url = ResolveUrl("~") + "Forms/ThietKe/NhapThietKe.aspx";
            Response.Redirect(url, false);
        }

        private void BindVatTu()
        {
            var list = vtDao.Search(txtFilterVatTu.Text.Trim());
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

                        if(vt != null)
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
            if (ThietKe == null)
                return;

            var list = cttkdao.GetList(ThietKe.MADDK);

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
                    if (ThietKe == null)
                    {
                        //CloseWaitingDialog();
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

        /*
        protected void btnAddVatTu_Click(object sender, EventArgs e)
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
                SOLUONG = 1,
                ISCTYDTU = chkIsCtyDauTu.Checked
            };

            cttkdao.Insert(cttk); 
            BindSelectedVatTuGrid();
            txtMAVT.Text = "";
            txtMAVT.Focus();

            CloseWaitingDialog();
        }
        */




        private void BindGhiChu()
        {
            if (ThietKe == null)
                return;

            var list = gctkdao.GetList(ThietKe.MADDK);

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
                if (ThietKe == null)
                {
                    CloseWaitingDialog(); 
                    return;
                }

                var gcmbvt = new GCTHIETKE
                {
                    MAMBVT = ThietKe.MADDK,
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
            if (ThietKe == null)
                return;

            var list = dltkDao.GetList(ThietKe.MADDK);

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

                var cptk = new DAOLAPTK
                {
                    MADDK = ThietKe.MADDK,
                    NOIDUNG = "Vận chuyển",
                    DONGIACP = 5500,
                    SOLUONG = 1,
                    DVT = dvtList[8].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 5500,
                    LOAICP = "VC",
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
            Session["NHAPTHIETKE_MADDK"] = ThietKe.MADDK;
            Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpVTTKBVT.aspx",
                                   false);
            CloseWaitingDialog();
            
        }
    }  
}
