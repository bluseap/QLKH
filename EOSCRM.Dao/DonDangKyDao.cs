using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Data;

namespace EOSCRM.Dao
{
    public class DonDangKyDao
    {
        private readonly HopDongDao _hdDao = new HopDongDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly ThietKeDao _tkDao = new ThietKeDao();
        private readonly ChietTinhDao _ctDao = new ChietTinhDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly DuyetQuyenDao _dqDao = new DuyetQuyenDao();

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DonDangKyDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public int DemCMND(string cmnd, string makv)
        {
            return _db.DONDANGKies.Where(p => p.CMND.Equals(cmnd) && p.MAKV.Equals(makv)).Count();
        }

        public DONDANGKY Get(string ma)
        {
            return _db.DONDANGKies.Where(p => p.MADDK.Equals(ma)).SingleOrDefault();
        }

        public DONDANGKY Get2(string ma)
        {
            return _db.DONDANGKies.Where(p => p.MADDK.Equals(ma)).SingleOrDefault();
        }

        public List<DONDANGKY> GetListForTcdldm()
        {
            return _db.DONDANGKies.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListMa(string ma)
        {
            return _db.DONDANGKies.Where(p => p.MADDK.Equals(ma)).ToList();
        }

        public List<DONDANGKY> GetList()
        {
            return _db.DONDANGKies.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null))
                .OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public DONDANGKY GetListCMNV1(string macnmnd, string makv)
        {
            return _db.DONDANGKies.Where(d => d.CMND.Equals(macnmnd) && d.MAKV.Equals(makv)).SingleOrDefault();
        }

        public List<DONDANGKY> GetListCMNV2(string macnmnd, string makv)
        {
            return _db.DONDANGKies.Where(d => d.CMND.Equals(macnmnd) && d.MAKV.Equals(makv)).ToList();
        }

        public List<DONDANGKY> GetListCMNV(string macnmnd, string makv)
        {
            return _db.DONDANGKies.Where(d => d.CMND.Equals(macnmnd) && d.MAKV.Equals(makv)).ToList();
        }

        public List<DONDANGKY> GetListKV(String makv)
        {
            return _db.DONDANGKies.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null) && d.MAKV.Equals(makv))
                //.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
                .OrderByDescending(d => d.MADDK.Substring(3,8)).ToList();
        }

        public List<DONDANGKY> GetListHD(String maddk, String tenkh, String makv)
        {
            var query = _db.DONDANGKies.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDK.Contains(maddk));
            if (!String.IsNullOrEmpty(tenkh))
                query = query.Where(d => d.TENKH.Contains(tenkh));               

            return query.Where(d => d.MAKV.Equals(makv) && d.TTCT.Equals(TTCT.CT_A) && (d.TTHD.Equals(TTHD.HD_A)) )
                .OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            
        }
       
        public List<DONDANGKY> GetList(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong)
        {
            var query = _db.DONDANGKies.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null)).AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDK.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADP.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));


            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSD.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKV.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListMAPB(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong, String mapb)
        {
            var query = _db.DONDANGKies.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null) && d.MADDK.Substring(1,2)==mapb).AsQueryable();

            /*var query1 = from don in _db.DONDANGKies
                        join duyet in _db.DUYET_QUYENs on don.MADDK equals duyet.MADDK
                        where (don.TTDK.Equals(TTDK.DK_A) && (don.TTCT == null) && don.MADDK.Substring(1, 2).Equals(mapb))
                        select don;
            var query = query1.AsQueryable();*/
                
            /*
            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDK.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADP.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));


            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSD.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKV.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));
            */


            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.MADDK).ToList();
            return query.OrderByDescending(d => d.MADDK.Substring(3,8)).ToList();
        }


        public List<DONDANGKY> GetListForTcdldm(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong)
        {
            var query = _db.DONDANGKies.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDK.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADP.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));


            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSD.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKV.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListForTcdldmCM(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong, String cmnd)
        {
            var query = _db.DONDANGKies.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDK.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADP.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));


            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSD.Equals(mamdsd));

            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKV.Equals(makv));

            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                //query = query.Where(d => d.MAPHUONG.Equals(maphuong));
                query = query.Where(d => d.MADDK.Substring(1,2).Equals(maphuong));

            if (!String.IsNullOrEmpty(cmnd))
                query = query.Where(d => d.CMND.Contains(cmnd));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListForTcdldmPB(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, 
                        int? sohodn, int? sonk, int? dmnk, String mamdsd, String makv, String maphuong,String ma_PB, String socmnd)
        {
            //var query1 = _db.DONDANGKies.Where(p => p.MADDK.Substring(1, 2).Equals(ma_PB));

            //var query2 = from don in _db.DONDANGKies join duyet in _db.DUYET_QUYENs on don.MADDK equals duyet.MADDK
            //            where (don.MADDK.Substring(1,2).Equals(ma_PB) || duyet.MAPB.Equals(ma_PB) )                        
            //            select don;

            //var query = query1 != null ? query1.AsQueryable() : query2.AsQueryable();

            var query1 = from don in _db.DONDANGKies
                         join duyet in _db.DUYET_QUYENs on don.MADDK equals duyet.MADDK into sr
                         from x in sr.DefaultIfEmpty()
                         where (don.MADDK.Substring(1, 2).Equals(ma_PB) || x.MAPB.Equals(ma_PB))
                         select don;

            var query = query1.AsQueryable();            

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDK.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADP.Equals(madp));

            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));

            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSD.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKV.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (!String.IsNullOrEmpty(socmnd))
                query = query.Where(d => d.CMND.Contains(socmnd));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DONDANGKY> GetList(String keyword, DateTime? fromDate, DateTime? toDate, string makv, string maNV)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;

            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N))
                               && phuong.Contains(don.MAPHUONG))
                        select don;
            */

            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N)))
                        select don;

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));

            if (makv != "" && makv != "%")
                dsdon = dsdon.Where(d => d.MAKV == makv);

            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();

        }

        public List<DONDANGKY> GetListPB(String keyword, string maPb)
        {
           /*
            var dsdon = from don in _db.DONDANGKies 
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N)))
                        select don;
            */
            var dsdon = from don in _db.DONDANGKies 
                            join quyen in _db.DUYET_QUYENs on don.MADDK equals quyen.MADDK
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.TTTK.Equals(TTTK.TK_N)
                                && quyen.MAPB.Equals(maPb))                        
                        select don ;

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));

            return dsdon.ToList();

        }

        public List<DONDANGKY> GetListPBKV(String keyword, string maPb, string maKV)
        {
            /*
             var dsdon = from don in _db.DONDANGKies 
                         where (don.TTDK.Equals(TTDK.DK_A.ToString())
                                && (don.TTTK.Equals(TTTK.TK_N)))
                         select don;
             */
            var dsdon = from don in _db.DONDANGKies
                        join quyen in _db.DUYET_QUYENs on don.MADDK equals quyen.MADDK
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.TTTK.Equals(TTTK.TK_N)
                                && quyen.MAPB.Equals(maPb) && don.MAKV.Equals(maKV))
                        select don;

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));

            return dsdon.ToList();

         }        
        
        public List<DONDANGKY> GetDaiDienList(string maKV)
        {
            return _db.DONDANGKies.Where(d => (d.MAKV.Equals(maKV) && d.DAIDIEN.Equals(true))).ToList();
        }

        /// <summary>
        /// Tạo mới mã đơn
        /// </summary>
        /// <returns></returns>
        public string NewId()
        {
            string sReturn = "";

            string sToday = "";
            string nams = DateTime.Now.Year.ToString();
            sToday = nams.Substring(2, 2);
            sToday = DateTime.Now.Month < 10
                         ? sToday + "0" + DateTime.Now.Month.ToString()
                         : sToday + DateTime.Now.Month.ToString();

            //sToday = DateTime.Now.Day < 10
            //             ? sToday + "0" + DateTime.Now.Day.ToString()
            //             : sToday + DateTime.Now.Day.ToString();

            sReturn = (from p in _db.DONDANGKies.Where(p => p.MADDK.Substring(3, 8).Contains(sToday))
                       select p.MADDK.Substring(3, 8)).Max();

            if (!string.IsNullOrEmpty(sReturn))
            {
                int thutu = int.Parse(sReturn.Substring(4, 4));
                thutu = thutu + 1;
                if (thutu < 10)
                {
                    sReturn = sToday + "000" + thutu.ToString();
                }
                else if (thutu < 100)
                {
                    sReturn = sToday + "00" + thutu.ToString();
                }
                else if (thutu < 1000)
                {
                    sReturn = sToday + "0" + thutu.ToString();
                }
                else if (thutu < 10000)
                {
                    sReturn = sToday + thutu.ToString();
                }
            }
            else
            {
                sReturn = sToday + "0001";
            }

            return sReturn;
        }

        /// <summary>
        /// Lấy danh sách đơn đăng ký cần khảo sát thiết kế đi khảo sát thiết kế
        /// </summary>
        /// <param name="keyword">Từ khóa cần tìm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="maNV">Mã nhân viên phụ trách</param>
        /// <returns></returns>
        public List<DONDANGKY> GetListForKhaoSat(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string maNV)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;
            
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && don.LOAIDK.Equals(LOAIDK.DK.ToString())
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString()))
                               && phuong.Contains(don.MAPHUONG))
                        select don;
            */
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString())))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
            {
                if (stateCode == "NULL")
                    dsdon = dsdon.Where(d => d.TTTK.Equals(null));
                else
                    dsdon = dsdon.Where(d => d.TTTK.Equals(stateCode));
            }

            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListForKhaoSatKV(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string maNV, string maKV)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;
            
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && don.LOAIDK.Equals(LOAIDK.DK.ToString())
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString()))
                               && phuong.Contains(don.MAPHUONG))
                        select don;
            */
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.MAKV.Equals(maKV)
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString())))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
            {
                if (stateCode == "NULL")
                    dsdon = dsdon.Where(d => d.TTTK.Equals(null));
                else
                    dsdon = dsdon.Where(d => d.TTTK.Equals(stateCode));
            }

            //return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            return dsdon.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListKhaoSatKVTanChau(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string maNV, string maKV)
        {           
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.MAKV.Equals(maKV)
                                && (don.TTTK.Equals(TTTK.TK_A.ToString()) || don.TTTK.Equals(TTTK.TK_RA.ToString()))
                                && ((don.TTCT.Equals(null) || don.TTCT.Equals("CT_N") || don.TTCT.Equals("CT_RA")) ))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            //if (stateCode != null)
            //{
            //    if (stateCode == "NULL")
            //        dsdon = dsdon.Where(d => d.TTTK.Equals(null));
            //    else
            //        dsdon = dsdon.Where(d => d.TTTK.Equals(stateCode));
            //}

            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListForKhaoSatPB(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string maNV, String mapb)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;
            
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && don.LOAIDK.Equals(LOAIDK.DK.ToString())
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString()))
                               && phuong.Contains(don.MAPHUONG))
                        select don;
            */
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString()))
                               && don.MADDK.Substring(1, 2) == mapb
                               )
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
            {
                if (stateCode == "NULL")
                    dsdon = dsdon.Where(d => d.TTTK.Equals(null));
                else
                    dsdon = dsdon.Where(d => d.TTTK.Equals(stateCode));
            }

            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }
        /// <summary>
        /// Danh sách các đơn chờ bốc vật tư 
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maNV">Mã nhân viên</param>
        /// <returns></returns>
        public List<DONDANGKY> GetListForChoBocVatTu(String keyword, DateTime? fromDate, DateTime? toDate, string maNV)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;

            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N))
                               && phuong.Contains(don.MAPHUONG))
                        select don;
            */
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N)))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        /// <summary>
        /// Danh sách các đơn đang bốc vật tư 
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maNV">Mã nhân viên</param>
        /// <returns></returns>
        public List<THIETKE> GetListForBocVatTu(String keyword, DateTime? fromDate, DateTime? toDate, string maNV)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;

            var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P) && phuong.Contains(tk.DONDANGKY.MAPHUONG))
                        select don;
            */

            var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKY.TENKH.Contains(keyword) ||
                                         d.DONDANGKY.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKY.MADDK).ToList();
        }

        public List<THIETKE> GetListForBocVatTuKV(String keyword, DateTime? fromDate, DateTime? toDate, string maNV, string maKV)
        {           

            //var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P))
            //            select don;

            var dsdon = from tk in _db.THIETKEs join don in _db.DONDANGKies on tk.MADDK equals don.MADDK
                        orderby tk.MADDK descending
                        where (don.TTTK.Equals(TTTK.TK_P) && don.MAKV.Equals(maKV))
                        select tk;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKY.TENKH.Contains(keyword) ||
                                         d.DONDANGKY.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value <= toDate.Value);


            //return dsdon.OrderByDescending(d => d.DONDANGKY.MADDK).ToList();
            return dsdon.OrderByDescending(d => d.NGAYN).ToList();
        }

        public List<THIETKE> GetListForBocVatTuPB(String keyword, DateTime? fromDate, DateTime? toDate, string maNV, string maPB)
        {            

            /*var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P))
                        select don;*/

            var dsdon = from tk in _db.THIETKEs join duyet in _db.DUYET_QUYENs on tk.MADDK equals duyet.MADDK
                                join don in _db.DONDANGKies on tk.MADDK equals don.MADDK
                        orderby tk.MADDK descending
                        where (don.TTTK.Equals(TTTK.TK_P) && don.MADDK.Substring(1,2).Equals(maPB) || duyet.MAPB.Equals(maPB))
                        //where (don.TTTK.Equals(TTTK.TK_P) && don.MADDK.Substring(1, 2).Equals(maPB) && duyet.MAPB.Equals(maPB))
                        select tk;

            /*var dsdon = from tk in _db.THIETKEs
                        join duyet in _db.DUYET_QUYENs on tk.MADDK equals duyet.MADDK
                        join don in _db.DONDANGKies on tk.MADDK equals don.MADDK
                        where (don.TTTK.Equals(TTTK.TK_P) && duyet.MAPB.Equals(maPB))
                        select new
                        {
                            tk.MADDK,don.TENKH,tk.TENTK,don.DIACHILD,don.NGAYKS,tk.NGAYLTK
                            ,tk.CHUTHICH,tk.DONDANGKY

                        };*/

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKY.TENKH.Contains(keyword) ||
                                         d.DONDANGKY.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value <= toDate.Value);
            
            //return dsdon.OrderByDescending(d => d.DONDANGKY.MADDK).ToList();
            return dsdon.OrderByDescending(d => d.NGAYN).ToList();
            
        }
        

        /// <summary>
        /// Danh sách các đơn đã được bốc vật tư
        /// </summary>
        /// <param name="keyword">Từ khóa cần tìm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<THIETKE> GetListForTraCuuThietKe(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {

            var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P) || tk.DONDANGKY.TTTK.Equals(TTTK.TK_A))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKY.TENKH.Contains(keyword) ||
                                         d.DONDANGKY.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKY.MADDK).ToList();
        }

        public List<THIETKE> GetListForTraCuuThietKePB(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, String maPB)
        {          

            var dsdon = from tk in _db.THIETKEs join don in _db.DONDANGKies on tk.MADDK equals don.MADDK
                            join duyet in _db.DUYET_QUYENs on tk.MADDK equals duyet.MADDK
                        where((don.TTTK.Equals(TTTK.TK_P) || don.TTTK.Equals(TTTK.TK_A))
                            && duyet.MAPB.Equals(maPB))
                        select tk;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKY.TENKH.Contains(keyword) ||
                                         d.DONDANGKY.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKY.MADDK).ToList();
        }

        public List<THIETKE> GetListForTraCuuThietKeKV(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, String maKV)
        {
            var dsdon = from tk in _db.THIETKEs
                        join don in _db.DONDANGKies on tk.MADDK equals don.MADDK                        
                        where ((don.TTTK.Equals(TTTK.TK_P) || don.TTTK.Equals(TTTK.TK_A))
                            && don.MAKV.Equals(maKV))
                        select tk;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKY.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKY.TENKH.Contains(keyword) ||
                                         d.DONDANGKY.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKY.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKY.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKY.NGAYDK.HasValue
                                           && d.DONDANGKY.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKY.MADDK).ToList();
        }
        
        public List<DUYETTHIETKE> GetListForDuyetThietKe(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETTHIETKEs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString()));
            //from don in _db.DONDANGKies join duyet in _db.DUYET_QUYENs on don.MADDK equals duyet.MADDK

            //var result = from d in _db.DUYETTHIETKEs join duyet in _db.DUYET_QUYENs on d.MADDK equals duyet.MADDK
            //             where(d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) && (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString())                              
            //             select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
                result = result.Where(d => d.TTTK == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDK.Substring(3,8))
                        .ToList();
        }

        public List<DUYETTHIETKE> GetListForDuyetThietKeBravo(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {

            var result = _db.DUYETTHIETKEs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode)
                        && (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString()
                            || (d.TTTK == TTTK.TK_A.ToString() && (d.TTCT == null || d.TTCT == "CT_N" || d.TTCT == "CT_RA")))
                        ); 
            
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            //if (stateCode != null)
            //    result = result.Where(d => d.TTTK == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDK.Substring(3, 8))
                        .ToList();
        }

        public List<DUYETTHIETKE> GetListForDuyetThietKePB(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, string mapb)
        {

            var result = from d in _db.DUYETTHIETKEs
                         join duyet in _db.DUYET_QUYENs on d.MADDK equals duyet.MADDK
                         where (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                             (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString())
                             && duyet.MAPB.Equals(mapb)
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
                result = result.Where(d => d.TTTK == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDK.Substring(3, 8))
                        .ToList();
        }

        public List<DUYETTHIETKE> GetListForDuyetThietKePBBravo(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, string mapb)
        {
            var result = from d in _db.DUYETTHIETKEs
                         join duyet in _db.DUYET_QUYENs on d.MADDK equals duyet.MADDK
                         where (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode)
                            && (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString()
                                || (d.TTTK == TTTK.TK_A.ToString() && (d.TTCT == null || d.TTCT == "CT_N" || d.TTCT == "CT_RA"))    )
                            //&& (d.TTTK == TTTK.TK_A.ToString() && (d.TTCT == null || d.TTCT == "CT_N")) 
                            && duyet.MAPB.Equals(mapb)
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            //if (stateCode != null)
            //    result = result.Where(d => d.TTTK == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDK.Substring(3, 8))
                        .ToList();
        }
        
        /// <summary>
        /// Lấy danh sách chờ duyệt chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa chờ duyệt</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<DUYETCHIETTINH> GetListForDuyetChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        ((d.TTCT == TTCT.CT_P.ToString()) || (d.TTCT == TTCT.CT_RA.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value <= toDate.Value);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d=>d.NGAYLCT).ToList();
        }

        public List<DUYETCHIETTINH> GetListForDuyetChietTinhKV(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        ((d.TTCT == TTCT.CT_P.ToString()) || (d.TTCT == TTCT.CT_RA.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value <= toDate.Value);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYLCT).ToList();
        }

        public List<DUYETCHIETTINH> GetListForDuyetCTKVTuNgayLX(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        ((d.TTCT == TTCT.CT_P.ToString()) || (d.TTCT == TTCT.CT_RA.ToString())));
            //var result = from dct in _db.DUYETCHIETTINHs 
            //             join ct in _db.CHIETTINHs on dct.MADDK equals ct.MADDK
            //             where ct.NGAY

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value <= toDate.Value);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYLCT).ToList();
        }

        /*
        /// <summary>
        /// Lấy danh sách đơn chờ lập chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa cần lọc</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<DUYETTHIETKE> GetListForLapChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETTHIETKEs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT != null)).AsQueryable();

            
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                           d.MADDKTONG.Contains(keyword) ||
                                           d.TENKH.Contains(keyword) ||
                                           d.TENTK.Contains(keyword) ||
                                           d.DIACHILD.Contains(keyword) ||
                                           d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }
        */

        /// <summary>
        /// Lấy danh sách đơn chờ lập chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa cần lọc</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<DONDANGKY> GetListForLapChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString()) && (d.TTHD == null)).AsQueryable();

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                           d.TENKH.Contains(keyword) ||
                                           d.DIACHILD.Contains(keyword) ||
                                           d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListForLapChietTinhKV(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString()))// && (d.TTHD == null))
                                                        .AsQueryable();

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                           d.TENKH.Contains(keyword) ||
                                           d.DIACHILD.Contains(keyword) ||
                                           d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListForLapChietTinhKVCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString())).AsQueryable();

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                           d.TENKH.Contains(keyword) ||
                                           d.DIACHILD.Contains(keyword) ||
                                           d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DUYETCHIETTINH> GetListForTraCuuChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString() || d.TTCT == TTCT.CT_P.ToString() ||
                                                        d.TTCT == TTCT.CT_RA.ToString() || d.TTCT == TTCT.CT_A.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYLCT).ToList();
        }

        public List<DUYETCHIETTINH> GetListForTraCuuChietTinhKV(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString() || d.TTCT == TTCT.CT_P.ToString() ||
                                                        d.TTCT == TTCT.CT_RA.ToString() || d.TTCT == TTCT.CT_A.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYLCT).ToList();
        }

        public List<DUYETCHIETTINH> GetListForTraCuuCTKVTuNgay(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKV.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString() || d.TTCT == TTCT.CT_P.ToString() ||
                                                        d.TTCT == TTCT.CT_RA.ToString() || d.TTCT == TTCT.CT_A.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYN.HasValue
                                           && d.NGAYN.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYN.HasValue
                                           && d.NGAYN.Value <= toDate.Value.AddDays(1));
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYLCT).ToList();
        }
        
        public List<DUYETQUYETTOAN> GetListForTraCuuQuyetToan(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETQUYETTOANs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTTC == TTTC.TC_A.ToString()) &&
                                                        (d.TTHC == TTQT.QT_A.ToString() || d.TTHC == TTQT.QT_P.ToString() ||
                                                        d.TTHC == TTQT.QT_RA.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            if (stateCode != null)
                result = result.Where(d => d.TTHC == stateCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYLCT).ToList();
        }

        /// <summary>
        /// Lấy danh sách xử lý đơn chờ hợp đồng
        /// </summary>
        /// <param name="keyword">Từ khóa cần tìm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<DONDANGKY> GetListForXldchd(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {

            var dsdon = from don in _db.DONDANGKies
                        where (don.TTCT.Equals(TTCT.CT_A.ToString())
                               && don.LOAIDK.Equals(LOAIDK.DK.ToString())
                               && (don.TTHD.Equals(null) || don.TTHD.Equals(TTHD.HD_RA.ToString())))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
            {
                if (stateCode != "NULL")
                    dsdon = dsdon.Where(d => d.TTHD.Equals(stateCode));
            }

            if (areaCode != null)
                dsdon = dsdon.Where(d => d.MAKV == areaCode);

            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DONDANGKY> GetListForDonChoHopdong(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            var dsdon = from don in _db.DONDANGKies
                        where (don.TTCT.Equals(TTCT.CT_A.ToString())
                               && don.TTHD.Equals(TTHD.HD_N.ToString()))
                        select don; 

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
            {
                if (stateCode != "NULL")
                    dsdon = dsdon.Where(d => d.TTHD.Equals(stateCode));
            }

            if (!string.IsNullOrEmpty(areaCode) && areaCode != "%")
                dsdon = dsdon.Where(d => d.MAKV == areaCode);

            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKY> GetListDaDuyetTK_CD(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            var dsdon = from don in _db.DONDANGKies
                        //where (don.TTCT.Equals(TTCT.CT_A.ToString()) && don.TTHD.Equals(TTHD.HD_N.ToString()))
                        where (don.TTTK.Equals(TTTK.TK_A.ToString()) && (don.TTHD.Equals(TTHD.HD_N.ToString()) || don.TTHD == null))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);

            if (stateCode != null)
            {
                if (stateCode != "NULL")
                    dsdon = dsdon.Where(d => d.TTHD.Equals(stateCode));
            }

            if (!string.IsNullOrEmpty(areaCode) && areaCode != "%")
                dsdon = dsdon.Where(d => d.MAKV == areaCode);

            return dsdon.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DONDANGKY> GetListForXuLyDonChoThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(null)));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DONDANGKY> GetListForLapQuyetToan(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTTC == TTTC.TC_A.ToString()) &&
                                                        (d.THICONG.TTQT == true) &&
                                                        (d.TTHC == null || d.TTHC == TTQT.QT_N.ToString()) &&
                                                        d.QUYETTOAN != null);
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null)
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public Message Insert(DONDANGKY objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.DONDANGKies.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                                        {
                                            MADON = objUi.MADDK,
                                            IPAddress = ipAddress,
                                            MANV = sManv,
                                            UserAgent = useragent,
                                            NGAYTHUCHIEN = DateTime.Now,
                                            TACVU = TACVUKYDUYET.I.ToString(),
                                            MACN = CHUCNANGKYDUYET.KH01.ToString(),
                                            MATT = TTDK.DK_A.ToString(),
                                            MOTA = "Nhập đơn lắp mới"
                                        };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn đăng ký");
            }
            catch (Exception ex)
            {
                // rollback transaction
                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "đơn đăng ký");
            }
            return msg;
        }

        public Message UpdateTT(DONDANGKY objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                    objDb.TTTK = objUi.TTTK;

                    // Submit changes to db
                    _db.SubmitChanges();
                }
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn đăng ký");
            }
            catch (Exception ex)
            {
                // rollback transaction
                _db.Connection.Close();
                msg = ExceptionHandler.HandleInsertException(ex, "đơn đăng ký");
            }
            return msg;
        }

        public Message UpdateNhapHDTC(DONDANGKY objUi)
        {
            Message msg;
            try
            {
                
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;

                    // Submit changes to db
                   // _db.SubmitChanges();
                }
                _db.Connection.Close();
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn đăng ký");                
            }
            catch (Exception ex)
            {
                // rollback transaction
                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "đơn đăng ký");
            }
            return msg;
        }

        public Message UpdateNhapHDTCMoi(string maddk, string makv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var objDb = Get(maddk);

                if (objDb != null)
                {
                    objDb.TTHD = "HD_A";
                    objDb.TTTC = "TC_N";
                }

                _db.SubmitChanges();

                _db.Connection.Close();
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn đăng ký");
            }
            catch (Exception ex)
            {
                // rollback transaction
                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "đơn đăng ký");
            }
            return msg;
        }

        public Message UpdateQT_A(DONDANGKY objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    objDb.TTHC = objUi.TTHC;

                    // Submit changes to db
                    _db.SubmitChanges();
                }
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "đơn đăng ký");         
            }
            catch (Exception ex)
            {
                // rollback transaction
                _db.Connection.Close();
                msg = ExceptionHandler.HandleInsertException(ex, "đơn đăng ký");
            }
            return msg;
        }

        public Message Update(DONDANGKY objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDK = objUi.MADDK;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;

                    objDb.TENDK = objUi.TENDK;

                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADP = objUi.MADP;
                    objDb.MAKV = objUi.MAKV;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSD = objUi.MAMDSD;
                    if (objUi.NGAYDK.HasValue)
                        objDb.NGAYDK = objUi.NGAYDK.Value;
                    if (objUi.NGAYHKS.HasValue)
                        objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    objDb.NGAYSINH = objUi.NGAYSINH;
                    if (objUi.CAPNGAY.HasValue)
                        objDb.CAPNGAY = objUi.CAPNGAY.Value;
                    objDb.TAI = objUi.TAI;
                    objDb.MAHTTT = objUi.MAHTTT;

                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                    objDb.TTTK = objUi.TTTK;

                    objDb.LOAIDK = objUi.LOAIDK;
                    objDb.TENDK = objUi.TENDK;
                    objDb.DACBIET = objUi.DACBIET;
                    if (objUi.NGAYCD.HasValue)
                        objDb.NGAYCD = objUi.NGAYCD;
                    if (objUi.NGAYKS.HasValue)
                        objDb.NGAYKS = objUi.NGAYKS;
                    objDb.DTNGOAI = objUi.DTNGOAI;
                    objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                    objDb.BATAICHO = objUi.BATAICHO;
                    objDb.CTCTMOI = objUi.CTCTMOI;
                    objDb.MAPB = objUi.MAPB;
                    objDb.MANV = objUi.MANV;
                    objDb.SOHODN = objUi.SOHODN;
                    objDb.SONK = objUi.SONK;
                    objDb.DMNK = objUi.DMNK;
                    objDb.DAIDIEN = objUi.DAIDIEN;
                    
                    objDb.NOIDUNG = objUi.NOIDUNG;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;
                    objDb.DIACHILD = objUi.DIACHILD;

                    if (objUi.NOILAPDHHN != null)
                    { objDb.NOILAPDHHN = objUi.NOILAPDHHN; }
                    else { objDb.NOILAPDHHN = ""; }

                    //ho ngheo
                    objDb.DIACHINGHEO = objUi.DIACHINGHEO;
                    objDb.ISHONGHEO = objUi.ISHONGHEO;
                    objDb.DONVICAPHN = objUi.DONVICAPHN;
                    objDb.MAHN = objUi.MAHN;
                    objDb.NGAYCAPHN = objUi.NGAYCAPHN;
                    objDb.NGAYKETTHUCHN = objUi.NGAYKETTHUCHN;
                    objDb.NGAYKYHN = objUi.NGAYKYHN;

                    objDb.SONHA2 = objUi.SONHA2;
                    objDb.MADPKHGAN = objUi.MADPKHGAN;

                    var hopdong = _hdDao.Get(objUi.MADDK);
                    if (hopdong != null)
                    {
                        hopdong.MAMDSD = objUi.MAMDSD;
                    }

                    objDb.TIENCOCLX = objUi.TIENCOCLX;
                    objDb.TIENVATTULX = objUi.TIENVATTULX;

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
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Sửa thông tin đơn lắp mới"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đơn đăng ký");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đơn đăng ký của khách hàng", objUi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đơn đăng ký");
            }
            return msg;
        }

        public Message UpdateMDSD(DONDANGKY objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDK = objUi.MADDK;

                    objDb.MAMDSD = objUi.MAMDSD;

                    //objDb.MADDKTONG = objUi.MADDKTONG;
                    //objDb.TENKH = objUi.TENKH;
                    //objDb.TENDK = objUi.TENDK;
                    //objDb.DIACHILD = objUi.DIACHILD;
                    //objDb.SONHA = objUi.SONHA;
                    //objDb.DIENTHOAI = objUi.DIENTHOAI;
                    //objDb.MAPHUONG = objUi.MAPHUONG;
                    //objDb.DUONGPHU = objUi.DUONGPHU;
                    //objDb.MADP = objUi.MADP;
                    //objDb.MAKV = objUi.MAKV;
                    //objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    
                    //if (objUi.NGAYDK.HasValue)
                    //    objDb.NGAYDK = objUi.NGAYDK.Value;
                    //if (objUi.NGAYHKS.HasValue)
                    //    objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    //objDb.NGAYSINH = objUi.NGAYSINH;
                    //if (objUi.CAPNGAY.HasValue)
                    //    objDb.CAPNGAY = objUi.CAPNGAY.Value;
                    //objDb.TAI = objUi.TAI;
                    //objDb.MAHTTT = objUi.MAHTTT;
                    //objDb.TTCT = objUi.TTCT;
                    //objDb.TTDK = objUi.TTDK;
                    //objDb.TTHC = objUi.TTHC;
                    //objDb.TTHD = objUi.TTHD;
                    //objDb.TTTC = objUi.TTTC;
                    //objDb.TTTK = objUi.TTTK;
                    //objDb.LOAIDK = objUi.LOAIDK;
                    //objDb.TENDK = objUi.TENDK;
                    //objDb.DACBIET = objUi.DACBIET;
                    //if (objUi.NGAYCD.HasValue)
                    //    objDb.NGAYCD = objUi.NGAYCD;
                    //if (objUi.NGAYKS.HasValue)
                    //    objDb.NGAYKS = objUi.NGAYKS;
                    //objDb.DTNGOAI = objUi.DTNGOAI;
                    //objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                    //objDb.BATAICHO = objUi.BATAICHO;
                    //objDb.CTCTMOI = objUi.CTCTMOI;
                    //objDb.MAPB = objUi.MAPB;
                    //objDb.MANV = objUi.MANV;
                    //objDb.SOHODN = objUi.SOHODN;
                    //objDb.SONK = objUi.SONK;
                    //objDb.DMNK = objUi.DMNK;
                    //objDb.DAIDIEN = objUi.DAIDIEN;
                    ////objDb.NOIDUNG = objUi.NOIDUNG;
                    //objDb.CMND = objUi.CMND;
                    //objDb.MST = objUi.MST;
                    //objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    //objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    //objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;
                    //objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;
                    //objDb.DIACHILD = objUi.DIACHILD;
                    //if (objUi.NOILAPDHHN != null)
                    //{ objDb.NOILAPDHHN = objUi.NOILAPDHHN; }
                    //else { objDb.NOILAPDHHN = ""; }
                    ////ho ngheo
                    //objDb.DIACHINGHEO = objUi.DIACHINGHEO;
                    //objDb.ISHONGHEO = objUi.ISHONGHEO;
                    //objDb.DONVICAPHN = objUi.DONVICAPHN;
                    //objDb.MAHN = objUi.MAHN;
                    //objDb.NGAYCAPHN = objUi.NGAYCAPHN;
                    //objDb.NGAYKETTHUCHN = objUi.NGAYKETTHUCHN;
                    //objDb.NGAYKYHN = objUi.NGAYKYHN;
                    //objDb.SONHA2 = objUi.SONHA2;
                    //objDb.MADPKHGAN = objUi.MADPKHGAN;

                    var hopdong = _hdDao.Get(objUi.MADDK);
                    if (hopdong != null)
                    {
                        hopdong.MAMDSD = objUi.MAMDSD;
                    }

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
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Sửa thông tin đơn lắp mới"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đơn đăng ký");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đơn đăng ký của khách hàng", objUi.TENKH);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "đơn đăng ký");
            }
            return msg;
        } 

        public void UpdateTuChoiTK(DONDANGKY objUi, String useragent, String ipAddress, String sManv)
        {           
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK);

                if (objDb != null)
                {
                    //TODO: update all fields
                    
                    objDb.TTTK = objUi.TTTK;                    
                    objDb.NOIDUNG = objUi.NOIDUNG;                   

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
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Sửa thông tin đơn lắp mới"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                   // msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "đơn đăng ký");
                }
                else
                {
                    // error message
                    //msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "đơn đăng ký của khách hàng", objUi.TENKH);
                }
            }
            catch (Exception ex)
            {
                //msg = ExceptionHandler.HandleUpdateException(ex, "đơn đăng ký");
            }           
        }

        public bool IsInUse(string ma)
        {
            if (_db.DONDANGKies.Where(p => p.MADDKTONG.Equals(ma)).Count() > 0)
                return true;
            if (_db.KHACHHANGs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
                return true;
            if (_db.HOADONLAPDATs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
                return true;
            if (_db.THIETKEs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
                return true;
            if (_db.CHIETTINHs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
                return true;
            if (_db.QUYETTOANs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
                return true;
            if (_db.HOPDONGs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
                return true;

            return false;
        }

        public Message Delete(DONDANGKY objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký ", objUi.TENKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DONDANGKies.DeleteOnSubmit(objDb);
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
                    MATT = TTDK.DK_A.ToString(),
                    MOTA = "Xóa đơn lắp mới"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đơn đăng ký ");
            }
            catch (Exception ex)
            {
                _db.Connection.Close();

                msg = ExceptionHandler.HandleDeleteException(ex, "Đơn đăng ký ");
            }

            return msg;
        }

        public Message UpdateDiaChi(List<DONDANGKY> objList)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.DIACHILD = objUi.DIACHILD;
                    
                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, objList.Count.ToString(),
                                  " đơn đăng ký");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách đơn đăng ký", ex.Message);
            }

            return msg;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="useragent"></param>
        /// <param name="ipAddress"></param>
        /// <param name="sManv"></param>
        /// <returns></returns>
        public Message UpdateList(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDK = objUi.MADDK;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADP = objUi.MADP;
                    objDb.MAKV = objUi.MAKV;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSD = objUi.MAMDSD;
                    if (objUi.NGAYDK.HasValue)
                        objDb.NGAYDK = objUi.NGAYDK.Value;
                    if (objUi.NGAYHKS.HasValue)
                        objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                    objDb.TTTK = objUi.TTTK;

                    objDb.LOAIDK = objUi.LOAIDK;
                    objDb.TENDK = objUi.TENDK;
                    objDb.DACBIET = objUi.DACBIET;
                    if (objUi.NGAYCD.HasValue)
                        objDb.NGAYCD = objUi.NGAYCD;
                    if (objUi.NGAYKS.HasValue)
                        objDb.NGAYKS = objUi.NGAYKS;
                    objDb.DTNGOAI = objUi.DTNGOAI;
                    objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                    objDb.BATAICHO = objUi.BATAICHO;
                    objDb.CTCTMOI = objUi.CTCTMOI;
                    objDb.MAPB = objUi.MAPB;
                    objDb.MANV = objUi.MANV;
                    objDb.SOHODN = objUi.SOHODN;
                    objDb.SONK = objUi.SONK;
                    objDb.DMNK = objUi.DMNK;
                    objDb.DAIDIEN = objUi.DAIDIEN;
                    //objDb.NOIDUNG = objUi.NOIDUNG;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                                            {
                                                MADON = objUi.MADDK,
                                                IPAddress = ipAddress,
                                                MANV = sManv,
                                                UserAgent = useragent,
                                                NGAYTHUCHIEN = DateTime.Now,
                                                TACVU = TACVUKYDUYET.U.ToString(),
                                                MACN = CHUCNANGKYDUYET.KH01.ToString(),
                                                MATT = TTDK.DK_A.ToString(),
                                                MOTA = "Duyệt đơn thông tin đơn lắp mới"
                                            };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                   

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, objList.Count.ToString(),
                                  " đơn đăng ký");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách đơn đăng ký", ex.Message);
            }

            return msg;
        }

        public Message UpdateListDuyetQuyen(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, String mapB)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDK = objUi.MADDK;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADP = objUi.MADP;
                    objDb.MAKV = objUi.MAKV;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSD = objUi.MAMDSD;
                    if (objUi.NGAYDK.HasValue)
                        objDb.NGAYDK = objUi.NGAYDK.Value;
                    if (objUi.NGAYHKS.HasValue)
                        objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                    objDb.TTTK = objUi.TTTK;

                    objDb.LOAIDK = objUi.LOAIDK;
                    objDb.TENDK = objUi.TENDK;
                    objDb.DACBIET = objUi.DACBIET;
                    if (objUi.NGAYCD.HasValue)
                        objDb.NGAYCD = objUi.NGAYCD;
                    if (objUi.NGAYKS.HasValue)
                        objDb.NGAYKS = objUi.NGAYKS;
                    objDb.DTNGOAI = objUi.DTNGOAI;
                    objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                    objDb.BATAICHO = objUi.BATAICHO;
                    objDb.CTCTMOI = objUi.CTCTMOI;
                    objDb.MAPB = objUi.MAPB;
                    objDb.MANV = objUi.MANV;
                    objDb.SOHODN = objUi.SOHODN;
                    objDb.SONK = objUi.SONK;
                    objDb.DMNK = objUi.DMNK;
                    objDb.DAIDIEN = objUi.DAIDIEN;
                    //objDb.NOIDUNG = objUi.NOIDUNG;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Duyệt đơn thông tin đơn lắp mới"
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    var duyetquyen = new DUYET_QUYEN()
                    {
                        MADDK = objUi.MADDK,
                        MANV = sManv,
                        MAPB = mapB,
                        MAKV = objUi.MAKV,
                        NGAY_PQ = DateTime.Now
                    };
                    _db.DUYET_QUYENs.InsertOnSubmit(duyetquyen);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, objList.Count.ToString(),
                                  " đơn đăng ký");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách đơn đăng ký", ex.Message);
            }

            return msg;
        }

        public Message UpdateListDuyetQuyen2(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, String mapB)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDK = objUi.MADDK;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADP = objUi.MADP;
                    objDb.MAKV = objUi.MAKV;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSD = objUi.MAMDSD;
                    if (objUi.NGAYDK.HasValue)
                        objDb.NGAYDK = objUi.NGAYDK.Value;
                    if (objUi.NGAYHKS.HasValue)
                        objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                   // objDb.TTTK = TTTK.TK_N.ToString();//thiet ke moi
                    objDb.TTTK = _dqDao.Get(objUi.MADDK) != null ? TTTK.TK_P.ToString() : TTTK.TK_N.ToString();

                    //objDb.NGAYNHAPHSTRA = (_dqDao.Get(objUi.MADDK) != null && objUi.ISTRAHS.Equals(1)) ? DateTime.Now : objUi.NGAYNHAPHSTRA; //DateTimeUtil.GetVietNamDate("1111/11/11");
                    //objDb.NGAYNHAPHSTRA = DateTime.Now;
                    objDb.NGAYDUYETHS = DateTime.Now;

                    objDb.LOAIDK = objUi.LOAIDK;
                    objDb.TENDK = objUi.TENDK;
                    objDb.DACBIET = objUi.DACBIET;
                    if (objUi.NGAYCD.HasValue)
                        objDb.NGAYCD = objUi.NGAYCD;
                    if (objUi.NGAYKS.HasValue)
                        objDb.NGAYKS = objUi.NGAYKS;
                    objDb.DTNGOAI = objUi.DTNGOAI;
                    objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                    objDb.BATAICHO = objUi.BATAICHO;
                    objDb.CTCTMOI = objUi.CTCTMOI;
                    objDb.MAPB = objUi.MAPB;
                    objDb.MANV = objUi.MANV;
                    objDb.SOHODN = objUi.SOHODN;
                    objDb.SONK = objUi.SONK;
                    objDb.DMNK = objUi.DMNK;
                    objDb.DAIDIEN = objUi.DAIDIEN;
                    //objDb.NOIDUNG = objUi.NOIDUNG;
                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;
                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;                    

                    if (objUi.ISTRAHS != true)
                    {
                        _rpClass.HisNgayDangKyBien(objUi.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "DUYETDONDK");

                        var duyetquyen = new DUYET_QUYEN()
                        {
                            MADDK = objUi.MADDK,
                            MANV = sManv,
                            MAPB = mapB,
                            MAKV = objUi.MAKV,
                            NGAY_PQ = DateTime.Now
                        };
                        _db.DUYET_QUYENs.InsertOnSubmit(duyetquyen);
                    }
                    else
                    {
                        objUi.ISTRAHS = false;
                        objDb.NGAYDUYETHSTRA = DateTime.Now;

                        _rpClass.HisNgayDangKyBien(objUi.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                       "", "", "", "", "DUYETTRADONDK");
                    }                    

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Duyệt đơn thông tin đơn lắp mới"
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);                    

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, objList.Count.ToString(), " đơn đăng ký");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách đơn đăng ký", ex.Message);
            }

            return msg;
        }
        
        public Message UpdateListTuChoi(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDK = objUi.MADDK;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHU = objUi.DUONGPHU;
                    objDb.MADP = objUi.MADP;
                    objDb.MAKV = objUi.MAKV;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSD = objUi.MAMDSD;
                    if (objUi.NGAYDK.HasValue)
                        objDb.NGAYDK = objUi.NGAYDK.Value;
                    if (objUi.NGAYHKS.HasValue)
                        objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                    objDb.TTTK = objUi.TTTK;
                    objDb.NGAYDUYETHS = DateTime.Now;

                    objDb.LOAIDK = objUi.LOAIDK;
                    objDb.TENDK = objUi.TENDK;
                    objDb.DACBIET = objUi.DACBIET;
                    if (objUi.NGAYCD.HasValue)
                        objDb.NGAYCD = objUi.NGAYCD;
                    if (objUi.NGAYKS.HasValue)
                        objDb.NGAYKS = objUi.NGAYKS;
                    objDb.DTNGOAI = objUi.DTNGOAI;
                    objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                    objDb.BATAICHO = objUi.BATAICHO;
                    objDb.CTCTMOI = objUi.CTCTMOI;
                    objDb.MAPB = objUi.MAPB;
                    objDb.MANV = objUi.MANV;
                    objDb.SOHODN = objUi.SOHODN;
                    objDb.SONK = objUi.SONK;
                    objDb.DMNK = objUi.DMNK;
                    objDb.DAIDIEN = objUi.DAIDIEN;
                    objDb.NOIDUNG = objDb.NOIDUNG + " " + objUi.NOIDUNG;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;

                    _rpClass.HisNgayDangKyBien(objUi.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOIDONDK");

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Từ chối đơn lắp mới."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, objList.Count.ToString(), " đơn đăng ký");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "danh sách đơn đăng ký", ex.Message);
            }

            return msg;
        }

        public Message ApproveThietKeListCD(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTCT = TTCT.CT_N.ToString();
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = tk.NGAYLTK;
                    tk.NGAYDUYETN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_A",
                        MOTA = @"Duyệt thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message ApproveChoThietKeListCD(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, string noidung)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objUi.TENKH);
                        return msg;
                    }

                    //tk.MANVDTK = sManv;
                    //tk.NGAYDTK = tk.NGAYLTK;
                    //tk.NGAYDUYETN = DateTime.Now;
                    tk.CHUTHICH = noidung;
                    tk.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_P",
                        MOTA = @"Chờ duyệt thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "Chờ duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message ApproveThietKeList(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",  objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTCT = TTCT.CT_N.ToString();
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if(tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",  objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;
                    tk.NGAYDUYETN = DateTime.Now;                    

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_A",
                        MOTA = @"Duyệt thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message ApproveThietKeListTKToCT(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();
                        objDb.TTCT = TTCT.CT_P.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTTC == "TC_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();
                        //objDb.TTTC = "TC_P";

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();
                        objDb.TTNT = "NT_A";

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTTK = TTTK.TK_A.ToString();
                        objDb.TTCT = TTCT.CT_N.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;
                    tk.NGAYDUYETN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_A",
                        MOTA = @"Duyệt thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message ApproveThietKeListTanChau(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        //_rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        //_rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTTK = TTTK.TK_A.ToString();

                        //_rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                   // objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;
                    tk.NGAYDUYETN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_A",
                        MOTA = @"Duyệt thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message ApproveThietKeListLaiChuaChietTinh(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_N" || objDb.TTCT == "CT_RA" || objDb.TTCT == null)
                    {
                        objDb.TTTK = TTTK.TK_P.ToString();
                        objDb.TTCT = TTCT.CT_RA.ToString();
                    }                   

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    // objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objUi.TENKH);
                        return msg;
                    }

                    //tk.MANVDTK = sManv;
                    //tk.NGAYDTK = ngayduyet;
                    //tk.NGAYDUYETN = DateTime.Now;

                    //_rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                    //        "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_P",
                        MOTA = @"Trả về thiết kế lại."
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message AppDuyetDonThietKeTanChau(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objUi.TENKH);
                        return msg;
                    }

                    //tk.MANVDTK = sManv;
                    //tk.NGAYDTK = ngayduyet;
                    //tk.NGAYDUYETN = DateTime.Now;

                    tk.MANVDUYETKD = sManv;
                    tk.NGAYDUYETKD = ngayduyet;
                    tk.NGAYDUYETKDN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETDONTHIETKETC");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_A",
                        MOTA = @"Duyệt đơn, thiết kế (TC)."
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt đơn, thiết kế", ex.Message);
            }

            return msg;
        }

        public Message ApproveThietKeListDuyetCT(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {                       
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTCT = TTCT.CT_N.ToString();
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDK, objDb.MAKV, "", "", "", "", "DONTKTOCT");
                    }                   

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",  objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;
                    //tk.NGAYDUYETN = DateTime.Now;
                    //tk.NGAYTRAKH = tk.ISTRAKEHOACH.Equals(1) ? DateTime.Now : tk.NGAYTRAKH;
                    //tk.NGAYTRATC = tk.ISTRATHICONG.Equals(1) ? DateTime.Now : tk.NGAYTRATC;

                    if (tk.ISTRAKEHOACH == true)
                    {
                        tk.ISTRATHICONG = false;
                        tk.NGAYTRAKH = DateTime.Now;
                        
                        objDb.TTCT = TTCT.CT_P.ToString();

                        _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTRAHSTKKH");
                    }
                    else
                    {
                        if (tk.ISTRATHICONG == true)
                        {
                            tk.ISTRAKEHOACH = false;
                            tk.NGAYTRATC = DateTime.Now;

                            // update thiet ke
                            var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                            if (ct == null)
                            {
                                msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính", objUi.TENKH);
                                return msg;
                            }

                            ct.ISTRAHSTC = true; //chiet tinh

                            objDb.TTCT = TTCT.CT_P.ToString();                            

                            _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                                "", "", "", "", "DUYETTRAHSTKKHTC");
                        }
                        else
                        {
                            tk.ISTRAKEHOACH = false;
                            tk.ISTRATHICONG = false;

                            tk.NGAYDUYETN = DateTime.Now;
                            _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                                "", "", "", "", "DUYETTHIETKE");
                        }
                    }

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = "TK_A",
                        MOTA = @"Duyệt thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();                    
                }             

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public void ApproveThietKeListDuyetCT2(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            //Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {                   
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {                       
                       return; 
                    }

                    //chay chiet tinh
                    var thietke = _tkDao.Get(objUi.MADDK);
                    var chiettinh = _ctDao.CreateChietTinh3(thietke, useragent, ipAddress, sManv);
                    if (thietke == null || chiettinh == null)
                    {
                        return;
                    }
                    
                    _db.SubmitChanges();
                }               

                // commit
                trans.Commit();

                _db.Connection.Close();
               
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();
                _db.Connection.Close();               
            }           
        }

        public Message DuyetTKCTTraHSThiCong(DONDANGKY obj, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;
                
                // Get current Item in db
                var objDb = Get(obj.MADDK);
                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", obj.TENKH);
                    return msg;
                }
                // update don dang ky
                objDb.TTCT = TTCT.CT_A.ToString();
                objDb.TTTK = TTTK.TK_A.ToString();

                // update thiet ke
                var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                if (tk == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", obj.TENKH);
                    return msg;
                }
                //tk.MANVDTK = sManv;
                //tk.NGAYDTK = ngayduyet;   
                
                if (tk.ISTRATHICONG == true)
                {
                    tk.ISTRATHICONG = false;
                    tk.ISTRAKEHOACH = false;
                    tk.NGAYTRATC = DateTime.Now;

                    objDb.TTTC = TTTC.TC_P.ToString();

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "DUYETTRAHSTKVETC");
                } 

                // luu vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = obj.MADDK,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.CT01.ToString(),
                    MATT = "TK_A",
                    MOTA = @"Duyệt thiết kế"
                };
                _kdDao.Insert(luuvetKyduyet);

                // Submit changes to db
                _db.SubmitChanges();               

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message RejectThietKeList(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTTK = TTTK.TK_RA.ToString();
                    

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_RA.ToString(),
                        MOTA = "Từ chối thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message RejectThietKeListTK(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, String chuthichtk)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTTK = TTTK.TK_RA.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;
                    tk.CHUTHICH = chuthichtk;
                    tk.NGAYDUYETN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOITK");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_RA.ToString(),
                        MOTA = "Từ chối thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        public Message RejectThietKeDonTanChau(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, string chuthichtk)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTTK = TTTK.TK_P.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    //tk.MANVDTK = sManv;
                    //tk.NGAYDTK = ngayduyet;
                    //tk.CHUTHICH = chuthichtk;
                    //tk.NGAYDUYETN = DateTime.Now;

                    tk.NGAYTRATK = DateTime.Now;
                    tk.MANVTRAKD = sManv;
                    tk.NOIDUNGTRAKD = chuthichtk;

                    _rpClass.HisNgayDangKyBien(tk.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TRAHSTKTANCHAU");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_P.ToString(),
                        MOTA = "Trả thiết kế (TC)."
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }

        /// Delete list        
        public Message DoAction(List<DONDANGKY> objList, PageAction action, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                #region Delete action
                
                if (action.Equals(PageAction.Delete))
                {
                    foreach (var obj in objList)
                    {
                        // Get current Item in db
                        var objDb = Get(obj.MADDK);
                        if (objDb == null)
                        {
                            // error message
                            msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", obj.TENKH);
                            return msg;
                        }
                        // Set delete info
                        _db.DONDANGKies.DeleteOnSubmit(objDb);

                        var luuvetKyduyet = new LUUVET_KYDUYET
                        {
                            MADON = objDb.MADDK,
                            IPAddress = ipAddress,
                            MANV = sManv,
                            UserAgent = useragent,
                            NGAYTHUCHIEN = DateTime.Now,
                            TACVU = TACVUKYDUYET.D.ToString(),
                            MACN = CHUCNANGKYDUYET.KH01.ToString(),
                            MATT = TTDK.DK_A.ToString(),
                            MOTA = "Xóa đơn lắp mới"
                        };
                        _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                    }

                    // Submit changes to db
                    _db.SubmitChanges();

                    // commit
                    trans.Commit();

                    _db.Connection.Close();

                    // success message
                    msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " đơn đăng ký");

                    return msg;
                } 
                #endregion

                #region Update
                if (action.Equals(PageAction.Update))
                {
                    foreach (var objUi in objList)
                    {
                        // Get current Item in db
                        var objDb = Get(objUi.MADDK);
                        if (objDb == null)
                        {
                            // error message
                            msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                            return msg;
                        }

                        objDb.MADDK = objUi.MADDK;
                        objDb.MADDKTONG = objUi.MADDKTONG;
                        objDb.TENKH = objUi.TENKH;
                        objDb.DIACHILD = objUi.DIACHILD;
                        objDb.SONHA = objUi.SONHA;
                        objDb.DIENTHOAI = objUi.DIENTHOAI;
                        objDb.MAPHUONG = objUi.MAPHUONG;
                        objDb.DUONGPHU = objUi.DUONGPHU;
                        objDb.MADP = objUi.MADP;
                        objDb.MAKV = objUi.MAKV;
                        objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                        objDb.MAMDSD = objUi.MAMDSD;
                        if (objUi.NGAYDK.HasValue)
                            objDb.NGAYDK = objUi.NGAYDK.Value;
                        if (objUi.NGAYHKS.HasValue)
                            objDb.NGAYHKS = objUi.NGAYHKS.Value;

                        objDb.TTCT = objUi.TTCT;
                        objDb.TTDK = objUi.TTDK;
                        objDb.TTHC = objUi.TTHC;
                        objDb.TTHD = objUi.TTHD;
                        objDb.TTTC = objUi.TTTC;
                        objDb.TTTK = objUi.TTTK;

                        objDb.LOAIDK = objUi.LOAIDK;
                        objDb.TENDK = objUi.TENDK;
                        objDb.DACBIET = objUi.DACBIET;
                        if (objUi.NGAYCD.HasValue)
                            objDb.NGAYCD = objUi.NGAYCD;
                        if (objUi.NGAYKS.HasValue)
                            objDb.NGAYKS = objUi.NGAYKS;
                        objDb.DTNGOAI = objUi.DTNGOAI;
                        objDb.PASSDUYETTK = objUi.PASSDUYETTK;
                        objDb.BATAICHO = objUi.BATAICHO;
                        objDb.CTCTMOI = objUi.CTCTMOI;
                        objDb.MAPB = objUi.MAPB;
                        objDb.MANV = objUi.MANV;
                        objDb.SOHODN = objUi.SOHODN;
                        objDb.SONK = objUi.SONK;
                        objDb.DMNK = objUi.DMNK;
                        objDb.DAIDIEN = objUi.DAIDIEN;
                        //objDb.NOIDUNG = objUi.NOIDUNG;

                        var luuvetKyduyet = new LUUVET_KYDUYET
                        {
                            MADON = objUi.MADDK,
                            IPAddress = ipAddress,
                            MANV = sManv,
                            UserAgent = useragent,
                            NGAYTHUCHIEN = DateTime.Now,
                            TACVU = TACVUKYDUYET.U.ToString(),
                            MACN = CHUCNANGKYDUYET.KH01.ToString(),
                            MATT = TTDK.DK_A.ToString(),
                            MOTA = "Sửa thông tin đơn lắp mới"
                        };
                        _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);

                        // Submit changes to db
                        _db.SubmitChanges();
                    }

                    // commit
                    trans.Commit();

                    _db.Connection.Close();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, objList.Count.ToString(), " đơn đăng ký");

                    return msg;
                } 
                #endregion

                foreach (var obj in objList)
                {
                    if (action.Equals(PageAction.Update))
                    {
                        msg = Update(obj, useragent, ipAddress, sManv);

                        if (msg.MsgType.Equals(MessageType.Error))
                        {
                            trans.Rollback();
                            _db.Connection.Close();
                            return msg;
                        }
                    }
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đơn đăng ký ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách đơn đăng ký ");
            }

            return msg;
        }


        private decimal? GetHeSo(string mahs)
        {
            // get dmnk
            var hs = _db.HESOs.Where(h => h.MAHS.Equals(mahs)).SingleOrDefault();
            decimal? gths = (hs != null && hs.GIATRI.HasValue) ? hs.GIATRI.Value : (decimal?)null;

            return gths;
        }

        public Message ApproveChietTinhList(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",  objUi.TENKH);
                        return msg;
                    }                    

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_A.ToString();
                    objDb.TTHD = TTHD.HD_N.ToString();

                    if (objDb.ISTUYENONGCHUNG.HasValue && objDb.ISTUYENONGCHUNG.Value)
                    {
                        objDb.TTHD = TTHD.HD_A.ToString();
                        objDb.TTTC = TTTC.TC_N.ToString();
                    }
                    
                    // update thiet ke
                    var ct = _db.CHIETTINHs.SingleOrDefault(t => t.MADDK.Equals(objDb.MADDK));
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính", objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    ct.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(ct.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETCTKH");

                    // Insert into QuyetToan
                   
                    //_db.SubmitChanges();
                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_A.ToString(),
                        MOTA = "Duyệt chiết tính"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }

        public Message AppChietTinhPhuTanTanChau(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_A.ToString();
                    //objDb.TTHD = TTHD.HD_N.ToString();

                    if (objDb.ISTUYENONGCHUNG.HasValue && objDb.ISTUYENONGCHUNG.Value)
                    {
                        objDb.TTHD = TTHD.HD_A.ToString();
                        objDb.TTTC = TTTC.TC_N.ToString();
                    }

                    // update thiet ke
                    var ct = _db.CHIETTINHs.SingleOrDefault(t => t.MADDK.Equals(objDb.MADDK));
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính", objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    ct.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(ct.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETCTKH");

                    // Insert into QuyetToan

                    //_db.SubmitChanges();
                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_A.ToString(),
                        MOTA = "Duyệt chiết tính"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }

        public Message ApproveChietTinhListLX(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {                  
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {                   
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_A.ToString();
                    objDb.TTHD = TTHD.HD_N.ToString();

                    if (objDb.ISTUYENONGCHUNG.HasValue && objDb.ISTUYENONGCHUNG.Value)
                    {
                        objDb.TTHD = TTHD.HD_A.ToString();
                        objDb.TTTC = TTTC.TC_N.ToString();
                    }

                    // update thiet ke
                    var ct = _db.CHIETTINHs.SingleOrDefault(t => t.MADDK.Equals(objDb.MADDK));
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính", objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    //ct.NGAYN = DateTime.Now;
                    //ct.GHICHU = ghichu;
                    // Insert into QuyetToan

                    if (ct.ISTRAHSTC == true)
                    {
                        ct.NGAYDUYETHSTC = DateTime.Now;

                        _rpClass.HisNgayDangKyBien(ct.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTRAHSTC");
                    }
                    else
                    {
                        ct.ISTRAHSTC = false;
                        ct.NGAYN = DateTime.Now;

                        _rpClass.HisNgayDangKyBien(ct.MADDK, sManv, objDb.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETCTKH");
                    }

                    //_db.SubmitChanges();
                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_A.ToString(),
                        MOTA = "Duyệt vật tư"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();
              
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {              
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }
            return msg;
        }

        public void ApproveChietTinhListDuyetCT(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {            
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    //var objDb = new DonDangKyDao().Get(objUi.MADDK);
                    var objDb = _ctDao.GetDonDangKyChietTinh(objUi.MADDK);
                    if (objDb == null)
                    {                       
                        return;
                    }
                                   
                    //UpdateDuyetCT(objDb, useragent, ipAddress, sManv);
                    
                    // update don dang ky
                    _rpClass.DSQuiTrinhNuocBien(DateTime.Now, DateTime.Now, "", "", objUi.MADDK, "", "DUYETCTLX");

                    //objDb.TTHD = "HD_N"; // TTHD.HD_N.ToString();
                    //objDb.TTCT = "CT_A"; // TTCT.CT_A.ToString();                    

                    //if (objDb.ISTUYENONGCHUNG.HasValue && objDb.ISTUYENONGCHUNG.Value)
                    //{
                    //    objDb.TTHD = TTHD.HD_A.ToString();
                    //    objDb.TTTC = TTTC.TC_N.ToString();
                    //}

                    // update thiet ke
                    //var ct = _db.CHIETTINHs.SingleOrDefault(t => t.MADDK.Equals(objDb.MADDK));
                    //if (ct == null)
                    //{                        
                    //    return ;
                    //}
                    //ct.MANVDCT = sManv;
                    //ct.NGAYDCT = ngayduyet;                 

                    //_db.SubmitChanges();
                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "CT_P",
                        MOTA = "Chờ duyệt vật tư"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();
              
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();
                _db.Connection.Close();
            }          
        }

        public Message RejectChietTinhList(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_RA.ToString();

                    // update thiet ke
                    var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    ct.NGAYN = DateTime.Now;

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_RA.ToString(),
                        MOTA = "Từ chối chiết tính"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_REJECT_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }

        public Message RejectChietTinhListTuChoi(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, string ghichu)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_RA.ToString();

                    // update thiet ke
                    var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    ct.GHICHU = ghichu;
                    ct.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(ct.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOICT");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_RA.ToString(),
                        MOTA = "Từ chối chiết tính"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_REJECT_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }

        public Message RejectChietTinhListLX(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, string ghichu)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_RA.ToString();

                    // update thiet ke
                    var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    ct.GHICHU = ghichu;
                    ct.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBien(ct.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOICT");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_RA.ToString(),
                        MOTA = "Từ chối vật tư"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_REJECT_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }

        public Message RejectVatTuTraHSKyThuat(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, string ghichu)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_RA.ToString();
                    objDb.TTTK = TTTK.TK_P.ToString();                    

                    //update thiet ke
                    var objTK = _tkDao.Get(objUi.MADDK);
                    if (objTK == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế", objTK.MADDK);
                        return msg;
                    }

                    objTK.ISTRAKEHOACH = true;

                    // update chiet tinh
                    var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDK)).SingleOrDefault();
                    if (ct == null)
                    {                        
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính", objUi.TENKH);
                        return msg;
                    }
                    
                    ct.NGAYTRAHSKH = DateTime.Now;
                    ct.LYDOTRAHSKH = ghichu;

                    _rpClass.HisNgayDangKyBien(objUi.MADDK, sManv, objUi.MAKV, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TRAHSKHKT");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTCT.CT_RA.ToString(),
                        MOTA = "Từ chối vật tư"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_REJECT_SUCCEED, MessageType.Info, "danh sách chiết tính");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách chiết tính", ex.Message);
            }

            return msg;
        }

        public Message ApproveThiCongList(List<THICONG> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    _db.THICONGs.InsertOnSubmit(objUi);
                    _db.SubmitChanges();

                    // commit
                    var ddk = _db.DONDANGKies.Where(d => d.MADDK.Equals(objUi.MADDK)).SingleOrDefault();
                    ddk.TTTC = TTTC.TC_N.ToString();

                    // luu vet
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

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thi công");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thi công", ex.Message);
            }

            return msg;
        }

        public Message RejectThiCongList(List<DONDANGKY> objList, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var objUi in objList)
                {
                    // Get current Item in db
                    var objDb = Get(objUi.MADDK);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.TTCT = TTTC.TC_RA.ToString();

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDK,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTTC.TC_RA.ToString(),
                        MOTA = "Từ chối thi công"
                    };
                    _kdDao.Insert(luuvetKyduyet);

                    // Submit changes to db
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_APPROVE_SUCCEED, MessageType.Info, "danh sách thiết kế");

                return msg;
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleUpdateException(ex, "duyệt danh sách thiết kế", ex.Message);
            }

            return msg;
        }



        public List<DONDANGKY> GetListDonChoThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonChoThiCongPB(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {           
            var result = from d in _db.DONDANGKies
                         join quyen in _db.DUYET_QUYENs on d.MADDK equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               d.TTCT == TTCT.CT_A.ToString() && d.TTHD == TTHD.HD_A.ToString() 
                            //   &&  d.TTTC.Equals(TTTC.TC_N) 
                               && quyen.MAPB.Equals(mapb) && d.MAKV.Equals(areaCode)
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonChoThiCongKhuVuc(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {            
            var result = from d in _db.DONDANGKies
                         join quyen in _db.DUYET_QUYENs on d.MADDK equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               d.TTCT == TTCT.CT_A.ToString() && d.TTHD == TTHD.HD_A.ToString()
                               //d.TTTC.Equals(TTTC.TC_N) && quyen.MAPB.Equals(mapb) && d.MAKV.Equals(areaCode)
                              // d.TTTC.Equals(TTTC.TC_N) 
                               && d.MAKV.Equals(areaCode)
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonChoThiCongKhuVucCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later                     
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKies
                         join quyen in _db.DUYET_QUYENs on d.MADDK equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               //d.TTCT == TTCT.CT_A.ToString() &&
                               d.TTHD == TTHD.HD_A.ToString() 
                             //d.TTTC.Equals(TTTC.TC_N) && quyen.MAPB.Equals(mapb) && d.MAKV.Equals(areaCode)
                             //  d.TTTC.Equals(TTTC.TC_N) 
                                   && d.MAKV.Equals(areaCode)
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListBienBanPB(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKies
                         join quyen in _db.DUYET_QUYENs on d.MADDK equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               d.TTCT == TTCT.CT_A.ToString() && d.TTHD == TTHD.HD_A.ToString() &&
                               d.TTTC == TTTC.TC_A.ToString() && d.TTNT == null
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListBienBanPBCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKies
                         join quyen in _db.DUYET_QUYENs on d.MADDK equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               //d.TTCT == TTCT.CT_A.ToString() && 
                               d.TTHD == TTHD.HD_A.ToString() &&
                               d.TTTC == TTTC.TC_A.ToString() && d.TTNT == null
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListBienBanPBKV(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKies
                         join quyen in _db.DUYET_QUYENs on d.MADDK equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               d.TTCT == TTCT.CT_A.ToString() && d.TTHD == TTHD.HD_A.ToString() &&
                               d.TTTC == TTTC.TC_A.ToString() && d.TTNT == null
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonThiCong()
        {
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_P)));
            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonDongHo(string makv, string sono)
        {
            var result = from don in _db.DONDANGKies 
                         join tc in _db.THICONGs on don.MADDK equals tc.MADDK
                         join dh in _db.DONGHOs on tc.MADH equals dh.MADH
                         where don.MAKV.Equals(makv) && dh.SONO.Contains(sono)
                         select don;

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK.Equals(TTDK.DK_A.ToString())) &&
                                                        (d.TTTK.Equals(TTTK.TK_A.ToString())) &&
                                                        (d.TTCT.Equals(TTCT.CT_A.ToString())) &&
                                                        (d.TTHD.Equals(TTHD.HD_A.ToString())) &&
                                                        (d.TTTC.Equals(TTTC.TC_P.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonThiCongCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK.Equals(TTDK.DK_A.ToString())) &&
                                                        (d.TTTK.Equals(TTTK.TK_A.ToString())) &&
                                                        //(d.TTCT.Equals(TTCT.CT_A.ToString())) &&
                                                        (d.TTHD.Equals(TTHD.HD_A.ToString())) &&
                                                        (d.TTTC.Equals(TTTC.TC_P.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }

        public List<DONDANGKY> GetListDonDaThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_A)));
            if (keyword != null)
                result = result.Where(d => d.MADDK.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);
            if (areaCode != null && areaCode != "%")
                result = result.Where(d => d.MAKV == areaCode);

            return result.OrderByDescending(d => d.MADDK).ToList();
        }
    }
}
