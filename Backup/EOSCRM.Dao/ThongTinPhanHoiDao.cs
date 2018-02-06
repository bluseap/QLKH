using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ThongTinPhanHoiDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ThongTinPhanHoiDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THONGTINPHANHOI   Get(string ma)
        {
            return _db.THONGTINPHANHOIs.Where(p => p.MAPH .Equals(ma)).SingleOrDefault();
        }

        public List<THONGTINPHANHOI> Search(string key)
        {
            return _db.THONGTINPHANHOIs.Where(p => p.TENPH.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<THONGTINPHANHOI> GetList()
        {
            return _db.THONGTINPHANHOIs.ToList();
        }

        public List<THONGTINPHANHOI> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.THONGTINPHANHOIs.Count();
        }

        public Message Insert(THONGTINPHANHOI objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.THONGTINPHANHOIs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "thông tin phản hồi");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "thông tin phản hồi");
            }
            return msg;
        }

        public Message Update(THONGTINPHANHOI objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAPH       );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.HIENTHI  = objUi.HIENTHI   ;
                    if (!string.IsNullOrEmpty(objUi.MALOAI))
                        objDb.LOAIPHANHOI = _db.LOAIPHANHOIs.Single(p => p.MALOAI.Equals(objUi.MALOAI));
                    //objDb.MALOAI = objUi.MALOAI  ;
                    if (!string.IsNullOrEmpty(objUi.MANHOM))
                        objDb.NHOMPHANHOI = _db.NHOMPHANHOIs.Single(p => p.MANHOM.Equals(objUi.MANHOM));
                    //objDb.MANHOM = objUi.MANHOM;
                    objDb.STT = objUi.STT;
                    objDb.TENPH = objUi.TENPH;
                    
                        // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thông tin phản hồi");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "thông tin phản hồi");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thông tin phản hồi");
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.GIAIQUYETTHONGTINSUACHUAs .Where(p => p.MAPH .Equals(ma)).Count() > 0;
        }

        public Message Delete(THONGTINPHANHOI objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAPH    );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thông tin phản hồi", objUi.TENPH     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.THONGTINPHANHOIs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thông tin phản hồi ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thông tin phản hồi ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<THONGTINPHANHOI> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách phản hồi");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "phản hồi", failed, "phản hồi");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " phản hồi");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách phản hồi");
            }

            return msg;
        }

    }
}
