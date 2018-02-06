using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class LTPXTDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LTPXTDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LTPXT Get(string ma)
        {
            return _db.LTPXTs.Where(p => p.MATPXT.Equals(ma)).SingleOrDefault();
        }

        public List<LTPXT> GetList()
        {
            return _db.LTPXTs.ToList();
        } 

        public List<LTPXT> Search(string key)
        {
            return _db.LTPXTs.Where(p => p.MATPXT.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LTPXTs.Count();
        }

    }
}
