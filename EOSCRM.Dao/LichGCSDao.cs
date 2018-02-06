﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LichGCSDao
    {
        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        //private readonly DuongPhoDao _dpDao = new DuongPhoDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public LichGCSDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public LICHGCS Get(int id)
        {
            return _db.LICHGCS.Where(p => p.IDMADP.Equals(id)).SingleOrDefault();
        }

        public LICHGCS GetListKyDPKV(int nam, int thang, string madp, string makv)
        {
            return _db.LICHGCS.Where(p => p.NAM.Equals(nam) && p.THANG.Equals(thang) && p.MADP.Equals(madp)
                        && p.MAKV.Equals(makv)).SingleOrDefault();
        }

        public List<LICHGCS> GetListKyKV(int nam, int thang, string makv)
        {
            return _db.LICHGCS.Where(p => p.NAM.Equals(nam) && p.THANG.Equals(thang) && p.MAKV.Equals(makv))
                .OrderBy(p => p.IDMADP)
                .ToList();
        }

        public int Count()
        {
            return _db.LICHGCS.Count();
        }

        public Message Insert(LICHGCS objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.LICHGCS.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "lịch ghi");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "lịch ghi", objUi.MADP);
            }
            return msg;
        }

        public Message Update(LICHGCS objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.IDMADP);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENDP = objUi.TENDP;
                    objDb.NGAYGHI1 = objUi.NGAYGHI1;
                    objDb.NGAYGHI2 = objUi.NGAYGHI2;
                    objDb.TUNGAY = objUi.TUNGAY;
                    objDb.DENNGAY = objUi.DENNGAY;
                    objDb.MANVG = objUi.MANVG;
                    objDb.MANVT = objUi.MANVT;
                    objDb.IDMADOTIN = objUi.IDMADOTIN;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.NGAYN = objUi.NGAYN;
                    objDb.MANVN = objUi.MANVN;                    

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADP,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH05.ToString(),
                        MATT = "ULGCS",
                        MOTA = "Cập nhật lịch ghi chỉ số."
                    };

                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "lịch ghi ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "lịch ghi ", objUi.MADP);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "lịch ghi ", objUi.MADP);
            }
            return msg;
        }

        public Message DeleteLichGCS(int idmadp, string madp, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // Get current Item in db
                var objDb = Get(idmadp);

                //update lai dot in duong pho nhu cu
                //_dpDao.UpDotInInLichGCS2(madp, objDb.IDMADOTINCU, useragent, ipAddress, sManv);

                if (objDb == null)
                {                     
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "lịch ghi", madp);
                    return msg;                   
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.LICHGCS.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = madp,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = "XLGCS",
                    MOTA = "Xóa lịch ghi chỉ số."
                };

                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "lịch ghi ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "lịch ghi ");
            }
            return msg;
        }

    }
}
