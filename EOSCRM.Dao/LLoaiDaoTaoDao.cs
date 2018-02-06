using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LLoaiDaoTaoDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LLoaiDaoTaoDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LLOAIDTBD Get(string ma)
        {
            return _db.LLOAIDTBDs.Where(p => p.MALOAIDTBD.Equals(ma)).SingleOrDefault();
        }

        public List<LLOAIDTBD> GetList()
        {
            return _db.LLOAIDTBDs.ToList();
        }

        public List<LLOAIDTBD> Search(string key)
        {
            return _db.LLOAIDTBDs.Where(p => p.MALOAIDTBD.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LLOAIDTBDs.Count();
        }
    }
}
