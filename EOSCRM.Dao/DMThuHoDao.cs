using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class DMThuHoDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DMThuHoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DMThuHo Get(string id)
        {
            return _db.DMThuHos.Where(p => p.ID.Equals(id)).SingleOrDefault();
        }

        public List<DMThuHo> GetList()
        {
            return _db.DMThuHos.OrderBy(c => c.STT).ToList();
        }

    }
}
