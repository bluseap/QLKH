using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections;

using EOSCRM.Dao;
using EOSCRM.Util;

public class StoredProcedureDao
{
    public StoredProcedureDao()
    {

    }

    public DataSet Get_HisNgayDangKy_ByMaDDKMoTaTTDON(string maddk, string motattdon)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@MOTATTDON", SqlDbType.VarChar  , 50, motattdon )
                };
        DataSet ds = db.RunExecProc("Get_HisNgayDangKy_ByMaDDKMoTaTTDON", prams);
        db.Dispose();
        return ds;
    }


}
