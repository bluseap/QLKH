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
    public class MauThietKeDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public MauThietKeDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public MAUTHIETKE  Get(string ma)
        {
            return _db.MAUTHIETKEs.Where(p => p.MAMAUTK .Equals(ma)).SingleOrDefault();
        }       

        public List<MAUTHIETKE> GetList()
        {
            return _db.MAUTHIETKEs
                .Where(p => p.MAMAUTK.Substring(0,1).Equals("N"))
                .OrderBy(vt => vt.MAMAUTK).ToList();
        }

        public List<MAUTHIETKE> GetListFromOrder9()
        {
            return _db.MAUTHIETKEs
                .Where(p => p.MAMAUTK.Substring(0, 1).Equals("N") && p.Orders > 9)
                .OrderBy(vt => vt.Orders).ToList();
        }

        public List<MAUTHIETKE> GetListDien()
        {
            return _db.MAUTHIETKEs.Where(p => p.MAMAUTK.Substring(0,1).Equals("D")).OrderBy(vt => vt.MAMAUTK).ToList();
        }


    }
}
