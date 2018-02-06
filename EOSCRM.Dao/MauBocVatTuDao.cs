using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class MauBocVatTuDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public MauBocVatTuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public MAUBOCVATTU  Get(string ma)
        {
            return _db.MAUBOCVATTUs.Where(p => p.MADDK.Equals(ma) && p.LOAIMBVT.Equals("NN")).SingleOrDefault();
        }

        public MAUBOCVATTU GetD(string ma)
        {
            return _db.MAUBOCVATTUs.Where(p => p.MADDK.Equals(ma) && p.LOAIMBVT.Equals("DD")).SingleOrDefault();
        }

        public List<MAUBOCVATTU> GetList()
        {
            return _db.MAUBOCVATTUs.Where(p => p.LOAIMBVT.Equals("NN")).ToList();
        }

        public List<MAUBOCVATTU> GetListMAKV(string makv)
        {
            return _db.MAUBOCVATTUs.Where(p => p.LOAIMBVT.Equals("NN") && p.MAKV.Equals(makv)).ToList();
        }

        public List<MAUBOCVATTU> GetListDien()
        {
            return _db.MAUBOCVATTUs.Where(p => p.LOAIMBVT.Equals("DD")).ToList();
        }

        public List<MAUBOCVATTU> GetListDienMAKV(string makv)
        {
            return _db.MAUBOCVATTUs.Where(p => p.LOAIMBVT.Equals("DD") && p.MAKV.Equals(makv)).ToList();
        }

        public List<MAUBOCVATTU> GetListNN()
        {
            return _db.MAUBOCVATTUs.Where(p => p.LOAIMBVT.Equals("NN")).ToList();
        }

        public List<MAUBOCVATTU> GetListDD()
        {
            return _db.MAUBOCVATTUs.Where(p => p.LOAIMBVT.Equals("DD")).ToList();
        }

        public List<MAUBOCVATTU> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.MAUBOCVATTUs.Count();
        }

        public Message Insert(MAUBOCVATTU objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.MAUBOCVATTUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Mẫu bốc vật tư ", "");
            }
            return msg;
        }

        public Message Update(MAUBOCVATTU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENTK = objUi.TENTK;
                    objDb.MAKV = objUi.MAKV;
                    
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Mẫu bốc vật tư ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Mẫu bốc vật tư ", "");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Mẫu bốc vật tư ", "");
            }
            return msg;
        }        

        public Message UpdatePo(MAUBOCVATTU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetD(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENTK = objUi.TENTK;
                    objDb.MAKV = objUi.MAKV;
                    
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Mẫu bốc vật tư ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Mẫu bốc vật tư ", "");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Mẫu bốc vật tư ", "");
            }
            return msg;
        }
        public bool IsInUse(string ma)
        {
            return _db.MAUBOCVATTUs .Where( p=>p.MADDK .Equals( ma)).Count( ) >0;
        }

        public Message Delete(MAUBOCVATTU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Mẫu bốc vật tư ", "");
                    return msg;
                }

                //TODO: delete all references
                var ctmbvtlist = _db.CTMAUBOCVATTUs.Where(p => p.MADDK.Equals(objDb.MADDK)).ToList();
                foreach (var ctmbvt in ctmbvtlist)
                {
                    _db.CTMAUBOCVATTUs.DeleteOnSubmit(ctmbvt);
                }

                var cpmbvtlist = _db.DAOLAPMAUBOCVATTUs.Where(p => p.MAMAUBOCVATTU.Equals(objDb.MADDK)).ToList();
                foreach (var cpmbvt in cpmbvtlist)
                {
                    _db.DAOLAPMAUBOCVATTUs.DeleteOnSubmit(cpmbvt);
                }

                var gcmbvtlist = _db.GCMAUBOCVATTUs.Where(p => p.MAMBVT.Equals(objDb.MADDK)).ToList();
                foreach (var gcmbvt in gcmbvtlist)
                {
                    _db.GCMAUBOCVATTUs.DeleteOnSubmit(gcmbvt);
                }

                // Set delete info
                _db.MAUBOCVATTUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Mẫu bốc vật tư ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<MAUBOCVATTU> objList)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Mẫu bốc vật tư ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách mẫu bốc vật tư ");
            }

            return msg;
        }
    }
}
