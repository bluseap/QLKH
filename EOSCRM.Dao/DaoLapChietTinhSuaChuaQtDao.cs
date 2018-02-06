using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class DaoLapChietTinhSuaChuaQtDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DaoLapChietTinhSuaChuaQtDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public DAOLAPSUACHUAQT Get(int maDon)
        {
            return _db.DAOLAPSUACHUAQTs.Where(p => p.MA.Equals(maDon)).SingleOrDefault();
        }

        public List<DAOLAPSUACHUAQT> GetList(string maSC)
        {
            return _db.DAOLAPSUACHUAQTs.Where(p => p.MASC.Equals(maSC)).ToList();
        }


        public Message Insert(DAOLAPSUACHUAQT objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DAOLAPSUACHUAQTs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Đào lấp sữa chữa quyết toán ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Đào lấp sữa chữa quyết toán ", objUi.QUYETTOANSUACHUA.TENCT);
            }
            return msg;
        }

        public Message Update(DAOLAPSUACHUAQT objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MA);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DONGIACP = objUi.DONGIACP;
                    objDb.DVT = objUi.DVT;
                    objDb.HESOCP = objUi.HESOCP;
                    objDb.LOAICP = objUi.LOAICP;
                    objDb.LOAICV = objUi.LOAICV;
                    objDb.NOIDUNG = objUi.NOIDUNG;
                    objDb.SOLUONG = objUi.SOLUONG;
                    objDb.THANHTIENCP = objUi.THANHTIENCP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đào lấp sữa chữa quyết toán ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đào lấp quyết toán sữa chữa ", objUi.QUYETTOANSUACHUA.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đào lấp quyết toán sữa chữa ", objUi.QUYETTOANSUACHUA.TENCT);
            }
            return msg;
        }


        public Message Delete(DAOLAPSUACHUAQT objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MA);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đào lấp quyết toán sữa chữa ", objUi.QUYETTOANSUACHUA.TENCT);
                    return msg;
                }

               
                // Set delete info
                _db.DAOLAPSUACHUAQTs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp quyết toán sữa chữa ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đào lấp quyết toán sữa chữa ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<DAOLAPSUACHUAQT> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp quyết toán sửa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách đào lấp quyết toán sửa chữa ");
            }

            return msg;
        }
    }
}
