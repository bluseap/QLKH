using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class HeSoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        
        public HeSoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public HESO Get(string ma)
        {
            return _db.HESOs.Where(p => p.MAHS.Equals(ma)).SingleOrDefault();
        }

        public HESOKH GetHSKH(string ma)
        {
            return _db.HESOKHs.Where(p => p.MAHS.Equals(ma)).SingleOrDefault();
        }

        public List<HESO> GetList()
        {
            return _db.HESOs.ToList();
        }

        public List<HESOKH> GetListHSKH()
        {
            return _db.HESOKHs.ToList();
        }
        
        /// <summary>
        /// Update list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="useragent"></param>
        /// <param name="ipAddress"></param>
        /// <param name="sManv"></param>
        /// <returns></returns>
        public Message UpdateList(List<HESO> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in objList)
                {
                    var objDb = Get(obj.MAHS);

                    // update when has changes
                    if( objDb.TENHS == obj.TENHS && objDb.GIATRI.Equals(obj.GIATRI) && objDb.NGAYAD.Equals(obj.NGAYAD)) continue;

                    objDb.TENHS = obj.TENHS;
                    objDb.GIATRI = obj.GIATRI;
                    objDb.NGAYAD = obj.NGAYAD;

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.MAHS,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = CHUCNANGKYDUYET.CT01.ToString(),
                        MOTA = "Cập nhật hệ số thiết kế"
                    };

                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    #endregion

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, "danh sách", "hệ số thiết kế");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách hệ số thiết kế");
            }

            return msg;
        }

        /// <summary>
        /// Update list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="useragent"></param>
        /// <param name="ipAddress"></param>
        /// <param name="sManv"></param>
        /// <returns></returns>
        public Message UpdateListHSKH(List<HESOKH> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in objList)
                {
                    var objDb = GetHSKH(obj.MAHS);

                    // update when has changes
                    if (objDb.TENHS == obj.TENHS && objDb.GIATRI.Equals(obj.GIATRI) && objDb.NGAYAD.Equals(obj.NGAYAD)) continue;

                    objDb.TENHS = obj.TENHS;
                    objDb.GIATRI = obj.GIATRI;
                    objDb.NGAYAD = obj.NGAYAD;

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.MAHS,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = CHUCNANGKYDUYET.CT01.ToString(),
                        MOTA = "Cập nhật hệ số khách hàng"
                    };

                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    #endregion

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, "danh sách", "hệ số khách hàng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách hệ số khách hàng");
            }

            return msg;
        }
    }
}
