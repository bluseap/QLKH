using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Reflection;
using System .Data ;


namespace EOSCRM.Domain
{
    public partial class EOSCRMDataContext
    {
        [Function(Name = "dbo.GetFunctionList")]
        public ISingleResult<CrmFunction> GetFunctionList(
            [Parameter(DbType = "Int")] int parentId,
            [Parameter(DbType = "NVarChar(50)")] string userName)
        {
            var result = ExecuteMethodCall(this, ((MethodInfo)(MethodBase.GetCurrentMethod())), parentId, userName);
            return (ISingleResult<CrmFunction>)(result.ReturnValue);
        }

        
        
    }
}