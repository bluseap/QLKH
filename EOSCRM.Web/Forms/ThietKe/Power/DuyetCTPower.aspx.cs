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
    public partial class DuyetCTPower : Authentication
    {
        private readonly ChiTietThietKeDao _cttkDao = new ChiTietThietKeDao();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
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

        protected String StateCode
        {
            get
            {
                var param = ParameterWrapper.GetParams();
                if (!param.ContainsKey(Constants.PARAM_STATECODE))
                {
                    return TTCT.CT_P.ToString();
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
                Authenticate(Functions.TK_DuyetChietTinhPo, Permission.Read);
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

        public void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TK_DUYETCHIETTINHPO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_DUYETCHIETTINHPO;
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

                var objList = _ddkpoDao.GetListForDuyetChietTinh(Keyword, FromDate, ToDate, null, kvpo.MAKVPO);
                //var objList = _ddkpoDao.GetListForDuyetChietTinhBravo(Keyword, FromDate, ToDate, null, kvpo.MAKVPO);
                
                gvList.DataSource = objList;
                gvList.PagerInforText = objList.Count.ToString();
                gvList.DataBind();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnApprove_Click(object sender, EventArgs e)
        {
            // duyet:   -> TTCT = TC_A
            //          -> TTHD = HD_P
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetChietTinhPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DateTime? ngayduyet = null;
                //try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); } catch { }
                try { ngayduyet = DateTimeUtil.GetVietNamDate(txtApproveDate.Text); }
                catch { }

                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;
                var nhanvien = _nvDao.Get(b);

                if (nhanvien.MAKV == "S" || nhanvien.MAKV == "T" || nhanvien.MAKV == "K" // chau doc-tan chau-cho moi
                    || nhanvien.MAKV == "P" || nhanvien.MAKV == "L" || nhanvien.MAKV == "M" || nhanvien.MAKV == "Q" //phu tan,tri ton,tinh bien,anphu
                    )
                {
                    // Get list of ids that to be update
                    var strIds = Request["listIds"];
                    if ((strIds != null) && (!string.Empty.Equals(strIds)))
                    {
                        var objs = new List<DONDANGKYPO>();
                        var listIds = strIds.Split(',');

                        //Add ma vao danh sách cần delete
                        objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                        var msg = _ddkpoDao.ApproveChietTinhPhuTanTanChau(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);

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
                        ShowError("Vui lòng chọn chiết tính cần duyệt.");
                    }
                }
                else
                {
                    // Get list of ids that to be update
                    var strIds = Request["listIds"];
                    if ((strIds != null) && (!string.Empty.Equals(strIds)))
                    {
                        var objs = new List<DONDANGKYPO>();
                        var listIds = strIds.Split(',');

                        //Add ma vao danh sách cần delete
                        objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                        var msg = _ddkpoDao.ApproveChietTinhList(objs, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), LoginInfo.MANV, ngayduyet);

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
                        ShowError("Vui lòng chọn chiết tính cần duyệt.");
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            // tu choi: -> TTCT = CT_RA
            try
            {
                // Authenticate
                if (!HasPermission(Functions.TK_DuyetChietTinhPo, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DateTime? ngayduyet = null;
                try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); }
                catch { }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKYPO>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete
                    objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                    var msg = _ddkpoDao.RejectChietTinhList(objs, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV, ngayduyet);

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
                    ShowError("Vui lòng chọn chiết tính cần từ chối.");
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
            var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (lastCell == null) return;

            var source = gvList.DataSource as List<DUYETCHIETTINHPO>;
            if (source == null) return;
            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;
            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = _ddkpoDao.Get(source[index].MADDKPO);


                if (imgTT != null && ddk != null)
                {
                    imgTT.Attributes.Add("class", ddk.TRANGTHAITHIETKE2.COLOR);
                    imgTT.ToolTip = ddk.TRANGTHAITHIETKE2.TENTT;
                }
            }
            catch { }
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

                            lbTENKH.Text = _ddkpoDao.Get(madon).TENKH;
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

        protected void btTRAHOSO_Click(object sender, EventArgs e)
        {
            try
            {
                var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
                if (loginInfo == null) return;
                string b = loginInfo.Username;

                // Authenticate
                if (!HasPermission(Functions.TK_DuyetChietTinh, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DateTime? ngayduyet = null;
                try { ngayduyet = Convert.ToDateTime(txtApproveDate.Text); }
                catch { }

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var objs = new List<DONDANGKYPO>();
                    var listIds = strIds.Split(',');

                    //Add ma vao danh sách cần delete                    
                    objs.AddRange(listIds.Select(ma => _ddkpoDao.Get(ma)));

                    var msg = _ddkpoDao.RejectChietTinhListTraHSTK(objs, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), 
                        LoginInfo.MANV, ngayduyet);

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
                    ShowError("Vui lòng chọn chiết tính cần từ chối.");
                }


                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
    }
}