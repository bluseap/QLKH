using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Configuration;

namespace EOSCRM.Dao
{
    public class PhuongPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public PhuongPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public PHUONGPO Get(string ma)
        {
            return _db.PHUONGPOs.Where(p => p.MAPHUONGPO.Equals(ma)).SingleOrDefault();
        }

        public PHUONGPO GetMAKV(string ma, string kv)
        {
            return _db.PHUONGPOs.Where(p => p.MAPHUONGPO.Equals(ma) && p.MAKVPO.Equals(kv)).SingleOrDefault();
        }

        public PHUONGPO GetByName(string tenPhuong, string makv)
        {
            return _db.PHUONGPOs.Where(p => p.TENPHUONG.Equals(tenPhuong) && p.MAKVPO .Equals( makv)).SingleOrDefault();
        }
        
        public List<PHUONGPO> Search(string key)
        {
            return _db.PHUONGPOs.Where(p => p.TENPHUONG .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<PHUONGPO> GetList()
        {
            return _db.PHUONGPOs.OrderBy(p => p.MAKVPO).ToList();
        }

        public List<PHUONGPO> GetListKVTS(string maKV, string keyword)
        {
            var list = _db.PHUONGPOs.Where(p => p.MAKVPO.Equals(maKV) && p.TENPHUONG.Contains(keyword)).ToList();

            if (keyword != "")
            {
                return list;
            }

            return list;
        }

        public List<PHUONGPO> GetListKV(string makv)
        {
            return _db.PHUONGPOs.Where(p => p.MAKVPO.Equals(makv)).OrderBy(p => p.MAKVPO).ToList();
        }

        public List<PHUONGPO> GetList(string makv)
        {
            if (string.IsNullOrEmpty(makv) || makv == "%")
                return _db.PHUONGPOs.OrderBy(p => p.MAPHUONGPO).OrderBy(p => p.MAKVPO).ToList();

            return _db.PHUONGPOs.Where(p => p.MAKVPO.Equals(makv)).ToList();
        } 

        public List<PHUONGPO> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.PHUONGPOs.Count();
        }

        public Message Insert(PHUONGPO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.PHUONGPOs.InsertOnSubmit(objUi);
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

        public Message Update(PHUONGPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAPHUONGPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENPHUONG  = objUi.TENPHUONG  ;
                    objDb.KHUVUCPO = _db.KHUVUCPOs.Single(p => p.MAKVPO.Equals(objUi.MAKVPO));
                        // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Phường ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Phường ", objUi.TENPHUONG     );
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
            if (_db.DONDANGKYPOs.Where(p => p.MAPHUONG .Equals(ma)).Count() > 0)
                return true;
/*
            else if (_db.KHACHHANGs .Where(p => p.MAPHUONG .Equals(ma)).Count() > 0)
                return true;
            else if (_db.HOPDONGs  .Where(p => p.MAPHUONG .Equals(ma)).Count() > 0)
                return true;
                */
            else
            {
                return false;
            }
        }

        public Message Delete(PHUONGPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAPHUONGPO    );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Phường", objUi.TENPHUONG     );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.PHUONGPOs.DeleteOnSubmit(objDb);
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

        public IList< KHUVUCPO > GetListKhuVuc()
        {
            return _db.KHUVUCPOs.OrderBy(p => p.ORDERS).ToList();
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<PHUONGPO> objList, PageAction action)
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

        public Message DeleteList(List<PHUONGPO> objList)
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
