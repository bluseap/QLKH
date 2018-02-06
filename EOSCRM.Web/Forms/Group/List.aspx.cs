using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using System.Web.UI;
using EOSCRM.Controls;
using EOSCRM.Dao;
using EOSCRM.Web.Common;
using EOSCRM.Util;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.Group
{
    public partial class List : Authentication
    {
        #region Variable(s)

        private readonly GroupDao groupDao = new GroupDao();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                Authenticate(Functions.SYS_Group, Permission.Read);

                PrepareUI();

                if (!Page.IsPostBack)
                {
                    // Set properties for grid view
                    CommonFunc.SetPropertiesForGrid(grdView);

                    // Bind data for grid view
                    BindDataForGrid(grdView);
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
                header.TitlePage = Resources.Message.PAGE_HT_NHOMNGUOIDUNG_LIETKE;
            }

            CommonFunc.SetPropertiesForGrid(grdView);
        }

        #region Startup script registeration
        private void SetControlValue(string id, string value)
        {
            ((EOS)Page.Master).SetControlValue(id, value);
        }

        private void ShowError(string message)
        {
            ((EOS)Page.Master).ShowError(message);
        }

        private void ShowError(string message, string controlId)
        {
            ((EOS)Page.Master).ShowError(message, controlId);
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

        protected void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.SYS_Group, Permission.Delete))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);                    
                    return;
                }

                // Do delete action
                DoAction(PageAction.Delete);
                CloseWaitingDialog();
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        protected void btnAddnew_Click(object sender, EventArgs e)
        {
            try
            {
                // Authenticate
                if (!HasPermission(Functions.SYS_Group, Permission.Insert))
                {
                    CloseWaitingDialog();
                    ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                    return;
                }

                CloseWaitingDialog();
                Response.Redirect(WebUrlConstants.GROUP_CREATTION, false);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        protected void grdData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {                
                grdView.PageIndex = e.NewPageIndex;
                BindDataForGrid(grdView);
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
        
        protected void grdData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "EditGroup")
                {
                    // Authenticate
                    if (!HasPermission(Functions.SYS_Group, Permission.Update))
                    {
                        ShowError(Resources.Message.WARN_PERMISSION_DENIED);
                        return;
                    }
                    var groupID = e.CommandArgument.ToString();
                    if (!string.Empty.Equals(groupID))
                    {
                        // Save this group to singleton
                        var group = new Domain.Group
                        {
                            Id = int.Parse(groupID)
                        };
                        
                        Session[SessionKey.GROUP] = group;
                        Session[SessionKey.USER_LOGIN] = LoginInfo;

                        // Bring to group edition page
                        Response.Redirect(WebUrlConstants.GROUP_EDITION, false);
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }

        /// <summary>
        /// Do Action
        /// </summary>
        /// <param name="action"></param>
        private void DoAction(PageAction action)
        {

            // Get list of groupId that to be update
            var strIds = Request["listIds"];

            if ((strIds == null) || (string.Empty.Equals(strIds)))
                return;

            var groupList = new List<Domain.Group>();
            var listIds = strIds.Split(',');

            foreach (var groupId in listIds)
            {
                var updateDate = CommonFunc.GetValueFromGrid(grdView, "GroupId", groupId, "UpdateDate");
                var group = new Domain.Group
                {
                    Id = int.Parse(groupId),
                    UpdateDate = DateTime.Parse(updateDate),
                    UpdateBy = LoginInfo.Username
                };

                // Add group to list
                groupList.Add(group);
            }

            EOSCRM.Util.Message msg;

            // Check relation
            if (action == PageAction.Active)
            {
                // Not check relation
                msg = groupDao.UpdateList(groupList, action);
            }
            else
            {
                if (!groupDao.HasRelation(groupList, out msg, action))
                {
                    // Update group list
                    msg = groupDao.UpdateList(groupList, action);
                }
            }

            if ((msg != null) && (msg.MsgType != MessageType.Error))
            {
                // Refresh grid view
                BindDataForGrid(grdView);
                ShowInfor(ResourceLabel.Get(msg));
            }
            else
            {
                ShowError(ResourceLabel.Get(msg));
            }
        }

        /// <summary>
        /// Bind data for grid
        /// </summary>
        /// <param name="gridView"></param>
        private void BindDataForGrid(Grid gridView)
        {
            try
            {
                if (gridView != null)
                {
                    var groupList = groupDao.GetList(null);

                    gridView.DataSource = groupList;
                    grdView.PagerInforText = groupList.Count.ToString();
                    gridView.DataBind();

                    for (var i = 0; i < gridView.Rows.Count; i++)
                    {
                        var group = groupList[i];
                        gridView.Rows[i].Cells[2].Text = group.Active ? Constants.Yes : Constants.No;
                    }
                }
            }
            catch (Exception ex)
            {
                DoError(new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace));
            }
        }
    }
}