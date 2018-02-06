using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class TenKhoVTDao
    {   
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public TenKhoVTDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public TENKHOVT  Get(string macb)
        {
            return _db.TENKHOVTs.Where(p => p.MATAIKHO .Equals(macb)).SingleOrDefault();
        }               

        public List<TENKHOVT> GetList()
        {
            return _db.TENKHOVTs.OrderBy(vt => vt.MATAIKHO).ToList();
        }

        public List<TENKHOVT> GetListKV(string makv)
        {
            return _db.TENKHOVTs.OrderBy(vt => vt.MAKV).ToList();
        }

    }
}
