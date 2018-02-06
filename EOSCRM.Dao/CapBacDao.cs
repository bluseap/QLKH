using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public  class CapBacDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public CapBacDao ()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CAPBAC Get(string macb)
        {
            return _db.CAPBACs.Where(p => p.MACB.Equals(macb)).SingleOrDefault();
        }

        public List<CAPBAC> Search(string key)
        {
            return _db.CAPBACs.Where(p => p.TENCB.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<CAPBAC> GetList()
        {
            return _db.CAPBACs.OrderBy(c => c.TENCB).ToList();
        }

        public List<CAPBAC> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.CAPBACs.Count();
        }

        public Message Insert(CAPBAC objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CAPBACs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                _db.Connection.Close();
                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "cấp bậc");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "cấp bậc", objUi.TENCB);
            }
            return msg;
        }

        public Message Update(CAPBAC objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MACB);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENCB = objUi.TENCB;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "cấp bậc");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "cấp bậc", objUi.TENCB );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "cấp bậc");
            }
            return msg;
        }

        public bool IsInUse(string macb)
        {
            return _db.NHANVIENs.Where(p => p.MACB.Equals(macb)).Count() > 0;
        }

        public Message Delete(CAPBAC objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MACB );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Cấp bậc", objUi.TENCB);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CAPBACs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "cấp bậc");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Cấp bậc");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<CAPBAC> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách cấp bậc");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "cấp bậc", failed, "cấp bậc");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " cấp bậc");
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