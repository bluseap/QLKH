using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class NhanVienVayDao
    {
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public NhanVienVayDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public NVVAYCD  Get(string ma)
        {
            return _db.NVVAYCDs.Where(p => p.MANVV.Equals(ma)).SingleOrDefault();
        }

        public LANVAY GetLanVay(string ma)
        {
            return _db.LANVAYs.Where(p => p.MALV.Equals(ma)).SingleOrDefault();
        }

        public LANVAY GetLanVayMaNVV(string ma)
        {
           return _db.LANVAYs.Where(p => p.MANVV.Equals(ma) && p.THANHTOAN.Equals("CO")).SingleOrDefault();
        }

        public KYVAYCD GetNVVayKy(string manvv, int nam, int thang)
        {
            return _db.KYVAYCDs.Where(p => p.MANVV.Equals(manvv) && p.NAM.Equals(nam) && p.THANG.Equals(thang)).SingleOrDefault();
        }

        public List<KYVAYCD> GetListKyVay(int nam, int thang)
        {
            return _db.KYVAYCDs.Where(p => p.NAM.Equals(nam) && p.THANG.Equals(thang)).ToList();
        }

        public List<KYVAYCD> GetListKyVayMN(string manv)
        {
            return _db.KYVAYCDs.Where(p => p.MANVV.Equals(manv))
                .OrderByDescending(p => p.THANG)
                .OrderByDescending(p => p.NAM)
                .ToList();
        }

        public List<NVVAYCD> Search(string key)
        {
            return _db.NVVAYCDs.Where(p => p.HOTEN.ToUpper().Contains(key.ToUpper())
                || p.MANVV.ToUpper().Contains(key.ToUpper()) 
                || p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                || p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper()))
                    .OrderBy(p=>p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NVVAYCD> SearchKV(string key, string makv, string macv)
        {
            //return _db.NHANVIENs.Where(p => (p.MAKV.Equals(makv) && p.MAPB.Equals(macv))// && p.HOTEN.ToUpper().Contains(key.ToUpper())
                //|| p.MANV.ToUpper().Contains(key.ToUpper())
                //|| p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                //|| p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper())
                //)
            var query = from nvv in _db.NVVAYCDs
                        //join lv in _db.LANVAYs on nvv.MANVV equals lv.MALV
                        where nvv.MAKV.Equals(makv)
                           && nvv.HOTEN.ToUpper().Contains(key.ToUpper())
                        select nvv;

            return query
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NVVAYCD> SearchKV2(string key, string makv)
        {
            return _db.NVVAYCDs.Where(p => ((p.MAKV.Equals(makv) && p.HOTEN.ToUpper().Contains(key.ToUpper()))))
                //|| p.MANV.ToUpper().Contains(key.ToUpper())
                //|| p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                //|| p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper())))
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NVVAYCD> SearchKV3(string key, string makv, string macv)
        {
            return _db.NVVAYCDs.Where(p => (p.MAKV.Equals(makv))// && p.HOTEN.ToUpper().Contains(key.ToUpper())
                //|| p.MANV.ToUpper().Contains(key.ToUpper())
                //|| p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                //|| p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper())
                //)
                )
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NVVAYCD> Search(string key, string mapb)
        {
            var query = _db.NVVAYCDs.AsQueryable();

            if (mapb != "" && mapb != "%")
                query = query.Where(nv => nv.MAPB.Equals(mapb)).AsQueryable();

            return query.Where(p => p.HOTEN.ToUpper().Contains(key.ToUpper())
                || p.MANVV.ToUpper().Contains(key.ToUpper())
                || p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                || p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper()))
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NVVAYCD> GetList()
        {
            return _db.NVVAYCDs.OrderByDescending(p => p.MANVV).ToList();
        }

        public List<VTRACUUNVVAYO> GetListTraCuuNV()
        {
            return _db.VTRACUUNVVAYOs.OrderBy(p => p.HOTEN)
                .OrderByDescending(p => p.SOLANVAY)
                .ToList();
        }

        public List<VAYNVLAN> GetListLanVay()
        {
            var query = _db.NVVAYCDs
                        .Join(_db.LANVAYs, nv => nv.MANVV, lv => lv.MANVV, (nv, lv) => new { nv, lv })
                        .Where(@res => @res.lv.THANHTOAN.Equals("CO"));

            return query.Select(@res => new
            {
                @res.lv.MALV,
                @res.lv.MANVV,
                @res.lv.TIENVAY,
                @res.lv.LAISUAT,
                @res.lv.NGAYVAY,
                @res.lv.THANGVAY,
                @res.lv.KYBATDAU,
                @res.lv.KYKETTHUC,
                @res.lv.THANHTOAN,
                @res.nv.HOTEN,
                @res.nv.MAPB
            }).AsEnumerable().Select(x => new VAYNVLAN
            {
                MALV = x.MALV,
                MANVV = x.MANVV,
                TIENVAY = x.TIENVAY,
                LAISUAT = x.LAISUAT,
                NGAYVAY = x.NGAYVAY,
                THANGVAY = x.THANGVAY,
                KYBATDAU = x.KYBATDAU,
                KYKETTHUC = x.KYKETTHUC,
                THANHTOAN = x.THANHTOAN,
                HOTEN = x.HOTEN,
                MAPB = x.MAPB
            }).OrderBy(p => p.HOTEN).OrderBy(p => p.KYBATDAU).ToList();
        }

        public List<VAYKYNHANVIEN> GetListKyVay(DateTime date)
        {
            var query = _db.NVVAYCDs                        
                        .Join(_db.KYVAYCDs, nv => nv.MANVV, kv => kv.MANVV, (nv, kv) => new { nv, kv })
                        .Where(@res => @res.kv.TTLANVAY.Equals("CO")
                            && @res.kv.NAM.Equals(date.Year) && @res.kv.THANG.Equals(date.Month))
                        .OrderBy(p => p.nv.HOTEN)
                        .OrderByDescending(p => p.kv.TRAKYLAN)
                        ;

            return query.Select(@res => new
            {
                 @res.kv.MANVV, @res.kv.NAM, @res.kv.THANG, @res.kv.TIENGOC, @res.kv.TIENLAI, @res.kv.MAHTTTT,
                 @res.kv.TONGTIEN, @res.kv. CONLAI, @res.kv.THANHTOAN, @res.kv.TRAKYLAN,
                 @res.nv.HOTEN, @res.nv.MAPB, @res.kv.KYBATDAU, @res.kv.KYKETTHUC, @res.kv.THANGVAY

            }).AsEnumerable().Select(x => new VAYKYNHANVIEN
            {
                MANVV = x.MANVV, NAM = x.NAM, THANG = x.THANG, TIENGOC = x.TIENGOC, TIENLAI = x.TIENLAI, MAHTTTT = x.MAHTTTT,
                TONGTIEN = x.TONGTIEN, CONLAI = x.CONLAI, THANHTOAN = x.THANHTOAN, TRAKYLAN = x.TRAKYLAN,
                HOTEN = x.HOTEN, MAPB = x.MAPB, KYBATDAU = x.KYBATDAU, KYKETTHUC = x.KYKETTHUC, THANGVAY = x.THANGVAY

            }).ToList();
        }

        public string SumTongTienGocKy(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month)
                        select kv;

            return query.Sum(m => m.TIENGOC).ToString();
        }

        public string SumTongTienLaiKy(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month)
                        select kv;

            return query.Sum(m => m.TIENLAI).ToString();
        }

        public decimal SumTongTienLaiKyInt(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month)
                        select kv;

            return query.Sum(m => m.TIENLAI).Value;
        }

        public string SumTongTienKy(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month)
                        select kv;

            return query.Sum(m => m.TONGTIEN).ToString();
        }

        public string SumTongTienChuaThuKy(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month) && kv.THANHTOAN.Equals("CO")
                        select kv;

            return query.Sum(m => m.TONGTIEN).ToString();
        }

        public string SumConLaiKy(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month) && kv.THANHTOAN.Equals("CO")
                        select kv;

            return query.Sum(m => m.CONLAI).ToString();
        }

        public Int32 SumLaiNV(string manvv)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.MANVV.Equals(manvv)
                        select kv;

            return (int) query.Sum(m => m.TIENLAI);
        }

        public string SumConLaiKyMax()
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(MaxNam()) && kv.THANG.Equals(MaxNamThang()) //&& kv.THANHTOAN.Equals("CO")
                        select kv;

            return query.Sum(m => m.CONLAI).ToString();
        }

        public string MaxNam()
        {
            var query = from kv in _db.KYVAYCDs                        
                        select kv;

            return query.Max(m => m.NAM).ToString();
        }
        public string MaxNamThang()
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(MaxNam())
                        select kv;

            return query.Max(m => m.THANG).ToString();
        }

        public string SumTongChiConLaiKy(DateTime kynay)
        {
            var query = from kv in _db.KYVAYCDs
                        where kv.NAM.Equals(kynay.Year) && kv.THANG.Equals(kynay.Month)
                        select kv;

            return query.Sum(m => m.CONLAI).ToString();
        }

        public string SumTongTienLai()
        {
            var query = from kv in _db.KYVAYCDs                       
                        select kv;

            return query.Sum(m => m.TIENLAI).ToString();
        }

        public decimal SumTongTienLaiInt()
        {
            var query = from kv in _db.KYVAYCDs
                        select kv;

            return query.Sum(m => m.TIENLAI).Value;
        }

        public List<NVVAYCD> GetListKV(string ma)
        {
            return _db.NVVAYCDs.Where(p => p.MANVV == ma).ToList();
        }

        public NVVAYCD GetKV(string ma)
        {
            return _db.NVVAYCDs.Where(p => p.MANVV == ma).SingleOrDefault();
        }

        public NVVAYCD GetMAPB(string ma)
        {
            return _db.NVVAYCDs.Where(p => p.MAPB == ma).SingleOrDefault();
        }


        public List<NVVAYCD> GetList(String manv, String tennv, String khuvuc, String phongban, String congviec)
        {
            var query = _db.NVVAYCDs.AsEnumerable();

            if (!String.IsNullOrEmpty(manv))
                query = query.Where(nv => nv.MANVV.ToUpper().Contains(manv.ToUpper()));

            if (!String.IsNullOrEmpty(tennv))
                query = query.Where(nv => nv.HOTEN.ToUpper().Contains(tennv.ToUpper()));

            if (!String.IsNullOrEmpty(khuvuc) && khuvuc != "%")
                query = query.Where(nv => nv.MAKV.ToUpper().Equals(khuvuc.ToUpper()));

            if (!String.IsNullOrEmpty(phongban) && phongban != "%")
                query = query.Where(nv => nv.MAPB.ToUpper().Equals(phongban.ToUpper()));

            if (!String.IsNullOrEmpty(congviec) && congviec != "%")
                query = query.Where(nv => nv.MACV.ToUpper().Equals(congviec.ToUpper()));

            return query.OrderByDescending(p => p.MANVV).ToList();
        }

        public List<VTRACUUNVVAYO> GetListTraCuuNVM(String manv, String tennv, String phongban)
        {
            var query = _db.VTRACUUNVVAYOs.AsEnumerable();
            
            if (!String.IsNullOrEmpty(manv))
                query = query.Where(nv => nv.MANVV.ToUpper().Contains(manv.ToUpper()));

            if (!String.IsNullOrEmpty(tennv))
                query = query.Where(nv => nv.HOTEN.ToUpper().Contains(tennv.ToUpper()));            

            if (!String.IsNullOrEmpty(phongban) && phongban != "%")
                query = query.Where(nv => nv.MAPB.ToUpper().Equals(phongban.ToUpper()));

            return query.OrderBy(p => p.HOTEN)
                .OrderByDescending(p => p.SOLANVAY)
                .ToList();
        }

        public List<NVVAYCD> GetListByPB(string maPb)
        {
            return _db.NVVAYCDs.Where(p => p.MAPB.Equals(maPb)).ToList();
        }

        public List<NVVAYCD> GetListByCV(string maCv)
        {
            return _db.NVVAYCDs.Where(p => p.MACV.Equals(maCv)).ToList();
        }

        public List<NVVAYCD> GetListByCVNV(string maCv, string makv)
        {
            return _db.NVVAYCDs.Where(p => p.MACV.Equals(maCv) && p.MAKV == makv).ToList();
        }

        public List<NVVAYCD> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.NVVAYCDs.Count();
        }

        public Message Insert(NVVAYCD objUi, DateTime ngaybatdau, DateTime ngayketthuc, string manv)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.NVVAYCDs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
               
                _rpClass.VayKyTietKiem(objUi.MANVV.ToString(), ngaybatdau, ngayketthuc, manv);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MANVV,
                    IPAddress = "192.168.1.19",
                    MANV = manv,
                    UserAgent = manv,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "VN_A",
                    MOTA = "Nhập nhân viên tham gia tiết kiệm công đoàn."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
               
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nhân viên");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "nhân viên");
            }
            return msg;
        }

        public Message Update(NVVAYCD objUi, string manvv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(manvv);

                if (objDb != null)
                {
                    //TODO: update all fields
                   
                    objDb.DIACHI = objUi.DIACHI  ;
                    objDb.HOTEN = objUi.HOTEN  ;
                    if (!string.IsNullOrEmpty(objUi.MACB))
                        objDb.CAPBAC = _db.CAPBACs.Single(p => p.MACB.Equals(objUi.MACB));
                    //objDb.MACB= objUi.MACB  ;
                    if (!string.IsNullOrEmpty(objUi.MACV))
                        objDb.CONGVIEC = _db.CONGVIECs.Single(p => p.MACV.Equals(objUi.MACV));
                    //objDb.MACV = objUi.MACV ;
                    if (!string.IsNullOrEmpty(objUi.MAKV))
                        objDb.KHUVUC = _db.KHUVUCs.Single(p => p.MAKV.Equals(objUi.MAKV));
                    //objDb.MAKV= objUi.MAKV ;
                    if (!string.IsNullOrEmpty(objUi.MAPB))
                        objDb.PHONGBAN = _db.PHONGBANs.Single(p => p.MAPB.Equals(objUi.MAPB));
                    //objDb.MAPB = objUi.MAPB ;
                    if (!string.IsNullOrEmpty(objUi.MATD))
                        objDb.TRINHDO = _db.TRINHDOs.Single(p => p.MATD.Equals(objUi.MATD));
                    //objDb.MATD  = objUi.MATD  ;
                    objDb.NGAYSINH = objUi.NGAYSINH ;
                   
                    objDb.SODT = objUi.SODT;
                     
                      // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MANVV,
                        IPAddress = "192.168.1.19",
                        MANV = manvv,
                        UserAgent = manvv,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.I.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "VN_S",
                        MOTA = "Sửa nhập nhân viên tham gia tiết kiệm công đoàn."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nhân viên");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nhân viên");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Nhân viên ", objUi.HOTEN    );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if( _db.THICONGs  .Where(p => p.MANV  .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies  .Where(p => p.MANV .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DUONGPHOs .Where(p => p.MANVG .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DUONGPHOs.Where(p => p.MANVT .Equals(ma)).Count() > 0)
                return true;
            else if (_db.KIEMDINHDHs .Where(p => p.MANVKD.Equals(ma)).Count() > 0)
                return true;
            else if (_db.KIEMDINHDHs.Where(p => p.MANVRS.Equals(ma)).Count() > 0)
                return true;
            else if (_db.GIAIQUYETTHONGTINSUACHUAs .Where(p => p.MANVBAO.Equals(ma)).Count() > 0)
                return true;
            else if (_db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MANVN.Equals(ma)).Count() > 0)
                return true;
            else if (_db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MANVXL.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(NVVAYCD objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MANVV );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Nhân viên ", objUi.MANVV );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.NVVAYCDs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Nhân viên ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Nhân viên ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<NVVAYCD> objList, PageAction action)
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
                    Delete(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Nhân viên ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách nhân viên ");
            }

            return msg;
        }

        public string NewId()
        {
            var query = _db.NVVAYCDs.Max(p => p.MANVV);            

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");
            }
            else
            {
                query = "000001";
            }
            return query;
        }

        public string NewIdLanVay()
        {
            var query = _db.LANVAYs.Max(p => p.MALV);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                return temp.ToString("D6");
            }
            else
            {
                query = "000001";
            }
            return query;
        }

        /*public Message KhoiTaoKyVay(DateTime kydau, DateTime kycuoi)
        {
            
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var prevDate = kydau.AddMonths(-1);

                //TODO: Kiểm tra khu vực khởi tạo hay chưa
                var joinQuery = from tt in _db.TIEUTHUs
                                join lck in _db.LOCKSTATUS
                                    on new { tt.THANG, tt.NAM, tt.MADP }
                                    equals new { THANG = lck.KYGT.Month, NAM = lck.KYGT.Year, lck.MADP }
                                where tt.THANG.Equals(kydau.Month) && tt.NAM.Equals(kydau.Year) //&& tt.MAKV.Equals(makv)
                                select new { tt, lck };

                var count = joinQuery.Count();

                if (count > 0)
                {
                    msg = new Message(MessageConstants.E_GCS_KHOITAO_KYDAKHOITAO, MessageType.Error);
                    // commit
                    trans.Rollback();
                    _db.Connection.Close();
                    return msg;
                }

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo kỳ ghi chỉ số");

            }
            catch { }
            
        }
        */


    }

    public class VAYNVLAN
    {
        public VAYNVLAN()
        {
        }

        public VAYNVLAN(string malv, string manvv, decimal tienvay, decimal laisuat, DateTime ngayvay, decimal thangvay,
            DateTime kybatdau, DateTime kyketthuc, string thanhtoan, 
            string hoten, string mapb)
        {
            MALV = malv; MANVV = manvv; TIENVAY = tienvay; LAISUAT = laisuat; NGAYVAY = ngayvay; THANGVAY = thangvay;
            KYBATDAU = kybatdau; KYKETTHUC = kyketthuc; THANHTOAN = thanhtoan;
            HOTEN = hoten; MAPB = mapb;
        }

        public string MALV { get; set; }
        public string MANVV { get; set; }
        public decimal? TIENVAY { get; set; }
        public decimal? LAISUAT { get; set; }
        public DateTime? NGAYVAY { get; set; }
        public decimal? THANGVAY { get; set; }
        public DateTime? KYBATDAU { get; set; }
        public DateTime? KYKETTHUC { get; set; }
        public string THANHTOAN { get; set; }
        public string HOTEN { get; set; }
        public string MAPB { get; set; }
    }

    public class VAYKYNHANVIEN
    {
        public VAYKYNHANVIEN()
        {
        }

        public VAYKYNHANVIEN(string manvv, int nam, int thang, decimal tiengoc, decimal tienlai,  string mahttt,
            decimal tongtien, decimal conlai, string thanhtoan, int trakylan,
            string hoten, string mapb, DateTime kybatdau, DateTime kyketthuc, decimal thangvay )
        {
            MANVV = manvv; NAM = nam; THANG = thang; TIENGOC = tiengoc; TIENLAI = tienlai; MAHTTTT = mahttt;
            TONGTIEN = tongtien; CONLAI = conlai; THANHTOAN = thanhtoan; TRAKYLAN = trakylan;
            HOTEN = hoten; MAPB = mapb; KYBATDAU = kybatdau; KYKETTHUC = kyketthuc; THANGVAY = thangvay;
        }
        
        public string MANVV { get; set; }
        public int NAM { get; set; }
        public int THANG { get; set; }
        public decimal? TIENGOC { get; set; }
        public decimal? TIENLAI { get; set; }
        public string MAHTTTT { get; set; }
        public decimal? TONGTIEN { get; set; }
        public decimal? CONLAI { get; set; }
        public string THANHTOAN { get; set; }
        public int? TRAKYLAN { get; set; }
        public string HOTEN { get; set; }
        public string MAPB { get; set; }            
        public DateTime? KYBATDAU { get; set; }
        public DateTime? KYKETTHUC { get; set; }
        public decimal? THANGVAY { get; set; }
        
        
       
    }

}
