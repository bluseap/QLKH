using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LDaoTaoBDDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly POWACOSADataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTIONPOWACO];
        public LDaoTaoBDDao()
        {
            _db = new POWACOSADataContext(Connectionstring);
        }
        
        public LDAOTAOBD Get(string ma)
        {
            return _db.LDAOTAOBDs.Where(p => p.MADTBD.Equals(ma)).SingleOrDefault();
        }

        public LDAOTAOBD GetMaNV(string ma)
        {
            return _db.LDAOTAOBDs.Where(p => p.MANVLL.Equals(ma)).SingleOrDefault();
        }

        public LDAOTAOBD GetNVTD(string manv, string matd)
        {
            return _db.LDAOTAOBDs.Where(p => p.MANVLL.Equals(manv) && p.MADTBD.Equals(matd)).SingleOrDefault();
        }

        public List<LDAOTAOBD> GetList()
        {
            return _db.LDAOTAOBDs.Where(p => p.XOA.Equals(false)).ToList();
        }

        public List<LDAOTAOBD> GetListNV(string manv)
        {
            return _db.LDAOTAOBDs.Where(p => p.MANVLL.Equals(manv)).ToList();
        }

        public List<LDAOTAOBD> Search(string key)
        {
            return _db.LDAOTAOBDs.Where(p => p.MADTBD.ToUpper().Contains(key.ToUpper())).ToList();
        }        

        public int Count( )
        {
            return _db.LDAOTAOBDs.Count();
        }

        public string NewId()
        {
            var query = _db.LDAOTAOBDs.Max(p => p.MADTBD);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");                
            }

            return "000001";
        }

        public Message Insert(LDAOTAOBD objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.LDAOTAOBDs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADTBD,
                    IPAddress = "11",
                    MANV = objUi.MANVN,
                    UserAgent = "11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "NDTBD",
                    MOTA = "Nhập đào tạo, bồi dưỡng."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "ĐT - BD");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "ĐT - BD");
            }
            return msg;
        }

        public Message Update(LDAOTAOBD objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = GetNVTD(objUi.MANVLL, objUi.MADTBD);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MALOAIDTBD = objUi.MALOAIDTBD;
                    objDb.NGAYBD = objUi.NGAYBD;
                    objDb.NGAYKT = objUi.NGAYKT;
                    objDb.MALOAIBC = objUi.MALOAIBC;
                    objDb.MACHEDOHOC = objUi.MACHEDOHOC;
                    objDb.CHUYENNGANH = objUi.CHUYENNGANH;
                    objDb.TENTRUONG = objUi.TENTRUONG;
                    objDb.DCDAOTAOBD = objUi.DCDAOTAOBD;
                    objDb.NOIDUNGDTBD = objUi.NOIDUNGDTBD;

                    objDb.NGAYUP = objUi.NGAYUP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADTBD,
                        IPAddress = "11",
                        MANV = objUi.MANVN,
                        UserAgent = "11",
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "UDTBD",
                        MOTA = "Cập nhật đào tạo - bồi dưỡng."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Up ĐT - BD");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Up ĐT - BD");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Up ĐT - BD ", objUi.MADTBD);
            }
            return msg;
        }

    }
}
