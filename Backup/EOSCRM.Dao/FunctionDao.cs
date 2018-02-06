using System.Collections.Generic;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;
using System.Configuration;

namespace EOSCRM.Dao
{
    public class FunctionDao
    {
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        private readonly EOSCRMDataContext _db;

        /// <summary>
        /// constructor
        /// </summary>
        public FunctionDao()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }

        /// <summary>
        /// Get function by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public CrmFunction Get(int id)
        {
            return _db.CrmFunctions.Where(g => g.Id.Equals(id)
                && g.Deleted.Equals(false)).SingleOrDefault();
        }

        /// <summary>
        /// Get function list
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public List<CrmFunction> GetList(int id)
        {
            return _db.CrmFunctions.Where(g => g.Parent.Equals(id) &&
                                               g.Deleted.Equals(false)).
                                               OrderBy(f => f.Order).
                                               ToList();
        }

        /// <summary>
        /// Get function list by parent id and logged in username
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userName"></param>
        /// <returns></returns>
        public List<CrmFunction> GetList(int id, string userName)
        {
            return _db.GetFunctionList(id, userName).ToList();
        }

        

    }
}
