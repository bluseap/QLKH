using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class MucDichSuDungPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public MucDichSuDungPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public MDSDPO Get(string ma)
        {
            return _db.MDSDPOs.Where(p => p.MAMDSDPO.Equals(ma)).SingleOrDefault();
        }

        public List<MDSDPO> Search(string key)
        {
            return _db.MDSDPOs.Where(p => p.TENMDSD .ToUpper().Contains(key.ToUpper())  || p.MOTAMDSD .ToUpper( ).Contains( key .ToUpper( ))).ToList();
        }

        public List<MDSDPO> GetList()
        {
            return _db.MDSDPOs .OrderBy(p=>p.MAMDSDPO) .ToList();
        }

        public List<MDSDPO> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.MDSDPOs.Count();
        }

        public Message Insert(MDSDPO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.MDSDPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Mục đích sử dụng ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Mục đích sử dụng ", objUi.TENMDSD  );
            }
            return msg;
        }

        public Message Update(MDSDPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAMDSDPO );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENMDSD   = objUi.TENMDSD   ;
                    objDb.MOTAMDSD = objUi.MOTAMDSD;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Mục đích sử dụng ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Mục đích sử dụng ", objUi.TENMDSD  );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Mục đích sử dụng ", objUi.TENMDSD);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.KHACHHANGs.Where(p => p.MAMDSD.Equals(ma)).Count() > 0)
                return true;
            else if (_db.TIEUTHUs.Where((p => p.MAMDSD.Equals(ma))).Count() > 0)
                return true;
            else if (_db.HOPDONGs.Where(p => p.MAMDSD.Equals(ma)).Count() > 0)
                return true;
            
            else if (_db.DONDANGKYPOs.Where(p => p.MAMDSDPO.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(MDSDPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAMDSDPO  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Mục đích sử dụng ", objUi.TENMDSD  );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.MDSDPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Mục đích sử dụng ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Mục đích sử dụng ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<MDSDPO> objList, PageAction action)
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
                    //TODO: check valid update infor
                    Delete(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Mục đích sử dụng ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách mục đích sử dụng ");
            }

            return msg;
        }
    }
}
