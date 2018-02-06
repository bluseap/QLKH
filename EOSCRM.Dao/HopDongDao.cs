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
    public class HopDongDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        //private readonly DonDangKyDao _ddkDao = new DonDangKyDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public HopDongDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public HOPDONG Get(string ma)
        {
            return _db.HOPDONGs.Where(p => p.MADDK.Equals(ma)).SingleOrDefault();
        }

        public List<HOPDONG> GetList()
        {
            return _db.HOPDONGs.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONG> GetList(string keyword, bool? dacapdb)
        {
            if (keyword != "")
            {
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.SOHD).ToList()
                        : _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.SOHD).ToList();

                return _db.HOPDONGs.Where(hd => hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                    //.OrderByDescending(hd => hd.NGAYTAO).ToList();
                                                    .OrderByDescending(hd => hd.SOHD).ToList();
            }

            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value))
                                 .OrderByDescending(hd => hd.SOHD).ToList()
                           : _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null)
                                 .OrderByDescending(hd => hd.SOHD).ToList();
            }

            return _db.HOPDONGs.OrderByDescending(hd => hd.SOHD).ToList();
        }

        public List<HOPDONG> GetListKV(string keyword, bool? dacapdb, string maKV)
        {
            if (keyword != "")
            {
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) && hd.MAKV.Equals(maKV) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.SOHD).ToList()
                        : _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) && hd.MAKV.Equals(maKV) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.SOHD).ToList();

                return _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) && hd.MAKV.Equals(maKV) )
                    //.OrderByDescending(hd => hd.NGAYTAO).ToList();
                                                    .OrderByDescending(hd => hd.SOHD).ToList();
            }

            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) && hd.MAKV.Equals(maKV))
                                 .OrderByDescending(hd => hd.SOHD).ToList()
                           : _db.HOPDONGs.Where(hd => (hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null) && hd.MAKV.Equals(maKV))
                                 .OrderByDescending(hd => hd.SOHD).ToList();
            }

            return _db.HOPDONGs.OrderByDescending(hd => Convert.ToInt32(hd.SOHD))
                .ToList();
        }

        public List<HOPDONG> GetListKVLX(string keyword, bool? dacapdb, string maKV)
        {
            if (keyword != "")
            {
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) && hd.MAKV.Equals(maKV) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.SOHD).ToList()
                        : _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) && hd.MAKV.Equals(maKV) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.SOHD).ToList();

                return _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) && hd.MAKV.Equals(maKV))
                    //.OrderByDescending(hd => hd.NGAYTAO).ToList();
                                                    .OrderByDescending(hd => Convert.ToInt32(hd.SOHD.Length == 11 ? hd.SOHD.Substring(0, 3)
                                                        : hd.SOHD.Length == 10 ? hd.SOHD.Substring(0, 2)
                                                            : hd.SOHD.Substring(0, 1)))
                                                    .OrderByDescending(hd => hd.NGAYTAO)
                                            .ToList();
            }

            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) && hd.MAKV.Equals(maKV))
                                    .OrderByDescending(hd => Convert.ToInt32(hd.SOHD.Length == 11 ? hd.SOHD.Substring(0, 3)
                                                        : hd.SOHD.Length == 10 ? hd.SOHD.Substring(0, 2)
                                                            : hd.SOHD.Substring(0, 1)))
                                    .OrderByDescending(hd => hd.NGAYTAO)
                                    
                                .ToList()
                           : _db.HOPDONGs.Where(hd => (hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null) && hd.MAKV.Equals(maKV))
                                    .OrderByDescending(hd => Convert.ToInt32(hd.SOHD.Length == 11 ? hd.SOHD.Substring(0, 3)
                                                        : hd.SOHD.Length == 10 ? hd.SOHD.Substring(0, 2)
                                                            : hd.SOHD.Substring(0, 1)))      
                                    .OrderByDescending(hd => hd.NGAYTAO)
                                 
                                .ToList();
            }

            return _db.HOPDONGs
                    .OrderByDescending(hd => Convert.ToInt32(hd.SOHD.Length == 11 ? hd.SOHD.Substring(0, 3)
                                                        : hd.SOHD.Length == 10 ? hd.SOHD.Substring(0, 2)
                                                            : hd.SOHD.Substring(0, 1)))
                    .OrderByDescending(hd => hd.NGAYTAO)
                .ToList();
        }

        public List<HOPDONG> GetListNN(string keyword, bool? dacapdb)
        {  
            var hopdong = from hd in _db.HOPDONGs
                          join bb in _db.BBNGHIEMTHUs on hd.MADDK equals bb.MADDK
                          where ((hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                && hd.DACAPDB.Equals(dacapdb.Value)
                                                && hd.MADDK.Equals(bb.MADDK))
                          select hd;

            if (keyword != "")
            {
                /*
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.NGAYTAO).ToList()
                        : _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.NGAYTAO).ToList();
                */

                return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
            }

            /*
            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value))
                                 .OrderByDescending(hd => hd.NGAYTAO).ToList()
                           : _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null)
                                 .OrderByDescending(hd => hd.NGAYTAO).ToList();
            }*/

            return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONG> GetListNNKV(string keyword, bool? dacapdb, string makv)
        {
            //var hopdong = from hd in _db.HOPDONGs
            //              join bb in _db.BBNGHIEMTHUs on hd.MADDK equals bb.MADDK
            //              where ((hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))
            //                    )
            //                                    && hd.DACAPDB.Equals(dacapdb.Value)
            //                                    && hd.MADDK.Equals(bb.MADDK)
            //                                    && hd.MAKV.Equals(makv))
            //              select hd;
            var hopdong = from hd in _db.HOPDONGs
                          join bb in _db.BBNGHIEMTHUs on hd.MADDK equals bb.MADDK
                          join tc in _db.THICONGs on hd.MADDK equals tc.MADDK
                          join dh in _db.DONGHOs on tc.MADH equals dh.MADH
                          where ((hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))
                                || dh.SONO.Contains(keyword) )
                                                && hd.DACAPDB.Equals(dacapdb.Value)
                                                && hd.MADDK.Equals(bb.MADDK)
                                                && hd.MAKV.Equals(makv))
                          select hd;

            if (keyword != "")
            {
                /*
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.NGAYTAO).ToList()
                        : _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.NGAYTAO).ToList();
                */
                return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
            }

            /*
            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value))
                                 .OrderByDescending(hd => hd.NGAYTAO).ToList()
                           : _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null)
                                 .OrderByDescending(hd => hd.NGAYTAO).ToList();
            }*/

            return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONG> GetListNNKVTSON(string keyword, bool? dacapdb, string makv)
        {
            var hopdong = from hd in _db.HOPDONGs
                          join bb in _db.BBNGHIEMTHUs on hd.MADDK equals bb.MADDK
                          where ((hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                && hd.DACAPDB.Equals(dacapdb.Value)
                                                && hd.MADDK.Equals(bb.MADDK)
                                                && hd.MAKV.Equals(makv))
                          select hd;                    

            return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONG> GetListKVNghiemThuTS(string keyword, bool? dacapdb, string makv)
        {
            var hopdong = from hd in _db.HOPDONGs
                          join bb in _db.BBNGHIEMTHUs on hd.MADDK equals bb.MADDK
                          where ((hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                && hd.DACAPDB.Equals(dacapdb.Value)
                                                && hd.MADDK.Equals(bb.MADDK)
                                                && hd.MAKV.Equals(makv))
                          select hd;

            if (keyword != "")
            {              

                return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
            }          

            return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONG> GetList(String madon, String sohd, String tenKh, String sonha, String maKV, DateTime? fromDate, DateTime? toDate)
        {
            var dshd = from hd in _db.HOPDONGs
                        where hd.DACAPDB.Equals(false) &&  hd.DONDANGKY.MAKV.Equals(maKV)
                        select hd;

            if (madon != null)
                dshd = dshd.Where(d => d.MADDK.Contains(madon));

            if (sohd != null)
                dshd = dshd.Where(d => d.SOHD.Contains(sohd));

            if (tenKh != null)
                dshd = dshd.Where(d => d.DONDANGKY.TENKH.Contains(tenKh));

            if (sonha != null)
                dshd = dshd.Where(d => d.SONHA.Contains(sonha));

            if (maKV != null)
                dshd = dshd.Where(d => d.MAKV.Contains(maKV));

            if (fromDate.HasValue)
                dshd = dshd.Where(d => d.NGAYTAO.HasValue
                                           && d.NGAYTAO.Value >= fromDate.Value);

            if (toDate.HasValue)
                dshd = dshd.Where(d => d.NGAYTAO.HasValue
                                           && d.NGAYTAO.Value <= toDate.Value);

            return dshd.OrderByDescending(d => d.NGAYTAO).ToList();
        }

        public int Count()
        {
            return _db.HOPDONGs.Count();
        }

        public Message Insert(HOPDONG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                _db.HOPDONGs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                var ddk = _db.DONDANGKies.SingleOrDefault(d => d.MADDK.Equals(objUi.MADDK));

                if (ddk != null)
                {
                    ddk.TTHD = "HD_A";// TTHD.HD_A.ToString();
                    ddk.TTTC = "TC_N";// TTTC.TC_N.ToString();

                    //_ddkDao.UpdateTT(ddk);
                }
                else
                {
                    trans.Rollback();
                    return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập hợp đồng", "Mã đơn đăng ký không tồn tại.");
                }

                _db.SubmitChanges();

                // commit
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
                    MATT = TTHD.HD_N.ToString( ),
                    MOTA = "Nhập hợp đồng"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "hợp đồng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();
                _db.Connection.Close();
                msg = ExceptionHandler.HandleInsertException(ex, "hợp đồng", objUi.MADDK);
            }
            return msg;
        }

        public Message Update(HOPDONG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADP = objUi.MADP;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADB = objUi.MADB;
                    objDb.LOTRINH = objUi.LOTRINH;
                    objDb.NGAYTAO = objUi.NGAYTAO;
                    objDb.NGAYKT = objUi.NGAYKT;
                    objDb.NGAYHL = objUi.NGAYHL;
                    objDb.SONHA = objUi.SONHA;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.MAKV = objUi.MAKV;
                    objDb.CODH = objUi.CODH;
                    objDb.LOAIONG = objUi.LOAIONG;
                    objDb.MAHTTT = objUi.MAHTTT;
                    objDb.MAMDSD = objUi.MAMDSD;
                    objDb.DINHMUCSD = objUi.DINHMUCSD;
                    objDb.SOHO = objUi.SOHO;
                    objDb.SONHANKHAU = objUi.SONHANKHAU;
                    //objDb.DACAPDB = objUi.DACAPDB;
                    //objDb.DACAPDB = false;
                    objDb.LOAIHD = objUi.LOAIHD;
                    objDb.SOHD = objUi.SOHD;
                    objDb.TRANGTHAI = objUi.TRANGTHAI;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

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
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTHD.HD_P.ToString(),
                        MOTA = "Sửa nhập hợp đồng tra cứu."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "hợp đồng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hợp đồng", objUi.MADDK);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hợp đồng", objUi.MADDK);
            }
            return msg;
        }

        public Message UpdateM(HOPDONG objUi, String useragent, String ipAddress, String sManv, bool dacap)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADP = objUi.MADP;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADB = objUi.MADB;
                    objDb.LOTRINH = objUi.LOTRINH;
                    objDb.NGAYTAO = objUi.NGAYTAO;
                    objDb.NGAYKT = objUi.NGAYKT;
                    objDb.NGAYHL = objUi.NGAYHL;
                    objDb.SONHA = objUi.SONHA;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.MAKV = objUi.MAKV;
                    objDb.CODH = objUi.CODH;
                    objDb.LOAIONG = objUi.LOAIONG;
                    objDb.MAHTTT = objUi.MAHTTT;
                    objDb.MAMDSD = objUi.MAMDSD;
                    objDb.DINHMUCSD = objUi.DINHMUCSD;
                    objDb.SOHO = objUi.SOHO;
                    objDb.SONHANKHAU = objUi.SONHANKHAU;
                    objDb.DACAPDB = dacap;
                    //objDb.DACAPDB = false;
                    objDb.LOAIHD = objUi.LOAIHD;
                    objDb.SOHD = objUi.SOHD;
                    objDb.TRANGTHAI = objUi.TRANGTHAI;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

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
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTHD.HD_P.ToString(),
                        MOTA = "Sửa nhập hợp đồng."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "hợp đồng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hợp đồng", objUi.MADDK);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hợp đồng", objUi.MADDK);
            }
            return msg;
        }

        public Message UpdateDaSDTS(KHACHHANG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    
                    objDb.DACAPDB = true;                    

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
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTHD.HD_P.ToString(),
                        MOTA = "Sửa nhập hợp đồng. Đã cấp"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "hợp đồng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hợp đồng", objUi.MADDK);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hợp đồng", objUi.MADDK);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return false;
        }

        public Message Delete(HOPDONG objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Hợp đồng ", objUi.MADDK);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.HOPDONGs.DeleteOnSubmit(objDb);
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
                    MATT = TTHD.HD_P.ToString(),
                    MOTA = "Xóa hợp đồng"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hợp đồng ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Hợp đồng ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<HOPDONG> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                    Delete(obj, useragent,  ipAddress,  sManv);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hợp đồng ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách hợp đồng ");
            }

            return msg;
        }

        public string NewId()
        {
            var sToday = DateTime.Now.ToString("yyyyMMdd");

            var query = (from p in _db.HOPDONGs.Where(p => p.SOHD.Substring(0, 8).Contains(sToday)) select p.SOHD).Max();
            //var query = (from p in _db.HOPDONGs select p.SOHD).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query.Substring(8, 3)) + 1;
                query = sToday + temp.ToString("D3");
            }
            else
            {
                query = sToday + "001";
            }

            return query;
        }

        public string NewIdMAKV(string makv)
        {
            var sToday = DateTime.Now.ToString("yyyyMMdd");

            var query = (from p in _db.HOPDONGs.Where(p => p.SOHD.Substring(0, 8).Contains(sToday))
                         where p.MAKV.Equals(makv)
                         select p.SOHD).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query.Substring(8, 3)) + 1;
                query = sToday + temp.ToString("D3");
            }
            else
            {
                query = sToday + "001";
            }

            return query;
        }

        public string NewIdLX(string makv)
        {
            var sNAM = DateTime.Now.ToString("yyyy");
            var sTHANG = DateTime.Now.ToString("MM");            

            var query = (from p in _db.HOPDONGs.Where(p => p.MAKV.Equals(makv) &&
                            p.SOHD.Substring(4, 2).Contains(sTHANG) && p.SOHD.Substring(7, 4).Contains(sNAM))
                         select p.SOHD.Substring(0,3)).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                query = temp.ToString("D3") + "/" + sTHANG + "/" + sNAM;
            }
            else
            {
                query = "001" + "/" + sTHANG + "/" + sNAM;
            }

            return query;
        }

    }
}
