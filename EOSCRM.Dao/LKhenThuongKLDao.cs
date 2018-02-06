using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LKhenThuongKLDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LKhenThuongKLDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LKHENTKL Get(string ma)
        {
            return _db.LKHENTKLs.Where(p => p.MAKTKL.Equals(ma)).SingleOrDefault();
        }

        public LKHENTKL GetMaNV(string ma)
        {
            return _db.LKHENTKLs.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LKHENTKL GetNVTD(string manv, string matd)
        {
            return _db.LKHENTKLs.Where(p => p.MANVLL.Equals(manv) && p.MAKTKL.Equals(matd)).SingleOrDefault();
        }

        public List<LKHENTKL> GetList()
        {
            return _db.LKHENTKLs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LKHENTKL> GetListNV(string manv)
        {
            return _db.LKHENTKLs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }               

        public int Count( )
        {
            return _db.LKHENTKLs.Count();
        }

        public string NewId()
        {
            var query = _db.LKHENTKLs.Max(p => p.MAKTKL);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }


        public Message Insert(LKHENTKL objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LKHENTKLs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAKTKL,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NKTKL",
                    MOTA = "Nhập khen thưởng kỷ luật."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "KT - KL");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "KT - KL");
            }
            return msg;
        }

        public Message Update(LKHENTKL objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MAKTKL);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MALKLKT = objUi.MALKLKT;
                    objDb.TUNGAY = objUi.TUNGAY;
                    objDb.DENNGAY = objUi.DENNGAY;
                    objDb.SOQD = objUi.SOQD;
                    objDb.NOIDUNG = objUi.NOIDUNG; 
                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAKTKL,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UKTKL",
                        MOTA = "Cập nhật khen thưởng kỷ luật."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up KT - KL");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up KT - KL");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up KT - KL ", objUi.MAKTKL);
            }
            return msg;
        }
    }
}
