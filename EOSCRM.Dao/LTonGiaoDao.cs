using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class LTonGiaoDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LTonGiaoDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LTONGIAO Get(string ma)
        {
            return _db.LTONGIAOs.Where(p => p.MATG.Equals(ma)).SingleOrDefault();
        }

        public List<LTONGIAO> GetList()
        {
            return _db.LTONGIAOs.ToList();
        }

        public List<LTONGIAO> Search(string key)
        {
            return _db.LTONGIAOs.Where(p => p.MATG.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LTONGIAOs.Count();
        }
    }
}
