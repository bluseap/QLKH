using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class DanhSachCoQuanThanhToanDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public DanhSachCoQuanThanhToanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public DANHSACHCOQUANTT     Get(string ma)
        {
            return _db.DANHSACHCOQUANTTs.Where(p => p.MACQ .Equals(ma)).SingleOrDefault();
        }

        public List<DANHSACHCOQUANTT> Search(string key)
        {
            return _db.DANHSACHCOQUANTTs.Where(p => p.TENCQ.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<DANHSACHCOQUANTT> GetList()
        {
            return _db.DANHSACHCOQUANTTs.ToList();
        }

        public List<DANHSACHCOQUANTT> GetList(string keyword)
        {
            return _db.DANHSACHCOQUANTTs.Where(c => (c.MACQ.Contains(keyword) ||
                                                        c.TENCQ.Contains(keyword) || 
                                                        c.NGANHANG.TENNH.Contains(keyword) || 
                                                        c.SOTK.Contains(keyword))).ToList();
        }

        public List<DANHSACHCOQUANTT> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.DANHSACHCOQUANTTs.Count();
        }

        public Message Insert(DANHSACHCOQUANTT objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.DANHSACHCOQUANTTs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "cơ quan thanh toán");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "cơ quan thanh toán");
            }
            return msg;
        }

        public Message Update(DANHSACHCOQUANTT objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MACQ       );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.DIACHI  = objUi.DIACHI   ;
                    objDb.CHUONG = objUi.CHUONG  ;
                    objDb.DVQHNS = objUi.DVQHNS;
                    if (!string.IsNullOrEmpty(objUi.MANH))
                        objDb.NGANHANG = _db.NGANHANGs.Single(p=>p.MANH .Equals(  objUi.MANH));
                    //objDb.MANH = objUi.MANH;
                    objDb.MS = objUi.MS;
                    objDb.NDKT = objUi.NDKT;
                    objDb.NKT = objUi.NKT;
                    objDb.NUONNS = objUi.NUONNS;
                    objDb.SODT = objUi.SODT;
                    objDb.SOTK = objUi.SOTK;
                    objDb.TENCQ = objUi.TENCQ;
                        // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "cơ quan thanh toán");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "cơ quan thanh toán");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "cơ quan thanh toán");
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.KHACHHANGs.Where(p => p.MACQ.Equals(ma)).Count() > 0)
                return true;
            else if (_db.TIEUTHUs.Where(p => p.MACQ.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }

        }

        public Message Delete(DANHSACHCOQUANTT objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MACQ     );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Danh sách cơ quan thanh toán", objUi.TENCQ      );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.DANHSACHCOQUANTTs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Danh sách cơ quan thanh toán ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Danh sách cơ quan thanh toán ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<DANHSACHCOQUANTT> objList)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                var failed = 0;

                foreach (var obj in objList)
                {
                    var temp = Delete(obj);
                    if (temp != null && temp.MsgType.Equals(MessageType.Error))
                        failed++;
                }

                // commit
                trans.Commit();

                if (failed > 0)
                {
                    if (failed == objList.Count)
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách cơ quan");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "danh sách cơ quan", failed, "danh sách cơ quan");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " danh sách cơ quan");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách cơ quan");
            }

            return msg;
        }
    }
}
