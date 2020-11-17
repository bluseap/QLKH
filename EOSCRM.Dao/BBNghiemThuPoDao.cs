using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class BBNghiemThuPoDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly ReportClass _rpDao = new ReportClass();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public BBNghiemThuPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public BBNGHIEMTHUPO Get(string macb)
        {
            return _db.BBNGHIEMTHUPOs.Where(p => p.MABBNTPO.Equals(macb)).SingleOrDefault();
        }

        public BBNGHIEMTHUPO GetMADDK(string macb)
        {
            return _db.BBNGHIEMTHUPOs.Where(p => p.MADDKPO.Equals(macb)).SingleOrDefault();
        }

        public List<BBNGHIEMTHUPO> GetList()
        {
            //return _db.BBNGHIEMTHUs.OrderBy(c => c.MABBNT).ToList();

            var nt = from bb in _db.BBNGHIEMTHUPOs join don in _db.DONDANGKYPOs on bb.MABBNTPO equals don.MADDKPO
                     orderby bb.MABBNTPO
                     select bb;
            return nt.OrderByDescending(b => b.NGAYLAPBB).ToList();
        }

        public List<BBNGHIEMTHUPO> GetListKV(string makv)
        {
            //return _db.BBNGHIEMTHUs.OrderBy(c => c.MABBNT).ToList();

            var nt = from bb in _db.BBNGHIEMTHUPOs
                     join don in _db.DONDANGKYPOs on bb.MABBNTPO equals don.MADDKPO
                     where don.MAKVPO.Equals(makv)
                     //orderby bb.MABBNTPO
                     orderby bb.NGAYNHAP descending
                     select bb;
            return nt.OrderByDescending(b => b.NGAYNHAP).ToList();
        }

        public List<BBNGHIEMTHUPO> GetListKV(DateTime? fromDate, DateTime? toDate, string makv)
        {
            var query = (from bb in _db.BBNGHIEMTHUPOs
                     join don in _db.DONDANGKYPOs on bb.MABBNTPO equals don.MADDKPO
                     where don.MAKVPO.Equals(makv)
                     orderby bb.MABBNTPO
                     select bb) .AsQueryable();

            if (fromDate.HasValue || toDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB >= fromDate || dh.NGAYLAPBB <= toDate).AsQueryable();

            //if (toDate.HasValue)
            //    query = query.Where(dh => dh.NGAYLAPBB <= toDate).AsQueryable();

            return query.OrderByDescending(dh => dh.NGAYLAPBB).ToList();
        }  

        public List<BBNGHIEMTHUPO> GetList(DateTime? fromDate, DateTime? toDate)
        {
            var query = _db.BBNGHIEMTHUPOs.AsQueryable();            

            if (fromDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB >= fromDate).AsQueryable();

            if (toDate.HasValue)
                query = query.Where(dh => dh.NGAYLAPBB <= toDate).AsQueryable();
            
            return query.OrderByDescending(dh => dh.NGAYLAPBB).ToList();
        }        

        public List<BBNGHIEMTHUPO> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public List<BBNGHIEMTHUPO> GetListKVTenMaDon(string makv, string tukhoa)
        {    
            var query = _db.DONDANGKYPOs.AsQueryable();

            if (!string.IsNullOrEmpty(tukhoa))
                query = query.Where(d => d.MADDKPO.Equals(tukhoa) || d.TENKH.Contains(tukhoa));


            var nt = from bb in _db.BBNGHIEMTHUPOs
                     join don in query on bb.MABBNTPO equals don.MADDKPO
                     where don.MAKVPO.Equals(makv)
                     orderby bb.MABBNTPO
                     select bb;
            return nt
                .OrderByDescending(b => b.NGAYLAPBB).ToList();
        }

        public int Count( )
        {
            return _db.BBNGHIEMTHUPOs.Count();
        }

        public Message Insert(BBNGHIEMTHUPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {

                _db.Connection.Open();     

                _db.BBNGHIEMTHUPOs.InsertOnSubmit(objUi);

                var ddk = _db.DONDANGKYPOs.SingleOrDefault(d => d.MADDKPO.Equals(objUi.MADDKPO));
                if (ddk != null)
                    ddk.TTNT = "NT_A";
                else
                {
                    trans.Rollback();
                    return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập biên bản nghiệm thu.", "Mã đơn đăng ký không tồn tại.");
                }

                _db.SubmitChanges();

                trans.Commit();


                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "NT_A",
                    MOTA = "Nhập biên bản nghiệm thu điện."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
               
                //_db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "nghiệm thu", objUi.MABBNTPO);
            }
            return msg;
        }

        public Message Update(BBNGHIEMTHUPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MABBNTPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDKPO = objUi.MADDKPO;
                    objDb.MANV1 = objUi.MANV1;
                    objDb.HOTEN1 = objUi.HOTEN1;
                    objDb.MANV2 = objUi.MANV2;
                    objDb.HOTEN2 = objUi.HOTEN2;
                    objDb.MANV3 = objUi.MANV3;
                    objDb.HOTEN3 = objUi.HOTEN3;
                    objDb.CHIEUCAO = objUi.CHIEUCAO;
                    objDb.KHOANGCACH = objUi.KHOANGCACH;
                    objDb.VITRI = objUi.VITRI;
                    objDb.CHINIEMM1 = objUi.CHINIEMM1;
                    objDb.CHINIEMM2 = objUi.CHINIEMM2;
                    objDb.KETLUAN = objUi.KETLUAN;
                    objDb.MADHPO = objUi.MADHPO;
                    objDb.HETHONGCN = objUi.HETHONGCN;
                    objDb.MANV = objUi.MANV;


                    objDb.CNKIEMDINH = objUi.CNKIEMDINH;
                    objDb.CNHOPDAUDAY = objUi.CNHOPDAUDAY;
                    objDb.CNHOPNHUA = objUi.CNHOPNHUA;
                    objDb.NAPDAYHOPDAUDAY = objUi.NAPDAYHOPDAUDAY;
                    objDb.TTDONGHODIEN = objUi.TTDONGHODIEN;
                    objDb.TTBANGGONHUA = objUi.TTBANGGONHUA;
                    objDb.KCDENPOTEKH = objUi.KCDENPOTEKH;
                    objDb.DOVONGONGSU = objUi.DOVONGONGSU;
                    objDb.TCCHIEUDAIDAY = objUi.TCCHIEUDAIDAY;
                    objDb.LOAITRUDO = objUi.LOAITRUDO;
                    objDb.SLTRUDO = objUi.SLTRUDO;
                    objDb.CHIEUDAIVUOTS = objUi.CHIEUDAIVUOTS;
                    objDb.CHIEUCAOVUOTS = objUi.CHIEUCAOVUOTS;
                    objDb.CHIEUDAIVUOTD = objUi.CHIEUDAIVUOTD;
                    objDb.CHIEUCAOVUOTD = objUi.CHIEUCAOVUOTD;
                    objDb.TTDAYNHANH = objUi.TTDAYNHANH;
                    objDb.SLTTDAYNHANH = objUi.SLTTDAYNHANH;
                    objDb.DAUNOIDAYNHANHDZ = objUi.DAUNOIDAYNHANHDZ;
                    objDb.SLDAUNOIDAYNHANHDZ = objUi.SLDAUNOIDAYNHANHDZ;
                    objDb.LOAIDAYHOTRUOC = objUi.LOAIDAYHOTRUOC;
                    objDb.CHIEUDAIDAYTRUOC = objUi.CHIEUDAIDAYTRUOC;
                    objDb.LOAIDAYHOSAU = objUi.LOAIDAYHOSAU;
                    objDb.CHIEUDAIDAYSAU = objUi.CHIEUDAIDAYSAU;
                    objDb.LOAICHICHAY = objUi.LOAICHICHAY;
                    objDb.DAYNHANHSUDUNG = objUi.DAYNHANHSUDUNG;
                    objDb.DAUNOIDNDZBKEO = objUi.DAUNOIDNDZBKEO;
                    objDb.DAYNHANHVUOTMAI = objUi.DAYNHANHVUOTMAI;
                    objDb.KCVUOTMAINHA = objUi.KCVUOTMAINHA;
                    objDb.DSHOTRUOC = objUi.DSHOTRUOC;
                    objDb.DSHOSAU = objUi.DSHOSAU;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "NT_P",
                        MOTA = "Cập nhập bản nghiệm thu điện."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nghiệm thu", objUi.MABBNTPO);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "nghiệm thu");
            }
            return msg;
        }

        public Message UpdateNTToTK(BBNGHIEMTHUPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MABBNTPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDKPO = objUi.MADDKPO;
                    objDb.MANV1 = objUi.MANV1;
                    objDb.HOTEN1 = objUi.HOTEN1;
                    objDb.MANV2 = objUi.MANV2;
                    objDb.HOTEN2 = objUi.HOTEN2;
                    objDb.MANV3 = objUi.MANV3;
                    objDb.HOTEN3 = objUi.HOTEN3;
                    objDb.CHIEUCAO = objUi.CHIEUCAO;
                    objDb.KHOANGCACH = objUi.KHOANGCACH;
                    objDb.VITRI = objUi.VITRI;
                    objDb.CHINIEMM1 = objUi.CHINIEMM1;
                    objDb.CHINIEMM2 = objUi.CHINIEMM2;
                    objDb.KETLUAN = objUi.KETLUAN;
                    objDb.MADHPO = objUi.MADHPO;
                    objDb.HETHONGCN = objUi.HETHONGCN;
                    objDb.MANV = objUi.MANV;


                    objDb.CNKIEMDINH = objUi.CNKIEMDINH;
                    objDb.CNHOPDAUDAY = objUi.CNHOPDAUDAY;
                    objDb.CNHOPNHUA = objUi.CNHOPNHUA;
                    objDb.NAPDAYHOPDAUDAY = objUi.NAPDAYHOPDAUDAY;
                    objDb.TTDONGHODIEN = objUi.TTDONGHODIEN;
                    objDb.TTBANGGONHUA = objUi.TTBANGGONHUA;
                    objDb.KCDENPOTEKH = objUi.KCDENPOTEKH;
                    objDb.DOVONGONGSU = objUi.DOVONGONGSU;
                    objDb.TCCHIEUDAIDAY = objUi.TCCHIEUDAIDAY;
                    objDb.LOAITRUDO = objUi.LOAITRUDO;
                    objDb.SLTRUDO = objUi.SLTRUDO;
                    objDb.CHIEUDAIVUOTS = objUi.CHIEUDAIVUOTS;
                    objDb.CHIEUCAOVUOTS = objUi.CHIEUCAOVUOTS;
                    objDb.CHIEUDAIVUOTD = objUi.CHIEUDAIVUOTD;
                    objDb.CHIEUCAOVUOTD = objUi.CHIEUCAOVUOTD;
                    objDb.TTDAYNHANH = objUi.TTDAYNHANH;
                    objDb.SLTTDAYNHANH = objUi.SLTTDAYNHANH;
                    objDb.DAUNOIDAYNHANHDZ = objUi.DAUNOIDAYNHANHDZ;
                    objDb.SLDAUNOIDAYNHANHDZ = objUi.SLDAUNOIDAYNHANHDZ;
                    objDb.LOAIDAYHOTRUOC = objUi.LOAIDAYHOTRUOC;
                    objDb.CHIEUDAIDAYTRUOC = objUi.CHIEUDAIDAYTRUOC;
                    objDb.LOAIDAYHOSAU = objUi.LOAIDAYHOSAU;
                    objDb.CHIEUDAIDAYSAU = objUi.CHIEUDAIDAYSAU;
                    objDb.LOAICHICHAY = objUi.LOAICHICHAY;
                    objDb.DAYNHANHSUDUNG = objUi.DAYNHANHSUDUNG;
                    objDb.DAUNOIDNDZBKEO = objUi.DAUNOIDNDZBKEO;
                    objDb.DAYNHANHVUOTMAI = objUi.DAYNHANHVUOTMAI;
                    objDb.KCVUOTMAINHA = objUi.KCVUOTMAINHA;
                    objDb.DSHOTRUOC = objUi.DSHOTRUOC;
                    objDb.DSHOSAU = objUi.DSHOSAU;

                    var ddk = _db.DONDANGKYPOs.SingleOrDefault(d => d.MADDKPO.Equals(objUi.MADDKPO));
                    if (ddk != null)
                    {
                        //ddk.TTNT = "NT_RA";
                        ddk.TTTK = "TK_P";
                    }
                    else
                    {
                        //trans.Rollback();
                        return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Nhập biên bản nghiệm thu.", "Mã đơn đăng ký không tồn tại.");
                    }

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "NT_P",
                        MOTA = "Cập nhập bản nghiệm thu điện."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nghiệm thu", objUi.MABBNTPO);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "nghiệm thu");
            }
            return msg;
        }

        public bool IsInUse(string macb)
        {
            return _db.BBNGHIEMTHUs.Where(p => p.MABBNT.Equals(macb)).Count() > 0;
        }

        public Message Delete(BBNGHIEMTHUPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MABBNTPO );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "nghiệm thu", objUi.MABBNTPO);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.BBNGHIEMTHUPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "nghiệm thu");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<BBNGHIEMTHUPO> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách nghiệm thu");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "nghiệm thu", failed, "nghiệm thu");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách nghiệm thu");
            }

            return msg;
        }

        public Message UpdateGhiChuNT(string maddkpo, string ghichu, string useragent, string ipAddress, string sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(maddkpo);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.GHICHU = ghichu;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = maddkpo,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "GhiChu_NTPO",
                        MOTA = "Update ghi chú nghiệm thu."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nghiệm thu", maddkpo);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "nghiệm thu");
            }
            return msg;
        }

    }
}
