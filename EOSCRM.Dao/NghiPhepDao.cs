using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class NghiPhepDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public NghiPhepDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public NGHIPHEP Get(string id)
        {
            return _db.NGHIPHEPs.Where(p => p.MAPHEP.Equals(id)).SingleOrDefault();
        }

        public NGHIPHEP GetNV(string id)
        {
            return _db.NGHIPHEPs.Where(p => p.MANV.Equals(id)).SingleOrDefault();
        }



    }
}
