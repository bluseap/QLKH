using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class KhuVucPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KhuVucPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHUVUCPO Get(string ma)
        {
            return _db.KHUVUCPOs.SingleOrDefault(p => p.MAKVPO.Equals(ma));
        }

        public KHUVUCPO GetPo(string ma)
        {
            return _db.KHUVUCPOs.SingleOrDefault(p => p.MAKV.Equals(ma));
        } 

        public List<KHUVUCPO> GetList()
        {
            return _db.KHUVUCPOs.Where(p=>p.DACBIET.Equals(true)).OrderBy(p=>p.ORDERS  ).ToList();
        }

        public List<KHUVUCPO> GetListPo(string makv)
        {
            return _db.KHUVUCPOs.Where(p => p.MAKV.Equals(makv) && p.DACBIET.Equals(true)).OrderBy(p => p.ORDERS).ToList();
        }

        public List<KHUVUCPO> GetListKV(string makv)
        {
            return _db.KHUVUCPOs.Where(p => p.DACBIET.Equals(true) && p.MAKVPO==makv).ToList();
        }

        public List<KHUVUCPO> GetListKVPO(string makv)
        {
            return _db.KHUVUCPOs.Where(p => p.DACBIET.Equals(true) && p.MAKV == makv).ToList();
        }

        public Message Insert(KHUVUCPO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.KHUVUCPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "khu vực");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "khu vực", objUi.TENKV);
            }
            return msg;
        }

        public Message Update(KHUVUCPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAKVPO     );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENKV = objUi.TENKV  ;
                    objDb.DACBIET = objUi.DACBIET ;
                    objDb.STARTCODE  = objUi.STARTCODE;
                    objDb.ORDERS   = objUi.ORDERS  ;
                       // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khu vực");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "khu vực", objUi.TENKV);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "khu vực", objUi.TENKV);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.DUONGPHOPOs.Where(p => p.MAKVPO.Equals(ma)).Count() > 0)
                return true;

            if (_db.PHUONGPOs.Where(p => p.MAKVPO.Equals(ma)).Count() > 0)
                return true;

            if (_db.DONDANGKYPOs.Where(p => p.MAKVPO .Equals(ma)).Count() > 0)
                return true;
            
            if (_db.NHANVIENs.Where(p => p.MAKV .Equals(ma)).Count() > 0)
                return true;
            
            /*if (_db.TIEUTHUs  .Where(p => p.MAKV.Equals(ma)).Count() > 0)
                return true;
            
            if (_db.KHACHHANGs .Where(p => p.MAKV.Equals(ma)).Count() > 0)
                return true;
            
            if (_db.GIAIQUYETTHONGTINSUACHUAs .Where(p => p.MAKV.Equals(ma)).Count() > 0)
                return true;

            if (_db.HOPDONGs .Where(p => p.MAKV.Equals(ma)).Count() > 0)
                return true;
            */
            return false;
        }

        public Message Delete(KHUVUCPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAKVPO   );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khu vực", objUi.TENKV      );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.KHUVUCPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Khu vực ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Khu vực ");
            }

            return msg;
        }

        public Message DeleteList(List<KHUVUCPO> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách khu vực");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "khu vực", failed, "khu vực");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " khu vực");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách khu vực");
            }

            return msg;
        }
    }
}
