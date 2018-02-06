using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class HoaDonLapDatDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public HoaDonLapDatDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public HOADONLAPDAT   Get(int ma)
        {
            return _db.HOADONLAPDATs.Where(p => p.SOHD.Equals(ma)).SingleOrDefault();
        }

        public List<HOADONLAPDAT> GetList()
        {
            return _db.HOADONLAPDATs.ToList();
        }

        public List<HOADONLAPDAT> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.HOADONLAPDATs.Count();
        }

        public Message Insert(HOADONLAPDAT objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.HOADONLAPDATs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Hóa đơn lắp đặt");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Hóa đơn lắp đặt ", objUi.DONDANGKY.TENKH      );
            }
            return msg;
        }

        public Message Update(HOADONLAPDAT  objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.SOHD );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.ISQUYETTOAN    = objUi.ISQUYETTOAN      ;
                    objDb.KYHIEU = objUi.KYHIEU   ;
                    objDb.LOAI= objUi.LOAI   ;
                    if (!string.IsNullOrEmpty(objUi.MAHTTT))
                        objDb.HTTHANHTOAN = _db.HTTHANHTOANs.Single(p => p.MAHTTT.Equals(objUi.MAHTTT));
                    //objDb.MAHTTT = objUi.MAHTTT;
                    objDb.MAKH = objUi.MAKH ;
                    objDb.MST = objUi.MST;
                    objDb.NGAYHD = objUi.NGAYHD;
                    objDb.SODBCU  = objUi.SODBCU  ;
                    objDb.SOHD = objUi.SOHD  ;
                    objDb.TENHANG = objUi.TENHANG;
                    objDb.TENKH = objUi.TENKH;
                    objDb.THUE = objUi.THUE;
                    objDb.TIENVAT = objUi.TIENVAT;
                    objDb.TIENVAT_ND117 = objUi.TIENVAT_ND117;
                    objDb.TONGCONG = objUi.TONGCONG;
                    objDb.TONGCONG_ND117 = objUi.TONGCONG_ND117;
                    objDb.TONGTIEN = objUi.TONGTIEN;
                    objDb.TONGTIEN_ND117 = objUi.TONGTIEN_ND117;
                    objDb.TRANGTHAI = objUi.TRANGTHAI;
                 
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Hóa đơn lắp đặt ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Hóa đơn lắp đặt ", objUi.DONDANGKY .TENKH );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Hóa đơn lắp đặt ", objUi.DONDANGKY .TENKH );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return false;
        }

        public Message Delete(HOADONLAPDAT  objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.SOHD );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Hóa đơn lắp đặt ", objUi.DONDANGKY .TENKH );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.HOADONLAPDATs .DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hóa đơn lắp đặt ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Hóa đơn lắp đặt ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<HOADONLAPDAT> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Hóa đơn lắp đặt ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách hóa đơn lắp đặt ");
            }

            return msg;
        }
 }
}
