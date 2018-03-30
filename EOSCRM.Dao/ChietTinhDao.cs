using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChietTinhDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public ChietTinhDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CHIETTINH Get(string ma)
        {
            return _db.CHIETTINHs.Where(p => p.MADDK.Equals(ma)).SingleOrDefault();
        }

        public DONDANGKY GetDonDangKyChietTinh(string ma)
        {
            return _db.DONDANGKies.Where(p => p.MADDK.Equals(ma)).SingleOrDefault();
        }

        public List<CHIETTINH> GetList()
        {
            return _db.CHIETTINHs.ToList();
        }

        public List<CHIETTINH> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.CHIETTINHs.Count();
        }
               
      
        public Message Update(CHIETTINH objUi,String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK    );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CONGVIEC = objUi.CONGVIEC;
                    objDb.CPCHUNG = objUi.CPCHUNG;
                    objDb.CPKHAC = objUi.CPKHAC;
                    objDb.CPNHANCONG = objUi.CPNHANCONG;
                    objDb.CPTHIETKE = objUi.CPTHIETKE;
                    objDb.CPTHUNHAP = objUi.CPTHUNHAP;
                    objDb.CPVATLIEU = objUi.CPVATLIEU;
                    objDb.DIACHIHM = objUi.DIACHIHM;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.HSCHUNG = objUi.HSCHUNG;
                    objDb.HSNHANCONG = objUi.HSNHANCONG;
                    objDb.HSTHIETKE1 = objUi.HSTHIETKE1;
                    objDb.HSTHIETKE2 = objUi.HSTHIETKE2;
                    objDb.HSTHIETKE3 = objUi.HSTHIETKE3;
                    objDb.HSTHUE = objUi.HSTHUE;
                    objDb.HSTHUNHAP = objUi.HSTHUNHAP;
                    objDb.ISSTK = objUi.ISSTK;
                    objDb.LOAICT = objUi.LOAICT;

                    objDb.NGAYLCT = objUi.NGAYLCT;
                    if (!string.IsNullOrEmpty(objUi.MANVLCT))
                        objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVLCT));

                    objDb.NGAYDCT = objUi.NGAYDCT;
                    if (!string.IsNullOrEmpty(objUi.MANVDCT))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVDCT));

                    //objDb.MANVLCT = objUi.MANVLCT;
                    objDb.NGAYGUI_CN = objUi.NGAYGUI_CN;                    
                    objDb.NGAYNHAN_CN = objUi.NGAYNHAN_CN;
                    objDb.QUYETTOAN = objUi.QUYETTOAN;
                    objDb.SOCT = objUi.SOCT;
                    objDb.TENCT = objUi.TENCT;
                    objDb.TENHM = objUi.TENHM;
                    objDb.TIENTHUE = objUi.TIENTHUE;
                    objDb.TONG_ST = objUi.TONG_ST;
                    objDb.TONG_TT = objUi.TONG_TT;
                    objDb.FILECT = objUi.FILECT;
                    objDb.TONGCONG = objUi.TONGCONG;
                    objDb.NHANCONG = objUi.NHANCONG;
                    objDb.CPMAY = objUi.CPMAY;
                    objDb.CPTTVT = objUi.CPTTVT;
                    objDb.CPTTMAY = objUi.CPTTMAY;
                    objDb.TCPTT = objUi.TCPTT;
                    objDb.HSCPC = objUi.HSCPC;
                    objDb.CPTTHUE = objUi.CPTTHUE;
                    objDb.CPC = objUi.CPC;
                    objDb.HSPVL = objUi.HSPVL;
                    objDb.HSCPM = objUi.HSCPM;
                    objDb.HSTTP = objUi.HSTTP;
                    objDb.CPKSHS = objUi.CPKSHS;
                    objDb.TTP = objUi.TTP;
                    objDb.CHUNGONG = objUi.CHUNGONG;
                    objDb.KPDT = objUi.KPDT;
                    objDb.GIAMGIACPVL = objUi.GIAMGIACPVL;
                    objDb.GIAMGIACPNC = objUi.GIAMGIACPNC;
                    objDb.SDGIA = objUi.SDGIA;
                    objDb.TONGTIENCTPS = objUi.TONGTIENCTPS;

                    objDb.MADH = objUi.MADH;
                    objDb.NGAYNDH = objUi.NGAYNDH;

                    // Submit changes to db
                    _db.SubmitChanges();

                    //UpdateChiPhiForChietTinh(objUi.MADDK);

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_P.ToString(),
                        MOTA = "Cập nhật thông tin lập chiết tính."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Chiết tính ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "chiết tính", objUi.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chiết tính ", objUi.TENCT);
            }
            return msg;
        }

        public decimal? GetHeSo(string mahs)
        {
            // get dmnk
            var hs = _db.HESOs.SingleOrDefault(h => h.MAHS.Equals(mahs));
            decimal? gths = (hs != null && hs.GIATRI.HasValue) ? hs.GIATRI.Value : (decimal?)null;

            return gths;
        }

        public Message CreateChietTinh(THIETKE obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINH
                                    {
                                        MADDK = obj.MADDK,
                                        TENCT = obj.TENTK,
                                        TENHM = Constants.CongTacDefault,
                                        DIACHIHM = obj.DONDANGKY.DIACHILD,
                                        GHICHU = obj.CHUTHICH,
                                        MANVLCT = sManv,
                                        NGAYLCT = DateTime.Now,
                                        QUYETTOAN = 0,
                                        CONGVIEC = 0,
                                        CPVATLIEU = 0,
                                        CPNHANCONG = 0,
                                        CPCHUNG =0,
                                        CPTHUNHAP = 0,
                                        CPTHIETKE =0,
                                        CPKHAC = 0,
                                        HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                                        HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                                        HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                                        HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                                        HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                                        HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                                        HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                                        HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                                        TIENTHUE =0,
                                        TONG_TT =0,
                                        TONG_ST =0,
                                        
                                        //NGAYLCT
                                        //NGAYGUI_CN
                                        //NGAYNHAN_CN
                                        //ISSTK
                                         FILECT = "", 
                                         TONGCONG = 0, 
                                         NHANCONG= 0, 
                                         CPMAY = 0, 
                                         CPTTVT = 0, 
                                         CPTTMAY = 0, 
                                         TCPTT = 0,
                                        
                                         CPTTHUE = 0, 
                                         CPC = 0, 
                                         HSPVL =0, 
                                         HSCPM =0, 
                                         HSTTP =0, 
                                         CPKSHS =0, 
                                         TTP = 0, 
                                         CHUNGONG = 0, 
                                         KPDT ="", 
                                         GIAMGIACPVL =0, 
                                         GIAMGIACPNC =0, 
                                         SDGIA = 1
                                    };

                _db.CHIETTINHs.InsertOnSubmit(chiettinh);

                // update dondangky

                var objDDK = _db.DONDANGKies.Where(p => p.MADDK.Equals(obj.MADDK)).FirstOrDefault();
                objDDK.TTCT = TTCT.CT_P.ToString();
                _db.SubmitChanges();

                // copy from ctthietke, gcthietke, daolaptk
                var cttk = _db.CTTHIETKEs.Where(ct => ct.MADDK.Equals(obj.MADDK)).ToList();
                foreach (var ct in cttk)
                {
                    if(ct.ISCTYDTU.HasValue && ct.ISCTYDTU.Value)
                    {
                        var ctct = new CTCHIETTINH
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC,
                            ISCTYDTU = ct.ISCTYDTU
                        };

                        _db.CTCHIETTINHs.InsertOnSubmit(ctct);
                    }
                    else
                    {
                        var ctct = new CTCHIETTINH_ND117
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC
                        };
                        _db.CTCHIETTINH_ND117s.InsertOnSubmit(ctct);
                    }
                }

                var gctk = _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(obj.MADDK)).ToList();
                foreach (var gc in gctk)
                {
                    var gcct = new GHICHU
                    {
                        MADDK = obj.MADDK,
                        NOIDUNG = gc.NOIDUNG
                    };
                    _db.GHICHUs.InsertOnSubmit(gcct);
                }

                var cptk = _db.DAOLAPTKs.Where(p => p.MADDK.Equals(obj.MADDK)).ToList();
                foreach (var cp in cptk)
                {
                    var cpct = new DAOLAP_ND117
                    {
                        MADDK = obj.MADDK,
                        LOAICV = cp.LOAICV,
                        LOAICT = cp.LOAICT,
                        NOIDUNG = cp.NOIDUNG,
                        DONGIACP = cp.DONGIACP,
                        DVT = cp.DVT,
                        HESOCP = cp.HESOCP,
                        SOLUONG = cp.SOLUONG,
                        THANHTIENCP = cp.THANHTIENCP,
                        LOAICP = cp.LOAICP,
                        LOAI = cp.LOAI,
                        NGAYLAP = cp.NGAYLAP
                    };
                    _db.DAOLAP_ND117s.InsertOnSubmit(cpct);
                }

                _db.SubmitChanges();
                
                // commit
                trans.Commit();

                //UpdateChiPhiForChietTinh(obj.MADDK);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Chạy chiết tính"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chiết tính");
            }

            return msg;
        }

        public Message CreateChietTinh2Po(DONDANGKYPO obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINH
                {
                    MADDK = obj.MADDKPO,
                    TENCT = obj.TENKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = obj.DIACHILD,
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,

                    //NGAYLCT
                    //NGAYGUI_CN
                    //NGAYNHAN_CN
                    //ISSTK
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,

                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1
                };

                _db.CHIETTINHs.InsertOnSubmit(chiettinh);

                // update dondangky

                var objDDK = _db.DONDANGKYPOs.Where(p => p.MADDKPO.Equals(obj.MADDKPO)).FirstOrDefault();
                objDDK.TTCT = TTCT.CT_P.ToString();
                _db.SubmitChanges();

                // commit
                trans.Commit();

                //UpdateChiPhiForChietTinh(obj.MADDK);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Chạy chiết tính điện"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Chạy chiết tính điện");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chạy chiết tính điện");
            }

            return msg;
        }

        public Message CreateChietTinh2(DONDANGKY obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINH
                {
                    MADDK = obj.MADDK,
                    TENCT = obj.TENKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = obj.DIACHILD,
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,

                    //NGAYLCT
                    //NGAYGUI_CN
                    //NGAYNHAN_CN
                    //ISSTK
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,

                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1
                };

                _db.CHIETTINHs.InsertOnSubmit(chiettinh);

                // update dondangky

                var objDDK = _db.DONDANGKies.Where(p => p.MADDK.Equals(obj.MADDK)).FirstOrDefault();
                objDDK.TTCT = TTCT.CT_P.ToString();
                _db.SubmitChanges();

                // commit
                trans.Commit();

                //UpdateChiPhiForChietTinh(obj.MADDK);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Chạy chiết tính"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Chạy chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chạy chiết tính");
            }

            return msg;
        }

        public Message CreateChietTinh2(THIETKE obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINH
                {
                    MADDK = obj.MADDK,
                    TENCT = obj.DONDANGKY.TENKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = obj.DONDANGKY.DIACHILD,
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,

                    //NGAYLCT
                    //NGAYGUI_CN
                    //NGAYNHAN_CN
                    //ISSTK
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,

                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1
                };               
                _db.CHIETTINHs.InsertOnSubmit(chiettinh);

                // update dondangky
                var objDDK = _db.DONDANGKies.FirstOrDefault(p => p.MADDK.Equals(obj.MADDK));
                if (objDDK != null) 
                    objDDK.TTCT = TTCT.CT_P.ToString();

                _db.SubmitChanges();

                // copy from ctthietke, gcthietke, daolaptk
                var cttk = _db.CTTHIETKEs.Where(ct => ct.MADDK.Equals(obj.MADDK)).ToList();
                foreach (var ct in cttk)
                {
                    if (ct.ISCTYDTU.HasValue && ct.ISCTYDTU.Value)
                    {
                        var ctct = new CTCHIETTINH
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            /*GIAVT = ct.VATTU.GIAVT,                            
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC,                            
                            ISCTYDTU = ct.ISCTYDTU*/
                            GIAVT = ct.GIAVT,
                            TIENVT = ct.SOLUONG * ct.GIAVT,
                            GIANC = ct.GIANC,
                            TIENNC = ct.SOLUONG * ct.GIANC,
                            ISCTYDTU = ct.ISCTYDTU
                        };
                       /* var ctqt = new CTQUYETTOAN
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC,
                            ISCTYDTU = ct.ISCTYDTU
                        };
                        _db.CTQUYETTOANs.InsertOnSubmit(ctqt);*/
                        _db.CTCHIETTINHs.InsertOnSubmit(ctct);
                    }
                    else
                    {
                        var ctct = new CTCHIETTINH_ND117
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            /*GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC*/
                            GIAVT = ct.GIAVT,
                            TIENVT = ct.SOLUONG * ct.GIAVT,
                            GIANC = ct.GIANC,
                            TIENNC = ct.SOLUONG * ct.GIANC
                        };
                        /*var ctqt117 = new CTQUYETTOAN_ND117
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC
                        };
                        _db.CTQUYETTOAN_ND117s.InsertOnSubmit(ctqt117);*/
                        _db.CTCHIETTINH_ND117s.InsertOnSubmit(ctct);
                    }
                }

                var gctk = _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(obj.MADDK)).ToList();
                foreach (var gc in gctk)
                {
                    var gcct = new GHICHU
                    {
                        MADDK = obj.MADDK,
                        NOIDUNG = gc.NOIDUNG
                    };
                    /*var gcqt = new GHICHUQUYETOAN
                    {
                        MADDK = obj.MADDK,
                        NOIDUNG = gc.NOIDUNG
                    };
                    _db.GHICHUQUYETOANs.InsertOnSubmit(gcqt);*/
                    _db.GHICHUs.InsertOnSubmit(gcct);
                }

                var cptk = _db.DAOLAPTKs.Where(p => p.MADDK.Equals(obj.MADDK)).ToList();
                foreach (var cp in cptk)
                {
                    var cpct = new DAOLAP_ND117
                    {
                        MADDK = obj.MADDK,
                        LOAICV = cp.LOAICV,
                        LOAICT = cp.LOAICT,
                        NOIDUNG = cp.NOIDUNG,
                        DONGIACP = cp.DONGIACP,
                        DVT = cp.DVT,
                        HESOCP = cp.HESOCP,
                        SOLUONG = cp.SOLUONG,
                        THANHTIENCP = cp.THANHTIENCP,
                        LOAICP = cp.LOAICP,
                        LOAI = cp.LOAI,
                        NGAYLAP = cp.NGAYLAP
                    };
                    /*var cpqt = new DAOLAPQUYETTOAN_ND117
                    {
                        MADDK = obj.MADDK,
                        LOAICV = cp.LOAICV,
                        LOAICT = cp.LOAICT,
                        NOIDUNG = cp.NOIDUNG,
                        DONGIACP = cp.DONGIACP,
                        DVT = cp.DVT,
                        HESOCP = cp.HESOCP,
                        SOLUONG = cp.SOLUONG,
                        THANHTIENCP = cp.THANHTIENCP,
                        LOAICP = cp.LOAICP,
                        LOAI = cp.LOAI,
                        NGAYLAP = cp.NGAYLAP
                    };

                    _db.DAOLAPQUYETTOAN_ND117s.InsertOnSubmit(cpqt);*/
                    _db.DAOLAP_ND117s.InsertOnSubmit(cpct);
                }

                _db.SubmitChanges();

                // commit
                trans.Commit();

                //UpdateChiPhiForChietTinh(obj.MADDK);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Chạy chiết tính"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Chạy chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chạy chiết tính");
            }

            return msg;
        }

        public Message CreateChietTinh3(THIETKE obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINH
                {
                    MADDK = obj.MADDK,
                    TENCT = obj.DONDANGKY.TENKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = obj.DONDANGKY.DIACHILD != null ? obj.DONDANGKY.DIACHILD : "",
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,
                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1,

                    TONGTIENCTPS = obj.TONGTIENTK != null ? obj.TONGTIENTK : 0
                };
               
                _db.CHIETTINHs.InsertOnSubmit(chiettinh);

                // update dondangky
                var objDDK = _db.DONDANGKies.FirstOrDefault(p => p.MADDK.Equals(obj.MADDK));
                if (objDDK != null) objDDK.TTCT = TTCT.CT_P.ToString();

                _db.SubmitChanges();

                // copy from ctthietke, gcthietke, daolaptk
                var cttk = _db.CTTHIETKEs.Where(ct => ct.MADDK.Equals(obj.MADDK)).ToList();
                foreach (var ct in cttk)
                {
                    if (ct.ISCTYDTU.HasValue && ct.ISCTYDTU.Value)
                    {
                        var ctct = new CTCHIETTINH
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,                          
                            GIAVT = ct.GIAVT,
                            TIENVT = ct.SOLUONG * ct.GIAVT,
                            GIANC = ct.GIANC,
                            TIENNC = ct.SOLUONG * ct.GIANC,
                            ISCTYDTU = ct.ISCTYDTU
                        };
                       
                        _db.CTCHIETTINHs.InsertOnSubmit(ctct);
                    }
                    else
                    {
                        var ctct = new CTCHIETTINH_ND117
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,                         
                            GIAVT = ct.GIAVT,
                            TIENVT = ct.SOLUONG * ct.GIAVT,
                            GIANC = ct.GIANC,
                            TIENNC = ct.SOLUONG * ct.GIANC
                        };
                     
                        _db.CTCHIETTINH_ND117s.InsertOnSubmit(ctct);
                    }
                }

                var gctk = _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(obj.MADDK)).ToList();
                foreach (var gc in gctk)
                {
                    var gcct = new GHICHU
                    {
                        MADDK = obj.MADDK,
                        NOIDUNG = gc.NOIDUNG
                    };
                  
                    _db.GHICHUs.InsertOnSubmit(gcct);
                }

                var cptk = _db.DAOLAPTKs.Where(p => p.MADDK.Equals(obj.MADDK)).ToList();
                foreach (var cp in cptk)
                {
                    var cpct = new DAOLAP_ND117
                    {
                        MADDK = obj.MADDK,
                        LOAICV = cp.LOAICV,
                        LOAICT = cp.LOAICT,
                        NOIDUNG = cp.NOIDUNG,
                        DONGIACP = cp.DONGIACP,
                        DVT = cp.DVT,
                        HESOCP = cp.HESOCP,
                        SOLUONG = cp.SOLUONG,
                        THANHTIENCP = cp.THANHTIENCP,
                        LOAICP = cp.LOAICP,
                        LOAI = cp.LOAI,
                        NGAYLAP = cp.NGAYLAP
                    };
                    
                    _db.DAOLAP_ND117s.InsertOnSubmit(cpct);
                }

                _db.SubmitChanges();

                // commit
                trans.Commit();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Chạy chiết tính"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                _db.Connection.Close();
                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Chạy chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();
                msg = ExceptionHandler.HandleInsertException(ex, "Chạy chiết tính");
            }

            return msg;
        }

        public Message CreateChietTinh2Po(THIETKEPO obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINH
                {
                    MADDK = obj.MADDKPO,
                    TENCT = obj.DONDANGKYPO.TENKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = obj.DONDANGKYPO.DIACHILD,
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,

                    //NGAYLCT
                    //NGAYGUI_CN
                    //NGAYNHAN_CN
                    //ISSTK
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,

                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1,
                    NGAYN = DateTime.Now
                };
               
                _db.CHIETTINHs.InsertOnSubmit(chiettinh);

                // update dondangky
                var objDDK = _db.DONDANGKYPOs.FirstOrDefault(p => p.MADDKPO.Equals(obj.MADDKPO));
                if (objDDK != null) 
                    objDDK.TTCT = TTCT.CT_P.ToString();

                _db.SubmitChanges();

                // copy from ctthietke, gcthietke, daolaptk
                var cttk = _db.CTTHIETKEs.Where(ct => ct.MADDK.Equals(obj.MADDKPO)).ToList();
                foreach (var ct in cttk)
                {
                    if (ct.ISCTYDTU.HasValue && ct.ISCTYDTU.Value)
                    {
                        var ctct = new CTCHIETTINH
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            /*GIAVT = ct.VATTU.GIAVT,                            
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC,                            
                            ISCTYDTU = ct.ISCTYDTU*/
                            GIAVT = ct.GIAVT,
                            TIENVT = ct.SOLUONG * ct.GIAVT,
                            GIANC = ct.GIANC,
                            TIENNC = ct.SOLUONG * ct.GIANC,
                            ISCTYDTU = ct.ISCTYDTU
                        };
                        /* var ctqt = new CTQUYETTOAN
                         {
                             MADDK = ct.MADDK,
                             MAVT = ct.MAVT,
                             LOAICT = CT.CT.ToString(),
                             LOAICV = "---***---",
                             SOLUONG = ct.SOLUONG,
                             GIAVT = ct.VATTU.GIAVT,
                             TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                             GIANC = ct.VATTU.GIANC,
                             TIENNC = ct.SOLUONG * ct.VATTU.GIANC,
                             ISCTYDTU = ct.ISCTYDTU
                         };
                         _db.CTQUYETTOANs.InsertOnSubmit(ctqt);*/
                        _db.CTCHIETTINHs.InsertOnSubmit(ctct);
                    }
                    else
                    {
                        var ctct = new CTCHIETTINH_ND117
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            /*GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC*/
                            GIAVT = ct.GIAVT,
                            TIENVT = ct.SOLUONG * ct.GIAVT,
                            GIANC = ct.GIANC,
                            TIENNC = ct.SOLUONG * ct.GIANC
                        };
                        /*var ctqt117 = new CTQUYETTOAN_ND117
                        {
                            MADDK = ct.MADDK,
                            MAVT = ct.MAVT,
                            LOAICT = CT.CT.ToString(),
                            LOAICV = "---***---",
                            SOLUONG = ct.SOLUONG,
                            GIAVT = ct.VATTU.GIAVT,
                            TIENVT = ct.SOLUONG * ct.VATTU.GIAVT,
                            GIANC = ct.VATTU.GIANC,
                            TIENNC = ct.SOLUONG * ct.VATTU.GIANC
                        };
                        _db.CTQUYETTOAN_ND117s.InsertOnSubmit(ctqt117);*/
                        _db.CTCHIETTINH_ND117s.InsertOnSubmit(ctct);
                    }
                }

                var gctk = _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(obj.MADDKPO)).ToList();
                foreach (var gc in gctk)
                {
                    var gcct = new GHICHU
                    {
                        MADDK = obj.MADDKPO,
                        NOIDUNG = gc.NOIDUNG
                    };
                    /*var gcqt = new GHICHUQUYETOAN
                    {
                        MADDK = obj.MADDK,
                        NOIDUNG = gc.NOIDUNG
                    };
                    _db.GHICHUQUYETOANs.InsertOnSubmit(gcqt);*/
                    _db.GHICHUs.InsertOnSubmit(gcct);
                }

                var cptk = _db.DAOLAPTKs.Where(p => p.MADDK.Equals(obj.MADDKPO)).ToList();
                foreach (var cp in cptk)
                {
                    var cpct = new DAOLAP_ND117
                    {
                        MADDK = obj.MADDKPO,
                        LOAICV = cp.LOAICV,
                        LOAICT = cp.LOAICT,
                        NOIDUNG = cp.NOIDUNG,
                        DONGIACP = cp.DONGIACP,
                        DVT = cp.DVT,
                        HESOCP = cp.HESOCP,
                        SOLUONG = cp.SOLUONG,
                        THANHTIENCP = cp.THANHTIENCP,
                        LOAICP = cp.LOAICP,
                        LOAI = cp.LOAI,
                        NGAYLAP = cp.NGAYLAP
                    };
                    /*var cpqt = new DAOLAPQUYETTOAN_ND117
                    {
                        MADDK = obj.MADDK,
                        LOAICV = cp.LOAICV,
                        LOAICT = cp.LOAICT,
                        NOIDUNG = cp.NOIDUNG,
                        DONGIACP = cp.DONGIACP,
                        DVT = cp.DVT,
                        HESOCP = cp.HESOCP,
                        SOLUONG = cp.SOLUONG,
                        THANHTIENCP = cp.THANHTIENCP,
                        LOAICP = cp.LOAICP,
                        LOAI = cp.LOAI,
                        NGAYLAP = cp.NGAYLAP
                    };

                    _db.DAOLAPQUYETTOAN_ND117s.InsertOnSubmit(cpqt);*/
                    _db.DAOLAP_ND117s.InsertOnSubmit(cpct);
                }

                _db.SubmitChanges();

                // commit
                trans.Commit();

                //UpdateChiPhiForChietTinh(obj.MADDK);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Chạy chiết tính điện"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Chạy chiết tính điện");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chạy chiết tính điện");
            }

            return msg;
        }
        /*
        public bool UpdateChiPhiForChietTinh(string ma)
        {
            try
            {
                var chiettinh = _db.CHIETTINHs.SingleOrDefault(p => p.MADDK.Equals(ma));
                if (chiettinh == null)
                    return false;

                return true;
                #region Bo
                ////gia tri co truoc trong chuong co so du lieu
                //decimal hsnhancong = 0;
                //decimal hschung = 0;
                //decimal hsthunhap = 0;
                //decimal hsthietke1 = 0;
                //decimal hsthietke2 = 0;
                //decimal hsthietke3 = 0;
                //decimal hsthue = 0;
                //decimal hscpc = 0;
                //decimal giamgiacpvl = 0;
                //decimal giamgiacpnc = 0;
                //if (chiettinh.HSNHANCONG != null)
                //{
                //    hsnhancong = (decimal) chiettinh.HSNHANCONG;
                //}
                //if (chiettinh.HSCHUNG != null)
                //{
                //    hschung = (decimal)chiettinh.HSCHUNG;
                //}
                //if (chiettinh.HSTHUNHAP != null)
                //{
                //    hsthunhap = (decimal)chiettinh.HSTHUNHAP;
                //}
                //if (chiettinh.HSTHIETKE1 != null)
                //{
                //    hsthietke1 = (decimal) chiettinh.HSTHIETKE1;
                //}
                //if (chiettinh.HSTHIETKE2 != null)
                //{
                //    hsthietke2 = (decimal) chiettinh.HSTHIETKE2;
                //}
                //if (chiettinh.HSTHIETKE3 != null)
                //{
                //    hsthietke3 = (decimal) chiettinh.HSTHIETKE3;
                //}
                //if (chiettinh.HSTHUE != null)
                //{
                //    hsthue = (decimal) chiettinh.HSTHUE;
                //}
                //if (chiettinh.HSCPC != null)
                //{
                //    hscpc = (decimal)chiettinh.HSCPC;
                //}

                ////gia tri se duoc tinh thanh
                //if (chiettinh.GIAMGIACPVL != null)
                //{
                //    giamgiacpvl = (decimal) chiettinh.GIAMGIACPVL ;
                //}
                //if (chiettinh.GIAMGIACPNC != null)
                //{
                //    giamgiacpnc = (decimal) chiettinh.GIAMGIACPNC ;
                //}

                //decimal cpvatlieu = 0;
                //decimal cpnhancong = 0;
                //decimal cpchung = 0;
                //decimal cpthunhap = 0;
                //decimal cpthietke = 0;
                //decimal cpkhac = 0;


                //decimal tienthue = 0;
                //decimal tongTt = 0;
                //decimal tongSt = 0;


                //decimal tongcong = 0;
                //decimal nhancong = 0;
                //decimal cpmay = 0;
                //decimal cpttvt = 0;
                //decimal cpttmay = 0;
                //decimal tcptt = 0;

                //decimal cptthue = 0;
                //decimal cpc = 0;
                //decimal hspvl = 0;
                //decimal hscpm = 0;
                //decimal hsttp = 0;
                //decimal cpkshs = 0;
                //decimal ttp = 0;

                                        

                //var ctchiettinhnd117List = _db.CTCHIETTINH_ND117s.Where(p => p.MADDK.Equals(ma)).ToList() ;
                //foreach (CTCHIETTINH_ND117 ctchiettinhNd117 in ctchiettinhnd117List)
                //{
                //    if (ctchiettinhNd117.TIENNC != null)
                //        cpnhancong = cpnhancong + (decimal) ctchiettinhNd117.TIENNC;

                //    if (ctchiettinhNd117.TIENVT  != null)
                //        cpvatlieu = cpvatlieu + (decimal)ctchiettinhNd117.TIENVT ;
                //}

                ////var daolapNd117S = _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(ma)).ToList();
                ////foreach (DAOLAP_ND117 daolapNd117 in daolapNd117S)
                ////{
                ////    if(daolapNd117 .)
                ////}
                ////- chi phí vật liệu và nhân công)
                //if( giamgiacpvl > 0)
                //{
                //    cpvatlieu = cpvatlieu - cpvatlieu*giamgiacpvl/100;
                //}
                
                //if(giamgiacpnc > 0)
                //{
                //    cpnhancong = cpnhancong - cpnhancong*giamgiacpnc/100;
                //}

                //cpvatlieu = Math.Round(cpvatlieu,0);

                //cpnhancong = cpnhancong*hsnhancong*hsthietke3;
                //cpc = (cpvatlieu + cpnhancong) * hscpc;
                //cpchung = (cpvatlieu + cpnhancong + cpc) * hschung;
                //cpthunhap = (cpvatlieu + cpnhancong + cpc + cpchung) * hsthunhap;
                //tienthue = (cpvatlieu + cpnhancong + cpc + cpchung + cpthunhap) * hsthue / 100;
                //cpthietke = (cpvatlieu + cpnhancong + cpc + cpchung + cpthunhap) * hsthietke1 * hsthietke2;
                //tongTt = (cpvatlieu + cpnhancong + cpc + cpchung + cpthunhap);


                //cpnhancong = Math.Round(cpnhancong/100, 0);
                //cpnhancong = cpnhancong*100;

               
                //cpc = Math.Round(cpc/100, 0);
                //cpc = cpc*100;

               
                //cpchung = Math.Round(cpchung/100, 0);
                //cpchung = cpchung*100;             
                               
               
                //cpthunhap = Math.Round(cpthunhap/100, 0);
                //cpthunhap = cpthunhap*100;

               
                //tienthue = Math.Round(tienthue/100, 0);
                //tienthue = tienthue*100;

               
                //cpthietke = Math.Round(cpthietke/100, 0);
                //cpthietke = cpthietke*100;

                
                //tongTt = Math.Round(tongTt/100, 0);
                //tongTt = tongTt*100;

                //tongSt = tienthue + cpthietke + tongTt;
                //tongSt = Math.Round(tongSt, 0);


                //chiettinh.CPNHANCONG = cpnhancong;
                //chiettinh.CPVATLIEU = cpvatlieu;
                //chiettinh.CPC = cpc;
                //chiettinh.CPCHUNG = cpchung;
                //chiettinh.CPTHUNHAP = cpthunhap;
                //chiettinh.TIENTHUE = tienthue;
                //chiettinh.CPTHIETKE = cpthietke;
                //chiettinh.TONG_TT = tongTt;
                //chiettinh.TONG_ST = tongSt;


                //_db.SubmitChanges();


                //return true;

                #endregion
            }
            catch
            {
                return false;
            }
        }
        */

        public Message ChangeFromMBVT(CHIETTINH objUi, MAUBOCVATTU mbvt)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                if (mbvt != null)
                {
                    // clear all existing data
                    var cttk = _db.CTCHIETTINHs.Where(ct => ct.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var ct in cttk)
                    {
                        _db.CTCHIETTINHs.DeleteOnSubmit(ct);
                    }
                    var ctqts=_db.CTQUYETTOANs.Where(qt=>qt.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var qt in ctqts)
                    {
                        _db.CTQUYETTOANs.DeleteOnSubmit(qt);
                    }

                    var cttk117 = _db.CTCHIETTINH_ND117s.Where(ct => ct.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var ct in cttk117)
                    {
                        _db.CTCHIETTINH_ND117s.DeleteOnSubmit(ct);
                    }
                    var ctqt117s = _db.CTQUYETTOAN_ND117s.Where(qt => qt.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var qt in ctqt117s)
                    {
                        _db.CTQUYETTOAN_ND117s.DeleteOnSubmit(qt);
                    }

                    _db.SubmitChanges();

                    // insert from maubocvattu
                    // insert:
                    // - CTMAUBOCVATTU to CTCHIETTINHs, CTCHIETTINH_ND117s
                    var ctmbvt = _db.CTMAUBOCVATTUs.Where(ct => ct.MADDK.Equals(mbvt.MADDK)).ToList();

                    foreach (var mb in ctmbvt)
                    {
                        if (mb.ISCTYDTU.HasValue && mb.ISCTYDTU.Value)
                        {
                            var ctct = new CTCHIETTINH
                                {
                                    MADDK = objUi.MADDK,
                                    MAVT = mb.MAVT,
                                    LOAICT = CT.CT.ToString(),
                                    LOAICV = "---***---",
                                    SOLUONG = mb.SOLUONG,
                                    GIAVT = mb.GIAVT,
                                    TIENVT = mb.SOLUONG * mb.GIAVT,
                                    GIANC = mb.GIANC,
                                    TIENNC = mb.SOLUONG * mb.GIANC,
                                    ISCTYDTU = mb.ISCTYDTU
                                };
                            _db.CTCHIETTINHs.InsertOnSubmit(ctct);
                        }
                        else
                        {
                            var ctct = new CTCHIETTINH_ND117
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.GIAVT,
                                TIENVT = mb.SOLUONG * mb.GIAVT,
                                GIANC = mb.GIANC,
                                TIENNC = mb.SOLUONG * mb.GIANC
                            };
                            _db.CTCHIETTINH_ND117s.InsertOnSubmit(ctct);
                        }

                        /*if (mb.ISCTYDTU.HasValue && mb.ISCTYDTU.Value)
                        {
                            var ctct = new CTCHIETTINH
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC,
                                ISCTYDTU = mb.ISCTYDTU
                            };
                            var ctqt = new CTQUYETTOAN
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC,
                                ISCTYDTU = mb.ISCTYDTU
                            };
                            _db.CTQUYETTOANs.InsertOnSubmit(ctqt);
                            _db.CTCHIETTINHs.InsertOnSubmit(ctct);
                        }
                        else
                        {
                            var ctct = new CTCHIETTINH_ND117
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC
                            };
                            var ctqt117 = new CTQUYETTOAN_ND117
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC
                            };
                            _db.CTQUYETTOAN_ND117s.InsertOnSubmit(ctqt117);
                            _db.CTCHIETTINH_ND117s.InsertOnSubmit(ctct);
                        }*/
                    }

                    /*var ctmbvt1 = _db.CTMAUBOCVATTUs.Where(ct => ct.MADDK.Equals(mbvt.MADDK)).ToList();
                    foreach (var mb1 in ctmbvt1)
                    {
                        if (mb1.ISCTYDTU.HasValue && mb1.ISCTYDTU.Value)
                        {
                            var ctqt = new CTQUYETTOAN
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb1.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb1.SOLUONG,
                                GIAVT = mb1.GIAVT,
                                TIENVT = mb1.SOLUONG * mb1.GIAVT,
                                GIANC = mb1.GIANC,
                                TIENNC = mb1.SOLUONG * mb1.GIANC,
                                ISCTYDTU = mb1.ISCTYDTU
                            };
                            _db.CTQUYETTOANs.InsertOnSubmit(ctqt);
                        }
                        else
                        {
                            var ctqt117 = new CTQUYETTOAN_ND117
                            {
                                MADDK = objUi.MADDK,
                                MAVT = mb1.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb1.SOLUONG,
                                GIAVT = mb1.GIAVT,
                                TIENVT = mb1.SOLUONG * mb1.GIAVT,
                                GIANC = mb1.GIANC,
                                TIENNC = mb1.SOLUONG * mb1.GIANC
                            };
                            _db.CTQUYETTOAN_ND117s.InsertOnSubmit(ctqt117);
                        }
                    }*/

                    var ctdl = _db.DAOLAPs.Where(d => d.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var _dl in ctdl)
                    {
                        _db.DAOLAPs.DeleteOnSubmit(_dl);
                    }
                    var ctdlqt = _db.DAOLAPQUYETTOANs.Where(d => d.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var dlqt in ctdlqt)
                    {
                        _db.DAOLAPQUYETTOANs.DeleteOnSubmit(dlqt);
                    }
                    var ctdl117 = _db.DAOLAP_ND117s.Where(d => d.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var _dl in ctdl117)
                    {
                        _db.DAOLAP_ND117s.DeleteOnSubmit(_dl);
                    }
                    var ctdlqt117 = _db.DAOLAPQUYETTOAN_ND117s.Where(d => d.MADDK.Equals(objUi.MADDK)).ToList();
                    foreach (var dlqt in ctdlqt117)
                    {
                        _db.DAOLAPQUYETTOAN_ND117s.DeleteOnSubmit(dlqt);
                    }
                    _db.SubmitChanges();

                    var ctmdl = _db.DAOLAPMAUBOCVATTUs.Where(d => d.MAMAUBOCVATTU.Equals(mbvt.MADDK)).ToList();
                    foreach (var dl in ctmdl)
                    {
                        var cpdl = new DAOLAP_ND117
                        {
                            MADDK = objUi.MADDK,
                            NOIDUNG = dl.NOIDUNG,
                            DONGIACP = dl.DONGIACP,
                            SOLUONG = dl.SOLUONG,
                            DVT = dl.DVT,
                            HESOCP = dl.HESOCP,
                            THANHTIENCP = dl.THANHTIENCP,
                            LOAICP = dl.LOAICP,
                            NGAYLAP = DateTime.Now,
                            LOAICT = dl.LOAICT
                        };
                        _db.DAOLAP_ND117s.InsertOnSubmit(cpdl);

                        /*var cpdlQT = new DAOLAPQUYETTOAN_ND117
                        {
                            MADDK = objUi.MADDK,
                            NOIDUNG = dl.NOIDUNG,
                            DONGIACP = dl.DONGIACP,
                            SOLUONG = dl.SOLUONG,
                            DVT = dl.DVT,
                            HESOCP = dl.HESOCP,
                            THANHTIENCP = dl.THANHTIENCP,
                            LOAICP = dl.LOAICP,
                            NGAYLAP = DateTime.Now,
                            LOAICT = dl.LOAICT
                        };                        
                        _db.DAOLAPQUYETTOAN_ND117s.InsertOnSubmit(cpdlQT);*/
                    }

                    /*var ctmdl1 = _db.DAOLAPMAUBOCVATTUs.Where(d => d.MAMAUBOCVATTU.Equals(mbvt.MADDK)).ToList();
                    foreach (var dl1 in ctmdl1)
                    {
                        var cpdlQT = new DAOLAPQUYETTOAN_ND117
                        {
                            MADDK = objUi.MADDK,
                            NOIDUNG = dl1.NOIDUNG,
                            DONGIACP = dl1.DONGIACP,
                            SOLUONG = dl1.SOLUONG,
                            DVT = dl1.DVT,
                            HESOCP = dl1.HESOCP,
                            THANHTIENCP = dl1.THANHTIENCP,
                            LOAICP = dl1.LOAICP,
                            NGAYLAP = DateTime.Now,
                            LOAICT = dl1.LOAICT
                        };
                        _db.DAOLAPQUYETTOAN_ND117s.InsertOnSubmit(cpdlQT);
                    }*/

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Đổi mẫu bốc vật tư cho chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Đổi mẫu bốc vật tư cho chiết tính");
            }
            return msg;
        }
    }
}
