using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LQTCongTacDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LQTCongTacDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LQTCONGTAC Get(string ma)
        {
            return _db.LQTCONGTACs.Where(p => p.MAQTCT.Equals(ma)).SingleOrDefault();
        }

        public LQTCONGTAC GetMaNV(string ma)
        {
            return _db.LQTCONGTACs.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LQTCONGTAC GetNVTD(string manv, string matd)
        {
            return _db.LQTCONGTACs.Where(p => p.MANVLL.Equals(manv) && p.MAQTCT.Equals(matd)).SingleOrDefault();
        }

        public List<LQTCONGTAC> GetList()
        {
            return _db.LQTCONGTACs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LQTCONGTAC> GetListNV(string manv)
        {
            return _db.LQTCONGTACs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }               

        public int Count( )
        {
            return _db.LQTCONGTACs.Count();
        }

        public string NewId()
        {
            var query = _db.LQTCONGTACs.Max(p => p.MAQTCT);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }

        
        public Message Insert(LQTCONGTAC objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LQTCONGTACs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAQTCT,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NCONGTAC",
                    MOTA = "Nhập quá trình công tác."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "công tác");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "công tác");
            }
            return msg;
        }

        public Message Update(LQTCONGTAC objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MAQTCT);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.NGAYBDCT = objUi.NGAYBDCT;
                    objDb.NGAYKTCT = objUi.NGAYKTCT;
                    objDb.NOIDUNG = objUi.NOIDUNG; 
                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAQTCT,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UCONGTAC",
                        MOTA = "Cập nhật quá trình công tác."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up công tác");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up công tác");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up công tác ", objUi.MAQTCT);
            }
            return msg;
        }
        

    }
}
