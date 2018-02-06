using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class BBNghiemThuDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public BBNghiemThuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public BBNGHIEMTHU Get(string macb)
        {
            return _db.BBNGHIEMTHUs.Where(p => p.MABBNT.Equals(macb)).SingleOrDefault();
        }


        public List<BBNGHIEMTHU> GetList()
        {
            //return _db.BBNGHIEMTHUs.OrderBy(c => c.MABBNT).ToList();

            var nt = from bb in _db.BBNGHIEMTHUs join don in _db.DONDANGKies on bb.MABBNT equals don.MADDK
                     orderby bb.MABBNT
                     select bb;
            return nt.ToList();
        }

        public List<BBNGHIEMTHU> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.BBNGHIEMTHUs.Count();
        }

        public Message Insert(BBNGHIEMTHU objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.BBNGHIEMTHUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "nghiệm thu", objUi.MABBNT);
            }
            return msg;
        }

        public Message Update(BBNGHIEMTHU objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MABBNT);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.MADDK = objUi.MADDK;
                    objDb.MANV1 = objUi.MANV1;
                    objDb.HOTEN1 = objUi.HOTEN1;
                    objDb.MANV2 = objUi.MANV2;
                    objDb.HOTEN2 = objUi.HOTEN2;
                    objDb.MANV3 = objUi.MANV3;
                    objDb.HOTEN3 = objUi.HOTEN3;
                    objDb.CHIEUCAO = objUi.CHIEUCAO;
                    objDb.KHOANGCACH = objUi.KHOANGCACH;
                    objDb.VITRI = objUi.VITRI;
                    objDb.CHINIEMM1 = objUi.CHINIEMM1;
                    objDb.CHINIEMM2 = objUi.CHINIEMM2;
                    objDb.KETLUAN = objUi.KETLUAN;

                    objDb.MADH = objUi.MADH;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nghiệm thu");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nghiệm thu", objUi.MABBNT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "nghiệm thu");
            }
            return msg;
        }

        public bool IsInUse(string macb)
        {
            return _db.BBNGHIEMTHUs.Where(p => p.MABBNT.Equals(macb)).Count() > 0;
        }

        public Message Delete(BBNGHIEMTHU objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MABBNT );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "nghiệm thu", objUi.MABBNT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.BBNGHIEMTHUs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "nghiệm thu");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "nghiệm thu");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<BBNGHIEMTHU> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách nghiệm thu");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "nghiệm thu", failed, "nghiệm thu");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " nghiệm thu");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách nghiệm thu");
            }

            return msg;
        }
    }
}
