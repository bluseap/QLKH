﻿using System;
using System.Web.UI;

namespace EOSCRM.Web
{
    public partial class Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Redirect("Forms/Default/", true);
        }
    }
}
