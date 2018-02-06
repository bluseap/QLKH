using System;
using System.Web.UI;
using EOSCRM.Dao;
using EOSCRM.Domain;
using EOSCRM.Util;

namespace EOSCRM.Web.Common
{
    /// <summary>
    /// Summary description for Authorization
    /// </summary>
    public class Authentication : Page
    {
        public UserAdmin LoginInfo
        {
            get { return Session[SessionKey.USER_LOGIN] as UserAdmin; }
            set { Session[SessionKey.USER_LOGIN] = value; }
        }

        /// <summary>
        /// Override OnInit Event
        /// </summary>
        /// <param name="e"></param>
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            // Authenticate
            Authenticate();
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        public void Authenticate()
        {
            if (LoginInfo == null)
            {
                // Redirect to login page
                Context.Response.Redirect(WebUrlConstants.LOGIN_PAGE, false);
            }
        }

        /// <summary>
        /// Authenticate
        /// </summary>
        public void Authenticate(Functions function, Permission permission)
        {
            if (HasPermission(function, permission)) return;

            var msg = new Message(MessageConstants.WARN_PERMISSION_DENIED, MessageType.Warning, permission.ToString());
                
            // Do error
            DoError(msg);
        }

        /// <summary>
        /// Has permission
        /// </summary>
        /// <param name="function"></param>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool HasPermission(Functions function, Permission permission)
        {
            try
            {
                return new UserAdminDao().CheckPermision(
                    LoginInfo,
                    function.GetHashCode(),
                    permission.GetHashCode());
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Do error
        /// </summary>
        /// <param name="msg"></param>
        public void DoError(Message msg)
        {
            if (msg == null) return;

            Session[SessionKey.MESSAGE] = msg;

            // Redirect to error page
            Response.Redirect(WebUrlConstants.ERROR_PAGE, false);
        }
       
    }
}