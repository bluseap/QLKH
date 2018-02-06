﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class GhiChuQuyetToanDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public GhiChuQuyetToanDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public GHICHUQUYETOAN Get(int ma)
        {
            return _db.GHICHUQUYETOANs.Where(p => p.MAGC.Equals(ma)).SingleOrDefault();
        }

        public List<GHICHUQUYETOAN> GetList(string maDDK)
        {
            return _db.GHICHUQUYETOANs.Where(p => p.MADDK.Equals(maDDK)).ToList();
        }


        public Message Insert(GHICHUQUYETOAN objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.GHICHUQUYETOANs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Ghi chú chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Ghi chú chiết tính", objUi.QUYETTOAN.TENCT);
            }
            return msg;
        }

        public Message Update(GHICHUQUYETOAN objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAGC);

                if (objDb != null)
                {
                    //TODO: update all fields

                    objDb.MADDK = objUi.MADDK;
                    objDb.NOIDUNG = objUi.NOIDUNG;
                  
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Ghi chú chiết tính");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Ghi chú chiết tính", objUi.QUYETTOAN.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Ghi chú chiết tính", objUi.QUYETTOAN.TENCT);
            }
            return msg;
        }


        public Message Delete(GHICHUQUYETOAN objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAGC);

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Ghi chú chiết tính", objUi.QUYETTOAN.TENCT);
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.GHICHUQUYETOANs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Ghi chú chiết tính");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Ghi chú thiết kế");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<GHICHUQUYETOAN> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Ghi chú chiết tính");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách ghi chú chiết tính");
            }

            return msg;
        }
    }
}
