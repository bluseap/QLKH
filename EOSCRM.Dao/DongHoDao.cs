using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


using System.ComponentModel;
using System.Reflection;
using System.Data.SqlClient;


namespace EOSCRM.Dao
{
    public  class DongHoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DongHoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public DataTable GetDataToTable(SqlCommand cmd)
        {
            DataTable dt = new DataTable();
            // Khai báo chuỗi kết nối
            //String strConnString = @"Server =192.168.1.7;Initial Catalog=EOSAG;User ID=sa;Password=P@sswOrD";
            SqlConnection con = new SqlConnection(Connectionstring);
            SqlDataAdapter sda = new SqlDataAdapter();
            cmd.CommandType = CommandType.Text;
            cmd.Connection = con;
            try
            {
                // Mở kết nối và đổ dữ liệu vào bảng
                con.Open();
                sda.SelectCommand = cmd;
                sda.Fill(dt);
                return dt;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Đóng kết nối
                con.Close();
                sda.Dispose();
                con.Dispose();
            }
        }

        public DONGHO  Get(string ma)
        {
            return _db.DONGHOs.Where(p => p.MADH.Equals(ma)).SingleOrDefault();
        }

        public DONGHO GetDASD(string ma)
        {
            return _db.DONGHOs.Where(p => p.MADH.Equals(ma) && p.DASD.Equals(1)).SingleOrDefault();
        }

        public DONGHO GetSoNo(string ma)
        {
            return _db.DONGHOs.Where(p => p.SONO.Equals(ma)).SingleOrDefault();
        }

        public DONGHO GetSoNoKV(string ma, string makv)
        {
            return _db.DONGHOs.Where(p => p.SONO.Equals(ma) && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public DONGHO GetSoNoKVLoai(string ma, string makv, string malhd)
        {
            return _db.DONGHOs.Where(p => p.SONO.Equals(ma) && p.MAKV.Equals(makv) && p.MALDH.Equals(malhd)).SingleOrDefault();
        }

        public List<DONGHO> Search(string key)
        {
            return _db.DONGHOs.Where(p => p.MADH.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<DONGHO> GetList()
        {
            return _db.DONGHOs.OrderByDescending(dh => dh.MADH).ToList();
        }  

        public List<DONGHO> GetList(String madh, String maldh, String namsx, String namtt, 
            DateTime? fromDate, DateTime? toDate, String trangthai)
        {
            var query = _db.DONGHOs.AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADH.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDH.Equals(maldh)).AsQueryable();

            if (!String.IsNullOrEmpty(namsx))
                query = query.Where(dh => dh.NAMSX.Contains(namsx)).AsQueryable();

            if (!String.IsNullOrEmpty(namtt))
                query = query.Where(dh => dh.NAMTT.Contains(namtt)).AsQueryable();

            if (!String.IsNullOrEmpty(trangthai))
                query = query.Where(dh => dh.TRANGTHAI.Contains(trangthai)).AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP >= fromDate).AsQueryable(); 
            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP <= toDate).AsQueryable();
            

            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADH).ToList();
        }

        public List<DONGHO> GetListKV(String madh, String maldh, String namsx, String namtt,
            DateTime? fromDate, DateTime? toDate, String trangthai, string maKV)
        {
            var query = _db.DONGHOs.Where(q => q.MAKV.Equals(maKV)).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADH.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDH.Equals(maldh)).AsQueryable();

            if (!String.IsNullOrEmpty(namsx))
                query = query.Where(dh => dh.NAMSX.Contains(namsx)).AsQueryable();

            if (!String.IsNullOrEmpty(namtt))
                query = query.Where(dh => dh.NAMTT.Contains(namtt)).AsQueryable();

            if (!String.IsNullOrEmpty(trangthai))
                query = query.Where(dh => dh.TRANGTHAI.Contains(trangthai)).AsQueryable();

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP >= fromDate).AsQueryable();
            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP <= toDate).AsQueryable();


            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADH).ToList();
        }

        public List<DONGHO> GetList2(String madh, String maldh, String sono)
        {
            var query = _db.DONGHOs.AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADH.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDH.Equals(maldh)).AsQueryable();


            if (!String.IsNullOrEmpty(sono))
                query = query.Where(dh => dh.SONO.Contains(sono)).AsQueryable();

            
            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADH).ToList();
        }

        public List<DONGHO> GetList2KV(String madh, String maldh, String sono, string maKV)
        {
            var query = _db.DONGHOs.Where(d => d.MAKV.Equals(maKV)).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(dh => dh.MADH.Contains(madh)).AsQueryable();

            if (!String.IsNullOrEmpty(maldh) && maldh != "%")
                query = query.Where(dh => dh.MALDH.Equals(maldh)).AsQueryable();


            if (!String.IsNullOrEmpty(sono))
                query = query.Where(dh => dh.SONO.Contains(sono)).AsQueryable();


            //return query.OrderByDescending(dh => dh.NGAYNK).ToList();
            return query.OrderByDescending(dh => dh.MADH).ToList();
        }

        public List<DONGHO> GetList(string keyword)
        {
            return _db.DONGHOs.Where(p => (p.MADH.ToLower().Contains(keyword.ToLower())
                || p.MALDH.ToLower().Contains(keyword.ToLower())
                || p.NAMSX.ToLower().Contains(keyword.ToLower())
                || p.SONO.ToLower().Contains(keyword.ToLower())                
                )).ToList();
        }

        public List<DONGHO> GetListDASD(string keyword)
        {
            /*return _db.DONGHOs.Where(p => (p.MADH.ToLower().Contains(keyword.ToLower())
                || p.MALDH.ToLower().Contains(keyword.ToLower())
                || p.NAMSX.ToLower().Contains(keyword.ToLower())
                || p.SONO.ToLower().Contains(keyword.ToLower())                
                )).ToList();*/
            return _db.DONGHOs.Where(p => p.DASD.Equals(0)                        
                        //&& (p.MALDH.ToLower().Contains(keyword.ToLower())
                        && p.SONO.ToLower().Contains(keyword.ToLower())).ToList();
        }

        public List<DONGHO> GetListDASDKV(string keyword, string makv)
        {
            /*return _db.DONGHOs.Where(p => (p.MADH.ToLower().Contains(keyword.ToLower())
                || p.MALDH.ToLower().Contains(keyword.ToLower())
                || p.NAMSX.ToLower().Contains(keyword.ToLower())
                || p.SONO.ToLower().Contains(keyword.ToLower())                
                )).ToList();*/
            return _db.DONGHOs.Where(p => p.DASD.Equals(0) && p.MAKV.Equals(makv)
                        && (p.MALDH.ToLower().Contains(keyword.ToLower())
                        || p.SONO.ToLower().Contains(keyword.ToLower()))).ToList();
        }

        public List<DONGHO> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.DONGHOs.Count();
        }

        public string NewId()
        {
            var query = _db.DONGHOs.Max(p => p.MADH);

            var temp = int.Parse(query) + 1;
            return temp.ToString("D6");
        }

        public string NewIdKVCT(string makv)
        {
            var query = _db.DONGHOs.Where(p => p.MAKV.Equals(makv)).Max(p => p.MADH);

            var temp = int.Parse(query) + 1;
            return temp.ToString("D6");
        }

        public string NewIdKVCTTS(string makv)
        {
            var query = _db.DONGHOs.Where(p => p.MAKV.Equals("O") || p.MAKV.Equals("U")).Max(p => p.MADH);

            var temp = int.Parse(query) + 1;
            return temp.ToString("D6");
        }

        public string NewIdLX(string makv)
        {
            var query = _db.DONGHOs.Where(p => p.MAKV.Equals(makv)).Max(p => p.MADH);                        

            var temp = int.Parse(query.Substring(2,6)) + 1;
            return temp.ToString("D6");
        }

        public string NewIdCD(string makv, string chaudoc)
        {
            var query = _db.DONGHOs.Where(p => p.MAKV.Equals(makv) && p.MADH.Substring(0,2).Equals(chaudoc)).Max(p => p.MADH);

            var temp = int.Parse(query.Substring(2, 6)) + 1;
            return temp.ToString("D6");
        }

        public Message Insert(DONGHO objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DONGHOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đồng hồ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "đồng hồ");
            }
            return msg;
        }

        public void Insert2(DONGHO objUi, String useragent, String ipAddress, String sManv)
        {            
            try
            {

                _db.Connection.Open();

                _db.DONGHOs.InsertOnSubmit(objUi);

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "DH_A",
                    MOTA = "Nhập đồng hồ nước."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                _db.SubmitChanges();
                
            }
            catch  {   }           
        }

        public Message Update(DONGHO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADH );

                if (objDb != null)
                {
                    //TODO: update all fields
                    //objDb.DASD    = objUi.DASD    ;
                    if(!string .IsNullOrEmpty( objUi .MALDH ))
                        objDb.LOAIDH = _db.LOAIDHs.Single(p => p.MALDH.Equals(objUi.MALDH));
                    //objDb.MALDH = objUi.MALDH ;
                    objDb.NAMSX= objUi.NAMSX ;
                    objDb.NAMTT = objUi.NAMTT;
                    objDb.NGAYNK= objUi.NGAYNK;
                    objDb.NGAYXK= objUi.NGAYXK;
                    objDb.TRANGTHAI  = objUi.TRANGTHAI ;
                    objDb.SONO = objUi.SONO;
                    objDb.SOKD = objUi.SOKD;
                    objDb.TEMKD = objUi.TEMKD;
                    objDb.HANKD = objUi.HANKD;
                    objDb.NGAYKD = objUi.NGAYKD;
                    objDb.TENCTKD = objUi.TENCTKD;
                    objDb.SXTAI = objUi.SXTAI;
                    objDb.CONGSUAT = objUi.CONGSUAT;

                    objDb.DASD = objUi.DASD;

                    objDb.NgaySua = DateTime.Now;
                    objDb.ManvSua = sManv;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "DH_P",
                        MOTA = "Sửa thông tin đồng hồ nước."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                    
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public Message UpdateDASD(DONGHO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADH);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = true;                    

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public Message UpdateKoSD(DONGHO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADH);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = false;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public Message UpdateDASDDH(String madh)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(madh);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = true;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đồng hồ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đồng hồ");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đồng hồ");
            }
            return msg;
        }

        public void UpdateDASDMoi(string madh)
        {            
            try
            {
                // get current object in database
                var objDb = Get(madh);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DASD = true;
                    // Submit changes to db
                    _db.SubmitChanges();                   
                }                
            }
            catch (Exception ex)
            {               
            }           
        }

        public bool IsInUse(string ma)
        {
            if( _db.THICONGs  .Where(p => p.MADH .Equals(ma)).Count() > 0)
                return true;
            if (_db.KDDHCTs .Where(p => p.MADH.Equals(ma)).Count() > 0)
                return true;
            return false;
        }

        public Message Delete(DONGHO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADH);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đồng hồ ", objUi.MADH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DONGHOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đồng hồ ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đồng hồ ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<DONGHO> objList)
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
                    Delete(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "danh sách đồng hồ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách đồng hồ");
            }

            return msg;
        }
    }
}
