using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChiTietChietTinhSuaChuaDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ChiTietChietTinhSuaChuaDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CTCHIETTINHSUACHUA Get(string maDon, string maVatTu)
        {
            return _db.CTCHIETTINHSUACHUAs.Where(p => p.MADON .Equals(maDon) && p.MAVT.Equals(maVatTu)).SingleOrDefault();
        }

        public List<CTCHIETTINHSUACHUA> GetList(string maDon)
        {
            return _db.CTCHIETTINHSUACHUAs.Where(p => p.MADON.Equals(maDon)).ToList();
        }


        public Message Insert(CTCHIETTINHSUACHUA objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CTCHIETTINHSUACHUAs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chi tiết chiết tính sữa chữa");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chi tiết chiết tính sữa chữa", objUi.CHIETTINHSUACHUA .TENCT );
            }
            return msg;
        }

        public Message Update(CTCHIETTINHSUACHUA objUi)
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
                                      objUi.CHIETTINHSUACHUA.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chi tiết chiết tính sữa chữa ",
                                                             objUi.CHIETTINHSUACHUA.TENCT);
            }
            return msg;
        }


        public Message Delete(CTCHIETTINHSUACHUA objUi)
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
                                      objUi.CHIETTINHSUACHUA.TENCT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CTCHIETTINHSUACHUAs.DeleteOnSubmit(objDb);
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
        public Message DeleteList(List<CTCHIETTINHSUACHUA> objList, PageAction action)
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
