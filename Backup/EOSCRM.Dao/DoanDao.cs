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
    public  class DoanDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DoanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DOAN Get(string madoan)
        {
            return _db.DOANs.SingleOrDefault(p => p.MADOAN.Equals(madoan));
        }

        public DOAN Get(string madoan, string makv)
        {
            return _db.DOANs.SingleOrDefault(p => p.MADOAN.Equals(madoan) && p.MAKV.Equals(makv));
        }

        public List<DOAN> GetList()
        {
            return _db.DOANs.OrderBy(c => c.MADOAN).OrderBy(c => c.MAKV).ToList();
        }

        public List<DOAN> GetList(String madoan, String mathehien, String makv)
        {
            var query = _db.DOANs.AsQueryable();

            if (!String.IsNullOrEmpty(madoan))
                query = query.Where(dp => dp.MADOAN.ToUpper().Contains(madoan.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(mathehien))
                query = query.Where(dp => dp.MATHEHIEN.ToUpper().Contains(mathehien.ToUpper())).AsQueryable();

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(dp => dp.MAKV.ToUpper().Equals(makv.ToUpper())).AsQueryable();

            return query.OrderBy(c => c.MADOAN).OrderBy(c => c.MAKV).ToList();
        }

        public Message Insert(DOAN objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.DOANs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                if(_db.Connection.State == ConnectionState.Open)
                    _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đoạn");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (_db.Connection.State == ConnectionState.Open)
                    _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "đoạn", objUi.MADOAN);
            }

            return msg;
        }

        public Message Update(DOAN objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADOAN);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADOAN = objUi.MADOAN;
                    objDb.MAKV = objUi.MAKV;
                    objDb.MATHEHIEN = objUi.MATHEHIEN;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đoạn");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đoạn", objUi.MADOAN);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đoạn");
            }
            return msg;
        }

        public bool IsInUse(string madoan)
        {
            return _db.KHACHHANGs.Count(p => p.MADOAN.Equals(madoan)) > 0;
        }

        public Message Delete(DOAN objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADOAN );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đoạn", objUi.MADOAN);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use
                if (IsInUse(objDb.MADOAN))
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đoạn", objUi.MADOAN);
                    return msg;
                }

                // Set delete info
                _db.DOANs.DeleteOnSubmit(objDb);

                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "đoạn");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đoạn");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<DOAN> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách đoạn");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "đoạn", failed, "đoạn");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " đoạn");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách cấp bậc");
            }

            return msg;
        }

    }
}