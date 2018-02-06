using System;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Collections.Generic;

namespace EOSCRM.Dao
{
    public class GhiChiSoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        private readonly ReportClass rp = new ReportClass();

        public GhiChiSoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public Message TinhTien(int thang, int nam, string khuvuc)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                rp.DONGCUA_TT(thang, nam, khuvuc);
                var ttList = _db.TIEUTHUs.Where(tt => tt.THANG == thang && tt.NAM == nam && tt.MAKV == khuvuc).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;
                    //ChangeTieuThu(tt);
                    ChangeTieuThuLX(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienLX(int thang, int nam, string khuvuc)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                rp.DONGCUA_TT(thang, nam, khuvuc);
                var ttList = _db.TIEUTHUs.Where(tt => tt.THANG == thang && tt.NAM == nam && tt.MAKV == khuvuc).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = tt.TTHAIGHI.Equals("Q") ?  (Convert.ToInt64(tt.CHISODAU).ToString().Length == 4 ?
                                (10000 - tt.CHISODAU) + tt.CHISOCUOI + tt.MTRUYTHU : (Convert.ToInt64(tt.CHISODAU).ToString().Length == 5 ?
                                (100000 - tt.CHISODAU) + tt.CHISOCUOI + tt.MTRUYTHU : 0))
                            : (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;
                    ChangeTieuThuLX(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienDCLX(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUDCs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUDC = (tt.CHISOCUOIDC + tt.MTRUYTHUDC) - tt.CHISODAUDC;                    
                    ChangeTieuThuDCDMLX(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienDC(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUDCs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUDC = (tt.CHISOCUOIDC + tt.MTRUYTHUDC) - tt.CHISODAUDC;
                    //ChangeTieuThuDC(tt);
                    ChangeTieuThuDCDM(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTongTienNuoc(string idkh, int thang, int nam, string mamdsd, decimal tienuoc, decimal tienthue, decimal tienthuemt, decimal tongtien,
                int chisodau, int chisocuoi, int mtruythu, int kltt)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;

                    ChangeTongTienNuoc(tt, tienuoc, tienthue, tienthuemt, tongtien, chisodau, chisocuoi, mtruythu, kltt);

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTongTienNuocDC(string idkh, int thang, int nam, string mamdsd, decimal tienuoc, decimal tienthue, decimal tienthuemt, decimal tongtien, 
                int chisodau, int chisocuoi, int mtruythu, int kltt )
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUDCs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUDC = (tt.CHISOCUOIDC + tt.MTRUYTHUDC) - tt.CHISODAUDC;
                    
                    ChangeTongTienNuocDC(tt, tienuoc, tienthue, tienthuemt, tongtien, chisodau, chisocuoi, mtruythu, kltt);

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTVP1GIA(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUTTVPs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUVP = (tt.CHISOCUOIVP + tt.MTRUYTHUVP) - tt.CHISODAUVP;
                    ChangeTieuThuTTVP1GIA(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTVP(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUTTVPs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUVP = (tt.CHISOCUOIVP + tt.MTRUYTHUVP) - tt.CHISODAUVP;
                    ChangeTieuThuTTVP(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTVPSD(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUTTVPs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUVP = (tt.CHISOCUOIVP + tt.MTRUYTHUVP) - tt.CHISODAUVP;
                    ChangeTieuThuTTVPSD(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienDC1GIALX(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUDCs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUDC = (tt.CHISOCUOIDC + tt.MTRUYTHUDC) - tt.CHISODAUDC;
                    ChangeTieuThuDC1GIALX(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienDC1GIA(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUDCs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUDC = (tt.CHISOCUOIDC + tt.MTRUYTHUDC) - tt.CHISODAUDC;
                    ChangeTieuThuDC1GIA(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTDCLX(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;
                    ChangeTieuThuLX(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTDC(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;
                    ChangeTieuThu(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTDC1GIALX(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;
                    ChangeTieuThu1GIALX(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTDC1GIA(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHU = (tt.CHISOCUOI + tt.MTRUYTHU) - tt.CHISODAU;
                    ChangeTieuThu1GIA(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message TinhTienTTDCTG(string idkh, int thang, int nam)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var ttList = _db.TIEUTHUDCTGs.Where(tt => tt.IDKH == idkh && tt.THANG == thang && tt.NAM == nam).OrderBy(tt => tt.SODB).ToList();

                foreach (var tt in ttList)
                {
                    tt.KLTIEUTHUDC = (tt.CHISOCUOIDC + tt.MTRUYTHUDC) - tt.CHISODAUDC;
                    ChangeTieuThuDCTG(tt);
                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        private decimal? GetDMNK()
        {
            // get dmnk
            var hskh = _db.HESOKHs.SingleOrDefault(hs => hs.MAHS.Equals(MAHSKH.DMNK));
            var dmnk = (hskh != null && hskh.GIATRI.HasValue) ? hskh.GIATRI.Value : (decimal?)null;

            return dmnk;
        }

        private decimal? GiaNuocB1(MAMDSD mamdsd, DUONGPHO dp)
        {
            if (dp.GIAKHAC.HasValue && dp.GIAKHAC.Value)
            {
                if (mamdsd == MAMDSD.KD)
                    return dp.GIAKDK;
                if (mamdsd == MAMDSD.CQ)
                    return dp.GIACQK;
                if (mamdsd == MAMDSD.HN)
                    return dp.GIAHNK;

                 if (mamdsd == MAMDSD.SH)
                     return dp.GIASH;
                 if (mamdsd == MAMDSD.HC)
                     return dp.GIAHC;
                 if (mamdsd == MAMDSD.XS)
                     return dp.GIAXS;
                 if (mamdsd == MAMDSD.CS)
                     return dp.GIACS;
                 if (mamdsd == MAMDSD.VD)
                     return dp.GIAVD;
                 if (mamdsd == MAMDSD.DS)
                     return dp.GIADS;


                return dp.GIASHK;
            }

            var dmsd = _db.DMSDs.SingleOrDefault(dm => dm.MADMSD.ToLower().Equals(mamdsd.ToString().ToLower()));
            var gia = (dmsd != null && dmsd.BAC1.HasValue) ? dmsd.BAC1.Value : (decimal?)null;

            return gia;
        }

        private decimal? GiaNuocB2(MAMDSD mamdsd, DUONGPHO dp)
        {
            if (dp.GIAKHAC.HasValue && dp.GIAKHAC.Value)
            {
                if (mamdsd == MAMDSD.KD)
                    return dp.GIAKDK;
                if (mamdsd == MAMDSD.CQ)
                    return dp.GIACQK;
                if (mamdsd == MAMDSD.HN)
                    return dp.GIAHNK;

                if (mamdsd == MAMDSD.SH)
                    return dp.GIASH;
                if (mamdsd == MAMDSD.HC)
                    return dp.GIAHC;
                if (mamdsd == MAMDSD.XS)
                    return dp.GIAXS;
                if (mamdsd == MAMDSD.CS)
                    return dp.GIACS;
                if (mamdsd == MAMDSD.VD)
                    return dp.GIAVD;
                if (mamdsd == MAMDSD.DS)
                    return dp.GIADS;


                return dp.GIASHK;
            }

            var dmsd = _db.DMSDs.SingleOrDefault(dm => dm.MADMSD.ToLower().Equals(mamdsd.ToString().ToLower()));
            var gia = (dmsd != null && dmsd.BAC2.HasValue) ? dmsd.BAC2.Value : (decimal?)null;

            return gia;
        }

        private decimal? GiaNuocB3(MAMDSD mamdsd, DUONGPHO dp)
        {
            if (dp.GIAKHAC.HasValue && dp.GIAKHAC.Value)
            {
                if (mamdsd == MAMDSD.KD)
                    return dp.GIAKDK;
                if (mamdsd == MAMDSD.CQ)
                    return dp.GIACQK;
                if (mamdsd == MAMDSD.HN)
                    return dp.GIAHNK;

                if (mamdsd == MAMDSD.SH)
                    return dp.GIASH;
                if (mamdsd == MAMDSD.HC)
                    return dp.GIAHC;
                if (mamdsd == MAMDSD.XS)
                    return dp.GIAXS;
                if (mamdsd == MAMDSD.CS)
                    return dp.GIACS;
                if (mamdsd == MAMDSD.VD)
                    return dp.GIAVD;
                if (mamdsd == MAMDSD.DS)
                    return dp.GIADS;


                return dp.GIASHK;
            }

            var dmsd = _db.DMSDs.SingleOrDefault(dm => dm.MADMSD.ToLower().Equals(mamdsd.ToString().ToLower()));
            var gia = (dmsd != null && dmsd.BAC3.HasValue) ? dmsd.BAC3.Value : (decimal?)null;

            return gia;
        }

        private decimal? GiaNuoc(MAMDSD mamdsd, DUONGPHO dp)
        {
            if(dp.GIAKHAC.HasValue && dp.GIAKHAC.Value)
            {
                if (mamdsd == MAMDSD.KD)
                    return dp.GIAKDK;
                if (mamdsd == MAMDSD.CQ)
                    return dp.GIACQK;
                if (mamdsd == MAMDSD.HN)
                    return dp.GIAHNK;    
                return dp.GIASHK;
            }
            
            var dmsd = _db.DMSDs.SingleOrDefault(dm => dm.MADMSD.ToLower().Equals(mamdsd.ToString().ToLower()));
            var gia = (dmsd != null && dmsd.GIA.HasValue) ? dmsd.GIA.Value : (decimal?)null;

            return gia;
        }

        private decimal? GiaVAT(MAMDSD mamdsd, DUONGPHO dp)
        {
            if (dp.GIAKHAC.HasValue && dp.GIAKHAC.Value)
            {
                if (mamdsd == MAMDSD.KD)
                    return dp.THUEKDK;
                if (mamdsd == MAMDSD.CQ)
                    return dp.THUECQK;
                if (mamdsd == MAMDSD.HN)
                    return dp.THUEHNK;

                if (mamdsd == MAMDSD.SH)
                    return dp.GIASH;
                if (mamdsd == MAMDSD.HC)
                    return dp.GIAHC;
                if (mamdsd == MAMDSD.XS)
                    return dp.GIAXS;
                if (mamdsd == MAMDSD.CS)
                    return dp.GIACS;
                if (mamdsd == MAMDSD.VD)
                    return dp.GIAVD;
                if (mamdsd == MAMDSD.DS)
                    return dp.GIADS;

                return dp.THUESHK;
            }

            var dmsd = _db.DMSDs.SingleOrDefault(dm => dm.MADMSD.ToLower().Equals(mamdsd.ToString().ToLower()));
            var gia = (dmsd != null && dmsd.VAT.HasValue) ? dmsd.VAT.Value : (decimal?)null;

            return gia;
        }

        private decimal? GiaPhi(MAMDSD mamdsd, DUONGPHO dp)
        {
            if (dp.GIAKHAC.HasValue && dp.GIAKHAC.Value)
            {
                if (mamdsd == MAMDSD.KD)
                    return dp.PHIKDK;
                if (mamdsd == MAMDSD.CQ)
                    return dp.PHICQK;
                if (mamdsd == MAMDSD.HN)
                    return dp.PHIHNK;
                return dp.PHISHK;
            }

            var dmsd = _db.DMSDs.SingleOrDefault(dm => dm.MADMSD.ToLower().Equals(mamdsd.ToString().ToLower()));
            var gia = (dmsd != null && dmsd.PBVMT.HasValue) ? dmsd.PBVMT.Value : (decimal?)null;

            return gia;
        }

        


        public Message KhoiTaoGhiChiSo(DateTime date, DUONGPHO dp)
        {
            //TODO: Thêm lockstatus bị thiếu
            try
            {
                var prevDate = date.AddMonths(-1);

                var existedLockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(dp.MADP) &&
                                                                               l.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                                               l.KYGT.Month == date.Month &&
                                                                               l.KYGT.Year == date.Year);
                if (existedLockstatus == null)
                {
                    var lockstatus = new LOCKSTATUS
                    {
                        MADP = dp.MADP,
                        DUONGPHU = dp.DUONGPHU,
                        KYGT = date,
                        LOCKGCS = false,
                        LOCKTINHCUOC = false,
                        LOCKCN = false,
                        MAKV = dp.MADP.Substring(0,1)
                    };
                    _db.LOCKSTATUS.InsertOnSubmit(lockstatus);

                    var prevLockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(dp.MADP) &&
                                                                            l.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                                            l.KYGT.Month == prevDate.Month &&
                                                                            l.KYGT.Year == prevDate.Year);
                    if (prevLockstatus != null)
                    {
                        prevLockstatus.LOCKGCS = true;
                        prevLockstatus.LOCKTINHCUOC = true;
                    }
                }

                var existedGT = _db.DUONGPHOGTs.FirstOrDefault(gt => gt.MADP.Equals(dp.MADP) &&
                                                                     gt.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                                     gt.NAM.Equals(date.Year) &&
                                                                     gt.THANG.Equals(date.Month));
                if (existedGT == null)
                {
                    var prevGT = _db.DUONGPHOGTs.FirstOrDefault(gt => gt.MADP.Equals(dp.MADP) &&
                                                                      gt.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                                      gt.NAM.Equals(prevDate.Year) &&
                                                                      gt.THANG.Equals(prevDate.Month));
                    if (prevGT != null)
                    {

                        var submitGT = new DUONGPHOGT
                                           {
                                               MADP = prevGT.MADP,
                                               DUONGPHU = prevGT.DUONGPHU,
                                               NAM = date.Year,
                                               THANG = date.Month,
                                               MAKV = prevGT.MAKV,
                                               MANVG = prevGT.MANVG,
                                               MANVT = prevGT.MANVT
                                           };
                        _db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                    }
                    else
                    {
                        var submitGT = new DUONGPHOGT
                        {
                            MADP = dp.MADP,
                            DUONGPHU = dp.DUONGPHU,
                            NAM = date.Year,
                            THANG = date.Month,
                            MAKV = dp.MAKV,
                            MANVG = dp.MANVG ?? "admin",
                            MANVT = dp.MANVT ?? "admin"
                        };
                        _db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                    }
                }
                _db.SubmitChanges();
            }
            catch
            {
                return new Message(MessageConstants.E_GCS_KHOITAO_KYDAKHOITAO, MessageType.Error);
            }

            //TODO: cập nhật TTSD trong tiêu thụ (chuyển trạng thái cúp mở nước)
            var joinQuery = from kh in _db.KHACHHANGs
                            join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                            where kh.MADP == dp.MADP && tt.NAM == date.Year && tt.THANG == date.Month
                            select tt;

            foreach (var tieuthu in joinQuery)
            {
                tieuthu.TTSD = tieuthu.KHACHHANG.TTSD;

                if(tieuthu.TTSD == "CUP")
                    tieuthu.INHD = false;
            }

            //TODO: Thêm mới các khách hàng tiêu thụ tháng này
            try
            {
                var insertNewQuery = from kh in _db.KHACHHANGs
                                     join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                                     into khTt
                                     // left outer join
                                     from tt in khTt.Where(t => t.NAM == date.Year && t.THANG == date.Month).DefaultIfEmpty()
                                     where (tt.IDKH == null)
                                         && (kh.MADP == dp.MADP)
                                         && (kh.DUONGPHU == dp.DUONGPHU)
                                     select new
                                     {
                                         kh.IDKH, NAM = date.Year, THANG = date.Month, dp.MAKV, kh.MAKVDN,
                                         kh.MADP, kh.DUONGPHU, kh.MADB, SODB = (kh.MADP + kh.DUONGPHU + kh.MADB),

                                         //CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                                         //CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                                         CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                                         CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                                         KLTIEUTHU = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                         kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                                         KLDHTONG = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                         kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                                         kh.MAMDSD, kh.MABG, dp.NONGTHON, kh.MACQ,
                                         kh.SOHO, kh.SONK, kh.ISDINHMUC, kh.SODINHMUC,
                                         kh.TTSD, kh.KHONGTINH117, kh.KYHOTRO,
                                         COPHIMT = kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1, 
                                         TTHAIGHI = kh.MATT, PHIBVMT = 100, PHIBVMT2 = 0,
                                         PTVAT = (kh.VAT.HasValue && !kh.VAT.Value) ? 0 : (decimal)0.05,
                                         kh.MAHTTT, INHD_TT = tt.INHD,

                                         TINHTIEN2THANG = (kh.GHI2THANG1LAN == "1") ? "A" : null,

                                         kh.GHI2THANG1LAN,
                                         ING2T1L = (kh.GHI2THANG1LAN == "1") ? 1 : 0
                                     };

                var newCS = insertNewQuery.AsEnumerable() 
                         .Select(x => new TIEUTHU
                         {
                             IDKH = x.IDKH, NAM = x.NAM, THANG = x.THANG, MAKV = x.MAKV, MAKVDN = x.MAKVDN,
                             MADP = x.MADP, DUONGPHU = x.DUONGPHU, MADB = x.MADB, SODB = x.SODB,
                             CHISODAU = x.CHISODAU, CHISOCUOI = x.CHISOCUOI, KLDHTONG = x.KLDHTONG, KLTIEUTHU = x.KLTIEUTHU,
                             MAMDSD = x.MAMDSD, MABG = x.MABG, MACQ = x.MACQ,
                             SOHO = x.SOHO,
                             SONK = x.SONK,
                             ISDINHMUC = x.ISDINHMUC,
                             DMNK = x.SODINHMUC,//DMNK = GetDMNK(),
                             TTSD = x.TTSD, KHONGTINH117 = x.KHONGTINH117, KYHOTRO = x.KYHOTRO,
                             COPHIMT = x.COPHIMT, TTHAIGHI = "GDH_BT", TIENNUOC = 0, THUEBAO = 0, TONGTIEN_PS = 0,
                             PHIBVMT = x.PHIBVMT, PTVAT = x.PTVAT,
                             CHISODAU_1 = 0, CHISOCUOI_1 = 0, KLTIEUTHU_1 = 0, TONGTIENPS_1 = 0,
                             INHD_TT = x.INHD_TT.HasValue && x.INHD_TT.Value,
                             MAHTTT = x.MAHTTT, INHD = false, GHICHU_CS = "",
                             HETNO = false, THUTQ = false,
                             TIENPHI = 0,TIENTHUE = 0,KOPHINT = true,KOVAT = false, 
                             M3MUC1 = 0,GIAMUC1 = 0,
                             M3MUC2 = 0,GIAMUC2 = 0, 
                             M3MUC3 = 0,GIAMUC3 = 0,
                             M3MUC4 = 0,GIAMUC4 = 0,
                             TNUOCMUC1 = 0,THUEMUC1 = 0,PHIMUC1 = 0,
                             TNUOCMUC2 = 0,THUEMUC2 = 0,PHIMUC2 = 0,
                             TNUOCMUC3 = 0,THUEMUC3 = 0,PHIMUC3 = 0,
                             TNUOCMUC4 = 0,THUEMUC4 = 0,PHIMUC4 = 0, 
                             
                             THBT = "BT", MTRUYTHU=0,
                             TINHTIEN2THANG = x.TINHTIEN2THANG,
                             GHI2THANG1LAN = x.GHI2THANG1LAN,
                             ING2T1L = x.ING2T1L                            
                                 
                         });

                //TODO: tinh tien cho cac khach hang moi tai thoi diem nay
                foreach (var cs in newCS)
                {
                    cs.TTHAIGHI = !string.IsNullOrEmpty(cs.TTHAIGHI) ? cs.TTHAIGHI.Trim() : null;
                    cs.MANVN_CS = "admin";
                    cs.THBT = "BT";

                    if (cs.TTSD != "CUP")
                    {
                        ChangeTieuThu(cs);
                        cs.INHD = true;
                    }

                    cs.HETNO = false; cs.THUTQ = false; cs.MANVCN = null; cs.MANVNHAPCN = null;
                    cs.NGAYCN = null; cs.SOPHIEUCN = null; cs.NGAYNHAPCN = null;

                    _db.TIEUTHUs.InsertOnSubmit(cs);
                }
                _db.SubmitChanges();

            }
            catch(Exception ex)
            {
                return ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
            }

            return new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo kỳ ghi chỉ số");
        }

        public Message KhoiTaoGhiChiSo(DateTime date, string makv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var prevDate = date.AddMonths(-1);

                //TODO: Kiểm tra khu vực khởi tạo hay chưa
                var joinQuery = from tt in _db.TIEUTHUs
                                join lck in _db.LOCKSTATUS
                                    on new { tt.THANG, tt.NAM, tt.MADP }
                                    equals new { THANG = lck.KYGT.Month, NAM = lck.KYGT.Year, lck.MADP }
                                where tt.THANG.Equals(date.Month) && tt.NAM.Equals(date.Year) && tt.MAKV.Equals(makv)
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

                //TODO: Thêm lockstatus
                var dpList = new DuongPhoDao().GetList(makv);
                foreach (var dp in dpList)
                {
                    var lockstatus = new LOCKSTATUS
                    {
                        MADP = dp.MADP,
                        DUONGPHU = dp.DUONGPHU,
                        KYGT = date,
                        LOCKGCS = false,
                        LOCKTINHCUOC = false,
                        LOCKCN = false,
                        MAKV = makv
                        
                    };
                    _db.LOCKSTATUS.InsertOnSubmit(lockstatus);

                    //TODO: khoa ghi chi so thang truoc
                    // ReSharper disable AccessToModifiedClosure
                    var prevLockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(dp.MADP) &&
                                                                            l.DUONGPHU.Equals(dp.DUONGPHU) && l.KYGT.Month == prevDate.Month &&
                                                                            l.KYGT.Year == prevDate.Year);
                    // ReSharper restore AccessToModifiedClosure
                    if (prevLockstatus != null)
                    {
                        prevLockstatus.LOCKGCS = true;
                        prevLockstatus.LOCKTINHCUOC = true;
                    }

                    //TODO: Thêm mới duongphogt
                    // ReSharper disable AccessToModifiedClosure
                    var cnt = _db.DUONGPHOGTs.Count(gt => gt.MADP.Equals(dp.MADP) &&
                                                          gt.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                          gt.NAM.Equals(date.Year) &&
                                                          gt.THANG.Equals(date.Month));
                    if (cnt != 0) continue;

                    var prevGT = _db.DUONGPHOGTs.FirstOrDefault(gt => gt.MADP.Equals(dp.MADP) &&
                                                                      gt.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                                      gt.NAM.Equals(prevDate.Year) &&
                                                                      gt.THANG.Equals(prevDate.Month));
                    // ReSharper restore AccessToModifiedClosure
                    if (prevGT == null) continue;

                    var submitGT = new DUONGPHOGT
                    {
                        MADP = prevGT.MADP,
                        DUONGPHU = prevGT.DUONGPHU,
                        NAM = date.Year,
                        THANG = date.Month,
                        MAKV = prevGT.MAKV,
                        MANVG = prevGT.MANVG,
                        MANVT = prevGT.MANVT
                    };

                    _db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                }

                try
                {
                    _db.SubmitChanges();
                }
                catch
                {
                    msg = new Message(MessageConstants.E_GCS_KHOITAO_KYDAKHOITAO, MessageType.Error);
                    // commit
                    trans.Rollback();
                    _db.Connection.Close();
                    return msg;
                }
                
                //TODO: Đổ dữ liệu từ kỳ ghi thu trước sang bảng TIEUTHU
                foreach (var dp in dpList)
                {
                    var temp = dp;
                    var insertOldQuery = from kh in _db.KHACHHANGs
                                         join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                                         where (kh.MADP == temp.MADP)
                                               && (kh.DUONGPHU == temp.DUONGPHU)
                                               && (tt.NAM == prevDate.Year)
                                               && (tt.THANG == prevDate.Month)
                                               && (kh.MAKV == makv)
                                               && (kh.XOABOKH == false)
                                         select new
                                         {
                                             kh.IDKH, NAM = date.Year, THANG = date.Month, MAKV = makv, kh.MAKVDN, 
                                             kh.MADP, kh.DUONGPHU, kh.MADB, SODB = kh.MADP + kh.DUONGPHU + kh.MADB,

                                             //CHISODAU = tt.TINHTIEN2THANG.Equals("A") ? tt.CHISODAU : tt.CHISOCUOI,
                                             CHISODAU = kh.TTSD.Equals("THAY") ? 0 : tt.CHISOCUOI,
                                             CHISOCUOI = 0,//kh.TTSD.Equals("CUP") ? tt.CHISOCUOI : 0,
                                             //TTHAIGHI = (tt.TTHAIGHI.Equals("D") || tt.TTHAIGHI.Equals("THAY")) ? "GDH_BT" : tt.TTHAIGHI,
                                             TTHAIGHI = "GDH_BT",
                                             kh.MAMDSD, kh.MABG, temp.NONGTHON, kh.MACQ,

                                             SOHO = kh.ISDINHMUCTAM == true ? 1 : kh.SOHO,
                                             SONK = kh.ISDINHMUCTAM == true ? 1 : kh.SONK,
                                             ISDINHMUC = kh.ISDINHMUCTAM == true ? false : kh.ISDINHMUC,
                                             SODINHMUC = kh.ISDINHMUCTAM == true ? 1 : kh.SODINHMUC,

                                             kh.TTSD, kh.KHONGTINH117, kh.KYHOTRO,
                                             COPHIMT = kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1,
                                             PHIBVMT = (decimal)0.05, PHIBVMT2 = 0, 
                                             PTVAT = (kh.VAT.HasValue && !kh.VAT.Value) ? 0 : (decimal)0.05,
                                             CHISODAU_1 = tt.CHISODAU, CHISOCUOI_1 = tt.CHISOCUOI, 
                                             KLTIEUTHU_1 = tt.KLTIEUTHU, TONGTIENPS_1 = tt.TONGTIEN_PS,
                                             kh.MAHTTT, INHD_TT = tt.INHD,

                                             //TINHTIEN2THANG= (tt.TINHTIEN2THANG != null && kh.GHI2THANG1LAN.Equals("1")) ? 
                                             //   (tt.TINHTIEN2THANG.Equals("A") ? "B" : "A") : null,                                                
                                             TINHTIEN2THANG = tt.TINHTIEN2THANG ,                                                
                                             GHI2THANG1LAN= tt.KLTIEUTHU.Equals(0) ? "1" : "0" ,
                                             ING2T1L=tt.GHI2THANG1LAN.Equals("1") ? 1 : 0

                                         };

                    var oldCS = insertOldQuery.AsEnumerable() // From here on it's in-process
                        .Select(x => new TIEUTHU
                                         {
                                             IDKH = x.IDKH, NAM = x.NAM, THANG = x.THANG, MAKV = x.MAKV, MAKVDN = x.MAKVDN, 
                                             MADP = x.MADP, DUONGPHU = x.DUONGPHU, MADB = x.MADB, SODB = x.SODB,
                                             CHISODAU = x.CHISODAU, CHISOCUOI = x.CHISOCUOI, KLTIEUTHU = 0, KLDHTONG = 0,
                                             MAMDSD = x.MAMDSD, MABG = x.MABG, MACQ = x.MACQ,
                                             SOHO = x.SOHO, SONK = x.SONK, ISDINHMUC = x.ISDINHMUC, DMNK = x.SODINHMUC,//DMNK = GetDMNK(),
                                             TTSD = x.TTSD, KHONGTINH117 = x.KHONGTINH117, KYHOTRO = x.KYHOTRO,
                                             COPHIMT = x.COPHIMT, PHIBVMT = x.PHIBVMT, PTVAT = x.PTVAT,
                                             CHISODAU_1 = x.CHISODAU_1, CHISOCUOI_1 = x.CHISOCUOI_1, KLTIEUTHU_1 = x.KLTIEUTHU_1, TONGTIENPS_1 = x.TONGTIENPS_1,
                                             MAHTTT = x.MAHTTT, INHD_TT = x.INHD_TT,
                                             INHD = false, TIENNUOC = 0, THUEBAO = 0, TONGTIEN_PS = 0,TONGTIEN = 0,
                                             GHICHU_CS = "", TTHAIGHI = x.TTHAIGHI, HETNO = false, THUTQ = false,

                                             TIENPHI = 0, TIENTHUE = 0, KOPHINT = true, KOVAT = false,
                                             M3MUC1 = 0,  GIAMUC1 = 0,  
                                             M3MUC2 = 0,  GIAMUC2 = 0, 
                                             M3MUC3 = 0,  GIAMUC3 = 0,
                                             M3MUC4 = 0,  GIAMUC4 = 0,
                                             TNUOCMUC1 = 0, THUEMUC1 = 0, PHIMUC1 = 0, 
                                             TNUOCMUC2 = 0, THUEMUC2 = 0, PHIMUC2 = 0,
                                             TNUOCMUC3 = 0, THUEMUC3 = 0, PHIMUC3 = 0,
                                             TNUOCMUC4 = 0, THUEMUC4 = 0, PHIMUC4 = 0, 
                                             THBT="BT",MTRUYTHU=0,
                                             TINHTIEN2THANG = x.TINHTIEN2THANG,
                                             GHI2THANG1LAN = x.GHI2THANG1LAN,
                                             ING2T1L = x.ING2T1L                                                
                                         });

                    _db.TIEUTHUs.InsertAllOnSubmit(oldCS);
                }

                _db.SubmitChanges();

                
                //TODO:Đổ dữ liệu khách hàng mới trong kỳ ghi thu hiện tại vô TIEUTHU
                foreach (var dp in dpList)
                {
                    var temp = dp;

                    //TODO: Thêm mới các khách hàng tiêu thụ tháng này
                    var insertNewQuery = from kh in _db.KHACHHANGs
                                         join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                                         into khTt
                                         // left outer join
                                         from tt in khTt.Where(t => t.NAM == date.Year && t.THANG == date.Month).DefaultIfEmpty()
                                         where (tt.IDKH == null)
                                             && (kh.MADP == temp.MADP)
                                             && (kh.DUONGPHU == temp.DUONGPHU)
                                             && (kh.MAKV == makv)
                                             && (kh.XOABOKH == false)
                                         select new
                                         {
                                             kh.IDKH, NAM = date.Year, THANG = date.Month, MAKV = makv, kh.MAKVDN,
                                             kh.MADP, kh.DUONGPHU, kh.MADB, SODB = (kh.MADP + kh.DUONGPHU + kh.MADB),

                                             //CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                                             //CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                                             CHISODAU = kh.CHISODAU,
                                             CHISOCUOI = kh.CHISOCUOI,
                                             KLTIEUTHU = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                             kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                                             KLDHTONG = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                             kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,

                                             kh.MAMDSD, kh.MABG, temp.NONGTHON, kh.MACQ,
                                             kh.SOHO, kh.SONK, kh.ISDINHMUC, kh.SODINHMUC,
                                             kh.TTSD, kh.KHONGTINH117, kh.KYHOTRO,
                                             COPHIMT = kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1,
                                             TTHAIGHI = kh.MATT.Equals("CUP") ? "CUP" : "GDH_BT",
                                             PHIBVMT = 100,
                                             PHIBVMT2 = 0,
                                             PTVAT = (kh.VAT.HasValue && !kh.VAT.Value) ? 0 : (decimal)0.05,
                                             kh.MAHTTT, INHD_TT = tt.INHD,

                                             TINHTIEN2THANG= (kh.GHI2THANG1LAN=="1") ? "A" : null,

                                             GHI2THANG1LAN="0",
                                             ING2T1L = (kh.GHI2THANG1LAN == "1") ? 1 : 0

                                         };

                    var newCS = insertNewQuery.AsEnumerable() 
                             .Select(x => new TIEUTHU
                             {
                                 IDKH = x.IDKH, NAM = x.NAM, THANG = x.THANG, MAKV = x.MAKV, MAKVDN = x.MAKVDN,
                                 MADP = x.MADP, DUONGPHU = x.DUONGPHU, MADB = x.MADB, SODB = x.SODB,
                                 CHISODAU = x.CHISODAU, CHISOCUOI = x.CHISOCUOI, KLDHTONG = x.KLDHTONG, KLTIEUTHU = x.KLTIEUTHU,
                                 MAMDSD = x.MAMDSD, MABG = x.MABG, MACQ = x.MACQ,
                                 SOHO = x.SOHO, SONK = x.SONK, ISDINHMUC = x.ISDINHMUC, DMNK = x.SODINHMUC,//DMNK = GetDMNK(),
                                 TTSD = x.TTSD, KHONGTINH117 = x.KHONGTINH117, KYHOTRO = x.KYHOTRO,
                                 COPHIMT = x.COPHIMT, //TTHAIGHI = "GDH_BT",
                                 TTHAIGHI=x.TTHAIGHI,
                                 TIENNUOC = 0, THUEBAO = 0, TONGTIEN_PS = 0,TONGTIEN = 0,
                                 PHIBVMT = x.PHIBVMT, PTVAT = x.PTVAT,
                                 CHISODAU_1 = 0, CHISOCUOI_1 = 0, KLTIEUTHU_1 = 0, TONGTIENPS_1 = 0,
                                 INHD_TT = x.INHD_TT.HasValue && x.INHD_TT.Value,
                                 MAHTTT = x.MAHTTT, INHD = false, GHICHU_CS = "",
                                 HETNO = false, THUTQ = false,

                                 TIENPHI = 0, TIENTHUE = 0, KOPHINT = true, KOVAT = false,
                                 M3MUC1 = 0, GIAMUC1 = 0, 
                                 M3MUC2 = 0, GIAMUC2 = 0,   
                                 M3MUC3 = 0, GIAMUC3 = 0,
                                 M3MUC4 = 0, GIAMUC4 = 0,
                                 TNUOCMUC1 = 0, THUEMUC1 = 0, PHIMUC1 = 0,
                                 TNUOCMUC2 = 0, THUEMUC2 = 0, PHIMUC2 = 0, 
                                 TNUOCMUC3 = 0, THUEMUC3 = 0, PHIMUC3 = 0,
                                 TNUOCMUC4 = 0, THUEMUC4 = 0, PHIMUC4 = 0, 
                                 
                                 THBT = "BT",MTRUYTHU=0,
                                 TINHTIEN2THANG = x.TINHTIEN2THANG,
                                 GHI2THANG1LAN = x.GHI2THANG1LAN,ING2T1L=x.ING2T1L
                             });

                    //TODO: tinh tien cho cac khach hang moi tai thoi diem nay
                    foreach (var cs in newCS)
                    {
                        cs.TTHAIGHI = !string.IsNullOrEmpty(cs.TTHAIGHI) ? cs.TTHAIGHI.Trim() : null;
                        cs.MANVN_CS = "admin";
                        cs.THBT = "BT";

                        if (cs.TTSD != "CUP")
                        {
                            ChangeTieuThu(cs);
                            cs.INHD = true;
                        }

                        cs.HETNO = false; cs.THUTQ = false; cs.MANVCN = null; cs.MANVNHAPCN = null;
                        cs.NGAYCN = null; cs.SOPHIEUCN = null; cs.NGAYNHAPCN = null;

                        _db.TIEUTHUs.InsertOnSubmit(cs);
                    }
                }

                _db.SubmitChanges();

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo kỳ ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
            }

            return msg;
        }

        public Message KhoiTaoGhiChiSoN(DateTime date, string makv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var prevDate = date.AddMonths(-1);

                //TODO: Kiểm tra khu vực khởi tạo hay chưa
                var joinQuery = from tt in _db.TIEUTHUs
                                join lck in _db.LOCKSTATUS
                                    on new { tt.THANG, tt.NAM, tt.MADP }
                                    equals new { THANG = lck.KYGT.Month, NAM = lck.KYGT.Year, lck.MADP }
                                where tt.THANG.Equals(date.Month) && tt.NAM.Equals(date.Year) && tt.MAKV.Equals(makv)
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

                //TODO: Thêm lockstatus
                var dpList = new DuongPhoDao().GetList(makv);
                foreach (var dp in dpList)
                {
                    var lockstatus = new LOCKSTATUS
                    {
                        MADP = dp.MADP,
                        DUONGPHU = dp.DUONGPHU,
                        KYGT = date,
                        LOCKGCS = false,
                        LOCKTINHCUOC = false,
                        LOCKCN = false,
                        MAKV = makv

                    };
                    _db.LOCKSTATUS.InsertOnSubmit(lockstatus);

                    //TODO: khoa ghi chi so thang truoc
                    // ReSharper disable AccessToModifiedClosure
                    var prevLockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(dp.MADP) &&
                                                                            l.DUONGPHU.Equals(dp.DUONGPHU) && l.KYGT.Month == prevDate.Month &&
                                                                            l.KYGT.Year == prevDate.Year);
                    // ReSharper restore AccessToModifiedClosure
                    if (prevLockstatus != null)
                    {
                        prevLockstatus.LOCKGCS = true;
                        prevLockstatus.LOCKTINHCUOC = true;
                    }

                    //TODO: Thêm mới duongphogt
                    // ReSharper disable AccessToModifiedClosure
                    var cnt = _db.DUONGPHOGTs.Count(gt => gt.MADP.Equals(dp.MADP) &&
                                                          gt.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                          gt.NAM.Equals(date.Year) &&
                                                          gt.THANG.Equals(date.Month));
                    if (cnt != 0) continue;

                    var prevGT = _db.DUONGPHOGTs.FirstOrDefault(gt => gt.MADP.Equals(dp.MADP) &&
                                                                      gt.DUONGPHU.Equals(dp.DUONGPHU) &&
                                                                      gt.NAM.Equals(prevDate.Year) &&
                                                                      gt.THANG.Equals(prevDate.Month));
                    // ReSharper restore AccessToModifiedClosure
                    if (prevGT == null) continue;

                    var submitGT = new DUONGPHOGT
                    {
                        MADP = prevGT.MADP,
                        DUONGPHU = prevGT.DUONGPHU,
                        NAM = date.Year,
                        THANG = date.Month,
                        MAKV = prevGT.MAKV,
                        MANVG = prevGT.MANVG,
                        MANVT = prevGT.MANVT
                    };

                    _db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                }

                try
                {
                    _db.SubmitChanges();
                }
                catch
                {
                    msg = new Message(MessageConstants.E_GCS_KHOITAO_KYDAKHOITAO, MessageType.Error);
                    // commit
                    trans.Rollback();
                    _db.Connection.Close();
                    return msg;
                }

                //TODO: Đổ dữ liệu từ kỳ ghi thu trước sang bảng TIEUTHU
                foreach (var dp in dpList)
                {
                    var temp = dp;
                    var insertOldQuery = from kh in _db.KHACHHANGs
                                         join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                                         where (kh.MADP == temp.MADP)
                                               && (kh.DUONGPHU == temp.DUONGPHU)
                                               && (tt.NAM == prevDate.Year)
                                               && (tt.THANG == prevDate.Month)
                                               && (kh.MAKV == makv)
                                               && (kh.XOABOKH == false)
                                         select new
                                         {
                                             kh.IDKH,
                                             NAM = date.Year,
                                             THANG = date.Month,
                                             MAKV = makv,
                                             kh.MAKVDN,
                                             kh.MADP,
                                             kh.DUONGPHU,
                                             kh.MADB,
                                             SODB = kh.MADP + kh.DUONGPHU + kh.MADB,

                                             //CHISODAU = tt.TINHTIEN2THANG.Equals("A") ? tt.CHISODAU : tt.CHISOCUOI,
                                             CHISODAU = kh.TTSD.Equals("THAY") ? 0 : tt.CHISOCUOI,
                                             CHISOCUOI = 0,// kh.TTSD.Equals("CUP") ? tt.CHISOCUOI : 0,
                                             //TTHAIGHI = tt.TTHAIGHI.Equals("D") ? "GDH_BT" : tt.TTHAIGHI,
                                             TTHAIGHI = "GDH_BT",
                                             kh.MAMDSD,
                                             kh.MABG,
                                             temp.NONGTHON,
                                             kh.MACQ,
                                             SOHO = kh.ISDINHMUCTAM == true ? 1 : kh.SOHO,
                                             SONK = kh.ISDINHMUCTAM == true ? 1 : kh.SONK,
                                             ISDINHMUC = kh.ISDINHMUCTAM == true ? false : kh.ISDINHMUC,
                                             SODINHMUC = kh.ISDINHMUCTAM == true ? 1 : kh.SODINHMUC,
                                             kh.TTSD,
                                             kh.KHONGTINH117,
                                             kh.KYHOTRO,
                                             COPHIMT = kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1,
                                             PHIBVMT = (decimal)0.05,
                                             PHIBVMT2 = 0,
                                             PTVAT = (kh.VAT.HasValue && !kh.VAT.Value) ? 0 : (decimal)0.05,
                                             CHISODAU_1 = tt.CHISODAU,
                                             CHISOCUOI_1 = tt.CHISOCUOI,
                                             KLTIEUTHU_1 = tt.KLTIEUTHU,
                                             TONGTIENPS_1 = tt.TONGTIEN_PS,
                                             kh.MAHTTT,
                                             INHD_TT = tt.INHD,

                                             //TINHTIEN2THANG= (tt.TINHTIEN2THANG != null && kh.GHI2THANG1LAN.Equals("1")) ? 
                                             //   (tt.TINHTIEN2THANG.Equals("A") ? "B" : "A") : null,                                                
                                             TINHTIEN2THANG = tt.TINHTIEN2THANG,
                                             GHI2THANG1LAN = tt.KLTIEUTHU.Equals(0) ? "1" : "0",
                                             ING2T1L = tt.GHI2THANG1LAN.Equals("1") ? 1 : 0

                                         };

                    var oldCS = insertOldQuery.AsEnumerable() // From here on it's in-process
                        .Select(x => new TIEUTHU
                        {
                            IDKH = x.IDKH,
                            NAM = x.NAM,
                            THANG = x.THANG,
                            MAKV = x.MAKV,
                            MAKVDN = x.MAKVDN,
                            MADP = x.MADP,
                            DUONGPHU = x.DUONGPHU,
                            MADB = x.MADB,
                            SODB = x.SODB,
                            CHISODAU = x.CHISODAU,
                            CHISOCUOI = x.CHISOCUOI,
                            KLTIEUTHU = 0,
                            KLDHTONG = 0,
                            MAMDSD = x.MAMDSD,
                            MABG = x.MABG,
                            MACQ = x.MACQ,
                            SOHO = x.SOHO,
                            SONK = x.SONK,
                            ISDINHMUC = x.ISDINHMUC,                          
                            DMNK = x.SODINHMUC,//DMNK = GetDMNK(),
                            TTSD = x.TTSD,
                            KHONGTINH117 = x.KHONGTINH117,
                            KYHOTRO = x.KYHOTRO,
                            COPHIMT = x.COPHIMT,
                            PHIBVMT = x.PHIBVMT,
                            PTVAT = x.PTVAT,
                            CHISODAU_1 = x.CHISODAU_1,
                            CHISOCUOI_1 = x.CHISOCUOI_1,
                            KLTIEUTHU_1 = x.KLTIEUTHU_1,
                            TONGTIENPS_1 = x.TONGTIENPS_1,
                            MAHTTT = x.MAHTTT,
                            INHD_TT = x.INHD_TT,
                            INHD = false,
                            TIENNUOC = 0,
                            THUEBAO = 0,
                            TONGTIEN_PS = 0,
                            TONGTIEN = 0,
                            GHICHU_CS = "",
                            TTHAIGHI = x.TTHAIGHI,
                            HETNO = false,
                            THUTQ = false,

                            TIENPHI = 0,
                            TIENTHUE = 0,
                            KOPHINT = true,
                            KOVAT = false,
                            M3MUC1 = 0,
                            GIAMUC1 = 0,
                            M3MUC2 = 0,
                            GIAMUC2 = 0,
                            M3MUC3 = 0,
                            GIAMUC3 = 0,
                            M3MUC4 = 0,
                            GIAMUC4 = 0,

                            TNUOCMUC1 = 0,
                            THUEMUC1 = 0,
                            PHIMUC1 = 0,
                            TNUOCMUC2 = 0,
                            THUEMUC2 = 0,
                            PHIMUC2 = 0,
                            TNUOCMUC3 = 0,
                            THUEMUC3 = 0,
                            PHIMUC3 = 0,
                            TNUOCMUC4 = 0,
                            THUEMUC4 = 0,
                            PHIMUC4 = 0,

                            THBT = "BT",
                            MTRUYTHU = 0,
                            TINHTIEN2THANG = x.TINHTIEN2THANG,
                            GHI2THANG1LAN = x.GHI2THANG1LAN,
                            ING2T1L = x.ING2T1L

                        });

                    _db.TIEUTHUs.InsertAllOnSubmit(oldCS);
                }

                _db.SubmitChanges();


                //TODO:Đổ dữ liệu khách hàng mới trong kỳ ghi thu hiện tại vô TIEUTHU
                foreach (var dp in dpList)
                {
                    var temp = dp;

                    //TODO: Thêm mới các khách hàng tiêu thụ tháng này
                    var insertNewQuery = from kh in _db.KHACHHANGs
                                         join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                                         into khTt
                                         // left outer join
                                         from tt in khTt.Where(t => t.NAM == date.Year && t.THANG == date.Month).DefaultIfEmpty()
                                         where (tt.IDKH == null)
                                             && (kh.MADP == temp.MADP)
                                             && (kh.DUONGPHU == temp.DUONGPHU)
                                             && (kh.MAKV == makv)
                                             && (kh.XOABOKH == false)
                                         select new
                                         {
                                             kh.IDKH,
                                             NAM = date.Year,
                                             THANG = date.Month,
                                             MAKV = makv,
                                             kh.MAKVDN,
                                             kh.MADP,
                                             kh.DUONGPHU,
                                             kh.MADB,
                                             SODB = (kh.MADP + kh.DUONGPHU + kh.MADB),

                                             //CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                                             //CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                                             //CHISODAU = kh.CHISODAU,
                                             //CHISOCUOI = kh.CHISOCUOI,
                                             CHISODAU = kh.CHISOCUOI,
                                             CHISOCUOI = 0,
                                             KLTIEUTHU = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                             kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                                             KLDHTONG = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                             kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,

                                             kh.MAMDSD,
                                             kh.MABG,
                                             temp.NONGTHON,
                                             kh.MACQ,
                                             kh.SOHO,
                                             kh.SONK,
                                             kh.ISDINHMUC,kh.SODINHMUC,
                                             kh.TTSD,
                                             kh.KHONGTINH117,
                                             kh.KYHOTRO,
                                             COPHIMT = kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1,
                                             TTHAIGHI = kh.MATT.Equals("CUP") ? "CUP" : "GDH_BT",
                                             PHIBVMT = 100,
                                             PHIBVMT2 = 0,
                                             PTVAT = (kh.VAT.HasValue && !kh.VAT.Value) ? 0 : (decimal)0.05,
                                             kh.MAHTTT,
                                             INHD_TT = tt.INHD,

                                             TINHTIEN2THANG = (kh.GHI2THANG1LAN == "1") ? "A" : null,

                                             GHI2THANG1LAN = "0",
                                             ING2T1L = (kh.GHI2THANG1LAN == "1") ? 1 : 0

                                         };

                    var newCS = insertNewQuery.AsEnumerable()
                             .Select(x => new TIEUTHU
                             {
                                 IDKH = x.IDKH,
                                 NAM = x.NAM,
                                 THANG = x.THANG,
                                 MAKV = x.MAKV,
                                 MAKVDN = x.MAKVDN,
                                 MADP = x.MADP,
                                 DUONGPHU = x.DUONGPHU,
                                 MADB = x.MADB,
                                 SODB = x.SODB,
                                 CHISODAU = x.CHISODAU,
                                 CHISOCUOI = x.CHISOCUOI,
                                 KLDHTONG = x.KLDHTONG,
                                 KLTIEUTHU = x.KLTIEUTHU,
                                 MAMDSD = x.MAMDSD,
                                 MABG = x.MABG,
                                 MACQ = x.MACQ,
                                 SOHO = x.SOHO,
                                 SONK = x.SONK,
                                 ISDINHMUC = x.ISDINHMUC,
                                 DMNK = x.SODINHMUC,
                                 TTSD = x.TTSD,
                                 KHONGTINH117 = x.KHONGTINH117,
                                 KYHOTRO = x.KYHOTRO,
                                 COPHIMT = x.COPHIMT, //TTHAIGHI = "GDH_BT",
                                 TTHAIGHI = x.TTHAIGHI,
                                 TIENNUOC = 0,
                                 THUEBAO = 0,
                                 TONGTIEN_PS = 0,
                                 TONGTIEN = 0,
                                 PHIBVMT = x.PHIBVMT,
                                 PTVAT = x.PTVAT,
                                 CHISODAU_1 = 0,
                                 CHISOCUOI_1 = 0,
                                 KLTIEUTHU_1 = 0,
                                 TONGTIENPS_1 = 0,
                                 INHD_TT = x.INHD_TT.HasValue && x.INHD_TT.Value,
                                 MAHTTT = x.MAHTTT,
                                 INHD = false,
                                 GHICHU_CS = "",
                                 HETNO = false,
                                 THUTQ = false,

                                 TIENPHI = 0,
                                 TIENTHUE = 0,
                                 KOPHINT = true,
                                 KOVAT = false,
                                 M3MUC1 = 0,
                                 GIAMUC1 = 0,
                                 M3MUC2 = 0,
                                 GIAMUC2 = 0,
                                 M3MUC3 = 0,
                                 GIAMUC3 = 0,
                                 M3MUC4 = 0,
                                 GIAMUC4 = 0,

                                 TNUOCMUC1 = 0,
                                 THUEMUC1 = 0,
                                 PHIMUC1 = 0,
                                 TNUOCMUC2 = 0,
                                 THUEMUC2 = 0,
                                 PHIMUC2 = 0,
                                 TNUOCMUC3 = 0,
                                 THUEMUC3 = 0,
                                 PHIMUC3 = 0,
                                 TNUOCMUC4 = 0,
                                 THUEMUC4 = 0,
                                 PHIMUC4 = 0,

                                 THBT = "BT",
                                 MTRUYTHU = 0,
                                 TINHTIEN2THANG = x.TINHTIEN2THANG,
                                 GHI2THANG1LAN = x.GHI2THANG1LAN,
                                 ING2T1L = x.ING2T1L
                             });

                    //TODO: tinh tien cho cac khach hang moi tai thoi diem nay
                    foreach (var cs in newCS)
                    {
                        cs.TTHAIGHI = !string.IsNullOrEmpty(cs.TTHAIGHI) ? cs.TTHAIGHI.Trim() : null;
                        cs.MANVN_CS = "admin";
                        cs.THBT = "BT";

                        if (cs.TTSD != "CUP")
                        {
                            ChangeTieuThu(cs);
                            cs.INHD = true;
                        }

                        cs.HETNO = false; cs.THUTQ = false; cs.MANVCN = null; cs.MANVNHAPCN = null;
                        cs.NGAYCN = null; cs.SOPHIEUCN = null; cs.NGAYNHAPCN = null;

                        _db.TIEUTHUs.InsertOnSubmit(cs);
                    }
                }

                _db.SubmitChanges();

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo kỳ ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
            }

            return msg;
        }

        public Message KhoiTaoGhiChiSo(DateTime date, KHACHHANG kh)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var prevDate = date.AddMonths(-1);
                var lockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(kh.MADP) &&
                                                                    l.DUONGPHU.Equals(kh.DUONGPHU) && l.KYGT.Month == date.Month &&
                                                                    l.KYGT.Year == date.Year);
                if (lockstatus == null) {
                    //msg = new Message(MessageConstants.E_GCS_KHOITAO_KYCHUAKHOITAO, MessageType.Error);
                    //return msg;
                    var khoakyghi = new LOCKSTATUS
                    {
                        MADP = kh.MADP,
                        DUONGPHU = kh.DUONGPHU,
                        KYGT = date,
                        LOCKGCS = false,
                        LOCKTINHCUOC = false,
                        LOCKCN = false,
                        MAKV=kh.MADP.Substring(0,1)
                    };
                    _db.LOCKSTATUS.InsertOnSubmit(khoakyghi);

                    //var submitGT = new DUONGPHOGT
                    //{
                    //    MADP = kh.MADP,
                    //    DUONGPHU = kh.DUONGPHU,
                    //    NAM = date.Year,
                    //    THANG = date.Month,
                    //    MAKV = kh.MAKV,
                    //    MANVG = "admin",
                    //    MANVT = "admin"
                    //};
                    //_db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                }

                //var prevtt = _db.TIEUTHUs.SingleOrDefault(t => t.IDKH.Equals(kh.IDKH) &&
               //                                                t.NAM.Equals(prevDate.Year) &&
               //                                                t.THANG.Equals(prevDate.Month));

                var prevtt = (from khm in _db.KHACHHANGs
                             join tt in _db.TIEUTHUs on khm.IDKH equals tt.IDKH
                             where khm.IDKH.Equals(kh.IDKH)
                                   && (tt.NAM == prevDate.Year)
                                   && (tt.THANG == prevDate.Month)                                 
                                   && (khm.XOABOKH == false)
                             select tt).SingleOrDefault();

                // tồn tại tiêu thụ tháng trước, copy qua tháng này
                if(prevtt != null)
                {
                    var tt = new TIEUTHU
                                 {
                                     IDKH = kh.IDKH, NAM = date.Year, THANG = date.Month, MAKV = kh.MAKV, MAKVDN = kh.MAKVDN,
                                     MADP = kh.MADP, DUONGPHU = kh.DUONGPHU, MADB = kh.MADB, SODB = kh.MADP + kh.MADB,
                                     CHISODAU = prevtt.CHISOCUOI, CHISOCUOI = kh.TTSD.Equals("CUP") ? prevtt.CHISOCUOI : 0, KLDHTONG = 0, KLTIEUTHU = 0,
                                     
                                     MAMDSD = kh.MAMDSD, MABG = kh.MABG, MACQ = kh.MACQ,

                                     SOHO = kh.ISDINHMUCTAM == true ? 1 : kh.SOHO,
                                     SONK = kh.ISDINHMUCTAM == true ? 1 : kh.SONK,
                                     ISDINHMUC = kh.ISDINHMUCTAM == true ? false : kh.ISDINHMUC,
                                     DMNK = kh.ISDINHMUCTAM == true ? 1 : kh.SODINHMUC,//DMNK = GetDMNK(), 

                                     KHONGTINH117 = kh.KHONGTINH117,
                                     
                                     TTSD = kh.TTSD, KYHOTRO = kh.KYHOTRO,
                                     COPHIMT = (kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1), 
                                     TTHAIGHI = "GDH_BT", 
                                     
                                     TIENNUOC = 0, THUEBAO = 0, TONGTIEN_PS = 0, PHIBVMT = (decimal)0.05, PTVAT = (decimal)0.05,

                                     TIENPHI = 0,  TIENTHUE = 0,KOPHINT = true,           KOVAT = false,
                                     M3MUC1 = 0,  GIAMUC1 = 0,   
                                     M3MUC2 = 0,  GIAMUC2 = 0, 
                                     M3MUC3 = 0,  GIAMUC3 = 0,
                                     M3MUC4 = 0,  GIAMUC4 = 0,
                                     TNUOCMUC1 = 0, THUEMUC1 = 0,PHIMUC1 = 0, 
                                     TNUOCMUC2 = 0, THUEMUC2 = 0,PHIMUC2 = 0, 
                                     TNUOCMUC3 = 0, THUEMUC3 = 0,PHIMUC3 = 0,
                                     TNUOCMUC4 = 0, THUEMUC4 = 0,PHIMUC4 = 0,
                                     
                                     THBT = "BT",
                                     MTRUYTHU = 0,  //TINHTIEN2THANG = x.TINHTIEN2THANG,
                                     
                                     CHISODAU_1 = prevtt.CHISODAU, CHISOCUOI_1 = prevtt.CHISOCUOI, KLTIEUTHU_1 = prevtt.KLTIEUTHU, TONGTIENPS_1 = prevtt.TONGTIEN_PS,
                                     MAHTTT = "TM", INHD = false, INHD_TT = prevtt.INHD, GHICHU_CS = "", MANVN_CS = null, 
                                     HETNO = false, THUTQ = false, 
                                     MANVCN = null, NGAYCN = null, MANVNHAPCN = null, NGAYNHAPCN = null, GHICHUCN = null, SOPHIEUCN = null, TIENCONGNO = null,
                                     
                                 };
                    _db.TIEUTHUs.InsertOnSubmit(tt);
                }
                // không tồn tại, khởi tạo mới
                else
                {

                    var tt = new TIEUTHU
                    {
                        IDKH = kh.IDKH, NAM = date.Year, THANG = date.Month, MAKV = kh.MAKV, MAKVDN = kh.MAKVDN,
                        MADP = kh.MADP, DUONGPHU = kh.DUONGPHU, MADB = kh.MADB, SODB = kh.MADP + kh.MADB,
                        CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                        CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                        KLDHTONG = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                         kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                        KLTIEUTHU = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                         kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                        MTRUYTHU = kh.MTRUYTHU.HasValue ? kh.MTRUYTHU.Value : 0 ,
                        MAMDSD = kh.MAMDSD, MABG = kh.MABG, MACQ = kh.MACQ,
                        SOHO = kh.SOHO, SONK = kh.SONK, ISDINHMUC = kh.ISDINHMUC, DMNK = kh.SODINHMUC,//DMNK = GetDMNK(), 
                        KHONGTINH117 = kh.KHONGTINH117,

                        TTSD = kh.TTSD, KYHOTRO = kh.KYHOTRO, COPHIMT = (kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1),
                        TTHAIGHI = "GDH_BT", //THBT = "THGS",
                        TIENNUOC = 0, THUEBAO = 0, TONGTIEN_PS = 0, PHIBVMT = (decimal)0.05, PTVAT = (decimal)0.05,  
                        CHISODAU_1 = 0, CHISOCUOI_1 = 0, KLTIEUTHU_1 = 0, TONGTIENPS_1 = 0,
                        MAHTTT = "TM", INHD = ((kh.CHISOCUOI-kh.CHISODAU)>0)?true:false,
                        INHD_TT = false, GHICHU_CS = "", MANVN_CS = null,
                        HETNO = false, THUTQ = false,
                        MANVCN = null, NGAYCN = null, MANVNHAPCN = null, NGAYNHAPCN = null, 
                        GHICHUCN = null, SOPHIEUCN = null, TIENCONGNO = null,

                        TIENPHI = 0,TIENTHUE = 0,KOPHINT = true, KOVAT = false, 
                        M3MUC1 = 0, GIAMUC1 = 0,
                        M3MUC2 = 0, GIAMUC2 = 0, 
                        M3MUC3 = 0, GIAMUC3 = 0,
                        M3MUC4 = 0, GIAMUC4 = 0, 
                        TNUOCMUC1 = 0, THUEMUC1 = 0,PHIMUC1 = 0,
                        TNUOCMUC2 = 0, THUEMUC2 = 0,PHIMUC2 = 0, 
                        TNUOCMUC3 = 0, THUEMUC3 = 0,PHIMUC3 = 0,
                        TNUOCMUC4 = 0, THUEMUC4 = 0,PHIMUC4 = 0,
                        THBT = "BT", //MTRUYTHU = kh.MTRUYTHU,  //TINHTIEN2THANG = x.TINHTIEN2THANG,
                        GHI2THANG1LAN = kh.GHI2THANG1LAN,
                        ING2T1L = kh.GHI2THANG1LAN.Equals(1) ? 1 : 0

                    };   
               
                    _db.TIEUTHUs.InsertOnSubmit(tt);

                    if (kh.MAKV == "X")
                    {
                        ChangeTieuThuLX(tt);
                    }
                    else
                    {
                        ChangeTieuThu(tt);
                    }
                }

                _db.SubmitChanges();
                trans.Commit();
                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo chỉ số cho khách hàng");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Khởi tạo chỉ số");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "Khởi tạo chỉ số");
            }

            return msg;
        }

        public Message KhoiTaoGhiChiSoToKHMLX(DateTime date, string makvlx)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                //var dskh = _db.KHACHHANGs.Where(p => p.KYKHAITHAC.Month.Equals(date.Month) && p.KYKHAITHAC.Year.Equals(date.Year));
                var dskh = from k in _db.KHACHHANGs
                           where k.KYKHAITHAC.Value.Month.Equals(date.Month) && k.KYKHAITHAC.Value.Year.Equals(date.Year) && k.MAKV.Equals(makvlx) &&
                            !(from t in _db.TIEUTHUs
                              where t.THANG.Equals(date.Month) && t.NAM.Equals(date.Year) && t.MAKV.Equals(makvlx)
                              select t.IDKH).Contains(k.IDKH)
                           select k;

                foreach (var kh in dskh)
                {
                    var prevDate = date.AddMonths(-1);
                    var lockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(kh.MADP) &&
                                                                        l.DUONGPHU.Equals(kh.DUONGPHU) && l.KYGT.Month == date.Month &&
                                                                        l.KYGT.Year == date.Year);
                    if (lockstatus == null)
                    {
                        //msg = new Message(MessageConstants.E_GCS_KHOITAO_KYCHUAKHOITAO, MessageType.Error);
                        //return msg;
                        var khoakyghi = new LOCKSTATUS
                        {
                            MADP = kh.MADP,
                            DUONGPHU = kh.DUONGPHU,
                            KYGT = date,
                            LOCKGCS = false,
                            LOCKTINHCUOC = false,
                            LOCKCN = false,
                            MAKV = kh.MADP.Substring(0, 1)
                        };
                        _db.LOCKSTATUS.InsertOnSubmit(khoakyghi);

                        var submitGT = new DUONGPHOGT
                        {
                            MADP = kh.MADP,
                            DUONGPHU = kh.DUONGPHU,
                            NAM = date.Year,
                            THANG = date.Month,
                            MAKV = kh.MAKV,
                            MANVG = "admin",
                            MANVT = "admin"
                        };
                        _db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                    }

                    //var prevtt = _db.TIEUTHUs.SingleOrDefault(t => t.IDKH.Equals(kh.IDKH) &&
                    //                                                t.NAM.Equals(prevDate.Year) &&
                    //                                                t.THANG.Equals(prevDate.Month));

                    var prevtt = (from khm in _db.KHACHHANGs
                                  join tt in _db.TIEUTHUs on khm.IDKH equals tt.IDKH
                                  where khm.IDKH.Equals(kh.IDKH)
                                        && (tt.NAM == prevDate.Year)
                                        && (tt.THANG == prevDate.Month)
                                        && (khm.XOABOKH == false)
                                  select tt).SingleOrDefault();

                    // tồn tại tiêu thụ tháng trước, copy qua tháng này
                    if (prevtt != null)
                    {
                        var tt = new TIEUTHU
                        {
                            IDKH = kh.IDKH,
                            NAM = date.Year,
                            THANG = date.Month,
                            MAKV = kh.MAKV,
                            MAKVDN = kh.MAKVDN,
                            MADP = kh.MADP,
                            DUONGPHU = kh.DUONGPHU,
                            MADB = kh.MADB,
                            SODB = kh.MADP + kh.MADB,
                            CHISODAU = prevtt.CHISOCUOI,
                            CHISOCUOI = kh.TTSD.Equals("CUP") ? prevtt.CHISOCUOI : 0,
                            KLDHTONG = 0,
                            KLTIEUTHU = 0,

                            MAMDSD = kh.MAMDSD,
                            MABG = kh.MABG,
                            MACQ = kh.MACQ,

                            SOHO = kh.ISDINHMUCTAM == true ? 1 : kh.SOHO,
                            SONK = kh.ISDINHMUCTAM == true ? 1 : kh.SONK,
                            ISDINHMUC = kh.ISDINHMUCTAM == true ? false : kh.ISDINHMUC,
                            DMNK = kh.ISDINHMUCTAM == true ? 1 : kh.SODINHMUC,//DMNK = GetDMNK(), 

                            KHONGTINH117 = kh.KHONGTINH117,

                            TTSD = kh.TTSD,
                            KYHOTRO = kh.KYHOTRO,
                            COPHIMT = (kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1),
                            TTHAIGHI = "GDH_BT",

                            TIENNUOC = 0,
                            THUEBAO = 0,
                            TONGTIEN_PS = 0,
                            PHIBVMT = (decimal)0.05,
                            PTVAT = (decimal)0.05,

                            TIENPHI = 0,
                            TIENTHUE = 0,
                            KOPHINT = true,
                            KOVAT = false,
                            M3MUC1 = 0,
                            GIAMUC1 = 0,
                            M3MUC2 = 0,
                            GIAMUC2 = 0,
                            M3MUC3 = 0,
                            GIAMUC3 = 0,
                            M3MUC4 = 0,
                            GIAMUC4 = 0,
                            TNUOCMUC1 = 0,
                            THUEMUC1 = 0,
                            PHIMUC1 = 0,
                            TNUOCMUC2 = 0,
                            THUEMUC2 = 0,
                            PHIMUC2 = 0,
                            TNUOCMUC3 = 0,
                            THUEMUC3 = 0,
                            PHIMUC3 = 0,
                            TNUOCMUC4 = 0,
                            THUEMUC4 = 0,
                            PHIMUC4 = 0,

                            THBT = "BT",
                            MTRUYTHU = 0,  //TINHTIEN2THANG = x.TINHTIEN2THANG,

                            CHISODAU_1 = prevtt.CHISODAU,
                            CHISOCUOI_1 = prevtt.CHISOCUOI,
                            KLTIEUTHU_1 = prevtt.KLTIEUTHU,
                            TONGTIENPS_1 = prevtt.TONGTIEN_PS,
                            MAHTTT = "TM",
                            INHD = false,
                            INHD_TT = prevtt.INHD,
                            GHICHU_CS = "",
                            MANVN_CS = null,
                            HETNO = false,
                            THUTQ = false,
                            MANVCN = null,
                            NGAYCN = null,
                            MANVNHAPCN = null,
                            NGAYNHAPCN = null,
                            GHICHUCN = null,
                            SOPHIEUCN = null,
                            TIENCONGNO = null,

                        };
                        _db.TIEUTHUs.InsertOnSubmit(tt);
                    }
                    // không tồn tại, khởi tạo mới
                    else
                    {

                        var tt = new TIEUTHU
                        {
                            IDKH = kh.IDKH,
                            NAM = date.Year,
                            THANG = date.Month,
                            MAKV = kh.MAKV,
                            MAKVDN = kh.MAKVDN,
                            MADP = kh.MADP,
                            DUONGPHU = kh.DUONGPHU,
                            MADB = kh.MADB,
                            SODB = kh.MADP + kh.MADB,
                            CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                            CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                            KLDHTONG = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                             kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                            KLTIEUTHU = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                             kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,

                            MAMDSD = kh.MAMDSD,
                            MABG = kh.MABG,
                            MACQ = kh.MACQ,
                            SOHO = kh.SOHO,
                            SONK = kh.SONK,
                            ISDINHMUC = kh.ISDINHMUC,
                            DMNK = kh.SODINHMUC,//DMNK = GetDMNK(), 
                            KHONGTINH117 = kh.KHONGTINH117,

                            TTSD = kh.TTSD,
                            KYHOTRO = kh.KYHOTRO,
                            COPHIMT = (kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1),
                            TTHAIGHI = "GDH_BT", //THBT = "THGS",
                            TIENNUOC = 0,
                            THUEBAO = 0,
                            TONGTIEN_PS = 0,
                            PHIBVMT = (decimal)0.05,
                            PTVAT = (decimal)0.05,
                            CHISODAU_1 = 0,
                            CHISOCUOI_1 = 0,
                            KLTIEUTHU_1 = 0,
                            TONGTIENPS_1 = 0,
                            MAHTTT = "TM",
                            INHD = ((kh.CHISOCUOI - kh.CHISODAU) > 0) ? true : false,
                            INHD_TT = false,
                            GHICHU_CS = "",
                            MANVN_CS = null,
                            HETNO = false,
                            THUTQ = false,
                            MANVCN = null,
                            NGAYCN = null,
                            MANVNHAPCN = null,
                            NGAYNHAPCN = null,
                            GHICHUCN = null,
                            SOPHIEUCN = null,
                            TIENCONGNO = null,

                            TIENPHI = 0,
                            TIENTHUE = 0,
                            KOPHINT = true,
                            KOVAT = false,
                            M3MUC1 = 0,
                            GIAMUC1 = 0,
                            M3MUC2 = 0,
                            GIAMUC2 = 0,
                            M3MUC3 = 0,
                            GIAMUC3 = 0,
                            M3MUC4 = 0,
                            GIAMUC4 = 0,
                            TNUOCMUC1 = 0,
                            THUEMUC1 = 0,
                            PHIMUC1 = 0,
                            TNUOCMUC2 = 0,
                            THUEMUC2 = 0,
                            PHIMUC2 = 0,
                            TNUOCMUC3 = 0,
                            THUEMUC3 = 0,
                            PHIMUC3 = 0,
                            TNUOCMUC4 = 0,
                            THUEMUC4 = 0,
                            PHIMUC4 = 0,
                            THBT = "BT",
                            MTRUYTHU = kh.MTRUYTHU,  //TINHTIEN2THANG = x.TINHTIEN2THANG,
                            GHI2THANG1LAN = kh.GHI2THANG1LAN,
                            ING2T1L = kh.GHI2THANG1LAN.Equals(1) ? 1 : 0

                        };

                        _db.TIEUTHUs.InsertOnSubmit(tt);

                        if (kh.MAKV == "X")
                        {
                            ChangeTieuThuLX(tt);
                        }
                        else
                        {
                            ChangeTieuThu(tt);
                        }
                    }
                    _db.SubmitChanges();
                }
                
                trans.Commit();
                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo chỉ số cho khách hàng");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Khởi tạo chỉ số");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "Khởi tạo chỉ số");
            }

            return msg;
        }

        public Message KhoiTaoGhiChiSoTam(DateTime date, KHACHHANG_T kh)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var prevDate = date.AddMonths(-1);
                var lockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP.Equals(kh.MADP) &&
                                                                    l.DUONGPHU.Equals(kh.DUONGPHU) && l.KYGT.Month == date.Month &&
                                                                    l.KYGT.Year == date.Year);
                if (lockstatus == null)
                {
                    //msg = new Message(MessageConstants.E_GCS_KHOITAO_KYCHUAKHOITAO, MessageType.Error);
                    //return msg;
                    var khoakyghi = new LOCKSTATUS
                    {
                        MADP = kh.MADP,
                        DUONGPHU = kh.DUONGPHU,
                        KYGT = date,
                        LOCKGCS = false,
                        LOCKTINHCUOC = false,
                        LOCKCN = false,
                        MAKV = kh.MADP.Substring(0, 1)
                    };
                    _db.LOCKSTATUS.InsertOnSubmit(khoakyghi);

                    var submitGT = new DUONGPHOGT
                    {
                        MADP = kh.MADP,
                        DUONGPHU = kh.DUONGPHU,
                        NAM = date.Year,
                        THANG = date.Month,
                        MAKV = kh.MAKV,
                        MANVG = "admin",
                        MANVT = "admin"
                    };
                    _db.DUONGPHOGTs.InsertOnSubmit(submitGT);
                }

                //var prevtt = _db.TIEUTHUTEMPs.SingleOrDefault(t => t.IDKH.Equals(kh.IDKH) &&
                //                                               t.NAM.Equals(prevDate.Year) &&
                //                                               t.THANG.Equals(prevDate.Month));

                var prevtt = (from khm in _db.KHACHHANGs
                              join tt in _db.TIEUTHUs on khm.IDKH equals tt.IDKH
                              where khm.IDKH.Equals(kh.IDKH)
                                    && (tt.NAM == prevDate.Year)
                                    && (tt.THANG == prevDate.Month)
                                    && (khm.XOABOKH == false)
                              select tt).SingleOrDefault();

                // tồn tại tiêu thụ tháng trước, copy qua tháng này
                if (prevtt != null)
                {
                    var tt = new TIEUTHUTEMP
                    {
                        IDKH = kh.IDKH,
                        NAM = date.Year,
                        THANG = date.Month,
                        MAKV = kh.MAKV,
                        MAKVDN = kh.MAKVDN,
                        MADP = kh.MADP,
                        DUONGPHU = kh.DUONGPHU,
                        MADB = kh.MADB,
                        SODB = kh.MADP + kh.MADB,
                        CHISODAU = prevtt.CHISOCUOI,
                        CHISOCUOI = kh.TTSD.Equals("CUP") ? prevtt.CHISOCUOI : 0,
                        KLDHTONG = 0,
                        KLTIEUTHU = 0,

                        MAMDSD = kh.MAMDSD,
                        MABG = kh.MABG,
                        MACQ = kh.MACQ,
                        SOHO = kh.SOHO,
                        SONK = kh.SONK,
                        ISDINHMUC = kh.ISDINHMUC,
                        DMNK = kh.SODINHMUC,
                        KHONGTINH117 = kh.KHONGTINH117,

                        TTSD = kh.TTSD,
                        KYHOTRO = kh.KYHOTRO,
                        COPHIMT = (kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1),
                        TTHAIGHI = "GDH_BT",

                        TIENNUOC = 0,
                        THUEBAO = 0,
                        TONGTIEN_PS = 0,
                        PHIBVMT = (decimal)0.05,
                        PTVAT = (decimal)0.05,

                        TIENPHI = 0,
                        TIENTHUE = 0,
                        KOPHINT = true,
                        KOVAT = false,
                        M3MUC1 = 0,
                        GIAMUC1 = 0,
                        M3MUC2 = 0,
                        GIAMUC2 = 0,
                        M3MUC3 = 0,
                        GIAMUC3 = 0,
                        M3MUC4 = 0,
                        GIAMUC4 = 0,

                        TNUOCMUC1 = 0,
                        THUEMUC1 = 0,
                        PHIMUC1 = 0,
                        TNUOCMUC2 = 0,
                        THUEMUC2 = 0,
                        PHIMUC2 = 0,
                        TNUOCMUC3 = 0,
                        THUEMUC3 = 0,
                        PHIMUC3 = 0,
                        TNUOCMUC4 = 0,
                        THUEMUC4 = 0,
                        PHIMUC4 = 0,

                        THBT = "BT",
                        MTRUYTHU = 0,  //TINHTIEN2THANG = x.TINHTIEN2THANG,

                        CHISODAU_1 = prevtt.CHISODAU,
                        CHISOCUOI_1 = prevtt.CHISOCUOI,
                        KLTIEUTHU_1 = prevtt.KLTIEUTHU,
                        TONGTIENPS_1 = prevtt.TONGTIEN_PS,
                        MAHTTT = "TM",
                        INHD = false,
                        INHD_TT = prevtt.INHD,
                        GHICHU_CS = "",
                        MANVN_CS = null,
                        HETNO = false,
                        THUTQ = false,
                        MANVCN = null,
                        NGAYCN = null,
                        MANVNHAPCN = null,
                        NGAYNHAPCN = null,
                        GHICHUCN = null,
                        SOPHIEUCN = null,
                        TIENCONGNO = null
                    };
                    _db.TIEUTHUTEMPs.InsertOnSubmit(tt);
                }
                // không tồn tại, khởi tạo mới
                else
                {

                    var tt = new TIEUTHUTEMP
                    {
                        IDKH = kh.IDKH,
                        NAM = date.Year,
                        THANG = date.Month,
                        MAKV = kh.MAKV,
                        MAKVDN = kh.MAKVDN,
                        MADP = kh.MADP,
                        DUONGPHU = kh.DUONGPHU,
                        MADB = kh.MADB,
                        SODB = kh.MADP + kh.MADB,
                        CHISODAU = kh.CHISODAU.HasValue ? kh.CHISODAU.Value : 0,
                        CHISOCUOI = kh.CHISOCUOI.HasValue ? kh.CHISOCUOI.Value : 0,
                        KLDHTONG = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                         kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,
                        KLTIEUTHU = kh.CHISODAU.HasValue && kh.CHISOCUOI.HasValue ?
                                                         kh.CHISOCUOI.Value - kh.CHISODAU.Value : 0,

                        MAMDSD = kh.MAMDSD,
                        MABG = kh.MABG,
                        MACQ = kh.MACQ,
                        SOHO = kh.SOHO,
                        SONK = kh.SONK,
                        ISDINHMUC = kh.ISDINHMUC,
                        DMNK = kh.SODINHMUC,
                        KHONGTINH117 = kh.KHONGTINH117,

                        TTSD = kh.TTSD,
                        KYHOTRO = kh.KYHOTRO,
                        COPHIMT = (kh.KOPHINT.HasValue ? (kh.KOPHINT.Value.Equals(true) ? 0 : 1) : 1),
                        TTHAIGHI = "GDH_BT", //THBT = "THGS",
                        TIENNUOC = 0,
                        THUEBAO = 0,
                        TONGTIEN_PS = 0,
                        PHIBVMT = (decimal)0.05,
                        PTVAT = (decimal)0.05,
                        CHISODAU_1 = 0,
                        CHISOCUOI_1 = 0,
                        KLTIEUTHU_1 = 0,
                        TONGTIENPS_1 = 0,
                        MAHTTT = "TM",
                        INHD = ((kh.CHISOCUOI - kh.CHISODAU) > 0) ? true : false,
                        INHD_TT = false,
                        GHICHU_CS = "",
                        MANVN_CS = null,
                        HETNO = false,
                        THUTQ = false,
                        MANVCN = null,
                        NGAYCN = null,
                        MANVNHAPCN = null,
                        NGAYNHAPCN = null,
                        GHICHUCN = null,
                        SOPHIEUCN = null,
                        TIENCONGNO = null,

                        TIENPHI = 0,
                        TIENTHUE = 0,
                        KOPHINT = true,
                        KOVAT = false,
                        M3MUC1 = 0,
                        GIAMUC1 = 0,
                        M3MUC2 = 0,
                        GIAMUC2 = 0,
                        M3MUC3 = 0,
                        GIAMUC3 = 0,
                        M3MUC4 = 0,
                        GIAMUC4 = 0,

                        TNUOCMUC1 = 0,
                        THUEMUC1 = 0,
                        PHIMUC1 = 0,
                        TNUOCMUC2 = 0,
                        THUEMUC2 = 0,
                        PHIMUC2 = 0,
                        TNUOCMUC3 = 0,
                        THUEMUC3 = 0,
                        PHIMUC3 = 0,
                        TNUOCMUC4 = 0,
                        THUEMUC4 = 0,
                        PHIMUC4 = 0,

                        THBT = "BT",
                        MTRUYTHU = 0,  //TINHTIEN2THANG = x.TINHTIEN2THANG,
                        GHI2THANG1LAN = kh.GHI2THANG1LAN,
                        ING2T1L = kh.GHI2THANG1LAN.Equals(1) ? 1 : 0
                    };

                    _db.TIEUTHUTEMPs.InsertOnSubmit(tt);
                    ChangeTieuThuTam(tt);
                }

                _db.SubmitChanges();
                trans.Commit();
                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo chỉ số cho khách hàng");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Khởi tạo chỉ số");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "Khởi tạo chỉ số");
            }

            return msg;
        }

        public Message KhoiTaoGhiChiSoBoSung(DateTime date, string makv)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;
                
                var joinQuery = from kh in _db.KHACHHANGs
                                join tt in _db.TIEUTHUs on kh.IDKH equals tt.IDKH
                                where (tt.TTSD != "CUP") && (tt.INHD == true) && tt.THANG == date.Month 
                                    && (tt.MAKV == makv || makv == "%") && kh.XOABOKH.Equals(false)
                                select tt;

                foreach (var tieuthu in joinQuery)
                {
                    ChangeTieuThu(tieuthu);
                }

                _db.SubmitChanges();
                
                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khởi tạo kỳ ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Khởi tạo kỳ ghi chỉ số");
            }
            return msg;
        }
        
        public List<TINHTIEN> TongTienNuoc(Int64 kltt, string mdsd, decimal thuegtgt, decimal thuemt, int heso, int sodinhmuc, int m3m1)
        {

            var dm = heso * sodinhmuc * m3m1;

            // vượt 3 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                            kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ?
                                dm : (kltt - (2 * dm)) < 0 ? 0 : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? 
                                dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            // gia tung muc co thue * m3
            var giacothuem1 = m3muc1 > 0 ? GetGiaCoThue(mdsd, 1) * m3muc1 : 0;
            var giacothuem2 = m3muc2 > 0 ? GetGiaCoThue(mdsd, 2) * m3muc2 : 0;
            var giacothuem3 = m3muc3 > 0 ? GetGiaCoThue(mdsd, 3) * m3muc3 : 0;
            var giacothuem4 = m3muc4 > 0 ? GetGiaCoThue(mdsd, 4) * m3muc4 : 0;

            // gia tung muc chua thue
            var giamuc1 = giacothuem1 == 0 ? 0 : Math.Round(giacothuem1 / thuegtgt, 0, MidpointRounding.AwayFromZero);
            var giamuc2 = giacothuem2 == 0 ? 0 : Math.Round(giacothuem2 / thuegtgt, 0, MidpointRounding.AwayFromZero);
            var giamuc3 = giacothuem3 == 0 ? 0 : Math.Round(giacothuem3 / thuegtgt, 0, MidpointRounding.AwayFromZero);
            var giamuc4 = giacothuem4 == 0 ? 0 : Math.Round(giacothuem4 / thuegtgt, 0, MidpointRounding.AwayFromZero);

            var tonggia = giamuc1 + giamuc2 + giamuc3 + giamuc4 ;
           

            // thue GTGT tung muc
            var giathuem1 = Math.Round(giacothuem1 - giamuc1, 0, MidpointRounding.AwayFromZero);
            var giathuem2 = Math.Round(giacothuem2 - giamuc2, 0, MidpointRounding.AwayFromZero);
            var giathuem3 = Math.Round(giacothuem3 - giamuc3, 0, MidpointRounding.AwayFromZero);
            var giathuem4 = Math.Round(giacothuem4 - giamuc4, 0, MidpointRounding.AwayFromZero);

            var tonggiathue = giathuem1 + giathuem2 + giathuem3 + giathuem4;

           
            // thue moi truong tung muc
            var giathuemtm1 = Math.Round(giamuc1 * thuemt, 0, MidpointRounding.AwayFromZero);
            var giathuemtm2 = Math.Round(giamuc2 * thuemt, 0, MidpointRounding.AwayFromZero);
            var giathuemtm3 = Math.Round(giamuc3 * thuemt, 0, MidpointRounding.AwayFromZero);
            var giathuemtm4 = Math.Round(giamuc4 * thuemt, 0, MidpointRounding.AwayFromZero);

            var tonggiathuemt = giathuemtm1 + giathuemtm2 + giathuemtm3 + giathuemtm4;
         

            var tongtientra = tonggia + tonggiathue + tonggiathuemt;

            List<decimal> ttlist = new List<decimal>();
            ttlist.Add(tonggia);
            ttlist.Add(tonggiathue);
            ttlist.Add(tonggiathuemt);
            ttlist.Add(tongtientra);

            var query = ttlist;
            return query            
                .AsEnumerable().Select(x => new TINHTIEN
                {
                    TienNuoc = tonggia,         
                    TienThue = tonggiathue,
                    TienThueMoiTruong = tonggiathuemt,
                    TongTienNuoc = tongtientra
                }).ToList();
        }

        public List<TINHTIEN> TongTienNuocGiaMuc01(Int64 kltt, string mdsd, decimal thuegtgt, decimal thuemt, int heso, int sodinhmuc, int m3m1)
        {

            var dm = heso * sodinhmuc * m3m1;

            // vượt 3 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                            kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ?
                                dm : (kltt - (2 * dm)) < 0 ? 0 : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ?
                                dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            // gia tung muc co thue * m3
            var giacothuem1 = m3muc1 > 0 ? GetGiaCoThue(mdsd, 1) * m3muc1 : 0;
            var giacothuem2 = m3muc2 > 0 ? GetGiaCoThue(mdsd, 1) * m3muc2 : 0;
            var giacothuem3 = m3muc3 > 0 ? GetGiaCoThue(mdsd, 1) * m3muc3 : 0;
            var giacothuem4 = m3muc4 > 0 ? GetGiaCoThue(mdsd, 1) * m3muc4 : 0;

            // gia tung muc chua thue
            var giamuc1 = giacothuem1 == 0 ? 0 : Math.Round(giacothuem1 / thuegtgt, 0, MidpointRounding.AwayFromZero);
            var giamuc2 = giacothuem2 == 0 ? 0 : Math.Round(giacothuem2 / thuegtgt, 0, MidpointRounding.AwayFromZero);
            var giamuc3 = giacothuem3 == 0 ? 0 : Math.Round(giacothuem3 / thuegtgt, 0, MidpointRounding.AwayFromZero);
            var giamuc4 = giacothuem4 == 0 ? 0 : Math.Round(giacothuem4 / thuegtgt, 0, MidpointRounding.AwayFromZero);

            var tonggia = giamuc1 + giamuc2 + giamuc3 + giamuc4;


            // thue GTGT tung muc
            var giathuem1 = Math.Round(giacothuem1 - giamuc1, 0, MidpointRounding.AwayFromZero);
            var giathuem2 = Math.Round(giacothuem2 - giamuc2, 0, MidpointRounding.AwayFromZero);
            var giathuem3 = Math.Round(giacothuem3 - giamuc3, 0, MidpointRounding.AwayFromZero);
            var giathuem4 = Math.Round(giacothuem4 - giamuc4, 0, MidpointRounding.AwayFromZero);

            var tonggiathue = giathuem1 + giathuem2 + giathuem3 + giathuem4;


            // thue moi truong tung muc
            var giathuemtm1 = Math.Round(giamuc1 * thuemt, 0, MidpointRounding.AwayFromZero);
            var giathuemtm2 = Math.Round(giamuc2 * thuemt, 0, MidpointRounding.AwayFromZero);
            var giathuemtm3 = Math.Round(giamuc3 * thuemt, 0, MidpointRounding.AwayFromZero);
            var giathuemtm4 = Math.Round(giamuc4 * thuemt, 0, MidpointRounding.AwayFromZero);

            var tonggiathuemt = giathuemtm1 + giathuemtm2 + giathuemtm3 + giathuemtm4;


            var tongtientra = tonggia + tonggiathue + tonggiathuemt;

            List<decimal> ttlist = new List<decimal>();
            ttlist.Add(tonggia);
            ttlist.Add(tonggiathue);
            ttlist.Add(tonggiathuemt);
            ttlist.Add(tongtientra);

            var query = ttlist;
            return query
                .AsEnumerable().Select(x => new TINHTIEN
                {
                    TienNuoc = tonggia,
                    TienThue = tonggiathue,
                    TienThueMoiTruong = tonggiathuemt,
                    TongTienNuoc = tongtientra
                }).ToList();
        }

        private void ChangeTongTienNuocDC(TIEUTHUDC cs,  decimal tiennuoc, decimal tienthue, decimal tienthuemt, decimal tongtien, 
                int chisodau, int chisocuoi, int mtruythu, int kltt )
        {
            cs.NGAYDCCS = DateTime.Now;

            cs.CHISODAUDC = chisodau;
            cs.CHISOCUOIDC = chisocuoi;
            cs.MTRUYTHUDC = mtruythu;
            cs.KLTIEUTHUDC = kltt;

            cs.TIENNUOCDC = tiennuoc;
            cs.TIENTHUEDC = tienthue;
            cs.TIENPHIDC = tienthuemt;
            cs.TONGTIENDC = tongtien;
        }

        private void ChangeTongTienNuoc(TIEUTHU cs, decimal tiennuoc, decimal tienthue, decimal tienthuemt, decimal tongtien,
                int chisodau, int chisocuoi, int mtruythu, int kltt)
        {
            cs.NGAYDCCS = DateTime.Now;

            cs.CHISODAU = chisodau;
            cs.CHISOCUOI = chisocuoi;
            cs.MTRUYTHU = mtruythu;
            cs.KLTIEUTHU = kltt;

            cs.TIENNUOC = tiennuoc;
            cs.TIENTHUE = tienthue;
            cs.TIENPHI = tienthuemt;
            cs.TONGTIEN = tongtien;
        }

        private void ChangeTieuThu(TIEUTHU cs)
        {
            // reset
            cs.M3MUC1 = 0; cs.GIAMUC1 = 0; cs.TNUOCMUC1 = 0; cs.THUEMUC1 = 0; cs.PHIMUC1 = 0;
            cs.M3MUC2 = 0; cs.GIAMUC2 = 0; cs.TNUOCMUC2 = 0; cs.THUEMUC2 = 0; cs.PHIMUC2 = 0;
            cs.M3MUC3 = 0; cs.GIAMUC3 = 0; cs.TNUOCMUC3 = 0; cs.THUEMUC3 = 0; cs.PHIMUC3 = 0;
            cs.M3MUC4 = 0; cs.GIAMUC4 = 0; cs.TNUOCMUC4 = 0; cs.THUEMUC4 = 0; cs.PHIMUC4 = 0;
            
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            var heso = 1;// (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;           

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            var kltt = cs.KLTIEUTHU.HasValue ? cs.KLTIEUTHU.Value : 0;

            //var dm = heso * soho * 10;
            var dm = heso * sodinhmuc * 10;

            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 3 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                            kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;                           

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUE = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHI = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2, 
                                    MidpointRounding.AwayFromZero);

            cs.TONGTIEN = cs.TIENNUOC + cs.TIENTHUE + cs.TIENPHI;
            
        }

        private void ChangeTieuThuLX(TIEUTHU cs)
        {
            // reset
            cs.M3MUC1 = 0; cs.GIAMUC1 = 0; cs.TNUOCMUC1 = 0; cs.THUEMUC1 = 0; cs.PHIMUC1 = 0;
            cs.M3MUC2 = 0; cs.GIAMUC2 = 0; cs.TNUOCMUC2 = 0; cs.THUEMUC2 = 0; cs.PHIMUC2 = 0;
            cs.M3MUC3 = 0; cs.GIAMUC3 = 0; cs.TNUOCMUC3 = 0; cs.THUEMUC3 = 0; cs.PHIMUC3 = 0;
            cs.M3MUC4 = 0; cs.GIAMUC4 = 0; cs.TNUOCMUC4 = 0; cs.THUEMUC4 = 0; cs.PHIMUC4 = 0;

            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            var kltt = cs.KLTIEUTHU.HasValue ? cs.KLTIEUTHU.Value : 0;

            //var dm = heso * soho * 10;
            var dm = heso * sodinhmuc * 10;

            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 3 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                            kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                (kltt > (3 * dm) ? dm : kltt - (2 * dm) < 0 ? 0 : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            //var phitn = !mienphi ? GetPhi(mdsd) : 0;
            var phimt1 = m3muc1 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt2 = m3muc2 > 0 ? GetPhiMTLX(mdsd, 2) : 0;
            var phimt3 = m3muc3 > 0 ? GetPhiMTLX(mdsd, 3) : 0;
            var phimt4 = m3muc4 > 0 ? GetPhiMTLX(mdsd, 4) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.PHIMUC1 = m3muc1 * giamuc1 * phitn;
            cs.PHIMUC1 = m3muc1 * phimt1;           

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.PHIMUC2 = m3muc2 * giamuc2 * phitn;
            cs.PHIMUC2 = m3muc2 * phimt2;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.PHIMUC3 = m3muc3 * giamuc3 * phitn;
            cs.PHIMUC3 = m3muc3 * phimt3;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.PHIMUC4 = m3muc4 * giamuc4 * phitn;
            cs.PHIMUC4 = m3muc4 * phimt4;

            cs.TIENTHUE = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHI = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0, 
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                    MidpointRounding.AwayFromZero);

            cs.TONGTIEN = cs.TIENNUOC + cs.TIENTHUE + cs.TIENPHI;            
        }

        private void ChangeTieuThu1GIALX(TIEUTHU cs)
        {
            // reset
            cs.M3MUC1 = 0; cs.GIAMUC1 = 0; cs.TNUOCMUC1 = 0; cs.THUEMUC1 = 0; cs.PHIMUC1 = 0;
            cs.M3MUC2 = 0; cs.GIAMUC2 = 0; cs.TNUOCMUC2 = 0; cs.THUEMUC2 = 0; cs.PHIMUC2 = 0;
            cs.M3MUC3 = 0; cs.GIAMUC3 = 0; cs.TNUOCMUC3 = 0; cs.THUEMUC3 = 0; cs.PHIMUC3 = 0;

            cs.M3MUC4 = 0; cs.GIAMUC4 = 0; cs.TNUOCMUC4 = 0; cs.THUEMUC4 = 0; cs.PHIMUC4 = 0;


            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;


            var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var kltt = cs.KLTIEUTHU.HasValue ? cs.KLTIEUTHU.Value : 0;

            var dm = heso * soho * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // chi tinh 1 gia cho muc 1
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            // tinh thue 1 gia cho muc 1
            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;

            var phimt1 = m3muc1 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt2 = m3muc2 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt3 = m3muc3 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt4 = m3muc4 > 0 ? GetPhiMTLX(mdsd, 1) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phimt1;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phimt2;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phimt3;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phimt4;

            cs.TIENTHUE = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHI = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                    MidpointRounding.AwayFromZero);

            cs.TONGTIEN = cs.TIENNUOC + cs.TIENTHUE + cs.TIENPHI;

        }

        private void ChangeTieuThu1GIA(TIEUTHU cs)
        {
            // reset
            cs.M3MUC1 = 0; cs.GIAMUC1 = 0; cs.TNUOCMUC1 = 0; cs.THUEMUC1 = 0; cs.PHIMUC1 = 0;
            cs.M3MUC2 = 0; cs.GIAMUC2 = 0; cs.TNUOCMUC2 = 0; cs.THUEMUC2 = 0; cs.PHIMUC2 = 0;
            cs.M3MUC3 = 0; cs.GIAMUC3 = 0; cs.TNUOCMUC3 = 0; cs.THUEMUC3 = 0; cs.PHIMUC3 = 0;

            cs.M3MUC4 = 0; cs.GIAMUC4 = 0; cs.TNUOCMUC4 = 0; cs.THUEMUC4 = 0; cs.PHIMUC4 = 0;


            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;


            var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var kltt = cs.KLTIEUTHU.HasValue ? cs.KLTIEUTHU.Value : 0;

            var dm = heso * soho * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // chi tinh 1 gia cho muc 1
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            // tinh thue 1 gia cho muc 1
            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUE = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHI = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                    MidpointRounding.AwayFromZero);

            cs.TONGTIEN = cs.TIENNUOC + cs.TIENTHUE + cs.TIENPHI;

        }

        private void ChangeTieuThuTam(TIEUTHUTEMP cs)
        {
            // reset
            cs.M3MUC1 = 0; cs.GIAMUC1 = 0; cs.TNUOCMUC1 = 0; cs.THUEMUC1 = 0; cs.PHIMUC1 = 0;
            cs.M3MUC2 = 0; cs.GIAMUC2 = 0; cs.TNUOCMUC2 = 0; cs.THUEMUC2 = 0; cs.PHIMUC2 = 0;
            cs.M3MUC3 = 0; cs.GIAMUC3 = 0; cs.TNUOCMUC3 = 0; cs.THUEMUC3 = 0; cs.PHIMUC3 = 0;

            cs.M3MUC4 = 0; cs.GIAMUC4 = 0; cs.TNUOCMUC4 = 0; cs.THUEMUC4 = 0; cs.PHIMUC4 = 0;


            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            
            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            var kltt = cs.KLTIEUTHU.HasValue ? cs.KLTIEUTHU.Value : 0;

            var dm = heso * sodinhmuc * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 =m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUE = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHI = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2, 
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIEN = cs.TIENNUOC + cs.TIENTHUE + cs.TIENPHI;
        }

        private void ChangeTieuThuDC(TIEUTHUDC cs)
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUDC = ( cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;
            var kltt = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;

            var dm = heso * sodinhmuc * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc3 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.THUEMUC1 = ((m3muc1 * giamuc1) * 5) / 100;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2; 
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.THUEMUC2 = ((m3muc2 * giamuc2) * 5) / 100;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.THUEMUC3 = ((m3muc3 * giamuc3) * 5) / 100;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.THUEMUC4 = ((m3muc4 * giamuc4) * 5) / 100;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEDC = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIDC = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCDC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2, 
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENDC = cs.TIENNUOCDC + cs.TIENTHUEDC + cs.TIENPHIDC;
            
        }

        private void ChangeTieuThuDCDMLX(TIEUTHUDC cs)
        {           
            cs.NGAYDCCS = DateTime.Now;
           
            var heso = 1;
           
            var sodinhmuc = cs.SODINHMUC != null ? (cs.SODINHMUC > 1 ? cs.SODINHMUC.Value : 1) : 1;
          
            cs.KLTIEUTHUDC = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;
            var kltt = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;        

            //var dm = heso * soho * 10;
            var dm = heso * sodinhmuc * 10;

            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 3 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                            kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            //var phitn = !mienphi ? GetPhi(mdsd) : 0;
            var phimt1 = m3muc1 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt2 = m3muc2 > 0 ? GetPhiMTLX(mdsd, 2) : 0;
            var phimt3 = m3muc3 > 0 ? GetPhiMTLX(mdsd, 3) : 0;
            var phimt4 = m3muc4 > 0 ? GetPhiMTLX(mdsd, 4) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.PHIMUC1 = m3muc1 * giamuc1 * phitn;
            cs.PHIMUC1 = m3muc1 * phimt1;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.PHIMUC2 = m3muc2 * giamuc2 * phitn;
            cs.PHIMUC2 = m3muc2 * phimt2;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.PHIMUC3 = m3muc3 * giamuc3 * phitn;
            cs.PHIMUC3 = m3muc3 * phimt3;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.PHIMUC4 = m3muc4 * giamuc4 * phitn;
            cs.PHIMUC4 = m3muc4 * phimt4;

            cs.TIENTHUEDC = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIDC = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCDC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENDC = cs.TIENNUOCDC + cs.TIENTHUEDC + cs.TIENPHIDC;

        }

        private void ChangeTieuThuDCDM(TIEUTHUDC cs)
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = cs.SODINHMUC != null ? (cs.SODINHMUC > 1 ? cs.SODINHMUC.Value : 1) : 1;

            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUDC = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;
            var kltt = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;

            var dm = heso * sodinhmuc * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc3 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.THUEMUC1 = ((m3muc1 * giamuc1) * 5) / 100;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.THUEMUC2 = ((m3muc2 * giamuc2) * 5) / 100;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.THUEMUC3 = ((m3muc3 * giamuc3) * 5) / 100;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.THUEMUC4 = ((m3muc4 * giamuc4) * 5) / 100;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEDC = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIDC = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCDC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENDC = cs.TIENNUOCDC + cs.TIENTHUEDC + cs.TIENPHIDC;

        }

        private void ChangeTieuThuTTVP1GIA(TIEUTHUTTVP cs)
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUVP = (cs.CHISOCUOIVP + cs.MTRUYTHUVP) - cs.CHISODAUVP;
            var kltt = (cs.CHISOCUOIVP + cs.MTRUYTHUVP) - cs.CHISODAUVP;

            var dm = heso * sodinhmuc * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc4 = m3muc3 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.THUEMUC1 = ((m3muc1 * giamuc1) * 5) / 100;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.THUEMUC2 = ((m3muc2 * giamuc2) * 5) / 100;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.THUEMUC3 = ((m3muc3 * giamuc3) * 5) / 100;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.THUEMUC4 = ((m3muc4 * giamuc4) * 5) / 100;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEVP = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIVP = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCVP = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENVP = cs.TIENNUOCVP + cs.TIENTHUEVP + cs.TIENPHIVP;

        }

        private void ChangeTieuThuTTVP(TIEUTHUTTVP cs)
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUVP = (cs.CHISOCUOIVP + cs.MTRUYTHUVP) - cs.CHISODAUVP;
            var kltt = (cs.CHISOCUOIVP + cs.MTRUYTHUVP) - cs.CHISODAUVP;

            var dm = heso * sodinhmuc * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc3 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.THUEMUC1 = ((m3muc1 * giamuc1) * 5) / 100;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.THUEMUC2 = ((m3muc2 * giamuc2) * 5) / 100;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.THUEMUC3 = ((m3muc3 * giamuc3) * 5) / 100;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.THUEMUC4 = ((m3muc4 * giamuc4) * 5) / 100;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEVP = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIVP = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCVP = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENVP = cs.TIENNUOCVP + cs.TIENTHUEVP + cs.TIENPHIVP;

        }

        private void ChangeTieuThuTTVPSD(TIEUTHUTTVP cs)//lay gia cuoi mdsd
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            //var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            var sodinhmuc = (cs.DMNK > 1) ? cs.DMNK.Value : 1;

            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUVP = (cs.CHISOCUOIVP + cs.MTRUYTHUVP) - cs.CHISODAUVP;
            var kltt = (cs.CHISOCUOIVP + cs.MTRUYTHUVP) - cs.CHISODAUVP;

            var dm = heso * sodinhmuc * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            var giamuc4 = m3muc3 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 4)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            //cs.THUEMUC1 = ((m3muc1 * giamuc1) * 5) / 100;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            //cs.THUEMUC2 = ((m3muc2 * giamuc2) * 5) / 100;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            //cs.THUEMUC3 = ((m3muc3 * giamuc3) * 5) / 100;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            //cs.THUEMUC4 = ((m3muc4 * giamuc4) * 5) / 100;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEVP = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIVP = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCVP = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENVP = cs.TIENNUOCVP + cs.TIENTHUEVP + cs.TIENPHIVP;

        }

        private void ChangeTieuThuDC1GIALX(TIEUTHUDC cs)
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUDC = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;
            var kltt = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;

            var dm = heso * soho * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // Chi dung 1 gia muc 1
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            //lay gia thue 1 muc 1
            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;

            var phimt1 = m3muc1 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt2 = m3muc2 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt3 = m3muc3 > 0 ? GetPhiMTLX(mdsd, 1) : 0;
            var phimt4 = m3muc4 > 0 ? GetPhiMTLX(mdsd, 1) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phimt1;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phimt2;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phimt3;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phimt4;

            cs.TIENTHUEDC = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIDC = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCDC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENDC = cs.TIENNUOCDC + cs.TIENTHUEDC + cs.TIENPHIDC;

        }

        private void ChangeTieuThuDC1GIA(TIEUTHUDC cs)
        {
            //cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUDC = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;
            var kltt = (cs.CHISOCUOIDC + cs.MTRUYTHUDC) - cs.CHISODAUDC;

            var dm = heso * soho * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 4
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // Chi dung 1 gia muc 1
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            //lay gia thue 1 muc 1
            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEDC = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIDC = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCDC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENDC = cs.TIENNUOCDC + cs.TIENTHUEDC + cs.TIENPHIDC;

        }

        private void ChangeTieuThuDCTG(TIEUTHUDCTG cs)
        {
            cs.MTRUYTHUDC = 0;
            cs.NGAYDCCS = DateTime.Now;
            //var heso = (cs.GHI2THANG1LAN.Equals(1)&&cs.ING2T1L.Equals(1))  ?  2 : 1 ; 
            //var heso = (cs.GHI2THANG1LAN.Equals("1") && cs.TTHAIGHI.Equals("1")) ? 2 : 1;
            var heso = 1;

            var soho = !cs.SOHO.HasValue ? 0 : cs.SOHO.Value;
            //var kltt = cs.KLTIEUTHUDC.HasValue ? cs.KLTIEUTHUDC.Value : 0;
            cs.KLTIEUTHUDC = cs.CHISOCUOIDC - cs.CHISODAUDC;
            var kltt = cs.CHISOCUOIDC - cs.CHISODAUDC;

            var dm = heso * soho * 10;
            var mdsd = cs.MAMDSD;
            var mienphi = cs.KOPHINT.HasValue && cs.KOPHINT.Value;
            var mienthue = cs.KOVAT.HasValue && cs.KOVAT.Value;

            // vượt 2 mức đm, đưa vô mức 3
            var m3muc4 = kltt > (3 * dm) ?
                        kltt - (3 * dm) : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc3 = kltt > dm ?
                            (kltt > (3 * dm) ? dm : kltt - (2 * dm))
                            : 0;

            // vượt 1 mức đm, nếu vượt 2 mức đm thì dùng đm, ngược lại dùng kltt - dm
            var m3muc2 = kltt > dm ?
                            (kltt > (2 * dm) ? dm : kltt - dm)
                            : 0;

            // vượt đm, dùng đm ngược lại dùng kltt
            var m3muc1 = kltt > dm ? dm : kltt;

            var giamuc1 = m3muc1 > 0
                              ? GetGia(mdsd, 1)
                              : 0;
            var giamuc2 = m3muc2 > 0
                              ? GetGia(mdsd, 2)
                              : 0;
            var giamuc3 = m3muc3 > 0
                              ? GetGia(mdsd, 3)
                              : 0;
            var giamuc4 = m3muc4 > 0
                              ? GetGia(mdsd, 4)
                              : 0;
            if (mienthue)
            {
                giamuc1 = Math.Round(giamuc1, 0, MidpointRounding.AwayFromZero);
                giamuc2 = Math.Round(giamuc2, 0, MidpointRounding.AwayFromZero);
                giamuc3 = Math.Round(giamuc3, 0, MidpointRounding.AwayFromZero);
                giamuc4 = Math.Round(giamuc4, 0, MidpointRounding.AwayFromZero);
            }

            var thuemuc1 = m3muc1 > 0 && !mienthue
                              ? GetThue(mdsd, 1)
                              : 0;
            var thuemuc2 = m3muc2 > 0 && !mienthue
                              ? GetThue(mdsd, 2)
                              : 0;
            var thuemuc3 = m3muc3 > 0 && !mienthue
                              ? GetThue(mdsd, 3)
                              : 0;
            var thuemuc4 = m3muc4 > 0 && !mienthue
                             ? GetThue(mdsd, 4)
                             : 0;
            var phitn = !mienphi ? GetPhi(mdsd) : 0;

            cs.M3MUC1 = m3muc1;
            cs.GIAMUC1 = giamuc1;
            cs.TNUOCMUC1 = m3muc1 * giamuc1;
            cs.THUEMUC1 = m3muc1 * thuemuc1;
            cs.PHIMUC1 = m3muc1 * phitn;

            cs.M3MUC2 = m3muc2;
            cs.GIAMUC2 = giamuc2;
            cs.TNUOCMUC2 = m3muc2 * giamuc2;
            cs.THUEMUC2 = m3muc2 * thuemuc2;
            cs.PHIMUC2 = m3muc2 * phitn;

            cs.M3MUC3 = m3muc3;
            cs.GIAMUC3 = giamuc3;
            cs.TNUOCMUC3 = m3muc3 * giamuc3;
            cs.THUEMUC3 = m3muc3 * thuemuc3;
            cs.PHIMUC3 = m3muc3 * phitn;

            cs.M3MUC4 = m3muc4;
            cs.GIAMUC4 = giamuc4;
            cs.TNUOCMUC4 = m3muc4 * giamuc4;
            cs.THUEMUC4 = m3muc4 * thuemuc4;
            cs.PHIMUC4 = m3muc4 * phitn;

            cs.TIENTHUEDC = Math.Round((decimal)(cs.THUEMUC1 + cs.THUEMUC2 + cs.THUEMUC3 + cs.THUEMUC4), 2,
                                       MidpointRounding.AwayFromZero);

            cs.TIENPHIDC = Math.Round((decimal)(cs.PHIMUC1 + cs.PHIMUC2 + cs.PHIMUC3 + cs.PHIMUC4), 0,
                                       MidpointRounding.AwayFromZero);

            cs.TIENNUOCDC = Math.Round((decimal)(cs.TNUOCMUC1 + cs.TNUOCMUC2 + cs.TNUOCMUC3 + cs.TNUOCMUC4), 2,
                                        MidpointRounding.AwayFromZero);

            cs.TONGTIENDC = cs.TIENNUOCDC + cs.TIENTHUEDC + cs.TIENPHIDC;

        }

        public decimal GetThue(string md, int level)
        {
            var mdsd = _db.MDSDs.SingleOrDefault(m => m.MAMDSD.Equals(md));
            if (mdsd == null) return 0;

            if (level == 1)
                return mdsd.THUEMUC1.HasValue ? mdsd.THUEMUC1.Value : 0;
            if (level == 2)
                return mdsd.THUEMUC2.HasValue ? mdsd.THUEMUC2.Value : 0;
            if (level == 3)
                return mdsd.THUEMUC3.HasValue ? mdsd.THUEMUC3.Value : 0;
            if (level == 4)
                return mdsd.THUEMUC4.HasValue ? mdsd.THUEMUC4.Value : 0;

            return 0;
        }

        public decimal GetPhi(string md)
        {
            var mdsd = _db.MDSDs.SingleOrDefault(m => m.MAMDSD.Equals(md));
            if (mdsd == null) return 0;

            return mdsd.PHINT.HasValue ? mdsd.PHINT.Value : 0;
        }

        public decimal GetPhiMTLX(string md, int level)
        {
            var mdsd = _db.MDSDs.SingleOrDefault(m => m.MAMDSD.Equals(md));
            if (mdsd == null) return 0;

            if (level == 1)
                return mdsd.PHIMTM1.HasValue ? mdsd.PHIMTM1.Value : 0;
            if (level == 2)
                return mdsd.PHIMTM2.HasValue ? mdsd.PHIMTM2.Value : 0;
            if (level == 3)
                return mdsd.PHIMTM3.HasValue ? mdsd.PHIMTM3.Value : 0;
            if (level == 4)
                return mdsd.PHIMTM4.HasValue ? mdsd.PHIMTM4.Value : 0;

            return 0;        
        }

        public decimal GetGia(string md, int level)
        {
            var mdsd = _db.MDSDs.SingleOrDefault(m => m.MAMDSD.Equals(md));
            if (mdsd == null) return 0;

            if (level == 1)
                return mdsd.GIAMUC1.HasValue ? mdsd.GIAMUC1.Value : 0;
            if (level == 2)
                return mdsd.GIAMUC2.HasValue ? mdsd.GIAMUC2.Value : 0;
            if (level == 3)
                return mdsd.GIAMUC3.HasValue ? mdsd.GIAMUC3.Value : 0;

            if (level == 4)
                return mdsd.GIAMUC4.HasValue ? mdsd.GIAMUC4.Value : 0;

            return 0;
        }

        public decimal GetGiaCoThue(string md, int level)
        {
            var mdsd = _db.MDSDs.SingleOrDefault(m => m.MAMDSD.Equals(md));
            if (mdsd == null) return 0;

            if (level == 1)
                return mdsd.GIACOTHUEMUC1.HasValue ? mdsd.GIACOTHUEMUC1.Value : 0;
            if (level == 2)
                return mdsd.GIACOTHUEMUC2.HasValue ? mdsd.GIACOTHUEMUC2.Value : 0;
            if (level == 3)
                return mdsd.GIACOTHUEMUC3.HasValue ? mdsd.GIACOTHUEMUC3.Value : 0;
            if (level == 4)
                return mdsd.GIACOTHUEMUC4.HasValue ? mdsd.GIACOTHUEMUC4.Value : 0;

            return 0;
        }

        private string GetTHBT(decimal? ttkn, decimal? kltieuthuKytruoc)
        {
            var ttbt = "BT";

            if (ttkn == 0 && kltieuthuKytruoc > 5)
                ttbt = "TNKSD";

            if (ttkn > 0 && ttkn < 6 &&
                ttkn < kltieuthuKytruoc / 5)
                ttbt = "TNNH5LTT";

            if (ttkn > 5 && ttkn < 11 &&
                ttkn < kltieuthuKytruoc / 4)
                ttbt = "TNNH4LTT";

            if (ttkn > 10 && ttkn < 21 &&
                ttkn < kltieuthuKytruoc / 3)
                ttbt = "TNNH3LTT";

            if (ttkn > 20 && ttkn < 101 &&
                ttkn < kltieuthuKytruoc / 2)
                ttbt = "TNNH2LTT";

            if (ttkn > 100 && ttkn < 501 &&
                ttkn < kltieuthuKytruoc / (decimal)1.5)
                ttbt = "TNNH1.5LTT";

            if (ttkn > 500 && ttkn < 2001 &&
                ttkn < kltieuthuKytruoc / (decimal)1.3)
                ttbt = "TNNH1.3LTT";

            if (ttkn > 2000 && ttkn < 5001 &&
                ttkn < kltieuthuKytruoc / (decimal)1.2)
                ttbt = "TNNH1.2LTT";

            if (ttkn > 2000 && ttkn < 5001 &&
                ttkn > kltieuthuKytruoc * (decimal)1.2)
                ttbt = "TNLH1.2LTT";

            if (ttkn > 500 && ttkn < 2001 &&
                ttkn > kltieuthuKytruoc * (decimal)1.3)
                ttbt = "TNLH1.3LTT";

            if (ttkn > 100 && ttkn < 501 &&
                ttkn > kltieuthuKytruoc * (decimal)1.5)
                ttbt = "TNLH1.5LTT";

            if (ttkn > 20 && ttkn < 101 &&
                ttkn > kltieuthuKytruoc * 2)
                ttbt = "TNLH2LTT";

            if (ttkn > 5000)
                ttbt = "TNBT";

            return ttbt;
        }

        public bool IsDaKhoiTao(DateTime date, string idkh)
        {
            var count = _db.TIEUTHUs.Count(tt => (tt.THANG.Equals(date.Month) && tt.NAM.Equals(date.Year)
                                                  && tt.IDKH.Equals(idkh)));

            return (count > 0);
        }

        public List<GHICHISO> GetListForUpdatePHIENLXTT(DateTime kyghi, string sodb, string madp, string madotin, string phien,
                    string trangthai, string makv)
        {
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH, kh => kh.IDKH, (tt, kh) => new { tt, kh })
                .Join(_db.DUONGPHOs, khdp => khdp.kh.MADP, dp => dp.MADP, (khdp, dp) => new { khdp, dp })
                .Where(
                        @res => @res.khdp.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.khdp.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                            //(sodb == @res.kh.MADP + @res.kh.DUONGPHU + @res.kh.MADB) &&
                        @res.dp.DOT.Equals(phien) &&
                        @res.khdp.tt.TTHAIGHI == trangthai &&
                         @res.khdp.kh.MAKV == makv &&
                        @res.khdp.kh.XOABOKH == false
                    );

            query = query.OrderBy(q => q.khdp.kh.MADP + q.khdp.kh.MADB);

            return query.Select(@res => new
            {
                @res.khdp.kh.IDKH,
                SODB = @res.khdp.tt.MADP + @res.khdp.tt.MADB,
                @res.khdp.tt.MADP,
                @res.khdp.tt.NAM,
                @res.khdp.tt.THANG,
                @res.khdp.kh.SONHA2,
                @res.khdp.kh.TENKH,
                @res.khdp.tt.CHISODAU,
                @res.khdp.tt.CHISOCUOI,
                @res.khdp.tt.MTRUYTHU,
                @res.khdp.tt.KLTIEUTHU,
                TTHAIGHI = @res.khdp.tt.TTHAIGHI ?? "GDH_BT",
                @res.khdp.tt.MANVN_CS//,
                //DOTINHD = @res.dp.IDMADOTIN == "NNIN0005" ? "Đ 1" : (@res.dp.IDMADOTIN == "NNIN0006" ? "Đ 2" : "Đ 3"),
                // @res.dp.DOT,
            }).AsEnumerable().Select(x => new GHICHISO
            {
                IDKH = x.IDKH,
                SODB = x.SODB,
                MADP = x.MADP,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA2,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                MTRUYTHU = x.MTRUYTHU,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS//,                DOTINHD = x.DOTINHD,                PHIENLX = x.DOT
            }).ToList();
        }

        public List<GHICHISO> GetListForUpdateDOTINTT(DateTime kyghi, string sodb, string madp, string madotin, string phien,
                    string trangthai, string makv )
        {
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH, kh => kh.IDKH, (tt, kh) => new { tt, kh })            
                .Join(_db.DUONGPHOs, khdp => khdp.kh.MADP, dp => dp.MADP, (khdp, dp) => new {khdp, dp}  )
                .Where(
                        @res => @res.khdp.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.khdp.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                            //(sodb == @res.kh.MADP + @res.kh.DUONGPHU + @res.kh.MADB) &&
                        @res.dp.IDMADOTIN.Equals(madotin) &&
                        @res.khdp.tt.TTHAIGHI == trangthai &&
                        @res.khdp.kh.MAKV == makv &&
                        @res.khdp.kh.XOABOKH == false
                    );

            query = query.OrderBy(q => q.khdp.kh.MADP + q.khdp.kh.MADB);

            return query.Select(@res => new
            {
                @res.khdp.kh.IDKH,
                SODB = @res.khdp.tt.MADP + @res.khdp.tt.MADB,
                @res.khdp.tt.MADP,
                @res.khdp.tt.NAM,
                @res.khdp.tt.THANG,
                @res.khdp.kh.SONHA2,
                @res.khdp.kh.TENKH,
                @res.khdp.tt.CHISODAU,
                @res.khdp.tt.CHISOCUOI,
                @res.khdp.tt.MTRUYTHU,
                @res.khdp.tt.KLTIEUTHU,
                TTHAIGHI = @res.khdp.tt.TTHAIGHI ?? "GDH_BT",
                @res.khdp.tt.MANVN_CS//,
                //DOTINHD = @res.dp.IDMADOTIN == "NNIN0005" ? "Đ 1" : (@res.dp.IDMADOTIN == "NNIN0006" ? "Đ 2" : "Đ 3"),
               // @res.dp.DOT,
            }).AsEnumerable().Select(x => new GHICHISO
            {
                IDKH = x.IDKH,
                SODB = x.SODB,
                MADP = x.MADP,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA2,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                MTRUYTHU = x.MTRUYTHU,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS//,                DOTINHD = x.DOTINHD,                PHIENLX = x.DOT
            }).ToList();
        }

        public List<GHICHISO> GetListForUpdateMoi(DateTime kyghi, string sodb, string madp, string trangthai)
        {
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH, kh => kh.IDKH,
                      (tt, kh) => new { tt, kh })               
                .Where(
                        @res => @res.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                        //(sodb == @res.kh.MADP + @res.kh.DUONGPHU + @res.kh.MADB) &&
                        @res.tt.TTHAIGHI == trangthai &&
                        @res.kh.XOABOKH == false
                    );

            query = query.OrderBy(kh => kh.kh.MADP + kh.kh.MADB);

            return query.Select(@res => new
            {
                @res.kh.IDKH,
                SODB = @res.tt.MADP+@res.tt.MADB ,
                @res.tt.MADP,
                @res.tt.NAM,
                @res.tt.THANG,
                @res.kh.SONHA2,
                @res.kh.TENKH,
                @res.tt.CHISODAU,
                @res.tt.CHISOCUOI,
                @res.tt.MTRUYTHU,
                @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT",
                @res.tt.MANVN_CS
            }).AsEnumerable().Select(x => new GHICHISO
            {
                IDKH = x.IDKH,
                SODB = x.SODB,
                MADP = x.MADP,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA2,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                MTRUYTHU = x.MTRUYTHU,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS
            }).ToList();
        }

        public List<GHICHISO> GetListForUpdate(DateTime kyghi, string sodb, string madp)
        {
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH, kh => kh.IDKH,
                      (tt, kh) => new { tt, kh })
                .Where(
                        @res => @res.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                        (sodb == @res.kh.MADP + @res.kh.DUONGPHU + @res.kh.MADB) 
                    );
            return query.Select(@res => new
            {
                @res.kh.IDKH, sodb, madp, @res.tt.NAM, @res.tt.THANG,
                @res.kh.SONHA, @res.kh.TENKH,
                @res.tt.CHISODAU, @res.tt.CHISOCUOI,
                @res.tt.MTRUYTHU, @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT", @res.tt.MANVN_CS
            }).AsEnumerable().Select(x => new GHICHISO
            {
                IDKH = x.IDKH, SODB = x.sodb, MADP = x.madp, NAM = x.NAM, THANG = x.THANG,
                SONHA = x.SONHA, TENKH = x.TENKH,
                CHISODAU = x.CHISODAU, CHISOCUOI = x.CHISOCUOI, 
                MTRUYTHU = x.MTRUYTHU ,KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI, MANV_CS = x.MANVN_CS
            }).ToList();
        }

        public List<GHICHISOTT> GetListForUpdateTT(DateTime kyghi, string sodb, string madp)
        {
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH, kh => kh.IDKH,
                      (tt, kh) => new { tt, kh })
                .Where(
                        @res => @res.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                        (sodb == @res.kh.MADP + @res.kh.DUONGPHU + @res.kh.MADB)
                    );
            return query.Select(@res => new
            {
                @res.kh.IDKH,
                sodb,
                madp,
                @res.tt.NAM,
                @res.tt.THANG,
                @res.kh.SONHA,
                @res.kh.TENKH,
                @res.tt.CHISODAU,
                @res.tt.CHISOCUOI,
                @res.tt.KLTIEUTHU,
                @res.tt.MTRUYTHU,
                @res.tt.TIENTRUYTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT",
                @res.tt.MANVN_CS
            }).AsEnumerable().Select(x => new GHICHISOTT
            {
                IDKH = x.IDKH,
                SODB = x.sodb,
                MADP = x.madp,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,
                MTRUYTHU = x.MTRUYTHU,
                
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS
            }).ToList();
        }

        public List<GHICHISO> GetList(DateTime kyghi, DUONGPHO dp)
        {
            /*var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.SODB.Trim(), kh => (kh.MADP + kh.DUONGPHU + kh.MADB).Trim(), 
                      (tt, kh) => new { tt, kh })*/
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH.Trim(), kh => kh.IDKH.Trim(), 
                      (tt, kh) => new { tt, kh })
                .Where( 
                        @res => @res.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                        @res.tt.SODB.StartsWith(dp.MADP + dp.DUONGPHU) // so danh bo bat dau bang madp + duong phu
                            && @res.kh.KYKHAITHAC < kyghi && @res.kh.XOABOKH.Equals(0)
                        //!@res.tt.TTSD.Equals("CUP")                 // trang thai ghi
                        //@res.tt.SODB.StartsWith(dp.MADP.Substring(0,3))
                    );

            query = query.OrderBy(tt => tt.kh.DUONGPHU).OrderBy(tt => tt.kh.MADP).OrderBy(tt => tt.kh.MADB);

            return query.Select(@res => new
            {
                @res.kh.IDKH, @res.tt.SODB, @res.tt.MADP, @res.tt.NAM, @res.tt.THANG,
                @res.kh.SONHA, @res.kh.TENKH, @res.kh.KYKHAITHAC,
                @res.tt.CHISODAU, @res.tt.CHISOCUOI, @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT", @res.tt.MANVN_CS
            }).AsEnumerable().Select(x => new GHICHISO
            {
                IDKH = x.IDKH, SODB = x.SODB, MADP = x.MADP, NAM = x.NAM, THANG = x.THANG,
                SONHA = x.SONHA, TENKH = x.TENKH,
                CHISODAU = x.CHISODAU, CHISOCUOI = x.CHISOCUOI, KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI, MANV_CS = x.MANVN_CS
            }).ToList();
        }

        //GCSTTKYTRUOC
        public List<GCSTTKYTRUOC> GetListTTKYTRUOC(DateTime kyghi, DUONGPHO dp)
        {
            /*var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.SODB.Trim(), kh => (kh.MADP + kh.DUONGPHU + kh.MADB).Trim(), 
                      (tt, kh) => new { tt, kh })*/
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH.Trim(), kh => kh.IDKH.Trim(),
                      (tt, kh) => new { tt, kh })
                .Where(
                        @res => @res.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                        @res.tt.SODB.StartsWith(dp.MADP + dp.DUONGPHU) // so danh bo bat dau bang madp + duong phu
                            && @res.kh.KYKHAITHAC < kyghi && @res.kh.XOABOKH.Equals(0)
                //!@res.tt.TTSD.Equals("CUP")                 // trang thai ghi
                //@res.tt.SODB.StartsWith(dp.MADP.Substring(0,3))
                    );

            query = query.OrderBy(tt => tt.kh.DUONGPHU).OrderBy(tt => tt.kh.MADP).OrderBy(tt => tt.kh.MADB);

            return query.Select(@res => new
            {
                @res.kh.IDKH,
                @res.tt.SODB,
                @res.tt.MADP,
                @res.tt.NAM,
                @res.tt.THANG,
                @res.kh.SONHA,
                @res.kh.TENKH,
                @res.kh.KYKHAITHAC,
                @res.tt.CHISODAU,
                @res.tt.CHISOCUOI,
                @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT",
                @res.tt.MANVN_CS,

                @res.tt.CHISODAU_1,
                @res.tt.CHISOCUOI_1,
                @res.tt.KLTIEUTHU_1,
            }).AsEnumerable().Select(x => new GCSTTKYTRUOC
            {
                IDKH = x.IDKH,
                SODB = x.SODB,
                MADP = x.MADP,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS,

                CHISODAU_1 = x.CHISODAU_1,
                CHISOCUOI_1 = x.CHISOCUOI_1,
                KLTIEUTHU_1 = x.KLTIEUTHU_1
            }).ToList();
        }

        public List<GHICHISODB> GetListDB(DateTime kyghi, DUONGPHO dp)
        {
            /*var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.SODB.Trim(), kh => (kh.MADP + kh.DUONGPHU + kh.MADB).Trim(), 
                      (tt, kh) => new { tt, kh })*/
            var query = _db.TIEUTHUs
                .Join(_db.KHACHHANGs, tt => tt.IDKH.Trim(), kh => kh.IDKH.Trim(),
                      (tt, kh) => new { tt, kh })
                .Where(
                        @res => @res.tt.NAM.Equals(kyghi.Year) &&   // nam trong tieu thu
                        @res.tt.THANG.Equals(kyghi.Month) &&        // thang trong tieu thu
                        @res.tt.SODB.StartsWith(dp.MADP + dp.DUONGPHU) //&&   // so danh bo bat dau bang madp + duong phu
                        && @res.kh.XOABOKH.Equals(0)
                //!@res.tt.TTSD.Equals("CUP")                 // trang thai ghi
                    );

            query = query.OrderBy(tt => tt.kh.DUONGPHU).OrderBy(tt => tt.kh.MADP).OrderBy(tt => tt.kh.STT);

            return query.Select(@res => new
            {
                @res.kh.IDKH,
                @res.tt.SODB,
                @res.tt.MADP,
                @res.tt.MADB,
                @res.tt.NAM,
                @res.tt.THANG,
                @res.kh.SONHA,
                @res.kh.TENKH
                
            }).AsEnumerable().Select(x => new GHICHISODB
            {
                IDKH = x.IDKH,
                SODB = x.SODB,
                MADP = x.MADP,
                MADB = x.MADB,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH
            }).ToList();
        }

        public Message Update(GHICHISO obj)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                if(obj == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Ghi chỉ số");

                var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == obj.IDKH &&
                                                               tt.NAM == obj.NAM &&
                                                               tt.THANG == obj.THANG);

                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Ghi chỉ số");

                var kh = objDb.KHACHHANG;
                if (kh == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Ghi chỉ số");

                if (kh.TTSD == "CUP")
                {
                    objDb.TTSD = "CUP";
                    objDb.TTHAIGHI = "CUP";
                    objDb.MALKH = kh.MALKH;
                    objDb.MAMDSD = kh.MAMDSD;
                    objDb.CHISODAU = obj.CHISODAU; objDb.CHISOCUOI = obj.CHISOCUOI; 
                    objDb.MTRUYTHU = 0; objDb.KLTIEUTHU = 0;
                    objDb.THBT = null;
                    objDb.M3MUC1 = 0; objDb.GIAMUC1 = 0; objDb.PHIMUC1 = 0; objDb.THUEMUC1 = 0;
                    objDb.M3MUC2 = 0; objDb.GIAMUC2 = 0; objDb.PHIMUC2 = 0; objDb.THUEMUC2 = 0;
                    objDb.M3MUC3 = 0; objDb.GIAMUC3 = 0; objDb.PHIMUC3 = 0; objDb.THUEMUC3 = 0;
                    objDb.M3MUC4 = 0; objDb.GIAMUC4 = 0; objDb.PHIMUC4 = 0; objDb.THUEMUC4 = 0;
                    objDb.TONGTIEN = 0; objDb.TIENNUOC = 0;
                    objDb.TIENTHUE = 0; objDb.TIENPHI = 0;

                    objDb.INHD = false; objDb.ISGCS = false; objDb.HETNO = false; objDb.THUTQ = false;
                    objDb.MANVNHAPCN = null; objDb.NGAYNHAPCN = null; objDb.NGAYCN = null;
                    objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                    _db.SubmitChanges();
                    goto commit;
                }

                //Important: Neu khach hang doi duong pho, cap nhat lai makv, madp, duongphu, sodb, madb
                objDb.MAKV = kh.MAKV;
                objDb.MAKVDN = kh.MAKVDN;
                objDb.MADP = kh.MADP;
                objDb.DUONGPHU = kh.DUONGPHU;
                objDb.MADB = kh.MADB;
                objDb.SODB = kh.MADP + kh.DUONGPHU + kh.MADB;

                objDb.CHISODAU = obj.CHISODAU;
                objDb.CHISOCUOI = obj.CHISOCUOI;
                objDb.MTRUYTHU = obj.MTRUYTHU;
                objDb.KLTIEUTHU = obj.KLTIEUTHU;

                objDb.MAMDSD = kh.MAMDSD;
                objDb.MACQ = kh.MACQ;
                objDb.SOHO = kh.SOHO;
                objDb.SONK = kh.SONK;
                objDb.ISDINHMUC = kh.ISDINHMUC;
                objDb.DMNK = kh.SODINHMUC;
                objDb.KHONGTINH117 = kh.KHONGTINH117;
                objDb.KYHOTRO = kh.KYHOTRO;

                objDb.TTSD = kh.TTSD;
                //objDb.TTSD = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;

                objDb.KOPHINT = kh.KOPHINT;
                objDb.KOVAT = kh.KOVAT;

                objDb.TTHAIGHI = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;
                objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                //objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? GetHSKH(MAHSKH.PHIMT.ToString()) : 0;
                //objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? GetHSKH(MAHSKH.VAT.ToString()) : 0;

                objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? (decimal)0.05 : 0;
                objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? (decimal)0.05 : 0;
                objDb.MAHTTT = kh.MAHTTT;

                //TODO: update tình hình bất thường
                string ttbt;

                // chỉ số cuối = 0: ghi sót
                if (obj.CHISODAU > 0 && obj.CHISOCUOI == 0)
                    ttbt = "THGS";
                else
                {
                    var date = new DateTime(obj.NAM, obj.THANG, 1).AddMonths(-1);

                    var kltieuthuKytruoc = _db.TIEUTHUs.Where(tt => tt.SODB == obj.SODB &&
                                                                     tt.NAM == date.Year &&
                                                                     tt.THANG == date.Month).Select(t => t.KLTIEUTHU)
                        .SingleOrDefault();

                    ttbt = GetTHBT(objDb.KLTIEUTHU, kltieuthuKytruoc);
                }
                objDb.THBT = ttbt;

                decimal thuegtgt = (decimal)1.05;   // thue 5%
                decimal thuemt = (decimal)1.1;  // thue 10%

                if (objDb.MAKV == "X")
                {
                    //ChangeTieuThuLX(objDb);
                    int kltt = Convert.ToInt32(objDb.KLTIEUTHU);
                    int dmnk = Convert.ToInt32(objDb.DMNK);
                    TongTienNuoc(kltt, objDb.MAMDSD, thuegtgt, thuemt, 1, dmnk, 10);   
                }
                else
                {
                    //ChangeTieuThu(objDb);
                    //ChangeTieuThuLX(objDb);
                    int kltt = Convert.ToInt32(objDb.KLTIEUTHU);
                    int dmnk = Convert.ToInt32(objDb.DMNK);
                    TongTienNuoc(kltt, objDb.MAMDSD, thuegtgt, thuemt, 1, dmnk, 10);   
                }

                objDb.HETNO = false;
                objDb.THUTQ = false;

                objDb.MANVCN = null;
                objDb.MANVNHAPCN = null;
                objDb.NGAYCN = null;
                objDb.NGAYNHAPCN = null;
                
                //objDb.INHD = objDb.TONGTIEN > 0;
                objDb.INHD = objDb.TONGTIEN > 0 ? true : false;
                objDb.NGAYGHICS = DateTime.Now.Date;

                objDb.ISGCS = true;
                _db.SubmitChanges();

                commit:
                // commit
                trans.Commit();
                _db.Connection.Close();

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message UpdateDB(GHICHISODB obj)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                if (obj == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Ghi chỉ số");

                var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == obj.IDKH &&
                                                               tt.NAM == obj.NAM &&
                                                               tt.THANG == obj.THANG);

                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Ghi chỉ số");

                var kh = objDb.KHACHHANG;
                if (kh == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Ghi chỉ số");

                if (kh.TTSD == "CUP")
                {
                    objDb.TTSD = "CUP";
                    objDb.TTHAIGHI = "CUP";
                    objDb.MALKH = kh.MALKH;
                    objDb.MAMDSD = kh.MAMDSD;
                    //objDb.CHISODAU = obj.CHISODAU; objDb.CHISOCUOI = obj.CHISOCUOI;
                    objDb.MTRUYTHU = 0; objDb.KLTIEUTHU = 0;
                    objDb.THBT = null;
                    objDb.M3MUC1 = 0; objDb.GIAMUC1 = 0; objDb.PHIMUC1 = 0; objDb.THUEMUC1 = 0;
                    objDb.M3MUC2 = 0; objDb.GIAMUC2 = 0; objDb.PHIMUC2 = 0; objDb.THUEMUC2 = 0;
                    objDb.M3MUC3 = 0; objDb.GIAMUC3 = 0; objDb.PHIMUC3 = 0; objDb.THUEMUC3 = 0;
                    objDb.M3MUC4 = 0; objDb.GIAMUC4 = 0; objDb.PHIMUC4 = 0; objDb.THUEMUC4 = 0;
                    objDb.TONGTIEN = 0; objDb.TIENNUOC = 0;
                    objDb.TIENTHUE = 0; objDb.TIENPHI = 0;

                    objDb.INHD = false; objDb.ISGCS = false; objDb.HETNO = false; objDb.THUTQ = false;
                    objDb.MANVNHAPCN = null; objDb.NGAYNHAPCN = null; objDb.NGAYCN = null;
                    //objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                    _db.SubmitChanges();
                    goto commit;
                }

                //Important: Neu khach hang doi duong pho, cap nhat lai makv, madp, duongphu, sodb, madb
                objDb.MAKV = kh.MAKV;
                objDb.MAKVDN = kh.MAKVDN;
                objDb.MADP = kh.MADP;
                objDb.DUONGPHU = kh.DUONGPHU;
                objDb.MADB = kh.MADB;
                objDb.SODB = kh.MADP + kh.DUONGPHU + kh.MADB;

                /*objDb.CHISODAU = obj.CHISODAU;
                objDb.CHISOCUOI = obj.CHISOCUOI;
                objDb.MTRUYTHU = obj.MTRUYTHU;
                objDb.KLTIEUTHU = obj.KLTIEUTHU;*/

                objDb.MAMDSD = kh.MAMDSD;
                objDb.MACQ = kh.MACQ;
                objDb.SOHO = kh.SOHO;
                objDb.SONK = kh.SONK;
                objDb.ISDINHMUC = kh.ISDINHMUC;
                objDb.DMNK = kh.SODINHMUC;
                objDb.KHONGTINH117 = kh.KHONGTINH117;
                objDb.KYHOTRO = kh.KYHOTRO;

                objDb.TTSD = kh.TTSD;
                //objDb.TTSD = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;

                objDb.KOPHINT = kh.KOPHINT;
                objDb.KOVAT = kh.KOVAT;

                //objDb.TTHAIGHI = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;
                //objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                //objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? GetHSKH(MAHSKH.PHIMT.ToString()) : 0;
                //objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? GetHSKH(MAHSKH.VAT.ToString()) : 0;

                objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? (decimal)0.05 : 0;
                objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? (decimal)0.05 : 0;
                objDb.MAHTTT = kh.MAHTTT;

                //TODO: update tình hình bất thường
                string ttbt;

                // chỉ số cuối = 0: ghi sót
                /*if (obj.CHISODAU > 0 && obj.CHISOCUOI == 0)
                    ttbt = "THGS";
                else
                {
                    var date = new DateTime(obj.NAM, obj.THANG, 1).AddMonths(-1);

                    var kltieuthuKytruoc = _db.TIEUTHUs.Where(tt => tt.SODB == obj.SODB &&
                                                                     tt.NAM == date.Year &&
                                                                     tt.THANG == date.Month).Select(t => t.KLTIEUTHU)
                        .SingleOrDefault();

                    ttbt = GetTHBT(objDb.KLTIEUTHU, kltieuthuKytruoc);
                }
                objDb.THBT = ttbt;*/

                //ChangeTieuThu(objDb);

                objDb.HETNO = false;
                objDb.THUTQ = false;

                objDb.MANVCN = null;
                objDb.MANVNHAPCN = null;
                objDb.NGAYCN = null;
                objDb.NGAYNHAPCN = null;

                //objDb.INHD = objDb.TONGTIEN > 0;
                objDb.INHD = objDb.TONGTIEN > 0 ||
                    (objDb.GHI2THANG1LAN.Equals("1") && objDb.ING2T1L.Equals("1")) ? true : false
                              ;
                objDb.NGAYGHICS = DateTime.Now.Date;

                objDb.ISGCS = true;
                _db.SubmitChanges();

            commit:
                // commit
                trans.Commit();
                _db.Connection.Close();

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Ghi chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }
            return msg;
        }

        public Message UpdateTT(GHICHISOTT obj)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                if (obj == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "TT chỉ số");

                var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == obj.IDKH &&
                                                               tt.NAM == obj.NAM &&
                                                               tt.THANG == obj.THANG);

                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "TT chỉ số");

                var kh = objDb.KHACHHANG;
                if (kh == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "TT chỉ số");

                if (kh.TTSD == "CUP")
                {
                    objDb.TTSD = "CUP";

                    objDb.MAMDSD = kh.MAMDSD;
                    objDb.MAKV = kh.MAKV;
                    objDb.MAKVDN = kh.MAKVDN;
                    objDb.MADP = kh.MADP;
                    objDb.DUONGPHU = kh.DUONGPHU;
                    objDb.MADB = kh.MADB;
                    objDb.SODB = kh.MADP + kh.DUONGPHU + kh.MADB;

                    objDb.CHISOCUOI = obj.CHISODAU; objDb.KLTIEUTHU = 0; objDb.KLDHTONG = 0;
                    objDb.THBT = null;
                    objDb.PHIBVMT = 0; objDb.PTVAT = 0;
                    
                    objDb.TONGTIEN_PS = 0; objDb.TIENNUOC = 0; objDb.THUEBAO = 0;

                    objDb.INHD = false; objDb.HETNO = false; objDb.THUTQ = false;
                    objDb.MANVNHAPCN = null; objDb.MANVCN = null; objDb.NGAYNHAPCN = null; objDb.NGAYCN = null;
                    objDb.TIENCONGNO = 0;

                    _db.SubmitChanges();
                    goto commit;
                }

                //Important: Neu khach hang doi duong pho, cap nhat lai makv, madp, duongphu, sodb, madb
                objDb.MAKV = kh.MAKV;
                objDb.MAKVDN = kh.MAKVDN;
                objDb.MADP = kh.MADP;
                objDb.DUONGPHU = kh.DUONGPHU;
                objDb.MADB = kh.MADB;
                objDb.SODB = kh.MADP + kh.DUONGPHU + kh.MADB;

                objDb.CHISODAU = obj.CHISODAU;
                objDb.CHISOCUOI = obj.CHISOCUOI;
                objDb.KLTIEUTHU = obj.KLTIEUTHU;

                objDb.MTRUYTHU = obj.MTRUYTHU;
                

                objDb.MAMDSD = kh.MAMDSD;
                objDb.MACQ = kh.MACQ;
                objDb.SOHO = kh.SOHO;
                objDb.SONK = kh.SONK;
                objDb.ISDINHMUC = kh.ISDINHMUC;
                objDb.DMNK = kh.SODINHMUC;
                objDb.KHONGTINH117 = kh.KHONGTINH117;
                objDb.KYHOTRO = kh.KYHOTRO;

                objDb.TTSD = kh.TTSD;
                objDb.MAMDSD = kh.MAMDSD;

                objDb.TTHAIGHI = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;
                objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                //objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? GetHSKH(MAHSKH.PHIMT.ToString()) : 0;
                //objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? GetHSKH(MAHSKH.VAT.ToString()) : 0;

                objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? (decimal)0.05 : 0;
                objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? (decimal)0.05 : 0;
                objDb.MAHTTT = kh.MAHTTT;

                //TODO: update tình hình bất thường
                string ttbt;

                // chỉ số cuối = 0: ghi sót
                if (obj.CHISODAU > 0 && obj.CHISOCUOI == 0)
                    ttbt = "THGS";
                else
                {
                    var date = new DateTime(obj.NAM, obj.THANG, 1).AddMonths(-1);

                    var kltieuthuKytruoc = _db.TIEUTHUs.Where(tt => tt.SODB == obj.SODB &&
                                                                     tt.NAM == date.Year &&
                                                                     tt.THANG == date.Month).Select(t => t.KLTIEUTHU)
                        .SingleOrDefault();

                    ttbt = GetTHBT(objDb.KLTIEUTHU, kltieuthuKytruoc);
                }
                objDb.THBT = ttbt;

                ChangeTieuThu(objDb);

                //objDb.INHD = true;
                objDb.INHD = objDb.TONGTIEN > 0 ||
                    (objDb.GHI2THANG1LAN.Equals("1") && objDb.ING2T1L.Equals("1")) ? true : false
                              ;

                objDb.NGAYGHICS = DateTime.Now.Date;
                                     


                objDb.HETNO = false;
                objDb.THUTQ = false;

                objDb.MANVCN = null;
                objDb.MANVNHAPCN = null;
                objDb.NGAYCN = null;
                objDb.NGAYNHAPCN = null;

                _db.SubmitChanges();

            commit:
                // commit
                trans.Commit();
                _db.Connection.Close();

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "TT chỉ số");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "TT chỉ số");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "TT chỉ số");
            }

            return msg;
        }

        public Message UpdateList(List<GHICHISO> list)
        {
            Message msg;
            DbTransaction trans = null;

            var errorcount = 0;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in list)
                {
                    if (obj == null) continue;

                    //TODO: update chỉ số
                    // ReSharper disable AccessToModifiedClosure
                    var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == obj.IDKH &&
                                                                   tt.NAM == obj.NAM &&
                                                                   tt.THANG == obj.THANG);
                    // ReSharper restore AccessToModifiedClosure
                    if (objDb == null)
                    {
                        errorcount++;
                        continue;
                    }

                    var kh = objDb.KHACHHANG;
                    if (kh == null)
                    {
                        errorcount++;
                        continue;
                    }

                    if (kh.TTSD == "CUP")
                    {
                        objDb.TTSD = "CUP";
                        objDb.MAMDSD = kh.MAMDSD;
                        objDb.CHISOCUOI = obj.CHISODAU; objDb.KLTIEUTHU = 0; objDb.KLDHTONG = 0;
                        objDb.THBT = null;
                        objDb.PHIBVMT = 0; objDb.PTVAT = 0;
                        
                        objDb.TONGTIEN_PS = 0; objDb.TIENNUOC = 0; objDb.THUEBAO = 0;

                        objDb.INHD = false; objDb.HETNO = false; objDb.THUTQ = false;
                        objDb.MANVNHAPCN = null; objDb.MANVCN = null; objDb.NGAYNHAPCN = null; objDb.NGAYCN = null;
                        objDb.TIENCONGNO = 0;
                       
                        objDb.MALKH = kh.MALKH;
                        
                        objDb.CHISOCUOI = obj.CHISODAU; objDb.KLTIEUTHU = 0;
                        objDb.THBT = null;

                        objDb.M3MUC1 = 0; objDb.GIAMUC1 = 0; objDb.PHIMUC1 = 0; objDb.THUEMUC1 = 0;
                        objDb.M3MUC2 = 0; objDb.GIAMUC2 = 0; objDb.PHIMUC2 = 0; objDb.THUEMUC2 = 0;
                        objDb.M3MUC3 = 0; objDb.GIAMUC3 = 0; objDb.PHIMUC3 = 0; objDb.THUEMUC3 = 0;
                        objDb.M3MUC4 = 0; objDb.GIAMUC4 = 0; objDb.PHIMUC4 = 0; objDb.THUEMUC4 = 0;
                        objDb.TONGTIEN = 0; objDb.TIENNUOC = 0;
                        objDb.TIENTHUE = 0; objDb.TIENPHI = 0;

                        objDb.ISGCS = false; 

                        _db.SubmitChanges();
                        continue;
                    }

                    //Important: Neu khach hang doi duong pho, cap nhat lai makv, madp, duongphu, sodb, madb
                    objDb.MAKV = kh.MAKV;
                    objDb.MADP = kh.MADP;
                    objDb.DUONGPHU = kh.DUONGPHU;
                    objDb.MADB = kh.MADB;
                    objDb.SODB = kh.MADP + kh.DUONGPHU + kh.MADB;

                    //Important: TTSD cua tieu thu phai theo ttsd cua khachhang
                    objDb.TTSD = kh.TTSD;
                    objDb.MAMDSD = kh.MAMDSD;

                    objDb.SONK = kh.SONK;
                    objDb.ISDINHMUC = kh.ISDINHMUC;
                    objDb.DMNK = kh.SODINHMUC;
                    objDb.KHONGTINH117 = kh.KHONGTINH117;

                    objDb.CHISODAU = obj.CHISODAU;
                    objDb.CHISOCUOI = obj.CHISOCUOI;
                    objDb.MTRUYTHU = obj.MTRUYTHU;
                    objDb.KLTIEUTHU = obj.KLTIEUTHU;                    

                    objDb.SOHO = kh.SOHO;
                    objDb.KOPHINT = kh.KOPHINT;
                    objDb.KOVAT = kh.KOVAT;
                    objDb.TTHAIGHI = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;
                    objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                    //TODO: update tình hình bất thường
                    var ttbt = "BT";

                    // chỉ số cuối = 0: ghi sót
                    if (obj.CHISODAU > 0 && obj.CHISOCUOI == 0)
                        ttbt = "THGS";

                    var date = new DateTime(obj.NAM, obj.THANG, 1).AddMonths(-1);
                    // ReSharper disable AccessToModifiedClosure
                    var kltieuthu_kytruoc = _db.TIEUTHUs.Where(tt => tt.SODB == obj.SODB &&
                                                    tt.NAM == date.Year &&
                                                    tt.THANG == date.Month).Select(t => t.KLTIEUTHU).SingleOrDefault();
                    // ReSharper restore AccessToModifiedClosure
                    if (obj.KLTIEUTHU == 0 && kltieuthu_kytruoc > 5)
                        ttbt = "TNKSD";

                    if (obj.KLTIEUTHU > 0 && obj.KLTIEUTHU < 6 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 5)
                        ttbt = "TNNH5LTT";

                    if (obj.KLTIEUTHU > 5 && obj.KLTIEUTHU < 11 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 4)
                        ttbt = "TNNH4LTT";

                    if (obj.KLTIEUTHU > 10 && obj.KLTIEUTHU < 21 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 3)
                        ttbt = "TNNH3LTT";

                    if (obj.KLTIEUTHU > 20 && obj.KLTIEUTHU < 101 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 2)
                        ttbt = "TNNH2LTT";

                    if (obj.KLTIEUTHU > 100 && obj.KLTIEUTHU < 501 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / (decimal)1.5)
                        ttbt = "TNNH1.5LTT";

                    if (obj.KLTIEUTHU > 500 && obj.KLTIEUTHU < 2001 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / (decimal)1.3)
                        ttbt = "TNNH1.3LTT";

                    if (obj.KLTIEUTHU > 2000 && obj.KLTIEUTHU < 5001 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / (decimal)1.2)
                        ttbt = "TNNH1.2LTT";

                    if (obj.KLTIEUTHU > 2000 && obj.KLTIEUTHU < 5001 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * (decimal)1.2)
                        ttbt = "TNLH1.2LTT";

                    if (obj.KLTIEUTHU > 500 && obj.KLTIEUTHU < 2001 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * (decimal)1.3)
                        ttbt = "TNLH1.3LTT";

                    if (obj.KLTIEUTHU > 100 && obj.KLTIEUTHU < 501 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * (decimal)1.5)
                        ttbt = "TNLH1.5LTT";

                    if (obj.KLTIEUTHU > 20 && obj.KLTIEUTHU < 101 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * 2)
                        ttbt = "TNLH2LTT";

                    if (obj.KLTIEUTHU > 5000)
                        ttbt = "TNBT";

                    objDb.THBT = ttbt;
                    objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? 100 : 0;
                    objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? (decimal)0.05 : 0;
                    objDb.KLDHTONG = obj.KLTIEUTHU;

                    ChangeTieuThu(objDb);

                    //if (objDb.TONGTIEN_PS > 0)
                    //    objDb.INHD = true;
                    objDb.INHD = objDb.TONGTIEN > 0 ||
                    (objDb.GHI2THANG1LAN.Equals("1") && objDb.ING2T1L.Equals("1")) ? true : false
                              ;

                    objDb.NGAYGHICS = DateTime.Now.Date;

                    //TODO: update cong no
                    objDb.HETNO = false;
                    objDb.THUTQ = false;

                    objDb.MANVCN = null;
                    objDb.MANVNHAPCN = null;
                    objDb.NGAYCN = null;
                    objDb.NGAYNHAPCN = null;
                    
                    objDb.INHD = objDb.TONGTIEN > 0;
                    objDb.ISGCS = true;

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();
                _db.Connection.Close();

                msg = errorcount > 0
                          ? new Message(MessageConstants.W_UPDATELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                "chỉ số", list.Count - errorcount + " khách hàng", errorcount, "khách hàng")
                          : new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, "chỉ số", list.Count + " khách hàng");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }

            return msg;
        }

        public Message UpdateListTT(List<GHICHISOTT> list)
        {
            Message msg;
            DbTransaction trans = null;

            var errorcount = 0;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in list)
                {
                    if (obj == null) continue;

                    //TODO: update chỉ số
                    // ReSharper disable AccessToModifiedClosure
                    var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == obj.IDKH &&
                                                                   tt.NAM == obj.NAM &&
                                                                   tt.THANG == obj.THANG);
                    // ReSharper restore AccessToModifiedClosure
                    if (objDb == null)
                    {
                        errorcount++;
                        continue;
                    }

                    var kh = objDb.KHACHHANG;
                    if (kh == null)
                    {
                        errorcount++;
                        continue;
                    }

                    if (kh.TTSD == "CUP")
                    {
                        objDb.TTSD = "CUP";
                        objDb.MAMDSD = kh.MAMDSD;
                        objDb.CHISOCUOI = obj.CHISODAU; objDb.KLTIEUTHU = 0; objDb.KLDHTONG = 0;
                        objDb.THBT = null;
                        objDb.PHIBVMT = 0; objDb.PTVAT = 0;
                        
                        objDb.TONGTIEN_PS = 0; objDb.TIENNUOC = 0; objDb.THUEBAO = 0;

                        objDb.INHD = false; objDb.HETNO = false; objDb.THUTQ = false;
                        objDb.MANVNHAPCN = null; objDb.MANVCN = null; objDb.NGAYNHAPCN = null; objDb.NGAYCN = null;
                        objDb.TIENCONGNO = 0;

                        _db.SubmitChanges();
                        continue;
                    }

                    //Important: Neu khach hang doi duong pho, cap nhat lai makv, madp, duongphu, sodb, madb
                    objDb.MAKV = kh.MAKV;
                    objDb.MADP = kh.MADP;
                    objDb.DUONGPHU = kh.DUONGPHU;
                    objDb.MADB = kh.MADB;
                    objDb.SODB = kh.MADP + kh.DUONGPHU + kh.MADB;

                    //Important: TTSD cua tieu thu phai theo ttsd cua khachhang
                    objDb.TTSD = kh.TTSD;
                    objDb.MAMDSD = kh.MAMDSD;

                    objDb.SONK = kh.SONK;
                    objDb.ISDINHMUC = kh.ISDINHMUC;
                    objDb.DMNK = kh.SODINHMUC;
                    objDb.KHONGTINH117 = kh.KHONGTINH117;

                    objDb.CHISODAU = obj.CHISODAU;
                    objDb.CHISOCUOI = obj.CHISOCUOI;
                    objDb.KLTIEUTHU = obj.KLTIEUTHU;

                    objDb.MTRUYTHU = obj.MTRUYTHU;
                    

                    objDb.TTHAIGHI = !string.IsNullOrEmpty(obj.TTHAIGHI.Trim()) ? obj.TTHAIGHI.Trim() : null;
                    objDb.MANVN_CS = !string.IsNullOrEmpty(obj.MANV_CS.Trim()) ? obj.MANV_CS.Trim() : null;

                    //TODO: update tình hình bất thường
                    var ttbt = "BT";

                    // chỉ số cuối = 0: ghi sót
                    if (obj.CHISODAU > 0 && obj.CHISOCUOI == 0)
                        ttbt = "THGS";

                    var date = new DateTime(obj.NAM, obj.THANG, 1).AddMonths(-1);
                    // ReSharper disable AccessToModifiedClosure
                    var kltieuthu_kytruoc = _db.TIEUTHUs.Where(tt => tt.SODB == obj.SODB &&
                                                    tt.NAM == date.Year &&
                                                    tt.THANG == date.Month).Select(t => t.KLTIEUTHU).SingleOrDefault();
                    // ReSharper restore AccessToModifiedClosure
                    if (obj.KLTIEUTHU == 0 && kltieuthu_kytruoc > 5)
                        ttbt = "TNKSD";

                    if (obj.KLTIEUTHU > 0 && obj.KLTIEUTHU < 6 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 5)
                        ttbt = "TNNH5LTT";

                    if (obj.KLTIEUTHU > 5 && obj.KLTIEUTHU < 11 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 4)
                        ttbt = "TNNH4LTT";

                    if (obj.KLTIEUTHU > 10 && obj.KLTIEUTHU < 21 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 3)
                        ttbt = "TNNH3LTT";

                    if (obj.KLTIEUTHU > 20 && obj.KLTIEUTHU < 101 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / 2)
                        ttbt = "TNNH2LTT";

                    if (obj.KLTIEUTHU > 100 && obj.KLTIEUTHU < 501 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / (decimal)1.5)
                        ttbt = "TNNH1.5LTT";

                    if (obj.KLTIEUTHU > 500 && obj.KLTIEUTHU < 2001 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / (decimal)1.3)
                        ttbt = "TNNH1.3LTT";

                    if (obj.KLTIEUTHU > 2000 && obj.KLTIEUTHU < 5001 &&
                        obj.KLTIEUTHU < kltieuthu_kytruoc / (decimal)1.2)
                        ttbt = "TNNH1.2LTT";

                    if (obj.KLTIEUTHU > 2000 && obj.KLTIEUTHU < 5001 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * (decimal)1.2)
                        ttbt = "TNLH1.2LTT";

                    if (obj.KLTIEUTHU > 500 && obj.KLTIEUTHU < 2001 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * (decimal)1.3)
                        ttbt = "TNLH1.3LTT";

                    if (obj.KLTIEUTHU > 100 && obj.KLTIEUTHU < 501 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * (decimal)1.5)
                        ttbt = "TNLH1.5LTT";

                    if (obj.KLTIEUTHU > 20 && obj.KLTIEUTHU < 101 &&
                        obj.KLTIEUTHU > kltieuthu_kytruoc * 2)
                        ttbt = "TNLH2LTT";

                    if (obj.KLTIEUTHU > 5000)
                        ttbt = "TNBT";

                    objDb.THBT = ttbt;
                    objDb.PHIBVMT = (kh.KOPHINT.HasValue && !kh.KOPHINT.Value) ? 100 : 0;
                    objDb.PTVAT = (kh.VAT.HasValue && kh.VAT.Value) ? (decimal)0.05 : 0;
                    objDb.KLDHTONG = obj.KLTIEUTHU;

                    ChangeTieuThu(objDb);

                    //if (objDb.TONGTIEN_PS > 0)
                    //    objDb.INHD = true;
                    objDb.INHD = objDb.TONGTIEN > 0 ||
                    (objDb.GHI2THANG1LAN.Equals("1") && objDb.ING2T1L.Equals("1")) ? true : false
                              ;

                    objDb.NGAYGHICS = DateTime.Now.Date;

                    //TODO: update cong no
                    objDb.HETNO = false;
                    objDb.THUTQ = false;

                    objDb.MANVCN = null;
                    objDb.MANVNHAPCN = null;
                    objDb.NGAYCN = null;
                    objDb.NGAYNHAPCN = null;

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();
                _db.Connection.Close();

                msg = errorcount > 0
                          ? new Message(MessageConstants.W_UPDATELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                "chỉ số", list.Count - errorcount + " khách hàng", errorcount, "khách hàng")
                          : new Message(MessageConstants.I_UPDATELIST_SUCCEED, MessageType.Info, "chỉ số", list.Count + " khách hàng");
            }
            catch (Exception ex)
            {
                try
                {
                    // rollback transaction
                    if (trans != null)
                        trans.Rollback();
                    if (_db.Connection.State == ConnectionState.Open)
                        _db.Connection.Close();
                }
                catch
                {
                    return ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
                }

                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chỉ số");
            }

            return msg;
        }

        public List<TINHCUOC> GetListForTinhCuoc(DateTime kyghi, String makv, String madp, String duongphu)
        {
            var firstDay = new DateTime(kyghi.Year, kyghi.Month, 1);
            var lastDay = firstDay.AddMonths(1);

            var list = _db.LOCKSTATUS.Join(_db.DUONGPHOs, LOCK => LOCK.MADP, DP => DP.MADP,
                                           (LOCK, DP) => new { LOCK, DP }).
                                    Where(ldp => ldp.LOCK.KYGT >= firstDay && ldp.LOCK.KYGT < lastDay);

            if (makv != null && makv != "%")
                list = list.Where(ldp => ldp.DP.MAKV == makv);

            if (madp != null)
                list = list.Where(ldp => ldp.DP.MADP == madp);

            if (duongphu != null)
                list = list.Where(ldp => ldp.DP.DUONGPHU == duongphu);

            // SODB, NAM, THANG, DAKS (LOCKSTATUS), LOTRINH, SONHA, TENKH, CHISODAU, CHISOCUOI, KLTIEUTHU, TTHAIGHI, THBT, GHICHU_CS, TTSD, MAMDSD
            return list.Select(ldp => new
            {
                ldp.DP.MADP,
                ldp.DP.DUONGPHU,
                ldp.DP.MAKV,
                ldp.DP.TENDP,
                ldp.LOCK.LOCKGCS,
                ldp.LOCK.LOCKTINHCUOC,
                ldp.LOCK.LOCKCN

                //TODO: join them bang nhan vien sau,
            }).AsEnumerable().Select(x => new TINHCUOC
            {
                MADP = x.MADP,
                DUONGPHU = x.DUONGPHU,
                MAKV = x.MAKV,
                TENKV = _db.KHUVUCs.Where(k => k.MAKV.Equals(x.MAKV)).FirstOrDefault().TENKV,
                TENDP = x.TENDP,
                LOCKGCS = x.LOCKGCS,
                LOCKTINHCUOC = x.LOCKTINHCUOC,
                LOCKCN = x.LOCKCN,
                ENABLED = !(x.LOCKTINHCUOC.HasValue && x.LOCKTINHCUOC.Value),
                SHOWUNLOCKED = x.LOCKTINHCUOC.HasValue && x.LOCKTINHCUOC.Value,
                KYGHI = kyghi
            })
            .OrderBy(x => x.DUONGPHU)
            .OrderBy(x => x.MADP)
            .OrderBy(x => x.MAKV).ToList();
        }

        public List<TINHCUOC> GetListForTinhCuocDotIn(DateTime kyghi, String makv, String madp, String duongphu, string iddotin)
        {
            var firstDay = new DateTime(kyghi.Year, kyghi.Month, 1);
            var lastDay = firstDay.AddMonths(1);

            var list = _db.LOCKSTATUS.Join(_db.DUONGPHOs, LOCK => LOCK.MADP, DP => DP.MADP, 
                                           (LOCK, DP) => new { LOCK, DP }).
                        Where(ldp => ldp.LOCK.KYGT >= firstDay && ldp.LOCK.KYGT < lastDay && ldp.DP.IDMADOTIN.Equals(iddotin));    

            if (makv != null && makv != "%")
                list = list.Where(ldp => ldp.DP.MAKV == makv);

            if (madp != null)
                list = list.Where(ldp => ldp.DP.MADP == madp);

            if (duongphu != null)
                list = list.Where(ldp => ldp.DP.DUONGPHU == duongphu);

            // SODB, NAM, THANG, DAKS (LOCKSTATUS), LOTRINH, SONHA, TENKH, CHISODAU, CHISOCUOI, KLTIEUTHU, TTHAIGHI, THBT, GHICHU_CS, TTSD, MAMDSD
            return list.Select(ldp => new
            {
                ldp.DP.MADP,
                ldp.DP.DUONGPHU,
                ldp.DP.MAKV,
                ldp.DP.TENDP,
                ldp.LOCK.LOCKGCS,
                ldp.LOCK.LOCKTINHCUOC,
                ldp.LOCK.LOCKCN

                //TODO: join them bang nhan vien sau,
            }).AsEnumerable().Select(x => new TINHCUOC
            {
                MADP = x.MADP,
                DUONGPHU = x.DUONGPHU,
                MAKV = x.MAKV,
                TENKV = _db.KHUVUCs.Where(k => k.MAKV.Equals(x.MAKV)).FirstOrDefault().TENKV,
                TENDP = x.TENDP,
                LOCKGCS = x.LOCKGCS,
                LOCKTINHCUOC = x.LOCKTINHCUOC,
                LOCKCN = x.LOCKCN,
                ENABLED = !(x.LOCKTINHCUOC.HasValue && x.LOCKTINHCUOC.Value),
                SHOWUNLOCKED = x.LOCKTINHCUOC.HasValue && x.LOCKTINHCUOC.Value,
                KYGHI = kyghi
            })
            .OrderBy(x => x.DUONGPHU)
            .OrderBy(x => x.MADP)
            .OrderBy(x => x.MAKV).ToList();
        }



        public Message UnlockTinhCuoc(DateTime kyghi, DUONGPHO dp)
        {
            Message msg;

            try
            {
                var lockstatus = _db.LOCKSTATUS.FirstOrDefault(l => l.MADP == dp.MADP &&
                                                                    l.DUONGPHU == dp.DUONGPHU && l.KYGT.Month == kyghi.Month &&
                                                                    l.KYGT.Year == kyghi.Year);

                if (lockstatus != null && lockstatus.LOCKGCS == true)
                {
                    return new Message(MessageConstants.E_FAILED_EXCEPTION, MessageType.Error, "Mở khóa ghi chỉ số", "Kỳ ghi chỉ số kế tiếp đã được khởi tạo."); 
                }

                if (lockstatus != null) 
                    lockstatus.LOCKTINHCUOC = false;

                _db.SubmitChanges();

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Mở khóa ghi chỉ số");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleInsertException(ex, "Mở khóa ghi chỉ số");
            }
            return msg;
        }

        public Message TinhCuoc(List<TINHCUOC> updateList)
        {
            Message msg;

            try
            {
                //TODO: lock
                foreach (var tinhcuoc in updateList)
                {
                    var temp = tinhcuoc;
                    var lockstatus = _db.LOCKSTATUS.Where(l => l.MADP == temp.MADP &&
                                                               l.DUONGPHU == temp.DUONGPHU && l.KYGT.Month == temp.KYGHI.Month &&
                                                               l.KYGT.Year == temp.KYGHI.Year).FirstOrDefault();

                    lockstatus.LOCKTINHCUOC = true;
                }
                _db.SubmitChanges();

                //TODO: tinh cuoc later

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Khóa ghi chỉ số");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleInsertException(ex, "Khóa ghi chỉ sốc");
            }
            return msg;
        }

        public List<TINHCUOC> GetListForTinhCuoc(DateTime kyghi, String makv, String madp)
        {
            var firstDay = new DateTime(kyghi.Year, kyghi.Month, 1);
            var lastDay = firstDay.AddMonths(1);

            var list = _db.LOCKSTATUS.Join(_db.DUONGPHOs, LOCK => LOCK.MADP, DP => DP.MADP,
                                           (LOCK, DP) => new { LOCK, DP }).
                                    Where(ldp => ldp.LOCK.KYGT >= firstDay && ldp.LOCK.KYGT < lastDay);

            if (makv != null && makv != "%")
                list = list.Where(ldp => ldp.DP.MAKV == makv);

            if (madp != null)
                list = list.Where(ldp => ldp.DP.MADP == madp);

            // SODB, NAM, THANG, DAKS (LOCKSTATUS), LOTRINH, SONHA, TENKH, CHISODAU, CHISOCUOI, KLTIEUTHU, TTHAIGHI, THBT, GHICHU_CS, TTSD, MAMDSD
            return list.Select(ldp => new
            {
                ldp.DP.MADP,
                ldp.DP.MAKV,
                ldp.DP.TENDP,
                ldp.LOCK.LOCKGCS,
                ldp.LOCK.LOCKTINHCUOC,
                ldp.LOCK.LOCKCN

                //TODO: join them bang nhan vien sau,
            }).AsEnumerable().Select(x => new TINHCUOC
            {
                MADP = x.MADP,
                MAKV = x.MAKV,
                TENKV = _db.KHUVUCs.Where(k => k.MAKV.Equals(x.MAKV)).FirstOrDefault().TENKV,
                TENDP = x.TENDP,
                LOCKGCS = x.LOCKGCS,
                LOCKTINHCUOC = x.LOCKTINHCUOC,
                LOCKCN = x.LOCKCN,
                ENABLED = !(x.LOCKTINHCUOC.HasValue && x.LOCKTINHCUOC.Value),
                SHOWUNLOCKED = x.LOCKTINHCUOC.HasValue && x.LOCKTINHCUOC.Value,
                KYGHI = kyghi
            })
            .OrderBy(x => x.DUONGPHU)
            .OrderBy(x => x.MADP)
            .OrderBy(x => x.MAKV).ToList();
        }

        public List<TIEUTHU> GetLitForExport(DateTime kyghi, string mamdsd, string madp)
        {
            return _db.TIEUTHUs.Where(p => !p.TTSD.Equals("CUP") && (p.MADP.Equals(madp) || string.IsNullOrEmpty(madp))
                && (p.MALKH.Equals(mamdsd) || string.IsNullOrEmpty(mamdsd))
                && p.THANG.Equals(kyghi.Month)
                && p.NAM.Equals(kyghi.Year)).OrderBy(p => p.SODB).OrderBy(p => p.MADP).ToList();
        }

        public bool IsLockTinhCuoc(DateTime kyghi, DUONGPHO dp)
        {
            var lockKN = _db.LOCKSTATUS.Where(lck => lck.MADP.Equals(dp.MADP)
                                                     && lck.DUONGPHU.Equals(dp.DUONGPHU)
                                                     && lck.KYGT.Month.Equals(kyghi.Month)
                                                     && lck.KYGT.Year.Equals(kyghi.Year))
                                                .Select(lck => lck.LOCKTINHCUOC).FirstOrDefault();
            lockKN = lockKN ?? false;

            return lockKN.Value;
        }

        public bool IsLockTinhCuocKy(DateTime kyghi, String makv)        
        {
            var lockKN = _db.LOCKSTATUS.Where(lck => lck.KYGT.Month.Equals(kyghi.Month)
                                                     && lck.KYGT.Year.Equals(kyghi.Year)
                                                     && lck.MAKV.Equals(makv))
                                                .Select(lck => lck.LOCKTINHCUOC).FirstOrDefault();
            lockKN = lockKN ?? false;

            return lockKN.Value;
        }

        public bool IsLockTinhCuocKy1(DateTime kyghi, String makv, String dp)        
        {
            var lockKN = _db.LOCKSTATUS.Where(lck => lck.KYGT.Month.Equals(kyghi.Month)
                                                     && lck.KYGT.Year.Equals(kyghi.Year)
                                                     && lck.MAKV.Equals(makv)
                                                     && lck.MADP.Equals(dp))
                                                .Select(lck => lck.LOCKTINHCUOC).FirstOrDefault();
            lockKN = lockKN ?? false;

            return lockKN.Value;
        }

        public int IsLockTachDuongNKy(int thang, int nam, string makv, string madpcu)
        //public bool IsLockTinhCuocKy(DateTime kyghi)
        {
            var lockKN = _db.TACHDUONGNs.Where(p => p.THANG.Equals(thang)  && p.NAM.Equals(nam)   && p.MAKV.Equals(makv)
                                                 && p.MADPCU.Equals(madpcu))
                                        .Count();
            //lockKN = lockKN ?? false;

            return lockKN;
        }
    }

    public class TINHTIEN
    {
        public decimal TienNuoc { get; set; }
        public decimal TienThue { get; set; }
        public decimal TienThueMoiTruong { get; set; }
        public decimal TongTienNuoc { get; set; }

        public TINHTIEN() { }

        public TINHTIEN(decimal tiennuoc, decimal tienthue, decimal tienthuemoitruong, decimal tongtiennuoc)
        {
            TienNuoc = tiennuoc;
            TienThue = tienthue;
            TienThueMoiTruong = tienthuemoitruong;
            TongTienNuoc = tongtiennuoc;
        }
    }

    public class TINHCUOC
    {
        public DateTime KYGHI { get; set; }
        public string MADP { get; set; }
        public string TENDP { get; set; }
        public string DUONGPHU { get; set; }
        public string MAKV { get; set; }
        public string TENKV { get; set; }
        public bool? LOCKGCS { get; set; }
        public bool? LOCKTINHCUOC { get; set; }
        public bool? LOCKCN { get; set; }
        public bool ENABLED { get; set; }
        public bool SHOWUNLOCKED { get; set; }      


        public TINHCUOC()
        {

        }

        public TINHCUOC(string madp, string duongphu, string makv, string tenkv, string tendp,
            bool? lockgcs, bool? locktinhcuoc, bool? lockcn,
            bool enabled, bool showUnlocked, DateTime kyghi)
        {
            MADP = madp;
            DUONGPHU = duongphu;
            MAKV = makv;
            TENKV = tenkv;
            TENDP = tendp;
            LOCKGCS = lockgcs;
            LOCKTINHCUOC = locktinhcuoc;
            LOCKCN = lockcn;
            ENABLED = enabled;
            SHOWUNLOCKED = showUnlocked;
            KYGHI = kyghi;
        }
    }

    public class GHICHISO
    {
        public GHICHISO()
        {

        }

        public GHICHISO(string idkh, string soDb, string madp, int nam, int thang, string sonha, string tenkh,
            decimal? chisodau, decimal? chisocuoi, decimal? mtruythu, decimal kltieuthu,
            string tthaighi, string manv_cs)//,             string dotinhd, string phienlx)
        {
            IDKH = idkh; SODB = soDb; MADP = madp; NAM = nam; THANG = thang;
            SONHA = sonha; TENKH = tenkh;
            CHISODAU = chisodau; CHISOCUOI = chisocuoi;
            MTRUYTHU = mtruythu; KLTIEUTHU = kltieuthu;
            TTHAIGHI = tthaighi; MANV_CS = manv_cs;

            //DOTINHD = dotinhd; PHIENLX = phienlx;
        }

        public string MADP { get; set; }
        public string IDKH { get; set; }
        public string SODB { get; set; }
        public int NAM { get; set; }
        public int THANG { get; set; }
        public string SONHA { get; set; }
        public string TENKH { get; set; }
        public decimal? CHISODAU { get; set; }
        public decimal? CHISOCUOI { get; set; }
        public decimal? MTRUYTHU { get; set; }
        public decimal? KLTIEUTHU { get; set; }
        public string TTHAIGHI { get; set; }
        public string MANV_CS { get; set; }

        //public string DOTINHD { get; set; }
        //public string PHIENLX { get; set; }
       
    }

    public class GCSTTKYTRUOC
    {
        public GCSTTKYTRUOC()
        {

        }

        public GCSTTKYTRUOC(string idkh, string soDb, string madp, int nam, int thang, string sonha, string tenkh,
            decimal? chisodau, decimal? chisocuoi, decimal? mtruythu, decimal kltieuthu, string tthaighi, string manv_cs,
            decimal? chisodau_1, decimal? chisocuoi_1, decimal kltieuthu_1)
        {
            IDKH = idkh; SODB = soDb; MADP = madp; NAM = nam; THANG = thang;
            SONHA = sonha; TENKH = tenkh;
            CHISODAU = chisodau; CHISOCUOI = chisocuoi;
            MTRUYTHU = mtruythu; KLTIEUTHU = kltieuthu;
            TTHAIGHI = tthaighi; MANV_CS = manv_cs;

            CHISODAU_1 = chisodau_1; CHISOCUOI_1 = chisocuoi_1;
            KLTIEUTHU_1 = kltieuthu_1;
        }

        public string MADP { get; set; }
        public string IDKH { get; set; }
        public string SODB { get; set; }
        public int NAM { get; set; }
        public int THANG { get; set; }
        public string SONHA { get; set; }
        public string TENKH { get; set; }
        public decimal? CHISODAU { get; set; }
        public decimal? CHISOCUOI { get; set; }
        public decimal? MTRUYTHU { get; set; }
        public decimal? KLTIEUTHU { get; set; }
        public string TTHAIGHI { get; set; }
        public string MANV_CS { get; set; }

        public decimal? CHISODAU_1 { get; set; }
        public decimal? CHISOCUOI_1 { get; set; }        
        public decimal? KLTIEUTHU_1 { get; set; }

    }

    public class GHICHISOTT
    {
          public GHICHISOTT()
        {

        }
        
        public GHICHISOTT(string idkh, string soDb, string madp, int nam, int thang, string sonha, string tenkh,
            decimal? chisodau, decimal? chisocuoi, decimal kltieuthu,
            decimal mtruythu, string tthaighi, string manv_cs)
        {
            IDKH = idkh; SODB = soDb; MADP = madp; NAM = nam; THANG = thang; 
            SONHA = sonha; TENKH = tenkh;
            CHISODAU = chisodau; CHISOCUOI = chisocuoi; KLTIEUTHU = kltieuthu;
            MTRUYTHU = mtruythu; 
            TTHAIGHI = tthaighi; MANV_CS = manv_cs;
        }

        public string MADP { get; set; }
        public string IDKH { get; set; }
        public string SODB { get; set; }
        public int NAM { get; set; }
        public int THANG { get; set; }
        public string SONHA { get; set; }
        public string TENKH { get; set; }
        public decimal? CHISODAU { get; set; }
        public decimal? CHISOCUOI { get; set; }
        public decimal? KLTIEUTHU { get; set; }
        public decimal? MTRUYTHU { get; set; }
        
        public string TTHAIGHI { get; set; }
        public string MANV_CS { get; set; }

      
    }

    public class GHICHISODB
    {
        public GHICHISODB()
        {

        }

        public GHICHISODB(string idkh, string soDb, string madp, string madb, 
            int nam, int thang, string sonha, string tenkh)
        {
            IDKH = idkh; SODB = soDb; MADP = madp; MADB = madb;
            NAM = nam; THANG = thang;
            SONHA = sonha; TENKH = tenkh;
        }

        public string IDKH { get; set; }
        public string SODB { get; set; }
        public string MADP { get; set; }
        public string MADB { get; set; }  
        public int NAM { get; set; }
        public int THANG { get; set; }
        public string SONHA { get; set; }
        public string TENKH { get; set; }
        
    }

}
