using System;
using System.Collections.Generic;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;

using System.Web.Script.Services;
using System.IO;
using System.Data;
using System.Text;
using System.Collections;

namespace EOSCRM.Web.Common
{
    public class AjaxCRM
    {
        #region Imports DAO
        private readonly NhanVienDao _nvDao = new NhanVienDao();
        private readonly ThietKeDao _tkDao = new ThietKeDao();
        private readonly KhachHangTamLXDao _khtlxDao = new KhachHangTamLXDao();
        private readonly DuongPhoPoDao _dppoDao = new DuongPhoPoDao();
        private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly ChiTietMauBocVatTuDao ctmbvtDao = new ChiTietMauBocVatTuDao();
        private readonly GhiChuMauBocVatTuDao gcmbvtDao = new GhiChuMauBocVatTuDao();
        private readonly DaoLapMauBocVatTuDao dlmbvtDao = new DaoLapMauBocVatTuDao();
        private readonly DaoLapThietKeDao dltkDao = new DaoLapThietKeDao();
        private readonly ChiTietThietKeDao cttkdao = new ChiTietThietKeDao();
        private readonly GhiChuThietKeDao gctkdao = new GhiChuThietKeDao();
        private readonly DaoLapChietTinhDao dlDao = new DaoLapChietTinhDao();
        private readonly ChiTietChietTinhDao ctctDao = new ChiTietChietTinhDao();
        private readonly GhiChuChietTinhDao gcDao = new GhiChuChietTinhDao();
        private readonly DaoLapChietTinhNd117Dao dl117Dao = new DaoLapChietTinhNd117Dao();
        private readonly ChiTietChietTinhNd117Dao ctct117Dao = new ChiTietChietTinhNd117Dao();
        private readonly ChiTietChietTinhSuaChuaDao _chiTietChietTinhSuaChuaDao = new ChiTietChietTinhSuaChuaDao();
        private readonly ChiTietChietTinhSuaChuaNd117Dao _chiTietChietTinhSuaChuaND117Dao =
            new ChiTietChietTinhSuaChuaNd117Dao();
        private readonly GhiChuChietTinhSuaChuaDao _ghiChuChietTinhSuaChuaDao = new GhiChuChietTinhSuaChuaDao();
        private readonly DaoLapChietTinhSuaChuaDao _daoLapChietTinhSuaChuaDao = new DaoLapChietTinhSuaChuaDao();
        private readonly DaoLapChietTinhSuaChuaNd117Dao _daoLapChietTinhSuaChuaND117Dao =
            new DaoLapChietTinhSuaChuaNd117Dao();
        private readonly ChietTinhDao _chietTinhDao = new ChietTinhDao();
        private readonly ChietTinhSuaChuaDao _chietTinhSuaChuaDao = new ChietTinhSuaChuaDao();
        private readonly QuyetToanDao _quyettoandao = new QuyetToanDao();
        private readonly ChiTietQuyetToanDao _chiTietQuyetToanDao = new ChiTietQuyetToanDao();
        private readonly ChiTietQuyetToanNd117Dao _chiTietQuyetToanNd117Dao = new ChiTietQuyetToanNd117Dao();
        private readonly GhiChuQuyetToanDao _chuQuyetToanDao = new GhiChuQuyetToanDao();
        private readonly DaoLapQuyetToanNd117Dao _daoLapQuyetToanNd117Dao = new DaoLapQuyetToanNd117Dao();
        private readonly DaoLapQuyetToanDao _daoLapQuyetToanDao = new DaoLapQuyetToanDao();
        private readonly KhachHangDao _khDao = new KhachHangDao();
        private readonly GhiChiSoDao _gcsDao = new GhiChiSoDao();
        private readonly KhachHangPoDao _khpoDao = new KhachHangPoDao();
        private readonly GhiChiSoPoDao _gcspoDao = new GhiChiSoPoDao();
        private readonly QuyetToanSuaChuaDao qtscDao = new QuyetToanSuaChuaDao();
        private readonly ChiTietQuyetToanSuaChuaDao ctqtscDao = new ChiTietQuyetToanSuaChuaDao();
        private readonly ChiTietQuyetToanSuaChuaND117Dao ctqtsc117Dao = new ChiTietQuyetToanSuaChuaND117Dao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly TruyThuDao _ttDao = new TruyThuDao();
        private readonly HistoriDao _historiDao = new HistoriDao();
        private readonly ReportClass report = new ReportClass();
        private readonly NhanVienVayDao _nvvDao = new NhanVienVayDao();
        private readonly DonDangKyPoDao _ddkpoDao = new DonDangKyPoDao();
        private readonly HopDongPoDao _hdpoDao = new HopDongPoDao();
        private readonly BBNghiemThuPoDao _bbntpoDao = new BBNghiemThuPoDao();
        private readonly DongHoPoDao _dhpoDao = new DongHoPoDao();
        private readonly DonDangKyDao _ddkDao = new DonDangKyDao();
        private readonly HopDongDao _hdDao = new HopDongDao();
        private readonly BBNghiemThuDao _bbntDao = new BBNghiemThuDao();
        private readonly ThiCongDao _tcDao = new ThiCongDao();
        private readonly DongHoDao _dhDao = new DongHoDao();       
       

        #endregion

        #region ReportClass

        [AjaxPro.AjaxMethod]
        public string DonChietTinhBravo(string maddk, string makv, string maddk3, string ghichu, string ghichu2, string ghichu3, string cobien)
        {
            report.DonToKeToan("", makv, "", "", "", "", cobien);

            return "OK";
        }

        #endregion

        #region Methods for MauBocVatTu
        [AjaxPro.AjaxMethod]
        public string UpdateCTMBVT(string maMBVT, string maVT, string sl, string giavt, string gianc, string cb)
        {
            var ctmbvt = ctmbvtDao.Get(maMBVT, maVT);
            if (ctmbvt == null) return DELIMITER.Failed;

            var failedMsg = ctmbvt.SOLUONG + DELIMITER.Delimiter +
                                ctmbvt.GIAVT + DELIMITER.Delimiter +
                                ctmbvt.GIANC + DELIMITER.Delimiter +
                                (ctmbvt.ISCTYDTU.HasValue && ctmbvt.ISCTYDTU.Value ? "true" : "false");

            try
            {
                ctmbvt.SOLUONG = decimal.Parse(sl);
                ctmbvt.GIAVT = decimal.Parse(giavt);
                ctmbvt.TIENVT = ctmbvt.SOLUONG * ctmbvt.GIAVT;
                ctmbvt.GIANC = decimal.Parse(gianc);
                ctmbvt.TIENNC = ctmbvt.SOLUONG * ctmbvt.GIANC;
                ctmbvt.ISCTYDTU = (cb == "true");

                var msg = ctmbvtDao.Update(ctmbvt);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCMBVT(string maGC, string noidung)
        {
            GCMAUBOCVATTU gcmbvt;
            try
            {
                gcmbvt = gcmbvtDao.Get(Int32.Parse(maGC));
                if (gcmbvt == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var oldNoiDung = gcmbvt.NOIDUNG;

            try
            {

                gcmbvt.NOIDUNG = noidung;
                var msg = gcmbvtDao.Update(gcmbvt);

                return msg.MsgType.Equals(MessageType.Error) ? oldNoiDung : DELIMITER.Passed;
            }
            catch
            {
                return oldNoiDung;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPMBVT(string maCP, string nd, string dg,
            string dvt, string sl, string cr, string cc, string hs, string loai)
        {
            DAOLAPMAUBOCVATTU dlmbvt;
            try
            {
                dlmbvt = dlmbvtDao.Get(Int32.Parse(maCP));
                if (dlmbvt == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlmbvt.NOIDUNG + DELIMITER.Delimiter +
                                dlmbvt.DONGIACP + DELIMITER.Delimiter +
                                dlmbvt.DVT + DELIMITER.Delimiter +
                                dlmbvt.SOLUONG + DELIMITER.Delimiter +
                                dlmbvt.CHIEURONG + DELIMITER.Delimiter +
                                dlmbvt.CHIEUCAO + DELIMITER.Delimiter +
                                dlmbvt.HESOCP + DELIMITER.Delimiter +
                                dlmbvt.LOAICP;

            try
            {
                dlmbvt.NOIDUNG = nd;
                dlmbvt.DONGIACP = decimal.Parse(dg);
                dlmbvt.DVT = dvt;
                dlmbvt.SOLUONG = decimal.Parse(sl);
                dlmbvt.CHIEURONG = decimal.Parse(cr);
                dlmbvt.CHIEUCAO = decimal.Parse(cc);
                dlmbvt.HESOCP = decimal.Parse(hs);
                //dlmbvt.THANHTIENCP = decimal.Parse(sl) * decimal.Parse(hs) * decimal.Parse(dg);
                dlmbvt.THANHTIENCP = decimal.Parse(sl) * decimal.Parse(cr) * decimal.Parse(cc) * decimal.Parse(dg);                
                dlmbvt.LOAICP = loai;

                var msg = dlmbvtDao.Update(dlmbvt);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        } 
        #endregion

        #region Methods for ThietKe
        [AjaxPro.AjaxMethod]
        public string UpdateCTTK(string maMBVT, string maVT, string sl, string giavt, string gianc, string cb)
        {
            var cttk = cttkdao.Get(maMBVT, maVT);
            if (cttk == null) return DELIMITER.Failed;

            var failedMsg = cttk.SOLUONG + DELIMITER.Delimiter +
                                cttk.GIAVT + DELIMITER.Delimiter +
                                cttk.GIANC + DELIMITER.Delimiter +
                                (cttk.ISCTYDTU.HasValue && cttk.ISCTYDTU.Value ? "true" : "false");

            try
            {
                cttk.SOLUONG = decimal.Parse(sl);
                cttk.GIAVT = decimal.Parse(giavt);
                cttk.TIENVT = cttk.SOLUONG * cttk.GIAVT;
                cttk.GIANC = decimal.Parse(gianc);
                cttk.TIENNC = cttk.SOLUONG * cttk.GIANC;
                cttk.ISCTYDTU = (cb == "true");

                var msg = cttkdao.Update(cttk);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCTK(string maGC, string noidung)
        {
            GCTHIETKE gctk;
            try
            {
                gctk = gctkdao.Get(Int32.Parse(maGC));
                if (gctk == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var oldNoiDung = gctk.NOIDUNG;

            try
            {

                gctk.NOIDUNG = noidung;
                var msg = gctkdao.Update(gctk);

                return msg.MsgType.Equals(MessageType.Error) ? oldNoiDung : DELIMITER.Passed;
            }
            catch
            {
                return oldNoiDung;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPTK(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string loai)
        {
            DAOLAPTK dltk;
            try
            {
                dltk = dltkDao.Get(Int32.Parse(maCP));
                if (dltk == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dltk.NOIDUNG + DELIMITER.Delimiter +
                                dltk.DONGIACP + DELIMITER.Delimiter +
                                dltk.DVT + DELIMITER.Delimiter +
                                dltk.SOLUONG + DELIMITER.Delimiter +
                                dltk.HESOCP + DELIMITER.Delimiter +
                                dltk.LOAICP;

            try
            {
                dltk.NOIDUNG = nd;
                dltk.DONGIACP = decimal.Parse(dg);
                dltk.DVT = dvt;
                dltk.SOLUONG = decimal.Parse(sl);
                dltk.HESOCP = decimal.Parse(hs);
                dltk.THANHTIENCP = decimal.Parse(sl) * decimal.Parse(hs) * decimal.Parse(dg);
                dltk.LOAICP = loai;

                var msg = dltkDao.Update(dltk);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPTKChieuCaoRong(string maCP, string nd, string dg,
            string dvt, string sl, string cr, string cc, string loai)
        {
            DAOLAPTK dltk;
            try
            {
                dltk = dltkDao.Get(Int32.Parse(maCP));
                if (dltk == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dltk.NOIDUNG + DELIMITER.Delimiter +
                                dltk.DONGIACP + DELIMITER.Delimiter +
                                dltk.DVT + DELIMITER.Delimiter +
                                dltk.SOLUONG + DELIMITER.Delimiter +
                                //dltk.HESOCP + DELIMITER.Delimiter +
                                dltk.CHIEURONG + DELIMITER.Delimiter +
                                dltk.CHIEUCAO + DELIMITER.Delimiter +
                                dltk.LOAICP;
            try
            {
                dltk.NOIDUNG = nd;
                dltk.DONGIACP = decimal.Parse(dg);
                dltk.DVT = dvt;
                dltk.SOLUONG = decimal.Parse(sl);
                //dltk.HESOCP = decimal.Parse(hs);
                dltk.CHIEURONG = decimal.Parse(cr);
                dltk.CHIEUCAO = decimal.Parse(cc);

                dltk.THANHTIENCP = decimal.Parse(sl) * decimal.Parse(cr) * decimal.Parse(cc) * decimal.Parse(dg);
                dltk.LOAICP = loai;

                var msg = dltkDao.Update(dltk);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        } 
        #endregion

        #region Methods for ChietTinh
        [AjaxPro.AjaxMethod]
        public string UpdateCTCT(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = ctctDao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;

            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = ctctDao.Update(ctct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCTCT117(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = ctct117Dao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;

                   
            


            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = ctct117Dao.Update(ctct);

                //_chietTinhDao.UpdateChiPhiForChietTinh(maMBVT);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCCT(string maGC, string noidung)
        {
            GHICHU gcct;
            try
            {
                gcct = gcDao.Get(Int32.Parse(maGC));
                if (gcct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var oldNoiDung = gcct.NOIDUNG;

            try
            {

                gcct.NOIDUNG = noidung;
                var msg = gcDao.Update(gcct);

                return msg.MsgType.Equals(MessageType.Error) ? oldNoiDung : DELIMITER.Passed;
            }
            catch
            {
                return oldNoiDung;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPCT117(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string loai)
        {
            DAOLAP_ND117 dlct;
            try
            {
                dlct = dl117Dao.Get(Int32.Parse(maCP));
                if (dlct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlct.NOIDUNG + DELIMITER.Delimiter +
                                dlct.DONGIACP + DELIMITER.Delimiter +
                                dlct.DVT + DELIMITER.Delimiter +
                                dlct.SOLUONG + DELIMITER.Delimiter +
                                dlct.HESOCP + DELIMITER.Delimiter +
                                dlct.LOAICP;

            try
            {
                dlct.NOIDUNG = nd;
                dlct.DONGIACP = decimal.Parse(dg);
                dlct.DVT = dvt;
                dlct.SOLUONG = decimal.Parse(sl);
                dlct.HESOCP = decimal.Parse(hs);
                dlct.THANHTIENCP = decimal.Parse(dg)*decimal.Parse(sl)*decimal.Parse(hs);
                dlct.LOAICP = loai;

                var msg = dl117Dao.Update(dlct);
                var qt = _daoLapQuyetToanNd117Dao.Get(dlct.MADON);
                if (qt != null)
                {
                    qt.NOIDUNG = dlct.NOIDUNG;
                    qt.DONGIACP = dlct.DONGIACP;
                    qt.DVT = dlct.DVT;
                    qt.SOLUONG = dlct.SOLUONG;
                    qt.HESOCP = dlct.HESOCP;
                    qt.THANHTIENCP = dlct.THANHTIENCP;
                    qt.LOAICP = dlct.LOAICP;
                    _daoLapQuyetToanNd117Dao.Update(qt);
                }
                //_chietTinhDao.UpdateChiPhiForChietTinh(dlct.MADDK );
                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPCT(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string loai)
        {
            DAOLAP dlct;
            try
            {
                dlct = dlDao.Get(Int32.Parse(maCP));
                if (dlct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlct.NOIDUNG + DELIMITER.Delimiter +
                                dlct.DONGIACP + DELIMITER.Delimiter +
                                dlct.DVT + DELIMITER.Delimiter +
                                dlct.SOLUONG + DELIMITER.Delimiter +
                                dlct.HESOCP + DELIMITER.Delimiter +
                                dlct.LOAICP;

            try
            {
                dlct.NOIDUNG = nd;
                dlct.DONGIACP = decimal.Parse(dg);
                dlct.DVT = dvt;
                dlct.SOLUONG = decimal.Parse(sl);
                dlct.HESOCP = decimal.Parse(hs);
                dlct.THANHTIENCP = decimal.Parse(dg) * decimal.Parse(sl) * decimal.Parse(hs);
                dlct.LOAICP = loai;

                var msg = dlDao.Update(dlct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        } 
        #endregion

        #region Methods for ChietTinh Sua chua
        [AjaxPro.AjaxMethod]
        public string UpdateCTCTSC(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = _chiTietChietTinhSuaChuaDao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;

            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = _chiTietChietTinhSuaChuaDao.Update(ctct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCCTCSC(string maGC, string noidung)
        {
            GHICHUSUACHUA gcct;
            try
            {
                gcct = _ghiChuChietTinhSuaChuaDao.Get(Int32.Parse(maGC));
                if (gcct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var oldNoiDung = gcct.NOIDUNG;

            try
            {

                gcct.NOIDUNG = noidung;
                var msg = _ghiChuChietTinhSuaChuaDao.Update(gcct);

                return msg.MsgType.Equals(MessageType.Error) ? oldNoiDung : DELIMITER.Passed;
            }
            catch
            {
                return oldNoiDung;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPCTSC(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string tt, string loai)
        {
            DAOLAPSUACHUA dlct;
            try
            {
                dlct = _daoLapChietTinhSuaChuaDao.Get(Int32.Parse(maCP));
                if (dlct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlct.NOIDUNG + DELIMITER.Delimiter +
                                dlct.DONGIACP + DELIMITER.Delimiter +
                                dlct.DVT + DELIMITER.Delimiter +
                                dlct.SOLUONG + DELIMITER.Delimiter +
                                dlct.HESOCP + DELIMITER.Delimiter +
                                dlct.LOAICP;

            try
            {
                dlct.NOIDUNG = nd;
                dlct.DONGIACP = decimal.Parse(dg);
                dlct.DVT = dvt;
                dlct.SOLUONG = decimal.Parse(sl);
                dlct.HESOCP = decimal.Parse(hs);
                tt = (decimal.Parse(dg)*decimal.Parse(sl)*decimal.Parse(hs)).ToString();
                dlct.THANHTIENCP = decimal.Parse(tt);
                dlct.LOAICP = loai;

                var msg = _daoLapChietTinhSuaChuaDao.Update(dlct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }


        [AjaxPro.AjaxMethod]
        public string UpdateCTCTSCND117(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = _chiTietChietTinhSuaChuaND117Dao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;

            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = _chiTietChietTinhSuaChuaND117Dao.Update(ctct);

                _chietTinhSuaChuaDao.UpdateChiPhiForChietTinh(maMBVT);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]
        public string UpdateCPCTSCND117(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string tt, string loai)
        {
            DAOLAPSUACHUA_ND117 dlct;
            try
            {
                dlct = _daoLapChietTinhSuaChuaND117Dao.Get(Int32.Parse(maCP));
                if (dlct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlct.NOIDUNG + DELIMITER.Delimiter +
                                dlct.DONGIACP + DELIMITER.Delimiter +
                                dlct.DVT + DELIMITER.Delimiter +
                                dlct.SOLUONG + DELIMITER.Delimiter +
                                dlct.HESOCP + DELIMITER.Delimiter +
                                dlct.LOAICP;

            try
            {
                dlct.NOIDUNG = nd;
                dlct.DONGIACP = decimal.Parse(dg);
                dlct.DVT = dvt;
                dlct.SOLUONG = decimal.Parse(sl);
                dlct.HESOCP = decimal.Parse(hs);
                tt = (decimal.Parse(dg) * decimal.Parse(sl) * decimal.Parse(hs)).ToString();
                dlct.THANHTIENCP = decimal.Parse(tt);
                dlct.LOAICP = loai;

                var msg = _daoLapChietTinhSuaChuaND117Dao.Update(dlct);
                _chietTinhSuaChuaDao.UpdateChiPhiForChietTinh(dlct.MADON );

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }
        #endregion

        #region Methods for Quyet toan
        [AjaxPro.AjaxMethod]// update chi tiết quyet toán phần vật tư miễn phí
        public string UpdateCTCTQT(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = _chiTietQuyetToanDao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;

            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = _chiTietQuyetToanDao.Update(ctct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }
        [AjaxPro.AjaxMethod]// update chi tiet quyet toan phần 117
        public string UpdateCTCT117QT(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = _chiTietQuyetToanNd117Dao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;





            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;
                
                var msg = _chiTietQuyetToanNd117Dao.Update(ctct);

                _quyettoandao.UpdateChiPhiForQuyetToan(maMBVT);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }
        
        [AjaxPro.AjaxMethod]// update chi tiết ghi chú quyết toán
        public string UpdateGCCTQT(string maGC, string noidung)
        {
            GHICHUQUYETOAN gcct;
            try
            {
                gcct = _chuQuyetToanDao.Get(Int32.Parse(maGC));
                if (gcct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var oldNoiDung = gcct.NOIDUNG;

            try
            {

                gcct.NOIDUNG = noidung;
                var msg = _chuQuyetToanDao.Update(gcct);

                return msg.MsgType.Equals(MessageType.Error) ? oldNoiDung : DELIMITER.Passed;
            }
            catch
            {
                return oldNoiDung;
            }
        }

        [AjaxPro.AjaxMethod]// update chi phí đào lắp quyết toán 117
        public string UpdateCPCT117QT(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string loai)
        {
            DAOLAPQUYETTOAN_ND117 dlct;
           
            try
            {
                dlct = _daoLapQuyetToanNd117Dao.Get(Int32.Parse(maCP));
                if (dlct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlct.NOIDUNG + DELIMITER.Delimiter +
                                dlct.DONGIACP + DELIMITER.Delimiter +
                                dlct.DVT + DELIMITER.Delimiter +
                                dlct.SOLUONG + DELIMITER.Delimiter +
                                dlct.HESOCP + DELIMITER.Delimiter +
                                dlct.LOAICP;

            try
            {
                dlct.NOIDUNG = nd;
                dlct.DONGIACP = decimal.Parse(dg);
                dlct.DVT = dvt;
                dlct.SOLUONG = decimal.Parse(sl);
                dlct.HESOCP = decimal.Parse(hs);
                dlct.THANHTIENCP = decimal.Parse(dg) * decimal.Parse(sl) * decimal.Parse(hs);
                dlct.LOAICP = loai;

                var msg = _daoLapQuyetToanNd117Dao.Update(dlct);
                //_chietTinhDao.UpdateChiPhiForChietTinh(dlct.MADDK);
                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }

        [AjaxPro.AjaxMethod]// update chi phí đào lắp Quyết toán 
        public string UpdateCPCTQT(string maCP, string nd, string dg,
            string dvt, string sl, string hs, string loai)
        {
            DAOLAPQUYETTOAN dlct;
            try
            {
                dlct = _daoLapQuyetToanDao.Get(Int32.Parse(maCP));
                if (dlct == null) return DELIMITER.Failed;
            }
            catch
            {
                return DELIMITER.Failed;
            }

            var failedMsg = dlct.NOIDUNG + DELIMITER.Delimiter +
                                dlct.DONGIACP + DELIMITER.Delimiter +
                                dlct.DVT + DELIMITER.Delimiter +
                                dlct.SOLUONG + DELIMITER.Delimiter +
                                dlct.HESOCP + DELIMITER.Delimiter +
                                dlct.LOAICP;

            try
            {
                dlct.NOIDUNG = nd;
                dlct.DONGIACP = decimal.Parse(dg);
                dlct.DVT = dvt;
                dlct.SOLUONG = decimal.Parse(sl);
                dlct.HESOCP = decimal.Parse(hs);
                dlct.THANHTIENCP = decimal.Parse(dg) * decimal.Parse(sl) * decimal.Parse(hs);
                dlct.LOAICP = loai;

                var msg = _daoLapQuyetToanDao.Update(dlct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }
        #endregion

        #region Methods for CapNhatSoBo

        [AjaxPro.AjaxMethod]
        public string UpdateSoBoThoaiSonDBDP(string idkh, string sttCu, string sttMoi, string tenkh, string sonha, string madp, string madb, string manv)
        {
            var kh = _khDao.Get(idkh);

            if (kh == null)
                return DELIMITER.Failed;

            int stttd = int.Parse(sttMoi);

            kh.STT = int.Parse(sttMoi);
            kh.TENKH = tenkh.Trim();
            kh.SONHA = sonha.Trim();
            kh.MADP = madp.Trim();
            kh.MADB = madb.Trim();

            report.InTachDuong(99, 99, madp, idkh, madb);

            var msg = _khDao.UpdateSoBoTachDuongTSon(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM());            

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateSoBoThoaiSon(string idkh, string sttCu, string sttMoi, string tenkh, string sonha, string madp, string madb, string manv)
        { 
            var kh = _khDao.Get(idkh);

            if (kh == null)
                return DELIMITER.Failed;

            int stttd = int.Parse(sttMoi);

            kh.STT = int.Parse(sttMoi);
            kh.TENKH = tenkh.Trim();
            kh.SONHA = sonha.Trim();
            kh.MADP = madp.Trim();
            kh.MADB = madb.Trim();

            /*
            string lydothay = "Tách đường";
            int thangm = DateTime.Now.Month;
            int namm = DateTime.Now.Year;
            report.UPKHDANHBO(kh.IDKH, madp.Trim(), madb.Trim(), "", thangm, namm, lydothay);            
            */

            //var msg = _khDao.UpdateSoBo(kh, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), manv);
            //var msg = _khDao.UpdateSoBoDungPhim(kh, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), manv);
            //report.UpdateTieuThu(idkh, madp.Trim(), madb.Trim());

            report.InTachDuong(stttd, 99, madp, idkh, "upstt");

            var msg = _khDao.UpdateSoBoTachDuongTSon(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM());
            
            //string lydothay = "Tách đường";
            //int thangm = DateTime.Now.Month;
            //int namm = DateTime.Now.Year;
            //report.UPKHDANHBO(kh.IDKH, madp.Trim(), madb.Trim(), "", thangm, namm, lydothay);

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateSoBo(string idkh, string sttCu, string sttMoi, string tenkh, string sonha, string madp, string madb, string manv)
        {
            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int stt;
            int stt2;

            try
            {
                stt = int.Parse(sttCu);
                stt2 = int.Parse(sttMoi);
            }
            catch
            {
                return DELIMITER.Failed;
            }

            // nếu đổi số tt, cập nhật nguyên danh sách
            if (stt != stt2)
            {
                // số thứ tự mới giảm
                if (stt > stt2)
                {
                    var list = _khDao.GetList(kh.MADP, stt2, stt - 1);
                    foreach (var kh2 in list)
                    {
                        kh2.STT = kh2.STT + 1;
                        _khDao.UpdateSoBo(kh2, CommonFunc.GetComputerName(),
                            CommonFunc.GetIpAdddressComputerName(), manv);
                    }
                }
                else // số thứ tự mới tăng
                {
                    var list = _khDao.GetList(kh.MADP, stt + 1, stt2);
                    foreach (var kh2 in list)
                    {
                        kh2.STT = kh2.STT - 1;
                        _khDao.UpdateSoBo(kh2, CommonFunc.GetComputerName(),
                            CommonFunc.GetIpAdddressComputerName(), manv);
                    }
                }
            }

            kh.STT = stt2;
            kh.TENKH = tenkh.Trim();
            kh.SONHA = sonha.Trim();
            kh.MADP = madp.Trim();
            kh.MADB = madb.Trim();
            /*
            string lydothay = "Tách đường";
            int thangm = DateTime.Now.Month;
            int namm = DateTime.Now.Year;
            report.UPKHDANHBO(kh.IDKH, madp.Trim(), madb.Trim(), "", thangm, namm, lydothay);            
            */

            //var msg = _khDao.UpdateSoBo(kh, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), manv);
            var msg = _khDao.UpdateSoBoDungPhim(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manv);

            report.UpdateTieuThu(idkh, madp.Trim(), madb.Trim());
            

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateSoBoTachDuong(string idkh, string sttCu, string sttMoi, string tenkh, string sonha, string madp, string madb)
        {
            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int stt;
            int stt2;

            try
            {
                stt = int.Parse(sttCu);
                stt2 = int.Parse(sttMoi);
            }
            catch
            {
                return DELIMITER.Failed;
            }

            // nếu đổi số tt, cập nhật nguyên danh sách
           /* if (stt != stt2)
            {
                // số thứ tự mới giảm
                if (stt > stt2)
                {
                    var list = _khDao.GetList(kh.MADP, stt2, stt - 1);
                    foreach (var kh2 in list)
                    {
                        kh2.STT = kh2.STT + 1;
                        //_khDao.UpdateSoBo(kh2, CommonFunc.GetComputerName(),  CommonFunc.GetIpAdddressComputerName(), manv);
                    }
                }
                else // số thứ tự mới tăng
                {
                    var list = _khDao.GetList(kh.MADP, stt + 1, stt2);
                    foreach (var kh2 in list)
                    {
                        kh2.STT = kh2.STT - 1;
                        //_khDao.UpdateSoBo(kh2, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), manv);
                    }
                }
            }*/

            kh.STT = stt2;
            kh.TENKH = tenkh.Trim();
            kh.SONHA = sonha.Trim();
            kh.MADP = madp.Trim();
            kh.MADB = madb.Trim();

            var msg = _khDao.UpdateSoBoTachDuong(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM());

            string lydothay = "Tách đường";
            int thangm = DateTime.Now.Month;
            int namm = DateTime.Now.Year;
            report.UPKHDANHBO(kh.IDKH, madp.Trim(), madb.Trim(), " ", thangm, namm, lydothay);
            
            //report.UpdateTieuThu(idkh, madp.Trim(), madb.Trim());            
            
            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateMaDoan(string idkh, string madoan, string manv)
        {
            var kh = _khDao.Get(idkh);
            if (kh == null)
                return "Cập nhật mã đoạn không thành công. Khách hàng không tồn tại.";

            kh.MADOAN = madoan;

            var msg = _khDao.UpdateMaDoan(kh, CommonFunc.GetComputerName(),
                CommonFunc.GetIpAdddressComputerName(), manv);

            return msg.MsgType.Equals(MessageType.Error) ? ResourceLabel.Get(msg) : DELIMITER.Passed;
        }        
        
        
        #endregion

        #region Methods for QuyetToanSuaChua
        [AjaxPro.AjaxMethod]// update chi tiết quyết toán sửa chữa phần vật tư thanh toán
        public string UpdateCTQTSC117(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = ctqtsc117Dao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;





            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = ctqtsc117Dao.Update(ctct);

                _quyettoandao.UpdateChiPhiForQuyetToan(maMBVT);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }
        [AjaxPro.AjaxMethod]// update chi tiet quyet toan sữa chữa phần miễn phí
        public string UpdateCTQTSC(string maMBVT, string maVT, string sl,
            string giavt, string gianc)
        {
            var ctct = ctqtscDao.Get(maMBVT, maVT);
            if (ctct == null) return DELIMITER.Failed;

            var failedMsg = ctct.SOLUONG + DELIMITER.Delimiter +
                                ctct.GIAVT + DELIMITER.Delimiter +
                                ctct.GIANC;

            try
            {
                ctct.SOLUONG = decimal.Parse(sl);
                ctct.GIAVT = decimal.Parse(giavt);
                ctct.TIENVT = ctct.SOLUONG * ctct.GIAVT;
                ctct.GIANC = decimal.Parse(gianc);
                ctct.TIENNC = ctct.SOLUONG * ctct.GIANC;

                var msg = ctqtscDao.Update(ctct);

                return msg.MsgType.Equals(MessageType.Error) ? failedMsg : DELIMITER.Passed;
            }
            catch
            {
                return failedMsg;
            }
        }
        #endregion

        #region Methods for GhiChiSo

        [AjaxPro.AjaxMethod]
        public string UpdateDotInPo(string madp, string idmadotin, string manv, string tennv)
        {
            //if (tthaighi == "THAY")
            //    return DELIMITER.Passed;
            //report.KhachHangTT(idkh, tthaighi);
            //var kh = _khDao.Get(idkh);
            //if (kh == null)
            //    return DELIMITER.Failed;            

            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi            
            //var obj = new GHICHISO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv);            

            var msg = _dpDao.UpDotInInLichGCS(madp, idmadotin, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manv);

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateDotIn(string madp, string idmadotin, string manv, string tennv)
        {
            //if (tthaighi == "THAY")
            //    return DELIMITER.Passed;
            //report.KhachHangTT(idkh, tthaighi);
            //var kh = _khDao.Get(idkh);
            //if (kh == null)
            //    return DELIMITER.Failed;            
                        
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi            
            //var obj = new GHICHISO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv);            

            var msg = _dpDao.UpDotInInLichGCS(madp, idmadotin, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manv);            

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCS(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr ,string klttStr,
            string ghichu, string tthaighi, string manv)
        {
            if (tthaighi == "THAY")
                return DELIMITER.Passed;

            report.KhachHangTT(idkh, tthaighi);

            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int csd, csc, mtt, nam, thang, kltt;

            try
            {
                csd = int.Parse(csdStr);
                csc = int.Parse(cscStr);
                mtt = int.Parse(mttStr);
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                kltt = int.Parse(klttStr);
            }
            catch
            {
                return DELIMITER.Failed;
            }

            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            //var obj = new GHICHISO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv);
            var obj = new GHICHISO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv);

            var msg = _gcsDao.Update(obj);

            #region Lich su truy thu
           
            /*var truythulichsu = new TRUYTHUHISTORY
            {
                IDKH = idkh,
                //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                IPADDRESS = CommonFunc.GetMACAddress1(),
               
                MANV = manv,
                //USERAGENT = CommonFunc.GetComputerName(),
                USERAGENT = CommonFunc.GetComputerName(),
                NGAYTHUCHIEN = DateTime.Now,
                NAM = int.Parse(namStr),
                THANG = int.Parse(thangStr),
                CHISODAUMOI = int.Parse(csdStr),
                CHISOCUOIMOI = int.Parse(cscStr),
                KLTIEUTHUMOI = int.Parse(klttStr),
                
                MOTA = "Ghi sai chỉ số"
            };
            _ttDao.Insert(truythulichsu);*/
            
            #endregion            

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCSTBTT(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr, string klttStr,
            string ghichu, string tthaighi, string manv)
        {
            if (tthaighi == "THAY")
                return DELIMITER.Passed;

            report.KhachHangTT(idkh, tthaighi);

            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int csd, csc, mtt, nam, thang, kltt;

            try
            {
                csd = int.Parse(csdStr);
                csc = int.Parse(cscStr);
                mtt = int.Parse(mttStr);
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                kltt = int.Parse(klttStr);
            }
            catch
            {
                return DELIMITER.Failed;
            }

            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            var obj = new GHICHISO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv);

            var msg = _gcsDao.Update(obj);

            #region Lich su truy thu

            /*var truythulichsu = new TRUYTHUHISTORY
            {
                IDKH = idkh,
                //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                IPADDRESS = CommonFunc.GetMACAddress1(),
               
                MANV = manv,
                //USERAGENT = CommonFunc.GetComputerName(),
                USERAGENT = CommonFunc.GetComputerName(),
                NGAYTHUCHIEN = DateTime.Now,
                NAM = int.Parse(namStr),
                THANG = int.Parse(thangStr),
                CHISODAUMOI = int.Parse(csdStr),
                CHISOCUOIMOI = int.Parse(cscStr),
                KLTIEUTHUMOI = int.Parse(klttStr),
                
                MOTA = "Ghi sai chỉ số"
            };
            _ttDao.Insert(truythulichsu);*/

            #endregion

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCSPo(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr, string klttStr,
            string ghichu, string tthaighi, string manv)
        {

            report.KhachHangTTPo(idkh, tthaighi);

            var kh = _khpoDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int csd, csc, mtt, nam, thang, kltt;

            try
            {
                csd = int.Parse(csdStr);
                csc = int.Parse(cscStr);
                mtt = int.Parse(mttStr);
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                kltt = int.Parse(klttStr);
            }
            catch
            {
                return DELIMITER.Failed;
            }

            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            var obj = new GHICHISOPO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv, null, null, null);

            var msg = _gcspoDao.Update(obj);

            #region Lich su truy thu

            /*var truythulichsu = new TRUYTHUHISTORY
            {
                IDKH = idkh,
                //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                IPADDRESS = CommonFunc.GetMACAddress1(),
               
                MANV = manv,
                //USERAGENT = CommonFunc.GetComputerName(),
                USERAGENT = CommonFunc.GetComputerName(),
                NGAYTHUCHIEN = DateTime.Now,
                NAM = int.Parse(namStr),
                THANG = int.Parse(thangStr),
                CHISODAUMOI = int.Parse(csdStr),
                CHISOCUOIMOI = int.Parse(cscStr),
                KLTIEUTHUMOI = int.Parse(klttStr),
                
                MOTA = "Ghi sai chỉ số"
            };
            _ttDao.Insert(truythulichsu);*/

            #endregion

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        
        [AjaxPro.AjaxMethod]
        public string UpdateNhanVienVayKy(string manvvStr, string namStr, string thangStr, string tiengocStr, string tienlaiStr,
            string htttStr, string thanhtoanStr)
        {            

            var nvv = _nvvDao.Get(manvvStr);
            if (nvv == null)
                return DELIMITER.Failed;

            int nam, thang;
            decimal tiengoc, tienlai, tongtien;

            try
            {
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                tiengoc = decimal.Parse(tiengocStr);
                tienlai = decimal.Parse(tienlaiStr);
                tongtien = tiengoc + tienlai;
            }
            catch
            {
                return DELIMITER.Failed;
            }
                        
            //var obj = new VAYKYNHANVIEN(manvvStr, nam, thang, tiengoc, tienlai, htttStr, tongtien, null, 
            //    thanhtoanStr, null, null, null, null, null, null);

            report.VayUpkyNhanVien(manvvStr, nam, thang, tiengoc, tienlai, htttStr, tongtien, thanhtoanStr);

            #region Lich su truy thu

            /*var truythulichsu = new TRUYTHUHISTORY
            {
                IDKH = idkh,
                //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                IPADDRESS = CommonFunc.GetMACAddress1(),
               
                MANV = manv,
                //USERAGENT = CommonFunc.GetComputerName(),
                USERAGENT = CommonFunc.GetComputerName(),
                NGAYTHUCHIEN = DateTime.Now,
                NAM = int.Parse(namStr),
                THANG = int.Parse(thangStr),
                CHISODAUMOI = int.Parse(csdStr),
                CHISOCUOIMOI = int.Parse(cscStr),
                KLTIEUTHUMOI = int.Parse(klttStr),
                
                MOTA = "Ghi sai chỉ số"
            };
            _ttDao.Insert(truythulichsu);*/

            #endregion            
            var msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Đã thu ");

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCS2(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr, string klttStr,
            string ghichu, string tthaighi, string manv)
        {

            report.KhachHangTT(idkh, tthaighi);

            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int csd, csc, mtt, nam, thang, kltt;

            try
            {
                csd = int.Parse(csdStr);
                csc = int.Parse(cscStr);
                mtt = int.Parse(mttStr);
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                kltt = int.Parse(klttStr);
            }
            catch
            {
                return DELIMITER.Failed;
            }                      
                
            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            var obj = new GHICHISO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv);

            var msg = _gcsDao.Update(obj);


            int thanght = DateTime.Now.Month;
            int namht = DateTime.Now.Year;
            var khmoi = _khDao.Get(idkh);
            int thangkhm = khmoi.KYKHAITHAC.Value.Month;
            int namkhm = khmoi.KYKHAITHAC.Value.Year;

            if (thangkhm == thanght && namkhm == namht)
            {
                report.UPKHTTCOBIEN(idkh, "", "", thanght, namht, "", "", "", "", "", Convert.ToDecimal(mtt), csd, csc, "UPCSTTVAOKHMOI");
            }

            #region Lich su 
           
             var histori = new HISTORY
             {                 
                 MANV = manv,
                 NGAYTHUCHIEN = DateTime.Now,
                 MOTA = "Cập nhật lại chỉ số",
                 USERAGENT = CommonFunc.GetComputerName(),
                 IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                 PHYSICAL0 = CommonFunc.GetMACAddress0(),
                 PHYSICAL1 = CommonFunc.GetMACAddress1(),
                 IDKH = idkh,
                 CHISODAU = int.Parse(csdStr),
                 CHISOCUOI = int.Parse(cscStr),
                 KLTIEUTHU = int.Parse(klttStr),
                 MTRUYTHU = int.Parse(mttStr),
                 NAM = int.Parse(namStr),
                 THANG = int.Parse(thangStr)                 
             };
             _historiDao.Insert(histori);
            
             #endregion            

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateGCS2Po(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr, string klttStr,
            string ghichu, string tthaighi, string manv)
        {

            report.KhachHangTTPo(idkh, tthaighi);

            var kh = _khpoDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int csd, csc, mtt, nam, thang, kltt;

            try
            {
                csd = int.Parse(csdStr);
                csc = int.Parse(cscStr);
                mtt = int.Parse(mttStr);
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                kltt = int.Parse(klttStr);
            }
            catch
            {
                return DELIMITER.Failed;
            }

            if (manv != "nguyen")
            {
                var kyghimay = new DateTime(nam, thang, 1);
                bool khoaso = _gcspoDao.IsLockTinhCuocMADP(kyghimay, kh.MADPPO);
                if (khoaso == true)
                {
                    return DELIMITER.Failed;
                }
            }
            
            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            var obj = new GHICHISOPO(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt, tthaighi, manv, null, null, null);

            var msg = _gcspoDao.Update(obj);
            
            //report.KhachHangTTPo(idkh, tthaighi);

            int thanght = DateTime.Now.Month;
            int namht = DateTime.Now.Year;

            var khpomoi = _khpoDao.Get(idkh);
            int thangkhm = khpomoi.KYKHAITHAC.Month;
            int namkhm = khpomoi.KYKHAITHAC.Year;

            if (thangkhm == thanght && namkhm == namht)
            {
                report.UPKHTTCOBIEN(idkh, "", "", thanght, namht, "", "", "", "", "", Convert.ToDecimal(mtt), csd, csc, "UPCSTTVAOKHMOIPO");
            }

            #region Lich su

            var histori = new HISTORY
            {
                MANV = manv,
                NGAYTHUCHIEN = DateTime.Now,
                MOTA = "Cập nhật lại chỉ số điện",
                USERAGENT = CommonFunc.GetComputerName(),
                IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                PHYSICAL0 = CommonFunc.GetMACAddress0(),
                PHYSICAL1 = CommonFunc.GetMACAddress1(),
                IDKH = idkh,
                CHISODAU = int.Parse(csdStr),
                CHISOCUOI = int.Parse(cscStr),
                KLTIEUTHU = int.Parse(klttStr),
                MTRUYTHU = int.Parse(mttStr),
                NAM = int.Parse(namStr),
                THANG = int.Parse(thangStr)
            };
            _historiDao.Insert(histori);

            #endregion

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        [AjaxPro.AjaxMethod]
        public string UpdateDB(string idkh, string namStr, string thangStr, string mdpStr, string mdbStr, string ghichu)
        {           

            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int  nam, thang;

            try
            {                
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);                
            }
            catch
            {
                return DELIMITER.Failed;
            }

            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            var obj = new GHICHISODB(idkh, null, mdpStr, mdbStr, nam, thang, null, null);

            var msg = _gcsDao.UpdateDB(obj);

           

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }

        #endregion

        #region Methods for Truy thu vi pham
        [AjaxPro.AjaxMethod]
        public string UpdateGCSTT(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr, string klttStr,
            string ghichu, 
            string tthaighi, string manv)
        {
            var kh = _khDao.Get(idkh);
            if (kh == null)
                return DELIMITER.Failed;

            int csd, csc, nam, thang, kltt, mtt, ttt;

            try
            {
                csd = int.Parse(csdStr);
                csc = int.Parse(cscStr);
                nam = int.Parse(namStr);
                thang = int.Parse(thangStr);
                kltt = int.Parse(klttStr);
                mtt = int.Parse(mttStr);
               
            }
            catch
            {
                return DELIMITER.Failed;
            }

            //idkh, soDb, madp, nam, thang, sonha, tenkh, chisodau, chisocuoi, kltieuthu, tthaighi, manv_cs
            // hàm updatelist ghi chỉ số chỉ quan tâm đến một số field cần thiết thôi
            var obj = new GHICHISOTT(idkh, null, null, nam, thang, null, null, csd, csc, mtt, kltt,  tthaighi, manv);

            var msg = _gcsDao.UpdateTT(obj);

            #region Lich su truy thu

            /* var truythulichsu = new TRUYTHUHISTORY
            {
                IDKH = idkh,
                IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                MANV = manv,
                USERAGENT = CommonFunc.GetComputerName(),
                NGAYTHUCHIEN = DateTime.Now,
                NAM = int.Parse(namStr),
                THANG = int.Parse(thangStr),
                CHISODAUMOI = int.Parse(csdStr),
                CHISOCUOIMOI = int.Parse(cscStr),
                KLTIEUTHUMOI = int.Parse(klttStr),
                
                MOTA = "Ghi sai chỉ số"
            };
            _ttDao.Insert(truythulichsu);*/

            #endregion

            return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
        }
        #endregion

        
        [AjaxPro.AjaxMethod]
        [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
        [System.Web.Services.WebMethod]
        public static List<double> GetLatLong()
        {

            List<double> all = new List<double>();
            all.Add(52.511467);
            all.Add(13.447179);   
            
            //all = dc.TopCompanies.ToList();
            
            return all;

            //ArrayList  mm = new ArrayList();
                   

            //var neighborhoods = [new google.maps.LatLng(52.511467, 13.447179),
           //report.LatLongAll();            
        }

        [ScriptMethod]
        [AjaxPro.AjaxMethod]
        public static string GetCurrentTime()
        {
            var mm = "Hello ";
            return mm;
         
        }

        [AjaxPro.AjaxMethod]
        public string InKhachHangMoiCD(string idkhStr, string makvStr, string csdStr, string cscStr, string manvStr,
                                        string thangStr, string namStr, string madp, string madb)
        {
            //if (tthaighi == "THAY")
            //    return DELIMITER.Passed;
            //report.KhachHangTT(idkh, tthaighi);           

            int csd, csc, kltieuthu, thang, nam;
            csd = Convert.ToInt32(csdStr);
            csc = Convert.ToInt32(cscStr);
            thang = Convert.ToInt32(thangStr);
            nam = Convert.ToInt32(namStr);
            kltieuthu = csc - csd;

            if (_khDao.Get(idkhStr) != null)// update chisocuoi khach hang moi
            {
                //var truythulichsu2 = new TRUYTHUHISTORY
                //{
                //    IDKH = "55555",
                //    //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                //    IPADDRESS = CommonFunc.GetMACAddress1(),

                //    MANV = manvStr,
                //    //USERAGENT = CommonFunc.GetComputerName(),
                //    USERAGENT = CommonFunc.GetComputerName(),
                //    NGAYTHUCHIEN = DateTime.Now,
                //    NAM = int.Parse(namStr),
                //    THANG = int.Parse(thangStr),
                //    CHISODAUMOI = Convert.ToInt32(csdStr),
                //    CHISOCUOIMOI = Convert.ToInt32(cscStr),
                //    KLTIEUTHUMOI = 12,

                //    MOTA = "Ghi sai chỉ số222"
                //};
                //_ttDao.Insert(truythulichsu2);

                string kykt = "01/" + thang + "/" + nam;
                var kh = new KHACHHANG
                {
                    IDKH = idkhStr,
                    MADP = madp,
                    MADB = madb,
                    CHISODAU = csd,
                    CHISOCUOI = csc
                };

                var msg = _khDao.UpdateKHMCSC(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);

                var tieuthum = UpdateGCS(idkhStr, namStr, thangStr, csdStr, cscStr, "0", kltieuthu.ToString(), "", "GDH_BT", "admin");

                return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
            }
            else
            {
                //var truythulichsu2 = new TRUYTHUHISTORY
                //{
                //    IDKH = "33333",
                //    //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                //    IPADDRESS = CommonFunc.GetMACAddress1(),

                //    MANV = manvStr,
                //    //USERAGENT = CommonFunc.GetComputerName(),
                //    USERAGENT = CommonFunc.GetComputerName(),
                //    NGAYTHUCHIEN = DateTime.Now,
                //    NAM = int.Parse(namStr),
                //    THANG = int.Parse(thangStr),
                //    CHISODAUMOI = Convert.ToInt32(csdStr),
                //    CHISOCUOIMOI = Convert.ToInt32(cscStr),
                //    KLTIEUTHUMOI = 12,

                //    MOTA = "Ghi sai chỉ số222"
                //};
                //_ttDao.Insert(truythulichsu2);

                var donkh = _khDao.GetMADDK(idkhStr);
                if (donkh != null)
                {
                    return DELIMITER.Passed;
                }

                var don = _ddkDao.Get(idkhStr);
                var hop = _hdDao.Get(idkhStr);
                var bb = _bbntDao.GetMADDK(idkhStr);
                var tc = _tcDao.Get(idkhStr);
                var dh = _dhDao.Get(tc != null ? tc.MADH : "");


                string vitri = _tkDao.Get(hop.MADDK.ToString()) != null ? (_tkDao.Get(hop.MADDK.ToString()).DUONGHEMTK != null ? _tkDao.Get(hop.MADDK.ToString()).DUONGHEMTK : "") + "," +
                    (_tkDao.Get(hop.MADDK.ToString()).DIACHITK != null ? _tkDao.Get(hop.MADDK.ToString()).DIACHITK : "") + "," +
                    (_tkDao.Get(hop.MADDK.ToString()).VITRIDHTK != null ? _tkDao.Get(hop.MADDK.ToString()).VITRIDHTK : "") : "";


                string kykt = "01/" + thang + "/" + nam;
                var khtlx = new KHACHHANGTAMLX
                {
                    IDKH = _khtlxDao.NewId(),

                    MADB = hop.MADB != null ? madb : "",
                    MADP = hop.MADP != null ? madp : "",
                    DUONGPHU = "",
                    MALKHDB = "D",
                    MAMDSD = hop.MAMDSD != null ? hop.MAMDSD : "",
                    MADDK = idkhStr,
                    SOHD = hop.SOHD,
                    MABG = null,
                    MAHOTRO = "C",
                    MAPHUONG = hop.MADP != null ? hop.MADP.ToUpper().Trim().Substring(1, 1) : "",
                    TENKH = don.TENKH,
                    SONHA = don.SONHA != null ? don.SONHA : "",
                    MST = hop.MST != null ? hop.MST : "",
                    MAHTTT = "TM",
                    STK = "",
                    SDT = don.DIENTHOAI != null ? don.DIENTHOAI : "",
                    TTSD = "",
                    KOPHINT = false,
                    THUYLK = "15",
                    NGAYGNHAP = DateTime.Now,
                    VAT = true,
                    KHONGTINH117 = false,
                    MATT = "GDH_BT",
                    KYKHAITHAC = DateTimeUtil.GetVietNamDate(kykt),
                    GHI2THANG1LAN = "0",
                    THUHO = "0",
                    MAKV = makvStr,
                    MAKVDN = "O",
                    //MACQ = txtCQ.Text.Trim();
                    SOHO = 1,
                    SONK = 4,
                    SODINHMUC = 1,
                    ISDINHMUC = false,
                    ISVIPH = false,
                    NGAYHT = tc.NGAYHT,
                    //kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                    MALDH = dh.MALDH != null ? dh.MALDH : "",
                    MADH = dh.SONO != null ? dh.MADH : "",
                    KLKHOAN = 4,
                    STT = 1,
                    SDInfo_INHOADON = false,
                    TENKH_INHOADON = "",
                    DIACHI_INHOADON = "",
                    NAMBDKT = DateTime.Now.Year,
                    THANGBDKT = DateTime.Now.Month,
                    CHISODAU = csd,
                    ISHONGHEO = false,
                    CHISOCUOI = csc,
                    XOABOKH = false,
                    ISTACHDUONG = false,
                    ISDINHMUCTAM = false,
                    MTRUYTHU = 0,
                    VITRI = vitri
                };

                //var msg = _khDao.InsertTS2(kh, CommonFunc.GetComputerName(), CommonFunc.GetIpAdddressComputerName(), manvStr);
                              
                //khtlx.VITRI = lbVITRI.Text.Trim();

                var msg = _khtlxDao.InsertLX(khtlx, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);


                report.BienKHNuoc("", _nvDao.Get(manvStr).MAKV, "", "", thang, nam, "INTAMTOKHMLX");

                msg = _khDao.InsertTamToKHM(1, 1, _nvDao.Get(manvStr).MAKV, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);

                var kynay = new DateTime(nam, thang, 1);
                //var kh = _khDao.GetKhachHangFromMadb(txtSODB.Text.Trim());
                var msgkt = _gcsDao.KhoiTaoGhiChiSoToKHMLX(kynay, _nvDao.Get(manvStr).MAKV);


                
                //var kynay = new DateTime(nam, thang, 1);
                //var msgktgcs = _gcsDao.KhoiTaoGhiChiSo(kynay, kh);

                //bool dacap = true;
                var hdkh = _khDao.Get(khtlx.IDKH);
                var msghopdong = _hdDao.UpdateDaSDTS(hdkh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);

                return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
            }


        }

        [AjaxPro.AjaxMethod]
        public string InKhachHangMoi(string idkhStr, string makvStr, string csdStr, string cscStr, string manvStr,
                                        string thangStr, string namStr)
        {    
            //if (tthaighi == "THAY")
            //    return DELIMITER.Passed;
            //report.KhachHangTT(idkh, tthaighi);           

            int csd, csc, kltieuthu, thang, nam;
            csd = Convert.ToInt32(csdStr);
            csc = Convert.ToInt32(cscStr);
            thang = Convert.ToInt32(thangStr);
            nam = Convert.ToInt32(namStr);
            kltieuthu = csc - csd;

            if (_khDao.Get(idkhStr) != null)// update chisocuoi khach hang moi
            {
                //var truythulichsu2 = new TRUYTHUHISTORY
                //{
                //    IDKH = "55555",
                //    //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                //    IPADDRESS = CommonFunc.GetMACAddress1(),

                //    MANV = manvStr,
                //    //USERAGENT = CommonFunc.GetComputerName(),
                //    USERAGENT = CommonFunc.GetComputerName(),
                //    NGAYTHUCHIEN = DateTime.Now,
                //    NAM = int.Parse(namStr),
                //    THANG = int.Parse(thangStr),
                //    CHISODAUMOI = Convert.ToInt32(csdStr),
                //    CHISOCUOIMOI = Convert.ToInt32(cscStr),
                //    KLTIEUTHUMOI = 12,

                //    MOTA = "Ghi sai chỉ số222"
                //};
                //_ttDao.Insert(truythulichsu2);

                string kykt = "01/" + thang + "/" + nam;
                var kh = new KHACHHANG
                {
                    IDKH = idkhStr,
                    CHISOCUOI = csc
                };

                var msg = _khDao.UpdateKHMCSC(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);

                var tieuthum = UpdateGCS(idkhStr, namStr, thangStr, csdStr, cscStr, "0", kltieuthu.ToString(), "", "GDH_BT", "admin");

                return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;                
            }
            else 
            {
                //var truythulichsu2 = new TRUYTHUHISTORY
                //{
                //    IDKH = "33333",
                //    //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                //    IPADDRESS = CommonFunc.GetMACAddress1(),

                //    MANV = manvStr,
                //    //USERAGENT = CommonFunc.GetComputerName(),
                //    USERAGENT = CommonFunc.GetComputerName(),
                //    NGAYTHUCHIEN = DateTime.Now,
                //    NAM = int.Parse(namStr),
                //    THANG = int.Parse(thangStr),
                //    CHISODAUMOI = Convert.ToInt32(csdStr),
                //    CHISOCUOIMOI = Convert.ToInt32(cscStr),
                //    KLTIEUTHUMOI = 12,

                //    MOTA = "Ghi sai chỉ số222"
                //};
                //_ttDao.Insert(truythulichsu2);

                var donkh = _khDao.GetMADDK(idkhStr);
                if (donkh != null)
                {
                    return DELIMITER.Passed;
                }

                var don = _ddkDao.Get(idkhStr);
                var hop = _hdDao.Get(idkhStr);
                var bb = _bbntDao.GetMADDK(idkhStr);
                var tc = _tcDao.Get(idkhStr);
                var dh = _dhDao.Get(tc != null ? tc.MADH : "");

                string kykt = "01/" + thang + "/" + nam;
                var kh = new KHACHHANG
                {
                    IDKH = _khDao.NewId(),

                    STTTS = _khDao.NewSTTTS(makvStr), // stt thoai son

                    MADB = hop.MADB != null ? hop.MADB : "",
                    MADP = hop.MADP != null ? hop.MADP : "",
                    DUONGPHU = "",
                    MALKHDB = "D",
                    MAMDSD = hop.MAMDSD != null ? hop.MAMDSD : "",
                    MADDK = idkhStr,
                    SOHD = hop.SOHD,
                    MABG = null,
                    MAHOTRO = "C",
                    MAPHUONG = hop.MADP != null ? hop.MADP.ToUpper().Trim().Substring(1, 1) : "",
                    TENKH = don.TENKH,
                    SONHA = don.SONHA != null ? don.SONHA : "",
                    MST = hop.MST != null ? hop.MST : "",
                    MAHTTT = "TM",
                    STK = "",
                    SDT = don.DIENTHOAI != null ? don.DIENTHOAI : "",
                    TTSD = "",
                    KOPHINT = false,
                    THUYLK = "15",
                    NGAYGNHAP = DateTime.Now,
                    VAT = true,
                    KHONGTINH117 = false,
                    MATT = "GDH_BT",
                    KYKHAITHAC = DateTimeUtil.GetVietNamDate(kykt),
                    GHI2THANG1LAN = "0",
                    THUHO = "0",
                    MAKV = makvStr,
                    MAKVDN = "O",
                    //MACQ = txtCQ.Text.Trim();
                    SOHO = 1,
                    SONK = 4,
                    SODINHMUC = 1,
                    ISDINHMUC = false,
                    ISVIPH = false,
                    NGAYHT = tc.NGAYHT,
                    //kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                    MALDH = dh.MALDH != null ? dh.MALDH : "",
                    MADH = dh.SONO != null ? dh.MADH : "",
                    KLKHOAN = 4,
                    STT = 1,
                    SDInfo_INHOADON = false,
                    TENKH_INHOADON = "",
                    DIACHI_INHOADON = "",
                    NAMBDKT = DateTime.Now.Year,
                    THANGBDKT = DateTime.Now.Month,
                    CHISODAU = 0,
                    ISHONGHEO = false,
                    CHISOCUOI = csc,
                    XOABOKH = false,
                    ISTACHDUONG = false,
                    ISDINHMUCTAM = false,
                    MTRUYTHU = 0
                };

                var msg = _khDao.InsertTS2(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);
                var kynay = new DateTime(nam, thang, 1);
                var msgktgcs = _gcsDao.KhoiTaoGhiChiSo(kynay, kh);

                //bool dacap = true;
                var msghopdong = _hdDao.UpdateDaSDTS(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);

                return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
            }


        }

        [AjaxPro.AjaxMethod]
        public string InKhachHangMoiPo(string idkhStr, string makvStr, string csdStr, string cscStr, string manvStr,
                                        string thangStr, string namStr)
        {
            //if (tthaighi == "THAY")
            //    return DELIMITER.Passed;
            //report.KhachHangTT(idkh, tthaighi);           
            manvStr = "admin";           

            int csd, csc, kltieuthu, thang, nam;
            csd = Convert.ToInt32(csdStr);
            csc = Convert.ToInt32(cscStr);
            thang = Convert.ToInt32(thangStr);
            nam = Convert.ToInt32(namStr);
            kltieuthu = csc-csd;
            
            //if (_khpoDao.Get(idkhStr) != null)// update chisocuoi khach hang moi
            if (_khpoDao.Get(idkhStr) != null)// update chisocuoi khach hang moi
            {
                var truythulichsu1 = new TRUYTHUHISTORY
                {
                    IDKH = "1",
                    //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                    IPADDRESS = CommonFunc.GetLanIPAddressM(),

                    MANV = manvStr,
                    //USERAGENT = CommonFunc.GetComputerName(),
                    USERAGENT = CommonFunc.GetComputerName(),
                    NGAYTHUCHIEN = DateTime.Now,
                    NAM = int.Parse(namStr),
                    THANG = int.Parse(thangStr),
                    CHISODAUMOI = Convert.ToInt32(csdStr),
                    CHISOCUOIMOI = Convert.ToInt32(cscStr),
                    KLTIEUTHUMOI = 12,

                    MOTA = "Ghi sai chỉ số1"
                };
                _ttDao.Insert(truythulichsu1);


                string kykt = "01/" + thang + "/" + nam;
                var kh = new KHACHHANGPO
                {
                    IDKHPO = idkhStr,
                    //IDKHPO = _khpoDao.GetMADDK(idkhStr).IDKHPO,
                    CHISOCUOI = csc
                };

                var msg = _khpoDao.UpdateKHMCSC(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);

                var tieuthupo = UpdateGCSPo(idkhStr, nam.ToString(), thang.ToString(), csdStr, cscStr, "0", Convert.ToString(kltieuthu), "", "GDH_BT", "admin");

                return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
            }
            else
            {
                var truythulichsu2 = new TRUYTHUHISTORY
                {
                    IDKH = "2",
                    //IPADDRESS = CommonFunc.GetIpAdddressComputerName(),
                    IPADDRESS = CommonFunc.GetLanIPAddressM(),

                    MANV = manvStr,
                    //USERAGENT = CommonFunc.GetComputerName(),
                    USERAGENT = CommonFunc.GetComputerName(),
                    NGAYTHUCHIEN = DateTime.Now,
                    NAM = int.Parse(namStr),
                    THANG = int.Parse(thangStr),
                    CHISODAUMOI = Convert.ToInt32(csdStr),
                    CHISOCUOIMOI = Convert.ToInt32(cscStr),
                    KLTIEUTHUMOI = 12,

                    MOTA = "Ghi sai chỉ số2"
                };
                _ttDao.Insert(truythulichsu2);


                var donkh = _khpoDao.GetMADDK(idkhStr);
                if (donkh != null)
                {
                    return DELIMITER.Passed;
                }

                var don = _ddkpoDao.Get(idkhStr);
                var hop = _hdpoDao.Get(idkhStr);
                var bb = _bbntpoDao.GetMADDK(idkhStr);
                var tc = _tcDao.Get(idkhStr);
                var dh = _dhpoDao.Get(tc != null ? tc.MADH : "");               

                string kykt = "01/" + thang + "/" + nam;
                var kh = new KHACHHANGPO
                {
                    //IDKHPO = idkhStr, //_khDao.NewId(),
                    IDKHPO = _khpoDao.NewId(),

                    STTTS = _khpoDao.NewSTTTS(makvStr),

                    MADBPO = hop.MADB != null ? hop.MADB : "",
                    MADPPO = hop.MADPPO != null ? hop.MADPPO : "",
                    DUONGPHUPO = "",
                    MALKHDB = "D",
                    MAMDSDPO = hop.MAMDSDPO != null ? hop.MAMDSDPO : "",
                    MADDKPO = idkhStr,
                    SOHD = hop.SOHD,
                    MABG = null,
                    MAHOTRO = "C",
                    MAPHUONGPO = hop.MADPPO != null ? hop.MADPPO.ToUpper().Trim().Substring(1, 1) : "",
                    TENKH = don.TENKH,
                    SONHA = don.SONHA != null ? don.SONHA : "",
                    MST = hop.MST != null ? hop.MST : "",
                    MAHTTT = "TM",
                    STK = "",
                    SDT = don.DIENTHOAI != null ? don.DIENTHOAI : "",
                    TTSD = "",
                    KOPHINT = false,
                    THUYLK = "",
                    NGAYGNHAP = DateTime.Now,
                    VAT = true,
                    KHONGTINH117 = false,
                    MATT = "GDH_BT",
                    KYKHAITHAC = DateTimeUtil.GetVietNamDate(kykt),
                    //GHI2THANG1LAN = "0",
                    THUHO = "0",
                    MAKVPO = makvStr,
                    MAKVDN = "O",
                    //MACQ = txtCQ.Text.Trim();
                    SOHO = 1,
                    SONK = 4,

                    SODINHMUC = 1,

                    ISDINHMUC = false,
                    //ISVIPH = false,
                    NGAYHT = tc.NGAYHT,
                    //kh.KYHOTRO = DateTimeUtil.GetVietNamDate("01/" + txtKYHOTRO.Text.Trim());
                    MALDHPO = dh.MALDHPO != null ? dh.MALDHPO : "",
                    MADHPO = dh.SONO != null ? dh.MADHPO : "",
                    KLKHOAN = 4,
                    STT = 1,
                    SDInfo_INHOADON = false,
                    TENKH_INHOADON = "",
                    DIACHI_INHOADON = "",
                    NAMBDKT = DateTime.Now.Year,
                    THANGBDKT = DateTime.Now.Month,
                    CHISODAU = 0,
                    //ISHONGHEO = false,
                    CHISOCUOI = csc,                   

                    XOABOKHPO = false,
                    MUCDICHKHAC=false
                    //ISTACHDUONG = false,
                    //ISDINHMUCTAM = false
                };

                var msg = _khpoDao.InsertTS2(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);
                var kynay = new DateTime(nam, thang, 1);
                var msgktgcs = _gcspoDao.KhoiTaoGhiChiSo(kynay, kh);

                //bool dacap = true;
                var msghopdong = _hdpoDao.UpdateDaSDTS(kh, CommonFunc.GetComputerName(), CommonFunc.GetLanIPAddressM(), manvStr);
                
                return msg.MsgType.Equals(MessageType.Error) ? DELIMITER.Failed : DELIMITER.Passed;
            }


        }


    }
}
       