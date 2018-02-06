using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class QuyetToanSuaChuaDao
    {
        private readonly EOSCRMDataContext _db;
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public QuyetToanSuaChuaDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public QUYETTOANSUACHUA   Get(string ma)
        {
            return _db.QUYETTOANSUACHUAs.Where(p => p.MADON.Equals(ma)).SingleOrDefault();
        }

        public List<QUYETTOANSUACHUA> GetList()
        {
            return _db.QUYETTOANSUACHUAs.ToList();
        }

        public List<QUYETTOANSUACHUA> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }
        public List<QUYETTOANSUACHUA> GetListForChoLapQuyetToan(String KeyWord, DateTime? TuNgay, DateTime? DenNgay)
        {
            var result = _db.QUYETTOANSUACHUAs.Where(d => d.NGAYLQT == null);
            if(TuNgay!=null)
                result=result.Where(d=>d.NGAYLCT>=TuNgay);
            if(DenNgay!=null)
                result=result.Where(d=>d.NGAYLCT<=DenNgay);
            if(KeyWord!=null)
            {
                result = result.Where(d => d.MADON.Contains(KeyWord) ||
                d.TENCT.Contains(KeyWord) ||
                d.TENHM.Contains(KeyWord) ||
                d.DIACHIHM.Contains(KeyWord));
            }
            return result.ToList();
        }
        public int Count( )
        {
            return _db.QUYETTOANSUACHUAs.Count();
        }

        public Message CopyTuChietTinhVatTuSuaChua(QUYETTOANSUACHUA objUi, CHIETTINHSUACHUA chiettinh)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON );
                              

                if (objDb != null)
                {
                    #region Chiet tiet vat tu mien phi

                    //xóa bỏ chi tiết vật tư hiện tại
                    var ctvattuslist = _db.CTQUYETTOANSUACHUAs.Where(p => p.MADON.Equals(objDb.MADON)).ToList();
                    foreach (var ctvattu in ctvattuslist)
                    {
                        _db.CTQUYETTOANSUACHUAs.DeleteOnSubmit(ctvattu);
                    }
                    //Lay danh sách 
                    var ctmaubocvattuslist = _db.CTCHIETTINHSUACHUAs.Where(p => p.MADON.Equals(objDb.MADON)).ToList();

                    foreach (var ctmaubocvattu in ctmaubocvattuslist)
                    {
                        var ctvattu = new CTQUYETTOANSUACHUA
                                          {
                                              MADON  = objDb.MADON,
                                              MAVT = ctmaubocvattu.MAVT,
                                              SOLUONG = ctmaubocvattu.SOLUONG ,
                                              GIANC = ctmaubocvattu.GIANC ,
                                              GIAVT = ctmaubocvattu .GIAVT ,
                                              TIENNC = ctmaubocvattu .TIENNC ,
                                              TIENVT = ctmaubocvattu .TIENVT 
                                          };
                        _db.CTQUYETTOANSUACHUAs.InsertOnSubmit(ctvattu);
                    }
                    #endregion

                    #region Chiet tiet dao lap mien phi

                    //xóa bỏ chi tiết vật tư hiện tại
                    var ctDaoLapSlist = _db.DAOLAPSUACHUAQTs .Where(p => p.MASC.Equals(objDb.MADON )).ToList();
                    foreach (var ctvattu in ctDaoLapSlist)
                    {
                        _db.DAOLAPSUACHUAQTs.DeleteOnSubmit(ctvattu);
                    }
                    //Lay danh sách 
                    var ctmauDaoLapSlist = _db.DAOLAPSUACHUAs.Where(p => p.MADON.Equals(objDb.MADON)).ToList();

                    foreach (var ctmaubocvattu in ctmauDaoLapSlist)
                    {
                        var ctvattu = new DAOLAPSUACHUAQT
                        {
                            MASC  = objDb.MADON ,
                            SOLUONG = ctmaubocvattu.SOLUONG,
                            DONGIACP = ctmaubocvattu .DONGIACP ,
                            DVT = ctmaubocvattu .DVT ,
                            HESOCP = ctmaubocvattu .HESOCP ,
                            LOAI = ctmaubocvattu .LOAI ,
                            LOAICP = ctmaubocvattu .LOAICP ,
                            LOAICT = ctmaubocvattu .LOAICT ,
                            LOAICV = ctmaubocvattu .LOAICV ,
                            THANHTIENCP = ctmaubocvattu .THANHTIENCP 

                        };
                        _db.DAOLAPSUACHUAQTs.InsertOnSubmit(ctvattu);
                    }
                    #endregion

                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Quyết toán sữa chữa ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Quyết toán sữa chữa ", objUi .TENCT );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Quyết toán sữa chữa ", objUi.TENCT);
            }
            return msg;
        }

        public Message Insert(QUYETTOANSUACHUA  objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.QUYETTOANSUACHUAs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Quyết toán sữa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "Quyết toán sữa chữa ", objUi .TENCT );
            }
            return msg;
        }

        public Message Update(QUYETTOANSUACHUA objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADON   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CONGVIEC = objUi.CONGVIEC;
                    objDb.CPCHUNG = objUi.CPCHUNG;
                    objDb.CPKHAC = objUi.CPKHAC;
                    objDb.CPNHANCONG = objUi.CPNHANCONG;
                    objDb.CPTHIETKE = objUi.CPTHIETKE;
                    objDb.CPTHUNHAP = objUi.CPTHUNHAP;
                    objDb.CPVATLIEU = objUi.CPVATLIEU;
                    objDb.DIACHIHM = objUi.DIACHIHM;
                    objDb.GHICHU = objUi.GHICHU;
                    objDb.HSCHUNG = objUi.HSCHUNG;
                    objDb.HSNHANCONG = objUi.HSNHANCONG;
                    objDb.HSTHIETKE1 = objUi.HSTHIETKE1;
                    objDb.HSTHIETKE2 = objUi.HSTHIETKE2;
                    objDb.HSTHIETKE3 = objUi.HSTHIETKE3;
                    objDb.HSTHUE = objUi.HSTHUE;
                    objDb.HSTHUNHAP = objUi.HSTHUNHAP;
                    objDb.ISSTK = objUi.ISSTK;
                    objDb.LOAICT = objUi.LOAICT;
                    objDb.MANVLCT = objUi.MANVLCT;
                    objDb.NGAYGUI_CN = objUi.NGAYGUI_CN;
                    objDb.NGAYLCT = objUi.NGAYLCT;
                    objDb.NGAYNHAN_CN = objUi.NGAYNHAN_CN;
                    objDb.SOCT = objUi.SOCT;
                    objDb.TENCT = objUi.TENCT;
                    objDb.TENHM = objUi.TENHM;
                    objDb.TIENTHUE = objUi.TIENTHUE;
                    objDb.TONG_ST = objUi.TONG_ST;
                    objDb.TONG_TT = objUi.TONG_TT;
                    objDb.NGAYLQT = objUi.NGAYLQT;
                    // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Quyết toán sữa chữa ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Quyết toán sữa chữa ", objUi.TENCT);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Quyết toán sữa chữa ", objUi.TENCT);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if (_db.CTQUYETTOANSUACHUAs.Where(p => p.MADON.Equals(ma)).Count() > 0)
                return true;
            else if (_db.DAOLAPSUACHUAQTs.Where(p => p.MASC.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(QUYETTOANSUACHUA objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADON );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Quyết toán sữa chữa ", objUi.TENCT);
                    return msg;
                }

                var ctvattuslist = _db.CTQUYETTOANSUACHUAs.Where(p => p.MADON .Equals(objDb)).ToList();
                foreach (var ctvattu in ctvattuslist)
                {
                    _db.CTQUYETTOANSUACHUAs.DeleteOnSubmit(ctvattu);
                }

              

                var ctdaolaplist = _db.DAOLAPSUACHUAQTs .Where(p => p.MASC .Equals(objDb)).ToList();
                foreach (var daolap in ctdaolaplist)
                {
                    _db.DAOLAPSUACHUAQTs.DeleteOnSubmit(daolap);
                }

           


                // Set delete info
                _db.QUYETTOANSUACHUAs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Quyết toán sữa chữa ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Quyết toán sữa chữa ", objUi.TENCT);
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<QUYETTOANSUACHUA> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Quyết toán sửa chữa ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách quyết toán sữa chữa ");
            }

            return msg;
        }
        public List<QUYETTOANSUACHUA> GetListDonChoDuyetQuyetToan(String KeyWord, DateTime? TuNgay, DateTime? DenNgay)
        {
            var result = _db.QUYETTOANSUACHUAs.Where(d => d.NGAYLQT != null && d.NGAYDQT==null);
            if (TuNgay != null)
                result = result.Where(d => d.NGAYLCT >= TuNgay);
            if (DenNgay != null)
                result = result.Where(d => d.NGAYLCT <= DenNgay);
            if (KeyWord != null)
            {
                result = result.Where(d => d.MADON.Contains(KeyWord) ||
                d.TENCT.Contains(KeyWord) ||
                d.TENHM.Contains(KeyWord) ||
                d.DIACHIHM.Contains(KeyWord));
            }
            return result.ToList();
        }
        public bool DuyetQT(QUYETTOANSUACHUA qt, String useragent, String ipAddress, String sManv, DateTime? ngayduyet)
        {
            DbTransaction trans = null;
            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;
                qt.NGAYDQT = ngayduyet;
                qt.MANVDQT = sManv;
                _db.SubmitChanges();
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = qt.MADON,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.CT05.ToString(),
                    MATT = TTCT.CT_A.ToString(),
                    MOTA = "Duyệt quyết toán Sửa Chữa cho đơn:" + qt.MADON
                };
                _kdDao.Insert(luuvetKyduyet);

                // Submit changes to db
                _db.SubmitChanges();
                trans.Commit();
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
        public List<QUYETTOANSUACHUA> GetListForTraCuu(String keyword, DateTime? fromDate, DateTime? toDate, String stateCode, String areaCode)
        {
            var result = _db.QUYETTOANSUACHUAs.Where(d => d.NGAYDQT == null || d.NGAYDQT != null);
            if (keyword != null)
                result = result.Where(d => d.TENHM.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.MADON.Contains(keyword) ||
                                      d.TENCT.Contains(keyword) ||
                                      d.DIACHIHM.Contains(keyword));
            if (fromDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value >= fromDate.Value);

            if (toDate.HasValue)
                result = result.Where(d => d.NGAYLCT.HasValue
                                           && d.NGAYLCT.Value <= toDate.Value);
            return result.OrderByDescending(d => d.MADON).OrderByDescending(d => d.NGAYLCT).ToList();
        }
    }
}
