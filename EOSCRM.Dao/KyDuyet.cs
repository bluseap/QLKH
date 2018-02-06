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

        public LUUVET_KYDUYET GetMaDon(string madkk, string matt, string mota)
        {
            var query = _db.LUUVET_KYDUYETs.Where(p => p.MADON.Equals(madkk) && p.MATT.Equals(matt) && p.MOTA.Equals(mota)
                        && p.ID.Equals((from c in _db.LUUVET_KYDUYETs.Where(c => c.MADON.Equals(madkk) && c.MATT.Equals(matt) && c.MOTA.Equals(mota))
                                            select c.ID).Max())     
                );

            return query.SingleOrDefault();
            //return _db.LUUVET_KYDUYETs.Where(p => p.MADON.Equals(madkk) && p.MATT.Equals(matt) && p.MOTA.Equals(mota)).SingleOrDefault();

        }

        public LUUVET_KYDUYET GetMaTT(string maddk, string matthai)
        {
            return _db.LUUVET_KYDUYETs.Where(p => p.MADON.Equals(maddk) && p.MATT.Equals(matthai)).SingleOrDefault();
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