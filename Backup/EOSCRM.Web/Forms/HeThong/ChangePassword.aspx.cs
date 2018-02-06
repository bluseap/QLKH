using System;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.HeThong
{
    public partial class ChangePassword : Authentication
    {
        private readonly UserAdminDao _objDao = new UserAdminDao();


        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
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

        private void LoadStaticReferences()
        {
            if (LoginInfo == null) return;

            lblUSERNAME.Text = LoginInfo.MANV;
            txtPASSWORD.Text = LoginInfo.Password;
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_SYS_CHANGEPASSWORD;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_HETHONG;
                header.TitlePage = Resources.Message.PAGE_SYS_CHANGEPASSWORD;
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

        private void CloseWaitingDialog()
        {
            ((EOS)Page.Master).CloseWaitingDialog();
        }
        #endregion
        
        
        private void ClearForm()
        {
            LoadStaticReferences();
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if(txtPASSWORD.Text.Trim() == "")
            {
                CloseWaitingDialog();
                ShowError("Mật khẩu không hợp lệ.", txtPASSWORD.ClientID);
                return;
            }

            if (LoginInfo == null)
            {
                CloseWaitingDialog();
                return;
            }

            var useradmin = _objDao.Get(LoginInfo.Id);
            if (useradmin == null)
            {
                CloseWaitingDialog();
                return;
            }

            useradmin.Password = txtPASSWORD.Text.Trim();
            useradmin.UpdateBy = LoginInfo.Username;
            useradmin.UpdateDate = DateTime.Now;

            var msg = _objDao.Update(useradmin);

            CloseWaitingDialog();

            if(msg != null)
            {
                if(msg.MsgType.Equals(MessageType.Error))
                {
                    ShowError(ResourceLabel.Get(msg));
                }
                else
                {
                    ShowInfor(ResourceLabel.Get(msg));
                    ClearForm();
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            CloseWaitingDialog();
        }
    }
}