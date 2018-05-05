using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.IO;

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

        string khuvuc, filenametk1, filenametk2, duongdantk1, duongdantk2;

        decimal PageTotal_ExtendedPrice = 0;
        decimal GrandTotal_ExtendedPrice = 0;

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

        private void LoadReferences()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;           
            string manv = loginInfo.Username;
            string makvnv = _nvDao.Get(manv).MAKV;

            if (ThietKe == null)
                return;

            var donsc = _scDao.Get(ThietKe.MADDK.ToString());
            var madon2 = _ddk.Get(ThietKe.MADDK.ToString());            
            if ( madon2 != null)
            {
                var list = mbvtDao.GetList();
                //var list = mbvtDao.GetListMAKV(makvnv);
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

                var listMTK = _mtkDao.GetListFromOrder9();
                ddlMBTHIETKE.Items.Clear();
                ddlMBTHIETKE.Items.Add(new ListItem("Chọn mẫu TK", "DLMK"));
                foreach (var mtk in listMTK)
                    ddlMBTHIETKE.Items.Add(new ListItem(mtk.TENMAUTK, mtk.MAMAUTK));

                LoadMauTK();

                var thieke = tkDao.Get(ThietKe.MADDK);
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
                if (ThietKe.DANHSOTRAI != null)
                {
                    txtDANHSOBT.Text = ThietKe.DANHSOTRAI.ToString();
                }
                else { txtDANHSOBT.Text = ""; }

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
            }

            if (donsc != null)
            {
                var list = mbvtDao.GetList();
                ddlMBVT.Items.Clear();
                ddlMBVT.Items.Add(new ListItem("", ""));

                var scid = _scDao.Get(ThietKe.MADDK.ToString());
                lbTENKH.Text = _khDao.Get(scid.IDKH.ToString()).TENKH;

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

                var listMTK = _mtkDao.GetList();
                ddlMBTHIETKE.Items.Clear();
                ddlMBTHIETKE.Items.Add(new ListItem("Chọn mẫu TK", "ALL"));
                foreach (var mtk in listMTK)
                    ddlMBTHIETKE.Items.Add(new ListItem(mtk.TENMAUTK, mtk.MAMAUTK));                

                var thieke = tkDao.Get(ThietKe.MADDK);
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
                if (ThietKe.DANHSOTRAI != null)
                {
                    txtDANHSOBT.Text = ThietKe.DANHSOTRAI.ToString();
                }
                else { txtDANHSOBT.Text = ""; }

                LoadMauTK();
            }

        }

        private void LoadMauTK()
        {
            var mautk = tkDao.Get(ThietKe.MADDK).MAMAUTK;
            if (mautk != null)
            {
                var item10 = ddlMBTHIETKE.Items.FindByValue(mautk);
                if (item10 != null)
                    ddlMBTHIETKE.SelectedIndex = ddlMBTHIETKE.Items.IndexOf(item10);
            }
            else
            {
                var listMTK = _mtkDao.GetList();
                ddlMBTHIETKE.Items.Clear();
                ddlMBTHIETKE.Items.Add(new ListItem("Chọn mẫu TK", "ALL"));
                foreach (var mtk in listMTK)
                    ddlMBTHIETKE.Items.Add(new ListItem(mtk.TENMAUTK, mtk.MAMAUTK));
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
            try
            {
                //var url = ResolveUrl("~") + "Forms/ThietKe/NhapThietKe.aspx";
                //Response.Redirect(url, false);
                
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
                }

                if (UpHINH2.PostedFile.ContentLength > 0)
                {
                    filenametk2 = Path.GetFileName(UpHINH2.PostedFile.FileName);
                    duongdantk2 = "UpLoadFile/" + khuvuc + "/hinhthietke/" + ngaygio + filenametk2;
                    UpHINH2.SaveAs(Server.MapPath("~/" + duongdantk2));
                }

                var tk = new THIETKE
                {
                    MADDK = ThietKe.MADDK,
                    HINHTK1 =  "~/" + duongdantk1,
                    HINHTK2 = "~/" + duongdantk2
                };
                tkDao.Update(tk, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

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

        private void BindVatTu()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string manv = loginInfo.Username;
                string makvnv = _nvDao.Get(manv).MAKV;

                if (makvnv == "S")
                {
                    //var list = vtDao.Search(txtFilterVatTu.Text.Trim());   
                    var list = vtDao.SearchMAKVAll(txtFilterVatTu.Text.Trim(), makvnv);

                    gvVatTu.DataSource = list;
                    gvVatTu.PagerInforText = list.Count.ToString();
                    gvVatTu.DataBind();
                }
                else
                {
                    var list = vtDao.Search(txtFilterVatTu.Text.Trim());   

                    gvVatTu.DataSource = list;
                    gvVatTu.PagerInforText = list.Count.ToString();
                    gvVatTu.DataBind();
                }
            }
            catch { }
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

        protected void btnFilterVatTu_Click(object sender, EventArgs e)
        {
            BindVatTu();
            CloseWaitingDialog();
        }

        /*protected void gvSelectedVatTu_RowCreated(Object sender, GridViewRowEventArgs e)
        {

            // Retrieve the current row. 
            GridViewRow row = e.Row;

            // Update the column total if the row being created is
            // a footer row.
            //GrandTotal_ExtendedPrice = Convert.ToDecimal(vtDao.SumGIAVT(txtFilterVatTu.Text.Trim()));
            GrandTotal_ExtendedPrice = Convert.ToDecimal("77");
            
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int ExtendedPrice = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TIENVT"));
                PageTotal_ExtendedPrice += ExtendedPrice;
            }

            if (row.RowType == DataControlRowType.Header)
            //if (e.Row.RowType != DataControlRowType.Footer)
            {
                int intMergeCol = e.Row.Cells.Count - 1;
                int intCellCol = 1;
                for (intCellCol = 1; intCellCol <= intMergeCol - 2; intCellCol++)
                {
                    e.Row.Cells.RemoveAt(1);
                }
                e.Row.Cells[0].ColumnSpan = 7;

                var lblPageTotal = e.Row.FindControl("lblPageTotal") as Label;
                var lblGrandTotal = e.Row.FindControl("lblGrandTotal") as Label;
                if ((lblPageTotal != null))
                {
                    //lblPageTotal.Text = PageTotal_ExtendedPrice.ToString("N2");
                    lblPageTotal.Text = "55";
                }

                if ((lblGrandTotal != null))
                {
                    lblGrandTotal.Text = GrandTotal_ExtendedPrice.ToString("N2");
                }
            }

        }*/

        private void BindSelectedVatTuGrid()
        {
            if (ThietKe == null)
                return;

            var list = cttkdao.GetList(ThietKe.MADDK);  

            gvSelectedVatTu.DataSource = list;
            gvSelectedVatTu.PagerInforText = list.Count.ToString();
            gvSelectedVatTu.DataBind();

            upnlMBVT.Update();
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
            
            //var lblPageTotal = e.Row.FindControl("lblPageTotal") as Label;
            //var lblGrandTotal = e.Row.FindControl("lblGrandTotal") as Label;



            /*if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int ExtendedPrice = Convert.ToInt32(DataBinder.Eval(e.Row.DataItem, "TIENVT"));
                PageTotal_ExtendedPrice += ExtendedPrice;
            }

            GridViewRow row = e.Row;
            if (row.RowType == DataControlRowType.Footer)
            //if (e.Row.RowType != DataControlRowType.Footer)
            {
                int intMergeCol = e.Row.Cells.Count - 1;
                int intCellCol = 1;
                for (intCellCol = 1; intCellCol <= intMergeCol - 2; intCellCol++)
                {
                    e.Row.Cells.RemoveAt(1);
                }
                e.Row.Cells[0].ColumnSpan = 7;

                var lblPageTotal = e.Row.FindControl("lblPageTotal") as Label;
                var lblGrandTotal = e.Row.FindControl("lblGrandTotal") as Label;
                if ((lblPageTotal != null))
                {
                    //lblPageTotal.Text = PageTotal_ExtendedPrice.ToString("N2");
                    lblPageTotal.Text = "55";
                }

                if ((lblGrandTotal != null))
                {
                    lblGrandTotal.Text = GrandTotal_ExtendedPrice.ToString("N2");
                }
            }*/



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
            //if (ThietKe == null)
            //{
            //    CloseWaitingDialog();
            //    return;
            //}

            //if (txtMAVT.Text.Trim() == "")
            //{
            //    CloseWaitingDialog();
            //    ShowError("Vui lòng nhập mã vật tư", txtMAVT.ClientID);
            //    return;
            //}

            //try
            //{
            //    decimal.Parse(txtKHOILUONG.Text.Trim());
            //}
            //catch
            //{
            //    CloseWaitingDialog();
            //    ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khối lượng"), txtKHOILUONG.ClientID);
            //    return;
            //}

            //var vt = vtDao.Get(txtMAVT.Text.Trim());
            //if (vt == null)
            //{
            //    CloseWaitingDialog();
            //    ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.");
            //    return;
            //}

            //// add to grid
            //var cttk = new CTTHIETKE
            //{
            //    MADDK = ThietKe.MADDK,
            //    MAVT = vt.MAVT,
            //    NOIDUNG = vt.TENVT,
            //    SOLUONG = decimal.Parse(txtKHOILUONG.Text.Trim()),
            //    GIAVT = vt.GIAVT.HasValue ? vt.GIAVT.Value : 0,
            //    GIANC = vt.GIANC.HasValue ? vt.GIANC.Value : 0,
            //    TIENVT = vt.GIAVT.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIAVT.Value : 0,
            //    TIENNC = vt.GIANC.HasValue ? decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIANC.Value : 0,
            //    ISCTYDTU = chkIsCtyDauTu.Checked
            //};

            //Message msg;
            //msg = cttkdao.Insert(cttk);

            //BindSelectedVatTuGrid();

            //txtMAVT.Text = "";
            //txtKHOILUONG.Text = "";
            //lblTENVT.Text = "";
            //FocusAndSelect(txtMAVT.ClientID);

            //CloseWaitingDialog();   
            //upnlMBVT.Update();

            lnkChangeKhoiLuong();
        }

        private void lnkChangeKhoiLuong()
        {
            if (ThietKe == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (txtMAVT.Text.Trim() == "")
            {
                //CloseWaitingDialog();
                //ShowError("Vui lòng nhập mã vật tư", txtMAVT.ClientID);
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

            Message msg;
            msg = cttkdao.Insert(cttk);

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

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {            
            Session["NHAPTHIETKE_MADDK"] = ThietKe.MADDK;
            Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpVTTKBVT.aspx", false);
            CloseWaitingDialog();            
        }        

        protected void ddlMBTHIETKE_SelectedIndexChanged1(object sender, EventArgs e)
        {
            SaveMauThietke();

           /* if (mamtk.ToString() == "ALL")
            {
                ShowError("Chọn mẫu thiết kế...");
            }
            else
            {
                try
                {
                    _rpC.UPMAUTK(maddk.ToString(), mamtk.ToString(),
                        string.Empty.Equals(txtTENKHBP.Text) ? " " : txtTENKHBP.Text.ToString(),
                        string.Empty.Equals(txtTENKHBT.Text) ? " " : txtTENKHBT.Text.ToString(),
                        string.Empty.Equals(txtDANHSOBP.Text) ? " " : txtDANHSOBP.Text.ToString(),
                        string.Empty.Equals(txtDANHSOBT.Text) ? " " : txtDANHSOBT.Text.ToString());
                    ShowInFor("Cập nhật sơ đồ thiết kế thành công..");
                }
                catch { ShowError("Lỗi cập nhật mẫu sơ đồ thiết kế."); }
            }*/
            
        }

        private void SaveMauThietke()
        {
            try
            {
                var mamtk = ddlMBTHIETKE.SelectedValue;
                var maddk = ThietKe.MADDK;

                _rpC.UPMAUTK(maddk.ToString(), mamtk.ToString(),
                    string.Empty.Equals(txtTENKHBP.Text) ? " " : txtTENKHBP.Text.ToString(),
                    string.Empty.Equals(txtTENKHBT.Text) ? " " : txtTENKHBT.Text.ToString(),
                    string.Empty.Equals(txtDANHSOBP.Text) ? " " : txtDANHSOBP.Text.ToString(),
                    string.Empty.Equals(txtDANHSOBT.Text) ? " " : txtDANHSOBT.Text.ToString());

                ShowInFor("Cập nhật sơ đồ thiết kế thành công..");
            }
            catch 
            { 
                ShowError("Lỗi cập nhật mẫu sơ đồ thiết kế."); 
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

        protected void btAddVatTu_Click(object sender, EventArgs e)
        {
            try
            {
                lnkChangeKhoiLuong();

                CloseWaitingDialog();
                upnlMBVT.Update();
            }
            catch (Exception ex) 
            { 
                ShowError(ex.ToString()); 
            }
        }

        protected void btSaveMauThietKe_Click(object sender, EventArgs e)
        {
            try
            {
                SaveMauThietke();
            }
            catch { }
        }

        protected void txtKHOILUONG_TextChanged(object sender, EventArgs e)
        {
            try
            {
                lnkChangeKhoiLuong();

                CloseWaitingDialog();
                upnlMBVT.Update();
            }
            catch (Exception ex)
            {
                ShowError(ex.ToString());
            }
        }



    }  
}
