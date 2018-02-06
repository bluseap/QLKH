using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class PhongBanDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public PhongBanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public PHONGBAN  Get(string ma)
        {
            return _db.PHONGBANs.Where(p => p.MAPB .Equals(ma)).SingleOrDefault();
        }

        public List<PHONGBAN> Search(string key)
        {
            return _db.PHONGBANs.Where(p => p.MAPB.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<PHONGBAN> GetList()
        {
            return _db.PHONGBANs.ToList();
        }

        public List<PHONGBAN> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.PHONGBANs.Count();
        }

        public Message Insert(PHONGBAN objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.PHONGBANs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Phòng ban ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Phòng ban ", objUi.TENPB  );
            }
            return msg;
        }

        public Message Update(PHONGBAN objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAPB    );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DIACHI = objUi.DIACHI ;
                    objDb.MOTA = objUi.MOTA ;
                    objDb.ORDERS  = objUi.ORDERS;
                    objDb.PHONGBAN1 = _db.PHONGBANs.Single(p => p.MAPB.Equals(objUi.TRUCTHUOC));
                    objDb.SDT = objUi.SDT;
                    objDb.TENPB = objUi.TENPB;
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Phòng ban ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Phòng ban ", objUi.TENPB    );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Phòng ban ", objUi.TENPB  );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.DONDANGKies.Where(p => p.MAPB.Equals(ma)).Count() > 0)
                return true;
            else if (_db.NHANVIENs.Where(p => p.MAPB.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(PHONGBAN objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAPB  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Phòng ban", objUi.MAPB     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.PHONGBANs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Phòng ban ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Phòng ban ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<PHONGBAN> objList)
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

                if(failed > 0)
                {
                    if (failed == objList.Count)
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách phòng ban");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "phòng ban", failed, "phòng ban");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " phòng ban");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách phòng ban");
            }

            return msg;
        }
    }
}
