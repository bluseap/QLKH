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
    public class HopDongPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        //private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public HopDongPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public HOPDONGPO Get(string ma)
        {
            return _db.HOPDONGPOs.Where(p => p.MADDKPO.Equals(ma)).SingleOrDefault();
        }

        public List<HOPDONGPO> GetListPo(string ma)
        {
            return _db.HOPDONGPOs.Where(p => p.MADDKPO.Equals(ma)).ToList();
        }

        public List<HOPDONGPO> GetList()
        {
            return _db.HOPDONGPOs.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONGPO> GetList(string keyword, bool? dacapdb, string makv)
        {
            if (keyword != "")
            {
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.HOPDONGPOs.Where(hd => hd.MAKVPO.Equals(makv) && (hd.MADDKPO.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKYPO != null && hd.DONDANGKYPO.TENKH.Contains(keyword))) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.SOHD).ToList()
                        : _db.HOPDONGPOs.Where(hd => hd.MAKVPO.Equals(makv) && (hd.MADDKPO.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKYPO != null && hd.DONDANGKYPO.TENKH.Contains(keyword))) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.SOHD).ToList();

                return _db.HOPDONGPOs.Where(hd => hd.MAKVPO.Equals(makv) && hd.MADDKPO.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                                (hd.DONDANGKYPO != null && hd.DONDANGKYPO.TENKH.Contains(keyword)))
                                                    //.OrderByDescending(hd => hd.NGAYTAO).ToList();
                                                    .OrderByDescending(hd => hd.SOHD).ToList();
            }

            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGPOs.Where(hd => hd.MAKVPO.Equals(makv) && hd.DACAPDB.Equals(dacapdb.Value))
                                 .OrderByDescending(hd => hd.SOHD).ToList()
                           : _db.HOPDONGPOs.Where(hd => hd.MAKVPO.Equals(makv) && hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null)
                                 .OrderByDescending(hd => hd.SOHD).ToList();
            }

            return _db.HOPDONGPOs.OrderByDescending(hd => hd.SOHD).ToList();
        }

        public List<HOPDONGPO> GetListKeyword(string keyword, string makv)
        {
            var query = _db.HOPDONGPOs.Where(p=>p.MAKVPO.Equals(makv));

            if (!String.IsNullOrEmpty(keyword))
            {
                query = query.Where(hd => hd.MADDKPO.Contains(keyword) || hd.SOHD.Contains(keyword) || hd.DONDANGKYPO.TENKH.Contains(keyword))
                                .OrderByDescending(hd => hd.SOHD);
            }
            
            return query.OrderByDescending(hd => hd.SOHD).ToList();
        }

        public List<HOPDONGPO> GetListNN(string keyword, bool? dacapdb)
        {  
            var hopdong = from hd in _db.HOPDONGPOs
                          join bb in _db.BBNGHIEMTHUPOs on hd.MADDKPO equals bb.MADDKPO
                          where ((hd.MADDKPO.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKYPO != null && hd.DONDANGKYPO.TENKH.Contains(keyword)))
                                                && hd.DACAPDB.Equals(dacapdb.Value) && hd.MADDKPO.Equals(bb.MADDKPO)
                                                )
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

        public List<HOPDONGPO> GetListKhuVuc(string keyword, bool? dacapdb, string makv)
        {
            var hopdong = from hd in _db.HOPDONGPOs
                          join bb in _db.BBNGHIEMTHUPOs on hd.MADDKPO equals bb.MADDKPO
                          where ((hd.MADDKPO.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKYPO != null && hd.DONDANGKYPO.TENKH.Contains(keyword)))
                                                && hd.DACAPDB.Equals(dacapdb.Value) && hd.MADDKPO.Equals(bb.MADDKPO)
                                                && hd.MAKVPO.Equals(makv))
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

        public List<HOPDONGPO> GetList(String madon, String sohd, String tenKh, String sonha, String maKV, DateTime? fromDate, DateTime? toDate)
        {
            var dshd = from hd in _db.HOPDONGPOs
                        where hd.DACAPDB.Equals(false)
                        select hd;

            if (madon != null)
                dshd = dshd.Where(d => d.MADDKPO.Contains(madon));

            if (sohd != null)
                dshd = dshd.Where(d => d.SOHD.Contains(sohd));

            if (tenKh != null)
                dshd = dshd.Where(d => d.DONDANGKYPO.TENKH.Contains(tenKh));

            if (sonha != null)
                dshd = dshd.Where(d => d.SONHA.Contains(sonha));

            if (maKV != null)
                dshd = dshd.Where(d => d.MAKVPO.Contains(maKV));

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
            return _db.HOPDONGPOs.Count();
        }

        public Message Insert(HOPDONGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                _db.HOPDONGPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                var ddk = _db.DONDANGKYPOs.SingleOrDefault(d => d.MADDKPO.Equals(objUi.MADDKPO));

                if (ddk != null)
                {
                    ddk.TTHD = "HD_A";
                    ddk.TTTC = TTTC.TC_N.ToString();

                    //_ddkpoDao.UpdateTT(ddk);
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
                    MADON = objUi.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTHD.HD_N.ToString( ),
                    MOTA = "Nhập hợp đồng điện"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "hợp đồng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "hợp đồng", objUi.MADDKPO);
            }
            return msg;
        }

        public Message Update(HOPDONGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                    objDb.MADB = objUi.MADB;
                    objDb.LOTRINH = objUi.LOTRINH;
                    objDb.NGAYTAO = objUi.NGAYTAO;
                    objDb.NGAYKT = objUi.NGAYKT;
                    objDb.NGAYHL = objUi.NGAYHL;
                    objDb.SONHA = objUi.SONHA;
                    objDb.MAPHUONGPO = objUi.MAPHUONGPO;
                    objDb.MAKVPO = objUi.MAKVPO;
                    objDb.CODH = objUi.CODH;
                    objDb.LOAIONG = objUi.LOAIONG;
                    objDb.MAHTTT = objUi.MAHTTT;
                    objDb.MAMDSDPO = objUi.MAMDSDPO;
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
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTHD.HD_P.ToString(),
                        MOTA = "Sửa nhập hợp đồng điện."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "hợp đồng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hợp đồng", objUi.MADDKPO);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hợp đồng", objUi.MADDKPO);
            }
            return msg;
        }

        public Message UpdateM(HOPDONGPO objUi, String useragent, String ipAddress, String sManv, bool dacap)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                    objDb.MADB = objUi.MADB;
                    objDb.LOTRINH = objUi.LOTRINH;
                    objDb.NGAYTAO = objUi.NGAYTAO;
                    objDb.NGAYKT = objUi.NGAYKT;
                    objDb.NGAYHL = objUi.NGAYHL;
                    objDb.SONHA = objUi.SONHA;
                    objDb.MAPHUONGPO = objUi.MAPHUONGPO;
                    objDb.MAKVPO = objUi.MAKVPO;
                    objDb.CODH = objUi.CODH;
                    objDb.LOAIONG = objUi.LOAIONG;
                    objDb.MAHTTT = objUi.MAHTTT;
                    objDb.MAMDSDPO = objUi.MAMDSDPO;
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
                        MADON = objUi.MADDKPO,
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
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hợp đồng", objUi.MADDKPO);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hợp đồng", objUi.MADDKPO);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return false;
        }

        public Message Delete(HOPDONGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDKPO);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Hợp đồng ", objUi.MADDKPO);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.HOPDONGPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDKPO,
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
        public Message DeleteList(List<HOPDONGPO> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
            var sToday = DateTime.Now.ToString("yyMMdd");

            //var query = (from p in _db.HOPDONGPOs.Where(p => p.SOHD.Substring(0, 8).Contains(sToday))
            //             select p.SOHD).Max();
            var query = (from p in _db.HOPDONGPOs
                         select p.SOHD).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query.Substring(6, 5)) + 1;
                query = sToday + temp.ToString("D5");
            }
            else
            {
                query = sToday + "00001";
            }

            return query;
        }

        public Message UpdateDaSDTS(KHACHHANGPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO);

                if (objDb != null)
                {

                    objDb.DACAPDB = true;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTHD.HD_P.ToString(),
                        MOTA = "Sửa nhập hợp đồng điện. Đã cấp"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "hợp đồng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "hợp đồng", objUi.MADDKPO);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "hợp đồng", objUi.MADDKPO);
            }
            return msg;
        }
    }
}
