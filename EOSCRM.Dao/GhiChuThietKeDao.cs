using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class GhiChuThietKeDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public GhiChuThietKeDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public GCTHIETKE Get(int maDon)
        {
            return _db.GCTHIETKEs.Where(p => p.MAGHICHU.Equals(maDon)).SingleOrDefault();
        }

        public GCTHIETKE GetMaDonKyHieu(string maddk, string kyhieu)
        {
            return _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(maddk) && p.KYHIEU.Equals(kyhieu)).SingleOrDefault();
        }

        public GCTHIETKE GetMaDonKy(string maddk)
        {
            return _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(maddk)).SingleOrDefault();
        } 

        public List<GCTHIETKE> GetList(string maMBVT)
        {
            return _db.GCTHIETKEs.Where(p => p.MAMBVT.Equals(maMBVT)).ToList();
        }

        public Message Insert(GCTHIETKE objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.GCTHIETKEs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Ghi chú thiết kế");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chú thiết kế", objUi.THIETKE.TENTK);
            }
            return msg;
        }

        public Message Update(GCTHIETKE objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAGHICHU);

                if (objDb != null)
                {
                    //TODO: update all fields

                    objDb.MAMBVT = objUi.MAMBVT;
                    objDb.NOIDUNG = objUi.NOIDUNG;
                  
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Ghi chú thiết kế");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Ghi chú thiết kế", objUi.THIETKE.TENTK);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Ghi chú thiết kế", objUi.THIETKE.TENTK);
            }
            return msg;
        }


        public Message Delete(GCTHIETKE objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAGHICHU);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Ghi chú thiết kế", objUi.THIETKE.TENTK);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.GCTHIETKEs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Ghi chú thiết kế");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Ghi chú thiết kế");
            }

            return msg;
        }
       
        public Message DeleteList(List<GCTHIETKE> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Ghi chú thiết kế");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách ghi chú thiết kế");
            }

            return msg;
        }

    }
}
