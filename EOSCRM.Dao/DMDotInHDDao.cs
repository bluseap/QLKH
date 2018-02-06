using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class DMDotInHDDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DMDotInHDDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DMDOTINHD Get(string macb)
        {
            return _db.DMDOTINHDs.Where(p => p.MADOTIN.Equals(macb)).SingleOrDefault();
        }

        public List<DMDOTINHD> GetList()
        {
            return _db.DMDOTINHDs.OrderBy(c => c.STT).ToList();
        }

        public List<DMDOTINHD> GetListNuoc()
        {
            return _db.DMDOTINHDs.Where(p => p.MADOTIN.Substring(0,2).Equals("NN"))
                .OrderBy(c => c.STT).ToList();
        }

        public List<DMDOTINHD> GetListDienNoP7()
        {
            return _db.DMDOTINHDs.Where(p => p.MADOTIN.Substring(0, 2).Equals("DD") && p.MADOTIN != "DDP7D1")
                .OrderBy(c => c.STT).ToList();
        }

        public List<DMDOTINHD> GetListDien()
        {
            return _db.DMDOTINHDs.Where(p => p.MADOTIN.Substring(0, 2).Equals("DD"))
                .OrderBy(c => c.STT).ToList();
        }
       
        public int Count( )
        {
            return _db.DMDOTINHDs.Count();
        }

       
    }
}
