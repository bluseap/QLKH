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
    public partial class DuyetDonTKTCPo : Authentication
    {
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly ThietKePoDao _tkpoDao = new ThietKePoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();

        //string madon1;

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
                Authenticate(Functions.TK_DuyetDonTKTCPo, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_TK_DUYETDONTKTCPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_DUYETDONTKTCPO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvTKVT);

            txtApproveDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

        private void BindDataForGrid()
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var kvpo = _kvpoDao.GetPo(_nvDao.Get(b).MAKV);

                var objList = _ddkpoDao.GetListForDuyetThietKeTCPo(Keyword, FromDate, ToDate, StateCode, kvpo.MAKVPO);
                //var objList = _ddkpoDao.GetListTKTanChauPo(kvpo.MAKVPO);                

                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
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
                            lbMADDKMOI.Text = madon;
                            BindTKVT();
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

        private void BindTKVT()
        {
            var list = _cttkDao.GetList(lbMADDKMOI.Text.Trim());

            lbTENKH.Text = _ddkpoDao.Get(lbMADDKMOI.Text.Trim()).TENKH;

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

        protected void btCHAPNHAN_Click(object sender, EventArgs e)
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            // duyet:   -> TTTK = TK_A
            //          -> TTCT = CT_N
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetDonTKTCPo, Permission.Update))
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
                    var objs = new List<DONDANGKYPO>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                    Message msg;

                    if (_nvDao.Get(b).MAKV == "T" || _nvDao.Get(b).MAKV == "K" || _nvDao.Get(b).MAKV == "O") // tan chau,cho moi, chau thanh
                    {
                        msg = _ddkpoDao.ApproveDonThietKeKDTanChau(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
                    }
                    else
                    {
                        msg = _ddkpoDao.ApproveDonThietKeKDTanChau(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);
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

        protected void btKODUDIEUKIEN_Click(object sender, EventArgs e)
        {
            // tu choi: -> TTTK = TK_RA
            try
            {
                var makv = _nvDao.Get(LoginInfo.MANV).MAKV;
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetDonTKTCPo, Permission.Update))
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

                    DateTime? ngayduyet = null;
                    try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); }
                    catch { }

                    string chuthich = txtNoiDung.Text.Trim();
                    if (string.IsNullOrEmpty(chuthich))
                    {
                        ShowError("Nhập nội dung khi không chấp nhận.");
                        return;
                    }

                    var msg = _ddkpoDao.RejectDonThietKeTanChau(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet, chuthich);

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
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

    }
}