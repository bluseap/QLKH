using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class KyTietKiemDao
    {
        private readonly ReportClass _rpClass = new ReportClass();

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KyTietKiemDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KYTIETKIEMCD  Get(string ma)
        {
            return _db.KYTIETKIEMCDs.Where(p => p.MANVV.Equals(ma)).SingleOrDefault();
        }

        public List<KYTIETKIEMCD> GetListKyTietKiem(int nam, int thang)
        {
            return _db.KYTIETKIEMCDs.Where(p => p.NAM.Equals(nam) && p.THANG.Equals(thang)).ToList();
        }

        public string GetSumTien(string ma)
        {
            var query = from ktk in _db.KYTIETKIEMCDs
                         where ktk.MANVV.Equals(ma)
                         //group ktk by ktk.MANVV into g
                         //select new { SOTIEN = g.Sum(m => m.SOTIEN)};
                        select  ktk;


            return query.Sum(m => m.SOTIEN).ToString();
        }

        public string GetSumTienThu(string ma)
        {
            var query = from ktk in _db.KYTIETKIEMCDs
                        //where ktk.MANVV.Equals(ma)
                        //group ktk by ktk.MANVV into g
                        //select new { SOTIEN = g.Sum(m => m.SOTIEN)};
                        select ktk;


            return query.Sum(m => m.SOTIEN).ToString();
        }

        public string GetSumTienThu2()
        {
            var query = from ktk in _db.KYTIETKIEMCDs
                        //where ktk.MANVV.Equals(ma)
                        //group ktk by ktk.MANVV into g
                        //select new { SOTIEN = g.Sum(m => m.SOTIEN)};
                        select ktk;


            return query.Sum(m => m.SOTIEN).ToString();
        }

        

    }
}
