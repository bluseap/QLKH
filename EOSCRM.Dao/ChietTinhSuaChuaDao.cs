using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using System.Net;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChietTinhSuaChuaDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();


        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ChietTinhSuaChuaDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CHIETTINHSUACHUA   Get(string ma)
        {
            return _db.CHIETTINHSUACHUAs.Where(p => p.MADON .Equals(ma)).SingleOrDefault();
        }

        public List<CHIETTINHSUACHUA> GetList()
        {
            return _db.CHIETTINHSUACHUAs.ToList();
        }

        public List<CHIETTINHSUACHUA> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.CHIETTINHSUACHUAs.Count();
        }


        public Message Insert(CHIETTINHSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CHIETTINHSUACHUAs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADON,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Lập chiết tính sữa chữa"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chiết tính sữa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chiết tính sữa chữa ", objUi .TENCT );
            }
            return msg;
        }

        public Message Update(CHIETTINHSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CONGVIEC = objUi.CONGVIEC;
                    objDb.CPCHUNG = objUi.CPCHUNG;
                    objDb.CPKHAC = objUi.CPKHAC;
                    objDb.CPNHANCONG = objUi.CPNHANCONG;
                    objDb.CPTHIETKE = objUi.CPTHIETKE;
                    objDb.CPTHUNHAP = objUi.CPTHUNHAP;
                    objDb.CPVATLIEU = objUi.CPVATLIEU;
                    objDb.DIACHIHM = objUi.DIACHIHM;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.HSCHUNG = objUi.HSCHUNG;
                    objDb.HSNHANCONG = objUi.HSNHANCONG;
                    objDb.HSTHIETKE1 = objUi.HSTHIETKE1;
                    objDb.HSTHIETKE2 = objUi.HSTHIETKE2;
                    objDb.HSTHIETKE3 = objUi.HSTHIETKE3;
                    objDb.HSTHUE = objUi.HSTHUE;
                    objDb.HSTHUNHAP = objUi.HSTHUNHAP;
                    objDb.ISSTK = objUi.ISSTK;
                    objDb.LOAICT = objUi.LOAICT;

                    if (!string.IsNullOrEmpty(objUi.MANVLCT))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVLCT));

                    //objDb.MANVLCT = objUi.MANVLCT;
                    
                    objDb.NGAYGUI_CN = objUi.NGAYGUI_CN;
                    objDb.NGAYLCT = objUi.NGAYLCT;
                    objDb.NGAYNHAN_CN = objUi.NGAYNHAN_CN;
                    objDb.QUYETTOAN = objUi.QUYETTOAN;
                    objDb.SOCT = objUi.SOCT;
                    objDb.TENCT = objUi.TENCT;
                    objDb.TENHM = objUi.TENHM;
                    objDb.TIENTHUE = objUi.TIENTHUE;
                    objDb.TONG_ST = objUi.TONG_ST;
                    objDb.TONG_TT = objUi.TONG_TT;
                     // Submit changes to db
                    _db.SubmitChanges();




                    //TODO: update all fields
                    objDb.CONGVIEC = objUi.CONGVIEC;
                    objDb.CPCHUNG = objUi.CPCHUNG;
                    objDb.CPKHAC = objUi.CPKHAC;
                    objDb.CPNHANCONG = objUi.CPNHANCONG;
                    objDb.CPTHIETKE = objUi.CPTHIETKE;
                    objDb.CPTHUNHAP = objUi.CPTHUNHAP;
                    objDb.CPVATLIEU = objUi.CPVATLIEU;
                    objDb.DIACHIHM = objUi.DIACHIHM;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.HSCHUNG = objUi.HSCHUNG;
                    objDb.HSNHANCONG = objUi.HSNHANCONG;
                    objDb.HSTHIETKE1 = objUi.HSTHIETKE1;
                    objDb.HSTHIETKE2 = objUi.HSTHIETKE2;
                    objDb.HSTHIETKE3 = objUi.HSTHIETKE3;
                    objDb.HSTHUE = objUi.HSTHUE;
                    objDb.HSTHUNHAP = objUi.HSTHUNHAP;
                    objDb.ISSTK = objUi.ISSTK;
                    objDb.LOAICT = objUi.LOAICT;

                    objDb.NGAYLCT = objUi.NGAYLCT;
                    if (!string.IsNullOrEmpty(objUi.MANVLCT))
                        objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVLCT));

                    objDb.NGAYDCT = objUi.NGAYDCT;
                    if (!string.IsNullOrEmpty(objUi.MANVDCT))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVDCT));

                    objDb.NGAYGUI_CN = objUi.NGAYGUI_CN;

                    objDb.NGAYNHAN_CN = objUi.NGAYNHAN_CN;
                    objDb.QUYETTOAN = objUi.QUYETTOAN;
                    objDb.SOCT = objUi.SOCT;
                    objDb.TENCT = objUi.TENCT;
                    objDb.TENHM = objUi.TENHM;
                    objDb.TIENTHUE = objUi.TIENTHUE;
                    objDb.TONG_ST = objUi.TONG_ST;
                    objDb.TONG_TT = objUi.TONG_TT;

                    objDb.FILECT = objUi.FILECT;
                    objDb.TONGCONG = objUi.TONGCONG;
                    objDb.NHANCONG = objUi.NHANCONG;
                    objDb.CPMAY = objUi.CPMAY;
                    objDb.CPTTVT = objUi.CPTTVT;
                    objDb.CPTTMAY = objUi.CPTTMAY;
                    objDb.TCPTT = objUi.TCPTT;
                    objDb.HSCPC = objUi.HSCPC;
                    objDb.CPTTHUE = objUi.CPTTHUE;
                    objDb.CPC = objUi.CPC;
                    objDb.HSPVL = objUi.HSPVL;
                    objDb.HSCPM = objUi.HSCPM;
                    objDb.HSTTP = objUi.HSTTP;
                    objDb.CPKSHS = objUi.CPKSHS;
                    objDb.TTP = objUi.TTP;
                    objDb.CHUNGONG = objUi.CHUNGONG;
                    objDb.KPDT = objUi.KPDT;
                    objDb.GIAMGIACPVL = objUi.GIAMGIACPVL;
                    objDb.GIAMGIACPNC = objUi.GIAMGIACPNC;
                    objDb.SDGIA = objUi.SDGIA;

                    objDb.ISTHUTIEN = objUi.ISTHUTIEN;
                    objDb.TIENNCNHANVIEN = objUi.TIENNCNHANVIEN;


                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADON,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.I.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_P.ToString(),
                        MOTA = "Cập nhật tính sữa chữa"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Chiết tính sữa chữa ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "chiết tính sữa chữa", objUi.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chiết tính sữa chữa ", objUi.TENCT);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.CTCHIETTINHSUACHUAs.Where(p => p.MADON.Equals(ma)).Count() > 0)
                return true;
            else if (_db.DAOLAPSUACHUAs.Where(p => p.MADON.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(CHIETTINHSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính sữa chữa ", objUi.TENCT);
                    return msg;
                }

                var ctvattuslist = _db.CTCHIETTINHSUACHUAs.Where(p => p.MADON.Equals(objDb)).ToList();
                foreach (var ctvattu in ctvattuslist)
                {
                    _db.CTCHIETTINHSUACHUAs.DeleteOnSubmit(ctvattu);
                }

               
                var ctdaolaplist = _db.DAOLAPSUACHUAs .Where(p => p.MA .Equals(objDb)).ToList();
                foreach (var daolap in ctdaolaplist)
                {
                    _db.DAOLAPSUACHUAs.DeleteOnSubmit(daolap);
                }

              

                // Set delete info
                _db.CHIETTINHSUACHUAs.DeleteOnSubmit(objDb);
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
                    MATT = TTCT.CT_P.ToString(),
                    MOTA = "Xóa chiết tính sữa chữa"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chiết tính sữa chữa ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Chiết tính sữa chữa ", objUi.TENCT);
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<CHIETTINHSUACHUA> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chiết tính sữa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách chiết tính sữa chữa ");
            }

            return msg;
        }

        public Message CreateChietTinh(GIAIQUYETTHONGTINSUACHUA objGqTtSc, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // create chiet tinh from thiet ke
                var chiettinhsuachua = new CHIETTINHSUACHUA()
                {
                    MADON = objGqTtSc.MADON ,
                    //SOCT
                    LOAICT = CT.CT.ToString(),
                    
                    GHICHU = objGqTtSc.GHICHU ,
                    MANVLCT = sManv
                    
                };


                if(objGqTtSc.KHACHHANG != null)
                {
                    chiettinhsuachua.TENCT = objGqTtSc.KHACHHANG.TENKH;
                    chiettinhsuachua.DIACHIHM = objGqTtSc.KHACHHANG.SONHA + " " + objGqTtSc.KHACHHANG.DUONGPHO.TENDP;
                }else
                {
                     chiettinhsuachua.TENCT = objGqTtSc.THONGTINKH;
                     chiettinhsuachua.DIACHIHM = "";
                }

                _db.CHIETTINHSUACHUAs.InsertOnSubmit(chiettinhsuachua);


                var giaiquyetttsc = _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Equals(objGqTtSc.MADON)).FirstOrDefault();;
                giaiquyetttsc.TTCT = TTCT.CT_P.ToString();

                
                _db.SubmitChanges();

                
                // commit
                trans.Commit();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objGqTtSc.MADON,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Lập chiết tính sữa chữa"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chiết tính");
            }

            return msg;
        }

        public Message DuyetChietTinh(CHIETTINHSUACHUA objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
         
            try
            {
                var objDb = Get(objUi.MADON);
                var giaiquyetttsuachua = _db.GIAIQUYETTHONGTINSUACHUAs.Single(p => p.MADON.Equals(objUi.MADON));
                var chitietchiettinhsuachuaList = new ChiTietChietTinhSuaChuaDao().GetList(objUi.MADON);
                var chitietdaolapList = new DaoLapChietTinhSuaChuaDao().GetList(objUi.MADON);
                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.ISTHUTIEN = objUi.ISTHUTIEN;
                    giaiquyetttsuachua.TTCT = TTCT.CT_A.ToString();

                    decimal chiphivattu = 0;
                    decimal chiphinhancong = 0;
                    decimal chiphidaolap = 0;


                    foreach (var chitietchiettinhsuachua in chitietchiettinhsuachuaList)
                    {
                        if (chitietchiettinhsuachua.TIENVT != null)
                        chiphivattu = chiphivattu + (decimal )chitietchiettinhsuachua.TIENVT;
                        if (chitietchiettinhsuachua.TIENNC  != null)
                            chiphinhancong = chiphinhancong + (decimal)chitietchiettinhsuachua.TIENNC;
                    }

                    foreach (var chitietdaolap in chitietdaolapList)
                    {
                        if (chitietdaolap.THANHTIENCP != null)
                            chiphidaolap = chiphidaolap + (decimal )chitietdaolap.THANHTIENCP;
                    }
                    //Tinh tiền công trình trên

                    objDb.CPCHUNG = 0;
                    objDb.CPKHAC = chiphidaolap;
                    objDb.CPNHANCONG = chiphinhancong;
                    objDb.CPTHIETKE = 0;
                    objDb.CPTHUNHAP = 0;
                    objDb.CPVATLIEU = chiphivattu ;
                  
                    objDb.HSCHUNG = objUi.HSCHUNG;
                    objDb.HSNHANCONG = 1;
                    objDb.HSTHIETKE1 = 1;
                    objDb.HSTHIETKE2 = 1;
                    objDb.HSTHIETKE3 = 1;
                    objDb.HSTHUE = 1;
                    objDb.HSTHUNHAP = 1;
                    objDb.TIENNCNHANVIEN = 0;
                    objDb.TONG_ST = 0;
                    objDb.TONG_TT = chiphidaolap + chiphinhancong + chiphivattu ;
                    // Submit changes to db
                    _db.SubmitChanges();

                }
                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADON,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_A.ToString(),
                    MOTA = "Duyệt chiết tính sữa chữa"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion              
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chiết tính");
            }
            catch (Exception ex)
            {
                 

                msg = ExceptionHandler.HandleInsertException(ex, "Chiết tính");
            }

            return msg;
        }

        public bool UpdateChiPhiForChietTinh(string ma)
        {
            try
            {
                /*
                var chiettinh = _db.CHIETTINHSUACHUAs.SingleOrDefault(p => p.MADON.Equals(ma));
                if (chiettinh == null)
                    return false;

                
                decimal cpvatlieu = 0;
                decimal cpnhancong = 0;

                decimal tienthue = 0;
                decimal tongTt = 0;
                decimal tongSt = 0;

                var ctchiettinhnd117List = _db.CTCHIETTINHSUACHUA_ND117s.Where(p => p.MADON .Equals(ma)).ToList();
                foreach (CTCHIETTINHSUACHUA_ND117 ctchiettinhNd117 in ctchiettinhnd117List)
                {
                    if (ctchiettinhNd117.TIENNC != null)
                        cpnhancong = cpnhancong + (decimal)ctchiettinhNd117.TIENNC;

                    if (ctchiettinhNd117.TIENVT != null)
                        cpvatlieu = cpvatlieu + (decimal)ctchiettinhNd117.TIENVT;
                }

                //var daolapNd117S = _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(ma)).ToList();
                //foreach (DAOLAP_ND117 daolapNd117 in daolapNd117S)
                //{
                //    if(daolapNd117 .)
                //}
                //- chi phí vật liệu và nhân công)
               

                cpvatlieu = Math.Round(cpvatlieu/100, 0);
                cpvatlieu = cpvatlieu*100;
                
                cpnhancong = Math.Round(cpnhancong / 100, 0);
                cpnhancong = cpnhancong * 100;

                tongTt = cpvatlieu + cpnhancong;

                tongTt = Math.Round(tongTt / 100, 0);
                tongTt = tongTt * 100;

                tienthue = tongTt*10/100;
                tienthue = Math.Round(tienthue / 100, 0);
                tienthue = tienthue*100;

                tongSt = tienthue + tongTt;
                tongSt = Math.Round(tongSt, 0);


                chiettinh.CPNHANCONG = cpnhancong;
                chiettinh.CPVATLIEU = cpvatlieu;
               
                chiettinh.TIENTHUE = tienthue;
                chiettinh.TONG_TT = tongTt;
                chiettinh.TONG_ST = tongSt;


                _db.SubmitChanges();

                */
                return true;
            }
            catch
            {
                return false;
            }
        }


        private decimal? GetHeSo(string mahs)
        {
            // get dmnk
            var hs = _db.HESOs.Where(h => h.MAHS.Equals(mahs)).SingleOrDefault();
            decimal? gths = (hs != null && hs.GIATRI.HasValue) ? hs.GIATRI.Value : (decimal?)null;

            return gths;
        }

        public Message ChangeFromMBVT(CHIETTINHSUACHUA objUi, MAUBOCVATTU mbvt)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                if (mbvt != null)
                {
                    // clear all existing data
                    var cttk = _db.CTCHIETTINHSUACHUAs.Where(ct => ct.MADON.Equals(objUi.MADON)).ToList();
                    foreach (var ct in cttk)
                    {
                        _db.CTCHIETTINHSUACHUAs.DeleteOnSubmit(ct);
                    }
                    var ctqts = _db.CTQUYETTOANSUACHUAs.Where(p => p.MADON.Equals(objUi.MADON)).ToList();
                    foreach (var ctqt in ctqts)
                    {
                        _db.CTQUYETTOANSUACHUAs.DeleteOnSubmit(ctqt);
                    }
                    var cttk117 = _db.CTCHIETTINHSUACHUA_ND117s.Where(ct => ct.MADON.Equals(objUi.MADON)).ToList();
                    foreach (var ct in cttk117)
                    {
                        _db.CTCHIETTINHSUACHUA_ND117s.DeleteOnSubmit(ct);
                    }
                    var ctqt117s = _db.CTQUYETTOANSUACHUA_ND117s.Where(p => p.MADON.Equals(objUi.MADON)).ToList();
                    foreach (var qt in ctqt117s)
                    {
                        _db.CTQUYETTOANSUACHUA_ND117s.DeleteOnSubmit(qt);
                    }
                    _db.SubmitChanges();

                    // insert from maubocvattu
                    // insert:
                    // - CTMAUBOCVATTU to CTCHIETTINHs, CTCHIETTINH_ND117s
                    var ctmbvt = _db.CTMAUBOCVATTUs.Where(ct => ct.MADDK.Equals(mbvt.MADDK)).ToList();
                    foreach (var mb in ctmbvt)
                    {
                        if (mb.ISCTYDTU.HasValue && mb.ISCTYDTU.Value)
                        {
                            var ctct = new CTCHIETTINHSUACHUA
                            {
                                MADON = objUi.MADON,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC//,
                                //ISCTYDTU = mb.ISCTYDTU
                            };
                            var ctQTSC = new CTQUYETTOANSUACHUA
                            {
                                MADON = objUi.MADON,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC//,
                                //ISCTYDTU = mb.ISCTYDTU
                            };
                            _db.CTQUYETTOANSUACHUAs.InsertOnSubmit(ctQTSC);
                            _db.CTCHIETTINHSUACHUAs.InsertOnSubmit(ctct);
                        }
                        else
                        {
                            var ctct = new CTCHIETTINHSUACHUA_ND117
                            {
                                MADON = objUi.MADON,
                                MAVT = mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC
                            };
                            var ctQTSC117 = new CTQUYETTOANSUACHUA_ND117
                            {
                                MADON = objUi.MADON,
                                MAVT= mb.MAVT,
                                LOAICT = CT.CT.ToString(),
                                LOAICV = "---***---",
                                SOLUONG = mb.SOLUONG,
                                GIAVT = mb.VATTU.GIAVT,
                                TIENVT = mb.SOLUONG * mb.VATTU.GIAVT,
                                GIANC = mb.VATTU.GIANC,
                                TIENNC = mb.SOLUONG * mb.VATTU.GIANC
                            };
                            _db.CTQUYETTOANSUACHUA_ND117s.InsertOnSubmit(ctQTSC117);
                            _db.CTCHIETTINHSUACHUA_ND117s.InsertOnSubmit(ctct);
                        }
                    }

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Đổi mẫu bốc vật tư cho chiết tính sửa chữa");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Đổi mẫu bốc vật tư cho chiết tính sửa chữa");
            }
            return msg;
        }
        
        public Message CreateChietTinh2(GIAIQUYETTHONGTINSUACHUA obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var sonha = obj.KHACHHANG != null && obj.KHACHHANG.SONHA.Trim() != "" ? 
                                    obj.KHACHHANG.SONHA.Trim() + ", " : "";
                var tenduong = obj.KHACHHANG != null ? obj.KHACHHANG.DUONGPHO.TENDP + ", " : "";
                var tenphuong = obj.KHACHHANG != null && obj.KHACHHANG.PHUONG != null
                                    ? obj.KHACHHANG.PHUONG.TENPHUONG + ", " : "";
                var tenkv = obj.KHACHHANG != null ? obj.KHACHHANG.KHUVUC.TENKV : "";

                var diachild = obj.KHACHHANG != null ?
                                    string.Format("{0}{1}{2}{3}", sonha, tenduong, tenphuong, tenkv) : 
                                    obj.THONGTINKH;

                // create chiet tinh from thiet ke
                var chiettinh = new CHIETTINHSUACHUA
                {
                    MADON = obj.MADON,
                    TENCT = obj.KHACHHANG != null ? obj.KHACHHANG.TENKH : obj.THONGTINKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = diachild,
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,

                    //NGAYLCT
                    //NGAYGUI_CN
                    //NGAYNHAN_CN
                    //ISSTK
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,

                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1,
                };

                // begin 
                var quyettoansc = new QUYETTOANSUACHUA
                {
                    MADON = obj.MADON,
                    TENCT = obj.KHACHHANG != null ? obj.KHACHHANG.TENKH : obj.THONGTINKH,
                    TENHM = Constants.CongTacDefault,
                    DIACHIHM = diachild,
                    GHICHU = Constants.GhiChuThietKeDefault,
                    MANVLCT = sManv,
                    NGAYLCT = DateTime.Now,
                    QUYETTOAN = 0,
                    CONGVIEC = 0,
                    CPVATLIEU = 0,
                    CPNHANCONG = 0,
                    CPCHUNG = 0,
                    CPTHUNHAP = 0,
                    CPTHIETKE = 0,
                    CPKHAC = 0,
                    HSNHANCONG = GetHeSo(MAHS.HSNC1),   // he so nhan cong 1
                    HSCHUNG = GetHeSo(MAHS.HSCPC),      // he so chi phi chung
                    HSCPC = GetHeSo(MAHS.HSCPK),        // he so phi khac
                    HSTHUNHAP = GetHeSo(MAHS.HSTHU),    // he so thu nhap chiu thue tinh truoc
                    HSTHIETKE1 = GetHeSo(MAHS.HSTK1),   // he so thiet ke 1
                    HSTHIETKE2 = GetHeSo(MAHS.HSTK2),   // he so thiet ke 2
                    HSTHIETKE3 = GetHeSo(MAHS.HSNC2),   // he so nhan cong 2
                    HSTHUE = GetHeSo(MAHS.HSTHE),       // he so thue
                    TIENTHUE = 0,
                    TONG_TT = 0,
                    TONG_ST = 0,

                    //NGAYLCT
                    //NGAYGUI_CN
                    //NGAYNHAN_CN
                    //ISSTK
                    FILECT = "",
                    TONGCONG = 0,
                    NHANCONG = 0,
                    CPMAY = 0,
                    CPTTVT = 0,
                    CPTTMAY = 0,
                    TCPTT = 0,

                    CPTTHUE = 0,
                    CPC = 0,
                    HSPVL = 0,
                    HSCPM = 0,
                    HSTTP = 0,
                    CPKSHS = 0,
                    TTP = 0,
                    CHUNGONG = 0,
                    KPDT = "",
                    GIAMGIACPVL = 0,
                    GIAMGIACPNC = 0,
                    SDGIA = 1,
                    NGAYDQT=null
                };
                // end
                _db.CHIETTINHSUACHUAs.InsertOnSubmit(chiettinh);

                _db.SubmitChanges();
                
                _db.QUYETTOANSUACHUAs.InsertOnSubmit(quyettoansc);

                // update dondangky

                var objDDK = _db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MADON.Equals(obj.MADON)).FirstOrDefault();
                objDDK.TTCT = TTCT.CT_P.ToString();
                _db.SubmitChanges();

                // commit
                trans.Commit();

                UpdateChiPhiForChietTinh(obj.MADON);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADON,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTCT.CT_N.ToString(),
                    MOTA = "Lập chiết tính sữa chữa"
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Chạy chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Chạy chiết tính");
            }

            return msg;
        }
    }
}
