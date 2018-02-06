using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Data.SqlClient;


namespace EOSCRM.Dao
{
    public  class KhachHangDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly ReportClass report = new ReportClass();
        private readonly ThayDongHoDao _tdh = new ThayDongHoDao();



        public KhachHangDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHACHHANG Get(string ma)
        {
            return _db.KHACHHANGs.FirstOrDefault(p => p.IDKH.Equals(ma));
        }

        public KHACHHANG GetKhachHangFromMadb(string MAKH)
        {
            return _db.KHACHHANGs.FirstOrDefault(p => (p.MADP + p.DUONGPHU + p.MADB) == MAKH);
        }

        public KHACHHANG GetKHDBKV(string MAKH, string makv)
        {
            return _db.KHACHHANGs.FirstOrDefault(p => (p.MADP + p.DUONGPHU + p.MADB) == MAKH && p.MAKV == makv);
        }
     
        public List<KHACHHANG> GetList()
        {
            return _db.KHACHHANGs.ToList();
        }

        public List<KHACHHANG> GetList(string madp)
        {
            return _db.KHACHHANGs.Where(kh => kh.MADP.Equals(madp))
                .OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANG> GetList(string madp, int fromStt, int toStt)
        {
            return _db.KHACHHANGs.Where(kh => kh.MADP.Equals(madp) &&
                                                kh.STT >= fromStt &&
                                                kh.STT <= toStt)
                .OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANG> SearchKhachHang(string maDanhBo, string tenKhachHang, string maDongHo, string soHopDong, string soNha, string duongPho, string maKv)
        {
            if (maDanhBo == "" && tenKhachHang == "" && maDongHo == "" && soHopDong == "" && soNha == "" && duongPho == "" && (maKv == "" || maKv == "NULL"))
                return null;

            var query = _db.KHACHHANGs.AsEnumerable();
            
            if(!string.IsNullOrEmpty(maDanhBo))
                query = query.Where(kh => (kh.MADP + kh.DUONGPHU + kh.MADB).ToUpper().Contains(maDanhBo.ToUpper()));

            if (!string.IsNullOrEmpty(tenKhachHang))
                query = query.Where(kh => (kh.TENKH.ToUpper().Contains(tenKhachHang.ToUpper())));

            if (!string.IsNullOrEmpty(maDongHo))
                query = query.Where(kh => (kh.MALDH.ToUpper().Contains(maDongHo.ToUpper())));

            if (!string.IsNullOrEmpty(soHopDong))
                query = query.Where(kh => (kh.SOHD.ToUpper().Contains(soHopDong.ToUpper())));

            if (!string.IsNullOrEmpty(soNha))
                query = query.Where(kh => (kh.SONHA.ToUpper().Contains(soNha.ToUpper())));

            if (!string.IsNullOrEmpty(duongPho))
                query = query.Where(kh => (kh.DUONGPHO.TENDP.ToUpper().Contains(duongPho.ToUpper()) || kh.MADP.Equals(duongPho)));

            if (!string.IsNullOrEmpty(maKv))
                query = query.Where(kh => (maKv=="%"  || kh.MAKV.Equals(maKv)));

            return query.ToList();
        }

        public int Count( )
        {
            return _db.KHACHHANGs.Count();
        }

        
        public Message Insert(KHACHHANG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                var count = _db.KHACHHANGs.Count(kh => kh.IDKH.Equals(objUi.IDKH) ||
                                                       (objUi.IDKH != null &&
                                                        kh.MADP.Equals(objUi.MADP) &&
                                                        kh.DUONGPHU.Equals(objUi.DUONGPHU) &&
                                                        kh.MADB.Equals(objUi.MADB)));
                if (count > 0)
                {
                    // success message
                    return new Message(MessageConstants.E_KH_MADB_TONTAI, MessageType.Error, "Thêm mới khách hàng");
                }

                // insert to KHACHHANG
                _db.KHACHHANGs.InsertOnSubmit(objUi);

                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Nhập khách hàng mới"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "khách hàng ");
                
                
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "Khách hàng ", objUi.TENKH);
            }
            return msg;
        }

        public Message UpdateTTSD(KHACHHANG obj, int thang, int nam, string ghichu, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(obj.IDKH);

                if (cu != null)
                {
                    //TODO: update all fields
                    cu.TTSD = obj.TTSD;
                    _db.SubmitChanges();

                    var ttsd = new TTSDKHACHHANG
                                   {
                                       THANG = thang,
                                       NAM = nam,
                                       IDKH = cu.IDKH,
                                       TTSD = cu.TTSD,                                       
                                       NGAYTHAYDOI = DateTime.Now,
                                       MANVTD = sManv,
                                       GHICHU = ghichu
                                   };
                    _db.TTSDKHACHHANGs.InsertOnSubmit(ttsd);
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = obj.TTSD == "CUP" ? "Cúp nước khách hàng" : "Mở nước khách hàng"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng ");
                    
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng ", obj.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpdateThayDongHo(KHACHHANG obj, DateTime ngayht, string tem, string ghichu, String useragent, String ipAddress, String sManv,
                                string madh, string maldh, DateTime ngaythay, DateTime ngayhoanthanh)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var cu = Get(obj.IDKH);

                if (cu != null)
                {
                    //TODO: insert into thay dong ho
                    var thaydongho = new THAYDONGHO
                                         {
                                             IDKH = obj.IDKH,
                                             MADHCU = madh,
                                             MALDHCU = maldh,
                                             NGAYTDCU = ngaythay,
                                             NGAYHTCU = ngayhoanthanh,                                             
                                             NGAYTD = obj.NGAYTHAYDH,
                                             NGAYHT = ngayht,
                                             MALDH = obj.MALDH,
                                             MADH = obj.MADH,
                                             KICHCO = obj.THUYLK,
                                             GHICHU = ghichu,
                                             SOTEM = tem,
                                             MANVTD = sManv,
                                             CHISONGUNG=obj.CHISODAU,
                                             CHISOBATDAU=obj.CHISOCUOI,
                                             MTRUYTHU = obj.m4Poor,
                                             CHISOMOI = obj.KLKHOAN,
                                             
                                             KYTHAYDH = DateTime.Now
                                         };
                    _db.THAYDONGHOs.InsertOnSubmit(thaydongho);
                    _db.SubmitChanges();

                    //TODO: update khach hang
                    cu.THUYLK = obj.THUYLK;
                    cu.LOAIDH = obj.MALDH != null ? _db.LOAIDHs.Single(p => p.MALDH.Equals(obj.MALDH)) : null;
                    cu.MADH = obj.MADH;
                    cu.NGAYTHAYDH = obj.NGAYTHAYDH;
                    cu.NGAYHT = obj.NGAYHT;
                    cu.CHISODAU = 0;
                    cu.CHISOCUOI = obj.CHISOCUOI;

                    if (cu.SLANTHAYDH == null)
                        cu.SLANTHAYDH = 1;
                    else cu.SLANTHAYDH = cu.SLANTHAYDH + 1;

                    _db.SubmitChanges();

                    #region Luu Vet

                    var luuvetKyduyet = new LUUVET_KYDUYET
                                            {
                                                MADON = obj.IDKH,
                                                IPAddress = ipAddress,
                                                MANV = sManv,
                                                UserAgent = useragent,
                                                NGAYTHUCHIEN = DateTime.Now,
                                                TACVU = TACVUKYDUYET.U.ToString(),
                                                MACN = CHUCNANGKYDUYET.KH05.ToString(),
                                                MATT = CHUCNANGKYDUYET.KH05.ToString(),
                                                MOTA = @"Thay đồng hồ"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "thay đồng hồ khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng", obj.TENKH);
                }

                // commit
                trans.Commit();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpThayDongHo(THAYDONGHO obj, DateTime ngayht, string tem, string ghichu, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var cu = _tdh.Get(obj.ID);

                if (cu != null)
                {
                    /*cu.CHISONGUNG = obj.CHISONGUNG;
                    cu.MTRUYTHU = obj.MTRUYTHU;
                    cu.MALDH = obj.MALDH;
                    cu.MADH = obj.MADH;
                    cu.CHISOBATDAU = obj.CHISOBATDAU;
                    cu.CHISOMOI = obj.CHISOMOI;
                    cu.NGAYTD = obj.NGAYTD;
                    cu.NGAYHT = obj.NGAYHT;
                    cu.KICHCO = obj.KICHCO;
                    cu.GHICHU = ghichu;

                    _db.SubmitChanges(); */

                    //TODO: update khach hang
                    var kh = Get(obj.IDKH);
                    kh.THUYLK = obj.KICHCO;
                    kh.LOAIDH = obj.MALDH != null ? _db.LOAIDHs.Single(p => p.MALDH.Equals(obj.MALDH)) : null;
                    kh.MADH = obj.MADH;
                    kh.NGAYTHAYDH = obj.NGAYTD;
                    kh.NGAYHT = obj.NGAYHT;
                    kh.CHISODAU = 0;
                    kh.CHISOCUOI = obj.CHISOMOI;                    

                    _db.SubmitChanges();
                    #region Luu Vet

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = @"Thay đồng hồ"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "thay đồng hồ khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thay đồng hồ", obj.IDKH);
                }

                // commit
                trans.Commit();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpdateSoBo(KHACHHANG moi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(moi.IDKH);

                if (cu != null)
                {
                    cu.STT = moi.STT;
                    cu.TENKH = moi.TENKH;
                    cu.SONHA = moi.SONHA;
                    cu.MADP = moi.MADP;
                    cu.MADB = moi.MADB;

                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = "Cập nhật sổ bộ"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }

        public Message UpdateMaDoan(KHACHHANG moi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(moi.IDKH);

                if (cu != null)
                {
                    var madoancu = cu.MADOAN;

                    var doan = new DoanDao().Get(moi.MADOAN, moi.MAKV);
                    if(doan == null)
                    {
                        return new Message(MessageConstants.E_INVALID_DATA, MessageType.Error, "Mã đoạn");
                    }

                    cu.MADOAN = moi.MADOAN;

                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = string.Format("Cập nhật mã đoạn: {0} -> {1}", madoancu, moi.MADOAN)
                    };

                    _kdDao.Insert(luuvetKyduyet);

                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }

        public Message Update(KHACHHANG moi, int nam, int thang, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                /*var count = _db.KHACHHANGs.Count(kh => !kh.IDKH.Equals(moi.IDKH) && 
                                                       kh.MADP.Equals(moi.MADP) &&
                                                       kh.DUONGPHU.Equals(moi.DUONGPHU) &&
                                                       kh.MADB.Equals(moi.MADB));
                
                if (count > 0)
                {
                    // success message
                    return new Message(MessageConstants.E_KH_MADB_TONTAI, MessageType.Error, "Cập nhật khách hàng");
                }*/

                // get current object in database
                var cu = Get(moi.IDKH);
                
                if (cu != null)
                {
                    //TODO: update all fields
                    cu.MADP = moi.MADP;
                    cu.MADB = moi.MADB;
                    cu.DUONGPHU = moi.DUONGPHU;
                    cu.MALKHDB = moi.MALKHDB;
                    cu.MACQ = moi.MACQ;
                    cu.MAMDSD = moi.MAMDSD;
                    cu.SOHD = moi.SOHD;
                    //cu.LOTRINH = moi.LOTRINH;
                    cu.STT = moi.STT;
                    cu.MABG = moi.MABG;
                    cu.MAPHUONG = moi.MAPHUONG;
                    cu.TENKH = moi.TENKH;
                    cu.SONHA = moi.SONHA;
                    cu.SOHO = moi.SOHO;
                    cu.MST = moi.MST;
                    cu.MAHTTT = moi.MAHTTT;
                    cu.STK = moi.STK;
                    cu.SDT = moi.SDT;
                    cu.MAKV = moi.MAKV;
                    cu.MADH = moi.MADH;
                    cu.NGAYHT = moi.NGAYHT;
                    cu.NGAYTHAYDH = moi.NGAYTHAYDH;
                    cu.NGAYCUP = moi.NGAYCUP;
                    cu.NGAYGNHAP = moi.NGAYGNHAP;
                    cu.SLANTHAYDH = moi.SLANTHAYDH;
                    cu.TTSD = moi.TTSD;
                    cu.KOPHINT = moi.KOPHINT;
                    cu.THUYLK = moi.THUYLK;
                    cu.MATT = moi.MATT;
                    cu.VAT = moi.VAT;
                    cu.SDInfo_INHOADON = moi.SDInfo_INHOADON;
                    cu.TENKH_INHOADON = moi.TENKH_INHOADON;
                    cu.DIACHI_INHOADON = moi.DIACHI_INHOADON;
                    cu.THANGBDKT = moi.THANGBDKT;
                    cu.NAMBDKT = moi.NAMBDKT;
                    cu.CHISOCUOI = moi.CHISOCUOI;
                    cu.CHISODAU = moi.CHISODAU;
                    cu.SONK = moi.SONK;
                    cu.ISDINHMUC = moi.ISDINHMUC;
                    cu.GHI2THANG1LAN = moi.GHI2THANG1LAN;
                    cu.KHONGTINH117 = moi.KHONGTINH117;
                    cu.KYHOTRO = moi.KYHOTRO;
                    cu.CMND = moi.CMND;
                    
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = "Cập nhật khách hàng"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion                                    
                    

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng ");
                    
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH     );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }
        
        public bool IsInUse(string ma)
        {
            if (_db.TIEUTHUs.Count(p => p.IDKH.Equals(ma)) > 0)
                return true;
            else if (_db.KHACHHANG_EDITs.Count(p => p.IDKH.Equals(ma)) > 0)
                return true;
            else
            {
                return false;
            }
        }

        public bool ExistsMaKhachHang(string idkh, string madp, string madb)
        {
            return
                (_db.KHACHHANGs.Count(p => (p.IDKH.Equals(idkh) || string.IsNullOrEmpty(idkh)) && p.MADP.Equals(madp) && p.MADB.Equals(madb)) > 0);

        }

        public bool ExistsAnotherMaKhachHang(string idkh, string madp, string madb)
        {
            return
                (_db.KHACHHANGs.Count(p => (p.IDKH.Equals(idkh) || string.IsNullOrEmpty(idkh)) && p.MADP.Equals(madp) && p.MADB.Equals(madb)) > 1);

        }

        public Message Delete(KHACHHANG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.IDKH);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng ", objUi.TENKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

              
                // Set delete info
                _db.KHACHHANGs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Xóa khách hàng"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Khách hàng");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Khách hàng ");
            }

            return msg;
        }
        
        public Message DeleteList(List<KHACHHANG> objList, PageAction action, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in objList)
                {
                    //TODO: check valid update infor
                    Delete(obj, useragent, ipAddress, sManv );
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Khách hàng ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách khách hàng ");
            }

            return msg;
        }

        public string NewId()
        {
            var query = _db.KHACHHANGs.Max(p => p.IDKH);

            var temp = int.Parse(query) + 1;
            return temp.ToString("D6");
        }

        public string NewMADB(string maDP)
        {
            var query = (from p in _db.KHACHHANGs.Where(p => p.MADP.Equals(maDP))
                        select p.MADB).Max();
            if (!string.IsNullOrEmpty(query))
            {
                var bn = 20;
                var dp = _db.DUONGPHOs.FirstOrDefault(d => d.MADP.Equals(maDP));
                if (dp != null && dp.BUOCNHAY.HasValue && dp.BUOCNHAY > 0)
                    bn = dp.BUOCNHAY.Value;

                var temp = int.Parse(query) + bn;
                
                return temp.ToString("D4");
            }

            return "0001";
           
        }

        public int NewSTT(string maDp)
        {
            var query = (from p in _db.KHACHHANGs.Where(p => p.MADP.Equals(maDp))
                         select p.STT).Max();

            if (query.HasValue)
                return query.Value + 1;

            return 1;
        }

        public List<KHACHHANG> GetListInKKT(int thang, int nam)
        {
            return _db.KHACHHANGs.Where(k => (k.THANGBDKT.HasValue && 
                                                k.THANGBDKT.Value.Equals(thang) &&
                                                k.NAMBDKT.HasValue &&
                                                k.NAMBDKT.Value.Equals(nam)))
                                            .OrderByDescending(k => k.IDKH).ToList();
        }

        public List<KHACHHANG> GetListKH(int thang, int nam)
        {
            return _db.KHACHHANGs.Where(k => k.KYKHAITHAC.Month==thang && k.KYKHAITHAC.Year==nam)
                                            .OrderByDescending(k => k.IDKH).ToList();
        }

        public List<KHACHHANG> GetList(String IDKH, String SOHD, String MADH, String TENKH, String SONHA, String TENDP, String MAKV, String GHI2THANG1LAN)
        {
            var dskh = from kh in _db.KHACHHANGs
                       select kh;

            if (IDKH != null)
                dskh = dskh.Where(kh => (kh.MADP + kh.DUONGPHU + kh.MADB).Contains(IDKH));

            if (SOHD != null)
                dskh = dskh.Where(kh => kh.SOHD.Equals(SOHD));

            if (MADH != null)
                dskh = dskh.Where(kh => kh.MADH.Contains(MADH));

            if (TENKH != null)
                dskh = dskh.Where(kh => kh.TENKH.Contains(TENKH));

            if (SONHA != null)
                dskh = dskh.Where(kh => kh.SONHA.Contains(SONHA));

            if (TENDP != null)
                dskh = dskh.Where(kh => kh.DUONGPHO.TENDP.Contains(TENDP) ||
                                            kh.MADP.Equals(TENDP));

            if (MAKV != null)
                dskh = dskh.Where(kh => kh.MAKV.Equals(MAKV));

            if (GHI2THANG1LAN != null)
                dskh = dskh.Where(kh => kh.GHI2THANG1LAN.Equals(GHI2THANG1LAN));           

            return dskh
                        .OrderBy(kh => kh.STT)
                        .OrderBy(kh => kh.DUONGPHU)
                        .OrderBy(kh => kh.MADP)
                        .ToList();
        }

        public List<TIEUTHU> GetThongTinTieuThu(string idkh)
        {
            return _db.TIEUTHUs.Where(tt => tt.IDKH.Equals(idkh))
                .OrderByDescending(tt => tt.THANG)
                .OrderByDescending(tt => tt.NAM).ToList();
        }

        public List<TTSDKHACHHANG> GetTTSDList(int thang, int nam, string ttsd)
        {
            return
                _db.TTSDKHACHHANGs.Where(tt => (tt.THANG == thang && tt.NAM == nam && tt.TTSD == ttsd))
                        .OrderByDescending(tt => tt.NGAYTHAYDOI).ToList();
        }

        public TTSDKHACHHANG GetTTSD(int id)
        {
            return _db.TTSDKHACHHANGs.SingleOrDefault(tt => tt.ID == id); 
        }

        public List<THAYDONGHO> GetThayDongHoList(int thang, int nam)
        {
            return
                _db.THAYDONGHOs.Where(tt =>tt.NGAYTD .HasValue && tt.NGAYTD.Value.Month == thang && tt.NGAYTD.Value.Year == nam  ).ToList();
        }

        public List<THAYDONGHO> GetThayDongHoList()
        {
            return _db.THAYDONGHOs.OrderByDescending(t => t.ID).ToList();
        }

        public List<THAYDONGHO> GetThayDongHoListThang(int thang)
        {
            return _db.THAYDONGHOs.Where(t => t.KYTHAYDH.Value.Month == thang).ToList();
        }
    }
}
