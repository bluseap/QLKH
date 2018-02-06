using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LDanTocDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LDanTocDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LDANTOC Get(string ma)
        {
            return _db.LDANTOCs.Where(p => p.MADT.Equals(ma)).SingleOrDefault();
        }

        public List<LDANTOC> GetList()
        {
            return _db.LDANTOCs.OrderBy(s => s.STT).ToList();
        }

        public List<LDANTOC> Search(string key)
        {
            return _db.LDANTOCs.Where(p => p.MADT.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LDANTOCs.Count();
        }

    }
}
