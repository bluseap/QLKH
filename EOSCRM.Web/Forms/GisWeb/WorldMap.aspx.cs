using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Domain;
using EOSCRM.Web.Common;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

using System.Data;
using System.Configuration;
using System.Data.SqlClient;

namespace EOSCRM.Web.GisWeb
{
    public partial class WorldMap : Authentication
    {
        private readonly ReportClass _rp = new ReportClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!this.IsPostBack)
            {
                var ll = _rp.LatLongAll();

                //DataTable dt = ll;
                rptMarkers.DataSource = ll;
                rptMarkers.DataBind();
            }
        }

       
    }
}