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
    public class TieuThuPoDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];


        public TieuThuPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public TIEUTHUPO Get(string ma)
        {
            return _db.TIEUTHUPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma));
        }

        public TIEUTHUPO GetTN(string ma, int thang, int nam)
        {
            return _db.TIEUTHUPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }


        public Message Update(TIEUTHUPO obj)
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

        public List<TIEUTHUPO> SearchTieuThu(int thang, int nam, String makv, String madp, String makh, String tenkh,int loTrinhDau, int loTrinhCuoi)
        {
            var danhsach = from tieuthu in _db.TIEUTHUPOs
                           where (tieuthu.THANG.Equals(thang) && tieuthu.NAM.Equals(nam) && tieuthu.INHD== true )
                           select tieuthu;

            if (loTrinhDau > 0)
                danhsach = danhsach.Where(p => p.KHACHHANGPO.STT >= loTrinhDau);

            if (loTrinhCuoi != 999999)
                danhsach = danhsach.Where(p => p.KHACHHANGPO.STT <= loTrinhDau);

            if (! string.IsNullOrEmpty(makv))
                danhsach = danhsach.Where(p => p.MAKVPO.Equals(makv));
            
            if (! string.IsNullOrEmpty(madp))
                danhsach = danhsach.Where(p => p.MADPPO.Equals(madp));

            if (! string.IsNullOrEmpty(makh))
                danhsach = danhsach.Where(p => p.MADPPO + p.DUONGPHUPO + p.MADBPO == makh || p.MADPPO + p.MADBPO == makh);

            if (!string.IsNullOrEmpty(tenkh))
                danhsach = danhsach.Where(p => p.KHACHHANGPO.TENKH.Contains(tenkh));


            return danhsach.OrderBy(tt => tt.DUONGPHUPO).OrderBy(tt => tt.MADPPO).OrderBy(tt => tt.KHACHHANGPO.STT).ToList();

        }

        public TIEUTHUPO GetTNKyTruoc(string ma, int thang, int nam)
        {
            return _db.TIEUTHUPOs.FirstOrDefault(p => p.IDKHPO.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }

    }
}
