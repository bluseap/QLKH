using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Configuration;


namespace EOSCRM.Dao
{
    public  class PhanCongDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public PhanCongDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public List<PHANCONG> GetList()
        {
            return _db.PHANCONGs.ToList();
        }

        /// <summary>
        /// update shared user list
        /// </summary>
        /// <param name="pcs"></param>
        /// <returns></returns>
        public Message UpdatePhanCong(List<PHANCONGTHEOPHUONG> pcs)
        {
            DbTransaction trans = null;
            Message msg;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // clear all user ids
                var list = _db.PHANCONGTHEOPHUONGs.ToList();
                _db.PHANCONGTHEOPHUONGs.DeleteAllOnSubmit(list);
                _db.SubmitChanges();

                if (pcs.Count > 0)
                {
                    // add new user id
                    foreach (var pc in pcs)
                    {
                        _db.PHANCONGTHEOPHUONGs.InsertOnSubmit(pc);
                    }

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Phân công khảo sát thiết kế");

            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "phân công khảo sát thiết kế");
            }

            return msg;
        }
    }
}
