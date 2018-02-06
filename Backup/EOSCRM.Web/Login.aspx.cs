using System;
using EOSCRM.Domain;
using EOSCRM.Dao;
using EOSCRM.Web.Common;
using EOSCRM.Util;

namespace EOSCRM.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly UserAdminDao _objServ = new UserAdminDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();
            if (!Page.IsPostBack)
            {
                txtUserName.Focus();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckLogin())
            {
                Response.Redirect(WebUrlConstants.HOME_PAGE, false);
            }
        }

        private bool CheckLogin()
        {
            try
            {
                if (Page.IsValid)
                {
                    var objUi = new UserAdmin
                                    {
                                        Username = txtUserName.Text.Trim()
                                    };
                    var password = txtPassword.Text.Trim();
                    objUi.Password = password;
                    var objDb = _objServ.CheckLogin(objUi);

                    if(objDb != null)
                    {
                        Session[SessionKey.USER_LOGIN] = objDb;
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
            return false;
        }

        protected void btnReset_Click(object sender, EventArgs e)
        {
            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Focus();
        }
    }
}