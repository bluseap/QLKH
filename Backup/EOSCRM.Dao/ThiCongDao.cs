using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ThiCongDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly ChietTinhDao _ctDao = new ChietTinhDao();

        public ThiCongDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THICONG Get(string ma)
        {
            return _db.THICONGs.SingleOrDefault(p => p.MADDK .Equals(ma));
        }

        public List<THICONG > Search(string key)
        {
            return _db.THICONGs.Where(p => p.DONDANGKY.TENKH.ToUpper().Contains(key.ToUpper()) || p.DONDANGKY.DUONGPHO .TENDP .ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<THICONG> GetList()
        {
            return _db.THICONGs.ToList();
        }

        public List<THICONG> GetListDangThiCong()
        {
            return _db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_P.ToString()))
                .OrderByDescending(tc => tc.NGAYGTC)
                .ToList();
        }

        public List<THICONG> GetListDangThiCong(String madon, String tenkh, String sonha, String tenduong, 
            String tenphuong, String tenkv, String manv, String tennv, DateTime? fromdate, DateTime? todate)
        {
            var query =_db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_P.ToString())).AsQueryable();

            if (!String.IsNullOrEmpty(madon))
                query = query.Where(tc => tc.MADDK.Contains(madon)).AsQueryable();

            if (!String.IsNullOrEmpty(tenkh))
                query = query.Where(tc => tc.DONDANGKY.TENKH.Contains(tenkh)).AsQueryable();
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(tc => tc.DONDANGKY.DIACHILD.Contains(sonha)).AsQueryable();
            if (!String.IsNullOrEmpty(tenduong))
                query = query.Where(tc => tc.DONDANGKY.DIACHILD.Contains(tenduong)).AsQueryable();
            if (!String.IsNullOrEmpty(tenphuong))
                query = query.Where(tc => tc.DONDANGKY.DIACHILD.Contains(tenphuong)).AsQueryable();
            if (!String.IsNullOrEmpty(tenkv))
                query = query.Where(tc => tc.DONDANGKY.DIACHILD.Contains(tenkv)).AsQueryable();

            if (!String.IsNullOrEmpty(manv))
                query = query.Where(tc => tc.MANV.Contains(manv)).AsQueryable();
            if (!String.IsNullOrEmpty(tennv))
                query = query.Where(tc => tc.NHANVIEN.HOTEN.Contains(tennv)).AsQueryable();

            if (fromdate.HasValue)
                query = query.Where(tc => tc.NGAYGTC >= fromdate.Value).AsQueryable();
            if (todate.HasValue)
                query = query.Where(tc => tc.NGAYGTC <= todate.Value).AsQueryable();


            return query.OrderBy(tc => tc.MADDK)
                .OrderByDescending(tc => tc.NGAYGTC)
                .ToList();
        }

        public List<THICONG> GetListDangThiCong2()
        {
            return _db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_P.ToString()) ||
                    tc.DONDANGKY.TTTC.Equals(TTTC.TC_A.ToString()))
                .OrderByDescending(tc => tc.NGAYGTC)
                .ToList();
        }

        public List<THICONG> GetListDaThiCong()
        {
            return _db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_A.ToString()))
                .OrderByDescending(tc => tc.NGAYGTC)
                .ToList();
        }

        public List<THICONG> GetListDaThiCong(String madon, String tenkh, String sonha, String tenduong,
            String tenphuong, String tenkv, String manv, String tennv, 
            String madh, int? chiso, String ongnhanh, int? duongkinh, decimal? somet, String soserial, String ghichu,
            DateTime? fromdate, DateTime? todate)
        {
            var query = _db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_A.ToString())).AsQueryable();

            if (!String.IsNullOrEmpty(madon))
                query = query.Where(tc => tc.MADDK.Contains(madon)).AsQueryable();

            if (!String.IsNullOrEmpty(tenkh))
                query = query.Where(tc => tc.DONDANGKY.TENKH.ToLower().Contains(tenkh.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(tc => tc.DONDANGKY.SONHA.ToLower().Contains(sonha.ToLower())).AsQueryable();
            
            if (!String.IsNullOrEmpty(tenduong))
                query = query.Where(tc => tc.DONDANGKY.TEN_DC_KHAC.ToLower().Contains(tenduong.ToLower()) ||
                    (tc.DONDANGKY.DUONGPHO != null && tc.DONDANGKY.DUONGPHO.TENDP.ToLower().Contains(tenduong.ToLower()))
                    ).AsQueryable();

            if (!String.IsNullOrEmpty(tenphuong))
                query = query.Where(tc => tc.DONDANGKY.PHUONG != null &&
                    tc.DONDANGKY.PHUONG.TENPHUONG.ToLower().Contains(tenphuong.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(tenkv))
                query = query.Where(tc => tc.DONDANGKY.KHUVUC != null &&
                    tc.DONDANGKY.KHUVUC.TENKV.ToLower().Contains(tenkv.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(manv))
                query = query.Where(tc => tc.MANV.ToLower().Contains(manv.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(tennv))
                query = query.Where(tc => tc.NHANVIEN != null &&
                    tc.NHANVIEN.HOTEN.ToLower().Contains(tennv.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(tc => tc.MADH.ToLower().Contains(madh.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(ongnhanh))
                query = query.Where(tc => tc.ONGNHANH.ToLower().Contains(ongnhanh.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(soserial))
                query = query.Where(tc => tc.SOSERIAL.ToLower().Contains(soserial.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(ghichu))
                query = query.Where(tc => tc.GHICHU.ToLower().Contains(ghichu.ToLower())).AsQueryable();

            if(chiso.HasValue)
                query = query.Where(tc => tc.CSDAU.Equals(chiso.Value)).AsQueryable();

            if (duongkinh.HasValue)
                query = query.Where(tc => tc.DUONGKINH.Equals(duongkinh.Value)).AsQueryable();

            if (somet.HasValue)
                query = query.Where(tc => tc.MTHUCTE.Equals(somet.Value)).AsQueryable();

            if (fromdate.HasValue)
                query = query.Where(tc => tc.NGAYHT >= fromdate.Value).AsQueryable();

            if (todate.HasValue)
                query = query.Where(tc => tc.NGAYHT <= todate.Value).AsQueryable();

            return query.OrderByDescending(tc => tc.NGAYGTC).ToList();
        }

        public List<THICONG> GetListTraCuuThiCong()
        {
            return _db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_A.ToString()) ||
                tc.DONDANGKY.TTTC.Equals(TTTC.TC_P.ToString()))
                .OrderByDescending(tc => tc.NGAYGTC)
                .ToList();
        }

        public List<THICONG> GetListTraCuuThiCong(String madon, String tenkh, String sonha, String tenduong,
            String tenphuong, String tenkv, String manv, String tennv,
            String madh, int? chiso, String ongnhanh, int? duongkinh, decimal? somet, String soserial, String ghichu,
            DateTime? fromdate, DateTime? todate)
        {
            var query = _db.THICONGs.Where(tc => tc.DONDANGKY.TTTC.Equals(TTTC.TC_A.ToString()) ||
                tc.DONDANGKY.TTTC.Equals(TTTC.TC_P.ToString())).AsQueryable();

            if (!String.IsNullOrEmpty(madon))
                query = query.Where(tc => tc.MADDK.Contains(madon)).AsQueryable();

            if (!String.IsNullOrEmpty(tenkh))
                query = query.Where(tc => tc.DONDANGKY.TENKH.ToLower().Contains(tenkh.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(tc => tc.DONDANGKY.SONHA.ToLower().Contains(sonha.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(tenduong))
                query = query.Where(tc => tc.DONDANGKY.TEN_DC_KHAC.ToLower().Contains(tenduong.ToLower()) ||
                    (tc.DONDANGKY.DUONGPHO != null && tc.DONDANGKY.DUONGPHO.TENDP.ToLower().Contains(tenduong.ToLower()))
                    ).AsQueryable();

            if (!String.IsNullOrEmpty(tenphuong))
                query = query.Where(tc => tc.DONDANGKY.PHUONG != null &&
                    tc.DONDANGKY.PHUONG.TENPHUONG.ToLower().Contains(tenphuong.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(tenkv))
                query = query.Where(tc => tc.DONDANGKY.KHUVUC != null &&
                    tc.DONDANGKY.KHUVUC.TENKV.ToLower().Contains(tenkv.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(manv))
                query = query.Where(tc => tc.MANV.ToLower().Contains(manv.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(tennv))
                query = query.Where(tc => tc.NHANVIEN != null &&
                    tc.NHANVIEN.HOTEN.ToLower().Contains(tennv.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(madh))
                query = query.Where(tc => tc.MADH.ToLower().Contains(madh.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(ongnhanh))
                query = query.Where(tc => tc.ONGNHANH.ToLower().Contains(ongnhanh.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(soserial))
                query = query.Where(tc => tc.SOSERIAL.ToLower().Contains(soserial.ToLower())).AsQueryable();

            if (!String.IsNullOrEmpty(ghichu))
                query = query.Where(tc => tc.GHICHU.ToLower().Contains(ghichu.ToLower())).AsQueryable();

            if (chiso.HasValue)
                query = query.Where(tc => tc.CSDAU.Equals(chiso.Value)).AsQueryable();

            if (duongkinh.HasValue)
                query = query.Where(tc => tc.DUONGKINH.Equals(duongkinh.Value)).AsQueryable();

            if (somet.HasValue)
                query = query.Where(tc => tc.MTHUCTE.Equals(somet.Value)).AsQueryable();

            if (fromdate.HasValue)
                query = query.Where(tc => tc.NGAYHT >= fromdate.Value).AsQueryable();

            if (todate.HasValue)
                query = query.Where(tc => tc.NGAYHT <= todate.Value).AsQueryable();

            return query.OrderByDescending(tc => tc.NGAYGTC).ToList();
        }

        public Message Insert(THICONG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

             
                _db.THICONGs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                var ddk = _db.DONDANGKies.SingleOrDefault(d => d.MADDK.Equals(objUi.MADDK));
                if (ddk != null) 
                    ddk.TTTC = TTTC.TC_P.ToString();
                else
                {
                    trans.Rollback();
                    return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập thi công", "Mã đơn đăng ký không tồn tại.");
                }

                _db.SubmitChanges();


                trans.Commit();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTTC.TC_N.ToString(),
                    MOTA = "Thêm mới thi công"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Thi công ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();
                 
                msg = ExceptionHandler.HandleInsertException(ex, "Thi công ", objUi.DONDANGKY .TENKH    );
            }
            return msg;
        }

        public Message Update(THICONG objUi, TTTC tttc, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var objDb = Get(objUi.MADDK  );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CSDAU    = objUi.CSDAU    ;
                    objDb.DALAP = objUi.DALAP  ;
                    objDb.DUONGKINH = objUi.DUONGKINH  ;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.MADH = objUi.MADH;
                     
                    if (!string.IsNullOrEmpty(objUi.MANV))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANV));
                    //objDb.MANV = objUi.MANV;
                    if (!string.IsNullOrEmpty(objUi.MANV2))
                        objDb.MANV2 = objUi.MANV2;
                    

                    if (!string.IsNullOrEmpty(objUi.MAVT))
                        objDb.VATTU = _db.VATTUs.Single(p => p.MAVT.Equals(objUi.MAVT));
                    objDb.ONGNHANH = objUi.ONGNHANH;

                    if (!string.IsNullOrEmpty(objUi.MAPB))
                        objDb.PHONGBAN = _db.PHONGBANs.SingleOrDefault(p => p.MAPB.Equals(objUi.MAPB));
                    objDb.MOTATC = objUi.MOTATC;
                    objDb.MTHUCTE = objUi.MTHUCTE;
                    objDb.NGAYBD = objUi.NGAYBD;
                    objDb.NGAYGTC = objUi.NGAYGTC;
                    objDb.NGAYHT = objUi.NGAYHT;
                    objDb.NGAYTV = objUi.NGAYTV;
                    objDb.TTQT = objUi.TTQT;
                    objDb.SOSERIAL = objUi.SOSERIAL;

                    objDb.CHIKDM1 = objUi.CHIKDM1;
                    objDb.CHIKDM2 = objUi.CHIKDM2;
                 
                      // Submit changes to db
                    _db.SubmitChanges();

                    var ddk = _db.DONDANGKies.SingleOrDefault(d => d.MADDK.Equals(objUi.MADDK));

                    if (ddk != null)
                    {
                        ddk.TTTC = tttc.ToString();
                    }
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTTC.TC_P.ToString(),
                        MOTA = "Cập nhật thi công"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    trans.Commit();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Thi công ");
                }
                else
                {
                    trans.Rollback();

                    // error message
                    msg = new Message(MessageConstants.E_FAILED, MessageType.Error, "Cập nhật thi công");
                }

                if (_db.Connection.State == ConnectionState.Open)
                    _db.Connection.Close();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                if (_db.Connection.State == ConnectionState.Open)
                    _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "Thi công ", objUi.MADDK);
            }
            return msg;
        }

        public Message UpdateQuyetToan(THICONG objUi, string sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    var objDDK = _db.DONDANGKies.FirstOrDefault(p => p.MADDK.Equals(objDb.MADDK));
                    if (objDDK == null)
                    {
                        if (_db.Connection.State == ConnectionState.Open)
                            _db.Connection.Close();

                        // error message
                        msg = new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error,
                            "Cập nhật quyết toán", "Đơn đăng ký không tồn tại.");
                        return msg;
                    }

                    var chiettinh = objDDK.CHIETTINH;
                    if(chiettinh == null)
                    {
                        if (_db.Connection.State == ConnectionState.Open)
                            _db.Connection.Close();

                        // error message
                        msg = new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, 
                            "Cập nhật quyết toán", "Chiết tính không tồn tại.");
                        return msg;
                    }

                    var quyettoan = objDDK.QUYETTOAN;

                    if(quyettoan == null)
                    {
                        quyettoan = new QUYETTOAN
                        {
                            MADDK = chiettinh.MADDK,
                            TENCT = chiettinh.TENCT,
                            TENHM = chiettinh.TENHM,
                            DIACHIHM = chiettinh.DIACHIHM,
                            GHICHU = chiettinh.GHICHU,
                            MANVLCT = sManv,
                            NGAYLCT = DateTime.Now,
                            CONGVIEC = 0, CPVATLIEU = 0, CPNHANCONG = 0, 
                            CPCHUNG = 0, CPTHUNHAP = 0, CPTHIETKE = 0, CPKHAC = 0,
                            HSNHANCONG = chiettinh.HSNHANCONG,
                            HSCHUNG = chiettinh.HSCHUNG,
                            HSCPC = chiettinh.HSCPC,
                            HSTHUNHAP = chiettinh.HSTHUNHAP,
                            HSTHIETKE1 = chiettinh.HSTHIETKE1,
                            HSTHIETKE2 = chiettinh.HSTHIETKE2,
                            HSTHIETKE3 = chiettinh.HSTHIETKE3,
                            HSTHUE = chiettinh.HSTHUE,
                            TIENTHUE = 0, TONG_TT = 0, TONG_ST = 0, FILECT = "",
                            TONGCONG = 0, NHANCONG = 0, CPMAY = 0,
                            CPTTVT = 0, CPTTMAY = 0, TCPTT = 0,

                            CPTTHUE = 0, CPC = 0, HSPVL = 0,
                            HSCPM = 0, HSTTP = 0, CPKSHS = 0,
                            TTP = 0, CHUNGONG = 0, KPDT = "",
                            GIAMGIACPVL = 0, GIAMGIACPNC = 0, SDGIA = 1,
                        };

                        _db.QUYETTOANs.InsertOnSubmit(quyettoan);
                        objDDK.TTHC = TTQT.QT_N.ToString();

                        _db.SubmitChanges();

                        // clear CTQUYETTOAN, CTQUYETTOAN_ND117, DAOLAPQUYETTOAN, DAOLAPQUYETTOAN_ND117
                        var listCTQT = _db.CTQUYETTOANs.Where(ct => ct.MADDK == objDDK.MADDK);
                        _db.CTQUYETTOANs.DeleteAllOnSubmit(listCTQT);

                        var listCTQT117 = _db.CTQUYETTOAN_ND117s.Where(ct => ct.MADDK == objDDK.MADDK);
                        _db.CTQUYETTOAN_ND117s.DeleteAllOnSubmit(listCTQT117);

                        var listDLQT = _db.DAOLAPQUYETTOANs.Where(ct => ct.MADDK == objDDK.MADDK);
                        _db.DAOLAPQUYETTOANs.DeleteAllOnSubmit(listDLQT);

                        var listDLQT117 = _db.DAOLAPQUYETTOAN_ND117s.Where(ct => ct.MADDK == objDDK.MADDK);
                        _db.DAOLAPQUYETTOAN_ND117s.DeleteAllOnSubmit(listDLQT117);

                        _db.SubmitChanges();
                        
                        if (objDb.TTQT.HasValue && objDb.TTQT.Value)
                        {
                            var ctctList = _db.CTCHIETTINHs.Where(ct => ct.MADDK.Equals(objDDK.MADDK)).ToList();
                            foreach (var ctct in ctctList)
                            {
                                var ctqt = new CTQUYETTOAN
                                               {
                                                   MADDK = ctct.MADDK,
                                                   MAVT = ctct.MAVT,
                                                   LOAICT = CT.CT.ToString(),
                                                   LOAICV = "---***---",
                                                   SOLUONG = ctct.SOLUONG,
                                                   GIAVT = ctct.VATTU.GIAVT,
                                                   TIENVT = ctct.SOLUONG*ctct.VATTU.GIAVT,
                                                   GIANC = ctct.VATTU.GIANC,
                                                   TIENNC = ctct.SOLUONG*ctct.VATTU.GIANC,
                                                   ISCTYDTU = ctct.ISCTYDTU
                                               };
                                _db.CTQUYETTOANs.InsertOnSubmit(ctqt);
                            }

                            var ctct117List = _db.CTCHIETTINH_ND117s.Where(ct => ct.MADDK.Equals(objDDK.MADDK)).ToList();
                            foreach (var ctct in ctct117List)
                            {
                                var ctqt = new CTQUYETTOAN_ND117
                                               {
                                                   MADDK = ctct.MADDK,
                                                   MAVT = ctct.MAVT,
                                                   LOAICT = CT.CT.ToString(),
                                                   LOAICV = "---***---",
                                                   SOLUONG = ctct.SOLUONG,
                                                   GIAVT = ctct.VATTU.GIAVT,
                                                   TIENVT = ctct.SOLUONG*ctct.VATTU.GIAVT,
                                                   GIANC = ctct.VATTU.GIANC,
                                                   TIENNC = ctct.SOLUONG*ctct.VATTU.GIANC
                                               };
                                _db.CTQUYETTOAN_ND117s.InsertOnSubmit(ctqt);
                            }

                            var dlList = _db.DAOLAP_ND117s.Where(ct => ct.MADDK.Equals(objDDK.MADDK)).ToList();
                            foreach (var dlct in dlList)
                            {
                                var dlqt = new DAOLAPQUYETTOAN_ND117
                                               {
                                                   MADDK = dlct.MADDK,
                                                   LOAICV = dlct.LOAICV,
                                                   LOAICT = dlct.LOAICT,
                                                   NOIDUNG = dlct.NOIDUNG,
                                                   DONGIACP = dlct.DONGIACP,
                                                   DVT = dlct.DVT,
                                                   HESOCP = dlct.HESOCP,
                                                   SOLUONG = dlct.SOLUONG,
                                                   THANHTIENCP = dlct.THANHTIENCP,
                                                   LOAICP = dlct.LOAICP,
                                                   LOAI = dlct.LOAI,
                                                   NGAYLAP = dlct.NGAYLAP
                                               };
                                _db.DAOLAPQUYETTOAN_ND117s.InsertOnSubmit(dlqt);
                            }

                            _db.SubmitChanges();
                        }
                    }

                    trans.Commit();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "quyết toán");
                }
                else
                {
                    trans.Rollback();

                    // error message
                    msg = new Message(MessageConstants.E_FAILED, MessageType.Error, "Cập nhật quyết toán");
                }

                if (_db.Connection.State == ConnectionState.Open)
                    _db.Connection.Close();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                if (_db.Connection.State == ConnectionState.Open)
                    _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "Quyết toán", objUi.MADDK);
            }
            return msg;
        }

        public Message Update(THICONG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CSDAU = objUi.CSDAU;
                    objDb.DALAP = objUi.DALAP;
                    objDb.DUONGKINH = objUi.DUONGKINH;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.MADH = objUi.MADH;

                    if (!string.IsNullOrEmpty(objUi.MANV))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANV));
                    //objDb.MANV = objUi.MANV;
                    if (!string.IsNullOrEmpty(objUi.MAVT))
                        objDb.VATTU = _db.VATTUs.Single(p => p.MAVT.Equals(objUi.MAVT));
                    objDb.ONGNHANH = objUi.ONGNHANH;

                    if (!string.IsNullOrEmpty(objUi.MAPB))
                        objDb.PHONGBAN = _db.PHONGBANs.SingleOrDefault(p => p.MAPB.Equals(objUi.MAPB));
                    objDb.MOTATC = objUi.MOTATC;
                    objDb.MTHUCTE = objUi.MTHUCTE;
                    objDb.NGAYBD = objUi.NGAYBD;
                    objDb.NGAYGTC = objUi.NGAYGTC;
                    objDb.NGAYHT = objUi.NGAYHT;
                    objDb.NGAYTV = objUi.NGAYTV;
                    objDb.TTQT = objUi.TTQT;
                    objDb.SOSERIAL = objUi.SOSERIAL;

                    // Submit changes to db
                    _db.SubmitChanges();

                    trans.Commit();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTTC.TC_P.ToString(),
                        MOTA = "Cập nhật thi công"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Thi công ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_FAILED, MessageType.Error, "Cập nhật thi công");
                }
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Thi công ", objUi.MADDK);
            }
            return msg;
        }
        
        public bool IsInUse(string ma)
        {

            return false;

        }

        public Message Delete(THICONG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thi công ", objUi.DONDANGKY.TENKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.THICONGs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTTC.TC_P.ToString(),
                    MOTA = "Xóa thi công"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thi công ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thi công ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<THICONG> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thi công ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách thi công ");
            }

            return msg;
        }

        
    }
}
