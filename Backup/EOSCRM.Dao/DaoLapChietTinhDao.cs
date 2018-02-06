using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class DaoLapChietTinhDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DaoLapChietTinhDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DAOLAP  Get(int maDon)
        {
            return _db.DAOLAPs.Where(p => p.MADON.Equals(maDon)).SingleOrDefault();
        }

        public List<DAOLAP> GetList(string maDDK)
        {
            return _db.DAOLAPs.Where(p => p.MADDK.Equals(maDDK)).ToList();
        }


        public Message Insert(DAOLAP objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();
                var cpqt = new DAOLAPQUYETTOAN
                {
                    MADDK = objUi.MADDK,
                    NOIDUNG = objUi.NOIDUNG,
                    DONGIACP = objUi.DONGIACP,
                    SOLUONG = objUi.SOLUONG,
                    DVT = objUi.DVT,
                    HESOCP = objUi.HESOCP,
                    THANHTIENCP = objUi.THANHTIENCP,
                    LOAICP = objUi.LOAICP,
                    NGAYLAP = objUi.NGAYLAP,
                    LOAICT = objUi.LOAICT
                };
                _db.DAOLAPQUYETTOANs.InsertOnSubmit(cpqt);
                _db.DAOLAPs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
            }
            return msg;
        }

        public Message Update(DAOLAP objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON);

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
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
            }
            return msg;
        }


        public Message Delete(DAOLAP objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DAOLAPs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đào lấp chiết tính miễn phí ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<DAOLAP> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp chiết tính ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách đào lấp chiết tính ");
            }

            return msg;
        }
    }
}
