using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Data.SqlClient;

namespace EOSCRM.Dao
{
    public class KhachHangXoaDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly TieuThuDao _ttDao = new TieuThuDao();
        private readonly ReportClass report = new ReportClass();   

        public KhachHangXoaDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHACHHANGXOA Get(string ma)
        {
            return _db.KHACHHANGXOAs.FirstOrDefault(p => p.IDKH.Equals(ma));
        }

        public KHACHHANGXOA GetMADDK(string ma)
        {
            return _db.KHACHHANGXOAs.FirstOrDefault(p => p.MADDK.Equals(ma));
        }

        public KHACHHANGXOA GetKhachHangFromMadb(string MAKH)
        {
            return _db.KHACHHANGXOAs.FirstOrDefault(p => (p.MADP + p.DUONGPHU + p.MADB) == MAKH);
        }

        public KHACHHANGXOA GetKHDBKV(string MAKH, string makv)
        {
            return _db.KHACHHANGXOAs.FirstOrDefault(p => (p.MADP + p.DUONGPHU + p.MADB) == MAKH && p.MAKV == makv);
        }

        public List<KHACHHANGXOA> GetList()
        {
            return _db.KHACHHANGXOAs.ToList();
        }

        public List<KHACHHANGXOA> GetListKV(string makv)
        {
            return _db.KHACHHANGXOAs.Where(p => p.MAKV.Equals(makv)).ToList();
        }

        public List<KHACHHANGXOA> GetListKVKy(string makv, DateTime kynay)
        {
            return _db.KHACHHANGXOAs.Where(p => p.MAKV.Equals(makv)
                && p.NGAYXOA.Value.Month.Equals(kynay.Month) && p.NGAYXOA.Value.Year.Equals(kynay.Year)).ToList();
        }

        public List<KHACHHANGXOA> GetList(string madp)
        {
            return _db.KHACHHANGXOAs.Where(kh => kh.MADP.Equals(madp))
                //.OrderBy(kh => kh.STT).ToList();
                .OrderBy(kh => kh.MADP).OrderBy(kh => kh.MADB).OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANGXOA> GetList(string madp, int fromStt, int toStt)
        {
            return _db.KHACHHANGXOAs.Where(kh => kh.MADP.Equals(madp) &&
                                                kh.STT >= fromStt &&
                                                kh.STT <= toStt)
                .OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANGXOA> SearchKhachHang(string maDanhBo, string tenKhachHang, string maDongHo, string soHopDong, string soNha, string duongPho, string maKv)
        {
            if (maDanhBo == "" && tenKhachHang == "" && maDongHo == "" && soHopDong == "" && soNha == "" && duongPho == "" && (maKv == "" || maKv == "NULL"))
                return null;

            var query = _db.KHACHHANGXOAs.AsEnumerable();
            
            if(!string.IsNullOrEmpty(maDanhBo))
                query = query.Where(kh => (kh.MADP + kh.DUONGPHU + kh.MADB).ToUpper().Contains(maDanhBo.ToUpper()));

            if (!string.IsNullOrEmpty(tenKhachHang))
                query = query.Where(kh => (kh.TENKH.ToUpper().Contains(tenKhachHang.ToUpper())));

            if (!string.IsNullOrEmpty(maDongHo))
                query = query.Where(kh => (kh.MALDH.ToUpper().Contains(maDongHo.ToUpper())));

            if (!string.IsNullOrEmpty(soHopDong))
                query = query.Where(kh => (kh.SOHD.ToUpper().Contains(soHopDong.ToUpper())));

            if (!string.IsNullOrEmpty(soNha))
                query = query.Where(kh => (kh.SONHA.ToUpper().Contains(soNha.ToUpper())));

           
            if (!string.IsNullOrEmpty(maKv))
                query = query.Where(kh => (maKv=="%"  || kh.MAKV.Equals(maKv)));

            return query.ToList();
        }

        public int Count( )
        {
            return _db.KHACHHANGXOAs.Count();
        }

        public List<KHACHHANGXOA> GetListInKKT(int thang, int nam)
        {
            return _db.KHACHHANGXOAs.Where(k => (k.THANGBDKT.HasValue && 
                                                k.THANGBDKT.Value.Equals(thang) &&
                                                k.NAMBDKT.HasValue &&
                                                k.NAMBDKT.Value.Equals(nam)))
                                            .OrderByDescending(k => k.IDKH).ToList();
        }

        public List<KHACHHANGXOA> GetListKH(int thang, int nam)
        {
            return _db.KHACHHANGXOAs.Where(k => k.KYKHAITHAC.Month == thang && k.KYKHAITHAC.Year == nam)
                                            .OrderByDescending(k => k.IDKH).ToList();
        }

        public List<KHACHHANGXOA> GetListKHKV(int thang, int nam, string makv)
        {
            return _db.KHACHHANGXOAs.Where(k => k.KYKHAITHAC.Month == thang && k.KYKHAITHAC.Year == nam && k.MAKV.Equals(makv))
                                            .OrderByDescending(k => k.IDKH).ToList();
        }

        public List<KHACHHANGXOA> GetList(String IDKH, String SOHD, String MADH, String TENKH, String SONHA, String TENDP, String MAKV, String GHI2THANG1LAN)
        {
            //var dskh = from kh in _db.KHACHHANGs
            //           select kh;

            var dskh = from kh in _db.KHACHHANGXOAs
                       join d in _db.DONGHOs on kh.MADH equals d.MADH                      
                       select kh;

            if (IDKH != null)
                dskh = dskh.Where(kh => (kh.MADP + kh.DUONGPHU + kh.MADB).Contains(IDKH));

            if (SOHD != null)
                dskh = dskh.Where(kh => kh.SOHD.Equals(SOHD));           

            if (TENKH != null)
                dskh = dskh.Where(kh => kh.TENKH.Contains(TENKH));

            if (SONHA != null)
                dskh = dskh.Where(kh => kh.SONHA.Contains(SONHA));
         
            if (MAKV != null)
                dskh = dskh.Where(kh => kh.MAKV.Equals(MAKV));                

            return dskh
                        .OrderBy(kh => kh.STT)
                        .OrderBy(kh => kh.DUONGPHU)
                        .OrderBy(kh => kh.MADP)
                        .ToList();
        }

        public bool IsInUse(string ma)
        {
            return _db.KHACHHANGXOAs.Where(p => p.IDKH.Equals(ma)).Count() > 0;
        }

        public Message DeleteList(List<KHACHHANGXOA> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách khách hàng xóa");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "khách hàng xóa", failed, "khách hàng xóa");
                }

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " khách hàng xóa");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách khách hàng xóa");
            }

            return msg;
        }

        public Message Delete(KHACHHANGXOA objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.IDKH);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng xóa ", objUi.IDKH);
                    return msg;
                }

                report.UpKhachHangXoa(objUi.IDKH);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = "192.168.1.11",
                    MANV = "KO",
                    UserAgent = "192.168.1.11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "RXBN",
                    MOTA = "Phục hồi xóa bộ khách hàng nước."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.KHACHHANGXOAs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Khách hàng xóa ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Khách hàng xóa ");
            }

            return msg;
        }

      
    }
}
