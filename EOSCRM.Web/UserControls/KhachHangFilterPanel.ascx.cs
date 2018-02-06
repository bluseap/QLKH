using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;

using EOSCRM.Domain;
using EOSCRM.Web.Shared;
using EOSCRM.Web.UserControls;

namespace EOSCRM.Web.UserControls
{
    public partial class KhachHangFilterPanel : UserControl
    {
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly NhanVienDao _nvDao = new NhanVienDao();        

        private string id;
        private string sohd;
        private string madh;
        private string name;
        private string addr;
        private string tendp;
        private string areaCode;
        private string xoabonuoc;
        private string sodienthoai;

        public string IDKH
        {
            get { return id; }
            set { id = value; }
        }

        public string SOHD
        {
            get { return sohd; }
            set { sohd = value; }
        }

        public string MADH
        {
            get { return madh; }
            set { madh = value; }
        }

        public string TENKH
        {
            get { return name; }
            set { name = value; }
        }

        public string SONHA
        {
            get { return addr; }
            set { addr = value; }
        }

        public string SODIENTHOAI
        {
            get { return sodienthoai; }
            set { sodienthoai = value; }
        }

        public string TENDP
        {
            get { return tendp; }
            set { tendp = value; }
        }

        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        public string XOABONUOC
        {
            get { return xoabonuoc; }
            set { xoabonuoc = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            LoadStaticReferences();

            txtIDKH.Text = id ?? "";
            txtTENKH.Text = name ?? "";
            txtSONHA.Text = addr ?? "";
            txtSOHD.Text = sohd ?? "";
            txtMADH.Text = madh ?? "";
            txtTENDP.Text = tendp ?? "";

            txtSODIENTHOAI.Text = sodienthoai ?? "";

            ddlXOABO.Items.Clear();            
            ddlXOABO.Items.Add(new ListItem("Không", "0"));
            ddlXOABO.Items.Add(new ListItem("Có", "1"));

            cpeFilter.Collapsed = false;
        }


        public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _nvDao.GetListKV(b);
            foreach (var a in query)
            {
                string d = a.MAKV;

                if (a.MAKV == "99")
                {
                    var kvList = kvDao.GetList();
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "NULL"));
                    foreach (var kv in kvList)
                    {
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
                else
                {
                    var kvList = kvDao.GetListKV(d);
                    ddlKHUVUC.Items.Clear();                    
                    foreach (var kv in kvList)
                    {                        
                        ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
                    }
                }
            }
        }

        private void LoadStaticReferences()
        {
            
            timkv();
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var url = string.Format("{0}?", Request.Url.AbsolutePath);
            var hash = new Hashtable { { Constants.PARAM_FILTERED, "true" } };

            if(txtIDKH.Text.Trim() != "") 
                hash.Add(Constants.PARAM_IDKH, EncryptUtil.Encrypt(txtIDKH.Text.Trim()));

            if (txtSOHD.Text.Trim() != "")
                hash.Add(Constants.PARAM_SOHD, EncryptUtil.Encrypt(txtSOHD.Text.Trim()));

            if (txtMADH.Text.Trim() != "")
                hash.Add(Constants.PARAM_MADH, EncryptUtil.Encrypt(txtMADH.Text.Trim()));

            if (txtTENKH.Text.Trim() != "")
                hash.Add(Constants.PARAM_TENKH, EncryptUtil.Encrypt(txtTENKH.Text.Trim()));

            if (txtSONHA.Text.Trim() != "")
                hash.Add(Constants.PARAM_SONHA, EncryptUtil.Encrypt(txtSONHA.Text.Trim()));

            if (txtTENDP.Text.Trim() != "")
                hash.Add(Constants.PARAM_TENDP, EncryptUtil.Encrypt(txtTENDP.Text.Trim()));

            if (txtSODIENTHOAI.Text.Trim() != "")
                hash.Add(Constants.PARAM_SODIENTHOAI, EncryptUtil.Encrypt(txtSODIENTHOAI.Text.Trim()));

            if(ddlKHUVUC.SelectedIndex >=0)
                hash.Add(Constants.PARAM_AREACODE, EncryptUtil.Encrypt(ddlKHUVUC.SelectedValue.Trim()));

            if (ddlXOABO.SelectedIndex >= 0)
                hash.Add(Constants.PARAM_XOABONUOC, EncryptUtil.Encrypt(ddlXOABO.SelectedValue.Trim()));

            
            foreach (DictionaryEntry de in hash)
            {
                url += string.Format("{0}={1}&", de.Key, de.Value);
            }
            Response.Redirect(url.Remove(url.Length - 1), false);
        }
    }
}