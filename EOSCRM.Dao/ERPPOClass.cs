using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;

using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ERPPOClass
    {

        public DataSet HisLyLich(string matd, string manvll, string tenbang)
        {
            DataERPPO db = new DataERPPO();
            SqlParameter[] prams = {
				db.MakeInParam("@MATD", SqlDbType.VarChar   , 10, matd ) ,
                db.MakeInParam("@MANVLL", SqlDbType.VarChar   , 10, manvll ),
                db.MakeInParam("@TENBANG", SqlDbType.VarChar   , 20, tenbang )
			};
            DataSet ds = db.RunExecProc("HisLyLich", prams);
            db.Dispose();
            return ds;
        }

        


    }
}
