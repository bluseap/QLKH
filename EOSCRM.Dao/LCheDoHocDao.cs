using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LCheDoHocDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LCheDoHocDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LCHEDOHOC Get(string ma)
        {
            return _db.LCHEDOHOCs.Where(p => p.MACHEDOHOC.Equals(ma)).SingleOrDefault();
        }

        public List<LCHEDOHOC> GetList()
        {
            return _db.LCHEDOHOCs.ToList();
        }

        public List<LCHEDOHOC> Search(string key)
        {
            return _db.LCHEDOHOCs.Where(p => p.MACHEDOHOC.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LCHEDOHOCs.Count();
        }

    }
}
