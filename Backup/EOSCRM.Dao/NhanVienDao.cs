using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public  class NhanVienDao
    {
        private readonly EOSCRMDataContext _db;
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public NhanVienDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public NHANVIEN  Get(string ma)
        {
            return _db.NHANVIENs.Where(p => p.MANV.Equals(ma)).SingleOrDefault();
        }

        public List<NHANVIEN> Search(string key)
        {
            return _db.NHANVIENs.Where(p => p.HOTEN.ToUpper().Contains(key.ToUpper())
                || p.MANV.ToUpper().Contains(key.ToUpper()) 
                || p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                || p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper()))
                    .OrderBy(p=>p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NHANVIEN> SearchKV(string key, string makv, string macv)
        {
            //return _db.NHANVIENs.Where(p => (p.MAKV.Equals(makv) && p.MAPB.Equals(macv))// && p.HOTEN.ToUpper().Contains(key.ToUpper())
                //|| p.MANV.ToUpper().Contains(key.ToUpper())
                //|| p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                //|| p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper())
                //)
            return _db.NHANVIENs.Where(p => (p.MAKV.Equals(makv))
                )
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NHANVIEN> SearchKV2(string key, string makv)
        {
            return _db.NHANVIENs.Where(p => ((p.MAKV.Equals(makv) && p.HOTEN.ToUpper().Contains(key.ToUpper()))))
                //|| p.MANV.ToUpper().Contains(key.ToUpper())
                //|| p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                //|| p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper())))
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NHANVIEN> SearchKV3(string key, string makv, string macv)
        {
            return _db.NHANVIENs.Where(p => (p.MAKV.Equals(makv))// && p.HOTEN.ToUpper().Contains(key.ToUpper())
                //|| p.MANV.ToUpper().Contains(key.ToUpper())
                //|| p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                //|| p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper())
                //)
                )
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NHANVIEN> Search(string key, string mapb)
        {
            var query = _db.NHANVIENs.AsQueryable();

            if (mapb != "" && mapb != "%")
                query = query.Where(nv => nv.MAPB.Equals(mapb)).AsQueryable();

            return query.Where(p => p.HOTEN.ToUpper().Contains(key.ToUpper())
                || p.MANV.ToUpper().Contains(key.ToUpper())
                || p.PHONGBAN.TENPB.ToUpper().Contains(key.ToUpper())
                || p.CONGVIEC.TENCV.ToUpper().Contains(key.ToUpper()))
                    .OrderBy(p => p.HOTEN)
                    .OrderBy(p => p.MACV)
                    .OrderBy(p => p.MAPB)
                    .ToList();
        }

        public List<NHANVIEN> GetList()
        {
            return _db.NHANVIENs.ToList();
        }

        public List<NHANVIEN> GetListKV(string ma)
        {
            return _db.NHANVIENs.Where(p=>p.MANV==ma).ToList();
        }

        

        public  NHANVIEN GetKV(string ma)
        {
            return _db.NHANVIENs.Where(p => p.MANV == ma).SingleOrDefault();
        }

        public NHANVIEN GetMAPB(string ma)
        {
            return _db.NHANVIENs.Where(p => p.MAPB == ma).SingleOrDefault();
        }
     

        public List<NHANVIEN> GetList(String manv, String tennv, String khuvuc, String phongban, String congviec)
        {
            var query = _db.NHANVIENs.AsEnumerable();

            if (!String.IsNullOrEmpty(manv))
                query = query.Where(nv => nv.MANV.ToUpper().Contains(manv.ToUpper()));

            if (!String.IsNullOrEmpty(tennv))
                query = query.Where(nv => nv.HOTEN.ToUpper().Contains(tennv.ToUpper()));

            if (!String.IsNullOrEmpty(khuvuc) && khuvuc != "%")
                query = query.Where(nv => nv.MAKV.ToUpper().Equals(khuvuc.ToUpper()));

            if (!String.IsNullOrEmpty(phongban) && phongban != "%")
                query = query.Where(nv => nv.MAPB.ToUpper().Equals(phongban.ToUpper()));

            if (!String.IsNullOrEmpty(congviec) && congviec != "%")
                query = query.Where(nv => nv.MACV.ToUpper().Equals(congviec.ToUpper()));

            return query.ToList();
        }

        public List<NHANVIEN> GetListByPB(string maPb)
        {
            return _db.NHANVIENs.Where(p => p.MAPB.Equals(maPb)).ToList();
        }

        public List<NHANVIEN> GetListByCV(string maCv)
        {
            return _db.NHANVIENs.Where(p => p.MACV.Equals(maCv)).ToList();
        }

        public List<NHANVIEN> GetListByCVNV(string maCv, string makv)
        {
            return _db.NHANVIENs.Where(p => p.MACV.Equals(maCv) && p.MAKV==makv).ToList();
        }

        public List<NHANVIEN> GetList(int fromIndex, int toIndex)
        {
            return GetList().Skip(fromIndex).Take(toIndex - fromIndex).ToList();
        }

        public int Count( )
        {
            return _db.NHANVIENs.Count();
        }

        public Message Insert(NHANVIEN objUi)
        {
            Message msg;
            try
            {

                _db.Connection.Open();

                _db.NHANVIENs.InsertOnSubmit(objUi);
                _db.SubmitChanges();

                // commit

                // success message
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "nhân viên");
            }
            catch (Exception ex)
            {
                // rollback transaction

                msg = ExceptionHandler.HandleInsertException(ex, "nhân viên");
            }
            return msg;
        }

        public Message Update(NHANVIEN objUi)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MANV  );

                if (objDb != null)
                {
                    //TODO: update all fields
                    objDb.CHUKY    = objUi.CHUKY     ;
                    objDb.DIACHI = objUi.DIACHI  ;
                    objDb.HOTEN = objUi.HOTEN  ;
                    if (!string.IsNullOrEmpty(objUi.MACB))
                        objDb.CAPBAC = _db.CAPBACs.Single(p => p.MACB.Equals(objUi.MACB));
                    //objDb.MACB= objUi.MACB  ;
                    if (!string.IsNullOrEmpty(objUi.MACV))
                        objDb.CONGVIEC = _db.CONGVIECs.Single(p => p.MACV.Equals(objUi.MACV));
                    //objDb.MACV = objUi.MACV ;
                    if (!string.IsNullOrEmpty(objUi.MAKV))
                        objDb.KHUVUC = _db.KHUVUCs.Single(p => p.MAKV.Equals(objUi.MAKV));
                    //objDb.MAKV= objUi.MAKV ;
                    if (!string.IsNullOrEmpty(objUi.MAPB))
                        objDb.PHONGBAN = _db.PHONGBANs.Single(p => p.MAPB.Equals(objUi.MAPB));
                    //objDb.MAPB = objUi.MAPB ;
                    if (!string.IsNullOrEmpty(objUi.MATD))
                        objDb.TRINHDO = _db.TRINHDOs.Single(p => p.MATD.Equals(objUi.MATD));
                    //objDb.MATD  = objUi.MATD  ;
                    objDb.NGAYSINH = objUi.NGAYSINH ;
                    objDb.PASSWORD_SV = objUi.PASSWORD_SV;
                    objDb.SDT = objUi.SDT;
                     
                      // Submit changes to db
                    _db.SubmitChanges();

                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "nhân viên");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "nhân viên");
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Nhân viên ", objUi.HOTEN    );
            }
            return msg;
        }

        public bool IsInUse(string ma)
        {
            if( _db.THICONGs  .Where(p => p.MANV  .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DONDANGKies  .Where(p => p.MANV .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DUONGPHOs .Where(p => p.MANVG .Equals(ma)).Count() > 0)
                return true;
            else if (_db.DUONGPHOs.Where(p => p.MANVT .Equals(ma)).Count() > 0)
                return true;
            else if (_db.KIEMDINHDHs .Where(p => p.MANVKD.Equals(ma)).Count() > 0)
                return true;
            else if (_db.KIEMDINHDHs.Where(p => p.MANVRS.Equals(ma)).Count() > 0)
                return true;
            else if (_db.GIAIQUYETTHONGTINSUACHUAs .Where(p => p.MANVBAO.Equals(ma)).Count() > 0)
                return true;
            else if (_db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MANVN.Equals(ma)).Count() > 0)
                return true;
            else if (_db.GIAIQUYETTHONGTINSUACHUAs.Where(p => p.MANVXL.Equals(ma)).Count() > 0)
                return true;
            else
            {
                return false;
            }
        }

        public Message Delete(NHANVIEN objUi)
        {
            Message msg;

            try
            {
                // Get current Item in db
                var objDb = Get(objUi.MANV );

                if (objDb == null)
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Nhân viên ", objUi.MANV );
                    return msg;
                }

                //TODO: check if "hồ sơ đất" is in use

                // Set delete info
                _db.NHANVIENs.DeleteOnSubmit(objDb);
                // Submit changes to db
                _db.SubmitChanges();

                // success message
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Nhân viên ");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleDeleteException(ex, "Nhân viên ");
            }

            return msg;
        }

        /// <summary>
        /// Delete list
        /// </summary>
        /// <param name="objList"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public Message DeleteList(List<NHANVIEN> objList, PageAction action)
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
                msg = new Message(MessageConstants.I_DELETE_SUCCEED, MessageType.Info, "Nhân viên ");
            }
            catch (Exception ex)
            {
                // rollback transaction
                if (trans != null)
                    trans.Rollback();

                msg = ExceptionHandler.HandleInsertException(ex, "Danh sách nhân viên ");
            }

            return msg;
        }
    }
}
