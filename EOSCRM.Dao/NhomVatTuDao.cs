using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class NhomVatTuDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public NhomVatTuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public NHOMVATTU Get(string ma)
        {
            return _db.NHOMVATTUs.Where(p => p.MANHOM .Equals(ma)).SingleOrDefault();
        }

        public List<NHOMVATTU> Search(string key)
        {
            return _db.NHOMVATTUs.Where(p => p.TENNHOM.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<NHOMVATTU> GetList()
        {
            return _db.NHOMVATTUs.ToList();
        }

        public List<NHOMVATTU> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.NHOMVATTUs.Count();
        }

        public Message Insert(NHOMVATTU objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.NHOMVATTUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Nhóm vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Nhóm vật tư ", objUi.TENNHOM );
            }
            return msg;
        }

        public Message Update(NHOMVATTU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MANHOM);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENNHOM  = objUi.TENNHOM;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Nhóm vật tư ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Nhóm vật tư ", objUi.TENNHOM );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Nhóm vật tư ", objUi.TENNHOM);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.VATTUs.Where(p => p.MANHOM.Equals(ma)).Count() > 0;
        }

        public Message Delete(NHOMVATTU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MANHOM );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Nhóm vật tư ", objUi.TENNHOM);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.NHOMVATTUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Nhóm vật tư ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Nhóm vật tư ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<NHOMVATTU> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Nhóm vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách nhóm vật tư ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<NHOMVATTU> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách nhóm vật tư");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "nhóm vật tư", failed, "nhóm vật tư");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " nhóm vật tư");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách nhóm phản hồi");
            }

            return msg;
        }
    }
}
