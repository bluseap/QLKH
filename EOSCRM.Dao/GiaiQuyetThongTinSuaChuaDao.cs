using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{

    public class GiaiQuyetThongTinSuaChuaDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly ThietKeDao _tkDao = new ThietKeDao();

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public GiaiQuyetThongTinSuaChuaDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public GIAIQUYETTHONGTINSUACHUA Get(string madon)
        {
            return _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Equals(madon)).SingleOrDefault();
        }

        public List<GIAIQUYETTHONGTINSUACHUA> GetListChuaPhanCong()
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => d.TTDON.Equals(TTSC.SC_P.ToString()));

            return dsdon.OrderByDescending(d => d.MADON).ToList();
        }

        public List<GIAIQUYETTHONGTINSUACHUA> GetListChuaPhanCong(String madon, String makv, String makh,
            String tenkh, String sodt, String ttsb, String maph, DateTime? ngaybd, DateTime? ngaykt)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => d.TTDON.Equals(TTSC.SC_P.ToString())).AsQueryable();

            if(!String.IsNullOrEmpty(madon))
                dsdon = dsdon.Where(d => d.MADON.Equals(madon)).AsQueryable();

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                dsdon = dsdon.Where(d => d.MAKV.Equals(makv)).AsQueryable();

            if (!String.IsNullOrEmpty(makh))
                dsdon = dsdon.Where(d => d.IDKH.Equals(makh)).AsQueryable();

            if (!String.IsNullOrEmpty(tenkh))
                dsdon = dsdon.Where(d => d.KHACHHANG.TENKH.Contains(tenkh)).AsQueryable();

            if (!String.IsNullOrEmpty(sodt))
                dsdon = dsdon.Where(d => d.SDT.Equals(sodt)).AsQueryable();

            if (!String.IsNullOrEmpty(ttsb))
                dsdon = dsdon.Where(d => d.THONGTINKH.Equals(ttsb)).AsQueryable();

            if (!String.IsNullOrEmpty(maph) && maph != "%")
                dsdon = dsdon.Where(d => d.MAPH.Equals(maph)).AsQueryable();

            if (ngaybd.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO >= ngaybd.Value).AsQueryable();

            if (ngaykt.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO <= ngaykt.Value).AsQueryable();

            return dsdon.OrderByDescending(d => d.MADON).ToList();
        }


        /// <summary>
        /// Lấy danh sách đơn sữa chữa bất kỳ
        /// </summary>
        /// <param name="keyword">Từ khóa tra cứu</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListDonSuaChua(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = from p in _db.GIAIQUYETTHONGTINSUACHUAs select p;
                           

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();

           
        }
        /// <summary>
        /// Danh sách thông tin các đơn sữa chữa chưa được phân công
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListChuaPhanCong(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d =>d.TTDON.Equals(TTSC.SC_P.ToString()));

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if(toDate .HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();
            
        }
        /// <summary>
        /// Danh sách đã phân công sửa chữa
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListDaPhanCong(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => d.TTDON.Equals(TTSC.SC_N.ToString()));

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();
        }

        /// <summary>
        /// Danh sách đã phân công sửa chữa
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListForTraCuu(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => d.TTDON != null && d.TTDON != TTSC.SC_P.ToString());

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();
        }


        /// <summary>
        /// Lấy danh sách khách hàng lập chiết tính sữa chữa
        /// </summary>
        /// <param name="keyword">Từ khóa tra cứu</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đếnngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListDonChoLapChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {

            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => d.TTCT != null && (d.TTCT.Equals(TTCT.CT_N.ToString()) /*|| d.TTCT.Equals(TTCT.CT_P.ToString())*/));

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();

        }
        /// <summary>
        /// Danh sách đơn chờ duyệt chiết tính sữa chữa
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="fromDate"></param>
        /// <param name="toDate"></param>
        /// <param name="maKv"></param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListDonChoDuyetChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => d.TTCT != null && d.TTCT.Equals(TTCT.CT_P.ToString()));

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();


        }
        /// <summary>
        /// Danh sách đơn chờ cập nhật chiết tính sữa chữa
        /// </summary>
        /// <param name="keyword">Từ khóa tra cứu</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListDonChoCapNhatSauSuaChua(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs
                            .Where(d => ((d.TTDON.Equals(TTSC.SC_I.ToString()) && d.TTCT == null) || (d.TTDON.Equals(TTSC.SC_I.ToString()) && d.TTCT.Equals("CT_A"))));

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();

        }
        /// <summary>
        /// Danh sách đơn sữa chữa có lập chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa tra cứu</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maKv">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListDonCoLapChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String maKv)
        {
            var dsdon = _db.GIAIQUYETTHONGTINSUACHUAs.Where(d => d.TTCT != null);

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADON.Contains(keyword) ||
                                         d.THONGTINKH.Contains(keyword) ||
                                         d.GHICHU.Contains(keyword) ||
                                         d.SDT.Contains(keyword) ||
                                         (d.KHACHHANG != null && d.KHACHHANG.TENKH.Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword)) ||
                                         (d.KHACHHANG != null &&
                                          (d.KHACHHANG.MADP + d.KHACHHANG.DUONGPHU + d.KHACHHANG.MADB).Contains(
                                              keyword)) ||
                                         d.SDT.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value >= fromDate.Value);
            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYBAO.HasValue
                                         && d.NGAYBAO.Value <= toDate.Value);
            if (!string.IsNullOrEmpty(maKv))
                dsdon = dsdon.Where(d => d.MAKV.Equals(maKv));

            return dsdon.OrderByDescending(d => d.MADON).ToList();

        }

        public Message Insert(GIAIQUYETTHONGTINSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                objUi.TTDON = TTSC.SC_P.ToString();
                _db.Connection.Open();

                _db.GIAIQUYETTHONGTINSUACHUAs.InsertOnSubmit(objUi);

                _db.SubmitChanges();
               
               
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTDK.DK_A.ToString(),
                    MOTA = "Nhập đơn sữa chữa."
                };
                _kdDao.Insert(luuvetKyduyet);

                
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Đơn sữa chữa");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Đơn sữa chữa ", objUi.THONGTINKH);
            }
            return msg;
        }

        public Message InsertTKSC(GIAIQUYETTHONGTINSUACHUA objUi, String useragent, String ipAddress, String sManv, string tennvtk, string manvtk)
        {
            Message msg;
            try
            {
                objUi.TTDON = TTSC.SC_P.ToString();
                _db.Connection.Open();

                _db.GIAIQUYETTHONGTINSUACHUAs.InsertOnSubmit(objUi);

                _db.SubmitChanges();
             
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTDK.DK_A.ToString(),
                    MOTA = "Nhập đơn sữa chữa."
                };
                _kdDao.Insert(luuvetKyduyet);


                var khachhang = _khDao.Get(objUi.IDKH);
                var thietke = new THIETKE
                {
                    MADDK = objUi.MADON,
                    TENTK = "Sữa chữa",
                    CHUTHICH = "Khách hàng tự trám lại sân, nền, tường, vỉa hè sau khi thi công xong công trình",
                    MANVLTK = sManv,
                    THAMGIAONGCAI = false,
                    MANVTK = manvtk,
                    TENNVTK = tennvtk,
                    SODB = (khachhang.MADP + khachhang.MADB).ToString(),
                    
                    THECHAP = null,
                    NGAYLTK = DateTime.Now
                };
                _tkDao.InsertTKSC(thietke);
             
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Đơn sữa chữa");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Đơn sữa chữa ", objUi.THONGTINKH);
            }
            return msg;
        }

        public Message UpdateNhanTongTin(GIAIQUYETTHONGTINSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON);

                if (objDb != null)
                {
                    ////TODO: update all fields

                    objDb.KHACHHANG = !string.IsNullOrEmpty(objUi.IDKH) ? _db.KHACHHANGs.Single(p => p.IDKH.Equals(objUi.IDKH)) : null;


                    objDb.NGAYBAO = objUi.NGAYBAO;
                    objDb.SDT = objUi.SDT;
                    objDb.THONGTINKH = objUi.THONGTINKH;
                    objDb.NGAYBAO = objUi.NGAYBAO;
                    objDb.THONGTINPHANHOI = !string.IsNullOrEmpty(objUi.MAPH) ? _db.THONGTINPHANHOIs.Single(p => p.MAPH.Equals(objUi.MAPH)) : null;

                    objDb.NHANVIEN1 = !string.IsNullOrEmpty(objUi.MANVBAO) ? _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVBAO)) : null;
                    if(!string .IsNullOrEmpty(  objUi.MAKV))
                        objDb.KHUVUC = _db.KHUVUCs.Single(p=>p.MAKV .Equals(  objUi.MAKV));
                    //objDb.BIENBAN = objUi.BIENBAN;
                    //objDb.CHINIEM = objUi.CHINIEM;
                    //objDb.CSSAU = objUi.CSSAU;
                    //objDb.CSTRUOC = objUi.CSTRUOC;
                    //objDb.GHICHU = objUi.GHICHU;

                    //objDb.ISLAPCHIETTINH = objUi.ISLAPCHIETTINH;
                    //objDb.ISLAPQUYETTOAN = objUi.ISLAPQUYETTOAN;
                    //objDb.KYGG = objUi.KYGG;
                    //objDb.LANXL = objUi.LANXL;
                    //objDb.LYDO = objUi.LYDO;
                    //objDb.MADON = objUi.MADON;
                    //objDb.MAKV = objUi.MAKV ;
                    //objDb.MANVBAO = objUi.MANVBAO;
                    //objDb.MANVN = objUi.MANVN;
                    //objDb.MANVXL = objUi.MANVXL;
                    //objDb.MAPH = objUi.MAPH;
                    //objDb.MAXL = objUi.MAXL;

                    //objDb.NGAYBB = objUi.NGAYBB;
                    //objDb.NGAYGQ = objUi.NGAYGQ;
                    //objDb.NGAYHT = objUi.NGAYHT;
                    //objDb.NGAYNHAP = objUi.NGAYNHAP;

                    //objDb.SOBB = objUi.SOBB;

                    //objDb.TTCT = objUi.TTCT;
                    //objDb.TTQT = objUi.TTQT;
                    //objDb.TTDON = objUi.TTDON;
                    //objDb.XACNHAN = objUi.XACNHAN;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Cập nhật đơn sữa chữa."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Giải quyết thông tin sữa chữa ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Giải quyết thông tin sữa chữa ", objUi.THONGTINKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Giải quyết thông tin sữa chữa  ", objUi.THONGTINKH);
            }
            return msg;
        }

        public Message UpdateNhanTongTinTKSC(GIAIQUYETTHONGTINSUACHUA objUi, String useragent, String ipAddress, String sManv, string tennvtk, string manvtk)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON);

                if (objDb != null)
                {
                    ////TODO: update all fields

                    objDb.KHACHHANG = !string.IsNullOrEmpty(objUi.IDKH) ? _db.KHACHHANGs.Single(p => p.IDKH.Equals(objUi.IDKH)) : null;


                    objDb.NGAYBAO = objUi.NGAYBAO;
                    objDb.SDT = objUi.SDT;
                    objDb.THONGTINKH = objUi.THONGTINKH;
                    objDb.NGAYBAO = objUi.NGAYBAO;
                    objDb.THONGTINPHANHOI = !string.IsNullOrEmpty(objUi.MAPH) ? _db.THONGTINPHANHOIs.Single(p => p.MAPH.Equals(objUi.MAPH)) : null;

                    objDb.NHANVIEN1 = !string.IsNullOrEmpty(objUi.MANVBAO) ? _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVBAO)) : null;
                    if (!string.IsNullOrEmpty(objUi.MAKV))
                        objDb.KHUVUC = _db.KHUVUCs.Single(p => p.MAKV.Equals(objUi.MAKV));
                    //objDb.BIENBAN = objUi.BIENBAN;
                    //objDb.CHINIEM = objUi.CHINIEM;
                    //objDb.CSSAU = objUi.CSSAU;
                    //objDb.CSTRUOC = objUi.CSTRUOC;
                    //objDb.GHICHU = objUi.GHICHU;

                    //objDb.ISLAPCHIETTINH = objUi.ISLAPCHIETTINH;
                    //objDb.ISLAPQUYETTOAN = objUi.ISLAPQUYETTOAN;
                    //objDb.KYGG = objUi.KYGG;
                    //objDb.LANXL = objUi.LANXL;
                    //objDb.LYDO = objUi.LYDO;
                    //objDb.MADON = objUi.MADON;
                    //objDb.MAKV = objUi.MAKV ;
                    //objDb.MANVBAO = objUi.MANVBAO;
                    //objDb.MANVN = objUi.MANVN;
                    //objDb.MANVXL = objUi.MANVXL;
                    //objDb.MAPH = objUi.MAPH;
                    //objDb.MAXL = objUi.MAXL;

                    //objDb.NGAYBB = objUi.NGAYBB;
                    //objDb.NGAYGQ = objUi.NGAYGQ;
                    //objDb.NGAYHT = objUi.NGAYHT;
                    //objDb.NGAYNHAP = objUi.NGAYNHAP;

                    //objDb.SOBB = objUi.SOBB;

                    //objDb.TTCT = objUi.TTCT;
                    //objDb.TTQT = objUi.TTQT;
                    //objDb.TTDON = objUi.TTDON;
                    //objDb.XACNHAN = objUi.XACNHAN;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.IDKH,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Cập nhật đơn sữa chữa."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    
                    var thietke = new THIETKE
                    {
                        MADDK = objUi.IDKH,                        
                        MANVTK = manvtk,
                        TENNVTK = tennvtk                        
                    };
                    _tkDao.Update(thietke, useragent, ipAddress, sManv);

                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Giải quyết thông tin sữa chữa ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Giải quyết thông tin sữa chữa ", objUi.THONGTINKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Giải quyết thông tin sữa chữa  ", objUi.THONGTINKH);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.CHIETTINHSUACHUAs.Where(p => p.MADON.Equals(ma)).Count() > 0)
                return true;
            else if (_db.QUYETTOANSUACHUAs.Where(p => p.MADON.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(GIAIQUYETTHONGTINSUACHUA objUi,String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Giải quyết thông tin sữa chữa ", objUi.THONGTINKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.GIAIQUYETTHONGTINSUACHUAs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADON,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTDK.DK_A.ToString(),
                    MOTA = "Xóa đơn sữa chữa"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Giải quyết thông tin sữa chữa ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Giải quyết thông tin sữa chữa ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<GIAIQUYETTHONGTINSUACHUA> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                    Delete(obj,  useragent,  ipAddress,  sManv);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Giải quyết thông tin sửa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách giải quyết thông tin sửa chữa ");
            }

            return msg;
        }

        public string MakeIdkhToInsertNew()
        {
            string sReturn = "";

            string sToday = "";
            sToday = DateTime.Now.Year.ToString().Substring(2, 2);
            sToday = DateTime.Now.Month < 10 ? 
                sToday + "0" + DateTime.Now.Month.ToString() :
                sToday + DateTime.Now.Month.ToString();

            sToday = DateTime.Now.Day < 10 ? 
                sToday + "0" + DateTime.Now.Day.ToString() :
                sToday + DateTime.Now.Day.ToString();

            sReturn = (from p in _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Substring(0, 6).Contains(sToday))
                       select p.MADON).Max();

            if (!string.IsNullOrEmpty(sReturn))
            {
                int thutu = int.Parse(sReturn.Substring(6, 3));
                thutu = thutu + 1;
                if (thutu < 10)
                {
                    sReturn = sToday + "00" + thutu.ToString();
                }
                else if (thutu < 100)
                {
                    sReturn = sToday + "0" + thutu.ToString();
                }
                else if (thutu < 1000)
                {
                    sReturn = sToday + thutu.ToString();
                }
            }
            else
            {
                sReturn = sToday + "001";
            }

            return sReturn;
        }

        public Message PhanCongSuaChua(string madon, string manv, DateTime ngayphancong, string sMaxl,String useragent, String ipAddress, String sManv)
        {
            Message msg;
            GIAIQUYETTHONGTINSUACHUA objDb = _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Equals(madon)).SingleOrDefault();

            objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(manv));
            objDb.NGAYPHANCONG = ngayphancong;
            objDb.TTDON = TTSC.SC_N.ToString();
            objDb.THONGTINXULY = _db.THONGTINXULies.Single(p => p.MAXL.Equals(sMaxl));
            _db.SubmitChanges();

           
            var luuvetKyduyet = new LUUVET_KYDUYET
            {
                MADON = objDb.IDKH,
                IPAddress = ipAddress,
                MANV = sManv,
                UserAgent = useragent,
                NGAYTHUCHIEN = DateTime.Now,
                TACVU = TACVUKYDUYET.I.ToString(),
                MACN = CHUCNANGKYDUYET.KH01.ToString(),
                MATT = TTDK.DK_A.ToString(),
                MOTA = "Phân công sữa chữa."
            };
            _kdDao.Insert(luuvetKyduyet);
           

            msg = new Message("Phân công sữa chữa thành công", MessageType.Info, "Phân công sữa chữa ");
            return msg;
        }

        public Message GiaiQuyetDonSuaChua(GIAIQUYETTHONGTINSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            GIAIQUYETTHONGTINSUACHUA objDb = _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Equals(objUi.MADON)).SingleOrDefault();

            objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVXL));
            objDb.KHACHHANG = !string.IsNullOrEmpty(objUi.IDKH) ? _db.KHACHHANGs.Single(p => p.IDKH.Equals(objUi.IDKH)) : null;


            objDb.XACNHAN = objUi.XACNHAN;
            objDb.BIENBAN = objUi.BIENBAN;
            objDb.LANXL = objUi.LANXL;
            objDb.LYDO = objUi.LYDO;
            objDb.CODH = objUi.CODH;
            objDb.CHINIEM = objUi.CHINIEM;
            objDb.MADH = objUi.MADH;
            objDb.CSTRUOC = objUi.CSTRUOC;
            objDb.CSSAU = objUi.CSSAU;
            objDb.ISLAPCHIETTINH = objUi.ISLAPCHIETTINH;
            objDb.THONGTINKH = objUi.THONGTINKH;
            objDb.SDT = objUi.SDT;
            objDb.GHICHU = objUi.GHICHU;
            objDb.THONGTINXULY = _db.THONGTINXULies.Single(p => p.MAXL.Equals(objUi.MAXL));

            if (objUi.TTDON.Equals(TTSC.SC_F.ToString()))
            {
                objDb.ISLAPCHIETTINH = false;
                objDb.TTDON = TTSC.SC_F.ToString();
                objDb.NGAYHT = objUi.NGAYHT;

                if (!string.IsNullOrEmpty(objUi.IDKH))
                {
                    KHACHHANG khachhang = _db.KHACHHANGs.Single(p => p.IDKH.Equals(objUi.IDKH));
                    khachhang.THUYLK = objUi.CODH.ToString();
                    if (!string.IsNullOrEmpty(objUi.MADH))
                    {
                        khachhang.MADH = objUi.MADH;
                    }
                }
            }
            else
            {
                objDb.TTDON = TTSC.SC_I.ToString();
                if (objUi.ISLAPCHIETTINH == true)
                {
                    objDb.ISLAPCHIETTINH = true;
                    objDb.TTCT = TTCT.CT_N.ToString();
                }
            }

            _db.SubmitChanges();

          
            var luuvetKyduyet = new LUUVET_KYDUYET
            {
                MADON = objDb.IDKH,
                IPAddress = ipAddress,
                MANV = sManv,
                UserAgent = useragent,
                NGAYTHUCHIEN = DateTime.Now,
                TACVU = TACVUKYDUYET.I.ToString(),
                MACN = CHUCNANGKYDUYET.KH01.ToString(),
                MATT = TTDK.DK_A.ToString(),
                MOTA = "Giải quyết đơn sữa chữa."
            };
            _kdDao.Insert(luuvetKyduyet);
            

            msg = new Message("Cập nhật thành công", MessageType.Info, "Cập nhật sữa chữa ");
            return msg;

        }

        public Message CapNhatSauSuaChua(GIAIQUYETTHONGTINSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            GIAIQUYETTHONGTINSUACHUA objDb = _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Equals(objUi.MADON)).SingleOrDefault();

            objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVXL));
            objDb.KHACHHANG = !string.IsNullOrEmpty(objUi.IDKH) ? _db.KHACHHANGs.Single(p => p.IDKH.Equals(objUi.IDKH)) : null;


            objDb.XACNHAN = objUi.XACNHAN;
            objDb.BIENBAN = objUi.BIENBAN;
            objDb.LANXL = objUi.LANXL;
            objDb.LYDO = objUi.LYDO;
            objDb.CODH = objUi.CODH;
            objDb.CHINIEM = objUi.CHINIEM;
            objDb.MADH = objUi.MADH;
            objDb.CSTRUOC = objUi.CSTRUOC;
            objDb.CSSAU = objUi.CSSAU;
            objDb.ISLAPCHIETTINH = objUi.ISLAPCHIETTINH;
            objDb.THONGTINKH = objUi.THONGTINKH;
            objDb.SDT = objUi.SDT;
            objDb.GHICHU = objUi.GHICHU;
            objDb.THONGTINXULY = _db.THONGTINXULies.Single(p => p.MAXL.Equals(objUi.MAXL));

            if (objUi.TTDON.Equals(TTSC.SC_F.ToString()))
            {
                objDb.TTDON = TTSC.SC_F.ToString();
                objDb.NGAYHT = objUi.NGAYHT;

                if (!string.IsNullOrEmpty(objUi.IDKH))
                {
                    KHACHHANG khachhang = _db.KHACHHANGs.Single(p => p.IDKH.Equals(objUi.IDKH));
                    khachhang.THUYLK = objUi.CODH.ToString();
                    if (!string.IsNullOrEmpty(objUi.MADH))
                    {
                        khachhang.MADH = objUi.MADH;
                    }
                }
            }
            else
            {
                objDb.TTDON = TTSC.SC_I.ToString();
            }


            _db.SubmitChanges();

            
            var luuvetKyduyet = new LUUVET_KYDUYET
            {
                MADON = objDb.IDKH,
                IPAddress = ipAddress,
                MANV = sManv,
                UserAgent = useragent,
                NGAYTHUCHIEN = DateTime.Now,
                TACVU = TACVUKYDUYET.I.ToString(),
                MACN = CHUCNANGKYDUYET.KH01.ToString(),
                MATT = TTDK.DK_A.ToString(),
                MOTA = "Cập nhật sau sữa chữa."
            };
            _kdDao.Insert(luuvetKyduyet);
           

            msg = new Message("Cập nhật thành công", MessageType.Info, "Cập nhật sữa chữa ");
            return msg;

        }

        public Message ApproveChietTinhList(List<GIAIQUYETTHONGTINSUACHUA> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    // Get current Item in db
                    var objDb = Get(objUi.MADON);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thông tin sửa chữa", "");
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_A.ToString();

                    // update thiet ke
                    var ct = _db.CHIETTINHSUACHUAs.Where(t => t.MADON.Equals(objDb.MADON)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính sửa chữa", "");
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADON,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTCT.CT_A.ToString(),
                        MOTA = "Duyệt chiết tính sửa chữa"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách chiết tính sửa chữa");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính sửa chữa", ex.Message);
            }

            return msg;
        }

        public Message RejectChietTinhList(List<GIAIQUYETTHONGTINSUACHUA> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    // Get current Item in db
                    var objDb = Get(objUi.MADON);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thông tin sửa chữa", "");
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_RA.ToString();

                    // update thiet ke
                    var ct = _db.CHIETTINHSUACHUAs.Where(t => t.MADON.Equals(objDb.MADON)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính sửa chữa", "");
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADON,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTCT.CT_RA.ToString(),
                        MOTA = "Từ chối chiết tính sửa chữa"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_REJECT_SUCCEED, MessageType.Info, "danh sách chiết tính sửa chữa");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }


        /// <summary>
        /// Danh sách tra cứu chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<GIAIQUYETTHONGTINSUACHUA> GetListForTraCuuChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.GIAIQUYETTHONGTINSUACHUAs.Where(d => (d.TTCT == TTCT.CT_N.ToString() || d.TTCT == TTCT.CT_P.ToString() ||
                                                        d.TTCT == TTCT.CT_RA.ToString() || d.TTCT == TTCT.CT_A.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADON.Contains(keyword) ||
                                      d.CHIETTINHSUACHUA.TENCT.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.CHIETTINHSUACHUA.NGAYLCT.HasValue
                                           && d.CHIETTINHSUACHUA.NGAYLCT.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.CHIETTINHSUACHUA.NGAYLCT.HasValue
                                           && d.CHIETTINHSUACHUA.NGAYLCT.Value <= toDate.Value);
            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            return result.OrderByDescending(d => d.MADON).OrderByDescending(d => d.CHIETTINHSUACHUA.NGAYLCT).ToList();
        }

        public List<GIAIQUYETTHONGTINSUACHUA> GetListBienBanPB(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            var result = from gq in _db.GIAIQUYETTHONGTINSUACHUAs
                         join  kh in _db.KHACHHANGs on gq.IDKH equals kh.IDKH
                         where gq.TTDON == "SC_P"
                         select gq;

            if (keyword != null)
                result = result.Where(d => (d.KHACHHANG.MADP + d.KHACHHANG.MADB).Contains(keyword) ||
                                      d.KHACHHANG.TENKH.Contains(keyword));            
            
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.KHACHHANG.MADP + d.KHACHHANG.MADB).ToList();
        }


    }
}
