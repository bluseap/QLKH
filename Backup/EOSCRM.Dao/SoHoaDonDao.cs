using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class SoHoaDonDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public SoHoaDonDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public SOHOADON Get(string ma)
        {
            return _db.SOHOADONs.SingleOrDefault(p => p.MASOHD.Equals(ma));
        }

        public List<SOHOADON> GetList()
        {
            return _db.SOHOADONs.OrderBy(p => p.MAKV).ToList();
        }

        public List<SOHOADON> GetListSHD(int thang, int nam, string makv)
        {
            return _db.SOHOADONs.Where(p => p.THANG==thang && p.NAM==nam && p.MAKV == makv)
                .OrderBy(p => p.SOHDDAU).ToList();
                    //.OrderByDescending(p => p.MASOHD).ToList();
        }

        public Message Insert(SOHOADON objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.SOHOADONs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "số HĐ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "số HĐ", objUi.MASOHD);
            }
            return msg;
        }

        
       
       

    }
}
