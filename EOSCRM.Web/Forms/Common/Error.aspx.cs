using System;
using EOSCRM.Web.Common;
using EOSCRM.Util;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.Common
{
    public partial class Error : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareUI();

            if (Page.IsPostBack) return;

            var msg = Session[SessionKey.MESSAGE] as EOSCRM.Util.Message;

            if (msg != null)
            {
                if (Resources.Message.ResourceManager != null)

                    if (msg.MsgCode != null)

                        lblmessage.Text = String.Format(Resources.Message.ResourceManager.GetString(msg.MsgCode), msg.Holders);

                // Remove msg
                Session[SessionKey.MESSAGE] = null;
            }
            else
            {
                lblmessage.Text = String.Format(Resources.Message.WARN_PERMISSION_DENIED);
            }
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_ERROR;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_TRANGCHU;
                header.TitlePage = Resources.Message.PAGE_ERROR;
            }
        }
    }
}