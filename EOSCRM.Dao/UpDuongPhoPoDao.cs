using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class UpDuongPhoPoDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public UpDuongPhoPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public UPDUONGPHOPO Get(string maup)
        {
            return _db.UPDUONGPHOPOs.Where(p => p.MAUPDP.Equals(maup)).SingleOrDefault();
        }

        public List<UPDUONGPHOPO> Search(string key)
        {
            return _db.UPDUONGPHOPOs.Where(p => p.TENFILE.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<UPDUONGPHOPO> GetList()
        {
            return _db.UPDUONGPHOPOs.OrderBy(c => c.NGAYN).ToList();
        }

        public List<UPDUONGPHOPO> GetListKV(string makv)
        {
            return _db.UPDUONGPHOPOs.Where(p => p.MAKVPO.Equals(makv)).OrderByDescending(c => c.NGAYN).ToList();
        }

        public Message Insert(UPDUONGPHOPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.UPDUONGPHOPOs.InsertOnSubmit(objUi);
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
                    MATT = "UPFILEDPPO",
                    MOTA = "Up file đường phố điện."
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

        public Message Update(UPDUONGPHOPO objUi, String useragent, String ipAddress, String sManv)
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
                        MATT = "SUAFILEDPPO",
                        MOTA = "Sửa file đường phố điện."
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
            var query = (from p in _db.UPDUONGPHOPOs select p.MAUPDP).Max();

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
