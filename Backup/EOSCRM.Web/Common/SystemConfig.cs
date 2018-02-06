using System;

namespace EOSCRM.Web.Common
{
    /// <summary>
    /// Summary description for SystemConfig
    /// </summary>
    public class SystemConfigInfo
    {
        public int PageSize
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Resources.SystemConfig.PageSize);
                }
                catch
                {
                    return 10;
                }
            }
        }


        public int PageButtonCount
        {
            get
            {
                try
                {
                    return Convert.ToInt32(Resources.SystemConfig.PageButtonCount);
                }
                catch
                {
                    return 10;
                }
            }
        }
        
        
    }
}