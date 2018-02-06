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
    public class SuaDongHoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public SuaDongHoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public SUADONGHO Get(string ma)
        {
            return _db.SUADONGHOs.Where(p => p.IDSDH.Equals(ma)).SingleOrDefault();
        }

        public SUADONGHO GetMaDDK(string madon, string ids)
        {
            return _db.SUADONGHOs.Where(p => p.MADDK.Equals(madon) && p.IDSDH.Equals(ids)).SingleOrDefault();
        }

        public List<SUADONGHO> GetList()
        {
            return _db.SUADONGHOs.OrderByDescending(hd => hd.NGAYNHAP).ToList();
        }

        public List<SUADONGHO> GetListKV(string makv)
        {
            var query = _db.SUADONGHOs.Where(s => s.DONDANGKY.MAKV.Equals(makv))
                .OrderByDescending(hd => hd.NGAYNHAP);

            return query.ToList();
        }

        public List<SUADONGHO> GetListMaDDK(string madon)
        {
            return _db.SUADONGHOs.OrderByDescending(hd => hd.MADDK.Equals(madon)).ToList();
        }

        public Message Insert(SUADONGHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();

                _db.SUADONGHOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "INDHN",
                    MOTA = "Sửa đồng hồ nước."
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

                msg = ExceptionHandler.HandleInsertException(ex, "sửa đồng hồ ", objUi.MADDK);
            }
            return msg;
        }
    }
}
