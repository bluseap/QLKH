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
    public class ThayHopDongPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public ThayHopDongPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public THAYHOPDONGPO Get(string ma)
        {
            return _db.THAYHOPDONGPOs.Where(p => p.IDTHDPO.Equals(ma)).SingleOrDefault();
        }

        public List<THAYHOPDONGPO> GetList()
        {
            return _db.THAYHOPDONGPOs.OrderByDescending(hd => hd.NGAYNHAP).ToList();
        }

        public List<THAYHOPDONGPO> GetListKV(string makv)
        {
            var query = _db.THAYHOPDONGPOs.Where(hd => hd.KHACHHANGPO.MAKVPO.Equals(makv))
                .OrderByDescending(hd => hd.NGAYNHAP);
            return query.ToList();
        }        

        public int Count()
        {
            return _db.THAYHOPDONGPOs.Count();
        }

        public void Insert(THAYHOPDONGPO objUi, String useragent, String ipAddress, String sManv)
        {
            try
            {
                _db.Connection.Open();
                _db.THAYHOPDONGPOs.InsertOnSubmit(objUi);
               
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDTHDPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "THAYHDPO",
                    MOTA = "Thay đổi hợp đồng điện."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();
            }
            catch { }           
        }

        public void InsertToKHTen(THAYHOPDONGPO objUi, String useragent, String ipAddress, String sManv, string maddk, string tenkhmoi, int thang, int nam,
            string lydothay)
        {
            try
            {
                _db.Connection.Open();
                _db.THAYHOPDONGPOs.InsertOnSubmit(objUi);

                _rpClass.UPKHTENTHDPO(maddk, tenkhmoi, thang, nam, 1, lydothay);

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDTHDPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "THAYHDPO",
                    MOTA = "Thay đổi hợp đồng điện."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();
            }
            catch { }
        }

        public void InsertGiaHanHD(THAYHOPDONGPO objUi, String useragent, String ipAddress, String sManv)
        {
            try
            {
                _db.Connection.Open();
                _db.THAYHOPDONGPOs.InsertOnSubmit(objUi);
               
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDTHDPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "GIAHANHDPO",
                    MOTA = "Gia hạn hợp đồng điện."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();
            }
            catch { }
        }

        public Message Update(THAYHOPDONGPO objUi, String useragent, String ipAddress, String sManv, string idthd, string idkh)
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

                    objDb.THANG = objUi.THANG;
                    objDb.NAM = objUi.NAM;   

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = idthd,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "CAPHDPO",
                        MOTA = "Sửa thông tin thay hợp đồng điện."
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

    public Message UpdateGiaHanHD(THAYHOPDONGPO objUi, String useragent, String ipAddress, String sManv, string idthd, string idkh)
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
                    MATT = "UPGIAHANHDPO",
                    MOTA = "Sửa thông tin gia hạn hợp đồng điện."
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

        

        public bool IsInUse(string ma)
        {
            return false;
        }        

        public string NewId()
        {
            var sToday = DateTime.Now.ToString("yyMM");

            //var query = (from p in _db.HOPDONGs.Where(p => p.SOHD.Substring(0, 8).Contains(sToday))  select p.SOHD).Max();
            var query = _db.THAYHOPDONGPOs.Max(p => p.IDTHDPO.Substring(4,6));

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
