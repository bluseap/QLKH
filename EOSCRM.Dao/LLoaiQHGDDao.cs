using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LLoaiQHGDDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LLoaiQHGDDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LLOAIQHGD Get(string ma)
        {
            return _db.LLOAIQHGDs.Where(p => p.MALQHGD.Equals(ma)).SingleOrDefault();
        }

        public List<LLOAIQHGD> GetList()
        {
            return _db.LLOAIQHGDs.ToList();
        }

        public List<LLOAIQHGD> Search(string key)
        {
            return _db.LLOAIQHGDs.Where(p => p.MALQHGD.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LLOAIQHGDs.Count();
        }
    }
}
