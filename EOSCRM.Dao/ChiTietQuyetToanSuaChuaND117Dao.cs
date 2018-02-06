using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChiTietQuyetToanSuaChuaND117Dao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ChiTietQuyetToanSuaChuaND117Dao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CTQUYETTOANSUACHUA_ND117 Get(string maDon, string maVatTu)
        {
            return _db.CTQUYETTOANSUACHUA_ND117s.Where(p => p.MADON.Equals(maDon) && p.MAVT.Equals(maVatTu)).SingleOrDefault();
        }

        public List<CTQUYETTOANSUACHUA_ND117> GetList(string maDon)
        {
            return _db.CTQUYETTOANSUACHUA_ND117s.Where(p => p.MADON.Equals(maDon)).ToList();
        }


        public Message Insert(CTQUYETTOANSUACHUA_ND117 objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CTQUYETTOANSUACHUA_ND117s.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chi tiết chiết tính sữa chữa");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chi tiết chiết tính sữa chữa", objUi.QUYETTOANSUACHUA.TENCT );
            }
            return msg;
        }

        public Message Update(CTQUYETTOANSUACHUA_ND117 objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON  , objUi .MAVT );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.GIANC = objUi.GIANC;
                    objDb.GIAVT = objUi.GIAVT;
                    objDb.SOLUONG = objUi.SOLUONG;
                    objDb.TIENNC = objUi.TIENNC;
                    objDb.TIENVT = objUi.TIENVT;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "chi tiết chiết tính sữa chữa");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "chi tiết chiết tính sữa chữa",
                                      objUi.QUYETTOANSUACHUA.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chi tiết chiết tính sữa chữa ",
                                                             objUi.QUYETTOANSUACHUA.TENCT);
            }
            return msg;
        }


        public Message Delete(CTQUYETTOANSUACHUA_ND117 objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON  , objUi .MAVT  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chi tiết chiết tính sữa chữa ",
                                      objUi.QUYETTOANSUACHUA.TENCT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CTQUYETTOANSUACHUA_ND117s.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết chiết tính sữa chũa ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Chi tiết chiết tính sữa chữa ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<CTQUYETTOANSUACHUA_ND117> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết chiết tính sửa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách chi tiết chiết tính sữa chữa ");
            }

            return msg;
        }
    }
}
