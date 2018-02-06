using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;

namespace EOSCRM.Web.UserControls
{
    public partial class HopDongFilterPanel : UserControl
    {
        private readonly KhuVucDao kvDao = new KhuVucDao();

        private string id;
        private string sohd;
        private string name;
        private string addr;

        private string fromDate;
        private string toDate;
        private string areaCode;


        public string MADDK
        {
            get { return id; }
            set { id = value; }
        }

        public string SOHD
        {
            get { return sohd; }
            set { sohd = value; }
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

        public string FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        public string ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            LoadStaticReferences();

            cpeFilter.Collapsed = false;

            txtMADDK.Text = id ?? "";
            txtTENKH.Text = name ?? "";
            txtSONHA.Text = addr ?? "";
            txtSOHD.Text = sohd ?? "";
            txtFromDate.Text = fromDate ?? "";
            txtToDate.Text = toDate ?? "";

            try
            {
                if (areaCode == "")
                {
                    ddlKHUVUC.SelectedIndex = 0; 
                    return;
                }
                var item = ddlKHUVUC.Items.FindByValue(areaCode);
                if (item != null)
                    ddlKHUVUC.SelectedIndex = ddlKHUVUC.Items.IndexOf(item);
            }
            catch
            {
                ddlKHUVUC.SelectedIndex = 0;
            }
        }

        private void LoadStaticReferences()
        {
            var list = kvDao.GetList();
            
            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("--Tất cả--", "NULL"));
            foreach(var kv in list)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var url = string.Format("{0}?", Request.Url.AbsolutePath);
            var hash = new Hashtable();

            if(txtMADDK.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_MADDK, EncryptUtil.Encrypt(txtMADDK.Text.Trim()));
            }
            if (txtSOHD.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_SOHD, EncryptUtil.Encrypt(txtSOHD.Text.Trim()));
            }
            if (txtTENKH.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_TENKH, EncryptUtil.Encrypt(txtTENKH.Text.Trim()));
            }
            if (txtSONHA.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_SONHA, EncryptUtil.Encrypt(txtSONHA.Text.Trim()));
            }

            if(txtFromDate.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_FROMDATE, EncryptUtil.Encrypt(txtFromDate.Text.Trim()));
            }
            if (txtToDate.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_TODATE, EncryptUtil.Encrypt(txtToDate.Text.Trim()));
            }
            if(ddlKHUVUC.SelectedIndex > 0)
            {
                hash.Add(Constants.PARAM_AREACODE, EncryptUtil.Encrypt(ddlKHUVUC.SelectedValue.Trim()));
            }
            

            foreach (DictionaryEntry de in hash)
            {
                url += string.Format("{0}={1}&", de.Key, de.Value);
            }
            Response.Redirect(url.Remove(url.Length - 1), false);
        }
    }
}