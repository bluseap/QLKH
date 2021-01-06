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
    public class KhachHangPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        private readonly DotInHDDao _dihdDao = new DotInHDDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly TieuThuPoDao _ttpoDao = new TieuThuPoDao();
        private readonly ThayDongHoPoDao _tdhpoDao = new ThayDongHoPoDao();
        private readonly ReportClass report = new ReportClass();     

        public KhachHangPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KHACHHANGPO Get(string ma)
        {
            return _db.KHACHHANGPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma));
        }

        public KHACHHANGPO GetMADDK(string ma)
        {
            return _db.KHACHHANGPOs.FirstOrDefault(p => p.MADDKPO.Equals(ma));
        }

        public KHACHHANGPO GetKhachHangFromMadb(string MAKH)
        {
            return _db.KHACHHANGPOs.FirstOrDefault(p => (p.MADPPO + p.DUONGPHUPO + p.MADBPO) == MAKH);
        }

        public KHACHHANGPO GetKHDBKV(string MAKH, string makv)
        {
            return _db.KHACHHANGPOs.FirstOrDefault(p => (p.MADPPO + p.DUONGPHUPO + p.MADBPO) == MAKH && p.MAKVPO == makv);
        }

        public KHACHHANGPO GetMADH(string madh)
        {
            return _db.KHACHHANGPOs.FirstOrDefault(p => p.MADHPO.Equals(madh));
        }
     
        public List<KHACHHANGPO> GetList()
        {
            return _db.KHACHHANGPOs.ToList();
        }

        public List<KHACHHANGPO> GetList(string madp)
        {
            return _db.KHACHHANGPOs.Where(kh => kh.MADPPO.Equals(madp))
                //.OrderBy(kh => kh.STT).ToList();
                .OrderBy(kh => kh.MADPPO).OrderBy(kh => kh.MADBPO).OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANGPO> GetList(string madp, int fromStt, int toStt)
        {
            return _db.KHACHHANGPOs.Where(kh => kh.MADPPO.Equals(madp) &&
                                                kh.STT >= fromStt &&
                                                kh.STT <= toStt)
                .OrderBy(kh => kh.STT).ToList();
        }

        public List<KHACHHANGPO> SearchKhachHang(string maDanhBo, string tenKhachHang, string maDongHo, string soHopDong, 
            string soNha, string duongPho, string maKv)
        {
            if (maDanhBo == "" && tenKhachHang == "" && maDongHo == "" && soHopDong == "" && soNha == "" && duongPho == "" && (maKv == "" || maKv == "NULL"))
                return null;

            var query = _db.KHACHHANGPOs.Where(kh => kh.XOABOKHPO == false || kh.XOABOKHPO == null).AsEnumerable(); 
            
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

            if (!string.IsNullOrEmpty(duongPho))
                query = query.Where(kh => (kh.DUONGPHOPO.TENDP.ToUpper().Contains(duongPho.ToUpper()) || kh.MADPPO.Equals(duongPho)));

            if (!string.IsNullOrEmpty(maKv))
                query = query.Where(kh => (maKv=="%"  || kh.MAKVPO.Equals(maKv)));

            return query.ToList();
        }

        public List<KHACHHANGPO> SearchKhachHangThayDH(string maDanhBo, string tenKhachHang, string maDongHo, string soHopDong,
            string soNha, string duongPho, string maKv, int thang, int nam, string trangthaighi)
        {
            if (maDanhBo == "" && tenKhachHang == "" && maDongHo == "" && soHopDong == "" && soNha == "" && duongPho == "" && (maKv == "" || maKv == "NULL"))
                return null;

            var khthaydh = from kh in _db.KHACHHANGPOs
                           join tt in _db.TIEUTHUPOs on kh.IDKHPO equals tt.IDKHPO
                           where tt.THANG.Equals(thang) && tt.NAM.Equals(nam) && tt.TTHAIGHI.Equals(trangthaighi)
                           select kh;

            var query = khthaydh.Where(kh => kh.XOABOKHPO == false).AsEnumerable();
            //var query = _db.KHACHHANGPOs.Where(kh => kh.XOABOKHPO == false || kh.XOABOKHPO == null).AsEnumerable();

            if (!string.IsNullOrEmpty(maDanhBo))
                query = query.Where(kh => (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).ToUpper().Contains(maDanhBo.ToUpper()));

            if (!string.IsNullOrEmpty(tenKhachHang))
                query = query.Where(kh => (kh.TENKH.ToUpper().Contains(tenKhachHang.ToUpper())));

            if (!string.IsNullOrEmpty(maDongHo))
                query = query.Where(kh => (kh.MALDHPO.ToUpper().Contains(maDongHo.ToUpper())));

            if (!string.IsNullOrEmpty(soHopDong))
                query = query.Where(kh => (kh.SOHD.ToUpper().Contains(soHopDong.ToUpper())));

            if (!string.IsNullOrEmpty(soNha))
                query = query.Where(kh => (kh.SONHA.ToUpper().Contains(soNha.ToUpper())));

            if (!string.IsNullOrEmpty(duongPho))
                query = query.Where(kh => (kh.DUONGPHOPO.TENDP.ToUpper().Contains(duongPho.ToUpper()) || kh.MADPPO.Equals(duongPho)));

            if (!string.IsNullOrEmpty(maKv))
                query = query.Where(kh => (maKv == "%" || kh.MAKVPO.Equals(maKv)));

            return query.ToList();
        }

        public int Count( )
        {
            return _db.KHACHHANGPOs.Count();
        }
        
        public Message Insert(KHACHHANGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                //var count = _db.KHACHHANGPOs.Count(kh => kh.IDKHPO.Equals(objUi.IDKHPO) ||
                //                                       (objUi.IDKHPO != null &&
                //                                        kh.MADPPO.Equals(objUi.MADPPO) &&
                //                                        kh.DUONGPHUPO.Equals(objUi.DUONGPHUPO) &&
                //                                        kh.MADBPO.Equals(objUi.MADBPO)));

                var count = _db.KHACHHANGPOs.Count(kh => kh.IDKHPO.Equals(objUi.IDKHPO) );

                if (count > 0)
                {
                    // success message
                    return new Message(MessageConstants.E_KH_MADB_TONTAI, MessageType.Error, "Thêm mới khách hàng");
                }

                // insert to KHACHHANG
                _db.KHACHHANGPOs.InsertOnSubmit(objUi);

                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKHPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Nhập khách hàng mới điện."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // commit
                // success message
                
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "khách hàng ");
                
                
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "Khách hàng ", objUi.TENKH);
            }
            return msg;
        }

        public Message UpdateTTSD(KHACHHANGPO obj, int thang, int nam, string ghichu, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(obj.IDKHPO);

                if (cu != null)
                {
                    //TODO: update all fields
                    cu.TTSD = obj.TTSD;
                    _db.SubmitChanges();

                    var ttsd = new TTSDKHACHHANG
                                   {
                                       THANG = thang,
                                       NAM = nam,
                                       IDKH = cu.IDKHPO,
                                       TTSD = cu.TTSD,                                       
                                       NGAYTHAYDOI = DateTime.Now,
                                       MANVTD = sManv,
                                       GHICHU = ghichu
                                   };
                    _db.TTSDKHACHHANGs.InsertOnSubmit(ttsd);
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = obj.TTSD == "CUP" ? "Cúp nước khách hàng" : "Mở nước khách hàng"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng ");
                    
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng ", obj.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpdateThayDongHoKyThay(KHACHHANGPO obj, DateTime ngayht, string tem, string ghichu, String useragent, String ipAddress, String sManv,
                                string madh, string maldh, DateTime ngaythay, DateTime ngayhoanthanh, string madhkh, string dhcap, DateTime kythay)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var cu = Get(obj.IDKHPO);

                var dotinhd = _dihdDao.Get(cu.DOTINHD != null ? cu.DOTINHD : "");

                string idmadotin = "";

                if (dotinhd != null && dotinhd.MADOTIN == "DDP7D1")
                {
                    idmadotin = cu.DOTINHD;
                }
                else
                {
                    idmadotin = _dppoDao.GetDP(cu.MADPPO) != null ? _dppoDao.GetDP(cu.MADPPO).IDMADOTIN : "";
                }                

                if (cu != null)
                {
                    //TODO: insert into thay dong ho
                    var thaydongho = new THAYDONGHOPO
                    {
                        IDKHPO = obj.IDKHPO,
                        MADHCU = madh,
                        MALDHCU = maldh,
                        NGAYTDCU = ngaythay,
                        NGAYHTCU = ngayhoanthanh,
                        NGAYTD = obj.NGAYTHAYDH,
                        NGAYHT = ngayht,
                        MALDHPO = obj.MALDHPO,
                        MADHPO = obj.MADHPO,
                        KICHCO = obj.THUYLK,
                        GHICHU = ghichu,
                        SOTEM = tem,
                        MANVTD = sManv,
                        CHISONGUNG = obj.CHISODAU,
                        CHISOBATDAU = obj.CHISOCUOI,
                        MTRUYTHU = obj.m4Poor,
                        CHISOMOI = obj.KLKHOAN,
                        KYTHAYDH = kythay,
                        MADHCUNO = madhkh,
                        DHCAPBAN = dhcap,
                        MADPPO = obj.MADPPO,
                        MADBPO = obj.MADBPO,

                        IDMADOTIN = idmadotin, //dotin != null ? dotin.IDMADOTIN : "",
                        //KYTHAYDH =  DateTimeUtil.GetVietNamDate("01/06/2014")
                        LYDOTHAY = obj.DIACHI_INHOADON,

                        NGAYN = DateTime.Now
                    };
                    _db.THAYDONGHOPOs.InsertOnSubmit(thaydongho);
                    _db.SubmitChanges();

                    //TODO: update khach hang
                    cu.THUYLK = obj.THUYLK;
                    cu.MALDHPO = "CV140";
                    cu.MADHPO = obj.MADHPO;
                    cu.NGAYTHAYDH = obj.NGAYTHAYDH;
                    cu.NGAYHT = obj.NGAYHT;
                    cu.CHISODAU = 0;
                    cu.CHISOCUOI = obj.CHISOCUOI;

                    cu.SLANTHAYDH = 1;

                    _db.SubmitChanges();

                    #region Luu Vet

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "TDHPO",
                        MOTA = @"Thay đồng hồ điện."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "thay đồng hồ điện khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng", obj.TENKH);
                }

                // commit
                trans.Commit();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpdateThayDongHo(KHACHHANGPO obj, DateTime ngayht, string tem, string ghichu, String useragent, String ipAddress, String sManv,
                                string madh, string maldh, DateTime ngaythay, DateTime ngayhoanthanh, string madhkh, string dhcap)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var cu = Get(obj.IDKHPO);

                var dotin = _dppoDao.GetDP(cu.MADPPO);

                if (cu != null)
                {
                    //TODO: insert into thay dong ho
                    var thaydongho = new THAYDONGHOPO
                                         {
                                             IDKHPO = obj.IDKHPO,
                                             MADHCU = madh,
                                             MALDHCU = maldh,
                                             NGAYTDCU = ngaythay,
                                             NGAYHTCU = ngayhoanthanh,                                             
                                             NGAYTD = obj.NGAYTHAYDH,
                                             NGAYHT = ngayht,
                                             MALDHPO = obj.MALDHPO,
                                             MADHPO = obj.MADHPO,
                                             KICHCO = obj.THUYLK,
                                             GHICHU = ghichu,
                                             SOTEM = tem,
                                             MANVTD = sManv,
                                             CHISONGUNG=obj.CHISODAU,
                                             CHISOBATDAU=obj.CHISOCUOI,
                                             MTRUYTHU = obj.m4Poor,
                                             CHISOMOI = obj.KLKHOAN,                                             
                                             KYTHAYDH = DateTime.Now,
                                             MADHCUNO = madhkh,
                                             DHCAPBAN = dhcap,

                                             MADPPO = obj.MADPPO,
                                             MADBPO = obj.MADBPO,
                                             IDMADOTIN = dotin != null ? dotin.IDMADOTIN : ""
                                             //KYTHAYDH =  DateTimeUtil.GetVietNamDate("01/06/2014")
                                         };
                    _db.THAYDONGHOPOs.InsertOnSubmit(thaydongho);
                    _db.SubmitChanges();

                    //TODO: update khach hang
                    cu.THUYLK = obj.THUYLK;
                    cu.MALDHPO = "CV140";
                    cu.MADHPO = obj.MADHPO;
                    cu.NGAYTHAYDH = obj.NGAYTHAYDH;
                    cu.NGAYHT = obj.NGAYHT;
                    cu.CHISODAU = 0;
                    cu.CHISOCUOI = obj.CHISOCUOI;
                    
                    cu.SLANTHAYDH = 1;                 

                    _db.SubmitChanges();

                    #region Luu Vet

                    var luuvetKyduyet = new LUUVET_KYDUYET
                                            {
                                                MADON = obj.IDKHPO,
                                                IPAddress = ipAddress,
                                                MANV = sManv,
                                                UserAgent = useragent,
                                                NGAYTHUCHIEN = DateTime.Now,
                                                TACVU = TACVUKYDUYET.U.ToString(),
                                                MACN = CHUCNANGKYDUYET.KH05.ToString(),
                                                MATT = "TDHPO",
                                                MOTA = @"Thay đồng hồ điện."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "thay đồng hồ điện khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng", obj.TENKH);
                }

                // commit
                trans.Commit();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpThayDongHo(THAYDONGHOPO obj, DateTime ngayht, string tem, string ghichu, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var cu = _tdhpoDao.Get(obj.ID);

                if (cu != null)
                {
                    /*cu.CHISONGUNG = obj.CHISONGUNG;
                    cu.MTRUYTHU = obj.MTRUYTHU;
                    cu.MALDH = obj.MALDH;
                    cu.MADH = obj.MADH;
                    cu.CHISOBATDAU = obj.CHISOBATDAU;
                    cu.CHISOMOI = obj.CHISOMOI;
                    cu.NGAYTD = obj.NGAYTD;
                    cu.NGAYHT = obj.NGAYHT;
                    cu.KICHCO = obj.KICHCO;
                    cu.GHICHU = ghichu;

                    _db.SubmitChanges(); */

                    //TODO: update khach hang
                    var kh = Get(obj.IDKHPO);
                    kh.THUYLK = obj.KICHCO;
                    //kh.LOAIDH = obj.MALDH != null ? _db.LOAIDHs.Single(p => p.MALDH.Equals(obj.MALDH)) : null;
                    kh.MADHPO = obj.MADHPO;
                    kh.NGAYTHAYDH = obj.NGAYTD;
                    kh.NGAYHT = obj.NGAYHT;
                    kh.CHISODAU = 0;
                    kh.CHISOCUOI = obj.CHISOMOI;                    

                    _db.SubmitChanges();
                    #region Luu Vet

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = obj.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = @"Thay đồng hồ điện"
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "thay đồng hồ khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thay đồng hồ", obj.IDKHPO);
                }

                // commit
                trans.Commit();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng");
            }

            return msg;
        }

        public Message UpdateSoBo(KHACHHANGPO moi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(moi.IDKHPO);

                if (cu != null)
                {
                    cu.STT = moi.STT;
                    cu.TENKH = moi.TENKH;
                    cu.SONHA = moi.SONHA;
                    cu.MADPPO = moi.MADPPO;
                    cu.MADBPO = moi.MADBPO;

                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = "Cập nhật sổ bộ"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }

        public Message UpdateMaDoan(KHACHHANGPO moi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(moi.IDKHPO);

                if (cu != null)
                {
                    var madoancu = cu.MADOAN;

                    var doan = new DoanDao().Get(moi.MADOAN, moi.MAKVPO);
                    if(doan == null)
                    {
                        return new Message(MessageConstants.E_INVALID_DATA, MessageType.Error, "Mã đoạn");
                    }

                    cu.MADOAN = moi.MADOAN;

                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = string.Format("Cập nhật mã đoạn: {0} -> {1}", madoancu, moi.MADOAN)
                    };

                    _kdDao.Insert(luuvetKyduyet);

                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }

        public Message Update(KHACHHANGPO moi, int nam, int thang, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                /*var count = _db.KHACHHANGs.Count(kh => !kh.IDKH.Equals(moi.IDKH) && 
                                                       kh.MADP.Equals(moi.MADP) &&
                                                       kh.DUONGPHU.Equals(moi.DUONGPHU) &&
                                                       kh.MADB.Equals(moi.MADB));
                
                if (count > 0)
                {
                    // success message
                    return new Message(MessageConstants.E_KH_MADB_TONTAI, MessageType.Error, "Cập nhật khách hàng");
                }*/

                _db.Connection.Close();
                _db.Connection.Open();

                // get current object in database
                var cu = Get(moi.IDKHPO);
                
                if (cu != null)
                {
                    //TODO: update all fields
                    cu.MADPPO = moi.MADPPO;
                    cu.MADBPO = moi.MADBPO;
                    //cu.DUONGPHUPO = moi.DUONGPHUPO;
                    //cu.MALKHDB = moi.MALKHDB;
                    //cu.MACQ = moi.MACQ;
                    cu.MAMDSDPO = moi.MAMDSDPO;
                    //cu.SOHD = moi.SOHD;
                    //cu.LOTRINH = moi.LOTRINH;
                    //cu.STT = moi.STT;
                    //cu.MABG = moi.MABG;
                    //cu.MAPHUONGPO = moi.MAPHUONGPO;
                    cu.TENKH = moi.TENKH;
                   
                 //   cu.SONHA = moi.SONHA;

                    cu.SOTRUKD = moi.SOTRUKD;

                 //   cu.SOHO = moi.SOHO;
                  //  cu.MST = moi.MST;
                    //cu.MAHTTT = moi.MAHTTT;
                 //   cu.STK = moi.STK;
                  //  cu.SDT = moi.SDT;
                    //cu.MAKVPO = moi.MAKVPO;
                    //cu.MADHPO = moi.MADHPO;
                    //cu.NGAYHT = moi.NGAYHT;
                    //cu.NGAYTHAYDH = moi.NGAYTHAYDH;
                    //cu.NGAYCUP = moi.NGAYCUP;
                    //cu.NGAYGNHAP = moi.NGAYGNHAP;
                    //cu.SLANTHAYDH = moi.SLANTHAYDH;
                    //cu.TTSD = moi.TTSD;
                    //cu.KOPHINT = moi.KOPHINT;
                    //cu.THUYLK = moi.THUYLK;
                    //cu.MATT = moi.MATT;
                    //cu.VAT = moi.VAT;
                    //cu.SDInfo_INHOADON = moi.SDInfo_INHOADON;
                    //cu.TENKH_INHOADON = moi.TENKH_INHOADON;
                    //cu.DIACHI_INHOADON = moi.DIACHI_INHOADON;
                    //cu.THANGBDKT = moi.THANGBDKT;
                   // cu.NAMBDKT = moi.NAMBDKT;
                   // cu.CHISOCUOI = moi.CHISOCUOI;
                    //cu.CHISODAU = moi.CHISODAU;
                   // cu.SONK = moi.SONK;
                   // cu.ISDINHMUC = moi.ISDINHMUC;
                    //cu.GHI2THANG1LAN = moi.GHI2THANG1LAN;
                    //cu.KHONGTINH117 = moi.KHONGTINH117;
                    //cu.KYHOTRO = moi.KYHOTRO;
                  //  cu.CMND = moi.CMND;

                    cu.STTTS = moi.STTTS != null ? moi.STTTS : 1;

                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = "Cập nhập khách hàng điện."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion                                    
                    
                    _db.Connection.Close();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng ");
                    
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH     );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }
        
        public bool IsInUse(string ma)
        {
            if (_db.TIEUTHUPOs.Count(p => p.IDKHPO.Equals(ma)) > 0)
                return true;
            else if (_db.KHACHHANG_EDITs.Count(p => p.IDKH.Equals(ma)) > 0)
                return true;
            else
            {
                return false;
            }
        }

        public bool ExistsMaKhachHang(string idkh, string madp, string madb)
        {
            return
                (_db.KHACHHANGPOs.Count(p => (p.IDKHPO.Equals(idkh) || string.IsNullOrEmpty(idkh)) && p.MADPPO.Equals(madp) && p.MADBPO.Equals(madb)) > 0);

        }

        public bool ExistsAnotherMaKhachHang(string idkh, string madp, string madb)
        {
            return
                (_db.KHACHHANGPOs.Count(p => (p.IDKHPO.Equals(idkh) || string.IsNullOrEmpty(idkh)) && p.MADPPO.Equals(madp) && p.MADBPO.Equals(madb)) > 1);

        }

        public Message Delete(KHACHHANGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.IDKHPO);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Khách hàng ", objUi.TENKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

              
                // Set delete info
                _db.KHACHHANGPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKHPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "Xóa khách hàng điện"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Khách hàng");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Khách hàng ");
            }

            return msg;
        }
        
        public Message DeleteList(List<KHACHHANGPO> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                    Delete(obj, useragent, ipAddress, sManv );
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Khách hàng ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách khách hàng ");
            }

            return msg;
        }

        public string NewId()
        {
            var query = _db.KHACHHANGPOs.Max(p => p.IDKHPO);

            //var temp = int.Parse(query) + 1;
            //return temp.ToString("D6");

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                query = temp.ToString("D6");
            }
            else
            {
                query = "000001";
            }
            return query;
        }

        public int? NewSTTTS(string makvpo)
        {
            var query = _db.KHACHHANGPOs.Where(p => p.MAKVPO.Equals(makvpo)).Max(p => p.STTTS);

            if (query != null)
            {
                query = query + 1;
            }
            else
            {
                query = 1;
            }

            return query;
        }

        public string NewMADB(string maDP)
        {
            var query = (from p in _db.KHACHHANGPOs.Where(p => p.MADPPO.Equals(maDP))
                        select p.MADBPO).Max();
            if (!string.IsNullOrEmpty(query))
            {
                var bn = 20;
                var dp = _db.DUONGPHOPOs.FirstOrDefault(d => d.MADPPO.Equals(maDP));
                if (dp != null && dp.BUOCNHAY.HasValue && dp.BUOCNHAY > 0)
                    bn = dp.BUOCNHAY.Value;

                var temp = int.Parse(query) + bn;
                
                return temp.ToString("D4");
            }

            return "0001";
           
        }

        public int NewSTT(string maDp)
        {
            var query = (from p in _db.KHACHHANGPOs.Where(p => p.MADPPO.Equals(maDp))
                         select p.STT).Max();

            if (query.HasValue)
                return query.Value + 1;

            return 1;
        }

        public List<KHACHHANGPO> GetListInKKT(int thang, int nam)
        {
            return _db.KHACHHANGPOs.Where(k => (k.THANGBDKT.HasValue && 
                                                k.THANGBDKT.Value.Equals(thang) &&
                                                k.NAMBDKT.HasValue &&
                                                k.NAMBDKT.Value.Equals(nam)))
                                            .OrderByDescending(k => k.IDKHPO).ToList();
        }

        public List<KHACHHANGPO> GetListKH(int thang, int nam)
        {
            return _db.KHACHHANGPOs.Where(k => k.KYKHAITHAC.Month==thang && k.KYKHAITHAC.Year==nam)
                                            .OrderByDescending(k => k.IDKHPO).ToList();
        }

        public List<KHACHHANGPO> GetListKhuVuc(int thang, int nam, string makv)
        {
            return _db.KHACHHANGPOs.Where(k => k.KYKHAITHAC.Month == thang && k.KYKHAITHAC.Year == nam && k.MAKVPO.Equals(makv))
                                            .OrderByDescending(k => k.IDKHPO).ToList();
        }

        public List<KHACHHANGPO> GetList(String IDKH, String SOHD, String MADH, String TENKH, String SONHA, String TENDP, String MAKV, String GHI2THANG1LAN)
        {
            //var dskh = from kh in _db.KHACHHANGs
            //           select kh;

            var dskh = from kh in _db.KHACHHANGPOs join d in _db.DONGHOPOs on kh.MADHPO equals d.MADHPO
                       select kh;

            if (IDKH != null)
                dskh = dskh.Where(kh => (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).Contains(IDKH));

            if (SOHD != null)
                dskh = dskh.Where(kh => kh.SOHD.Equals(SOHD));

            if (MADH != null)
                //dskh = dskh.Where(kh => kh.MADH.Contains(MADH));
                dskh = dskh.Where(kh => kh.DONGHOPO.SONO.Contains(MADH));

            if (TENKH != null)
                dskh = dskh.Where(kh => kh.TENKH.Contains(TENKH));

            if (SONHA != null)
                dskh = dskh.Where(kh => kh.SONHA.Contains(SONHA));

            if (TENDP != null)
                dskh = dskh.Where(kh => kh.DUONGPHOPO.TENDP.Contains(TENDP) || kh.MADPPO.Equals(TENDP));

            if (MAKV != null)
                dskh = dskh.Where(kh => kh.MAKVPO.Equals(MAKV));

            if (GHI2THANG1LAN != null)
                dskh = dskh.Where(kh => kh.XOABOKHPO.Equals(Convert.ToInt16(GHI2THANG1LAN)));              

            return dskh
                        .OrderBy(kh => kh.STT)
                        .OrderBy(kh => kh.DUONGPHUPO)
                        .OrderBy(kh => kh.MADPPO)
                        .ToList();
        }

        public List<KHACHHANGPO> GetListMaKhachHang(String IDKH, String SOHD, String MADH, String TENKH, String SONHA, String TENDP,
            String MAKV, String GHI2THANG1LAN, string makhachhang, string cmnd)
        {          

            var dskh = from kh in _db.KHACHHANGPOs
                       join d in _db.DONGHOPOs on kh.MADHPO equals d.MADHPO
                       select kh;

            if (IDKH != null)
                dskh = dskh.Where(kh => (kh.MADPPO + kh.DUONGPHUPO + kh.MADBPO).Contains(IDKH));

            if (SOHD != null)
                dskh = dskh.Where(kh => kh.SOHD.Equals(SOHD));

            if (MADH != null)
                //dskh = dskh.Where(kh => kh.MADH.Contains(MADH));
                dskh = dskh.Where(kh => kh.DONGHOPO.SONO.Contains(MADH));

            if (TENKH != null)
                dskh = dskh.Where(kh => kh.TENKH.Contains(TENKH));

            if (SONHA != null)
                dskh = dskh.Where(kh => kh.SONHA.Contains(SONHA));

            if (TENDP != null)
                dskh = dskh.Where(kh => kh.DUONGPHOPO.TENDP.Contains(TENDP) || kh.MADPPO.Equals(TENDP));

            if (MAKV != null)
                dskh = dskh.Where(kh => kh.MAKVPO.Equals(MAKV));

            if (GHI2THANG1LAN != null)
                dskh = dskh.Where(kh => kh.XOABOKHPO.Equals(Convert.ToInt16(GHI2THANG1LAN)));

            if (makhachhang != null)
                dskh = dskh.Where(kh => kh.MAKVPO.Equals(makhachhang.Substring(0, 1)) && kh.IDKHPO.Equals(makhachhang.Substring(1, 6)));

            if (cmnd != null)
            {
                var khachhangmoi = from kh in dskh
                                   join don in _db.DONDANGKYPOs on kh.MADDKPO equals don.MADDKPO
                                   where don.CMND == cmnd
                                   select kh;

                dskh = khachhangmoi;
            }

            return dskh
                        .OrderBy(kh => kh.STT)
                        .OrderBy(kh => kh.DUONGPHUPO)
                        .OrderBy(kh => kh.MADPPO)
                        .ToList();
        }

        public List<TIEUTHUPO> GetThongTinTieuThu(string idkh)
        {
            return _db.TIEUTHUPOs.Where(tt => tt.IDKHPO.Equals(idkh))
                .OrderByDescending(tt => tt.THANG)
                .OrderByDescending(tt => tt.NAM).ToList();
        }

        public List<TTSDKHACHHANG> GetTTSDList(int thang, int nam, string ttsd)
        {
            return
                _db.TTSDKHACHHANGs.Where(tt => (tt.THANG == thang && tt.NAM == nam && tt.TTSD == ttsd))
                        .OrderByDescending(tt => tt.NGAYTHAYDOI).ToList();
        }

        public TTSDKHACHHANG GetTTSD(int id)
        {
            return _db.TTSDKHACHHANGs.SingleOrDefault(tt => tt.ID == id); 
        }

        public List<THAYDONGHOPO> GetThayDongHoList(int thang, int nam)
        {
            return
                _db.THAYDONGHOPOs.Where(tt =>tt.NGAYTD .HasValue && tt.NGAYTD.Value.Month == thang && tt.NGAYTD.Value.Year == nam  ).ToList();
        }

        public List<THAYDONGHOPO> GetThayDongHoList()
        {
            return _db.THAYDONGHOPOs.OrderByDescending(t => t.ID).ToList();
        }

        public List<THAYDONGHOPO> GetThayDongHoListThang(int thang)
        {
            return _db.THAYDONGHOPOs.Where(t => t.KYTHAYDH.Value.Month == thang).ToList();
        }

        public List<THAYDONGHOPO> GetThayDongHoListThangNam(int thang, int nam)
        {
            return _db.THAYDONGHOPOs.Where(t => t.KYTHAYDH.Value.Month == thang && t.KYTHAYDH.Value.Year == nam).ToList();
        }

        public List<THAYDONGHOPO> GetTHDKV(int thang, int nam, string makv)
        {
            return _db.THAYDONGHOPOs.Where(t => t.KYTHAYDH.Value.Month == thang && t.KYTHAYDH.Value.Year == nam
                            && t.KHACHHANGPO.MAKVPO.Equals(makv)).ToList();
        }

        public List<THAYDONGHOPO> GetListTDHDotIn(int thang, int nam, string makv, string idmadotin)
        {
            var query = from th in _db.THAYDONGHOPOs
                        join kh in _db.KHACHHANGPOs on th.IDKHPO equals kh.IDKHPO
                        join dp in _db.DUONGPHOPOs on kh.MADPPO equals dp.MADPPO
                        where dp.IDMADOTIN.Equals(idmadotin) && th.KYTHAYDH.Value.Month == thang && th.KYTHAYDH.Value.Year == nam
                        select th;

            //query.Where(t => t.KYTHAYDH.Value.Month == thang && t.KYTHAYDH.Value.Year == nam
            //                       && t.KHACHHANG.MAKV.Equals(makv));

            return query.ToList();
        }

        public List<THAYDONGHOPO> GetListTDHDotInP7D1(int thang, int nam, string makv, string idmadotin)
        {
            var query = from th in _db.THAYDONGHOPOs
                        join kh in _db.KHACHHANGPOs on th.IDKHPO equals kh.IDKHPO
                        //join dp in _db.DUONGPHOPOs on kh.MADPPO equals dp.MADPPO
                        where kh.MAKVPO.Equals(makv) && th.KYTHAYDH.Value.Month == thang && th.KYTHAYDH.Value.Year == nam 
                            && kh.DOTINHD.Equals(idmadotin)
                        select th;
          
            return query.ToList();
        }

        public List<THAYDONGHOPO> GetListTDHDotIn2(int thang, int nam, string makv, string idmadotin)
        {
            var query = from th in _db.THAYDONGHOPOs
                        join kh in _db.KHACHHANGPOs on th.IDKHPO equals kh.IDKHPO
                        join dp in _db.DUONGPHOPOs on kh.MADPPO equals dp.MADPPO
                        where kh.MAKVPO.Equals(makv) && th.KYTHAYDH.Value.Month == thang && th.KYTHAYDH.Value.Year == nam
                            && dp.IDMADOTIN.Equals(idmadotin)
                        select th;
           
            return query.ToList();
        }

        public Message UpdateKHMCSC(KHACHHANGPO moi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var cu = Get(moi.IDKHPO);
                if (cu != null)
                {
                    cu.CHISOCUOI = moi.CHISOCUOI;

                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", moi.TENKH);
            }
            return msg;
        }

        public Message InsertTS2(KHACHHANGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {                
                // insert to KHACHHANG
                _db.KHACHHANGPOs.InsertOnSubmit(objUi);

                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKHPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "KHMDDSTSON",
                    MOTA = "Nhập khách hàng mới điện."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion                

                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "khách hàng ");
            }
            catch (Exception ex)
            {                
                msg = ExceptionHandler.HandleInsertException(ex, "Khách hàng ", objUi.TENKH);
            }
            return msg;
        }

        public int IsKyKhaiThacCSCuoi(DateTime kyghi, string idkh)        
        {
            return _db.KHACHHANGPOs.Where(lck => lck.KYKHAITHAC.Month.Equals(kyghi.Month)
                                                     && lck.KYKHAITHAC.Year.Equals(kyghi.Year)
                                                     && lck.IDKHPO.Equals(idkh))
                                               .Count();
        }

        public Message UpdateKhachHangMaDHPo(KHACHHANGPO moi, int nam, int thang, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.Connection.Close();
                _db.Connection.Open();

                // get current object in database
                var cu = Get(moi.IDKHPO);

                if (cu != null)
                {
                    cu.MADHPO = moi.MADHPO;

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = moi.IDKHPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = moi.MADHPO,
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = CHUCNANGKYDUYET.KH05.ToString(),
                        MOTA = "Cập nhập thay đồng hồ, sửa đồng hồ điện khách hàng khi nhập nhầm"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // Submit changes to db
                    _db.SubmitChanges();

                    _db.Connection.Close();
                   
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng ");
                }
                else
                {                    
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Khách hàng ", moi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Khách hàng ", ex.ToString());
            }
            return msg;
        }

    }
}
