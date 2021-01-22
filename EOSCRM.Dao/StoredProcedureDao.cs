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

    #region Get
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

    public DataSet Get_DonDangKy_ByMaddk(string maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk)
                };
        DataSet ds = db.RunExecProc("Get_DonDangKy_ByMaddk", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Get_MauNhanVien_ById(int id)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Id", SqlDbType.Int  , 10, id)
                };
        DataSet ds = db.RunExecProc("Get_MauNhanVien_ById", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Get_MauNhanVien_ByMakvService(string makv, int serviceid)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Makv", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@Serviceid", SqlDbType.Int  , 10, serviceid)
                };
        DataSet ds = db.RunExecProc("Get_MauNhanVien_ByMakvService", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Get_MauNhanVienChiTiet_ByMauNhanVienId(int maunhanvienid)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MauNhanVienId", SqlDbType.Int  , 10, maunhanvienid)                    
                };
        DataSet ds = db.RunExecProc("Get_MauNhanVienChiTiet_ByMauNhanVienId", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Get_MauNhanVienChiTiet_ById(int id)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Id", SqlDbType.Int  , 10, id)
                };
        DataSet ds = db.RunExecProc("Get_MauNhanVienChiTiet_ById", prams);
        db.Dispose();
        return ds;
    }

    #endregion

    #region Insert
    public DataSet Insert_MauNhanVien(string tenmaunhanvien, string masokimm1, string masokimm2, string makv, int serviceid, int sortordder, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                    db.MakeInParam("@TenMauNhanVien", SqlDbType.NVarChar  , 100, tenmaunhanvien),
                    db.MakeInParam("@MaSoKimM1", SqlDbType.NVarChar  , 50, masokimm1),
                    db.MakeInParam("@MaSoKimM2", SqlDbType.NVarChar  , 50, masokimm2),
                    db.MakeInParam("@Makv", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@Serviceid", SqlDbType.Int  , 10, serviceid),
                    db.MakeInParam("@SortOrder", SqlDbType.Int  , 10, sortordder),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv)
                };
        DataSet ds = db.RunExecProc("Insert_MauNhanVien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Insert_MauNhanVienChiTiet(string manvMauChiTiet, int maunhanvienid, int sortordder, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@ManvMauNhanVienChiTiet", SqlDbType.VarChar  , 10, manvMauChiTiet),
                    db.MakeInParam("@MauNhanVienId", SqlDbType.Int  , 10, maunhanvienid),                  
                    db.MakeInParam("@SortOrder", SqlDbType.Int  , 10, sortordder),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv)
                };
        DataSet ds = db.RunExecProc("Insert_MauNhanVienChiTiet", prams);
        db.Dispose();
        return ds;
    }
    #endregion

    #region Update
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

    public DataSet Update_DonDangKy_SoDienThoai2MuaVatTu(string maddk, string sodienthoai2, bool muavattu, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@SoDienThoai2", SqlDbType.VarChar  , 20, sodienthoai2 ),
                    db.MakeInParam("@IsMuaVatTu", SqlDbType.Bit  , 20, muavattu ),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv )
                };
        DataSet ds = db.RunExecProc("Update_DonDangKy_SoDienThoai2MuaVatTu", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Update_MauNhanVien(int id, string tenmaunhanvien, string masokimm1, string masokimm2, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Id", SqlDbType.Int  , 100, id),
                    db.MakeInParam("@TenMauNhanVien", SqlDbType.NVarChar  , 100, tenmaunhanvien),
                    db.MakeInParam("@MaSoKimM1", SqlDbType.NVarChar  , 50, masokimm1),
                    db.MakeInParam("@MaSoKimM2", SqlDbType.NVarChar  , 50, masokimm2),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv)
                };
        DataSet ds = db.RunExecProc("Update_MauNhanVien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Update_MauNhanVienChiTiet_BySortOrder(int maunhanvienchitietid, int sortorder, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MauNhanVienChiTietId", SqlDbType.Int  , 100, maunhanvienchitietid),
                    db.MakeInParam("@SortOrder", SqlDbType.Int  , 100, sortorder),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv)
                };
        DataSet ds = db.RunExecProc("Update_MauNhanVienChiTiet_BySortOrder", prams);
        db.Dispose();
        return ds;
    }

    #endregion

    #region Delete
    public DataSet Delete_DonDangKy_ByMaddk(string maddk, string ghichuxoa)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddk", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@GhiChuDLM", SqlDbType.NVarChar  , 1000, ghichuxoa)
                };
        DataSet ds = db.RunExecProc("Delete_DonDangKy_ByMaddk", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Delete_DonDangKyPo_ByMaddk(string maddkpo, string ghichuxoa)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Maddkpo", SqlDbType.VarChar  , 11, maddkpo),
                    db.MakeInParam("@GhiChuDLM", SqlDbType.NVarChar  , 1000, ghichuxoa)
                };
        DataSet ds = db.RunExecProc("Delete_DonDangKyPo_ByMaddk", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Delete_MauNhanVien(int id, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Id", SqlDbType.Int  , 100, id),                    
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv)
                };
        DataSet ds = db.RunExecProc("Delete_MauNhanVien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Delete_MauNhanVienChiTiet(int id, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@Id", SqlDbType.Int  , 100, id),
                    db.MakeInParam("@Manv", SqlDbType.VarChar  , 50, manv)
                };
        DataSet ds = db.RunExecProc("Delete_MauNhanVienChiTiet", prams);
        db.Dispose();
        return ds;
    }

    #endregion

}
