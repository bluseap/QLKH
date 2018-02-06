using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class PhatSinhTangDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public PhatSinhTangDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public PHATSINHTANG Get(string ma)
        {
            return _db.PHATSINHTANGs.Where(p => p.MAPS.Equals(ma)).SingleOrDefault();
        }

        public PHATSINHTANG Get(string ma, int nam, int thang)
        {
            return _db.PHATSINHTANGs.Where(p => p.MAPS.Equals(ma) && p.NAM.Equals(nam) && p.THANG.Equals(thang)).SingleOrDefault();
        }

        public List<PHATSINHTANG> GetList()
        {
            return _db.PHATSINHTANGs.
                OrderByDescending(p => p.NGAYNHAP).
                OrderByDescending(p => p.THANG).
                OrderByDescending(p => p.NAM).ToList();
        }

        public List<PHATSINHTANG> GetList(int nam, int thang, string sophieucn)
        {
            return _db.PHATSINHTANGs.
                Where(pst => pst.NAM == nam && pst.THANG == thang && pst.SOPHIEUCN == sophieucn && pst.SOPHIEUCN != "").
                OrderByDescending(p => p.NGAYNHAPCN).
                OrderByDescending(p => p.THANG).
                OrderByDescending(p => p.NAM).ToList();
        }

        public List<PHATSINHTANG> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count()
        {
            return _db.PHATSINHTANGs.Count();
        }

        public Message Insert(PHATSINHTANG objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.PHATSINHTANGs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "khách hàng phát sinh tăng");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "khách hàng phát sinh tăng", objUi.MAPS);
            }
            return msg;
        }

        public Message UpdateCongNo(PHATSINHTANG objUi)
        {
            Message msg;
            try
            {
                var pst = Get(objUi.MAPS);

                if (pst != null)
                {
                    pst.MANVNHAPCN = objUi.MANVNHAPCN;
                    pst.NGAYNHAPCN = objUi.NGAYNHAPCN;
                    pst.SOPHIEUCN = objUi.SOPHIEUCN;
                    pst.GHICHUCN = objUi.GHICHUCN;
                    pst.NGAYCN = objUi.NGAYCN;
                    
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "công nợ phát sinh tăng");
                }
                else
                    // error message
                    msg = new Message(MessageConstants.E_UPDATE_FAILED_EXCEPTION, MessageType.Error, "công nợ phát sinh tăng", "");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleUpdateException(ex, "công nợ phát sinh tăng", objUi.MAPS);
            }

            return msg;
        }

        public Message Update(PHATSINHTANG objUi)
        {
            Message msg;
            try
            {
                var pst = Get(objUi.MAPS);

                if (pst != null)
                {
                    pst.THANG = objUi.THANG;
                    pst.NAM = objUi.NAM;
                    pst.CSC = objUi.CSC;
                    pst.CSD = objUi.CSD;
                    pst.TENKH = objUi.TENKH;
                    pst.DIACHI = objUi.DIACHI;
                    pst.DONGIA = objUi.DONGIA;
                    pst.MADP = objUi.MADP;
                    pst.DUONGPHU = objUi.DUONGPHU;
                    pst.ISTHUE = objUi.ISTHUE;
                    pst.MST = objUi.MST;
                    pst.MAKV = objUi.MAKV;
                    pst.MAMDSD = objUi.MAMDSD;
                    pst.KLTIEUTHU = objUi.KLTIEUTHU;
                    pst.TIENNUOC = objUi.TIENNUOC;
                    pst.VAT = objUi.VAT;
                    pst.TIENTHUE = objUi.TIENTHUE;
                    pst.PHI = objUi.PHI;
                    pst.TIENPHI = objUi.TIENPHI;
                    pst.TONGTIEN = objUi.TONGTIEN;
                    pst.LYDO = objUi.LYDO;
                    pst.MANVNHAP = objUi.MANVNHAP;
                    pst.NGAYNHAP = objUi.NGAYNHAP;

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "khách hàng phát sinh tăng");
                }
                else
                    // error message
                    msg = new Message(MessageConstants.E_UPDATE_FAILED_EXCEPTION, MessageType.Error, "khách hàng phát sinh tăng", "");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "khách hàng phát sinh tăng", objUi.MAPS);
            }

            return msg;
        }

        public Message Delete(PHATSINHTANG objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAPS);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Phát sinh tăng ", objUi.MAPS);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.PHATSINHTANGs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Phát sinh tăng ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Phát sinh tăng ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<PHATSINHTANG> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách phát sinh tăng");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "khách hàng phát sinh tăng", failed, "khách hàng phát sinh tăng");
                }

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " khách hàng phát sinh tăng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách phát sinh tăng");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteListCongNo(List<PHATSINHTANG> objList)
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
                    obj.MANVNHAPCN = null;
                    obj.NGAYNHAPCN = null;
                    obj.SOPHIEUCN = null;
                    obj.GHICHUCN = null;
                    obj.NGAYCN = null;

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "công nợ " + objList.Count + " khách hàng phát sinh tăng");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleDeleteException(ex, "công nợ phát sinh tăng");
            }

            return msg;
        }

        public string NewId()
        {
            var sToday = DateTime.Now.ToString("yyMMdd");

            var query = (from p in _db.PHATSINHTANGs.Where(p => p.MAPS.Substring(0, 6).Contains(sToday))
                         select p.MAPS).Max();

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;
                query = temp.ToString();
            }
            else
            {
                query = sToday + "1";
            }

            return query;
        }
    }
}
