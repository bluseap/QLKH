using System;
using Dapper;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace POWACO.Dapper.DanhMuc.BaoCao.DuongPho
{
    public class DuongPhoRepository : IDuongPhoRepository
    {        
        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];

        public DuongPhoRepository()
        {
           
        }

        public List<DuongPhoVm> GetAll()
        {
            using (var conn = new SqlConnection(Connectionstring))            
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();

                var paramaters = new DynamicParameters();
                //paramaters.Add("@id", id);

                var result = conn.QueryAsync<DuongPhoVm>("Get_DuongPho_ByAll",
                    paramaters, null, null, System.Data.CommandType.StoredProcedure);

                return result.Result.ToList();
            }
        }

    }
}
