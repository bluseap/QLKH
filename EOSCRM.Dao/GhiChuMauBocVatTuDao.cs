using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class GhiChuMauBocVatTuDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public GhiChuMauBocVatTuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public GCMAUBOCVATTU Get(int maDon)
        {
            return _db.GCMAUBOCVATTUs.Where(p => p.MAGHICHU.Equals(maDon)).SingleOrDefault();
        }

        public List<GCMAUBOCVATTU> GetList(string maMBVT)
        {
            return _db.GCMAUBOCVATTUs.Where(p => p.MAMBVT.Equals(maMBVT)).ToList();
        }


        public Message Insert(GCMAUBOCVATTU objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.GCMAUBOCVATTUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chi phí đào lấp mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chi phí đào lấp mẫu bốc vật tư ", objUi.MAUBOCVATTU.TENTK);
            }
            return msg;
        }

        public Message Update(GCMAUBOCVATTU objUi)
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
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Chi tiết mẫu bốc vật tư ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Chi tiết mẫu bốc vật tư", objUi.MAUBOCVATTU .TENTK  );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chi tiết mẫu bốc vật tư", objUi.MAUBOCVATTU.TENTK);
            }
            return msg;
        }


        public Message Delete(GCMAUBOCVATTU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAGHICHU);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chi phí đào lấp mẫu bốc vật tư", objUi.MAUBOCVATTU.TENTK);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.GCMAUBOCVATTUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi phí đào lấp mẫu bốc vật tư");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Chi phí đào lấp mẫu bốc vật tư");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<GCMAUBOCVATTU> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi phí đào lấp mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách chi phí đào lấp mẫu bốc vật tư ");
            }

            return msg;
        }
    }
}
