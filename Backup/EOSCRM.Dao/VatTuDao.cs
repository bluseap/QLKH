using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public  class VatTuDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public VatTuDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public VATTU  Get(string macb)
        {
            return _db.VATTUs.Where(p => p.MAVT .Equals(macb)).SingleOrDefault();
        }

        public List<VATTU> Search(string key)
        {
            return _db.VATTUs.Where(p =>p.MAVT .ToUpper( ).Contains( key.ToUpper( )) ||  p.TENVT .ToUpper().Contains(key.ToUpper()) || p.MAHIEU.ToUpper( ).Contains( key .ToUpper( )))
                .OrderBy(p => p.MAHIEU).ToList();
        }

        public List<VATTU> GetList()
        {
            return _db.VATTUs.OrderBy(vt => vt.TENVT).ToList();
        }

        public List<VATTU> GetList(String mavt, String mahieu, String tenvt, String madvt, String manhom, decimal? gianc, decimal? giavt)
        {
            var query = _db.VATTUs.AsQueryable();

            if (!String.IsNullOrEmpty(mavt))
                query = query.Where(vt => vt.MAVT.Contains(mavt));
            if (!String.IsNullOrEmpty(mahieu))
                query = query.Where(vt => vt.MAHIEU.Contains(mahieu));
            if (!String.IsNullOrEmpty(tenvt))
                query = query.Where(vt => vt.TENVT.Contains(tenvt));


            if (!String.IsNullOrEmpty(madvt) && madvt != "%")
                query = query.Where(vt => vt.DVT.Equals(madvt));
            if (!String.IsNullOrEmpty(manhom) && manhom != "%")
                query = query.Where(vt => vt.MANHOM.Equals(manhom));

            /*
            if (giavt.HasValue)
                query = query.Where(vt => vt.GIAVT.Equals(giavt.Value));
            if (gianc.HasValue)
                query = query.Where(vt => vt.GIANC.Equals(gianc.Value));
            */

            return query.OrderBy(vt => vt.TENVT).ToList();
        }


        public int Count( )
        {
            return _db.VATTUs.Count();
        }

        public Message Insert(VATTU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                              
             
                _db.VATTUs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit
                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAVT,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.VT00.ToString(),
                    MATT = CHUCNANGKYDUYET.VT00.ToString(),
                    MOTA = "Nhập vật tư mới"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "vật tư");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "vật tư", objUi.TENVT );
            }
            return msg;
        }

        public Message Update(VATTU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MAVT);

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.TENVT  = objUi.TENVT;
                    if (!string.IsNullOrEmpty(objUi.DVT))
                        objDb.DVT1 = _db.DVTs.Single(p => p.DVT1.Equals(objUi.DVT));
                    //objDb.DVT = objUi.DVT;
                    objDb.GIANC = objUi.GIANC;
                    objDb.GIAVT = objUi.GIAVT;
                    objDb.HAYDUNG = objUi.HAYDUNG;
                    objDb.MAHIEU = objUi.MAHIEU;
                    if (!string.IsNullOrEmpty(objUi.MANHOM))
                        objDb.NHOMVATTU = _db.NHOMVATTUs.Single(p => p.MANHOM.Equals(objUi.MANHOM));
                    //objDb.MANHOM = objUi.MANHOM;
                    objDb.NGAYAD = objUi.NGAYAD;
                    if (!string.IsNullOrEmpty(objUi.MANVN))
                        objDb.NHANVIEN = _db.NHANVIENs.Single(p => p.MANV.Equals(objUi.MANVN));
                    //objDb.MANVN = objUi.MANVN;
                    objDb.TENVT = objUi.TENVT;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MAVT,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.VT00.ToString(),
                        MATT = CHUCNANGKYDUYET.VT00.ToString(),
                        MOTA = "Cập nhật vật tư mới"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "vật tư");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "vật tư");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "vật tư");
            }
            return msg;
        }

        public bool IsInUse(string macb)
        {
            if( _db.CTCHIETTINHs.Where(p => p.MAVT .Equals(macb)).Count() > 0)
                return true ;
            else if (_db.CTTHIETKEs .Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.CTCHIETTINH_ND117s.Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.CTCHIETTINHSUACHUAs.Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.CTMAUBOCVATTUs.Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.CTQUYETTOAN_ND117s.Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.CTQUYETTOANs.Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.CTQUYETTOANSUACHUAs.Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else if (_db.THICONGs .Where(p => p.MAVT.Equals(macb)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(VATTU objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MAVT  );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Vật tư ", objUi.TENVT );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.VATTUs .DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MAVT,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.VT00.ToString(),
                    MATT = CHUCNANGKYDUYET.VT00.ToString(),
                    MOTA = "Cập nhật vật tư mới"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Vật tư ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Vật tư ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="useragent"></param>
        /// <param name="ipAddress"></param>
        /// <param name="sManv"></param>
        /// <returns></returns>
        public Message DeleteList(List<VATTU> objList, String useragent, String ipAddress, String sManv)
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
                    Delete(obj,  useragent,  ipAddress,  sManv);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "danh sách vật tư");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách vật tư");
            }

            return msg;
        }
    }
}
