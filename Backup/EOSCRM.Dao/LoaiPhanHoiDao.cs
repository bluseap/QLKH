using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class LoaiPhanHoiDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public LoaiPhanHoiDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public LOAIPHANHOI Get(string ma)
        {
            return _db.LOAIPHANHOIs.Where(p => p.MALOAI .Equals(ma)).SingleOrDefault();
        }

        public List<LOAIPHANHOI> Search(string key)
        {
            return _db.LOAIPHANHOIs.Where(p => p.TENLOAI .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<LOAIPHANHOI> GetList()
        {
            return _db.LOAIPHANHOIs.ToList();
        }

        public List<LOAIPHANHOI> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.LOAIPHANHOIs.Count();
        }

        public Message Insert(LOAIPHANHOI objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.LOAIPHANHOIs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "loại phản hồi");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "loại phản hồi");
            }
            return msg;
        }

        public Message Update(LOAIPHANHOI objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MALOAI   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENLOAI     =  objUi.TENLOAI   ;
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "loại phản hồi");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "loại phản hồi");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "loại phản hồi");
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.THONGTINPHANHOIs  .Where(p => p.MALOAI  .Equals(ma)).Count() > 0;
        }

        public Message Delete(LOAIPHANHOI objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MALOAI     );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Loại phản hồi ", objUi.MALOAI     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.LOAIPHANHOIs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Mã loại ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Mã loại ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<LOAIPHANHOI> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách loại phản hồi");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "loại phản hồi", failed, "loại phản hồi");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " loại phản hồi");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách loại phản hồi");
            }

            return msg;
        }
    }
}
