using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class ThuTienCoQuanDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ThuTienCoQuanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THUTIEN_CQ     Get(string ma, int thang , int nam)
        {
            return
                _db.THUTIEN_CQs.Where(p => p.MACQ.Equals(ma) && p.THANG.Equals(thang) && p.NAM.Equals(nam)).
                    SingleOrDefault();
        }

        public List<THUTIEN_CQ> GetList()
        {
            return _db.THUTIEN_CQs.ToList();
        }

        public List<THUTIEN_CQ> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.THUTIEN_CQs.Count();
        }

        public Message Insert(THUTIEN_CQ objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.THUTIEN_CQs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Thu tiền cơ quan thanh toán hộ ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Thu tiền cơ quan thanh toán hộ ", objUi.MACQ );
            }
            return msg;
        }

        public Message Update(THUTIEN_CQ  objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MACQ , objUi .THANG , objUi .NAM );

                if (objDb != null)
                {
                    //TODO: update all fields
                    if (!string.IsNullOrEmpty(objUi.MANVNHAP))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVNHAP));
                    //objDb.MANVNHAP = objUi.MANVNHAP;
                    objDb.NGAYNHAP = objUi.NGAYNHAP;
                       
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Đồng hồ ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Đồng hồ ", objUi.DANHSACHCOQUANTT .TENCQ     );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Đồng hồ ", objUi.DANHSACHCOQUANTT.TENCQ);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {

            return false;

        }

        public Message Delete(THUTIEN_CQ objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MACQ , objUi .THANG , objUi .NAM );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thu tiền cơ quan ", objUi.MACQ );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.THUTIEN_CQs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thu tiền cơ quan ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thu tiền cơ quan ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<THUTIEN_CQ> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thu tiền cơ quan ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách thu tiền cơ quan ");
            }

            return msg;
        }

    }
}
