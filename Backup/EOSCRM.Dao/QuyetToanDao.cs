using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class QuyetToanDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly DonDangKyDao _ddkDao = new DonDangKyDao();

        public QuyetToanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public bool DuyetQT(QUYETTOAN qt, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;                
                qt.NGAYDQT = ngayduyet;
                _db.SubmitChanges();

                //update don dang ky 
                var ddk = new DONDANGKY
                {
                    MADDK = qt.MADDK,
                    TTHC = "QT_A"
                };
               _ddkDao.UpdateQT_A(ddk);

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = qt.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.CT05.ToString(),
                    MATT = TTCT.CT_A.ToString(),
                    MOTA = "Duyệt quyết toán cho đơn:"+qt.MADDK
                };
                _kdDao.Insert(luuvetKyduyet);

                // Submit changes to db
                _db.SubmitChanges();
                trans.Commit();
            }
            catch(Exception ex)
            {
                return false;
            }
            return true;
        }
        
        public QUYETTOAN Get(string ma)
        {
            return _db.QUYETTOANs.SingleOrDefault(p => p.MADDK.Equals(ma));
        }

        public Message Update(QUYETTOAN objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK    );

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
                    

                    // Submit changes to db
                    _db.SubmitChanges();

                    UpdateChiPhiForQuyetToan(objUi.MADDK);

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
                        MATT = TTQT.QT_N.ToString(),
                        MOTA = "Cập nhật thông tin quyết toán"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Quyết toán ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Quyết toán ", objUi.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Quyết toán ", objUi.TENCT);
            }
            return msg;
        }
  
        public bool UpdateChiPhiForQuyetToan(string ma)
        {
            try
            {
                var chiettinh = _db.QUYETTOANs.SingleOrDefault(p => p.MADDK.Equals(ma));
                if (chiettinh == null)
                    return false;
                
                //gia tri co truoc trong chuong co so du lieu
                decimal hsnhancong = 0;
                decimal hschung = 0;
                decimal hsthunhap = 0;
                decimal hsthietke1 = 0;
                decimal hsthietke2 = 0;
                decimal hsthietke3 = 0;
                decimal hsthue = 0;
                decimal hscpc = 0;
                decimal giamgiacpvl = 0;
                decimal giamgiacpnc = 0;
                if (chiettinh.HSNHANCONG != null)
                {
                    hsnhancong = (decimal) chiettinh.HSNHANCONG;
                }
                if (chiettinh.HSCHUNG != null)
                {
                    hschung = (decimal)chiettinh.HSCHUNG;
                }
                if (chiettinh.HSTHUNHAP != null)
                {
                    hsthunhap = (decimal)chiettinh.HSTHUNHAP;
                }
                if (chiettinh.HSTHIETKE1 != null)
                {
                    hsthietke1 = (decimal) chiettinh.HSTHIETKE1;
                }
                if (chiettinh.HSTHIETKE2 != null)
                {
                    hsthietke2 = (decimal) chiettinh.HSTHIETKE2;
                }
                if (chiettinh.HSTHIETKE3 != null)
                {
                    hsthietke3 = (decimal) chiettinh.HSTHIETKE3;
                }
                if (chiettinh.HSTHUE != null)
                {
                    hsthue = (decimal) chiettinh.HSTHUE;
                }
                if (chiettinh.HSCPC != null)
                {
                    hscpc = (decimal)chiettinh.HSCPC;
                }

                //gia tri se duoc tinh thanh
                if (chiettinh.GIAMGIACPVL != null)
                {
                    giamgiacpvl = (decimal) chiettinh.GIAMGIACPVL ;
                }
                if (chiettinh.GIAMGIACPNC != null)
                {
                    giamgiacpnc = (decimal) chiettinh.GIAMGIACPNC ;
                }

                decimal cpvatlieu = 0;
                decimal cpnhancong = 0;
                decimal cpchung = 0;
                decimal cpthunhap = 0;
                decimal cpthietke = 0;
                decimal cpkhac = 0;


                decimal tienthue = 0;
                decimal tongTt = 0;
                decimal tongSt = 0;


                decimal tongcong = 0;
                decimal nhancong = 0;
                decimal cpmay = 0;
                decimal cpttvt = 0;
                decimal cpttmay = 0;
                decimal tcptt = 0;

                decimal cptthue = 0;
                decimal cpc = 0;
                decimal hspvl = 0;
                decimal hscpm = 0;
                decimal hsttp = 0;
                decimal cpkshs = 0;
                decimal ttp = 0;

                                        

                var ctchiettinhnd117List = _db.CTCHIETTINH_ND117s.Where(p => p.MADDK.Equals(ma)).ToList() ;
                foreach (CTCHIETTINH_ND117 ctchiettinhNd117 in ctchiettinhnd117List)
                {
                    if (ctchiettinhNd117.TIENNC != null)
                        cpnhancong = cpnhancong + (decimal) ctchiettinhNd117.TIENNC;

                    if (ctchiettinhNd117.TIENVT  != null)
                        cpvatlieu = cpvatlieu + (decimal)ctchiettinhNd117.TIENVT ;
                }

                //var daolapNd117S = _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(ma)).ToList();
                //foreach (DAOLAP_ND117 daolapNd117 in daolapNd117S)
                //{
                //    if(daolapNd117 .)
                //}
                //- chi phí vật liệu và nhân công)
                if( giamgiacpvl > 0)
                {
                    cpvatlieu = cpvatlieu - cpvatlieu*giamgiacpvl/100;
                }
                
                if(giamgiacpnc > 0)
                {
                    cpnhancong = cpnhancong - cpnhancong*giamgiacpnc/100;
                }

                cpvatlieu = Math.Round(cpvatlieu,0);

                cpnhancong = cpnhancong*hsnhancong*hsthietke3;
                cpc = (cpvatlieu + cpnhancong) * hscpc;
                cpchung = (cpvatlieu + cpnhancong + cpc) * hschung;
                cpthunhap = (cpvatlieu + cpnhancong + cpc + cpchung) * hsthunhap;
                tienthue = (cpvatlieu + cpnhancong + cpc + cpchung + cpthunhap) * hsthue / 100;
                cpthietke = (cpvatlieu + cpnhancong + cpc + cpchung + cpthunhap) * hsthietke1 * hsthietke2;
                tongTt = (cpvatlieu + cpnhancong + cpc + cpchung + cpthunhap);


                cpnhancong = Math.Round(cpnhancong/100, 0);
                cpnhancong = cpnhancong*100;

               
                cpc = Math.Round(cpc/100, 0);
                cpc = cpc*100;

               
                cpchung = Math.Round(cpchung/100, 0);
                cpchung = cpchung*100;             
                               
               
                cpthunhap = Math.Round(cpthunhap/100, 0);
                cpthunhap = cpthunhap*100;

               
                tienthue = Math.Round(tienthue/100, 0);
                tienthue = tienthue*100;

               
                cpthietke = Math.Round(cpthietke/100, 0);
                cpthietke = cpthietke*100;

                
                tongTt = Math.Round(tongTt/100, 0);
                tongTt = tongTt*100;

                tongSt = tienthue + cpthietke + tongTt;
                tongSt = Math.Round(tongSt, 0);


                chiettinh.CPNHANCONG = cpnhancong;
                chiettinh.CPVATLIEU = cpvatlieu;
                chiettinh.CPC = cpc;
                chiettinh.CPCHUNG = cpchung;
                chiettinh.CPTHUNHAP = cpthunhap;
                chiettinh.TIENTHUE = tienthue;
                chiettinh.CPTHIETKE = cpthietke;
                chiettinh.TONG_TT = tongTt;
                chiettinh.TONG_ST = tongSt;


                _db.SubmitChanges();


                return true;
            }catch
            {
                return false;
            }
        }
        
        public List<QUYETTOAN> GetListForDuyetQuyetToan(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            var result = _db.QUYETTOANs.Where(d => (d.DONDANGKY.TTHC==TTQT.QT_P.ToString() ||
                                                        d.DONDANGKY.TTHC==TTQT.QT_RA.ToString()) &&
                                                        (d.DONDANGKY.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.DONDANGKY.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.DONDANGKY.TTTC == TTTC.TC_A.ToString()) &&
                                                        d.DONDANGKY.THICONG.TTQT == true);
            if (keyword != null)
                result = result.Where(d => d.DONDANGKY.MADDK.Contains(keyword) ||
                                      d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                      d.DONDANGKY.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                      d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value <= toDate.Value);

            

            if (areaCode != null)
                result = result.Where(d => d.DONDANGKY.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d=>d.NGAYLCT).ToList();
        }
        
    }
}
