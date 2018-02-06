using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class XaPhuongDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public XaPhuongDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public XAPHUONG Get(string maxa, string makv)
        {
            return _db.XAPHUONGs.Where(p => p.MAXA.Equals(maxa) && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public List<XAPHUONG> Search(string key)
        {
            return _db.XAPHUONGs.Where(p => p.TENXA.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<XAPHUONG> GetList()
        {
            return _db.XAPHUONGs.OrderBy(c => c.MAXA).ToList();
        }

        public List<XAPHUONG> GetListKV(string makv)
        {
            return _db.XAPHUONGs.Where(p => p.MAKV.Equals(makv) && p.STT < 999).OrderBy(c => c.STT).ToList();
        }

        public List<XAPHUONG> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.XAPHUONGs.Count();
        }

        public Message Insert(XAPHUONG objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.XAPHUONGs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAXA + objUi.MAKV,
                    IPAddress = "192.168.1.19",
                    MANV = "nguyenm",
                    UserAgent = "192.168.1.119",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "INXP",
                    MOTA = "Thêm Xã phường"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Xã, phường ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "Xã, phường ", objUi.TENXA);
            }
            return msg;
        }

        public Message Update(XAPHUONG objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAXA, objUi.MAKV);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENXA = objUi.TENXA;
                    objDb.STT = objUi.STT;
                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAXA + objUi.MAKV,
                        IPAddress = "192.168.1.19",
                        MANV = "nguyenm",
                        UserAgent = "192.168.1.119",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "UPXP",
                        MOTA = "Cập nhật Xã phường"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Xã, phường ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Xã, phường ", objUi.TENXA);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Xã, phường ", objUi.TENXA);
            }
            return msg;
        }       


    }
}
