using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Data.Common;
using System.Linq;
using EOSCRM.Domain;
using EOSCRM.Util;


namespace EOSCRM.Dao
{
    public class WPOClass
    {
        private readonly EOSCRMDataContext _db;        

        public static readonly string Connectionstring = ConfigurationManager.AppSettings[Constants.SETTINGS_CONNECTION];
        public WPOClass()
        {
            _db = new EOSCRMDataContext(Connectionstring);
        }       

        public DataTable FillTable(string ProcName, params ObjectPara[] Para)
        {
            try
            {
                DataTable tb = new DataTable();
                SqlDataAdapter adap = new SqlDataAdapter(ProcName, Connectionstring);
                adap.SelectCommand.CommandType = CommandType.StoredProcedure;
                if (Para != null)
                {
                    foreach (ObjectPara p in Para)
                    {
                        adap.SelectCommand.Parameters.Add(new SqlParameter(p.Name, p.Value));
                    }
                }
                adap.Fill(tb);
                return tb;
            }
            catch
            {
                return null;
            }
        }

    }

    public class ObjectPara
    {
        string _name;

        object _Value;
        public ObjectPara(string Pname, object PValue)
        {
            _name = Pname;
            _Value = PValue;
        }

        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public object Value
        {
            get { return _Value; }
            set { _Value = value; }
        }
    }
}
