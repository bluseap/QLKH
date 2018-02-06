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
        DataSet ds = db.RunExecProc("RPDSCDHopDong", prams);
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
        DataSet ds = db.RunExecProc("TinhHinhTieuThu", prams);
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

    public DataSet ThongTinNgayThayDongHo(DateTime tuNgay, DateTime denNgay, string madp, string duongphu, string khuVuc)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                    db.MakeInParam("@TUNGAY", SqlDbType.DateTime  , 8, tuNgay),
                    db.MakeInParam("@DENNGAY", SqlDbType.DateTime  , 8, denNgay),
                    db.MakeInParam("@MADP",  SqlDbType.VarChar  , 50, madp),
                    db.MakeInParam("@DUONGPHU",  SqlDbType.VarChar  , 50, duongphu),
                    db.MakeInParam("@MaKV", SqlDbType.VarChar  , 50, khuVuc )
                };
        DataSet ds = db.RunExecProc("ThongTinNgayThayDongHo", prams);
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

    public DataSet ThemTieuThuDC(string idkh, int thang, int nam, int csddc, int cscdc,string ghichudc,string masohddc)
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
        DataSet ds = db.RunExecProc("ThemTieuThuDC", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UpTieuThuDC(string idkh, int thang, int nam, int csddc, int cscdc, string ghichudc, string masohddc)
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
        DataSet ds = db.RunExecProc("UpTieuThuDC", prams);
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

    public DataSet UPKHSONHA(string idkh, string sonha, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@SONHA", SqlDbType.NVarChar  , 150, sonha ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam)
                };
        DataSet ds = db.RunExecProc("UPKHSONHA", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHDANHBO(string idkh, string madp, string madb, string duongphu, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MADP", SqlDbType.VarChar  , 10, madp ),
                     db.MakeInParam("@MADB", SqlDbType.VarChar  , 10, madb ),
                     db.MakeInParam("@DUONGPHU", SqlDbType.VarChar  , 3, duongphu),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam)
                };
        DataSet ds = db.RunExecProc("UPKHDANHBO", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHMST(string idkh, string mst, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MST", SqlDbType.VarChar  , 20, mst ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam)
                };
        DataSet ds = db.RunExecProc("UPKHMST", prams);
        db.Dispose();
        return ds;
    }

    public DataSet UPKHMAMDSD(string idkh, string mamdsd, int thang, int nam)
    {
        Database db = new Database();
        SqlParameter[] prams = {
                     db.MakeInParam("@IDKH", SqlDbType.VarChar  , 11, idkh ),
                     db.MakeInParam("@MAMDSD", SqlDbType.VarChar  , 10, mamdsd ),
                     db.MakeInParam("@THANG", SqlDbType.Int  , 2, thang),
                     db.MakeInParam("@NAM", SqlDbType.Int  , 4, nam)
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


}
   
