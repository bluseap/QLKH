using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ThongTinXuLyDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ThongTinXuLyDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THONGTINXULY Get(string ma)
        {
            return _db.THONGTINXULies.Where(p => p.MAXL .Equals(ma)).SingleOrDefault();
        }

        public List<THONGTINXULY> Search(string key)
        {
            return _db.THONGTINXULies.Where(p => p.TENXL .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<THONGTINXULY> GetList()
        {
            return _db.THONGTINXULies.OrderBy(p => p.STT).ToList();
        }

        public List<THONGTINXULY> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.THONGTINXULies.Count();
        }

        public Message Insert(THONGTINXULY objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.THONGTINXULies.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "thông tin xử lý");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "thông tin xử lý");
            }
            return msg;
        }

        public Message Update(THONGTINXULY objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAXL     );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.HIENTHI  = objUi.HIENTHI   ;
                    objDb.ISLAPCHIETTINH = objUi.ISLAPCHIETTINH   ;
                    objDb.STT = objUi.STT;
                    objDb.TENXL = objUi.TENXL;
                    
                        // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thông tin xử lý");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "thông tin xử lý");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thông tin xử lý");
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.GIAIQUYETTHONGTINSUACHUAs .Where(p => p.MAPH .Equals(ma)).Count() > 0;
        }

        public Message Delete(THONGTINXULY objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAXL );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thông tin xử lý", objUi.TENXL      );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.THONGTINXULies.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thông tin xử lý ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thông tin xử lý ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<THONGTINXULY> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách thông tin xử lý");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "thông tin xử lý", failed, "thông tin xử lý");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " thông tin xử lý");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách thông tin xử lý");
            }

            return msg;
        }
    }
}
