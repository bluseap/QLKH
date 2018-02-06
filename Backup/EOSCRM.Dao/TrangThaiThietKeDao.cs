using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class TrangThaiThietKeDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public TrangThaiThietKeDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public TRANGTHAITHIETKE    Get(string ma)
        {
            return _db.TRANGTHAITHIETKEs.Where(p => p.MATT .Equals(ma)).SingleOrDefault();
        }

        public List<TRANGTHAITHIETKE> Search(string key)
        {
            return _db.TRANGTHAITHIETKEs.Where(p => p.TENTT .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<TRANGTHAITHIETKE> GetList()
        {
            return _db.TRANGTHAITHIETKEs.ToList();
        }

        public List<TRANGTHAITHIETKE> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.TRANGTHAITHIETKEs.Count();
        }

        public Message Insert(TRANGTHAITHIETKE objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.TRANGTHAITHIETKEs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Trạng thái thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Trạng thái thiết kế ", objUi.TENTT     );
            }
            return msg;
        }

        public Message Update(TRANGTHAITHIETKE objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MATT   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENTT     = objUi.TENTT     ;
                    objDb.ORDERS = objUi.ORDERS   ;
                    objDb.COLOR = objUi.COLOR  ;
                    
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Trạng thái thiết kế ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Trạng thái thiết kế ", objUi.MATT     );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Trạng thái thiết kế ", objUi.TENTT     );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.DONDANGKies.Where(p => p.TTCT.Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies  .Where(p => p.TTDK .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies.Where(p => p.TTHC.Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies.Where(p => p.TTHD.Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies.Where(p => p.TTTC.Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies.Where(p => p.TTTK.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(TRANGTHAITHIETKE objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MATT  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Trạng thái thiết kế ", objUi.TENTT );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.TRANGTHAITHIETKEs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Trạng thái thiết kế ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Trạng thái thiết kế ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<TRANGTHAITHIETKE> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Trạng thái thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách trạng thái thiết kế ");
            }

            return msg;
        }
    }
}
