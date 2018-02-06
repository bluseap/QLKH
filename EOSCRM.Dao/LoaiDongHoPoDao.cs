using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class LoaiDongHoPoDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public LoaiDongHoPoDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public LOAIDHPO  Get(string ma)
        {
            return _db.LOAIDHPOs.Where(p => p.MALDHPO.Equals(ma)).SingleOrDefault();
        }       

        public List<LOAIDHPO> Search(string key)
        {
            return _db.LOAIDHPOs.Where(p => p.MOTAKC.ToUpper().Contains(key.ToUpper())).ToList();
        }

        public List<LOAIDHPO> GetList()
        {
            return _db.LOAIDHPOs.ToList();
        }

        public List<LOAIDHPO> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public List<LOAIDHPO> GetList(string keyword)
        {
            return _db.LOAIDHPOs.Where(p => (p.MALDHPO.ToLower().Contains(keyword.ToLower())
                || p.NSX.ToLower().Contains(keyword.ToLower())
                || p.KICHCO.ToLower().Contains(keyword.ToLower())
                || p.KDH.ToLower().Contains(keyword.ToLower())
                )).ToList();
        }

        public List<LOAIDHPO> GetListldh(string keyword)
        {
            return _db.LOAIDHPOs.Where(p => (p.MALDHPO.ToLower().Contains(keyword.ToLower())
                )).ToList();
        }

        public int Count( )
        {
            return _db.LOAIDHPOs.Count();
        }

        public Message Insert(LOAIDHPO objUi)
        {
            Message msg;
            try
            {
                _db.Connection.Open();

                _db.LOAIDHPOs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "loại đồng hồ");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "loại đồng hồ", objUi.MOTAKC);
            }
            return msg;
        }

        public Message Update(LOAIDHPO objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MALDHPO   );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.KICHCO = objUi.KICHCO;
                    objDb.MOTAKC     =  objUi.MOTAKC    ;
                    objDb.LUULUONG_CT = objUi.LUULUONG_CT;
                    objDb.LUULUONG_DN = objUi.LUULUONG_DN;
                    objDb.LUULUONG_NN = objUi.LUULUONG_NN;
                    objDb.MOTAKDH = objUi.MOTAKDH;
                    objDb.MOTAKDH = objUi.MOTAKDH;
                    objDb.MOTANSX = objUi.MOTANSX;
                    objDb.NSX = objUi.NSX;
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "Loại đồng hồ ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "Loại đồng hồ ", objUi.MOTAKC   );
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Loại đồng hồ ", objUi.MOTAKC);
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            return _db.DONGHOs.Where(p => p.MALDH.Equals(ma)).Count() > 0;
        }

        public Message Delete(LOAIDHPO objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MALDHPO );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Loại đồng hồ ", objUi.MALDHPO      );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.LOAIDHPOs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "loại đồng hồ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "loại đồng hồ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <returns></returns>
        public Message DeleteList(List<LOAIDHPO> objList)
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
                        return new Message(MessageConstants.E_DELETE_FAILED_EXCEPTION, MessageType.Info, "danh sách loại đồng hồ");

                    var succeed = objList.Count - failed;
                    return new Message(MessageConstants.W_DELETELIST_SUCCEED_WITH_ERRORS, MessageType.Warning,
                                       succeed, "loại đồng hồ", failed, "loại đồng hồ");
                }
                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, objList.Count + " loại đồng hồ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "danh sách loại đồng hồ");
            }

            return msg;
        }
    }
}
