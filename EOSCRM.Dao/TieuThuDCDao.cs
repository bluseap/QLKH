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
    public class TieuThuDCDao
    {

        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public TieuThuDCDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public TIEUTHUDC Get(string ma)
        {
            return _db.TIEUTHUDCs.FirstOrDefault(p => p.IDKH.Equals(ma));
        }

        public TIEUTHUDC GetTN(string ma, int thang, int nam)
        {
            return _db.TIEUTHUDCs.FirstOrDefault(p => p.IDKH.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }

        public Message Insert(TIEUTHUDC objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.TIEUTHUDCs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.IDKH,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.VT00.ToString(),
                    MATT = CHUCNANGKYDUYET.VT00.ToString(),
                    MOTA = "Điều chỉnh hoá đơn."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "điều chỉnh chỉ số");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "vật tư", objUi.IDKH);
            }
            return msg;
        }

        public Message Update(TIEUTHUDC obj)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {

                var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == obj.IDKH &&
                                                               tt.NAM == obj.NAM &&
                                                               tt.THANG == obj.THANG);
                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Tiêu thụ");


                objDb.MADP = obj.MADP;
                objDb.DUONGPHU = obj.DUONGPHU;
                objDb.MADB = obj.MADB;
                objDb.SODB = obj.MADP + obj.DUONGPHU + obj.MADB;
                objDb.MAMDSD = obj.MAMDSD;
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

        public List<TIEUTHUDC1> GetDC(string khuvuc, int thang, int nam)
        {
            var q = _db.TIEUTHUDCs
                .Join(_db.KHACHHANGs, tt => tt.IDKH.Trim(), kh => kh.IDKH.Trim(), 
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG==thang && tt.tt.NAM==nam && tt.tt.MAKV==khuvuc);
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKH, @res.tt.SODB, @res.tt.MADP, @res.tt.NAM, @res.tt.THANG,
                @res.kh.SONHA, @res.kh.TENKH,
                @res.tt.CHISODAU, @res.tt.CHISOCUOI, @res.tt.KLTIEUTHU,
                TTHAIGHI = @res.tt.TTHAIGHI ?? "GDH_BT", @res.tt.MANVN_CS,
                @res.tt.CHISODAUDC,@res.tt.CHISOCUOIDC,@res.tt.KLTIEUTHUDC,
                @res.tt.TIENNUOCDC,@res.tt.TONGTIENDC,@res.tt.TIENPHIDC,
                @res.tt.TIENTHUEDC,@res.tt.MTRUYTHUDC,@res.tt.GHICHUDC,@res.tt.MASOHDDC,
                @res.tt.INHDDC
            }).AsEnumerable().Select(x => new TIEUTHUDC1
            {
                IDKH = x.IDKH, SODB = x.SODB, MADP = x.MADP, NAM = x.NAM, THANG = x.THANG,
                SONHA = x.SONHA, TENKH = x.TENKH,  CHISODAU = x.CHISODAU,  CHISOCUOI = x.CHISOCUOI,
                KLTIEUTHU = x.KLTIEUTHU,  TTHAIGHI = x.TTHAIGHI, MANV_CS = x.MANVN_CS,
                CHISODAUDC=x.CHISODAUDC, CHISOCUOIDC=x.CHISOCUOIDC, KLTIEUTHUDC=x.KLTIEUTHUDC,
                TIENNUOCDC=x.TIENNUOCDC, TONGTIENDC=x.TONGTIENDC, TIENPHIDC=x.TIENPHIDC,
                TIENTHUEDC=x.TIENTHUEDC, MTRUYTHUDC=x.MTRUYTHUDC, GHICHUDC=x.GHICHUDC,MASOHDDC=x.MASOHDDC,
                INHDDC=x.INHDDC
            }).ToList();
            
        }

        public List<TIEUTHUDC1> GetDCDotIn(string khuvuc, int thang, int nam, string iddotin)
        {
            var q = _db.TIEUTHUDCs
                .Join(_db.KHACHHANGs, tt => tt.IDKH.Trim(), kh => kh.IDKH.Trim(),
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG == thang && tt.tt.NAM == nam && tt.tt.MAKV == khuvuc
                    && tt.kh.DUONGPHO.IDMADOTIN.Equals(iddotin));

            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKH,
                @res.tt.SODB,
                @res.tt.MADP,
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
            }).AsEnumerable().Select(x => new TIEUTHUDC1
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

        public List<TIEUTHUDC1> GetDC1(int thang, int nam)
        {
            var q = _db.TIEUTHUDCs
                .Join(_db.KHACHHANGs, tt => tt.IDKH.Trim(), kh => kh.IDKH.Trim(),
                    (tt, kh) => new { tt, kh })
                .Where(tt => tt.tt.THANG == thang && tt.tt.NAM == nam);
            q = q.OrderByDescending(tt => tt.tt.NGAYDCCS);
            return q.Select(@res => new
            {
                @res.kh.IDKH,
                @res.tt.SODB,
                @res.tt.MADP,
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
            }).AsEnumerable().Select(x => new TIEUTHUDC1
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

    public class TIEUTHUDC1
    {
        public TIEUTHUDC1()
        {

        }

        public TIEUTHUDC1(string idkh, string soDb, string madp, int nam, int thang, string sonha, string tenkh,
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
