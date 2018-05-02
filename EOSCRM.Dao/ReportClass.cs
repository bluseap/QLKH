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


public class ReportClass
{
	public ReportClass()
	{
		
	}

    public DataSet UPDATETTDHPOMOI(string idkh, string makv, string idmadotin, int nam, int thang, decimal chisongung, decimal truythu,
        string madhpo, string sono, decimal chisodau, decimal chisocuoi, decimal mtruythu, int hesonhan, DateTime ngaythay, DateTime ngaybamchi,
        string donghocapban, string lydo, string ghichu, string idkh2, string ghichu2, string manv, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {      
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar  , 20, makv),
                                   db.MakeInParam("@IDMAODTIN", SqlDbType.VarChar  , 20, idmadotin),
                                   db.MakeInParam("@NAM", SqlDbType.Int  , 20, nam),
                                   db.MakeInParam("@THANG", SqlDbType.Int  , 20, thang),
                                   db.MakeInParam("@CHISONGUNG", SqlDbType.Decimal  , 20, chisongung),
                                   db.MakeInParam("@TRUYTHU", SqlDbType.Decimal  , 20, truythu),
                                   db.MakeInParam("@MADHPO", SqlDbType.VarChar  , 20, madhpo),
                                   db.MakeInParam("@SONO", SqlDbType.NVarChar  , 50, sono),
                                   db.MakeInParam("@CHISODAU", SqlDbType.Decimal  , 20, chisodau),
                                   db.MakeInParam("@CHISOCUOI", SqlDbType.Decimal  , 20, chisocuoi),
                                   db.MakeInParam("@MTRUYTHU", SqlDbType.Decimal  , 20, mtruythu),
                                   db.MakeInParam("@HESONHAN", SqlDbType.Int  , 20, hesonhan),
                                   db.MakeInParam("@NGAYTHAY", SqlDbType.DateTime  , 20, ngaythay),
                                   db.MakeInParam("@NGAYBAMCHI", SqlDbType.DateTime  , 20, ngaybamchi),
                                   db.MakeInParam("@DONGHOCAPBAN", SqlDbType.NVarChar  , 20, donghocapban),
                                   db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 50, lydo),
                                   db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 1000, ghichu),
                                   db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2),
                                   db.MakeInParam("@GHICHU2", SqlDbType.NVarChar  , 1000, ghichu2),                    

                                   db.MakeInParam("@MANV", SqlDbType.VarChar  , 20, manv),
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("UPDATETTDHPOMOI", prams);
        db.Dispose();
        return ds;
    }

    public DataSet TinhTienTheoBac(int thang, int nam, string makv, string madotin, int sohoadondau, int sohoadoncuoi, string ghichu, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@THANG", SqlDbType.Int  , 20, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 20, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 20, makv),
                    db.MakeInParam("@MADOTINHD", SqlDbType.VarChar  , 20, madotin),
                    db.MakeInParam("@SOHOADONDAU", SqlDbType.Int  , 20, sohoadondau),
                    db.MakeInParam("@SOHOADONCUOI", SqlDbType.Int  , 20, sohoadoncuoi),
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 1000, ghichu),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("TinhTienTheoBac", prams);
        db.Dispose();
        return ds;
    }

    public DataSet HisTableCoBien(string idkh, string idkh2, string madon, string madon2, string makv, DateTime tungay, DateTime denngay, int thang,
        int nam, string ghichu, string ghichu2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 20, idkh),
                    db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 20, idkh2),
                    db.MakeInParam("@MADON", SqlDbType.VarChar  , 20, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 20, madon2),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 20, makv),
                     db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 50, tungay),
                      db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 50, denngay),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 8, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 8, nam),
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 1000, ghichu),
                    db.MakeInParam("@GHICHU2", SqlDbType.NVarChar  , 1000, ghichu2),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("HisTableCoBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpLoadFileDuongPho(string maup, string maup2, string makv, string mapb, string tenfile, string tenfile2,
        DateTime tungay, DateTime denngay, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MAUP", SqlDbType.VarChar  ,20, maup),                    
                                   db.MakeInParam("@MAUP2", SqlDbType.VarChar  ,20, maup2),     
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar  ,10, makv),     
                                   db.MakeInParam("@MAPB", SqlDbType.VarChar  ,10, mapb),     
                                   db.MakeInParam("@TENFILE", SqlDbType.NVarChar  ,300, tenfile),     
                                   db.MakeInParam("@TENFILE2", SqlDbType.NVarChar  ,300, tenfile2),     

                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  ,20, tungay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  ,20, denngay),                    
                    
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("UpLoadFileDuongPho", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTongHopDDK(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb, string madon, string madon2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  ,20, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  ,20, denNgay),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 11, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                    db.MakeInParam("@MADON", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)

                };
        DataSet ds = db.RunExecProc("DSTongHopDDK", prams);
        db.Dispose();
        return ds;
    }

    public DataSet HisTieuThuBien(string idkh, string idkh2, int thang, int nam, string makv, DateTime ngaynhap, DateTime tungay,
            DateTime denngay, string ghichu, string ghichu2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                    db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 11, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 11, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 20, ngaynhap),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 20, tungay),                    
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 20, denngay), 
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 500, ghichu),
                    db.MakeInParam("@GHICHU2", SqlDbType.NVarChar  , 500, ghichu2),                    
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("HisTieuThuBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHTTCOBIEN(string idkh, string idkh2, string makv, int thang, int nam, string sonha2, string mamdsd,
            string madp, string madb, string idkhlx, decimal tiencoclx, decimal chisodau, decimal chisocuoi, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                    db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 11, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 11, nam),
                    db.MakeInParam("@SONHA2", SqlDbType.NVarChar  , 500, sonha2),
                    db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd),
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),
                    db.MakeInParam("@MADB", SqlDbType.VarChar  , 11, madb),
                    db.MakeInParam("@IDKHLX", SqlDbType.VarChar  , 20, idkhlx),
                    db.MakeInParam("@TIENCOCLX", SqlDbType.Decimal  , 20, tiencoclx),
                    db.MakeInParam("@CHISODAU", SqlDbType.Decimal  , 20, chisodau),
                    db.MakeInParam("@CHISOCUOI", SqlDbType.Decimal  , 20, chisocuoi),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )             
                };
        DataSet ds = db.RunExecProc("UPKHTTCOBIEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet HisNgayDangKyBienPo(string madon, string madon2, string makvpo, DateTime ngaynhap, DateTime tungay,
            DateTime denngay, string mota, string mota2, string ghichu, string ghichu2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {db.MakeInParam("@MADON", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makvpo),

                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 8, ngaynhap),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tungay),                    
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denngay),     

                    db.MakeInParam("@MOTA", SqlDbType.VarChar  , 50, mota),
                    db.MakeInParam("@MOTA2", SqlDbType.VarChar  , 50, mota2),
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 500, ghichu),
                    db.MakeInParam("@GHICHU2", SqlDbType.NVarChar  , 500, ghichu2),
                    
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("HisNgayDangKyBienPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet HisNgayDangKyBien(string madon, string madon2, string makv, DateTime ngaynhap, DateTime tungay,
            DateTime denngay, string mota, string mota2, string ghichu, string ghichu2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {db.MakeInParam("@MADON", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),

                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 8, ngaynhap),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tungay),                    
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denngay),     

                    db.MakeInParam("@MOTA", SqlDbType.VarChar  , 50, mota),
                    db.MakeInParam("@MOTA2", SqlDbType.VarChar  , 50, mota2),
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 500, ghichu),
                    db.MakeInParam("@GHICHU2", SqlDbType.NVarChar  , 500, ghichu2),
                    
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("HisNgayDangKyBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet TongHopCPTK(string maddk, string maddk2, string maddk3, decimal? t, decimal? c, decimal? tl, decimal? g,
                        decimal? vat1, decimal? g1, decimal? tk, decimal? vat2, decimal? g2, decimal? vt, decimal? vat3, decimal? g3,
                        decimal? vc, decimal? vat4, decimal? g4, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk),
                    db.MakeInParam("@MADDK2", SqlDbType.VarChar  , 11, maddk2),
                    db.MakeInParam("@MADDK3", SqlDbType.VarChar  , 11, maddk3),
                   
                    db.MakeInParam("@T", SqlDbType.Decimal, 23 , t),
                    db.MakeInParam("@C", SqlDbType.Decimal, 23 , c),
                    db.MakeInParam("@TL", SqlDbType.Decimal, 23 , tl),
                    db.MakeInParam("@G", SqlDbType.Decimal, 23 , g),
                    db.MakeInParam("@VAT1", SqlDbType.Decimal, 23 , vat1),
                    db.MakeInParam("@G1", SqlDbType.Decimal, 23 , g1),
                    db.MakeInParam("@TK", SqlDbType.Decimal, 23 , tk),
                    db.MakeInParam("@VAT2", SqlDbType.Decimal, 23 , vat2),
                    db.MakeInParam("@G2", SqlDbType.Decimal, 23 , g2),
                    db.MakeInParam("@VT", SqlDbType.Decimal, 23 , vt),
                    db.MakeInParam("@VAT3", SqlDbType.Decimal, 23 , vat3),
                    db.MakeInParam("@G3", SqlDbType.Decimal, 23 , g3),
                    db.MakeInParam("@VC", SqlDbType.Decimal, 23 , vc),
                    db.MakeInParam("@VAT4", SqlDbType.Decimal, 23 , vat4),
                    db.MakeInParam("@G4", SqlDbType.Decimal, 23 , g4),

                    db.MakeInParam("@COBIEN ", SqlDbType.VarChar  , 20, cobien )             
                };
        DataSet ds = db.RunExecProc("TongHopCPTK", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BienKHPoTuDenNgay(DateTime tuNgay, DateTime denNgay, string idkhpo, string maddkpo, string makvpo,
                        string tttk, string tthd, string tttc, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 10, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 10, denNgay),
                    db.MakeInParam("@IDKHPO", SqlDbType.VarChar  , 11, idkhpo ),
                    db.MakeInParam("@MADDKPO", SqlDbType.VarChar  , 11, maddkpo),
                    db.MakeInParam("@MAKVPO", SqlDbType.VarChar  , 11, makvpo),
                    db.MakeInParam("@TTTK", SqlDbType.VarChar  , 10, tttk ),
                    db.MakeInParam("@TTHD", SqlDbType.VarChar  , 10, tthd),
                    db.MakeInParam("@TTTC", SqlDbType.VarChar  , 10, tttc ),
                    db.MakeInParam("@COBIEN ", SqlDbType.VarChar  , 20, cobien )             
                };
        DataSet ds = db.RunExecProc("BienKHPoTuDenNgay", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Pro_Menu_List(int ItemID, int IsVisible)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@ItemID", SqlDbType.Int  , 3, ItemID),
                    db.MakeInParam("@IsVisible", SqlDbType.Int  , 3, IsVisible)                    
                };
        DataSet ds = db.RunExecProc("Pro_Menu_List", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSQuiTrinhPoBien(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb, string madon, string madon2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 11, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                    db.MakeInParam("@MADON", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)

                };
        DataSet ds = db.RunExecProc("DSQuiTrinhPoBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSQuiTrinhNuocBien(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb, string madon, string madon2,string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  ,20, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  ,20, denNgay),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 11, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                    db.MakeInParam("@MADON", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)

                };
        DataSet ds = db.RunExecProc("DSQuiTrinhNuocBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BienKHPo(string madon, string makv, string madon2, string madon3, int thang, int nam, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@MADON3", SqlDbType.VarChar  , 11, madon3),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("BienKHPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BienKHNuoc(string madon, string makv, string madon2, string madon3, int thang, int nam,string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@MADON3", SqlDbType.VarChar  , 11, madon3),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("BienKHNuoc", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BienKHNuocLX(string madon, string madon2, string makv, string idkh, string idkh2, int thang, int nam, DateTime tungay,
            DateTime denngay, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, madon),
                    db.MakeInParam("@MADON2", SqlDbType.VarChar  , 11, madon2),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),                    
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                    db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 20, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 20, nam),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 20, tungay),
                    db.MakeInParam("@DENGAY", SqlDbType.DateTime  , 20, denngay),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("BienKHNuocLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSKhoaSoDotIn(DateTime kygt, string makv, string idma, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                       
                    db.MakeInParam("@KYGT", SqlDbType.DateTime  , 8, kygt),
                    db.MakeInParam("@MAKVPO", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@IDMA", SqlDbType.VarChar  , 10, idma),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("DSKhoaSoDotIn", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHCOBIEN(string idkh, string makv, int thang, int nam, string tenkh1, string tenkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),      
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@TENKH1", SqlDbType.NVarChar  , 200, tenkh1 ),
                     db.MakeInParam("@TENKH2", SqlDbType.NVarChar  , 200, tenkh2 ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien ),
                };
        DataSet ds = db.RunExecProc("UPKHCOBIEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPTHayDoiCTPO(string idkh, int thang, int nam, string cobien, 
            string madp, string madb, string duongphu, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKHPO", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien),

                     db.MakeInParam("@MADPPO", SqlDbType.NVarChar  , 50, madp ),// dia chi lap, ma so thue, mamuc dich su dung
                     db.MakeInParam("@MADBPO", SqlDbType.NVarChar  , 50, madb ),
                     db.MakeInParam("@DUONGPHUPO", SqlDbType.NVarChar  , 50, duongphu),                     
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPTHayDoiCTPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpHoNgheoHis(string mangheo, string idkh, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = { db.MakeInParam("@MANGHEO", SqlDbType.VarChar  , 10, mangheo ),
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )                    
                };
        DataSet ds = db.RunExecProc("UpHoNgheoHis", prams);
        db.Dispose();
        return ds;
    }

    public DataSet dsKHCBiKTPO(string makv, string maphuong, string cobien, DateTime tungay, DateTime denngay)
    {
        Database db = new Database();
        SqlParameter[] prams = { db.MakeInParam("@MAKVPO", SqlDbType.VarChar  , 11, makv ),
                    db.MakeInParam("@MAPHUONG", SqlDbType.VarChar  , 11, maphuong ),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 15, cobien ),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 25, tungay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 25, denngay)   
                };
        DataSet ds = db.RunExecProc("dsKHCBiKTPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTGIAM6THANG(int thang, int nam, string makv, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = { 
                    db.MakeInParam("@THANG", SqlDbType.Int  , 10, thang),        
                    db.MakeInParam("@NAM", SqlDbType.Int  , 10, nam),        
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),                    
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )                    
                };
        DataSet ds = db.RunExecProc("DSTTGIAM6THANG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTMUCDICHKHAC(int tuthang, int tunam, int denthang, int dennam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = { 
                    db.MakeInParam("@TUTHANG", SqlDbType.Int  , 10, tuthang),        
                    db.MakeInParam("@TUNAM", SqlDbType.Int  , 10, tunam),    
                    db.MakeInParam("@DENTHANG", SqlDbType.Int  , 10, denthang),        
                    db.MakeInParam("@DENNAM", SqlDbType.Int  , 10, dennam),      
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),                    
                    db.MakeInParam("@IDKH", SqlDbType.NVarChar  , 50, idkh ),
                    db.MakeInParam("@IDKH2", SqlDbType.NVarChar  , 50, idkh2 ),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )                    
                };
        DataSet ds = db.RunExecProc("DSTTMUCDICHKHAC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTDK3THANG(int thang, int nam, string makv, int kltieuthu, string ghichu, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = { 
                    db.MakeInParam("@THANG", SqlDbType.Int  , 10, thang),        
                    db.MakeInParam("@NAM", SqlDbType.Int  , 10, nam),        
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),    
                    db.MakeInParam("@KLTIEUTHU", SqlDbType.Int  , 10, kltieuthu ),    
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 50, ghichu ),    
                    
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )                    
                };
        DataSet ds = db.RunExecProc("DSTTDK3THANG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet dsKHCBiKT(string makv, string maphuong, string cobien, DateTime tungay, DateTime denngay)
    {
        Database db = new Database();
        SqlParameter[] prams = { db.MakeInParam("@MAKV", SqlDbType.VarChar  , 11, makv ),
                    db.MakeInParam("@MAPHUONG", SqlDbType.VarChar  , 11, maphuong ),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 15, cobien ),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 25, tungay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 25, denngay)   
                };
        DataSet ds = db.RunExecProc("dsKHCBiKT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InTachDuong(int thang, int nam, string madp, string manv,string bienco)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 10, thang),                    
                    db.MakeInParam("@NAM", SqlDbType.Int  , 10, nam),
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 8, madp),
                    db.MakeInParam("@MANV", SqlDbType.VarChar  , 10, manv),
                    db.MakeInParam("@BIENCO", SqlDbType.VarChar  , 11, bienco)  
                };
        DataSet ds = db.RunExecProc("InTachDuong", prams);
        db.Dispose();
        return ds;
    }

    public DataSet HisApKhomBien(string maap, string maxa, string makv, string manvn, string bienco)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MAAP", SqlDbType.VarChar  , 10, maap),  
                    db.MakeInParam("@MAXA", SqlDbType.VarChar  , 10, maxa),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@BIENCO", SqlDbType.VarChar  , 10, bienco),
                };
        DataSet ds = db.RunExecProc("HisApKhomBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet HisBienCo(string maxa, string makv, string manvn, string bienco)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MAXA", SqlDbType.VarChar  , 20, maxa),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 20, makv),
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 20, manvn),
                    db.MakeInParam("@BIENCO", SqlDbType.VarChar  , 20, bienco),
                };
        DataSet ds = db.RunExecProc("HisBienCo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DongHo_His(string madh, string manvn, DateTime ngaynhap)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADH", SqlDbType.VarChar  , 20, madh),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 20, manvn),
                    db.MakeInParam("@NGAYN", SqlDbType.DateTime  , 20, ngaynhap)                    

                };
        DataSet ds = db.RunExecProc("DongHo_His", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DongHoPo_His(string madh, string manvn, DateTime ngaynhap)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADH", SqlDbType.VarChar  , 20, madh),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 20, manvn),
                    db.MakeInParam("@NGAYN", SqlDbType.DateTime  , 20, ngaynhap)                    

                };
        DataSet ds = db.RunExecProc("DongHoPo_His", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KHDongHoNuocBien(string madh, string madh2, string sono, string sono2, string maldhcu, string maldhmoi,
                       string congsuatcu, string congsuatmoi,
                        string idkh, string madp, string madb, string makv, string tenkh,
                        string manv,string ghichu, DateTime tungay, DateTime denngay, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADH", SqlDbType.VarChar  , 20, madh),                    
                    db.MakeInParam("@MADH2", SqlDbType.VarChar  , 20, madh2),
                    db.MakeInParam("@SONO", SqlDbType.VarChar  , 50, sono),
                    db.MakeInParam("@SONO2", SqlDbType.VarChar  , 50, sono2),

                    db.MakeInParam("@MALDHCU", SqlDbType.VarChar  , 10, maldhcu),
                    db.MakeInParam("@MALDHMOI", SqlDbType.VarChar  , 10, maldhmoi),
                    db.MakeInParam("@CONGSUATCU", SqlDbType.VarChar  , 10, congsuatcu),
                    db.MakeInParam("@CONGSUATMOI", SqlDbType.VarChar  , 10, congsuatmoi),

                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 11, madp),
                    db.MakeInParam("@MADB", SqlDbType.VarChar  , 11, madb),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@TENKH", SqlDbType.NVarChar  , 200, tenkh),

                    db.MakeInParam("@MANV", SqlDbType.VarChar  , 10, manv),
                    db.MakeInParam("@GHICHU", SqlDbType.NVarChar  , 300, ghichu),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 20, tungay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 20, denngay),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)  
                };
        DataSet ds = db.RunExecProc("KHDongHoNuocBien", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KHDuongPho(string madp, string manvn, DateTime ngaynhap, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 8, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)      

                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KHDuongPhoPo(string madp, string manvn, DateTime ngaynhap, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 8, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)      

                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHOPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KHDuongPhoTen(string madp, string manvn, DateTime ngaynhap, string makv, string idmadotin)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 8, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)   ,
                    db.MakeInParam("@IDMADOTIN", SqlDbType.VarChar  , 10, idmadotin) 

                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHOTEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHDUONGPHODOTIN(string madp, string manvn, DateTime ngaynhap, string makv, string idmadotincu, string idmadotin)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 20, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)   ,
                    db.MakeInParam("@IDMADOTINCU", SqlDbType.VarChar  , 10, idmadotincu) ,
                    db.MakeInParam("@IDMADOTIN", SqlDbType.VarChar  , 10, idmadotin) 
                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHODOTIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHDUONGPHOPHIENLX(string madp, string manvn, DateTime ngaynhap, string makv, string dotcu, string dot, string idmadp, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 20, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)   ,
                    db.MakeInParam("@DOTCU", SqlDbType.VarChar  , 10, dotcu) ,
                    db.MakeInParam("@DOT", SqlDbType.VarChar  , 10, dot),
                    db.MakeInParam("@IDMADP", SqlDbType.VarChar  , 20, idmadp) ,
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien) 
                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHOPHIENLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KHDuongPhoTenPo(string madp, string manvn, DateTime ngaynhap, string makv, string idmadotin)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 20, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)   ,
                    db.MakeInParam("@IDMADOTIN", SqlDbType.VarChar  , 10, idmadotin) 

                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHOTENPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHDUONGPHODOTINPO(string madp, string manvn, DateTime ngaynhap, string makv, string idmadotincu, string idmadotin)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp),                    
                    db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn),
                    db.MakeInParam("@NGAYNHAP", SqlDbType.DateTime  , 20, ngaynhap),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)   ,
                    db.MakeInParam("@IDMADOTINCU", SqlDbType.VarChar  , 10, idmadotincu) ,
                    db.MakeInParam("@IDMADOTIN", SqlDbType.VarChar  , 10, idmadotin) 
                };
        DataSet ds = db.RunExecProc("UPKHDUONGPHODOTINPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DuongPho(string makv) 
    {
        Database db = new Database();
        SqlParameter[] prams = {
				db.MakeInParam("@MAKV", SqlDbType.VarChar   , 50, makv ) 
			};
        DataSet ds = db.RunExecProc("RPDUONGPHO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Rpvattu()
    {
        Database db = new Database();
        
        DataSet ds = db.RunExecProc("RPVATTU");
        db.Dispose();
        return ds;
    }

    public DataSet VayKyTietKiem(string manvv, DateTime kybatdau, DateTime kyketthuc, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MANVV", SqlDbType.VarChar  , 10, manvv),                    
                    db.MakeInParam("@KYBATDAU", SqlDbType.DateTime  , 8, kybatdau),
                    db.MakeInParam("@KYKETTHUC", SqlDbType.DateTime  , 8, kyketthuc),
                    db.MakeInParam("@MANV", SqlDbType.VarChar  , 10, manv)                    

                };
        DataSet ds = db.RunExecProc("VAYKYTIETKIEM", prams);
        db.Dispose();
        return ds;
    }

    public DataSet VayLanTKCD(string malv, string manvv, decimal tienvay, decimal laisuat, DateTime kybatdau, DateTime kyketthuc,
                                string manvnhap, decimal tiengoc, decimal tienlai, string mahttt, string thanhtoan)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MALV", SqlDbType.VarChar  , 10, malv),                    
                    db.MakeInParam("@MANVV", SqlDbType.VarChar  , 10, manvv),
                    db.MakeInParam("@TIENVAY", SqlDbType.Decimal  , 9, tienvay),
                    db.MakeInParam("@LAISUAT", SqlDbType.Decimal  , 15, laisuat),        
                    db.MakeInParam("@KYBATDAU", SqlDbType.DateTime  , 8, kybatdau),
                    db.MakeInParam("@KYKETTHUC", SqlDbType.DateTime  , 8, kyketthuc),
                    db.MakeInParam("@MANVNHAP", SqlDbType.VarChar  , 10, manvnhap),
                    db.MakeInParam("@TIENGOC", SqlDbType.Decimal  , 9, tiengoc),
                    db.MakeInParam("@TIENLAI", SqlDbType.Decimal  , 9, tienlai),
                    db.MakeInParam("@MAHTTT", SqlDbType.VarChar  , 2, mahttt),  
                    db.MakeInParam("@THANHTOAN", SqlDbType.VarChar  , 2, thanhtoan)

                };
        DataSet ds = db.RunExecProc("VAYLANTKCD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet VayUpkyNhanVien(string manvv, int nam, int thang, decimal tiengoc, decimal tienlai, string mahttt, 
                                    decimal tongtien, string thanhtoan)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                    
                    db.MakeInParam("@MANVV", SqlDbType.VarChar  , 10, manvv),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),                    
                    db.MakeInParam("@TIENGOC", SqlDbType.Decimal  , 9, tiengoc),
                    db.MakeInParam("@TIENLAI", SqlDbType.Decimal  , 9, tienlai),
                    db.MakeInParam("@MAHTTT", SqlDbType.VarChar  , 2, mahttt),  
                    db.MakeInParam("@TONGTIEN", SqlDbType.Decimal  , 9, tongtien),
                    db.MakeInParam("@THANHTOAN", SqlDbType.VarChar  , 2, thanhtoan)

                };
        DataSet ds = db.RunExecProc("VAYUPKYNHANVIEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhoiTaoKyVay( DateTime kydau, DateTime kycuoi, string manv)
    {
        Database db = new Database();
        SqlParameter[] prams = {                                     
                    db.MakeInParam("@KYBATDAU", SqlDbType.DateTime  , 8, kydau),
                    db.MakeInParam("@KYKETTHUC", SqlDbType.DateTime  , 8, kycuoi),
                    db.MakeInParam("@MANV", SqlDbType.VarChar  , 10, manv)

                };
        DataSet ds = db.RunExecProc("VAYLANKYTKCD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdscdDangKy(DateTime tuNgay, DateTime denNgay, string khuVuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),

                };
        DataSet ds = db.RunExecProc("RPDSCDDangKy" , prams );
        db.Dispose();
        return ds;
    }

    public DataSet RpdscdDangKyPB(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                };
        DataSet ds = db.RunExecProc("RPDSCDDangKyPB", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdscdDangKyPBNgayN(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                };
        DataSet ds = db.RunExecProc("RpdscdDangKyPBNgayN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdscdDangKyPBNgayNPo(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                };
        DataSet ds = db.RunExecProc("RpdscdDangKyPBNgayNPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSDaThietKe(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),                    
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb )
                };
        DataSet ds = db.RunExecProc("DSDaThietKe", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSDaThietKePo(DateTime tuNgay, DateTime denNgay, string khuVuc, string mapb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@MAPB", SqlDbType.VarChar  , 10, mapb ),

                };
        DataSet ds = db.RunExecProc("DSDaThietKePo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdsDonSuaChua(DateTime tuNgay, DateTime denNgay, string maKv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, maKv )
                };
        DataSet ds = db.RunExecProc("RPDONSUACHUA", prams);
        db.Dispose();
        return ds;
    }

    //Chung cho Chưa, đã, chờ thiết kế
    public DataSet RpdsChoThietKe(DateTime tuNgay, DateTime denNgay, string khuVuc, string tttk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTTK", SqlDbType.VarChar  , 10, tttk),
                };
        DataSet ds = db.RunExecProc("RPDSCDThietKe", prams);
        db.Dispose();
        return ds;
    }

    public DataSet dskoBVTThietKe(DateTime tuNgay, DateTime denNgay, string khuVuc, string tttk, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTTK", SqlDbType.VarChar  , 10, tttk),

                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                    db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("dskoBVTThietKe", prams);
        db.Dispose();
        return ds;
    }

    //Chung cho Chưa, đã, chờ Thi Cong
    public DataSet RpdsThiCong(DateTime tuNgay, DateTime denNgay, string khuVuc, string tttc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTTC", SqlDbType.VarChar  , 10, tttc),
                };
        DataSet ds = db.RunExecProc("RPDSCDThiCong", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdsChoLapCt(DateTime tuNgay, DateTime denNgay, string khuVuc, string ttct)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTCT", SqlDbType.VarChar  , 10, ttct),
                };
        DataSet ds = db.RunExecProc("RPDSCDChietTinh", prams);
        db.Dispose();
        return ds;
    }
    public DataSet RpDONSUACHUALAPCHIETTINH(DateTime tuNgay, DateTime denNgay, string khuVuc, string ttct)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTCT", SqlDbType.VarChar  , 10, ttct),
                };
        DataSet ds = db.RunExecProc("RPDONSUACHUALAPCHIETTINH", prams);
        db.Dispose();
        return ds;
    }
    public DataSet RpdsDaLapCt(DateTime tuNgay, DateTime denNgay, string khuVuc, string ttct)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTCT", SqlDbType.VarChar  , 10, ttct),
                };
        DataSet ds = db.RunExecProc("RPDSCDChietTinh", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoBVTCTLX(string maddk, string maddk2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ),
                     db.MakeInParam("@MADDK2", SqlDbType.VarChar  , 11, maddk2 ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("BaoCaoBVTCTLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoBVTCTLXKHTT100(string maddk, string maddk2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ),
                     db.MakeInParam("@MADDK2", SqlDbType.VarChar  , 11, maddk2 ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("BaoCaoBVTCTLXKHTT100", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoLapChietTinh(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoLapChietTinh", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoLapQuyetToan(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoLapQuyetToan", prams);
        db.Dispose();
        return ds;
    }
    public DataSet BaoCaoLapQuyetToanSuaChua(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoLapQuyetToanSuaChua", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoLapChietTinhSuaChua(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoLapChietTinhSuaChua", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdsHopDong(DateTime tuNgay, DateTime denNgay, string khuVuc, string tthd)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTHD", SqlDbType.VarChar  , 10, tthd),
                };
        DataSet ds = db.RunExecProc("RPDSHopDongChuaKhaiThac", prams);
        db.Dispose();
        return ds;
    }    
    
    public DataSet RpdsDaHopDong(DateTime tuNgay, DateTime denNgay, string khuVuc, string tthd)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTHD", SqlDbType.VarChar  , 10, tthd),
                };
        DataSet ds = db.RunExecProc("RPDSDaHopDongChuaKhaiThac", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdsChietTinhKoKhaiThac(DateTime tuNgay, DateTime denNgay, string khuVuc, string tthd)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc ),
                    db.MakeInParam("@TTHD", SqlDbType.VarChar  , 10, tthd),
                };
        DataSet ds = db.RunExecProc("RPDSChietTinhKoKhaiThac", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangDeNghiXuatVatTu(DateTime tuNgay, DateTime denNgay, string khuVuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc )
                };
        DataSet ds = db.RunExecProc("BangDeNghiXuatVatTu", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangTongHopQuyetToan(DateTime tuNgay, DateTime denNgay, string khuVuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc )
                };
        DataSet ds = db.RunExecProc("BangQuyetToanTongHop", prams);
        db.Dispose();
        return ds;
    }

    /// <summary>
    /// Danh sách kiểm tra ghi chỉ số
    /// </summary>
    /// <param name="thang">Tháng </param>
    /// <param name="nam">Năm </param>
    /// <param name="strMadp">Danh sách đường phố</param>
    /// <returns></returns>
    public DataSet Dsktky(int thang, int nam, string strMadp)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@STR_MADP", SqlDbType.VarChar  , 400, strMadp ),
                };
        DataSet ds = db.RunExecProc("PROC_DSKTKY", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Dsktdo(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@MAKV1", SqlDbType.VarChar  , 10, makv)                    
                };
        DataSet ds = db.RunExecProc("PROC_DSKTDO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KiemDo3T(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)                    
                };
        DataSet ds = db.RunExecProc("KIEMDO3T", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KiemDo3TDotIn(int thang, int nam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh),
                    db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien)
                };
        DataSet ds = db.RunExecProc("KIEMDO3TDOTIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KiemDo4T(int thang, int nam, string makv, string duongpho)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv),
                    db.MakeInParam("@CHUOIDP", SqlDbType.VarChar  , 1000, duongpho)
                };
        DataSet ds = db.RunExecProc("KIEMDO4T", prams);
        db.Dispose();
        return ds;
    }

    public DataSet Dskh(string madp, string duongphu, string mamdsd, string ttsd, string makv, bool isdinhmuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@TTSD", SqlDbType.VarChar, 50, ttsd),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                    db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdinhmuc),
                                  };
        DataSet ds = db.RunExecProc("DSKH", prams);
        db.Dispose();
        return ds;
    }
    public DataSet Dskh(string madp, string duongphu, string mamdsd, string ttsd, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@TTSD", SqlDbType.VarChar, 50, ttsd),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   
                                  };
        DataSet ds = db.RunExecProc("DSKH", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSKHTS(string madp, string duongphu, string mamdsd, string ttsd, string makv, bool isdinhmuc, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@TTSD", SqlDbType.VarChar, 50, ttsd),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdinhmuc),
                                   db.MakeInParam("@THANG", SqlDbType.Int, 2, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam)
                                  };
        DataSet ds = db.RunExecProc("DSKHTS", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DsTieuThuDk(int thang, int nam, string madp, string duongphu, string mamdsd, string ttsd, string makv, int kltu, int klten, decimal tongtientu, decimal tongtienden)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@TTSD", SqlDbType.VarChar, 50, ttsd),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@KLTU", SqlDbType.Int, 4, kltu),
                                   db.MakeInParam("@KLDEN", SqlDbType.Int, 4, klten),
                                   db.MakeInParam("@TONGTIENTU", SqlDbType.Decimal, 17, tongtientu),
                                   db.MakeInParam("@TONGTIENDEN", SqlDbType.Decimal, 17, tongtienden),
                               };
        DataSet ds = db.RunExecProc("DSTIEUTHUDK", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTBANG1(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTTBANG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICHITIETNUOCDPLX(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICHITIETNUOCDPLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICHITIETNUOCLX(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICHITIETNUOCLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTDCTNLX(int thang, int nam, string makv, string dotin, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@DOTIN", SqlDbType.VarChar, 11, dotin),    
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 11, idkh),   
                                   db.MakeInParam("@IDKH2", SqlDbType.VarChar, 11, idkh2),
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 20, cobien)    
                               };
        DataSet ds = db.RunExecProc("DSTDCTNLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICHITIETNUOCDOTINLX(int thang, int nam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 11, idkh),    
                                   db.MakeInParam("@IDKH2", SqlDbType.VarChar, 11, idkh2),   
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 20, cobien)    
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICHITIETNUOCDOTINLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICHITIETNUOC(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICHITIETNUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICHITIETNUOCDOTIN(int thang, int nam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 11, idkh),    
                                   db.MakeInParam("@IDKH2", SqlDbType.VarChar, 11, idkh2),   
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 20, cobien)    
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICHITIETNUOCDOTIN", prams);
        db.Dispose();
        return ds;
    }
    //DSTHAYDOICHITIETDIEN
    public DataSet DSTHAYDOICHITIETDIEN(int thang, int nam, string makv, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKVPO", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 10, cobien) 
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICHITIETDIEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICTPODOTIN(int thang, int nam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKVPO", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 11, idkh), 
                                   db.MakeInParam("@IDKH2", SqlDbType.VarChar, 11, idkh2), 
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 20, cobien) 
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICTPODOTIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICTPOP7D1(int thang, int nam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKVPO", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 11, idkh), 
                                   db.MakeInParam("@IDKH2", SqlDbType.VarChar, 11, idkh2), 
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 20, cobien) 
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICTPOP7D1", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTHAYDOICTMUCDKPO(int thang, int nam, string makv, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKVPO", SqlDbType.VarChar, 10, makv),
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 10, cobien) 
                               };
        DataSet ds = db.RunExecProc("DSTHAYDOICTMUCDKPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTBTCLD(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTTBTCLD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTBTKLD(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTTBTKLD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSTTGIAM(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 10, makv)                                   
                               };
        DataSet ds = db.RunExecProc("DSTTGIAM", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DskhMoi(int thang, int nam, string madp, string duongphu, string mamdsd, string ttsd, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@TTSD", SqlDbType.VarChar, 50, ttsd),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv)
                                  };
        DataSet ds = db.RunExecProc("DSKHMOI", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DskhMoiDotIn(int thang, int nam, string madp, string duongphu, string mamdsd, string ttsd, string makv,
                                    string idmadot, string idmadotin2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@TTSD", SqlDbType.VarChar, 50, ttsd),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@IDMADOTIN", SqlDbType.VarChar, 11, idmadot),
                                   db.MakeInParam("@IDMADOTIN2", SqlDbType.VarChar, 11, idmadotin2),
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 20, cobien)
                                  };
        DataSet ds = db.RunExecProc("DSKHMOIDOTIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSKHTACHDUONG(int thang, int nam, string makv, int co)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@BIENCO", SqlDbType.Int, 2, co)
                                  };
        DataSet ds = db.RunExecProc("DSKHTACHDUONG", prams);
        db.Dispose();
        return ds;
    }
  
    public DataSet DSKHXOABO(int thang, int nam, string makv, int co)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@BIENCO", SqlDbType.Int, 2, co)
                                  };
        DataSet ds = db.RunExecProc("DSKHXOABO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSKHXOABOPO(int thang, int nam, string makv, int co)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),                                   
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@BIENCO", SqlDbType.Int, 2, co)
                                  };
        DataSet ds = db.RunExecProc("DSKHXOABOPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangTongHopChuanThu(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv)
                               };
        DataSet ds = db.RunExecProc("[BANGTONGHOPCHUANTHU]", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BANGTONGHOPCHUANTHUBIEUDO(int thang, int nam, DateTime tuky, DateTime denky ,string makv, string idkh, string mamdsd, string ghichu, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@TUKY", SqlDbType.DateTime, 30, tuky),
                                   db.MakeInParam("@DENKY", SqlDbType.DateTime, 30, denky),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 50, idkh),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@GHICHU", SqlDbType.NVarChar, 100, ghichu),
                                   db.MakeInParam("@COBIEN", SqlDbType.VarChar, 50, cobien)
                               };
        DataSet ds = db.RunExecProc("BANGTONGHOPCHUANTHUBIEUDO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InHoaDonTn(int thang, int nam, string strMadp)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@STR_MADP", SqlDbType.VarChar  , 400, strMadp ),
                };
        DataSet ds = db.RunExecProc("InHoaDonTN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InHoaDonN(int thang, int nam, string makv, int dau, int cuoi)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),
                    db.MakeInParam("@DAU", SqlDbType.Int  , 7, dau),
                    db.MakeInParam("@CUOI", SqlDbType.Int  , 7, cuoi),
                };
        DataSet ds = db.RunExecProc("InHoaDonN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InHoaDonN1(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 4, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                    
                };
        DataSet ds = db.RunExecProc("InHoaDonN1", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ChiTietHoaDonTon(int thang, int nam, string madp,string manv, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                               };
        DataSet ds = db.RunExecProc("CHITIETHOADONTON", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhachHangTonHoaDonNhieuKy(string madp, string manv, string makv, DateTime? fromDate, DateTime? toDate)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, fromDate),
                                   db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, toDate)
                               };
        DataSet ds = db.RunExecProc("KHACHHANGTONHOADONNHIEUKY", prams);
        db.Dispose();
        return ds;
    }

    public DataSet LichSuSuDungNuoc(int nam, string madp, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                               };
        DataSet ds = db.RunExecProc("LICHSUSUDUNGNUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet TongHopCongNoTheoNhanVien(int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                              
                               };
        DataSet ds = db.RunExecProc("TONGHOPCONGNOTHEONHANVIEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InHoaDonLeTn(int thang, int nam, string idkhstr)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@IDKHSTR", SqlDbType.VarChar, 4000, idkhstr)
                               };
        DataSet ds = db.RunExecProc("InHoaDonLeTN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InHoaDonLeTN1(int thang, int nam, string idkhstr)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@IDKHSTR", SqlDbType.VarChar, 4000, idkhstr)
                               };
        DataSet ds = db.RunExecProc("InHoaDonLeTN1", prams);
        db.Dispose();
        return ds;
    }
   

    public DataSet InHoaDonLeTheoLoTrinhTN(int thang, int nam, string madp, string duongphu, int lotrinhdau, int lotrinhcuoi)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                    db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                    db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                    db.MakeInParam("@LOTRINHDAU", SqlDbType.Int, 4, lotrinhdau),
                                    db.MakeInParam("@LOTRINHCUOI", SqlDbType.Int, 4, lotrinhcuoi),
                                    db.MakeInParam("@MADP", SqlDbType.VarChar, 3, madp),
                                    db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 3, duongphu)
                                    
                               };
        DataSet ds = db.RunExecProc("InHoaDonLeTheoLoTrinhTN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InHoaDonLeTheoLoTrinhDT(int thang, int nam, string madp, string duongphu, int lotrinhdau, int lotrinhcuoi)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                    db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                    db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                    db.MakeInParam("@LOTRINHDAU", SqlDbType.Int, 4, lotrinhdau),
                                    db.MakeInParam("@LOTRINHCUOI", SqlDbType.Int, 4, lotrinhcuoi),
                                    db.MakeInParam("@MADP", SqlDbType.VarChar, 3, madp),
                                    db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 3, duongphu)
                                    
                               };
        DataSet ds = db.RunExecProc("InHoaDonLeTheoLoTrinhDT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ChiTietThuNoTheoThoiGian(DateTime tungay, DateTime denngay, string manv, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@TUNGAY", SqlDbType.DateTime, 8, tungay),
                                   db.MakeInParam("@DENNGAY", SqlDbType.DateTime, 8, denngay),
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)
                               };
        DataSet ds = db.RunExecProc("CHITIETTHUNOTHEOTHOIGIAN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangKeChiTietThucThu(int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANGCN", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAMCN", SqlDbType.Int, 4, nam)
                              
                               };
        DataSet ds = db.RunExecProc("BANGKECHITIETTHUCTHU", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangKeChiTietKhachHang(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                    db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)
                              
                               };
        DataSet ds = db.RunExecProc("BANGKECHITIETKHACHHANG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhachHangTonHoaDon(string manv, string makv, int nam, int thang)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@NAM", SqlDbType.Int , 4, nam),
                                   db.MakeInParam("@THANG", SqlDbType.Int , 4, thang)
                               };
        DataSet ds = db.RunExecProc("KHACHHANGTONHOADON", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangKeThuTienNuoc(string manv, int nam, int thang, DateTime ngayThu)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@NAM", SqlDbType.Int , 4, nam),
                                   db.MakeInParam("@THANG", SqlDbType.Int , 4, thang),
                                   db.MakeInParam("@NGAYTHU", SqlDbType.DateTime, 8, ngayThu)
                               };
        DataSet ds = db.RunExecProc("BANGKETHUTIENNUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangKeTonHoaDonTheoDuong(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                    db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)
                               };
        DataSet ds = db.RunExecProc("BANGKETONHOADONTHEODUONG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet PhieuThu(int thang, int nam, string sophieu)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANGCN", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAMCN", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@SOPHIEUCN", SqlDbType.VarChar, 4000, sophieu)
                               };
        DataSet ds = db.RunExecProc("PHIEUTHU", prams);
        db.Dispose();
        return ds;
    }

    public DataSet TinhHinhTieuThu(string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@IDKH", SqlDbType.VarChar, 50, idkh),
                                  
                               };
        DataSet ds = db.RunExecProc("TINHHINHTIEUTHU", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangKeGiaoNhanHoaDon(int thang, int nam, string manv, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                               };
        DataSet ds = db.RunExecProc("BANGKEGIAONHANHOADON", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangChuanThuTheoDuong(int thang, int nam, string makv, string mamdsd, string malkhdb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@MAMDSD", SqlDbType.VarChar, 50, mamdsd),
                                   db.MakeInParam("@MALKHDB", SqlDbType.VarChar, 50, malkhdb)
                               };
        DataSet ds = db.RunExecProc("BANGCHUANTHUTHEODUONG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet B_CHUANTHUTHEOMDSD(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)                                   
                               };
        DataSet ds = db.RunExecProc("B_CHUANTHUTHEOMDSD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet B_CHUANTHUTHEOMDSDKV(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)                                   
                               };
        DataSet ds = db.RunExecProc("B_CHUANTHUTHEOMDSDKV", prams);
        db.Dispose();
        return ds;
    }

    public DataSet B_TRANGTHAITHEODUONG(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)                                   
                               };
        DataSet ds = db.RunExecProc("B_TRANGTHAITHEODUONG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet SanLuongThucTe(int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv)
                               };
        DataSet ds = db.RunExecProc("SANLUONGTHUCTE", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DanhSachKhachHangGhiSot(int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                              
                               };
        DataSet ds = db.RunExecProc("DANHSACHKHACHHANGGHISOT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DanhSachKhachHangMoiSoVoiThangTruoc(int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                              
                               };
        DataSet ds = db.RunExecProc("DANHSACHKHACHHANGMOISOVOITHANGTRUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DanhSachKhachHangCupSoVoiThangTruoc(int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                              
                               };
        DataSet ds = db.RunExecProc("DANHSACHKHACHHANGCUPSOVOITHANGTRUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DanhSachKhachHangMoSoVoiThangTruoc(int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                              
                               };
        DataSet ds = db.RunExecProc("DANHSACHKHACHHANGMOSOVOITHANGTRUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoTinhHinhThucThu(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                               };
        DataSet ds = db.RunExecProc("BCTHTT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BangChiTietChuanThu(int thang, int nam, string madp, string duongphu,  string makv, DateTime  tungay, DateTime denngay)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@DUONGPHU", SqlDbType.VarChar, 50, duongphu),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, makv),
                                   db.MakeInParam("@TUNGAY", SqlDbType.DateTime, 8, tungay),
                                   db.MakeInParam("@DENNGAY", SqlDbType.DateTime, 8, denngay),
                                    
                               };
        DataSet ds = db.RunExecProc("BANGCHITIETCHUANTHU", prams);
        db.Dispose();
        return ds;
    }

    public DataSet TinhHinhThuTienNhanVien(int thang, int nam, string manv, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@THANG", SqlDbType.Int, 4, thang),
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 50, manv),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                               };
        DataSet ds = db.RunExecProc("TINHHINHTHUTIENNHANVIEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet SoBoCongTy(int nam, string madp, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@NAM", SqlDbType.Int, 4, nam),
                                   db.MakeInParam("@MADP", SqlDbType.VarChar, 50, madp),
                                   db.MakeInParam("@MAKV", SqlDbType.VarChar, 50, makv),
                               };
        DataSet ds = db.RunExecProc("SOBOCONGTY", prams);
        db.Dispose();
        return ds;
    }

    public DataSet RpdsDonSuaChua(DateTime tuNgay, DateTime denNgay, string manvxl,string manvg,string maxl,  string maKv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@TUNGAY", SqlDbType.DateTime, 8, tuNgay),
                                   db.MakeInParam("@DENNGAY", SqlDbType.DateTime, 8, denNgay),
                                   db.MakeInParam("@MANVXL", SqlDbType.VarChar, 50, manvxl),
                                   db.MakeInParam("@MANVG", SqlDbType.VarChar, 50, manvg),
                                   db.MakeInParam("@MAXL", SqlDbType.VarChar, 50, maxl),
                                   db.MakeInParam("@MaKV", SqlDbType.VarChar, 50, maKv)
                               };
        DataSet ds = db.RunExecProc("RPDSDONSUACHUA_HOANTHANH", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhongThongTinThayDongHo(string madp, string duongphu, string khuVuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@MADP",  SqlDbType.VarChar  , 50, madp),
                    db.MakeInParam("@DUONGPHU",  SqlDbType.VarChar  , 50, duongphu),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc )
                };
        DataSet ds = db.RunExecProc("KhongThongTinThayDongHo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThayDongHo(int thang, int nam, string khuvuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, khuvuc )
                };
        DataSet ds = db.RunExecProc("RP_THAYDONGHO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThayDongHodOotIn(int thang, int nam, string khuvuc, string idkh, string madotin, string cobien)
    {              
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, khuvuc ),
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                    db.MakeInParam("@MADOTIN", SqlDbType.VarChar  , 20, madotin ),
                    db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("RP_THAYDONGHO_DOTIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThayDongHoPo(int thang, int nam, string khuvuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),                    
                    db.MakeInParam("@MAKVPO", SqlDbType.VarChar  , 10, khuvuc )
                };
        DataSet ds = db.RunExecProc("RP_THAYDONGHOPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThayDongHoDC(int thang, int nam, string khuvuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, khuvuc )
                };
        DataSet ds = db.RunExecProc("RP_THAYDONGHODC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSDongHoCSLon(DateTime tuNgay, DateTime denNgay, string khuVuc, int congsuat)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, khuVuc ),
                    db.MakeInParam("@CONGSUAT", SqlDbType.Int  , 2, congsuat ),
                };
        DataSet ds = db.RunExecProc("DSDongHoCSLon", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoVatTuThietKe(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoVatTuThietKe", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoVTTK(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoVTTK", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BaoCaoVatTuThietKePo(string maDdk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MaDDK", SqlDbType.VarChar  , 50, maDdk )
                };
        DataSet ds = db.RunExecProc("BaoCaoVatTuThietKePo", prams);
        db.Dispose();
        return ds;
    }    

    public DataSet BAOCAOVTTK_LOI(string maDdk, string manhom)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maDdk ),
                     db.MakeInParam("@MANHOM", SqlDbType.VarChar  , 10, manhom )
                };
        DataSet ds = db.RunExecProc("BAOCAOVTTK_LOI", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhachHangHis(string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh )
                };
        DataSet ds = db.RunExecProc("KHACHHANGH", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhachHangTT(string idkh, string ttsd)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@TTSD", SqlDbType.VarChar  , 10, ttsd )
                };
        DataSet ds = db.RunExecProc("KHACHHANGTT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet KhachHangTTPo(string idkh, string ttsd)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@TTSD", SqlDbType.VarChar  , 10, ttsd )
                };
        DataSet ds = db.RunExecProc("KHACHHANGTTPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpdateTieuThu(string idkh, string madp, string madb)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp ),
                     db.MakeInParam("@MADB", SqlDbType.VarChar  , 11, madb )
                };
        DataSet ds = db.RunExecProc("UPDATETT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpdateTieuThuDH(string idkh, int csd, int csc, int tt)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@CHISODAU", SqlDbType.Decimal  , 19, csd ), 
                     db.MakeInParam("@CHISOCUOI", SqlDbType.Decimal  , 19, csc ), 
                     db.MakeInParam("@MTRUYTHU", SqlDbType.Decimal  , 19, tt )
                };
        DataSet ds = db.RunExecProc("UPDATETTDH", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpdateTieuThuDHMoi(string idkh, int csd, int csc, int tt, int thang, int nam, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@CHISODAU", SqlDbType.Decimal  , 19, csd ), 
                     db.MakeInParam("@CHISOCUOI", SqlDbType.Decimal  , 19, csc ), 
                     db.MakeInParam("@MTRUYTHU", SqlDbType.Decimal  , 19, tt ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 19, thang ), 
                     db.MakeInParam("@NAM", SqlDbType.Int  , 19, nam ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("UpdateTieuThuDHMoi", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpdateTieuThuDHMoi2(string idkh, string idkh2, string idkh3, int csd, int csc, int tt, int thang, int nam, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@IDKH2", SqlDbType.NVarChar  , 50, idkh2 ),
                     db.MakeInParam("@IDKH3", SqlDbType.NVarChar  , 50, idkh3),
                     db.MakeInParam("@CHISODAU", SqlDbType.Decimal  , 19, csd ), 
                     db.MakeInParam("@CHISOCUOI", SqlDbType.Decimal  , 19, csc ), 
                     db.MakeInParam("@MTRUYTHU", SqlDbType.Decimal  , 19, tt ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 19, thang ), 
                     db.MakeInParam("@NAM", SqlDbType.Int  , 19, nam ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("UpdateTieuThuDHMoi2", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpdateTieuThuDHPo(string idkh, int csd, int csc, int tt)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@CHISODAU", SqlDbType.Decimal  , 19, csd ), 
                     db.MakeInParam("@CHISOCUOI", SqlDbType.Decimal  , 19, csc ), 
                     db.MakeInParam("@MTRUYTHU", SqlDbType.Decimal  , 19, tt )
                };
        DataSet ds = db.RunExecProc("UPDATETTDHPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThemTieuThuDC(string idkh, int thang, int nam, int csddc, int cscdc,string ghichudc,string masohddc, string inhddc, int truythudc, int sodm)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc ),
                     db.MakeInParam("@INHDDC", SqlDbType.VarChar  , 2, inhddc ),
                     db.MakeInParam("@MTRUYTHUDC", SqlDbType.Decimal  , 19, truythudc ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Decimal  , 19, sodm )
                };
        DataSet ds = db.RunExecProc("ThemTieuThuDC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThemTieuThuTTVP(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc, string inhddc, int truythudc, int sodm, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc ),
                     db.MakeInParam("@INHDDC", SqlDbType.VarChar  , 2, inhddc ),
                     db.MakeInParam("@MTRUYTHUDC", SqlDbType.Decimal  , 19, truythudc ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Decimal  , 7, sodm ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("ThemTieuThuTTVP", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThemTieuThuDCPo(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc, string inhddc, int truythudc, int sodm)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc ),
                     db.MakeInParam("@INHDDC", SqlDbType.VarChar  , 2, inhddc ),
                     db.MakeInParam("@MTRUYTHUDC", SqlDbType.Decimal  , 19, truythudc ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Decimal  , 19, sodm )
                };
        DataSet ds = db.RunExecProc("ThemTieuThuDCPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ThemTieuThuDCTG(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc, string mamdsdtg)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc ),
                     db.MakeInParam("@MAMDSDTG", SqlDbType.VarChar  , 10, mamdsdtg )
                };
        DataSet ds = db.RunExecProc("ThemTieuThuDCTG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpTieuThuDC(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc, string inhddc, int truythudc, int sodm)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc ),
                     db.MakeInParam("@INHDDC", SqlDbType.VarChar  , 2, inhddc ),
                     db.MakeInParam("@MTRUYTHUDC", SqlDbType.Decimal  , 18, truythudc ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Decimal  , 19, sodm )   
                };
        DataSet ds = db.RunExecProc("UpTieuThuDC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpTieuThuDCPo(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc, string inhddc, int truythudc, int sodm)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc ),
                     db.MakeInParam("@INHDDC", SqlDbType.VarChar  , 2, inhddc ),
                     db.MakeInParam("@MTRUYTHUDC", SqlDbType.Decimal  , 18, truythudc ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Decimal  , 19, sodm ),
                };
        DataSet ds = db.RunExecProc("UpTieuThuDCPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpTieuThuDCTG(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@CHISODAUDC", SqlDbType.Decimal  , 19, csddc ),                     
                     db.MakeInParam("@CHISOCUOIDC", SqlDbType.Decimal  , 19, cscdc ),
                     db.MakeInParam("@GHICHUDC", SqlDbType.NVarChar  , 300, ghichudc ),
                     db.MakeInParam("@MASOHDDC", SqlDbType.NVarChar  , 30, masohddc )
                };
        DataSet ds = db.RunExecProc("UpTieuThuDCTG", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BKDieuChinh(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),                     
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                };
        DataSet ds = db.RunExecProc("BKDCHD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BKDieuChinhDotIn(int thang, int nam, string makv, string idkh, string idkh2, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),                     
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2 ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("BKDCHDDOTIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INSOGHINUOC(int thang, int nam, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang ),                     
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                };
        DataSet ds = db.RunExecProc("INSOGHINUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DONGCUA_TT(int thang, int nam, string khuvuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                    db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, khuvuc )
                };
        DataSet ds = db.RunExecProc("DONGCUA_TT", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHTEN(string idkh, string tenkh, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@TENKH", SqlDbType.NVarChar  , 200, tenkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam)
                };
        DataSet ds = db.RunExecProc("UPKHTEN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHTENUP(string idkh, string tenkh, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@TENKH", SqlDbType.NVarChar  , 200, tenkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam)
                };
        DataSet ds = db.RunExecProc("UPKHTENUP", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHTENTHD(string idkh, string tenkh, int thang, int nam, int inup, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@TENKH", SqlDbType.NVarChar  , 200, tenkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@INUP", SqlDbType.Int  , 4, inup),
                     db.MakeInParam("@lydo", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPKHTENTHD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHTENTHDPO(string idkh, string tenkh, int thang, int nam, int inup, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@TENKH", SqlDbType.NVarChar  , 200, tenkh ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@INUP", SqlDbType.Int  , 4, inup),
                     db.MakeInParam("@lydo", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPKHTENTHDPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHSONHA(string idkh, string sonha2, string sonha, int thang, int nam, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SONHA2", SqlDbType.NVarChar  , 200, sonha2 ),
                     db.MakeInParam("@SONHA", SqlDbType.NVarChar  , 200, sonha ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPKHSONHA", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHDANHBO(string idkh, string madp, string madb, string duongphu, int thang, int nam, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp ),
                     db.MakeInParam("@MADB", SqlDbType.VarChar  , 10, madb ),
                     db.MakeInParam("@DUONGPHU", SqlDbType.VarChar  , 3, duongphu),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPKHDANHBO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHMST(string idkh, string mst, int thang, int nam, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MST", SqlDbType.VarChar  , 20, mst ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPKHMST", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHMAMDSD(string idkh, string mamdsd, int thang, int nam, string lydo)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 100, lydo )
                };
        DataSet ds = db.RunExecProc("UPKHMAMDSD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet IN_DLND117(string maddk, int gia)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@maddk", SqlDbType.VarChar  , 11, maddk ),                     
                     db.MakeInParam("@gianc", SqlDbType.Int  , 6, gia )
                     
                };
        DataSet ds = db.RunExecProc("InsertDLND117", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BANGKEVATTUNUOC(DateTime tungay, DateTime denngay, String makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tungay ),                     
                     db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denngay ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                     
                };
        DataSet ds = db.RunExecProc("BANGKEVATTUNUOC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet BANGKEVATTUNUOCCT(DateTime tungay, DateTime denngay, String makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tungay ),                     
                     db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denngay ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                     
                };
        DataSet ds = db.RunExecProc("BANGKEVATTUNUOCCT", prams);
        db.Dispose();
        return ds;
    }
    public DataSet BANGKEVATTUNUOCKH(DateTime tungay, DateTime denngay, String makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tungay ),                     
                     db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denngay ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                     
                };
        DataSet ds = db.RunExecProc("BANGKEVATTUNUOCKH", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INHOPDONG(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("InHopDong", prams);
        db.Dispose();
        return ds;
    }
    
    public DataSet InThayHopDong(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("InThayHopDong", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InThayHopDongPo(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("InThayHopDongPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDSDongHo(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("InThayHopDong", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INGIAYDENGHIN(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_GIAYDENGHI", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INGIAYDENGHID(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_GIAYDENGHIPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INPHIEULAPDHN(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_PHIEULAPDHN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INPHIEULAPDHD(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_PHIEULAPDHD", prams);
        db.Dispose();
        return ds;
    }
    
    public DataSet INPHIEULAPNIEMCHIDHN(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_PHIEULAPNIEMCHIDHN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INPHIEULAPNIEMCHIDHD(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_PHIEULAPNIEMCHIDHD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INPHIEUNIEMCHIN(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_PHIEUNIEMCHIN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INBBNGHIEMTHUN(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("IN_BBNGHIEMTHUN", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPCHIKDNUOC(String maddk, String chikd1, String chikd2)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ),
                     db.MakeInParam("@CHIKDM1", SqlDbType.NVarChar  , 50, chikd1 ),
                     db.MakeInParam("@CHIKDM2", SqlDbType.NVarChar  , 50, chikd2 )
                };
        DataSet ds = db.RunExecProc("UPMACHIKD", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPDDKTHICONGA(String maddk)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk )
                };
        DataSet ds = db.RunExecProc("UPDDKTHICONGA", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPMAUTK(string maddk, string mau, string tenkhp, string tenkht, string dskhp, string dskht)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ),
                     db.MakeInParam("@MAMAUTK", SqlDbType.VarChar  , 6, mau ),
                     db.MakeInParam("@TENKHPHAI", SqlDbType.NVarChar  , 100, tenkhp ),
                     db.MakeInParam("@TENKHTRAI", SqlDbType.NVarChar  , 100, tenkht ),
                     db.MakeInParam("@DANHSOPHAI", SqlDbType.VarChar  , 9, dskhp ),
                     db.MakeInParam("@DANHSOTRAI", SqlDbType.VarChar  , 9, dskht )
                };
        DataSet ds = db.RunExecProc("UPTHIETKE", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPMAUTKPO(string maddk, string mau, string tenkhp, string tenkht, string dskhp, string dskht,
                        string trutruoc, string trusau, string dstrutruoc, string dstrusau)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ),
                     db.MakeInParam("@MAMAUTK", SqlDbType.VarChar  , 6, mau ),
                     db.MakeInParam("@TENKHPHAI", SqlDbType.NVarChar  , 100, tenkhp ),
                     db.MakeInParam("@TENKHTRAI", SqlDbType.NVarChar  , 100, tenkht ),
                     db.MakeInParam("@DANHSOPHAI", SqlDbType.VarChar  , 9, dskhp ),
                     db.MakeInParam("@DANHSOTRAI", SqlDbType.VarChar  , 9, dskht ),

                     db.MakeInParam("@TENTRUTRUOC", SqlDbType.NVarChar  , 100, trutruoc ),
                     db.MakeInParam("@TENTRUSAU", SqlDbType.NVarChar  , 100, trusau ),
                     db.MakeInParam("@DSTRUTRUOC", SqlDbType.VarChar  , 9, dstrutruoc ),
                     db.MakeInParam("@DSTRUSAU", SqlDbType.VarChar  , 9, dstrusau )
                };
        DataSet ds = db.RunExecProc("UPTHIETKEPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpSentNV1(int magui, string masnv, string maup, string manv, string manvs)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                                   db.MakeInParam("@MAGUI", SqlDbType.Int, 4, magui),                                  
                                   db.MakeInParam("@MASENTNV", SqlDbType.VarChar, 20, masnv),//phong ban, khu vuc
                                   db.MakeInParam("@MAUPLOAD", SqlDbType.VarChar, 20, maup),
                                   db.MakeInParam("@MANV", SqlDbType.VarChar, 10, manv),
                                   db.MakeInParam("@MANVSENT", SqlDbType.VarChar, 10, manvs)//phong ban, khu vuc
                                   
                                    
                               };
        DataSet ds = db.RunExecProc("UpSentNV1", prams);
        db.Dispose();
        return ds;
    }

    public DataSet SumKhoiLuongBB(DateTime tuNgay, DateTime denNgay)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay)
                };
        DataSet ds = db.RunExecProc("SumKhoiLuongBB", prams);
        db.Dispose();
        return ds;
    }

    public DataSet SumKhoiLuongBBPo(DateTime tuNgay, DateTime denNgay)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay)
                };
        DataSet ds = db.RunExecProc("SumKhoiLuongBBPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet LatLongAll()
    {
        Database db = new Database();
        SqlParameter[] prams = {
                };
        DataSet ds = db.RunExecProc("LatLongAll", prams);
        db.Dispose();
        return ds;
    }

    public DataSet LSTHAYDOICHITIET(string idkh, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ), 
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv )
                };
        DataSet ds = db.RunExecProc("LUOCSUTHAYDOICHITIET", prams);
        db.Dispose();
        return ds;
    }

    public DataSet LSTHAYDONGHO(string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh )
                    
                };
        DataSet ds = db.RunExecProc("LUOCSUTHAYDONGHO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet LSNHANVIENVAYTK(string manvv)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@MANVV", SqlDbType.VarChar  , 10, manvv )
                    
                };
        DataSet ds = db.RunExecProc("VAYTRACUULANVAY", prams);
        db.Dispose();
        return ds;
    }

    public DataSet LSDIEUCHINHCS(string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh )
                    
                };
        DataSet ds = db.RunExecProc("LUOCSUDIEUCHINHCS", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INSSUADONGHO(string maddk, string madhcu, string madhmoi, string sonocu, string sonomoi, string maldhcu,
                                string maldhmoi, string manv, string ghichu)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ), 
                     db.MakeInParam("@MADHCU", SqlDbType.VarChar  , 20, madhcu ),
                     db.MakeInParam("@MADHMOI", SqlDbType.VarChar  , 20, madhmoi ), 
                     db.MakeInParam("@SONOCU", SqlDbType.VarChar  , 50, sonocu ),
                     db.MakeInParam("@SONOMOI", SqlDbType.VarChar  , 50, sonomoi ), 
                     db.MakeInParam("@MALDHCU", SqlDbType.VarChar  , 10, maldhcu ),
                     db.MakeInParam("@MALDHMOI", SqlDbType.VarChar  , 10, maldhmoi ), 
                     db.MakeInParam("@MANV", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@GHICHU", SqlDbType.NVarChar , 200, ghichu )
                };
        DataSet ds = db.RunExecProc("INSSUADONGHO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet INSSUADONGHOPO(string maddk, string madhcu, string madhmoi, string sonocu, string sonomoi, string maldhcu,
                                string maldhmoi, string manv, string ghichu)
    {
        Database db = new Database();
        SqlParameter[] prams = {                     
                     db.MakeInParam("@MADDK", SqlDbType.VarChar  , 11, maddk ), 
                     db.MakeInParam("@MADHCU", SqlDbType.VarChar  , 20, madhcu ),
                     db.MakeInParam("@MADHMOI", SqlDbType.VarChar  , 20, madhmoi ), 
                     db.MakeInParam("@SONOCU", SqlDbType.VarChar  , 50, sonocu ),
                     db.MakeInParam("@SONOMOI", SqlDbType.VarChar  , 50, sonomoi ), 
                     db.MakeInParam("@MALDHCU", SqlDbType.VarChar  , 10, maldhcu ),
                     db.MakeInParam("@MALDHMOI", SqlDbType.VarChar  , 10, maldhmoi ), 
                     db.MakeInParam("@MANV", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@GHICHU", SqlDbType.NVarChar , 200, ghichu )
                };
        DataSet ds = db.RunExecProc("INSSUADONGHOPO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet ListDongHo(string tukhoa, DateTime tuNgay, DateTime denNgay, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUKHOA", SqlDbType.VarChar  , 2, tukhoa),
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv)
                };
        DataSet ds = db.RunExecProc("GETDONGHO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InsKhachHangXoa(string idkh, string makv, string lydo, string manvn, string maxa, string maapto, DateTime ngayxoa)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 300, lydo ),
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn ),
                     db.MakeInParam("@MAXA", SqlDbType.VarChar  , 10, maxa ),
                     db.MakeInParam("@MAAPTO", SqlDbType.VarChar  , 10, maapto ),
                     db.MakeInParam("@NGAYXOA", SqlDbType.DateTime  , 30, ngayxoa )
        };
        DataSet ds = db.RunExecProc("InsKhachHangXoa", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InsKhachHangXoaPo(string idkh, string makv, string lydo, string manvn, string maxa, string maapto, DateTime ngayxoa)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 300, lydo ),
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manvn ),
                     db.MakeInParam("@MAXA", SqlDbType.VarChar  , 10, maxa ),
                     db.MakeInParam("@MAAPTO", SqlDbType.VarChar  , 10, maapto ),
                     db.MakeInParam("@NGAYXOA", SqlDbType.DateTime  , 30, ngayxoa )
        };
        DataSet ds = db.RunExecProc("InsKhachHangXoaPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpKhachHangXoa(string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh )                     
                };
        DataSet ds = db.RunExecProc("UpKhachHangXoa", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpKhachHangXoaPo(string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh )                     
                };
        DataSet ds = db.RunExecProc("UpKhachHangXoaPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpKhachHangHoNgheo(string idkh, string mangheo, int co)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MANGHEO", SqlDbType.VarChar  , 10, mangheo ),
                     db.MakeInParam("@CO", SqlDbType.Int  , 2, co )
                };
        DataSet ds = db.RunExecProc("UpKhachHangHoNgheo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InKHHoNgheo(string mangheo, string idkh, string makv, string maxa, string donvicap, string mahn,
                DateTime ngaycap, DateTime ngaykt, DateTime ngayky, DateTime kyhotro, string manv, string diachi)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@MANGHEO", SqlDbType.VarChar  , 10, mangheo ),
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),
                     db.MakeInParam("@MAXA", SqlDbType.VarChar  , 10, maxa ),
                     db.MakeInParam("@DONVICAPHN", SqlDbType.NVarChar, 100, donvicap ),
                     db.MakeInParam("@MAHN", SqlDbType.NVarChar  , 50, mahn ),
                     db.MakeInParam("@NGAYCAPHN", SqlDbType.DateTime  , 8, ngaycap ),
                     db.MakeInParam("@NGAYKETTHUCHN", SqlDbType.DateTime  , 8, ngaykt ),
                     db.MakeInParam("@NGAYKYHN", SqlDbType.DateTime  , 8, ngayky ),
                     db.MakeInParam("@KYHOTROHN", SqlDbType.DateTime  , 8, kyhotro ),
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@DIACHI", SqlDbType.NVarChar  , 300, diachi )
                };
        DataSet ds = db.RunExecProc("InKHHoNgheo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet DSHONGHEON(int tuthang, int tunam, int denthang, int dennam, string makv, string mangheo, string idkh)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUTHANG", SqlDbType.Int  , 2, tuthang),
                    db.MakeInParam("@TUNAM", SqlDbType.Int  , 4, tunam),
                    db.MakeInParam("@DENTHANG", SqlDbType.Int  , 2, denthang),
                    db.MakeInParam("@DENNAM", SqlDbType.Int  , 4, dennam),
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 10, makv ),
                    db.MakeInParam("@MANGHEO", SqlDbType.VarChar  , 10, mangheo ),
                    db.MakeInParam("@IDKH", SqlDbType.VarChar  , 10, idkh )
                };
        DataSet ds = db.RunExecProc("DSKHNGHEO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMuc(string idkh, decimal soho, int sonk, bool isdm, int sodm, string manv, string mamdsd, string kydm)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm )                     
                };
        DataSet ds = db.RunExecProc("InDinhMuc", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMucLyDo(string idkh, decimal soho, int sonk, bool isdm, int sodm, string manv, string mamdsd, 
        string kydm, string lydo, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm ),
                     db.MakeInParam("@LYDO", SqlDbType.NVarChar  , 500, lydo ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )              
                };
        DataSet ds = db.RunExecProc("InDinhMucLyDo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMucKHTAMLX(string idkh, decimal soho, int sonk, bool isdm, int sodm, string manv, string mamdsd, string kydm)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm )                     
                };
        DataSet ds = db.RunExecProc("InDinhMucKHTAMLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMucTamKHTAMLX(string idkh, string idkh2, decimal soho, int sonk, bool isdm, int sodm, string manv, 
        string mamdsd, string kydm, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@IDKH2", SqlDbType.VarChar  , 11, idkh2 ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("InDinhMucTamKHTAMLX", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMucPo(string idkh, decimal soho, int sonk, bool isdm, int sodm, string manv, string mamdsd, string kydm)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm )                     
                };
        DataSet ds = db.RunExecProc("InDinhMucPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMucTam(string idkh, decimal soho, int sonk, bool isdm, int sodm, string manv, string mamdsd, string kydm)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm )                     
                };
        DataSet ds = db.RunExecProc("InDinhMucTam", prams);
        db.Dispose();
        return ds;
    }

    public DataSet InDinhMucTamPo(string idkh, decimal soho, int sonk, bool isdm, int sodm, string manv, string mamdsd, string kydm, string cobien)
    {
        Database db = new Database();
        SqlParameter[] prams = {                    
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SOHO", SqlDbType.Decimal  , 10, soho ),
                     db.MakeInParam("@SONK", SqlDbType.Int  , 2, sonk ),
                     db.MakeInParam("@ISDINHMUC", SqlDbType.Bit, 1, isdm ),
                     db.MakeInParam("@SODINHMUC", SqlDbType.Int  , 2, sodm ),                     
                     db.MakeInParam("@MANVN", SqlDbType.VarChar  , 10, manv ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@KYDINHMUC", SqlDbType.VarChar  , 10, kydm ),
                     db.MakeInParam("@COBIEN", SqlDbType.VarChar  , 20, cobien )
                };
        DataSet ds = db.RunExecProc("InDinhMucTamPo", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpHoNgheoHetHan(DateTime ngayhethan, string makv)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@NGAYKTHN", SqlDbType.DateTime  , 8, ngayhethan),                    
                    db.MakeInParam("@MAKV", SqlDbType.VarChar  , 50, makv )                    
                };
        DataSet ds = db.RunExecProc("UpHoNgheoHetHan", prams);
        db.Dispose();
        return ds;
    }
    

}
   
