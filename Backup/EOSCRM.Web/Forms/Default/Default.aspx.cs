using System;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.Forms.Default
{
    public partial class Default : Authentication
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            PrepareUI();
        }

        private void PrepareUI()
        {
            Page.Title = Resources.Message.TITLE_TRANGCHU;

            var header = (Header)Master.FindControl("header");
            if (header != null)
            {
                header.ModuleName = Resources.Message.MODULE_TRANGCHU;
                header.TitlePage = Resources.Message.PAGE_TRANGCHU;
            }
        }
    }
}