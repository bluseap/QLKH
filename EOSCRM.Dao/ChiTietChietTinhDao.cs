using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChiTietChietTinhDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ChiTietChietTinhDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CTCHIETTINH Get(string maDon, string maVatTu)
        {
            return _db.CTCHIETTINHs.Where(p => p.MADDK.Equals(maDon) && p.MAVT.Equals(maVatTu)).SingleOrDefault();
        }

        public List<CTCHIETTINH> GetList(string maDon)
        {
            var ccct = from ct in _db.CTCHIETTINHs
                       join vt in _db.VATTUs on ct.MAVT equals vt.MAVT
                       orderby vt.MAHIEU                       
                       select ct ;
            return ccct.Where(p => p.MADDK.Equals(maDon)).ToList();
            //return _db.CTCHIETTINHs.Where(p => p.MADDK.Equals(maDon)).ToList();
        }


        public Message Insert(CTCHIETTINH objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();
                _db.CTCHIETTINHs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chi tiết chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chi tiết chiết tính ", objUi.CHIETTINH.TENCT );
            }
            return msg;
        }

        public Message Update(CTCHIETTINH objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK , objUi .MAVT );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.GIANC = objUi.GIANC;
                    objDb.GIAVT = objUi.GIAVT;
                    objDb.SOLUONG = objUi.SOLUONG;
                    objDb.TIENNC = objUi.TIENNC ;
                    objDb.TIENVT = objUi.TIENVT ;
                    objDb.ISCTYDTU = objUi.ISCTYDTU;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Chi tiết chiết tính ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "chi tiết chiết tính", objUi.CHIETTINH.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chi tiết chiết tính ", objUi.CHIETTINH .TENCT);
            }
            return msg;
        }


        public Message Delete(CTCHIETTINH objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK , objUi .MAVT  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chi tiết chiết tính ", objUi.CHIETTINH .TENCT );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CTCHIETTINHs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết chiết tính ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Chi tiết chiết tính ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<CTCHIETTINH > objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết chiết tính ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách chi tiết chiết tính ");
            }

            return msg;
        }
    }
}
