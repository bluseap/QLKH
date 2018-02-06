using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class KHSoNoDHPoDao
    {private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KHSoNoDHPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHSONODHPO Get(int id)
        {
            return _db.KHSONODHPOs.FirstOrDefault(p => p.IDSONO.Equals(id));
        }

        public KHSONODHPO GetIDKH(string idkh)
        {
            return _db.KHSONODHPOs.FirstOrDefault(p => p.IDKHPO.Equals(idkh));
        }

        public KHSONODHPO GetKyIDKV(int nam, int thang, string idkh, string makv)
        {
            return _db.KHSONODHPOs.FirstOrDefault(p => p.NAM.Equals(nam) && p.THANG.Equals(thang) && p.IDKHPO.Equals(idkh) && p.MAKVPO.Equals(makv));
        }

        public List<KHSONODHPO> GetList()
        {
            return _db.KHSONODHPOs.ToList();
        }

        public List<KHSONODHPO> GetListKy(int nam, int thang)
        {
            return _db.KHSONODHPOs.Where(hn => hn.THANG.Equals(thang) && hn.NAM.Equals(nam)).ToList();
        }

        public List<KHSONODHPO> GetListKyKV(int nam, int thang, string makv)
        {
            return _db.KHSONODHPOs.Where(hn => hn.THANG.Equals(thang) && hn.NAM.Equals(nam) && hn.MAKVPO.Equals(makv)).ToList();
        }

        public List<KHSONODHPO> GetListKV(string makv)
        {
            return _db.KHSONODHPOs.Where(hn => hn.MAKVPO.Equals(makv)).ToList();
        }

        public Message Insert(KHSONODHPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.KHSONODHPOs.InsertOnSubmit(objUi);             

                /*var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKHPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "SUASONOKHPO",
                    MOTA = "Nhập số No khách hàng điện khi khai thác nhầm."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);*/
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thêm số No KH ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thêm số No KH ");
            }
            return msg;
        }

        public Message Update(KHSONODHPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.IDSONO);

                if (objDb != null)
                {
                    //so no khach hang
                    objDb.MADHPOMOI = objUi.MADHPOMOI;
                    objDb.SONOMOI = objUi.SONOMOI;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.NGAYN = objUi.NGAYN;
                    objDb.MANVN = objUi.MANVN;                   

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "SUASONOKHUPO",
                        MOTA = "Sửa số No khách hàng điện."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "sửa số No KH ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "sửa số No KH ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "sửa số No KH ");
            }
            return msg;
        }
    }
}
