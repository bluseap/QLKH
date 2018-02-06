using System;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class CapNhatSoBo : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly KhachHangDao khDao = new KhachHangDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.KH_CapNhatSoBo, Permission.Read);
                AjaxPro.Utility.RegisterTypeForAjax(typeof(AjaxCRM), Page);
                PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadStaticReferences();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_KH_CAPNHATSOBO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_CAPNHATSOBO;
            }

            CommonFunc.SetPropertiesForGrid(gvList);
            CommonFunc.SetPropertiesForGrid(gvDuongPho);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void SetLabel(string id, string value)
        {
            ((EOS)Page.Master).SetLabel(id, value);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
        }

        private void HideDialog(string divId)
        {
            ((EOS)Page.Master).HideDialog(divId);
        }

        private void UnblockDialog(string divId)
        {
            ((EOS)Page.Master).UnblockDialog(divId);
        }

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion

        private void LoadStaticReferences()
        {
           ClearForm();
        }

        private void ClearForm()
        {
            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";
        }

        protected bool BindData()
        {
            if (txtMADP.Text.Trim() == "")
                return false;

            var list = khDao.GetList(txtMADP.Text.Trim());
            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString();
            gvList.DataBind();

            hfMADP.Value = txtMADP.Text.Trim();
            divList.Visible = true;

            return true;
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            if (!BindData())
            {
                CloseWaitingDialog();
                ShowError("Chọn đường phố để ghi chỉ số.", txtMADP.ClientID);
                return;
            }

            upnlGrid.Update();

            CloseWaitingDialog();
        }

        #region Đường phố
        protected void btnFilterDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
        }

        private void BindDuongPho()
        {
            var list = dpDao.GetList("%", txtKeywordDP.Text.Trim());
            gvDuongPho.DataSource = list;
            gvDuongPho.PagerInforText = list.Count.ToString();
            gvDuongPho.DataBind();

            CloseWaitingDialog();
        }

        private void UpdateKhuVuc(DUONGPHO dp)
        {
            SetLabel(lblTENDUONG.ClientID, dp.TENDP);
        }

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void gvDuongPho_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                var id = e.CommandArgument.ToString();

                switch (e.CommandName)
                {
                    case "SelectMADP":
                        var res = id.Split('-');
                        var dp = dpDao.Get(res[0], res[1]);
                        if (dp != null)
                        {
                            SetControlValue(txtMADP.ClientID, dp.MADP);
                            SetControlValue(txtDUONGPHU.ClientID, dp.DUONGPHU);

                            UpdateKhuVuc(dp);
                            upnlGhiChiSo.Update();

                            HideDialog("divDuongPho");
                            CloseWaitingDialog();

                            txtMADP.Focus();
                        }

                        break;

                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvDuongPho.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindDuongPho();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvDuongPho_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var lnkBtnID = e.Row.FindControl("lnkBtnID") as LinkButton;
            if (lnkBtnID == null) return;
            lnkBtnID.Attributes.Add("onclick", "onClientClickGridItem('" + CommonFunc.UniqueIDWithDollars(lnkBtnID) + "')");
        } 
        #endregion



        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var txtSTT = e.Row.FindControl("txtSTT") as TextBox;
            var txtTENKH = e.Row.FindControl("txtTENKH") as TextBox;
            var txtSONHA = e.Row.FindControl("txtSONHA") as TextBox;
            var txtMADPG = e.Row.FindControl("txtMADPG") as TextBox;
            var txtMADBG = e.Row.FindControl("txtMADBG") as TextBox;
            var hfCNSB = e.Row.FindControl("hfCNSB") as HiddenField;

            if (txtSTT == null || txtTENKH == null || txtSONHA == null || hfCNSB == null
                || txtMADPG == null || txtMADBG == null) return;
            
            var onBlurEventHandler = "javascript:onBlurEventHandler(\"" + txtSTT.ClientID +
                                                                "\", \"" + txtTENKH.ClientID +
                                                                "\", \"" + txtSONHA.ClientID +
                                                                "\", \"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +
                                                                "\", \"" + hfCNSB.ClientID +
                                                                "\", \"" + LoginInfo.MANV +
                                                                "\"";
            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtSTT.ClientID +
                                                                "\", \"" + txtTENKH.ClientID +
                                                                "\", \"" + txtSONHA.ClientID +
                                                                "\", \"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +
                                                                "\", \"" + hfCNSB.ClientID +
                                                                "\"";

            txtSTT.Attributes.Add("onblur", onBlurEventHandler + ", 1);");
            txtTENKH.Attributes.Add("onblur", onBlurEventHandler + ", 2);");
            txtSONHA.Attributes.Add("onblur", onBlurEventHandler + ", 3);");
            txtMADPG.Attributes.Add("onblur", onBlurEventHandler + ", 4);");
            txtMADBG.Attributes.Add("onblur", onBlurEventHandler + ", 5);");

            txtSTT.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtTENKH.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");
            txtSONHA.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 3, event);");
            txtMADPG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 4, event);");
            txtMADBG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 5, event);");
        }

        protected void gvList_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvList.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindData();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void linkBtnHidden_Click(object sender, EventArgs e)
        {
            var madp = hfMADP.Value;

            var list = khDao.GetList(madp);
            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString();
            gvList.DataBind();

            CloseWaitingDialog();
        }
    }
}
