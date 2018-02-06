using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class UpLoadFileDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public UpLoadFileDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public UPLOADFILE Get(string ma)
        {
            return _db.UPLOADFILEs.Where(p => p.MAUPLOAD.Equals(ma)).SingleOrDefault();
        }

        public List<UPLOADFILE> Search(string key)
        {
            return _db.UPLOADFILEs.Where(p => p.MANV.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<UPLOADFILE> GetList()
        {
            return _db.UPLOADFILEs.OrderByDescending(cv => cv.DATE).ToList();
        }

        public List<UPLOADFILE> GetListMANVS(string manv)
        {
            /*var dsup = from upfile in _db.UPLOADFILEs
                       join sentnv in _db.UPSENTNVs on upfile.MAUPLOAD equals sentnv.MAUPLOAD into g
                       from sentnv in g.DefaultIfEmpty()
                        where (upfile.MANV == manv || sentnv.MANV==manv)
                       select upfile;
            */
            var dsup = from upfile in _db.UPLOADFILEs
                       join sentnv in _db.UPSENTNVs on upfile.MAUPLOAD equals sentnv.MAUPLOAD                       
                       where (sentnv.MANVSENT == manv)
                       orderby sentnv.DATE descending
                       select upfile;
            //return dsup.OrderByDescending(cv => cv.DATE).ToList();
            return dsup.ToList();
        }

        public UPLOADFILE GetTenFile(string tenfile)
        {
            return _db.UPLOADFILEs.FirstOrDefault(p => p.TENFILE == tenfile);
        }

        /*public List<UPLOADFILE> GetList(string maup, string manv)
        {
            var query = _db.UPLOADFILEs.AsQueryable();

            if (!string.IsNullOrEmpty(maup))
                query = query.Where(cv => cv.MACV.Contains(macv)).AsQueryable();

            if (!string.IsNullOrEmpty(manv))
                query = query.Where(cv => cv.TENCV.Contains(tencv)).AsQueryable();

            return query.OrderBy(cv => cv.TENCV).ToList();
        }*/

        public int Count()
        {
            return _db.CONGVIECs.Count();
        }

        public Message Insert(UPLOADFILE objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.UPLOADFILEs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "tải file thành công");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "upload file", objUi.MANV);
            }
            return msg;
        }

        public Message Update(UPLOADFILE objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAUPLOAD);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MANV = objUi.MANV;
                    objDb.TENFILE = objUi.TENFILE;
                    objDb.TENPATH = objUi.TENPATH;
                    objDb.DATE = objUi.DATE;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "upload file");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "upload file", objUi.MANV);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "upload file", objUi.MANV);
            }
            return msg;
        }

        /*public bool IsInUse(string ma)
        {
            return _db.UPLOADFILEs.Where(p => p.MACV.Equals(ma)).Count() > 0;
        }*/

        public Message Delete(UPLOADFILE objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MANV);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Upload file ", objUi.MANV);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.UPLOADFILEs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Upload file ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Upload file ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<UPLOADFILE> objList)
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
                                       succeed, "upload file", failed, "upload file");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " upload file");
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
            var query = _db.UPLOADFILEs.Max(p => p.MAUPLOAD);

            if (query != null)
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D20");
            }

            return "00000000000000000001";
        }

        public Message SentFileList(List<UPLOADFILE> objList)
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
