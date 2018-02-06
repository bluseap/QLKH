using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class BBNghiemThuDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly DonDangKyDao _ddkDao = new DonDangKyDao();
        private readonly ReportClass _rpDao = new ReportClass();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public BBNghiemThuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }       
        
        public BBNGHIEMTHU Get(string macb)
        {
            return _db.BBNGHIEMTHUs.Where(p => p.MABBNT.Equals(macb)).SingleOrDefault();
        }

        public BBNGHIEMTHU GetMADDK(string macb)
        {
            return _db.BBNGHIEMTHUs.Where(p => p.MADDK.Equals(macb)).SingleOrDefault();
        }

        public List<BBNGHIEMTHU> GetList()
        {
            //return _db.BBNGHIEMTHUs.OrderBy(c => c.MABBNT).ToList();

            var nt = from bb in _db.BBNGHIEMTHUs join don in _db.DONDANGKies on bb.MABBNT equals don.MADDK
                     orderby bb.MABBNT
                     select bb;
            return nt.OrderByDescending(b => b.NGAYLAPBB).ToList();
        }
        

        public List<BBNGHIEMTHU> GetListKV(string makv)
        {
            //return _db.BBNGHIEMTHUs.OrderBy(c => c.MABBNT).ToList();

            var nt = from bb in _db.BBNGHIEMTHUs
                     join don in _db.DONDANGKies on bb.MABBNT equals don.MADDK
                     where don.MAKV.Equals(makv)
                     //orderby bb.MABBNT
                     orderby bb.NGAYNHAP descending
                     select bb;
            return nt.OrderByDescending(b => b.NGAYLAPBB).ToList();
        }

        public List<BBNGHIEMTHU> GetListKVTenMaDon(string makv, string tukhoa)
        {
            //return _db.BBNGHIEMTHUs.OrderBy(c => c.MABBNT).ToList();

            //var nt = from bb in _db.BBNGHIEMTHUs
            //         join don in _db.DONDANGKies on bb.MABBNT equals don.MADDK
            //         where don.MAKV.Equals(makv)
            //         orderby bb.MABBNT
            //         select bb;

            var query = _db.DONDANGKies.AsQueryable();

            if (!string.IsNullOrEmpty(tukhoa))
                query = query.Where(d => d.MADDK.Equals(tukhoa) || d.TENKH.Contains(tukhoa));
                       

            var nt = from bb in _db.BBNGHIEMTHUs
                     join don in query on bb.MABBNT equals don.MADDK
                     where don.MAKV.Equals(makv)
                     orderby bb.MABBNT
                     select bb;
            return nt
                .OrderByDescending(b => b.NGAYLAPBB).ToList();
        }

        public List<BBNGHIEMTHU> GetListSC()
        {   
            var nt = from bb in _db.BBNGHIEMTHUs
                     join gq in _db.GIAIQUYETTHONGTINSUACHUAs on bb.MABBNT equals gq.MADON
                     orderby bb.MABBNT
                     select bb;
            return nt.OrderByDescending(b => b.NGAYLAPBB).ToList();
        }

        public List<BBNGHIEMTHU> GetList(DateTime? fromDate, DateTime? toDate)
        {
            var query = _db.BBNGHIEMTHUs.AsQueryable();            

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB >= fromDate).AsQueryable();

            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB <= toDate).AsQueryable();
            
            return query.OrderByDescending(dh => dh.NGAYLAPBB).ToList();
        }

        public List<BBNGHIEMTHU> GetListKV(DateTime? fromDate, DateTime? toDate, string makv)
        {
            var query = _db.BBNGHIEMTHUs.Where(b => b.DONDANGKY.MAKV.Equals(makv)).AsQueryable();

            if (fromDate.HasValue || toDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB >= fromDate && dh.NGAYLAPBB <= toDate).AsQueryable();

            //if (toDate.HasValue)
              //  query = query.Where(dh => dh.NGAYLAPBB <= toDate).AsQueryable();

            return query.OrderByDescending(dh => dh.NGAYLAPBB).ToList();
        }

        public List<BBNGHIEMTHU> GetListKVLX(DateTime? fromDate, DateTime? toDate, string makv)
        {
            var query = _db.BBNGHIEMTHUs.Where(b => b.DONDANGKY.MAKV.Equals(makv)).AsQueryable();

            if (fromDate.HasValue || toDate.HasValue)
                query = query.Where(dh => dh.NGAYNHAP >= fromDate && dh.NGAYNHAP <= toDate.Value.AddDays(1)).AsQueryable();

            return query.OrderByDescending(dh => dh.NGAYNHAP).ToList();
        }

        public List<BBNGHIEMTHU> GetListSC(DateTime? fromDate, DateTime? toDate)
        {
            //var query = _db.BBNGHIEMTHUs.AsQueryable();
            var query = from bb in _db.BBNGHIEMTHUs
                         join gq in _db.GIAIQUYETTHONGTINSUACHUAs on bb.MABBNT equals gq.MADON
                         where gq.TTDON == "SC_P"
                         select bb;

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB >= fromDate).AsQueryable();

            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB <= toDate).AsQueryable();

            return query.OrderByDescending(dh => dh.NGAYLAPBB).ToList();
        }   

        public List<BBNGHIEMTHU> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }        

        public int Count( )
        {
            return _db.BBNGHIEMTHUs.Count();
        }

        public Message Insert(BBNGHIEMTHU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {

                _db.Connection.Open();     
                _db.BBNGHIEMTHUs.InsertOnSubmit(objUi);

                var ddk = _db.DONDANGKies.SingleOrDefault(d => d.MADDK.Equals(objUi.MADDK));
                if (ddk != null)
                    ddk.TTNT = "NT_A";
                else
                {
                    trans.Rollback();
                    return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập biên bản nghiệm thu.", "Mã đơn đăng ký không tồn tại.");
                }

                _db.SubmitChanges();

                trans.Commit();

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "NT_A",
                    MOTA = "Nhập biên bản nghiệm thu."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                //_db.SubmitChanges();
                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "nghiệm thu", objUi.MABBNT);
            }
            return msg;
        }

        public Message InsertSC(BBNGHIEMTHU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                _db.BBNGHIEMTHUs.InsertOnSubmit(objUi);

                var gq = _db.GIAIQUYETTHONGTINSUACHUAs.SingleOrDefault(d => d.MADON.Equals(objUi.MABBNT));
                if (gq != null)
                    gq.TTDON = "SC_A";
                else
                {
                    trans.Rollback();
                    return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập biên bản nghiệm thu.", "Mã khách hàng không tồn tại.");
                }
                _db.SubmitChanges();
                trans.Commit();

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "SC_A",
                    MOTA = "Nhập biên bản nghiệm thu sửa chữa."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                //_db.SubmitChanges();
                // commit
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "nghiệm thu", objUi.MABBNT);
            }
            return msg;
        }

        public Message Update(BBNGHIEMTHU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MABBNT);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDK = objUi.MADDK;
                    objDb.MANV1 = objUi.MANV1;
                    objDb.HOTEN1 = objUi.HOTEN1;
                    objDb.MANV2 = objUi.MANV2;
                    objDb.HOTEN2 = objUi.HOTEN2;
                    objDb.MANV3 = objUi.MANV3;
                    objDb.HOTEN3 = objUi.HOTEN3;
                    objDb.CHIEUCAO = objUi.CHIEUCAO;
                    objDb.KHOANGCACH = objUi.KHOANGCACH;
                    objDb.VITRI = objUi.VITRI;
                    objDb.CHINIEMM1 = objUi.CHINIEMM1;
                    objDb.CHINIEMM2 = objUi.CHINIEMM2;
                    objDb.KETLUAN = objUi.KETLUAN;
                    objDb.MADH = objUi.MADH;
                    objDb.HETHONGCN = objUi.HETHONGCN;
                    objDb.MANV = objUi.MANV;
                    objDb.NGAYNHANHSTC = objUi.NGAYNHANHSTC;
                    objDb.NGAYCHUYENHSKTOAN = objUi.NGAYCHUYENHSKTOAN;

                    objDb.GHICHU = objUi.GHICHU;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "NT_P",
                        MOTA = "Cập nhập bản nghiệm thu."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nghiệm thu", objUi.MABBNT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "nghiệm thu");
            }
            return msg;
        }

        public Message UpdateNTToTK(BBNGHIEMTHU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MABBNT);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDK = objUi.MADDK;
                    objDb.MANV1 = objUi.MANV1;
                    objDb.HOTEN1 = objUi.HOTEN1;
                    objDb.MANV2 = objUi.MANV2;
                    objDb.HOTEN2 = objUi.HOTEN2;
                    objDb.MANV3 = objUi.MANV3;
                    objDb.HOTEN3 = objUi.HOTEN3;
                    objDb.CHIEUCAO = objUi.CHIEUCAO;
                    objDb.KHOANGCACH = objUi.KHOANGCACH;
                    objDb.VITRI = objUi.VITRI;
                    objDb.CHINIEMM1 = objUi.CHINIEMM1;
                    objDb.CHINIEMM2 = objUi.CHINIEMM2;
                    objDb.KETLUAN = objUi.KETLUAN;
                    objDb.MADH = objUi.MADH;
                    objDb.HETHONGCN = objUi.HETHONGCN;
                    objDb.MANV = objUi.MANV;
                    objDb.NGAYNHANHSTC = objUi.NGAYNHANHSTC;
                    objDb.NGAYCHUYENHSKTOAN = objUi.NGAYCHUYENHSKTOAN;

                    objDb.GHICHU = objUi.GHICHU;

                    var ddk = _db.DONDANGKies.SingleOrDefault(d => d.MADDK.Equals(objUi.MADDK));
                    if (ddk != null)
                    {
                        //ddk.TTNT = "NT_RA";
                        ddk.TTTK = "TK_P";
                    }
                    else
                    {
                        //trans.Rollback();
                        return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập biên bản nghiệm thu.", "Mã đơn đăng ký không tồn tại.");
                    }

                    //_db.SubmitChanges();

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "NT_P",
                        MOTA = "Cập nhập bản nghiệm thu."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nghiệm thu", objUi.MABBNT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "nghiệm thu");
            }
            return msg;
        }

        public Message UpdateChuyenHS_KT(List<BBNGHIEMTHU> objList, String useragent, String ipAddress, String sManv, DateTime ngaychuyen)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    var objDb = Get(objUi.MABBNT);
                    if (objDb == null)
                    {                       
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "BB nghiệm thu", objUi.MABBNT);
                        return msg;
                    }

                    objDb.NGAYCHUYENHSKTOAN = ngaychuyen;

                    _rpDao.HisNgayDangKyBien(objDb.MADDK, sManv, _ddkDao.Get(objDb.MADDK).MAKV, ngaychuyen, DateTime.Now,
                        DateTime.Now, "", "", "", "", "UPBBNTCHUYEN");

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = ngaychuyen,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "NGAYCHUYEN",
                        MOTA = "Ngày chuyển bản nghiệm thu về kế toán."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();    
                    
                // success message
                msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");

                return msg;               
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách đơn đăng ký", ex.Message);
            }
            return msg;
        }

        public bool IsInUse(string macb)
        {
            return _db.BBNGHIEMTHUs.Where(p => p.MABBNT.Equals(macb)).Count() > 0;
        }

        public Message Delete(BBNGHIEMTHU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MABBNT );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "nghiệm thu", objUi.MABBNT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.BBNGHIEMTHUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "nghiệm thu");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<BBNGHIEMTHU> objList)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var failed = 0;

                foreach (var obj in objList)
                {
                    var temp = Delete(obj);
                    if (temp != null && temp.MsgType.Equals(MessageType.Error))
                        failed++;
                }

                // commit
                trans.Commit();

                if (failed > 0)
                {
                    if (failed == objList.Count)
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách nghiệm thu");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "nghiệm thu", failed, "nghiệm thu");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách nghiệm thu");
            }

            return msg;
        }


        

    }
}
