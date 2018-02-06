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
    public class DuongPhoPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        private readonly LichGCSDao _lgcspoDao = new LichGCSDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();


        public DuongPhoPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DUONGPHOPO Get(string ma, string duongphu)
        {
            return _db.DUONGPHOPOs.Where(p => p.MADPPO .Equals(ma) && p.DUONGPHUPO .Equals( duongphu)).SingleOrDefault();
        }

        public DUONGPHOPO GetDP(string ma)
        {
            return _db.DUONGPHOPOs.Where(p => p.MADPPO.Equals(ma) ).SingleOrDefault();
        }

        public DUONGPHOPO Get(string ma, string duongphu, string makv)
        {
            return _db.DUONGPHOPOs.Where(p => p.MADPPO.Equals(ma) && 
                            p.DUONGPHUPO.Equals(duongphu) &&
                            p.MAKVPO.Equals(makv)).SingleOrDefault();
        }

        public List<DUONGPHOPO> Search(string key)
        {
            return
                _db.DUONGPHOPOs.Where(
                    p => p.TENDP.ToUpper().Contains(key.ToUpper()) || p.TENTAT.ToUpper().Contains(key.ToUpper())).ToList
                    ();
        }

        public List<DUONGPHOPO> GetList()
        {
            return _db.DUONGPHOPOs.OrderBy(dp => dp.MADPPO).ToList();
        }

        public List<DUONGPHOPO> GetListKV(string makv)
        {
            return _db.DUONGPHOPOs.Where(p => p.MAKVPO.Equals(makv)).OrderBy(dp => dp.MADPPO).ToList();
        }

        public List<DUONGPHOPO> GetListMADP(string madp)
        {
            return _db.DUONGPHOPOs.Where(p => p.MADPPO.Equals(madp)).OrderBy(dp => dp.MADPPO).ToList();
        }

        public List<DUONGPHOPO> GetListKVDotIn(string makv, string madotin)
        {
            var query = _db.DUONGPHOPOs.ToList();

            if (madotin != "%")
            {
                query = query.Where(p => p.MAKVPO.Equals(makv) && p.IDMADOTIN.Equals(madotin)).ToList();
            }
            else
            {
                query = query.Where(p => p.MAKVPO.Equals(makv)).ToList();
            }

            return query.OrderBy(dp => dp.MADPPO).ToList();
        }

        public List<DUONGPHOPO> GetList(String madp, String duongphu, String tendp, 
                String makv, DateTime? ngayghi, int? tlpt)
        {
            var query = _db.DUONGPHOPOs.AsQueryable();

            if (!String.IsNullOrEmpty(madp))
                query = query.Where(dp => dp.MADPPO.ToUpper().Contains(madp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(duongphu))
                query = query.Where(dp => dp.DUONGPHUPO.ToUpper().Contains(duongphu.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(tendp))
                query = query.Where(dp => dp.TENDP.ToUpper().Contains(tendp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(dp => dp.MAKVPO.ToUpper().Equals(makv.ToUpper())).AsQueryable();

            if(ngayghi.HasValue)
                query = query.Where(dp => dp.NGAYGHI.Equals(ngayghi.Value)).AsQueryable();
            if (tlpt.HasValue)
                query = query.Where(dp => dp.TLPHUTHU.Equals(tlpt.Value)).AsQueryable();
            

            return query.OrderBy(dp => dp.MADPPO).ToList();
        }

        public List<DUONGPHOPO> GetList(string maKV)
        {
            if (!string.IsNullOrEmpty(maKV))
                if (maKV != "%")
                    return _db.DUONGPHOPOs.Where(p => p.MAKVPO.Equals(maKV)).OrderBy(dp => dp.MADPPO).ToList();
                    
            return _db.DUONGPHOPOs.OrderBy(dp=>dp.MADPPO).ToList();
        }

        public List<DUONGPHOPO> GetList(string maKV, string keyword)
        {
            var list = _db.DUONGPHOPOs.Where(p => p.MAKVPO.Equals(maKV)).ToList();

            if (!string.IsNullOrEmpty(maKV))
                if (maKV != "%")
                    list = list.Where(p => p.MAKVPO.Equals(maKV)).ToList();

            if (keyword != "")
                list = list.Where(p => (p.MADPPO.ToLower().Contains(keyword.ToLower()) ||
                                        p.DUONGPHUPO.ToLower().Contains(keyword.ToLower()) ||
                                        p.TENDP.ToLower().Contains(keyword.ToLower()))).ToList();
            return list;
        }

        public List<DUONGPHOPO> GetListKV(String madp, String duongphu, String tendp,  String makv)
        {
            var query = _db.DUONGPHOPOs.Where(p => p.MAKVPO.Equals(makv)).AsQueryable();

            if (!String.IsNullOrEmpty(madp))
                query = query.Where(dp => dp.MADPPO.ToUpper().Contains(madp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(duongphu))
                query = query.Where(dp => dp.DUONGPHUPO.ToUpper().Contains(duongphu.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(tendp))
                query = query.Where(dp => dp.TENDP.ToUpper().Contains(tendp.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(dp => dp.MAKVPO.ToUpper().Equals(makv.ToUpper())).AsQueryable();

            return query.OrderBy(dp => dp.MADPPO).ToList();
        }        

        public string NewId()
        {
            var query = (from p in _db.DUONGPHOPOs
                         select p.MADPPO).Max();

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
            return _db.DUONGPHOPOs.Count();
        }

        public Message Insert(DUONGPHOPO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DUONGPHOPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

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

        public Message Update(DUONGPHOPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADPPO , objUi.DUONGPHUPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENDP   = objUi.TENDP  ;
                    objDb.TENTAT  = objUi.TENTAT   ;
                    objDb.KOPHIMT = objUi.KOPHIMT ;
                    if (!string.IsNullOrEmpty(objUi.MAKVPO))
                        objDb.KHUVUCPO = _db.KHUVUCPOs.Single(p => p.MAKVPO.Equals(objUi.MAKVPO));
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

                        // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADPPO,
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

                    var lichghi = _lgcspoDao.GetListKyDPKV(namht, thanght, madp, objDb.MAKVPO);
                    if (lichghi == null)
                    {
                        #region Add Lich GCS
                        var lichgcs = new LICHGCS
                        {
                            NAM = namht,
                            THANG = thanght,
                            MADP = madp,
                            TENDP = objDb.TENDP,
                            MAKV = objDb.MAKVPO,
                            IDMADOTIN = idmadotin,
                            NGAYN = DateTime.Now,
                            MANVN = sManv,
                            IDMADOTINCU = objDb.IDMADOTIN
                        };

                        _lgcspoDao.Insert(lichgcs);
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
                        lichgcs.MAKV = objDb.MAKVPO;
                        lichgcs.IDMADOTIN = idmadotin;
                        lichgcs.NGAYN = DateTime.Now;
                        lichgcs.MANVN = sManv;

                        _lgcspoDao.Update(lichgcs, useragent, ipAddress, sManv);
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
                        MATT = "UDOTINLGCSPO",
                        MOTA = "Cập nhật đợt ghi chỉ số điện."
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

        public Message Delete(DUONGPHOPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADPPO , objUi .DUONGPHUPO .ToString( ) );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đường phố", objUi.TENDP      );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DUONGPHOPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADPPO,
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

        public Message DeleteList(List<DUONGPHOPO> objList, String useragent, String ipAddress, String sManv)
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
