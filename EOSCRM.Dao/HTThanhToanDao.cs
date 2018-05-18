using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class HinhThucThanhToanDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public HinhThucThanhToanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public HTTHANHTOAN Get(string ma)
        {
            return _db.HTTHANHTOANs.Where(p => p.MAHTTT.Equals(ma)).SingleOrDefault();
        }

        public List<HTTHANHTOAN> Search(string key)
        {
            return _db.HTTHANHTOANs.Where(p => p.MOTA.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<HTTHANHTOAN> GetList()
        {
            return _db.HTTHANHTOANs.OrderBy(h => h.ORDER).ToList();
        }

        public List<HTTHANHTOAN> GetListIsKeToan()
        {
            return _db.HTTHANHTOANs.Where(p => p.IsKeToan.Equals(true)).OrderBy(h => h.ORDER).ToList();
        }

        public List<HTTHANHTOAN> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.HTTHANHTOANs.Count();
        }

        public Message Insert(HTTHANHTOAN objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.HTTHANHTOANs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Hình thức thanh toán");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Hình thức thanh toán", objUi.MOTA );
            }
            return msg;
        }

        public Message Update(HTTHANHTOAN objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAHTTT);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MOTA  = objUi.MOTA ;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Hình thức thanh toán");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Hình thức thanh toán", objUi.MOTA );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Hình thức thanh toán", objUi.MOTA );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.HOPDONGs.Where(p => p.MAHTTT.Equals(ma)).Count() > 0)
                return true;
            else if (_db.HOADONLAPDATs.Where(p => p.MAHTTT.Equals(ma)).Count() > 0)
                return true;
            else if (_db.KHACHHANGs.Where(p => p.MAHTTT.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(HTTHANHTOAN objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAHTTT);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Hình thức thanh toán", objUi.MOTA);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.HTTHANHTOANs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hình thức thanh toán");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Hình thức thanh toán");
            }

            return msg;
        }

    }
}
