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
    public class DonDangKyPoDao
    {
        private readonly KhuVucPoDao _kvpoDao = new KhuVucPoDao();
        private readonly ReportClass _rpClass = new ReportClass();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();        

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DonDangKyPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public int DemCMND(string cmnd, string makvpo)
        {
            return _db.DONDANGKYPOs.Where(p => p.CMND.Equals(cmnd) && p.MAKVPO.Equals(makvpo)).Count();
        }

        public DONDANGKYPO Get(string ma)
        {
            return _db.DONDANGKYPOs.Where(p => p.MADDKPO.Equals(ma)).SingleOrDefault();
        }

        public DONDANGKYPO GetCMND(string cmnd, string makvpo)
        {
            return _db.DONDANGKYPOs.Where(p => p.CMND.Equals(cmnd) && p.MADDKPO.Equals(makvpo)).SingleOrDefault();
        }

        public DONDANGKYPO GetListCMNV1(string macnmnd, string makv)
        {
            return _db.DONDANGKYPOs.Where(d => d.CMND.Equals(macnmnd) && d.MAKVPO.Equals(makv)).SingleOrDefault();
        }

        public List<DONDANGKYPO> GetListCMNV2(string macnmnd, string makv)
        {
            return _db.DONDANGKYPOs.Where(d => d.CMND.Equals(macnmnd) && d.MAKVPO.Equals(makv)).ToList();
        }

        public List<DONDANGKYPO> GetListForTcdldm()
        {
            return _db.DONDANGKYPOs.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKYPO> GetListMa(string ma)
        {
            return _db.DONDANGKYPOs.Where(p => p.MADDKPO.Equals(ma)).ToList();
        }

        public List<DONDANGKYPO> GetList()
        {
            return _db.DONDANGKYPOs.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null))
                .OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKYPO> GetListKV(String makv)
        {
            return _db.DONDANGKYPOs.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null) && d.MAKVPO.Equals(makv))
                //.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
                .OrderByDescending(d => d.MADDKPO.Substring(3,8)).ToList();
        }

        public List<DONDANGKYPO> GetListHD(String maddk, String tenkh, String makv)
        {
            var query = _db.DONDANGKYPOs.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDKPO.Contains(maddk));
            if (!String.IsNullOrEmpty(tenkh))
                query = query.Where(d => d.TENKH.Contains(tenkh));               

            return query.Where(d => d.MAKVPO.Equals(makv) && d.TTCT.Equals(TTCT.CT_A) && (d.TTHD.Equals(TTHD.HD_A)) )
                .OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
            
        }
        
        public List<DONDANGKYPO> GetList(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong)
        {
            var query = _db.DONDANGKYPOs.Where(d => d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null)).AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDKPO.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADPPO.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));

            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSDPO.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKVPO.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            return query.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKYPO> GetListMAPB(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong, String mapb)
        {
            var query = _db.DONDANGKYPOs.Where(d => d.MAKVPO.Equals(makv) && d.TTDK.Equals(TTDK.DK_A) && (d.TTCT == null) && d.MADDKPO.Substring(1, 2) == mapb).AsQueryable();

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
            return query.OrderByDescending(d => d.MADDKPO.Substring(3,8)).ToList();
        }

        public List<DONDANGKYPO> GetListForTcdldm(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong)
        {
            var query = _db.DONDANGKYPOs.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDKPO.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADPPO.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));


            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSDPO.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKVPO.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            return query.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListForTcdldmcm(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong, string cmnd)
        {
            var query = _db.DONDANGKYPOs.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDKPO.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADPPO.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));
            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSDPO.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKVPO.Equals(makv));

            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MADDKPO.Substring(1, 2).Equals(maphuong));

            //if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
            //    query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (!String.IsNullOrEmpty(cmnd))
                query = query.Where(d => d.CMND.Contains(cmnd));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
            return query.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListForTcdldmPB(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong,String ma_PB)
        {
            //var query = _db.DONDANGKies.Where(p => p.MADDK.Substring(1,2).Equals(ma_PB))
                    //.AsQueryable();
            var query1 = from don in _db.DONDANGKYPOs
                         join duyet in _db.DUYET_QUYENs on don.MADDKPO equals duyet.MADDK
                        where (don.MADDKPO.Substring(1,2).Equals(ma_PB) || duyet.MAPB.Equals(ma_PB) )
                        select don;
            var query = query1.AsQueryable();
            

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDKPO.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADPPO.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));


            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSDPO.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKVPO.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            return query.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKYPO> GetListForTcdldmPBcm(String maddk, String hoten, String sonha, String dienthoai,
                        String madp, String duong, int? sohodn, int? sonk, int? dmnk,
                        String mamdsd, String makv, String maphuong, String ma_PB, string cmnd)
        {            
            //var query1 = from don in _db.DONDANGKYPOs
            //             join duyet in _db.DUYET_QUYENs on don.MADDKPO equals duyet.MADDK
            //             where (don.MADDKPO.Substring(1, 2).Equals(ma_PB) || duyet.MAPB.Equals(ma_PB))
            //             select don;

            var query1 = from don in _db.DONDANGKYPOs
                         join duyet in _db.DUYET_QUYENs on don.MADDKPO equals duyet.MADDK into sr
                         from x in sr.DefaultIfEmpty()
                         where (don.MADDKPO.Substring(1, 2).Equals(ma_PB) || x.MAPB.Equals(ma_PB))
                         select don;

            var query = query1.AsQueryable();

            if (!String.IsNullOrEmpty(maddk))
                query = query.Where(d => d.MADDKPO.Contains(maddk));
            if (!String.IsNullOrEmpty(hoten))
                query = query.Where(d => d.TENKH.Contains(hoten));
            if (!String.IsNullOrEmpty(sonha))
                query = query.Where(d => d.SONHA.Contains(sonha));
            if (!String.IsNullOrEmpty(dienthoai))
                query = query.Where(d => d.DIENTHOAI.Contains(dienthoai));
            if (!String.IsNullOrEmpty(madp))
                query = query.Where(d => d.MADPPO.Equals(madp));
            if (!String.IsNullOrEmpty(duong))
                query = query.Where(d => d.DIACHILD.Contains(duong));
            if (!String.IsNullOrEmpty(mamdsd) && mamdsd != "%")
                query = query.Where(d => d.MAMDSDPO.Equals(mamdsd));
            if (!String.IsNullOrEmpty(makv) && makv != "%")
                query = query.Where(d => d.MAKVPO.Equals(makv));
            if (!String.IsNullOrEmpty(maphuong) && maphuong != "%")
                query = query.Where(d => d.MAPHUONG.Equals(maphuong));

            if (!String.IsNullOrEmpty(cmnd))
                query = query.Where(d => d.CMND.Contains(cmnd));

            if (sohodn.HasValue)
                query = query.Where(d => d.SOHODN.Equals(sohodn.Value));
            if (sonk.HasValue)
                query = query.Where(d => d.SONK.Equals(sonk.Value));
            if (dmnk.HasValue)
                query = query.Where(d => d.DMNK.Equals(dmnk.Value));

            return query.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKYPO> GetListDonDongHo(string makvpo, string sono)
        {
            var result = from don in _db.DONDANGKYPOs
                         join tc in _db.THICONGs on don.MADDKPO equals tc.MADDK
                         join dh in _db.DONGHOPOs on tc.MADH equals dh.MADHPO
                         where don.MAKVPO.Equals(makvpo) && dh.SONO.Contains(sono)
                         select don;

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }
        
        public List<DONDANGKYPO> GetList(String keyword, DateTime? fromDate, DateTime? toDate, string makv, string maNV)
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

            var dsdon = from don in _db.DONDANGKYPOs
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N)))
                        select don;

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));

            if (makv != "" && makv != "%")
                dsdon = dsdon.Where(d => d.MAKVPO == makv);

            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.NGAYDK.HasValue
                                           && d.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();

        }

        public List<DONDANGKYPO> GetListPB(String keyword, string maPb, string makv)
        {
           /*
            var dsdon = from don in _db.DONDANGKies 
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N)))
                        select don;
            */
            var dsdon = from don in _db.DONDANGKYPOs 
                            join quyen in _db.DUYET_QUYENs on don.MADDKPO equals quyen.MADDK
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.TTTK.Equals(TTTK.TK_N) && don.MAKVPO.Equals(makv)
                                && quyen.MAPB.Equals(maPb))                        
                        select don ;

            if (!string.IsNullOrEmpty(keyword))
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                      d.MADDKTONG.Contains(keyword) ||
                                      d.TENKH.Contains(keyword) ||
                                      d.DIACHILD.Contains(keyword) ||
                                      d.DIENTHOAI.Contains(keyword));

            return dsdon.ToList();

        }        
        
        public List<DONDANGKYPO> GetDaiDienList(string maKV)
        {
            return _db.DONDANGKYPOs.Where(d => (d.MAKVPO.Equals(maKV) && d.DAIDIEN.Equals(true))).ToList();
        }
       
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

            sReturn = (from p in _db.DONDANGKYPOs.Where(p => p.MADDKPO.Substring(3, 8).Contains(sToday))
                       select p.MADDKPO.Substring(3, 8)).Max();

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
       
        public List<DONDANGKYPO> GetListForKhaoSat(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string maNV, string makv)
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
            var dsdon = from don in _db.DONDANGKYPOs
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.MAKVPO.Equals(makv)
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString())))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
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

            //return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
            return dsdon.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListForKhaoSatPB(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string maNV, String mapb, string makv)
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
            var dsdon = from don in _db.DONDANGKYPOs
                        where (don.TTDK.Equals(TTDK.DK_A.ToString()) && don.MAKVPO.Equals(makv)
                               && (don.TTTK.Equals(null) || don.TTTK.Equals(TTTK.TK_RA.ToString()))
                               && don.MADDKPO.Substring(1, 2) == mapb
                               )
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
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

            return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }
        /// <summary>
        /// Danh sách các đơn chờ bốc vật tư 
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maNV">Mã nhân viên</param>
        /// <returns></returns>
        public List<DONDANGKYPO> GetListForChoBocVatTu(String keyword, DateTime? fromDate, DateTime? toDate, string maNV)
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
            var dsdon = from don in _db.DONDANGKYPOs
                        where (don.TTDK.Equals(TTDK.DK_A.ToString())
                               && (don.TTTK.Equals(TTTK.TK_N)))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
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


            return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        /// <summary>
        /// Danh sách các đơn đang bốc vật tư 
        /// </summary>
        /// <param name="keyword">Từ khóa tìm kiếm</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="maNV">Mã nhân viên</param>
        /// <returns></returns>
        public List<THIETKEPO> GetListForBocVatTu(String keyword, DateTime? fromDate, DateTime? toDate, string maNV)
        {
            /*
            var phuong = from pc in _db.PHANCONGTHEOPHUONGs
                         where pc.MANV.Equals(maNV)
                         select pc.MAPHUONG;

            var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P) && phuong.Contains(tk.DONDANGKY.MAPHUONG))
                        select don;
            */

            var dsdon = from don in _db.THIETKEPOs.Where(tk => tk.DONDANGKYPO.TTTK.Equals(TTTK.TK_P))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKYPO.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKYPO.TENKH.Contains(keyword) ||
                                         d.DONDANGKYPO.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKYPO.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKYPO.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKYPO.MADDKPO).ToList();
        }

        public List<THIETKEPO> GetListForBocVatTuKV(String keyword, DateTime? fromDate, DateTime? toDate, string maNV, string maKV)
        {           

            //var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P))
            //            select don;

            var dsdon = from tk in _db.THIETKEPOs join don in _db.DONDANGKYPOs on tk.MADDKPO equals don.MADDKPO
                        orderby tk.MADDKPO descending
                        where (don.TTTK.Equals(TTTK.TK_P) && don.MAKVPO.Equals(maKV))
                        select tk;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKYPO.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKYPO.TENKH.Contains(keyword) ||
                                         d.DONDANGKYPO.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKYPO.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKYPO.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value <= toDate.Value);

            //return dsdon.OrderByDescending(d => d.DONDANGKYPO.MADDKPO).ToList();
            return dsdon.OrderByDescending(d => d.NGAY).ToList();
        }

        public List<THIETKEPO> GetListForBocVatTuPB(String keyword, DateTime? fromDate, DateTime? toDate, string maNV, string maPB, string makv)
        {            

            /*var dsdon = from don in _db.THIETKEs.Where(tk => tk.DONDANGKY.TTTK.Equals(TTTK.TK_P))
                        select don;*/

            var dsdon = from tk in _db.THIETKEPOs join duyet in _db.DUYET_QUYENs on tk.MADDKPO equals duyet.MADDK
                                join don in _db.DONDANGKYPOs on tk.MADDKPO equals don.MADDKPO
                        orderby tk.MADDKPO descending
                        where (don.MAKVPO.Equals(makv) && don.TTTK.Equals(TTTK.TK_P) && don.MADDKPO.Substring(1,2).Equals(maPB) || duyet.MAPB.Equals(maPB))
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
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKYPO.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKYPO.TENKH.Contains(keyword) ||
                                         d.DONDANGKYPO.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKYPO.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKYPO.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value <= toDate.Value);


            //return dsdon.OrderByDescending(d => d.DONDANGKYPO.MADDKPO).ToList();
            return dsdon.OrderByDescending(d => d.NGAY).ToList();
            
        }
               
        public List<THIETKEPO> GetListForTraCuuThietKe(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {

            var dsdon = from don in _db.THIETKEPOs.Where(tk => tk.DONDANGKYPO.TTTK.Equals(TTTK.TK_P) || tk.DONDANGKYPO.TTTK.Equals(TTTK.TK_A))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKYPO.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKYPO.TENKH.Contains(keyword) ||
                                         d.DONDANGKYPO.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKYPO.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKYPO.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKYPO.MADDKPO).ToList();
        }

        public List<THIETKEPO> GetListForTraCuuThietKePB(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, String maPB)
        {          

            var dsdon = from tk in _db.THIETKEPOs join don in _db.DONDANGKYPOs on tk.MADDKPO equals don.MADDKPO
                            join duyet in _db.DUYET_QUYENs on tk.MADDKPO equals duyet.MADDK
                        where((don.TTTK.Equals(TTTK.TK_P) || don.TTTK.Equals(TTTK.TK_A))
                            && duyet.MAPB.Equals(maPB))
                        select tk;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKYPO.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKYPO.TENKH.Contains(keyword) ||
                                         d.DONDANGKYPO.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKYPO.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKYPO.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKYPO.MADDKPO).ToList();
        }

        public List<THIETKEPO> GetListForTraCuuThietKeKV(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, String maKV)
        {
            var dsdon = from tk in _db.THIETKEPOs
                        join don in _db.DONDANGKYPOs on tk.MADDKPO equals don.MADDKPO                       
                        where ((don.TTTK.Equals(TTTK.TK_P) || don.TTTK.Equals(TTTK.TK_A))
                            && don.MAKVPO.Equals(maKV))
                        select tk;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
                                         d.TENTK.Contains(keyword) ||
                                         d.CHUTHICH.Contains(keyword) ||
                                         d.DONDANGKYPO.MADDKTONG.Contains(keyword) ||
                                         d.DONDANGKYPO.TENKH.Contains(keyword) ||
                                         d.DONDANGKYPO.NOIDUNG.Contains(keyword) ||
                                         d.DONDANGKYPO.DIACHILD.Contains(keyword) ||
                                         d.DONDANGKYPO.DIENTHOAI.Contains(keyword));
            if (fromDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value >= fromDate.Value);

            if (toDate.HasValue)
                dsdon = dsdon.Where(d => d.DONDANGKYPO.NGAYDK.HasValue
                                           && d.DONDANGKYPO.NGAYDK.Value <= toDate.Value);


            return dsdon.OrderByDescending(d => d.DONDANGKYPO.MADDKPO).ToList();
        }
        /// <summary>
        /// Danh sách đơn chờ duyệt thiết kế
        /// </summary>
        /// <param name="keyword">Từ khóa cần tra cứu</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<DUYETTHIETKEPO> GetListForDuyetThietKe(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETTHIETKEPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDKPO.Substring(3,8))
                        .ToList();
        }

        public List<DUYETTHIETKEPO> GetListForDuyetThietKePo(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETTHIETKEPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKVPO.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDKPO.Substring(3, 8))
                        .ToList();
        }

        public List<DUYETTHIETKEPO> GetListForDuyetThietKeTCPo(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, string areaCode)
        {
            // increase performance later
            //var result = _db.DUYETTHIETKEPOs.Where(d => d.MAKVPO.Equals(makvpo) && d.TTDK.Equals("DK_A")
              // && d.TTTK.Equals("TK_A") && d.TTCT.Equals(null));         

            var result = _db.DUYETTHIETKEPOs.Where(d => d.TTDK.Equals("DK_A") && d.MAKVPO.Equals(areaCode) &&
                                                        (d.TTTK.Equals("TK_A") || d.TTTK == TTTK.TK_RA.ToString()) && d.TTCT.Equals(null));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||                                    
                                      d.TENKH.Contains(keyword) ||                                    
                                      d.DIENTHOAI.Contains(keyword));
           // if (fromDate.HasValue)
          //      result = result.Where(d => d.NGAYDK.HasValue
          //                                 && d.NGAYDK.Value >= fromDate.Value);

           // if (toDate.HasValue)
            //    result = result.Where(d => d.NGAYDK.HasValue
           //                                && d.NGAYDK.Value <= toDate.Value);

           // if (stateCode != null)
           //     result = result.Where(d => d.TTTK == stateCode);

            //if (areaCode != null)
             //   result = result.Where(d => d.MAKVPO == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDKPO.Substring(3, 8))
                        .ToList();
        }

        public List<DUYETTHIETKEPO> GetListForDuyetThietKePBPo(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode, string mapb)
        {
            // increase performance later
            //var result = _db.DUYETTHIETKEPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKVPO.Equals(areaCode) &&
            //                                            (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString()));

            var result = from d in _db.DUYETTHIETKEPOs join duyet in _db.DUYET_QUYENs on d.MADDKPO equals duyet.MADDK
                    where(d.TTDK == TTDK.DK_A.ToString()) && d.MAKVPO.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_P.ToString() || d.TTTK == TTTK.TK_RA.ToString())
                        && duyet.MAPB.Equals(mapb)
                    select d;

            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDKPO.Substring(3, 8))
                        .ToList();
        }

        public List<DUYETTHIETKEPO> GetListTKTanChauPo(string makvpo)
        {
            var result = _db.DUYETTHIETKEPOs.Where(d => d.MAKVPO.Equals(makvpo) && d.TTDK.Equals("DK_A")
                && d.TTTK.Equals("TK_A") && d.TTCT.Equals(null));         
            

            //return result.OrderByDescending(d => d.MADDK)
            return result.OrderByDescending(d => d.MADDKPO.Substring(3, 8))
                        .ToList();
        }

        public List<DUYETCHIETTINHPO> GetListForDuyetChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKVPO.Equals(areaCode) &&
                                                        ((d.TTCT == TTCT.CT_P.ToString()) || (d.TTCT == TTCT.CT_RA.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).OrderByDescending(d=>d.NGAYLCT).ToList();
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
        public List<DONDANGKYPO> GetListForLapChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) && d.MAKVPO.Equals(areaCode) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        //(d.TTCT == TTCT.CT_N.ToString()) && (d.TTHD == null)).AsQueryable();
                                                        (d.TTCT == TTCT.CT_N.ToString())).AsQueryable();

            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        /// <summary>
        /// Danh sách tra cứu chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
        public List<DUYETCHIETTINHPO> GetListForTraCuuChietTinh(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            // increase performance later
            var result = _db.DUYETCHIETTINHPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_N.ToString() || d.TTCT == TTCT.CT_P.ToString() ||
                                                        d.TTCT == TTCT.CT_RA.ToString() || d.TTCT == TTCT.CT_A.ToString()));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            if (stateCode != null)
                result = result.Where(d => d.TTCT == stateCode);

            return result.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYLCT).ToList();
        }

        /// <summary>
        /// Danh sách tra cứu chiết tính
        /// </summary>
        /// <param name="keyword">Từ khóa</param>
        /// <param name="fromDate">Từ ngày</param>
        /// <param name="toDate">Đến ngày</param>
        /// <param name="stateCode">Trạng thái</param>
        /// <param name="areaCode">Khu vực</param>
        /// <returns></returns>
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
        
        public List<DONDANGKYPO> GetListForXldchd(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {

            var dsdon = from don in _db.DONDANGKYPOs
                        where (don.TTCT.Equals(TTCT.CT_A.ToString())
                               && don.LOAIDK.Equals(LOAIDK.DK.ToString())
                               && (don.TTHD.Equals(null) || don.TTHD.Equals(TTHD.HD_RA.ToString())))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
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
                dsdon = dsdon.Where(d => d.MAKVPO == areaCode);

            return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DONDANGKYPO> GetListForDonChoHopdong(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            var dsdon = from don in _db.DONDANGKYPOs
                        where (don.TTCT.Equals(TTCT.CT_A.ToString())
                               && don.TTHD.Equals(TTHD.HD_N.ToString()))
                        select don; 

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
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
                dsdon = dsdon.Where(d => d.MAKVPO == areaCode);

            return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public List<DONDANGKYPO> GetListDaDuyetTK_CD(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            var dsdon = from don in _db.DONDANGKYPOs
                        //where (don.TTCT.Equals(TTCT.CT_A.ToString())  && don.TTHD.Equals(TTHD.HD_N.ToString()))
                        where (don.TTTK.Equals(TTTK.TK_A.ToString()) && (don.TTHD.Equals(TTHD.HD_N.ToString()) || don.TTHD == null))
                        select don;

            if (keyword != null)
                dsdon = dsdon.Where(d => d.MADDKPO.Contains(keyword) ||
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
                dsdon = dsdon.Where(d => d.MAKVPO == areaCode);

            return dsdon.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }
        
        public List<DONDANGKYPO> GetListForXuLyDonChoThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(null)));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        
        public List<DONDANGKYPO> GetListForLapQuyetToan(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTTC == TTTC.TC_A.ToString()) &&
                                                        //(d.THICONG.TTQT == true) &&
                                                        //d.QUYETTOAN != null
                                                        (d.TTHC == null || d.TTHC == TTQT.QT_N.ToString())   );
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public Message Insert(DONDANGKYPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.DONDANGKYPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                                        {
                                            MADON = objUi.MADDKPO,
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

        public Message UpdateTT(DONDANGKYPO objUi)
        {
             Message msg;
             try
             {
                 // get current object in database
                 var objDb = Get(objUi.MADDKPO);

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
                 }// success message
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

        public Message UpdateHDTC(DONDANGKYPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO);

                if (objDb != null)
                {   
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;                   

                    // Submit changes to db
                    _db.SubmitChanges();
                }// success message
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

        public Message UpdateHDTCMoi(string maddk, string makv)
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

                // Submit changes to db
                _db.SubmitChanges();

                _db.Connection.Close();

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

        public void UpdateQT_A(DONDANGKYPO objUi)
        {
            // get current object in database
            var objDb = Get(objUi.MADDKPO);

            if (objDb != null)
            {

                objDb.TTHC = objUi.TTHC;                

                // Submit changes to db
                _db.SubmitChanges();
            }
        }

        public Message Update(DONDANGKYPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDKPO = objUi.MADDKPO;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;

                    objDb.TENDK = objUi.TENDK;

                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.MAKVPO = objUi.MAKVPO;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSDPO = objUi.MAMDSDPO;
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
                    objDb.TTNT = objUi.TTNT;

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

                    var hopdongpo = _hdpoDao.Get(objUi.MADDKPO);
                    if (hopdongpo != null)
                    {
                        hopdongpo.MAMDSDPO = objUi.MAMDSDPO;
                    }

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

        public bool IsInUse(string ma)
        {
            if (_db.DONDANGKYPOs.Where(p => p.MADDKTONG.Equals(ma)).Count() > 0)
                return true;
            /*if (_db.KHACHHANGs.Where(p => p.MADDK.Equals(ma)).Count() > 0)
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
            */
            return false;
        }

        public Message Delete(DONDANGKYPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDKPO);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký ", objUi.TENKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DONDANGKYPOs.DeleteOnSubmit(objDb);
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

        public Message UpdateDiaChi(List<DONDANGKYPO> objList)
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
                    var objDb = Get(objUi.MADDKPO);
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
        
        public Message UpdateList(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDKPO = objUi.MADDKPO;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.MAKVPO = objUi.MAKVPO;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSDPO = objUi.MAMDSDPO;
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
                    objDb.NOIDUNG = objUi.NOIDUNG;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;

                    var luuvetKyduyet = new LUUVET_KYDUYET
                                            {
                                                MADON = objUi.MADDKPO,
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

        public Message UpdateList2(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, string mapb)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDKPO = objUi.MADDKPO;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.MAKVPO = objUi.MAKVPO;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSDPO = objUi.MAMDSDPO;
                    if (objUi.NGAYDK.HasValue)
                        objDb.NGAYDK = objUi.NGAYDK.Value;
                    if (objUi.NGAYHKS.HasValue)
                        objDb.NGAYHKS = objUi.NGAYHKS.Value;

                    objDb.TTCT = objUi.TTCT;
                    objDb.TTDK = objUi.TTDK;
                    objDb.TTHC = objUi.TTHC;
                    objDb.TTHD = objUi.TTHD;
                    objDb.TTTC = objUi.TTTC;
                    objDb.TTTK = TTTK.TK_N.ToString();//thiet ke moi

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

                    _rpClass.HisNgayDangKyBienPo(objUi.MADDKPO, sManv, objUi.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "DUYETDONDK");

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
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
                        MADDK = objUi.MADDKPO,
                        MANV = sManv,
                        MAPB = mapb,
                        MAKV = objUi.MAKVPO,
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

        public Message UpdateListTuChoi(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    objDb.MADDKPO = objUi.MADDKPO;
                    objDb.MADDKTONG = objUi.MADDKTONG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.DIACHILD = objUi.DIACHILD;
                    objDb.SONHA = objUi.SONHA;
                    objDb.DIENTHOAI = objUi.DIENTHOAI;
                    objDb.MAPHUONG = objUi.MAPHUONG;
                    objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.MAKVPO = objUi.MAKVPO;
                    objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                    objDb.MAMDSDPO = objUi.MAMDSDPO;
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
                    objDb.NOIDUNG = objUi.NOIDUNG;

                    objDb.CMND = objUi.CMND;
                    objDb.MST = objUi.MST;
                    objDb.SDInfo_INHOADON = objUi.SDInfo_INHOADON;
                    objDb.TENKH_INHOADON = objUi.TENKH_INHOADON;
                    objDb.DIACHI_INHOADON = objUi.DIACHI_INHOADON;

                    objDb.ISTUYENONGCHUNG = objUi.ISTUYENONGCHUNG;

                    _rpClass.HisNgayDangKyBienPo(objUi.MADDKPO, sManv, objUi.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOIDONDK");

                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTDK.DK_A.ToString(),
                        MOTA = "Từ chối đơn lắp mới 2."
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

        public Message ApproveThietKeListCD(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    var khuvucpo = _kvpoDao.Get(objDb.MAKVPO);
                    //_rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTCT = TTCT.CT_N.ToString();
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = tk.NGAYLTK;

                    tk.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_A.ToString(),
                        MOTA = "Duyệt thiết kế điện"
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

        public Message ApproveThietKeLaiChuaChietTinh(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    if (objDb.TTCT == "CT_N" || objDb.TTCT == null)
                    {
                        objDb.TTTK = TTTK.TK_P.ToString();
                        objDb.TTCT = TTCT.CT_RA.ToString();                       
                    }                    

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    //tk.MANVDTK = sManv;
                    //tk.NGAYDTK = tk.NGAYLTK;
                    //tk.NGAYN = DateTime.Now;
                    //_rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                    //        "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
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

        public Message ApproveDonThietKeTanChau(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }                    

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTTK = TTTK.TK_A.ToString();
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if(tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;

                    tk.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_A.ToString(),
                        MOTA = "Duyệt thiết kế điện"
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

        public Message ApproveDonThietKeKDTanChau(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    var khuvucpo = _kvpoDao.Get(objDb.MAKVPO);
                    _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");

                    // update thiet ke
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    //tk.MANVDTK = sManv;
                    //tk.NGAYDTK = ngayduyet;
                    //tk.NGAYN = DateTime.Now;

                    tk.MANVDUYETKD = sManv;
                    tk.NGAYDUYETKD = ngayduyet;
                    tk.NGAYDUYETKDN = DateTime.Now;

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETDONTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_A.ToString(),
                        MOTA = "Duyệt đơn, thiết kế điện (Tân Châu)"
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

        public Message ApproveThietKeList(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    var khuvucpo = _kvpoDao.Get(objDb.MAKVPO);
                    //_rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTCT = TTCT.CT_N.ToString();
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }

                    // update don dang ky
                    //objDb.TTCT = TTCT.CT_N.ToString();
                    //objDb.TTTK = TTTK.TK_A.ToString();

                    // update thiet ke
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;

                    tk.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_A.ToString(),
                        MOTA = "Duyệt thiết kế điện"
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

        public Message ApproveThietKeListTKToCT(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    var khuvucpo = _kvpoDao.Get(objDb.MAKVPO);

                    if (objDb.TTCT == "CT_RA" || objDb.TTTC == "TC_RA" || objDb.TTNT == "NT_RA")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }
                    else if (objDb.TTNT == "NT_A")
                    {
                        objDb.TTTK = TTTK.TK_A.ToString();

                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }
                    else
                    {
                        // update don dang ky
                        objDb.TTCT = TTCT.CT_N.ToString();
                        objDb.TTTK = TTTK.TK_A.ToString();
                       
                        _rpClass.DonToKeToan(objDb.MADDKPO, khuvucpo.MAKV, objDb.MAKVPO, "", "", "", "DONTKTOCTPO");
                    }

                    // update thiet ke
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (tk == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế",
                                          objUi.TENKH);
                        return msg;
                    }

                    tk.MANVDTK = sManv;
                    tk.NGAYDTK = ngayduyet;

                    tk.NGAYN = DateTime.Now;

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETTHIETKE");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_A.ToString(),
                        MOTA = "Duyệt thiết kế điện"
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

        public Message RejectThietKeList(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
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
                    var tk = _db.THIETKEs.Where(t => t.MADDK.Equals(objDb.MADDKPO)).SingleOrDefault();
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
                        MADON = objDb.MADDKPO,
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

        public Message RejectThietKeListTK(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, String chuthichtk)
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
                    var objDb = Get(objUi.MADDKPO);
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
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
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

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objUi.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOITK");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_RA.ToString(),
                        MOTA = "Từ chối thiết kế điện"
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

        public Message RejectDonThietKeTanChau(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet, String chuthichtk)
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
                    var objDb = Get(objUi.MADDKPO);
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
                    var tk = _db.THIETKEPOs.Where(t => t.MADDKPO.Equals(objDb.MADDKPO)).SingleOrDefault();
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

                    tk.NGAYTRATK = DateTime.Now;
                    tk.MANVTRAKD = sManv;
                    tk.NOIDUNGTRAKD = chuthichtk;

                    _rpClass.HisNgayDangKyBienPo(tk.MADDKPO, sManv, objUi.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                        "", "", "", "", "TUCHOIDONTKTC");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.A.ToString(),
                        MACN = CHUCNANGKYDUYET.CT01.ToString(),
                        MATT = TTTK.TK_P.ToString(),
                        MOTA = "Trả đơn thiết kế điện. (Tân Châu)"
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

        public Message DoAction(List<DONDANGKYPO> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                        var objDb = Get(obj.MADDKPO);
                        if (objDb == null)
                        {
                            // error message
                            msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", obj.TENKH);
                            return msg;
                        }
                        // Set delete info
                        _db.DONDANGKYPOs.DeleteOnSubmit(objDb);

                        var luuvetKyduyet = new LUUVET_KYDUYET
                        {
                            MADON = objDb.MADDKPO,
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
                        var objDb = Get(objUi.MADDKPO);
                        if (objDb == null)
                        {
                            // error message
                            msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký", objUi.TENKH);
                            return msg;
                        }

                        objDb.MADDKPO = objUi.MADDKPO;
                        objDb.MADDKTONG = objUi.MADDKTONG;
                        objDb.TENKH = objUi.TENKH;
                        objDb.DIACHILD = objUi.DIACHILD;
                        objDb.SONHA = objUi.SONHA;
                        objDb.DIENTHOAI = objUi.DIENTHOAI;
                        objDb.MAPHUONG = objUi.MAPHUONG;
                        objDb.DUONGPHUPO = objUi.DUONGPHUPO;
                        objDb.MADPPO = objUi.MADPPO;
                        objDb.MAKVPO = objUi.MAKVPO;
                        objDb.TEN_DC_KHAC = objUi.TEN_DC_KHAC;
                        objDb.MAMDSDPO = objUi.MAMDSDPO;
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
                        objDb.NOIDUNG = objUi.NOIDUNG;

                        var luuvetKyduyet = new LUUVET_KYDUYET
                        {
                            MADON = objUi.MADDKPO,
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

        public Message ApproveChietTinhList(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
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
                    var ct = _db.CHIETTINHs.SingleOrDefault(t => t.MADDK.Equals(objDb.MADDKPO));
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    // Insert into QuyetToan

                    _rpClass.HisNgayDangKyBienPo(ct.MADDK, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETCTKH");

                    _db.SubmitChanges();
                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
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

        public Message ApproveChietTinhPhuTanTanChau(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
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
                    var ct = _db.CHIETTINHs.SingleOrDefault(t => t.MADDK.Equals(objDb.MADDKPO));
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;
                    // Insert into QuyetToan

                    _rpClass.HisNgayDangKyBienPo(ct.MADDK, sManv, objDb.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                            "", "", "", "", "DUYETCTKH");

                    _db.SubmitChanges();
                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
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

        public Message RejectChietTinhList(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
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
                    var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;

                    _rpClass.HisNgayDangKyBienPo(ct.MADDK, sManv, objUi.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                       "", "", "", "", "TUCHOICT");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
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

        public Message RejectChietTinhListTraHSTK(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
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
                    var objDb = Get(objUi.MADDKPO);
                    if (objDb == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đơn đăng ký",
                                          objUi.TENKH);
                        return msg;
                    }

                    // update don dang ky
                    objDb.TTCT = TTCT.CT_RA.ToString();
                    objDb.TTTK = TTTK.TK_P.ToString();

                    // update thiet ke
                    var ct = _db.CHIETTINHs.Where(t => t.MADDK.Equals(objDb.MADDKPO)).SingleOrDefault();
                    if (ct == null)
                    {
                        // error message
                        msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chiết tính",
                                          objUi.TENKH);
                        return msg;
                    }

                    ct.MANVDCT = sManv;
                    ct.NGAYDCT = ngayduyet;

                    _rpClass.HisNgayDangKyBienPo(ct.MADDK, sManv, objUi.MAKVPO, DateTime.Now, DateTime.Now, DateTime.Now,
                       "", "", "", "", "TRAHSKHKT");

                    // luu vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objDb.MADDKPO,
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

        public Message RejectThiCongList(List<DONDANGKYPO> objList, String useragent, String ipAddress, String sManv)
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
                    var objDb = Get(objUi.MADDKPO);
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
                        MADON = objDb.MADDKPO,
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

        public List<DONDANGKYPO> GetListDonChoThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListDonChoThiCongCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        //(d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }


        public List<DONDANGKYPO> GetListDonChoThiCongPB(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later                     
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKYPOs
                         join quyen in _db.DUYET_QUYENs on d.MADDKPO equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() && d.MAKVPO.Equals(areaCode) &&
                               d.TTCT == TTCT.CT_A.ToString() && d.TTHD == TTHD.HD_A.ToString() &&
                               d.TTTC.Equals(TTTC.TC_N) && quyen.MAPB.Equals(mapb)
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListBienBanPB(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKYPOs
                         join quyen in _db.DUYET_QUYENs on d.MADDKPO equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               d.TTCT == TTCT.CT_A.ToString() && d.TTHD == TTHD.HD_A.ToString() &&
                               d.TTTC == TTTC.TC_A.ToString() && d.TTNT == null
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListBienBanPBCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode, String mapb)
        {
            // increase performance later
            /*var result = _db.DONDANGKies.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_N)));*/
            var result = from d in _db.DONDANGKYPOs
                         join quyen in _db.DUYET_QUYENs on d.MADDKPO equals quyen.MADDK
                         where d.TTDK == TTDK.DK_A.ToString() && d.TTTK == TTTK.TK_A.ToString() &&
                               //d.TTCT == TTCT.CT_A.ToString() && 
                               d.TTHD == TTHD.HD_A.ToString() &&
                               d.TTTC == TTTC.TC_A.ToString() && d.TTNT == null
                         select d;

            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListDonThiCong()
        {
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_P)));
            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListDonThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK.Equals(TTDK.DK_A.ToString())) && d.MAKVPO.Equals(areaCode) &&
                                                        (d.TTTK.Equals(TTTK.TK_A.ToString())) &&
                                                        (d.TTCT.Equals(TTCT.CT_A.ToString())) &&
                                                        (d.TTHD.Equals(TTHD.HD_A.ToString())) &&
                                                        (d.TTTC.Equals(TTTC.TC_P.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListDonThiCongCD(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK.Equals(TTDK.DK_A.ToString())) && d.MAKVPO.Equals(areaCode) &&
                                                        (d.TTTK.Equals(TTTK.TK_A.ToString())) &&
                                                        //(d.TTCT.Equals(TTCT.CT_A.ToString())) &&
                                                        (d.TTHD.Equals(TTHD.HD_A.ToString())) &&
                                                        (d.TTTC.Equals(TTTC.TC_P.ToString())));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }

        public List<DONDANGKYPO> GetListDonDaThiCong(String keyword, DateTime? fromDate, DateTime? toDate, String areaCode)
        {
            // increase performance later
            var result = _db.DONDANGKYPOs.Where(d => (d.TTDK == TTDK.DK_A.ToString()) &&
                                                        (d.TTTK == TTTK.TK_A.ToString()) &&
                                                        (d.TTCT == TTCT.CT_A.ToString()) &&
                                                        (d.TTHD == TTHD.HD_A.ToString()) &&
                                                        (d.TTTC.Equals(TTTC.TC_A)));
            if (keyword != null)
                result = result.Where(d => d.MADDKPO.Contains(keyword) ||
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
                result = result.Where(d => d.MAKVPO == areaCode);

            return result.OrderByDescending(d => d.MADDKPO).ToList();
        }
    }
}
