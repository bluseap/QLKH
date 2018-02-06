using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LNhapNguDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LNhapNguDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LNHAPNGU Get(string ma)
        {
            return _db.LNHAPNGUs.Where(p => p.MANNGU.Equals(ma)).SingleOrDefault();
        }

        public LNHAPNGU GetMaNV(string ma)
        {
            return _db.LNHAPNGUs.Where(p => p.MANNGU.Equals(ma)).SingleOrDefault();
        }

        public LNHAPNGU GetNVTD(string manv, string matd)
        {
            return _db.LNHAPNGUs.Where(p => p.MANVLL.Equals(manv) && p.MANNGU.Equals(matd)).SingleOrDefault();
        }

        public List<LNHAPNGU> GetList()
        {
            return _db.LNHAPNGUs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LNHAPNGU> GetListNV(string manv)
        {
            return _db.LNHAPNGUs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }               

        public int Count( )
        {
            return _db.LNHAPNGUs.Count();
        }

        public string NewId()
        {
            var query = _db.LNHAPNGUs.Max(p => p.MANNGU);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }


        public Message Insert(LNHAPNGU objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LNHAPNGUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MANNGU,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NNHAPNGU",
                    MOTA = "Nhập quá trình nhập ngũ."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nhập ngũ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "nhập ngũ");
            }
            return msg;
        }

        public Message Update(LNHAPNGU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MANNGU);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.NGAYBDNN = objUi.NGAYBDNN;
                    objDb.NGAYKTNN = objUi.NGAYKTNN;
                    objDb.QUANHAM = objUi.QUANHAM;
                    objDb.NOIDUNG = objUi.NOIDUNG; 
                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MANNGU,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UNHAPNGU",
                        MOTA = "Cập nhật quá trình nhập ngũ."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up nhập ngũ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up nhập ngũ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up nhập ngũ ", objUi.MANNGU);
            }
            return msg;
        }

    }
}
