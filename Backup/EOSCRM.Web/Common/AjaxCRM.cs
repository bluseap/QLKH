using System;
using System.Collections.Generic;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Web.Common
{
    public class AjaxCRM
    {
        #region Imports DAO
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

        private readonly QuyetToanSuaChuaDao qtscDao = new QuyetToanSuaChuaDao();
        private readonly ChiTietQuyetToanSuaChuaDao ctqtscDao = new ChiTietQuyetToanSuaChuaDao();
        private readonly ChiTietQuyetToanSuaChuaND117Dao ctqtsc117Dao = new ChiTietQuyetToanSuaChuaND117Dao();

        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        private readonly TruyThuDao _ttDao = new TruyThuDao();
        private readonly HistoriDao _historiDao = new HistoriDao();
        private readonly ReportClass report = new ReportClass();
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
            string dvt, string sl, string hs, string loai)
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
                                dlmbvt.HESOCP + DELIMITER.Delimiter +
                                dlmbvt.LOAICP;

            try
            {
                dlmbvt.NOIDUNG = nd;
                dlmbvt.DONGIACP = decimal.Parse(dg);
                dlmbvt.DVT = dvt;
                dlmbvt.SOLUONG = decimal.Parse(sl);
                dlmbvt.HESOCP = decimal.Parse(hs);
                dlmbvt.THANHTIENCP = decimal.Parse(sl) * decimal.Parse(hs) * decimal.Parse(dg);
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

            var msg = _khDao.UpdateSoBo(kh, CommonFunc.GetComputerName(),
                CommonFunc.GetIpAdddressComputerName(), manv);

            report.UpdateTieuThu(idkh, madp.Trim(), madb.Trim());

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
        public string UpdateGCS(string idkh, string namStr, string thangStr, string csdStr, string cscStr,
            string mttStr ,string klttStr,
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

    }
}
