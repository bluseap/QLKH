using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class VatTuKeToanBravoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public VatTuKeToanBravoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public VatTuKeToanBravo Get(string ma)
        {
            return _db.VatTuKeToanBravos.Where(p => p.Id.Equals(ma)).SingleOrDefault();
        }

        public List<VatTuKeToanBravo> SearchDonVi(string key, string madonvi)
        {
            return _db.VatTuKeToanBravos
                .Where(p => p.DonViId.Equals(madonvi) && p.TenVatTu.ToUpper().Contains(key.ToUpper()))
                .OrderByDescending(p => p.NgayTaoBravo)
                .ToList();
        }

        public List<VatTuKeToanBravo> Search(string key)
        {
            return _db.VatTuKeToanBravos.Where(p => p.TenVatTu.ToUpper().Contains(key.ToUpper()))
                .OrderByDescending(p => p.NgayTaoBravo)
                .ToList();
        }

        public List<VatTuKeToanBravo> GetList()
        {
            return _db.VatTuKeToanBravos
                .OrderByDescending(p => p.NgayTaoBravo)
                .ToList();
        }

        public List<VatTuKeToanBravo> GetListDonVi(string madonvi)
        {
            return _db.VatTuKeToanBravos.Where(p => p.DonViId.Equals(madonvi))
                .OrderByDescending(p => p.NgayTaoBravo)
                .ToList();
        }

        public List<VatTuKeToanBravo> GetListKoDonVi(string madonvi)
        {
            return _db.VatTuKeToanBravos.Where(p => p.DonViId != madonvi )
                .OrderByDescending(p => p.NgayTaoBravo)
                .ToList();
        }
        



    }
}
