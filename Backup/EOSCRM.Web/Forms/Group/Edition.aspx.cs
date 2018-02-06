using System;
using System.Collections.Generic;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.Group
{
    public partial class Edition : Authentication
    {
        #region Variable(s)

        private readonly GroupDao groupDao = new GroupDao();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                Authenticate(Functions.SYS_Group, Permission.Update);

                PrepareUI();

                if (!IsPostBack)
                {
                    // Get group info
                    var group = Session[SessionKey.GROUP] as Domain.Group;

                    if (group != null)
                    {
                        group.Deleted = false;
                        group.Active = true;

                        UcGroupUI.Group = groupDao.Get(group.Id);
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_HT_NHOMNGUOIDUNG;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_HETHONG;
                header.TitlePage = Resources.Message.PAGE_HT_NHOMNGUOIDUNG_SUA;
            }

            UcGroupUI.LoginInfo = LoginInfo;
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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.IsValid)
                {
                    // Check unique name
                    if (groupDao.IsNameDuplicated(UcGroupUI.Group.Id, UcGroupUI.Group.Name, true))
                    {
                        CloseWaitingDialog();
                        ShowError("Tên nhóm đã tồn tại");
                        return;
                    }

                    var objGroup = UcGroupUI.Group;
                    objGroup.UpdateBy = LoginInfo.Username;
                    var msg = groupDao.Update(objGroup);

                    if (msg != null)
                    {
                        if (msg.MsgType != MessageType.Error)
                        {
                            CloseWaitingDialog();
                            ShowInfor(ResourceLabel.Get(msg));
                            Response.Redirect(WebUrlConstants.GROUP_LIST, false);
                        }
                        else
                        {
                            CloseWaitingDialog();
                            ShowError(ResourceLabel.Get(msg));
                            return;
                        }
                    }

                    CloseWaitingDialog();
                }
                else
                    CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
       
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect(WebUrlConstants.GROUP_LIST, false);
        }
    }
}