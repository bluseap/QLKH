using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class DotInHDDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DotInHDDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DOTINHD Get(string macb)
        {
            return _db.DOTINHDs.Where(p => p.IDMADOTIN.Equals(macb)).SingleOrDefault();
        }

        public DMDOTINHD GetDMDotIn(string macb)
        {
            return _db.DMDOTINHDs.Where(p => p.MADOTIN.Equals(Get(macb).MADOTIN)).SingleOrDefault();
        } 

        public DOTINHD GetKVDot(string madot, string makv)
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Equals(madot) && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public DOTINHD GetKVP7D1(string p7d1, string makv)
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Equals(p7d1) && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public DOTINHD GetKVPoDot(string madot, string makvpo)
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Equals(madot) && p.MAKVPO.Equals(makvpo)).SingleOrDefault();
        }        

        public List<DOTINHD> GetList()
        {
            return _db.DOTINHDs.OrderBy(c => c.STT).ToList();
        }

        public List<DOTINHD> GetListNN()
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Substring(0, 2).Equals("NN")).OrderBy(c => c.STT).ToList();
        }

        public List<DOTINHD> GetListDD()
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Substring(0, 2).Equals("DD")).OrderBy(c => c.STT).ToList();
        }

        public List<DOTINHD> GetListKVPO(string makvpo)
        {
            return _db.DOTINHDs.Where(p => p.MAKVPO.Equals(makvpo)).OrderBy(c => c.STT).ToList();
        }       

        public List<DOTINHD> GetListKVNN(string makv)
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Substring(0,2).Equals("NN") && p.MAKV.Equals(makv)).OrderBy(c => c.STT).ToList();
        }
        
        public List<DOTINHD> GetListKVDDNotP7(string makvpo)
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Substring(0, 2).Equals("DD") && p.MAKVPO.Equals(makvpo) 
                && p.MADOTIN != "DDP7D1").OrderBy(c => c.STT).ToList();
        }

        public List<DOTINHD> GetListKVDDP7(string makvpo)
        {
            return _db.DOTINHDs.Where(p => p.MADOTIN.Substring(0, 2).Equals("DD") && p.MAKVPO.Equals(makvpo))
                .OrderBy(c => c.STT).ToList();
        }
       
        public int Count( )
        {
            return _db.DOTINHDs.Count();
        }


    }
}
