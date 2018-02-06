using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
namespace EOSCRM.Dao
{
    public class SystemConfigDao
    {
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly EOSCRMDataContext db;

         /// <summary>
        /// constructor
        /// </summary>
        public SystemConfigDao()
        {
            db = new EOSCRMDataContext(Connectionstring);
        }

        /// <summary>
        /// Get Hash List
        /// </summary>
        /// <returns></returns>
        public Hashtable GetHashList()
        {
            try
            {
                var objList = db.SystemConfigs.ToList();
                var objHash = new Hashtable();
                foreach (var info in objList)
                {
                    objHash.Add(info.Key, info.Value);
                }

                return objHash;
            }
            catch
            {
                return null;
            }
        }

        public List<SystemConfig> GetList()
        {
            return db.SystemConfigs.ToList();
        }

        public Message Update(SystemConfig objUI)
        {
            Message msg;
            try
            {

                var objDB = db.SystemConfigs.Where(p => p.Key == objUI.Key).FirstOrDefault();

                if (objDB == null || objUI == null)
                {
                    // TODO: Show system error
                    return new Message(MessageConstants.E_OBJECT_NOT_EXISTS, MessageType.Error, "Cấu hình", "");
                }

                objDB.Value = objUI.Value;

                // Submit changes to db
                db.SubmitChanges();

                // Show success message
                msg = new Message(MessageConstants.I_UPDATE_SUCCEED, MessageType.Info, "config");
            }
            catch (Exception ex)
            {
                // TODO: Show system error
                msg = new Message(MessageConstants.E_EXCEPTION, MessageType.Error, ex.Message, ex.StackTrace);
            }

            return msg;
        }
    }
}
