using System;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;

namespace EOSCRM.Web.Forms.Group
{
    public partial class PermissionUI : UserControl
    {
        public UserAdmin LoginInfo { get; set; }

        #region Variables

        private List<GroupPermission> groupPermissions;

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            CommonFunc.SetPropertiesForGrid(grdPermission);
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Bind data for grid permission
            BindDataForGrid();

            // Show function of group
            if ((groupPermissions == null) || (groupPermissions.Count <= 0))
                return;

            foreach (var gp in groupPermissions)
            {
                for (var i = 0; i < grdPermission.Rows.Count; i++)
                {
                    var row = grdPermission.Rows[i];
                    var chkFunctionId = (HtmlInputCheckBox)row.FindControl("chkFunctionId");
                    if (gp.FunctionId != int.Parse(chkFunctionId.Value))
                        continue;

                    chkFunctionId.Checked = true;
                    switch (gp.Mash)
                    {
                        case 1:
                            {
                                var chkRead = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkRead");
                                chkRead.Checked = true;
                                break;
                            }
                        case 2:
                            {
                                var chkInsert = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkInsert");
                                chkInsert.Checked = true;
                                break;
                            }
                        case 3:
                            {
                                var chkUpdate = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkUpdate");
                                chkUpdate.Checked = true;
                                break;
                            }
                        case 4:
                            {
                                var chkDelete = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkDelete");
                                chkDelete.Checked = true;
                                break;
                            }
                    }
                    break;
                }
            }
        }

        #region Properties

        public List<GroupPermission> GroupPermissions
        {
            get
            {
                groupPermissions = new List<GroupPermission>();

                for (var i = 0; i < grdPermission.Rows.Count; i++)
                {
                    var row = grdPermission.Rows[i];
                    var chkFunctionId = (HtmlInputCheckBox)row.FindControl("chkFunctionId");

                    if (!chkFunctionId.Checked)
                        continue;

                    var chkRead = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkRead");
                    if (chkRead.Checked)
                    {
                        var gp = GetGroupPermission(int.Parse(chkFunctionId.Value), int.Parse(chkRead.Value));
                        groupPermissions.Add(gp);
                    }

                    var chkInsert = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkInsert");
                    if (chkInsert.Checked)
                    {
                        var gp = GetGroupPermission(int.Parse(chkFunctionId.Value), int.Parse(chkInsert.Value));
                        groupPermissions.Add(gp);
                    }

                    var chkUpdate = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkUpdate");
                    if (chkUpdate.Checked)
                    {
                        var gp = GetGroupPermission(int.Parse(chkFunctionId.Value), int.Parse(chkUpdate.Value));
                        groupPermissions.Add(gp);
                    }

                    var chkDelete = (HtmlInputCheckBox)grdPermission.Rows[i].FindControl("chkDelete");
                    if (!chkDelete.Checked)
                        continue;

                    var gpDel = GetGroupPermission(int.Parse(chkFunctionId.Value), int.Parse(chkDelete.Value));
                    groupPermissions.Add(gpDel);
                }
                return groupPermissions;
            }
            set
            {
                groupPermissions = value;
            }
        }

        #endregion

        /// <summary>
        /// Get Group Permission
        /// </summary>
        /// <param name="functionId"></param>
        /// <param name="mash"></param>
        /// <returns></returns>
        private GroupPermission GetGroupPermission(int functionId, int mash)
        {
            return new GroupPermission
            {
                FunctionId = functionId,
                Mash = mash,
                Active = true,
                Deleted = false,
                CreateBy = LoginInfo.Username,
                CreateDate = DateTime.Now,
                UpdateBy = LoginInfo.Username,
                UpdateDate = DateTime.Now
            };
        }

        /// <summary>
        /// Bind data for grid permission
        /// </summary>
        private void BindDataForGrid()
        {
            if (grdPermission.Rows.Count <= 0)
            {
                grdPermission.DataSource = PermConstants.ListPerms;
                grdPermission.PagerInforText = PermConstants.ListPerms.Count.ToString();
                grdPermission.DataBind();
            }
        }

        protected void grdPermission_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            // Update page index
            grdPermission.PageIndex = e.NewPageIndex;

            // Bind data for grid
            BindDataForGrid();
        }
    }
}