using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class QuanHuyenDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public QuanHuyenDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public QUANHUYEN Get(string id)
        {
            return _db.QUANHUYENs.Where(p => p.Id.Equals(id)).SingleOrDefault();
        }

        public QUANHUYEN GetMAKV(string makv)
        {
            return _db.QUANHUYENs.Where(p => p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public List<QUANHUYEN> GetList()
        {
            return _db.QUANHUYENs.Where(p=>p.DACBIET.Equals(1)).OrderBy(c => c.ORDERS).ToList();
        }

        public int Count( )
        {
            return _db.QUANHUYENs.Count();
        }

    }
}
