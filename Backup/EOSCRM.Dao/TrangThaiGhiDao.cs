using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class TrangThaiGhiDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public TrangThaiGhiDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public TRANGTHAIGHI   Get(string ma)
        {
            return _db.TRANGTHAIGHIs.SingleOrDefault(p => p.TTHAIGHI.Equals(ma));
        }
        
        public List<TRANGTHAIGHI> GetList()
        {
            return _db.TRANGTHAIGHIs.OrderBy(p => p.STT).ToList();
        }

      

        public List<TRANGTHAIGHI> GetListTTG(string makv)
        {
            return _db.TRANGTHAIGHIs.Where(p => p.TTHAIGHI == makv).ToList();
        }

        
    }
}
