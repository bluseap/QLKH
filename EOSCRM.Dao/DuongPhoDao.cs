using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public  class DuongPhoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        private readonly LichGCSDao _lgcsDao = new LichGCSDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public DuongPhoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DUONGPHO Get(string ma, string duongphu)
        {
            return _db.DUONGPHOs.Where(p => p.MADP .Equals(ma) && p.DUONGPHU .Equals( duongphu)).SingleOrDefault();
        }

        public DUONGPHO GetDP(string ma)
        {
            return _db.DUONGPHOs.Where(p => p.MADP.Equals(ma) ).SingleOrDefault();
        }

        public DUONGPHO Get(string ma, string duongphu, string makv)
        {
            return _db.DUONGPHOs.Where(p => p.MADP.Equals(ma) && 
                            p.DUONGPHU.Equals(duongphu) &&
                            p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public List<DUONGPHO> Search(string key)
        {
            return
                _db.DUONGPHOs.Where(
                    p => p.TENDP.ToUpper().Contains(key.ToUpper()) || p.TENTAT.ToUpper().Contains(key.ToUpper())).ToList
                    ();
        }

        public List<DUONGPHO> GetList()
        {
            return _db.DUONGPHOs.OrderBy(dp => dp.MADP).ToList();
        }

        public List<DUONGPHO> GetListMADP(string madp)
        {
            return _db.DUONGPHOs.Where(p => p.MADP.Equals(madp)).OrderBy(dp => dp.MADP).ToList();
        }

        public List<DUONGPHO> GetListKV(string makv)
        {
            return _db.DUONGPHOs.Where(p => p.MAKV.Equals(makv)).OrderBy(dp => dp.MADP).ToList();
        }

        public List<DUONGPHO> GetList(String madp, String duongphu, String tendp, 
                String makv, DateTime? ngayghi, int? tlpt)
        {
            var query = _db.DUONGPHOs.AsQueryable();

            if (!String.IsNullOrEmpty(madp))
                query = query.Where(dp => dp.MADP.ToUpper().Contains(madp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(duongphu))
                query = query.Where(dp => dp.DUONGPHU.ToUpper().Contains(duongphu.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(tendp))
                query = query.Where(dp => dp.TENDP.ToUpper().Contains(tendp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(dp => dp.MAKV.ToUpper().Equals(makv.ToUpper())).AsQueryable();

            if(ngayghi.HasValue)
                query = query.Where(dp => dp.NGAYGHI.Equals(ngayghi.Value)).AsQueryable();
            if (tlpt.HasValue)
                query = query.Where(dp => dp.TLPHUTHU.Equals(tlpt.Value)).AsQueryable();
            

            return query.OrderBy(dp => dp.MADP).ToList();
        }

        public List<DUONGPHO> GetListKV(String madp, String duongphu, String tendp,
                String makv, DateTime? ngayghi, int? tlpt)
        {
            var query = _db.DUONGPHOs.Where(p => p.MAKV.Equals(makv)).AsQueryable();

            if (!String.IsNullOrEmpty(madp))
                query = query.Where(dp => dp.MADP.ToUpper().Contains(madp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(duongphu))
                query = query.Where(dp => dp.DUONGPHU.ToUpper().Contains(duongphu.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(tendp))
                query = query.Where(dp => dp.TENDP.ToUpper().Contains(tendp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(dp => dp.MAKV.ToUpper().Equals(makv.ToUpper())).AsQueryable();

            if (ngayghi.HasValue)
                query = query.Where(dp => dp.NGAYGHI.Equals(ngayghi.Value)).AsQueryable();
            if (tlpt.HasValue)
                query = query.Where(dp => dp.TLPHUTHU.Equals(tlpt.Value)).AsQueryable();


            return query.OrderBy(dp => dp.MADP).ToList();
        }

        public List<DUONGPHO> GetList(string maKV)
        {
            if (!string.IsNullOrEmpty(maKV))
                if (maKV != "%")
                    return _db.DUONGPHOs.Where(p => p.MAKV.Equals(maKV)).OrderBy(dp => dp.MADP).ToList();
                    
            return _db.DUONGPHOs.OrderBy(dp=>dp.MADP).ToList();
        }

        public List<DUONGPHO> GetListKVDotIn(string makv, string madotin)
        {
            var query = _db.DUONGPHOs.ToList();

            if (madotin != "%")
            {
                query = query.Where(p => p.MAKV.Equals(makv) && p.IDMADOTIN.Equals(madotin)).ToList();                    
            }
            else
            {
                query = query.Where(p => p.MAKV.Equals(makv)).ToList();
            }          

            return query.OrderBy(dp => dp.MADP).ToList();
        }

        public List<DUONGPHO> GetList(string maKV, string keyword)
        {
            var list = _db.DUONGPHOs.ToList();

            if (!string.IsNullOrEmpty(maKV))
                if (maKV != "%")
                    list = list.Where(p => p.MAKV.Equals(maKV)).ToList();

            if (keyword != "")
                list = list.Where(p => (p.MADP.ToLower().Contains(keyword.ToLower()) ||
                                        p.DUONGPHU.ToLower().Contains(keyword.ToLower()) ||
                                        p.TENDP.ToLower().Contains(keyword.ToLower()))).ToList();
            return list;
        }

        public List<DUONGPHO> GetListKV(string maKV, string keyword)
        {
            var list = _db.DUONGPHOs.Where(p => p.MAKV.Equals(maKV)).ToList();

            if (!string.IsNullOrEmpty(maKV))
                if (maKV != "%")
                    list = list.Where(p => p.MAKV.Equals(maKV)).ToList();

            if (keyword != "")
                list = list.Where(p => (p.MADP.ToLower().Contains(keyword.ToLower()) ||
                                        p.DUONGPHU.ToLower().Contains(keyword.ToLower()) ||
                                        p.TENDP.ToLower().Contains(keyword.ToLower()))).ToList();
            return list;
        }

        public string NewId()
        {
            var query = (from p in _db.DUONGPHOs
                         select p.MADP).Max();

            if (!string.IsNullOrEmpty(query))
            {
                try
                {
                    var temp = int.Parse(query) + 1;
                    //return temp.ToString("D3");
                    return temp.ToString("D4");
                }
                catch
                {
                    return "";
                }
            }

            return "";
        }

        public int Count( )
        {
            return _db.DUONGPHOs.Count();
        }

        public Message Insert(DUONGPHO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DUONGPHOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADP,
                    IPAddress = "",
                    MANV = "",
                    UserAgent = "",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Thêm đường phố"
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đường phố");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "đường phố", objUi.TENDP);
            }
            return msg;
        }

        public Message InsertDMMOI(DUONGPHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DUONGPHOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADP,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Thêm đường phố mới"
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đường phố");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "đường phố", objUi.TENDP);
            }
            return msg;
        }

        public Message Update(DUONGPHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADP , objUi.DUONGPHU);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENDP   = objUi.TENDP  ;
                    objDb.TENTAT  = objUi.TENTAT   ;
                    objDb.KOPHIMT = objUi.KOPHIMT ;
                    if (!string.IsNullOrEmpty(objUi.MAKV))
                        objDb.KHUVUC = _db.KHUVUCs.Single(p => p.MAKV.Equals(objUi.MAKV));
                    if (!string.IsNullOrEmpty(objUi.MANVG))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVG));
                    if (!string.IsNullOrEmpty(objUi.MANVT))
                        objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVT));
                    //objDb.MANVG  = objUi.MANVG   ;
                    //objDb.MANVT= objUi.MANVT   ;
                    objDb.NGAYGHI= objUi.NGAYGHI;
                    objDb.NONGTHON = objUi.NONGTHON;
                    objDb.TLPHUTHU = objUi.TLPHUTHU;
                    objDb.BUOCNHAY = objUi.BUOCNHAY;
                    objDb.GIAKHAC = objUi.GIAKHAC;
                    objDb.GIACQK = objUi.GIACQK;
                    objDb.THUECQK = objUi.THUECQK;
                    objDb.PHICQK = objUi.PHICQK;
                    objDb.GIAKDK = objUi.GIAKDK;
                    objDb.THUEKDK = objUi.THUEKDK;
                    objDb.PHIKDK = objUi.PHIKDK;
                    objDb.GIAHNK = objUi.GIAHNK;
                    objDb.THUEHNK = objUi.THUEHNK;
                    objDb.PHIHNK = objUi.PHIHNK;
                    objDb.GIASHK = objUi.GIASHK;
                    objDb.THUESHK = objUi.THUESHK;
                    objDb.PHISHK = objUi.PHISHK;

                    objDb.TIENNUOCCQ = objUi.TIENNUOCCQ;
                    objDb.TIENNUOCHN = objUi.TIENNUOCHN;
                    objDb.TIENNUOCKD = objUi.TIENNUOCKD;
                    objDb.TIENNUOCSH = objUi.TIENNUOCSH;

                    objDb.IDMADOTIN = objUi.IDMADOTIN;

                    //objDb.DOT = objUi.DOT;

                        // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADP,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = "Cập nhật đường phố"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đường phố ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đường phố ", objUi.TENDP       );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đường phố ", objUi.TENDP   );
            }
            return msg;
        }

        public Message UpDotInInLichGCS(string madp, string idmadotin, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetDP(madp);

                if (objDb != null)
                {                   

                    int namht = DateTime.Now.Year;
                    int thanght = DateTime.Now.Month;

                    var lichghi = _lgcsDao.GetListKyDPKV(namht, thanght, madp, objDb.MAKV);
                    if (lichghi == null)
                    {
                        #region Add Lich GCS
                        var lichgcs = new LICHGCS
                        {
                            NAM = namht,
                            THANG = thanght,
                            MADP = madp,
                            TENDP = objDb.TENDP,
                            MAKV = objDb.MAKV,
                            IDMADOTIN = idmadotin,
                            NGAYN = DateTime.Now,
                            MANVN = sManv,
                            IDMADOTINCU = objDb.IDMADOTIN
                        };

                        _lgcsDao.Insert(lichgcs);
                        _db.SubmitChanges();
                        #endregion
                    }
                    else
                    {
                        #region Update Lich GCS
                        var lichgcs = lichghi;

                        lichgcs.NAM = namht;
                        lichgcs.THANG = thanght;
                        lichgcs.MADP = madp;
                        lichgcs.TENDP = objDb.TENDP;
                        lichgcs.MAKV = objDb.MAKV;
                        lichgcs.IDMADOTIN = idmadotin;
                        lichgcs.NGAYN = DateTime.Now;
                        lichgcs.MANVN = sManv;

                        _lgcsDao.Update(lichgcs, useragent, ipAddress, sManv);
                        _db.SubmitChanges();
                        #endregion 
                    }

                    //TODO: update all fields
                    objDb.IDMADOTIN = idmadotin;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = madp,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UDOTINLGCS",
                        MOTA = "Cập nhật đợt ghi chỉ số"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đường phố ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đường phố ", madp);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đường phố ", madp);
            }
            return msg;
        }

        public Message UpDotInInLichGCS2(string madp, string idmadotin, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetDP(madp);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.IDMADOTIN = idmadotin;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = madp,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UDOTINLGCS",
                        MOTA = "Cập nhật đợt ghi chỉ số."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đường phố ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đường phố ", madp);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đường phố ", madp);
            }
            return msg;
        }

        public bool IsInUse(string ma, string duongphu)
        {
            if (_db.DONDANGKies.Count(p => p.MADP.Equals(ma) && p.DUONGPHU.Equals(duongphu)) > 0)
                return true;

            if (_db.KHACHHANGs.Count(p => p.MADP.Equals(ma) && p.DUONGPHU.Equals(duongphu)) > 0)
                return true;

            if (_db.TIEUTHUs.Count(p => p.MADP.Equals(ma) && p.DUONGPHU.Equals(duongphu)) > 0)
                return true;

            if (_db.HOPDONGs.Count(p => p.MADP.Equals(ma) && p.DUONGPHU.Equals(duongphu)) > 0)
                return true;

            return false;
        }

        public Message Delete(DUONGPHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADP , objUi .DUONGPHU .ToString( ) );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đường phố", objUi.TENDP      );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DUONGPHOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADP,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Xóa đường phố"
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đường phố ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đường phố ");
            }

            return msg;
        }

        public Message DeleteList(List<DUONGPHO> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var failed = 0;

                foreach (var obj in objList)
                {
                    var temp = Delete(obj, useragent, ipAddress, sManv);
                    if (temp != null && temp.MsgType.Equals(MessageType.Error))
                        failed++;
                }

                // commit
                trans.Commit();

                if (failed > 0)
                {
                    if (failed == objList.Count)
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách đường phố");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "đường phố", failed, "đường phố");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " đường phố");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách đường phố");
            }

            return msg;
        }

        public List<DUONGPHOGT> GetListGhiThu(String makv, String manv, int nam, int thang)
        {
            var list = _db.DUONGPHOGTs.Where(gt => gt.NAM == nam && gt.THANG == thang);

            if(makv != null)
                list = list.Where(gt => gt.MAKV == makv);
            
            if (manv != null)
                list = list.Where(gt => gt.MANVG == manv || gt.MANVT == manv);
            
            return list.OrderBy(gt => gt.MADP).OrderBy(gt => gt.MAKV).ToList();
        }

        public List<NHANVIEN> GetListNhanVienGT(String makv, int nam, int thang)
        {
            var list = _db.DUONGPHOGTs.Where(gt => gt.NAM == nam && gt.THANG == thang);
            if (makv != null && makv != "%")
                list = list.Where(gt => gt.MAKV == makv);

            return list.Select(gt => gt.NHANVIEN).Distinct().ToList();
        }

        public Message UpdateGhiThu(List<DUONGPHOGT> listGT)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                //TODO: cap nhat phan cong ghi thu o day
                foreach(var gt in listGT)
                {
                    if (gt == null) continue;
                    var objDb = _db.DUONGPHOGTs.SingleOrDefault(tt => tt.NAM == gt.NAM &&

                                                                      tt.THANG == gt.THANG &&         
                                                                      tt.MADP == gt.MADP &&
                                                                      tt.DUONGPHU == gt.DUONGPHU);
                    // ReSharper restore AccessToModifiedClosure

                    if (objDb == null) continue;
                    objDb.MANVG = gt.MANVG;
                    objDb.MANVT = gt.MANVT;

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();
                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Phân công ghi thu");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Phân công ghi thu");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "Phân công ghi thu");
            }

            return msg;
        }
    }
}
