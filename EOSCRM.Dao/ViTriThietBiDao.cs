using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ViTriThietBiDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public ViTriThietBiDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        public VITRITB GetAp(int mavt)
        {
            return _db.VITRITBs.Where(p => p.IDVT.Equals(mavt) ).SingleOrDefault();
        }

        public List<VITRITB> GetList()
        {
            return _db.VITRITBs.ToList();
        }

        public List<VITRITB> GetListIP(string ipaddress)
        {
            return _db.VITRITBs.Where(p => p.IPADDRESS.Equals(ipaddress)).ToList();
        }

        public List<VITRITB> GetListMAC(string physycal)
        {
            return _db.VITRITBs.Where(p => p.PHYSYCAL.Equals(physycal)).ToList();
        }       

        public int Count( )
        {
            return _db.VITRITBs.Count();
        }

        public Message Insert(VITRITB objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.VITRITBs.InsertOnSubmit(objUi);
                _db.SubmitChanges();
                // commit
                //#region Luu Vet
                //var luuvetKyduyet = new LUUVET_KYDUYET
                //{
                //    MADON = objUi.MAAPTO + objUi.MAXA + objUi.MAKV,
                //    IPAddress = "192.168.1.19",
                //    MANV = "nguyenm",
                //    UserAgent = "192.168.1.119",
                //    NGAYTHUCHIEN = DateTime.Now,
                //    TACVU = TACVUKYDUYET.A.ToString(),
                //    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                //    MATT = "INAXP",
                //    MOTA = "Thêm Ấp khóm"
                //};
                //_kdDao.Insert(luuvetKyduyet);
                //#endregion
                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Vị trí thiết bị. ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                msg = ExceptionHandler.HandleInsertException(ex, "Vị trí thiết bị. ", objUi.IPADDRESS);
            }
            return msg;
        }

        //public Message Update(APTO objUi)
        //{
        //    Message msg;
        //    try
        //    {
        //        // get current object in database
        //        var objDb = Get(objUi.MAAPTO, objUi.MAXA, objUi.MAKV);

        //        if (objDb != null)
        //        {
        //            //TODO: update all fields
        //            objDb.TENAPTO = objUi.TENAPTO;
        //            objDb.STT = objUi.STT;
        //            // Submit changes to db
        //            _db.SubmitChanges();

        //            #region Luu Vet
        //            var luuvetKyduyet = new LUUVET_KYDUYET
        //            {
        //                MADON = objUi.MAAPTO + objUi.MAXA + objUi.MAKV,
        //                IPAddress = "192.168.1.19",
        //                MANV = "nguyenm",
        //                UserAgent = "192.168.1.119",
        //                NGAYTHUCHIEN = DateTime.Now,
        //                TACVU = TACVUKYDUYET.A.ToString(),
        //                MACN = CHUCNANGKYDUYET.KH01.ToString(),
        //                MATT = "UPAXP",
        //                MOTA = "Cập nhật Ấp khóm"
        //            };
        //            _kdDao.Insert(luuvetKyduyet);
        //            #endregion
        //            // success message
        //            msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Ấp, khóm ");
        //        }
        //        else
        //        {
        //            // error message
        //            msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Ấp, khóm ", objUi.TENAPTO);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        msg = ExceptionHandler.HandleUpdateException(ex, "Ấp, khóm ", objUi.TENAPTO);
        //    }
        //    return msg;
        //}

    }
}
