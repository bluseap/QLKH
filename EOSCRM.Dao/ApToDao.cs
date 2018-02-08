using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ApToDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ApToDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public APTO Get(string maapto, string maxa, string makv)
        {
            return _db.APTOs.Where(p => p.MAAPTO.Equals(maapto) && p.MAXA.Equals(maxa) && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public APTO GetAp(string maapto, string makv)
        {
            return _db.APTOs.Where(p => p.MAAPTO.Equals(maapto) && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public List<APTO> Search(string key)
        {
            return _db.APTOs.Where(p => p.TENAPTO.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<APTO> GetList()
        {
            return _db.APTOs.OrderBy(c => c.STT).ToList();
        }

        public List<APTO> GetList(string makv)
        {
            return _db.APTOs.Where(p => p.MAKV.Equals(makv)).OrderBy(c => c.STT).ToList();
        }

        public List<APTO> GetList(string makv, string maxa)
        {
            var query = _db.APTOs.Where(p => p.MAKV.Equals(makv) && p.STT != 999).AsEnumerable();
            //var query = _db.APTOs.Where(p => p.MAKV.Equals(makv) && p.MAXA.Equals(maxa) && p.STT != 999).AsEnumerable();

            if (maxa != "%")
            {
                query = query.Where(p => p.MAXA.Equals(maxa));
            }
            //else
            //{
            //    query = query.Where(p => p.MAXA.Equals(maxa));
            //}

            return query.OrderBy(c => c.STT).ToList();
            //return _db.APTOs.Where(p => p.MAKV.Equals(makv) && p.MAXA.Equals(maxa) && p.STT != 999).OrderBy(c => c.STT).ToList();
        }

        public List<APTO> GetListAp(string makv, string map)
        {
            return _db.APTOs.Where(p => p.MAKV.Equals(makv) && p.MAAPTO.Equals(map) && p.STT != 999).OrderBy(c => c.STT).ToList();
        }

        public int Count( )
        {
            return _db.APTOs.Count();
        }

        public Message Insert(APTO objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.APTOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAAPTO + objUi.MAXA + objUi.MAKV,
                    IPAddress = "192.168.1.19",
                    MANV = "nguyenm",
                    UserAgent = "192.168.1.119",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "INAXP",
                    MOTA = "Thêm Ấp khóm"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Ấp, khóm ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "Ấp, khóm ", objUi.TENAPTO);
            }
            return msg;
        }

        public Message Update(APTO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAAPTO, objUi.MAXA, objUi.MAKV);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENAPTO = objUi.TENAPTO;
                    objDb.STT = objUi.STT;
                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAAPTO + objUi.MAXA + objUi.MAKV,
                        IPAddress = "192.168.1.19",
                        MANV = "nguyenm",
                        UserAgent = "192.168.1.119",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "UPAXP",
                        MOTA = "Cập nhật Ấp khóm"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Ấp, khóm ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Ấp, khóm ", objUi.TENAPTO);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Ấp, khóm ", objUi.TENAPTO);
            }
            return msg;
        }

    }
}
