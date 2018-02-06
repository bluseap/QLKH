using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class UpDuongPhoDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public UpDuongPhoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public UPDUONGPHO Get(string maup)
        {
            return _db.UPDUONGPHOs.Where(p => p.MAUPDP.Equals(maup)).SingleOrDefault();
        }

        public List<UPDUONGPHO> Search(string key)
        {
            return _db.UPDUONGPHOs.Where(p => p.TENFILE.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<UPDUONGPHO> GetList()
        {
            return _db.UPDUONGPHOs.OrderBy(c => c.NGAYN).ToList();
        }

        public List<UPDUONGPHO> GetListKV(string makv)
        {
            return _db.UPDUONGPHOs.Where(p => p.MAKV.Equals(makv)).OrderByDescending(c => c.NGAYN).ToList();
        }

        public Message Insert(UPDUONGPHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.UPDUONGPHOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                _db.Connection.Close();
                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAUPDP,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "UPFILEDP",
                    MOTA = "Up file đường phố."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Up DP");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "Up DP", objUi.MAUPDP);
            }
            return msg;
        }

        public Message Update(UPDUONGPHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAUPDP);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENFILE = objUi.TENFILE;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAUPDP,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.I.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "SUAFILEDP",
                        MOTA = "Sửa file đường phố."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up DP");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up DP", objUi.TENFILE);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up DP");
            }
            return msg;
        }

        public string NewId()
        {     
            var query = (from p in _db.UPDUONGPHOs select p.MAUPDP).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                query = temp.ToString("D8");
            }
            else
            {
                query = "00000001";
            }

            return query;
        }

    }
}
