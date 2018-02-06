using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThiCongCongTrinh
{
    public partial class LapQuyetToan : Authentication
    {
        //private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly DvtDao dvtDao = new DvtDao();
        private readonly MauBocVatTuDao mbvtDao = new MauBocVatTuDao();



        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ChietTinhDao ctDao = new ChietTinhDao();
        private readonly QuyetToanDao qtDao = new QuyetToanDao();
        private readonly ChiTietQuyetToanDao ctqtDao = new ChiTietQuyetToanDao();
        private readonly ChiTietQuyetToanNd117Dao ctqtNd117Dao = new ChiTietQuyetToanNd117Dao();

        private readonly NhanVienDao nvDao = new NhanVienDao();

        private readonly DaoLapQuyetToanDao dlqtDao = new DaoLapQuyetToanDao();
        private readonly DaoLapQuyetToanNd117Dao dlqtNd117Dao = new DaoLapQuyetToanNd117Dao();
        private readonly GhiChuQuyetToanDao gcqtDao = new GhiChuQuyetToanDao();


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
        protected bool FromReport
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_REPORTED))
                    return false;

                return true;
            }
        }

        protected QUYETTOAN QuyetToan
        {
            get
            {
                try { return qtDao.Get(MADDK); }
                catch { return null; }
            }
        }

        private string MADDK
        {
            get { return Session["LAPQUYETTOAN_MADDK_2"].ToString(); }
            set { Session["LAPQUYETTOAN_MADDK_2"] = value; }
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



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_LapQuyetToan, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();

                    // truong hop back tu form report
                    if (Session["LAPQUYETTOAN_MADDK"] != null && FromReport)
                    {
                        var ct = qtDao.Get(Session["LAPQUYETTOAN_MADDK"].ToString());
                        MADDK = Session["LAPQUYETTOAN_MADDK"].ToString();

                        divChietTinhInfo.Visible = true;

                        upnlCustomers.Visible = true;
                        upnlTongHopChiPhi.Visible = true;

                        BindSelectedVatTuGrid();
                        BindSelectedVatTu117Grid();
                        BindChiPhi();
                        BindChiPhi117();

                        divGridList.Visible = false;
                        filterPanel.Visible = false;

                        ShowThongTinHeSoChietTinh(ct);
                        
                        upnlTongHopChiPhi.Update();
                        upnlCustomers.Update();
                    }
                    else
                    {
                        UpdateMode = Mode.Create;

                        BindDataForGrid();

                        upnlVatTu.Visible = false;
                        upnlCustomers.Visible = false;
                        upnlTongHopChiPhi.Visible = false;
                    }

                    Session["LAPQUYETTOAN_MADDK"] = null;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TC_LAPQUYETTOAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_LAPQUYETTOAN;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvVatTu);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu);
            CommonFunc.SetPropertiesForGrid(gvChiPhi);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu117);
            CommonFunc.SetPropertiesForGrid(gvChiPhi117);
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

        private void FocusAndSelect(string controlId)
        {
            ((EOS)Page.Master).FocusAndSelect(controlId);
        }
        #endregion

        private void BindDataWhenClick(string ma)
        {
            // update TTQT
            var objDDK = ddkDao.Get(ma);
            objDDK.TTHC = TTQT.QT_P.ToString();
            ddkDao.UpdateTT(objDDK);

            // bind to gvselectedVatTu
            BindSelectedVatTuGrid();

            // bind to gvDaoLapMienPhi
            BindChiPhi();

            // bind to gvVatTu117
            BindSelectedVatTu117Grid();

            // bind to gvDaoLap117
            BindChiPhi117();

            // show cac cpnel
            divGridList.Visible = false;
            filterPanel.Visible = false;


            divChietTinhInfo.Visible = true;
            upnlVatTu.Visible = true;
            upnlCustomers.Visible = true;

            var qt = qtDao.Get(ma);
            ShowThongTinHeSoChietTinh(qt);
            upnlTongHopChiPhi.Visible = true;
            upnlTongHopChiPhi.Update();

        }
        
        protected void gvList_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditItem":
                        // create chiet tinh
                        var objTK = qtDao.Get(id);
                        if (objTK == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }
                        
                        var nhanvien = nvDao.Get(LoginInfo.Username);
                        if (nhanvien == null)
                        {
                            CloseWaitingDialog();
                            return;
                        }

                        MADDK = id;

                        BindDataWhenClick(MADDK);

                        CloseWaitingDialog();

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

            var lnkBtnID2 = e.Row.FindControl("lnkBtnID2") as LinkButton;
            if (lnkBtnID2 == null) return;
            lnkBtnID2.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID2) + "')");
        }

        
        private void ShowThongTinHeSoChietTinh(QUYETTOAN chiettinh)
        {
            //Hien thi thong tin dia chi chiet tinh

            txtTenHM.Text = chiettinh.TENHM;
            txtTENCT.Text = chiettinh.TENCT;
            lblDIACHI.Text = chiettinh.DONDANGKY.DIACHILD;
            txtGhiChu.Text = chiettinh.GHICHU;


            txtGIAMGIACPNC.Text = chiettinh.GIAMGIACPNC.ToString();
            txtGIAMGIACPVL.Text = chiettinh.GIAMGIACPVL.ToString();
            txtHSNHANCONG.Text = chiettinh.HSNHANCONG.ToString();
            txtHSTHIETKE3.Text = chiettinh.HSTHIETKE3.ToString();
            txtHSCPC.Text = chiettinh.HSCPC.ToString();
            txtHSCHUNG.Text = chiettinh.HSCHUNG.ToString();
            txtHSTHUNHAP.Text = chiettinh.HSTHUNHAP.ToString();
            txtHSTHIETKE1.Text = chiettinh.HSTHIETKE1.ToString();

            //hien thi thong tin cac he so cua chiet tinh
            var dt = new ReportClass().BaoCaoLapChietTinh(chiettinh.MADDK);

            //Chi phí vật liệu công ty đầu tư
            decimal VL = 0;
            //Chi phí vật liệu khách hàng
            decimal A = 0;
            //Chi phí nhân công cho hai trường hợp trên
            decimal B = 0;
            //Lấy giá trị tiền vật tư công ty, khách hàng, tiền nhân công
            foreach (DataRow dataRow in dt.Tables[0].Rows)
            {
                if (dataRow["THUTU"].ToString() == "1")
                {
                    VL = VL + (decimal)dataRow["TIENVT"];
                }

                if (dataRow["THUTU"].ToString() == "2")
                {
                    A = A + (decimal)dataRow["TIENVT"];
                }

                B = B + (decimal)dataRow["TIENNC"];
            }
            //Trường hợp có giảm giá chi phí vật liệu 
            if (chiettinh.GIAMGIACPVL != null && chiettinh.GIAMGIACPVL > 0)
            {
                VL = VL - VL * (decimal)chiettinh.GIAMGIACPVL / 100;
                A = A - A * (decimal)chiettinh.GIAMGIACPVL / 100;
            }

            VL = Math.Round(VL, 0, MidpointRounding.ToEven);
            A = Math.Round(A, 0, MidpointRounding.ToEven);
            //Trường hợp có giảm giá chi phí nhân công)
            if (chiettinh.GIAMGIACPNC != null && chiettinh.GIAMGIACPNC > 0)
            {
                B = B - B * (decimal)chiettinh.GIAMGIACPNC / 100;
            }
            B = Math.Round(B, 0, MidpointRounding.ToEven);

            //Chi phí nhân công
            decimal NC = 0;
            if(chiettinh.HSNHANCONG != null)
                NC = B * (decimal)chiettinh.HSNHANCONG;
            NC = Math.Round(NC, 0, MidpointRounding.ToEven);

            //Chi phí trực tiếp khác 
            decimal TT = 0;
            if (chiettinh.HSCPC != null)
                TT = (VL + A + NC) * (decimal)chiettinh.HSCPC;
            TT = Math.Round(TT, 0, MidpointRounding.ToEven);

            //Cộng chi phí trực tiếp
            decimal T = 0;
            T = VL + A + NC + TT;
            T = Math.Round(T, 0, MidpointRounding.ToEven);


            //Chi phí chung
            decimal C = 0;
            if (chiettinh.HSCHUNG != null)
                C = T * (decimal)chiettinh.HSCHUNG;
            C = Math.Round(C, 0, MidpointRounding.ToEven);


            //Thu nhập chịu thuế tính trước
            decimal TL = 0;
            if (chiettinh.HSTHUNHAP != null)
                TL = (A + NC + TT + C) * (decimal)chiettinh.HSTHUNHAP;
            TL = Math.Round(TL, 0, MidpointRounding.ToEven);

            //Thuế giá trị gia tăng đầu ra
            decimal VAT = 0;
            if (chiettinh.HSTHUE != null)
                VAT = (A + NC + TT + C + TL) * (decimal)chiettinh.HSTHUE / 100;
            VAT = Math.Round(VAT, 0, MidpointRounding.ToEven);

            //Khảo sát phí
            decimal KS = 0;
            if (chiettinh.HSTHIETKE1 != null && chiettinh.HSTHIETKE2 != null)
                KS = (T + C + TL) * (decimal)chiettinh.HSTHIETKE1 * (decimal)chiettinh.HSTHIETKE2;
            KS = Math.Round(KS, 0, MidpointRounding.ToEven);

            //Giá trị dự tóan 
            decimal GTDT = A + NC + TT + C + TL + VAT + KS;
            GTDT = Math.Round(GTDT, 0, MidpointRounding.ToEven);


            lblChiPhiVatLieu.Text = VL.ToString();
            lblChiPhiKhachHang.Text = A.ToString();
            lblChiPhiNhanCong.Text = NC.ToString();
            lblCPC.Text = TT.ToString();
            lblCongChiPhiTrucTiep.Text = T.ToString();
            lblCPCHUNG.Text = C.ToString();
            lblCPTHUNHAP.Text = TL.ToString();
            lblThue.Text = VAT.ToString();
            lblCPTHIETKE.Text = KS.ToString();
            lblTONGST.Text = GTDT.ToString();
        }

        private void LoadStaticReferences()
        {
            var list = mbvtDao.GetList();

            var index = -1;
            var i = 0;
        }


        private readonly DonDangKyDao ddk = new DonDangKyDao();

        private void BindDataForGrid()
        {
            try
            {

                var objList = ddk.GetListForLapQuyetToan(Keyword, FromDate, ToDate, null);
                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private bool IsValidate()
        {
            try
            {
                decimal.Parse(txtGIAMGIACPNC.Text);
                decimal.Parse(txtGIAMGIACPVL.Text);
                decimal.Parse(txtHSNHANCONG.Text);
                decimal.Parse(txtHSTHIETKE3.Text);
                decimal.Parse(txtHSCPC.Text);
                decimal.Parse(txtHSCHUNG.Text);
                decimal.Parse(txtHSTHUNHAP.Text);
                decimal.Parse(txtHSTHIETKE1.Text);
            }
            catch
            {
                ShowError("Các thông số nhập vào không đúng, vui lòng nhập lại");
                return false;
            }

            return true;
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            var objUi = qtDao.Get(MADDK);

            objUi.TENCT = txtTENCT.Text.Trim();
            objUi.DIACHIHM = lblDIACHI.Text.Trim();
            objUi.TENHM = txtTenHM.Text.Trim();
            objUi.GHICHU = txtGhiChu.Text.Trim();
            var msg = qtDao.Update(objUi, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

            CloseWaitingDialog();

            if (msg != null)
            {
                if (msg.MsgType != MessageType.Error)
                {
                    ShowInfor("Cập nhật thông tin thành công.");
                }
                else
                {
                    ShowError("Cập nhật thông tin không thành công.");
                }
            }
        }

        protected void btnCapNhatGiaTri_Click(object sender, EventArgs e)
        {
            if (!IsValidate())
            {
                CloseWaitingDialog();
                return;
            }


            QUYETTOAN objUi = qtDao.Get(MADDK);

            objUi.GIAMGIACPNC = decimal.Parse(txtGIAMGIACPNC.Text);
            objUi.GIAMGIACPVL = decimal.Parse(txtGIAMGIACPVL.Text);
            objUi.HSNHANCONG = decimal.Parse(txtHSNHANCONG.Text);
            objUi.HSTHIETKE3 = decimal.Parse(txtHSTHIETKE3.Text);
            objUi.HSCPC = decimal.Parse(txtHSCPC.Text);
            objUi.HSCHUNG = decimal.Parse(txtHSCHUNG.Text);
            objUi.HSTHUNHAP = decimal.Parse(txtHSTHUNHAP.Text);
            objUi.HSTHIETKE1 = decimal.Parse(txtHSTHIETKE1.Text);

            qtDao.Update(objUi, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

            var chiettinhtontai = qtDao.Get(objUi.MADDK);
            ShowThongTinHeSoChietTinh(chiettinhtontai);

            CloseWaitingDialog();
            ShowInfor("Cập nhật thông tin thành công.");

            upnlTongHopChiPhi.Update();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            if (MADDK != null)
            {
                Session["LAPQUYETTOAN_MADDK"] = MADDK;
                Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpLapQuyetToan.aspx",
                                       false);
            }
            
            CloseWaitingDialog();
        }

        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            var objUi = qtDao.Get(MADDK);

            ShowThongTinHeSoChietTinh(objUi);
            upnlTongHopChiPhi.Update();

            CloseWaitingDialog();
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
            UpdateMode = Mode.Create;
            UnblockDialog("divVatTu");
        }

        protected void btnBrowseVatTu117_Click(object sender, EventArgs e)
        {
            BindVatTu();
            upnlVatTu.Update();
            UpdateMode = Mode.Update;
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

                        switch (UpdateMode)
                        {
                            case Mode.Create:
                                SetControlValue(txtMAVT.ClientID, id);
                                break;
                            case Mode.Update:
                                SetControlValue(txtMAVT117.ClientID, id);
                                break;
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
            if (QuyetToan == null) return;

            var list = ctqtDao.GetList(MADDK);

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
                    if (QuyetToan == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }

                    var deletingCTCT = ctqtDao.Get(QuyetToan.MADDK, mavt);
                    if (deletingCTCT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    ctqtDao.Delete(deletingCTCT);

                    BindSelectedVatTuGrid();
                    
                    upnlCustomers.Update();

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

            if (txtSL == null || txtGIAVT == null || txtGIANC == null ||
                lblTIENNC == null || lblTIENVT == null) return;

            var source = gvSelectedVatTu.DataSource as List<CTQUYETTOAN>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MAVT;
            var maddk = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MADDK;

            var script = "javascript:UpdateCTCTQT(\"" + maddk + "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\")";
            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);

            //var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            //if (btnDeleteItem == null) return;
            //btnDeleteItem.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }
        
        private void BindChiPhi()
        {
            if (QuyetToan == null) return;

            var list = dlqtDao.GetList(MADDK);

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
                    var deletingCPCT = dlqtDao.Get(Int32.Parse(macp));
                    if (deletingCPCT == null)
                    {
                        CloseWaitingDialog();
                        return;
                    }

                    dlqtDao.Delete(deletingCPCT);
                    BindChiPhi();

                    CloseWaitingDialog();

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

            var source = gvChiPhi.DataSource as List<DAOLAP>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi.PageSize * gvChiPhi.PageIndex].MADON;


            var script = "javascript:UpdateCPCTQT(\"" + madon + "\", \"" + txtND.ClientID +
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
                if (QuyetToan == null)
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

                var cpct = new DAOLAPQUYETTOAN
                {
                    MADDK = QuyetToan.MADDK,
                    NOIDUNG = "",
                    DONGIACP = 0,
                    SOLUONG = 1,
                    DVT = dvtList[0].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 0,
                    LOAICP = "DAO",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP"
                };

                dlqtDao.Insert(cpct);
                BindChiPhi();

                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }




        private void BindSelectedVatTu117Grid()
        {
            if (QuyetToan == null) return;
            var list = ctqtNd117Dao.GetList(MADDK);

            gvSelectedVatTu117.DataSource = list;
            gvSelectedVatTu.PagerInforText = list.Count.ToString();
            gvSelectedVatTu117.DataBind();
        }

        protected void gvSelectedVatTu117_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var mavt = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteVatTu":
                    if (QuyetToan == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }

                    var deletingCTCT = ctqtNd117Dao.Get(QuyetToan.MADDK, mavt);
                    if (deletingCTCT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    ctqtNd117Dao.Delete(deletingCTCT);

                    BindSelectedVatTu117Grid();

                    upnlCustomers.Update();

                    //CloseWaitingDialog();

                    break;
            }
        }

        protected void gvSelectedVatTu117_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSL = e.Row.FindControl("txtSOLUONG") as TextBox;
            var txtGIAVT = e.Row.FindControl("txtGIAVT") as TextBox;
            var lblTIENVT = e.Row.FindControl("lblTIENVT") as Label;
            var txtGIANC = e.Row.FindControl("txtGIANC") as TextBox;
            var lblTIENNC = e.Row.FindControl("lblTIENNC") as Label;

            if (txtSL == null || txtGIAVT == null || txtGIANC == null ||
                lblTIENNC == null || lblTIENVT == null) return;

            var source = gvSelectedVatTu117.DataSource as List<CTQUYETTOAN_ND117>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu117.PageSize * gvSelectedVatTu117.PageIndex].MAVT;
            var maddk = source[e.Row.RowIndex + gvSelectedVatTu117.PageSize * gvSelectedVatTu117.PageIndex].MADDK;

            var script = "javascript:UpdateCTCT117QT(\"" + maddk + "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\")";
            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);

            //var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            //if (btnDeleteItem == null) return;
            //btnDeleteItem.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }

        private void BindChiPhi117()
        {
            if (QuyetToan == null) return;

            var list = dlqtNd117Dao.GetList(MADDK);

            gvChiPhi117.DataSource = list;
            gvChiPhi117.PagerInforText = list.Count.ToString();
            gvChiPhi117.DataBind();
        }

        protected void gvChiPhi117_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var macp = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteChiPhi":
                    var deletingCPCT = dlqtNd117Dao.Get(Int32.Parse(macp));
                    if (deletingCPCT == null)
                    {
                        CloseWaitingDialog();
                        return;
                    }
                    dlqtNd117Dao.Delete(deletingCPCT);

                    BindChiPhi117();

                    //CloseWaitingDialog();

                    break;
            }
        }

        protected void gvChiPhi117_RowDataBound(object sender, GridViewRowEventArgs e)
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

            var source = gvChiPhi117.DataSource as List<DAOLAPQUYETTOAN_ND117>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi117.PageSize * gvChiPhi117.PageIndex].MADON;


            var script = "javascript:UpdateCPCT117QT(\"" + madon + "\", \"" + txtND.ClientID +
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

        protected void btnAddChiPhi117_Click(object sender, EventArgs e)
        {
            try
            {
                if (QuyetToan == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                
                var dvtList = dvtDao.GetList();
                if(dvtList.Count == 0)
                {
                    CloseWaitingDialog();
                    ShowError("Không lấy được danh sách đơn vị tính từ cơ sở dữ liệu.");
                    return;
                }

                var cpct = new DAOLAPQUYETTOAN_ND117
                {
                    MADDK = QuyetToan.MADDK,
                    NOIDUNG = "",
                    DONGIACP = 0,
                    SOLUONG = 1,
                    DVT = dvtList[0].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 0,
                    LOAICP = "DAO",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP"
                };
                dlqtNd117Dao.Insert(cpct);
                //ctDao.UpdateChiPhiForQuyetToan(QuyetToan.MADDK);
                BindChiPhi117();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
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

        protected void linkBtnChangeKhoiLuong_Click(object sender, EventArgs e)
        {
            if (QuyetToan == null)
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

            // add to grid
            var ctct = new CTQUYETTOAN
            {
                MADDK = QuyetToan.MADDK,
                MAVT = vt.MAVT,
                LOAICT = CT.CT.ToString(),
                LOAICV = "---***---",
                SOLUONG = decimal.Parse(txtKHOILUONG.Text.Trim()),
                GIAVT = vt.GIAVT,
                TIENVT = decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIAVT,
                GIANC = vt.GIANC,
                TIENNC = decimal.Parse(txtKHOILUONG.Text.Trim()) * vt.GIANC,
                ISCTYDTU = true
            };

            ctqtDao.Insert(ctct);
            BindSelectedVatTuGrid();

            txtMAVT.Text = "";
            lblTENVT.Text = "";
            txtKHOILUONG.Text = "";
            FocusAndSelect(txtMAVT.ClientID);

            upnlCustomers.Update();

            CloseWaitingDialog();
        }

        protected void linkBtnChangeMAVT117_Click(object sender, EventArgs e)
        {
            if (txtMAVT117.Text.Trim() == "")
            {
                CloseWaitingDialog();
                return;
            }

            var vt = vtDao.Get(txtMAVT117.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.", txtMAVT117.ClientID);
                return;
            }

            lblTENVT117.Text = vt.TENVT;
            txtKHOILUONG117.Text = "1";
            FocusAndSelect(txtKHOILUONG117.ClientID);

            CloseWaitingDialog();
        }

        protected void linkBtnChangeKhoiLuong117_Click(object sender, EventArgs e)
        {
            if (QuyetToan == null)
            {
                CloseWaitingDialog();
                return;
            }

            if (txtMAVT117.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Vui lòng nhập mã vật tư", txtMAVT117.ClientID);
                return;
            }

            var vt = vtDao.Get(txtMAVT117.Text.Trim());
            if (vt == null)
            {
                CloseWaitingDialog();
                ShowError("Vật tư không có trong cơ sở dữ liệu. Vui lòng chọn lại.", txtMAVT117.ClientID);
                return;
            }

            try
            {
                decimal.Parse(txtKHOILUONG117.Text.Trim());
            }
            catch
            {
                CloseWaitingDialog();
                ShowError(String.Format(Resources.Message.E_INVALID_DATA, "Khối lượng"), txtKHOILUONG117.ClientID);
                return;
            }

            // add to grid
            var ctct = new CTQUYETTOAN_ND117
            {
                MADDK = QuyetToan.MADDK,
                MAVT = vt.MAVT,
                LOAICT = CT.CT.ToString(),
                LOAICV = "---***---",
                SOLUONG = decimal.Parse(txtKHOILUONG117.Text.Trim()),
                GIAVT = vt.GIAVT,
                TIENVT = decimal.Parse(txtKHOILUONG117.Text.Trim()) * vt.GIAVT,
                GIANC = vt.GIANC,
                TIENNC = decimal.Parse(txtKHOILUONG117.Text.Trim()) * vt.GIANC
            };

            ctqtNd117Dao.Insert(ctct);
            //ctDao.UpdateChiPhiForQuyetToan(QuyetToan.MADDK);
            BindSelectedVatTu117Grid();

            //ShowThongTinHeSoQuyetToan(QuyetToan);
            upnlTongHopChiPhi.Update();

            txtMAVT117.Text = "";
            lblTENVT117.Text = "";
            txtKHOILUONG117.Text = "";
            FocusAndSelect(txtMAVT117.ClientID);
            upnlCustomers.Update();

            CloseWaitingDialog();
        }
    }
}
