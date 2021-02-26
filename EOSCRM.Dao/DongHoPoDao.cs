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
    public class DongHoPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DongHoPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DONGHOPO  Get(string ma)
        {
            return _db.DONGHOPOs.Where(p => p.MADHPO.Equals(ma)).SingleOrDefault();
        }

        public DONGHOPO GetDASD(string ma)
        {
            return _db.DONGHOPOs.Where(p => p.MADHPO.Equals(ma) && p.DASD.Equals(1)).SingleOrDefault();
        }

        public List<DONGHOPO> Search(string key)
        {
            return _db.DONGHOPOs.Where(p => p.MADHPO.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<DONGHOPO> GetList()
        {
            return _db.DONGHOPOs.OrderByDescending(dh => dh.MADHPO).ToList();
        }

        public List<DONGHOPO> GetListKV(string makv)
        {
            return _db.DONGHOPOs.Where(dh => dh.MAKVPO.Equals(makv))
                .OrderByDescending(dh => dh.MADHPO).ToList();
        }

        public List<DONGHOPO> GetListDASDKV(string keyword, string makvpo)
        {           
            return _db.DONGHOPOs.Where(p => p.DASD.Equals(0) && p.MAKVPO.Equals(makvpo)
                        && (p.MALDHPO.ToLower().Contains(keyword.ToLower())
                        || p.SONO.ToLower().Contains(keyword.ToLower()))).ToList();
        }

        public List<DONGHOPO> GetListKV(String madh, String maldh, String namsx, String namtt,
            DateTime? fromDate, DateTime? toDate, String trangthai, string maKV)
        {
            var query = _db.DONGHOPOs.Where(q => q.MAKVPO.Equals(maKV)).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADHPO.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDHPO.Equals(maldh)).AsQueryable();

            if (!String.IsNullOrEmpty(namsx))
                query = query.Where(dh => dh.NAMSX.Contains(namsx)).AsQueryable();

            if (!String.IsNullOrEmpty(namtt))
                query = query.Where(dh => dh.NAMTT.Contains(namtt)).AsQueryable();

            if (!String.IsNullOrEmpty(trangthai))
                query = query.Where(dh => dh.TRANGTHAI.Contains(trangthai)).AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP >= fromDate).AsQueryable();
            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP <= toDate).AsQueryable();

            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADHPO).ToList();
        }

        public List<DONGHOPO> GetList2KV(String madh, String maldh, String sono, string maKV)
        {
            var query = _db.DONGHOPOs.Where(d => d.MAKVPO.Equals(maKV)).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADHPO.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDHPO.Equals(maldh)).AsQueryable();


            if (!String.IsNullOrEmpty(sono))
                query = query.Where(dh => dh.SONO.Contains(sono)).AsQueryable();

            return query.OrderByDescending(dh => dh.MADHPO).ToList();
        }

        public List<DONGHOPO> GetList(String madh, String maldh, String namsx, String namtt, 
            DateTime? fromDate, DateTime? toDate, String trangthai)
        {
            var query = _db.DONGHOPOs.AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADHPO.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDHPO.Equals(maldh)).AsQueryable();

            if (!String.IsNullOrEmpty(namsx))
                query = query.Where(dh => dh.NAMSX.Contains(namsx)).AsQueryable();

            if (!String.IsNullOrEmpty(namtt))
                query = query.Where(dh => dh.NAMTT.Contains(namtt)).AsQueryable();

            if (!String.IsNullOrEmpty(trangthai))
                query = query.Where(dh => dh.TRANGTHAI.Contains(trangthai)).AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYNK >= fromDate).AsQueryable();

            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYNK <= toDate).AsQueryable();

            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADHPO).ToList();
        }

        public List<DONGHOPO> GetList2(String madh, String maldh, String sono, string makv)
        {
            var query = _db.DONGHOPOs.Where(d => d.MAKVPO.Equals(makv)).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADHPO.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDHPO.Equals(maldh)).AsQueryable();


            if (!String.IsNullOrEmpty(sono))
                query = query.Where(dh => dh.SONO.Contains(sono)).AsQueryable();

            
            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADHPO).ToList();
        }

        public List<DONGHOPO> GetList(string keyword)
        {
            return _db.DONGHOPOs.Where(p => (p.MADHPO.ToLower().Contains(keyword.ToLower())
                || p.MALDHPO.ToLower().Contains(keyword.ToLower())
                || p.NAMSX.ToLower().Contains(keyword.ToLower())
                || p.SONO.ToLower().Contains(keyword.ToLower())                
                )).ToList();
        }

        public List<DONGHOPO> GetListDASD(string keyword, string makv)
        {            
            return _db.DONGHOPOs.Where(p => p.MAKVPO.Equals(makv) && p.DASD.Equals(0)                        
                        && p.SONO.ToLower().Contains(keyword.ToLower())
                        ).ToList();
        }

        public List<DONGHOPO> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public List<DONGHOPO> GetListDASD(string keyword)
        {           
            return _db.DONGHOPOs.Where(p => p.DASD.Equals(0)  //&& (p.MALDH.ToLower().Contains(keyword.ToLower())
                        && p.SONO.ToLower().Contains(keyword.ToLower())).ToList();
        }

        public int Count( )
        {
            return _db.DONGHOPOs.Count();
        }

        public string NewId()
        {
            var query = (from dh in _db.DONGHOPOs
                        select dh.MADHPO).Max();            
            
            var temp = int.Parse(query) + 1;
            return temp.ToString("D6");
        }

        public string NewIdMAKV(string makv)
        {         
            var sToday = DateTime.Now.ToString("yyMM");

            var query = (from dh in _db.DONGHOPOs
                         //where dh.MADHPO.Substring(0, 7).Contains(makv + "KD" + sToday)
                         where dh.MAKVPO.Equals(makv) && dh.MADHPO.Substring(1, 2) == "KD"
                         select dh.MADHPO).Max();

            //var query = (from p in _db.HOPDONGs.Where(p => p.SOHD.Substring(0, 7).Contains(makv + "KD" + sToday)) //20150101 0001
           //              select p.SOHD).Max();

            if (!string.IsNullOrEmpty(query))
            {
                //var temp = int.Parse(query.Substring(7, 4)) + 1;
                //query = makv + "KD" + sToday + temp.ToString("D4");
                var temp = int.Parse(query.Substring(3, 8)) + 1;
                query = makv + "KD" +  temp.ToString("D8");
            }
            else
            {
                //query = makv + "KD" + sToday + "0001";
                query = makv + "KD" + sToday + "0001";
            }

            return query;
        }

        public Message Insert(DONGHOPO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DONGHOPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đồng hồ điện");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "đồng hồ điện");
            }
            return msg;
        }

        public void Insert2(DONGHOPO objUi, String useragent, String ipAddress, String sManv)
        {            
            try
            {

                _db.Connection.Open();

                _db.DONGHOPOs.InsertOnSubmit(objUi);

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADHPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "DH_A",
                    MOTA = "Nhập đồng hồ điện."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                _db.SubmitChanges();
                
            }
            catch 
            {
               
            }
           
        }

        public Message Update(DONGHOPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADHPO );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD    = objUi.DASD    ;
                    if(!string .IsNullOrEmpty( objUi .MALDHPO ))
                        objDb.LOAIDHPO = _db.LOAIDHPOs.Single(p => p.MALDHPO.Equals(objUi.MALDHPO));
                    //objDb.MALDH = objUi.MALDH ;
                    objDb.NAMSX= objUi.NAMSX ;
                    objDb.NAMTT = objUi.NAMTT;
                    objDb.NGAYNK= objUi.NGAYNK;
                    objDb.NGAYXK= objUi.NGAYXK;
                    objDb.TRANGTHAI  = objUi.TRANGTHAI ;
                    objDb.SONO = objUi.SONO;
                    objDb.SOKD = objUi.SOKD;
                    objDb.TEMKD = objUi.TEMKD;
                    objDb.HANKD = objUi.HANKD;
                    objDb.NGAYKD = objUi.NGAYKD;
                    objDb.TENCTKD = objUi.TENCTKD;
                    objDb.SXTAI = objUi.SXTAI;
                    objDb.CONGSUAT = objUi.CONGSUAT;

                    objDb.NGAYNHAP = objUi.NGAYNHAP;
                    objDb.MANVNHAP = sManv;

                    objDb.NgaySua = DateTime.Now;
                    objDb.ManvSua = sManv;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "DH_P",
                        MOTA = "Sửa thông tin đồng hồ điện."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                    
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public Message UpdateDASD(DONGHOPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADHPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = true;
                    

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public Message UpdateDASDDH(String madh)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(madh);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = true;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public Message UpdateKoSD(DONGHOPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADHPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = false;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if( _db.THICONGs  .Where(p => p.MADH .Equals(ma)).Count() > 0)
                return true;
            if (_db.KDDHCTs .Where(p => p.MADH.Equals(ma)).Count() > 0)
                return true;
            return false;
        }

        public Message Delete(DONGHOPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADHPO);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đồng hồ ", objUi.MADHPO);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DONGHOPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đồng hồ ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đồng hồ ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<DONGHOPO> objList)
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
                    Delete(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "danh sách đồng hồ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách đồng hồ");
            }

            return msg;
        }
    }
}
