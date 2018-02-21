using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class SuaChuaDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public SuaChuaDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public SUACHUA Get(string ma)
        {
            return _db.SUACHUAs.Where(p => p.ID.Equals(ma)).SingleOrDefault();
        }

        public List<SUACHUA> GetList()
        {
            return _db.SUACHUAs.OrderByDescending(p => p.NGAYN).ToList();
        }

        public List<SUACHUA> GetListKV(string makv)
        {
            return _db.SUACHUAs.Where(p => p.KHUVUCID.Equals(makv)).OrderByDescending(p => p.NGAYN).ToList();
        }

        public List<SUACHUA> GetListDBTEN(string sodb, string ten, string makv)
        {
            var query = _db.SUACHUAs.Where(p => p.KHUVUCID.Equals(makv)).AsQueryable();

            if (!string.IsNullOrEmpty(sodb))
                query = query.Where(d => (d.MADP+d.MADB).Contains(sodb));

            if (!string.IsNullOrEmpty(ten))
                query = query.Where(d => d.TENKH.Contains(ten));

            return query.OrderByDescending(p => p.NGAYN).ToList();
        }

        public string SuaChuaNewID(string khuvucID)
        {
            var query = _db.SUACHUAs.Max(p => p.ID.Substring(1,6));

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query.Substring(1, 6)) + 1;
                query = query.Substring(0,1) + temp.ToString("D6");
            }
            else
            {
                query = khuvucID + "000001";
            }

            return query;
        }
        
        public int Count( )
        {
            return _db.SUACHUAs.Count();
        }

        public Message Insert(SUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.SUACHUAs.InsertOnSubmit(objUi);               

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.ID.ToString(),
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "SUACHUA",
                    MOTA = "Nhập sữa đồng hồ."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "sữa chữa");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "sữa chữa");
            }
            return msg;
        }

        public Message Update(SUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.ID);

                if (objDb != null)
                {
                    //TODO: update all fields
                    //objDb.TTSUA = objUi.TTSUA;
                    //objDb.TTSUATK = objUi.TTSUATK;
                    //objDb.TTSUACT = objUi.TTSUACT;
                    //objDb.TTSUAHD = objUi.TTSUAHD;
                    //objDb.TTSUATC = objUi.TTSUATC;
                    //objDb.TTSUANT = objUi.TTSUANT;                   

                    objDb.NGAYUP = objUi.NGAYUP;
                    objDb.MANVUP = objUi.MANVUP;   

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.ID,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "SUACHUAUP",
                        MOTA = "Update sửa đồng hồ khách hàng."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thông tin kh");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "thông tin kh");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thông tin kh");
            }
            return msg;
        }

    }
}
