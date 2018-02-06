using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LLoaiKLKTDao
    {
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LLoaiKLKTDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LLOAIKYLUATKT Get(string ma)
        {
            return _db.LLOAIKYLUATKTs.Where(p => p.MALKLKT.Equals(ma)).SingleOrDefault();
        }

        public List<LLOAIKYLUATKT> GetList()
        {
            return _db.LLOAIKYLUATKTs.ToList();
        }

        public List<LLOAIKYLUATKT> Search(string key)
        {
            return _db.LLOAIKYLUATKTs.Where(p => p.MALKLKT.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LLOAIKYLUATKTs.Count();
        }
    }
}
