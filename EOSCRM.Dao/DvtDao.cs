using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class DvtDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DvtDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DVT Get(string ma)
        {
            return _db.DVTs .Where(p => p.DVT1.Equals(ma)).SingleOrDefault();
        }

        public List<DVT> Search(string key)
        {
            return _db.DVTs.Where(p => p.TENDVT .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<DVT> GetList()
        {
            return _db.DVTs.ToList();
        }

        public List<DVT> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.DVTs.Count();
        }

        public Message Insert(DVT objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DVTs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn vị tính");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "đơn vị tính");
            }
            return msg;
        }

        public Message Update(DVT objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.DVT1);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENDVT = objUi.TENDVT;
                 
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đơn vị tính");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đơn vị tính", objUi.TENDVT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đơn vị tính");
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.VATTUs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAP_ND117s.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPMAUBOCVATTUs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPQUYETTOANs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPQUYETTOAN_ND117s.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPSUACHUA_ND117s.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPSUACHUAQTs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPSUACHUAs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            if (_db.DAOLAPTKs.Where(p => p.DVT.Equals(ma)).Count() > 0)
                return true;

            return false;
        }

        public Message Delete(DVT objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.DVT1);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn vị tính ", objUi.TENDVT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DVTs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đơn vị tính ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đơn vị tính ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<DVT> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách đơn vị tính");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "đơn vị tính", failed, "đơn vị tính");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " đơn vị tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách đơn vị tính");
            }

            return msg;
        }
    }
}
