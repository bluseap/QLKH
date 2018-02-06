using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class HoNgheoNDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public HoNgheoNDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public HONGHEON Get(string id)
        {
            return _db.HONGHEONs.FirstOrDefault(p => p.MANGHEO.Equals(id));
        }

        public List<HONGHEON> GetList()
        {
            return _db.HONGHEONs.Where(hn => hn.ISHONGHEO.Equals(true))
                .OrderBy(hn => hn.MAXA).ToList();
        }

        public List<HONGHEON> GetListKV(string makv)
        {
            return _db.HONGHEONs.Where(hn => hn.ISHONGHEO.Equals(true) && hn.MAKV.Equals(makv))
                .OrderBy(hn => hn.MAXA).ToList();
        }

        public List<HONGHEON> TimHNN(string mangheo, string makv)
        {  
            var query = from hn in _db.HONGHEONs
                        join kh in _db.KHACHHANGs on hn.IDKH equals kh.IDKH
                        where (hn.ISHONGHEO.Equals(true) && hn.MAKV.Equals(makv))
                        select hn;

            if (string.IsNullOrEmpty(mangheo) || mangheo == "")
                query.ToList(); 

            if (!string.IsNullOrEmpty(mangheo))
                query = query.Where(hn => (hn.KHACHHANG.MADP + hn.KHACHHANG.MADB).ToUpper().Contains(mangheo.ToUpper()) 
                        || hn.KHACHHANG.TENKH.ToUpper().Contains(mangheo.ToUpper()));

            //if (!string.IsNullOrEmpty(mangheo))
            //    query = query.Where(hn => (hn.KHACHHANG.TENKH).ToUpper().Contains(mangheo.ToUpper()));            

            return query.ToList();
        }

        public List<HONGHEON> GetListKy(int nam, int thang)
        {
            return _db.HONGHEONs.Where(hn => hn.KYHOTROHN.Value.Year.Equals(nam) && hn.KYHOTROHN.Value.Month.Equals(thang) && hn.ISHONGHEO.Equals(true))
                .OrderBy(hn => hn.MAXA).ToList();
        }

        public List<HONGHEON> GetListKyKV(int nam, int thang, string makv)
        {
            return _db.HONGHEONs.Where(hn => hn.KYHOTROHN.Value.Year.Equals(nam) && hn.KYHOTROHN.Value.Month.Equals(thang) 
                && hn.MAKV.Equals(makv)
                && hn.ISHONGHEO.Equals(true))

                .OrderBy(hn => hn.MAXA).ToList();                
        }

        public void Insert(HONGHEON objUi, String useragent, String ipAddress, String sManv)
        {
            try
            {
                _db.Connection.Open();
                _db.HONGHEONs.InsertOnSubmit(objUi);             

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MANGHEO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "NGHEONN",
                    MOTA = "Nhập hộ nghèo nước."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();
            }
            catch { }
        }

        public Message Update(HONGHEON objUi, String useragent, String ipAddress, String sManv, string mangheo, string idkh)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(mangheo);

                if (objDb != null)
                {
                    //ho ngheo
                    objDb.MAXA = objUi.MAXA;
                    objDb.ISHONGHEO = objUi.ISHONGHEO;
                    objDb.DONVICAPHN = objUi.DONVICAPHN;
                    objDb.MAHN = objUi.MAHN;
                    objDb.NGAYCAPHN = objUi.NGAYCAPHN;
                    objDb.NGAYKETTHUCHN = objUi.NGAYKETTHUCHN;
                    objDb.NGAYKYHN = objUi.NGAYKYHN;
                    objDb.DIACHINGHEO = objUi.DIACHINGHEO;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = mangheo,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "NGHEONU",
                        MOTA = "Sửa hộ nghèo nước."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "hộ nghèo nước");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hộ nghèo nước");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hộ nghèo nước");
            }
            return msg;
        }

        public Message Delete(HONGHEON objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MANGHEO );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Hộ nghèo ", "");
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.HONGHEONs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hộ nghèo");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Hộ nghèo");
            }

            return msg;
        }
       
        public Message DeleteList(List<HONGHEON> objList, PageAction action)
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
                    //TODO: check valid update infor
                    Delete(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hộ nghèo ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Hộ nghèo ");
            }

            return msg;
        }

        public string NewId()
        {
            var query = _db.HONGHEONs.Max(p => p.MANGHEO);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D9");
            }

            return "000000001";
        }
       
        
        

    }
}
