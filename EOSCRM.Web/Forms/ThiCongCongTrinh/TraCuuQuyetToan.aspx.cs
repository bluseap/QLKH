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
    public partial class TraCuuQuyetToan : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
        private readonly ChietTinhDao ctDao = new ChietTinhDao();
        private readonly TrangThaiThietKeDao ttDao = new TrangThaiThietKeDao();


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

        protected DONDANGKY DonDangKy
        {
            get
            {
                try { return (DONDANGKY)Session["TCDLDM_DDK"]; }
                catch { return null; }
            }

            set { Session["TCDLDM_DDK"] = value; }
        }

        protected THIETKE ThietKe
        {
            get
            {
                try { return (THIETKE)Session["TCDLDM_TK"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_TK"] = value; }
        }

        protected CHIETTINH ChietTinh
        {
            get
            {
                try { return (CHIETTINH)Session["TCDLDM_CT"]; }
                catch { return null; }
            }
            set { Session["TCDLDM_CT"] = value; }
        }

        #endregion



        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.TC_TraCuuQuyetToan, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_TC_TRACUUQUYETTOAN;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THICONG;
                header.TitlePage = Resources.Message.PAGE_TC_TRACUUQUYETTOAN;
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

        private void ShowInFor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }
        #endregion

        private void BindDataForGrid()
        {
            try
            {
                var objList = ddkDao.GetListForTraCuuQuyetToan(Keyword, FromDate, ToDate, StateCode, AreaCode);

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

            var source = gvList.DataSource as List<DUYETQUYETTOAN>;
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
                    imgTT.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(imgTT) + "')");

                    var maTTCT = dct.TTHC;
                    var ttct = ttDao.Get(maTTCT);

                    if (ttct != null)
                    {
                        imgTT.Attributes.Add("class", ttct.COLOR);
                        imgTT.ToolTip = ttct.TENTT;
                    }
                    else
                    {
                        imgTT.ToolTip = "Chưa duyệt quyết toán";
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
                var list = ddkDao.Get(id); 
                switch (e.CommandName)
                {
                    case "EditItem":
                        if (list.TTHC.Equals("QT_A"))
                        {
                            ShowInFor("Quyết toán đã duyệt. Xin chọn quyết toán chưa duyệt.");
                            CloseWaitingDialog();
                            break;
                        }
                        else
                        {
                            Session["LAPQUYETTOAN_MADDK"] = id;
                            Page.Response.Redirect(ResolveUrl("~") + "Forms/ThiCongCongTrinh/LapQuyetToan.aspx?" + Constants.PARAM_REPORTED + "=true", false);

                            CloseWaitingDialog();

                            break;
                        }

                    case "ReportItem":
                        Session["LAPQUYETTOAN_MADDK"] = id;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpLapQuyetToan.aspx", false);

                        CloseWaitingDialog();

                        break;

                    case "showCTStatus":
                        if (!string.Empty.Equals(id))
                        {
                            DonDangKy = ddkDao.Get(id);
                            ThietKe = tkDao.Get(id);
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
