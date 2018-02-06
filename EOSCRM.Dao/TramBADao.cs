using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Dao
{
    public class TramBADao
    {
        private readonly EOSCRMDataContext _db;

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public TramBADao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }
        
        public TRAMBIENAP Get(string id)
        {
            return _db.TRAMBIENAPs.FirstOrDefault(p => p.MATBA.Equals(id));
        }

        public List<TRAMBIENAP> GetListKV(string makv)
        {
            return _db.TRAMBIENAPs.Where(hn => hn.MAKVPO.Equals(makv))
                .OrderBy(hn => hn.MADPPO)
                .ToList();
        }

        public List<TRAMBIENAP> GetListTim(string makv, string maxa, string madp, string tentba, string dsdl)
        {           
            var query1 = from tr in _db.TRAMBIENAPs                         
                         where tr.MAKVPO.Equals(makv)
                         select tr;
            var query = query1.AsQueryable();

            if (maxa != "%")
                query = query.Where(d => d.MAXA.Contains(maxa));

            if (madp != "%")
                query = query.Where(d => d.MADPPO.Contains(madp));

            if (!string.IsNullOrEmpty(tentba))
                query = query.Where(d => d.TENTBA.Contains(tentba));

            if (!string.IsNullOrEmpty(dsdl))
                query = query.Where(d => d.DSODL.Contains(dsdl));

            return query.OrderBy(d => d.MADPPO)//.OrderBy(d => d.MADPPO)
                .ToList();
            //return query.OrderByDescending(d => d.MADDK).OrderByDescending(d => d.NGAYDK).ToList();
        }

        public Message Insert(TRAMBIENAP objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                _db.Connection.Open();
                _db.TRAMBIENAPs.InsertOnSubmit(objUi);             

                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = objUi.MATBA,
                    IPAddress = ipAddress,
                    MANV = sManv,
                    UserAgent = useragent,
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.U.ToString(),
                    MACN = CHUCNANGKYDUYET.KH01.ToString(),
                    MATT = "TRAMBA",
                    MOTA = "Nhập trạm biến áp."
                };
                _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                _db.SubmitChanges();
                msg = new Message(MessageConstants.I_CREATE_SUCCEED, MessageType.Info, "trạm biến áp");
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleInsertException(ex, "Xã, phường ", objUi.TENTBA);
            }
            return msg;
        }

        public Message Update(TRAMBIENAP objUi, String useragent, String ipAddress, String sManv)
        {
            Message msg;
            try
            {
                // get current object in database
                var objDb = Get(objUi.MATBA);

                if (objDb != null)
                {
                    objDb.MAXA = objUi.MAXA;
                    objDb.TENTBA = objUi.TENTBA;
                    objDb.DSODL = objUi.DSODL;
                    objDb.MADPPO = objUi.MADPPO;
                    objDb.NGAYN = DateTime.Now;
                    objDb.MANVN = sManv;

                    objDb.TENTBA2 = objUi.TENTBA2;
                    objDb.QuanHuyenId = objUi.QuanHuyenId;

                    _db.SubmitChanges();

                    #region Luu Vet
                    var luuvetKyduyet = new LUUVET_KYDUYET
                    {
                        MADON = objUi.MATBA,
                        IPAddress = ipAddress,
                        MANV = sManv,
                        UserAgent = useragent,
                        NGAYTHUCHIEN = DateTime.Now,
                        TACVU = TACVUKYDUYET.U.ToString(),
                        MACN = CHUCNANGKYDUYET.KH01.ToString(),
                        MATT = "UPTRAMBA",
                        MOTA = "Up trạm biến áp."
                    };
                    _db.LUUVET_KYDUYETs.InsertOnSubmit(luuvetKyduyet);
                    
                    #endregion
                    // success message
                    msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "trạm biến áp ");
                }
                else
                {
                    // error message
                    msg = new Message(MessageConstants.E_OBJECT_IN_USED, MessageType.Error, "trạm biến áp ", objUi.TENTBA);
                }
            }
            catch (Exception ex)
            {
                msg = ExceptionHandler.HandleUpdateException(ex, "Xã, phường ", objUi.TENTBA);
            }
            return msg;
        }

        public string NewId()
        {
            var query = _db.TRAMBIENAPs.Max(p => p.MATBA);

            if (!string.IsNullOrEmpty(query))
            {
                var temp = int.Parse(query) + 1;

                return temp.ToString("D6");
            }
            else
            {
                return "000001";
            }
        }

    }
}
