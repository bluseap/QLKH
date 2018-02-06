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
        private readonly ReportClass _rpClass = new ReportClass();
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

        public TIEUTHU GetTNKV(string ma, int thang, int nam, string makv)
        {
            return _db.TIEUTHUs.FirstOrDefault(p => p.IDKH.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam)
                    && p.MAKV.Equals(makv));
        }

        public TIEUTHU GetTNKyTruoc(string ma, int thang, int nam)
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

                //commit:
                //// commit
                //trans.Commit();
                //_db.Connection.Close();
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

        public Message UpdateGhiChuLX(string idkh, int thang, int nam, string manv, string ghichukd, string ghichucs, 
            string lydo, string lydo2, string lydo3)
        {
            Message msg;
            DbTransaction trans = null;
            try
            {
                var objDb = _db.TIEUTHUs.SingleOrDefault(tt => tt.IDKH == idkh &&
                                                               tt.NAM == nam &&
                                                               tt.THANG == thang);
                if (objDb == null)
                    return new Message(MessageConstants.E_FAILED, MessageType.Error, "Tiêu thụ");

                objDb.GHICHUKDLX = ghichukd;
                objDb.MANVGCKDLX = manv;
                objDb.NGAYGCKDLX = DateTime.Now;

                objDb.GHICHUCSLX = ghichucs;
                objDb.MANVGCCSLX = manv;
                objDb.NGAYGCCSLX = DateTime.Now;

                _db.SubmitChanges();

                //_rpClass.HisTieuThuBien(idkh, "", thang, nam, "", DateTime.Now, DateTime.Now, DateTime.Now, "", "",
                //    "INTIEUTHUGC");

                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "ghi chú tiêu thụ");
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

        public Message Delete(TIEUTHU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = GetTNKV(objUi.IDKH, objUi.THANG, objUi.NAM, objUi.MAKV);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chi tiết thiết kế", objUi.KHACHHANG.TENKH);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.TIEUTHUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Tiêu thụ khách hàng");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Tiêu thụ khách hàng");
            }

            return msg;
        }

    }
}