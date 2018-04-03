using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class TachDuongPoDao
    {
        private readonly EOSCRMDataContext _db;

        private readonly DMDotInHDDao _dmdiDao = new DMDotInHDDao();
        private readonly DotInHDDao _dihdDao = new DotInHDDao();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public TachDuongPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public TACHDUONGPO Get(string matach)
        {
            return _db.TACHDUONGPOs.Where(p => p.IDTACH.Equals(matach)).SingleOrDefault();
        }

        public List<TACHDUONGPO> GetListMAKV(string makv)
        {
            return _db.TACHDUONGPOs.Where(p => p.MAKVPO.Equals(makv)).ToList();
        }

        public List<TACHDUONGPO> GetListKyMAKV(int thang, int nam, string makv)
        {
            return _db.TACHDUONGPOs.Where(p => p.THANG.Equals(thang) && p.NAM.Equals(nam) && p.MAKVPO.Equals(makv))
                .OrderBy(p => p.MADPPOCU).OrderBy(p => p.MADBPOCU)
                .ToList();
        }

        public List<TACHDUONGPO> GetListKyMAKVDotIn(int thang, int nam, string makv, string madotin)
        {
            var dotin = _dihdDao.Get(madotin);
            var dotinp7 = _dihdDao.GetKVP7D1("DDP7D1", makv);

            if (dotin.MADOTIN == "DDP7D1")
            {
                var query = from tp in _db.TACHDUONGPOs
                            join dp in _db.DUONGPHOPOs on tp.MADPPOCU equals dp.MADPPO
                            join kh in _db.KHACHHANGPOs on tp.IDKHPO equals kh.IDKHPO
                            where tp.THANG.Equals(thang) && tp.NAM.Equals(nam) && tp.MAKVPO.Equals(makv) && kh.DOTINHD.Equals(madotin)
                            select tp;

                return query.ToList();
            }
            else
            {
                var query = from tp in _db.TACHDUONGPOs
                            join dp in _db.DUONGPHOPOs on tp.MADPPOCU equals dp.MADPPO
                            join kh in _db.KHACHHANGPOs on tp.IDKHPO equals kh.IDKHPO
                            where tp.THANG.Equals(thang) && tp.NAM.Equals(nam) && tp.MAKVPO.Equals(makv) && dp.IDMADOTIN.Equals(madotin)
                                && (kh.DOTINHD != dotinp7.IDMADOTIN || kh.DOTINHD == null)
                            select tp;

                return query.ToList();
            }
        }

        public string NewId()
        {
            var query = (from p in _db.TACHDUONGPOs select p.IDTACH).Max();

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

        public Message Insert(TACHDUONGPO objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.TACHDUONGPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                _db.Connection.Close();
                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "tách đường ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "cấp bậc", objUi.IDKHPO);
            }
            return msg;
        }

        public Message Update(TACHDUONGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.IDTACH);

                objDb.MADPPOMOI = objUi.MADPPOMOI;
                objDb.MADBPOMOI = objUi.MADBPOMOI;                                

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
                        MATT = "SUATDPO",
                        MOTA = "Sửa TĐ điện."+ objUi.MADPPOMOI + objUi.MADBPOMOI
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
