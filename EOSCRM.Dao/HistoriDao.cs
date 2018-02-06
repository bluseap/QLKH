using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class HistoriDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public HistoriDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public HISTORY Get(int macb)
        {
            return _db.HISTORies.Where(p => p.ID.Equals(macb)).SingleOrDefault();
            
        }

        public List<HISTORY> Search(string key)
        {
            return _db.HISTORies.Where(p => p.MANV.Equals(key)).ToList();
        }

        public Message Insert(HISTORY objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.HISTORies.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                               
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "", "");
            }
            return msg;
        }
    }
}
