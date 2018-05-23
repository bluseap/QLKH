using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.UserControls;
using EOSCRM.Web.Shared;

namespace EOSCRM.Web.Forms.ThietKe
{
    public partial class TraCuuThietKe : Authentication
    {
        private readonly DonDangKyDao ddkDao = new DonDangKyDao();
        private readonly ThietKeDao tkDao = new ThietKeDao();
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
        
        #endregion

        #region Startup script registeration
        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void ShowInFor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
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
                Authenticate(Functions.TK_TraCuuThietKe, Permission.Read);

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
            Page.Title = Resources.Message.TITLE_TK_TRACUUTHIETKE;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_THIETKE;
                header.TitlePage = Resources.Message.PAGE_TK_TRACUUTHIETKE;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
        }

        private void BindDataForGrid()
        {
            try
            {
                //var objList = ddkDao.GetListForTraCuuThietKe(Keyword, FromDate, ToDate, StateCode,AreaCode);

                string b = LoginInfo.MANV;
                var pb = _nvDao.GetKV(b);

                if (pb.MAKV == "O" && (pb.MAPB == "KTDN" || pb.MAPB == "TA" || pb.MAPB == "TD" || pb.MAPB == "NB"))
                {
                    var objList = ddkDao.GetListForTraCuuThietKePB(Keyword, FromDate, ToDate, StateCode, AreaCode, pb.MAPB);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else if( pb.MAPB == "TS" || pb.MAPB == "TO" || pb.MAPB == "TK" || pb.MAPB == "NS" || pb.MAPB == "NH"
                        || pb.MAPB == "CV" || pb.MAPB == "HL" || pb.MAPB == "MM" || pb.MAPB == "PM" // PHU TAN
                         || pb.MAPB == "BC" || pb.MAPB == "CT" || pb.MAPB == "NT" || pb.MAPB == "TT" // tri ton
                    || pb.MAPB == "AT" || pb.MAPB == "CM" || pb.MAPB == "HB" || pb.MAPB == "KT" // CHO MOI
                    || pb.MAPB == "LG" || pb.MAPB == "ML" || pb.MAPB == "NL" || pb.MAPB == "TM" // CHO MOI
                        || pb.MAPB == "LA" || pb.MAPB == "NC" || pb.MAPB == "VH")// tan chau
                {
                    var objList = ddkDao.GetListForTraCuuThietKePB(Keyword, FromDate, ToDate, StateCode, AreaCode, pb.MAPB);

                    gvList.DataSource = objList;
                    gvList.PagerInforText = objList.Count.ToString();
                    gvList.DataBind();
                }
                else //if (pb.MAKV == "O")
                {
                    var objList = ddkDao.GetListForTraCuuThietKeKV(Keyword, FromDate, ToDate, StateCode, AreaCode, pb.MAKV);

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

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lastCell = e.Row.Cells[e.Row.Cells.Count - 1];
            if (lastCell == null) return;

            var source = gvList.DataSource as List<THIETKE>;
            if (source == null) return;
            lastCell.Attributes.Add("style", "border-left: none 0px; padding: 6px 0 4px !important;");

            var imgTT = e.Row.FindControl("imgTT") as Button;

            try
            {
                var index = e.Row.RowIndex + gvList.PageSize * gvList.PageIndex;

                if (imgTT != null)
                {
                    imgTT.Attributes.Add("class", source[index].DONDANGKY.TRANGTHAITHIETKE1.COLOR);
                    imgTT.ToolTip = source[index].DONDANGKY.TRANGTHAITHIETKE1.TENTT;
                }
            }
            catch {}
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

                        if (list.TTTK.Equals("TK_A"))
                        {
                            if (LoginInfo.MANV == "tam" && list.TTCT == "CT_N")
                            {
                                Session["NHAPTHIETKE_MADDK"] = id;
                                var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx";
                                Response.Redirect(url, false);
                                break;
                            }
                            else if (list.TTTK == "TK_A" && list.TTCT == "CT_N")//chi tri ton,an phu,tinh bien,thoai son,chau phu,chau doc
                            {
                                Session["NHAPTHIETKE_MADDK"] = id;
                                var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx";
                                Response.Redirect(url, false); 
                                break;
                            }
                            else
                            {
                                ShowInFor("Thiết kế đã duyệt và nhập chiết tính. Xin chọn thiết kế chưa duyệt va nhập chiết tính.");
                                break;
                            }
                        }
                        else if (_nvDao.Get(LoginInfo.MANV).MAKV == "X")
                        {     
                            Session["NHAPTHIETKE_MADDK"] = id;
                            var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTuLX.aspx";
                            Response.Redirect(url, false);
                            break;                                                       
                        }
                        else
                        {

                            Session["NHAPTHIETKE_MADDK"] = id;
                            var url = ResolveUrl("~") + "Forms/ThietKe/BocVatTu.aspx";
                            Response.Redirect(url, false);
                            break;
                        }

                    case "ReportItem":
                        Session["NHAPTHIETKE_MADDK"] = id;
                        Page.Response.Redirect(ResolveUrl("~") + "Forms/KhachHang/BaoCao/DonLapDatMoi/rpVTTKBVT.aspx", false);

                        break;
                }
            }
            catch(Exception ex)
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
  
    }
}
