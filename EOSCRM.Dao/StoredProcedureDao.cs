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

    public DataSet Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON(string maddk, string motattdon)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADDKPO", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@MOTATTDON", SqlDbType.VarChar  , 50, motattdon )
                };
        DataSet ds = db.RunExecProc("Get_HisNgayDangKyPo_ByMaDDKPoMoTaTTDON", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Get_HopDong_ByMaddk(string maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk)                    
                };
        DataSet ds = db.RunExecProc("Get_HopDong_ByMaddk", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Get_HopDongPo_ByMaddk(string maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk)
                };
        DataSet ds = db.RunExecProc("Get_HopDongPo_ByMaddk", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Update_HopDong_GhiChu(string maddk, string ghichu, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@GhiChu", SqlDbType.NVarChar  , 1000, ghichu ),                   
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv )
                };
        DataSet ds = db.RunExecProc("Update_HopDong_GhiChu", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Update_HopDongPo_GhiChu(string maddk, string ghichu, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@GhiChu", SqlDbType.NVarChar  , 1000, ghichu ),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv )
                };
        DataSet ds = db.RunExecProc("Update_HopDongPo_GhiChu", prams);
        db.Dispose();
        return ds;
    }


}
