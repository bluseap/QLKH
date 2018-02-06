using System;
using System.Web.UI;
using EOSCRM.Util;

namespace EOSCRM.Web.Shared
{
    public partial class PO : MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

#pragma warning disable 114,108
        public void RegisterStartupScript(string key, string script)
#pragma warning restore 114,108
        {
            ScriptManager.RegisterStartupScript(phrJsRunner, phrJsRunner.GetType(), key, script, true);
        }

        public void FocusAndSelect(string id)
        {
            RegisterStartupScript(string.Format("jsFocusAndSelect-{0}-{1}", id, Guid.NewGuid()),
                string.Format("FocusAndSelect('{0}');", id));
        }

        public void SetControlValue(string id, string value)
        {
            RegisterStartupScript(string.Format("jsSetControlValue-{0}-{1}", id, Guid.NewGuid()),
                string.Format("setControlValue('{0}', '{1}');", id, value));
        }

        public void SetLabel(string id, string value)
        {
            RegisterStartupScript(string.Format("jsSetLabel-{0}-{1}", id, Guid.NewGuid()),
                string.Format("setLabelText('{0}', '{1}');", id, value));
        }

        public void SetReadonly(string id, bool isReadonly)
        {
            if (isReadonly)
                RegisterStartupScript(string.Format("jsSetReadonly-{0}-{1}", id, Guid.NewGuid()),
                    string.Format("setReadonly('{0}');", id));
            else
                RegisterStartupScript(string.Format("jsRemoveReadonly-{0}-{1}", id, Guid.NewGuid()),
                    string.Format("removeReadonly('{0}');", id));
        }

        public void ShowError(string message, string controlId)
        {
            RegisterStartupScript(string.Format("jsShowErrorWithFocus-{0}-{1}", controlId, Guid.NewGuid()),
                string.Format("showErrorWithFocus('{0}', '{1}');", ConvertUtil.CleanTextHTML2(message), controlId));
        }

        public void ShowError(string message)
        {
            RegisterStartupScript(string.Format("jsShowError-{0}", Guid.NewGuid()),
                string.Format("showError('{0}');", ConvertUtil.CleanTextHTML2(message)));
        }

        public void ShowInfor(string message)
        {
            RegisterStartupScript(string.Format("jsShowInfor-{0}", Guid.NewGuid()),
                string.Format("showInfor('{0}');", ConvertUtil.CleanTextHTML2(message)));
        }

        public void ShowWarning(string message)
        {
            RegisterStartupScript(string.Format("jsShowWarning-{0}", Guid.NewGuid()),
                string.Format("showWarning('{0}');", ConvertUtil.CleanTextHTML2(message)));
        }

        public void HideDialog(string divId)
        {
            RegisterStartupScript(string.Format("jsCloseDialog-{0}-{1}", divId, Guid.NewGuid()),
                string.Format("closeDialog('{0}');", divId));
        }

        public void UnblockDialog(string divId)
        {
            RegisterStartupScript(string.Format("jsUnblockDialog-{0}-{1}", divId, Guid.NewGuid()),
                string.Format("unblockDialog('{0}');", divId));
        }

        public void OpenWaitingDialog()
        {
            RegisterStartupScript(string.Format("jsOpenWaitingDialog-{0}", Guid.NewGuid()),
                "openWaitingDialog();");
        }

        public void UnblockWaitingDialog()
        {
            RegisterStartupScript(string.Format("jsUnblockWaitingDialog-{0}", Guid.NewGuid()),
                "unblockWaitingDialog();");
        }

        public void CloseWaitingDialog()
        {
            RegisterStartupScript(string.Format("jsCloseWaitingDialog-{0}", Guid.NewGuid()),
                "closeWaitingDialog();");
        }
    }
}