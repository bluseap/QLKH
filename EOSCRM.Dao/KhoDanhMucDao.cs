using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class KhoDanhMucDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public KhoDanhMucDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KhoDanhMuc Get(string id)
        {
            return _db.KhoDanhMucs.Where(p => p.Id.Equals(id)).SingleOrDefault();
        }        

        public List<KhoDanhMuc> Search(string key)
        {
            return _db.KhoDanhMucs.Where(p => p.TenKho.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<KhoDanhMuc> GetList()
        {
            return _db.KhoDanhMucs.OrderBy(c => c.SoThuTu).ToList();
        }

        public List<KhoDanhMuc> GetListXiNghiep(string makvxn)
        {
            return _db.KhoDanhMucs.Where(p => p.KhuVucId.Equals(makvxn) )
                .OrderBy(c => c.SoThuTu).ToList();
        }

        public List<KhoDanhMuc> GetListXiNghiepLoaiVatTu(string makvxn, string maloaivt)
        {
            return _db.KhoDanhMucs.Where(p => p.KhuVucId.Equals(makvxn) && p.LoaiVatTuId.Equals(maloaivt))
                .OrderBy(c => c.SoThuTu).ToList();
        }

        public string NewId()
        {         
            var query = (from p in _db.KhoDanhMucs where p.Id.Substring(0,2).Equals("KH") select p.Id).Max();            

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query.Substring(2, 4)) + 1;
                query = "KH" + temp.ToString("D4");
            }
            else
            {
                query =  "KH0001";
            }

            return query;
        }

    }
}
