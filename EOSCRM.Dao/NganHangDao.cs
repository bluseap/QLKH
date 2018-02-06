using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class NganHangDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public NganHangDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public NGANHANG    Get(string ma)
        {
            return _db.NGANHANGs.Where(p => p.MANH .Equals(ma)).SingleOrDefault();
        }

        public List<NGANHANG> Search(string key)
        {
            return
                _db.NGANHANGs.Where(
                    p => p.TENNH.ToUpper().Contains(key.ToUpper()) || p.STKCTY.ToUpper().Contains(key.ToUpper())).ToList
                    ();
        }

        public List<NGANHANG> GetList()
        {
            return _db.NGANHANGs.ToList();
        }

        public List<NGANHANG> GetList(string keyword)
        {
            return _db.NGANHANGs.ToList().Where(n => (n.MANH.Contains(keyword) ||
                                                        n.TENNH.Contains(keyword) ||
                                                        n.TINHTP.Contains(keyword) ||
                                                        n.STKCTY.Contains(keyword) ||
                                                        n.GHICHU.Contains(keyword))).ToList();
        }

        public List<NGANHANG> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.NGANHANGs.Count();
        }

        public Message Insert(NGANHANG objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.NGANHANGs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "ngân hàng");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "ngân hàng", objUi.TENNH);
            }
            return msg;
        }

        public Message Update(NGANHANG objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MANH      );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DIACHI  = objUi.DIACHI   ;
                    objDb.GHICHU = objUi.GHICHU  ;
                    objDb.STKCTY = objUi.STKCTY;
                    objDb.TENNH = objUi.TENNH;
                    objDb.TINHTP = objUi.TINHTP;
                   
                        // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "ngân hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "ngân hàng", objUi.TENNH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "ngân hàng", objUi.TENNH);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.DANHSACHCOQUANTTs.Where(p => p.MANH.Equals(ma)).Count() > 0;
        }

        public Message Delete(NGANHANG  objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MANH    );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Ngân hàng", objUi.TENNH     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.NGANHANGs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Ngân hàng ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Ngân hàng ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<NGANHANG> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách ngân hàng");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "ngân hàng", failed, "ngân hàng");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " ngân hàng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách ngân hàng");
            }

            return msg;
        }
    }
}
