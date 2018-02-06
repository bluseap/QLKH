using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class TachDuongNDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public TachDuongNDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public TACHDUONGN Get(string matach)
        {
            return _db.TACHDUONGNs.Where(p => p.IDTACH.Equals(matach)).SingleOrDefault();
        }

        public List<TACHDUONGN> GetListMAKV(string makv)
        {
            return _db.TACHDUONGNs.Where(p => p.MAKV.Equals(makv)).ToList();
        }

        public List<TACHDUONGN> GetListKyMAKV(int thang, int nam, string makv)
        {
            return _db.TACHDUONGNs.Where(p => p.THANG.Equals(thang) && p.NAM.Equals(nam) && p.MAKV.Equals(makv))
                .OrderBy(p => p.MADPCU).OrderBy(p => p.MADBCU)
                .ToList();
        }

        public List<TACHDUONGN> GetListKyMAKVDotIn(int thang, int nam, string makv, string madotin)
        {
            var query = from tp in _db.TACHDUONGNs
                        join dp in _db.DUONGPHOs on tp.MADPCU equals dp.MADP
                        where tp.THANG.Equals(thang) && tp.NAM.Equals(nam) && tp.MAKV.Equals(makv) && dp.IDMADOTIN.Equals(madotin)
                        select tp;

            return query.ToList();
        }

        public string NewId()
        {
            var query = (from p in _db.TACHDUONGNs select p.IDTACH).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = Int64.Parse(query) + 1;
                query = temp.ToString("D11");
            }
            else
            {
                query = "00000000001";
            }

            return query;
        }

        public Message Insert(TACHDUONGN objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.TACHDUONGNs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                _db.Connection.Close();
                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "tách đường ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "cấp bậc", objUi.IDKH);
            }
            return msg;
        }

        public Message Update(TACHDUONGN objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.IDTACH);

                objDb.MADPMOI = objUi.MADPMOI;
                objDb.MADBMOI = objUi.MADBMOI;                                

                if (objDb != null)
                {
                    //TODO: update all fields
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.IDTACH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "SUATDN",
                        MOTA = "Sửa TĐ nước."+ objUi.MADPMOI + objUi.MADBMOI
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "tách đường ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "tách đường ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "tách đường ");
            }
            return msg;
        }

    }
}
