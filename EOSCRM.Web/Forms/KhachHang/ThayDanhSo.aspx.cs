using System;
using System.Collections.Generic;
using System.Globalization;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;
using System.Web.UI;
using System.Data;
using CrystalDecisions.Shared;

namespace EOSCRM.Web.Forms.KhachHang
{
    public partial class ThayDanhSo : Authentication
    {
        private readonly DuongPhoDao dpDao = new DuongPhoDao();
        private readonly GhiChiSoDao gcsDao = new GhiChiSoDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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
            Page.Title = Resources.Message.TITLE_KH_THAYDANHSO;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_KHACHHANG;
                header.TitlePage = Resources.Message.PAGE_KH_THAYDANHSO;
            }

            //CommonFunc.SetPropertiesForGrid(gvList);
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

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowInfor(string message)
        {
            ((EOS)Page.Master).ShowInfor(message);
        }

        private void ShowWarning(string message)
        {
            ((EOS)Page.Master).ShowWarning(message);
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
            var listKhuVuc = new KhuVucDao().GetList();
            ddlKHUVUC.DataSource = listKhuVuc;
            ddlKHUVUC.DataTextField = "TENKV";
            ddlKHUVUC.DataValueField = "MAKV";
            ddlKHUVUC.DataBind();
            ClearForm();
        }

        private void ClearForm()
        {
            /*
             * clear phần thông tin hồ sơ
             */
            ddlTHANG.SelectedIndex = DateTime.Now.Month - 1;
            txtNAM.Text = DateTime.Now.Year.ToString();
            txtMADP.Text = "";
            txtDUONGPHU.Text = "";
            lblTENDUONG.Text = "";
        }

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

            upnlDuongPho.Update();

            CloseWaitingDialog();
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

                            upnlThayDanhSo.Update();

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

        protected void btnBrowseDP_Click(object sender, EventArgs e)
        {
            BindDuongPho();
            upnlDuongPho.Update();
            UnblockDialog("divDuongPho");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            // Update page index
            gvList.PageIndex = 0;

            if (!BindData())
            {
                CloseWaitingDialog();
                ShowError("Chọn đường phố để ghi chỉ số.", txtMADP.ClientID);
                return;
            }

            CloseWaitingDialog();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
           
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

        private bool BindData()
        {
            if (txtMADP.Text.Trim() == "")
                return false;

            var dp = dpDao.Get(txtMADP.Text.Trim(), txtDUONGPHU.Text.Trim());
            if (dp == null) return false;

            var kynay = new DateTime(int.Parse(txtNAM.Text.Trim()), int.Parse(ddlTHANG.SelectedValue), 1);
            var list = gcsDao.GetListDB(kynay, dp);

            gvList.DataSource = list;
            gvList.PagerInforText = list.Count.ToString(CultureInfo.InvariantCulture);
            gvList.DataBind();

            //TODO: kiểm tra kỳ khai thác có bị lock tính cước trên đường được chọn hay không
            gvList.Enabled = !gcsDao.IsLockTinhCuoc(kynay, dp);
            divList.Visible = true;
            //divWarning.Visible = dp.GIAKHAC.HasValue && dp.GIAKHAC.Value;

            upnlGrid.Update();

            return true;
        }

        protected void gvList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (!e.Row.RowType.Equals(DataControlRowType.DataRow)) return;

            var hfGCS = e.Row.FindControl("hfGCS") as HiddenField;
            var txtMADPG = e.Row.FindControl("txtMADPG") as TextBox;
            var txtMADBG = e.Row.FindControl("txtMADBG") as TextBox;



            if (hfGCS == null || txtMADPG == null || txtMADBG == null) return;

            var onKeyDownEventHandler = "javascript:onKeyDownEventHandler(\"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +                                            
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            txtMADPG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 1, event);");
            txtMADBG.Attributes.Add("onkeydown", onKeyDownEventHandler + ", 2, event);");


            txtMADPG.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtMADPG.ClientID + "\");");
            txtMADBG.Attributes.Add("onfocus", "javascript:onFocusEventHandler(\"" + txtMADBG.ClientID + "\");");

            var onSelectedIndexChangedEventHandler = "javascript:onSelectedIndexChangedEventHandler(\"" + txtMADPG.ClientID +
                                                                "\", \"" + txtMADBG.ClientID +                                              
                                                                "\", \"" + hfGCS.ClientID +
                                                                "\"";
            
        }


    }
}
