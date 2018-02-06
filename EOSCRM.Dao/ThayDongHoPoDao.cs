using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ThayDongHoPoDao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ThayDongHoPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THAYDONGHOPO Get(int id)
        {
            return _db.THAYDONGHOPOs.FirstOrDefault(p => p.ID.Equals(id));
        }

        public THAYDONGHOPO GetKy(string idkhpo, int thang, int nam)
        {
            return _db.THAYDONGHOPOs.FirstOrDefault(p => p.IDKHPO.Equals(idkhpo) && p.KYTHAYDH.Value.Month.Equals(thang)
                        && p.KYTHAYDH.Value.Year.Equals(nam));
        }

        public Message Delete(THAYDONGHOPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.ID );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thay đồng hồ", "");
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.THAYDONGHOPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thay đồng hồ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thay đồng hồ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<THAYDONGHOPO > objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thay đồng hồ ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Thay đồng hồ ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <param name="useragent"></param>
        /// <param name="ipAddress"></param>
        /// <returns></returns>
        public Message DoAction(List<THAYDONGHOPO> objList, PageAction action, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                #region Delete action

                if (action.Equals(PageAction.Delete))
                {
                    foreach (var obj in objList)
                    {
                        // Get current Item in db
                        var objDb = Get(obj.ID);
                        if (objDb == null)
                        {
                            // error message
                            msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thay đồng hồ", "");
                            return msg;
                        }
                        // Set delete info
                        _db.THAYDONGHOPOs.DeleteOnSubmit(objDb);

                        var luuvetKyduyet = new LUUVET_KYDUYET
                        {
                            MADON = objDb.IDKHPO,
                            IPAddress = ipAddress,
                            MANV = sManv,
                            UserAgent = useragent,
                            NGAYTHUCHIEN = DateTime.Now,
                            TACVU = TACVUKYDUYET.D.ToString(),
                            MACN = CHUCNANGKYDUYET.KH01.ToString(),
                            MATT = TTDK.DK_A.ToString(),
                            MOTA = "Thay đồng hồ"
                        };
                        _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                    }

                    // Submit changes to db
                    _db.SubmitChanges();

                    // commit
                    trans.Commit();

                    _db.Connection.Close();

                    // success message
                    msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + "record thay đồng hồ");

                    return msg;
                }
                #endregion
              

                // commit
                trans.Commit();

                _db.Connection.Close();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "record thay đồng hồ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                _db.Connection.Close();

                msg = ExceptionHandler.HandleInsertException(ex, "record thay đồng hồ");
            }

            return msg;
        }

        public Message UpThayDongHo(THAYDONGHOPO obj, DateTime ngayht, string tem, string ghichu, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // get current object in database
                var cu = Get(obj.ID);

                if (cu != null)
                {
                    cu.CHISONGUNG = obj.CHISONGUNG;
                    cu.MTRUYTHU = obj.MTRUYTHU;
                    cu.MALDHPO = obj.MALDHPO;
                    cu.MADHPO = obj.MADHPO;
                    cu.CHISOBATDAU = obj.CHISOBATDAU;
                    cu.CHISOMOI = obj.CHISOMOI;
                    cu.NGAYTD = obj.NGAYTD;
                    cu.NGAYHT = obj.NGAYHT;
                    cu.KICHCO = obj.KICHCO;
                    cu.GHICHU = ghichu;
                    cu.DHCAPBAN = obj.DHCAPBAN;

                    cu.LYDOTHAY = obj.LYDOTHAY;

                    _db.SubmitChanges();                 
                    #region Luu Vet
                                        #endregion

                    // success message
                    msg = new Message(MessageConstants.I_THANHCONG, MessageType.Info, "thay đồng hồ khách hàng");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thay đồng hồ", obj.IDKHPO);
                }

                // commit
                trans.Commit();
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleUpdateException(ex, "Thay đồng hồ");
            }

            return msg;
        }
    }
}
