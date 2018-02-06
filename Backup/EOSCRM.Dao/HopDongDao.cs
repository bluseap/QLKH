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
                                .OrderByDescending(hd => hd.NGAYTAO).ToList()
                        : _db.HOPDONGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.NGAYTAO).ToList();

                return _db.HOPDONGs.Where(hd => hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                    .OrderByDescending(hd => hd.NGAYTAO).ToList();
            }

            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value))
                                 .OrderByDescending(hd => hd.NGAYTAO).ToList()
                           : _db.HOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null)
                                 .OrderByDescending(hd => hd.NGAYTAO).ToList();
            }

            return _db.HOPDONGs.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }

        public List<HOPDONG> GetList(String madon, String sohd, String tenKh, String sonha, String maKV, DateTime? fromDate, DateTime? toDate)
        {
            var dshd = from hd in _db.HOPDONGs
                        where hd.DACAPDB.Equals(false)
                        select hd;

            if (madon != null)
                dshd = dshd.Where(d => d.MADDK.Contains(madon));

            if (sohd != null)
                dshd = dshd.Where(d => d.SOHD.Contains(sohd));

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
                    ddk.TTHD = TTHD.HD_A.ToString();
                    ddk.TTTC = TTTC.TC_N.ToString();
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

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "hợp đồng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

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
                    objDb.DACAPDB = objUi.DACAPDB;
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
                        MOTA = "Cập nhật hợp đồng"
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

            var query = (from p in _db.HOPDONGs.Where(p => p.SOHD.Substring(0, 8).Contains(sToday))
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
    }
}
