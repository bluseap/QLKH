using System;
using System.Web.UI;
using EOSCRM.Domain;
using EOSCRM.Util;
using EOSCRM.Web.Common;

namespace EOSCRM.Web.Forms.Group
{
    public partial class GroupUI : UserControl
    {
        public UserAdmin LoginInfo { get; set; }

        protected void Page_Load(object sender, EventArgs e)
        {
            txtGroupName.Focus();
            UcPermissionUI.LoginInfo = LoginInfo;
        }

        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);

            // Set error message
            reqGroupName.ErrorMessage = String.Format(Resources.Message.E_INVALID_DATA, "Tên nhóm");
            regDesc.ErrorMessage = String.Format(Resources.Message.E0009, "Mô tả", "500");
            regDesc.ValidationExpression = RegExp.MaxLength(500);
        }

        /// <summary>
        /// Get - Set Group
        /// </summary>
        public Domain.Group Group
        {
            get
            {
                var group = new Domain.Group
                {
                    Name = txtGroupName.Text.Trim(),
                    Description = txtDesc.Text.Trim(),
                    CreateBy = LoginInfo.Username,
                    UpdateBy = LoginInfo.Username,
                    Id = int.Parse(lblGroupID.Text),
                    Active = CheckBox_IsActive.Checked
                };
                if ((lblUpdateDate.Text != null) && (!string.Empty.Equals(lblUpdateDate.Text)))
                {
                    group.UpdateDate = Convert.ToDateTime(lblUpdateDate.Text);
                }
                group.GroupPermissions.AddRange(UcPermissionUI.GroupPermissions);

                return group;
            }
            set
            {
                txtGroupName.Text = value.Name;
                txtDesc.Text = value.Description;
                lblGroupID.Text = value.Id.ToString();
                lblUpdateDate.Text = value.UpdateDate.ToString();
                lblIsActive.Text = value.Active.ToString();
                CheckBox_IsActive.Checked = value.Active;
                UcPermissionUI.GroupPermissions.AddRange(value.GroupPermissions);
            }
        }

        public bool InActive
        {
            get
            {
                if (lblIsActive.Text.Equals(Constants.Active.ToString()))
                {
                    if (CheckBox_IsActive.Checked == false)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        
    }
}