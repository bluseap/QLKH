using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Configuration;


namespace EOSCRM.Dao
{
    public  class PhuongDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public PhuongDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public PHUONG   Get(string ma)
        {
            return _db.PHUONGs.Where(p => p.MAPHUONG .Equals(ma)).SingleOrDefault();
        }

        public PHUONG GetMAKV(string ma, string kv)
        {
            return _db.PHUONGs.Where(p => p.MAPHUONG.Equals(ma) && p.MAKV.Equals(kv)).SingleOrDefault();
        }

        public PHUONG GetByName(string tenPhuong, string makv)
        {
            return _db.PHUONGs.Where(p => p.TENPHUONG.Equals(tenPhuong) && p.MAKV .Equals( makv)).SingleOrDefault();
        }
        
        public List<PHUONG> Search(string key)
        {
            return _db.PHUONGs.Where(p => p.TENPHUONG .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<PHUONG> GetList()
        {
            return _db.PHUONGs.OrderBy(p => p.MAKV).ToList();
        }

        public List<PHUONG> GetListKV(string makv)
        {
            return _db.PHUONGs.Where(p => p.MAKV.Equals(makv)).OrderBy(p => p.MAKV).ToList();
        }

        public List<PHUONG> GetListKVTS(string maKV, string keyword)
        {
            var list = _db.PHUONGs.Where(p => p.MAKV.Equals(maKV) && p.TENPHUONG.Contains(keyword)).ToList();            

            if (keyword != "")
            {                                       
                return list;
            }

            return list;
        }

        public List<PHUONG> GetList(string makv)
        {
            if (string.IsNullOrEmpty(makv) || makv == "%")
                return _db.PHUONGs.OrderBy(p => p.MAPHUONG).OrderBy(p => p.MAKV).ToList();

            return _db.PHUONGs.Where(p => p.MAKV.Equals(makv)).ToList();
        }

        public List<PHUONG> GetListKVM(string makv)
        {
            //if (string.IsNullOrEmpty(makv) || makv == "%")
            //    return _db.PHUONGs.OrderBy(p => p.MAPHUONG).OrderBy(p => p.MAKV).ToList();

            return _db.PHUONGs.Where(p => p.MAKV.Equals(makv)).ToList();
        }


        public List<PHUONG> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.PHUONGs.Count();
        }

        public Message Insert(PHUONG objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.PHUONGs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Phường ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Phường ", objUi.TENPHUONG    );
            }
            return msg;
        }

        public Message Update(PHUONG objUi, string makv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetMAKV(objUi.MAPHUONG, makv);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENPHUONG  = objUi.TENPHUONG  ;
                   // objDb.KHUVUC = _db.KHUVUCs.Single(p => p.MAKV.Equals(objUi.MAKV));
                    objDb.MAKV = objUi.MAKV;
                        // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Phường ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Phường ", objUi.TENPHUONG);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Phường ", objUi.TENPHUONG   );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.DONDANGKies.Where(p => p.MAPHUONG .Equals(ma)).Count() > 0)
                return true;
            else if (_db.KHACHHANGs .Where(p => p.MAPHUONG .Equals(ma)).Count() > 0)
                return true;
            else if (_db.HOPDONGs  .Where(p => p.MAPHUONG .Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(PHUONG objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAPHUONG    );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Phường", objUi.TENPHUONG     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.PHUONGs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Phường ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Phường ");
            }

            return msg;
        }

        public IList< KHUVUC > GetListKhuVuc()
        {
            return _db.KHUVUCs.OrderBy(p => p.ORDERS).ToList();
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<PHUONG> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Phường ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách phường ");
            }

            return msg;
        }

        public Message DeleteList(List<PHUONG> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách phường");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "phường", failed, "phường");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " phường");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách phường");
            }

            return msg;
        }
    }
}
