using System;
using System.Collections;
using System.Web.UI;
using System.Web.UI.WebControls;
using EOSCRM.Dao;
using EOSCRM.Util;
using EOSCRM.Web.Common;
using EOSCRM.Domain;


namespace EOSCRM.Web.UserControls
{
    public partial class FilterPanel : UserControl
    {
        private readonly KhuVucDao kvDao = new KhuVucDao();
        private readonly EOSCRMDataContext _db = new EOSCRMDataContext();

        private string keyword;
        private string fromDate;
        private string toDate;
        private string stateCode;
        private string areaCode;
        private string dateText;

        public string FromDate
        {
            get { return fromDate; }
            set { fromDate = value; }
        }

        public string Keyword
        {
            get { return keyword; }
            set { keyword = value; }
        }

        public string ToDate
        {
            get { return toDate; }
            set { toDate = value; }
        }

        public string StateCode
        {
            get { return stateCode; }
            set { stateCode = value; }
        }

        public string AreaCode
        {
            get { return areaCode; }
            set { areaCode = value; }
        }

        public string DateText
        {
            get { return dateText; }
            set { dateText = value; }
        }

        public string TrangThai
        {
            get
            {
                return (Session["FILTERPANEL_TRANGTHAI"] == null) ?
                    "" :
                    Session["FILTERPANEL_TRANGTHAI"].ToString();
            }
            set { Session["FILTERPANEL_TRANGTHAI"] = value; }
        }

        public bool ShowAreaCode
        {
            get
            {
                return (Session["FILTERPANEL_SHOWAREA"] == null) ? 
                    false :
                    Convert.ToBoolean(Session["FILTERPANEL_SHOWAREA"].ToString());
            }
            set { Session["FILTERPANEL_SHOWAREA"] = value.ToString(); }
        }

        public bool ShowStates
        {
            get
            {
                return (Session["FILTERPANEL_SHOWSTATES"] == null) ?
                    false :
                    Convert.ToBoolean(Session["FILTERPANEL_SHOWSTATES"].ToString());
            }
            set { Session["FILTERPANEL_SHOWSTATES"] = value.ToString(); }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Page.IsPostBack) return;

            LoadStaticReferences();


            /*
            cpeFilter.Collapsed = (keyword == null ||keyword.Equals(""))
                                    && (fromDate == null || fromDate.Equals("")) 
                                    && (toDate == null || toDate.Equals(""));

            if(ShowAreaCode)
            {
                if (ddlKHUVUC.SelectedIndex > 0)
                    cpeFilter.Collapsed = false;
            }

            if(ShowStates)
            {
                cpeFilter.Collapsed = false;
            }
            */

            txtKeyword.Text = keyword ?? "";            
            txtFromDate.Text = fromDate ?? "";
            txtToDate.Text = toDate ?? "";

            txtKeyword.Focus();
        }

        /*public void timkv()
        {
            var loginInfo = Session[SessionKey.USER_LOGIN] as UserAdmin;
            if (loginInfo == null) return;
            string b = loginInfo.Username;

            var query = _db.TIMKHUVUC(b);
            foreach (var a in query)
            {
                string d = a.MAKV;
                if (a.MAKV == "99")
                {
                    var list = kvDao.GetList();  
                    ddlKHUVUC.Items.Clear();
                    ddlKHUVUC.Items.Add(new ListItem("Tất cả", "%"));
                    foreach (var kv in list)
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
        }*/

        private void LoadStaticReferences()
        {
            var list = kvDao.GetList();            
            ddlKHUVUC.Items.Clear();
            ddlKHUVUC.Items.Add(new ListItem("--Tất cả--", "0"));
            foreach(var kv in list)
            {
                ddlKHUVUC.Items.Add(new ListItem(kv.TENKV, kv.MAKV));
            }
            //timkv();

            // hiển thị trạng thái đơn ở chế độ khảo sát
            if(ShowStates)
            {
                ddlTRANGTHAI.Items.Clear();

                if (TrangThai == TT.CHOKHAOSAT.ToString())
                {
                    ddlTRANGTHAI.Items.Add(new ListItem("Chờ xử lý", "NULL"));
                    ddlTRANGTHAI.Items.Add(new ListItem("Bị từ chối", TTTK.TK_RA.ToString()));
                }
                else if (TrangThai == TT.CHODUYETTHIETKE.ToString())
                {
                    ddlTRANGTHAI.Items.Add(new ListItem("Chờ duyệt", TTTK.TK_P.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Từ chối", TTTK.TK_RA.ToString()));
                }
                else if (TrangThai == TT.CHODUYETCHIETTINH.ToString())
                {
                    ddlTRANGTHAI.Items.Add(new ListItem("Chờ duyệt", TTCT.CT_P.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Từ chối", TTCT.CT_RA.ToString()));
                }
                else if(TrangThai == TT.TRACUUTHIETKE.ToString())
                {
                    ddlTRANGTHAI.Items.Add(new ListItem("--Tất cả--", "NULL"));
                    ddlTRANGTHAI.Items.Add(new ListItem("Thiết kế mới", TTTK.TK_N.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Đang xử lý", TTTK.TK_P.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Đã duyệt", TTTK.TK_A.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Bị từ chối", TTTK.TK_RA.ToString()));
                }
                else if (TrangThai == TT.TRACUUCHIETTINH.ToString())
                {
                    ddlTRANGTHAI.Items.Add(new ListItem("--Tất cả--", "NULL"));
                    ddlTRANGTHAI.Items.Add(new ListItem("Chiết tính mới", TTCT.CT_N.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Đang xử lý", TTCT.CT_P.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Đã duyệt", TTCT.CT_A.ToString()));
                    ddlTRANGTHAI.Items.Add(new ListItem("Bị từ chối", TTCT.CT_RA.ToString()));
                }
                else if (TrangThai == TT.XULYDONCHOHOPDONG.ToString())
                {
                    ddlTRANGTHAI.Items.Add(new ListItem("Chờ xử lý", "NULL"));
                    ddlTRANGTHAI.Items.Add(new ListItem("Bị từ chối", TTHD.HD_RA.ToString()));
                }

                if (stateCode != null && TrangThai != "")
                {
                    try
                    {
                        ddlTRANGTHAI.Text = stateCode;
                    }
                    catch
                    {
                        ddlTRANGTHAI.SelectedIndex = 0;
                    }
                }
            }
            
            if (ShowAreaCode)
            {

                if (areaCode != null)
                {
                    try
                    {
                        ddlKHUVUC.SelectedIndex = list.FindIndex(p => p.MAKV == areaCode) + 1;
                    }
                    catch
                    {
                        ddlKHUVUC.SelectedIndex = 0;
                    }
                }
            }

           
        }

        protected void btnFilter_Click(object sender, EventArgs e)
        {
            var url = string.Format("{0}?", Request.Url.AbsolutePath);
            var hash = new Hashtable();

            if(txtKeyword.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_KEYWORD, EncryptUtil.Encrypt(txtKeyword.Text.Trim()));
            }
            if(txtFromDate.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_FROMDATE, EncryptUtil.Encrypt(txtFromDate.Text.Trim()));
            }
            if (txtToDate.Text.Trim() != "")
            {
                hash.Add(Constants.PARAM_TODATE, EncryptUtil.Encrypt(txtToDate.Text.Trim()));
            }

            if(ShowAreaCode)
            {
                if(ddlKHUVUC.SelectedIndex > 0)
                {
                    hash.Add(Constants.PARAM_AREACODE, EncryptUtil.Encrypt(ddlKHUVUC.SelectedValue.Trim()));
                }
            } 
            
            if (ShowStates)
            {
                if (ddlTRANGTHAI.SelectedIndex > -1)
                {
                    hash.Add(Constants.PARAM_STATECODE, EncryptUtil.Encrypt(ddlTRANGTHAI.SelectedValue.Trim()));
                }
            }

            foreach (DictionaryEntry de in hash)
            {
                url += string.Format("{0}={1}&", de.Key, de.Value);
            }

            Response.Redirect(url.Remove(url.Length - 1), false);
        }
    }
}