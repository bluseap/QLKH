using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class LSucKhoeDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LSucKhoeDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LSUCKHOE Get(string ma)
        {
            return _db.LSUCKHOEs.Where(p => p.MASUCKHOE.Equals(ma)).SingleOrDefault();
        }

        public LSUCKHOE GetMaNV(string ma)
        {
            return _db.LSUCKHOEs.Where(p => p.MASUCKHOE.Equals(ma)).SingleOrDefault();
        }

        public LSUCKHOE GetNVTD(string manv, string matd)
        {
            return _db.LSUCKHOEs.Where(p => p.MANVLL.Equals(manv) && p.MASUCKHOE.Equals(matd)).SingleOrDefault();
        }

        public List<LSUCKHOE> GetList()
        {
            return _db.LSUCKHOEs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LSUCKHOE> GetListNV(string manv)
        {
            return _db.LSUCKHOEs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }               

        public int Count( )
        {
            return _db.LSUCKHOEs.Count();
        }

        public string NewId()
        {
            var query = _db.LSUCKHOEs.Max(p => p.MASUCKHOE);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }


        public Message Insert(LSUCKHOE objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LSUCKHOEs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MASUCKHOE,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NSUCKHOE",
                    MOTA = "Nhập quá trình sức khỏe."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "sức khỏe");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "sức khỏe");
            }
            return msg;
        }

        public Message Update(LSUCKHOE objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MASUCKHOE);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CHIEUCAO = objUi.CHIEUCAO;
                    objDb.CANNANG = objUi.CANNANG;
                    objDb.TINHTRANG = objUi.TINHTRANG;
                    objDb.BANHMANTINH = objUi.BANHMANTINH; 
                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MASUCKHOE,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "USUCKHOE",
                        MOTA = "Cập nhật quá trình sức khỏe."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up sức khỏe");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up sức khỏe");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up sức khỏe ", objUi.MASUCKHOE);
            }
            return msg;
        }
    }
}
