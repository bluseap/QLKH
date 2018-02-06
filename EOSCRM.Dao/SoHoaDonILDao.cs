using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class SoHoaDonILDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public SoHoaDonILDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public SOHOADONIL Get(string ma)
        {
            return _db.SOHOADONILs.SingleOrDefault(p => p.MASOHDIL.Equals(ma));
        }

        public List<SOHOADONIL> GetList(string ma)
        {
            return _db.SOHOADONILs.OrderByDescending(p => p.MASOHDIL).ToList();
        }

        public List<SOHOADONIL> GetList()
        {
            return _db.SOHOADONILs.OrderBy(p => p.MASOHDIL).ToList();
        }

        public List<SOHOADONIL> GetListSHDIL(int masohd)
        {
            return _db.SOHOADONILs.Where(p => p.MASOHD==masohd)
                    .OrderBy(p => p.SOHDDAUIL).ToList();
        }

        public Message Insert(SOHOADONIL objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.SOHOADONILs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "số HĐ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "số HĐ", objUi.MASOHDIL);
            }
            return msg;
        }





    }
}
