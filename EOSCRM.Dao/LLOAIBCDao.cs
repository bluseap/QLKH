using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LLOAIBCDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LLOAIBCDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LLOAIBC Get(string ma)
        {
            return _db.LLOAIBCs.Where(p => p.MALOAIBC.Equals(ma)).SingleOrDefault();
        }

        public List<LLOAIBC> GetList()
        {
            return _db.LLOAIBCs.ToList();
        }

        public List<LLOAIBC> Search(string key)
        {
            return _db.LLOAIBCs.Where(p => p.MALOAIBC.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LLOAIBCs.Count();
        }
    }
}
