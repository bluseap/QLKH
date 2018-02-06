using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class SuaDongHoPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public SuaDongHoPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public SUADONGHOPO Get(string ma)
        {
            return _db.SUADONGHOPOs.Where(p => p.IDSDHPO.Equals(ma)).SingleOrDefault();
        }

        public SUADONGHOPO GetMaDDK(string madon, string ids)
        {
            return _db.SUADONGHOPOs.Where(p => p.MADDKPO.Equals(madon) && p.IDSDHPO.Equals(ids)).SingleOrDefault();
        }

        public List<SUADONGHOPO> GetList()
        {
            return _db.SUADONGHOPOs.OrderByDescending(hd => hd.NGAYNHAP).ToList();
        }

        public List<SUADONGHOPO> GetListKV(string makv)
        {
            var query = _db.SUADONGHOPOs.Where(s => s.DONDANGKYPO.MAKVPO.Equals(makv))
                .OrderByDescending(hd => hd.NGAYNHAP);

            return query.ToList();
        }

        public List<SUADONGHOPO> GetListMaDDK(string madon)
        {
            return _db.SUADONGHOPOs.OrderByDescending(hd => hd.MADDKPO.Equals(madon)).ToList();
        }

        public Message Insert(SUADONGHOPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();

                _db.SUADONGHOPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "INDHPO",
                    MOTA = "Sửa đồng hồ điện."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // commit
                trans.Commit();
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "sửa đồng hồ ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "sửa đồng hồ ", objUi.MADDKPO);
            }
            return msg;
        }
    }
}
