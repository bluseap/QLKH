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
    public partial class DuyetThietKe : Authentication
    {
        private readonly PhongBanDao _pbDao = new PhongBanDao();
        private readonly ChietTinhDao _ctDao = new ChietTinhDao();
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao _tkDao = new ThietKeDao();
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly KhuVucDao _kvDao = new KhuVucDao();

        //string madon1="";

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
                    return TTTK.TK_P.ToString(); 
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

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TK_DuyetThietKe, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadFromN();
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
            Page.Title = Resources.Message.TITLE_TK_DUYETTHIETKE;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_DUYETTHIETKE;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvTKVT);
            
            txtApproveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void LoadFromN()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                if (_nvDao.Get(b).MAKV == "S")
                {
                    txtApproveDate.Enabled = false;
                }
                else
                {
                    txtApproveDate.Enabled = true;
                }
            }
            catch { }
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                var nhanvien = _nvDao.Get(loginInfo.Username);

                if (nhanvien.MAKV == "O" || nhanvien.MAKV == "X" || nhanvien.MAKV == "S"
                    || (nhanvien.MAKV == "U" && nhanvien.MAPB == "KD") || (nhanvien.MAKV == "U" && nhanvien.MAPB == "KTDN")
                    )// || nhanvien.MAKV == "T")
                {
                    var objList = ddkDao.GetListForDuyetThietKe(Keyword, FromDate, ToDate, StateCode, nhanvien.MAKV);                
                    //var objList = ddkDao.GetListForDuyetThietKePB(Keyword, FromDate, ToDate, StateCode, _nvDao.Get(b).MAKV, _nvDao.Get(b).MAPB);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else
                {
                    //var objList = ddkDao.GetListForDuyetThietKe(Keyword, FromDate, ToDate, StateCode, _nvDao.Get(b).MAKV);                
                    var objList = ddkDao.GetListForDuyetThietKePB(Keyword, FromDate, ToDate, StateCode, nhanvien.MAKV, nhanvien.MAPB);

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

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var nhanvien = _nvDao.Get(b);

            // duyet:   -> TTTK = TK_A
            //          -> TTCT = CT_N
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetThietKe, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DateTime? ngayduyet = null;
                //try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); } catch { }
                try { ngayduyet = DateTimeUtil.GetVietNamDate(txtApproveDate.Text); }
                catch { }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKY>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    Message msg;
                    if (nhanvien.MAKV == "X") // long xuyen
                    {
                        
                        msg = ddkDao.ApproveThietKeListDuyetCT(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                        ddkDao.ApproveThietKeListDuyetCT2(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                        ddkDao.ApproveChietTinhListDuyetCT(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                    }
                    else if (nhanvien.MAKV == "S") // chau doc
                    {
                        msg = ddkDao.ApproveThietKeListCD(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                    }
                    else if (nhanvien.MAKV == "T" ) // tan chau
                    {
                        msg = ddkDao.ApproveThietKeListTanChau(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                    }
                    else if (nhanvien.MAKV == "P" || nhanvien.MAKV == "L" || nhanvien.MAKV == "M" 
                            || nhanvien.MAKV == "Q") // phu tan,tri ton,an phu, tinh bien
                    {
                        msg = ddkDao.ApproveThietKeListTKToCT(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                    }
                    else if (nhanvien.MAKV == "K") // cho moi
                    {
                        msg = ddkDao.ApproveThietKeListTanChau(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                    }
                    else if (nhanvien.MAKV == "U") // Thoại son
                    {
                        if (nhanvien.MAPB == "KTDN")
                        {
                            msg = ddkDao.ApproveThietKeList(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                        }
                        else
                        {
                            msg = null;
                        }
                    }
                    else
                    {                        
                        msg = ddkDao.ApproveThietKeList(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);                       
                    }

                    if ((msg != null) && (msg.MsgType != MessageType.Error))
                    {
                        CloseWaitingDialog();
                        ShowInfor(ResourceLabel.Get(msg));
                     
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
                    ShowError("Vui lòng chọn thiết kế cần duyệt.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            // tu choi: -> TTTK = TK_RA
            try
            {
                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;
                var nhanvien = _nvDao.Get(LoginInfo.MANV);
                
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetThietKe, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                if (makv == "U")
                {
                    if (nhanvien.MAPB == "KTDN")
                    {
                        // Get list of ids that to be update
                        var strIds = Request["listIds"];
                        if ((strIds != null) && (!string.Empty.Equals(strIds)))
                        {
                            var objs = new List<DONDANGKY>();
                            var listIds = strIds.Split(',');

                            //Add ma vao danh sách cần delete
                            objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                            DateTime? ngayduyet = null;
                            try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); }
                            catch { }

                            String chuthich = txtNoiDung.Text.Trim();

                            var msg = ddkDao.RejectThietKeListTK(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet, chuthich);

                            if ((msg != null) && (msg.MsgType != MessageType.Error))
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
                            ShowError("Vui lòng chọn thiết kế cần từ chối.");
                        }
                    }
                }
                else
                {
                    // Get list of ids that to be update
                    var strIds = Request["listIds"];
                    if ((strIds != null) && (!string.Empty.Equals(strIds)))
                    {
                        var objs = new List<DONDANGKY>();
                        var listIds = strIds.Split(',');

                        //Add ma vao danh sách cần delete
                        objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                        DateTime? ngayduyet = null;
                        try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); }
                        catch { }

                        String chuthich = txtNoiDung.Text.Trim();

                        var msg = ddkDao.RejectThietKeListTK(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet, chuthich);

                        if ((msg != null) && (msg.MsgType != MessageType.Error))
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
                        ShowError("Vui lòng chọn thiết kế cần từ chối.");
                    }
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
                        if (!string.Empty.Equals(madon))
                        {
                            lbMADDK.Text = madon;

                            lbTENKH.Text = ddkDao.Get(madon).TENKH;
                            BindTKVT(madon);

                            upnlThietKeVatTu.Update();
                            UnblockDialog("divThietKeVatTu");                            
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

        private void BindTKVT(string madon)
        {
            var list = _cttkDao.GetList(madon);

            //lbTENKH.Text = ddkDao.Get(madon1).TENKH;

            gvTKVT.DataSource = list;
            gvTKVT.PagerInforText = list.Count.ToString();
            gvTKVT.DataBind();
        }

        protected void gvTKVT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                gvTKVT.PageIndex = e.NewPageIndex;
                BindTKVT(lbMADDK.Text.Trim());
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btChoTK_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            // duyet:   -> TTTK = TK_A
            //          -> TTCT = CT_N
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetThietKe, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DateTime? ngayduyet = null;
                //try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); } catch { }
                try { ngayduyet = DateTimeUtil.GetVietNamDate(txtApproveDate.Text); }
                catch { }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKY>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => ddkDao.Get(ma)));

                    Message msg;
                   
                    if (_nvDao.Get(b).MAKV == "S")
                    {
                        msg = ddkDao.ApproveChoThietKeListCD(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(),
                            LoginInfo.MANV, ngayduyet, txtNoiDung.Text.Trim());
                    }
                    else
                    {
                        msg = ddkDao.ApproveChoThietKeListCD(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(),
                            LoginInfo.MANV, ngayduyet, txtNoiDung.Text.Trim());
                    }                    

                    if ((msg != null) && (msg.MsgType != MessageType.Error))
                    {
                        CloseWaitingDialog();
                        ShowInfor(ResourceLabel.Get(msg));

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
                    ShowError("Vui lòng chọn thiết kế cần duyệt.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }



    }
}
