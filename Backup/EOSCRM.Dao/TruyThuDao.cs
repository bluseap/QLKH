using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class TruyThuDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public TruyThuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public TRUYTHUHISTORY Get(int macb)
        {
            return _db.TRUYTHUHISTORies.Where(p => p.ID.Equals(macb)).SingleOrDefault();
            
        }

        public List<TRUYTHUHISTORY> Search(string key)
        {
            return _db.TRUYTHUHISTORies.Where(p => p.MANV.Equals(key)).ToList();
        }

        public Message Insert(TRUYTHUHISTORY objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.TRUYTHUHISTORies.InsertOnSubmit(objUi);
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
