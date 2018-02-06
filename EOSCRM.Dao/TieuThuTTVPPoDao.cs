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
    public class TieuThuTTVPPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public TieuThuTTVPPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }


        public TIEUTHUTTVPPO Get(string ma)
        {
            return _db.TIEUTHUTTVPPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma));
        }

        public TIEUTHUTTVPPO GetTN(string ma, int thang, int nam)
        {
            return _db.TIEUTHUTTVPPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }

        public Message Insert(TIEUTHUTTVPPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.TIEUTHUTTVPPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKHPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.VT00.ToString(),
                    MATT = "TTVPKHPO",
                    MOTA = "Truy thu VP khách hàng điện."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "truy thu vi phạm");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "truy thu", objUi.IDKHPO);
            }
            return msg;
        }

        public Message Update(TIEUTHUTTVPPO obj)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {

                var objDb = _db.TIEUTHUTTVPPOs.SingleOrDefault(tt => tt.IDKHPO == obj.IDKHPO &&
                                                               tt.NAM == obj.NAM &&
                                                               tt.THANG == obj.THANG);
                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Tiêu thụ");


                objDb.MADPPO = obj.MADPPO;
                objDb.DUONGPHUPO = obj.DUONGPHUPO;
                objDb.MADBPO = obj.MADBPO;
                objDb.SODB = obj.MADPPO + obj.DUONGPHUPO + obj.MADBPO;
                objDb.MAMDSDPO = obj.MAMDSDPO;
                objDb.SOHO = obj.SOHO;


                _db.SubmitChanges();

                commit:
                // commit
                trans.Commit();
                _db.Connection.Close();
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Tiêu thụ");
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
                    return ExceptionHandler.HandleInsertException(ex, "Tiêu thụ");
                }
                msg = ExceptionHandler.HandleInsertException(ex, "Tiêu thụ");
            }
            return msg;
        }

        public List<TIEUTHUTTVPPO1> GetDC(string khuvuc, int thang, int nam)
        {
            var q = _db.TIEUTHUTTVPPOs
                .Join(_db.KHACHHANGPOs, tt => tt.IDKHPO.Trim(), kh => kh.IDKHPO.Trim(), 
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG==thang && tt.tt.NAM==nam && tt.tt.MAKVPO==khuvuc);
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKHPO, @res.tt.SODB, @res.tt.MADPPO, @res.tt.NAM, @res.tt.THANG,
                @res.kh.SONHA, @res.kh.TENKH,
                @res.tt.CHISODAU, @res.tt.CHISOCUOI, @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT", @res.tt.MANVN_CS,
                @res.tt.CHISODAUVP,
                @res.tt.CHISOCUOIVP,
                @res.tt.KLTIEUTHUVP,
                @res.tt.TIENNUOCVP,
                @res.tt.TONGTIENVP,
                @res.tt.TIENPHIVP,
                @res.tt.TIENTHUEVP,
                @res.tt.MTRUYTHUVP,
                @res.tt.GHICHUVP,
                @res.tt.MASOHDVP,
                @res.tt.INHDVP
            }).AsEnumerable().Select(x => new TIEUTHUTTVPPO1
            {
                IDKHPO = x.IDKHPO, SODB = x.SODB, MADPPO = x.MADPPO, NAM = x.NAM, THANG = x.THANG,
                SONHA = x.SONHA, TENKH = x.TENKH,  CHISODAU = x.CHISODAU,  CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,  TTHAIGHI = x.TTHAIGHI, MANV_CS = x.MANVN_CS,
                CHISODAUVP = x.CHISODAUVP,
                CHISOCUOIVP = x.CHISOCUOIVP,
                KLTIEUTHUVP = x.KLTIEUTHUVP,
                TIENNUOCVP = x.TIENNUOCVP,
                TONGTIENVP = x.TONGTIENVP,
                TIENPHIVP = x.TIENPHIVP,
                TIENTHUEVP = x.TIENTHUEVP,
                MTRUYTHUVP = x.MTRUYTHUVP,
                GHICHUVP = x.GHICHUVP,
                MASOHDVP = x.MASOHDVP,
                INHDVP = x.INHDVP
            }).ToList();
            
        }

        public List<TIEUTHUTTVPPO1> GetDCDotIn(string khuvuc, int thang, int nam, string idmadot)
        {
            var q = _db.TIEUTHUTTVPPOs
                .Join(_db.KHACHHANGPOs, tt => tt.IDKHPO.Trim(), kh => kh.IDKHPO.Trim(),
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG == thang && tt.tt.NAM == nam && tt.tt.MAKVPO == khuvuc
                    && tt.tt.IDMADOTIN.Equals(idmadot));
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKHPO,
                @res.tt.SODB,
                @res.tt.MADPPO,
                @res.tt.NAM,
                @res.tt.THANG,
                @res.kh.SONHA,
                @res.kh.TENKH,
                @res.tt.CHISODAU,
                @res.tt.CHISOCUOI,
                @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT",
                @res.tt.MANVN_CS,
                @res.tt.CHISODAUVP,
                @res.tt.CHISOCUOIVP,
                @res.tt.KLTIEUTHUVP,
                @res.tt.TIENNUOCVP,
                @res.tt.TONGTIENVP,
                @res.tt.TIENPHIVP,
                @res.tt.TIENTHUEVP,
                @res.tt.MTRUYTHUVP,
                @res.tt.GHICHUVP,
                @res.tt.MASOHDVP,
                @res.tt.INHDVP
            }).AsEnumerable().Select(x => new TIEUTHUTTVPPO1
            {
                IDKHPO = x.IDKHPO,
                SODB = x.SODB,
                MADPPO = x.MADPPO,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS,
                CHISODAUVP = x.CHISODAUVP,
                CHISOCUOIVP = x.CHISOCUOIVP,
                KLTIEUTHUVP = x.KLTIEUTHUVP,
                TIENNUOCVP = x.TIENNUOCVP,
                TONGTIENVP = x.TONGTIENVP,
                TIENPHIVP = x.TIENPHIVP,
                TIENTHUEVP = x.TIENTHUEVP,
                MTRUYTHUVP = x.MTRUYTHUVP,
                GHICHUVP = x.GHICHUVP,
                MASOHDVP = x.MASOHDVP,
                INHDVP = x.INHDVP
            }).ToList();

        }

        public List<TIEUTHUTTVPPO1> GetDC1(int thang, int nam)
        {
            var q = _db.TIEUTHUTTVPPOs
                .Join(_db.KHACHHANGPOs, tt => tt.IDKHPO.Trim(), kh => kh.IDKHPO.Trim(),
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG == thang && tt.tt.NAM == nam);
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKHPO,
                @res.tt.SODB,
                @res.tt.MADPPO,
                @res.tt.NAM,
                @res.tt.THANG,
                @res.kh.SONHA,
                @res.kh.TENKH,
                @res.tt.CHISODAU,
                @res.tt.CHISOCUOI,
                @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT",
                @res.tt.MANVN_CS,
                @res.tt.CHISODAUVP,
                @res.tt.CHISOCUOIVP,
                @res.tt.KLTIEUTHUVP,
                @res.tt.TIENNUOCVP,
                @res.tt.TONGTIENVP,
                @res.tt.TIENPHIVP,
                @res.tt.TIENTHUEVP,
                @res.tt.MTRUYTHUVP,
                @res.tt.GHICHUVP,
                @res.tt.MASOHDVP,
                @res.tt.INHDVP
            }).AsEnumerable().Select(x => new TIEUTHUTTVPPO1
            {
                IDKHPO = x.IDKHPO,
                SODB = x.SODB,
                MADPPO = x.MADPPO,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS,
                CHISODAUVP = x.CHISODAUVP,
                CHISOCUOIVP = x.CHISOCUOIVP,
                KLTIEUTHUVP = x.KLTIEUTHUVP,
                TIENNUOCVP = x.TIENNUOCVP,
                TONGTIENVP = x.TONGTIENVP,
                TIENPHIVP = x.TIENPHIVP,
                TIENTHUEVP = x.TIENTHUEVP,
                MTRUYTHUVP = x.MTRUYTHUVP,
                GHICHUVP = x.GHICHUVP,
                MASOHDVP = x.MASOHDVP,
                INHDVP = x.INHDVP
            }).ToList();

        }
       
    }

    public class TIEUTHUTTVPPO1
    {
        public TIEUTHUTTVPPO1()
        {

        }

        public TIEUTHUTTVPPO1(string idkh, string soDb, string madp, int nam, int thang, string sonha, string tenkh,
            decimal? chisodau, decimal? chisocuoi, decimal? mtruythu, decimal kltieuthu,
            string tthaighi, string manv_cs, 
            decimal? chisodauvp, decimal? chisocuoivp, decimal kltieuthuvp,
            decimal tiennuocvp, decimal tongtienvp, decimal tienphivp, decimal tienthuevp,
            decimal? mtruythuvp, string ghichuvp, string masohdvp, string inhdvp)
        {
            IDKHPO = idkh; SODB = soDb; MADPPO = madp; NAM = nam; THANG = thang;
            SONHA = sonha; TENKH = tenkh;
            CHISODAU = chisodau; CHISOCUOI = chisocuoi;
            MTRUYTHU = mtruythu; KLTIEUTHU = kltieuthu;
            TTHAIGHI = tthaighi; MANV_CS = manv_cs;
            CHISODAUVP = chisodauvp; CHISOCUOIVP = chisocuoivp; KLTIEUTHUVP = kltieuthuvp;
            TIENNUOCVP = tiennuocvp; TONGTIENVP = tongtienvp; TIENPHIVP = tienphivp;
            TIENTHUEVP = tienthuevp; MTRUYTHUVP = mtruythuvp; GHICHUVP = ghichuvp; MASOHDVP = masohdvp;
            INHDVP = inhdvp;
        }

        public string MADPPO { get; set; }
        public string IDKHPO { get; set; }
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
        public decimal? CHISODAUVP { get; set; }
        public decimal? CHISOCUOIVP { get; set; }
        public decimal? KLTIEUTHUVP { get; set; }
        public decimal? TIENNUOCVP { get; set; }
        public decimal? TONGTIENVP { get; set; }
        public decimal? TIENPHIVP { get; set; }
        public decimal? TIENTHUEVP { get; set; }
        public decimal? MTRUYTHUVP { get; set; }
        public string GHICHUVP { get; set; }
        public string MASOHDVP { get; set; }
        public string INHDVP { get; set; }
    }
}
