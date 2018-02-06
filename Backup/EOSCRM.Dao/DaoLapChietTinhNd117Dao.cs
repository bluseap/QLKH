using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class DaoLapChietTinhNd117Dao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        
        public DaoLapChietTinhNd117Dao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public DAOLAP_ND117 Get(int maDon)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADON.Equals(maDon)).SingleOrDefault();
        }

        public List<DAOLAP_ND117> GetList(string maDDK)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(maDDK)).ToList();
        }

        public List<DAOLAP_ND117> GetListNCVC(string maDDK)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(maDDK) &&
                    ( p.LOAICP.Equals("VXT") || p.LOAICP.Equals("VC"))
                
                ).ToList();
        }

        public List<DAOLAP_ND117> GetListVC(string maDDK)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(maDDK) && 
                    p.LOAICP.Equals("VC")          
                ).ToList();
        }

        public DAOLAP_ND117 GetVXT(string maDDK)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(maDDK) &&
                    p.LOAICP.Equals("VXT")
                ).SingleOrDefault();
        }

        public DAOLAP_ND117 GetVC(string maDDK)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(maDDK) &&
                    p.LOAICP.Equals("VC")
                ).SingleOrDefault();
        }

        public List<DAOLAP_ND117> GetListNCVUOT(string maDDK)
        {
            return _db.DAOLAP_ND117s.Where(p => p.MADDK.Equals(maDDK) &&
                    (p.LOAICP.Equals("NC") || p.LOAICP.Equals("TT") || p.LOAICP.Equals("C") ||
                     p.LOAICP.Equals("CPC") || p.LOAICP.Equals("TL") || p.LOAICP.Equals("Z") ||
                     p.LOAICP.Equals("XL") || p.LOAICP.Equals("VXT"))
                ).ToList();
        }

        public Message Insert(DAOLAP_ND117 objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();
                _db.DAOLAP_ND117s.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
            }
            return msg;
        }

        public Message Update(DAOLAP_ND117 objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DONGIACP = objUi.DONGIACP;
                    objDb.DVT = objUi.DVT;
                    objDb.HESOCP = objUi.HESOCP;
                    objDb.LOAICP = objUi.LOAICP;
                    objDb.LOAICV = objUi.LOAICV;
                    objDb.NOIDUNG = objUi.NOIDUNG;
                    objDb.SOLUONG = objUi.SOLUONG;
                    objDb.THANHTIENCP = objUi.THANHTIENCP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đào lấp quyết toán miễn phí ", objUi.CHIETTINH.TENCT);
            }
            return msg;
        }


        public Message Delete(DAOLAP_ND117 objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Đào lấp chiết tính miễn phí ", objUi.CHIETTINH.TENCT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DAOLAP_ND117s.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Đào lấp chiết tính miễn phí ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<DAOLAP_ND117> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Đào lấp chiết tính miễn phí");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách đào chiết tính miễn phí");
            }

            return msg;
        }
    }
}
