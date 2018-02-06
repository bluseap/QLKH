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
    public class HisNgayDangKyDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public HisNgayDangKyDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public HISNGAYDANGKY Get(string ma)
        {
            return _db.HISNGAYDANGKies.Where(p => p.MALUOCSU.Equals(ma)).SingleOrDefault();
        }

        public HISNGAYDANGKY GetMADDK(string madon)
        {
            return _db.HISNGAYDANGKies.Where(p => p.MADDK.Equals(madon)).SingleOrDefault();
        }

        public HISNGAYDANGKY GetMADDKMoTa(string madon, string mota)
        {
            return _db.HISNGAYDANGKies.Where(p => p.MADDK.Equals(madon) && p.MOTATTDON.Equals(mota)).SingleOrDefault();
        }

        public HISNGAYDANGKY GetMaxMADDKMoTa(string madon, string mota)
        {
            var query = _db.HISNGAYDANGKies.Where(p => p.MALUOCSU.Equals(MaxMADDK(madon, mota)));

            return query.SingleOrDefault();
        }

        public List<HISNGAYDANGKY> GetList(string makv)
        {
            return _db.HISNGAYDANGKies.Where(p => p.MAKV.Equals(makv)).OrderByDescending(hd => hd.MADDK).ToList();
        }

        public int MaxMADDK(string madon, string mota)
        {
            var query = _db.HISNGAYDANGKies.Where(p => p.MADDK.Equals(madon) && p.MOTATTDON.Equals(mota)).Max(p => p.MALUOCSU);

            return query;
        }

    }
}
