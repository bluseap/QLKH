using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ThuTaiQuayDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ThuTaiQuayDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THUTAIQUAY Get(int id)
        {
            return
              _db.THUTAIQUAYs.Where(p => p.ID .Equals(id )).SingleOrDefault();
        }
     
        public List<THUTAIQUAY> GetList(string idkh, int thang, int nam)
        {
            return _db.THUTAIQUAYs.Where(p => p.IDKH.Equals(idkh) && p.THANG.Equals(thang) && p.NAM.Equals(nam)).ToList();
        }       

        /// <summary>
        /// Thêm mới thu tại quầy
        /// </summary>
        /// <param name="objUi"></param>
        /// <returns></returns>
        public Message Insert(THUTAIQUAY objUi)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // Set trạng thái thu tại quầy cho đối tượng tiêu thụ
                TIEUTHU tieuthu =
                    _db.TIEUTHUs.SingleOrDefault(
                        p => p.IDKH.Equals(objUi.IDKH) && p.THANG.Equals(objUi.THANG) && p.NAM.Equals(objUi.NAM));

                tieuthu.THUTQ = true;

                var listthutaiquay = GetList(objUi.IDKH, objUi.THANG, objUi.NAM);
                var sotientruocdo = listthutaiquay.Sum(p => p.SOTIEN);

                tieuthu.HETNO = tieuthu.TONGTIEN <= objUi.SOTIEN + sotientruocdo;

                //Tinh ra số tiền nước và số tiền phi
                
                decimal? phantram = tieuthu.TIENNUOC /tieuthu.TONGTIEN;

                if(phantram == null)
                {
                    objUi.TIENNUOC = 0;
                    objUi.PHI = 0;
                }else
                {
                    var tiennuoc = (decimal)(objUi.SOTIEN * phantram);
                    objUi.TIENNUOC = tiennuoc;
                    objUi.TIENNUOC = Math.Round(tiennuoc, 4, MidpointRounding.AwayFromZero);
                    objUi.PHI = objUi.SOTIEN - tiennuoc;
                }


                _db.THUTAIQUAYs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                trans.Commit();


                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Thu tại quầy ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Thu tại quầy ", objUi.TIEUTHU.KHACHHANG.TENKH);
            }
            return msg;
        }
  
        public Message Delete(THUTAIQUAY objUi)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // Get current Item in db
                var objDb = Get(objUi.ID );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thu tại quầy ", objUi.TIEUTHU.KHACHHANG.TENKH);
                    return msg;
                }

                TIEUTHU tieuthu =
                  _db.TIEUTHUs.SingleOrDefault(
                      p => p.IDKH.Equals(objUi.IDKH) && p.THANG.Equals(objUi.THANG) && p.NAM.Equals(objUi.NAM));
                               

                var listthutaiquay = GetList(objUi.IDKH, objUi.THANG, objUi.NAM);
                if (listthutaiquay.Count <= 1)
                {
                    tieuthu.THUTQ = false;
                    tieuthu.HETNO = false;
                }else
                {
                    var sotientruocdo = listthutaiquay.Sum(p => p.SOTIEN);
                    if (sotientruocdo - objDb.SOTIEN < tieuthu.TONGTIEN)
                            tieuthu.HETNO = false;
                   
                }

                // Set delete info
                _db.THUTAIQUAYs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thu tại quầy ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleDeleteException(ex, "Thu tại quầy ");
            }

            return msg;
        }             

        public List<THUTAIQUAY> GetList(string madp, string duongphu, DateTime? tungay, DateTime? denngay , int thang, int nam)
        {          
            var dsthutaiquayList = from thutaiquay in _db.THUTAIQUAYs
                                   where (thutaiquay.THANG.Equals(thang) && thutaiquay.NAM.Equals(nam))
                                   select thutaiquay;
            if(!string.IsNullOrEmpty(madp))
            {
                dsthutaiquayList =
                    dsthutaiquayList.Where(d => d.TIEUTHU.MADP.Equals(madp) && d.TIEUTHU.DUONGPHU.Equals(duongphu));
            }

            if (tungay.HasValue)
                dsthutaiquayList = dsthutaiquayList.Where(d => d.NGAYNHAP >= tungay);
            if (denngay.HasValue)
                dsthutaiquayList = dsthutaiquayList.Where(d => d.NGAYNHAP <= denngay);

            return dsthutaiquayList.ToList();
        }

    }
}
