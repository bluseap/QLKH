using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class KiemDinhDongHoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public KiemDinhDongHoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public KIEMDINHDH Get(int ma)
        {
            return _db.KIEMDINHDHs.Where(p => p.ID.Equals(ma)).SingleOrDefault();
        }
              
        public List<KIEMDINHDH > GetList()
        {
            return _db.KIEMDINHDHs.ToList();
        }

        public List<KIEMDINHDH> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.KIEMDINHDHs.Count();
        }

        public Message Insert(KIEMDINHDH objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.KIEMDINHDHs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Kiểm định đồng hồ ", "");
            }
            return msg;
        }

        public Message Update(KIEMDINHDH  objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.ID   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CAPDOLUONG = objUi.CAPDOLUONG;
                    objDb.CS_SANXUAT = objUi.CS_SANXUAT;
                    objDb.DKMT_NDMT = objUi.DKMT_NDMT;
                    objDb.DKMT_NDNUOC = objUi.DKMT_NDNUOC;
                    objDb.DTKT_DKDD = objUi.DTKT_DKDD;
                    objDb.DTKT_PVLL = objUi.DTKT_PVLL;
                    objDb.DUONGKINH = objUi.DUONGKINH;
                    objDb.KIEU = objUi.KIEU;
                    objDb.KQKD_KTBN = objUi.KQKD_KTBN;
                    objDb.KQKD_KTDK = objUi.KQKD_KTDK;
                    objDb.KQKD_XDSSTD = objUi.KQKD_XDSSTD;
                    if (!string.IsNullOrEmpty(objUi.MALDH))
                        objDb.LOAIDH = _db.LOAIDHs.SingleOrDefault(p => p.MALDH.Equals(objUi.MALDH));
                        //objDb.MALDH = objUi.MALDH;
                    if (!string.IsNullOrEmpty(objUi.MANVKD))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVKD));
                    //objDb.MANVKD = objUi.MANVKD;
                    if (!string.IsNullOrEmpty(objUi.MANVRS))
                        objDb.NHANVIEN1 = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVRS));
                    //objDb.MANVRS = objUi.MANVRS;
                    objDb.NAMSX = objUi.NAMSX;
                    objDb.NGAYKD = objUi.NGAYKD;
                    objDb.NOISUDUNG = objUi.NOISUDUNG;
                    objDb.PPTHUCHIEN = objUi.PPTHUCHIEN;
                    objDb.TBSD = objUi.TBSD;
                    objDb.TRANGTHAI = objUi.TRANGTHAI;
                 
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Kiểm định đồng hồ ", ""   );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Kiểm định đồng hồ ", ""   );
            }
            return msg;
        }

        public bool IsInUse(int ma)
        {
            return _db .KDDHCTs .Where( p=>p.ID .Equals( ma)).Count( ) >0;
        }

        public Message Delete(KIEMDINHDH    objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.ID );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Kiểm định đồng hồ ", "");
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                var kddhctsList = _db.KDDHCTs.Where(p => p.ID.Equals(objDb.ID)).ToList();
                foreach (var kddhct in kddhctsList )
                {
                    _db.KDDHCTs.DeleteOnSubmit(kddhct);
                }
                // Set delete info
                _db.KIEMDINHDHs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Kiểm định đồng hồ ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<KIEMDINHDH> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Kiểm định đồng hồ ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách kiểm định đồng hồ ");
            }

            return msg;
        }
    }
}
