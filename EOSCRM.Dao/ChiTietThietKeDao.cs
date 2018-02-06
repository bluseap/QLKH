using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ChiTietThietKeDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ChiTietThietKeDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public CTTHIETKE Get(string maDon, string maVatTu)
        {
            return _db.CTTHIETKEs.Where(p => p.MADDK.Equals(maDon) && p.MAVT.Equals(maVatTu)).SingleOrDefault();
        }

        public CTTHIETKE GetNhomVT(string maDon, string maNhom)
        {
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT
                        join nvt in _db.NHOMVATTUs on vt.MANHOM equals nvt.MANHOM
                        where (tk.MADDK.Equals(maDon) && vt.MANHOM.Equals(maNhom))
                        select tk;

            return query.SingleOrDefault();
        }

        public CTTHIETKE GetVatTuVuot(string maDon)
        {
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT                       
                        where tk.MADDK.Equals(maDon) && vt.LOAIVT.Equals("VUOT")
                        select tk;

            return query.SingleOrDefault();
        }

        public decimal? TTNhanCong(string madon)
        {
            var query = _db.CTTHIETKEs.Where(p => p.MADDK.Equals(madon)).Sum(p => p.TIENNC);

            if (query == null)
                return 0;

            return query.Value;
        }

        public decimal? TTVatTu(string madon)
        {
            var query = _db.CTTHIETKEs.Where(p => p.MADDK.Equals(madon)).Sum(p => p.TIENVT);

            if (query == null)
                return 0;

            return query.Value;
        }

        public List<CTTHIETKE> GetList(string maDon)
        {
            //return _db.CTTHIETKEs.Where( p=>p.MADDK .Equals( maDon)).ToList();
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT
                        orderby Convert.ToInt32(vt.MAHIEU)
                        where (tk.MADDK.Equals(maDon))
                        select tk;
            return query.ToList();
        }

        public List<CTTHIETKE> GetListCTyDauTu(string maDon)
        {
            //return _db.CTTHIETKEs.Where( p=>p.MADDK .Equals( maDon)).ToList();
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT
                        orderby Convert.ToInt32(vt.MAHIEU)
                        where (tk.MADDK.Equals(maDon) && tk.ISCTYDTU.Equals(1))
                        select tk;
            return query.ToList();
        }

        public List<CTTHIETKE> GetListKHTT(string maDon)
        {
            //return _db.CTTHIETKEs.Where( p=>p.MADDK .Equals( maDon)).ToList();
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT
                        orderby vt.MAHIEU
                        where (tk.MADDK.Equals(maDon) && tk.ISCTYDTU.Equals(0) && vt.LOAIVT == null)
                        select tk;
            return query.ToList();
        }

        public List<CTTHIETKE> GetListLoaiVTKHTT(string maDon)
        {
            //return _db.CTTHIETKEs.Where( p=>p.MADDK .Equals( maDon)).ToList();
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT
                        orderby vt.MAHIEU
                        where (tk.MADDK.Equals(maDon) && tk.ISCTYDTU.Equals(0) && vt.LOAIVT == "KHTT")
                        select tk;
            return query.ToList();
        }

        public List<CTTHIETKE> GetListKHTTVuotOng(string maDon)
        {
            //return _db.CTTHIETKEs.Where( p=>p.MADDK .Equals( maDon)).ToList();
            var query = from tk in _db.CTTHIETKEs
                        join vt in _db.VATTUs on tk.MAVT equals vt.MAVT
                        orderby vt.MAHIEU
                        where (tk.MADDK.Equals(maDon) && tk.ISCTYDTU.Equals(0) && vt.LOAIVT.Equals("VUOT"))
                        select tk;
            return query.ToList();
        }
    
        public Message Insert(CTTHIETKE objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.CTTHIETKEs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Chi tiết thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Chi tiết thiết kế ", objUi.THIETKE .TENTK);
            }
            return msg;
        }

        public Message Update(CTTHIETKE objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDK , objUi .MAVT );

                if (objDb != null)
                {
                    objDb.NOIDUNG = objUi.NOIDUNG;
                    objDb.SOLUONG = objUi.SOLUONG;
                    objDb.GIANC = objUi.GIANC;
                    objDb.TIENNC = objUi.TIENNC;
                    objDb.GIAVT = objUi.GIAVT;
                    objDb.TIENVT = objUi.TIENVT;
                    objDb.ISCTYDTU = objUi.ISCTYDTU;
                  
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "chi tiết thiết kế");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "chi tiết thiết kế", objUi.THIETKE  .TENTK  );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Chi tiết thiết kế", objUi.THIETKE.TENTK);
            }
            return msg;
        }


        public Message Delete(CTTHIETKE objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDK , objUi .MAVT  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Chi tiết thiết kế", objUi.THIETKE.TENTK);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.CTTHIETKEs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết thiết kế");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Chi tiết thiết kế");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<CTTHIETKE> objList, PageAction action)
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
                    Delete(obj);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Chi tiết thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách chi tiết thiết kế ");
            }

            return msg;
        }
    }
}
