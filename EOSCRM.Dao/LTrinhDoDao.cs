using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LTrinhDoDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LTrinhDoDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LTRINHDO Get(string ma)
        {
            return _db.LTRINHDOs.Where(p => p.MATD.Equals(ma)).SingleOrDefault();
        }

        public LTRINHDO GetMaNV(string ma)
        {
            return _db.LTRINHDOs.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LTRINHDO GetNVTD(string manv, string matd)
        {
            return _db.LTRINHDOs.Where(p => p.MANVLL.Equals(manv) && p.MATD.Equals(matd)).SingleOrDefault();
        }

        public LTRINHDO GetHinhM1(string hinh)
        {
            return _db.LTRINHDOs.Where(p => p.HINHBC1.Equals(hinh)).SingleOrDefault();
        }

        public LTRINHDO GetHinhM2(string hinh)
        {
            return _db.LTRINHDOs.Where(p => p.HINHBC2.Equals(hinh)).SingleOrDefault();
        }

        public List<LTRINHDO> GetList()
        {
            return _db.LTRINHDOs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LTRINHDO> GetListNV(string manv)
        {
            return _db.LTRINHDOs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }

        public List<LTRINHDO> Search(string key)
        {
            return _db.LTRINHDOs.Where(p => p.MATD.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LTRINHDOs.Count();
        }

        public string NewId()
        {
            var query = _db.LTRINHDOs.Max(p => p.MATD);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }

        public Message Insert(LTRINHDO objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LTRINHDOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MATD,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NTRDO",
                    MOTA = "Nhập trình độ."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "trình độ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "trình độ");
            }
            return msg;
        }

        public Message Update(LTRINHDO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MATD);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MALOAIBC = objUi.MALOAIBC;
                    objDb.CHUYENNGANH = objUi.CHUYENNGANH;
                    objDb.NGAYCAP = objUi.NGAYCAP;                    
                    objDb.TENTRUONG = objUi.TENTRUONG;
                    objDb.MACHEDOHOC = objUi.MACHEDOHOC;
                    objDb.HINHBC1 = objUi.HINHBC1;
                    objDb.HINHBC2 = objUi.HINHBC2;

                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MATD,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UTRDO",
                        MOTA = "Cập nhật trình độ."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up trình độ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up trình độ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up trình độ ", objUi.MATD);
            }
            return msg;
        }


    }
}
