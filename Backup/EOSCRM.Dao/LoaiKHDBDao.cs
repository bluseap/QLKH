using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class LoaiKhDacBietDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public LoaiKhDacBietDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public LOAIKHDB Get(string ma)
        {
            return _db.LOAIKHDBs.Where(p => p.MALKHDB .Equals(ma)).SingleOrDefault();
        }

        public List<LOAIKHDB> Search(string key)
        {
            return _db.LOAIKHDBs.Where(p => p.TENLKHDB .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<LOAIKHDB> GetList()
        {
            return _db.LOAIKHDBs.ToList();
        }

        public List<LOAIKHDB> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.LOAIKHDBs.Count();
        }

        public Message Insert(LOAIKHDB objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.LOAIKHDBs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Loại khách hàng đặc biệt ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Loại khách hàng đặc biệt ", objUi.TENLKHDB  );
            }
            return msg;
        }

        public Message Update(LOAIKHDB objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MALKHDB   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENLKHDB = objUi.TENLKHDB;
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Loại khách hàng đặt biệt ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Loại khách hàng đặt biệt ", objUi.TENLKHDB    );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Loại khách hàng đặt biệt ", objUi.TENLKHDB );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.KHACHHANGs.Where(p => p.MALKHDB.Equals(ma)).Count() > 0;
        }

        public Message Delete(LOAIKHDB objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MALKHDB );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Loại khách hàng đặc biệt", objUi.TENLKHDB       );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.LOAIKHDBs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Loại khách hàng đặc biệt ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Loại khách hàng đặc biệt ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<LOAIKHDB> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Loại khách hàng đặc biệt ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách loại khách hàng đặc biệt ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<LOAIKHDB> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách loại KHDB");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "loại KHDB", failed, "loại KHDB");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " loại KHDB");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách loại KHDB");
            }

            return msg;
        }
    }
}
