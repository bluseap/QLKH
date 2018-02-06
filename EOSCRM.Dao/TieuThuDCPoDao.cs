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
    public class TieuThuDCPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public TieuThuDCPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }


        public TIEUTHUDCPO Get(string ma)
        {
            return _db.TIEUTHUDCPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma));
        }

        public TIEUTHUDCPO GetTN(string ma, int thang, int nam)
        {
            return _db.TIEUTHUDCPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }

        public Message Insert(TIEUTHUDCPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.TIEUTHUDCPOs.InsertOnSubmit(objUi);
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
                    MATT = CHUCNANGKYDUYET.VT00.ToString(),
                    MOTA = "Điều chỉnh hoá đơn điện."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "điều chỉnh chỉ số");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "vật tư", objUi.IDKHPO);
            }
            return msg;
        }

        public Message Update(TIEUTHUDCPO obj)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {

                var objDb = _db.TIEUTHUPOs.SingleOrDefault(tt => tt.IDKHPO == obj.IDKHPO &&
                                                               tt.NAM == obj.NAM &&
                                                               tt.THANG == obj.THANG);
                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Tiêu thụ");


                objDb.MADPPO = obj.MADPPO;
                objDb.DUONGPHUPO = obj.DUONGPHUPO;
                objDb.MADBPO = obj.MADBPO;
                objDb.SODBPO = obj.MADPPO + obj.DUONGPHUPO + obj.MADBPO;
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

        public List<TIEUTHUDC1PO> GetDC(string khuvuc, int thang, int nam)
        {
            var q = _db.TIEUTHUDCPOs
                .Join(_db.KHACHHANGPOs, tt => tt.IDKHPO.Trim(), kh => kh.IDKHPO.Trim(), 
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG==thang && tt.tt.NAM==nam && tt.tt.MAKVPO==khuvuc);
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKHPO, @res.tt.SODBPO, @res.tt.MADPPO, @res.tt.NAM, @res.tt.THANG,
                @res.kh.SONHA, @res.kh.TENKH,
                @res.tt.CHISODAU, @res.tt.CHISOCUOI, @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT", @res.tt.MANVN_CS,
                @res.tt.CHISODAUDC,@res.tt.CHISOCUOIDC,@res.tt.KLTIEUTHUDC,
                @res.tt.TIENNUOCDC,@res.tt.TONGTIENDC,@res.tt.TIENPHIDC,
                @res.tt.TIENTHUEDC,@res.tt.MTRUYTHUDC,@res.tt.GHICHUDC,@res.tt.MASOHDDC,
                @res.tt.INHDDC
            }).AsEnumerable().Select(x => new TIEUTHUDC1PO
            {
                IDKH = x.IDKHPO, SODB = x.SODBPO, MADP = x.MADPPO, NAM = x.NAM, THANG = x.THANG,
                SONHA = x.SONHA, TENKH = x.TENKH,  CHISODAU = x.CHISODAU,  CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,  TTHAIGHI = x.TTHAIGHI, MANV_CS = x.MANVN_CS,
                CHISODAUDC=x.CHISODAUDC, CHISOCUOIDC=x.CHISOCUOIDC, KLTIEUTHUDC=x.KLTIEUTHUDC,
                TIENNUOCDC=x.TIENNUOCDC, TONGTIENDC=x.TONGTIENDC, TIENPHIDC=x.TIENPHIDC,
                TIENTHUEDC=x.TIENTHUEDC, MTRUYTHUDC=x.MTRUYTHUDC, GHICHUDC=x.GHICHUDC,MASOHDDC=x.MASOHDDC,
                INHDDC=x.INHDDC
            }).ToList();
            
        }

        public List<TIEUTHUDC1PO> GetDCDotIn(string khuvuc, int thang, int nam, string idmadot)
        {
            var q = _db.TIEUTHUDCPOs
                .Join(_db.KHACHHANGPOs, tt => tt.IDKHPO.Trim(), kh => kh.IDKHPO.Trim(),
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG == thang && tt.tt.NAM == nam && tt.tt.MAKVPO == khuvuc
                    && tt.tt.IDMADOTIN.Equals(idmadot));
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKHPO,
                @res.tt.SODBPO,
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
                @res.tt.CHISODAUDC,
                @res.tt.CHISOCUOIDC,
                @res.tt.KLTIEUTHUDC,
                @res.tt.TIENNUOCDC,
                @res.tt.TONGTIENDC,
                @res.tt.TIENPHIDC,
                @res.tt.TIENTHUEDC,
                @res.tt.MTRUYTHUDC,
                @res.tt.GHICHUDC,
                @res.tt.MASOHDDC,
                @res.tt.INHDDC
            }).AsEnumerable().Select(x => new TIEUTHUDC1PO
            {
                IDKH = x.IDKHPO,
                SODB = x.SODBPO,
                MADP = x.MADPPO,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS,
                CHISODAUDC = x.CHISODAUDC,
                CHISOCUOIDC = x.CHISOCUOIDC,
                KLTIEUTHUDC = x.KLTIEUTHUDC,
                TIENNUOCDC = x.TIENNUOCDC,
                TONGTIENDC = x.TONGTIENDC,
                TIENPHIDC = x.TIENPHIDC,
                TIENTHUEDC = x.TIENTHUEDC,
                MTRUYTHUDC = x.MTRUYTHUDC,
                GHICHUDC = x.GHICHUDC,
                MASOHDDC = x.MASOHDDC,
                INHDDC = x.INHDDC
            }).ToList();

        }

        public List<TIEUTHUDC1PO> GetDC1(int thang, int nam)
        {
            var q = _db.TIEUTHUDCPOs
                .Join(_db.KHACHHANGPOs, tt => tt.IDKHPO.Trim(), kh => kh.IDKHPO.Trim(),
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG == thang && tt.tt.NAM == nam);
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKHPO,
                @res.tt.SODBPO,
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
                @res.tt.CHISODAUDC,
                @res.tt.CHISOCUOIDC,
                @res.tt.KLTIEUTHUDC,
                @res.tt.TIENNUOCDC,
                @res.tt.TONGTIENDC,
                @res.tt.TIENPHIDC,
                @res.tt.TIENTHUEDC,
                @res.tt.MTRUYTHUDC,
                @res.tt.GHICHUDC,
                @res.tt.MASOHDDC,
                @res.tt.INHDDC
            }).AsEnumerable().Select(x => new TIEUTHUDC1PO
            {
                IDKH = x.IDKHPO,
                SODB = x.SODBPO,
                MADP = x.MADPPO,
                NAM = x.NAM,
                THANG = x.THANG,
                SONHA = x.SONHA,
                TENKH = x.TENKH,
                CHISODAU = x.CHISODAU,
                CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,
                TTHAIGHI = x.TTHAIGHI,
                MANV_CS = x.MANVN_CS,
                CHISODAUDC = x.CHISODAUDC,
                CHISOCUOIDC = x.CHISOCUOIDC,
                KLTIEUTHUDC = x.KLTIEUTHUDC,
                TIENNUOCDC = x.TIENNUOCDC,
                TONGTIENDC = x.TONGTIENDC,
                TIENPHIDC = x.TIENPHIDC,
                TIENTHUEDC = x.TIENTHUEDC,
                MTRUYTHUDC = x.MTRUYTHUDC,
                GHICHUDC = x.GHICHUDC,
                MASOHDDC = x.MASOHDDC,
                INHDDC = x.INHDDC
            }).ToList();

        }
       
    }

    public class TIEUTHUDC1PO
    {
        public TIEUTHUDC1PO()
        {

        }

        public TIEUTHUDC1PO(string idkh, string soDb, string madp, int nam, int thang, string sonha, string tenkh,
            decimal? chisodau, decimal? chisocuoi, decimal? mtruythu, decimal kltieuthu,
            string tthaighi, string manv_cs, 
            decimal? chisodaudc, decimal? chisocuoidc, decimal kltieuthudc, 
            decimal tiennuocdc, decimal tongtiendc, decimal tienphidc, decimal tienthuedc,
            decimal? mtruythudc, string ghichudc, string masohddc, string inhddc)
        {
            IDKH = idkh; SODB = soDb; MADP = madp; NAM = nam; THANG = thang;
            SONHA = sonha; TENKH = tenkh;
            CHISODAU = chisodau; CHISOCUOI = chisocuoi;
            MTRUYTHU = mtruythu; KLTIEUTHU = kltieuthu;
            TTHAIGHI = tthaighi; MANV_CS = manv_cs;
            CHISODAUDC = chisodaudc; CHISOCUOIDC = chisocuoidc; KLTIEUTHUDC = kltieuthudc;
            TIENNUOCDC = tiennuocdc; TONGTIENDC = tongtiendc; TIENPHIDC=tienphidc;
            TIENTHUEDC = tienthuedc; MTRUYTHU = mtruythudc; GHICHUDC = ghichudc; MASOHDDC = masohddc;
            INHDDC = inhddc ;
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
        public decimal? CHISODAUDC { get; set; }
        public decimal? CHISOCUOIDC { get; set; }        
        public decimal? KLTIEUTHUDC { get; set; }
        public decimal? TIENNUOCDC { get; set; }
        public decimal? TONGTIENDC { get; set; }
        public decimal? TIENPHIDC { get; set; }
        public decimal? TIENTHUEDC { get; set; }
        public decimal? MTRUYTHUDC { get; set; }
        public string GHICHUDC { get; set; }
        public string MASOHDDC { get; set; }
        public string INHDDC { get; set; }
    }
}
