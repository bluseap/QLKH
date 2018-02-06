using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class CongViecDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public CongViecDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public CONGVIEC Get(string ma)
        {
            return _db.CONGVIECs.Where(p => p.MACV.Equals(ma)).SingleOrDefault();
        }

        public List<CONGVIEC> Search(string key)
        {
            return _db.CONGVIECs.Where(p => p.TENCV.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<CONGVIEC> GetList()
        {
            return _db.CONGVIECs.OrderBy(cv => cv.TENCV).ToList();
        }

        public List<CONGVIEC> GetList(string macv, string tencv)
        {
            var query = _db.CONGVIECs.AsQueryable();

            if (!string.IsNullOrEmpty(macv))
                query = query.Where(cv => cv.MACV.Contains(macv)).AsQueryable();

            if (!string.IsNullOrEmpty(tencv))
                query = query.Where(cv => cv.TENCV.Contains(tencv)).AsQueryable();

            return query.OrderBy(cv => cv.TENCV).ToList();
        }

        public int Count()
        {
            return _db.CONGVIECs.Count();
        }

        public Message Insert(CONGVIEC objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CONGVIECs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "công việc");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "công việc", objUi.TENCV);
            }
            return msg;
        }

        public Message Update(CONGVIEC objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MACV);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENCV = objUi.TENCV;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "công việc");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "công việc", objUi.TENCV);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "công việc", objUi.TENCV);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.NHANVIENs.Where(p => p.MACV.Equals(ma)).Count() > 0;
        }

        public Message Delete(CONGVIEC objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MACV);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Công việc ", objUi.TENCV);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CONGVIECs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Công việc ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Công việc ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<CONGVIEC> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách công việc");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "công việc", failed, "công việc");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " công việc");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách công việc");
            }

            return msg;
        }
    }
}

