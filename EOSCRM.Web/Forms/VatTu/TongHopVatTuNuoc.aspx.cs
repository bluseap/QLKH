using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.Common;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.VatTu
{
    public partial class TongHopVatTuNuoc : Authentication
    {
        private readonly ReportClass _rpDao = new ReportClass();
        private readonly NhanVienDao _nvDao = new NhanVienDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                Authenticate(Functions.VATTU_TongHopVatTuNuoc, Permission.Read);

                //PrepareUI();

                if (!Page.IsPostBack)
                {
                    LoadData();
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }

            
        }

        #region Startup script registeration
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

        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
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

        private void LoadData()
        {
            txtTUNGAY.Text = DateTime.Now.ToString("dd/MM/yyyy");
            txtDENNGAY.Text = DateTime.Now.ToString("dd/MM/yyyy");


        }

        private void BindVatTuCT()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            var list = _rpDao.BANGKEVATTUNUOCCT(DateTimeUtil.GetVietNamDate(txtTUNGAY.Text.Trim()), DateTimeUtil.GetVietNamDate(txtDENNGAY.Text.Trim()),query.MAKV.ToString());
            gvListCT.DataSource = list;
            //gvListCT.PagerInforText = list.Cout.ToString();
            gvListCT.DataBind();

        }

        private void BindVatTuKH()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;
            var query = _nvDao.GetKV(b);

            var list = _rpDao.BANGKEVATTUNUOCKH(DateTimeUtil.GetVietNamDate(txtTUNGAY.Text.Trim()), DateTimeUtil.GetVietNamDate(txtDENNGAY.Text.Trim()), query.MAKV.ToString());
            gvListKH.DataSource = list;
            //gvListCT.PagerInforText = list.Cout.ToString();
            gvListKH.DataBind();
        }

        protected void gvListCT_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvListCT.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindVatTuCT();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void gvListKH_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                // Update page index
                gvListKH.PageIndex = e.NewPageIndex;

                // Bind data for grid
                BindVatTuKH();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }


        protected void btnTim_Click(object sender, EventArgs e)
        {
            BindVatTuCT();
            BindVatTuKH();
            upnlGrid.Update();
        }



       


    }
}
