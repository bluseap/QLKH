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

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class KhaoSatThietKe : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly DuyetQuyenDao _dqDao = new DuyetQuyenDao();
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();

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

        /*
        private DONDANGKY DonDangKy
        {
            //TODO: Kiểm tra validate dữ liệu, tạo và trả về đối tượng từ UI
            get
            {
                if (!IsDataValid())
                    return null;

                var dondk = new DONDANGKY
                {
                    MADDK = txtMADDK.Text.Trim(),
                    MADDKTONG = ddlMADDKTONG.SelectedValue.Equals("") ? null : ddlMADDKTONG.SelectedValue,
                    TENKH = txtTENKH.Text.Trim(),
                    SONHA = txtSONHA.Text.Trim(),       // so nha
                    TEN_DC_KHAC = txtMADP.Text.Trim(),  // ten duong luu o day
                    DIENTHOAI = txtDIENTHOAI.Text.Trim(),
                    MAPHUONG = ddlPHUONG.SelectedValue,
                    MAKV = ddlKHUVUC.SelectedValue,
                    MAMDSD = ddlMUCDICH.SelectedValue,
                    DAIDIEN = cbDAIDIEN.Checked
                };

                // dai dien, ma duong
                var phuong = phuongDao.Get(ddlPHUONG.SelectedValue);

                var item = nvDao.Get(LoginInfo.Username);
                if (item == null) return null;
                khuvuc = kvDao.Get(item.MAKV);
                if (khuvuc == null) return null;

                // dinh dang: [so nha] [ten duong], [phuong], [khu vuc]
                dondk.DIACHILD = string.Format("{0} {1}, {2}{3}",
                    dondk.SONHA,
                    dondk.TEN_DC_KHAC,
                    ((phuong != null) ? phuong.TENPHUONG + ", " : ""),
                    khuvuc.TENKV);

                if (!txtSOHODN.Text.Trim().Equals(String.Empty))
                    dondk.SOHODN = Convert.ToInt32(txtSOHODN.Text.Trim());
                if (!txtSONK.Text.Trim().Equals(String.Empty))
                    dondk.SONK = Convert.ToInt32(txtSONK.Text.Trim());
                if (!txtDMNK.Text.Trim().Equals(String.Empty))
                    dondk.DMNK = Convert.ToInt32(txtDMNK.Text.Trim());
                if (!txtNGAYCD.Text.Trim().Equals(String.Empty))
                    dondk.NGAYDK = Convert.ToDateTime(txtNGAYCD.Text.Trim());
                if (!txtNGAYKS.Text.Trim().Equals(String.Empty))
                    dondk.NGAYHKS = Convert.ToDateTime(txtNGAYKS.Text.Trim());

                // default value
                dondk.LOAIDK = LOAIDK.DK.ToString();
                dondk.TTDK = TTDK.DK_A.ToString();

                return dondk;
            }

            //TODO: Xóa content và error message trên UI, dán dữ liệu vào UI
            set
            {
                ClearError();
                ClearContent();
                if (value == null)
                    return;

                #region textboxes
                txtMADDK.Text = value.MADDK;
                txtTENKH.Text = value.TENKH;
                txtSONHA.Text = value.SONHA;
                txtDIENTHOAI.Text = value.DIENTHOAI;
                txtMADP.Text = value.TEN_DC_KHAC;
                txtSOHODN.Text = value.SOHODN.HasValue ? value.SOHODN.Value.ToString() : "";
                txtSONK.Text = value.SONK.HasValue ? value.SONK.Value.ToString() : "";
                txtDMNK.Text = value.DMNK.HasValue ? value.DMNK.Value.ToString() : "";
                txtNGAYCD.Text = value.NGAYDK.HasValue ? value.NGAYDK.Value.ToString(Constants.DateformatView) : "";
                txtNGAYKS.Text = value.NGAYHKS.HasValue ? value.NGAYHKS.Value.ToString(Constants.DateformatView) : "";

                cbDAIDIEN.Checked = value.DAIDIEN;
                #endregion

                #region references
                try
                {
                    ddlMADDKTONG.SelectedValue = value.MADDKTONG;
                }
                catch
                {
                    ddlMADDKTONG.SelectedIndex = 0;
                }

                try
                {
                    ddlPHUONG.SelectedValue = value.MAPHUONG;
                }
                catch
                {
                    ddlPHUONG.SelectedIndex = 0;
                }

                try
                {
                    ddlMUCDICH.SelectedValue = value.MAMDSD;
                }
                catch
                {
                    ddlMUCDICH.SelectedIndex = 0;
                }
                #endregion
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
       
 
         */
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_KhaoSatThietKe, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_TK_XULYDONCHOTHIETKE;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_XULYDONCHOTHIETKE;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            txtApproveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        #region Startup script registeration
        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

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
                //hien theo phong ban, khu vuc
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var query = _nvDao.Get(b);//nhan vien khu vuc ??

                if (query.MAPB == "NB" || query.MAPB == "TA" || query.MAPB == "TD")
                {
                    var objList = ddkDao.GetListForKhaoSatPB(Keyword, FromDate, ToDate, StateCode, LoginInfo.MANV, query.MAPB.ToString());

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();

                    upnlInfor.Update();
                }
                else
                {
                    var objList = ddkDao.GetListForKhaoSat(Keyword, FromDate, ToDate, StateCode, LoginInfo.MANV);

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
                        /*
                        if (!string.Empty.Equals(madon))
                        {

                            madon1 = madon;
                            BindTKVT();
                            upnlThietKeVatTu.Update();
                            UnblockDialog("divThietKeVatTu");

                            
                        }
                        */
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

            var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (lastCell == null) return;

            var source = gvList.DataSource as List<DONDANGKY>;
            if (source == null) return;

            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;

            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = ddkDao.Get(source[index].MADDK);
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
                // Update page index
                gvTKVT.PageIndex = e.NewPageIndex;

                // Bind data for grid
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
                if (!HasPermission(Functions.TK_KhaoSatThietKe, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKY>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    foreach (var obj in objs)
                    {
                        // ReSharper disable EmptyGeneralCatchClause
                        try { obj.NGAYKS = DateTimeUtil.GetVietNamDate(txtApproveDate.Text); } catch {}
                        // ReSharper restore EmptyGeneralCatchClause
                        

                        // approve thiet ke
                        if (obj.TTTK == null)
                            obj.TTTK = TTTK.TK_N.ToString();

                        obj.NOIDUNG = txtNoiDung.Text.Trim();

                        AddDuyetQuyen(obj.MADDK, ddlPHONGBAN.SelectedValue);
                        

                    }

                    //TODO: check relation before update list
                    var msg = ddkDao.UpdateList(objs, CommonFunc.GetComputerName(),
                                              CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

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

        protected void AddDuyetQuyen(String madon, String maPb)
        {
            var mapb = _nvDao.Get(LoginInfo.MANV);
            var don = new DUYET_QUYEN()
            {
                MADDK=madon,
                MANV=LoginInfo.MANV,
                MAPB = maPb,
                MAKV=mapb.MAKV,
                NGAY_PQ = DateTime.Now
            };


            if (don == null)
            {
                CloseWaitingDialog();
                return;
            }
            Message msg;
            // insert
            msg = _dqDao.Insert(don, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);
        }

        protected void listPhongBan()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if(a.MAKV == "O")
                {
                    if (a.MAPB == "NB") {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                    }
                    if (a.MAPB == "TA")
                    {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ An Châu", "TA"));
                    }
                    if (a.MAPB == "TD")
                    {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));  
                    }
                    if (a.MAPB == "KD")
                    {
                        ddlPHONGBAN.Items.Clear();
                        ddlPHONGBAN.Items.Add(new ListItem("Phòng Kinh Doanh", "KD"));
                        ddlPHONGBAN.Items.Add(new ListItem("Phòng Kỹ Thuật Điện Nước", "KTDN"));
                        ddlPHONGBAN.Items.Add(new ListItem("Nhà máy nước Bình Hoà", "NB"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ An Châu", "TA"));
                        ddlPHONGBAN.Items.Add(new ListItem("Tổ Vĩnh Hanh", "TD"));
                    }
                }                
                else
                {
                    ddlPHONGBAN.Items.Clear();
                    ddlPHONGBAN.Items.Add(new ListItem("Tất cả", ""));
                }
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_KhaoSatThietKe, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKY>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    foreach (var obj in objs)
                    {
// ReSharper disable EmptyGeneralCatchClause
                        try { obj.NGAYKS = Convert.ToDateTime(txtApproveDate.Text); } catch { }
// ReSharper restore EmptyGeneralCatchClause

                        obj.TTTK = TTTK.TK_RA.ToString();

                        obj.NOIDUNG = txtNoiDung.Text.Trim();
                    }

                    //TODO: check relation before update list
                    var msg = ddkDao.UpdateList(objs, CommonFunc.GetComputerName(),
                                              CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV);

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
