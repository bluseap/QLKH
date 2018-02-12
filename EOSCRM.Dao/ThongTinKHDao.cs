using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ThongTinKHDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public ThongTinKHDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THONGTINKH Get(string ma)
        {
            return _db.THONGTINKHs.Where(p => p.IDTTKH.Equals(ma)).SingleOrDefault();
        }

        public List<THONGTINKH> GetList()
        {
            return _db.THONGTINKHs.OrderByDescending(p => p.NGAYNHAP).ToList();
        }

        public List<THONGTINKH> GetListKV(string makv)
        {
            return _db.THONGTINKHs.Where(p => p.MAKV.Equals(makv)).OrderByDescending(p => p.NGAYNHAP).ToList();
        }

        public List<THONGTINKH> GetListDBTEN(string sodb, string ten, string makv)
        {
            var query = _db.THONGTINKHs.Where(p => p.MAKV.Equals(makv)).AsQueryable();

            if (!string.IsNullOrEmpty(sodb))
                query = query.Where(d => d.SODB.Contains(sodb));

            if (!string.IsNullOrEmpty(ten))
                query = query.Where(d => d.TENKH.Contains(ten));

            return query.OrderByDescending(p => p.NGAYNHAP).ToList();
        }
        
        public int Count( )
        {
            return _db.THONGTINKHs.Count();
        }

        public void Insert(THONGTINKH objUi, String useragent, String ipAddress, String sManv)
        {
            try
            {
                _db.Connection.Open();
                _db.THONGTINKHs.InsertOnSubmit(objUi);

                //objUi.IDKH

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDTTKH.ToString(),
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "TTKH",
                    MOTA = "Thông tin khách hàng."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();
            }
            catch { }
        }

        public Message Update(THONGTINKH objUi, String useragent, String ipAddress, String sManv, string idttkh)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(idttkh);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.SODB = objUi.SODB;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHI = objUi.DIACHI;
                    objDb.MALOAI = objUi.MALOAI;
                    objDb.NGAYNHAN = objUi.NGAYNHAN;
                    objDb.GHICHU = objUi.GHICHU;                   

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = idttkh,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "TTKHUP",
                        MOTA = "Sửa thông tin khách hàng nước."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thông tin kh");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "thông tin kh");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thông tin kh");
            }
            return msg;
        }

        /*
        public bool IsInUse(string ma)
        {
            return _db.GIAIQUYETTHONGTINSUACHUAs .Where(p => p.MAPH .Equals(ma)).Count() > 0;
        }

        public Message Delete(THONGTINPHANHOI objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAPH    );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thông tin phản hồi", objUi.TENPH     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.THONGTINPHANHOIs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thông tin phản hồi ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thông tin phản hồi ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<THONGTINPHANHOI> objList)
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
                    var temp = Delete(obj);
                    if (temp != null && temp.MsgType.Equals(MessageType.Error))
                        failed++;
                }

                // commit
                trans.Commit();

                if (failed > 0)
                {
                    if (failed == objList.Count)
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách phản hồi");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "phản hồi", failed, "phản hồi");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " phản hồi");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách phản hồi");
            }

            return msg;
        }
        */



    }
}
