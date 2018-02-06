using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChiTietMauBocVatTuDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ChiTietMauBocVatTuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CTMAUBOCVATTU Get(string maDon, string maVatTu)
        {
            return _db.CTMAUBOCVATTUs.Where(p => p.MADDK.Equals(maDon) && p.MAVT .Equals( maVatTu )).SingleOrDefault();
        }

        public List<CTMAUBOCVATTU> GetList(string maDon)
        {
            var ctvt = from ctv in _db.CTMAUBOCVATTUs
                       join vt in _db.VATTUs on ctv.MAVT equals vt.MAVT
                       orderby vt.MAHIEU
                       where (ctv.MADDK.Equals(maDon))
                       select ctv;
            return ctvt.ToList();
            //return _db.CTMAUBOCVATTUs.Where( p=>p.MADDK .Equals( maDon)).ToList();
        }

   
        public Message Insert(CTMAUBOCVATTU objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CTMAUBOCVATTUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chi tiết mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chi tiết mẫu bốc vật tư ", objUi.MAUBOCVATTU .TENTK );
            }
            return msg;
        }

        public Message Update(CTMAUBOCVATTU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK , objUi .MAVT );

                if (objDb != null)
                {
                    objDb.NOIDUNG = objUi.NOIDUNG;
                    objDb.SOLUONG = objUi.SOLUONG;
                    objDb.GIANC = objUi.GIANC;
                    objDb.TIENNC = objUi.TIENNC;
                    objDb.GIAVT = objUi.GIAVT;
                    objDb.TIENVT = objUi.TIENVT;
                    objDb.ISCTYDTU = objUi.ISCTYDTU;
                  
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "chi tiết mẫu bốc vật tư ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "chi tiết mẫu bốc vật tư", objUi.MAUBOCVATTU .TENTK  );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chi tiết mẫu bốc vật tư", objUi.MAUBOCVATTU.TENTK);
            }
            return msg;
        }


        public Message Delete(CTMAUBOCVATTU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK , objUi .MAVT  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chi tiết mẫu bốc vật tư", objUi.MAUBOCVATTU.TENTK);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CTMAUBOCVATTUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết mẫu bốc vật tư");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Chi tiết mẫu bốc vật tư");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<CTMAUBOCVATTU> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách chi tiết mẫu bốc vật tư ");
            }

            return msg;
        }
    }
}
