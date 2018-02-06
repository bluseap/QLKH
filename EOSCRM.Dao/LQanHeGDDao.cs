using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LQanHeGDDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LQanHeGDDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LQUANHEGD Get(string ma)
        {
            return _db.LQUANHEGDs.Where(p => p.MAQHGD.Equals(ma)).SingleOrDefault();
        }

        public LQUANHEGD GetMaNV(string ma)
        {
            return _db.LQUANHEGDs.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LQUANHEGD GetNVTD(string manv, string matd)
        {
            return _db.LQUANHEGDs.Where(p => p.MANVLL.Equals(manv) && p.MAQHGD.Equals(matd)).SingleOrDefault();
        }

        public List<LQUANHEGD> GetList()
        {
            return _db.LQUANHEGDs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LQUANHEGD> GetListNV(string manv)
        {
            return _db.LQUANHEGDs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }

        public List<LQUANHEGD> Search(string key)
        {
            return _db.LQUANHEGDs.Where(p => p.MAQHGD.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LQUANHEGDs.Count();
        }

        public string NewId()
        {
            var query = _db.LQUANHEGDs.Max(p => p.MAQHGD);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }

        public Message Insert(LQUANHEGD objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LQUANHEGDs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAQHGD,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NQHGD",
                    MOTA = "Nhập quan hệ gia đình."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "QH - GĐ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "QH - GĐ");
            }
            return msg;
        }

        public Message Update(LQUANHEGD objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MAQHGD);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MALQHGD = objUi.MALQHGD;
                    objDb.TEN = objUi.TEN;
                    objDb.NGAYSINH = objUi.NGAYSINH;
                    objDb.NAMSINH = objUi.NAMSINH;
                    objDb.QUEQUAN = objUi.QUEQUAN;
                    objDb.MADT = objUi.MADT;
                    objDb.MATG = objUi.MATG;
                    objDb.NGHE = objUi.NGHE;
                    objDb.DVCONGTAC = objUi.DVCONGTAC;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.MANVN = objUi.MANVN;
                    objDb.NGAYN = objUi.NGAYN;  
                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAQHGD,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UQHGD",
                        MOTA = "Cập nhật QH - GĐ."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up QH - GĐ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up ĐT - BD");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up QH - GĐ ", objUi.MAQHGD);
            }
            return msg;
        }

    }
}
