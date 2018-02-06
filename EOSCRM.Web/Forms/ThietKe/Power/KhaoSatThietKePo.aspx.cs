using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.ThietKe.Power
{
    public partial class KhaoSatThietKePo : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();
        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly DuyetQuyenDao _dqDao = new DuyetQuyenDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly PhongBanDao _pbDao = new PhongBanDao();

        string madon1;

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
                return EncryptUtil.Decrypt(param[Constants.PARAM_STATECODE].ToString());
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
        
        #endregion

        #region Startup script registeration
        private void ShowError(string message)
        {
            ((PO)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((PO)Page.Master).ShowInfor(message);
        }

        private void CloseWaitingDialog()
        {
            ((PO)Page.Master).CloseWaitingDialog();
        }

        private void UnblockDialog(string divId)
        {
            ((PO)Page.Master).UnblockDialog(divId);
        }

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_KhaoSatThietKePo, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    listPhongBan();
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
            Page.Title = Resources.Message.TITLE_TK_XULYDONCHOTHIETKEDIEN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_XULYDONCHOTHIETKEDIEN;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            txtApproveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }       

        private void BindDataForGrid()
        {
            try
            {
                //hien theo phong ban, khu vuc
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);//nhan vien khu vuc ??
                var kvpo = _kvpoDao.GetPo(query.MAKV);

                if (query.MAPB == "NB" || query.MAPB == "TA" || query.MAPB == "TD"
                        || query.MAPB == "TS" || query.MAPB == "TO" || query.MAPB == "TK" || query.MAPB == "NS" || query.MAPB == "NH"
                        || query.MAPB == "CV" || query.MAPB == "HL" || query.MAPB == "MM" || query.MAPB == "PM" // PHU TAN
                        || query.MAPB == "BC" || query.MAPB == "CT" || query.MAPB == "NT" || query.MAPB == "TT" // tri ton
                        || query.MAPB == "CL" || query.MAPB == "MB" || query.MAPB == "NC" || query.MAPB == "TB" // TINH BIEN
                        || query.MAPB == "LA" || query.MAPB == "NC" || query.MAPB == "VH") // TAN CHAU)
                {
                    var objList = _ddkpoDao.GetListForKhaoSatPB(Keyword, FromDate, ToDate, StateCode, LoginInfo.MANV, query.MAPB.ToString(), kvpo.MAKVPO);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();

                    upnlInfor.Update();
                }
                else
                {
                    var objList = _ddkpoDao.GetListForKhaoSat(Keyword, FromDate, ToDate, StateCode, LoginInfo.MANV, kvpo.MAKVPO);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();

                    upnlInfor.Update();
                }

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
                var madon = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "EditHoSo":                        
                        
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

            var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (lastCell == null) return;

            var source = gvList.DataSource as List<DONDANGKYPO>;
            if (source == null) return;

            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;

            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = _ddkpoDao.Get(source[index].MADDKPO);
                var dct = source[index];

                if (imgTT != null && ddk != null)
                {
                    var maTTCT = dct.TTTK;
                    var ttct = ttDao.Get(maTTCT);

                    if (ttct != null)
                    {
                        imgTT.Attributes.Add("class", ttct.COLOR);
                        imgTT.ToolTip = ttct.TENTT;
                    }
                    else
                    {
                        imgTT.ToolTip = "Chưa duyệt khảo sát";
                        imgTT.Attributes.Add("class", "noneIndicator");
                    }
                }
            }
            catch { }
        }

        private void BindTKVT()
        {
            var list = _cttkDao.GetList(madon1);

            gvTKVT.DataSource = list;
            gvTKVT.PagerInforText = list.Count.ToString();
            gvTKVT.DataBind();
        }

        protected void gvTKVT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvTKVT.PageIndex = e.NewPageIndex;                
                BindTKVT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_KhaoSatThietKePo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                var item9 = "%";
                if (ddlPHONGBAN.SelectedValue.Equals(item9) || ddlPHONGBAN.SelectedValue == "%")
                {
                    CloseWaitingDialog();
                    ShowError("Chọn nơi giao hồ sơ thiết kế cho đúng.");
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKYPO>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                    /*foreach (var obj in objs)
                    {
                        // ReSharper disable EmptyGeneralCatchClause
                        try { obj.NGAYKS = DateTimeUtil.GetVietNamDate(txtApproveDate.Text); }
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause
                        // approve thiet ke
                        if (obj.TTTK == null)
                            obj.TTTK = TTTK.TK_N.ToString();

                        obj.NOIDUNG = txtNoiDung.Text.Trim();

                        AddDuyetQuyenPo(obj.MADDKPO, ddlPHONGBAN.SelectedValue);
                    }*/

                    //TODO: check relation before update list
                    //var msg = _ddkpoDao.UpdateList(objs, CommonFunc.GetComputerName(),
                      //                        CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

                    var msg = _ddkpoDao.UpdateList2(objs, CommonFunc.GetComputerName(),
                                              CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ddlPHONGBAN.SelectedValue);


                    if (msg != null)
                    {
                        if (msg.MsgType != MessageType.Error)
                        {
                            CloseWaitingDialog();
                            ShowInfor(ResourceLabel.Get(msg));
                            // Refresh grid view
                            BindDataForGrid();
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
                        ShowError("Duyệt đơn khảo sát thiết kế không thành công.");
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Vui lòng chọn đơn cần duyệt.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void AddDuyetQuyenPo(String madon, String maPb)
        {
            var mapb = _nvDao.Get(LoginInfo.MANV);
            var kvpo = _kvpoDao.GetPo(mapb.MAKV);

            var don = new DUYET_QUYEN()
            {
                MADDK = madon,
                MANV = LoginInfo.MANV,
                MAPB = maPb,
                MAKV = kvpo.MAKVPO,
                NGAY_PQ = DateTime.Now
            };

            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }
            Message msg;
            // insert
            msg = _dqDao.InsertPo(don, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);
        }

        protected void listPhongBan()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.Get(b);
            //foreach (var a in query)
            //{
                //string d = a.MAKV;

            if (query.MAKV == "O") //chau thanh
            {
                /*if (a.MAPB == "NB")
                {
                    ddlPHONGBAN.Items.Clear();
                    ddlPHONGBAN.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                }*/
                if (query.MAPB == "TA")
                {
                    ddlPHONGBAN.Items.Clear();
                    ddlPHONGBAN.Items.Add(new ListItem("Tổ An Châu", "TA"));
                }
                if (query.MAPB == "TD")
                {
                    ddlPHONGBAN.Items.Clear();
                    ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                }
                if (query.MAPB == "KD")
                {
                    ddlPHONGBAN.Items.Clear();
                    ddlPHONGBAN.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                    ddlPHONGBAN.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                    //ddlPHONGBAN.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                    ddlPHONGBAN.Items.Add(new ListItem("Tổ An Châu", "TA"));
                    ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                }
            }
            else
            {
                if (query.MAKV == "U") //thoai son dien
                {
                    ddlPHONGBAN.Items.Clear();
                    if (query.MAPB == "KD")
                    {
                        var kvList = _pbDao.GetListKV(query.MAKV);
                        ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Khánh", "TK"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Óc Eo", "TO"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Núi Sập", "TS"));

                        /*foreach (var pb in kvList)
                        {
                            ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }*/
                    }
                    else
                    {
                        var pbList = _pbDao.GetListPB(query.MAPB);
                        foreach (var pb in pbList)
                        {
                            ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                        }
                    }
                }
                else
                {
                    if (query.MAKV == "S") //chau doc
                    {
                        var kvList = _pbDao.GetListKV(query.MAKV);
                        ddlPHONGBAN.Items.Clear();
                        if (query.MAPB == "KD")
                        {
                            ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
                            ddlPHONGBAN.Items.Add(new ListItem("Phòng KT Điện Nước", "KTDN"));
                            //ddlPHONGBAN.Items.Add(new ListItem("Tổ khu vực điện CĐ", "DC"));
                            foreach (var pb in kvList)
                            {
                                ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                            }
                        }
                        else
                        {
                            var pbList = _pbDao.GetListPB(query.MAPB);
                            foreach (var pb in pbList)
                            {
                                ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                            }
                        }
                    }
                    else
                    {
                        ddlPHONGBAN.Items.Clear();
                        if (query.MAPB == "KD")
                        {
                            var kvList = _pbDao.GetListKV(query.MAKV);
                            ddlPHONGBAN.Items.Add(new ListItem("Tất cả", "%"));
                            ddlPHONGBAN.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                            ddlPHONGBAN.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                            foreach (var pb in kvList)
                            {
                                ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                            }
                        }
                        else
                        {
                            var pbList = _pbDao.GetListPB(query.MAPB);
                            //ddlPHONGBAN.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                            foreach (var pb in pbList)
                            {
                                ddlPHONGBAN.Items.Add(new ListItem(pb.TENPB, pb.MAPB));
                            }
                        }
                    }
                }
            }

        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_KhaoSatThietKePo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKYPO>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                    foreach (var obj in objs)
                    {
                        // ReSharper disable EmptyGeneralCatchClause
                        try { obj.NGAYKS = Convert.ToDateTime(txtApproveDate.Text); }
                        catch { }
                        // ReSharper restore EmptyGeneralCatchClause

                        obj.TTTK = TTTK.TK_RA.ToString();

                        obj.NOIDUNG = txtNoiDung.Text.Trim();
                    }

                    //TODO: check relation before update list
                    var msg = _ddkpoDao.UpdateListTuChoi(objs, CommonFunc.GetComputerName(),  CommonFunc.GetLanIPAddressM(), LoginInfo.MANV);

                    //TODO: check relation before update list
                    //var msg = ddkDao.DoAction(objs, PageAction.Update, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

                    if (msg != null)
                    {
                        if (msg.MsgType != MessageType.Error)
                        {
                            CloseWaitingDialog();

                            ShowInfor(ResourceLabel.Get(msg));

                            // Refresh grid view
                            BindDataForGrid();
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
                        ShowError("Từ chối đơn khảo sát thiết kế không thành công.");
                    }
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Vui lòng chọn đơn cần từ chối.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

    }
}