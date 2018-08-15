using System;
using EOSCRM.Domain;
using EOSCRM.Dao;
using EOSCRM.Web.Common;
using EOSCRM.Util;

namespace EOSCRM.Web
{
    public partial class Login : System.Web.UI.Page
    {
        private readonly ViTriThietBiDao _vttbDao = new ViTriThietBiDao();
        private readonly UserAdminDao _objServ = new UserAdminDao();
        private readonly KyDuyetDao _kdDao = new KyDuyetDao();

        protected void Page_Load(object sender, EventArgs e)
        {
            Session.RemoveAll();

            //AutoViTriThietBi();

            if (!Page.IsPostBack)
            {
                //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "LoadgetLocation();", true);

                txtUserName.Focus();
            }
        }

        protected void btnOK_Click(object sender, EventArgs e)
        {
            if (CheckLogin())
            {
                /*if (screen.width <= 800)
                {
                    window.location = "http://powaco.com.vn/WebMobi/KhachHang/MNhapDLDM.aspx";
                }
                else
                {

                    Response.Redirect(WebUrlConstants.HOME_PAGE, false);
                }*/

                //AutoViTriThietBi();

                Response.Redirect(WebUrlConstants.HOME_PAGE, false);

                #region Luu Vet
                var luuvetKyduyet = new LUUVET_KYDUYET
                {
                    MADON = txtUserName.Text.Trim(),
                    //IPAddress = CommonFunc.GetIpAdddressComputerName(),
                    IPAddress = CommonFunc.GetLanIPAddressM(),

                    MANV = txtUserName.Text.Trim(),
                    UserAgent = CommonFunc.GetComputerName(),
                    NGAYTHUCHIEN = DateTime.Now,
                    TACVU = TACVUKYDUYET.I.ToString(),
                    MACN = CHUCNANGKYDUYET.KH05.ToString(),
                    MATT = CHUCNANGKYDUYET.KH05.ToString(),
                    MOTA = "User login.."
                };
                _kdDao.Insert(luuvetKyduyet);
                #endregion
            }
        }

        private bool CheckLogin()
        {
            try
            {
                if (Page.IsValid)
                {
                    var nguoidung = _objServ.GetHoTen(txtUserName.Text.Trim());

                    var objUi = new UserAdmin
                                    {
                                        //Username = txtUserName.Text.Trim()
                                        Username = nguoidung.Username.ToString()
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
            //AutoViTriThietBi();

            txtUserName.Text = string.Empty;
            txtPassword.Text = string.Empty;
            txtUserName.Focus();            
        }

        private void AutoViTriThietBi()
        {
            string latvt = hfLATVT.Value;
            var vitri = new VITRITB
            {
                USERAGENT = CommonFunc.GetDeviceName(),
                IPADDRESS = CommonFunc.GetLanIPAddressM(),
                PHYSYCAL = CommonFunc.GetMACAddress0(),
                LATVT = hfLATVT.Value,
                LONGVT = hfLONGVT.Value,
                //MANVN = "",
                NGAYN = DateTime.Now                
            };
            _vttbDao.Insert(vitri);     
        }

    }
}