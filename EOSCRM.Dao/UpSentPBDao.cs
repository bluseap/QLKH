using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class UpSentPBDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public UpSentPBDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public UPSENTPB Gets(string ma)
        {
            return _db.UPSENTPBs.Where(p => p.MASENTPB.Equals(ma)).SingleOrDefault();
        }

        public UPSENTPB Get(string ma)
        {
            return _db.UPSENTPBs.Where(p => p.MAUPLOAD.Equals(ma)).SingleOrDefault();
        }

        public UPSENTPB GetMS(string maup, string mapbs)
        {
            return _db.UPSENTPBs.Where(p => p.MAUPLOAD.Equals(maup) && p.MAPBSENT.Equals(mapbs)).SingleOrDefault();
        }

        public List<UPSENTPB> Search(string key)
        {
            return _db.UPSENTPBs.Where(p => p.MANV.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<UPSENTPB> GetList()
        {
            return _db.UPSENTPBs.OrderByDescending(cv => cv.DATE).ToList();
        }


        public Message Insert(UPSENTPB objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.UPSENTPBs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "sent file thành công");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "sent file", objUi.MANV);
            }
            return msg;
        }

        public Message Update(UPSENTPB objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MASENTPB);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MANV = objUi.MANV;
                    objDb.MAPBSENT = objUi.MAPBSENT;
                   
                    objDb.DATE = objUi.DATE;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "sent file");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "sent file", objUi.MANV);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "sent file", objUi.MANV);
            }
            return msg;
        }

        /*public bool IsInUse(string ma)
        {
            return _db.UPLOADFILEs.Where(p => p.MACV.Equals(ma)).Count() > 0;
        }*/

        public Message Delete(UPSENTPB objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MASENTPB);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "sent file ", objUi.MANV);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.UPSENTPBs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "sent file ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "sent file ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<UPSENTPB> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách file");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "sent file", failed, "sent file");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " sent file");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách file");
            }

            return msg;
        }

        public string NewId()
        {
            var query = _db.UPSENTPBs.Max(p => p.MASENTPB);

            var temp = int.Parse(query) + 1;
            return temp.ToString("D20");
        }

        public Message SentFileList(List<UPSENTPB> objList)
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
                    //var temp = SentFile(obj);
                    if (temp != null && temp.MsgType.Equals(MessageType.Error))
                        failed++;
                }

                // commit
                trans.Commit();

                if (failed > 0)
                {
                    if (failed == objList.Count)
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách file gửi");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "gửi file", failed, "gửi file");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " gửi file");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách file gửi");
            }

            return msg;
        }
    }
}
