using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.Common;

using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class DuyetQuyenDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DuyetQuyenDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public DUYET_QUYEN Get(string ma)
        {
            return _db.DUYET_QUYENs.Where(p => p.MADDK.Equals(ma)).SingleOrDefault();
        }

        public List<DUYET_QUYEN> GetList()
        {
            return _db.DUYET_QUYENs.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DUYET_QUYEN> GetListPB(String mddk, String mapb)
        {
            return _db.DUYET_QUYENs.Where(prop => prop.MADDK.Equals(mddk) && prop.MAPB.Equals(mapb))
                .OrderByDescending(d => d.MADDK).OrderByDescending(d => d.MADDK).ToList();
        }

        public Message Insert(DUYET_QUYEN objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.DUYET_QUYENs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTDK.DK_A.ToString(),
                    MOTA = "Nhập duyệt phân quyền cho đơn vị."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn duyệt quyền");
            }
            catch (Exception ex)
            {
                // rollback transaction
                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "đơn đăng ký");
            }
            return msg;
        }



    }
}
