using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Controls;
using EOSCRM.Util ;
using EOSCRM.Domain;
using EOSCRM.Web;
using EOSCRM.Web.Common;
using EOSCRM.Dao;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;


namespace EOSCRM.Web.Forms.SuaChua
{
    public partial class DuyetQuyetToanSuaChua : Authentication
    {
        private readonly QuyetToanSuaChuaDao _objDao = new QuyetToanSuaChuaDao();


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
        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.SC_DuyetChietTinhSuaChua, Permission.Read);
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
            Page.Title = Resources.Message.TITLE_SC_DUYETCHIETTINHSUACHUA;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_SUACHUA;
                header.TitlePage = Resources.Message.PAGE_SC_DUYETCHIETTINHSUACHUA;
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

        #endregion





        private void BindDataForGrid()
        {
            try
            {
                var objList = _objDao.GetListDonChoDuyetQuyetToan(Keyword, FromDate, ToDate);

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
                if (!HasPermission(Functions.SC_DuyetChietTinhSuaChua, Permission.Update))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                DateTime? ngayduyet = DateTime.Now;

                // Get list of ids that to be update
                var strIds = Request["listIds"];
                if ((strIds != null) && (!string.Empty.Equals(strIds)))
                {
                    var listIds = strIds.Split(',');
                    bool flag = true;
                    foreach (var id in listIds)
                    {
                        var qt = _objDao.Get(id);
                        if (qt == null)
                            break;
                        flag = _objDao.DuyetQT(qt, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), LoginInfo.MANV, ngayduyet);
                        if (flag == false)
                            ShowError("Duyệt không thành công đơn " + id);
                    }
                    if (flag) ShowInfor("Duyệt Quyết toán thành công");
                    BindDataForGrid();
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Vui lòng chọn chiết tính sửa chữa cần duyệt.");
                }

            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }

            CloseWaitingDialog();
        }

        protected void btnReject_Click(object sender, EventArgs e)
        {
            // tu choi: -> TTCT = CT_RA
            try
            {
                // Authenticate
                if (!HasPermission(Functions.SC_DuyetChietTinhSuaChua, Permission.Update))
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
                    var listIds = strIds.Split(',');
                }
                else
                {
                    CloseWaitingDialog();
                    ShowError("Vui lòng chọn chiết tính sửa chữa cần từ chối.");
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }

            CloseWaitingDialog();
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
            /*var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (lastCell == null) return;

            var source = gvList.DataSource as List<DUYETCHIETTINH>;
            if (source == null) return;
            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;
            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;
                var ddk = ddkDao.Get(source[index].MADDK);


                if (imgTT != null && ddk != null)
                {
                    imgTT.Attributes.Add("class", ddk.TRANGTHAITHIETKE2.COLOR);
                    imgTT.ToolTip = ddk.TRANGTHAITHIETKE2.TENTT;
                }
            }
            catch { }*/
        }
    }
}