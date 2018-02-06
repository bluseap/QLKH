using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class LapChietTinh : Authentication
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly ChietTinhDao ctDao = new ChietTinhDao();
        private readonly NhanVienDao nvDao = new NhanVienDao();
        private readonly DaoLapChietTinhDao dlDao = new DaoLapChietTinhDao();
        private readonly DaoLapChietTinhNd117Dao dl117Dao = new DaoLapChietTinhNd117Dao();
        private readonly ChiTietChietTinhDao ctctDao = new ChiTietChietTinhDao();
        private readonly ChiTietChietTinhNd117Dao ctct117Dao = new ChiTietChietTinhNd117Dao();
        private readonly GhiChuChietTinhDao gcDao = new GhiChuChietTinhDao();
        private readonly VatTuDao vtDao = new VatTuDao();
        private readonly DvtDao dvtDao = new DvtDao();
        private readonly MauBocVatTuDao mbvtDao = new MauBocVatTuDao();
        private readonly QuyetToanDao qtdao = new QuyetToanDao();
        private readonly ChiTietQuyetToanDao ctqtdao = new ChiTietQuyetToanDao();
        private readonly ChiTietQuyetToanNd117Dao ctqt117dao = new ChiTietQuyetToanNd117Dao();
        private readonly DaoLapQuyetToanDao dlqtdao = new DaoLapQuyetToanDao();
        private readonly DaoLapQuyetToanNd117Dao dlqt117dao = new DaoLapQuyetToanNd117Dao();
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly PhuongDao _phuongDao = new PhuongDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

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

        protected CHIETTINH ChietTinh
        {
            get
            {
                try { return ctDao.Get(MADDK); }
                catch { return null; }
            }
        }

        private string MADDK
        {
            get { return Session["LAPCHIETTINH_MADDK_2"].ToString(); }
            set { Session["LAPCHIETTINH_MADDK_2"] = value; }
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
                Authenticate(Functions.TK_LapChietTinh, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();               

                if (!Page.IsPostBack)
                {
                   LoadStaticReferences();
                   EventEnter();
                   // truong hop back tu form report
                    if (Session["LAPCHIETTINH_MADDK"] != null && FromReport)
                    {
                        var ct = ctDao.Get(Session["LAPCHIETTINH_MADDK"].ToString());                        
                        MADDK = Session["LAPCHIETTINH_MADDK"].ToString();

                        var mp = ddkDao.Get(MADDK);
                        if (mp.MAPHUONG != null) 
                        { 
                            timkv(mp.MAPHUONG); 
                        }
                        else 
                        { 
                            ddlPHUONG.Items.Add(new ListItem("Tất cả", "%")); 
                        }

                        divChietTinhInfo.Visible = true;

                        upnlCustomers.Visible = true;
                        upnlTongHopChiPhi.Visible = true;

                        BindSelectedVatTuGrid();
                        BindSelectedVatTu117Grid();                        

                        BindChiPhi();
                        BindChiPhi117();

                        BindNCVUOT();

                        divGridList.Visible = false;
                        filterPanel.Visible = false;

                        TongTien(MADDK);

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

                    Session["LAPCHIETTINH_MADDK"] = null;
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_LAPCHIETTINH;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_LAPCHIETTINH;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvVatTu);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu);
            CommonFunc.SetPropertiesForGrid(gvChiPhi);
            CommonFunc.SetPropertiesForGrid(gvSelectedVatTu117);
            CommonFunc.SetPropertiesForGrid(gvChiPhi117);
            
        }

        public void timkv(string maphuong)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var phuong = _phuongDao.GetMAKV(maphuong, nvDao.Get(b).MAKV);
            ddlPHUONG.Items.Clear();
            ddlPHUONG.Items.Add(new ListItem(phuong.TENPHUONG, phuong.MAPHUONG));
            

            /*
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.Get(b);
            var phuongList = _phuongDao.GetList(query.MAKV);
            
            ddlPHUONG.Items.Clear();
            
            foreach (var ph in phuongList)
            {
                ddlPHUONG.Items.Add(new ListItem(ph.TENPHUONG, ph.MAPHUONG));
            }*/
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
                        var objTK = tkDao.Get(id);
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
                        
                        ChayChietTinh(objTK, nhanvien);

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

            var lnkBtnID2 = e.Row.FindControl("lnkBtnID2") as LinkButton;
            if (lnkBtnID2 == null) return;
            lnkBtnID2.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID2) + "')");
        }

        private void ChayChietTinh2(DONDANGKY ddk, NHANVIEN nv)
        {
            var ct = ctDao.Get(ddk.MADDK);
            if (ct == null)
            {
                var result = ctDao.CreateChietTinh2(ddk, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), nv.MANV);

                if (result != null &&
                    !result.MsgType.Equals(MessageType.Error))
                {
                    // show chiet tinh form
                    upnlVatTu.Visible = true;
                    upnlCustomers.Visible = true;

                    // reload grid
                    BindDataForGrid();

                    BindSelectedVatTuGrid();
                    BindSelectedVatTu117Grid();

                    BindChiPhi();
                    BindChiPhi117();

                    //BindNCVUOT();

                    divGridList.Visible = false;
                    filterPanel.Visible = false;


                    ct = ctDao.Get(ddk.MADDK);
                    divChietTinhInfo.Visible = true;
                    upnlVatTu.Visible = true;
                    upnlCustomers.Visible = true;


                    ShowThongTinHeSoChietTinh(ct);
                    upnlTongHopChiPhi.Visible = true;
                    upnlTongHopChiPhi.Update();
                }
                else
                {
                    // Show message
                    ShowError("Chạy chiết tính không thành công.");
                }
            }
            else
            {
                divChietTinhInfo.Visible = true;
                upnlVatTu.Visible = true;
                upnlCustomers.Visible = true;

                BindDataForGrid();

                BindSelectedVatTuGrid();
                BindSelectedVatTu117Grid();

                BindChiPhi();
                BindChiPhi117();

                //BindNCVUOT();

                divGridList.Visible = false;
                filterPanel.Visible = false;

                ShowThongTinHeSoChietTinh(ct);
                upnlTongHopChiPhi.Visible = true;
                upnlTongHopChiPhi.Update();
            }
        }

        private void ChayChietTinh(THIETKE tk, NHANVIEN nv)
        {
            var ct = ctDao.Get(tk.MADDK);
            if (ct == null)
            {
                var result = ctDao.CreateChietTinh2(tk, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), nv.MANV);

                if (result != null && !result.MsgType.Equals(MessageType.Error))
                {
                    // show chiet tinh form
                    upnlVatTu.Visible = true;
                    upnlCustomers.Visible = true;

                    // reload grid
                    BindDataForGrid();

                    BindSelectedVatTuGrid();
                    BindSelectedVatTu117Grid();

                    BindChiPhi();
                    BindChiPhi117();

                    //BindNCVUOT();

                    divGridList.Visible = false;
                    filterPanel.Visible = false;

                    ct = ctDao.Get(tk.MADDK);
                    divChietTinhInfo.Visible = true;
                    upnlVatTu.Visible = true;
                    upnlCustomers.Visible = true;

                    ShowThongTinHeSoChietTinh(ct);
                    upnlTongHopChiPhi.Visible = true;
                    upnlTongHopChiPhi.Update();
                }
                else
                {
                    // Show message
                    ShowError("Chạy chiết tính không thành công.");
                }
            }
            else
            {
                divChietTinhInfo.Visible = true;
                upnlVatTu.Visible = true;
                upnlCustomers.Visible = true;

                BindDataForGrid();

                BindSelectedVatTuGrid();
                BindSelectedVatTu117Grid();

                BindChiPhi();
                BindChiPhi117();

                //BindNCVUOT();

                divGridList.Visible = false;
                filterPanel.Visible = false;

                ShowThongTinHeSoChietTinh(ct);
                upnlTongHopChiPhi.Visible = true;
                upnlTongHopChiPhi.Update();
            }
        }

        private void ShowThongTinHeSoChietTinh(CHIETTINH chiettinh)
        {
            //Hien thi thong tin dia chi chiet tinh

            txtTenHM.Text = chiettinh.TENHM;
            txtTENCT.Text = chiettinh.TENCT;
            lblDIACHI.Text = chiettinh.DONDANGKY.DIACHILD;
            txtGhiChu.Text = chiettinh.GHICHU;

            txtTONGTIENCT.Text = (chiettinh.TONGTIENCTPS.ToString() != null) ? 
                    chiettinh.TONGTIENCTPS.ToString() : "" ;

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
            ddlMBVT.Items.Clear();
            ddlMBVT.Items.Add(new ListItem("", ""));

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

            //timkv();
            

        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (_nvDao.Get(b).MAKV == "S" || _nvDao.Get(b).MAKV == "P" || _nvDao.Get(b).MAKV == "T" || _nvDao.Get(b).MAKV == "K"//chau doc, phu tan, tan chau,cho moi
                    || _nvDao.Get(b).MAKV == "L" || _nvDao.Get(b).MAKV == "M" || _nvDao.Get(b).MAKV == "Q") //tri ton,tinh bien,an phu
                {
                    //var objList = ddkDao.GetListForLapChietTinh(Keyword, FromDate, ToDate, AreaCode);
                    var objList = ddkDao.GetListForLapChietTinhKVCD(Keyword, FromDate, ToDate, nvDao.Get(b).MAKV);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    var objList = ddkDao.GetListForLapChietTinhKV(Keyword, FromDate, ToDate, nvDao.Get(b).MAKV);

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
                decimal.Parse(txtTONGTIENCT.Text);
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
            var objUi = ctDao.Get(MADDK);
            var qt = qtdao.Get(MADDK);
            objUi.TENCT = txtTENCT.Text.Trim();
            objUi.DIACHIHM = lblDIACHI.Text.Trim();
            objUi.TENHM = txtTenHM.Text.Trim();
            objUi.GHICHU = txtGhiChu.Text.Trim();

            objUi.TONGTIENCTPS = Convert.ToDecimal(txtTONGTIENCT.Text.Trim());          

            var msg = ctDao.Update(objUi, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

            _rpClass.HisNgayDangKyBien(objUi.MADDK, LoginInfo.MANV, _nvDao.Get(LoginInfo.MANV).MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "INCHIETTINH");

            CloseWaitingDialog();

            if (msg != null)
            {
                if (msg.MsgType != MessageType.Error)
                {
                    //ShowInfor("Cập nhật thông tin thành công.");
                    Page.Response.Redirect(ResolveUrl("~") + "Forms/ThietKe/LapChietTinh.aspx", false);
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
                

            CHIETTINH objUi = ctDao.Get(MADDK);

            objUi.GIAMGIACPNC = decimal.Parse(txtGIAMGIACPNC.Text);
            objUi.GIAMGIACPVL = decimal.Parse(txtGIAMGIACPVL.Text);
            objUi.HSNHANCONG = decimal.Parse(txtHSNHANCONG.Text);
            objUi.HSTHIETKE3 = decimal.Parse(txtHSTHIETKE3.Text);
            objUi.HSCPC = decimal.Parse(txtHSCPC.Text);
            objUi.HSCHUNG = decimal.Parse(txtHSCHUNG.Text);
            objUi.HSTHUNHAP = decimal.Parse(txtHSTHUNHAP.Text);
            objUi.HSTHIETKE1 = decimal.Parse(txtHSTHIETKE1.Text);

            QUYETTOAN qt = qtdao.Get(MADDK);

            qt.GIAMGIACPNC = decimal.Parse(txtGIAMGIACPNC.Text);
            qt.GIAMGIACPVL = decimal.Parse(txtGIAMGIACPVL.Text);
            qt.HSNHANCONG = decimal.Parse(txtHSNHANCONG.Text);
            qt.HSTHIETKE3 = decimal.Parse(txtHSTHIETKE3.Text);
            qt.HSCPC = decimal.Parse(txtHSCPC.Text);
            qt.HSCHUNG = decimal.Parse(txtHSCHUNG.Text);
            qt.HSTHUNHAP = decimal.Parse(txtHSTHUNHAP.Text);
            qt.HSTHIETKE1 = decimal.Parse(txtHSTHIETKE1.Text);
            qtdao.Update(qt, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
            ctDao.Update(objUi, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

            var chiettinhtontai = ctDao.Get(objUi.MADDK);
            ShowThongTinHeSoChietTinh(chiettinhtontai);

            CloseWaitingDialog();
            ShowInfor("Cập nhật thông tin thành công.");

            upnlTongHopChiPhi.Update();
        }

        protected void btnBaoCao_Click(object sender, EventArgs e)
        {
            if (MADDK != null)
            {
                Session["LAPCHIETTINH_MADDK"] = MADDK;
                Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpLapChietTinh.aspx",
                                       false);
            }
            
            CloseWaitingDialog();
        }

        protected void btnRefreshGrid_Click(object sender, EventArgs e)
        {
            var objUi = ctDao.Get(MADDK);

            ShowThongTinHeSoChietTinh(objUi);

            TongTien(MADDK);
            upnlTongHopChiPhi.Update();

            CloseWaitingDialog();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            TongTien(MADDK);            
            BindSelectedVatTu117Grid();           
            BindChiPhi117();

            //BindNCVUOT();

            upnlTongHopChiPhi.Update();
            upnlCustomers.Update();
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
            if (ChietTinh == null) return;

            var list = ctctDao.GetList(MADDK);

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
                    if (ChietTinh == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }

                    var deletingCTCT = ctctDao.Get(ChietTinh.MADDK, mavt);
                    if (deletingCTCT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    ctctDao.Delete(deletingCTCT);

                    /*var deletingctqt = ctqtdao.Get(ChietTinh.MADDK, mavt);
                    if (deletingctqt == null)
                        return;
                    ctqtdao.Delete(deletingctqt);*/

                    
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

            if (txtSL == null || txtGIAVT == null || lblTIENVT == null || txtGIANC == null ||
                lblTIENNC == null ) return;

            var source = gvSelectedVatTu.DataSource as List<CTCHIETTINH>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MAVT;
            var maddk = source[e.Row.RowIndex + gvSelectedVatTu.PageSize * gvSelectedVatTu.PageIndex].MADDK;

            var script = "javascript:updateCTCT(\"" + maddk + 
                                                        "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\")";
            // update for chi tiet quyettoan
            /*var ctqt = ctqtdao.Get(maddk, mavt);
            var ctct = ctctDao.Get(maddk, mavt);
            if (ctqt == null)
                return;
            ctqt.SOLUONG = ctct.SOLUONG;
            ctct.GIAVT = ctct.GIAVT;
            ctqt.TIENVT = ctct.TIENVT;
            ctqt.GIANC = ctct.GIANC;
            ctqt.TIENNC = ctct.TIENNC;            
            ctqtdao.Update(ctqt);*/

            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);

            //var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            //if (btnDeleteItem == null) return;
            //btnDeleteItem.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }


        
        private void BindChiPhi()
        {
            if (ChietTinh == null) return;

            var list = dlDao.GetList(MADDK);

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
                    var deletingCPCT = dlDao.Get(Int32.Parse(macp));
                    if (deletingCPCT == null)
                    {
                        //loseWaitingDialog();
                        return;
                    }
                    dlDao.Delete(deletingCPCT);

                    var deletingcqqt = dlqtdao.Get(Int32.Parse(macp));
                    if (deletingcqqt == null)
                        return;
                    dlqtdao.Delete(deletingcqqt);
                                        
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

            var source = gvChiPhi.DataSource as List<DAOLAP>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi.PageSize * gvChiPhi.PageIndex].MADON;


            var script = "javascript:updateCPCT(\"" + madon + "\", \"" + txtND.ClientID +
                                                                "\", \"" + txtDG.ClientID +
                                                                "\", \"" + ddlDVT.ClientID +
                                                                "\", \"" + txtSL.ClientID +
                                                                "\", \"" + txtHS.ClientID +
                                                                "\", \"" + lblTHANHTIENCP.ClientID +
                                                                "\", \"" + ddlLCP.ClientID +
                                                                "\")";
            var dlct = dlDao.Get(madon);
            var dlqt = dlqtdao.Get(madon);
            if (dlqt == null)
                return;
            dlqt.NOIDUNG = dlct.NOIDUNG;
            dlqt.DONGIACP = dlct.DONGIACP;
            dlqt.DVT = dlct.DVT;
            dlqt.SOLUONG = dlct.SOLUONG;
            dlqt.HESOCP = dlct.HESOCP;
            dlqt.THANHTIENCP = dlct.THANHTIENCP;
            dlqt.LOAICP = dlct.LOAICP;
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
                if (ChietTinh == null)
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

                var cpct = new DAOLAP
                {
                    MADDK = ChietTinh.MADDK,
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

                dlDao.Insert(cpct);// trong lop kia da insert voa bang daolapQuyettoan lun roi
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
            if (ChietTinh == null) return;
            var list = ctct117Dao.GetList(MADDK);
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
                    if (ChietTinh == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }

                    var deletingCTCT = ctct117Dao.Get(ChietTinh.MADDK, mavt);
                    if (deletingCTCT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    ctct117Dao.Delete(deletingCTCT);

                    /*var deletingCTQT = ctqt117dao.Get(ChietTinh.MADDK, mavt);
                    if (deletingCTQT == null)
                    {
                        return;
                    }
                    ctqt117dao.Delete(deletingCTQT);*/
                    
                    BindSelectedVatTu117Grid();

                    TongTien(ChietTinh.MADDK);
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

            var source = gvSelectedVatTu117.DataSource as List<CTCHIETTINH_ND117>;
            if (source == null) return;

            var mavt = source[e.Row.RowIndex + gvSelectedVatTu117.PageSize * gvSelectedVatTu117.PageIndex].MAVT;
            var maddk = source[e.Row.RowIndex + gvSelectedVatTu117.PageSize * gvSelectedVatTu117.PageIndex].MADDK;

            var script = "javascript:updateCTCT117(\"" + maddk + "\", \"" + mavt +
                                                        "\", \"" + txtSL.ClientID +
                                                        "\", \"" + txtGIAVT.ClientID +
                                                        "\", \"" + lblTIENVT.ClientID +
                                                        "\", \"" + txtGIANC.ClientID +
                                                        "\", \"" + lblTIENNC.ClientID +
                                                        "\")";
            /*var ctqt = ctqt117dao.Get(maddk, mavt);
            var ctct = ctct117Dao.Get(maddk, mavt);
            if (ctqt == null)
                return;
            ctqt.SOLUONG = ctct.SOLUONG;
            ctct.GIAVT = ctct.GIAVT;
            ctqt.TIENVT = ctct.TIENVT;
            ctqt.GIANC = ctct.GIANC;
            ctqt.TIENNC = ctct.TIENNC;*/

            txtSL.Attributes.Add("onblur", script);
            txtGIAVT.Attributes.Add("onblur", script);
            txtGIANC.Attributes.Add("onblur", script);

            //var btnDeleteItem = e.Row.FindControl("btnDelete") as LinkButton;
            //if (btnDeleteItem == null) return;
            //btnDeleteItem.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(btnDeleteItem) + "')");
        }


        private void BindChiPhi117()
        {
            if (ChietTinh == null) 
                return;

            //var list = dl117Dao.GetList(MADDK);
            var list = dl117Dao.GetListVC(MADDK);

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
                    var deletingCPCT = dl117Dao.Get(Int32.Parse(macp));
                    if (deletingCPCT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    dl117Dao.Delete(deletingCPCT);

                    /*var deletingCPQT = dlqt117dao.Get(Int32.Parse(macp));
                    if (deletingCPQT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    dlqt117dao.Delete(deletingCPQT);*/

                    TongTien(ChietTinh.MADDK);
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

            var source = gvChiPhi117.DataSource as List<DAOLAP_ND117>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvChiPhi117.PageSize * gvChiPhi117.PageIndex].MADON;


            var script = "javascript:updateCPCT117(\"" + madon + "\", \"" + txtND.ClientID +
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
                if (ChietTinh == null)
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

                var cpct = new DAOLAP_ND117
                {
                    MADDK = ChietTinh.MADDK,
                    NOIDUNG = "Vận chuyển",
                    DONGIACP = 5000,
                    SOLUONG = 1,
                    DVT = dvtList[8].DVT1,
                    HESOCP = 1,
                    THANHTIENCP = 5000,
                    LOAICP = "VC",
                    NGAYLAP = DateTime.Now,
                    LOAICT = "CP"
                };    
                dl117Dao.Insert(cpct);


                BindChiPhi117();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }



        protected void btnChange_Click(object sender, EventArgs e)
        {
            var mbvt = mbvtDao.Get(ddlMBVT.SelectedValue);

            if(mbvt == null)
            {
                CloseWaitingDialog();
                ShowError("Vui lòng chọn mẫu bốc vật tư");
                return;
            }

            ctDao.ChangeFromMBVT(ChietTinh, mbvt);

            BindSelectedVatTuGrid();
            BindSelectedVatTu117Grid();
            BindChiPhi();
            BindChiPhi117();

            //BindNCVUOT();

            ShowThongTinHeSoChietTinh(ChietTinh);

            TongTien(ChietTinh.MADDK);

            upnlTongHopChiPhi.Update();
            upnlCustomers.Update();

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

        protected void linkBtnChangeKhoiLuong_Click(object sender, EventArgs e)
        {
            if (ChietTinh == null)
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
            var ctct = new CTCHIETTINH
            {
                MADDK = ChietTinh.MADDK,
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
            

            ctctDao.Insert(ctct);
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
            if (ChietTinh == null)
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
            var ctct = new CTCHIETTINH_ND117
            {
                MADDK = ChietTinh.MADDK,
                MAVT = vt.MAVT,
                LOAICT = CT.CT.ToString(),
                LOAICV = "---***---",
                SOLUONG = decimal.Parse(txtKHOILUONG117.Text.Trim()),
                GIAVT = vt.GIAVT,
                TIENVT = decimal.Parse(txtKHOILUONG117.Text.Trim()) * vt.GIAVT,
                GIANC = vt.GIANC,
                TIENNC = decimal.Parse(txtKHOILUONG117.Text.Trim()) * vt.GIANC
            };

            ctct117Dao.Insert(ctct);
            //ctDao.UpdateChiPhiForChietTinh(ChietTinh.MADDK);
            BindSelectedVatTu117Grid();

            ShowThongTinHeSoChietTinh(ChietTinh);
            upnlTongHopChiPhi.Update();

            txtMAVT117.Text = "";
            lblTENVT117.Text = "";
            txtKHOILUONG117.Text = "";

            TongTien(ChietTinh.MADDK); 

            FocusAndSelect(txtMAVT117.ClientID);
            upnlCustomers.Update();

            CloseWaitingDialog();
        }

        public void TongTien(string madk)
        {
            //sum tong tien            
            var listctct = ctctDao.GetList(madk);
            var listctnd117 = ctct117Dao.GetList(madk);
            var lisrdaoct117 = dl117Dao.GetListNCVC(madk);

            var tienctct = Math.Round(listctct.Sum(p => p.TIENVT).Value, 0, MidpointRounding.AwayFromZero);
            var tienctnd117 = Math.Round(listctnd117.Sum(p => p.TIENVT).Value, 0, MidpointRounding.AwayFromZero);
            var tiendaoct117 = Math.Round(lisrdaoct117.Sum(p => p.THANHTIENCP).Value, 0, MidpointRounding.AwayFromZero);

            //tien vat tu cong ty cap
            lbTIENVTCTCAP.Text = String.Format("{0:0,0.0}", tienctct.ToString());
            //tien vat tu
            lbTIENVT.Text = String.Format("{0:0,0.0}", tienctnd117.ToString());
            //tien nhan cong
            var tienctnd117NC = Math.Round(listctnd117.Sum(p => p.TIENNC).Value, 0, MidpointRounding.AwayFromZero);
            lbTIENNC.Text = String.Format("{0:0,0.0}", tienctnd117NC.ToString());

            //tien khach hang thanh toan
            var tongtien = tienctnd117 + tiendaoct117;
            lblTHONGKE.Text = String.Format("{0:0,0.0}",tongtien.ToString());            
                
        }

       

        protected void btnNCVUOT_Click(object sender, EventArgs e)
        {
            try
            {
                if (ChietTinh == null)
                {
                    CloseWaitingDialog();
                    return;
                }
                
                int gianc = Int16.Parse(lbTIENNC.Text.Trim());
                _rpDao.IN_DLND117(MADDK, gianc);       

                BindNCVUOT();
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void BindNCVUOT()
        {
            if (ChietTinh == null)
                return;

            var list = dl117Dao.GetListNCVUOT(MADDK);

            gvNCVUOT.DataSource = list;
            gvNCVUOT.PagerInforText = list.Count.ToString();
            gvNCVUOT.DataBind();
        }

        protected void gvNCVUOT_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            var macp = e.CommandArgument.ToString();

            switch (e.CommandName)
            {
                case "DeleteChiPhi":
                    var deletingCPCT = dl117Dao.Get(Int32.Parse(macp));
                    if (deletingCPCT == null)
                    {
                        //CloseWaitingDialog();
                        return;
                    }
                    dl117Dao.Delete(deletingCPCT);

                    

                    //TongTien(ChietTinh.MADDK);
                    BindNCVUOT();
                    //CloseWaitingDialog();
                    break;
            }

        }

        protected void gvNCVUOT_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtND = e.Row.FindControl("txtNOIDUNGNCVUOT") as TextBox;
            var txtDG = e.Row.FindControl("txtDONGIANCVUOT") as TextBox;
            var ddlDVT = e.Row.FindControl("ddlDVTNCVUOT") as DropDownList;
            var txtSL = e.Row.FindControl("txtSOLUONGNCVUOT") as TextBox;
            var txtHS = e.Row.FindControl("txtHESOCPNCVUOT") as TextBox;
            var lblTHANHTIENCP = e.Row.FindControl("lblTHANHTIENCPNCVUOT") as Label;
            var ddlLCP = e.Row.FindControl("ddlLOAICPNCVUOT") as DropDownList;

            if (txtND == null || txtDG == null || ddlDVT == null || txtSL == null ||
                txtHS == null || lblTHANHTIENCP == null || ddlLCP == null) return;

            var source = gvNCVUOT.DataSource as List<DAOLAP_ND117>;
            if (source == null) return;

            var madon = source[e.Row.RowIndex + gvNCVUOT.PageSize * gvNCVUOT.PageIndex].MADON;


            //var script = "javascript:updateCPCT117(\"" + madon + "\", \"" + txtND.ClientID +
            var script = "javascript:updateCPNCVUOT(\"" + madon + "\", \"" + txtND.ClientID +
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

        private void EventEnter()
        {
            txtTONGTIENCT.Attributes.Add("onkeypress", "return clickButtonSAVE(event)");
            
        }
    }
}
