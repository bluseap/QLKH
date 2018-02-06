using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Data.Common;

using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ThayHopDongDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();


        public ThayHopDongDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public THAYHOPDONG Get(string ma)
        {
            return _db.THAYHOPDONGs.Where(p => p.IDTHD.Equals(ma)).SingleOrDefault();
        }

        public THAYHOPDONG GetThayHopDongKy(string idkh, int thang, int nam)
        {
            var lockKN = _db.THAYHOPDONGs.Where(lck => lck.THANG.Equals(thang) && lck.NAM.Equals(nam)  && lck.IDKH.Equals(idkh)).SingleOrDefault();
            
            return lockKN;
        }

        public List<THAYHOPDONG> GetList()
        {
            return _db.THAYHOPDONGs.OrderByDescending(hd => hd.NGAYNHAP).ToList();
        }

        public List<THAYHOPDONG> GetListKV(string makv)
        {
            var query = _db.THAYHOPDONGs.Where(hd => hd.KHACHHANG.MAKV.Equals(makv))
                .OrderByDescending(hd => hd.NGAYNHAP);
            return query.ToList();
        }

        /*public List<THAYHOPDONG> GetList(string keyword, bool? dacapdb)
        {
            
            if (keyword != "")
            {
                if (dacapdb.HasValue)
                    return dacapdb.Value
                        ? _db.THAYHOPDONGGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                 (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                hd.DACAPDB.Equals(dacapdb.Value))
                                .OrderByDescending(hd => hd.SOHD).ToList()
                        : _db.THAYHOPDONGGs.Where(hd => (hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword))) &&
                                !(hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null))
                                .OrderByDescending(hd => hd.SOHD).ToList();

                return _db.THAYHOPDONGs.Where(hd => hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) ||
                                                (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                    //.OrderByDescending(hd => hd.NGAYTAO).ToList();
                                                    .OrderByDescending(hd => hd.SOHD).ToList();
            }

            if (dacapdb.HasValue)
            {
                return dacapdb.Value
                           ? _db.THAYHOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value))
                                 .OrderByDescending(hd => hd.SOHD).ToList()
                           : _db.THAYHOPDONGs.Where(hd => hd.DACAPDB.Equals(dacapdb.Value) || hd.DACAPDB == null)
                                 .OrderByDescending(hd => hd.SOHD).ToList();
            }

            return _db.THAYHOPDONGs.OrderByDescending(hd => hd.SOHD).ToList();
            
        }*/

        /*public List<THAYHOPDONG> GetListNN(string keyword, bool? dacapdb)
        {  
            var hopdong = from hd in _db.HOPDONGs
                          join bb in _db.BBNGHIEMTHUs on hd.MADDK equals bb.MADDK
                          where ((hd.MADDK.Contains(keyword) || hd.SOHD.Contains(keyword) || (hd.DONDANGKY != null && hd.DONDANGKY.TENKH.Contains(keyword)))
                                                && hd.DACAPDB.Equals(dacapdb.Value)
                                                && hd.MADDK.Equals(bb.MADDK))
                          select hd;

            if (keyword != "")
            {
               

                return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
            }

            

            return hopdong.OrderByDescending(hd => hd.NGAYTAO).ToList();
        }*/

        /*public List<HOPDONG> GetList(String madon, String sohd, String tenKh, String sonha, String maKV, DateTime? fromDate, DateTime? toDate)
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
        }*/

        public int Count()
        {
            return _db.THAYHOPDONGs.Count();
        }

        public void Insert(THAYHOPDONG objUi, String useragent, String ipAddress, String sManv)
        {
            try
            {
                _db.Connection.Open();
                _db.THAYHOPDONGs.InsertOnSubmit(objUi);
               
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDTHD,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "THAYHD",
                    MOTA = "Thay đổi hợp đồng nước."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                _db.SubmitChanges();
            }
            catch { }           
        }

        public void InsertGiaHanHD(THAYHOPDONG objUi, String useragent, String ipAddress, String sManv)
        {
            try
            {
                _db.Connection.Open();
                _db.THAYHOPDONGs.InsertOnSubmit(objUi);
               
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDTHD,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "GIAHANHD",
                    MOTA = "Gia hạn hợp đồng nước."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                _db.SubmitChanges();
            }
            catch { }
        }

        public Message Update(THAYHOPDONG objUi, String useragent, String ipAddress, String sManv, string idthd, string idkh)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(idthd);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENKHMOI = objUi.TENKHMOI;
                    objDb.UYQUYEN = objUi.UYQUYEN;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.NGAYSINH = objUi.NGAYSINH;
                    objDb.CMND = objUi.CMND;
                    objDb.CAPNGAY = objUi.CAPNGAY;
                    objDb.TAI = objUi.TAI;
                    objDb.SONHA = objUi.SONHA;
                    objDb.MST = objUi.MST;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.NGAYKT = objUi.NGAYKT;
                    objDb.NGAYHL = objUi.NGAYHL;
                    objDb.LYDO = objUi.LYDO;
                    objDb.SOHOKHAU = objUi.SOHOKHAU;                    

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = idthd,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "CAPHD",
                        MOTA = "Sửa thông tin thay hợp đồng nước."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                    
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thay hợp đồng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "thay hợp đồng");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "thay hợp đồng");
            }
            return msg;
        }

    public Message UpdateGiaHanHD(THAYHOPDONG objUi, String useragent, String ipAddress, String sManv, string idthd, string idkh)
    {
        Message msg;
        try
        {
            // get current object in database
            var objDb = Get(idthd);

            if (objDb != null)
            {
                //TODO: update all fields
                objDb.TENKHMOI = objUi.TENKHMOI;
                objDb.UYQUYEN = objUi.UYQUYEN;
                objDb.DIACHILD = objUi.DIACHILD;
                objDb.NGAYSINH = objUi.NGAYSINH;
                objDb.CMND = objUi.CMND;
                objDb.CAPNGAY = objUi.CAPNGAY;
                objDb.TAI = objUi.TAI;
                objDb.SONHA = objUi.SONHA;
                objDb.MST = objUi.MST;
                objDb.DIENTHOAI = objUi.DIENTHOAI;
                objDb.NGAYKT = objUi.NGAYKT;
                objDb.NGAYHL = objUi.NGAYHL;
                objDb.LYDO = objUi.LYDO;
                objDb.SOHOKHAU = objUi.SOHOKHAU;

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = idthd,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "UPGIAHANHD",
                    MOTA = "Sửa thông tin gia hạn hợp đồng nước."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "thay hợp đồng");
            }
            else
            {
                // error message
                msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "thay hợp đồng");
            }
        }
        catch (Exception ex)
        {
            msg = ExceptionHandler.HandleUpdateException(ex, "thay hợp đồng");
        }
        return msg;
    }

        /*public Message UpdateM(HOPDONG objUi, String useragent, String ipAddress, String sManv, bool dacap)
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
        }*/

        public bool IsInUse(string ma)
        {
            return false;
        }

        /*public Message Delete(HOPDONG objUi, String useragent, String ipAddress, String sManv)
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
        }*/

        public string NewId()
        {
            var sToday = DateTime.Now.ToString("yyMM");

            //var query = (from p in _db.HOPDONGs.Where(p => p.SOHD.Substring(0, 8).Contains(sToday))  select p.SOHD).Max();
            var query = _db.THAYHOPDONGs.Max(p => p.IDTHD.Substring(4,6));

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                query = sToday + temp.ToString("D6");
            }
            else
            {
                query = sToday + "000001";
            }

            return query;

            /*var query = _db.THAYHOPDONGs.Max(p => p.IDTHD);
            var temp = int.Parse(query) + 1;
            return temp.ToString("D6");*/
        }

        

    }
}
