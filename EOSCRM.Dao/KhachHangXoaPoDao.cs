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
    public class KhachHangXoaPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly TieuThuPoDao _ttpoDao = new TieuThuPoDao();
        private readonly ReportClass report = new ReportClass();

        public KhachHangXoaPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHACHHANGXOAPO Get(string ma)
        {
            return _db.KHACHHANGXOAPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma));
        }

        public KHACHHANGXOAPO GetMADDK(string ma)
        {
            return _db.KHACHHANGXOAPOs.FirstOrDefault(p => p.MADDKPO.Equals(ma));
        }

        public KHACHHANGXOAPO GetKhachHangFromMadb(string MAKH)
        {
            return _db.KHACHHANGXOAPOs.FirstOrDefault(p => (p.MADPPO + p.DUONGPHUPO + p.MADBPO) == MAKH);
        }

        public KHACHHANGXOAPO GetKHDBKV(string MAKH, string makv)
        {
            return _db.KHACHHANGXOAPOs.FirstOrDefault(p => (p.MADPPO + p.DUONGPHUPO + p.MADBPO) == MAKH && p.MAKVPO == makv);
        }

        public List<KHACHHANGXOAPO> GetList()
        {
            return _db.KHACHHANGXOAPOs.ToList();
        }

        public List<KHACHHANGXOAPO> GetListKV(string makv)
        {
            return _db.KHACHHANGXOAPOs.Where(p => p.MAKVPO.Equals(makv)).ToList();
        }

        public List<KHACHHANGXOAPO> GetListKVKy(string makv, DateTime kynay)
        {
            return _db.KHACHHANGXOAPOs.Where(p => p.MAKVPO.Equals(makv)
                && p.NGAYXOA.Value.Month.Equals(kynay.Month) && p.NGAYXOA.Value.Year.Equals(kynay.Year)).ToList();
        }

        public List<KHACHHANGXOAPO> GetList(string madp)
        {
            return _db.KHACHHANGXOAPOs.Where(kh => kh.MADPPO.Equals(madp))
                //.OrderBy(kh => kh.STT).ToList();
                .OrderBy(kh => kh.MADPPO).OrderBy(kh => kh.MADBPO).OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANGXOAPO> GetList(string madp, int fromStt, int toStt)
        {
            return _db.KHACHHANGXOAPOs.Where(kh => kh.MADPPO.Equals(madp) &&
                                                kh.STT >= fromStt &&
                                                kh.STT <= toStt)
                .OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANGXOAPO> SearchKhachHang(string maDanhBo, string tenKhachHang, string maDongHo, string soHopDong, string soNha, string duongPho, string maKv)
        {
            if (maDanhBo == "" && tenKhachHang == "" && maDongHo == "" && soHopDong == "" && soNha == "" && duongPho == "" && (maKv == "" || maKv == "NULL"))
                return null;

            var query = _db.KHACHHANGXOAPOs.AsEnumerable();
            
            if(!string.IsNullOrEmpty(maDanhBo))
                query = query.Where(kh => (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).ToUpper().Contains(maDanhBo.ToUpper()));

            if (!string.IsNullOrEmpty(tenKhachHang))
                query = query.Where(kh => (kh.TENKH.ToUpper().Contains(tenKhachHang.ToUpper())));

            if (!string.IsNullOrEmpty(maDongHo))
                query = query.Where(kh => (kh.MALDHPO.ToUpper().Contains(maDongHo.ToUpper())));

            if (!string.IsNullOrEmpty(soHopDong))
                query = query.Where(kh => (kh.SOHD.ToUpper().Contains(soHopDong.ToUpper())));

            if (!string.IsNullOrEmpty(soNha))
                query = query.Where(kh => (kh.SONHA.ToUpper().Contains(soNha.ToUpper())));
           
            if (!string.IsNullOrEmpty(maKv))
                query = query.Where(kh => (maKv == "%" || kh.MAKVPO.Equals(maKv)));

            return query.ToList();
        }

        public int Count( )
        {
            return _db.KHACHHANGXOAPOs.Count();
        }

        public List<KHACHHANGXOAPO> GetListInKKT(int thang, int nam)
        {
            return _db.KHACHHANGXOAPOs.Where(k => (k.THANGBDKT.HasValue && 
                                                k.THANGBDKT.Value.Equals(thang) &&
                                                k.NAMBDKT.HasValue &&
                                                k.NAMBDKT.Value.Equals(nam)))
                                            .OrderByDescending(k => k.IDKHPO).ToList();
        }

        public List<KHACHHANGXOAPO> GetListKH(int thang, int nam)
        {
            return _db.KHACHHANGXOAPOs.Where(k => k.KYKHAITHAC.Month == thang && k.KYKHAITHAC.Year == nam)
                                            .OrderByDescending(k => k.IDKHPO).ToList();
        }

        public List<KHACHHANGXOAPO> GetListKHKV(int thang, int nam, string makv)
        {
            return _db.KHACHHANGXOAPOs.Where(k => k.KYKHAITHAC.Month == thang && k.KYKHAITHAC.Year == nam && k.MAKVPO.Equals(makv))
                                            .OrderByDescending(k => k.IDKHPO).ToList();
        }

        public List<KHACHHANGXOAPO> GetList(String IDKH, String SOHD, String MADH, String TENKH, String SONHA, String TENDP, String MAKV, String GHI2THANG1LAN)
        {
            //var dskh = from kh in _db.KHACHHANGs
            //           select kh;

            var dskh = from kh in _db.KHACHHANGXOAPOs
                       join d in _db.DONGHOPOs on kh.MADHPO equals d.MADHPO             
                       select kh;

            if (IDKH != null)
                dskh = dskh.Where(kh => (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).Contains(IDKH));

            if (SOHD != null)
                dskh = dskh.Where(kh => kh.SOHD.Equals(SOHD));           

            if (TENKH != null)
                dskh = dskh.Where(kh => kh.TENKH.Contains(TENKH));

            if (SONHA != null)
                dskh = dskh.Where(kh => kh.SONHA.Contains(SONHA));
         
            if (MAKV != null)
                dskh = dskh.Where(kh => kh.MAKVPO.Equals(MAKV));                

            return dskh
                        .OrderBy(kh => kh.STT)
                        .OrderBy(kh => kh.DUONGPHUPO)
                        .OrderBy(kh => kh.MADPPO)
                        .ToList();
        }

        public bool IsInUse(string ma)
        {
            return _db.KHACHHANGXOAPOs.Where(p => p.IDKHPO.Equals(ma)).Count() > 0;
        }

        public Message DeleteList(List<KHACHHANGXOAPO> objList)
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

        public Message Delete(KHACHHANGXOAPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.IDKHPO);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng xóa ", objUi.IDKHPO);
                    return msg;
                }

                report.UpKhachHangXoaPo(objUi.IDKHPO);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKHPO,
                    IPAddress = "192.168.1.11",
                    MANV = "KO",
                    UserAgent = "192.168.1.11",
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "RXBPO",
                    MOTA = "Phục hồi xóa bộ khách hàng điện."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.KHACHHANGXOAPOs.DeleteOnSubmit(objDb);
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
