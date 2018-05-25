using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class KHSoNoDHDao
    {
        private readonly EOSCRMDataContext _db;

        private readonly KhachHangDao _khDao = new KhachHangDao();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KHSoNoDHDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHSONODH Get(int id)
        {
            return _db.KHSONODHs.FirstOrDefault(p => p.IDSONO.Equals(id));
        }

        public KHSONODH GetIDKH(string idkh)
        {
            return _db.KHSONODHs.FirstOrDefault(p => p.IDKH.Equals(idkh));
        }

        public KHSONODH GetKyIDKV(int nam, int thang, string idkh, string makv)
        {
            return _db.KHSONODHs.FirstOrDefault(p => p.NAM.Equals(nam) && p.THANG.Equals(thang) && p.IDKH.Equals(idkh) && p.MAKV.Equals(makv));
        }

        public List<KHSONODH> GetList()
        {
            return _db.KHSONODHs.OrderByDescending(p => p.IDSONO).ToList();
        }

        public List<KHSONODH> GetListKy(int nam, int thang)
        {
            return _db.KHSONODHs.Where(hn => hn.THANG.Equals(thang) && hn.NAM.Equals(nam)).ToList();
        }

        public List<KHSONODH> GetListKyKV(int nam, int thang, string makv)
        {
            return _db.KHSONODHs.Where(hn => hn.THANG.Equals(thang) && hn.NAM.Equals(nam) && hn.MAKV.Equals(makv)).OrderByDescending(p => p.IDSONO).ToList();
        }

        public List<KHSONODH> GetListKV(string makv)
        {
            return _db.KHSONODHs.Where(hn => hn.MAKV.Equals(makv)).ToList();
        }

        public Message Insert(KHSONODH objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.KHSONODHs.InsertOnSubmit(objUi);               

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "SUASONOKH",
                    MOTA = "Nhập số No khách hàng nước."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thêm số No KH ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thêm số No KH ");
            }
            return msg;
        }

        public Message Update(KHSONODH objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.IDSONO);                                  

                if (objDb != null)
                {
                    //so no khach hang
                    objDb.MADHMOI = objUi.MADHMOI;
                    objDb.SONOMOI = objUi.SONOMOI;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.NGAYN = objUi.NGAYN;
                    objDb.MANVN = objUi.MANVN;                                

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "SUASONOKHU",
                        MOTA = "Sửa số No khách hàng nước."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "sửa số No KH ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "sửa số No KH ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "sửa số No KH ");
            }
            return msg;
        }

        //public Message Delete(HONGHEON objUi)
        //{
        //    Message msg;

        //    try
        //    {
        //        // Get current Item in db
        //        var objDb = Get(objUi.MANGHEO );

        //        if (objDb == null)
        //        {
        //            // error message
        //            msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Hộ nghèo ", "");
        //            return msg;
        //        }

        //        //TODO: check if "hồ sơ đất" is in use

        //        // Set delete info
        //        _db.HONGHEONs.DeleteOnSubmit(objDb);
        //        // Submit changes to db
        //        _db.SubmitChanges();

        //        // success message
        //        msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hộ nghèo");
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ExceptionHandler.HandleDeleteException(ex, "Hộ nghèo");
        //    }

        //    return msg;
        //}
       
        //public Message DeleteList(List<HONGHEON> objList, PageAction action)
        //{
        //    Message msg;
        //    DbTransaction trans = null;

        //    try
        //    {
        //        _db.Connection.Open();
        //        trans = _db.Connection.BeginTransaction();
        //        _db.Transaction = trans;

        //        foreach (var obj in objList)
        //        {
        //            //TODO: check valid update infor
        //            Delete(obj);
        //        }

        //        // commit
        //        trans.Commit();

        //        // success message
        //        msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hộ nghèo ");
        //    }
        //    catch (Exception ex)
        //    {
        //        // rollback transaction
        //        if (trans != null)
        //            trans.Rollback();

        //        msg = ExceptionHandler.HandleInsertException(ex, "Hộ nghèo ");
        //    }

        //    return msg;
        //}

    }
}
