using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class KiemDinhDongHoChiTietDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KiemDinhDongHoChiTietDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KDDHCT Get(int ma)
        {
            return _db.KDDHCTs.Where(p => p.KDCTID.Equals(ma)).SingleOrDefault();
        }

        public List<KDDHCT> GetList()
        {
            return _db.KDDHCTs.ToList();
        }

        public List<KDDHCT> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.KDDHCTs.Count();
        }

        public Message Insert(KDDHCT objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.KDDHCTs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ chi tiết");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Kiểm định đồng hồ ", "");
            }
            return msg;
        }

        public Message Insert(List<KDDHCT> objUiList)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                foreach (var objUi in objUiList )
                    _db.KDDHCTs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ chi tiết");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Kiểm định đồng hồ ", "");
            }
            return msg;
        }

        public Message Update(KDDHCT  objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.KDCTID   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CHUYEN = objUi.CHUYEN;
                    objDb.DAT = objUi.DAT;
                    objDb.DATRS = objUi.DATRS;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.ID = objUi.ID;
                    objDb.MADH = objUi.MADH;
                    objDb.SSAU1 = objUi.SSAU1;
                    objDb.SSAU2 = objUi.SSAU2;
                    objDb.SSAU3 = objUi.SSAU3;
                    objDb.STRUOC = objUi.STRUOC;
                    objDb.VDO1 = objUi.VDO1;
                    objDb.VDO2 = objUi.VDO2;
                    objDb.VDO3 = objUi.VDO3;
                   
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ chi tiết");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Kiểm định đồng hồ chi tiết", ""   );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Kiểm định đồng hồ chi tiết", ""   );
            }
            return msg;
        }

        public bool IsInUse(int ma)
        {
            return false;
        }

        public Message Delete(KDDHCT objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.KDCTID );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Kiểm định đồng hồ chi tiết ", "");
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.KDDHCTs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ chi tiết ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Kiểm định đồng hồ chi tiết ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<KDDHCT> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ chi tiết ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách kiểm định đồng hồ chi tiết ");
            }

            return msg;
        }
    }
}
