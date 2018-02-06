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
    public  class TieuThuDao
    {
        
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];


        public TieuThuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        
        public TIEUTHU Get(string ma)
        {
            return _db.TIEUTHUs.FirstOrDefault(p => p.IDKH.Equals(ma));
        }

        public TIEUTHU GetTN(string ma, int thang, int nam)
        {
            return _db.TIEUTHUs.FirstOrDefault(p => p.IDKH.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }


        public Message Update(TIEUTHU obj)
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

        public List<TIEUTHU> SearchTieuThu(int thang, int nam, String makv, String madp, String makh, String tenkh,int loTrinhDau, int loTrinhCuoi)
        {
            var danhsach = from tieuthu in _db.TIEUTHUs
                           where (tieuthu.THANG.Equals(thang) && tieuthu.NAM.Equals(nam) && tieuthu.INHD== true )
                           select tieuthu;

            if (loTrinhDau > 0)
                danhsach = danhsach.Where(p => p.KHACHHANG.STT >= loTrinhDau);

            if (loTrinhCuoi != 999999)
                danhsach = danhsach.Where(p => p.KHACHHANG.STT <= loTrinhDau);

            if (! string.IsNullOrEmpty(makv))
                danhsach = danhsach.Where(p => p.MAKV.Equals(makv));
            
            if (! string.IsNullOrEmpty(madp))
                danhsach = danhsach.Where(p => p.MADP.Equals(madp));

            if (! string.IsNullOrEmpty(makh))
                danhsach = danhsach.Where(p => p.MADP + p.DUONGPHU + p.MADB == makh || p.MADP + p.MADB == makh);

            if (!string.IsNullOrEmpty(tenkh))
                danhsach = danhsach.Where(p => p.KHACHHANG.TENKH.Contains(tenkh));


            return danhsach.OrderBy(tt => tt.DUONGPHU).OrderBy(tt => tt.MADP).OrderBy(tt => tt.KHACHHANG.STT).ToList();

        }
    }
}