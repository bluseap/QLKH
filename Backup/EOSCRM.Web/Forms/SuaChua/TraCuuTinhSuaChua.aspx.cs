using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.SuaChua
{
    public partial class TraCuuTinhSuaChua : Authentication
    {
        /*
        private readonly GiaiQuyetThongTinSuaChuaDao _gqttsuDao = new GiaiQuyetThongTinSuaChuaDao();

        private readonly ChietTinhSuaChuaDao _ctSuaChuaDao = new ChietTinhSuaChuaDao();
        private readonly NhanVienDao nvDao = new NhanVienDao();

        private readonly DaoLapChietTinhSuaChuaDao _dlDao = new DaoLapChietTinhSuaChuaDao();
        private readonly ChiTietChietTinhSuaChuaDao _ctctDao = new ChiTietChietTinhSuaChuaDao();
        private readonly GhiChuChietTinhSuaChuaDao _gcDao = new GhiChuChietTinhSuaChuaDao();
        

        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly KhuVucDao khuvucDao = new KhuVucDao();
     
        #region Properties

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

        protected String AreaCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_AREACODE))
                {
                    return null;
                }

                return EncryptUtil.Decrypt(param[Constants.PARAM_AREACODE].ToString());
            }
        }


        protected CHIETTINHSUACHUA ChietTinhSuaChua
        {
            get {
                try { return _ctSuaChuaDao.Get(MADON); }
                catch { return null; } 
            }
        }

        private string MADON
        {
            get { return Session["LAPCHIETTINH_MADON"].ToString(); }
            set { Session["LAPCHIETTINH_MADON"] = value; }
        }
        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_TraCuuChietTinhSuaChua, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    //TODO: bind data
                    BindDataForGrid(gvList);

                    upnlEditCustomer.Visible = false;
                    upnlCustomers.Visible = false;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_SC_TRACUUCHIETTINHSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_TRACUUCHIETTINHSUACHUA;
            }

          
        }

        #region Startup script registeration
#pragma warning disable 114,108
        private void RegisterStartupScript(string key, string script)
#pragma warning restore 114,108
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), key, script, true);
        }

        private void SetTextBox(string id, string value)
        {
            RegisterStartupScript("jsSetTextBoxText" + id, "setTextBoxText('" + id + "', '" + value + "');");
        }

        private void HideDialog(string divId)
        {
            RegisterStartupScript("jscloseDialog" + divId, "closeDialog('" + divId + "');");
        }

        private void SetControlValue(string id, string value)
        {
            RegisterStartupScript(string.Format("jsSetValueForControl-{0}-{1}", id, Guid.NewGuid()),
                string.Format("setControlValue('{0}', '{1}');", id, value));
        }

        private void SetLabel(string id, string value)
        {
            RegisterStartupScript("jssetLabelText" + id, "setLabelText('" + id + "', '" + value + "');");
        }

        private void ShowError(string message, string controlId)
        {
            RegisterStartupScript("jsShowError" + controlId, "showError('" + message + "', '" + controlId + "');");
        }

        private void showWarning(string message)
        {
            RegisterStartupScript("jsShowWarning" + Guid.NewGuid(), "showWarning('" + message + "');");
        }

        private void ShowInfor(string message)
        {
            RegisterStartupScript("jsShowInfor" + Guid.NewGuid(), "showInfor('" + message + "');");
        }
        #endregion

        private void BindDataForGrid(BaseDataBoundControl grid)
        {
            try
            {

                var objList = _gqttsuDao.GetListDonCoLapChietTinh(Keyword, FromDate, ToDate, AreaCode);

                grid.DataSource = objList;
                grid.DataBind();

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message,
                                                        ex.StackTrace));
            }
        }

        private void BindSelectedVatTuGrid()
        {
            if (ChietTinhSuaChua == null) return;

            var list = _ctctDao.GetList(MADON);

            gvSelectedVatTu.DataSource = list;
            gvSelectedVatTu.DataBind();
        }

        private void BindChiPhi()
        {
            if (ChietTinhSuaChua == null) return;

            var list = _dlDao.GetList(MADON);

            gvChiPhi.DataSource = list;
            gvChiPhi.DataBind();
        }

        private void BindGhiChu()
        {
            if (ChietTinhSuaChua == null) return;

            var list = _gcDao.GetList(MADON);

            gvGhiChu.DataSource = list;
            gvGhiChu.DataBind();
        }

        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "LapChietTinh":
                        // create chiet tinh
                        var nhanvien = nvDao.Get(LoginInfo.Username);
                        if (nhanvien == null) return;

                        MADON = id;

                        GIAIQUYETTHONGTINSUACHUA objGqTtSc = _gqttsuDao.Get(MADON);


                        ChayChietTinh(objGqTtSc, nhanvien);

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
                // Update page index
                gvList.PageIndex = e.NewPageIndex;
                // Bind data for grid
                BindDataForGrid(gvList);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
     

        private void ChayChietTinh(GIAIQUYETTHONGTINSUACHUA objGqTtSc, NHANVIEN nv)
        {
            var tontaichiettinhsuachua = _ctSuaChuaDao.Get(objGqTtSc.MADON);
            if (tontaichiettinhsuachua == null)
            {
                MsgBox.Show("Chiết tính chưa chạy.");

                gvSelectedVatTu.DataSource = null;
                gvSelectedVatTu.DataBind();

                gvChiPhi.DataSource = null;
                gvChiPhi.DataBind();

                gvGhiChu.DataSource = null;
                gvGhiChu.DataBind();

                ChietTinhInfo.Visible = true;
                upnlEditCustomer.Visible = true;
                upnlCustomers.Visible = true;
                txtTENCT.Text = "";
                txtDiaChi.Text = "";
                 

                gvList.Visible = false;
                filterPanel.Visible = false;
           
            }
            else
            {
                ChietTinhInfo.Visible = true;
                upnlEditCustomer.Visible = true;
                upnlCustomers.Visible = true;
                txtTENCT.Text = tontaichiettinhsuachua.TENCT;
                txtDiaChi.Text = tontaichiettinhsuachua.DIACHIHM;
                // reload grid
                BindDataForGrid(gvList);

                BindSelectedVatTuGrid();
                BindChiPhi();
                BindGhiChu();

                gvList.Visible = false;
                filterPanel.Visible = false;
            }
        }

       
        protected void btnBrowseVatTu_Click(object sender, EventArgs e)
        {
            RegisterStartupScript("jsUnblockDialog", "unblockDialog();");
        }

   
        protected void gvSelectedVatTu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
                case "DeleteVatTu":
                    if (ChietTinhSuaChua == null) return;

                    var deletingCTCT = _ctctDao.Get(ChietTinhSuaChua.MADON, mavt);
                    if (deletingCTCT == null) return;

                    _ctctDao.Delete(deletingCTCT);

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

            if (txtSL == null || txtGIAVT == null || txtGIANC == null ||
                lblTIENNC == null || lblTIENVT == null) return;

            var source = gvSelectedVatTu.DataSource as List<CTCHIETTINHSUACHUA>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MAVT;
            var maddk = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MADON ;

            var script = "javascript:updateCTCT(\"" + maddk + "\", \"" + mavt + 
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID + 
                                                        "\")";
            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);
        }

        protected void btnAddGhiChu_Click(object sender, EventArgs e)
        {
            try
            {
                if (ChietTinhSuaChua == null) return;

                var gcct = new GHICHUSUACHUA()
                {
                    MADON  = ChietTinhSuaChua.MADON ,
                    NOIDUNG = ""
                };
                _gcDao.Insert(gcct);
                BindGhiChu();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvGhiChu_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var magc = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
                case "DeleteGhiChu":
                    var deletingGC = _gcDao.Get(Int32.Parse(magc));
                    if (deletingGC == null) return;

                    _gcDao.Delete(deletingGC);

                    BindGhiChu();

                    break;
            }
        }

        protected void gvGhiChu_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtNOIDUNG") as TextBox;
            if (txtSL == null) return;

            var source = gvGhiChu.DataSource as List<GHICHUSUACHUA>;
            if (source == null) return;

            var magc = source[e.Row.RowIndex + gvGhiChu.PageSize * gvGhiChu.PageIndex].MAGC;

            var script = "javascript:updateGCCT(\"" + magc + "\", \"" + txtSL.ClientID + "\")";
            txtSL.Attributes.Add("onblur", script);

            
        }

        protected void btnAddChiPhi_Click(object sender, EventArgs e)
        {
            try
            {
                if (ChietTinhSuaChua == null) return;

                var cpct = new DAOLAPSUACHUA()
                {
                    MADON = ChietTinhSuaChua.MADON,
                    NOIDUNG = "",
                    DONGIACP = 0,
                    SOLUONG = 1,
                    DVT = "M2",
                    HESOCP = 1,
                    THANHTIENCP = 0,
                    LOAICP = "DAO",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP"
                };
                _dlDao.Insert(cpct);
                BindChiPhi();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvChiPhi_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var macp = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                //Can't use just Edit and Delete or need to bypass RowEditing and Deleting
                case "DeleteChiPhi":
                    var deletingCPCT = _dlDao.Get(Int32.Parse(macp));
                    if (deletingCPCT == null) return;

                    _dlDao.Delete(deletingCPCT);

                    BindChiPhi();

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
            var txtTT = e.Row.FindControl("txtTHANHTIENCP") as TextBox;
            var ddlLCP = e.Row.FindControl("ddlLOAICP") as DropDownList;

            if (txtND == null || txtDG == null || ddlDVT == null || txtSL == null ||
                txtHS == null || txtTT == null || ddlLCP == null) return;

            var source = gvChiPhi.DataSource as List<DAOLAPSUACHUA>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi.PageSize * gvChiPhi.PageIndex].MA;


            var script = "javascript:updateCPCT(\"" + madon + "\", \"" + txtND.ClientID +
                                                                "\", \"" + txtDG.ClientID +
                                                                "\", \"" + ddlDVT.ClientID +
                                                                "\", \"" + txtSL.ClientID +
                                                                "\", \"" + txtHS.ClientID +
                                                                "\", \"" + txtTT.ClientID +
                                                                "\", \"" + ddlLCP.ClientID +
                                                                "\")";
            txtND.Attributes.Add("onblur", script);
            txtDG.Attributes.Add("onblur", script);
            txtSL.Attributes.Add("onblur", script);
            txtHS.Attributes.Add("onblur", script);
            txtTT.Attributes.Add("onblur", script);
            ddlDVT.Attributes.Add("onchange", script);
            ddlLCP.Attributes.Add("onchange", script);
        }

        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            BindSelectedVatTuGrid();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
             
            CHIETTINHSUACHUA objUi = _ctSuaChuaDao.Get(MADON);
            objUi.TENCT = txtTENCT.Text.Trim();
            objUi.DIACHIHM = txtDiaChi.Text.Trim();

            _ctSuaChuaDao.Update(objUi, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

            MsgBox.Show("Cập nhật thông tin thành công.");
        }
        */

        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();
        private readonly GiaiQuyetThongTinSuaChuaDao _gqttsuDao = new GiaiQuyetThongTinSuaChuaDao();
        private readonly ChietTinhSuaChuaDao ctDao = new ChietTinhSuaChuaDao();


        #region Properties

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

        protected String StateCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_STATECODE))
                {
                    return null;
                }

                var res = EncryptUtil.Decrypt(param[Constants.PARAM_STATECODE].ToString());


                return res == "NULL" ? null : res;
            }
        }

        protected String AreaCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_AREACODE))
                {
                    return null;
                }

                return EncryptUtil.Decrypt(param[Constants.PARAM_AREACODE].ToString());
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

        protected GIAIQUYETTHONGTINSUACHUA GQTTSC
        {
            get
            {
                try { return (GIAIQUYETTHONGTINSUACHUA)Session["TCCTSC_GQTTSC"]; }
                catch { return null; }
            }

            set { Session["TCCTSC_GQTTSC"] = value; }
        }

        protected CHIETTINHSUACHUA ChietTinh
        {
            get
            {
                try { return (CHIETTINHSUACHUA)Session["TCCTSC_CTSC"]; }
                catch { return null; }
            }
            set { Session["TCCTSC_CTSC"] = value; }
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_TraCuuChietTinhSuaChua, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_SC_TRACUUCHIETTINHSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_TRACUUCHIETTINHSUACHUA;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }

        #region Startup script registeration
        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        #endregion




        private void BindDataForGrid()
        {
            try
            {
                var objList = _gqttsuDao.GetListForTraCuuChietTinh(Keyword, FromDate, ToDate, StateCode, AreaCode);

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }



        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (lastCell == null) return;

            var source = gvList.DataSource as List<DUYETCHIETTINHSUACHUA>;
            if (source == null) return;

            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;

            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = _gqttsuDao.Get(source[index].MADON);
                var dct = source[index];


                if (imgTT != null && ddk != null)
                {
                    imgTT.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTT) + "')");

                    var maTTCT = dct.TTCT;
                    var ttct = ttDao.Get(maTTCT);

                    if (ttct != null)
                    {
                        imgTT.Attributes.Add("class", ttct.COLOR);
                        imgTT.ToolTip = ttct.TENTT;
                    }
                    else
                    {
                        imgTT.ToolTip = "Chưa duyệt chiết tính";
                        imgTT.Attributes.Add("class", "noneIndicator");
                    }
                }
            }
            catch { }

            var lnkBtnIDReport = e.Row.FindControl("lnkBtnIDReport") as LinkButton;
            if (lnkBtnIDReport == null) return;
            lnkBtnIDReport.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnIDReport) + "')");

            var lnkBtnIDEdit = e.Row.FindControl("lnkBtnIDEdit") as LinkButton;
            if (lnkBtnIDEdit == null) return;
            lnkBtnIDEdit.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnIDEdit) + "')");

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
                        Session["LAPCHIETTINHSUACHUA_MADDK"] = id;
                        //var gq = _gqttsuDao.Get(id);
                        //if (gq.TTCT == "CT_P")
                        //{
                        //    Session["LAPCHIETTINHSUACHUA_MADDK"] = null;
                        //    return;
                        //}
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/SuaChua/LapChietTinhSuaChua.aspx", false);

                        CloseWaitingDialog();

                        break;

                    case "ReportItem":
                        Session["LAPCHIETTINHSUACHUA_MADDK"] = id;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpLapChietTinhSuaChua.aspx",
                                               false);

                        CloseWaitingDialog();

                        break;

                    case "showCTStatus":
                        if (!string.Empty.Equals(id))
                        {
                            GQTTSC = _gqttsuDao.Get(id);
                            ChietTinh = ctDao.Get(id);

                            txtGHICHU.Text = ChietTinh.GHICHU;

                            upnlChietTinh.Update();
                            UnblockDialog("divChietTinh");
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
    }
}
