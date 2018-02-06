using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class DaoLapThietKeDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DaoLapThietKeDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DAOLAPTK  Get(int maDon)
        {
            return _db.DAOLAPTKs.Where(p => p.MADON.Equals(maDon)).SingleOrDefault();
        }

        public List<DAOLAPTK> GetList(string maDon)
        {
            return _db.DAOLAPTKs.Where( p=>p.MADDK .Equals( maDon )).ToList();
        }


        public Message Insert(DAOLAPTK objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DAOLAPTKs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Đào lấp thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Đào lấp thiết kế ", objUi.THIETKE .TENTK);
            }
            return msg;
        }

        public Message Update(DAOLAPTK objUi)
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
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đào lấp thiết kế ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đào lấp thiết kế ", objUi.THIETKE.TENTK);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đào lấp thiết kế ", objUi.THIETKE.TENTK);
            }
            return msg;
        }


        public Message Delete(DAOLAPTK objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đào lấp thiết kế ", objUi.THIETKE.TENTK);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DAOLAPTKs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp thiết kế ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đào lấp thiết kế ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<DAOLAPTK> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách đào lấp thiết kế ");
            }

            return msg;
        }
    }
}
