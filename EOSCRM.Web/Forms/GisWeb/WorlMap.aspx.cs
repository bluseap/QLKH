using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Globalization;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.GisWeb
{
    public partial class WorlMap : Authentication
    {
       

        

        protected void Page_Load(object sender, EventArgs e)
        {

        }

       
        [System.Web.Services.WebMethod]
        public static string GetCurrentTime()
        {
            return "Hello ";

        }


    }
}