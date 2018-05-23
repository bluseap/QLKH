using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class ThietKePoDao
    {
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        private readonly EOSCRMDataContext _db;
        private static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        
        public ThietKePoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public THIETKEPO  Get(string ma)
        {
            return _db.THIETKEPOs.SingleOrDefault(p => p.MADDKPO.Equals(ma));
        }

        public Message Insert(THIETKEPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                // insert to thietke
                _db.THIETKEPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // change dondangky's TTTK field to TK_P
                objUi.DONDANGKYPO.TTTK = TTTK.TK_P.ToString();
                objUi.DONDANGKYPO.SOTRUPO = objUi.SOTRUKH != null ? objUi.SOTRUKH : "";
                _db.SubmitChanges();

                var mbvt =
                    _db.MAUBOCVATTUs.FirstOrDefault(m => m.DUOCCHON.HasValue && m.DUOCCHON.Value.Equals(true));

                if (mbvt != null)
                {
                    // insert:
                    // - CTMAUBOCVATTU to CTTHIETKE, 
                    // - DAOLAPMAUBOCVATTU to DAOLAPTK
                    // - GCMAUBOCVATTU to GCTHIETKE
                    var ctmbvt = _db.CTMAUBOCVATTUs.Where(ct => ct.MADDK.Equals(mbvt.MADDK)).ToList();
                    foreach (var ct in ctmbvt)
                    {
                        var cttk = new CTTHIETKE
                                       {
                                           MADDK = objUi.MADDKPO,
                                           MAVT = ct.MAVT,
                                           NOIDUNG = ct.NOIDUNG,
                                           SOLUONG = ct.SOLUONG,
                                           GIANC = ct.GIANC,
                                           GIAVT = ct.GIAVT,
                                           TIENNC = ct.TIENNC,
                                           TIENVT = ct.TIENVT,
                                           ISCTYDTU = ct .ISCTYDTU 
                                       };
                        _db.CTTHIETKEs.InsertOnSubmit(cttk);
                    }

                    var gcmbvt = _db.GCMAUBOCVATTUs.Where(p => p.MAMBVT.Equals(mbvt.MADDK)).ToList();
                    foreach (var gc in gcmbvt)
                    {
                        var gctk = new GCTHIETKE
                                       {
                                           MAMBVT = objUi.MADDKPO,
                                           NOIDUNG = gc.NOIDUNG
                                       };
                        _db.GCTHIETKEs.InsertOnSubmit(gctk);
                    }

                    var cpmbvt = _db.DAOLAPMAUBOCVATTUs.Where(p => p.MAMAUBOCVATTU.Equals(mbvt.MADDK)).ToList();
                    foreach (var cp in cpmbvt)
                    {
                        var cptk = new DAOLAPTK
                                       {
                                           MADDK = objUi.MADDKPO,
                                           LOAICV = cp.LOAICV,
                                           LOAICT = cp.LOAICT,
                                           NOIDUNG = cp.NOIDUNG,
                                           DONGIACP = cp.DONGIACP,
                                           DVT = cp.DVT,
                                           HESOCP = cp.HESOCP,
                                           SOLUONG = cp.SOLUONG,
                                           THANHTIENCP = cp.THANHTIENCP,
                                           LOAICP = cp.LOAICP,
                                           LOAI = cp.LOAI,
                                           NGAYLAP = cp.NGAYLAP
                                       };
                        _db.DAOLAPTKs.InsertOnSubmit(cptk);
                    }

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.A.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTTK.TK_P.ToString(),
                    MOTA = "Nhập thiết kế."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "thiết kế");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Thiết kế ", "");
            }
            return msg;
        }

        public Message Update(THIETKEPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO    );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CHUTHICH = objUi.CHUTHICH;
                    objDb.FILETK = objUi.FILETK;
                    objDb.FILETK_HC = objUi.FILETK_HC;
                    objDb.NGAYGUI_CT = objUi.NGAYGUI_CT;
                    objDb.NGAYNHAN_CT = objUi.NGAYNHAN_CT;                    
                    objDb.SOBCT = objUi.SOBCT;
                    objDb.TENTK = objUi.TENTK;
                    objDb.NGAYLTK = objUi.NGAYLTK;
                    objDb.NGAYDTK = objUi.NGAYDTK;
                    objDb.MANVLTK = objUi.MANVLTK;
                    objDb.MANVDTK = objUi.MANVDTK;
                    objDb.MANVTK = objUi.MANVTK;
                    objDb.TENNVTK = objUi.TENNVTK;
                    objDb.THECHAP = objUi.THECHAP;
                    objDb.THAMGIAONGCAI = objUi.THAMGIAONGCAI;
                    objDb.SODB = objUi.SODB;
                    objDb.SOTRUKH = objUi.SOTRUKH;
                    objDb.TENTRAMKH = objUi.TENTRAMKH;
                    objDb.TUYENDAYHATHE = objUi.TUYENDAYHATHE;

                    objDb.MAHTTT = objUi.MAHTTT;
                  
                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTTK.TK_P.ToString(),
                        MOTA = "Sửa thiết kế"
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Thiết kế ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Thiết kế ", "");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Thiết kế ", "");
            }
            return msg;
        }

        

        public Message Delete(THIETKEPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MADDKPO );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Thiết kế ", "");
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                var ctvattuslist = _db.CTTHIETKEs.Where(p => p.MADDK.Equals(objDb)).ToList();
                foreach (var ctvattu in ctvattuslist)
                {
                    _db.CTTHIETKEs.DeleteOnSubmit(ctvattu);
                }
                // Set delete info
                _db.THIETKEPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();


                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MADDKPO,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.D.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = TTTK.TK_P.ToString(),
                    MOTA = "Sửa thiết kế"
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thiết kế ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Thiết kế ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<THIETKEPO> objList, PageAction action, String useragent, String ipAddress, String sManv)
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
                    Delete(obj, useragent, ipAddress, sManv);
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách thiết kế ");
            }

            return msg;
        }

        public Message ChangeFromMBVT(THIETKEPO objUi, MAUBOCVATTU mbvt)
        {
            Message msg;
            DbTransaction trans = null;

            try
            {
                _db.Connection.Open();
                trans = _db.Connection.BeginTransaction();
                _db.Transaction = trans;

                if (mbvt != null)
                {
                    // clear all existing data
                    var cttk = _db.CTTHIETKEs.Where(ct => ct.MADDK.Equals(objUi.MADDKPO)).ToList();
                    foreach (var ct in cttk)
                    {
                        _db.CTTHIETKEs.DeleteOnSubmit(ct);
                    }

                    var gctk = _db.GCTHIETKEs.Where(gc => gc.MAMBVT.Equals(objUi.MADDKPO)).ToList();
                    foreach (var gc in gctk)
                    {
                        _db.GCTHIETKEs.DeleteOnSubmit(gc);
                    }

                    var cptk = _db.DAOLAPTKs.Where(cp => cp.MADDK.Equals(objUi.MADDKPO)).ToList();
                    foreach (var cp in cptk)
                    {
                        _db.DAOLAPTKs.DeleteOnSubmit(cp);
                    }
                    
                    _db.SubmitChanges();

                    // insert from maubocvattu
                    // insert:
                    // - CTMAUBOCVATTU to CTTHIETKE, 
                    // - DAOLAPMAUBOCVATTU to DAOLAPTK
                    // - GCMAUBOCVATTU to GCTHIETKE
                    var ctmbvt = _db.CTMAUBOCVATTUs.Where(ct => ct.MADDK.Equals(mbvt.MADDK)).ToList();
                    foreach (var ct in ctmbvt)
                    {
                        var newCttk = new CTTHIETKE
                        {
                            MADDK = objUi.MADDKPO,
                            MAVT = ct.MAVT,
                            NOIDUNG = ct.NOIDUNG,
                            SOLUONG = ct.SOLUONG,
                            GIANC = ct.GIANC,
                            GIAVT = ct.GIAVT,
                            TIENNC = ct.TIENNC,
                            TIENVT = ct.TIENVT,
                            ISCTYDTU = ct.ISCTYDTU 
                        };
                        _db.CTTHIETKEs.InsertOnSubmit(newCttk);
                    }

                    var gcmbvt = _db.GCMAUBOCVATTUs.Where(p => p.MAMBVT.Equals(mbvt.MADDK)).ToList();
                    foreach (var gc in gcmbvt)
                    {
                        var newGctk = new GCTHIETKE
                        {
                            MAMBVT = objUi.MADDKPO,
                            NOIDUNG = gc.NOIDUNG
                        };
                        _db.GCTHIETKEs.InsertOnSubmit(newGctk);
                    }

                    var cpmbvt = _db.DAOLAPMAUBOCVATTUs.Where(p => p.MAMAUBOCVATTU.Equals(mbvt.MADDK)).ToList();
                    foreach (var cp in cpmbvt)
                    {
                        var newCptk = new DAOLAPTK
                        {
                            MADDK = objUi.MADDKPO,
                            LOAICV = cp.LOAICV,
                            LOAICT = cp.LOAICT,
                            NOIDUNG = cp.NOIDUNG,
                            DONGIACP = cp.DONGIACP,
                            DVT = cp.DVT,
                            HESOCP = cp.HESOCP,
                            SOLUONG = cp.SOLUONG,
                            THANHTIENCP = cp.THANHTIENCP,
                            LOAICP = cp.LOAICP,
                            LOAI = cp.LOAI,
                            NGAYLAP = cp.NGAYLAP
                        };
                        _db.DAOLAPTKs.InsertOnSubmit(newCptk);
                    }

                    _db.SubmitChanges();
                }

                // commit
                trans.Commit();

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "Thiết kế ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Thiết kế ", "");
            }
            return msg;
        }

        public Message UpdateHinh1(THIETKEPO objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MADDKPO);

                if (objDb != null)
                {
                    //TODO: update all fields       
                    objDb.HINHTK1 = objUi.HINHTK1;

                    // Submit changes to db
                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MADDKPO,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = TTTK.TK_P.ToString(),
                        MOTA = "Sửa thiết kế. Hình thiết kế."
                    };
                    _kdDao.Insert(luuvetKyduyet);
                    #endregion

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Thiết kế ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Thiết kế ", "");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Thiết kế ", "");
            }
            return msg;
        }
    }
}
