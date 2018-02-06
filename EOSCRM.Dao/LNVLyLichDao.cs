using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LNVLyLichDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];


        public LNVLyLichDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }

        public LNVLYLICH Get(string ma)
        {
            return _db.LNVLYLICHes.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LNVLYLICH GetMaNV(string ma)
        {
            return _db.LNVLYLICHes.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LNVLYLICH GetNVTD(string manv, string matd)
        {
            return _db.LNVLYLICHes.Where(p => p.MANVLL.Equals(manv) && p.MANVLL.Equals(matd)).SingleOrDefault();
        }

        public List<LNVLYLICH> GetList()
        {
            return _db.LNVLYLICHes.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LNVLYLICH> GetListNV(string manv)
        {
            return _db.LNVLYLICHes.Where(p => p.MANVLL.Equals(manv)).ToList();
        }

        public int Count()
        {
            return _db.LNVLYLICHes.Count();
        }

        public string NewId()
        {
            var query = _db.LNVLYLICHes.Max(p => p.MANVLL);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");
            }

            return "000001";
        }


        public Message Insert(LNVLYLICH objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LNVLYLICHes.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MANVLL,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NNVLL",
                    MOTA = "Nhập lý lịch nhân viên."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "lý lịch");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "lý lịch");
            }
            return msg;
        }

        public Message Update(LNVLYLICH objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetMaNV(objUi.MANVLL);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.HOTENKS = objUi.HOTENKS;
                    objDb.TENTHUONGGOI = objUi.TENTHUONGGOI;
                    objDb.TENGOIKHAC = objUi.TENGOIKHAC;
                    objDb.NGAYSINH = objUi.NGAYSINH;
                    objDb.GIOITINH = objUi.GIOITINH;
                    objDb.NOISINH = objUi.NOISINH;
                    objDb.QUEQUAN = objUi.QUEQUAN;
                    objDb.NOIO = objUi.NOIO;
                    objDb.MADT = objUi.MADT;
                    objDb.MATG = objUi.MATG;
                    objDb.MATPXT = objUi.MATPXT;
                    objDb.MATD = objUi.MATD;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.MANVN = objUi.MANVN;
                    objDb.NGAYN = objUi.NGAYN;
                    objDb.NGAYUP = objUi.NGAYUP;
                    objDb.NAMSINH = objUi.NAMSINH;
                    objDb.NGHETRUOCTD = objUi.NGHETRUOCTD;
                    objDb.SOTRUONGCT = objUi.SOTRUONGCT;
                               
                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MANVLL,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UNVLL",
                        MOTA = "Cập nhật lý lịch nhân viên."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up lý lịch");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up lý lịch");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up lý lịch ", objUi.MANVLL);
            }
            return msg;
        }

    }
}
