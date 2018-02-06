using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class KyDuyetDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KyDuyetDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public LUUVET_KYDUYET Get(int macb)
        {
            return _db.LUUVET_KYDUYETs.Where(p => p.ID.Equals(macb)).SingleOrDefault();
        }

        public List<LUUVET_KYDUYET> Search(string key)
        {
            return _db.LUUVET_KYDUYETs.Where(p => p.MADON.Equals(key)).ToList();
        }

        public Message Insert(LUUVET_KYDUYET objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.LUUVET_KYDUYETs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                // commit

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