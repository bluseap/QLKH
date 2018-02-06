using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public  class CongNoDao
    {
        private readonly EOSCRMDataContext _db;

        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public CongNoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public TIEUTHU Get(string idkh, int thang, int nam)
        {
            return _db.TIEUTHUs.SingleOrDefault(p => p.IDKH.Equals(idkh) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }

        public TIEUTHU Get(string madp, string madb, int thang, int nam)
        {
            return _db.TIEUTHUs.SingleOrDefault(p => p.MADP.Equals(madp) && p.MADB.Equals(madb) && p.THANG.Equals(thang) && p.NAM.Equals(nam));
        }

        public List<TIEUTHU> GetList(int? namCN, int? thangCN, DateTime? ngaynhap, String mahttt, String sophiencn)
        {
            var query = _db.TIEUTHUs.Where(p => p.HETNO == true &&
                (!p.THUTQ.HasValue || p.THUTQ.Value == false));

            if (namCN.HasValue)
                query = query.Where(tt => tt.NGAYCN.HasValue && tt.NGAYCN.Value.Year == namCN.Value);
            if (thangCN.HasValue)
                query = query.Where(tt => tt.NGAYCN.HasValue && tt.NGAYCN.Value.Month == thangCN.Value);

            if (ngaynhap.HasValue)
                query = query.Where(tt => tt.NGAYCN.HasValue && tt.NGAYCN.Value == ngaynhap.Value);

            if (!string.IsNullOrEmpty(mahttt))
                query = query.Where(tt => tt.MAHTTT.Equals(mahttt));

            if (!string.IsNullOrEmpty(sophiencn))
                query = query.Where(tt => tt.SOPHIEUCN.Equals(sophiencn));

            return query.OrderByDescending(tt => tt.NGAYNHAPCN).ToList();
        }

        public List<TIEUTHU> GetReversedList(int namCN, int thangCN, string madp)
        {
            return _db.TIEUTHUs.Where(p => p.CONNO == true && p.HETNO == false &&
                    p.THANG == thangCN && 
                    p.NAM == namCN && 
                    p.MADP == madp &&
                    p.INHD == true).ToList();
        }

        public List<TIEUTHU> GetReversedListForKiemTra(int namCN, int thangCN, string madp)
        {
            return _db.TIEUTHUs.Where(p => p.CONNO_KIEMTRA == true && p.HETNO == false &&
                    p.THANG == thangCN &&
                    p.NAM == namCN &&
                    p.MADP == madp &&
                    p.INHD == true).ToList();
        }

        public List<TIEUTHU> GetMissingListForKiemTra(int namCN, int thangCN, string madp)
        {
            return _db.TIEUTHUs.Where(p => (!p.CONNO_KIEMTRA.HasValue || p.CONNO_KIEMTRA.Value == false) &&
                    p.HETNO == false &&
                    p.THANG == thangCN &&
                    p.NAM == namCN &&
                    p.MADP == madp &&
                    p.INHD == true).ToList();
        }

        public decimal? GetTongTienHetNo(int namCN, int thangCN, string madp)
        {
            try {
                return _db.TIEUTHUs.Where(p => p.HETNO == true &&
                    p.THANG == thangCN &&
                    p.NAM == namCN &&
                    p.MADP == madp &&
                    p.INHD == true).Sum(p => p.TONGTIEN);
            }
            catch {
                return 0;
            }
        }

        public decimal? GetChuanThu(int namCN, int thangCN, string madp)
        {
            try
            {
                return _db.TIEUTHUs.Where(p => p.THANG == thangCN &&
                    p.NAM == namCN &&
                    p.MADP == madp &&
                    p.INHD == true).Sum(p => p.TONGTIEN);
            }
            catch
            {
                return 0;
            }
        }

        public decimal? GetTonBiThieu(int namCN, int thangCN, string madp)
        {
            try
            {
                return _db.TIEUTHUs.Where(p => 
                    (!p.CONNO_KIEMTRA.HasValue || p.CONNO_KIEMTRA.Value == false) &&
                    p.HETNO == false &&
                    p.THANG == thangCN &&
                    p.NAM == namCN &&
                    p.MADP == madp &&
                    p.INHD == true).Sum(p => p.TONGTIEN);
                    //p.INHD == true).Sum(p => p.TONGTIEN_PS);
            }
            catch
            {
                return 0;
            }
        }

        public Message DeleteReversedList(IEnumerable<TIEUTHU> objList)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in objList)
                    obj.CONNO = null;

                _db.SubmitChanges();

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "công nợ ngược");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "công nợ ngược");
            }

            return msg;
        }

        public Message DeleteReversedListForKiemTra(IEnumerable<TIEUTHU> objList)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in objList)
                    obj.CONNO_KIEMTRA = null;

                _db.SubmitChanges();

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "công nợ ngược");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "công nợ ngược");
            }

            return msg;
        }

        public Message UpdateReversedList(string madp, int thang, int nam, string mahttt, DateTime ngaythucn, DateTime ngaynhapcn, string manvcn, string sophieucn)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var query = _db.TIEUTHUs.Where(p => 
                    p.THANG == thang &&
                    p.NAM == nam &&
                    p.MADP == madp &&
                    p.INHD == true &&
                    (!p.CONNO.HasValue || (p.CONNO.HasValue && !p.CONNO.Value)) );

                foreach (var tieuthu in query)
                {
                    tieuthu.THUTQ = false;
                    tieuthu.HETNO = true;
                    tieuthu.NGAYCN = ngaythucn;
                    tieuthu.MAHTTT = mahttt;
                    tieuthu.NGAYNHAPCN = ngaynhapcn;
                    tieuthu.MANVNHAPCN = manvcn;
                    tieuthu.GHICHUCN = "";
                    tieuthu.SOPHIEUCN = String.IsNullOrEmpty(tieuthu.SOPHIEUCN) ? sophieucn : tieuthu.SOPHIEUCN;
                }

                //TODO: có cần thiết xóa conno cho các khách hàng trong danh sách quét ngược hay không

                _db.SubmitChanges();

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "Quét công nợ ngược");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "công nợ ngược");
            }

            return msg;
        }

        public List<TIEUTHU> GetList(int? namCN, int? thangCN, DateTime? ngaynhap, String makh, String tenkh, String sonha, String tendp, String makv, String sophiencn)
        {
            var query = _db.TIEUTHUs.Where(p => p.HETNO == true && 
                (!p.THUTQ.HasValue || p.THUTQ.Value == false));

            if (namCN.HasValue)
                query = query.Where(tt => tt.NGAYCN.HasValue && tt.NGAYCN.Value.Year == namCN.Value);
            if (thangCN.HasValue)
                query = query.Where(tt => tt.NGAYCN.HasValue && tt.NGAYCN.Value.Month == thangCN.Value);

            if (ngaynhap.HasValue)
                query = query.Where(tt => tt.NGAYCN.HasValue && tt.NGAYCN.Value == ngaynhap.Value);

            if (!string.IsNullOrEmpty(makh))
                query = query.Where(tt => tt.SODB.Contains(makh));
            if (!string.IsNullOrEmpty(tenkh))
                query = query.Where(tt => tt.KHACHHANG.TENKH.Contains(tenkh));
            if (!string.IsNullOrEmpty(sonha))
                query = query.Where(tt => tt.KHACHHANG.SONHA.Contains(sonha));
            if (!string.IsNullOrEmpty(tendp))
                query = query.Where(tt => tt.DUONGPHO.TENDP.Contains(tendp));
            if (makv != null && makv != "%")
                query = query.Where(tt => tt.MAKV.Equals(makv));

            if (!string.IsNullOrEmpty(sophiencn))
                query = query.Where(tt => tt.SOPHIEUCN.Equals(sophiencn));

            return query.OrderByDescending(tt=>tt.NGAYNHAPCN).ToList();
        }

        public Message UpdateCongNo(TIEUTHU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.IDKH , objUi.THANG , objUi.NAM);

                if (objDb != null)
                {
                    objDb.TIENCONGNO = objUi.TIENCONGNO;
                    objDb.MANVCN = objUi.MANVCN;
                    objDb.NGAYCN = objUi.NGAYCN;
                    objDb.MANVNHAPCN = objUi.MANVNHAPCN;
                    objDb.NGAYNHAPCN = objUi.NGAYNHAPCN;
                    objDb.GHICHUCN = objUi.GHICHUCN;
                    objDb.SOPHIEUCN = objUi.SOPHIEUCN;
                    objDb.HETNO = objUi.HETNO;
                    objDb.CONNO = objUi.CONNO;
                    objDb.CONNO_KIEMTRA = objUi.CONNO_KIEMTRA;
                    objDb.MAHTTT = objUi.MAHTTT;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "công nợ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_UPDATE_FAILED_EXCEPTION, MessageType.Error, "công nợ của khách hàng", "");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Công nợ", "");
            }
            return msg;
        }

        public Message DeleteCongNo(TIEUTHU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.IDKH, objUi.THANG, objUi.NAM);

                if (objDb != null)
                {
                    //TODO: update all fields

                    objDb.TIENCONGNO = 0;
                    objDb.MANVCN = null;
                    objDb.NGAYCN = null;
                    objDb.MANVNHAPCN = null;
                    objDb.NGAYNHAPCN = null;
                    objDb.GHICHUCN = "";
                    objDb.HETNO = false;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Công nợ");
                }
             
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Công nợ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Công nợ");
            }

            return msg;
        }



        public Message DeleteListCongNo(List<TIEUTHU> objList, PageAction action)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                foreach (var obj in objList)
                {
                    //TODO: check valid update infor
                    DeleteCongNo(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "công nợ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Công nợ ");
            }

            return msg;
        }
    }
}